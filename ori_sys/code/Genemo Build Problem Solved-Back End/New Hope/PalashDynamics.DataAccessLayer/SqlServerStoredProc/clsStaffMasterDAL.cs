namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration.StaffMaster;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsStaffMasterDAL : clsBaseStaffMasterDAL
    {
        private Database dbServer;

        private clsStaffMasterDAL()
        {
            try
            {
                if (this.dbServer == null)
                {
                    this.dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override IValueObject AddStaffAddressInfo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddStaffAddressInfoBizActionVO nvo = (clsAddStaffAddressInfoBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddStaffAddressDetail");
                this.dbServer.AddInParameter(storedProcCommand, "StaffId", DbType.Int64, nvo.objStaffBankDetail.StaffId);
                this.dbServer.AddInParameter(storedProcCommand, "AddressTypeID", DbType.Int64, nvo.objStaffBankDetail.AddressTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "Name", DbType.String, nvo.objStaffBankDetail.Name);
                this.dbServer.AddInParameter(storedProcCommand, "Address", DbType.String, nvo.objStaffBankDetail.Address);
                this.dbServer.AddInParameter(storedProcCommand, "Contact1", DbType.String, nvo.objStaffBankDetail.Contact1);
                this.dbServer.AddInParameter(storedProcCommand, "Contact2", DbType.String, nvo.objStaffBankDetail.Contact2);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, nvo.SuccessStatus);
                this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.objStaffBankDetail.ID = Convert.ToInt64(DALHelper.HandleDBNull(this.dbServer.GetParameterValue(storedProcCommand, "ID")));
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddStaffBankInfo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddStaffBankInfoBizActionVO nvo = (clsAddStaffBankInfoBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddStaffBankDetail");
                this.dbServer.AddInParameter(storedProcCommand, "StaffId", DbType.Int64, nvo.objStaffBankDetail.StaffId);
                this.dbServer.AddInParameter(storedProcCommand, "BankId", DbType.Int64, nvo.objStaffBankDetail.BankId);
                this.dbServer.AddInParameter(storedProcCommand, "BranchId", DbType.Int64, nvo.objStaffBankDetail.BranchId);
                this.dbServer.AddInParameter(storedProcCommand, "AccountNumber", DbType.String, nvo.objStaffBankDetail.AccountNumber);
                this.dbServer.AddInParameter(storedProcCommand, "AccountType", DbType.Boolean, nvo.objStaffBankDetail.AccountType);
                this.dbServer.AddInParameter(storedProcCommand, "BranchAddress", DbType.String, nvo.objStaffBankDetail.BranchAddress);
                this.dbServer.AddInParameter(storedProcCommand, "MICRNumber", DbType.String, nvo.objStaffBankDetail.MICRNumber);
                this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.objStaffBankDetail.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        private clsAddStaffMasterBizActionVO AddStaffMaster(clsAddStaffMasterBizActionVO BizActionObj, clsUserVO objUserVO)
        {
            try
            {
                clsStaffMasterVO staffDetails = BizActionObj.StaffDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddStaffMaster");
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, staffDetails.FirstName.Trim());
                if (staffDetails.MiddleName != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, staffDetails.MiddleName.Trim());
                }
                this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, staffDetails.LastName.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "DOB", DbType.DateTime, staffDetails.DOB);
                this.dbServer.AddInParameter(storedProcCommand, "DesignationID", DbType.Int64, staffDetails.DesignationID);
                this.dbServer.AddInParameter(storedProcCommand, "EmailId", DbType.String, staffDetails.EmailId);
                this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, staffDetails.GenderID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, staffDetails.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "EmployeeNumber", DbType.String, staffDetails.EmployeeNumber);
                this.dbServer.AddInParameter(storedProcCommand, "PFNumber", DbType.String, staffDetails.PFNumber);
                this.dbServer.AddInParameter(storedProcCommand, "MaritalStatusId", DbType.Int32, staffDetails.MaritalStatusId);
                this.dbServer.AddInParameter(storedProcCommand, "Photo", DbType.Binary, staffDetails.Photo);
                this.dbServer.AddInParameter(storedProcCommand, "DateOfJoining", DbType.DateTime, staffDetails.DateofJoining);
                this.dbServer.AddInParameter(storedProcCommand, "AccessCardNumber", DbType.String, staffDetails.AccessCardNumber);
                this.dbServer.AddInParameter(storedProcCommand, "PANNumber", DbType.String, staffDetails.PANNumber);
                this.dbServer.AddInParameter(storedProcCommand, "Education", DbType.String, staffDetails.Education);
                this.dbServer.AddInParameter(storedProcCommand, "Experience", DbType.String, staffDetails.Experience);
                this.dbServer.AddInParameter(storedProcCommand, "Department", DbType.String, staffDetails.DepartmentName);
                this.dbServer.AddInParameter(storedProcCommand, "ClinicId", DbType.Int64, staffDetails.ClinicId);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, staffDetails.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "IsApplicableAdvise", DbType.Boolean, staffDetails.IsDischargeApprove);
                this.dbServer.AddInParameter(storedProcCommand, "IsMarketingExecutives", DbType.Boolean, staffDetails.IsMarketingExecutives);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, staffDetails.ID);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.StaffDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject AddStaffMaster(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddStaffMasterBizActionVO bizActionObj = valueObject as clsAddStaffMasterBizActionVO;
            return ((bizActionObj.StaffDetails.ID != 0L) ? this.UpdateStaffMaster(bizActionObj, objUserVO) : this.AddStaffMaster(bizActionObj, objUserVO));
        }

        public override IValueObject GetStaffAddressInfo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetStaffAddressInfoBizActionVO nvo = (clsGetStaffAddressInfoBizActionVO) valueObject;
            try
            {
                clsStaffAddressInfoVO objStaffAddressDetail = nvo.objStaffAddressDetail;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStaffAddressInfo");
                this.dbServer.AddInParameter(storedProcCommand, "StaffId", DbType.Int64, nvo.StaffId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.StaffAddressDetailList == null)
                    {
                        nvo.StaffAddressDetailList = new List<clsStaffAddressInfoVO>();
                    }
                    while (reader.Read())
                    {
                        clsStaffAddressInfoVO item = new clsStaffAddressInfoVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            StaffId = (long) DALHelper.HandleDBNull(reader["StaffId"]),
                            AddressType = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            AddressTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AddressType"])),
                            Name = Convert.ToString(DALHelper.HandleDBNull(reader["Name"])),
                            Address = Convert.ToString(DALHelper.HandleDBNull(reader["Address"])),
                            Contact1 = Convert.ToString(DALHelper.HandleDBNull(reader["Contact1"])),
                            Contact2 = Convert.ToString(DALHelper.HandleDBNull(reader["Contact2"]))
                        };
                        nvo.StaffAddressDetailList.Add(item);
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

        public override IValueObject GetStaffAddressInfoById(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetStaffAddressInfoByIdVO dvo = (clsGetStaffAddressInfoByIdVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStaffAddressInfoById");
                this.dbServer.AddInParameter(storedProcCommand, "StaffId", DbType.Int64, dvo.StaffId);
                this.dbServer.AddInParameter(storedProcCommand, "AddressTypeID", DbType.Int64, dvo.AddressTypeId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dvo.objStaffAddressDetail = new clsStaffAddressInfoVO();
                        dvo.objStaffAddressDetail.StaffId = (long) DALHelper.HandleDBNull(reader["StaffId"]);
                        dvo.objStaffAddressDetail.AddressTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AddressType"]));
                        dvo.objStaffAddressDetail.Name = Convert.ToString(DALHelper.HandleDBNull(reader["Name"]));
                        dvo.objStaffAddressDetail.Address = Convert.ToString(DALHelper.HandleDBNull(reader["Address"]));
                        dvo.objStaffAddressDetail.Contact1 = Convert.ToString(DALHelper.HandleDBNull(reader["Contact1"]));
                        dvo.objStaffAddressDetail.Contact2 = Convert.ToString(DALHelper.HandleDBNull(reader["Contact2"]));
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return dvo;
        }

        public override IValueObject GetStaffBankInfo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetStaffBankInfoBizActionVO nvo = (clsGetStaffBankInfoBizActionVO) valueObject;
            try
            {
                clsStaffBankInfoVO objStaffBankDetail = nvo.objStaffBankDetail;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStaffBankInfo");
                this.dbServer.AddInParameter(storedProcCommand, "StaffId", DbType.Int64, nvo.StaffID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.StaffBankDetailList == null)
                    {
                        nvo.StaffBankDetailList = new List<clsStaffBankInfoVO>();
                    }
                    while (reader.Read())
                    {
                        clsStaffBankInfoVO item = new clsStaffBankInfoVO {
                            StaffId = (long) DALHelper.HandleDBNull(reader["StaffId"]),
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            BankName = Convert.ToString(DALHelper.HandleDBNull(reader["BankName"])),
                            BankId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BankId"])),
                            BranchName = Convert.ToString(DALHelper.HandleDBNull(reader["BankBranchName"])),
                            BranchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BankBrachId"])),
                            AccountNumber = Convert.ToString(DALHelper.HandleDBNull(reader["AccountNumber"])),
                            BranchAddress = Convert.ToString(DALHelper.HandleDBNull(reader["Address"])),
                            AccountTypeName = Convert.ToString(DALHelper.HandleDBNull(reader["AccountType"])),
                            StaffName = Convert.ToString(DALHelper.HandleDBNull(reader["StaffName"])),
                            MICRNumber = Convert.ToString(DALHelper.HandleDBNull(reader["MICRNumber"]))
                        };
                        nvo.StaffBankDetailList.Add(item);
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

        public override IValueObject GetStaffBankInfoById(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetStaffBankInfoByIdVO dvo = (clsGetStaffBankInfoByIdVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStaffBankInfoById");
                this.dbServer.AddInParameter(storedProcCommand, "StaffId", DbType.Int64, dvo.StaffID);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, dvo.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dvo.objStaffBankDetail = new clsStaffBankInfoVO();
                        dvo.objStaffBankDetail.StaffId = (long) DALHelper.HandleDBNull(reader["StaffId"]);
                        dvo.objStaffBankDetail.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        dvo.objStaffBankDetail.BankName = Convert.ToString(DALHelper.HandleDBNull(reader["BankName"]));
                        dvo.objStaffBankDetail.BankId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BankId"]));
                        dvo.objStaffBankDetail.BranchName = Convert.ToString(DALHelper.HandleDBNull(reader["BankBranchName"]));
                        dvo.objStaffBankDetail.BranchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BankBrachId"]));
                        dvo.objStaffBankDetail.AccountNumber = Convert.ToString(DALHelper.HandleDBNull(reader["AccountNumber"]));
                        dvo.objStaffBankDetail.BranchAddress = Convert.ToString(DALHelper.HandleDBNull(reader["Address"]));
                        dvo.objStaffBankDetail.AccountTypeName = Convert.ToString(DALHelper.HandleDBNull(reader["AccountType"]));
                        dvo.objStaffBankDetail.StaffName = Convert.ToString(DALHelper.HandleDBNull(reader["StaffName"]));
                        dvo.objStaffBankDetail.MICRNumber = Convert.ToString(DALHelper.HandleDBNull(reader["MICRNumber"]));
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return dvo;
        }

        public override IValueObject GetStaffByUnitID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetStaffMasterByUnitIDBizActionVO nvo = (clsGetStaffMasterByUnitIDBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStaffByUnitID");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID ", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "DesignationID ", DbType.Int64, nvo.DesignationID);
                this.dbServer.AddInParameter(storedProcCommand, "FromNurseSchedule ", DbType.Int16, nvo.FromNurseSchedule);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsStaffMasterVO item = new clsStaffMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            StaffNo = reader["StaffNo"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            StaffName = reader["SataffName"].ToString(),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"])),
                            UnitName = reader["Name"].ToString()
                        };
                        nvo.StaffMasterList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject GetStaffByUnitIDandID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetStaffMasterByUnitIDBizActionVO nvo = (clsGetStaffMasterByUnitIDBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStaffByUnitIDandID");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID ", DbType.Int64, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    reader.Read();
                    clsStaffMasterVO item = new clsStaffMasterVO {
                        StaffName = (string) DALHelper.HandleDBNull(reader["StaffName"]),
                        Designation = (string) DALHelper.HandleDBNull(reader["Designation"]),
                        DOB = new DateTime?(DALHelper.HandleDate(reader["DOB"]).Value),
                        UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                        ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                        FirstName = (string) DALHelper.HandleDBNull(reader["FirstName"]),
                        MiddleName = (string) DALHelper.HandleDBNull(reader["MiddleName"]),
                        LastName = (string) DALHelper.HandleDBNull(reader["LastName"]),
                        BloodGroupId = (long) DALHelper.HandleDBNull(reader["BloodGroupId"]),
                        AgeDays = (long) DALHelper.HandleDBNull(reader["AgeDays"]),
                        AgeMonth = (long) DALHelper.HandleDBNull(reader["AgeMonth"]),
                        AgeYears = (long) DALHelper.HandleDBNull(reader["AgeYears"]),
                        GenderID = (long) DALHelper.HandleDBNull(reader["GenderID"]),
                        MaritalStatusId = (long) DALHelper.HandleDBNull(reader["MaritalStatusId"]),
                        ReligionId = (long) DALHelper.HandleDBNull(reader["ReligionId"]),
                        EmailId = (string) DALHelper.HandleDBNull(reader["EmailId"]),
                        MobileNo = (long) DALHelper.HandleDBNull(reader["MobileNo"]),
                        MobileCountryCode = (long) DALHelper.HandleDBNull(reader["MobileCountryCode"]),
                        ResiNoCountryCode = (long) DALHelper.HandleDBNull(reader["ResiNoCountryCode"]),
                        ResiSTDCode = (long) DALHelper.HandleDBNull(reader["ResiSTDCode"]),
                        ResidenceNo = (long) DALHelper.HandleDBNull(reader["ResidenceNo"]),
                        PrefixID = (long) DALHelper.HandleDBNull(reader["PrefixID"])
                    };
                    nvo.StaffMasterList.Add(item);
                    reader.NextResult();
                    while (reader.Read())
                    {
                        clsStaffAddressDetailsVO svo = new clsStaffAddressDetailsVO {
                            Address = (string) DALHelper.HandleDBNull(reader["Address"]),
                            AddressTypeID = (long) DALHelper.HandleDBNull(reader["AddressType"])
                        };
                        nvo.StaffAddressList.Add(svo);
                    }
                }
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject GetStaffMasterByID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetStaffMasterDetailsByIDBizActionVO nvo = valueObject as clsGetStaffMasterDetailsByIDBizActionVO;
            try
            {
                clsStaffMasterVO staffMasterList = nvo.StaffMasterList;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStaffMasterDetailsByStaffID");
                this.dbServer.AddInParameter(storedProcCommand, "StaffID ", DbType.Int64, nvo.StaffId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (nvo.StaffMasterList == null)
                        {
                            nvo.StaffMasterList = new clsStaffMasterVO();
                        }
                        nvo.StaffMasterList.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.StaffMasterList.FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]));
                        nvo.StaffMasterList.MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"]));
                        nvo.StaffMasterList.LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]));
                        nvo.StaffMasterList.DOB = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["DOB"])));
                        nvo.StaffMasterList.GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]));
                        nvo.StaffMasterList.DesignationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Designation"]));
                        nvo.StaffMasterList.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.StaffMasterList.EmailId = Convert.ToString(DALHelper.HandleDBNull(reader["EmailId"]));
                        nvo.StaffMasterList.PFNumber = Convert.ToString(DALHelper.HandleDBNull(reader["PFNumber"]));
                        nvo.StaffMasterList.MaritalStatusId = Convert.ToInt64(DALHelper.HandleDBNull(reader["MaritalStatusId"]));
                        nvo.StaffMasterList.Photo = (byte[]) DALHelper.HandleDBNull(reader["Photo"]);
                        nvo.StaffMasterList.DateofJoining = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["DateofJoining"])));
                        nvo.StaffMasterList.AccessCardNumber = Convert.ToString(DALHelper.HandleDBNull(reader["AccessCardNumber"]));
                        nvo.StaffMasterList.EmployeeNumber = Convert.ToString(DALHelper.HandleDBNull(reader["EmployeeNumber"]));
                        nvo.StaffMasterList.PANNumber = Convert.ToString(DALHelper.HandleDBNull(reader["PANNumber"]));
                        nvo.StaffMasterList.Education = Convert.ToString(DALHelper.HandleDBNull(reader["Education"]));
                        nvo.StaffMasterList.Experience = Convert.ToString(DALHelper.HandleDBNull(reader["Experience"]));
                        nvo.StaffMasterList.DepartmentID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"])));
                        nvo.StaffMasterList.ClinicId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClinicId"]));
                        nvo.StaffMasterList.IsDischargeApprove = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsApplicableAdvise"]));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetStaffMasterList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetStaffMasterDetailsBizActionVO nvo = (clsGetStaffMasterDetailsBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStaffMasterDetailsList");
                if ((nvo.FirstName != null) && (nvo.FirstName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, nvo.FirstName + "%");
                }
                if ((nvo.LastName != null) && (nvo.LastName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, nvo.LastName + "%");
                }
                if ((nvo.StrClinicID != null) && (nvo.StrClinicID.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "StrClinicID", DbType.String, nvo.StrClinicID);
                }
                this.dbServer.AddInParameter(storedProcCommand, "DesignationID ", DbType.Int64, nvo.DesignationID);
                this.dbServer.AddInParameter(storedProcCommand, "ClinicId ", DbType.Int64, nvo.ClinicId);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.StaffMasterList == null)
                    {
                        nvo.StaffMasterList = new List<clsStaffMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsStaffMasterVO item = new clsStaffMasterVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            FirstName = (string) DALHelper.HandleDBNull(reader["FirstName"]),
                            MiddleName = (string) DALHelper.HandleDBNull(reader["MiddleName"]),
                            LastName = (string) DALHelper.HandleDBNull(reader["LastName"]),
                            DOB = DALHelper.HandleDate(reader["DOB"]),
                            GenderID = (long) DALHelper.HandleDBNull(reader["GenderID"]),
                            DesignationID = (long) DALHelper.HandleDBNull(reader["Designation"]),
                            DepartmentID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"]))),
                            IsApplicableAdvise = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsApplicableAdvise"])),
                            Designation = (string) DALHelper.HandleDBNull(reader["Description"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            Value = (string) DALHelper.HandleDBNull(reader["Value"]),
                            EmailId = (string) DALHelper.HandleDBNull(reader["EmailId"]),
                            ClinicName = (string) DALHelper.HandleDBNull(reader["ClinicName"])
                        };
                        nvo.StaffMasterList.Add(item);
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
            return nvo;
        }

        public override IValueObject GetUserSearchList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetUserSearchBizActionVO nvo = (clsGetUserSearchBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetUnitWiseUserSearch");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID ", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, nvo.FirstName);
                this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, nvo.LastName);
                this.dbServer.AddInParameter(storedProcCommand, "DesignationID", DbType.Int64, nvo.DesignationID);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, nvo.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsStaffMasterVO item = new clsStaffMasterVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UserName = (string) DALHelper.HandleDBNull(reader["UserName"]),
                            FirstName = (string) DALHelper.HandleDBNull(reader["FirstName"]),
                            LastName = (string) DALHelper.HandleDBNull(reader["LastName"]),
                            UserID = (long) DALHelper.HandleDBNull(reader["UserID"]),
                            UserUnitID = (long) DALHelper.HandleDBNull(reader["UserUnitID"]),
                            Designation = (string) DALHelper.HandleDBNull(reader["Designation"])
                        };
                        DateTime? nullable = DALHelper.HandleDate(reader["DOB"]);
                        item.DOB = new DateTime?(nullable.Value);
                        item.UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]);
                        item.RowID = (long) DALHelper.HandleDBNull(reader["RowNum"]);
                        nvo.StaffMasterList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject UpdateStaffAddressInfo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateStaffAddressInfoVO ovo = (clsUpdateStaffAddressInfoVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateStaffAddressDetail");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, ovo.objStaffAddressDetail.ID);
                this.dbServer.AddInParameter(storedProcCommand, "StaffId", DbType.Int64, ovo.objStaffAddressDetail.StaffId);
                this.dbServer.AddInParameter(storedProcCommand, "AddressTypeID", DbType.Int64, ovo.objStaffAddressDetail.AddressTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "Name", DbType.String, ovo.objStaffAddressDetail.Name);
                this.dbServer.AddInParameter(storedProcCommand, "Address", DbType.String, ovo.objStaffAddressDetail.Address);
                this.dbServer.AddInParameter(storedProcCommand, "Contact1", DbType.String, ovo.objStaffAddressDetail.Contact1);
                this.dbServer.AddInParameter(storedProcCommand, "Contact2", DbType.String, ovo.objStaffAddressDetail.Contact2);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, ovo.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                ovo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return ovo;
        }

        public override IValueObject UpdateStaffBankInfo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateStaffBankInfoVO ovo = (clsUpdateStaffBankInfoVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateStaffBankDetail");
                this.dbServer.AddInParameter(storedProcCommand, "StaffId", DbType.Int64, ovo.objStaffBankDetail.StaffId);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, ovo.objStaffBankDetail.ID);
                this.dbServer.AddInParameter(storedProcCommand, "BankId", DbType.Int64, ovo.objStaffBankDetail.BankId);
                this.dbServer.AddInParameter(storedProcCommand, "BranchId", DbType.Int64, ovo.objStaffBankDetail.BranchId);
                this.dbServer.AddInParameter(storedProcCommand, "AccountNumber", DbType.String, ovo.objStaffBankDetail.AccountNumber);
                this.dbServer.AddInParameter(storedProcCommand, "AccountType", DbType.Boolean, ovo.objStaffBankDetail.AccountType);
                this.dbServer.AddInParameter(storedProcCommand, "BranchAddress", DbType.String, ovo.objStaffBankDetail.BranchAddress);
                this.dbServer.AddInParameter(storedProcCommand, "MICRNumber", DbType.String, ovo.objStaffBankDetail.MICRNumber);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                ovo.objStaffBankDetail.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return ovo;
        }

        private clsAddStaffMasterBizActionVO UpdateStaffMaster(clsAddStaffMasterBizActionVO BizActionObj, clsUserVO objUserVO)
        {
            try
            {
                clsStaffMasterVO staffDetails = BizActionObj.StaffDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateStaffMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.String, staffDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, staffDetails.FirstName.Trim());
                if (staffDetails.MiddleName != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, staffDetails.MiddleName.Trim());
                }
                this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, staffDetails.LastName.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "DOB", DbType.DateTime, staffDetails.DOB);
                this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, staffDetails.GenderID);
                this.dbServer.AddInParameter(storedProcCommand, "DesignationID", DbType.Int64, staffDetails.DesignationID);
                this.dbServer.AddInParameter(storedProcCommand, "EmailId", DbType.String, staffDetails.EmailId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, staffDetails.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "EmployeeNumber", DbType.String, staffDetails.EmployeeNumber);
                this.dbServer.AddInParameter(storedProcCommand, "PFNumber", DbType.String, staffDetails.PFNumber);
                this.dbServer.AddInParameter(storedProcCommand, "MaritalStatusId", DbType.Int32, staffDetails.MaritalStatusId);
                this.dbServer.AddInParameter(storedProcCommand, "Photo", DbType.Binary, staffDetails.Photo);
                this.dbServer.AddInParameter(storedProcCommand, "DateOfJoining", DbType.DateTime, staffDetails.DateofJoining);
                this.dbServer.AddInParameter(storedProcCommand, "AccessCardNumber", DbType.String, staffDetails.AccessCardNumber);
                this.dbServer.AddInParameter(storedProcCommand, "PANNumber", DbType.String, staffDetails.PANNumber);
                this.dbServer.AddInParameter(storedProcCommand, "Education", DbType.String, staffDetails.Education);
                this.dbServer.AddInParameter(storedProcCommand, "Experience", DbType.String, staffDetails.Experience);
                this.dbServer.AddInParameter(storedProcCommand, "Department", DbType.String, staffDetails.DepartmentName);
                this.dbServer.AddInParameter(storedProcCommand, "ClinicId", DbType.Int64, staffDetails.ClinicId);
                this.dbServer.AddInParameter(storedProcCommand, "IsApplicableAdvise", DbType.Boolean, staffDetails.IsDischargeApprove);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, staffDetails.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }
    }
}

