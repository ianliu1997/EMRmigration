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
    using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Reflection;

    public class clsItemSalesDAL : clsBaseItemSalesDAL
    {
        private Database dbServer;
        private LogManager logManager;
        private bool IsAuditTrail;

        private clsItemSalesDAL()
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
            clsAddItemSalesBizActionVO bizActionObj = valueObject as clsAddItemSalesBizActionVO;
            bizActionObj = this.AddDetails(bizActionObj, UserVo, null, null);
            return valueObject;
        }

        public override IValueObject Add(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            clsAddItemSalesBizActionVO bizActionObj = valueObject as clsAddItemSalesBizActionVO;
            bizActionObj = this.AddDetails(bizActionObj, UserVo, pConnection, pTransaction);
            return valueObject;
        }

        private clsAddItemSalesBizActionVO AddDetails(clsAddItemSalesBizActionVO BizActionObj, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            DbConnection myConnection = null;
            DbTransaction transaction = null;
            try
            {
                myConnection = (pConnection == null) ? this.dbServer.CreateConnection() : pConnection;
                if (myConnection.State == ConnectionState.Closed)
                {
                    myConnection.Open();
                }
                transaction = (pTransaction == null) ? myConnection.BeginTransaction() : pTransaction;
                clsItemSalesVO details = BizActionObj.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddItemSales");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, details.LinkServer);
                if (details.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, details.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, details.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, details.Date);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, details.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, details.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemSaleNo", DbType.String, details.ItemSaleNo);
                this.dbServer.AddInParameter(storedProcCommand, "ConcessionPercentage", DbType.Double, details.ConcessionPercentage);
                this.dbServer.AddInParameter(storedProcCommand, "ConcessionAmount", DbType.Double, details.ConcessionAmount);
                this.dbServer.AddInParameter(storedProcCommand, "VATPercentage", DbType.Double, details.VATPercentage);
                this.dbServer.AddInParameter(storedProcCommand, "VATAmount", DbType.Double, details.VATAmount);
                this.dbServer.AddInParameter(storedProcCommand, "TotalAmount", DbType.Double, details.TotalAmount);
                this.dbServer.AddInParameter(storedProcCommand, "NetAmount", DbType.Double, details.NetAmount);
                this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, details.Remarks);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, details.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.Int64, details.BillID);
                this.dbServer.AddInParameter(storedProcCommand, "IsBilled", DbType.Boolean, details.IsBilled);
                this.dbServer.AddInParameter(storedProcCommand, "ReasonForVariance", DbType.String, details.ReasonForVariance);
                this.dbServer.AddInParameter(storedProcCommand, "ReferenceDoctorID", DbType.Int64, details.ReferenceDoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "ReferenceDoctor", DbType.String, details.ReferenceDoctor);
                this.dbServer.AddInParameter(storedProcCommand, "AdmissionID", DbType.Int64, details.Opd_Ipd_External_Id);
                this.dbServer.AddInParameter(storedProcCommand, "AdmissionUnitID", DbType.Int64, details.Opd_Ipd_External_UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, details.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CostingDivisionID", DbType.Int64, details.CostingDivisionID);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, details.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, details.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                foreach (clsItemSalesDetailsVO svo2 in details.Items)
                {
                    svo2.ItemSaleId = BizActionObj.Details.ID;
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddItemSalesDetails");
                    this.dbServer.AddInParameter(command2, "LinkServer", DbType.String, details.LinkServer);
                    if (details.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(command2, "LinkServerAlias", DbType.String, details.LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddInParameter(command2, "ItemSaleId", DbType.Int64, svo2.ItemSaleId);
                    this.dbServer.AddInParameter(command2, "ItemID", DbType.Int64, svo2.ItemID);
                    this.dbServer.AddInParameter(command2, "BatchId", DbType.String, svo2.BatchID);
                    this.dbServer.AddInParameter(command2, "Quantity", DbType.Double, svo2.Quantity);
                    this.dbServer.AddInParameter(command2, "MRP", DbType.Double, svo2.MRP);
                    this.dbServer.AddInParameter(command2, "ConcessionPercentage", DbType.Double, svo2.ConcessionPercentage);
                    this.dbServer.AddInParameter(command2, "ConcessionAmount", DbType.Double, svo2.ConcessionAmount);
                    this.dbServer.AddInParameter(command2, "VatPercentage", DbType.Double, svo2.VATPercent);
                    this.dbServer.AddInParameter(command2, "VATAmount", DbType.Double, svo2.VATAmount);
                    this.dbServer.AddInParameter(command2, "TotalAmount", DbType.Double, svo2.Amount);
                    this.dbServer.AddInParameter(command2, "NetAmount", DbType.Double, svo2.NetAmount);
                    this.dbServer.AddInParameter(command2, "PendingQuantity", DbType.Double, svo2.BaseQuantity);
                    this.dbServer.AddInParameter(command2, "StoreID", DbType.Int64, details.StoreID);
                    this.dbServer.AddInParameter(command2, "PrescriptionDetailsID", DbType.Int64, svo2.PrescriptionDetailsID);
                    this.dbServer.AddInParameter(command2, "PrescriptionDetailsUnitID", DbType.Int64, svo2.PrescriptionDetailsUnitID);
                    this.dbServer.AddInParameter(command2, "PackageID", DbType.Int64, svo2.PackageID);
                    this.dbServer.AddInParameter(command2, "BaseQuantity", DbType.Single, svo2.BaseQuantity);
                    this.dbServer.AddInParameter(command2, "BaseCF", DbType.Single, svo2.BaseConversionFactor);
                    this.dbServer.AddInParameter(command2, "TransactionUOMID", DbType.Int64, svo2.SelectedUOM.ID);
                    this.dbServer.AddInParameter(command2, "BaseUMID", DbType.Int64, svo2.BaseUOMID);
                    this.dbServer.AddInParameter(command2, "StockUOMID", DbType.Int64, svo2.SUOMID);
                    this.dbServer.AddInParameter(command2, "StockCF", DbType.Single, svo2.ConversionFactor);
                    this.dbServer.AddInParameter(command2, "StockingQuantity", DbType.Single, svo2.Quantity * svo2.ConversionFactor);
                    this.dbServer.AddInParameter(command2, "ActualNetAmt", DbType.Double, svo2.ActualNetAmt);
                    this.dbServer.AddInParameter(command2, "NetAmtCalculation", DbType.Double, svo2.NetAmtCalculation);
                    this.dbServer.AddInParameter(command2, "ItemVatType", DbType.Int32, svo2.ItemVatType);
                    this.dbServer.AddInParameter(command2, "ISForMaterialConsumption", DbType.String, details.ISForMaterialConsumption);
                    this.dbServer.AddInParameter(command2, "PackageBillID", DbType.Int64, svo2.PackageBillID);
                    this.dbServer.AddInParameter(command2, "PackageBillUnitID", DbType.Int64, svo2.PackageBillUnitID);
                    this.dbServer.AddInParameter(command2, "PackageConcessionPercentage", DbType.Double, svo2.PackageConcessionPercentage);
                    this.dbServer.AddInParameter(command2, "PackageConcessionAmount", DbType.Double, svo2.PackageConcessionAmount);
                    this.dbServer.AddInParameter(command2, "SGSTPercentage", DbType.Double, svo2.SGSTPercent);
                    this.dbServer.AddInParameter(command2, "SGSTAmount", DbType.Double, svo2.SGSTAmount);
                    this.dbServer.AddInParameter(command2, "CGSTPercentage", DbType.Double, svo2.CGSTPercent);
                    this.dbServer.AddInParameter(command2, "CGSTAmount", DbType.Double, svo2.CGSTAmount);
                    this.dbServer.AddInParameter(command2, "IGSTPercentage", DbType.Double, svo2.IGSTPercent);
                    this.dbServer.AddInParameter(command2, "IGSTAmount", DbType.Double, svo2.IGSTAmount);
                    this.dbServer.AddInParameter(command2, "SGSTTaxType", DbType.Int32, svo2.SGSTtaxtype);
                    this.dbServer.AddInParameter(command2, "SGSTApplicableOn", DbType.Int32, svo2.SGSTapplicableon);
                    this.dbServer.AddInParameter(command2, "CGSTTaxType", DbType.Int32, svo2.CGSTtaxtype);
                    this.dbServer.AddInParameter(command2, "CGSTApplicableOn", DbType.Int32, svo2.CGSTapplicableon);
                    this.dbServer.AddInParameter(command2, "IGSTTaxType", DbType.Int32, svo2.IGSTtaxtype);
                    this.dbServer.AddInParameter(command2, "IGSTApplicableOn", DbType.Int32, svo2.IGSTapplicableon);
                    this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, details.Status);
                    this.dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, details.AddedDateTime);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo2.ID);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                    svo2.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                    if (!details.ISForMaterialConsumption && details.IsBilled)
                    {
                        clsBaseItemStockDAL instance = clsBaseItemStockDAL.GetInstance();
                        clsAddItemStockBizActionVO valueObject = new clsAddItemStockBizActionVO();
                        clsItemStockVO kvo = new clsItemStockVO {
                            ItemID = svo2.ItemID,
                            BatchID = svo2.BatchID,
                            TransactionTypeID = InventoryTransactionType.ItemsSale,
                            TransactionID = svo2.ItemSaleId,
                            Date = BizActionObj.Details.Date,
                            Time = BizActionObj.Details.Time,
                            OperationType = InventoryStockOperationType.Subtraction,
                            StoreID = new long?(BizActionObj.Details.StoreID),
                            InputTransactionQuantity = Convert.ToSingle(svo2.Quantity),
                            BaseUOMID = svo2.BaseUOMID,
                            BaseConversionFactor = svo2.BaseConversionFactor,
                            TransactionQuantity = svo2.BaseQuantity,
                            SUOMID = svo2.SUOMID,
                            ConversionFactor = svo2.ConversionFactor,
                            StockingQuantity = svo2.Quantity * svo2.ConversionFactor,
                            SelectedUOM = { ID = svo2.SelectedUOM.ID }
                        };
                        valueObject.Details = kvo;
                        valueObject = (clsAddItemStockBizActionVO) instance.Add(valueObject, UserVo, myConnection, transaction);
                        if (valueObject.SuccessStatus == -1)
                        {
                            throw new Exception();
                        }
                        kvo.ID = valueObject.Details.ID;
                    }
                }
                if (pConnection == null)
                {
                    transaction.Commit();
                }
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                if (pConnection == null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (pConnection == null)
                {
                    myConnection.Close();
                }
            }
            return BizActionObj;
        }

        public override IValueObject AddItemSaleReturn(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddItemSalesReturnBizActionVO nvo = valueObject as clsAddItemSalesReturnBizActionVO;
            DbConnection myConnection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            try
            {
                myConnection.Open();
                transaction = myConnection.BeginTransaction();
                clsItemSalesReturnVO itemMatserDetail = nvo.ItemMatserDetail;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddtemSaleReturn");
                storedProcCommand.Connection = myConnection;
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, itemMatserDetail.LinkServer);
                if (itemMatserDetail.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, itemMatserDetail.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CostingDivisionID", DbType.Int64, itemMatserDetail.CostingDivisionID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, itemMatserDetail.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, itemMatserDetail.Time);
                this.dbServer.AddParameter(storedProcCommand, "SalesReturnNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");
                this.dbServer.AddInParameter(storedProcCommand, "ItemSaleId", DbType.Int64, itemMatserDetail.ItemSalesID);
                this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, itemMatserDetail.Remarks);
                this.dbServer.AddInParameter(storedProcCommand, "ConcessionPercentage", DbType.Double, itemMatserDetail.ConcessionPercentage);
                this.dbServer.AddInParameter(storedProcCommand, "ConcessionAmount", DbType.Double, itemMatserDetail.ConcessionAmount);
                this.dbServer.AddInParameter(storedProcCommand, "VATPercentage", DbType.Double, itemMatserDetail.VATPercentage);
                this.dbServer.AddInParameter(storedProcCommand, "VATAmount", DbType.Double, itemMatserDetail.VATAmount);
                this.dbServer.AddInParameter(storedProcCommand, "TotalSGST", DbType.Double, itemMatserDetail.TotalSGST);
                this.dbServer.AddInParameter(storedProcCommand, "TotalCGST", DbType.Double, itemMatserDetail.TotalCGST);
                this.dbServer.AddInParameter(storedProcCommand, "TotalNetAmount", DbType.Double, itemMatserDetail.TotalAmount);
                this.dbServer.AddInParameter(storedProcCommand, "NetAmount", DbType.Double, itemMatserDetail.NetAmount);
                this.dbServer.AddInParameter(storedProcCommand, "CalculatedNetAmount", DbType.Double, itemMatserDetail.CalculatedNetAmount);
                this.dbServer.AddInParameter(storedProcCommand, "TotalReturnAmount", DbType.Double, itemMatserDetail.TotalReturnAmount);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, itemMatserDetail.Status);
                this.dbServer.AddInParameter(storedProcCommand, "AddUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "By", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "On", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "DateTime", DbType.DateTime, itemMatserDetail.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "WindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.ItemSalesReturnID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.ItemMatserDetail.ItemSaleReturnNo = (string) this.dbServer.GetParameterValue(storedProcCommand, "SalesReturnNo");
                foreach (clsItemSalesReturnDetailsVO svo in nvo.ItemsDetail)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddtemSaleReturnDetails");
                    command2.Connection = myConnection;
                    this.dbServer.AddInParameter(command2, "LinkServer", DbType.String, itemMatserDetail.LinkServer);
                    if (itemMatserDetail.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(command2, "LinkServerAlias", DbType.String, itemMatserDetail.LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddInParameter(command2, "ItemSaleReturnId", DbType.Int64, nvo.ItemSalesReturnID);
                    this.dbServer.AddInParameter(command2, "ItemID", DbType.Int64, svo.ItemID);
                    this.dbServer.AddInParameter(command2, "BatchId", DbType.String, svo.BatchID);
                    this.dbServer.AddInParameter(command2, "ReturnedQuantity", DbType.Double, svo.ReturnQuantity);
                    this.dbServer.AddInParameter(command2, "Rate", DbType.Double, svo.MRP);
                    this.dbServer.AddInParameter(command2, "ConcessionPercentage", DbType.Double, svo.ConcessionPercentage);
                    this.dbServer.AddInParameter(command2, "ConcessionAmount", DbType.Double, svo.ConcessionAmount);
                    this.dbServer.AddInParameter(command2, "VatPercentage", DbType.Double, svo.VATPercent);
                    this.dbServer.AddInParameter(command2, "VATAmount", DbType.Double, svo.VATAmount);
                    this.dbServer.AddInParameter(command2, "SGSTPercent", DbType.Double, svo.SGSTPercent);
                    this.dbServer.AddInParameter(command2, "SGSTAmount", DbType.Double, svo.SGSTAmount);
                    this.dbServer.AddInParameter(command2, "CGSTPercent", DbType.Double, svo.CGSTPercent);
                    this.dbServer.AddInParameter(command2, "CGSTAmount", DbType.Double, svo.CGSTAmount);
                    this.dbServer.AddInParameter(command2, "TotalAmount", DbType.Double, svo.Amount);
                    this.dbServer.AddInParameter(command2, "NetAmount", DbType.Double, svo.NetAmount);
                    this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, itemMatserDetail.Status);
                    this.dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "By", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "On", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "DateTime", DbType.DateTime, itemMatserDetail.AddedDateTime);
                    this.dbServer.AddInParameter(command2, "WindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command2, "PendingQuantity", DbType.Double, svo.BaseQuantity);
                    this.dbServer.AddInParameter(command2, "ItemVatType", DbType.Int32, svo.ItemVatType);
                    this.dbServer.AddInParameter(command2, "BaseQuantity", DbType.Single, svo.BaseQuantity);
                    this.dbServer.AddInParameter(command2, "BaseCF", DbType.Single, svo.BaseConversionFactor);
                    this.dbServer.AddInParameter(command2, "TransactionUOMID", DbType.Int64, svo.SelectedUOM.ID);
                    this.dbServer.AddInParameter(command2, "BaseUMID", DbType.Int64, svo.BaseUOMID);
                    this.dbServer.AddInParameter(command2, "StockUOMID", DbType.Int64, svo.SUOMID);
                    this.dbServer.AddInParameter(command2, "StockCF", DbType.Single, svo.ConversionFactor);
                    this.dbServer.AddInParameter(command2, "StockingQuantity", DbType.Single, svo.ReturnQuantity * svo.ConversionFactor);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddOutParameter(command2, "ID", DbType.Int64, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_UpdateItemSale");
                    command3.Connection = myConnection;
                    this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "PendingQuantity", DbType.Decimal, svo.BaseQuantity);
                    this.dbServer.AddInParameter(command3, "SaleDetailsID", DbType.Int64, svo.ID);
                    this.dbServer.ExecuteNonQuery(command3, transaction);
                    clsBaseItemStockDAL instance = clsBaseItemStockDAL.GetInstance();
                    clsAddItemStockBizActionVO nvo3 = new clsAddItemStockBizActionVO();
                    clsItemStockVO kvo = new clsItemStockVO {
                        ItemID = svo.ItemID,
                        BatchID = svo.BatchID,
                        TransactionTypeID = InventoryTransactionType.ItemSaleReturn,
                        TransactionID = nvo.ItemSalesReturnID,
                        Date = nvo.ItemMatserDetail.Date,
                        Time = nvo.ItemMatserDetail.Time,
                        OperationType = InventoryStockOperationType.Addition,
                        StoreID = new long?(nvo.ItemMatserDetail.StoreID),
                        InputTransactionQuantity = Convert.ToSingle(svo.ReturnQuantity),
                        BaseUOMID = svo.BaseUOMID,
                        BaseConversionFactor = svo.BaseConversionFactor,
                        TransactionQuantity = svo.BaseQuantity,
                        SUOMID = svo.SUOMID,
                        ConversionFactor = svo.ConversionFactor,
                        StockingQuantity = svo.ReturnQuantity * svo.ConversionFactor,
                        SelectedUOM = { ID = svo.SelectedUOM.ID }
                    };
                    nvo3.Details = kvo;
                    nvo3 = (clsAddItemStockBizActionVO) instance.Add(nvo3, UserVo, myConnection, transaction);
                    if (nvo3.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    kvo.ID = nvo3.Details.ID;
                }
                if (nvo.ItemMatserDetail.RefundDetails != null)
                {
                    clsBaseRefundDAL instance = clsBaseRefundDAL.GetInstance();
                    clsAddRefundBizActionVO nvo4 = new clsAddRefundBizActionVO {
                        Details = new clsRefundVO()
                    };
                    nvo4.Details = nvo.ItemMatserDetail.RefundDetails;
                    nvo4.Details.ItemSaleReturnID = nvo.ItemSalesReturnID;
                    nvo4.Details.PaymentDetails.CostingDivisionID = nvo.ItemMatserDetail.CostingDivisionID;
                    if (nvo.IsRefundToAdvance)
                    {
                        nvo4.IsRefundToAdvance = nvo.IsRefundToAdvance;
                        nvo4.RefundToAdvancePatientID = nvo.RefundToAdvancePatientID;
                        nvo4.RefundToAdvancePatientUnitID = nvo.RefundToAdvancePatientUnitID;
                        nvo4.IsExchangeMaterial = nvo.IsExchangeMaterial;
                    }
                    nvo4 = (clsAddRefundBizActionVO) instance.Add(nvo4, UserVo, myConnection, transaction);
                    if (nvo4.SuccessStatus == -5)
                    {
                        throw new Exception();
                    }
                    nvo.ItemMatserDetail.RefundDetails.ID = nvo4.Details.ID;
                }
                transaction.Commit();
                if (this.IsAuditTrail && (nvo.LogInfoList != null))
                {
                    if (nvo.ItemsDetail.Count > 0)
                    {
                        LogInfo item = new LogInfo();
                        Guid guid = default(Guid);
                        item.guid = guid;
                        item.UserId = UserVo.ID;
                        item.TimeStamp = DateTime.Now;
                        item.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                        item.MethodName = MethodBase.GetCurrentMethod().ToString();
                        string[] strArray = new string[] { " 107 : T_ItemSaleReturnDetails To Get  Item Sale Return Id Unit Id : ", Convert.ToString(UserVo.UserLoginInfo.UnitId), "  ,Item Sales Return ID:", Convert.ToString(nvo.ItemSalesReturnID), " , Item Sale Return No : ", Convert.ToString(nvo.ItemMatserDetail.ItemSaleReturnNo) };
                        item.Message = string.Concat(strArray);
                        nvo.LogInfoList.Add(item);
                    }
                    if ((nvo.LogInfoList.Count > 0) && this.IsAuditTrail)
                    {
                        this.SetLogInfo(nvo.LogInfoList, UserVo.ID);
                        nvo.LogInfoList.Clear();
                    }
                }
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                nvo.ItemMatserDetail = null;
                nvo.ItemsDetail = null;
                nvo.SuccessStatus = -1;
            }
            finally
            {
                myConnection.Close();
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject GetItemSale(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetItemSalesBizActionVO nvo = valueObject as clsGetItemSalesBizActionVO;
            nvo.Details = new List<clsItemSalesVO>();
            clsItemSalesVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemSales");
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CostingDivisionID", DbType.Int64, nvo.CostingDivisionID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SerachExpression);
                this.dbServer.AddInParameter(storedProcCommand, "billFreeze", DbType.Boolean, nvo.isBillFreezed);
                if ((nvo.FirstName != null) && (nvo.FirstName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, nvo.FirstName + "%");
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, null);
                }
                if ((nvo.LastName != null) && (nvo.LastName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, nvo.LastName + "%");
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, null);
                }
                this.dbServer.AddInParameter(storedProcCommand, "BillNo", DbType.String, "%" + nvo.BillNo + "%");
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsItemSalesVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            CostingDivisionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CostingDivisionID"])),
                            Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"])),
                            VisitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitID"])),
                            ItemSaleNo = Convert.ToString(DALHelper.HandleDBNull(reader["ItemSaleNo"])),
                            Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"])),
                            ConcessionPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionPercentage"])),
                            ConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"])),
                            TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"])),
                            VATPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["VATPercentage"])),
                            VATAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["VATAmount"])),
                            NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"])),
                            BalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalAmt"])),
                            OPDNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["OPDNO"])),
                            StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillID"])),
                            LastName = (string) DALHelper.HandleDBNull(reader["LastName"]),
                            FirstName = (string) DALHelper.HandleDBNull(reader["FirstName"]),
                            MiddleName = (string) DALHelper.HandleDBNull(reader["MiddleName"]),
                            BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]))
                        };
                        nvo.Details.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
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

        public override IValueObject GetItemSaleComplete(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsGetItemSalesCompleteBizActionVO nvo = valueObject as clsGetItemSalesCompleteBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_PharmacyChargeList");
                this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.Int64, nvo.BillID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.BillUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsBilled", DbType.Boolean, nvo.IsBilled);
                this.dbServer.AddInParameter(storedProcCommand, "AdmID", DbType.Int64, nvo.AdmID);
                this.dbServer.AddInParameter(storedProcCommand, "AdmissionUnitID", DbType.Int64, nvo.AdmissionUnitID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    nvo.Details = new clsItemSalesVO();
                    while (reader.Read())
                    {
                        nvo.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.Details.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.Details.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        nvo.Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        nvo.Details.VisitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitID"]));
                        nvo.Details.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
                        nvo.Details.IsBilled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBilled"]));
                        clsItemSalesDetailsVO item = new clsItemSalesDetailsVO {
                            IsBilled = nvo.Details.IsBilled,
                            ItemSaleId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemSaleId"])),
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            BatchID = Convert.ToInt32(DALHelper.HandleDBNull(reader["BatchId"])),
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"])),
                            MRPBill = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"])),
                            AmountBill = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"])),
                            NetAmountBill = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"])),
                            NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"])),
                            ConcessionPercentageBill = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionPercentage"])),
                            ConcessionAmountBill = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"])),
                            VATPercentBill = Convert.ToDouble(DALHelper.HandleDBNull(reader["VATPercentage"])),
                            VATAmountBill = Convert.ToDouble(DALHelper.HandleDBNull(reader["VATAmount"])),
                            ExpiryDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["ExpiryDate"]))),
                            Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]))),
                            SGSTPercentBill = Convert.ToDouble(DALHelper.HandleDBNull(reader["SGSTPercentage"])),
                            SGSTAmountBill = Convert.ToDouble(DALHelper.HandleDBNull(reader["SGSTAmount"])),
                            CGSTPercentBill = Convert.ToDouble(DALHelper.HandleDBNull(reader["CGSTPercentage"])),
                            CGSTAmountBill = Convert.ToDouble(DALHelper.HandleDBNull(reader["CGSTAmount"])),
                            IGSTPercentBill = Convert.ToDouble(DALHelper.HandleDBNull(reader["IGSTPercentage"])),
                            IGSTAmountBill = Convert.ToDouble(DALHelper.HandleDBNull(reader["IGSTAmount"]))
                        };
                        nvo.Details.Items.Add(item);
                    }
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

        public override IValueObject GetItemSaleDetails(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetItemSalesDetailsBizActionVO nvo = valueObject as clsGetItemSalesDetailsBizActionVO;
            nvo.Details = new List<clsItemSalesDetailsVO>();
            clsItemSalesDetailsVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemSalesDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ItemSalesID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsItemSalesDetailsVO {
                            IsItemSaleReturn = !nvo.IsFromItemSaleReturn ? false : true,
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            ItemSaleId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemSaleId"])),
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"])),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            BatchID = Convert.ToInt32(DALHelper.HandleDBNull(reader["BatchId"])),
                            Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"])),
                            PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQuantity"])),
                            MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"])),
                            Amount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"])),
                            VATPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["VATPercentage"])),
                            VATAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["VATAmount"])),
                            ConcessionPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionPercentage"])),
                            ConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"])),
                            NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            InclusiveOfTax = Convert.ToBoolean(DALHelper.HandleDBNull(reader["InclusiveOfTax"])),
                            ItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["ItemVatType"])),
                            ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"])),
                            SelectedUOM = { 
                                Description = Convert.ToString(DALHelper.HandleDBNull(reader["TransactionUOM"])),
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]))
                            },
                            BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUM"])),
                            SellingUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SellingUMID"])),
                            PUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PUMID"])),
                            PUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PurchaseUM"])),
                            PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PurchaseUM"])),
                            SellingUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SellingUM"])),
                            ActualNetAmt = Convert.ToSingle(DALHelper.HandleDBNull(reader["ActualNetAmt"])),
                            NetAmtCalculation = Convert.ToSingle(DALHelper.HandleDBNull(reader["NetAmtCalculation"])),
                            StockingQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockingQuantity"])),
                            SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"])),
                            BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"])),
                            BaseQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseQuantity"])),
                            BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])),
                            SaleUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"])),
                            SaleUOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransactionUOM"])),
                            StockCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"])),
                            SGSTPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["SGSTPercentage"])),
                            SGSTAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["SGSTAmount"])),
                            SGSTtaxtype = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTTaxType"])),
                            SGSTapplicableon = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTApplicableOn"])),
                            CGSTPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["CGSTPercentage"])),
                            CGSTAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["CGSTAmount"])),
                            CGSTtaxtype = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTTaxType"])),
                            CGSTapplicableon = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTApplicableOn"])),
                            IGSTPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["IGSTPercentage"])),
                            IGSTAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["IGSTAmount"])),
                            IGSTtaxtype = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTTaxType"])),
                            IGSTapplicableon = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTApplicableOn"]))
                        };
                        nvo.Details.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
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

        public override IValueObject GetItemSaleDetailsForCashSettlement(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetItemSalesDetailsForCashSettlementBizActionVO nvo = valueObject as clsGetItemSalesDetailsForCashSettlementBizActionVO;
            nvo.Details = new List<clsItemSalesDetailsVO>();
            clsItemSalesDetailsVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemSalesDetailsForCashSettlement");
                this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.Int64, nvo.BillID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsItemSalesDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            ItemSaleId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemSaleId"])),
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"])),
                            Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]))),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            BatchID = Convert.ToInt32(DALHelper.HandleDBNull(reader["BatchId"])),
                            Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"])),
                            NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"])),
                            SettleNetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"])),
                            PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQuantity"])),
                            MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"])),
                            Amount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"])),
                            ConcessionPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionPercentage"])),
                            ConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"])),
                            DiscountPerc = Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountPerc"])),
                            DiscountAmt = Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountAmt"])),
                            DeductiblePerc = Convert.ToDouble(DALHelper.HandleDBNull(reader["DeductionPerc"])),
                            DeductibleAmt = Convert.ToDouble(DALHelper.HandleDBNull(reader["DeductionAmt"])),
                            CompanyDiscountPerc = Convert.ToDouble(DALHelper.HandleDBNull(reader["CompanyDiscountPerc"])),
                            CompanyDiscountAmt = Convert.ToDouble(DALHelper.HandleDBNull(reader["CompanyDiscountAmt"])),
                            VATPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["VATPercentage"])),
                            VATAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["VATAmount"])),
                            ItemPaidAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PatientPaidAmount"])),
                            CompanyPaidAmt = Convert.ToDouble(DALHelper.HandleDBNull(reader["CompanyPaidAmt"])),
                            IsCashTariff = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCashTariff"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]))
                        };
                        nvo.Details.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
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

        public override IValueObject GetItemSaleReturn(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetItemSalesReturnBizActionVO nvo = valueObject as clsGetItemSalesReturnBizActionVO;
            nvo.Details = new List<clsItemSalesReturnVO>();
            clsItemSalesReturnVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemSalesReturn");
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SerachExpression);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsItemSalesReturnVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"])),
                            Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"])),
                            ItemSaleReturnNo = Convert.ToString(DALHelper.HandleDBNull(reader["SalesReturnNo"])),
                            ItemSalesID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemSaleId"])),
                            Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"])),
                            ConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"])),
                            ConcessionPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionPercentage"])),
                            VATAmount = Convert.ToInt64(DALHelper.HandleDBNull(reader["VATAmount"])),
                            VATPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["VATPercentage"])),
                            TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalNetAmount"])),
                            NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"])),
                            TotalReturnAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalReturnAmount"]))
                        };
                        nvo.Details.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
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

        public override IValueObject GetItemSaleReturnDetails(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetItemSalesReturnDetailsBizActionVO nvo = valueObject as clsGetItemSalesReturnDetailsBizActionVO;
            nvo.Details = new List<clsItemSalesReturnDetailsVO>();
            clsItemSalesReturnDetailsVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemSalesReturnDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ItemSalesReturnID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsItemSalesReturnDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            ItemSaleReturnId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemSaleReturnId"])),
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"])),
                            BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchId"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            ReturnQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["ReturnedQuantity"])),
                            MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"])),
                            Amount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"])),
                            ConcessionPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionPercentage"])),
                            ConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"])),
                            VATPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["VATPercentage"])),
                            VATAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["VATAmount"])),
                            TotalSalesAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"])),
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            ItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["ItemVatType"])),
                            BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUM"])),
                            SellingUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"])),
                            SelectedUOM = { Description = Convert.ToString(DALHelper.HandleDBNull(reader["TransactionUOM"])) },
                            BaseQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseQuantity"]))
                        };
                        nvo.Details.Add(item);
                    }
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
    }
}

