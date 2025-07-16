namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.CustomExceptions;
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Billing;
    using PalashDynamics.ValueObjects.Inventory;
    using PalashDynamics.ValueObjects.Log;
    using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
    using PalashDynamics.ValueObjects.Pathology;
    using PalashDynamics.ValueObjects.Radiology;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public class clsRefundDAL : clsBaseRefundDAL
    {
        private Database dbServer;
        private LogManager logManager;
        private bool IsAuditTrail;

        private clsRefundDAL()
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
            clsAddRefundBizActionVO bizActionObj = valueObject as clsAddRefundBizActionVO;
            bizActionObj = (bizActionObj.Details.ID != 0L) ? this.UpdateDetails(bizActionObj, null, null) : this.AddDetails(bizActionObj, UserVo, null, null);
            return valueObject;
        }

        public override IValueObject Add(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            clsAddRefundBizActionVO bizActionObj = valueObject as clsAddRefundBizActionVO;
            bizActionObj = (bizActionObj.Details.ID != 0L) ? this.UpdateDetails(bizActionObj, pConnection, pTransaction) : this.AddDetails(bizActionObj, UserVo, pConnection, pTransaction);
            return valueObject;
        }

        private clsAddRefundBizActionVO AddDetails(clsAddRefundBizActionVO BizActionObj, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
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
                clsRefundVO details = BizActionObj.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddRefund");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, details.LinkServer);
                if (details.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, details.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, details.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemSaleReturnID", DbType.Int64, details.ItemSaleReturnID);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyID", DbType.Int64, details.CompanyID);
                this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.Int64, details.BillID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, details.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Amount", DbType.Double, details.Amount);
                this.dbServer.AddInParameter(storedProcCommand, "AdvanceId", DbType.Int64, details.AdvanceID);
                this.dbServer.AddInParameter(storedProcCommand, "ReceiptPrinted", DbType.Boolean, details.ReceiptPrinted);
                if (details.Remarks != null)
                {
                    details.Remarks = details.Remarks.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, details.Remarks);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, details.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CostingDivisionID", DbType.Int64, details.CostingDivisionID);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, details.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "ApprovalRequestID", DbType.Int64, details.ApprovalRequestID);
                this.dbServer.AddInParameter(storedProcCommand, "ApprovalRequestUnitID", DbType.Int64, details.ApprovalRequestUnitID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0x7fffffff);
                this.dbServer.AddParameter(storedProcCommand, "ReceiptNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "00000000000000000000000000000000000000000000000000000000000000000000");
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                BizActionObj.Details.ReceiptNo = Convert.ToString(this.dbServer.GetParameterValue(storedProcCommand, "ReceiptNo"));
                if (BizActionObj.Details.PaymentDetails != null)
                {
                    clsBasePaymentDAL instance = clsBasePaymentDAL.GetInstance();
                    clsAddPaymentBizActionVO valueObject = new clsAddPaymentBizActionVO {
                        Details = new clsPaymentVO()
                    };
                    valueObject.Details = BizActionObj.Details.PaymentDetails;
                    valueObject.Details.RefundID = BizActionObj.Details.ID;
                    valueObject.Details.RefundAmount = details.Amount;
                    valueObject.Details.Date = BizActionObj.Details.Date;
                    valueObject.Details.LinkServer = BizActionObj.Details.LinkServer;
                    valueObject.Details.CostingDivisionID = BizActionObj.Details.CostingDivisionID;
                    valueObject = (clsAddPaymentBizActionVO) instance.Add(valueObject, UserVo, connection, transaction);
                    if (valueObject.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    BizActionObj.Details.PaymentDetails.ID = valueObject.Details.ID;
                    if (BizActionObj.IsRefundToAdvance)
                    {
                        clsBaseAdvanceDAL edal = clsBaseAdvanceDAL.GetInstance();
                        clsAddAdvanceBizActionVO nvo2 = new clsAddAdvanceBizActionVO {
                            IsRefundToAdvance = BizActionObj.IsRefundToAdvance,
                            BillID = details.BillID,
                            Details = new clsAdvanceVO()
                        };
                        nvo2.Details.PatientID = BizActionObj.RefundToAdvancePatientID;
                        nvo2.Details.PatientUnitID = BizActionObj.RefundToAdvancePatientUnitID;
                        nvo2.Details.Date = details.Date;
                        nvo2.Details.Total = details.Amount;
                        nvo2.Details.AdvanceTypeId = 2L;
                        nvo2.Details.Balance = details.Amount;
                        nvo2.Details.AdvanceAgainstId = BizActionObj.IsExchangeMaterial ? ((long) 3) : ((long) 1);
                        nvo2.Details.Remarks = BizActionObj.Details.ReceiptNo;
                        nvo2.Details.Status = details.Status;
                        nvo2.Details.CostingDivisionID = details.CostingDivisionID;
                        nvo2.Details.FromRefundID = BizActionObj.Details.ID;
                        clsAddPaymentBizActionVO nvo3 = new clsAddPaymentBizActionVO {
                            Details = new clsPaymentVO()
                        };
                        nvo3.Details.Date = valueObject.Details.Date;
                        nvo3.Details.AdvanceID = nvo2.Details.ID;
                        nvo3.Details.AdvanceAmount = valueObject.Details.RefundAmount;
                        nvo3.Details.Remarks = BizActionObj.Details.ReceiptNo;
                        nvo3.Details.CostingDivisionID = details.CostingDivisionID;
                        nvo3.Details.LinkServer = BizActionObj.Details.LinkServer;
                        nvo3.Details.PaymentDetails = new List<clsPaymentDetailsVO>();
                        clsPaymentDetailsVO item = new clsPaymentDetailsVO {
                            LinkServer = BizActionObj.Details.LinkServer,
                            PaymentModeID = details.PaymentDetails.PaymentDetails[0].PaymentModeID,
                            PaidAmount = valueObject.Details.RefundAmount,
                            Status = details.Status
                        };
                        nvo3.Details.PaymentDetails.Add(item);
                        nvo3.Details.CostingDivisionID = BizActionObj.Details.CostingDivisionID;
                        nvo2.Details.PaymentDetails = nvo3.Details;
                        if (((clsAddAdvanceBizActionVO) edal.AddAdvance(nvo2, UserVo, connection, transaction)).SuccessStatus == -1)
                        {
                            throw new Exception();
                        }
                    }
                    if (this.IsAuditTrail && (BizActionObj.LogInfoList != null))
                    {
                        LogInfo item = new LogInfo();
                        Guid guid = default(Guid);
                        item.guid = guid;
                        item.UserId = UserVo.ID;
                        item.TimeStamp = DateTime.Now;
                        item.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                        item.MethodName = MethodBase.GetCurrentMethod().ToString();
                        string[] strArray = new string[] { " 28 : Data After Refund Services  \r\nUser Unit Id : ", Convert.ToString(UserVo.UserLoginInfo.UnitId), " Patient ID : ", Convert.ToString(details.PatientID), " Bill ID : ", Convert.ToString(details.BillID), " Costing Division ID : ", Convert.ToString(details.CostingDivisionID), " Amount : " };
                        strArray[9] = Convert.ToString(details.Amount);
                        strArray[10] = " Advance ID : ";
                        strArray[11] = Convert.ToString(details.AdvanceID);
                        strArray[12] = " Remarks : ";
                        strArray[13] = Convert.ToString(details.Remarks);
                        strArray[14] = " Refund ID : ";
                        strArray[15] = Convert.ToString(BizActionObj.Details.ID);
                        strArray[0x10] = " Receipt No : ";
                        strArray[0x11] = Convert.ToString(BizActionObj.Details.ReceiptNo);
                        strArray[0x12] = " ";
                        item.Message = string.Concat(strArray);
                        item.Message = item.Message.Replace("\r\n", Environment.NewLine);
                        BizActionObj.LogInfoList.Add(item);
                    }
                }
                if (pConnection == null)
                {
                    transaction.Commit();
                }
                BizActionObj.SuccessStatus = 0;
                if (((BizActionObj.LogInfoList != null) && (BizActionObj.LogInfoList.Count > 0)) && this.IsAuditTrail)
                {
                    this.SetLogInfo(BizActionObj.LogInfoList, UserVo.ID);
                    BizActionObj.LogInfoList.Clear();
                }
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
                    connection.Close();
                    connection = null;
                    transaction = null;
                }
            }
            return BizActionObj;
        }

        public override IValueObject ApproveAdvanceRefundRequestDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsApproveSendRequestVO tvo = valueObject as clsApproveSendRequestVO;
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
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddApprovedRequest");
                storedProcCommand.Connection = connection;
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, tvo.ApprovalRequestUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ApprovalRequestID", DbType.Int64, tvo.ApprovalRequestID);
                this.dbServer.AddInParameter(storedProcCommand, "ApprovalRequestUnitID", DbType.Int64, tvo.ApprovalRequestUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ApprovalRemark", DbType.String, tvo.ApprovalRemark);
                this.dbServer.AddInParameter(storedProcCommand, "LevelID", DbType.Int64, tvo.LevelID);
                this.dbServer.AddInParameter(storedProcCommand, "Amount", DbType.Int64, tvo.RefundAmount);
                this.dbServer.AddInParameter(storedProcCommand, "IsApproved", DbType.Boolean, tvo.IsApproved);
                this.dbServer.AddInParameter(storedProcCommand, "ApprovedBy", DbType.Int64, tvo.ApprovedByID);
                this.dbServer.AddInParameter(storedProcCommand, "ApprovedByName", DbType.String, tvo.ApprovedByName);
                this.dbServer.AddInParameter(storedProcCommand, "ApprovedDateTime", DbType.DateTime, tvo.ApprovedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                tvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                tvo.ApprovedRequestID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                tvo.ApprovedRequestUnitID = tvo.ApprovalRequestUnitID;
                DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddApprovalRequestDetails");
                this.dbServer.AddInParameter(command2, "ID", DbType.Int64, 0);
                this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, tvo.ApprovalRequestUnitID);
                this.dbServer.AddInParameter(command2, "ApprovedRequestID", DbType.Int64, tvo.ApprovedRequestID);
                this.dbServer.AddInParameter(command2, "ApprovedRequestUnitID", DbType.Int64, tvo.ApprovedRequestUnitID);
                this.dbServer.AddInParameter(command2, "ApprovalRequestID", DbType.Int64, tvo.ApprovalRequestID);
                this.dbServer.AddInParameter(command2, "ApprovalRequestUnitID", DbType.Int64, tvo.ApprovalRequestUnitID);
                this.dbServer.AddInParameter(command2, "ChargeID", DbType.Int64, 0);
                this.dbServer.AddInParameter(command2, "ChargeUnitID", DbType.Int64, 0);
                this.dbServer.AddInParameter(command2, "ServiceID", DbType.Int64, 0);
                this.dbServer.AddInParameter(command2, "ApprovalStatus", DbType.Boolean, tvo.IsApproved);
                this.dbServer.AddInParameter(command2, "ApprovalRemark", DbType.String, tvo.ApprovalRemark);
                this.dbServer.AddInParameter(command2, "ApprovedBy", DbType.Int64, tvo.ApprovedByID);
                this.dbServer.AddInParameter(command2, "ApprovedDateTime", DbType.DateTime, tvo.ApprovedDateTime);
                this.dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(command2, transaction);
                tvo.SuccessStatus = Convert.ToInt32(this.dbServer.GetParameterValue(command2, "ResultStatus"));
                transaction.Commit();
                tvo.SuccessStatus = 0;
            }
            catch (Exception)
            {
                tvo.SuccessStatus = -1;
                tvo.SuccessStatus = -1;
                transaction.Rollback();
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection = null;
                    transaction = null;
                }
            }
            return tvo;
        }

        public override IValueObject ApproveConcessionRequest(IValueObject valueObject, clsUserVO UserVo)
        {
            clsApproveSendRequestVO tvo = valueObject as clsApproveSendRequestVO;
            DbTransaction transaction = null;
            DbConnection pConnection = null;
            try
            {
                pConnection = this.dbServer.CreateConnection();
                pConnection.Open();
                transaction = pConnection.BeginTransaction();
                clsBillVO objDetailsVO = tvo.BillDetails.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteOnlyChargeDetails");
                this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.Int64, objDetailsVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objDetailsVO.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                long iParentID = 0L;
                long iCDParentID = 0L;
                Func<clsChargeVO, bool> predicate = null;
                int i = 0;
                while (true)
                {
                    if (i >= objDetailsVO.ChargeDetails.Count)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_UpdateBill");
                        this.dbServer.AddInParameter(command2, "IsPackageBill", DbType.Int64, tvo.BillDetails.IsPackageBill);
                        if (tvo.BillDetails.IsPackageBill)
                        {
                            StringBuilder builder = new StringBuilder();
                            foreach (clsChargeVO evo2 in objDetailsVO.ChargeDetails)
                            {
                                if ((evo2.PackageID > 0L) && evo2.isPackageService)
                                {
                                    builder.Append("," + Convert.ToString(evo2.PackageID));
                                }
                            }
                            builder.Replace(",", "", 0, 1);
                            this.dbServer.AddInParameter(command2, "ipPackageList", DbType.String, Convert.ToString(builder));
                        }
                        this.dbServer.AddInParameter(command2, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                        if (objDetailsVO.LinkServer != null)
                        {
                            this.dbServer.AddInParameter(command2, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));
                        }
                        this.dbServer.AddInParameter(command2, "Date", DbType.DateTime, objDetailsVO.Date);
                        this.dbServer.AddInParameter(command2, "TotalBillAmount", DbType.Double, objDetailsVO.TotalBillAmount);
                        this.dbServer.AddInParameter(command2, "TotalConcessionAmount", DbType.Double, objDetailsVO.TotalConcessionAmount);
                        this.dbServer.AddInParameter(command2, "NetBillAmount", DbType.Double, objDetailsVO.NetBillAmount);
                        this.dbServer.AddInParameter(command2, "SelfAmount", DbType.Double, objDetailsVO.SelfAmount);
                        if (objDetailsVO.BalanceAmountSelf < 0.0)
                        {
                            objDetailsVO.BalanceAmountSelf = 0.0;
                        }
                        this.dbServer.AddInParameter(command2, "BalanceAmountSelf", DbType.Double, objDetailsVO.BalanceAmountSelf);
                        this.dbServer.AddInParameter(command2, "NonSelfAmount", DbType.Double, objDetailsVO.NonSelfAmount);
                        this.dbServer.AddInParameter(command2, "BalanceAmountNonSelf", DbType.Double, objDetailsVO.BalanceAmountNonSelf);
                        this.dbServer.AddInParameter(command2, "IsPrinted", DbType.Boolean, objDetailsVO.IsPrinted);
                        this.dbServer.AddInParameter(command2, "SponserType", DbType.Boolean, objDetailsVO.SponserType);
                        if (objDetailsVO.Remark != null)
                        {
                            objDetailsVO.Remark = objDetailsVO.Remark.Trim();
                        }
                        this.dbServer.AddInParameter(command2, "Remark", DbType.String, objDetailsVO.Remark);
                        if (objDetailsVO.BillRemark != null)
                        {
                            objDetailsVO.BillRemark = objDetailsVO.BillRemark.Trim();
                        }
                        this.dbServer.AddInParameter(command2, "BillRemark", DbType.String, objDetailsVO.BillRemark);
                        this.dbServer.AddInParameter(command2, "BillType", DbType.Int16, (short) objDetailsVO.BillType);
                        this.dbServer.AddInParameter(command2, "BillPaymentType", DbType.Int16, (short) objDetailsVO.BillPaymentType);
                        this.dbServer.AddInParameter(command2, "IsFreezed", DbType.Boolean, objDetailsVO.IsFreezed);
                        this.dbServer.AddInParameter(command2, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);
                        this.dbServer.AddInParameter(command2, "ConcessionreasonId", DbType.Int64, objDetailsVO.ConcessionReasonId);
                        this.dbServer.AddInParameter(command2, "ConcessionRemark", DbType.String, objDetailsVO.ConcessionRemark);
                        this.dbServer.AddInParameter(command2, "GrossDiscountReason", DbType.Int64, objDetailsVO.GrossDiscountReasonID);
                        this.dbServer.AddInParameter(command2, "GrossDiscountPercentage", DbType.Double, objDetailsVO.GrossDiscountPercentage);
                        this.dbServer.AddInParameter(command2, "ConcessionAuthorizedBy", DbType.Int64, objDetailsVO.ConcessionAuthorizedBy);
                        this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, objDetailsVO.Status);
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, objDetailsVO.UnitID);
                        this.dbServer.AddInParameter(command2, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "UpdatedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command2, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command2, "UpdatedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                        this.dbServer.AddInParameter(command2, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command2, "ID", DbType.Int64, objDetailsVO.ID);
                        this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                        StringBuilder builder2 = new StringBuilder();
                        StringBuilder builder3 = new StringBuilder();
                        StringBuilder builder4 = new StringBuilder();
                        StringBuilder builder5 = new StringBuilder();
                        int num3 = 0;
                        while (true)
                        {
                            if (num3 >= objDetailsVO.ChargeDetails.Count)
                            {
                                this.dbServer.AddInParameter(command2, "ChargeIdList", DbType.String, builder2.ToString());
                                this.dbServer.AddInParameter(command2, "StatusList", DbType.String, builder5.ToString());
                                this.dbServer.AddInParameter(command2, "SponsorTypeList", DbType.String, builder3.ToString());
                                this.dbServer.AddInParameter(command2, "BalanceAmountList", DbType.String, builder4.ToString());
                                this.dbServer.AddInParameter(command2, "IsFromApprovedRequest", DbType.Boolean, true);
                                this.dbServer.ExecuteNonQuery(command2, transaction);
                                tvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                                DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddApprovedRequest");
                                command2.Connection = pConnection;
                                this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.ID);
                                this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command3, "ApprovalRequestID", DbType.Int64, tvo.ApprovalRequestID);
                                this.dbServer.AddInParameter(command3, "ApprovalRequestUnitID", DbType.Int64, tvo.ApprovalRequestUnitID);
                                this.dbServer.AddInParameter(command3, "ApprovalRemark", DbType.String, tvo.ApprovalRemark);
                                this.dbServer.AddInParameter(command3, "LevelID", DbType.Int64, tvo.LevelID);
                                this.dbServer.AddInParameter(command3, "IsApproved", DbType.Boolean, tvo.IsApproved);
                                this.dbServer.AddInParameter(command3, "ApprovedBy", DbType.Int64, tvo.ApprovedByID);
                                this.dbServer.AddInParameter(command3, "ApprovedByName", DbType.String, tvo.ApprovedByName);
                                this.dbServer.AddInParameter(command3, "ApprovedDateTime", DbType.DateTime, tvo.ApprovedDateTime);
                                this.dbServer.AddInParameter(command3, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                                this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(command3, transaction);
                                tvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
                                tvo.ApprovedRequestID = (long) this.dbServer.GetParameterValue(command3, "ID");
                                tvo.ApprovedRequestUnitID = UserVo.UserLoginInfo.UnitId;
                                foreach (clsChargeVO evo3 in tvo.ApprovalChargeList)
                                {
                                    DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_AddApprovalRequestDetailsHistroy");
                                    this.dbServer.AddInParameter(command4, "ID", DbType.Int64, evo3.ApprovalHistoryID);
                                    this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command4, "ApprovedRequestID", DbType.Int64, tvo.ApprovedRequestID);
                                    this.dbServer.AddInParameter(command4, "ApprovedRequestUnitID", DbType.Int64, tvo.ApprovedRequestUnitID);
                                    this.dbServer.AddInParameter(command4, "ApprovalRequestID", DbType.Int64, evo3.ApprovalRequestID);
                                    this.dbServer.AddInParameter(command4, "ApprovalRequestUnitID", DbType.Int64, evo3.ApprovalRequestUnitID);
                                    this.dbServer.AddInParameter(command4, "ChargeID", DbType.Int64, evo3.ID);
                                    this.dbServer.AddInParameter(command4, "ChargeUnitID", DbType.Int64, evo3.UnitID);
                                    this.dbServer.AddInParameter(command4, "ServiceID", DbType.Int64, evo3.ServiceId);
                                    this.dbServer.AddInParameter(command4, "ConcessionPercent", DbType.Single, evo3.ConcessionPercent);
                                    this.dbServer.AddInParameter(command4, "ConcessionAmount", DbType.Single, evo3.ConcessionAmount);
                                    this.dbServer.AddInParameter(command4, "ApprovalStatus", DbType.Boolean, evo3.SelectCharge);
                                    this.dbServer.AddInParameter(command4, "ApprovalRemark", DbType.String, tvo.ApprovalRemark);
                                    this.dbServer.AddInParameter(command4, "ApprovedBy", DbType.Int64, tvo.ApprovedByID);
                                    this.dbServer.AddInParameter(command4, "ApprovedDateTime", DbType.DateTime, tvo.ApprovedDateTime);
                                    this.dbServer.AddInParameter(command4, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                                    this.dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    this.dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                    this.dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, 0x7fffffff);
                                    this.dbServer.ExecuteNonQuery(command4, transaction);
                                    tvo.SuccessStatus = Convert.ToInt32(this.dbServer.GetParameterValue(command4, "ResultStatus"));
                                }
                                transaction.Commit();
                                break;
                            }
                            builder2.Append(objDetailsVO.ChargeDetails[num3].ID);
                            builder5.Append(objDetailsVO.ChargeDetails[num3].Status);
                            builder4.Append((double) (objDetailsVO.ChargeDetails[num3].NetAmount - objDetailsVO.ChargeDetails[num3].PaidAmount));
                            builder3.Append(objDetailsVO.ChargeDetails[num3].SponsorType);
                            if (num3 < (objDetailsVO.ChargeDetails.Count - 1))
                            {
                                builder2.Append(",");
                                builder5.Append(",");
                                builder4.Append(",");
                                builder3.Append(",");
                            }
                            num3++;
                        }
                        break;
                    }
                    if (!objDetailsVO.ChargeDetails[i].ChildPackageService)
                    {
                        clsBaseChargeDAL instance = clsBaseChargeDAL.GetInstance();
                        clsAddChargeBizActionVO nvo = new clsAddChargeBizActionVO {
                            Details = objDetailsVO.ChargeDetails[i]
                        };
                        nvo.Details.IsBilled = tvo.BillDetails.Details.IsFreezed;
                        nvo.Details.Date = new DateTime?(objDetailsVO.Date);
                        nvo.Details.PaidAmount = !nvo.Details.Status ? 0.0 : nvo.Details.NetAmount;
                        nvo.Details.Opd_Ipd_External_Id = objDetailsVO.Opd_Ipd_External_Id;
                        nvo.Details.Opd_Ipd_External_UnitId = objDetailsVO.Opd_Ipd_External_UnitId;
                        nvo.Details.Opd_Ipd_External = objDetailsVO.Opd_Ipd_External;
                        nvo.Details.ClassId = objDetailsVO.ChargeDetails[i].ClassId;
                        nvo.Details.IsIPDBill = objDetailsVO.IsIPDBill;
                        nvo = (clsAddChargeBizActionVO) instance.Add(nvo, UserVo, pConnection, transaction, 0L, 0L);
                        if (nvo.SuccessStatus == -1)
                        {
                            throw new Exception();
                        }
                        nvo.Details.ID = nvo.Details.ID;
                        objDetailsVO.ChargeDetails[i].ID = nvo.Details.ID;
                        iParentID = objDetailsVO.ChargeDetails[i].ID;
                        iCDParentID = objDetailsVO.ChargeDetails[i].ChargeDetails.ID;
                        if (predicate == null)
                        {
                            predicate = charge => charge.PackageID.Equals(objDetailsVO.ChargeDetails[i].PackageID) && charge.ChildPackageService;
                        }
                        foreach (clsChargeVO evo in objDetailsVO.ChargeDetails.Where<clsChargeVO>(predicate))
                        {
                            clsBaseChargeDAL edal2 = clsBaseChargeDAL.GetInstance();
                            clsAddChargeBizActionVO nvo2 = new clsAddChargeBizActionVO {
                                Details = evo
                            };
                            nvo2.Details.PaidAmount = !nvo2.Details.Status ? 0.0 : evo.NetAmount;
                            nvo2.Details.IsFromApprovedRequest = true;
                            nvo2.Details.IsBilled = tvo.BillDetails.Details.IsFreezed;
                            nvo2.Details.Date = new DateTime?(objDetailsVO.Date);
                            nvo2.Details.Opd_Ipd_External_Id = objDetailsVO.Opd_Ipd_External_Id;
                            nvo2.Details.Opd_Ipd_External_UnitId = objDetailsVO.Opd_Ipd_External_UnitId;
                            nvo2.Details.Opd_Ipd_External = objDetailsVO.Opd_Ipd_External;
                            nvo.Details.ClassId = objDetailsVO.ChargeDetails[i].ClassId;
                            nvo2 = (clsAddChargeBizActionVO) edal2.Add(nvo2, UserVo, pConnection, transaction, iParentID, iCDParentID);
                            if (nvo2.SuccessStatus == -1)
                            {
                                throw new Exception();
                            }
                            evo.ID = nvo2.Details.ID;
                            evo.ChargeDetails.ID = nvo2.Details.ChargeDetails.ID;
                        }
                    }
                    i++;
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
                tvo.SuccessStatus = -1;
            }
            finally
            {
                pConnection.Close();
                transaction = null;
                pConnection = null;
            }
            return tvo;
        }

        public override IValueObject ApproveRefundRequest(IValueObject valueObject, clsUserVO UserVo)
        {
            clsApproveSendRequestVO bizActionObj = valueObject as clsApproveSendRequestVO;
            bizActionObj = this.ApproveRefundRequestDetails(bizActionObj, UserVo, null, null);
            return valueObject;
        }

        public clsApproveSendRequestVO ApproveRefundRequestDetails(clsApproveSendRequestVO BizActionObj, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
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
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddApprovedRequest");
                storedProcCommand.Connection = connection;
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ApprovalRequestID", DbType.Int64, BizActionObj.ApprovalRequestID);
                this.dbServer.AddInParameter(storedProcCommand, "ApprovalRequestUnitID", DbType.Int64, BizActionObj.ApprovalRequestUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ApprovalRemark", DbType.String, BizActionObj.ApprovalRemark);
                this.dbServer.AddInParameter(storedProcCommand, "LevelID", DbType.Int64, BizActionObj.LevelID);
                this.dbServer.AddInParameter(storedProcCommand, "Amount", DbType.Int64, BizActionObj.RefundAmount);
                this.dbServer.AddInParameter(storedProcCommand, "IsApproved", DbType.Boolean, BizActionObj.IsApproved);
                this.dbServer.AddInParameter(storedProcCommand, "ApprovedBy", DbType.Int64, BizActionObj.ApprovedByID);
                this.dbServer.AddInParameter(storedProcCommand, "ApprovedByName", DbType.String, BizActionObj.ApprovedByName);
                this.dbServer.AddInParameter(storedProcCommand, "ApprovedDateTime", DbType.DateTime, BizActionObj.ApprovedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.ApprovedRequestID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                BizActionObj.ApprovedRequestUnitID = UserVo.UserLoginInfo.UnitId;
                foreach (clsChargeVO evo in BizActionObj.ApprovalChargeList)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddApprovalRequestDetailsHistroy");
                    this.dbServer.AddInParameter(command2, "ID", DbType.Int64, evo.ApprovalHistoryID);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "ApprovedRequestID", DbType.Int64, BizActionObj.ApprovedRequestID);
                    this.dbServer.AddInParameter(command2, "ApprovedRequestUnitID", DbType.Int64, BizActionObj.ApprovedRequestUnitID);
                    this.dbServer.AddInParameter(command2, "ApprovalRequestID", DbType.Int64, evo.ApprovalRequestID);
                    this.dbServer.AddInParameter(command2, "ApprovalRequestUnitID", DbType.Int64, evo.ApprovalRequestUnitID);
                    this.dbServer.AddInParameter(command2, "ChargeID", DbType.Int64, evo.ID);
                    this.dbServer.AddInParameter(command2, "ChargeUnitID", DbType.Int64, evo.UnitID);
                    this.dbServer.AddInParameter(command2, "ServiceID", DbType.Int64, evo.ServiceId);
                    this.dbServer.AddInParameter(command2, "ApprovalStatus", DbType.Boolean, evo.SelectCharge);
                    this.dbServer.AddInParameter(command2, "ApprovalRemarkID", DbType.Int64, evo.SelectedApprovalRefundReason.ID);
                    this.dbServer.AddInParameter(command2, "ApprovalRemark", DbType.String, evo.SelectedApprovalRefundReason.Description);
                    this.dbServer.AddInParameter(command2, "ApprovedBy", DbType.Int64, BizActionObj.ApprovedByID);
                    this.dbServer.AddInParameter(command2, "ApprovedDateTime", DbType.DateTime, BizActionObj.ApprovedDateTime);
                    this.dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    BizActionObj.SuccessStatus = Convert.ToInt32(this.dbServer.GetParameterValue(command2, "ResultStatus"));
                }
                if (this.IsAuditTrail && (BizActionObj.LogInfoList != null))
                {
                    LogInfo item = new LogInfo();
                    Guid guid = default(Guid);
                    item.guid = guid;
                    item.UserId = UserVo.ID;
                    item.TimeStamp = DateTime.Now;
                    item.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                    item.MethodName = MethodBase.GetCurrentMethod().ToString();
                    string[] strArray = new string[] { " 26 : Approved Services \r\nUser Unit Id : ", Convert.ToString(UserVo.UserLoginInfo.UnitId), " Approval Request ID : ", Convert.ToString(BizActionObj.ApprovalRequestID), " Approval Request UnitID : ", Convert.ToString(BizActionObj.ApprovalRequestUnitID), " Approval Remark : ", Convert.ToString(BizActionObj.ApprovalRemark), " ApprovedBy ID : " };
                    strArray[9] = Convert.ToString(BizActionObj.ApprovedByID);
                    strArray[10] = " ApprovedBy Name : ";
                    strArray[11] = Convert.ToString(BizActionObj.ApprovedByName);
                    strArray[12] = " Approved Date Time : ";
                    strArray[13] = Convert.ToString(BizActionObj.ApprovedDateTime);
                    strArray[14] = " Level ID : ";
                    strArray[15] = Convert.ToString(BizActionObj.LevelID);
                    strArray[0x10] = " Approved Request ID : ";
                    strArray[0x11] = Convert.ToString(BizActionObj.ApprovedRequestID);
                    strArray[0x12] = " Approved Request UnitID : ";
                    strArray[0x13] = Convert.ToString(BizActionObj.ApprovedRequestUnitID);
                    strArray[20] = "\r\n";
                    item.Message = string.Concat(strArray);
                    item.Message = item.Message.Replace("\r\n", Environment.NewLine);
                    BizActionObj.LogInfoList.Add(item);
                }
                if (pConnection == null)
                {
                    transaction.Commit();
                }
                BizActionObj.SuccessStatus = 0;
                if ((this.IsAuditTrail && ((BizActionObj.LogInfoList != null) && (BizActionObj.LogInfoList.Count > 0))) && this.IsAuditTrail)
                {
                    this.SetLogInfo(BizActionObj.LogInfoList, UserVo.ID);
                    BizActionObj.LogInfoList.Clear();
                }
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
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
                    connection.Close();
                    connection = null;
                    transaction = null;
                }
            }
            return BizActionObj;
        }

        public override IValueObject Delete(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteRefundBizActionVO nvo = valueObject as clsDeleteRefundBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteRefund");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AdvanceID", DbType.Int64, nvo.AdvanceID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject DeleteApprovalRequest(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteApprovalRequestVO tvo = valueObject as clsDeleteApprovalRequestVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteApprovalRequest");
                this.dbServer.AddInParameter(storedProcCommand, "ApprovalRequestID", DbType.Int64, tvo.ApprovalRequestID);
                this.dbServer.AddInParameter(storedProcCommand, "ApprovalRequestUnitID", DbType.Int64, tvo.ApprovalRequestUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                tvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return tvo;
        }

        public override IValueObject GetList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRefundListBizActionVO nvo = valueObject as clsGetRefundListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetRefund");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AllCompany", DbType.Boolean, nvo.AllCompanies);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyID", DbType.Int64, nvo.CompanyID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CostingDivisionID", DbType.Int64, nvo.CostingDivisionID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Details == null)
                    {
                        nvo.Details = new List<clsRefundVO>();
                    }
                    while (reader.Read())
                    {
                        clsRefundVO item = new clsRefundVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            PatientID = (long) DALHelper.HandleDBNull(reader["PatientId"]),
                            PatientUnitID = (long) DALHelper.HandleDBNull(reader["PatientUnitID"]),
                            CompanyID = (long) DALHelper.HandleDBNull(reader["CompanyID"]),
                            AdvanceID = (long) DALHelper.HandleDBNull(reader["AdvanceID"]),
                            Date = DALHelper.HandleDate(reader["Date"]),
                            Amount = (double) DALHelper.HandleDBNull(reader["Amount"]),
                            BillID = (long) DALHelper.HandleDBNull(reader["BillID"]),
                            Remarks = (string) DALHelper.HandleDBNull(reader["Remarks"]),
                            ReceiptNo = (string) DALHelper.HandleDBNull(reader["ReceiptNo"]),
                            ReceiptPrinted = (bool) DALHelper.HandleDBNull(reader["ReceiptPrinted"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitId"]),
                            CostingDivisionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CostingDivisionID"])),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            AddedBy = (long) DALHelper.HandleDBNull(reader["AddedBy"]),
                            AddedOn = (string) DALHelper.HandleDBNull(reader["AddedOn"]),
                            AddedDateTime = DALHelper.HandleDate(reader["AddedDateTime"]),
                            AddedWindowsLoginName = (string) DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]),
                            UpdatedBy = (long?) DALHelper.HandleDBNull(reader["UpdatedBy"]),
                            UpdatedOn = (string) DALHelper.HandleDBNull(reader["UpdatedOn"]),
                            UpdatedDateTime = DALHelper.HandleDate(reader["UpdatedDateTime"]),
                            UpdateWindowsLoginName = (string) DALHelper.HandleDBNull(reader["UpdateWindowsLoginName"]),
                            UnitName = (string) DALHelper.HandleDBNull(reader["UnitName"]),
                            AdvanceNo = Convert.ToString(DALHelper.HandleDBNull(reader["AdvanceNO"]))
                        };
                        nvo.Details.Add(item);
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

        private clsPathOrderBookingVO GetPathologyTestDetails(long pServiceID, clsUserVO UserVo)
        {
            clsPathOrderBookingVO gvo = null;
            try
            {
                clsBaseOrderBookingDAL instance = clsBaseOrderBookingDAL.GetInstance();
                clsGetPathTestDetailsBizActionVO valueObject = new clsGetPathTestDetailsBizActionVO {
                    ServiceID = pServiceID
                };
                valueObject = (clsGetPathTestDetailsBizActionVO) instance.GetTestList(valueObject, UserVo);
                gvo = new clsPathOrderBookingVO();
                if (((valueObject != null) && (valueObject.TestList != null)) && (valueObject.TestList.Count > 0))
                {
                    foreach (clsPathOrderBookingVO gvo2 in valueObject.TestList)
                    {
                        clsPathOrderBookingDetailVO item = new clsPathOrderBookingDetailVO {
                            TestID = gvo2.TestID,
                            IsSampleCollected = false
                        };
                        gvo.Items.Add(item);
                    }
                }
            }
            catch (Exception)
            {
            }
            return gvo;
        }

        private clsPathOrderBookingVO GetPathologyTestDetails(long pServiceID, clsUserVO UserVo, long pOBID, long pOBUnitID, long pChargeID, DbConnection pConnection, DbTransaction pTransaction)
        {
            clsPathOrderBookingVO gvo = null;
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
                clsBaseOrderBookingDAL instance = clsBaseOrderBookingDAL.GetInstance();
                clsGetPathTestDetailsBizActionVO valueObject = new clsGetPathTestDetailsBizActionVO {
                    ServiceID = pServiceID,
                    pobID = pOBID,
                    pobUnitID = pOBUnitID,
                    pChargeID = pChargeID
                };
                valueObject = (clsGetPathTestDetailsBizActionVO) instance.GetTestListWithDetailsID(valueObject, UserVo, pConnection, pTransaction);
                gvo = new clsPathOrderBookingVO();
                if (((valueObject != null) && (valueObject.TestList != null)) && (valueObject.TestList.Count > 0))
                {
                    foreach (clsPathOrderBookingVO gvo2 in valueObject.TestList)
                    {
                        clsPathOrderBookingDetailVO item = new clsPathOrderBookingDetailVO {
                            TestID = gvo2.TestID,
                            IsSampleCollected = false,
                            ID = gvo2.POBDID
                        };
                        gvo.Items.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
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
            return gvo;
        }

        private clsRadOrderBookingDetailsVO GetRadilogyTestDetails(long pServiceID, clsUserVO UserVo)
        {
            clsRadOrderBookingDetailsVO svo = null;
            try
            {
                clsBaseRadiologyDAL instance = clsBaseRadiologyDAL.GetInstance();
                clsGetRadTestDetailsBizActionVO valueObject = new clsGetRadTestDetailsBizActionVO {
                    ServiceID = pServiceID
                };
                valueObject = (clsGetRadTestDetailsBizActionVO) instance.GetTestList(valueObject, UserVo);
                svo = new clsRadOrderBookingDetailsVO();
                if (((valueObject != null) && (valueObject.TestList != null)) && (valueObject.TestList.Count > 0))
                {
                    svo.TestID = valueObject.TestList[0].TestID;
                }
            }
            catch (Exception)
            {
            }
            return svo;
        }

        public override IValueObject GetRefundReceiptList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRefundReceiptListBizActionVO nvo = valueObject as clsGetRefundReceiptListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetRefundReceipt");
                this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.Int64, nvo.BillID);
                this.dbServer.AddInParameter(storedProcCommand, "BillUnitID", DbType.Int64, nvo.BillUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Details == null)
                    {
                        nvo.Details = new List<clsRefundVO>();
                    }
                    while (reader.Read())
                    {
                        clsRefundVO item = new clsRefundVO {
                            ID = (long) DALHelper.HandleDBNull(reader["RefundID"]),
                            Date = DALHelper.HandleDate(reader["RefundDate"]),
                            Amount = (double) DALHelper.HandleDBNull(reader["RefundAmount"]),
                            BillID = (long) DALHelper.HandleDBNull(reader["BillID"]),
                            Remarks = (string) DALHelper.HandleDBNull(reader["Remarks"]),
                            ReceiptNo = (string) DALHelper.HandleDBNull(reader["ReceiptNo"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            CostingDivisionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CostingDivisionID"])),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            AddedBy = (long) DALHelper.HandleDBNull(reader["AddedBy"]),
                            AddedOn = (string) DALHelper.HandleDBNull(reader["AddedOn"]),
                            AddedDateTime = DALHelper.HandleDate(reader["AddedDateTime"]),
                            AddedWindowsLoginName = (string) DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]),
                            UpdatedBy = (long?) DALHelper.HandleDBNull(reader["UpdatedBy"]),
                            UpdatedOn = (string) DALHelper.HandleDBNull(reader["UpdatedOn"]),
                            UpdatedDateTime = DALHelper.HandleDate(reader["UpdatedDateTime"]),
                            UpdateWindowsLoginName = (string) DALHelper.HandleDBNull(reader["UpdateWindowsLoginName"]),
                            UnitName = (string) DALHelper.HandleDBNull(reader["UnitName"])
                        };
                        nvo.Details.Add(item);
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

        public override IValueObject SendApprovalRequest(IValueObject valueObject, clsUserVO UserVo)
        {
            clsSendRequestForApprovalVO bizActionObj = valueObject as clsSendRequestForApprovalVO;
            bizActionObj = this.SendApprovalRequestDetails(bizActionObj, UserVo, null, null);
            return valueObject;
        }

        public clsSendRequestForApprovalVO SendApprovalRequestDetails(clsSendRequestForApprovalVO BizActionObj, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
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
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddRefundApprovalRequest");
                storedProcCommand.Connection = connection;
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.Details.ApprovalRequestID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDBill", DbType.Boolean, BizActionObj.Details.IsOPDBill);
                this.dbServer.AddInParameter(storedProcCommand, "RequestTypeID", DbType.Int64, BizActionObj.Details.RequestTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "RequestType", DbType.String, BizActionObj.Details.RequestType);
                this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.Int64, BizActionObj.Details.BillID);
                this.dbServer.AddInParameter(storedProcCommand, "BillUnitID", DbType.Int64, BizActionObj.Details.BillUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ApprovalDate", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "Amount", DbType.Double, BizActionObj.RefundAmount);
                this.dbServer.AddInParameter(storedProcCommand, "IsAgainstBill", DbType.Boolean, BizActionObj.IsAgainstBill);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.Details.ApprovalRequestID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                BizActionObj.Details.ApprovalRequestUnitID = UserVo.UserLoginInfo.UnitId;
                LogInfo item = new LogInfo();
                if (this.IsAuditTrail && (BizActionObj.LogInfoList != null))
                {
                    Guid guid = default(Guid);
                    item.guid = guid;
                    item.UserId = UserVo.ID;
                    item.TimeStamp = DateTime.Now;
                    item.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                    item.MethodName = MethodBase.GetCurrentMethod().ToString();
                    string[] strArray = new string[] { " 22 : Cancel Service data In DAL  User Unit Id : ", Convert.ToString(UserVo.UserLoginInfo.UnitId), " Bill ID : ", Convert.ToString(BizActionObj.Details.BillID), " Bill Unit ID : ", Convert.ToString(BizActionObj.Details.BillUnitID), " Is OPD Bill : ", Convert.ToString(BizActionObj.Details.IsOPDBill), " RequestType : " };
                    strArray[9] = Convert.ToString(BizActionObj.Details.RequestType);
                    strArray[10] = " ApprovalDate :";
                    strArray[11] = Convert.ToString(DateTime.Now);
                    strArray[12] = " ";
                    item.Message = string.Concat(strArray);
                }
                foreach (clsChargeVO evo in BizActionObj.ChargeList)
                {
                    if (this.IsAuditTrail && (BizActionObj.LogInfoList != null))
                    {
                        string[] strArray2 = new string[] { item.Message, "Approval RequestID : ", Convert.ToString(BizActionObj.Details.ApprovalRequestID), " Approval Request UnitID : ", Convert.ToString(BizActionObj.Details.ApprovalRequestUnitID), " Approval Status : ", Convert.ToString("false"), " Cancel Service : ", Convert.ToString(evo.SelectCharge) };
                        strArray2[9] = " Approval Remark : ";
                        strArray2[10] = Convert.ToString(evo.ApprovalRemark);
                        strArray2[11] = " Request Remark : ";
                        strArray2[12] = Convert.ToString(evo.ApprovalRequestRemark);
                        strArray2[13] = " Service Code : ";
                        strArray2[14] = Convert.ToString(evo.ServiceId);
                        strArray2[15] = " Service Name : ";
                        strArray2[0x10] = Convert.ToString(evo.ServiceName);
                        strArray2[0x11] = " \r\n";
                        item.Message = string.Concat(strArray2);
                    }
                    if (this.IsAuditTrail && (BizActionObj.LogInfoList != null))
                    {
                        item.Message = item.Message.Replace("\r\n", Environment.NewLine);
                        BizActionObj.LogInfoList.Add(item);
                    }
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddRefundChargeApprovalDetails");
                    this.dbServer.AddInParameter(command2, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command2, "IsRefundRequest", DbType.Boolean, BizActionObj.IsRefundRequest);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "ChargeID", DbType.Int64, evo.ID);
                    this.dbServer.AddInParameter(command2, "ChargeUnitID", DbType.Int64, evo.UnitID);
                    this.dbServer.AddInParameter(command2, "ApprovalRequestID", DbType.Int64, BizActionObj.Details.ApprovalRequestID);
                    this.dbServer.AddInParameter(command2, "ApprovalRequestUnitID", DbType.Int64, BizActionObj.Details.ApprovalRequestUnitID);
                    this.dbServer.AddInParameter(command2, "ApprovalStatus", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command2, "ServiceID", DbType.Int64, evo.ServiceId);
                    this.dbServer.AddInParameter(command2, "ApprovalRequestRemarkID", DbType.Int64, evo.SelectedRequestRefundReason.ID);
                    this.dbServer.AddInParameter(command2, "ApprovalRequestRemark", DbType.String, evo.SelectedRequestRefundReason.Description);
                    this.dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    BizActionObj.SuccessStatus = Convert.ToInt32(this.dbServer.GetParameterValue(command2, "ResultStatus"));
                    if (!BizActionObj.IsRefundRequest)
                    {
                        break;
                    }
                }
                if (pConnection == null)
                {
                    transaction.Commit();
                }
                BizActionObj.SuccessStatus = 0;
                if ((this.IsAuditTrail && ((BizActionObj.LogInfoList != null) && (BizActionObj.LogInfoList.Count > 0))) && this.IsAuditTrail)
                {
                    this.SetLogInfo(BizActionObj.LogInfoList, UserVo.ID);
                    BizActionObj.LogInfoList.Clear();
                }
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
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
                    connection.Close();
                    connection = null;
                    transaction = null;
                }
            }
            return BizActionObj;
        }

        public override IValueObject SendApprovalRequestForAdvanceRefundDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsSendRequestForApprovalVO lvo = valueObject as clsSendRequestForApprovalVO;
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
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddRefundApprovalRequest");
                storedProcCommand.Connection = connection;
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, lvo.Details.ApprovalRequestID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDBill", DbType.Boolean, lvo.Details.IsOPDBill);
                this.dbServer.AddInParameter(storedProcCommand, "RequestTypeID", DbType.Int64, lvo.Details.RequestTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "RequestType", DbType.String, lvo.Details.RequestType);
                this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.Int64, lvo.Details.BillID);
                this.dbServer.AddInParameter(storedProcCommand, "BillUnitID", DbType.Int64, lvo.Details.BillUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ApprovalDate", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "Amount", DbType.Double, lvo.RefundAmount);
                this.dbServer.AddInParameter(storedProcCommand, "IsAgainstBill", DbType.Boolean, lvo.IsAgainstBill);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "AdvanceID", DbType.Int64, lvo.Details.AdvanceID);
                this.dbServer.AddInParameter(storedProcCommand, "AdvanceUnitID", DbType.Int64, lvo.Details.AdvanceUnitID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                lvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                lvo.Details.ApprovalRequestID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                lvo.Details.ApprovalRequestUnitID = UserVo.UserLoginInfo.UnitId;
                DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddRefundApprovalRequestDetails");
                this.dbServer.AddInParameter(command2, "ID", DbType.Int64, 0);
                this.dbServer.AddInParameter(command2, "IsRefundRequest", DbType.Boolean, true);
                this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command2, "ChargeID", DbType.Int64, 0);
                this.dbServer.AddInParameter(command2, "ChargeUnitID", DbType.Int64, 0);
                this.dbServer.AddInParameter(command2, "ApprovalRequestID", DbType.Int64, lvo.Details.ApprovalRequestID);
                this.dbServer.AddInParameter(command2, "ApprovalRequestUnitID", DbType.Int64, lvo.Details.ApprovalRequestUnitID);
                this.dbServer.AddInParameter(command2, "ApprovalStatus", DbType.Boolean, false);
                this.dbServer.AddInParameter(command2, "ServiceID", DbType.Int64, 0);
                this.dbServer.AddInParameter(command2, "ApprovalRequestRemark", DbType.String, lvo.Details.Remarks);
                this.dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(command2, transaction);
                lvo.SuccessStatus = Convert.ToInt32(this.dbServer.GetParameterValue(command2, "ResultStatus"));
                transaction.Commit();
                lvo.SuccessStatus = 0;
            }
            catch (Exception)
            {
                lvo.SuccessStatus = -1;
                lvo.SuccessStatus = -1;
                if (connection == null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection = null;
                    transaction = null;
                }
            }
            return lvo;
        }

        public override IValueObject SendApprovalRequestForBill(IValueObject valueObject, clsUserVO UserVo)
        {
            clsSendRequestForApprovalVO lvo = valueObject as clsSendRequestForApprovalVO;
            DbTransaction transaction = null;
            DbConnection pConnection = null;
            try
            {
                int num9;
                DbCommand command10;
                object[] objArray;
                clsBillVO objDetailsVO;
                pConnection = this.dbServer.CreateConnection();
                pConnection.Open();
                transaction = pConnection.BeginTransaction();
                long iParentID = 0L;
                long iCDParentID = 0L;
                bool flag = false;
                if (lvo.BillDetails.Details.ID != 0L)
                {
                    objDetailsVO = lvo.BillDetails.Details;
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateChargeDetailsNew");
                    this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.Int64, objDetailsVO.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objDetailsVO.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    Func<clsChargeVO, bool> predicate = null;
                    int i = 0;
                    while (true)
                    {
                        if (i < objDetailsVO.ChargeDetails.Count)
                        {
                            if (!objDetailsVO.ChargeDetails[i].ChildPackageService)
                            {
                                clsBaseChargeDAL instance = clsBaseChargeDAL.GetInstance();
                                clsAddChargeBizActionVO nvo7 = new clsAddChargeBizActionVO {
                                    Details = objDetailsVO.ChargeDetails[i]
                                };
                                nvo7.Details.IsBilled = lvo.BillDetails.Details.IsFreezed;
                                nvo7.Details.Date = new DateTime?(objDetailsVO.Date);
                                nvo7.Details.PaidAmount = !nvo7.Details.Status ? 0.0 : nvo7.Details.NetAmount;
                                nvo7.Details.Opd_Ipd_External_Id = objDetailsVO.Opd_Ipd_External_Id;
                                nvo7.Details.Opd_Ipd_External_UnitId = objDetailsVO.Opd_Ipd_External_UnitId;
                                nvo7.Details.Opd_Ipd_External = objDetailsVO.Opd_Ipd_External;
                                nvo7.Details.ClassId = objDetailsVO.ChargeDetails[i].ClassId;
                                nvo7.Details.IsIPDBill = objDetailsVO.IsIPDBill;
                                nvo7 = (clsAddChargeBizActionVO) instance.Add(nvo7, UserVo, pConnection, transaction, 0L, 0L);
                                if (nvo7.SuccessStatus == -1)
                                {
                                    throw new Exception();
                                }
                                nvo7.Details.ID = nvo7.Details.ID;
                                objDetailsVO.ChargeDetails[i].ID = nvo7.Details.ID;
                                iParentID = objDetailsVO.ChargeDetails[i].ID;
                                iCDParentID = objDetailsVO.ChargeDetails[i].ChargeDetails.ID;
                                if (predicate == null)
                                {
                                    predicate = charge => charge.PackageID.Equals(objDetailsVO.ChargeDetails[i].PackageID) && charge.ChildPackageService;
                                }
                                foreach (clsChargeVO evo3 in objDetailsVO.ChargeDetails.Where<clsChargeVO>(predicate))
                                {
                                    clsBaseChargeDAL edal4 = clsBaseChargeDAL.GetInstance();
                                    clsAddChargeBizActionVO nvo8 = new clsAddChargeBizActionVO {
                                        Details = evo3
                                    };
                                    nvo8.Details.PaidAmount = !nvo8.Details.Status ? 0.0 : evo3.NetAmount;
                                    nvo8.Details.IsBilled = lvo.BillDetails.Details.IsFreezed;
                                    nvo8.Details.Date = new DateTime?(objDetailsVO.Date);
                                    nvo8.Details.Opd_Ipd_External_Id = objDetailsVO.Opd_Ipd_External_Id;
                                    nvo8.Details.Opd_Ipd_External_UnitId = objDetailsVO.Opd_Ipd_External_UnitId;
                                    nvo8.Details.Opd_Ipd_External = objDetailsVO.Opd_Ipd_External;
                                    nvo7.Details.ClassId = objDetailsVO.ChargeDetails[i].ClassId;
                                    nvo8 = (clsAddChargeBizActionVO) edal4.Add(nvo8, UserVo, pConnection, transaction, iParentID, iCDParentID);
                                    if (nvo8.SuccessStatus == -1)
                                    {
                                        throw new Exception();
                                    }
                                    evo3.ID = nvo8.Details.ID;
                                    evo3.ChargeDetails.ID = nvo8.Details.ChargeDetails.ID;
                                }
                            }
                            i++;
                            continue;
                        }
                        DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_UpdateChargeNew");
                        this.dbServer.AddInParameter(command, "BillID", DbType.Int64, objDetailsVO.ID);
                        this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDetailsVO.UnitID);
                        this.dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);
                        this.dbServer.ExecuteNonQuery(command, transaction);
                        DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_UpdateBill");
                        this.dbServer.AddInParameter(command4, "IsPackageBill", DbType.Int64, lvo.BillDetails.IsPackageBill);
                        if (lvo.BillDetails.IsPackageBill)
                        {
                            StringBuilder builder6 = new StringBuilder();
                            foreach (clsChargeVO evo4 in objDetailsVO.ChargeDetails)
                            {
                                if ((evo4.PackageID > 0L) && evo4.isPackageService)
                                {
                                    builder6.Append("," + Convert.ToString(evo4.PackageID));
                                }
                            }
                            builder6.Replace(",", "", 0, 1);
                            this.dbServer.AddInParameter(command4, "ipPackageList", DbType.String, Convert.ToString(builder6));
                        }
                        this.dbServer.AddInParameter(command4, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                        if (objDetailsVO.LinkServer != null)
                        {
                            this.dbServer.AddInParameter(command4, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));
                        }
                        this.dbServer.AddInParameter(command4, "Date", DbType.DateTime, objDetailsVO.Date);
                        this.dbServer.AddInParameter(command4, "TotalBillAmount", DbType.Double, objDetailsVO.TotalBillAmount);
                        this.dbServer.AddInParameter(command4, "TotalConcessionAmount", DbType.Double, objDetailsVO.TotalConcessionAmount);
                        this.dbServer.AddInParameter(command4, "NetBillAmount", DbType.Double, objDetailsVO.NetBillAmount);
                        this.dbServer.AddInParameter(command4, "SelfAmount", DbType.Double, objDetailsVO.SelfAmount);
                        if (objDetailsVO.BalanceAmountSelf < 0.0)
                        {
                            objDetailsVO.BalanceAmountSelf = 0.0;
                        }
                        this.dbServer.AddInParameter(command4, "BalanceAmountSelf", DbType.Double, objDetailsVO.BalanceAmountSelf);
                        this.dbServer.AddInParameter(command4, "NonSelfAmount", DbType.Double, objDetailsVO.NonSelfAmount);
                        this.dbServer.AddInParameter(command4, "BalanceAmountNonSelf", DbType.Double, objDetailsVO.BalanceAmountNonSelf);
                        this.dbServer.AddInParameter(command4, "IsPrinted", DbType.Boolean, objDetailsVO.IsPrinted);
                        this.dbServer.AddInParameter(command4, "SponserType", DbType.Boolean, objDetailsVO.SponserType);
                        if (objDetailsVO.Remark != null)
                        {
                            objDetailsVO.Remark = objDetailsVO.Remark.Trim();
                        }
                        this.dbServer.AddInParameter(command4, "Remark", DbType.String, objDetailsVO.Remark);
                        if (objDetailsVO.BillRemark != null)
                        {
                            objDetailsVO.BillRemark = objDetailsVO.BillRemark.Trim();
                        }
                        this.dbServer.AddInParameter(command4, "BillRemark", DbType.String, objDetailsVO.BillRemark);
                        this.dbServer.AddInParameter(command4, "BillType", DbType.Int16, (short) objDetailsVO.BillType);
                        this.dbServer.AddInParameter(command4, "BillPaymentType", DbType.Int16, (short) objDetailsVO.BillPaymentType);
                        this.dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, objDetailsVO.IsFreezed);
                        this.dbServer.AddInParameter(command4, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);
                        this.dbServer.AddInParameter(command4, "GrossDiscountReason", DbType.Int64, objDetailsVO.GrossDiscountReasonID);
                        this.dbServer.AddInParameter(command4, "GrossDiscountPercentage", DbType.Double, objDetailsVO.GrossDiscountPercentage);
                        this.dbServer.AddInParameter(command4, "ConcessionAuthorizedBy", DbType.Int64, objDetailsVO.ConcessionAuthorizedBy);
                        this.dbServer.AddInParameter(command4, "Status", DbType.Boolean, objDetailsVO.Status);
                        this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, objDetailsVO.UnitID);
                        this.dbServer.AddInParameter(command4, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command4, "ConcessionRemark", DbType.String, objDetailsVO.ConcessionRemark);
                        this.dbServer.AddInParameter(command4, "UpdatedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command4, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command4, "UpdatedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                        this.dbServer.AddInParameter(command4, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command4, "ConcessionreasonId", DbType.Int64, objDetailsVO.ConcessionReasonId);
                        this.dbServer.AddInParameter(command4, "ID", DbType.Int64, objDetailsVO.ID);
                        this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, 0x7fffffff);
                        StringBuilder builder7 = new StringBuilder();
                        StringBuilder builder8 = new StringBuilder();
                        StringBuilder builder9 = new StringBuilder();
                        StringBuilder builder10 = new StringBuilder();
                        int num7 = 0;
                        while (true)
                        {
                            if (num7 < objDetailsVO.ChargeDetails.Count)
                            {
                                builder7.Append(objDetailsVO.ChargeDetails[num7].ID);
                                builder10.Append(objDetailsVO.ChargeDetails[num7].Status);
                                builder9.Append((double) (objDetailsVO.ChargeDetails[num7].NetAmount - objDetailsVO.ChargeDetails[num7].PaidAmount));
                                builder8.Append(objDetailsVO.ChargeDetails[num7].SponsorType);
                                if (num7 < (objDetailsVO.ChargeDetails.Count - 1))
                                {
                                    builder7.Append(",");
                                    builder10.Append(",");
                                    builder9.Append(",");
                                    builder8.Append(",");
                                }
                                num7++;
                                continue;
                            }
                            this.dbServer.AddInParameter(command4, "ChargeIdList", DbType.String, builder7.ToString());
                            this.dbServer.AddInParameter(command4, "StatusList", DbType.String, builder10.ToString());
                            this.dbServer.AddInParameter(command4, "SponsorTypeList", DbType.String, builder8.ToString());
                            this.dbServer.AddInParameter(command4, "BalanceAmountList", DbType.String, builder9.ToString());
                            this.dbServer.ExecuteNonQuery(command4, transaction);
                            lvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command4, "ResultStatus");
                            if (lvo.BillDetails.Details.IsFreezed && (lvo.BillDetails.Details.PaymentDetails != null))
                            {
                                clsBasePaymentDAL instance = clsBasePaymentDAL.GetInstance();
                                clsAddPaymentBizActionVO nvo9 = new clsAddPaymentBizActionVO {
                                    Details = new clsPaymentVO()
                                };
                                nvo9.Details = lvo.BillDetails.Details.PaymentDetails;
                                nvo9.myTransaction = true;
                                nvo9.Details.BillID = lvo.BillDetails.Details.ID;
                                nvo9.Details.BillAmount = objDetailsVO.NetBillAmount;
                                nvo9.Details.Date = new DateTime?(lvo.BillDetails.Details.Date);
                                nvo9.Details.LinkServer = lvo.BillDetails.Details.LinkServer;
                                nvo9 = (clsAddPaymentBizActionVO) instance.Add(nvo9, UserVo, pConnection, transaction);
                                if (nvo9.SuccessStatus == -1)
                                {
                                    throw new Exception();
                                }
                                lvo.BillDetails.Details.PaymentDetails.ID = nvo9.Details.ID;
                            }
                            if ((lvo.BillDetails.Details.PathoWorkOrder != null) && ((lvo.BillDetails.Details.IsFreezed && !lvo.BillDetails.Details.IsIPDBill) || lvo.BillDetails.Details.IsIPDBill))
                            {
                                lvo.BillDetails.Details.PathoWorkOrder.Opd_Ipd_External_ID = lvo.BillDetails.Details.Opd_Ipd_External_Id;
                                lvo.BillDetails.Details.PathoWorkOrder.Opd_Ipd_External_UnitID = lvo.BillDetails.Details.Opd_Ipd_External_UnitId;
                                lvo.BillDetails.Details.PathoWorkOrder.SampleType = false;
                                lvo.BillDetails.Details.PathoWorkOrder.Opd_Ipd_External = new long?((long) lvo.BillDetails.Details.Opd_Ipd_External);
                                lvo.BillDetails.Details.PathoWorkOrder.UnitId = lvo.BillDetails.Details.UnitID;
                                lvo.BillDetails.Details.PathoWorkOrder.AddedDateTime = lvo.BillDetails.Details.AddedDateTime;
                                lvo.BillDetails.Details.PathoWorkOrder.OrderDate = new DateTime?(lvo.BillDetails.Details.Date);
                                lvo.BillDetails.Details.PathoWorkOrder.BillNo = lvo.BillDetails.Details.BillNo;
                                lvo.BillDetails.Details.PathoWorkOrder.BillID = lvo.BillDetails.Details.ID;
                                int num8 = 0;
                                while (true)
                                {
                                    if (num8 >= objDetailsVO.ChargeDetails.Count)
                                    {
                                        clsBaseOrderBookingDAL instance = clsBaseOrderBookingDAL.GetInstance();
                                        clsAddPathOrderBookingBizActionVO nvo10 = new clsAddPathOrderBookingBizActionVO {
                                            PathOrderBooking = lvo.BillDetails.Details.PathoWorkOrder,
                                            PathOrderBookingDetailList = lvo.BillDetails.Details.PathoWorkOrder.Items
                                        };
                                        if (nvo10.PathOrderBookingDetailList.Count > 0)
                                        {
                                            nvo10 = (clsAddPathOrderBookingBizActionVO) instance.AddPathOrderBooking(nvo10, UserVo, transaction, pConnection);
                                            if (nvo10.SuccessStatus == -1)
                                            {
                                                throw new Exception();
                                            }
                                            if (nvo10.PathOrderBooking.ID > 0L)
                                            {
                                                lvo.BillDetails.Details.PathoWorkOrder.ID = nvo10.PathOrderBooking.ID;
                                            }
                                        }
                                        objArray = new object[] { "DELETE FROM T_PathOrderBookingDetails Where OrderID = (Select TOP 1 ID FROM T_PathOrderBooking Where BillID = ", lvo.BillDetails.Details.PathoWorkOrder.BillID, ") AND UnitID =", lvo.BillDetails.Details.PathoWorkOrder.UnitId, " AND Status = 0 " };
                                        DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(objArray));
                                        this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                                        if ((lvo.BillDetails.Details.DeletedChargeDetails != null) && ((lvo.BillDetails.Details.DeletedChargeDetails.Count > 0) && lvo.BillDetails.Details.IsIPDBill))
                                        {
                                            foreach (clsChargeVO evo5 in lvo.BillDetails.Details.DeletedChargeDetails)
                                            {
                                                if ((evo5.ID != 0L) && ((evo5.TariffServiceId != 0L) && (lvo.BillDetails.Details.PathoSpecilizationID == evo5.ServiceSpecilizationID)))
                                                {
                                                    objArray = new object[] { "Delete from T_PathOrderBookingDetails where ChargeID = ", evo5.ID, " And TariffServiceID = ", evo5.TariffServiceId, " And UnitID = ", evo5.UnitID };
                                                    DbCommand command6 = this.dbServer.GetSqlStringCommand(string.Concat(objArray));
                                                    this.dbServer.ExecuteNonQuery(command6, transaction);
                                                }
                                            }
                                        }
                                        break;
                                    }
                                    if (lvo.BillDetails.Details.PathoWorkOrder.PathoSpecilizationID == objDetailsVO.ChargeDetails[num8].ServiceSpecilizationID)
                                    {
                                        lvo.BillDetails.Details.PathoWorkOrder.DoctorID = lvo.BillDetails.Details.PathoWorkOrder.DoctorID;
                                        if (objDetailsVO.ChargeDetails[num8].POBID > 0L)
                                        {
                                            lvo.BillDetails.Details.PathoWorkOrder.ID = objDetailsVO.ChargeDetails[num8].POBID;
                                        }
                                        lvo.BillDetails.Details.PathoWorkOrder.ID = lvo.BillDetails.Details.PathoWorkOrder.ID;
                                        clsPathOrderBookingVO gvo2 = new clsPathOrderBookingVO();
                                        gvo2 = (lvo.BillDetails.Details.PathoWorkOrder.ID <= 0L) ? this.GetPathologyTestDetails(objDetailsVO.ChargeDetails[num8].ServiceId, UserVo) : this.GetPathologyTestDetails(objDetailsVO.ChargeDetails[num8].ServiceId, UserVo, lvo.BillDetails.Details.PathoWorkOrder.ID, objDetailsVO.ChargeDetails[num8].UnitID, objDetailsVO.ChargeDetails[num8].ID, pConnection, transaction);
                                        if ((gvo2.Items != null) && (gvo2.Items.Count > 0))
                                        {
                                            foreach (clsPathOrderBookingDetailVO lvo3 in gvo2.Items)
                                            {
                                                lvo3.ID = lvo3.ID;
                                                objDetailsVO.ChargeDetails[num8].POBDID = lvo3.ID;
                                                lvo3.ServiceID = objDetailsVO.ChargeDetails[num8].ServiceId;
                                                lvo3.ChargeID = objDetailsVO.ChargeDetails[num8].ID;
                                                lvo3.TariffServiceID = objDetailsVO.ChargeDetails[num8].TariffServiceId;
                                                lvo3.TestCharges = objDetailsVO.ChargeDetails[num8].Rate;
                                                lvo.BillDetails.Details.PathoWorkOrder.Items.Add(lvo3);
                                            }
                                        }
                                    }
                                    num8++;
                                }
                            }
                            if (((lvo.BillDetails.Details.RadiologyWorkOrder == null) || (lvo.BillDetails.Details.RadiologyWorkOrder.OrderBookingDetails.Count <= 0)) || ((!lvo.BillDetails.Details.IsFreezed || lvo.BillDetails.Details.IsIPDBill) && !lvo.BillDetails.Details.IsIPDBill))
                            {
                                goto TR_007C;
                            }
                            else
                            {
                                lvo.BillDetails.Details.RadiologyWorkOrder.Opd_Ipd_External_ID = lvo.BillDetails.Details.Opd_Ipd_External_Id;
                                lvo.BillDetails.Details.RadiologyWorkOrder.Opd_Ipd_External_UnitID = lvo.BillDetails.Details.Opd_Ipd_External_UnitId;
                                lvo.BillDetails.Details.RadiologyWorkOrder.Opd_Ipd_External = new long?((long) lvo.BillDetails.Details.Opd_Ipd_External);
                                lvo.BillDetails.Details.RadiologyWorkOrder.UnitID = lvo.BillDetails.Details.UnitID;
                                lvo.BillDetails.Details.RadiologyWorkOrder.AddedDateTime = lvo.BillDetails.Details.AddedDateTime;
                                lvo.BillDetails.Details.RadiologyWorkOrder.Date = new DateTime?(lvo.BillDetails.Details.Date);
                                lvo.BillDetails.Details.RadiologyWorkOrder.BillNo = lvo.BillDetails.Details.BillNo;
                                num9 = 0;
                            }
                            break;
                        }
                        break;
                    }
                }
                else
                {
                    clsBillVO lvo1 = lvo.BillDetails.Details;
                    Func<clsChargeVO, bool> predicate = null;
                    int num1 = 0;
                    while (true)
                    {
                        if (num1 >= lvo1.ChargeDetails.Count)
                        {
                            DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddBillOld");
                            this.dbServer.AddInParameter(storedProcCommand, "IsPackageBill", DbType.Int64, lvo.BillDetails.IsPackageBill);
                            if (lvo.BillDetails.IsPackageBill)
                            {
                                StringBuilder builder = new StringBuilder();
                                foreach (clsChargeVO evo2 in lvo1.ChargeDetails)
                                {
                                    if ((evo2.PackageID > 0L) && evo2.isPackageService)
                                    {
                                        builder.Append("," + Convert.ToString(evo2.PackageID));
                                    }
                                }
                                builder.Replace(",", "", 0, 1);
                                this.dbServer.AddInParameter(storedProcCommand, "ipPackageList", DbType.String, Convert.ToString(builder));
                            }
                            this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, lvo1.LinkServer);
                            if (lvo1.LinkServer != null)
                            {
                                this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, lvo1.LinkServer.Replace(@"\", "_"));
                            }
                            this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, lvo1.Date);
                            this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External_Id", DbType.Int64, lvo1.Opd_Ipd_External_Id);
                            this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External_UnitId", DbType.Int64, lvo1.Opd_Ipd_External_UnitId);
                            this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External", DbType.Int16, lvo1.Opd_Ipd_External);
                            this.dbServer.AddInParameter(storedProcCommand, "CashCounterId", DbType.Int64, lvo1.CashCounterId);
                            this.dbServer.AddInParameter(storedProcCommand, "CompanyId", DbType.Int64, lvo1.CompanyId);
                            this.dbServer.AddInParameter(storedProcCommand, "PatientSourceId", DbType.Int64, lvo1.PatientSourceId);
                            this.dbServer.AddInParameter(storedProcCommand, "PatientCategoryId", DbType.Int64, lvo1.PatientCategoryId);
                            this.dbServer.AddInParameter(storedProcCommand, "TariffId", DbType.Int64, lvo1.TariffId);
                            if (lvo1.BillNo != null)
                            {
                                lvo1.BillNo = lvo1.BillNo.Trim();
                            }
                            this.dbServer.AddInParameter(storedProcCommand, "TotalBillAmount", DbType.Double, lvo1.TotalBillAmount);
                            this.dbServer.AddInParameter(storedProcCommand, "TotalConcessionAmount", DbType.Double, lvo1.TotalConcessionAmount);
                            this.dbServer.AddInParameter(storedProcCommand, "NetBillAmount", DbType.Double, lvo1.NetBillAmount);
                            this.dbServer.AddInParameter(storedProcCommand, "CalculatedNetBillAmount", DbType.Double, lvo1.CalculatedNetBillAmount);
                            this.dbServer.AddInParameter(storedProcCommand, "SelfAmount", DbType.Double, lvo1.SelfAmount);
                            if (lvo1.BalanceAmountSelf < 0.0)
                            {
                                lvo1.BalanceAmountSelf = 0.0;
                            }
                            this.dbServer.AddInParameter(storedProcCommand, "BalanceAmountSelf", DbType.Double, lvo1.BalanceAmountSelf);
                            this.dbServer.AddInParameter(storedProcCommand, "StaffFree", DbType.Double, lvo1.StaffFree);
                            this.dbServer.AddInParameter(storedProcCommand, "NonSelfAmount", DbType.Double, lvo1.NonSelfAmount);
                            this.dbServer.AddInParameter(storedProcCommand, "BalanceAmountNonSelf", DbType.Double, lvo1.BalanceAmountNonSelf);
                            this.dbServer.AddInParameter(storedProcCommand, "TotalAdvance", DbType.Double, lvo1.TotalAdvance);
                            this.dbServer.AddInParameter(storedProcCommand, "IsPrinted", DbType.Boolean, lvo1.IsPrinted);
                            this.dbServer.AddInParameter(storedProcCommand, "BillType", DbType.Int16, (short) lvo1.BillType);
                            this.dbServer.AddInParameter(storedProcCommand, "BillPaymentType", DbType.Int16, (short) lvo1.BillPaymentType);
                            this.dbServer.AddInParameter(storedProcCommand, "SponserType", DbType.Boolean, lvo1.SponserType);
                            this.dbServer.AddInParameter(storedProcCommand, "SeniorCitizenCon", DbType.Boolean, lvo1.SeniorCitizenCon);
                            if (lvo1.Remark != null)
                            {
                                lvo1.Remark = lvo1.Remark.Trim();
                            }
                            this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, lvo1.Remark);
                            if (lvo1.BillRemark != null)
                            {
                                lvo1.BillRemark = lvo1.BillRemark.Trim();
                            }
                            this.dbServer.AddInParameter(storedProcCommand, "BillRemark", DbType.String, lvo1.BillRemark);
                            this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, lvo1.IsFreezed);
                            this.dbServer.AddInParameter(storedProcCommand, "CostingDivisionID", DbType.Int64, lvo1.CostingDivisionID);
                            this.dbServer.AddInParameter(storedProcCommand, "GrossDiscountReasonID", DbType.Int64, lvo1.GrossDiscountReasonID);
                            this.dbServer.AddInParameter(storedProcCommand, "GrossDiscountPercentage", DbType.Double, lvo1.GrossDiscountPercentage);
                            this.dbServer.AddInParameter(storedProcCommand, "ConcessionAuthorizedBy", DbType.Int64, lvo1.ConcessionAuthorizedBy);
                            this.dbServer.AddInParameter(storedProcCommand, "ConcessionRemark", DbType.String, lvo1.ConcessionRemark);
                            this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, lvo1.Status);
                            this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            lvo1.UnitID = UserVo.UserLoginInfo.UnitId;
                            this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, lvo1.AddedDateTime);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddParameter(storedProcCommand, "BillNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000");
                            this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, lvo1.ID);
                            this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.AddInParameter(storedProcCommand, "ConcessionreasonId", DbType.Int64, lvo1.ConcessionReasonId);
                            this.dbServer.AddInParameter(storedProcCommand, "IsPackageConsumption", DbType.Boolean, flag);
                            StringBuilder builder2 = new StringBuilder();
                            StringBuilder builder3 = new StringBuilder();
                            StringBuilder builder4 = new StringBuilder();
                            StringBuilder builder5 = new StringBuilder();
                            int num3 = 0;
                            while (true)
                            {
                                if (num3 >= lvo1.ChargeDetails.Count)
                                {
                                    this.dbServer.AddInParameter(storedProcCommand, "ChargeIdList", DbType.String, builder2.ToString());
                                    this.dbServer.AddInParameter(storedProcCommand, "StatusList", DbType.String, builder5.ToString());
                                    this.dbServer.AddInParameter(storedProcCommand, "SponsorTypeList", DbType.String, builder3.ToString());
                                    this.dbServer.AddInParameter(storedProcCommand, "BalanceAmountList", DbType.String, builder4.ToString());
                                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                                    lvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                                    lvo.BillDetails.Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                                    lvo.BillDetails.Details.BillNo = (string) this.dbServer.GetParameterValue(storedProcCommand, "BillNo");
                                    if (lvo.SuccessStatus == -2)
                                    {
                                        throw new Exception();
                                    }
                                    if (lvo.BillDetails.Details.IsFreezed && (lvo.BillDetails.Details.PaymentDetails != null))
                                    {
                                        clsBasePaymentDAL instance = clsBasePaymentDAL.GetInstance();
                                        clsAddPaymentBizActionVO nvo3 = new clsAddPaymentBizActionVO {
                                            Details = new clsPaymentVO()
                                        };
                                        nvo3.Details = lvo.BillDetails.Details.PaymentDetails;
                                        nvo3.Details.BillID = lvo.BillDetails.Details.ID;
                                        nvo3.Details.BillAmount = lvo1.NetBillAmount;
                                        nvo3.Details.Date = new DateTime?(lvo.BillDetails.Details.Date);
                                        nvo3.Details.LinkServer = lvo.BillDetails.Details.LinkServer;
                                        nvo3.myTransaction = true;
                                        nvo3 = (clsAddPaymentBizActionVO) instance.Add(nvo3, UserVo, pConnection, transaction);
                                        if (nvo3.SuccessStatus == -1)
                                        {
                                            throw new Exception();
                                        }
                                        lvo.BillDetails.Details.PaymentDetails.ID = nvo3.Details.ID;
                                    }
                                    if (((lvo.BillDetails.Details.PathoWorkOrder != null) && (lvo.BillDetails.Details.PathoWorkOrder.PathoSpecilizationID > 0L)) && ((lvo.BillDetails.Details.IsFreezed && !lvo.BillDetails.Details.IsIPDBill) || lvo.BillDetails.Details.IsIPDBill))
                                    {
                                        lvo.BillDetails.Details.PathoWorkOrder.Opd_Ipd_External_ID = lvo.BillDetails.Details.Opd_Ipd_External_Id;
                                        lvo.BillDetails.Details.PathoWorkOrder.Opd_Ipd_External_UnitID = lvo.BillDetails.Details.Opd_Ipd_External_UnitId;
                                        lvo.BillDetails.Details.PathoWorkOrder.SampleType = false;
                                        lvo.BillDetails.Details.PathoWorkOrder.Opd_Ipd_External = new long?((long) lvo.BillDetails.Details.Opd_Ipd_External);
                                        lvo.BillDetails.Details.PathoWorkOrder.UnitId = lvo.BillDetails.Details.UnitID;
                                        lvo.BillDetails.Details.PathoWorkOrder.AddedDateTime = lvo.BillDetails.Details.AddedDateTime;
                                        lvo.BillDetails.Details.PathoWorkOrder.OrderDate = new DateTime?(lvo.BillDetails.Details.Date);
                                        lvo.BillDetails.Details.PathoWorkOrder.BillNo = lvo.BillDetails.Details.BillNo;
                                        lvo.BillDetails.Details.PathoWorkOrder.BillID = lvo.BillDetails.Details.ID;
                                        int num4 = 0;
                                        while (true)
                                        {
                                            if (num4 >= lvo1.ChargeDetails.Count)
                                            {
                                                clsBaseOrderBookingDAL instance = clsBaseOrderBookingDAL.GetInstance();
                                                clsAddPathOrderBookingBizActionVO nvo4 = new clsAddPathOrderBookingBizActionVO {
                                                    myTransaction = true,
                                                    PathOrderBooking = lvo.BillDetails.Details.PathoWorkOrder,
                                                    PathOrderBookingDetailList = lvo.BillDetails.Details.PathoWorkOrder.Items
                                                };
                                                nvo4 = (clsAddPathOrderBookingBizActionVO) instance.AddPathOrderBooking(nvo4, UserVo, transaction, pConnection);
                                                if (nvo4.SuccessStatus == -1)
                                                {
                                                    throw new Exception();
                                                }
                                                lvo.BillDetails.Details.PathoWorkOrder.ID = nvo4.PathOrderBooking.ID;
                                                break;
                                            }
                                            if (lvo.BillDetails.Details.PathoWorkOrder.PathoSpecilizationID == lvo1.ChargeDetails[num4].ServiceSpecilizationID)
                                            {
                                                lvo.BillDetails.Details.PathoWorkOrder.DoctorID = lvo.BillDetails.Details.PathoWorkOrder.DoctorID;
                                                clsPathOrderBookingVO pathologyTestDetails = new clsPathOrderBookingVO();
                                                pathologyTestDetails = this.GetPathologyTestDetails(lvo1.ChargeDetails[num4].ServiceId, UserVo);
                                                if ((pathologyTestDetails.Items != null) && (pathologyTestDetails.Items.Count > 0))
                                                {
                                                    foreach (clsPathOrderBookingDetailVO lvo2 in pathologyTestDetails.Items)
                                                    {
                                                        lvo2.ServiceID = lvo1.ChargeDetails[num4].ServiceId;
                                                        lvo2.ChargeID = lvo1.ChargeDetails[num4].ID;
                                                        lvo2.TariffServiceID = lvo1.ChargeDetails[num4].TariffServiceId;
                                                        lvo2.TestCharges = lvo1.ChargeDetails[num4].Rate;
                                                        lvo.BillDetails.Details.PathoWorkOrder.Items.Add(lvo2);
                                                    }
                                                }
                                            }
                                            num4++;
                                        }
                                    }
                                    if (((lvo.BillDetails.Details.RadiologyWorkOrder != null) && (lvo.BillDetails.Details.RadiologyWorkOrder.OrderBookingDetails.Count > 0)) && ((lvo.BillDetails.Details.IsFreezed && !lvo.BillDetails.Details.IsIPDBill) || lvo.BillDetails.Details.IsIPDBill))
                                    {
                                        lvo.BillDetails.Details.RadiologyWorkOrder.Opd_Ipd_External_ID = lvo.BillDetails.Details.Opd_Ipd_External_Id;
                                        lvo.BillDetails.Details.RadiologyWorkOrder.Opd_Ipd_External_UnitID = lvo.BillDetails.Details.Opd_Ipd_External_UnitId;
                                        lvo.BillDetails.Details.RadiologyWorkOrder.Opd_Ipd_External = new long?((long) lvo.BillDetails.Details.Opd_Ipd_External);
                                        lvo.BillDetails.Details.RadiologyWorkOrder.UnitID = lvo.BillDetails.Details.UnitID;
                                        lvo.BillDetails.Details.RadiologyWorkOrder.AddedDateTime = lvo.BillDetails.Details.AddedDateTime;
                                        lvo.BillDetails.Details.RadiologyWorkOrder.Date = new DateTime?(lvo.BillDetails.Details.Date);
                                        lvo.BillDetails.Details.RadiologyWorkOrder.BillNo = lvo.BillDetails.Details.BillNo;
                                        lvo.BillDetails.Details.RadiologyWorkOrder.BillID = lvo.BillDetails.Details.ID;
                                        int num5 = 0;
                                        while (true)
                                        {
                                            if (num5 >= lvo.BillDetails.Details.RadiologyWorkOrder.OrderBookingDetails.Count)
                                            {
                                                clsBaseRadiologyDAL instance = clsBaseRadiologyDAL.GetInstance();
                                                clsAddRadOrderBookingBizActionVO nvo5 = new clsAddRadOrderBookingBizActionVO {
                                                    BookingDetails = lvo.BillDetails.Details.RadiologyWorkOrder,
                                                    myTransaction = true
                                                };
                                                nvo5 = (clsAddRadOrderBookingBizActionVO) instance.AddOrderBooking(nvo5, UserVo, pConnection, transaction);
                                                if (nvo5.SuccessStatus == -1)
                                                {
                                                    throw new Exception();
                                                }
                                                lvo1.RadiologyWorkOrder.ID = nvo5.BookingDetails.ID;
                                                break;
                                            }
                                            int num6 = 0;
                                            while (true)
                                            {
                                                if (num6 < lvo1.ChargeDetails.Count)
                                                {
                                                    if (lvo.BillDetails.Details.RadiologyWorkOrder.OrderBookingDetails[num5].TariffServiceID != lvo1.ChargeDetails[num6].TariffServiceId)
                                                    {
                                                        num6++;
                                                        continue;
                                                    }
                                                    lvo.BillDetails.Details.RadiologyWorkOrder.OrderBookingDetails[num5].ChargeID = lvo1.ChargeDetails[num6].ID;
                                                    clsRadOrderBookingDetailsVO radilogyTestDetails = new clsRadOrderBookingDetailsVO();
                                                    radilogyTestDetails = this.GetRadilogyTestDetails(lvo.BillDetails.Details.RadiologyWorkOrder.OrderBookingDetails[num5].ServiceID, UserVo);
                                                    if (radilogyTestDetails != null)
                                                    {
                                                        lvo.BillDetails.Details.RadiologyWorkOrder.OrderBookingDetails[num5].TestID = radilogyTestDetails.TestID;
                                                    }
                                                }
                                                num5++;
                                                break;
                                            }
                                        }
                                    }
                                    if (lvo1.PharmacyItems.Items.Count > 0)
                                    {
                                        clsBaseItemSalesDAL instance = clsBaseItemSalesDAL.GetInstance();
                                        clsAddItemSalesBizActionVO nvo6 = new clsAddItemSalesBizActionVO {
                                            Details = lvo1.PharmacyItems
                                        };
                                        nvo6.Details.BillID = lvo.BillDetails.Details.ID;
                                        nvo6.Details.IsBilled = lvo.BillDetails.Details.IsFreezed;
                                        nvo6.myTransaction = true;
                                        nvo6 = (clsAddItemSalesBizActionVO) instance.Add(nvo6, UserVo, pConnection, transaction);
                                        lvo1.PharmacyItems.ID = nvo6.Details.ID;
                                    }
                                    break;
                                }
                                builder2.Append(lvo1.ChargeDetails[num3].ID);
                                builder5.Append(lvo1.ChargeDetails[num3].Status);
                                builder4.Append((double) (lvo1.ChargeDetails[num3].NetAmount - lvo1.ChargeDetails[num3].PaidAmount));
                                builder3.Append(lvo1.ChargeDetails[num3].SponsorType);
                                if (num3 < (lvo1.ChargeDetails.Count - 1))
                                {
                                    builder2.Append(",");
                                    builder5.Append(",");
                                    builder4.Append(",");
                                    builder3.Append(",");
                                }
                                num3++;
                            }
                            break;
                        }
                        if (!lvo1.ChargeDetails[num1].ChildPackageService)
                        {
                            clsBaseChargeDAL instance = clsBaseChargeDAL.GetInstance();
                            clsAddChargeBizActionVO nvo = new clsAddChargeBizActionVO {
                                Details = lvo1.ChargeDetails[num1]
                            };
                            if (lvo1.ChargeDetails[num1].ParentID > 0L)
                            {
                                flag = true;
                            }
                            nvo.Details.PaidAmount = !nvo.Details.Status ? 0.0 : nvo.Details.NetAmount;
                            nvo.Details.IsBilled = lvo.BillDetails.Details.IsFreezed;
                            nvo.Details.Date = new DateTime?(lvo1.Date);
                            nvo.Details.Opd_Ipd_External_Id = lvo1.Opd_Ipd_External_Id;
                            nvo.Details.Opd_Ipd_External_UnitId = lvo1.Opd_Ipd_External_UnitId;
                            nvo.Details.Opd_Ipd_External = lvo1.Opd_Ipd_External;
                            nvo.Details.ClassId = lvo1.ChargeDetails[num1].ClassId;
                            nvo.Details.IsIPDBill = lvo1.IsIPDBill;
                            nvo = (clsAddChargeBizActionVO) instance.Add(nvo, UserVo, pConnection, transaction, 0L, 0L);
                            if (nvo.SuccessStatus == -1)
                            {
                                throw new Exception();
                            }
                            lvo1.ChargeDetails[num1].ID = nvo.Details.ID;
                            lvo1.ChargeDetails[num1].ChargeDetails.ID = nvo.Details.ChargeDetails.ID;
                            iParentID = lvo1.ChargeDetails[num1].ID;
                            iCDParentID = lvo1.ChargeDetails[num1].ChargeDetails.ID;
                            if (predicate == null)
                            {
                                predicate = charge => charge.PackageID.Equals(lvo1.ChargeDetails[num1].PackageID) && charge.ChildPackageService;
                            }
                            foreach (clsChargeVO evo in lvo1.ChargeDetails.Where<clsChargeVO>(predicate))
                            {
                                clsBaseChargeDAL edal2 = clsBaseChargeDAL.GetInstance();
                                clsAddChargeBizActionVO nvo2 = new clsAddChargeBizActionVO();
                                evo.ID = 0L;
                                nvo2.Details = evo;
                                nvo2.Details.PaidAmount = !nvo2.Details.Status ? 0.0 : evo.NetAmount;
                                nvo2.Details.IsBilled = lvo.BillDetails.Details.IsFreezed;
                                nvo2.Details.Date = new DateTime?(lvo1.Date);
                                nvo2.Details.Opd_Ipd_External_Id = lvo1.Opd_Ipd_External_Id;
                                nvo2.Details.Opd_Ipd_External_UnitId = lvo1.Opd_Ipd_External_UnitId;
                                nvo2.Details.Opd_Ipd_External = lvo1.Opd_Ipd_External;
                                nvo2 = (clsAddChargeBizActionVO) edal2.Add(nvo2, UserVo, pConnection, transaction, iParentID, iCDParentID);
                                if (nvo2.SuccessStatus == -1)
                                {
                                    throw new Exception();
                                }
                                evo.ID = nvo2.Details.ID;
                                evo.ChargeDetails.ID = nvo2.Details.ChargeDetails.ID;
                            }
                        }
                        num1++;
                    }
                    goto TR_000F;
                }
                goto TR_0096;
            TR_000F:
                command10 = this.dbServer.GetStoredProcCommand("CIMS_AddRefundApprovalRequest");
                command10.Connection = pConnection;
                this.dbServer.AddParameter(command10, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, lvo.Details.ApprovalRequestID);
                this.dbServer.AddInParameter(command10, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command10, "IsOPDBill", DbType.Boolean, lvo.Details.IsOPDBill);
                this.dbServer.AddInParameter(command10, "RequestTypeID", DbType.Int64, lvo.Details.RequestTypeID);
                this.dbServer.AddInParameter(command10, "RequestType", DbType.String, lvo.Details.RequestType);
                this.dbServer.AddInParameter(command10, "BillID", DbType.Int64, lvo.BillDetails.Details.ID);
                this.dbServer.AddInParameter(command10, "BillUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command10, "ApprovalDate", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(command10, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command10, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(command10, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(command10, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(command10, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(command10, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(command10, "Status", DbType.Boolean, true);
                this.dbServer.ExecuteNonQuery(command10, transaction);
                lvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command10, "ResultStatus");
                lvo.Details.ApprovalRequestID = (long) this.dbServer.GetParameterValue(command10, "ID");
                lvo.Details.ApprovalRequestUnitID = UserVo.UserLoginInfo.UnitId;
                foreach (clsChargeVO evo7 in lvo.ChargeList)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddRefundChargeApprovalDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, evo7.ApprovalRequestDetailsID);
                    this.dbServer.AddInParameter(storedProcCommand, "IsRefundRequest", DbType.Boolean, lvo.IsRefundRequest);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "ChargeID", DbType.Int64, evo7.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "ChargeUnitID", DbType.Int64, evo7.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "ApprovalRequestID", DbType.Int64, lvo.Details.ApprovalRequestID);
                    this.dbServer.AddInParameter(storedProcCommand, "ApprovalRequestUnitID", DbType.Int64, lvo.Details.ApprovalRequestUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "ApprovalStatus", DbType.Boolean, false);
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, evo7.ServiceId);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    lvo.SuccessStatus = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
                    if (!lvo.IsRefundRequest)
                    {
                        break;
                    }
                }
                transaction.Commit();
                return lvo;
            TR_007C:
                if (objDetailsVO.PharmacyItems.Items.Count > 0)
                {
                    objArray = new object[] { "Delete from T_Itemsaledetails where UnitID = ", objDetailsVO.UnitID, " And ItemSaleId in (select ID from T_ItemSale where UnitID = ", objDetailsVO.PharmacyItems.UnitId, " And BillId = ", objDetailsVO.ID, ")" };
                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(objArray));
                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                    objArray = new object[] { "Delete from T_ItemSale where UnitID = ", objDetailsVO.UnitID, " And BillId = ", objDetailsVO.ID };
                    DbCommand command = this.dbServer.GetSqlStringCommand(string.Concat(objArray));
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    clsBaseItemSalesDAL instance = clsBaseItemSalesDAL.GetInstance();
                    clsAddItemSalesBizActionVO nvo12 = new clsAddItemSalesBizActionVO {
                        Details = objDetailsVO.PharmacyItems
                    };
                    nvo12.Details.ItemSaleNo = objDetailsVO.PharmacyItems.ItemSaleNo;
                    nvo12.Details.BillID = lvo.BillDetails.Details.ID;
                    nvo12.Details.IsBilled = lvo.BillDetails.Details.IsFreezed;
                    nvo12 = (clsAddItemSalesBizActionVO) instance.Add(nvo12, UserVo, pConnection, transaction);
                    if (nvo12.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    objDetailsVO.PharmacyItems.ID = nvo12.Details.ID;
                }
                goto TR_000F;
            TR_0089:
                num9++;
            TR_0096:
                while (true)
                {
                    if (num9 < lvo.BillDetails.Details.RadiologyWorkOrder.OrderBookingDetails.Count)
                    {
                        for (int i = 0; i < objDetailsVO.ChargeDetails.Count; i++)
                        {
                            if (objDetailsVO.ChargeDetails[i].ROBID > 0L)
                            {
                                lvo.BillDetails.Details.RadiologyWorkOrder.ID = objDetailsVO.ChargeDetails[i].ROBID;
                            }
                            lvo.BillDetails.Details.RadiologyWorkOrder.ID = lvo.BillDetails.Details.RadiologyWorkOrder.ID;
                            if (lvo.BillDetails.Details.RadiologyWorkOrder.OrderBookingDetails[num9].TariffServiceID == objDetailsVO.ChargeDetails[i].TariffServiceId)
                            {
                                lvo.BillDetails.Details.RadiologyWorkOrder.OrderBookingDetails[num9].ID = objDetailsVO.ChargeDetails[i].ROBDID;
                                lvo.BillDetails.Details.RadiologyWorkOrder.OrderBookingDetails[num9].ChargeID = objDetailsVO.ChargeDetails[i].ID;
                                clsRadOrderBookingDetailsVO svo2 = new clsRadOrderBookingDetailsVO();
                                if (svo2 != null)
                                {
                                    lvo.BillDetails.Details.RadiologyWorkOrder.OrderBookingDetails[num9].ID = svo2.ID;
                                    objDetailsVO.ChargeDetails[i].ROBDID = svo2.ID;
                                    lvo.BillDetails.Details.RadiologyWorkOrder.OrderBookingDetails[num9].TestID = svo2.TestID;
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        clsBaseRadiologyDAL instance = clsBaseRadiologyDAL.GetInstance();
                        clsAddRadOrderBookingBizActionVO nvo11 = new clsAddRadOrderBookingBizActionVO {
                            BookingDetails = lvo.BillDetails.Details.RadiologyWorkOrder
                        };
                        nvo11 = (clsAddRadOrderBookingBizActionVO) instance.AddOrderBooking(nvo11, UserVo, pConnection, transaction);
                        if (nvo11.SuccessStatus == -1)
                        {
                            throw new Exception();
                        }
                        if (nvo11.BookingDetails.ID > 0L)
                        {
                            lvo.BillDetails.Details.RadiologyWorkOrder.ID = nvo11.BookingDetails.ID;
                        }
                        if ((lvo.BillDetails.Details.DeletedChargeDetails != null) && ((lvo.BillDetails.Details.DeletedChargeDetails.Count > 0) && lvo.BillDetails.Details.IsIPDBill))
                        {
                            foreach (clsChargeVO evo6 in lvo.BillDetails.Details.DeletedChargeDetails)
                            {
                                if ((evo6.ID != 0L) && ((evo6.TariffServiceId != 0L) && (lvo.BillDetails.Details.RadioSpecilizationID == evo6.ServiceSpecilizationID)))
                                {
                                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Delete from T_RadiologyOrderBookingDetails where ChargeID = ", evo6.ID, " And TariffServiceId = ", evo6.TariffServiceId, " And UnitID = ", evo6.UnitID }));
                                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                                }
                            }
                        }
                        goto TR_007C;
                    }
                    break;
                }
                goto TR_0089;
            }
            catch (Exception)
            {
                transaction.Rollback();
                lvo.Details = null;
                lvo.SuccessStatus = -1;
            }
            finally
            {
                pConnection.Close();
                transaction = null;
                pConnection = null;
            }
            return lvo;
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

        private clsAddRefundBizActionVO UpdateDetails(clsAddRefundBizActionVO BizActionObj, DbConnection pConnection, DbTransaction pTransaction)
        {
            return BizActionObj;
        }
    }
}

