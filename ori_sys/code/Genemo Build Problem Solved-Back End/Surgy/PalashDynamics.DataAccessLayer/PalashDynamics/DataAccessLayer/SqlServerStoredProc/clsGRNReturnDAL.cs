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

    public class clsGRNReturnDAL : clsBaseGRNReturnDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsGRNReturnDAL()
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
            clsAddGRNReturnBizActionVO bizActionObj = valueObject as clsAddGRNReturnBizActionVO;
            if (bizActionObj.Details.ID == 0L)
            {
                bizActionObj = this.AddDetails(bizActionObj, UserVo);
            }
            return valueObject;
        }

        private clsAddGRNReturnBizActionVO AddDetails(clsAddGRNReturnBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbConnection myConnection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            try
            {
                myConnection.Open();
                transaction = myConnection.BeginTransaction();
                clsGRNReturnVO details = BizActionObj.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddGRNReturn");
                storedProcCommand.Connection = myConnection;
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, details.LinkServer);
                if (details.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, details.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, details.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, details.Time);
                this.dbServer.AddParameter(storedProcCommand, "GRNReturnNO", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");
                this.dbServer.AddInParameter(storedProcCommand, "GRNID", DbType.Int64, 0);
                this.dbServer.AddInParameter(storedProcCommand, "GrnUnitID", DbType.Int64, 0);
                this.dbServer.AddInParameter(storedProcCommand, "GRNReturnType", DbType.Int16, details.GoodReturnType);
                this.dbServer.AddInParameter(storedProcCommand, "PaymentModeID", DbType.Int16, (short) details.PaymentModeID);
                this.dbServer.AddInParameter(storedProcCommand, "TotalVAT", DbType.Double, details.TotalVAT);
                this.dbServer.AddInParameter(storedProcCommand, "TotalItemTax", DbType.Double, details.TotalItemTax);
                this.dbServer.AddInParameter(storedProcCommand, "TotalSGSTAmount", DbType.Double, details.TotalSGSTAmount);
                this.dbServer.AddInParameter(storedProcCommand, "TotalCGSTAmount", DbType.Double, details.TotalCGSTAmount);
                this.dbServer.AddInParameter(storedProcCommand, "TotalIGSTAmount", DbType.Double, details.TotalIGSTAmount);
                this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, details.Remarks);
                this.dbServer.AddInParameter(storedProcCommand, "TotalAmount", DbType.Double, details.TotalAmount);
                this.dbServer.AddInParameter(storedProcCommand, "NetAmount", DbType.Double, details.NetAmount);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, details.Status);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, details.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierID", DbType.Int64, details.SupplierID);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, details.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.Details.ID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                BizActionObj.Details.GRNReturnNO = Convert.ToString(this.dbServer.GetParameterValue(storedProcCommand, "GRNReturnNO"));
                BizActionObj.Details.UnitId = UserVo.UserLoginInfo.UnitId;
                foreach (clsGRNReturnDetailsVO svo in details.Items)
                {
                    svo.GRNReturnID = BizActionObj.Details.ID;
                    svo.StockDetails.PurchaseRate = svo.Rate;
                    svo.GRNReturnUnitID = BizActionObj.Details.UnitId;
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddGRNReturnItems");
                    command2.Connection = myConnection;
                    this.dbServer.AddInParameter(command2, "LinkServer", DbType.String, details.LinkServer);
                    if (details.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(command2, "LinkServerAlias", DbType.String, details.LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddInParameter(command2, "GRNReturnID", DbType.Int64, svo.GRNReturnID);
                    this.dbServer.AddInParameter(command2, "GRNReturnUnitID", DbType.Int64, svo.GRNReturnUnitID);
                    this.dbServer.AddInParameter(command2, "ItemID", DbType.Int64, svo.ItemID);
                    this.dbServer.AddInParameter(command2, "BatchID", DbType.Int64, svo.BatchID);
                    this.dbServer.AddInParameter(command2, "AvailableQuantity", DbType.Double, svo.AvailableQuantity);
                    this.dbServer.AddInParameter(command2, "ReceivedQuantity", DbType.Double, svo.ReceivedQuantity);
                    this.dbServer.AddInParameter(command2, "ReturnedQuantity", DbType.Double, svo.ReturnedQuantity * svo.BaseConversionFactor);
                    this.dbServer.AddInParameter(command2, "Rate", DbType.Double, svo.Rate);
                    this.dbServer.AddInParameter(command2, "MRP", DbType.Double, svo.MRP);
                    this.dbServer.AddInParameter(command2, "Amount", DbType.Double, svo.Amount);
                    this.dbServer.AddInParameter(command2, "VATPercent", DbType.Double, svo.VATPercent);
                    this.dbServer.AddInParameter(command2, "VATAmount", DbType.Double, svo.VATAmount);
                    this.dbServer.AddInParameter(command2, "NetAmount", DbType.Double, svo.NetAmount);
                    this.dbServer.AddInParameter(command2, "Remarks", DbType.String, svo.Remarks);
                    this.dbServer.AddInParameter(command2, "ReturnReason", DbType.String, svo.ReturnReason);
                    this.dbServer.AddInParameter(command2, "GRNID", DbType.Int64, svo.GRNID);
                    this.dbServer.AddInParameter(command2, "GRNUnitID", DbType.Int64, svo.GRNUnitID);
                    this.dbServer.AddInParameter(command2, "GRNDetailID", DbType.Int64, svo.GRNDetailID);
                    this.dbServer.AddInParameter(command2, "GRNDetailUnitID", DbType.Int64, svo.GRNDetailUnitID);
                    this.dbServer.AddInParameter(command2, "StoreID", DbType.Int64, svo.StoreID);
                    this.dbServer.AddInParameter(command2, "InputTransactionQuantity", DbType.Double, svo.ReturnedQuantity);
                    this.dbServer.AddInParameter(command2, "TransactionUOMID", DbType.Int64, svo.TransactionUOMID);
                    this.dbServer.AddInParameter(command2, "BaseUOMID", DbType.Int64, svo.BaseUOMID);
                    this.dbServer.AddInParameter(command2, "BaseCF", DbType.Double, svo.BaseConversionFactor);
                    this.dbServer.AddInParameter(command2, "StockUOMID", DbType.Int64, svo.SUOMID);
                    this.dbServer.AddInParameter(command2, "StockCF", DbType.Double, svo.ConversionFactor);
                    this.dbServer.AddInParameter(command2, "StockingQuantity", DbType.Double, svo.ReturnedQuantity * svo.ConversionFactor);
                    this.dbServer.AddInParameter(command2, "CDiscountPercent", DbType.Double, svo.CDiscountPercent);
                    this.dbServer.AddInParameter(command2, "CDiscountAmount", DbType.Double, svo.CDiscountAmount);
                    this.dbServer.AddInParameter(command2, "SchDiscountPercent", DbType.Double, svo.SchDiscountPercent);
                    this.dbServer.AddInParameter(command2, "SchDiscountAmount", DbType.Double, svo.SchDiscountAmount);
                    this.dbServer.AddInParameter(command2, "ItemTax", DbType.Double, svo.ItemTax);
                    this.dbServer.AddInParameter(command2, "ItemTaxAmount", DbType.Double, svo.TaxAmount);
                    this.dbServer.AddInParameter(command2, "Vattype", DbType.Int32, svo.GRNItemVatType);
                    this.dbServer.AddInParameter(command2, "VatApplicableon", DbType.Int32, svo.GRNItemVatApplicationOn);
                    this.dbServer.AddInParameter(command2, "otherTaxType", DbType.Int32, svo.OtherGRNItemTaxType);
                    this.dbServer.AddInParameter(command2, "othertaxApplicableon", DbType.Int32, svo.OtherGRNItemTaxApplicationOn);
                    this.dbServer.AddInParameter(command2, "ReceivedID", DbType.Int64, svo.ReceivedID);
                    this.dbServer.AddInParameter(command2, "ReceivedUnitID", DbType.Int64, svo.ReceivedUnitID);
                    this.dbServer.AddInParameter(command2, "IsFreeItem", DbType.Boolean, svo.IsFreeItem);
                    this.dbServer.AddInParameter(command2, "SGSTTaxType", DbType.Int32, svo.GRNSGSTVatType);
                    this.dbServer.AddInParameter(command2, "SGSTApplicableOn", DbType.Int32, svo.GRNSGSTVatApplicationOn);
                    this.dbServer.AddInParameter(command2, "CGSTTaxType", DbType.Int32, svo.GRNCGSTVatType);
                    this.dbServer.AddInParameter(command2, "CGSTApplicableOn", DbType.Int32, svo.GRNCGSTVatApplicationOn);
                    this.dbServer.AddInParameter(command2, "IGSTTaxType", DbType.Int32, svo.GRNIGSTVatType);
                    this.dbServer.AddInParameter(command2, "IGSTApplicableOn", DbType.Int32, svo.GRNIGSTVatApplicationOn);
                    this.dbServer.AddInParameter(command2, "SGSTPercent", DbType.Double, svo.SGSTPercent);
                    this.dbServer.AddInParameter(command2, "SGSTAmount", DbType.Double, svo.SGSTAmount);
                    this.dbServer.AddInParameter(command2, "CGSTPercent", DbType.Double, svo.CGSTPercent);
                    this.dbServer.AddInParameter(command2, "CGSTAmount", DbType.Double, svo.CGSTAmount);
                    this.dbServer.AddInParameter(command2, "IGSTPercent", DbType.Double, svo.IGSTPercent);
                    this.dbServer.AddInParameter(command2, "IGSTAmount", DbType.Double, svo.IGSTAmount);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, -1);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                    svo.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                    long receivedDetailID = svo.ReceivedDetailID;
                    if (svo.ReceivedDetailID > 0L)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_UpdateReceive");
                        command2.Connection = myConnection;
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, svo.ReceivedUnitID);
                        this.dbServer.AddInParameter(command3, "BalanceQty", DbType.Decimal, svo.ReturnedQuantity * svo.BaseConversionFactor);
                        this.dbServer.AddInParameter(command3, "ReceiveItemDetailsID", DbType.Int64, svo.ReceivedDetailID);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                    }
                    svo.StockDetails.BatchID = svo.BatchID;
                    svo.StockDetails.OperationType = InventoryStockOperationType.Subtraction;
                    svo.StockDetails.ItemID = svo.ItemID;
                    svo.StockDetails.TransactionTypeID = InventoryTransactionType.GRNReturn;
                    svo.StockDetails.TransactionID = svo.GRNReturnID;
                    svo.StockDetails.TransactionQuantity = svo.ReturnedQuantity * svo.BaseConversionFactor;
                    svo.StockDetails.Date = details.Date;
                    svo.StockDetails.Time = details.Time;
                    svo.StockDetails.StoreID = new long?(svo.StoreID);
                    svo.StockDetails.InputTransactionQuantity = Convert.ToSingle(svo.ReturnedQuantity);
                    svo.StockDetails.BaseUOMID = svo.BaseUOMID;
                    svo.StockDetails.BaseConversionFactor = svo.BaseConversionFactor;
                    svo.StockDetails.SUOMID = svo.SUOMID;
                    svo.StockDetails.ConversionFactor = svo.ConversionFactor;
                    svo.StockDetails.StockingQuantity = svo.ReturnedQuantity * svo.ConversionFactor;
                    svo.StockDetails.SelectedUOM.ID = svo.SelectedUOM.ID;
                    svo.StockDetails.ExpiryDate = svo.ExpiryDate;
                    if (details.IsApproved)
                    {
                        clsBaseItemStockDAL instance = clsBaseItemStockDAL.GetInstance();
                        clsAddItemStockBizActionVO valueObject = new clsAddItemStockBizActionVO {
                            Details = svo.StockDetails
                        };
                        valueObject.Details.ID = 0L;
                        valueObject = (clsAddItemStockBizActionVO) instance.Add(valueObject, UserVo, myConnection, transaction);
                        if (valueObject.SuccessStatus == -1)
                        {
                            throw new Exception();
                        }
                        svo.StockDetails.ID = valueObject.Details.ID;
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

        public override IValueObject GetItemDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetGRNReturnDetailsListBizActionVO nvo = valueObject as clsGetGRNReturnDetailsListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetGRNReturnItemsList");
                this.dbServer.AddInParameter(storedProcCommand, "GRNReturnID", DbType.Int64, nvo.GRNReturnID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsGRNReturnDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsGRNReturnDetailsVO item = new clsGRNReturnDetailsVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitId = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            GRNReturnID = (long) DALHelper.HandleDBNull(reader["GRNReturnID"]),
                            ItemID = (long) DALHelper.HandleDBNull(reader["ItemID"]),
                            GRNID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNID"])),
                            GRNUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNUnitID"])),
                            GRNDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailID"])),
                            GRNDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailUnitID"])),
                            StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"])),
                            BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"])),
                            BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"]))
                        };
                        item.ReturnedQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["ReturnedQuantity"])) / item.BaseConversionFactor;
                        item.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCf"]));
                        item.SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));
                        item.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                        item.TransactionUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                        item.TransUOM = Convert.ToString(DALHelper.HandleDBNull(reader["ReturnedUOM"]));
                        item.ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]);
                        item.Rate = Convert.ToSingle(DALHelper.HandleDBNull(reader["Rate"]));
                        item.MRP = Convert.ToSingle(DALHelper.HandleDBNull(reader["MRP"]));
                        item.ItemTax = Convert.ToSingle(DALHelper.HandleDBNull(reader["ItemTax"]));
                        item.VATPercent = Convert.ToSingle(DALHelper.HandleDBNull(reader["VATPercent"]));
                        item.CDiscountPercent = Convert.ToSingle(DALHelper.HandleDBNull(reader["CDiscountPercent"]));
                        item.SchDiscountPercent = Convert.ToSingle(DALHelper.HandleDBNull(reader["SchDiscountPercent"]));
                        item.GRNItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Vattype"]));
                        item.GRNItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["VatApplicableon"]));
                        item.OtherGRNItemTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherTaxType"]));
                        item.OtherGRNItemTaxApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxApplicableon"]));
                        item.BatchCode = (string) DALHelper.HandleDBNull(reader["BatchCode"]);
                        item.ItemName = (string) DALHelper.HandleDBNull(reader["ItemName"]);
                        item.ReturnReason = (string) DALHelper.HandleDBNull(reader["ReturnReason"]);
                        item.ReceivedQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["ReceiveddQuantity"]));
                        item.ReceivedQuantityUOM = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedUOM"]));
                        item.IsFreeItem = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreeItem"]));
                        item.GRNNo = Convert.ToString(DALHelper.HandleDBNull(reader["GRNNo"]));
                        item.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"]));
                        item.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedUOM"]));
                        item.ReceivedNo = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedNumber"]));
                        item.SGSTPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["SGSTPercent"]));
                        if (item.SGSTPercent <= 0.0)
                        {
                            item.SGSTAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["SGSTAmount"]));
                        }
                        item.GRNSGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTApplicableOn"]));
                        item.GRNSGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTTaxType"]));
                        item.CGSTPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["CGSTPercent"]));
                        if (item.CGSTPercent <= 0.0)
                        {
                            item.CGSTAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["CGSTAmount"]));
                        }
                        item.GRNCGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTApplicableOn"]));
                        item.GRNCGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTTaxType"]));
                        item.IGSTPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["IGSTPercent"]));
                        if (item.IGSTPercent <= 0.0)
                        {
                            item.IGSTAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["IGSTAmount"]));
                        }
                        item.GRNIGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTApplicableOn"]));
                        item.GRNIGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTTaxType"]));
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

        public override IValueObject GetSearchList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetGRNReturnListBizActionVO nvo = valueObject as clsGetGRNReturnListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetGRNReturnSearchList");
                this.dbServer.AddInParameter(storedProcCommand, "SupplierID", DbType.Int64, nvo.SupplierID);
                if (nvo.FromDate != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                }
                if (nvo.ToDate != null)
                {
                    if (nvo.FromDate != null)
                    {
                        nvo.ToDate = new DateTime?(nvo.ToDate.Value.Date.AddDays(1.0));
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsGRNReturnVO>();
                    }
                    while (reader.Read())
                    {
                        clsGRNReturnVO item = new clsGRNReturnVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            SupplierName = (string) DALHelper.HandleDBNull(reader["Supplier"]),
                            StoreName = (string) DALHelper.HandleDBNull(reader["Store"]),
                            GRNID = (long) DALHelper.HandleDBNull(reader["GRNID"]),
                            GRNNO = (string) DALHelper.HandleDBNull(reader["GRNNO"]),
                            GRNReturnNO = (string) DALHelper.HandleDBNull(reader["GRNReturnNO"]),
                            GoodReturnType = (InventoryGoodReturnType) ((short) DALHelper.HandleDBNull(reader["GRNReturnType"])),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        DateTime? nullable5 = DALHelper.HandleDate(reader["Date"]);
                        item.Date = nullable5.Value;
                        item.UnitId = (long) DALHelper.HandleDBNull(reader["UnitID"]);
                        item.IsApproved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsApproved"]));
                        item.IsRejected = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRejected"]));
                        item.ApprovedOrRejectedBy = Convert.ToString(DALHelper.HandleDBNull(reader["ApproveOrRejectedBy"]));
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
            return valueObject;
        }

        public override IValueObject GRNReturnApprove(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddGRNReturnBizActionVO nvo = valueObject as clsAddGRNReturnBizActionVO;
            DbConnection myConnection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            try
            {
                myConnection.Open();
                transaction = myConnection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateGRNReturnForApprove");
                storedProcCommand.Connection = myConnection;
                this.dbServer.AddInParameter(storedProcCommand, "GRNReturnID", DbType.String, nvo.Details.ID);
                this.dbServer.AddInParameter(storedProcCommand, "GRNReturnUnitID", DbType.String, nvo.Details.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                if (Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus")) == -1)
                {
                    throw new Exception();
                }
                List<clsGRNReturnDetailsVO> gRNDetailsList = nvo.GRNDetailsList;
                if (gRNDetailsList.Count > 0)
                {
                    foreach (clsGRNReturnDetailsVO svo in gRNDetailsList.ToList<clsGRNReturnDetailsVO>())
                    {
                        svo.StockDetails.PurchaseRate = svo.Rate;
                        svo.StockDetails.BatchID = svo.BatchID;
                        svo.StockDetails.OperationType = InventoryStockOperationType.Subtraction;
                        svo.StockDetails.ItemID = svo.ItemID;
                        svo.StockDetails.TransactionTypeID = InventoryTransactionType.GRNReturn;
                        svo.StockDetails.TransactionID = svo.GRNReturnID;
                        svo.StockDetails.TransactionQuantity = svo.ReturnedQuantity * svo.BaseConversionFactor;
                        svo.StockDetails.Date = DateTime.Now;
                        svo.StockDetails.Time = DateTime.Now;
                        svo.StockDetails.StoreID = new long?(svo.StoreID);
                        svo.StockDetails.InputTransactionQuantity = Convert.ToSingle(svo.ReturnedQuantity);
                        svo.StockDetails.BaseUOMID = svo.BaseUOMID;
                        svo.StockDetails.BaseConversionFactor = svo.BaseConversionFactor;
                        svo.StockDetails.SUOMID = svo.SUOMID;
                        svo.StockDetails.ConversionFactor = svo.ConversionFactor;
                        svo.StockDetails.StockingQuantity = svo.ReturnedQuantity * svo.ConversionFactor;
                        svo.StockDetails.SelectedUOM.ID = svo.TransactionUOMID;
                        svo.StockDetails.ExpiryDate = svo.ExpiryDate;
                        clsBaseItemStockDAL instance = clsBaseItemStockDAL.GetInstance();
                        clsAddItemStockBizActionVO nvo2 = new clsAddItemStockBizActionVO {
                            Details = svo.StockDetails
                        };
                        nvo2.Details.ID = 0L;
                        nvo2 = (clsAddItemStockBizActionVO) instance.Add(nvo2, UserVo, myConnection, transaction);
                        if (nvo2.SuccessStatus == -1)
                        {
                            throw new Exception();
                        }
                        svo.StockDetails.ID = nvo2.Details.ID;
                    }
                }
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                nvo.SuccessStatus = -1;
                nvo.Details = null;
            }
            finally
            {
                myConnection.Close();
                transaction = null;
            }
            return valueObject;
        }

        public override IValueObject GRNReturnReject(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddGRNReturnBizActionVO nvo = valueObject as clsAddGRNReturnBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateGRNReturnForReject");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "GRNReturnID", DbType.String, nvo.Details.ID);
                this.dbServer.AddInParameter(storedProcCommand, "GRNReturnUnitID", DbType.String, nvo.Details.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "RejectionRemark", DbType.String, nvo.Details.RejectionRemarks);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception exception)
            {
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                nvo.SuccessStatus = -1;
                nvo.Details = null;
            }
            finally
            {
                connection.Close();
            }
            return valueObject;
        }
    }
}

