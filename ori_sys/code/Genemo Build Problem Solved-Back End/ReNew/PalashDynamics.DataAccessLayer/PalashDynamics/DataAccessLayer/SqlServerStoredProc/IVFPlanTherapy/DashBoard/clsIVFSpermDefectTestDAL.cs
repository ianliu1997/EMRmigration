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

    internal class clsIVFSpermDefectTestDAL : clsBaseIVFSpermDefectTestDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsIVFSpermDefectTestDAL()
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

        public override IValueObject AddUpdateSpermDefectTest(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateIVFSpermDefectTestBizActionVO nvo = valueObject as clsAddUpdateIVFSpermDefectTestBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateSpermDefectTest");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.ClsIVFSpermDefectTest.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.ClsIVFSpermDefectTest.Date);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.ClsIVFSpermDefectTest.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitUnitID", DbType.Int64, nvo.ClsIVFSpermDefectTest.VisitUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AnnexinBinding", DbType.Double, nvo.ClsIVFSpermDefectTest.AnnexinBinding);
                this.dbServer.AddInParameter(storedProcCommand, "CaspaseActivity", DbType.Double, nvo.ClsIVFSpermDefectTest.CaspaseActivity);
                this.dbServer.AddInParameter(storedProcCommand, "AcrosinActivity", DbType.Double, nvo.ClsIVFSpermDefectTest.AcrosinActivity);
                this.dbServer.AddInParameter(storedProcCommand, "GlucosidaseActivity", DbType.Double, nvo.ClsIVFSpermDefectTest.GlucosidaseActivity);
                this.dbServer.AddInParameter(storedProcCommand, "AndrologistID", DbType.Int64, nvo.ClsIVFSpermDefectTest.AndrologistID);
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, nvo.ClsIVFSpermDefectTest.Remarks.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, nvo.ClsIVFSpermDefectTest.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.ClsIVFSpermDefectTest.ID);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.ClsIVFSpermDefectTest.ID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
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

        public override IValueObject GetSpermDefectTestList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIVFSpermDefectTestBizActionVO nvo = valueObject as clsGetIVFSpermDefectTestBizActionVO;
            nvo.ObjSpermDefectTestList = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSpermDefectTestList");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.ClsIVFSpermDefectTest.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.ClsIVFSpermDefectTest.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.ClsIVFSpermDefectTest.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ObjSpermDefectTestList == null)
                    {
                        nvo.ObjSpermDefectTestList = new List<clsIVFSpermDefectTestVO>();
                    }
                    while (reader.Read())
                    {
                        clsIVFSpermDefectTestVO item = new clsIVFSpermDefectTestVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]))),
                            VisitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitID"])),
                            VisitUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitUnitID"])),
                            AnnexinBinding = Convert.ToDouble(DALHelper.HandleDBNull(reader["AnnexinBinding"])),
                            CaspaseActivity = Convert.ToDouble(DALHelper.HandleDBNull(reader["CaspaseActivity"])),
                            AcrosinActivity = Convert.ToDouble(DALHelper.HandleDBNull(reader["AcrosinActivity"])),
                            GlucosidaseActivity = Convert.ToDouble(DALHelper.HandleDBNull(reader["GlucosidaseActivity"])),
                            AndrologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AndrologistID"])),
                            Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"])),
                            IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"])),
                            OPDNo = Convert.ToString(DALHelper.HandleDBNull(reader["OPDNO"]))
                        };
                        nvo.ObjSpermDefectTestList.Add(item);
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

