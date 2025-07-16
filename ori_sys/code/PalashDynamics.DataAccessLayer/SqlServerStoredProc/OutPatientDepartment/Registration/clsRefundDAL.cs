using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using System.Data.Common;
using System.Data;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.ValueObjects.Radiology;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.Log;
using System.Reflection;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsRefundDAL : clsBaseRefundDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        bool IsAuditTrail = false;
        #endregion


        private clsRefundDAL()
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
                IsAuditTrail = Convert.ToBoolean(HMSConfigurationManager.GetValueFromApplicationConfig("IsAuditTrail")); // By Umesh For Enable/Disable Audit Trail
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        private clsAddRefundBizActionVO AddDetails(clsAddRefundBizActionVO BizActionObj, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            DbConnection con = null;
            DbTransaction trans = null;

            try
            {

                //===================================
                if (pConnection != null) con = pConnection;
                else
                    con = dbServer.CreateConnection();

                if (con.State == ConnectionState.Closed) con.Open();

                if (pTransaction != null) trans = pTransaction;
                else trans = con.BeginTransaction();
                //===================================

                clsRefundVO objDetailsVO = BizActionObj.Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddRefund");
                command.Connection = con;

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                if (objDetailsVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objDetailsVO.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objDetailsVO.PatientUnitID);
                dbServer.AddInParameter(command, "ItemSaleReturnID", DbType.Int64, objDetailsVO.ItemSaleReturnID);
                dbServer.AddInParameter(command, "CompanyID", DbType.Int64, objDetailsVO.CompanyID);
                dbServer.AddInParameter(command, "BillID", DbType.Int64, objDetailsVO.BillID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);
                dbServer.AddInParameter(command, "Amount", DbType.Double, objDetailsVO.Amount);
                dbServer.AddInParameter(command, "AdvanceId", DbType.Int64, objDetailsVO.AdvanceID);
                dbServer.AddInParameter(command, "ReceiptPrinted", DbType.Boolean, objDetailsVO.ReceiptPrinted);
                if (objDetailsVO.Remarks != null) objDetailsVO.Remarks = objDetailsVO.Remarks.Trim();
                dbServer.AddInParameter(command, "Remarks", DbType.String, objDetailsVO.Remarks);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);  //Added By CDS For Cash Counter
                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "ApprovalRequestID", DbType.Int64, objDetailsVO.ApprovalRequestID);//Added By Bhushanp 01062017
                dbServer.AddInParameter(command, "ApprovalRequestUnitID", DbType.Int64, objDetailsVO.ApprovalRequestUnitID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddOutParameter(command, "ID", DbType.Int64, int.MaxValue);
                dbServer.AddParameter(command, "ReceiptNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "00000000000000000000000000000000000000000000000000000000000000000000");
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Details.ID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.Details.ReceiptNo = Convert.ToString(dbServer.GetParameterValue(command, "ReceiptNo"));
                if (BizActionObj.Details.PaymentDetails != null)
                {
                    clsBasePaymentDAL objBaseDAL = clsBasePaymentDAL.GetInstance();
                    clsAddPaymentBizActionVO obj = new clsAddPaymentBizActionVO();

                    obj.Details = new clsPaymentVO();
                    obj.Details = BizActionObj.Details.PaymentDetails;
                    obj.Details.RefundID = BizActionObj.Details.ID;
                    obj.Details.RefundAmount = objDetailsVO.Amount;
                    obj.Details.Date = BizActionObj.Details.Date;
                    obj.Details.LinkServer = BizActionObj.Details.LinkServer;
                    obj.Details.CostingDivisionID = BizActionObj.Details.CostingDivisionID;
                    obj = (clsAddPaymentBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);
                    if (obj.SuccessStatus == -1) throw new Exception();
                    BizActionObj.Details.PaymentDetails.ID = obj.Details.ID;
                    #region 20042017 Refund To Advance

                    if (BizActionObj.IsRefundToAdvance == true)
                    {

                        //############################### Begin Advance ###################################################

                        clsBaseAdvanceDAL objBaseAdvDAL = clsBaseAdvanceDAL.GetInstance();
                        clsAddAdvanceBizActionVO objAdvanceFromRefund = new clsAddAdvanceBizActionVO();
                        objAdvanceFromRefund.IsRefundToAdvance = BizActionObj.IsRefundToAdvance;
                        objAdvanceFromRefund.BillID = objDetailsVO.BillID;
                        objAdvanceFromRefund.Details = new clsAdvanceVO();
                        objAdvanceFromRefund.Details.PatientID = BizActionObj.RefundToAdvancePatientID;
                        objAdvanceFromRefund.Details.PatientUnitID = BizActionObj.RefundToAdvancePatientUnitID;
                        //objAdvanceFromRefund.Details.CompanyID = 0;
                        objAdvanceFromRefund.Details.Date = objDetailsVO.Date;
                        objAdvanceFromRefund.Details.Total = objDetailsVO.Amount;
                        objAdvanceFromRefund.Details.AdvanceTypeId = 2;
                        //objAdvanceFromRefund.Details.Used = 0;
                        //objAdvanceFromRefund.Details.Refund = 0;
                        objAdvanceFromRefund.Details.Balance = objDetailsVO.Amount;
                        objAdvanceFromRefund.Details.AdvanceAgainstId = BizActionObj.IsExchangeMaterial == true ? 3 : 1;  //1==OPD, 3==Pharamacy
                        objAdvanceFromRefund.Details.Remarks = BizActionObj.Details.ReceiptNo;
                        objAdvanceFromRefund.Details.Status = objDetailsVO.Status;
                        //objAdvanceFromRefund.Details.UnitID = UserVo.UserLoginInfo.UnitId;
                        objAdvanceFromRefund.Details.CostingDivisionID = objDetailsVO.CostingDivisionID;

                        objAdvanceFromRefund.Details.FromRefundID = BizActionObj.Details.ID;

                        //############################### End Advance #####################################################

                        //+++++++++++++++++++++++++++++++ Begin Advance Payment +++++++++++++++++++++++++++++++++++++++++++

                        //clsBasePaymentDAL objBasePayDAL = clsBasePaymentDAL.GetInstance();
                        clsAddPaymentBizActionVO objAdvPayment = new clsAddPaymentBizActionVO();

                        objAdvPayment.Details = new clsPaymentVO();
                        objAdvPayment.Details.Date = obj.Details.Date;
                        //objAdvPayment.Details.ReceiptNo = "";
                        //objAdvPayment.Details.BillID = 0;
                        //objAdvPayment.Details.BillAmount = 0;
                        //objAdvPayment.Details.BillBalanceAmount = 0;
                        objAdvPayment.Details.AdvanceID = objAdvanceFromRefund.Details.ID;
                        objAdvPayment.Details.AdvanceAmount = obj.Details.RefundAmount;
                        //objAdvPayment.Details.AdvanceUsed = 0;
                        //objAdvPayment.Details.RefundID = 0;
                        //objAdvPayment.Details.RefundAmount = 0;
                        //objAdvPayment.Details.IsBillSettlement = false;
                        objAdvPayment.Details.Remarks = BizActionObj.Details.ReceiptNo;
                        objAdvPayment.Details.CostingDivisionID = objDetailsVO.CostingDivisionID;
                        objAdvPayment.Details.LinkServer = BizActionObj.Details.LinkServer;

                        //============================= Begin Advance Pament Details =======================================

                        objAdvPayment.Details.PaymentDetails = new List<clsPaymentDetailsVO>();
                        clsPaymentDetailsVO objPaymentDetailsVO = new clsPaymentDetailsVO();

                        objPaymentDetailsVO.LinkServer = BizActionObj.Details.LinkServer;
                        //objPaymentDetailsVO.PaymentID = objAdvPayment.Details.ID;
                        objPaymentDetailsVO.PaymentModeID = objDetailsVO.PaymentDetails.PaymentDetails[0].PaymentModeID;
                        objPaymentDetailsVO.PaidAmount = obj.Details.RefundAmount;
                        objPaymentDetailsVO.Status = objDetailsVO.Status;

                        objAdvPayment.Details.PaymentDetails.Add(objPaymentDetailsVO);

                        objAdvPayment.Details.CostingDivisionID = BizActionObj.Details.CostingDivisionID;

                        objAdvanceFromRefund.Details.PaymentDetails = objAdvPayment.Details;

                        //============================= End Advance Pament Details =========================================

                        objAdvanceFromRefund = (clsAddAdvanceBizActionVO)objBaseAdvDAL.AddAdvance(objAdvanceFromRefund, UserVo, con, trans);
                        if (objAdvanceFromRefund.SuccessStatus == -1) throw new Exception();

                        //objAdvPayment = (clsAddPaymentBizActionVO)objBasePayDAL.Add(objAdvPayment, UserVo, con, trans);
                        //if (objAdvPayment.SuccessStatus == -1) throw new Exception();


                        //+++++++++++++++++++++++++++++++ End Advance Payment ++++++++++++++++++++++++++++++++++++++++++++++

                    }
                    #endregion

                    if (IsAuditTrail && BizActionObj.LogInfoList != null)   // By Umesh For Audit Trail
                    {
                        LogInfo LogInformation = new LogInfo();
                        LogInformation.guid = new Guid();
                        LogInformation.UserId = UserVo.ID;
                        LogInformation.TimeStamp = DateTime.Now;
                        LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                        LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                        LogInformation.Message = " 28 : Data After Refund Services  " + "\r\n"
                                                + "User Unit Id : " + Convert.ToString(UserVo.UserLoginInfo.UnitId) + " "
                                                + "Patient ID : " + Convert.ToString(objDetailsVO.PatientID) + " "
                                                + "Bill ID : " + Convert.ToString(objDetailsVO.BillID) + " "
                                                + "Costing Division ID : " + Convert.ToString(objDetailsVO.CostingDivisionID) + " "
                                                + "Amount : " + Convert.ToString(objDetailsVO.Amount) + " "
                                                + "Advance ID : " + Convert.ToString(objDetailsVO.AdvanceID) + " "
                                                + "Remarks : " + Convert.ToString(objDetailsVO.Remarks) + " "
                                                + "Refund ID : " + Convert.ToString(BizActionObj.Details.ID) + " "
                                                + "Receipt No : " + Convert.ToString(BizActionObj.Details.ReceiptNo) + " "
                                                ;
                        LogInformation.Message = LogInformation.Message.Replace("\r\n", Environment.NewLine);
                        BizActionObj.LogInfoList.Add(LogInformation);
                    }
                }

                if (pConnection == null) trans.Commit();
                BizActionObj.SuccessStatus = 0;
                if (BizActionObj.LogInfoList != null)              // By Umesh for activity log
                {
                    if (BizActionObj.LogInfoList.Count > 0 && IsAuditTrail == true)
                    {
                        SetLogInfo(BizActionObj.LogInfoList, UserVo.ID);
                        BizActionObj.LogInfoList.Clear();
                    }
                }

            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                if (pConnection == null) trans.Rollback();
            }
            finally
            {
                if (pConnection == null)
                {
                    con.Close();
                    con = null;
                    trans = null;
                }
            }

            return BizActionObj;
        }

        public override IValueObject SendApprovalRequest(IValueObject valueObject, clsUserVO UserVo)
        {
            clsSendRequestForApprovalVO BizActionObj = valueObject as clsSendRequestForApprovalVO;
            BizActionObj = SendApprovalRequestDetails(BizActionObj, UserVo, null, null);
            return valueObject;
        }

        public override IValueObject ApproveRefundRequest(IValueObject valueObject, clsUserVO UserVo)
        {
            clsApproveSendRequestVO BizActionObj = valueObject as clsApproveSendRequestVO;
            BizActionObj = ApproveRefundRequestDetails(BizActionObj, UserVo, null, null);
            return valueObject;
        }

        public clsSendRequestForApprovalVO SendApprovalRequestDetails(clsSendRequestForApprovalVO BizActionObj, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            //clsSendRequestForApprovalVO BizActionObj = valueObject as clsSendRequestForApprovalVO;

            DbConnection con = null;
            DbTransaction trans = null;

            try
            {
                if (pConnection != null) con = pConnection;
                else
                    con = dbServer.CreateConnection();

                if (con.State == ConnectionState.Closed) con.Open();

                if (pTransaction != null) trans = pTransaction;
                else trans = con.BeginTransaction();

                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddRefundApprovalRequest");
                command1.Connection = con;
                dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.Details.ApprovalRequestID);
                dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command1, "IsOPDBill", DbType.Boolean, BizActionObj.Details.IsOPDBill);
                dbServer.AddInParameter(command1, "RequestTypeID", DbType.Int64, BizActionObj.Details.RequestTypeID);
                dbServer.AddInParameter(command1, "RequestType", DbType.String, BizActionObj.Details.RequestType);
                dbServer.AddInParameter(command1, "BillID", DbType.Int64, BizActionObj.Details.BillID);
                dbServer.AddInParameter(command1, "BillUnitID", DbType.Int64, BizActionObj.Details.BillUnitID);
                dbServer.AddInParameter(command1, "ApprovalDate", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command1, "Amount", DbType.Double, BizActionObj.RefundAmount);
                dbServer.AddInParameter(command1, "IsAgainstBill", DbType.Boolean, BizActionObj.IsAgainstBill);
                dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
                int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                BizActionObj.Details.ApprovalRequestID = (long)dbServer.GetParameterValue(command1, "ID");
                BizActionObj.Details.ApprovalRequestUnitID = UserVo.UserLoginInfo.UnitId;

                # region // For Audit Trail
                LogInfo LogInformation = new LogInfo();
                if (IsAuditTrail && BizActionObj.LogInfoList != null)   // By Umesh For Audit Trail
                {
                    LogInformation.guid = new Guid();
                    LogInformation.UserId = UserVo.ID;
                    LogInformation.TimeStamp = DateTime.Now;
                    LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                    LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                    LogInformation.Message = " 22 : Cancel Service data In DAL  "
                                            + "User Unit Id : " + Convert.ToString(UserVo.UserLoginInfo.UnitId) + " "
                                            + "Bill ID : " + Convert.ToString(BizActionObj.Details.BillID) + " "
                                            + "Bill Unit ID : " + Convert.ToString(BizActionObj.Details.BillUnitID) + " "
                                            + "Is OPD Bill : " + Convert.ToString(BizActionObj.Details.IsOPDBill) + " "
                                            + "RequestType : " + Convert.ToString(BizActionObj.Details.RequestType) + " "
                                            + "ApprovalDate :" + Convert.ToString(DateTime.Now) + " "
                                            ;
                }
                #endregion

                foreach (var item in BizActionObj.ChargeList)
                {
                    if (IsAuditTrail && BizActionObj.LogInfoList != null)   // By Umesh For Audit Trail
                    {
                        LogInformation.Message = LogInformation.Message
                                                        + "Approval RequestID : " + Convert.ToString(BizActionObj.Details.ApprovalRequestID) + " "
                                                        + "Approval Request UnitID : " + Convert.ToString(BizActionObj.Details.ApprovalRequestUnitID) + " "
                                                        + "Approval Status : " + Convert.ToString("false") + " "
                                                        + "Cancel Service : " + Convert.ToString(item.SelectCharge) + " "
                                                        + "Approval Remark : " + Convert.ToString(item.ApprovalRemark) + " "
                                                        + "Request Remark : " + Convert.ToString(item.ApprovalRequestRemark) + " "
                                                        + "Service Code : " + Convert.ToString(item.ServiceId) + " "
                                                        + "Service Name : " + Convert.ToString(item.ServiceName) + " "
                                                        + "\r\n"
                                                ;
                    }
                    if (IsAuditTrail && BizActionObj.LogInfoList != null)
                    {
                        LogInformation.Message = LogInformation.Message.Replace("\r\n", Environment.NewLine);
                        BizActionObj.LogInfoList.Add(LogInformation);
                    }
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddRefundChargeApprovalDetails");
                    dbServer.AddInParameter(command, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command, "IsRefundRequest", DbType.Boolean, BizActionObj.IsRefundRequest);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "ChargeID", DbType.Int64, item.ID);
                    dbServer.AddInParameter(command, "ChargeUnitID", DbType.Int64, item.UnitID);
                    dbServer.AddInParameter(command, "ApprovalRequestID", DbType.Int64, BizActionObj.Details.ApprovalRequestID);
                    dbServer.AddInParameter(command, "ApprovalRequestUnitID", DbType.Int64, BizActionObj.Details.ApprovalRequestUnitID);
                    dbServer.AddInParameter(command, "ApprovalStatus", DbType.Boolean, false);
                    dbServer.AddInParameter(command, "ServiceID", DbType.Int64, item.ServiceId);

                    dbServer.AddInParameter(command, "ApprovalRequestRemarkID", DbType.Int64, item.SelectedRequestRefundReason.ID);
                    dbServer.AddInParameter(command, "ApprovalRequestRemark", DbType.String, item.SelectedRequestRefundReason.Description); //item.ApprovalRequestRemark);

                    dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    BizActionObj.SuccessStatus = Convert.ToInt32(dbServer.GetParameterValue(command, "ResultStatus"));

                    if (BizActionObj.IsRefundRequest != true)
                        break;
                }

                if (pConnection == null) trans.Commit();
                BizActionObj.SuccessStatus = 0;
                if (IsAuditTrail && BizActionObj.LogInfoList != null)              // By Umesh for activity log
                {
                    if (BizActionObj.LogInfoList.Count > 0 && IsAuditTrail == true)
                    {
                        SetLogInfo(BizActionObj.LogInfoList, UserVo.ID);
                        BizActionObj.LogInfoList.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                BizActionObj.SuccessStatus = -1;
                if (pConnection == null) trans.Rollback();
            }
            finally
            {
                if (pConnection == null)
                {
                    con.Close();
                    con = null;
                    trans = null;
                }
            }

            return BizActionObj;
        }

        public clsApproveSendRequestVO ApproveRefundRequestDetails(clsApproveSendRequestVO BizActionObj, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            DbConnection con = null;
            DbTransaction trans = null;

            try
            {
                if (pConnection != null) con = pConnection;
                else
                    con = dbServer.CreateConnection();

                if (con.State == ConnectionState.Closed) con.Open();

                if (pTransaction != null) trans = pTransaction;
                else trans = con.BeginTransaction();

                //  DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateRefundChargeApprovalDetails");
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddApprovedRequest");
                command.Connection = con;
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "ApprovalRequestID", DbType.Int64, BizActionObj.ApprovalRequestID);
                dbServer.AddInParameter(command, "ApprovalRequestUnitID", DbType.Int64, BizActionObj.ApprovalRequestUnitID);
                dbServer.AddInParameter(command, "ApprovalRemark", DbType.String, BizActionObj.ApprovalRemark);
                dbServer.AddInParameter(command, "LevelID", DbType.Int64, BizActionObj.LevelID);
                dbServer.AddInParameter(command, "Amount", DbType.Int64, BizActionObj.RefundAmount);
                dbServer.AddInParameter(command, "IsApproved", DbType.Boolean, BizActionObj.IsApproved);
                dbServer.AddInParameter(command, "ApprovedBy", DbType.Int64, BizActionObj.ApprovedByID);
                dbServer.AddInParameter(command, "ApprovedByName", DbType.String, BizActionObj.ApprovedByName);
                dbServer.AddInParameter(command, "ApprovedDateTime", DbType.DateTime, BizActionObj.ApprovedDateTime);
                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus1 = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.ApprovedRequestID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.ApprovedRequestUnitID = UserVo.UserLoginInfo.UnitId;


                foreach (var item in BizActionObj.ApprovalChargeList)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddApprovalRequestDetailsHistroy");
                    dbServer.AddInParameter(command1, "ID", DbType.Int64, item.ApprovalHistoryID);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "ApprovedRequestID", DbType.Int64, BizActionObj.ApprovedRequestID);
                    dbServer.AddInParameter(command1, "ApprovedRequestUnitID", DbType.Int64, BizActionObj.ApprovedRequestUnitID);
                    dbServer.AddInParameter(command1, "ApprovalRequestID", DbType.Int64, item.ApprovalRequestID);
                    dbServer.AddInParameter(command1, "ApprovalRequestUnitID", DbType.Int64, item.ApprovalRequestUnitID);
                    dbServer.AddInParameter(command1, "ChargeID", DbType.Int64, item.ID);
                    dbServer.AddInParameter(command1, "ChargeUnitID", DbType.Int64, item.UnitID);
                    dbServer.AddInParameter(command1, "ServiceID", DbType.Int64, item.ServiceId);
                    dbServer.AddInParameter(command1, "ApprovalStatus", DbType.Boolean, item.SelectCharge);

                    dbServer.AddInParameter(command1, "ApprovalRemarkID", DbType.Int64, item.SelectedApprovalRefundReason.ID);
                    dbServer.AddInParameter(command1, "ApprovalRemark", DbType.String, item.SelectedApprovalRefundReason.Description);

                    dbServer.AddInParameter(command1, "ApprovedBy", DbType.Int64, BizActionObj.ApprovedByID);
                    dbServer.AddInParameter(command1, "ApprovedDateTime", DbType.DateTime, BizActionObj.ApprovedDateTime);
                    dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = Convert.ToInt32(dbServer.GetParameterValue(command1, "ResultStatus"));
                }

                if (IsAuditTrail && BizActionObj.LogInfoList != null)   // By Umesh For Audit Trail
                {
                    LogInfo LogInformation = new LogInfo();
                    LogInformation.guid = new Guid();
                    LogInformation.UserId = UserVo.ID;
                    LogInformation.TimeStamp = DateTime.Now;
                    LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                    LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                    LogInformation.Message = " 26 : Approved Services " + "\r\n"
                                            + "User Unit Id : " + Convert.ToString(UserVo.UserLoginInfo.UnitId) + " "
                                            + "Approval Request ID : " + Convert.ToString(BizActionObj.ApprovalRequestID) + " "
                                            + "Approval Request UnitID : " + Convert.ToString(BizActionObj.ApprovalRequestUnitID) + " "
                                            + "Approval Remark : " + Convert.ToString(BizActionObj.ApprovalRemark) + " "
                                            + "ApprovedBy ID : " + Convert.ToString(BizActionObj.ApprovedByID) + " "
                                            + "ApprovedBy Name : " + Convert.ToString(BizActionObj.ApprovedByName) + " "
                                            + "Approved Date Time : " + Convert.ToString(BizActionObj.ApprovedDateTime) + " "
                                            + "Level ID : " + Convert.ToString(BizActionObj.LevelID) + " "
                                            + "Approved Request ID : " + Convert.ToString(BizActionObj.ApprovedRequestID) + " "
                                            + "Approved Request UnitID : " + Convert.ToString(BizActionObj.ApprovedRequestUnitID) + "\r\n";
                    LogInformation.Message = LogInformation.Message.Replace("\r\n", Environment.NewLine);
                    BizActionObj.LogInfoList.Add(LogInformation);
                }

                if (pConnection == null) trans.Commit();
                BizActionObj.SuccessStatus = 0;
                if (IsAuditTrail && BizActionObj.LogInfoList != null)  // by Umesh For Activity Log
                {
                    if (BizActionObj.LogInfoList.Count > 0 && IsAuditTrail == true)
                    {
                        SetLogInfo(BizActionObj.LogInfoList, UserVo.ID);
                        BizActionObj.LogInfoList.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                BizActionObj.SuccessStatus = -1;
                if (pConnection == null) trans.Rollback();
            }
            finally
            {
                if (pConnection == null)
                {
                    con.Close();
                    con = null;
                    trans = null;
                }
            }
            return BizActionObj;
        }

        //private clsPaymentVO AddPayment(clsPaymentVO payDetails)
        //{


        //    try
        //    {
        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPayment");

        //        dbServer.AddInParameter(command, "LinkServer", DbType.String, payDetails.LinkServer);
        //        if (payDetails.LinkServer != null)
        //            dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, payDetails.LinkServer.Replace(@"\", "_"));

        //        dbServer.AddInParameter(command, "Date", DbType.DateTime, payDetails.Date);
        //        if (payDetails.ReceiptNo != null) payDetails.ReceiptNo = payDetails.ReceiptNo.Trim();
        //        dbServer.AddInParameter(command, "ReceiptNo", DbType.String, payDetails.ReceiptNo);

        //        dbServer.AddInParameter(command, "BillID", DbType.Int64, payDetails.BillID);
        //        dbServer.AddInParameter(command, "BillAmount", DbType.Double, payDetails.BillAmount);
        //        dbServer.AddInParameter(command, "BillBalanceAmount", DbType.Double, payDetails.BillBalanceAmount);
        //        dbServer.AddInParameter(command, "AdvanceID", DbType.Int64, payDetails.AdvanceID);
        //        dbServer.AddInParameter(command, "AdvanceAmount", DbType.Double, payDetails.AdvanceAmount);
        //        dbServer.AddInParameter(command, "AdvanceUsed", DbType.Double, payDetails.AdvanceUsed);
        //        dbServer.AddInParameter(command, "RefundID", DbType.Int64, payDetails.RefundID);
        //        dbServer.AddInParameter(command, "RefundAmount", DbType.Double, payDetails.RefundAmount);

        //        if (payDetails.Remarks != null) payDetails.Remarks = payDetails.Remarks.Trim();
        //        dbServer.AddInParameter(command, "Remarks", DbType.String, payDetails.Remarks);
        //        if (payDetails.PayeeNarration != null) payDetails.PayeeNarration = payDetails.PayeeNarration.Trim();
        //        dbServer.AddInParameter(command, "PayeeNarration", DbType.String, payDetails.PayeeNarration);
        //        dbServer.AddInParameter(command, "ChequeAuthorizedBy", DbType.Int64, payDetails.ChequeAuthorizedBy);
        //        dbServer.AddInParameter(command, "CreditAuthorizedBy", DbType.Int64, payDetails.CreditAuthorizedBy);


        //        dbServer.AddInParameter(command, "IsPrinted", DbType.Boolean, payDetails.IsPrinted);
        //        dbServer.AddInParameter(command, "IsCancelled", DbType.Boolean, payDetails.IsCancelled);

        //        dbServer.AddInParameter(command, "UnitId", DbType.Int64, payDetails.UnitID);
        //        dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, payDetails.CreatedUnitID);

        //        dbServer.AddInParameter(command, "Status", DbType.Boolean, payDetails.Status);
        //        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, payDetails.AddedBy);
        //        if (payDetails.AddedOn != null) payDetails.AddedOn = payDetails.AddedOn.Trim();
        //        dbServer.AddInParameter(command, "AddedOn", DbType.String, payDetails.AddedOn);
        //        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, payDetails.AddedDateTime);
        //        if (payDetails.AddedWindowsLoginName != null) payDetails.AddedWindowsLoginName = payDetails.AddedWindowsLoginName.Trim();
        //        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, payDetails.AddedWindowsLoginName);

        //        dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, payDetails.ID);
        //        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

        //        int intStatus = dbServer.ExecuteNonQuery(command);
        //        //payDetails.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
        //        payDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

        //        foreach (var item in payDetails.PaymentDetails)
        //        {

        //            DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPaymentDetails");

        //            dbServer.AddInParameter(command1, "LinkServer", DbType.String, payDetails.LinkServer);
        //            if (payDetails.LinkServer != null)
        //                dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, payDetails.LinkServer.Replace(@"\", "_"));

        //            dbServer.AddInParameter(command1, "PaymentID", DbType.Int64, payDetails.ID);
        //            dbServer.AddInParameter(command1, "PaymentModeID", DbType.Int64, item.PaymentModeID);
        //            if (item.Number != null) item.Number = item.Number.Trim();
        //            dbServer.AddInParameter(command1, "ChequeNo", DbType.String, item.Number);
        //            dbServer.AddInParameter(command1, "Date", DbType.DateTime, item.Date);
        //            dbServer.AddInParameter(command1, "BankID", DbType.Int64, item.BankID);
        //            dbServer.AddInParameter(command1, "AdvanceID", DbType.Int64, item.AdvanceID);
        //            dbServer.AddInParameter(command1, "PaidAmount", DbType.Double, item.PaidAmount);                    
        //            dbServer.AddInParameter(command1, "ChequeCleared", DbType.Boolean, item.ChequeCleared);

        //            dbServer.AddInParameter(command1, "UnitId", DbType.Int64, payDetails.UnitID);
        //            dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, payDetails.CreatedUnitID);

        //            dbServer.AddInParameter(command1, "Status", DbType.Boolean, payDetails.Status);
        //            dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, payDetails.AddedBy);
        //            if (payDetails.AddedOn != null) payDetails.AddedOn = payDetails.AddedOn.Trim();
        //            dbServer.AddInParameter(command1, "AddedOn", DbType.String, payDetails.AddedOn);
        //            dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, payDetails.AddedDateTime);
        //            if (payDetails.AddedWindowsLoginName != null) payDetails.AddedWindowsLoginName = payDetails.AddedWindowsLoginName.Trim();
        //            dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, payDetails.AddedWindowsLoginName.Trim());

        //            dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
        //            dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

        //            int iStatus = dbServer.ExecuteNonQuery(command1);
        //            //payDetails.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
        //            //item.ID = (long)dbServer.GetParameterValue(command, "ID");
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    finally
        //    {

        //    }
        //    return payDetails;
        //}

        private clsAddRefundBizActionVO UpdateDetails(clsAddRefundBizActionVO BizActionObj, DbConnection pConnection, DbTransaction pTransaction)
        {
            //try
            //{
            //    clsVisitVO objDetailsVO = BizActionObj.VisitDetails;
            //    DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateVisit");

            //    dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
            //    if (objDetailsVO.LinkServer != null)
            //        dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));

            //    dbServer.AddInParameter(command, "PatientID", DbType.Int64, objDetailsVO.PatientId);
            //    dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);
            //    dbServer.AddInParameter(command, "OPDNO", DbType.String, objDetailsVO.OPDNO);
            //    dbServer.AddInParameter(command, "VisitTypeID", DbType.Int64, objDetailsVO.VisitTypeID);
            //    dbServer.AddInParameter(command, "Complaints", DbType.String, objDetailsVO.Complaints);
            //    dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, objDetailsVO.DepartmentID);
            //    dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objDetailsVO.DoctorID);
            //    dbServer.AddInParameter(command, "CabinID", DbType.Int64, objDetailsVO.CabinID);
            //    dbServer.AddInParameter(command, "ReferredDoctorID", DbType.Int64, objDetailsVO.ReferredDoctorID);
            //    dbServer.AddInParameter(command, "ReferredDoctor", DbType.String, objDetailsVO.ReferredDoctor);
            //    dbServer.AddInParameter(command, "PatientCaseRecord", DbType.String, objDetailsVO.PatientCaseRecord);
            //    dbServer.AddInParameter(command, "CaseReferralSheet", DbType.String, objDetailsVO.CaseReferralSheet);
            //    dbServer.AddInParameter(command, "VisitNotes", DbType.String, objDetailsVO.VisitNotes);

            //    dbServer.AddInParameter(command, "UnitId", DbType.Int64, objDetailsVO.UnitId);
            //    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objDetailsVO.UpdatedUnitId);

            //    dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);
            //    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objDetailsVO.UpdatedBy);
            //    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objDetailsVO.UpdatedOn);
            //    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objDetailsVO.UpdatedDateTime);
            //    dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objDetailsVO.UpdatedWindowsLoginName);

            //    dbServer.AddInParameter(command, "ID", DbType.Int64, objDetailsVO.ID);

            //    dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.SuccessStatus);

            //    int intStatus = dbServer.ExecuteNonQuery(command);
            //    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
            //finally
            //{

            //}

            return BizActionObj;
        }

        public override IValueObject Add(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddRefundBizActionVO BizActionObj = valueObject as clsAddRefundBizActionVO;

            if (BizActionObj.Details.ID == 0)
                BizActionObj = AddDetails(BizActionObj, UserVo, null, null);
            else
                BizActionObj = UpdateDetails(BizActionObj, null, null);

            return valueObject;

        }
        public override IValueObject Add(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            clsAddRefundBizActionVO BizActionObj = valueObject as clsAddRefundBizActionVO;

            if (BizActionObj.Details.ID == 0)
                BizActionObj = AddDetails(BizActionObj, UserVo, pConnection, pTransaction);
            else
                BizActionObj = UpdateDetails(BizActionObj, pConnection, pTransaction);

            return valueObject;

        }
        public override IValueObject GetList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRefundListBizActionVO BizActionObj = valueObject as clsGetRefundListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetRefund");

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "AllCompany", DbType.Boolean, BizActionObj.AllCompanies);
                dbServer.AddInParameter(command, "CompanyID", DbType.Int64, BizActionObj.CompanyID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, BizActionObj.CostingDivisionID);

                DbDataReader reader;

                //int intStatus = dbServer.ExecuteNonQuery(command);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.Details == null)
                        BizActionObj.Details = new List<clsRefundVO>();
                    while (reader.Read())
                    {
                        clsRefundVO objDetails = new clsRefundVO();

                        objDetails.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objDetails.PatientID = (long)DALHelper.HandleDBNull(reader["PatientId"]);
                        objDetails.PatientUnitID = (long)DALHelper.HandleDBNull(reader["PatientUnitID"]);
                        objDetails.CompanyID = (long)DALHelper.HandleDBNull(reader["CompanyID"]);
                        objDetails.AdvanceID = (long)DALHelper.HandleDBNull(reader["AdvanceID"]);
                        objDetails.Date = (DateTime?)DALHelper.HandleDate(reader["Date"]);
                        objDetails.Amount = (double)DALHelper.HandleDBNull(reader["Amount"]);
                        objDetails.BillID = (long)DALHelper.HandleDBNull(reader["BillID"]);
                        objDetails.Remarks = (String)DALHelper.HandleDBNull(reader["Remarks"]);
                        objDetails.ReceiptNo = (String)DALHelper.HandleDBNull(reader["ReceiptNo"]);
                        objDetails.ReceiptPrinted = (bool)DALHelper.HandleDBNull(reader["ReceiptPrinted"]);

                        objDetails.UnitID = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                        objDetails.CostingDivisionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CostingDivisionID"]));

                        objDetails.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                        objDetails.AddedBy = (Int64)DALHelper.HandleDBNull(reader["AddedBy"]);
                        objDetails.AddedOn = (String)DALHelper.HandleDBNull(reader["AddedOn"]);
                        objDetails.AddedDateTime = (DateTime?)DALHelper.HandleDate(reader["AddedDateTime"]);
                        objDetails.AddedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]);
                        objDetails.UpdatedBy = (Int64?)DALHelper.HandleDBNull(reader["UpdatedBy"]);
                        objDetails.UpdatedOn = (String)DALHelper.HandleDBNull(reader["UpdatedOn"]);
                        objDetails.UpdatedDateTime = (DateTime?)DALHelper.HandleDate(reader["UpdatedDateTime"]);
                        objDetails.UpdateWindowsLoginName = (String)DALHelper.HandleDBNull(reader["UpdateWindowsLoginName"]);
                        objDetails.UnitName = (String)DALHelper.HandleDBNull(reader["UnitName"]);
                        objDetails.AdvanceNo = Convert.ToString(DALHelper.HandleDBNull(reader["AdvanceNO"]));

                        //objDetails.Company = (string)DALHelper.HandleDBNull(reader["Company"]);
                        //objDetails.AdvanceAgainst = (string)DALHelper.HandleDBNull(reader["AdvanceAgainst"]);

                        BizActionObj.Details.Add(objDetails);

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
            return valueObject;
        }

        public override IValueObject Delete(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteRefundBizActionVO BizActionObj = valueObject as clsDeleteRefundBizActionVO;
            try
            {
                DbCommand command;

                command = dbServer.GetStoredProcCommand("CIMS_DeleteRefund");
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "AdvanceID", DbType.Int64, BizActionObj.AdvanceID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);

                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");


            }
            catch (Exception ex)
            {

                // logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return valueObject;
        }
        public override IValueObject SendApprovalRequestForBill(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsSendRequestForApprovalVO BizActionObj = valueObject as clsSendRequestForApprovalVO;
            DbTransaction trans = null;
            DbConnection con = null;

            try
            {

                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                long ParentID = 0;//T_charge.id
                long CDParentID = 0;//T_chargeDetails.id
                bool IsPackageConsumption = false; //Added By Bhushanp For Package Consumption 07082017
                long chargeID = 0;
                double TestCharge = 0;

                if (BizActionObj.BillDetails.Details.ID == 0)
                {
                    clsBillVO objDetailsVO = BizActionObj.BillDetails.Details;

                    for (int i = 0; i < objDetailsVO.ChargeDetails.Count; i++)
                    {
                        if (objDetailsVO.ChargeDetails[i].ChildPackageService == false)
                        {
                            clsBaseChargeDAL objBaseDAL = clsBaseChargeDAL.GetInstance();
                            clsAddChargeBizActionVO obj = new clsAddChargeBizActionVO();
                            obj.Details = objDetailsVO.ChargeDetails[i];
                            if (objDetailsVO.ChargeDetails[i].ParentID > 0)//Added By Bhushanp For Package Consumption 07082017
                            {
                                IsPackageConsumption = true;
                            }

                            if (obj.Details.Status == true)
                                obj.Details.PaidAmount = obj.Details.NetAmount;
                            else
                                obj.Details.PaidAmount = 0;

                            obj.Details.IsBilled = BizActionObj.BillDetails.Details.IsFreezed;
                            obj.Details.Date = objDetailsVO.Date;
                            obj.Details.Opd_Ipd_External_Id = objDetailsVO.Opd_Ipd_External_Id;
                            obj.Details.Opd_Ipd_External_UnitId = objDetailsVO.Opd_Ipd_External_UnitId;
                            obj.Details.Opd_Ipd_External = objDetailsVO.Opd_Ipd_External;
                            obj.Details.ClassId = objDetailsVO.ChargeDetails[i].ClassId;
                            obj.Details.IsIPDBill = objDetailsVO.IsIPDBill;    // For Service date Column Only In Case IPD BILL In T_Charge 

                            obj = (clsAddChargeBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans, 0, 0);   //obj = (clsAddChargeBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);
                            if (obj.SuccessStatus == -1) throw new Exception();
                            objDetailsVO.ChargeDetails[i].ID = obj.Details.ID;
                            objDetailsVO.ChargeDetails[i].ChargeDetails.ID = obj.Details.ChargeDetails.ID;

                            ParentID = objDetailsVO.ChargeDetails[i].ID;
                            CDParentID = objDetailsVO.ChargeDetails[i].ChargeDetails.ID;


                            var _List = from charge in objDetailsVO.ChargeDetails
                                        where (charge.PackageID.Equals(objDetailsVO.ChargeDetails[i].PackageID) && charge.ChildPackageService == true)
                                        select charge;


                            foreach (var item in _List)
                            {

                                clsBaseChargeDAL objBaseNewDAL = clsBaseChargeDAL.GetInstance();
                                clsAddChargeBizActionVO objCharge = new clsAddChargeBizActionVO();
                                item.ID = 0;

                                objCharge.Details = item;

                                if (objCharge.Details.Status == true)
                                    objCharge.Details.PaidAmount = item.NetAmount;
                                else
                                    objCharge.Details.PaidAmount = 0;

                                objCharge.Details.IsBilled = BizActionObj.BillDetails.Details.IsFreezed;
                                objCharge.Details.Date = objDetailsVO.Date;
                                objCharge.Details.Opd_Ipd_External_Id = objDetailsVO.Opd_Ipd_External_Id;
                                objCharge.Details.Opd_Ipd_External_UnitId = objDetailsVO.Opd_Ipd_External_UnitId;
                                objCharge.Details.Opd_Ipd_External = objDetailsVO.Opd_Ipd_External;

                                objCharge = (clsAddChargeBizActionVO)objBaseNewDAL.Add(objCharge, UserVo, con, trans, ParentID, CDParentID);
                                if (objCharge.SuccessStatus == -1) throw new Exception();

                                item.ID = objCharge.Details.ID;//T_Charge.ID

                                item.ChargeDetails.ID = objCharge.Details.ChargeDetails.ID; //T_ChargeDetails.ID

                            }
                        }
                    }

                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddBillOld");

                    dbServer.AddInParameter(command, "IsPackageBill", DbType.Int64, BizActionObj.BillDetails.IsPackageBill);
                    //dbServer.AddInParameter(command, "PackageID", DbType.Int64, BizActionObj.BillDetails.PackageID);
                    if (BizActionObj.BillDetails.IsPackageBill == true)
                    {
                        StringBuilder sbPackageList = new StringBuilder();

                        foreach (var item in objDetailsVO.ChargeDetails)
                        {
                            if (item.PackageID > 0 && item.isPackageService == true)
                            {
                                sbPackageList.Append("," + Convert.ToString(item.PackageID));
                            }
                        }

                        sbPackageList.Replace(",", "", 0, 1);

                        dbServer.AddInParameter(command, "ipPackageList", DbType.String, Convert.ToString(sbPackageList));
                    }

                    dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                    if (objDetailsVO.LinkServer != null)
                        dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));

                    dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);
                    dbServer.AddInParameter(command, "Opd_Ipd_External_Id", DbType.Int64, objDetailsVO.Opd_Ipd_External_Id);
                    dbServer.AddInParameter(command, "Opd_Ipd_External_UnitId", DbType.Int64, objDetailsVO.Opd_Ipd_External_UnitId);
                    dbServer.AddInParameter(command, "Opd_Ipd_External", DbType.Int16, objDetailsVO.Opd_Ipd_External);
                    dbServer.AddInParameter(command, "CashCounterId", DbType.Int64, objDetailsVO.CashCounterId);
                    dbServer.AddInParameter(command, "CompanyId", DbType.Int64, objDetailsVO.CompanyId);
                    dbServer.AddInParameter(command, "PatientSourceId", DbType.Int64, objDetailsVO.PatientSourceId);
                    dbServer.AddInParameter(command, "PatientCategoryId", DbType.Int64, objDetailsVO.PatientCategoryId);
                    dbServer.AddInParameter(command, "TariffId", DbType.Int64, objDetailsVO.TariffId);
                    if (objDetailsVO.BillNo != null) objDetailsVO.BillNo = objDetailsVO.BillNo.Trim();
                    //dbServer.AddInParameter(command, "BillNo", DbType.String, objDetailsVO.BillNo);
                    dbServer.AddInParameter(command, "TotalBillAmount", DbType.Double, objDetailsVO.TotalBillAmount);
                    dbServer.AddInParameter(command, "TotalConcessionAmount", DbType.Double, objDetailsVO.TotalConcessionAmount);
                    dbServer.AddInParameter(command, "NetBillAmount", DbType.Double, objDetailsVO.NetBillAmount);
                    //By Anjali................................
                    dbServer.AddInParameter(command, "CalculatedNetBillAmount", DbType.Double, objDetailsVO.CalculatedNetBillAmount);
                    //.............................................
                    dbServer.AddInParameter(command, "SelfAmount", DbType.Double, objDetailsVO.SelfAmount);
                    if (objDetailsVO.BalanceAmountSelf < 0) objDetailsVO.BalanceAmountSelf = 0;
                    dbServer.AddInParameter(command, "BalanceAmountSelf", DbType.Double, objDetailsVO.BalanceAmountSelf);
                    dbServer.AddInParameter(command, "StaffFree", DbType.Double, objDetailsVO.StaffFree);
                    dbServer.AddInParameter(command, "NonSelfAmount", DbType.Double, objDetailsVO.NonSelfAmount);
                    dbServer.AddInParameter(command, "BalanceAmountNonSelf", DbType.Double, objDetailsVO.BalanceAmountNonSelf);
                    dbServer.AddInParameter(command, "TotalAdvance", DbType.Double, objDetailsVO.TotalAdvance);
                    dbServer.AddInParameter(command, "IsPrinted", DbType.Boolean, objDetailsVO.IsPrinted);
                    dbServer.AddInParameter(command, "BillType", DbType.Int16, (Int16)objDetailsVO.BillType);
                    dbServer.AddInParameter(command, "BillPaymentType", DbType.Int16, (Int16)objDetailsVO.BillPaymentType);
                    dbServer.AddInParameter(command, "SponserType", DbType.Boolean, objDetailsVO.SponserType);
                    dbServer.AddInParameter(command, "SeniorCitizenCon", DbType.Boolean, objDetailsVO.SeniorCitizenCon);
                    if (objDetailsVO.Remark != null) objDetailsVO.Remark = objDetailsVO.Remark.Trim();
                    dbServer.AddInParameter(command, "Remark", DbType.String, objDetailsVO.Remark);
                    if (objDetailsVO.BillRemark != null) objDetailsVO.BillRemark = objDetailsVO.BillRemark.Trim();
                    dbServer.AddInParameter(command, "BillRemark", DbType.String, objDetailsVO.BillRemark);
                    dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, objDetailsVO.IsFreezed);

                    dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);  //Costing Divisions for Clinical & Pharmacy Billing

                    dbServer.AddInParameter(command, "GrossDiscountReasonID", DbType.Int64, objDetailsVO.GrossDiscountReasonID);
                    dbServer.AddInParameter(command, "GrossDiscountPercentage", DbType.Double, objDetailsVO.GrossDiscountPercentage);
                    dbServer.AddInParameter(command, "ConcessionAuthorizedBy", DbType.Int64, objDetailsVO.ConcessionAuthorizedBy);
                    dbServer.AddInParameter(command, "ConcessionRemark", DbType.String, objDetailsVO.ConcessionRemark); //Added By Bhushanp 09032017
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);

                    dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    objDetailsVO.UnitID = UserVo.UserLoginInfo.UnitId;
                    dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    dbServer.AddParameter(command, "BillNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000");
                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                    dbServer.AddInParameter(command, "ConcessionreasonId", DbType.Int64, objDetailsVO.ConcessionReasonId);//Added By Yogesh K
                    dbServer.AddInParameter(command, "IsPackageConsumption", DbType.Boolean, IsPackageConsumption);////Added By Bhushanp For Package Consumption 07082017

                    StringBuilder ChargeIdListList = new StringBuilder();
                    StringBuilder SponsorTypeList = new StringBuilder();
                    StringBuilder BalanceAmountList = new StringBuilder();
                    StringBuilder StatusList = new StringBuilder();

                    for (int UnitCount = 0; UnitCount < objDetailsVO.ChargeDetails.Count; UnitCount++)
                    {
                        ChargeIdListList.Append(objDetailsVO.ChargeDetails[UnitCount].ID);
                        StatusList.Append(objDetailsVO.ChargeDetails[UnitCount].Status);
                        BalanceAmountList.Append(objDetailsVO.ChargeDetails[UnitCount].NetAmount - objDetailsVO.ChargeDetails[UnitCount].PaidAmount);
                        SponsorTypeList.Append(objDetailsVO.ChargeDetails[UnitCount].SponsorType);

                        if (UnitCount < (objDetailsVO.ChargeDetails.Count - 1))
                        {
                            ChargeIdListList.Append(",");
                            StatusList.Append(",");
                            BalanceAmountList.Append(",");
                            SponsorTypeList.Append(",");
                        }
                    }

                    dbServer.AddInParameter(command, "ChargeIdList", DbType.String, ChargeIdListList.ToString());
                    dbServer.AddInParameter(command, "StatusList", DbType.String, StatusList.ToString());
                    dbServer.AddInParameter(command, "SponsorTypeList", DbType.String, SponsorTypeList.ToString());
                    dbServer.AddInParameter(command, "BalanceAmountList", DbType.String, BalanceAmountList.ToString());

                    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    BizActionObj.BillDetails.Details.ID = (long)dbServer.GetParameterValue(command, "ID");
                    BizActionObj.BillDetails.Details.BillNo = (string)dbServer.GetParameterValue(command, "BillNo");

                    if (BizActionObj.SuccessStatus == -2) throw new Exception();//Added By Bhushanp 19072017 For VisitID & VisitUnitID Passess Zero & Bill No Duplication

                    if (BizActionObj.BillDetails.Details.IsFreezed == true && BizActionObj.BillDetails.Details.PaymentDetails != null)
                    {

                        clsBasePaymentDAL objBaseDAL = clsBasePaymentDAL.GetInstance();
                        clsAddPaymentBizActionVO obj = new clsAddPaymentBizActionVO();

                        obj.Details = new clsPaymentVO();
                        obj.Details = BizActionObj.BillDetails.Details.PaymentDetails;

                        obj.Details.BillID = BizActionObj.BillDetails.Details.ID;
                        obj.Details.BillAmount = objDetailsVO.NetBillAmount;
                        obj.Details.Date = BizActionObj.BillDetails.Details.Date;
                        obj.Details.LinkServer = BizActionObj.BillDetails.Details.LinkServer;
                        obj.myTransaction = true;
                        obj = (clsAddPaymentBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);
                        if (obj.SuccessStatus == -1) throw new Exception();
                        BizActionObj.BillDetails.Details.PaymentDetails.ID = obj.Details.ID;

                    }

                    //if (BizActionObj.Details.IsFreezed == true && BizActionObj.Details.PathoWorkOrder != null && BizActionObj.Details.PathoWorkOrder.PathoSpecilizationID > 0) //&& BizActionObj.Details.PathoWorkOrder.ServiceID>0
                    if (BizActionObj.BillDetails.Details.PathoWorkOrder != null && BizActionObj.BillDetails.Details.PathoWorkOrder.PathoSpecilizationID > 0 && ((BizActionObj.BillDetails.Details.IsFreezed == true && BizActionObj.BillDetails.Details.IsIPDBill == false) || (BizActionObj.BillDetails.Details.IsIPDBill == true))) //&& BizActionObj.Details.PathoWorkOrder.ServiceID>0
                    {
                        BizActionObj.BillDetails.Details.PathoWorkOrder.Opd_Ipd_External_ID = BizActionObj.BillDetails.Details.Opd_Ipd_External_Id;
                        BizActionObj.BillDetails.Details.PathoWorkOrder.Opd_Ipd_External_UnitID = BizActionObj.BillDetails.Details.Opd_Ipd_External_UnitId;
                        BizActionObj.BillDetails.Details.PathoWorkOrder.SampleType = false;
                        BizActionObj.BillDetails.Details.PathoWorkOrder.Opd_Ipd_External = BizActionObj.BillDetails.Details.Opd_Ipd_External;
                        // BizActionObj.Details.PathoWorkOrder.Opd_Ipd_External_UnitID = BizActionObj.Details.Opd_Ipd_External
                        BizActionObj.BillDetails.Details.PathoWorkOrder.UnitId = BizActionObj.BillDetails.Details.UnitID;
                        BizActionObj.BillDetails.Details.PathoWorkOrder.AddedDateTime = BizActionObj.BillDetails.Details.AddedDateTime;
                        BizActionObj.BillDetails.Details.PathoWorkOrder.OrderDate = BizActionObj.BillDetails.Details.Date;
                        BizActionObj.BillDetails.Details.PathoWorkOrder.BillNo = BizActionObj.BillDetails.Details.BillNo;
                        BizActionObj.BillDetails.Details.PathoWorkOrder.BillID = BizActionObj.BillDetails.Details.ID;

                        //Added by Priyanka
                        //for (int count = 0; count < BizActionObj.Details.PathoWorkOrder.Items.Count; count++)
                        //{
                        for (int chargecount = 0; chargecount < objDetailsVO.ChargeDetails.Count; chargecount++)
                        {
                            if (BizActionObj.BillDetails.Details.PathoWorkOrder.PathoSpecilizationID == objDetailsVO.ChargeDetails[chargecount].ServiceSpecilizationID)
                            {


                                BizActionObj.BillDetails.Details.PathoWorkOrder.DoctorID = BizActionObj.BillDetails.Details.PathoWorkOrder.DoctorID;


                                clsPathOrderBookingVO BizAction = new clsPathOrderBookingVO();
                                BizAction = GetPathologyTestDetails(objDetailsVO.ChargeDetails[chargecount].ServiceId, UserVo);
                                if (BizAction.Items != null && BizAction.Items.Count > 0)
                                    foreach (var item in BizAction.Items)
                                    {
                                        item.ServiceID = objDetailsVO.ChargeDetails[chargecount].ServiceId;
                                        item.ChargeID = objDetailsVO.ChargeDetails[chargecount].ID;
                                        item.TariffServiceID = objDetailsVO.ChargeDetails[chargecount].TariffServiceId;
                                        item.TestCharges = objDetailsVO.ChargeDetails[chargecount].Rate;

                                        BizActionObj.BillDetails.Details.PathoWorkOrder.Items.Add(item);
                                    }
                            }
                            //}

                        }

                        clsBaseOrderBookingDAL objBaseDAL = clsBaseOrderBookingDAL.GetInstance();
                        clsAddPathOrderBookingBizActionVO obj = new clsAddPathOrderBookingBizActionVO();
                        obj.myTransaction = true;
                        obj.PathOrderBooking = BizActionObj.BillDetails.Details.PathoWorkOrder;
                        obj.PathOrderBookingDetailList = BizActionObj.BillDetails.Details.PathoWorkOrder.Items;
                        obj = (clsAddPathOrderBookingBizActionVO)objBaseDAL.AddPathOrderBooking(obj, UserVo, trans, con);
                        if (obj.SuccessStatus == -1) throw new Exception();
                        BizActionObj.BillDetails.Details.PathoWorkOrder.ID = obj.PathOrderBooking.ID;

                    }

                    //if (BizActionObj.Details.IsFreezed == true && BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails.Count > 0)
                    if (BizActionObj.BillDetails.Details.RadiologyWorkOrder != null && BizActionObj.BillDetails.Details.RadiologyWorkOrder.OrderBookingDetails.Count > 0 && ((BizActionObj.BillDetails.Details.IsFreezed == true && BizActionObj.BillDetails.Details.IsIPDBill == false) || (BizActionObj.BillDetails.Details.IsIPDBill == true)))
                    {
                        BizActionObj.BillDetails.Details.RadiologyWorkOrder.Opd_Ipd_External_ID = BizActionObj.BillDetails.Details.Opd_Ipd_External_Id;
                        BizActionObj.BillDetails.Details.RadiologyWorkOrder.Opd_Ipd_External_UnitID = BizActionObj.BillDetails.Details.Opd_Ipd_External_UnitId;
                        //BizActionObj.Details.RadiologyWorkOrder.SampleType = false;
                        BizActionObj.BillDetails.Details.RadiologyWorkOrder.Opd_Ipd_External = BizActionObj.BillDetails.Details.Opd_Ipd_External;
                        // BizActionObj.Details.PathoWorkOrder.Opd_Ipd_External_UnitID = BizActionObj.Details.Opd_Ipd_External
                        BizActionObj.BillDetails.Details.RadiologyWorkOrder.UnitID = BizActionObj.BillDetails.Details.UnitID;
                        BizActionObj.BillDetails.Details.RadiologyWorkOrder.AddedDateTime = BizActionObj.BillDetails.Details.AddedDateTime;
                        BizActionObj.BillDetails.Details.RadiologyWorkOrder.Date = BizActionObj.BillDetails.Details.Date;
                        BizActionObj.BillDetails.Details.RadiologyWorkOrder.BillNo = BizActionObj.BillDetails.Details.BillNo;
                        BizActionObj.BillDetails.Details.RadiologyWorkOrder.BillID = BizActionObj.BillDetails.Details.ID;

                        for (int count = 0; count < BizActionObj.BillDetails.Details.RadiologyWorkOrder.OrderBookingDetails.Count; count++)
                        {
                            for (int chargecount = 0; chargecount < objDetailsVO.ChargeDetails.Count; chargecount++)
                            {
                                if (BizActionObj.BillDetails.Details.RadiologyWorkOrder.OrderBookingDetails[count].TariffServiceID == objDetailsVO.ChargeDetails[chargecount].TariffServiceId)
                                {
                                    BizActionObj.BillDetails.Details.RadiologyWorkOrder.OrderBookingDetails[count].ChargeID = objDetailsVO.ChargeDetails[chargecount].ID;
                                    clsRadOrderBookingDetailsVO BizAction = new clsRadOrderBookingDetailsVO();
                                    BizAction = GetRadilogyTestDetails(BizActionObj.BillDetails.Details.RadiologyWorkOrder.OrderBookingDetails[count].ServiceID, UserVo);
                                    if (BizAction != null) BizActionObj.BillDetails.Details.RadiologyWorkOrder.OrderBookingDetails[count].TestID = BizAction.TestID;
                                    break;
                                }
                            }

                        }

                        clsBaseRadiologyDAL objBaseDAL = clsBaseRadiologyDAL.GetInstance();
                        clsAddRadOrderBookingBizActionVO obj = new clsAddRadOrderBookingBizActionVO();

                        obj.BookingDetails = BizActionObj.BillDetails.Details.RadiologyWorkOrder;
                        // obj. = BizActionObj.Details.PathoWorkOrder.Items;
                        obj.myTransaction = true;
                        obj = (clsAddRadOrderBookingBizActionVO)objBaseDAL.AddOrderBooking(obj, UserVo, con, trans);
                        if (obj.SuccessStatus == -1) throw new Exception();
                        objDetailsVO.RadiologyWorkOrder.ID = obj.BookingDetails.ID;
                    }

                    if (objDetailsVO.PharmacyItems.Items.Count > 0)
                    {
                        clsBaseItemSalesDAL objBaseDAL = clsBaseItemSalesDAL.GetInstance();
                        clsAddItemSalesBizActionVO obj = new clsAddItemSalesBizActionVO();
                        // obj.Details.ItemSaleNo 
                        obj.Details = objDetailsVO.PharmacyItems;
                        obj.Details.BillID = BizActionObj.BillDetails.Details.ID;
                        obj.Details.IsBilled = BizActionObj.BillDetails.Details.IsFreezed;
                        obj.myTransaction = true;
                        obj = (clsAddItemSalesBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);
                        objDetailsVO.PharmacyItems.ID = obj.Details.ID;
                    }
                }
                else
                {
                    clsBillVO objDetailsVO = BizActionObj.BillDetails.Details;
                    DbCommand command8 = dbServer.GetStoredProcCommand("CIMS_UpdateChargeDetailsNew");
                    dbServer.AddInParameter(command8, "BillID", DbType.Int64, objDetailsVO.ID);
                    dbServer.AddInParameter(command8, "UnitID", DbType.Int64, objDetailsVO.UnitID);
                    dbServer.AddInParameter(command8, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);

                    int intStatus8 = dbServer.ExecuteNonQuery(command8, trans);
                    for (int i = 0; i < objDetailsVO.ChargeDetails.Count; i++)
                    {
                        if (objDetailsVO.ChargeDetails[i].ChildPackageService == false)
                        {
                            clsBaseChargeDAL objBaseDAL = clsBaseChargeDAL.GetInstance();
                            clsAddChargeBizActionVO obj = new clsAddChargeBizActionVO();

                            obj.Details = objDetailsVO.ChargeDetails[i];
                            obj.Details.IsBilled = BizActionObj.BillDetails.Details.IsFreezed;

                            obj.Details.Date = objDetailsVO.Date;

                            if (obj.Details.Status == true)
                                obj.Details.PaidAmount = obj.Details.NetAmount;
                            else
                                obj.Details.PaidAmount = 0;

                            obj.Details.Opd_Ipd_External_Id = objDetailsVO.Opd_Ipd_External_Id;
                            obj.Details.Opd_Ipd_External_UnitId = objDetailsVO.Opd_Ipd_External_UnitId;
                            obj.Details.Opd_Ipd_External = objDetailsVO.Opd_Ipd_External;
                            obj.Details.ClassId = objDetailsVO.ChargeDetails[i].ClassId;
                            obj.Details.IsIPDBill = objDetailsVO.IsIPDBill;    // For Service date Column Only In Case IPD BILL In T_Charge 
                            //obj.Details.ID = 0;
                            obj = (clsAddChargeBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans, 0, 0);  //obj = (clsAddChargeBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);
                            if (obj.SuccessStatus == -1) throw new Exception();
                            obj.Details.ID = obj.Details.ID;
                            objDetailsVO.ChargeDetails[i].ID = obj.Details.ID;

                            ParentID = objDetailsVO.ChargeDetails[i].ID;
                            CDParentID = objDetailsVO.ChargeDetails[i].ChargeDetails.ID;
                            var _List = from charge in objDetailsVO.ChargeDetails
                                        where (charge.PackageID.Equals(objDetailsVO.ChargeDetails[i].PackageID) && charge.ChildPackageService == true)
                                        select charge;
                            foreach (var item in _List)
                            {
                                clsBaseChargeDAL objBaseNewDAL = clsBaseChargeDAL.GetInstance();
                                clsAddChargeBizActionVO objCharge = new clsAddChargeBizActionVO();
                                //item.ID = 0;
                                objCharge.Details = item;
                                if (objCharge.Details.Status == true)
                                    objCharge.Details.PaidAmount = item.NetAmount;
                                else
                                    objCharge.Details.PaidAmount = 0;
                                objCharge.Details.IsBilled = BizActionObj.BillDetails.Details.IsFreezed;
                                objCharge.Details.Date = objDetailsVO.Date;
                                objCharge.Details.Opd_Ipd_External_Id = objDetailsVO.Opd_Ipd_External_Id;
                                objCharge.Details.Opd_Ipd_External_UnitId = objDetailsVO.Opd_Ipd_External_UnitId;
                                objCharge.Details.Opd_Ipd_External = objDetailsVO.Opd_Ipd_External;
                                obj.Details.ClassId = objDetailsVO.ChargeDetails[i].ClassId;
                                objCharge = (clsAddChargeBizActionVO)objBaseNewDAL.Add(objCharge, UserVo, con, trans, ParentID, CDParentID);
                                if (objCharge.SuccessStatus == -1) throw new Exception();
                                item.ID = objCharge.Details.ID;//T_Charge.ID
                                item.ChargeDetails.ID = objCharge.Details.ChargeDetails.ID; //T_ChargeDetails.ID
                            }
                        }
                    }
                    DbCommand command7 = dbServer.GetStoredProcCommand("CIMS_UpdateChargeNew");
                    dbServer.AddInParameter(command7, "BillID", DbType.Int64, objDetailsVO.ID);
                    dbServer.AddInParameter(command7, "UnitID", DbType.Int64, objDetailsVO.UnitID);
                    dbServer.AddInParameter(command7, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);
                    int intStatus7 = dbServer.ExecuteNonQuery(command7, trans);

                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateBill");
                    dbServer.AddInParameter(command, "IsPackageBill", DbType.Int64, BizActionObj.BillDetails.IsPackageBill);
                    //dbServer.AddInParameter(command, "PackageID", DbType.Int64, BizActionObj.BillDetails.PackageID);
                    if (BizActionObj.BillDetails.IsPackageBill == true)
                    {
                        StringBuilder sbPackageList = new StringBuilder();

                        foreach (var item in objDetailsVO.ChargeDetails)
                        {
                            if (item.PackageID > 0 && item.isPackageService == true)
                            {
                                sbPackageList.Append("," + Convert.ToString(item.PackageID));
                            }
                        }

                        sbPackageList.Replace(",", "", 0, 1);

                        dbServer.AddInParameter(command, "ipPackageList", DbType.String, Convert.ToString(sbPackageList));
                    }

                    dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                    if (objDetailsVO.LinkServer != null)
                        dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));

                    dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);

                    dbServer.AddInParameter(command, "TotalBillAmount", DbType.Double, objDetailsVO.TotalBillAmount);
                    dbServer.AddInParameter(command, "TotalConcessionAmount", DbType.Double, objDetailsVO.TotalConcessionAmount);
                    dbServer.AddInParameter(command, "NetBillAmount", DbType.Double, objDetailsVO.NetBillAmount);
                    dbServer.AddInParameter(command, "SelfAmount", DbType.Double, objDetailsVO.SelfAmount);
                    if (objDetailsVO.BalanceAmountSelf < 0) objDetailsVO.BalanceAmountSelf = 0;
                    dbServer.AddInParameter(command, "BalanceAmountSelf", DbType.Double, objDetailsVO.BalanceAmountSelf);
                    dbServer.AddInParameter(command, "NonSelfAmount", DbType.Double, objDetailsVO.NonSelfAmount);
                    dbServer.AddInParameter(command, "BalanceAmountNonSelf", DbType.Double, objDetailsVO.BalanceAmountNonSelf);

                    dbServer.AddInParameter(command, "IsPrinted", DbType.Boolean, objDetailsVO.IsPrinted);
                    dbServer.AddInParameter(command, "SponserType", DbType.Boolean, objDetailsVO.SponserType);
                    if (objDetailsVO.Remark != null) objDetailsVO.Remark = objDetailsVO.Remark.Trim();
                    dbServer.AddInParameter(command, "Remark", DbType.String, objDetailsVO.Remark);
                    if (objDetailsVO.BillRemark != null) objDetailsVO.BillRemark = objDetailsVO.BillRemark.Trim();
                    dbServer.AddInParameter(command, "BillRemark", DbType.String, objDetailsVO.BillRemark);
                    dbServer.AddInParameter(command, "BillType", DbType.Int16, (Int16)objDetailsVO.BillType);
                    dbServer.AddInParameter(command, "BillPaymentType", DbType.Int16, (Int16)objDetailsVO.BillPaymentType);
                    dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, objDetailsVO.IsFreezed);

                    // Added by Changdeo
                    dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);  //Costing Divisions for Clinical & Pharmacy Billing
                    dbServer.AddInParameter(command, "GrossDiscountReason", DbType.Int64, objDetailsVO.GrossDiscountReasonID);
                    dbServer.AddInParameter(command, "GrossDiscountPercentage", DbType.Double, objDetailsVO.GrossDiscountPercentage);
                    dbServer.AddInParameter(command, "ConcessionAuthorizedBy", DbType.Int64, objDetailsVO.ConcessionAuthorizedBy);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDetailsVO.UnitID);
                    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "ConcessionRemark", DbType.String, objDetailsVO.ConcessionRemark); //Added By Bhushanp 09032017
                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                    dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    dbServer.AddInParameter(command, "ConcessionreasonId", DbType.Int64, objDetailsVO.ConcessionReasonId);//Added By Yogesh K

                    dbServer.AddInParameter(command, "ID", DbType.Int64, objDetailsVO.ID);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);



                    StringBuilder ChargeIdListList = new StringBuilder();
                    StringBuilder SponsorTypeList = new StringBuilder();
                    StringBuilder BalanceAmountList = new StringBuilder();
                    StringBuilder StatusList = new StringBuilder();

                    for (int UnitCount = 0; UnitCount < objDetailsVO.ChargeDetails.Count; UnitCount++)
                    {
                        ChargeIdListList.Append(objDetailsVO.ChargeDetails[UnitCount].ID);
                        StatusList.Append(objDetailsVO.ChargeDetails[UnitCount].Status);
                        BalanceAmountList.Append(objDetailsVO.ChargeDetails[UnitCount].NetAmount - objDetailsVO.ChargeDetails[UnitCount].PaidAmount);
                        SponsorTypeList.Append(objDetailsVO.ChargeDetails[UnitCount].SponsorType);

                        if (UnitCount < (objDetailsVO.ChargeDetails.Count - 1))
                        {
                            ChargeIdListList.Append(",");
                            StatusList.Append(",");
                            BalanceAmountList.Append(",");
                            SponsorTypeList.Append(",");
                        }
                    }

                    dbServer.AddInParameter(command, "ChargeIdList", DbType.String, ChargeIdListList.ToString());
                    dbServer.AddInParameter(command, "StatusList", DbType.String, StatusList.ToString());
                    dbServer.AddInParameter(command, "SponsorTypeList", DbType.String, SponsorTypeList.ToString());
                    dbServer.AddInParameter(command, "BalanceAmountList", DbType.String, BalanceAmountList.ToString());
                    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    if (BizActionObj.BillDetails.Details.IsFreezed == true && BizActionObj.BillDetails.Details.PaymentDetails != null)
                    {

                        clsBasePaymentDAL objBaseDAL = clsBasePaymentDAL.GetInstance();
                        clsAddPaymentBizActionVO obj = new clsAddPaymentBizActionVO();
                        obj.Details = new clsPaymentVO();
                        obj.Details = BizActionObj.BillDetails.Details.PaymentDetails;
                        obj.myTransaction = true;
                        obj.Details.BillID = BizActionObj.BillDetails.Details.ID;
                        obj.Details.BillAmount = objDetailsVO.NetBillAmount;
                        obj.Details.Date = BizActionObj.BillDetails.Details.Date;
                        obj.Details.LinkServer = BizActionObj.BillDetails.Details.LinkServer;
                        obj = (clsAddPaymentBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);
                        if (obj.SuccessStatus == -1) throw new Exception();
                        BizActionObj.BillDetails.Details.PaymentDetails.ID = obj.Details.ID;
                    }

                    if (BizActionObj.BillDetails.Details.PathoWorkOrder != null && ((BizActionObj.BillDetails.Details.IsFreezed == true && BizActionObj.BillDetails.Details.IsIPDBill == false) || (BizActionObj.BillDetails.Details.IsIPDBill == true))) //&& BizActionObj.Details.PathoWorkOrder.ServiceID>0                
                    {
                        BizActionObj.BillDetails.Details.PathoWorkOrder.Opd_Ipd_External_ID = BizActionObj.BillDetails.Details.Opd_Ipd_External_Id;
                        BizActionObj.BillDetails.Details.PathoWorkOrder.Opd_Ipd_External_UnitID = BizActionObj.BillDetails.Details.Opd_Ipd_External_UnitId;
                        BizActionObj.BillDetails.Details.PathoWorkOrder.SampleType = false;
                        BizActionObj.BillDetails.Details.PathoWorkOrder.Opd_Ipd_External = BizActionObj.BillDetails.Details.Opd_Ipd_External;
                        BizActionObj.BillDetails.Details.PathoWorkOrder.UnitId = BizActionObj.BillDetails.Details.UnitID;
                        BizActionObj.BillDetails.Details.PathoWorkOrder.AddedDateTime = BizActionObj.BillDetails.Details.AddedDateTime;
                        BizActionObj.BillDetails.Details.PathoWorkOrder.OrderDate = BizActionObj.BillDetails.Details.Date;
                        BizActionObj.BillDetails.Details.PathoWorkOrder.BillNo = BizActionObj.BillDetails.Details.BillNo;
                        BizActionObj.BillDetails.Details.PathoWorkOrder.BillID = BizActionObj.BillDetails.Details.ID;
                        for (int chargecount = 0; chargecount < objDetailsVO.ChargeDetails.Count; chargecount++)
                        {
                            if (BizActionObj.BillDetails.Details.PathoWorkOrder.PathoSpecilizationID == objDetailsVO.ChargeDetails[chargecount].ServiceSpecilizationID)
                            {
                                BizActionObj.BillDetails.Details.PathoWorkOrder.DoctorID = BizActionObj.BillDetails.Details.PathoWorkOrder.DoctorID;
                                if (objDetailsVO.ChargeDetails[chargecount].POBID > 0)
                                    BizActionObj.BillDetails.Details.PathoWorkOrder.ID = objDetailsVO.ChargeDetails[chargecount].POBID;
                                BizActionObj.BillDetails.Details.PathoWorkOrder.ID = BizActionObj.BillDetails.Details.PathoWorkOrder.ID;  // objDetailsVO.ChargeDetails[chargecount].POBID;  //to update T_PathOrderBooking using Id
                                clsPathOrderBookingVO BizAction = new clsPathOrderBookingVO();
                                if (BizActionObj.BillDetails.Details.PathoWorkOrder.ID > 0) //if (objDetailsVO.ChargeDetails[chargecount].POBID > 0)
                                    BizAction = GetPathologyTestDetails(objDetailsVO.ChargeDetails[chargecount].ServiceId, UserVo, BizActionObj.BillDetails.Details.PathoWorkOrder.ID, objDetailsVO.ChargeDetails[chargecount].UnitID, objDetailsVO.ChargeDetails[chargecount].ID, con, trans); //GetPathologyTestDetails(objDetailsVO.ChargeDetails[chargecount].ServiceId, UserVo, objDetailsVO.ChargeDetails[chargecount].POBID, objDetailsVO.ChargeDetails[chargecount].UnitID);
                                else
                                    BizAction = GetPathologyTestDetails(objDetailsVO.ChargeDetails[chargecount].ServiceId, UserVo);

                                if (BizAction.Items != null && BizAction.Items.Count > 0)
                                    foreach (var item in BizAction.Items)
                                    {
                                        item.ID = item.ID;       //to update T_PathOrderBookingDetails using Id                         
                                        objDetailsVO.ChargeDetails[chargecount].POBDID = item.ID;
                                        item.ServiceID = objDetailsVO.ChargeDetails[chargecount].ServiceId;
                                        item.ChargeID = objDetailsVO.ChargeDetails[chargecount].ID;
                                        item.TariffServiceID = objDetailsVO.ChargeDetails[chargecount].TariffServiceId;
                                        item.TestCharges = objDetailsVO.ChargeDetails[chargecount].Rate;
                                        BizActionObj.BillDetails.Details.PathoWorkOrder.Items.Add(item);
                                    }
                            }
                        }
                        clsBaseOrderBookingDAL objBaseDAL = clsBaseOrderBookingDAL.GetInstance();
                        clsAddPathOrderBookingBizActionVO obj = new clsAddPathOrderBookingBizActionVO();
                        obj.PathOrderBooking = BizActionObj.BillDetails.Details.PathoWorkOrder;
                        obj.PathOrderBookingDetailList = BizActionObj.BillDetails.Details.PathoWorkOrder.Items;
                        //Commented By Bhushanp 20012017
                        //obj = (clsAddPathOrderBookingBizActionVO)objBaseDAL.AddPathOrderBooking(obj, UserVo, trans, con);
                        //if (obj.SuccessStatus == -1) throw new Exception();
                        //if (obj.PathOrderBooking.ID > 0)
                        //    BizActionObj.BillDetails.Details.PathoWorkOrder.ID = obj.PathOrderBooking.ID;
                        // Changes By Bhushanp 20012017 
                        if (obj.PathOrderBookingDetailList.Count > 0)
                        {
                            obj = (clsAddPathOrderBookingBizActionVO)objBaseDAL.AddPathOrderBooking(obj, UserVo, trans, con);
                            if (obj.SuccessStatus == -1) throw new Exception();
                            if (obj.PathOrderBooking.ID > 0)
                                BizActionObj.BillDetails.Details.PathoWorkOrder.ID = obj.PathOrderBooking.ID;
                        }
                        //Added By Bhushanp For Delete Path Work Order

                        DbCommand Sqlcommand = dbServer.GetSqlStringCommand("DELETE FROM T_PathOrderBookingDetails Where OrderID = (Select TOP 1 ID FROM T_PathOrderBooking Where BillID = " + BizActionObj.BillDetails.Details.PathoWorkOrder.BillID + ") AND UnitID =" + BizActionObj.BillDetails.Details.PathoWorkOrder.UnitId + " AND Status = 0 ");
                        int sqlStatus = dbServer.ExecuteNonQuery(Sqlcommand, trans);

                        // Added by CDS For IPD 
                        if (BizActionObj.BillDetails.Details.DeletedChargeDetails != null && BizActionObj.BillDetails.Details.DeletedChargeDetails.Count > 0 && BizActionObj.BillDetails.Details.IsIPDBill == true)
                        {
                            foreach (clsChargeVO item in BizActionObj.BillDetails.Details.DeletedChargeDetails)
                            {
                                if (item.ID != 0 && item.TariffServiceId != 0 && BizActionObj.BillDetails.Details.PathoSpecilizationID == item.ServiceSpecilizationID)
                                {
                                    DbCommand SqlcommandP = dbServer.GetSqlStringCommand("Delete from T_PathOrderBookingDetails where ChargeID = " + item.ID + " And TariffServiceID = " + item.TariffServiceId + " And UnitID = " + item.UnitID + "");
                                    int sqlStatusP = dbServer.ExecuteNonQuery(SqlcommandP, trans);
                                }
                            }
                        }
                    }
                    if (BizActionObj.BillDetails.Details.RadiologyWorkOrder != null && BizActionObj.BillDetails.Details.RadiologyWorkOrder.OrderBookingDetails.Count > 0 && ((BizActionObj.BillDetails.Details.IsFreezed == true && BizActionObj.BillDetails.Details.IsIPDBill == false) || (BizActionObj.BillDetails.Details.IsIPDBill == true)))
                    {
                        BizActionObj.BillDetails.Details.RadiologyWorkOrder.Opd_Ipd_External_ID = BizActionObj.BillDetails.Details.Opd_Ipd_External_Id;
                        BizActionObj.BillDetails.Details.RadiologyWorkOrder.Opd_Ipd_External_UnitID = BizActionObj.BillDetails.Details.Opd_Ipd_External_UnitId;
                        BizActionObj.BillDetails.Details.RadiologyWorkOrder.Opd_Ipd_External = BizActionObj.BillDetails.Details.Opd_Ipd_External;
                        BizActionObj.BillDetails.Details.RadiologyWorkOrder.UnitID = BizActionObj.BillDetails.Details.UnitID;
                        BizActionObj.BillDetails.Details.RadiologyWorkOrder.AddedDateTime = BizActionObj.BillDetails.Details.AddedDateTime;
                        BizActionObj.BillDetails.Details.RadiologyWorkOrder.Date = BizActionObj.BillDetails.Details.Date;
                        BizActionObj.BillDetails.Details.RadiologyWorkOrder.BillNo = BizActionObj.BillDetails.Details.BillNo;
                        for (int count = 0; count < BizActionObj.BillDetails.Details.RadiologyWorkOrder.OrderBookingDetails.Count; count++)
                        {
                            for (int chargecount = 0; chargecount < objDetailsVO.ChargeDetails.Count; chargecount++)
                            {
                                if (objDetailsVO.ChargeDetails[chargecount].ROBID > 0)
                                    BizActionObj.BillDetails.Details.RadiologyWorkOrder.ID = objDetailsVO.ChargeDetails[chargecount].ROBID;
                                BizActionObj.BillDetails.Details.RadiologyWorkOrder.ID = BizActionObj.BillDetails.Details.RadiologyWorkOrder.ID;  //objDetailsVO.ChargeDetails[chargecount].ROBID; //to update T_RadiologyOrderBooking using Id

                                if (BizActionObj.BillDetails.Details.RadiologyWorkOrder.OrderBookingDetails[count].TariffServiceID == objDetailsVO.ChargeDetails[chargecount].TariffServiceId)
                                {
                                    BizActionObj.BillDetails.Details.RadiologyWorkOrder.OrderBookingDetails[count].ID = objDetailsVO.ChargeDetails[chargecount].ROBDID;  //to update T_RadiologyOrderBookingDetails using Id
                                    BizActionObj.BillDetails.Details.RadiologyWorkOrder.OrderBookingDetails[count].ChargeID = objDetailsVO.ChargeDetails[chargecount].ID;
                                    clsRadOrderBookingDetailsVO BizAction = new clsRadOrderBookingDetailsVO();
                                    if (BizAction != null)
                                    {
                                        BizActionObj.BillDetails.Details.RadiologyWorkOrder.OrderBookingDetails[count].ID = BizAction.ID;  //to update T_RadiologyOrderBookingDetails using Id
                                        objDetailsVO.ChargeDetails[chargecount].ROBDID = BizAction.ID;

                                        BizActionObj.BillDetails.Details.RadiologyWorkOrder.OrderBookingDetails[count].TestID = BizAction.TestID;
                                    }
                                    break;
                                }
                            }
                        }
                        clsBaseRadiologyDAL objBaseDAL = clsBaseRadiologyDAL.GetInstance();
                        clsAddRadOrderBookingBizActionVO obj = new clsAddRadOrderBookingBizActionVO();
                        obj.BookingDetails = BizActionObj.BillDetails.Details.RadiologyWorkOrder;
                        obj = (clsAddRadOrderBookingBizActionVO)objBaseDAL.AddOrderBooking(obj, UserVo, con, trans);
                        if (obj.SuccessStatus == -1) throw new Exception();
                        if (obj.BookingDetails.ID > 0)
                            BizActionObj.BillDetails.Details.RadiologyWorkOrder.ID = obj.BookingDetails.ID;

                        if (BizActionObj.BillDetails.Details.DeletedChargeDetails != null && BizActionObj.BillDetails.Details.DeletedChargeDetails.Count > 0 && BizActionObj.BillDetails.Details.IsIPDBill == true)
                        {
                            foreach (clsChargeVO item in BizActionObj.BillDetails.Details.DeletedChargeDetails)
                            {
                                if (item.ID != 0 && item.TariffServiceId != 0 && BizActionObj.BillDetails.Details.RadioSpecilizationID == item.ServiceSpecilizationID)
                                {
                                    DbCommand SqlcommandR = dbServer.GetSqlStringCommand("Delete from T_RadiologyOrderBookingDetails where ChargeID = " + item.ID + " And TariffServiceId = " + item.TariffServiceId + " And UnitID = " + item.UnitID + "");
                                    int sqlStatusR = dbServer.ExecuteNonQuery(SqlcommandR, trans);
                                }
                            }
                        }
                    }


                    if (objDetailsVO.PharmacyItems.Items.Count > 0)
                    {
                        DbCommand Sqlcommand1 = dbServer.GetSqlStringCommand("Delete from T_Itemsaledetails where UnitID = " + objDetailsVO.UnitID + " And ItemSaleId in (select ID from T_ItemSale where UnitID = " + objDetailsVO.PharmacyItems.UnitId + " And BillId = " + objDetailsVO.ID + ")");
                        int sqlStatus1 = dbServer.ExecuteNonQuery(Sqlcommand1, trans);

                        DbCommand Sqlcommand2 = dbServer.GetSqlStringCommand("Delete from T_ItemSale where UnitID = " + objDetailsVO.UnitID + " And BillId = " + objDetailsVO.ID);
                        int sqlStatus2 = dbServer.ExecuteNonQuery(Sqlcommand2, trans);

                        clsBaseItemSalesDAL objBaseDAL = clsBaseItemSalesDAL.GetInstance();
                        clsAddItemSalesBizActionVO obj = new clsAddItemSalesBizActionVO();

                        obj.Details = objDetailsVO.PharmacyItems;
                        obj.Details.ItemSaleNo = objDetailsVO.PharmacyItems.ItemSaleNo;
                        obj.Details.BillID = BizActionObj.BillDetails.Details.ID;
                        obj.Details.IsBilled = BizActionObj.BillDetails.Details.IsFreezed;
                        obj = (clsAddItemSalesBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);
                        if (obj.SuccessStatus == -1) throw new Exception();

                        objDetailsVO.PharmacyItems.ID = obj.Details.ID;
                    }
                }
                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddRefundApprovalRequest");
                command1.Connection = con;
                dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.Details.ApprovalRequestID);
                dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command1, "IsOPDBill", DbType.Boolean, BizActionObj.Details.IsOPDBill);
                dbServer.AddInParameter(command1, "RequestTypeID", DbType.Int64, BizActionObj.Details.RequestTypeID);
                dbServer.AddInParameter(command1, "RequestType", DbType.String, BizActionObj.Details.RequestType);
                dbServer.AddInParameter(command1, "BillID", DbType.Int64, BizActionObj.BillDetails.Details.ID);
                dbServer.AddInParameter(command1, "BillUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command1, "ApprovalDate", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
                int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                BizActionObj.Details.ApprovalRequestID = (long)dbServer.GetParameterValue(command1, "ID");
                BizActionObj.Details.ApprovalRequestUnitID = UserVo.UserLoginInfo.UnitId;

                foreach (var item in BizActionObj.ChargeList)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddRefundChargeApprovalDetails");
                    dbServer.AddInParameter(command, "ID", DbType.Int64, item.ApprovalRequestDetailsID);
                    dbServer.AddInParameter(command, "IsRefundRequest", DbType.Boolean, BizActionObj.IsRefundRequest);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "ChargeID", DbType.Int64, item.ID);
                    dbServer.AddInParameter(command, "ChargeUnitID", DbType.Int64, item.UnitID);
                    dbServer.AddInParameter(command, "ApprovalRequestID", DbType.Int64, BizActionObj.Details.ApprovalRequestID);
                    dbServer.AddInParameter(command, "ApprovalRequestUnitID", DbType.Int64, BizActionObj.Details.ApprovalRequestUnitID);
                    dbServer.AddInParameter(command, "ApprovalStatus", DbType.Boolean, false);
                    dbServer.AddInParameter(command, "ServiceID", DbType.Int64, item.ServiceId);
                    dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    BizActionObj.SuccessStatus = Convert.ToInt32(dbServer.GetParameterValue(command, "ResultStatus"));

                    if (BizActionObj.IsRefundRequest != true)
                        break;
                }


                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.Details = null;
                BizActionObj.SuccessStatus = -1;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }

            return BizActionObj;
        }




        private clsRadOrderBookingDetailsVO GetRadilogyTestDetails(long pServiceID, clsUserVO UserVo)
        {
            clsRadOrderBookingDetailsVO BizAction = null;
            try
            {

                clsBaseRadiologyDAL objBaseDAL = clsBaseRadiologyDAL.GetInstance();
                clsGetRadTestDetailsBizActionVO obj = new clsGetRadTestDetailsBizActionVO();

                obj.ServiceID = pServiceID;
                //obj.PathOrderBookingDetailList = BizActionObj.Details.PathoWorkOrder.Items;

                obj = (clsGetRadTestDetailsBizActionVO)objBaseDAL.GetTestList(obj, UserVo);

                BizAction = new clsRadOrderBookingDetailsVO();

                if (obj != null && obj.TestList != null && obj.TestList.Count > 0)
                {
                    BizAction.TestID = obj.TestList[0].TestID;




                }



            }
            catch (Exception)
            {

                // throw;
            }

            return BizAction;
        }

        //Change By Bhushan 20012017
        private clsPathOrderBookingVO GetPathologyTestDetails(long pServiceID, clsUserVO UserVo, long pOBID, long pOBUnitID, long pChargeID, DbConnection pConnection, DbTransaction pTransaction)
        {
            clsPathOrderBookingVO BizAction = null;
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                if (pConnection != null) con = pConnection;
                else con = dbServer.CreateConnection();

                if (con.State == ConnectionState.Closed) con.Open();

                if (pTransaction != null) trans = pTransaction;
                else trans = con.BeginTransaction();

                clsBaseOrderBookingDAL objBaseDAL = clsBaseOrderBookingDAL.GetInstance();
                clsGetPathTestDetailsBizActionVO obj = new clsGetPathTestDetailsBizActionVO();

                obj.ServiceID = pServiceID;
                obj.pobID = pOBID;
                obj.pobUnitID = pOBUnitID;
                obj.pChargeID = pChargeID;
                obj = (clsGetPathTestDetailsBizActionVO)objBaseDAL.GetTestListWithDetailsID(obj, UserVo, pConnection, pTransaction);

                BizAction = new clsPathOrderBookingVO();


                if (obj != null && obj.TestList != null && obj.TestList.Count > 0)
                {

                    foreach (var item in obj.TestList)
                    {
                        clsPathOrderBookingDetailVO Obj1 = new clsPathOrderBookingDetailVO();
                        Obj1.TestID = item.TestID;
                        Obj1.IsSampleCollected = false;

                        Obj1.ID = item.POBDID;
                        BizAction.Items.Add(Obj1);
                    }

                }

            }
            catch (Exception ex)
            {
                trans.Rollback();
            }
            finally
            {
                if (pConnection == null)
                {
                    con.Close();
                    con = null;
                    trans = null;
                }
            }
            return BizAction;
        }

        private clsPathOrderBookingVO GetPathologyTestDetails(long pServiceID, clsUserVO UserVo)
        {
            clsPathOrderBookingVO BizAction = null;
            try
            {

                clsBaseOrderBookingDAL objBaseDAL = clsBaseOrderBookingDAL.GetInstance();
                clsGetPathTestDetailsBizActionVO obj = new clsGetPathTestDetailsBizActionVO();

                obj.ServiceID = pServiceID;
                obj = (clsGetPathTestDetailsBizActionVO)objBaseDAL.GetTestList(obj, UserVo);

                BizAction = new clsPathOrderBookingVO();


                if (obj != null && obj.TestList != null && obj.TestList.Count > 0)
                {

                    foreach (var item in obj.TestList)
                    {
                        clsPathOrderBookingDetailVO Obj1 = new clsPathOrderBookingDetailVO();
                        Obj1.TestID = item.TestID;
                        Obj1.IsSampleCollected = false;
                        BizAction.Items.Add(Obj1);
                    }

                }


            }
            catch (Exception)
            {

                // throw;
            }

            return BizAction;
        }

        public override IValueObject ApproveConcessionRequest(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();


            clsApproveSendRequestVO BizActionObj = valueObject as clsApproveSendRequestVO;
            DbTransaction trans = null;
            DbConnection con = null;

            try
            {

                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                clsBillVO objDetailsVO = BizActionObj.BillDetails.Details;

                DbCommand command8 = dbServer.GetStoredProcCommand("CIMS_DeleteOnlyChargeDetails");
                dbServer.AddInParameter(command8, "BillID", DbType.Int64, objDetailsVO.ID);
                dbServer.AddInParameter(command8, "UnitID", DbType.Int64, objDetailsVO.UnitID);
                dbServer.AddInParameter(command8, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);

                int intStatus8 = dbServer.ExecuteNonQuery(command8, trans);

                long ParentID = 0;//T_charge.id
                long CDParentID = 0;//T_chargeDetails.id

                long chargeID = 0;
                double TestCharge = 0;

                for (int i = 0; i < objDetailsVO.ChargeDetails.Count; i++)
                {
                    if (objDetailsVO.ChargeDetails[i].ChildPackageService == false)
                    {
                        clsBaseChargeDAL objBaseDAL = clsBaseChargeDAL.GetInstance();
                        clsAddChargeBizActionVO obj = new clsAddChargeBizActionVO();

                        obj.Details = objDetailsVO.ChargeDetails[i];
                        obj.Details.IsBilled = BizActionObj.BillDetails.Details.IsFreezed;

                        obj.Details.Date = objDetailsVO.Date;

                        if (obj.Details.Status == true)
                            obj.Details.PaidAmount = obj.Details.NetAmount;
                        else
                            obj.Details.PaidAmount = 0;

                        obj.Details.Opd_Ipd_External_Id = objDetailsVO.Opd_Ipd_External_Id;
                        obj.Details.Opd_Ipd_External_UnitId = objDetailsVO.Opd_Ipd_External_UnitId;
                        obj.Details.Opd_Ipd_External = objDetailsVO.Opd_Ipd_External;
                        obj.Details.ClassId = objDetailsVO.ChargeDetails[i].ClassId;
                        obj.Details.IsIPDBill = objDetailsVO.IsIPDBill;    // For Service date Column Only In Case IPD BILL In T_Charge 
                        //  obj.Details.ID = 0;
                        obj = (clsAddChargeBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans, 0, 0);  //obj = (clsAddChargeBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);
                        if (obj.SuccessStatus == -1) throw new Exception();
                        obj.Details.ID = obj.Details.ID;
                        objDetailsVO.ChargeDetails[i].ID = obj.Details.ID;

                        ParentID = objDetailsVO.ChargeDetails[i].ID;
                        CDParentID = objDetailsVO.ChargeDetails[i].ChargeDetails.ID;
                        var _List = from charge in objDetailsVO.ChargeDetails
                                    where (charge.PackageID.Equals(objDetailsVO.ChargeDetails[i].PackageID) && charge.ChildPackageService == true)
                                    select charge;
                        foreach (var item in _List)
                        {
                            clsBaseChargeDAL objBaseNewDAL = clsBaseChargeDAL.GetInstance();
                            clsAddChargeBizActionVO objCharge = new clsAddChargeBizActionVO();
                            //item.ID = 0;
                            objCharge.Details = item;
                            if (objCharge.Details.Status == true)
                                objCharge.Details.PaidAmount = item.NetAmount;
                            else
                                objCharge.Details.PaidAmount = 0;
                            objCharge.Details.IsFromApprovedRequest = true;
                            objCharge.Details.IsBilled = BizActionObj.BillDetails.Details.IsFreezed;
                            objCharge.Details.Date = objDetailsVO.Date;
                            objCharge.Details.Opd_Ipd_External_Id = objDetailsVO.Opd_Ipd_External_Id;
                            objCharge.Details.Opd_Ipd_External_UnitId = objDetailsVO.Opd_Ipd_External_UnitId;
                            objCharge.Details.Opd_Ipd_External = objDetailsVO.Opd_Ipd_External;
                            obj.Details.ClassId = objDetailsVO.ChargeDetails[i].ClassId;
                            objCharge = (clsAddChargeBizActionVO)objBaseNewDAL.Add(objCharge, UserVo, con, trans, ParentID, CDParentID);
                            if (objCharge.SuccessStatus == -1) throw new Exception();
                            item.ID = objCharge.Details.ID;//T_Charge.ID
                            item.ChargeDetails.ID = objCharge.Details.ChargeDetails.ID; //T_ChargeDetails.ID
                        }
                    }
                }

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateBill");
                dbServer.AddInParameter(command, "IsPackageBill", DbType.Int64, BizActionObj.BillDetails.IsPackageBill);
                //dbServer.AddInParameter(command, "PackageID", DbType.Int64, BizActionObj.BillDetails.PackageID);

                if (BizActionObj.BillDetails.IsPackageBill == true)
                {
                    StringBuilder sbPackageList = new StringBuilder();

                    foreach (var item in objDetailsVO.ChargeDetails)
                    {
                        if (item.PackageID > 0 && item.isPackageService == true)
                        {
                            sbPackageList.Append("," + Convert.ToString(item.PackageID));
                        }
                    }

                    sbPackageList.Replace(",", "", 0, 1);

                    dbServer.AddInParameter(command, "ipPackageList", DbType.String, Convert.ToString(sbPackageList));
                }

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                if (objDetailsVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);

                dbServer.AddInParameter(command, "TotalBillAmount", DbType.Double, objDetailsVO.TotalBillAmount);
                dbServer.AddInParameter(command, "TotalConcessionAmount", DbType.Double, objDetailsVO.TotalConcessionAmount);
                dbServer.AddInParameter(command, "NetBillAmount", DbType.Double, objDetailsVO.NetBillAmount);
                dbServer.AddInParameter(command, "SelfAmount", DbType.Double, objDetailsVO.SelfAmount);
                if (objDetailsVO.BalanceAmountSelf < 0) objDetailsVO.BalanceAmountSelf = 0;
                dbServer.AddInParameter(command, "BalanceAmountSelf", DbType.Double, objDetailsVO.BalanceAmountSelf);
                dbServer.AddInParameter(command, "NonSelfAmount", DbType.Double, objDetailsVO.NonSelfAmount);
                dbServer.AddInParameter(command, "BalanceAmountNonSelf", DbType.Double, objDetailsVO.BalanceAmountNonSelf);

                dbServer.AddInParameter(command, "IsPrinted", DbType.Boolean, objDetailsVO.IsPrinted);
                dbServer.AddInParameter(command, "SponserType", DbType.Boolean, objDetailsVO.SponserType);
                if (objDetailsVO.Remark != null) objDetailsVO.Remark = objDetailsVO.Remark.Trim();
                dbServer.AddInParameter(command, "Remark", DbType.String, objDetailsVO.Remark);
                if (objDetailsVO.BillRemark != null) objDetailsVO.BillRemark = objDetailsVO.BillRemark.Trim();
                dbServer.AddInParameter(command, "BillRemark", DbType.String, objDetailsVO.BillRemark);
                dbServer.AddInParameter(command, "BillType", DbType.Int16, (Int16)objDetailsVO.BillType);
                dbServer.AddInParameter(command, "BillPaymentType", DbType.Int16, (Int16)objDetailsVO.BillPaymentType);
                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, objDetailsVO.IsFreezed);

                // Added by Changdeo
                dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);  //Costing Divisions for Clinical & Pharmacy Billing

                dbServer.AddInParameter(command, "ConcessionreasonId", DbType.Int64, objDetailsVO.ConcessionReasonId);//Added By Yogesh K
                dbServer.AddInParameter(command, "ConcessionRemark", DbType.String, objDetailsVO.ConcessionRemark); //Added By Bhushanp 09032017
                dbServer.AddInParameter(command, "GrossDiscountReason", DbType.Int64, objDetailsVO.GrossDiscountReasonID);
                dbServer.AddInParameter(command, "GrossDiscountPercentage", DbType.Double, objDetailsVO.GrossDiscountPercentage);
                dbServer.AddInParameter(command, "ConcessionAuthorizedBy", DbType.Int64, objDetailsVO.ConcessionAuthorizedBy);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDetailsVO.UnitID);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddInParameter(command, "ID", DbType.Int64, objDetailsVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                StringBuilder ChargeIdListList = new StringBuilder();
                StringBuilder SponsorTypeList = new StringBuilder();
                StringBuilder BalanceAmountList = new StringBuilder();
                StringBuilder StatusList = new StringBuilder();

                for (int UnitCount = 0; UnitCount < objDetailsVO.ChargeDetails.Count; UnitCount++)
                {
                    ChargeIdListList.Append(objDetailsVO.ChargeDetails[UnitCount].ID);
                    StatusList.Append(objDetailsVO.ChargeDetails[UnitCount].Status);
                    BalanceAmountList.Append(objDetailsVO.ChargeDetails[UnitCount].NetAmount - objDetailsVO.ChargeDetails[UnitCount].PaidAmount);
                    SponsorTypeList.Append(objDetailsVO.ChargeDetails[UnitCount].SponsorType);

                    if (UnitCount < (objDetailsVO.ChargeDetails.Count - 1))
                    {
                        ChargeIdListList.Append(",");
                        StatusList.Append(",");
                        BalanceAmountList.Append(",");
                        SponsorTypeList.Append(",");
                    }
                }

                dbServer.AddInParameter(command, "ChargeIdList", DbType.String, ChargeIdListList.ToString());
                dbServer.AddInParameter(command, "StatusList", DbType.String, StatusList.ToString());
                dbServer.AddInParameter(command, "SponsorTypeList", DbType.String, SponsorTypeList.ToString());
                dbServer.AddInParameter(command, "BalanceAmountList", DbType.String, BalanceAmountList.ToString());

                //by Anjali........................
                dbServer.AddInParameter(command, "IsFromApprovedRequest", DbType.Boolean, true);
                //.....................

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddApprovedRequest");
                command.Connection = con;
                dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.ID);
                dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command2, "ApprovalRequestID", DbType.Int64, BizActionObj.ApprovalRequestID);
                dbServer.AddInParameter(command2, "ApprovalRequestUnitID", DbType.Int64, BizActionObj.ApprovalRequestUnitID);
                dbServer.AddInParameter(command2, "ApprovalRemark", DbType.String, BizActionObj.ApprovalRemark);
                dbServer.AddInParameter(command2, "LevelID", DbType.Int64, BizActionObj.LevelID);
                dbServer.AddInParameter(command2, "IsApproved", DbType.Boolean, BizActionObj.IsApproved);
                dbServer.AddInParameter(command2, "ApprovedBy", DbType.Int64, BizActionObj.ApprovedByID);
                dbServer.AddInParameter(command2, "ApprovedByName", DbType.String, BizActionObj.ApprovedByName);
                dbServer.AddInParameter(command2, "ApprovedDateTime", DbType.DateTime, BizActionObj.ApprovedDateTime);
                dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus1 = dbServer.ExecuteNonQuery(command2, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                BizActionObj.ApprovedRequestID = (long)dbServer.GetParameterValue(command2, "ID");
                BizActionObj.ApprovedRequestUnitID = UserVo.UserLoginInfo.UnitId;


                foreach (var item in BizActionObj.ApprovalChargeList)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddApprovalRequestDetailsHistroy");
                    dbServer.AddInParameter(command1, "ID", DbType.Int64, item.ApprovalHistoryID);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "ApprovedRequestID", DbType.Int64, BizActionObj.ApprovedRequestID);
                    dbServer.AddInParameter(command1, "ApprovedRequestUnitID", DbType.Int64, BizActionObj.ApprovedRequestUnitID);
                    dbServer.AddInParameter(command1, "ApprovalRequestID", DbType.Int64, item.ApprovalRequestID);
                    dbServer.AddInParameter(command1, "ApprovalRequestUnitID", DbType.Int64, item.ApprovalRequestUnitID);
                    dbServer.AddInParameter(command1, "ChargeID", DbType.Int64, item.ID);
                    dbServer.AddInParameter(command1, "ChargeUnitID", DbType.Int64, item.UnitID);
                    dbServer.AddInParameter(command1, "ServiceID", DbType.Int64, item.ServiceId);
                    dbServer.AddInParameter(command1, "ConcessionPercent", DbType.Single, item.ConcessionPercent);
                    dbServer.AddInParameter(command1, "ConcessionAmount", DbType.Single, item.ConcessionAmount);
                    dbServer.AddInParameter(command1, "ApprovalStatus", DbType.Boolean, item.SelectCharge);
                    dbServer.AddInParameter(command1, "ApprovalRemark", DbType.String, BizActionObj.ApprovalRemark);
                    dbServer.AddInParameter(command1, "ApprovedBy", DbType.Int64, BizActionObj.ApprovedByID);
                    dbServer.AddInParameter(command1, "ApprovedDateTime", DbType.DateTime, BizActionObj.ApprovedDateTime);
                    dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus2 = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = Convert.ToInt32(dbServer.GetParameterValue(command1, "ResultStatus"));
                }




                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }

            return BizActionObj;
        }
        public override IValueObject DeleteApprovalRequest(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();


            clsDeleteApprovalRequestVO BizActionObj = valueObject as clsDeleteApprovalRequestVO;

            DbConnection con = null;
            try
            {

                DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_DeleteApprovalRequest");
                dbServer.AddInParameter(command2, "ApprovalRequestID", DbType.Int64, BizActionObj.ApprovalRequestID);
                dbServer.AddInParameter(command2, "ApprovalRequestUnitID", DbType.Int64, BizActionObj.ApprovalRequestUnitID);
                dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus1 = dbServer.ExecuteNonQuery(command2);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                //con.Close();
            }
            return BizActionObj;
        }

        private void SetLogInfo(List<LogInfo> objLogList, long userID)
        {
            try
            {
                if (objLogList != null && objLogList.Count > 0)
                {
                    foreach (LogInfo itemLog in objLogList)
                    {
                        logManager.LogInfo(itemLog.guid, userID, itemLog.TimeStamp, itemLog.ClassName, itemLog.MethodName, itemLog.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                logManager.LogError(userID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(),
                    MethodBase.GetCurrentMethod().ToString(), ex.ToString());
            }
        }


        public override IValueObject SendApprovalRequestForAdvanceRefundDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsSendRequestForApprovalVO BizActionObj = valueObject as clsSendRequestForApprovalVO;

            DbConnection pConnection = null;
            DbTransaction trans = null;

            try
            {

                pConnection = dbServer.CreateConnection();
                if (pConnection.State == ConnectionState.Closed) pConnection.Open();
                trans = pConnection.BeginTransaction();
                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddRefundApprovalRequest");
                command1.Connection = pConnection;
                dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.Details.ApprovalRequestID);
                dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command1, "IsOPDBill", DbType.Boolean, BizActionObj.Details.IsOPDBill);
                dbServer.AddInParameter(command1, "RequestTypeID", DbType.Int64, BizActionObj.Details.RequestTypeID);
                dbServer.AddInParameter(command1, "RequestType", DbType.String, BizActionObj.Details.RequestType);
                dbServer.AddInParameter(command1, "BillID", DbType.Int64, BizActionObj.Details.BillID);
                dbServer.AddInParameter(command1, "BillUnitID", DbType.Int64, BizActionObj.Details.BillUnitID);
                dbServer.AddInParameter(command1, "ApprovalDate", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command1, "Amount", DbType.Double, BizActionObj.RefundAmount);
                dbServer.AddInParameter(command1, "IsAgainstBill", DbType.Boolean, BizActionObj.IsAgainstBill);
                dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command1, "AdvanceID", DbType.Int64, BizActionObj.Details.AdvanceID);
                dbServer.AddInParameter(command1, "AdvanceUnitID", DbType.Int64, BizActionObj.Details.AdvanceUnitID);
                int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                BizActionObj.Details.ApprovalRequestID = (long)dbServer.GetParameterValue(command1, "ID");
                BizActionObj.Details.ApprovalRequestUnitID = UserVo.UserLoginInfo.UnitId;


                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddRefundApprovalRequestDetails");
                dbServer.AddInParameter(command, "ID", DbType.Int64, 0);
                dbServer.AddInParameter(command, "IsRefundRequest", DbType.Boolean, true);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "ChargeID", DbType.Int64, 0);
                dbServer.AddInParameter(command, "ChargeUnitID", DbType.Int64, 0);
                dbServer.AddInParameter(command, "ApprovalRequestID", DbType.Int64, BizActionObj.Details.ApprovalRequestID);
                dbServer.AddInParameter(command, "ApprovalRequestUnitID", DbType.Int64, BizActionObj.Details.ApprovalRequestUnitID);
                dbServer.AddInParameter(command, "ApprovalStatus", DbType.Boolean, false);
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, 0);
                dbServer.AddInParameter(command, "ApprovalRequestRemark", DbType.String, BizActionObj.Details.Remarks);
                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = Convert.ToInt32(dbServer.GetParameterValue(command, "ResultStatus"));


                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                BizActionObj.SuccessStatus = -1;
                if (pConnection == null) trans.Rollback();
            }
            finally
            {
                if (pConnection != null)
                {
                    pConnection.Close();
                    pConnection = null;
                    trans = null;
                }
            }

            return BizActionObj;
        }
        //Created New By Bhushanp 
        public override IValueObject ApproveAdvanceRefundRequestDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsApproveSendRequestVO BizActionObj = valueObject as clsApproveSendRequestVO;
            DbConnection con = null;
            DbTransaction trans = null;

            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddApprovedRequest");
                command.Connection = con;
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.ApprovalRequestUnitID);
                dbServer.AddInParameter(command, "ApprovalRequestID", DbType.Int64, BizActionObj.ApprovalRequestID);
                dbServer.AddInParameter(command, "ApprovalRequestUnitID", DbType.Int64, BizActionObj.ApprovalRequestUnitID);
                dbServer.AddInParameter(command, "ApprovalRemark", DbType.String, BizActionObj.ApprovalRemark);
                dbServer.AddInParameter(command, "LevelID", DbType.Int64, BizActionObj.LevelID);
                dbServer.AddInParameter(command, "Amount", DbType.Int64, BizActionObj.RefundAmount);
                dbServer.AddInParameter(command, "IsApproved", DbType.Boolean, BizActionObj.IsApproved);
                dbServer.AddInParameter(command, "ApprovedBy", DbType.Int64, BizActionObj.ApprovedByID);
                dbServer.AddInParameter(command, "ApprovedByName", DbType.String, BizActionObj.ApprovedByName);
                dbServer.AddInParameter(command, "ApprovedDateTime", DbType.DateTime, BizActionObj.ApprovedDateTime);
                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus1 = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.ApprovedRequestID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.ApprovedRequestUnitID = BizActionObj.ApprovalRequestUnitID;


                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddApprovalRequestDetails");
                dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                dbServer.AddInParameter(command1, "UnitID", DbType.Int64, BizActionObj.ApprovalRequestUnitID);
                dbServer.AddInParameter(command1, "ApprovedRequestID", DbType.Int64, BizActionObj.ApprovedRequestID);
                dbServer.AddInParameter(command1, "ApprovedRequestUnitID", DbType.Int64, BizActionObj.ApprovedRequestUnitID);
                dbServer.AddInParameter(command1, "ApprovalRequestID", DbType.Int64, BizActionObj.ApprovalRequestID);
                dbServer.AddInParameter(command1, "ApprovalRequestUnitID", DbType.Int64, BizActionObj.ApprovalRequestUnitID);
                dbServer.AddInParameter(command1, "ChargeID", DbType.Int64, 0);
                dbServer.AddInParameter(command1, "ChargeUnitID", DbType.Int64, 0);
                dbServer.AddInParameter(command1, "ServiceID", DbType.Int64, 0);
                dbServer.AddInParameter(command1, "ApprovalStatus", DbType.Boolean, BizActionObj.IsApproved);
                dbServer.AddInParameter(command1, "ApprovalRemark", DbType.String, BizActionObj.ApprovalRemark);
                dbServer.AddInParameter(command1, "ApprovedBy", DbType.Int64, BizActionObj.ApprovedByID);
                dbServer.AddInParameter(command1, "ApprovedDateTime", DbType.DateTime, BizActionObj.ApprovedDateTime);
                dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command1, trans);
                BizActionObj.SuccessStatus = Convert.ToInt32(dbServer.GetParameterValue(command1, "ResultStatus"));

                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                    con = null;
                    trans = null;
                }
            }
            return BizActionObj;
        }

        //Begin:: Added by AniketK on 30-Jan-2019
        public override IValueObject GetRefundReceiptList(IValueObject valueObject, clsUserVO UserVo)
        {
            {
                clsGetRefundReceiptListBizActionVO BizActionObj = valueObject as clsGetRefundReceiptListBizActionVO;
                try
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetRefundReceipt");

                    dbServer.AddInParameter(command, "BillID", DbType.Int64, BizActionObj.BillID);
                    dbServer.AddInParameter(command, "BillUnitID", DbType.Int64, BizActionObj.BillUnitID);
                    //dbServer.AddInParameter(command, "Opd_Ipd_External", DbType.Int64, BizActionObj.Opd_Ipd_External);

                    DbDataReader reader;

                    //int intStatus = dbServer.ExecuteNonQuery(command);
                    reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.Details == null)
                            BizActionObj.Details = new List<clsRefundVO>();
                        while (reader.Read())
                        {
                            clsRefundVO objDetails = new clsRefundVO();

                            objDetails.ID = (long)DALHelper.HandleDBNull(reader["RefundID"]);
                            objDetails.Date = (DateTime?)DALHelper.HandleDate(reader["RefundDate"]);
                            objDetails.Amount = (double)DALHelper.HandleDBNull(reader["RefundAmount"]);
                            objDetails.BillID = (long)DALHelper.HandleDBNull(reader["BillID"]);
                            objDetails.Remarks = (String)DALHelper.HandleDBNull(reader["Remarks"]);
                            objDetails.ReceiptNo = (String)DALHelper.HandleDBNull(reader["ReceiptNo"]);
                            objDetails.UnitID = (Int64)DALHelper.HandleDBNull(reader["UnitID"]);
                            objDetails.CostingDivisionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CostingDivisionID"]));
                            objDetails.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                            objDetails.AddedBy = (Int64)DALHelper.HandleDBNull(reader["AddedBy"]);
                            objDetails.AddedOn = (String)DALHelper.HandleDBNull(reader["AddedOn"]);
                            objDetails.AddedDateTime = (DateTime?)DALHelper.HandleDate(reader["AddedDateTime"]);
                            objDetails.AddedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]);
                            objDetails.UpdatedBy = (Int64?)DALHelper.HandleDBNull(reader["UpdatedBy"]);
                            objDetails.UpdatedOn = (String)DALHelper.HandleDBNull(reader["UpdatedOn"]);
                            objDetails.UpdatedDateTime = (DateTime?)DALHelper.HandleDate(reader["UpdatedDateTime"]);
                            objDetails.UpdateWindowsLoginName = (String)DALHelper.HandleDBNull(reader["UpdateWindowsLoginName"]);
                            objDetails.UnitName = (String)DALHelper.HandleDBNull(reader["UnitName"]);

                            BizActionObj.Details.Add(objDetails);

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
        }

        //End:: Added by AniketK on 30-Jan-2019
    }
}
