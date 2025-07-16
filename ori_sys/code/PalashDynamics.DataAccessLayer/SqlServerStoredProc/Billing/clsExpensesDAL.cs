using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing;
using PalashDynamics.ValueObjects;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Data.Common;
using System.Data;
using PalashDynamics.ValueObjects.Billing;
using System.Reflection;


namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsExpensesDAL : clsBaseExpensesDAL
    {
        #region Variables Declaration     
        private Database dbServer = null;     
        private LogManager logManager = null;
        #endregion
        private clsExpensesDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql object
                if (dbServer == null)
                {
                    dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
                if (logManager == null)
                {
                    logManager = LogManager.GetInstance();
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public override IValueObject AddExpenses(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddExpensesBizActionVO BizActionObj = valueObject as clsAddExpensesBizActionVO;


            if (BizActionObj.Details.ID == 0)

                BizActionObj = AddExpensesdetails(BizActionObj, UserVo);
            else
                BizActionObj = UpdateExpenseDetails(BizActionObj);

            return valueObject;
        }

        private clsAddExpensesBizActionVO AddExpensesdetails(clsAddExpensesBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed)
                    con.Open();
                trans = con.BeginTransaction();
                clsExpensesVO objDetailsVO = BizActionObj.Details;                
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddExpense");
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objDetailsVO.UnitID);
                dbServer.AddInParameter(command, "Expense", DbType.Int64, objDetailsVO.Expense);
                dbServer.AddInParameter(command, "ExpenseDate", DbType.DateTime, objDetailsVO.ExpenseDate);
                dbServer.AddInParameter(command, "Amount", DbType.Double, objDetailsVO.Amount);
                dbServer.AddInParameter(command, "VoucherCreatedBy", DbType.String, objDetailsVO.voucherCreatedby);
                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, objDetailsVO.IsFreezed);
                if (objDetailsVO.Remark != null)
                    objDetailsVO.Remark = objDetailsVO.Remark.Trim();
                dbServer.AddInParameter(command, "Remarks", DbType.String, objDetailsVO.Remark);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
                dbServer.AddParameter(command, "VoucherNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "????????????????????");
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus3 = dbServer.ExecuteNonQuery(command, trans);
                objDetailsVO.ID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.Details.ID = objDetailsVO.ID;
                objDetailsVO.VoucherNo = (string)dbServer.GetParameterValue(command, "VoucherNo");
                objDetailsVO.UnitID = UserVo.UserLoginInfo.UnitId;

                if (objDetailsVO.ExpenseDetails != null && objDetailsVO.ExpenseDetails.Count > 0)
                {
                    foreach (var item in objDetailsVO.ExpenseDetails)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddExpenseDetails");
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objDetailsVO.UnitID);
                        dbServer.AddInParameter(command1, "ExpenseID", DbType.Int64, objDetailsVO.ID);
                        dbServer.AddInParameter(command1, "ExpenseUnitId", DbType.Int64, objDetailsVO.UnitID);
                        dbServer.AddInParameter(command1, "PaymentMode", DbType.Int32, item.PaymentMode);
                        dbServer.AddInParameter(command1, "TransferToBank", DbType.Int64, item.TransferToBank);
                        dbServer.AddInParameter(command1, "ChequeNo", DbType.String, item.ChequeNo);
                        dbServer.AddInParameter(command1, "ChequeDate", DbType.DateTime, item.ChequeDate);
                        dbServer.AddInParameter(command1, "Payto", DbType.String, item.Payto);
                        dbServer.AddInParameter(command1, "PaymentAmount", DbType.Double, item.PaymentAmount);
                        dbServer.AddInParameter(command1, "TransferToAccountNo", DbType.Int64, item.TransferToAccountNo);
                        dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, objDetailsVO.CreatedUnitID);
                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, objDetailsVO.Status);
                        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                        if (objDetailsVO.AddedOn != null) objDetailsVO.AddedOn = objDetailsVO.AddedOn.Trim();
                        dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                        if (objDetailsVO.AddedWindowsLoginName != null) objDetailsVO.AddedWindowsLoginName = objDetailsVO.AddedWindowsLoginName.Trim();
                        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                        dbServer.ExecuteNonQuery(command1, trans);
                        item.ID = (long)dbServer.GetParameterValue(command, "ID");
                    }
                }
                BizActionObj.SuccessStatus = 0;
                trans.Commit();
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }
            return BizActionObj;
        }

        private clsAddExpensesBizActionVO UpdateExpenseDetails(clsAddExpensesBizActionVO BizActionObj)
        {
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed)
                    con.Open();
                trans = con.BeginTransaction();
                clsExpensesVO objDetailsVO = BizActionObj.Details;
                if (objDetailsVO.ID > 0)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateExpense");
                    dbServer.AddInParameter(command, "UnitId", DbType.Int64, objDetailsVO.UnitID);
                    dbServer.AddInParameter(command, "ID", DbType.Int64, objDetailsVO.ID);
                    dbServer.AddInParameter(command, "Expense", DbType.Int64, objDetailsVO.Expense);
                    dbServer.AddInParameter(command, "VoucherNo", DbType.String, objDetailsVO.VoucherNo);
                    dbServer.AddInParameter(command, "ExpenseDate", DbType.DateTime, objDetailsVO.ExpenseDate);
                    dbServer.AddInParameter(command, "Amount", DbType.Double, objDetailsVO.Amount);
                    dbServer.AddInParameter(command, "VoucherCreatedBy", DbType.String, objDetailsVO.voucherCreatedby);
                    dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, objDetailsVO.IsFreezed);
                    if (objDetailsVO.Remark != null)
                        objDetailsVO.Remark = objDetailsVO.Remark.Trim();
                    dbServer.AddInParameter(command, "Remarks", DbType.String, objDetailsVO.Remark);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);
                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objDetailsVO.CreatedUnitID);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objDetailsVO.AddedBy);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, objDetailsVO.AddedOn);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objDetailsVO.AddedWindowsLoginName);
                    int intStatus3 = dbServer.ExecuteNonQuery(command, trans);
                    if (objDetailsVO.ExpenseDetails != null && objDetailsVO.ExpenseDetails.Count > 0)
                    {
                        foreach (var item in objDetailsVO.ExpenseDetails)
                        {
                            DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddExpenseDetails");
                            dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objDetailsVO.UnitID);
                            dbServer.AddInParameter(command1, "ExpenseID", DbType.Int64, objDetailsVO.ID);

                            dbServer.AddInParameter(command1, "ExpenseUnitId", DbType.Int64, objDetailsVO.UnitID);
                            dbServer.AddInParameter(command1, "PaymentMode", DbType.Int32, item.PaymentMode);
                            dbServer.AddInParameter(command1, "TransferToBank", DbType.Int64, item.TransferToBank);
                            dbServer.AddInParameter(command1, "ChequeNo", DbType.String, item.ChequeNo);
                            dbServer.AddInParameter(command1, "ChequeDate", DbType.DateTime, item.ChequeDate);
                            dbServer.AddInParameter(command1, "Payto", DbType.String, item.Payto);
                            dbServer.AddInParameter(command1, "PaymentAmount", DbType.Double, item.PaymentAmount);
                            dbServer.AddInParameter(command1, "TransferToAccountNo", DbType.Int64, item.TransferToAccountNo);
                            dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, objDetailsVO.CreatedUnitID);

                            dbServer.AddInParameter(command1, "Status", DbType.Boolean, objDetailsVO.Status);
                            dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, objDetailsVO.AddedBy);
                            if (objDetailsVO.AddedOn != null) objDetailsVO.AddedOn = objDetailsVO.AddedOn.Trim();
                            dbServer.AddInParameter(command1, "AddedOn", DbType.String, objDetailsVO.AddedOn);
                            dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                            if (objDetailsVO.AddedWindowsLoginName != null) objDetailsVO.AddedWindowsLoginName = objDetailsVO.AddedWindowsLoginName.Trim();
                            dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, objDetailsVO.AddedWindowsLoginName);
                            dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                            dbServer.ExecuteNonQuery(command1, trans);
                            item.ID = (long)dbServer.GetParameterValue(command, "ID");
                        }
                    }
                }
                BizActionObj.SuccessStatus = 0;
                trans.Commit();
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }
            return BizActionObj;

        }
        public override IValueObject GetExpenses(IValueObject valueObject, clsUserVO Uservo)
        {
            clsGetExpensesListBizActionVO BizActionObj = valueObject as clsGetExpensesListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetExpense");
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "Expense", DbType.Int64, BizActionObj.Expense);
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                dbServer.AddInParameter(command, "Voucherno", DbType.String, BizActionObj.Voucherno);
                dbServer.AddInParameter(command, "Vouchercreatedby", DbType.String, BizActionObj.Vouchercreatedby);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.Details == null)
                        BizActionObj.Details = new List<clsExpensesVO>();
                    while (reader.Read())
                    {
                        clsExpensesVO objDoctorScheduleVO = new clsExpensesVO();
                        objDoctorScheduleVO.ID = (long)reader["ID"];
                        objDoctorScheduleVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        objDoctorScheduleVO.UnitName = (string)DALHelper.HandleDBNull(reader["UnitName"]);
                        objDoctorScheduleVO.Expense = (long)DALHelper.HandleDBNull(reader["Expense"]);
                        objDoctorScheduleVO.ExpenseName = (string)DALHelper.HandleDBNull(reader["ExpenseName"]);
                        objDoctorScheduleVO.Amount = (double)DALHelper.HandleDBNull(reader["Amount"]);
                        objDoctorScheduleVO.Remark = (string)DALHelper.HandleDBNull(reader["Remarks"]);
                        objDoctorScheduleVO.VoucherNo = (string)DALHelper.HandleDBNull(reader["VoucherNo"]);
                        objDoctorScheduleVO.voucherCreatedby = (string)DALHelper.HandleDBNull(reader["VoucherCreatedBy"]);
                        objDoctorScheduleVO.ExpenseDate = (DateTime)DALHelper.HandleDate(reader["ExpenseDate"]);
                        objDoctorScheduleVO.IsFreezed = (bool)DALHelper.HandleDBNull(reader["IsFreezed"]);
                        objDoctorScheduleVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        BizActionObj.Details.Add(objDoctorScheduleVO);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return BizActionObj;

        }
    }

}
