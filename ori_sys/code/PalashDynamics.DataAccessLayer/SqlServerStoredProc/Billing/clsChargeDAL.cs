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
using PalashDynamics.ValueObjects.Administration;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsChargeDAL : clsBaseChargeDAL
    {

        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsChargeDAL()
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
                string err = ex.Message;
                throw;
            }
        }


        public override IValueObject Add(IValueObject valueObject, clsUserVO UserVo)
        {

            clsAddChargeBizActionVO BizActionObj = valueObject as clsAddChargeBizActionVO;

            //if (BizActionObj.Details.ID == 0)
            BizActionObj = AddDetails(BizActionObj, UserVo, null, null, 0, 0);  //BizActionObj = AddDetails(BizActionObj, UserVo,null,null);
            //else
            // BizActionObj = UpdateDetails(BizActionObj);

            return valueObject;
        }

        private clsAddChargeBizActionVO AddDetails(clsAddChargeBizActionVO BizActionObj, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction, long iParentID, long iCDParentID)
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

                clsChargeVO objDetailsVO = BizActionObj.Details;
                objDetailsVO.ChargeDetails = new clsChargeDetailsVO();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddCharge");
                //dbServer.AddInParameter(command, "ParentID", DbType.Int64, iParentID);

                dbServer.AddInParameter(command, "ParentID", DbType.Int64, objDetailsVO.ParentID);

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                if (objDetailsVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));
                dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);

                // if (objDetailsVO.IsIPDBill == true)   // Service  date Only In Case IPD BILL
                // dbServer.AddInParameter(command, "ServiceDate", DbType.DateTime, objDetailsVO.ServiceDate);

                dbServer.AddInParameter(command, "Opd_Ipd_External_Id", DbType.Int64, objDetailsVO.Opd_Ipd_External_Id);
                dbServer.AddInParameter(command, "Opd_Ipd_External_UnitId", DbType.Int64, objDetailsVO.Opd_Ipd_External_UnitId);
                dbServer.AddInParameter(command, "ClassId", DbType.Int64, objDetailsVO.ClassId);
                dbServer.AddInParameter(command, "Opd_Ipd_External", DbType.Int16, objDetailsVO.Opd_Ipd_External);
                dbServer.AddInParameter(command, "TariffServiceId", DbType.Int64, objDetailsVO.TariffServiceId);
                dbServer.AddInParameter(command, "TariffId", DbType.Int64, objDetailsVO.TariffId);
                dbServer.AddInParameter(command, "ServiceId", DbType.Int64, objDetailsVO.ServiceId);
                //Added By CDS
                dbServer.AddInParameter(command, "ServiceNameNew", DbType.String, objDetailsVO.ServiceName);
                dbServer.AddInParameter(command, "ServiceCode", DbType.String, objDetailsVO.ServiceCode);

                dbServer.AddInParameter(command, "DoctorId", DbType.Int64, objDetailsVO.SelectedDoctor.ID);    //dbServer.AddInParameter(command, "DoctorId", DbType.Int64, objDetailsVO.DoctorId);
                dbServer.AddInParameter(command, "Rate", DbType.Double, objDetailsVO.Rate);
                dbServer.AddInParameter(command, "Quantity", DbType.Double, objDetailsVO.Quantity);
                dbServer.AddInParameter(command, "TotalAmount", DbType.Double, objDetailsVO.TotalAmount);
                dbServer.AddInParameter(command, "Discount", DbType.Double, objDetailsVO.Discount);
                dbServer.AddInParameter(command, "StaffFree", DbType.Double, objDetailsVO.StaffFree);
                dbServer.AddInParameter(command, "NetAmount", DbType.Double, objDetailsVO.NetAmount);
                dbServer.AddInParameter(command, "PaidAmount", DbType.Double, objDetailsVO.ServicePaidAmount);
                dbServer.AddInParameter(command, "HospShareAmount", DbType.Double, objDetailsVO.HospShareAmount);
                dbServer.AddInParameter(command, "DoctorShareAmount", DbType.Double, objDetailsVO.DoctorShareAmount);
                dbServer.AddInParameter(command, "isPackageService", DbType.Boolean, objDetailsVO.isPackageService);
                dbServer.AddInParameter(command, "PackageID", DbType.Int64, objDetailsVO.PackageID);
                dbServer.AddInParameter(command, "ChargeIDPackage", DbType.Int64, objDetailsVO.ChargeIDPackage);
                dbServer.AddInParameter(command, "Emergency", DbType.Boolean, objDetailsVO.Emergency);
                dbServer.AddInParameter(command, "SponsorType", DbType.Boolean, objDetailsVO.SponsorType);
                dbServer.AddInParameter(command, "IsBilled", DbType.Boolean, objDetailsVO.IsBilled);
                dbServer.AddInParameter(command, "ServiceSpecilizationID", DbType.Int64, objDetailsVO.ServiceSpecilizationID);
                dbServer.AddInParameter(command, "ConcessionPercent", DbType.Double, objDetailsVO.ConcessionPercent);
                dbServer.AddInParameter(command, "ConcessionAmount", DbType.Double, objDetailsVO.Concession);
                dbServer.AddInParameter(command, "StaffDiscountPercent", DbType.Double, objDetailsVO.StaffDiscountPercent);
                dbServer.AddInParameter(command, "StaffDiscountAmount", DbType.Double, objDetailsVO.StaffDiscountAmount);
                dbServer.AddInParameter(command, "StaffParentDiscountPercent", DbType.Double, objDetailsVO.StaffParentDiscountPercent);
                dbServer.AddInParameter(command, "StaffParentDiscountAmount", DbType.Double, objDetailsVO.StaffParentDiscountAmount);
                dbServer.AddInParameter(command, "ServiceTaxPercent", DbType.Double, 0);// objDetailsVO.ServiceTaxPercent);  Added by Ashish Z. for Taxation Details on Dated 10052017
                dbServer.AddInParameter(command, "ServiceTaxAmount", DbType.Double, 0); //objDetailsVO.ServiceTaxAmount);    Added by Ashish Z. for Taxation Details on Dated 10052017 
                dbServer.AddInParameter(command, "GrossDiscountReason", DbType.Int64, objDetailsVO.GrossDiscountReasonID);
                dbServer.AddInParameter(command, "GrossDiscountPercentage", DbType.Double, objDetailsVO.GrossDiscountPercentage);


                // Added by Changdeo
                dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);  //Costing Divisions for Clinical & Pharmacy Billing
                dbServer.AddInParameter(command, "IsTemp", DbType.Boolean, objDetailsVO.IsTemp);
                ////dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, UserVo.UserLoginInfo.CashCounterID);  //Costing Divisions for Clinical & Pharmacy Billing    

                dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "IsAutoCharge", DbType.Boolean, false);

                dbServer.AddInParameter(command, "ConditionTypeID", DbType.Int64, objDetailsVO.ConditionTypeID);  //used to identify that service is add from Conditional Service Search with AND/OR

                dbServer.AddInParameter(command, "InitialRate", DbType.Int64, objDetailsVO.InitialRate); // to maintain the Initial Rate of Service after changing Doctor Service Rate. (added By Ashish Z. on 170616)

                dbServer.AddInParameter(command, "TotalServiceTaxAmount", DbType.Double, objDetailsVO.TotalServiceTaxAmount); //Added by Ashish Z. for Taxation Details on Dated 11052017

                dbServer.AddInParameter(command, "IsPackageConsumption", DbType.Boolean, objDetailsVO.IsPackageConsumption); //Added By Bhushanp For Package Consumption 09102017

                dbServer.AddInParameter(command, "ProcessID", DbType.Int64, objDetailsVO.ProcessID);    //Package New Changes for Process Added on 21042018

                dbServer.AddInParameter(command, "PackageConcessionPercent", DbType.Double, objDetailsVO.PackageConcessionPercent);   //Package New Changes for Process Added on 14062018
                dbServer.AddInParameter(command, "PackageConcessionAmount", DbType.Double, objDetailsVO.PackageConcession);           //Package New Changes for Process Added on 14062018

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Details.ID = (long)dbServer.GetParameterValue(command, "ID");

                //Addedby Priyanka On 25June2012 
                if (BizActionObj.FromVisit == false)
                {

                    //For add amount details in T_chargeDetails table
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddChargeDetails");
                    dbServer.AddInParameter(command1, "ChargeID", DbType.Int64, BizActionObj.Details.ID);
                    dbServer.AddInParameter(command1, "ChargeUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "ParentID", DbType.Int64, iCDParentID);

                    dbServer.AddInParameter(command1, "Date", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command1, "Quantity", DbType.Double, objDetailsVO.Quantity);
                    dbServer.AddInParameter(command1, "Rate", DbType.Double, objDetailsVO.Rate);
                    dbServer.AddInParameter(command1, "TotalAmount", DbType.Double, objDetailsVO.TotalAmount);
                    dbServer.AddInParameter(command1, "ConcessionAmount", DbType.Double, objDetailsVO.Concession);
                    dbServer.AddInParameter(command1, "ServiceTaxAmount", DbType.Double, objDetailsVO.ServiceTaxAmount);

                    dbServer.AddInParameter(command1, "NetAmount", DbType.Double, objDetailsVO.NetAmount);
                    dbServer.AddInParameter(command1, "PaidAmount", DbType.Double, objDetailsVO.ServicePaidAmount);
                    if (objDetailsVO.IsBilled)
                        dbServer.AddInParameter(command1, "BalanceAmount", DbType.Double, objDetailsVO.BalanceAmount);
                    else
                        dbServer.AddInParameter(command1, "BalanceAmount", DbType.Double, objDetailsVO.NetAmount);


                    dbServer.AddInParameter(command1, "RefundID", DbType.Int64, objDetailsVO.RefundID);
                    dbServer.AddInParameter(command1, "RefundAmount", DbType.Double, objDetailsVO.RefundAmount);

                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, objDetailsVO.Status);
                    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId); dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ChargeDetails.ID);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                    //By Anjali............................
                    dbServer.AddInParameter(command1, "IsFromApprovedRequest", DbType.Boolean, objDetailsVO.IsFromApprovedRequest);
                    //......................................

                    int intStatus2 = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                    BizActionObj.Details.ChargeDetails.ID = (long)dbServer.GetParameterValue(command1, "ID");
                    //END






                }

                #region Added by Ashish Z. for Taxation Details on Dated 10052017
                if (objDetailsVO.ChargeTaxDetailsList != null && objDetailsVO.ChargeTaxDetailsList.Count > 0)
                {
                    foreach (var item in objDetailsVO.ChargeTaxDetailsList)
                    {
                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdateChargeTaxDetails");
                        dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "ChargeID", DbType.Int64, BizActionObj.Details.ID);
                        dbServer.AddInParameter(command2, "ChargeUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "TaxID", DbType.Int64, item.TaxID);
                        dbServer.AddInParameter(command2, "TaxPercentage", DbType.Double, item.ServiceTaxPercent); //item.Percentage);
                        dbServer.AddInParameter(command2, "TaxType", DbType.Int32, item.TaxType);
                        dbServer.AddInParameter(command2, "TaxAmount", DbType.Double, item.ServiceTaxAmount);
                        dbServer.AddInParameter(command2, "IsTaxLimitApplicable", DbType.Boolean, item.IsTaxLimitApplicable);
                        dbServer.AddInParameter(command2, "TaxLimit", DbType.Decimal, item.TaxLimit);
                        dbServer.AddInParameter(command2, "Date", DbType.DateTime, item.Date);
                        dbServer.AddInParameter(command2, "Rate", DbType.Double, item.Rate);

                        dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                        dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                        dbServer.AddOutParameter(command2, "ReasultStatus", DbType.Int32, int.MaxValue);

                        int intStatus3 = dbServer.ExecuteNonQuery(command2, trans);
                        int ReasultStatus = Convert.ToInt32(dbServer.GetParameterValue(command2, "ReasultStatus"));
                    }
                }
                #endregion

                BizActionObj.SuccessStatus = 0;
                if (pConnection == null) trans.Commit();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                BizActionObj.SuccessStatus = -1;
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

            return BizActionObj;
        }

        //public override IValueObject Add(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        public override IValueObject Add(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction, long iParentID, long iCDParentID)
        {

            clsAddChargeBizActionVO BizActionObj = valueObject as clsAddChargeBizActionVO;
            BizActionObj = AddDetails(BizActionObj, UserVo, pConnection, pTransaction, iParentID, iCDParentID);   //BizActionObj = AddDetails(BizActionObj, UserVo, pConnection, pTransaction);

            return valueObject;

        }

        public override IValueObject GetList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetChargeListBizActionVO BizActionObj = (clsGetChargeListBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCharges");

                //Adding MasterTableName as Input Parameter to filter record
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "IsBilled", DbType.Boolean, BizActionObj.IsBilled);
                dbServer.AddInParameter(command, "Opd_Ipd_External", DbType.Int16, BizActionObj.Opd_Ipd_External);
                dbServer.AddInParameter(command, "Opd_Ipd_External_Id", DbType.Int64, BizActionObj.Opd_Ipd_External_Id);
                dbServer.AddInParameter(command, "Opd_Ipd_External_UnitId", DbType.Int64, BizActionObj.Opd_Ipd_External_UnitId);
                dbServer.AddInParameter(command, "BillID", DbType.Int64, BizActionObj.BillID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "RequestTypeID", DbType.Int64, BizActionObj.RequestTypeID);
                //dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, BizActionObj.CostingDivisionID);                
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                //Check whether the reader contains the records
                if (reader.HasRows)
                {
                    //if masterlist instance is null then creates new instance
                    if (BizActionObj.List == null)
                    {
                        BizActionObj.List = new List<clsChargeVO>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        //Add the object value in list
                        clsChargeVO objVO = new clsChargeVO();
                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                        objVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objVO.CostingDivisionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CostingDivisionID"]));
                        objVO.ApprovalID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalID"]));
                        objVO.ApprovalUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalUnitID"]));
                        objVO.IsSetForApproval = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSetApprovalReq"]));
                        objVO.FirstApprovalChecked = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFirstApproval"]));
                        objVO.SecondApprovalChecked = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSecondApproval"]));
                        objVO.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        objVO.ServiceDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ServiceDate"])); //Added by CDS For IPD Only                            
                        objVO.Opd_Ipd_External_Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"]));
                        objVO.Opd_Ipd_External = (Int16)DALHelper.HandleDBNull(reader["Opd_Ipd_External"]);
                        objVO.TariffServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffServiceId"]));
                        objVO.ServiceId = (Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceId"])));
                        objVO.ServiceSpecilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"]));
                        objVO.ServiceSubSpecilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecilizationID"]));
                        objVO.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                        //Added By CDS 
                        objVO.ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"]));
                        //Added By CDS 
                        objVO.DoctorId = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorId"]));
                        objVO.SelectedDoctor.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorId"]));
                        objVO.SelectedDoctor.Description = (string)DALHelper.HandleDBNull(reader["DoctorName"]);
                        objVO.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                        objVO.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
                        objVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"]));
                        objVO.ConcessionPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                        if (objVO.ConcessionPercent == 0) objVO.Concession = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                        objVO.StaffDiscountPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffDiscountPercent"]));
                        if (objVO.StaffDiscountPercent == 0) objVO.StaffDiscountAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffDiscountAmount"]));
                        objVO.StaffParentDiscountPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffParentDiscountPercent"]));
                        if (objVO.StaffParentDiscountPercent == 0) objVO.StaffParentDiscountAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffParentDiscountAmount"]));
                        objVO.ServiceTaxPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["ServiceTaxPercent"]));
                        if (objVO.ServiceTaxPercent == 0) objVO.ServiceTaxAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ServiceTaxAmount"]));
                        objVO.Discount = Convert.ToDouble(DALHelper.HandleDBNull(reader["Discount"]));
                        objVO.StaffFree = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffFree"]));
                        objVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                        objVO.ServicePaidAmount = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["PaidAmount"]));
                        objVO.SettleNetAmount = objVO.NetAmount;
                        objVO.IsCancelled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCancelled"]));
                        objVO.CancellationRemark = Convert.ToString(DALHelper.HandleDBNull(reader["CancellationRemark"]));
                        objVO.CancelledBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["CancelledBy"]));
                        objVO.CancelledDate = (DateTime?)DALHelper.HandleDate(reader["CancelledDate"]);
                        objVO.IsBilled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBilled"]));
                        objVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objVO.TariffServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffServiceID"]));
                        objVO.ServiceSpecilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"]));
                        objVO.RateEditable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["RateEditable"]));
                        objVO.MaxRate = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["MaxRate"]));
                        objVO.MinRate = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["MinRate"]));
                        objVO.BalanceAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmount"]));
                        objVO.isPackageService = Convert.ToBoolean(DALHelper.HandleDBNull(reader["isPackageService"]));
                        objVO.PackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageID"]));
                        objVO.ParentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ParentID"]));
                        objVO.TariffId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffId"]));
                        objVO.ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]));
                        objVO.IsAutoCharge = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAutoCharge"]));
                        objVO.ClassId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassId"]));
                        objVO.RefundID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RefundID"]));
                        if (objVO.ParentID > 0)
                        {
                            //objVO.ChildPackageService = true;
                        }
                        objVO.GrossDiscountPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["GrossDiscountPercent"]));
                        objVO.GrossDiscountReasonID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["GrossDiscountReason"]));
                        //Properties use for unfreezed Patho,Radio services
                        objVO.POBDID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POBDID"]));
                        objVO.IsSampleCollected = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleCollected"]));
                        objVO.POBID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POBID"]));
                        objVO.ROBDID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ROBDID"]));
                        objVO.IsReportCollected = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsReportCollected"]));
                        objVO.ROBID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ROBID"]));
                        objVO.ConditionTypeID = (long)DALHelper.HandleIntegerNull(reader["ConditionTypeID"]);  //used to identify that service is add from Conditional Service Search with AND/OR
                        //By Anjali..............................................
                        objVO.ApprovalRequestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalRequestID"]));
                        objVO.ApprovalRequestUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalRequestUnitID"]));
                        objVO.ApprovalRequestDetailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalRequestDetailsID"]));
                        objVO.ApprovalRequestDetailsUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalRequestDetailsUnitID"]));
                        objVO.IsSendForApproval = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSendForApproval"]));
                        objVO.LevelID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LevelID"]));
                        objVO.SelectCharge = Convert.ToBoolean(DALHelper.HandleDBNull(reader["SelectCharge"]));
                        objVO.ApprovalRemark = Convert.ToString(DALHelper.HandleDBNull(reader["ApprovalRemark"]));
                        objVO.ApprovalStatus = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ApprovalStatus"]));
                        objVO.ApprovalRequestRemark = Convert.ToString(DALHelper.HandleDBNull(reader["ApprovalRequestRemark"]));
                        objVO.InitialRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["InitialRate"]));
                        objVO.IsResultEntry = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsResultEntry"]));//Added By Yogesh K
                        objVO.Isupload = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsUpload"]));//Added By Yogesh K
                        objVO.CompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompanyId"])); // added by ashish Z. on Dated 260716 for checking the Multiple Sponsor Billing on on frmBill
                        objVO.PatientSourceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceId"])); // added by ashish Z. on Dated 260716 for checking the Multiple Sponsor Billing on on frmBill
                        //........................................................
                        objVO.IsAdjustableHead = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAdjustableHead"])); // added by BHUSHANP on Dated 17022017 For Package Change
                        objVO.ServiceComponentRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceComponentRate"])); // added by BHUSHANP on Dated 17022017 For Package Change
                        objVO.IsConsiderAdjustable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsConsiderAdjustable"])); // Added By CDS For Package IsConsiderAdjustable
                        objVO.SumOfExludedServices = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SumOfExludedServices"])); // Added By CDS For Package Not consider in Adjustable Head
                        objVO.IsAgainstBill = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAgainstBill"])); //Added By Bhushanp 22032017                        
                        if (objVO.IsAgainstBill)
                        {
                            if (!objVO.ApprovalStatus)
                            {
                                objVO.RefundID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgainstBillRefundID"]));
                            }
                        }
                        objVO.RefundAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["Amount"]));//Added By Bhushanp 22032017    
                        objVO.TaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["TaxType"]));  //GST Details added by Ashish Z. on dated 24062017
                        objVO.TotalServiceTaxAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalServiceTaxAmount"])); //GST Details added by Ashish Z. on dated 24062017
                        objVO.RefundedAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["RefundedAmount"]));//Added By Bhushanp 22032017    
                        objVO.IsRefund = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRefund"]));
                        objVO.IsConsumption = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCosumption"]));//Added By Bhushanp 08082017 For Package Consumption    
                        objVO.IsPackageConsumption = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackageConsume"]));//Added By Bhushanp 08082017 For Package Consumption    
                        objVO.TotalRefundAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["RefundAmount"]));//Added By Bhushanp 22032017  

                        objVO.RequestRefundReasonID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalRequestRemarkID"]));
                        objVO.ApprovalRefundReasonID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalRemarkID"]));
                        objVO.RefundReasonID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CancellationRemarkID"]));

                        objVO.ProcessID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcessID"]));     // Package New Changes for Process Added on 23042018

                        objVO.PackageConcessionPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["PackageConcessionPercent"]));
                        if (objVO.PackageConcessionPercent == 0) objVO.PackageConcession = Convert.ToDouble(DALHelper.HandleDBNull(reader["PackageConcessionAmount"]));

                        BizActionObj.List.Add(objVO); //new MasterListItem((long)reader["Id"], reader["Description"].ToString(), (DateTime?)DALHelper.HandleDate(reader["Date"])));
                    }
                }
                #region GST Details added by Ashish Z. on dated 24062017
                reader.NextResult();
                List<clsServiceTaxVO> TaxLinkingDetailsList = new List<clsServiceTaxVO>();
                while (reader.Read())
                {
                    clsServiceTaxVO objVO = new clsServiceTaxVO();
                    objVO.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]));
                    objVO.ServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceId"]));
                    objVO.ClassId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassId"]));
                    objVO.TaxID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TaxID"]));
                    objVO.Percentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TaxPercentage"]));

                    objVO.TaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["TaxType"]));
                    objVO.IsTaxLimitApplicable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsTaxLimitApplicable"]));
                    objVO.TaxLimit = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TaxLimit"]));
                    objVO.TaxName = Convert.ToString(DALHelper.HandleDBNull(reader["TaxName"]));

                    TaxLinkingDetailsList.Add(objVO);
                }

                if (BizActionObj.List != null && TaxLinkingDetailsList != null && BizActionObj.List.Count() > 0 && TaxLinkingDetailsList.Count() > 0)
                {
                    foreach (var item in BizActionObj.List.ToList())
                    {
                        item.ChargeTaxDetailsVO = new clsChargeTaxDetailsVO();
                        item.ChargeTaxDetailsVO.TaxLinkingDetailsList = new List<clsServiceTaxVO>();
                        foreach (var TaxLink in TaxLinkingDetailsList.ToList())
                        {
                            if (item.ServiceId == TaxLink.ServiceId)
                            {
                                item.ChargeTaxDetailsVO.TaxLinkingDetailsList.Add(TaxLink);
                            }
                        }
                    }
                }
                #endregion


                //if (!reader.IsClosed)
                //{
                //    reader.Close();
                //}
                reader.Close();

            }
            catch (Exception ex)
            {
                string err = ex.Message;
                //"Error Occured";
                //logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }


            return BizActionObj;

        }

        #region GST Details added by Ashish Z. on dated 24062017
        public override IValueObject GetChargeTaxDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetChargeListBizActionVO BizActionObj = (clsGetChargeListBizActionVO)valueObject;
            BizActionObj.ChargeVO = new clsChargeVO();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetChargeTaxDetails");
                dbServer.AddInParameter(command, "ChargeID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "ChargeUnitID", DbType.Int64, BizActionObj.UnitID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ChargeVO.ChargeTaxDetailsList == null)
                    {
                        BizActionObj.ChargeVO.ChargeTaxDetailsList = new List<clsChargeTaxDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsChargeTaxDetailsVO objVO = new clsChargeTaxDetailsVO();
                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objVO.ChargeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ChargeID"]));
                        objVO.ChargeUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ChargeUnitID"]));
                        objVO.TaxID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TaxID"]));
                        objVO.Percentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TaxPercentage"]));
                        //objVO.ServiceTaxPercent = Convert.ToDouble(objVO.Percentage);
                        objVO.TaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["TaxType"]));
                        objVO.ServiceTaxAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TaxAmount"]));
                        objVO.IsTaxLimitApplicable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsTaxLimitApplicable"]));
                        objVO.TaxLimit = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TaxLimit"]));
                        objVO.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                        objVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

                        objVO.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                        objVO.TaxName = Convert.ToString(DALHelper.HandleDBNull(reader["TaxName"]));
                        objVO.UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"]));

                        BizActionObj.ChargeVO.ChargeTaxDetailsList.Add(objVO);
                    }
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
            finally
            {
            }
            return BizActionObj;

        }
        #endregion

        public override IValueObject AddRefundServices(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = null;
            DbTransaction trans = null;
            clsAddRefundServiceChargeBizActionVO BizActionObj = (clsAddRefundServiceChargeBizActionVO)valueObject;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                foreach (var ObjCharge in BizActionObj.ChargeList)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_CancelServices");

                    dbServer.AddInParameter(command, "ChargeID", DbType.Int64, ObjCharge.ID);
                    dbServer.AddInParameter(command, "RefundID", DbType.Int64, ObjCharge.RefundID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, ObjCharge.UnitID);
                    BizActionObj.UnitID = ObjCharge.UnitID;
                    dbServer.AddInParameter(command, "IsCancelled", DbType.Boolean, ObjCharge.IsCancelled);
                    dbServer.AddInParameter(command, "CancelledBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "CancelledDate", DbType.DateTime, ObjCharge.CancelledDate);
                    dbServer.AddInParameter(command, "CancellationRemarkID", DbType.Int64, ObjCharge.SelectedRequestRefundReason.ID); //ObjCharge.SelectedRefundReason.ID);  
                    dbServer.AddInParameter(command, "CancellationRemark", DbType.String, ObjCharge.SelectedRequestRefundReason.Description); //ObjCharge.SelectedRefundReason.Description);
                    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    dbServer.AddInParameter(command, "ApprovalRequestID", DbType.Int64, ObjCharge.ApprovalRequestID);
                    dbServer.AddInParameter(command, "ApprovalRequestUnitID", DbType.Int64, ObjCharge.ApprovalRequestUnitID);
                    dbServer.AddInParameter(command, "ApprovalRequestDetailsID", DbType.Int64, ObjCharge.ApprovalRequestDetailsID);
                    dbServer.AddInParameter(command, "ApprovalRequestDetailsUnitID", DbType.Int64, ObjCharge.ApprovalRequestDetailsUnitID);

                    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    //int intStatus = dbServer.ExecuteNonQuery(command);

                    if (BizActionObj.IsUpdate == true)
                    {

                        DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_UpdateChargeDetailsFromRefund");

                        dbServer.AddInParameter(command3, "ChargeID ", DbType.Int64, ObjCharge.ID);
                        dbServer.AddInParameter(command3, "Date", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command3, "RefundID", DbType.Int64, ObjCharge.RefundID);
                        dbServer.AddInParameter(command3, "RefundAmount", DbType.Double, ObjCharge.RefundAmount);
                        int iStatus4 = dbServer.ExecuteNonQuery(command3, trans);
                    }
                    else
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddChargeDetails");
                        dbServer.AddInParameter(command1, "ChargeID", DbType.Int64, ObjCharge.ID);
                        dbServer.AddInParameter(command1, "ChargeUnitID", DbType.Int64, ObjCharge.UnitID);
                        dbServer.AddInParameter(command1, "ParentID", DbType.Int64, ObjCharge.ParentID);
                        dbServer.AddInParameter(command1, "Date", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command1, "Quantity", DbType.Double, ObjCharge.Quantity);
                        dbServer.AddInParameter(command1, "Rate", DbType.Double, 0);
                        dbServer.AddInParameter(command1, "TotalAmount", DbType.Double, 0);
                        dbServer.AddInParameter(command1, "ConcessionAmount", DbType.Double, 0);
                        dbServer.AddInParameter(command1, "ServiceTaxAmount", DbType.Double, 0);
                        dbServer.AddInParameter(command1, "NetAmount", DbType.Double, 0);
                        dbServer.AddInParameter(command1, "PaidAmount", DbType.Double, 0);
                        dbServer.AddInParameter(command1, "BalanceAmount", DbType.Double, 0);
                        dbServer.AddInParameter(command1, "RefundID", DbType.Int64, ObjCharge.RefundID);
                        dbServer.AddInParameter(command1, "RefundAmount", DbType.Double, ObjCharge.RefundAmount);
                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, ObjCharge.Status);
                        dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                        ObjCharge.ChargeDetails = new clsChargeDetailsVO();
                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjCharge.ChargeDetails.ID);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus2 = dbServer.ExecuteNonQuery(command1, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                        ObjCharge.ChargeDetails.ID = (long)dbServer.GetParameterValue(command1, "ID");
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

        //By Anjali
        public override IValueObject GetChargeListForApprovalRequestWindow(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetChargeListBizActionVO BizActionObj = (clsGetChargeListBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("GetChargeListForApprovalRequestWindow");
                //Adding MasterTableName as Input Parameter to filter record
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "IsBilled", DbType.Boolean, BizActionObj.IsBilled);
                dbServer.AddInParameter(command, "Opd_Ipd_External", DbType.Int16, BizActionObj.Opd_Ipd_External);
                dbServer.AddInParameter(command, "Opd_Ipd_External_Id", DbType.Int64, BizActionObj.Opd_Ipd_External_Id);
                dbServer.AddInParameter(command, "Opd_Ipd_External_UnitId", DbType.Int64, BizActionObj.Opd_Ipd_External_UnitId);
                dbServer.AddInParameter(command, "BillID", DbType.Int64, BizActionObj.BillID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                //dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, BizActionObj.CostingDivisionID);                
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                //Check whether the reader contains the records
                if (reader.HasRows)
                {
                    //if masterlist instance is null then creates new instance
                    if (BizActionObj.List == null)
                    {
                        BizActionObj.List = new List<clsChargeVO>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        //Add the object value in list
                        clsChargeVO objVO = new clsChargeVO();
                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                        objVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objVO.CostingDivisionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CostingDivisionID"]));

                        objVO.ApprovalID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalID"]));
                        objVO.ApprovalUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovalUnitID"]));
                        objVO.IsSetForApproval = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSetApprovalReq"]));
                        objVO.FirstApprovalChecked = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFirstApproval"]));
                        objVO.SecondApprovalChecked = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSecondApproval"]));

                        objVO.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        objVO.ServiceDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ServiceDate"])); //Added by CDS For IPD Only                            
                        objVO.Opd_Ipd_External_Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"]));
                        objVO.Opd_Ipd_External = (Int16)DALHelper.HandleDBNull(reader["Opd_Ipd_External"]);
                        objVO.TariffServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffServiceId"]));
                        objVO.ServiceId = (Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceId"])));
                        objVO.ServiceSpecilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"]));
                        objVO.ServiceSubSpecilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecilizationID"]));
                        objVO.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                        objVO.DoctorId = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorId"]));
                        objVO.SelectedDoctor.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorId"]));
                        objVO.SelectedDoctor.Description = (string)DALHelper.HandleDBNull(reader["DoctorName"]);
                        objVO.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                        objVO.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
                        objVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"]));
                        objVO.ConcessionPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                        if (objVO.ConcessionPercent == 0) objVO.Concession = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                        objVO.StaffDiscountPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffDiscountPercent"]));
                        if (objVO.StaffDiscountPercent == 0) objVO.StaffDiscountAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffDiscountAmount"]));
                        objVO.StaffParentDiscountPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffParentDiscountPercent"]));
                        if (objVO.StaffParentDiscountPercent == 0) objVO.StaffParentDiscountAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffParentDiscountAmount"]));
                        objVO.ServiceTaxPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["ServiceTaxPercent"]));
                        if (objVO.ServiceTaxPercent == 0) objVO.ServiceTaxAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ServiceTaxAmount"]));
                        objVO.Discount = Convert.ToDouble(DALHelper.HandleDBNull(reader["Discount"]));
                        objVO.StaffFree = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffFree"]));
                        objVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                        objVO.ServicePaidAmount = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["PaidAmount"]));
                        objVO.SettleNetAmount = objVO.NetAmount;
                        objVO.IsCancelled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCancelled"]));
                        objVO.CancellationRemark = Convert.ToString(DALHelper.HandleDBNull(reader["CancellationRemark"]));
                        objVO.CancelledBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["CancelledBy"]));
                        objVO.CancelledDate = (DateTime?)DALHelper.HandleDate(reader["CancelledDate"]);
                        objVO.IsBilled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBilled"]));
                        objVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objVO.TariffServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffServiceID"]));
                        objVO.ServiceSpecilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"]));
                        objVO.RateEditable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["RateEditable"]));
                        objVO.MaxRate = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["MaxRate"]));
                        objVO.MinRate = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["MinRate"]));
                        objVO.BalanceAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmount"]));
                        objVO.isPackageService = Convert.ToBoolean(DALHelper.HandleDBNull(reader["isPackageService"]));
                        objVO.PackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageID"]));
                        objVO.ParentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ParentID"]));
                        objVO.TariffId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffId"]));
                        objVO.ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]));
                        objVO.IsAutoCharge = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAutoCharge"]));
                        objVO.ClassId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassId"]));
                        if (objVO.ParentID > 0)
                        {
                            objVO.ChildPackageService = true;
                        }

                        objVO.GrossDiscountPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["GrossDiscountPercent"]));
                        objVO.GrossDiscountReasonID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["GrossDiscountReason"]));

                        //Properties use for unfreezed Patho,Radio services
                        objVO.POBDID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POBDID"]));
                        objVO.IsSampleCollected = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleCollected"]));
                        objVO.POBID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POBID"]));
                        objVO.ROBDID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ROBDID"]));
                        objVO.IsReportCollected = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsReportCollected"]));
                        objVO.ROBID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ROBID"]));

                        objVO.ConditionTypeID = (long)DALHelper.HandleIntegerNull(reader["ConditionTypeID"]);  //used to identify that service is add from Conditional Service Search with AND/OR
                        objVO.ApprovalRemark = Convert.ToString(DALHelper.HandleIntegerNull(reader["ApprovalRemark"]));
                        objVO.ApprovalStatus = Convert.ToBoolean(DALHelper.HandleIntegerNull(reader["ApprovalStatus"]));
                        BizActionObj.List.Add(objVO); //new MasterListItem((long)reader["Id"], reader["Description"].ToString(), (DateTime?)DALHelper.HandleDate(reader["Date"])));
                    }
                }
                //if (!reader.IsClosed)
                //{
                //    reader.Close();
                //}
                reader.Close();

            }
            catch (Exception ex)
            {
                string err = ex.Message;
                //"Error Occured";
                //logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }


            return BizActionObj;

        }

        //............


        public override IValueObject GetChargeListAgainstBills(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetChargeAgainstBillListBizActionVO BizActionObj = (clsGetChargeAgainstBillListBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetChargesAgainstBill");

                dbServer.AddInParameter(command, "BillID", DbType.String, BizActionObj.BillID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                //Check whether the reader contains the records
                if (reader.HasRows)
                {
                    //if masterlist instance is null then creates new instance
                    if (BizActionObj.List == null)
                    {
                        BizActionObj.List = new List<clsChargeVO>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        //Add the object value in list
                        clsChargeVO objVO = new clsChargeVO();
                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objVO.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);
                        objVO.Opd_Ipd_External_Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"]));
                        objVO.Opd_Ipd_External = (Int16)DALHelper.HandleDBNull(reader["Opd_Ipd_External"]);
                        objVO.TariffServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffServiceId"]));
                        objVO.ServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceId"]));
                        objVO.ServiceSpecilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"]));
                        objVO.ServiceName = (string)DALHelper.HandleDBNull(reader["ServiceName"]);
                        objVO.DoctorId = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorId"]));
                        objVO.SelectedDoctor.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorId"]));
                        objVO.SelectedDoctor.Description = (string)DALHelper.HandleDBNull(reader["DoctorName"]);
                        objVO.Rate = (double)DALHelper.HandleDBNull(reader["Rate"]);
                        objVO.Quantity = (double)DALHelper.HandleDBNull(reader["Quantity"]);
                        objVO.TotalAmount = (double)DALHelper.HandleDBNull(reader["TotalAmount"]);
                        objVO.ConcessionPercent = (double)DALHelper.HandleDBNull(reader["ConcessionPercent"]);
                        if (objVO.ConcessionPercent == 0) objVO.Concession = (double)DALHelper.HandleDBNull(reader["ConcessionAmount"]);
                        objVO.StaffDiscountPercent = (double)DALHelper.HandleDBNull(reader["StaffDiscountPercent"]);
                        if (objVO.StaffDiscountPercent == 0) objVO.StaffDiscountAmount = (double)DALHelper.HandleDBNull(reader["StaffDiscountAmount"]);
                        objVO.StaffParentDiscountPercent = (double)DALHelper.HandleDBNull(reader["StaffParentDiscountPercent"]);
                        if (objVO.StaffParentDiscountPercent == 0) objVO.StaffParentDiscountAmount = (double)DALHelper.HandleDBNull(reader["StaffParentDiscountAmount"]);
                        objVO.ServiceTaxPercent = (double)DALHelper.HandleDBNull(reader["ServiceTaxPercent"]);
                        if (objVO.ServiceTaxPercent == 0) objVO.ServiceTaxAmount = (double)DALHelper.HandleDBNull(reader["ServiceTaxAmount"]);
                        objVO.Discount = (double)DALHelper.HandleDBNull(reader["Discount"]);
                        objVO.StaffFree = (double)DALHelper.HandleDBNull(reader["StaffFree"]);
                        objVO.NetAmount = (double)DALHelper.HandleDBNull(reader["NetAmount"]);
                        objVO.ServicePaidAmount = (double)DALHelper.HandleDBNull(reader["PaidAmount"]);
                        objVO.SettleNetAmount = objVO.NetAmount;
                        objVO.IsCancelled = (bool)DALHelper.HandleDBNull(reader["IsCancelled"]);
                        objVO.CancellationRemark = (string)DALHelper.HandleDBNull(reader["CancellationRemark"]);
                        objVO.CancelledBy = (long?)DALHelper.HandleDBNull(reader["CancelledBy"]);
                        objVO.CancelledDate = (DateTime?)DALHelper.HandleDate(reader["CancelledDate"]);
                        objVO.IsBilled = (bool)DALHelper.HandleDBNull(reader["IsBilled"]);
                        objVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        objVO.TariffServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffServiceID"]));
                        objVO.ServiceSpecilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"]));
                        objVO.RateEditable = (bool)DALHelper.HandleDBNull(reader["RateEditable"]);
                        objVO.MaxRate = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["MaxRate"]));
                        objVO.MinRate = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["MinRate"]));
                        objVO.BalanceAmount = (double)DALHelper.HandleDBNull(reader["BalanceAmount"]);
                        objVO.isPackageService = (bool)DALHelper.HandleDBNull(reader["isPackageService"]);
                        objVO.PackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageID"]));
                        objVO.ParentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ParentID"]));
                        objVO.TariffId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffId"]));
                        if (objVO.ParentID > 0)
                        {
                            objVO.ChildPackageService = true;
                        }
                        objVO.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillID"]));
                        objVO.BillUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillUnitID"]));

                        BizActionObj.List.Add(objVO);
                    }
                }

                reader.Close();

            }
            catch (Exception ex)
            {
                string err = ex.Message;

            }
            finally
            {

            }


            return BizActionObj;
        }

    }
}
