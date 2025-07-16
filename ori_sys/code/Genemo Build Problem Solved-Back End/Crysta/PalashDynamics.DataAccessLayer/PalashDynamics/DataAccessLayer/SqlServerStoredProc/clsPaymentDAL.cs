namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Billing;
    using PalashDynamics.ValueObjects.Master.CompanyPayment;
    using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Reflection;

    public class clsPaymentDAL : clsBasePaymentDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsPaymentDAL()
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
            clsAddPaymentBizActionVO bizActionObj = valueObject as clsAddPaymentBizActionVO;
            if (bizActionObj.Details.ID == 0L)
            {
                bizActionObj = this.AddDetails(bizActionObj, UserVo, null, null);
            }
            return valueObject;
        }

        public override IValueObject Add(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            clsAddPaymentBizActionVO bizActionObj = valueObject as clsAddPaymentBizActionVO;
            if (bizActionObj.Details.ID == 0L)
            {
                bizActionObj = this.AddDetails(bizActionObj, UserVo, pConnection, pTransaction);
            }
            return valueObject;
        }

        private clsAddPaymentBizActionVO AddDetails(clsAddPaymentBizActionVO BizActionObj, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            clsPaymentVO details = BizActionObj.Details;
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                connection = (pConnection == null) ? this.dbServer.CreateConnection() : pConnection;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = (pTransaction == null) ? connection.BeginTransaction() : pTransaction;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPayment");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, details.LinkServer);
                if (details.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, details.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, details.Date);
                if (details.ReceiptNo != null)
                {
                    details.ReceiptNo = details.ReceiptNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ReceiptNo", DbType.String, details.ReceiptNo);
                this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.Int64, details.BillID);
                this.dbServer.AddInParameter(storedProcCommand, "BillAmount", DbType.Double, details.BillAmount);
                this.dbServer.AddInParameter(storedProcCommand, "BillBalanceAmount", DbType.Double, details.BillBalanceAmount);
                this.dbServer.AddInParameter(storedProcCommand, "AdvanceID", DbType.Int64, details.AdvanceID);
                this.dbServer.AddInParameter(storedProcCommand, "AdvanceAmount", DbType.Double, details.AdvanceAmount);
                this.dbServer.AddInParameter(storedProcCommand, "AdvanceUsed", DbType.Double, details.AdvanceUsed);
                this.dbServer.AddInParameter(storedProcCommand, "RefundID", DbType.Int64, details.RefundID);
                this.dbServer.AddInParameter(storedProcCommand, "RefundAmount", DbType.Double, details.RefundAmount);
                this.dbServer.AddInParameter(storedProcCommand, "IsBillSettlement", DbType.Boolean, details.IsBillSettlement);
                if (details.Remarks != null)
                {
                    details.Remarks = details.Remarks.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, details.Remarks);
                if (details.PayeeNarration != null)
                {
                    details.PayeeNarration = details.PayeeNarration.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "PayeeNarration", DbType.String, details.PayeeNarration);
                this.dbServer.AddInParameter(storedProcCommand, "ChequeAuthorizedBy", DbType.Int64, details.ChequeAuthorizedBy);
                this.dbServer.AddInParameter(storedProcCommand, "CreditAuthorizedBy", DbType.Int64, details.CreditAuthorizedBy);
                this.dbServer.AddInParameter(storedProcCommand, "ComAdvAuthorizedBy", DbType.Int64, details.ComAdvAuthorizedBy);
                this.dbServer.AddInParameter(storedProcCommand, "IsPrinted", DbType.Boolean, details.IsPrinted);
                this.dbServer.AddInParameter(storedProcCommand, "IsCancelled", DbType.Boolean, details.IsCancelled);
                this.dbServer.AddInParameter(storedProcCommand, "CostingDivisionID", DbType.Int64, details.CostingDivisionID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, details.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, details.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, details.AddedWindowsLoginName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, details.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                BizActionObj.Details.ID = details.ID;
                details.UnitID = UserVo.UserLoginInfo.UnitId;
                foreach (clsPaymentDetailsVO svo in details.PaymentDetails)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddPaymentDetails");
                    this.dbServer.AddInParameter(command2, "LinkServer", DbType.String, details.LinkServer);
                    if (details.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(command2, "LinkServerAlias", DbType.String, details.LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddInParameter(command2, "PaymentID", DbType.Int64, details.ID);
                    this.dbServer.AddInParameter(command2, "PaymentModeID", DbType.Int64, svo.PaymentModeID);
                    if (svo.Number != null)
                    {
                        svo.Number = svo.Number.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "Number", DbType.String, svo.Number);
                    this.dbServer.AddInParameter(command2, "Date", DbType.DateTime, svo.Date);
                    this.dbServer.AddInParameter(command2, "BankID", DbType.Int64, svo.BankID);
                    this.dbServer.AddInParameter(command2, "AdvanceID", DbType.Int64, svo.AdvanceID);
                    this.dbServer.AddInParameter(command2, "AdvanceUsed", DbType.Double, details.AdvanceUsed);
                    this.dbServer.AddInParameter(command2, "PaidAmount", DbType.Double, svo.PaidAmount);
                    this.dbServer.AddInParameter(command2, "ChequeCleared", DbType.Boolean, svo.ChequeCleared);
                    if (svo.TransactionID != null)
                    {
                        svo.TransactionID = svo.TransactionID.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "TransactionID", DbType.String, svo.TransactionID);
                    this.dbServer.AddInParameter(command2, "UnitId", DbType.Int64, details.UnitID);
                    this.dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, details.CreatedUnitID);
                    this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, details.Status);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, details.AddedBy);
                    if (details.AddedOn != null)
                    {
                        details.AddedOn = details.AddedOn.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, details.AddedOn);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, details.AddedDateTime);
                    if (details.AddedWindowsLoginName != null)
                    {
                        details.AddedWindowsLoginName = details.AddedWindowsLoginName.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, details.AddedWindowsLoginName.Trim());
                    this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    svo.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                }
                if (pConnection == null)
                {
                    transaction.Commit();
                }
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception exception)
            {
                BizActionObj.SuccessStatus = -1;
                if (pConnection == null)
                {
                    transaction.Rollback();
                }
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
            }
            finally
            {
                if (pConnection == null)
                {
                    connection.Close();
                    connection = null;
                    transaction = null;
                }
            }
            return BizActionObj;
        }

        private clsAddPaymentBizActionVO AddDetailsWithTransaction(clsAddPaymentBizActionVO BizActionObj, clsUserVO UserVo)
        {
            clsPaymentVO details = BizActionObj.Details;
            DbTransaction transaction = null;
            DbConnection pConnection = this.dbServer.CreateConnection();
            try
            {
                pConnection.Open();
                transaction = pConnection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPayment");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, details.LinkServer);
                if (details.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, details.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, details.Date);
                if (details.ReceiptNo != null)
                {
                    details.ReceiptNo = details.ReceiptNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ReceiptNo", DbType.String, details.ReceiptNo);
                this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.Int64, details.BillID);
                this.dbServer.AddInParameter(storedProcCommand, "BillAmount", DbType.Double, details.BillAmount);
                this.dbServer.AddInParameter(storedProcCommand, "BillBalanceAmount", DbType.Double, details.BillBalanceAmount);
                this.dbServer.AddInParameter(storedProcCommand, "AdvanceID", DbType.Int64, details.AdvanceID);
                this.dbServer.AddInParameter(storedProcCommand, "AdvanceAmount", DbType.Double, details.AdvanceAmount);
                this.dbServer.AddInParameter(storedProcCommand, "AdvanceUsed", DbType.Double, details.AdvanceUsed);
                this.dbServer.AddInParameter(storedProcCommand, "RefundID", DbType.Int64, details.RefundID);
                this.dbServer.AddInParameter(storedProcCommand, "RefundAmount", DbType.Double, details.RefundAmount);
                this.dbServer.AddInParameter(storedProcCommand, "IsBillSettlement", DbType.Boolean, details.IsBillSettlement);
                if (details.Remarks != null)
                {
                    details.Remarks = details.Remarks.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, details.Remarks);
                if (details.PayeeNarration != null)
                {
                    details.PayeeNarration = details.PayeeNarration.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "PayeeNarration", DbType.String, details.PayeeNarration);
                this.dbServer.AddInParameter(storedProcCommand, "ChequeAuthorizedBy", DbType.Int64, details.ChequeAuthorizedBy);
                this.dbServer.AddInParameter(storedProcCommand, "CreditAuthorizedBy", DbType.Int64, details.CreditAuthorizedBy);
                this.dbServer.AddInParameter(storedProcCommand, "ComAdvAuthorizedBy", DbType.Int64, details.ComAdvAuthorizedBy);
                this.dbServer.AddInParameter(storedProcCommand, "IsPrinted", DbType.Boolean, details.IsPrinted);
                this.dbServer.AddInParameter(storedProcCommand, "IsCancelled", DbType.Boolean, details.IsCancelled);
                this.dbServer.AddInParameter(storedProcCommand, "CostingDivisionID", DbType.Int64, details.CostingDivisionID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, details.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, details.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, details.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                BizActionObj.Details.ID = details.ID;
                details.UnitID = UserVo.UserLoginInfo.UnitId;
                foreach (clsPaymentDetailsVO svo in details.PaymentDetails)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddPaymentDetails");
                    this.dbServer.AddInParameter(command2, "LinkServer", DbType.String, details.LinkServer);
                    if (details.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(command2, "LinkServerAlias", DbType.String, details.LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddInParameter(command2, "PaymentID", DbType.Int64, details.ID);
                    this.dbServer.AddInParameter(command2, "PaymentModeID", DbType.Int64, svo.PaymentModeID);
                    if (svo.Number != null)
                    {
                        svo.Number = svo.Number.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "Number", DbType.String, svo.Number);
                    this.dbServer.AddInParameter(command2, "Date", DbType.DateTime, svo.Date);
                    this.dbServer.AddInParameter(command2, "BankID", DbType.Int64, svo.BankID);
                    this.dbServer.AddInParameter(command2, "AdvanceID", DbType.Int64, svo.AdvanceID);
                    this.dbServer.AddInParameter(command2, "PaidAmount", DbType.Double, svo.PaidAmount);
                    this.dbServer.AddInParameter(command2, "ChequeCleared", DbType.Boolean, svo.ChequeCleared);
                    if (svo.TransactionID != null)
                    {
                        svo.TransactionID = svo.TransactionID.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "TransactionID", DbType.String, svo.TransactionID);
                    this.dbServer.AddInParameter(command2, "UnitId", DbType.Int64, details.UnitID);
                    this.dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, details.CreatedUnitID);
                    this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, details.Status);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, details.AddedBy);
                    if (details.AddedOn != null)
                    {
                        details.AddedOn = details.AddedOn.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, details.AddedOn);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, details.AddedDateTime);
                    if (details.AddedWindowsLoginName != null)
                    {
                        details.AddedWindowsLoginName = details.AddedWindowsLoginName.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, details.AddedWindowsLoginName.Trim());
                    this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    svo.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                }
                if (BizActionObj.isUpdateBillPaymentDetails)
                {
                    clsBaseBillDAL instance = clsBaseBillDAL.GetInstance();
                    BizActionObj.mybillPayDetails = (clsUpdateBillPaymentDtlsBizActionVO) instance.UpdateBillPaymentDetailsWithTransaction(BizActionObj.mybillPayDetails, UserVo, transaction, pConnection);
                    if (BizActionObj.mybillPayDetails.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                }
                transaction.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                throw;
            }
            finally
            {
                transaction.Dispose();
            }
            return BizActionObj;
        }

        public override IValueObject AddWithTransaction(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPaymentBizActionVO bizActionObj = valueObject as clsAddPaymentBizActionVO;
            if (bizActionObj.Details.ID == 0L)
            {
                bizActionObj = this.AddDetailsWithTransaction(bizActionObj, UserVo);
            }
            return valueObject;
        }

        public override IValueObject CompanySettlement(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCompanySettlementBizActionVO nvo = valueObject as clsCompanySettlementBizActionVO;
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
                double num = 0.0;
                clsPaymentVO paymentDetailobj = nvo.PaymentDetailobj;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPaymentForCompany");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, paymentDetailobj.LinkServer);
                if (paymentDetailobj.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, paymentDetailobj.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, paymentDetailobj.Date);
                if (paymentDetailobj.ReceiptNo != null)
                {
                    paymentDetailobj.ReceiptNo = paymentDetailobj.ReceiptNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ReceiptNo", DbType.String, paymentDetailobj.ReceiptNo);
                this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.Int64, paymentDetailobj.InvoiceID);
                this.dbServer.AddInParameter(storedProcCommand, "BillAmount", DbType.Double, paymentDetailobj.BillAmount);
                this.dbServer.AddInParameter(storedProcCommand, "TDSAmt", DbType.Double, paymentDetailobj.TDSAmt);
                this.dbServer.AddInParameter(storedProcCommand, "BillBalanceAmount", DbType.Double, paymentDetailobj.BillBalanceAmount);
                this.dbServer.AddInParameter(storedProcCommand, "AdvanceID", DbType.Int64, paymentDetailobj.AdvanceID);
                this.dbServer.AddInParameter(storedProcCommand, "AdvanceAmount", DbType.Double, paymentDetailobj.AdvanceAmount);
                this.dbServer.AddInParameter(storedProcCommand, "AdvanceUsed", DbType.Double, paymentDetailobj.AdvanceUsed);
                this.dbServer.AddInParameter(storedProcCommand, "RefundID", DbType.Int64, paymentDetailobj.RefundID);
                this.dbServer.AddInParameter(storedProcCommand, "RefundAmount", DbType.Double, paymentDetailobj.RefundAmount);
                this.dbServer.AddInParameter(storedProcCommand, "CostingDivisionID", DbType.Int64, paymentDetailobj.CostingDivisionID);
                this.dbServer.AddInParameter(storedProcCommand, "IsBillSettlement", DbType.Boolean, paymentDetailobj.IsBillSettlement);
                if (paymentDetailobj.Remarks != null)
                {
                    paymentDetailobj.Remarks = paymentDetailobj.Remarks.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, paymentDetailobj.Remarks);
                if (paymentDetailobj.PayeeNarration != null)
                {
                    paymentDetailobj.PayeeNarration = paymentDetailobj.PayeeNarration.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "PayeeNarration", DbType.String, paymentDetailobj.PayeeNarration);
                this.dbServer.AddInParameter(storedProcCommand, "ChequeAuthorizedBy", DbType.Int64, paymentDetailobj.ChequeAuthorizedBy);
                this.dbServer.AddInParameter(storedProcCommand, "CreditAuthorizedBy", DbType.Int64, paymentDetailobj.CreditAuthorizedBy);
                this.dbServer.AddInParameter(storedProcCommand, "ComAdvAuthorizedBy", DbType.Int64, paymentDetailobj.ComAdvAuthorizedBy);
                this.dbServer.AddInParameter(storedProcCommand, "IsPrinted", DbType.Boolean, paymentDetailobj.IsPrinted);
                this.dbServer.AddInParameter(storedProcCommand, "IsCancelled", DbType.Boolean, paymentDetailobj.IsCancelled);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, paymentDetailobj.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, paymentDetailobj.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, paymentDetailobj.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                paymentDetailobj.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                paymentDetailobj.UnitID = UserVo.UserLoginInfo.UnitId;
                foreach (clsPaymentDetailsVO svo in paymentDetailobj.PaymentDetails)
                {
                    double tempPaidAmount = paymentDetailobj.TempPaidAmount;
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddPaymentDetails");
                    this.dbServer.AddInParameter(command2, "LinkServer", DbType.String, paymentDetailobj.LinkServer);
                    if (paymentDetailobj.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(command2, "LinkServerAlias", DbType.String, paymentDetailobj.LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddInParameter(command2, "PaymentID", DbType.Int64, paymentDetailobj.ID);
                    this.dbServer.AddInParameter(command2, "PaymentModeID", DbType.Int64, svo.PaymentModeID);
                    if (svo.Number != null)
                    {
                        svo.Number = svo.Number.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "Number", DbType.String, svo.Number);
                    this.dbServer.AddInParameter(command2, "Date", DbType.DateTime, svo.Date);
                    this.dbServer.AddInParameter(command2, "BankID", DbType.Int64, svo.BankID);
                    this.dbServer.AddInParameter(command2, "AdvanceID", DbType.Int64, svo.AdvanceID);
                    this.dbServer.AddInParameter(command2, "PaidAmount", DbType.Double, svo.PaidAmount);
                    this.dbServer.AddInParameter(command2, "ChequeCleared", DbType.Boolean, svo.ChequeCleared);
                    this.dbServer.AddInParameter(command2, "UnitId", DbType.Int64, paymentDetailobj.UnitID);
                    this.dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, paymentDetailobj.CreatedUnitID);
                    this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, paymentDetailobj.Status);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, paymentDetailobj.AddedBy);
                    if (paymentDetailobj.AddedOn != null)
                    {
                        paymentDetailobj.AddedOn = paymentDetailobj.AddedOn.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, paymentDetailobj.AddedOn);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, paymentDetailobj.AddedDateTime);
                    if (paymentDetailobj.AddedWindowsLoginName != null)
                    {
                        paymentDetailobj.AddedWindowsLoginName = paymentDetailobj.AddedWindowsLoginName.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, paymentDetailobj.AddedWindowsLoginName.Trim());
                    this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    svo.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                }
                foreach (clsPaymentVO tvo2 in nvo.Details)
                {
                    if ((nvo.BillDetails != null) && (nvo.BillDetails.Count > 0))
                    {
                        foreach (clsCompanyPaymentDetailsVO svo2 in nvo.BillDetails)
                        {
                            clsCompanyPaymentDetailsVO svo3 = svo2;
                            if (svo2.ID == tvo2.BillID)
                            {
                                DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_UpdateBillPaymentDetails");
                                this.dbServer.AddInParameter(command3, "BillID", DbType.Int64, svo3.ID);
                                this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, svo3.UnitID);
                                if (num > 0.0)
                                {
                                    this.dbServer.AddInParameter(command3, "BalanceAmountSelf", DbType.Double, svo3.TempBalanceAmount - num);
                                }
                                else
                                {
                                    if (svo3.BalanceAmountSelf < 0.0)
                                    {
                                        svo3.BalanceAmountSelf = 0.0;
                                    }
                                    this.dbServer.AddInParameter(command3, "BalanceAmountSelf", DbType.Double, svo3.TempBalanceAmount);
                                }
                                this.dbServer.AddInParameter(command3, "TotalConcessionAmount", DbType.Double, svo3.TotalConcessionAmount);
                                this.dbServer.AddInParameter(command3, "NetBillAmount", DbType.Double, svo3.NetBillAmount);
                                this.dbServer.AddInParameter(command3, "UpdatedBy", DbType.Int64, UserVo.ID);
                                this.dbServer.AddInParameter(command3, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                this.dbServer.AddInParameter(command3, "UpdatedDateTime", DbType.DateTime, svo3.AddedDateTime);
                                this.dbServer.AddInParameter(command3, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(command3, transaction);
                            }
                        }
                        DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_UpdateCompanyInvoice");
                        this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, paymentDetailobj.UnitID);
                        this.dbServer.AddInParameter(command4, "PaidAmount", DbType.Double, nvo.InvoicePaidAmount);
                        this.dbServer.AddInParameter(command4, "BalanceAmountSelf", DbType.Double, nvo.InvoiceBalanceAmount);
                        this.dbServer.AddInParameter(command4, "TDSAmount", DbType.Double, nvo.InvoiceTDSAmount);
                        this.dbServer.AddInParameter(command4, "UpdatedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command4, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command4, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, paymentDetailobj.InvoiceID);
                        this.dbServer.ExecuteNonQuery(command4, transaction);
                        nvo.InvoiceID = (long) this.dbServer.GetParameterValue(command4, "ID");
                        nvo.InvoiceTDSAmount = 0.0;
                        nvo.InvoicePaidAmount = 0.0;
                    }
                    if ((nvo.ChargeDetails != null) && (nvo.ChargeDetails.Count > 0))
                    {
                        foreach (clsChargeVO evo in nvo.ChargeDetails)
                        {
                            if ((evo.BillID == tvo2.BillID) && (evo.BillUnitID == tvo2.UnitID))
                            {
                                evo.ChargeDetails = new clsChargeDetailsVO();
                                DbCommand command5 = this.dbServer.GetStoredProcCommand("CIMS_UpdateChargeForComapnyBIll");
                                this.dbServer.AddInParameter(command5, "ID", DbType.Int64, evo.ID);
                                this.dbServer.AddInParameter(command5, "Date", DbType.DateTime, DateTime.Now);
                                this.dbServer.AddInParameter(command5, "UnitID", DbType.Int64, evo.UnitID);
                                this.dbServer.AddInParameter(command5, "PaidAmount", DbType.Double, evo.TotalServicePaidAmount);
                                this.dbServer.AddInParameter(command5, "NetAmount", DbType.Double, evo.TotalNetAmount);
                                this.dbServer.AddInParameter(command5, "Concession", DbType.Double, evo.TotalConcession);
                                this.dbServer.AddInParameter(command5, "ConcessionPercentage", DbType.Double, evo.TotalConcessionPercentage);
                                this.dbServer.ExecuteNonQuery(command5, transaction);
                                evo.ID = (long) this.dbServer.GetParameterValue(command5, "ID");
                                if (evo.IsUpdate)
                                {
                                    DbCommand command6 = this.dbServer.GetStoredProcCommand("CIMS_UpdateChargeDetailsForCompanyBill");
                                    this.dbServer.AddInParameter(command6, "ChargeID ", DbType.Int64, evo.ID);
                                    this.dbServer.AddInParameter(command6, "Date", DbType.DateTime, DateTime.Now);
                                    this.dbServer.AddInParameter(command6, "UnitID", DbType.Int64, evo.UnitID);
                                    this.dbServer.AddInParameter(command6, "PaidAmount", DbType.Double, evo.ServicePaidAmount);
                                    this.dbServer.AddInParameter(command6, "Concession", DbType.Double, evo.Concession);
                                    this.dbServer.AddInParameter(command6, "ServiceTaxAmount", DbType.Double, evo.ServiceTaxAmount);
                                    this.dbServer.AddInParameter(command6, "NetAmount", DbType.Double, evo.SettleNetAmount);
                                    this.dbServer.AddInParameter(command6, "BalanceAmount", DbType.Double, evo.BalanceAmount);
                                    this.dbServer.AddInParameter(command6, "IsSameDate", DbType.Boolean, evo.IsSameDate);
                                    this.dbServer.ExecuteNonQuery(command6, transaction);
                                    continue;
                                }
                                DbCommand command7 = this.dbServer.GetStoredProcCommand("CIMS_AddChargeDetailsForCompanyBill");
                                this.dbServer.AddInParameter(command7, "ChargeID", DbType.Int64, evo.ID);
                                this.dbServer.AddInParameter(command7, "ChargeUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command7, "Date", DbType.DateTime, DateTime.Now);
                                this.dbServer.AddInParameter(command7, "Quantity", DbType.Double, evo.Quantity);
                                this.dbServer.AddInParameter(command7, "Rate", DbType.Double, evo.Rate);
                                this.dbServer.AddInParameter(command7, "TotalAmount", DbType.Double, evo.TotalAmount);
                                this.dbServer.AddInParameter(command7, "ConcessionAmount", DbType.Double, evo.Concession);
                                this.dbServer.AddInParameter(command7, "ServiceTaxAmount", DbType.Double, evo.ServiceTaxAmount);
                                this.dbServer.AddInParameter(command7, "NetAmount", DbType.Double, evo.SettleNetAmount);
                                this.dbServer.AddInParameter(command7, "PaidAmount", DbType.Double, evo.ServicePaidAmount);
                                this.dbServer.AddInParameter(command7, "BalanceAmount", DbType.Double, evo.BalanceAmount);
                                this.dbServer.AddInParameter(command7, "Status", DbType.Boolean, evo.Status);
                                this.dbServer.AddInParameter(command7, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command7, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command7, "AddedBy", DbType.Int64, UserVo.ID);
                                this.dbServer.AddInParameter(command7, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                this.dbServer.AddInParameter(command7, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                this.dbServer.AddInParameter(command7, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                this.dbServer.AddInParameter(command7, "RefundID", DbType.Int64, evo.RefundID);
                                this.dbServer.AddInParameter(command7, "RefundAmount", DbType.Double, evo.RefundAmount);
                                this.dbServer.AddParameter(command7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.ChargeDetails.ID);
                                this.dbServer.AddOutParameter(command7, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(command7, transaction);
                                evo.ChargeDetails.SuccessStatus = (int) this.dbServer.GetParameterValue(command7, "ResultStatus");
                                evo.ChargeDetails.ID = (long) this.dbServer.GetParameterValue(command7, "ID");
                            }
                        }
                    }
                }
                transaction.Commit();
                nvo.SuccessStatus = 0;
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                transaction.Rollback();
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject GetList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPaymentListBizActionVO nvo = (clsGetPaymentListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetReceiptList");
                this.dbServer.AddInParameter(storedProcCommand, "BillId", DbType.Int64, nvo.BillID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsPaymentVO>();
                    }
                    while (reader.Read())
                    {
                        try
                        {
                            clsPaymentVO item = new clsPaymentVO();
                            DateTime? nullable = DALHelper.HandleDate(reader["Date"]);
                            item.Date = new DateTime?(nullable.Value);
                            item.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                            item.UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]);
                            item.BillAmount = (double) DALHelper.HandleDBNull(reader["BillAmount"]);
                            item.PaidAmount = (double) DALHelper.HandleDBNull(reader["PaidAmount"]);
                            item.ReceiptNo = (string) DALHelper.HandleDBNull(reader["ReceiptNo"]);
                            nvo.List.Add(item);
                        }
                        catch (Exception)
                        {
                        }
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
    }
}

