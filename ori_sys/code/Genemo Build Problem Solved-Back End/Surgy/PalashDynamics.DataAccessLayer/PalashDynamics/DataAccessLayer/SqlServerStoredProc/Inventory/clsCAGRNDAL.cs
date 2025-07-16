namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Inventory
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Inventory;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsCAGRNDAL : clsBaseCAGRNDAL
    {
        private Database dbServer;
        private LogManager logManager;

        public clsCAGRNDAL()
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

        public override IValueObject AddCAGRNItem(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddCAGRNItemBizActionVO nvo = valueObject as clsAddCAGRNItemBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddCAGRNDetail");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "GRNUnitID", DbType.Int64, nvo.GRNUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.StoreId);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierId", DbType.Int64, nvo.SupplierID);
                this.dbServer.AddInParameter(storedProcCommand, "IsFinalised", DbType.Boolean, nvo.Finalised);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceAgent", DbType.String, nvo.ServiceAgent);
                this.dbServer.AddInParameter(storedProcCommand, "ContractExpiryDate", DbType.DateTime, nvo.ContracExpirytDate);
                this.dbServer.AddInParameter(storedProcCommand, "SerialNo", DbType.String, nvo.SerialNumber);
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.StartDate);
                this.dbServer.AddInParameter(storedProcCommand, "EndDate", DbType.DateTime, nvo.EndDate);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddOutParameter(storedProcCommand, "Id", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.ID = (int) this.dbServer.GetParameterValue(storedProcCommand, "Id");
                if (nvo.SuccessStatus == 0)
                {
                    foreach (clsCAGRNDetailsVO svo in nvo.Details.ItemsCAGRN)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddCAGRNDetailItem");
                        command2.Connection = connection;
                        this.dbServer.AddInParameter(command2, "ContractManagementId", DbType.Int64, nvo.ID);
                        this.dbServer.AddInParameter(command2, "GRNUnitID", DbType.Int64, svo.GRNUnitID);
                        this.dbServer.AddInParameter(command2, "ItemID", DbType.Int64, svo.ItemID);
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, nvo.UnitId);
                        this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command2);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection = null;
            }
            return nvo;
        }

        public override IValueObject DeleteCAItemById(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteCAByIDBizActionVO nvo = valueObject as clsDeleteCAByIDBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteCAItemDetail");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ContractManagementId", DbType.Int64, nvo.ContractManagementId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection = null;
            }
            return nvo;
        }

        public override IValueObject GetCAGRNItemDetailList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCAGRNItemDetailListBizActionVO nvo = valueObject as clsGetCAGRNItemDetailListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCAGRNItemDetail");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "GRNID", DbType.Int64, nvo.GRNID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsCAGRNDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsCAGRNDetailsVO item = new clsCAGRNDetailsVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            ItemID = (long) DALHelper.HandleDBNull(reader["ItemID"]),
                            UnitId = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            GRNUnitID = (long) DALHelper.HandleDBNull(reader["GRNUnitID"]),
                            ItemName = (string) DALHelper.HandleDBNull(reader["ItemName"]),
                            ItemCode = (string) DALHelper.HandleDBNull(reader["ItemCode"])
                        };
                        nvo.List.Add(item);
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

        public override IValueObject GetCAGRNSearchList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCAGRNBizActionVO nvo = valueObject as clsCAGRNBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetGRNListForCA");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierID", DbType.Int64, nvo.SupplierID);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "GRNNO", DbType.String, nvo.GRNNO);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, nvo.Freezed);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsCAGRNVO>();
                    }
                    while (reader.Read())
                    {
                        clsCAGRNVO item = new clsCAGRNVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            StoreID = (long) DALHelper.HandleDBNull(reader["StoreID"]),
                            UnitId = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            SupplierID = (long) DALHelper.HandleDBNull(reader["SupplierID"]),
                            GRNNO = (string) DALHelper.HandleDBNull(reader["GRNNO"]),
                            GRNType = (InventoryGRNType) ((short) DALHelper.HandleDBNull(reader["GRNType"]))
                        };
                        DateTime? nullable = DALHelper.HandleDate(reader["Date"]);
                        item.Date = nullable.Value;
                        item.Freezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Freezed"]));
                        nvo.List.Add(item);
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

        public override IValueObject GetCAItemDetailList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCAItemDetailListBizActionVO nvo = valueObject as clsGetCAItemDetailListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCAItemDetail");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ContractManagementId", DbType.Int64, nvo.ContractManagementId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsCAGRNDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsCAGRNDetailsVO item = new clsCAGRNDetailsVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            ItemID = (long) DALHelper.HandleDBNull(reader["ItemID"]),
                            UnitId = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            GRNUnitID = (long) DALHelper.HandleDBNull(reader["GRNUnitID"]),
                            ItemName = (string) DALHelper.HandleDBNull(reader["ItemName"]),
                            ItemCode = (string) DALHelper.HandleDBNull(reader["ItemCode"])
                        };
                        nvo.List.Add(item);
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

        public override IValueObject GetCAItemDetailListById(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCADetailsListByIDBizActionVO nvo = valueObject as clsGetCADetailsListByIDBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCAItemDetail");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ContractManagementId", DbType.Int64, nvo.ContractManagementId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsCAGRNDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsCAGRNDetailsVO item = new clsCAGRNDetailsVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            ItemID = (long) DALHelper.HandleDBNull(reader["ItemID"]),
                            UnitId = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            GRNUnitID = (long) DALHelper.HandleDBNull(reader["GRNUnitID"]),
                            ItemName = (string) DALHelper.HandleDBNull(reader["ItemName"]),
                            ItemCode = (string) DALHelper.HandleDBNull(reader["ItemCode"])
                        };
                        nvo.List.Add(item);
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

        public override IValueObject GetCAList(IValueObject valueObject, clsUserVO UserVo)
        {
            DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCAListForSearch");
            clsGetCAListBizActionVO nvo = valueObject as clsGetCAListBizActionVO;
            try
            {
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierID", DbType.Int64, nvo.SupplierID);
                if (nvo.FromDate != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                }
                if (nvo.ToDate != null)
                {
                    nvo.ToDate = new DateTime?(nvo.ToDate.Value.Date.AddDays(1.0));
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsCAGRNVO>();
                    }
                    while (reader.Read())
                    {
                        clsCAGRNVO item = new clsCAGRNVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            SupplierName = (string) DALHelper.HandleDBNull(reader["SupplierName"]),
                            ServiceAgent = (string) DALHelper.HandleDBNull(reader["ServiceAgent"])
                        };
                        DateTime? nullable4 = DALHelper.HandleDate(reader["ContractExpiryDate"]);
                        item.ContractExpiryDate = new DateTime?(nullable4.Value);
                        DateTime? nullable5 = DALHelper.HandleDate(reader["FromDate"]);
                        item.FromDate = new DateTime?(nullable5.Value);
                        DateTime? nullable6 = DALHelper.HandleDate(reader["EndDate"]);
                        item.EndDate = new DateTime?(nullable6.Value);
                        item.SerialNo = (string) DALHelper.HandleDBNull(reader["SerialNo"]);
                        item.SupplierID = (long) DALHelper.HandleDBNull(reader["SupplierId"]);
                        item.StoreID = (long) DALHelper.HandleDBNull(reader["StoreId"]);
                        item.UnitId = (long) DALHelper.HandleDBNull(reader["UnitId"]);
                        item.Finalized = (bool) DALHelper.HandleBoolDBNull(reader["IsFinalised"]);
                        nvo.List.Add(item);
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

        public override IValueObject UpdateCAGRNItem(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateCAGRNItemBizActionVO nvo = valueObject as clsUpdateCAGRNItemBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateCAGRNDetail");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "GRNUnitID", DbType.Int64, nvo.GRNUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.StoreId);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierId", DbType.Int64, nvo.SupplierID);
                this.dbServer.AddInParameter(storedProcCommand, "IsFinalised", DbType.Boolean, nvo.Finalised);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceAgent", DbType.String, nvo.ServiceAgent);
                this.dbServer.AddInParameter(storedProcCommand, "ContractExpiryDate", DbType.DateTime, nvo.ContracExpirytDate);
                this.dbServer.AddInParameter(storedProcCommand, "SerialNo", DbType.String, nvo.SerialNumber);
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.StartDate);
                this.dbServer.AddInParameter(storedProcCommand, "EndDate", DbType.DateTime, nvo.EndDate);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if (nvo.SuccessStatus == 0)
                {
                    foreach (clsCAGRNDetailsVO svo in nvo.Details.ItemsCAGRN)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddCAGRNDetailItem");
                        command2.Connection = connection;
                        this.dbServer.AddInParameter(command2, "ContractManagementId", DbType.Int64, nvo.ID);
                        this.dbServer.AddInParameter(command2, "GRNUnitID", DbType.Int64, svo.GRNUnitID);
                        this.dbServer.AddInParameter(command2, "ItemID", DbType.Int64, svo.ItemID);
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, nvo.UnitId);
                        this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command2);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection = null;
            }
            return nvo;
        }
    }
}

