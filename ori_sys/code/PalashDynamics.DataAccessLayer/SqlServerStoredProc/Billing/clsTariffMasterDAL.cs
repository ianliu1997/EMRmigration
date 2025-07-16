using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.ValueObjects;
using System.Data.Common;
using System.Data;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Patient;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsTariffMasterDAL : clsBaseTariffMasterDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsTariffMasterDAL()
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

        public override IValueObject AddTariff(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddTariffMasterBizActionVO BizActionObj = valueObject as clsAddTariffMasterBizActionVO;

            if (BizActionObj.TariffDetails.ID == 0)
            {
                BizActionObj = AddTariffDetails(BizActionObj, UserVo);
            }
            else
            {
                BizActionObj = UpdateTariffDetails(BizActionObj, UserVo);
            }
            

            return valueObject;
        }

        private clsAddTariffMasterBizActionVO AddTariffDetails(clsAddTariffMasterBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();


                clsTariffMasterBizActionVO ObjTariff = BizActionObj.TariffDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddTariffMaster");

                dbServer.AddInParameter(command, "Code", DbType.String, ObjTariff.Code.Trim());
                dbServer.AddInParameter(command, "Description", DbType.String, ObjTariff.Description.Trim());
                dbServer.AddInParameter(command, "NoOfVisit", DbType.Int32, ObjTariff.NoOfVisit);
                dbServer.AddInParameter(command, "AllVisit", DbType.Boolean, ObjTariff.AllVisit);
                dbServer.AddInParameter(command, "Specify", DbType.Boolean, ObjTariff.Specify);
                dbServer.AddInParameter(command, "CheckDate", DbType.Boolean, ObjTariff.CheckDate);
                dbServer.AddInParameter(command, "EffectiveDate", DbType.DateTime, ObjTariff.EffectiveDate);
                dbServer.AddInParameter(command, "ExpiryDate", DbType.DateTime, ObjTariff.ExpiryDate);                
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, ObjTariff.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                //dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjTariff.ID);
                int Status = dbServer.ExecuteNonQuery(command,trans);
                BizActionObj.TariffDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

                if (ObjTariff.ServiceMasterList !=null && ObjTariff.ServiceMasterList.Count > 0)
                {
                    foreach (var ObjService in ObjTariff.ServiceMasterList)
                    {
                        //For T_TariffServiceMaster
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddTariffService");
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "TariffID", DbType.Int64, ObjTariff.ID);
                        dbServer.AddInParameter(command1, "ServiceID", DbType.Int64, ObjService.ServiceID);
                        dbServer.AddInParameter(command1, "ServiceCode", DbType.String, ObjService.ServiceCode);
                        dbServer.AddInParameter(command1, "SpecializationId", DbType.Int64, ObjService.Specialization);
                        dbServer.AddInParameter(command1, "SubSpecializationId", DbType.Int64, ObjService.SubSpecialization);
                        dbServer.AddInParameter(command1, "ShortDescription", DbType.String, ObjService.ShortDescription);
                        dbServer.AddInParameter(command1, "LongDescription", DbType.String, ObjService.LongDescription);
                        dbServer.AddInParameter(command1, "Description", DbType.String, ObjService.ServiceName);
                        dbServer.AddInParameter(command1, "CodeType", DbType.Int64, ObjService.CodeType);
                        dbServer.AddInParameter(command1, "Code", DbType.String, ObjService.Code);
                        dbServer.AddInParameter(command1, "StaffDiscount", DbType.Boolean, ObjService.StaffDiscount);
                        dbServer.AddInParameter(command1, "StaffDiscountAmount", DbType.Decimal, ObjService.StaffDiscountAmount);
                        dbServer.AddInParameter(command1, "StaffDiscountPercent", DbType.Decimal, ObjService.StaffDiscountPercent);

                        dbServer.AddInParameter(command1, "StaffDependantDiscount", DbType.Boolean, ObjService.StaffDependantDiscount);
                        dbServer.AddInParameter(command1, "StaffDependantDiscountAmount", DbType.Decimal, ObjService.StaffDependantDiscountAmount);
                        dbServer.AddInParameter(command1, "StaffDependantDiscountPercent", DbType.Decimal, ObjService.StaffDependantDiscountPercent);


                        dbServer.AddInParameter(command1, "Concession", DbType.Boolean, ObjService.Concession);
                        dbServer.AddInParameter(command1, "ConcessionAmount", DbType.Decimal, ObjService.ConcessionAmount);
                        dbServer.AddInParameter(command1, "ConcessionPercent", DbType.Decimal, ObjService.ConcessionPercent);

                        dbServer.AddInParameter(command1, "ServiceTax", DbType.Boolean, ObjService.ServiceTax);
                        dbServer.AddInParameter(command1, "ServiceTaxAmount", DbType.Decimal, ObjService.ServiceTaxAmount);
                        dbServer.AddInParameter(command1, "ServiceTaxPercent", DbType.Decimal, ObjService.ServiceTaxPercent);


                        dbServer.AddInParameter(command1, "InHouse", DbType.Boolean, ObjService.InHouse);
                        dbServer.AddInParameter(command1, "DoctorShare", DbType.Boolean, ObjService.DoctorShare);
                        dbServer.AddInParameter(command1, "DoctorSharePercentage", DbType.Decimal, ObjService.DoctorSharePercentage);
                        dbServer.AddInParameter(command1, "DoctorShareAmount", DbType.Decimal, ObjService.DoctorShareAmount);
                        dbServer.AddInParameter(command1, "RateEditable", DbType.Boolean, ObjService.RateEditable);
                        dbServer.AddInParameter(command1, "MaxRate", DbType.Decimal, ObjService.MaxRate);
                        dbServer.AddInParameter(command1, "MinRate", DbType.Decimal, ObjService.MinRate);
                        dbServer.AddInParameter(command1, "Rate", DbType.Decimal, ObjService.Rate);
                        dbServer.AddInParameter(command1, "CheckedAllTariffs", DbType.Boolean, ObjService.CheckedAllTariffs);

                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, ObjService.SelectService);
                        dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, ObjService.CreatedUnitID);
                        dbServer.AddInParameter(command1, "UpdatedUnitID", DbType.Int64, ObjService.UpdatedUnitID);
                        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, ObjService.AddedBy);
                        dbServer.AddInParameter(command1, "AddedOn", DbType.String, ObjService.AddedOn);
                        dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, ObjService.AddedDateTime);
                        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, ObjService.AddedWindowsLoginName);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, 0);

                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjService.ID);
                        int iStatus = dbServer.ExecuteNonQuery(command1,trans);
                        ObjService.ID = (long)dbServer.GetParameterValue(command1, "ID");


                        //For TarrifServiceClassRateDetail
                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddTariffServiceClassRateDetail");
                        dbServer.AddInParameter(command2, "TariffServiceId", DbType.Int64, ObjService.ID);
                        dbServer.AddInParameter(command2, "ClassId", DbType.Int64, 1);
                        dbServer.AddInParameter(command2, "Rate", DbType.Int64, ObjService.Rate);
                        dbServer.AddInParameter(command2, "Status", DbType.Boolean, ObjService.SelectService);
                        dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);


                        int TarrifServiceClassRateDetailStatus = dbServer.ExecuteNonQuery(command2,trans);

                    }
                }
                //-------------------------------------------For Specialization-----------------------------------------------//
                if (ObjTariff.ServiceSpecializationMasterList != null && ObjTariff.ServiceSpecializationMasterList.Count > 0)
                {
                    foreach (var ObjService in ObjTariff.ServiceSpecializationMasterList)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddTariffServieLinkingBySpecialization");
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "TariffID", DbType.Int64, ObjTariff.ID);
                        dbServer.AddInParameter(command1, "Specialization", DbType.Int64, ObjService.GroupID); //Specialization
                        dbServer.AddInParameter(command1, "SubSpecialization", DbType.Int64, ObjService.SubGroupID); //

                        dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, ObjService.CreatedUnitID);
                        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, ObjService.AddedBy);
                        dbServer.AddInParameter(command1, "AddedOn", DbType.String, ObjService.AddedOn);
                        dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, ObjService.AddedDateTime);
                        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, ObjService.AddedWindowsLoginName);
                        int iStatus = dbServer.ExecuteNonQuery(command1, trans);
                    }
                }

                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                //throw;
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.TariffDetails = null;

            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }

            return BizActionObj;
        }

        private clsAddTariffMasterBizActionVO UpdateTariffDetails(clsAddTariffMasterBizActionVO BizActionObj, clsUserVO UserVo)
        {

            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans=con.BeginTransaction();

                clsTariffMasterBizActionVO ObjTariff = BizActionObj.TariffDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateTariffMaster");

                dbServer.AddInParameter(command, "ID", DbType.Int64, ObjTariff.ID);
                dbServer.AddInParameter(command, "Code", DbType.String, ObjTariff.Code.Trim());
                dbServer.AddInParameter(command, "Description", DbType.String, ObjTariff.Description.Trim());
                dbServer.AddInParameter(command, "NoOfVisit", DbType.Int32, ObjTariff.NoOfVisit);
                dbServer.AddInParameter(command, "AllVisit", DbType.Boolean, ObjTariff.AllVisit);
                dbServer.AddInParameter(command, "Specify", DbType.Boolean, ObjTariff.Specify);
                dbServer.AddInParameter(command, "CheckDate", DbType.Boolean, ObjTariff.CheckDate);
                dbServer.AddInParameter(command, "EffectiveDate", DbType.DateTime, ObjTariff.EffectiveDate);
                dbServer.AddInParameter(command, "ExpiryDate", DbType.DateTime, ObjTariff.ExpiryDate);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                
                int intStatus = dbServer.ExecuteNonQuery(command,trans);


                if (ObjTariff.ServiceMasterList !=null && ObjTariff.ServiceMasterList.Count > 0)
                {
                    foreach (var ObjService in ObjTariff.ServiceMasterList)
                    {
                        //For T_TariffServiceMaster
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddTariffService");

                        dbServer.AddOutParameter(command1, "ID", DbType.Int64, 0);
                        dbServer.AddInParameter(command1, "TariffID", DbType.Int64, ObjTariff.ID);
                        dbServer.AddInParameter(command1, "ServiceID", DbType.Int64, ObjService.ServiceID);
                        dbServer.AddInParameter(command1, "ServiceCode", DbType.String, ObjService.ServiceCode);
                        dbServer.AddInParameter(command1, "SpecializationId", DbType.Int64, ObjService.Specialization);
                        dbServer.AddInParameter(command1, "SubSpecializationId", DbType.Int64, ObjService.SubSpecialization);
                        dbServer.AddInParameter(command1, "ShortDescription", DbType.String, ObjService.ShortDescription);
                        dbServer.AddInParameter(command1, "LongDescription", DbType.String, ObjService.LongDescription);
                        dbServer.AddInParameter(command1, "Description", DbType.String, ObjService.ServiceName);
                        dbServer.AddInParameter(command1, "CodeType", DbType.Int64, ObjService.CodeType);
                        dbServer.AddInParameter(command1, "Code", DbType.String, ObjService.Code);
                        dbServer.AddInParameter(command1, "StaffDiscount", DbType.Boolean, ObjService.StaffDiscount);
                        dbServer.AddInParameter(command1, "StaffDiscountAmount", DbType.Decimal, ObjService.StaffDiscountAmount);
                        dbServer.AddInParameter(command1, "StaffDiscountPercent", DbType.Decimal, ObjService.StaffDiscountPercent);

                        dbServer.AddInParameter(command1, "StaffDependantDiscount", DbType.Boolean, ObjService.StaffDependantDiscount);
                        dbServer.AddInParameter(command1, "StaffDependantDiscountAmount", DbType.Decimal, ObjService.StaffDependantDiscountAmount);
                        dbServer.AddInParameter(command1, "StaffDependantDiscountPercent", DbType.Decimal, ObjService.StaffDependantDiscountPercent);


                        dbServer.AddInParameter(command1, "Concession", DbType.Boolean, ObjService.Concession);
                        dbServer.AddInParameter(command1, "ConcessionAmount", DbType.Decimal, ObjService.ConcessionAmount);
                        dbServer.AddInParameter(command1, "ConcessionPercent", DbType.Decimal, ObjService.ConcessionPercent);

                        dbServer.AddInParameter(command1, "ServiceTax", DbType.Boolean, ObjService.ServiceTax);
                        dbServer.AddInParameter(command1, "ServiceTaxAmount", DbType.Decimal, ObjService.ServiceTaxAmount);
                        dbServer.AddInParameter(command1, "ServiceTaxPercent", DbType.Decimal, ObjService.ServiceTaxPercent);


                        dbServer.AddInParameter(command1, "InHouse", DbType.Boolean, ObjService.InHouse);
                        dbServer.AddInParameter(command1, "DoctorShare", DbType.Boolean, ObjService.DoctorShare);
                        dbServer.AddInParameter(command1, "DoctorSharePercentage", DbType.Decimal, ObjService.DoctorSharePercentage);
                        dbServer.AddInParameter(command1, "DoctorShareAmount", DbType.Decimal, ObjService.DoctorShareAmount);
                        dbServer.AddInParameter(command1, "RateEditable", DbType.Boolean, ObjService.RateEditable);
                        dbServer.AddInParameter(command1, "MaxRate", DbType.Decimal, ObjService.MaxRate);
                        dbServer.AddInParameter(command1, "MinRate", DbType.Decimal, ObjService.MinRate);
                        dbServer.AddInParameter(command1, "Rate", DbType.Decimal, ObjService.Rate);
                        dbServer.AddInParameter(command1, "CheckedAllTariffs", DbType.Boolean, ObjService.CheckedAllTariffs);

                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, ObjService.SelectService);
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, ObjService.CreatedUnitID);
                        dbServer.AddInParameter(command1, "UpdatedUnitID", DbType.Int64, ObjService.UpdatedUnitID);
                        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, ObjService.AddedBy);
                        dbServer.AddInParameter(command1, "AddedOn", DbType.String, ObjService.AddedOn);
                        dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, ObjService.AddedWindowsLoginName);

                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, 0);
                        int iStatus = dbServer.ExecuteNonQuery(command1,trans);
                        ObjService.ID = (long)dbServer.GetParameterValue(command1, "ID");


                        //For TarrifServiceClassRateDetail
                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddTariffServiceClassRateDetail");
                        dbServer.AddInParameter(command2, "TariffServiceId", DbType.Int64, ObjService.ID);
                        dbServer.AddInParameter(command2, "ClassId", DbType.Int64, 1);
                        dbServer.AddInParameter(command2, "Rate", DbType.Int64, ObjService.Rate);
                        dbServer.AddInParameter(command2, "Status", DbType.Boolean, ObjService.SelectService);
                        dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        int TarrifServiceClassRateDetailStatus = dbServer.ExecuteNonQuery(command2,trans);

                    }
                }
                trans.Commit();
                BizActionObj.SuccessStatus = 0;



            }
            catch (Exception ex)
            {
                //throw;
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.TariffDetails = null;
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }
            return BizActionObj;
        }

        public override IValueObject GetServiceByTariffID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetServiceByTariffIDBizActionVO BizActionObj = (clsGetServiceByTariffIDBizActionVO)valueObject;
            try
            {
                clsTariffMasterBizActionVO TariffVO = BizActionObj.TariffDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetServiceByTariffID");
                DbDataReader reader;
                dbServer.AddInParameter(command, "TariffID ", DbType.Int64, BizActionObj.TariffID);

                //By Anjali.............................................
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);

                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                //...............................................

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (BizActionObj.TariffDetails == null)
                            BizActionObj.TariffDetails = new clsTariffMasterBizActionVO();

                        BizActionObj.TariffDetails.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        BizActionObj.TariffDetails.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        BizActionObj.TariffDetails.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        BizActionObj.TariffDetails.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        BizActionObj.TariffDetails.NoOfVisit = (int)DALHelper.HandleDBNull(reader["NoOfVisit"]);
                        BizActionObj.TariffDetails.AllVisit = (bool)DALHelper.HandleDBNull(reader["AllVisit"]);
                        BizActionObj.TariffDetails.Specify = (bool)DALHelper.HandleDBNull(reader["Specify"]);
                        BizActionObj.TariffDetails.CheckDate = (bool)DALHelper.HandleDBNull(reader["CheckDate"]);
                        if(DALHelper.HandleDBNull(reader["EffectiveDate"])!=null)
                        {
                        BizActionObj.TariffDetails.EffectiveDate = (DateTime)DALHelper.HandleDBNull(reader["EffectiveDate"]);
                        }
                        if (DALHelper.HandleDBNull(reader["ExpiryDate"]) != null)
                        {
                            BizActionObj.TariffDetails.ExpiryDate = (DateTime)DALHelper.HandleDBNull(reader["ExpiryDate"]);
                        }
                        BizActionObj.TariffDetails.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                    }
                }

                reader.NextResult();

                if (reader.HasRows)
                {
                    BizActionObj.TariffDetails.ServiceMasterList = new List<clsServiceMasterVO>();
                    while (reader.Read())
                    {
                        clsServiceMasterVO objService = new clsServiceMasterVO();
                        objService.ID = (long)DALHelper.HandleDBNull(reader["Id"]);
                        objService.TariffID = (long)DALHelper.HandleDBNull(reader["TariffID"]);
                        objService.ServiceID = (long)DALHelper.HandleDBNull(reader["ServiceId"]);
                        objService.ServiceCode = (string)DALHelper.HandleDBNull(reader["ServiceCode"]);
                        objService.ServiceName = (string)DALHelper.HandleDBNull(reader["ServiceName"]);
                        objService.Rate = (decimal)DALHelper.HandleDBNull(reader["Rate"]);
                        objService.Specialization = (long)DALHelper.HandleDBNull(reader["SpecializationId"]);
                        objService.SpecializationString = (string)DALHelper.HandleDBNull(reader["Specialization"]);
                        objService.SubSpecialization = (long)DALHelper.HandleDBNull(reader["SubSpecializationId"]);
                        objService.ShortDescription = (string)DALHelper.HandleDBNull(reader["ShortDescription"]);
                        objService.LongDescription = (string)DALHelper.HandleDBNull(reader["LongDescription"]);
                        objService.SelectService = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        BizActionObj.TariffDetails.ServiceMasterList.Add(objService);
                     
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
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

        public override IValueObject GetServicesforIssue(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetServiceForIssueBizActionVO BizActionObj = (clsGetServiceForIssueBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetServicesForIssue");
                DbDataReader reader;
                dbServer.AddInParameter(command, "TariffID ", DbType.Int64, BizActionObj.TariffID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ServiceList == null)
                        BizActionObj.ServiceList = new List<clsPatientServiceDetails>();
                    while (reader.Read())
                    {
                        clsPatientServiceDetails objService = new clsPatientServiceDetails();

                        objService.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objService.TariffID = (long)DALHelper.HandleDBNull(reader["TariffID"]);
                        objService.ServiceID = (long)DALHelper.HandleDBNull(reader["ServiceId"]);
                        objService.ServiceCode = (string)DALHelper.HandleDBNull(reader["ServiceCode"]);
                        objService.ServiceName = (string)DALHelper.HandleDBNull(reader["ServiceName"]);
                        objService.Rate =Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["Rate"]));
                        objService.SelectService = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        BizActionObj.ServiceList.Add(objService);

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

        public override IValueObject GetTariffList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetTariffListBizActionVO BizActionObj = (clsGetTariffListBizActionVO)valueObject;
            try
            {
                DbDataReader reader = null;
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetTariffDetails");
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizActionObj.SearchExpression);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.TariffList == null)
                        BizActionObj.TariffList = new List<clsTariffMasterBizActionVO>();
                    while (reader.Read())
                    {
                        clsTariffMasterBizActionVO objTariffVO = new clsTariffMasterBizActionVO();
                        objTariffVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objTariffVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objTariffVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objTariffVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        BizActionObj.TariffList.Add(objTariffVO);
                    }
                }
                reader.NextResult(); 
                BizActionObj.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");

            }
            catch(Exception)
            {
                throw;
                
            }
            finally
            {

            }
            return BizActionObj;
        }

        public override IValueObject GetSpecializationsByTariffId(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetSpecializationsByTariffIdBizActionVO BizActionObj = (clsGetSpecializationsByTariffIdBizActionVO)valueObject;
            try
            {
                DbDataReader reader = null;
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetSpecializationsByTariffID");
                dbServer.AddInParameter(command, "TariffID", DbType.Int64, BizActionObj.TariffID);
                dbServer.AddInParameter(command, "IsFromTariffCopyUtility", DbType.Boolean, BizActionObj.IsFromTariffCopyUtility);

                dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizActionObj.SearchExpression);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);

                if (!BizActionObj.IsFromTariffCopyUtility)  //This flag is coming from frmBulkRateChange.xaml
                {
                    reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        if (BizActionObj.TariffList == null)
                            BizActionObj.TariffList = new List<clsTariffMasterBizActionVO>();
                        while (reader.Read())
                        {
                            clsTariffMasterBizActionVO objTariffVO = new clsTariffMasterBizActionVO();
                            objTariffVO.GroupID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"]));
                            objTariffVO.StrGroup = Convert.ToString(DALHelper.HandleDBNull(reader["SpecializationName"]));
                            objTariffVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffId"]));
                            BizActionObj.TariffList.Add(objTariffVO);
                        }
                        reader.NextResult();
                        BizActionObj.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");
                    }
                }
                else
                {
                    reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        if (BizActionObj.SpecializationList == null)
                            BizActionObj.SpecializationList = new List<clsSubSpecializationVO>();
                        while (reader.Read())
                        {
                            clsSubSpecializationVO objSpecializationVO = new clsSubSpecializationVO();
                            objSpecializationVO.SpecializationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            objSpecializationVO.SpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                            objSpecializationVO.SubSpecializationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["SuSpecializationID"]));
                            objSpecializationVO.SubSpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecializationName"]));

                            BizActionObj.SpecializationList.Add(objSpecializationVO);

                            //clsTariffMasterBizActionVO objTariffVO = new clsTariffMasterBizActionVO();
                            //objTariffVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            //objTariffVO.StrGroup = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                            //objTariffVO.SubGroupID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SuSpecializationID"]));
                            //objTariffVO.StrSubGroup = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecializationName"]));

                            //BizActionObj.TariffList.Add(objTariffVO);
                        }
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");
                }
                

            }
            catch (Exception)
            {
                throw;

            }
            finally
            {

            }
            return BizActionObj;
        }
  
    }
}
