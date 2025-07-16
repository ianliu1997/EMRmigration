using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using PalashDynamics.ValueObjects.Administration.StaffMaster;


namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsStaffMasterDAL : clsBaseStaffMasterDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        #endregion

        private clsStaffMasterDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql object
                //Create Instance of the database object and BaseSql object
                if (dbServer == null)
                {
                    dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override IValueObject AddStaffMaster(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddStaffMasterBizActionVO BizActionobj = valueObject as clsAddStaffMasterBizActionVO;


            if (BizActionobj.StaffDetails.ID == 0)
            {
                BizActionobj = AddStaffMaster(BizActionobj, objUserVO);
            }

            else
            {
                BizActionobj = UpdateStaffMaster(BizActionobj, objUserVO);

            }

            return BizActionobj;
        }

        private clsAddStaffMasterBizActionVO AddStaffMaster(clsAddStaffMasterBizActionVO BizActionObj, clsUserVO objUserVO)
        {
            try
            {
                clsStaffMasterVO ObjStaffVO = BizActionObj.StaffDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddStaffMaster");

                dbServer.AddInParameter(command, "FirstName", DbType.String, ObjStaffVO.FirstName.Trim());
                if(ObjStaffVO.MiddleName !=null)
                    dbServer.AddInParameter(command, "MiddleName", DbType.String, ObjStaffVO.MiddleName.Trim());
                dbServer.AddInParameter(command, "LastName", DbType.String, ObjStaffVO.LastName.Trim());
                dbServer.AddInParameter(command, "DOB", DbType.DateTime, ObjStaffVO.DOB);
                dbServer.AddInParameter(command, "DesignationID", DbType.Int64, ObjStaffVO.DesignationID);
                dbServer.AddInParameter(command, "EmailId", DbType.String, ObjStaffVO.EmailId);
                dbServer.AddInParameter(command, "GenderID", DbType.Int64, ObjStaffVO.GenderID);

                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);

                //rohinee
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, ObjStaffVO.DepartmentID);
                dbServer.AddInParameter(command, "EmployeeNumber", DbType.String, ObjStaffVO.EmployeeNumber);
                dbServer.AddInParameter(command, "PFNumber", DbType.String, ObjStaffVO.PFNumber);
                dbServer.AddInParameter(command, "MaritalStatusId", DbType.Int32, ObjStaffVO.MaritalStatusId);
                dbServer.AddInParameter(command, "Photo", DbType.Binary, ObjStaffVO.Photo);
                dbServer.AddInParameter(command, "DateOfJoining", DbType.DateTime, ObjStaffVO.DateofJoining);
                dbServer.AddInParameter(command, "AccessCardNumber", DbType.String, ObjStaffVO.AccessCardNumber);
                dbServer.AddInParameter(command, "PANNumber", DbType.String, ObjStaffVO.PANNumber);
                dbServer.AddInParameter(command, "Education", DbType.String, ObjStaffVO.Education);
                dbServer.AddInParameter(command, "Experience", DbType.String, ObjStaffVO.Experience);
                dbServer.AddInParameter(command, "Department", DbType.String, ObjStaffVO.DepartmentName);            
                dbServer.AddInParameter(command, "ClinicId", DbType.Int64, ObjStaffVO.ClinicId);
                //

                dbServer.AddInParameter(command, "UnitID", DbType.Int64,objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, ObjStaffVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                //Added by AJ Date 10/11/2016
                dbServer.AddInParameter(command, "IsApplicableAdvise", DbType.Boolean, ObjStaffVO.IsDischargeApprove);
                dbServer.AddInParameter(command, "IsMarketingExecutives", DbType.Boolean, ObjStaffVO.IsMarketingExecutives);
                //***//-----------------
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjStaffVO.ID);

                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.StaffDetails.ID = (long)dbServer.GetParameterValue(command, "ID");        

              
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

        private clsAddStaffMasterBizActionVO UpdateStaffMaster(clsAddStaffMasterBizActionVO BizActionObj, clsUserVO objUserVO)
        {
            try
            {
                clsStaffMasterVO ObjStaffVO = BizActionObj.StaffDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateStaffMaster");

                dbServer.AddInParameter(command, "ID", DbType.String, ObjStaffVO.ID);
                dbServer.AddInParameter(command, "FirstName", DbType.String, ObjStaffVO.FirstName.Trim());
                if (ObjStaffVO.MiddleName != null)
                    dbServer.AddInParameter(command, "MiddleName", DbType.String, ObjStaffVO.MiddleName.Trim());
                dbServer.AddInParameter(command, "LastName", DbType.String, ObjStaffVO.LastName.Trim());
                dbServer.AddInParameter(command, "DOB", DbType.DateTime, ObjStaffVO.DOB);
                dbServer.AddInParameter(command, "GenderID", DbType.Int64, ObjStaffVO.GenderID);
                dbServer.AddInParameter(command, "DesignationID", DbType.Int64, ObjStaffVO.DesignationID);
                dbServer.AddInParameter(command, "EmailId", DbType.String, ObjStaffVO.EmailId);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);

                //rohinee
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, ObjStaffVO.DepartmentID);
                dbServer.AddInParameter(command, "EmployeeNumber", DbType.String, ObjStaffVO.EmployeeNumber);
                dbServer.AddInParameter(command, "PFNumber", DbType.String, ObjStaffVO.PFNumber);
                dbServer.AddInParameter(command, "MaritalStatusId", DbType.Int32, ObjStaffVO.MaritalStatusId);
                dbServer.AddInParameter(command, "Photo", DbType.Binary, ObjStaffVO.Photo);
                dbServer.AddInParameter(command, "DateOfJoining", DbType.DateTime, ObjStaffVO.DateofJoining);
                dbServer.AddInParameter(command, "AccessCardNumber", DbType.String, ObjStaffVO.AccessCardNumber);
                dbServer.AddInParameter(command, "PANNumber", DbType.String, ObjStaffVO.PANNumber);
                dbServer.AddInParameter(command, "Education", DbType.String, ObjStaffVO.Education);
                dbServer.AddInParameter(command, "Experience", DbType.String, ObjStaffVO.Experience);
                dbServer.AddInParameter(command, "Department", DbType.String, ObjStaffVO.DepartmentName);
                dbServer.AddInParameter(command, "ClinicId", DbType.Int64, ObjStaffVO.ClinicId);
                //

                //Added by AJ Date 10/11/2016
                dbServer.AddInParameter(command, "IsApplicableAdvise", DbType.Boolean, ObjStaffVO.IsDischargeApprove);
                //***//-----------------

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);


                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, ObjStaffVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                int intStatus = dbServer.ExecuteNonQuery(command);

                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
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

        public override IValueObject GetStaffMasterList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetStaffMasterDetailsBizActionVO BizActionObj = (clsGetStaffMasterDetailsBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetStaffMasterDetailsList");

                if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                    dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.FirstName + "%");

                if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.LastName + "%");

                if (BizActionObj.StrClinicID != null && BizActionObj.StrClinicID.Length != 0)
                    dbServer.AddInParameter(command, "StrClinicID", DbType.String, BizActionObj.StrClinicID);

                dbServer.AddInParameter(command, "DesignationID ", DbType.Int64, BizActionObj.DesignationID);

                dbServer.AddInParameter(command, "ClinicId ", DbType.Int64, BizActionObj.ClinicId);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.StaffMasterList == null)
                        BizActionObj.StaffMasterList = new List<clsStaffMasterVO>();
                    while (reader.Read())
                    {
                        clsStaffMasterVO StaffMasterVO = new clsStaffMasterVO();
                        StaffMasterVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        StaffMasterVO.FirstName = (string)DALHelper.HandleDBNull(reader["FirstName"]);
                        StaffMasterVO.MiddleName = (string)DALHelper.HandleDBNull(reader["MiddleName"]);
                        StaffMasterVO.LastName = (string)DALHelper.HandleDBNull(reader["LastName"]);
                        StaffMasterVO.DOB = (DateTime?)DALHelper.HandleDate(reader["DOB"]);
                        StaffMasterVO.GenderID = (long)DALHelper.HandleDBNull(reader["GenderID"]);
                        StaffMasterVO.DesignationID = (long)DALHelper.HandleDBNull(reader["Designation"]);

                        //By Anjali....... On 11/04/2014
                        StaffMasterVO.DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"]));
                        StaffMasterVO.IsApplicableAdvise = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsApplicableAdvise"]));

                        StaffMasterVO.Designation = (string)DALHelper.HandleDBNull(reader["Description"]);
                        StaffMasterVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        StaffMasterVO.Value = (string)DALHelper.HandleDBNull(reader["Value"]);
                        StaffMasterVO.EmailId = (string)DALHelper.HandleDBNull(reader["EmailId"]);
                        //StaffMasterVO.ClinicId = (long)DALHelper.HandleDBNull(reader["ClinicId"]);
                        StaffMasterVO.ClinicName = (string)DALHelper.HandleDBNull(reader["ClinicName"]);
                        BizActionObj.StaffMasterList.Add(StaffMasterVO);
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
            return BizActionObj;
        }

        public override IValueObject GetStaffMasterByID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetStaffMasterDetailsByIDBizActionVO BizAction = valueObject as clsGetStaffMasterDetailsByIDBizActionVO;
            try
            {
                clsStaffMasterVO ObjStaffVo = BizAction.StaffMasterList;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetStaffMasterDetailsByStaffID");
                DbDataReader reader;

                dbServer.AddInParameter(command, "StaffID ", DbType.Int64, BizAction.StaffId);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (BizAction.StaffMasterList == null)
                            BizAction.StaffMasterList = new clsStaffMasterVO();

                        BizAction.StaffMasterList.ID =Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.StaffMasterList.FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]));
                        BizAction.StaffMasterList.MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"]));
                        BizAction.StaffMasterList.LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]));
                        BizAction.StaffMasterList.DOB = Convert.ToDateTime(DALHelper.HandleDate(reader["DOB"]));
                        BizAction.StaffMasterList.GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]));
                        BizAction.StaffMasterList.DesignationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Designation"]));
                        BizAction.StaffMasterList.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizAction.StaffMasterList.EmailId =Convert.ToString(DALHelper.HandleDBNull(reader["EmailId"]));
                        //rohinee
                       // BizAction.StaffMasterList.DepartmentID = (long)DALHelper.HandleDBNull(reader["    "]);
                        BizAction.StaffMasterList.PFNumber = Convert.ToString(DALHelper.HandleDBNull(reader["PFNumber"]));
                        BizAction.StaffMasterList.MaritalStatusId = Convert.ToInt64(DALHelper.HandleDBNull(reader["MaritalStatusId"]));
                        BizAction.StaffMasterList.Photo = (byte[])DALHelper.HandleDBNull(reader["Photo"]);
                        BizAction.StaffMasterList.DateofJoining = Convert.ToDateTime(DALHelper.HandleDBNull(reader["DateofJoining"]));
                        BizAction.StaffMasterList.AccessCardNumber =Convert.ToString(DALHelper.HandleDBNull(reader["AccessCardNumber"]));
                        BizAction.StaffMasterList.EmployeeNumber = Convert.ToString(DALHelper.HandleDBNull(reader["EmployeeNumber"]));
                        BizAction.StaffMasterList.PANNumber = Convert.ToString(DALHelper.HandleDBNull(reader["PANNumber"]));
                        BizAction.StaffMasterList.Education = Convert.ToString(DALHelper.HandleDBNull(reader["Education"]));
                        BizAction.StaffMasterList.Experience = Convert.ToString(DALHelper.HandleDBNull(reader["Experience"]));
                        BizAction.StaffMasterList.DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"]));
                        BizAction.StaffMasterList.ClinicId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClinicId"]));
                        //

                        //Added by AJ Date 10/11/2016
                        BizAction.StaffMasterList.IsDischargeApprove = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsApplicableAdvise"]));
                      
                        //***//-----------------
                        
                       
                     
                     
                    }
                }
               // reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return BizAction;
        }

        public override IValueObject GetStaffByUnitIDandID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetStaffMasterByUnitIDBizActionVO BizAction = (clsGetStaffMasterByUnitIDBizActionVO)valueObject;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetStaffByUnitIDandID");
                DbDataReader reader;
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.ID);
                dbServer.AddInParameter(command, "UnitID ", DbType.Int64, BizAction.UnitID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    reader.Read();
                    clsStaffMasterVO StaffMasterVO = new clsStaffMasterVO();
                    StaffMasterVO.StaffName = (string)DALHelper.HandleDBNull(reader["StaffName"]);
                    StaffMasterVO.Designation = (string)DALHelper.HandleDBNull(reader["Designation"]);
                    StaffMasterVO.DOB = (DateTime)DALHelper.HandleDate(reader["DOB"]);
                    StaffMasterVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                    StaffMasterVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                    StaffMasterVO.FirstName = (string)DALHelper.HandleDBNull(reader["FirstName"]);
                    StaffMasterVO.MiddleName = (string)DALHelper.HandleDBNull(reader["MiddleName"]);
                    StaffMasterVO.LastName = (string)DALHelper.HandleDBNull(reader["LastName"]);
                    StaffMasterVO.BloodGroupId = (long)DALHelper.HandleDBNull(reader["BloodGroupId"]);
                    StaffMasterVO.AgeDays = (long)DALHelper.HandleDBNull(reader["AgeDays"]);
                    StaffMasterVO.AgeMonth = (long)DALHelper.HandleDBNull(reader["AgeMonth"]);
                    StaffMasterVO.AgeYears = (long)DALHelper.HandleDBNull(reader["AgeYears"]);
                    StaffMasterVO.GenderID = (long)DALHelper.HandleDBNull(reader["GenderID"]);
                    StaffMasterVO.MaritalStatusId = (long)DALHelper.HandleDBNull(reader["MaritalStatusId"]);
                    StaffMasterVO.ReligionId = (long)DALHelper.HandleDBNull(reader["ReligionId"]);
                    StaffMasterVO.EmailId = (string)DALHelper.HandleDBNull(reader["EmailId"]);
                    StaffMasterVO.MobileNo = (long)DALHelper.HandleDBNull(reader["MobileNo"]);
                    StaffMasterVO.MobileCountryCode = (long)DALHelper.HandleDBNull(reader["MobileCountryCode"]);
                    StaffMasterVO.ResiNoCountryCode = (long)DALHelper.HandleDBNull(reader["ResiNoCountryCode"]);
                    StaffMasterVO.ResiSTDCode = (long)DALHelper.HandleDBNull(reader["ResiSTDCode"]);
                    StaffMasterVO.ResidenceNo = (long)DALHelper.HandleDBNull(reader["ResidenceNo"]);
                    StaffMasterVO.PrefixID = (long)DALHelper.HandleDBNull(reader["PrefixID"]);
                    BizAction.StaffMasterList.Add(StaffMasterVO);
                    reader.NextResult();
                    while (reader.Read())
                    {
                        clsStaffAddressDetailsVO StaffAdressVO = new clsStaffAddressDetailsVO();
                        StaffAdressVO.Address = (string)DALHelper.HandleDBNull(reader["Address"]);
                        StaffAdressVO.AddressTypeID = (long)DALHelper.HandleDBNull(reader["AddressType"]);
                        BizAction.StaffAddressList.Add(StaffAdressVO);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return BizAction;
        }

        public override IValueObject GetStaffByUnitID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetStaffMasterByUnitIDBizActionVO BizAction = (clsGetStaffMasterByUnitIDBizActionVO)valueObject;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetStaffByUnitID");
                DbDataReader reader;

                dbServer.AddInParameter(command, "UnitID ", DbType.Int64, BizAction.UnitID);
                dbServer.AddInParameter(command, "DesignationID ", DbType.Int64, BizAction.DesignationID);
                dbServer.AddInParameter(command, "FromNurseSchedule ", DbType.Int16, BizAction.FromNurseSchedule);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsStaffMasterVO StaffMasterVO = new clsStaffMasterVO();
                        StaffMasterVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        StaffMasterVO.StaffNo = reader["StaffNo"].ToString();
                        StaffMasterVO.FirstName = reader["FirstName"].ToString();
                        StaffMasterVO.LastName = reader["LastName"].ToString();
                        StaffMasterVO.StaffName = reader["SataffName"].ToString();
                        StaffMasterVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        StaffMasterVO.GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]));
                        StaffMasterVO.UnitName = reader["Name"].ToString();
                        BizAction.StaffMasterList.Add(StaffMasterVO);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return BizAction;
        }

        public override IValueObject GetUserSearchList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetUserSearchBizActionVO BizAction = (clsGetUserSearchBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetUnitWiseUserSearch");
                DbDataReader reader;
                dbServer.AddInParameter(command, "UnitID ", DbType.Int64, BizAction.UnitID);
                dbServer.AddInParameter(command, "FirstName", DbType.String, BizAction.FirstName);
                dbServer.AddInParameter(command, "LastName", DbType.String, BizAction.LastName);
                dbServer.AddInParameter(command, "DesignationID", DbType.Int64, BizAction.DesignationID);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizAction.DepartmentID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizAction.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizAction.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizAction.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizAction.sortExpression);
                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsStaffMasterVO StaffMasterVO = new clsStaffMasterVO();
                        StaffMasterVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        StaffMasterVO.UserName = (string)DALHelper.HandleDBNull(reader["UserName"]);
                        StaffMasterVO.FirstName = (string)DALHelper.HandleDBNull(reader["FirstName"]);
                        StaffMasterVO.LastName = (string)DALHelper.HandleDBNull(reader["LastName"]);
                        StaffMasterVO.UserID = (long)DALHelper.HandleDBNull(reader["UserID"]);
                        StaffMasterVO.UserUnitID = (long)DALHelper.HandleDBNull(reader["UserUnitID"]);
                        StaffMasterVO.Designation = (string)DALHelper.HandleDBNull(reader["Designation"]);
                        StaffMasterVO.DOB = (DateTime)DALHelper.HandleDate(reader["DOB"]);
                        StaffMasterVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);

                        StaffMasterVO.RowID = (long)DALHelper.HandleDBNull(reader["RowNum"]);
                        BizAction.StaffMasterList.Add(StaffMasterVO);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return BizAction;
        }


        //added by rohinee

        public override IValueObject AddStaffBankInfo(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsAddStaffBankInfoBizActionVO BizActionObj = (clsAddStaffBankInfoBizActionVO)valueObject;
            //BizActionObj.objDoctorBankDetail = new clsDoctorBankInfoVO();
            try
            {


                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddStaffBankDetail");
                dbServer.AddInParameter(command, "StaffId", DbType.Int64, BizActionObj.objStaffBankDetail.StaffId);
                dbServer.AddInParameter(command, "BankId", DbType.Int64, BizActionObj.objStaffBankDetail.BankId);
                dbServer.AddInParameter(command, "BranchId", DbType.Int64, BizActionObj.objStaffBankDetail.BranchId);
                dbServer.AddInParameter(command, "AccountNumber", DbType.String, BizActionObj.objStaffBankDetail.AccountNumber);
                dbServer.AddInParameter(command, "AccountType", DbType.Boolean, BizActionObj.objStaffBankDetail.AccountType);
                dbServer.AddInParameter(command, "BranchAddress", DbType.String, BizActionObj.objStaffBankDetail.BranchAddress);
                dbServer.AddInParameter(command, "MICRNumber", DbType.String, BizActionObj.objStaffBankDetail.MICRNumber);
                //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.objDoctorBankDetail.ID);
                dbServer.AddOutParameter(command, "ID", DbType.Int64, int.MaxValue);
                int intStatus2 = dbServer.ExecuteNonQuery(command);

                BizActionObj.objStaffBankDetail.ID = (long)dbServer.GetParameterValue(command, "ID");
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

        public override IValueObject UpdateStaffBankInfo(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsUpdateStaffBankInfoVO BizActionObj = (clsUpdateStaffBankInfoVO)valueObject;
            //BizActionObj.objDoctorBankDetail = new clsDoctorBankInfoVO();
            try
            {


                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateStaffBankDetail");
                dbServer.AddInParameter(command, "StaffId", DbType.Int64, BizActionObj.objStaffBankDetail.StaffId);
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.objStaffBankDetail.ID);
                dbServer.AddInParameter(command, "BankId", DbType.Int64, BizActionObj.objStaffBankDetail.BankId);
                dbServer.AddInParameter(command, "BranchId", DbType.Int64, BizActionObj.objStaffBankDetail.BranchId);
                dbServer.AddInParameter(command, "AccountNumber", DbType.String, BizActionObj.objStaffBankDetail.AccountNumber);
                dbServer.AddInParameter(command, "AccountType", DbType.Boolean, BizActionObj.objStaffBankDetail.AccountType);
                dbServer.AddInParameter(command, "BranchAddress", DbType.String, BizActionObj.objStaffBankDetail.BranchAddress);
                dbServer.AddInParameter(command, "MICRNumber", DbType.String, BizActionObj.objStaffBankDetail.MICRNumber);
                //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.objDoctorBankDetail.ID);
                //dbServer.AddOutParameter(command, "ID", DbType.Int64, int.MaxValue);
                int intStatus2 = dbServer.ExecuteNonQuery(command);

                BizActionObj.objStaffBankDetail.ID = (long)dbServer.GetParameterValue(command, "ID");
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

        public override IValueObject GetStaffBankInfo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetStaffBankInfoBizActionVO BizActionObj = (clsGetStaffBankInfoBizActionVO)valueObject;

            try
            {
                clsStaffBankInfoVO ObjDoctorBankVo = BizActionObj.objStaffBankDetail;
                //clsDoctorBankInfoVO objdoctoebankinfo = new clsDoctorBankInfoVO();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetStaffBankInfo");
                DbDataReader reader;
                dbServer.AddInParameter(command, "StaffId", DbType.Int64, BizActionObj.StaffID);
                 reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.StaffBankDetailList == null)
                        BizActionObj.StaffBankDetailList = new List<clsStaffBankInfoVO>();
                    while (reader.Read())
                    {

                        clsStaffBankInfoVO objdoctoebankinfo = new clsStaffBankInfoVO();
                        objdoctoebankinfo.StaffId = (long)DALHelper.HandleDBNull(reader["StaffId"]);
                        objdoctoebankinfo.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objdoctoebankinfo.BankName = Convert.ToString(DALHelper.HandleDBNull(reader["BankName"]));
                        objdoctoebankinfo.BankId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BankId"]));
                        objdoctoebankinfo.BranchName = Convert.ToString(DALHelper.HandleDBNull(reader["BankBranchName"]));
                        objdoctoebankinfo.BranchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BankBrachId"]));
                        objdoctoebankinfo.AccountNumber = Convert.ToString(DALHelper.HandleDBNull(reader["AccountNumber"]));
                        objdoctoebankinfo.BranchAddress = Convert.ToString(DALHelper.HandleDBNull(reader["Address"]));
                        objdoctoebankinfo.AccountTypeName = Convert.ToString(DALHelper.HandleDBNull(reader["AccountType"]));
                        objdoctoebankinfo.StaffName = Convert.ToString(DALHelper.HandleDBNull(reader["StaffName"]));
                        objdoctoebankinfo.MICRNumber = Convert.ToString(DALHelper.HandleDBNull(reader["MICRNumber"]));
                        BizActionObj.StaffBankDetailList.Add(objdoctoebankinfo);

                    }
                }


                reader.Close();
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

        public override IValueObject GetStaffBankInfoById(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetStaffBankInfoByIdVO BizActionObj = (clsGetStaffBankInfoByIdVO)valueObject;

            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetStaffBankInfoById");
                DbDataReader reader;

                dbServer.AddInParameter(command, "StaffId", DbType.Int64, BizActionObj.StaffID);
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);


                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        BizActionObj.objStaffBankDetail = new clsStaffBankInfoVO();
                        //clsDoctorBankInfoVO objdoctoebankinfo = new clsDoctorBankInfoVO();
                        BizActionObj.objStaffBankDetail.StaffId = (long)DALHelper.HandleDBNull(reader["StaffId"]);
                        BizActionObj.objStaffBankDetail.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizActionObj.objStaffBankDetail.BankName = Convert.ToString(DALHelper.HandleDBNull(reader["BankName"]));
                        BizActionObj.objStaffBankDetail.BankId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BankId"]));
                        BizActionObj.objStaffBankDetail.BranchName = Convert.ToString(DALHelper.HandleDBNull(reader["BankBranchName"]));
                        BizActionObj.objStaffBankDetail.BranchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BankBrachId"]));
                        BizActionObj.objStaffBankDetail.AccountNumber = Convert.ToString(DALHelper.HandleDBNull(reader["AccountNumber"]));
                        BizActionObj.objStaffBankDetail.BranchAddress = Convert.ToString(DALHelper.HandleDBNull(reader["Address"]));
                        BizActionObj.objStaffBankDetail.AccountTypeName = Convert.ToString(DALHelper.HandleDBNull(reader["AccountType"]));
                        BizActionObj.objStaffBankDetail.StaffName = Convert.ToString(DALHelper.HandleDBNull(reader["StaffName"]));
                        BizActionObj.objStaffBankDetail.MICRNumber = Convert.ToString(DALHelper.HandleDBNull(reader["MICRNumber"]));
                        //BizActionObj.objDoctorBankDetail.Add(objdoctoebankinfo);

                    }
                }
                reader.Close();
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

        public override IValueObject AddStaffAddressInfo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddStaffAddressInfoBizActionVO BizActionObj = (clsAddStaffAddressInfoBizActionVO)valueObject;
            //BizActionObj.objDoctorBankDetail = new clsDoctorBankInfoVO();
            try
            {
                DbCommand command;
                //if (BizActionObj.objStaffBankDetail.IsFromMarketing)
                //{
                //    command = dbServer.GetStoredProcCommand("CIMS_AddMktStaffAddressDetail");
                //    dbServer.AddInParameter(command, "MOVID", DbType.Int64, BizActionObj.objStaffBankDetail.MOVID);
                //}
                //else

                    command = dbServer.GetStoredProcCommand("CIMS_AddStaffAddressDetail");
                dbServer.AddInParameter(command, "StaffId", DbType.Int64, BizActionObj.objStaffBankDetail.StaffId);
                dbServer.AddInParameter(command, "AddressTypeID", DbType.Int64, BizActionObj.objStaffBankDetail.AddressTypeID);
                //dbServer.AddInParameter(command, "BranchId", DbType.Int64, BizActionObj.objDoctorBankDetail.BranchId);
                dbServer.AddInParameter(command, "Name", DbType.String, BizActionObj.objStaffBankDetail.Name);
                dbServer.AddInParameter(command, "Address", DbType.String, BizActionObj.objStaffBankDetail.Address);
                dbServer.AddInParameter(command, "Contact1", DbType.String, BizActionObj.objStaffBankDetail.Contact1);
                dbServer.AddInParameter(command, "Contact2", DbType.String, BizActionObj.objStaffBankDetail.Contact2);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus); 
                dbServer.AddOutParameter(command, "ID", DbType.Int64, int.MaxValue);
                // dbServer.AddInParameter(command, "isfrommkt", DbType.Boolean, BizActionObj.objDoctorBankDetail.IsFromMarketing);
                int intStatus2 = dbServer.ExecuteNonQuery(command);

                BizActionObj.objStaffBankDetail.ID= Convert.ToInt64(DALHelper.HandleDBNull(dbServer.GetParameterValue(command, "ID"))); //(long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
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

        public override IValueObject GetStaffAddressInfo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetStaffAddressInfoBizActionVO BizActionObj = (clsGetStaffAddressInfoBizActionVO)valueObject;
            try
            {
                DbCommand command;
                clsStaffAddressInfoVO ObjDoctorBankVo = BizActionObj.objStaffAddressDetail;

                //if (BizActionObj.IsfromMarketing)
                //    command = dbServer.GetStoredProcCommand("CIMS_GetMktDoctorAddressInfo");
                //else

                    command = dbServer.GetStoredProcCommand("CIMS_GetStaffAddressInfo");
                DbDataReader reader;
                dbServer.AddInParameter(command, "StaffId", DbType.Int64, BizActionObj.StaffId);
                //dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                //dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                //dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                //dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                //dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.StaffAddressDetailList == null)
                        BizActionObj.StaffAddressDetailList = new List<clsStaffAddressInfoVO>();
                    while (reader.Read())
                    {
                        clsStaffAddressInfoVO objdoctoebankinfo = new clsStaffAddressInfoVO();
                        objdoctoebankinfo.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objdoctoebankinfo.StaffId = (long)DALHelper.HandleDBNull(reader["StaffId"]);
                        objdoctoebankinfo.AddressType = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objdoctoebankinfo.AddressTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AddressType"]));
                        objdoctoebankinfo.Name = Convert.ToString(DALHelper.HandleDBNull(reader["Name"]));
                        objdoctoebankinfo.Address = Convert.ToString(DALHelper.HandleDBNull(reader["Address"]));
                        objdoctoebankinfo.Contact1 = Convert.ToString(DALHelper.HandleDBNull(reader["Contact1"]));
                        objdoctoebankinfo.Contact2 = Convert.ToString(DALHelper.HandleDBNull(reader["Contact2"]));
                        //objdoctoebankinfo.BranchAddress = Convert.ToString(DALHelper.HandleDBNull(reader["Address"]));
                        //objdoctoebankinfo.AccountTypeName = Convert.ToString(DALHelper.HandleDBNull(reader["AccountType"]));
                        //objdoctoebankinfo.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                        //objdoctoebankinfo.MICRNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["MICRNumber"]));
                        BizActionObj.StaffAddressDetailList.Add(objdoctoebankinfo);
                    }
                }
                reader.Close();
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

        public override IValueObject GetStaffAddressInfoById(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetStaffAddressInfoByIdVO BizActionObj = (clsGetStaffAddressInfoByIdVO)valueObject;
            try
            {
                DbCommand command;
                //if (BizActionObj.IsfromMarketing)
                //{
                //    command = dbServer.GetStoredProcCommand("CIMS_GetMktDoctorAddressInfoById");
                //    dbServer.AddInParameter(command, "DoctorId", DbType.Int64, BizActionObj.DoctorID);
                //}
                //else
                //{
                    command = dbServer.GetStoredProcCommand("CIMS_GetStaffAddressInfoById");
                    dbServer.AddInParameter(command, "StaffId", DbType.Int64, BizActionObj.StaffId);
                    dbServer.AddInParameter(command, "AddressTypeID", DbType.Int64, BizActionObj.AddressTypeId);
                //}

                DbDataReader reader;


                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        BizActionObj.objStaffAddressDetail = new clsStaffAddressInfoVO();

                        BizActionObj.objStaffAddressDetail.StaffId = (long)DALHelper.HandleDBNull(reader["StaffId"]);
                        BizActionObj.objStaffAddressDetail.AddressTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AddressType"]));
                        BizActionObj.objStaffAddressDetail.Name = Convert.ToString(DALHelper.HandleDBNull(reader["Name"]));
                        BizActionObj.objStaffAddressDetail.Address = Convert.ToString(DALHelper.HandleDBNull(reader["Address"]));
                        BizActionObj.objStaffAddressDetail.Contact1 = Convert.ToString(DALHelper.HandleDBNull(reader["Contact1"]));
                        BizActionObj.objStaffAddressDetail.Contact2 = Convert.ToString(DALHelper.HandleDBNull(reader["Contact2"]));


                    }
                }
                reader.Close();
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

        public override IValueObject UpdateStaffAddressInfo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateStaffAddressInfoVO BizActionObj = (clsUpdateStaffAddressInfoVO)valueObject;
            //BizActionObj.objDoctorBankDetail = new clsDoctorBankInfoVO();
            try
            {
                DbCommand command;
                //if (BizActionObj.objStaffAddressDetail.IsFromMarketing)
                //    command = dbServer.GetStoredProcCommand("CIMS_UpdateMktDoctorAddressDetail");
                //else
                    command = dbServer.GetStoredProcCommand("CIMS_UpdateStaffAddressDetail");
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.objStaffAddressDetail.ID);
                dbServer.AddInParameter(command, "StaffId", DbType.Int64, BizActionObj.objStaffAddressDetail.StaffId);
                dbServer.AddInParameter(command, "AddressTypeID", DbType.Int64, BizActionObj.objStaffAddressDetail.AddressTypeID);
                dbServer.AddInParameter(command, "Name", DbType.String, BizActionObj.objStaffAddressDetail.Name);
                dbServer.AddInParameter(command, "Address", DbType.String, BizActionObj.objStaffAddressDetail.Address);
                dbServer.AddInParameter(command, "Contact1", DbType.String, BizActionObj.objStaffAddressDetail.Contact1);
                dbServer.AddInParameter(command, "Contact2", DbType.String, BizActionObj.objStaffAddressDetail.Contact2);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus); 
                //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.objDoctorBankDetail.ID);
                //dbServer.AddOutParameter(command, "ID", DbType.Int64, int.MaxValue);
                int intStatus2 = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                //BizActionObj.objDoctorAddressDetail.ID = (long)dbServer.GetParameterValue(command, "ID");
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
