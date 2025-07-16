namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.CustomExceptions;
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsAdvanceDAL : clsBaseAdvanceDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsAdvanceDAL()
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

        public override IValueObject AddAdvance(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddAdvanceBizActionVO bizActionObj = valueObject as clsAddAdvanceBizActionVO;
            bizActionObj = (bizActionObj.Details.ID != 0L) ? this.UpdateDetails(bizActionObj) : this.AddDetails(bizActionObj, UserVo);
            return valueObject;
        }

        public override IValueObject AddAdvance(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            clsAddAdvanceBizActionVO bizActionObj = valueObject as clsAddAdvanceBizActionVO;
            if (bizActionObj.Details.ID == 0L)
            {
                bizActionObj = this.AddDetailsWithTransaction(bizActionObj, UserVo, pConnection, pTransaction);
            }
            return valueObject;
        }

        public override IValueObject AddAdvanceWithPackageBill(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            clsAddAdvanceBizActionVO nvo = valueObject as clsAddAdvanceBizActionVO;
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
                clsAdvanceVO details = nvo.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddAdvance");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, details.LinkServer);
                if (details.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, details.LinkServer.Replace(@"\", "_"));
                }
                details.Balance = (details.Total - details.Used) - details.Refund;
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, details.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyID", DbType.Int64, details.CompanyID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, details.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Total", DbType.Double, details.Total);
                this.dbServer.AddInParameter(storedProcCommand, "AdvanceTypeId", DbType.Int64, details.AdvanceTypeId);
                this.dbServer.AddInParameter(storedProcCommand, "Used", DbType.Double, details.Used);
                this.dbServer.AddInParameter(storedProcCommand, "Refund", DbType.Double, details.Refund);
                this.dbServer.AddInParameter(storedProcCommand, "Balance", DbType.Double, details.Balance);
                this.dbServer.AddInParameter(storedProcCommand, "AdvanceAgainstId", DbType.Int64, details.AdvanceAgainstId);
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
                this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int64, details.PackageID);
                this.dbServer.AddInParameter(storedProcCommand, "PackageBillID", DbType.Int64, details.PackageBillID);
                this.dbServer.AddInParameter(storedProcCommand, "PackageBillUnitID", DbType.Int64, details.PackageBillUnitID);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, details.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddParameter(storedProcCommand, "AdvanceNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "00000000000000000000000000000000000000000000000000000000000000000000");
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.Details.AdvanceNO = Convert.ToString(this.dbServer.GetParameterValue(storedProcCommand, "AdvanceNo"));
                if (nvo.Details.PaymentDetails != null)
                {
                    clsBasePaymentDAL instance = clsBasePaymentDAL.GetInstance();
                    clsAddPaymentBizActionVO nvo2 = new clsAddPaymentBizActionVO {
                        Details = new clsPaymentVO()
                    };
                    nvo2.Details = nvo.Details.PaymentDetails;
                    nvo2.Details.AdvanceID = nvo.Details.ID;
                    nvo2.Details.AdvanceAmount = details.Total;
                    nvo2.Details.Date = nvo.Details.Date;
                    nvo2.Details.LinkServer = nvo.Details.LinkServer;
                    nvo2 = (clsAddPaymentBizActionVO) instance.Add(nvo2, UserVo, connection, transaction);
                    if (nvo2.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    nvo.Details.PaymentDetails.ID = nvo2.Details.ID;
                }
                if (pConnection == null)
                {
                    transaction.Commit();
                }
                nvo.SuccessStatus = 0;
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
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
            return nvo;
        }

        private clsAddAdvanceBizActionVO AddDetails(clsAddAdvanceBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbConnection pConnection = null;
            DbTransaction transaction = null;
            try
            {
                pConnection = this.dbServer.CreateConnection();
                if (pConnection.State == ConnectionState.Closed)
                {
                    pConnection.Open();
                }
                transaction = pConnection.BeginTransaction();
                clsAdvanceVO details = BizActionObj.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddAdvance");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, details.LinkServer);
                if (details.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, details.LinkServer.Replace(@"\", "_"));
                }
                details.Balance = (details.Total - details.Used) - details.Refund;
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, details.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyID", DbType.Int64, details.CompanyID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, details.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Total", DbType.Double, details.Total);
                this.dbServer.AddInParameter(storedProcCommand, "AdvanceTypeId", DbType.Int64, details.AdvanceTypeId);
                this.dbServer.AddInParameter(storedProcCommand, "Used", DbType.Double, details.Used);
                this.dbServer.AddInParameter(storedProcCommand, "Refund", DbType.Double, details.Refund);
                this.dbServer.AddInParameter(storedProcCommand, "Balance", DbType.Double, details.Balance);
                this.dbServer.AddInParameter(storedProcCommand, "AdvanceAgainstId", DbType.Int64, details.AdvanceAgainstId);
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
                this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int64, details.PackageID);
                this.dbServer.AddInParameter(storedProcCommand, "PackageBillID", DbType.Int64, details.PackageBillID);
                this.dbServer.AddInParameter(storedProcCommand, "PackageBillUnitID", DbType.Int64, details.PackageBillUnitID);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, details.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddParameter(storedProcCommand, "AdvanceNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "00000000000000000000000000000000000000000000000000000000000000000000");
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                BizActionObj.Details.AdvanceNO = Convert.ToString(this.dbServer.GetParameterValue(storedProcCommand, "AdvanceNo"));
                if (BizActionObj.Details.PaymentDetails != null)
                {
                    clsBasePaymentDAL instance = clsBasePaymentDAL.GetInstance();
                    clsAddPaymentBizActionVO valueObject = new clsAddPaymentBizActionVO {
                        Details = new clsPaymentVO()
                    };
                    valueObject.Details = BizActionObj.Details.PaymentDetails;
                    valueObject.Details.AdvanceID = BizActionObj.Details.ID;
                    valueObject.Details.AdvanceAmount = details.Total;
                    valueObject.Details.Date = BizActionObj.Details.Date;
                    valueObject.Details.LinkServer = BizActionObj.Details.LinkServer;
                    valueObject = (clsAddPaymentBizActionVO) instance.Add(valueObject, UserVo, pConnection, transaction);
                    if (valueObject.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    BizActionObj.Details.PaymentDetails.ID = valueObject.Details.ID;
                }
                transaction.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
            }
            finally
            {
                pConnection.Close();
                pConnection = null;
                transaction = null;
            }
            return BizActionObj;
        }

        private clsAddAdvanceBizActionVO AddDetailsWithTransaction(clsAddAdvanceBizActionVO BizActionObj, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
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
                clsAdvanceVO details = BizActionObj.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddAdvance");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, details.LinkServer);
                if (details.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, details.LinkServer.Replace(@"\", "_"));
                }
                details.Balance = (details.Total - details.Used) - details.Refund;
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, details.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyID", DbType.Int64, details.CompanyID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, details.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Total", DbType.Double, details.Total);
                this.dbServer.AddInParameter(storedProcCommand, "AdvanceTypeId", DbType.Int64, details.AdvanceTypeId);
                this.dbServer.AddInParameter(storedProcCommand, "Used", DbType.Double, details.Used);
                this.dbServer.AddInParameter(storedProcCommand, "Refund", DbType.Double, details.Refund);
                this.dbServer.AddInParameter(storedProcCommand, "Balance", DbType.Double, details.Balance);
                this.dbServer.AddInParameter(storedProcCommand, "AdvanceAgainstId", DbType.Int64, details.AdvanceAgainstId);
                if (details.Remarks != null)
                {
                    details.Remarks = details.Remarks.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, details.Remarks);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, details.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CostingDivisionID", DbType.Int64, details.CostingDivisionID);
                this.dbServer.AddInParameter(storedProcCommand, "FromRefundID", DbType.Int64, details.FromRefundID);
                this.dbServer.AddInParameter(storedProcCommand, "PackageBillID", DbType.Int64, details.PackageBillID);
                this.dbServer.AddInParameter(storedProcCommand, "PackageBillUnitID", DbType.Int64, details.PackageBillUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, details.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, details.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddParameter(storedProcCommand, "AdvanceNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "00000000000000000000000000000000000000000000000000000000000000000000");
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                BizActionObj.Details.AdvanceNO = Convert.ToString(this.dbServer.GetParameterValue(storedProcCommand, "AdvanceNo"));
                if (BizActionObj.Details.PaymentDetails != null)
                {
                    clsBasePaymentDAL instance = clsBasePaymentDAL.GetInstance();
                    clsAddPaymentBizActionVO valueObject = new clsAddPaymentBizActionVO {
                        Details = new clsPaymentVO()
                    };
                    valueObject.Details = BizActionObj.Details.PaymentDetails;
                    valueObject.Details.AdvanceID = BizActionObj.Details.ID;
                    valueObject.Details.AdvanceAmount = details.Total;
                    valueObject.Details.Date = BizActionObj.Details.Date;
                    valueObject.Details.LinkServer = BizActionObj.Details.LinkServer;
                    valueObject = (clsAddPaymentBizActionVO) instance.Add(valueObject, UserVo, connection, transaction);
                    if (valueObject.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    BizActionObj.Details.PaymentDetails.ID = valueObject.Details.ID;
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
                    connection.Close();
                    connection = null;
                    transaction = null;
                }
            }
            return BizActionObj;
        }

        public override IValueObject DeleteAdvance(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteAdvanceBizActionVO nvo = valueObject as clsDeleteAdvanceBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteAdvance");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
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

        public override IValueObject GetAdvance(IValueObject valueObject, clsUserVO UserVo)
        {
            return valueObject;
        }

        public override IValueObject GetAdvanceList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetAdvanceListBizActionVO nvo = valueObject as clsGetAdvanceListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAdvance");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CostingDivisionID", DbType.Int64, nvo.CostingDivisionID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AllCompany", DbType.Boolean, nvo.AllCompanies);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyID", DbType.Int64, nvo.CompanyID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Details == null)
                    {
                        nvo.Details = new List<clsAdvanceVO>();
                    }
                    while (reader.Read())
                    {
                        clsAdvanceVO item = new clsAdvanceVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            PatientID = (long) DALHelper.HandleDBNull(reader["PatientId"]),
                            PatientUnitID = (long) DALHelper.HandleDBNull(reader["PatientUnitID"]),
                            CompanyID = (long) DALHelper.HandleDBNull(reader["CompanyID"]),
                            Date = DALHelper.HandleDate(reader["Date"]),
                            Total = (double) DALHelper.HandleDBNull(reader["Total"]),
                            AdvanceTypeId = (long) DALHelper.HandleDBNull(reader["AdvanceTypeId"]),
                            AdvanceAgainstId = (long) DALHelper.HandleDBNull(reader["AdvanceAgainstId"]),
                            Used = (double) DALHelper.HandleDBNull(reader["Used"]),
                            Refund = (double) DALHelper.HandleDBNull(reader["Refund"]),
                            Balance = (double) DALHelper.HandleDBNull(reader["Balance"]),
                            Remarks = (string) DALHelper.HandleDBNull(reader["Remarks"]),
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
                            Company = (string) DALHelper.HandleDBNull(reader["Company"]),
                            AdvanceAgainst = (string) DALHelper.HandleDBNull(reader["AdvanceAgainst"]),
                            UnitName = (string) DALHelper.HandleDBNull(reader["UnitName"]),
                            AdvanceNO = Convert.ToString(DALHelper.HandleDBNull(reader["AdvanceNO"])),
                            ApprovalRequestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalRequestID"])),
                            ApprovalRequestUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalRequestUnitID"])),
                            ApprovalRequestDetailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalRequestDetailsID"])),
                            ApprovalRequestDetailsUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalRequestDetailsUnitID"])),
                            IsSendForApproval = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSendForApproval"])),
                            LevelID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LevelID"])),
                            SelectCharge = Convert.ToBoolean(DALHelper.HandleDBNull(reader["SelectCharge"])),
                            ApprovalRemark = Convert.ToString(DALHelper.HandleDBNull(reader["ApprovalRemark"])),
                            ApprovalStatus = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ApprovalStatus"])),
                            ApprovalRequestRemark = Convert.ToString(DALHelper.HandleDBNull(reader["ApprovalRequestRemark"]))
                        };
                        item.Refund = Convert.ToDouble(DALHelper.HandleDBNull(reader["Amount"]));
                        item.IsRefund = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRefund"]));
                        item.PackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageID"]));
                        item.PackageName = Convert.ToString(DALHelper.HandleDBNull(reader["PackageName"]));
                        item.IsPackageBillFreeze = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackageBillFreeze"]));
                        item.PackageBillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageBillID"]));
                        item.PackageBillUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageBillUnitID"]));
                        item.PackageBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PackageBillAmount"]));
                        item.PackageAdvanceBalance = Convert.ToDouble(DALHelper.HandleDBNull(reader["PackageAdvanceBalance"]));
                        nvo.Details.Add(item);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.PatientDailyCashAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PatientDailyCashAmount"]));
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

        public override IValueObject GetAdvanceListForRequestApproval(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetAdvanceListBizActionVO nvo = valueObject as clsGetAdvanceListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAdvanceListForRequestApproval");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.String, nvo.UnitID);
                if ((nvo.FirstName != null) && (nvo.FirstName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, nvo.FirstName);
                }
                if ((nvo.MiddleName != null) && (nvo.MiddleName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, nvo.MiddleName);
                }
                if ((nvo.LastName != null) && (nvo.LastName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, nvo.LastName);
                }
                if (nvo.FromDate != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate.Value.Date.Date);
                }
                if (nvo.ToDate != null)
                {
                    if (nvo.FromDate != null)
                    {
                        nvo.ToDate = new DateTime?(nvo.ToDate.Value.Date.AddDays(1.0));
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate.Value.Date.Date);
                }
                this.dbServer.AddInParameter(storedProcCommand, "CostingDivisionID", DbType.Int64, nvo.CostingDivisionID);
                this.dbServer.AddInParameter(storedProcCommand, "UserLevelID", DbType.Int64, nvo.UserLevelID);
                this.dbServer.AddInParameter(storedProcCommand, "UserRightsTypeID", DbType.Int64, nvo.UserRightsTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Details == null)
                    {
                        nvo.Details = new List<clsAdvanceVO>();
                    }
                    while (reader.Read())
                    {
                        clsAdvanceVO item = new clsAdvanceVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]))),
                            MRNO = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"])),
                            ContactNo = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo1"])),
                            Total = Convert.ToDouble(DALHelper.HandleDBNull(reader["Total"])),
                            Used = Convert.ToDouble(DALHelper.HandleDBNull(reader["Used"])),
                            Refund = Convert.ToDouble(DALHelper.HandleDBNull(reader["Refund"])),
                            Balance = Convert.ToDouble(DALHelper.HandleDBNull(reader["Balance"])),
                            PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            CostingDivisionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CostingDivisionID"])),
                            RequestType = Convert.ToString(DALHelper.HandleDBNull(reader["RequestType"])),
                            RequestTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RequestTypeID"])),
                            ApprovalRequestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalRequestID"])),
                            ApprovalRequestUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalRequestUnitID"])),
                            LevelID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LevelID"])),
                            ApprovalRemark = Convert.ToString(DALHelper.HandleDBNull(reader["ApprovalRemark"])),
                            FirstName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]))),
                            MiddleName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"]))),
                            LastName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])))
                        };
                        nvo.Details.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetPatientAdvanceList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientAdvanceListBizActionVO nvo = valueObject as clsGetPatientAdvanceListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientAdvance");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyID", DbType.Int64, nvo.CompanyID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CostingDivisionID", DbType.Int64, nvo.CostingDivisionID);
                this.dbServer.AddInParameter(storedProcCommand, "IsFromCompany", DbType.Boolean, nvo.IsFromCompany);
                if (nvo.AdvanceDetails != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int64, nvo.AdvanceDetails.PackageID);
                    this.dbServer.AddInParameter(storedProcCommand, "PackageBillID", DbType.Int64, nvo.AdvanceDetails.PackageBillID);
                    this.dbServer.AddInParameter(storedProcCommand, "PackageBillUnitID", DbType.Int64, nvo.AdvanceDetails.PackageBillUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "IsForTotalAdvance", DbType.Boolean, nvo.AdvanceDetails.IsForTotalAdvance);
                }
                this.dbServer.AddInParameter(storedProcCommand, "IsShowBothAdvance", DbType.Boolean, nvo.IsShowBothAdvance);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Details == null)
                    {
                        nvo.Details = new List<clsAdvanceVO>();
                    }
                    while (reader.Read())
                    {
                        clsAdvanceVO item = new clsAdvanceVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            PatientID = (long) DALHelper.HandleDBNull(reader["PatientId"]),
                            PatientUnitID = (long) DALHelper.HandleDBNull(reader["PatientUnitId"]),
                            CompanyID = (long) DALHelper.HandleDBNull(reader["CompanyID"]),
                            Date = DALHelper.HandleDate(reader["Date"]),
                            Total = (double) DALHelper.HandleDBNull(reader["Total"]),
                            AdvanceTypeId = (long) DALHelper.HandleDBNull(reader["AdvanceTypeId"]),
                            AdvanceAgainstId = (long) DALHelper.HandleDBNull(reader["AdvanceAgainstId"]),
                            Used = (double) DALHelper.HandleDBNull(reader["Used"]),
                            Refund = (double) DALHelper.HandleDBNull(reader["Refund"]),
                            Balance = (double) DALHelper.HandleDBNull(reader["Balance"]),
                            Remarks = (string) DALHelper.HandleDBNull(reader["Remarks"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitId"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            AddedBy = (long) DALHelper.HandleDBNull(reader["AddedBy"]),
                            AddedOn = (string) DALHelper.HandleDBNull(reader["AddedOn"]),
                            AddedDateTime = DALHelper.HandleDate(reader["AddedDateTime"]),
                            AddedWindowsLoginName = (string) DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]),
                            UpdatedBy = (long?) DALHelper.HandleDBNull(reader["UpdatedBy"]),
                            UpdatedOn = (string) DALHelper.HandleDBNull(reader["UpdatedOn"]),
                            UpdatedDateTime = DALHelper.HandleDate(reader["UpdatedDateTime"]),
                            UpdateWindowsLoginName = (string) DALHelper.HandleDBNull(reader["UpdateWindowsLoginName"]),
                            Company = (string) DALHelper.HandleDBNull(reader["Company"]),
                            AdvanceAgainst = (string) DALHelper.HandleDBNull(reader["AdvanceAgainst"]),
                            PackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageID"])),
                            PackageBillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageBillID"])),
                            PackageBillUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageBillUnitID"]))
                        };
                        nvo.Details.Add(item);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.PatientDailyCashAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PatientDailyCashAmount"]));
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

        public override IValueObject UpdateAdvance(IValueObject valueObject, clsUserVO UserVo)
        {
            return valueObject;
        }

        private clsAddAdvanceBizActionVO UpdateDetails(clsAddAdvanceBizActionVO BizActionObj)
        {
            return BizActionObj;
        }
    }
}

