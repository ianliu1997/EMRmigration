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
    using System.Reflection;

    internal class clsReturnItemDAL : clsBaseReturnItemDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsReturnItemDAL()
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

        public override IValueObject AddReturnItemToStore(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddReturnItemToStoreBizActionVO nvo = valueObject as clsAddReturnItemToStoreBizActionVO;
            DbConnection myConnection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            try
            {
                myConnection.Open();
                transaction = myConnection.BeginTransaction();
                clsReturnItemVO returnItemDetails = nvo.ReturnItemDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddReturnItemToStore");
                storedProcCommand.Connection = myConnection;
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, returnItemDetails.LinkServer);
                if (returnItemDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, returnItemDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "IssueID", DbType.Int64, returnItemDetails.IssueID);
                if (DALHelper.IsValidDateRangeDB(returnItemDetails.ReturnDate))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ReturnDate", DbType.DateTime, returnItemDetails.ReturnDate);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ReturnDate", DbType.DateTime, null);
                }
                this.dbServer.AddInParameter(storedProcCommand, "ReturnFromStoreId", DbType.Int64, returnItemDetails.ReturnFromStoreId);
                this.dbServer.AddParameter(storedProcCommand, "ReturnNumber", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");
                this.dbServer.AddInParameter(storedProcCommand, "ReturnToStoreId", DbType.Int64, returnItemDetails.ReturnToStoreId);
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, returnItemDetails.Remark);
                this.dbServer.AddInParameter(storedProcCommand, "TotalAmount", DbType.Decimal, returnItemDetails.TotalAmount);
                this.dbServer.AddInParameter(storedProcCommand, "TotalItems", DbType.Decimal, returnItemDetails.TotalItems);
                this.dbServer.AddInParameter(storedProcCommand, "TotalVATAmount", DbType.Decimal, returnItemDetails.TotalVATAmount);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "LoginUserId", DbType.Int64, userVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "ReceivedID", DbType.Int64, returnItemDetails.ReceivedID);
                this.dbServer.AddInParameter(storedProcCommand, "IssueUnitID", DbType.Int64, returnItemDetails.IssueUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "MachineName", DbType.String, userVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, returnItemDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, returnItemDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsAgainstPatient", DbType.Boolean, returnItemDetails.IsAgainstPatient);
                this.dbServer.AddInParameter(storedProcCommand, "IsIndent", DbType.Int32, returnItemDetails.IsIndent);
                if (returnItemDetails.IsIndent == 1)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "InventoryIndentType", DbType.String, InventoryIndentType.Indent);
                }
                else if (returnItemDetails.IsIndent == 0)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "InventoryIndentType", DbType.String, InventoryIndentType.PurchaseRequisition);
                }
                else if (returnItemDetails.IsIndent == 2)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineExpired);
                }
                else if (returnItemDetails.IsIndent == 3)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineDOS);
                }
                else if (returnItemDetails.IsIndent == 4)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineGRNReturn);
                }
                else if (returnItemDetails.IsIndent == 6)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "InventoryIndentType", DbType.String, InventoryIndentType.Direct);
                }
                this.dbServer.AddParameter(storedProcCommand, "ReturnID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, returnItemDetails.ReturnID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.ReturnItemDetails.ReturnNumber = (string) this.dbServer.GetParameterValue(storedProcCommand, "ReturnNumber");
                nvo.ReturnItemDetails.ReturnID = (long?) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                foreach (clsReturnItemDetailsVO svo in nvo.ReturnItemDetails.ReturnItemDetailsList)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddReturnItemListToStore");
                    command2.Connection = myConnection;
                    this.dbServer.AddInParameter(command2, "LinkServer", DbType.String, returnItemDetails.LinkServer);
                    if (returnItemDetails.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(command2, "LinkServerAlias", DbType.String, returnItemDetails.LinkServer.Replace(@"\", "_"));
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
                    this.dbServer.AddInParameter(command2, "IssueQty", DbType.Decimal, svo.IssueQty);
                    this.dbServer.AddInParameter(command2, "ReturnQty", DbType.Decimal, svo.ReturnQty);
                    this.dbServer.AddInParameter(command2, "BalanceQty", DbType.Decimal, svo.BaseQuantity);
                    this.dbServer.AddInParameter(command2, "TransactionUOMID", DbType.Int64, svo.SelectedUOM.ID);
                    this.dbServer.AddInParameter(command2, "StockUOMID", DbType.Int64, svo.SUOMID);
                    this.dbServer.AddInParameter(command2, "StockCF", DbType.Double, svo.ConversionFactor);
                    this.dbServer.AddInParameter(command2, "StockingQuantity", DbType.Decimal, svo.StockingQuantity);
                    this.dbServer.AddInParameter(command2, "BaseUOMID", DbType.Int64, svo.BaseUOMID);
                    this.dbServer.AddInParameter(command2, "BaseCF", DbType.Int64, svo.BaseConversionFactor);
                    this.dbServer.AddInParameter(command2, "BaseQuantity", DbType.Decimal, svo.BaseQuantity);
                    this.dbServer.AddInParameter(command2, "IssuedUOMID", DbType.Int64, svo.UOMID);
                    this.dbServer.AddInParameter(command2, "ItemId", DbType.Int64, svo.ItemId);
                    this.dbServer.AddInParameter(command2, "ItemTotalAmount", DbType.Decimal, svo.ItemTotalAmount);
                    this.dbServer.AddInParameter(command2, "ItemVATAmount", DbType.Decimal, svo.ItemVATAmount);
                    this.dbServer.AddInParameter(command2, "ItemVATPercentage", DbType.Decimal, svo.ItemVATPercentage);
                    this.dbServer.AddInParameter(command2, "MRP", DbType.Decimal, svo.MRP);
                    this.dbServer.AddInParameter(command2, "PurchaseRate", DbType.Decimal, svo.PurchaseRate);
                    this.dbServer.AddInParameter(command2, "UnitId", DbType.Decimal, userVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "ReturnFromStoreID", DbType.Int64, returnItemDetails.ReturnFromStoreId);
                    this.dbServer.AddInParameter(command2, "ReturnToStoreID", DbType.Int64, returnItemDetails.ReturnToStoreId);
                    this.dbServer.AddInParameter(command2, "DepartmentID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command2, "TransactionTypeID", DbType.Int32, InventoryTransactionType.ItemReturn);
                    this.dbServer.AddInParameter(storedProcCommand, "LoginUserId", DbType.Int64, userVO.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "MachineName", DbType.String, userVO.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command2, "IsIndent", DbType.Int32, svo.IsIndent);
                    if (svo.IsIndent == 1)
                    {
                        this.dbServer.AddInParameter(command2, "InventoryIndentType", DbType.String, InventoryIndentType.Indent);
                    }
                    else if (svo.IsIndent == 0)
                    {
                        this.dbServer.AddInParameter(command2, "InventoryIndentType", DbType.String, InventoryIndentType.PurchaseRequisition);
                    }
                    else if (svo.IsIndent == 2)
                    {
                        this.dbServer.AddInParameter(command2, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineExpired);
                    }
                    else if (svo.IsIndent == 3)
                    {
                        this.dbServer.AddInParameter(command2, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineDOS);
                    }
                    else if (svo.IsIndent == 4)
                    {
                        this.dbServer.AddInParameter(command2, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineGRNReturn);
                    }
                    else if (returnItemDetails.IsIndent == 6)
                    {
                        this.dbServer.AddInParameter(command2, "InventoryIndentType", DbType.String, InventoryIndentType.Direct);
                    }
                    this.dbServer.AddInParameter(command2, "ReturnId", DbType.Int64, nvo.ReturnItemDetails.ReturnID);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddOutParameter(command2, "ID", DbType.Int64, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                    if (svo.ReceivedItemDetailsID != null)
                    {
                        long? receivedItemDetailsID = svo.ReceivedItemDetailsID;
                        if ((receivedItemDetailsID.GetValueOrDefault() != 0L) || (receivedItemDetailsID == null))
                        {
                            DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_UpdateReceive");
                            command3.Connection = myConnection;
                            this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command3, "BalanceQty", DbType.Decimal, svo.BaseQuantity);
                            this.dbServer.AddInParameter(command3, "ReceiveItemDetailsID", DbType.Int64, svo.ReceivedItemDetailsID);
                            this.dbServer.ExecuteNonQuery(command3, transaction);
                        }
                    }
                    returnItemDetails.StockDetails.BatchID = Convert.ToInt64(svo.BatchId);
                    returnItemDetails.StockDetails.OperationType = InventoryStockOperationType.Subtraction;
                    returnItemDetails.StockDetails.ItemID = Convert.ToInt64(svo.ItemId);
                    returnItemDetails.StockDetails.TransactionTypeID = InventoryTransactionType.ItemReturn;
                    returnItemDetails.StockDetails.TransactionID = Convert.ToInt64(returnItemDetails.ReturnID);
                    returnItemDetails.StockDetails.TransactionQuantity = ((svo.IsIndent != 0) || !returnItemDetails.IsIssue) ? ((double) svo.BaseQuantity) : ((double) svo.BaseQuantity);
                    returnItemDetails.StockDetails.Date = DateTime.Now;
                    returnItemDetails.StockDetails.Time = DateTime.Now;
                    returnItemDetails.StockDetails.StoreID = returnItemDetails.ReturnFromStoreId;
                    returnItemDetails.StockDetails.StockingQuantity = svo.StockingQuantity;
                    returnItemDetails.StockDetails.BaseUOMID = svo.BaseUOMID;
                    returnItemDetails.StockDetails.BaseConversionFactor = svo.BaseConversionFactor;
                    returnItemDetails.StockDetails.SUOMID = svo.SUOMID;
                    returnItemDetails.StockDetails.ConversionFactor = Convert.ToDouble(svo.ConversionFactor);
                    returnItemDetails.StockDetails.SelectedUOM.ID = svo.SelectedUOM.ID;
                    returnItemDetails.StockDetails.InputTransactionQuantity = Convert.ToSingle(svo.ReturnQty);
                    returnItemDetails.StockDetails.ExpiryDate = svo.ExpiryDate;
                    clsBaseItemStockDAL instance = clsBaseItemStockDAL.GetInstance();
                    clsAddItemStockBizActionVO nvo2 = new clsAddItemStockBizActionVO {
                        Details = returnItemDetails.StockDetails
                    };
                    nvo2.Details.ID = 0L;
                    nvo2 = (clsAddItemStockBizActionVO) instance.Add(nvo2, userVO, myConnection, transaction);
                    if (nvo2.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    returnItemDetails.StockDetails.ID = nvo2.Details.ID;
                }
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(userVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                nvo.SuccessStatus = -1;
                nvo.ReturnItemDetails = null;
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
                myConnection = null;
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject GetItemListByReturnId(IValueObject valueObject, clsUserVO userVO)
        {
            clsGetItemListByReturnIdBizActionVO nvo = valueObject as clsGetItemListByReturnIdBizActionVO;
            if (nvo.ItemList == null)
            {
                nvo.ItemList = new List<clsReturnItemDetailsVO>();
            }
            if (nvo.ItemListAgainstReturn == null)
            {
                nvo.ItemListAgainstReturn = new List<clsReceivedItemAgainstReturnDetailsVO>();
            }
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemListByReturnId");
                this.dbServer.AddInParameter(storedProcCommand, "ReturnId", DbType.Int64, nvo.ReturnId);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitId);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemList == null)
                    {
                        nvo.ItemList = new List<clsReturnItemDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsReturnItemDetailsVO svo = new clsReturnItemDetailsVO {
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            BatchId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchId"]))),
                            ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]),
                            IssueQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IssueQty"])),
                            ReturnQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ReturndQuantity"])),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            ItemId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]))),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            ItemTotalAmount = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemTotalAmount"]))),
                            ItemVATPercentage = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPercentage"]))),
                            MRP = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["BatchMRP"]))),
                            PurchaseRate = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"]))),
                            ConversionFactor = new float?((float) Convert.ToInt64(DALHelper.HandleDBNull(reader["ConversionFactor"]))),
                            ReturnedUOM = Convert.ToString(DALHelper.HandleDBNull(reader["ReturnedUOM"])),
                            IsIndent = Convert.ToInt32(DALHelper.HandleDBNull(reader["IsIndent"]))
                        };
                        nvo.ItemList.Add(svo);
                        clsReceivedItemAgainstReturnDetailsVO svo2 = new clsReceivedItemAgainstReturnDetailsVO {
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            BatchId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchId"]))),
                            ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]),
                            ReturnQty = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["ReturndQuantity"]))),
                            ReceivedQty = 0M,
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            ItemId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]))),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            ItemVATPercentage = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPercentage"]))),
                            MRP = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["BatchMRP"]))),
                            PurchaseRate = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"]))),
                            BalanceQty = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["PendingQuantity"]))),
                            ReturnItemDetailsID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ReturnId"]))),
                            IsIndent = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsIndent"])),
                            BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUOMID"])),
                            SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"])),
                            BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"])),
                            SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"])),
                            UOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"])),
                            ReturnUOM = Convert.ToString(DALHelper.HandleDBNull(reader["ReturnedUOM"])),
                            ConversionFactor = Convert.ToSingle(DALHelper.HandleBoolDBNull(reader["StockCF"])),
                            BaseConversionFactor = Convert.ToSingle(DALHelper.HandleBoolDBNull(reader["BaseCF"]))
                        };
                        MasterListItem item = new MasterListItem {
                            ID = svo2.UOMID,
                            Description = svo2.ReturnUOM
                        };
                        svo2.SelectedUOM = item;
                        nvo.ItemListAgainstReturn.Add(svo2);
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
            return nvo;
        }

        public override IValueObject GetReturnList(IValueObject valueObject, clsUserVO userVO)
        {
            clsGetReturnListBizActionVO nvo = valueObject as clsGetReturnListBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetReturnList");
                this.dbServer.AddInParameter(storedProcCommand, "ReturnNumberSrc", DbType.String, nvo.ReturnNumberSrc);
                if (DALHelper.IsValidDateRangeDB(nvo.ReturnFromDate))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ReturnFromDate", DbType.DateTime, nvo.ReturnFromDate.Value);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ReturnFromDate", DbType.DateTime, null);
                }
                if (DALHelper.IsValidDateRangeDB(nvo.ReturnToDate))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ReturnToDate", DbType.DateTime, nvo.ReturnToDate.Value);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ReturnToDate", DbType.DateTime, null);
                }
                this.dbServer.AddInParameter(storedProcCommand, "ReturnFromStoreId", DbType.Int64, nvo.ReturnFromStoreId);
                this.dbServer.AddInParameter(storedProcCommand, "ReturnToStoreId", DbType.Int64, nvo.ReturnToStoreId);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.InputSortExpression);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, nvo.UserId);
                if (nvo.transactionType == InventoryTransactionType.ReceivedItemAgainstReturn)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "transactionType", DbType.Boolean, true);
                }
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsReturnListVO item = new clsReturnListVO {
                            ReturnDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["ReturnDate"]))),
                            ReturnFromStoreId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ReturnFromStoreId"]))),
                            ReturnFromStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["ReturnFromStoreName"])),
                            ReturnId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ReturnId"]))),
                            ReturnNumber = Convert.ToString(DALHelper.HandleDBNull(reader["ReturnNumber"])),
                            ReturnToStoreId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ReturnToStoreId"]))),
                            ReturnToStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["ReturnToStoreName"])),
                            TotalAmount = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalAmount"]))),
                            TotalItems = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalItems"]))),
                            IssueId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["IssueId"]))),
                            IssueNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IssueNumber"])),
                            Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            IsIndent = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsIndent"])),
                            ReturnByName = Convert.ToString(DALHelper.HandleDBNull(reader["ReturnByName"]))
                        };
                        nvo.ReturnList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
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
            return nvo;
        }
    }
}

