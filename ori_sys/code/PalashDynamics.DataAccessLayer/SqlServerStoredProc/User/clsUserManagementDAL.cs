using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
//using PalashDynamics.ConfigurationManager;

using System.Data.Common;
//using PalashDynamics.CustomExceptions;
using System.Data;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.ValueObjects.Administration.RoleMaster;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.User;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.Administration.UnitMaster;
using PalashDynamics.ValueObjects.EMR.NewEMR;
using PalashDynamics.ValueObjects.Administration.UserRights;

namespace  PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    internal partial class clsUserManagementDAL : clsBaseUserManagementDAL
    {
         //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        public int AuditTrailId=0;
        #endregion

        private clsUserManagementDAL()
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
        //public override IValueObject ForgotPassword(IValueObject valueObj, clsUserVO objUserVO)
        //{
        //    bool CurrentMethodExecutionStatus = true;
        //    clsGetSecretQuestionBizActionVO BizActionObj = valueObj as clsGetSecretQuestionBizActionVO;
        //    try
        //    {
        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdatePassword");
        //        dbServer.AddInParameter(command, "LoginName", DbType.String, Security.base64Encode(BizActionObj.Details.LoginName));
        //        dbServer.AddInParameter(command, "SecreteQtn", DbType.String, BizActionObj.Details.UserGeneralDetailVO.SecreteQtn);
        //        dbServer.AddInParameter(command, "SecreteAns", DbType.String, Security.base64Encode(BizActionObj.Details.UserGeneralDetailVO.SecreteAns));

        //        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

        //        int intStatus = dbServer.ExecuteNonQuery(command);
        //        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
               

        //    }
        //    catch (Exception ex)
        //    {
        //        CurrentMethodExecutionStatus = false;
        //        //logError
        //        throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
        //    }
        //    return BizActionObj;
        //}
       
        public override IValueObject UpdateForgotPassword(long UserId, string NewPassword)
        {
            bool CurrentMethodExecutionStatus = true;
            clsUserVO User = null;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateForgotPassword");
                dbServer.AddInParameter(command, "ID", DbType.Int64, UserId);
                dbServer.AddInParameter(command, "Password", DbType.String, Security.base64Encode(NewPassword)); // newSecurity.EncryptDecryptUserKey(NewPassword)); //
               
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        User = new clsUserVO();
                    }
                }
                reader.Close();
            }
            
             catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                //logError
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return User;
        }
      
        public override IValueObject ForgotPassword(string LoginName, long SecretQ, string SecretA)
        {
            bool CurrentMethodExecutionStatus = true;
            clsUserVO User = null;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_ForgotPassword");
                dbServer.AddInParameter(command, "LoginName", DbType.String, Security.base64Encode(LoginName));
                dbServer.AddInParameter(command, "SecreteQtn", DbType.Int64, SecretQ);
                dbServer.AddInParameter(command, "SecreteAns", DbType.String, Security.base64Encode(SecretA)); // newSecurity.EncryptDecryptUserKey(SecretA)); //
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        User = new clsUserVO();
                        User.ID = (long)reader["ID"];
                        User.PassConfig.MaxPasswordLength = (Int16)DALHelper.HandleDBNull(reader["MaxPasswordLength"]);
                        User.PassConfig.MinPasswordLength = (Int16)DALHelper.HandleDBNull(reader["MinPasswordLength"]);
                        User.PassConfig.AtLeastOneDigit = (bool)DALHelper.HandleDBNull(reader["AtleastOneDigit"]);
                        User.PassConfig.AtLeastOneLowerCaseChar = (bool)DALHelper.HandleDBNull(reader["AtleastOneLowerCaseChar"]);
                        User.PassConfig.AtLeastOneUpperCaseChar = (bool)DALHelper.HandleDBNull(reader["AtleastOneUpperCaseChar"]);
                        User.PassConfig.AtLeastOneSpecialChar = (bool)DALHelper.HandleDBNull(reader["AtleastOneSpecialChar"]);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                //logError
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return User;
        }

        public override IValueObject LockUser(long UserId)
        {
            bool CurrentMethodExecutionStatus = true;
            clsUserVO User = null;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateLockStatus");
                dbServer.AddInParameter(command, "ID", DbType.Int64, UserId);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        User = new clsUserVO();
                        User.LoginName= Security.base64Decode((string)reader["LoginName"]);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                //logError
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return User;
        }

        public override IValueObject AutoUnLockUser(long UserId)
        {
            bool CurrentMethodExecutionStatus = true;
            clsUserVO User = null;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateLockStatus");
                dbServer.AddInParameter(command, "ID", DbType.Int64, UserId);
                dbServer.AddInParameter(command, "LockStatus", DbType.Boolean,0);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        User = new clsUserVO();
                        User.LoginName = Security.base64Decode((string)reader["LoginName"]);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                //logError
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return User;
        }

        public override IValueObject UpdateAuditOnClose(long AuditId)
        {
            bool CurrentMethodExecutionStatus = true;
            clsUserVO User = null;
            try
            {
                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_UpdateAuditTrail");

                // dbServer.AddInParameter(command, "LoginName", DbType.String, Security.base64Encode(objUserVO.LoginName));
                //dbServer.AddInParameter(command, "MachineName", DbType.String, objUserVO.UserLoginInfo.MachineName);
                //dbServer.AddInParameter(command, "Id", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "AuditId", DbType.Int32, AuditId);
                //User.UserLoginInfo.RetunUnitId = AuditId;
                int intStatus = dbServer.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                //   throw;
            }

            return User;
        }

        public override IValueObject UpdateUserAuditTrail(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsUpdateAuditTrailBizActionVO bizObject = valueObject as clsUpdateAuditTrailBizActionVO;

            try
            {
                
                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_UpdateAuditTrail");
               
               // dbServer.AddInParameter(command, "LoginName", DbType.String, Security.base64Encode(objUserVO.LoginName));
                dbServer.AddInParameter(command, "MachineName", DbType.String, Environment.MachineName); //objUserVO.UserLoginInfo.MachineName);
                //dbServer.AddInParameter(command, "Id", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "AuditId", DbType.Int32, AuditTrailId);

                int intStatus = dbServer.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {                
             //   throw;
            }

            return bizObject;
        }

        public override IValueObject UpdateLicenseTo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateLicenseToBizActionVO BizActionObj = valueObject as clsUpdateLicenseToBizActionVO;
            clsUnitMasterVO objUnit = new clsUnitMasterVO();
            objUnit = BizActionObj.UnitDetails;
            try
            {
                if(BizActionObj.Id > 0)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateCodeLicenseTo");

                    dbServer.AddInParameter(command, "Code", DbType.Int64, BizActionObj.Id);  
                    
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus2 = dbServer.ExecuteNonQuery(command);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                }
            }
            catch (Exception ex)
            {                
             //   throw;
            }

            return BizActionObj;
        }
        
        public override IValueObject GetLicenseDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetLicenseDetailsBizActionVO BizActionObj = valueObject as clsGetLicenseDetailsBizActionVO;
            try
            {               

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetLicenseDetails");

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (BizActionObj.UnitDetails == null)
                            BizActionObj.UnitDetails = new clsUnitMasterVO();
                        BizActionObj.Id = (long)DALHelper.HandleDBNull(reader["Code"]);
                        BizActionObj.UnitDetails.Description = newSecurity.EncryptDecryptUserKey((string)DALHelper.HandleDBNull(reader["ClinicName"]), true);
                        BizActionObj.UnitDetails.Area = newSecurity.EncryptDecryptUserKey((string)DALHelper.HandleDBNull(reader["Area"]), true);
                        BizActionObj.UnitDetails.Pincode = newSecurity.EncryptDecryptUserKey((string)DALHelper.HandleDBNull(reader["PinCode"]), true);
                        BizActionObj.UnitDetails.Email = (string)DALHelper.HandleDBNull(reader["EmailId"]);
                        BizActionObj.sDate = newSecurity.EncryptDecryptUserKey((string)DALHelper.HandleDBNull(reader["CurrentD"]),true ); //Security.base64Decode((string)DALHelper.HandleDBNull(reader["CurrentD"]));
                        BizActionObj.sTime = newSecurity.EncryptDecryptUserKey((string)DALHelper.HandleDBNull(reader["CurrentT"]), true ); //Security.base64Decode((string)DALHelper.HandleDBNull(reader["CurrentT"]));           
                        BizActionObj.K1 = newSecurity.EncryptDecryptUserKey((string)DALHelper.HandleDBNull(reader["K1"]), true);
                        BizActionObj.K2 = newSecurity.EncryptDecryptUserKey((string)DALHelper.HandleDBNull(reader["K2"]), true);

                        BizActionObj.UnitDetails.AddressLine1 = (string)DALHelper.HandleDBNull(reader["AddressLine1"]);
                        BizActionObj.UnitDetails.AddressLine2 = (string)DALHelper.HandleDBNull(reader["AddressLine2"]);
                        BizActionObj.UnitDetails.AddressLine3 = (string)DALHelper.HandleDBNull(reader["AddressLine3"]);
                        BizActionObj.UnitDetails.Country = (string)DALHelper.HandleDBNull(reader["Country"]);
                        BizActionObj.UnitDetails.State = (string)DALHelper.HandleDBNull(reader["State"]);
                        BizActionObj.UnitDetails.District = (string)DALHelper.HandleDBNull(reader["District"]);
                        BizActionObj.UnitDetails.Taluka = (string)DALHelper.HandleDBNull(reader["Taluka"]);
                        BizActionObj.UnitDetails.City = (string)DALHelper.HandleDBNull(reader["City"]);
                        BizActionObj.UnitDetails.ContactNo = (string)DALHelper.HandleDBNull(reader["ContactNo"]);
                        BizActionObj.UnitDetails.ResiNoCountryCode = (int)DALHelper.HandleDBNull(reader["ResiCountryCode"]);
                        BizActionObj.UnitDetails.ResiSTDCode = (int)DALHelper.HandleDBNull(reader["ResiSTDCode"]);
                        BizActionObj.UnitDetails.FaxNo = (string)DALHelper.HandleDBNull(reader["FaxNo"]);
                        BizActionObj.UnitDetails.Email = (string)DALHelper.HandleDBNull(reader["EmailId"]);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {                
             //   throw;
            }
            return BizActionObj;
        }

        public override IValueObject AddtoDatabase(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsdbDetailsBizActionVO objUnitVO = valueObject as clsdbDetailsBizActionVO;
            try 
	        {	        		
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddToDB");
                dbServer.AddInParameter(command, "Value", DbType.String, Security.base64Encode(objUnitVO.Value));

                int intStatus2 = dbServer.ExecuteNonQuery(command);
	        }
	        catch (Exception ex)
	        {		
	        //	throw;
	        }

            return objUnitVO;
        }

        #region Add to static db is done via a new webservice  
              
        public override IValueObject AddtoStaticDB(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddLicenseToBizActionVO BizActionObj = valueObject as clsAddLicenseToBizActionVO;
            try
            {

                //add
                clsUnitMasterVO ObjUnitVO = BizActionObj.UnitDetails;
                if (ObjUnitVO.UnitID == 0)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddClientInfo");

                    dbServer.AddInParameter(command, "ClientName", DbType.String, newSecurity.EncryptDecryptUserKey(ObjUnitVO.Description));
                    dbServer.AddInParameter(command, "AddressLine1", DbType.String, ObjUnitVO.AddressLine1);
                    dbServer.AddInParameter(command, "AddressLine2", DbType.String, ObjUnitVO.AddressLine2);
                    dbServer.AddInParameter(command, "AddressLine3", DbType.String, ObjUnitVO.AddressLine3);
                    dbServer.AddInParameter(command, "Country", DbType.String, ObjUnitVO.Country);
                    dbServer.AddInParameter(command, "State", DbType.String, ObjUnitVO.State);
                    dbServer.AddInParameter(command, "City", DbType.String, ObjUnitVO.City);
                    dbServer.AddInParameter(command, "Taluka", DbType.String, ObjUnitVO.Taluka);
                    dbServer.AddInParameter(command, "Area", DbType.String, newSecurity.EncryptDecryptUserKey(ObjUnitVO.Area));
                    dbServer.AddInParameter(command, "District", DbType.String, ObjUnitVO.District);
                    dbServer.AddInParameter(command, "Pincode", DbType.String, newSecurity.EncryptDecryptUserKey(ObjUnitVO.Pincode));
                    dbServer.AddInParameter(command, "ResiNoCountryCode", DbType.Int32, ObjUnitVO.ResiNoCountryCode);
                    dbServer.AddInParameter(command, "ResiSTDCode", DbType.Int32, ObjUnitVO.ResiSTDCode);
                    dbServer.AddInParameter(command, "ContactNo", DbType.String, ObjUnitVO.ContactNo);
                    dbServer.AddInParameter(command, "FaxNo", DbType.String, ObjUnitVO.FaxNo);
                    dbServer.AddInParameter(command, "Email", DbType.String, ObjUnitVO.Email);

                    dbServer.AddInParameter(command, "RegistrationDateTime", DbType.DateTime, BizActionObj.RegDateTime);
                    dbServer.AddInParameter(command, "ClientKey", DbType.String, newSecurity.EncryptDecryptAccessKey(BizActionObj.Key));

                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjUnitVO.UnitID);

                    int intStatus2 = dbServer.ExecuteNonQuery(command);
                }
                else
                {
                    //Update.
                }

            }
            catch (Exception ex)
            {                
             //   throw;
            }

            return BizActionObj;
        }
        #endregion
        public override IValueObject AddLicenseTo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddLicenseToBizActionVO BizActionObj = valueObject as clsAddLicenseToBizActionVO;
            try
            {
                if (BizActionObj.K1 == "True")
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateLicenseTo");

                    dbServer.AddInParameter(command, "K1", DbType.String, newSecurity.EncryptDecryptUserKey(BizActionObj.K1));
                    dbServer.AddInParameter(command, "K2", DbType.String, newSecurity.EncryptDecryptUserKey(BizActionObj.K2));
                   
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                    
                    int intStatus2 = dbServer.ExecuteNonQuery(command);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                }
                else
                {
                    clsUnitMasterVO ObjUnitVO = BizActionObj.UnitDetails;
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddLicenseTo");
                    
                    dbServer.AddInParameter(command, "Name", DbType.String, newSecurity.EncryptDecryptUserKey(ObjUnitVO.Description));
                    dbServer.AddInParameter(command, "AddressLine1", DbType.String, ObjUnitVO.AddressLine1);
                    dbServer.AddInParameter(command, "AddressLine2", DbType.String, ObjUnitVO.AddressLine2);
                    dbServer.AddInParameter(command, "AddressLine3", DbType.String, ObjUnitVO.AddressLine3);
                    dbServer.AddInParameter(command, "Country", DbType.String, ObjUnitVO.Country);
                    dbServer.AddInParameter(command, "State", DbType.String, ObjUnitVO.State);
                    dbServer.AddInParameter(command, "City", DbType.String, ObjUnitVO.City);
                    dbServer.AddInParameter(command, "Taluka", DbType.String, ObjUnitVO.Taluka);
                    dbServer.AddInParameter(command, "Area", DbType.String, newSecurity.EncryptDecryptUserKey(ObjUnitVO.Area));
                    dbServer.AddInParameter(command, "District", DbType.String, ObjUnitVO.District);
                    dbServer.AddInParameter(command, "Pincode", DbType.String, newSecurity.EncryptDecryptUserKey(ObjUnitVO.Pincode));
                    dbServer.AddInParameter(command, "ContactNo", DbType.String, ObjUnitVO.ContactNo);
                    dbServer.AddInParameter(command, "ResiNoCountryCode", DbType.Int32, ObjUnitVO.ResiNoCountryCode);
                    dbServer.AddInParameter(command, "ResiSTDCode", DbType.Int32, ObjUnitVO.ResiSTDCode);
                    dbServer.AddInParameter(command, "FaxNo", DbType.String, ObjUnitVO.FaxNo);
                    dbServer.AddInParameter(command, "Email", DbType.String, ObjUnitVO.Email);


                    dbServer.AddInParameter(command, "CurrentD", DbType.String, newSecurity.EncryptDecryptUserKey(BizActionObj.sDate)); //Security.base64Encode(sdate));
                    dbServer.AddInParameter(command, "CurrentT", DbType.String, newSecurity.EncryptDecryptUserKey(BizActionObj.sTime)); //Security.base64Encode(stime));
                    dbServer.AddInParameter(command, "K1", DbType.String, newSecurity.EncryptDecryptUserKey(BizActionObj.K1));
                    dbServer.AddInParameter(command, "K2", DbType.String, newSecurity.EncryptDecryptUserKey(BizActionObj.K2));
                   

                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjUnitVO.UnitID);

                    int intStatus2 = dbServer.ExecuteNonQuery(command);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                }
                
            }
            catch (Exception ex)
            {                
             //   throw;
            }

            return BizActionObj;
        }
       
        public override IValueObject AuthenticateLoginName(string LoginName, string Password, long UnitId)
        {
            bool CurrentMethodExecutionStatus = true;
            
            clsUserVO User = null;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_LoginNameExists");
                
                dbServer.AddInParameter(command, "LoginName", DbType.String, Security.base64Encode(LoginName));
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UnitId);
                if (Password != null && Password != " ")
                {
                    dbServer.AddInParameter(command, "Password", DbType.String, Security.base64Encode(Password)); //  newSecurity.EncryptDecryptUserKey(Password)); //
                }
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                 
                //dbServer.AddInParameter(command, "Password", DbType.String, Security.base64Encode(Password));
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                //AuditTrailId = (int)dbServer.GetParameterValue(command, "ResultStatus");

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        User = new clsUserVO();
                        User.IsAuditTrail = Convert.ToBoolean(HMSConfigurationManager.GetValueFromApplicationConfig("IsAuditTrail"));  // By Umesh For Enable/Disable Audit Trail
                        User.ID = (long)reader["Id"];
                     
                        User.UserGeneralDetailVO.DoctorID = (long)reader["DoctorID"];
                        User.UserGeneralDetailVO.EmployeeID = (long)reader["EmployeeID"];
                        User.LoginName = Security.base64Decode((string)reader["LoginName"]);
                        User.UserNameNew = Convert.ToString(reader["UserName"]);
                        
                        User.UserGeneralDetailVO.FirstPasswordChanged = (bool)DALHelper.HandleDBNull(reader["FirstPasswordChanged"]);
                        User.UserGeneralDetailVO.RoleDetails.ID = (long)DALHelper.HandleDBNull(reader["RoleID"]);
                        User.RoleName = (string)DALHelper.HandleDBNull(reader["Description"]);
                        User.UserGeneralDetailVO.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                        User.UserGeneralDetailVO.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                        User.UserGeneralDetailVO.AddedBy = (Int64?)DALHelper.HandleDBNull(reader["AddedBy"]);
                        User.UserGeneralDetailVO.AddedOn = (String)DALHelper.HandleDBNull(reader["AddedOn"]);
                        User.UserGeneralDetailVO.AddedDateTime = (DateTime?)DALHelper.HandleDate(reader["AddedDateTime"]);

                        User.Password = newSecurity.EncryptDecryptUserKey(Convert.ToString(DALHelper.HandleDBNull(reader["Password"])), true); // Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["Password"]))); //
                        User.PasswordDe = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["Password"])));
                        //User.UserGeneralDetailVO.SecretQtnValue = (string)DALHelper.HandleDBNull(reader["SecretQtn"]);
                        User.PassConfig.AccountLockThreshold = (Int16)DALHelper.HandleDBNull(reader["AccountLockThreshold"]);
                        User.UserGeneralDetailVO.Locked = (bool)DALHelper.HandleDBNull(reader["Locked"]);

                        //Password Details.
                        User.UserGeneralDetailVO.DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"]));
                        User.UserGeneralDetailVO.IsEmployee = (bool)DALHelper.HandleDBNull(reader["IsEmployee"]);
                        User.UserGeneralDetailVO.IsDoctor = (bool)DALHelper.HandleDBNull(reader["IsDoctor"]);
                        User.UserGeneralDetailVO.IsPatient = (bool)DALHelper.HandleDBNull(reader["IsPatient"]);
                        User.UserGeneralDetailVO.EmployeeID = (long)DALHelper.HandleDBNull(reader["EmployeeID"]);
                        User.UserGeneralDetailVO.DoctorID = (long)DALHelper.HandleDBNull(reader["DoctorID"]);
                        User.UserGeneralDetailVO.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                        //User.UserGeneralDetailVO.SecretQtnValue = (string)DALHelper.HandleDBNull(reader["SecretQtn"]);
                        User.UserGeneralDetailVO.SecreteQtn = (long)DALHelper.HandleDBNull(reader["SecreteQtn"]);
                       // User.UserGeneralDetailVO.SecreteAns = newSecurity.EncryptDecryptUserKey((string)DALHelper.HandleDBNull(reader["SecreteAns"]), true); //Security.base64Decode((string)DALHelper.HandleDBNull(reader["SecreteAns"])); //
                        User.UserGeneralDetailVO.Locked = (bool)DALHelper.HandleDBNull(reader["Locked"]);
                        User.UserGeneralDetailVO.EnablePasswordExpiration = (bool)DALHelper.HandleDBNull(reader["EnablePasswordExpiration"]);
                        User.UserGeneralDetailVO.PasswordExpirationInterval = (int)DALHelper.HandleDBNull(reader["PasswordExpirationInterval"]);
                        User.UserGeneralDetailVO.FirstPasswordChanged = (bool)DALHelper.HandleDBNull(reader["FirstPasswordChanged"]);
                        User.UserType = (int)DALHelper.HandleDBNull(reader["UserType"]);

                        // For Password Configuration.
                        User.PassConfig.MinPasswordLength = (Int16)DALHelper.HandleDBNull(reader["MinPasswordLength"]);
                        User.PassConfig.MaxPasswordLength = (Int16)DALHelper.HandleDBNull(reader["MaxPasswordLength"]);
                        User.PassConfig.MinPasswordAge = (Int16)DALHelper.HandleDBNull(reader["MinPasswordAge"]);
                        User.PassConfig.MaxPasswordAge = (Int16)DALHelper.HandleDBNull(reader["MaxPasswordAge"]);
                        User.PassConfig.NoOfPasswordsToRemember = (Int16)DALHelper.HandleDBNull(reader["NoOfPasswordsToRemember"]);
                        User.PassConfig.AtLeastOneDigit = (bool)DALHelper.HandleDBNull(reader["AtLeastOneDigit"]);
                        User.PassConfig.AtLeastOneLowerCaseChar = (bool)DALHelper.HandleDBNull(reader["AtleastOneLowerCaseChar"]);
                        User.PassConfig.AtLeastOneUpperCaseChar = (bool)DALHelper.HandleDBNull(reader["AtLeastOneUpperCaseChar"]);
                        User.PassConfig.AtLeastOneSpecialChar = (bool)DALHelper.HandleDBNull(reader["AtLeastOneSpecialChar"]);
                        User.PassConfig.AccountLockThreshold = (Int16)DALHelper.HandleDBNull(reader["AccountLockThreshold"]);
                        User.PassConfig.AccountLockDuration = (float)(double)DALHelper.HandleDBNull(reader["AccountLockDuration"]);

                        //End Password Configuration.

                        User.UserGeneralDetailVO.RoleDetails = new clsUserRoleVO();
                        User.UserGeneralDetailVO.RoleDetails.ID = (long)DALHelper.HandleDBNull(reader["RoleID"]);
                        User.UserGeneralDetailVO.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                        User.UserGeneralDetailVO.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                        User.UserGeneralDetailVO.AddedBy = (Int64?)DALHelper.HandleDBNull(reader["AddedBy"]);
                        User.UserGeneralDetailVO.AddedOn = (String)DALHelper.HandleDBNull(reader["AddedOn"]);
                        User.UserGeneralDetailVO.AddedDateTime = (DateTime?)DALHelper.HandleDate(reader["AddedDateTime"]);
                        User.UserGeneralDetailVO.UpdatedBy = (Int64?)DALHelper.HandleDBNull(reader["UpdatedBy"]);
                        User.UserGeneralDetailVO.UpdatedOn = (String)DALHelper.HandleDBNull(reader["UpdatedOn"]);
                        User.UserGeneralDetailVO.UpdatedDateTime = (DateTime?)DALHelper.HandleDate(reader["UpdatedDateTime"]);

                        // Use to Enable Item Selection control on Counter Sale Screen
                        User.IsCSControlEnable = Convert.ToBoolean(HMSConfigurationManager.GetValueFromApplicationConfig("IsCSControlEnable"));

                        #region Added by Prashant Channe on 27/11/2019, reading ReportsFP config setting

                        User.ReportsFolder = Convert.ToString(HMSConfigurationManager.GetValueFromApplicationConfig("ReportsFolder"));

                        #endregion

                        // Use to Enable Item Selection control on Counter Sale Screen
                        User.IsEmailMandatory = Convert.ToBoolean(HMSConfigurationManager.GetValueFromApplicationConfig("IsEmailMandatory"));

                        User.PackageID = Convert.ToInt64(HMSConfigurationManager.GetValueFromApplicationConfig("PackageID"));

                        User.ValidationsFlag = Convert.ToBoolean(HMSConfigurationManager.GetValueFromApplicationConfig("ValidationsFlag")); //Added by NileshD on 19April2019
                      
                        string strFirstName, strMiddleName, strLastName;

                        //switch (User.UserType)
                        //{
                        //    //case 1: //'Doctor'
                        //    //    strFirstName = (string)DALHelper.HandleDBNull(reader["UserFirstName"]);
                        //    //    strMiddleName = (string)DALHelper.HandleDBNull(reader["UserMiddleName"]);
                        //    //    strLastName = (string)DALHelper.HandleDBNull(reader["UserLastName"]);
                        //    //    break;

                        //    //case 2: //Employee
                        //    //    strFirstName = (string)DALHelper.HandleDBNull(reader["UserFirstName"]);
                        //    //    strMiddleName = (string)DALHelper.HandleDBNull(reader["UserMiddleName"]);
                        //    //    strLastName = (string)DALHelper.HandleDBNull(reader["UserLastName"]);
                        //    //    break;

                        //    //case 3: // Patient
                        //    //    strFirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["UserFirstName"]));
                        //    //    strMiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["UserMiddleName"]));
                        //    //    strLastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["UserLastName"]));
                        //    //    break;

                        //    default:

                        //        strFirstName = (string)DALHelper.HandleDBNull(reader["UserFirstName"]);
                        //        strMiddleName = (string)DALHelper.HandleDBNull(reader["UserMiddleName"]);
                        //        strLastName = (string)DALHelper.HandleDBNull(reader["UserLastName"]);
                        //        break;
                        //}

                        //User.UserGeneralDetailVO.UserName = strFirstName + " " + strMiddleName + " " + strLastName;

                        //User.UserGeneralDetailVO.UnitName = (string)reader["UnitName"];
                        //User.UserGeneralDetailVO.UnitDescription = (string)reader["UnitDescription"];

                        /////////////////////////////////////////////////////////////////////////////////////////
                    }
                }
                             

                reader.NextResult();
                if (reader.HasRows)
                {
                    User.UserGeneralDetailVO.UnitDetails = new List<clsUserUnitDetailsVO>();
                    while (reader.Read())
                    {
                        clsUserUnitDetailsVO objUnit = new clsUserUnitDetailsVO();

                        objUnit.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objUnit.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        objUnit.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        objUnit.IsDefault = (bool)DALHelper.HandleDBNull(reader["IsDefault"]);

                        User.UserGeneralDetailVO.UnitDetails.Add(objUnit);
                    }
                }               

                clsUserRoleVO obj;
               
                reader.NextResult();
                if (reader.HasRows)
                {
                    obj = User.UserGeneralDetailVO.RoleDetails;
                    obj.MenuList = new List<clsMenuVO>();
                    while (reader.Read())
                    {
                        //obj.RoleDetails.RoleMenuRights.Add(new RoleMenuRightsItem(Convert.ToInt64(reader["MenuId"]), Convert.ToBoolean(reader["Status"])));

                        clsMenuVO objMenu = new clsMenuVO();
                        objMenu.ID = (long)DALHelper.HandleDBNull(reader["MenuID"]);
                        objMenu.Title = (string)DALHelper.HandleDBNull(reader["Title"]);
                        objMenu.ImagePath = (string)DALHelper.HandleDBNull(reader["ImagePath"]);
                        objMenu.Parent = (string)DALHelper.HandleDBNull(reader["Parent"]);
                        objMenu.ParentId = (long)DALHelper.HandleDBNull(reader["ParentID"]);
                        objMenu.Module = (string)DALHelper.HandleDBNull(reader["Module"]);
                        objMenu.Action = (string)DALHelper.HandleDBNull(reader["Action"]);
                        objMenu.Header = (string)DALHelper.HandleDBNull(reader["Header"]);
                        objMenu.Configuration = (string)DALHelper.HandleDBNull(reader["Configuration"]);
                        objMenu.Mode = (string)DALHelper.HandleDBNull(reader["Mode"]);
                        //Added by ramesh
                        objMenu.SOPFileName = (string)DALHelper.HandleDBNull(reader["SOPFileName"]);
                      //  objMenu.Active = (bool)DALHelper.HandleDBNull(reader["Active"]);
                        objMenu.MenuOrder = (int)DALHelper.HandleDBNull(reader["MenuOrder"]);
                        objMenu.Status = Convert.ToBoolean(reader["Status"]);
                        //objMenu.IsPrint = (bool)DALHelper.HandleDBNull(reader["IsPrint"]);
                        //objMenu.IsUpdate = (bool)DALHelper.HandleDBNull(reader["IsUpdate"]);
                        //objMenu.IsRead = (bool)DALHelper.HandleDBNull(reader["IsRead"]);
                        //objMenu.IsDelete = (bool)DALHelper.HandleDBNull(reader["IsDelete"]);
                        //objMenu.IsCreate = (bool)DALHelper.HandleDBNull(reader["IsCreate"]);
                        User.UserGeneralDetailVO.RoleDetails.MenuList.Add(objMenu);
                        
                        //obj.MenuList.Add(new clsMenuVO()
                        //{
                        //    //ID = Convert.ToInt64(reader["MenuId"]),
                        //    //Status = Convert.ToBoolean(reader["Status"]),
                        //    //IsPrint = (bool)DALHelper.HandleDBNull(reader["IsPrint"]),
                        //    //IsUpdate = (bool)DALHelper.HandleDBNull(reader["IsUpdate"]),
                        //    //IsRead = (bool)DALHelper.HandleDBNull(reader["IsRead"]),
                        //    //IsDelete = (bool)DALHelper.HandleDBNull(reader["IsDelete"]),
                        //    //IsCreate = (bool)DALHelper.HandleDBNull(reader["IsCreate"])
                        
                        //});
                    }


                    reader.NextResult();
                    if (reader.HasRows)
                    {
                        obj.DashBoardList = new List<clsDashBoardVO>();
                        while (reader.Read())
                        {
                            obj.DashBoardList.Add(new clsDashBoardVO()
                            {
                                ID = (long)DALHelper.HandleDBNull(reader["DashBoardID"]),
                                Status = (bool)DALHelper.HandleDBNull(reader["Status"])
                            });
                        }
                    }

                    User.UserGeneralDetailVO.RoleDetails = obj;
                }
 
                 reader.NextResult();
                    if (reader.HasRows)
                    {
                       // User.UserCategoryLinkList = new List<clsUserCategoryLinkVO>();
                        while (reader.Read())
                        {
                            clsUserCategoryLinkVO objUserCat = new clsUserCategoryLinkVO();

                            objUserCat.UserCategoryLinkID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UserCategoryLinkID"]));
                            objUserCat.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                            objUserCat.UserID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UserID"]));
                            objUserCat.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryID"]));
                            objUserCat.CategoryTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryTypeID"]));
                            objUserCat.CategoryType = Convert.ToString(DALHelper.HandleDBNull(reader["CategoryType"]));
                            objUserCat.CategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["CategoryName"]));
                            User.UserCategoryLinkList.Add(objUserCat);
                        }
                    }

                    reader.NextResult();    //Rohini for aditional rights of users
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {                  
                            User.UserAditionalRights.IsEditAfterFinalized = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEditAfterFinalized"]));
                        }
                    }
                    
                reader.Close();

               

                AuditTrailId = (int)dbServer.GetParameterValue(command, "ResultStatus");
                if(User!=null) User.UserLoginInfo.RetunUnitId = AuditTrailId;

                if (User != null)
                {
                    if (User.UserGeneralDetailVO.UnitDetails.Count != 0)
                    {
                        List<long> unitList = new List<long>();
                        foreach (var item in User.UserGeneralDetailVO.UnitDetails)
                        {
                            unitList.Add(item.UnitID);
                        }

                        bool validEntry = unitList.Contains(UnitId);
                        if (validEntry == false)
                        {
                            DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_UpdateValidEntryOfUser");

                            dbServer.AddInParameter(command1, "ID", DbType.Int64, AuditTrailId);
                            dbServer.AddInParameter(command1, "LoginName", DbType.String, Security.base64Encode(LoginName));
                            dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UnitId);
                            if (Password != null && Password != " ")
                            {
                                dbServer.AddInParameter(command1, "Password", DbType.String, Security.base64Encode(Password)); //  newSecurity.EncryptDecryptUserKey(Password)); //
                            }
                            dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                            //dbServer.AddInParameter(command, "Password", DbType.String, Security.base64Encode(Password));
                            int intStatus = dbServer.ExecuteNonQuery(command1);
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw;
                //logError
              //  throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return User;
        } 

        public override clsUserVO AuthenticateUser(string LoginName, string Password)
        {
            bool CurrentMethodExecutionStatus = true;
            clsUserVO User = null;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AuthenticateUser");
                dbServer.AddInParameter(command, "LoginName", DbType.String, Security.base64Encode(LoginName));
              /// dbServer.AddInParameter(command, "Password", DbType.String, Security.base64Encode(Password));
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        User = new clsUserVO();
                        //User.ID = (long)reader["UserId"];
                        User.ID = (long)reader["Id"];
                        User.PassConfig.AccountLockThreshold = (Int16)DALHelper.HandleDBNull(reader["AccountLockThreshold"]);
                        User.LoginName=Security.base64Decode((string)DALHelper.HandleDBNull(reader["LoginName"]));
                        User.PassConfig.AccountLockDuration = (float)Convert.ToDouble(DALHelper.HandleDBNull(reader["AccountLockDuration"])); //By Umesh
                        User.UserGeneralDetailVO.LokedDateTime = (DateTime)DALHelper.HandleDBNull(reader["LockedDateTime"]);
                        User.UserGeneralDetailVO.Locked = (bool)reader["Locked"];
                       //// User.LoginUnitId = (long)reader["UserUnitID"];
                      
                       //// User.UserGeneralDetailVO.UserTypeID = (bool)reader["UserTypeID"];
                       // User.UserGeneralDetailVO.DoctorID = (long)reader["DoctorID"];
                       //// User.UserGeneralDetailVO.UserName = (string)reader["UserName"];
                       // User.LoginName = Security.base64Decode((string)reader["LoginName"]);
                       // User.UserGeneralDetailVO.FirstPasswordChanged = (bool)DALHelper.HandleDBNull(reader["FirstPasswordChanged"]);
                       // User.UserGeneralDetailVO.RoleDetails.ID = (long)DALHelper.HandleDBNull(reader["RoleID"]);
                       // User.UserGeneralDetailVO.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                       // User.UserGeneralDetailVO.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                       // User.UserGeneralDetailVO.AddedBy = (Int64?)DALHelper.HandleDBNull(reader["AddedBy"]);
                       // User.UserGeneralDetailVO.AddedOn = (String)DALHelper.HandleDBNull(reader["AddedOn"]);
                       // User.UserGeneralDetailVO.AddedDateTime = (DateTime?)DALHelper.HandleDate(reader["AddedDateTime"]);

                       // User.UserGeneralDetailVO.UpdatedBy = (Int64?)DALHelper.HandleDBNull(reader["UpdatedBy"]);
                       // User.UserGeneralDetailVO.UpdatedOn = (String)DALHelper.HandleDBNull(reader["UpdatedOn"]);
                       // User.UserGeneralDetailVO.UpdatedDateTime = (DateTime?)DALHelper.HandleDate(reader["UpdatedDateTime"]);

                        //User.UserGeneralDetailVO.UnitName = (string)reader["UnitName"];
                        //User.UserGeneralDetailVO.UnitDescription = (string)reader["UnitDescription"];


                    }

                    //--------------- Begin For User rightswise fill Unit List -------------------------- 02032017

                    reader.NextResult();

                    while (reader.Read())
                    {
                        if (User.UserUnitList == null)
                        {
                            User.UserUnitList = new List<clsUserUnitDetailsVO>();
                        }

                        clsUserUnitDetailsVO _UUDVO = new clsUserUnitDetailsVO();
                        _UUDVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        _UUDVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        _UUDVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        _UUDVO.IsDefault = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDefault"]));

                        User.UserUnitList.Add(_UUDVO);
                    }

                    //--------------- Begin For User rightswise fill Unit List -------------------------- 02032017

                }
                reader.Close();
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw;
                //logError
               // throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return User;
        }

        //public override IValueObject ChangePassword(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    bool CurrentMethodExecutionStatus = true;
        //    clsChangePasswordBizActionVO BizActionObj = valueObject as clsChangePasswordBizActionVO;
        //    try
        //    {
        //        DbCommand command = dbServer.GetStoredProcCommand("Web_IVF_ChangePassword");
        //        //dbServer.AddInParameter(command, "UserId", DbType.Int64, BizActionObj.ID);
        //        dbServer.AddInParameter(command, "UserPassword", DbType.String, Security.base64Encode(BizActionObj.NewPassword));
        //        dbServer.AddInParameter(command, "UserId", DbType.Int64, UserVo.ID);
        //        dbServer.AddInParameter(command, "MachineName", DbType.String, UserVo.LoginMachineName);
        //        dbServer.AddInParameter(command, "ServerDateTime", DbType.DateTime, DateTime.Today);
        //        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
        //        int intStatus = dbServer.ExecuteNonQuery(command);
        //        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
        //        //BizActionObj.AlertDetail.AlertId = (long)dbServer.GetParameterValue(command, "AlertId");
        //    }
        //    catch (Exception ex)
        //    {
        //        CurrentMethodExecutionStatus = false;
        //        //logError
        //        throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
        //    }
        //    return BizActionObj;
        //}
              
        public override IValueObject ChangePassword(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsChangePasswordBizActionVO BizActionObj = valueObject as clsChangePasswordBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdatePassword");
                dbServer.AddInParameter(command, "Id", DbType.Int64, BizActionObj.Details.ID);
                dbServer.AddInParameter(command, "Password", DbType.String, Security.base64Encode(BizActionObj.Details.Password)); // newSecurity.EncryptDecryptUserKey(BizActionObj.Details.Password)); //
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
              
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);   
             
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                //logError
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return BizActionObj;
        }
             
        public override IValueObject ChangeFirstPassword(IValueObject valueObject, clsUserVO objUserVO)
        {            bool CurrentMethodExecutionStatus = true;
            clsChangePasswordFirstTimeBizActionVO BizActionObj = valueObject as clsChangePasswordFirstTimeBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateFirstPassword");
                dbServer.AddInParameter(command, "UserID", DbType.Int64, BizActionObj.Details.ID );// .UserId);
                dbServer.AddInParameter(command, "Password", DbType.String, Security.base64Encode(BizActionObj.Details.Password)); //.NewPassword)); newSecurity.EncryptDecryptUserKey(BizActionObj.Details.Password)); //
                //dbServer.AddInParameter(command, "Question", DbType.String, BizActionObj.Details.UserGeneralDetailVO.SecreteQtn);//.strQuestion);
                dbServer.AddInParameter(command, "Question", DbType.Int64, BizActionObj.Details.UserGeneralDetailVO.SecreteQtn);
                dbServer.AddInParameter(command, "Answer", DbType.String, newSecurity.EncryptDecryptUserKey(BizActionObj.Details.UserGeneralDetailVO.SecreteAns)); // Security.base64Encode(BizActionObj.Details.UserGeneralDetailVO.SecreteAns)); //.strAnswer));
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

              
                StringBuilder DashboardIdList = new StringBuilder();
                StringBuilder DashboardStatusList = new StringBuilder();

              //  clsUserVO varDashBoard = BizActionObj.DashBoardList;
                //clsUserRoleVO varRole = BizActionObj.Details.UserGeneralDetailVO.RoleDetails;
                //clsDashBoardVO varDashboard = BizActionObj.DashBoardList;
                int count = BizActionObj.DashBoardList.Count;              

                //for (int DashboardCnt = 0; DashboardCnt < varRole.DashBoardList.Count; DashboardCnt++)
                for (int DashboardCnt = 0; DashboardCnt < count; DashboardCnt++)
                {
                    clsDashBoardVO varDashboard = BizActionObj.DashBoardList[DashboardCnt]; //varRole.DashBoardList[DashboardCnt];
                    DashboardIdList.Append(varDashboard.ID);
                    DashboardStatusList.Append(varDashboard.Status);

                    if (DashboardCnt < (count - 1))
                    {
                        DashboardIdList.Append(",");
                        DashboardStatusList.Append(",");

                    }
                }

                dbServer.AddInParameter(command, "DashboardIdList", DbType.String, DashboardIdList.ToString());
                dbServer.AddInParameter(command, "DashboardStatusList", DbType.String, DashboardStatusList.ToString());

                
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");



                //StringBuilder DashboardIdList = new StringBuilder();
                //StringBuilder DashboardStatusList = new StringBuilder();


                //clsUserRoleVO varRole = BizActionObj.Details.UserGeneralDetailVO.RoleDetails;
                //    //objVO.UserGeneralDetailVO.RoleDetails;

                //for (int DashboardCnt = 0; DashboardCnt < varRole.DashBoardList.Count; DashboardCnt++)
                //{
                //    clsDashBoardVO varDashboard = varRole.DashBoardList[DashboardCnt];
                //    DashboardIdList.Append(varDashboard.ID);
                //    DashboardStatusList.Append(varDashboard.Status);

                //    if (DashboardCnt < (varRole.DashBoardList.Count - 1))
                //    {
                //        DashboardIdList.Append(",");
                //        DashboardStatusList.Append(",");

                //    }
                //}

            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                //logError
               // throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return BizActionObj;
        }

        private void AddCommonParametersForAddUpdateMethods(DbCommand command, clsUserVO objVO, clsUserVO objUserVO)
        {
            try
            {

                dbServer.AddInParameter(command, "Password", DbType.String, Security.base64Encode(objVO.Password)); // newSecurity.EncryptDecryptUserKey(objVO.Password));  //
                dbServer.AddInParameter(command, "LoginName", DbType.String, Security.base64Encode(objVO.LoginName));
                dbServer.AddInParameter(command, "IsEmployee", DbType.Boolean, objVO.UserGeneralDetailVO.IsEmployee);
                dbServer.AddInParameter(command, "IsDoctor", DbType.Boolean, objVO.UserGeneralDetailVO.IsDoctor);
                dbServer.AddInParameter(command, "IsPatient", DbType.Boolean, objVO.UserGeneralDetailVO.IsPatient);
                dbServer.AddInParameter(command, "EmployeeID", DbType.Int64, objVO.UserGeneralDetailVO.EmployeeID);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objVO.UserGeneralDetailVO.DoctorID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objVO.UserGeneralDetailVO.PatientID);
                //dbServer.AddInParameter(command, "SecreteQtn", DbType.String, objVO.UserGeneralDetailVO.SecreteQtn);
                // dbServer.AddInParameter(command, "SecreteQtn", DbType.Int64, objVO.UserGeneralDetailVO.SecreteQtn);
                //dbServer.AddInParameter(command, "SecreteAns", DbType.String, Security.base64Encode(objVO.UserGeneralDetailVO.SecreteAns));
                dbServer.AddInParameter(command, "Locked", DbType.Boolean, objVO.UserGeneralDetailVO.Locked);
                //  dbServer.AddInParameter(command, "EnablePassowordExpiration", DbType.Boolean, objVO.UserGeneralDetailVO.EnablePasswordExpiration);
                dbServer.AddInParameter(command, "EnablePasswordExpiration", DbType.Boolean, objVO.UserGeneralDetailVO.EnablePasswordExpiration);
                //dbServer.AddInParameter(command, "FirstPasswordChanged", DbType.Boolean, objVO.UserGeneralDetailVO.FirstPasswordChanged);
                dbServer.AddInParameter(command, "PasswordExpirationInterval", DbType.Int32, objVO.UserGeneralDetailVO.PasswordExpirationInterval);
                dbServer.AddInParameter(command, "UserType", DbType.Int32, objVO.UserType);

                dbServer.AddInParameter(command, "Status", DbType.Boolean, objVO.UserGeneralDetailVO.Status);
                dbServer.AddInParameter(command, "RoleId", DbType.Int64, objVO.UserGeneralDetailVO.RoleDetails.ID);
                // dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, int.MaxValue);

                // For Password Configuration.

                dbServer.AddInParameter(command, "MinPasswordLength", DbType.Int16, objVO.PassConfig.MinPasswordLength);
                dbServer.AddInParameter(command, "MaxPasswordLength", DbType.Int16, objVO.PassConfig.MaxPasswordLength);
                dbServer.AddInParameter(command, "AtLeastOneDigit", DbType.Boolean, objVO.PassConfig.AtLeastOneDigit);
                dbServer.AddInParameter(command, "AtLeastOneLowerCaseChar", DbType.Boolean, objVO.PassConfig.AtLeastOneLowerCaseChar);
                dbServer.AddInParameter(command, "AtLeastOneUpperCaseChar", DbType.Boolean, objVO.PassConfig.AtLeastOneUpperCaseChar);
                dbServer.AddInParameter(command, "AtLeastOneSpecialChar", DbType.Boolean, objVO.PassConfig.AtLeastOneSpecialChar);
                dbServer.AddInParameter(command, "NoOfPasswordsToRemember", DbType.Int16, objVO.PassConfig.NoOfPasswordsToRemember);
                dbServer.AddInParameter(command, "MinPasswordAge", DbType.Int16, objVO.PassConfig.MinPasswordAge);
                dbServer.AddInParameter(command, "MaxPasswordAge", DbType.Int16, objVO.PassConfig.MaxPasswordAge);
                dbServer.AddInParameter(command, "AccountLockThreshold", DbType.Int16, objVO.PassConfig.AccountLockThreshold);
                dbServer.AddInParameter(command, "AccountLockDuration", DbType.Double, objVO.PassConfig.AccountLockDuration);

                // Password Configuration.
                //dbServer.AddOutParameter(command, "RoleId", DbType.Int64, int.MaxValue);

                StringBuilder MenuRightsIdList = new StringBuilder();
                StringBuilder MenuRightsStatusList = new StringBuilder();
                //StringBuilder CreateList = new StringBuilder();
                //StringBuilder UpdateList = new StringBuilder();
                //StringBuilder DeleteList = new StringBuilder();
                //StringBuilder ReadList = new StringBuilder();
                //StringBuilder PrintList = new StringBuilder();

                StringBuilder DashboardIdList = new StringBuilder();
                StringBuilder DashboardStatusList = new StringBuilder();


                clsUserRoleVO varRole = objVO.UserGeneralDetailVO.RoleDetails;

                for (int MenuCount = 0; MenuCount < varRole.MenuList.Count; MenuCount++)
                {


                    clsMenuVO varMenu = varRole.MenuList[MenuCount];
                    MenuRightsIdList.Append(varMenu.ID);
                    MenuRightsStatusList.Append(varMenu.Status);
                    //CreateList.Append(varMenu.IsCreate);
                    //UpdateList.Append(varMenu.IsUpdate);
                    //DeleteList.Append(varMenu.IsDelete);
                    //ReadList.Append(varMenu.IsRead);
                    //PrintList.Append(varMenu.IsPrint);

                    if (MenuCount < (varRole.MenuList.Count - 1))
                    {
                        MenuRightsIdList.Append(",");
                        MenuRightsStatusList.Append(",");
                        //CreateList.Append(",");
                        //UpdateList.Append(",");
                        //DeleteList.Append(",");
                        //ReadList.Append(",");
                        //PrintList.Append(",");
                    }

                }

                for (int DashboardCnt = 0; DashboardCnt < varRole.DashBoardList.Count; DashboardCnt++)
                {
                    clsDashBoardVO varDashboard = varRole.DashBoardList[DashboardCnt];
                    DashboardIdList.Append(varDashboard.ID);
                    DashboardStatusList.Append(varDashboard.Status);

                    if (DashboardCnt < (varRole.DashBoardList.Count - 1))
                    {
                        DashboardIdList.Append(",");
                        DashboardStatusList.Append(",");

                    }
                }

                dbServer.AddInParameter(command, "MenuIdList", DbType.String, MenuRightsIdList.ToString());
                dbServer.AddInParameter(command, "MenuStatusList", DbType.String, MenuRightsStatusList.ToString());
                //dbServer.AddInParameter(command, "CreateList", DbType.String, CreateList.ToString());
                //dbServer.AddInParameter(command, "UpdateList", DbType.String, UpdateList.ToString());
                //dbServer.AddInParameter(command, "DeleteList", DbType.String, DeleteList.ToString());
                //dbServer.AddInParameter(command, "ReadList", DbType.String, ReadList.ToString());
                //dbServer.AddInParameter(command, "PrintList", DbType.String, PrintList.ToString());
                dbServer.AddInParameter(command, "DashboardIdList", DbType.String, DashboardIdList.ToString());
                dbServer.AddInParameter(command, "DashboardStatusList", DbType.String, DashboardStatusList.ToString());

                StringBuilder UnitIdList = new StringBuilder();
                StringBuilder UnitStatusList = new StringBuilder();
                StringBuilder UnitIsDefaultList = new StringBuilder();


                for (int UnitCount = 0; UnitCount < objVO.UserGeneralDetailVO.UnitDetails.Count; UnitCount++)
                {
                    UnitIdList.Append(objVO.UserGeneralDetailVO.UnitDetails[UnitCount].UnitID);
                    UnitStatusList.Append(objVO.UserGeneralDetailVO.UnitDetails[UnitCount].Status);
                    UnitIsDefaultList.Append(objVO.UserGeneralDetailVO.UnitDetails[UnitCount].IsDefault);

                    if (UnitCount < (objVO.UserGeneralDetailVO.UnitDetails.Count - 1))
                    {
                        UnitIdList.Append(",");
                        UnitStatusList.Append(",");
                        UnitIsDefaultList.Append(",");

                    }
                }

                dbServer.AddInParameter(command, "UnitIdList", DbType.String, UnitIdList.ToString());
                dbServer.AddInParameter(command, "UnitStatusList", DbType.String, UnitStatusList.ToString());
                dbServer.AddInParameter(command, "UnitIsDefaultList", DbType.String, UnitIsDefaultList.ToString());

                // StringBuilder UserIdList= new StringBuilder();
                StringBuilder UnitStoreIdList = new StringBuilder();
                StringBuilder StoreIdList = new StringBuilder();
                StringBuilder StatusList = new StringBuilder();
                bool flag = false;
                for (int UnitCount = 0; UnitCount < objVO.UserGeneralDetailVO.UnitDetails.Count; UnitCount++)
                {
                    for (int store = 0; store < objVO.UserGeneralDetailVO.UnitDetails[UnitCount].StoreDetails.Count; store++)
                    {
                        //if (objVO.UserGeneralDetailVO.UnitDetails[UnitCount].StoreDetails[store].StoreStatus == true)
                        //{
                        //UserIdList.Append(objVO.ID); 
                        if (flag == true)
                        {
                            UnitStoreIdList.Append(",");
                            StoreIdList.Append(",");
                            StatusList.Append(",");
                        }
                        flag = true;
                        UnitStoreIdList.Append(objVO.UserGeneralDetailVO.UnitDetails[UnitCount].StoreDetails[store].UnitId);
                        StoreIdList.Append(objVO.UserGeneralDetailVO.UnitDetails[UnitCount].StoreDetails[store].ID);
                        StatusList.Append(objVO.UserGeneralDetailVO.UnitDetails[UnitCount].StoreDetails[store].StoreStatus);

                        //if (store < (objVO.UserGeneralDetailVO.UnitDetails[UnitCount].StoreDetails.Count - 1))
                        //{
                        //    //UserIdList.Append(",");
                        //    UnitStoreIdList.Append(",");
                        //    StoreIdList.Append(",");
                        //    StatusList.Append(",");
                        //}
                        // break;
                        //   }

                    }
                }
                //for (int i = 0; i < objVO.UserGeneralDetailVO.StoreDetails.Count; i++)
                //{
                //    UserIdList.Append(objVO.ID);
                //    UnitStoreIdList.Append(objVO.UserGeneralDetailVO.StoreDetails[i].UnitId);
                //    StoreIdList.Append(objVO.UserGeneralDetailVO.StoreDetails[i].ID);
                //    StatusList.Append(objVO.UserGeneralDetailVO.StoreDetails[i].status);
                //    if (i < (objVO.UserGeneralDetailVO.StoreDetails.Count - 1))
                //    {
                //        UserIdList.Append(",");
                //        UnitStoreIdList.Append(",");
                //        StoreIdList.Append(",");
                //        StatusList.Append(",");

                //    }
                //}

                // dbServer.AddInParameter(command, "UserIdList", DbType.String, UserIdList.ToString());
                dbServer.AddInParameter(command, "UnitStoreIdList", DbType.String, UnitStoreIdList.ToString());
                dbServer.AddInParameter(command, "StoreIdList", DbType.String, StoreIdList.ToString());
                dbServer.AddInParameter(command, "StatusList", DbType.String, StatusList.ToString());
            }
            catch (Exception e)
            {
            }
        }

        public override IValueObject UpdateUserLockedStatus(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsUpdateUserLockedStatusBizActionVO bizObject = valueObject as clsUpdateUserLockedStatusBizActionVO;
            try 
	        {	        
		        clsUserVO objVO = bizObject.UserLockedStatus;
                DbCommand command = null;

                command = dbServer.GetStoredProcCommand("CIMS_UpdateUserLockedStatus");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objVO.ID);                
                dbServer.AddInParameter(command, "Locked", DbType.Boolean, objVO.UserGeneralDetailVO.Locked);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
              
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                bizObject.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                bizObject.UserLockedStatus.ID = (long)dbServer.GetParameterValue(command, "ID"); 

        	}
	        catch (Exception ex)
	        {		
	        //	throw;
	        }
            finally
            {
                //log error  
                //  logManager.LogInfo(bizObject.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return bizObject;
        }

        public override IValueObject ResetPassword(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsResetPasswordBizActionVO bizActionObj = valueObject as clsResetPasswordBizActionVO;

            try 
	        {	        
		        clsUserVO objVO = bizActionObj.RPassword;
                objVO.UserGeneralDetailVO.FirstPasswordChanged=false;
                DbCommand command = null;

                command = dbServer.GetStoredProcCommand("CIMS_ResetPassword");
              //  dbServer.AddInParameter(command, "ID", DbType.Int64, bizObject.ID); 
                dbServer.AddInParameter(command, "ID", DbType.Int64, objVO.ID);
                //dbServer.AddInParameter(command, "Status", DbType.Boolean, objVO.Status);
                dbServer.AddInParameter(command, "Password", DbType.String, Security.base64Encode(objVO.Password));  //newSecurity.EncryptDecryptUserKey(objVO.Password));
                dbServer.AddInParameter(command, "FirstPasswordChanged", DbType.Boolean, objVO.UserGeneralDetailVO.FirstPasswordChanged);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
              

               dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
               
                int intStatus = dbServer.ExecuteNonQuery(command);
                bizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                bizActionObj.RPassword.ID = (long)dbServer.GetParameterValue(command, "ID"); 
	        }
	        catch (Exception ex)
	        {		
		        throw;
	        }

            return bizActionObj;
                //throw new NotImplementedException();
        }

        public override IValueObject UpdateUserStatus(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsUpdateUserStatusBizActionVO bizObject = valueObject as clsUpdateUserStatusBizActionVO;
            try
            {
                clsUserVO objVO = bizObject.UserStatus;
                DbCommand command = null;

                command = dbServer.GetStoredProcCommand("CIMS_UpdateUserStatus");
              //  dbServer.AddInParameter(command, "ID", DbType.Int64, bizObject.ID); 
                dbServer.AddInParameter(command, "ID", DbType.Int64, objVO.ID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objVO.Status);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
              

               dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
               
                int intStatus = dbServer.ExecuteNonQuery(command);
                bizObject.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                bizObject.UserStatus.ID = (long)dbServer.GetParameterValue(command, "ID"); 

            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                // logManager.LogError(bizObject.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //  logManager.LogInfo(bizObject.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return bizObject;
            //throw new NotImplementedException();
        }

        public override IValueObject CheckUserExists(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetExistingUserNameBizActionVO BizActionObj = valueObject as clsGetExistingUserNameBizActionVO;
            try
            {
                clsUserVO objVO = BizActionObj.UserName;
                DbCommand command=dbServer.GetStoredProcCommand("CIMS_GetUserExists");
                if(objVO.UserGeneralDetailVO.IsDoctor==true)
                {
                     dbServer.AddInParameter(command, "LoginId", DbType.Int64, objVO.UserGeneralDetailVO.DoctorID);                                       
                }
                else if (objVO.UserGeneralDetailVO.IsEmployee==true)
                {
                    dbServer.AddInParameter(command, "LoginId", DbType.Int64, objVO.UserGeneralDetailVO.EmployeeID);                                                       
                }
                else if (objVO.UserGeneralDetailVO.IsPatient==true)
                {
                    dbServer.AddInParameter(command, "LoginId", DbType.Int64, objVO.UserGeneralDetailVO.PatientID);                                                      
                }
                
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");                           
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                // logManager.LogError(bizObject.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //  logManager.LogInfo(bizObject.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return BizActionObj;             
        }

        public override IValueObject CheckUserLoginExists(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            //throw new NotImplementedException();
            clsGetLoginNameBizActionVO BizActionObjLogin = valueObject as clsGetLoginNameBizActionVO;
            try
            {
                //clsUserVO objVO = BizActionObjLogin.LoginName;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetUserName");
                dbServer.AddInParameter(command, "LoginName", DbType.String,BizActionObjLogin.LoginName);
                //dbServer.AddInParameter(command, "LoginName", DbType.String, Security.base64Encode(objVO.LoginName));
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObjLogin.SuccessStatus=(int)dbServer.GetParameterValue(command, "ResultStatus");              
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                // logManager.LogError(bizObject.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //  logManager.LogInfo(bizObject.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return BizActionObjLogin;
        }
               
        public override IValueObject AddUser(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsAddUserBizActionVO bizObject = valueObject as clsAddUserBizActionVO;
            try
            {
                clsUserVO objVO = bizObject.Details;
                DbCommand command = null;
                if (objVO.ID == 0)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_AddUser");
                    dbServer.AddOutParameter(command, "ID", DbType.Int64, int.MaxValue);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    if(objVO.UserGeneralDetailVO.IsPatient==true)
                        dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("CIMS_UpdateUser");
                    dbServer.AddInParameter(command, "ID", DbType.Int64, objVO.ID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                }

                AddCommonParametersForAddUpdateMethods(command, objVO, objUserVO);

                int intStatus = dbServer.ExecuteNonQuery(command);
                bizObject.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
             //   bizObject.Details.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                // logManager.LogError(bizObject.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
               // throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //  logManager.LogInfo(bizObject.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return bizObject;
        }

        public override IValueObject GetSecretQtn(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsSecretQtnBizActionVO bizObjectDetails = valueObject as clsSecretQtnBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetSecretQtnList");
                DbDataReader reader;

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (bizObjectDetails.Details == null)
                        bizObjectDetails.Details = new List<clsSecretQtnVO>();
                    while (reader.Read())
                    {
                        clsSecretQtnVO objSecretQtn = new clsSecretQtnVO();
                        objSecretQtn.Id = (long)DALHelper.HandleDBNull(reader["Id"]);
                        objSecretQtn.Value = (string)DALHelper.HandleDBNull(reader["Description"]);
                        bizObjectDetails.Details.Add(objSecretQtn);
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {                
             //   throw;
            }
            return bizObjectDetails;
        }

        public override IValueObject GetUnitStoreStatusList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetUnitStoreStatusBizActionVO bizObjectDetails = valueObject as clsGetUnitStoreStatusBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetUnitStoreStatusList");
                DbDataReader reader;

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, bizObjectDetails.ID);
                dbServer.AddInParameter(command, "UserId", DbType.Int64, bizObjectDetails.UserId);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (bizObjectDetails.Details == null)
                        bizObjectDetails.Details = new List<clsItemStoreVO>();
                    while (reader.Read())
                    {
                        //chkFlag = true;
                        clsItemStoreVO objStoreStatusVO = new clsItemStoreVO();
                        objStoreStatusVO.ID = (long)DALHelper.HandleDBNull(reader["StoreId"]);
                        objStoreStatusVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitId"]);
                        objStoreStatusVO.UserStoreId = (long)DALHelper.HandleDBNull(reader["Id"]);
                        objStoreStatusVO.StoreName = (string)DALHelper.HandleDBNull(reader["StoreDesc"]);
                        objStoreStatusVO.StoreStatus = reader["UserStoreStatus"].HandleDBNull() == null ? false : Convert.ToBoolean(reader["UserStoreStatus"]);
                        bizObjectDetails.Details.Add(objStoreStatusVO);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {                
             //   throw;
            }

            return bizObjectDetails;
        }
       
        public override IValueObject GetUnitStoreList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetUnitStoreBizActionVO bizObject = valueObject as clsGetUnitStoreBizActionVO;
                      
            //bool chkFlag = false;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetUnitStoreList");
                DbDataReader reader;

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, bizObject.ID);
               // dbServer.AddInParameter(command, "UserId", DbType.Int64, bizObject.UserId);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (bizObject.Details == null)
                        bizObject.Details = new List<clsItemStoreVO>();
                    while (reader.Read())
                    {
                        clsItemStoreVO objStoreUnitVO = new clsItemStoreVO();
                        objStoreUnitVO.ID = (long)DALHelper.HandleDBNull(reader["StoreId"]);
                        objStoreUnitVO.StoreName = (string)DALHelper.HandleDBNull(reader["StoreDesc"]);                      
                        //objStoreUnitVO.StoreStatus = reader["UserStoreStatus"].HandleDBNull() == null ? false : Convert.ToBoolean(reader["UserStoreStatus"]); 
                            //(bool)DALHelper.HandleDBNull(reader["UserStoreStatus"]);
                      
                        bizObject.Details.Add(objStoreUnitVO);
                    }
                }
                reader.Close();              
            }
            catch (Exception ex)
            {                
             //   throw;
            }
                return bizObject;           
        }
        
        public override IValueObject GetUserList(IValueObject valueObject, clsUserVO objUserVO)
        {
           
            clsGetUserListBizActionVO bizObject = valueObject as clsGetUserListBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetUserListForSearch");
                DbDataReader reader;             

                 dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, bizObject.IsPagingEnabled);
                 dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, bizObject.StartRowIndex);
                 dbServer.AddInParameter(command, "maximumRows", DbType.Int32, bizObject.MaximumRows);
                 dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
            //     dbServer.AddInParameter(command, "SearchExpression", DbType.String, Security.base64Encode(bizObject.SearchExpression));
                 dbServer.AddInParameter(command, "SearchExpression", DbType.String,bizObject.SearchExpression);
            // Added By Umesh
                 dbServer.AddInParameter(command, "UserRoleID", DbType.String, bizObject.UserRoleID);
                 dbServer.AddInParameter(command, "IsDoctor", DbType.String, bizObject.IsDoctor);
                 dbServer.AddInParameter(command, "IsEmployee", DbType.String, bizObject.IsEmployee);
                 dbServer.AddInParameter(command, "IsPatient", DbType.String, bizObject.IsPatient);
           //      dbServer.AddInParameter(command, "IsActive", DbType.String, bizObject.IsActive);
                 dbServer.AddInParameter(command, "IsDeActive", DbType.String, bizObject.IsDeActive);

                 dbServer.AddInParameter(command, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                 dbServer.AddInParameter(command, "UserId", DbType.Int64, objUserVO.ID); 
                
                 dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, bizObject.TotalRows);
             
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (bizObject.Details == null)
                        bizObject.Details = new List<clsUserVO>();

                    while (reader.Read())
                    {
                        clsUserVO objPatientVO = new clsUserVO();

                        objPatientVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);

                        objPatientVO.UserGeneralDetailVO.UserName = (string)DALHelper.HandleDBNull(reader["UserName"]);
                       
                        objPatientVO.LoginName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LoginName"]));
                        objPatientVO.UserLoginInfo.Name = objPatientVO.LoginName;
                        objPatientVO.UserGeneralDetailVO.Locked = (bool)DALHelper.HandleDBNull(reader["Locked"]);
                        objPatientVO.UserGeneralDetailVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        objPatientVO.UserTypeName = (string)DALHelper.HandleDBNull(reader["UserType"]);
                        objPatientVO.UserType = (Int64)DALHelper.HandleDBNull(reader["UserTypeID"]);
                        objPatientVO.RoleName = (string)DALHelper.HandleDBNull(reader["Role"]);
                        objPatientVO.EmailId = (string)DALHelper.HandleDBNull(reader["EmailId"]);
                        bizObject.Details.Add(objPatientVO);
                    }

                }
                reader.NextResult();
                bizObject.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }


            return bizObject;
        }

        public override IValueObject UpdateUserDashBoard(IValueObject valueObj, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsUpdateDashboardBizActionVO bizObject = valueObj as clsUpdateDashboardBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateDashBoard");
                dbServer.AddInParameter(command, "UserID", DbType.Int64, bizObject.ID); // .UserId);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
               
                StringBuilder DashboardIdList = new StringBuilder();
                StringBuilder DashboardStatusList = new StringBuilder();

                //  clsUserVO varDashBoard = BizActionObj.DashBoardList;
                //clsUserRoleVO varRole = BizActionObj.Details.UserGeneralDetailVO.RoleDetails;
                //clsDashBoardVO varDashboard = BizActionObj.DashBoardList;
                int count = bizObject.DashBoardList.Count;

                //for (int DashboardCnt = 0; DashboardCnt < varRole.DashBoardList.Count; DashboardCnt++)
                for (int DashboardCnt = 0; DashboardCnt < count; DashboardCnt++)
                {
                    clsDashBoardVO varDashboard = bizObject.DashBoardList[DashboardCnt]; //varRole.DashBoardList[DashboardCnt];
                    DashboardIdList.Append(varDashboard.ID);
                    DashboardStatusList.Append(varDashboard.Status);

                    if (DashboardCnt < (count - 1))
                    {
                        DashboardIdList.Append(",");
                        DashboardStatusList.Append(",");
                    }
                }
                dbServer.AddInParameter(command, "DashboardIdList", DbType.String, DashboardIdList.ToString());
                dbServer.AddInParameter(command, "DashboardStatusList", DbType.String, DashboardStatusList.ToString());

                int intStatus = dbServer.ExecuteNonQuery(command);
                bizObject.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
            }
            catch (Exception)
            {
                CurrentMethodExecutionStatus = false;
            }

            return bizObject;
        }

        public override IValueObject GetLoginNamePassword(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetLoginNamePasswordBizActionVO BizActiobObj = valueObject as clsGetLoginNamePasswordBizActionVO;
            try 
	        {
                clsUserVO objVO = BizActiobObj.LoginDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetLoginPassword");
                DbDataReader reader;
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActiobObj.ID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (BizActiobObj.LoginDetails == null)
                            BizActiobObj.LoginDetails = new clsUserVO();

                        BizActiobObj.LoginDetails.LoginName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LoginName"]));
                        BizActiobObj.LoginDetails.UserLoginInfo.Name = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LoginName"]));

                        BizActiobObj.LoginDetails.UserGeneralDetailVO.FirstPasswordChanged = (bool)DALHelper.HandleDBNull(reader["FirstPasswordChanged"]);
                        
                        BizActiobObj.DashBoardList.Add (new clsDashBoardVO()                      
                        {
                            ID = (long)DALHelper.HandleDBNull(reader["DashBoardID"]),
                            Status = (bool)DALHelper.HandleDBNull(reader["CurrentStatus"]),
                            Description = (string)DALHelper.HandleDBNull(reader["DashBoardDescription"]),
                            Active=(bool)DALHelper.HandleDBNull(reader["Active"])
                        });
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

            return BizActiobObj;
        
        }
        
        public override IValueObject GetUser(IValueObject valueObject, clsUserVO objUserVO)
        {
            //throw new NotImplementedException();
            clsGetUserBizActionVO BizActionObj = valueObject as clsGetUserBizActionVO;
            // clsAddPatientBizActionVO BizActionObj = ValueObject as clsAddPatientBizActionVO;
            try
            {
                clsUserVO objVO = BizActionObj.Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetUser");
                // DataSysDescriptionAttribute 
                DbDataReader reader;
                if (BizActionObj.ID != null && BizActionObj.ID != 0)
                {
                    dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                }
                else         
                    dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.Details.ID);

                dbServer.AddInParameter(command, "FlagDisableUser", DbType.Int64, BizActionObj.FlagDisableUser);
                dbServer.AddInParameter(command, "UserType", DbType.Int64, BizActionObj.UserType);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);

                // int intStatus = dbServer.ExecuteNonQuery(command);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (BizActionObj.Details == null)
                            BizActionObj.Details = new clsUserVO();

                        BizActionObj.Details.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        BizActionObj.Details.LoginName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LoginName"]));
                        BizActionObj.Details.UserLoginInfo.Name = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LoginName"]));

                        //Password.
                        BizActionObj.Details.Password = Security.base64Decode((string)DALHelper.HandleDBNull(reader["Password"])); // newSecurity.EncryptDecryptUserKey((string)DALHelper.HandleDBNull(reader["Password"]), true); //

                        BizActionObj.Details.UserGeneralDetailVO.IsEmployee = (bool)DALHelper.HandleDBNull(reader["IsEmployee"]);
                        BizActionObj.Details.UserGeneralDetailVO.IsDoctor = (bool)DALHelper.HandleDBNull(reader["IsDoctor"]);
                        BizActionObj.Details.UserGeneralDetailVO.IsPatient = (bool)DALHelper.HandleDBNull(reader["IsPatient"]);
                        BizActionObj.Details.UserGeneralDetailVO.EmployeeID = (long)DALHelper.HandleDBNull(reader["EmployeeID"]);
                        BizActionObj.Details.UserGeneralDetailVO.DoctorID = (long)DALHelper.HandleDBNull(reader["DoctorID"]);
                        BizActionObj.Details.UserGeneralDetailVO.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                       // BizActionObj.Details.Password = Security.base64Decode((string)DALHelper.HandleDBNull(reader["Password"]));
                       // BizActionObj.Details.UserGeneralDetailVO.SecretQtnValue = (string)DALHelper.HandleDBNull(reader["SecreteQtn"]);
                        BizActionObj.Details.UserGeneralDetailVO.SecreteQtn = (long)DALHelper.HandleDBNull(reader["SecreteQtn"]);
                        
                     
                        if ((string)DALHelper.HandleDBNull(reader["SecreteAns"]) != null)
                            BizActionObj.Details.UserGeneralDetailVO.SecreteAns = newSecurity.EncryptDecryptUserKey((string)DALHelper.HandleDBNull(reader["SecreteAns"]), true); //Security.base64Decode((string)DALHelper.HandleDBNull(reader["SecreteAns"]));

                        BizActionObj.Details.UserGeneralDetailVO.Locked = (bool)DALHelper.HandleDBNull(reader["Locked"]);
                        BizActionObj.Details.UserGeneralDetailVO.EnablePasswordExpiration = (bool)DALHelper.HandleDBNull(reader["EnablePasswordExpiration"]);

                        BizActionObj.Details.UserGeneralDetailVO.PasswordExpirationInterval = (int)DALHelper.HandleDBNull(reader["PasswordExpirationInterval"]);
                        BizActionObj.Details.UserGeneralDetailVO.FirstPasswordChanged = (bool)DALHelper.HandleDBNull(reader["FirstPasswordChanged"]);
                        BizActionObj.Details.UserType = (int)DALHelper.HandleDBNull(reader["UserType"]);

                        // For Password Configuration.
                        BizActionObj.Details.PassConfig.MinPasswordLength = (Int16)DALHelper.HandleDBNull(reader["MinPasswordLength"]);
                        BizActionObj.Details.PassConfig.MaxPasswordLength = (Int16)DALHelper.HandleDBNull(reader["MaxPasswordLength"]);
                        BizActionObj.Details.PassConfig.MinPasswordAge = (Int16)DALHelper.HandleDBNull(reader["MinPasswordAge"]);
                        BizActionObj.Details.PassConfig.MaxPasswordAge = (Int16)DALHelper.HandleDBNull(reader["MaxPasswordAge"]);
                        BizActionObj.Details.PassConfig.NoOfPasswordsToRemember = (Int16)DALHelper.HandleDBNull(reader["NoOfPasswordsToRemember"]);
                        BizActionObj.Details.PassConfig.AtLeastOneDigit = (bool)DALHelper.HandleDBNull(reader["AtLeastOneDigit"]);
                        BizActionObj.Details.PassConfig.AtLeastOneLowerCaseChar = (bool)DALHelper.HandleDBNull(reader["AtleastOneLowerCaseChar"]);
                        BizActionObj.Details.PassConfig.AtLeastOneUpperCaseChar = (bool)DALHelper.HandleDBNull(reader["AtLeastOneUpperCaseChar"]);
                        BizActionObj.Details.PassConfig.AtLeastOneSpecialChar = (bool)DALHelper.HandleDBNull(reader["AtLeastOneSpecialChar"]);
                        BizActionObj.Details.PassConfig.AccountLockThreshold = (Int16)DALHelper.HandleDBNull(reader["AccountLockThreshold"]);
                        BizActionObj.Details.PassConfig.AccountLockDuration = (float)(double)DALHelper.HandleDBNull(reader["AccountLockDuration"]);

                        //End Password Configuration.

                        BizActionObj.Details.UserGeneralDetailVO.RoleDetails = new clsUserRoleVO();
                        BizActionObj.Details.UserGeneralDetailVO.RoleDetails.ID = (long)DALHelper.HandleDBNull(reader["RoleID"]);

                        BizActionObj.Details.UserGeneralDetailVO.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                        BizActionObj.Details.UserGeneralDetailVO.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                        BizActionObj.Details.UserGeneralDetailVO.AddedBy = (Int64?)DALHelper.HandleDBNull(reader["AddedBy"]);
                        BizActionObj.Details.UserGeneralDetailVO.AddedOn = (String)DALHelper.HandleDBNull(reader["AddedOn"]);
                        BizActionObj.Details.UserGeneralDetailVO.AddedDateTime = (DateTime?)DALHelper.HandleDate(reader["AddedDateTime"]);

                        BizActionObj.Details.UserGeneralDetailVO.UpdatedBy = (Int64?)DALHelper.HandleDBNull(reader["UpdatedBy"]);
                        BizActionObj.Details.UserGeneralDetailVO.UpdatedOn = (String)DALHelper.HandleDBNull(reader["UpdatedOn"]);
                        BizActionObj.Details.UserGeneralDetailVO.UpdatedDateTime = (DateTime?)DALHelper.HandleDate(reader["UpdatedDateTime"]);


                        string strFirstName, strMiddleName, strLastName;

                        switch (BizActionObj.Details.UserType)
                        {
                            //case 1: //'Doctor'
                            //    strFirstName =  (string)DALHelper.HandleDBNull(reader["UserFirstName"]);
                            //    strMiddleName = (string)DALHelper.HandleDBNull(reader["UserMiddleName"]);
                            //    strLastName = (string)DALHelper.HandleDBNull(reader["UserLastName"]);
                            //    break;

                            //case 2: //Employee
                            //    strFirstName = (string)DALHelper.HandleDBNull(reader["UserFirstName"]);
                            //    strMiddleName = (string)DALHelper.HandleDBNull(reader["UserMiddleName"]);
                            //    strLastName = (string)DALHelper.HandleDBNull(reader["UserLastName"]);
                            //    break;

                            case 3: // Patient
                                strFirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["UserFirstName"]));
                                strMiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["UserMiddleName"]));
                                strLastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["UserLastName"]));
                                break;

                            default:

                                strFirstName = (string)DALHelper.HandleDBNull(reader["UserFirstName"]);
                                strMiddleName = (string)DALHelper.HandleDBNull(reader["UserMiddleName"]);
                                strLastName = (string)DALHelper.HandleDBNull(reader["UserLastName"]);
                                break;
                        }

                        if (strMiddleName != null)
                            BizActionObj.Details.UserGeneralDetailVO.UserName = strFirstName + " " + strMiddleName + " " + strLastName;
                        else
                            BizActionObj.Details.UserGeneralDetailVO.UserName = strFirstName + " " + strLastName;
                        }
                       }

                        reader.NextResult();
                        if (reader.HasRows)
                        {
                            BizActionObj.Details.UserGeneralDetailVO.UnitDetails = new List<clsUserUnitDetailsVO>();
                            while (reader.Read())
                            {
                                clsUserUnitDetailsVO objUnit = new clsUserUnitDetailsVO();

                                objUnit.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                                //reader["UserStoreStatus"].HandleDBNull() == null ? false : Convert.ToBoolean(reader["UserStoreStatus"]); 
                                //objUnit.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);

                                objUnit.UnitID = reader["UnitID"].HandleDBNull() == null ? 0 : Convert.ToInt64(reader["UnitID"]);
                                objUnit.Status = reader["Status"].HandleDBNull() == null ? false : Convert.ToBoolean(reader["Status"]); //(bool)DALHelper.HandleDBNull(reader["Status"]);
                                objUnit.IsDefault = reader["IsDefault"].HandleDBNull() == null ? false : Convert.ToBoolean(reader["IsDefault"]); //(bool)DALHelper.HandleDBNull(reader["IsDefault"]);

                                BizActionObj.Details.UserGeneralDetailVO.UnitDetails.Add(objUnit);
                            }
                        }

                        reader.NextResult();
                        if (reader.HasRows)
                        {
                            for (int count = 0; count < BizActionObj.Details.UserGeneralDetailVO.UnitDetails.Count; count++)
                            {
                                BizActionObj.Details.UserGeneralDetailVO.UnitDetails[count].StoreDetails = new List<clsItemStoreVO>();
                                //if (BizActionObj.Details.UserGeneralDetailVO.UnitDetails[count].StoreDetails.Count > 0)
                                //{
                                //for (int StoreCount = 0; StoreCount < BizActionObj.Details.UserGeneralDetailVO.UnitDetails[count].StoreDetails.Count; StoreCount++)
                                //{
                                    //BizActionObj.Details.UserGeneralDetailVO.UnitDetails[count].StoreDetails[StoreCount].StoreList = new List<clsItemStoreVO>();
                                while (reader.Read())
                                {
                                    clsItemStoreVO objUserStore = new clsItemStoreVO();
                                    objUserStore.UserStoreId = (long)DALHelper.HandleDBNull(reader["Id"]);
                                    objUserStore.StoreName = (string)DALHelper.HandleDBNull(reader["StoreName"]);
                                    objUserStore.UnitId = (long)DALHelper.HandleDBNull(reader["ClinicId"]);
                                    objUserStore.ID = (long)DALHelper.HandleDBNull(reader["StoreId"]);
                                    objUserStore.StoreStatus = (bool)DALHelper.HandleDBNull(reader["StoreStatus"]);

                                    BizActionObj.Details.UserGeneralDetailVO.UnitDetails[count].StoreDetails.Add(objUserStore);
                                
                                }
                               // }
                                // }
                            }
                        }

                        clsUserRoleVO obj;
                        obj = BizActionObj.Details.UserGeneralDetailVO.RoleDetails;
                        reader.NextResult();
                        if (reader.HasRows)
                        {
                            obj.MenuList = new List<clsMenuVO>();
                            while (reader.Read())
                            {
                                //obj.RoleDetails.RoleMenuRights.Add(new RoleMenuRightsItem(Convert.ToInt64(reader["MenuId"]), Convert.ToBoolean(reader["Status"])));

                                clsMenuVO objMenu = new clsMenuVO();
                                objMenu.ID = (long)DALHelper.HandleDBNull(reader["MenuID"]);
                                objMenu.Title = (string)DALHelper.HandleDBNull(reader["Title"]);
                                objMenu.ImagePath = (string)DALHelper.HandleDBNull(reader["ImagePath"]);
                                objMenu.Parent = (string)DALHelper.HandleDBNull(reader["Parent"]);
                                objMenu.ParentId = (long)DALHelper.HandleDBNull(reader["ParentID"]);
                                objMenu.Module = (string)DALHelper.HandleDBNull(reader["Module"]);
                                objMenu.Action = (string)DALHelper.HandleDBNull(reader["Action"]);
                                objMenu.Header = (string)DALHelper.HandleDBNull(reader["Header"]);
                                objMenu.Configuration = (string)DALHelper.HandleDBNull(reader["Configuration"]);
                                objMenu.Mode = (string)DALHelper.HandleDBNull(reader["Mode"]);
                                // objMenu.Active = (bool)DALHelper.HandleDBNull(reader["Active"]);
                                objMenu.MenuOrder = (int)DALHelper.HandleDBNull(reader["MenuOrder"]);
                                objMenu.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                                //objMenu.IsPrint = (bool)DALHelper.HandleDBNull(reader["IsPrint"]);
                                //objMenu.IsUpdate = (bool)DALHelper.HandleDBNull(reader["IsUpdate"]);
                                //objMenu.IsRead = (bool)DALHelper.HandleDBNull(reader["IsRead"]);
                                //objMenu.IsDelete = (bool)DALHelper.HandleDBNull(reader["IsDelete"]);
                                //objMenu.IsCreate = (bool)DALHelper.HandleDBNull(reader["IsCreate"]);
                                BizActionObj.Details.UserGeneralDetailVO.RoleDetails.MenuList.Add(objMenu);

                            }
                        }

                        reader.NextResult();
                        if (reader.HasRows)
                        {
                             obj.DashBoardList = new List<clsDashBoardVO>();
                             while (reader.Read())
                             {
                                  obj.DashBoardList.Add(new clsDashBoardVO()
                                  {
                                       ID = (long)DALHelper.HandleDBNull(reader["DashBoardID"]),
                                       Status = (bool)DALHelper.HandleDBNull(reader["Status"])
                                  });

                              }
                        }
                        BizActionObj.Details.UserGeneralDetailVO.RoleDetails = obj;
                  //  }
                  
               // }
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

        public override IValueObject GetUserDashBoard(IValueObject valueObject, clsUserVO objUserVO)
        {
            //This is to get the dash board assigned to the respective user INCOMPLETE.
            clsGetUserDashBoardVO BizActionObj = valueObject as clsGetUserDashBoardVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetUserDashBoard");
                DbDataReader reader;
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);

                //int intStatus = dbServer.ExecuteNonQuery(command);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsDashBoardVO>();
                    while (reader.Read())
                    {
                        clsDashBoardVO objVO = new clsDashBoardVO();

                        objVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        objVO.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        objVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);

                        BizActionObj.List.Add(objVO);
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
            return valueObject;
        }

        public override IValueObject AssignUserEMRTemplate(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAssignUserEMRTemplatesBizActionVO bizObject = valueObject as clsAssignUserEMRTemplatesBizActionVO;
            try
            {


                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUserEMRTempalteDetails");
                dbServer.AddInParameter(command, "UserID", DbType.Int64, bizObject.UserID); // .UserId);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                StringBuilder IdList = new StringBuilder();
                StringBuilder StatusList = new StringBuilder();

               
                int count = bizObject.Details.Count;

                //for (int DashboardCnt = 0; DashboardCnt < varRole.DashBoardList.Count; DashboardCnt++)
                for (int DashboardCnt = 0; DashboardCnt < count; DashboardCnt++)
                {
                    clsUserEMRTemplateDetailsVO varDashboard = bizObject.Details[DashboardCnt]; //varRole.DashBoardList[DashboardCnt];
                    if (varDashboard.Status == true)
                    {
                        IdList.Append(varDashboard.TemplateID);
                        StatusList.Append(varDashboard.Status);

                       
                            IdList.Append(",");
                            StatusList.Append(",");
                        
                    }


                }

                IdList = IdList.Remove(IdList.Length - 1, 1);
                StatusList = StatusList.Remove(StatusList.Length - 1, 1);

                dbServer.AddInParameter(command, "EMRTemplateIDList", DbType.String, IdList.ToString());
                dbServer.AddInParameter(command, "StatusList", DbType.String, StatusList.ToString());

                int intStatus = dbServer.ExecuteNonQuery(command);
              //  bizObject.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");



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

        public override IValueObject GetUserEMRTemplateList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetUserEMRTemplateListBizActionVO BizActionObj = valueObject as clsGetUserEMRTemplateListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetUserEMRTemplate");
                DbDataReader reader;
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.UserID);

                
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsUserEMRTemplateDetailsVO>();
                    while (reader.Read())
                    {
                        clsUserEMRTemplateDetailsVO objVO = new clsUserEMRTemplateDetailsVO();
                                              
                        objVO.TemplateID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVO.TemplateName = (string)DALHelper.HandleDBNull(reader["Title"]);
                        objVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);

                        BizActionObj.List.Add(objVO);
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
            return valueObject;
        }

        public override IValueObject GetEMRMenu(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetEMRMenuBizActionVO BizAction = valueObject as clsGetEMRMenuBizActionVO;
            DbDataReader reader = null;
            BizAction.MenuDetails = new List<clsMenuVO>();
            DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetUserWiseEMRMenuList");
            try
            {
                dbServer.AddInParameter(command, "UserID", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                while (reader.Read())
                {
                    clsMenuVO objMenu = new clsMenuVO();
                    objMenu.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MenuID"]));
                    objMenu.Title = Convert.ToString(DALHelper.HandleDBNull(reader["Title"]));
                    objMenu.ImagePath = Convert.ToString(DALHelper.HandleDBNull(reader["ImagePath"]));
                    objMenu.Parent = Convert.ToString(DALHelper.HandleDBNull(reader["Parent"]));
                    objMenu.ParentId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ParentID"]));
                    objMenu.Module = Convert.ToString(DALHelper.HandleDBNull(reader["Module"]));
                    objMenu.Action = Convert.ToString(DALHelper.HandleDBNull(reader["Action"]));
                    objMenu.Header = Convert.ToString(DALHelper.HandleDBNull(reader["Header"]));
                    objMenu.Configuration = Convert.ToString(DALHelper.HandleDBNull(reader["Configuration"]));
                    objMenu.Mode = Convert.ToString(DALHelper.HandleDBNull(reader["Mode"]));
                    objMenu.MenuOrder = Convert.ToInt32(DALHelper.HandleDBNull(reader["MenuOrder"]));
                    objMenu.ISFemale = Convert.ToBoolean(DALHelper.HandleDBNull(reader["isfemale"]));
                    objMenu.Status = Convert.ToBoolean(reader["Status"]);
                    BizAction.MenuDetails.Add(objMenu);
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally { reader.Close(); }
            return BizAction;
        }


        public override IValueObject AddUserCategoryLink(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUserCategoryLinkBizActionVO BizActionObj = (clsAddUserCategoryLinkBizActionVO)valueObject;
            clsUserCategoryLinkVO objUserCatVO = BizActionObj.UserCategoryLinkDetails;
            DbCommand cmd = dbServer.GetStoredProcCommand("CIMS_DeleteUserCategoryLink");
            try
            {
                dbServer.AddInParameter(cmd, "UserID", DbType.Int64, objUserCatVO.UserID);
                int intStatus1 = dbServer.ExecuteNonQuery(cmd);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd.Connection.Dispose();
                }
            }
            if (BizActionObj.UserCategoryLinkList != null && BizActionObj.UserCategoryLinkList.Count > 0)
            {
                foreach (clsUserCategoryLinkVO item in BizActionObj.UserCategoryLinkList)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUserCategoryLinking");
                    try
                    {
                        dbServer.AddInParameter(command, "UserID", DbType.Int64, item.UserID);
                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, item.UnitId);
                        dbServer.AddInParameter(command, "CategoryID", DbType.Int64, item.ID);
                        dbServer.AddInParameter(command, "CategoryTypeID", DbType.Int64, item.CategoryTypeID);
                        dbServer.AddInParameter(command, "CategoryType", DbType.String, item.CategoryType);
                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                        dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                        int intStatus = dbServer.ExecuteNonQuery(command);
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
            return BizActionObj;
        }

        public override IValueObject GetCategoryList(IValueObject valueObject, clsUserVO objUserVO)
        {
            //throw new NotImplementedException();
            clsGetCategoryListBizActionVO BizActionObj = valueObject as clsGetCategoryListBizActionVO;
            clsUserCategoryLinkVO objUserCat = BizActionObj.CategoryListDetails;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_CategoryListForUserLinking");
                dbServer.AddInParameter(command, "CategoryName", DbType.String, objUserCat.CategoryName);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.CategoryList == null)
                        BizActionObj.CategoryList = new List<clsUserCategoryLinkVO>();
                    while (reader.Read())
                    {
                        clsUserCategoryLinkVO objCat = new clsUserCategoryLinkVO();
                        objCat.CategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objCat.CategoryType = Convert.ToString(DALHelper.HandleDBNull(reader["CategoryType"]));
                        objCat.CategoryTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryTypeID"]));
                        objCat.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objCat.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objCat.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objCat.AddedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["AddedBy"]));
                        objCat.UpdatedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["UpdatedBy"]));
                        objCat.AddedOn = Convert.ToString(DALHelper.HandleDBNull(reader["AddedOn"]));
                        objCat.UpdatedOn = Convert.ToString(DALHelper.HandleDBNull(reader["UpdatedOn"]));
                        objCat.AddedDateTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["AddedDateTime"]));
                        BizActionObj.CategoryList.Add(objCat);
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetExistingCategoryListForUser(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetExistingCategoryListBizActionVO BizActionObj = valueObject as clsGetExistingCategoryListBizActionVO;
            clsUserCategoryLinkVO objUserCat = BizActionObj.ExistingCategoryListDetails;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetExistingLinkedCategoryForUser");
                dbServer.AddInParameter(command, "UserID", DbType.Int64, objUserCat.UserID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ExistingCategoryList == null)
                        BizActionObj.ExistingCategoryList = new List<clsUserCategoryLinkVO>();
                    while (reader.Read())
                    {
                        clsUserCategoryLinkVO objCat = new clsUserCategoryLinkVO();
                        objCat.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objCat.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryID"]));
                        objCat.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryID"]));
                        objCat.CategoryTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryTypeID"]));
                        objCat.CategoryType = Convert.ToString(DALHelper.HandleDBNull(reader["CategoryType"]));
                        objCat.UserID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UserID"]));
                        objCat.CategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["CategoryName"]));
                        BizActionObj.ExistingCategoryList.Add(objCat);
                    }
                    reader.NextResult();
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

    }
}
