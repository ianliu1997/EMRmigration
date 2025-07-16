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

    internal class clsIVFEmbryoTransferDAL : clsBaseIVFEmbryoTransferDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsIVFEmbryoTransferDAL()
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

        public override IValueObject AddForwardedEmbryos(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            clsAddForwardedEmbryoTransferBizActionVO bizActionObj = valueObject as clsAddForwardedEmbryoTransferBizActionVO;
            bizActionObj = this.AddUpdateDetails(bizActionObj, UserVo, pConnection, pTransaction);
            return valueObject;
        }

        private clsAddForwardedEmbryoTransferBizActionVO AddUpdateDetails(clsAddForwardedEmbryoTransferBizActionVO BizActionObj, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                connection = (pConnection == null) ? this.dbServer.CreateConnection() : pConnection;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = (pTransaction == null) ? connection.BeginTransaction() : pTransaction;
                if ((BizActionObj.ForwardedEmbryos != null) && (BizActionObj.ForwardedEmbryos.Count > 0))
                {
                    for (int i = 0; i < BizActionObj.ForwardedEmbryos.Count; i++)
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddEmbryoTransferDetails");
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, BizActionObj.UnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "CoupleId", DbType.Int64, BizActionObj.CoupleID);
                        this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitId", DbType.Int64, BizActionObj.CoupleUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, BizActionObj.ForwardedEmbryos[i].TransferDate);
                        this.dbServer.AddInParameter(storedProcCommand, "FormID", DbType.Int64, BizActionObj.ForwardedEmbryos[i].TransferDay);
                        this.dbServer.AddInParameter(storedProcCommand, "FormRecID", DbType.Int64, BizActionObj.ForwardedEmbryos[i].RecID);
                        this.dbServer.AddInParameter(storedProcCommand, "EmbNO", DbType.Int64, BizActionObj.ForwardedEmbryos[i].EmbryoNumber);
                        this.dbServer.AddInParameter(storedProcCommand, "GradeID", DbType.Int64, BizActionObj.ForwardedEmbryos[i].GradeID);
                        this.dbServer.AddInParameter(storedProcCommand, "Score", DbType.Int32, BizActionObj.ForwardedEmbryos[i].Score);
                        this.dbServer.AddInParameter(storedProcCommand, "FertilizationStageID", DbType.Int64, BizActionObj.ForwardedEmbryos[i].FertilizationStageID);
                        this.dbServer.AddInParameter(storedProcCommand, "EmbStatus", DbType.String, BizActionObj.ForwardedEmbryos[i].EmbryoStatus);
                        this.dbServer.AddInParameter(storedProcCommand, "IsUsed", DbType.Boolean, false);
                        this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.ForwardedEmbryos[i].ID);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                        BizActionObj.ForwardedEmbryos[i].ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                        if (BizActionObj.SuccessStatus == -1)
                        {
                            throw new Exception();
                        }
                        BizActionObj.SuccessStatus = 0;
                        if (pConnection == null)
                        {
                            transaction.Commit();
                        }
                    }
                }
            }
            catch (Exception exception1)
            {
                string message = exception1.Message;
                BizActionObj.SuccessStatus = -1;
                if (pConnection == null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (pConnection == null)
                {
                    connection.Close();
                    connection = null;
                    transaction = null;
                }
            }
            return BizActionObj;
        }

        public override IValueObject AddUpdateEmbryoTransfer(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection pConnection = null;
            clsAddUpdateEmbryoTransferBizActionVO nvo = valueObject as clsAddUpdateEmbryoTransferBizActionVO;
            try
            {
                DbCommand storedProcCommand;
                pConnection = this.dbServer.CreateConnection();
                pConnection.Open();
                transaction = pConnection.BeginTransaction();
                if (!nvo.IsUpdate)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddEmbryoTransfer");
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.EmbryoTransfer.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_UpdateEmbryoTransfer");
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.EmbryoTransfer.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleId", DbType.Int64, nvo.EmbryoTransfer.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitId", DbType.Int64, nvo.EmbryoTransfer.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.EmbryoTransfer.ProcDate);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, nvo.EmbryoTransfer.ProcTime);
                this.dbServer.AddInParameter(storedProcCommand, "EmbryologistID", DbType.Int64, nvo.EmbryoTransfer.EmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssEmbryologistID", DbType.Int64, nvo.EmbryoTransfer.AssEmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AnasthesistID", DbType.Int64, nvo.EmbryoTransfer.AnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssAnasthesistID", DbType.Int64, nvo.EmbryoTransfer.AssAnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "IsTreatmentUnderGA", DbType.Boolean, nvo.EmbryoTransfer.IsTreatmentUnderGA);
                this.dbServer.AddInParameter(storedProcCommand, "CatheterTypeID", DbType.Int64, nvo.EmbryoTransfer.CatheterTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "Difficulty", DbType.Boolean, nvo.EmbryoTransfer.IsDifficult);
                this.dbServer.AddInParameter(storedProcCommand, "DifficultyTypeID", DbType.Int64, nvo.EmbryoTransfer.DifficultyTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, nvo.EmbryoTransfer.IsFreezed);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if (nvo.SuccessStatus == -1)
                {
                    throw new Exception();
                }
                if (!nvo.IsUpdate)
                {
                    nvo.EmbryoTransfer.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                }
                else
                {
                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Delete from T_IVF_EmbryoTransferDetails where UnitID=", nvo.EmbryoTransfer.UnitID, " AND ETID =", nvo.EmbryoTransfer.ID }));
                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                    sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Delete from T_IVF_LabDayUploadedFiles where UnitID=", nvo.EmbryoTransfer.UnitID, " AND OocyteID =", nvo.EmbryoTransfer.ID, " AND LabDay=", IVFLabDay.EmbryoTransfer }));
                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                }
                if ((nvo.EmbryoTransfer.FUSetting != null) && (nvo.EmbryoTransfer.FUSetting.Count > 0))
                {
                    for (int i = 0; i < nvo.EmbryoTransfer.FUSetting.Count; i++)
                    {
                        storedProcCommand = null;
                       storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayUploadedFiles");
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "OocyteID", DbType.Int64, nvo.EmbryoTransfer.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "LabDay", DbType.Int16, IVFLabDay.EmbryoTransfer);
                        this.dbServer.AddInParameter(storedProcCommand, "FileName", DbType.String, nvo.EmbryoTransfer.FUSetting[i].FileName);
                        this.dbServer.AddInParameter(storedProcCommand, "FileIndex", DbType.Int32, nvo.EmbryoTransfer.FUSetting[i].Index);
                        this.dbServer.AddInParameter(storedProcCommand, "Value", DbType.Binary, nvo.EmbryoTransfer.FUSetting[i].Data);
                        this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                        this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.EmbryoTransfer.FUSetting[i].ID);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                        nvo.EmbryoTransfer.FUSetting[i].ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                        if (nvo.SuccessStatus == -1)
                        {
                            throw new Exception();
                        }
                    }
                }
                if ((nvo.EmbryoTransfer.Details != null) && (nvo.EmbryoTransfer.Details.Count > 0))
                {
                    for (int i = 0; i < nvo.EmbryoTransfer.Details.Count; i++)
                    {
                        storedProcCommand = null;
                        storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddEmbryoTransferDetails");
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "TransferDate", DbType.DateTime, nvo.EmbryoTransfer.Details[i].TransferDate);
                        this.dbServer.AddInParameter(storedProcCommand, "TransferDay", DbType.String, nvo.EmbryoTransfer.Details[i].TransferDay);
                        this.dbServer.AddInParameter(storedProcCommand, "ETID", DbType.Int64, nvo.EmbryoTransfer.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "EmbNO", DbType.Int64, nvo.EmbryoTransfer.Details[i].EmbryoNumber);
                        this.dbServer.AddInParameter(storedProcCommand, "Grade", DbType.String, nvo.EmbryoTransfer.Details[i].Grade);
                        this.dbServer.AddInParameter(storedProcCommand, "Score", DbType.Int32, nvo.EmbryoTransfer.Details[i].Score);
                        this.dbServer.AddInParameter(storedProcCommand, "FertilizationStage", DbType.String, nvo.EmbryoTransfer.Details[i].FertilizationStage);
                        this.dbServer.AddInParameter(storedProcCommand, "EmbStatus", DbType.String, nvo.EmbryoTransfer.Details[i].EmbryoStatus);
                        this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.EmbryoTransfer.Details[i].ID);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                        nvo.EmbryoTransfer.Details[i].ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                        if (nvo.SuccessStatus == -1)
                        {
                            throw new Exception();
                        }
                    }
                }
                if (nvo.EmbryoTransfer.LabDaySummary != null)
                {
                    clsBaseIVFLabDaysSummaryDAL instance = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                    clsAddUpdateLabDaysSummaryBizActionVO nvo2 = new clsAddUpdateLabDaysSummaryBizActionVO {
                        LabDaysSummary = nvo.EmbryoTransfer.LabDaySummary
                    };
                    nvo2.LabDaysSummary.OocyteID = nvo.EmbryoTransfer.ID;
                    nvo2.LabDaysSummary.CoupleID = nvo.EmbryoTransfer.CoupleID;
                    nvo2.LabDaysSummary.CoupleUnitID = nvo.EmbryoTransfer.CoupleUnitID;
                    nvo2.LabDaysSummary.FormID = IVFLabWorkForm.EmbryoTransfer;
                    nvo2.LabDaysSummary.Priority = 1;
                    nvo2.LabDaysSummary.ProcDate = nvo.EmbryoTransfer.ProcDate;
                    nvo2.LabDaysSummary.ProcTime = nvo.EmbryoTransfer.ProcTime;
                    nvo2.LabDaysSummary.UnitID = nvo.EmbryoTransfer.UnitID;
                    nvo2.IsUpdate = nvo.IsUpdate;
                    nvo2 = (clsAddUpdateLabDaysSummaryBizActionVO) instance.AddUpdateLabDaysSummary(nvo2, UserVo, pConnection, transaction);
                    if (nvo2.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    nvo.EmbryoTransfer.LabDaySummary.ID = nvo2.LabDaysSummary.ID;
                }
                transaction.Commit();
                nvo.SuccessStatus = 0;
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                transaction.Rollback();
                nvo.EmbryoTransfer = null;
            }
            finally
            {
                pConnection.Close();
                transaction = null;
                pConnection = null;
            }
            return nvo;
        }

        public override IValueObject GetEmbryoTransfer(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetEmbryoTransferBizActionVO nvo = valueObject as clsGetEmbryoTransferBizActionVO;
            try
            {
                if (nvo.EmbryoTransfer == null)
                {
                    nvo.EmbryoTransfer = new clsEmbryoTransferVO();
                }
                if (nvo.EmbryoTransfer.Details == null)
                {
                    nvo.EmbryoTransfer.Details = new List<clsEmbryoTransferDetailsVO>();
                }
                if (nvo.EmbryoTransfer.FUSetting == null)
                {
                    nvo.EmbryoTransfer.FUSetting = new List<FileUpload>();
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_GetEmbryoTransfer");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, nvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, nvo.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "LabDay", DbType.Int16, IVFLabDay.EmbryoTransfer);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.EmbryoTransfer.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.EmbryoTransfer.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.EmbryoTransfer.CoupleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"]));
                        nvo.EmbryoTransfer.CoupleUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleUnitID"]));
                        nvo.EmbryoTransfer.ProcDate = DALHelper.HandleDate(reader["Date"]);
                        nvo.EmbryoTransfer.ProcTime = DALHelper.HandleDate(reader["Time"]);
                        nvo.EmbryoTransfer.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        nvo.EmbryoTransfer.AssEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssEmbryologistID"]));
                        nvo.EmbryoTransfer.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnasthesistID"]));
                        nvo.EmbryoTransfer.AssAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssAnasthesistID"]));
                        nvo.EmbryoTransfer.IsTreatmentUnderGA = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsTreatmentUnderGA"]));
                        nvo.EmbryoTransfer.CatheterTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CatheterTypeID"]));
                        nvo.EmbryoTransfer.IsDifficult = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Difficulty"]));
                        nvo.EmbryoTransfer.DifficultyTypeID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["DifficultyTypeID"])));
                        nvo.EmbryoTransfer.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        nvo.EmbryoTransfer.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsEmbryoTransferDetailsVO item = new clsEmbryoTransferDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            EmbryoNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbNO"])),
                            TransferDate = (DateTime?) DALHelper.HandleDBNull(reader["TransferDate"]),
                            Day = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"])),
                            Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"])),
                            Score = (int) DALHelper.HandleDBNull(reader["Score"]),
                            FertilizationStage = (string) DALHelper.HandleDBNull(reader["FertilizationStage"]),
                            EmbryoStatus = (string) DALHelper.HandleDBNull(reader["EmbStatus"])
                        };
                        nvo.EmbryoTransfer.Details.Add(item);
                    }
                }
                reader.NextResult();
                int num = 0;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        FileUpload item = new FileUpload {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Index = num,
                            FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                            Data = (byte[]) DALHelper.HandleDBNull(reader["Value"])
                        };
                        nvo.EmbryoTransfer.FUSetting.Add(item);
                        num++;
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                nvo.EmbryoTransfer = null;
                throw;
            }
            return nvo;
        }

        public override IValueObject GetForwardedEmbryos(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetForwardedEmbryoTransferBizActionVO nvo = valueObject as clsGetForwardedEmbryoTransferBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_GetForwardedEmbryos");
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, nvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, nvo.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                List<long> list = new List<long>();
                if (reader.HasRows)
                {
                    if (nvo.EmbryoTransfer == null)
                    {
                        nvo.EmbryoTransfer = new List<clsEmbryoTransferDetailsVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            if (nvo.EmbryoTransfer.Count > 1)
                            {
                                nvo.EmbryoTransfer.Reverse();
                            }
                            break;
                        }
                        if (!list.Contains((long) DALHelper.HandleDBNull(reader["EmbNO"])))
                        {
                            clsEmbryoTransferDetailsVO item = new clsEmbryoTransferDetailsVO {
                                ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                                UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                                TransferDate = DALHelper.HandleDate(reader["Date"]),
                                TransferDay = this.GetIVFenumValue(Convert.ToInt16(DALHelper.HandleDBNull(reader["FormID"])))
                            };
                            item.Day = item.TransferDay.ToString();
                            item.EmbryoNumber = (long) DALHelper.HandleDBNull(reader["EmbNO"]);
                            item.GradeID = (long) DALHelper.HandleDBNull(reader["GradeID"]);
                            item.FertilizationStageID = (long) DALHelper.HandleDBNull(reader["FertilizationStageID"]);
                            item.Grade = (string) DALHelper.HandleDBNull(reader["Grade"]);
                            item.Score = (int) DALHelper.HandleDBNull(reader["Score"]);
                            item.FertilizationStage = (string) DALHelper.HandleDBNull(reader["FertilizationStage"]);
                            nvo.EmbryoTransfer.Add(item);
                            list.Add(item.EmbryoNumber);
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

        public IVFLabDay GetIVFenumValue(short ob)
        {
            IVFLabDay day = IVFLabDay.Day0;
            switch (ob)
            {
                case 0:
                    day = IVFLabDay.Day0;
                    break;

                case 1:
                    day = IVFLabDay.Day1;
                    break;

                case 2:
                    day = IVFLabDay.Day2;
                    break;

                case 3:
                    day = IVFLabDay.Day3;
                    break;

                case 4:
                    day = IVFLabDay.Day4;
                    break;

                case 5:
                    day = IVFLabDay.Day5;
                    break;

                case 6:
                    day = IVFLabDay.Day6;
                    break;

                default:
                    break;
            }
            return day;
        }
    }
}

