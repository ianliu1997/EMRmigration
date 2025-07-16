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
    using System.Reflection;

    internal class clsWOGRNDAL : clsBaseWOGRNDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsWOGRNDAL()
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

        private clsAddWOGRNBizActionVO AddDetails(clsAddWOGRNBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbConnection myConnection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            try
            {
                myConnection.Open();
                transaction = myConnection.BeginTransaction();
                clsWOGRNVO details = BizActionObj.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddWOGRN");
                storedProcCommand.Connection = myConnection;
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, details.LinkServer);
                if (details.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, details.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, details.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, details.Time);
                this.dbServer.AddInParameter(storedProcCommand, "GRNNO", DbType.String, details.GRNNO);
                this.dbServer.AddInParameter(storedProcCommand, "GRNType", DbType.Int16, (int) details.GRNType);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, details.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierID", DbType.Int64, details.SupplierID);
                this.dbServer.AddInParameter(storedProcCommand, "InvoiceNo", DbType.String, details.InvoiceNo);
                this.dbServer.AddInParameter(storedProcCommand, "InvoiceDate", DbType.DateTime, details.InvoiceDate);
                this.dbServer.AddInParameter(storedProcCommand, "DeliveryChallanNo", DbType.String, details.DeliveryChallanNo);
                this.dbServer.AddInParameter(storedProcCommand, "GatePassNo", DbType.String, details.GatePassNo);
                this.dbServer.AddInParameter(storedProcCommand, "WOID", DbType.Int64, details.WOID);
                this.dbServer.AddInParameter(storedProcCommand, "PaymentModeID", DbType.Int16, (int) details.PaymentModeID);
                this.dbServer.AddInParameter(storedProcCommand, "ReceivedByID", DbType.Int64, details.ReceivedByID);
                this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, details.Remarks);
                this.dbServer.AddInParameter(storedProcCommand, "TotalCDiscount", DbType.Double, details.TotalCDiscount);
                this.dbServer.AddInParameter(storedProcCommand, "TotalSchDiscount", DbType.Double, details.TotalSchDiscount);
                this.dbServer.AddInParameter(storedProcCommand, "TotalVAT", DbType.Double, details.TotalVAT);
                this.dbServer.AddInParameter(storedProcCommand, "TotalAmount", DbType.Double, details.TotalAmount);
                this.dbServer.AddInParameter(storedProcCommand, "NetAmount", DbType.Double, details.NetAmount);
                this.dbServer.AddInParameter(storedProcCommand, "Other", DbType.Double, details.Other);
                this.dbServer.AddInParameter(storedProcCommand, "IsConsignment", DbType.Boolean, details.IsConsignment);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, details.Status);
                if (BizActionObj.IsFileAttached.Value)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FileData", DbType.Binary, BizActionObj.File);
                    this.dbServer.AddInParameter(storedProcCommand, "FileName", DbType.String, BizActionObj.FileName);
                    this.dbServer.AddInParameter(storedProcCommand, "IsFileAttached", DbType.String, BizActionObj.IsFileAttached);
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, details.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "Freezed", DbType.Boolean, BizActionObj.Details.Freezed);
                this.dbServer.AddParameter(storedProcCommand, "Number", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, details.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.Details.GRNNO = (details.ID != 0L) ? details.GRNNO : ((string) this.dbServer.GetParameterValue(storedProcCommand, "Number"));
                BizActionObj.Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                BizActionObj.Details.UnitId = UserVo.UserLoginInfo.UnitId;
                if (BizActionObj.IsEditMode)
                {
                    GetInstance().WODeleteGRNItems(BizActionObj, UserVo);
                }
                foreach (clsWOGRNDetailsVO svo in details.Items)
                {
                    svo.GRNID = BizActionObj.Details.ID;
                    svo.GRNUnitID = BizActionObj.Details.UnitId;
                    svo.StockDetails.PurchaseRate = svo.Rate;
                    svo.StockDetails.MRP = svo.MRP;
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateItemBatchByGRN");
                    command2.Connection = myConnection;
                    this.dbServer.AddInParameter(command2, "LinkServer", DbType.String, details.LinkServer);
                    if (details.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(command2, "LinkServerAlias", DbType.String, details.LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddInParameter(command2, "POID", DbType.Int64, svo.WOID);
                    this.dbServer.AddInParameter(command2, "POUnitID", DbType.Int64, svo.WOUnitID);
                    this.dbServer.AddInParameter(command2, "GRNID", DbType.Int64, svo.GRNID);
                    this.dbServer.AddInParameter(command2, "GRNUnitID", DbType.Int64, svo.GRNUnitID);
                    this.dbServer.AddInParameter(command2, "ItemID", DbType.Int64, svo.ItemID);
                    this.dbServer.AddParameter(command2, "BatchID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.BatchID);
                    this.dbServer.AddInParameter(command2, "BatchCode", DbType.String, svo.BatchCode);
                    this.dbServer.AddInParameter(command2, "ExpiryDate", DbType.DateTime, svo.ExpiryDate);
                    this.dbServer.AddInParameter(command2, "Quantity", DbType.Double, svo.Quantity * svo.ConversionFactor);
                    this.dbServer.AddInParameter(command2, "FreeQuantity", DbType.Double, svo.FreeQuantity * svo.ConversionFactor);
                    this.dbServer.AddInParameter(command2, "CoversionFactor", DbType.Double, svo.ConversionFactor);
                    this.dbServer.AddInParameter(command2, "Rate", DbType.Double, svo.Rate);
                    this.dbServer.AddInParameter(command2, "MRP", DbType.Double, svo.MRP);
                    this.dbServer.AddInParameter(command2, "POQty", DbType.Double, svo.WOQuantity);
                    this.dbServer.AddInParameter(command2, "PoPendingQty", DbType.Double, svo.PendingQuantity);
                    this.dbServer.AddInParameter(command2, "ActualPendingQty", DbType.Double, svo.WOPendingQuantity);
                    this.dbServer.AddInParameter(command2, "Amount", DbType.Double, svo.Amount);
                    this.dbServer.AddInParameter(command2, "VATPercent", DbType.Double, svo.VATPercent);
                    this.dbServer.AddInParameter(command2, "VATAmount", DbType.Double, svo.VATAmount);
                    this.dbServer.AddInParameter(command2, "CDiscountPercent", DbType.Double, svo.CDiscountPercent);
                    this.dbServer.AddInParameter(command2, "CDiscountAmount", DbType.Double, svo.CDiscountAmount);
                    this.dbServer.AddInParameter(command2, "SchDiscountPercent", DbType.Double, svo.SchDiscountPercent);
                    this.dbServer.AddInParameter(command2, "SchDiscountAmount", DbType.Double, svo.SchDiscountAmount);
                    this.dbServer.AddInParameter(command2, "ItemTax", DbType.Double, svo.ItemTax);
                    this.dbServer.AddInParameter(command2, "NetAmount", DbType.Double, svo.NetAmount);
                    this.dbServer.AddInParameter(command2, "Remarks", DbType.String, svo.Remarks);
                    this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, details.Status);
                    this.dbServer.AddInParameter(command2, "IsConsignment", DbType.Boolean, details.IsConsignment);
                    this.dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, details.AddedDateTime);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command2, "AssignSupplier", DbType.Boolean, svo.AssignSupplier);
                    this.dbServer.AddInParameter(command2, "SupplierId", DbType.Int64, details.SupplierID);
                    this.dbServer.AddInParameter(command2, "BarCode", DbType.String, svo.BarCode);
                    this.dbServer.AddInParameter(command2, "Date", DbType.DateTime, details.Date);
                    this.dbServer.AddInParameter(command2, "Time", DbType.DateTime, details.Time);
                    this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, -1);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                    svo.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                    svo.BatchID = (long) this.dbServer.GetParameterValue(command2, "BatchID");
                    if (details.ItemsWOGRN.Count > 0)
                    {
                        foreach (clsWOGRNDetailsVO svo2 in details.ItemsWOGRN)
                        {
                            if (svo2.ItemID == svo.ItemID)
                            {
                                DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddPOGRNItemsLink");
                                command3.Connection = myConnection;
                                this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command3, "GRNID", DbType.Int64, details.ID);
                                this.dbServer.AddInParameter(command3, "GRNUnitID", DbType.Int64, details.UnitId);
                                this.dbServer.AddInParameter(command3, "GRNDetailID", DbType.Int64, svo.ID);
                                this.dbServer.AddInParameter(command3, "GRNDetailUnitID", DbType.Int64, details.UnitId);
                                this.dbServer.AddInParameter(command3, "POID", DbType.Int64, svo2.WOID);
                                this.dbServer.AddInParameter(command3, "POUnitID", DbType.Int64, svo2.WOUnitID);
                                this.dbServer.AddInParameter(command3, "PODetailsID", DbType.Int64, svo2.WoItemsID);
                                this.dbServer.AddInParameter(command3, "PODetailsUnitID", DbType.Int64, svo2.WOUnitID);
                                this.dbServer.AddInParameter(command3, "ItemID", DbType.Int64, svo2.ItemID);
                                this.dbServer.AddInParameter(command3, "Quantity", DbType.Decimal, svo2.Quantity);
                                this.dbServer.AddInParameter(command3, "PendingQty", DbType.Decimal, svo2.WOPendingQuantity);
                                this.dbServer.AddInParameter(command3, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                                this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, details.AddedDateTime);
                                this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, -1);
                                this.dbServer.ExecuteNonQuery(command3, transaction);
                            }
                        }
                    }
                    if (!BizActionObj.IsDraft && (details.Freezed && (svo.WOID > 0L)))
                    {
                        DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_UpdateWOFromGRN");
                        command4.Connection = myConnection;
                        this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command4, "BalanceQty", DbType.Decimal, svo.Quantity);
                        this.dbServer.AddInParameter(command4, "WOItemsID", DbType.Int64, svo.ItemID);
                        this.dbServer.AddInParameter(command4, "PendingQty", DbType.Decimal, svo.WOPendingQuantity);
                        this.dbServer.AddInParameter(command4, "IndentID", DbType.Decimal, svo.IndentID);
                        this.dbServer.AddInParameter(command4, "IndentUnitID", DbType.Decimal, svo.IndentUnitID);
                        this.dbServer.AddInParameter(command4, "WOID", DbType.Int64, svo.WOID);
                        this.dbServer.AddInParameter(command4, "WOUnitID", DbType.Int64, svo.WOUnitID);
                        this.dbServer.AddInParameter(command4, "GRNID", DbType.Int64, svo.GRNID);
                        this.dbServer.AddInParameter(command4, "GRNUnitID", DbType.Int64, svo.GRNUnitID);
                        this.dbServer.ExecuteNonQuery(command4, transaction);
                    }
                    svo.StockDetails.BatchID = svo.BatchID;
                    svo.StockDetails.OperationType = InventoryStockOperationType.Addition;
                    svo.StockDetails.ItemID = svo.ItemID;
                    svo.StockDetails.TransactionTypeID = InventoryTransactionType.GoodsReceivedNote;
                    svo.StockDetails.TransactionID = svo.GRNID;
                    svo.StockDetails.TransactionQuantity = (svo.Quantity + svo.FreeQuantity) * svo.ConversionFactor;
                    svo.StockDetails.BatchCode = svo.BatchCode;
                    svo.StockDetails.Date = details.Date;
                    svo.StockDetails.Time = details.Time;
                    svo.StockDetails.StoreID = new long?(details.StoreID);
                    if (details.Freezed)
                    {
                        clsBaseItemStockDAL instance = clsBaseItemStockDAL.GetInstance();
                        clsAddItemStockBizActionVO valueObject = new clsAddItemStockBizActionVO {
                            Details = svo.StockDetails
                        };
                        valueObject.Details.ID = 0L;
                        if (BizActionObj.IsDraft)
                        {
                            svo.StockDetails.ID = 0L;
                        }
                        else
                        {
                            valueObject = (clsAddItemStockBizActionVO) instance.Add(valueObject, UserVo, myConnection, transaction);
                            if (valueObject.SuccessStatus == -1)
                            {
                                throw new Exception("Error");
                            }
                            svo.StockDetails.ID = valueObject.Details.ID;
                        }
                    }
                }
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                BizActionObj.SuccessStatus = -1;
                BizActionObj.Details = null;
            }
            finally
            {
                myConnection.Close();
                transaction = null;
            }
            return BizActionObj;
        }

        public override IValueObject WOAdd(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddWOGRNBizActionVO bizActionObj = valueObject as clsAddWOGRNBizActionVO;
            bizActionObj = this.AddDetails(bizActionObj, UserVo);
            return valueObject;
        }

        public override IValueObject WODeleteGRNItems(IValueObject valueObject, clsUserVO UserVo)
        {
            this.dbServer.CreateConnection();
            clsAddWOGRNBizActionVO nvo = null;
            clsWOGRNVO details = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteGRNItemDetails");
                nvo = valueObject as clsAddWOGRNBizActionVO;
                details = nvo.Details;
                this.dbServer.AddInParameter(storedProcCommand, "GRNID", DbType.Int64, details.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, details.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception exception)
            {
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                nvo.SuccessStatus = -1;
                nvo.Details = null;
            }
            return nvo;
        }

        public override IValueObject WOGetGRNForGRNReturn(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetWOGRNDetailsForGRNReturnListBizActionVO nvo = valueObject as clsGetWOGRNDetailsForGRNReturnListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetWOGRNItemsForWOGRNReturnList");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "GRNID", DbType.Int64, nvo.GRNID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Freezed", DbType.Int64, nvo.Freezed);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsWOGRNDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsWOGRNDetailsVO item = new clsWOGRNDetailsVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitId = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            GRNID = (long) DALHelper.HandleDBNull(reader["GRNID"]),
                            ItemID = (long) DALHelper.HandleDBNull(reader["ItemID"]),
                            BatchID = (long) DALHelper.HandleDBNull(reader["BatchID"]),
                            BatchCode = (string) DALHelper.HandleDBNull(reader["BatchCode"]),
                            ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]),
                            ConversionFactor = Convert.ToDouble(DALHelper.HandleDBNull(reader["conversionFactor"]))
                        };
                        item.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"])) / item.ConversionFactor;
                        item.FreeQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["FreeQuantity"])) / item.ConversionFactor;
                        item.AvailableQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        item.WOPendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        item.WOQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["WOIQuantity"]));
                        item.PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        item.WOID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WOID"]));
                        item.WOUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WOrderUnitId"]));
                        item.WODate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["WOrderDate"]));
                        item.StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        item.PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["WorkUOM"]));
                        item.BarCode = Convert.ToString(DALHelper.HandleDBNull(reader["BarCode"]));
                        item.Rate = (double) DALHelper.HandleDBNull(reader["Rate"]);
                        item.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"]));
                        item.IsBatchRequired = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"]));
                        item.VATPercent = (double) DALHelper.HandleDBNull(reader["VATPercent"]);
                        if (item.VATPercent <= 0.0)
                        {
                            item.VATAmount = (double) DALHelper.HandleDBNull(reader["VATAmount"]);
                        }
                        item.CDiscountPercent = (double) DALHelper.HandleDBNull(reader["CDiscountPercent"]);
                        if (item.CDiscountPercent <= 0.0)
                        {
                            item.CDiscountAmount = (double) DALHelper.HandleDBNull(reader["CDiscountAmount"]);
                        }
                        item.SchDiscountPercent = (double) DALHelper.HandleDBNull(reader["SchDiscountPercent"]);
                        if (item.SchDiscountPercent <= 0.0)
                        {
                            item.SchDiscountAmount = (double) DALHelper.HandleDBNull(reader["SchDiscountAmount"]);
                        }
                        item.ItemName = (string) DALHelper.HandleDBNull(reader["ItemName"]);
                        item.ItemTax = (double) DALHelper.HandleDBNull(reader["ItemTax"]);
                        item.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        item.WONO = Convert.ToString(DALHelper.HandleDBNull(reader["WONO"]));
                        nvo.List.Add(item);
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

        public override IValueObject WOGetItemDetailsByID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetWOGRNDetailsListByIDBizActionVO nvo = valueObject as clsGetWOGRNDetailsListByIDBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetWOGRNItemsByID");
                this.dbServer.AddInParameter(storedProcCommand, "GRNID", DbType.Int64, nvo.GRNID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsWOGRNDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsWOGRNDetailsVO item = new clsWOGRNDetailsVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitId = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            GRNID = (long) DALHelper.HandleDBNull(reader["GRNID"]),
                            ItemID = (long) DALHelper.HandleDBNull(reader["ItemID"]),
                            BatchID = (long) DALHelper.HandleDBNull(reader["BatchID"]),
                            BatchCode = (string) DALHelper.HandleDBNull(reader["BatchCode"]),
                            ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]),
                            ConversionFactor = Convert.ToDouble(DALHelper.HandleDBNull(reader["coversionFactor"]))
                        };
                        item.Quantity = ((double) DALHelper.HandleDBNull(reader["Quantity"])) / item.ConversionFactor;
                        item.FreeQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["FreeQuantity"])) / item.ConversionFactor;
                        item.AvailableQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        item.WOQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["POQty"]));
                        item.PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["WoPendingQty"]));
                        item.WOPendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["ActualPendingQty"]));
                        item.WOID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WOID"]));
                        item.WOUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WOrderUnitId"]));
                        item.WODate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["WOrderDate"]));
                        item.StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        item.PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["WorkUOM"]));
                        item.UnitMRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"]));
                        item.UnitRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                        item.BarCode = Convert.ToString(DALHelper.HandleDBNull(reader["BarCode"]));
                        item.Rate = (double) DALHelper.HandleDBNull(reader["Rate"]);
                        item.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"]));
                        item.IsBatchRequired = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"]));
                        item.VATPercent = (double) DALHelper.HandleDBNull(reader["VATPercent"]);
                        if (item.VATPercent <= 0.0)
                        {
                            item.VATAmount = (double) DALHelper.HandleDBNull(reader["VATAmount"]);
                        }
                        item.CDiscountPercent = (double) DALHelper.HandleDBNull(reader["CDiscountPercent"]);
                        if (item.CDiscountPercent <= 0.0)
                        {
                            item.CDiscountAmount = (double) DALHelper.HandleDBNull(reader["CDiscountAmount"]);
                        }
                        item.SchDiscountPercent = (double) DALHelper.HandleDBNull(reader["SchDiscountPercent"]);
                        if (item.SchDiscountPercent <= 0.0)
                        {
                            item.SchDiscountAmount = (double) DALHelper.HandleDBNull(reader["SchDiscountAmount"]);
                        }
                        item.ItemName = (string) DALHelper.HandleDBNull(reader["ItemName"]);
                        item.ItemTax = (double) DALHelper.HandleDBNull(reader["ItemTax"]);
                        item.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        item.WONO = Convert.ToString(DALHelper.HandleDBNull(reader["WONO"]));
                        item.ItemGroup = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemGroup"]));
                        item.ItemCategory = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"]));
                        item.IsConsignment = (bool) DALHelper.HandleBoolDBNull(reader["IsConsignment"]);
                        nvo.List.Add(item);
                    }
                }
                reader.NextResult();
                if (nvo.WOGRNList == null)
                {
                    nvo.WOGRNList = new List<clsWOGRNDetailsVO>();
                }
                while (true)
                {
                    if (!reader.Read())
                    {
                        reader.Close();
                        break;
                    }
                    clsWOGRNDetailsVO item = new clsWOGRNDetailsVO {
                        ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                        UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                        GRNID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNID"])),
                        GRNUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNUnitID"])),
                        GRNDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailID"])),
                        GRNDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailUnitID"])),
                        WOID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WOID"])),
                        WOUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WOUnitID"])),
                        WODetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WODetailID"])),
                        WoItemsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WODetailID"])),
                        WODetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WODetailUnitID"])),
                        ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"])),
                        Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"])),
                        PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQty"])),
                        WOPendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQty"]))
                    };
                    nvo.WOGRNList.Add(item);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject WOGetItemDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetWOGRNDetailsListBizActionVO nvo = valueObject as clsGetWOGRNDetailsListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetWOGRNItemsList");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "GRNID", DbType.Int64, nvo.GRNID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Freezed", DbType.Int64, nvo.Freezed);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsWOGRNDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsWOGRNDetailsVO item = new clsWOGRNDetailsVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitId = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            GRNID = (long) DALHelper.HandleDBNull(reader["GRNID"]),
                            ItemID = (long) DALHelper.HandleDBNull(reader["ItemID"]),
                            BatchID = (long) DALHelper.HandleDBNull(reader["BatchID"]),
                            BatchCode = (string) DALHelper.HandleDBNull(reader["BatchCode"]),
                            ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]),
                            ConversionFactor = Convert.ToDouble(DALHelper.HandleDBNull(reader["conversionFactor"]))
                        };
                        item.Quantity = ((double) DALHelper.HandleDBNull(reader["Quantity"])) / item.ConversionFactor;
                        item.FreeQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["FreeQuantity"])) / item.ConversionFactor;
                        item.AvailableQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        item.WOPendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        item.WOQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["WOIQuantity"]));
                        item.PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        item.WOID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WOID"]));
                        item.WOUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WOrderUnitId"]));
                        item.WODate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["WOrderDate"]));
                        item.StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        item.PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["WorkUOM"]));
                        item.UnitMRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"]));
                        item.UnitRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                        item.BarCode = Convert.ToString(DALHelper.HandleDBNull(reader["BarCode"]));
                        item.Rate = (double) DALHelper.HandleDBNull(reader["Rate"]);
                        item.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"]));
                        item.IsBatchRequired = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"]));
                        item.VATPercent = (double) DALHelper.HandleDBNull(reader["VATPercent"]);
                        if (item.VATPercent <= 0.0)
                        {
                            item.VATAmount = (double) DALHelper.HandleDBNull(reader["VATAmount"]);
                        }
                        item.CDiscountPercent = (double) DALHelper.HandleDBNull(reader["CDiscountPercent"]);
                        if (item.CDiscountPercent <= 0.0)
                        {
                            item.CDiscountAmount = (double) DALHelper.HandleDBNull(reader["CDiscountAmount"]);
                        }
                        item.SchDiscountPercent = (double) DALHelper.HandleDBNull(reader["SchDiscountPercent"]);
                        if (item.SchDiscountPercent <= 0.0)
                        {
                            item.SchDiscountAmount = (double) DALHelper.HandleDBNull(reader["SchDiscountAmount"]);
                        }
                        item.ItemName = (string) DALHelper.HandleDBNull(reader["ItemName"]);
                        item.ItemTax = (double) DALHelper.HandleDBNull(reader["ItemTax"]);
                        item.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        item.WONO = Convert.ToString(DALHelper.HandleDBNull(reader["WONO"]));
                        item.ItemGroup = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemGroup"]));
                        item.ItemCategory = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"]));
                        item.IsConsignment = (bool) DALHelper.HandleBoolDBNull(reader["IsConsignment"]);
                        nvo.List.Add(item);
                    }
                }
                reader.NextResult();
                if (nvo.WOGRNList == null)
                {
                    nvo.WOGRNList = new List<clsWOGRNDetailsVO>();
                }
                while (true)
                {
                    if (!reader.Read())
                    {
                        reader.Close();
                        break;
                    }
                    clsWOGRNDetailsVO item = new clsWOGRNDetailsVO {
                        ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                        UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                        GRNID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNID"])),
                        GRNUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNUnitID"])),
                        GRNDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailID"])),
                        GRNDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailUnitID"])),
                        WOID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WOID"])),
                        WOUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WOUnitID"])),
                        WODetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WODetailID"])),
                        WoItemsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WODetailID"])),
                        WODetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WODetailUnitID"])),
                        ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"])),
                        Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"])),
                        PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQty"])),
                        WOPendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQty"]))
                    };
                    nvo.WOGRNList.Add(item);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject WOGetSearchList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetWOGRNListBizActionVO nvo = valueObject as clsGetWOGRNListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetWOGRNListForSearch");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
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
                if (nvo.GRNNo != "")
                {
                    this.dbServer.AddInParameter(storedProcCommand, "GRNNo", DbType.String, nvo.GRNNo);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "GRNNo", DbType.String, null);
                }
                this.dbServer.AddInParameter(storedProcCommand, "GrnReturnSearch", DbType.Boolean, nvo.GrnReturnSearch);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, nvo.Freezed);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsWOGRNVO>();
                    }
                    while (reader.Read())
                    {
                        clsWOGRNVO item = new clsWOGRNVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            StoreID = (long) DALHelper.HandleDBNull(reader["StoreID"]),
                            UnitId = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            SupplierID = (long) DALHelper.HandleDBNull(reader["SupplierID"]),
                            GRNNO = (string) DALHelper.HandleDBNull(reader["GRNNO"]),
                            GRNType = (InventoryGRNType) ((short) DALHelper.HandleDBNull(reader["GRNType"])),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        DateTime? nullable4 = DALHelper.HandleDate(reader["Date"]);
                        item.Date = nullable4.Value;
                        item.SupplierName = (string) DALHelper.HandleDBNull(reader["SupplierName"]);
                        item.StoreName = (string) DALHelper.HandleDBNull(reader["StoreName"]);
                        item.PaymentModeID = (MaterPayModeList) Convert.ToInt16(DALHelper.HandleDBNull(reader["PaymentModeID"]));
                        item.Freezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Freezed"]));
                        item.GatePassNo = Convert.ToString(DALHelper.HandleDBNull(reader["GatePassNo"]));
                        item.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"]));
                        item.WOID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WOID"]));
                        item.InvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader["InvoiceNo"]));
                        item.InvoiceDate = DALHelper.HandleDate(reader["InvoiceDate"]);
                        item.WONowithDate = (string) DALHelper.HandleDBNull(reader["WO No - Date"]);
                        item.FileName = (string) DALHelper.HandleDBNull(reader["FileName"]);
                        item.File = (byte[]) DALHelper.HandleDBNull(reader["FileData"]);
                        item.IsFileAttached = (bool) DALHelper.HandleDBNull(reader["IsFileAttached"]);
                        item.FileAttached = item.IsFileAttached ? "Visible" : "Collapsed";
                        item.IsConsignment = (bool) DALHelper.HandleBoolDBNull(reader["IsConsignment"]);
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

        public override IValueObject WOUpdateGRNForBarcode(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            clsAddWOGRNBizActionVO nvo = valueObject as clsAddWOGRNBizActionVO;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                clsWOGRNDetailsVO gRNItemDetails = nvo.GRNItemDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateGRNForBarcode");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, gRNItemDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, gRNItemDetails.UnitId);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                nvo.SuccessStatus = -1;
                nvo.GRNItemDetails = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
            }
            return nvo;
        }
    }
}

