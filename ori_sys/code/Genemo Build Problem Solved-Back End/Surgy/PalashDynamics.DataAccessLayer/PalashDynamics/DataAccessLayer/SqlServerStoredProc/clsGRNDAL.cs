namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Inventory;
    using PalashDynamics.ValueObjects.Log;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Reflection;
    using System.Text;

    public class clsGRNDAL : clsBaseGRNDAL
    {
        private Database dbServer;
        private LogManager logManager;
        private bool IsAuditTrail;

        private clsGRNDAL()
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
                this.IsAuditTrail = Convert.ToBoolean(HMSConfigurationManager.GetValueFromApplicationConfig("IsAuditTrail"));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override IValueObject Add(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddGRNBizActionVO bizActionObj = valueObject as clsAddGRNBizActionVO;
            bizActionObj = this.AddDetails(bizActionObj, UserVo);
            return valueObject;
        }

        private clsAddGRNBizActionVO AddDetails(clsAddGRNBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbConnection myConnection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            int num = 0;
            try
            {
                myConnection.Open();
                transaction = myConnection.BeginTransaction();
                clsGRNVO details = BizActionObj.Details;
                if (!details.DirectApprove)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddGRN");
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
                    this.dbServer.AddInParameter(storedProcCommand, "POID", DbType.Int64, details.POID);
                    this.dbServer.AddInParameter(storedProcCommand, "PaymentModeID", DbType.Int16, (int) details.PaymentModeID);
                    this.dbServer.AddInParameter(storedProcCommand, "ReceivedByID", DbType.Int64, details.ReceivedByID);
                    this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, details.Remarks);
                    this.dbServer.AddInParameter(storedProcCommand, "TotalCDiscount", DbType.Double, details.TotalCDiscount);
                    this.dbServer.AddInParameter(storedProcCommand, "TotalSchDiscount", DbType.Double, details.TotalSchDiscount);
                    this.dbServer.AddInParameter(storedProcCommand, "TotalVAT", DbType.Double, details.TotalVAT);
                    this.dbServer.AddInParameter(storedProcCommand, "TotalAmount", DbType.Double, details.TotalAmount);
                    this.dbServer.AddInParameter(storedProcCommand, "TotalSGST", DbType.Double, details.TotalSGST);
                    this.dbServer.AddInParameter(storedProcCommand, "TotalCGST", DbType.Double, details.TotalCGST);
                    this.dbServer.AddInParameter(storedProcCommand, "TotalIGST", DbType.Double, details.TotalIGST);
                    this.dbServer.AddInParameter(storedProcCommand, "PrevNetAmount", DbType.Double, details.PrevNetAmount);
                    this.dbServer.AddInParameter(storedProcCommand, "OtherCharges", DbType.Double, details.OtherCharges);
                    this.dbServer.AddInParameter(storedProcCommand, "GRNDiscount", DbType.Double, details.GRNDiscount);
                    this.dbServer.AddInParameter(storedProcCommand, "GRNRoundOff", DbType.Double, details.GRNRoundOff);
                    this.dbServer.AddInParameter(storedProcCommand, "EditForApprove", DbType.Boolean, BizActionObj.Details.EditForApprove);
                    this.dbServer.AddInParameter(storedProcCommand, "NetAmount", DbType.Double, details.GRNRoundOff);
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
                }
                foreach (clsGRNDetailsVO svo in details.Items)
                {
                    svo.GRNID = BizActionObj.Details.ID;
                    svo.GRNUnitID = BizActionObj.Details.UnitId;
                    svo.StockDetails.PurchaseRate = svo.Rate;
                    svo.StockDetails.MRP = svo.MRP;
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateItemBatchByGRN");
                    storedProcCommand.Connection = myConnection;
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, details.LinkServer);
                    if (details.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, details.LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "POID", DbType.Int64, svo.POID);
                    this.dbServer.AddInParameter(storedProcCommand, "POUnitID", DbType.Int64, svo.POUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "GRNID", DbType.Int64, svo.GRNID);
                    this.dbServer.AddInParameter(storedProcCommand, "GRNUnitID", DbType.Int64, svo.GRNUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, svo.ItemID);
                    this.dbServer.AddParameter(storedProcCommand, "BatchID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.BatchID);
                    this.dbServer.AddInParameter(storedProcCommand, "BatchCode", DbType.String, svo.BatchCode);
                    this.dbServer.AddInParameter(storedProcCommand, "ExpiryDate", DbType.DateTime, svo.ExpiryDate);
                    this.dbServer.AddInParameter(storedProcCommand, "Quantity", DbType.Double, svo.Quantity * svo.BaseConversionFactor);
                    this.dbServer.AddInParameter(storedProcCommand, "FreeQuantity", DbType.Double, svo.FreeQuantity);
                    this.dbServer.AddInParameter(storedProcCommand, "CoversionFactor", DbType.Double, svo.BaseConversionFactor);
                    this.dbServer.AddInParameter(storedProcCommand, "InputTransactionQuantity", DbType.Double, svo.Quantity);
                    this.dbServer.AddInParameter(storedProcCommand, "TransactionUOMID", DbType.Double, svo.SelectedUOM.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "BaseUMID", DbType.Double, svo.BaseUOMID);
                    this.dbServer.AddInParameter(storedProcCommand, "StockUOMID", DbType.Double, svo.SUOMID);
                    this.dbServer.AddInParameter(storedProcCommand, "StockCF", DbType.Double, svo.ConversionFactor);
                    this.dbServer.AddInParameter(storedProcCommand, "StockingQuantity", DbType.Double, svo.Quantity * svo.ConversionFactor);
                    this.dbServer.AddInParameter(storedProcCommand, "Rate", DbType.Double, svo.Rate);
                    this.dbServer.AddInParameter(storedProcCommand, "MRP", DbType.Double, svo.MRP);
                    this.dbServer.AddInParameter(storedProcCommand, "POQty", DbType.Double, svo.POQuantity);
                    this.dbServer.AddInParameter(storedProcCommand, "PoPendingQty", DbType.Double, svo.PendingQuantity);
                    this.dbServer.AddInParameter(storedProcCommand, "ActualPendingQty", DbType.Double, svo.POPendingQuantity);
                    this.dbServer.AddInParameter(storedProcCommand, "Amount", DbType.Double, svo.Amount);
                    this.dbServer.AddInParameter(storedProcCommand, "VATPercent", DbType.Double, svo.VATPercent);
                    this.dbServer.AddInParameter(storedProcCommand, "VATAmount", DbType.Double, svo.VATAmount);
                    this.dbServer.AddInParameter(storedProcCommand, "SGSTPercent", DbType.Double, svo.SGSTPercent);
                    this.dbServer.AddInParameter(storedProcCommand, "SGSTAmount", DbType.Double, svo.SGSTAmount);
                    this.dbServer.AddInParameter(storedProcCommand, "CGSTPercent", DbType.Double, svo.CGSTPercent);
                    this.dbServer.AddInParameter(storedProcCommand, "CGSTAmount", DbType.Double, svo.CGSTAmount);
                    this.dbServer.AddInParameter(storedProcCommand, "IGSTPercent", DbType.Double, svo.IGSTPercent);
                    this.dbServer.AddInParameter(storedProcCommand, "IGSTAmount", DbType.Double, svo.IGSTAmount);
                    this.dbServer.AddInParameter(storedProcCommand, "CDiscountPercent", DbType.Double, svo.CDiscountPercent);
                    this.dbServer.AddInParameter(storedProcCommand, "CDiscountAmount", DbType.Double, svo.CDiscountAmount);
                    this.dbServer.AddInParameter(storedProcCommand, "SchDiscountPercent", DbType.Double, svo.SchDiscountPercent);
                    this.dbServer.AddInParameter(storedProcCommand, "SchDiscountAmount", DbType.Double, svo.SchDiscountAmount);
                    this.dbServer.AddInParameter(storedProcCommand, "ItemTax", DbType.Double, svo.ItemTax);
                    this.dbServer.AddInParameter(storedProcCommand, "ItemTaxAmount", DbType.Double, svo.TaxAmount);
                    this.dbServer.AddInParameter(storedProcCommand, "Vattype", DbType.Int32, svo.GRNItemVatType);
                    this.dbServer.AddInParameter(storedProcCommand, "VatApplicableon", DbType.Int32, svo.GRNItemVatApplicationOn);
                    this.dbServer.AddInParameter(storedProcCommand, "otherTaxType", DbType.Int32, svo.OtherGRNItemTaxType);
                    this.dbServer.AddInParameter(storedProcCommand, "othertaxApplicableon", DbType.Int32, svo.OtherGRNItemTaxApplicationOn);
                    this.dbServer.AddInParameter(storedProcCommand, "SGSTTaxType", DbType.Int32, svo.GRNSGSTVatType);
                    this.dbServer.AddInParameter(storedProcCommand, "SGSTApplicableOn", DbType.Int32, svo.GRNSGSTVatApplicationOn);
                    this.dbServer.AddInParameter(storedProcCommand, "CGSTTaxType", DbType.Int32, svo.GRNCGSTVatType);
                    this.dbServer.AddInParameter(storedProcCommand, "CGSTApplicableOn", DbType.Int32, svo.GRNCGSTVatApplicationOn);
                    this.dbServer.AddInParameter(storedProcCommand, "IGSTTaxType", DbType.Int32, svo.GRNIGSTVatType);
                    this.dbServer.AddInParameter(storedProcCommand, "IGSTApplicableOn", DbType.Int32, svo.GRNIGSTVatApplicationOn);
                    this.dbServer.AddInParameter(storedProcCommand, "NetAmount", DbType.Double, svo.NetAmount);
                    this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, svo.Remarks);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, details.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "IsConsignment", DbType.Boolean, details.IsConsignment);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "EditForApprove", DbType.Boolean, BizActionObj.Details.EditForApprove);
                    this.dbServer.AddInParameter(storedProcCommand, "DirectApprove", DbType.Boolean, BizActionObj.Details.DirectApprove);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, details.AddedDateTime);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(storedProcCommand, "AssignSupplier", DbType.Boolean, svo.AssignSupplier);
                    this.dbServer.AddInParameter(storedProcCommand, "SupplierId", DbType.Int64, details.SupplierID);
                    this.dbServer.AddInParameter(storedProcCommand, "BarCode", DbType.String, svo.BarCode);
                    this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, details.Date);
                    this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, details.Time);
                    this.dbServer.AddInParameter(storedProcCommand, "GRNQtyOnHand", DbType.Double, svo.BaseAvailableQuantity);
                    this.dbServer.AddInParameter(storedProcCommand, "SrNo", DbType.Int64, svo.SrNo);
                    this.dbServer.AddInParameter(storedProcCommand, "AvgCost", DbType.Decimal, svo.AvgCost);
                    this.dbServer.AddInParameter(storedProcCommand, "AvgCostAmount", DbType.Double, svo.AvgCostAmount);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, -1);
                    this.dbServer.AddParameter(storedProcCommand, "GRNDetailID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.GRNDetailID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    svo.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "GRNDetailID");
                    svo.BatchID = (long) this.dbServer.GetParameterValue(storedProcCommand, "BatchID");
                    if ((details.ItemsPOGRN.Count > 0) && !details.DirectApprove)
                    {
                        int num2 = 0;
                        foreach (clsGRNDetailsVO svo2 in details.ItemsPOGRN)
                        {
                            StringBuilder builder = new StringBuilder();
                            StringBuilder builder2 = new StringBuilder();
                            if (svo2.ItemID == svo.ItemID)
                            {
                                foreach (clsGRNDetailsVO svo3 in details.ItemsPOGRN)
                                {
                                    if (svo3.ItemID == svo2.ItemID)
                                    {
                                        builder.Append("," + Convert.ToString(svo3.POID));
                                        builder2.Append("," + Convert.ToString(svo3.PoItemsID));
                                    }
                                }
                                builder.Replace(",", "", 0, 1);
                                builder2.Replace(",", "", 0, 1);
                                DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddPOGRNItemsLink");
                                command.Connection = myConnection;
                                this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command, "GRNID", DbType.Int64, details.ID);
                                this.dbServer.AddInParameter(command, "GRNUnitID", DbType.Int64, details.UnitId);
                                this.dbServer.AddInParameter(command, "GRNDetailID", DbType.Int64, svo.ID);
                                this.dbServer.AddInParameter(command, "TempResult", DbType.Int64, num2);
                                this.dbServer.AddInParameter(command, "GRNDetailUnitID", DbType.Int64, details.UnitId);
                                this.dbServer.AddInParameter(command, "POID", DbType.Int64, svo2.POID);
                                this.dbServer.AddInParameter(command, "POUnitID", DbType.Int64, svo2.POUnitID);
                                this.dbServer.AddInParameter(command, "PODetailsID", DbType.Int64, svo2.PoItemsID);
                                this.dbServer.AddInParameter(command, "PODetailsUnitID", DbType.Int64, svo2.POUnitID);
                                this.dbServer.AddInParameter(command, "ItemID", DbType.Int64, svo2.ItemID);
                                this.dbServer.AddInParameter(command, "Quantity", DbType.Decimal, svo2.Quantity);
                                this.dbServer.AddInParameter(command, "PendingQty", DbType.Decimal, svo2.POPendingQuantity);
                                this.dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                                this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, details.AddedDateTime);
                                this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                this.dbServer.AddInParameter(command, "BaseQuantity", DbType.Decimal, svo.Quantity * Convert.ToSingle(svo.BaseConversionFactor));
                                this.dbServer.AddInParameter(command, "IsForApprove", DbType.Boolean, details.EditForApprove);
                                this.dbServer.AddInParameter(command, "POIDList", DbType.String, Convert.ToString(builder));
                                this.dbServer.AddInParameter(command, "PODetailsIDList", DbType.String, Convert.ToString(builder2));
                                this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, -1);
                                this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0);
                                this.dbServer.ExecuteNonQuery(command, transaction);
                                num = Convert.ToInt32(this.dbServer.GetParameterValue(command, "ResultStatus"));
                                if (num == 0)
                                {
                                    num2 = 1;
                                }
                                if (num == 1)
                                {
                                    BizActionObj.ItemName = svo.ItemName;
                                    throw new Exception();
                                }
                            }
                        }
                    }
                    if (!BizActionObj.IsDraft && (details.Freezed && (details.EditForApprove && (svo.POID > 0L))))
                    {
                        DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_UpdatePOFromGRN");
                        command.Connection = myConnection;
                        this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "BalanceQty", DbType.Decimal, svo.Quantity * svo.BaseConversionFactor);
                        this.dbServer.AddInParameter(command, "POItemsID", DbType.Int64, svo.ItemID);
                        this.dbServer.AddInParameter(command, "PendingQty", DbType.Decimal, svo.POPendingQuantity);
                        this.dbServer.AddInParameter(command, "IndentID", DbType.Decimal, svo.IndentID);
                        this.dbServer.AddInParameter(command, "IndentUnitID", DbType.Decimal, svo.IndentUnitID);
                        this.dbServer.AddInParameter(command, "POID", DbType.Int64, svo.POID);
                        this.dbServer.AddInParameter(command, "POUnitID", DbType.Int64, svo.POUnitID);
                        this.dbServer.AddInParameter(command, "GRNID", DbType.Int64, svo.GRNID);
                        this.dbServer.AddInParameter(command, "GRNUnitID", DbType.Int64, svo.GRNUnitID);
                        this.dbServer.ExecuteNonQuery(command, transaction);
                    }
                    svo.StockDetails.BatchID = svo.BatchID;
                    svo.StockDetails.OperationType = InventoryStockOperationType.Addition;
                    svo.StockDetails.ItemID = svo.ItemID;
                    svo.StockDetails.TransactionTypeID = InventoryTransactionType.GoodsReceivedNote;
                    svo.StockDetails.TransactionID = svo.GRNID;
                    svo.StockDetails.BatchCode = svo.BatchCode;
                    svo.StockDetails.Date = details.Date;
                    svo.StockDetails.Time = details.Time;
                    svo.StockDetails.StoreID = new long?(details.StoreID);
                    svo.StockDetails.InputTransactionQuantity = Convert.ToSingle(svo.Quantity);
                    svo.StockDetails.BaseUOMID = svo.BaseUOMID;
                    svo.StockDetails.BaseConversionFactor = svo.BaseConversionFactor;
                    svo.StockDetails.TransactionQuantity = svo.Quantity * Convert.ToSingle(svo.BaseConversionFactor);
                    svo.StockDetails.SUOMID = svo.SUOMID;
                    svo.StockDetails.ConversionFactor = svo.ConversionFactor;
                    svo.StockDetails.StockingQuantity = svo.Quantity * svo.ConversionFactor;
                    svo.StockDetails.SelectedUOM.ID = svo.SelectedUOM.ID;
                    svo.StockDetails.ExpiryDate = svo.ExpiryDate;
                    svo.StockDetails.IsFromOpeningBalance = true;
                    svo.StockDetails.MRP = svo.MRP / ((double) svo.BaseConversionFactor);
                    svo.StockDetails.PurchaseRate = svo.Rate / ((double) svo.BaseConversionFactor);
                    if (details.Freezed && details.EditForApprove)
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
                if ((details.ItemsDeletedMain != null) && (details.ItemsDeletedMain.Count > 0))
                {
                    foreach (clsGRNDetailsVO svo4 in details.ItemsDeletedMain)
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteGRNMainDetails");
                        storedProcCommand.Connection = myConnection;
                        this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, svo4.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, svo4.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "GRNID", DbType.Int64, svo4.GRNID);
                        this.dbServer.AddInParameter(storedProcCommand, "GRNUnitID", DbType.Int64, svo4.UnitId);
                        this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    }
                }
                foreach (clsGRNDetailsVO svo5 in details.Items)
                {
                    foreach (clsGRNDetailsFreeVO evo in details.ItemsFree)
                    {
                        if ((svo5.SrNo == evo.MainSrNo) && (svo5.ItemID == evo.MainItemID))
                        {
                            evo.GRNID = BizActionObj.Details.ID;
                            evo.GRNUnitID = BizActionObj.Details.UnitId;
                            evo.StockDetails.PurchaseRate = evo.Rate;
                            evo.StockDetails.MRP = evo.MRP;
                            DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateItemBatchByGRNForFree");
                            storedProcCommand.Connection = myConnection;
                            this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, details.LinkServer);
                            if (details.LinkServer != null)
                            {
                                this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, details.LinkServer.Replace(@"\", "_"));
                            }
                            this.dbServer.AddInParameter(storedProcCommand, "POID", DbType.Int64, evo.POID);
                            this.dbServer.AddInParameter(storedProcCommand, "POUnitID", DbType.Int64, evo.POUnitID);
                            this.dbServer.AddInParameter(storedProcCommand, "GRNID", DbType.Int64, evo.GRNID);
                            this.dbServer.AddInParameter(storedProcCommand, "GRNUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, evo.FreeItemID);
                            this.dbServer.AddParameter(storedProcCommand, "BatchID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.FreeBatchID);
                            this.dbServer.AddInParameter(storedProcCommand, "BatchCode", DbType.String, evo.FreeBatchCode);
                            this.dbServer.AddInParameter(storedProcCommand, "ExpiryDate", DbType.DateTime, evo.FreeExpiryDate);
                            this.dbServer.AddInParameter(storedProcCommand, "Quantity", DbType.Double, evo.Quantity * evo.BaseConversionFactor);
                            this.dbServer.AddInParameter(storedProcCommand, "FreeQuantity", DbType.Double, evo.FreeQuantity);
                            this.dbServer.AddInParameter(storedProcCommand, "CoversionFactor", DbType.Double, evo.BaseConversionFactor);
                            this.dbServer.AddInParameter(storedProcCommand, "InputTransactionQuantity", DbType.Double, evo.Quantity);
                            this.dbServer.AddInParameter(storedProcCommand, "TransactionUOMID", DbType.Double, evo.SelectedUOM.ID);
                            this.dbServer.AddInParameter(storedProcCommand, "BaseUMID", DbType.Double, evo.BaseUOMID);
                            this.dbServer.AddInParameter(storedProcCommand, "StockUOMID", DbType.Double, evo.SUOMID);
                            this.dbServer.AddInParameter(storedProcCommand, "StockCF", DbType.Double, evo.ConversionFactor);
                            this.dbServer.AddInParameter(storedProcCommand, "StockingQuantity", DbType.Double, evo.Quantity * evo.ConversionFactor);
                            this.dbServer.AddInParameter(storedProcCommand, "Rate", DbType.Double, evo.Rate);
                            this.dbServer.AddInParameter(storedProcCommand, "MRP", DbType.Double, evo.MRP);
                            this.dbServer.AddInParameter(storedProcCommand, "POQty", DbType.Double, evo.POQuantity);
                            this.dbServer.AddInParameter(storedProcCommand, "PoPendingQty", DbType.Double, evo.PendingQuantity);
                            this.dbServer.AddInParameter(storedProcCommand, "ActualPendingQty", DbType.Double, evo.POPendingQuantity);
                            this.dbServer.AddInParameter(storedProcCommand, "Amount", DbType.Double, evo.Amount);
                            this.dbServer.AddInParameter(storedProcCommand, "VATPercent", DbType.Double, evo.VATPercent);
                            this.dbServer.AddInParameter(storedProcCommand, "VATAmount", DbType.Double, evo.VATAmount);
                            this.dbServer.AddInParameter(storedProcCommand, "SGSTPercent", DbType.Double, evo.SGSTPercent);
                            this.dbServer.AddInParameter(storedProcCommand, "SGSTAmount", DbType.Double, evo.SGSTAmount);
                            this.dbServer.AddInParameter(storedProcCommand, "CGSTPercent", DbType.Double, evo.CGSTPercent);
                            this.dbServer.AddInParameter(storedProcCommand, "CGSTAmount", DbType.Double, evo.CGSTAmount);
                            this.dbServer.AddInParameter(storedProcCommand, "IGSTPercent", DbType.Double, evo.IGSTPercent);
                            this.dbServer.AddInParameter(storedProcCommand, "IGSTAmount", DbType.Double, evo.IGSTAmount);
                            this.dbServer.AddInParameter(storedProcCommand, "CDiscountPercent", DbType.Double, evo.CDiscountPercent);
                            this.dbServer.AddInParameter(storedProcCommand, "CDiscountAmount", DbType.Double, evo.CDiscountAmount);
                            this.dbServer.AddInParameter(storedProcCommand, "SchDiscountPercent", DbType.Double, evo.SchDiscountPercent);
                            this.dbServer.AddInParameter(storedProcCommand, "SchDiscountAmount", DbType.Double, evo.SchDiscountAmount);
                            this.dbServer.AddInParameter(storedProcCommand, "ItemTax", DbType.Double, evo.ItemTax);
                            this.dbServer.AddInParameter(storedProcCommand, "ItemTaxAmount", DbType.Double, evo.TaxAmount);
                            this.dbServer.AddInParameter(storedProcCommand, "Vattype", DbType.Int32, evo.GRNItemVatType);
                            this.dbServer.AddInParameter(storedProcCommand, "VatApplicableon", DbType.Int32, evo.GRNItemVatApplicationOn);
                            this.dbServer.AddInParameter(storedProcCommand, "otherTaxType", DbType.Int32, evo.OtherGRNItemTaxType);
                            this.dbServer.AddInParameter(storedProcCommand, "othertaxApplicableon", DbType.Int32, evo.OtherGRNItemTaxApplicationOn);
                            this.dbServer.AddInParameter(storedProcCommand, "SGSTTaxType", DbType.Int32, evo.GRNSGSTVatType);
                            this.dbServer.AddInParameter(storedProcCommand, "SGSTApplicableOn", DbType.Int32, evo.GRNSGSTVatApplicationOn);
                            this.dbServer.AddInParameter(storedProcCommand, "CGSTTaxType", DbType.Int32, evo.GRNCGSTVatType);
                            this.dbServer.AddInParameter(storedProcCommand, "CGSTApplicableOn", DbType.Int32, evo.GRNCGSTVatApplicationOn);
                            this.dbServer.AddInParameter(storedProcCommand, "IGSTTaxType", DbType.Int32, evo.GRNIGSTVatType);
                            this.dbServer.AddInParameter(storedProcCommand, "IGSTApplicableOn", DbType.Int32, evo.GRNIGSTVatApplicationOn);
                            this.dbServer.AddInParameter(storedProcCommand, "NetAmount", DbType.Double, evo.NetAmount);
                            this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, evo.Remarks);
                            this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, details.Status);
                            this.dbServer.AddInParameter(storedProcCommand, "IsConsignment", DbType.Boolean, details.IsConsignment);
                            this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(storedProcCommand, "EditForApprove", DbType.Boolean, BizActionObj.Details.EditForApprove);
                            this.dbServer.AddInParameter(storedProcCommand, "DirectApprove", DbType.Boolean, BizActionObj.Details.DirectApprove);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, details.AddedDateTime);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(storedProcCommand, "AssignSupplier", DbType.Boolean, evo.AssignSupplier);
                            this.dbServer.AddInParameter(storedProcCommand, "SupplierId", DbType.Int64, details.SupplierID);
                            this.dbServer.AddInParameter(storedProcCommand, "BarCode", DbType.String, evo.BarCode);
                            this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, details.Date);
                            this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, details.Time);
                            this.dbServer.AddInParameter(storedProcCommand, "GRNQtyOnHand", DbType.Double, evo.BaseAvailableQuantity);
                            this.dbServer.AddInParameter(storedProcCommand, "MainGRNDetailId", DbType.Int64, svo5.ID);
                            this.dbServer.AddInParameter(storedProcCommand, "MainGRNDetailUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(storedProcCommand, "MainItemID", DbType.Int64, svo5.ItemID);
                            this.dbServer.AddInParameter(storedProcCommand, "MainBatchCode", DbType.String, svo5.BatchCode);
                            this.dbServer.AddInParameter(storedProcCommand, "MainExpiryDate", DbType.DateTime, svo5.ExpiryDate);
                            this.dbServer.AddInParameter(storedProcCommand, "MainMRP", DbType.Double, svo5.MRP);
                            this.dbServer.AddInParameter(storedProcCommand, "MainRate", DbType.Double, svo5.Rate);
                            this.dbServer.AddInParameter(storedProcCommand, "MainBatchID", DbType.Int64, svo5.BatchID);
                            this.dbServer.AddInParameter(storedProcCommand, "MainSrNo", DbType.Int64, evo.MainSrNo);
                            this.dbServer.AddInParameter(storedProcCommand, "AvgCost", DbType.Decimal, svo5.AvgCost);
                            this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, -1);
                            this.dbServer.AddParameter(storedProcCommand, "GRNDetailID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.FreeGRNDetailID);
                            this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                            BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                            evo.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "GRNDetailID");
                            evo.FreeBatchID = (long) this.dbServer.GetParameterValue(storedProcCommand, "BatchID");
                            evo.StockDetails.BatchID = evo.FreeBatchID;
                            evo.StockDetails.OperationType = InventoryStockOperationType.Addition;
                            evo.StockDetails.ItemID = evo.FreeItemID;
                            evo.StockDetails.TransactionTypeID = InventoryTransactionType.GoodsReceivedNote;
                            evo.StockDetails.TransactionID = evo.GRNID;
                            evo.StockDetails.BatchCode = evo.FreeBatchCode;
                            evo.StockDetails.Date = details.Date;
                            evo.StockDetails.Time = details.Time;
                            evo.StockDetails.StoreID = new long?(details.StoreID);
                            evo.StockDetails.InputTransactionQuantity = Convert.ToSingle(evo.Quantity);
                            evo.StockDetails.BaseUOMID = evo.BaseUOMID;
                            evo.StockDetails.BaseConversionFactor = evo.BaseConversionFactor;
                            evo.StockDetails.TransactionQuantity = evo.Quantity * Convert.ToSingle(evo.BaseConversionFactor);
                            evo.StockDetails.SUOMID = evo.SUOMID;
                            evo.StockDetails.ConversionFactor = evo.ConversionFactor;
                            evo.StockDetails.StockingQuantity = evo.Quantity * evo.ConversionFactor;
                            evo.StockDetails.SelectedUOM.ID = evo.SelectedUOM.ID;
                            evo.StockDetails.ExpiryDate = evo.FreeExpiryDate;
                            evo.StockDetails.IsFromOpeningBalance = true;
                            evo.StockDetails.MRP = evo.MRP / ((double) evo.BaseConversionFactor);
                            evo.StockDetails.PurchaseRate = evo.Rate / ((double) evo.BaseConversionFactor);
                            if (details.Freezed && details.EditForApprove)
                            {
                                clsBaseItemStockDAL instance = clsBaseItemStockDAL.GetInstance();
                                clsAddItemStockBizActionVO valueObject = new clsAddItemStockBizActionVO {
                                    Details = evo.StockDetails
                                };
                                valueObject.Details.ID = 0L;
                                if (BizActionObj.IsDraft)
                                {
                                    evo.StockDetails.ID = 0L;
                                }
                                else
                                {
                                    valueObject = (clsAddItemStockBizActionVO) instance.AddFree(valueObject, UserVo, myConnection, transaction);
                                    if (valueObject.SuccessStatus == -1)
                                    {
                                        throw new Exception("Error");
                                    }
                                    evo.StockDetails.ID = valueObject.Details.ID;
                                }
                            }
                        }
                    }
                }
                if ((details.ItemsDeletedFree != null) && (details.ItemsDeletedFree.Count > 0))
                {
                    foreach (clsGRNDetailsFreeVO evo2 in details.ItemsDeletedFree)
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteGRNFreeDetails");
                        storedProcCommand.Connection = myConnection;
                        this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, evo2.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, evo2.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "GRNID", DbType.Int64, evo2.GRNID);
                        this.dbServer.AddInParameter(storedProcCommand, "GRNUnitID", DbType.Int64, evo2.UnitId);
                        this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    }
                }
                if (details.Freezed && details.EditForApprove)
                {
                    clsBaseGRNDAL instance = GetInstance();
                    clsUpdateGRNForApprovalVO valueObject = new clsUpdateGRNForApprovalVO {
                        Details = details
                    };
                    if (!BizActionObj.IsDraft && (((clsUpdateGRNForApprovalVO) instance.UpdateGRNForApproval(valueObject, UserVo, myConnection, transaction)).SuccessStatus == -1))
                    {
                        throw new Exception("Error");
                    }
                }
                if ((details.GRNType == InventoryGRNType.AgainstPO) && (details.Freezed && details.EditForApprove))
                {
                    StringBuilder builder3 = new StringBuilder();
                    StringBuilder builder4 = new StringBuilder();
                    foreach (clsGRNDetailsVO svo6 in details.Items)
                    {
                        if (svo6.POID > 0L)
                        {
                            builder3.Append("," + Convert.ToString(svo6.POID));
                            builder4.Append("," + Convert.ToString(svo6.POUnitID));
                        }
                    }
                    builder3.Replace(",", "", 0, 1);
                    builder4.Replace(",", "", 0, 1);
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ChangeIndentStatusFromGRNAtSameIndentingSore");
                    storedProcCommand.Connection = myConnection;
                    this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, details.StoreID);
                    this.dbServer.AddInParameter(storedProcCommand, "ipPOIDList", DbType.String, Convert.ToString(builder3));
                    this.dbServer.AddInParameter(storedProcCommand, "ipPOUnitIDList", DbType.String, Convert.ToString(builder4));
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                }
                transaction.Commit();
                if (this.IsAuditTrail && (BizActionObj.LogInfoList != null))
                {
                    if (details.Items.Count > 0)
                    {
                        LogInfo item = new LogInfo();
                        Guid guid = default(Guid);
                        item.guid = guid;
                        item.UserId = UserVo.ID;
                        item.TimeStamp = DateTime.Now;
                        item.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                        item.MethodName = MethodBase.GetCurrentMethod().ToString();
                        string[] strArray = new string[] { " 140 : Get Output Parameters From T_GRN Unit Id : ", Convert.ToString(UserVo.UserLoginInfo.UnitId), "  , GRNNO :", Convert.ToString(BizActionObj.Details.GRNNO), " , GRN ID : ", Convert.ToString(BizActionObj.Details.ID), " , GRN UnitId : ", Convert.ToString(BizActionObj.Details.UnitId) };
                        item.Message = string.Concat(strArray);
                        BizActionObj.LogInfoList.Add(item);
                    }
                    if ((BizActionObj.LogInfoList.Count > 0) && this.IsAuditTrail)
                    {
                        this.SetLogInfo(BizActionObj.LogInfoList, UserVo.ID);
                        BizActionObj.LogInfoList.Clear();
                    }
                }
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                BizActionObj.SuccessStatus = (num != 1) ? -1 : -2;
                BizActionObj.Details = null;
            }
            finally
            {
                myConnection.Close();
                transaction = null;
            }
            return BizActionObj;
        }

        private clsUpdateGRNForApprovalVO ApproveGRN(clsUpdateGRNForApprovalVO BizActionObj, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
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
                clsGRNVO details = BizActionObj.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdaterGRNApprovalStatus");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "ApprovedOrRejectedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, details.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, details.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ApprovedOrRejectedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
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

        public override IValueObject DeleteGRNItems(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddGRNBizActionVO nvo = null;
            clsGRNVO details = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteGRNItemDetails");
                nvo = valueObject as clsAddGRNBizActionVO;
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

        public override IValueObject GetGRNForGRNReturn(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetGRNDetailsForGRNReturnListBizActionVO nvo = valueObject as clsGetGRNDetailsForGRNReturnListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetGRNItemsForGRNReturnList");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "GRNID", DbType.Int64, nvo.GRNID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Freezed", DbType.Int64, nvo.Freezed);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsGRNDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsGRNDetailsVO item = new clsGRNDetailsVO {
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
                        item.POPendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        item.POQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["POIQuantity"]));
                        item.PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        item.POID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POID"]));
                        item.POUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POrderUnitId"]));
                        item.PODate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["POrderDate"]));
                        item.StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        item.PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PurchaseUOM"]));
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
                        item.PONO = Convert.ToString(DALHelper.HandleDBNull(reader["PONO"]));
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

        public override IValueObject GetGRNInvoiceFile(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetGRNListBizActionVO nvo = valueObject as clsGetGRNListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetGRNInvoiceFile");
                this.dbServer.AddInParameter(storedProcCommand, "GRNID", DbType.String, nvo.GRNVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "GRNUnitId", DbType.Int64, nvo.GRNVO.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsGRNVO sgrnvo = new clsGRNVO {
                            FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                            File = (byte[]) DALHelper.HandleDBNull(reader["FileData"]),
                            IsFileAttached = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFileAttached"]))
                        };
                        nvo.GRNVO = sgrnvo;
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetGRNItemsForGRNReturnQSSearch(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetGRNDetailsForGRNReturnListBizActionVO nvo = valueObject as clsGetGRNDetailsForGRNReturnListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetGRNItemsForGRNReturnQSSearch");
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "StoreId", DbType.Int64, nvo.StoreId);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierId", DbType.Int64, nvo.SupplierId);
                this.dbServer.AddInParameter(storedProcCommand, "GRNNo", DbType.String, nvo.GRNNo);
                this.dbServer.AddInParameter(storedProcCommand, "ItemName", DbType.String, nvo.ItemName);
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
                        nvo.List = new List<clsGRNDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsGRNDetailsVO item = new clsGRNDetailsVO {
                            Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"])),
                            TransactionUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"])),
                            ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"])),
                            BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])),
                            PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceQty"])),
                            Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"])),
                            MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"])),
                            VATPercent = (double) DALHelper.HandleDBNull(reader["VATPercent"]),
                            CDiscountPercent = (double) DALHelper.HandleDBNull(reader["CDiscountPercent"]),
                            SchDiscountPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["SchDiscountPercent"])),
                            ItemTax = (double) DALHelper.HandleDBNull(reader["ItemTax"]),
                            GRNItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Vattype"])),
                            GRNItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["VatApplicableon"])),
                            OtherGRNItemTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherTaxType"])),
                            OtherGRNItemTaxApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxApplicableon"])),
                            GRNNo = Convert.ToString(DALHelper.HandleDBNull(reader["GRNNO"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            SupplierName = Convert.ToString(DALHelper.HandleDBNull(reader["SupplierName"])),
                            GRNDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["GRNDate"]))),
                            TransUOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransUOM"])),
                            PONO = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedNumber"])),
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                            GRNID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNID"])),
                            GRNUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNUnitID"])),
                            ExpiryDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["ExpiryDate"]))),
                            SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"])),
                            BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUOMID"])),
                            GRNDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailID"])),
                            GRNDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailUnitID"])),
                            ReceivedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedID"])),
                            ReceivedUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedUnitID"])),
                            ReceivedDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedDetailID"])),
                            BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"])),
                            StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["QSStoreID"])),
                            StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["QSStoreName"])),
                            IsFreeItem = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreeItem"])),
                            SGSTPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["SGSTPercent"]))
                        };
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

        public override IValueObject GetGRNItemsForQS(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetGRNDetailsForGRNReturnListBizActionVO nvo = valueObject as clsGetGRNDetailsForGRNReturnListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetGRNItemsForQS");
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "StoreId", DbType.Int64, nvo.StoreId);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierId", DbType.Int64, nvo.SupplierId);
                this.dbServer.AddInParameter(storedProcCommand, "GRNNo", DbType.String, nvo.GRNNo);
                this.dbServer.AddInParameter(storedProcCommand, "ItemName", DbType.String, nvo.ItemName);
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
                        nvo.List = new List<clsGRNDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsGRNDetailsVO item = new clsGRNDetailsVO {
                            GRNDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["GRNDate"]))),
                            GRNNo = Convert.ToString(DALHelper.HandleDBNull(reader["GRNNO"])),
                            StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            SupplierName = Convert.ToString(DALHelper.HandleDBNull(reader["SupplierName"])),
                            Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"])) / Convert.ToDouble(DALHelper.HandleDBNull(reader["CoversionFactor"])),
                            NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"])),
                            GRNID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNID"])),
                            GRNUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNUnitID"])),
                            GRNDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailID"])),
                            GRNDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailUnitID"])),
                            StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"])),
                            BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"])),
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                            SupplierID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SupplierID"])),
                            Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"])),
                            MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"])),
                            TransactionUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"])),
                            TransUOM = Convert.ToString(DALHelper.HandleDBNull(reader["GRNQtyUOM"])),
                            SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"])),
                            SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"])),
                            ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"])),
                            BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"])),
                            BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"])),
                            BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"])),
                            VATPercent = (double) DALHelper.HandleDBNull(reader["VATPercent"])
                        };
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
                        item.ItemTax = (double) DALHelper.HandleDBNull(reader["ItemTax"]);
                        item.TaxAmount = Convert.ToSingle(DALHelper.HandleDBNull(reader["ItemTaxAmount"]));
                        item.GRNItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Vattype"]));
                        item.GRNItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["VatApplicableon"]));
                        item.OtherGRNItemTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherTaxType"]));
                        item.OtherGRNItemTaxApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxApplicableon"]));
                        item.IsFreeItem = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreeItem"]));
                        item.PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["QSPendingQty"]));
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

        public override IValueObject GetItemDetailsByID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetGRNDetailsListByIDBizActionVO nvo = valueObject as clsGetGRNDetailsListByIDBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetGRNItemsByID");
                this.dbServer.AddInParameter(storedProcCommand, "GRNID", DbType.Int64, nvo.GRNID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsGRNDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsGRNDetailsVO item = new clsGRNDetailsVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitId = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            GRNID = (long) DALHelper.HandleDBNull(reader["GRNID"]),
                            GRNDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            GRNDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            ItemID = (long) DALHelper.HandleDBNull(reader["ItemID"]),
                            BatchID = (long) DALHelper.HandleDBNull(reader["BatchID"]),
                            BatchCode = (string) DALHelper.HandleDBNull(reader["BatchCode"]),
                            ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]),
                            Quantity = ((double) DALHelper.HandleDBNull(reader["Quantity"])) / ((double) Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"]))),
                            FreeQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["FreeQuantity"])) / ((double) Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"]))),
                            SelectedUOM = { ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"])) },
                            TransUOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransUOM"])),
                            BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"])),
                            SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"])),
                            SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"])),
                            ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"])),
                            BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"])),
                            BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"])),
                            BaseQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["Quantity"])),
                            MainRate = Convert.ToSingle((float) (Convert.ToSingle(DALHelper.HandleDBNull(reader["Rate"])) / Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"])))),
                            MainMRP = Convert.ToSingle((float) (Convert.ToSingle(DALHelper.HandleDBNull(reader["MRP"])) / Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"])))),
                            AvailableQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"])),
                            AvailableStockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["AvailableStockUOM"])),
                            POQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["POQty"])),
                            PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PoPendingQty"])),
                            POPendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["ActualPendingQty"]))
                        };
                        item.PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["POPendingQuantity"]));
                        item.POPendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["POPendingQuantity"]));
                        item.POID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POID"]));
                        item.POUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POrderUnitId"]));
                        item.PODate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["POrderDate"]));
                        item.StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        item.PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PurchaseUOM"]));
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
                        item.TaxAmount = Convert.ToSingle(DALHelper.HandleDBNull(reader["ItemTaxAmount"]));
                        item.GRNItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Vattype"]));
                        item.GRNItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["VatApplicableon"]));
                        item.OtherGRNItemTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherTaxType"]));
                        item.OtherGRNItemTaxApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxApplicableon"]));
                        item.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        item.PONO = Convert.ToString(DALHelper.HandleDBNull(reader["PONO"]));
                        item.ItemGroup = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemGroup"]));
                        item.ItemCategory = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"]));
                        item.IsConsignment = (bool) DALHelper.HandleBoolDBNull(reader["IsConsignment"]);
                        item.PrevNetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PrevNetAmount"]));
                        item.OtherCharges = Convert.ToDouble(DALHelper.HandleDBNull(reader["OtherCharges"]));
                        item.GRNDiscount = Convert.ToDouble(DALHelper.HandleDBNull(reader["GRNDiscount"]));
                        item.AbatedMRP = Convert.ToDouble((double) ((Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"])) / (Convert.ToDouble(DALHelper.HandleDBNull(reader["VATPercent"])) + 100.0)) * 100.0));
                        item.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"]));
                        item.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"]));
                        item.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["Container"]));
                        item.GRNApprItemQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["GRNApprovedItemQty"]));
                        item.GRNPendItemQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["GRNPendingItemQty"]));
                        item.GRNDetailsViewTimeQty = Convert.ToSingle(DALHelper.HandleDBNull(reader["Quantity"]));
                        item.SrNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrNo"]));
                        item.SGSTPercent = (double) DALHelper.HandleDBNull(reader["SGSTPercent"]);
                        if (item.SGSTPercent <= 0.0)
                        {
                            item.SGSTAmount = (double) DALHelper.HandleDBNull(reader["SGSTAmount"]);
                        }
                        item.GRNSGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTApplicableOn"]));
                        item.GRNSGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTTaxType"]));
                        item.CGSTPercent = (double) DALHelper.HandleDBNull(reader["CGSTPercent"]);
                        if (item.CGSTPercent <= 0.0)
                        {
                            item.CGSTAmount = (double) DALHelper.HandleDBNull(reader["CGSTAmount"]);
                        }
                        item.GRNCGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTApplicableOn"]));
                        item.GRNCGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTTaxType"]));
                        item.IGSTPercent = (double) DALHelper.HandleDBNull(reader["IGSTPercent"]);
                        if (item.IGSTPercent <= 0.0)
                        {
                            item.IGSTAmount = (double) DALHelper.HandleDBNull(reader["IGSTAmount"]);
                        }
                        item.GRNIGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTApplicableOn"]));
                        item.GRNIGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTTaxType"]));
                        item.AvgCost = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvgCost"]));
                        item.AvgCostAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvgCostAmount"]));
                        item.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"]));
                        nvo.List.Add(item);
                    }
                }
                reader.NextResult();
                if (nvo.POGRNList == null)
                {
                    nvo.POGRNList = new List<clsGRNDetailsVO>();
                }
                while (true)
                {
                    if (!reader.Read())
                    {
                        reader.NextResult();
                        if (reader.HasRows)
                        {
                            if (nvo.ListFree == null)
                            {
                                nvo.ListFree = new List<clsGRNDetailsFreeVO>();
                            }
                            while (reader.Read())
                            {
                                clsGRNDetailsFreeVO evo = new clsGRNDetailsFreeVO {
                                    ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                                    UnitId = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                                    GRNID = (long) DALHelper.HandleDBNull(reader["GRNID"]),
                                    GRNDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailID"])),
                                    GRNDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailUnitID"])),
                                    FreeItemID = (long) DALHelper.HandleDBNull(reader["FreeItemID"]),
                                    FreeBatchID = (long) DALHelper.HandleDBNull(reader["FreeBatchID"]),
                                    FreeBatchCode = (string) DALHelper.HandleDBNull(reader["FreeBatchCode"]),
                                    FreeExpiryDate = DALHelper.HandleDate(reader["FreeExpiryDate"]),
                                    FreeGRNDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                    Quantity = ((double) DALHelper.HandleDBNull(reader["FreeQuantity"])) / ((double) Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"]))),
                                    SelectedUOM = { ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"])) },
                                    TransUOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransUOM"])),
                                    BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"])),
                                    SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"])),
                                    SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"])),
                                    ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"])),
                                    BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"])),
                                    BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"])),
                                    BaseQuantity = Convert.ToSingle((float) (Convert.ToSingle(DALHelper.HandleDBNull(reader["FreeQuantity"])) / Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"])))),
                                    MainRate = Convert.ToSingle((float) (Convert.ToSingle(DALHelper.HandleDBNull(reader["Rate"])) / Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"])))),
                                    MainMRP = Convert.ToSingle((float) (Convert.ToSingle(DALHelper.HandleDBNull(reader["FreeMRP"])) / Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"])))),
                                    AvailableQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"])),
                                    AvailableStockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["AvailableStockUOM"])),
                                    StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"])),
                                    PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PurchaseUOM"])),
                                    UnitMRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["FreeMRP"])),
                                    UnitRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"])),
                                    BarCode = Convert.ToString(DALHelper.HandleDBNull(reader["FreeBarCode"])),
                                    Rate = (double) DALHelper.HandleDBNull(reader["Rate"]),
                                    MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["FreeMRP"])),
                                    IsBatchRequired = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"])),
                                    VATPercent = (double) DALHelper.HandleDBNull(reader["VATPercent"])
                                };
                                if (evo.VATPercent <= 0.0)
                                {
                                    evo.VATAmount = (double) DALHelper.HandleDBNull(reader["VATAmount"]);
                                }
                                evo.FreeItemName = (string) DALHelper.HandleDBNull(reader["ItemName"]);
                                evo.ItemTax = (double) DALHelper.HandleDBNull(reader["ItemTax"]);
                                evo.TaxAmount = Convert.ToSingle(DALHelper.HandleDBNull(reader["ItemTaxAmount"]));
                                evo.GRNItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Vattype"]));
                                evo.GRNItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["VatApplicableon"]));
                                evo.OtherGRNItemTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherTaxType"]));
                                evo.OtherGRNItemTaxApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxApplicableon"]));
                                evo.FreeItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                                evo.ItemGroup = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemGroup"]));
                                evo.ItemCategory = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"]));
                                evo.PrevNetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PrevNetAmount"]));
                                evo.OtherCharges = Convert.ToDouble(DALHelper.HandleDBNull(reader["OtherCharges"]));
                                evo.GRNDiscount = Convert.ToDouble(DALHelper.HandleDBNull(reader["GRNDiscount"]));
                                evo.AbatedMRP = Convert.ToDouble((double) ((Convert.ToDouble(DALHelper.HandleDBNull(reader["FreeMRP"])) / (Convert.ToDouble(DALHelper.HandleDBNull(reader["VATPercent"])) + 100.0)) * 100.0));
                                evo.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"]));
                                evo.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"]));
                                evo.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["Container"]));
                                evo.GRNDetailsViewTimeQty = Convert.ToSingle(DALHelper.HandleDBNull(reader["FreeQuantity"]));
                                evo.SGSTPercent = (double) DALHelper.HandleDBNull(reader["SGSTPercent"]);
                                if (evo.SGSTPercent <= 0.0)
                                {
                                    evo.SGSTAmount = (double) DALHelper.HandleDBNull(reader["SGSTAmount"]);
                                }
                                evo.GRNSGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTApplicableOn"]));
                                evo.GRNSGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTTaxType"]));
                                evo.CGSTPercent = (double) DALHelper.HandleDBNull(reader["CGSTPercent"]);
                                if (evo.CGSTPercent <= 0.0)
                                {
                                    evo.CGSTAmount = (double) DALHelper.HandleDBNull(reader["CGSTAmount"]);
                                }
                                evo.GRNCGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTApplicableOn"]));
                                evo.GRNCGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTTaxType"]));
                                evo.IGSTPercent = (double) DALHelper.HandleDBNull(reader["IGSTPercent"]);
                                if (evo.IGSTPercent <= 0.0)
                                {
                                    evo.IGSTAmount = (double) DALHelper.HandleDBNull(reader["IGSTAmount"]);
                                }
                                evo.GRNIGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTApplicableOn"]));
                                evo.GRNIGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTTaxType"]));
                                evo.MainItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainItemID"]));
                                evo.MainItemName = Convert.ToString(DALHelper.HandleDBNull(reader["MainItemName"]));
                                evo.MainItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["MainItemCode"]));
                                evo.MainBatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainBatchID"]));
                                evo.MainBatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["MainBatchCode"]));
                                evo.MainExpiryDate = DALHelper.HandleDate(reader["MainExpiryDate"]);
                                evo.MainItemMRP = Convert.ToSingle(DALHelper.HandleDBNull(reader["MainItemMRP"]));
                                evo.MainItemCostRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["MainItemCostRate"]));
                                evo.MainSrNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainSrNo"]));
                                nvo.ListFree.Add(evo);
                            }
                        }
                        reader.Close();
                        break;
                    }
                    clsGRNDetailsVO item = new clsGRNDetailsVO {
                        ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                        UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                        GRNID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNID"])),
                        GRNUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNUnitID"])),
                        GRNDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailID"])),
                        GRNDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailUnitID"])),
                        POID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POID"])),
                        POUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POUnitID"])),
                        PODetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PODetailID"])),
                        PoItemsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PODetailID"])),
                        PODetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PODetailUnitID"])),
                        ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"])),
                        Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"])),
                        PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQty"])),
                        POPendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQty"])),
                        POFinalPendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQty"]))
                    };
                    nvo.POGRNList.Add(item);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetItemDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetGRNDetailsListBizActionVO nvo = valueObject as clsGetGRNDetailsListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetGRNItemsList");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "GRNID", DbType.Int64, nvo.GRNID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Freezed", DbType.Int64, nvo.Freezed);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsGRNDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsGRNDetailsVO item = new clsGRNDetailsVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitId = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            GRNID = (long) DALHelper.HandleDBNull(reader["GRNID"]),
                            GRNDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            GRNDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            ItemID = (long) DALHelper.HandleDBNull(reader["ItemID"]),
                            BatchID = (long) DALHelper.HandleDBNull(reader["BatchID"]),
                            BatchCode = (string) DALHelper.HandleDBNull(reader["BatchCode"]),
                            ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]),
                            ConversionFactor = Convert.ToDouble(DALHelper.HandleDBNull(reader["conversionFactor"]))
                        };
                        item.Quantity = ((double) DALHelper.HandleDBNull(reader["Quantity"])) / item.ConversionFactor;
                        item.FreeQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["FreeQuantity"])) / item.ConversionFactor;
                        item.AvailableQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        item.POPendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        item.POQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["POIQuantity"]));
                        item.PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        item.POID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POID"]));
                        item.POUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POrderUnitId"]));
                        item.PODate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["POrderDate"]));
                        item.StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        item.PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PurchaseUOM"]));
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
                        item.TaxAmount = Convert.ToSingle(DALHelper.HandleDBNull(reader["ItemTaxAmount"]));
                        item.GRNItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Vattype"]));
                        item.GRNItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["VatApplicableon"]));
                        item.OtherGRNItemTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherTaxType"]));
                        item.OtherGRNItemTaxApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxApplicableon"]));
                        item.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        item.PONO = Convert.ToString(DALHelper.HandleDBNull(reader["PONO"]));
                        item.ItemGroup = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemGroup"]));
                        item.ItemCategory = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"]));
                        item.IsConsignment = (bool) DALHelper.HandleBoolDBNull(reader["IsConsignment"]);
                        if (Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"])) > 0f)
                        {
                            item.Quantity = Convert.ToDouble((double) (Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"])) / ((double) Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"])))));
                            item.BaseQuantity = Convert.ToSingle((float) (Convert.ToSingle(DALHelper.HandleDBNull(reader["Quantity"])) / Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"]))));
                        }
                        else
                        {
                            item.Quantity = (double) DALHelper.HandleDBNull(reader["Quantity"]);
                            item.BaseQuantity = Convert.ToSingle(Convert.ToSingle(DALHelper.HandleDBNull(reader["Quantity"])));
                        }
                        item.SelectedUOM.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                        item.TransUOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransUOM"]));
                        item.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                        item.SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));
                        item.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        item.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"]));
                        item.BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"]));
                        item.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"]));
                        item.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"]));
                        item.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"]));
                        item.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["Container"]));
                        item.FPNetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                        item.AvgCostForFront = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvgCost"]));
                        item.AvgCostAmountForFront = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvgCostAmount"]));
                        nvo.List.Add(item);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    if (nvo.FreeItemsList == null)
                    {
                        nvo.FreeItemsList = new List<clsGRNDetailsFreeVO>();
                    }
                    while (reader.Read())
                    {
                        clsGRNDetailsFreeVO item = new clsGRNDetailsFreeVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitId = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            GRNID = (long) DALHelper.HandleDBNull(reader["GRNID"]),
                            GRNDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            GRNDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            FreeItemID = (long) DALHelper.HandleDBNull(reader["FreeItemID"]),
                            FreeBatchID = (long) DALHelper.HandleDBNull(reader["FreeBatchID"]),
                            FreeBatchCode = (string) DALHelper.HandleDBNull(reader["FreeBatchCode"]),
                            FreeExpiryDate = DALHelper.HandleDate(reader["FreeExpiryDate"]),
                            ConversionFactor = Convert.ToDouble(DALHelper.HandleDBNull(reader["conversionFactor"]))
                        };
                        item.Quantity = ((double) DALHelper.HandleDBNull(reader["FreeQuantity"])) / item.ConversionFactor;
                        item.FreeQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["FreeQuantity"])) / item.ConversionFactor;
                        item.AvailableQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        item.StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        item.PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PurchaseUOM"]));
                        item.UnitMRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["FreeMRP"]));
                        item.UnitRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                        item.BarCode = Convert.ToString(DALHelper.HandleDBNull(reader["BarCode"]));
                        item.Rate = (double) DALHelper.HandleDBNull(reader["Rate"]);
                        item.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["FreeMRP"]));
                        item.IsBatchRequired = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"]));
                        item.VATPercent = (double) DALHelper.HandleDBNull(reader["VATPercent"]);
                        if (item.VATPercent <= 0.0)
                        {
                            item.VATAmount = (double) DALHelper.HandleDBNull(reader["VATAmount"]);
                        }
                        item.FreeItemName = (string) DALHelper.HandleDBNull(reader["ItemName"]);
                        item.ItemTax = (double) DALHelper.HandleDBNull(reader["ItemTax"]);
                        item.TaxAmount = Convert.ToSingle(DALHelper.HandleDBNull(reader["ItemTaxAmount"]));
                        item.GRNItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Vattype"]));
                        item.GRNItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["VatApplicableon"]));
                        item.OtherGRNItemTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherTaxType"]));
                        item.OtherGRNItemTaxApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxApplicableon"]));
                        item.FreeItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        item.ItemGroup = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemGroup"]));
                        item.ItemCategory = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"]));
                        if (Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"])) > 0f)
                        {
                            item.Quantity = Convert.ToDouble((double) (Convert.ToDouble(DALHelper.HandleDBNull(reader["FreeQuantity"])) / ((double) Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"])))));
                            item.BaseQuantity = Convert.ToSingle((float) (Convert.ToSingle(DALHelper.HandleDBNull(reader["FreeQuantity"])) / Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"]))));
                        }
                        else
                        {
                            item.Quantity = (double) DALHelper.HandleDBNull(reader["FreeQuantity"]);
                            item.BaseQuantity = Convert.ToSingle(Convert.ToSingle(DALHelper.HandleDBNull(reader["FreeQuantity"])));
                        }
                        item.SelectedUOM.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                        item.TransUOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransUOM"]));
                        item.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                        item.SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));
                        item.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        item.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"]));
                        item.BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"]));
                        item.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"]));
                        item.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"]));
                        item.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"]));
                        item.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["Container"]));
                        item.FPNetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                        nvo.FreeItemsList.Add(item);
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
            clsGetGRNListBizActionVO nvo = valueObject as clsGetGRNListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetGRNListForSearch");
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
                        nvo.List = new List<clsGRNVO>();
                    }
                    while (reader.Read())
                    {
                        clsGRNVO item = new clsGRNVO {
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
                        item.POID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POID"]));
                        item.InvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader["InvoiceNo"]));
                        item.InvoiceDate = DALHelper.HandleDate(reader["InvoiceDate"]);
                        item.PONowithDate = (string) DALHelper.HandleDBNull(reader["PO No - Date"]);
                        item.IndentNowithDate = (string) DALHelper.HandleDBNull(reader["Indent No. - Date"]);
                        item.FileName = (string) DALHelper.HandleDBNull(reader["FileName"]);
                        item.File = (byte[]) DALHelper.HandleDBNull(reader["FileData"]);
                        item.IsFileAttached = (bool) DALHelper.HandleDBNull(reader["IsFileAttached"]);
                        item.FileAttached = item.IsFileAttached ? "Visible" : "Collapsed";
                        item.IsConsignment = (bool) DALHelper.HandleBoolDBNull(reader["IsConsignment"]);
                        item.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                        item.Approve = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Approved"]));
                        item.Reject = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Rejected"]));
                        item.ApprovedOrRejectedBy = Convert.ToString(DALHelper.HandleDBNull(reader["ApprovedOrRejectedBy"]));
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

        private clsUpdateGRNForRejectionVO RejectGRN(clsUpdateGRNForRejectionVO BizActionObj, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
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
                clsGRNVO details = BizActionObj.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdaterGRNRejectedStatus");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "ApprovedOrRejectedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, details.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, details.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ApprovedOrRejectedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
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

        private void SetLogInfo(List<LogInfo> objLogList, long userID)
        {
            try
            {
                if ((objLogList != null) && (objLogList.Count > 0))
                {
                    foreach (LogInfo info in objLogList)
                    {
                        this.logManager.LogInfo(info.guid, userID, info.TimeStamp, info.ClassName, info.MethodName, info.Message);
                    }
                }
            }
            catch (Exception exception)
            {
                this.logManager.LogError(userID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
            }
        }

        public override IValueObject UpdateGRNForApproval(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateGRNForApprovalVO bizActionObj = valueObject as clsUpdateGRNForApprovalVO;
            if (bizActionObj.Details.ID > 0L)
            {
                bizActionObj = this.ApproveGRN(bizActionObj, UserVo, null, null);
            }
            return valueObject;
        }

        public override IValueObject UpdateGRNForApproval(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {
            clsUpdateGRNForApprovalVO bizActionObj = valueObject as clsUpdateGRNForApprovalVO;
            if (bizActionObj.Details.ID > 0L)
            {
                bizActionObj = this.ApproveGRN(bizActionObj, UserVo, myConnection, myTransaction);
            }
            return valueObject;
        }

        public override IValueObject UpdateGRNForBarcode(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            clsAddGRNBizActionVO nvo = valueObject as clsAddGRNBizActionVO;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                clsGRNDetailsVO gRNItemDetails = nvo.GRNItemDetails;
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

        public override IValueObject UpdateGRNForRejection(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateGRNForRejectionVO bizActionObj = valueObject as clsUpdateGRNForRejectionVO;
            if (bizActionObj.Details.ID > 0L)
            {
                bizActionObj = this.RejectGRN(bizActionObj, UserVo, null, null);
            }
            return valueObject;
        }

        public override IValueObject UpdateGRNForRejection(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {
            clsUpdateGRNForRejectionVO bizActionObj = valueObject as clsUpdateGRNForRejectionVO;
            if (bizActionObj.Details.ID > 0L)
            {
                bizActionObj = this.RejectGRN(bizActionObj, UserVo, myConnection, myTransaction);
            }
            return valueObject;
        }
    }
}

