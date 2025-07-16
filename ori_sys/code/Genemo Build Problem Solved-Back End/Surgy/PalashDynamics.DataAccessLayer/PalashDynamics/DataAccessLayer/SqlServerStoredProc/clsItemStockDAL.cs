namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
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

    public class clsItemStockDAL : clsBaseItemStockDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsItemStockDAL()
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

        public override IValueObject Add(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddItemStockBizActionVO bizActionObj = valueObject as clsAddItemStockBizActionVO;
            if (bizActionObj.Details.ID == 0L)
            {
                bizActionObj = this.AddDetails(bizActionObj, UserVo, null, null);
            }
            return valueObject;
        }

        public override IValueObject Add(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {
            clsAddItemStockBizActionVO bizActionObj = valueObject as clsAddItemStockBizActionVO;
            if (bizActionObj.Details.ID == 0L)
            {
                bizActionObj = this.AddDetails(bizActionObj, UserVo, myConnection, myTransaction);
            }
            return valueObject;
        }

        private clsAddItemStockBizActionVO AddDetails(clsAddItemStockBizActionVO BizActionObj, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                connection = (myConnection == null) ? this.dbServer.CreateConnection() : myConnection;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = (myTransaction == null) ? connection.BeginTransaction() : myTransaction;
                clsItemStockVO details = BizActionObj.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddItemStock");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, details.LinkServer);
                if (details.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, details.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, details.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, details.Date);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, details.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, details.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, details.ItemID);
                this.dbServer.AddInParameter(storedProcCommand, "BatchID", DbType.Int64, details.BatchID);
                if ((details.BatchCode != null) && (details.BatchCode.Trim().Length > 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "BatchCode", DbType.String, details.BatchCode);
                    this.dbServer.AddInParameter(storedProcCommand, "ExpiryDateNew", DbType.DateTime, details.ExpiryDate);
                }
                this.dbServer.AddInParameter(storedProcCommand, "TransactionTypeID", DbType.Int16, (short) details.TransactionTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "TransactionID", DbType.Int64, details.TransactionID);
                this.dbServer.AddInParameter(storedProcCommand, "OperationType", DbType.Int16, (short) details.OperationType);
                this.dbServer.AddInParameter(storedProcCommand, "TransactionQuantity", DbType.Double, details.TransactionQuantity);
                this.dbServer.AddInParameter(storedProcCommand, "InputTransactionQuantity", DbType.Double, details.InputTransactionQuantity);
                this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, details.TransactionTypeID.ToString());
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, details.Status);
                this.dbServer.AddInParameter(storedProcCommand, "BaseUOMID", DbType.Int64, details.BaseUOMID);
                this.dbServer.AddInParameter(storedProcCommand, "BaseCF", DbType.Double, details.BaseConversionFactor);
                this.dbServer.AddInParameter(storedProcCommand, "StockUOMID", DbType.Int64, details.SUOMID);
                this.dbServer.AddInParameter(storedProcCommand, "StockCF", DbType.Double, details.ConversionFactor);
                this.dbServer.AddInParameter(storedProcCommand, "TransactionUOMID", DbType.Int64, details.SelectedUOM.ID);
                this.dbServer.AddInParameter(storedProcCommand, "StockingQuantity", DbType.Double, details.StockingQuantity);
                this.dbServer.AddInParameter(storedProcCommand, "PurchaseRate1", DbType.Single, details.PurchaseRate);
                this.dbServer.AddInParameter(storedProcCommand, "MRP1", DbType.Single, details.MRP);
                this.dbServer.AddInParameter(storedProcCommand, "IsFromOpeningBalance", DbType.Boolean, details.IsFromOpeningBalance);
                if (details.UnitId > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, details.UnitId);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                }
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, details.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "BarCode", DbType.String, details.BarCode);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if (BizActionObj.SuccessStatus == -2)
                {
                    throw new Exception();
                }
                if (myConnection == null)
                {
                    transaction.Commit();
                }
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                if (myConnection == null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (myConnection == null)
                {
                    connection.Close();
                }
            }
            return BizActionObj;
        }

        private clsAddItemStockBizActionVO AddDetailsFree(clsAddItemStockBizActionVO BizActionObj, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                connection = (myConnection == null) ? this.dbServer.CreateConnection() : myConnection;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = (myTransaction == null) ? connection.BeginTransaction() : myTransaction;
                clsItemStockVO details = BizActionObj.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddItemStockFree");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, details.LinkServer);
                if (details.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, details.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, details.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, details.Date);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, details.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, details.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, details.ItemID);
                this.dbServer.AddInParameter(storedProcCommand, "BatchID", DbType.Int64, details.BatchID);
                if ((details.BatchCode != null) && (details.BatchCode.Trim().Length > 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "BatchCode", DbType.String, details.BatchCode);
                    this.dbServer.AddInParameter(storedProcCommand, "ExpiryDateNew", DbType.DateTime, details.ExpiryDate);
                }
                this.dbServer.AddInParameter(storedProcCommand, "TransactionTypeID", DbType.Int16, (short) details.TransactionTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "TransactionID", DbType.Int64, details.TransactionID);
                this.dbServer.AddInParameter(storedProcCommand, "OperationType", DbType.Int16, (short) details.OperationType);
                this.dbServer.AddInParameter(storedProcCommand, "TransactionQuantity", DbType.Double, details.TransactionQuantity);
                this.dbServer.AddInParameter(storedProcCommand, "InputTransactionQuantity", DbType.Double, details.InputTransactionQuantity);
                this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, details.TransactionTypeID.ToString());
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, details.Status);
                this.dbServer.AddInParameter(storedProcCommand, "BaseUOMID", DbType.Int64, details.BaseUOMID);
                this.dbServer.AddInParameter(storedProcCommand, "BaseCF", DbType.Double, details.BaseConversionFactor);
                this.dbServer.AddInParameter(storedProcCommand, "StockUOMID", DbType.Int64, details.SUOMID);
                this.dbServer.AddInParameter(storedProcCommand, "StockCF", DbType.Double, details.ConversionFactor);
                this.dbServer.AddInParameter(storedProcCommand, "TransactionUOMID", DbType.Int64, details.SelectedUOM.ID);
                this.dbServer.AddInParameter(storedProcCommand, "StockingQuantity", DbType.Double, details.StockingQuantity);
                this.dbServer.AddInParameter(storedProcCommand, "PurchaseRate1", DbType.Single, details.PurchaseRate);
                this.dbServer.AddInParameter(storedProcCommand, "MRP1", DbType.Single, details.MRP);
                this.dbServer.AddInParameter(storedProcCommand, "IsFromOpeningBalance", DbType.Boolean, details.IsFromOpeningBalance);
                if (details.UnitId > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, details.UnitId);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                }
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, details.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "BarCode", DbType.String, details.BarCode);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if (myConnection == null)
                {
                    transaction.Commit();
                }
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                if (myConnection == null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (myConnection == null)
                {
                    connection.Close();
                }
            }
            return BizActionObj;
        }

        public override IValueObject AddFree(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {
            clsAddItemStockBizActionVO bizActionObj = valueObject as clsAddItemStockBizActionVO;
            if (bizActionObj.Details.ID == 0L)
            {
                bizActionObj = this.AddDetailsFree(bizActionObj, UserVo, myConnection, myTransaction);
            }
            return valueObject;
        }

        public override IValueObject GetAvailableStockList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetAvailableStockListBizActionVO nvo = valueObject as clsGetAvailableStockListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_rpt_AvailableStock");
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "clincId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "storeId", DbType.Int64, 0);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.BatchList == null)
                    {
                        nvo.BatchList = new List<clsItemStockVO>();
                    }
                    while (reader.Read())
                    {
                        clsItemStockVO item = new clsItemStockVO {
                            ItemName = (string) DALHelper.HandleDBNull(reader["ItemName"]),
                            BatchCode = (string) DALHelper.HandleDBNull(reader["BatchCode"]),
                            PhysicalStock = 0.0,
                            AvailableStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"])),
                            PurchaseRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["PurchaseRate"]))
                        };
                        nvo.BatchList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetGRNItemBatchwiseStockForQS(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetItemStockBizActionVO nvo = valueObject as clsGetItemStockBizActionVO;
            List<clsItemStockVO> list1 = new List<clsItemStockVO>();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetGRNItemBatchwiseStockForQS");
                this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, nvo.ItemID);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "BatchID", DbType.Int64, nvo.BatchID);
                this.dbServer.AddInParameter(storedProcCommand, "TransactionID", DbType.Int64, nvo.TransactionID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.BatchList == null)
                    {
                        nvo.BatchList = new List<clsItemStockVO>();
                    }
                    while (reader.Read())
                    {
                        clsItemStockVO item = new clsItemStockVO {
                            Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"])),
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            AvailableStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"])),
                            StockingUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"])),
                            PurchaseRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["PurchaseRate"])),
                            MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"])),
                            ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]),
                            BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"])),
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                            TransactionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionID"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"])),
                            IsItemBlock = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsItemBlock"]))
                        };
                        nvo.BatchList.Add(item);
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

        public override IValueObject GetItemBatchwiseStock(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetItemStockBizActionVO nvo = valueObject as clsGetItemStockBizActionVO;
            List<clsItemStockVO> list1 = new List<clsItemStockVO>();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemBatchListForSearch");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, nvo.ItemID);
                this.dbServer.AddInParameter(storedProcCommand, "ShowExpiredBatches", DbType.Boolean, nvo.ShowExpiredBatches);
                this.dbServer.AddInParameter(storedProcCommand, "ShowZeroStockBatches", DbType.Boolean, nvo.ShowZeroStockBatches);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "IsFree", DbType.Boolean, nvo.IsFree);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.BatchList == null)
                    {
                        nvo.BatchList = new List<clsItemStockVO>();
                    }
                    while (reader.Read())
                    {
                        clsItemStockVO item = new clsItemStockVO {
                            ID = Convert.ToInt64(reader["ID"].HandleDBNull()),
                            StoreID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]))),
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                            BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"])),
                            AvailableStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"])),
                            CurrentStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["CurrentStock"])),
                            BatchCode = (string) DALHelper.HandleDBNull(reader["BatchCode"]),
                            ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]),
                            ConversionFactor = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConversionFactor"])),
                            MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"])),
                            PurchaseRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["PurchaseRate"])),
                            Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"])),
                            VATAmt = Convert.ToDouble(DALHelper.HandleDBNull(reader["VatAmount"])),
                            VATPerc = Convert.ToDouble(DALHelper.HandleDBNull(reader["VatPercentage"])),
                            DiscountOnSale = Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountOnSale"])),
                            Status = false,
                            BarCode = Convert.ToString(DALHelper.HandleDBNull(reader["BarCode"])),
                            IsConsignment = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsConsignment"])),
                            Re_Order = Convert.ToSingle(DALHelper.HandleDBNull(reader["Re_Order"])),
                            AvailableStockInBase = Convert.ToSingle(DALHelper.HandleDBNull(reader["AvailableStockInBase"])),
                            SGSTPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SGSTPercentage"])),
                            SGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SGSTAmount"])),
                            CGSTPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["CGSTPercentage"])),
                            CGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["CGSTAmount"])),
                            IGSTPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IGSTPercentage"])),
                            IGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IGSTAmount"]))
                        };
                        if (nvo.ShowZeroStockBatches)
                        {
                            item.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SUM"]));
                            item.PUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"]));
                            item.SUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["SUMID"]));
                            item.PUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["PUMID"]));
                            item.PurchaseToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["PTBCF"]));
                            item.StockingToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["STBCF"]));
                        }
                        item.StockingUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        item.IsFreeItem = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsFree"]));
                        item.StaffDiscount = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffDiscount"]));
                        item.WalkinDiscount = Convert.ToDouble(DALHelper.HandleDBNull(reader["WalkinDiscount"]));
                        item.RegisteredPatientsDiscount = Convert.ToDouble(DALHelper.HandleDBNull(reader["RegisteredPatientsDiscount"]));
                        if (!nvo.ShowZeroStockBatches)
                        {
                            item.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"]));
                            item.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"]));
                            item.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["Container"]));
                        }
                        nvo.BatchList.Add(item);
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

        public override IValueObject GetItemBatchwiseStockFree(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetItemStockBizActionVO nvo = valueObject as clsGetItemStockBizActionVO;
            List<clsItemStockVO> list1 = new List<clsItemStockVO>();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemBatchListForSearchFree");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, nvo.ItemID);
                this.dbServer.AddInParameter(storedProcCommand, "ShowExpiredBatches", DbType.Boolean, nvo.ShowExpiredBatches);
                this.dbServer.AddInParameter(storedProcCommand, "ShowZeroStockBatches", DbType.Boolean, nvo.ShowZeroStockBatches);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "IsFree", DbType.Boolean, nvo.IsFree);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.BatchList == null)
                    {
                        nvo.BatchList = new List<clsItemStockVO>();
                    }
                    while (reader.Read())
                    {
                        clsItemStockVO item = new clsItemStockVO {
                            ID = Convert.ToInt64(reader["ID"].HandleDBNull()),
                            StoreID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]))),
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                            BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"])),
                            AvailableStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"])),
                            CurrentStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["CurrentStock"])),
                            BatchCode = (string) DALHelper.HandleDBNull(reader["BatchCode"]),
                            ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]),
                            ConversionFactor = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConversionFactor"])),
                            MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"])),
                            PurchaseRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["PurchaseRate"])),
                            Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"])),
                            VATAmt = Convert.ToDouble(DALHelper.HandleDBNull(reader["VatAmount"])),
                            VATPerc = Convert.ToDouble(DALHelper.HandleDBNull(reader["VatPercentage"])),
                            SGSTPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SGSTPercentage"])),
                            SGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SGSTPercentage"])),
                            CGSTPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["CGSTPercentage"])),
                            CGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["CGSTPercentage"])),
                            IGSTPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IGSTPercentage"])),
                            IGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IGSTPercentage"])),
                            DiscountOnSale = Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountOnSale"])),
                            Status = false,
                            BarCode = Convert.ToString(DALHelper.HandleDBNull(reader["BarCode"])),
                            IsConsignment = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsConsignment"])),
                            Re_Order = Convert.ToSingle(DALHelper.HandleDBNull(reader["Re_Order"])),
                            AvailableStockInBase = Convert.ToSingle(DALHelper.HandleDBNull(reader["AvailableStockInBase"]))
                        };
                        if (nvo.ShowZeroStockBatches)
                        {
                            item.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SUM"]));
                            item.PUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"]));
                            item.SUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["SUMID"]));
                            item.PUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["PUMID"]));
                            item.PurchaseToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["PTBCF"]));
                            item.StockingToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["STBCF"]));
                        }
                        nvo.BatchList.Add(item);
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

        public override IValueObject GetItemCurrentStockList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetItemCurrentStockListBizActionVO nvo = valueObject as clsGetItemCurrentStockListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemCurrentStockList");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, nvo.UserID);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, nvo.ItemID);
                this.dbServer.AddInParameter(storedProcCommand, "BatchID", DbType.Int64, nvo.BatchID);
                if (nvo.IsForCentralPurChaseFromApproveIndent)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IsFromIndent", DbType.Boolean, nvo.IsForCentralPurChaseFromApproveIndent);
                    this.dbServer.AddInParameter(storedProcCommand, "IndentID", DbType.Int64, nvo.IndentID);
                    this.dbServer.AddInParameter(storedProcCommand, "IndentUnitID", DbType.Int64, nvo.IndentUnitID);
                }
                if ((nvo.ItemName != null) && (nvo.ItemName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ItemName", DbType.String, "%" + nvo.ItemName + "%");
                }
                if ((nvo.BatchCode != null) && (nvo.BatchCode.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "BatchCode", DbType.String, "%" + nvo.BatchCode + "%");
                }
                if (nvo.FromDate != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                }
                this.dbServer.AddInParameter(storedProcCommand, "isStockZero", DbType.Boolean, nvo.IsStockZero);
                this.dbServer.AddInParameter(storedProcCommand, "IsConsignment", DbType.Boolean, nvo.IsConsignment);
                this.dbServer.AddInParameter(storedProcCommand, "ItemGroupID", DbType.Int64, nvo.ItemGroupID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemCategoryID", DbType.Int64, nvo.ItemCategoryID);
                if (nvo.ToDate != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.Int32, nvo.SortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.BatchList == null)
                    {
                        nvo.BatchList = new List<clsItemStockVO>();
                    }
                    while (reader.Read())
                    {
                        clsItemStockVO item = new clsItemStockVO {
                            ID = Convert.ToInt64(reader["ID"].HandleDBNull()),
                            UnitId = Convert.ToInt64(reader["UnitId"].HandleDBNull()),
                            StoreID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]))),
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                            BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"])),
                            AvailableStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"])),
                            BatchCode = (string) DALHelper.HandleDBNull(reader["BatchCode"]),
                            ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]),
                            MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRPPACK"])),
                            PurchaseRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["CPPack"])),
                            VATAmt = Convert.ToDouble(DALHelper.HandleDBNull(reader["VatAmount"])),
                            VATPerc = Convert.ToDouble(DALHelper.HandleDBNull(reader["VatPercentage"])),
                            UnitName = (string) DALHelper.HandleDBNull(reader["UnitName"]),
                            ItemCode = (string) DALHelper.HandleDBNull(reader["ItemCode"]),
                            ItemName = (string) DALHelper.HandleDBNull(reader["ItemName"]),
                            StoreName = (string) DALHelper.HandleDBNull(reader["StoreName"]),
                            StockingUOM = (string) DALHelper.HandleDBNull(reader["UOM"]),
                            BaseCP = Convert.ToSingle(DALHelper.HandleDBNull(reader["PurchaseRate"])),
                            BaseMRP = Convert.ToSingle(DALHelper.HandleDBNull(reader["MRP"])),
                            TotalNetCP = Convert.ToSingle(DALHelper.HandleDBNull(reader["TotalNetCP"])),
                            IsFreeItem = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFree"])),
                            SGSTPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SGSTPercentage"])),
                            SGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SGSTAmount"])),
                            CGSTPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["CGSTPercentage"])),
                            CGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["CGSTAmount"])),
                            IGSTPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IGSTPercentage"])),
                            IGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IGSTAmount"])),
                            ItemGroupString = Convert.ToString(DALHelper.HandleDBNull(reader["ItemGroupString"])),
                            ItemCategoryString = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCategoryString"]))
                        };
                        nvo.BatchList.Add(item);
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

