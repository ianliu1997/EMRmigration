namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.CustomExceptions;
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration;
    using PalashDynamics.ValueObjects.Administration.UnitMaster;
    using PalashDynamics.ValueObjects.EMR.NewEMR;
    using PalashDynamics.ValueObjects.Inventory;
    using PalashDynamics.ValueObjects.Master;
    using PalashDynamics.ValueObjects.User;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    internal class clsUserManagementDAL : clsBaseUserManagementDAL
    {
        private Database dbServer;
        public int AuditTrailId;

        private clsUserManagementDAL()
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

        private void AddCommonParametersForAddUpdateMethods(DbCommand command, clsUserVO objVO, clsUserVO objUserVO)
        {
            try
            {
                this.dbServer.AddInParameter(command, "Password", DbType.String, Security.base64Encode(objVO.Password));
                this.dbServer.AddInParameter(command, "LoginName", DbType.String, Security.base64Encode(objVO.LoginName));
                this.dbServer.AddInParameter(command, "IsEmployee", DbType.Boolean, objVO.UserGeneralDetailVO.IsEmployee);
                this.dbServer.AddInParameter(command, "IsDoctor", DbType.Boolean, objVO.UserGeneralDetailVO.IsDoctor);
                this.dbServer.AddInParameter(command, "IsPatient", DbType.Boolean, objVO.UserGeneralDetailVO.IsPatient);
                this.dbServer.AddInParameter(command, "EmployeeID", DbType.Int64, objVO.UserGeneralDetailVO.EmployeeID);
                this.dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objVO.UserGeneralDetailVO.DoctorID);
                this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, objVO.UserGeneralDetailVO.PatientID);
                this.dbServer.AddInParameter(command, "Locked", DbType.Boolean, objVO.UserGeneralDetailVO.Locked);
                this.dbServer.AddInParameter(command, "EnablePasswordExpiration", DbType.Boolean, objVO.UserGeneralDetailVO.EnablePasswordExpiration);
                this.dbServer.AddInParameter(command, "PasswordExpirationInterval", DbType.Int32, objVO.UserGeneralDetailVO.PasswordExpirationInterval);
                this.dbServer.AddInParameter(command, "UserType", DbType.Int32, objVO.UserType);
                this.dbServer.AddInParameter(command, "Status", DbType.Boolean, objVO.UserGeneralDetailVO.Status);
                this.dbServer.AddInParameter(command, "RoleId", DbType.Int64, objVO.UserGeneralDetailVO.RoleDetails.ID);
                this.dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0x7fffffff);
                this.dbServer.AddInParameter(command, "MinPasswordLength", DbType.Int16, objVO.PassConfig.MinPasswordLength);
                this.dbServer.AddInParameter(command, "MaxPasswordLength", DbType.Int16, objVO.PassConfig.MaxPasswordLength);
                this.dbServer.AddInParameter(command, "AtLeastOneDigit", DbType.Boolean, objVO.PassConfig.AtLeastOneDigit);
                this.dbServer.AddInParameter(command, "AtLeastOneLowerCaseChar", DbType.Boolean, objVO.PassConfig.AtLeastOneLowerCaseChar);
                this.dbServer.AddInParameter(command, "AtLeastOneUpperCaseChar", DbType.Boolean, objVO.PassConfig.AtLeastOneUpperCaseChar);
                this.dbServer.AddInParameter(command, "AtLeastOneSpecialChar", DbType.Boolean, objVO.PassConfig.AtLeastOneSpecialChar);
                this.dbServer.AddInParameter(command, "NoOfPasswordsToRemember", DbType.Int16, objVO.PassConfig.NoOfPasswordsToRemember);
                this.dbServer.AddInParameter(command, "MinPasswordAge", DbType.Int16, objVO.PassConfig.MinPasswordAge);
                this.dbServer.AddInParameter(command, "MaxPasswordAge", DbType.Int16, objVO.PassConfig.MaxPasswordAge);
                this.dbServer.AddInParameter(command, "AccountLockThreshold", DbType.Int16, objVO.PassConfig.AccountLockThreshold);
                this.dbServer.AddInParameter(command, "AccountLockDuration", DbType.Double, objVO.PassConfig.AccountLockDuration);
                StringBuilder builder = new StringBuilder();
                StringBuilder builder2 = new StringBuilder();
                StringBuilder builder3 = new StringBuilder();
                StringBuilder builder4 = new StringBuilder();
                clsUserRoleVO roleDetails = objVO.UserGeneralDetailVO.RoleDetails;
                int num = 0;
                while (true)
                {
                    if (num >= roleDetails.MenuList.Count)
                    {
                        int num2 = 0;
                        while (true)
                        {
                            if (num2 >= roleDetails.DashBoardList.Count)
                            {
                                this.dbServer.AddInParameter(command, "MenuIdList", DbType.String, builder.ToString());
                                this.dbServer.AddInParameter(command, "MenuStatusList", DbType.String, builder2.ToString());
                                this.dbServer.AddInParameter(command, "DashboardIdList", DbType.String, builder3.ToString());
                                this.dbServer.AddInParameter(command, "DashboardStatusList", DbType.String, builder4.ToString());
                                StringBuilder builder5 = new StringBuilder();
                                StringBuilder builder6 = new StringBuilder();
                                StringBuilder builder7 = new StringBuilder();
                                int num3 = 0;
                                while (true)
                                {
                                    if (num3 >= objVO.UserGeneralDetailVO.UnitDetails.Count)
                                    {
                                        this.dbServer.AddInParameter(command, "UnitIdList", DbType.String, builder5.ToString());
                                        this.dbServer.AddInParameter(command, "UnitStatusList", DbType.String, builder6.ToString());
                                        this.dbServer.AddInParameter(command, "UnitIsDefaultList", DbType.String, builder7.ToString());
                                        StringBuilder builder8 = new StringBuilder();
                                        StringBuilder builder9 = new StringBuilder();
                                        StringBuilder builder10 = new StringBuilder();
                                        bool flag = false;
                                        int num4 = 0;
                                        while (true)
                                        {
                                            if (num4 >= objVO.UserGeneralDetailVO.UnitDetails.Count)
                                            {
                                                this.dbServer.AddInParameter(command, "UnitStoreIdList", DbType.String, builder8.ToString());
                                                this.dbServer.AddInParameter(command, "StoreIdList", DbType.String, builder9.ToString());
                                                this.dbServer.AddInParameter(command, "StatusList", DbType.String, builder10.ToString());
                                                break;
                                            }
                                            int num5 = 0;
                                            while (true)
                                            {
                                                if (num5 >= objVO.UserGeneralDetailVO.UnitDetails[num4].StoreDetails.Count)
                                                {
                                                    num4++;
                                                    break;
                                                }
                                                if (flag)
                                                {
                                                    builder8.Append(",");
                                                    builder9.Append(",");
                                                    builder10.Append(",");
                                                }
                                                flag = true;
                                                builder8.Append(objVO.UserGeneralDetailVO.UnitDetails[num4].StoreDetails[num5].UnitId);
                                                builder9.Append(objVO.UserGeneralDetailVO.UnitDetails[num4].StoreDetails[num5].ID);
                                                builder10.Append(objVO.UserGeneralDetailVO.UnitDetails[num4].StoreDetails[num5].StoreStatus);
                                                num5++;
                                            }
                                        }
                                        break;
                                    }
                                    builder5.Append(objVO.UserGeneralDetailVO.UnitDetails[num3].UnitID);
                                    builder6.Append(objVO.UserGeneralDetailVO.UnitDetails[num3].Status);
                                    builder7.Append(objVO.UserGeneralDetailVO.UnitDetails[num3].IsDefault);
                                    if (num3 < (objVO.UserGeneralDetailVO.UnitDetails.Count - 1))
                                    {
                                        builder5.Append(",");
                                        builder6.Append(",");
                                        builder7.Append(",");
                                    }
                                    num3++;
                                }
                                break;
                            }
                            clsDashBoardVO dvo = roleDetails.DashBoardList[num2];
                            builder3.Append(dvo.ID);
                            builder4.Append(dvo.Status);
                            if (num2 < (roleDetails.DashBoardList.Count - 1))
                            {
                                builder3.Append(",");
                                builder4.Append(",");
                            }
                            num2++;
                        }
                        break;
                    }
                    clsMenuVO uvo = roleDetails.MenuList[num];
                    builder.Append(uvo.ID);
                    builder2.Append(uvo.Status);
                    if (num < (roleDetails.MenuList.Count - 1))
                    {
                        builder.Append(",");
                        builder2.Append(",");
                    }
                    num++;
                }
            }
            catch (Exception)
            {
            }
        }

        public override IValueObject AddLicenseTo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddLicenseToBizActionVO nvo = valueObject as clsAddLicenseToBizActionVO;
            try
            {
                if (nvo.K1 == "True")
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateLicenseTo");
                    this.dbServer.AddInParameter(storedProcCommand, "K1", DbType.String, newSecurity.EncryptDecryptUserKey(nvo.K1, false));
                    this.dbServer.AddInParameter(storedProcCommand, "K2", DbType.String, newSecurity.EncryptDecryptUserKey(nvo.K2, false));
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                }
                else
                {
                    clsUnitMasterVO unitDetails = nvo.UnitDetails;
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddLicenseTo");
                    this.dbServer.AddInParameter(storedProcCommand, "Name", DbType.String, newSecurity.EncryptDecryptUserKey(unitDetails.Description, false));
                    this.dbServer.AddInParameter(storedProcCommand, "AddressLine1", DbType.String, unitDetails.AddressLine1);
                    this.dbServer.AddInParameter(storedProcCommand, "AddressLine2", DbType.String, unitDetails.AddressLine2);
                    this.dbServer.AddInParameter(storedProcCommand, "AddressLine3", DbType.String, unitDetails.AddressLine3);
                    this.dbServer.AddInParameter(storedProcCommand, "Country", DbType.String, unitDetails.Country);
                    this.dbServer.AddInParameter(storedProcCommand, "State", DbType.String, unitDetails.State);
                    this.dbServer.AddInParameter(storedProcCommand, "City", DbType.String, unitDetails.City);
                    this.dbServer.AddInParameter(storedProcCommand, "Taluka", DbType.String, unitDetails.Taluka);
                    this.dbServer.AddInParameter(storedProcCommand, "Area", DbType.String, newSecurity.EncryptDecryptUserKey(unitDetails.Area, false));
                    this.dbServer.AddInParameter(storedProcCommand, "District", DbType.String, unitDetails.District);
                    this.dbServer.AddInParameter(storedProcCommand, "Pincode", DbType.String, newSecurity.EncryptDecryptUserKey(unitDetails.Pincode, false));
                    this.dbServer.AddInParameter(storedProcCommand, "ContactNo", DbType.String, unitDetails.ContactNo);
                    this.dbServer.AddInParameter(storedProcCommand, "ResiNoCountryCode", DbType.Int32, unitDetails.ResiNoCountryCode);
                    this.dbServer.AddInParameter(storedProcCommand, "ResiSTDCode", DbType.Int32, unitDetails.ResiSTDCode);
                    this.dbServer.AddInParameter(storedProcCommand, "FaxNo", DbType.String, unitDetails.FaxNo);
                    this.dbServer.AddInParameter(storedProcCommand, "Email", DbType.String, unitDetails.Email);
                    this.dbServer.AddInParameter(storedProcCommand, "CurrentD", DbType.String, newSecurity.EncryptDecryptUserKey(nvo.sDate, false));
                    this.dbServer.AddInParameter(storedProcCommand, "CurrentT", DbType.String, newSecurity.EncryptDecryptUserKey(nvo.sTime, false));
                    this.dbServer.AddInParameter(storedProcCommand, "K1", DbType.String, newSecurity.EncryptDecryptUserKey(nvo.K1, false));
                    this.dbServer.AddInParameter(storedProcCommand, "K2", DbType.String, newSecurity.EncryptDecryptUserKey(nvo.K2, false));
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, unitDetails.UnitID);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                }
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject AddtoDatabase(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsdbDetailsBizActionVO nvo = valueObject as clsdbDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddToDB");
                this.dbServer.AddInParameter(storedProcCommand, "Value", DbType.String, Security.base64Encode(nvo.Value));
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject AddtoStaticDB(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddLicenseToBizActionVO nvo = valueObject as clsAddLicenseToBizActionVO;
            try
            {
                clsUnitMasterVO unitDetails = nvo.UnitDetails;
                if (unitDetails.UnitID == 0L)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddClientInfo");
                    this.dbServer.AddInParameter(storedProcCommand, "ClientName", DbType.String, newSecurity.EncryptDecryptUserKey(unitDetails.Description, false));
                    this.dbServer.AddInParameter(storedProcCommand, "AddressLine1", DbType.String, unitDetails.AddressLine1);
                    this.dbServer.AddInParameter(storedProcCommand, "AddressLine2", DbType.String, unitDetails.AddressLine2);
                    this.dbServer.AddInParameter(storedProcCommand, "AddressLine3", DbType.String, unitDetails.AddressLine3);
                    this.dbServer.AddInParameter(storedProcCommand, "Country", DbType.String, unitDetails.Country);
                    this.dbServer.AddInParameter(storedProcCommand, "State", DbType.String, unitDetails.State);
                    this.dbServer.AddInParameter(storedProcCommand, "City", DbType.String, unitDetails.City);
                    this.dbServer.AddInParameter(storedProcCommand, "Taluka", DbType.String, unitDetails.Taluka);
                    this.dbServer.AddInParameter(storedProcCommand, "Area", DbType.String, newSecurity.EncryptDecryptUserKey(unitDetails.Area, false));
                    this.dbServer.AddInParameter(storedProcCommand, "District", DbType.String, unitDetails.District);
                    this.dbServer.AddInParameter(storedProcCommand, "Pincode", DbType.String, newSecurity.EncryptDecryptUserKey(unitDetails.Pincode, false));
                    this.dbServer.AddInParameter(storedProcCommand, "ResiNoCountryCode", DbType.Int32, unitDetails.ResiNoCountryCode);
                    this.dbServer.AddInParameter(storedProcCommand, "ResiSTDCode", DbType.Int32, unitDetails.ResiSTDCode);
                    this.dbServer.AddInParameter(storedProcCommand, "ContactNo", DbType.String, unitDetails.ContactNo);
                    this.dbServer.AddInParameter(storedProcCommand, "FaxNo", DbType.String, unitDetails.FaxNo);
                    this.dbServer.AddInParameter(storedProcCommand, "Email", DbType.String, unitDetails.Email);
                    this.dbServer.AddInParameter(storedProcCommand, "RegistrationDateTime", DbType.DateTime, nvo.RegDateTime);
                    this.dbServer.AddInParameter(storedProcCommand, "ClientKey", DbType.String, newSecurity.EncryptDecryptAccessKey(nvo.Key, false));
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, unitDetails.UnitID);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                }
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject AddUser(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUserBizActionVO nvo = valueObject as clsAddUserBizActionVO;
            try
            {
                clsUserVO details = nvo.Details;
                DbCommand storedProcCommand = null;
                if (details.ID != 0L)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateUser");
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, details.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, objUserVO.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUser");
                    this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0x7fffffff);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    if (details.UserGeneralDetailVO.IsPatient)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, details.UserLoginInfo.UnitId);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                }
                this.AddCommonParametersForAddUpdateMethods(storedProcCommand, details, objUserVO);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject AddUserCategoryLink(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUserCategoryLinkBizActionVO nvo = (clsAddUserCategoryLinkBizActionVO) valueObject;
            clsUserCategoryLinkVO userCategoryLinkDetails = nvo.UserCategoryLinkDetails;
            DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteUserCategoryLink");
            try
            {
                this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, userCategoryLinkDetails.UserID);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (storedProcCommand != null)
                {
                    storedProcCommand.Dispose();
                    storedProcCommand.Connection.Dispose();
                }
            }
            if ((nvo.UserCategoryLinkList != null) && (nvo.UserCategoryLinkList.Count > 0))
            {
                foreach (clsUserCategoryLinkVO kvo2 in nvo.UserCategoryLinkList)
                {
                    DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddUserCategoryLinking");
                    try
                    {
                        this.dbServer.AddInParameter(command, "UserID", DbType.Int64, kvo2.UserID);
                        this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, kvo2.UnitId);
                        this.dbServer.AddInParameter(command, "CategoryID", DbType.Int64, kvo2.ID);
                        this.dbServer.AddInParameter(command, "CategoryTypeID", DbType.Int64, kvo2.CategoryTypeID);
                        this.dbServer.AddInParameter(command, "CategoryType", DbType.String, kvo2.CategoryType);
                        this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                        this.dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                        this.dbServer.ExecuteNonQuery(command);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        if (command != null)
                        {
                            command.Dispose();
                            command.Connection.Dispose();
                        }
                    }
                }
            }
            return nvo;
        }

        public override IValueObject AssignUserEMRTemplate(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAssignUserEMRTemplatesBizActionVO nvo = valueObject as clsAssignUserEMRTemplatesBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUserEMRTempalteDetails");
                this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, nvo.UserID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                StringBuilder builder = new StringBuilder();
                StringBuilder builder2 = new StringBuilder();
                int count = nvo.Details.Count;
                int num2 = 0;
                while (true)
                {
                    if (num2 >= count)
                    {
                        builder = builder.Remove(builder.Length - 1, 1);
                        builder2 = builder2.Remove(builder2.Length - 1, 1);
                        this.dbServer.AddInParameter(storedProcCommand, "EMRTemplateIDList", DbType.String, builder.ToString());
                        this.dbServer.AddInParameter(storedProcCommand, "StatusList", DbType.String, builder2.ToString());
                        this.dbServer.ExecuteNonQuery(storedProcCommand);
                        break;
                    }
                    clsUserEMRTemplateDetailsVO svo = nvo.Details[num2];
                    if (svo.Status)
                    {
                        builder.Append(svo.TemplateID);
                        builder2.Append(svo.Status);
                        builder.Append(",");
                        builder2.Append(",");
                    }
                    num2++;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject AuthenticateLoginName(string LoginName, string Password, long UnitId)
        {
            clsUserVO rvo = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_LoginNameExists");
                this.dbServer.AddInParameter(storedProcCommand, "LoginName", DbType.String, Security.base64Encode(LoginName));
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UnitId);
                if ((Password != null) && (Password != " "))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Password", DbType.String, Security.base64Encode(Password));
                }
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rvo = new clsUserVO {
                            IsAuditTrail = Convert.ToBoolean(HMSConfigurationManager.GetValueFromApplicationConfig("IsAuditTrail")),
                            ID = (long) reader["Id"],
                            UserGeneralDetailVO = { 
                                DoctorID = (long) reader["DoctorID"],
                                EmployeeID = (long) reader["EmployeeID"],
                                FirstPasswordChanged = (bool) DALHelper.HandleDBNull(reader["FirstPasswordChanged"]),
                                RoleDetails = { ID = (long) DALHelper.HandleDBNull(reader["RoleID"]) },
                                UnitId = (long) DALHelper.HandleDBNull(reader["UnitId"]),
                                Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                                AddedBy = (long?) DALHelper.HandleDBNull(reader["AddedBy"]),
                                AddedOn = (string) DALHelper.HandleDBNull(reader["AddedOn"]),
                                AddedDateTime = DALHelper.HandleDate(reader["AddedDateTime"]),
                                Locked = (bool) DALHelper.HandleDBNull(reader["Locked"]),
                                DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"])),
                                IsEmployee = (bool) DALHelper.HandleDBNull(reader["IsEmployee"]),
                                IsDoctor = (bool) DALHelper.HandleDBNull(reader["IsDoctor"]),
                                IsPatient = (bool) DALHelper.HandleDBNull(reader["IsPatient"])
                            },
                            LoginName = Security.base64Decode((string) reader["LoginName"]),
                            UserNameNew = Convert.ToString(reader["UserName"]),
                            RoleName = (string) DALHelper.HandleDBNull(reader["Description"]),
                            Password = newSecurity.EncryptDecryptUserKey(Convert.ToString(DALHelper.HandleDBNull(reader["Password"])), true),
                            PasswordDe = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["Password"]))),
                            PassConfig = { AccountLockThreshold = (short) DALHelper.HandleDBNull(reader["AccountLockThreshold"]) }
                        };
                        rvo.UserGeneralDetailVO.EmployeeID = (long) DALHelper.HandleDBNull(reader["EmployeeID"]);
                        rvo.UserGeneralDetailVO.DoctorID = (long) DALHelper.HandleDBNull(reader["DoctorID"]);
                        rvo.UserGeneralDetailVO.PatientID = (long) DALHelper.HandleDBNull(reader["PatientID"]);
                        rvo.UserGeneralDetailVO.SecreteQtn = (long) DALHelper.HandleDBNull(reader["SecreteQtn"]);
                        rvo.UserGeneralDetailVO.Locked = (bool) DALHelper.HandleDBNull(reader["Locked"]);
                        rvo.UserGeneralDetailVO.EnablePasswordExpiration = (bool) DALHelper.HandleDBNull(reader["EnablePasswordExpiration"]);
                        rvo.UserGeneralDetailVO.PasswordExpirationInterval = (int) DALHelper.HandleDBNull(reader["PasswordExpirationInterval"]);
                        rvo.UserGeneralDetailVO.FirstPasswordChanged = (bool) DALHelper.HandleDBNull(reader["FirstPasswordChanged"]);
                        rvo.UserType = (int) DALHelper.HandleDBNull(reader["UserType"]);
                        rvo.PassConfig.MinPasswordLength = (short) DALHelper.HandleDBNull(reader["MinPasswordLength"]);
                        rvo.PassConfig.MaxPasswordLength = (short) DALHelper.HandleDBNull(reader["MaxPasswordLength"]);
                        rvo.PassConfig.MinPasswordAge = (short) DALHelper.HandleDBNull(reader["MinPasswordAge"]);
                        rvo.PassConfig.MaxPasswordAge = (short) DALHelper.HandleDBNull(reader["MaxPasswordAge"]);
                        rvo.PassConfig.NoOfPasswordsToRemember = (short) DALHelper.HandleDBNull(reader["NoOfPasswordsToRemember"]);
                        rvo.PassConfig.AtLeastOneDigit = (bool) DALHelper.HandleDBNull(reader["AtLeastOneDigit"]);
                        rvo.PassConfig.AtLeastOneLowerCaseChar = (bool) DALHelper.HandleDBNull(reader["AtleastOneLowerCaseChar"]);
                        rvo.PassConfig.AtLeastOneUpperCaseChar = (bool) DALHelper.HandleDBNull(reader["AtLeastOneUpperCaseChar"]);
                        rvo.PassConfig.AtLeastOneSpecialChar = (bool) DALHelper.HandleDBNull(reader["AtLeastOneSpecialChar"]);
                        rvo.PassConfig.AccountLockThreshold = (short) DALHelper.HandleDBNull(reader["AccountLockThreshold"]);
                        rvo.PassConfig.AccountLockDuration = (float) ((double) DALHelper.HandleDBNull(reader["AccountLockDuration"]));
                        rvo.UserGeneralDetailVO.RoleDetails = new clsUserRoleVO();
                        rvo.UserGeneralDetailVO.RoleDetails.ID = (long) DALHelper.HandleDBNull(reader["RoleID"]);
                        rvo.UserGeneralDetailVO.UnitId = (long) DALHelper.HandleDBNull(reader["UnitId"]);
                        rvo.UserGeneralDetailVO.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                        rvo.UserGeneralDetailVO.AddedBy = (long?) DALHelper.HandleDBNull(reader["AddedBy"]);
                        rvo.UserGeneralDetailVO.AddedOn = (string) DALHelper.HandleDBNull(reader["AddedOn"]);
                        rvo.UserGeneralDetailVO.AddedDateTime = DALHelper.HandleDate(reader["AddedDateTime"]);
                        rvo.UserGeneralDetailVO.UpdatedBy = (long?) DALHelper.HandleDBNull(reader["UpdatedBy"]);
                        rvo.UserGeneralDetailVO.UpdatedOn = (string) DALHelper.HandleDBNull(reader["UpdatedOn"]);
                        rvo.UserGeneralDetailVO.UpdatedDateTime = DALHelper.HandleDate(reader["UpdatedDateTime"]);
                        rvo.IsCSControlEnable = Convert.ToBoolean(HMSConfigurationManager.GetValueFromApplicationConfig("IsCSControlEnable"));
                        rvo.IsEmailMandatory = Convert.ToBoolean(HMSConfigurationManager.GetValueFromApplicationConfig("IsEmailMandatory"));
                        rvo.PackageID = Convert.ToInt64(HMSConfigurationManager.GetValueFromApplicationConfig("PackageID"));
                        rvo.ValidationsFlag = Convert.ToBoolean(HMSConfigurationManager.GetValueFromApplicationConfig("ValidationsFlag"));
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    rvo.UserGeneralDetailVO.UnitDetails = new List<clsUserUnitDetailsVO>();
                    while (reader.Read())
                    {
                        clsUserUnitDetailsVO item = new clsUserUnitDetailsVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            IsDefault = (bool) DALHelper.HandleDBNull(reader["IsDefault"])
                        };
                        rvo.UserGeneralDetailVO.UnitDetails.Add(item);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    clsUserRoleVO roleDetails = rvo.UserGeneralDetailVO.RoleDetails;
                    roleDetails.MenuList = new List<clsMenuVO>();
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            if (reader.HasRows)
                            {
                                roleDetails.DashBoardList = new List<clsDashBoardVO>();
                                while (reader.Read())
                                {
                                    clsDashBoardVO dvo = new clsDashBoardVO {
                                        ID = (long) DALHelper.HandleDBNull(reader["DashBoardID"]),
                                        Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                                    };
                                    roleDetails.DashBoardList.Add(dvo);
                                }
                            }
                            rvo.UserGeneralDetailVO.RoleDetails = roleDetails;
                            break;
                        }
                        clsMenuVO item = new clsMenuVO {
                            ID = (long) DALHelper.HandleDBNull(reader["MenuID"]),
                            Title = (string) DALHelper.HandleDBNull(reader["Title"]),
                            ImagePath = (string) DALHelper.HandleDBNull(reader["ImagePath"]),
                            Parent = (string) DALHelper.HandleDBNull(reader["Parent"]),
                            ParentId = new long?((long) DALHelper.HandleDBNull(reader["ParentID"])),
                            Module = (string) DALHelper.HandleDBNull(reader["Module"]),
                            Action = (string) DALHelper.HandleDBNull(reader["Action"]),
                            Header = (string) DALHelper.HandleDBNull(reader["Header"]),
                            Configuration = (string) DALHelper.HandleDBNull(reader["Configuration"]),
                            Mode = (string) DALHelper.HandleDBNull(reader["Mode"]),
                            SOPFileName = (string) DALHelper.HandleDBNull(reader["SOPFileName"]),
                            MenuOrder = new int?((int) DALHelper.HandleDBNull(reader["MenuOrder"])),
                            Status = new bool?(Convert.ToBoolean(reader["Status"]))
                        };
                        rvo.UserGeneralDetailVO.RoleDetails.MenuList.Add(item);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsUserCategoryLinkVO item = new clsUserCategoryLinkVO {
                            UserCategoryLinkID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UserCategoryLinkID"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            UserID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UserID"])),
                            CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryID"])),
                            CategoryTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryTypeID"])),
                            CategoryType = Convert.ToString(DALHelper.HandleDBNull(reader["CategoryType"])),
                            CategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["CategoryName"]))
                        };
                        rvo.UserCategoryLinkList.Add(item);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rvo.UserAditionalRights.IsEditAfterFinalized = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEditAfterFinalized"]));
                    }
                }
                reader.Close();
                this.AuditTrailId = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if (rvo != null)
                {
                    rvo.UserLoginInfo.RetunUnitId = this.AuditTrailId;
                }
                if ((rvo != null) && (rvo.UserGeneralDetailVO.UnitDetails.Count != 0))
                {
                    List<long> list = new List<long>();
                    foreach (clsUserUnitDetailsVO svo2 in rvo.UserGeneralDetailVO.UnitDetails)
                    {
                        list.Add(svo2.UnitID);
                    }
                    if (!list.Contains(UnitId))
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_UpdateValidEntryOfUser");
                        this.dbServer.AddInParameter(command2, "ID", DbType.Int64, this.AuditTrailId);
                        this.dbServer.AddInParameter(command2, "LoginName", DbType.String, Security.base64Encode(LoginName));
                        this.dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UnitId);
                        if ((Password != null) && (Password != " "))
                        {
                            this.dbServer.AddInParameter(command2, "Password", DbType.String, Security.base64Encode(Password));
                        }
                        this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command2);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return rvo;
        }

        public override clsUserVO AuthenticateUser(string LoginName, string Password)
        {
            clsUserVO rvo = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AuthenticateUser");
                this.dbServer.AddInParameter(storedProcCommand, "LoginName", DbType.String, Security.base64Encode(LoginName));
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            while (reader.Read())
                            {
                                if (rvo.UserUnitList == null)
                                {
                                    rvo.UserUnitList = new List<clsUserUnitDetailsVO>();
                                }
                                clsUserUnitDetailsVO item = new clsUserUnitDetailsVO {
                                    ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                    UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                                    Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                                    IsDefault = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDefault"]))
                                };
                                rvo.UserUnitList.Add(item);
                            }
                            break;
                        }
                        rvo = new clsUserVO {
                            ID = (long) reader["Id"],
                            PassConfig = { 
                                AccountLockThreshold = (short) DALHelper.HandleDBNull(reader["AccountLockThreshold"]),
                                AccountLockDuration = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["AccountLockDuration"]))
                            },
                            LoginName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["LoginName"])),
                            UserGeneralDetailVO = { 
                                LokedDateTime = (DateTime) DALHelper.HandleDBNull(reader["LockedDateTime"]),
                                Locked = (bool) reader["Locked"]
                            }
                        };
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return rvo;
        }

        public override IValueObject AutoUnLockUser(long UserId)
        {
            clsUserVO rvo = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateLockStatus");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, UserId);
                this.dbServer.AddInParameter(storedProcCommand, "LockStatus", DbType.Boolean, 0);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rvo = new clsUserVO {
                            LoginName = Security.base64Decode((string) reader["LoginName"])
                        };
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return rvo;
        }

        public override IValueObject ChangeFirstPassword(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsChangePasswordFirstTimeBizActionVO nvo = valueObject as clsChangePasswordFirstTimeBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateFirstPassword");
                this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, nvo.Details.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Password", DbType.String, Security.base64Encode(nvo.Details.Password));
                this.dbServer.AddInParameter(storedProcCommand, "Question", DbType.Int64, nvo.Details.UserGeneralDetailVO.SecreteQtn);
                this.dbServer.AddInParameter(storedProcCommand, "Answer", DbType.String, newSecurity.EncryptDecryptUserKey(nvo.Details.UserGeneralDetailVO.SecreteAns, false));
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                StringBuilder builder = new StringBuilder();
                StringBuilder builder2 = new StringBuilder();
                int count = nvo.DashBoardList.Count;
                int num2 = 0;
                while (true)
                {
                    if (num2 >= count)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "DashboardIdList", DbType.String, builder.ToString());
                        this.dbServer.AddInParameter(storedProcCommand, "DashboardStatusList", DbType.String, builder2.ToString());
                        this.dbServer.ExecuteNonQuery(storedProcCommand);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                        break;
                    }
                    clsDashBoardVO dvo = nvo.DashBoardList[num2];
                    builder.Append(dvo.ID);
                    builder2.Append(dvo.Status);
                    if (num2 < (count - 1))
                    {
                        builder.Append(",");
                        builder2.Append(",");
                    }
                    num2++;
                }
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject ChangePassword(IValueObject valueObject, clsUserVO UserVo)
        {
            clsChangePasswordBizActionVO nvo = valueObject as clsChangePasswordBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePassword");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, nvo.Details.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Password", DbType.String, Security.base64Encode(nvo.Details.Password));
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return nvo;
        }

        public override IValueObject CheckUserExists(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetExistingUserNameBizActionVO nvo = valueObject as clsGetExistingUserNameBizActionVO;
            try
            {
                clsUserVO userName = nvo.UserName;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetUserExists");
                if (userName.UserGeneralDetailVO.IsDoctor)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LoginId", DbType.Int64, userName.UserGeneralDetailVO.DoctorID);
                }
                else if (userName.UserGeneralDetailVO.IsEmployee)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LoginId", DbType.Int64, userName.UserGeneralDetailVO.EmployeeID);
                }
                else if (userName.UserGeneralDetailVO.IsPatient)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LoginId", DbType.Int64, userName.UserGeneralDetailVO.PatientID);
                }
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return nvo;
        }

        public override IValueObject CheckUserLoginExists(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetLoginNameBizActionVO nvo = valueObject as clsGetLoginNameBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetUserName");
                this.dbServer.AddInParameter(storedProcCommand, "LoginName", DbType.String, nvo.LoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return nvo;
        }

        public override IValueObject ForgotPassword(string LoginName, long SecretQ, string SecretA)
        {
            clsUserVO rvo = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ForgotPassword");
                this.dbServer.AddInParameter(storedProcCommand, "LoginName", DbType.String, Security.base64Encode(LoginName));
                this.dbServer.AddInParameter(storedProcCommand, "SecreteQtn", DbType.Int64, SecretQ);
                this.dbServer.AddInParameter(storedProcCommand, "SecreteAns", DbType.String, Security.base64Encode(SecretA));
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rvo = new clsUserVO {
                            ID = (long) reader["ID"],
                            PassConfig = { 
                                MaxPasswordLength = (short) DALHelper.HandleDBNull(reader["MaxPasswordLength"]),
                                MinPasswordLength = (short) DALHelper.HandleDBNull(reader["MinPasswordLength"]),
                                AtLeastOneDigit = (bool) DALHelper.HandleDBNull(reader["AtleastOneDigit"]),
                                AtLeastOneLowerCaseChar = (bool) DALHelper.HandleDBNull(reader["AtleastOneLowerCaseChar"]),
                                AtLeastOneUpperCaseChar = (bool) DALHelper.HandleDBNull(reader["AtleastOneUpperCaseChar"]),
                                AtLeastOneSpecialChar = (bool) DALHelper.HandleDBNull(reader["AtleastOneSpecialChar"])
                            }
                        };
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return rvo;
        }

        public override IValueObject GetCategoryList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetCategoryListBizActionVO nvo = valueObject as clsGetCategoryListBizActionVO;
            clsUserCategoryLinkVO categoryListDetails = nvo.CategoryListDetails;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CategoryListForUserLinking");
                this.dbServer.AddInParameter(storedProcCommand, "CategoryName", DbType.String, categoryListDetails.CategoryName);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.CategoryList == null)
                    {
                        nvo.CategoryList = new List<clsUserCategoryLinkVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                            reader.Close();
                            break;
                        }
                        clsUserCategoryLinkVO item = new clsUserCategoryLinkVO {
                            CategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            CategoryType = Convert.ToString(DALHelper.HandleDBNull(reader["CategoryType"])),
                            CategoryTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryTypeID"])),
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            AddedBy = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["AddedBy"]))),
                            UpdatedBy = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["UpdatedBy"]))),
                            AddedOn = Convert.ToString(DALHelper.HandleDBNull(reader["AddedOn"])),
                            UpdatedOn = Convert.ToString(DALHelper.HandleDBNull(reader["UpdatedOn"])),
                            AddedDateTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["AddedDateTime"])))
                        };
                        nvo.CategoryList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetEMRMenu(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetEMRMenuBizActionVO nvo = valueObject as clsGetEMRMenuBizActionVO;
            DbDataReader reader = null;
            nvo.MenuDetails = new List<clsMenuVO>();
            DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetUserWiseEMRMenuList");
            try
            {
                this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                while (reader.Read())
                {
                    clsMenuVO item = new clsMenuVO {
                        ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MenuID"])),
                        Title = Convert.ToString(DALHelper.HandleDBNull(reader["Title"])),
                        ImagePath = Convert.ToString(DALHelper.HandleDBNull(reader["ImagePath"])),
                        Parent = Convert.ToString(DALHelper.HandleDBNull(reader["Parent"])),
                        ParentId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ParentID"]))),
                        Module = Convert.ToString(DALHelper.HandleDBNull(reader["Module"])),
                        Action = Convert.ToString(DALHelper.HandleDBNull(reader["Action"])),
                        Header = Convert.ToString(DALHelper.HandleDBNull(reader["Header"])),
                        Configuration = Convert.ToString(DALHelper.HandleDBNull(reader["Configuration"])),
                        Mode = Convert.ToString(DALHelper.HandleDBNull(reader["Mode"])),
                        MenuOrder = new int?(Convert.ToInt32(DALHelper.HandleDBNull(reader["MenuOrder"]))),
                        ISFemale = Convert.ToBoolean(DALHelper.HandleDBNull(reader["isfemale"])),
                        Status = new bool?(Convert.ToBoolean(reader["Status"]))
                    };
                    nvo.MenuDetails.Add(item);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                reader.Close();
            }
            return nvo;
        }

        public override IValueObject GetExistingCategoryListForUser(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetExistingCategoryListBizActionVO nvo = valueObject as clsGetExistingCategoryListBizActionVO;
            clsUserCategoryLinkVO existingCategoryListDetails = nvo.ExistingCategoryListDetails;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetExistingLinkedCategoryForUser");
                this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, existingCategoryListDetails.UserID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ExistingCategoryList == null)
                    {
                        nvo.ExistingCategoryList = new List<clsUserCategoryLinkVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            reader.Close();
                            break;
                        }
                        clsUserCategoryLinkVO item = new clsUserCategoryLinkVO {
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryID"])),
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryID"])),
                            CategoryTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryTypeID"])),
                            CategoryType = Convert.ToString(DALHelper.HandleDBNull(reader["CategoryType"])),
                            UserID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UserID"])),
                            CategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["CategoryName"]))
                        };
                        nvo.ExistingCategoryList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetLicenseDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetLicenseDetailsBizActionVO nvo = valueObject as clsGetLicenseDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetLicenseDetails");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (nvo.UnitDetails == null)
                        {
                            nvo.UnitDetails = new clsUnitMasterVO();
                        }
                        nvo.Id = (long) DALHelper.HandleDBNull(reader["Code"]);
                        nvo.UnitDetails.Description = newSecurity.EncryptDecryptUserKey((string) DALHelper.HandleDBNull(reader["ClinicName"]), true);
                        nvo.UnitDetails.Area = newSecurity.EncryptDecryptUserKey((string) DALHelper.HandleDBNull(reader["Area"]), true);
                        nvo.UnitDetails.Pincode = newSecurity.EncryptDecryptUserKey((string) DALHelper.HandleDBNull(reader["PinCode"]), true);
                        nvo.UnitDetails.Email = (string) DALHelper.HandleDBNull(reader["EmailId"]);
                        nvo.sDate = newSecurity.EncryptDecryptUserKey((string) DALHelper.HandleDBNull(reader["CurrentD"]), true);
                        nvo.sTime = newSecurity.EncryptDecryptUserKey((string) DALHelper.HandleDBNull(reader["CurrentT"]), true);
                        nvo.K1 = newSecurity.EncryptDecryptUserKey((string) DALHelper.HandleDBNull(reader["K1"]), true);
                        nvo.K2 = newSecurity.EncryptDecryptUserKey((string) DALHelper.HandleDBNull(reader["K2"]), true);
                        nvo.UnitDetails.AddressLine1 = (string) DALHelper.HandleDBNull(reader["AddressLine1"]);
                        nvo.UnitDetails.AddressLine2 = (string) DALHelper.HandleDBNull(reader["AddressLine2"]);
                        nvo.UnitDetails.AddressLine3 = (string) DALHelper.HandleDBNull(reader["AddressLine3"]);
                        nvo.UnitDetails.Country = (string) DALHelper.HandleDBNull(reader["Country"]);
                        nvo.UnitDetails.State = (string) DALHelper.HandleDBNull(reader["State"]);
                        nvo.UnitDetails.District = (string) DALHelper.HandleDBNull(reader["District"]);
                        nvo.UnitDetails.Taluka = (string) DALHelper.HandleDBNull(reader["Taluka"]);
                        nvo.UnitDetails.City = (string) DALHelper.HandleDBNull(reader["City"]);
                        nvo.UnitDetails.ContactNo = (string) DALHelper.HandleDBNull(reader["ContactNo"]);
                        nvo.UnitDetails.ResiNoCountryCode = (int) DALHelper.HandleDBNull(reader["ResiCountryCode"]);
                        nvo.UnitDetails.ResiSTDCode = (int) DALHelper.HandleDBNull(reader["ResiSTDCode"]);
                        nvo.UnitDetails.FaxNo = (string) DALHelper.HandleDBNull(reader["FaxNo"]);
                        nvo.UnitDetails.Email = (string) DALHelper.HandleDBNull(reader["EmailId"]);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject GetLoginNamePassword(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetLoginNamePasswordBizActionVO nvo = valueObject as clsGetLoginNamePasswordBizActionVO;
            try
            {
                clsUserVO loginDetails = nvo.LoginDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetLoginPassword");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (nvo.LoginDetails == null)
                        {
                            nvo.LoginDetails = new clsUserVO();
                        }
                        nvo.LoginDetails.LoginName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["LoginName"]));
                        nvo.LoginDetails.UserLoginInfo.Name = Security.base64Decode((string) DALHelper.HandleDBNull(reader["LoginName"]));
                        nvo.LoginDetails.UserGeneralDetailVO.FirstPasswordChanged = (bool) DALHelper.HandleDBNull(reader["FirstPasswordChanged"]);
                        clsDashBoardVO item = new clsDashBoardVO {
                            ID = (long) DALHelper.HandleDBNull(reader["DashBoardID"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["CurrentStatus"]),
                            Description = (string) DALHelper.HandleDBNull(reader["DashBoardDescription"]),
                            Active = (bool) DALHelper.HandleDBNull(reader["Active"])
                        };
                        nvo.DashBoardList.Add(item);
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

        public override IValueObject GetSecretQtn(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsSecretQtnBizActionVO nvo = valueObject as clsSecretQtnBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSecretQtnList");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Details == null)
                    {
                        nvo.Details = new List<clsSecretQtnVO>();
                    }
                    while (reader.Read())
                    {
                        clsSecretQtnVO item = new clsSecretQtnVO {
                            Id = (long) DALHelper.HandleDBNull(reader["Id"]),
                            Value = (string) DALHelper.HandleDBNull(reader["Description"])
                        };
                        nvo.Details.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject GetUnitStoreList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetUnitStoreBizActionVO nvo = valueObject as clsGetUnitStoreBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetUnitStoreList");
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Details == null)
                    {
                        nvo.Details = new List<clsItemStoreVO>();
                    }
                    while (reader.Read())
                    {
                        clsItemStoreVO item = new clsItemStoreVO {
                            ID = (long) DALHelper.HandleDBNull(reader["StoreId"]),
                            StoreName = (string) DALHelper.HandleDBNull(reader["StoreDesc"])
                        };
                        nvo.Details.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject GetUnitStoreStatusList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetUnitStoreStatusBizActionVO nvo = valueObject as clsGetUnitStoreStatusBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetUnitStoreStatusList");
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UserId", DbType.Int64, nvo.UserId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Details == null)
                    {
                        nvo.Details = new List<clsItemStoreVO>();
                    }
                    while (reader.Read())
                    {
                        clsItemStoreVO item = new clsItemStoreVO {
                            ID = (long) DALHelper.HandleDBNull(reader["StoreId"]),
                            UnitId = (long) DALHelper.HandleDBNull(reader["UnitId"]),
                            UserStoreId = (long) DALHelper.HandleDBNull(reader["Id"]),
                            StoreName = (string) DALHelper.HandleDBNull(reader["StoreDesc"]),
                            StoreStatus = (reader["UserStoreStatus"].HandleDBNull() != null) && Convert.ToBoolean(reader["UserStoreStatus"])
                        };
                        nvo.Details.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject GetUser(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetUserBizActionVO nvo = valueObject as clsGetUserBizActionVO;
            try
            {
                clsUserVO details = nvo.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetUser");
                long iD = nvo.ID;
                if (nvo.ID != 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.Details.ID);
                }
                this.dbServer.AddInParameter(storedProcCommand, "FlagDisableUser", DbType.Int64, nvo.FlagDisableUser);
                this.dbServer.AddInParameter(storedProcCommand, "UserType", DbType.Int64, nvo.UserType);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string str;
                        string str2;
                        string str3;
                        if (nvo.Details == null)
                        {
                            nvo.Details = new clsUserVO();
                        }
                        nvo.Details.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.Details.LoginName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["LoginName"]));
                        nvo.Details.UserLoginInfo.Name = Security.base64Decode((string) DALHelper.HandleDBNull(reader["LoginName"]));
                        nvo.Details.Password = Security.base64Decode((string) DALHelper.HandleDBNull(reader["Password"]));
                        nvo.Details.UserGeneralDetailVO.IsEmployee = (bool) DALHelper.HandleDBNull(reader["IsEmployee"]);
                        nvo.Details.UserGeneralDetailVO.IsDoctor = (bool) DALHelper.HandleDBNull(reader["IsDoctor"]);
                        nvo.Details.UserGeneralDetailVO.IsPatient = (bool) DALHelper.HandleDBNull(reader["IsPatient"]);
                        nvo.Details.UserGeneralDetailVO.EmployeeID = (long) DALHelper.HandleDBNull(reader["EmployeeID"]);
                        nvo.Details.UserGeneralDetailVO.DoctorID = (long) DALHelper.HandleDBNull(reader["DoctorID"]);
                        nvo.Details.UserGeneralDetailVO.PatientID = (long) DALHelper.HandleDBNull(reader["PatientID"]);
                        nvo.Details.UserGeneralDetailVO.SecreteQtn = (long) DALHelper.HandleDBNull(reader["SecreteQtn"]);
                        if (((string) DALHelper.HandleDBNull(reader["SecreteAns"])) != null)
                        {
                            nvo.Details.UserGeneralDetailVO.SecreteAns = newSecurity.EncryptDecryptUserKey((string) DALHelper.HandleDBNull(reader["SecreteAns"]), true);
                        }
                        nvo.Details.UserGeneralDetailVO.Locked = (bool) DALHelper.HandleDBNull(reader["Locked"]);
                        nvo.Details.UserGeneralDetailVO.EnablePasswordExpiration = (bool) DALHelper.HandleDBNull(reader["EnablePasswordExpiration"]);
                        nvo.Details.UserGeneralDetailVO.PasswordExpirationInterval = (int) DALHelper.HandleDBNull(reader["PasswordExpirationInterval"]);
                        nvo.Details.UserGeneralDetailVO.FirstPasswordChanged = (bool) DALHelper.HandleDBNull(reader["FirstPasswordChanged"]);
                        nvo.Details.UserType = (int) DALHelper.HandleDBNull(reader["UserType"]);
                        nvo.Details.PassConfig.MinPasswordLength = (short) DALHelper.HandleDBNull(reader["MinPasswordLength"]);
                        nvo.Details.PassConfig.MaxPasswordLength = (short) DALHelper.HandleDBNull(reader["MaxPasswordLength"]);
                        nvo.Details.PassConfig.MinPasswordAge = (short) DALHelper.HandleDBNull(reader["MinPasswordAge"]);
                        nvo.Details.PassConfig.MaxPasswordAge = (short) DALHelper.HandleDBNull(reader["MaxPasswordAge"]);
                        nvo.Details.PassConfig.NoOfPasswordsToRemember = (short) DALHelper.HandleDBNull(reader["NoOfPasswordsToRemember"]);
                        nvo.Details.PassConfig.AtLeastOneDigit = (bool) DALHelper.HandleDBNull(reader["AtLeastOneDigit"]);
                        nvo.Details.PassConfig.AtLeastOneLowerCaseChar = (bool) DALHelper.HandleDBNull(reader["AtleastOneLowerCaseChar"]);
                        nvo.Details.PassConfig.AtLeastOneUpperCaseChar = (bool) DALHelper.HandleDBNull(reader["AtLeastOneUpperCaseChar"]);
                        nvo.Details.PassConfig.AtLeastOneSpecialChar = (bool) DALHelper.HandleDBNull(reader["AtLeastOneSpecialChar"]);
                        nvo.Details.PassConfig.AccountLockThreshold = (short) DALHelper.HandleDBNull(reader["AccountLockThreshold"]);
                        nvo.Details.PassConfig.AccountLockDuration = (float) ((double) DALHelper.HandleDBNull(reader["AccountLockDuration"]));
                        nvo.Details.UserGeneralDetailVO.RoleDetails = new clsUserRoleVO();
                        nvo.Details.UserGeneralDetailVO.RoleDetails.ID = (long) DALHelper.HandleDBNull(reader["RoleID"]);
                        nvo.Details.UserGeneralDetailVO.UnitId = (long) DALHelper.HandleDBNull(reader["UnitId"]);
                        nvo.Details.UserGeneralDetailVO.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                        nvo.Details.UserGeneralDetailVO.AddedBy = (long?) DALHelper.HandleDBNull(reader["AddedBy"]);
                        nvo.Details.UserGeneralDetailVO.AddedOn = (string) DALHelper.HandleDBNull(reader["AddedOn"]);
                        nvo.Details.UserGeneralDetailVO.AddedDateTime = DALHelper.HandleDate(reader["AddedDateTime"]);
                        nvo.Details.UserGeneralDetailVO.UpdatedBy = (long?) DALHelper.HandleDBNull(reader["UpdatedBy"]);
                        nvo.Details.UserGeneralDetailVO.UpdatedOn = (string) DALHelper.HandleDBNull(reader["UpdatedOn"]);
                        nvo.Details.UserGeneralDetailVO.UpdatedDateTime = DALHelper.HandleDate(reader["UpdatedDateTime"]);
                        if (nvo.Details.UserType == 3L)
                        {
                            str = Security.base64Decode((string) DALHelper.HandleDBNull(reader["UserFirstName"]));
                            str2 = Security.base64Decode((string) DALHelper.HandleDBNull(reader["UserMiddleName"]));
                            str3 = Security.base64Decode((string) DALHelper.HandleDBNull(reader["UserLastName"]));
                        }
                        else
                        {
                            str = (string) DALHelper.HandleDBNull(reader["UserFirstName"]);
                            str2 = (string) DALHelper.HandleDBNull(reader["UserMiddleName"]);
                            str3 = (string) DALHelper.HandleDBNull(reader["UserLastName"]);
                        }
                        if (str2 == null)
                        {
                            nvo.Details.UserGeneralDetailVO.UserName = str + " " + str3;
                        }
                        else
                        {
                            string[] strArray = new string[] { str, " ", str2, " ", str3 };
                            nvo.Details.UserGeneralDetailVO.UserName = string.Concat(strArray);
                        }
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    nvo.Details.UserGeneralDetailVO.UnitDetails = new List<clsUserUnitDetailsVO>();
                    while (reader.Read())
                    {
                        clsUserUnitDetailsVO item = new clsUserUnitDetailsVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (reader["UnitID"].HandleDBNull() == null) ? 0L : Convert.ToInt64(reader["UnitID"]),
                            Status = (reader["Status"].HandleDBNull() != null) && Convert.ToBoolean(reader["Status"]),
                            IsDefault = (reader["IsDefault"].HandleDBNull() != null) && Convert.ToBoolean(reader["IsDefault"])
                        };
                        nvo.Details.UserGeneralDetailVO.UnitDetails.Add(item);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    int num = 0;
                    while (num < nvo.Details.UserGeneralDetailVO.UnitDetails.Count)
                    {
                        nvo.Details.UserGeneralDetailVO.UnitDetails[num].StoreDetails = new List<clsItemStoreVO>();
                        while (true)
                        {
                            if (!reader.Read())
                            {
                                num++;
                                break;
                            }
                            clsItemStoreVO item = new clsItemStoreVO {
                                UserStoreId = (long) DALHelper.HandleDBNull(reader["Id"]),
                                StoreName = (string) DALHelper.HandleDBNull(reader["StoreName"]),
                                UnitId = (long) DALHelper.HandleDBNull(reader["ClinicId"]),
                                ID = (long) DALHelper.HandleDBNull(reader["StoreId"]),
                                StoreStatus = (bool) DALHelper.HandleDBNull(reader["StoreStatus"])
                            };
                            nvo.Details.UserGeneralDetailVO.UnitDetails[num].StoreDetails.Add(item);
                        }
                    }
                }
                clsUserRoleVO roleDetails = nvo.Details.UserGeneralDetailVO.RoleDetails;
                reader.NextResult();
                if (reader.HasRows)
                {
                    roleDetails.MenuList = new List<clsMenuVO>();
                    while (reader.Read())
                    {
                        clsMenuVO item = new clsMenuVO {
                            ID = (long) DALHelper.HandleDBNull(reader["MenuID"]),
                            Title = (string) DALHelper.HandleDBNull(reader["Title"]),
                            ImagePath = (string) DALHelper.HandleDBNull(reader["ImagePath"]),
                            Parent = (string) DALHelper.HandleDBNull(reader["Parent"]),
                            ParentId = new long?((long) DALHelper.HandleDBNull(reader["ParentID"])),
                            Module = (string) DALHelper.HandleDBNull(reader["Module"]),
                            Action = (string) DALHelper.HandleDBNull(reader["Action"]),
                            Header = (string) DALHelper.HandleDBNull(reader["Header"]),
                            Configuration = (string) DALHelper.HandleDBNull(reader["Configuration"]),
                            Mode = (string) DALHelper.HandleDBNull(reader["Mode"]),
                            MenuOrder = new int?((int) DALHelper.HandleDBNull(reader["MenuOrder"])),
                            Status = new bool?(Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])))
                        };
                        nvo.Details.UserGeneralDetailVO.RoleDetails.MenuList.Add(item);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    roleDetails.DashBoardList = new List<clsDashBoardVO>();
                    while (reader.Read())
                    {
                        clsDashBoardVO item = new clsDashBoardVO {
                            ID = (long) DALHelper.HandleDBNull(reader["DashBoardID"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        roleDetails.DashBoardList.Add(item);
                    }
                }
                nvo.Details.UserGeneralDetailVO.RoleDetails = roleDetails;
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetUserDashBoard(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetUserDashBoardVO dvo = valueObject as clsGetUserDashBoardVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetUserDashBoard");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, dvo.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (dvo.List == null)
                    {
                        dvo.List = new List<clsDashBoardVO>();
                    }
                    while (reader.Read())
                    {
                        clsDashBoardVO item = new clsDashBoardVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            Code = (string) DALHelper.HandleDBNull(reader["Code"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        dvo.List.Add(item);
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

        public override IValueObject GetUserEMRTemplateList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetUserEMRTemplateListBizActionVO nvo = valueObject as clsGetUserEMRTemplateListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetUserEMRTemplate");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.UserID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsUserEMRTemplateDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsUserEMRTemplateDetailsVO item = new clsUserEMRTemplateDetailsVO {
                            TemplateID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            TemplateName = (string) DALHelper.HandleDBNull(reader["Title"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
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

        public override IValueObject GetUserList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetUserListBizActionVO nvo = valueObject as clsGetUserListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetUserListForSearch");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "UserRoleID", DbType.String, nvo.UserRoleID);
                this.dbServer.AddInParameter(storedProcCommand, "IsDoctor", DbType.String, nvo.IsDoctor);
                this.dbServer.AddInParameter(storedProcCommand, "IsEmployee", DbType.String, nvo.IsEmployee);
                this.dbServer.AddInParameter(storedProcCommand, "IsPatient", DbType.String, nvo.IsPatient);
                this.dbServer.AddInParameter(storedProcCommand, "IsDeActive", DbType.String, nvo.IsDeActive);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UserId", DbType.Int64, objUserVO.ID);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Details == null)
                    {
                        nvo.Details = new List<clsUserVO>();
                    }
                    while (reader.Read())
                    {
                        clsUserVO item = new clsUserVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UserGeneralDetailVO = { UserName = (string) DALHelper.HandleDBNull(reader["UserName"]) },
                            LoginName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["LoginName"]))
                        };
                        item.UserLoginInfo.Name = item.LoginName;
                        item.UserGeneralDetailVO.Locked = (bool) DALHelper.HandleDBNull(reader["Locked"]);
                        item.UserGeneralDetailVO.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                        item.UserTypeName = (string) DALHelper.HandleDBNull(reader["UserType"]);
                        item.UserType = (long) DALHelper.HandleDBNull(reader["UserTypeID"]);
                        item.RoleName = (string) DALHelper.HandleDBNull(reader["Role"]);
                        item.EmailId = (string) DALHelper.HandleDBNull(reader["EmailId"]);
                        nvo.Details.Add(item);
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

        public override IValueObject LockUser(long UserId)
        {
            clsUserVO rvo = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateLockStatus");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, UserId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rvo = new clsUserVO {
                            LoginName = Security.base64Decode((string) reader["LoginName"])
                        };
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return rvo;
        }

        public override IValueObject ResetPassword(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsResetPasswordBizActionVO nvo = valueObject as clsResetPasswordBizActionVO;
            try
            {
                clsUserVO rPassword = nvo.RPassword;
                rPassword.UserGeneralDetailVO.FirstPasswordChanged = false;
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ResetPassword");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, rPassword.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Password", DbType.String, Security.base64Encode(rPassword.Password));
                this.dbServer.AddInParameter(storedProcCommand, "FirstPasswordChanged", DbType.Boolean, rPassword.UserGeneralDetailVO.FirstPasswordChanged);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.RPassword.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject UpdateAuditOnClose(long AuditId)
        {
            try
            {
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateAuditTrail");
                this.dbServer.AddInParameter(storedProcCommand, "AuditId", DbType.Int32, AuditId);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
            }
            return null;
        }

        public override IValueObject UpdateForgotPassword(long UserId, string NewPassword)
        {
            clsUserVO rvo = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateForgotPassword");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, UserId);
                this.dbServer.AddInParameter(storedProcCommand, "Password", DbType.String, Security.base64Encode(NewPassword));
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rvo = new clsUserVO();
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return rvo;
        }

        public override IValueObject UpdateLicenseTo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateLicenseToBizActionVO nvo = valueObject as clsUpdateLicenseToBizActionVO;
            clsUnitMasterVO rvo1 = new clsUnitMasterVO();
            clsUnitMasterVO unitDetails = nvo.UnitDetails;
            try
            {
                if (nvo.Id > 0L)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateCodeLicenseTo");
                    this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.Int64, nvo.Id);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                }
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject UpdateUserAuditTrail(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateAuditTrailBizActionVO nvo = valueObject as clsUpdateAuditTrailBizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateAuditTrail");
                this.dbServer.AddInParameter(storedProcCommand, "MachineName", DbType.String, Environment.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AuditId", DbType.Int32, this.AuditTrailId);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject UpdateUserDashBoard(IValueObject valueObj, clsUserVO objUserVO)
        {
            clsUpdateDashboardBizActionVO nvo = valueObj as clsUpdateDashboardBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateDashBoard");
                this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, nvo.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                StringBuilder builder = new StringBuilder();
                StringBuilder builder2 = new StringBuilder();
                int count = nvo.DashBoardList.Count;
                int num2 = 0;
                while (true)
                {
                    if (num2 >= count)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "DashboardIdList", DbType.String, builder.ToString());
                        this.dbServer.AddInParameter(storedProcCommand, "DashboardStatusList", DbType.String, builder2.ToString());
                        this.dbServer.ExecuteNonQuery(storedProcCommand);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                        break;
                    }
                    clsDashBoardVO dvo = nvo.DashBoardList[num2];
                    builder.Append(dvo.ID);
                    builder2.Append(dvo.Status);
                    if (num2 < (count - 1))
                    {
                        builder.Append(",");
                        builder2.Append(",");
                    }
                    num2++;
                }
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject UpdateUserLockedStatus(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateUserLockedStatusBizActionVO nvo = valueObject as clsUpdateUserLockedStatusBizActionVO;
            try
            {
                clsUserVO userLockedStatus = nvo.UserLockedStatus;
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateUserLockedStatus");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, userLockedStatus.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Locked", DbType.Boolean, userLockedStatus.UserGeneralDetailVO.Locked);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.UserLockedStatus.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject UpdateUserStatus(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateUserStatusBizActionVO nvo = valueObject as clsUpdateUserStatusBizActionVO;
            try
            {
                clsUserVO userStatus = nvo.UserStatus;
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateUserStatus");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, userStatus.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, userStatus.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.UserStatus.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return nvo;
        }
    }
}

