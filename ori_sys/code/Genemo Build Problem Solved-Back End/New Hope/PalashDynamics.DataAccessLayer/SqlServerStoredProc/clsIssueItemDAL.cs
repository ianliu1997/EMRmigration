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

    internal class clsIssueItemDAL : clsBaseIssueItemDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsIssueItemDAL()
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

        public override IValueObject AddIssueItemToStore(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddIssueItemToStoreBizActionVO nvo = valueObject as clsAddIssueItemToStoreBizActionVO;
            DbConnection myConnection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            try
            {
                myConnection.Open();
                transaction = myConnection.BeginTransaction();
                clsIssueItemVO issueItemDetails = nvo.IssueItemDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddIssueItemToStore");
                storedProcCommand.Connection = myConnection;
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, issueItemDetails.LinkServer);
                if (issueItemDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, issueItemDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "IndentID", DbType.Int64, issueItemDetails.IndentID);
                if (DALHelper.HandleDBNull(issueItemDetails.IndentUnitID) == null)
                {
                    issueItemDetails.IndentUnitID = 0L;
                }
                this.dbServer.AddInParameter(storedProcCommand, "IndentUnitID", DbType.Int64, issueItemDetails.IndentUnitID);
                if (DALHelper.IsValidDateRangeDB(new DateTime?(issueItemDetails.IssueDate)))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IssueDate", DbType.DateTime, issueItemDetails.IssueDate);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IssueDate", DbType.DateTime, null);
                }
                this.dbServer.AddInParameter(storedProcCommand, "IssueFromStoreId", DbType.Int64, issueItemDetails.IssueFromStoreId);
                this.dbServer.AddParameter(storedProcCommand, "IssueNumber", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");
                this.dbServer.AddInParameter(storedProcCommand, "IssueToStoreId", DbType.Int64, issueItemDetails.IssueToStoreId);
                this.dbServer.AddInParameter(storedProcCommand, "IssuedByID", DbType.Int64, issueItemDetails.ReceivedById);
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, issueItemDetails.Remark);
                this.dbServer.AddInParameter(storedProcCommand, "TotalAmount", DbType.Decimal, issueItemDetails.TotalAmount);
                this.dbServer.AddInParameter(storedProcCommand, "TotalItems", DbType.Decimal, issueItemDetails.TotalItems);
                this.dbServer.AddInParameter(storedProcCommand, "TotalVATAmount", DbType.Decimal, issueItemDetails.TotalVATAmount);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "LoginUserId", DbType.Int64, userVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "TotalTaxAmount", DbType.Decimal, issueItemDetails.TotalTaxAmount);
                this.dbServer.AddInParameter(storedProcCommand, "IsIndent", DbType.Int32, issueItemDetails.IsIndent);
                if (issueItemDetails.IsIndent == 1)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "InventoryIndentType", DbType.String, InventoryIndentType.Indent);
                }
                else if (issueItemDetails.IsIndent == 0)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "InventoryIndentType", DbType.String, InventoryIndentType.PurchaseRequisition);
                }
                else if (issueItemDetails.IsIndent == 2)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineExpired);
                }
                else if (issueItemDetails.IsIndent == 3)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineDOS);
                }
                else if (issueItemDetails.IsIndent == 4)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineGRNReturn);
                }
                else if (issueItemDetails.IsIndent == 6)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "InventoryIndentType", DbType.String, InventoryIndentType.Direct);
                }
                this.dbServer.AddInParameter(storedProcCommand, "IsApproved", DbType.Boolean, issueItemDetails.IsApprovedDirect);
                this.dbServer.AddInParameter(storedProcCommand, "ReasonForIssue", DbType.Int64, issueItemDetails.ReasonForIssue);
                this.dbServer.AddInParameter(storedProcCommand, "IsFromStoreQuarantine", DbType.Boolean, issueItemDetails.IsFromStoreQuarantine);
                this.dbServer.AddInParameter(storedProcCommand, "IsToStoreQuarantine", DbType.Boolean, issueItemDetails.IsToStoreQuarantine);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, issueItemDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, issueItemDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsPatientIndent", DbType.Boolean, issueItemDetails.IsAgainstPatient);
                this.dbServer.AddInParameter(storedProcCommand, "MachineName", DbType.String, userVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "IssueID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, issueItemDetails.IssueID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.IssueItemDetails.IssueID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                nvo.IssueItemDetails.IssueNumber = (string) this.dbServer.GetParameterValue(storedProcCommand, "IssueNumber");
                int indentStatus = 0;
                decimal? nullable2 = 0M;
                decimal? nullable3 = 0M;
                foreach (clsIssueItemDetailsVO svo in nvo.IssueItemDetails.IssueItemDetailsList)
                {
                    decimal? nullable13;
                    decimal? nullable14;
                    decimal? nullable4 = nullable2;
                    decimal? balanceQty = svo.BalanceQty;
                    if ((nullable4 != null) & (balanceQty != null))
                    {
                        nullable13 = new decimal?(nullable4.GetValueOrDefault() + balanceQty.GetValueOrDefault());
                    }
                    else
                    {
                        nullable13 = null;
                    }
                    nullable2 = nullable13;
                    decimal? nullable7 = nullable3;
                    decimal issueQty = svo.IssueQty;
                    if (nullable7 != null)
                    {
                        nullable14 = new decimal?(nullable7.GetValueOrDefault() + issueQty);
                    }
                    else
                    {
                        nullable14 = null;
                    }
                    nullable3 = nullable14;
                }
                decimal? nullable9 = nullable2;
                decimal? nullable10 = nullable3;
                if ((nullable9 != null) & (nullable10 != null))
                {
                    decimal? nullable1 = new decimal?(nullable9.GetValueOrDefault() - nullable10.GetValueOrDefault());
                }
                foreach (clsIssueItemDetailsVO svo2 in nvo.IssueItemDetails.IssueItemDetailsList)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddIssueItemListToStore");
                    command2.Connection = myConnection;
                    this.dbServer.AddInParameter(command2, "LinkServer", DbType.String, issueItemDetails.LinkServer);
                    if (issueItemDetails.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(command2, "LinkServerAlias", DbType.String, issueItemDetails.LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddInParameter(command2, "BatchId", DbType.Int64, svo2.BatchId);
                    this.dbServer.AddInParameter(command2, "ConversionFactor", DbType.Single, svo2.ConversionFactor);
                    if (DALHelper.IsValidDateRangeDB(svo2.ExpiryDate))
                    {
                        this.dbServer.AddInParameter(command2, "ExpiryDate", DbType.DateTime, svo2.ExpiryDate);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(command2, "ExpiryDate", DbType.DateTime, null);
                    }
                    this.dbServer.AddInParameter(command2, "IndentQty", DbType.Decimal, svo2.IndentQty);
                    this.dbServer.AddInParameter(command2, "IssueQty", DbType.Decimal, svo2.IssueQty);
                    this.dbServer.AddInParameter(command2, "ItemId", DbType.Int64, svo2.ItemId);
                    this.dbServer.AddInParameter(command2, "ItemVATAmount", DbType.Decimal, svo2.ItemVATAmount);
                    this.dbServer.AddInParameter(command2, "ItemVATPercentage", DbType.Decimal, svo2.ItemVATPercentage);
                    this.dbServer.AddInParameter(command2, "MRP", DbType.Decimal, svo2.MainMRP);
                    this.dbServer.AddInParameter(command2, "PurchaseRate", DbType.Decimal, svo2.MainRate);
                    this.dbServer.AddInParameter(command2, "TransactionUOMID", DbType.Int64, svo2.SelectedUOM.ID);
                    this.dbServer.AddInParameter(command2, "StockUOMID", DbType.Int64, svo2.SUOMID);
                    this.dbServer.AddInParameter(command2, "StockCF", DbType.Single, svo2.ConversionFactor);
                    this.dbServer.AddInParameter(command2, "StockingQuantity", DbType.Decimal, svo2.StockingQuantity);
                    this.dbServer.AddInParameter(command2, "BaseUOMID", DbType.Int64, svo2.BaseUOMID);
                    this.dbServer.AddInParameter(command2, "BaseCF", DbType.Single, svo2.BaseConversionFactor);
                    this.dbServer.AddInParameter(command2, "BaseQuantity", DbType.Decimal, svo2.BaseQuantity);
                    this.dbServer.AddInParameter(command2, "IndentUOMID", DbType.Int64, svo2.UOMID);
                    this.dbServer.AddInParameter(command2, "BalanceQty", DbType.Decimal, svo2.BaseQuantity);
                    this.dbServer.AddInParameter(command2, "IndentId", DbType.Int64, issueItemDetails.IndentID);
                    this.dbServer.AddInParameter(command2, "IndentUnitID", DbType.Int64, issueItemDetails.IndentUnitID);
                    this.dbServer.AddInParameter(command2, "GRNID", DbType.Int64, svo2.GRNID);
                    this.dbServer.AddInParameter(command2, "GRNUnitID", DbType.Int64, svo2.GRNUnitID);
                    this.dbServer.AddInParameter(command2, "GRNDetailID", DbType.Int64, svo2.GRNDetailID);
                    this.dbServer.AddInParameter(command2, "GRNDetailUnitID", DbType.Int64, svo2.GRNDetailUnitID);
                    this.dbServer.AddInParameter(command2, "othertaxApplicableon", DbType.Int16, svo2.OtherGRNItemTaxApplicationOn);
                    this.dbServer.AddInParameter(command2, "otherTaxType", DbType.Int64, svo2.OtherGRNItemTaxType);
                    this.dbServer.AddInParameter(command2, "VatApplicableon", DbType.Int64, svo2.GRNItemVatApplicationOn);
                    this.dbServer.AddInParameter(command2, "Vattype", DbType.Int64, svo2.GRNItemVatType);
                    this.dbServer.AddInParameter(command2, "InclusiveOfTax", DbType.Boolean, svo2.InclusiveOfTax);
                    this.dbServer.AddInParameter(command2, "ItemTaxAmount", DbType.Decimal, svo2.ItemTaxAmount);
                    this.dbServer.AddInParameter(command2, "ItemTaxPercentage", DbType.Decimal, svo2.ItemTaxPercentage);
                    this.dbServer.AddInParameter(command2, "MRPAmount", DbType.Decimal, svo2.MRPAmt);
                    this.dbServer.AddInParameter(command2, "PurchaseRateAmount", DbType.Decimal, svo2.PurchaseRateAmt);
                    this.dbServer.AddInParameter(command2, "IsIndent", DbType.Int32, svo2.IsIndent);
                    if (svo2.IsIndent == 1)
                    {
                        this.dbServer.AddInParameter(command2, "InventoryIndentType", DbType.String, InventoryIndentType.Indent);
                        this.dbServer.AddInParameter(command2, "ItemTotalAmount", DbType.Decimal, svo2.ItemTotalAmount);
                    }
                    else if (svo2.IsIndent == 0)
                    {
                        this.dbServer.AddInParameter(command2, "InventoryIndentType", DbType.String, InventoryIndentType.PurchaseRequisition);
                        this.dbServer.AddInParameter(command2, "ItemTotalAmount", DbType.Decimal, svo2.ItemTotalAmount);
                    }
                    else if (svo2.IsIndent == 2)
                    {
                        this.dbServer.AddInParameter(command2, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineExpired);
                        this.dbServer.AddInParameter(command2, "ItemTotalAmount", DbType.Decimal, svo2.NetAmount);
                    }
                    else if (issueItemDetails.IsIndent == 3)
                    {
                        this.dbServer.AddInParameter(command2, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineDOS);
                        this.dbServer.AddInParameter(command2, "ItemTotalAmount", DbType.Decimal, svo2.NetAmount);
                    }
                    else if (issueItemDetails.IsIndent == 4)
                    {
                        this.dbServer.AddInParameter(command2, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineGRNReturn);
                        this.dbServer.AddInParameter(command2, "ItemTotalAmount", DbType.Decimal, svo2.NetAmount);
                    }
                    else if (issueItemDetails.IsIndent == 6)
                    {
                        this.dbServer.AddInParameter(command2, "InventoryIndentType", DbType.String, InventoryIndentType.Direct);
                        this.dbServer.AddInParameter(command2, "ItemTotalAmount", DbType.Decimal, svo2.ItemTotalAmount);
                    }
                    this.dbServer.AddInParameter(command2, "ReasonForIssue", DbType.Int64, svo2.ReasonForIssue);
                    this.dbServer.AddInParameter(command2, "UnitId", DbType.Decimal, userVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "IssueFromStoreID", DbType.Int64, issueItemDetails.IssueFromStoreId);
                    this.dbServer.AddInParameter(command2, "IssueToStoreID", DbType.Int64, issueItemDetails.IssueToStoreId);
                    this.dbServer.AddInParameter(command2, "DepartmentID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command2, "TransactionTypeID", DbType.Int32, InventoryTransactionType.Issue);
                    this.dbServer.AddInParameter(storedProcCommand, "LoginUserId", DbType.Int64, userVO.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "MachineName", DbType.String, userVO.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command2, "IssueId", DbType.Int64, nvo.IssueItemDetails.IssueID);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddOutParameter(command2, "ID", DbType.Int64, 0);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                    if (nvo.SuccessStatus == -10)
                    {
                        throw new Exception();
                    }
                    if (svo2.IndentQty == null)
                    {
                        if ((svo2.GRNQty != null) && (svo2.IsIndent == 4))
                        {
                            DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_UpdateGRNQSPendingQty");
                            command3.Connection = myConnection;
                            this.dbServer.AddInParameter(command3, "GRNID", DbType.Int64, svo2.GRNID);
                            this.dbServer.AddInParameter(command3, "GRNUnitID", DbType.Int64, svo2.GRNUnitID);
                            this.dbServer.AddInParameter(command3, "GRNDetailID", DbType.Int64, svo2.GRNDetailID);
                            this.dbServer.AddInParameter(command3, "GRNDetailUnitID", DbType.Int64, svo2.GRNDetailUnitID);
                            this.dbServer.AddInParameter(command3, "BalanceQty", DbType.Decimal, svo2.BaseQuantity);
                            this.dbServer.AddInParameter(command3, "IsFreeItem", DbType.Boolean, svo2.IsGRNFreeItem);
                            this.dbServer.ExecuteNonQuery(command3, transaction);
                        }
                    }
                    else
                    {
                        decimal? balanceQty = new decimal?(svo2.IssueQty * Convert.ToDecimal(svo2.BaseConversionFactor));
                        if (svo2.IsIndent == 0)
                        {
                            indentStatus = 0;
                        }
                        else if (svo2.IsIndent == 1)
                        {
                            indentStatus = 1;
                        }
                        this.UpdateIndentStatus(indentStatus, issueItemDetails.IndentID, new long?(svo2.IndentUnitID), balanceQty, svo2.IndentDetailsID);
                    }
                    svo2.StockDetails.BatchID = svo2.BatchId;
                    svo2.StockDetails.OperationType = InventoryStockOperationType.Subtraction;
                    svo2.StockDetails.ItemID = svo2.ItemId;
                    svo2.StockDetails.TransactionTypeID = InventoryTransactionType.Issue;
                    svo2.StockDetails.TransactionID = nvo.IssueItemDetails.IssueID;
                    svo2.StockDetails.TransactionQuantity = svo2.BaseQuantity;
                    svo2.StockDetails.Date = issueItemDetails.IssueDate;
                    svo2.StockDetails.Time = DateTime.Now;
                    svo2.StockDetails.StoreID = issueItemDetails.IssueFromStoreId;
                    svo2.StockDetails.StockingQuantity = svo2.StockingQuantity;
                    svo2.StockDetails.BaseUOMID = svo2.BaseUOMID;
                    svo2.StockDetails.BaseConversionFactor = svo2.BaseConversionFactor;
                    svo2.StockDetails.SUOMID = svo2.SUOMID;
                    svo2.StockDetails.ConversionFactor = Convert.ToDouble(svo2.ConversionFactor);
                    svo2.StockDetails.SelectedUOM.ID = svo2.SelectedUOM.ID;
                    svo2.StockDetails.InputTransactionQuantity = Convert.ToSingle(svo2.IssueQty);
                    svo2.StockDetails.ExpiryDate = svo2.ExpiryDate;
                    svo2.StockDetails.IsFromOpeningBalance = true;
                    svo2.StockDetails.MRP = Convert.ToDouble(svo2.MainMRP);
                    svo2.StockDetails.PurchaseRate = Convert.ToDouble(svo2.MainRate);
                    clsBaseItemStockDAL instance = clsBaseItemStockDAL.GetInstance();
                    clsAddItemStockBizActionVO nvo2 = new clsAddItemStockBizActionVO {
                        Details = svo2.StockDetails
                    };
                    nvo2.Details.ID = 0L;
                    nvo2 = (clsAddItemStockBizActionVO) instance.Add(nvo2, userVO, myConnection, transaction);
                    if (nvo2.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    svo2.StockDetails.ID = nvo2.Details.ID;
                }
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(userVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                if (nvo.SuccessStatus != -10)
                {
                    nvo.SuccessStatus = -1;
                }
                nvo.IssueItemDetails = null;
            }
            finally
            {
                myConnection.Close();
                transaction = null;
            }
            return nvo;
        }

        public IValueObject GetBatchListForIndentItemIdForIsueItem(IValueObject valueObject, clsUserVO userVO)
        {
            GetItemListByIndentIdForIssueItemBizActionVO nvo = valueObject as GetItemListByIndentIdForIssueItemBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetBatchListForIndentItemIdForIssueItem");
                this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, nvo.ItemID);
                this.dbServer.AddInParameter(storedProcCommand, "IssueFromStoreId", DbType.String, nvo.IssueFromStoreId);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.IssueItemDetailsList == null)
                    {
                        nvo.IssueItemDetailsList = new List<clsIssueItemDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsIssueItemDetailsVO item = new clsIssueItemDetailsVO {
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            BatchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchId"])),
                            ConversionFactor = new float?((float) Convert.ToInt64(DALHelper.HandleDBNull(reader["ConversionFactor"]))),
                            ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            ItemVATPercentage = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPercentage"]))),
                            MRP = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["BatchMRP"]))),
                            NetPayBeforeVATAmount = 0M,
                            TotalForVAT = 0M,
                            UnverifiedStock = 0M,
                            VATInclusive = 0M,
                            AvailableStock = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["AvailableStock"]))),
                            AvailableStockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["AvailableStockUOM"])),
                            PurchaseRate = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"])))
                        };
                        nvo.IssueItemDetailsList.Add(item);
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

        public override IValueObject GetGRNToQSIssueList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIssueListBizActionVO nvo = valueObject as clsGetIssueListBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetGRNToQSIssueList");
                this.dbServer.AddInParameter(storedProcCommand, "IsForGRNQS", DbType.Boolean, nvo.IsForGRNQS);
                this.dbServer.AddInParameter(storedProcCommand, "IndentCriteria", DbType.Int32, nvo.IndentCriteria);
                this.dbServer.AddInParameter(storedProcCommand, "IssueNumber", DbType.String, nvo.IssueDetailsVO.IssueNumber);
                if (DALHelper.IsValidDateRangeDB(nvo.IssueFromDate))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IssueFromDate", DbType.DateTime, nvo.IssueFromDate.Value);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IssueFromDate", DbType.DateTime, null);
                }
                if (DALHelper.IsValidDateRangeDB(nvo.IssueToDate))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IssueToDate", DbType.DateTime, nvo.IssueToDate.Value);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IssueToDate", DbType.DateTime, null);
                }
                this.dbServer.AddInParameter(storedProcCommand, "IssueFromStoreId", DbType.Int64, nvo.IssueFromStoreId);
                this.dbServer.AddInParameter(storedProcCommand, "IssueToStoreId", DbType.Int64, nvo.IssueToStoreId);
                if (nvo.flagReceiveIssue)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, 0);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.InputSortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIssueListVO item = new clsIssueListVO {
                            IssueDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["IssueDate"]))),
                            IssueFromStoreId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["IssueFromStoreId"]))),
                            IssueFromStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["IssueFromStoreName"])),
                            IssueId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["IssueId"]))),
                            IssueNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IssueNumber"])),
                            IssueToStoreId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["IssueToStoreId"]))),
                            IssueToStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["IssueToStoreName"])),
                            ReceivedById = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["IssuedByID"]))),
                            ReceivedByName = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedByName"])),
                            TotalAmount = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalAmount"]))),
                            TotalItems = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalItems"]))),
                            IndentId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentId"]))),
                            IndentNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IndentNumber"])),
                            Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"])),
                            IssueUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            IsIndent = Convert.ToInt32(DALHelper.HandleDBNull(reader["IsIndent"])),
                            IsFromStoreQuarantine = new bool?(Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFromStoreQuarantine"]))),
                            IsToStoreQuarantine = new bool?(Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsToStoreQuarantine"]))),
                            ReasonForIssueName = Convert.ToString(DALHelper.HandleDBNull(reader["ReasonForIssueName"]))
                        };
                        nvo.IssueList.Add(item);
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
            return nvo;
        }

        public override IValueObject GetIndentListBySupplier(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIndentListBySupplierBizActionVO nvo = valueObject as clsGetIndentListBySupplierBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIndentListBySupplier");
                this.dbServer.AddInParameter(storedProcCommand, "FromIndentStoreId", DbType.String, nvo.FromIndentStoreId);
                this.dbServer.AddInParameter(storedProcCommand, "ToIndentStoreId", DbType.String, nvo.ToIndentStoreId);
                DateTime fromDate = nvo.FromDate;
                if (((int) nvo.FromDate.ToOADate()) != 0)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                }
                DateTime toDate = nvo.ToDate;
                if (((int) nvo.ToDate.ToOADate()) != 0)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                }
                this.dbServer.AddInParameter(storedProcCommand, "IndentNumber", DbType.String, nvo.SIndentNumber);
                this.dbServer.AddInParameter(storedProcCommand, "ItemName", DbType.String, nvo.SItemName);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierID", DbType.Int64, nvo.SSupplierID);
                this.dbServer.AddInParameter(storedProcCommand, "Freezed", DbType.Boolean, nvo.Freezed);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsItemListByIndentId item = new clsItemListByIndentId {
                            IndentId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]))),
                            IndentUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            IndentNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IndentNumber"])),
                            IndentQty = (Convert.ToDecimal(DALHelper.HandleDBNull(reader["BaseCF"])) <= 0M) ? new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["RequiredQuantity"]))) : new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["RequiredQuantity"])) / Convert.ToDecimal(DALHelper.HandleDBNull(reader["BaseCF"]))),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            ItemId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]))),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            PUM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"])),
                            BalanceQty = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["PendingQuantity"]))),
                            IssuePendingQuantity = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["IssuePendingQuantity"]))),
                            Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]))
                        };
                        if (!nvo.IssueIndentFlag)
                        {
                            decimal? issuePendingQuantity = item.IssuePendingQuantity;
                            if ((issuePendingQuantity.GetValueOrDefault() <= 0M) && (issuePendingQuantity != null))
                            {
                                continue;
                            }
                        }
                        item.IssueQty = new decimal?(Convert.ToDecimal("0"));
                        item.IndentDetailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentDetailsID"]));
                        item.IndentDetailsUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentDetailsUnitID"]));
                        item.ToStoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToStoreID"]));
                        item.FromStoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FromStoreID"]));
                        item.PurchaseRate = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"])));
                        item.VATPer = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPer"])));
                        item.ItemVatPer = (decimal) DALHelper.HandleDBNull(reader["othertax"]);
                        item.ItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["taxtype"]));
                        item.ItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["applicableon"]));
                        item.ItemOtherTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxtype"]));
                        item.OtherItemApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherapplicableon"]));
                        item.MRP = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"])));
                        item.Supplier = Convert.ToString(DALHelper.HandleDBNull(reader["Supplier"]));
                        item.SupplierID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SupplierID"]));
                        item.ItemGroup = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemGroup"]));
                        item.ItemCategory = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"]));
                        item.SelectedUOM.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UOM"]));
                        item.UOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UOM"]));
                        item.TransUOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransUOM"]));
                        item.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUOMID"]));
                        item.SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SUOM"]));
                        item.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"]));
                        item.BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"]));
                        item.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"]));
                        item.BaseMRP = Convert.ToSingle(DALHelper.HandleDBNull(reader["MRP"]));
                        item.BaseRate = Convert.ToSingle(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        item.BaseQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["RequiredQuantity"]));
                        item.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        item.PUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PUMID"]));
                        item.PurchaseToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["PTBCF"]));
                        item.StockingToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["STBCF"]));
                        item.IsItemBlock = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsItemBlock"]));
                        item.POApprItemQty = Convert.ToSingle(DALHelper.HandleDBNull(reader["POApprItemQty"]));
                        item.POPendingItemQty = Convert.ToSingle(DALHelper.HandleDBNull(reader["POPendingItemQty"]));
                        item.SGSTPercent = (decimal) DALHelper.HandleDBNull(reader["SGSTTax"]);
                        item.CGSTPercent = (decimal) DALHelper.HandleDBNull(reader["CGSTTax"]);
                        item.IGSTPercent = (decimal) DALHelper.HandleDBNull(reader["IGSTTax"]);
                        item.HSNCodes = Convert.ToString(DALHelper.HandleDBNull(reader["HSNCode"]));
                        item.SGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTTaxType"]));
                        item.SGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTApplicableOn"]));
                        item.CGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTTaxType"]));
                        item.CGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTApplicableOn"]));
                        item.IGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTTaxType"]));
                        item.IGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTApplicableOn"]));
                        item.TotalBatchAvailableStock = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalBatchAvailableStock"]));
                        nvo.ItemList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRowCount = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
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

        public override IValueObject GetIssueList(IValueObject valueObject, clsUserVO userVO)
        {
            clsGetIssueListBizActionVO nvo = valueObject as clsGetIssueListBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand;
                int? indentCriteria = nvo.IndentCriteria;
                if ((indentCriteria.GetValueOrDefault() == 4) && (indentCriteria != null))
                {
                    nvo.IndentCriteria = 4;
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIssueListForReturn");
                    this.dbServer.AddInParameter(storedProcCommand, "IsFromReceiveIssue", DbType.Boolean, nvo.IsFromReceiveIssue);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientunitID", DbType.Int64, nvo.PatientunitID);
                    this.dbServer.AddInParameter(storedProcCommand, "IsAgainstPatient", DbType.Boolean, nvo.IsAgainstPatient);
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIssueList");
                    this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, nvo.UserID);
                    this.dbServer.AddInParameter(storedProcCommand, "intIsIndent", DbType.Int32, nvo.intIsIndent);
                    if ((nvo.MRNo != null) && (nvo.MRNo.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "MrNo", DbType.String, "%" + nvo.MRNo + "%");
                    }
                    if ((nvo.PatientName != null) && (nvo.PatientName.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "PatientName", DbType.String, nvo.PatientName + "%");
                    }
                }
                this.dbServer.AddInParameter(storedProcCommand, "IndentCriteria", DbType.Int32, nvo.IndentCriteria);
                this.dbServer.AddInParameter(storedProcCommand, "IssueNumber", DbType.String, nvo.IssueDetailsVO.IssueNumber);
                if (DALHelper.IsValidDateRangeDB(nvo.IssueFromDate))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IssueFromDate", DbType.DateTime, nvo.IssueFromDate.Value);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IssueFromDate", DbType.DateTime, null);
                }
                if (DALHelper.IsValidDateRangeDB(nvo.IssueToDate))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IssueToDate", DbType.DateTime, nvo.IssueToDate.Value);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IssueToDate", DbType.DateTime, null);
                }
                this.dbServer.AddInParameter(storedProcCommand, "IssueFromStoreId", DbType.Int64, nvo.IssueFromStoreId);
                this.dbServer.AddInParameter(storedProcCommand, "IssueToStoreId", DbType.Int64, nvo.IssueToStoreId);
                if (nvo.flagReceiveIssue)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, 0);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.InputSortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIssueListVO item = new clsIssueListVO {
                            IssueDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["IssueDate"]))),
                            IssueFromStoreId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["IssueFromStoreId"]))),
                            IssueFromStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["IssueFromStoreName"])),
                            IssueId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["IssueId"]))),
                            IssueNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IssueNumber"])),
                            IssueToStoreId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["IssueToStoreId"]))),
                            IssueToStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["IssueToStoreName"])),
                            ReceivedById = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["IssuedByID"]))),
                            ReceivedByName = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedByName"])),
                            TotalAmount = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalAmount"]))),
                            TotalItems = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalItems"]))),
                            IndentId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentId"]))),
                            IndentNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IndentNumber"])),
                            Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"])),
                            IssueUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            IsIndent = Convert.ToInt32(DALHelper.HandleDBNull(reader["IsIndent"])),
                            IsFromStoreQuarantine = new bool?(Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFromStoreQuarantine"]))),
                            IsToStoreQuarantine = new bool?(Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsToStoreQuarantine"])))
                        };
                        int? nullable4 = nvo.IndentCriteria;
                        if ((nullable4.GetValueOrDefault() == 4) && (nullable4 != null))
                        {
                            item.IsAgainstPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPatientIndent"]));
                            item.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                            item.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        }
                        item.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"]));
                        item.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        int? nullable5 = nvo.IndentCriteria;
                        if ((nullable5.GetValueOrDefault() != 4) || (nullable5 == null))
                        {
                            item.IsApproved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsApprovedDirect"]));
                        }
                        nvo.IssueList.Add(item);
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
            return nvo;
        }

        public override IValueObject GetIssueListQS(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIssueListBizActionVO nvo = valueObject as clsGetIssueListBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand;
                int? indentCriteria = nvo.IndentCriteria;
                if ((indentCriteria.GetValueOrDefault() != 4) || (indentCriteria == null))
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIssueList");
                    this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, nvo.UserID);
                }
                else
                {
                    nvo.IndentCriteria = 4;
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIssueListForReturnQS");
                    this.dbServer.AddInParameter(storedProcCommand, "IsFromReceiveIssue", DbType.Boolean, nvo.IsFromReceiveIssue);
                    this.dbServer.AddInParameter(storedProcCommand, "IsQSOnly", DbType.Boolean, nvo.IsQSOnly);
                }
                this.dbServer.AddInParameter(storedProcCommand, "IndentCriteria", DbType.Int32, nvo.IndentCriteria);
                this.dbServer.AddInParameter(storedProcCommand, "IssueNumber", DbType.String, nvo.IssueDetailsVO.IssueNumber);
                if (DALHelper.IsValidDateRangeDB(nvo.IssueFromDate))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IssueFromDate", DbType.DateTime, nvo.IssueFromDate.Value);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IssueFromDate", DbType.DateTime, null);
                }
                if (DALHelper.IsValidDateRangeDB(nvo.IssueToDate))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IssueToDate", DbType.DateTime, nvo.IssueToDate.Value);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IssueToDate", DbType.DateTime, null);
                }
                this.dbServer.AddInParameter(storedProcCommand, "IssueFromStoreId", DbType.Int64, nvo.IssueFromStoreId);
                this.dbServer.AddInParameter(storedProcCommand, "IssueToStoreId", DbType.Int64, nvo.IssueToStoreId);
                if (nvo.flagReceiveIssue)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, 0);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.InputSortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIssueListVO item = new clsIssueListVO {
                            IssueDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["IssueDate"]))),
                            IssueFromStoreId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["IssueFromStoreId"]))),
                            IssueFromStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["IssueFromStoreName"])),
                            IssueId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["IssueId"]))),
                            IssueNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IssueNumber"])),
                            IssueToStoreId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["IssueToStoreId"]))),
                            IssueToStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["IssueToStoreName"])),
                            ReceivedById = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["IssuedByID"]))),
                            ReceivedByName = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedByName"])),
                            TotalAmount = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalAmount"]))),
                            TotalItems = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalItems"]))),
                            IndentId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentId"]))),
                            IndentNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IndentNumber"])),
                            Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"])),
                            IssueUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            IsIndent = Convert.ToInt32(DALHelper.HandleDBNull(reader["IsIndent"])),
                            IsFromStoreQuarantine = new bool?(Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFromStoreQuarantine"]))),
                            IsToStoreQuarantine = new bool?(Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsToStoreQuarantine"])))
                        };
                        int? nullable4 = nvo.IndentCriteria;
                        if ((nullable4.GetValueOrDefault() == 4) && (nullable4 != null))
                        {
                            item.ReasonForIssueName = Convert.ToString(DALHelper.HandleDBNull(reader["ReasonForIssueName"]));
                        }
                        nvo.IssueList.Add(item);
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
            return nvo;
        }

        public override IValueObject GetItemListByIndentIdForIsueItem(IValueObject valueObject, clsUserVO userVO)
        {
            GetItemListByIndentIdForIssueItemBizActionVO nvo = valueObject as GetItemListByIndentIdForIssueItemBizActionVO;
            DbDataReader reader = null;
            try
            {
                if (nvo.TransactionType != InventoryTransactionType.Issue)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemListByIndentIdForIssueItem");
                    this.dbServer.AddInParameter(storedProcCommand, "IndentId", DbType.Int64, nvo.IndentID);
                    this.dbServer.AddInParameter(storedProcCommand, "IssueFromStoreId", DbType.String, nvo.IssueFromStoreId);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.IssueItemDetailsList == null)
                        {
                            nvo.IssueItemDetailsList = new List<clsIssueItemDetailsVO>();
                        }
                        while (reader.Read())
                        {
                            clsIssueItemDetailsVO item = new clsIssueItemDetailsVO {
                                BalanceQty = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["PendingQuantity"]))),
                                BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                                BatchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchId"])),
                                ConversionFactor = new float?((float) Convert.ToInt64(DALHelper.HandleDBNull(reader["ConversionFactor"]))),
                                ExpiryDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["ExpiryDate"]))),
                                IndentQty = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["RequiredQuantity"]))),
                                IssueQty = Convert.ToDecimal("0"),
                                ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                                ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"])),
                                ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                                ItemVATPercentage = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPercentage"]))),
                                MRP = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["BatchMRP"]))),
                                NetPayBeforeVATAmount = 0M,
                                TotalForVAT = 0M,
                                UnverifiedStock = 0M,
                                VATInclusive = 0M,
                                AvailableStock = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["AvailableStock"])))
                            };
                            decimal? availableStock = item.AvailableStock;
                            if ((availableStock.GetValueOrDefault() > 0M) || (availableStock == null))
                            {
                                item.PurchaseRate = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"])));
                                item.IndentUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentUnitID"]));
                                item.IndentDetailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                                item.IndentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentId"]));
                                item.ToStoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToStoreID"]));
                                item.FromStoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FromStoreID"]));
                                nvo.IssueItemDetailsList.Add(item);
                            }
                        }
                    }
                    reader.NextResult();
                }
                else
                {
                    valueObject = this.GetBatchListForIndentItemIdForIsueItem(valueObject, userVO);
                    return valueObject;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }

        public override IValueObject GetItemListByIndentIdSrch(IValueObject valueObject, clsUserVO userVO)
        {
            clsGetItemListByIndentIdSrchBizActionVO nvo = valueObject as clsGetItemListByIndentIdSrchBizActionVO;
            DbDataReader reader = null;
            try
            {
                if (nvo.TransactionType != InventoryTransactionType.Issue)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemListByIndentIdSrch");
                    this.dbServer.AddInParameter(storedProcCommand, "IndentId", DbType.Int64, nvo.IndentId);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            clsItemListByIndentId item = new clsItemListByIndentId {
                                IndentId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentId"]))),
                                IndentNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IndentNumber"])),
                                IndentQty = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["RequiredQuantity"]))),
                                ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                                ItemId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]))),
                                ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                                PUM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"]))
                            };
                            nvo.ItemList.Add(item);
                        }
                    }
                    reader.NextResult();
                }
                else
                {
                    valueObject = this.GetItemListByIndentIdSrch1(valueObject, userVO);
                    return valueObject;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return valueObject;
        }

        private IValueObject GetItemListByIndentIdSrch1(IValueObject valueObject, clsUserVO userVO)
        {
            clsGetItemListByIndentIdSrchBizActionVO nvo = valueObject as clsGetItemListByIndentIdSrchBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemListByIndentIdSrch2");
                this.dbServer.AddInParameter(storedProcCommand, "IndentId", DbType.Int64, nvo.IndentId);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "FromPO", DbType.Boolean, nvo.IssueIndentFlag);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsItemListByIndentId item = new clsItemListByIndentId {
                            IndentId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentId"]))),
                            IndentNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IndentNumber"])),
                            IndentQty = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["Quantity"]))),
                            UOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransactionUOM"])),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            ItemId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]))),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            PUM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"])),
                            BalanceQty = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["PendingQuantity"]))),
                            IssuePendingQuantity = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["IssuePendingQuantity"]))),
                            UOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"])),
                            SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"])),
                            SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SUOM"])),
                            StockConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"])),
                            ConversionFactor = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockCF"])),
                            BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUOMID"])),
                            BaseConversionFactor = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConversionFactor"])),
                            BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"]))
                        };
                        if (!nvo.IssueIndentFlag)
                        {
                            decimal? issuePendingQuantity = item.IssuePendingQuantity;
                            if ((issuePendingQuantity.GetValueOrDefault() <= 0M) && (issuePendingQuantity != null))
                            {
                                continue;
                            }
                        }
                        item.IssueQty = new decimal?(Convert.ToDecimal("0"));
                        item.IndentUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentUnitID"]));
                        item.IndentDetailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        item.ToStoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToStoreID"]));
                        item.FromStoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FromStoreID"]));
                        item.PurchaseRate = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"])));
                        item.VATPer = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPer"])));
                        item.MRP = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"])));
                        item.Supplier = Convert.ToString(DALHelper.HandleDBNull(reader["Supplier"]));
                        item.SupplierID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SupplierID"]));
                        item.IsIndent = Convert.ToInt32(DALHelper.HandleDBNull(reader["IsIndent"]));
                        item.IsItemBlock = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsItemBlock"]));
                        item.ItemVATPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPer"]));
                        item.GRNItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["applicableon"]));
                        item.GRNItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["taxtype"]));
                        item.ItemTaxPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["othertax"]));
                        item.OtherGRNItemTaxApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherapplicableon"]));
                        item.OtherGRNItemTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxtype"]));
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

        public override IValueObject GetItemListByIssueId(IValueObject valueObject, clsUserVO userVO)
        {
            clsGetItemListByIssueIdBizActionVO nvo = valueObject as clsGetItemListByIssueIdBizActionVO;
            DbDataReader reader = null;
            try
            {
                if (nvo.TransactionType != InventoryTransactionType.ReceiveItemReturn)
                {
                    DbCommand command = nvo.flagReceivedIssue ? this.dbServer.GetStoredProcCommand("CIMS_GetItemListByIssueIdForReceive") : this.dbServer.GetStoredProcCommand("CIMS_GetItemListByIssueId");
                    this.dbServer.AddInParameter(command, "IssueId", DbType.Int64, nvo.IssueId);
                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, nvo.UnitID);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        if (nvo.ItemList == null)
                        {
                            nvo.ItemList = new List<clsIssueItemDetailsVO>();
                        }
                        if (nvo.ReturnItemList == null)
                        {
                            nvo.ReturnItemList = new List<clsReturnItemDetailsVO>();
                        }
                        if (nvo.ReceivedItemList == null)
                        {
                            nvo.ReceivedItemList = new List<clsReceivedItemDetailsVO>();
                        }
                        while (reader.Read())
                        {
                            clsIssueItemDetailsVO item = new clsIssueItemDetailsVO {
                                BalanceQty = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["PendingQuantity"]))),
                                BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                                BatchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchId"])),
                                ConversionFactor = new float?((float) Convert.ToInt64(DALHelper.HandleDBNull(reader["ConversionFactor"]))),
                                ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]),
                                IndentQty = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["RequiredQuantity"]))),
                                IssueQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IssuedQuantity"])),
                                ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                                ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"])),
                                ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                                ItemTotalAmount = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemTotalAmount"]))),
                                ItemTotalCost = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemTotalAmount"]))),
                                ItemVATPercentage = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPercentage"]))),
                                MRP = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["BatchMRP"])))
                            };
                            if (!nvo.flagReceivedIssue)
                            {
                                item.MRPAmt = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRPAmount"])));
                            }
                            item.NetPayBeforeVATAmount = 0M;
                            item.TotalForVAT = 0M;
                            item.UnverifiedStock = 0M;
                            item.VATInclusive = 0M;
                            item.PurchaseRate = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"])));
                            item.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"]));
                            item.IndentUOM = Convert.ToString(DALHelper.HandleDBNull(reader["IndentUOM"]));
                            item.IssuedUOM = Convert.ToString(DALHelper.HandleDBNull(reader["IssuedUOM"]));
                            item.ReasonForIssue = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReasonForIssue"]));
                            item.ReasonIssueDescription = Convert.ToString(DALHelper.HandleDBNull(reader["ReasonIssueDescription"]));
                            item.Rack = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"]));
                            item.Shelf = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"]));
                            item.Bin = Convert.ToString(DALHelper.HandleDBNull(reader["Container"]));
                            item.GRNNo = Convert.ToString(DALHelper.HandleDBNull(reader["GRNNO"]));
                            nvo.ItemList.Add(item);
                            clsReturnItemDetailsVO svo2 = new clsReturnItemDetailsVO {
                                BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                                BatchId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchId"]))),
                                ExpiryDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["ExpiryDate"]))),
                                IssueQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IssuedQuantity"])),
                                ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                                ItemId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]))),
                                ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                                ItemVATPercentage = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPercentage"]))),
                                MRP = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["BatchMRP"])))
                            };
                            if (nvo.flagReceivedIssue)
                            {
                                svo2.ToStoreID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ToStoreID"])));
                            }
                            svo2.IsIndent = Convert.ToInt32(DALHelper.HandleDBNull(reader["IsIndent"]));
                            svo2.ReasonForIssue = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReasonForIssue"]));
                            svo2.ReasonIssueDescription = Convert.ToString(DALHelper.HandleDBNull(reader["ReasonIssueDescription"]));
                            if (nvo.flagReceivedIssue)
                            {
                                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetReceiveQuantityAgainstIssue");
                                this.dbServer.AddInParameter(storedProcCommand, "IssueId", DbType.Int64, nvo.IssueId);
                                this.dbServer.AddInParameter(storedProcCommand, "ToStoreID", DbType.Int64, Convert.ToInt64(DALHelper.HandleDBNull(reader["ToStoreID"])));
                                this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"])));
                                this.dbServer.AddOutParameter(storedProcCommand, "ReceivedQuantity", DbType.Decimal, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(storedProcCommand);
                                svo2.BalanceQty = (DALHelper.HandleDBNull(this.dbServer.GetParameterValue(storedProcCommand, "ReceivedQuantity")) != null) ? ((decimal) this.dbServer.GetParameterValue(storedProcCommand, "ReceivedQuantity")) : 0M;
                            }
                            svo2.PurchaseRate = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"])));
                            nvo.ReturnItemList.Add(svo2);
                            clsReceivedItemDetailsVO svo3 = new clsReceivedItemDetailsVO {
                                BalanceQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PendingQuantity"])),
                                BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                                BatchId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchId"]))),
                                ExpiryDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["ExpiryDate"]))),
                                IssueQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IssuedQuantity"])),
                                BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUOMID"])),
                                IssueQtyBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])),
                                BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])),
                                BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"])),
                                IssuedUOM = Convert.ToString(DALHelper.HandleDBNull(reader["IssuedUOM"])),
                                UOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"])),
                                SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"])),
                                ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"])),
                                SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"])),
                                ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                                ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"])),
                                ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                                ItemVATPercentage = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemVATPercentage"]))),
                                MRP = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["BatchMRP"]))),
                                IssueDetailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IssueID"])),
                                IsIndent = Convert.ToInt32(DALHelper.HandleDBNull(reader["IsIndent"])),
                                IsItemBlock = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsItemBlock"])),
                                PurchaseRate = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"]))),
                                GRNNo = Convert.ToString(DALHelper.HandleDBNull(reader["GRNNO"])),
                                GRNID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNID"])),
                                GRNUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNUnitID"])),
                                GRNDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailID"])),
                                GRNDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailUnitID"])),
                                ReasonForIssueName = Convert.ToString(DALHelper.HandleDBNull(reader["ReasonIssueDescription"]))
                            };
                            if (!nvo.flagReceivedIssue)
                            {
                                svo3.OtherGRNItemTaxApplicationOn = Convert.ToInt16(DALHelper.HandleDBNull(reader["othertaxApplicableon"]));
                                svo3.OtherGRNItemTaxType = Convert.ToInt16(DALHelper.HandleDBNull(reader["otherTaxType"]));
                                svo3.GRNItemVatApplicationOn = Convert.ToInt16(DALHelper.HandleDBNull(reader["VatApplicableon"]));
                                svo3.GRNItemVatType = Convert.ToInt16(DALHelper.HandleDBNull(reader["Vattype"]));
                                svo3.ItemTaxPercentage = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemTaxPercentage"])));
                                svo3.InclusiveOfTax = Convert.ToBoolean(DALHelper.HandleDBNull(reader["InclusiveOfTax"]));
                                svo3.SupplierName = Convert.ToString(DALHelper.HandleDBNull(reader["SupplierName"]));
                            }
                            nvo.ReceivedItemList.Add(svo3);
                        }
                    }
                    reader.NextResult();
                }
                else
                {
                    valueObject = this.GetReceivedAgainstIssueItemList(valueObject, userVO);
                    return valueObject;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return valueObject;
        }

        private IValueObject GetReceivedAgainstIssueItemList(IValueObject valueObject, clsUserVO userVO)
        {
            clsGetItemListByIssueIdBizActionVO nvo = valueObject as clsGetItemListByIssueIdBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ReceivedItemAgainstIssueDetails");
                this.dbServer.AddInParameter(storedProcCommand, "IssueId", DbType.Int64, nvo.IssueId);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientunitID", DbType.Int64, nvo.PatientunitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsAgainstPatient", DbType.Boolean, nvo.IsAgainstPatient);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ReturnItemList == null)
                    {
                        nvo.ReturnItemList = new List<clsReturnItemDetailsVO>();
                    }
                    decimal num = 0M;
                    while (reader.Read())
                    {
                        clsReturnItemDetailsVO item = new clsReturnItemDetailsVO {
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            BatchId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchId"]))),
                            ExpiryDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["ExpiryDate"]))),
                            IssueQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IssueQty"]))
                        };
                        num = Convert.ToDecimal(DALHelper.HandleDBNull(reader["BalanceQty"]));
                        item.BalanceQty = num;
                        item.ReceivedUOM = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedUOM"]));
                        item.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        item.ItemId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"])));
                        item.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        item.ItemVATPercentage = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemVATPercentage"])));
                        item.MRP = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"])));
                        item.ToStoreID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ToStoreID"])));
                        item.ReceivedItemDetailsID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])));
                        item.ReceivedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedID"]));
                        item.PurchaseRate = new decimal?(Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"])));
                        item.AvailableStock = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"])));
                        item.AvailableStockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["AvailableStockUOM"]));
                        item.ConversionFactor = new float?((float) Convert.ToInt64(DALHelper.HandleDBNull(reader["ConversionFactor"])));
                        item.IsIndent = Convert.ToInt32(DALHelper.HandleDBNull(reader["IsIndent"]));
                        item.ReceivedByName = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedByName"]));
                        item.IssuedUOM = Convert.ToString(DALHelper.HandleDBNull(reader["IssuedUOM"]));
                        item.SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));
                        item.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUOMID"]));
                        item.UOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                        item.UOM = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedUOM"]));
                        item.ConversionFactor = new float?(Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"])));
                        item.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"]));
                        item.ReceivedQty = Convert.ToSingle(DALHelper.HandleDBNull(reader["ReceiveddQuantity"]));
                        item.ReceivedUOM = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedUOM"]));
                        item.BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"]));
                        item.TotalPatientIndentReceiveQty = Convert.ToSingle(DALHelper.HandleDBNull(reader["TotalPatientIndentReceiveQty"]));
                        item.TotalPatientIndentConsumptionQty = Convert.ToSingle(DALHelper.HandleDBNull(reader["TotalPatientIndentConsumptionQty"]));
                        nvo.ReturnItemList.Add(item);
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

        private decimal GetTransitBalance(long? itemId, long? issueId, decimal? issueQuantity)
        {
            decimal num2;
            DbConnection connection = this.dbServer.CreateConnection();
            DbDataReader reader = null;
            try
            {
                decimal num = 0M;
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("GetTransitBalance");
                this.dbServer.AddInParameter(storedProcCommand, "IssueId", DbType.Int64, issueId);
                this.dbServer.AddInParameter(storedProcCommand, "ItemId", DbType.Int64, itemId);
                this.dbServer.AddInParameter(storedProcCommand, "IssueQuantity", DbType.Int64, issueQuantity);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                while (true)
                {
                    if (!reader.Read())
                    {
                        reader.Close();
                        num2 = num;
                        break;
                    }
                    num = Convert.ToDecimal(DALHelper.HandleDBNull(reader[0]));
                }
            }
            catch (Exception)
            {
                num2 = 0M;
            }
            return num2;
        }

        private decimal GetTransitReturnBalance(long? itemId, long? issueId)
        {
            decimal num2;
            DbConnection connection = this.dbServer.CreateConnection();
            DbDataReader reader = null;
            try
            {
                decimal num = 0M;
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("GetReturnTransitBalance");
                this.dbServer.AddInParameter(storedProcCommand, "IssueId", DbType.Int64, issueId);
                this.dbServer.AddInParameter(storedProcCommand, "ItemId", DbType.Int64, itemId);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                while (true)
                {
                    if (!reader.Read())
                    {
                        reader.Close();
                        num2 = num;
                        break;
                    }
                    num = Convert.ToDecimal(DALHelper.HandleDBNull(reader[0]));
                }
            }
            catch (Exception)
            {
                num2 = 0M;
            }
            return num2;
        }

        public void UpdateIndentStatus(int indentStatus, long? indentId, long? UnitID, decimal? balanceQty, long IndentDetailsID)
        {
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateIndentStatus");
                this.dbServer.AddInParameter(storedProcCommand, "IndentStatus", DbType.Int32, indentStatus);
                this.dbServer.AddInParameter(storedProcCommand, "IndentID", DbType.Int64, indentId);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "BalanceQty", DbType.Decimal, balanceQty);
                this.dbServer.AddInParameter(storedProcCommand, "IndentDetailsID", DbType.Int64, IndentDetailsID);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

