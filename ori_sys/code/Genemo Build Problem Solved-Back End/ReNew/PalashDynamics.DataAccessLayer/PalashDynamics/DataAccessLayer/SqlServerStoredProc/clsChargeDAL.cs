namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration;
    using PalashDynamics.ValueObjects.Billing;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;

    public class clsChargeDAL : clsBaseChargeDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsChargeDAL()
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
            catch (Exception exception1)
            {
                string message = exception1.Message;
                throw;
            }
        }

        public override IValueObject Add(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddChargeBizActionVO bizActionObj = valueObject as clsAddChargeBizActionVO;
            bizActionObj = this.AddDetails(bizActionObj, UserVo, null, null, 0L, 0L);
            return valueObject;
        }

        public override IValueObject Add(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction, long iParentID, long iCDParentID)
        {
            clsAddChargeBizActionVO bizActionObj = valueObject as clsAddChargeBizActionVO;
            bizActionObj = this.AddDetails(bizActionObj, UserVo, pConnection, pTransaction, iParentID, iCDParentID);
            return valueObject;
        }

        private clsAddChargeBizActionVO AddDetails(clsAddChargeBizActionVO BizActionObj, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction, long iParentID, long iCDParentID)
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
                clsChargeVO details = BizActionObj.Details;
                details.ChargeDetails = new clsChargeDetailsVO();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddCharge");
                this.dbServer.AddInParameter(storedProcCommand, "ParentID", DbType.Int64, details.ParentID);
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, details.LinkServer);
                if (details.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, details.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, details.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External_Id", DbType.Int64, details.Opd_Ipd_External_Id);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External_UnitId", DbType.Int64, details.Opd_Ipd_External_UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ClassId", DbType.Int64, details.ClassId);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External", DbType.Int16, details.Opd_Ipd_External);
                this.dbServer.AddInParameter(storedProcCommand, "TariffServiceId", DbType.Int64, details.TariffServiceId);
                this.dbServer.AddInParameter(storedProcCommand, "TariffId", DbType.Int64, details.TariffId);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceId", DbType.Int64, details.ServiceId);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceNameNew", DbType.String, details.ServiceName);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceCode", DbType.String, details.ServiceCode);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorId", DbType.Int64, details.SelectedDoctor.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Rate", DbType.Double, details.Rate);
                this.dbServer.AddInParameter(storedProcCommand, "Quantity", DbType.Double, details.Quantity);
                this.dbServer.AddInParameter(storedProcCommand, "TotalAmount", DbType.Double, details.TotalAmount);
                this.dbServer.AddInParameter(storedProcCommand, "Discount", DbType.Double, details.Discount);
                this.dbServer.AddInParameter(storedProcCommand, "StaffFree", DbType.Double, details.StaffFree);
                this.dbServer.AddInParameter(storedProcCommand, "NetAmount", DbType.Double, details.NetAmount);
                this.dbServer.AddInParameter(storedProcCommand, "PaidAmount", DbType.Double, details.ServicePaidAmount);
                this.dbServer.AddInParameter(storedProcCommand, "HospShareAmount", DbType.Double, details.HospShareAmount);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorShareAmount", DbType.Double, details.DoctorShareAmount);
                this.dbServer.AddInParameter(storedProcCommand, "isPackageService", DbType.Boolean, details.isPackageService);
                this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int64, details.PackageID);
                this.dbServer.AddInParameter(storedProcCommand, "ChargeIDPackage", DbType.Int64, details.ChargeIDPackage);
                this.dbServer.AddInParameter(storedProcCommand, "Emergency", DbType.Boolean, details.Emergency);
                this.dbServer.AddInParameter(storedProcCommand, "SponsorType", DbType.Boolean, details.SponsorType);
                this.dbServer.AddInParameter(storedProcCommand, "IsBilled", DbType.Boolean, details.IsBilled);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceSpecilizationID", DbType.Int64, details.ServiceSpecilizationID);
                this.dbServer.AddInParameter(storedProcCommand, "ConcessionPercent", DbType.Double, details.ConcessionPercent);
                this.dbServer.AddInParameter(storedProcCommand, "ConcessionAmount", DbType.Double, details.Concession);
                this.dbServer.AddInParameter(storedProcCommand, "StaffDiscountPercent", DbType.Double, details.StaffDiscountPercent);
                this.dbServer.AddInParameter(storedProcCommand, "StaffDiscountAmount", DbType.Double, details.StaffDiscountAmount);
                this.dbServer.AddInParameter(storedProcCommand, "StaffParentDiscountPercent", DbType.Double, details.StaffParentDiscountPercent);
                this.dbServer.AddInParameter(storedProcCommand, "StaffParentDiscountAmount", DbType.Double, details.StaffParentDiscountAmount);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceTaxPercent", DbType.Double, 0);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceTaxAmount", DbType.Double, 0);
                this.dbServer.AddInParameter(storedProcCommand, "GrossDiscountReason", DbType.Int64, details.GrossDiscountReasonID);
                this.dbServer.AddInParameter(storedProcCommand, "GrossDiscountPercentage", DbType.Double, details.GrossDiscountPercentage);
                this.dbServer.AddInParameter(storedProcCommand, "CostingDivisionID", DbType.Int64, details.CostingDivisionID);
                this.dbServer.AddInParameter(storedProcCommand, "IsTemp", DbType.Boolean, details.IsTemp);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, details.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, details.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "IsAutoCharge", DbType.Boolean, false);
                this.dbServer.AddInParameter(storedProcCommand, "ConditionTypeID", DbType.Int64, details.ConditionTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "InitialRate", DbType.Int64, details.InitialRate);
                this.dbServer.AddInParameter(storedProcCommand, "TotalServiceTaxAmount", DbType.Double, details.TotalServiceTaxAmount);
                this.dbServer.AddInParameter(storedProcCommand, "IsPackageConsumption", DbType.Boolean, details.IsPackageConsumption);
                this.dbServer.AddInParameter(storedProcCommand, "ProcessID", DbType.Int64, details.ProcessID);
                this.dbServer.AddInParameter(storedProcCommand, "PackageConcessionPercent", DbType.Double, details.PackageConcessionPercent);
                this.dbServer.AddInParameter(storedProcCommand, "PackageConcessionAmount", DbType.Double, details.PackageConcession);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, details.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if (!BizActionObj.FromVisit)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddChargeDetails");
                    this.dbServer.AddInParameter(command2, "ChargeID", DbType.Int64, BizActionObj.Details.ID);
                    this.dbServer.AddInParameter(command2, "ChargeUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "ParentID", DbType.Int64, iCDParentID);
                    this.dbServer.AddInParameter(command2, "Date", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command2, "Quantity", DbType.Double, details.Quantity);
                    this.dbServer.AddInParameter(command2, "Rate", DbType.Double, details.Rate);
                    this.dbServer.AddInParameter(command2, "TotalAmount", DbType.Double, details.TotalAmount);
                    this.dbServer.AddInParameter(command2, "ConcessionAmount", DbType.Double, details.Concession);
                    this.dbServer.AddInParameter(command2, "ServiceTaxAmount", DbType.Double, details.ServiceTaxAmount);
                    this.dbServer.AddInParameter(command2, "NetAmount", DbType.Double, details.NetAmount);
                    this.dbServer.AddInParameter(command2, "PaidAmount", DbType.Double, details.ServicePaidAmount);
                    if (details.IsBilled)
                    {
                        this.dbServer.AddInParameter(command2, "BalanceAmount", DbType.Double, details.BalanceAmount);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(command2, "BalanceAmount", DbType.Double, details.NetAmount);
                    }
                    this.dbServer.AddInParameter(command2, "RefundID", DbType.Int64, details.RefundID);
                    this.dbServer.AddInParameter(command2, "RefundAmount", DbType.Double, details.RefundAmount);
                    this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, details.Status);
                    this.dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, details.AddedDateTime);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, details.ChargeDetails.ID);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddInParameter(command2, "IsFromApprovedRequest", DbType.Boolean, details.IsFromApprovedRequest);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                    BizActionObj.Details.ChargeDetails.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                }
                if ((details.ChargeTaxDetailsList != null) && (details.ChargeTaxDetailsList.Count > 0))
                {
                    foreach (clsChargeTaxDetailsVO svo in details.ChargeTaxDetailsList)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateChargeTaxDetails");
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "ChargeID", DbType.Int64, BizActionObj.Details.ID);
                        this.dbServer.AddInParameter(command3, "ChargeUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "TaxID", DbType.Int64, svo.TaxID);
                        this.dbServer.AddInParameter(command3, "TaxPercentage", DbType.Double, svo.ServiceTaxPercent);
                        this.dbServer.AddInParameter(command3, "TaxType", DbType.Int32, svo.TaxType);
                        this.dbServer.AddInParameter(command3, "TaxAmount", DbType.Double, svo.ServiceTaxAmount);
                        this.dbServer.AddInParameter(command3, "IsTaxLimitApplicable", DbType.Boolean, svo.IsTaxLimitApplicable);
                        this.dbServer.AddInParameter(command3, "TaxLimit", DbType.Decimal, svo.TaxLimit);
                        this.dbServer.AddInParameter(command3, "Date", DbType.DateTime, svo.Date);
                        this.dbServer.AddInParameter(command3, "Rate", DbType.Double, svo.Rate);
                        this.dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, details.AddedDateTime);
                        this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddOutParameter(command3, "ReasultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                        Convert.ToInt32(this.dbServer.GetParameterValue(command3, "ReasultStatus"));
                    }
                }
                BizActionObj.SuccessStatus = 0;
                if (pConnection == null)
                {
                    transaction.Commit();
                }
            }
            catch (Exception exception1)
            {
                string message = exception1.Message;
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

        public override IValueObject AddRefundServices(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            clsAddRefundServiceChargeBizActionVO nvo = (clsAddRefundServiceChargeBizActionVO) valueObject;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                foreach (clsChargeVO evo in nvo.ChargeList)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CancelServices");
                    this.dbServer.AddInParameter(storedProcCommand, "ChargeID", DbType.Int64, evo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "RefundID", DbType.Int64, evo.RefundID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, evo.UnitID);
                    nvo.UnitID = evo.UnitID;
                    this.dbServer.AddInParameter(storedProcCommand, "IsCancelled", DbType.Boolean, evo.IsCancelled);
                    this.dbServer.AddInParameter(storedProcCommand, "CancelledBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "CancelledDate", DbType.DateTime, evo.CancelledDate);
                    this.dbServer.AddInParameter(storedProcCommand, "CancellationRemarkID", DbType.Int64, evo.SelectedRequestRefundReason.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "CancellationRemark", DbType.String, evo.SelectedRequestRefundReason.Description);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(storedProcCommand, "ApprovalRequestID", DbType.Int64, evo.ApprovalRequestID);
                    this.dbServer.AddInParameter(storedProcCommand, "ApprovalRequestUnitID", DbType.Int64, evo.ApprovalRequestUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "ApprovalRequestDetailsID", DbType.Int64, evo.ApprovalRequestDetailsID);
                    this.dbServer.AddInParameter(storedProcCommand, "ApprovalRequestDetailsUnitID", DbType.Int64, evo.ApprovalRequestDetailsUnitID);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    if (nvo.IsUpdate)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_UpdateChargeDetailsFromRefund");
                        this.dbServer.AddInParameter(command2, "ChargeID ", DbType.Int64, evo.ID);
                        this.dbServer.AddInParameter(command2, "Date", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "RefundID", DbType.Int64, evo.RefundID);
                        this.dbServer.AddInParameter(command2, "RefundAmount", DbType.Double, evo.RefundAmount);
                        this.dbServer.ExecuteNonQuery(command2, transaction);
                        continue;
                    }
                    DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddChargeDetails");
                    this.dbServer.AddInParameter(command, "ChargeID", DbType.Int64, evo.ID);
                    this.dbServer.AddInParameter(command, "ChargeUnitID", DbType.Int64, evo.UnitID);
                    this.dbServer.AddInParameter(command, "ParentID", DbType.Int64, evo.ParentID);
                    this.dbServer.AddInParameter(command, "Date", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command, "Quantity", DbType.Double, evo.Quantity);
                    this.dbServer.AddInParameter(command, "Rate", DbType.Double, 0);
                    this.dbServer.AddInParameter(command, "TotalAmount", DbType.Double, 0);
                    this.dbServer.AddInParameter(command, "ConcessionAmount", DbType.Double, 0);
                    this.dbServer.AddInParameter(command, "ServiceTaxAmount", DbType.Double, 0);
                    this.dbServer.AddInParameter(command, "NetAmount", DbType.Double, 0);
                    this.dbServer.AddInParameter(command, "PaidAmount", DbType.Double, 0);
                    this.dbServer.AddInParameter(command, "BalanceAmount", DbType.Double, 0);
                    this.dbServer.AddInParameter(command, "RefundID", DbType.Int64, evo.RefundID);
                    this.dbServer.AddInParameter(command, "RefundAmount", DbType.Double, evo.RefundAmount);
                    this.dbServer.AddInParameter(command, "Status", DbType.Boolean, evo.Status);
                    this.dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    evo.ChargeDetails = new clsChargeDetailsVO();
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.ChargeDetails.ID);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                    evo.ChargeDetails.ID = (long) this.dbServer.GetParameterValue(command, "ID");
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

        public override IValueObject GetChargeListAgainstBills(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetChargeAgainstBillListBizActionVO nvo = (clsGetChargeAgainstBillListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetChargesAgainstBill");
                this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.String, nvo.BillID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsChargeVO>();
                    }
                    while (reader.Read())
                    {
                        clsChargeVO item = new clsChargeVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]))
                        };
                        DateTime? nullable = DALHelper.HandleDate(reader["Date"]);
                        item.Date = new DateTime?(nullable.Value);
                        item.Opd_Ipd_External_Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"]));
                        item.Opd_Ipd_External = (short) DALHelper.HandleDBNull(reader["Opd_Ipd_External"]);
                        item.TariffServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffServiceId"]));
                        item.ServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceId"]));
                        item.ServiceSpecilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"]));
                        item.ServiceName = (string) DALHelper.HandleDBNull(reader["ServiceName"]);
                        item.DoctorId = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorId"]));
                        item.SelectedDoctor.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorId"]));
                        item.SelectedDoctor.Description = (string) DALHelper.HandleDBNull(reader["DoctorName"]);
                        item.Rate = (double) DALHelper.HandleDBNull(reader["Rate"]);
                        item.Quantity = (double) DALHelper.HandleDBNull(reader["Quantity"]);
                        item.TotalAmount = (double) DALHelper.HandleDBNull(reader["TotalAmount"]);
                        item.ConcessionPercent = (double) DALHelper.HandleDBNull(reader["ConcessionPercent"]);
                        if (item.ConcessionPercent == 0.0)
                        {
                            item.Concession = (double) DALHelper.HandleDBNull(reader["ConcessionAmount"]);
                        }
                        item.StaffDiscountPercent = (double) DALHelper.HandleDBNull(reader["StaffDiscountPercent"]);
                        if (item.StaffDiscountPercent == 0.0)
                        {
                            item.StaffDiscountAmount = (double) DALHelper.HandleDBNull(reader["StaffDiscountAmount"]);
                        }
                        item.StaffParentDiscountPercent = (double) DALHelper.HandleDBNull(reader["StaffParentDiscountPercent"]);
                        if (item.StaffParentDiscountPercent == 0.0)
                        {
                            item.StaffParentDiscountAmount = (double) DALHelper.HandleDBNull(reader["StaffParentDiscountAmount"]);
                        }
                        item.ServiceTaxPercent = (double) DALHelper.HandleDBNull(reader["ServiceTaxPercent"]);
                        if (item.ServiceTaxPercent == 0.0)
                        {
                            item.ServiceTaxAmount = (double) DALHelper.HandleDBNull(reader["ServiceTaxAmount"]);
                        }
                        item.Discount = (double) DALHelper.HandleDBNull(reader["Discount"]);
                        item.StaffFree = (double) DALHelper.HandleDBNull(reader["StaffFree"]);
                        item.NetAmount = (double) DALHelper.HandleDBNull(reader["NetAmount"]);
                        item.ServicePaidAmount = (double) DALHelper.HandleDBNull(reader["PaidAmount"]);
                        item.SettleNetAmount = item.NetAmount;
                        item.IsCancelled = (bool) DALHelper.HandleDBNull(reader["IsCancelled"]);
                        item.CancellationRemark = (string) DALHelper.HandleDBNull(reader["CancellationRemark"]);
                        item.CancelledBy = (long?) DALHelper.HandleDBNull(reader["CancelledBy"]);
                        item.CancelledDate = DALHelper.HandleDate(reader["CancelledDate"]);
                        item.IsBilled = (bool) DALHelper.HandleDBNull(reader["IsBilled"]);
                        item.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                        item.TariffServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffServiceID"]));
                        item.ServiceSpecilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"]));
                        item.RateEditable = (bool) DALHelper.HandleDBNull(reader["RateEditable"]);
                        item.MaxRate = Convert.ToDouble((decimal) DALHelper.HandleDBNull(reader["MaxRate"]));
                        item.MinRate = Convert.ToDouble((decimal) DALHelper.HandleDBNull(reader["MinRate"]));
                        item.BalanceAmount = (double) DALHelper.HandleDBNull(reader["BalanceAmount"]);
                        item.isPackageService = (bool) DALHelper.HandleDBNull(reader["isPackageService"]);
                        item.PackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageID"]));
                        item.ParentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ParentID"]));
                        item.TariffId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffId"]));
                        if (item.ParentID > 0L)
                        {
                            item.ChildPackageService = true;
                        }
                        item.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillID"]));
                        item.BillUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillUnitID"]));
                        nvo.List.Add(item);
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

        public override IValueObject GetChargeListForApprovalRequestWindow(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetChargeListBizActionVO nvo = (clsGetChargeListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("GetChargeListForApprovalRequestWindow");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "IsBilled", DbType.Boolean, nvo.IsBilled);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External", DbType.Int16, nvo.Opd_Ipd_External);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External_Id", DbType.Int64, nvo.Opd_Ipd_External_Id);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External_UnitId", DbType.Int64, nvo.Opd_Ipd_External_UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.Int64, nvo.BillID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsChargeVO>();
                    }
                    while (reader.Read())
                    {
                        clsChargeVO item = new clsChargeVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            CostingDivisionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CostingDivisionID"])),
                            ApprovalID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalID"])),
                            ApprovalUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalUnitID"])),
                            IsSetForApproval = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSetApprovalReq"])),
                            FirstApprovalChecked = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFirstApproval"])),
                            SecondApprovalChecked = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSecondApproval"])),
                            Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]))),
                            ServiceDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["ServiceDate"]))),
                            Opd_Ipd_External_Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"])),
                            Opd_Ipd_External = (short) DALHelper.HandleDBNull(reader["Opd_Ipd_External"]),
                            TariffServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffServiceId"])),
                            ServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceId"])),
                            ServiceSpecilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"])),
                            ServiceSubSpecilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecilizationID"])),
                            ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"])),
                            DoctorId = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorId"])),
                            SelectedDoctor = { 
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorId"])),
                                Description = (string) DALHelper.HandleDBNull(reader["DoctorName"])
                            },
                            Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"])),
                            Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"])),
                            TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"])),
                            ConcessionPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionPercent"]))
                        };
                        if (item.ConcessionPercent == 0.0)
                        {
                            item.Concession = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                        }
                        item.StaffDiscountPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffDiscountPercent"]));
                        if (item.StaffDiscountPercent == 0.0)
                        {
                            item.StaffDiscountAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffDiscountAmount"]));
                        }
                        item.StaffParentDiscountPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffParentDiscountPercent"]));
                        if (item.StaffParentDiscountPercent == 0.0)
                        {
                            item.StaffParentDiscountAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffParentDiscountAmount"]));
                        }
                        item.ServiceTaxPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["ServiceTaxPercent"]));
                        if (item.ServiceTaxPercent == 0.0)
                        {
                            item.ServiceTaxAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ServiceTaxAmount"]));
                        }
                        item.Discount = Convert.ToDouble(DALHelper.HandleDBNull(reader["Discount"]));
                        item.StaffFree = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffFree"]));
                        item.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                        item.ServicePaidAmount = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["PaidAmount"]));
                        item.SettleNetAmount = item.NetAmount;
                        item.IsCancelled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCancelled"]));
                        item.CancellationRemark = Convert.ToString(DALHelper.HandleDBNull(reader["CancellationRemark"]));
                        item.CancelledBy = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["CancelledBy"])));
                        item.CancelledDate = DALHelper.HandleDate(reader["CancelledDate"]);
                        item.IsBilled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBilled"]));
                        item.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        item.TariffServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffServiceID"]));
                        item.ServiceSpecilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"]));
                        item.RateEditable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["RateEditable"]));
                        item.MaxRate = Convert.ToDouble((decimal) DALHelper.HandleDBNull(reader["MaxRate"]));
                        item.MinRate = Convert.ToDouble((decimal) DALHelper.HandleDBNull(reader["MinRate"]));
                        item.BalanceAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmount"]));
                        item.isPackageService = Convert.ToBoolean(DALHelper.HandleDBNull(reader["isPackageService"]));
                        item.PackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageID"]));
                        item.ParentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ParentID"]));
                        item.TariffId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffId"]));
                        item.ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]));
                        item.IsAutoCharge = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAutoCharge"]));
                        item.ClassId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassId"]));
                        if (item.ParentID > 0L)
                        {
                            item.ChildPackageService = true;
                        }
                        item.GrossDiscountPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["GrossDiscountPercent"]));
                        item.GrossDiscountReasonID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["GrossDiscountReason"]));
                        item.POBDID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POBDID"]));
                        item.IsSampleCollected = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleCollected"]));
                        item.POBID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POBID"]));
                        item.ROBDID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ROBDID"]));
                        item.IsReportCollected = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsReportCollected"]));
                        item.ROBID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ROBID"]));
                        item.ConditionTypeID = DALHelper.HandleIntegerNull(reader["ConditionTypeID"]);
                        item.ApprovalRemark = Convert.ToString(DALHelper.HandleIntegerNull(reader["ApprovalRemark"]));
                        item.ApprovalStatus = Convert.ToBoolean(DALHelper.HandleIntegerNull(reader["ApprovalStatus"]));
                        nvo.List.Add(item);
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

        public override IValueObject GetChargeTaxDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetChargeListBizActionVO nvo = (clsGetChargeListBizActionVO) valueObject;
            nvo.ChargeVO = new clsChargeVO();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetChargeTaxDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ChargeID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "ChargeUnitID", DbType.Int64, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ChargeVO.ChargeTaxDetailsList == null)
                    {
                        nvo.ChargeVO.ChargeTaxDetailsList = new List<clsChargeTaxDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsChargeTaxDetailsVO item = new clsChargeTaxDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            ChargeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ChargeID"])),
                            ChargeUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ChargeUnitID"])),
                            TaxID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TaxID"])),
                            Percentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TaxPercentage"])),
                            TaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["TaxType"])),
                            ServiceTaxAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TaxAmount"])),
                            IsTaxLimitApplicable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsTaxLimitApplicable"])),
                            TaxLimit = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TaxLimit"])),
                            Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"])),
                            TaxName = Convert.ToString(DALHelper.HandleDBNull(reader["TaxName"])),
                            UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"]))
                        };
                        nvo.ChargeVO.ChargeTaxDetailsList.Add(item);
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

        public override IValueObject GetList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetChargeListBizActionVO nvo = (clsGetChargeListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCharges");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "IsBilled", DbType.Boolean, nvo.IsBilled);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External", DbType.Int16, nvo.Opd_Ipd_External);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External_Id", DbType.Int64, nvo.Opd_Ipd_External_Id);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External_UnitId", DbType.Int64, nvo.Opd_Ipd_External_UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.Int64, nvo.BillID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "RequestTypeID", DbType.Int64, nvo.RequestTypeID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsChargeVO>();
                    }
                    while (reader.Read())
                    {
                        clsChargeVO item = new clsChargeVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            CostingDivisionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CostingDivisionID"])),
                            ApprovalID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalID"])),
                            ApprovalUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalUnitID"])),
                            IsSetForApproval = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSetApprovalReq"])),
                            FirstApprovalChecked = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFirstApproval"])),
                            SecondApprovalChecked = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSecondApproval"])),
                            Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]))),
                            ServiceDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["ServiceDate"]))),
                            Opd_Ipd_External_Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"])),
                            Opd_Ipd_External = (short) DALHelper.HandleDBNull(reader["Opd_Ipd_External"]),
                            TariffServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffServiceId"])),
                            ServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceId"])),
                            ServiceSpecilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"])),
                            ServiceSubSpecilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecilizationID"])),
                            ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"])),
                            ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"])),
                            DoctorId = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorId"])),
                            SelectedDoctor = { 
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorId"])),
                                Description = (string) DALHelper.HandleDBNull(reader["DoctorName"])
                            },
                            Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"])),
                            Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"])),
                            TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"])),
                            ConcessionPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionPercent"]))
                        };
                        if (item.ConcessionPercent == 0.0)
                        {
                            item.Concession = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                        }
                        item.StaffDiscountPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffDiscountPercent"]));
                        if (item.StaffDiscountPercent == 0.0)
                        {
                            item.StaffDiscountAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffDiscountAmount"]));
                        }
                        item.StaffParentDiscountPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffParentDiscountPercent"]));
                        if (item.StaffParentDiscountPercent == 0.0)
                        {
                            item.StaffParentDiscountAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffParentDiscountAmount"]));
                        }
                        item.ServiceTaxPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["ServiceTaxPercent"]));
                        if (item.ServiceTaxPercent == 0.0)
                        {
                            item.ServiceTaxAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ServiceTaxAmount"]));
                        }
                        item.Discount = Convert.ToDouble(DALHelper.HandleDBNull(reader["Discount"]));
                        item.StaffFree = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffFree"]));
                        item.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                        item.ServicePaidAmount = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["PaidAmount"]));
                        item.SettleNetAmount = item.NetAmount;
                        item.IsCancelled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCancelled"]));
                        item.CancellationRemark = Convert.ToString(DALHelper.HandleDBNull(reader["CancellationRemark"]));
                        item.CancelledBy = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["CancelledBy"])));
                        item.CancelledDate = DALHelper.HandleDate(reader["CancelledDate"]);
                        item.IsBilled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBilled"]));
                        item.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        item.TariffServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffServiceID"]));
                        item.ServiceSpecilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"]));
                        item.RateEditable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["RateEditable"]));
                        item.MaxRate = Convert.ToDouble((decimal) DALHelper.HandleDBNull(reader["MaxRate"]));
                        item.MinRate = Convert.ToDouble((decimal) DALHelper.HandleDBNull(reader["MinRate"]));
                        item.BalanceAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmount"]));
                        item.isPackageService = Convert.ToBoolean(DALHelper.HandleDBNull(reader["isPackageService"]));
                        item.PackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageID"]));
                        item.ParentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ParentID"]));
                        item.TariffId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffId"]));
                        item.ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]));
                        item.IsAutoCharge = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAutoCharge"]));
                        item.ClassId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassId"]));
                        item.RefundID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RefundID"]));
                        long parentID = item.ParentID;
                        item.GrossDiscountPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["GrossDiscountPercent"]));
                        item.GrossDiscountReasonID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["GrossDiscountReason"]));
                        item.POBDID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POBDID"]));
                        item.IsSampleCollected = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleCollected"]));
                        item.POBID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POBID"]));
                        item.ROBDID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ROBDID"]));
                        item.IsReportCollected = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsReportCollected"]));
                        item.ROBID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ROBID"]));
                        item.ConditionTypeID = DALHelper.HandleIntegerNull(reader["ConditionTypeID"]);
                        item.ApprovalRequestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalRequestID"]));
                        item.ApprovalRequestUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalRequestUnitID"]));
                        item.ApprovalRequestDetailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalRequestDetailsID"]));
                        item.ApprovalRequestDetailsUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalRequestDetailsUnitID"]));
                        item.IsSendForApproval = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSendForApproval"]));
                        item.LevelID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LevelID"]));
                        item.SelectCharge = Convert.ToBoolean(DALHelper.HandleDBNull(reader["SelectCharge"]));
                        item.ApprovalRemark = Convert.ToString(DALHelper.HandleDBNull(reader["ApprovalRemark"]));
                        item.ApprovalStatus = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ApprovalStatus"]));
                        item.ApprovalRequestRemark = Convert.ToString(DALHelper.HandleDBNull(reader["ApprovalRequestRemark"]));
                        item.InitialRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["InitialRate"]));
                        item.IsResultEntry = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsResultEntry"]));
                        item.Isupload = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsUpload"]));
                        item.CompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompanyId"]));
                        item.PatientSourceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceId"]));
                        item.IsAdjustableHead = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAdjustableHead"]));
                        item.ServiceComponentRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceComponentRate"]));
                        item.IsConsiderAdjustable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsConsiderAdjustable"]));
                        item.SumOfExludedServices = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SumOfExludedServices"]));
                        item.IsAgainstBill = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAgainstBill"]));
                        if (item.IsAgainstBill && !item.ApprovalStatus)
                        {
                            item.RefundID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgainstBillRefundID"]));
                        }
                        item.RefundAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["Amount"]));
                        item.TaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["TaxType"]));
                        item.TotalServiceTaxAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalServiceTaxAmount"]));
                        item.RefundedAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["RefundedAmount"]));
                        item.IsRefund = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRefund"]));
                        item.IsConsumption = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCosumption"]));
                        item.IsPackageConsumption = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackageConsume"]));
                        item.TotalRefundAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["RefundAmount"]));
                        item.RequestRefundReasonID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalRequestRemarkID"]));
                        item.ApprovalRefundReasonID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalRemarkID"]));
                        item.RefundReasonID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CancellationRemarkID"]));
                        item.ProcessID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcessID"]));
                        item.PackageConcessionPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["PackageConcessionPercent"]));
                        if (item.PackageConcessionPercent == 0.0)
                        {
                            item.PackageConcession = Convert.ToDouble(DALHelper.HandleDBNull(reader["PackageConcessionAmount"]));
                        }
                        nvo.List.Add(item);
                    }
                }
                reader.NextResult();
                List<clsServiceTaxVO> source = new List<clsServiceTaxVO>();
                while (true)
                {
                    if (!reader.Read())
                    {
                        if ((nvo.List != null) && ((source != null) && ((nvo.List.Count<clsChargeVO>() > 0) && (source.Count<clsServiceTaxVO>() > 0))))
                        {
                            foreach (clsChargeVO evo2 in nvo.List.ToList<clsChargeVO>())
                            {
                                evo2.ChargeTaxDetailsVO = new clsChargeTaxDetailsVO();
                                evo2.ChargeTaxDetailsVO.TaxLinkingDetailsList = new List<clsServiceTaxVO>();
                                foreach (clsServiceTaxVO xvo2 in source.ToList<clsServiceTaxVO>())
                                {
                                    if (evo2.ServiceId == xvo2.ServiceId)
                                    {
                                        evo2.ChargeTaxDetailsVO.TaxLinkingDetailsList.Add(xvo2);
                                    }
                                }
                            }
                        }
                        reader.Close();
                        break;
                    }
                    clsServiceTaxVO item = new clsServiceTaxVO {
                        UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"])),
                        ServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceId"])),
                        ClassId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassId"])),
                        TaxID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TaxID"])),
                        Percentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TaxPercentage"])),
                        TaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["TaxType"])),
                        IsTaxLimitApplicable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsTaxLimitApplicable"])),
                        TaxLimit = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TaxLimit"])),
                        TaxName = Convert.ToString(DALHelper.HandleDBNull(reader["TaxName"]))
                    };
                    source.Add(item);
                }
            }
            catch (Exception exception1)
            {
                string message = exception1.Message;
            }
            return nvo;
        }
    }
}

