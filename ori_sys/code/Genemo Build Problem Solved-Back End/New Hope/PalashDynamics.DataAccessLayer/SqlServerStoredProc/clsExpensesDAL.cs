namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Billing;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsExpensesDAL : clsBaseExpensesDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsExpensesDAL()
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

        public override IValueObject AddExpenses(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddExpensesBizActionVO bizActionObj = valueObject as clsAddExpensesBizActionVO;
            bizActionObj = (bizActionObj.Details.ID != 0L) ? this.UpdateExpenseDetails(bizActionObj) : this.AddExpensesdetails(bizActionObj, UserVo);
            return valueObject;
        }

        private clsAddExpensesBizActionVO AddExpensesdetails(clsAddExpensesBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsExpensesVO details = BizActionObj.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddExpense");
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, details.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Expense", DbType.Int64, details.Expense);
                this.dbServer.AddInParameter(storedProcCommand, "ExpenseDate", DbType.DateTime, details.ExpenseDate);
                this.dbServer.AddInParameter(storedProcCommand, "Amount", DbType.Double, details.Amount);
                this.dbServer.AddInParameter(storedProcCommand, "VoucherCreatedBy", DbType.String, details.voucherCreatedby);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, details.IsFreezed);
                if (details.Remark != null)
                {
                    details.Remark = details.Remark.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, details.Remark);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, details.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, details.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, details.ID);
                this.dbServer.AddParameter(storedProcCommand, "VoucherNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "????????????????????");
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                BizActionObj.Details.ID = details.ID;
                details.VoucherNo = (string) this.dbServer.GetParameterValue(storedProcCommand, "VoucherNo");
                details.UnitID = UserVo.UserLoginInfo.UnitId;
                if ((details.ExpenseDetails != null) && (details.ExpenseDetails.Count > 0))
                {
                    foreach (clsExpensesDetailsVO svo2 in details.ExpenseDetails)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddExpenseDetails");
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, details.UnitID);
                        this.dbServer.AddInParameter(command2, "ExpenseID", DbType.Int64, details.ID);
                        this.dbServer.AddInParameter(command2, "ExpenseUnitId", DbType.Int64, details.UnitID);
                        this.dbServer.AddInParameter(command2, "PaymentMode", DbType.Int32, svo2.PaymentMode);
                        this.dbServer.AddInParameter(command2, "TransferToBank", DbType.Int64, svo2.TransferToBank);
                        this.dbServer.AddInParameter(command2, "ChequeNo", DbType.String, svo2.ChequeNo);
                        this.dbServer.AddInParameter(command2, "ChequeDate", DbType.DateTime, svo2.ChequeDate);
                        this.dbServer.AddInParameter(command2, "Payto", DbType.String, svo2.Payto);
                        this.dbServer.AddInParameter(command2, "PaymentAmount", DbType.Double, svo2.PaymentAmount);
                        this.dbServer.AddInParameter(command2, "TransferToAccountNo", DbType.Int64, svo2.TransferToAccountNo);
                        this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, details.CreatedUnitID);
                        this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, details.Status);
                        this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                        if (details.AddedOn != null)
                        {
                            details.AddedOn = details.AddedOn.Trim();
                        }
                        this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, details.AddedDateTime);
                        if (details.AddedWindowsLoginName != null)
                        {
                            details.AddedWindowsLoginName = details.AddedWindowsLoginName.Trim();
                        }
                        this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                        this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command2, transaction);
                        svo2.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                    }
                }
                BizActionObj.SuccessStatus = 0;
                transaction.Commit();
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return BizActionObj;
        }

        public override IValueObject GetExpenses(IValueObject valueObject, clsUserVO Uservo)
        {
            clsGetExpensesListBizActionVO nvo = valueObject as clsGetExpensesListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetExpense");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Expense", DbType.Int64, nvo.Expense);
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "Voucherno", DbType.String, nvo.Voucherno);
                this.dbServer.AddInParameter(storedProcCommand, "Vouchercreatedby", DbType.String, nvo.Vouchercreatedby);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ID");
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Details == null)
                    {
                        nvo.Details = new List<clsExpensesVO>();
                    }
                    while (reader.Read())
                    {
                        clsExpensesVO item = new clsExpensesVO {
                            ID = (long) reader["ID"],
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            UnitName = (string) DALHelper.HandleDBNull(reader["UnitName"]),
                            Expense = (long) DALHelper.HandleDBNull(reader["Expense"]),
                            ExpenseName = (string) DALHelper.HandleDBNull(reader["ExpenseName"]),
                            Amount = (double) DALHelper.HandleDBNull(reader["Amount"]),
                            Remark = (string) DALHelper.HandleDBNull(reader["Remarks"]),
                            VoucherNo = (string) DALHelper.HandleDBNull(reader["VoucherNo"]),
                            voucherCreatedby = (string) DALHelper.HandleDBNull(reader["VoucherCreatedBy"])
                        };
                        DateTime? nullable = DALHelper.HandleDate(reader["ExpenseDate"]);
                        item.ExpenseDate = new DateTime?(nullable.Value);
                        item.IsFreezed = (bool) DALHelper.HandleDBNull(reader["IsFreezed"]);
                        item.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                        nvo.Details.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        private clsAddExpensesBizActionVO UpdateExpenseDetails(clsAddExpensesBizActionVO BizActionObj)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsExpensesVO details = BizActionObj.Details;
                if (details.ID > 0L)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateExpense");
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, details.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, details.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "Expense", DbType.Int64, details.Expense);
                    this.dbServer.AddInParameter(storedProcCommand, "VoucherNo", DbType.String, details.VoucherNo);
                    this.dbServer.AddInParameter(storedProcCommand, "ExpenseDate", DbType.DateTime, details.ExpenseDate);
                    this.dbServer.AddInParameter(storedProcCommand, "Amount", DbType.Double, details.Amount);
                    this.dbServer.AddInParameter(storedProcCommand, "VoucherCreatedBy", DbType.String, details.voucherCreatedby);
                    this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, details.IsFreezed);
                    if (details.Remark != null)
                    {
                        details.Remark = details.Remark.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, details.Remark);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, details.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, details.CreatedUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, details.AddedBy);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, details.AddedOn);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, details.AddedDateTime);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, details.AddedWindowsLoginName);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    if ((details.ExpenseDetails != null) && (details.ExpenseDetails.Count > 0))
                    {
                        foreach (clsExpensesDetailsVO svo2 in details.ExpenseDetails)
                        {
                            DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddExpenseDetails");
                            this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, details.UnitID);
                            this.dbServer.AddInParameter(command, "ExpenseID", DbType.Int64, details.ID);
                            this.dbServer.AddInParameter(command, "ExpenseUnitId", DbType.Int64, details.UnitID);
                            this.dbServer.AddInParameter(command, "PaymentMode", DbType.Int32, svo2.PaymentMode);
                            this.dbServer.AddInParameter(command, "TransferToBank", DbType.Int64, svo2.TransferToBank);
                            this.dbServer.AddInParameter(command, "ChequeNo", DbType.String, svo2.ChequeNo);
                            this.dbServer.AddInParameter(command, "ChequeDate", DbType.DateTime, svo2.ChequeDate);
                            this.dbServer.AddInParameter(command, "Payto", DbType.String, svo2.Payto);
                            this.dbServer.AddInParameter(command, "PaymentAmount", DbType.Double, svo2.PaymentAmount);
                            this.dbServer.AddInParameter(command, "TransferToAccountNo", DbType.Int64, svo2.TransferToAccountNo);
                            this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, details.CreatedUnitID);
                            this.dbServer.AddInParameter(command, "Status", DbType.Boolean, details.Status);
                            this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, details.AddedBy);
                            if (details.AddedOn != null)
                            {
                                details.AddedOn = details.AddedOn.Trim();
                            }
                            this.dbServer.AddInParameter(command, "AddedOn", DbType.String, details.AddedOn);
                            this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, details.AddedDateTime);
                            if (details.AddedWindowsLoginName != null)
                            {
                                details.AddedWindowsLoginName = details.AddedWindowsLoginName.Trim();
                            }
                            this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, details.AddedWindowsLoginName);
                            this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command, transaction);
                            svo2.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                        }
                    }
                }
                BizActionObj.SuccessStatus = 0;
                transaction.Commit();
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return BizActionObj;
        }
    }
}

