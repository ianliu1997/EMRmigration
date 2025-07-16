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

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsAdvanceDAL : clsBaseAdvanceDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsAdvanceDAL()
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

        public override IValueObject AddAdvance(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddAdvanceBizActionVO BizActionObj = valueObject as clsAddAdvanceBizActionVO;

            if (BizActionObj.Details.ID == 0)
                BizActionObj = AddDetails(BizActionObj, UserVo);
            else
                BizActionObj = UpdateDetails(BizActionObj);

            return valueObject;


        }

        private clsAddAdvanceBizActionVO AddDetails(clsAddAdvanceBizActionVO BizActionObj, clsUserVO UserVo)
        {

            DbConnection con = null;
            DbTransaction trans = null;

            try
            {

                //===================================
                con = dbServer.CreateConnection();

                if (con.State == ConnectionState.Closed) con.Open();

                trans = con.BeginTransaction();
                //===================================



                clsAdvanceVO objDetailsVO = BizActionObj.Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddAdvance");

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                if (objDetailsVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));

                objDetailsVO.Balance = objDetailsVO.Total - objDetailsVO.Used - objDetailsVO.Refund;
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objDetailsVO.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objDetailsVO.PatientUnitID);
                dbServer.AddInParameter(command, "CompanyID", DbType.Int64, objDetailsVO.CompanyID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);
                dbServer.AddInParameter(command, "Total", DbType.Double, objDetailsVO.Total);
                dbServer.AddInParameter(command, "AdvanceTypeId", DbType.Int64, objDetailsVO.AdvanceTypeId);
                dbServer.AddInParameter(command, "Used", DbType.Double, objDetailsVO.Used);
                dbServer.AddInParameter(command, "Refund", DbType.Double, objDetailsVO.Refund);
                dbServer.AddInParameter(command, "Balance", DbType.Double, objDetailsVO.Balance);
                dbServer.AddInParameter(command, "AdvanceAgainstId", DbType.Int64, objDetailsVO.AdvanceAgainstId);

                if (objDetailsVO.Remarks != null) objDetailsVO.Remarks = objDetailsVO.Remarks.Trim();
                dbServer.AddInParameter(command, "Remarks", DbType.String, objDetailsVO.Remarks);


                dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //Added By CDS For Cash Counter
                dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);
                ////dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, UserVo.UserLoginInfo.CashCounterID);

                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                //Added By Bhushanp For New Package Flow 18082017
                dbServer.AddInParameter(command, "PackageID", DbType.Int64, objDetailsVO.PackageID);   //UserVo.PackageID
                dbServer.AddInParameter(command, "PackageBillID", DbType.Int64, objDetailsVO.PackageBillID);
                dbServer.AddInParameter(command, "PackageBillUnitID", DbType.Int64, objDetailsVO.PackageBillUnitID);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddParameter(command, "AdvanceNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "00000000000000000000000000000000000000000000000000000000000000000000");

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Details.ID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.Details.AdvanceNO = Convert.ToString(dbServer.GetParameterValue(command, "AdvanceNo"));


                if (BizActionObj.Details.PaymentDetails != null)
                {
                    clsBasePaymentDAL objBaseDAL = clsBasePaymentDAL.GetInstance();
                    clsAddPaymentBizActionVO obj = new clsAddPaymentBizActionVO();

                    obj.Details = new clsPaymentVO();
                    obj.Details = BizActionObj.Details.PaymentDetails;

                    obj.Details.AdvanceID = BizActionObj.Details.ID;
                    obj.Details.AdvanceAmount = objDetailsVO.Total;
                    obj.Details.Date = BizActionObj.Details.Date;
                    obj.Details.LinkServer = BizActionObj.Details.LinkServer;

                    obj = (clsAddPaymentBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);
                    if (obj.SuccessStatus == -1) throw new Exception();
                    BizActionObj.Details.PaymentDetails.ID = obj.Details.ID;
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

        //private clsPaymentVO AddPayment(clsPaymentVO payDetails)
        //{
        //    try
        //    {
        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPayment");

        //        dbServer.AddInParameter(command, "LinkServer", DbType.String, payDetails.LinkServer);
        //        if (payDetails.LinkServer != null)
        //            dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, payDetails.LinkServer.Replace(@"\", "_"));

        //        dbServer.AddInParameter(command, "Date", DbType.DateTime, payDetails.Date);
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
        //        dbServer.AddInParameter(command, "PayeeNarration", DbType.String, payDetails.PayeeNarration.Trim());
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

        //            DbCommand command1   = dbServer.GetStoredProcCommand("CIMS_AddPaymentDetails");

        //            dbServer.AddInParameter(command1, "LinkServer", DbType.String, payDetails.LinkServer);
        //            if (payDetails.LinkServer != null)
        //                dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, payDetails.LinkServer.Replace(@"\", "_"));

        //            dbServer.AddInParameter(command1, "PaymentID", DbType.Int64, payDetails.ID);
        //            dbServer.AddInParameter(command1, "PaymentType", DbType.Int64, item.PaymentType);
        //            dbServer.AddInParameter(command1, "CashPayAmount", DbType.Double, item.CashPayAmount);
        //            if (item.ChequeNo != null) item.ChequeNo = item.ChequeNo.Trim();
        //            dbServer.AddInParameter(command1, "ChequeNo", DbType.String, item.ChequeNo);
        //            dbServer.AddInParameter(command1, "ChequeDate", DbType.DateTime, item.ChequeDate);
        //            dbServer.AddInParameter(command1, "ChequeBankID", DbType.Int64, item.ChequeBankID);
        //            dbServer.AddInParameter(command1, "ChequePayAmount", DbType.Double, item.ChequePayAmount);
        //            if (item.DDNo != null) item.DDNo = item.DDNo.Trim();
        //            dbServer.AddInParameter(command1, "DDNo", DbType.String, item.DDNo);
        //            dbServer.AddInParameter(command1, "DDDate", DbType.DateTime, item.DDDate);
        //            dbServer.AddInParameter(command1, "DDBankID", DbType.Int64, item.DDBankID);
        //            dbServer.AddInParameter(command1, "DDPayAmount", DbType.Double, item.DDPayAmount);
        //            if (item.CreditCardNo != null) item.CreditCardNo = item.CreditCardNo.Trim();
        //            dbServer.AddInParameter(command1, "CreditCardNo", DbType.String, item.CreditCardNo);
        //            dbServer.AddInParameter(command1, "CreditCardValidityDate", DbType.DateTime, item.CreditCardValidityDate);
        //            dbServer.AddInParameter(command1, "CreditCardBankID", DbType.Int64, item.CreditCardBankID);
        //            dbServer.AddInParameter(command1, "CreditCardPayAmount", DbType.Double, item.CreditCardPayAmount);
        //            if (item.DebitCardNo != null) item.DebitCardNo = item.DebitCardNo.Trim();
        //            dbServer.AddInParameter(command1, "DebitCardNo", DbType.String, item.DebitCardNo);
        //            dbServer.AddInParameter(command1, "DebitCardValidityDate", DbType.DateTime, item.DebitCardValidityDate);
        //            dbServer.AddInParameter(command1, "DebitCardBankID", DbType.Int64, item.DebitCardBankID);
        //            dbServer.AddInParameter(command1, "DebitCardPayAmount", DbType.Double, item.DebitCardPayAmount);
        //            dbServer.AddInParameter(command1, "ChequeCleared", DbType.Boolean, item.ChequeCleared);
        //            dbServer.AddInParameter(command1, "UnitId", DbType.Int64, payDetails.UnitID);
        //            dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, payDetails.CreatedUnitID);

        //            dbServer.AddInParameter(command1, "Status", DbType.Boolean, payDetails.Status);
        //            dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, payDetails.AddedBy);
        //            if (payDetails.AddedOn != null) payDetails.AddedOn = payDetails.AddedOn.Trim();
        //            dbServer.AddInParameter(command1, "AddedOn", DbType.String, payDetails.AddedOn);
        //            dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, payDetails.AddedDateTime);
        //            if (payDetails.AddedWindowsLoginName != null) payDetails.AddedWindowsLoginName = payDetails.AddedWindowsLoginName.Trim();
        //            dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, payDetails.AddedWindowsLoginName);

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

        private clsAddAdvanceBizActionVO UpdateDetails(clsAddAdvanceBizActionVO BizActionObj)
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

        public override IValueObject GetAdvance(IValueObject valueObject, clsUserVO UserVo)
        {

            //clsGetAdvanceBizActionVO BizActionObj = valueObject as clsGetVisitBizActionVO;
            //try
            //{
            //    clsVisitVO objDetailsVO = BizActionObj.Details;

            //    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetVisit");

            //    dbServer.AddInParameter(command, "PatientID", DbType.Int64, objDetailsVO.PatientId);
            //    dbServer.AddInParameter(command, "ID", DbType.Int64, objDetailsVO.ID);
            //    dbServer.AddInParameter(command, "GetLatestVisit", DbType.Boolean, BizActionObj.GetLatestVisit);

            //    DbDataReader reader;
            //    reader = (DbDataReader)dbServer.ExecuteReader(command);

            //    if (reader.HasRows)
            //    {
            //        while (reader.Read())
            //        {
            //            BizActionObj.Details.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
            //            BizActionObj.Details.PatientId = (long)DALHelper.HandleDBNull(reader["PatientId"]);
            //            BizActionObj.Details.DepartmentID = (long)DALHelper.HandleDBNull(reader["DepartmentID"]);
            //            BizActionObj.Details.VisitTypeID = (long)DALHelper.HandleDBNull(reader["VisitTypeID"]);
            //            BizActionObj.Details.CabinID = (long)DALHelper.HandleDBNull(reader["CabinID"]);
            //            BizActionObj.Details.DoctorID = (long)DALHelper.HandleDBNull(reader["DoctorID"]);
            //            BizActionObj.Details.ReferredDoctor = (string)DALHelper.HandleDBNull(reader["ReferredDoctor"]);
            //            BizActionObj.Details.PatientCaseRecord = (string)DALHelper.HandleDBNull(reader["PatientCaseRecord"]);
            //            BizActionObj.Details.CaseReferralSheet = (string)DALHelper.HandleDBNull(reader["CaseReferralSheet"]);
            //            BizActionObj.Details.Complaints = (string)DALHelper.HandleDBNull(reader["Complaints"]);
            //            BizActionObj.Details.VisitNotes = (string)DALHelper.HandleDBNull(reader["VisitNotes"]);
            //            BizActionObj.Details.Date = (DateTime)DALHelper.HandleDBNull(reader["Date"]);
            //            BizActionObj.Details.OPDNO = (string)DALHelper.HandleDBNull(reader["OPDNO"]);


            //            BizActionObj.Details.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
            //            BizActionObj.Details.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);

            //            BizActionObj.Details.CreatedUnitId = (Int64)DALHelper.HandleDBNull(reader["CreatedUnitId"]);
            //            BizActionObj.Details.AddedBy = (Int64?)DALHelper.HandleDBNull(reader["AddedBy"]);
            //            BizActionObj.Details.AddedOn = (String)DALHelper.HandleDBNull(reader["AddedOn"]);
            //            BizActionObj.Details.AddedDateTime = (DateTime?)DALHelper.HandleDate(reader["AddedDateTime"]);
            //            BizActionObj.Details.AddedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]);

            //            BizActionObj.Details.UpdatedUnitId = (Int64?)DALHelper.HandleDBNull(reader["UpdatedUnitID"]);
            //            BizActionObj.Details.UpdatedBy = (Int64?)DALHelper.HandleDBNull(reader["UpdatedBy"]);
            //            BizActionObj.Details.UpdatedOn = (String)DALHelper.HandleDBNull(reader["UpdatedOn"]);
            //            BizActionObj.Details.UpdatedDateTime = (DateTime?)DALHelper.HandleDate(reader["UpdatedDateTime"]);
            //            BizActionObj.Details.UpdatedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["UpdatedWindowsLoginName"]);



            //            BizActionObj.Details.Department = (string)DALHelper.HandleDBNull(reader["Department"]);
            //            BizActionObj.Details.VisitType = (string)DALHelper.HandleDBNull(reader["VisitType"]);
            //            BizActionObj.Details.Cabin = (string)DALHelper.HandleDBNull(reader["Cabin"]);
            //            BizActionObj.Details.Doctor = (string)DALHelper.HandleDBNull(reader["Doctor"]);

            //        }
            //    }
            //    int intStatus = dbServer.ExecuteNonQuery(command);

            //}
            //catch (Exception ex)
            //{

            //    // logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
            //    throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            //}
            //finally
            //{
            //    //log error  
            //    // logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            //}
            return valueObject;
        }

        public override IValueObject UpdateAdvance(IValueObject valueObject, clsUserVO UserVo)
        {

            return valueObject;
        }

        public override IValueObject GetAdvanceList(IValueObject valueObject, clsUserVO UserVo)
        {

            clsGetAdvanceListBizActionVO BizActionObj = valueObject as clsGetAdvanceListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAdvance");

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, BizActionObj.CostingDivisionID);

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "AllCompany", DbType.Boolean, BizActionObj.AllCompanies);
                dbServer.AddInParameter(command, "CompanyID", DbType.Int64, BizActionObj.CompanyID);

                DbDataReader reader;

                //int intStatus = dbServer.ExecuteNonQuery(command);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.Details == null)
                        BizActionObj.Details = new List<clsAdvanceVO>();
                    while (reader.Read())
                    {
                        clsAdvanceVO objDetails = new clsAdvanceVO();

                        objDetails.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objDetails.PatientID = (long)DALHelper.HandleDBNull(reader["PatientId"]);
                        objDetails.PatientUnitID = (long)DALHelper.HandleDBNull(reader["PatientUnitID"]);
                        objDetails.CompanyID = (long)DALHelper.HandleDBNull(reader["CompanyID"]);
                        objDetails.Date = (DateTime?)DALHelper.HandleDate(reader["Date"]);
                        objDetails.Total = (double)DALHelper.HandleDBNull(reader["Total"]);
                        objDetails.AdvanceTypeId = (long)DALHelper.HandleDBNull(reader["AdvanceTypeId"]);
                        objDetails.AdvanceAgainstId = (long)DALHelper.HandleDBNull(reader["AdvanceAgainstId"]);
                        objDetails.Used = (double)DALHelper.HandleDBNull(reader["Used"]);
                        objDetails.Refund = (double)DALHelper.HandleDBNull(reader["Refund"]);
                        objDetails.Balance = (double)DALHelper.HandleDBNull(reader["Balance"]);
                        objDetails.Remarks = (String)DALHelper.HandleDBNull(reader["Remarks"]);

                        objDetails.UnitID = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                        // Added By CDS
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

                        objDetails.Company = (string)DALHelper.HandleDBNull(reader["Company"]);
                        objDetails.AdvanceAgainst = (string)DALHelper.HandleDBNull(reader["AdvanceAgainst"]);
                        objDetails.UnitName = (string)DALHelper.HandleDBNull(reader["UnitName"]);
                        objDetails.AdvanceNO = Convert.ToString(DALHelper.HandleDBNull(reader["AdvanceNO"]));

                        //added By Bhushanp 01062017
                        objDetails.ApprovalRequestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalRequestID"]));
                        objDetails.ApprovalRequestUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalRequestUnitID"]));
                        objDetails.ApprovalRequestDetailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalRequestDetailsID"]));
                        objDetails.ApprovalRequestDetailsUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalRequestDetailsUnitID"]));
                        objDetails.IsSendForApproval = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSendForApproval"]));
                        objDetails.LevelID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LevelID"]));
                        objDetails.SelectCharge = Convert.ToBoolean(DALHelper.HandleDBNull(reader["SelectCharge"]));
                        objDetails.ApprovalRemark = Convert.ToString(DALHelper.HandleDBNull(reader["ApprovalRemark"]));
                        objDetails.ApprovalStatus = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ApprovalStatus"]));
                        objDetails.ApprovalRequestRemark = Convert.ToString(DALHelper.HandleDBNull(reader["ApprovalRequestRemark"]));
                        objDetails.Refund = Convert.ToDouble(DALHelper.HandleDBNull(reader["Amount"]));
                        objDetails.IsRefund = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRefund"]));

                        objDetails.PackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageID"]));
                        objDetails.PackageName = Convert.ToString(DALHelper.HandleDBNull(reader["PackageName"]));
                        objDetails.IsPackageBillFreeze = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackageBillFreeze"]));
                        objDetails.PackageBillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageBillID"]));
                        objDetails.PackageBillUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageBillUnitID"]));

                        objDetails.PackageBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PackageBillAmount"]));     // Added on 13062018 for Package New Changes
                        objDetails.PackageAdvanceBalance = Convert.ToDouble(DALHelper.HandleDBNull(reader["PackageAdvanceBalance"]));     // Added on 13062018 for Package New Changes

                        BizActionObj.Details.Add(objDetails);

                    }

                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizActionObj.PatientDailyCashAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PatientDailyCashAmount"]));
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

        public override IValueObject GetAdvanceListForRequestApproval(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetAdvanceListBizActionVO BizActionObj = valueObject as clsGetAdvanceListBizActionVO;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetAdvanceListForRequestApproval");
                DbDataReader reader;
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "UnitID", DbType.String, BizActionObj.UnitID);
                if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                    dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.FirstName);

                if (BizActionObj.MiddleName != null && BizActionObj.MiddleName.Length != 0)
                    dbServer.AddInParameter(command, "MiddleName", DbType.String, BizActionObj.MiddleName);

                if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.LastName);

                if (BizActionObj.FromDate != null)
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate.Value.Date.Date);
                if (BizActionObj.ToDate != null)
                {
                    if (BizActionObj.FromDate != null)
                    {
                        BizActionObj.ToDate = BizActionObj.ToDate.Value.Date.AddDays(1);
                    }
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate.Value.Date.Date);

                }
                dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, BizActionObj.CostingDivisionID);  //Costing Divisions for Clinical & Pharmacy Billing

                dbServer.AddInParameter(command, "UserLevelID", DbType.Int64, BizActionObj.UserLevelID);

                dbServer.AddInParameter(command, "UserRightsTypeID", DbType.Int64, BizActionObj.UserRightsTypeID);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.Details == null)
                        BizActionObj.Details = new List<clsAdvanceVO>();
                    while (reader.Read())
                    {
                        clsAdvanceVO objVO = new clsAdvanceVO();
                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objVO.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        objVO.MRNO = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"]));
                        objVO.ContactNo = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo1"]));
                        objVO.Total = Convert.ToDouble(DALHelper.HandleDBNull(reader["Total"]));
                        objVO.Used = Convert.ToDouble(DALHelper.HandleDBNull(reader["Used"]));
                        objVO.Refund = Convert.ToDouble(DALHelper.HandleDBNull(reader["Refund"]));
                        objVO.Balance = Convert.ToDouble(DALHelper.HandleDBNull(reader["Balance"]));
                        objVO.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        objVO.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        objVO.CostingDivisionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CostingDivisionID"]));
                        objVO.RequestType = Convert.ToString(DALHelper.HandleDBNull(reader["RequestType"]));
                        objVO.RequestTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RequestTypeID"]));
                        objVO.ApprovalRequestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalRequestID"]));
                        objVO.ApprovalRequestUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalRequestUnitID"]));
                        objVO.LevelID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LevelID"]));
                        objVO.ApprovalRemark = Convert.ToString(DALHelper.HandleDBNull(reader["ApprovalRemark"]));
                        objVO.FirstName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])));
                        objVO.MiddleName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"])));
                        objVO.LastName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])));
                        BizActionObj.Details.Add(objVO);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");

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

        public override IValueObject DeleteAdvance(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteAdvanceBizActionVO BizActionObj = valueObject as clsDeleteAdvanceBizActionVO;
            try
            {
                DbCommand command;

                command = dbServer.GetStoredProcCommand("CIMS_DeleteAdvance");
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
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

        public override IValueObject GetPatientAdvanceList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientAdvanceListBizActionVO BizActionObj = valueObject as clsGetPatientAdvanceListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientAdvance");

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "CompanyID", DbType.Int64, BizActionObj.CompanyID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, BizActionObj.CostingDivisionID);
                dbServer.AddInParameter(command, "IsFromCompany", DbType.Boolean, BizActionObj.IsFromCompany);

                if (BizActionObj.AdvanceDetails != null)
                {
                    dbServer.AddInParameter(command, "PackageID", DbType.Int64, BizActionObj.AdvanceDetails.PackageID);
                    dbServer.AddInParameter(command, "PackageBillID", DbType.Int64, BizActionObj.AdvanceDetails.PackageBillID);
                    dbServer.AddInParameter(command, "PackageBillUnitID", DbType.Int64, BizActionObj.AdvanceDetails.PackageBillUnitID);
                    dbServer.AddInParameter(command, "IsForTotalAdvance", DbType.Boolean, BizActionObj.AdvanceDetails.IsForTotalAdvance);
                }

                dbServer.AddInParameter(command, "IsShowBothAdvance", DbType.Boolean, BizActionObj.IsShowBothAdvance);  // Set to retreive Patient & Package advance both : Added on 16062018

                DbDataReader reader;

                //int intStatus = dbServer.ExecuteNonQuery(command);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.Details == null)
                        BizActionObj.Details = new List<clsAdvanceVO>();
                    while (reader.Read())
                    {
                        clsAdvanceVO objDetails = new clsAdvanceVO();

                        objDetails.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objDetails.PatientID = (long)DALHelper.HandleDBNull(reader["PatientId"]);
                        objDetails.PatientUnitID = (long)DALHelper.HandleDBNull(reader["PatientUnitId"]);
                        objDetails.CompanyID = (long)DALHelper.HandleDBNull(reader["CompanyID"]);
                        objDetails.Date = (DateTime?)DALHelper.HandleDate(reader["Date"]);
                        objDetails.Total = (double)DALHelper.HandleDBNull(reader["Total"]);
                        objDetails.AdvanceTypeId = (long)DALHelper.HandleDBNull(reader["AdvanceTypeId"]);
                        objDetails.AdvanceAgainstId = (long)DALHelper.HandleDBNull(reader["AdvanceAgainstId"]);
                        objDetails.Used = (double)DALHelper.HandleDBNull(reader["Used"]);
                        objDetails.Refund = (double)DALHelper.HandleDBNull(reader["Refund"]);
                        objDetails.Balance = (double)DALHelper.HandleDBNull(reader["Balance"]);
                        objDetails.Remarks = (String)DALHelper.HandleDBNull(reader["Remarks"]);

                        objDetails.UnitID = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                        objDetails.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                        objDetails.AddedBy = (Int64)DALHelper.HandleDBNull(reader["AddedBy"]);
                        objDetails.AddedOn = (String)DALHelper.HandleDBNull(reader["AddedOn"]);
                        objDetails.AddedDateTime = (DateTime?)DALHelper.HandleDate(reader["AddedDateTime"]);
                        objDetails.AddedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]);
                        objDetails.UpdatedBy = (Int64?)DALHelper.HandleDBNull(reader["UpdatedBy"]);
                        objDetails.UpdatedOn = (String)DALHelper.HandleDBNull(reader["UpdatedOn"]);
                        objDetails.UpdatedDateTime = (DateTime?)DALHelper.HandleDate(reader["UpdatedDateTime"]);
                        objDetails.UpdateWindowsLoginName = (String)DALHelper.HandleDBNull(reader["UpdateWindowsLoginName"]);

                        objDetails.Company = (string)DALHelper.HandleDBNull(reader["Company"]);
                        objDetails.AdvanceAgainst = (string)DALHelper.HandleDBNull(reader["AdvanceAgainst"]);

                        objDetails.PackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageID"]));                    // For Package New Changes Added on 16062018
                        objDetails.PackageBillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageBillID"]));            // For Package New Changes Added on 16062018
                        objDetails.PackageBillUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageBillUnitID"]));    // For Package New Changes Added on 16062018

                        BizActionObj.Details.Add(objDetails);

                    }

                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizActionObj.PatientDailyCashAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PatientDailyCashAmount"]));
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

        #region // 20042017 Refund To Advance

        public override IValueObject AddAdvance(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            clsAddAdvanceBizActionVO BizActionObj = valueObject as clsAddAdvanceBizActionVO;

            if (BizActionObj.Details.ID == 0)
                BizActionObj = AddDetailsWithTransaction(BizActionObj, UserVo, pConnection, pTransaction);
            //else
            //    BizActionObj = UpdateDetails(BizActionObj);

            return valueObject;
        }

        private clsAddAdvanceBizActionVO AddDetailsWithTransaction(clsAddAdvanceBizActionVO BizActionObj, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
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



                clsAdvanceVO objDetailsVO = BizActionObj.Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddAdvance");      // DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddAdvanceFromRefund");

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                if (objDetailsVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));

                objDetailsVO.Balance = objDetailsVO.Total - objDetailsVO.Used - objDetailsVO.Refund;
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objDetailsVO.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objDetailsVO.PatientUnitID);
                dbServer.AddInParameter(command, "CompanyID", DbType.Int64, objDetailsVO.CompanyID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);
                dbServer.AddInParameter(command, "Total", DbType.Double, objDetailsVO.Total);
                dbServer.AddInParameter(command, "AdvanceTypeId", DbType.Int64, objDetailsVO.AdvanceTypeId);
                dbServer.AddInParameter(command, "Used", DbType.Double, objDetailsVO.Used);
                dbServer.AddInParameter(command, "Refund", DbType.Double, objDetailsVO.Refund);
                dbServer.AddInParameter(command, "Balance", DbType.Double, objDetailsVO.Balance);
                dbServer.AddInParameter(command, "AdvanceAgainstId", DbType.Int64, objDetailsVO.AdvanceAgainstId);

                if (objDetailsVO.Remarks != null) objDetailsVO.Remarks = objDetailsVO.Remarks.Trim();
                dbServer.AddInParameter(command, "Remarks", DbType.String, objDetailsVO.Remarks);


                dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //Added By CDS For Cash Counter
                dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);
                ////dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, UserVo.UserLoginInfo.CashCounterID);

                dbServer.AddInParameter(command, "FromRefundID", DbType.Int64, objDetailsVO.FromRefundID);          // 21042017 Refund To Advance
                dbServer.AddInParameter(command, "PackageBillID", DbType.Int64, objDetailsVO.PackageBillID);
                dbServer.AddInParameter(command, "PackageBillUnitID", DbType.Int64, objDetailsVO.PackageBillUnitID);

                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddParameter(command, "AdvanceNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "00000000000000000000000000000000000000000000000000000000000000000000");

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Details.ID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.Details.AdvanceNO = Convert.ToString(dbServer.GetParameterValue(command, "AdvanceNo"));


                if (BizActionObj.Details.PaymentDetails != null)
                {



                    clsBasePaymentDAL objBaseDAL = clsBasePaymentDAL.GetInstance();
                    clsAddPaymentBizActionVO obj = new clsAddPaymentBizActionVO();

                    obj.Details = new clsPaymentVO();
                    obj.Details = BizActionObj.Details.PaymentDetails;

                    obj.Details.AdvanceID = BizActionObj.Details.ID;
                    obj.Details.AdvanceAmount = objDetailsVO.Total;
                    obj.Details.Date = BizActionObj.Details.Date;
                    obj.Details.LinkServer = BizActionObj.Details.LinkServer;

                    obj = (clsAddPaymentBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);
                    if (obj.SuccessStatus == -1) throw new Exception();
                    BizActionObj.Details.PaymentDetails.ID = obj.Details.ID;

                }

                if (pConnection == null) trans.Commit();
                BizActionObj.SuccessStatus = 0;


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

        #endregion

        #region For Package Advance & Bill Save in transaction : added on 16082018

        public override IValueObject AddAdvanceWithPackageBill(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            clsAddAdvanceBizActionVO BizActionObj = valueObject as clsAddAdvanceBizActionVO;

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



                clsAdvanceVO objDetailsVO = BizActionObj.Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddAdvance");

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                if (objDetailsVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));

                objDetailsVO.Balance = objDetailsVO.Total - objDetailsVO.Used - objDetailsVO.Refund;
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objDetailsVO.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objDetailsVO.PatientUnitID);
                dbServer.AddInParameter(command, "CompanyID", DbType.Int64, objDetailsVO.CompanyID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);
                dbServer.AddInParameter(command, "Total", DbType.Double, objDetailsVO.Total);
                dbServer.AddInParameter(command, "AdvanceTypeId", DbType.Int64, objDetailsVO.AdvanceTypeId);
                dbServer.AddInParameter(command, "Used", DbType.Double, objDetailsVO.Used);
                dbServer.AddInParameter(command, "Refund", DbType.Double, objDetailsVO.Refund);
                dbServer.AddInParameter(command, "Balance", DbType.Double, objDetailsVO.Balance);
                dbServer.AddInParameter(command, "AdvanceAgainstId", DbType.Int64, objDetailsVO.AdvanceAgainstId);

                if (objDetailsVO.Remarks != null) objDetailsVO.Remarks = objDetailsVO.Remarks.Trim();
                dbServer.AddInParameter(command, "Remarks", DbType.String, objDetailsVO.Remarks);


                dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //Added By CDS For Cash Counter
                dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);
                ////dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, UserVo.UserLoginInfo.CashCounterID);

                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                //Added By Bhushanp For New Package Flow 18082017
                dbServer.AddInParameter(command, "PackageID", DbType.Int64, objDetailsVO.PackageID);   //UserVo.PackageID
                dbServer.AddInParameter(command, "PackageBillID", DbType.Int64, objDetailsVO.PackageBillID);
                dbServer.AddInParameter(command, "PackageBillUnitID", DbType.Int64, objDetailsVO.PackageBillUnitID);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddParameter(command, "AdvanceNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "00000000000000000000000000000000000000000000000000000000000000000000");

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Details.ID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.Details.AdvanceNO = Convert.ToString(dbServer.GetParameterValue(command, "AdvanceNo"));


                if (BizActionObj.Details.PaymentDetails != null)
                {
                    clsBasePaymentDAL objBaseDAL = clsBasePaymentDAL.GetInstance();
                    clsAddPaymentBizActionVO obj = new clsAddPaymentBizActionVO();

                    obj.Details = new clsPaymentVO();
                    obj.Details = BizActionObj.Details.PaymentDetails;

                    obj.Details.AdvanceID = BizActionObj.Details.ID;
                    obj.Details.AdvanceAmount = objDetailsVO.Total;
                    obj.Details.Date = BizActionObj.Details.Date;
                    obj.Details.LinkServer = BizActionObj.Details.LinkServer;

                    obj = (clsAddPaymentBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);
                    if (obj.SuccessStatus == -1) throw new Exception();
                    BizActionObj.Details.PaymentDetails.ID = obj.Details.ID;
                }

                if (pConnection == null) trans.Commit();
                BizActionObj.SuccessStatus = 0;


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

        #endregion
    }
}
