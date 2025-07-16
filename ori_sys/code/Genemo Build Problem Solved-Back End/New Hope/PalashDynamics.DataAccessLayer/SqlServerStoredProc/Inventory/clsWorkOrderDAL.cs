namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Inventory
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Inventory.PurchaseOrder;
    using PalashDynamics.ValueObjects.Inventory.WorkOrder;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Reflection;
    using System.Windows;

    internal class clsWorkOrderDAL : clsBaseWorkOrderDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsWorkOrderDAL()
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

        public override IValueObject AddWorkOrder(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            clsAddWorkOrderBizActionVO nvo = null;
            clsWorkOrderVO purchaseOrder = null;
            DbTransaction transaction = null;
            try
            {
                DbCommand command;
                connection.Open();
                transaction = connection.BeginTransaction();
                nvo = valueObject as clsAddWorkOrderBizActionVO;
                purchaseOrder = nvo.PurchaseOrder;
                if (nvo.IsEditMode)
                {
                    command = this.dbServer.GetStoredProcCommand("CIMS_UpdateWorkOrder");
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
                }
                else
                {
                    command = this.dbServer.GetStoredProcCommand("CIMS_AddWorkOrder");
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
                    this.dbServer.AddInParameter(command, "WONO", DbType.String, purchaseOrder.WONO);
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
                this.dbServer.AddInParameter(command, "DeliveryDays", DbType.Int64, purchaseOrder.DeliveryDays);
                this.dbServer.AddInParameter(command, "PaymentMode", DbType.Int16, (short) purchaseOrder.PaymentModeID);
                this.dbServer.AddInParameter(command, "PaymentTerm", DbType.Int64, purchaseOrder.PaymentTerms);
                this.dbServer.AddInParameter(command, "Guarantee_Warrantee", DbType.String, purchaseOrder.Guarantee_Warrantee);
                this.dbServer.AddInParameter(command, "Schedule", DbType.Int64, purchaseOrder.Schedule);
                this.dbServer.AddInParameter(command, "Remarks", DbType.String, purchaseOrder.Remarks);
                this.dbServer.AddInParameter(command, "TotalDiscount", DbType.Decimal, purchaseOrder.TotalDiscount);
                this.dbServer.AddInParameter(command, "TotalVAT", DbType.Decimal, purchaseOrder.TotalVAT);
                this.dbServer.AddInParameter(command, "TotalAmount", DbType.Decimal, purchaseOrder.TotalAmount);
                this.dbServer.AddInParameter(command, "TotalNetAmount", DbType.Decimal, purchaseOrder.TotalNet);
                this.dbServer.AddInParameter(command, "UnitId", DbType.Int64, purchaseOrder.UnitId);
                this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddParameter(command, "WorkOrderNumber", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "000000000000000000000000000000000000000000000000000");
                this.dbServer.ExecuteNonQuery(command, transaction);
                if (!nvo.IsEditMode)
                {
                    nvo.PurchaseOrder.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                }
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                nvo.PurchaseOrder.WONO = (string) this.dbServer.GetParameterValue(command, "WorkOrderNumber");
                if (nvo.IsEditMode)
                {
                    GetInstance().DeleteWorkOrderItems(nvo, UserVo);
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddWorkOrderDetails");
                storedProcCommand.Connection = connection;
                foreach (clsWorkOrderDetailVO lvo in nvo.PurchaseOrder.Items)
                {
                    storedProcCommand.Parameters.Clear();
                    lvo.WOID = nvo.PurchaseOrder.ID;
                    this.dbServer.AddInParameter(command, "LinkServer", DbType.String, purchaseOrder.LinkServer);
                    if (purchaseOrder.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, purchaseOrder.LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, purchaseOrder.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, purchaseOrder.Date);
                    this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, purchaseOrder.Time);
                    this.dbServer.AddInParameter(storedProcCommand, "WOID", DbType.Int64, lvo.WOID);
                    this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, lvo.ItemID);
                    this.dbServer.AddInParameter(storedProcCommand, "Quantity", DbType.Decimal, lvo.Quantity * Convert.ToDecimal(lvo.ConversionFactor));
                    this.dbServer.AddInParameter(storedProcCommand, "Rate", DbType.Decimal, lvo.Rate / Convert.ToDecimal(lvo.ConversionFactor));
                    this.dbServer.AddInParameter(storedProcCommand, "Amount", DbType.Decimal, lvo.Amount);
                    this.dbServer.AddInParameter(storedProcCommand, "DiscountPercent", DbType.Decimal, lvo.DiscountPercent);
                    this.dbServer.AddInParameter(storedProcCommand, "DiscountAmount", DbType.Decimal, lvo.DiscountAmount);
                    this.dbServer.AddInParameter(storedProcCommand, "VatPercent", DbType.Decimal, lvo.VATPercent);
                    this.dbServer.AddInParameter(storedProcCommand, "VATAmount", DbType.Decimal, lvo.VATAmount);
                    this.dbServer.AddInParameter(storedProcCommand, "NetAmount", DbType.Decimal, lvo.NetAmount);
                    this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, lvo.Specification);
                    this.dbServer.AddInParameter(storedProcCommand, "MRP", DbType.Decimal, lvo.MRP / Convert.ToDecimal(lvo.ConversionFactor));
                    if (lvo.SelectedCurrency != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "CurrencyID", DbType.Int64, lvo.SelectedCurrency.ID);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "CurrencyID", DbType.Int64, 0);
                    }
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                }
                if (nvo.POTerms != null)
                {
                    foreach (clsWorkOrderTerms terms in nvo.POTerms)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddWOTermsAndCondition");
                        command3.Connection = connection;
                        command3.Parameters.Clear();
                        this.dbServer.AddInParameter(command3, "POID", DbType.Int64, nvo.PurchaseOrder.ID);
                        this.dbServer.AddInParameter(command3, "POUnitID", DbType.Int64, purchaseOrder.UnitId);
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Decimal, purchaseOrder.UnitId);
                        this.dbServer.AddInParameter(command3, "TermsAndConditionID", DbType.Int64, terms.TermsAndConditionID);
                        this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, terms.Status);
                        this.dbServer.AddOutParameter(command3, "ID", DbType.Int64, 0);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                    }
                }
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                nvo.SuccessStatus = -1;
                nvo.PurchaseOrder = null;
                MessageBox.Show("Error occurred :" + exception.Message);
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject CancelWorkOrder(IValueObject valueObject, clsUserVO UserVo)
        {
            IValueObject obj2;
            try
            {
                clsCancelWorkOrderBizActionVO nvo = null;
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CancelWO");
                nvo = valueObject as clsCancelWorkOrderBizActionVO;
                this.dbServer.AddInParameter(storedProcCommand, "POID", DbType.Int64, nvo.WorkOrder.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.WorkOrder.UnitId);
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

        public override IValueObject DeleteWorkOrderItems(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            clsAddWorkOrderBizActionVO nvo = null;
            clsWorkOrderVO purchaseOrder = null;
            DbTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteWorkOrderDetails");
                nvo = valueObject as clsAddWorkOrderBizActionVO;
                purchaseOrder = nvo.PurchaseOrder;
                this.dbServer.AddInParameter(storedProcCommand, "WOID", DbType.Int64, purchaseOrder.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, purchaseOrder.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                nvo.SuccessStatus = -1;
                nvo.PurchaseOrderList = null;
            }
            return nvo;
        }

        public override IValueObject FreezWorkOrder(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            clsFreezWorkOrderBizActionVO nvo = null;
            clsWorkOrderVO purchaseOrder = null;
            DbTransaction transaction = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FreezWorkOrder");
                nvo = valueObject as clsFreezWorkOrderBizActionVO;
                purchaseOrder = nvo.PurchaseOrder;
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, purchaseOrder.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, purchaseOrder.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                nvo.SuccessStatus = -1;
                nvo.PurchaseOrderList = null;
                connection.Close();
            }
            finally
            {
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject GetWorkOrder(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            clsGetWorkOrderBizActionVO nvo = null;
            DbDataReader reader = null;
            try
            {
                connection.Open();
                connection.BeginTransaction();
                nvo = valueObject as clsGetWorkOrderBizActionVO;
                if (!nvo.flagWOFromGRN)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetWorkOrder");
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "SupplierID", DbType.Int64, nvo.SearchSupplierID);
                    this.dbServer.AddInParameter(storedProcCommand, "CancelWO", DbType.Boolean, nvo.CancelWO);
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
                    this.dbServer.AddInParameter(storedProcCommand, "WONO", DbType.String, nvo.WONO);
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.WorkOrderList == null)
                        {
                            nvo.WorkOrderList = new List<clsWorkOrderVO>();
                        }
                        while (reader.Read())
                        {
                            clsWorkOrderVO item = new clsWorkOrderVO();
                            bool flag1 = (bool) DALHelper.HandleDBNull(reader["Status"]);
                            item.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                            item.WONO = (string) DALHelper.HandleDBNull(reader["WONO"]);
                            item.Date = (DateTime) DALHelper.HandleDBNull(reader["Date"]);
                            item.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                            item.UnitId = (long) DALHelper.HandleDBNull(reader["UnitID"]);
                            item.StoreName = (string) DALHelper.HandleDBNull(reader["StoreName"]);
                            item.SupplierName = (string) DALHelper.HandleDBNull(reader["SupplierName"]);
                            item.StoreID = (long) DALHelper.HandleDBNull(reader["StoreID"]);
                            item.SupplierID = (long) DALHelper.HandleDBNull(reader["SupplierID"]);
                            item.DeliveryDuration = Convert.ToInt64(DALHelper.HandleDBNull(reader["DeliveryDuration"]));
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
                            item.DeliveryLocation = Convert.ToString(DALHelper.HandleDBNull(reader["DeliveryLocation"]));
                            item.IsApproveded = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsApproved"]));
                            item.ApprovedBy = Convert.ToString(DALHelper.HandleDBNull(reader["ApprovedBy"]));
                            nvo.WorkOrderList.Add(item);
                        }
                    }
                    reader.NextResult();
                    nvo.TotalRowCount = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                }
                else
                {
                    valueObject = this.GetWorkOrderGromGRN(valueObject, UserVo);
                    return valueObject;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetWorkOrderDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            clsGetWorkOrderDetailsBizActionVO nvo = null;
            DbDataReader reader = null;
            try
            {
                connection.Open();
                connection.BeginTransaction();
                nvo = valueObject as clsGetWorkOrderDetailsBizActionVO;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetWorkOrderDetails_1");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.SearchID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "FilterPendingQuantity", DbType.Boolean, nvo.FilterPendingQuantity);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PurchaseOrderList == null)
                    {
                        nvo.PurchaseOrderList = new List<clsWorkOrderDetailVO>();
                    }
                    while (reader.Read())
                    {
                        clsWorkOrderDetailVO item = new clsWorkOrderDetailVO {
                            ItemCode = (string) DALHelper.HandleDBNull(reader["ItemCode"]),
                            ItemName = (string) DALHelper.HandleDBNull(reader["ItemName"]),
                            Quantity = (decimal) DALHelper.HandleDBNull(reader["Quantity"]),
                            Rate = (decimal) DALHelper.HandleDBNull(reader["Rate"]),
                            Amount = (decimal) DALHelper.HandleDBNull(reader["Amount"]),
                            DiscountPercent = (decimal) DALHelper.HandleDBNull(reader["DiscountPercent"]),
                            DiscountAmount = (decimal) DALHelper.HandleDBNull(reader["DiscountAmount"]),
                            VATPercent = (decimal) DALHelper.HandleDBNull(reader["VATPercent"]),
                            VATAmount = (decimal) DALHelper.HandleDBNull(reader["VATAmount"]),
                            NetAmount = (decimal) DALHelper.HandleDBNull(reader["NetAmount"]),
                            Specification = (string) DALHelper.HandleDBNull(reader["Remarks"]),
                            DiscPercent = (decimal) DALHelper.HandleDBNull(reader["DiscountPercent"]),
                            VATPer = (decimal) DALHelper.HandleDBNull(reader["VATPercent"]),
                            ItemID = new long?((long) DALHelper.HandleDBNull(reader["ItemID"])),
                            PendingQuantity = (decimal) DALHelper.HandleDBNull(reader["PendingQuantity"]),
                            WoItemsID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            WONO = Convert.ToString(DALHelper.HandleDBNull(reader["WONO"])),
                            WOID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WOID"])),
                            WOUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"])),
                            WODate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["PODate"]))
                        };
                        nvo.PurchaseOrderList.Add(item);
                    }
                }
                reader.NextResult();
                if (nvo.PoIndentList == null)
                {
                    nvo.PoIndentList = new List<clsWorkOrderDetailVO>();
                }
                while (true)
                {
                    if (!reader.Read())
                    {
                        reader.NextResult();
                        if (nvo.POTerms == null)
                        {
                            nvo.POTerms = new List<clsWorkOrderTerms>();
                        }
                        while (reader.Read())
                        {
                            clsWorkOrderTerms terms = new clsWorkOrderTerms {
                                TermsAndConditionID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["TermsAndConditionID"])),
                                Status = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["Status"]))
                            };
                            nvo.POTerms.Add(terms);
                        }
                        break;
                    }
                    clsWorkOrderDetailVO item = new clsWorkOrderDetailVO {
                        WOID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WOID"])),
                        WOUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WOUnitID"])),
                        IndentUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentUnitID"])),
                        Quantity = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Quantity"])),
                        ItemID = new long?(Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ItemID"])))
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

        public IValueObject GetWorkOrderGromGRN(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            clsGetWorkOrderBizActionVO nvo = null;
            DbDataReader reader = null;
            try
            {
                connection.Open();
                connection.BeginTransaction();
                nvo = valueObject as clsGetWorkOrderBizActionVO;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPurchaseOrderFromGRN");
                this.dbServer.AddInParameter(storedProcCommand, "FromGRN", DbType.Boolean, nvo.flagWOFromGRN);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierID", DbType.Int64, nvo.SearchSupplierID);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.SearchStoreID);
                this.dbServer.AddInParameter(storedProcCommand, "WONO", DbType.String, nvo.WONO);
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
                    if (nvo.WorkOrderList == null)
                    {
                        nvo.WorkOrderList = new List<clsWorkOrderVO>();
                    }
                    while (reader.Read())
                    {
                        clsWorkOrderVO item = new clsWorkOrderVO();
                        if ((bool) DALHelper.HandleDBNull(reader["Status"]))
                        {
                            item.WONO = (string) DALHelper.HandleDBNull(reader["WONO"]);
                            item.Date = (DateTime) DALHelper.HandleDBNull(reader["Date"]);
                            item.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                            item.UnitId = (long) DALHelper.HandleDBNull(reader["UnitID"]);
                            item.StoreName = (string) DALHelper.HandleDBNull(reader["StoreName"]);
                            item.SupplierName = (string) DALHelper.HandleDBNull(reader["SupplierName"]);
                            item.StoreID = (long) DALHelper.HandleDBNull(reader["StoreID"]);
                            item.SupplierID = (long) DALHelper.HandleDBNull(reader["SupplierID"]);
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
                            nvo.WorkOrderList.Add(item);
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

        public override IValueObject GetWorkOrderToCloseManually(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            clsGetWorkOrderForCloseBizActionVO nvo = null;
            DbDataReader reader = null;
            try
            {
                connection.Open();
                connection.BeginTransaction();
                nvo = valueObject as clsGetWorkOrderForCloseBizActionVO;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPurchaseOrderForCloseManually");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierID", DbType.Int64, nvo.SearchSupplierID);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.SearchStoreID);
                this.dbServer.AddInParameter(storedProcCommand, "WONO", DbType.String, nvo.WONO);
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
                    if (nvo.WorkOrderList == null)
                    {
                        nvo.WorkOrderList = new List<clsWorkOrderVO>();
                    }
                    while (reader.Read())
                    {
                        clsWorkOrderVO item = new clsWorkOrderVO {
                            WONO = Convert.ToString(reader["WONO"]),
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
                        bool flag = Convert.ToBoolean(reader["WOType"]);
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
                        nvo.WorkOrderList.Add(item);
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

        public override IValueObject UpdateRemarkForCancellationWO(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            clsUpdateRemarkForCancellationWO nwo = null;
            DbTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                nwo = valueObject as clsUpdateRemarkForCancellationWO;
                clsWorkOrderVO workOrder = nwo.WorkOrder;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateRemarkForCancellationWO");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "CancellationRemark", DbType.String, nwo.WorkOrder.CancellationRemark);
                this.dbServer.AddInParameter(storedProcCommand, "POID", DbType.Int64, nwo.WorkOrder.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nwo.WorkOrder.UnitId);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                nwo.SuccessStatus = -1;
                nwo.WorkOrder = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nwo;
        }

        public override IValueObject UpdateWorkOrderForApproval(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            clsUpdateWorkOrderForApproval approval = null;
            clsWorkOrderVO purchaseOrder = null;
            DbTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                approval = valueObject as clsUpdateWorkOrderForApproval;
                purchaseOrder = approval.PurchaseOrder;
                foreach (long num in approval.PurchaseOrder.ids)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateWorkOrderApprovalStatus");
                    storedProcCommand.Connection = connection;
                    this.dbServer.AddInParameter(storedProcCommand, "ApprovedBy", DbType.String, approval.PurchaseOrder.ApprovedBy);
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, num);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, approval.PurchaseOrder.UnitId);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                }
                foreach (clsWorkOrderDetailVO lvo in purchaseOrder.Items)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateIndetFromPO");
                    storedProcCommand.Connection = connection;
                    this.dbServer.AddInParameter(storedProcCommand, "POID", DbType.Int64, lvo.WOID);
                    this.dbServer.AddInParameter(storedProcCommand, "POUnitID", DbType.Decimal, lvo.WOUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "BalanceQty", DbType.Decimal, lvo.Quantity);
                    this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, lvo.ItemID);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
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
    }
}

