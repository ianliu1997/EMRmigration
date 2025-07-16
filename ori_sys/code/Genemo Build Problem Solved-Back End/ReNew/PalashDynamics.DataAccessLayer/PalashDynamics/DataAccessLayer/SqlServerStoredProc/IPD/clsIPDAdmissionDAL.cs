namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IPD
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration.IPD;
    using PalashDynamics.ValueObjects.IPD;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;

    public class clsIPDAdmissionDAL : clsBaseIPDAdmissionDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsIPDAdmissionDAL()
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
            }
        }

        private void AddAdmMLDCDetails(clsSaveIPDAdmissionBizActionVO BizActionObj)
        {
            DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddAdmMLCDetails");
            this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, BizActionObj.Details.UnitId);
            this.dbServer.AddInParameter(storedProcCommand, "AdmID", DbType.Int64, BizActionObj.Details.ID);
            this.dbServer.AddInParameter(storedProcCommand, "AdmUnitID", DbType.Int64, BizActionObj.Details.UnitId);
            this.dbServer.AddInParameter(storedProcCommand, "PoliceStation", DbType.String, BizActionObj.AdmMLDCDetails.PoliceStation);
            this.dbServer.AddInParameter(storedProcCommand, "Authority", DbType.String, BizActionObj.AdmMLDCDetails.Authority);
            this.dbServer.AddInParameter(storedProcCommand, "Number", DbType.String, BizActionObj.AdmMLDCDetails.Number);
            this.dbServer.AddInParameter(storedProcCommand, "Address", DbType.String, BizActionObj.AdmMLDCDetails.Address);
            this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, BizActionObj.AdmMLDCDetails.Remark);
            this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, BizActionObj.AdmMLDCDetails.Description);
            this.dbServer.AddInParameter(storedProcCommand, "Title", DbType.String, BizActionObj.AdmMLDCDetails.Title);
            this.dbServer.AddInParameter(storedProcCommand, "AttachedFileName", DbType.String, BizActionObj.AdmMLDCDetails.AttachedFileName);
            this.dbServer.AddInParameter(storedProcCommand, "AttachedFileContent", DbType.Binary, BizActionObj.AdmMLDCDetails.AttachedFileContent);
            this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
            this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
            this.dbServer.ExecuteNonQuery(storedProcCommand);
        }

        private clsAddIPDAdviseDischargeListBizActionVO AddAdviseDischargeList(clsAddIPDAdviseDischargeListBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsIPDAdmissionVO addAdviseDetails = BizActionObj.AddAdviseDetails;
                if (BizActionObj.AddAdviseDList != null)
                {
                    foreach (MasterListItem item in BizActionObj.AddAdviseDList)
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddAdvisedDischargeList");
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "AdmID", DbType.Int64, addAdviseDetails.AdmID);
                        this.dbServer.AddInParameter(storedProcCommand, "AdmUnitID", DbType.Int64, addAdviseDetails.AdmissionUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, item.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, addAdviseDetails.CreatedUnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, addAdviseDetails.AddedBy);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, addAdviseDetails.AddedWindowsLoginName);
                        this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, addAdviseDetails.ID);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                        BizActionObj.AddAdviseDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return BizActionObj;
        }

        public override IValueObject AddAdviseDischargeList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddIPDAdviseDischargeListBizActionVO bizActionObj = valueObject as clsAddIPDAdviseDischargeListBizActionVO;
            bizActionObj = this.AddAdviseDischargeList(bizActionObj, UserVo);
            return valueObject;
        }

        private clsAddDiischargeApprovedByDepartmentBizActionVO AddApprovedAdviseDischarge(clsAddDiischargeApprovedByDepartmentBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                clsDiischargeApprovedByDepartmentVO addAdviseDetails = BizActionObj.AddAdviseDetails;
                if (BizActionObj.AddAdviseList != null)
                {
                    foreach (clsDiischargeApprovedByDepartmentVO tvo in BizActionObj.AddAdviseList)
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddDischargeApprovalByDepartment");
                        this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, tvo.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, tvo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "AdmissionID", DbType.Int64, tvo.AdmissionId);
                        this.dbServer.AddInParameter(storedProcCommand, "AdmissionUnitID", DbType.Int64, tvo.AdmissionUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, tvo.PatientId);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, tvo.PatientUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, tvo.DepartmentID);
                        this.dbServer.AddInParameter(storedProcCommand, "DepartmentName", DbType.String, tvo.DepartmentName);
                        this.dbServer.AddInParameter(storedProcCommand, "AdviseAuthorityName", DbType.String, (tvo.SelectedStaff != null) ? tvo.SelectedStaff.Description : string.Empty);
                        this.dbServer.AddInParameter(storedProcCommand, "LoginUserID", DbType.Int64, tvo.LoginUserID);
                        this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, tvo.Remark);
                        this.dbServer.AddInParameter(storedProcCommand, "ApprovalStatus", DbType.Boolean, tvo.ApprovalStatus);
                        this.dbServer.AddInParameter(storedProcCommand, "ApprovalRemark", DbType.String, tvo.ApprovalRemark);
                        this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(storedProcCommand);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                connection.Close();
                connection = null;
            }
            return BizActionObj;
        }

        private clsSaveIPDAdmissionBizActionVO AddDetails(clsSaveIPDAdmissionBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                clsIPDAdmissionVO details = BizActionObj.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateIPDAdmissionNew");
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, details.Date);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, details.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientId", DbType.Int64, details.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "IPDNO", DbType.String, details.IPDNO);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, details.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, details.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "MLC", DbType.Boolean, details.MLC);
                this.dbServer.AddOutParameter(storedProcCommand, "IPDAdmissionNO", DbType.String, 0x7fffffff);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                BizActionObj.Details.IPDAdmissionNO = Convert.ToString(this.dbServer.GetParameterValue(storedProcCommand, "IPDAdmissionNO"));
                foreach (clsIPDBedMasterVO rvo in BizActionObj.List)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateIPDAdmissionDetailsNew");
                    this.dbServer.AddInParameter(command2, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AdmissionID", DbType.Int64, BizActionObj.Details.ID);
                    this.dbServer.AddInParameter(command2, "PatientSourceID", DbType.Int64, details.PatientSourceID);
                    this.dbServer.AddInParameter(command2, "ReferingEntityID", DbType.Int64, details.RefEntityTypeID);
                    this.dbServer.AddInParameter(command2, "ReferingEntityTypeID", DbType.Int64, details.RefEntityID);
                    this.dbServer.AddInParameter(command2, "AdmissionType", DbType.Int64, details.AdmissionTypeID);
                    this.dbServer.AddInParameter(command2, "ProvDiagnosis", DbType.String, details.ProvisionalDiagnosis);
                    this.dbServer.AddInParameter(command2, "Remark", DbType.String, details.Remarks);
                    this.dbServer.AddInParameter(command2, "BedID", DbType.Int64, rvo.ID);
                    this.dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, details.AddedDateTime);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, details.Status);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateBedTranferNew");
                    this.dbServer.AddInParameter(command3, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command3, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "IPDAdmissionNO", DbType.String, BizActionObj.Details.IPDAdmissionNO);
                    this.dbServer.AddInParameter(command3, "PatientId", DbType.Int64, details.PatientId);
                    this.dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, details.PatientUnitID);
                    this.dbServer.AddInParameter(command3, "IPDAdmissionID", DbType.Int64, BizActionObj.Details.ID);
                    this.dbServer.AddInParameter(command3, "TransferDate", DbType.DateTime, details.AddedDateTime);
                    this.dbServer.AddInParameter(command3, "ToBedCategoryID", DbType.Int64, rvo.BedCategoryID);
                    this.dbServer.AddInParameter(command3, "ToWardID", DbType.Int64, rvo.WardID);
                    this.dbServer.AddInParameter(command3, "ToBedID", DbType.Int64, rvo.ID);
                    this.dbServer.AddInParameter(command3, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, details.AddedDateTime);
                    this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command3, "BillingToBedCategoryID", DbType.Int64, rvo.BillingToBedCategoryID);
                    this.dbServer.ExecuteNonQuery(command3, transaction);
                }
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                string message = exception.Message;
            }
            finally
            {
                transaction.Dispose();
                transaction = null;
            }
            return BizActionObj;
        }

        public override IValueObject AddDischargeApprovalByDepartment(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddDiischargeApprovedByDepartmentBizActionVO bizActionObj = valueObject as clsAddDiischargeApprovedByDepartmentBizActionVO;
            bizActionObj = this.AddApprovedAdviseDischarge(bizActionObj, UserVo);
            return valueObject;
        }

        public override IValueObject AddIPDAdmissionDetailsWithTransaction(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            clsSaveIPDAdmissionBizActionVO nvo = (clsSaveIPDAdmissionBizActionVO) valueObject;
            try
            {
                if (pConnection == null)
                {
                    pConnection = this.dbServer.CreateConnection();
                }
                if (pConnection.State != ConnectionState.Open)
                {
                    pConnection.Open();
                }
                if (pTransaction == null)
                {
                    pTransaction = pConnection.BeginTransaction();
                }
                clsIPDAdmissionVO details = nvo.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateIPDAdmissionNew");
                storedProcCommand.Connection = pConnection;
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, details.Date);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, details.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientId", DbType.Int64, details.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "IPDNO", DbType.String, details.IPDNO);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, details.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, details.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "MLC", DbType.Boolean, details.MLC);
                this.dbServer.AddOutParameter(storedProcCommand, "IPDAdmissionNO", DbType.String, 0x7fffffff);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, pTransaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.Details.IPDAdmissionNO = Convert.ToString(this.dbServer.GetParameterValue(storedProcCommand, "IPDAdmissionNO"));
                foreach (clsIPDBedMasterVO rvo in nvo.List)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateIPDAdmissionDetailsNew");
                    command2.Connection = pConnection;
                    this.dbServer.AddInParameter(command2, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AdmissionID", DbType.Int64, nvo.Details.ID);
                    this.dbServer.AddInParameter(command2, "PatientSourceID", DbType.Int64, details.PatientSourceID);
                    this.dbServer.AddInParameter(command2, "ReferingEntityID", DbType.Int64, details.RefEntityTypeID);
                    this.dbServer.AddInParameter(command2, "ReferingEntityTypeID", DbType.Int64, details.RefEntityID);
                    this.dbServer.AddInParameter(command2, "AdmissionType", DbType.Int64, details.AdmissionTypeID);
                    this.dbServer.AddInParameter(command2, "ProvDiagnosis", DbType.String, details.ProvisionalDiagnosis);
                    this.dbServer.AddInParameter(command2, "Remark", DbType.String, details.Remarks);
                    this.dbServer.AddInParameter(command2, "BedID", DbType.Int64, rvo.ID);
                    this.dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, details.AddedDateTime);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, details.Status);
                    this.dbServer.ExecuteNonQuery(command2, pTransaction);
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateBedTranferNew");
                    command3.Connection = pConnection;
                    this.dbServer.AddInParameter(command3, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command3, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "IPDAdmissionNO", DbType.String, nvo.Details.IPDAdmissionNO);
                    this.dbServer.AddInParameter(command3, "PatientId", DbType.Int64, details.PatientId);
                    this.dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, details.PatientUnitID);
                    this.dbServer.AddInParameter(command3, "IPDAdmissionID", DbType.Int64, nvo.Details.ID);
                    this.dbServer.AddInParameter(command3, "TransferDate", DbType.DateTime, details.AddedDateTime);
                    this.dbServer.AddInParameter(command3, "ToBedCategoryID", DbType.Int64, rvo.BedCategoryID);
                    this.dbServer.AddInParameter(command3, "ToWardID", DbType.Int64, rvo.WardID);
                    this.dbServer.AddInParameter(command3, "ToBedID", DbType.Int64, rvo.ID);
                    this.dbServer.AddInParameter(command3, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, details.AddedDateTime);
                    this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command3, "BillingToBedCategoryID", DbType.Int64, rvo.BillingToBedCategoryID);
                    this.dbServer.ExecuteNonQuery(command3, pTransaction);
                }
            }
            catch (Exception exception)
            {
                nvo.SuccessStatus = -1;
                throw exception;
            }
            return nvo;
        }

        public override IValueObject AddRefEntityTransfer(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddRefEntityDetailsBizActionVO nvo = valueObject as clsAddRefEntityDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("AddRefEntityDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, 0);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.Details.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AdmissionID", DbType.Int64, nvo.Details.AdmID);
                this.dbServer.AddInParameter(storedProcCommand, "AdmissionUnitID", DbType.Int64, nvo.Details.AdmissionUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "RefEntityID", DbType.Int64, nvo.Details.RefEntityID);
                this.dbServer.AddInParameter(storedProcCommand, "RefEntityTypeID", DbType.Int64, nvo.Details.RefEntityTypeID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception exception1)
            {
                string message = exception1.Message;
            }
            return nvo;
        }

        public override IValueObject CancelIPDAdmission(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCancelIPDAdmissionBizactionVO nvo = valueObject as clsCancelIPDAdmissionBizactionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetChargesByAdmIDAndAdmUnitID");
                this.dbServer.AddInParameter(storedProcCommand, "AdmID", DbType.Int64, nvo.AdmissionID);
                this.dbServer.AddInParameter(storedProcCommand, "AdmUnitID", DbType.Int64, nvo.AdmissionUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.AdmissionDetailsList == null)
                    {
                        nvo.AdmissionDetailsList = new List<clsIPDAdmissionVO>();
                    }
                    while (reader.Read())
                    {
                        clsIPDAdmissionVO item = new clsIPDAdmissionVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_UnitId"])),
                            IsBilled = (bool) DALHelper.HandleDBNull(reader["IsBilled"]),
                            IsCancelled = (bool) DALHelper.HandleDBNull(reader["IsCancelled"])
                        };
                        nvo.AdmissionDetailsList.Add(item);
                    }
                }
                reader.NextResult();
                while (true)
                {
                    if (!reader.Read())
                    {
                        if (nvo.AdmissionDetailsList != null)
                        {
                            if ((from S in nvo.AdmissionDetailsList
                                where S.IsBilled.Equals(true)
                                select S).Count<clsIPDAdmissionVO>() > 0)
                            {
                                nvo.SuccessStatus = 1;
                            }
                            else
                            {
                                nvo.SuccessStatus = ((from S in nvo.AdmissionDetailsList
                                    where S.IsBilled.Equals(false) && S.IsCancelled.Equals(false)
                                    select S).Count<clsIPDAdmissionVO>() <= 0) ? 3 : 2;
                            }
                        }
                        reader.NextResult();
                        reader.Close();
                        break;
                    }
                    clsIPDAdmissionVO item = new clsIPDAdmissionVO {
                        ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"])),
                        UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_UnitId"])),
                        IsBilled = (bool) DALHelper.HandleDBNull(reader["IsBilled"])
                    };
                    nvo.AdmissionDetailsList.Add(item);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject CheckIPDRoundExitsOrNot(IValueObject valueObject, clsUserVO UserVo)
        {
            ClsIPDCheckRoundExistsBizactionVO nvo = valueObject as ClsIPDCheckRoundExistsBizactionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CheckIPDRoundExists");
                this.dbServer.AddInParameter(storedProcCommand, "PatientId", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitId);
                this.dbServer.AddInParameter(storedProcCommand, "IsOpdIpd", DbType.Boolean, nvo.IsOpdIpd);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorId", DbType.Int64, nvo.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                nvo.status = Convert.ToInt64(this.dbServer.ExecuteScalar(storedProcCommand));
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject Get(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIPDAdmissionBizActionVO nvo = valueObject as clsGetIPDAdmissionBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIPDAdmission");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    nvo.Details = new clsIPDAdmissionVO();
                    while (reader.Read())
                    {
                        DateTime? nullable = DALHelper.HandleDate(reader["Date"]);
                        nvo.Details.Date = new DateTime?(nullable.Value);
                        DateTime? nullable2 = DALHelper.HandleDate(reader["Time"]);
                        nvo.Details.Time = nullable2.Value;
                        nvo.Details.PatientId = (long) DALHelper.HandleDBNull(reader["PatientId"]);
                        nvo.Details.PatientUnitID = (long) DALHelper.HandleDBNull(reader["PatientUnitID"]);
                        nvo.Details.AdmissionTypeID = (long) DALHelper.HandleDBNull(reader["AdmissionTypeID"]);
                        nvo.Details.DepartmentID = (long) DALHelper.HandleDBNull(reader["DepartmentID"]);
                        nvo.Details.DoctorID = (long) DALHelper.HandleDBNull(reader["DoctorID"]);
                        nvo.Details.RefferedDoctor = (string) DALHelper.HandleDBNull(reader["RefferedDoctor"]);
                        nvo.Details.MLC = (bool) DALHelper.HandleDBNull(reader["MLC"]);
                        nvo.Details.BedCategoryID = (long) DALHelper.HandleDBNull(reader["BedCategoryID"]);
                        nvo.Details.WardID = (long) DALHelper.HandleDBNull(reader["WardID"]);
                        nvo.Details.BedID = (long) DALHelper.HandleDBNull(reader["BedID"]);
                        nvo.Details.KinRelationID = (long) DALHelper.HandleDBNull(reader["KinRelationID"]);
                        nvo.Details.KinAddress = (string) DALHelper.HandleDBNull(reader["KinAddress"]);
                        nvo.Details.kinPhone = (string) DALHelper.HandleDBNull(reader["kinPhone"]);
                        nvo.Details.KinMobile = (string) DALHelper.HandleDBNull(reader["KinMobile"]);
                        nvo.Details.Doctor1_ID = (long) DALHelper.HandleDBNull(reader["Doctor1_ID"]);
                        nvo.Details.Doctor2_ID = (long) DALHelper.HandleDBNull(reader["Doctor2_ID"]);
                        nvo.Details.Remarks = (string) DALHelper.HandleDBNull(reader["KinMobile"]);
                        nvo.Details.UnitId = (long) DALHelper.HandleDBNull(reader["UnitId"]);
                        nvo.Details.CreatedUnitId = (long) DALHelper.HandleDBNull(reader["CreatedUnitId"]);
                        nvo.Details.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                        nvo.Details.AddedBy = (long?) DALHelper.HandleDBNull(reader["AddedBy"]);
                        nvo.Details.AddedOn = (string) DALHelper.HandleDBNull(reader["AddedOn"]);
                        nvo.Details.AddedDateTime = DALHelper.HandleDate(reader["AddedDateTime"]);
                        nvo.Details.AddedWindowsLoginName = (string) DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]);
                        nvo.Details.UpdatedUnitId = (long?) DALHelper.HandleDBNull(reader["UpdatedUnitID"]);
                        nvo.Details.UpdatedBy = (long?) DALHelper.HandleDBNull(reader["UpdatedBy"]);
                        nvo.Details.UpdatedOn = (string) DALHelper.HandleDBNull(reader["UpdatedOn"]);
                        nvo.Details.UpdatedDateTime = DALHelper.HandleDate(reader["UpdatedDateTime"]);
                        nvo.Details.UpdatedWindowsLoginName = (string) DALHelper.HandleDBNull(reader["UpdatedWindowsLoginName"]);
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                string message = exception1.Message;
            }
            return valueObject;
        }

        public override IValueObject GetActiveAdmissionOfRegisteredPatient(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetActiveAdmissionBizActionVO nvo = valueObject as clsGetActiveAdmissionBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetActiveAdmissionOfRegisteredPatient");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, nvo.MRNo);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.Details = new clsIPDAdmissionVO();
                        nvo.Details.AdmID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.Details.AdmissionUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                    }
                }
                reader.NextResult();
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetAdmissionList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIPDAdmissionListBizActionVO nvo = valueObject as clsGetIPDAdmissionListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientAdmissionList");
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.AdmDetails.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.AdmDetails.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "PatientId", DbType.Int64, nvo.AdmDetails.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.AdmDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.AdmDetails.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.AdmDetails.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "BedCategoryID", DbType.Int64, nvo.AdmDetails.BedCategoryID);
                this.dbServer.AddInParameter(storedProcCommand, "WardID", DbType.Int64, nvo.AdmDetails.WardID);
                this.dbServer.AddInParameter(storedProcCommand, "strWard", DbType.String, nvo.AdmDetails.strWard);
                this.dbServer.AddInParameter(storedProcCommand, "MrNo", DbType.String, nvo.AdmDetails.MRNo);
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, nvo.AdmDetails.FirstName);
                this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, nvo.AdmDetails.LastName);
                this.dbServer.AddInParameter(storedProcCommand, "OldRegistrationNo", DbType.String, nvo.AdmDetails.OldRegistrationNo);
                if (!nvo.AdmDetails.IsAllPatient.Equals(true))
                {
                    if (nvo.AdmDetails.CurrentAdmittedList.Equals(true))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "IsDischarged", DbType.String, "false");
                        this.dbServer.AddInParameter(storedProcCommand, "IsCancel", DbType.String, "false");
                        this.dbServer.AddInParameter(storedProcCommand, "IsClosed", DbType.String, "false");
                    }
                    if (nvo.AdmDetails.IsCancelled.Equals(true))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "IsCancel", DbType.String, "true");
                    }
                    if (nvo.AdmDetails.IsMedicoLegalCase.Equals(true))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "IsMedicoLegalCase", DbType.String, "true");
                    }
                    if (nvo.AdmDetails.IsNonPresence.Equals(true))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "IsNonPresence", DbType.String, "true");
                    }
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnRegistered", DbType.Boolean, nvo.AdmDetails.UnRegistered);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.AdmList == null)
                    {
                        nvo.AdmList = new List<clsIPDAdmissionVO>();
                    }
                    while (reader.Read())
                    {
                        clsIPDAdmissionVO item = new clsIPDAdmissionVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            MRNo = (string) DALHelper.HandleDBNull(reader["MRNo"]),
                            FirstName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["FirstName"])),
                            LastName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["LastName"])),
                            Number = (string) DALHelper.HandleDBNull(reader["ContactNo1"]),
                            IPDNO = (string) DALHelper.HandleDBNull(reader["IPDNO"])
                        };
                        string[] strArray = new string[] { Convert.ToString(DALHelper.HandleDBNull(reader["Preffix"])), " ", item.FirstName, " ", item.LastName };
                        item.PatientName = string.Concat(strArray);
                        item.GenderName = (string) DALHelper.HandleDBNull(reader["Gender"]);
                        item.DateOfBirth = DALHelper.HandleDate(reader["DateOfBirth"]);
                        item.AdmissionDate = DALHelper.HandleDate(reader["AdmissionDate"]);
                        item.DFName = (string) DALHelper.HandleDBNull(reader["DFName"]);
                        item.DLName = (string) DALHelper.HandleDBNull(reader["DLName"]);
                        item.DoctorName = item.DFName + " " + item.DLName;
                        item.PatientId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        item.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        item.RefEntityTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferingEntityTypeID"]));
                        item.RefEntityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferingEntityID"]));
                        item.RefDoctor = (string) DALHelper.HandleDBNull(reader["RefTypeDesc"]);
                        item.CompanyName = (string) DALHelper.HandleDBNull(reader["CompanyName"]);
                        item.Bed = (string) DALHelper.HandleDBNull(reader["Bed"]);
                        item.Ward = (string) DALHelper.HandleDBNull(reader["Ward"]);
                        item.AdmissionTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionType"]));
                        item.IsCancelled = (bool) DALHelper.HandleDBNull(reader["IsCancel"]);
                        item.IsClosed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsClosed"]));
                        item.IsDischarge = (bool) DALHelper.HandleDBNull(reader["ISDischarged"]);
                        item.PatientSourceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceID"]));
                        item.classID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedCategoryId"]));
                        item.BillCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillCount"]));
                        item.UnfreezedBillCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnfreezedBillCount"]));
                        item.CompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompanyID"]));
                        item.OldRegistrationNo = Convert.ToString(DALHelper.HandleDBNull(reader["OldRegistrationNo"]));
                        item.DischargeDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["DischargeDate"])));
                        DateTime? dischargeDate = item.DischargeDate;
                        if (dischargeDate.ToString() == "1/1/0001 12:00:00 AM")
                        {
                            item.DischargeDate = null;
                        }
                        else if (item.DischargeDate.ToString() == "1/1/1900 12:00:00 AM")
                        {
                            item.DischargeDate = null;
                        }
                        item.Balance = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Balance"]));
                        item.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                        item.PatientCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientCategoryID"]));
                        item.BabyWeight = Convert.ToString(DALHelper.HandleDBNull(reader["BabyWeight"]));
                        item.GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]));
                        item.TariffID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"])));
                        nvo.AdmList.Add(item);
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

        public override IValueObject GetAdvisedDischargeByAdmIdAndAdmUnitID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetAdvisedDischargeByAdmIdAndAdmUnitIDBizActionVO nvo = valueObject as clsGetAdvisedDischargeByAdmIdAndAdmUnitIDBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAdvisedDischargeByAdmIdAndAdmUnitID");
                this.dbServer.AddInParameter(storedProcCommand, "AdmID", DbType.Int64, nvo.GetAdviseDetails.AdmID);
                this.dbServer.AddInParameter(storedProcCommand, "AdmUnitID", DbType.Int64, nvo.GetAdviseDetails.AdmissionUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.GetAdviseDList == null)
                    {
                        nvo.GetAdviseDList = new List<clsIPDAdmissionVO>();
                    }
                    while (reader.Read())
                    {
                        clsIPDAdmissionVO item = new clsIPDAdmissionVO {
                            AdmID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmID"])),
                            AdmissionUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmUnitID"])),
                            DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"])),
                            Status = (bool) DALHelper.HandleDBNull(reader["ConfirmationStatus"])
                        };
                        nvo.GetAdviseDList.Add(item);
                    }
                }
                reader.NextResult();
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetAdviseDischargeListForApproval(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetListOfAdviseDischargeForApprovalBizActionVO nvo = valueObject as clsGetListOfAdviseDischargeForApprovalBizActionVO;
            nvo.AddAdviseList = new List<clsDiischargeApprovedByDepartmentVO>();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("GetListOfDischargeForApproval");
                this.dbServer.AddInParameter(storedProcCommand, "AdmissionID", DbType.Int64, nvo.AddAdviseDetails.AdmissionId);
                this.dbServer.AddInParameter(storedProcCommand, "AdmissionUnitID", DbType.Int64, nvo.AddAdviseDetails.AdmissionUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    nvo.AddAdviseList = new List<clsDiischargeApprovedByDepartmentVO>();
                    while (reader.Read())
                    {
                        clsDiischargeApprovedByDepartmentVO item = new clsDiischargeApprovedByDepartmentVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"])),
                            DepartmentName = Convert.ToString(DALHelper.HandleDBNull(reader["DepartmentName"])),
                            AdmissionId = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionID"])),
                            AdmissionUnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["AdmissionUnitID"])),
                            PatientId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"])),
                            AdviseAuthorityName = Convert.ToString(DALHelper.HandleDBNull(reader["AdviseAuthorityName"])),
                            Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"])),
                            ApprovalRemark = Convert.ToString(DALHelper.HandleDBNull(reader["ApprovalRemark"])),
                            ApprovalStatus = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ApprovalStatus"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        if (UserVo.UserGeneralDetailVO.DepartmentID == item.DepartmentID)
                        {
                            item.IsEnable = true;
                        }
                        if (item.ApprovalStatus)
                        {
                            item.IsEnable = false;
                        }
                        nvo.AddAdviseList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                string message = exception1.Message;
            }
            return nvo;
        }

        public override IValueObject GetConsentByTempleteID(IValueObject valueObject, clsUserVO UserVo)
        {
            throw new NotImplementedException();
        }

        public override IValueObject GetDoctorDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIPDRoundDoctorBizactionVO nvo = valueObject as clsIPDRoundDoctorBizactionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorDetails");
                this.dbServer.AddInParameter(storedProcCommand, "DoctorId", DbType.Int64, nvo.DoctorId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]));
                        nvo.SpecCode = Convert.ToString(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.SpecName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetMedicoLegalCase(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIPDAdmissionListBizActionVO nvo = valueObject as clsGetIPDAdmissionListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMedicoLegalCaseDetailsByAdmID");
                this.dbServer.AddInParameter(storedProcCommand, "AdmID", DbType.Int64, nvo.AdmDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AdmUnitID", DbType.Int64, nvo.AdmDetails.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.AdmMLDCDetails == null)
                    {
                        nvo.AdmMLDCDetails = new clsIPDAdmMLCDetailsVO();
                    }
                    while (reader.Read())
                    {
                        clsIPDAdmMLCDetailsVO svo = new clsIPDAdmMLCDetailsVO {
                            PoliceStation = Convert.ToString(DALHelper.HandleDBNull(reader["PoliceStation"])),
                            IsMLC = Convert.ToBoolean(DALHelper.HandleDBNull(reader["MLC"])),
                            Number = Convert.ToString(DALHelper.HandleDBNull(reader["Number"])),
                            Authority = Convert.ToString(DALHelper.HandleDBNull(reader["Authority"])),
                            Address = Convert.ToString(DALHelper.HandleDBNull(reader["Address"])),
                            Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Title = Convert.ToString(DALHelper.HandleDBNull(reader["Title"])),
                            AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["MLCFileName"])),
                            AttachedFileContent = (byte[]) DALHelper.HandleDBNull(reader["MLCFile"])
                        };
                        nvo.AdmMLDCDetails = svo;
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

        public override IValueObject GetPatientDischargeApprovalList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIPDAdmissionListBizActionVO nvo = valueObject as clsGetIPDAdmissionListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientDischargeApprovalList");
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.AdmDetails.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.AdmDetails.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "PatientId", DbType.Int64, nvo.AdmDetails.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.AdmDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.AdmDetails.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "BedCategoryID", DbType.Int64, nvo.AdmDetails.BedCategoryID);
                this.dbServer.AddInParameter(storedProcCommand, "WardID", DbType.Int64, nvo.AdmDetails.WardID);
                this.dbServer.AddInParameter(storedProcCommand, "MrNo", DbType.String, nvo.AdmDetails.MRNo);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.AdmList == null)
                    {
                        nvo.AdmList = new List<clsIPDAdmissionVO>();
                    }
                    while (reader.Read())
                    {
                        clsIPDAdmissionVO item = new clsIPDAdmissionVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            MRNo = (string) DALHelper.HandleDBNull(reader["MRNo"]),
                            FirstName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["PatientName"])),
                            Number = (string) DALHelper.HandleDBNull(reader["ContactNo1"]),
                            IPDNO = (string) DALHelper.HandleDBNull(reader["IPDNO"]),
                            PatientName = (string) DALHelper.HandleDBNull(reader["PatientName"]),
                            GenderName = (string) DALHelper.HandleDBNull(reader["Gender"]),
                            DateOfBirth = DALHelper.HandleDate(reader["DateOfBirth"]),
                            AdmissionDate = DALHelper.HandleDate(reader["AdmissionDate"]),
                            DFName = (string) DALHelper.HandleDBNull(reader["DFName"]),
                            DLName = (string) DALHelper.HandleDBNull(reader["DLName"])
                        };
                        item.DoctorName = "Dr " + item.DFName + " " + item.DLName;
                        item.PatientId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        item.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        item.RefEntityTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferingEntityTypeID"]));
                        item.RefEntityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferingEntityID"]));
                        item.RefDoctor = (string) DALHelper.HandleDBNull(reader["RefTypeDesc"]);
                        item.CompanyName = (string) DALHelper.HandleDBNull(reader["CompanyName"]);
                        item.Bed = (string) DALHelper.HandleDBNull(reader["Bed"]);
                        item.Ward = (string) DALHelper.HandleDBNull(reader["Ward"]);
                        item.AdmissionTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionType"]));
                        item.IsCancelled = (bool) DALHelper.HandleDBNull(reader["IsCancel"]);
                        item.IsClosed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsClosed"]));
                        item.IsDischarge = (bool) DALHelper.HandleDBNull(reader["ISDischarged"]);
                        item.PatientSourceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceID"]));
                        item.classID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedCategoryId"]));
                        item.BedClass = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]));
                        item.CompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompanyID"]));
                        nvo.AdmList.Add(item);
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

        public override IValueObject GetRefEntityTransfer(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRefEntityDetailsBizActionVO nvo = valueObject as clsGetRefEntityDetailsBizActionVO;
            nvo.List = new List<clsRefEntityDetailsVO>();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("GetRefEntityDetails");
                this.dbServer.AddInParameter(storedProcCommand, "AdmissionID", DbType.Int64, nvo.Details.AdmID);
                this.dbServer.AddInParameter(storedProcCommand, "AdmissionUnitID", DbType.Int64, nvo.Details.AdmissionUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    nvo.List = new List<clsRefEntityDetailsVO>();
                    while (reader.Read())
                    {
                        clsRefEntityDetailsVO item = new clsRefEntityDetailsVO {
                            RefEntityIDDesc = Convert.ToString(DALHelper.HandleDBNull(reader["RefTypeDesc"])),
                            RefEntityTypeIDDesc = Convert.ToString(DALHelper.HandleDBNull(reader["RefEntityTypeDesc"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.List.Add(item);
                    }
                }
                reader.NextResult();
                reader.Close();
            }
            catch (Exception exception1)
            {
                string message = exception1.Message;
            }
            return nvo;
        }

        public override IValueObject Save(IValueObject valueObject, clsUserVO UserVo)
        {
            clsSaveIPDAdmissionBizActionVO bizActionObj = valueObject as clsSaveIPDAdmissionBizActionVO;
            bizActionObj = (bizActionObj.Details.ID != 0L) ? this.UpdateDetails(bizActionObj, UserVo) : this.AddDetails(bizActionObj, UserVo);
            return valueObject;
        }

        public override IValueObject SaveRoundTrip(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIPDRoundDetailsBizactionVO nvo = valueObject as clsIPDRoundDetailsBizactionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddIPDRound");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorId);
                this.dbServer.AddInParameter(storedProcCommand, "AdmissionId", DbType.Int64, nvo.AdmisstionId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.Output, null, DataRowVersion.Default, nvo.ID);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.ID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject UpdateAdmissionType(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateAdmissionTypeBizActionVO nvo = valueObject as clsUpdateAdmissionTypeBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateAdmtype");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.AdmID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.AdmUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AdmissionType", DbType.Int64, nvo.AdmTypeID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, nvo.UpdateAdmType.UpdatedUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, nvo.UpdateAdmType.UpdatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, nvo.UpdateAdmType.UpdatedOn);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, nvo.UpdateAdmType.UpdatedWindowsLoginName);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception exception1)
            {
                string message = exception1.Message;
            }
            return nvo;
        }

        public override IValueObject UpdateCancelIPDAdmission(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCancelIPDAdmissionBizactionVO nvo = valueObject as clsCancelIPDAdmissionBizactionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateAdmissionDetailsByAdmIDAndAdmUnitIDForCancel");
                this.dbServer.AddInParameter(storedProcCommand, "AdmID", DbType.Int64, nvo.AdmissionID);
                this.dbServer.AddInParameter(storedProcCommand, "AdmUnitID", DbType.Int64, nvo.AdmissionUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsCancel", DbType.Boolean, 1);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        private clsSaveIPDAdmissionBizActionVO UpdateDetails(clsSaveIPDAdmissionBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsIPDAdmissionVO details = BizActionObj.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateIPDAdmissionNew");
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, details.Date);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, details.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientId", DbType.Int64, details.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "IPDNO", DbType.String, details.IPDNO);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, details.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, details.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "MLC", DbType.Boolean, BizActionObj.IsMedicoLegalCase);
                this.dbServer.AddOutParameter(storedProcCommand, "IPDAdmissionNO", DbType.String, 0x7fffffff);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                BizActionObj.Details.IPDAdmissionNO = Convert.ToString(this.dbServer.GetParameterValue(storedProcCommand, "IPDAdmissionNO"));
                if (BizActionObj.IsMedicoLegalCase)
                {
                    this.AddAdmMLDCDetails(BizActionObj);
                }
            }
            catch (Exception exception1)
            {
                string message = exception1.Message;
            }
            return BizActionObj;
        }

        public override IValueObject UpdateDischargeApproval(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddDiischargeApprovedByDepartmentBizActionVO nvo = valueObject as clsAddDiischargeApprovedByDepartmentBizActionVO;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                clsDiischargeApprovedByDepartmentVO addAdviseDetails = nvo.AddAdviseDetails;
                if (nvo.AddAdviseList != null)
                {
                    foreach (clsDiischargeApprovedByDepartmentVO tvo in nvo.AddAdviseList)
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateDischargeApproval");
                        this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, tvo.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, tvo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "AdmissionID", DbType.Int64, tvo.AdmissionId);
                        this.dbServer.AddInParameter(storedProcCommand, "AdmissionUnitID", DbType.Int64, tvo.AdmissionUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, tvo.PatientId);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, tvo.PatientUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, tvo.DepartmentID);
                        this.dbServer.AddInParameter(storedProcCommand, "ApprovalStatus", DbType.Boolean, tvo.ApprovalStatus);
                        this.dbServer.AddInParameter(storedProcCommand, "ApprovalRemark", DbType.String, tvo.ApprovalRemark);
                        this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(storedProcCommand);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                connection.Close();
                connection = null;
            }
            return nvo;
        }
    }
}

