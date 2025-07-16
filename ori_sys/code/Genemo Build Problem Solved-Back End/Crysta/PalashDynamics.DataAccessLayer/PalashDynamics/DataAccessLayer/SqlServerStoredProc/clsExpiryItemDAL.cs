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
    using System.Linq;
    using System.Reflection;

    public class clsExpiryItemDAL : clsBaseExpiryItemDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsExpiryItemDAL()
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

        public override IValueObject AddExpiryItemReturn(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddExpiryItemReturnBizActionVO nvo = valueObject as clsAddExpiryItemReturnBizActionVO;
            DbConnection myConnection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            try
            {
                myConnection.Open();
                transaction = myConnection.BeginTransaction();
                clsExpiredItemReturnVO objExpiryItem = nvo.objExpiryItem;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddItemExpiryReturn");
                storedProcCommand.Connection = myConnection;
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, objExpiryItem.LinkServer);
                if (objExpiryItem.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, objExpiryItem.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, objExpiryItem.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierID", DbType.Int64, objExpiryItem.SupplierID);
                this.dbServer.AddInParameter(storedProcCommand, "ExpiryReturnNo", DbType.String, objExpiryItem.ExpiryReturnNo);
                this.dbServer.AddInParameter(storedProcCommand, "GateEntryNo", DbType.String, objExpiryItem.GateEntryNo);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, objExpiryItem.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, objExpiryItem.Time);
                this.dbServer.AddInParameter(storedProcCommand, "TotalAmount", DbType.Double, objExpiryItem.TotalAmount);
                this.dbServer.AddInParameter(storedProcCommand, "TotalVATAmount", DbType.Double, objExpiryItem.TotalVATAmount);
                this.dbServer.AddInParameter(storedProcCommand, "TotalTaxAmount", DbType.Double, objExpiryItem.TotalTaxAmount);
                this.dbServer.AddInParameter(storedProcCommand, "TotalOctriAmount", DbType.Double, objExpiryItem.TotalOctriAmount);
                this.dbServer.AddInParameter(storedProcCommand, "OtherDeducution", DbType.Double, objExpiryItem.OtherDeducution);
                this.dbServer.AddInParameter(storedProcCommand, "NetAmount", DbType.Double, objExpiryItem.NetAmount);
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, objExpiryItem.Remark);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, objExpiryItem.Status);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "EditForApprove", DbType.Boolean, objExpiryItem.EditForApprove);
                this.dbServer.AddParameter(storedProcCommand, "Number", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objExpiryItem.ID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.objExpiryItem.ExpiryReturnNo = (objExpiryItem.ID != 0L) ? objExpiryItem.ExpiryReturnNo : Convert.ToString(this.dbServer.GetParameterValue(storedProcCommand, "Number"));
                nvo.objExpiryItem.ID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                foreach (clsExpiredItemReturnDetailVO lvo in nvo.objExpiryItemDetailList)
                {
                    lvo.ItemExpiryReturnID = new long?(nvo.objExpiryItem.ID);
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddItemExpiryReturnDetails");
                    command2.Connection = myConnection;
                    this.dbServer.AddInParameter(command2, "LinkServer", DbType.String, objExpiryItem.LinkServer);
                    if (objExpiryItem.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(command2, "LinkServerAlias", DbType.String, objExpiryItem.LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "ItemExpiryReturnID", DbType.Int64, lvo.ItemExpiryReturnID);
                    this.dbServer.AddInParameter(command2, "ItemID", DbType.Int64, lvo.ItemID);
                    this.dbServer.AddInParameter(command2, "BatchID", DbType.Int64, lvo.BatchID);
                    this.dbServer.AddInParameter(command2, "BatchExpiryDate", DbType.DateTime, lvo.BatchExpiryDate);
                    this.dbServer.AddInParameter(command2, "Conversion", DbType.Double, lvo.Conversion);
                    this.dbServer.AddInParameter(command2, "ReturnQty", DbType.Double, lvo.ReturnQty);
                    this.dbServer.AddInParameter(command2, "PurchaseRate", DbType.Double, lvo.PurchaseRate);
                    this.dbServer.AddInParameter(command2, "TotalPurchaseRate", DbType.Double, lvo.TotalPurchaseRate);
                    this.dbServer.AddInParameter(command2, "MRP", DbType.Double, lvo.MRP);
                    this.dbServer.AddInParameter(command2, "TotalAmount", DbType.Double, lvo.MRPAmount);
                    this.dbServer.AddInParameter(command2, "ReturnRate", DbType.Double, lvo.ReturnRate);
                    this.dbServer.AddInParameter(command2, "DiscountPercentage", DbType.Double, lvo.DiscountPercentage);
                    this.dbServer.AddInParameter(command2, "DiscountAmount", DbType.Double, lvo.DiscountAmount);
                    this.dbServer.AddInParameter(command2, "OctroiAmount", DbType.Double, lvo.OctroiAmount);
                    this.dbServer.AddInParameter(command2, "VATPercentage", DbType.Double, lvo.VATPercentage);
                    this.dbServer.AddInParameter(command2, "VATAmount", DbType.Double, lvo.VATAmount);
                    this.dbServer.AddInParameter(command2, "TotalTaxAmount", DbType.Double, lvo.TotalTaxAmount);
                    this.dbServer.AddInParameter(command2, "NetAmount", DbType.Double, lvo.NetAmount);
                    this.dbServer.AddInParameter(command2, "BaseQuantity", DbType.Single, lvo.ReturnQty * lvo.BaseConversionFactor);
                    this.dbServer.AddInParameter(command2, "BaseCF", DbType.Single, lvo.BaseConversionFactor);
                    this.dbServer.AddInParameter(command2, "TransactionUOMID", DbType.Int64, lvo.SelectedUOM.ID);
                    this.dbServer.AddInParameter(command2, "BaseUMID", DbType.Int64, lvo.BaseUOMID);
                    this.dbServer.AddInParameter(command2, "StockUOMID", DbType.Int64, lvo.StockUOMID);
                    this.dbServer.AddInParameter(command2, "StockCF", DbType.Single, lvo.ConversionFactor);
                    this.dbServer.AddInParameter(command2, "EditForApprove", DbType.Boolean, objExpiryItem.EditForApprove);
                    this.dbServer.AddInParameter(command2, "DirectApprove", DbType.Boolean, objExpiryItem.DirectApprove);
                    this.dbServer.AddInParameter(command2, "othertaxApplicableon", DbType.Int16, lvo.OtherGRNItemTaxApplicationOn);
                    this.dbServer.AddInParameter(command2, "otherTaxType", DbType.Int64, lvo.OtherGRNItemTaxType);
                    this.dbServer.AddInParameter(command2, "VatApplicableon", DbType.Int64, lvo.GRNItemVatApplicationOn);
                    this.dbServer.AddInParameter(command2, "Vattype", DbType.Int64, lvo.GRNItemVatType);
                    this.dbServer.AddInParameter(command2, "TAXPercentage", DbType.Double, lvo.ItemTax);
                    this.dbServer.AddInParameter(command2, "TaxAmount", DbType.Double, lvo.TaxAmount);
                    this.dbServer.AddInParameter(command2, "ReceivedID", DbType.Int64, lvo.ReceivedID);
                    this.dbServer.AddInParameter(command2, "ReceivedUnitID", DbType.Int64, lvo.ReceivedUnitID);
                    this.dbServer.AddInParameter(command2, "ReceivedDetailID", DbType.Int64, lvo.ReceivedDetailID);
                    this.dbServer.AddInParameter(command2, "ReceivedDetailUnitID", DbType.Int64, lvo.ReceivedDetailUnitID);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, lvo.ID);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                    lvo.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                    long receivedDetailID = lvo.ReceivedDetailID;
                    if (lvo.ReceivedDetailID > 0L)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_UpdateReceive");
                        command3.Connection = myConnection;
                        this.dbServer.AddInParameter(command3, "ReceiveItemDetailsID", DbType.Int64, lvo.ReceivedDetailID);
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, lvo.ReceivedDetailUnitID);
                        this.dbServer.AddInParameter(command3, "BalanceQty", DbType.Decimal, lvo.ReturnQty * lvo.BaseConversionFactor);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                    }
                }
                if (objExpiryItem.EditForApprove)
                {
                    clsBaseExpiryItemDAL instance = GetInstance();
                    clsUpdateExpiryForApproveRejectVO tvo = new clsUpdateExpiryForApproveRejectVO {
                        objExpiryItem = objExpiryItem
                    };
                    if (((clsUpdateExpiryForApproveRejectVO) instance.UpdateExpiryForApproveOrReject(tvo, UserVo, myConnection, transaction)).SuccessStatus == -1)
                    {
                        throw new Exception("Error");
                    }
                }
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                nvo.SuccessStatus = -1;
                nvo.objExpiryItem = null;
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

        public override IValueObject ApproveExpiryItemReturn(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddExpiryItemReturnBizActionVO nvo = valueObject as clsAddExpiryItemReturnBizActionVO;
            DbConnection myConnection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            try
            {
                myConnection.Open();
                transaction = myConnection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateExpiryItemReturnForApprove");
                storedProcCommand.Connection = myConnection;
                this.dbServer.AddInParameter(storedProcCommand, "ItemExpiryReturnID", DbType.Int64, nvo.objExpiryItem.ID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemExpiryReturnUnitID", DbType.Int64, nvo.objExpiryItem.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ApproveOrRejectedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddParameter(storedProcCommand, "ReasultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "ReasultStatus"));
                if (nvo.SuccessStatus == -10)
                {
                    throw new Exception();
                }
                List<clsExpiredItemReturnDetailVO> objExpiryItemDetailList = nvo.objExpiryItemDetailList;
                if (objExpiryItemDetailList.Count > 0)
                {
                    foreach (clsExpiredItemReturnDetailVO lvo in objExpiryItemDetailList.ToList<clsExpiredItemReturnDetailVO>())
                    {
                        lvo.StockDetails.PurchaseRate = lvo.PurchaseRate;
                        lvo.StockDetails.BatchID = Convert.ToInt64(lvo.BatchID);
                        lvo.StockDetails.OperationType = InventoryStockOperationType.Subtraction;
                        lvo.StockDetails.ItemID = lvo.ItemID;
                        lvo.StockDetails.TransactionTypeID = InventoryTransactionType.ExpiryItemReturn;
                        lvo.StockDetails.TransactionID = Convert.ToInt64(lvo.ItemExpiryReturnID);
                        lvo.StockDetails.TransactionQuantity = lvo.BaseQuantity;
                        lvo.StockDetails.Date = DateTime.Now;
                        lvo.StockDetails.Time = DateTime.Now;
                        lvo.StockDetails.StoreID = new long?(lvo.StoreId);
                        lvo.StockDetails.InputTransactionQuantity = Convert.ToSingle(lvo.ReturnQty);
                        lvo.StockDetails.BaseUOMID = lvo.BaseUOMID;
                        lvo.StockDetails.BaseConversionFactor = lvo.BaseConversionFactor;
                        lvo.StockDetails.SUOMID = lvo.StockUOMID;
                        lvo.StockDetails.ConversionFactor = lvo.ConversionFactor;
                        lvo.StockDetails.StockingQuantity = lvo.ReturnQty * lvo.ConversionFactor;
                        lvo.StockDetails.SelectedUOM.ID = lvo.TransactionUOMID;
                        lvo.StockDetails.ExpiryDate = lvo.BatchExpiryDate;
                        clsBaseItemStockDAL instance = clsBaseItemStockDAL.GetInstance();
                        clsAddItemStockBizActionVO nvo2 = new clsAddItemStockBizActionVO {
                            Details = lvo.StockDetails
                        };
                        nvo2.Details.ID = 0L;
                        nvo2 = (clsAddItemStockBizActionVO) instance.Add(nvo2, UserVo, myConnection, transaction);
                        if (nvo2.SuccessStatus == -1)
                        {
                            throw new Exception();
                        }
                        lvo.StockDetails.ID = nvo2.Details.ID;
                    }
                }
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
            }
            finally
            {
                myConnection.Close();
                transaction = null;
            }
            return valueObject;
        }

        private clsUpdateExpiryForApproveRejectVO ApproveRejectExpiry(clsUpdateExpiryForApproveRejectVO BizActionObj, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
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
                clsExpiredItemReturnVO objExpiryItem = BizActionObj.objExpiryItem;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdaterExpiryApproveRejectStatus");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "ApprovedOrRejectedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, objExpiryItem.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objExpiryItem.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "IsApprove", DbType.Boolean, objExpiryItem.Approve);
                this.dbServer.AddInParameter(storedProcCommand, "IsReject", DbType.Boolean, objExpiryItem.Reject);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
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

        public override IValueObject GetExpiredReturnDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetExpiredReturnDetailsBizActionVO nvo = valueObject as clsGetExpiredReturnDetailsBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetExpiredReturnDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ExpiredID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ExpiredMainList == null)
                    {
                        nvo.ExpiredMainList = new clsExpiredItemReturnVO();
                    }
                    while (reader.Read())
                    {
                        clsExpiredItemReturnVO nvo2 = new clsExpiredItemReturnVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"])),
                            StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"])),
                            SupplierID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SupplierID"])),
                            ExpiryReturnNo = Convert.ToString(DALHelper.HandleDBNull(reader["ExpiryReturnNo"])),
                            Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]))),
                            Time = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["Time"]))),
                            TotalAmount = Convert.ToDouble((decimal) DALHelper.HandleDBNull(reader["TotalAmount"])),
                            TotalVATAmount = Convert.ToDouble((decimal) DALHelper.HandleDBNull(reader["TotalVATAmount"])),
                            TotalTaxAmount = Convert.ToDouble((decimal) DALHelper.HandleDBNull(reader["TotalTaxAmount"])),
                            TotalOctriAmount = Convert.ToDouble((decimal) DALHelper.HandleDBNull(reader["TotalOctriAmount"])),
                            OtherDeducution = Convert.ToDouble((decimal) DALHelper.HandleDBNull(reader["OtherDeducution"])),
                            NetAmount = Convert.ToDouble((decimal) DALHelper.HandleDBNull(reader["NetAmount"])),
                            Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["Store"])),
                            SupplierName = Convert.ToString(DALHelper.HandleDBNull(reader["Supplier"]))
                        };
                        nvo.ExpiredMainList = nvo2;
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    if (nvo.ExpiredItemList == null)
                    {
                        nvo.ExpiredItemList = new List<clsExpiredItemReturnDetailVO>();
                    }
                    while (reader.Read())
                    {
                        clsExpiredItemReturnDetailVO item = new clsExpiredItemReturnDetailVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"])),
                            ItemExpiryReturnID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemExpiryReturnID"]))),
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                            BatchID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]))),
                            BatchExpiryDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["BatchExpiryDate"]))),
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            ReturnQty = Convert.ToDouble(DALHelper.HandleDBNull(reader["ReturnQty"])),
                            SelectedUOM = { Description = Convert.ToString(DALHelper.HandleDBNull(reader["UOM"])) },
                            VATPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["VATPercentage"])),
                            GRNItemVatApplicationOn = Convert.ToInt16(DALHelper.HandleDBNull(reader["VatApplicableon"])),
                            GRNItemVatType = Convert.ToInt16(DALHelper.HandleDBNull(reader["Vattype"])),
                            ItemTax = Convert.ToDouble(DALHelper.HandleDBNull(reader["TAXPercentage"])),
                            OtherGRNItemTaxApplicationOn = Convert.ToInt16(DALHelper.HandleDBNull(reader["othertaxApplicableon"])),
                            OtherGRNItemTaxType = Convert.ToInt16(DALHelper.HandleDBNull(reader["otherTaxType"])),
                            PurchaseRate = Convert.ToSingle(DALHelper.HandleDBNull(reader["PurchaseRate"])),
                            MRP = Convert.ToSingle(DALHelper.HandleDBNull(reader["MRP"])),
                            DiscountPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountPercentage"])),
                            BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"])),
                            BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])),
                            BaseQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseQuantity"])),
                            StockUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"])),
                            ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"])),
                            OctroiAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["OctroiAmount"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            AvailableStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"])),
                            StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["AvailableStockUOM"])),
                            AvailableStockInBase = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStockInBase"])),
                            ReceivedNo = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedNumber"])),
                            ReceivedQty = Convert.ToDouble(DALHelper.HandleDBNull(reader["ReceiveddQuantity"])),
                            ReceivedQtyUOM = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedNumber"])),
                            TransactionUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]))
                        };
                        item.SelectedUOM = new MasterListItem(item.TransactionUOMID, item.SelectedUOM.Description);
                        item.PendingQty = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceQty"]));
                        item.MainRate = Convert.ToSingle(item.PurchaseRate) / item.BaseConversionFactor;
                        item.MainMRP = Convert.ToSingle(item.MRP) / item.BaseConversionFactor;
                        nvo.ExpiredItemList.Add(item);
                    }
                }
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

        public override IValueObject GetExpiryItemList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetExpiredItemListBizActionVO nvo = valueObject as clsGetExpiredItemListBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetExpiryItemList");
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierID", DbType.Int64, nvo.SupplierID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemName", DbType.String, nvo.objExpiredItemsVO.ItemName);
                this.dbServer.AddInParameter(storedProcCommand, "BatchCode", DbType.String, nvo.objExpiredItemsVO.BatchCode);
                this.dbServer.AddInParameter(storedProcCommand, "ReceivedNo", DbType.String, nvo.objExpiredItemsVO.ReceivedNo);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "IsFromDOS", DbType.Boolean, nvo.IsFromDOS);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ExpiredItemList == null)
                    {
                        nvo.ExpiredItemList = new List<clsExpiredItemReturnDetailVO>();
                    }
                    while (reader.Read())
                    {
                        clsExpiredItemReturnDetailVO item = new clsExpiredItemReturnDetailVO {
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            BatchExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]),
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            ReceivedNo = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedNumber"])),
                            ReceivedQty = Convert.ToDouble(DALHelper.HandleDBNull(reader["ReceiveddQuantity"])),
                            ReceivedQtyUOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransactionUOM"])),
                            AvailableStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"])),
                            StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["AvailableStockUOM"])),
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                            BatchID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]))),
                            TransactionUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"])),
                            StockUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"])),
                            ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"])),
                            BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUOMID"])),
                            BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])),
                            ItemTax = Convert.ToDouble(Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemTaxPercentage"]))),
                            OtherGRNItemTaxApplicationOn = Convert.ToInt16(DALHelper.HandleDBNull(reader["othertaxApplicableon"])),
                            OtherGRNItemTaxType = Convert.ToInt16(DALHelper.HandleDBNull(reader["otherTaxType"])),
                            VATPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["ItemVATPercentage"])),
                            GRNItemVatApplicationOn = Convert.ToInt16(DALHelper.HandleDBNull(reader["VatApplicableon"])),
                            GRNItemVatType = Convert.ToInt16(DALHelper.HandleDBNull(reader["Vattype"])),
                            ReceivedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedID"])),
                            ReceivedUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedUnitID"])),
                            ReceivedDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedDetailID"])),
                            ReceivedDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedDetailUnitID"])),
                            MainRate = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseRate"])),
                            MainMRP = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseMRP"]))
                        };
                        item.PurchaseRate = item.MainRate * item.BaseConversionFactor;
                        item.MRP = item.MainMRP * item.BaseConversionFactor;
                        item.PendingQty = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceQty"]));
                        item.BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"]));
                        item.ReceivedDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["ReceivedDate"])));
                        nvo.ExpiredItemList.Add(item);
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

        public override IValueObject GetExpiryItemReturnForDashBoard(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetExpiryItemForDashBoardBizActionVO nvo = valueObject as clsGetExpiryItemForDashBoardBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetExpiryItemListForDashBoard");
                if (nvo.IsOrderBy != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "OrderBy", DbType.Int64, nvo.IsOrderBy);
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "IsPaging", DbType.Boolean, nvo.IsPaging);
                this.dbServer.AddInParameter(storedProcCommand, "StartIndex", DbType.Int64, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "NoOfRecordShow", DbType.Int64, nvo.NoOfRecordShow);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, nvo.UserID);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                if (nvo.ExpiryFromDate != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ExpiryFromDate", DbType.DateTime, nvo.ExpiryFromDate);
                }
                if (nvo.ExpiryToDate != null)
                {
                    if (nvo.ExpiryFromDate != null)
                    {
                        nvo.ExpiryToDate = new DateTime?(nvo.ExpiryToDate.Value.Date.AddDays(1.0));
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "ExpiryToDate", DbType.DateTime, nvo.ExpiryToDate);
                }
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ExpiredItemList == null)
                    {
                        nvo.ExpiredItemList = new List<clsExpiredItemReturnDetailVO>();
                    }
                    while (reader.Read())
                    {
                        clsExpiredItemReturnDetailVO item = new clsExpiredItemReturnDetailVO {
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            StoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreId"])),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            AvailableStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"])),
                            BatchExpiryDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["ExpiryDate"]))),
                            Store = Convert.ToString(DALHelper.HandleDBNull(reader["Store"])),
                            StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["UOM"])),
                            BatchID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]))),
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                            PurchaseRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["PurchaseRate"])),
                            MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"])),
                            Re_Order = Convert.ToSingle(DALHelper.HandleDBNull(reader["Re_Order"])),
                            AvailableStockInBase = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStockInBase"])),
                            BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUOMID"])),
                            BaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])),
                            StockUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"])),
                            StockCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"])),
                            TransactionUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"])),
                            UOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransactionUOM"])),
                            BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"])),
                            PONowithDate = Convert.ToString(DALHelper.HandleDBNull(reader["PONowithDate"])),
                            GRNNowithDate = Convert.ToString(DALHelper.HandleDBNull(reader["GRNNowithDate"])),
                            SupplierName = Convert.ToString(DALHelper.HandleDBNull(reader["SupplierName"])),
                            BaseCPDashBoard = Convert.ToDouble(DALHelper.HandleDBNull(reader["BaseCP"])),
                            BaseMRPDashBoard = Convert.ToDouble(DALHelper.HandleDBNull(reader["BaseMRP"])),
                            TotalCP = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalCP"])),
                            TotalMRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalMRP"])),
                            OtherGRNItemTaxApplicationOn = Convert.ToInt16(DALHelper.HandleDBNull(reader["othertaxApplicableon"])),
                            OtherGRNItemTaxType = Convert.ToInt16(DALHelper.HandleDBNull(reader["otherTaxType"])),
                            OtherGRNItemTaxPer = Convert.ToSingle(DALHelper.HandleDBNull(reader["othertax"])),
                            GRNItemVatApplicationOn = Convert.ToInt16(DALHelper.HandleDBNull(reader["VatApplicableon"])),
                            GRNItemVatType = Convert.ToInt16(DALHelper.HandleDBNull(reader["Vattype"])),
                            GRNItemVATPer = Convert.ToSingle(DALHelper.HandleDBNull(reader["VatPer"])),
                            IsItemBlock = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsItemBlock"]))
                        };
                        nvo.ExpiredItemList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRow = new long?((long) Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows")));
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

        public override IValueObject GetExpiryList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetExpiredListBizActionVO nvo = valueObject as clsGetExpiredListBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetExpiryList");
                this.dbServer.AddInParameter(storedProcCommand, "SupplierID", DbType.Int64, nvo.SupplierID);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.StoreId);
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "IsPaging", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "StartIndex", DbType.Int64, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "NoOfRecordShow", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, UserVo.ID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ExpiredList == null)
                    {
                        nvo.ExpiredList = new List<clsExpiredItemReturnVO>();
                    }
                    while (reader.Read())
                    {
                        clsExpiredItemReturnVO item = new clsExpiredItemReturnVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            ExpiryReturnNo = Convert.ToString(DALHelper.HandleDBNull(reader["ExpiryReturnNo"])),
                            Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]))),
                            StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["Store"])),
                            SupplierName = Convert.ToString(DALHelper.HandleDBNull(reader["Supplier"])),
                            NetAmount = Convert.ToDouble((decimal) DALHelper.HandleDBNull(reader["NetAmount"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Approve = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Approved"])),
                            Reject = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Rejected"])),
                            ApproveOrRejectByName = Convert.ToString(DALHelper.HandleDBNull(reader["ApproveOrRejectByName"]))
                        };
                        nvo.ExpiredList.Add(item);
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

        public override IValueObject GetExpiryListForExpiredReturn(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetExpiredListForExpiredReturnBizActionVO nvo = valueObject as clsGetExpiredListForExpiredReturnBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetExpiryListForExpiredReturn");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ConsumptionID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemList == null)
                    {
                        nvo.ItemList = new List<clsExpiredItemReturnDetailVO>();
                    }
                    while (reader.Read())
                    {
                        clsExpiredItemReturnDetailVO item = new clsExpiredItemReturnDetailVO {
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"])),
                            BatchID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]))),
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            PurchaseRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["PurchaseRate"])),
                            MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"])),
                            VATPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["VATPercentage"])),
                            GRNItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["VatApplicableon"])),
                            GRNItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Vattype"])),
                            ItemTax = Convert.ToDouble(DALHelper.HandleDBNull(reader["TAXPercentage"])),
                            OtherGRNItemTaxApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxApplicableon"])),
                            OtherGRNItemTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherTaxType"])),
                            ReturnQty = Convert.ToDouble(DALHelper.HandleDBNull(reader["ReturnQty"])),
                            UOM = Convert.ToString(DALHelper.HandleDBNull(reader["UOM"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            ReceivedNo = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedNumber"])),
                            ItemExpiryReturnID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemExpiryReturnID"]))),
                            StoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"])),
                            BaseQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseQuantity"])),
                            BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"])),
                            BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])),
                            StockUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"])),
                            ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"])),
                            TransactionUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"])),
                            BatchExpiryDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["BatchExpiryDate"])))
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
            return nvo;
        }

        public override IValueObject UpdateExpiryForApproveOrReject(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateExpiryForApproveRejectVO bizActionObj = valueObject as clsUpdateExpiryForApproveRejectVO;
            if (bizActionObj.objExpiryItem.ID > 0L)
            {
                bizActionObj = this.ApproveRejectExpiry(bizActionObj, UserVo, null, null);
            }
            return valueObject;
        }

        public override IValueObject UpdateExpiryForApproveOrReject(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {
            clsUpdateExpiryForApproveRejectVO bizActionObj = valueObject as clsUpdateExpiryForApproveRejectVO;
            if (bizActionObj.objExpiryItem.ID > 0L)
            {
                bizActionObj = this.ApproveRejectExpiry(bizActionObj, UserVo, myConnection, myTransaction);
            }
            return valueObject;
        }
    }
}

