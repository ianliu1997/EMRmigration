using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing;
using PalashDynamics.ValueObjects;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects.Billing;
using System.Data.Common;
using System.Data;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.Radiology;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects.Administration.DoctorShareRange;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.Master.DoctorPayment;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using System.Reflection;
using PalashDynamics.ValueObjects.Log;
using System.Xml.Serialization;
using System.IO;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsBillDAL : clsBaseBillDAL
    {

        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        bool IsAuditTrail = false;
        #endregion

        private clsBillDAL()
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

        public override IValueObject Add(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddBillBizActionVO BizActionObj = valueObject as clsAddBillBizActionVO;

            if (BizActionObj.Details.IsFreezed == true)
            {
                if (BizActionObj.Details.ChargeDetails != null && BizActionObj.Details.ChargeDetails.Count > 0
                    && BizActionObj.Details.PharmacyItems != null && BizActionObj.Details.PharmacyItems.Items != null && BizActionObj.Details.PharmacyItems.Items.Count > 0)
                    BizActionObj.Details.BillType = BillTypes.Clinical_Pharmacy;
                else if (BizActionObj.Details.ChargeDetails != null && BizActionObj.Details.ChargeDetails.Count > 0)
                    BizActionObj.Details.BillType = BillTypes.Clinical;
                else if (BizActionObj.Details.PharmacyItems != null && BizActionObj.Details.PharmacyItems.Items != null && BizActionObj.Details.PharmacyItems.Items.Count > 0)
                    BizActionObj.Details.BillType = BillTypes.Pharmacy;
                // BizActionObj.Details.BillType=BillTypes.
            }

            if (BizActionObj.Details.ID == 0)
                BizActionObj = AddDetails(BizActionObj, UserVo, null, null);
            else
                BizActionObj = UpdateDetails(BizActionObj, UserVo);

            return valueObject;
        }

        //By Anjali.................
        public override IValueObject Add(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {
            clsAddBillBizActionVO BizActionObj = valueObject as clsAddBillBizActionVO;

            if (BizActionObj.Details.IsFreezed == true)
            {
                if (BizActionObj.Details.ChargeDetails != null && BizActionObj.Details.ChargeDetails.Count > 0
                    && BizActionObj.Details.PharmacyItems != null && BizActionObj.Details.PharmacyItems.Items != null && BizActionObj.Details.PharmacyItems.Items.Count > 0)
                    BizActionObj.Details.BillType = BillTypes.Clinical_Pharmacy;
                else if (BizActionObj.Details.ChargeDetails != null && BizActionObj.Details.ChargeDetails.Count > 0)
                    BizActionObj.Details.BillType = BillTypes.Clinical;
                else if (BizActionObj.Details.PharmacyItems != null && BizActionObj.Details.PharmacyItems.Items != null && BizActionObj.Details.PharmacyItems.Items.Count > 0)
                    BizActionObj.Details.BillType = BillTypes.Pharmacy;
            }

            if (BizActionObj.Details.ID == 0)
                BizActionObj = AddDetails(BizActionObj, UserVo, myConnection, myTransaction);
            else
                BizActionObj = UpdatePharmacyDetails(BizActionObj, UserVo, myConnection, myTransaction);

            return valueObject;
        }
        //Added by AJ Date 4/2/2017............................
        private clsAddBillBizActionVO UpdatePharmacyDetails(clsAddBillBizActionVO BizActionObj, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                if (myConnection != null)
                    con = myConnection;
                else
                    con = dbServer.CreateConnection();


                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                if (myTransaction != null)
                    trans = myTransaction;
                else
                    trans = con.BeginTransaction();

                clsBillVO objDetailsVO = BizActionObj.Details;


                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateBill");

                dbServer.AddInParameter(command, "IsPackageBill", DbType.Int64, BizActionObj.IsPackageBill);

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                if (objDetailsVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);

                dbServer.AddInParameter(command, "TotalBillAmount", DbType.Double, objDetailsVO.TotalBillAmount);
                dbServer.AddInParameter(command, "TotalConcessionAmount", DbType.Double, objDetailsVO.TotalConcessionAmount);
                dbServer.AddInParameter(command, "NetBillAmount", DbType.Double, objDetailsVO.NetBillAmount);
                dbServer.AddInParameter(command, "CalculatedNetBillAmount", DbType.Double, objDetailsVO.CalculatedNetBillAmount);
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
                dbServer.AddInParameter(command, "ConcessionReasonId", DbType.Int64, objDetailsVO.ConcessionReasonId);
                dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);
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


                dbServer.AddInParameter(command, "ChargeIdList", DbType.String, ChargeIdListList.ToString());
                dbServer.AddInParameter(command, "StatusList", DbType.String, StatusList.ToString());
                dbServer.AddInParameter(command, "SponsorTypeList", DbType.String, SponsorTypeList.ToString());
                dbServer.AddInParameter(command, "BalanceAmountList", DbType.String, BalanceAmountList.ToString());

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Details.ID = (long)dbServer.GetParameterValue(command, "ID");
                //BizActionObj.Details.BillNo = (string)dbServer.GetParameterValue(command, "BillNo");

                if (BizActionObj.SuccessStatus == -2) throw new Exception();

                if (BizActionObj.Details.IsFreezed == true && BizActionObj.Details.PaymentDetails != null)
                {
                    clsBasePaymentDAL objBaseDAL = clsBasePaymentDAL.GetInstance();
                    clsAddPaymentBizActionVO obj = new clsAddPaymentBizActionVO();

                    obj.Details = new clsPaymentVO();
                    obj.Details = BizActionObj.Details.PaymentDetails;

                    obj.Details.BillID = BizActionObj.Details.ID;
                    obj.Details.BillAmount = objDetailsVO.NetBillAmount;
                    obj.Details.Date = BizActionObj.Details.Date;
                    obj.Details.LinkServer = BizActionObj.Details.LinkServer;
                    obj.myTransaction = true;
                    obj = (clsAddPaymentBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);
                    if (obj.SuccessStatus == -1) throw new Exception();
                    BizActionObj.Details.PaymentDetails.ID = obj.Details.ID;
                }

                bool IsFromCountersale = false;
                if (objDetailsVO.PharmacyItems.Items.Count > 0)
                {
                    IsFromCountersale = true;
                    clsBaseItemSalesDAL objBaseDAL = clsBaseItemSalesDAL.GetInstance();
                    clsAddItemSalesBizActionVO obj = new clsAddItemSalesBizActionVO();
                    obj.Details = objDetailsVO.PharmacyItems;
                    obj.Details.BillID = BizActionObj.Details.ID;
                    obj.Details.IsBilled = BizActionObj.Details.IsFreezed;
                    obj.Details.TotalAmount = objDetailsVO.TotalBillAmount;
                    obj.Details.NetAmount = objDetailsVO.NetBillAmount;
                    obj.Details.ConcessionAmount = objDetailsVO.TotalConcessionAmount;
                    obj.myTransaction = true;
                    obj = (clsAddItemSalesBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);
                    if (obj.SuccessStatus == -1) throw new Exception();
                    objDetailsVO.PharmacyItems.ID = obj.Details.ID;
                }
                if (con == null) trans.Commit();
                if (BizActionObj.LogInfoList != null && IsFromCountersale == false)
                {
                    if (BizActionObj.LogInfoList.Count > 0 && IsAuditTrail == true)
                    {
                        SetLogInfo(BizActionObj.LogInfoList, UserVo.ID);
                        BizActionObj.LogInfoList.Clear();
                    }
                }

                //trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.Details = null;
            }
            finally
            {
                // con.Close();
                // trans = null;
                //  con = null;
            }

            return BizActionObj;
        }
        //***//--------

        private clsAddBillBizActionVO UpdateDetails(clsAddBillBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction trans = null;  //Change by Bhushanp 056/01/2017 For Delete Pathology Service Remove commented code 
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                clsBillVO objDetailsVO = BizActionObj.Details;

                DbCommand command8 = dbServer.GetStoredProcCommand("CIMS_UpdateChargeDetailsNew");
                dbServer.AddInParameter(command8, "BillID", DbType.Int64, objDetailsVO.ID);
                dbServer.AddInParameter(command8, "UnitID", DbType.Int64, objDetailsVO.UnitID);
                dbServer.AddInParameter(command8, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);
                int intStatus8 = dbServer.ExecuteNonQuery(command8, trans);

                long ParentID = 0;//T_charge.id
                long CDParentID = 0;//T_chargeDetails.id

                for (int i = 0; i < objDetailsVO.ChargeDetails.Count; i++)
                {
                    if (objDetailsVO.ChargeDetails[i].ChildPackageService == false)
                    {
                        clsBaseChargeDAL objBaseDAL = clsBaseChargeDAL.GetInstance();
                        clsAddChargeBizActionVO obj = new clsAddChargeBizActionVO();

                        obj.Details = objDetailsVO.ChargeDetails[i];
                        obj.Details.IsBilled = BizActionObj.Details.IsFreezed;
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

                        if (objDetailsVO.ChargeDetails[0].isPackageService) //For Parent ID for package Servce Billing 18 04 17 
                            ParentID = objDetailsVO.ChargeDetails[0].ID; //For Parent ID for package Servce Billing 18 04 17 

                        obj = (clsAddChargeBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans, ParentID, 0);  //obj = (clsAddChargeBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);
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
                            objCharge.Details = item;
                            if (objCharge.Details.Status == true)
                                objCharge.Details.PaidAmount = item.NetAmount;
                            else
                                objCharge.Details.PaidAmount = 0;
                            objCharge.Details.IsBilled = BizActionObj.Details.IsFreezed;
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

                //***//
                if (BizActionObj.Details.DeleteChargeDetails.Count > 0)
                {
                    foreach (var item in BizActionObj.Details.DeleteChargeDetails)
                    {
                        if (item.PrescriptionDetailsID > 0)
                        {
                            DbCommand SqlcommandPr = dbServer.GetSqlStringCommand("UPDATE T_PatientEMRDiagnosisDetails SET BillDone=0 ,BillID=0 where ID= " + item.PrescriptionDetailsID);
                            int sqlStatusPr = dbServer.ExecuteNonQuery(SqlcommandPr, trans);
                        }
                        else if (item.InvestigationDetailsID > 0)
                        {
                            DbCommand SqlcommandPr = dbServer.GetSqlStringCommand("UPDATE T_DoctorSuggestedServiceDetail SET BillDone=0 ,BillID=0 where ID= " + item.InvestigationDetailsID);
                            int sqlStatusPr = dbServer.ExecuteNonQuery(SqlcommandPr, trans);
                        }
                    }
                }

                if (BizActionObj.DeletedRadSerDetailsList.Count > 0)
                {
                    foreach (clsChargeVO item in BizActionObj.DeletedRadSerDetailsList)
                    {
                        if (item.PrescriptionDetailsID > 0)
                        {
                            DbCommand SqlcommandPr = dbServer.GetSqlStringCommand("UPDATE T_PatientEMRDiagnosisDetails SET BillDone=0 ,BillID=0 where ID= " + item.PrescriptionDetailsID);
                            int sqlStatusPr = dbServer.ExecuteNonQuery(SqlcommandPr, trans);
                        }
                        else if (item.InvestigationDetailsID > 0)
                        {
                            DbCommand SqlcommandPr = dbServer.GetSqlStringCommand("UPDATE T_DoctorSuggestedServiceDetail SET BillDone=0 ,BillID=0 where ID= " + item.InvestigationDetailsID);
                            int sqlStatusPr = dbServer.ExecuteNonQuery(SqlcommandPr, trans);
                        }
                    }
                }
                //

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateBill");
                dbServer.AddInParameter(command, "IsPackageBill", DbType.Int64, BizActionObj.IsPackageBill);

                if (BizActionObj.IsPackageBill == true)
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
                dbServer.AddInParameter(command, "CalculatedNetBillAmount", DbType.Double, objDetailsVO.CalculatedNetBillAmount);
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
                dbServer.AddInParameter(command, "ConcessionReasonId", DbType.Int64, objDetailsVO.ConcessionReasonId);// By Yogesh K 20 Apr 2016
                dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);  //Costing Divisions for Clinical & Pharmacy Billing
                dbServer.AddInParameter(command, "GrossDiscountReason", DbType.Int64, objDetailsVO.GrossDiscountReasonID);
                dbServer.AddInParameter(command, "GrossDiscountPercentage", DbType.Double, objDetailsVO.GrossDiscountPercentage);
                dbServer.AddInParameter(command, "ConcessionAuthorizedBy", DbType.String, objDetailsVO.ConcessionAuthorizedBy);
                dbServer.AddInParameter(command, "ConcessionRemark", DbType.String, objDetailsVO.ConcessionRemark);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDetailsVO.UnitID);
                //***//
                dbServer.AddInParameter(command, "AgainstDonor", DbType.Boolean, objDetailsVO.AgainstDonor);
                dbServer.AddInParameter(command, "LinkPatientID", DbType.Int64, objDetailsVO.LinkPatientID);
                dbServer.AddInParameter(command, "LinkPatientUnitID", DbType.Int64, objDetailsVO.LinkPatientUnitID);
                //------
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

                //***//
                dbServer.AddInParameter(command, "PrescriptionDetailsID", DbType.String, BizActionObj.PrescriptionDetailsID);
                dbServer.AddInParameter(command, "InvestigationDetailsID", DbType.String, BizActionObj.InvestigationDetailsID);
                //

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                if (BizActionObj.Details.IsFreezed == true && BizActionObj.Details.PaymentDetails != null)
                {
                    clsBasePaymentDAL objBaseDAL = clsBasePaymentDAL.GetInstance();
                    clsAddPaymentBizActionVO obj = new clsAddPaymentBizActionVO();
                    obj.Details = new clsPaymentVO();
                    obj.Details = BizActionObj.Details.PaymentDetails;
                    obj.myTransaction = true;
                    obj.Details.BillID = BizActionObj.Details.ID;
                    obj.Details.BillAmount = objDetailsVO.NetBillAmount;
                    obj.Details.Date = BizActionObj.Details.Date;
                    obj.Details.LinkServer = BizActionObj.Details.LinkServer;
                    obj = (clsAddPaymentBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);
                    if (obj.SuccessStatus == -1) throw new Exception();
                    BizActionObj.Details.PaymentDetails.ID = obj.Details.ID;
                }

                //if (BizActionObj.Details.IsFreezed == true && BizActionObj.Details.PathoWorkOrder != null)//&& BizActionObj.Details.PathoWorkOrder.ServiceID > 0
                if (BizActionObj.Details.PathoWorkOrder != null && ((BizActionObj.Details.IsFreezed == true && BizActionObj.Details.IsIPDBill == false) || (BizActionObj.Details.IsIPDBill == true))) //&& BizActionObj.Details.PathoWorkOrder.ServiceID>0                
                {
                    //UpdatePathOrderBookingDetailStatus(BizActionObj.Details.ID, BizActionObj.Details.UnitID, con, trans); //C
                    BizActionObj.Details.PathoWorkOrder.Opd_Ipd_External_ID = BizActionObj.Details.Opd_Ipd_External_Id;
                    BizActionObj.Details.PathoWorkOrder.Opd_Ipd_External_UnitID = BizActionObj.Details.Opd_Ipd_External_UnitId;
                    BizActionObj.Details.PathoWorkOrder.SampleType = false;
                    BizActionObj.Details.PathoWorkOrder.Opd_Ipd_External = BizActionObj.Details.Opd_Ipd_External;
                    BizActionObj.Details.PathoWorkOrder.UnitId = BizActionObj.Details.UnitID;
                    BizActionObj.Details.PathoWorkOrder.AddedDateTime = BizActionObj.Details.AddedDateTime;
                    BizActionObj.Details.PathoWorkOrder.OrderDate = BizActionObj.Details.Date;
                    BizActionObj.Details.PathoWorkOrder.BillNo = BizActionObj.Details.BillNo;
                    BizActionObj.Details.PathoWorkOrder.BillID = BizActionObj.Details.ID;
                    for (int chargecount = 0; chargecount < objDetailsVO.ChargeDetails.Count; chargecount++)
                    {
                        if (BizActionObj.Details.PathoWorkOrder.PathoSpecilizationID == objDetailsVO.ChargeDetails[chargecount].ServiceSpecilizationID)
                        {
                            BizActionObj.Details.PathoWorkOrder.DoctorID = BizActionObj.Details.PathoWorkOrder.DoctorID;
                            if (objDetailsVO.ChargeDetails[chargecount].POBID > 0)
                                BizActionObj.Details.PathoWorkOrder.ID = objDetailsVO.ChargeDetails[chargecount].POBID;
                            BizActionObj.Details.PathoWorkOrder.ID = BizActionObj.Details.PathoWorkOrder.ID;  // objDetailsVO.ChargeDetails[chargecount].POBID;  //to update T_PathOrderBooking using Id
                            clsPathOrderBookingVO BizAction = new clsPathOrderBookingVO();
                            if (BizActionObj.Details.PathoWorkOrder.ID > 0) //if (objDetailsVO.ChargeDetails[chargecount].POBID > 0)
                                BizAction = GetPathologyTestDetails(objDetailsVO.ChargeDetails[chargecount].ServiceId, UserVo, BizActionObj.Details.PathoWorkOrder.ID, objDetailsVO.ChargeDetails[chargecount].UnitID, objDetailsVO.ChargeDetails[chargecount].ID, con, trans); //GetPathologyTestDetails(objDetailsVO.ChargeDetails[chargecount].ServiceId, UserVo, objDetailsVO.ChargeDetails[chargecount].POBID, objDetailsVO.ChargeDetails[chargecount].UnitID);
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
                                    BizActionObj.Details.PathoWorkOrder.Items.Add(item);
                                }
                        }
                    }
                    clsBaseOrderBookingDAL objBaseDAL = clsBaseOrderBookingDAL.GetInstance();
                    clsAddPathOrderBookingBizActionVO obj = new clsAddPathOrderBookingBizActionVO();
                    obj.PathOrderBooking = BizActionObj.Details.PathoWorkOrder;
                    obj.PathOrderBookingDetailList = BizActionObj.Details.PathoWorkOrder.Items;
                    // Changes By Bhushanp 20012017 
                    if (obj.PathOrderBookingDetailList.Count > 0)
                    {
                        obj = (clsAddPathOrderBookingBizActionVO)objBaseDAL.AddPathOrderBooking(obj, UserVo, trans, con);
                        if (obj.SuccessStatus == -1) throw new Exception();
                        if (obj.PathOrderBooking.ID > 0)
                            BizActionObj.Details.PathoWorkOrder.ID = obj.PathOrderBooking.ID;
                    }
                    //Added By Bhushanp For Delete Path Work Order

                    DbCommand Sqlcommand = dbServer.GetSqlStringCommand("DELETE FROM T_PathOrderBookingDetails Where OrderID IN (Select ID FROM T_PathOrderBooking Where BillID = " + BizActionObj.Details.PathoWorkOrder.BillID + ") AND UnitID =" + BizActionObj.Details.PathoWorkOrder.UnitId + " AND Status = 0 ");
                    int sqlStatus = dbServer.ExecuteNonQuery(Sqlcommand, trans);

                    // Added by CDS For IPD 
                    if (BizActionObj.Details.DeletedChargeDetails != null && BizActionObj.Details.DeletedChargeDetails.Count > 0 && BizActionObj.Details.IsIPDBill == true)
                    {
                        foreach (clsChargeVO item in BizActionObj.Details.DeletedChargeDetails)
                        {
                            if (item.ID != 0 && item.TariffServiceId != 0 && BizActionObj.Details.PathoSpecilizationID == item.ServiceSpecilizationID)
                            {
                                DbCommand SqlcommandP = dbServer.GetSqlStringCommand("Delete from T_PathOrderBookingDetails where ChargeID = " + item.ID + " And TariffServiceID = " + item.TariffServiceId + " And UnitID = " + item.UnitID + "");
                                int sqlStatusP = dbServer.ExecuteNonQuery(SqlcommandP, trans);
                            }
                        }
                    }
                }

                #region commented Code By Yogesh K 17 8 16
                if (BizActionObj.Details.RadiologyWorkOrder != null && BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails.Count > 0 && ((BizActionObj.Details.IsFreezed == false)))
                {
                    BizActionObj.Details.RadiologyWorkOrder.Opd_Ipd_External_ID = BizActionObj.Details.Opd_Ipd_External_Id;
                    BizActionObj.Details.RadiologyWorkOrder.Opd_Ipd_External_UnitID = BizActionObj.Details.Opd_Ipd_External_UnitId;
                    BizActionObj.Details.RadiologyWorkOrder.Opd_Ipd_External = BizActionObj.Details.Opd_Ipd_External;
                    BizActionObj.Details.RadiologyWorkOrder.UnitID = BizActionObj.Details.UnitID;
                    BizActionObj.Details.RadiologyWorkOrder.AddedDateTime = BizActionObj.Details.AddedDateTime;
                    BizActionObj.Details.RadiologyWorkOrder.Date = BizActionObj.Details.Date;
                    BizActionObj.Details.RadiologyWorkOrder.BillNo = BizActionObj.Details.BillNo;
                    for (int count = 0; count < BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails.Count; count++)
                    {
                        for (int chargecount = 0; chargecount < objDetailsVO.ChargeDetails.Count; chargecount++)
                        {
                            if (objDetailsVO.ChargeDetails[chargecount].ROBID > 0)
                                BizActionObj.Details.RadiologyWorkOrder.ID = objDetailsVO.ChargeDetails[chargecount].ROBID;

                            BizActionObj.Details.RadiologyWorkOrder.ID = BizActionObj.Details.RadiologyWorkOrder.ID;  //objDetailsVO.ChargeDetails[chargecount].ROBID; //to update T_RadiologyOrderBooking using Id

                            if (BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[count].TariffServiceID == objDetailsVO.ChargeDetails[chargecount].TariffServiceId)
                            {
                                BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[count].ID = objDetailsVO.ChargeDetails[chargecount].ROBDID;  //to update T_RadiologyOrderBookingDetails using Id
                                BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[count].ChargeID = objDetailsVO.ChargeDetails[chargecount].ID;
                                clsRadOrderBookingDetailsVO BizAction = new clsRadOrderBookingDetailsVO();
                                if (BizActionObj.Details.RadiologyWorkOrder.ID > 0)    // if (objDetailsVO.ChargeDetails[chargecount].ROBID > 0)
                                {
                                    BizAction = GetRadilogyTestDetails(BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[count].ServiceID, UserVo);
                                }
                                else
                                    BizAction = GetRadilogyTestDetails(BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[count].ServiceID, UserVo);
                                if (BizAction != null)
                                {
                                    BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[count].TestID = BizAction.TestID;
                                }
                                break;
                            }
                        }
                    }
                    clsBaseRadiologyDAL objBaseDAL = clsBaseRadiologyDAL.GetInstance();
                    clsAddRadOrderBookingBizActionVO obj = new clsAddRadOrderBookingBizActionVO();
                    obj.BookingDetails = BizActionObj.Details.RadiologyWorkOrder;

                    obj = (clsAddRadOrderBookingBizActionVO)objBaseDAL.AddOrderBooking(obj, UserVo, con, trans);
                    if (obj.SuccessStatus == -1) throw new Exception();
                    if (obj.BookingDetails.ID > 0)
                        BizActionObj.Details.RadiologyWorkOrder.ID = obj.BookingDetails.ID;
                    if (BizActionObj.DeletedRadSerDetailsList != null && BizActionObj.DeletedRadSerDetailsList.Count > 0)
                    {
                        foreach (clsChargeVO item in BizActionObj.DeletedRadSerDetailsList)
                        {
                            if (item.ID != 0 && item.TariffServiceId != 0)//Deleted Condition && BizActionObj.Details.RadioSpecilizationID == item.ServiceSpecilizationID
                            {
                                DbCommand SqlcommandR = dbServer.GetSqlStringCommand("Delete from T_RadiologyOrderBookingDetails where ChargeID = " + item.ID + " And TariffServiceId = " + item.TariffServiceId + " And UnitID = " + item.UnitID + " And IsResultEntry=0 ");
                                int sqlStatusR = dbServer.ExecuteNonQuery(SqlcommandR, trans);
                            }
                        }
                    }
                }

                #endregion
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
                    obj.Details.BillID = BizActionObj.Details.ID;
                    obj.Details.IsBilled = BizActionObj.Details.IsFreezed;
                    obj = (clsAddItemSalesBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);
                    if (obj.SuccessStatus == -1) throw new Exception();

                    objDetailsVO.PharmacyItems.ID = obj.Details.ID;
                }

                #region Doctor Share on 12062018
                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddDoctorPaymentDetails");
                dbServer.AddInParameter(command1, "BillID", DbType.Int64, BizActionObj.Details.ID);
                dbServer.AddInParameter(command1, "BillUnitID", DbType.Int64, BizActionObj.Details.UnitID);

                dbServer.AddInParameter(command1, "Opd_Ipd_External_Id", DbType.Int64, objDetailsVO.Opd_Ipd_External_Id);
                dbServer.AddInParameter(command1, "Opd_Ipd_External_UnitId", DbType.Int64, objDetailsVO.Opd_Ipd_External_UnitId);
                dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                #endregion


                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.Details = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }

            return BizActionObj;
        }


        private clsRadOrderBookingDetailsVO GetRadilogyTestDetails(long pServiceID, clsUserVO UserVo, long rOBID, long rOBUnitID)
        {
            clsRadOrderBookingDetailsVO BizAction = null;
            try
            {

                clsBaseRadiologyDAL objBaseDAL = clsBaseRadiologyDAL.GetInstance();
                clsGetRadTestDetailsBizActionVO obj = new clsGetRadTestDetailsBizActionVO();

                obj.ServiceID = pServiceID;
                obj.robID = rOBID;
                obj.robUnitID = rOBUnitID;
                //obj.PathOrderBookingDetailList = BizActionObj.Details.PathoWorkOrder.Items;

                obj = (clsGetRadTestDetailsBizActionVO)objBaseDAL.GetTestListWithDetailsID(obj, UserVo);

                BizAction = new clsRadOrderBookingDetailsVO();

                if (obj != null && obj.TestList != null && obj.TestList.Count > 0)
                {
                    BizAction.TestID = obj.TestList[0].TestID;
                    BizAction.ID = obj.TestList[0].ROBDID;
                }



            }
            catch (Exception)
            {

                // throw;
            }

            return BizAction;
        }

        private clsAddBillBizActionVO AddDetails(clsAddBillBizActionVO BizActionObj, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {
            DbConnection con = null;
            DbTransaction trans = null;
            bool IsPackageConsumption = false; //Added By Bhushanp For Package Consumption 07082017
            try
            {
                if (myConnection != null)
                    con = myConnection;
                else
                    con = dbServer.CreateConnection();


                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                if (myTransaction != null)
                    trans = myTransaction;
                else
                    trans = con.BeginTransaction();


                long ParentID = 0;//T_charge.id
                long CDParentID = 0;//T_chargeDetails.id

                long chargeID = 0;
                double TestCharge = 0;
                if (BizActionObj.LogInfoList == null)
                {  // By Umesh
                    BizActionObj.LogInfoList = new List<LogInfo>();
                }

                #region For Package Advance & Bill Save in transaction : added on 16082018

                if (BizActionObj.IsFromSaveAndPackageBill == true && BizActionObj.objPackageAdvanceVODetails != null)
                {
                    clsBaseAdvanceDAL objAdvBaseDAL = clsBaseAdvanceDAL.GetInstance();
                    clsAddAdvanceBizActionVO objAdv = new clsAddAdvanceBizActionVO();

                    objAdv = BizActionObj.objPackageAdvanceVODetails;

                    objAdv = (clsAddAdvanceBizActionVO)objAdvBaseDAL.AddAdvanceWithPackageBill(objAdv, UserVo, con, trans);   //obj = (clsAddChargeBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);
                    if (objAdv.SuccessStatus == -1) throw new Exception();

                    BizActionObj.Details.AdvanceID = objAdv.Details.ID;
                    BizActionObj.Details.AdvanceUnitID = UserVo.UserLoginInfo.UnitId;       // objAdv.Details.UnitID;

                }

                #endregion

                clsBillVO objDetailsVO = BizActionObj.Details;
                long chID = 0;
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
                            objDetailsVO.ChargeDetails[i].IsPackageConsumption = true;//Added By Bhushanp For Package Consumption 09102017
                        }

                        if (obj.Details.Status == true)
                            obj.Details.PaidAmount = obj.Details.NetAmount;
                        else
                            obj.Details.PaidAmount = 0;

                        obj.Details.IsBilled = BizActionObj.Details.IsFreezed;
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
                        chID = obj.Details.ID;  // By Umesh for T_charge  ID into log Activity
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

                            objCharge.Details.IsBilled = BizActionObj.Details.IsFreezed;
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
                // for T_chargeDetails
                if (objDetailsVO.ChargeDetails.Count > 0 && IsAuditTrail == true && BizActionObj.LogInfoList != null)
                {
                    LogInfo LogInformation = new LogInfo();
                    LogInformation.guid = new Guid();
                    LogInformation.UserId = UserVo.ID;
                    LogInformation.TimeStamp = DateTime.Now;
                    LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                    LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                    LogInformation.Message = " 9 : T_chargeDetails" //+ Convert.ToString(lineNumber)
                                           + "Unit Id : " + Convert.ToString(UserVo.UserLoginInfo.UnitId);
                    foreach (clsChargeVO obj in objDetailsVO.ChargeDetails.ToList())
                    {
                        LogInformation.Message = LogInformation.Message
                                                                + " ,Charge ID:" + Convert.ToString(chID)
                                                                + " , Rate : " + Convert.ToString(obj.Rate)
                                                                + " , Quantity : " + Convert.ToString(obj.Quantity)
                                                                + " , Total Amount : " + Convert.ToString(obj.TotalAmount)
                                                                + " , Concession Amount : " + Convert.ToString(obj.ConcessionAmount)
                                                                + " , Service Tax Amount : " + Convert.ToString(obj.ServiceTaxAmount)
                                                                + " , Net Amount : " + Convert.ToString(obj.NetAmount)
                                                                + " , Paid Amount : " + Convert.ToString(obj.PaidAmount)
                                                                + " , Opd_ipd_external_ID : " + Convert.ToString(obj.Opd_Ipd_External_Id)
                                                                + " , Opd_ipd_external_UnitID : " + Convert.ToString(obj.Opd_Ipd_External_UnitId)
                                                                + " , Refund ID : " + Convert.ToString(obj.RefundID)
                                                                + " , Refund Amount: " + Convert.ToString(obj.RefundAmount)
                                                                ;
                    }

                    BizActionObj.LogInfoList.Add(LogInformation);
                }
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddBill");

                dbServer.AddInParameter(command, "IsPackageBill", DbType.Int64, BizActionObj.IsPackageBill);
                // dbServer.AddInParameter(command, "PackageID", DbType.Int64, BizActionObj.PackageID);

                if (BizActionObj.IsPackageBill == true)
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
                //By Anjali...............................
                dbServer.AddInParameter(command, "CalculatedNetBillAmount", DbType.Double, objDetailsVO.CalculatedNetBillAmount);
                //........................................
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


                dbServer.AddInParameter(command, "ConcessionReasonId", DbType.Int64, objDetailsVO.ConcessionReasonId);//Added by Yogesh K 20 Apr 2016
                dbServer.AddInParameter(command, "ConcessionRemark", DbType.String, objDetailsVO.ConcessionRemark); //Added By Bhushnp 09032017
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);
                //***//
                dbServer.AddInParameter(command, "AgainstDonor", DbType.Boolean, objDetailsVO.AgainstDonor);
                dbServer.AddInParameter(command, "LinkPatientID", DbType.Int64, objDetailsVO.LinkPatientID);
                dbServer.AddInParameter(command, "LinkPatientUnitID", DbType.Int64, objDetailsVO.LinkPatientUnitID);
                //--------------
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                objDetailsVO.UnitID = UserVo.UserLoginInfo.UnitId;
                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "IsDraftBill", DbType.Boolean, objDetailsVO.IsDraftBill);// Added By BhushanP
                dbServer.AddInParameter(command, "IsPackageConsumption", DbType.Boolean, IsPackageConsumption);////Added By Bhushanp For Package Consumption 07082017
                dbServer.AddInParameter(command, "IsCouterSalesPackage", DbType.Boolean, BizActionObj.IsCouterSalesPackage); //Couter Sale Package 
                dbServer.AddInParameter(command, "AdvanceID", DbType.Int64, objDetailsVO.AdvanceID);////Added By Bhushanp For Package Consumption 07082017
                dbServer.AddInParameter(command, "AdvanceUnitID", DbType.Int64, objDetailsVO.AdvanceUnitID);////Added By Bhushanp For Package Consumption 07082017

                //dbServer.AddParameter(command, "BillNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000");
                dbServer.AddParameter(command, "BillNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000");
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
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

                //***//
                dbServer.AddInParameter(command, "PrescriptionDetailsID", DbType.String, BizActionObj.PrescriptionDetailsID);
                dbServer.AddInParameter(command, "InvestigationDetailsID", DbType.String, BizActionObj.InvestigationDetailsID);
                dbServer.AddInParameter(command, "PrescriptionDetailsDrugID", DbType.String, BizActionObj.PrescriptionDetailsDrugID);

                //

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Details.ID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.Details.BillNo = (string)dbServer.GetParameterValue(command, "BillNo");

                //Patch created for double bill entry
                if (BizActionObj.SuccessStatus == -2) throw new Exception();
                //

                if (BizActionObj.Details.IsFreezed == true && BizActionObj.Details.PaymentDetails != null)
                {
                    clsBasePaymentDAL objBaseDAL = clsBasePaymentDAL.GetInstance();
                    clsAddPaymentBizActionVO obj = new clsAddPaymentBizActionVO();

                    obj.Details = new clsPaymentVO();
                    obj.Details = BizActionObj.Details.PaymentDetails;

                    obj.Details.BillID = BizActionObj.Details.ID;
                    obj.Details.BillAmount = objDetailsVO.NetBillAmount;
                    obj.Details.Date = BizActionObj.Details.Date;
                    obj.Details.LinkServer = BizActionObj.Details.LinkServer;
                    obj.myTransaction = true;
                    obj = (clsAddPaymentBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);
                    if (obj.SuccessStatus == -1) throw new Exception();
                    BizActionObj.Details.PaymentDetails.ID = obj.Details.ID;
                    if (IsAuditTrail == true && BizActionObj.LogInfoList != null)
                    {
                        LogInfo LogInformation = new LogInfo();    // By Umesh T_payment
                        LogInformation.guid = new Guid();
                        LogInformation.UserId = UserVo.ID;
                        LogInformation.TimeStamp = DateTime.Now;
                        LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                        LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                        LogInformation.Message = " 10 : T_payment" //+ Convert.ToString(lineNumber)
                                               + "Unit Id : " + Convert.ToString(UserVo.UserLoginInfo.UnitId)
                            //foreach (clsChargeVO objy in objDetailsVO.ChargeDetails.ToList())
                            //{
                                               + " , Bill ID : " + Convert.ToString(obj.Details.BillID)
                                               + " , Bill Amount : " + Convert.ToString(obj.Details.BillAmount)
                                               + " , Bill Balance Amount : " + Convert.ToString(obj.Details.BillBalanceAmount)
                                               + " , Advance ID : " + Convert.ToString(obj.Details.AdvanceID)
                                               + " , Advance Amount : " + Convert.ToString(obj.Details.AdvanceAmount)
                                               + " , Refund ID : " + Convert.ToString(obj.Details.RefundID)
                                               + " , Refund Amount : " + Convert.ToString(obj.Details.RefundAmount)
                                               + " , Costing Division ID : " + Convert.ToString(obj.Details.CostingDivisionID)
                                               + " , Payee Naration : " + Convert.ToString(obj.Details.PayeeNarration)
                                               + " , TDS Amount : " + Convert.ToString(obj.Details.TDSAmt)
                                               + " , Receipt No: " + Convert.ToString(obj.Details.ReceiptNo)
                                               ;
                        //    }
                        foreach (var item in obj.Details.PaymentDetails)   // For T_paymentDetails
                        {
                            LogInformation.Message = LogInformation.Message + "\r\n" + "Payment Details" + "\r\n"
                                                                          + " , Payment ID : " + Convert.ToString(obj.Details.ID)
                                                                          + " , Payment Mode ID : " + Convert.ToString(item.PaymentModeID)
                                                                          + " , Number : " + Convert.ToString(item.Number)
                                                                          + " , Bank ID : " + Convert.ToString(item.BankID)
                                                                          + " , Advance ID : " + Convert.ToString(item.AdvanceID) + "\r\n"
                                                                          ;
                        }
                        BizActionObj.LogInfoList.Add(LogInformation);
                    }
                }

                //if (BizActionObj.Details.IsFreezed == true && BizActionObj.Details.PathoWorkOrder != null && BizActionObj.Details.PathoWorkOrder.PathoSpecilizationID > 0) //&& BizActionObj.Details.PathoWorkOrder.ServiceID>0
                if (BizActionObj.Details.PathoWorkOrder != null && BizActionObj.Details.PathoWorkOrder.PathoSpecilizationID > 0 && ((BizActionObj.Details.IsFreezed == true && BizActionObj.Details.IsIPDBill == false) || (BizActionObj.Details.IsIPDBill == true))) //&& BizActionObj.Details.PathoWorkOrder.ServiceID>0
                {
                    BizActionObj.Details.PathoWorkOrder.Opd_Ipd_External_ID = BizActionObj.Details.Opd_Ipd_External_Id;
                    BizActionObj.Details.PathoWorkOrder.Opd_Ipd_External_UnitID = BizActionObj.Details.Opd_Ipd_External_UnitId;
                    BizActionObj.Details.PathoWorkOrder.SampleType = false;
                    BizActionObj.Details.PathoWorkOrder.Opd_Ipd_External = BizActionObj.Details.Opd_Ipd_External;
                    // BizActionObj.Details.PathoWorkOrder.Opd_Ipd_External_UnitID = BizActionObj.Details.Opd_Ipd_External
                    BizActionObj.Details.PathoWorkOrder.UnitId = UserVo.UserLoginInfo.UnitId;
                    BizActionObj.Details.PathoWorkOrder.AddedDateTime = BizActionObj.Details.AddedDateTime;
                    BizActionObj.Details.PathoWorkOrder.OrderDate = BizActionObj.Details.Date;
                    BizActionObj.Details.PathoWorkOrder.BillNo = BizActionObj.Details.BillNo;
                    BizActionObj.Details.PathoWorkOrder.BillID = BizActionObj.Details.ID;

                    //added by rohini dated 8/3/2016 as per disscuss with priyanka for external patient goes direct recive
                    // BizActionObj.Details.PathoWorkOrder.IsExternalPatient =false;
                    //
                    //Added by Priyanka
                    //for (int count = 0; count < BizActionObj.Details.PathoWorkOrder.Items.Count; count++)
                    //{
                    for (int chargecount = 0; chargecount < objDetailsVO.ChargeDetails.Count; chargecount++)
                    {
                        if (BizActionObj.Details.PathoWorkOrder.PathoSpecilizationID == objDetailsVO.ChargeDetails[chargecount].ServiceSpecilizationID)
                        {


                            BizActionObj.Details.PathoWorkOrder.DoctorID = BizActionObj.Details.PathoWorkOrder.DoctorID;


                            clsPathOrderBookingVO BizAction = new clsPathOrderBookingVO();
                            BizAction = GetPathologyTestDetails(objDetailsVO.ChargeDetails[chargecount].ServiceId, UserVo);
                            if (BizAction.Items != null && BizAction.Items.Count > 0)
                                foreach (var item in BizAction.Items)
                                {
                                    item.ServiceID = objDetailsVO.ChargeDetails[chargecount].ServiceId;
                                    item.ChargeID = objDetailsVO.ChargeDetails[chargecount].ID;
                                    item.TariffServiceID = objDetailsVO.ChargeDetails[chargecount].TariffServiceId;
                                    item.TestCharges = objDetailsVO.ChargeDetails[chargecount].Rate;

                                    BizActionObj.Details.PathoWorkOrder.Items.Add(item);
                                }
                        }
                        //}

                    }

                    clsBaseOrderBookingDAL objBaseDAL = clsBaseOrderBookingDAL.GetInstance();
                    clsAddPathOrderBookingBizActionVO obj = new clsAddPathOrderBookingBizActionVO();
                    obj.myTransaction = true;
                    obj.PathOrderBooking = BizActionObj.Details.PathoWorkOrder;
                    obj.PathOrderBookingDetailList = BizActionObj.Details.PathoWorkOrder.Items;
                    obj = (clsAddPathOrderBookingBizActionVO)objBaseDAL.AddPathOrderBooking(obj, UserVo, trans, con);
                    if (obj.SuccessStatus == -1) throw new Exception();
                    BizActionObj.Details.PathoWorkOrder.ID = obj.PathOrderBooking.ID;

                }

                #region Add Radiology Work Order
                // if (BizActionObj.Details.IsFreezed == true && BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails.Count > 0)//Commented by Yogesh k 23 5 16
                if (BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails.Count > 0)
                {
                    BizActionObj.Details.RadiologyWorkOrder.Opd_Ipd_External_ID = BizActionObj.Details.Opd_Ipd_External_Id;
                    BizActionObj.Details.RadiologyWorkOrder.Opd_Ipd_External_UnitID = BizActionObj.Details.Opd_Ipd_External_UnitId;
                    //BizActionObj.Details.RadiologyWorkOrder.SampleType = false;
                    BizActionObj.Details.RadiologyWorkOrder.Opd_Ipd_External = BizActionObj.Details.Opd_Ipd_External;
                    // BizActionObj.Details.PathoWorkOrder.Opd_Ipd_External_UnitID = BizActionObj.Details.Opd_Ipd_External
                    BizActionObj.Details.RadiologyWorkOrder.UnitID = BizActionObj.Details.UnitID;
                    BizActionObj.Details.RadiologyWorkOrder.AddedDateTime = BizActionObj.Details.AddedDateTime;
                    BizActionObj.Details.RadiologyWorkOrder.Date = BizActionObj.Details.Date;
                    BizActionObj.Details.RadiologyWorkOrder.BillNo = BizActionObj.Details.BillNo;
                    BizActionObj.Details.RadiologyWorkOrder.BillID = BizActionObj.Details.ID;

                    for (int count = 0; count < BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails.Count; count++)
                    {
                        for (int chargecount = 0; chargecount < objDetailsVO.ChargeDetails.Count; chargecount++)
                        {
                            if (objDetailsVO.ChargeDetails[chargecount].isPackageService == false)
                            {
                                if (BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[count].TariffServiceID == objDetailsVO.ChargeDetails[chargecount].TariffServiceId)
                                {
                                    BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[count].ChargeID = objDetailsVO.ChargeDetails[chargecount].ID;
                                    clsRadOrderBookingDetailsVO BizAction = new clsRadOrderBookingDetailsVO();
                                    BizAction = GetRadilogyTestDetails(BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[count].ServiceID, UserVo);
                                    if (BizAction != null) BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[count].TestID = BizAction.TestID;
                                    break;
                                }
                            }
                            else
                            {
                                BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[count].ChargeID = objDetailsVO.ChargeDetails[chargecount].ID;
                                clsRadOrderBookingDetailsVO BizAction = new clsRadOrderBookingDetailsVO();
                                BizAction = GetRadilogyTestDetails(BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[count].ServiceID, UserVo);
                                if (BizAction != null) BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[count].TestID = BizAction.TestID;
                                break;
                            }
                        }

                    }


                    clsBaseRadiologyDAL objBaseDAL = clsBaseRadiologyDAL.GetInstance();
                    clsAddRadOrderBookingBizActionVO obj = new clsAddRadOrderBookingBizActionVO();

                    obj.BookingDetails = BizActionObj.Details.RadiologyWorkOrder;
                    // obj. = BizActionObj.Details.PathoWorkOrder.Items;
                    obj.myTransaction = true;
                    obj = (clsAddRadOrderBookingBizActionVO)objBaseDAL.AddOrderBooking(obj, UserVo, con, trans);
                    if (obj.SuccessStatus == -1) throw new Exception();
                    objDetailsVO.RadiologyWorkOrder.ID = obj.BookingDetails.ID;
                }
                #endregion
                #region Commented Code
                //if (BizActionObj.Details.RadiologyWorkOrder != null && BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails.Count > 0 && ((BizActionObj.Details.IsFreezed == true && BizActionObj.Details.IsIPDBill == false) || (BizActionObj.Details.IsIPDBill == true)))
                //{
                //    BizActionObj.Details.RadiologyWorkOrder.Opd_Ipd_External_ID = BizActionObj.Details.Opd_Ipd_External_Id;
                //    BizActionObj.Details.RadiologyWorkOrder.Opd_Ipd_External_UnitID = BizActionObj.Details.Opd_Ipd_External_UnitId;
                //    //BizActionObj.Details.RadiologyWorkOrder.SampleType = false;
                //    BizActionObj.Details.RadiologyWorkOrder.Opd_Ipd_External = BizActionObj.Details.Opd_Ipd_External;
                //    // BizActionObj.Details.PathoWorkOrder.Opd_Ipd_External_UnitID = BizActionObj.Details.Opd_Ipd_External
                //    BizActionObj.Details.RadiologyWorkOrder.UnitID = BizActionObj.Details.UnitID;
                //    BizActionObj.Details.RadiologyWorkOrder.AddedDateTime = BizActionObj.Details.AddedDateTime;
                //    BizActionObj.Details.RadiologyWorkOrder.Date = BizActionObj.Details.Date;
                //    BizActionObj.Details.RadiologyWorkOrder.BillNo = BizActionObj.Details.BillNo;
                //    BizActionObj.Details.RadiologyWorkOrder.BillID = BizActionObj.Details.ID;

                //    for (int count = 0; count < BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails.Count; count++)
                //    {
                //        for (int chargecount = 0; chargecount < objDetailsVO.ChargeDetails.Count; chargecount++)
                //        {
                //            if (BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[count].TariffServiceID == objDetailsVO.ChargeDetails[chargecount].TariffServiceId)
                //            {
                //                BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[count].ChargeID = objDetailsVO.ChargeDetails[chargecount].ID;
                //                clsRadOrderBookingDetailsVO BizAction = new clsRadOrderBookingDetailsVO();
                //                BizAction = GetRadilogyTestDetails(BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[count].ServiceID, UserVo);
                //                if (BizAction != null) BizActionObj.Details.RadiologyWorkOrder.OrderBookingDetails[count].TestID = BizAction.TestID;
                //                break;
                //            }
                //        }

                //    }

                //    clsBaseRadiologyDAL objBaseDAL = clsBaseRadiologyDAL.GetInstance();
                //    clsAddRadOrderBookingBizActionVO obj = new clsAddRadOrderBookingBizActionVO();

                //    obj.BookingDetails = BizActionObj.Details.RadiologyWorkOrder;
                //    // obj. = BizActionObj.Details.PathoWorkOrder.Items;
                //    obj.myTransaction = true;
                //    obj = (clsAddRadOrderBookingBizActionVO)objBaseDAL.AddOrderBooking(obj, UserVo, con, trans);
                //    if (obj.SuccessStatus == -1) throw new Exception();
                //    objDetailsVO.RadiologyWorkOrder.ID = obj.BookingDetails.ID;
                //}
                #endregion
                bool IsFromCountersale = false;  //By Umesh
                if (objDetailsVO.PharmacyItems.Items.Count > 0)
                {
                    IsFromCountersale = true;
                    clsBaseItemSalesDAL objBaseDAL = clsBaseItemSalesDAL.GetInstance();
                    clsAddItemSalesBizActionVO obj = new clsAddItemSalesBizActionVO();
                    // obj.Details.ItemSaleNo 
                    obj.Details = objDetailsVO.PharmacyItems;
                    obj.Details.BillID = BizActionObj.Details.ID;
                    obj.Details.IsBilled = BizActionObj.Details.IsFreezed;
                    obj.myTransaction = true;
                    obj = (clsAddItemSalesBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);
                    if (obj.SuccessStatus == -1) throw new Exception();
                    objDetailsVO.PharmacyItems.ID = obj.Details.ID;

                    #region Package New Changes Added on 28042018
                    // When get call from Counter Sale to check whether Pharmacy Consumed Amount > Pharmacy  Component
                    if (BizActionObj.IsPackagePharmacyConsumption == true && BizActionObj.objPatientPackInfoVODetails != null)
                    {
                        //DbCommand commandCS = dbServer.GetStoredProcCommand("CIMS_GetPatientPackageDetailsForCS");

                        //dbServer.AddInParameter(commandCS, "PackageBillID", DbType.Int64, BizActionObj.objPatientPackInfoVODetails.PackageBillID);
                        //dbServer.AddInParameter(commandCS, "PackageBillUnitID", DbType.Int64, BizActionObj.objPatientPackInfoVODetails.PackageBillUnitID);

                        //DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(commandCS);
                        //if (reader.HasRows)
                        //{
                        //    if (BizActionObj.objPatientPackInfoVODetails.MasterList == null)
                        //    {
                        //        BizActionObj.objPatientPackInfoVODetails.MasterList = new List<MasterListItem>();
                        //    }

                        //    while (reader.Read())
                        //    {
                        //        BizActionObj.objPatientPackInfoVODetails.MasterList.Add(new MasterListItem(Convert.ToInt64(reader["PackageID"]), reader["PackageName"].ToString(), Convert.ToInt64(reader["PatientInfoID"]), Convert.ToBoolean(reader["ApplicableToAll"]), Convert.ToDouble(reader["ApplicableToAllDiscount"]), Convert.ToDouble(reader["PharmacyFixedRate"]), Convert.ToInt64(reader["PackageBillID"]), Convert.ToInt64(reader["PackageBillUnitID"]), Convert.ToInt64(reader["ChargeID"]), Convert.ToDouble(Convert.ToDecimal(reader["PackageConsumptionAmount"])), Convert.ToDouble(reader["OPDConsumption"]), Convert.ToDouble(reader["OPDExcludeServices"]), Convert.ToDouble(reader["TotalPackageAdvance"]), Convert.ToDouble(reader["PharmacyConsumeAmount"]), Convert.ToDouble(reader["PackageConsumableLimit"])));
                        //    }
                        //}
                        //reader.Close();

                        //if(BizActionObj.objPatientPackInfoVODetails.MasterList.Count > 0)
                        //{
                        //    if (BizActionObj.objPatientPackInfoVODetails.MasterList[0].PharmacyConsumeAmount > BizActionObj.objPatientPackInfoVODetails.MasterList[0].PharmacyFixedRate)
                        //    {
                        //        throw new Exception();
                        //    }
                        //}

                    }

                    #endregion
                }
                #region Doctor Share on 12062018
                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddDoctorPaymentDetails");
                dbServer.AddInParameter(command1, "BillID", DbType.Int64, BizActionObj.Details.ID);
                dbServer.AddInParameter(command1, "BillUnitID", DbType.Int64, BizActionObj.Details.UnitID);

                dbServer.AddInParameter(command1, "Opd_Ipd_External_Id", DbType.Int64, objDetailsVO.Opd_Ipd_External_Id);
                dbServer.AddInParameter(command1, "Opd_Ipd_External_UnitId", DbType.Int64, objDetailsVO.Opd_Ipd_External_UnitId);
                dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                #endregion
                if (myConnection == null) trans.Commit();
                if (BizActionObj.LogInfoList != null && IsFromCountersale == false)   // By Umesh for activity log
                {
                    if (BizActionObj.LogInfoList.Count > 0 && IsAuditTrail == true)
                    {
                        SetLogInfo(BizActionObj.LogInfoList, UserVo.ID);
                        BizActionObj.LogInfoList.Clear();
                    }
                }
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                //throw;
                BizActionObj.Details = null;
                BizActionObj.SuccessStatus = -1;
                if (myConnection == null) trans.Rollback();
            }
            finally
            {
                if (myConnection == null) con.Close();
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
                    //  BizAction.ID = obj.TestList[0].ROBDID;



                }



            }
            catch (Exception)
            {

                // throw;
            }

            return BizAction;
        }

        //Change By Bhushanp 18012017
        private clsPathOrderBookingVO GetPathologyTestDetails(long pServiceID, clsUserVO UserVo, long pOBID, long pOBUnitID, long pChargeID, DbConnection pConnection, DbTransaction pTransaction) //Change By Bhushanp Maintain transacion 17012017
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
                obj.pChargeID = pChargeID; //Change By Bhushanp 18012017
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



        public override IValueObject GetCompanyCreditDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCompanyCreditDtlsBizActionVO BizActionObj = (clsGetCompanyCreditDtlsBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCompanyCreditDetails");
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.Details == null)
                        BizActionObj.Details = new clsCompanyCreditDetailsVO();
                    while (reader.Read())
                    {
                        try
                        {
                            BizActionObj.Details.Balance = (double)DALHelper.HandleDBNull(reader["Balance"]);
                            BizActionObj.Details.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            BizActionObj.Details.CreditLimit = (double)DALHelper.HandleDBNull(reader["CreditLimit"]);
                            BizActionObj.Details.Refund = (double)DALHelper.HandleDBNull(reader["Refund"]);
                            BizActionObj.Details.TotalAdvance = (double)DALHelper.HandleDBNull(reader["TotalAdvance"]);
                            BizActionObj.Details.Used = (double)DALHelper.HandleDBNull(reader["Used"]);

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
        public override IValueObject GetFreezedList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetFreezedBillListBizActionVO BizActionObj = valueObject as clsGetFreezedBillListBizActionVO;
            try
            {
                DbCommand command;
                //if (BizActionObj.IsIPDBillList == true)
                //{
                //    command = dbServer.GetStoredProcCommand("CIMS_GetIPDFreezedBillListForSearch");
                //    dbServer.AddInParameter(command, "IPDNO", DbType.String, BizActionObj.IPDNO);
                //}
                //else
                //{
                command = dbServer.GetStoredProcCommand("CIMS_GetBillListAgainstBillID");
                //}
                DbDataReader reader;

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                //if (BizActionObj.IsFreeze.HasValue)

                dbServer.AddInParameter(command, "UnitID", DbType.String, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "BillID", DbType.Int64, BizActionObj.BillID);

                #region commented
                //    dbServer.AddInParameter(command, "FreezeBill", DbType.Boolean, BizActionObj.IsFreeze);
                //dbServer.AddInParameter(command, "Refunded", DbType.Boolean, BizActionObj.IsRefunded);
                //dbServer.AddInParameter(command, "Opd_Ipd_External_Id", DbType.Int64, BizActionObj.Opd_Ipd_External_Id);
                //dbServer.AddInParameter(command, "Opd_Ipd_External", DbType.Int64, BizActionObj.Opd_Ipd_External);
                //dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                //dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                //dbServer.AddInParameter(command, "OPDNO", DbType.String, BizActionObj.OPDNO);
                //dbServer.AddInParameter(command, "ContactNo", DbType.String, BizActionObj.ContactNo);
                //dbServer.AddInParameter(command, "BillNO", DbType.String, BizActionObj.BillNO);

                //dbServer.AddInParameter(command, "BillSettleType", DbType.Int16, BizActionObj.BillStatus);
                //dbServer.AddInParameter(command, "FromRefund", DbType.Boolean, BizActionObj.FromRefund);
                //if (BizActionObj.BillType.HasValue)
                //    dbServer.AddInParameter(command, "BillType", DbType.Int16, (Int16)BizActionObj.BillType);
                //dbServer.AddInParameter(command, "UnitID", DbType.String, BizActionObj.UnitID);
                //if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                //    dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.FirstName);
                ////dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(BizActionObj.FirstName));
                //if (BizActionObj.MiddleName != null && BizActionObj.MiddleName.Length != 0)
                //    dbServer.AddInParameter(command, "MiddleName", DbType.String, BizActionObj.MiddleName);
                ////dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(BizActionObj.MiddleName));
                //if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                //    dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.LastName);
                ////dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(BizActionObj.LastName));
                //if (BizActionObj.FromDate != null)
                //    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate.Value.Date.Date);
                //if (BizActionObj.ToDate != null)
                //{
                //    if (BizActionObj.FromDate != null)
                //    {
                //        // if (BizActionObj.FromDate.Equals(BizActionObj.ToDate))
                //        BizActionObj.ToDate = BizActionObj.ToDate.Value.Date.AddDays(1);
                //    }
                //    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate.Value.Date.Date);

                //}

                //// Added by Changdeo
                //dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, BizActionObj.CostingDivisionID);  //Costing Divisions for Clinical & Pharmacy Billing

                //dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                //dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                //dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);

                //dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                #endregion

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsBillVO>();
                    while (reader.Read())
                    {
                        clsBillVO objVO = new clsBillVO();

                        objVO.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        objVO.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                        objVO.PaidAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PaidAmount"]));
                        objVO.NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"]));
                        objVO.PaymentMode = Convert.ToString(DALHelper.HandleDBNull(reader["PaymentMode"]));
                        objVO.PaymentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PaymentID"]));
                        objVO.PaymentModeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PaymentModeId"]));
                        objVO.Number = Convert.ToString(DALHelper.HandleDBNull(reader["Number"]));
                        objVO.Bank = Convert.ToString(DALHelper.HandleDBNull(reader["Bank"]));
                        objVO.UnitID = BizActionObj.UnitID;
                        objVO.PaymentDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PaymentDetailId"]));

                        #region Commented
                        //objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        //objVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        //objVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

                        //objVO.IsCancelled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCancelled"]));

                        //objVO.FirstName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])));
                        //objVO.MiddleName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"])));
                        //objVO.LastName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])));


                        //objVO.MRNO = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"]));
                        //objVO.ContactNo = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo1"]));
                        //objVO.BillType = (BillTypes)(Convert.ToInt16(DALHelper.HandleDBNull(reader["BillType"])));
                        //objVO.Opd_Ipd_External = Convert.ToInt16(DALHelper.HandleDBNull(reader["Opd_Ipd_External"]));
                        //objVO.Opd_Ipd_External_Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"]));
                        //objVO.Opd_Ipd_External_UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_UnitId"]));

                        //objVO.TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalConcessionAmount"]));


                        //objVO.Opd_Ipd_External_No = (string)DALHelper.HandleDBNull(reader["OPDNO"]);
                        //objVO.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        //if (BizActionObj.IsIPDBillList == true)
                        //{
                        //    objVO.AdmissionType = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionType"]));
                        //    objVO.IPDNO = Convert.ToString(DALHelper.HandleDBNull(reader["IPDNO"]));
                        //    objVO.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        //    objVO.ConcessionAuthorizedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConcessionAuthorizedBy"]));

                        //    objVO.TotalRefund = Convert.ToDouble(DALHelper.HandleDBNull(reader["RefundAmount"]));
                        //    Func<double, bool> myFunc = x => x != 0;
                        //    objVO.IsRefunded = myFunc(objVO.TotalRefund);
                        //}
                        //else
                        //{
                        //    objVO.VisitTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitTypeID"]));
                        //    objVO.TotalRefund = Convert.ToDouble(DALHelper.HandleDBNull(reader["RefundAmount"]));
                        //    Func<double, bool> myFunc = x => x != 0;
                        //    objVO.IsRefunded = myFunc(objVO.TotalRefund);
                        //    objVO.Opd_Ipd_External_No = Convert.ToString(DALHelper.HandleDBNull(reader["OPDNO"]));
                        //    objVO.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        //    //objVO.ConcessiongivenBy = Convert.ToString(DALHelper.HandleDBNull(reader["ConBy"]));
                        //    //objVO.Naration = Convert.ToString(DALHelper.HandleDBNull(reader["naration"]));
                        //}
                        //objVO.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));

                        //objVO.BalanceAmountSelf = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmountSelf"]));
                        //objVO.SelfAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["SelfAmount"]));
                        //objVO.BillPaymentType = (BillPaymentTypes)(Convert.ToInt16(DALHelper.HandleDBNull(reader["BillPaymentType"])));
                        ////objVO.BalanceAmountSelf = objVO.TotalBillAmount - objVO.PaidAmountSelf;
                        //objVO.GrossDiscountPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["GrossDiscountPercentage"]));
                        //objVO.GrossDiscountReasonID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GrossDiscountReasonID"]));
                        //objVO.IsComanyBillCancelled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCompanyBillCancelled"]));
                        //objVO.PatientSourceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceId"]));

                        //objVO.CostingDivisionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CostingDivisionID"]));

                        //if (BizActionObj.IsIPDBillList == true)
                        //{
                        //    objVO.PatientCategoryId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientCategoryId"]));
                        //    objVO.TariffId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffId"]));
                        //}

                        //if (BizActionObj.IsIPDBillList != true)
                        //{
                        //    objVO.PaymentModeDetails = Convert.ToString(DALHelper.HandleDBNull(reader["PaymentMode"]));
                        //}

                        ////Added by priyanka- for refund services
                        //objVO.TotalRefund = (double)DALHelper.HandleDBNull(reader["RefundAmount"]);

                        #endregion


                        BizActionObj.List.Add(objVO);
                        //objVO.PaidAmountSelf
                    }

                }
                reader.NextResult();
                //  BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");

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

        public override IValueObject UpdatePaymentDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = dbServer.CreateConnection();


            try
            {
                clsUpdatePaymentDetailsBizActionVO BizAction = valueObject as clsUpdatePaymentDetailsBizActionVO;

                con.Open();
                trans = con.BeginTransaction();

                DbCommand command1;
                DbCommand command;
                //DbDataReader reader;

                command1 = dbServer.GetStoredProcCommand("CIMS_MaintainPaymentLogBeforeEdit");
                //DbDataReader reader;

                dbServer.AddInParameter(command1, "PaymentID", DbType.Int64, BizAction.PaymentId);
                dbServer.AddInParameter(command1, "PaymentUnitID", DbType.Int64, BizAction.UnitID);
                dbServer.AddInParameter(command1, "PaymentDetailId", DbType.Int64, BizAction.PaymentDetailId);
                //reader = (DbDataReader)dbServer.ExecuteReader(command);
                int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                command = dbServer.GetStoredProcCommand("CIMS_UpdatePaymentDetails");


                //dbServer.AddInParameter(command, "BillID", DbType.Int64, BizAction.BillID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                //dbServer.AddInParameter(command, "BillNo", DbType.Int64, BizAction.BillNO);
                dbServer.AddInParameter(command, "BankID", DbType.String, BizAction.BankID);
                dbServer.AddInParameter(command, "Number", DbType.String, BizAction.Number);
                dbServer.AddInParameter(command, "PaidAmount", DbType.String, BizAction.PaidAmount);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizAction.Date.Value.Date.Date);
                dbServer.AddInParameter(command, "PaymentModeId", DbType.Int64, BizAction.PaymentModeID);
                dbServer.AddInParameter(command, "PaymentDetailId", DbType.Int64, BizAction.PaymentDetailId);


                dbServer.AddInParameter(command, "PaymentID", DbType.Int64, BizAction.PaymentId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                // dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, BizAction.AddedDateTime);
                //reader = (DbDataReader)dbServer.ExecuteReader(command);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);

                trans.Commit();
            }
            catch (Exception Ex)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                trans.Dispose();
                con.Close();
            }

            return valueObject;
        }

        public override IValueObject MaintainPaymentLog(IValueObject valueObject, clsUserVO UserVo)
        {
            throw new NotImplementedException();
        }

        //public override IValueObject MaintainPaymentLog(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    throw 
        //    clsMaintainPaymentModeLogBizActionVO BizActionObj = valueObject as clsMaintainPaymentModeLogBizActionVO;
        //    try
        //    {
        //        DbCommand command;

        //        command = dbServer.GetStoredProcCommand("CIMS_MaintainPaymentLogBeforeEdit");
        //        DbDataReader reader;

        //        dbServer.AddInParameter(command, "PaymentID", DbType.Int64, BizActionObj.PaymentID);

        //        reader = (DbDataReader)dbServer.ExecuteReader(command);



        //    }
        //    catch (Exception e)
        //    {
        //    }
        //    finally
        //    {
        //    }


        //}

        public override IValueObject GetFreezedSearchList(IValueObject valueObject, clsUserVO UserVo)
        {

            clsGetFreezedBillSearchListBizActionVO BizActionObj = valueObject as clsGetFreezedBillSearchListBizActionVO;
            try
            {
                DbCommand command;
                if (BizActionObj.IsIPDBillList == true)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetIPDFreezedBillListForSearch");
                    dbServer.AddInParameter(command, "IPDNO", DbType.String, BizActionObj.IPDNO);
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetFreezedBillListForSearch");
                }
                DbDataReader reader;

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                if (BizActionObj.IsFreeze.HasValue)
                    dbServer.AddInParameter(command, "FreezeBill", DbType.Boolean, BizActionObj.IsFreeze);
                dbServer.AddInParameter(command, "Refunded", DbType.Boolean, BizActionObj.IsRefunded);
                dbServer.AddInParameter(command, "Opd_Ipd_External_Id", DbType.Int64, BizActionObj.Opd_Ipd_External_Id);
                dbServer.AddInParameter(command, "Opd_Ipd_External", DbType.Int64, BizActionObj.Opd_Ipd_External);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "OPDNO", DbType.String, BizActionObj.OPDNO);
                dbServer.AddInParameter(command, "ContactNo", DbType.String, BizActionObj.ContactNo);
                dbServer.AddInParameter(command, "BillNO", DbType.String, BizActionObj.BillNO);
                dbServer.AddInParameter(command, "MRNO", DbType.String, BizActionObj.MRNO);
                dbServer.AddInParameter(command, "BillSettleType", DbType.Int16, BizActionObj.BillStatus);
                dbServer.AddInParameter(command, "FromRefund", DbType.Boolean, BizActionObj.FromRefund);
                dbServer.AddInParameter(command, "IsPaymentModeChange", DbType.Boolean, BizActionObj.IsPaymentModeChange);
                if (BizActionObj.BillType.HasValue)
                    dbServer.AddInParameter(command, "BillType", DbType.Int16, (Int16)BizActionObj.BillType);
                dbServer.AddInParameter(command, "UnitID", DbType.String, BizActionObj.UnitID);
                if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                    dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.FirstName);
                //dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(BizActionObj.FirstName));
                if (BizActionObj.MiddleName != null && BizActionObj.MiddleName.Length != 0)
                    dbServer.AddInParameter(command, "MiddleName", DbType.String, BizActionObj.MiddleName);
                //dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(BizActionObj.MiddleName));
                if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.LastName);
                //dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(BizActionObj.LastName));
                if (BizActionObj.FromDate != null)
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate.Value.Date.Date);
                if (BizActionObj.ToDate != null)
                {
                    if (BizActionObj.FromDate != null)
                    {
                        // if (BizActionObj.FromDate.Equals(BizActionObj.ToDate))
                        BizActionObj.ToDate = BizActionObj.ToDate.Value.Date.AddDays(1);
                    }
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate.Value.Date.Date);

                }

                // Added by Changdeo
                dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, BizActionObj.CostingDivisionID);  //Costing Divisions for Clinical & Pharmacy Billing

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsBillVO>();
                    while (reader.Read())
                    {
                        clsBillVO objVO = new clsBillVO();

                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objVO.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        objVO.IsCancelled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCancelled"]));
                        objVO.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                        objVO.MRNO = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"]));
                        objVO.ContactNo = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo1"]));
                        objVO.BillType = (BillTypes)(Convert.ToInt16(DALHelper.HandleDBNull(reader["BillType"])));
                        objVO.Opd_Ipd_External = Convert.ToInt16(DALHelper.HandleDBNull(reader["Opd_Ipd_External"]));
                        objVO.Opd_Ipd_External_Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"]));
                        objVO.Opd_Ipd_External_UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_UnitId"]));
                        objVO.TotalBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalBillAmount"]));
                        objVO.TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalConcessionAmount"]));
                        objVO.NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"]));
                        //objVO.Opd_Ipd_External_No = (string)DALHelper.HandleDBNull(reader["OPDNO"]);
                        objVO.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        if (BizActionObj.IsIPDBillList == true)
                        {
                            objVO.AdmissionType = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionType"]));
                            objVO.IPDNO = Convert.ToString(DALHelper.HandleDBNull(reader["IPDNO"]));
                            objVO.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                            objVO.ConcessionAuthorizedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConcessionAuthorizedBy"]));

                            objVO.TotalRefund = Convert.ToDouble(DALHelper.HandleDBNull(reader["RefundAmount"]));
                            Func<double, bool> myFunc = x => x != 0;
                            objVO.IsRefunded = myFunc(objVO.TotalRefund);
                        }
                        else
                        {
                            objVO.VisitTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitTypeID"]));
                            objVO.TotalRefund = Convert.ToDouble(DALHelper.HandleDBNull(reader["RefundAmount"]));
                            Func<double, bool> myFunc = x => x != 0;
                            objVO.IsRefunded = myFunc(objVO.TotalRefund);
                            objVO.Opd_Ipd_External_No = Convert.ToString(DALHelper.HandleDBNull(reader["OPDNO"]));
                            objVO.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                            //objVO.ConcessiongivenBy = Convert.ToString(DALHelper.HandleDBNull(reader["ConBy"]));
                            //objVO.Naration = Convert.ToString(DALHelper.HandleDBNull(reader["naration"]));
                        }
                        objVO.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        objVO.FirstName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])));
                        objVO.MiddleName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"])));
                        objVO.LastName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])));
                        objVO.BalanceAmountSelf = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmountSelf"]));
                        objVO.SelfAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["SelfAmount"]));
                        objVO.BillPaymentType = (BillPaymentTypes)(Convert.ToInt16(DALHelper.HandleDBNull(reader["BillPaymentType"])));
                        //objVO.BalanceAmountSelf = objVO.TotalBillAmount - objVO.PaidAmountSelf;
                        objVO.GrossDiscountPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["GrossDiscountPercentage"]));
                        objVO.GrossDiscountReasonID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GrossDiscountReasonID"]));
                        objVO.IsComanyBillCancelled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCompanyBillCancelled"]));
                        objVO.PatientSourceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceId"]));

                        objVO.CostingDivisionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CostingDivisionID"]));
                        //***//BY AJ Date 10/12/2016 
                        objVO.IsModify = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsChange"]));
                        //------------//

                        //if (BizActionObj.IsIPDBillList == true)
                        //{
                        //    objVO.PatientCategoryId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientCategoryId"]));
                        //    objVO.TariffId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffId"]));
                        //}

                        //if (BizActionObj.IsIPDBillList != true)
                        //{
                        //    objVO.PaymentModeDetails = Convert.ToString(DALHelper.HandleDBNull(reader["PaymentMode"]));
                        //}

                        ////Added by priyanka- for refund services
                        //objVO.TotalRefund = (double)DALHelper.HandleDBNull(reader["RefundAmount"]);



                        BizActionObj.List.Add(objVO);
                        //objVO.PaidAmountSelf
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

        public override IValueObject GetSearchList(IValueObject valueObject, clsUserVO UserVo)
        {

            clsGetBillSearchListBizActionVO BizActionObj = valueObject as clsGetBillSearchListBizActionVO;
            try
            {
                DbCommand command;
                if (BizActionObj.IsIPDBillList == true)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetIPDBillListForSearch");
                    dbServer.AddInParameter(command, "IPDNO", DbType.String, BizActionObj.IPDNO);
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetBillListForSearch");

                }
                DbDataReader reader;
                dbServer.AddInParameter(command, "IsRequest", DbType.Boolean, BizActionObj.IsRequest);
                dbServer.AddInParameter(command, "RequestTypeID", DbType.Int64, BizActionObj.RequestTypeID);
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                if (BizActionObj.IsFreeze.HasValue)
                    dbServer.AddInParameter(command, "FreezeBill", DbType.Boolean, BizActionObj.IsFreeze);
                dbServer.AddInParameter(command, "Refunded", DbType.Boolean, BizActionObj.IsRefunded);
                dbServer.AddInParameter(command, "Opd_Ipd_External_Id", DbType.Int64, BizActionObj.Opd_Ipd_External_Id);
                dbServer.AddInParameter(command, "Opd_Ipd_External", DbType.Int64, BizActionObj.Opd_Ipd_External);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "OPDNO", DbType.String, BizActionObj.OPDNO);
                dbServer.AddInParameter(command, "ContactNo", DbType.String, BizActionObj.ContactNo);
                dbServer.AddInParameter(command, "BillNO", DbType.String, BizActionObj.BillNO);
                dbServer.AddInParameter(command, "MRNO", DbType.String, BizActionObj.MRNO);
                dbServer.AddInParameter(command, "BillSettleType", DbType.Int16, BizActionObj.BillStatus);
                dbServer.AddInParameter(command, "FromRefund", DbType.Boolean, BizActionObj.FromRefund);
                if (BizActionObj.BillType.HasValue)
                    dbServer.AddInParameter(command, "BillType", DbType.Int16, (Int16)BizActionObj.BillType);
                dbServer.AddInParameter(command, "UnitID", DbType.String, BizActionObj.UnitID);
                if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                    dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.FirstName);
                //dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(BizActionObj.FirstName));
                if (BizActionObj.MiddleName != null && BizActionObj.MiddleName.Length != 0)
                    dbServer.AddInParameter(command, "MiddleName", DbType.String, BizActionObj.MiddleName);
                //dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(BizActionObj.MiddleName));
                if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.LastName);
                //dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(BizActionObj.LastName));
                if (BizActionObj.FromDate != null)
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate.Value.Date.Date);
                if (BizActionObj.ToDate != null)
                {
                    if (BizActionObj.FromDate != null)
                    {
                        // if (BizActionObj.FromDate.Equals(BizActionObj.ToDate))
                        BizActionObj.ToDate = BizActionObj.ToDate.Value.Date.AddDays(1);
                    }
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate.Value.Date.Date);

                }
                // Added by Changdeo
                dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, BizActionObj.CostingDivisionID);  //Costing Divisions for Clinical & Pharmacy Billing
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "@IsConsumption", DbType.Boolean, BizActionObj.IsConsumption);
                //dbServer.AddInParameter(command,"ConcessionReasonId",DbType.Int64, BizActionObj.
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsBillVO>();
                    while (reader.Read())
                    {
                        clsBillVO objVO = new clsBillVO();

                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objVO.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        objVO.IsCancelled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCancelled"]));
                        objVO.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                        objVO.MRNO = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"]));
                        objVO.ContactNo = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo1"]));
                        objVO.BillType = (BillTypes)(Convert.ToInt16(DALHelper.HandleDBNull(reader["BillType"])));
                        objVO.Opd_Ipd_External = Convert.ToInt16(DALHelper.HandleDBNull(reader["Opd_Ipd_External"]));
                        objVO.Opd_Ipd_External_Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"]));
                        objVO.Opd_Ipd_External_UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_UnitId"]));
                        objVO.TotalBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalBillAmount"]));
                        objVO.TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalConcessionAmount"]));
                        objVO.NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"]));
                        //objVO.Opd_Ipd_External_No = (string)DALHelper.HandleDBNull(reader["OPDNO"]);
                        objVO.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        if (BizActionObj.IsIPDBillList == true)
                        {
                            objVO.AdmissionType = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionType"]));
                            objVO.IPDNO = Convert.ToString(DALHelper.HandleDBNull(reader["IPDNO"]));
                            objVO.ConcessionAuthorizedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConcessionAuthorizedBy"]));

                            objVO.TotalRefund = Convert.ToDouble(DALHelper.HandleDBNull(reader["RefundAmount"]));
                            Func<double, bool> myFunc = x => x != 0;
                            objVO.IsRefunded = myFunc(objVO.TotalRefund);
                        }
                        else
                        {
                            objVO.VisitTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitTypeID"]));
                            objVO.TotalRefund = Convert.ToDouble(DALHelper.HandleDBNull(reader["RefundAmount"]));
                            Func<double, bool> myFunc = x => x != 0;
                            objVO.IsRefunded = myFunc(objVO.TotalRefund);
                            objVO.Opd_Ipd_External_No = Convert.ToString(DALHelper.HandleDBNull(reader["OPDNO"]));
                            objVO.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                            //By Parmeshwar
                            objVO.ConcessionAuthorizedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConcessionAuthorizedBy"]));
                            //...............................................................

                            //objVO.ConcessiongivenBy = Convert.ToString(DALHelper.HandleDBNull(reader["ConBy"]));
                            //objVO.Naration = Convert.ToString(DALHelper.HandleDBNull(reader["naration"]));
                            objVO.PatientCategoryId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientCategoryId"]));  // Added hy Ashish Z. ondated 21122016
                            objVO.IsPackageConsumption = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackageConsumption"])); //Added By Bhushanp

                            objVO.PackageBillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageBillID"]));                         // For Package New Changes Added on 18062018
                            objVO.PackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageID"]));                                 // For Package New Changes Added on 18062018
                            objVO.PackageConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PackageConcessionAmount"]));    // For Package New Changes Added on 18062018

                            objVO.IsAdjustableBillDone = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAdjustableBillDone"]));     // For Package New Changes Added on 20062018
                            objVO.IsAllBillSettle = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAllBillSettle"]));               // For Package New Changes Added on 20062018
                            objVO.PackageSettleRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["PackageSettleRate"]));            // For Package New Changes Added on 20062018

                            objVO.LinkPatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LinkPatientID"]));
                            objVO.LinkPatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LinkPatientUnitID"]));


                        }

                        objVO.IsPackageServiceInclude = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackageServiceInclude"]));//Change By Bhushanp 25042017
                        objVO.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        objVO.FirstName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])));
                        objVO.MiddleName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"])));
                        objVO.LastName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])));
                        objVO.BalanceAmountSelf = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmountSelf"]));
                        objVO.SelfAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["SelfAmount"]));
                        objVO.BillPaymentType = (BillPaymentTypes)(Convert.ToInt16(DALHelper.HandleDBNull(reader["BillPaymentType"])));
                        //objVO.BalanceAmountSelf = objVO.TotalBillAmount - objVO.PaidAmountSelf;
                        objVO.GrossDiscountPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["GrossDiscountPercentage"]));
                        objVO.GrossDiscountReasonID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GrossDiscountReasonID"]));
                        objVO.IsComanyBillCancelled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCompanyBillCancelled"]));
                        objVO.PatientSourceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceId"]));
                        objVO.CompanyId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompanyId"]));
                        objVO.CostingDivisionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CostingDivisionID"]));

                        //***//
                        objVO.PatientCategoryId = (long)DALHelper.HandleDBNull(reader["PatientTypeID"]);
                        objVO.RelationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RelationID"]));
                        //------

                        //if (BizActionObj.IsIPDBillList == true)
                        //{
                        //    objVO.PatientCategoryId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientCategoryId"]));
                        //    objVO.TariffId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffId"]));
                        //}

                        if (BizActionObj.IsIPDBillList != true)
                        {
                            objVO.PaymentModeDetails = Convert.ToString(DALHelper.HandleDBNull(reader["Payemtmode"]));
                            // Added BY CDS 
                            if ((Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"])) == false && objVO.BillType != (BillTypes)(2)))
                            {
                                objVO.BalanceAmountSelf = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"]));
                                //objVO.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                            }
                            else  //Added by AJ Date 6/2/2017
                            {
                                objVO.BalanceAmountSelf = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmountSelf"]));
                            }
                            // Added BY CDS 
                        }

                        //By Anjali.....................
                        //By Anjali..............................................................
                        objVO.IsRequestSend = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRequestSend"]));
                        if (BizActionObj.IsRequest == true)
                        {
                            objVO.IsApproved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsApproved"]));
                            objVO.LevelID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LevelID"]));
                            objVO.LevelDescription = Convert.ToString(DALHelper.HandleDBNull(reader["LevelDescription"]));
                            objVO.ApprovalRemark = Convert.ToString(DALHelper.HandleDBNull(reader["ApprovalRemark"]));
                            objVO.RequestTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RequestTypeID"]));
                            objVO.AuthorityPerson = Convert.ToString(DALHelper.HandleDBNull(reader["AuthorityPerson"]));
                        }

                        //.......................

                        objVO.IsInvoiceGenerated = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsInvoiceGenerated"]));



                        ////Added by priyanka- for refund services
                        //objVO.TotalRefund = (double)DALHelper.HandleDBNull(reader["RefundAmount"]);
                        objVO.ConcessionReasonId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConcessionReasonId"]));
                        objVO.ConcessionRemark = Convert.ToString(DALHelper.HandleDBNull(reader["ConcessionRemark"]));

                        if (BizActionObj.Opd_Ipd_External == 1)
                        {
                            objVO.ISDischarged = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ISDischarged"]));
                        }

                        BizActionObj.List.Add(objVO);
                        //objVO.PaidAmountSelf
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


        public override IValueObject GetTotalBillAccountsLedgers(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetTotalBillAccountsLedgersVO BizActionObj = valueObject as clsGetTotalBillAccountsLedgersVO;


            DbDataReader reader; DbDataReader _reader; DbDataReader _reader1; DbDataReader _reader2; DbDataReader reader2;
            List<clsLedgerVO> LedgerAccountList = new List<clsLedgerVO>();
            List<clsLedgerVO> BillIDsList = new List<clsLedgerVO>();
            try
            {
                #region OPD
                #region OPD Self
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAccountInterface");
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizActionObj.Details.UnitID);
                dbServer.AddInParameter(command, "Date", DbType.Date, BizActionObj.Details.ExportDate);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.Details.OPDBillsLedgerAccount == null)
                        BizActionObj.Details.OPDBillsLedgerAccount = new List<clsLedgerVO>();

                    while (reader.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(reader["Narration"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(reader["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Receipt";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.OPDBillsLedgerAccount.Add(objCashVO);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(reader["Narration"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(reader["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Receipt";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.OPDBillsLedgerAccount.Add(objCashVO);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(reader["Narration"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(reader["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Receipt";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.OPDBillsLedgerAccount.Add(objCashVO);
                    }
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(reader["Narration"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalConcession"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Receipt";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.OPDBillsLedgerAccount.Add(objCashVO);
                    }
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(reader["Narration"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalConcession"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Receipt";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.OPDBillsLedgerAccount.Add(objCashVO);
                    }
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(reader["BillIDs"]));
                        objCashVO.DR = 0;
                        objCashVO.CR = Convert.ToDouble(DALHelper.HandleDBNull(reader["Amount"]));

                        if (BizActionObj.Details.OPDBillsLedgerAccount != null)
                        {
                            if (BizActionObj.Details.OPDBillsLedgerAccount.SingleOrDefault(S => S.RowID.Equals(1)) == null)
                                objCashVO.RowID = 1;
                            else
                                objCashVO.RowID = 0;
                        }
                        else
                            objCashVO.RowID = 1;
                        objCashVO.VoucherType = "Receipt";
                        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(reader["TransactionType"]));
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(reader["GPVoucherType"]));
                        objCashVO.PatientType = Convert.ToString(DALHelper.HandleDBNull(reader["PatientType"]));
                        objCashVO.Sponsor = Convert.ToString(DALHelper.HandleDBNull(reader["SponsorType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(reader["VoucherMode"]));
                        objCashVO.PatientNo = Convert.ToString(DALHelper.HandleDBNull(reader["PatientNo"]));
                        objCashVO.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(reader["Pat_Comp_Name"]));
                        objCashVO.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(reader["TransactionGroup"]));
                        objCashVO.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionId"]));
                        objCashVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"]));
                        objCashVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(reader["VendorID"]));
                        objCashVO.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader["PurchaseInvoiceNo"]));
                        objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(reader["TransactionNo"]));
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCredit"]));
                        if (objCashVO.CR > 0)
                            BizActionObj.Details.OPDBillsLedgerAccount.Add(objCashVO);
                    }
                }
                reader.Close();


                #endregion

                #region "OPD Credit"
                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_GetOPDCreditBillsLedgerAccount");
                dbServer.AddInParameter(command1, "UnitId", DbType.Int64, BizActionObj.Details.UnitID);
                dbServer.AddInParameter(command1, "Date", DbType.DateTime, BizActionObj.Details.ExportDate);
                _reader = (DbDataReader)dbServer.ExecuteReader(command1);

                LedgerAccountList = new List<clsLedgerVO>();
                BillIDsList = new List<clsLedgerVO>();

                if (BizActionObj.Details.OPDSelfCreditBillAccount == null)
                    BizActionObj.Details.OPDSelfCreditBillAccount = new List<clsLedgerVO>();

                if (_reader.HasRows)
                {
                    while (_reader.Read())
                    {
                        clsLedgerVO objCashCon = new clsLedgerVO();
                        objCashCon.RowID = 1;
                        //objCashCon.ID = Convert.ToInt64(DALHelper.HandleDBNull(_reader["ID"]));
                        objCashCon.LedgerName = Convert.ToString(DALHelper.HandleDBNull(_reader["LedgerName"]));
                        objCashCon.Narration = Convert.ToString(DALHelper.HandleDBNull(_reader["Narration"]));
                        objCashCon.DR = Convert.ToDouble(DALHelper.HandleDBNull(_reader["Amount"]));
                        //objCashCon.Reference = Convert.ToString(DALHelper.HandleDBNull(_reader["BillNo"]));
                        objCashCon.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(_reader["IsCredit"]));
                        objCashCon.CR = 0;
                        objCashCon.VoucherType = "Sales";
                        if (objCashCon.DR > 0)
                            BizActionObj.Details.OPDSelfCreditBillAccount.Add(objCashCon);
                    }

                }
                _reader.NextResult();

                if (_reader.HasRows)
                {
                    while (_reader.Read())
                    {
                        clsLedgerVO objCashCon = new clsLedgerVO();
                        //objCashCon.ID = Convert.ToInt64(DALHelper.HandleDBNull(_reader["ID"]));
                        objCashCon.LedgerName = Convert.ToString(DALHelper.HandleDBNull(_reader["LedgerName"]));
                        objCashCon.Narration = Convert.ToString(DALHelper.HandleDBNull(_reader["Narration"]));
                        objCashCon.DR = Convert.ToDouble(DALHelper.HandleDBNull(_reader["Amount"]));
                        //objCashCon.Reference = Convert.ToString(DALHelper.HandleDBNull(_reader["BillNo"]));
                        objCashCon.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(_reader["IsCredit"]));
                        objCashCon.CR = 0;
                        objCashCon.VoucherType = "Sales";
                        if (objCashCon.DR > 0)
                            BizActionObj.Details.OPDSelfCreditBillAccount.Add(objCashCon);
                    }

                }
                _reader.NextResult();

                if (_reader.HasRows)
                {
                    while (_reader.Read())
                    {
                        clsLedgerVO objCashCon = new clsLedgerVO();
                        //objCashCon.ID = Convert.ToInt64(DALHelper.HandleDBNull(_reader["ID"]));
                        objCashCon.LedgerName = Convert.ToString(DALHelper.HandleDBNull(_reader["LedgerName"]));
                        objCashCon.Narration = Convert.ToString(DALHelper.HandleDBNull(_reader["Narration"]));
                        objCashCon.DR = Convert.ToDouble(DALHelper.HandleDBNull(_reader["Amount"]));
                        // objCashCon.Reference = Convert.ToString(DALHelper.HandleDBNull(_reader["BillNo"]));
                        objCashCon.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(_reader["IsCredit"]));
                        objCashCon.CR = 0;
                        objCashCon.VoucherType = "Sales";
                        if (objCashCon.DR > 0)
                            BizActionObj.Details.OPDSelfCreditBillAccount.Add(objCashCon);
                    }

                }
                _reader.NextResult();
                if (_reader.HasRows)
                {
                    while (_reader.Read())
                    {
                        clsLedgerVO objCashCon = new clsLedgerVO();
                        //  objCashCon.ID = Convert.ToInt64(DALHelper.HandleDBNull(_reader["ID"]));
                        objCashCon.LedgerName = Convert.ToString(DALHelper.HandleDBNull(_reader["LedgerName"]));
                        objCashCon.Narration = Convert.ToString(DALHelper.HandleDBNull(_reader["Narration"]));
                        objCashCon.DR = Convert.ToDouble(DALHelper.HandleDBNull(_reader["Amount"]));
                        // objCashCon.Reference = Convert.ToString(DALHelper.HandleDBNull(_reader["BillNo"]));
                        objCashCon.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(_reader["IsCredit"]));
                        objCashCon.CR = 0;
                        objCashCon.VoucherType = "Sales";
                        if (objCashCon.DR > 0)
                            BizActionObj.Details.OPDSelfCreditBillAccount.Add(objCashCon);
                    }

                }
                _reader.NextResult();

                if (_reader.HasRows)
                {
                    while (_reader.Read())
                    {
                        clsLedgerVO _objCashVO = new clsLedgerVO();
                        // _objCashVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(_reader["ID"]));
                        _objCashVO.LedgerName = (string)DALHelper.HandleDBNull(_reader["LedgerName"]);
                        _objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(_reader["Narration"]));
                        //  _objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(_reader["BillNo"]));
                        _objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(_reader["Amount"]));
                        _objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(_reader["IsCredit"]));
                        _objCashVO.CR = 0;
                        _objCashVO.VoucherType = "Sales";
                        if (_objCashVO.DR > 0)
                            BizActionObj.Details.OPDSelfCreditBillAccount.Add(_objCashVO);
                    }
                }
                _reader.NextResult();

                if (_reader.HasRows)
                {
                    while (_reader.Read())
                    {
                        clsLedgerVO _objCashVO = new clsLedgerVO();
                        // _objCashVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(_reader["ID"]));
                        _objCashVO.LedgerName = (string)DALHelper.HandleDBNull(_reader["LedgerName"]);
                        _objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(_reader["Narration"]));
                        //  _objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(_reader["BillNo"]));
                        _objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(_reader["Amount"]));
                        _objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(_reader["IsCredit"]));
                        _objCashVO.CR = 0;
                        _objCashVO.VoucherType = "Sales";
                        if (_objCashVO.DR > 0)
                            BizActionObj.Details.OPDSelfCreditBillAccount.Add(_objCashVO);
                    }
                }
                _reader.NextResult();

                if (_reader.HasRows)
                {
                    while (_reader.Read())
                    {
                        clsLedgerVO objCashCon = new clsLedgerVO();
                        //objCashCon.ID = Convert.ToInt64(DALHelper.HandleDBNull(_reader["ID"]));
                        objCashCon.LedgerName = Convert.ToString(DALHelper.HandleDBNull(_reader["LedgerName"]));
                        if (BizActionObj.Details.OPDSelfCreditBillAccount != null)
                        {
                            if (BizActionObj.Details.OPDSelfCreditBillAccount.Where(S => S.RowID == 1).Any() == null)
                                objCashCon.RowID = 1;
                            else
                                objCashCon.RowID = 0;
                        }
                        else
                            objCashCon.RowID = 1;
                        objCashCon.Narration = Convert.ToString(DALHelper.HandleDBNull(_reader["Narration"]));
                        objCashCon.CR = Convert.ToDouble(DALHelper.HandleDBNull(_reader["Amount"]));
                        //objCashCon.Reference = Convert.ToString(DALHelper.HandleDBNull(_reader["BillNo"]));
                        objCashCon.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(_reader["IsCredit"]));
                        objCashCon.DR = 0;
                        objCashCon.VoucherType = "Sales";
                        if (objCashCon.CR > 0)
                            BizActionObj.Details.OPDSelfCreditBillAccount.Add(objCashCon);
                    }

                }

                _reader.Close();
                #endregion

                #region OPD Advance
                DbCommand commandOPDAdvance = dbServer.GetStoredProcCommand("CIMS_GetOPDAdvanceAmount");
                dbServer.AddInParameter(commandOPDAdvance, "UnitId", DbType.Int64, BizActionObj.Details.UnitID);
                dbServer.AddInParameter(commandOPDAdvance, "Date", DbType.DateTime, BizActionObj.Details.ExportDate);
                DbDataReader readerOPDAdvance = (DbDataReader)dbServer.ExecuteReader(commandOPDAdvance);

                LedgerAccountList = new List<clsLedgerVO>();
                BillIDsList = new List<clsLedgerVO>();
                if (readerOPDAdvance.HasRows)
                {
                    if (BizActionObj.Details.OPDAdvanceLedgerAccount == null)
                        BizActionObj.Details.OPDAdvanceLedgerAccount = new List<clsLedgerVO>();
                    while (readerOPDAdvance.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(readerOPDAdvance["ID"]));
                        objCashVO.LedgerName = (string)DALHelper.HandleDBNull(readerOPDAdvance["LedgerName"]);
                        objCashVO.Narration = (string)DALHelper.HandleDBNull(readerOPDAdvance["Narration"]);
                        objCashVO.DR = 0;
                        objCashVO.CR = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDAdvance["Amount"]));
                        objCashVO.VoucherType = "Receipt";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerOPDAdvance["IsCredit"]));
                        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvance["TransactionType"]));
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvance["GPVoucherType"]));
                        objCashVO.PatientType = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvance["PatientType"]));
                        objCashVO.Sponsor = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvance["SponsorType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvance["VoucherMode"]));
                        objCashVO.PatientNo = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvance["PatientNo"]));
                        objCashVO.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvance["Pat_Comp_Name"]));
                        objCashVO.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvance["TransactionGroup"]));
                        objCashVO.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(readerOPDAdvance["TransactionId"]));
                        objCashVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvance["Remark"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDAdvance["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDAdvance["TotalAmount"]));
                        objCashVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDAdvance["NetAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(readerOPDAdvance["VendorID"]));
                        objCashVO.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvance["PurchaseInvoiceNo"]));
                        objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvance["TransactionNo"]));
                        objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvance["ID"]));
                        LedgerAccountList.Add(objCashVO);
                        BillIDsList.Add(objCashVO);
                    }
                    readerOPDAdvance.NextResult();

                    while (readerOPDAdvance.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(readerOPDAdvance["ID"]));
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvance["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvance["Narration"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDAdvance["Amount"]));
                        objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvance["ID"]));
                        objCashVO.VoucherType = "Receipt";
                        objCashVO.CR = 0;
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerOPDAdvance["IsCredit"]));
                        LedgerAccountList.Add(objCashVO);
                    }

                    foreach (var item in BillIDsList)
                    {
                        foreach (var _item in LedgerAccountList.Where(S => S.ID.Equals(item.ID)).ToList())
                        {
                            item.RowID = 1;
                            BizActionObj.Details.OPDAdvanceLedgerAccount.Add(_item);
                        }
                    }

                    readerOPDAdvance.Close();
                }

                #endregion

                #region OPD Advance Refund
                DbCommand commandOPDAdvanceRefund = dbServer.GetStoredProcCommand("CIMS_GetOPDAdvanceRefundDetails");

                dbServer.AddInParameter(commandOPDAdvanceRefund, "UnitId", DbType.Int64, BizActionObj.Details.UnitID);
                dbServer.AddInParameter(commandOPDAdvanceRefund, "Date", DbType.DateTime, BizActionObj.Details.ExportDate);

                DbDataReader readerOPDAdvanceRefund = (DbDataReader)dbServer.ExecuteReader(commandOPDAdvanceRefund);
                LedgerAccountList = new List<clsLedgerVO>();
                BillIDsList = new List<clsLedgerVO>();
                if (readerOPDAdvanceRefund.HasRows)
                {
                    if (BizActionObj.Details.OPDAdvanceRefundLedgerAccount == null)
                        BizActionObj.Details.OPDAdvanceRefundLedgerAccount = new List<clsLedgerVO>();
                    while (readerOPDAdvanceRefund.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(readerOPDAdvanceRefund["ID"]));
                        objCashVO.LedgerName = (string)DALHelper.HandleDBNull(readerOPDAdvanceRefund["LedgerName"]);
                        objCashVO.Narration = (string)DALHelper.HandleDBNull(readerOPDAdvanceRefund["Narration"]);
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDAdvanceRefund["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Journal";
                        objCashVO.RowID = 1;
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerOPDAdvanceRefund["IsCredit"]));
                        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvanceRefund["TransactionType"]));
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvanceRefund["GPVoucherType"]));
                        objCashVO.PatientType = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvanceRefund["PatientType"]));
                        objCashVO.Sponsor = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvanceRefund["SponsorType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvanceRefund["VoucherMode"]));
                        objCashVO.PatientNo = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvanceRefund["PatientNo"]));
                        objCashVO.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvanceRefund["Pat_Comp_Name"]));
                        objCashVO.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvanceRefund["TransactionGroup"]));
                        objCashVO.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(readerOPDAdvanceRefund["TransactionId"]));
                        objCashVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvanceRefund["Remark"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDAdvanceRefund["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDAdvanceRefund["TotalAmount"]));
                        objCashVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDAdvanceRefund["NetAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(readerOPDAdvanceRefund["VendorID"]));
                        objCashVO.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvanceRefund["PurchaseInvoiceNo"]));
                        objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvanceRefund["TransactionNo"]));
                        objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvanceRefund["ID"]));
                        LedgerAccountList.Add(objCashVO);
                        BillIDsList.Add(objCashVO);
                    }
                    readerOPDAdvanceRefund.NextResult();

                    while (readerOPDAdvanceRefund.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(readerOPDAdvanceRefund["ID"]));
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvanceRefund["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvanceRefund["Narration"]));
                        objCashVO.DR = 0;
                        objCashVO.CR = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDAdvanceRefund["Amount"]));
                        objCashVO.VoucherType = "Journal";
                        objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(readerOPDAdvanceRefund["ID"]));
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerOPDAdvanceRefund["IsCredit"]));
                        LedgerAccountList.Add(objCashVO);
                    }

                    foreach (var item in BillIDsList)
                    {
                        foreach (var _item in LedgerAccountList.Where(S => S.ID.Equals(item.ID)).ToList())
                        {
                            //item.RowID = 1;
                            BizActionObj.Details.OPDAdvanceRefundLedgerAccount.Add(_item);
                        }
                    }
                    readerOPDAdvanceRefund.Close();
                }

                #endregion

                #region OPD RefundBill
                DbCommand commandOPDRefundBill = dbServer.GetStoredProcCommand("CIMS_GetOPDBillRefundDetails");
                dbServer.AddInParameter(commandOPDRefundBill, "UnitId", DbType.Int64, BizActionObj.Details.UnitID);
                dbServer.AddInParameter(commandOPDRefundBill, "Date", DbType.DateTime, BizActionObj.Details.ExportDate);
                DbDataReader readerOPDRefundBill = (DbDataReader)dbServer.ExecuteReader(commandOPDRefundBill);
                if (readerOPDRefundBill.HasRows)
                {
                    if (BizActionObj.Details.OPDRefundBillLedgerAccount == null)
                        BizActionObj.Details.OPDRefundBillLedgerAccount = new List<clsLedgerVO>();
                    while (readerOPDRefundBill.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = (string)DALHelper.HandleDBNull(readerOPDRefundBill["Company"]);
                        objCashVO.Narration = (string)DALHelper.HandleDBNull(readerOPDRefundBill["Narration"]);
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDRefundBill["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.RowID = 1;
                        objCashVO.VoucherType = "Payment";
                        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(readerOPDRefundBill["TransactionType"]));
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(readerOPDRefundBill["GPVoucherType"]));
                        objCashVO.PatientType = Convert.ToString(DALHelper.HandleDBNull(readerOPDRefundBill["PatientType"]));
                        objCashVO.Sponsor = Convert.ToString(DALHelper.HandleDBNull(readerOPDRefundBill["SponsorType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(readerOPDRefundBill["VoucherMode"]));
                        objCashVO.PatientNo = Convert.ToString(DALHelper.HandleDBNull(readerOPDRefundBill["PatientNo"]));
                        objCashVO.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(readerOPDRefundBill["Pat_Comp_Name"]));
                        objCashVO.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(readerOPDRefundBill["TransactionGroup"]));
                        objCashVO.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(readerOPDRefundBill["TransactionId"]));
                        objCashVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(readerOPDRefundBill["Remark"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDRefundBill["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDRefundBill["TotalAmount"]));
                        objCashVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDRefundBill["NetAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(readerOPDRefundBill["VendorID"]));
                        objCashVO.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(readerOPDRefundBill["PurchaseInvoiceNo"]));
                        objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(readerOPDRefundBill["TransactionNo"]));
                        objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(readerOPDRefundBill["ID"]));
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerOPDRefundBill["IsCredit"]));
                        BizActionObj.Details.OPDRefundBillLedgerAccount.Add(objCashVO);
                    }
                    readerOPDRefundBill.NextResult();

                    while (readerOPDRefundBill.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerOPDRefundBill["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerOPDRefundBill["Narration"]));
                        if (BizActionObj.Details.OPDRefundBillLedgerAccount != null)
                        {
                            if (BizActionObj.Details.OPDRefundBillLedgerAccount.Where(z => z.RowID == 1).Any())
                                objCashVO.RowID = 1;
                            else
                                objCashVO.RowID = 0;
                        }
                        else
                            objCashVO.RowID = 1;
                        objCashVO.DR = 0;
                        objCashVO.VoucherType = "Payment";
                        objCashVO.CR = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDRefundBill["Amount"]));
                        objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(readerOPDRefundBill["ID"]));
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerOPDRefundBill["IsCredit"]));
                        if (objCashVO.CR > 0)
                            BizActionObj.Details.OPDRefundBillLedgerAccount.Add(objCashVO);
                    }
                    readerOPDRefundBill.Close();
                }

                #endregion

                #region OPD Self Receipt
                DbCommand commandOPDReceipt = dbServer.GetStoredProcCommand("CIMS_GetOPDSelfReceiptLedgerAccount");
                dbServer.AddInParameter(commandOPDReceipt, "UnitId", DbType.Int64, BizActionObj.Details.UnitID);
                dbServer.AddInParameter(commandOPDReceipt, "Date", DbType.DateTime, BizActionObj.Details.ExportDate);
                DbDataReader readerOPDReceipt = (DbDataReader)dbServer.ExecuteReader(commandOPDReceipt);

                LedgerAccountList = new List<clsLedgerVO>();
                BillIDsList = new List<clsLedgerVO>();
                if (BizActionObj.Details.OPDReceiptLedgerAccount == null)
                    BizActionObj.Details.OPDReceiptLedgerAccount = new List<clsLedgerVO>();
                if (readerOPDReceipt.HasRows)
                {
                    while (readerOPDReceipt.Read())
                    {

                        clsLedgerVO objCashVO = new clsLedgerVO();
                        // objCashVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(readerOPDReceipt["ID"]));
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["Narration"]));
                        // objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["BillNo"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDReceipt["Amount"]));
                        objCashVO.CR = 0;
                        //objCashVO.RowID = 1;
                        objCashVO.VoucherType = "Receipt";
                        //objCashVO.BalanceAmount = Convert.ToDouble(DALHelper.HandleDBNull(_reader["BalanceAmountSelf"]));
                        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["TransactionType"]));
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["GPVoucherType"]));
                        objCashVO.PatientType = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["PatientType"]));
                        objCashVO.Sponsor = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["SponsorType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["VoucherMode"]));
                        // objCashVO.PatientNo = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["PatientNo"]));
                        // objCashVO.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["Pat_Comp_Name"]));
                        objCashVO.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["TransactionGroup"]));
                        //  objCashVO.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(readerOPDReceipt["TransactionId"]));
                        //  objCashVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["Remark"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDReceipt["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDReceipt["TotalAmount"]));
                        objCashVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDReceipt["NetAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(readerOPDReceipt["VendorID"]));
                        objCashVO.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["PurchaseInvoiceNo"]));
                        //  objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["TransactionNo"]));
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerOPDReceipt["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.OPDReceiptLedgerAccount.Add(objCashVO);
                        //LedgerAccountList.Add(objCashVO);
                    }
                }
                readerOPDReceipt.NextResult();
                if (readerOPDReceipt.HasRows)
                {
                    while (readerOPDReceipt.Read())
                    {

                        clsLedgerVO objCashVO = new clsLedgerVO();
                        //   objCashVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(readerOPDReceipt["ID"]));
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["Narration"]));
                        //   objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["BillNo"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDReceipt["Amount"]));
                        objCashVO.CR = 0;
                        //objCashVO.RowID = 1;
                        objCashVO.VoucherType = "Receipt";
                        //objCashVO.BalanceAmount = Convert.ToDouble(DALHelper.HandleDBNull(_reader["BalanceAmountSelf"]));
                        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["TransactionType"]));
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["GPVoucherType"]));
                        objCashVO.PatientType = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["PatientType"]));
                        objCashVO.Sponsor = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["SponsorType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["VoucherMode"]));
                        //objCashVO.PatientNo = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["PatientNo"]));
                        //objCashVO.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["Pat_Comp_Name"]));
                        objCashVO.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["TransactionGroup"]));
                        //objCashVO.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(readerOPDReceipt["TransactionId"]));
                        //objCashVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["Remark"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDReceipt["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDReceipt["TotalAmount"]));
                        objCashVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDReceipt["NetAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(readerOPDReceipt["VendorID"]));
                        objCashVO.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["PurchaseInvoiceNo"]));
                        //objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["TransactionNo"]));
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerOPDReceipt["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.OPDReceiptLedgerAccount.Add(objCashVO);
                        //LedgerAccountList.Add(objCashVO);
                    }
                }
                readerOPDReceipt.NextResult();
                if (readerOPDReceipt.HasRows)
                {
                    while (readerOPDReceipt.Read())
                    {

                        clsLedgerVO objCashVO = new clsLedgerVO();
                        //objCashVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(readerOPDReceipt["ID"]));
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["Narration"]));
                        //objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["BillNo"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDReceipt["Amount"]));
                        objCashVO.CR = 0;
                        //objCashVO.RowID = 1;
                        objCashVO.VoucherType = "Receipt";
                        //objCashVO.BalanceAmount = Convert.ToDouble(DALHelper.HandleDBNull(_reader["BalanceAmountSelf"]));
                        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["TransactionType"]));
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["GPVoucherType"]));
                        objCashVO.PatientType = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["PatientType"]));
                        objCashVO.Sponsor = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["SponsorType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["VoucherMode"]));
                        //objCashVO.PatientNo = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["PatientNo"]));
                        //objCashVO.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["Pat_Comp_Name"]));
                        objCashVO.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["TransactionGroup"]));
                        //objCashVO.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(readerOPDReceipt["TransactionId"]));
                        objCashVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["Remark"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDReceipt["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDReceipt["TotalAmount"]));
                        objCashVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDReceipt["NetAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(readerOPDReceipt["VendorID"]));
                        objCashVO.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["PurchaseInvoiceNo"]));
                        // objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["TransactionNo"]));
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerOPDReceipt["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.OPDReceiptLedgerAccount.Add(objCashVO);
                        //LedgerAccountList.Add(objCashVO);
                    }
                }
                readerOPDReceipt.NextResult();
                if (readerOPDReceipt.HasRows)
                {
                    while (readerOPDReceipt.Read())
                    {

                        clsLedgerVO objCashVO = new clsLedgerVO();
                        //  objCashVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(readerOPDReceipt["ID"]));
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["Narration"]));
                        //  objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["BillNo"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDReceipt["Amount"]));
                        objCashVO.CR = 0;
                        //objCashVO.RowID = 1;
                        objCashVO.VoucherType = "Receipt";
                        //objCashVO.BalanceAmount = Convert.ToDouble(DALHelper.HandleDBNull(_reader["BalanceAmountSelf"]));
                        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["TransactionType"]));
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["GPVoucherType"]));
                        objCashVO.PatientType = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["PatientType"]));
                        objCashVO.Sponsor = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["SponsorType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["VoucherMode"]));
                        // objCashVO.PatientNo = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["PatientNo"]));
                        // objCashVO.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["Pat_Comp_Name"]));
                        objCashVO.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["TransactionGroup"]));
                        //  objCashVO.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(readerOPDReceipt["TransactionId"]));
                        // objCashVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["Remark"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDReceipt["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDReceipt["TotalAmount"]));
                        objCashVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDReceipt["NetAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(readerOPDReceipt["VendorID"]));
                        objCashVO.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["PurchaseInvoiceNo"]));
                        // objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["TransactionNo"]));
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerOPDReceipt["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.OPDReceiptLedgerAccount.Add(objCashVO);
                        ///LedgerAccountList.Add(objCashVO);
                    }
                }
                readerOPDReceipt.NextResult();
                if (readerOPDReceipt.HasRows)
                {
                    while (readerOPDReceipt.Read())
                    {

                        clsLedgerVO objCashVO = new clsLedgerVO();
                        //objCashVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(readerOPDReceipt["ID"]));
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["Narration"]));
                        //   objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["BillNo"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDReceipt["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Receipt";
                        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["TransactionType"]));
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["GPVoucherType"]));
                        objCashVO.PatientType = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["PatientType"]));
                        objCashVO.Sponsor = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["SponsorType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["VoucherMode"]));
                        // objCashVO.PatientNo = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["PatientNo"]));
                        // objCashVO.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["Pat_Comp_Name"]));
                        objCashVO.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["TransactionGroup"]));
                        //  objCashVO.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(readerOPDReceipt["TransactionId"]));
                        //  objCashVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["Remark"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDReceipt["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDReceipt["TotalAmount"]));
                        objCashVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDReceipt["NetAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(readerOPDReceipt["VendorID"]));
                        objCashVO.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["PurchaseInvoiceNo"]));
                        //   objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["TransactionNo"]));
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerOPDReceipt["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.OPDReceiptLedgerAccount.Add(objCashVO);
                    }
                }
                readerOPDReceipt.NextResult();
                if (readerOPDReceipt.HasRows)
                {
                    while (readerOPDReceipt.Read())
                    {

                        clsLedgerVO objCashVO = new clsLedgerVO();
                        //  objCashVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(readerOPDReceipt["ID"]));
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["Narration"]));

                        if (BizActionObj.Details.OPDReceiptLedgerAccount != null)
                        {
                            if (BizActionObj.Details.OPDReceiptLedgerAccount.Where(z => z.RowID == 1).Any())
                                objCashVO.RowID = 1;
                            else
                                objCashVO.RowID = 0;
                        }
                        else
                            objCashVO.RowID = 1;
                        //   objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["BillNo"]));
                        objCashVO.CR = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDReceipt["Amount"]));
                        objCashVO.DR = 0;
                        objCashVO.VoucherType = "Receipt";
                        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["TransactionType"]));
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["GPVoucherType"]));
                        objCashVO.PatientType = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["PatientType"]));
                        objCashVO.Sponsor = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["SponsorType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["VoucherMode"]));
                        // objCashVO.PatientNo = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["PatientNo"]));
                        // objCashVO.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["Pat_Comp_Name"]));
                        objCashVO.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["TransactionGroup"]));
                        //  objCashVO.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(readerOPDReceipt["TransactionId"]));
                        //  objCashVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["Remark"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDReceipt["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDReceipt["TotalAmount"]));
                        objCashVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerOPDReceipt["NetAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(readerOPDReceipt["VendorID"]));
                        objCashVO.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["PurchaseInvoiceNo"]));
                        // objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(readerOPDReceipt["TransactionNo"]));
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerOPDReceipt["IsCredit"]));
                        if (objCashVO.CR > 0)
                            BizActionObj.Details.OPDReceiptLedgerAccount.Add(objCashVO);
                    }
                }

                readerOPDReceipt.Close();




                #endregion

                #region "OPD Company Credit"
                DbCommand _command1 = dbServer.GetStoredProcCommand("CIMS_GetOPDCompanyBillsLedgerAccount");
                dbServer.AddInParameter(_command1, "UnitId", DbType.Int64, BizActionObj.Details.UnitID);
                dbServer.AddInParameter(_command1, "Date", DbType.DateTime, BizActionObj.Details.ExportDate);
                _reader1 = (DbDataReader)dbServer.ExecuteReader(_command1);
                LedgerAccountList = new List<clsLedgerVO>();
                BillIDsList = new List<clsLedgerVO>();
                if (_reader1.HasRows)
                {
                    if (BizActionObj.Details.OPDCompanyCreditBillAccount == null)
                        BizActionObj.Details.OPDCompanyCreditBillAccount = new List<clsLedgerVO>();
                    while (_reader1.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(_reader1["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(_reader1["Narration"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(_reader1["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Sales";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(_reader1["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.OPDCompanyCreditBillAccount.Add(objCashVO);
                    }
                }
                _reader1.NextResult();
                if (_reader1.HasRows)
                {
                    while (_reader1.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(_reader1["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(_reader1["Narration"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(_reader1["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Sales";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(_reader1["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.OPDCompanyCreditBillAccount.Add(objCashVO);
                    }
                }
                _reader1.NextResult();
                if (_reader1.HasRows)
                {
                    while (_reader1.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(_reader1["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(_reader1["Narration"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(_reader1["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Sales";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(_reader1["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.OPDCompanyCreditBillAccount.Add(objCashVO);
                    }
                }
                _reader1.NextResult();
                if (_reader1.HasRows)
                {
                    while (_reader1.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(_reader1["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(_reader1["Narration"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(_reader1["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Sales";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(_reader1["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.OPDCompanyCreditBillAccount.Add(objCashVO);
                    }
                }
                _reader1.NextResult();
                if (_reader1.HasRows)
                {
                    while (_reader1.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(_reader1["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(_reader1["Narration"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(_reader1["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Sales";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(_reader1["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.OPDCompanyCreditBillAccount.Add(objCashVO);
                    }
                }
                _reader1.NextResult();
                if (_reader1.HasRows)
                {
                    while (_reader1.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(_reader1["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(_reader1["Narration"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(_reader1["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Sales";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(_reader1["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.OPDCompanyCreditBillAccount.Add(objCashVO);
                    }
                }
                _reader1.NextResult();
                if (_reader1.HasRows)
                {
                    while (_reader1.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(_reader1["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(_reader1["Narration"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(_reader1["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Sales";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(_reader1["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.OPDCompanyCreditBillAccount.Add(objCashVO);
                    }
                }
                _reader1.NextResult();
                if (_reader1.HasRows)
                {
                    while (_reader1.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(_reader1["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(_reader1["BillIDs"]));
                        objCashVO.DR = 0;
                        objCashVO.CR = Convert.ToDouble(DALHelper.HandleDBNull(_reader1["Amount"]));

                        if (BizActionObj.Details.OPDCompanyCreditBillAccount != null)
                        {
                            if (BizActionObj.Details.OPDCompanyCreditBillAccount.SingleOrDefault(S => S.RowID.Equals(1)) == null)
                                objCashVO.RowID = 1;
                            else
                                objCashVO.RowID = 0;
                        }
                        else
                            objCashVO.RowID = 1;
                        objCashVO.VoucherType = "Sales";
                        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(_reader1["TransactionType"]));
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(_reader1["GPVoucherType"]));
                        objCashVO.PatientType = Convert.ToString(DALHelper.HandleDBNull(_reader1["PatientType"]));
                        objCashVO.Sponsor = Convert.ToString(DALHelper.HandleDBNull(_reader1["SponsorType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(_reader1["VoucherMode"]));
                        objCashVO.PatientNo = Convert.ToString(DALHelper.HandleDBNull(_reader1["PatientNo"]));
                        objCashVO.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(_reader1["Pat_Comp_Name"]));
                        objCashVO.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(_reader1["TransactionGroup"]));
                        objCashVO.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(_reader1["TransactionId"]));
                        objCashVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(_reader1["Remark"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(_reader1["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(_reader1["TotalAmount"]));
                        objCashVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(_reader1["NetAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(_reader1["VendorID"]));
                        objCashVO.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(_reader1["PurchaseInvoiceNo"]));
                        objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(_reader1["TransactionNo"]));
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(_reader1["IsCredit"]));
                        if (objCashVO.CR > 0)
                            BizActionObj.Details.OPDCompanyCreditBillAccount.Add(objCashVO);
                    }

                }
                _reader1.Close();



                #endregion

                #region "OPD Company Receipt"

                DbCommand _commandReceipt = dbServer.GetStoredProcCommand("CIMS_GetOPDCompanyReceiptsLedgerAccount");
                dbServer.AddInParameter(_commandReceipt, "UnitId", DbType.Int64, BizActionObj.Details.UnitID);
                dbServer.AddInParameter(_commandReceipt, "Date", DbType.DateTime, BizActionObj.Details.ExportDate);

                DbDataReader _readerReceipt = (DbDataReader)dbServer.ExecuteReader(_commandReceipt);
                LedgerAccountList = new List<clsLedgerVO>();
                BillIDsList = new List<clsLedgerVO>();
                if (BizActionObj.Details.OPDCompanyReceiptBillAccount == null)
                    BizActionObj.Details.OPDCompanyReceiptBillAccount = new List<clsLedgerVO>();
                if (_readerReceipt.HasRows)
                {
                    while (_readerReceipt.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(_readerReceipt["ID"]));
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["Narration"]));
                        objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["BillNo"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(_readerReceipt["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Receipt";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(_readerReceipt["IsCredit"]));
                        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["TransactionType"]));
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["GPVoucherType"]));
                        objCashVO.PatientType = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["PatientType"]));
                        objCashVO.Sponsor = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["SponsorType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["VoucherMode"]));
                        objCashVO.PatientNo = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["PatientNo"]));
                        objCashVO.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["Pat_Comp_Name"]));
                        objCashVO.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["TransactionGroup"]));
                        objCashVO.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(_readerReceipt["TransactionId"]));
                        objCashVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["Remark"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(_readerReceipt["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(_readerReceipt["TotalAmount"]));
                        objCashVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(_readerReceipt["NetAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(_readerReceipt["VendorID"]));
                        objCashVO.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["PurchaseInvoiceNo"]));
                        objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["TransactionNo"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.OPDCompanyReceiptBillAccount.Add(objCashVO);
                    }
                }
                _readerReceipt.NextResult();

                if (_readerReceipt.HasRows)
                {
                    while (_readerReceipt.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(_readerReceipt["ID"]));
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["Narration"]));
                        objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["BillNo"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(_readerReceipt["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Receipt";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(_readerReceipt["IsCredit"]));
                        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["TransactionType"]));
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["GPVoucherType"]));
                        objCashVO.PatientType = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["PatientType"]));
                        objCashVO.Sponsor = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["SponsorType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["VoucherMode"]));
                        objCashVO.PatientNo = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["PatientNo"]));
                        objCashVO.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["Pat_Comp_Name"]));
                        objCashVO.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["TransactionGroup"]));
                        objCashVO.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(_readerReceipt["TransactionId"]));
                        objCashVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["Remark"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(_readerReceipt["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(_readerReceipt["TotalAmount"]));
                        objCashVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(_readerReceipt["NetAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(_readerReceipt["VendorID"]));
                        objCashVO.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["PurchaseInvoiceNo"]));
                        objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["TransactionNo"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.OPDCompanyReceiptBillAccount.Add(objCashVO);
                    }
                }
                _readerReceipt.NextResult();

                if (_readerReceipt.HasRows)
                {
                    while (_readerReceipt.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(_readerReceipt["ID"]));
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["Narration"]));
                        objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["BillNo"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(_readerReceipt["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Receipt";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(_readerReceipt["IsCredit"]));
                        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["TransactionType"]));
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["GPVoucherType"]));
                        objCashVO.PatientType = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["PatientType"]));
                        objCashVO.Sponsor = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["SponsorType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["VoucherMode"]));
                        objCashVO.PatientNo = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["PatientNo"]));
                        objCashVO.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["Pat_Comp_Name"]));
                        objCashVO.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["TransactionGroup"]));
                        objCashVO.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(_readerReceipt["TransactionId"]));
                        objCashVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["Remark"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(_readerReceipt["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(_readerReceipt["TotalAmount"]));
                        objCashVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(_readerReceipt["NetAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(_readerReceipt["VendorID"]));
                        objCashVO.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["PurchaseInvoiceNo"]));
                        objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["TransactionNo"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.OPDCompanyReceiptBillAccount.Add(objCashVO);
                    }
                }
                _readerReceipt.NextResult();
                if (_readerReceipt.HasRows)
                {
                    while (_readerReceipt.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(_readerReceipt["ID"]));
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["Narration"]));
                        objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["BillNo"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(_readerReceipt["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Receipt";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(_readerReceipt["IsCredit"]));
                        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["TransactionType"]));
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["GPVoucherType"]));
                        objCashVO.PatientType = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["PatientType"]));
                        objCashVO.Sponsor = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["SponsorType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["VoucherMode"]));
                        objCashVO.PatientNo = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["PatientNo"]));
                        objCashVO.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["Pat_Comp_Name"]));
                        objCashVO.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["TransactionGroup"]));
                        objCashVO.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(_readerReceipt["TransactionId"]));
                        objCashVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["Remark"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(_readerReceipt["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(_readerReceipt["TotalAmount"]));
                        objCashVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(_readerReceipt["NetAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(_readerReceipt["VendorID"]));
                        objCashVO.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["PurchaseInvoiceNo"]));
                        objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["TransactionNo"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.OPDCompanyReceiptBillAccount.Add(objCashVO);
                    }
                }
                _readerReceipt.NextResult();
                if (_readerReceipt.HasRows)
                {
                    while (_readerReceipt.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(_readerReceipt["ID"]));
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["Narration"]));
                        if (BizActionObj.Details.OPDCompanyReceiptBillAccount != null)
                        {
                            if (BizActionObj.Details.OPDCompanyReceiptBillAccount.SingleOrDefault(S => S.RowID.Equals(1)) == null)
                                objCashVO.RowID = 1;
                            else
                                objCashVO.RowID = 0;
                        }
                        else
                            objCashVO.RowID = 1;
                        objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["BillNo"]));
                        objCashVO.CR = Convert.ToDouble(DALHelper.HandleDBNull(_readerReceipt["Amount"]));
                        objCashVO.DR = 0;
                        objCashVO.VoucherType = "Receipt";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(_readerReceipt["IsCredit"]));
                        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["TransactionType"]));
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["GPVoucherType"]));
                        objCashVO.PatientType = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["PatientType"]));
                        objCashVO.Sponsor = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["SponsorType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["VoucherMode"]));
                        objCashVO.PatientNo = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["PatientNo"]));
                        objCashVO.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["Pat_Comp_Name"]));
                        objCashVO.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["TransactionGroup"]));
                        objCashVO.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(_readerReceipt["TransactionId"]));
                        objCashVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["Remark"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(_readerReceipt["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(_readerReceipt["TotalAmount"]));
                        objCashVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(_readerReceipt["NetAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(_readerReceipt["VendorID"]));
                        objCashVO.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["PurchaseInvoiceNo"]));
                        objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(_readerReceipt["TransactionNo"]));
                        if (objCashVO.CR > 0)
                            BizActionObj.Details.OPDCompanyReceiptBillAccount.Add(objCashVO);
                    }
                }
                _readerReceipt.Close();

                #endregion

                #endregion

                #region IPD
                #region IPD Self
                DbCommand commandIPDSelf = dbServer.GetStoredProcCommand("CIMS_GetSelfIPDBillForTallyInterface ");
                dbServer.AddInParameter(commandIPDSelf, "UnitId", DbType.Int64, BizActionObj.Details.UnitID);
                dbServer.AddInParameter(commandIPDSelf, "Date", DbType.Date, BizActionObj.Details.ExportDate);
                DbDataReader readerIPDSelf = (DbDataReader)dbServer.ExecuteReader(commandIPDSelf);
                if (readerIPDSelf.HasRows)
                {
                    if (BizActionObj.Details.IPDSelfBillAccount == null)
                        BizActionObj.Details.IPDSelfBillAccount = new List<clsLedgerVO>();

                    while (readerIPDSelf.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelf["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelf["Narration"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerIPDSelf["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Receipt";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerIPDSelf["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.IPDSelfBillAccount.Add(objCashVO);
                    }
                }
                readerIPDSelf.NextResult();
                if (readerIPDSelf.HasRows)
                {
                    while (readerIPDSelf.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelf["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelf["Narration"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerIPDSelf["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Receipt";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerIPDSelf["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.IPDSelfBillAccount.Add(objCashVO);
                    }
                }
                readerIPDSelf.NextResult();
                if (readerIPDSelf.HasRows)
                {
                    while (readerIPDSelf.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelf["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelf["Narration"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerIPDSelf["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Receipt";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerIPDSelf["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.IPDSelfBillAccount.Add(objCashVO);
                    }
                }
                readerIPDSelf.NextResult();

                if (readerIPDSelf.HasRows)
                {
                    while (readerIPDSelf.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelf["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelf["Narration"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerIPDSelf["TotalConcession"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Receipt";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerIPDSelf["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.IPDSelfBillAccount.Add(objCashVO);
                    }
                }
                readerIPDSelf.NextResult();

                if (readerIPDSelf.HasRows)
                {
                    while (readerIPDSelf.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelf["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelf["Narration"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerIPDSelf["TotalConcession"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Receipt";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerIPDSelf["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.IPDSelfBillAccount.Add(objCashVO);
                    }
                }
                readerIPDSelf.NextResult();

                if (readerIPDSelf.HasRows)
                {
                    while (readerIPDSelf.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelf["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelf["BillIDs"]));
                        objCashVO.DR = 0;
                        objCashVO.CR = Convert.ToDouble(DALHelper.HandleDBNull(readerIPDSelf["Amount"]));

                        if (BizActionObj.Details.IPDSelfBillAccount != null)
                        {
                            if (BizActionObj.Details.IPDSelfBillAccount.SingleOrDefault(S => S.RowID.Equals(1)) == null)
                                objCashVO.RowID = 1;
                            else
                                objCashVO.RowID = 0;
                        }
                        else
                            objCashVO.RowID = 1;
                        objCashVO.VoucherType = "Receipt";
                        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelf["TransactionType"]));
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelf["GPVoucherType"]));
                        objCashVO.PatientType = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelf["PatientType"]));
                        objCashVO.Sponsor = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelf["SponsorType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelf["VoucherMode"]));
                        objCashVO.PatientNo = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelf["PatientNo"]));
                        objCashVO.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelf["Pat_Comp_Name"]));
                        objCashVO.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelf["TransactionGroup"]));
                        objCashVO.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(readerIPDSelf["TransactionId"]));
                        objCashVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelf["Remark"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerIPDSelf["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerIPDSelf["TotalAmount"]));
                        objCashVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerIPDSelf["NetAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(readerIPDSelf["VendorID"]));
                        objCashVO.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelf["PurchaseInvoiceNo"]));
                        objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelf["TransactionNo"]));
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerIPDSelf["IsCredit"]));
                        if (objCashVO.CR > 0)
                            BizActionObj.Details.IPDSelfBillAccount.Add(objCashVO);
                    }
                }
                readerIPDSelf.Close();


                #endregion

                #region "IPD Credit"
                DbCommand commandIPDSelfCr = dbServer.GetStoredProcCommand("CIMS_GetCreditIPDBillForTallyInterface");
                dbServer.AddInParameter(commandIPDSelfCr, "UnitId", DbType.Int64, BizActionObj.Details.UnitID);
                dbServer.AddInParameter(commandIPDSelfCr, "Date", DbType.DateTime, BizActionObj.Details.ExportDate);
                DbDataReader readerIPDSelfCr = (DbDataReader)dbServer.ExecuteReader(commandIPDSelfCr);

                LedgerAccountList = new List<clsLedgerVO>();
                BillIDsList = new List<clsLedgerVO>();

                if (BizActionObj.Details.IPDSelfCreditForTallyInterface == null)
                    BizActionObj.Details.IPDSelfCreditForTallyInterface = new List<clsLedgerVO>();

                if (readerIPDSelfCr.HasRows)
                {
                    while (readerIPDSelfCr.Read())
                    {
                        clsLedgerVO objCashCon = new clsLedgerVO();
                        objCashCon.RowID = 1;
                        //objCashCon.ID = Convert.ToInt64(DALHelper.HandleDBNull(readerIPDSelfCr["ID"]));
                        objCashCon.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelfCr["LedgerName"]));
                        objCashCon.Narration = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelfCr["Narration"]));
                        objCashCon.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerIPDSelfCr["Amount"]));
                        //objCashCon.Reference = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelfCr["BillNo"]));
                        objCashCon.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerIPDSelfCr["IsCredit"]));
                        objCashCon.CR = 0;
                        objCashCon.VoucherType = "Sales";
                        if (objCashCon.DR > 0)
                            BizActionObj.Details.IPDSelfCreditForTallyInterface.Add(objCashCon);
                    }

                }
                readerIPDSelfCr.NextResult();

                if (readerIPDSelfCr.HasRows)
                {
                    while (readerIPDSelfCr.Read())
                    {
                        clsLedgerVO objCashCon = new clsLedgerVO();
                        //objCashCon.ID = Convert.ToInt64(DALHelper.HandleDBNull(readerIPDSelfCr["ID"]));
                        objCashCon.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelfCr["LedgerName"]));
                        objCashCon.Narration = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelfCr["Narration"]));
                        objCashCon.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerIPDSelfCr["Amount"]));
                        //objCashCon.Reference = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelfCr["BillNo"]));
                        objCashCon.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerIPDSelfCr["IsCredit"]));
                        objCashCon.CR = 0;
                        objCashCon.VoucherType = "Sales";
                        if (objCashCon.DR > 0)
                            BizActionObj.Details.IPDSelfCreditForTallyInterface.Add(objCashCon);
                    }

                }
                readerIPDSelfCr.NextResult();

                if (readerIPDSelfCr.HasRows)
                {
                    while (readerIPDSelfCr.Read())
                    {
                        clsLedgerVO objCashCon = new clsLedgerVO();
                        //objCashCon.ID = Convert.ToInt64(DALHelper.HandleDBNull(readerIPDSelfCr["ID"]));
                        objCashCon.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelfCr["LedgerName"]));
                        objCashCon.Narration = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelfCr["Narration"]));
                        objCashCon.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerIPDSelfCr["Amount"]));
                        // objCashCon.Reference = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelfCr["BillNo"]));
                        objCashCon.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerIPDSelfCr["IsCredit"]));
                        objCashCon.CR = 0;
                        objCashCon.VoucherType = "Sales";
                        if (objCashCon.DR > 0)
                            BizActionObj.Details.IPDSelfCreditForTallyInterface.Add(objCashCon);
                    }

                }
                readerIPDSelfCr.NextResult();
                if (readerIPDSelfCr.HasRows)
                {
                    while (readerIPDSelfCr.Read())
                    {
                        clsLedgerVO objCashCon = new clsLedgerVO();
                        //  objCashCon.ID = Convert.ToInt64(DALHelper.HandleDBNull(readerIPDSelfCr["ID"]));
                        objCashCon.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelfCr["LedgerName"]));
                        objCashCon.Narration = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelfCr["Narration"]));
                        objCashCon.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerIPDSelfCr["Amount"]));
                        // objCashCon.Reference = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelfCr["BillNo"]));
                        objCashCon.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerIPDSelfCr["IsCredit"]));
                        objCashCon.CR = 0;
                        objCashCon.VoucherType = "Sales";
                        if (objCashCon.DR > 0)
                            BizActionObj.Details.IPDSelfCreditForTallyInterface.Add(objCashCon);
                    }

                }
                readerIPDSelfCr.NextResult();

                if (readerIPDSelfCr.HasRows)
                {
                    while (readerIPDSelfCr.Read())
                    {
                        clsLedgerVO _objCashVO = new clsLedgerVO();
                        // _objCashVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(readerIPDSelfCr["ID"]));
                        _objCashVO.LedgerName = (string)DALHelper.HandleDBNull(readerIPDSelfCr["LedgerName"]);
                        _objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelfCr["Narration"]));
                        //  _objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelfCr["BillNo"]));
                        _objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerIPDSelfCr["Amount"]));
                        _objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerIPDSelfCr["IsCredit"]));
                        _objCashVO.CR = 0;
                        _objCashVO.VoucherType = "Sales";
                        if (_objCashVO.DR > 0)
                            BizActionObj.Details.IPDSelfCreditForTallyInterface.Add(_objCashVO);
                    }
                }
                readerIPDSelfCr.NextResult();

                if (readerIPDSelfCr.HasRows)
                {
                    while (readerIPDSelfCr.Read())
                    {
                        clsLedgerVO _objCashVO = new clsLedgerVO();
                        // _objCashVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(readerIPDSelfCr["ID"]));
                        _objCashVO.LedgerName = (string)DALHelper.HandleDBNull(readerIPDSelfCr["LedgerName"]);
                        _objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelfCr["Narration"]));
                        //  _objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelfCr["BillNo"]));
                        _objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerIPDSelfCr["Amount"]));
                        _objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerIPDSelfCr["IsCredit"]));
                        _objCashVO.CR = 0;
                        _objCashVO.VoucherType = "Sales";
                        if (_objCashVO.DR > 0)
                            BizActionObj.Details.IPDSelfCreditForTallyInterface.Add(_objCashVO);
                    }
                }
                readerIPDSelfCr.NextResult();

                if (readerIPDSelfCr.HasRows)
                {
                    while (readerIPDSelfCr.Read())
                    {
                        clsLedgerVO objCashCon = new clsLedgerVO();
                        //objCashCon.ID = Convert.ToInt64(DALHelper.HandleDBNull(readerIPDSelfCr["ID"]));
                        objCashCon.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelfCr["LedgerName"]));
                        if (BizActionObj.Details.IPDSelfCreditForTallyInterface != null)
                        {
                            if (BizActionObj.Details.IPDSelfCreditForTallyInterface.Where(S => S.RowID == 1).Any() == null)
                                objCashCon.RowID = 1;
                            else
                                objCashCon.RowID = 0;
                        }
                        else
                            objCashCon.RowID = 1;
                        objCashCon.Narration = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelfCr["Narration"]));
                        objCashCon.CR = Convert.ToDouble(DALHelper.HandleDBNull(readerIPDSelfCr["Amount"]));
                        //objCashCon.Reference = Convert.ToString(DALHelper.HandleDBNull(readerIPDSelfCr["BillNo"]));
                        objCashCon.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerIPDSelfCr["IsCredit"]));
                        objCashCon.DR = 0;
                        objCashCon.VoucherType = "Sales";
                        if (objCashCon.CR > 0)
                            BizActionObj.Details.IPDSelfCreditForTallyInterface.Add(objCashCon);
                    }

                }

                readerIPDSelfCr.Close();
                #endregion

                #endregion

                #region DoctorBill
                DbCommand commandDoctorBill = dbServer.GetStoredProcCommand("CIMS_GetDoctorBillLedger");
                dbServer.AddInParameter(commandDoctorBill, "UnitId", DbType.Int64, BizActionObj.Details.UnitID);
                dbServer.AddInParameter(commandDoctorBill, "Date", DbType.Date, BizActionObj.Details.ExportDate);
                reader = (DbDataReader)dbServer.ExecuteReader(commandDoctorBill);
                if (reader.HasRows)
                {
                    if (BizActionObj.Details.DoctorPaymentLedgers == null)
                        BizActionObj.Details.DoctorPaymentLedgers = new List<clsLedgerVO>();
                    while (reader.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(reader["Narration"]));
                        objCashVO.DR = 0;
                        objCashVO.CR = Convert.ToDouble(DALHelper.HandleDBNull(reader["Amount"]));
                        if (objCashVO.CR > 0)
                            BizActionObj.Details.DoctorPaymentLedgers.Add(objCashVO);
                    }
                    reader.NextResult();

                    while (reader.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(reader["Narration"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(reader["Amount"]));
                        objCashVO.CR = 0;
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.DoctorPaymentLedgers.Add(objCashVO);
                    }
                    reader.NextResult();
                    while (reader.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(reader["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(reader["Narration"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(reader["Amount"]));
                        objCashVO.CR = 0;
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.DoctorPaymentLedgers.Add(objCashVO);
                    }
                    reader.Close();
                }
                #endregion

                #region Inventory
                #region Inventory Purchase

                DbCommand commandPurchase = dbServer.GetStoredProcCommand("CIMS_GetAccItemGRN");
                dbServer.AddInParameter(commandPurchase, "UnitId", DbType.Int64, BizActionObj.Details.UnitID);
                dbServer.AddInParameter(commandPurchase, "Date", DbType.DateTime, BizActionObj.Details.ExportDate);
                //dbServer.AddInParameter(commandPurchase, "StoreID", DbType.Int64, BizActionObj.Details.StoreID);

                DbDataReader readerPurchase = (DbDataReader)dbServer.ExecuteReader(commandPurchase);
                BillIDsList = new List<clsLedgerVO>();
                if (readerPurchase.HasRows)
                {
                    if (BizActionObj.Details.PurchaseLedger == null)
                        BizActionObj.Details.PurchaseLedger = new List<clsLedgerVO>();

                    while (readerPurchase.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerPurchase["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerPurchase["GRNIDs"]));
                        objCashVO.DR = 0;
                        objCashVO.CR = Convert.ToDouble(DALHelper.HandleDBNull(readerPurchase["Amount"]));

                        if (BizActionObj.Details.PurchaseLedger != null)
                        {
                            if (BizActionObj.Details.PurchaseLedger.SingleOrDefault(S => S.RowID.Equals(1)) == null)
                                objCashVO.RowID = 1;
                            else
                                objCashVO.RowID = 0;
                        }
                        else
                            objCashVO.RowID = 1;

                        objCashVO.VoucherType = "Purchase";
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(readerPurchase["GPVoucherType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(readerPurchase["VoucherMode"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerPurchase["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerPurchase["TotalAmount"]));
                        objCashVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerPurchase["NetAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(readerPurchase["VendorID"]));
                        objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(readerPurchase["TransactionNo"]));
                        objCashVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(readerPurchase["GRNUnitId"]));
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerPurchase["IsCredit"]));
                        if (objCashVO.CR > 0)
                            BizActionObj.Details.PurchaseLedger.Add(objCashVO);
                    }

                    readerPurchase.NextResult();


                    while (readerPurchase.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerPurchase["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerPurchase["GRNIDs"]));
                        objCashVO.CR = 0;
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerPurchase["Amount"]));

                        if (BizActionObj.Details.PurchaseLedger != null)
                        {
                            if (BizActionObj.Details.PurchaseLedger.SingleOrDefault(S => S.RowID.Equals(1)) == null)
                                objCashVO.RowID = 1;
                            else
                                objCashVO.RowID = 0;
                        }
                        else
                            objCashVO.RowID = 1;
                        objCashVO.VoucherType = "Purchase";
                        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(readerPurchase["TransactionType"]));
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(readerPurchase["GPVoucherType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(readerPurchase["VoucherMode"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerPurchase["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerPurchase["TotalAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(readerPurchase["VendorID"]));
                        objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(readerPurchase["TransactionNo"]));
                        objCashVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(readerPurchase["GRNUnitId"]));
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerPurchase["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.PurchaseLedger.Add(objCashVO);
                    }

                    readerPurchase.NextResult();

                    while (readerPurchase.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerPurchase["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerPurchase["GRNIDs"]));
                        objCashVO.CR = 0;
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerPurchase["Amount"]));

                        if (BizActionObj.Details.PurchaseLedger != null)
                        {
                            if (BizActionObj.Details.PurchaseLedger.SingleOrDefault(S => S.RowID.Equals(1)) == null)
                                objCashVO.RowID = 1;
                            else
                                objCashVO.RowID = 0;
                        }
                        else
                            objCashVO.RowID = 1;
                        objCashVO.VoucherType = "Purchase";
                        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(readerPurchase["TransactionType"]));
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(readerPurchase["GPVoucherType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(readerPurchase["VoucherMode"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerPurchase["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerPurchase["TotalAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(readerPurchase["VendorID"]));
                        objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(readerPurchase["TransactionNo"]));
                        objCashVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(readerPurchase["GRNUnitId"]));
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerPurchase["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.PurchaseLedger.Add(objCashVO);
                    }
                    readerPurchase.Close();
                }

                #endregion

                #region Inventory Sale Income

                DbCommand commandSaleIncome = dbServer.GetStoredProcCommand("CIMS_GetAccountSaleIncome");

                dbServer.AddInParameter(commandSaleIncome, "UnitId", DbType.Int64, BizActionObj.Details.UnitID);
                dbServer.AddInParameter(commandSaleIncome, "Date", DbType.DateTime, BizActionObj.Details.ExportDate);
                //dbServer.AddInParameter(commandSaleIncome, "StoreID", DbType.Int64, BizActionObj.Details.StoreID);

                DbDataReader readerSaleIncome = (DbDataReader)dbServer.ExecuteReader(commandSaleIncome);
                LedgerAccountList = new List<clsLedgerVO>();
                BillIDsList = new List<clsLedgerVO>();

                if (BizActionObj.Details.SaleSelfReceiptLedgers == null)
                    BizActionObj.Details.SaleSelfReceiptLedgers = new List<clsLedgerVO>();

                if (readerSaleIncome.HasRows)
                {
                    while (readerSaleIncome.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["Narration"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleIncome["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Sales";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerSaleIncome["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.SaleIncome.Add(objCashVO);
                    }
                }
                readerSaleIncome.NextResult();
                if (readerSaleIncome.HasRows)
                {
                    while (readerSaleIncome.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["Narration"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleIncome["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Sales";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerSaleIncome["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.SaleIncome.Add(objCashVO);
                    }
                }

                readerSaleIncome.NextResult();
                if (readerSaleIncome.HasRows)
                {
                    while (readerSaleIncome.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["Narration"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleIncome["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Sales";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerSaleIncome["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.SaleIncome.Add(objCashVO);
                    }
                }
                readerSaleIncome.NextResult();
                if (readerSaleIncome.HasRows)
                {
                    while (readerSaleIncome.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["Narration"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleIncome["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Sales";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerSaleIncome["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.SaleIncome.Add(objCashVO);
                    }
                }
                readerSaleIncome.NextResult();
                if (readerSaleIncome.HasRows)
                {
                    while (readerSaleIncome.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["Narration"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleIncome["TotalConcession"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Sales";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerSaleIncome["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.SaleIncome.Add(objCashVO);
                    }
                }
                readerSaleIncome.NextResult();
                if (readerSaleIncome.HasRows)
                {
                    while (readerSaleIncome.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["Narration"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleIncome["PatientBalance"]));
                        objCashVO.CR = 0;
                        objCashVO.VoucherType = "Sales";
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerSaleIncome["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.SaleIncome.Add(objCashVO);
                    }

                }
                readerSaleIncome.NextResult();
                if (readerSaleIncome.HasRows)
                {
                    while (readerSaleIncome.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["BillIDs"]));
                        objCashVO.DR = 0;
                        objCashVO.CR = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleIncome["Amount"]));

                        if (BizActionObj.Details.SaleIncome != null)
                        {
                            if (BizActionObj.Details.SaleIncome.SingleOrDefault(S => S.RowID.Equals(1)) == null)
                                objCashVO.RowID = 1;
                            else
                                objCashVO.RowID = 0;
                        }
                        else
                            objCashVO.RowID = 1;
                        objCashVO.VoucherType = "Sales";
                        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["TransactionType"]));
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["GPVoucherType"]));
                        objCashVO.PatientType = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["PatientType"]));
                        objCashVO.Sponsor = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["SponsorType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["VoucherMode"]));
                        objCashVO.PatientNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["PatientNo"]));
                        objCashVO.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["Pat_Comp_Name"]));
                        objCashVO.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["TransactionGroup"]));
                        objCashVO.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleIncome["TransactionId"]));
                        objCashVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["Remark"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleIncome["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleIncome["TotalAmount"]));
                        objCashVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleIncome["NetAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleIncome["VendorID"]));
                        objCashVO.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["PurchaseInvoiceNo"]));
                        objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["TransactionNo"]));
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerSaleIncome["IsCredit"]));
                        if (objCashVO.CR > 0)
                            BizActionObj.Details.SaleIncome.Add(objCashVO);
                    }
                }
                readerSaleIncome.NextResult();
                if (readerSaleIncome.HasRows)
                {
                    while (readerSaleIncome.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["BillIDs"]));
                        objCashVO.DR = 0;
                        objCashVO.CR = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleIncome["Amount"]));

                        if (BizActionObj.Details.SaleIncome != null)
                        {
                            if (BizActionObj.Details.SaleIncome.SingleOrDefault(S => S.RowID.Equals(1)) == null)
                                objCashVO.RowID = 1;
                            else
                                objCashVO.RowID = 0;
                        }
                        else
                            objCashVO.RowID = 1;
                        objCashVO.VoucherType = "Sales";
                        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["TransactionType"]));
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["GPVoucherType"]));
                        objCashVO.PatientType = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["PatientType"]));
                        objCashVO.Sponsor = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["SponsorType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["VoucherMode"]));
                        objCashVO.PatientNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["PatientNo"]));
                        objCashVO.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["Pat_Comp_Name"]));
                        objCashVO.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["TransactionGroup"]));
                        objCashVO.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleIncome["TransactionId"]));
                        objCashVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["Remark"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleIncome["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleIncome["TotalAmount"]));
                        objCashVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleIncome["NetAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleIncome["VendorID"]));
                        objCashVO.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["PurchaseInvoiceNo"]));
                        objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleIncome["TransactionNo"]));
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerSaleIncome["IsCredit"]));
                        if (objCashVO.CR > 0)
                            BizActionObj.Details.SaleIncome.Add(objCashVO);
                    }
                }
                readerSaleIncome.Close();


                #endregion

                #region Inventory Sale Credit

                DbCommand commandSaleCredit = dbServer.GetStoredProcCommand("CIMS_GetAccItemSaleCreditBill");

                dbServer.AddInParameter(commandSaleCredit, "UnitId", DbType.Int64, BizActionObj.Details.UnitID);
                dbServer.AddInParameter(commandSaleCredit, "Date", DbType.DateTime, BizActionObj.Details.ExportDate);
                //dbServer.AddInParameter(commandSaleCredit, "StoreID", DbType.Int64, BizActionObj.Details.StoreID);

                DbDataReader readerSaleCredit = (DbDataReader)dbServer.ExecuteReader(commandSaleCredit);
                LedgerAccountList = new List<clsLedgerVO>();
                BillIDsList = new List<clsLedgerVO>();

                if (BizActionObj.Details.SaleCredit == null)
                    BizActionObj.Details.SaleCredit = new List<clsLedgerVO>();
                if (readerSaleCredit.HasRows)
                {
                    while (readerSaleCredit.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleCredit["ID"]));
                        objCashVO.LedgerName = (string)DALHelper.HandleDBNull(readerSaleCredit["CashInHand"]);
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["Narration"]));
                        objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["BillNo"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerSaleCredit["IsCredit"]));
                        objCashVO.VoucherType = "Sales";
                        objCashVO.BalanceAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["BalanceAmountSelf"]));
                        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["TransactionType"]));
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["GPVoucherType"]));
                        objCashVO.PatientType = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["PatientType"]));
                        objCashVO.Sponsor = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["SponsorType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["VoucherMode"]));
                        objCashVO.PatientNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["PatientNo"]));
                        objCashVO.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["Pat_Comp_Name"]));
                        objCashVO.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["TransactionGroup"]));
                        objCashVO.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleCredit["TransactionId"]));
                        objCashVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["Remark"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["TotalAmount"]));
                        objCashVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["NetAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleCredit["VendorID"]));
                        objCashVO.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["PurchaseInvoiceNo"]));
                        objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["TransactionNo"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.SaleCredit.Add(objCashVO);
                    }
                }
                readerSaleCredit.NextResult();

                if (readerSaleCredit.HasRows)
                {
                    while (readerSaleCredit.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleCredit["ID"]));
                        objCashVO.LedgerName = (string)DALHelper.HandleDBNull(readerSaleCredit["CashInHand"]);
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["Narration"]));
                        objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["BillNo"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerSaleCredit["IsCredit"]));
                        objCashVO.VoucherType = "Sales";
                        objCashVO.BalanceAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["BalanceAmountSelf"]));
                        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["TransactionType"]));
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["GPVoucherType"]));
                        objCashVO.PatientType = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["PatientType"]));
                        objCashVO.Sponsor = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["SponsorType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["VoucherMode"]));
                        objCashVO.PatientNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["PatientNo"]));
                        objCashVO.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["Pat_Comp_Name"]));
                        objCashVO.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["TransactionGroup"]));
                        objCashVO.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleCredit["TransactionId"]));
                        objCashVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["Remark"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["TotalAmount"]));
                        objCashVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["NetAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleCredit["VendorID"]));
                        objCashVO.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["PurchaseInvoiceNo"]));
                        objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["TransactionNo"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.SaleCredit.Add(objCashVO);
                    }
                }

                readerSaleCredit.NextResult();

                if (readerSaleCredit.HasRows)
                {
                    while (readerSaleCredit.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleCredit["ID"]));
                        objCashVO.LedgerName = (string)DALHelper.HandleDBNull(readerSaleCredit["CashInHand"]);
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["Narration"]));
                        objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["BillNo"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerSaleCredit["IsCredit"]));
                        objCashVO.VoucherType = "Sales";
                        objCashVO.BalanceAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["BalanceAmountSelf"]));
                        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["TransactionType"]));
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["GPVoucherType"]));
                        objCashVO.PatientType = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["PatientType"]));
                        objCashVO.Sponsor = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["SponsorType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["VoucherMode"]));
                        objCashVO.PatientNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["PatientNo"]));
                        objCashVO.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["Pat_Comp_Name"]));
                        objCashVO.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["TransactionGroup"]));
                        objCashVO.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleCredit["TransactionId"]));
                        objCashVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["Remark"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["TotalAmount"]));
                        objCashVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["NetAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleCredit["VendorID"]));
                        objCashVO.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["PurchaseInvoiceNo"]));
                        objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["TransactionNo"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.SaleCredit.Add(objCashVO);
                    }
                }
                readerSaleCredit.NextResult();

                if (readerSaleCredit.HasRows)
                {
                    while (readerSaleCredit.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleCredit["ID"]));
                        objCashVO.LedgerName = (string)DALHelper.HandleDBNull(readerSaleCredit["CashInHand"]);
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["Narration"]));
                        objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["BillNo"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerSaleCredit["IsCredit"]));
                        objCashVO.VoucherType = "Sales";
                        objCashVO.BalanceAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["BalanceAmountSelf"]));
                        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["TransactionType"]));
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["GPVoucherType"]));
                        objCashVO.PatientType = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["PatientType"]));
                        objCashVO.Sponsor = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["SponsorType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["VoucherMode"]));
                        objCashVO.PatientNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["PatientNo"]));
                        objCashVO.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["Pat_Comp_Name"]));
                        objCashVO.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["TransactionGroup"]));
                        objCashVO.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleCredit["TransactionId"]));
                        objCashVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["Remark"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["TotalAmount"]));
                        objCashVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["NetAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleCredit["VendorID"]));
                        objCashVO.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["PurchaseInvoiceNo"]));
                        objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["TransactionNo"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.SaleCredit.Add(objCashVO);
                    }
                }
                readerSaleCredit.NextResult();

                if (readerSaleCredit.HasRows)
                {
                    while (readerSaleCredit.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleCredit["ID"]));
                        objCashVO.LedgerName = (string)DALHelper.HandleDBNull(readerSaleCredit["CashInHand"]);
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["Narration"]));
                        objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["BillNo"]));
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["Amount"]));
                        objCashVO.CR = 0;
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerSaleCredit["IsCredit"]));
                        objCashVO.VoucherType = "Sales";
                        objCashVO.BalanceAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["BalanceAmountSelf"]));
                        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["TransactionType"]));
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["GPVoucherType"]));
                        objCashVO.PatientType = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["PatientType"]));
                        objCashVO.Sponsor = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["SponsorType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["VoucherMode"]));
                        objCashVO.PatientNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["PatientNo"]));
                        objCashVO.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["Pat_Comp_Name"]));
                        objCashVO.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["TransactionGroup"]));
                        objCashVO.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleCredit["TransactionId"]));
                        objCashVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["Remark"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["TotalAmount"]));
                        objCashVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["NetAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleCredit["VendorID"]));
                        objCashVO.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["PurchaseInvoiceNo"]));
                        objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["TransactionNo"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.SaleCredit.Add(objCashVO);
                    }
                }
                readerSaleCredit.NextResult();
                if (readerSaleCredit.HasRows)
                {
                    while (readerSaleCredit.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleCredit["ID"]));
                        objCashVO.LedgerName = (string)DALHelper.HandleDBNull(readerSaleCredit["CashInHand"]);
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["Narration"]));
                        objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["BillNo"]));
                        objCashVO.CR = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["Amount"]));
                        objCashVO.DR = 0;
                        if (LedgerAccountList != null)
                        {
                            if (BizActionObj.Details.SaleCredit.SingleOrDefault(S => S.RowID.Equals(1)) == null)
                                objCashVO.RowID = 1;
                            else
                                objCashVO.RowID = 0;
                        }
                        else
                            objCashVO.RowID = 1;
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerSaleCredit["IsCredit"]));
                        objCashVO.VoucherType = "Sales";
                        objCashVO.BalanceAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["BalanceAmountSelf"]));
                        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["TransactionType"]));
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["GPVoucherType"]));
                        objCashVO.PatientType = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["PatientType"]));
                        objCashVO.Sponsor = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["SponsorType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["VoucherMode"]));
                        objCashVO.PatientNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["PatientNo"]));
                        objCashVO.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["Pat_Comp_Name"]));
                        objCashVO.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["TransactionGroup"]));
                        objCashVO.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleCredit["TransactionId"]));
                        objCashVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["Remark"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["TotalAmount"]));
                        objCashVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCredit["NetAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleCredit["VendorID"]));
                        objCashVO.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["PurchaseInvoiceNo"]));
                        objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleCredit["TransactionNo"]));
                        if (objCashVO.CR > 0)
                            BizActionObj.Details.SaleCredit.Add(objCashVO);
                    }
                }
                readerSaleCredit.Close();

                #endregion

                #region Inventory Sale return
                DbCommand commandSaleReturn = dbServer.GetStoredProcCommand("CIMS_GetAccItemSaleReturn");

                dbServer.AddInParameter(commandSaleReturn, "UnitId", DbType.Int64, BizActionObj.Details.UnitID);
                dbServer.AddInParameter(commandSaleReturn, "Date", DbType.DateTime, BizActionObj.Details.ExportDate);

                DbDataReader readerSaleReturn = (DbDataReader)dbServer.ExecuteReader(commandSaleReturn);
                LedgerAccountList = new List<clsLedgerVO>();
                BillIDsList = new List<clsLedgerVO>();

                if (readerSaleReturn.HasRows)
                {
                    while (readerSaleReturn.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerSaleReturn["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerSaleReturn["GRNIDs"]));
                        objCashVO.DR = 0;
                        objCashVO.CR = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleReturn["Amount"]));

                        if (BizActionObj.Details.ItemSaleReturnLedgers != null)
                        {
                            if (BizActionObj.Details.ItemSaleReturnLedgers.SingleOrDefault(S => S.RowID.Equals(1)) == null)
                                objCashVO.RowID = 1;
                            else
                                objCashVO.RowID = 0;
                        }
                        else
                            objCashVO.RowID = 1;
                        objCashVO.VoucherType = "Credit Note";

                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(readerSaleReturn["GPVoucherType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(readerSaleReturn["VoucherMode"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleReturn["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleReturn["TotalAmount"]));
                        objCashVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleReturn["NetAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleReturn["VendorID"]));
                        objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleReturn["TransactionNo"]));
                        objCashVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleReturn["GRNUnitId"]));
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerSaleReturn["IsCredit"]));
                        if (objCashVO.CR > 0)
                            BizActionObj.Details.SaleReturn.Add(objCashVO);
                    }
                }
                readerSaleReturn.NextResult();

                if (readerSaleReturn.HasRows)
                {
                    if (BizActionObj.Details.SaleReturn == null)
                        BizActionObj.Details.SaleReturn = new List<clsLedgerVO>();
                    while (readerSaleReturn.Read())
                    {
                        clsLedgerVO objCashVO = new clsLedgerVO();
                        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerSaleReturn["LedgerName"]));
                        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerSaleReturn["GRNIDs"]));
                        objCashVO.CR = 0;
                        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleReturn["Amount"]));

                        if (BizActionObj.Details.SaleReturn != null)
                        {
                            if (BizActionObj.Details.SaleReturn.SingleOrDefault(S => S.RowID.Equals(1)) == null)
                                objCashVO.RowID = 1;
                            else
                                objCashVO.RowID = 0;
                        }
                        else
                            objCashVO.RowID = 1;
                        objCashVO.VoucherType = "Credit Note";
                        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(readerSaleReturn["TransactionType"]));
                        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(readerSaleReturn["GPVoucherType"]));
                        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(readerSaleReturn["VoucherMode"]));
                        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleReturn["VatAmount"]));
                        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleReturn["TotalAmount"]));
                        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleReturn["VendorID"]));
                        objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleReturn["TransactionNo"]));
                        objCashVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleReturn["GRNUnitId"]));
                        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerSaleReturn["IsCredit"]));
                        if (objCashVO.DR > 0)
                            BizActionObj.Details.SaleReturn.Add(objCashVO);
                    }
                }

                readerSaleReturn.Close();



                #endregion

                #region Inventory Company Sale Receipt
                //DbCommand commandCompanySale = dbServer.GetStoredProcCommand("CIMS_GetAccItemSaleCompanyReceipt");

                //dbServer.AddInParameter(commandCompanySale, "UnitId", DbType.Int64, BizActionObj.Details.UnitID);
                //dbServer.AddInParameter(commandCompanySale, "Date", DbType.DateTime, BizActionObj.Details.ExportDate);
                //dbServer.AddInParameter(commandCompanySale, "StoreID", DbType.Int64, BizActionObj.Details.StoreID);

                //DbDataReader readerSaleCompany = (DbDataReader)dbServer.ExecuteReader(commandCompanySale);
                //LedgerAccountList = new List<clsLedgerVO>();
                //BillIDsList = new List<clsLedgerVO>();
                //if (readerSaleCompany.HasRows)
                //{
                //    if (BizActionObj.Details.SaleCompanyReceiptLedgers == null)
                //        BizActionObj.Details.SaleCompanyReceiptLedgers = new List<clsLedgerVO>();
                //    while (readerSaleCompany.Read())
                //    {
                //        clsLedgerVO objCashVO = new clsLedgerVO();
                //        objCashVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleCompany["ID"]));
                //        objCashVO.LedgerName = (string)DALHelper.HandleDBNull(readerSaleCompany["LedgerName"]);
                //        objCashVO.Narration = (string)DALHelper.HandleDBNull(readerSaleCompany["Narration"]);
                //        objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(readerSaleCompany["TransactionNo"]));
                //        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerSaleCompany["IsCredit"]));
                //        objCashVO.DR = 0;
                //        objCashVO.CR = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCompany["Amount"]));
                //        objCashVO.VoucherType = "Sales";
                //        objCashVO.TransactionType = Convert.ToString(DALHelper.HandleDBNull(readerSaleCompany["TransactionType"]));
                //        objCashVO.GPVoucherType = Convert.ToString(DALHelper.HandleDBNull(readerSaleCompany["GPVoucherType"]));
                //        objCashVO.PatientType = Convert.ToString(DALHelper.HandleDBNull(readerSaleCompany["PatientType"]));
                //        objCashVO.Sponsor = Convert.ToString(DALHelper.HandleDBNull(readerSaleCompany["SponsorType"]));
                //        objCashVO.VoucherMode = Convert.ToString(DALHelper.HandleDBNull(readerSaleCompany["VoucherMode"]));
                //        objCashVO.PatientNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleCompany["PatientNo"]));
                //        objCashVO.Pat_Comp_Name = Convert.ToString(DALHelper.HandleDBNull(readerSaleCompany["Pat_Comp_Name"]));
                //        objCashVO.TransactionGroup = Convert.ToString(DALHelper.HandleDBNull(readerSaleCompany["TransactionGroup"]));
                //        objCashVO.TransactionId = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleCompany["TransactionId"]));
                //        objCashVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(readerSaleCompany["Remark"]));
                //        objCashVO.VatAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCompany["VatAmount"]));
                //        objCashVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCompany["TotalAmount"]));
                //        objCashVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCompany["NetAmount"]));
                //        objCashVO.VendorId = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleCompany["VendorID"]));
                //        objCashVO.PurchaseInvoiceNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleCompany["PurchaseInvoiceNo"]));
                //        objCashVO.TransactionNo = Convert.ToString(DALHelper.HandleDBNull(readerSaleCompany["TransactionNo"]));
                //        if (objCashVO.CR > 0)
                //        {
                //            objCashVO.RowID = 1;
                //            LedgerAccountList.Add(objCashVO);
                //        }
                //        BillIDsList.Add(objCashVO);

                //    }
                //    readerSaleCompany.NextResult();

                //    while (readerSaleCompany.Read())
                //    {
                //        clsLedgerVO objCashVO = new clsLedgerVO();
                //        objCashVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(readerSaleCompany["ID"]));
                //        objCashVO.LedgerName = Convert.ToString(DALHelper.HandleDBNull(readerSaleCompany["LedgerName"]));
                //        objCashVO.Narration = Convert.ToString(DALHelper.HandleDBNull(readerSaleCompany["Narration"]));
                //        objCashVO.Reference = Convert.ToString(DALHelper.HandleDBNull(readerSaleCompany["TransactionNo"]));
                //        objCashVO.IsCredit = Convert.ToBoolean(DALHelper.HandleDBNull(readerSaleCompany["IsCredit"]));
                //        objCashVO.DR = Convert.ToDouble(DALHelper.HandleDBNull(readerSaleCompany["Amount"]));
                //        objCashVO.CR = 0;
                //        objCashVO.VoucherType = "Sales";
                //        if (objCashVO.DR > 0)
                //            LedgerAccountList.Add(objCashVO);

                //    }

                //    foreach (var item in BillIDsList)
                //    {
                //        foreach (var _item in LedgerAccountList.Where(S => S.ID.Equals(item.ID)).ToList())
                //        {
                //            BizActionObj.Details.SaleCompanyReceiptLedgers.Add(_item);
                //        }
                //    }

                //    readerSaleCompany.Close();
                //}


                #endregion

                #endregion
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {

            }
            return valueObject;
        }

        public override IValueObject UpdateBillPaymentDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            // throw new NotImplementedException();
            DbTransaction trans = null;
            DbConnection con = null;


            clsUpdateBillPaymentDtlsBizActionVO BizAction = valueObject as clsUpdateBillPaymentDtlsBizActionVO;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                clsBillVO objDetailsVO = BizAction.Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateBillPaymentDetails");


                dbServer.AddInParameter(command, "BillID", DbType.Int64, objDetailsVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDetailsVO.UnitID);
                // Added by CDS
                dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);


                if (objDetailsVO.BalanceAmountSelf < 0) objDetailsVO.BalanceAmountSelf = 0;
                dbServer.AddInParameter(command, "BalanceAmountSelf", DbType.Double, objDetailsVO.BalanceAmountSelf);
                // Added by CDS
                dbServer.AddInParameter(command, "TotalConcessionAmount", DbType.Double, objDetailsVO.TotalConcessionAmount);
                dbServer.AddInParameter(command, "NetBillAmount", DbType.Double, objDetailsVO.NetBillAmount);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);

                BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                #region For IPD Module

                foreach (var item in BizAction.ChargeDetails)
                {
                    item.ChargeDetails = new clsChargeDetailsVO();
                    //Add service details in t_charge
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_UpdateCharge");


                    dbServer.AddInParameter(command1, "ID", DbType.Int64, item.ID);
                    dbServer.AddInParameter(command1, "Date", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    // Added by CDS
                    dbServer.AddInParameter(command1, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);

                    dbServer.AddInParameter(command1, "PaidAmount", DbType.Double, item.TotalServicePaidAmount);
                    dbServer.AddInParameter(command1, "NetAmount", DbType.Double, item.TotalNetAmount);
                    dbServer.AddInParameter(command1, "Concession", DbType.Double, item.TotalConcession);
                    dbServer.AddInParameter(command1, "ConcessionPercentage", DbType.Double, item.TotalConcessionPercentage);

                    int iStatus = dbServer.ExecuteNonQuery(command1, trans);
                    item.ID = (long)dbServer.GetParameterValue(command1, "ID");


                    if (item.IsUpdate == true)
                    {
                        DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_UpdateChargeDetails");


                        dbServer.AddInParameter(command3, "ChargeID ", DbType.Int64, item.ID);
                        dbServer.AddInParameter(command3, "Date", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                        dbServer.AddInParameter(command3, "PaidAmount", DbType.Double, item.ServicePaidAmount);
                        dbServer.AddInParameter(command3, "Concession", DbType.Double, item.Concession);
                        dbServer.AddInParameter(command3, "ServiceTaxAmount", DbType.Double, item.ServiceTaxAmount);
                        dbServer.AddInParameter(command3, "NetAmount", DbType.Double, item.SettleNetAmount);
                        dbServer.AddInParameter(command3, "BalanceAmount", DbType.Double, item.BalanceAmount);
                        dbServer.AddInParameter(command3, "IsSameDate", DbType.Boolean, item.IsSameDate);

                        int iStatus4 = dbServer.ExecuteNonQuery(command3, trans);

                    }
                    else
                    {

                        //Add service payment details in t_chargeDetails
                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddChargeDetails");
                        dbServer.AddInParameter(command2, "ChargeID", DbType.Int64, item.ID);
                        dbServer.AddInParameter(command2, "ChargeUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                        dbServer.AddInParameter(command2, "Date", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command2, "Quantity", DbType.Double, item.Quantity);
                        dbServer.AddInParameter(command2, "Rate", DbType.Double, item.Rate);
                        dbServer.AddInParameter(command2, "TotalAmount", DbType.Double, item.TotalAmount);
                        dbServer.AddInParameter(command2, "ConcessionAmount", DbType.Double, item.Concession);
                        dbServer.AddInParameter(command2, "ServiceTaxAmount", DbType.Double, item.ServiceTaxAmount);

                        dbServer.AddInParameter(command2, "NetAmount", DbType.Double, item.SettleNetAmount);
                        dbServer.AddInParameter(command2, "PaidAmount", DbType.Double, item.ServicePaidAmount);
                        dbServer.AddInParameter(command2, "BalanceAmount", DbType.Double, item.BalanceAmount);

                        dbServer.AddInParameter(command2, "Status", DbType.Boolean, item.Status);
                        dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                        dbServer.AddInParameter(command2, "RefundID", DbType.Int64, item.RefundID);
                        dbServer.AddInParameter(command2, "RefundAmount", DbType.Double, item.RefundAmount);

                        dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ChargeDetails.ID);
                        dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                        item.ChargeDetails.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                        item.ChargeDetails.ID = (long)dbServer.GetParameterValue(command2, "ID");
                    }
                }

                #endregion

                trans.Commit();
                BizAction.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizAction.SuccessStatus = -1;
                trans.Rollback();
                BizAction.ChargeDetails = null;
                BizAction.ChargeDetails = null;
                //throw;
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;

            }
            return BizAction;

        }

        // Bhushanp 
        public override IValueObject GenerateXML(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetTotalBillAccountsLedgersVO BizActionObj = valueObject as clsGetTotalBillAccountsLedgersVO;
            List<ENVELOPE> ENVELOPEList = BizActionObj.ENVELOPEList;

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            XmlSerializer serialiser = new XmlSerializer(typeof(List<ENVELOPE>));
            TextWriter Filestream = new StreamWriter(@"C:\XML\" + BizActionObj.strGenrateXMLName + ".xml");
            serialiser.Serialize(Filestream, ENVELOPEList, ns);
            Filestream.Close();



            ////StringWriter strWriter = new StringWriter();
            ////serialiser.Serialize(strWriter, ENVELOPEList, ns);

            ////string Str = strWriter.ToString();

            return BizActionObj;
        }


        public override IValueObject DeleteFiles(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetTotalBillAccountsLedgersVO BizActionObj = valueObject as clsGetTotalBillAccountsLedgersVO;
            string sourceDir = @"C:\XML\";
            try
            {
                string[] picList = Directory.GetFiles(sourceDir, "*.xml");


                //// Copy picture files. 
                //foreach (string f in picList)
                //{
                //    // Remove path from the file name. 
                //    string fName = f.Substring(sourceDir.Length + 1);

                //    // Use the Path.Combine method to safely append the file name to the path. 
                //    // Will overwrite if the destination file already exists.
                //    File.Copy(Path.Combine(sourceDir, fName), Path.Combine(backupDir, fName), true);
                //}

                // Copy text files. 
                //foreach (string f in picList)
                //{

                //    // Remove path from the file name. 
                //    string fName = f.Substring(sourceDir.Length + 1);

                //    try
                //    {
                //        // Will not overwrite if the destination file already exists.
                //        File.Copy(Path.Combine(sourceDir, fName), Path.Combine(backupDir, fName));
                //    }

                //    // Catch exception if the file was already copied. 
                //    catch (IOException copyError)
                //    {
                //        Console.WriteLine(copyError.Message);
                //    }
                //}

                // Delete source files that were copied. 
                foreach (string f in picList)
                {
                    File.Delete(f);
                }
            }
            catch (DirectoryNotFoundException dirNotFound)
            {
                Console.WriteLine(dirNotFound.Message);
            }
            return BizActionObj;
        }

        // BY BHUSHAN
        public override IValueObject GetDailyCollection(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDailyCollectionListBizActionVO BizActionObj = valueObject as clsGetDailyCollectionListBizActionVO;
            try
            {
                // Daily Collection
                if (BizActionObj.DailySales == true)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_Chart_DailyCollectionReportSubReport");
                    dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizActionObj.UnitID);
                    DbDataReader reader;
                    reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        if (BizActionObj.List == null)
                            BizActionObj.List = new List<clsDailyCollectionReportVO>();
                        while (reader.Read())
                        {
                            clsDailyCollectionReportVO objVO = new clsDailyCollectionReportVO();
                            objVO.PaymentModeID = (MaterPayModeList)(long)(DALHelper.HandleDBNull(reader["PaymentModeID"]));
                            objVO.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);
                            objVO.Collection = (double)(DALHelper.HandleDBNull(reader["Collection"]));
                            BizActionObj.List.Add(objVO);
                        }
                    }
                    reader.Close();
                }

                //Daily sales report
                if (BizActionObj.DailySales == false)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_Chart_DailySalesReport");
                    DbDataReader reader;

                    if (BizActionObj.CollectionDate != null)
                        dbServer.AddInParameter(command, "CollectionDate", DbType.DateTime, BizActionObj.CollectionDate);

                    dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizActionObj.UnitID);

                    reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.SalesList == null)
                            BizActionObj.SalesList = new List<clsDailySalesReportVO>();
                        while (reader.Read())
                        {
                            clsDailySalesReportVO objVO = new clsDailySalesReportVO();
                            objVO.TotalAmount = (double)DALHelper.HandleDBNull(reader["TotalAmount"]);
                            objVO.Specialization = (string)(DALHelper.HandleDBNull(reader["Specialization"]));
                            BizActionObj.SalesList.Add(objVO);
                        }

                    }
                    reader.Close();
                }

                // APpointment List
                if (BizActionObj.ISAppointmentList == true)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_DashboardAppointment");
                    DbDataReader reader;

                    if (BizActionObj.CollectionDate != null)
                        dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.CollectionDate);

                    dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizActionObj.UnitID);

                    reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.AppointmentList == null)
                            BizActionObj.AppointmentList = new List<clsAppointmentVO>();
                        while (reader.Read())
                        {
                            clsAppointmentVO objVO = new clsAppointmentVO();
                            objVO.AppointmentReasonId = (long)DALHelper.HandleDBNull(reader["AppointmentReasonID"]);
                            objVO.AppointmentID = Convert.ToInt64((DALHelper.HandleDBNull(reader["AppointmentID"])));
                            objVO.AppointmentReason = Convert.ToString((DALHelper.HandleDBNull(reader["Appointment"])));
                            BizActionObj.AppointmentList.Add(objVO);
                        }
                    }
                    reader.Close();
                }

                if (BizActionObj.IsVisit == true)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_DashboardVisit");
                    DbDataReader reader;

                    if (BizActionObj.CollectionDate != null)
                        dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.CollectionDate);

                    dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizActionObj.UnitID);

                    reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.VisitList == null)
                            BizActionObj.VisitList = new List<clsVisitVO>();
                        while (reader.Read())
                        {
                            clsVisitVO objVO = new clsVisitVO();
                            objVO.VisitType = (string)DALHelper.HandleDBNull(reader["VisitDesc"]);
                            objVO.VisitTypeID = Convert.ToInt64((DALHelper.HandleDBNull(reader["VisitTypeID"])));
                            objVO.VisitID = Convert.ToInt64((DALHelper.HandleDBNull(reader["NoVisit"])));
                            BizActionObj.VisitList.Add(objVO);
                        }
                    }
                    reader.Close();
                }
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

        // BY BHUSHAN . . . . . . . . . . 
        public override IValueObject GetBillSearch_IVF_List_DashBoard(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetBillSearch_IVF_List_DashBoardBizActionVO BizActionObj = valueObject as clsGetBillSearch_IVF_List_DashBoardBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetBillListForSearch_DashBoard");
                DbDataReader reader;

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "MRNO", DbType.String, BizActionObj.MRNO);


                dbServer.AddInParameter(command, "BillType", DbType.Int32, BizActionObj.BillType);
                dbServer.AddInParameter(command, "UnitID", DbType.String, BizActionObj.UnitID);

                if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                    dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(BizActionObj.FirstName));

                if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(BizActionObj.LastName));

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
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsBillVO>();
                    while (reader.Read())
                    {
                        clsBillVO objVO = new clsBillVO();

                        objVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        objVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        objVO.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);
                        objVO.IsCancelled = (bool)DALHelper.HandleDBNull(reader["IsCancelled"]);
                        objVO.BillNo = (string)DALHelper.HandleDBNull(reader["BillNo"]);
                        objVO.MRNO = (string)DALHelper.HandleDBNull(reader["MRNO"]);
                        objVO.BillType = (BillTypes)((Int16)DALHelper.HandleDBNull(reader["BillType"]));
                        //objVO.Opd_Ipd_External = (Int16)DALHelper.HandleDBNull(reader["Opd_Ipd_External"]);
                        //objVO.Opd_Ipd_External_Id = (long)DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"]);
                        //objVO.Opd_Ipd_External_UnitId = (long)DALHelper.HandleDBNull(reader["Opd_Ipd_External_UnitId"]);
                        objVO.TotalBillAmount = (double)DALHelper.HandleDBNull(reader["TotalBillAmount"]);
                        objVO.TotalConcessionAmount = (double)DALHelper.HandleDBNull(reader["TotalConcessionAmount"]);
                        objVO.NetBillAmount = (double)DALHelper.HandleDBNull(reader["NetBillAmount"]);
                        objVO.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                        objVO.Opd_Ipd_External_No = (string)DALHelper.HandleDBNull(reader["OPDNO"]);
                        objVO.IsFreezed = (bool)DALHelper.HandleDBNull(reader["IsFreezed"]);
                        objVO.VisitTypeID = (long)DALHelper.HandleDBNull(reader["VisitTypeID"]);
                        objVO.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                        objVO.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                        objVO.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                        objVO.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                        objVO.BalanceAmountSelf = (double)DALHelper.HandleDBNull(reader["BalanceAmountSelf"]);
                        objVO.SelfAmount = (double)DALHelper.HandleDBNull(reader["SelfAmount"]);
                        objVO.BillPaymentType = (BillPaymentTypes)((Int16)DALHelper.HandleDBNull(reader["BillPaymentType"]));
                        //Added by priyanka- for refund services
                        objVO.TotalRefund = (double)DALHelper.HandleDBNull(reader["RefundAmount"]);
                        BizActionObj.List.Add(objVO);
                        //objVO.PaidAmountSelf
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");

                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return valueObject;
        }
        // BY BHUSHAN . . . . . . . . . . 
        public override IValueObject GetBillSearch_USG_List_DashBoard(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetBillSearch_USG_List_DashBoardBizActionVO BizActionObj = valueObject as clsGetBillSearch_USG_List_DashBoardBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetBillListForSearch_DashBoard");
                DbDataReader reader;

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "MRNO", DbType.String, BizActionObj.MRNO);


                dbServer.AddInParameter(command, "BillType", DbType.Int32, BizActionObj.BillType);
                dbServer.AddInParameter(command, "UnitID", DbType.String, BizActionObj.UnitID);

                if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                    dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(BizActionObj.FirstName));

                if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(BizActionObj.LastName));

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
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsBillVO>();
                    while (reader.Read())
                    {
                        clsBillVO objVO = new clsBillVO();

                        objVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        objVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        objVO.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);
                        objVO.IsCancelled = (bool)DALHelper.HandleDBNull(reader["IsCancelled"]);
                        objVO.BillNo = (string)DALHelper.HandleDBNull(reader["BillNo"]);
                        objVO.MRNO = (string)DALHelper.HandleDBNull(reader["MRNO"]);
                        objVO.BillType = (BillTypes)((Int16)DALHelper.HandleDBNull(reader["BillType"]));
                        //objVO.Opd_Ipd_External = (Int16)DALHelper.HandleDBNull(reader["Opd_Ipd_External"]);
                        //objVO.Opd_Ipd_External_Id = (long)DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"]);
                        //objVO.Opd_Ipd_External_UnitId = (long)DALHelper.HandleDBNull(reader["Opd_Ipd_External_UnitId"]);
                        objVO.TotalBillAmount = (double)DALHelper.HandleDBNull(reader["TotalBillAmount"]);
                        objVO.TotalConcessionAmount = (double)DALHelper.HandleDBNull(reader["TotalConcessionAmount"]);
                        objVO.NetBillAmount = (double)DALHelper.HandleDBNull(reader["NetBillAmount"]);
                        objVO.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                        objVO.Opd_Ipd_External_No = (string)DALHelper.HandleDBNull(reader["OPDNO"]);
                        objVO.IsFreezed = (bool)DALHelper.HandleDBNull(reader["IsFreezed"]);
                        objVO.VisitTypeID = (long)DALHelper.HandleDBNull(reader["VisitTypeID"]);
                        objVO.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                        objVO.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                        objVO.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                        objVO.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                        objVO.BalanceAmountSelf = (double)DALHelper.HandleDBNull(reader["BalanceAmountSelf"]);
                        objVO.SelfAmount = (double)DALHelper.HandleDBNull(reader["SelfAmount"]);
                        objVO.BillPaymentType = (BillPaymentTypes)((Int16)DALHelper.HandleDBNull(reader["BillPaymentType"]));
                        //Added by priyanka- for refund services
                        objVO.TotalRefund = (double)DALHelper.HandleDBNull(reader["RefundAmount"]);
                        BizActionObj.List.Add(objVO);
                        //objVO.PaidAmountSelf
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");

                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return valueObject;
        }

        public override IValueObject AddDoctorShareRange(IValueObject valueObject, clsUserVO UserVO)
        {
            clsAddDoctorShareRangeBizActionVO BizActionobj = valueObject as clsAddDoctorShareRangeBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                clsDoctorShareRangeVO ObjShareRangeVO = BizActionobj.ShareRangeDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddDoctorShareRangeMaster");
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, ObjShareRangeVO.UnitID);
                dbServer.AddInParameter(command, "UpperLimit", DbType.Decimal, ObjShareRangeVO.UpperLimit);
                dbServer.AddInParameter(command, "LowerLimit", DbType.Decimal, ObjShareRangeVO.LowerLimit);
                dbServer.AddInParameter(command, "SharePercentage", DbType.Decimal, ObjShareRangeVO.SharePercentage);
                dbServer.AddInParameter(command, "ShareAmount", DbType.Decimal, ObjShareRangeVO.ShareAmount);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, int.MaxValue);
                int intStatus2 = dbServer.ExecuteNonQuery(command);
                BizActionobj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
            }
            catch (Exception ex)
            {
                BizActionobj.SuccessStatus = -1;
                trans.Rollback();
                BizActionobj.ShareRangeDetails = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return BizActionobj;
        }

        public override IValueObject GetDoctorShareRangeList(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetDoctorShareRangeList BizActionObj = valueObject as clsGetDoctorShareRangeList;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorShareRangeList");
                DbDataReader reader;
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ShareRangeList == null)
                        BizActionObj.ShareRangeList = new List<clsDoctorShareRangeVO>();
                    while (reader.Read())
                    {
                        clsDoctorShareRangeVO objVO = new clsDoctorShareRangeVO();
                        objVO.ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        objVO.UnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"]));
                        objVO.UpperLimit = Convert.ToDecimal(DALHelper.HandleDBNull(reader["UpperLimit"]));
                        objVO.LowerLimit = Convert.ToDecimal(DALHelper.HandleDBNull(reader["LowerLimit"]));
                        objVO.SharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SharePercentage"]));
                        objVO.ShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ShareAmount"]));
                        objVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        BizActionObj.ShareRangeList.Add(objVO);
                    }
                }
                reader.NextResult();
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

        public override IValueObject ChangeStatus(IValueObject valueObject, clsUserVO UserVO)
        {
            clsAddDoctorShareRangeBizActionVO BizActionobj = valueObject as clsAddDoctorShareRangeBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                clsDoctorShareRangeVO ObjShareRangeVO = BizActionobj.ShareRangeDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_ChangeDoctorShareRangeMasterStatus");
                dbServer.AddInParameter(command, "ID", DbType.Int64, ObjShareRangeVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, ObjShareRangeVO.UnitID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, ObjShareRangeVO.Status);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, int.MaxValue);
                int intStatus2 = dbServer.ExecuteNonQuery(command);
                BizActionobj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
            }
            catch (Exception ex)
            {
                BizActionobj.SuccessStatus = -1;
                trans.Rollback();
                BizActionobj.ShareRangeDetails = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return BizActionobj;
        }

        public override IValueObject GetPharmacyBillSearchList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsPreviousPatientBillsBizActionVO BizActionObj = valueObject as clsPreviousPatientBillsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPharmacyBillListForSearch");
                DbDataReader reader;

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                // Added By Rohit
                dbServer.AddInParameter(command, "IsPharmacyQueue", DbType.Boolean, BizActionObj.IsPharmacyQueue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsBillVO>();
                    while (reader.Read())
                    {
                        clsBillVO objVO = new clsBillVO();
                        if (!BizActionObj.IsPharmacyQueue)
                        {
                            objVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                            objVO.IsCancelled = (bool)DALHelper.HandleDBNull(reader["IsCancelled"]);
                            objVO.Opd_Ipd_External = (Int16)DALHelper.HandleDBNull(reader["Opd_Ipd_External"]);
                            objVO.BalanceAmountSelf = (double)DALHelper.HandleDBNull(reader["BalanceAmountSelf"]);
                            objVO.SelfAmount = (double)DALHelper.HandleDBNull(reader["SelfAmount"]);
                            //objVO.GrossDiscountReasonID = (long)DALHelper.HandleIntegerNull(reader["GrossDiscountReasonID"]);
                            //Added by priyanka- for refund services
                            objVO.TotalRefund = (double)DALHelper.HandleDBNull(reader["RefundAmount"]);
                            //objVO.IsComanyBillCancelled = (bool)DALHelper.HandleBoolDBNull(reader["IsCompanyBillCancelled"]);
                            objVO.PatientSourceId = (long)DALHelper.HandleIntegerNull(reader["PatientSourceId"]);
                            objVO.PatientCategoryId = (long)DALHelper.HandleIntegerNull(reader["PatientCategoryId"]);
                        }
                        if (BizActionObj.IsPharmacyQueue)
                        {
                            //objVO.IsDelivered = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsDelivered"]));
                            //objVO.ItemSaleId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemSaleId"]));
                        }

                        objVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        objVO.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);
                        objVO.BillNo = (string)DALHelper.HandleDBNull(reader["BillNo"]);
                        objVO.MRNO = (string)DALHelper.HandleDBNull(reader["MRNO"]);
                        objVO.BillType = (BillTypes)((Int16)DALHelper.HandleDBNull(reader["BillType"]));
                        objVO.Opd_Ipd_External_Id = (long)DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"]);
                        objVO.Opd_Ipd_External_UnitId = (long)DALHelper.HandleDBNull(reader["Opd_Ipd_External_UnitId"]);
                        objVO.TotalBillAmount = (double)DALHelper.HandleDBNull(reader["TotalBillAmount"]);
                        objVO.TotalConcessionAmount = (double)DALHelper.HandleDBNull(reader["TotalConcessionAmount"]);
                        objVO.NetBillAmount = (double)DALHelper.HandleDBNull(reader["NetBillAmount"]);
                        objVO.Opd_Ipd_External_No = (string)DALHelper.HandleDBNull(reader["OPDNO"]);
                        objVO.IsFreezed = (bool)DALHelper.HandleDBNull(reader["IsFreezed"]);
                        objVO.VisitTypeID = (long)DALHelper.HandleIntegerNull(reader["VisitTypeID"]);
                        objVO.PatientID = (long)DALHelper.HandleIntegerNull(reader["PatientID"]);
                        objVO.FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]));
                        objVO.MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"]));
                        objVO.LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]));
                        objVO.BillPaymentType = (BillPaymentTypes)((Int16)DALHelper.HandleDBNull(reader["BillPaymentType"]));
                        //objVO.GrossDiscountPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["GrossDiscountPercentage"]));
                        ////objVO.TokenNo = Convert.ToInt32(DALHelper.HandleDBNull(reader["TokenNo"]));
                        ////objVO.IsReady = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsReady"]));
                        BizActionObj.List.Add(objVO);
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

        #region For IPD Module

        public override IValueObject FillGrossDiscountReason(IValueObject valueObject, clsUserVO UserVo)
        {
            clsFillGrossDiscountReasonBizActionVO BizActionObj = valueObject as clsFillGrossDiscountReasonBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_FillDiscountReasonMaster");
                DbDataReader reader;

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.MasterList == null)
                        BizActionObj.MasterList = new List<MasterListItem>();
                    while (reader.Read())
                    {
                        MasterListItem objVO = new MasterListItem();
                        objVO.ID = Convert.ToInt64(reader["ID"]);
                        objVO.Description = Convert.ToString(reader["Description"]);
                        objVO.Value = Convert.ToDouble(reader["DiscountPercentage"]);
                        BizActionObj.MasterList.Add(objVO);
                    }
                }

                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }


            return BizActionObj;
        }

        #endregion

        // Added By CDS
        public override IValueObject UpdateBillPaymentDetailsWithTransaction(IValueObject valueObject, clsUserVO UserVo, DbTransaction pTransaction, DbConnection pConnection)
        {

            // throw new NotImplementedException();
            //DbTransaction trans = null;
            //DbConnection con = null;

            clsUpdateBillPaymentDtlsBizActionVO BizAction = valueObject as clsUpdateBillPaymentDtlsBizActionVO;
            try
            {
                //con = dbServer.CreateConnection();
                //if (con.State == ConnectionState.Closed) con.Open();
                if (pConnection == null)
                    pConnection = dbServer.CreateConnection();

                //trans = con.BeginTransaction();

                if (pConnection.State != ConnectionState.Open) pConnection.Open();
                if (pTransaction == null)
                    pTransaction = pConnection.BeginTransaction();

                clsBillVO objDetailsVO = BizAction.Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateBillPaymentDetails");
                command.Connection = pConnection;

                dbServer.AddInParameter(command, "BillID", DbType.Int64, objDetailsVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDetailsVO.UnitID);
                // Added by CDS
                dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);

                if (objDetailsVO.BalanceAmountSelf < 0) objDetailsVO.BalanceAmountSelf = 0;
                dbServer.AddInParameter(command, "BalanceAmountSelf", DbType.Double, objDetailsVO.BalanceAmountSelf);
                dbServer.AddInParameter(command, "TotalConcessionAmount", DbType.Double, objDetailsVO.TotalConcessionAmount);
                dbServer.AddInParameter(command, "NetBillAmount", DbType.Double, objDetailsVO.NetBillAmount);
                //Added by AJ Date 7/2/2017                
                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, objDetailsVO.IsFreezed);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, pTransaction); // int intStatus = dbServer.ExecuteNonQuery(command, trans);

                BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                # region commented

                if (BizAction.ChargeDetails != null && BizAction.ChargeDetails.Count > 0)
                {
                    foreach (var item in BizAction.ChargeDetails)
                    {
                        item.ChargeDetails = new clsChargeDetailsVO();
                        //Add service details in t_charge
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_UpdateCharge");


                        dbServer.AddInParameter(command1, "ID", DbType.Int64, item.ID);
                        dbServer.AddInParameter(command1, "Date", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        // Added by CDS
                        dbServer.AddInParameter(command1, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);

                        dbServer.AddInParameter(command1, "PaidAmount", DbType.Double, item.TotalServicePaidAmount);
                        dbServer.AddInParameter(command1, "NetAmount", DbType.Double, item.TotalNetAmount);
                        dbServer.AddInParameter(command1, "Concession", DbType.Double, item.TotalConcession);
                        dbServer.AddInParameter(command1, "ConcessionPercentage", DbType.Double, item.TotalConcessionPercentage);

                        //                        int iStatus = dbServer.ExecuteNonQuery(command1, trans);
                        int iStatus = dbServer.ExecuteNonQuery(command1, pTransaction);
                        item.ID = (long)dbServer.GetParameterValue(command1, "ID");


                        if (item.IsUpdate == true)
                        {
                            DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_UpdateChargeDetails");


                            dbServer.AddInParameter(command3, "ChargeID ", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command3, "Date", DbType.DateTime, DateTime.Now);
                            dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command3, "PaidAmount", DbType.Double, item.ServicePaidAmount);
                            dbServer.AddInParameter(command3, "Concession", DbType.Double, item.Concession);
                            dbServer.AddInParameter(command3, "ServiceTaxAmount", DbType.Double, item.ServiceTaxAmount);
                            dbServer.AddInParameter(command3, "NetAmount", DbType.Double, item.SettleNetAmount);
                            dbServer.AddInParameter(command3, "BalanceAmount", DbType.Double, item.BalanceAmount);
                            dbServer.AddInParameter(command3, "IsSameDate", DbType.Boolean, item.IsSameDate);

                            //int iStatus4 = dbServer.ExecuteNonQuery(command3, trans);
                            int iStatus4 = dbServer.ExecuteNonQuery(command3, pTransaction);

                        }
                        else
                        {

                            //Add service payment details in t_chargeDetails
                            DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddChargeDetails");
                            dbServer.AddInParameter(command2, "ChargeID", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command2, "ChargeUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                            dbServer.AddInParameter(command2, "Date", DbType.DateTime, DateTime.Now);
                            dbServer.AddInParameter(command2, "Quantity", DbType.Double, item.Quantity);
                            dbServer.AddInParameter(command2, "Rate", DbType.Double, item.Rate);
                            dbServer.AddInParameter(command2, "TotalAmount", DbType.Double, item.TotalAmount);
                            dbServer.AddInParameter(command2, "ConcessionAmount", DbType.Double, item.Concession);
                            dbServer.AddInParameter(command2, "ServiceTaxAmount", DbType.Double, item.ServiceTaxAmount);

                            dbServer.AddInParameter(command2, "NetAmount", DbType.Double, item.SettleNetAmount);
                            dbServer.AddInParameter(command2, "PaidAmount", DbType.Double, item.ServicePaidAmount);
                            dbServer.AddInParameter(command2, "BalanceAmount", DbType.Double, item.BalanceAmount);

                            dbServer.AddInParameter(command2, "Status", DbType.Boolean, item.Status);
                            dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                            dbServer.AddInParameter(command2, "RefundID", DbType.Int64, item.RefundID);
                            dbServer.AddInParameter(command2, "RefundAmount", DbType.Double, item.RefundAmount);


                            dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ChargeDetails.ID);
                            dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);

                            //                            int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                            int intStatus2 = dbServer.ExecuteNonQuery(command2, pTransaction);
                            item.ChargeDetails.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                            item.ChargeDetails.ID = (long)dbServer.GetParameterValue(command2, "ID");
                        }
                    }
                }
                //END
                # endregion

                //trans.Commit();
                BizAction.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizAction.SuccessStatus = -1;
                //trans.Rollback();
                BizAction.ChargeDetails = null;
                BizAction.ChargeDetails = null;

                //throw;
            }
            //finally
            //{
            //    con.Close();
            //    con = null;
            //    trans = null;

            //}

            return BizAction;

        }


        // Added By CDS For Package Discount

        public override IValueObject ApplyPackageDiscountRateOnService(IValueObject valueObject, clsUserVO UserVo)
        {
            clsApplyPackageDiscountRateOnServiceBizActionVO BizActionObj = valueObject as clsApplyPackageDiscountRateOnServiceBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_ApplyPackageDiscountRateOnServiceForPackageMillan");          //CIMS_ApplyPackageDiscountRateOnServiceForPackage4          //CIMS_ApplyPackageDiscountRateOnServiceForPackage3  //CIMS_ApplyPackageDiscountRateOnServiceForPackage2 //CIMS_ApplyPackageDiscountRateOnService
                DbDataReader reader;

                if (BizActionObj.objApplyPackageDiscountRate == null)
                    BizActionObj.objApplyPackageDiscountRate = new List<clsApplyPackageDiscountRateOnServiceVO>();


                if (BizActionObj.ipServiceList == null)
                    BizActionObj.ipServiceList = new List<clsServiceMasterVO>();

                //StringBuilder sbServiceList = new StringBuilder();
                //StringBuilder sbTariffList = new StringBuilder();

                //foreach (clsServiceMasterVO item in BizActionObj.ipServiceList)
                //{
                //    sbServiceList.Append("," + Convert.ToString(item.ID));
                //    sbTariffList.Append("," + Convert.ToString(item.TariffID));
                //}

                //sbServiceList.Replace(",", "", 0, 1);
                //sbTariffList.Replace(",", "", 0, 1);

                StringBuilder sbServiceList = new StringBuilder();
                StringBuilder sbTariffList = new StringBuilder();
                StringBuilder sbPackageList = new StringBuilder();
                StringBuilder sbParentList = new StringBuilder();
                StringBuilder sbProcessList = new StringBuilder();      // Package New Changes for Process Added on 20042018

                foreach (clsServiceMasterVO item in BizActionObj.ipServiceList)
                {
                    sbServiceList.Append("," + Convert.ToString(item.ID));
                    sbTariffList.Append("," + Convert.ToString(item.TariffID));
                    sbPackageList.Append("," + Convert.ToString(item.PackageID));
                    sbParentList.Append("," + Convert.ToString(item.ChargeID));
                    sbProcessList.Append("," + Convert.ToString(item.ProcessID));       // Package New Changes for Process Added on 20042018
                }

                sbServiceList.Replace(",", "", 0, 1);
                sbTariffList.Replace(",", "", 0, 1);
                sbPackageList.Replace(",", "", 0, 1);
                sbParentList.Replace(",", "", 0, 1);
                sbProcessList.Replace(",", "", 0, 1);       // Package New Changes for Process Added on 20042018

                dbServer.AddInParameter(command, "ipLoginUnitID", DbType.Int64, BizActionObj.ipLoginUnitID);

                dbServer.AddInParameter(command, "ipPatientID", DbType.Int64, BizActionObj.ipPatientID);
                dbServer.AddInParameter(command, "ipPatientUnitID", DbType.Int64, BizActionObj.ipPatientUnitID);

                dbServer.AddInParameter(command, "ipVisitID", DbType.Int64, BizActionObj.ipVisitID);
                dbServer.AddInParameter(command, "IsIPD", DbType.Boolean, BizActionObj.IsIPD);

                dbServer.AddInParameter(command, "ipServiceList", DbType.String, Convert.ToString(sbServiceList));
                dbServer.AddInParameter(command, "ipTariffList", DbType.String, Convert.ToString(sbTariffList));
                dbServer.AddInParameter(command, "ipPackageList", DbType.String, Convert.ToString(sbPackageList));
                dbServer.AddInParameter(command, "ipParentList", DbType.String, Convert.ToString(sbParentList));
                dbServer.AddInParameter(command, "ipProcessList", DbType.String, Convert.ToString(sbProcessList));      // Package New Changes for Process Added on 20042018

                dbServer.AddInParameter(command, "ipPatientGenderID", DbType.Int64, BizActionObj.ipPatientGenderID);
                dbServer.AddInParameter(command, "ipPatientDateOfBirth", DbType.DateTime, BizActionObj.ipPatientDateOfBirth);

                dbServer.AddInParameter(command, "MemberRelationID", DbType.Int64, BizActionObj.MemberRelationID);

                dbServer.AddInParameter(command, "SponsorID", DbType.Int64, BizActionObj.SponsorID);
                dbServer.AddInParameter(command, "SponsorUnitID", DbType.Int64, BizActionObj.SponsorUnitID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {

                        clsApplyPackageDiscountRateOnServiceVO objVO = new clsApplyPackageDiscountRateOnServiceVO();

                        objVO.DiscountedPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountedPercentage"]));
                        objVO.DiscountedRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountedRate"]));
                        objVO.GrossDiscountID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GrossDiscountID"]));
                        objVO.IsApplyOn_Rate_Percentage = Convert.ToInt32(DALHelper.HandleDBNull(reader["IsApplyOn_Rate_Percentage"]));
                        objVO.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        objVO.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        objVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        objVO.ServiceID_AsPackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID_AsPackageID"]));
                        objVO.TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"]));
                        objVO.PackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageID"]));
                        objVO.IsServiceItemStockAvailable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ServiceItemStockAvailable"]));

                        objVO.IsDiscountOnQuantity = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDiscountOnQuantity"]));    ////set to check whether discount is apply on qty or not

                        objVO.ActualQuantity = Convert.ToInt64(DALHelper.HandleDBNull(reader["ActualQuantity"]));
                        objVO.UsedQuantity = Convert.ToInt64(DALHelper.HandleDBNull(reader["UsedQuantity"]));

                        objVO.IsAgeApplicable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAgeApplicable"]));

                        objVO.ServiceMemberRelationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceMemberRelationID"]));

                        objVO.ConcessionPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionPercent"])); //used to get patient wise ConcessionPercentage from its package details

                        objVO.ProcessID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcessID"]));     // Package New Changes for Process Added on 20042018

                        BizActionObj.objApplyPackageDiscountRate.Add(objVO);
                    }
                }

                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }


            return BizActionObj;


        }



        public override IValueObject ApplyPackageDiscountRateOnItems(IValueObject valueObject, clsUserVO UserVo)
        {
            clsApplyPackageDiscountRateToItems BizActionObj = valueObject as clsApplyPackageDiscountRateToItems;
            try
            {
                ////By Anjali...........................................
                //// DbCommand command = dbServer.GetStoredProcCommand("CIMS_ApplyPackageDiscountRateOnItemForPackage");  //CIMS_ApplyPackageDiscountRateOnItem
                //DbCommand command = dbServer.GetStoredProcCommand("CIMS_ApplyPackageDiscountRateOnItemForPackage_ByAnjali"); // commented by Ashish Z. on dated 170616
                ////.........................................................
                ////DbCommand command = dbServer.GetStoredProcCommand("Temp_CIMS_ApplyPackageDiscountRateOnItemForPackage_ByAnjali");  // added by Ashish Z. on dated 170616

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_ApplyPackageDiscountRateOnItemForPackage_ByAnjali_II");

                DbDataReader reader;
                dbServer.AddInParameter(command, "PatientCatagoryL1", DbType.Int64, BizActionObj.objApplyItemPackageDiscountRateDetails.PatientCatagoryL1);

                dbServer.AddInParameter(command, "PatientCatagoryL2", DbType.Int64, BizActionObj.objApplyItemPackageDiscountRateDetails.PatientCatagoryL2);
                dbServer.AddInParameter(command, "PatientCatagoryL3", DbType.Int64, BizActionObj.objApplyItemPackageDiscountRateDetails.PatientCatagoryL3);

                dbServer.AddInParameter(command, "ItemIDs", DbType.String, BizActionObj.objApplyItemPackageDiscountRateDetails.ItemIDs);

                dbServer.AddInParameter(command, "ipLoginUnitID", DbType.String, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "CompanyID ", DbType.Int64, BizActionObj.objApplyItemPackageDiscountRateDetails.CompanyID);
                //By Anjali...............................................
                dbServer.AddInParameter(command, "ipPatientGenderID ", DbType.Int64, BizActionObj.objApplyItemPackageDiscountRateDetails.PatientGenderID);
                dbServer.AddInParameter(command, "ipPatientDateOfBirth ", DbType.DateTime, BizActionObj.objApplyItemPackageDiscountRateDetails.PatientDateOfBirth);
                dbServer.AddInParameter(command, "ipPatientID ", DbType.Int64, BizActionObj.objApplyItemPackageDiscountRateDetails.PatientID);
                dbServer.AddInParameter(command, "ipPatientUnitID ", DbType.Int64, BizActionObj.objApplyItemPackageDiscountRateDetails.PatientUnitID);
                dbServer.AddInParameter(command, "ipPackageID ", DbType.Int64, BizActionObj.objApplyItemPackageDiscountRateDetails.PackageID);

                //.........................................................

                #region Package Change 18042017
                dbServer.AddInParameter(command, "ipPackageBillID ", DbType.Int64, BizActionObj.objApplyItemPackageDiscountRateDetails.PackageBillID);
                dbServer.AddInParameter(command, "ipPackageBillUnitID ", DbType.Int64, BizActionObj.objApplyItemPackageDiscountRateDetails.PackageBillUnitID);
                #endregion

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.objApplyItemPackageDiscountRate == null)
                        BizActionObj.objApplyItemPackageDiscountRate = new List<clsApplyPackageDiscountRateOnItemVO>();
                    while (reader.Read())
                    {

                        clsApplyPackageDiscountRateOnItemVO objVO = new clsApplyPackageDiscountRateOnItemVO();

                        # region commented for Package Change 17042017
                        //objVO.ApplicableToAllDiscount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ApplicableToAllDiscount"]));

                        //if (objVO.ApplicableToAllDiscount > 0)
                        //{
                        //    objVO.DiscountedPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["ApplicableToAllDiscount"]));
                        //}
                        //else
                        //{
                        #endregion

                        objVO.ApplicableToAll = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ApplicableToAll"]));   //Package Change 17042017

                        objVO.DiscountedPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["Discount"]));
                        objVO.ItemId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ItemID"]));

                        //To set properties used for Package Item Discount ItemId,CategoryId,GroupId,DiscountExpiryDate wise
                        objVO.CategoryId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["CategoryId"]));
                        objVO.IsCategory = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCategory"]));

                        objVO.GroupId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["GroupId"]));
                        objVO.IsGroup = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsGroup"]));

                        //By Anjali.................................................
                        objVO.Budget = Convert.ToSingle(DALHelper.HandleDBNull(reader["Budget"]));
                        objVO.TotalBudget = Convert.ToSingle(DALHelper.HandleDBNull(reader["TotalBudget"]));
                        objVO.CalculatedBudget = Convert.ToSingle(DALHelper.HandleDBNull(reader["CalculatedBudget"]));
                        objVO.CalculatedTotalBudget = Convert.ToSingle(DALHelper.HandleDBNull(reader["CalculatedTotalBudget"]));
                        //............................................................

                        //}      // Package Change 17042017

                        BizActionObj.objApplyItemPackageDiscountRate.Add(objVO);
                    }
                }

                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }


        // Added By CDS For PackageServiceSearchFor Package

        public override IValueObject GetTariffTypeDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPackageTariffBizActionVO BizActionObj = valueObject as clsGetPackageTariffBizActionVO;
            try
            {
                DbDataReader reader;

                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_GetPackageTariffType");
                dbServer.AddInParameter(command1, "TariffID", DbType.Int64, BizActionObj.TariffID);
                reader = (DbDataReader)dbServer.ExecuteReader(command1);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizActionObj.isPackageTariff = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["PackageType"]));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }


        public override IValueObject GetPatientPackageServiceDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPackageDetailsBizActionVO BizActionObj = (clsGetPackageDetailsBizActionVO)valueObject;
            DbCommand command;
            DbDataReader reader;
            try
            {

                #region Pending Services
                //command = dbServer.GetStoredProcCommand("CIMS_GetPatientPackageServiceList");
                command = dbServer.GetStoredProcCommand("CIMS_GetPatientPackageServiceListNewForPackage");    //CIMS_GetPatientPackageServiceListNewUsedForPackage // CIMS_GetPatientPackageServiceListNewForPackage //CIMS_GetPatientPackageServiceListNew

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                if (BizActionObj.ServiceName != null && BizActionObj.ServiceName.Length != 0)
                    dbServer.AddInParameter(command, "ServiceName", DbType.String, BizActionObj.ServiceName);
                dbServer.AddInParameter(command, "Specialization", DbType.Int64, BizActionObj.Specialization);
                dbServer.AddInParameter(command, "SubSpecialization", DbType.Int64, BizActionObj.SubSpecialization);
                dbServer.AddInParameter(command, "TariffID", DbType.Int64, BizActionObj.TariffID);
                dbServer.AddInParameter(command, "PackageID", DbType.Int64, BizActionObj.PackageID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "Pending", DbType.Boolean, true);

                if (BizActionObj.SearchExpression != null && BizActionObj.SearchExpression.Length > 0)
                    dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.SearchExpression);
                //if (BizActionObj.ClassID > 0)
                //    dbServer.AddInParameter(command, "ClassID", DbType.Int64, BizActionObj.ClassID);

                dbServer.AddInParameter(command, "SponsorID", DbType.Int64, BizActionObj.SponsorID);
                dbServer.AddInParameter(command, "SponsorUnitID", DbType.Int64, BizActionObj.SponsorUnitID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                //long ServiceSetForPackage = 0;


                if (reader.HasRows)
                {
                    if (BizActionObj.PendingServiceList == null)
                        BizActionObj.PendingServiceList = new List<clsServiceMasterVO>();

                    while (reader.Read())
                    {
                        clsServiceMasterVO objServiceMasterVO = new clsServiceMasterVO();
                        objServiceMasterVO.ID = Convert.ToInt64(reader["ServiceID"]);
                        // objServiceMasterVO.TariffServiceMasterID = Convert.ToInt64(reader["ID"]);
                        objServiceMasterVO.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                        //objServiceMasterVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objServiceMasterVO.ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"]));
                        objServiceMasterVO.TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"]));
                        objServiceMasterVO.Specialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationID"]));
                        objServiceMasterVO.SubSpecialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"]));
                        //if (BizActionObj.PatientSourceType == 2) // Camp
                        //{
                        //    objServiceMasterVO.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                        //    objServiceMasterVO.ConcessionAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                        //    objServiceMasterVO.ConcessionPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                        //}
                        //else if (BizActionObj.PatientSourceType == 1)   //Loyalty
                        //{
                        //    objServiceMasterVO.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                        //    objServiceMasterVO.ConcessionAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                        //    objServiceMasterVO.ConcessionPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                        //}
                        //else
                        //{

                        objServiceMasterVO.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));

                        //Commented On 16/12/2015 By CDS 
                        //////objServiceMasterVO.SeniorCitizen = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["SeniorCitizen"]));
                        //////objServiceMasterVO.SeniorCitizenConAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SeniorCitizenConAmount"]));
                        //////objServiceMasterVO.SeniorCitizenConPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SeniorCitizenConPercent"]));
                        //////objServiceMasterVO.SeniorCitizenAge = Convert.ToInt32(DALHelper.HandleDBNull(reader["SeniorCitizenAge"]));


                        //////if (objServiceMasterVO.SeniorCitizen == true && BizActionObj.Age >= objServiceMasterVO.SeniorCitizenAge)
                        //////{
                        //////    objServiceMasterVO.ConcessionAmount = objServiceMasterVO.SeniorCitizenConAmount;
                        //////    objServiceMasterVO.ConcessionPercent = objServiceMasterVO.SeniorCitizenConPercent;
                        //////}
                        //////else
                        //////{
                        //////    objServiceMasterVO.ConcessionAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                        //////    objServiceMasterVO.ConcessionPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionPercent"]));

                        //////}
                        //END

                        //}

                        //Commented On 16/12/2015 By CDS 
                        //////objServiceMasterVO.StaffDiscount = Convert.ToBoolean(DALHelper.HandleDBNull(reader["StaffDiscount"]));
                        //////objServiceMasterVO.StaffDiscountAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDiscountAmount"]));
                        //////objServiceMasterVO.StaffDiscountPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDiscountPercent"]));
                        //////objServiceMasterVO.StaffDependantDiscount = Convert.ToBoolean(DALHelper.HandleDBNull(reader["StaffDependantDiscount"]));
                        //////objServiceMasterVO.StaffDependantDiscountAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDependantDiscountAmount"]));
                        //////objServiceMasterVO.StaffDependantDiscountPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDependantDiscountPercent"]));
                        //////objServiceMasterVO.Concession = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Concession"]));
                        //////objServiceMasterVO.ServiceTax = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ServiceTax"]));
                        //////objServiceMasterVO.ServiceTaxAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceTaxAmount"]));
                        //////objServiceMasterVO.ServiceTaxPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceTaxPercent"]));
                        //////objServiceMasterVO.InHouse = Convert.ToBoolean(DALHelper.HandleDBNull(reader["InHouse"]));
                        //////objServiceMasterVO.DoctorShare = Convert.ToBoolean(DALHelper.HandleDBNull(reader["DoctorShare"]));
                        //////objServiceMasterVO.DoctorSharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorSharePercentage"]));
                        //////objServiceMasterVO.DoctorShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorShareAmount"]));
                        //////objServiceMasterVO.RateEditable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["RateEditable"]));
                        //////objServiceMasterVO.MaxRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MaxRate"]));
                        //////objServiceMasterVO.MinRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MinRate"]));
                        //////objServiceMasterVO.TarrifCode = Convert.ToString(DALHelper.HandleDBNull(reader["TarrifServiceCode"]));
                        //////objServiceMasterVO.TarrifName = Convert.ToString(DALHelper.HandleDBNull(reader["TarrifServiceName"]));
                        //////objServiceMasterVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        //////objServiceMasterVO.CodeType = (Int64)DALHelper.HandleDBNull(reader["CodeType"]);
                        //////objServiceMasterVO.ShortDescription = Convert.ToString(DALHelper.HandleDBNull(reader["ShortDescription"]));
                        //////objServiceMasterVO.LongDescription = Convert.ToString(DALHelper.HandleDBNull(reader["LongDescription"]));
                        //END

                        objServiceMasterVO.SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["Specialization"]));
                        objServiceMasterVO.SubSpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecialization"]));
                        //Commented By CSD
                        objServiceMasterVO.IsPackage = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackage"]));
                        //Commented By CSD
                        //objServiceMasterVO.PackageID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PackageID"]));
                        objServiceMasterVO.PackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageID"]));
                        //objServiceMasterVO.IsMarkUp = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsMarkUp"])); Commented By CSD

                        ////if (BizActionObj.PendingServiceList == true)
                        ////{

                        //Commented By CSD
                        //objServiceMasterVO.ApplicableTo = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ApplicableTo"]));
                        //objServiceMasterVO.ApplicableToString = Convert.ToString(DALHelper.HandleDBNull(reader["ApplicableToString"]));
                        //Commented By CSD

                        // to set service background color to identify that this service is having Package Conditional Services

                        //Commented By CSD
                        objServiceMasterVO.PackageServiceConditionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageServiceConditionID"]));
                        //objServiceMasterVO.PackageServiceConditionID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PackageServiceConditionID"]));
                        //Commented By CSD

                        //// } 

                        //objServiceMasterVO.ActualQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
                        //objServiceMasterVO.ActualUsedQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["UsedQuantity"]));

                        ////if (BizActionObj.AvailedServiceList == null)
                        ////    BizActionObj.AvailedServiceList = new List<clsServiceMasterVO>();

                        //if (objServiceMasterVO.ActualQuantity > objServiceMasterVO.ActualUsedQuantity)
                        BizActionObj.PendingServiceList.Add(objServiceMasterVO);
                        ////else
                        ////    BizActionObj.AvailedServiceList.Add(objServiceMasterVO);

                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));

                #endregion

                #region Availed Service List
                //command = dbServer.GetStoredProcCommand("CIMS_GetPatientPackageUsedServiceList");
                command = dbServer.GetStoredProcCommand("CIMS_GetPatientPackageUsedServiceListNewForPackage");    //CIMS_GetPatientPackageUsedServiceListNew

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                //dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                //Added by AJ Date 17/11/2016
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, 0);
                //***//------------------
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                if (BizActionObj.ServiceName != null && BizActionObj.ServiceName.Length != 0)
                    dbServer.AddInParameter(command, "ServiceName", DbType.String, BizActionObj.ServiceName);
                dbServer.AddInParameter(command, "Specialization", DbType.Int64, BizActionObj.Specialization);
                dbServer.AddInParameter(command, "SubSpecialization", DbType.Int64, BizActionObj.SubSpecialization);
                dbServer.AddInParameter(command, "TariffID", DbType.Int64, BizActionObj.TariffID);

                //dbServer.AddInParameter(command, "PackageIDList", DbType.String, BizActionObj.PackageIDList);
                dbServer.AddInParameter(command, "PackageID", DbType.Int64, BizActionObj.PackageID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                //dbServer.AddInParameter(command, "Pending", DbType.Boolean, false);

                if (BizActionObj.SearchExpression != null && BizActionObj.SearchExpression.Length > 0)
                    dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.SearchExpression);

                dbServer.AddInParameter(command, "SponsorID", DbType.Int64, BizActionObj.SponsorID);
                dbServer.AddInParameter(command, "SponsorUnitID", DbType.Int64, BizActionObj.SponsorUnitID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.AvailedServiceList == null)
                        BizActionObj.AvailedServiceList = new List<clsServiceMasterVO>();
                    while (reader.Read())
                    {
                        clsServiceMasterVO objServiceMasterVO = new clsServiceMasterVO();
                        //objServiceMasterVO.ID = Convert.ToInt64(reader["ServiceID"]);
                        objServiceMasterVO.TariffServiceMasterID = Convert.ToInt64(reader["ID"]);
                        objServiceMasterVO.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                        objServiceMasterVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objServiceMasterVO.ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"]));
                        objServiceMasterVO.TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"]));
                        objServiceMasterVO.Specialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationID"]));
                        objServiceMasterVO.SubSpecialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"]));
                        objServiceMasterVO.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                        objServiceMasterVO.SeniorCitizen = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["SeniorCitizen"]));
                        objServiceMasterVO.SeniorCitizenConAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SeniorCitizenConAmount"]));
                        objServiceMasterVO.SeniorCitizenConPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SeniorCitizenConPercent"]));
                        objServiceMasterVO.SeniorCitizenAge = Convert.ToInt32(DALHelper.HandleDBNull(reader["SeniorCitizenAge"]));
                        if (objServiceMasterVO.SeniorCitizen == true && BizActionObj.Age >= objServiceMasterVO.SeniorCitizenAge)
                        {
                            objServiceMasterVO.ConcessionAmount = objServiceMasterVO.SeniorCitizenConAmount;
                            objServiceMasterVO.ConcessionPercent = objServiceMasterVO.SeniorCitizenConPercent;
                        }
                        else
                        {
                            objServiceMasterVO.ConcessionAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                            objServiceMasterVO.ConcessionPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                        }
                        objServiceMasterVO.StaffDiscount = Convert.ToBoolean(DALHelper.HandleDBNull(reader["StaffDiscount"]));
                        objServiceMasterVO.StaffDiscountAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDiscountAmount"]));
                        objServiceMasterVO.StaffDiscountPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDiscountPercent"]));
                        objServiceMasterVO.StaffDependantDiscount = Convert.ToBoolean(DALHelper.HandleDBNull(reader["StaffDependantDiscount"]));
                        objServiceMasterVO.StaffDependantDiscountAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDependantDiscountAmount"]));
                        objServiceMasterVO.StaffDependantDiscountPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDependantDiscountPercent"]));
                        objServiceMasterVO.Concession = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Concession"]));
                        objServiceMasterVO.ServiceTax = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ServiceTax"]));
                        objServiceMasterVO.ServiceTaxAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceTaxAmount"]));
                        objServiceMasterVO.ServiceTaxPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceTaxPercent"]));
                        objServiceMasterVO.InHouse = Convert.ToBoolean(DALHelper.HandleDBNull(reader["InHouse"]));
                        //objServiceMasterVO.DoctorShare = Convert.ToBoolean(DALHelper.HandleDBNull(reader["DoctorShare"]));
                        //objServiceMasterVO.DoctorSharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorSharePercentage"]));
                        //objServiceMasterVO.DoctorShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorShareAmount"]));


                        objServiceMasterVO.RateEditable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["RateEditable"]));
                        objServiceMasterVO.MaxRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MaxRate"]));
                        objServiceMasterVO.MinRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MinRate"]));
                        objServiceMasterVO.TarrifCode = Convert.ToString(DALHelper.HandleDBNull(reader["TarrifServiceCode"]));
                        objServiceMasterVO.TarrifName = Convert.ToString(DALHelper.HandleDBNull(reader["TarrifServiceName"]));


                        //objServiceMasterVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        //objServiceMasterVO.CodeType = (Int64)DALHelper.HandleDBNull(reader["CodeType"]);
                        //objServiceMasterVO.ShortDescription = Convert.ToString(DALHelper.HandleDBNull(reader["ShortDescription"]));
                        //objServiceMasterVO.LongDescription = Convert.ToString(DALHelper.HandleDBNull(reader["LongDescription"]));
                        objServiceMasterVO.SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["Specialization"]));
                        objServiceMasterVO.SubSpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecialization"]));
                        objServiceMasterVO.IsPackage = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackage"]));
                        //objServiceMasterVO.PackageID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PackageID"]));
                        objServiceMasterVO.PackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageID"]));

                        //Commented By CSD
                        //objServiceMasterVO.IsMarkUp = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsMarkUp"]));
                        //objServiceMasterVO.ApplicableTo = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ApplicableTo"]));
                        //objServiceMasterVO.ApplicableToString = Convert.ToString(DALHelper.HandleDBNull(reader["ApplicableToString"]));
                        //Commented By CSD

                        // to set service background color to identify that this service is having Package Conditional Services
                        //objServiceMasterVO.PackageServiceConditionID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PackageServiceConditionID"]));
                        objServiceMasterVO.PackageServiceConditionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageServiceConditionID"]));
                        objServiceMasterVO.ExpiryDate = Convert.ToDateTime(Convert.ToDateTime(DALHelper.HandleDate(reader["UsedDate"])).ToShortDateString());

                        objServiceMasterVO.UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["SerConUnitName"]));
                        objServiceMasterVO.Patient_Name = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));

                        objServiceMasterVO.Package_Name = Convert.ToString(DALHelper.HandleDBNull(reader["PackageName"]));

                        BizActionObj.AvailedServiceList.Add(objServiceMasterVO);
                    }
                }
                #endregion
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }
        public override IValueObject GetBillListForRequestApproval(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetBillSearchListBizActionVO BizActionObj = valueObject as clsGetBillSearchListBizActionVO;
            try
            {
                DbCommand command;

                command = dbServer.GetStoredProcCommand("CIMS_GetBillListForRequestApprovalWindow");

                DbDataReader reader;

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                if (BizActionObj.IsFreeze.HasValue)
                    dbServer.AddInParameter(command, "FreezeBill", DbType.Boolean, BizActionObj.IsFreeze);
                dbServer.AddInParameter(command, "Opd_Ipd_External_Id", DbType.Int64, BizActionObj.Opd_Ipd_External_Id);
                dbServer.AddInParameter(command, "Opd_Ipd_External", DbType.Int64, BizActionObj.Opd_Ipd_External);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "OPDNO", DbType.String, BizActionObj.OPDNO);
                dbServer.AddInParameter(command, "ContactNo", DbType.String, BizActionObj.ContactNo);
                dbServer.AddInParameter(command, "BillNO", DbType.String, BizActionObj.BillNO);
                dbServer.AddInParameter(command, "MRNO", DbType.String, BizActionObj.MRNO);
                dbServer.AddInParameter(command, "BillSettleType", DbType.Int16, BizActionObj.BillStatus);
                dbServer.AddInParameter(command, "FromRefund", DbType.Boolean, BizActionObj.FromRefund);
                dbServer.AddInParameter(command, "IsOPDBill", DbType.Boolean, BizActionObj.IsOPDBill);
                if (BizActionObj.BillType.HasValue)
                    dbServer.AddInParameter(command, "BillType", DbType.Int16, (Int16)BizActionObj.BillType);
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
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsBillVO>();
                    while (reader.Read())
                    {
                        clsBillVO objVO = new clsBillVO();

                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objVO.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        objVO.IsCancelled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCancelled"]));
                        objVO.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                        objVO.MRNO = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"]));
                        objVO.ContactNo = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo1"]));
                        objVO.BillType = (BillTypes)(Convert.ToInt16(DALHelper.HandleDBNull(reader["BillType"])));
                        objVO.Opd_Ipd_External = Convert.ToInt16(DALHelper.HandleDBNull(reader["Opd_Ipd_External"]));
                        objVO.Opd_Ipd_External_Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"]));
                        objVO.Opd_Ipd_External_UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_UnitId"]));
                        objVO.TotalBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalBillAmount"]));
                        objVO.TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalConcessionAmount"]));
                        objVO.NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"]));
                        objVO.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        if (BizActionObj.IsOPDBill == true)
                        {
                            objVO.VisitTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitTypeID"]));
                            objVO.Opd_Ipd_External_No = Convert.ToString(DALHelper.HandleDBNull(reader["OPDNO"]));
                        }
                        else
                        {
                            objVO.AdmissionType = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionType"]));
                            objVO.Opd_Ipd_External_No = Convert.ToString(DALHelper.HandleDBNull(reader["IPDNO"]));
                        }

                        objVO.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        objVO.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        objVO.FirstName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])));
                        objVO.MiddleName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"])));
                        objVO.LastName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])));
                        objVO.BalanceAmountSelf = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmountSelf"]));
                        objVO.SelfAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["SelfAmount"]));
                        objVO.PatientSourceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceId"]));
                        objVO.CostingDivisionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CostingDivisionID"]));
                        objVO.RequestType = Convert.ToString(DALHelper.HandleDBNull(reader["RequestType"]));
                        objVO.RequestTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RequestTypeID"]));
                        objVO.ApprovalRequestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalRequestID"]));
                        objVO.ApprovalRequestUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalRequestUnitID"]));
                        objVO.LevelID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LevelID"]));
                        objVO.ApprovalRemark = Convert.ToString(DALHelper.HandleDBNull(reader["ApprovalRemark"]));
                        objVO.IsOPDBill = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsOPDBill"]));

                        objVO.ConcessionReasonId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConcessionReasonId"]));  // Added By Yogesh K 27042016
                        objVO.ConcessionRemark = Convert.ToString(DALHelper.HandleDBNull(reader["ConcessionRemark"]));
                        BizActionObj.List.Add(objVO);

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


        public override IValueObject AddPharmacyBill(IValueObject valueObject, clsUserVO UserVo)
        {
            bool StatusVar = false;
            clsAddBillBizActionVO BizActionObj = valueObject as clsAddBillBizActionVO;

            if (BizActionObj.Details.IsFreezed == true)
            {
                if (BizActionObj.Details.ChargeDetails != null && BizActionObj.Details.ChargeDetails.Count > 0
                 && BizActionObj.Details.PharmacyItems != null && BizActionObj.Details.PharmacyItems.Items != null && BizActionObj.Details.PharmacyItems.Items.Count > 0)
                    BizActionObj.Details.BillType = BillTypes.Clinical_Pharmacy;
            }

            DbTransaction trans = null;
            DbConnection con = null;

            try
            {

                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                if (BizActionObj.objPatientVODetails != null)
                {
                    clsBasePatientDAL objBaseDAL = clsBasePatientDAL.GetInstance();
                    clsAddPatientBizActionVO obj = new clsAddPatientBizActionVO();
                    obj = BizActionObj.objPatientVODetails;
                    obj = (clsAddPatientBizActionVO)objBaseDAL.AddPatientOPDWithTransaction(obj, UserVo, con, trans);
                    if (obj.SuccessStatus == -1) throw new Exception();
                    if (BizActionObj.objVisitVODetails != null)
                    {
                        BizActionObj.objVisitVODetails.VisitDetails.PatientId = obj.PatientDetails.GeneralDetails.PatientID;
                        BizActionObj.objVisitVODetails.VisitDetails.PatientUnitId = obj.PatientDetails.GeneralDetails.UnitId;
                    }
                    BizActionObj.Details.PatientID = obj.PatientDetails.GeneralDetails.PatientID;
                    BizActionObj.Details.PatientUnitID = obj.PatientDetails.GeneralDetails.UnitId;
                    if (BizActionObj.Details.PharmacyItems != null)
                    {
                        BizActionObj.Details.PharmacyItems.PatientID = obj.PatientDetails.GeneralDetails.PatientID;
                        BizActionObj.Details.PharmacyItems.PatientUnitID = obj.PatientDetails.GeneralDetails.UnitId;
                    }
                }
                if (BizActionObj.objVisitVODetails != null)
                {
                    clsBaseVisitDAL objBaseDAL = clsBaseVisitDAL.GetInstance();
                    clsAddVisitBizActionVO obj = new clsAddVisitBizActionVO();
                    obj = BizActionObj.objVisitVODetails;
                    obj = (clsAddVisitBizActionVO)objBaseDAL.AddVisit(obj, UserVo, con, trans);
                    if (obj.SuccessStatus == -1) throw new Exception();
                    BizActionObj.Details.Opd_Ipd_External_Id = obj.VisitDetails.ID;
                    BizActionObj.Details.Opd_Ipd_External_UnitId = UserVo.UserLoginInfo.UnitId;
                    if (BizActionObj.Details.PharmacyItems != null)
                        BizActionObj.Details.PharmacyItems.VisitID = obj.VisitDetails.ID;

                }


                clsBaseBillDAL objBaseDAL1 = clsBaseBillDAL.GetInstance();
                clsAddBillBizActionVO obj1 = new clsAddBillBizActionVO();
                obj1 = BizActionObj;
                obj1 = (clsAddBillBizActionVO)objBaseDAL1.Add(obj1, UserVo, con, trans);
                if (obj1.SuccessStatus == -1)
                {
                    StatusVar = true;
                    throw new Exception();
                }
                BizActionObj.SuccessStatus = obj1.SuccessStatus;

                trans.Commit();
                BizActionObj.SuccessStatus = 0;

                if (BizActionObj.Details.PharmacyItems != null && BizActionObj.LogInfoList != null && BizActionObj.LogInfoList.Count > 0)
                {
                    foreach (LogInfo itemLog in BizActionObj.LogInfoList)
                    {
                        itemLog.Message = itemLog.Message + " , PatientID : " + Convert.ToString(BizActionObj.Details.PharmacyItems.PatientID)
                                                            + " , PatientUnitID : " + Convert.ToString(BizActionObj.Details.PharmacyItems.PatientUnitID)
                                                            + " , Opd_Ipd_External_Id : " + Convert.ToString(BizActionObj.Details.Opd_Ipd_External_Id)
                                                            + " , Opd_Ipd_External_UnitId : " + Convert.ToString(BizActionObj.Details.Opd_Ipd_External_UnitId)
                                                            + " , BillID : " + Convert.ToString(BizActionObj.Details.ID)
                                                            + " , BillNo : " + Convert.ToString(BizActionObj.Details.BillNo);
                    }
                    if (BizActionObj.LogInfoList != null)
                    {
                        if (IsAuditTrail)
                        {
                            SetLogInfo(BizActionObj.LogInfoList, UserVo.ID);
                            BizActionObj.LogInfoList.Clear();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                if (StatusVar == true)
                    BizActionObj.SuccessStatus = -10;
                else
                    BizActionObj.SuccessStatus = -1;
                trans.Rollback();

                BizActionObj.Details = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
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

        public override IValueObject AddPathologyBill(IValueObject valueObject, clsUserVO UserVo)
        {
            bool StatusVar = false;
            clsAddBillBizActionVO BizActionObj = valueObject as clsAddBillBizActionVO;

            if (BizActionObj.Details.IsFreezed == true)
            {
                if (BizActionObj.Details.ChargeDetails != null && BizActionObj.Details.ChargeDetails.Count > 0 && BizActionObj.Details.PharmacyItems != null && BizActionObj.Details.PharmacyItems.Items != null && BizActionObj.Details.PharmacyItems.Items.Count > 0)
                {
                    BizActionObj.Details.BillType = BillTypes.Clinical;
                }
            }
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {

                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                if (BizActionObj.obPathoPatientVODetails != null)
                {
                    clsBasePatientDAL objBaseDAL = clsBasePatientDAL.GetInstance();
                    clsAddPatientForPathologyBizActionVO obj = new clsAddPatientForPathologyBizActionVO();
                    obj = BizActionObj.obPathoPatientVODetails;
                    //added by rohini dated 19.5.16 for patient not display in bill list
                    obj.PatientDetails.IsVisitForPatho = false;
                    obj = (clsAddPatientForPathologyBizActionVO)objBaseDAL.AddPatientForPathology(obj, UserVo, con, trans);
                    if (obj.SuccessStatus == -1) throw new Exception();
                    //if (BizActionObj.obPathoPatientVisitVODetails != null)
                    //{
                    //    BizActionObj.obPathoPatientVisitVODetails.VisitDetails.PatientId = obj.PatientDetails.GeneralDetails.PatientID;
                    //    BizActionObj.obPathoPatientVisitVODetails.VisitDetails.PatientUnitId = obj.PatientDetails.GeneralDetails.UnitId;
                    //}
                    BizActionObj.Details.PatientID = obj.PatientDetails.GeneralDetails.PatientID;
                    BizActionObj.Details.PatientUnitID = obj.PatientDetails.GeneralDetails.UnitId;
                    BizActionObj.Details.Opd_Ipd_External_Id = obj.BizActionVOSaveVisit.VisitDetails.ID;
                    BizActionObj.Details.Opd_Ipd_External_UnitId = obj.BizActionVOSaveVisit.VisitDetails.UnitId;
                    //if (BizActionObj.Details.PharmacyItems != null)
                    //{
                    //    BizActionObj.Details.PharmacyItems.PatientID = obj.PatientDetails.GeneralDetails.PatientID;
                    //    BizActionObj.Details.PharmacyItems.PatientUnitID = obj.PatientDetails.GeneralDetails.UnitId;
                    //}                    

                }
                if (BizActionObj.obPathoPatientVisitVODetails != null)
                {
                    clsBaseVisitDAL objBaseDAL = clsBaseVisitDAL.GetInstance();
                    clsAddVisitBizActionVO obj = new clsAddVisitBizActionVO();
                    obj = BizActionObj.obPathoPatientVisitVODetails;
                    obj = (clsAddVisitBizActionVO)objBaseDAL.AddVisit(obj, UserVo, con, trans);
                    if (obj.SuccessStatus == -1) throw new Exception();
                    BizActionObj.Details.Opd_Ipd_External_Id = obj.VisitDetails.ID;
                    BizActionObj.Details.Opd_Ipd_External_UnitId = UserVo.UserLoginInfo.UnitId;
                    if (BizActionObj.Details.PharmacyItems != null)
                        BizActionObj.Details.PharmacyItems.VisitID = obj.VisitDetails.ID;
                }

                clsBaseBillDAL objBaseDAL1 = clsBaseBillDAL.GetInstance();
                clsAddBillBizActionVO obj1 = new clsAddBillBizActionVO();
                obj1 = BizActionObj;
                obj1 = (clsAddBillBizActionVO)objBaseDAL1.Add(obj1, UserVo, con, trans);
                if (obj1.SuccessStatus == -1)
                {
                    StatusVar = true;
                    throw new Exception();
                }
                BizActionObj.SuccessStatus = obj1.SuccessStatus;

                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                if (StatusVar == true)
                    BizActionObj.SuccessStatus = -10;
                else
                    BizActionObj.SuccessStatus = -1;
                trans.Rollback();

                BizActionObj.Details = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return BizActionObj;
        }

        public void UpdatePathOrderBookingDetailStatus(long BillID, long UnitID, DbConnection pConnection, DbTransaction pTransaction)
        {
            DbConnection con = null;
            DbTransaction trans = null;

            try
            {
                if (pConnection != null) con = pConnection;
                else con = dbServer.CreateConnection();

                if (con.State == ConnectionState.Closed) con.Open();

                if (pTransaction != null) trans = pTransaction;
                else trans = con.BeginTransaction();
                DbDataReader reader;
                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_UpdatePathOrderBookingDetailStatus");
                dbServer.AddInParameter(command1, "BillID", DbType.Int64, BillID);
                dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UnitID);
                int intStatus8 = dbServer.ExecuteNonQuery(command1, trans);


                //if (pConnection == null) trans.Commit();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                if (pConnection == null) trans.Rollback();
                //throw;
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
        }


        public override IValueObject DeleteIsTempCharges(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddBillBizActionVO BizActionObj = valueObject as clsAddBillBizActionVO;

            DbTransaction trans = null;  //Change by Bhushanp 056/01/2017 For Delete Pathology Service Remove commented code 
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                clsBillVO objDetailsVO = BizActionObj.Details;

                //DbCommand command8 = dbServer.GetStoredProcCommand("CIMS_UpdateChargeDetailsNew");
                //dbServer.AddInParameter(command8, "BillID", DbType.Int64, objDetailsVO.ID);
                //dbServer.AddInParameter(command8, "UnitID", DbType.Int64, objDetailsVO.UnitID);
                //dbServer.AddInParameter(command8, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);
                //int intStatus8 = dbServer.ExecuteNonQuery(command8, trans);


                DbCommand command7 = dbServer.GetStoredProcCommand("CIMS_DeleteTempCharge");
                dbServer.AddInParameter(command7, "BillID", DbType.Int64, objDetailsVO.ID);
                dbServer.AddInParameter(command7, "UnitID", DbType.Int64, objDetailsVO.UnitID);
                dbServer.AddInParameter(command7, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);
                int intStatus7 = dbServer.ExecuteNonQuery(command7, trans);

                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.Details = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }

            return BizActionObj;
        }


        //***//-------------------------------------
        public override IValueObject GetBillClearanceList(IValueObject valueObject, clsUserVO UserVo)
        {

            clsGetBillClearanceBizActionVO BizActionObj = valueObject as clsGetBillClearanceBizActionVO;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetBillClearanceListForSearch");
                DbDataReader reader;

                //dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");                             
                //dbServer.AddInParameter(command, "BillNO", DbType.String, BizActionObj.BillNO);
                //dbServer.AddInParameter(command, "MRNO", DbType.String, BizActionObj.MRNO);              
                //if (BizActionObj.BillType.HasValue)
                //    dbServer.AddInParameter(command, "BillType", DbType.Int16, (Int16)BizActionObj.BillType);
                //dbServer.AddInParameter(command, "UnitID", DbType.String, BizActionObj.UnitID);               

                //if (BizActionObj.FromDate != null)
                //    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate.Value.Date.Date);
                //if (BizActionObj.ToDate != null)
                //{
                //    if (BizActionObj.FromDate != null)
                //    {
                //        BizActionObj.ToDate = BizActionObj.ToDate.Value.Date.AddDays(1);
                //    }
                //    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate.Value.Date.Date);

                //}

                //dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, BizActionObj.CostingDivisionID);  
                //dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                //dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                //dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);            

                //dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                //reader = (DbDataReader)dbServer.ExecuteReader(command);



                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                if (BizActionObj.IsFreeze.HasValue)
                    dbServer.AddInParameter(command, "FreezeBill", DbType.Boolean, BizActionObj.IsFreeze);
                dbServer.AddInParameter(command, "Refunded", DbType.Boolean, BizActionObj.IsRefunded);
                dbServer.AddInParameter(command, "Opd_Ipd_External_Id", DbType.Int64, BizActionObj.Opd_Ipd_External_Id);
                dbServer.AddInParameter(command, "Opd_Ipd_External", DbType.Int64, BizActionObj.Opd_Ipd_External);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "OPDNO", DbType.String, BizActionObj.OPDNO);
                dbServer.AddInParameter(command, "ContactNo", DbType.String, BizActionObj.ContactNo);
                dbServer.AddInParameter(command, "BillNO", DbType.String, BizActionObj.BillNO);
                dbServer.AddInParameter(command, "MRNO", DbType.String, BizActionObj.MRNO);
                dbServer.AddInParameter(command, "BillSettleType", DbType.Int16, BizActionObj.BillStatus);
                dbServer.AddInParameter(command, "FromRefund", DbType.Boolean, BizActionObj.FromRefund);
                dbServer.AddInParameter(command, "IsPaymentModeChange", DbType.Boolean, 1);

                dbServer.AddInParameter(command, "IsShowIPD", DbType.Boolean, 1);

                if (BizActionObj.BillType.HasValue)
                    dbServer.AddInParameter(command, "BillType", DbType.Int16, (Int16)BizActionObj.BillType);
                dbServer.AddInParameter(command, "UnitID", DbType.String, BizActionObj.UnitID);
                if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                    dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.FirstName);
                //dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(BizActionObj.FirstName));
                if (BizActionObj.MiddleName != null && BizActionObj.MiddleName.Length != 0)
                    dbServer.AddInParameter(command, "MiddleName", DbType.String, BizActionObj.MiddleName);
                //dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(BizActionObj.MiddleName));
                if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.LastName);
                //dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(BizActionObj.LastName));
                if (BizActionObj.FromDate != null)
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate.Value.Date.Date);
                if (BizActionObj.ToDate != null)
                {
                    if (BizActionObj.FromDate != null)
                    {
                        // if (BizActionObj.FromDate.Equals(BizActionObj.ToDate))
                        BizActionObj.ToDate = BizActionObj.ToDate.Value.Date.AddDays(1);
                    }
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate.Value.Date.Date);

                }

                // Added by Changdeo
                dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, BizActionObj.CostingDivisionID);  //Costing Divisions for Clinical & Pharmacy Billing

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);





                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsBillVO>();
                    while (reader.Read())
                    {
                        clsBillVO objVO = new clsBillVO();

                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objVO.Status = false;
                        objVO.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        objVO.IsCancelled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCancelled"]));
                        objVO.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                        objVO.MRNO = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"]));
                        objVO.ContactNo = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo1"]));
                        objVO.BillType = (BillTypes)(Convert.ToInt16(DALHelper.HandleDBNull(reader["BillType"])));
                        objVO.Opd_Ipd_External = Convert.ToInt16(DALHelper.HandleDBNull(reader["Opd_Ipd_External"]));
                        objVO.Opd_Ipd_External_Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"]));
                        objVO.Opd_Ipd_External_UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_UnitId"]));
                        objVO.TotalBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalBillAmount"]));
                        objVO.TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalConcessionAmount"]));
                        objVO.NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"]));
                        objVO.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        if (BizActionObj.IsIPDBillList == true)
                        {
                            objVO.AdmissionType = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionType"]));
                            objVO.IPDNO = Convert.ToString(DALHelper.HandleDBNull(reader["IPDNO"]));
                            objVO.ConcessionAuthorizedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConcessionAuthorizedBy"]));
                            objVO.TotalRefund = Convert.ToDouble(DALHelper.HandleDBNull(reader["RefundAmount"]));
                            Func<double, bool> myFunc = x => x != 0;
                            objVO.IsRefunded = myFunc(objVO.TotalRefund);
                        }
                        else
                        {
                            objVO.VisitTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitTypeID"]));
                            objVO.TotalRefund = Convert.ToDouble(DALHelper.HandleDBNull(reader["RefundAmount"]));
                            Func<double, bool> myFunc = x => x != 0;
                            objVO.IsRefunded = myFunc(objVO.TotalRefund);
                            objVO.Opd_Ipd_External_No = Convert.ToString(DALHelper.HandleDBNull(reader["OPDNO"]));
                            objVO.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                            // objVO.ConcessionAuthorizedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConcessionAuthorizedBy"]));                                     
                            //objVO.PatientCategoryId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientCategoryId"]));  
                            //objVO.IsPackageConsumption = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackageConsumption"])); 
                        }

                        //objVO.IsPackageServiceInclude = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackageServiceInclude"]));
                        objVO.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        objVO.FirstName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])));
                        objVO.MiddleName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"])));
                        objVO.LastName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])));
                        objVO.BalanceAmountSelf = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmountSelf"]));
                        objVO.SelfAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["SelfAmount"]));
                        objVO.BillPaymentType = (BillPaymentTypes)(Convert.ToInt16(DALHelper.HandleDBNull(reader["BillPaymentType"])));
                        objVO.GrossDiscountPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["GrossDiscountPercentage"]));
                        objVO.GrossDiscountReasonID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GrossDiscountReasonID"]));
                        objVO.IsComanyBillCancelled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCompanyBillCancelled"]));
                        objVO.PatientSourceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceId"]));
                        //objVO.CompanyId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompanyId"]));
                        //objVO.CostingDivisionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CostingDivisionID"]));

                        //***//----
                        objVO.PatientAdvance = Convert.ToDouble(DALHelper.HandleDBNull(reader["PatientAdvance"]));
                        objVO.PackageAdvance = Convert.ToDouble(DALHelper.HandleDBNull(reader["PackageAdvance"]));
                        objVO.BalanceAdvance = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAdvance"]));

                        if (Convert.ToInt64(DALHelper.HandleDBNull(reader["PacBilledCount"])) == 1)
                        {
                            objVO.PacBilledCount = "Done";
                        }
                        else
                        {
                            objVO.PacBilledCount = "Not Done";
                        }

                        if (Convert.ToInt64(DALHelper.HandleDBNull(reader["SemenfreezingCount"])) == 1)
                        {
                            objVO.SemenfreezingCount = "Done";
                        }
                        else
                        {
                            objVO.SemenfreezingCount = "Not Done";
                        }

                        //-------------


                        if (BizActionObj.IsIPDBillList != true)
                        {
                            //objVO.PaymentModeDetails = Convert.ToString(DALHelper.HandleDBNull(reader["Payemtmode"]));

                            if ((Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"])) == false && objVO.BillType != (BillTypes)(2)))
                            {
                                objVO.BalanceAmountSelf = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"]));
                            }
                            else
                            {
                                objVO.BalanceAmountSelf = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmountSelf"]));
                            }

                        }
                        //objVO.IsRequestSend = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRequestSend"]));
                        //if (BizActionObj.IsRequest == true)
                        //{
                        //    objVO.IsApproved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsApproved"]));
                        //    objVO.LevelID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LevelID"]));
                        //    objVO.LevelDescription = Convert.ToString(DALHelper.HandleDBNull(reader["LevelDescription"]));
                        //    objVO.ApprovalRemark = Convert.ToString(DALHelper.HandleDBNull(reader["ApprovalRemark"]));
                        //    objVO.RequestTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RequestTypeID"]));
                        //    objVO.AuthorityPerson = Convert.ToString(DALHelper.HandleDBNull(reader["AuthorityPerson"]));
                        //}                     
                        //objVO.IsInvoiceGenerated = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsInvoiceGenerated"]));                     
                        //objVO.ConcessionReasonId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConcessionReasonId"]));
                        //objVO.ConcessionRemark = Convert.ToString(DALHelper.HandleDBNull(reader["ConcessionRemark"]));

                        if (BizActionObj.Opd_Ipd_External == 1)
                        {
                            objVO.ISDischarged = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ISDischarged"]));
                        }

                        BizActionObj.List.Add(objVO);

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

        public override IValueObject AddBillClearanceList(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;
            clsGetBillClearanceBizActionVO BizActionObj = valueObject as clsGetBillClearanceBizActionVO;
            try
            {
                con.Open();
                trans = con.BeginTransaction();
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_AddOTBillClearance");
                DbDataReader reader;
                command.Connection = con;

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, BizActionObj.IsFreeze);
                dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, BizActionObj.ScheduleID);
                dbServer.AddInParameter(command, "ScheduleUnitID", DbType.Int64, BizActionObj.ScheduleUnitID);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddOutParameter(command, "ID", DbType.Int64, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.OTBillClearanceID = (long)dbServer.GetParameterValue(command, "ID");

                if (BizActionObj.SuccessStatus == -1)
                {
                    throw new Exception();
                }

                foreach (var item in BizActionObj.List)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddOTBillClearanceDetails");
                    command1.Connection = con;
                    dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "OTBillClearanceID", DbType.Int64, BizActionObj.OTBillClearanceID);
                    dbServer.AddInParameter(command1, "OTBillClearanceUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "BillID", DbType.Int64, item.ID);
                    dbServer.AddInParameter(command1, "BillUnitID", DbType.Int64, item.UnitID);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, item.Status);
                    dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                }

                if (BizActionObj.SuccessStatus == -1)
                {
                    throw new Exception();
                }

                trans.Commit();

            }

            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.List = null;
                BizActionObj.SuccessStatus = -1;
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                con = null;
                trans = null;

            }
            return valueObject;

        }


        public override IValueObject GetSaveBillClearanceList(IValueObject valueObject, clsUserVO UserVo)
        {

            clsGetBillClearanceBizActionVO BizActionObj = valueObject as clsGetBillClearanceBizActionVO;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetBillClearanceListForSave");
                DbDataReader reader;


                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                if (BizActionObj.IsFreeze.HasValue)
                    dbServer.AddInParameter(command, "FreezeBill", DbType.Boolean, BizActionObj.IsFreeze);
                dbServer.AddInParameter(command, "Refunded", DbType.Boolean, BizActionObj.IsRefunded);
                dbServer.AddInParameter(command, "Opd_Ipd_External_Id", DbType.Int64, BizActionObj.Opd_Ipd_External_Id);
                dbServer.AddInParameter(command, "Opd_Ipd_External", DbType.Int64, BizActionObj.Opd_Ipd_External);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "OPDNO", DbType.String, BizActionObj.OPDNO);
                dbServer.AddInParameter(command, "ContactNo", DbType.String, BizActionObj.ContactNo);
                dbServer.AddInParameter(command, "BillNO", DbType.String, BizActionObj.BillNO);
                dbServer.AddInParameter(command, "MRNO", DbType.String, BizActionObj.MRNO);
                dbServer.AddInParameter(command, "BillSettleType", DbType.Int16, BizActionObj.BillStatus);
                dbServer.AddInParameter(command, "FromRefund", DbType.Boolean, BizActionObj.FromRefund);
                dbServer.AddInParameter(command, "IsPaymentModeChange", DbType.Boolean, 1);

                dbServer.AddInParameter(command, "IsShowIPD", DbType.Boolean, 1);

                if (BizActionObj.BillType.HasValue)
                    dbServer.AddInParameter(command, "BillType", DbType.Int16, (Int16)BizActionObj.BillType);
                dbServer.AddInParameter(command, "UnitID", DbType.String, BizActionObj.UnitID);
                if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                    dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.FirstName);
                //dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(BizActionObj.FirstName));
                if (BizActionObj.MiddleName != null && BizActionObj.MiddleName.Length != 0)
                    dbServer.AddInParameter(command, "MiddleName", DbType.String, BizActionObj.MiddleName);
                //dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(BizActionObj.MiddleName));
                if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.LastName);
                //dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(BizActionObj.LastName));
                if (BizActionObj.FromDate != null)
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate.Value.Date.Date);
                if (BizActionObj.ToDate != null)
                {
                    if (BizActionObj.FromDate != null)
                    {
                        // if (BizActionObj.FromDate.Equals(BizActionObj.ToDate))
                        BizActionObj.ToDate = BizActionObj.ToDate.Value.Date.AddDays(1);
                    }
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate.Value.Date.Date);

                }

                // Added by Changdeo
                dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, BizActionObj.CostingDivisionID);  //Costing Divisions for Clinical & Pharmacy Billing

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);





                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsBillVO>();
                    while (reader.Read())
                    {
                        clsBillVO objVO = new clsBillVO();

                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objVO.Status = false;
                        objVO.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        objVO.IsCancelled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCancelled"]));
                        objVO.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                        objVO.MRNO = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"]));
                        objVO.ContactNo = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo1"]));
                        objVO.BillType = (BillTypes)(Convert.ToInt16(DALHelper.HandleDBNull(reader["BillType"])));
                        objVO.Opd_Ipd_External = Convert.ToInt16(DALHelper.HandleDBNull(reader["Opd_Ipd_External"]));
                        objVO.Opd_Ipd_External_Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"]));
                        objVO.Opd_Ipd_External_UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_UnitId"]));
                        objVO.TotalBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalBillAmount"]));
                        objVO.TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalConcessionAmount"]));
                        objVO.NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"]));
                        objVO.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        if (BizActionObj.IsIPDBillList == true)
                        {
                            objVO.AdmissionType = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionType"]));
                            objVO.IPDNO = Convert.ToString(DALHelper.HandleDBNull(reader["IPDNO"]));
                            objVO.ConcessionAuthorizedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConcessionAuthorizedBy"]));
                            objVO.TotalRefund = Convert.ToDouble(DALHelper.HandleDBNull(reader["RefundAmount"]));
                            Func<double, bool> myFunc = x => x != 0;
                            objVO.IsRefunded = myFunc(objVO.TotalRefund);
                        }
                        else
                        {
                            objVO.VisitTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitTypeID"]));
                            objVO.TotalRefund = Convert.ToDouble(DALHelper.HandleDBNull(reader["RefundAmount"]));
                            Func<double, bool> myFunc = x => x != 0;
                            objVO.IsRefunded = myFunc(objVO.TotalRefund);
                            objVO.Opd_Ipd_External_No = Convert.ToString(DALHelper.HandleDBNull(reader["OPDNO"]));
                            objVO.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        }

                        //objVO.IsPackageServiceInclude = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackageServiceInclude"]));
                        objVO.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        objVO.FirstName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])));
                        objVO.MiddleName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"])));
                        objVO.LastName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])));
                        objVO.BalanceAmountSelf = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmountSelf"]));
                        objVO.SelfAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["SelfAmount"]));
                        objVO.BillPaymentType = (BillPaymentTypes)(Convert.ToInt16(DALHelper.HandleDBNull(reader["BillPaymentType"])));
                        objVO.GrossDiscountPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["GrossDiscountPercentage"]));
                        objVO.GrossDiscountReasonID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GrossDiscountReasonID"]));
                        objVO.IsComanyBillCancelled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCompanyBillCancelled"]));
                        objVO.PatientSourceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceId"]));

                        //***//----
                        objVO.PatientAdvance = Convert.ToDouble(DALHelper.HandleDBNull(reader["PatientAdvance"]));
                        objVO.PackageAdvance = Convert.ToDouble(DALHelper.HandleDBNull(reader["PackageAdvance"]));
                        objVO.BalanceAdvance = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAdvance"]));

                        if (Convert.ToInt64(DALHelper.HandleDBNull(reader["PacBilledCount"])) == 1)
                        {
                            objVO.PacBilledCount = "Done";
                        }
                        else
                        {
                            objVO.PacBilledCount = "Not Done";
                        }

                        if (Convert.ToInt64(DALHelper.HandleDBNull(reader["SemenfreezingCount"])) == 1)
                        {
                            objVO.SemenfreezingCount = "Done";
                        }
                        else
                        {
                            objVO.SemenfreezingCount = "Not Done";
                        }
                        //-------------

                        if (BizActionObj.IsIPDBillList != true)
                        {

                            if ((Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"])) == false && objVO.BillType != (BillTypes)(2)))
                            {
                                objVO.BalanceAmountSelf = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"]));
                            }
                            else
                            {
                                objVO.BalanceAmountSelf = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmountSelf"]));
                            }

                        }

                        if (BizActionObj.Opd_Ipd_External == 1)
                        {
                            objVO.ISDischarged = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ISDischarged"]));
                        }

                        objVO.OTBillClearanceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTBillClearanceID"]));

                        BizActionObj.List.Add(objVO);

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

        //-----------------------------------------

        public override IValueObject CheckPackageAdvanceBilling(IValueObject valueObject, clsUserVO UserVO)
        {
            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;
            clsAddBillBizActionVO BizActionObj = valueObject as clsAddBillBizActionVO;
            try
            {
                con.Open();
                trans = con.BeginTransaction();
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_CheckPackageAdvanceBilling");
                DbDataReader reader;
                command.Connection = con;

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.Details.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.Details.PatientUnitID);
                dbServer.AddInParameter(command, "PackageID", DbType.Int64, BizActionObj.Details.PackageID);
                dbServer.AddInParameter(command, "CurrentBillAmount", DbType.Double, BizActionObj.Details.TotalBillAmount);
                dbServer.AddInParameter(command, "BillID", DbType.Int64, BizActionObj.Details.BillID);
                dbServer.AddInParameter(command, "BillUnitID", DbType.Int64, BizActionObj.Details.UnitID);
                dbServer.AddInParameter(command, "ParentID", DbType.Int64, BizActionObj.Details.ParentID);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddOutParameter(command, "BalancePackageAdvance", DbType.Decimal, 0);
                dbServer.AddOutParameter(command, "ServiceConsumptionAmount", DbType.Decimal, 0);
                dbServer.AddOutParameter(command, "PharmacyConsumptionAmount", DbType.Decimal, 0);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.BalancePackageAdvance = Convert.ToDecimal(dbServer.GetParameterValue(command, "BalancePackageAdvance"));
                BizActionObj.ServiceConsumptionAmount = Convert.ToDecimal(dbServer.GetParameterValue(command, "ServiceConsumptionAmount"));
                BizActionObj.PharmacyConsumptionAmount = Convert.ToDecimal(dbServer.GetParameterValue(command, "PharmacyConsumptionAmount"));
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                con = null;
                trans = null;

            }
            return valueObject;
        }
    }
}
