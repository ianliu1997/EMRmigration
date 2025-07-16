namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Inventory;
    using PalashDynamics.ValueObjects.Inventory.Indent;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Reflection;

    public class clsIndentDAL : clsBaseIndentDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsIndentDAL()
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

        public override IValueObject AddIndent(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddIndentBizActionVO nvo = valueObject as clsAddIndentBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                clsIndentMasterVO objIndent = nvo.objIndent;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddIndent");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, objIndent.LinkServer);
                if (objIndent.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, objIndent.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, objIndent.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, objIndent.Time);
                this.dbServer.AddInParameter(storedProcCommand, "IndentNumber", DbType.String, objIndent.IndentNumber);
                this.dbServer.AddInParameter(storedProcCommand, "TransactionMovementID", DbType.Int64, objIndent.TransactionMovementID);
                this.dbServer.AddInParameter(storedProcCommand, "FromStoreID", DbType.Int64, objIndent.FromStoreID);
                this.dbServer.AddInParameter(storedProcCommand, "ToStoreID", DbType.Int64, objIndent.ToStoreID);
                this.dbServer.AddInParameter(storedProcCommand, "DueDate", DbType.DateTime, objIndent.DueDate);
                this.dbServer.AddInParameter(storedProcCommand, "IndentCreatedByID", DbType.Int64, objIndent.IndentCreatedByID);
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, objIndent.Remark);
                this.dbServer.AddInParameter(storedProcCommand, "IndentStatus", DbType.Int32, objIndent.IndentStatus);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, objIndent.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "IsForwarded", DbType.Boolean, objIndent.IsForwarded);
                this.dbServer.AddInParameter(storedProcCommand, "IsApproved", DbType.Int32, objIndent.IsApproved);
                this.dbServer.AddInParameter(storedProcCommand, "InventoryIndentType", DbType.String, objIndent.InventoryIndentType);
                this.dbServer.AddInParameter(storedProcCommand, "IsIndent", DbType.Boolean, objIndent.IsIndent);
                if (!nvo.IsConvertToPR)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IsAuthorized", DbType.Boolean, 0);
                    this.dbServer.AddInParameter(storedProcCommand, "AuthorizedByID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(storedProcCommand, "AuthorizationDate", DbType.DateTime, null);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ConvertToPRID", DbType.Int64, objIndent.ConvertToPRID);
                    this.dbServer.AddInParameter(storedProcCommand, "IsConvertToPR", DbType.Boolean, nvo.IsConvertToPR);
                    this.dbServer.AddInParameter(storedProcCommand, "IsAuthorized", DbType.Boolean, objIndent.IsAuthorized);
                    this.dbServer.AddInParameter(storedProcCommand, "AuthorizedByID", DbType.Int64, objIndent.AuthorizedByID);
                    this.dbServer.AddInParameter(storedProcCommand, "AuthorizationDate", DbType.DateTime, objIndent.AuthorizationDate);
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, objIndent.Status);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, objIndent.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, objIndent.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsAgainstPatient", DbType.Boolean, objIndent.IsAgainstPatient);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objIndent.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddParameter(storedProcCommand, "Number", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.objIndent.ID = (long?) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.objIndent.IndentNumber = (string) this.dbServer.GetParameterValue(storedProcCommand, "Number");
                foreach (clsIndentDetailVO lvo in nvo.objIndentDetailList)
                {
                    lvo.IndentID = nvo.objIndent.ID;
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddIndentDetail");
                    command2.Connection = connection;
                    this.dbServer.AddInParameter(command2, "LinkServer", DbType.String, objIndent.LinkServer);
                    if (objIndent.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(command2, "LinkServerAlias", DbType.String, objIndent.LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddInParameter(command2, "IndentID", DbType.Int64, lvo.IndentID);
                    this.dbServer.AddInParameter(command2, "ItemID", DbType.Int64, lvo.ItemID);
                    this.dbServer.AddInParameter(command2, "Quantity", DbType.Double, lvo.SingleQuantity);
                    this.dbServer.AddInParameter(command2, "UOM", DbType.Int64, lvo.SelectedUOM.ID);
                    this.dbServer.AddInParameter(command2, "SUOM", DbType.Int64, lvo.SUOMID);
                    this.dbServer.AddInParameter(command2, "StockCF", DbType.Single, lvo.StockCF);
                    this.dbServer.AddInParameter(command2, "StockingQuantity", DbType.Double, lvo.StockingQuantity);
                    this.dbServer.AddInParameter(command2, "BaseUOMID", DbType.Int64, lvo.BaseUOMID);
                    this.dbServer.AddInParameter(command2, "ConversionFactor", DbType.Single, lvo.ConversionFactor);
                    this.dbServer.AddInParameter(command2, "RequiredQuantity", DbType.Double, lvo.RequiredQuantity);
                    this.dbServer.AddInParameter(command2, "PendingQuantity", DbType.Double, lvo.RequiredQuantity);
                    this.dbServer.AddInParameter(command2, "PurchaseOrderQuantity", DbType.Double, lvo.PurchaseOrderQuantity);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "IndentUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "InventoryIndentType", DbType.String, objIndent.InventoryIndentType);
                    this.dbServer.AddInParameter(command2, "IsIndent", DbType.Boolean, objIndent.IsIndent);
                    this.dbServer.AddInParameter(command2, "FromStoreID", DbType.Int64, objIndent.FromStoreID);
                    this.dbServer.AddInParameter(command2, "IsForwarded", DbType.Boolean, 1);
                    this.dbServer.AddInParameter(command2, "IsApproved", DbType.Boolean, 1);
                    this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, lvo.ID);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                    lvo.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                }
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                nvo.SuccessStatus = -1;
                nvo.objIndent = null;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection = null;
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject ApproveDirect(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateIndentBizActionVO nvo = valueObject as clsUpdateIndentBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                clsIndentMasterVO objIndent = nvo.objIndent;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ApproveDirect");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "IssueId", DbType.Int64, objIndent.IssueId);
                this.dbServer.AddInParameter(storedProcCommand, "IssueUnitID", DbType.Int64, objIndent.IssueUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, objIndent.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsAuthorized", DbType.Boolean, objIndent.IsAuthorized);
                this.dbServer.AddInParameter(storedProcCommand, "AuthorizedByID", DbType.Int64, objIndent.AuthorizedByID);
                this.dbServer.AddInParameter(storedProcCommand, "AuthorizationDate", DbType.DateTime, objIndent.AuthorizationDate);
                this.dbServer.AddInParameter(storedProcCommand, "IsApproved", DbType.Boolean, objIndent.IsApproved);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
            }
            finally
            {
                connection.Close();
                transaction = null;
            }
            return valueObject;
        }

        public override IValueObject CloseIndentFromDashBoard(IValueObject valueObject, clsUserVO userVO)
        {
            clsCloseIndentFromDashboard dashboard = valueObject as clsCloseIndentFromDashboard;
            try
            {
                if (!dashboard.isBulkIndentClosed)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CloseIndentFromDashBoard");
                    this.dbServer.AddInParameter(storedProcCommand, "IndentID", DbType.Int64, dashboard.IndentID);
                    this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, dashboard.Remarks);
                    this.dbServer.AddInParameter(storedProcCommand, "IndentStatus", DbType.Int32, InventoryIndentStatus.ForceFullyCompleted);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, userVO.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, userVO.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, dashboard.UnitID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    dashboard.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                }
                else if (dashboard.isBulkIndentClosed)
                {
                    for (int i = 0; i < dashboard.BulkCloseIndetList.Count; i++)
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CloseIndentFromDashBoard");
                        this.dbServer.AddInParameter(storedProcCommand, "IndentID", DbType.Int64, dashboard.BulkCloseIndetList[i].ID);
                        this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, dashboard.Remarks);
                        this.dbServer.AddInParameter(storedProcCommand, "IndentStatus", DbType.Int32, InventoryIndentStatus.BulkClose);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, userVO.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, userVO.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, dashboard.BulkCloseIndetList[i].UnitID);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(storedProcCommand);
                        dashboard.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                        storedProcCommand.Cancel();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dashboard;
        }

        public override IValueObject ClosePurchaseRequisitionFromDashBoard(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCloseIndentFromDashboard dashboard = valueObject as clsCloseIndentFromDashboard;
            try
            {
                if (!dashboard.isBulkPRClosed)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ClosePurchaseRequisitionFromDashBoard");
                    this.dbServer.AddInParameter(storedProcCommand, "IndentID", DbType.Int64, dashboard.IndentID);
                    this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, dashboard.Remarks);
                    this.dbServer.AddInParameter(storedProcCommand, "IndentStatus", DbType.Int32, InventoryIndentStatus.ForceFullyCompleted);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, dashboard.UnitID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    dashboard.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                }
                else if (dashboard.isBulkPRClosed)
                {
                    for (int i = 0; i < dashboard.BulkCloseIndetList.Count; i++)
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ClosePurchaseRequisitionFromDashBoard");
                        this.dbServer.AddInParameter(storedProcCommand, "IndentID", DbType.Int64, dashboard.BulkCloseIndetList[i].ID);
                        this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, dashboard.Remarks);
                        this.dbServer.AddInParameter(storedProcCommand, "IndentStatus", DbType.Int32, InventoryIndentStatus.BulkClose);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, dashboard.BulkCloseIndetList[i].UnitID);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(storedProcCommand);
                        dashboard.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                        storedProcCommand.Cancel();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dashboard;
        }

        public override IValueObject GetIndentDetailList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIndentDetailListBizActionVO nvo = valueObject as clsGetIndentDetailListBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStoreIndentDetailList");
                this.dbServer.AddInParameter(storedProcCommand, "IndentID", DbType.Int64, nvo.IndentID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.IndentDetailList == null)
                    {
                        nvo.IndentDetailList = new List<clsIndentDetailVO>();
                    }
                    while (reader.Read())
                    {
                        clsIndentDetailVO item = new clsIndentDetailVO {
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            IndentID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentID"]))),
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            SingleQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["Quantity"])),
                            UOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UOM"])),
                            UOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransactionUOM"])),
                            SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SUOM"])),
                            StockCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"])),
                            ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["ConversionFactor"])),
                            StockingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["StockingQuantity"])),
                            SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUom"])),
                            BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUOMID"])),
                            RequiredQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["RequiredQuantity"])),
                            BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUom"])),
                            PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQuantity"])),
                            PurchaseOrderQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PurchaseOrderQuantity"])),
                            IndentUnitID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentUnitID"]))),
                            PurchaseRate = (double) Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"])),
                            IsItemBlock = Convert.ToBoolean(reader["IsItemBlock"]),
                            TotalBatchAvailableStock = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalBatchAvailableStock"])),
                            PendingQtyFP = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQuantityFP"]))
                        };
                        nvo.IndentDetailList.Add(item);
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
            return valueObject;
        }

        public override IValueObject GetIndentList(IValueObject valueObject, clsUserVO userVO)
        {
            clsGetIndenListBizActionVO nvo = valueObject as clsGetIndenListBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStoreIndentList");
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
                this.dbServer.AddInParameter(storedProcCommand, "FromStoreID", DbType.Int64, nvo.FromStoreID);
                this.dbServer.AddInParameter(storedProcCommand, "ToStoreID", DbType.Int64, nvo.ToStoreID);
                this.dbServer.AddInParameter(storedProcCommand, "IndentNO", DbType.String, nvo.IndentNO);
                if ((nvo.MRNo != null) && (nvo.MRNo.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MrNo", DbType.String, "%" + nvo.MRNo + "%");
                }
                if ((nvo.PatientName != null) && (nvo.PatientName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "PatientName", DbType.String, nvo.PatientName + "%");
                }
                this.dbServer.AddInParameter(storedProcCommand, "CheckStatusType", DbType.Boolean, nvo.CheckStatusType);
                this.dbServer.AddInParameter(storedProcCommand, "IndentStatus", DbType.Int32, nvo.IndentStatus);
                this.dbServer.AddInParameter(storedProcCommand, "IsApproved", DbType.Boolean, nvo.isApproved);
                this.dbServer.AddInParameter(storedProcCommand, "IsForwarded", DbType.Boolean, nvo.isFrowrded);
                this.dbServer.AddInParameter(storedProcCommand, "IsCancelled", DbType.Boolean, nvo.isCancelled);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, nvo.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, nvo.UserID);
                this.dbServer.AddInParameter(storedProcCommand, "isIndent", DbType.Int32, nvo.isIndent);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.IndentList == null)
                    {
                        nvo.IndentList = new List<clsIndentMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsIndentMasterVO item = new clsIndentMasterVO {
                            Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]))),
                            DueDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["DueDate"]))),
                            FromStoreID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["FromStoreID"]))),
                            ID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]))),
                            UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            FromStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["FromStoreName"])),
                            ToStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["ToStoreName"])),
                            IndentCreatedByID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentCreatedByID"]))),
                            IndentNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IndentNumber"])),
                            IndentStatus = (InventoryIndentStatus) Convert.ToInt32(DALHelper.HandleDBNull(reader["IndentStatus"])),
                            IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"])),
                            IsForwarded = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsForwarded"])),
                            IsApproved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsApproved"])),
                            IsAuthorized = new bool?(Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAuthorized"]))),
                            AuthorizedByID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["AuthorizedByID"]))),
                            AuthorizationDate = DALHelper.HandleDate(reader["AuthorizationDate"]),
                            Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"])),
                            Time = DALHelper.HandleDate(reader["Time"]),
                            ToStoreID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ToStoreID"]))),
                            TransactionMovementID = new int?(Convert.ToInt32(DALHelper.HandleDBNull(reader["TransactionMovementID"]))),
                            UnitID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])))
                        };
                        item.IndentUnitID = item.UnitID;
                        item.IndentType = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IndentType"]));
                        item.IsConvertToPR = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsConvertToPR"]));
                        item.IsIndent = Convert.ToInt32(DALHelper.HandleDBNull(reader["IsIndent"]));
                        item.InventoryIndentType = (InventoryIndentType) Enum.Parse(typeof(InventoryIndentType), Convert.ToString(DALHelper.HandleDBNull(reader["InventoryIndentType"])));
                        item.GRNNowithDate = !string.IsNullOrEmpty(Convert.ToString(DALHelper.HandleDBNull(reader["GRN No - Date"]))) ? Convert.ToString(DALHelper.HandleDBNull(reader["GRN No - Date"])) : Convert.ToString(DALHelper.HandleDBNull(reader["IndentGRN No - Date"]));
                        item.PONowithDate = !string.IsNullOrEmpty(Convert.ToString(DALHelper.HandleDBNull(reader["PO No - Date"]))) ? Convert.ToString(DALHelper.HandleDBNull(reader["PO No - Date"])) : Convert.ToString(DALHelper.HandleDBNull(reader["IndentPO No - Date"]));
                        item.IssueNowithDate = !string.IsNullOrEmpty(Convert.ToString(DALHelper.HandleDBNull(reader["Issue No - Date"]))) ? Convert.ToString(DALHelper.HandleDBNull(reader["Issue No - Date"])) : Convert.ToString(DALHelper.HandleDBNull(reader["IndentIssue No - Date"]));
                        item.PRNowithDate = !string.IsNullOrEmpty(Convert.ToString(DALHelper.HandleDBNull(reader["Indent No - Date"]))) ? Convert.ToString(DALHelper.HandleDBNull(reader["Indent No - Date"])) : Convert.ToString(DALHelper.HandleDBNull(reader["PR No - Date"]));
                        item.IsAgainstPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPatientIndent"]));
                        item.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        item.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        item.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        item.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        nvo.IndentList.Add(item);
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

        public override IValueObject GetIndentListByStoreId(IValueObject valueObject, clsUserVO userVO)
        {
            clsGetIndenListByStoreIdBizActionVO nvo = valueObject as clsGetIndenListByStoreIdBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIndentListByStoreId_1");
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
                this.dbServer.AddInParameter(storedProcCommand, "IndentNumber", DbType.String, nvo.IndentNumber);
                this.dbServer.AddInParameter(storedProcCommand, "Freezed", DbType.Boolean, nvo.Freezed);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "FromPO", DbType.Boolean, nvo.FromPO);
                this.dbServer.AddInParameter(storedProcCommand, "IsIndent", DbType.Int32, nvo.IsIndent);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.IndentList == null)
                    {
                        nvo.IndentList = new List<clsIndentMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsIndentMasterVO item = new clsIndentMasterVO {
                            AuthorizationDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["AuthorizationDate"]))),
                            AuthorizedByID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["AuthorizedByID"]))),
                            AuthorizedByName = Convert.ToString(DALHelper.HandleDBNull(reader["AuthorizedByName"])),
                            Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]))),
                            DueDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["DueDate"]))),
                            FromStoreID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["FromStoreID"]))),
                            ID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]))),
                            IndentCreatedByID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentCreatedByID"]))),
                            IndentCreatedByName = Convert.ToString(DALHelper.HandleDBNull(reader["IndentCreatedByName"])),
                            IndentNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IndentNumber"])),
                            IndentStatus = (InventoryIndentStatus) Convert.ToInt32(DALHelper.HandleDBNull(reader["IndentStatus"])),
                            IsAuthorized = new bool?(Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAuthorized"]))),
                            Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"])),
                            Time = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Time"]))),
                            ToStoreID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ToStoreID"]))),
                            TransactionMovementID = new int?(Convert.ToInt32(DALHelper.HandleDBNull(reader["TransactionMovementID"]))),
                            UnitID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]))),
                            UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"])),
                            IsIndent = Convert.ToInt32(DALHelper.HandleDBNull(reader["IsIndent"]))
                        };
                        item.IndentUnitID = item.UnitID;
                        item.IsAgainstPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPatientIndent"]));
                        item.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        item.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        item.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"]));
                        item.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        nvo.IndentList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRowCount = Convert.ToInt32(DALHelper.HandleDBNull(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows")));
            }
            catch (Exception exception1)
            {
                throw exception1;
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

        public override IValueObject GetIndentListForDashBoard(IValueObject valueObject, clsUserVO userVO)
        {
            clsGetIndentListForInventorDashBoardBizActionVO nvo = valueObject as clsGetIndentListForInventorDashBoardBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStoreIndentListForDashBoard_New_01");
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
                this.dbServer.AddInParameter(storedProcCommand, "FromStoreID", DbType.Int64, nvo.FromStoreID);
                this.dbServer.AddInParameter(storedProcCommand, "ToStoreID", DbType.Int64, nvo.ToStoreID);
                this.dbServer.AddInParameter(storedProcCommand, "IndentNO", DbType.String, nvo.IndentNO);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.Date);
                this.dbServer.AddInParameter(storedProcCommand, "IndentStatus", DbType.Int64, nvo.IndentStatus);
                this.dbServer.AddInParameter(storedProcCommand, "IsPaging", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "NoOfRecordShow", DbType.Int64, nvo.NoOfRecords);
                this.dbServer.AddInParameter(storedProcCommand, "StartIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRow", DbType.Int64, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, nvo.UserID);
                this.dbServer.AddInParameter(storedProcCommand, "IsIndent", DbType.Boolean, nvo.IsIndent);
                if (nvo.IsOrderBy != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "OrderBy", DbType.Int64, nvo.IsOrderBy);
                }
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.IndentList == null)
                    {
                        nvo.IndentList = new List<clsIndentMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsIndentMasterVO item = new clsIndentMasterVO {
                            ID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]))),
                            Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]))),
                            Time = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Time"]))),
                            IndentNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IndentNumber"])),
                            DueDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["DueDate"]))),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            IndentStatus = (InventoryIndentStatus) Convert.ToInt32(DALHelper.HandleDBNull(reader["IndentStatus"])),
                            FromStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["FromStore"])),
                            ToStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["ToStore"])),
                            AddedOn = Convert.ToString(DALHelper.HandleDBNull(reader["LoginName"])),
                            IsAuthorized = new bool?(Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAuthorized"]))),
                            AuthorizedByID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["AuthorizedByID"]))),
                            AuthorizedByName = Convert.ToString(DALHelper.HandleDBNull(reader["UserName"])),
                            UnitID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])))
                        };
                        if (!nvo.IsIndent)
                        {
                            item.PRBaseItemQty = Convert.ToInt32(DALHelper.HandleDBNull(reader["PRBaseItemQty"]));
                            item.POBaseItemQty = Convert.ToInt32(DALHelper.HandleDBNull(reader["POBaseItemQty"]));
                            item.GRNBaseItemQty = Convert.ToInt32(DALHelper.HandleDBNull(reader["GRNBaseItemQty"]));
                        }
                        else if (nvo.IsIndent)
                        {
                            item.IndentBaseItemQty = Convert.ToInt32(DALHelper.HandleDBNull(reader["IndentBaseItemQty"]));
                            item.IssueBaseItemQty = Convert.ToInt32(DALHelper.HandleDBNull(reader["IssueBaseItemQty"]));
                            item.ReceivedBaseItemQty = Convert.ToInt32(DALHelper.HandleDBNull(reader["ReceivedBaseItemQty"]));
                        }
                        nvo.IndentList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRow = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRow"));
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

        public override IValueObject UpdateIndent(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateIndentBizActionVO nvo = valueObject as clsUpdateIndentBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                clsIndentMasterVO objIndent = nvo.objIndent;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateIndent");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, objIndent.LinkServer);
                if (objIndent.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, objIndent.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, objIndent.Date);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, objIndent.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, objIndent.Time);
                this.dbServer.AddInParameter(storedProcCommand, "IndentID", DbType.Int64, objIndent.ID);
                this.dbServer.AddInParameter(storedProcCommand, "TransactionMovementID", DbType.Int64, objIndent.TransactionMovementID);
                this.dbServer.AddInParameter(storedProcCommand, "FromStoreID", DbType.Int64, objIndent.FromStoreID);
                this.dbServer.AddInParameter(storedProcCommand, "ToStoreID", DbType.Int64, objIndent.ToStoreID);
                this.dbServer.AddInParameter(storedProcCommand, "DueDate", DbType.DateTime, objIndent.DueDate);
                this.dbServer.AddInParameter(storedProcCommand, "IndentCreatedByID", DbType.Int64, objIndent.IndentCreatedByID);
                this.dbServer.AddInParameter(storedProcCommand, "IsAuthorized", DbType.Boolean, objIndent.IsAuthorized);
                this.dbServer.AddInParameter(storedProcCommand, "AuthorizedByID", DbType.Int64, objIndent.AuthorizedByID);
                this.dbServer.AddInParameter(storedProcCommand, "AuthorizationDate", DbType.DateTime, objIndent.AuthorizationDate);
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, objIndent.Remark);
                this.dbServer.AddInParameter(storedProcCommand, "IndentStatus", DbType.Int32, objIndent.IndentStatus);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, objIndent.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "IsForwarded", DbType.Boolean, objIndent.IsForwarded);
                this.dbServer.AddInParameter(storedProcCommand, "IsApproved", DbType.Int32, objIndent.IsApproved);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, objIndent.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "InventoryIndentType", DbType.String, objIndent.InventoryIndentType);
                this.dbServer.AddInParameter(storedProcCommand, "IsIndent", DbType.Boolean, objIndent.IsIndent);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if (objIndent.IsChangeAndApprove)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteIndentDetails");
                    command2.Connection = connection;
                    this.dbServer.AddInParameter(command2, "IndentID", DbType.Int64, objIndent.ID);
                    this.dbServer.AddInParameter(command2, "IndentUnitID", DbType.Int64, objIndent.UnitID);
                    this.dbServer.AddInParameter(command2, "IsForChangeAndApprove", DbType.Boolean, 0);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                }
                if (!objIndent.IsForwarded && !objIndent.IsApproved)
                {
                    foreach (clsIndentDetailVO lvo in nvo.objIndent.IndentDetailsList)
                    {
                        lvo.IndentID = nvo.objIndent.ID;
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddIndentDetail");
                        command3.Connection = connection;
                        this.dbServer.AddInParameter(command3, "LinkServer", DbType.String, objIndent.LinkServer);
                        if (objIndent.LinkServer != null)
                        {
                            this.dbServer.AddInParameter(command3, "LinkServerAlias", DbType.String, objIndent.LinkServer.Replace(@"\", "_"));
                        }
                        this.dbServer.AddInParameter(command3, "IndentID", DbType.Int64, lvo.IndentID);
                        this.dbServer.AddInParameter(command3, "ItemID", DbType.Int64, lvo.ItemID);
                        this.dbServer.AddInParameter(command3, "Quantity", DbType.Double, lvo.SingleQuantity);
                        this.dbServer.AddInParameter(command3, "UOM", DbType.Int64, lvo.SelectedUOM.ID);
                        this.dbServer.AddInParameter(command3, "SUOM", DbType.Int64, lvo.SUOMID);
                        this.dbServer.AddInParameter(command3, "StockCF", DbType.Single, lvo.StockCF);
                        this.dbServer.AddInParameter(command3, "StockingQuantity", DbType.Double, lvo.StockingQuantity);
                        this.dbServer.AddInParameter(command3, "BaseUOMID", DbType.Int64, lvo.BaseUOMID);
                        this.dbServer.AddInParameter(command3, "ConversionFactor", DbType.Single, lvo.ConversionFactor);
                        this.dbServer.AddInParameter(command3, "RequiredQuantity", DbType.Double, lvo.RequiredQuantity);
                        this.dbServer.AddInParameter(command3, "PendingQuantity", DbType.Double, lvo.RequiredQuantity);
                        this.dbServer.AddInParameter(command3, "PurchaseOrderQuantity", DbType.Double, lvo.PurchaseOrderQuantity);
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, objIndent.UnitID);
                        this.dbServer.AddInParameter(command3, "IndentUnitID", DbType.Int64, objIndent.UnitID);
                        this.dbServer.AddInParameter(command3, "InventoryIndentType", DbType.String, objIndent.InventoryIndentType);
                        this.dbServer.AddInParameter(command3, "IsIndent", DbType.Int32, objIndent.IsIndent);
                        this.dbServer.AddInParameter(command3, "FromStoreID", DbType.Int64, objIndent.FromStoreID);
                        this.dbServer.AddInParameter(command3, "IsForwarded", DbType.Boolean, objIndent.IsForwarded);
                        if (!objIndent.IsChangeAndApprove)
                        {
                            this.dbServer.AddInParameter(command3, "IsApproved", DbType.Boolean, objIndent.IsApproved);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "IsChangeAndApprove", DbType.Boolean, objIndent.IsChangeAndApprove);
                            if (objIndent.IsIndent == 1)
                            {
                                this.dbServer.AddInParameter(command3, "IsApproved", DbType.Boolean, 1);
                            }
                        }
                        this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, lvo.ID);
                        this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
                        lvo.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                    }
                }
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                nvo.SuccessStatus = -1;
                nvo.objIndent = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
            }
            return valueObject;
        }

        public override IValueObject UpdateIndentForChangeAndApprove(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateIndentBizActionVO nvo = valueObject as clsUpdateIndentBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                clsIndentMasterVO objIndent = nvo.objIndent;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateIndent");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, objIndent.LinkServer);
                if (objIndent.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, objIndent.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, objIndent.Date);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, objIndent.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, objIndent.Time);
                this.dbServer.AddInParameter(storedProcCommand, "IndentID", DbType.Int64, objIndent.ID);
                this.dbServer.AddInParameter(storedProcCommand, "TransactionMovementID", DbType.Int64, objIndent.TransactionMovementID);
                this.dbServer.AddInParameter(storedProcCommand, "FromStoreID", DbType.Int64, objIndent.FromStoreID);
                this.dbServer.AddInParameter(storedProcCommand, "ToStoreID", DbType.Int64, objIndent.ToStoreID);
                this.dbServer.AddInParameter(storedProcCommand, "DueDate", DbType.DateTime, objIndent.DueDate);
                this.dbServer.AddInParameter(storedProcCommand, "IndentCreatedByID", DbType.Int64, objIndent.IndentCreatedByID);
                this.dbServer.AddInParameter(storedProcCommand, "IsAuthorized", DbType.Boolean, objIndent.IsAuthorized);
                this.dbServer.AddInParameter(storedProcCommand, "AuthorizedByID", DbType.Int64, objIndent.AuthorizedByID);
                this.dbServer.AddInParameter(storedProcCommand, "AuthorizationDate", DbType.DateTime, objIndent.AuthorizationDate);
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, objIndent.Remark);
                this.dbServer.AddInParameter(storedProcCommand, "IndentStatus", DbType.Int32, objIndent.IndentStatus);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, objIndent.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "IsForwarded", DbType.Boolean, objIndent.IsForwarded);
                if (objIndent.IsChangeAndApprove)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IsChangeAndApprove", DbType.Boolean, objIndent.IsChangeAndApprove);
                }
                this.dbServer.AddInParameter(storedProcCommand, "IsApproved", DbType.Int32, objIndent.IsApproved);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, objIndent.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "InventoryIndentType", DbType.String, objIndent.InventoryIndentType);
                this.dbServer.AddInParameter(storedProcCommand, "IsIndent", DbType.Boolean, objIndent.IsIndent);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if (!objIndent.IsForwarded)
                {
                    foreach (clsIndentDetailVO lvo in nvo.objIndent.IndentDetailsList)
                    {
                        lvo.IndentID = nvo.objIndent.ID;
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_UpdateIndentDetail");
                        command2.Connection = connection;
                        this.dbServer.AddInParameter(command2, "LinkServer", DbType.String, objIndent.LinkServer);
                        if (objIndent.LinkServer != null)
                        {
                            this.dbServer.AddInParameter(command2, "LinkServerAlias", DbType.String, objIndent.LinkServer.Replace(@"\", "_"));
                        }
                        this.dbServer.AddInParameter(command2, "IndentID", DbType.Int64, lvo.IndentID);
                        this.dbServer.AddInParameter(command2, "ItemID", DbType.Int64, lvo.ItemID);
                        this.dbServer.AddInParameter(command2, "Quantity", DbType.Double, lvo.SingleQuantity);
                        this.dbServer.AddInParameter(command2, "UOM", DbType.Int64, lvo.SelectedUOM.ID);
                        this.dbServer.AddInParameter(command2, "SUOM", DbType.Int64, lvo.SUOMID);
                        this.dbServer.AddInParameter(command2, "StockCF", DbType.Single, lvo.StockCF);
                        this.dbServer.AddInParameter(command2, "ConversionFactor", DbType.Single, lvo.ConversionFactor);
                        this.dbServer.AddInParameter(command2, "StockingQuantity", DbType.Double, lvo.StockingQuantity);
                        this.dbServer.AddInParameter(command2, "BaseUOMID", DbType.Int64, lvo.BaseUOMID);
                        this.dbServer.AddInParameter(command2, "RequiredQuantity", DbType.Double, lvo.RequiredQuantity);
                        this.dbServer.AddInParameter(command2, "PendingQuantity", DbType.Double, lvo.RequiredQuantity);
                        this.dbServer.AddInParameter(command2, "PurchaseOrderQuantity", DbType.Double, lvo.PurchaseOrderQuantity);
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, objIndent.UnitID);
                        this.dbServer.AddInParameter(command2, "IndentUnitID", DbType.Int64, objIndent.UnitID);
                        this.dbServer.AddInParameter(command2, "InventoryIndentType", DbType.String, objIndent.InventoryIndentType);
                        this.dbServer.AddInParameter(command2, "IsIndent", DbType.Boolean, objIndent.IsIndent);
                        this.dbServer.AddInParameter(command2, "FromStoreID", DbType.Int64, objIndent.FromStoreID);
                        this.dbServer.AddInParameter(command2, "IsForwarded", DbType.Boolean, objIndent.IsForwarded);
                        this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, lvo.ID);
                        this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command2, transaction);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                        lvo.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                    }
                }
                if (nvo.objIndent.DeletedIndentDetailsList.Count > 0)
                {
                    foreach (clsIndentDetailVO lvo2 in nvo.objIndent.DeletedIndentDetailsList)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_DeleteIndentDetails");
                        command3.Connection = connection;
                        this.dbServer.AddInParameter(command3, "IndentID", DbType.Int64, lvo2.IndentID);
                        this.dbServer.AddInParameter(command3, "ItemID", DbType.Int64, lvo2.ItemID);
                        this.dbServer.AddInParameter(command3, "IndentUnitID", DbType.Int64, lvo2.IndentUnitID);
                        this.dbServer.AddInParameter(command3, "IsForChangeAndApprove", DbType.Boolean, 1);
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
                nvo.objIndent = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
            }
            return valueObject;
        }

        public override IValueObject UpdateIndentOnlyForFreeze(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateIndentOnlyForFreezeBizActionVO nvo = valueObject as clsUpdateIndentOnlyForFreezeBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateIndentOnlyForFreeze");
                this.dbServer.AddInParameter(storedProcCommand, "IndentID", DbType.Int64, nvo.IndentID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject UpdateIndentRemarkandCancelIndent(IValueObject valueObject, clsUserVO userVO)
        {
            clsUpdateRemarkForIndentCancellationBizActionVO nvo = valueObject as clsUpdateRemarkForIndentCancellationBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            connection.Open();
            DbTransaction transaction = connection.BeginTransaction();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CancelIndentAndUpdateRemark");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "IndentID", DbType.Int64, nvo.IndentMaster.ID);
                this.dbServer.AddInParameter(storedProcCommand, "CancellationRemark", DbType.String, nvo.CancellationRemark);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, userVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, userVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "IndentUnitID", DbType.Int64, nvo.IndentMaster.UnitID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw exception;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection = null;
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject UpdateIndentRemarkandRejectIndent(IValueObject valueObject, clsUserVO userVO)
        {
            clsUpdateRemarkForIndentCancellationBizActionVO nvo = valueObject as clsUpdateRemarkForIndentCancellationBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            connection.Open();
            DbTransaction transaction = connection.BeginTransaction();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_RejectIndentAndUpdateRemark");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "IndentID", DbType.Int64, nvo.IndentMaster.ID);
                this.dbServer.AddInParameter(storedProcCommand, "CancellationRemark", DbType.String, nvo.CancellationRemark);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, userVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, userVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "IndentUnitID", DbType.Int64, nvo.IndentMaster.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IndentStatus", DbType.Int64, nvo.IndentMaster.IndentStatus);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw exception;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection = null;
                transaction = null;
            }
            return nvo;
        }
    }
}

