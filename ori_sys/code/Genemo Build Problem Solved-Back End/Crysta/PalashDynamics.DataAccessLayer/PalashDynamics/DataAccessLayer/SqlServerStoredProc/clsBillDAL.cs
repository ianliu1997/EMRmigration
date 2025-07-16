namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration;
    using PalashDynamics.ValueObjects.Administration.DoctorShareRange;
    using PalashDynamics.ValueObjects.Billing;
    using PalashDynamics.ValueObjects.Inventory;
    using PalashDynamics.ValueObjects.Log;
    using PalashDynamics.ValueObjects.Master.DoctorPayment;
    using PalashDynamics.ValueObjects.OutPatientDepartment;
    using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
    using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
    using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
    using PalashDynamics.ValueObjects.Pathology;
    using PalashDynamics.ValueObjects.Patient;
    using PalashDynamics.ValueObjects.Radiology;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Xml.Serialization;

    public class clsBillDAL : clsBaseBillDAL
    {
        private Database dbServer;
        private LogManager logManager;
        private bool IsAuditTrail;

        private clsBillDAL()
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
            clsAddBillBizActionVO bizActionObj = valueObject as clsAddBillBizActionVO;
            if (bizActionObj.Details.IsFreezed)
            {
                if ((bizActionObj.Details.ChargeDetails != null) && ((bizActionObj.Details.ChargeDetails.Count > 0) && ((bizActionObj.Details.PharmacyItems != null) && ((bizActionObj.Details.PharmacyItems.Items != null) && (bizActionObj.Details.PharmacyItems.Items.Count > 0)))))
                {
                    bizActionObj.Details.BillType = BillTypes.Clinical_Pharmacy;
                }
                else if ((bizActionObj.Details.ChargeDetails != null) && (bizActionObj.Details.ChargeDetails.Count > 0))
                {
                    bizActionObj.Details.BillType = BillTypes.Clinical;
                }
                else if ((bizActionObj.Details.PharmacyItems != null) && ((bizActionObj.Details.PharmacyItems.Items != null) && (bizActionObj.Details.PharmacyItems.Items.Count > 0)))
                {
                    bizActionObj.Details.BillType = BillTypes.Pharmacy;
                }
            }
            bizActionObj = (bizActionObj.Details.ID != 0L) ? this.UpdateDetails(bizActionObj, UserVo) : this.AddDetails(bizActionObj, UserVo, null, null);
            return valueObject;
        }

        public override IValueObject Add(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {
            clsAddBillBizActionVO bizActionObj = valueObject as clsAddBillBizActionVO;
            if (bizActionObj.Details.IsFreezed)
            {
                if ((bizActionObj.Details.ChargeDetails != null) && ((bizActionObj.Details.ChargeDetails.Count > 0) && ((bizActionObj.Details.PharmacyItems != null) && ((bizActionObj.Details.PharmacyItems.Items != null) && (bizActionObj.Details.PharmacyItems.Items.Count > 0)))))
                {
                    bizActionObj.Details.BillType = BillTypes.Clinical_Pharmacy;
                }
                else if ((bizActionObj.Details.ChargeDetails != null) && (bizActionObj.Details.ChargeDetails.Count > 0))
                {
                    bizActionObj.Details.BillType = BillTypes.Clinical;
                }
                else if ((bizActionObj.Details.PharmacyItems != null) && ((bizActionObj.Details.PharmacyItems.Items != null) && (bizActionObj.Details.PharmacyItems.Items.Count > 0)))
                {
                    bizActionObj.Details.BillType = BillTypes.Pharmacy;
                }
            }
            bizActionObj = (bizActionObj.Details.ID != 0L) ? this.UpdatePharmacyDetails(bizActionObj, UserVo, myConnection, myTransaction) : this.AddDetails(bizActionObj, UserVo, myConnection, myTransaction);
            return valueObject;
        }

        public override IValueObject AddBillClearanceList(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            clsGetBillClearanceBizActionVO nvo = valueObject as clsGetBillClearanceBizActionVO;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddOTBillClearance");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, nvo.IsFreeze);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleID", DbType.Int64, nvo.ScheduleID);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleUnitID", DbType.Int64, nvo.ScheduleUnitID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.OTBillClearanceID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if (nvo.SuccessStatus == -1)
                {
                    throw new Exception();
                }
                foreach (clsBillVO lvo in nvo.List)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddOTBillClearanceDetails");
                    command2.Connection = connection;
                    this.dbServer.AddInParameter(command2, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "OTBillClearanceID", DbType.Int64, nvo.OTBillClearanceID);
                    this.dbServer.AddInParameter(command2, "OTBillClearanceUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "BillID", DbType.Int64, lvo.ID);
                    this.dbServer.AddInParameter(command2, "BillUnitID", DbType.Int64, lvo.UnitID);
                    this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, lvo.Status);
                    this.dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                }
                if (nvo.SuccessStatus == -1)
                {
                    throw new Exception();
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.List = null;
                nvo.SuccessStatus = -1;
                throw;
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
            return valueObject;
        }

        private clsAddBillBizActionVO AddDetails(clsAddBillBizActionVO BizActionObj, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {
            DbConnection pConnection = null;
            DbTransaction pTransaction = null;
            bool flag = false;
            try
            {
                pConnection = (myConnection == null) ? this.dbServer.CreateConnection() : myConnection;
                if (pConnection.State == ConnectionState.Closed)
                {
                    pConnection.Open();
                }
                pTransaction = (myTransaction == null) ? pConnection.BeginTransaction() : myTransaction;
                long iParentID = 0L;
                long iCDParentID = 0L;
                if (BizActionObj.LogInfoList == null)
                {
                    BizActionObj.LogInfoList = new List<LogInfo>();
                }
                if (BizActionObj.IsFromSaveAndPackageBill && (BizActionObj.objPackageAdvanceVODetails != null))
                {
                    clsAddAdvanceBizActionVO nvo = new clsAddAdvanceBizActionVO();
                    nvo = (clsAddAdvanceBizActionVO) clsBaseAdvanceDAL.GetInstance().AddAdvanceWithPackageBill(BizActionObj.objPackageAdvanceVODetails, UserVo, pConnection, pTransaction);
                    if (nvo.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    BizActionObj.Details.AdvanceID = nvo.Details.ID;
                    BizActionObj.Details.AdvanceUnitID = UserVo.UserLoginInfo.UnitId;
                }
                clsBillVO objDetailsVO = BizActionObj.Details;
                long iD = 0L;
                Func<clsChargeVO, bool> predicate = null;
                int i = 0;
                while (true)
                {
                    if (i >= objDetailsVO.ChargeDetails.Count)
                    {
                        if ((objDetailsVO.ChargeDetails.Count > 0) && (this.IsAuditTrail && (BizActionObj.LogInfoList != null)))
                        {
                            LogInfo item = new LogInfo();
                            Guid guid = default(Guid);
                            item.guid = guid;
                            item.UserId = UserVo.ID;
                            item.TimeStamp = DateTime.Now;
                            item.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            item.MethodName = MethodBase.GetCurrentMethod().ToString();
                            item.Message = " 9 : T_chargeDetailsUnit Id : " + Convert.ToString(UserVo.UserLoginInfo.UnitId);
                            foreach (clsChargeVO evo2 in objDetailsVO.ChargeDetails.ToList<clsChargeVO>())
                            {
                                string[] strArray = new string[] { item.Message, " ,Charge ID:", Convert.ToString(iD), " , Rate : ", Convert.ToString(evo2.Rate), " , Quantity : ", Convert.ToString(evo2.Quantity), " , Total Amount : ", Convert.ToString(evo2.TotalAmount) };
                                strArray[9] = " , Concession Amount : ";
                                strArray[10] = Convert.ToString(evo2.ConcessionAmount);
                                strArray[11] = " , Service Tax Amount : ";
                                strArray[12] = Convert.ToString(evo2.ServiceTaxAmount);
                                strArray[13] = " , Net Amount : ";
                                strArray[14] = Convert.ToString(evo2.NetAmount);
                                strArray[15] = " , Paid Amount : ";
                                strArray[0x10] = Convert.ToString(evo2.PaidAmount);
                                strArray[0x11] = " , Opd_ipd_external_ID : ";
                                strArray[0x12] = Convert.ToString(evo2.Opd_Ipd_External_Id);
                                strArray[0x13] = " , Opd_ipd_external_UnitID : ";
                                strArray[20] = Convert.ToString(evo2.Opd_Ipd_External_UnitId);
                                strArray[0x15] = " , Refund ID : ";
                                strArray[0x16] = Convert.ToString(evo2.RefundID);
                                strArray[0x17] = " , Refund Amount: ";
                                strArray[0x18] = Convert.ToString(evo2.RefundAmount);
                                item.Message = string.Concat(strArray);
                            }
                            BizActionObj.LogInfoList.Add(item);
                        }
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddBill");
                        this.dbServer.AddInParameter(storedProcCommand, "IsPackageBill", DbType.Int64, BizActionObj.IsPackageBill);
                        if (BizActionObj.IsPackageBill)
                        {
                            StringBuilder builder = new StringBuilder();
                            foreach (clsChargeVO evo3 in objDetailsVO.ChargeDetails)
                            {
                                if ((evo3.PackageID > 0L) && evo3.isPackageService)
                                {
                                    builder.Append("," + Convert.ToString(evo3.PackageID));
                                }
                            }
                            builder.Replace(",", "", 0, 1);
                            this.dbServer.AddInParameter(storedProcCommand, "ipPackageList", DbType.String, Convert.ToString(builder));
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                        if (objDetailsVO.LinkServer != null)
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, objDetailsVO.Date);
                        this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External_Id", DbType.Int64, objDetailsVO.Opd_Ipd_External_Id);
                        this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External_UnitId", DbType.Int64, objDetailsVO.Opd_Ipd_External_UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External", DbType.Int16, objDetailsVO.Opd_Ipd_External);
                        this.dbServer.AddInParameter(storedProcCommand, "CashCounterId", DbType.Int64, objDetailsVO.CashCounterId);
                        this.dbServer.AddInParameter(storedProcCommand, "CompanyId", DbType.Int64, objDetailsVO.CompanyId);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientSourceId", DbType.Int64, objDetailsVO.PatientSourceId);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientCategoryId", DbType.Int64, objDetailsVO.PatientCategoryId);
                        this.dbServer.AddInParameter(storedProcCommand, "TariffId", DbType.Int64, objDetailsVO.TariffId);
                        if (objDetailsVO.BillNo != null)
                        {
                            objDetailsVO.BillNo = objDetailsVO.BillNo.Trim();
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "TotalBillAmount", DbType.Double, objDetailsVO.TotalBillAmount);
                        this.dbServer.AddInParameter(storedProcCommand, "TotalConcessionAmount", DbType.Double, objDetailsVO.TotalConcessionAmount);
                        this.dbServer.AddInParameter(storedProcCommand, "NetBillAmount", DbType.Double, objDetailsVO.NetBillAmount);
                        this.dbServer.AddInParameter(storedProcCommand, "CalculatedNetBillAmount", DbType.Double, objDetailsVO.CalculatedNetBillAmount);
                        this.dbServer.AddInParameter(storedProcCommand, "SelfAmount", DbType.Double, objDetailsVO.SelfAmount);
                        if (objDetailsVO.BalanceAmountSelf < 0.0)
                        {
                            objDetailsVO.BalanceAmountSelf = 0.0;
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "BalanceAmountSelf", DbType.Double, objDetailsVO.BalanceAmountSelf);
                        this.dbServer.AddInParameter(storedProcCommand, "StaffFree", DbType.Double, objDetailsVO.StaffFree);
                        this.dbServer.AddInParameter(storedProcCommand, "NonSelfAmount", DbType.Double, objDetailsVO.NonSelfAmount);
                        this.dbServer.AddInParameter(storedProcCommand, "BalanceAmountNonSelf", DbType.Double, objDetailsVO.BalanceAmountNonSelf);
                        this.dbServer.AddInParameter(storedProcCommand, "TotalAdvance", DbType.Double, objDetailsVO.TotalAdvance);
                        this.dbServer.AddInParameter(storedProcCommand, "IsPrinted", DbType.Boolean, objDetailsVO.IsPrinted);
                        this.dbServer.AddInParameter(storedProcCommand, "BillType", DbType.Int16, (short) objDetailsVO.BillType);
                        this.dbServer.AddInParameter(storedProcCommand, "BillPaymentType", DbType.Int16, (short) objDetailsVO.BillPaymentType);
                        this.dbServer.AddInParameter(storedProcCommand, "SponserType", DbType.Boolean, objDetailsVO.SponserType);
                        this.dbServer.AddInParameter(storedProcCommand, "SeniorCitizenCon", DbType.Boolean, objDetailsVO.SeniorCitizenCon);
                        if (objDetailsVO.Remark != null)
                        {
                            objDetailsVO.Remark = objDetailsVO.Remark.Trim();
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, objDetailsVO.Remark);
                        if (objDetailsVO.BillRemark != null)
                        {
                            objDetailsVO.BillRemark = objDetailsVO.BillRemark.Trim();
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "BillRemark", DbType.String, objDetailsVO.BillRemark);
                        this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, objDetailsVO.IsFreezed);
                        this.dbServer.AddInParameter(storedProcCommand, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);
                        this.dbServer.AddInParameter(storedProcCommand, "GrossDiscountReasonID", DbType.Int64, objDetailsVO.GrossDiscountReasonID);
                        this.dbServer.AddInParameter(storedProcCommand, "GrossDiscountPercentage", DbType.Double, objDetailsVO.GrossDiscountPercentage);
                        this.dbServer.AddInParameter(storedProcCommand, "ConcessionAuthorizedBy", DbType.Int64, objDetailsVO.ConcessionAuthorizedBy);
                        this.dbServer.AddInParameter(storedProcCommand, "ConcessionReasonId", DbType.Int64, objDetailsVO.ConcessionReasonId);
                        this.dbServer.AddInParameter(storedProcCommand, "ConcessionRemark", DbType.String, objDetailsVO.ConcessionRemark);
                        this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, objDetailsVO.Status);
                        this.dbServer.AddInParameter(storedProcCommand, "AgainstDonor", DbType.Boolean, objDetailsVO.AgainstDonor);
                        this.dbServer.AddInParameter(storedProcCommand, "LinkPatientID", DbType.Int64, objDetailsVO.LinkPatientID);
                        this.dbServer.AddInParameter(storedProcCommand, "LinkPatientUnitID", DbType.Int64, objDetailsVO.LinkPatientUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        objDetailsVO.UnitID = UserVo.UserLoginInfo.UnitId;
                        this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(storedProcCommand, "IsDraftBill", DbType.Boolean, objDetailsVO.IsDraftBill);
                        this.dbServer.AddInParameter(storedProcCommand, "IsPackageConsumption", DbType.Boolean, flag);
                        this.dbServer.AddInParameter(storedProcCommand, "IsCouterSalesPackage", DbType.Boolean, BizActionObj.IsCouterSalesPackage);
                        this.dbServer.AddInParameter(storedProcCommand, "AdvanceID", DbType.Int64, objDetailsVO.AdvanceID);
                        this.dbServer.AddInParameter(storedProcCommand, "AdvanceUnitID", DbType.Int64, objDetailsVO.AdvanceUnitID);
                        this.dbServer.AddParameter(storedProcCommand, "BillNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000");
                        this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                        StringBuilder builder2 = new StringBuilder();
                        StringBuilder builder3 = new StringBuilder();
                        StringBuilder builder4 = new StringBuilder();
                        StringBuilder builder5 = new StringBuilder();
                        int num4 = 0;
                        while (true)
                        {
                            if (num4 >= objDetailsVO.ChargeDetails.Count)
                            {
                                this.dbServer.AddInParameter(storedProcCommand, "ChargeIdList", DbType.String, builder2.ToString());
                                this.dbServer.AddInParameter(storedProcCommand, "StatusList", DbType.String, builder5.ToString());
                                this.dbServer.AddInParameter(storedProcCommand, "SponsorTypeList", DbType.String, builder3.ToString());
                                this.dbServer.AddInParameter(storedProcCommand, "BalanceAmountList", DbType.String, builder4.ToString());
                                this.dbServer.AddInParameter(storedProcCommand, "PrescriptionDetailsID", DbType.String, BizActionObj.PrescriptionDetailsID);
                                this.dbServer.AddInParameter(storedProcCommand, "InvestigationDetailsID", DbType.String, BizActionObj.InvestigationDetailsID);
                                this.dbServer.AddInParameter(storedProcCommand, "PrescriptionDetailsDrugID", DbType.String, BizActionObj.PrescriptionDetailsDrugID);
                                this.dbServer.ExecuteNonQuery(storedProcCommand, pTransaction);
                                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                                BizActionObj.Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                                BizActionObj.Details.BillNo = (string) this.dbServer.GetParameterValue(storedProcCommand, "BillNo");
                                if (BizActionObj.SuccessStatus == -2)
                                {
                                    throw new Exception();
                                }
                                if (BizActionObj.Details.IsFreezed && (BizActionObj.Details.PaymentDetails != null))
                                {
                                    clsBasePaymentDAL instance = clsBasePaymentDAL.GetInstance();
                                    clsAddPaymentBizActionVO valueObject = new clsAddPaymentBizActionVO {
                                        Details = new clsPaymentVO()
                                    };
                                    valueObject.Details = BizActionObj.Details.PaymentDetails;
                                    valueObject.Details.BillID = BizActionObj.Details.ID;
                                    valueObject.Details.BillAmount = objDetailsVO.NetBillAmount;
                                    valueObject.Details.Date = new DateTime?(BizActionObj.Details.Date);
                                    valueObject.Details.LinkServer = BizActionObj.Details.LinkServer;
                                    valueObject.myTransaction = true;
                                    valueObject = (clsAddPaymentBizActionVO) instance.Add(valueObject, UserVo, pConnection, pTransaction);
                                    if (valueObject.SuccessStatus == -1)
                                    {
                                        throw new Exception();
                                    }
                                    BizActionObj.Details.PaymentDetails.ID = valueObject.Details.ID;
                                    if (this.IsAuditTrail && (BizActionObj.LogInfoList != null))
                                    {
                                        LogInfo item = new LogInfo();
                                        Guid guid2 = default(Guid);
                                        item.guid = guid2;
                                        item.UserId = UserVo.ID;
                                        item.TimeStamp = DateTime.Now;
                                        item.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                                        item.MethodName = MethodBase.GetCurrentMethod().ToString();
                                        string[] strArray2 = new string[] { " 10 : T_paymentUnit Id : ", Convert.ToString(UserVo.UserLoginInfo.UnitId), " , Bill ID : ", Convert.ToString(valueObject.Details.BillID), " , Bill Amount : ", Convert.ToString(valueObject.Details.BillAmount), " , Bill Balance Amount : ", Convert.ToString(valueObject.Details.BillBalanceAmount), " , Advance ID : " };
                                        strArray2[9] = Convert.ToString(valueObject.Details.AdvanceID);
                                        strArray2[10] = " , Advance Amount : ";
                                        strArray2[11] = Convert.ToString(valueObject.Details.AdvanceAmount);
                                        strArray2[12] = " , Refund ID : ";
                                        strArray2[13] = Convert.ToString(valueObject.Details.RefundID);
                                        strArray2[14] = " , Refund Amount : ";
                                        strArray2[15] = Convert.ToString(valueObject.Details.RefundAmount);
                                        strArray2[0x10] = " , Costing Division ID : ";
                                        strArray2[0x11] = Convert.ToString(valueObject.Details.CostingDivisionID);
                                        strArray2[0x12] = " , Payee Naration : ";
                                        strArray2[0x13] = Convert.ToString(valueObject.Details.PayeeNarration);
                                        strArray2[20] = " , TDS Amount : ";
                                        strArray2[0x15] = Convert.ToString(valueObject.Details.TDSAmt);
                                        strArray2[0x16] = " , Receipt No: ";
                                        strArray2[0x17] = Convert.ToString(valueObject.Details.ReceiptNo);
                                        item.Message = string.Concat(strArray2);
                                        foreach (clsPaymentDetailsVO svo in valueObject.Details.PaymentDetails)
                                        {
                                            string[] strArray3 = new string[] { item.Message, "\r\nPayment Details\r\n , Payment ID : ", Convert.ToString(valueObject.Details.ID), " , Payment Mode ID : ", Convert.ToString(svo.PaymentModeID), " , Number : ", Convert.ToString(svo.Number), " , Bank ID : ", Convert.ToString(svo.BankID) };
                                            strArray3[9] = " , Advance ID : ";
                                            strArray3[10] = Convert.ToString(svo.AdvanceID);
                                            strArray3[11] = "\r\n";
                                            item.Message = string.Concat(strArray3);
                                        }
                                        BizActionObj.LogInfoList.Add(item);
                                    }
                                }
                                if (((BizActionObj.Details.PathoWorkOrder != null) && (BizActionObj.Details.PathoWorkOrder.PathoSpecilizationID > 0L)) && ((BizActionObj.Details.IsFreezed && !BizActionObj.Details.IsIPDBill) || BizActionObj.Details.IsIPDBill))
                                {
                                    BizActionObj.Details.PathoWorkOrder.Opd_Ipd_External_ID = BizActionObj.Details.Opd_Ipd_External_Id;
                                    BizActionObj.Details.PathoWorkOrder.Opd_Ipd_External_UnitID = BizActionObj.Details.Opd_Ipd_External_UnitId;
                                    BizActionObj.Details.PathoWorkOrder.SampleType = false;
                                    BizActionObj.Details.PathoWorkOrder.Opd_Ipd_External = new long?((long) BizActionObj.Details.Opd_Ipd_External);
                                    BizActionObj.Details.PathoWorkOrder.UnitId = UserVo.UserLoginInfo.UnitId;
                                    BizActionObj.Details.PathoWorkOrder.AddedDateTime = BizActionObj.Details.AddedDateTime;
                                    BizActionObj.Details.PathoWorkOrder.OrderDate = new DateTime?(BizActionObj.Details.Date);
                                    BizActionObj.Details.PathoWorkOrder.BillNo = BizActionObj.Details.BillNo;
                                    BizActionObj.Details.PathoWorkOrder.BillID = BizActionObj.Details.ID;
                                    int num5 = 0;
                                    while (true)
                                    {
                                        if (num5 >= objDetailsVO.ChargeDetails.Count)
                                        {
                                            clsBaseOrderBookingDAL instance = clsBaseOrderBookingDAL.GetInstance();
                                            clsAddPathOrderBookingBizActionVO valueObject = new clsAddPathOrderBookingBizActionVO {
                                                myTransaction = true,
                                                PathOrderBooking = BizActionObj.Details.PathoWorkOrder,
                                                PathOrderBookingDetailList = BizActionObj.Details.PathoWorkOrder.Items
                                            };
                                            valueObject = (clsAddPathOrderBookingBizActionVO) instance.AddPathOrderBooking(valueObject, UserVo, pTransaction, pConnection);
                                            if (valueObject.SuccessStatus == -1)
                                            {
                                                throw new Exception();
                                            }
                                            BizActionObj.Details.PathoWorkOrder.ID = valueObject.PathOrderBooking.ID;
                                            break;
                                        }
                                        if (BizActionObj.Details.PathoWorkOrder.PathoSpecilizationID == objDetailsVO.ChargeDetails[num5].ServiceSpecilizationID)
                                        {
                                            BizActionObj.Details.PathoWorkOrder.DoctorID = BizActionObj.Details.PathoWorkOrder.DoctorID;
                                            clsPathOrderBookingVO pathologyTestDetails = new clsPathOrderBookingVO();
                                            pathologyTestDetails = this.GetPathologyTestDetails(objDetailsVO.ChargeDetails[num5].ServiceId, UserVo);
                                            if ((pathologyTestDetails.Items != null) && (pathologyTestDetails.Items.Count > 0))
                                            {
                                                foreach (clsPathOrderBookingDetailVO lvo in pathologyTestDetails.Items)
                                                {
                                                    lvo.ServiceID = objDetailsVO.ChargeDetails[num5].ServiceId;
                                                    lvo.ChargeID = objDetailsVO.ChargeDetails[num5].ID;
                                                    lvo.TariffServiceID = objDetailsVO.ChargeDetails[num5].TariffServiceId;
                                                    lvo.TestCharges = objDetailsVO.ChargeDetails[num5].Rate;
                                                    BizActionObj.Details.PathoWorkOrder.Items.Add(lvo);
                                                }
                                            }
                                        }
                                        num5++;
                                    }
                                }
                                if (BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails.Count > 0)
                                {
                                    BizActionObj.Details.RadiologyWorkOrder.Opd_Ipd_External_ID = BizActionObj.Details.Opd_Ipd_External_Id;
                                    BizActionObj.Details.RadiologyWorkOrder.Opd_Ipd_External_UnitID = BizActionObj.Details.Opd_Ipd_External_UnitId;
                                    BizActionObj.Details.RadiologyWorkOrder.Opd_Ipd_External = new long?((long) BizActionObj.Details.Opd_Ipd_External);
                                    BizActionObj.Details.RadiologyWorkOrder.UnitID = BizActionObj.Details.UnitID;
                                    BizActionObj.Details.RadiologyWorkOrder.AddedDateTime = BizActionObj.Details.AddedDateTime;
                                    BizActionObj.Details.RadiologyWorkOrder.Date = new DateTime?(BizActionObj.Details.Date);
                                    BizActionObj.Details.RadiologyWorkOrder.BillNo = BizActionObj.Details.BillNo;
                                    BizActionObj.Details.RadiologyWorkOrder.BillID = BizActionObj.Details.ID;
                                    int num6 = 0;
                                    while (true)
                                    {
                                        if (num6 >= BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails.Count)
                                        {
                                            clsBaseRadiologyDAL instance = clsBaseRadiologyDAL.GetInstance();
                                            clsAddRadOrderBookingBizActionVO valueObject = new clsAddRadOrderBookingBizActionVO {
                                                BookingDetails = BizActionObj.Details.RadiologyWorkOrder,
                                                myTransaction = true
                                            };
                                            valueObject = (clsAddRadOrderBookingBizActionVO) instance.AddOrderBooking(valueObject, UserVo, pConnection, pTransaction);
                                            if (valueObject.SuccessStatus == -1)
                                            {
                                                throw new Exception();
                                            }
                                            objDetailsVO.RadiologyWorkOrder.ID = valueObject.BookingDetails.ID;
                                            break;
                                        }
                                        int num7 = 0;
                                        while (true)
                                        {
                                            if (num7 < objDetailsVO.ChargeDetails.Count)
                                            {
                                                if (objDetailsVO.ChargeDetails[num7].isPackageService)
                                                {
                                                    BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[num6].ChargeID = objDetailsVO.ChargeDetails[num7].ID;
                                                    clsRadOrderBookingDetailsVO radilogyTestDetails = new clsRadOrderBookingDetailsVO();
                                                    radilogyTestDetails = this.GetRadilogyTestDetails(BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[num6].ServiceID, UserVo);
                                                    if (radilogyTestDetails != null)
                                                    {
                                                        BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[num6].TestID = radilogyTestDetails.TestID;
                                                    }
                                                }
                                                else
                                                {
                                                    if (BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[num6].TariffServiceID != objDetailsVO.ChargeDetails[num7].TariffServiceId)
                                                    {
                                                        num7++;
                                                        continue;
                                                    }
                                                    BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[num6].ChargeID = objDetailsVO.ChargeDetails[num7].ID;
                                                    clsRadOrderBookingDetailsVO radilogyTestDetails = new clsRadOrderBookingDetailsVO();
                                                    radilogyTestDetails = this.GetRadilogyTestDetails(BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[num6].ServiceID, UserVo);
                                                    if (radilogyTestDetails != null)
                                                    {
                                                        BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[num6].TestID = radilogyTestDetails.TestID;
                                                    }
                                                }
                                            }
                                            num6++;
                                            break;
                                        }
                                    }
                                }
                                bool flag2 = false;
                                if (objDetailsVO.PharmacyItems.Items.Count > 0)
                                {
                                    flag2 = true;
                                    clsBaseItemSalesDAL instance = clsBaseItemSalesDAL.GetInstance();
                                    clsAddItemSalesBizActionVO valueObject = new clsAddItemSalesBizActionVO {
                                        Details = objDetailsVO.PharmacyItems
                                    };
                                    valueObject.Details.BillID = BizActionObj.Details.ID;
                                    valueObject.Details.IsBilled = BizActionObj.Details.IsFreezed;
                                    valueObject.myTransaction = true;
                                    valueObject = (clsAddItemSalesBizActionVO) instance.Add(valueObject, UserVo, pConnection, pTransaction);
                                    if (valueObject.SuccessStatus == -1)
                                    {
                                        throw new Exception();
                                    }
                                    objDetailsVO.PharmacyItems.ID = valueObject.Details.ID;
                                    if (BizActionObj.IsPackagePharmacyConsumption)
                                    {
                                        clsGetPatientPackageInfoListBizActionVO objPatientPackInfoVODetails = BizActionObj.objPatientPackInfoVODetails;
                                    }
                                }
                                DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddDoctorPaymentDetails");
                                this.dbServer.AddInParameter(command, "BillID", DbType.Int64, BizActionObj.Details.ID);
                                this.dbServer.AddInParameter(command, "BillUnitID", DbType.Int64, BizActionObj.Details.UnitID);
                                this.dbServer.AddInParameter(command, "Opd_Ipd_External_Id", DbType.Int64, objDetailsVO.Opd_Ipd_External_Id);
                                this.dbServer.AddInParameter(command, "Opd_Ipd_External_UnitId", DbType.Int64, objDetailsVO.Opd_Ipd_External_UnitId);
                                this.dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.ExecuteNonQuery(command, pTransaction);
                                if (myConnection == null)
                                {
                                    pTransaction.Commit();
                                }
                                if ((BizActionObj.LogInfoList != null) && (!flag2 && ((BizActionObj.LogInfoList.Count > 0) && this.IsAuditTrail)))
                                {
                                    this.SetLogInfo(BizActionObj.LogInfoList, UserVo.ID);
                                    BizActionObj.LogInfoList.Clear();
                                }
                                BizActionObj.SuccessStatus = 0;
                                break;
                            }
                            builder2.Append(objDetailsVO.ChargeDetails[num4].ID);
                            builder5.Append(objDetailsVO.ChargeDetails[num4].Status);
                            builder4.Append((double) (objDetailsVO.ChargeDetails[num4].NetAmount - objDetailsVO.ChargeDetails[num4].PaidAmount));
                            builder3.Append(objDetailsVO.ChargeDetails[num4].SponsorType);
                            if (num4 < (objDetailsVO.ChargeDetails.Count - 1))
                            {
                                builder2.Append(",");
                                builder5.Append(",");
                                builder4.Append(",");
                                builder3.Append(",");
                            }
                            num4++;
                        }
                        break;
                    }
                    if (!objDetailsVO.ChargeDetails[i].ChildPackageService)
                    {
                        clsBaseChargeDAL instance = clsBaseChargeDAL.GetInstance();
                        clsAddChargeBizActionVO valueObject = new clsAddChargeBizActionVO {
                            Details = objDetailsVO.ChargeDetails[i]
                        };
                        if (objDetailsVO.ChargeDetails[i].ParentID > 0L)
                        {
                            flag = true;
                            objDetailsVO.ChargeDetails[i].IsPackageConsumption = true;
                        }
                        valueObject.Details.PaidAmount = !valueObject.Details.Status ? 0.0 : valueObject.Details.NetAmount;
                        valueObject.Details.IsBilled = BizActionObj.Details.IsFreezed;
                        valueObject.Details.Date = new DateTime?(objDetailsVO.Date);
                        valueObject.Details.Opd_Ipd_External_Id = objDetailsVO.Opd_Ipd_External_Id;
                        valueObject.Details.Opd_Ipd_External_UnitId = objDetailsVO.Opd_Ipd_External_UnitId;
                        valueObject.Details.Opd_Ipd_External = objDetailsVO.Opd_Ipd_External;
                        valueObject.Details.ClassId = objDetailsVO.ChargeDetails[i].ClassId;
                        valueObject.Details.IsIPDBill = objDetailsVO.IsIPDBill;
                        valueObject = (clsAddChargeBizActionVO) instance.Add(valueObject, UserVo, pConnection, pTransaction, 0L, 0L);
                        if (valueObject.SuccessStatus == -1)
                        {
                            throw new Exception();
                        }
                        objDetailsVO.ChargeDetails[i].ID = valueObject.Details.ID;
                        objDetailsVO.ChargeDetails[i].ChargeDetails.ID = valueObject.Details.ChargeDetails.ID;
                        iD = valueObject.Details.ID;
                        iParentID = objDetailsVO.ChargeDetails[i].ID;
                        iCDParentID = objDetailsVO.ChargeDetails[i].ChargeDetails.ID;
                        if (predicate == null)
                        {
                            predicate = charge => charge.PackageID.Equals(objDetailsVO.ChargeDetails[i].PackageID) && charge.ChildPackageService;
                        }
                        foreach (clsChargeVO evo in objDetailsVO.ChargeDetails.Where<clsChargeVO>(predicate))
                        {
                            clsBaseChargeDAL edal3 = clsBaseChargeDAL.GetInstance();
                            clsAddChargeBizActionVO nvo3 = new clsAddChargeBizActionVO();
                            evo.ID = 0L;
                            nvo3.Details = evo;
                            nvo3.Details.PaidAmount = !nvo3.Details.Status ? 0.0 : evo.NetAmount;
                            nvo3.Details.IsBilled = BizActionObj.Details.IsFreezed;
                            nvo3.Details.Date = new DateTime?(objDetailsVO.Date);
                            nvo3.Details.Opd_Ipd_External_Id = objDetailsVO.Opd_Ipd_External_Id;
                            nvo3.Details.Opd_Ipd_External_UnitId = objDetailsVO.Opd_Ipd_External_UnitId;
                            nvo3.Details.Opd_Ipd_External = objDetailsVO.Opd_Ipd_External;
                            nvo3 = (clsAddChargeBizActionVO) edal3.Add(nvo3, UserVo, pConnection, pTransaction, iParentID, iCDParentID);
                            if (nvo3.SuccessStatus == -1)
                            {
                                throw new Exception();
                            }
                            evo.ID = nvo3.Details.ID;
                            evo.ChargeDetails.ID = nvo3.Details.ChargeDetails.ID;
                        }
                    }
                    i++;
                }
            }
            catch (Exception)
            {
                BizActionObj.Details = null;
                BizActionObj.SuccessStatus = -1;
                if (myConnection == null)
                {
                    pTransaction.Rollback();
                }
            }
            finally
            {
                if (myConnection == null)
                {
                    pConnection.Close();
                }
            }
            return BizActionObj;
        }

        public override IValueObject AddDoctorShareRange(IValueObject valueObject, clsUserVO UserVO)
        {
            clsAddDoctorShareRangeBizActionVO nvo = valueObject as clsAddDoctorShareRangeBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                clsDoctorShareRangeVO shareRangeDetails = nvo.ShareRangeDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddDoctorShareRangeMaster");
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, shareRangeDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpperLimit", DbType.Decimal, shareRangeDetails.UpperLimit);
                this.dbServer.AddInParameter(storedProcCommand, "LowerLimit", DbType.Decimal, shareRangeDetails.LowerLimit);
                this.dbServer.AddInParameter(storedProcCommand, "SharePercentage", DbType.Decimal, shareRangeDetails.SharePercentage);
                this.dbServer.AddInParameter(storedProcCommand, "ShareAmount", DbType.Decimal, shareRangeDetails.ShareAmount);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                transaction.Rollback();
                nvo.ShareRangeDetails = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject AddPathologyBill(IValueObject valueObject, clsUserVO UserVo)
        {
            bool flag = false;
            clsAddBillBizActionVO nvo = valueObject as clsAddBillBizActionVO;
            if (nvo.Details.IsFreezed && ((nvo.Details.ChargeDetails != null) && ((nvo.Details.ChargeDetails.Count > 0) && ((nvo.Details.PharmacyItems != null) && ((nvo.Details.PharmacyItems.Items != null) && (nvo.Details.PharmacyItems.Items.Count > 0))))))
            {
                nvo.Details.BillType = BillTypes.Clinical;
            }
            DbTransaction myTransaction = null;
            DbConnection myConnection = null;
            try
            {
                myConnection = this.dbServer.CreateConnection();
                myConnection.Open();
                myTransaction = myConnection.BeginTransaction();
                if (nvo.obPathoPatientVODetails != null)
                {
                    clsBasePatientDAL instance = clsBasePatientDAL.GetInstance();
                    clsAddPatientForPathologyBizActionVO obPathoPatientVODetails = new clsAddPatientForPathologyBizActionVO();
                    obPathoPatientVODetails = nvo.obPathoPatientVODetails;
                    obPathoPatientVODetails.PatientDetails.IsVisitForPatho = false;
                    obPathoPatientVODetails = (clsAddPatientForPathologyBizActionVO) instance.AddPatientForPathology(obPathoPatientVODetails, UserVo, myConnection, myTransaction);
                    if (obPathoPatientVODetails.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    nvo.Details.PatientID = obPathoPatientVODetails.PatientDetails.GeneralDetails.PatientID;
                    nvo.Details.PatientUnitID = obPathoPatientVODetails.PatientDetails.GeneralDetails.UnitId;
                    nvo.Details.Opd_Ipd_External_Id = obPathoPatientVODetails.BizActionVOSaveVisit.VisitDetails.ID;
                    nvo.Details.Opd_Ipd_External_UnitId = obPathoPatientVODetails.BizActionVOSaveVisit.VisitDetails.UnitId;
                }
                if (nvo.obPathoPatientVisitVODetails != null)
                {
                    clsAddVisitBizActionVO nvo3 = new clsAddVisitBizActionVO();
                    nvo3 = (clsAddVisitBizActionVO) clsBaseVisitDAL.GetInstance().AddVisit(nvo.obPathoPatientVisitVODetails, UserVo, myConnection, myTransaction);
                    if (nvo3.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    nvo.Details.Opd_Ipd_External_Id = nvo3.VisitDetails.ID;
                    nvo.Details.Opd_Ipd_External_UnitId = UserVo.UserLoginInfo.UnitId;
                    if (nvo.Details.PharmacyItems != null)
                    {
                        nvo.Details.PharmacyItems.VisitID = nvo3.VisitDetails.ID;
                    }
                }
                clsAddBillBizActionVO nvo4 = new clsAddBillBizActionVO();
                nvo4 = (clsAddBillBizActionVO) GetInstance().Add(nvo, UserVo, myConnection, myTransaction);
                if (nvo4.SuccessStatus == -1)
                {
                    flag = true;
                    throw new Exception();
                }
                nvo.SuccessStatus = nvo4.SuccessStatus;
                myTransaction.Commit();
                nvo.SuccessStatus = 0;
            }
            catch (Exception)
            {
                nvo.SuccessStatus = !flag ? -1 : -10;
                myTransaction.Rollback();
                nvo.Details = null;
            }
            finally
            {
                myConnection.Close();
                myTransaction = null;
                myConnection = null;
            }
            return nvo;
        }

        public override IValueObject AddPharmacyBill(IValueObject valueObject, clsUserVO UserVo)
        {
            bool flag = false;
            clsAddBillBizActionVO nvo = valueObject as clsAddBillBizActionVO;
            if (nvo.Details.IsFreezed && ((nvo.Details.ChargeDetails != null) && ((nvo.Details.ChargeDetails.Count > 0) && ((nvo.Details.PharmacyItems != null) && ((nvo.Details.PharmacyItems.Items != null) && (nvo.Details.PharmacyItems.Items.Count > 0))))))
            {
                nvo.Details.BillType = BillTypes.Clinical_Pharmacy;
            }
            DbTransaction myTransaction = null;
            DbConnection myConnection = null;
            try
            {
                myConnection = this.dbServer.CreateConnection();
                myConnection.Open();
                myTransaction = myConnection.BeginTransaction();
                if (nvo.objPatientVODetails != null)
                {
                    clsAddPatientBizActionVO nvo2 = new clsAddPatientBizActionVO();
                    nvo2 = (clsAddPatientBizActionVO) clsBasePatientDAL.GetInstance().AddPatientOPDWithTransaction(nvo.objPatientVODetails, UserVo, myConnection, myTransaction);
                    if (nvo2.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    if (nvo.objVisitVODetails != null)
                    {
                        nvo.objVisitVODetails.VisitDetails.PatientId = nvo2.PatientDetails.GeneralDetails.PatientID;
                        nvo.objVisitVODetails.VisitDetails.PatientUnitId = nvo2.PatientDetails.GeneralDetails.UnitId;
                    }
                    nvo.Details.PatientID = nvo2.PatientDetails.GeneralDetails.PatientID;
                    nvo.Details.PatientUnitID = nvo2.PatientDetails.GeneralDetails.UnitId;
                    if (nvo.Details.PharmacyItems != null)
                    {
                        nvo.Details.PharmacyItems.PatientID = nvo2.PatientDetails.GeneralDetails.PatientID;
                        nvo.Details.PharmacyItems.PatientUnitID = nvo2.PatientDetails.GeneralDetails.UnitId;
                    }
                }
                if (nvo.objVisitVODetails != null)
                {
                    clsAddVisitBizActionVO nvo3 = new clsAddVisitBizActionVO();
                    nvo3 = (clsAddVisitBizActionVO) clsBaseVisitDAL.GetInstance().AddVisit(nvo.objVisitVODetails, UserVo, myConnection, myTransaction);
                    if (nvo3.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    nvo.Details.Opd_Ipd_External_Id = nvo3.VisitDetails.ID;
                    nvo.Details.Opd_Ipd_External_UnitId = UserVo.UserLoginInfo.UnitId;
                    if (nvo.Details.PharmacyItems != null)
                    {
                        nvo.Details.PharmacyItems.VisitID = nvo3.VisitDetails.ID;
                    }
                }
                clsAddBillBizActionVO nvo4 = new clsAddBillBizActionVO();
                nvo4 = (clsAddBillBizActionVO) GetInstance().Add(nvo, UserVo, myConnection, myTransaction);
                if (nvo4.SuccessStatus == -1)
                {
                    flag = true;
                    throw new Exception();
                }
                nvo.SuccessStatus = nvo4.SuccessStatus;
                myTransaction.Commit();
                nvo.SuccessStatus = 0;
                if (((nvo.Details.PharmacyItems != null) && (nvo.LogInfoList != null)) && (nvo.LogInfoList.Count > 0))
                {
                    foreach (LogInfo info in nvo.LogInfoList)
                    {
                        string[] strArray = new string[] { info.Message, " , PatientID : ", Convert.ToString(nvo.Details.PharmacyItems.PatientID), " , PatientUnitID : ", Convert.ToString(nvo.Details.PharmacyItems.PatientUnitID), " , Opd_Ipd_External_Id : ", Convert.ToString(nvo.Details.Opd_Ipd_External_Id), " , Opd_Ipd_External_UnitId : ", Convert.ToString(nvo.Details.Opd_Ipd_External_UnitId) };
                        strArray[9] = " , BillID : ";
                        strArray[10] = Convert.ToString(nvo.Details.ID);
                        strArray[11] = " , BillNo : ";
                        strArray[12] = Convert.ToString(nvo.Details.BillNo);
                        info.Message = string.Concat(strArray);
                    }
                    if ((nvo.LogInfoList != null) && this.IsAuditTrail)
                    {
                        this.SetLogInfo(nvo.LogInfoList, UserVo.ID);
                        nvo.LogInfoList.Clear();
                    }
                }
            }
            catch (Exception)
            {
                nvo.SuccessStatus = !flag ? -1 : -10;
                myTransaction.Rollback();
                nvo.Details = null;
            }
            finally
            {
                myConnection.Close();
                myTransaction = null;
                myConnection = null;
            }
            return nvo;
        }

        public override IValueObject ApplyPackageDiscountRateOnItems(IValueObject valueObject, clsUserVO UserVo)
        {
            clsApplyPackageDiscountRateToItems items = valueObject as clsApplyPackageDiscountRateToItems;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ApplyPackageDiscountRateOnItemForPackage_ByAnjali_II");
                this.dbServer.AddInParameter(storedProcCommand, "PatientCatagoryL1", DbType.Int64, items.objApplyItemPackageDiscountRateDetails.PatientCatagoryL1);
                this.dbServer.AddInParameter(storedProcCommand, "PatientCatagoryL2", DbType.Int64, items.objApplyItemPackageDiscountRateDetails.PatientCatagoryL2);
                this.dbServer.AddInParameter(storedProcCommand, "PatientCatagoryL3", DbType.Int64, items.objApplyItemPackageDiscountRateDetails.PatientCatagoryL3);
                this.dbServer.AddInParameter(storedProcCommand, "ItemIDs", DbType.String, items.objApplyItemPackageDiscountRateDetails.ItemIDs);
                this.dbServer.AddInParameter(storedProcCommand, "ipLoginUnitID", DbType.String, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyID ", DbType.Int64, items.objApplyItemPackageDiscountRateDetails.CompanyID);
                this.dbServer.AddInParameter(storedProcCommand, "ipPatientGenderID ", DbType.Int64, items.objApplyItemPackageDiscountRateDetails.PatientGenderID);
                this.dbServer.AddInParameter(storedProcCommand, "ipPatientDateOfBirth ", DbType.DateTime, items.objApplyItemPackageDiscountRateDetails.PatientDateOfBirth);
                this.dbServer.AddInParameter(storedProcCommand, "ipPatientID ", DbType.Int64, items.objApplyItemPackageDiscountRateDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "ipPatientUnitID ", DbType.Int64, items.objApplyItemPackageDiscountRateDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ipPackageID ", DbType.Int64, items.objApplyItemPackageDiscountRateDetails.PackageID);
                this.dbServer.AddInParameter(storedProcCommand, "ipPackageBillID ", DbType.Int64, items.objApplyItemPackageDiscountRateDetails.PackageBillID);
                this.dbServer.AddInParameter(storedProcCommand, "ipPackageBillUnitID ", DbType.Int64, items.objApplyItemPackageDiscountRateDetails.PackageBillUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (items.objApplyItemPackageDiscountRate == null)
                    {
                        items.objApplyItemPackageDiscountRate = new List<clsApplyPackageDiscountRateOnItemVO>();
                    }
                    while (reader.Read())
                    {
                        clsApplyPackageDiscountRateOnItemVO item = new clsApplyPackageDiscountRateOnItemVO {
                            ApplicableToAll = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ApplicableToAll"])),
                            DiscountedPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["Discount"])),
                            ItemId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ItemID"])),
                            CategoryId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["CategoryId"])),
                            IsCategory = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCategory"])),
                            GroupId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["GroupId"])),
                            IsGroup = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsGroup"])),
                            Budget = Convert.ToSingle(DALHelper.HandleDBNull(reader["Budget"])),
                            TotalBudget = Convert.ToSingle(DALHelper.HandleDBNull(reader["TotalBudget"])),
                            CalculatedBudget = Convert.ToSingle(DALHelper.HandleDBNull(reader["CalculatedBudget"])),
                            CalculatedTotalBudget = Convert.ToSingle(DALHelper.HandleDBNull(reader["CalculatedTotalBudget"]))
                        };
                        items.objApplyItemPackageDiscountRate.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return items;
        }

        public override IValueObject ApplyPackageDiscountRateOnService(IValueObject valueObject, clsUserVO UserVo)
        {
            clsApplyPackageDiscountRateOnServiceBizActionVO nvo = valueObject as clsApplyPackageDiscountRateOnServiceBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ApplyPackageDiscountRateOnServiceForPackageMillan");
                if (nvo.objApplyPackageDiscountRate == null)
                {
                    nvo.objApplyPackageDiscountRate = new List<clsApplyPackageDiscountRateOnServiceVO>();
                }
                if (nvo.ipServiceList == null)
                {
                    nvo.ipServiceList = new List<clsServiceMasterVO>();
                }
                StringBuilder builder = new StringBuilder();
                StringBuilder builder2 = new StringBuilder();
                StringBuilder builder3 = new StringBuilder();
                StringBuilder builder4 = new StringBuilder();
                StringBuilder builder5 = new StringBuilder();
                foreach (clsServiceMasterVO rvo in nvo.ipServiceList)
                {
                    builder.Append("," + Convert.ToString(rvo.ID));
                    builder2.Append("," + Convert.ToString(rvo.TariffID));
                    builder3.Append("," + Convert.ToString(rvo.PackageID));
                    builder4.Append("," + Convert.ToString(rvo.ChargeID));
                    builder5.Append("," + Convert.ToString(rvo.ProcessID));
                }
                builder.Replace(",", "", 0, 1);
                builder2.Replace(",", "", 0, 1);
                builder3.Replace(",", "", 0, 1);
                builder4.Replace(",", "", 0, 1);
                builder5.Replace(",", "", 0, 1);
                this.dbServer.AddInParameter(storedProcCommand, "ipLoginUnitID", DbType.Int64, nvo.ipLoginUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ipPatientID", DbType.Int64, nvo.ipPatientID);
                this.dbServer.AddInParameter(storedProcCommand, "ipPatientUnitID", DbType.Int64, nvo.ipPatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ipVisitID", DbType.Int64, nvo.ipVisitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsIPD", DbType.Boolean, nvo.IsIPD);
                this.dbServer.AddInParameter(storedProcCommand, "ipServiceList", DbType.String, Convert.ToString(builder));
                this.dbServer.AddInParameter(storedProcCommand, "ipTariffList", DbType.String, Convert.ToString(builder2));
                this.dbServer.AddInParameter(storedProcCommand, "ipPackageList", DbType.String, Convert.ToString(builder3));
                this.dbServer.AddInParameter(storedProcCommand, "ipParentList", DbType.String, Convert.ToString(builder4));
                this.dbServer.AddInParameter(storedProcCommand, "ipProcessList", DbType.String, Convert.ToString(builder5));
                this.dbServer.AddInParameter(storedProcCommand, "ipPatientGenderID", DbType.Int64, nvo.ipPatientGenderID);
                this.dbServer.AddInParameter(storedProcCommand, "ipPatientDateOfBirth", DbType.DateTime, nvo.ipPatientDateOfBirth);
                this.dbServer.AddInParameter(storedProcCommand, "MemberRelationID", DbType.Int64, nvo.MemberRelationID);
                this.dbServer.AddInParameter(storedProcCommand, "SponsorID", DbType.Int64, nvo.SponsorID);
                this.dbServer.AddInParameter(storedProcCommand, "SponsorUnitID", DbType.Int64, nvo.SponsorUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsApplyPackageDiscountRateOnServiceVO item = new clsApplyPackageDiscountRateOnServiceVO {
                            DiscountedPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountedPercentage"])),
                            DiscountedRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountedRate"])),
                            GrossDiscountID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GrossDiscountID"])),
                            IsApplyOn_Rate_Percentage = Convert.ToInt32(DALHelper.HandleDBNull(reader["IsApplyOn_Rate_Percentage"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"])),
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"])),
                            ServiceID_AsPackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID_AsPackageID"])),
                            TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"])),
                            PackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageID"])),
                            IsServiceItemStockAvailable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ServiceItemStockAvailable"])),
                            IsDiscountOnQuantity = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDiscountOnQuantity"])),
                            ActualQuantity = Convert.ToInt64(DALHelper.HandleDBNull(reader["ActualQuantity"])),
                            UsedQuantity = Convert.ToInt64(DALHelper.HandleDBNull(reader["UsedQuantity"])),
                            IsAgeApplicable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAgeApplicable"])),
                            ServiceMemberRelationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceMemberRelationID"])),
                            ConcessionPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionPercent"])),
                            ProcessID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcessID"]))
                        };
                        nvo.objApplyPackageDiscountRate.Add(item);
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

        public override IValueObject ChangeStatus(IValueObject valueObject, clsUserVO UserVO)
        {
            clsAddDoctorShareRangeBizActionVO nvo = valueObject as clsAddDoctorShareRangeBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                clsDoctorShareRangeVO shareRangeDetails = nvo.ShareRangeDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ChangeDoctorShareRangeMasterStatus");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, shareRangeDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, shareRangeDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, shareRangeDetails.Status);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                transaction.Rollback();
                nvo.ShareRangeDetails = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject CheckPackageAdvanceBilling(IValueObject valueObject, clsUserVO UserVO)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            clsAddBillBizActionVO nvo = valueObject as clsAddBillBizActionVO;
            try
            {
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CheckPackageAdvanceBilling");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.Details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.Details.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int64, nvo.Details.PackageID);
                this.dbServer.AddInParameter(storedProcCommand, "CurrentBillAmount", DbType.Double, nvo.Details.TotalBillAmount);
                this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.Int64, nvo.Details.BillID);
                this.dbServer.AddInParameter(storedProcCommand, "BillUnitID", DbType.Int64, nvo.Details.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ParentID", DbType.Int64, nvo.Details.ParentID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddOutParameter(storedProcCommand, "BalancePackageAdvance", DbType.Decimal, 0);
                this.dbServer.AddOutParameter(storedProcCommand, "ServiceConsumptionAmount", DbType.Decimal, 0);
                this.dbServer.AddOutParameter(storedProcCommand, "PharmacyConsumptionAmount", DbType.Decimal, 0);
                this.dbServer.ExecuteNonQuery(storedProcCommand, connection.BeginTransaction());
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.BalancePackageAdvance = Convert.ToDecimal(this.dbServer.GetParameterValue(storedProcCommand, "BalancePackageAdvance"));
                nvo.ServiceConsumptionAmount = Convert.ToDecimal(this.dbServer.GetParameterValue(storedProcCommand, "ServiceConsumptionAmount"));
                nvo.PharmacyConsumptionAmount = Convert.ToDecimal(this.dbServer.GetParameterValue(storedProcCommand, "PharmacyConsumptionAmount"));
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
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
            return valueObject;
        }

        public override IValueObject DeleteFiles(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetTotalBillAccountsLedgersVO svo = valueObject as clsGetTotalBillAccountsLedgersVO;
            string path = @"C:\XML\";
            try
            {
                foreach (string str2 in Directory.GetFiles(path, "*.xml"))
                {
                    File.Delete(str2);
                }
            }
            catch (DirectoryNotFoundException exception1)
            {
                Console.WriteLine(exception1.Message);
            }
            return svo;
        }

        public override IValueObject DeleteIsTempCharges(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddBillBizActionVO nvo = valueObject as clsAddBillBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                clsBillVO details = nvo.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteTempCharge");
                this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.Int64, details.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, details.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CostingDivisionID", DbType.Int64, details.CostingDivisionID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                transaction.Commit();
                nvo.SuccessStatus = 0;
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                transaction.Rollback();
                nvo.Details = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject FillGrossDiscountReason(IValueObject valueObject, clsUserVO UserVo)
        {
            clsFillGrossDiscountReasonBizActionVO nvo = valueObject as clsFillGrossDiscountReasonBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillDiscountReasonMaster");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        MasterListItem item = new MasterListItem {
                            ID = Convert.ToInt64(reader["ID"]),
                            Description = Convert.ToString(reader["Description"]),
                            Value = Convert.ToDouble(reader["DiscountPercentage"])
                        };
                        nvo.MasterList.Add(item);
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

        public override IValueObject GenerateXML(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetTotalBillAccountsLedgersVO svo = valueObject as clsGetTotalBillAccountsLedgersVO;
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            TextWriter textWriter = new StreamWriter(@"C:\XML\" + svo.strGenrateXMLName + ".xml");
            new XmlSerializer(typeof(List<ENVELOPE>)).Serialize(textWriter, svo.ENVELOPEList, namespaces);
            textWriter.Close();
            return svo;
        }

        public override IValueObject GetBillClearanceList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetBillClearanceBizActionVO nvo = valueObject as clsGetBillClearanceBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetBillClearanceListForSearch");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                if (nvo.IsFreeze != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FreezeBill", DbType.Boolean, nvo.IsFreeze);
                }
                this.dbServer.AddInParameter(storedProcCommand, "Refunded", DbType.Boolean, nvo.IsRefunded);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External_Id", DbType.Int64, nvo.Opd_Ipd_External_Id);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External", DbType.Int64, nvo.Opd_Ipd_External);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "OPDNO", DbType.String, nvo.OPDNO);
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo", DbType.String, nvo.ContactNo);
                this.dbServer.AddInParameter(storedProcCommand, "BillNO", DbType.String, nvo.BillNO);
                this.dbServer.AddInParameter(storedProcCommand, "MRNO", DbType.String, nvo.MRNO);
                this.dbServer.AddInParameter(storedProcCommand, "BillSettleType", DbType.Int16, nvo.BillStatus);
                this.dbServer.AddInParameter(storedProcCommand, "FromRefund", DbType.Boolean, nvo.FromRefund);
                this.dbServer.AddInParameter(storedProcCommand, "IsPaymentModeChange", DbType.Boolean, 1);
                this.dbServer.AddInParameter(storedProcCommand, "IsShowIPD", DbType.Boolean, 1);
                if (nvo.BillType != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "BillType", DbType.Int16, (short) nvo.BillType.Value);
                }
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
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsBillVO>();
                    }
                    while (reader.Read())
                    {
                        clsBillVO item = new clsBillVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Status = false,
                            Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"])),
                            IsCancelled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCancelled"])),
                            BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"])),
                            MRNO = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"])),
                            ContactNo = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo1"])),
                            BillType = (BillTypes) Convert.ToInt16(DALHelper.HandleDBNull(reader["BillType"])),
                            Opd_Ipd_External = Convert.ToInt16(DALHelper.HandleDBNull(reader["Opd_Ipd_External"])),
                            Opd_Ipd_External_Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"])),
                            Opd_Ipd_External_UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_UnitId"])),
                            TotalBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalBillAmount"])),
                            TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalConcessionAmount"])),
                            NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"])),
                            IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]))
                        };
                        if (nvo.IsIPDBillList)
                        {
                            item.AdmissionType = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionType"]));
                            item.IPDNO = Convert.ToString(DALHelper.HandleDBNull(reader["IPDNO"]));
                            item.ConcessionAuthorizedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConcessionAuthorizedBy"]));
                            item.TotalRefund = Convert.ToDouble(DALHelper.HandleDBNull(reader["RefundAmount"]));
                            Func<double, bool> myFunc = x => x != 0;
                            //item.IsRefunded = x => (x != 0.0)(item.TotalRefund);
                            item.IsRefunded = myFunc(item.TotalRefund);
                        }
                        else
                        {
                            item.VisitTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitTypeID"]));
                            item.TotalRefund = Convert.ToDouble(DALHelper.HandleDBNull(reader["RefundAmount"]));

                            Func<double, bool> myFunc = x => x != 0;
                            //item.IsRefunded = x => (x != 0.0)(item.TotalRefund);
                            item.IsRefunded = myFunc(item.TotalRefund);

                            item.Opd_Ipd_External_No = Convert.ToString(DALHelper.HandleDBNull(reader["OPDNO"]));
                            item.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        }
                        item.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        item.FirstName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])));
                        item.MiddleName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"])));
                        item.LastName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])));
                        item.BalanceAmountSelf = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmountSelf"]));
                        item.SelfAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["SelfAmount"]));
                        item.BillPaymentType = (BillPaymentTypes) Convert.ToInt16(DALHelper.HandleDBNull(reader["BillPaymentType"]));
                        item.GrossDiscountPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["GrossDiscountPercentage"]));
                        item.GrossDiscountReasonID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GrossDiscountReasonID"]));
                        item.IsComanyBillCancelled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCompanyBillCancelled"]));
                        item.PatientSourceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceId"]));
                        item.PatientAdvance = Convert.ToDouble(DALHelper.HandleDBNull(reader["PatientAdvance"]));
                        item.PackageAdvance = Convert.ToDouble(DALHelper.HandleDBNull(reader["PackageAdvance"]));
                        item.BalanceAdvance = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAdvance"]));
                        item.PacBilledCount = (Convert.ToInt64(DALHelper.HandleDBNull(reader["PacBilledCount"])) != 1L) ? "Not Done" : "Done";
                        item.SemenfreezingCount = (Convert.ToInt64(DALHelper.HandleDBNull(reader["SemenfreezingCount"])) != 1L) ? "Not Done" : "Done";
                        if (!nvo.IsIPDBillList)
                        {
                            item.BalanceAmountSelf = (Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"])) || (item.BillType == BillTypes.Pharmacy)) ? Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmountSelf"])) : Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"]));
                        }
                        if (nvo.Opd_Ipd_External == 1)
                        {
                            item.ISDischarged = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ISDischarged"]));
                        }
                        nvo.List.Add(item);
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

        public override IValueObject GetBillListForRequestApproval(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetBillSearchListBizActionVO nvo = valueObject as clsGetBillSearchListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetBillListForRequestApprovalWindow");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                if (nvo.IsFreeze != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FreezeBill", DbType.Boolean, nvo.IsFreeze);
                }
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External_Id", DbType.Int64, nvo.Opd_Ipd_External_Id);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External", DbType.Int64, nvo.Opd_Ipd_External);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "OPDNO", DbType.String, nvo.OPDNO);
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo", DbType.String, nvo.ContactNo);
                this.dbServer.AddInParameter(storedProcCommand, "BillNO", DbType.String, nvo.BillNO);
                this.dbServer.AddInParameter(storedProcCommand, "MRNO", DbType.String, nvo.MRNO);
                this.dbServer.AddInParameter(storedProcCommand, "BillSettleType", DbType.Int16, nvo.BillStatus);
                this.dbServer.AddInParameter(storedProcCommand, "FromRefund", DbType.Boolean, nvo.FromRefund);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDBill", DbType.Boolean, nvo.IsOPDBill);
                if (nvo.BillType != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "BillType", DbType.Int16, (short) nvo.BillType.Value);
                }
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
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsBillVO>();
                    }
                    while (reader.Read())
                    {
                        clsBillVO item = new clsBillVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"])),
                            IsCancelled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCancelled"])),
                            BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"])),
                            MRNO = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"])),
                            ContactNo = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo1"])),
                            BillType = (BillTypes) Convert.ToInt16(DALHelper.HandleDBNull(reader["BillType"])),
                            Opd_Ipd_External = Convert.ToInt16(DALHelper.HandleDBNull(reader["Opd_Ipd_External"])),
                            Opd_Ipd_External_Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"])),
                            Opd_Ipd_External_UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_UnitId"])),
                            TotalBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalBillAmount"])),
                            TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalConcessionAmount"])),
                            NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"])),
                            IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]))
                        };
                        if (nvo.IsOPDBill)
                        {
                            item.VisitTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitTypeID"]));
                            item.Opd_Ipd_External_No = Convert.ToString(DALHelper.HandleDBNull(reader["OPDNO"]));
                        }
                        else
                        {
                            item.AdmissionType = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionType"]));
                            item.Opd_Ipd_External_No = Convert.ToString(DALHelper.HandleDBNull(reader["IPDNO"]));
                        }
                        item.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        item.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        item.FirstName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])));
                        item.MiddleName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"])));
                        item.LastName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])));
                        item.BalanceAmountSelf = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmountSelf"]));
                        item.SelfAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["SelfAmount"]));
                        item.PatientSourceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceId"]));
                        item.CostingDivisionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CostingDivisionID"]));
                        item.RequestType = Convert.ToString(DALHelper.HandleDBNull(reader["RequestType"]));
                        item.RequestTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RequestTypeID"]));
                        item.ApprovalRequestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalRequestID"]));
                        item.ApprovalRequestUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalRequestUnitID"]));
                        item.LevelID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LevelID"]));
                        item.ApprovalRemark = Convert.ToString(DALHelper.HandleDBNull(reader["ApprovalRemark"]));
                        item.IsOPDBill = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsOPDBill"]));
                        item.ConcessionReasonId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConcessionReasonId"]));
                        item.ConcessionRemark = Convert.ToString(DALHelper.HandleDBNull(reader["ConcessionRemark"]));
                        nvo.List.Add(item);
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

        public override IValueObject GetBillSearch_IVF_List_DashBoard(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetBillSearch_IVF_List_DashBoardBizActionVO nvo = valueObject as clsGetBillSearch_IVF_List_DashBoardBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetBillListForSearch_DashBoard");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "MRNO", DbType.String, nvo.MRNO);
                this.dbServer.AddInParameter(storedProcCommand, "BillType", DbType.Int32, nvo.BillType);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.String, nvo.UnitID);
                if ((nvo.FirstName != null) && (nvo.FirstName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, Security.base64Encode(nvo.FirstName));
                }
                if ((nvo.LastName != null) && (nvo.LastName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, Security.base64Encode(nvo.LastName));
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
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsBillVO>();
                    }
                    while (reader.Read())
                    {
                        clsBillVO item = new clsBillVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        DateTime? nullable7 = DALHelper.HandleDate(reader["Date"]);
                        item.Date = nullable7.Value;
                        item.IsCancelled = (bool) DALHelper.HandleDBNull(reader["IsCancelled"]);
                        item.BillNo = (string) DALHelper.HandleDBNull(reader["BillNo"]);
                        item.MRNO = (string) DALHelper.HandleDBNull(reader["MRNO"]);
                        item.BillType = (BillTypes) ((short) DALHelper.HandleDBNull(reader["BillType"]));
                        item.TotalBillAmount = (double) DALHelper.HandleDBNull(reader["TotalBillAmount"]);
                        item.TotalConcessionAmount = (double) DALHelper.HandleDBNull(reader["TotalConcessionAmount"]);
                        item.NetBillAmount = (double) DALHelper.HandleDBNull(reader["NetBillAmount"]);
                        item.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                        item.Opd_Ipd_External_No = (string) DALHelper.HandleDBNull(reader["OPDNO"]);
                        item.IsFreezed = (bool) DALHelper.HandleDBNull(reader["IsFreezed"]);
                        item.VisitTypeID = (long) DALHelper.HandleDBNull(reader["VisitTypeID"]);
                        item.PatientID = (long) DALHelper.HandleDBNull(reader["PatientID"]);
                        item.FirstName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["FirstName"]));
                        item.MiddleName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["MiddleName"]));
                        item.LastName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["LastName"]));
                        item.BalanceAmountSelf = (double) DALHelper.HandleDBNull(reader["BalanceAmountSelf"]);
                        item.SelfAmount = (double) DALHelper.HandleDBNull(reader["SelfAmount"]);
                        item.BillPaymentType = (BillPaymentTypes) ((short) DALHelper.HandleDBNull(reader["BillPaymentType"]));
                        item.TotalRefund = (double) DALHelper.HandleDBNull(reader["RefundAmount"]);
                        nvo.List.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject GetBillSearch_USG_List_DashBoard(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetBillSearch_USG_List_DashBoardBizActionVO nvo = valueObject as clsGetBillSearch_USG_List_DashBoardBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetBillListForSearch_DashBoard");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "MRNO", DbType.String, nvo.MRNO);
                this.dbServer.AddInParameter(storedProcCommand, "BillType", DbType.Int32, nvo.BillType);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.String, nvo.UnitID);
                if ((nvo.FirstName != null) && (nvo.FirstName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, Security.base64Encode(nvo.FirstName));
                }
                if ((nvo.LastName != null) && (nvo.LastName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, Security.base64Encode(nvo.LastName));
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
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsBillVO>();
                    }
                    while (reader.Read())
                    {
                        clsBillVO item = new clsBillVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        DateTime? nullable7 = DALHelper.HandleDate(reader["Date"]);
                        item.Date = nullable7.Value;
                        item.IsCancelled = (bool) DALHelper.HandleDBNull(reader["IsCancelled"]);
                        item.BillNo = (string) DALHelper.HandleDBNull(reader["BillNo"]);
                        item.MRNO = (string) DALHelper.HandleDBNull(reader["MRNO"]);
                        item.BillType = (BillTypes) ((short) DALHelper.HandleDBNull(reader["BillType"]));
                        item.TotalBillAmount = (double) DALHelper.HandleDBNull(reader["TotalBillAmount"]);
                        item.TotalConcessionAmount = (double) DALHelper.HandleDBNull(reader["TotalConcessionAmount"]);
                        item.NetBillAmount = (double) DALHelper.HandleDBNull(reader["NetBillAmount"]);
                        item.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                        item.Opd_Ipd_External_No = (string) DALHelper.HandleDBNull(reader["OPDNO"]);
                        item.IsFreezed = (bool) DALHelper.HandleDBNull(reader["IsFreezed"]);
                        item.VisitTypeID = (long) DALHelper.HandleDBNull(reader["VisitTypeID"]);
                        item.PatientID = (long) DALHelper.HandleDBNull(reader["PatientID"]);
                        item.FirstName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["FirstName"]));
                        item.MiddleName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["MiddleName"]));
                        item.LastName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["LastName"]));
                        item.BalanceAmountSelf = (double) DALHelper.HandleDBNull(reader["BalanceAmountSelf"]);
                        item.SelfAmount = (double) DALHelper.HandleDBNull(reader["SelfAmount"]);
                        item.BillPaymentType = (BillPaymentTypes) ((short) DALHelper.HandleDBNull(reader["BillPaymentType"]));
                        item.TotalRefund = (double) DALHelper.HandleDBNull(reader["RefundAmount"]);
                        nvo.List.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject GetCompanyCreditDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCompanyCreditDtlsBizActionVO nvo = (clsGetCompanyCreditDtlsBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCompanyCreditDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Details == null)
                    {
                        nvo.Details = new clsCompanyCreditDetailsVO();
                    }
                    while (reader.Read())
                    {
                        try
                        {
                            nvo.Details.Balance = (double) DALHelper.HandleDBNull(reader["Balance"]);
                            nvo.Details.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                            nvo.Details.CreditLimit = (double) DALHelper.HandleDBNull(reader["CreditLimit"]);
                            nvo.Details.Refund = (double) DALHelper.HandleDBNull(reader["Refund"]);
                            nvo.Details.TotalAdvance = (double) DALHelper.HandleDBNull(reader["TotalAdvance"]);
                            nvo.Details.Used = (double) DALHelper.HandleDBNull(reader["Used"]);
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

        public override IValueObject GetDailyCollection(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDailyCollectionListBizActionVO nvo = valueObject as clsGetDailyCollectionListBizActionVO;
            try
            {
                if (nvo.DailySales)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_Chart_DailyCollectionReportSubReport");
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitID);
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.List == null)
                        {
                            nvo.List = new List<clsDailyCollectionReportVO>();
                        }
                        while (reader.Read())
                        {
                            clsDailyCollectionReportVO item = new clsDailyCollectionReportVO {
                                PaymentModeID = (MaterPayModeList) ((int) ((long) DALHelper.HandleDBNull(reader["PaymentModeID"])))
                            };
                            DateTime? nullable = DALHelper.HandleDate(reader["Date"]);
                            item.Date = nullable.Value;
                            item.Collection = (double) DALHelper.HandleDBNull(reader["Collection"]);
                            nvo.List.Add(item);
                        }
                    }
                    reader.Close();
                }
                if (!nvo.DailySales)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_Chart_DailySalesReport");
                    if (nvo.CollectionDate != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "CollectionDate", DbType.DateTime, nvo.CollectionDate);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitID);
                    DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader2.HasRows)
                    {
                        if (nvo.SalesList == null)
                        {
                            nvo.SalesList = new List<clsDailySalesReportVO>();
                        }
                        while (reader2.Read())
                        {
                            clsDailySalesReportVO item = new clsDailySalesReportVO {
                                TotalAmount = (double) DALHelper.HandleDBNull(reader2["TotalAmount"]),
                                Specialization = (string) DALHelper.HandleDBNull(reader2["Specialization"])
                            };
                            nvo.SalesList.Add(item);
                        }
                    }
                    reader2.Close();
                }
                if (nvo.ISAppointmentList)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DashboardAppointment");
                    if (nvo.CollectionDate != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.CollectionDate);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitID);
                    DbDataReader reader3 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader3.HasRows)
                    {
                        if (nvo.AppointmentList == null)
                        {
                            nvo.AppointmentList = new List<clsAppointmentVO>();
                        }
                        while (reader3.Read())
                        {
                            clsAppointmentVO item = new clsAppointmentVO {
                                AppointmentReasonId = (long) DALHelper.HandleDBNull(reader3["AppointmentReasonID"]),
                                AppointmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["AppointmentID"])),
                                AppointmentReason = Convert.ToString(DALHelper.HandleDBNull(reader3["Appointment"]))
                            };
                            nvo.AppointmentList.Add(item);
                        }
                    }
                    reader3.Close();
                }
                if (nvo.IsVisit)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DashboardVisit");
                    if (nvo.CollectionDate != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.CollectionDate);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitID);
                    DbDataReader reader4 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader4.HasRows)
                    {
                        if (nvo.VisitList == null)
                        {
                            nvo.VisitList = new List<clsVisitVO>();
                        }
                        while (reader4.Read())
                        {
                            clsVisitVO item = new clsVisitVO {
                                VisitType = (string) DALHelper.HandleDBNull(reader4["VisitDesc"]),
                                VisitTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader4["VisitTypeID"])),
                                VisitID = Convert.ToInt64(DALHelper.HandleDBNull(reader4["NoVisit"]))
                            };
                            nvo.VisitList.Add(item);
                        }
                    }
                    reader4.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetDoctorShareRangeList(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetDoctorShareRangeList list = valueObject as clsGetDoctorShareRangeList;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorShareRangeList");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (list.ShareRangeList == null)
                    {
                        list.ShareRangeList = new List<clsDoctorShareRangeVO>();
                    }
                    while (reader.Read())
                    {
                        clsDoctorShareRangeVO item = new clsDoctorShareRangeVO {
                            ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"])),
                            UpperLimit = Convert.ToDecimal(DALHelper.HandleDBNull(reader["UpperLimit"])),
                            LowerLimit = Convert.ToDecimal(DALHelper.HandleDBNull(reader["LowerLimit"])),
                            SharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SharePercentage"])),
                            ShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ShareAmount"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        list.ShareRangeList.Add(item);
                    }
                }
                reader.NextResult();
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return list;
        }

        public override IValueObject GetFreezedList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetFreezedBillListBizActionVO nvo = valueObject as clsGetFreezedBillListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetBillListAgainstBillID");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.String, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.Int64, nvo.BillID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsBillVO>();
                    }
                    while (reader.Read())
                    {
                        clsBillVO item = new clsBillVO {
                            Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"])),
                            BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"])),
                            PaidAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PaidAmount"])),
                            NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"])),
                            PaymentMode = Convert.ToString(DALHelper.HandleDBNull(reader["PaymentMode"])),
                            PaymentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PaymentID"])),
                            PaymentModeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PaymentModeId"])),
                            Number = Convert.ToString(DALHelper.HandleDBNull(reader["Number"])),
                            Bank = Convert.ToString(DALHelper.HandleDBNull(reader["Bank"])),
                            UnitID = nvo.UnitID,
                            PaymentDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PaymentDetailId"])),
                            BankID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BankID"]))
                        };
                        nvo.List.Add(item);
                    }
                }
                reader.NextResult();
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetFreezedSearchList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetFreezedBillSearchListBizActionVO nvo = valueObject as clsGetFreezedBillSearchListBizActionVO;
            try
            {
                DbCommand storedProcCommand;
                if (!nvo.IsIPDBillList)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetFreezedBillListForSearch");
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIPDFreezedBillListForSearch");
                    this.dbServer.AddInParameter(storedProcCommand, "IPDNO", DbType.String, nvo.IPDNO);
                }
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                if (nvo.IsFreeze != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FreezeBill", DbType.Boolean, nvo.IsFreeze);
                }
                this.dbServer.AddInParameter(storedProcCommand, "Refunded", DbType.Boolean, nvo.IsRefunded);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External_Id", DbType.Int64, nvo.Opd_Ipd_External_Id);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External", DbType.Int64, nvo.Opd_Ipd_External);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "OPDNO", DbType.String, nvo.OPDNO);
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo", DbType.String, nvo.ContactNo);
                this.dbServer.AddInParameter(storedProcCommand, "BillNO", DbType.String, nvo.BillNO);
                this.dbServer.AddInParameter(storedProcCommand, "MRNO", DbType.String, nvo.MRNO);
                this.dbServer.AddInParameter(storedProcCommand, "BillSettleType", DbType.Int16, nvo.BillStatus);
                this.dbServer.AddInParameter(storedProcCommand, "FromRefund", DbType.Boolean, nvo.FromRefund);
                this.dbServer.AddInParameter(storedProcCommand, "IsPaymentModeChange", DbType.Boolean, nvo.IsPaymentModeChange);
                if (nvo.BillType != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "BillType", DbType.Int16, (short) nvo.BillType.Value);
                }
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
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsBillVO>();
                    }
                    while (reader.Read())
                    {
                        clsBillVO item = new clsBillVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"])),
                            IsCancelled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCancelled"])),
                            BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"])),
                            MRNO = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"])),
                            ContactNo = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo1"])),
                            BillType = (BillTypes) Convert.ToInt16(DALHelper.HandleDBNull(reader["BillType"])),
                            Opd_Ipd_External = Convert.ToInt16(DALHelper.HandleDBNull(reader["Opd_Ipd_External"])),
                            Opd_Ipd_External_Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"])),
                            Opd_Ipd_External_UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_UnitId"])),
                            TotalBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalBillAmount"])),
                            TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalConcessionAmount"])),
                            NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"])),
                            IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]))
                        };
                        if (!nvo.IsIPDBillList)
                        {
                            item.VisitTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitTypeID"]));
                            item.TotalRefund = Convert.ToDouble(DALHelper.HandleDBNull(reader["RefundAmount"]));
                            Func<double, bool> myFunc = x => x != 0;
                            //item.IsRefunded = x => (x != 0.0)(item.TotalRefund);
                            item.IsRefunded = myFunc(item.TotalRefund);

                            item.Opd_Ipd_External_No = Convert.ToString(DALHelper.HandleDBNull(reader["OPDNO"]));
                            item.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        }
                        else
                        {
                            item.AdmissionType = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionType"]));
                            item.IPDNO = Convert.ToString(DALHelper.HandleDBNull(reader["IPDNO"]));
                            item.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                            item.ConcessionAuthorizedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConcessionAuthorizedBy"]));
                            item.TotalRefund = Convert.ToDouble(DALHelper.HandleDBNull(reader["RefundAmount"]));
                            Func<double, bool> myFunc = x => x != 0;
                            //item.IsRefunded = x => (x != 0.0)(item.TotalRefund);
                            item.IsRefunded = myFunc(item.TotalRefund);

                        }
                        item.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        item.FirstName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])));
                        item.MiddleName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"])));
                        item.LastName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])));
                        item.BalanceAmountSelf = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmountSelf"]));
                        item.SelfAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["SelfAmount"]));
                        item.BillPaymentType = (BillPaymentTypes) Convert.ToInt16(DALHelper.HandleDBNull(reader["BillPaymentType"]));
                        item.GrossDiscountPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["GrossDiscountPercentage"]));
                        item.GrossDiscountReasonID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GrossDiscountReasonID"]));
                        item.IsComanyBillCancelled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCompanyBillCancelled"]));
                        item.PatientSourceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceId"]));
                        item.CostingDivisionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CostingDivisionID"]));
                        item.IsModify = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsChange"]));
                        nvo.List.Add(item);
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

        public override IValueObject GetPatientPackageServiceDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPackageDetailsBizActionVO nvo = (clsGetPackageDetailsBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientPackageServiceListNewForPackage");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                if ((nvo.ServiceName != null) && (nvo.ServiceName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceName", DbType.String, nvo.ServiceName);
                }
                this.dbServer.AddInParameter(storedProcCommand, "Specialization", DbType.Int64, nvo.Specialization);
                this.dbServer.AddInParameter(storedProcCommand, "SubSpecialization", DbType.Int64, nvo.SubSpecialization);
                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, nvo.TariffID);
                this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int64, nvo.PackageID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Pending", DbType.Boolean, true);
                if ((nvo.SearchExpression != null) && (nvo.SearchExpression.Length > 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.SearchExpression);
                }
                this.dbServer.AddInParameter(storedProcCommand, "SponsorID", DbType.Int64, nvo.SponsorID);
                this.dbServer.AddInParameter(storedProcCommand, "SponsorUnitID", DbType.Int64, nvo.SponsorUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PendingServiceList == null)
                    {
                        nvo.PendingServiceList = new List<clsServiceMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsServiceMasterVO item = new clsServiceMasterVO {
                            ID = Convert.ToInt64(reader["ServiceID"]),
                            ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"])),
                            ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"])),
                            TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"])),
                            Specialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationID"])),
                            SubSpecialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"])),
                            Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"])),
                            SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["Specialization"])),
                            SubSpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecialization"])),
                            IsPackage = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackage"])),
                            PackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageID"])),
                            PackageServiceConditionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageServiceConditionID"]))
                        };
                        nvo.PendingServiceList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientPackageUsedServiceListNewForPackage");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, 0);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                if ((nvo.ServiceName != null) && (nvo.ServiceName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceName", DbType.String, nvo.ServiceName);
                }
                this.dbServer.AddInParameter(storedProcCommand, "Specialization", DbType.Int64, nvo.Specialization);
                this.dbServer.AddInParameter(storedProcCommand, "SubSpecialization", DbType.Int64, nvo.SubSpecialization);
                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, nvo.TariffID);
                this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int64, nvo.PackageID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                if ((nvo.SearchExpression != null) && (nvo.SearchExpression.Length > 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.SearchExpression);
                }
                this.dbServer.AddInParameter(storedProcCommand, "SponsorID", DbType.Int64, nvo.SponsorID);
                this.dbServer.AddInParameter(storedProcCommand, "SponsorUnitID", DbType.Int64, nvo.SponsorUnitID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.AvailedServiceList == null)
                    {
                        nvo.AvailedServiceList = new List<clsServiceMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsServiceMasterVO item = new clsServiceMasterVO {
                            TariffServiceMasterID = Convert.ToInt64(reader["ID"]),
                            ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"])),
                            TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"])),
                            Specialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationID"])),
                            SubSpecialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"])),
                            Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"])),
                            SeniorCitizen = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["SeniorCitizen"])),
                            SeniorCitizenConAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SeniorCitizenConAmount"])),
                            SeniorCitizenConPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SeniorCitizenConPercent"])),
                            SeniorCitizenAge = Convert.ToInt32(DALHelper.HandleDBNull(reader["SeniorCitizenAge"]))
                        };
                        if (item.SeniorCitizen && (nvo.Age >= item.SeniorCitizenAge))
                        {
                            item.ConcessionAmount = item.SeniorCitizenConAmount;
                            item.ConcessionPercent = item.SeniorCitizenConPercent;
                        }
                        else
                        {
                            item.ConcessionAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                            item.ConcessionPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                        }
                        item.StaffDiscount = Convert.ToBoolean(DALHelper.HandleDBNull(reader["StaffDiscount"]));
                        item.StaffDiscountAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDiscountAmount"]));
                        item.StaffDiscountPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDiscountPercent"]));
                        item.StaffDependantDiscount = Convert.ToBoolean(DALHelper.HandleDBNull(reader["StaffDependantDiscount"]));
                        item.StaffDependantDiscountAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDependantDiscountAmount"]));
                        item.StaffDependantDiscountPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDependantDiscountPercent"]));
                        item.Concession = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Concession"]));
                        item.ServiceTax = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ServiceTax"]));
                        item.ServiceTaxAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceTaxAmount"]));
                        item.ServiceTaxPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceTaxPercent"]));
                        item.InHouse = Convert.ToBoolean(DALHelper.HandleDBNull(reader["InHouse"]));
                        item.RateEditable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["RateEditable"]));
                        item.MaxRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MaxRate"]));
                        item.MinRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MinRate"]));
                        item.TarrifCode = Convert.ToString(DALHelper.HandleDBNull(reader["TarrifServiceCode"]));
                        item.TarrifName = Convert.ToString(DALHelper.HandleDBNull(reader["TarrifServiceName"]));
                        item.SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["Specialization"]));
                        item.SubSpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecialization"]));
                        item.IsPackage = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackage"]));
                        item.PackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageID"]));
                        item.PackageServiceConditionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageServiceConditionID"]));
                        item.ExpiryDate = new DateTime?(Convert.ToDateTime(Convert.ToDateTime(DALHelper.HandleDate(reader["UsedDate"])).ToShortDateString()));
                        item.UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["SerConUnitName"]));
                        item.Patient_Name = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        item.Package_Name = Convert.ToString(DALHelper.HandleDBNull(reader["PackageName"]));
                        nvo.AvailedServiceList.Add(item);
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

        public override IValueObject GetPharmacyBillSearchList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsPreviousPatientBillsBizActionVO nvo = valueObject as clsPreviousPatientBillsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPharmacyBillListForSearch");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "IsPharmacyQueue", DbType.Boolean, nvo.IsPharmacyQueue);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsBillVO>();
                    }
                    while (reader.Read())
                    {
                        clsBillVO item = new clsBillVO();
                        if (!nvo.IsPharmacyQueue)
                        {
                            item.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                            item.IsCancelled = (bool) DALHelper.HandleDBNull(reader["IsCancelled"]);
                            item.Opd_Ipd_External = (short) DALHelper.HandleDBNull(reader["Opd_Ipd_External"]);
                            item.BalanceAmountSelf = (double) DALHelper.HandleDBNull(reader["BalanceAmountSelf"]);
                            item.SelfAmount = (double) DALHelper.HandleDBNull(reader["SelfAmount"]);
                            item.TotalRefund = (double) DALHelper.HandleDBNull(reader["RefundAmount"]);
                            item.PatientSourceId = DALHelper.HandleIntegerNull(reader["PatientSourceId"]);
                            item.PatientCategoryId = DALHelper.HandleIntegerNull(reader["PatientCategoryId"]);
                        }
                        bool isPharmacyQueue = nvo.IsPharmacyQueue;
                        item.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        item.UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]);
                        item.Date = DALHelper.HandleDate(reader["Date"]).Value;
                        item.BillNo = (string) DALHelper.HandleDBNull(reader["BillNo"]);
                        item.MRNO = (string) DALHelper.HandleDBNull(reader["MRNO"]);
                        item.BillType = (BillTypes) ((short) DALHelper.HandleDBNull(reader["BillType"]));
                        item.Opd_Ipd_External_Id = (long) DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"]);
                        item.Opd_Ipd_External_UnitId = (long) DALHelper.HandleDBNull(reader["Opd_Ipd_External_UnitId"]);
                        item.TotalBillAmount = (double) DALHelper.HandleDBNull(reader["TotalBillAmount"]);
                        item.TotalConcessionAmount = (double) DALHelper.HandleDBNull(reader["TotalConcessionAmount"]);
                        item.NetBillAmount = (double) DALHelper.HandleDBNull(reader["NetBillAmount"]);
                        item.Opd_Ipd_External_No = (string) DALHelper.HandleDBNull(reader["OPDNO"]);
                        item.IsFreezed = (bool) DALHelper.HandleDBNull(reader["IsFreezed"]);
                        item.VisitTypeID = DALHelper.HandleIntegerNull(reader["VisitTypeID"]);
                        item.PatientID = DALHelper.HandleIntegerNull(reader["PatientID"]);
                        item.FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]));
                        item.MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"]));
                        item.LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]));
                        item.BillPaymentType = (BillPaymentTypes) ((short) DALHelper.HandleDBNull(reader["BillPaymentType"]));
                        nvo.List.Add(item);
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

        private clsRadOrderBookingDetailsVO GetRadilogyTestDetails(long pServiceID, clsUserVO UserVo, long rOBID, long rOBUnitID)
        {
            clsRadOrderBookingDetailsVO svo = null;
            try
            {
                clsBaseRadiologyDAL instance = clsBaseRadiologyDAL.GetInstance();
                clsGetRadTestDetailsBizActionVO valueObject = new clsGetRadTestDetailsBizActionVO {
                    ServiceID = pServiceID,
                    robID = rOBID,
                    robUnitID = rOBUnitID
                };
                valueObject = (clsGetRadTestDetailsBizActionVO) instance.GetTestListWithDetailsID(valueObject, UserVo);
                svo = new clsRadOrderBookingDetailsVO();
                if (((valueObject != null) && (valueObject.TestList != null)) && (valueObject.TestList.Count > 0))
                {
                    svo.TestID = valueObject.TestList[0].TestID;
                    svo.ID = valueObject.TestList[0].ROBDID;
                }
            }
            catch (Exception)
            {
            }
            return svo;
        }

        public override IValueObject GetSaveBillClearanceList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetBillClearanceBizActionVO nvo = valueObject as clsGetBillClearanceBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetBillClearanceListForSave");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                if (nvo.IsFreeze != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FreezeBill", DbType.Boolean, nvo.IsFreeze);
                }
                this.dbServer.AddInParameter(storedProcCommand, "Refunded", DbType.Boolean, nvo.IsRefunded);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External_Id", DbType.Int64, nvo.Opd_Ipd_External_Id);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External", DbType.Int64, nvo.Opd_Ipd_External);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "OPDNO", DbType.String, nvo.OPDNO);
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo", DbType.String, nvo.ContactNo);
                this.dbServer.AddInParameter(storedProcCommand, "BillNO", DbType.String, nvo.BillNO);
                this.dbServer.AddInParameter(storedProcCommand, "MRNO", DbType.String, nvo.MRNO);
                this.dbServer.AddInParameter(storedProcCommand, "BillSettleType", DbType.Int16, nvo.BillStatus);
                this.dbServer.AddInParameter(storedProcCommand, "FromRefund", DbType.Boolean, nvo.FromRefund);
                this.dbServer.AddInParameter(storedProcCommand, "IsPaymentModeChange", DbType.Boolean, 1);
                this.dbServer.AddInParameter(storedProcCommand, "IsShowIPD", DbType.Boolean, 1);
                if (nvo.BillType != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "BillType", DbType.Int16, (short) nvo.BillType.Value);
                }
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
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsBillVO>();
                    }
                    while (reader.Read())
                    {
                        clsBillVO item = new clsBillVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Status = false,
                            Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"])),
                            IsCancelled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCancelled"])),
                            BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"])),
                            MRNO = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"])),
                            ContactNo = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo1"])),
                            BillType = (BillTypes) Convert.ToInt16(DALHelper.HandleDBNull(reader["BillType"])),
                            Opd_Ipd_External = Convert.ToInt16(DALHelper.HandleDBNull(reader["Opd_Ipd_External"])),
                            Opd_Ipd_External_Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"])),
                            Opd_Ipd_External_UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_UnitId"])),
                            TotalBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalBillAmount"])),
                            TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalConcessionAmount"])),
                            NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"])),
                            IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]))
                        };
                        if (nvo.IsIPDBillList)
                        {
                            item.AdmissionType = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionType"]));
                            item.IPDNO = Convert.ToString(DALHelper.HandleDBNull(reader["IPDNO"]));
                            item.ConcessionAuthorizedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConcessionAuthorizedBy"]));
                            item.TotalRefund = Convert.ToDouble(DALHelper.HandleDBNull(reader["RefundAmount"]));
                            Func<double, bool> myFunc = x => x != 0;
                            //item.IsRefunded = x => (x != 0.0)(item.TotalRefund);
                            item.IsRefunded = myFunc(item.TotalRefund);
                        }
                        else
                        {
                            item.VisitTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitTypeID"]));
                            item.TotalRefund = Convert.ToDouble(DALHelper.HandleDBNull(reader["RefundAmount"]));
                            Func<double, bool> myFunc = x => x != 0;
                            //item.IsRefunded = x => (x != 0.0)(item.TotalRefund);
                            item.IsRefunded = myFunc(item.TotalRefund);

                            item.Opd_Ipd_External_No = Convert.ToString(DALHelper.HandleDBNull(reader["OPDNO"]));
                            item.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        }
                        item.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        item.FirstName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])));
                        item.MiddleName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"])));
                        item.LastName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])));
                        item.BalanceAmountSelf = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmountSelf"]));
                        item.SelfAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["SelfAmount"]));
                        item.BillPaymentType = (BillPaymentTypes) Convert.ToInt16(DALHelper.HandleDBNull(reader["BillPaymentType"]));
                        item.GrossDiscountPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["GrossDiscountPercentage"]));
                        item.GrossDiscountReasonID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GrossDiscountReasonID"]));
                        item.IsComanyBillCancelled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCompanyBillCancelled"]));
                        item.PatientSourceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceId"]));
                        item.PatientAdvance = Convert.ToDouble(DALHelper.HandleDBNull(reader["PatientAdvance"]));
                        item.PackageAdvance = Convert.ToDouble(DALHelper.HandleDBNull(reader["PackageAdvance"]));
                        item.BalanceAdvance = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAdvance"]));
                        item.PacBilledCount = (Convert.ToInt64(DALHelper.HandleDBNull(reader["PacBilledCount"])) != 1L) ? "Not Done" : "Done";
                        item.SemenfreezingCount = (Convert.ToInt64(DALHelper.HandleDBNull(reader["SemenfreezingCount"])) != 1L) ? "Not Done" : "Done";
                        if (!nvo.IsIPDBillList)
                        {
                            item.BalanceAmountSelf = (Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"])) || (item.BillType == BillTypes.Pharmacy)) ? Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmountSelf"])) : Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"]));
                        }
                        if (nvo.Opd_Ipd_External == 1)
                        {
                            item.ISDischarged = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ISDischarged"]));
                        }
                        item.OTBillClearanceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTBillClearanceID"]));
                        nvo.List.Add(item);
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

        public override IValueObject GetSearchList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetBillSearchListBizActionVO nvo = valueObject as clsGetBillSearchListBizActionVO;
            try
            {
                DbCommand storedProcCommand;
                if (!nvo.IsIPDBillList)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetBillListForSearch");
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIPDBillListForSearch");
                    this.dbServer.AddInParameter(storedProcCommand, "IPDNO", DbType.String, nvo.IPDNO);
                }
                this.dbServer.AddInParameter(storedProcCommand, "IsRequest", DbType.Boolean, nvo.IsRequest);
                this.dbServer.AddInParameter(storedProcCommand, "RequestTypeID", DbType.Int64, nvo.RequestTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                if (nvo.IsFreeze != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FreezeBill", DbType.Boolean, nvo.IsFreeze);
                }
                this.dbServer.AddInParameter(storedProcCommand, "Refunded", DbType.Boolean, nvo.IsRefunded);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External_Id", DbType.Int64, nvo.Opd_Ipd_External_Id);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External", DbType.Int64, nvo.Opd_Ipd_External);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "OPDNO", DbType.String, nvo.OPDNO);
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo", DbType.String, nvo.ContactNo);
                this.dbServer.AddInParameter(storedProcCommand, "BillNO", DbType.String, nvo.BillNO);
                this.dbServer.AddInParameter(storedProcCommand, "MRNO", DbType.String, nvo.MRNO);
                this.dbServer.AddInParameter(storedProcCommand, "BillSettleType", DbType.Int16, nvo.BillStatus);
                this.dbServer.AddInParameter(storedProcCommand, "FromRefund", DbType.Boolean, nvo.FromRefund);
                if (nvo.BillType != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "BillType", DbType.Int16, (short) nvo.BillType.Value);
                }
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
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "@IsConsumption", DbType.Boolean, nvo.IsConsumption);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsBillVO>();
                    }
                    while (reader.Read())
                    {
                        clsBillVO item = new clsBillVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"])),
                            IsCancelled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCancelled"])),
                            BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"])),
                            MRNO = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"])),
                            ContactNo = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo1"])),
                            BillType = (BillTypes) Convert.ToInt16(DALHelper.HandleDBNull(reader["BillType"])),
                            Opd_Ipd_External = Convert.ToInt16(DALHelper.HandleDBNull(reader["Opd_Ipd_External"])),
                            Opd_Ipd_External_Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"])),
                            Opd_Ipd_External_UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_UnitId"])),
                            TotalBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalBillAmount"])),
                            TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalConcessionAmount"])),
                            NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"])),
                            IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]))
                        };
                        if (nvo.IsIPDBillList)
                        {
                            item.AdmissionType = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionType"]));
                            item.IPDNO = Convert.ToString(DALHelper.HandleDBNull(reader["IPDNO"]));
                            item.ConcessionAuthorizedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConcessionAuthorizedBy"]));
                            item.TotalRefund = Convert.ToDouble(DALHelper.HandleDBNull(reader["RefundAmount"]));
                            Func<double, bool> myFunc = x => x != 0;
                            //item.IsRefunded = x => (x != 0.0)(item.TotalRefund);
                            item.IsRefunded = myFunc(item.TotalRefund);
                        }
                        else
                        {
                            item.VisitTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitTypeID"]));
                            item.TotalRefund = Convert.ToDouble(DALHelper.HandleDBNull(reader["RefundAmount"]));
                            Func<double, bool> myFunc = x => x != 0;
                            //item.IsRefunded = x => (x != 0.0)(item.TotalRefund);
                            item.IsRefunded = myFunc(item.TotalRefund);

                            item.Opd_Ipd_External_No = Convert.ToString(DALHelper.HandleDBNull(reader["OPDNO"]));
                            item.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                            item.ConcessionAuthorizedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConcessionAuthorizedBy"]));
                            item.PatientCategoryId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientCategoryId"]));
                            item.IsPackageConsumption = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackageConsumption"]));
                            item.PackageBillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageBillID"]));
                            item.PackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageID"]));
                            item.PackageConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PackageConcessionAmount"]));
                            item.IsAdjustableBillDone = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAdjustableBillDone"]));
                            item.IsAllBillSettle = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAllBillSettle"]));
                            item.PackageSettleRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["PackageSettleRate"]));
                            item.LinkPatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LinkPatientID"]));
                            item.LinkPatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LinkPatientUnitID"]));
                        }
                        item.IsPackageServiceInclude = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackageServiceInclude"]));
                        item.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        item.FirstName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])));
                        item.MiddleName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"])));
                        item.LastName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])));
                        item.BalanceAmountSelf = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmountSelf"]));
                        item.SelfAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["SelfAmount"]));
                        item.BillPaymentType = (BillPaymentTypes) Convert.ToInt16(DALHelper.HandleDBNull(reader["BillPaymentType"]));
                        item.GrossDiscountPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["GrossDiscountPercentage"]));
                        item.GrossDiscountReasonID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GrossDiscountReasonID"]));
                        item.IsComanyBillCancelled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCompanyBillCancelled"]));
                        item.PatientSourceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceId"]));
                        item.CompanyId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompanyId"]));
                        item.CostingDivisionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CostingDivisionID"]));
                        item.PatientCategoryId = (long) DALHelper.HandleDBNull(reader["PatientTypeID"]);
                        item.RelationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RelationID"]));
                        if (!nvo.IsIPDBillList)
                        {
                            item.PaymentModeDetails = Convert.ToString(DALHelper.HandleDBNull(reader["Payemtmode"]));
                            item.BalanceAmountSelf = (Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"])) || (item.BillType == BillTypes.Pharmacy)) ? Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmountSelf"])) : Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"]));
                        }
                        item.IsRequestSend = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRequestSend"]));
                        if (nvo.IsRequest)
                        {
                            item.IsApproved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsApproved"]));
                            item.LevelID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LevelID"]));
                            item.LevelDescription = Convert.ToString(DALHelper.HandleDBNull(reader["LevelDescription"]));
                            item.ApprovalRemark = Convert.ToString(DALHelper.HandleDBNull(reader["ApprovalRemark"]));
                            item.RequestTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RequestTypeID"]));
                            item.AuthorityPerson = Convert.ToString(DALHelper.HandleDBNull(reader["AuthorityPerson"]));
                        }
                        item.IsInvoiceGenerated = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsInvoiceGenerated"]));
                        item.ConcessionReasonId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConcessionReasonId"]));
                        item.ConcessionRemark = Convert.ToString(DALHelper.HandleDBNull(reader["ConcessionRemark"]));
                        if (nvo.Opd_Ipd_External == 1)
                        {
                            item.ISDischarged = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ISDischarged"]));
                        }
                        nvo.List.Add(item);
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

        public override IValueObject GetTariffTypeDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPackageTariffBizActionVO nvo = valueObject as clsGetPackageTariffBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPackageTariffType");
                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, nvo.TariffID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.isPackageTariff = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["PackageType"]));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetTotalBillAccountsLedgers(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetTotalBillAccountsLedgersVO svo = valueObject as clsGetTotalBillAccountsLedgersVO;
            List<clsLedgerVO> source = new List<clsLedgerVO>();
            List<clsLedgerVO> list2 = new List<clsLedgerVO>();
            try
            {
                double? dR;
                List<clsLedgerVO>.Enumerator enumerator;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAccountInterface");
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, svo.Details.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.Date, svo.Details.ExportDate);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (svo.Details.OPDBillsLedgerAccount == null)
                    {
                        svo.Details.OPDBillsLedgerAccount = new List<clsLedgerVO>();
                    }
                    while (reader.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader["Amount"]))),
                            CR = 0.0,
                            VoucherType = "Receipt",
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCredit"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDBillsLedgerAccount.Add(item);
                        }
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader["Amount"]))),
                            CR = 0.0,
                            VoucherType = "Receipt",
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCredit"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDBillsLedgerAccount.Add(item);
                        }
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader["Amount"]))),
                            CR = 0.0,
                            VoucherType = "Receipt",
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCredit"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDBillsLedgerAccount.Add(item);
                        }
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalConcession"]))),
                            CR = 0.0,
                            VoucherType = "Receipt",
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCredit"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDBillsLedgerAccount.Add(item);
                        }
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalConcession"]))),
                            CR = 0.0,
                            VoucherType = "Receipt",
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCredit"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDBillsLedgerAccount.Add(item);
                        }
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader["BillIDs"])),
                            DR = 0.0,
                            CR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader["Amount"])))
                        };
                        if (svo.Details.OPDBillsLedgerAccount == null)
                        {
                            item.RowID = 1L;
                        }
                        else
                        {
                            item.RowID = (svo.Details.OPDBillsLedgerAccount.SingleOrDefault<clsLedgerVO>(S => S.RowID.Equals((long) 1L)) != null) ? 0L : 1L;
                        }
                        item.VoucherType = "Receipt";
                        item.TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader["TransactionType"]));
                        item.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader["GPVoucherType"]));
                        item.PatientType = Convert.ToString(DALHelper.HandleDBNull(reader["PatientType"]));
                        item.Sponsor = Convert.ToString(DALHelper.HandleDBNull(reader["SponsorType"]));
                        item.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader["VoucherMode"]));
                        item.PatientNo = Convert.ToString(DALHelper.HandleDBNull(reader["PatientNo"]));
                        item.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(reader["Pat_Comp_Name"]));
                        item.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(reader["TransactionGroup"]));
                        item.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionId"]));
                        item.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        item.VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader["VatAmount"])));
                        item.TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"])));
                        item.NetAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"])));
                        item.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader["VendorID"]));
                        item.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader["PurchaseInvoiceNo"]));
                        item.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(reader["TransactionNo"]));
                        item.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCredit"]));
                        dR = item.CR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDBillsLedgerAccount.Add(item);
                        }
                    }
                }
                reader.Close();
                DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_GetOPDCreditBillsLedgerAccount");
                this.dbServer.AddInParameter(command2, "UnitId", DbType.Int64, svo.Details.UnitID);
                this.dbServer.AddInParameter(command2, "Date", DbType.DateTime, svo.Details.ExportDate);
                DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(command2);
                source = new List<clsLedgerVO>();
                list2 = new List<clsLedgerVO>();
                if (svo.Details.OPDSelfCreditBillAccount == null)
                {
                    svo.Details.OPDSelfCreditBillAccount = new List<clsLedgerVO>();
                }
                if (reader2.HasRows)
                {
                    while (reader2.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            RowID = 1L,
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader2["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader2["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader2["Amount"]))),
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsCredit"])),
                            CR = 0.0,
                            VoucherType = "Sales"
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDSelfCreditBillAccount.Add(item);
                        }
                    }
                }
                reader2.NextResult();
                if (reader2.HasRows)
                {
                    while (reader2.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader2["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader2["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader2["Amount"]))),
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsCredit"])),
                            CR = 0.0,
                            VoucherType = "Sales"
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDSelfCreditBillAccount.Add(item);
                        }
                    }
                }
                reader2.NextResult();
                if (reader2.HasRows)
                {
                    while (reader2.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader2["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader2["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader2["Amount"]))),
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsCredit"])),
                            CR = 0.0,
                            VoucherType = "Sales"
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDSelfCreditBillAccount.Add(item);
                        }
                    }
                }
                reader2.NextResult();
                if (reader2.HasRows)
                {
                    while (reader2.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader2["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader2["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader2["Amount"]))),
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsCredit"])),
                            CR = 0.0,
                            VoucherType = "Sales"
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDSelfCreditBillAccount.Add(item);
                        }
                    }
                }
                reader2.NextResult();
                if (reader2.HasRows)
                {
                    while (reader2.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = (string) DALHelper.HandleDBNull(reader2["LedgerName"]),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader2["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader2["Amount"]))),
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsCredit"])),
                            CR = 0.0,
                            VoucherType = "Sales"
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDSelfCreditBillAccount.Add(item);
                        }
                    }
                }
                reader2.NextResult();
                if (reader2.HasRows)
                {
                    while (reader2.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = (string) DALHelper.HandleDBNull(reader2["LedgerName"]),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader2["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader2["Amount"]))),
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsCredit"])),
                            CR = 0.0,
                            VoucherType = "Sales"
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDSelfCreditBillAccount.Add(item);
                        }
                    }
                }
                reader2.NextResult();
                if (reader2.HasRows)
                {
                    while (reader2.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader2["LedgerName"]))
                        };
                        if (svo.Details.OPDSelfCreditBillAccount == null)
                        {
                            item.RowID = 1L;
                        }
                        else
                        {
                            (from S in svo.Details.OPDSelfCreditBillAccount
                                where S.RowID == 1L
                                select S).Any<clsLedgerVO>();
                            item.RowID = 0L;
                        }
                        item.Narration = Convert.ToString(DALHelper.HandleDBNull(reader2["Narration"]));
                        item.CR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader2["Amount"])));
                        item.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsCredit"]));
                        item.DR = 0.0;
                        item.VoucherType = "Sales";
                        dR = item.CR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDSelfCreditBillAccount.Add(item);
                        }
                    }
                }
                reader2.Close();
                DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_GetOPDAdvanceAmount");
                this.dbServer.AddInParameter(command3, "UnitId", DbType.Int64, svo.Details.UnitID);
                this.dbServer.AddInParameter(command3, "Date", DbType.DateTime, svo.Details.ExportDate);
                DbDataReader reader4 = (DbDataReader) this.dbServer.ExecuteReader(command3);
                source = new List<clsLedgerVO>();
                list2 = new List<clsLedgerVO>();
                if (reader4.HasRows)
                {
                    if (svo.Details.OPDAdvanceLedgerAccount == null)
                    {
                        svo.Details.OPDAdvanceLedgerAccount = new List<clsLedgerVO>();
                    }
                    while (true)
                    {
                        if (!reader4.Read())
                        {
                            reader4.NextResult();
                            while (true)
                            {
                                if (!reader4.Read())
                                {
                                    using (enumerator = list2.GetEnumerator())
                                    {
                                        Func<clsLedgerVO, bool> predicate = null;
                                        while (enumerator.MoveNext())
                                        {
                                            clsLedgerVO item = enumerator.Current;
                                            if (predicate == null)
                                            {
                                                predicate = S => S.ID.Equals(item.ID);
                                            }
                                            foreach (clsLedgerVO rvo16 in source.Where<clsLedgerVO>(predicate).ToList<clsLedgerVO>())
                                            {
                                                item.RowID = 1L;
                                                svo.Details.OPDAdvanceLedgerAccount.Add(rvo16);
                                            }
                                        }
                                    }
                                    reader4.Close();
                                    break;
                                }
                                clsLedgerVO rvo15 = new clsLedgerVO {
                                    ID = Convert.ToInt64(DALHelper.HandleDBNull(reader4["ID"])),
                                    LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader4["LedgerName"])),
                                    Narration = Convert.ToString(DALHelper.HandleDBNull(reader4["Narration"])),
                                    DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader4["Amount"]))),
                                    Reference = Convert.ToString(DALHelper.HandleDBNull(reader4["ID"])),
                                    VoucherType = "Receipt",
                                    CR = 0.0,
                                    IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader4["IsCredit"]))
                                };
                                source.Add(rvo15);
                            }
                            break;
                        }
                        clsLedgerVO rvo14 = new clsLedgerVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader4["ID"])),
                            LedgerName = (string) DALHelper.HandleDBNull(reader4["LedgerName"]),
                            Narration = (string) DALHelper.HandleDBNull(reader4["Narration"]),
                            DR = 0.0,
                            CR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader4["Amount"]))),
                            VoucherType = "Receipt",
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader4["IsCredit"])),
                            TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader4["TransactionType"])),
                            GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader4["GPVoucherType"])),
                            PatientType = Convert.ToString(DALHelper.HandleDBNull(reader4["PatientType"])),
                            Sponsor = Convert.ToString(DALHelper.HandleDBNull(reader4["SponsorType"])),
                            VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader4["VoucherMode"])),
                            PatientNo = Convert.ToString(DALHelper.HandleDBNull(reader4["PatientNo"])),
                            Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(reader4["Pat_Comp_Name"])),
                            TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(reader4["TransactionGroup"])),
                            TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(reader4["TransactionId"])),
                            Remarks = Convert.ToString(DALHelper.HandleDBNull(reader4["Remark"])),
                            VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader4["VatAmount"]))),
                            TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader4["TotalAmount"]))),
                            NetAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader4["NetAmount"]))),
                            VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader4["VendorID"])),
                            PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader4["PurchaseInvoiceNo"])),
                            TransactionNo = Convert.ToString(DALHelper.HandleDBNull(reader4["TransactionNo"])),
                            Reference = Convert.ToString(DALHelper.HandleDBNull(reader4["ID"]))
                        };
                        source.Add(rvo14);
                        list2.Add(rvo14);
                    }
                }
                DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_GetOPDAdvanceRefundDetails");
                this.dbServer.AddInParameter(command4, "UnitId", DbType.Int64, svo.Details.UnitID);
                this.dbServer.AddInParameter(command4, "Date", DbType.DateTime, svo.Details.ExportDate);
                DbDataReader reader5 = (DbDataReader) this.dbServer.ExecuteReader(command4);
                source = new List<clsLedgerVO>();
                list2 = new List<clsLedgerVO>();
                if (reader5.HasRows)
                {
                    if (svo.Details.OPDAdvanceRefundLedgerAccount == null)
                    {
                        svo.Details.OPDAdvanceRefundLedgerAccount = new List<clsLedgerVO>();
                    }
                    while (true)
                    {
                        if (!reader5.Read())
                        {
                            reader5.NextResult();
                            while (true)
                            {
                                if (!reader5.Read())
                                {
                                    using (enumerator = list2.GetEnumerator())
                                    {
                                        Func<clsLedgerVO, bool> predicate = null;
                                        while (enumerator.MoveNext())
                                        {
                                            clsLedgerVO rvo1 = enumerator.Current;
                                            if (predicate == null)
                                            {
                                                predicate = S => S.ID.Equals(rvo1.ID);
                                            }
                                            foreach (clsLedgerVO rvo19 in source.Where<clsLedgerVO>(predicate).ToList<clsLedgerVO>())
                                            {
                                                svo.Details.OPDAdvanceRefundLedgerAccount.Add(rvo19);
                                            }
                                        }
                                    }
                                    reader5.Close();
                                    break;
                                }
                                clsLedgerVO rvo18 = new clsLedgerVO {
                                    ID = Convert.ToInt64(DALHelper.HandleDBNull(reader5["ID"])),
                                    LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader5["LedgerName"])),
                                    Narration = Convert.ToString(DALHelper.HandleDBNull(reader5["Narration"])),
                                    DR = 0.0,
                                    CR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader5["Amount"]))),
                                    VoucherType = "Journal",
                                    Reference = Convert.ToString(DALHelper.HandleDBNull(reader5["ID"])),
                                    IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader5["IsCredit"]))
                                };
                                source.Add(rvo18);
                            }
                            break;
                        }
                        clsLedgerVO item = new clsLedgerVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader5["ID"])),
                            LedgerName = (string) DALHelper.HandleDBNull(reader5["LedgerName"]),
                            Narration = (string) DALHelper.HandleDBNull(reader5["Narration"]),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader5["Amount"]))),
                            CR = 0.0,
                            VoucherType = "Journal",
                            RowID = 1L,
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader5["IsCredit"])),
                            TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader5["TransactionType"])),
                            GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader5["GPVoucherType"])),
                            PatientType = Convert.ToString(DALHelper.HandleDBNull(reader5["PatientType"])),
                            Sponsor = Convert.ToString(DALHelper.HandleDBNull(reader5["SponsorType"])),
                            VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader5["VoucherMode"])),
                            PatientNo = Convert.ToString(DALHelper.HandleDBNull(reader5["PatientNo"])),
                            Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(reader5["Pat_Comp_Name"])),
                            TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(reader5["TransactionGroup"])),
                            TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(reader5["TransactionId"])),
                            Remarks = Convert.ToString(DALHelper.HandleDBNull(reader5["Remark"])),
                            VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader5["VatAmount"]))),
                            TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader5["TotalAmount"]))),
                            NetAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader5["NetAmount"]))),
                            VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader5["VendorID"])),
                            PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader5["PurchaseInvoiceNo"])),
                            TransactionNo = Convert.ToString(DALHelper.HandleDBNull(reader5["TransactionNo"])),
                            Reference = Convert.ToString(DALHelper.HandleDBNull(reader5["ID"]))
                        };
                        source.Add(item);
                        list2.Add(item);
                    }
                }
                DbCommand command5 = this.dbServer.GetStoredProcCommand("CIMS_GetOPDBillRefundDetails");
                this.dbServer.AddInParameter(command5, "UnitId", DbType.Int64, svo.Details.UnitID);
                this.dbServer.AddInParameter(command5, "Date", DbType.DateTime, svo.Details.ExportDate);
                DbDataReader reader6 = (DbDataReader) this.dbServer.ExecuteReader(command5);
                if (reader6.HasRows)
                {
                    if (svo.Details.OPDRefundBillLedgerAccount == null)
                    {
                        svo.Details.OPDRefundBillLedgerAccount = new List<clsLedgerVO>();
                    }
                    while (true)
                    {
                        if (!reader6.Read())
                        {
                            reader6.NextResult();
                            while (true)
                            {
                                if (!reader6.Read())
                                {
                                    reader6.Close();
                                    break;
                                }
                                clsLedgerVO rvo21 = new clsLedgerVO {
                                    LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader6["LedgerName"])),
                                    Narration = Convert.ToString(DALHelper.HandleDBNull(reader6["Narration"]))
                                };
                                if (svo.Details.OPDRefundBillLedgerAccount == null)
                                {
                                    rvo21.RowID = 1L;
                                }
                                else
                                {
                                    rvo21.RowID = !(from z in svo.Details.OPDRefundBillLedgerAccount
                                        where z.RowID == 1L
                                        select z).Any<clsLedgerVO>() ? 0L : 1L;
                                }
                                rvo21.DR = 0.0;
                                rvo21.VoucherType = "Payment";
                                rvo21.CR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader6["Amount"])));
                                rvo21.Reference = Convert.ToString(DALHelper.HandleDBNull(reader6["ID"]));
                                rvo21.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader6["IsCredit"]));
                                dR = rvo21.CR;
                                if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                                {
                                    svo.Details.OPDRefundBillLedgerAccount.Add(rvo21);
                                }
                            }
                            break;
                        }
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = (string) DALHelper.HandleDBNull(reader6["Company"]),
                            Narration = (string) DALHelper.HandleDBNull(reader6["Narration"]),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader6["Amount"]))),
                            CR = 0.0,
                            RowID = 1L,
                            VoucherType = "Payment",
                            TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader6["TransactionType"])),
                            GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader6["GPVoucherType"])),
                            PatientType = Convert.ToString(DALHelper.HandleDBNull(reader6["PatientType"])),
                            Sponsor = Convert.ToString(DALHelper.HandleDBNull(reader6["SponsorType"])),
                            VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader6["VoucherMode"])),
                            PatientNo = Convert.ToString(DALHelper.HandleDBNull(reader6["PatientNo"])),
                            Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(reader6["Pat_Comp_Name"])),
                            TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(reader6["TransactionGroup"])),
                            TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(reader6["TransactionId"])),
                            Remarks = Convert.ToString(DALHelper.HandleDBNull(reader6["Remark"])),
                            VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader6["VatAmount"]))),
                            TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader6["TotalAmount"]))),
                            NetAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader6["NetAmount"]))),
                            VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader6["VendorID"])),
                            PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader6["PurchaseInvoiceNo"])),
                            TransactionNo = Convert.ToString(DALHelper.HandleDBNull(reader6["TransactionNo"])),
                            Reference = Convert.ToString(DALHelper.HandleDBNull(reader6["ID"])),
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader6["IsCredit"]))
                        };
                        svo.Details.OPDRefundBillLedgerAccount.Add(item);
                    }
                }
                DbCommand command6 = this.dbServer.GetStoredProcCommand("CIMS_GetOPDSelfReceiptLedgerAccount");
                this.dbServer.AddInParameter(command6, "UnitId", DbType.Int64, svo.Details.UnitID);
                this.dbServer.AddInParameter(command6, "Date", DbType.DateTime, svo.Details.ExportDate);
                DbDataReader reader7 = (DbDataReader) this.dbServer.ExecuteReader(command6);
                source = new List<clsLedgerVO>();
                list2 = new List<clsLedgerVO>();
                if (svo.Details.OPDReceiptLedgerAccount == null)
                {
                    svo.Details.OPDReceiptLedgerAccount = new List<clsLedgerVO>();
                }
                if (reader7.HasRows)
                {
                    while (reader7.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader7["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader7["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader7["Amount"]))),
                            CR = 0.0,
                            VoucherType = "Receipt",
                            TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader7["TransactionType"])),
                            GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader7["GPVoucherType"])),
                            PatientType = Convert.ToString(DALHelper.HandleDBNull(reader7["PatientType"])),
                            Sponsor = Convert.ToString(DALHelper.HandleDBNull(reader7["SponsorType"])),
                            VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader7["VoucherMode"])),
                            TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(reader7["TransactionGroup"])),
                            VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader7["VatAmount"]))),
                            TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader7["TotalAmount"]))),
                            NetAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader7["NetAmount"]))),
                            VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader7["VendorID"])),
                            PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader7["PurchaseInvoiceNo"])),
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader7["IsCredit"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDReceiptLedgerAccount.Add(item);
                        }
                    }
                }
                reader7.NextResult();
                if (reader7.HasRows)
                {
                    while (reader7.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader7["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader7["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader7["Amount"]))),
                            CR = 0.0,
                            VoucherType = "Receipt",
                            TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader7["TransactionType"])),
                            GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader7["GPVoucherType"])),
                            PatientType = Convert.ToString(DALHelper.HandleDBNull(reader7["PatientType"])),
                            Sponsor = Convert.ToString(DALHelper.HandleDBNull(reader7["SponsorType"])),
                            VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader7["VoucherMode"])),
                            TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(reader7["TransactionGroup"])),
                            VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader7["VatAmount"]))),
                            TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader7["TotalAmount"]))),
                            NetAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader7["NetAmount"]))),
                            VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader7["VendorID"])),
                            PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader7["PurchaseInvoiceNo"])),
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader7["IsCredit"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDReceiptLedgerAccount.Add(item);
                        }
                    }
                }
                reader7.NextResult();
                if (reader7.HasRows)
                {
                    while (reader7.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader7["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader7["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader7["Amount"]))),
                            CR = 0.0,
                            VoucherType = "Receipt",
                            TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader7["TransactionType"])),
                            GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader7["GPVoucherType"])),
                            PatientType = Convert.ToString(DALHelper.HandleDBNull(reader7["PatientType"])),
                            Sponsor = Convert.ToString(DALHelper.HandleDBNull(reader7["SponsorType"])),
                            VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader7["VoucherMode"])),
                            TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(reader7["TransactionGroup"])),
                            Remarks = Convert.ToString(DALHelper.HandleDBNull(reader7["Remark"])),
                            VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader7["VatAmount"]))),
                            TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader7["TotalAmount"]))),
                            NetAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader7["NetAmount"]))),
                            VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader7["VendorID"])),
                            PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader7["PurchaseInvoiceNo"])),
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader7["IsCredit"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDReceiptLedgerAccount.Add(item);
                        }
                    }
                }
                reader7.NextResult();
                if (reader7.HasRows)
                {
                    while (reader7.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader7["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader7["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader7["Amount"]))),
                            CR = 0.0,
                            VoucherType = "Receipt",
                            TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader7["TransactionType"])),
                            GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader7["GPVoucherType"])),
                            PatientType = Convert.ToString(DALHelper.HandleDBNull(reader7["PatientType"])),
                            Sponsor = Convert.ToString(DALHelper.HandleDBNull(reader7["SponsorType"])),
                            VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader7["VoucherMode"])),
                            TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(reader7["TransactionGroup"])),
                            VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader7["VatAmount"]))),
                            TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader7["TotalAmount"]))),
                            NetAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader7["NetAmount"]))),
                            VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader7["VendorID"])),
                            PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader7["PurchaseInvoiceNo"])),
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader7["IsCredit"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDReceiptLedgerAccount.Add(item);
                        }
                    }
                }
                reader7.NextResult();
                if (reader7.HasRows)
                {
                    while (reader7.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader7["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader7["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader7["Amount"]))),
                            CR = 0.0,
                            VoucherType = "Receipt",
                            TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader7["TransactionType"])),
                            GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader7["GPVoucherType"])),
                            PatientType = Convert.ToString(DALHelper.HandleDBNull(reader7["PatientType"])),
                            Sponsor = Convert.ToString(DALHelper.HandleDBNull(reader7["SponsorType"])),
                            VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader7["VoucherMode"])),
                            TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(reader7["TransactionGroup"])),
                            VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader7["VatAmount"]))),
                            TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader7["TotalAmount"]))),
                            NetAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader7["NetAmount"]))),
                            VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader7["VendorID"])),
                            PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader7["PurchaseInvoiceNo"])),
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader7["IsCredit"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDReceiptLedgerAccount.Add(item);
                        }
                    }
                }
                reader7.NextResult();
                if (reader7.HasRows)
                {
                    while (reader7.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader7["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader7["Narration"]))
                        };
                        if (svo.Details.OPDReceiptLedgerAccount == null)
                        {
                            item.RowID = 1L;
                        }
                        else
                        {
                            item.RowID = !(from z in svo.Details.OPDReceiptLedgerAccount
                                where z.RowID == 1L
                                select z).Any<clsLedgerVO>() ? 0L : 1L;
                        }
                        item.CR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader7["Amount"])));
                        item.DR = 0.0;
                        item.VoucherType = "Receipt";
                        item.TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader7["TransactionType"]));
                        item.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader7["GPVoucherType"]));
                        item.PatientType = Convert.ToString(DALHelper.HandleDBNull(reader7["PatientType"]));
                        item.Sponsor = Convert.ToString(DALHelper.HandleDBNull(reader7["SponsorType"]));
                        item.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader7["VoucherMode"]));
                        item.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(reader7["TransactionGroup"]));
                        item.VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader7["VatAmount"])));
                        item.TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader7["TotalAmount"])));
                        item.NetAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader7["NetAmount"])));
                        item.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader7["VendorID"]));
                        item.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader7["PurchaseInvoiceNo"]));
                        item.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader7["IsCredit"]));
                        dR = item.CR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDReceiptLedgerAccount.Add(item);
                        }
                    }
                }
                reader7.Close();
                DbCommand command7 = this.dbServer.GetStoredProcCommand("CIMS_GetOPDCompanyBillsLedgerAccount");
                this.dbServer.AddInParameter(command7, "UnitId", DbType.Int64, svo.Details.UnitID);
                this.dbServer.AddInParameter(command7, "Date", DbType.DateTime, svo.Details.ExportDate);
                DbDataReader reader3 = (DbDataReader) this.dbServer.ExecuteReader(command7);
                source = new List<clsLedgerVO>();
                list2 = new List<clsLedgerVO>();
                if (reader3.HasRows)
                {
                    if (svo.Details.OPDCompanyCreditBillAccount == null)
                    {
                        svo.Details.OPDCompanyCreditBillAccount = new List<clsLedgerVO>();
                    }
                    while (reader3.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader3["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader3["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader3["Amount"]))),
                            CR = 0.0,
                            VoucherType = "Sales",
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader3["IsCredit"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDCompanyCreditBillAccount.Add(item);
                        }
                    }
                }
                reader3.NextResult();
                if (reader3.HasRows)
                {
                    while (reader3.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader3["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader3["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader3["Amount"]))),
                            CR = 0.0,
                            VoucherType = "Sales",
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader3["IsCredit"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDCompanyCreditBillAccount.Add(item);
                        }
                    }
                }
                reader3.NextResult();
                if (reader3.HasRows)
                {
                    while (reader3.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader3["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader3["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader3["Amount"]))),
                            CR = 0.0,
                            VoucherType = "Sales",
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader3["IsCredit"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDCompanyCreditBillAccount.Add(item);
                        }
                    }
                }
                reader3.NextResult();
                if (reader3.HasRows)
                {
                    while (reader3.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader3["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader3["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader3["Amount"]))),
                            CR = 0.0,
                            VoucherType = "Sales",
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader3["IsCredit"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDCompanyCreditBillAccount.Add(item);
                        }
                    }
                }
                reader3.NextResult();
                if (reader3.HasRows)
                {
                    while (reader3.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader3["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader3["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader3["Amount"]))),
                            CR = 0.0,
                            VoucherType = "Sales",
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader3["IsCredit"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDCompanyCreditBillAccount.Add(item);
                        }
                    }
                }
                reader3.NextResult();
                if (reader3.HasRows)
                {
                    while (reader3.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader3["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader3["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader3["Amount"]))),
                            CR = 0.0,
                            VoucherType = "Sales",
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader3["IsCredit"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDCompanyCreditBillAccount.Add(item);
                        }
                    }
                }
                reader3.NextResult();
                if (reader3.HasRows)
                {
                    while (reader3.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader3["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader3["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader3["Amount"]))),
                            CR = 0.0,
                            VoucherType = "Sales",
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader3["IsCredit"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDCompanyCreditBillAccount.Add(item);
                        }
                    }
                }
                reader3.NextResult();
                if (reader3.HasRows)
                {
                    while (reader3.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader3["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader3["BillIDs"])),
                            DR = 0.0,
                            CR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader3["Amount"])))
                        };
                        if (svo.Details.OPDCompanyCreditBillAccount == null)
                        {
                            item.RowID = 1L;
                        }
                        else
                        {
                            item.RowID = (svo.Details.OPDCompanyCreditBillAccount.SingleOrDefault<clsLedgerVO>(S => S.RowID.Equals((long) 1L)) != null) ? 0L : 1L;
                        }
                        item.VoucherType = "Sales";
                        item.TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader3["TransactionType"]));
                        item.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader3["GPVoucherType"]));
                        item.PatientType = Convert.ToString(DALHelper.HandleDBNull(reader3["PatientType"]));
                        item.Sponsor = Convert.ToString(DALHelper.HandleDBNull(reader3["SponsorType"]));
                        item.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader3["VoucherMode"]));
                        item.PatientNo = Convert.ToString(DALHelper.HandleDBNull(reader3["PatientNo"]));
                        item.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(reader3["Pat_Comp_Name"]));
                        item.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(reader3["TransactionGroup"]));
                        item.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(reader3["TransactionId"]));
                        item.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader3["Remark"]));
                        item.VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader3["VatAmount"])));
                        item.TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader3["TotalAmount"])));
                        item.NetAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader3["NetAmount"])));
                        item.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader3["VendorID"]));
                        item.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader3["PurchaseInvoiceNo"]));
                        item.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(reader3["TransactionNo"]));
                        item.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader3["IsCredit"]));
                        dR = item.CR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDCompanyCreditBillAccount.Add(item);
                        }
                    }
                }
                reader3.Close();
                DbCommand command8 = this.dbServer.GetStoredProcCommand("CIMS_GetOPDCompanyReceiptsLedgerAccount");
                this.dbServer.AddInParameter(command8, "UnitId", DbType.Int64, svo.Details.UnitID);
                this.dbServer.AddInParameter(command8, "Date", DbType.DateTime, svo.Details.ExportDate);
                DbDataReader reader8 = (DbDataReader) this.dbServer.ExecuteReader(command8);
                source = new List<clsLedgerVO>();
                list2 = new List<clsLedgerVO>();
                if (svo.Details.OPDCompanyReceiptBillAccount == null)
                {
                    svo.Details.OPDCompanyReceiptBillAccount = new List<clsLedgerVO>();
                }
                if (reader8.HasRows)
                {
                    while (reader8.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader8["ID"])),
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader8["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader8["Narration"])),
                            Reference = Convert.ToString(DALHelper.HandleDBNull(reader8["BillNo"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader8["Amount"]))),
                            CR = 0.0,
                            VoucherType = "Receipt",
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader8["IsCredit"])),
                            TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader8["TransactionType"])),
                            GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader8["GPVoucherType"])),
                            PatientType = Convert.ToString(DALHelper.HandleDBNull(reader8["PatientType"])),
                            Sponsor = Convert.ToString(DALHelper.HandleDBNull(reader8["SponsorType"])),
                            VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader8["VoucherMode"])),
                            PatientNo = Convert.ToString(DALHelper.HandleDBNull(reader8["PatientNo"])),
                            Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(reader8["Pat_Comp_Name"])),
                            TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(reader8["TransactionGroup"])),
                            TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(reader8["TransactionId"])),
                            Remarks = Convert.ToString(DALHelper.HandleDBNull(reader8["Remark"])),
                            VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader8["VatAmount"]))),
                            TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader8["TotalAmount"]))),
                            NetAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader8["NetAmount"]))),
                            VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader8["VendorID"])),
                            PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader8["PurchaseInvoiceNo"])),
                            TransactionNo = Convert.ToString(DALHelper.HandleDBNull(reader8["TransactionNo"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDCompanyReceiptBillAccount.Add(item);
                        }
                    }
                }
                reader8.NextResult();
                if (reader8.HasRows)
                {
                    while (reader8.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader8["ID"])),
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader8["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader8["Narration"])),
                            Reference = Convert.ToString(DALHelper.HandleDBNull(reader8["BillNo"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader8["Amount"]))),
                            CR = 0.0,
                            VoucherType = "Receipt",
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader8["IsCredit"])),
                            TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader8["TransactionType"])),
                            GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader8["GPVoucherType"])),
                            PatientType = Convert.ToString(DALHelper.HandleDBNull(reader8["PatientType"])),
                            Sponsor = Convert.ToString(DALHelper.HandleDBNull(reader8["SponsorType"])),
                            VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader8["VoucherMode"])),
                            PatientNo = Convert.ToString(DALHelper.HandleDBNull(reader8["PatientNo"])),
                            Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(reader8["Pat_Comp_Name"])),
                            TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(reader8["TransactionGroup"])),
                            TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(reader8["TransactionId"])),
                            Remarks = Convert.ToString(DALHelper.HandleDBNull(reader8["Remark"])),
                            VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader8["VatAmount"]))),
                            TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader8["TotalAmount"]))),
                            NetAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader8["NetAmount"]))),
                            VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader8["VendorID"])),
                            PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader8["PurchaseInvoiceNo"])),
                            TransactionNo = Convert.ToString(DALHelper.HandleDBNull(reader8["TransactionNo"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDCompanyReceiptBillAccount.Add(item);
                        }
                    }
                }
                reader8.NextResult();
                if (reader8.HasRows)
                {
                    while (reader8.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader8["ID"])),
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader8["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader8["Narration"])),
                            Reference = Convert.ToString(DALHelper.HandleDBNull(reader8["BillNo"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader8["Amount"]))),
                            CR = 0.0,
                            VoucherType = "Receipt",
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader8["IsCredit"])),
                            TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader8["TransactionType"])),
                            GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader8["GPVoucherType"])),
                            PatientType = Convert.ToString(DALHelper.HandleDBNull(reader8["PatientType"])),
                            Sponsor = Convert.ToString(DALHelper.HandleDBNull(reader8["SponsorType"])),
                            VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader8["VoucherMode"])),
                            PatientNo = Convert.ToString(DALHelper.HandleDBNull(reader8["PatientNo"])),
                            Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(reader8["Pat_Comp_Name"])),
                            TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(reader8["TransactionGroup"])),
                            TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(reader8["TransactionId"])),
                            Remarks = Convert.ToString(DALHelper.HandleDBNull(reader8["Remark"])),
                            VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader8["VatAmount"]))),
                            TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader8["TotalAmount"]))),
                            NetAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader8["NetAmount"]))),
                            VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader8["VendorID"])),
                            PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader8["PurchaseInvoiceNo"])),
                            TransactionNo = Convert.ToString(DALHelper.HandleDBNull(reader8["TransactionNo"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDCompanyReceiptBillAccount.Add(item);
                        }
                    }
                }
                reader8.NextResult();
                if (reader8.HasRows)
                {
                    while (reader8.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader8["ID"])),
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader8["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader8["Narration"])),
                            Reference = Convert.ToString(DALHelper.HandleDBNull(reader8["BillNo"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader8["Amount"]))),
                            CR = 0.0,
                            VoucherType = "Receipt",
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader8["IsCredit"])),
                            TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader8["TransactionType"])),
                            GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader8["GPVoucherType"])),
                            PatientType = Convert.ToString(DALHelper.HandleDBNull(reader8["PatientType"])),
                            Sponsor = Convert.ToString(DALHelper.HandleDBNull(reader8["SponsorType"])),
                            VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader8["VoucherMode"])),
                            PatientNo = Convert.ToString(DALHelper.HandleDBNull(reader8["PatientNo"])),
                            Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(reader8["Pat_Comp_Name"])),
                            TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(reader8["TransactionGroup"])),
                            TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(reader8["TransactionId"])),
                            Remarks = Convert.ToString(DALHelper.HandleDBNull(reader8["Remark"])),
                            VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader8["VatAmount"]))),
                            TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader8["TotalAmount"]))),
                            NetAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader8["NetAmount"]))),
                            VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader8["VendorID"])),
                            PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader8["PurchaseInvoiceNo"])),
                            TransactionNo = Convert.ToString(DALHelper.HandleDBNull(reader8["TransactionNo"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDCompanyReceiptBillAccount.Add(item);
                        }
                    }
                }
                reader8.NextResult();
                if (reader8.HasRows)
                {
                    while (reader8.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader8["ID"])),
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader8["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader8["Narration"]))
                        };
                        if (svo.Details.OPDCompanyReceiptBillAccount == null)
                        {
                            item.RowID = 1L;
                        }
                        else
                        {
                            item.RowID = (svo.Details.OPDCompanyReceiptBillAccount.SingleOrDefault<clsLedgerVO>(S => S.RowID.Equals((long) 1L)) != null) ? 0L : 1L;
                        }
                        item.Reference = Convert.ToString(DALHelper.HandleDBNull(reader8["BillNo"]));
                        item.CR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader8["Amount"])));
                        item.DR = 0.0;
                        item.VoucherType = "Receipt";
                        item.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader8["IsCredit"]));
                        item.TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader8["TransactionType"]));
                        item.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader8["GPVoucherType"]));
                        item.PatientType = Convert.ToString(DALHelper.HandleDBNull(reader8["PatientType"]));
                        item.Sponsor = Convert.ToString(DALHelper.HandleDBNull(reader8["SponsorType"]));
                        item.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader8["VoucherMode"]));
                        item.PatientNo = Convert.ToString(DALHelper.HandleDBNull(reader8["PatientNo"]));
                        item.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(reader8["Pat_Comp_Name"]));
                        item.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(reader8["TransactionGroup"]));
                        item.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(reader8["TransactionId"]));
                        item.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader8["Remark"]));
                        item.VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader8["VatAmount"])));
                        item.TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader8["TotalAmount"])));
                        item.NetAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader8["NetAmount"])));
                        item.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader8["VendorID"]));
                        item.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader8["PurchaseInvoiceNo"]));
                        item.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(reader8["TransactionNo"]));
                        dR = item.CR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.OPDCompanyReceiptBillAccount.Add(item);
                        }
                    }
                }
                reader8.Close();
                DbCommand command9 = this.dbServer.GetStoredProcCommand("CIMS_GetSelfIPDBillForTallyInterface ");
                this.dbServer.AddInParameter(command9, "UnitId", DbType.Int64, svo.Details.UnitID);
                this.dbServer.AddInParameter(command9, "Date", DbType.Date, svo.Details.ExportDate);
                DbDataReader reader9 = (DbDataReader) this.dbServer.ExecuteReader(command9);
                if (reader9.HasRows)
                {
                    if (svo.Details.IPDSelfBillAccount == null)
                    {
                        svo.Details.IPDSelfBillAccount = new List<clsLedgerVO>();
                    }
                    while (reader9.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader9["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader9["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader9["Amount"]))),
                            CR = 0.0,
                            VoucherType = "Receipt",
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader9["IsCredit"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.IPDSelfBillAccount.Add(item);
                        }
                    }
                }
                reader9.NextResult();
                if (reader9.HasRows)
                {
                    while (reader9.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader9["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader9["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader9["Amount"]))),
                            CR = 0.0,
                            VoucherType = "Receipt",
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader9["IsCredit"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.IPDSelfBillAccount.Add(item);
                        }
                    }
                }
                reader9.NextResult();
                if (reader9.HasRows)
                {
                    while (reader9.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader9["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader9["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader9["Amount"]))),
                            CR = 0.0,
                            VoucherType = "Receipt",
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader9["IsCredit"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.IPDSelfBillAccount.Add(item);
                        }
                    }
                }
                reader9.NextResult();
                if (reader9.HasRows)
                {
                    while (reader9.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader9["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader9["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader9["TotalConcession"]))),
                            CR = 0.0,
                            VoucherType = "Receipt",
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader9["IsCredit"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.IPDSelfBillAccount.Add(item);
                        }
                    }
                }
                reader9.NextResult();
                if (reader9.HasRows)
                {
                    while (reader9.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader9["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader9["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader9["TotalConcession"]))),
                            CR = 0.0,
                            VoucherType = "Receipt",
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader9["IsCredit"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.IPDSelfBillAccount.Add(item);
                        }
                    }
                }
                reader9.NextResult();
                if (reader9.HasRows)
                {
                    while (reader9.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader9["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader9["BillIDs"])),
                            DR = 0.0,
                            CR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader9["Amount"])))
                        };
                        if (svo.Details.IPDSelfBillAccount == null)
                        {
                            item.RowID = 1L;
                        }
                        else
                        {
                            item.RowID = (svo.Details.IPDSelfBillAccount.SingleOrDefault<clsLedgerVO>(S => S.RowID.Equals((long) 1L)) != null) ? 0L : 1L;
                        }
                        item.VoucherType = "Receipt";
                        item.TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader9["TransactionType"]));
                        item.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader9["GPVoucherType"]));
                        item.PatientType = Convert.ToString(DALHelper.HandleDBNull(reader9["PatientType"]));
                        item.Sponsor = Convert.ToString(DALHelper.HandleDBNull(reader9["SponsorType"]));
                        item.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader9["VoucherMode"]));
                        item.PatientNo = Convert.ToString(DALHelper.HandleDBNull(reader9["PatientNo"]));
                        item.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(reader9["Pat_Comp_Name"]));
                        item.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(reader9["TransactionGroup"]));
                        item.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(reader9["TransactionId"]));
                        item.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader9["Remark"]));
                        item.VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader9["VatAmount"])));
                        item.TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader9["TotalAmount"])));
                        item.NetAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader9["NetAmount"])));
                        item.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader9["VendorID"]));
                        item.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader9["PurchaseInvoiceNo"]));
                        item.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(reader9["TransactionNo"]));
                        item.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader9["IsCredit"]));
                        dR = item.CR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.IPDSelfBillAccount.Add(item);
                        }
                    }
                }
                reader9.Close();
                DbCommand command10 = this.dbServer.GetStoredProcCommand("CIMS_GetCreditIPDBillForTallyInterface");
                this.dbServer.AddInParameter(command10, "UnitId", DbType.Int64, svo.Details.UnitID);
                this.dbServer.AddInParameter(command10, "Date", DbType.DateTime, svo.Details.ExportDate);
                DbDataReader reader10 = (DbDataReader) this.dbServer.ExecuteReader(command10);
                source = new List<clsLedgerVO>();
                list2 = new List<clsLedgerVO>();
                if (svo.Details.IPDSelfCreditForTallyInterface == null)
                {
                    svo.Details.IPDSelfCreditForTallyInterface = new List<clsLedgerVO>();
                }
                if (reader10.HasRows)
                {
                    while (reader10.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            RowID = 1L,
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader10["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader10["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader10["Amount"]))),
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader10["IsCredit"])),
                            CR = 0.0,
                            VoucherType = "Sales"
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.IPDSelfCreditForTallyInterface.Add(item);
                        }
                    }
                }
                reader10.NextResult();
                if (reader10.HasRows)
                {
                    while (reader10.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader10["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader10["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader10["Amount"]))),
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader10["IsCredit"])),
                            CR = 0.0,
                            VoucherType = "Sales"
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.IPDSelfCreditForTallyInterface.Add(item);
                        }
                    }
                }
                reader10.NextResult();
                if (reader10.HasRows)
                {
                    while (reader10.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader10["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader10["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader10["Amount"]))),
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader10["IsCredit"])),
                            CR = 0.0,
                            VoucherType = "Sales"
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.IPDSelfCreditForTallyInterface.Add(item);
                        }
                    }
                }
                reader10.NextResult();
                if (reader10.HasRows)
                {
                    while (reader10.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader10["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader10["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader10["Amount"]))),
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader10["IsCredit"])),
                            CR = 0.0,
                            VoucherType = "Sales"
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.IPDSelfCreditForTallyInterface.Add(item);
                        }
                    }
                }
                reader10.NextResult();
                if (reader10.HasRows)
                {
                    while (reader10.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = (string) DALHelper.HandleDBNull(reader10["LedgerName"]),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader10["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader10["Amount"]))),
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader10["IsCredit"])),
                            CR = 0.0,
                            VoucherType = "Sales"
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.IPDSelfCreditForTallyInterface.Add(item);
                        }
                    }
                }
                reader10.NextResult();
                if (reader10.HasRows)
                {
                    while (reader10.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = (string) DALHelper.HandleDBNull(reader10["LedgerName"]),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader10["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader10["Amount"]))),
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader10["IsCredit"])),
                            CR = 0.0,
                            VoucherType = "Sales"
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.IPDSelfCreditForTallyInterface.Add(item);
                        }
                    }
                }
                reader10.NextResult();
                if (reader10.HasRows)
                {
                    while (reader10.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader10["LedgerName"]))
                        };
                        if (svo.Details.IPDSelfCreditForTallyInterface == null)
                        {
                            item.RowID = 1L;
                        }
                        else
                        {
                            (from S in svo.Details.IPDSelfCreditForTallyInterface
                                where S.RowID == 1L
                                select S).Any<clsLedgerVO>();
                            item.RowID = 0L;
                        }
                        item.Narration = Convert.ToString(DALHelper.HandleDBNull(reader10["Narration"]));
                        item.CR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader10["Amount"])));
                        item.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader10["IsCredit"]));
                        item.DR = 0.0;
                        item.VoucherType = "Sales";
                        dR = item.CR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.IPDSelfCreditForTallyInterface.Add(item);
                        }
                    }
                }
                reader10.Close();
                DbCommand command11 = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorBillLedger");
                this.dbServer.AddInParameter(command11, "UnitId", DbType.Int64, svo.Details.UnitID);
                this.dbServer.AddInParameter(command11, "Date", DbType.Date, svo.Details.ExportDate);
                reader = (DbDataReader) this.dbServer.ExecuteReader(command11);
                if (reader.HasRows)
                {
                    if (svo.Details.DoctorPaymentLedgers == null)
                    {
                        svo.Details.DoctorPaymentLedgers = new List<clsLedgerVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            while (true)
                            {
                                if (!reader.Read())
                                {
                                    reader.NextResult();
                                    while (true)
                                    {
                                        if (!reader.Read())
                                        {
                                            reader.Close();
                                            break;
                                        }
                                        clsLedgerVO rvo56 = new clsLedgerVO {
                                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader["LedgerName"])),
                                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader["Narration"])),
                                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader["Amount"]))),
                                            CR = 0.0
                                        };
                                        dR = rvo56.DR;
                                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                                        {
                                            svo.Details.DoctorPaymentLedgers.Add(rvo56);
                                        }
                                    }
                                    break;
                                }
                                clsLedgerVO rvo55 = new clsLedgerVO {
                                    LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader["LedgerName"])),
                                    Narration = Convert.ToString(DALHelper.HandleDBNull(reader["Narration"])),
                                    DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader["Amount"]))),
                                    CR = 0.0
                                };
                                dR = rvo55.DR;
                                if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                                {
                                    svo.Details.DoctorPaymentLedgers.Add(rvo55);
                                }
                            }
                            break;
                        }
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader["Narration"])),
                            DR = 0.0,
                            CR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader["Amount"])))
                        };
                        dR = item.CR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.DoctorPaymentLedgers.Add(item);
                        }
                    }
                }
                DbCommand command12 = this.dbServer.GetStoredProcCommand("CIMS_GetAccItemGRN");
                this.dbServer.AddInParameter(command12, "UnitId", DbType.Int64, svo.Details.UnitID);
                this.dbServer.AddInParameter(command12, "Date", DbType.DateTime, svo.Details.ExportDate);
                DbDataReader reader11 = (DbDataReader) this.dbServer.ExecuteReader(command12);
                list2 = new List<clsLedgerVO>();
                if (reader11.HasRows)
                {
                    if (svo.Details.PurchaseLedger == null)
                    {
                        svo.Details.PurchaseLedger = new List<clsLedgerVO>();
                    }
                    while (true)
                    {
                        if (!reader11.Read())
                        {
                            reader11.NextResult();
                            while (true)
                            {
                                if (!reader11.Read())
                                {
                                    reader11.NextResult();
                                    while (true)
                                    {
                                        if (!reader11.Read())
                                        {
                                            reader11.Close();
                                            break;
                                        }
                                        clsLedgerVO rvo59 = new clsLedgerVO {
                                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader11["LedgerName"])),
                                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader11["GRNIDs"])),
                                            CR = 0.0,
                                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader11["Amount"])))
                                        };
                                        if (svo.Details.PurchaseLedger == null)
                                        {
                                            rvo59.RowID = 1L;
                                        }
                                        else
                                        {
                                            rvo59.RowID = (svo.Details.PurchaseLedger.SingleOrDefault<clsLedgerVO>(S => S.RowID.Equals((long) 1L)) != null) ? 0L : 1L;
                                        }
                                        rvo59.VoucherType = "Purchase";
                                        rvo59.TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader11["TransactionType"]));
                                        rvo59.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader11["GPVoucherType"]));
                                        rvo59.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader11["VoucherMode"]));
                                        rvo59.VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader11["VatAmount"])));
                                        rvo59.TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader11["TotalAmount"])));
                                        rvo59.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader11["VendorID"]));
                                        rvo59.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(reader11["TransactionNo"]));
                                        rvo59.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader11["GRNUnitId"]));
                                        rvo59.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader11["IsCredit"]));
                                        dR = rvo59.DR;
                                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                                        {
                                            svo.Details.PurchaseLedger.Add(rvo59);
                                        }
                                    }
                                    break;
                                }
                                clsLedgerVO rvo58 = new clsLedgerVO {
                                    LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader11["LedgerName"])),
                                    Narration = Convert.ToString(DALHelper.HandleDBNull(reader11["GRNIDs"])),
                                    CR = 0.0,
                                    DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader11["Amount"])))
                                };
                                if (svo.Details.PurchaseLedger == null)
                                {
                                    rvo58.RowID = 1L;
                                }
                                else
                                {
                                    rvo58.RowID = (svo.Details.PurchaseLedger.SingleOrDefault<clsLedgerVO>(S => S.RowID.Equals((long) 1L)) != null) ? 0L : 1L;
                                }
                                rvo58.VoucherType = "Purchase";
                                rvo58.TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader11["TransactionType"]));
                                rvo58.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader11["GPVoucherType"]));
                                rvo58.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader11["VoucherMode"]));
                                rvo58.VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader11["VatAmount"])));
                                rvo58.TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader11["TotalAmount"])));
                                rvo58.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader11["VendorID"]));
                                rvo58.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(reader11["TransactionNo"]));
                                rvo58.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader11["GRNUnitId"]));
                                rvo58.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader11["IsCredit"]));
                                dR = rvo58.DR;
                                if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                                {
                                    svo.Details.PurchaseLedger.Add(rvo58);
                                }
                            }
                            break;
                        }
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader11["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader11["GRNIDs"])),
                            DR = 0.0,
                            CR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader11["Amount"])))
                        };
                        if (svo.Details.PurchaseLedger == null)
                        {
                            item.RowID = 1L;
                        }
                        else
                        {
                            item.RowID = (svo.Details.PurchaseLedger.SingleOrDefault<clsLedgerVO>(S => S.RowID.Equals((long) 1L)) != null) ? 0L : 1L;
                        }
                        item.VoucherType = "Purchase";
                        item.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader11["GPVoucherType"]));
                        item.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader11["VoucherMode"]));
                        item.VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader11["VatAmount"])));
                        item.TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader11["TotalAmount"])));
                        item.NetAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader11["NetAmount"])));
                        item.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader11["VendorID"]));
                        item.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(reader11["TransactionNo"]));
                        item.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader11["GRNUnitId"]));
                        item.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader11["IsCredit"]));
                        dR = item.CR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.PurchaseLedger.Add(item);
                        }
                    }
                }
                DbCommand command13 = this.dbServer.GetStoredProcCommand("CIMS_GetAccountSaleIncome");
                this.dbServer.AddInParameter(command13, "UnitId", DbType.Int64, svo.Details.UnitID);
                this.dbServer.AddInParameter(command13, "Date", DbType.DateTime, svo.Details.ExportDate);
                DbDataReader reader12 = (DbDataReader) this.dbServer.ExecuteReader(command13);
                source = new List<clsLedgerVO>();
                list2 = new List<clsLedgerVO>();
                if (svo.Details.SaleSelfReceiptLedgers == null)
                {
                    svo.Details.SaleSelfReceiptLedgers = new List<clsLedgerVO>();
                }
                if (reader12.HasRows)
                {
                    while (reader12.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader12["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader12["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader12["Amount"]))),
                            CR = 0.0,
                            VoucherType = "Sales",
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader12["IsCredit"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.SaleIncome.Add(item);
                        }
                    }
                }
                reader12.NextResult();
                if (reader12.HasRows)
                {
                    while (reader12.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader12["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader12["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader12["Amount"]))),
                            CR = 0.0,
                            VoucherType = "Sales",
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader12["IsCredit"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.SaleIncome.Add(item);
                        }
                    }
                }
                reader12.NextResult();
                if (reader12.HasRows)
                {
                    while (reader12.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader12["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader12["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader12["Amount"]))),
                            CR = 0.0,
                            VoucherType = "Sales",
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader12["IsCredit"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.SaleIncome.Add(item);
                        }
                    }
                }
                reader12.NextResult();
                if (reader12.HasRows)
                {
                    while (reader12.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader12["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader12["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader12["Amount"]))),
                            CR = 0.0,
                            VoucherType = "Sales",
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader12["IsCredit"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.SaleIncome.Add(item);
                        }
                    }
                }
                reader12.NextResult();
                if (reader12.HasRows)
                {
                    while (reader12.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader12["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader12["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader12["TotalConcession"]))),
                            CR = 0.0,
                            VoucherType = "Sales",
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader12["IsCredit"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.SaleIncome.Add(item);
                        }
                    }
                }
                reader12.NextResult();
                if (reader12.HasRows)
                {
                    while (reader12.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader12["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader12["Narration"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader12["PatientBalance"]))),
                            CR = 0.0,
                            VoucherType = "Sales",
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader12["IsCredit"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.SaleIncome.Add(item);
                        }
                    }
                }
                reader12.NextResult();
                if (reader12.HasRows)
                {
                    while (reader12.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader12["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader12["BillIDs"])),
                            DR = 0.0,
                            CR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader12["Amount"])))
                        };
                        if (svo.Details.SaleIncome == null)
                        {
                            item.RowID = 1L;
                        }
                        else
                        {
                            item.RowID = (svo.Details.SaleIncome.SingleOrDefault<clsLedgerVO>(S => S.RowID.Equals((long) 1L)) != null) ? 0L : 1L;
                        }
                        item.VoucherType = "Sales";
                        item.TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader12["TransactionType"]));
                        item.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader12["GPVoucherType"]));
                        item.PatientType = Convert.ToString(DALHelper.HandleDBNull(reader12["PatientType"]));
                        item.Sponsor = Convert.ToString(DALHelper.HandleDBNull(reader12["SponsorType"]));
                        item.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader12["VoucherMode"]));
                        item.PatientNo = Convert.ToString(DALHelper.HandleDBNull(reader12["PatientNo"]));
                        item.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(reader12["Pat_Comp_Name"]));
                        item.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(reader12["TransactionGroup"]));
                        item.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(reader12["TransactionId"]));
                        item.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader12["Remark"]));
                        item.VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader12["VatAmount"])));
                        item.TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader12["TotalAmount"])));
                        item.NetAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader12["NetAmount"])));
                        item.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader12["VendorID"]));
                        item.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader12["PurchaseInvoiceNo"]));
                        item.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(reader12["TransactionNo"]));
                        item.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader12["IsCredit"]));
                        dR = item.CR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.SaleIncome.Add(item);
                        }
                    }
                }
                reader12.NextResult();
                if (reader12.HasRows)
                {
                    while (reader12.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader12["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader12["BillIDs"])),
                            DR = 0.0,
                            CR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader12["Amount"])))
                        };
                        if (svo.Details.SaleIncome == null)
                        {
                            item.RowID = 1L;
                        }
                        else
                        {
                            item.RowID = (svo.Details.SaleIncome.SingleOrDefault<clsLedgerVO>(S => S.RowID.Equals((long) 1L)) != null) ? 0L : 1L;
                        }
                        item.VoucherType = "Sales";
                        item.TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader12["TransactionType"]));
                        item.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader12["GPVoucherType"]));
                        item.PatientType = Convert.ToString(DALHelper.HandleDBNull(reader12["PatientType"]));
                        item.Sponsor = Convert.ToString(DALHelper.HandleDBNull(reader12["SponsorType"]));
                        item.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader12["VoucherMode"]));
                        item.PatientNo = Convert.ToString(DALHelper.HandleDBNull(reader12["PatientNo"]));
                        item.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(reader12["Pat_Comp_Name"]));
                        item.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(reader12["TransactionGroup"]));
                        item.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(reader12["TransactionId"]));
                        item.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader12["Remark"]));
                        item.VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader12["VatAmount"])));
                        item.TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader12["TotalAmount"])));
                        item.NetAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader12["NetAmount"])));
                        item.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader12["VendorID"]));
                        item.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader12["PurchaseInvoiceNo"]));
                        item.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(reader12["TransactionNo"]));
                        item.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader12["IsCredit"]));
                        dR = item.CR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.SaleIncome.Add(item);
                        }
                    }
                }
                reader12.Close();
                DbCommand command14 = this.dbServer.GetStoredProcCommand("CIMS_GetAccItemSaleCreditBill");
                this.dbServer.AddInParameter(command14, "UnitId", DbType.Int64, svo.Details.UnitID);
                this.dbServer.AddInParameter(command14, "Date", DbType.DateTime, svo.Details.ExportDate);
                DbDataReader reader13 = (DbDataReader) this.dbServer.ExecuteReader(command14);
                source = new List<clsLedgerVO>();
                list2 = new List<clsLedgerVO>();
                if (svo.Details.SaleCredit == null)
                {
                    svo.Details.SaleCredit = new List<clsLedgerVO>();
                }
                if (reader13.HasRows)
                {
                    while (reader13.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader13["ID"])),
                            LedgerName = (string) DALHelper.HandleDBNull(reader13["CashInHand"]),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader13["Narration"])),
                            Reference = Convert.ToString(DALHelper.HandleDBNull(reader13["BillNo"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["Amount"]))),
                            CR = 0.0,
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader13["IsCredit"])),
                            VoucherType = "Sales",
                            BalanceAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["BalanceAmountSelf"]))),
                            TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader13["TransactionType"])),
                            GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader13["GPVoucherType"])),
                            PatientType = Convert.ToString(DALHelper.HandleDBNull(reader13["PatientType"])),
                            Sponsor = Convert.ToString(DALHelper.HandleDBNull(reader13["SponsorType"])),
                            VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader13["VoucherMode"])),
                            PatientNo = Convert.ToString(DALHelper.HandleDBNull(reader13["PatientNo"])),
                            Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(reader13["Pat_Comp_Name"])),
                            TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(reader13["TransactionGroup"])),
                            TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(reader13["TransactionId"])),
                            Remarks = Convert.ToString(DALHelper.HandleDBNull(reader13["Remark"])),
                            VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["VatAmount"]))),
                            TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["TotalAmount"]))),
                            NetAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["NetAmount"]))),
                            VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader13["VendorID"])),
                            PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader13["PurchaseInvoiceNo"])),
                            TransactionNo = Convert.ToString(DALHelper.HandleDBNull(reader13["TransactionNo"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.SaleCredit.Add(item);
                        }
                    }
                }
                reader13.NextResult();
                if (reader13.HasRows)
                {
                    while (reader13.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader13["ID"])),
                            LedgerName = (string) DALHelper.HandleDBNull(reader13["CashInHand"]),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader13["Narration"])),
                            Reference = Convert.ToString(DALHelper.HandleDBNull(reader13["BillNo"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["Amount"]))),
                            CR = 0.0,
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader13["IsCredit"])),
                            VoucherType = "Sales",
                            BalanceAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["BalanceAmountSelf"]))),
                            TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader13["TransactionType"])),
                            GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader13["GPVoucherType"])),
                            PatientType = Convert.ToString(DALHelper.HandleDBNull(reader13["PatientType"])),
                            Sponsor = Convert.ToString(DALHelper.HandleDBNull(reader13["SponsorType"])),
                            VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader13["VoucherMode"])),
                            PatientNo = Convert.ToString(DALHelper.HandleDBNull(reader13["PatientNo"])),
                            Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(reader13["Pat_Comp_Name"])),
                            TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(reader13["TransactionGroup"])),
                            TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(reader13["TransactionId"])),
                            Remarks = Convert.ToString(DALHelper.HandleDBNull(reader13["Remark"])),
                            VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["VatAmount"]))),
                            TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["TotalAmount"]))),
                            NetAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["NetAmount"]))),
                            VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader13["VendorID"])),
                            PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader13["PurchaseInvoiceNo"])),
                            TransactionNo = Convert.ToString(DALHelper.HandleDBNull(reader13["TransactionNo"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.SaleCredit.Add(item);
                        }
                    }
                }
                reader13.NextResult();
                if (reader13.HasRows)
                {
                    while (reader13.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader13["ID"])),
                            LedgerName = (string) DALHelper.HandleDBNull(reader13["CashInHand"]),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader13["Narration"])),
                            Reference = Convert.ToString(DALHelper.HandleDBNull(reader13["BillNo"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["Amount"]))),
                            CR = 0.0,
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader13["IsCredit"])),
                            VoucherType = "Sales",
                            BalanceAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["BalanceAmountSelf"]))),
                            TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader13["TransactionType"])),
                            GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader13["GPVoucherType"])),
                            PatientType = Convert.ToString(DALHelper.HandleDBNull(reader13["PatientType"])),
                            Sponsor = Convert.ToString(DALHelper.HandleDBNull(reader13["SponsorType"])),
                            VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader13["VoucherMode"])),
                            PatientNo = Convert.ToString(DALHelper.HandleDBNull(reader13["PatientNo"])),
                            Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(reader13["Pat_Comp_Name"])),
                            TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(reader13["TransactionGroup"])),
                            TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(reader13["TransactionId"])),
                            Remarks = Convert.ToString(DALHelper.HandleDBNull(reader13["Remark"])),
                            VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["VatAmount"]))),
                            TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["TotalAmount"]))),
                            NetAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["NetAmount"]))),
                            VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader13["VendorID"])),
                            PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader13["PurchaseInvoiceNo"])),
                            TransactionNo = Convert.ToString(DALHelper.HandleDBNull(reader13["TransactionNo"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.SaleCredit.Add(item);
                        }
                    }
                }
                reader13.NextResult();
                if (reader13.HasRows)
                {
                    while (reader13.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader13["ID"])),
                            LedgerName = (string) DALHelper.HandleDBNull(reader13["CashInHand"]),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader13["Narration"])),
                            Reference = Convert.ToString(DALHelper.HandleDBNull(reader13["BillNo"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["Amount"]))),
                            CR = 0.0,
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader13["IsCredit"])),
                            VoucherType = "Sales",
                            BalanceAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["BalanceAmountSelf"]))),
                            TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader13["TransactionType"])),
                            GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader13["GPVoucherType"])),
                            PatientType = Convert.ToString(DALHelper.HandleDBNull(reader13["PatientType"])),
                            Sponsor = Convert.ToString(DALHelper.HandleDBNull(reader13["SponsorType"])),
                            VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader13["VoucherMode"])),
                            PatientNo = Convert.ToString(DALHelper.HandleDBNull(reader13["PatientNo"])),
                            Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(reader13["Pat_Comp_Name"])),
                            TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(reader13["TransactionGroup"])),
                            TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(reader13["TransactionId"])),
                            Remarks = Convert.ToString(DALHelper.HandleDBNull(reader13["Remark"])),
                            VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["VatAmount"]))),
                            TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["TotalAmount"]))),
                            NetAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["NetAmount"]))),
                            VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader13["VendorID"])),
                            PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader13["PurchaseInvoiceNo"])),
                            TransactionNo = Convert.ToString(DALHelper.HandleDBNull(reader13["TransactionNo"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.SaleCredit.Add(item);
                        }
                    }
                }
                reader13.NextResult();
                if (reader13.HasRows)
                {
                    while (reader13.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader13["ID"])),
                            LedgerName = (string) DALHelper.HandleDBNull(reader13["CashInHand"]),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader13["Narration"])),
                            Reference = Convert.ToString(DALHelper.HandleDBNull(reader13["BillNo"])),
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["Amount"]))),
                            CR = 0.0,
                            IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader13["IsCredit"])),
                            VoucherType = "Sales",
                            BalanceAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["BalanceAmountSelf"]))),
                            TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader13["TransactionType"])),
                            GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader13["GPVoucherType"])),
                            PatientType = Convert.ToString(DALHelper.HandleDBNull(reader13["PatientType"])),
                            Sponsor = Convert.ToString(DALHelper.HandleDBNull(reader13["SponsorType"])),
                            VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader13["VoucherMode"])),
                            PatientNo = Convert.ToString(DALHelper.HandleDBNull(reader13["PatientNo"])),
                            Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(reader13["Pat_Comp_Name"])),
                            TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(reader13["TransactionGroup"])),
                            TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(reader13["TransactionId"])),
                            Remarks = Convert.ToString(DALHelper.HandleDBNull(reader13["Remark"])),
                            VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["VatAmount"]))),
                            TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["TotalAmount"]))),
                            NetAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["NetAmount"]))),
                            VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader13["VendorID"])),
                            PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader13["PurchaseInvoiceNo"])),
                            TransactionNo = Convert.ToString(DALHelper.HandleDBNull(reader13["TransactionNo"]))
                        };
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.SaleCredit.Add(item);
                        }
                    }
                }
                reader13.NextResult();
                if (reader13.HasRows)
                {
                    while (reader13.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader13["ID"])),
                            LedgerName = (string) DALHelper.HandleDBNull(reader13["CashInHand"]),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader13["Narration"])),
                            Reference = Convert.ToString(DALHelper.HandleDBNull(reader13["BillNo"])),
                            CR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["Amount"]))),
                            DR = 0.0
                        };
                        if (source == null)
                        {
                            item.RowID = 1L;
                        }
                        else
                        {
                            item.RowID = (svo.Details.SaleCredit.SingleOrDefault<clsLedgerVO>(S => S.RowID.Equals((long) 1L)) != null) ? 0L : 1L;
                        }
                        item.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader13["IsCredit"]));
                        item.VoucherType = "Sales";
                        item.BalanceAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["BalanceAmountSelf"])));
                        item.TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader13["TransactionType"]));
                        item.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader13["GPVoucherType"]));
                        item.PatientType = Convert.ToString(DALHelper.HandleDBNull(reader13["PatientType"]));
                        item.Sponsor = Convert.ToString(DALHelper.HandleDBNull(reader13["SponsorType"]));
                        item.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader13["VoucherMode"]));
                        item.PatientNo = Convert.ToString(DALHelper.HandleDBNull(reader13["PatientNo"]));
                        item.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(reader13["Pat_Comp_Name"]));
                        item.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(reader13["TransactionGroup"]));
                        item.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(reader13["TransactionId"]));
                        item.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader13["Remark"]));
                        item.VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["VatAmount"])));
                        item.TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["TotalAmount"])));
                        item.NetAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader13["NetAmount"])));
                        item.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader13["VendorID"]));
                        item.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader13["PurchaseInvoiceNo"]));
                        item.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(reader13["TransactionNo"]));
                        dR = item.CR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.SaleCredit.Add(item);
                        }
                    }
                }
                reader13.Close();
                DbCommand command15 = this.dbServer.GetStoredProcCommand("CIMS_GetAccItemSaleReturn");
                this.dbServer.AddInParameter(command15, "UnitId", DbType.Int64, svo.Details.UnitID);
                this.dbServer.AddInParameter(command15, "Date", DbType.DateTime, svo.Details.ExportDate);
                DbDataReader reader14 = (DbDataReader) this.dbServer.ExecuteReader(command15);
                source = new List<clsLedgerVO>();
                list2 = new List<clsLedgerVO>();
                if (reader14.HasRows)
                {
                    while (reader14.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader14["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader14["GRNIDs"])),
                            DR = 0.0,
                            CR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader14["Amount"])))
                        };
                        if (svo.Details.ItemSaleReturnLedgers == null)
                        {
                            item.RowID = 1L;
                        }
                        else
                        {
                            item.RowID = (svo.Details.ItemSaleReturnLedgers.SingleOrDefault<clsLedgerVO>(S => S.RowID.Equals((long) 1L)) != null) ? 0L : 1L;
                        }
                        item.VoucherType = "Credit Note";
                        item.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader14["GPVoucherType"]));
                        item.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader14["VoucherMode"]));
                        item.VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader14["VatAmount"])));
                        item.TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader14["TotalAmount"])));
                        item.NetAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader14["NetAmount"])));
                        item.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader14["VendorID"]));
                        item.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(reader14["TransactionNo"]));
                        item.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader14["GRNUnitId"]));
                        item.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader14["IsCredit"]));
                        dR = item.CR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.SaleReturn.Add(item);
                        }
                    }
                }
                reader14.NextResult();
                if (reader14.HasRows)
                {
                    if (svo.Details.SaleReturn == null)
                    {
                        svo.Details.SaleReturn = new List<clsLedgerVO>();
                    }
                    while (reader14.Read())
                    {
                        clsLedgerVO item = new clsLedgerVO {
                            LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader14["LedgerName"])),
                            Narration = Convert.ToString(DALHelper.HandleDBNull(reader14["GRNIDs"])),
                            CR = 0.0,
                            DR = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader14["Amount"])))
                        };
                        if (svo.Details.SaleReturn == null)
                        {
                            item.RowID = 1L;
                        }
                        else
                        {
                            item.RowID = (svo.Details.SaleReturn.SingleOrDefault<clsLedgerVO>(S => S.RowID.Equals((long) 1L)) != null) ? 0L : 1L;
                        }
                        item.VoucherType = "Credit Note";
                        item.TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader14["TransactionType"]));
                        item.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader14["GPVoucherType"]));
                        item.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader14["VoucherMode"]));
                        item.VatAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader14["VatAmount"])));
                        item.TotalAmount = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader14["TotalAmount"])));
                        item.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader14["VendorID"]));
                        item.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(reader14["TransactionNo"]));
                        item.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader14["GRNUnitId"]));
                        item.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader14["IsCredit"]));
                        dR = item.DR;
                        if ((dR.GetValueOrDefault() > 0.0) && (dR != null))
                        {
                            svo.Details.SaleReturn.Add(item);
                        }
                    }
                }
                reader14.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject MaintainPaymentLog(IValueObject valueObject, clsUserVO UserVo)
        {
            throw new NotImplementedException();
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

        public override IValueObject UpdateBillPaymentDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            clsUpdateBillPaymentDtlsBizActionVO nvo = valueObject as clsUpdateBillPaymentDtlsBizActionVO;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsBillVO details = nvo.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateBillPaymentDetails");
                this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.Int64, details.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, details.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CostingDivisionID", DbType.Int64, details.CostingDivisionID);
                if (details.BalanceAmountSelf < 0.0)
                {
                    details.BalanceAmountSelf = 0.0;
                }
                this.dbServer.AddInParameter(storedProcCommand, "BalanceAmountSelf", DbType.Double, details.BalanceAmountSelf);
                this.dbServer.AddInParameter(storedProcCommand, "TotalConcessionAmount", DbType.Double, details.TotalConcessionAmount);
                this.dbServer.AddInParameter(storedProcCommand, "NetBillAmount", DbType.Double, details.NetBillAmount);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, details.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                foreach (clsChargeVO evo in nvo.ChargeDetails)
                {
                    evo.ChargeDetails = new clsChargeDetailsVO();
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_UpdateCharge");
                    this.dbServer.AddInParameter(command2, "ID", DbType.Int64, evo.ID);
                    this.dbServer.AddInParameter(command2, "Date", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "CostingDivisionID", DbType.Int64, details.CostingDivisionID);
                    this.dbServer.AddInParameter(command2, "PaidAmount", DbType.Double, evo.TotalServicePaidAmount);
                    this.dbServer.AddInParameter(command2, "NetAmount", DbType.Double, evo.TotalNetAmount);
                    this.dbServer.AddInParameter(command2, "Concession", DbType.Double, evo.TotalConcession);
                    this.dbServer.AddInParameter(command2, "ConcessionPercentage", DbType.Double, evo.TotalConcessionPercentage);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    evo.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                    if (evo.IsUpdate)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_UpdateChargeDetails");
                        this.dbServer.AddInParameter(command3, "ChargeID ", DbType.Int64, evo.ID);
                        this.dbServer.AddInParameter(command3, "Date", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "PaidAmount", DbType.Double, evo.ServicePaidAmount);
                        this.dbServer.AddInParameter(command3, "Concession", DbType.Double, evo.Concession);
                        this.dbServer.AddInParameter(command3, "ServiceTaxAmount", DbType.Double, evo.ServiceTaxAmount);
                        this.dbServer.AddInParameter(command3, "NetAmount", DbType.Double, evo.SettleNetAmount);
                        this.dbServer.AddInParameter(command3, "BalanceAmount", DbType.Double, evo.BalanceAmount);
                        this.dbServer.AddInParameter(command3, "IsSameDate", DbType.Boolean, evo.IsSameDate);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                        continue;
                    }
                    DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_AddChargeDetails");
                    this.dbServer.AddInParameter(command4, "ChargeID", DbType.Int64, evo.ID);
                    this.dbServer.AddInParameter(command4, "ChargeUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command4, "Date", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command4, "Quantity", DbType.Double, evo.Quantity);
                    this.dbServer.AddInParameter(command4, "Rate", DbType.Double, evo.Rate);
                    this.dbServer.AddInParameter(command4, "TotalAmount", DbType.Double, evo.TotalAmount);
                    this.dbServer.AddInParameter(command4, "ConcessionAmount", DbType.Double, evo.Concession);
                    this.dbServer.AddInParameter(command4, "ServiceTaxAmount", DbType.Double, evo.ServiceTaxAmount);
                    this.dbServer.AddInParameter(command4, "NetAmount", DbType.Double, evo.SettleNetAmount);
                    this.dbServer.AddInParameter(command4, "PaidAmount", DbType.Double, evo.ServicePaidAmount);
                    this.dbServer.AddInParameter(command4, "BalanceAmount", DbType.Double, evo.BalanceAmount);
                    this.dbServer.AddInParameter(command4, "Status", DbType.Boolean, evo.Status);
                    this.dbServer.AddInParameter(command4, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command4, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command4, "RefundID", DbType.Int64, evo.RefundID);
                    this.dbServer.AddInParameter(command4, "RefundAmount", DbType.Double, evo.RefundAmount);
                    this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.ChargeDetails.ID);
                    this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command4, transaction);
                    evo.ChargeDetails.SuccessStatus = (int) this.dbServer.GetParameterValue(command4, "ResultStatus");
                    evo.ChargeDetails.ID = (long) this.dbServer.GetParameterValue(command4, "ID");
                }
                transaction.Commit();
                nvo.SuccessStatus = 0;
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                transaction.Rollback();
                nvo.ChargeDetails = null;
                nvo.ChargeDetails = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject UpdateBillPaymentDetailsWithTransaction(IValueObject valueObject, clsUserVO UserVo, DbTransaction pTransaction, DbConnection pConnection)
        {
            clsUpdateBillPaymentDtlsBizActionVO nvo = valueObject as clsUpdateBillPaymentDtlsBizActionVO;
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
                clsBillVO details = nvo.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateBillPaymentDetails");
                storedProcCommand.Connection = pConnection;
                this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.Int64, details.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, details.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CostingDivisionID", DbType.Int64, details.CostingDivisionID);
                if (details.BalanceAmountSelf < 0.0)
                {
                    details.BalanceAmountSelf = 0.0;
                }
                this.dbServer.AddInParameter(storedProcCommand, "BalanceAmountSelf", DbType.Double, details.BalanceAmountSelf);
                this.dbServer.AddInParameter(storedProcCommand, "TotalConcessionAmount", DbType.Double, details.TotalConcessionAmount);
                this.dbServer.AddInParameter(storedProcCommand, "NetBillAmount", DbType.Double, details.NetBillAmount);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, details.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, details.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, pTransaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if ((nvo.ChargeDetails != null) && (nvo.ChargeDetails.Count > 0))
                {
                    foreach (clsChargeVO evo in nvo.ChargeDetails)
                    {
                        evo.ChargeDetails = new clsChargeDetailsVO();
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_UpdateCharge");
                        this.dbServer.AddInParameter(command2, "ID", DbType.Int64, evo.ID);
                        this.dbServer.AddInParameter(command2, "Date", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "CostingDivisionID", DbType.Int64, details.CostingDivisionID);
                        this.dbServer.AddInParameter(command2, "PaidAmount", DbType.Double, evo.TotalServicePaidAmount);
                        this.dbServer.AddInParameter(command2, "NetAmount", DbType.Double, evo.TotalNetAmount);
                        this.dbServer.AddInParameter(command2, "Concession", DbType.Double, evo.TotalConcession);
                        this.dbServer.AddInParameter(command2, "ConcessionPercentage", DbType.Double, evo.TotalConcessionPercentage);
                        this.dbServer.ExecuteNonQuery(command2, pTransaction);
                        evo.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                        if (evo.IsUpdate)
                        {
                            DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_UpdateChargeDetails");
                            this.dbServer.AddInParameter(command3, "ChargeID ", DbType.Int64, evo.ID);
                            this.dbServer.AddInParameter(command3, "Date", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command3, "PaidAmount", DbType.Double, evo.ServicePaidAmount);
                            this.dbServer.AddInParameter(command3, "Concession", DbType.Double, evo.Concession);
                            this.dbServer.AddInParameter(command3, "ServiceTaxAmount", DbType.Double, evo.ServiceTaxAmount);
                            this.dbServer.AddInParameter(command3, "NetAmount", DbType.Double, evo.SettleNetAmount);
                            this.dbServer.AddInParameter(command3, "BalanceAmount", DbType.Double, evo.BalanceAmount);
                            this.dbServer.AddInParameter(command3, "IsSameDate", DbType.Boolean, evo.IsSameDate);
                            this.dbServer.ExecuteNonQuery(command3, pTransaction);
                            continue;
                        }
                        DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_AddChargeDetails");
                        this.dbServer.AddInParameter(command4, "ChargeID", DbType.Int64, evo.ID);
                        this.dbServer.AddInParameter(command4, "ChargeUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command4, "Date", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command4, "Quantity", DbType.Double, evo.Quantity);
                        this.dbServer.AddInParameter(command4, "Rate", DbType.Double, evo.Rate);
                        this.dbServer.AddInParameter(command4, "TotalAmount", DbType.Double, evo.TotalAmount);
                        this.dbServer.AddInParameter(command4, "ConcessionAmount", DbType.Double, evo.Concession);
                        this.dbServer.AddInParameter(command4, "ServiceTaxAmount", DbType.Double, evo.ServiceTaxAmount);
                        this.dbServer.AddInParameter(command4, "NetAmount", DbType.Double, evo.SettleNetAmount);
                        this.dbServer.AddInParameter(command4, "PaidAmount", DbType.Double, evo.ServicePaidAmount);
                        this.dbServer.AddInParameter(command4, "BalanceAmount", DbType.Double, evo.BalanceAmount);
                        this.dbServer.AddInParameter(command4, "Status", DbType.Boolean, evo.Status);
                        this.dbServer.AddInParameter(command4, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command4, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command4, "RefundID", DbType.Int64, evo.RefundID);
                        this.dbServer.AddInParameter(command4, "RefundAmount", DbType.Double, evo.RefundAmount);
                        this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.ChargeDetails.ID);
                        this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command4, pTransaction);
                        evo.ChargeDetails.SuccessStatus = (int) this.dbServer.GetParameterValue(command4, "ResultStatus");
                        evo.ChargeDetails.ID = (long) this.dbServer.GetParameterValue(command4, "ID");
                    }
                }
                nvo.SuccessStatus = 0;
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                nvo.ChargeDetails = null;
                nvo.ChargeDetails = null;
            }
            return nvo;
        }

        private clsAddBillBizActionVO UpdateDetails(clsAddBillBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection pConnection = null;
            try
            {
                int num5;
                object[] objArray;
                pConnection = this.dbServer.CreateConnection();
                pConnection.Open();
                transaction = pConnection.BeginTransaction();
                clsBillVO objDetailsVO = BizActionObj.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateChargeDetailsNew");
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
                    if (i < objDetailsVO.ChargeDetails.Count)
                    {
                        if (!objDetailsVO.ChargeDetails[i].ChildPackageService)
                        {
                            clsBaseChargeDAL instance = clsBaseChargeDAL.GetInstance();
                            clsAddChargeBizActionVO valueObject = new clsAddChargeBizActionVO {
                                Details = objDetailsVO.ChargeDetails[i]
                            };
                            valueObject.Details.IsBilled = BizActionObj.Details.IsFreezed;
                            valueObject.Details.Date = new DateTime?(objDetailsVO.Date);
                            valueObject.Details.PaidAmount = !valueObject.Details.Status ? 0.0 : valueObject.Details.NetAmount;
                            valueObject.Details.Opd_Ipd_External_Id = objDetailsVO.Opd_Ipd_External_Id;
                            valueObject.Details.Opd_Ipd_External_UnitId = objDetailsVO.Opd_Ipd_External_UnitId;
                            valueObject.Details.Opd_Ipd_External = objDetailsVO.Opd_Ipd_External;
                            valueObject.Details.ClassId = objDetailsVO.ChargeDetails[i].ClassId;
                            valueObject.Details.IsIPDBill = objDetailsVO.IsIPDBill;
                            if (objDetailsVO.ChargeDetails[0].isPackageService)
                            {
                                iParentID = objDetailsVO.ChargeDetails[0].ID;
                            }
                            valueObject = (clsAddChargeBizActionVO) instance.Add(valueObject, UserVo, pConnection, transaction, iParentID, 0L);
                            if (valueObject.SuccessStatus == -1)
                            {
                                throw new Exception();
                            }
                            valueObject.Details.ID = valueObject.Details.ID;
                            objDetailsVO.ChargeDetails[i].ID = valueObject.Details.ID;
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
                                nvo2.Details.IsBilled = BizActionObj.Details.IsFreezed;
                                nvo2.Details.Date = new DateTime?(objDetailsVO.Date);
                                nvo2.Details.Opd_Ipd_External_Id = objDetailsVO.Opd_Ipd_External_Id;
                                nvo2.Details.Opd_Ipd_External_UnitId = objDetailsVO.Opd_Ipd_External_UnitId;
                                nvo2.Details.Opd_Ipd_External = objDetailsVO.Opd_Ipd_External;
                                valueObject.Details.ClassId = objDetailsVO.ChargeDetails[i].ClassId;
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
                        continue;
                    }
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_UpdateChargeNew");
                    this.dbServer.AddInParameter(command2, "BillID", DbType.Int64, objDetailsVO.ID);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, objDetailsVO.UnitID);
                    this.dbServer.AddInParameter(command2, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    if (BizActionObj.Details.DeleteChargeDetails.Count > 0)
                    {
                        foreach (clsChargeVO evo2 in BizActionObj.Details.DeleteChargeDetails)
                        {
                            if (evo2.PrescriptionDetailsID > 0L)
                            {
                                DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand("UPDATE T_PatientEMRDiagnosisDetails SET BillDone=0 ,BillID=0 where ID= " + evo2.PrescriptionDetailsID);
                                this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                                continue;
                            }
                            if (evo2.InvestigationDetailsID > 0L)
                            {
                                DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand("UPDATE T_DoctorSuggestedServiceDetail SET BillDone=0 ,BillID=0 where ID= " + evo2.InvestigationDetailsID);
                                this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                            }
                        }
                    }
                    if (BizActionObj.DeletedRadSerDetailsList.Count > 0)
                    {
                        foreach (clsChargeVO evo3 in BizActionObj.DeletedRadSerDetailsList)
                        {
                            if (evo3.PrescriptionDetailsID > 0L)
                            {
                                DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand("UPDATE T_PatientEMRDiagnosisDetails SET BillDone=0 ,BillID=0 where ID= " + evo3.PrescriptionDetailsID);
                                this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                                continue;
                            }
                            if (evo3.InvestigationDetailsID > 0L)
                            {
                                DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand("UPDATE T_DoctorSuggestedServiceDetail SET BillDone=0 ,BillID=0 where ID= " + evo3.InvestigationDetailsID);
                                this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                            }
                        }
                    }
                    DbCommand command7 = this.dbServer.GetStoredProcCommand("CIMS_UpdateBill");
                    this.dbServer.AddInParameter(command7, "IsPackageBill", DbType.Int64, BizActionObj.IsPackageBill);
                    if (BizActionObj.IsPackageBill)
                    {
                        StringBuilder builder = new StringBuilder();
                        foreach (clsChargeVO evo4 in objDetailsVO.ChargeDetails)
                        {
                            if ((evo4.PackageID > 0L) && evo4.isPackageService)
                            {
                                builder.Append("," + Convert.ToString(evo4.PackageID));
                            }
                        }
                        builder.Replace(",", "", 0, 1);
                        this.dbServer.AddInParameter(command7, "ipPackageList", DbType.String, Convert.ToString(builder));
                    }
                    this.dbServer.AddInParameter(command7, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                    if (objDetailsVO.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(command7, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddInParameter(command7, "Date", DbType.DateTime, objDetailsVO.Date);
                    this.dbServer.AddInParameter(command7, "TotalBillAmount", DbType.Double, objDetailsVO.TotalBillAmount);
                    this.dbServer.AddInParameter(command7, "TotalConcessionAmount", DbType.Double, objDetailsVO.TotalConcessionAmount);
                    this.dbServer.AddInParameter(command7, "NetBillAmount", DbType.Double, objDetailsVO.NetBillAmount);
                    this.dbServer.AddInParameter(command7, "CalculatedNetBillAmount", DbType.Double, objDetailsVO.CalculatedNetBillAmount);
                    this.dbServer.AddInParameter(command7, "SelfAmount", DbType.Double, objDetailsVO.SelfAmount);
                    if (objDetailsVO.BalanceAmountSelf < 0.0)
                    {
                        objDetailsVO.BalanceAmountSelf = 0.0;
                    }
                    this.dbServer.AddInParameter(command7, "BalanceAmountSelf", DbType.Double, objDetailsVO.BalanceAmountSelf);
                    this.dbServer.AddInParameter(command7, "NonSelfAmount", DbType.Double, objDetailsVO.NonSelfAmount);
                    this.dbServer.AddInParameter(command7, "BalanceAmountNonSelf", DbType.Double, objDetailsVO.BalanceAmountNonSelf);
                    this.dbServer.AddInParameter(command7, "IsPrinted", DbType.Boolean, objDetailsVO.IsPrinted);
                    this.dbServer.AddInParameter(command7, "SponserType", DbType.Boolean, objDetailsVO.SponserType);
                    if (objDetailsVO.Remark != null)
                    {
                        objDetailsVO.Remark = objDetailsVO.Remark.Trim();
                    }
                    this.dbServer.AddInParameter(command7, "Remark", DbType.String, objDetailsVO.Remark);
                    if (objDetailsVO.BillRemark != null)
                    {
                        objDetailsVO.BillRemark = objDetailsVO.BillRemark.Trim();
                    }
                    this.dbServer.AddInParameter(command7, "BillRemark", DbType.String, objDetailsVO.BillRemark);
                    this.dbServer.AddInParameter(command7, "BillType", DbType.Int16, (short) objDetailsVO.BillType);
                    this.dbServer.AddInParameter(command7, "BillPaymentType", DbType.Int16, (short) objDetailsVO.BillPaymentType);
                    this.dbServer.AddInParameter(command7, "IsFreezed", DbType.Boolean, objDetailsVO.IsFreezed);
                    this.dbServer.AddInParameter(command7, "ConcessionReasonId", DbType.Int64, objDetailsVO.ConcessionReasonId);
                    this.dbServer.AddInParameter(command7, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);
                    this.dbServer.AddInParameter(command7, "GrossDiscountReason", DbType.Int64, objDetailsVO.GrossDiscountReasonID);
                    this.dbServer.AddInParameter(command7, "GrossDiscountPercentage", DbType.Double, objDetailsVO.GrossDiscountPercentage);
                    this.dbServer.AddInParameter(command7, "ConcessionAuthorizedBy", DbType.String, objDetailsVO.ConcessionAuthorizedBy);
                    this.dbServer.AddInParameter(command7, "ConcessionRemark", DbType.String, objDetailsVO.ConcessionRemark);
                    this.dbServer.AddInParameter(command7, "Status", DbType.Boolean, objDetailsVO.Status);
                    this.dbServer.AddInParameter(command7, "UnitID", DbType.Int64, objDetailsVO.UnitID);
                    this.dbServer.AddInParameter(command7, "AgainstDonor", DbType.Boolean, objDetailsVO.AgainstDonor);
                    this.dbServer.AddInParameter(command7, "LinkPatientID", DbType.Int64, objDetailsVO.LinkPatientID);
                    this.dbServer.AddInParameter(command7, "LinkPatientUnitID", DbType.Int64, objDetailsVO.LinkPatientUnitID);
                    this.dbServer.AddInParameter(command7, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command7, "UpdatedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command7, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command7, "UpdatedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                    this.dbServer.AddInParameter(command7, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command7, "ID", DbType.Int64, objDetailsVO.ID);
                    this.dbServer.AddOutParameter(command7, "ResultStatus", DbType.Int32, 0x7fffffff);
                    StringBuilder builder2 = new StringBuilder();
                    StringBuilder builder3 = new StringBuilder();
                    StringBuilder builder4 = new StringBuilder();
                    StringBuilder builder5 = new StringBuilder();
                    int num3 = 0;
                    while (true)
                    {
                        if (num3 < objDetailsVO.ChargeDetails.Count)
                        {
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
                            continue;
                        }
                        this.dbServer.AddInParameter(command7, "ChargeIdList", DbType.String, builder2.ToString());
                        this.dbServer.AddInParameter(command7, "StatusList", DbType.String, builder5.ToString());
                        this.dbServer.AddInParameter(command7, "SponsorTypeList", DbType.String, builder3.ToString());
                        this.dbServer.AddInParameter(command7, "BalanceAmountList", DbType.String, builder4.ToString());
                        this.dbServer.AddInParameter(command7, "PrescriptionDetailsID", DbType.String, BizActionObj.PrescriptionDetailsID);
                        this.dbServer.AddInParameter(command7, "InvestigationDetailsID", DbType.String, BizActionObj.InvestigationDetailsID);
                        this.dbServer.ExecuteNonQuery(command7, transaction);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command7, "ResultStatus");
                        if (BizActionObj.Details.IsFreezed && (BizActionObj.Details.PaymentDetails != null))
                        {
                            clsBasePaymentDAL instance = clsBasePaymentDAL.GetInstance();
                            clsAddPaymentBizActionVO valueObject = new clsAddPaymentBizActionVO {
                                Details = new clsPaymentVO()
                            };
                            valueObject.Details = BizActionObj.Details.PaymentDetails;
                            valueObject.myTransaction = true;
                            valueObject.Details.BillID = BizActionObj.Details.ID;
                            valueObject.Details.BillAmount = objDetailsVO.NetBillAmount;
                            valueObject.Details.Date = new DateTime?(BizActionObj.Details.Date);
                            valueObject.Details.LinkServer = BizActionObj.Details.LinkServer;
                            valueObject = (clsAddPaymentBizActionVO) instance.Add(valueObject, UserVo, pConnection, transaction);
                            if (valueObject.SuccessStatus == -1)
                            {
                                throw new Exception();
                            }
                            BizActionObj.Details.PaymentDetails.ID = valueObject.Details.ID;
                        }
                        if ((BizActionObj.Details.PathoWorkOrder != null) && ((BizActionObj.Details.IsFreezed && !BizActionObj.Details.IsIPDBill) || BizActionObj.Details.IsIPDBill))
                        {
                            BizActionObj.Details.PathoWorkOrder.Opd_Ipd_External_ID = BizActionObj.Details.Opd_Ipd_External_Id;
                            BizActionObj.Details.PathoWorkOrder.Opd_Ipd_External_UnitID = BizActionObj.Details.Opd_Ipd_External_UnitId;
                            BizActionObj.Details.PathoWorkOrder.SampleType = false;
                            BizActionObj.Details.PathoWorkOrder.Opd_Ipd_External = new long?((long) BizActionObj.Details.Opd_Ipd_External);
                            BizActionObj.Details.PathoWorkOrder.UnitId = BizActionObj.Details.UnitID;
                            BizActionObj.Details.PathoWorkOrder.AddedDateTime = BizActionObj.Details.AddedDateTime;
                            BizActionObj.Details.PathoWorkOrder.OrderDate = new DateTime?(BizActionObj.Details.Date);
                            BizActionObj.Details.PathoWorkOrder.BillNo = BizActionObj.Details.BillNo;
                            BizActionObj.Details.PathoWorkOrder.BillID = BizActionObj.Details.ID;
                            int num4 = 0;
                            while (true)
                            {
                                if (num4 >= objDetailsVO.ChargeDetails.Count)
                                {
                                    clsBaseOrderBookingDAL instance = clsBaseOrderBookingDAL.GetInstance();
                                    clsAddPathOrderBookingBizActionVO valueObject = new clsAddPathOrderBookingBizActionVO {
                                        PathOrderBooking = BizActionObj.Details.PathoWorkOrder,
                                        PathOrderBookingDetailList = BizActionObj.Details.PathoWorkOrder.Items
                                    };
                                    if (valueObject.PathOrderBookingDetailList.Count > 0)
                                    {
                                        valueObject = (clsAddPathOrderBookingBizActionVO) instance.AddPathOrderBooking(valueObject, UserVo, transaction, pConnection);
                                        if (valueObject.SuccessStatus == -1)
                                        {
                                            throw new Exception();
                                        }
                                        if (valueObject.PathOrderBooking.ID > 0L)
                                        {
                                            BizActionObj.Details.PathoWorkOrder.ID = valueObject.PathOrderBooking.ID;
                                        }
                                    }
                                    objArray = new object[] { "DELETE FROM T_PathOrderBookingDetails Where OrderID IN (Select ID FROM T_PathOrderBooking Where BillID = ", BizActionObj.Details.PathoWorkOrder.BillID, ") AND UnitID =", BizActionObj.Details.PathoWorkOrder.UnitId, " AND Status = 0 " };
                                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(objArray));
                                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                                    if ((BizActionObj.Details.DeletedChargeDetails != null) && ((BizActionObj.Details.DeletedChargeDetails.Count > 0) && BizActionObj.Details.IsIPDBill))
                                    {
                                        foreach (clsChargeVO evo5 in BizActionObj.Details.DeletedChargeDetails)
                                        {
                                            if ((evo5.ID != 0L) && ((evo5.TariffServiceId != 0L) && (BizActionObj.Details.PathoSpecilizationID == evo5.ServiceSpecilizationID)))
                                            {
                                                DbCommand command9 = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Delete from T_PathOrderBookingDetails where ChargeID = ", evo5.ID, " And TariffServiceID = ", evo5.TariffServiceId, " And UnitID = ", evo5.UnitID }));
                                                this.dbServer.ExecuteNonQuery(command9, transaction);
                                            }
                                        }
                                    }
                                    break;
                                }
                                if (BizActionObj.Details.PathoWorkOrder.PathoSpecilizationID == objDetailsVO.ChargeDetails[num4].ServiceSpecilizationID)
                                {
                                    BizActionObj.Details.PathoWorkOrder.DoctorID = BizActionObj.Details.PathoWorkOrder.DoctorID;
                                    if (objDetailsVO.ChargeDetails[num4].POBID > 0L)
                                    {
                                        BizActionObj.Details.PathoWorkOrder.ID = objDetailsVO.ChargeDetails[num4].POBID;
                                    }
                                    BizActionObj.Details.PathoWorkOrder.ID = BizActionObj.Details.PathoWorkOrder.ID;
                                    clsPathOrderBookingVO gvo = new clsPathOrderBookingVO();
                                    gvo = (BizActionObj.Details.PathoWorkOrder.ID <= 0L) ? this.GetPathologyTestDetails(objDetailsVO.ChargeDetails[num4].ServiceId, UserVo) : this.GetPathologyTestDetails(objDetailsVO.ChargeDetails[num4].ServiceId, UserVo, BizActionObj.Details.PathoWorkOrder.ID, objDetailsVO.ChargeDetails[num4].UnitID, objDetailsVO.ChargeDetails[num4].ID, pConnection, transaction);
                                    if ((gvo.Items != null) && (gvo.Items.Count > 0))
                                    {
                                        foreach (clsPathOrderBookingDetailVO lvo in gvo.Items)
                                        {
                                            lvo.ID = lvo.ID;
                                            objDetailsVO.ChargeDetails[num4].POBDID = lvo.ID;
                                            lvo.ServiceID = objDetailsVO.ChargeDetails[num4].ServiceId;
                                            lvo.ChargeID = objDetailsVO.ChargeDetails[num4].ID;
                                            lvo.TariffServiceID = objDetailsVO.ChargeDetails[num4].TariffServiceId;
                                            lvo.TestCharges = objDetailsVO.ChargeDetails[num4].Rate;
                                            BizActionObj.Details.PathoWorkOrder.Items.Add(lvo);
                                        }
                                    }
                                }
                                num4++;
                            }
                        }
                        if ((BizActionObj.Details.RadiologyWorkOrder == null) || ((BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails.Count <= 0) || BizActionObj.Details.IsFreezed))
                        {
                            goto TR_0010;
                        }
                        else
                        {
                            BizActionObj.Details.RadiologyWorkOrder.Opd_Ipd_External_ID = BizActionObj.Details.Opd_Ipd_External_Id;
                            BizActionObj.Details.RadiologyWorkOrder.Opd_Ipd_External_UnitID = BizActionObj.Details.Opd_Ipd_External_UnitId;
                            BizActionObj.Details.RadiologyWorkOrder.Opd_Ipd_External = new long?((long) BizActionObj.Details.Opd_Ipd_External);
                            BizActionObj.Details.RadiologyWorkOrder.UnitID = BizActionObj.Details.UnitID;
                            BizActionObj.Details.RadiologyWorkOrder.AddedDateTime = BizActionObj.Details.AddedDateTime;
                            BizActionObj.Details.RadiologyWorkOrder.Date = new DateTime?(BizActionObj.Details.Date);
                            BizActionObj.Details.RadiologyWorkOrder.BillNo = BizActionObj.Details.BillNo;
                            num5 = 0;
                        }
                        break;
                    }
                    break;
                }
                goto TR_002A;
            TR_0010:
                if (objDetailsVO.PharmacyItems.Items.Count > 0)
                {
                    objArray = new object[] { "Delete from T_Itemsaledetails where UnitID = ", objDetailsVO.UnitID, " And ItemSaleId in (select ID from T_ItemSale where UnitID = ", objDetailsVO.PharmacyItems.UnitId, " And BillId = ", objDetailsVO.ID, ")" };
                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(objArray));
                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                    DbCommand command12 = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Delete from T_ItemSale where UnitID = ", objDetailsVO.UnitID, " And BillId = ", objDetailsVO.ID }));
                    this.dbServer.ExecuteNonQuery(command12, transaction);
                    clsBaseItemSalesDAL instance = clsBaseItemSalesDAL.GetInstance();
                    clsAddItemSalesBizActionVO valueObject = new clsAddItemSalesBizActionVO {
                        Details = objDetailsVO.PharmacyItems
                    };
                    valueObject.Details.ItemSaleNo = objDetailsVO.PharmacyItems.ItemSaleNo;
                    valueObject.Details.BillID = BizActionObj.Details.ID;
                    valueObject.Details.IsBilled = BizActionObj.Details.IsFreezed;
                    valueObject = (clsAddItemSalesBizActionVO) instance.Add(valueObject, UserVo, pConnection, transaction);
                    if (valueObject.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    objDetailsVO.PharmacyItems.ID = valueObject.Details.ID;
                }
                DbCommand command13 = this.dbServer.GetStoredProcCommand("CIMS_AddDoctorPaymentDetails");
                this.dbServer.AddInParameter(command13, "BillID", DbType.Int64, BizActionObj.Details.ID);
                this.dbServer.AddInParameter(command13, "BillUnitID", DbType.Int64, BizActionObj.Details.UnitID);
                this.dbServer.AddInParameter(command13, "Opd_Ipd_External_Id", DbType.Int64, objDetailsVO.Opd_Ipd_External_Id);
                this.dbServer.AddInParameter(command13, "Opd_Ipd_External_UnitId", DbType.Int64, objDetailsVO.Opd_Ipd_External_UnitId);
                this.dbServer.AddInParameter(command13, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.ExecuteNonQuery(command13, transaction);
                transaction.Commit();
                BizActionObj.SuccessStatus = 0;
                return BizActionObj;
            TR_001D:
                num5++;
            TR_002A:
                while (true)
                {
                    if (num5 < BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails.Count)
                    {
                        for (int j = 0; j < objDetailsVO.ChargeDetails.Count; j++)
                        {
                            if (objDetailsVO.ChargeDetails[j].ROBID > 0L)
                            {
                                BizActionObj.Details.RadiologyWorkOrder.ID = objDetailsVO.ChargeDetails[j].ROBID;
                            }
                            BizActionObj.Details.RadiologyWorkOrder.ID = BizActionObj.Details.RadiologyWorkOrder.ID;
                            if (BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[num5].TariffServiceID == objDetailsVO.ChargeDetails[j].TariffServiceId)
                            {
                                BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[num5].ID = objDetailsVO.ChargeDetails[j].ROBDID;
                                BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[num5].ChargeID = objDetailsVO.ChargeDetails[j].ID;
                                clsRadOrderBookingDetailsVO svo = new clsRadOrderBookingDetailsVO();
                                svo = (BizActionObj.Details.RadiologyWorkOrder.ID <= 0L) ? this.GetRadilogyTestDetails(BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[num5].ServiceID, UserVo) : this.GetRadilogyTestDetails(BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[num5].ServiceID, UserVo);
                                if (svo != null)
                                {
                                    BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[num5].TestID = svo.TestID;
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        clsBaseRadiologyDAL instance = clsBaseRadiologyDAL.GetInstance();
                        clsAddRadOrderBookingBizActionVO valueObject = new clsAddRadOrderBookingBizActionVO {
                            BookingDetails = BizActionObj.Details.RadiologyWorkOrder
                        };
                        valueObject = (clsAddRadOrderBookingBizActionVO) instance.AddOrderBooking(valueObject, UserVo, pConnection, transaction);
                        if (valueObject.SuccessStatus == -1)
                        {
                            throw new Exception();
                        }
                        if (valueObject.BookingDetails.ID > 0L)
                        {
                            BizActionObj.Details.RadiologyWorkOrder.ID = valueObject.BookingDetails.ID;
                        }
                        if ((BizActionObj.DeletedRadSerDetailsList != null) && (BizActionObj.DeletedRadSerDetailsList.Count > 0))
                        {
                            foreach (clsChargeVO evo6 in BizActionObj.DeletedRadSerDetailsList)
                            {
                                if ((evo6.ID != 0L) && (evo6.TariffServiceId != 0L))
                                {
                                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Delete from T_RadiologyOrderBookingDetails where ChargeID = ", evo6.ID, " And TariffServiceId = ", evo6.TariffServiceId, " And UnitID = ", evo6.UnitID, " And IsResultEntry=0 " }));
                                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                                }
                            }
                        }
                        goto TR_0010;
                    }
                    break;
                }
                goto TR_001D;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.Details = null;
            }
            finally
            {
                pConnection.Close();
                transaction = null;
                pConnection = null;
            }
            return BizActionObj;
        }

        public void UpdatePathOrderBookingDetailStatus(long BillID, long UnitID, DbConnection pConnection, DbTransaction pTransaction)
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
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePathOrderBookingDetailStatus");
                this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.Int64, BillID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UnitID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
            }
            catch (Exception exception1)
            {
                string message = exception1.Message;
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
        }

        public override IValueObject UpdatePaymentDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                clsUpdatePaymentDetailsBizActionVO nvo = valueObject as clsUpdatePaymentDetailsBizActionVO;
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_MaintainPaymentLogBeforeEdit");
                this.dbServer.AddInParameter(storedProcCommand, "PaymentID", DbType.Int64, nvo.PaymentId);
                this.dbServer.AddInParameter(storedProcCommand, "PaymentUnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PaymentDetailId", DbType.Int64, nvo.PaymentDetailId);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_UpdatePaymentDetails");
                this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(command2, "BankID", DbType.String, nvo.BankID);
                this.dbServer.AddInParameter(command2, "Number", DbType.String, nvo.Number);
                this.dbServer.AddInParameter(command2, "PaidAmount", DbType.String, nvo.PaidAmount);
                this.dbServer.AddInParameter(command2, "Date", DbType.DateTime, nvo.Date.Value.Date.Date);
                this.dbServer.AddInParameter(command2, "PaymentModeId", DbType.Int64, nvo.PaymentModeID);
                this.dbServer.AddInParameter(command2, "PaymentDetailId", DbType.Int64, nvo.PaymentDetailId);
                this.dbServer.AddInParameter(command2, "PaymentID", DbType.Int64, nvo.PaymentId);
                this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(command2, transaction);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction.Dispose();
                connection.Close();
            }
            return valueObject;
        }

        private clsAddBillBizActionVO UpdatePharmacyDetails(clsAddBillBizActionVO BizActionObj, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {
            DbTransaction transaction = null;
            DbConnection pConnection = null;
            try
            {
                pConnection = (myConnection == null) ? this.dbServer.CreateConnection() : myConnection;
                if (pConnection.State == ConnectionState.Closed)
                {
                    pConnection.Open();
                }
                transaction = (myTransaction == null) ? pConnection.BeginTransaction() : myTransaction;
                clsBillVO details = BizActionObj.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateBill");
                this.dbServer.AddInParameter(storedProcCommand, "IsPackageBill", DbType.Int64, BizActionObj.IsPackageBill);
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, details.LinkServer);
                if (details.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, details.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, details.Date);
                this.dbServer.AddInParameter(storedProcCommand, "TotalBillAmount", DbType.Double, details.TotalBillAmount);
                this.dbServer.AddInParameter(storedProcCommand, "TotalConcessionAmount", DbType.Double, details.TotalConcessionAmount);
                this.dbServer.AddInParameter(storedProcCommand, "NetBillAmount", DbType.Double, details.NetBillAmount);
                this.dbServer.AddInParameter(storedProcCommand, "CalculatedNetBillAmount", DbType.Double, details.CalculatedNetBillAmount);
                this.dbServer.AddInParameter(storedProcCommand, "SelfAmount", DbType.Double, details.SelfAmount);
                if (details.BalanceAmountSelf < 0.0)
                {
                    details.BalanceAmountSelf = 0.0;
                }
                this.dbServer.AddInParameter(storedProcCommand, "BalanceAmountSelf", DbType.Double, details.BalanceAmountSelf);
                this.dbServer.AddInParameter(storedProcCommand, "NonSelfAmount", DbType.Double, details.NonSelfAmount);
                this.dbServer.AddInParameter(storedProcCommand, "BalanceAmountNonSelf", DbType.Double, details.BalanceAmountNonSelf);
                this.dbServer.AddInParameter(storedProcCommand, "IsPrinted", DbType.Boolean, details.IsPrinted);
                this.dbServer.AddInParameter(storedProcCommand, "SponserType", DbType.Boolean, details.SponserType);
                if (details.Remark != null)
                {
                    details.Remark = details.Remark.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, details.Remark);
                if (details.BillRemark != null)
                {
                    details.BillRemark = details.BillRemark.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "BillRemark", DbType.String, details.BillRemark);
                this.dbServer.AddInParameter(storedProcCommand, "BillType", DbType.Int16, (short) details.BillType);
                this.dbServer.AddInParameter(storedProcCommand, "BillPaymentType", DbType.Int16, (short) details.BillPaymentType);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, details.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "ConcessionReasonId", DbType.Int64, details.ConcessionReasonId);
                this.dbServer.AddInParameter(storedProcCommand, "CostingDivisionID", DbType.Int64, details.CostingDivisionID);
                this.dbServer.AddInParameter(storedProcCommand, "GrossDiscountReason", DbType.Int64, details.GrossDiscountReasonID);
                this.dbServer.AddInParameter(storedProcCommand, "GrossDiscountPercentage", DbType.Double, details.GrossDiscountPercentage);
                this.dbServer.AddInParameter(storedProcCommand, "ConcessionAuthorizedBy", DbType.Int64, details.ConcessionAuthorizedBy);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, details.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, details.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, details.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, details.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                StringBuilder builder = new StringBuilder();
                StringBuilder builder2 = new StringBuilder();
                StringBuilder builder3 = new StringBuilder();
                StringBuilder builder4 = new StringBuilder();
                this.dbServer.AddInParameter(storedProcCommand, "ChargeIdList", DbType.String, builder.ToString());
                this.dbServer.AddInParameter(storedProcCommand, "StatusList", DbType.String, builder4.ToString());
                this.dbServer.AddInParameter(storedProcCommand, "SponsorTypeList", DbType.String, builder2.ToString());
                this.dbServer.AddInParameter(storedProcCommand, "BalanceAmountList", DbType.String, builder3.ToString());
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if (BizActionObj.SuccessStatus == -2)
                {
                    throw new Exception();
                }
                if (BizActionObj.Details.IsFreezed && (BizActionObj.Details.PaymentDetails != null))
                {
                    clsBasePaymentDAL instance = clsBasePaymentDAL.GetInstance();
                    clsAddPaymentBizActionVO valueObject = new clsAddPaymentBizActionVO {
                        Details = new clsPaymentVO()
                    };
                    valueObject.Details = BizActionObj.Details.PaymentDetails;
                    valueObject.Details.BillID = BizActionObj.Details.ID;
                    valueObject.Details.BillAmount = details.NetBillAmount;
                    valueObject.Details.Date = new DateTime?(BizActionObj.Details.Date);
                    valueObject.Details.LinkServer = BizActionObj.Details.LinkServer;
                    valueObject.myTransaction = true;
                    valueObject = (clsAddPaymentBizActionVO) instance.Add(valueObject, UserVo, pConnection, transaction);
                    if (valueObject.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    BizActionObj.Details.PaymentDetails.ID = valueObject.Details.ID;
                }
                bool flag = false;
                if (details.PharmacyItems.Items.Count > 0)
                {
                    flag = true;
                    clsBaseItemSalesDAL instance = clsBaseItemSalesDAL.GetInstance();
                    clsAddItemSalesBizActionVO valueObject = new clsAddItemSalesBizActionVO {
                        Details = details.PharmacyItems
                    };
                    valueObject.Details.BillID = BizActionObj.Details.ID;
                    valueObject.Details.IsBilled = BizActionObj.Details.IsFreezed;
                    valueObject.Details.TotalAmount = details.TotalBillAmount;
                    valueObject.Details.NetAmount = details.NetBillAmount;
                    valueObject.Details.ConcessionAmount = details.TotalConcessionAmount;
                    valueObject.myTransaction = true;
                    valueObject = (clsAddItemSalesBizActionVO) instance.Add(valueObject, UserVo, pConnection, transaction);
                    if (valueObject.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    details.PharmacyItems.ID = valueObject.Details.ID;
                }
                if (pConnection == null)
                {
                    transaction.Commit();
                }
                if ((BizActionObj.LogInfoList != null) && (!flag && ((BizActionObj.LogInfoList.Count > 0) && this.IsAuditTrail)))
                {
                    this.SetLogInfo(BizActionObj.LogInfoList, UserVo.ID);
                    BizActionObj.LogInfoList.Clear();
                }
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.Details = null;
            }
            return BizActionObj;
        }
    }
}

