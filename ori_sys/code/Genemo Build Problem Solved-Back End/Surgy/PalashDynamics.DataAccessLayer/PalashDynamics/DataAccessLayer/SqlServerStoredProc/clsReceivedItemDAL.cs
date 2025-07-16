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

    internal class clsReceivedItemDAL : clsBaseReceivedItemDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsReceivedItemDAL()
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

        public override IValueObject AddReceivedItem(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddReceivedItemBizActionVO nvo = valueObject as clsAddReceivedItemBizActionVO;
            DbConnection myConnection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            try
            {
                myConnection.Open();
                transaction = myConnection.BeginTransaction();
                clsReceivedItemVO receivedItemDetails = nvo.ReceivedItemDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddReceivedItem");
                storedProcCommand.Connection = myConnection;
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, receivedItemDetails.LinkServer);
                if (receivedItemDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, receivedItemDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "IssueID", DbType.Int64, receivedItemDetails.IssueID);
                this.dbServer.AddInParameter(storedProcCommand, "IssueUnitID", DbType.Int64, receivedItemDetails.IssueUnitID);
                if (DALHelper.IsValidDateRangeDB(receivedItemDetails.ReceivedDate))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ReceivedDate", DbType.DateTime, receivedItemDetails.ReceivedDate);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ReceivedDate", DbType.DateTime, null);
                }
                this.dbServer.AddInParameter(storedProcCommand, "ReceivedFromStoreId", DbType.Int64, receivedItemDetails.ReceivedFromStoreId);
                this.dbServer.AddParameter(storedProcCommand, "ReceivedNumber", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");
                this.dbServer.AddInParameter(storedProcCommand, "ReceivedToStoreId", DbType.Int64, receivedItemDetails.ReceivedToStoreId);
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, receivedItemDetails.Remark);
                this.dbServer.AddInParameter(storedProcCommand, "TotalAmount", DbType.Decimal, receivedItemDetails.TotalAmount);
                this.dbServer.AddInParameter(storedProcCommand, "TotalItems", DbType.Decimal, receivedItemDetails.TotalItems);
                this.dbServer.AddInParameter(storedProcCommand, "TotalTAXAmount", DbType.Decimal, receivedItemDetails.TotalTaxAmount);
                this.dbServer.AddInParameter(storedProcCommand, "TotalVATAmount", DbType.Decimal, receivedItemDetails.TotalVATAmount);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "LoginUserId", DbType.Int64, userVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "MachineName", DbType.String, userVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "ReceivedById", DbType.Int64, receivedItemDetails.ReceivedById);
                this.dbServer.AddInParameter(storedProcCommand, "IsIndent", DbType.Int32, receivedItemDetails.IsIndent);
                this.dbServer.AddInParameter(storedProcCommand, "IsPatientIndent", DbType.Boolean, receivedItemDetails.IsAgainstPatient);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, receivedItemDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, receivedItemDetails.PatientUnitID);
                if (receivedItemDetails.IsIndent == 1)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "InventoryIndentType", DbType.String, InventoryIndentType.Indent);
                }
                else if (receivedItemDetails.IsIndent == 0)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "InventoryIndentType", DbType.String, InventoryIndentType.PurchaseRequisition);
                }
                else if (receivedItemDetails.IsIndent == 2)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineExpired);
                }
                else if (receivedItemDetails.IsIndent == 3)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineDOS);
                }
                else if (receivedItemDetails.IsIndent == 4)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineGRNReturn);
                }
                else if (receivedItemDetails.IsIndent == 6)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "InventoryIndentType", DbType.String, InventoryIndentType.Direct);
                }
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.ReceivedItemDetails.ReceivedID = (long?) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.ReceivedItemDetails.ReceivedNumber = (string) this.dbServer.GetParameterValue(storedProcCommand, "ReceivedNumber");
                foreach (clsReceivedItemDetailsVO svo in nvo.ReceivedItemDetails.ReceivedItemDetailsList)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddReceivedItemList");
                    command2.Connection = myConnection;
                    this.dbServer.AddInParameter(command2, "LinkServer", DbType.String, receivedItemDetails.LinkServer);
                    if (receivedItemDetails.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(command2, "LinkServerAlias", DbType.String, receivedItemDetails.LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddInParameter(command2, "BatchId", DbType.Int64, svo.BatchId);
                    this.dbServer.AddInParameter(command2, "BatchCode", DbType.String, svo.BatchCode);
                    this.dbServer.AddOutParameter(command2, "ID", DbType.Int64, 0);
                    if (DALHelper.IsValidDateRangeDB(svo.ExpiryDate))
                    {
                        this.dbServer.AddInParameter(command2, "ExpiryDate", DbType.DateTime, svo.ExpiryDate);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(command2, "ExpiryDate", DbType.DateTime, null);
                    }
                    this.dbServer.AddInParameter(command2, "IssueQty", DbType.Decimal, svo.IssueQty);
                    this.dbServer.AddInParameter(command2, "TransactionUOMID", DbType.Int64, svo.SelectedUOM.ID);
                    this.dbServer.AddInParameter(command2, "StockUOMID", DbType.Int64, svo.SUOMID);
                    this.dbServer.AddInParameter(command2, "StockCF", DbType.Single, svo.ConversionFactor);
                    this.dbServer.AddInParameter(command2, "StockingQuantity", DbType.Decimal, svo.StockingQuantity);
                    this.dbServer.AddInParameter(command2, "BaseUOMID", DbType.Int64, svo.BaseUOMID);
                    this.dbServer.AddInParameter(command2, "BaseCF", DbType.Int64, svo.BaseConversionFactor);
                    this.dbServer.AddInParameter(command2, "BaseQuantity", DbType.Decimal, svo.BaseQuantity);
                    this.dbServer.AddInParameter(command2, "IssuedUOMID", DbType.Int64, svo.UOMID);
                    this.dbServer.AddInParameter(command2, "ReceivedQty", DbType.Decimal, svo.ReceivedQty);
                    this.dbServer.AddInParameter(command2, "BalanceQty", DbType.Decimal, svo.BaseQuantity);
                    this.dbServer.AddInParameter(command2, "ItemId", DbType.Int64, svo.ItemId);
                    this.dbServer.AddInParameter(command2, "MRP", DbType.Decimal, svo.MainMRP);
                    this.dbServer.AddInParameter(command2, "MRPAmount", DbType.Decimal, svo.MRPAmt);
                    this.dbServer.AddInParameter(command2, "PurchaseRate", DbType.Decimal, svo.MainRate);
                    this.dbServer.AddInParameter(command2, "PurchaseRateAmount", DbType.Decimal, svo.PurchaseRateAmt);
                    this.dbServer.AddInParameter(command2, "UnitId", DbType.Decimal, userVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "ReceivedFromStoreID", DbType.Int64, receivedItemDetails.ReceivedFromStoreId);
                    this.dbServer.AddInParameter(command2, "ReceivedToStoreID", DbType.Int64, receivedItemDetails.ReceivedToStoreId);
                    this.dbServer.AddInParameter(command2, "DepartmentID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command2, "TransactionTypeID", DbType.Int32, InventoryTransactionType.ReceivedItemAgainstIssue);
                    this.dbServer.AddInParameter(storedProcCommand, "LoginUserId", DbType.Int64, userVO.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "MachineName", DbType.String, userVO.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command2, "IsIndent", DbType.Int32, svo.IsIndent);
                    this.dbServer.AddInParameter(command2, "GRNID", DbType.Int64, svo.GRNID);
                    this.dbServer.AddInParameter(command2, "GRNUnitID", DbType.Int64, svo.GRNUnitID);
                    this.dbServer.AddInParameter(command2, "GRNDetailID", DbType.Int64, svo.GRNDetailID);
                    this.dbServer.AddInParameter(command2, "GRNDetailUnitID", DbType.Int64, svo.GRNDetailUnitID);
                    this.dbServer.AddInParameter(command2, "ItemTaxPercentage", DbType.Decimal, svo.ItemTaxPercentage);
                    this.dbServer.AddInParameter(command2, "ItemTaxAmount", DbType.Decimal, svo.ItemTaxAmount);
                    this.dbServer.AddInParameter(command2, "ItemVATPercentage", DbType.Decimal, svo.ItemVATPercentage);
                    this.dbServer.AddInParameter(command2, "ItemVATAmount", DbType.Decimal, svo.ItemVATAmount);
                    this.dbServer.AddInParameter(command2, "othertaxApplicableon", DbType.Int16, svo.OtherGRNItemTaxApplicationOn);
                    this.dbServer.AddInParameter(command2, "otherTaxType", DbType.Int64, svo.OtherGRNItemTaxType);
                    this.dbServer.AddInParameter(command2, "VatApplicableon", DbType.Int64, svo.GRNItemVatApplicationOn);
                    this.dbServer.AddInParameter(command2, "Vattype", DbType.Int64, svo.GRNItemVatType);
                    this.dbServer.AddInParameter(command2, "InclusiveOfTax", DbType.Boolean, svo.InclusiveOfTax);
                    if (svo.IsIndent == 1)
                    {
                        this.dbServer.AddInParameter(command2, "InventoryIndentType", DbType.String, InventoryIndentType.Indent);
                        this.dbServer.AddInParameter(command2, "ItemTotalAmount", DbType.Decimal, svo.ItemTotalAmount);
                    }
                    else if (svo.IsIndent == 0)
                    {
                        this.dbServer.AddInParameter(command2, "InventoryIndentType", DbType.String, InventoryIndentType.PurchaseRequisition);
                        this.dbServer.AddInParameter(command2, "ItemTotalAmount", DbType.Decimal, svo.ItemTotalAmount);
                    }
                    else if (svo.IsIndent == 2)
                    {
                        this.dbServer.AddInParameter(command2, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineExpired);
                        this.dbServer.AddInParameter(command2, "ItemTotalAmount", DbType.Decimal, svo.NetAmount);
                    }
                    else if (receivedItemDetails.IsIndent == 3)
                    {
                        this.dbServer.AddInParameter(command2, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineDOS);
                        this.dbServer.AddInParameter(command2, "ItemTotalAmount", DbType.Decimal, svo.NetAmount);
                    }
                    else if (receivedItemDetails.IsIndent == 4)
                    {
                        this.dbServer.AddInParameter(command2, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineGRNReturn);
                        this.dbServer.AddInParameter(command2, "ItemTotalAmount", DbType.Decimal, svo.NetAmount);
                    }
                    else if (receivedItemDetails.IsIndent == 6)
                    {
                        this.dbServer.AddInParameter(command2, "InventoryIndentType", DbType.String, InventoryIndentType.Direct);
                        this.dbServer.AddInParameter(command2, "ItemTotalAmount", DbType.Decimal, svo.ItemTotalAmount);
                    }
                    this.dbServer.AddInParameter(command2, "IssueID", DbType.Int64, receivedItemDetails.IssueID);
                    this.dbServer.AddInParameter(command2, "IssueUnitID", DbType.Int64, receivedItemDetails.IssueUnitID);
                    this.dbServer.AddInParameter(command2, "ReceivedId", DbType.Int64, nvo.ReceivedItemDetails.ReceivedID);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                    if (nvo.SuccessStatus == 10)
                    {
                        nvo.ItemName = svo.ItemName;
                        throw new Exception();
                    }
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_UpdateIssue");
                    command3.Connection = myConnection;
                    this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, nvo.ReceivedItemDetails.IssueUnitID);
                    this.dbServer.AddInParameter(command3, "BalanceQty", DbType.Decimal, svo.ReceivedQty * Convert.ToDecimal(svo.BaseConversionFactor));
                    this.dbServer.AddInParameter(command3, "IssueDetailsID", DbType.Int64, svo.IssueDetailsID);
                    this.dbServer.ExecuteNonQuery(command3, transaction);
                    receivedItemDetails.StockDetails.BatchID = 0L;
                    receivedItemDetails.StockDetails.BatchCode = svo.BatchCode;
                    receivedItemDetails.StockDetails.OperationType = InventoryStockOperationType.Addition;
                    receivedItemDetails.StockDetails.ItemID = Convert.ToInt64(svo.ItemId);
                    receivedItemDetails.StockDetails.TransactionTypeID = InventoryTransactionType.ReceivedItemAgainstIssue;
                    receivedItemDetails.StockDetails.TransactionID = Convert.ToInt64(nvo.ReceivedItemDetails.ReceivedID);
                    receivedItemDetails.StockDetails.TransactionQuantity = (svo.IsIndent != 1) ? ((double) svo.BaseQuantity) : ((double) svo.BaseQuantity);
                    receivedItemDetails.StockDetails.Date = Convert.ToDateTime(receivedItemDetails.ReceivedDate);
                    receivedItemDetails.StockDetails.Time = DateTime.Now;
                    receivedItemDetails.StockDetails.StoreID = receivedItemDetails.ReceivedFromStoreId;
                    receivedItemDetails.StockDetails.StockingQuantity = svo.StockingQuantity;
                    receivedItemDetails.StockDetails.BaseUOMID = svo.BaseUOMID;
                    receivedItemDetails.StockDetails.BaseConversionFactor = svo.BaseConversionFactor;
                    receivedItemDetails.StockDetails.SUOMID = svo.SUOMID;
                    receivedItemDetails.StockDetails.ConversionFactor = Convert.ToDouble(svo.ConversionFactor);
                    receivedItemDetails.StockDetails.SelectedUOM.ID = svo.SelectedUOM.ID;
                    receivedItemDetails.StockDetails.InputTransactionQuantity = Convert.ToSingle(svo.ReceivedQty);
                    receivedItemDetails.StockDetails.ExpiryDate = svo.ExpiryDate;
                    receivedItemDetails.StockDetails.IsFromOpeningBalance = true;
                    receivedItemDetails.StockDetails.MRP = Convert.ToDouble(svo.MainMRP);
                    receivedItemDetails.StockDetails.PurchaseRate = Convert.ToDouble(svo.MainRate);
                    clsBaseItemStockDAL instance = clsBaseItemStockDAL.GetInstance();
                    clsAddItemStockBizActionVO nvo2 = new clsAddItemStockBizActionVO {
                        Details = receivedItemDetails.StockDetails
                    };
                    nvo2.Details.ID = 0L;
                    nvo2 = (clsAddItemStockBizActionVO) instance.Add(nvo2, userVO, myConnection, transaction);
                    if (nvo2.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    receivedItemDetails.StockDetails.ID = nvo2.Details.ID;
                }
                if ((receivedItemDetails.IsIndent == 0) || (receivedItemDetails.IsIndent == 1))
                {
                    DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_UpdateIndentStatusFromReceiveIssue");
                    command4.Connection = myConnection;
                    this.dbServer.AddInParameter(command4, "ReceivedID", DbType.Int64, nvo.ReceivedItemDetails.ReceivedID);
                    this.dbServer.AddInParameter(command4, "ReceivedUnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                    this.dbServer.ExecuteNonQuery(command4, transaction);
                }
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(userVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                if (nvo.SuccessStatus != 10)
                {
                    nvo.SuccessStatus = -1;
                }
                nvo.ReceivedItemDetails = null;
            }
            finally
            {
                myConnection.Close();
            }
            return nvo;
        }

        public override IValueObject GetItemListByIssueReceivedId(IValueObject valueObject, clsUserVO userVO)
        {
            clsGetItemListByIssueReceivedIdBizActionVO nvo = valueObject as clsGetItemListByIssueReceivedIdBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemListByReceivedId");
                this.dbServer.AddInParameter(storedProcCommand, "ReceivedId", DbType.Int64, nvo.ReceivedId);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemList == null)
                    {
                        nvo.ItemList = new List<clsReceivedItemDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsReceivedItemDetailsVO item = new clsReceivedItemDetailsVO {
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            BatchId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchId"]))),
                            ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]),
                            IssueQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IssueQty"])),
                            IssuedUOM = Convert.ToString(DALHelper.HandleDBNull(reader["IssuedUOM"])),
                            ReceivedQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ReceiveddQuantity"])),
                            ReceivedUOM = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedUOM"])),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            ItemTotalAmount = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemTotalAmount"]))),
                            ReceiveItemTotalAmount = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemTotalAmount"]))),
                            ItemVATAmount = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemVATAmount"]))),
                            ItemVATPercentage = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPercentage"]))),
                            MRP = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["BatchMRP"]))),
                            MRPAmt = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRPAmount"]))),
                            PurchaseRate = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"]))),
                            Rack = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"])),
                            Shelf = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"])),
                            Bin = Convert.ToString(DALHelper.HandleDBNull(reader["Container"])),
                            BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])),
                            GRNNo = Convert.ToString(DALHelper.HandleDBNull(reader["GRNNo"])),
                            SupplierName = Convert.ToString(DALHelper.HandleDBNull(reader["SupplierName"])),
                            BarCode = Convert.ToString(DALHelper.HandleDBNull(reader["BarCode"]))
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

        public override IValueObject GetReceivedList(IValueObject valueObject, clsUserVO userVO)
        {
            clsGetReceivedListBizActionVO nvo = valueObject as clsGetReceivedListBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetReceivedList");
                if ((nvo.MRNo != null) && (nvo.MRNo.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MrNo", DbType.String, "%" + nvo.MRNo + "%");
                }
                if ((nvo.PatientName != null) && (nvo.PatientName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "PatientName", DbType.String, nvo.PatientName + "%");
                }
                this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.String, nvo.UserId);
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
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsReceivedListVO item = new clsReceivedListVO {
                            ReceivedDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["ReceivedDate"]))),
                            ReceivedFromStoreId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedFromStoreId"]))),
                            ReceivedFromStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedFromStoreName"])),
                            ReceivedId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedId"]))),
                            ReceivedNumber = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedNumber"])),
                            ReceivedToStoreId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedToStoreId"]))),
                            ReceivedToStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedToStoreName"])),
                            TotalAmount = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalAmount"]))),
                            TotalItems = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalItems"]))),
                            IssueId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["IssueId"]))),
                            IssueNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IssueNumber"])),
                            Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"])),
                            UnitID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]))),
                            MRNO = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"])),
                            PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]))
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

        public override IValueObject GetReceivedListForGRNToQS(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetReceivedListBizActionVO nvo = valueObject as clsGetReceivedListBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetReceivedListForGRNToQS");
                this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.String, nvo.UserId);
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
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "IsForReceiveGRNToQS", DbType.Boolean, nvo.IsForReceiveGRNToQS);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsReceivedListVO item = new clsReceivedListVO {
                            ReceivedDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["ReceivedDate"]))),
                            ReceivedFromStoreId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedFromStoreId"]))),
                            ReceivedFromStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedFromStoreName"])),
                            ReceivedId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedId"]))),
                            ReceivedNumber = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedNumber"])),
                            ReceivedToStoreId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedToStoreId"]))),
                            ReceivedToStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedToStoreName"])),
                            TotalAmount = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalAmount"]))),
                            TotalItems = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalItems"]))),
                            IssueId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["IssueId"]))),
                            IssueNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IssueNumber"])),
                            Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"])),
                            UnitID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])))
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

        public override IValueObject GetReceivedListQS(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetReceivedListBizActionVO nvo = valueObject as clsGetReceivedListBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetReceivedListQS");
                this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.String, nvo.UserId);
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
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "IsQSOnly", DbType.Boolean, nvo.IsQSOnly);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsReceivedListVO item = new clsReceivedListVO {
                            ReceivedDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["ReceivedDate"]))),
                            ReceivedFromStoreId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedFromStoreId"]))),
                            ReceivedFromStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedFromStoreName"])),
                            ReceivedId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedId"]))),
                            ReceivedNumber = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedNumber"])),
                            ReceivedToStoreId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedToStoreId"]))),
                            ReceivedToStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedToStoreName"])),
                            TotalAmount = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalAmount"]))),
                            TotalItems = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalItems"]))),
                            IssueId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["IssueId"]))),
                            IssueNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IssueNumber"])),
                            Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"])),
                            UnitID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])))
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

