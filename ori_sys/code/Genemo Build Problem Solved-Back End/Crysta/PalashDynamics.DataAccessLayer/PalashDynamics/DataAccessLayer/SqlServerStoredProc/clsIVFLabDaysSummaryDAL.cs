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

    internal class clsIVFLabDaysSummaryDAL : clsBaseIVFLabDaysSummaryDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsIVFLabDaysSummaryDAL()
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

        private clsAddUpdateLabDaysSummaryBizActionVO AddUpdateDetails(clsAddUpdateLabDaysSummaryBizActionVO BizActionObj, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                DbCommand storedProcCommand;
                connection = (pConnection == null) ? this.dbServer.CreateConnection() : pConnection;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = (pTransaction == null) ? connection.BeginTransaction() : pTransaction;
                if (!BizActionObj.IsUpdate)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDaySummary");
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.LabDaysSummary.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_UpdateLabDaysSummary");
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, BizActionObj.LabDaysSummary.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, BizActionObj.LabDaysSummary.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, BizActionObj.LabDaysSummary.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, BizActionObj.LabDaysSummary.ProcDate);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, BizActionObj.LabDaysSummary.ProcTime);
                this.dbServer.AddInParameter(storedProcCommand, "FormID", DbType.Int16, BizActionObj.LabDaysSummary.FormID);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteID", DbType.Int64, BizActionObj.LabDaysSummary.OocyteID);
                this.dbServer.AddInParameter(storedProcCommand, "Impressions", DbType.String, BizActionObj.LabDaysSummary.Impressions);
                this.dbServer.AddInParameter(storedProcCommand, "Priority", DbType.Int32, BizActionObj.LabDaysSummary.Priority);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, BizActionObj.LabDaysSummary.IsFreezed);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if (!BizActionObj.IsUpdate)
                {
                    BizActionObj.LabDaysSummary.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                }
                BizActionObj.SuccessStatus = 0;
                if (pConnection == null)
                {
                    transaction.Commit();
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

        public override IValueObject AddUpdateLabDaysSummary(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateLabDaysSummaryBizActionVO bizActionObj = valueObject as clsAddUpdateLabDaysSummaryBizActionVO;
            bizActionObj = this.AddUpdateDetails(bizActionObj, UserVo, null, null);
            return valueObject;
        }

        public override IValueObject AddUpdateLabDaysSummary(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            clsAddUpdateLabDaysSummaryBizActionVO bizActionObj = valueObject as clsAddUpdateLabDaysSummaryBizActionVO;
            bizActionObj = this.AddUpdateDetails(bizActionObj, UserVo, pConnection, pTransaction);
            return valueObject;
        }

        public override IValueObject GetArtCycleSummary(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetArtCycleSummaryBizActionVO nvo = valueObject as clsGetArtCycleSummaryBizActionVO;
            try
            {
                if (nvo.ArtCycleSummary == null)
                {
                    nvo.ArtCycleSummary = new List<clsArtCycleSummaryVO>();
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_LabDaySummary");
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, nvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, nvo.CoupleUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsArtCycleSummaryVO item = new clsArtCycleSummaryVO {
                            PlanTherapyId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyId"])),
                            TherapyStartDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["TherapyStartDate"]))),
                            Oocytes = Convert.ToString(DALHelper.HandleDBNull(reader["Oocytes"])),
                            Treatment = Convert.ToString(DALHelper.HandleDBNull(reader["Treatment"])),
                            PronucleusStages = Convert.ToString(DALHelper.HandleDBNull(reader["PronucleusStages"])),
                            Embryos = Convert.ToString(DALHelper.HandleDBNull(reader["Embryos"]))
                        };
                        nvo.ArtCycleSummary.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                nvo.ArtCycleSummary = null;
                throw;
            }
            return nvo;
        }

        public IVFLabWorkForm GetIVFenumValue(short ob)
        {
            IVFLabWorkForm embryoTransfer = IVFLabWorkForm.FemaleLabDay0;
            embryoTransfer = (IVFLabWorkForm) ob;
            switch (ob)
            {
                case 0:
                    embryoTransfer = IVFLabWorkForm.FemaleLabDay0;
                    break;

                case 1:
                    embryoTransfer = IVFLabWorkForm.FemaleLabDay1;
                    break;

                case 2:
                    embryoTransfer = IVFLabWorkForm.FemaleLabDay2;
                    break;

                case 3:
                    embryoTransfer = IVFLabWorkForm.FemaleLabDay3;
                    break;

                case 4:
                    embryoTransfer = IVFLabWorkForm.FemaleLabDay4;
                    break;

                case 5:
                    embryoTransfer = IVFLabWorkForm.FemaleLabDay5;
                    break;

                case 6:
                    embryoTransfer = IVFLabWorkForm.FemaleLabDay6;
                    break;

                case 7:
                    embryoTransfer = IVFLabWorkForm.EmbryoTransfer;
                    break;

                case 8:
                    embryoTransfer = IVFLabWorkForm.Vitrification;
                    break;

                case 9:
                    embryoTransfer = IVFLabWorkForm.Thawing;
                    break;

                case 10:
                    embryoTransfer = IVFLabWorkForm.MediaCosting;
                    break;

                default:
                    break;
            }
            return embryoTransfer;
        }

        public override IValueObject GetLabDaysSummary(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetLabDaysSummaryBizActionVO nvo = valueObject as clsGetLabDaysSummaryBizActionVO;
            try
            {
                if (nvo.LabDaysSummary == null)
                {
                    nvo.LabDaysSummary = new List<clsLabDaysSummaryVO>();
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_GetLabDaysSummary");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, nvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, nvo.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsLabDaysSummaryVO item = new clsLabDaysSummaryVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"])),
                            CoupleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"])),
                            CoupleUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleUnitID"])),
                            ProcDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]))),
                            ProcTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Time"])))
                        };
                        int num = Convert.ToInt16(DALHelper.HandleDBNull(reader["FormID"]));
                        item.FormID = (IVFLabWorkForm) num;
                        item.FormName = item.FormID.ToString();
                        item.OocyteID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteID"]));
                        item.Impressions = Convert.ToString(DALHelper.HandleDBNull(reader["Impressions"]));
                        item.Priority = Convert.ToInt32(DALHelper.HandleDBNull(reader["Priority"]));
                        item.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        item.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        nvo.LabDaysSummary.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                reader.Close();
            }
            catch (Exception)
            {
                nvo.LabDaysSummary = null;
                throw;
            }
            return nvo;
        }
    }
}

