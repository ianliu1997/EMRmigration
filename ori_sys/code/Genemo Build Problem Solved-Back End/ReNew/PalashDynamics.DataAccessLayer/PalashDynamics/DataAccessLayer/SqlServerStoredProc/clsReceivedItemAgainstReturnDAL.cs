namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Inventory;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Reflection;

    internal class clsReceivedItemAgainstReturnDAL : clsBaseReceivedItemAgainstReturnDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsReceivedItemAgainstReturnDAL()
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

        public override IValueObject AddReceivedItemAgainstReturn(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddReceivedItemAgainstReturnBizActionVO nvo = valueObject as clsAddReceivedItemAgainstReturnBizActionVO;
            DbConnection myConnection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            try
            {
                myConnection.Open();
                transaction = myConnection.BeginTransaction();
                clsReceivedItemAgainstReturnVO receivedItemAgainstReturnDetails = nvo.ReceivedItemAgainstReturnDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddReceivedItemAgainstReturn");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, receivedItemAgainstReturnDetails.LinkServer);
                if (receivedItemAgainstReturnDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, receivedItemAgainstReturnDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "ReturnID", DbType.Int64, receivedItemAgainstReturnDetails.ReturnID);
                if (DALHelper.IsValidDateRangeDB(receivedItemAgainstReturnDetails.ReceivedDate))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ReceivedDate", DbType.DateTime, receivedItemAgainstReturnDetails.ReceivedDate);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ReceivedDate", DbType.DateTime, null);
                }
                this.dbServer.AddInParameter(storedProcCommand, "ReceivedByID", DbType.Int64, receivedItemAgainstReturnDetails.ReceivedByID);
                this.dbServer.AddInParameter(storedProcCommand, "ReceivedFromStoreId", DbType.Int64, receivedItemAgainstReturnDetails.ReceivedFromStoreId);
                this.dbServer.AddParameter(storedProcCommand, "ReceivedNumber", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");
                this.dbServer.AddInParameter(storedProcCommand, "ReceivedToStoreId", DbType.Int64, receivedItemAgainstReturnDetails.ReceivedToStoreId);
                this.dbServer.AddInParameter(storedProcCommand, "ReturnUnitId", DbType.Int64, receivedItemAgainstReturnDetails.ReturnUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, receivedItemAgainstReturnDetails.Remark);
                this.dbServer.AddInParameter(storedProcCommand, "TotalAmount", DbType.Decimal, receivedItemAgainstReturnDetails.TotalAmount);
                this.dbServer.AddInParameter(storedProcCommand, "TotalItems", DbType.Decimal, receivedItemAgainstReturnDetails.TotalItems);
                this.dbServer.AddInParameter(storedProcCommand, "TotalVATAmount", DbType.Decimal, receivedItemAgainstReturnDetails.TotalVATAmount);
                this.dbServer.AddInParameter(storedProcCommand, "IsIndent", DbType.Boolean, receivedItemAgainstReturnDetails.IsIndent);
                if (receivedItemAgainstReturnDetails.IsIndent)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "InventoryIndentType", DbType.String, InventoryIndentType.Indent);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "InventoryIndentType", DbType.String, InventoryIndentType.PurchaseRequisition);
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "LoginUserId", DbType.Int64, userVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "MachineName", DbType.String, userVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.ReceivedItemAgainstReturnDetails.ReceivedID = (long?) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.ReceivedItemAgainstReturnDetails.ReceivedNumber = (string) this.dbServer.GetParameterValue(storedProcCommand, "ReceivedNumber");
                foreach (clsReceivedItemAgainstReturnDetailsVO svo in nvo.ReceivedItemAgainstReturnDetails.ReceivedItemAgainstReturnDetailsList)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddReceivedItemAgainstReturnList");
                    command2.Connection = myConnection;
                    this.dbServer.AddInParameter(command2, "LinkServer", DbType.String, receivedItemAgainstReturnDetails.LinkServer);
                    if (receivedItemAgainstReturnDetails.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(command2, "LinkServerAlias", DbType.String, receivedItemAgainstReturnDetails.LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddInParameter(command2, "BatchId", DbType.Int64, svo.BatchId);
                    if (DALHelper.IsValidDateRangeDB(svo.ExpiryDate))
                    {
                        this.dbServer.AddInParameter(command2, "ExpiryDate", DbType.DateTime, svo.ExpiryDate);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(command2, "ExpiryDate", DbType.DateTime, null);
                    }
                    this.dbServer.AddInParameter(command2, "BatchCode", DbType.String, svo.BatchCode);
                    this.dbServer.AddInParameter(command2, "ReturnQty", DbType.Decimal, svo.ReturnQty);
                    this.dbServer.AddInParameter(command2, "ReceivedQty", DbType.Decimal, svo.ReceivedQty);
                    this.dbServer.AddInParameter(command2, "ItemId", DbType.Int64, svo.ItemId);
                    this.dbServer.AddInParameter(command2, "ItemTotalAmount", DbType.Decimal, svo.ItemTotalAmount);
                    this.dbServer.AddInParameter(command2, "ItemVATAmount", DbType.Decimal, svo.ItemVATAmount);
                    this.dbServer.AddInParameter(command2, "ItemVATPercentage", DbType.Decimal, svo.ItemVATPercentage);
                    this.dbServer.AddInParameter(command2, "MRP", DbType.Decimal, svo.MRP);
                    this.dbServer.AddInParameter(command2, "PurchaseRate", DbType.Decimal, svo.PurchaseRate);
                    this.dbServer.AddInParameter(command2, "IsIndent", DbType.Boolean, svo.IsIndent);
                    this.dbServer.AddInParameter(command2, "TransactionUOMID", DbType.Int64, svo.SelectedUOM.ID);
                    this.dbServer.AddInParameter(command2, "StockUOMID", DbType.Int64, svo.SUOMID);
                    this.dbServer.AddInParameter(command2, "StockCF", DbType.Double, svo.ConversionFactor);
                    this.dbServer.AddInParameter(command2, "StockingQuantity", DbType.Decimal, svo.StockingQuantity);
                    this.dbServer.AddInParameter(command2, "BaseUOMID", DbType.Int64, svo.BaseUOMID);
                    this.dbServer.AddInParameter(command2, "BaseCF", DbType.Int64, svo.BaseConversionFactor);
                    this.dbServer.AddInParameter(command2, "BaseQuantity", DbType.Decimal, svo.BaseQuantity);
                    this.dbServer.AddInParameter(command2, "ReturnUOMID", DbType.Int64, svo.UOMID);
                    if (receivedItemAgainstReturnDetails.IsIndent)
                    {
                        this.dbServer.AddInParameter(command2, "InventoryIndentType", DbType.String, InventoryIndentType.Indent);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(command2, "InventoryIndentType", DbType.String, InventoryIndentType.PurchaseRequisition);
                    }
                    this.dbServer.AddInParameter(command2, "UnitId", DbType.Decimal, userVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "ReceivedFromStoreID", DbType.Int64, receivedItemAgainstReturnDetails.ReceivedFromStoreId);
                    this.dbServer.AddInParameter(command2, "ReceivedToStoreID", DbType.Int64, receivedItemAgainstReturnDetails.ReceivedToStoreId);
                    this.dbServer.AddInParameter(command2, "DepartmentID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command2, "TransactionTypeID", DbType.Int32, InventoryTransactionType.ReceivedItemAgainstReturn);
                    this.dbServer.AddInParameter(storedProcCommand, "LoginUserId", DbType.Int64, userVO.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "MachineName", DbType.String, userVO.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command2, "ReceivedId", DbType.Int64, nvo.ReceivedItemAgainstReturnDetails.ReceivedID);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddOutParameter(command2, "ID", DbType.Int32, 0);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_UpdateReturn");
                    command3.Connection = myConnection;
                    this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, receivedItemAgainstReturnDetails.ReturnUnitId);
                    this.dbServer.AddInParameter(command3, "BalanceQty", DbType.Decimal, svo.BaseQuantity);
                    this.dbServer.AddInParameter(command3, "ReturnDetailsID", DbType.Int64, svo.ReturnItemDetailsID);
                    this.dbServer.ExecuteNonQuery(command3, transaction);
                    receivedItemAgainstReturnDetails.StockDetails.BatchID = 0L;
                    receivedItemAgainstReturnDetails.StockDetails.BatchCode = svo.BatchCode;
                    receivedItemAgainstReturnDetails.StockDetails.OperationType = InventoryStockOperationType.Addition;
                    receivedItemAgainstReturnDetails.StockDetails.ItemID = Convert.ToInt64(svo.ItemId);
                    receivedItemAgainstReturnDetails.StockDetails.TransactionTypeID = InventoryTransactionType.ReceivedItemAgainstReturn;
                    receivedItemAgainstReturnDetails.StockDetails.TransactionID = Convert.ToInt64(nvo.ReceivedItemAgainstReturnDetails.ReceivedID);
                    receivedItemAgainstReturnDetails.StockDetails.TransactionQuantity = svo.IsIndent ? ((double) svo.BaseQuantity) : ((double) svo.BaseQuantity);
                    receivedItemAgainstReturnDetails.StockDetails.Date = Convert.ToDateTime(receivedItemAgainstReturnDetails.ReceivedDate);
                    receivedItemAgainstReturnDetails.StockDetails.Time = DateTime.Now;
                    receivedItemAgainstReturnDetails.StockDetails.StoreID = receivedItemAgainstReturnDetails.ReceivedFromStoreId;
                    receivedItemAgainstReturnDetails.StockDetails.StockingQuantity = svo.StockingQuantity;
                    receivedItemAgainstReturnDetails.StockDetails.BaseUOMID = svo.BaseUOMID;
                    receivedItemAgainstReturnDetails.StockDetails.BaseConversionFactor = svo.BaseConversionFactor;
                    receivedItemAgainstReturnDetails.StockDetails.SUOMID = svo.SUOMID;
                    receivedItemAgainstReturnDetails.StockDetails.ConversionFactor = Convert.ToDouble(svo.ConversionFactor);
                    receivedItemAgainstReturnDetails.StockDetails.SelectedUOM.ID = svo.SelectedUOM.ID;
                    receivedItemAgainstReturnDetails.StockDetails.InputTransactionQuantity = Convert.ToSingle(svo.ReceivedQty);
                    receivedItemAgainstReturnDetails.StockDetails.ExpiryDate = svo.ExpiryDate;
                    clsBaseItemStockDAL instance = clsBaseItemStockDAL.GetInstance();
                    clsAddItemStockBizActionVO nvo3 = new clsAddItemStockBizActionVO {
                        Details = receivedItemAgainstReturnDetails.StockDetails
                    };
                    nvo3.Details.ID = 0L;
                    nvo3 = (clsAddItemStockBizActionVO) instance.Add(nvo3, userVO, myConnection, transaction);
                    if (nvo3.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    receivedItemAgainstReturnDetails.StockDetails.ID = nvo3.Details.ID;
                }
                transaction.Commit();
            }
            catch (Exception exception)
            {
                this.logManager.LogError(userVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                nvo.SuccessStatus = -1;
                nvo.ReceivedItemAgainstReturnDetails = null;
            }
            finally
            {
                myConnection.Close();
            }
            return nvo;
        }

        public override IValueObject GetItemListByReturnReceivedId(IValueObject valueObject, clsUserVO userVO)
        {
            clsGetItemListByReturnReceivedIdBizActionVO nvo = valueObject as clsGetItemListByReturnReceivedIdBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemListByReceivedAgainstReturnId");
                this.dbServer.AddInParameter(storedProcCommand, "ReceivedId", DbType.Int64, nvo.ReceivedId);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ReceivedUnitId", DbType.Int64, nvo.ReceivedUnitId);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemList == null)
                    {
                        nvo.ItemList = new List<clsReceivedItemAgainstReturnDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsReceivedItemAgainstReturnDetailsVO item = new clsReceivedItemAgainstReturnDetailsVO {
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            BatchId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchId"]))),
                            ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]),
                            ReturnQty = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["ReturnQty"]))),
                            ReturnUOM = Convert.ToString(DALHelper.HandleDBNull(reader["ReturnedQtyUOM"])),
                            ReceivedQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ReceiveddQuantity"])),
                            ReceivedUOM = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedQtyUOM"])),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            ItemId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]))),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            ItemTotalAmount = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemTotalAmount"]))),
                            ItemVATPercentage = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPercentage"]))),
                            MRP = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["BatchMRP"]))),
                            PurchaseRate = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"])))
                        };
                        nvo.ItemList.Add(item);
                    }
                }
                reader.NextResult();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return valueObject;
        }

        public override IValueObject GetReceivedListAgainstReturn(IValueObject valueObject, clsUserVO userVO)
        {
            clsGetReceivedListAgainstReturnBizActionVO nvo = valueObject as clsGetReceivedListAgainstReturnBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetReceivedListAgainstReturn");
                this.dbServer.AddInParameter(storedProcCommand, "ReceivedNumberSrc", DbType.String, nvo.ReceivedNumberSrc);
                if (DALHelper.IsValidDateRangeDB(nvo.ReceivedFromDate))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ReceivedFromDate", DbType.DateTime, nvo.ReceivedFromDate.Value);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ReceivedFromDate", DbType.DateTime, null);
                }
                if (DALHelper.IsValidDateRangeDB(nvo.ReceivedToDate))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ReceivedToDate", DbType.DateTime, nvo.ReceivedToDate.Value);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ReceivedToDate", DbType.DateTime, null);
                }
                this.dbServer.AddInParameter(storedProcCommand, "ReceivedFromStoreId", DbType.Int64, nvo.ReceivedFromStoreId);
                this.dbServer.AddInParameter(storedProcCommand, "ReceivedToStoreId", DbType.Int64, nvo.ReceivedToStoreId);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.InputSortExpression);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, userVO.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsReceivedAgainstReturnListVO item = new clsReceivedAgainstReturnListVO {
                            ReceivedDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["ReceivedDate"]))),
                            ReceivedFromStoreId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedFromStoreId"]))),
                            ReceivedFromStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedFromStoreName"])),
                            ReceivedId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedId"]))),
                            ReceivedNumber = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedNumber"])),
                            ReceivedToStoreId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedToStoreId"]))),
                            ReceivedToStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedToStoreName"])),
                            TotalAmount = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalAmount"]))),
                            TotalItems = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalItems"]))),
                            ReturnId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ReturnId"]))),
                            ReturnNumber = Convert.ToString(DALHelper.HandleDBNull(reader["ReturnNumber"])),
                            Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            ReturnUnitID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ReturnUnitId"])))
                        };
                        nvo.ReceivedList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return valueObject;
        }
    }
}

