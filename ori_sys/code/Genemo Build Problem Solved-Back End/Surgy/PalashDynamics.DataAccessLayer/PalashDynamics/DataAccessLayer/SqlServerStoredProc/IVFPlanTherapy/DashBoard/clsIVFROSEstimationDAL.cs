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

    internal class clsIVFROSEstimationDAL : clsBaseIVFROSEstimationDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsIVFROSEstimationDAL()
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

        public override IValueObject AddUpdateROSEstimation(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateIVFROSEstimationBizActionVO nvo = valueObject as clsAddUpdateIVFROSEstimationBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateROSEstimation");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.clsIVFROSEstimation.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.clsIVFROSEstimation.Date);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.clsIVFROSEstimation.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitUnitID", DbType.Int64, nvo.clsIVFROSEstimation.VisitUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ROSLevel", DbType.Double, nvo.clsIVFROSEstimation.ROSLevel);
                this.dbServer.AddInParameter(storedProcCommand, "TotalAntioxidantCapacity", DbType.Double, nvo.clsIVFROSEstimation.TotalAntioxidantCapacity);
                this.dbServer.AddInParameter(storedProcCommand, "ROSTACScore", DbType.Double, nvo.clsIVFROSEstimation.ROSTACScore);
                this.dbServer.AddInParameter(storedProcCommand, "AndrologistID", DbType.Int64, nvo.clsIVFROSEstimation.AndrologistID);
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, nvo.clsIVFROSEstimation.Remarks.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, nvo.clsIVFROSEstimation.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.clsIVFROSEstimation.ID);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.clsIVFROSEstimation.ID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
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

        public override IValueObject GetROSEstimationList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIVFROSEstimationBizActionVO nvo = valueObject as clsGetIVFROSEstimationBizActionVO;
            nvo.ObjROSEstimationList = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetROSEstimationList");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.clsIVFROSEstimation.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.clsIVFROSEstimation.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.clsIVFROSEstimation.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ObjROSEstimationList == null)
                    {
                        nvo.ObjROSEstimationList = new List<clsIVFROSEstimationVO>();
                    }
                    while (reader.Read())
                    {
                        clsIVFROSEstimationVO item = new clsIVFROSEstimationVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]))),
                            VisitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitID"])),
                            VisitUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitUnitID"])),
                            ROSLevel = Convert.ToDouble(DALHelper.HandleDBNull(reader["ROSLevel"])),
                            TotalAntioxidantCapacity = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAntioxidantCapacity"])),
                            ROSTACScore = Convert.ToDouble(DALHelper.HandleDBNull(reader["ROSTACScore"])),
                            AndrologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AndrologistID"])),
                            Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"])),
                            IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"])),
                            OPDNo = Convert.ToString(DALHelper.HandleDBNull(reader["OPDNO"]))
                        };
                        nvo.ObjROSEstimationList.Add(item);
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
            return valueObject;
        }
    }
}

