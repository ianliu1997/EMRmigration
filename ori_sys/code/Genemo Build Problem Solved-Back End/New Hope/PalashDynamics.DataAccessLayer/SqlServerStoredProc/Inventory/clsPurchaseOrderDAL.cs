namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Inventory
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Inventory;
    using PalashDynamics.ValueObjects.Inventory.PurchaseOrder;
    using PalashDynamics.ValueObjects.Log;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Reflection;

    public class clsPurchaseOrderDAL : clsBasePurchaseOrderDAL
    {
        private Database dbServer;
        private LogManager logManager;
        private bool IsAuditTrail;

        private clsPurchaseOrderDAL()
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

        private clsAddRateContractBizActionVO AddDetails(clsAddRateContractBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                clsRateContractVO rateContract = BizActionObj.RateContract;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddRateContract");
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, rateContract.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, rateContract.Description);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierID", DbType.Int64, rateContract.SupplierID);
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, rateContract.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, rateContract.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "ContractDate ", DbType.DateTime, rateContract.ContractDate);
                this.dbServer.AddInParameter(storedProcCommand, "ContractValue", DbType.Decimal, rateContract.ContractValue);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreeze", DbType.Boolean, rateContract.IsFreeze);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierRepresentative", DbType.String, rateContract.SupplierRepresentative);
                this.dbServer.AddInParameter(storedProcCommand, "ClinicRepresentativeID", DbType.Int64, rateContract.ClinicRepresentativeID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, rateContract.Remark);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, rateContract.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, rateContract.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.RateContract.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                rateContract.UnitId = UserVo.UserLoginInfo.UnitId;
                if ((BizActionObj.SuccessStatus == 1) && ((rateContract.ContractDetails != null) && (rateContract.ContractDetails.Count > 0)))
                {
                    foreach (clsRateContractDetailsVO svo in rateContract.ContractDetails)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddRateContractItemDetails");
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, rateContract.UnitId);
                        this.dbServer.AddInParameter(command2, "ContractID", DbType.Int64, rateContract.ID);
                        this.dbServer.AddInParameter(command2, "ContractUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "ItemID", DbType.Int64, svo.ItemID);
                        this.dbServer.AddInParameter(command2, "Rate", DbType.Decimal, svo.Rate);
                        this.dbServer.AddInParameter(command2, "MRP", DbType.Decimal, svo.MRP);
                        this.dbServer.AddInParameter(command2, "TotalRate", DbType.Decimal, svo.CostRate);
                        this.dbServer.AddInParameter(command2, "TotalMRP", DbType.Decimal, svo.Amount);
                        this.dbServer.AddInParameter(command2, "TransactionUOMID", DbType.Int64, svo.SelectedUOM.ID);
                        this.dbServer.AddInParameter(command2, "CoversionFactor", DbType.Single, svo.ConversionFactor);
                        this.dbServer.AddInParameter(command2, "StockingQuantity", DbType.Decimal, svo.Quantity * Convert.ToDecimal(svo.ConversionFactor));
                        this.dbServer.AddInParameter(command2, "StockUOMID", DbType.Int64, svo.SUOMID);
                        this.dbServer.AddInParameter(command2, "BaseCF", DbType.Single, svo.BaseConversionFactor);
                        this.dbServer.AddInParameter(command2, "BaseQuantity", DbType.Decimal, svo.Quantity * Convert.ToDecimal(svo.BaseConversionFactor));
                        this.dbServer.AddInParameter(command2, "BaseUMID", DbType.Int64, svo.BaseUOMID);
                        this.dbServer.AddInParameter(command2, "BaseRate", DbType.Decimal, svo.BaseRate);
                        this.dbServer.AddInParameter(command2, "BaseMRP", DbType.Decimal, svo.BaseMRP);
                        this.dbServer.AddInParameter(command2, "DiscountPercent", DbType.Decimal, svo.DiscountPercent);
                        this.dbServer.AddInParameter(command2, "DiscountAmount", DbType.Decimal, svo.DiscountAmount);
                        this.dbServer.AddInParameter(command2, "NetAmount", DbType.Decimal, svo.NetAmount);
                        this.dbServer.AddInParameter(command2, "Remarks", DbType.String, svo.Remarks);
                        this.dbServer.AddInParameter(command2, "Quantity", DbType.Decimal, svo.Quantity);
                        this.dbServer.AddInParameter(command2, "UnlimitedQuantity", DbType.Boolean, svo.UnlimitedQuantity);
                        if (svo.SelectedCondition != null)
                        {
                            this.dbServer.AddInParameter(command2, "Condition", DbType.String, svo.SelectedCondition.Description);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "Condition", DbType.String, null);
                        }
                        this.dbServer.AddInParameter(command2, "MinQuantity", DbType.Decimal, svo.MinQuantity);
                        this.dbServer.AddInParameter(command2, "MaxQuantity", DbType.Decimal, svo.MaxQuantity);
                        this.dbServer.AddOutParameter(command2, "ID", DbType.Int64, 0);
                        this.dbServer.ExecuteNonQuery(command2, transaction);
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.RateContract = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return BizActionObj;
        }

        public override IValueObject AddPurchaseOrder(IValueObject valueObject, clsUserVO UserVo)
        {
            Guid activityId = default(Guid);
            if (this.IsAuditTrail)
            {
                this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "Add Purchase Order Start");
            }
            DbConnection connection = this.dbServer.CreateConnection();
            clsAddPurchaseOrderBizActionVO nvo = null;
            clsPurchaseOrderVO purchaseOrder = null;
            DbTransaction transaction = null;
            int num = 0;
            try
            {
                DbCommand command;
                if (this.IsAuditTrail)
                {
                    this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "try Start ");
                }
                connection.Open();
                transaction = connection.BeginTransaction();
                nvo = valueObject as clsAddPurchaseOrderBizActionVO;
                purchaseOrder = nvo.PurchaseOrder;
                if (nvo.IsEditMode)
                {
                    if (this.IsAuditTrail)
                    {
                        this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "CIMS_UpdatePurchaseOrder Start");
                    }
                    command = this.dbServer.GetStoredProcCommand("CIMS_UpdatePurchaseOrder");
                    command.Connection = connection;
                    this.dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, purchaseOrder.UpdatedBy);
                    if (purchaseOrder.UpdatedOn != null)
                    {
                        purchaseOrder.UpdatedOn = purchaseOrder.UpdatedOn.Trim();
                    }
                    this.dbServer.AddInParameter(command, "UpdatedOn", DbType.String, purchaseOrder.UpdatedOn);
                    this.dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, purchaseOrder.UpdatedDateTime);
                    if (purchaseOrder.UpdatedWindowsLoginName != null)
                    {
                        purchaseOrder.UpdatedWindowsLoginName = purchaseOrder.UpdatedWindowsLoginName.Trim();
                    }
                    this.dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, purchaseOrder.UpdatedWindowsLoginName);
                    this.dbServer.AddInParameter(command, "ID", DbType.Int64, purchaseOrder.ID);
                    this.dbServer.AddInParameter(command, "UpdatedUnitId", DbType.Int64, purchaseOrder.UpdatedUnitId);
                    if (this.IsAuditTrail)
                    {
                        this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "CIMS_UpdatePurchaseOrder End ");
                    }
                }
                else
                {
                    if (this.IsAuditTrail)
                    {
                        this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "CIMS_AddPurchaseOrder Start");
                    }
                    command = this.dbServer.GetStoredProcCommand("CIMS_AddPurchaseOrder");
                    command.Connection = connection;
                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, purchaseOrder.AddedBy);
                    if (purchaseOrder.AddedOn != null)
                    {
                        purchaseOrder.AddedOn = purchaseOrder.AddedOn.Trim();
                    }
                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, purchaseOrder.AddedOn);
                    this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, purchaseOrder.AddedDateTime);
                    if (purchaseOrder.AddedWindowsLoginName != null)
                    {
                        purchaseOrder.AddedWindowsLoginName = purchaseOrder.AddedWindowsLoginName.Trim();
                    }
                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, purchaseOrder.AddedWindowsLoginName);
                    this.dbServer.AddOutParameter(command, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, purchaseOrder.CreatedUnitId);
                    this.dbServer.AddInParameter(command, "Status", DbType.Boolean, purchaseOrder.Status);
                    this.dbServer.AddInParameter(command, "PONO", DbType.String, purchaseOrder.PONO);
                    if (this.IsAuditTrail)
                    {
                        this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "CIMS_AddPurchaseOrder End");
                    }
                }
                this.dbServer.AddInParameter(command, "LinkServer", DbType.String, purchaseOrder.LinkServer);
                if (purchaseOrder.LinkServer != null)
                {
                    this.dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, purchaseOrder.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(command, "Date", DbType.DateTime, purchaseOrder.Date);
                this.dbServer.AddInParameter(command, "Time", DbType.DateTime, purchaseOrder.Time);
                this.dbServer.AddInParameter(command, "StoreID", DbType.Int64, purchaseOrder.StoreID);
                this.dbServer.AddInParameter(command, "SupplierID", DbType.Int64, purchaseOrder.SupplierID);
                this.dbServer.AddInParameter(command, "IndentID", DbType.Int64, purchaseOrder.IndentID);
                this.dbServer.AddInParameter(command, "IndentUnitID", DbType.Int64, purchaseOrder.IndentUnitID);
                this.dbServer.AddInParameter(command, "EnquiryID", DbType.Int64, purchaseOrder.EnquiryID);
                this.dbServer.AddInParameter(command, "DeliveryDuration", DbType.String, purchaseOrder.DeliveryDuration);
                this.dbServer.AddInParameter(command, "DeliveryDays", DbType.Int64, purchaseOrder.DeliveryDays);
                this.dbServer.AddInParameter(command, "PaymentMode", DbType.Int16, (short) purchaseOrder.PaymentModeID);
                this.dbServer.AddInParameter(command, "PaymentTerm", DbType.Int64, purchaseOrder.PaymentTerms);
                this.dbServer.AddInParameter(command, "Guarantee_Warrantee", DbType.String, purchaseOrder.Guarantee_Warrantee);
                this.dbServer.AddInParameter(command, "Schedule", DbType.Int64, purchaseOrder.Schedule);
                this.dbServer.AddInParameter(command, "Remarks", DbType.String, purchaseOrder.Remarks);
                this.dbServer.AddInParameter(command, "TotalDiscount", DbType.Decimal, purchaseOrder.TotalDiscount);
                this.dbServer.AddInParameter(command, "TotalVAT", DbType.Decimal, purchaseOrder.TotalVAT);
                this.dbServer.AddInParameter(command, "TotalAmount", DbType.Decimal, purchaseOrder.TotalAmount);
                this.dbServer.AddInParameter(command, "TotalSGST", DbType.Decimal, purchaseOrder.TotalSGST);
                this.dbServer.AddInParameter(command, "TotalCGST", DbType.Decimal, purchaseOrder.TotalCGST);
                this.dbServer.AddInParameter(command, "TotalIGST", DbType.Decimal, purchaseOrder.TotalIGST);
                this.dbServer.AddInParameter(command, "PrevTotalNetAmount", DbType.Decimal, purchaseOrder.PrevTotalNet);
                this.dbServer.AddInParameter(command, "OtherCharges", DbType.Decimal, purchaseOrder.OtherCharges);
                this.dbServer.AddInParameter(command, "PODiscount", DbType.Decimal, purchaseOrder.PODiscount);
                this.dbServer.AddInParameter(command, "TotalNetAmount", DbType.Decimal, purchaseOrder.TotalNet);
                this.dbServer.AddInParameter(command, "UnitId", DbType.Int64, purchaseOrder.UnitId);
                this.dbServer.AddInParameter(command, "Direct", DbType.Boolean, purchaseOrder.Direct);
                this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddParameter(command, "PurchaseOrderNumber", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "000000000000000000000000000000000000000000000000000");
                int num2 = this.dbServer.ExecuteNonQuery(command, transaction);
                if (this.IsAuditTrail)
                {
                    this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "Status intStatus = " + Convert.ToString(num2));
                }
                if (!nvo.IsEditMode)
                {
                    nvo.PurchaseOrder.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                }
                if (this.IsAuditTrail)
                {
                    this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "Purchase Order ID : " + Convert.ToString(nvo.PurchaseOrder.ID) + " Unit ID - " + Convert.ToString(purchaseOrder.UnitId));
                }
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                nvo.PurchaseOrder.PONO = (string) this.dbServer.GetParameterValue(command, "PurchaseOrderNumber");
                if (this.IsAuditTrail)
                {
                    this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "objBizActionVO.IsEditMode : " + Convert.ToString(nvo.IsEditMode));
                }
                if (this.IsAuditTrail && (nvo.LogInfoList != null))
                {
                    LogInfo item = new LogInfo();
                    Guid guid2 = default(Guid);
                    item.guid = guid2;
                    item.UserId = UserVo.ID;
                    item.TimeStamp = DateTime.Now;
                    item.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                    item.MethodName = MethodBase.GetCurrentMethod().ToString();
                    string[] strArray = new string[] { " 39 : After Saving PO \r\nUser Unit Id : ", Convert.ToString(UserVo.UserLoginInfo.UnitId), " PO ID : ", Convert.ToString(nvo.PurchaseOrder.ID), " PONO : ", Convert.ToString(nvo.PurchaseOrder.PONO), " \r\n" };
                    item.Message = string.Concat(strArray);
                    item.Message = item.Message.Replace("\r\n", Environment.NewLine);
                    nvo.LogInfoList.Add(item);
                }
                if (nvo.IsEditMode)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeletePurchaseOrderDetails");
                    command2.Connection = connection;
                    command2.Parameters.Clear();
                    this.dbServer.AddInParameter(command2, "POID", DbType.Int64, purchaseOrder.ID);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, purchaseOrder.UnitId);
                    this.dbServer.AddInParameter(command2, "ResultStatus", DbType.Int64, 0);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPurchaseOrderDetails");
                if (this.IsAuditTrail)
                {
                    this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "CIMS_AddPurchaseOrderDetails Start ");
                }
                storedProcCommand.Connection = connection;
                foreach (clsPurchaseOrderDetailVO lvo in nvo.PurchaseOrder.OrgItems)
                {
                    if (this.IsAuditTrail)
                    {
                        this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "it.ItemID : -  " + Convert.ToString(lvo.ItemID));
                    }
                    foreach (clsPurchaseOrderDetailVO lvo2 in nvo.PurchaseOrder.Items)
                    {
                        if (this.IsAuditTrail)
                        {
                            this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "item.ItemID : -  " + Convert.ToString(lvo2.ItemID) + " item.CheckInserted : - " + Convert.ToString(lvo2.CheckInserted));
                        }
                        long? itemID = lvo2.ItemID;
                        long? nullable2 = lvo.ItemID;
                        if (((itemID.GetValueOrDefault() == nullable2.GetValueOrDefault()) && ((itemID != null) == (nullable2 != null))) && !lvo2.CheckInserted)
                        {
                            if (this.IsAuditTrail)
                            {
                                this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "IF Start 1");
                            }
                            lvo2.CheckInserted = true;
                            storedProcCommand.Parameters.Clear();
                            lvo2.POID = nvo.PurchaseOrder.ID;
                            if (this.IsAuditTrail)
                            {
                                this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "item.POI : -  " + Convert.ToString(lvo2.POID));
                            }
                            this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, purchaseOrder.LinkServer);
                            if (purchaseOrder.LinkServer != null)
                            {
                                this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, purchaseOrder.LinkServer.Replace(@"\", "_"));
                            }
                            this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, purchaseOrder.UnitId);
                            this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, purchaseOrder.Date);
                            this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, purchaseOrder.Time);
                            this.dbServer.AddInParameter(storedProcCommand, "POID", DbType.Int64, lvo2.POID);
                            this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, lvo2.ItemID);
                            if (this.IsAuditTrail)
                            {
                                this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "item.SinleLineItem : -  " + Convert.ToString(lvo2.SinleLineItem));
                            }
                            if (lvo2.SinleLineItem)
                            {
                                this.dbServer.AddInParameter(storedProcCommand, "Quantity", DbType.Decimal, lvo2.Quantity);
                                this.dbServer.AddInParameter(storedProcCommand, "Amount", DbType.Decimal, lvo2.Amount / Convert.ToDecimal(lvo2.PurchaseToBaseCF));
                                this.dbServer.AddInParameter(storedProcCommand, "DiscountPercent", DbType.Decimal, lvo2.DiscountPercent);
                                this.dbServer.AddInParameter(storedProcCommand, "DiscountAmount", DbType.Decimal, lvo2.DiscountAmount / Convert.ToDecimal(lvo2.PurchaseToBaseCF));
                                this.dbServer.AddInParameter(storedProcCommand, "VatPercent", DbType.Decimal, lvo2.VATPercent);
                                this.dbServer.AddInParameter(storedProcCommand, "VATAmount", DbType.Decimal, lvo2.VATAmount / Convert.ToDecimal(lvo2.PurchaseToBaseCF));
                                this.dbServer.AddInParameter(storedProcCommand, "Itemtax", DbType.Decimal, lvo2.ItemVATPercent);
                                this.dbServer.AddInParameter(storedProcCommand, "ItemTaxAmount", DbType.Decimal, lvo2.ItemVATAmount / Convert.ToDecimal(lvo2.PurchaseToBaseCF));
                                this.dbServer.AddInParameter(storedProcCommand, "SGSTPercent", DbType.Decimal, lvo2.SGSTPercent);
                                this.dbServer.AddInParameter(storedProcCommand, "SGSTAmount", DbType.Decimal, lvo2.SGSTAmount);
                                this.dbServer.AddInParameter(storedProcCommand, "CGSTPercent", DbType.Decimal, lvo2.CGSTPercent);
                                this.dbServer.AddInParameter(storedProcCommand, "CGSTAmount", DbType.Decimal, lvo2.CGSTAmount);
                                this.dbServer.AddInParameter(storedProcCommand, "IGSTPercent", DbType.Decimal, lvo2.IGSTPercent);
                                this.dbServer.AddInParameter(storedProcCommand, "IGSTAmount", DbType.Decimal, lvo2.IGSTAmount);
                                this.dbServer.AddInParameter(storedProcCommand, "NetAmount", DbType.Decimal, lvo2.NetAmount / Convert.ToDecimal(lvo2.PurchaseToBaseCF));
                            }
                            else
                            {
                                this.dbServer.AddInParameter(storedProcCommand, "Quantity", DbType.Decimal, lvo2.Quantity * Convert.ToDecimal(lvo2.BaseConversionFactor));
                                this.dbServer.AddInParameter(storedProcCommand, "Amount", DbType.Decimal, lvo2.Amount);
                                this.dbServer.AddInParameter(storedProcCommand, "DiscountPercent", DbType.Decimal, lvo2.DiscountPercent);
                                this.dbServer.AddInParameter(storedProcCommand, "DiscountAmount", DbType.Decimal, lvo2.DiscountAmount);
                                this.dbServer.AddInParameter(storedProcCommand, "VatPercent", DbType.Decimal, lvo2.VATPercent);
                                this.dbServer.AddInParameter(storedProcCommand, "VATAmount", DbType.Decimal, lvo2.VATAmount);
                                this.dbServer.AddInParameter(storedProcCommand, "Itemtax", DbType.Decimal, lvo2.ItemVATPercent);
                                this.dbServer.AddInParameter(storedProcCommand, "ItemTaxAmount", DbType.Decimal, lvo2.ItemVATAmount);
                                this.dbServer.AddInParameter(storedProcCommand, "SGSTPercent", DbType.Decimal, lvo2.SGSTPercent);
                                this.dbServer.AddInParameter(storedProcCommand, "SGSTAmount", DbType.Decimal, lvo2.SGSTAmount);
                                this.dbServer.AddInParameter(storedProcCommand, "CGSTPercent", DbType.Decimal, lvo2.CGSTPercent);
                                this.dbServer.AddInParameter(storedProcCommand, "CGSTAmount", DbType.Decimal, lvo2.CGSTAmount);
                                this.dbServer.AddInParameter(storedProcCommand, "IGSTPercent", DbType.Decimal, lvo2.IGSTPercent);
                                this.dbServer.AddInParameter(storedProcCommand, "IGSTAmount", DbType.Decimal, lvo2.IGSTAmount);
                                this.dbServer.AddInParameter(storedProcCommand, "NetAmount", DbType.Decimal, lvo2.NetAmount);
                            }
                            this.dbServer.AddInParameter(storedProcCommand, "Rate", DbType.Decimal, lvo2.Rate);
                            this.dbServer.AddInParameter(storedProcCommand, "MRP", DbType.Decimal, lvo2.MRP);
                            if (lvo2.SinleLineItem)
                            {
                                this.dbServer.AddInParameter(storedProcCommand, "InputTransactionQuantity", DbType.Double, lvo2.Quantity / Convert.ToDecimal(lvo2.PurchaseToBaseCF));
                                if (this.IsAuditTrail)
                                {
                                    this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "InputTransactionQuantity : -  " + Convert.ToString((decimal) (lvo2.Quantity / Convert.ToDecimal(lvo2.PurchaseToBaseCF))));
                                }
                            }
                            else
                            {
                                this.dbServer.AddInParameter(storedProcCommand, "InputTransactionQuantity", DbType.Double, lvo2.Quantity);
                                if (this.IsAuditTrail)
                                {
                                    this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "InputTransactionQuantity : -  " + Convert.ToString(lvo2.Quantity));
                                }
                            }
                            this.dbServer.AddInParameter(storedProcCommand, "TransactionUOMID", DbType.Double, lvo2.SelectedUOM.ID);
                            this.dbServer.AddInParameter(storedProcCommand, "BaseUMID", DbType.Double, lvo2.BaseUOMID);
                            if (lvo2.SinleLineItem)
                            {
                                this.dbServer.AddInParameter(storedProcCommand, "BaseCF", DbType.Double, lvo2.PurchaseToBaseCF);
                                this.dbServer.AddInParameter(storedProcCommand, "BaseQuantity", DbType.Decimal, lvo2.Quantity);
                                this.dbServer.AddInParameter(storedProcCommand, "StockCF", DbType.Double, lvo2.StockingToBaseCF);
                                this.dbServer.AddInParameter(storedProcCommand, "StockingQuantity", DbType.Double, Convert.ToSingle((decimal) (lvo2.Quantity / Convert.ToDecimal(lvo2.PurchaseToBaseCF))) * lvo2.StockingToBaseCF);
                            }
                            else
                            {
                                this.dbServer.AddInParameter(storedProcCommand, "BaseCF", DbType.Double, lvo2.BaseConversionFactor);
                                this.dbServer.AddInParameter(storedProcCommand, "BaseQuantity", DbType.Decimal, lvo2.Quantity * Convert.ToDecimal(lvo2.BaseConversionFactor));
                                this.dbServer.AddInParameter(storedProcCommand, "StockCF", DbType.Double, lvo2.ConversionFactor);
                                this.dbServer.AddInParameter(storedProcCommand, "StockingQuantity", DbType.Double, Convert.ToSingle(lvo2.Quantity) * lvo2.ConversionFactor);
                            }
                            this.dbServer.AddInParameter(storedProcCommand, "StockUOMID", DbType.Double, lvo2.SUOMID);
                            this.dbServer.AddInParameter(storedProcCommand, "Taxtype", DbType.Int32, lvo2.POItemVatType);
                            this.dbServer.AddInParameter(storedProcCommand, "VatApplicableon", DbType.Int32, lvo2.POItemVatApplicationOn);
                            this.dbServer.AddInParameter(storedProcCommand, "otherTaxType", DbType.Int32, lvo2.POItemOtherTaxType);
                            this.dbServer.AddInParameter(storedProcCommand, "othertaxApplicableon", DbType.Int32, lvo2.POItemOtherTaxApplicationOn);
                            this.dbServer.AddInParameter(storedProcCommand, "SGSTType", DbType.Int32, lvo2.POSGSTVatType);
                            this.dbServer.AddInParameter(storedProcCommand, "SGSTApplicableOn", DbType.Int32, lvo2.POSGSTVatApplicationOn);
                            this.dbServer.AddInParameter(storedProcCommand, "CGSTType", DbType.Int32, lvo2.POCGSTVatType);
                            this.dbServer.AddInParameter(storedProcCommand, "CGSTApplicableOn", DbType.Int32, lvo2.POCGSTVatApplicationOn);
                            this.dbServer.AddInParameter(storedProcCommand, "IGSTType", DbType.Int32, lvo2.POIGSTVatType);
                            this.dbServer.AddInParameter(storedProcCommand, "IGSTApplicableOn", DbType.Int32, lvo2.POIGSTVatApplicationOn);
                            this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, lvo2.Specification);
                            this.dbServer.AddInParameter(storedProcCommand, "EditForApprove", DbType.Boolean, purchaseOrder.EditForApprove);
                            this.dbServer.AddInParameter(storedProcCommand, "PONO", DbType.String, purchaseOrder.PONO);
                            if (this.IsAuditTrail)
                            {
                                this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "item.ConditionFound : -  " + Convert.ToString(lvo2.ConditionFound));
                            }
                            this.dbServer.AddInParameter(storedProcCommand, "RateContractID", DbType.Int64, lvo2.RateContractID);
                            this.dbServer.AddInParameter(storedProcCommand, "RateContractUnitID", DbType.Int64, lvo2.RateContractUnitID);
                            this.dbServer.AddInParameter(storedProcCommand, "RateContractCondition", DbType.String, lvo2.RateContractCondition);
                            if (lvo2.SelectedCurrency != null)
                            {
                                this.dbServer.AddInParameter(storedProcCommand, "CurrencyID", DbType.Int64, lvo2.SelectedCurrency.ID);
                            }
                            else
                            {
                                this.dbServer.AddInParameter(storedProcCommand, "CurrencyID", DbType.Int64, 0);
                            }
                            if (this.IsAuditTrail)
                            {
                                this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "item.SelectedCurrency : -  " + Convert.ToString(lvo2.SelectedCurrency));
                            }
                            this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, lvo2.PoDetailsID);
                            int num3 = this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                            if (this.IsAuditTrail)
                            {
                                this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "intStatus1 : -  " + Convert.ToString(num3));
                            }
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                            if (this.IsAuditTrail)
                            {
                                this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "objBizActionVO.SuccessStatus : -  " + Convert.ToString(nvo.SuccessStatus));
                            }
                            long parameterValue = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                            if (this.IsAuditTrail)
                            {
                                this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "PoDetailsID : -  " + Convert.ToString(parameterValue));
                            }
                            lvo.PoDetailsID = parameterValue;
                            lvo2.PoDetailsID = parameterValue;
                            if (this.IsAuditTrail)
                            {
                                this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "IF End 1");
                            }
                            break;
                        }
                        long? nullable3 = lvo2.ItemID;
                        long? nullable4 = lvo.ItemID;
                        if (((nullable3.GetValueOrDefault() == nullable4.GetValueOrDefault()) && ((nullable3 != null) == (nullable4 != null))) && lvo2.CheckInserted)
                        {
                            if (this.IsAuditTrail)
                            {
                                this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "else IF Start 1");
                            }
                            lvo.PoDetailsID = lvo2.PoDetailsID;
                            if (this.IsAuditTrail)
                            {
                                this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "item.PoDetailsID : -  " + Convert.ToString(lvo2.PoDetailsID));
                            }
                            if (this.IsAuditTrail)
                            {
                                this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "else IF End 1");
                            }
                        }
                    }
                    if ((nvo.POIndentList != null) && (nvo.POIndentList.Count > 0))
                    {
                        if (this.IsAuditTrail)
                        {
                            this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "IF Start 2");
                            this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "objBizActionVO.POIndentList.Count : -  " + Convert.ToString(nvo.POIndentList.Count));
                        }
                        foreach (clsPurchaseOrderDetailVO lvo3 in nvo.POIndentList)
                        {
                            if (this.IsAuditTrail)
                            {
                                string[] strArray2 = new string[] { "item2.ItemID : -  ", Convert.ToString(lvo3.ItemID), " /item2.IndentID : -  ", Convert.ToString(lvo3.IndentID), " /item2.IndentUnitID : -  ", Convert.ToString(lvo3.IndentUnitID), " /item2.IndentDetailID : -  ", Convert.ToString(lvo3.IndentDetailID), " /item2.IndentDetailUnitID : -  " };
                                strArray2[9] = Convert.ToString(lvo3.IndentDetailUnitID);
                                this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), string.Concat(strArray2));
                                string[] strArray3 = new string[] { "it.ItemID : -  ", Convert.ToString(lvo.ItemID), " /it.IndentID : -  ", Convert.ToString(lvo.IndentID), " /it.IndentUnitID : -  ", Convert.ToString(lvo.IndentUnitID), " /it.IndentDetailID : -  ", Convert.ToString(lvo.IndentDetailID), " /it.IndentDetailUnitID : -  " };
                                strArray3[9] = Convert.ToString(lvo.IndentDetailUnitID);
                                this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), string.Concat(strArray3));
                            }
                            long? itemID = lvo3.ItemID;
                            long? nullable6 = lvo.ItemID;
                            if (((itemID.GetValueOrDefault() == nullable6.GetValueOrDefault()) && ((itemID != null) == (nullable6 != null))) && ((lvo3.IndentID == lvo.IndentID) && ((lvo3.IndentUnitID == lvo.IndentUnitID) && ((lvo3.IndentDetailID == lvo.IndentDetailID) && (lvo3.IndentDetailUnitID == lvo.IndentDetailUnitID)))))
                            {
                                if (this.IsAuditTrail)
                                {
                                    this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "IF Start 3");
                                }
                                lvo.POID = nvo.PurchaseOrder.ID;
                                DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_AddPOIndentdetails");
                                if (this.IsAuditTrail)
                                {
                                    this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "CIMS_AddPOIndentdetails Start");
                                }
                                command4.Connection = connection;
                                command4.Parameters.Clear();
                                lvo.POID = nvo.PurchaseOrder.ID;
                                this.dbServer.AddInParameter(command4, "POID", DbType.Int64, lvo.POID);
                                this.dbServer.AddInParameter(command4, "PoUnitID", DbType.Int64, purchaseOrder.UnitId);
                                this.dbServer.AddInParameter(command4, "PODetailsID", DbType.Int64, lvo.PoDetailsID);
                                this.dbServer.AddInParameter(command4, "PODetailsUnitID", DbType.Int64, purchaseOrder.UnitId);
                                this.dbServer.AddInParameter(command4, "IndentID", DbType.Int64, lvo3.IndentID);
                                this.dbServer.AddInParameter(command4, "IndentUnitId", DbType.Int64, lvo3.IndentUnitID);
                                this.dbServer.AddInParameter(command4, "IndentDetailID", DbType.Int64, lvo3.IndentDetailID);
                                this.dbServer.AddInParameter(command4, "IndentDetailUnitID", DbType.Int64, lvo3.IndentDetailUnitID);
                                this.dbServer.AddInParameter(command4, "ItemID", DbType.Int64, lvo3.ItemID);
                                this.dbServer.AddInParameter(command4, "Quantity", DbType.Decimal, lvo.Quantity * Convert.ToDecimal(lvo.BaseConversionFactor));
                                this.dbServer.AddInParameter(command4, "PendingQuantity", DbType.Decimal, 0);
                                this.dbServer.AddInParameter(command4, "UnitId", DbType.Decimal, purchaseOrder.UnitId);
                                this.dbServer.AddInParameter(command4, "Rate", DbType.Decimal, lvo3.Rate);
                                this.dbServer.AddInParameter(command4, "MRP", DbType.Decimal, lvo3.MRP);
                                this.dbServer.AddInParameter(command4, "InputTransactionQuantity", DbType.Double, lvo.Quantity);
                                if (this.IsAuditTrail)
                                {
                                    this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "it.SinleLineItem : -  " + Convert.ToString(lvo.SinleLineItem));
                                }
                                if (lvo.SinleLineItem)
                                {
                                    this.dbServer.AddInParameter(command4, "TransactionUOMID", DbType.Double, lvo3.SelectedUOM.ID);
                                    this.dbServer.AddInParameter(command4, "BaseUMID", DbType.Double, lvo3.BaseUOMID);
                                    this.dbServer.AddInParameter(command4, "BaseCF", DbType.Double, lvo3.BaseConversionFactor);
                                    this.dbServer.AddInParameter(command4, "BaseQuantity", DbType.Decimal, lvo.Quantity * Convert.ToDecimal(lvo.BaseConversionFactor));
                                    this.dbServer.AddInParameter(command4, "StockUOMID", DbType.Double, lvo3.SUOMID);
                                    this.dbServer.AddInParameter(command4, "StockCF", DbType.Double, lvo3.ConversionFactor);
                                    this.dbServer.AddInParameter(command4, "StockingQuantity", DbType.Double, Convert.ToSingle(lvo.Quantity) * lvo.ConversionFactor);
                                }
                                else
                                {
                                    this.dbServer.AddInParameter(command4, "TransactionUOMID", DbType.Double, lvo.SelectedUOM.ID);
                                    this.dbServer.AddInParameter(command4, "BaseUMID", DbType.Double, lvo.BaseUOMID);
                                    this.dbServer.AddInParameter(command4, "BaseCF", DbType.Double, lvo.BaseConversionFactor);
                                    this.dbServer.AddInParameter(command4, "BaseQuantity", DbType.Decimal, lvo.Quantity * Convert.ToDecimal(lvo.BaseConversionFactor));
                                    this.dbServer.AddInParameter(command4, "StockUOMID", DbType.Double, lvo.SUOMID);
                                    this.dbServer.AddInParameter(command4, "StockCF", DbType.Double, lvo.ConversionFactor);
                                    this.dbServer.AddInParameter(command4, "StockingQuantity", DbType.Double, Convert.ToSingle(lvo.Quantity) * lvo.ConversionFactor);
                                }
                                this.dbServer.AddOutParameter(command4, "ID", DbType.Int64, 0);
                                this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, 0);
                                int num5 = this.dbServer.ExecuteNonQuery(command4, transaction);
                                if (this.IsAuditTrail)
                                {
                                    this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "intStatus4 : -  " + Convert.ToString(num5));
                                    this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "IF End 3");
                                }
                                num = 0;
                                if (Convert.ToInt32(this.dbServer.GetParameterValue(command4, "ResultStatus")) == 1)
                                {
                                    nvo.ItemCode = lvo3.ItemCode;
                                    throw new Exception();
                                }
                            }
                        }
                    }
                    if ((nvo.POIndentList != null) && (nvo.POIndentList.Count > 0))
                    {
                        bool editForApprove = purchaseOrder.EditForApprove;
                    }
                }
                if (nvo.POTerms != null)
                {
                    foreach (clsPurchaseOrderTerms terms in nvo.POTerms)
                    {
                        DbCommand command5 = this.dbServer.GetStoredProcCommand("CIMS_AddPOTermsAndCondition");
                        command5.Connection = connection;
                        command5.Parameters.Clear();
                        this.dbServer.AddInParameter(command5, "POID", DbType.Int64, nvo.PurchaseOrder.ID);
                        this.dbServer.AddInParameter(command5, "POUnitID", DbType.Int64, purchaseOrder.UnitId);
                        this.dbServer.AddInParameter(command5, "UnitID", DbType.Decimal, purchaseOrder.UnitId);
                        this.dbServer.AddInParameter(command5, "TermsAndConditionID", DbType.Int64, terms.TermsAndConditionID);
                        this.dbServer.AddInParameter(command5, "Status", DbType.Boolean, terms.Status);
                        this.dbServer.AddOutParameter(command5, "ID", DbType.Int64, 0);
                        this.dbServer.ExecuteNonQuery(command5, transaction);
                    }
                }
                transaction.Commit();
                if (this.IsAuditTrail)
                {
                    this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "Add Purchase Order Comit Completed");
                }
                if ((this.IsAuditTrail && ((nvo.LogInfoList != null) && (nvo.LogInfoList.Count > 0))) && this.IsAuditTrail)
                {
                    this.SetLogInfo(nvo.LogInfoList, UserVo.ID);
                    nvo.LogInfoList.Clear();
                }
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                nvo.SuccessStatus = (num != 1) ? -1 : -2;
                nvo.PurchaseOrder = null;
            }
            finally
            {
                if (this.IsAuditTrail)
                {
                    this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "Finally Block");
                }
                connection.Close();
                transaction = null;
                connection = null;
            }
            if (this.IsAuditTrail)
            {
                this.SetLogInfo(activityId, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "Return");
            }
            return nvo;
        }

        public override IValueObject AddRateContract(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddRateContractBizActionVO bizActionObj = valueObject as clsAddRateContractBizActionVO;
            bizActionObj = (bizActionObj.RateContract.ID != 0L) ? this.UpdateDetails(bizActionObj, UserVo) : this.AddDetails(bizActionObj, UserVo);
            return valueObject;
        }

        public override IValueObject CancelPurchaseOrder(IValueObject valueObject, clsUserVO UserVo)
        {
            IValueObject obj2;
            try
            {
                clsCancelPurchaseOrderBizActionVO nvo = null;
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CancelPO");
                nvo = valueObject as clsCancelPurchaseOrderBizActionVO;
                this.dbServer.AddInParameter(storedProcCommand, "POID", DbType.Int64, nvo.PurchaseOrder.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.PurchaseOrder.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, false);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                obj2 = nvo;
            }
            catch (Exception)
            {
                throw;
            }
            return obj2;
        }

        public override IValueObject CheckContractValidity(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCheckContractBizActionVO nvo = valueObject as clsCheckContractBizActionVO;
            try
            {
                DbDataReader reader = null;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CheckRateContract");
                this.dbServer.AddInParameter(storedProcCommand, "ItemIDs", DbType.String, nvo.ItemIDs);
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierID", DbType.Int64, nvo.SupplierID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.Result = true;
                    }
                }
            }
            catch
            {
            }
            return nvo;
        }

        public override IValueObject ClosePurchaseOrderManually(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            clsClosePurchaseOrderManuallyBizActionVO nvo = null;
            DbTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                nvo = valueObject as clsClosePurchaseOrderManuallyBizActionVO;
                clsPurchaseOrderVO purchaseOrder = nvo.PurchaseOrder;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ClosePurchaseOrderManually");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "POID", DbType.Int64, nvo.PurchaseOrder.ID);
                this.dbServer.AddInParameter(storedProcCommand, "POUnitID", DbType.Int64, nvo.PurchaseOrder.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, UserVo.UserGeneralDetailVO.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateBy", DbType.Int64, UserVo.UserGeneralDetailVO.UpdatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateOn", DbType.String, UserVo.UserGeneralDetailVO.UpdatedOn);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsUserName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, nvo.PurchaseOrder.Remarks);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                nvo.SuccessStatus = -1;
                nvo.PurchaseOrder = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject DeletePurchaseOrderItems(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPurchaseOrderBizActionVO nvo = null;
            clsPurchaseOrderVO purchaseOrder = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeletePurchaseOrderDetails");
                nvo = valueObject as clsAddPurchaseOrderBizActionVO;
                purchaseOrder = nvo.PurchaseOrder;
                this.dbServer.AddInParameter(storedProcCommand, "POID", DbType.Int64, purchaseOrder.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, purchaseOrder.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception exception)
            {
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                nvo.SuccessStatus = -1;
                nvo.PurchaseOrderList = null;
            }
            return nvo;
        }

        public override IValueObject FreezPurchaseOrder(IValueObject valueObject, clsUserVO UserVo)
        {
            clsFreezPurchaseOrderBizActionVO nvo = null;
            clsPurchaseOrderVO purchaseOrder = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FreezPurchaseOrder");
                nvo = valueObject as clsFreezPurchaseOrderBizActionVO;
                purchaseOrder = nvo.PurchaseOrder;
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, purchaseOrder.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, purchaseOrder.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception exception)
            {
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                nvo.SuccessStatus = -1;
                nvo.PurchaseOrderList = null;
            }
            return nvo;
        }

        public override IValueObject GetPendingPurchaseOrder(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPendingPurchaseOrderBizActionVO nvo = (clsGetPendingPurchaseOrderBizActionVO) valueObject;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPurchaseOrderForDashBoard");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                if (nvo.IsOrderBy != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "OrderBy", DbType.Int64, nvo.IsOrderBy);
                }
                this.dbServer.AddInParameter(storedProcCommand, "SupplierID", DbType.Int64, nvo.SearchSupplierID);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.SearchStoreID);
                DateTime searchFromDate = nvo.searchFromDate;
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.searchFromDate);
                DateTime searchToDate = nvo.searchToDate;
                nvo.searchToDate = nvo.searchToDate.AddDays(1.0);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.searchToDate);
                this.dbServer.AddInParameter(storedProcCommand, "PONO", DbType.String, nvo.PONO);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, nvo.UserID);
                this.dbServer.AddInParameter(storedProcCommand, "Freezed", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.NoOfRecordShow);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartIndex);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.OutputTotalRows);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PurchaseOrderList == null)
                    {
                        nvo.PurchaseOrderList = new List<clsPurchaseOrderVO>();
                    }
                    while (reader.Read())
                    {
                        clsPurchaseOrderVO item = new clsPurchaseOrderVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            PONO = Convert.ToString(DALHelper.HandleDBNull(reader["PONO"])),
                            Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"])),
                            TotalAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalAmount"])),
                            SupplierName = Convert.ToString(DALHelper.HandleDBNull(reader["SupplierName"])),
                            DeliveryLocation = Convert.ToString(DALHelper.HandleDBNull(reader["DeliveryLocation"])),
                            StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"])),
                            POAutoCloseDuration = Convert.ToInt32(DALHelper.HandleDBNull(reader["POAutoCloseDuration"])),
                            POAutoCloseDurationDate = DALHelper.HandleDate(reader["POAutoCloseDate"]),
                            ApprovedByLvl2Date = Convert.ToDateTime(DALHelper.HandleDate(reader["ApprovedLvl2Date"])),
                            POAutoCloseReason = Convert.ToString(DALHelper.HandleDBNull(reader["POAutoCloseReason"])),
                            IsAutoClose = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAutoClose"]))
                        };
                        nvo.PurchaseOrderList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.OutputTotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
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

        public override IValueObject GetPurchaseOrder(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            clsGetPurchaseOrderBizActionVO nvo = null;
            DbDataReader reader = null;
            try
            {
                connection.Open();
                connection.BeginTransaction();
                nvo = valueObject as clsGetPurchaseOrderBizActionVO;
                if (!nvo.flagPOFromGRN)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPurchaseOrder");
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "SupplierID", DbType.Int64, nvo.SearchSupplierID);
                    this.dbServer.AddInParameter(storedProcCommand, "CancelPO", DbType.Boolean, nvo.CancelPO);
                    this.dbServer.AddInParameter(storedProcCommand, "UnAPProvePO", DbType.Boolean, nvo.UnApprovePo);
                    this.dbServer.AddInParameter(storedProcCommand, "ApprovePO", DbType.Boolean, nvo.ApprovePo);
                    this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.SearchStoreID);
                    if (nvo.searchFromDate != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.searchFromDate);
                    }
                    if (nvo.searchToDate != null)
                    {
                        nvo.searchToDate = new DateTime?(nvo.searchToDate.Value.Date.AddDays(1.0));
                        this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.searchToDate);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "Freezed", DbType.Boolean, nvo.Freezed);
                    this.dbServer.AddInParameter(storedProcCommand, "PONO", DbType.String, nvo.PONO);
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.PurchaseOrderList == null)
                        {
                            nvo.PurchaseOrderList = new List<clsPurchaseOrderVO>();
                        }
                        while (reader.Read())
                        {
                            clsPurchaseOrderVO item = new clsPurchaseOrderVO();
                            bool flag1 = (bool) DALHelper.HandleDBNull(reader["Status"]);
                            item.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                            item.PONO = (string) DALHelper.HandleDBNull(reader["PONO"]);
                            item.Date = (DateTime) DALHelper.HandleDBNull(reader["Date"]);
                            item.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                            item.UnitId = (long) DALHelper.HandleDBNull(reader["UnitID"]);
                            item.StoreName = (string) DALHelper.HandleDBNull(reader["StoreName"]);
                            item.SupplierName = (string) DALHelper.HandleDBNull(reader["SupplierName"]);
                            item.StoreID = (long) DALHelper.HandleDBNull(reader["StoreID"]);
                            item.SupplierID = (long) DALHelper.HandleDBNull(reader["SupplierID"]);
                            item.EnquiryID = (long) DALHelper.HandleDBNull(reader["EnquiryID"]);
                            item.DeliveryDuration = (long) DALHelper.HandleDBNull(reader["DeliveryDuration"]);
                            item.PaymentMode = (long) DALHelper.HandleDBNull(reader["PaymentMode"]);
                            item.PaymentTerms = (long) DALHelper.HandleDBNull(reader["PaymentTerm"]);
                            item.Guarantee_Warrantee = (string) DALHelper.HandleDBNull(reader["Guarantee_Warrantee"]);
                            item.Schedule = (long) DALHelper.HandleDBNull(reader["Schedule"]);
                            item.Remarks = (string) DALHelper.HandleDBNull(reader["Remarks"]);
                            item.TotalAmount = (decimal) DALHelper.HandleDBNull(reader["TotalAmount"]);
                            item.TotalDiscount = (decimal) DALHelper.HandleDBNull(reader["TotalDiscount"]);
                            item.TotalVAT = (decimal) DALHelper.HandleDBNull(reader["TotalVat"]);
                            item.TotalNet = (decimal) DALHelper.HandleDBNull(reader["TotalNetAmount"]);
                            item.Freezed = (bool) DALHelper.HandleDBNull(reader["Freezed"]);
                            item.IndentNumber = (string) DALHelper.HandleDBNull(reader["IndentNumber"]);
                            item.DeliveryDays = DALHelper.HandleIntegerNull(reader["DeliveryDays"]);
                            bool flag = (bool) DALHelper.HandleDBNull(reader["POType"]);
                            item.DeliveryLocation = Convert.ToString(DALHelper.HandleDBNull(reader["DeliveryLocation"]));
                            item.IsApproveded = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsApproved"]));
                            item.ApprovedBy = Convert.ToString(DALHelper.HandleDBNull(reader["ApprovedBy"]));
                            item.IsCancelded = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsCanceld"]));
                            if (!flag)
                            {
                                item.Type = "Mannual";
                            }
                            else if (flag)
                            {
                                item.Type = "Auto";
                            }
                            item.GRNNowithDate = (string) DALHelper.HandleDBNull(reader["GRN No - Date"]);
                            if (item.IndentNumber != "")
                            {
                                item.IndentNowithDate = (string) DALHelper.HandleDBNull(reader["Indent No - Date"]);
                            }
                            item.Direct = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["Direct"]));
                            item.POApproveLvlID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApproveLvlID"]));
                            item.ApprovedLvl1Details = Convert.ToString(DALHelper.HandleDBNull(reader["ApprovedLvl1Details"]));
                            item.ApprovedLvl2Details = Convert.ToString(DALHelper.HandleDBNull(reader["ApprovedLvl2Details"]));
                            item.TotalSGST = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalSGST"]));
                            item.TotalCGST = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalCGST"]));
                            item.TotalIGST = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalIGST"]));
                            nvo.PurchaseOrderList.Add(item);
                        }
                    }
                    reader.NextResult();
                    nvo.TotalRowCount = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                }
                else
                {
                    valueObject = this.GetPurchaseOrderGromGRN(valueObject, UserVo);
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

        public override IValueObject GetPurchaseOrderDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            clsGetPurchaseOrderDetailsBizActionVO nvo = null;
            DbDataReader reader = null;
            try
            {
                connection.Open();
                connection.BeginTransaction();
                nvo = valueObject as clsGetPurchaseOrderDetailsBizActionVO;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPurchaseOrderDetails_1");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.SearchID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "FilterPendingQuantity", DbType.Boolean, nvo.FilterPendingQuantity);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PurchaseOrderList == null)
                    {
                        nvo.PurchaseOrderList = new List<clsPurchaseOrderDetailVO>();
                    }
                    while (reader.Read())
                    {
                        clsPurchaseOrderDetailVO item = new clsPurchaseOrderDetailVO {
                            ItemCode = (string) DALHelper.HandleDBNull(reader["ItemCode"]),
                            ItemName = (string) DALHelper.HandleDBNull(reader["ItemName"]),
                            PUM = (string) DALHelper.HandleDBNull(reader["PUM"]),
                            Quantity = (Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])) <= 0f) ? ((decimal) DALHelper.HandleDBNull(reader["Quantity"])) : Convert.ToDecimal((float) (Convert.ToSingle(DALHelper.HandleDBNull(reader["Quantity"])) / Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])))),
                            Rate = (decimal) DALHelper.HandleDBNull(reader["Rate"])
                        };
                        item.CostRate = item.Quantity * item.Rate;
                        item.Amount = (decimal) DALHelper.HandleDBNull(reader["Amount"]);
                        item.DiscountPercent = (decimal) DALHelper.HandleDBNull(reader["DiscountPercent"]);
                        item.DiscountAmount = (decimal) DALHelper.HandleDBNull(reader["DiscountAmount"]);
                        item.VATPercent = (decimal) DALHelper.HandleDBNull(reader["VATPercent"]);
                        item.VATAmount = (decimal) DALHelper.HandleDBNull(reader["VATAmount"]);
                        item.SGSTPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SGSTPercent"]));
                        item.SGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SGSTAmount"]));
                        item.CGSTPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["CGSTPercent"]));
                        item.CGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["CGSTAmount"]));
                        item.IGSTPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IGSTPercent"]));
                        item.IGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IGSTAmount"]));
                        item.ItemVATPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Itemtax"]));
                        item.ItemVATAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemTaxAmount"]));
                        item.POItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Taxtype"]));
                        item.POItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["VatApplicableon"]));
                        item.POItemOtherTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherTaxType"]));
                        item.POItemOtherTaxApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxApplicableon"]));
                        item.POSGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTtype"]));
                        item.POSGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTApplicableOn"]));
                        item.POCGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTtype"]));
                        item.POCGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTApplicableOn"]));
                        item.POIGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTtype"]));
                        item.POIGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTApplicableOn"]));
                        item.HSNCode = Convert.ToString(DALHelper.HandleDBNull(reader["HSNCode"]));
                        item.PrevTotalNet = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PrevTotalNetAmount"]));
                        item.OtherCharges = Convert.ToDecimal(DALHelper.HandleDBNull(reader["OtherCharges"]));
                        item.PODiscount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PODiscount"]));
                        item.ItemExpiredInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemExpiredInDays"]));
                        item.AbatedMRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["AbatedMRP"]));
                        item.SelectedUOM.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                        item.TransUOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransUOM"]));
                        item.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                        item.SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));
                        item.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"]));
                        item.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["STUOM"]));
                        item.BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"]));
                        item.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"]));
                        item.BaseMRP = (Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])) <= 0f) ? Convert.ToSingle(DALHelper.HandleDBNull(reader["MRP"])) : Convert.ToSingle((decimal) (Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"])) / Convert.ToDecimal(DALHelper.HandleDBNull(reader["BaseCF"]))));
                        item.BaseRate = (Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])) <= 0f) ? Convert.ToSingle(DALHelper.HandleDBNull(reader["Rate"])) : Convert.ToSingle((decimal) (Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"])) / Convert.ToDecimal(DALHelper.HandleDBNull(reader["BaseCF"]))));
                        item.BaseQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseQuantity"]));
                        item.Specification = (string) DALHelper.HandleDBNull(reader["Remarks"]);
                        item.DiscPercent = (decimal) DALHelper.HandleDBNull(reader["DiscountPercent"]);
                        item.VATPer = (decimal) DALHelper.HandleDBNull(reader["VATPercent"]);
                        item.BatchesRequired = (bool) DALHelper.HandleDBNull(reader["BatchesRequired"]);
                        item.ItemID = new long?((long) DALHelper.HandleDBNull(reader["ItemID"]));
                        item.PendingQuantity = (decimal) DALHelper.HandleDBNull(reader["PendingQuantity"]);
                        item.PoItemsID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        item.ItemTax = (double) DALHelper.HandleDBNull(reader["PurchaseTax"]);
                        item.MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"]));
                        item.PONO = Convert.ToString(DALHelper.HandleDBNull(reader["PONO"]));
                        item.POID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POID"]));
                        item.PoDetailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        item.PoDetailsUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        item.IndentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentID"]));
                        item.IndentUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentUnitID"]));
                        item.IndentDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentDetailID"]));
                        item.IndentDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentDetailUnitID"]));
                        item.POUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]));
                        item.PODate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["PODate"]));
                        item.PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PurchaseUOM"]));
                        item.StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        item.BarCode = Convert.ToString(DALHelper.HandleDBNull(reader["BarCode"]));
                        item.RateContractCondition = Convert.ToString(DALHelper.HandleDBNull(reader["RCCondition"]));
                        item.RateContractID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RateContractID"]));
                        item.RateContractUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RateContractUnitID"]));
                        item.ItemGroup = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemGroup"]));
                        item.ItemCategory = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"]));
                        item.SelectedCurrency = new MasterListItem();
                        item.SelectedCurrency.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CurrencyID"]));
                        item.PRQTY = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PRQTY"]));
                        item.PRUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PRUOM"]));
                        item.PRPendingQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PRPendingQty"]));
                        item.IndentNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IndentNumber"]));
                        item.PurchaseToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["PurchaseToBaseCF"]));
                        item.StockingToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockingToBaseCF"]));
                        item.NetAmount = (decimal) DALHelper.HandleDBNull(reader["NetAmount"]);
                        item.IsItemBlock = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsItemBlock"]));
                        item.POApprItemQty = Convert.ToSingle(DALHelper.HandleDBNull(reader["POApprItemQty"]));
                        item.POPendingItemQty = Convert.ToSingle(DALHelper.HandleDBNull(reader["POPendingItemQty"]));
                        item.PODetailsViewTimeQty = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseQuantity"]));
                        item.PUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PUID"]));
                        item.TotalBatchAvailableStock = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalBatchAvailableStock"]));
                        nvo.PurchaseOrderList.Add(item);
                    }
                }
                reader.NextResult();
                if (nvo.PoIndentList == null)
                {
                    nvo.PoIndentList = new List<clsPurchaseOrderDetailVO>();
                }
                while (true)
                {
                    if (!reader.Read())
                    {
                        reader.NextResult();
                        if (nvo.POTerms == null)
                        {
                            nvo.POTerms = new List<clsPurchaseOrderTerms>();
                        }
                        while (reader.Read())
                        {
                            clsPurchaseOrderTerms terms = new clsPurchaseOrderTerms {
                                TermsAndConditionID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["TermsAndConditionID"])),
                                Status = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["Status"]))
                            };
                            nvo.POTerms.Add(terms);
                        }
                        break;
                    }
                    clsPurchaseOrderDetailVO item = new clsPurchaseOrderDetailVO {
                        POID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POID"])),
                        POUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POUnitID"])),
                        IndentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentID"])),
                        IndentUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentUnitID"])),
                        IndentDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentDetailID"])),
                        IndentDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentDetailUnitID"])),
                        Quantity = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Quantity"])),
                        ItemID = new long?(Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ItemID"]))),
                        PoDetailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PoDetailsID"])),
                        PoDetailsUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PoDetailsUnitID"])),
                        Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"])),
                        MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"])),
                        SelectedUOM = { ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"])) },
                        BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"])),
                        SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"])),
                        ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"])),
                        StockingQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockingQuantity"])),
                        BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])),
                        BaseQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseQuantity"]))
                    };
                    nvo.PoIndentList.Add(item);
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

        public override IValueObject GetPurchaseOrderDetailsForGRNAgainstPOSearch(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            clsGetPurchaseOrderDetailsBizActionVO nvo = null;
            DbDataReader reader = null;
            try
            {
                connection.Open();
                connection.BeginTransaction();
                nvo = valueObject as clsGetPurchaseOrderDetailsBizActionVO;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPurchaseOrderDetailsForGRNAgainstPOSearch");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.SearchID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "FilterPendingQuantity", DbType.Boolean, nvo.FilterPendingQuantity);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PurchaseOrderList == null)
                    {
                        nvo.PurchaseOrderList = new List<clsPurchaseOrderDetailVO>();
                    }
                    while (reader.Read())
                    {
                        clsPurchaseOrderDetailVO item = new clsPurchaseOrderDetailVO {
                            ItemCode = (string) DALHelper.HandleDBNull(reader["ItemCode"]),
                            ItemName = (string) DALHelper.HandleDBNull(reader["ItemName"]),
                            PUM = (string) DALHelper.HandleDBNull(reader["PUM"]),
                            Quantity = Convert.ToDecimal(DALHelper.HandleDBNull(reader["InputTransactionQuantity"])),
                            Rate = (decimal) DALHelper.HandleDBNull(reader["Rate"]),
                            Amount = (decimal) DALHelper.HandleDBNull(reader["Amount"]),
                            DiscountPercent = (decimal) DALHelper.HandleDBNull(reader["DiscountPercent"]),
                            DiscountAmount = (decimal) DALHelper.HandleDBNull(reader["DiscountAmount"]),
                            VATPercent = (decimal) DALHelper.HandleDBNull(reader["VATPercent"]),
                            VATAmount = (decimal) DALHelper.HandleDBNull(reader["VATAmount"]),
                            ItemVATPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Itemtax"])),
                            ItemVATAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemTaxAmount"])),
                            POItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Taxtype"])),
                            POItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["VatApplicableon"])),
                            POItemOtherTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherTaxType"])),
                            POItemOtherTaxApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxApplicableon"])),
                            PrevTotalNet = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PrevTotalNetAmount"])),
                            OtherCharges = Convert.ToDecimal(DALHelper.HandleDBNull(reader["OtherCharges"])),
                            PODiscount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PODiscount"])),
                            ItemExpiredInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemExpiredInDays"])),
                            AbatedMRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["AbatedMRP"])),
                            SelectedUOM = { ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"])) },
                            TransUOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransUOM"])),
                            BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"])),
                            SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"])),
                            ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"])),
                            SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["STUOM"])),
                            BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"])),
                            BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])),
                            BaseMRP = (Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])) <= 0f) ? Convert.ToSingle(DALHelper.HandleDBNull(reader["MRP"])) : Convert.ToSingle((decimal) (Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"])) / Convert.ToDecimal(DALHelper.HandleDBNull(reader["BaseCF"])))),
                            BaseRate = (Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])) <= 0f) ? Convert.ToSingle(DALHelper.HandleDBNull(reader["Rate"])) : Convert.ToSingle((decimal) (Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"])) / Convert.ToDecimal(DALHelper.HandleDBNull(reader["BaseCF"])))),
                            BaseQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseQuantity"])),
                            Specification = (string) DALHelper.HandleDBNull(reader["Remarks"]),
                            DiscPercent = (decimal) DALHelper.HandleDBNull(reader["DiscountPercent"]),
                            VATPer = (decimal) DALHelper.HandleDBNull(reader["VATPercent"]),
                            BatchesRequired = (bool) DALHelper.HandleDBNull(reader["BatchesRequired"]),
                            ItemID = new long?((long) DALHelper.HandleDBNull(reader["ItemID"])),
                            PendingQuantity = (decimal) DALHelper.HandleDBNull(reader["PendingQuantity"]),
                            PoItemsID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            ItemTax = (double) DALHelper.HandleDBNull(reader["PurchaseTax"]),
                            MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"])),
                            PONO = Convert.ToString(DALHelper.HandleDBNull(reader["PONO"])),
                            POID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POID"])),
                            PoDetailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            PoDetailsUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            POUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"])),
                            PODate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["PODate"])),
                            PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PurchaseUOM"])),
                            StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"])),
                            BarCode = Convert.ToString(DALHelper.HandleDBNull(reader["BarCode"])),
                            RateContractCondition = Convert.ToString(DALHelper.HandleDBNull(reader["RCCondition"])),
                            RateContractID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RateContractID"])),
                            RateContractUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RateContractUnitID"])),
                            ItemGroup = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemGroup"])),
                            ItemCategory = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"])),
                            SelectedCurrency = new MasterListItem()
                        };
                        item.SelectedCurrency.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CurrencyID"]));
                        item.NetAmount = (decimal) DALHelper.HandleDBNull(reader["NetAmount"]);
                        item.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"]));
                        item.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"]));
                        item.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["Container"]));
                        item.GRNApprItemQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["GRNApprovedItemQty"]));
                        item.GRNPendItemQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["GRNPendingItemQty"]));
                        item.IsItemBlock = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsItemBlock"]));
                        item.SGSTPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SGSTPercent"]));
                        item.SGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SGSTAmount"]));
                        item.CGSTPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["CGSTPercent"]));
                        item.CGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["CGSTAmount"]));
                        item.IGSTPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IGSTPercent"]));
                        item.IGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IGSTAmount"]));
                        item.GRNSGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTType"]));
                        item.GRNSGSTVatApplicableOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTApplicableOn"]));
                        item.GRNCGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTType"]));
                        item.GRNCGSTVatApplicableOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTApplicableOn"]));
                        item.GRNIGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTType"]));
                        item.GRNIGSTVatApplicableOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTApplicableOn"]));
                        nvo.PurchaseOrderList.Add(item);
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

        public IValueObject GetPurchaseOrderGromGRN(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            clsGetPurchaseOrderBizActionVO nvo = null;
            DbDataReader reader = null;
            try
            {
                connection.Open();
                connection.BeginTransaction();
                nvo = valueObject as clsGetPurchaseOrderBizActionVO;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPurchaseOrderFromGRN");
                this.dbServer.AddInParameter(storedProcCommand, "FromGRN", DbType.Boolean, nvo.flagPOFromGRN);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierID", DbType.Int64, nvo.SearchSupplierID);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.SearchStoreID);
                this.dbServer.AddInParameter(storedProcCommand, "PONO", DbType.String, nvo.PONO);
                this.dbServer.AddInParameter(storedProcCommand, "DeliverydStoreID", DbType.Int64, nvo.SearchDeliveryStoreID);
                if (nvo.searchFromDate != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.searchFromDate);
                }
                if (nvo.searchToDate != null)
                {
                    if ((nvo.searchFromDate != null) && nvo.searchFromDate.Equals(nvo.searchToDate))
                    {
                        nvo.searchToDate = new DateTime?(nvo.searchToDate.Value.Date.AddDays(1.0));
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.searchToDate);
                }
                this.dbServer.AddInParameter(storedProcCommand, "Freezed", DbType.Boolean, nvo.Freezed);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PurchaseOrderList == null)
                    {
                        nvo.PurchaseOrderList = new List<clsPurchaseOrderVO>();
                    }
                    while (reader.Read())
                    {
                        clsPurchaseOrderVO item = new clsPurchaseOrderVO();
                        if ((bool) DALHelper.HandleDBNull(reader["Status"]))
                        {
                            item.PONO = (string) DALHelper.HandleDBNull(reader["PONO"]);
                            item.Date = (DateTime) DALHelper.HandleDBNull(reader["Date"]);
                            item.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                            item.UnitId = (long) DALHelper.HandleDBNull(reader["UnitID"]);
                            item.StoreName = (string) DALHelper.HandleDBNull(reader["StoreName"]);
                            item.SupplierName = (string) DALHelper.HandleDBNull(reader["SupplierName"]);
                            item.StoreID = (long) DALHelper.HandleDBNull(reader["StoreID"]);
                            item.SupplierID = (long) DALHelper.HandleDBNull(reader["SupplierID"]);
                            item.IndentID = (long) DALHelper.HandleDBNull(reader["IndentID"]);
                            item.EnquiryID = (long) DALHelper.HandleDBNull(reader["EnquiryID"]);
                            item.DeliveryDuration = (long) DALHelper.HandleDBNull(reader["DeliveryDuration"]);
                            item.PaymentMode = (long) DALHelper.HandleDBNull(reader["PaymentMode"]);
                            item.PaymentTerms = (long) DALHelper.HandleDBNull(reader["PaymentTerm"]);
                            item.Guarantee_Warrantee = (string) DALHelper.HandleDBNull(reader["Guarantee_Warrantee"]);
                            item.Schedule = (long) DALHelper.HandleDBNull(reader["Schedule"]);
                            item.Remarks = (string) DALHelper.HandleDBNull(reader["Remarks"]);
                            item.TotalAmount = (decimal) DALHelper.HandleDBNull(reader["TotalAmount"]);
                            item.TotalDiscount = (decimal) DALHelper.HandleDBNull(reader["TotalDiscount"]);
                            item.TotalVAT = (decimal) DALHelper.HandleDBNull(reader["TotalVat"]);
                            item.TotalNet = (decimal) DALHelper.HandleDBNull(reader["TotalNetAmount"]);
                            item.Freezed = (bool) DALHelper.HandleDBNull(reader["Freezed"]);
                            item.IsApproveded = (bool) DALHelper.HandleDBNull(reader["IsApproved"]);
                            item.TotalSGST = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalSGST"]));
                            item.TotalCGST = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalCGST"]));
                            item.TotalIGST = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalIGST"]));
                            nvo.PurchaseOrderList.Add(item);
                        }
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
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }

        public override IValueObject GetPurchaseOrderToCloseManually(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            clsGetPurchaseOrderForCloseBizActionVO nvo = null;
            DbDataReader reader = null;
            try
            {
                connection.Open();
                connection.BeginTransaction();
                nvo = valueObject as clsGetPurchaseOrderForCloseBizActionVO;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPurchaseOrderForCloseManually");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierID", DbType.Int64, nvo.SearchSupplierID);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.SearchStoreID);
                this.dbServer.AddInParameter(storedProcCommand, "PONO", DbType.String, nvo.PONO);
                if (nvo.searchFromDate != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.searchFromDate);
                }
                if (nvo.searchToDate != null)
                {
                    nvo.searchToDate = new DateTime?(nvo.searchToDate.Value.Date.AddDays(1.0));
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.searchToDate);
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PurchaseOrderList == null)
                    {
                        nvo.PurchaseOrderList = new List<clsPurchaseOrderVO>();
                    }
                    while (reader.Read())
                    {
                        clsPurchaseOrderVO item = new clsPurchaseOrderVO {
                            PONO = Convert.ToString(reader["PONO"]),
                            Date = (DateTime) DALHelper.HandleDBNull(reader["Date"]),
                            ID = Convert.ToInt64(reader["ID"]),
                            UnitId = Convert.ToInt64(reader["UnitID"]),
                            StoreName = Convert.ToString(reader["StoreName"]),
                            SupplierName = Convert.ToString(reader["SupplierName"]),
                            StoreID = Convert.ToInt64(reader["StoreID"]),
                            SupplierID = Convert.ToInt64(reader["SupplierID"]),
                            TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                            TotalDiscount = Convert.ToDecimal(reader["TotalDiscount"]),
                            TotalVAT = Convert.ToDecimal(reader["TotalVat"]),
                            TotalNet = Convert.ToDecimal(reader["TotalNetAmount"])
                        };
                        bool flag = Convert.ToBoolean(reader["POType"]);
                        item.DeliveryLocation = Convert.ToString(DALHelper.HandleDBNull(reader["DeliveryLocation"]));
                        if (!flag)
                        {
                            item.Type = "Mannual";
                        }
                        else if (flag)
                        {
                            item.Type = "Auto";
                        }
                        item.GRNNowithDate = Convert.ToString(reader["GRN No - Date"]);
                        item.IndentNowithDate = Convert.ToString(reader["Indent No - Date"]);
                        nvo.PurchaseOrderList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRowCount = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
            }
            catch (Exception exception1)
            {
                throw exception1;
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

        public override IValueObject GetRateContract(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRateContractBizActionVO nvo = valueObject as clsGetRateContractBizActionVO;
            try
            {
                DbDataReader reader = null;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetRateContarctList");
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, nvo.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.Description);
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierID", DbType.Int64, nvo.SupplierID);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.RateContract == null)
                    {
                        nvo.RateContract = new List<clsRateContractVO>();
                    }
                    while (reader.Read())
                    {
                        clsRateContractVO item = new clsRateContractVO {
                            ID = DALHelper.HandleIntegerNull(reader["ID"]),
                            UnitId = DALHelper.HandleIntegerNull(reader["UnitId"]),
                            Code = (string) DALHelper.HandleDBNull(reader["Code"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"])
                        };
                        DateTime? nullable = DALHelper.HandleDate(reader["ContractDate"]);
                        item.ContractDate = new DateTime?(nullable.Value);
                        item.ContractValue = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ContractValue"]));
                        item.SupplierID = DALHelper.HandleIntegerNull(reader["SupplierID"]);
                        item.FromDate = DALHelper.HandleDate(reader["FromDate"]);
                        item.ToDate = DALHelper.HandleDate(reader["ToDate"]);
                        item.Supplier = (string) DALHelper.HandleDBNull(reader["Supplier"]);
                        item.SupplierRepresentative = (string) DALHelper.HandleDBNull(reader["SupplierRepresentative"]);
                        item.ClinicRepresentativeID = DALHelper.HandleIntegerNull(reader["ClinicRepresentativeID"]);
                        item.Remark = (string) DALHelper.HandleDBNull(reader["Remark"]);
                        item.IsFreeze = (bool) DALHelper.HandleBoolDBNull(reader["IsFreeze"]);
                        item.ReasonForEdit = Convert.ToString(DALHelper.HandleDBNull(reader["ReasonForEdit"]));
                        nvo.RateContract.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRowCount = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
            }
            catch
            {
            }
            return nvo;
        }

        public override IValueObject GetRateContractItemDetail(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRateContractItemDetailsBizActionVO nvo = valueObject as clsGetRateContractItemDetailsBizActionVO;
            try
            {
                DbDataReader reader = null;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetRateContractItemDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ContractID", DbType.Int64, nvo.ContractID);
                this.dbServer.AddInParameter(storedProcCommand, "ContractUnitId", DbType.Int64, nvo.ContractUnitId);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.RateContractList == null)
                    {
                        nvo.RateContractList = new List<clsRateContractDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsRateContractDetailsVO svo = new clsRateContractDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"])),
                            ContractID = DALHelper.HandleIntegerNull(reader["ContractID"]),
                            ContractUnitId = DALHelper.HandleIntegerNull(reader["ContractUnitId"]),
                            ItemID = DALHelper.HandleIntegerNull(reader["ItemID"]),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"])),
                            Quantity = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Quantity"])),
                            Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"])),
                            TransUOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransactionUOM"]))
                        };
                        MasterListItem item = new MasterListItem {
                            ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["TransactionUOMID"])),
                            Description = svo.TransUOM
                        };
                        svo.SelectedUOM = item;
                        svo.ConversionFactor = Convert.ToSingle((double) DALHelper.HandleDBNull(reader["CoversionFactor"]));
                        svo.SUOMID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["StockUOMID"]));
                        svo.BaseConversionFactor = Convert.ToSingle((double) DALHelper.HandleDBNull(reader["BaseCF"]));
                        svo.BaseUOMID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["BaseUMID"]));
                        svo.BaseRate = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseRate"]));
                        svo.BaseMRP = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseMRP"]));
                        svo.MainRate = svo.BaseRate;
                        svo.MainMRP = svo.BaseMRP;
                        svo.DiscountPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DiscountPercent"]));
                        svo.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"]));
                        svo.HSNCode = Convert.ToString(DALHelper.HandleDBNull(reader["HSNCode"]));
                        svo.UnlimitedQuantity = Convert.ToBoolean(DALHelper.HandleDBNull(reader["UnlimitedQuantity"]));
                        svo.Condition = Convert.ToString(DALHelper.HandleDBNull(reader["Condition"]));
                        svo.MinQuantity = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MinQuantity"]));
                        svo.MaxQuantity = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MaxQuantity"]));
                        svo.SelectedCondition.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Condition"]));
                        if (svo.SelectedCondition.Description == "=")
                        {
                            svo.SelectedCondition.ID = 1L;
                        }
                        else if (svo.SelectedCondition.Description == "<")
                        {
                            svo.SelectedCondition.ID = 2L;
                        }
                        else if (svo.SelectedCondition.Description == "Between")
                        {
                            svo.SelectedCondition.ID = 3L;
                        }
                        else if (svo.SelectedCondition.Description == ">")
                        {
                            svo.SelectedCondition.ID = 4L;
                        }
                        else if (svo.SelectedCondition.Description.Equals("No Limit"))
                        {
                            svo.SelectedCondition.ID = 5L;
                        }
                        svo.ReasonForEdit = Convert.ToString(DALHelper.HandleDBNull(reader["ReasonForEdit"]));
                        nvo.RateContractList.Add(svo);
                    }
                }
            }
            catch
            {
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

        private void SetLogInfo(Guid ActivityId, long UserId, DateTime TimeStamp, string ClassName, string MethodName, string Message)
        {
            try
            {
                this.logManager.LogInfo(ActivityId, UserId, TimeStamp, ClassName, MethodName, Message);
            }
            catch (Exception exception)
            {
                this.logManager.LogError(UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
            }
        }

        private clsAddRateContractBizActionVO UpdateDetails(clsAddRateContractBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                clsRateContractVO rateContract = BizActionObj.RateContract;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateRateContract");
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, rateContract.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, rateContract.Description);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierID", DbType.Int64, rateContract.SupplierID);
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, rateContract.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, rateContract.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "ContractDate ", DbType.DateTime, rateContract.ContractDate);
                this.dbServer.AddInParameter(storedProcCommand, "ContractValue", DbType.Decimal, rateContract.ContractValue);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreeze", DbType.Boolean, rateContract.IsFreeze);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierRepresentative", DbType.String, rateContract.SupplierRepresentative);
                this.dbServer.AddInParameter(storedProcCommand, "ClinicRepresentativeID", DbType.Int64, rateContract.ClinicRepresentativeID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, rateContract.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, rateContract.Remark);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, rateContract.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "ReasonForEdit", DbType.String, rateContract.ReasonForEdit);
                this.dbServer.AddInParameter(storedProcCommand, "IsEditAfterFreeze", DbType.Boolean, rateContract.IsEditAfterFreeze);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, rateContract.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
                BizActionObj.RateContract.ID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                if (!rateContract.IsEditAfterFreeze && ((BizActionObj.SuccessStatus == 1) && ((rateContract.ContractDetails != null) && (rateContract.ContractDetails.Count > 0))))
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteRateContaractDetails");
                    this.dbServer.AddInParameter(command2, "ContractID", DbType.Int64, rateContract.ID);
                    this.dbServer.AddInParameter(command2, "ContractUnitId", DbType.Int64, rateContract.UnitId);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                }
                if ((BizActionObj.SuccessStatus == 1) && ((rateContract.ContractDetails != null) && (rateContract.ContractDetails.Count > 0)))
                {
                    foreach (clsRateContractDetailsVO svo in rateContract.ContractDetails)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddRateContractItemDetails");
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, rateContract.UnitId);
                        this.dbServer.AddInParameter(command3, "ContractID", DbType.Int64, rateContract.ID);
                        this.dbServer.AddInParameter(command3, "ContractUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "ItemID", DbType.Int64, svo.ItemID);
                        this.dbServer.AddInParameter(command3, "Rate", DbType.Decimal, svo.Rate);
                        this.dbServer.AddInParameter(command3, "MRP", DbType.Decimal, svo.MRP);
                        this.dbServer.AddInParameter(command3, "TotalRate", DbType.Decimal, svo.CostRate);
                        this.dbServer.AddInParameter(command3, "TotalMRP", DbType.Decimal, svo.Amount);
                        this.dbServer.AddInParameter(command3, "TransactionUOMID", DbType.Int64, svo.SelectedUOM.ID);
                        this.dbServer.AddInParameter(command3, "CoversionFactor", DbType.Single, svo.ConversionFactor);
                        this.dbServer.AddInParameter(command3, "StockingQuantity", DbType.Decimal, svo.Quantity * Convert.ToDecimal(svo.ConversionFactor));
                        this.dbServer.AddInParameter(command3, "StockUOMID", DbType.Int64, svo.SUOMID);
                        this.dbServer.AddInParameter(command3, "BaseCF", DbType.Single, svo.BaseConversionFactor);
                        this.dbServer.AddInParameter(command3, "BaseQuantity", DbType.Decimal, svo.Quantity * Convert.ToDecimal(svo.BaseConversionFactor));
                        this.dbServer.AddInParameter(command3, "BaseUMID", DbType.Int64, svo.BaseUOMID);
                        this.dbServer.AddInParameter(command3, "BaseRate", DbType.Decimal, svo.BaseRate);
                        this.dbServer.AddInParameter(command3, "BaseMRP", DbType.Decimal, svo.BaseMRP);
                        this.dbServer.AddInParameter(command3, "DiscountPercent", DbType.Decimal, svo.DiscountPercent);
                        this.dbServer.AddInParameter(command3, "DiscountAmount", DbType.Decimal, svo.DiscountAmount);
                        this.dbServer.AddInParameter(command3, "NetAmount", DbType.Decimal, svo.NetAmount);
                        this.dbServer.AddInParameter(command3, "Remarks", DbType.String, svo.Remarks);
                        this.dbServer.AddInParameter(command3, "Quantity", DbType.Decimal, svo.Quantity);
                        this.dbServer.AddInParameter(command3, "UnlimitedQuantity", DbType.Boolean, svo.UnlimitedQuantity);
                        if (svo.SelectedCondition != null)
                        {
                            this.dbServer.AddInParameter(command3, "Condition", DbType.String, svo.SelectedCondition.Description);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "Condition", DbType.String, null);
                        }
                        this.dbServer.AddInParameter(command3, "MinQuantity", DbType.Decimal, svo.MinQuantity);
                        this.dbServer.AddInParameter(command3, "MaxQuantity", DbType.Decimal, svo.MaxQuantity);
                        this.dbServer.AddInParameter(command3, "ReasonForEdit", DbType.String, svo.ReasonForEdit);
                        this.dbServer.AddInParameter(command3, "IsEditAfterFreeze", DbType.Boolean, rateContract.IsEditAfterFreeze);
                        this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.RateContract = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return BizActionObj;
        }

        public override IValueObject UpdateForPOCloseDuration(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            clsUpdateRemarkForCancellationPO npo = null;
            DbTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                npo = valueObject as clsUpdateRemarkForCancellationPO;
                clsPurchaseOrderVO purchaseOrder = npo.PurchaseOrder;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ChangePOCloseDuration");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "POAutoCloseDuration", DbType.Int32, npo.PurchaseOrder.POAutoCloseDuration);
                this.dbServer.AddInParameter(storedProcCommand, "POAutoCloseDate", DbType.DateTime, npo.PurchaseOrder.POAutoCloseDurationDate);
                this.dbServer.AddInParameter(storedProcCommand, "POAutoCloseReason", DbType.String, npo.PurchaseOrder.POAutoCloseReason);
                this.dbServer.AddInParameter(storedProcCommand, "PONO", DbType.String, npo.PurchaseOrder.PONO);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, npo.PurchaseOrder.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                npo.SuccessStatus = -1;
                npo.PurchaseOrder = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return npo;
        }

        public override IValueObject UpdatePurchaseOrderForApproval(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            clsUpdatePurchaseOrderForApproval approval = null;
            clsPurchaseOrderVO purchaseOrder = null;
            DbTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                approval = valueObject as clsUpdatePurchaseOrderForApproval;
                purchaseOrder = approval.PurchaseOrder;
                foreach (long num in approval.PurchaseOrder.ids)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePurchaseOrderApprovalStatus");
                    storedProcCommand.Connection = connection;
                    this.dbServer.AddInParameter(storedProcCommand, "POApproveLvlID", DbType.Int64, approval.PurchaseOrder.POApproveLvlID);
                    this.dbServer.AddInParameter(storedProcCommand, "ApprovedBy", DbType.String, approval.PurchaseOrder.ApprovedBy);
                    this.dbServer.AddInParameter(storedProcCommand, "ApprovedByID", DbType.Int64, approval.PurchaseOrder.ApprovedByID);
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, num);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, approval.PurchaseOrder.UnitId);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                }
                if ((approval.PurchaseOrder.POApproveLvlID == 2L) && ((purchaseOrder.Items != null) && (purchaseOrder.Items.Count > 0)))
                {
                    foreach (clsPurchaseOrderDetailVO lvo in purchaseOrder.Items)
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateIndetFromPO");
                        storedProcCommand.Connection = connection;
                        this.dbServer.AddInParameter(storedProcCommand, "POID", DbType.Int64, lvo.POID);
                        this.dbServer.AddInParameter(storedProcCommand, "POUnitID", DbType.Decimal, lvo.POUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "PoDetailsID", DbType.Int64, lvo.PoDetailsID);
                        this.dbServer.AddInParameter(storedProcCommand, "PoDetailsUnitID", DbType.Decimal, lvo.PoDetailsUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "IndentID", DbType.Decimal, lvo.IndentID);
                        this.dbServer.AddInParameter(storedProcCommand, "IndentUnitID", DbType.Decimal, lvo.IndentUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "IndentDetailID", DbType.Decimal, lvo.IndentDetailID);
                        this.dbServer.AddInParameter(storedProcCommand, "IndentDetailUnitID", DbType.Decimal, lvo.IndentDetailUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "BalanceQty", DbType.Decimal, lvo.Quantity * Convert.ToDecimal(lvo.BaseConversionFactor));
                        this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, lvo.ItemID);
                        this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    }
                }
                if ((approval.PurchaseOrder.POApproveLvlID == 2L) && ((approval.POIndentList != null) && (approval.POIndentList.Count > 0)))
                {
                    foreach (clsPurchaseOrderDetailVO lvo2 in approval.POIndentList)
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateIndetFromPO");
                        storedProcCommand.Connection = connection;
                        this.dbServer.AddInParameter(storedProcCommand, "POID", DbType.Int64, lvo2.POID);
                        this.dbServer.AddInParameter(storedProcCommand, "POUnitID", DbType.Decimal, purchaseOrder.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "PoDetailsID", DbType.Int64, lvo2.PoDetailsID);
                        this.dbServer.AddInParameter(storedProcCommand, "PoDetailsUnitID", DbType.Decimal, lvo2.PoDetailsUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "IndentID", DbType.Decimal, lvo2.IndentID);
                        this.dbServer.AddInParameter(storedProcCommand, "IndentUnitID", DbType.Decimal, lvo2.IndentUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "IndentDetailID", DbType.Decimal, lvo2.IndentDetailID);
                        this.dbServer.AddInParameter(storedProcCommand, "IndentDetailUnitID", DbType.Decimal, lvo2.IndentDetailUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "BalanceQty", DbType.Decimal, lvo2.Quantity * Convert.ToDecimal(lvo2.BaseConversionFactor));
                        this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, lvo2.ItemID);
                        this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    }
                }
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                approval.SuccessStatus = -1;
                approval.PurchaseOrder = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return approval;
        }

        public override IValueObject UpdateRemarkForCancellationPO(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            clsUpdateRemarkForCancellationPO npo = null;
            DbTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                npo = valueObject as clsUpdateRemarkForCancellationPO;
                clsPurchaseOrderVO purchaseOrder = npo.PurchaseOrder;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateRemarkForCancellationPO");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "CancellationRemark", DbType.String, npo.PurchaseOrder.CancellationRemark);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, npo.PurchaseOrder.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, npo.PurchaseOrder.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CancelledByID", DbType.Int64, UserVo.ID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                npo.SuccessStatus = -1;
                npo.PurchaseOrder = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return npo;
        }
    }
}

