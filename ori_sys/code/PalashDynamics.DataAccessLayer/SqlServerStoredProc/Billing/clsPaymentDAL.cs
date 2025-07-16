using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Billing;
using System.Data.Common;
using System.Data;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using System.Reflection;
using PalashDynamics.ValueObjects.Master.CompanyPayment;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsPaymentDAL : clsBasePaymentDAL
    {

        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsPaymentDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql object
                //Create Instance of the database object and BaseSql object
                if (dbServer == null)
                {
                    dbServer = HMSConfigurationManager.GetDatabaseReference();
                }

                //Create Instance of the LogManager object 
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

        public override IValueObject Add(IValueObject valueObject, clsUserVO UserVo)
        {

            clsAddPaymentBizActionVO BizActionObj = valueObject as clsAddPaymentBizActionVO;

            if (BizActionObj.Details.ID == 0)
                BizActionObj = AddDetails(BizActionObj, UserVo, null, null);
            //else
            // BizActionObj = UpdateDetails(BizActionObj);

            return valueObject;
        }

        public override IValueObject Add(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            clsAddPaymentBizActionVO BizActionObj = valueObject as clsAddPaymentBizActionVO;

            if (BizActionObj.Details.ID == 0)
                BizActionObj = AddDetails(BizActionObj, UserVo, pConnection, pTransaction);
            //else
            // BizActionObj = UpdateDetails(BizActionObj);

            return valueObject;
        }

        private clsAddPaymentBizActionVO AddDetails(clsAddPaymentBizActionVO BizActionObj, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            clsPaymentVO payDetails = BizActionObj.Details;

            DbConnection con = null;
            DbTransaction trans = null;

            try
            {
                if (pConnection != null) con = pConnection;
                else con = dbServer.CreateConnection();

                if (con.State == ConnectionState.Closed) con.Open();

                if (pTransaction != null) trans = pTransaction;
                else trans = con.BeginTransaction();


                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPayment");

                dbServer.AddInParameter(command, "LinkServer", DbType.String, payDetails.LinkServer);
                if (payDetails.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, payDetails.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "Date", DbType.DateTime, payDetails.Date);
                if (payDetails.ReceiptNo != null) payDetails.ReceiptNo = payDetails.ReceiptNo.Trim();
                dbServer.AddInParameter(command, "ReceiptNo", DbType.String, payDetails.ReceiptNo);

                dbServer.AddInParameter(command, "BillID", DbType.Int64, payDetails.BillID);
                dbServer.AddInParameter(command, "BillAmount", DbType.Double, payDetails.BillAmount);
                dbServer.AddInParameter(command, "BillBalanceAmount", DbType.Double, payDetails.BillBalanceAmount);
                dbServer.AddInParameter(command, "AdvanceID", DbType.Int64, payDetails.AdvanceID);
                dbServer.AddInParameter(command, "AdvanceAmount", DbType.Double, payDetails.AdvanceAmount);
                dbServer.AddInParameter(command, "AdvanceUsed", DbType.Double, payDetails.AdvanceUsed);
                dbServer.AddInParameter(command, "RefundID", DbType.Int64, payDetails.RefundID);
                dbServer.AddInParameter(command, "RefundAmount", DbType.Double, payDetails.RefundAmount);
                dbServer.AddInParameter(command, "IsBillSettlement", DbType.Boolean, payDetails.IsBillSettlement);

                if (payDetails.Remarks != null) payDetails.Remarks = payDetails.Remarks.Trim();
                dbServer.AddInParameter(command, "Remarks", DbType.String, payDetails.Remarks);
                if (payDetails.PayeeNarration != null) payDetails.PayeeNarration = payDetails.PayeeNarration.Trim();
                dbServer.AddInParameter(command, "PayeeNarration", DbType.String, payDetails.PayeeNarration);
                dbServer.AddInParameter(command, "ChequeAuthorizedBy", DbType.Int64, payDetails.ChequeAuthorizedBy);
                dbServer.AddInParameter(command, "CreditAuthorizedBy", DbType.Int64, payDetails.CreditAuthorizedBy);
                dbServer.AddInParameter(command, "ComAdvAuthorizedBy", DbType.Int64, payDetails.ComAdvAuthorizedBy);

                dbServer.AddInParameter(command, "IsPrinted", DbType.Boolean, payDetails.IsPrinted);
                dbServer.AddInParameter(command, "IsCancelled", DbType.Boolean, payDetails.IsCancelled);

                dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, payDetails.CostingDivisionID);  //Costing Divisions for Clinical & Pharmacy Billing
                ////dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, UserVo.UserLoginInfo.CashCounterID);  //Costing Divisions for Clinical & Pharmacy Billing    

                dbServer.AddInParameter(command, "Status", DbType.Boolean, payDetails.Status);

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, payDetails.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, payDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);


                int intStatus = 0;


                intStatus = dbServer.ExecuteNonQuery(command, trans);


                //payDetails.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                payDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.Details.ID = payDetails.ID;
                payDetails.UnitID = UserVo.UserLoginInfo.UnitId;
                foreach (var item in payDetails.PaymentDetails)
                {

                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPaymentDetails");

                    dbServer.AddInParameter(command1, "LinkServer", DbType.String, payDetails.LinkServer);
                    if (payDetails.LinkServer != null)
                        dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, payDetails.LinkServer.Replace(@"\", "_"));

                    dbServer.AddInParameter(command1, "PaymentID", DbType.Int64, payDetails.ID);
                    dbServer.AddInParameter(command1, "PaymentModeID", DbType.Int64, item.PaymentModeID);
                    if (item.Number != null) item.Number = item.Number.Trim();
                    dbServer.AddInParameter(command1, "Number", DbType.String, item.Number);
                    dbServer.AddInParameter(command1, "Date", DbType.DateTime, item.Date);
                    dbServer.AddInParameter(command1, "BankID", DbType.Int64, item.BankID);
                    dbServer.AddInParameter(command1, "AdvanceID", DbType.Int64, item.AdvanceID);
                    dbServer.AddInParameter(command1, "AdvanceUsed", DbType.Double, payDetails.AdvanceUsed);
                    dbServer.AddInParameter(command1, "PaidAmount", DbType.Double, item.PaidAmount);
                    dbServer.AddInParameter(command1, "ChequeCleared", DbType.Boolean, item.ChequeCleared);

                    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, payDetails.UnitID);
                    dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, payDetails.CreatedUnitID);

                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, payDetails.Status);
                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, payDetails.AddedBy);
                    if (payDetails.AddedOn != null) payDetails.AddedOn = payDetails.AddedOn.Trim();
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, payDetails.AddedOn);
                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, payDetails.AddedDateTime);
                    if (payDetails.AddedWindowsLoginName != null) payDetails.AddedWindowsLoginName = payDetails.AddedWindowsLoginName.Trim();
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, payDetails.AddedWindowsLoginName.Trim());

                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);



                    dbServer.ExecuteNonQuery(command1, trans);


                    //payDetails.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    item.ID = (long)dbServer.GetParameterValue(command, "ID");
                }

                if (pConnection == null) trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                if (pConnection == null) trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());


            }
            finally
            {
                //Always close your connections.
                if (pConnection == null)
                {
                    con.Close();
                    con = null;
                    trans = null;
                }
            }
            return BizActionObj;

        }

        public override IValueObject AddWithTransaction(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPaymentBizActionVO BizActionObj = valueObject as clsAddPaymentBizActionVO;

            if (BizActionObj.Details.ID == 0)
                BizActionObj = AddDetailsWithTransaction(BizActionObj, UserVo);

            return valueObject;
        }

        private clsAddPaymentBizActionVO AddDetailsWithTransaction(clsAddPaymentBizActionVO BizActionObj, clsUserVO UserVo)
        {
            clsPaymentVO payDetails = BizActionObj.Details;

            DbTransaction trans = null;
            DbConnection con = dbServer.CreateConnection();

            try
            {
                con.Open();
                trans = con.BeginTransaction();

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPayment");

                dbServer.AddInParameter(command, "LinkServer", DbType.String, payDetails.LinkServer);
                if (payDetails.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, payDetails.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "Date", DbType.DateTime, payDetails.Date);
                if (payDetails.ReceiptNo != null) payDetails.ReceiptNo = payDetails.ReceiptNo.Trim();
                dbServer.AddInParameter(command, "ReceiptNo", DbType.String, payDetails.ReceiptNo);

                dbServer.AddInParameter(command, "BillID", DbType.Int64, payDetails.BillID);
                dbServer.AddInParameter(command, "BillAmount", DbType.Double, payDetails.BillAmount);
                dbServer.AddInParameter(command, "BillBalanceAmount", DbType.Double, payDetails.BillBalanceAmount);
                dbServer.AddInParameter(command, "AdvanceID", DbType.Int64, payDetails.AdvanceID);
                dbServer.AddInParameter(command, "AdvanceAmount", DbType.Double, payDetails.AdvanceAmount);
                dbServer.AddInParameter(command, "AdvanceUsed", DbType.Double, payDetails.AdvanceUsed);
                dbServer.AddInParameter(command, "RefundID", DbType.Int64, payDetails.RefundID);
                dbServer.AddInParameter(command, "RefundAmount", DbType.Double, payDetails.RefundAmount);
                dbServer.AddInParameter(command, "IsBillSettlement", DbType.Boolean, payDetails.IsBillSettlement);

                if (payDetails.Remarks != null) payDetails.Remarks = payDetails.Remarks.Trim();
                dbServer.AddInParameter(command, "Remarks", DbType.String, payDetails.Remarks);
                if (payDetails.PayeeNarration != null) payDetails.PayeeNarration = payDetails.PayeeNarration.Trim();
                dbServer.AddInParameter(command, "PayeeNarration", DbType.String, payDetails.PayeeNarration);
                dbServer.AddInParameter(command, "ChequeAuthorizedBy", DbType.Int64, payDetails.ChequeAuthorizedBy);
                dbServer.AddInParameter(command, "CreditAuthorizedBy", DbType.Int64, payDetails.CreditAuthorizedBy);
                dbServer.AddInParameter(command, "ComAdvAuthorizedBy", DbType.Int64, payDetails.ComAdvAuthorizedBy);

                dbServer.AddInParameter(command, "IsPrinted", DbType.Boolean, payDetails.IsPrinted);
                dbServer.AddInParameter(command, "IsCancelled", DbType.Boolean, payDetails.IsCancelled);

                dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, payDetails.CostingDivisionID);  //Costing Divisions for Clinical & Pharmacy Billing
                //dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, UserVo.UserLoginInfo.CashCounterID);  //Costing Divisions for Clinical & Pharmacy Billing    

                dbServer.AddInParameter(command, "Status", DbType.Boolean, payDetails.Status);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, payDetails.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, payDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = 0;

                intStatus = dbServer.ExecuteNonQuery(command, trans);

                //payDetails.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                payDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.Details.ID = payDetails.ID;
                payDetails.UnitID = UserVo.UserLoginInfo.UnitId;
                foreach (var item in payDetails.PaymentDetails)
                {

                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPaymentDetails");

                    dbServer.AddInParameter(command1, "LinkServer", DbType.String, payDetails.LinkServer);
                    if (payDetails.LinkServer != null)
                        dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, payDetails.LinkServer.Replace(@"\", "_"));

                    dbServer.AddInParameter(command1, "PaymentID", DbType.Int64, payDetails.ID);
                    dbServer.AddInParameter(command1, "PaymentModeID", DbType.Int64, item.PaymentModeID);
                    if (item.Number != null) item.Number = item.Number.Trim();
                    dbServer.AddInParameter(command1, "Number", DbType.String, item.Number);
                    dbServer.AddInParameter(command1, "Date", DbType.DateTime, item.Date);
                    dbServer.AddInParameter(command1, "BankID", DbType.Int64, item.BankID);
                    dbServer.AddInParameter(command1, "AdvanceID", DbType.Int64, item.AdvanceID);
                    dbServer.AddInParameter(command1, "PaidAmount", DbType.Double, item.PaidAmount);
                    dbServer.AddInParameter(command1, "ChequeCleared", DbType.Boolean, item.ChequeCleared);

                    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, payDetails.UnitID);
                    dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, payDetails.CreatedUnitID);

                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, payDetails.Status);
                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, payDetails.AddedBy);
                    if (payDetails.AddedOn != null) payDetails.AddedOn = payDetails.AddedOn.Trim();
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, payDetails.AddedOn);
                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, payDetails.AddedDateTime);
                    if (payDetails.AddedWindowsLoginName != null) payDetails.AddedWindowsLoginName = payDetails.AddedWindowsLoginName.Trim();
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, payDetails.AddedWindowsLoginName.Trim());

                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                    dbServer.ExecuteNonQuery(command1, trans);

                    //payDetails.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    item.ID = (long)dbServer.GetParameterValue(command, "ID");
                }

                if (BizActionObj.isUpdateBillPaymentDetails == true)
                {
                    clsBaseBillDAL objBaseDAL = clsBaseBillDAL.GetInstance();
                    BizActionObj.mybillPayDetails = (clsUpdateBillPaymentDtlsBizActionVO)objBaseDAL.UpdateBillPaymentDetailsWithTransaction(BizActionObj.mybillPayDetails, UserVo, trans, con);
                    if (BizActionObj.mybillPayDetails.SuccessStatus == -1) throw new Exception();
                }

                //if (pConnection == null) 
                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw;

            }
            finally
            {
                trans.Dispose();
            }
            return BizActionObj;

        }


        public override IValueObject GetList(IValueObject valueObject, clsUserVO UserVo)
        {
            /// throw new NotImplementedException();
            /// 
            clsGetPaymentListBizActionVO BizActionObj = (clsGetPaymentListBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetReceiptList");
                dbServer.AddInParameter(command, "BillId", DbType.Int64, BizActionObj.BillID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsPaymentVO>();
                    while (reader.Read())
                    {
                        try
                        {
                            clsPaymentVO payDetails = new clsPaymentVO();
                            payDetails.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);
                            payDetails.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            payDetails.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                            payDetails.BillAmount = (double)DALHelper.HandleDBNull(reader["BillAmount"]);
                            payDetails.PaidAmount = (double)DALHelper.HandleDBNull(reader["PaidAmount"]);
                            payDetails.ReceiptNo = (string)DALHelper.HandleDBNull(reader["ReceiptNo"]);

                            BizActionObj.List.Add(payDetails);
                        }
                        catch (Exception)
                        {

                            // throw;
                        }
                    }
                }

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


        public override IValueObject CompanySettlement(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCompanySettlementBizActionVO BizActionObj = valueObject as clsCompanySettlementBizActionVO;
            DbConnection con = null;
            DbTransaction trans = null;

            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();
                double TDSamountNew = 0.0;
                double PaidAmt = 0;

                clsPaymentVO payDetails = BizActionObj.PaymentDetailobj;
;



                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPaymentForCompany");
                    dbServer.AddInParameter(command, "LinkServer", DbType.String, payDetails.LinkServer);
                    if (payDetails.LinkServer != null)
                        dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, payDetails.LinkServer.Replace(@"\", "_"));
                    dbServer.AddInParameter(command, "Date", DbType.DateTime, payDetails.Date);
                    if (payDetails.ReceiptNo != null) payDetails.ReceiptNo = payDetails.ReceiptNo.Trim();
                    dbServer.AddInParameter(command, "ReceiptNo", DbType.String, payDetails.ReceiptNo);
                    dbServer.AddInParameter(command, "BillID", DbType.Int64, payDetails.InvoiceID);
                    dbServer.AddInParameter(command, "BillAmount", DbType.Double, payDetails.BillAmount);
                    dbServer.AddInParameter(command, "TDSAmt", DbType.Double, payDetails.TDSAmt);
                  //  dbServer.AddInParameter(command, "TDSAmt", DbType.Double, payDetails.BillBalanceAmount);
                    dbServer.AddInParameter(command, "BillBalanceAmount", DbType.Double, payDetails.BillBalanceAmount);

                    //if (payDetails.TDSAmt > 0)
                    //{
                    //    TDSamountNew = payDetails.TDSAmt;
                    //    dbServer.AddInParameter(command, "BillBalanceAmount", DbType.Double, (payDetails.BillBalanceAmount - payDetails.TDSAmt));
                    //}
                    //else
                    //{
                    //    dbServer.AddInParameter(command, "BillBalanceAmount", DbType.Double, payDetails.BillBalanceAmount);
                    //}

                    dbServer.AddInParameter(command, "AdvanceID", DbType.Int64, payDetails.AdvanceID);
                    dbServer.AddInParameter(command, "AdvanceAmount", DbType.Double, payDetails.AdvanceAmount);
                    dbServer.AddInParameter(command, "AdvanceUsed", DbType.Double, payDetails.AdvanceUsed);
                    dbServer.AddInParameter(command, "RefundID", DbType.Int64, payDetails.RefundID);
                    dbServer.AddInParameter(command, "RefundAmount", DbType.Double, payDetails.RefundAmount);
                    dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, payDetails.CostingDivisionID);
                    dbServer.AddInParameter(command, "IsBillSettlement", DbType.Boolean, payDetails.IsBillSettlement);
                    if (payDetails.Remarks != null) payDetails.Remarks = payDetails.Remarks.Trim();
                    dbServer.AddInParameter(command, "Remarks", DbType.String, payDetails.Remarks);
                    if (payDetails.PayeeNarration != null) payDetails.PayeeNarration = payDetails.PayeeNarration.Trim();
                    dbServer.AddInParameter(command, "PayeeNarration", DbType.String, payDetails.PayeeNarration);
                    dbServer.AddInParameter(command, "ChequeAuthorizedBy", DbType.Int64, payDetails.ChequeAuthorizedBy);
                    dbServer.AddInParameter(command, "CreditAuthorizedBy", DbType.Int64, payDetails.CreditAuthorizedBy);
                    dbServer.AddInParameter(command, "ComAdvAuthorizedBy", DbType.Int64, payDetails.ComAdvAuthorizedBy);
                    dbServer.AddInParameter(command, "IsPrinted", DbType.Boolean, payDetails.IsPrinted);
                    dbServer.AddInParameter(command, "IsCancelled", DbType.Boolean, payDetails.IsCancelled);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, payDetails.Status);
                    dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, payDetails.AddedDateTime);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, payDetails.ID);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus = 0;
                    intStatus = dbServer.ExecuteNonQuery(command, trans);
                    payDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
                    payDetails.UnitID = UserVo.UserLoginInfo.UnitId;
                   
                    foreach (var item in payDetails.PaymentDetails)
                    {
                        PaidAmt = +payDetails.TempPaidAmount;
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPaymentDetails");
                        dbServer.AddInParameter(command1, "LinkServer", DbType.String, payDetails.LinkServer);
                        if (payDetails.LinkServer != null)
                            dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, payDetails.LinkServer.Replace(@"\", "_"));
                        dbServer.AddInParameter(command1, "PaymentID", DbType.Int64, payDetails.ID);
                        dbServer.AddInParameter(command1, "PaymentModeID", DbType.Int64, item.PaymentModeID);
                        if (item.Number != null) item.Number = item.Number.Trim();
                        dbServer.AddInParameter(command1, "Number", DbType.String, item.Number);
                        dbServer.AddInParameter(command1, "Date", DbType.DateTime, item.Date);
                        dbServer.AddInParameter(command1, "BankID", DbType.Int64, item.BankID);
                        dbServer.AddInParameter(command1, "AdvanceID", DbType.Int64, item.AdvanceID);
                        //  dbServer.AddInParameter(command1, "AdvanceUnitID", DbType.Int64, item.AdvanceUnitID);
                        dbServer.AddInParameter(command1, "PaidAmount", DbType.Double, item.PaidAmount);
                        dbServer.AddInParameter(command1, "ChequeCleared", DbType.Boolean, item.ChequeCleared);
                        dbServer.AddInParameter(command1, "UnitId", DbType.Int64, payDetails.UnitID);
                        dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, payDetails.CreatedUnitID);
                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, payDetails.Status);
                        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, payDetails.AddedBy);
                        if (payDetails.AddedOn != null) payDetails.AddedOn = payDetails.AddedOn.Trim();
                        dbServer.AddInParameter(command1, "AddedOn", DbType.String, payDetails.AddedOn);
                        dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, payDetails.AddedDateTime);
                        if (payDetails.AddedWindowsLoginName != null) payDetails.AddedWindowsLoginName = payDetails.AddedWindowsLoginName.Trim();
                        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, payDetails.AddedWindowsLoginName.Trim());
                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                        dbServer.ExecuteNonQuery(command1, trans);
                        item.ID = (long)dbServer.GetParameterValue(command1, "ID");


                    }
                    foreach (clsPaymentVO item3 in BizActionObj.Details)
                    {
                        if (BizActionObj.BillDetails != null && BizActionObj.BillDetails.Count > 0)
                        {
                            foreach (var item in BizActionObj.BillDetails)
                            {
                                clsCompanyPaymentDetailsVO objDetailsVO = item;
                                if (item.ID == item3.BillID) //&& item.UnitID == item3.UnitID) If Any problem occured during company payment with unitid please use second condition
                                {

                                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_UpdateBillPaymentDetails");

                                    dbServer.AddInParameter(command2, "BillID", DbType.Int64, objDetailsVO.ID);
                                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, objDetailsVO.UnitID);
                                    if (TDSamountNew > 0)
                                    {
                                        dbServer.AddInParameter(command2, "BalanceAmountSelf", DbType.Double, objDetailsVO.TempBalanceAmount - TDSamountNew);
                                    }
                                    else
                                    {
                                        if (objDetailsVO.BalanceAmountSelf < 0) objDetailsVO.BalanceAmountSelf = 0;
                                      dbServer.AddInParameter(command2, "BalanceAmountSelf", DbType.Double, objDetailsVO.TempBalanceAmount);
                                    }

                                    dbServer.AddInParameter(command2, "TotalConcessionAmount", DbType.Double, objDetailsVO.TotalConcessionAmount);
                                    dbServer.AddInParameter(command2, "NetBillAmount", DbType.Double, objDetailsVO.NetBillAmount);
                                    dbServer.AddInParameter(command2, "UpdatedBy", DbType.Int64, UserVo.ID);
                                    dbServer.AddInParameter(command2, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    dbServer.AddInParameter(command2, "UpdatedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                                    dbServer.AddInParameter(command2, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);
                                    int intStatus4 = dbServer.ExecuteNonQuery(command2, trans);

                                }

                            }

                            DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_UpdateCompanyInvoice");
                            //dbServer.AddInParameter(command3, "ID", DbType.Int64, payDetails.InvoiceID);
                            dbServer.AddInParameter(command3, "UnitID", DbType.Int64, payDetails.UnitID);
                            dbServer.AddInParameter(command3, "PaidAmount", DbType.Double, BizActionObj.InvoicePaidAmount);
                            dbServer.AddInParameter(command3, "BalanceAmountSelf", DbType.Double, BizActionObj.InvoiceBalanceAmount);
                            dbServer.AddInParameter(command3, "TDSAmount", DbType.Double, BizActionObj.InvoiceTDSAmount);
                            dbServer.AddInParameter(command3, "UpdatedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command3, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command3, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, payDetails.InvoiceID);
                            int intStatus5 = dbServer.ExecuteNonQuery(command3, trans);
                            BizActionObj.InvoiceID = (long)dbServer.GetParameterValue(command3, "ID");
                            BizActionObj.InvoiceTDSAmount = 0.0;
                            BizActionObj.InvoicePaidAmount = 0.0;
                        }



                        if (BizActionObj.ChargeDetails != null && BizActionObj.ChargeDetails.Count > 0)
                        {
                            foreach (var item in BizActionObj.ChargeDetails)
                            {

                                if (item.BillID == item3.BillID && item.BillUnitID == item3.UnitID)
                                {
                                    item.ChargeDetails = new clsChargeDetailsVO();
                                    //Add service details in t_charge
                                    DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_UpdateChargeForComapnyBIll");
                                    dbServer.AddInParameter(command4, "ID", DbType.Int64, item.ID);
                                    dbServer.AddInParameter(command4, "Date", DbType.DateTime, DateTime.Now);
                                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, item.UnitID);
                                    dbServer.AddInParameter(command4, "PaidAmount", DbType.Double, item.TotalServicePaidAmount);
                                    dbServer.AddInParameter(command4, "NetAmount", DbType.Double, item.TotalNetAmount);
                                    dbServer.AddInParameter(command4, "Concession", DbType.Double, item.TotalConcession);
                                    dbServer.AddInParameter(command4, "ConcessionPercentage", DbType.Double, item.TotalConcessionPercentage);

                                    int iStatus = dbServer.ExecuteNonQuery(command4, trans);
                                    item.ID = (long)dbServer.GetParameterValue(command4, "ID");


                                    if (item.IsUpdate == true)
                                    {
                                        DbCommand command5 = dbServer.GetStoredProcCommand("CIMS_UpdateChargeDetailsForCompanyBill");
                                        dbServer.AddInParameter(command5, "ChargeID ", DbType.Int64, item.ID);
                                        dbServer.AddInParameter(command5, "Date", DbType.DateTime, DateTime.Now);
                                        dbServer.AddInParameter(command5, "UnitID", DbType.Int64, item.UnitID);
                                        dbServer.AddInParameter(command5, "PaidAmount", DbType.Double, item.ServicePaidAmount);
                                        dbServer.AddInParameter(command5, "Concession", DbType.Double, item.Concession);
                                        dbServer.AddInParameter(command5, "ServiceTaxAmount", DbType.Double, item.ServiceTaxAmount);
                                        dbServer.AddInParameter(command5, "NetAmount", DbType.Double, item.SettleNetAmount);
                                        dbServer.AddInParameter(command5, "BalanceAmount", DbType.Double, item.BalanceAmount);
                                        dbServer.AddInParameter(command5, "IsSameDate", DbType.Boolean, item.IsSameDate);
                                        int iStatus4 = dbServer.ExecuteNonQuery(command5, trans);

                                    }
                                    else
                                    {

                                        //Add service payment details in t_chargeDetails
                                        DbCommand command6 = dbServer.GetStoredProcCommand("CIMS_AddChargeDetailsForCompanyBill");
                                        dbServer.AddInParameter(command6, "ChargeID", DbType.Int64, item.ID);
                                        dbServer.AddInParameter(command6, "ChargeUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                        dbServer.AddInParameter(command6, "Date", DbType.DateTime, DateTime.Now);
                                        dbServer.AddInParameter(command6, "Quantity", DbType.Double, item.Quantity);
                                        dbServer.AddInParameter(command6, "Rate", DbType.Double, item.Rate);
                                        dbServer.AddInParameter(command6, "TotalAmount", DbType.Double, item.TotalAmount);
                                        dbServer.AddInParameter(command6, "ConcessionAmount", DbType.Double, item.Concession);
                                        dbServer.AddInParameter(command6, "ServiceTaxAmount", DbType.Double, item.ServiceTaxAmount);
                                        dbServer.AddInParameter(command6, "NetAmount", DbType.Double, item.SettleNetAmount);
                                        dbServer.AddInParameter(command6, "PaidAmount", DbType.Double, item.ServicePaidAmount);
                                        dbServer.AddInParameter(command6, "BalanceAmount", DbType.Double, item.BalanceAmount);
                                        dbServer.AddInParameter(command6, "Status", DbType.Boolean, item.Status);
                                        dbServer.AddInParameter(command6, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                        dbServer.AddInParameter(command6, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                        dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
                                        dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                        dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                        dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                        dbServer.AddInParameter(command6, "RefundID", DbType.Int64, item.RefundID);
                                        dbServer.AddInParameter(command6, "RefundAmount", DbType.Double, item.RefundAmount);

                                        dbServer.AddParameter(command6, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ChargeDetails.ID);
                                        dbServer.AddOutParameter(command6, "ResultStatus", DbType.Int32, int.MaxValue);
                                        int intStatus2 = dbServer.ExecuteNonQuery(command6, trans);
                                        item.ChargeDetails.SuccessStatus = (int)dbServer.GetParameterValue(command6, "ResultStatus");
                                        item.ChargeDetails.ID = (long)dbServer.GetParameterValue(command6, "ID");
                                    }
                                }
                            }



                        }
                    }

                trans.Commit();
                BizActionObj.SuccessStatus = 0;
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


    }

}
