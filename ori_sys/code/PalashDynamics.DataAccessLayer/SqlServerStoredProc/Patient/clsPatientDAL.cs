using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Patient;
using System.Data.Common;
using System.Data;
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects.Billing;
using System.IO;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsPatientDAL : clsBasePatientDAL
    {

        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;

        //added by neena
        string ImgIP = string.Empty;
        string ImgVirtualDir = string.Empty;
        string ImgSaveLocation = string.Empty;
        //

        //***// For Document
        string DocImgIP = string.Empty;
        string DocImgVirtualDir = string.Empty;
        string DocSaveLocation = string.Empty;


        #endregion

        private clsPatientDAL()
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

                //added by neena
                ImgIP = System.Configuration.ConfigurationManager.AppSettings["RegImgIP"];
                ImgVirtualDir = System.Configuration.ConfigurationManager.AppSettings["RegImgVirtualDir"];
                ImgSaveLocation = System.Configuration.ConfigurationManager.AppSettings["ImgSavingLocation"];

                //***//
                DocImgIP = System.Configuration.ConfigurationManager.AppSettings["DocImgIP"];
                DocImgVirtualDir = System.Configuration.ConfigurationManager.AppSettings["DocImgVirtualDir"];
                DocSaveLocation = System.Configuration.ConfigurationManager.AppSettings["DocSavingLocation"];
                //

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override IValueObject AddPatient(IValueObject valueObject, clsUserVO UserVo)
        {

            clsAddPatientBizActionVO BizActionObj = valueObject as clsAddPatientBizActionVO;        //

            if (BizActionObj.PatientDetails.IsPanNoSave != true)
            {
                if (BizActionObj.PatientDetails.GeneralDetails.PatientID == 0)
                    BizActionObj = AddPatientDetails(BizActionObj, UserVo);
                else
                    BizActionObj = UpdatePatientDetails(BizActionObj, UserVo);
            }
            else
            {
                BizActionObj = UpdatePatientPanNumber(BizActionObj, UserVo); //Added by Ajit,Date 29/8/2016, Pan No Save
            }

            return valueObject;
        }

        private clsAddPatientBizActionVO AddPatientDetails(clsAddPatientBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = dbServer.CreateConnection();

            try
            {
                clsPatientVO objPatientVO = BizActionObj.PatientDetails;

                con.Open();
                trans = con.BeginTransaction();

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPatient");

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objPatientVO.GeneralDetails.LinkServer);
                if (objPatientVO.GeneralDetails.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objPatientVO.GeneralDetails.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "PatientCategoryID", DbType.Int64, objPatientVO.GeneralDetails.PatientTypeID);

                // BY BHUSHAN . . . . 
                dbServer.AddInParameter(command, "ReferralTypeID", DbType.Int64, objPatientVO.GeneralDetails.ReferralTypeID);

                dbServer.AddInParameter(command, "ReferralDoctorID", DbType.Int64, objPatientVO.GeneralDetails.ReferralDoctorID);               
                dbServer.AddInParameter(command, "ReferredToDoctorID", DbType.Int64, objPatientVO.GeneralDetails.ReferredToDoctorID);
                dbServer.AddInParameter(command, "ReferralDetail", DbType.String, objPatientVO.GeneralDetails.ReferralDetail);

                dbServer.AddInParameter(command, "CompanyName", DbType.String, Security.base64Encode(objPatientVO.CompanyName));
                // END . . . .

                dbServer.AddInParameter(command, "RegistrationDate", DbType.DateTime, objPatientVO.GeneralDetails.RegistrationDate);
                if (objPatientVO.GeneralDetails.LastName != null) objPatientVO.GeneralDetails.LastName = objPatientVO.GeneralDetails.LastName.Trim();
                dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.LastName));
                if (objPatientVO.GeneralDetails.FirstName != null) objPatientVO.GeneralDetails.FirstName = objPatientVO.GeneralDetails.FirstName.Trim();
                dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.FirstName));
                if (objPatientVO.GeneralDetails.MiddleName != null) objPatientVO.GeneralDetails.MiddleName = objPatientVO.GeneralDetails.MiddleName.Trim();
                dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.MiddleName));
                if (objPatientVO.FamilyName != null) objPatientVO.FamilyName = objPatientVO.FamilyName.Trim();
                dbServer.AddInParameter(command, "FamilyName", DbType.String, Security.base64Encode(objPatientVO.FamilyName));
                dbServer.AddInParameter(command, "GenderID", DbType.Int64, objPatientVO.GenderID);
                dbServer.AddInParameter(command, "DateOfBirth", DbType.DateTime, objPatientVO.GeneralDetails.DateOfBirth);
                dbServer.AddInParameter(command, "BloodGroupID", DbType.Int64, objPatientVO.BloodGroupID);
                dbServer.AddInParameter(command, "MaritalStatusID", DbType.Int64, objPatientVO.MaritalStatusID);
                dbServer.AddInParameter(command, "CivilID", DbType.String, Security.base64Encode(objPatientVO.CivilID));
                if (objPatientVO.ContactNo1 != null) objPatientVO.ContactNo1 = objPatientVO.ContactNo1.Trim();
                dbServer.AddInParameter(command, "ContactNo1", DbType.String, objPatientVO.ContactNo1);
                if (objPatientVO.ContactNo2 != null) objPatientVO.ContactNo2 = objPatientVO.ContactNo2.Trim();
                dbServer.AddInParameter(command, "ContactNo2", DbType.String, objPatientVO.ContactNo2);
                if (objPatientVO.FaxNo != null) objPatientVO.FaxNo = objPatientVO.FaxNo.Trim();
                dbServer.AddInParameter(command, "FaxNo", DbType.String, objPatientVO.FaxNo);
                if (objPatientVO.Email != null) objPatientVO.Email = objPatientVO.Email.Trim();
                dbServer.AddInParameter(command, "Email", DbType.String, Security.base64Encode(objPatientVO.Email));
                if (objPatientVO.AddressLine1 != null) objPatientVO.AddressLine1 = objPatientVO.AddressLine1.Trim();
                dbServer.AddInParameter(command, "AddressLine1", DbType.String, Security.base64Encode(objPatientVO.AddressLine1));
                if (objPatientVO.AddressLine2 != null) objPatientVO.AddressLine2 = objPatientVO.AddressLine2.Trim();
                dbServer.AddInParameter(command, "AddressLine2", DbType.String, Security.base64Encode(objPatientVO.AddressLine2));
                if (objPatientVO.AddressLine3 != null) objPatientVO.AddressLine3 = objPatientVO.AddressLine3.Trim();
                dbServer.AddInParameter(command, "AddressLine3", DbType.String, Security.base64Encode(objPatientVO.AddressLine3));
                if (objPatientVO.Country != null) objPatientVO.Country = objPatientVO.Country.Trim();

                //added by neena
                if (objPatientVO.OldRegistrationNo != null) objPatientVO.OldRegistrationNo = objPatientVO.OldRegistrationNo.Trim();
                dbServer.AddInParameter(command, "OldRegistrationNo", DbType.String, objPatientVO.OldRegistrationNo);

                //



                //By Anjali........................................................................................................

                //dbServer.AddInParameter(command, "Country", DbType.String, Security.base64Encode(objPatientVO.Country));
                //if (objPatientVO.State != null) objPatientVO.State = objPatientVO.State.Trim();
                //dbServer.AddInParameter(command, "State", DbType.String, Security.base64Encode(objPatientVO.State));
                //if (objPatientVO.City != null) objPatientVO.City = objPatientVO.City.Trim();
                //dbServer.AddInParameter(command, "City", DbType.String, Security.base64Encode(objPatientVO.City));
                //if (objPatientVO.Taluka != null) objPatientVO.Taluka = objPatientVO.Taluka.Trim();
                //dbServer.AddInParameter(command, "Taluka", DbType.String, Security.base64Encode(objPatientVO.Taluka));
                //if (objPatientVO.Area != null) objPatientVO.Area = objPatientVO.Area.Trim();
                //dbServer.AddInParameter(command, "Area", DbType.String, Security.base64Encode(objPatientVO.Area));
                //if (objPatientVO.District != null) objPatientVO.District = objPatientVO.District.Trim();
                //dbServer.AddInParameter(command, "District", DbType.String, Security.base64Encode(objPatientVO.District));

                dbServer.AddInParameter(command, "Country", DbType.String, null);
                dbServer.AddInParameter(command, "State", DbType.String, null);
                dbServer.AddInParameter(command, "City", DbType.String, null);
                dbServer.AddInParameter(command, "Taluka", DbType.String, null);
                dbServer.AddInParameter(command, "Area", DbType.String, null);
                dbServer.AddInParameter(command, "District", DbType.String, null);

                if (objPatientVO.CountryID > 0)
                    dbServer.AddInParameter(command, "CountryID", DbType.Int64, objPatientVO.CountryID);
                if (objPatientVO.StateID > 0)
                    dbServer.AddInParameter(command, "StateID", DbType.Int64, objPatientVO.StateID);
                if (objPatientVO.CityID > 0)
                    dbServer.AddInParameter(command, "CityID", DbType.Int64, objPatientVO.CityID);
                if (objPatientVO.RegionID > 0)
                    dbServer.AddInParameter(command, "RegionID", DbType.Int64, objPatientVO.RegionID);
                dbServer.AddInParameter(command, "RegType", DbType.Int16, objPatientVO.GeneralDetails.RegType);
                //.............................................................................................................................
                dbServer.AddInParameter(command, "MobileCountryCode", DbType.String, objPatientVO.MobileCountryCode);
                dbServer.AddInParameter(command, "ResiNoCountryCode", DbType.Int64, objPatientVO.ResiNoCountryCode);
                dbServer.AddInParameter(command, "ResiSTDCode", DbType.Int64, objPatientVO.ResiSTDCode);
                if (objPatientVO.Pincode != null) objPatientVO.Pincode = objPatientVO.Pincode.Trim();
                dbServer.AddInParameter(command, "Pincode", DbType.String, Security.base64Encode(objPatientVO.Pincode));
                dbServer.AddInParameter(command, "ReligionID", DbType.Int64, objPatientVO.ReligionID);
                dbServer.AddInParameter(command, "OccupationId", DbType.Int64, objPatientVO.OccupationId);

                //if (objPatientVO.Photo != null)
                //{
                //    DemoImage ds = new DemoImage();
                //    byte[] newPhoto=ds.VaryQualityLevel(objPatientVO.Photo);
                //    dbServer.AddInParameter(command, "Photo", DbType.Binary, newPhoto);
                //}

                dbServer.AddInParameter(command, "Photo", DbType.Binary, objPatientVO.Photo);  //commented by neena


                dbServer.AddInParameter(command, "IsLoyaltyMember", DbType.Boolean, objPatientVO.IsLoyaltyMember);
                dbServer.AddInParameter(command, "LoyaltyCardID", DbType.Int64, objPatientVO.LoyaltyCardID);
                dbServer.AddInParameter(command, "RelationID", DbType.Int64, objPatientVO.RelationID);
                dbServer.AddInParameter(command, "ParentPatientID", DbType.Int64, objPatientVO.ParentPatientID);
                dbServer.AddInParameter(command, "IssueDate", DbType.DateTime, objPatientVO.IssueDate);
                dbServer.AddInParameter(command, "EffectiveDate", DbType.DateTime, objPatientVO.EffectiveDate);
                dbServer.AddInParameter(command, "ExpiryDate", DbType.DateTime, objPatientVO.ExpiryDate);
                if (objPatientVO.LoyaltyCardNo != null) objPatientVO.LoyaltyCardNo = objPatientVO.LoyaltyCardNo.Trim();
                dbServer.AddInParameter(command, "LoyaltyCardNo", DbType.String, objPatientVO.LoyaltyCardNo);               

                dbServer.AddInParameter(command, "Status", DbType.Boolean, objPatientVO.Status);

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objPatientVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                //dbServer.AddInParameter(command, "UnitId", DbType.Int64, objPatientVO.UnitId);
                //dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, objPatientVO.CreatedUnitId);

                //dbServer.AddInParameter(command, "Status", DbType.Boolean, objPatientVO.Status);
                //dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objPatientVO.AddedBy);
                //if (objPatientVO.AddedOn != null) objPatientVO.AddedOn = objPatientVO.AddedOn.Trim();
                //dbServer.AddInParameter(command, "AddedOn", DbType.String, objPatientVO.AddedOn);
                //dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objPatientVO.AddedDateTime);
                //if (objPatientVO.AddedWindowsLoginName != null) objPatientVO.AddedWindowsLoginName = objPatientVO.AddedWindowsLoginName.Trim();
                //dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objPatientVO.AddedWindowsLoginName);

                //dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objPatientVO.UpdatedUnitId);
                //dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objPatientVO.UpdatedBy);
                //dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objPatientVO.UpdatedOn);
                //dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objPatientVO.UpdatedDateTime);
                //dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objPatientVO.UpdatedWindowsLoginName);

                //dbServer.AddInParameter(command, "MRNo", DbType.String, objPatientVO.GeneralDetails.MRNo);
                dbServer.AddParameter(command, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                //dbServer.AddParameter(command, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "????????????????????");          // Commented on 15062018 For New MRNO format
                dbServer.AddParameter(command, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????");  // Added on 15062018 For New MRNO format
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.GeneralDetails.PatientID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                //added by neena
                Random random = new Random();
                long RandomNumber = random.Next(111111, 666666);
                dbServer.AddInParameter(command, "RandomNumber", DbType.String, Convert.ToString(RandomNumber));
                //dbServer.AddParameter(command, "ImagePath", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.ImageName);
                //

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                //int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.PatientDetails.GeneralDetails.PatientID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.PatientDetails.GeneralDetails.MRNo = (string)dbServer.GetParameterValue(command, "MRNo");
                //BizActionObj.PatientDetails.ImageName = (string)dbServer.GetParameterValue(command, "ImagePath");  //added by neena                    
                string Err = (string)dbServer.GetParameterValue(command, "Err");
                BizActionObj.PatientDetails.GeneralDetails.UnitId = UserVo.UserLoginInfo.UnitId;

                //added by neena
                string ImgName = BizActionObj.PatientDetails.GeneralDetails.MRNo + "_" + RandomNumber + ".png";

                DemoImage ObjDM = new DemoImage();
                if (objPatientVO.Photo != null)
                    ObjDM.VaryQualityLevel(objPatientVO.Photo, ImgName, ImgSaveLocation);

                //File.WriteAllBytes(@"D:\Palash\PALASH_PATIENTREGISTRATION\IMAGE\" + ImgName, objPatientVO.Photo);
                //


                if (BizActionObj.PatientDetails.SpouseDetails != null && (BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 7 || BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 8 || BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 9))
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPatient");
                    dbServer.AddInParameter(command1, "PatientCategoryID", DbType.Int64, objPatientVO.GeneralDetails.PatientTypeID);
                    // BY BHSUAN . . . .
                    dbServer.AddInParameter(command1, "ReferralTypeID", DbType.Int64, objPatientVO.GeneralDetails.ReferralTypeID);

                    dbServer.AddInParameter(command1, "ReferralDoctorID", DbType.Int64, objPatientVO.GeneralDetails.ReferralDoctorID);                  
                    dbServer.AddInParameter(command1, "ReferredToDoctorID", DbType.Int64, objPatientVO.GeneralDetails.ReferredToDoctorID);
                    dbServer.AddInParameter(command1, "ReferralDetail", DbType.String, objPatientVO.GeneralDetails.ReferralDetail);
                    //
                    dbServer.AddInParameter(command1, "RegistrationDate", DbType.DateTime, objPatientVO.GeneralDetails.RegistrationDate);

                    if (objPatientVO.SpouseDetails.LastName != null) objPatientVO.SpouseDetails.LastName = objPatientVO.SpouseDetails.LastName.Trim();
                    dbServer.AddInParameter(command1, "LastName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.LastName));
                    if (objPatientVO.SpouseDetails.FirstName != null) objPatientVO.SpouseDetails.FirstName = objPatientVO.SpouseDetails.FirstName.Trim();
                    dbServer.AddInParameter(command1, "FirstName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.FirstName));
                    if (objPatientVO.SpouseDetails.MiddleName != null) objPatientVO.SpouseDetails.MiddleName = objPatientVO.SpouseDetails.MiddleName.Trim();
                    dbServer.AddInParameter(command1, "MiddleName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.MiddleName));
                    if (objPatientVO.SpouseDetails.FamilyName != null) objPatientVO.SpouseDetails.FamilyName = objPatientVO.SpouseDetails.FamilyName.Trim();
                    dbServer.AddInParameter(command1, "FamilyName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.FamilyName));
                    dbServer.AddInParameter(command1, "GenderID", DbType.Int64, objPatientVO.SpouseDetails.GenderID);
                    dbServer.AddInParameter(command1, "DateOfBirth", DbType.DateTime, objPatientVO.SpouseDetails.DateOfBirth);
                    dbServer.AddInParameter(command1, "BloodGroupID", DbType.Int64, objPatientVO.SpouseDetails.BloodGroupID);
                    dbServer.AddInParameter(command1, "MaritalStatusID", DbType.Int64, objPatientVO.SpouseDetails.MaritalStatusID);
                    dbServer.AddInParameter(command1, "CivilID", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.CivilID));
                    if (objPatientVO.SpouseDetails.ContactNo1 != null) objPatientVO.SpouseDetails.ContactNo1 = objPatientVO.SpouseDetails.ContactNo1.Trim();
                    dbServer.AddInParameter(command1, "ContactNo1", DbType.String, objPatientVO.SpouseDetails.ContactNo1);
                    if (objPatientVO.SpouseDetails.ContactNo2 != null) objPatientVO.SpouseDetails.ContactNo2 = objPatientVO.SpouseDetails.ContactNo2.Trim();
                    dbServer.AddInParameter(command1, "ContactNo2", DbType.String, objPatientVO.SpouseDetails.ContactNo2);
                    if (objPatientVO.SpouseDetails.FaxNo != null) objPatientVO.SpouseDetails.FaxNo = objPatientVO.SpouseDetails.FaxNo.Trim();
                    dbServer.AddInParameter(command1, "FaxNo", DbType.String, objPatientVO.SpouseDetails.FaxNo);
                    if (objPatientVO.SpouseDetails.Email != null) objPatientVO.SpouseDetails.Email = objPatientVO.SpouseDetails.Email.Trim();
                    dbServer.AddInParameter(command1, "Email", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Email));
                    if (objPatientVO.SpouseDetails.AddressLine1 != null) objPatientVO.SpouseDetails.AddressLine1 = objPatientVO.SpouseDetails.AddressLine1.Trim();
                    dbServer.AddInParameter(command1, "AddressLine1", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.AddressLine1));
                    if (objPatientVO.SpouseDetails.AddressLine2 != null) objPatientVO.SpouseDetails.AddressLine2 = objPatientVO.SpouseDetails.AddressLine2.Trim();
                    dbServer.AddInParameter(command1, "AddressLine2", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.AddressLine2));
                    if (objPatientVO.SpouseDetails.AddressLine3 != null) objPatientVO.SpouseDetails.AddressLine3 = objPatientVO.SpouseDetails.AddressLine3.Trim();
                    dbServer.AddInParameter(command1, "AddressLine3", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.AddressLine3));
                    if (objPatientVO.SpouseDetails.Country != null) objPatientVO.SpouseDetails.Country = objPatientVO.SpouseDetails.Country.Trim();

                    //added by neena
                    if (objPatientVO.SpouseDetails.SpouseOldRegistrationNo != null) objPatientVO.SpouseDetails.SpouseOldRegistrationNo = objPatientVO.SpouseDetails.SpouseOldRegistrationNo.Trim();
                    dbServer.AddInParameter(command1, "OldRegistrationNo", DbType.String, objPatientVO.SpouseDetails.SpouseOldRegistrationNo);

                    //

                    //By Anjali....................................................................................
                    //dbServer.AddInParameter(command1, "Country", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Country));
                    //if (objPatientVO.SpouseDetails.State != null) objPatientVO.SpouseDetails.State = objPatientVO.SpouseDetails.State.Trim();
                    //dbServer.AddInParameter(command1, "State", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.State));
                    //if (objPatientVO.SpouseDetails.City != null) objPatientVO.SpouseDetails.City = objPatientVO.SpouseDetails.City.Trim();
                    //dbServer.AddInParameter(command1, "City", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.City));
                    //if (objPatientVO.SpouseDetails.Taluka != null) objPatientVO.SpouseDetails.Taluka = objPatientVO.SpouseDetails.Taluka.Trim();
                    //dbServer.AddInParameter(command1, "Taluka", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Taluka));
                    //if (objPatientVO.SpouseDetails.Area != null) objPatientVO.SpouseDetails.Area = objPatientVO.SpouseDetails.Area.Trim();
                    //dbServer.AddInParameter(command1, "Area", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Area));
                    //if (objPatientVO.SpouseDetails.District != null) objPatientVO.SpouseDetails.District = objPatientVO.SpouseDetails.District.Trim();
                    //dbServer.AddInParameter(command1, "District", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.District));

                    dbServer.AddInParameter(command1, "Country", DbType.String, null);
                    dbServer.AddInParameter(command1, "State", DbType.String, null);
                    dbServer.AddInParameter(command1, "City", DbType.String, null);
                    dbServer.AddInParameter(command1, "Taluka", DbType.String, null);
                    dbServer.AddInParameter(command1, "Area", DbType.String, null);
                    dbServer.AddInParameter(command1, "District", DbType.String, null);


                    if (objPatientVO.SpouseDetails.CountryID > 0)
                        dbServer.AddInParameter(command1, "CountryID", DbType.Int64, objPatientVO.SpouseDetails.CountryID);
                    if (objPatientVO.SpouseDetails.StateID > 0)
                        dbServer.AddInParameter(command1, "StateID", DbType.Int64, objPatientVO.SpouseDetails.StateID);
                    if (objPatientVO.SpouseDetails.CityID > 0)
                        dbServer.AddInParameter(command1, "CityID", DbType.Int64, objPatientVO.SpouseDetails.CityID);
                    if (objPatientVO.SpouseDetails.RegionID > 0)
                        dbServer.AddInParameter(command1, "RegionID", DbType.Int64, objPatientVO.SpouseDetails.RegionID);
                    dbServer.AddInParameter(command1, "RegType", DbType.Int16, objPatientVO.GeneralDetails.RegType);
                    //.............................................................................................................................
                    dbServer.AddInParameter(command1, "MobileCountryCode", DbType.String, objPatientVO.SpouseDetails.MobileCountryCode);
                    dbServer.AddInParameter(command1, "ResiNoCountryCode", DbType.Int64, objPatientVO.SpouseDetails.ResiNoCountryCode);
                    dbServer.AddInParameter(command1, "ResiSTDCode", DbType.Int64, objPatientVO.SpouseDetails.ResiSTDCode);
                    if (objPatientVO.SpouseDetails.Pincode != null) objPatientVO.Pincode = objPatientVO.SpouseDetails.Pincode.Trim();
                    dbServer.AddInParameter(command1, "Pincode", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Pincode));

                    // BY BHUSHAN. .
                    dbServer.AddInParameter(command1, "CompanyName", DbType.String, Security.base64Encode(objPatientVO.CompanyName));

                    dbServer.AddInParameter(command1, "ReligionID", DbType.Int64, objPatientVO.SpouseDetails.ReligionID);
                    dbServer.AddInParameter(command1, "OccupationId", DbType.Int64, objPatientVO.SpouseDetails.OccupationId);

                    //if (objPatientVO.SpouseDetails.Photo != null)
                    //{
                    //    DemoImage ds1 = new DemoImage();
                    //    byte[] newImg=ds1.VaryQualityLevel(objPatientVO.SpouseDetails.Photo);
                    //    dbServer.AddInParameter(command1, "Photo", DbType.Binary, newImg);
                    //}

                    dbServer.AddInParameter(command1, "Photo", DbType.Binary, objPatientVO.SpouseDetails.Photo);  //commented by neena

                    //dbServer.AddInParameter(command1, "IsLoyaltyMember", DbType.Boolean, objPatientVO.IsLoyaltyMember);
                    //dbServer.AddInParameter(command1, "LoyaltyCardID", DbType.Int64, objPatientVO.LoyaltyCardID);
                    //dbServer.AddInParameter(command1, "RelationID", DbType.Int64, objPatientVO.RelationID);
                    //dbServer.AddInParameter(command1, "ParentPatientID", DbType.Int64, objPatientVO.ParentPatientID);
                    //dbServer.AddInParameter(command1, "IssueDate", DbType.DateTime, objPatientVO.IssueDate);
                    //dbServer.AddInParameter(command1, "EffectiveDate", DbType.DateTime, objPatientVO.EffectiveDate);
                    //dbServer.AddInParameter(command1, "ExpiryDate", DbType.DateTime, objPatientVO.ExpiryDate);
                    //if (objPatientVO.LoyaltyCardNo != null) objPatientVO.LoyaltyCardNo = objPatientVO.LoyaltyCardNo.Trim();
                    //dbServer.AddInParameter(command1, "LoyaltyCardNo", DbType.String, objPatientVO.LoyaltyCardNo);

                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, objPatientVO.Status);

                    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, objPatientVO.AddedDateTime);
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    objPatientVO.SpouseDetails.MRNo = objPatientVO.GeneralDetails.MRNo.Remove(objPatientVO.GeneralDetails.MRNo.Length - 1, 1);
                    dbServer.AddParameter(command1, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                    dbServer.AddParameter(command1, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.SpouseDetails.MRNo);
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.SpouseDetails.PatientID);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                    //added by neena
                    Random random1 = new Random();
                    long RandomNumber1 = random1.Next(111111, 666666);
                    dbServer.AddInParameter(command1, "RandomNumber", DbType.String, Convert.ToString(RandomNumber1));
                    //dbServer.AddParameter(command1, "ImagePath", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.SpouseDetails.ImageName);
                    //

                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    //int intStatus = dbServer.ExecuteNonQuery(command1);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                    BizActionObj.PatientDetails.SpouseDetails.PatientID = (long)dbServer.GetParameterValue(command1, "ID");
                    BizActionObj.PatientDetails.SpouseDetails.MRNo = (string)dbServer.GetParameterValue(command1, "MRNo");
                    //BizActionObj.PatientDetails.SpouseDetails.ImageName = (string)dbServer.GetParameterValue(command1, "ImagePath");  //added by neena                    
                    string Err1 = (string)dbServer.GetParameterValue(command1, "Err");
                    BizActionObj.PatientDetails.SpouseDetails.UnitId = UserVo.UserLoginInfo.UnitId;


                    //added by neena
                    string ImgName1 = BizActionObj.PatientDetails.SpouseDetails.MRNo + "_" + RandomNumber1 + ".png";
                    //DemoImage ObjDM = new DemoImage();
                    if (objPatientVO.SpouseDetails.Photo != null)
                        ObjDM.VaryQualityLevel(objPatientVO.SpouseDetails.Photo, ImgName1, ImgSaveLocation);
                    //File.WriteAllBytes(@"D:\Palash\PALASH_PATIENTREGISTRATION\IMAGE\" + ImgName1, objPatientVO.SpouseDetails.Photo);
                    //





                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddCoupleRegistration");
                    if (BizActionObj.PatientDetails.GenderID == (long)Genders.Male)
                    {
                        dbServer.AddInParameter(command2, "MalePatientID", DbType.Int64, objPatientVO.GeneralDetails.PatientID);
                        dbServer.AddInParameter(command2, "FemalePatientID", DbType.Int64, objPatientVO.SpouseDetails.PatientID);
                    }
                    else
                    {
                        dbServer.AddInParameter(command2, "MalePatientID", DbType.Int64, objPatientVO.SpouseDetails.PatientID);
                        dbServer.AddInParameter(command2, "FemalePatientID", DbType.Int64, objPatientVO.GeneralDetails.PatientID);
                    }

                    dbServer.AddInParameter(command2, "MalePatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "FemalePatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                    dbServer.AddInParameter(command2, "RegistrationDate", DbType.DateTime, objPatientVO.GeneralDetails.RegistrationDate);

                    dbServer.AddInParameter(command2, "Status", DbType.Boolean, objPatientVO.Status);

                    dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);


                    dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, objPatientVO.AddedDateTime);
                    dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                    dbServer.AddParameter(command2, "CoupleRgistrationNumber", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "000000000000000000000000000000000000000000000000000");
                    dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                    //int intStatus = dbServer.ExecuteNonQuery(command1);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                    // BizActionObj.PatientDetails.SpouseDetails.PatientID = (long)dbServer.GetParameterValue(command1, "ID");
                    //BizActionObj.PatientDetails.SpouseDetails.MRNo = (string)dbServer.GetParameterValue(command1, "MRNo");
                    // string Err1 = (string)dbServer.GetParameterValue(command1, "Err");
                    //BizActionObj.PatientDetails.SpouseDetails.UnitId = UserVo.UserLoginInfo.UnitId;
                }

                trans.Commit();
                addPatientUserDetails(BizActionObj, UserVo);

            }
            catch (Exception ex)
            {

                trans.Rollback();
                throw ex;
            }
            finally
            {
                trans.Dispose();
                trans = null;
            }
            return BizActionObj;
        }

        private void addPatientUserDetails(clsAddPatientBizActionVO BizActionObj, clsUserVO UserVo)
        { }

        private clsAddPatientBizActionVO UpdatePatientDetails(clsAddPatientBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = dbServer.CreateConnection();

            try
            {
                clsPatientVO objPatientVO = BizActionObj.PatientDetails;

                con.Open();
                trans = con.BeginTransaction();


                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdatePatient");
                //objPatientVO.LinkServer = @"SHSPL19\SQLEXPRESS";
                dbServer.AddInParameter(command, "LinkServer", DbType.String, objPatientVO.GeneralDetails.LinkServer);
                if (objPatientVO.GeneralDetails.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objPatientVO.GeneralDetails.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "PatientCategoryID", DbType.Int64, objPatientVO.GeneralDetails.PatientTypeID);
                // BY BHUSHAN . . . . 
                dbServer.AddInParameter(command, "ReferralTypeID", DbType.Int64, objPatientVO.GeneralDetails.ReferralTypeID);

                dbServer.AddInParameter(command, "ReferralDoctorID", DbType.Int64, objPatientVO.GeneralDetails.ReferralDoctorID);
                dbServer.AddInParameter(command, "ReferredToDoctorID", DbType.Int64, objPatientVO.GeneralDetails.ReferredToDoctorID);
                dbServer.AddInParameter(command, "ReferralDetail", DbType.String, objPatientVO.GeneralDetails.ReferralDetail);
                dbServer.AddInParameter(command, "CompanyName", DbType.String, Security.base64Encode(objPatientVO.CompanyName));
                //
                dbServer.AddInParameter(command, "MRNo", DbType.String, objPatientVO.GeneralDetails.MRNo);
                dbServer.AddInParameter(command, "RegistrationDate", DbType.DateTime, objPatientVO.GeneralDetails.RegistrationDate);
                if (objPatientVO.GeneralDetails.LastName != null) objPatientVO.GeneralDetails.LastName = objPatientVO.GeneralDetails.LastName.Trim();
                dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.LastName));
                if (objPatientVO.GeneralDetails.FirstName != null) objPatientVO.GeneralDetails.FirstName = objPatientVO.GeneralDetails.FirstName.Trim();
                dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.FirstName));
                if (objPatientVO.GeneralDetails.MiddleName != null) objPatientVO.GeneralDetails.MiddleName = objPatientVO.GeneralDetails.MiddleName.Trim();
                dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.MiddleName));
                if (objPatientVO.FamilyName != null) objPatientVO.FamilyName = objPatientVO.FamilyName.Trim();
                dbServer.AddInParameter(command, "FamilyName", DbType.String, Security.base64Encode(objPatientVO.FamilyName));
                dbServer.AddInParameter(command, "GenderID", DbType.Int64, objPatientVO.GenderID);
                dbServer.AddInParameter(command, "DateOfBirth", DbType.DateTime, objPatientVO.GeneralDetails.DateOfBirth);
                dbServer.AddInParameter(command, "BloodGroupID", DbType.Int64, objPatientVO.BloodGroupID);
                dbServer.AddInParameter(command, "MaritalStatusID", DbType.Int64, objPatientVO.MaritalStatusID);
                dbServer.AddInParameter(command, "CivilID", DbType.String, Security.base64Encode(objPatientVO.CivilID));
                if (objPatientVO.ContactNo1 != null) objPatientVO.ContactNo1 = objPatientVO.ContactNo1.Trim();
                dbServer.AddInParameter(command, "ContactNo1", DbType.String, objPatientVO.ContactNo1);
                if (objPatientVO.ContactNo2 != null) objPatientVO.ContactNo2 = objPatientVO.ContactNo2.Trim();
                dbServer.AddInParameter(command, "ContactNo2", DbType.String, objPatientVO.ContactNo2);
                if (objPatientVO.FaxNo != null) objPatientVO.FaxNo = objPatientVO.FaxNo.Trim();
                dbServer.AddInParameter(command, "FaxNo", DbType.String, objPatientVO.FaxNo);
                if (objPatientVO.Email != null) objPatientVO.Email = objPatientVO.Email.Trim();
                dbServer.AddInParameter(command, "Email", DbType.String, Security.base64Encode(objPatientVO.Email));
                if (objPatientVO.AddressLine1 != null) objPatientVO.AddressLine1 = objPatientVO.AddressLine1.Trim();
                dbServer.AddInParameter(command, "AddressLine1", DbType.String, Security.base64Encode(objPatientVO.AddressLine1));
                if (objPatientVO.AddressLine2 != null) objPatientVO.AddressLine2 = objPatientVO.AddressLine2.Trim();
                dbServer.AddInParameter(command, "AddressLine2", DbType.String, Security.base64Encode(objPatientVO.AddressLine2));
                if (objPatientVO.AddressLine3 != null) objPatientVO.AddressLine3 = objPatientVO.AddressLine3.Trim();
                dbServer.AddInParameter(command, "AddressLine3", DbType.String, Security.base64Encode(objPatientVO.AddressLine3));

                //added by neena
                if (objPatientVO.OldRegistrationNo != null) objPatientVO.OldRegistrationNo = objPatientVO.OldRegistrationNo.Trim();
                dbServer.AddInParameter(command, "OldRegistrationNo", DbType.String, objPatientVO.OldRegistrationNo);
                //

                //By Anjali.................................................................................................
                //if (objPatientVO.Country != null) objPatientVO.Country = objPatientVO.Country.Trim();
                //dbServer.AddInParameter(command, "Country", DbType.String, Security.base64Encode(objPatientVO.Country));
                //if (objPatientVO.State != null) objPatientVO.State = objPatientVO.State.Trim();
                //dbServer.AddInParameter(command, "State", DbType.String, Security.base64Encode(objPatientVO.State));
                //if (objPatientVO.City != null) objPatientVO.City = objPatientVO.City.Trim();
                //dbServer.AddInParameter(command, "City", DbType.String, Security.base64Encode(objPatientVO.City));
                //if (objPatientVO.Taluka != null) objPatientVO.Taluka = objPatientVO.Taluka.Trim();
                //dbServer.AddInParameter(command, "Taluka", DbType.String, Security.base64Encode(objPatientVO.Taluka));
                //if (objPatientVO.Area != null) objPatientVO.Area = objPatientVO.Area.Trim();
                //dbServer.AddInParameter(command, "Area", DbType.String, Security.base64Encode(objPatientVO.Area));
                //if (objPatientVO.District != null) objPatientVO.District = objPatientVO.District.Trim();
                //dbServer.AddInParameter(command, "District", DbType.String, Security.base64Encode(objPatientVO.District));
                dbServer.AddInParameter(command, "Country", DbType.String, null);
                dbServer.AddInParameter(command, "State", DbType.String, null);
                dbServer.AddInParameter(command, "City", DbType.String, null);
                dbServer.AddInParameter(command, "Taluka", DbType.String, null);
                dbServer.AddInParameter(command, "Area", DbType.String, null);
                dbServer.AddInParameter(command, "District", DbType.String, null);
                if (objPatientVO.CountryID > 0)
                    dbServer.AddInParameter(command, "CountryID", DbType.Int64, objPatientVO.CountryID);
                if (objPatientVO.StateID > 0)
                    dbServer.AddInParameter(command, "StateID", DbType.Int64, objPatientVO.StateID);
                if (objPatientVO.CityID > 0)
                    dbServer.AddInParameter(command, "CityID", DbType.Int64, objPatientVO.CityID);
                if (objPatientVO.RegionID > 0)
                    dbServer.AddInParameter(command, "RegionID", DbType.Int64, objPatientVO.RegionID);
                //...............................................................................................................
                dbServer.AddInParameter(command, "MobileCountryCode", DbType.String, objPatientVO.MobileCountryCode);
                dbServer.AddInParameter(command, "ResiNoCountryCode", DbType.Int64, objPatientVO.ResiNoCountryCode);
                dbServer.AddInParameter(command, "ResiSTDCode", DbType.Int64, objPatientVO.ResiSTDCode);
                if (objPatientVO.Pincode != null) objPatientVO.Pincode = objPatientVO.Pincode.Trim();
                dbServer.AddInParameter(command, "Pincode", DbType.String, Security.base64Encode(objPatientVO.Pincode));
                dbServer.AddInParameter(command, "ReligionID", DbType.Int64, objPatientVO.ReligionID);
                dbServer.AddInParameter(command, "OccupationId", DbType.Int64, objPatientVO.OccupationId);
                dbServer.AddInParameter(command, "AgentID", DbType.Int64, objPatientVO.AgentID);
                dbServer.AddInParameter(command, "AgencyID", DbType.Int64, objPatientVO.AgencyID);

                dbServer.AddInParameter(command, "Photo", DbType.Binary, objPatientVO.Photo); //commented by neena

                dbServer.AddInParameter(command, "IsLoyaltyMember", DbType.Boolean, objPatientVO.IsLoyaltyMember);
                dbServer.AddInParameter(command, "LoyaltyCardID", DbType.Int64, objPatientVO.LoyaltyCardID);
                dbServer.AddInParameter(command, "IssueDate", DbType.DateTime, objPatientVO.IssueDate);
                dbServer.AddInParameter(command, "EffectiveDate", DbType.DateTime, objPatientVO.EffectiveDate);
                dbServer.AddInParameter(command, "ExpiryDate", DbType.DateTime, objPatientVO.ExpiryDate);
                if (objPatientVO.LoyaltyCardNo != null) objPatientVO.LoyaltyCardNo = objPatientVO.LoyaltyCardNo.Trim();
                dbServer.AddInParameter(command, "LoyaltyCardNo", DbType.String, objPatientVO.LoyaltyCardNo);


                dbServer.AddInParameter(command, "Status", DbType.Boolean, objPatientVO.Status);

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objPatientVO.GeneralDetails.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objPatientVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddInParameter(command, "ID", DbType.Int64, objPatientVO.GeneralDetails.PatientID);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.SuccessStatus);

                //added by neena
                //dbServer.AddParameter(command, "ImagePath", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.ImageName);
                dbServer.AddParameter(command, "ImagePath", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "000000000000000000000000000000000000000000000000000");
                //

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                //added by neena
                BizActionObj.PatientDetails.ImageName = (string)dbServer.GetParameterValue(command, "ImagePath");
                DemoImage ObjDM = new DemoImage();
                if (objPatientVO.Photo != null)
                    ObjDM.VaryQualityLevel(objPatientVO.Photo, BizActionObj.PatientDetails.ImageName, ImgSaveLocation);
                //else
                //    File.WriteAllBytes(@"D:\Palash\PALASH_PATIENTREGISTRATION\IMAGE\" + BizActionObj.PatientDetails.ImageName, objPatientVO.Photo);
                //

                trans.Commit();



                //if (BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 7) // Couple
                //{
                //    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_UpdatePatientSpouseInformation");
                //    //objPatientVO.LinkServer = @"SHSPL19\SQLEXPRESS";

                //    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, objPatientVO.SpouseDetails.PatientID);
                //    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, objPatientVO.SpouseDetails.PatientUnitID);

                //    dbServer.AddInParameter(command1, "MRNo", DbType.String, objPatientVO.SpouseDetails.MRNo);
                //    dbServer.AddInParameter(command1, "RegistrationDate", DbType.DateTime, objPatientVO.SpouseDetails.RegistrationDate);
                //    if (objPatientVO.SpouseDetails.LastName != null) objPatientVO.SpouseDetails.LastName = objPatientVO.SpouseDetails.LastName.Trim();
                //    dbServer.AddInParameter(command1, "LastName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.LastName));
                //    if (objPatientVO.SpouseDetails.FirstName != null) objPatientVO.SpouseDetails.FirstName = objPatientVO.SpouseDetails.FirstName.Trim();
                //    dbServer.AddInParameter(command1, "FirstName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.FirstName));
                //    if (objPatientVO.SpouseDetails.MiddleName != null) objPatientVO.SpouseDetails.MiddleName = objPatientVO.SpouseDetails.MiddleName.Trim();
                //    dbServer.AddInParameter(command1, "MiddleName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.MiddleName));
                //    if (objPatientVO.SpouseDetails.FamilyName != null) objPatientVO.SpouseDetails.FamilyName = objPatientVO.SpouseDetails.FamilyName.Trim();
                //    dbServer.AddInParameter(command1, "FamilyName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.FamilyName));
                //    dbServer.AddInParameter(command1, "GenderID", DbType.Int64, objPatientVO.SpouseDetails.GenderID);
                //    dbServer.AddInParameter(command1, "DateOfBirth", DbType.DateTime, objPatientVO.SpouseDetails.DateOfBirth);
                //    dbServer.AddInParameter(command1, "BloodGroupID", DbType.Int64, objPatientVO.SpouseDetails.BloodGroupID);
                //    dbServer.AddInParameter(command1, "MaritalStatusID", DbType.Int64, objPatientVO.SpouseDetails.MaritalStatusID);
                //    dbServer.AddInParameter(command1, "CivilID", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.CivilID));
                //    if (objPatientVO.SpouseDetails.ContactNo1 != null) objPatientVO.SpouseDetails.ContactNo1 = objPatientVO.SpouseDetails.ContactNo1.Trim();
                //    dbServer.AddInParameter(command1, "ContactNo1", DbType.String, objPatientVO.SpouseDetails.ContactNo1);
                //    if (objPatientVO.SpouseDetails.ContactNo2 != null) objPatientVO.SpouseDetails.ContactNo2 = objPatientVO.SpouseDetails.ContactNo2.Trim();
                //    dbServer.AddInParameter(command1, "ContactNo2", DbType.String, objPatientVO.SpouseDetails.ContactNo2);
                //    if (objPatientVO.SpouseDetails.FaxNo != null) objPatientVO.SpouseDetails.FaxNo = objPatientVO.SpouseDetails.FaxNo.Trim();
                //    dbServer.AddInParameter(command1, "FaxNo", DbType.String, objPatientVO.SpouseDetails.FaxNo);
                //    if (objPatientVO.SpouseDetails.Email != null) objPatientVO.SpouseDetails.Email = objPatientVO.SpouseDetails.Email.Trim();
                //    dbServer.AddInParameter(command1, "Email", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Email));
                //    if (objPatientVO.SpouseDetails.AddressLine1 != null) objPatientVO.SpouseDetails.AddressLine1 = objPatientVO.SpouseDetails.AddressLine1.Trim();
                //    dbServer.AddInParameter(command1, "AddressLine1", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.AddressLine1));
                //    if (objPatientVO.SpouseDetails.AddressLine2 != null) objPatientVO.SpouseDetails.AddressLine2 = objPatientVO.SpouseDetails.AddressLine2.Trim();
                //    dbServer.AddInParameter(command1, "AddressLine2", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.AddressLine2));
                //    if (objPatientVO.SpouseDetails.AddressLine3 != null) objPatientVO.SpouseDetails.AddressLine3 = objPatientVO.SpouseDetails.AddressLine3.Trim();
                //    dbServer.AddInParameter(command1, "AddressLine3", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.AddressLine3));
                //    if (objPatientVO.SpouseDetails.Country != null) objPatientVO.SpouseDetails.Country = objPatientVO.SpouseDetails.Country.Trim();
                //    dbServer.AddInParameter(command1, "Country", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Country));
                //    if (objPatientVO.SpouseDetails.State != null) objPatientVO.SpouseDetails.State = objPatientVO.SpouseDetails.State.Trim();
                //    dbServer.AddInParameter(command1, "State", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.State));
                //    if (objPatientVO.SpouseDetails.City != null) objPatientVO.SpouseDetails.City = objPatientVO.SpouseDetails.City.Trim();
                //    dbServer.AddInParameter(command1, "City", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.City));
                //    if (objPatientVO.SpouseDetails.Taluka != null) objPatientVO.SpouseDetails.Taluka = objPatientVO.SpouseDetails.Taluka.Trim();
                //    dbServer.AddInParameter(command1, "Taluka", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Taluka));
                //    if (objPatientVO.SpouseDetails.Area != null) objPatientVO.SpouseDetails.Area = objPatientVO.SpouseDetails.Area.Trim();
                //    dbServer.AddInParameter(command1, "Area", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Area));
                //    if (objPatientVO.SpouseDetails.District != null) objPatientVO.SpouseDetails.District = objPatientVO.SpouseDetails.District.Trim();
                //    dbServer.AddInParameter(command1, "District", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.District));
                //    dbServer.AddInParameter(command1, "MobileCountryCode", DbType.Int64, objPatientVO.SpouseDetails.MobileCountryCode);
                //    dbServer.AddInParameter(command1, "ResiNoCountryCode", DbType.Int64, objPatientVO.SpouseDetails.ResiNoCountryCode);
                //    dbServer.AddInParameter(command1, "ResiSTDCode", DbType.Int64, objPatientVO.SpouseDetails.ResiSTDCode);
                //    if (objPatientVO.SpouseDetails.Pincode != null) objPatientVO.SpouseDetails.Pincode = objPatientVO.SpouseDetails.Pincode.Trim();
                //    dbServer.AddInParameter(command1, "Pincode", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Pincode));
                //    dbServer.AddInParameter(command1, "ReligionID", DbType.Int64, objPatientVO.SpouseDetails.ReligionID);
                //    dbServer.AddInParameter(command1, "OccupationId", DbType.Int64, objPatientVO.SpouseDetails.OccupationId);
                //    dbServer.AddInParameter(command1, "Photo", DbType.Binary, objPatientVO.SpouseDetails.Photo);

                //    dbServer.AddInParameter(command1, "Status", DbType.Boolean, objPatientVO.SpouseDetails.Status);

                //    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, objPatientVO.SpouseDetails.UnitId);
                //    dbServer.AddInParameter(command1, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                //    dbServer.AddInParameter(command1, "UpdatedBy", DbType.Int64, UserVo.ID);
                //    dbServer.AddInParameter(command1, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //    dbServer.AddInParameter(command1, "UpdatedDateTime", DbType.DateTime, objPatientVO.SpouseDetails.UpdatedDateTime);
                //    dbServer.AddInParameter(command1, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                //    dbServer.AddInParameter(command1, "ID", DbType.Int64, objPatientVO.SpouseDetails.ID);
                //    dbServer.AddParameter(command1, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.SuccessStatus);

                //    int intStatus1 = dbServer.ExecuteNonQuery(command1);
                //    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                //}

            }
            catch (Exception ex)
            {

                trans.Rollback();
                throw ex;
            }
            finally
            {
                trans.Dispose();
                trans = null;
            }
            return BizActionObj;
        }

        // Added By CDS For Patient Registration With Transaction

        public override IValueObject AddPatientOPDWithTransaction(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientBizActionVO BizActionObj = valueObject as clsAddPatientBizActionVO;        //           
            if (BizActionObj.PatientDetails.GeneralDetails.PatientID == 0)
                BizActionObj = AddPatientDetailsOPDWithTransaction(BizActionObj, UserVo, null, null);
            else
                BizActionObj = UpdatePatientDetailsOPDWithTransaction(BizActionObj, UserVo);


            return valueObject;
        }

        //By Anjali.......................

        public override IValueObject AddPatientOPDWithTransaction(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {

            clsAddPatientBizActionVO BizActionObj = valueObject as clsAddPatientBizActionVO;

            if (BizActionObj.PatientDetails.GeneralDetails.PatientID == 0)
                BizActionObj = AddPatientDetailsOPDWithTransaction(BizActionObj, UserVo, myConnection, myTransaction);

            return valueObject;

        }
        //..................................

        private clsAddPatientBizActionVO AddPatientDetailsOPDWithTransaction(clsAddPatientBizActionVO BizActionObj, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {

            DbConnection con = null;
            DbTransaction trans = null;

            try
            {
                clsPatientVO objPatientVO = BizActionObj.PatientDetails;

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

                if (BizActionObj.IsSavePatientFromOPD == true)
                {

                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPatient");

                    dbServer.AddInParameter(command, "LinkServer", DbType.String, objPatientVO.GeneralDetails.LinkServer);
                    if (objPatientVO.GeneralDetails.LinkServer != null)
                        dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objPatientVO.GeneralDetails.LinkServer.Replace(@"\", "_"));

                    dbServer.AddInParameter(command, "PatientCategoryID", DbType.Int64, objPatientVO.GeneralDetails.PatientTypeID);

                    // BY BHUSHAN . . . . 
                    dbServer.AddInParameter(command, "ReferralTypeID", DbType.Int64, objPatientVO.GeneralDetails.ReferralTypeID);

                    dbServer.AddInParameter(command, "ReferralDoctorID", DbType.Int64, objPatientVO.GeneralDetails.ReferralDoctorID);
                    //Commeted by Narendra oct 2024
                   // dbServer.AddInParameter(command, "CoConsultantDoctorID", DbType.Int64, objPatientVO.GeneralDetails.CoConsultantDoctorID); //***//19
                    dbServer.AddInParameter(command, "ReferredToDoctorID", DbType.Int64, objPatientVO.GeneralDetails.ReferredToDoctorID);
                    dbServer.AddInParameter(command, "IsReferralDoc", DbType.Boolean, objPatientVO.GeneralDetails.IsReferralDoc);
                    dbServer.AddInParameter(command, "ReferralDetail", DbType.String, objPatientVO.GeneralDetails.ReferralDetail);
                    //dbServer.AddInParameter(command, "CompanyName", DbType.String, Security.base64Encode(objPatientVO.CompanyName));
                    dbServer.AddInParameter(command, "CompanyName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.CompanyName));
                    // END . . . .

                    //* Added by - Ajit Jadhav
                    //* Added Date - 4/8/2016
                    //* Comments - New BD Master Combo Add Save Data to T_Registration
                    dbServer.AddInParameter(command, "BDID", DbType.Int64, objPatientVO.BDID);

                    //* Added Date - 11/8/2016
                    if (objPatientVO.PanNumber != null)
                        dbServer.AddInParameter(command, "PanNumber", DbType.String, objPatientVO.PanNumber);
                    //Date 25/12/17
                    dbServer.AddInParameter(command, "CampID", DbType.Int64, objPatientVO.CampID);
                    dbServer.AddInParameter(command, "NoOfYearsOfMarriage", DbType.Int64, objPatientVO.NoOfYearsOfMarriage);
                    dbServer.AddInParameter(command, "NoOfExistingChildren", DbType.Int64, objPatientVO.NoOfExistingChildren);
                    dbServer.AddInParameter(command, "FamilyTypeID", DbType.Int64, objPatientVO.FamilyTypeID);
                    dbServer.AddInParameter(command, "AgentID", DbType.Int64, objPatientVO.AgentID);
                    dbServer.AddInParameter(command, "AgencyID", DbType.Int64, objPatientVO.AgencyID);
                    //***//----

                    dbServer.AddInParameter(command, "RegistrationDate", DbType.DateTime, objPatientVO.GeneralDetails.RegistrationDate);
                    if (objPatientVO.GeneralDetails.LastName != null) objPatientVO.GeneralDetails.LastName = objPatientVO.GeneralDetails.LastName.Trim();
                    dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.LastName));
                    if (objPatientVO.GeneralDetails.FirstName != null) objPatientVO.GeneralDetails.FirstName = objPatientVO.GeneralDetails.FirstName.Trim();
                    dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.FirstName));
                    if (objPatientVO.GeneralDetails.MiddleName != null) objPatientVO.GeneralDetails.MiddleName = objPatientVO.GeneralDetails.MiddleName.Trim();
                    dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.MiddleName));
                    if (objPatientVO.FamilyName != null) objPatientVO.FamilyName = objPatientVO.FamilyName.Trim();
                    dbServer.AddInParameter(command, "FamilyName", DbType.String, Security.base64Encode(objPatientVO.FamilyName));
                    dbServer.AddInParameter(command, "GenderID", DbType.Int64, objPatientVO.GenderID);
                    dbServer.AddInParameter(command, "DateOfBirth", DbType.DateTime, objPatientVO.GeneralDetails.DateOfBirth);
                    dbServer.AddInParameter(command, "IsAge", DbType.Boolean, objPatientVO.GeneralDetails.IsAge);
                    dbServer.AddInParameter(command, "BloodGroupID", DbType.Int64, objPatientVO.BloodGroupID);
                    dbServer.AddInParameter(command, "MaritalStatusID", DbType.Int64, objPatientVO.MaritalStatusID);
                    dbServer.AddInParameter(command, "CivilID", DbType.String, Security.base64Encode(objPatientVO.CivilID));
                    if (objPatientVO.ContactNo1 != null) objPatientVO.ContactNo1 = objPatientVO.ContactNo1.Trim();
                    dbServer.AddInParameter(command, "ContactNo1", DbType.String, objPatientVO.ContactNo1);
                    if (objPatientVO.ContactNo2 != null) objPatientVO.ContactNo2 = objPatientVO.ContactNo2.Trim();
                    dbServer.AddInParameter(command, "ContactNo2", DbType.String, objPatientVO.ContactNo2);
                    if (objPatientVO.FaxNo != null) objPatientVO.FaxNo = objPatientVO.FaxNo.Trim();
                    dbServer.AddInParameter(command, "FaxNo", DbType.String, objPatientVO.FaxNo);
                    if (objPatientVO.Email != null) objPatientVO.Email = objPatientVO.Email.Trim();
                    dbServer.AddInParameter(command, "Email", DbType.String, Security.base64Encode(objPatientVO.Email));
                    if (objPatientVO.AddressLine1 != null) objPatientVO.AddressLine1 = objPatientVO.AddressLine1.Trim();
                    dbServer.AddInParameter(command, "AddressLine1", DbType.String, Security.base64Encode(objPatientVO.AddressLine1));
                    if (objPatientVO.AddressLine2 != null) objPatientVO.AddressLine2 = objPatientVO.AddressLine2.Trim();
                    dbServer.AddInParameter(command, "AddressLine2", DbType.String, Security.base64Encode(objPatientVO.AddressLine2));
                    if (objPatientVO.AddressLine3 != null) objPatientVO.AddressLine3 = objPatientVO.AddressLine3.Trim();
                    dbServer.AddInParameter(command, "AddressLine3", DbType.String, Security.base64Encode(objPatientVO.AddressLine3));
                    if (objPatientVO.Country != null) objPatientVO.Country = objPatientVO.Country.Trim();

                    //added by neena
                    if (objPatientVO.OldRegistrationNo != null) objPatientVO.OldRegistrationNo = objPatientVO.OldRegistrationNo.Trim();
                    dbServer.AddInParameter(command, "OldRegistrationNo", DbType.String, objPatientVO.OldRegistrationNo);

                    //

                    //By Anjali........................................................................................................

                    //dbServer.AddInParameter(command, "Country", DbType.String, Security.base64Encode(objPatientVO.Country));
                    //if (objPatientVO.State != null) objPatientVO.State = objPatientVO.State.Trim();
                    //dbServer.AddInParameter(command, "State", DbType.String, Security.base64Encode(objPatientVO.State));
                    //if (objPatientVO.City != null) objPatientVO.City = objPatientVO.City.Trim();
                    //dbServer.AddInParameter(command, "City", DbType.String, Security.base64Encode(objPatientVO.City));
                    //if (objPatientVO.Taluka != null) objPatientVO.Taluka = objPatientVO.Taluka.Trim();
                    //dbServer.AddInParameter(command, "Taluka", DbType.String, Security.base64Encode(objPatientVO.Taluka));
                    //if (objPatientVO.Area != null) objPatientVO.Area = objPatientVO.Area.Trim();
                    //dbServer.AddInParameter(command, "Area", DbType.String, Security.base64Encode(objPatientVO.Area));
                    //if (objPatientVO.District != null) objPatientVO.District = objPatientVO.District.Trim();
                    //dbServer.AddInParameter(command, "District", DbType.String, Security.base64Encode(objPatientVO.District));

                    dbServer.AddInParameter(command, "Country", DbType.String, null);
                    dbServer.AddInParameter(command, "State", DbType.String, null);
                    dbServer.AddInParameter(command, "City", DbType.String, null);
                    dbServer.AddInParameter(command, "Taluka", DbType.String, null);
                    dbServer.AddInParameter(command, "Area", DbType.String, null);
                    dbServer.AddInParameter(command, "District", DbType.String, null);

                    if (objPatientVO.CountryID > 0)
                        dbServer.AddInParameter(command, "CountryID", DbType.Int64, objPatientVO.CountryID);
                    if (objPatientVO.StateID > 0)
                        dbServer.AddInParameter(command, "StateID", DbType.Int64, objPatientVO.StateID);
                    if (objPatientVO.CityID > 0)
                        dbServer.AddInParameter(command, "CityID", DbType.Int64, objPatientVO.CityID);
                    if (objPatientVO.RegionID > 0)
                        dbServer.AddInParameter(command, "RegionID", DbType.Int64, objPatientVO.RegionID);


                    if (objPatientVO.PrefixId > 0)
                        dbServer.AddInParameter(command, "PrefixId", DbType.Int64, objPatientVO.PrefixId);
                    if (objPatientVO.IdentityID > 0)
                        dbServer.AddInParameter(command, "IdentityID", DbType.Int64, objPatientVO.IdentityID);
                    dbServer.AddInParameter(command, "IdentityNumber", DbType.String, objPatientVO.IdentityNumber);
                    dbServer.AddInParameter(command, "RemarkForPatientType", DbType.String, objPatientVO.RemarkForPatientType);
                    dbServer.AddInParameter(command, "IsInternationalPatient", DbType.Boolean, objPatientVO.IsInternationalPatient);
                    dbServer.AddInParameter(command, "Education", DbType.String, objPatientVO.Education);
                    dbServer.AddInParameter(command, "RegType", DbType.Int16, objPatientVO.GeneralDetails.RegType);




                    //.............................................................................................................................
                    dbServer.AddInParameter(command, "MobileCountryCode", DbType.String, objPatientVO.MobileCountryCode);
                    dbServer.AddInParameter(command, "ResiNoCountryCode", DbType.Int64, objPatientVO.ResiNoCountryCode);
                    dbServer.AddInParameter(command, "ResiSTDCode", DbType.Int64, objPatientVO.ResiSTDCode);
                    if (objPatientVO.Pincode != null) objPatientVO.Pincode = objPatientVO.Pincode.Trim();
                    dbServer.AddInParameter(command, "Pincode", DbType.String, Security.base64Encode(objPatientVO.Pincode));
                    dbServer.AddInParameter(command, "ReligionID", DbType.Int64, objPatientVO.ReligionID);
                    dbServer.AddInParameter(command, "OccupationId", DbType.Int64, objPatientVO.OccupationId);

                    //if (objPatientVO.Photo != null)
                    //{
                    //    DemoImage ds = new DemoImage();
                    //    byte[] newPhoto = ds.VaryQualityLevel(objPatientVO.Photo);
                    //    dbServer.AddInParameter(command, "Photo", DbType.Binary, newPhoto);
                    //}

                    dbServer.AddInParameter(command, "Photo", DbType.Binary, objPatientVO.Photo);  //commented by neena

                    dbServer.AddInParameter(command, "IsLoyaltyMember", DbType.Boolean, objPatientVO.IsLoyaltyMember);
                    dbServer.AddInParameter(command, "LoyaltyCardID", DbType.Int64, objPatientVO.LoyaltyCardID);
                    dbServer.AddInParameter(command, "RelationID", DbType.Int64, objPatientVO.RelationID);
                    dbServer.AddInParameter(command, "ParentPatientID", DbType.Int64, objPatientVO.ParentPatientID);
                    dbServer.AddInParameter(command, "IssueDate", DbType.DateTime, objPatientVO.IssueDate);
                    dbServer.AddInParameter(command, "EffectiveDate", DbType.DateTime, objPatientVO.EffectiveDate);
                    dbServer.AddInParameter(command, "ExpiryDate", DbType.DateTime, objPatientVO.ExpiryDate);
                    if (objPatientVO.LoyaltyCardNo != null) objPatientVO.LoyaltyCardNo = objPatientVO.LoyaltyCardNo.Trim();
                    dbServer.AddInParameter(command, "LoyaltyCardNo", DbType.String, objPatientVO.LoyaltyCardNo);

                    //dbServer.AddInParameter(command, "LoyaltyCardNo", DbType.String, objPatientVO.LoyaltyCardNo);

                    //Added by Ashish Z.---
                    if (objPatientVO.GeneralDetails.SonDaughterOf != null) objPatientVO.GeneralDetails.SonDaughterOf = objPatientVO.GeneralDetails.SonDaughterOf.Trim();
                    dbServer.AddInParameter(command, "DaughterOf", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.SonDaughterOf));
                    dbServer.AddInParameter(command, "NationalityID", DbType.Int64, objPatientVO.NationalityID);
                    dbServer.AddInParameter(command, "PrefLangID", DbType.Int64, objPatientVO.PreferredLangID);
                    dbServer.AddInParameter(command, "TreatRequiredID", DbType.Int64, objPatientVO.TreatRequiredID);
                    dbServer.AddInParameter(command, "EducationID", DbType.Int64, objPatientVO.EducationID);
                    dbServer.AddInParameter(command, "MarriageAnnivDate", DbType.DateTime, objPatientVO.MarriageAnnDate);
                    dbServer.AddInParameter(command, "NoOfPeople", DbType.Int32, objPatientVO.GeneralDetails.NoOfPeople);
                    dbServer.AddInParameter(command, "IsClinicVisited", DbType.Boolean, BizActionObj.PatientDetails.IsClinicVisited);
                    dbServer.AddInParameter(command, "ClinicName", DbType.String, objPatientVO.GeneralDetails.ClinicName);
                    dbServer.AddInParameter(command, "SpecialRegID", DbType.Int64, objPatientVO.SpecialRegID);
                    //----
                    #region For Pediatric Flow

                    if (BizActionObj.PatientDetails != null && BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 13)
                    {
                        dbServer.AddInParameter(command, "BabyNo", DbType.Int32, objPatientVO.BabyNo);
                        dbServer.AddInParameter(command, "BabyOfNo", DbType.Int32, objPatientVO.BabyOfNo);
                        dbServer.AddInParameter(command, "BabyWeight", DbType.String, objPatientVO.BabyWeight);
                        dbServer.AddInParameter(command, "LinkPatientID", DbType.Int64, objPatientVO.LinkPatientID);
                        dbServer.AddInParameter(command, "LinkPatientUnitID", DbType.Int64, objPatientVO.LinkPatientUnitID);
                        dbServer.AddInParameter(command, "LinkPatientMrNo", DbType.String, objPatientVO.LinkPatientMrNo);
                    }

                    #endregion
                    //***//
                    dbServer.AddInParameter(command, "IsStaffPatient", DbType.Boolean, objPatientVO.IsStaffPatient);
                    dbServer.AddInParameter(command, "StaffID", DbType.Int64, objPatientVO.StaffID);
                    //------
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, objPatientVO.Status);

                    dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objPatientVO.AddedDateTime);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                    //dbServer.AddInParameter(command, "UnitId", DbType.Int64, objPatientVO.UnitId);
                    //dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, objPatientVO.CreatedUnitId);

                    //dbServer.AddInParameter(command, "Status", DbType.Boolean, objPatientVO.Status);
                    //dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objPatientVO.AddedBy);
                    //if (objPatientVO.AddedOn != null) objPatientVO.AddedOn = objPatientVO.AddedOn.Trim();
                    //dbServer.AddInParameter(command, "AddedOn", DbType.String, objPatientVO.AddedOn);
                    //dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objPatientVO.AddedDateTime);
                    //if (objPatientVO.AddedWindowsLoginName != null) objPatientVO.AddedWindowsLoginName = objPatientVO.AddedWindowsLoginName.Trim();
                    //dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objPatientVO.AddedWindowsLoginName);

                    //dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objPatientVO.UpdatedUnitId);
                    //dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objPatientVO.UpdatedBy);
                    //dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objPatientVO.UpdatedOn);
                    //dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objPatientVO.UpdatedDateTime);
                    //dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objPatientVO.UpdatedWindowsLoginName);

                    //dbServer.AddInParameter(command, "MRNo", DbType.String, objPatientVO.GeneralDetails.MRNo);
                    dbServer.AddParameter(command, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                    //dbServer.AddParameter(command, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "????????????????????");          // Commented on 15062018 For New MRNO format
                    dbServer.AddParameter(command, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????");  // Added on 15062018 For New MRNO format
                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.GeneralDetails.PatientID);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                    //added by neena
                    //Random random = new Random();
                    //long RandomNumber = random.Next(111111, 666666);
                    //dbServer.AddInParameter(command, "RandomNumber", DbType.String, Convert.ToString(RandomNumber));
                    dbServer.AddParameter(command, "ImagePath", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                    //


                    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    //int intStatus = dbServer.ExecuteNonQuery(command);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    BizActionObj.PatientDetails.GeneralDetails.PatientID = (long)dbServer.GetParameterValue(command, "ID");
                    BizActionObj.PatientDetails.GeneralDetails.MRNo = (string)dbServer.GetParameterValue(command, "MRNo");
                    BizActionObj.PatientDetails.ImageName = Convert.ToString(dbServer.GetParameterValue(command, "ImagePath"));  //added by neena
                    string Err = (string)dbServer.GetParameterValue(command, "Err");
                    BizActionObj.PatientDetails.GeneralDetails.UnitId = UserVo.UserLoginInfo.UnitId;

                    //added by neena
                    //string ImgName = BizActionObj.PatientDetails.GeneralDetails.MRNo + "_" + RandomNumber + ".png";
                    DemoImage ObjDM = new DemoImage();
                    if (objPatientVO.Photo != null)
                        ObjDM.VaryQualityLevel(objPatientVO.Photo, BizActionObj.PatientDetails.ImageName, ImgSaveLocation);
                    //File.WriteAllBytes(@"D:\Palash\PALASH_PATIENTREGISTRATION\IMAGE\" + ImgName, objPatientVO.Photo);
                    //


                    if (BizActionObj.PatientDetails.SpouseDetails != null && (BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 7 || BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 8 || BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 9))
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPatient");


                        //* Added by - Ajit Jadhav
                        //* Added Date - 4/8/2016
                        //* Comments - New BD Master Combo Add Save Data to T_Registration
                        dbServer.AddInParameter(command1, "BDID", DbType.Int64, objPatientVO.BDID);
                        if (objPatientVO.PanNumber != null)
                            dbServer.AddInParameter(command1, "PanNumber", DbType.String, objPatientVO.SpouseDetails.SpousePanNumber);
                        if (objPatientVO.CampID != null)
                        {
                            dbServer.AddInParameter(command1, "CampID", DbType.Int64, objPatientVO.CampID);
                        }
                        //***//-------------
                        dbServer.AddInParameter(command1, "PatientCategoryID", DbType.Int64, objPatientVO.GeneralDetails.PatientTypeID);
                        // BY BHSUAN . . . .
                        dbServer.AddInParameter(command1, "ReferralTypeID", DbType.Int64, objPatientVO.GeneralDetails.ReferralTypeID);

                        dbServer.AddInParameter(command1, "ReferralDoctorID", DbType.Int64, objPatientVO.GeneralDetails.ReferralDoctorID);
                        dbServer.AddInParameter(command1, "CoConsultantDoctorID", DbType.Int64, objPatientVO.GeneralDetails.CoConsultantDoctorID); //***//19
                        dbServer.AddInParameter(command1, "ReferredToDoctorID", DbType.Int64, objPatientVO.GeneralDetails.ReferredToDoctorID);
                        dbServer.AddInParameter(command1, "IsReferralDoc", DbType.Boolean, objPatientVO.GeneralDetails.IsReferralDoc);
                        dbServer.AddInParameter(command1, "ReferralDetail", DbType.String, objPatientVO.GeneralDetails.ReferralDetail);
                        dbServer.AddInParameter(command1, "CompanyName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.CompanyName));
                        //
                        dbServer.AddInParameter(command1, "RegistrationDate", DbType.DateTime, objPatientVO.GeneralDetails.RegistrationDate);

                        if (objPatientVO.SpouseDetails.LastName != null) objPatientVO.SpouseDetails.LastName = objPatientVO.SpouseDetails.LastName.Trim();
                        dbServer.AddInParameter(command1, "LastName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.LastName));
                        if (objPatientVO.SpouseDetails.FirstName != null) objPatientVO.SpouseDetails.FirstName = objPatientVO.SpouseDetails.FirstName.Trim();
                        dbServer.AddInParameter(command1, "FirstName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.FirstName));
                        if (objPatientVO.SpouseDetails.MiddleName != null) objPatientVO.SpouseDetails.MiddleName = objPatientVO.SpouseDetails.MiddleName.Trim();
                        dbServer.AddInParameter(command1, "MiddleName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.MiddleName));
                        if (objPatientVO.SpouseDetails.FamilyName != null) objPatientVO.SpouseDetails.FamilyName = objPatientVO.SpouseDetails.FamilyName.Trim();
                        dbServer.AddInParameter(command1, "FamilyName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.FamilyName));
                        dbServer.AddInParameter(command1, "GenderID", DbType.Int64, objPatientVO.SpouseDetails.GenderID);
                        dbServer.AddInParameter(command1, "DateOfBirth", DbType.DateTime, objPatientVO.SpouseDetails.DateOfBirth);
                        dbServer.AddInParameter(command1, "IsAge", DbType.Boolean, objPatientVO.SpouseDetails.IsAge);
                        dbServer.AddInParameter(command1, "BloodGroupID", DbType.Int64, objPatientVO.SpouseDetails.BloodGroupID);
                        dbServer.AddInParameter(command1, "MaritalStatusID", DbType.Int64, objPatientVO.SpouseDetails.MaritalStatusID);
                        dbServer.AddInParameter(command1, "CivilID", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.CivilID));
                        if (objPatientVO.SpouseDetails.ContactNo1 != null) objPatientVO.SpouseDetails.ContactNo1 = objPatientVO.SpouseDetails.ContactNo1.Trim();
                        dbServer.AddInParameter(command1, "ContactNo1", DbType.String, objPatientVO.SpouseDetails.ContactNo1);
                        if (objPatientVO.SpouseDetails.ContactNo2 != null) objPatientVO.SpouseDetails.ContactNo2 = objPatientVO.SpouseDetails.ContactNo2.Trim();
                        dbServer.AddInParameter(command1, "ContactNo2", DbType.String, objPatientVO.SpouseDetails.ContactNo2);
                        if (objPatientVO.SpouseDetails.FaxNo != null) objPatientVO.SpouseDetails.FaxNo = objPatientVO.SpouseDetails.FaxNo.Trim();
                        dbServer.AddInParameter(command1, "FaxNo", DbType.String, objPatientVO.SpouseDetails.FaxNo);
                        if (objPatientVO.SpouseDetails.Email != null) objPatientVO.SpouseDetails.Email = objPatientVO.SpouseDetails.Email.Trim();
                        dbServer.AddInParameter(command1, "Email", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Email));
                        if (objPatientVO.SpouseDetails.AddressLine1 != null) objPatientVO.SpouseDetails.AddressLine1 = objPatientVO.SpouseDetails.AddressLine1.Trim();
                        dbServer.AddInParameter(command1, "AddressLine1", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.AddressLine1));
                        if (objPatientVO.SpouseDetails.AddressLine2 != null) objPatientVO.SpouseDetails.AddressLine2 = objPatientVO.SpouseDetails.AddressLine2.Trim();
                        dbServer.AddInParameter(command1, "AddressLine2", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.AddressLine2));
                        if (objPatientVO.SpouseDetails.AddressLine3 != null) objPatientVO.SpouseDetails.AddressLine3 = objPatientVO.SpouseDetails.AddressLine3.Trim();
                        dbServer.AddInParameter(command1, "AddressLine3", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.AddressLine3));
                        if (objPatientVO.SpouseDetails.Country != null) objPatientVO.SpouseDetails.Country = objPatientVO.SpouseDetails.Country.Trim();

                        //added by neena
                        if (objPatientVO.SpouseDetails.SpouseOldRegistrationNo != null) objPatientVO.SpouseDetails.SpouseOldRegistrationNo = objPatientVO.SpouseDetails.SpouseOldRegistrationNo.Trim();
                        dbServer.AddInParameter(command1, "OldRegistrationNo", DbType.String, objPatientVO.SpouseDetails.SpouseOldRegistrationNo);

                        //

                        //By Anjali....................................................................................
                        //dbServer.AddInParameter(command1, "Country", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Country));
                        //if (objPatientVO.SpouseDetails.State != null) objPatientVO.SpouseDetails.State = objPatientVO.SpouseDetails.State.Trim();
                        //dbServer.AddInParameter(command1, "State", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.State));
                        //if (objPatientVO.SpouseDetails.City != null) objPatientVO.SpouseDetails.City = objPatientVO.SpouseDetails.City.Trim();
                        //dbServer.AddInParameter(command1, "City", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.City));
                        //if (objPatientVO.SpouseDetails.Taluka != null) objPatientVO.SpouseDetails.Taluka = objPatientVO.SpouseDetails.Taluka.Trim();
                        //dbServer.AddInParameter(command1, "Taluka", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Taluka));
                        //if (objPatientVO.SpouseDetails.Area != null) objPatientVO.SpouseDetails.Area = objPatientVO.SpouseDetails.Area.Trim();
                        //dbServer.AddInParameter(command1, "Area", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Area));
                        //if (objPatientVO.SpouseDetails.District != null) objPatientVO.SpouseDetails.District = objPatientVO.SpouseDetails.District.Trim();
                        //dbServer.AddInParameter(command1, "District", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.District));

                        dbServer.AddInParameter(command1, "Country", DbType.String, null);
                        dbServer.AddInParameter(command1, "State", DbType.String, null);
                        dbServer.AddInParameter(command1, "City", DbType.String, null);
                        dbServer.AddInParameter(command1, "Taluka", DbType.String, null);
                        dbServer.AddInParameter(command1, "Area", DbType.String, null);
                        dbServer.AddInParameter(command1, "District", DbType.String, null);


                        if (objPatientVO.SpouseDetails.CountryID > 0)
                            dbServer.AddInParameter(command1, "CountryID", DbType.Int64, objPatientVO.SpouseDetails.CountryID);
                        if (objPatientVO.SpouseDetails.StateID > 0)
                            dbServer.AddInParameter(command1, "StateID", DbType.Int64, objPatientVO.SpouseDetails.StateID);
                        if (objPatientVO.SpouseDetails.CityID > 0)
                            dbServer.AddInParameter(command1, "CityID", DbType.Int64, objPatientVO.SpouseDetails.CityID);
                        if (objPatientVO.SpouseDetails.RegionID > 0)
                            dbServer.AddInParameter(command1, "RegionID", DbType.Int64, objPatientVO.SpouseDetails.RegionID);

                        if (objPatientVO.PrefixId > 0)
                            dbServer.AddInParameter(command1, "PrefixId", DbType.Int64, objPatientVO.SpouseDetails.PrefixId);
                        if (objPatientVO.IdentityID > 0)
                            dbServer.AddInParameter(command1, "IdentityID", DbType.Int64, objPatientVO.SpouseDetails.IdentityID);
                        dbServer.AddInParameter(command1, "IdentityNumber", DbType.String, objPatientVO.SpouseDetails.IdentityNumber);
                        dbServer.AddInParameter(command1, "RemarkForPatientType", DbType.String, objPatientVO.SpouseDetails.RemarkForPatientType);
                        dbServer.AddInParameter(command1, "IsInternationalPatient", DbType.Boolean, objPatientVO.SpouseDetails.IsInternationalPatient);
                        dbServer.AddInParameter(command1, "Education", DbType.String, objPatientVO.SpouseDetails.Education);
                        dbServer.AddInParameter(command1, "RegType", DbType.Int16, objPatientVO.GeneralDetails.RegType);
                        dbServer.AddInParameter(command1, "AgentID", DbType.Int64, objPatientVO.AgentID);
                        dbServer.AddInParameter(command1, "AgencyID", DbType.Int64, objPatientVO.AgencyID);



                        //.............................................................................................................................
                        dbServer.AddInParameter(command1, "MobileCountryCode", DbType.String, objPatientVO.SpouseDetails.MobileCountryCode);
                        dbServer.AddInParameter(command1, "ResiNoCountryCode", DbType.Int64, objPatientVO.SpouseDetails.ResiNoCountryCode);
                        dbServer.AddInParameter(command1, "ResiSTDCode", DbType.Int64, objPatientVO.SpouseDetails.ResiSTDCode);
                        if (objPatientVO.SpouseDetails.Pincode != null) objPatientVO.Pincode = objPatientVO.SpouseDetails.Pincode.Trim();
                        dbServer.AddInParameter(command1, "Pincode", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Pincode));

                        // BY BHUSHAN. .
                        //dbServer.AddInParameter(command1, "CompanyName", DbType.String, Security.base64Encode(objPatientVO.CompanyName)); //Coomented By Bhushanp 18052017

                        dbServer.AddInParameter(command1, "ReligionID", DbType.Int64, objPatientVO.SpouseDetails.ReligionID);
                        dbServer.AddInParameter(command1, "OccupationId", DbType.Int64, objPatientVO.SpouseDetails.OccupationId);


                        dbServer.AddInParameter(command1, "Photo", DbType.Binary, objPatientVO.SpouseDetails.Photo);   //commented by neena



                        //dbServer.AddInParameter(command1, "IsLoyaltyMember", DbType.Boolean, objPatientVO.IsLoyaltyMember);
                        //dbServer.AddInParameter(command1, "LoyaltyCardID", DbType.Int64, objPatientVO.LoyaltyCardID);
                        //dbServer.AddInParameter(command1, "RelationID", DbType.Int64, objPatientVO.RelationID);
                        //dbServer.AddInParameter(command1, "ParentPatientID", DbType.Int64, objPatientVO.ParentPatientID);
                        //dbServer.AddInParameter(command1, "IssueDate", DbType.DateTime, objPatientVO.IssueDate);
                        //dbServer.AddInParameter(command1, "EffectiveDate", DbType.DateTime, objPatientVO.EffectiveDate);
                        //dbServer.AddInParameter(command1, "ExpiryDate", DbType.DateTime, objPatientVO.ExpiryDate);
                        //if (objPatientVO.LoyaltyCardNo != null) objPatientVO.LoyaltyCardNo = objPatientVO.LoyaltyCardNo.Trim();
                        //dbServer.AddInParameter(command1, "LoyaltyCardNo", DbType.String, objPatientVO.LoyaltyCardNo);

                        //Added by Ashish Z.---
                        if (objPatientVO.SpouseDetails.SonDaughterOf != null) objPatientVO.SpouseDetails.SonDaughterOf = objPatientVO.SpouseDetails.SonDaughterOf.Trim();
                        dbServer.AddInParameter(command1, "DaughterOf", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.SonDaughterOf));
                        dbServer.AddInParameter(command1, "NationalityID", DbType.Int64, objPatientVO.SpouseDetails.NationalityID);
                        dbServer.AddInParameter(command1, "PrefLangID", DbType.Int64, objPatientVO.SpouseDetails.PreferredLangID);
                        dbServer.AddInParameter(command1, "TreatRequiredID", DbType.Int64, objPatientVO.TreatRequiredID);
                        dbServer.AddInParameter(command1, "EducationID", DbType.Int64, objPatientVO.SpouseDetails.EducationID);
                        dbServer.AddInParameter(command1, "MarriageAnnivDate", DbType.DateTime, objPatientVO.MarriageAnnDate);
                        dbServer.AddInParameter(command1, "NoOfPeople", DbType.Int32, objPatientVO.GeneralDetails.NoOfPeople);
                        dbServer.AddInParameter(command1, "IsClinicVisited", DbType.Boolean, BizActionObj.PatientDetails.IsClinicVisited);
                        dbServer.AddInParameter(command1, "ClinicName", DbType.String, objPatientVO.GeneralDetails.ClinicName);
                        dbServer.AddInParameter(command1, "SpecialRegID", DbType.Int64, objPatientVO.SpecialRegID);
                        dbServer.AddInParameter(command1, "NoOfYearsOfMarriage", DbType.Int64, objPatientVO.NoOfYearsOfMarriage);
                        dbServer.AddInParameter(command1, "NoOfExistingChildren", DbType.Int64, objPatientVO.NoOfExistingChildren);
                        //----

                        //***//-----                       
                        dbServer.AddInParameter(command1, "FamilyTypeID", DbType.Int64, objPatientVO.FamilyTypeID);
                        dbServer.AddInParameter(command1, "DonorMRNO", DbType.String, BizActionObj.PatientDetails.GeneralDetails.MRNo);
                        //--------

                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, objPatientVO.Status);

                        dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, objPatientVO.AddedDateTime);
                        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        // objPatientVO.SpouseDetails.MRNo = objPatientVO.GeneralDetails.MRNo.Remove(objPatientVO.GeneralDetails.MRNo.Length - 1, 1); //commented by akshays on 21/11/2015 for barcode 
                        objPatientVO.SpouseDetails.MRNo = objPatientVO.GeneralDetails.MRNo; //added by akshays on 21/11/2015 for barcode
                        dbServer.AddParameter(command1, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        dbServer.AddParameter(command1, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.SpouseDetails.MRNo);
                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.SpouseDetails.PatientID);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                        //added by neena
                        //Random random1 = new Random();
                        //long RandomNumber1 = random1.Next(111111, 666666);
                        //dbServer.AddInParameter(command1, "RandomNumber", DbType.String, Convert.ToString(RandomNumber1));
                        dbServer.AddParameter(command1, "ImagePath", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");

                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                        //int intStatus = dbServer.ExecuteNonQuery(command1);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                        BizActionObj.PatientDetails.SpouseDetails.PatientID = (long)dbServer.GetParameterValue(command1, "ID");
                        BizActionObj.PatientDetails.SpouseDetails.MRNo = Convert.ToString(dbServer.GetParameterValue(command1, "MRNo"));
                        BizActionObj.PatientDetails.SpouseDetails.ImageName = Convert.ToString(dbServer.GetParameterValue(command1, "ImagePath"));  //added by neena
                        string Err1 = (string)dbServer.GetParameterValue(command1, "Err");
                        BizActionObj.PatientDetails.SpouseDetails.UnitId = UserVo.UserLoginInfo.UnitId;


                        //added by neena
                        //string ImgName1 = BizActionObj.PatientDetails.SpouseDetails.MRNo + "_" + RandomNumber1 + ".png";
                        //DemoImage ObjDM = new DemoImage();
                        if (objPatientVO.SpouseDetails.Photo != null)
                            ObjDM.VaryQualityLevel(objPatientVO.SpouseDetails.Photo, BizActionObj.PatientDetails.SpouseDetails.ImageName, ImgSaveLocation);
                        //File.WriteAllBytes(@"D:\Palash\PALASH_PATIENTREGISTRATION\IMAGE\" + ImgName1, objPatientVO.SpouseDetails.Photo);
                        //



                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddCoupleRegistration");
                        if (BizActionObj.PatientDetails.GenderID == (long)Genders.Male)
                        {
                            dbServer.AddInParameter(command2, "MalePatientID", DbType.Int64, objPatientVO.GeneralDetails.PatientID);
                            dbServer.AddInParameter(command2, "FemalePatientID", DbType.Int64, objPatientVO.SpouseDetails.PatientID);
                        }
                        else
                        {
                            dbServer.AddInParameter(command2, "MalePatientID", DbType.Int64, objPatientVO.SpouseDetails.PatientID);
                            dbServer.AddInParameter(command2, "FemalePatientID", DbType.Int64, objPatientVO.GeneralDetails.PatientID);
                        }

                        dbServer.AddInParameter(command2, "MalePatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "FemalePatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                        dbServer.AddInParameter(command2, "RegistrationDate", DbType.DateTime, objPatientVO.GeneralDetails.RegistrationDate);

                        dbServer.AddInParameter(command2, "Status", DbType.Boolean, objPatientVO.Status);

                        dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);


                        dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, objPatientVO.AddedDateTime);
                        dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                        dbServer.AddParameter(command2, "CoupleRgistrationNumber", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "000000000000000000000000000000000000000000000000000");
                        dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                        dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                        //int intStatus = dbServer.ExecuteNonQuery(command1);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                        // BizActionObj.PatientDetails.SpouseDetails.PatientID = (long)dbServer.GetParameterValue(command1, "ID");
                        //BizActionObj.PatientDetails.SpouseDetails.MRNo = (string)dbServer.GetParameterValue(command1, "MRNo");
                        // string Err1 = (string)dbServer.GetParameterValue(command1, "Err");
                        //BizActionObj.PatientDetails.SpouseDetails.UnitId = UserVo.UserLoginInfo.UnitId;


                        //***//---------------Spouse Bank Details ----------------                


                        if (BizActionObj.BankDetails != null)
                        {
                            DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientBankDetails");

                            dbServer.AddInParameter(command3, "PatientID", DbType.Int64, objPatientVO.SpouseDetails.PatientID);
                            dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.UnitId);
                            dbServer.AddInParameter(command3, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command3, "BankID", DbType.Int64, BizActionObj.BankDetails.BankId);
                            dbServer.AddInParameter(command3, "BranchID", DbType.Int64, BizActionObj.BankDetails.BranchId);
                            dbServer.AddInParameter(command3, "IFSCCode", DbType.String, BizActionObj.BankDetails.IFSCCode);
                            dbServer.AddInParameter(command3, "AccountType", DbType.Boolean, BizActionObj.BankDetails.AccountTypeId);
                            dbServer.AddInParameter(command3, "AccountNo", DbType.String, BizActionObj.BankDetails.AccountNumber);
                            dbServer.AddInParameter(command3, "AccountHolderName", DbType.String, BizActionObj.BankDetails.AccountHolderName);
                            dbServer.AddInParameter(command3, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, objPatientVO.AddedDateTime);
                            dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.ID);
                            dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                            int Status = dbServer.ExecuteNonQuery(command3, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                        }
                        //------------------------------------



                    }


                    //***//---------------MalePatient Bank Details----------------                


                    if (BizActionObj.BankDetails != null)
                    {
                        DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientBankDetails");

                        dbServer.AddInParameter(command3, "PatientID", DbType.Int64, objPatientVO.GeneralDetails.PatientID);
                        dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.UnitId);
                        dbServer.AddInParameter(command3, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command3, "BankID", DbType.Int64, BizActionObj.BankDetails.BankId);
                        dbServer.AddInParameter(command3, "BranchID", DbType.Int64, BizActionObj.BankDetails.BranchId);
                        dbServer.AddInParameter(command3, "IFSCCode", DbType.String, BizActionObj.BankDetails.IFSCCode);
                        dbServer.AddInParameter(command3, "AccountType", DbType.Boolean, BizActionObj.BankDetails.AccountTypeId);
                        dbServer.AddInParameter(command3, "AccountNo", DbType.String, BizActionObj.BankDetails.AccountNumber);
                        dbServer.AddInParameter(command3, "AccountHolderName", DbType.String, BizActionObj.BankDetails.AccountHolderName);
                        dbServer.AddInParameter(command3, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, objPatientVO.AddedDateTime);
                        dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.ID);
                        dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                        int Status = dbServer.ExecuteNonQuery(command3, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                    }
                    //------------------------------------

                }

                if (BizActionObj.IsSaveSponsor == true)
                {
                    clsBasePatientSposorDAL objBaseDAL = clsBasePatientSposorDAL.GetInstance();
                    BizActionObj.BizActionVOSaveSponsor.PatientSponsorDetails.PatientId = BizActionObj.PatientDetails.GeneralDetails.PatientID;
                    BizActionObj.BizActionVOSaveSponsor.PatientSponsorDetails.PatientUnitId = BizActionObj.PatientDetails.GeneralDetails.UnitId;

                    BizActionObj.BizActionVOSaveSponsor = (clsAddPatientSponsorBizActionVO)objBaseDAL.AddPatientSponsorDetailsWithTransaction(BizActionObj.BizActionVOSaveSponsor, UserVo, con, trans);
                    if (BizActionObj.BizActionVOSaveSponsor.SuccessStatus == -1) throw new Exception();


                    //......................................................
                    if (BizActionObj.PatientDetails.SpouseDetails != null && (BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 7 || BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 8 || BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 9))
                    {
                        clsBasePatientSposorDAL objBaseDAL1 = clsBasePatientSposorDAL.GetInstance();
                        BizActionObj.BizActionVOSaveSponsorForMale.PatientSponsorDetails.PatientId = BizActionObj.PatientDetails.SpouseDetails.PatientID;
                        BizActionObj.BizActionVOSaveSponsorForMale.PatientSponsorDetails.PatientUnitId = BizActionObj.PatientDetails.SpouseDetails.UnitId;

                        BizActionObj.BizActionVOSaveSponsorForMale = (clsAddPatientSponsorBizActionVO)objBaseDAL1.AddPatientSponsorDetailsWithTransaction(BizActionObj.BizActionVOSaveSponsorForMale, UserVo, con, trans);
                        if (BizActionObj.BizActionVOSaveSponsorForMale.SuccessStatus == -1) throw new Exception();
                    }
                    //..........................................................

                }

                if (myConnection == null) trans.Commit();

                addPatientUserDetails(BizActionObj, UserVo);
            }
            catch (Exception ex)
            {
                //throw;
                BizActionObj.SuccessStatus = -1;
                if (myConnection == null) trans.Rollback();
            }
            finally
            {
                if (myConnection == null) con.Close();
            }
            return BizActionObj;
        }


        private clsAddPatientBizActionVO UpdatePatientDetailsOPDWithTransaction(clsAddPatientBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = dbServer.CreateConnection();

            try
            {
                clsPatientVO objPatientVO = BizActionObj.PatientDetails;

                con.Open();
                trans = con.BeginTransaction();

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdatePatient");
                //objPatientVO.LinkServer = @"SHSPL19\SQLEXPRESS";
                dbServer.AddInParameter(command, "LinkServer", DbType.String, objPatientVO.GeneralDetails.LinkServer);
                if (objPatientVO.GeneralDetails.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objPatientVO.GeneralDetails.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "PatientCategoryID", DbType.Int64, objPatientVO.GeneralDetails.PatientTypeID);
                // BY BHUSHAN . . . . 
                dbServer.AddInParameter(command, "ReferralTypeID", DbType.Int64, objPatientVO.GeneralDetails.ReferralTypeID);

                dbServer.AddInParameter(command, "ReferralDoctorID", DbType.Int64, objPatientVO.GeneralDetails.ReferralDoctorID);
                dbServer.AddInParameter(command, "CoConsultantDoctorID", DbType.Int64, objPatientVO.GeneralDetails.CoConsultantDoctorID); //***//19
                dbServer.AddInParameter(command, "ReferredToDoctorID", DbType.Int64, objPatientVO.GeneralDetails.ReferredToDoctorID);
                dbServer.AddInParameter(command, "IsReferralDoc", DbType.Boolean, objPatientVO.GeneralDetails.IsReferralDoc);
                dbServer.AddInParameter(command, "ReferralDetail", DbType.String, objPatientVO.GeneralDetails.ReferralDetail);
                dbServer.AddInParameter(command, "CompanyName", DbType.String, Security.base64Encode(objPatientVO.CompanyName));
                //
                dbServer.AddInParameter(command, "MRNo", DbType.String, objPatientVO.GeneralDetails.MRNo);
                dbServer.AddInParameter(command, "RegistrationDate", DbType.DateTime, objPatientVO.GeneralDetails.RegistrationDate);
                if (objPatientVO.GeneralDetails.LastName != null) objPatientVO.GeneralDetails.LastName = objPatientVO.GeneralDetails.LastName.Trim();
                dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.LastName));
                if (objPatientVO.GeneralDetails.FirstName != null) objPatientVO.GeneralDetails.FirstName = objPatientVO.GeneralDetails.FirstName.Trim();
                dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.FirstName));
                if (objPatientVO.GeneralDetails.MiddleName != null) objPatientVO.GeneralDetails.MiddleName = objPatientVO.GeneralDetails.MiddleName.Trim();
                dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.MiddleName));
                if (objPatientVO.FamilyName != null) objPatientVO.FamilyName = objPatientVO.FamilyName.Trim();
                dbServer.AddInParameter(command, "FamilyName", DbType.String, Security.base64Encode(objPatientVO.FamilyName));
                dbServer.AddInParameter(command, "GenderID", DbType.Int64, objPatientVO.GenderID);
                dbServer.AddInParameter(command, "DateOfBirth", DbType.DateTime, objPatientVO.GeneralDetails.DateOfBirth);
                dbServer.AddInParameter(command, "IsAge", DbType.Boolean, objPatientVO.GeneralDetails.IsAge);
                dbServer.AddInParameter(command, "BloodGroupID", DbType.Int64, objPatientVO.BloodGroupID);
                dbServer.AddInParameter(command, "MaritalStatusID", DbType.Int64, objPatientVO.MaritalStatusID);
                dbServer.AddInParameter(command, "CivilID", DbType.String, Security.base64Encode(objPatientVO.CivilID));
                if (objPatientVO.ContactNo1 != null) objPatientVO.ContactNo1 = objPatientVO.ContactNo1.Trim();
                dbServer.AddInParameter(command, "ContactNo1", DbType.String, objPatientVO.ContactNo1);
                if (objPatientVO.ContactNo2 != null) objPatientVO.ContactNo2 = objPatientVO.ContactNo2.Trim();
                dbServer.AddInParameter(command, "ContactNo2", DbType.String, objPatientVO.ContactNo2);
                if (objPatientVO.FaxNo != null) objPatientVO.FaxNo = objPatientVO.FaxNo.Trim();
                dbServer.AddInParameter(command, "FaxNo", DbType.String, objPatientVO.FaxNo);
                if (objPatientVO.Email != null) objPatientVO.Email = objPatientVO.Email.Trim();
                dbServer.AddInParameter(command, "Email", DbType.String, Security.base64Encode(objPatientVO.Email));
                if (objPatientVO.AddressLine1 != null) objPatientVO.AddressLine1 = objPatientVO.AddressLine1.Trim();
                dbServer.AddInParameter(command, "AddressLine1", DbType.String, Security.base64Encode(objPatientVO.AddressLine1));
                if (objPatientVO.AddressLine2 != null) objPatientVO.AddressLine2 = objPatientVO.AddressLine2.Trim();
                dbServer.AddInParameter(command, "AddressLine2", DbType.String, Security.base64Encode(objPatientVO.AddressLine2));
                if (objPatientVO.AddressLine3 != null) objPatientVO.AddressLine3 = objPatientVO.AddressLine3.Trim();
                dbServer.AddInParameter(command, "AddressLine3", DbType.String, Security.base64Encode(objPatientVO.AddressLine3));
                if (objPatientVO.BDID > 0)   //added by devidas
                {
                    dbServer.AddInParameter(command, "BDID", DbType.Int64, objPatientVO.BDID);
                }
                //added by neena
                if (objPatientVO.OldRegistrationNo != null) objPatientVO.OldRegistrationNo = objPatientVO.OldRegistrationNo.Trim();
                dbServer.AddInParameter(command, "OldRegistrationNo", DbType.String, objPatientVO.OldRegistrationNo);
                //

                //By Anjali.................................................................................................
                //if (objPatientVO.Country != null) objPatientVO.Country = objPatientVO.Country.Trim();
                //dbServer.AddInParameter(command, "Country", DbType.String, Security.base64Encode(objPatientVO.Country));
                //if (objPatientVO.State != null) objPatientVO.State = objPatientVO.State.Trim();
                //dbServer.AddInParameter(command, "State", DbType.String, Security.base64Encode(objPatientVO.State));
                //if (objPatientVO.City != null) objPatientVO.City = objPatientVO.City.Trim();
                //dbServer.AddInParameter(command, "City", DbType.String, Security.base64Encode(objPatientVO.City));
                //if (objPatientVO.Taluka != null) objPatientVO.Taluka = objPatientVO.Taluka.Trim();
                //dbServer.AddInParameter(command, "Taluka", DbType.String, Security.base64Encode(objPatientVO.Taluka));
                //if (objPatientVO.Area != null) objPatientVO.Area = objPatientVO.Area.Trim();
                //dbServer.AddInParameter(command, "Area", DbType.String, Security.base64Encode(objPatientVO.Area));
                //if (objPatientVO.District != null) objPatientVO.District = objPatientVO.District.Trim();
                //dbServer.AddInParameter(command, "District", DbType.String, Security.base64Encode(objPatientVO.District));
                dbServer.AddInParameter(command, "Country", DbType.String, null);
                dbServer.AddInParameter(command, "State", DbType.String, null);
                dbServer.AddInParameter(command, "City", DbType.String, null);
                dbServer.AddInParameter(command, "Taluka", DbType.String, null);
                dbServer.AddInParameter(command, "Area", DbType.String, null);
                dbServer.AddInParameter(command, "District", DbType.String, null);
                if (objPatientVO.CountryID > 0)
                    dbServer.AddInParameter(command, "CountryID", DbType.Int64, objPatientVO.CountryID);
                if (objPatientVO.StateID > 0)
                    dbServer.AddInParameter(command, "StateID", DbType.Int64, objPatientVO.StateID);
                if (objPatientVO.CityID > 0)
                    dbServer.AddInParameter(command, "CityID", DbType.Int64, objPatientVO.CityID);
                if (objPatientVO.RegionID > 0)
                    dbServer.AddInParameter(command, "RegionID", DbType.Int64, objPatientVO.RegionID);

                if (objPatientVO.PrefixId > 0)
                    dbServer.AddInParameter(command, "PrefixId", DbType.Int64, objPatientVO.PrefixId);
                if (objPatientVO.IdentityID > 0)
                    dbServer.AddInParameter(command, "IdentityID", DbType.Int64, objPatientVO.IdentityID);
                dbServer.AddInParameter(command, "IdentityNumber", DbType.String, objPatientVO.IdentityNumber);
                dbServer.AddInParameter(command, "RemarkForPatientType", DbType.String, objPatientVO.RemarkForPatientType);
                dbServer.AddInParameter(command, "IsInternationalPatient", DbType.Boolean, objPatientVO.IsInternationalPatient);
                dbServer.AddInParameter(command, "Education", DbType.String, objPatientVO.Education);
                //...............................................................................................................
                dbServer.AddInParameter(command, "MobileCountryCode", DbType.String, objPatientVO.MobileCountryCode);
                dbServer.AddInParameter(command, "ResiNoCountryCode", DbType.Int64, objPatientVO.ResiNoCountryCode);
                dbServer.AddInParameter(command, "ResiSTDCode", DbType.Int64, objPatientVO.ResiSTDCode);
                if (objPatientVO.Pincode != null) objPatientVO.Pincode = objPatientVO.Pincode.Trim();
                dbServer.AddInParameter(command, "Pincode", DbType.String, Security.base64Encode(objPatientVO.Pincode));
                dbServer.AddInParameter(command, "ReligionID", DbType.Int64, objPatientVO.ReligionID);
                dbServer.AddInParameter(command, "OccupationId", DbType.Int64, objPatientVO.OccupationId);
                dbServer.AddInParameter(command, "AgentID", DbType.Int64, objPatientVO.AgentID);
                dbServer.AddInParameter(command, "AgencyID", DbType.Int64, objPatientVO.AgencyID);
                //if (objPatientVO.Photo != null)
                //{
                //    DemoImage ds = new DemoImage();
                //    byte[] newPhoto = ds.VaryQualityLevel(objPatientVO.Photo);
                //    dbServer.AddInParameter(command, "Photo", DbType.Binary, newPhoto);
                //}

                dbServer.AddInParameter(command, "Photo", DbType.Binary, objPatientVO.Photo);  //commented ny neena

                dbServer.AddInParameter(command, "IsLoyaltyMember", DbType.Boolean, objPatientVO.IsLoyaltyMember);
                dbServer.AddInParameter(command, "LoyaltyCardID", DbType.Int64, objPatientVO.LoyaltyCardID);
                dbServer.AddInParameter(command, "IssueDate", DbType.DateTime, objPatientVO.IssueDate);
                dbServer.AddInParameter(command, "EffectiveDate", DbType.DateTime, objPatientVO.EffectiveDate);
                dbServer.AddInParameter(command, "ExpiryDate", DbType.DateTime, objPatientVO.ExpiryDate);
                if (objPatientVO.LoyaltyCardNo != null) objPatientVO.LoyaltyCardNo = objPatientVO.LoyaltyCardNo.Trim();
                dbServer.AddInParameter(command, "LoyaltyCardNo", DbType.String, objPatientVO.LoyaltyCardNo);



                //Added by Ashish Z.---
                if (objPatientVO.GeneralDetails.SonDaughterOf != null) objPatientVO.GeneralDetails.SonDaughterOf = objPatientVO.GeneralDetails.SonDaughterOf.Trim();
                dbServer.AddInParameter(command, "DaughterOf", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.SonDaughterOf));
                dbServer.AddInParameter(command, "NationalityID", DbType.Int64, objPatientVO.NationalityID);
                dbServer.AddInParameter(command, "PrefLangID", DbType.Int64, objPatientVO.PreferredLangID);
                dbServer.AddInParameter(command, "TreatRequiredID", DbType.Int64, objPatientVO.TreatRequiredID);
                dbServer.AddInParameter(command, "EducationID", DbType.Int64, objPatientVO.EducationID);
                dbServer.AddInParameter(command, "MarriageAnnivDate", DbType.DateTime, objPatientVO.MarriageAnnDate);
                dbServer.AddInParameter(command, "NoOfPeople", DbType.Int32, objPatientVO.GeneralDetails.NoOfPeople);
                dbServer.AddInParameter(command, "IsClinicVisited", DbType.Boolean, objPatientVO.IsClinicVisited);
                dbServer.AddInParameter(command, "ClinicName", DbType.String, objPatientVO.GeneralDetails.ClinicName);
                dbServer.AddInParameter(command, "SpecialRegID", DbType.Int64, objPatientVO.SpecialRegID);



                #region For Pediatric Flow

                if (BizActionObj.PatientDetails != null && BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 13)
                {
                    dbServer.AddInParameter(command, "BabyNo", DbType.Int32, objPatientVO.BabyNo);
                    dbServer.AddInParameter(command, "BabyOfNo", DbType.Int32, objPatientVO.BabyOfNo);
                    dbServer.AddInParameter(command, "BabyWeight", DbType.String, objPatientVO.BabyWeight);
                    dbServer.AddInParameter(command, "LinkPatientID", DbType.Int64, objPatientVO.LinkPatientID);
                    dbServer.AddInParameter(command, "LinkPatientUnitID", DbType.Int64, objPatientVO.LinkPatientUnitID);
                    dbServer.AddInParameter(command, "LinkPatientMrNo", DbType.String, objPatientVO.LinkPatientMrNo);
                }

                #endregion








                //----


                //* Added by - Ajit Jadhav
                //* Added Date - 12/8/2016
                //* Comments - Update Data To T_Registration

                if (objPatientVO.PanNumber != null)
                {
                    dbServer.AddInParameter(command, "PanNumber", DbType.String, objPatientVO.PanNumber);
                }
                dbServer.AddInParameter(command, "FamilyTypeID", DbType.Int64, objPatientVO.FamilyTypeID);
                dbServer.AddInParameter(command, "NoOfYearsOfMarriage", DbType.Int64, objPatientVO.NoOfYearsOfMarriage);
                dbServer.AddInParameter(command, "NoOfExistingChildren", DbType.Int64, objPatientVO.NoOfExistingChildren);

                //***//----

                dbServer.AddInParameter(command, "Status", DbType.Boolean, objPatientVO.Status);

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objPatientVO.GeneralDetails.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objPatientVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddInParameter(command, "ID", DbType.Int64, objPatientVO.GeneralDetails.PatientID);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.SuccessStatus);

                //added by neena
                //dbServer.AddParameter(command, "ImagePath", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, Convert.ToString(objPatientVO.ImageName));


                dbServer.AddParameter(command, "ImagePath", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "000000000000000000000000000000000000000000000000000");
                //


                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                //added by neena
                BizActionObj.PatientDetails.ImageName = Convert.ToString(dbServer.GetParameterValue(command, "ImagePath"));
                DemoImage ObjDM = new DemoImage();
                if (objPatientVO.Photo != null)
                    ObjDM.VaryQualityLevel(objPatientVO.Photo, BizActionObj.PatientDetails.ImageName, ImgSaveLocation);
                //else
                //    File.WriteAllBytes(@"D:\Palash\PALASH_PATIENTREGISTRATION\IMAGE\" + BizActionObj.PatientDetails.ImageName, objPatientVO.Photo);
                //



                if (BizActionObj.BankDetails != null)
                {
                    DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientBankDetails");

                    dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.PatientID);
                    dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "BankID", DbType.Int64, BizActionObj.BankDetails.BankId);
                    dbServer.AddInParameter(command3, "BranchID", DbType.Int64, BizActionObj.BankDetails.BranchId);
                    dbServer.AddInParameter(command3, "IFSCCode", DbType.String, BizActionObj.BankDetails.IFSCCode);
                    dbServer.AddInParameter(command3, "AccountType", DbType.Boolean, BizActionObj.BankDetails.AccountTypeId);
                    dbServer.AddInParameter(command3, "AccountNo", DbType.String, BizActionObj.BankDetails.AccountNumber);
                    dbServer.AddInParameter(command3, "AccountHolderName", DbType.String, BizActionObj.BankDetails.AccountHolderName);
                    dbServer.AddInParameter(command3, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, objPatientVO.AddedDateTime);
                    dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.ID);
                    dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                    int Status = dbServer.ExecuteNonQuery(command3, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                }
                //------------------------------------



                trans.Commit();



                //if (BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 7) // Couple
                //{
                //    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_UpdatePatientSpouseInformation");
                //    //objPatientVO.LinkServer = @"SHSPL19\SQLEXPRESS";

                //    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, objPatientVO.SpouseDetails.PatientID);
                //    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, objPatientVO.SpouseDetails.PatientUnitID);

                //    dbServer.AddInParameter(command1, "MRNo", DbType.String, objPatientVO.SpouseDetails.MRNo);
                //    dbServer.AddInParameter(command1, "RegistrationDate", DbType.DateTime, objPatientVO.SpouseDetails.RegistrationDate);
                //    if (objPatientVO.SpouseDetails.LastName != null) objPatientVO.SpouseDetails.LastName = objPatientVO.SpouseDetails.LastName.Trim();
                //    dbServer.AddInParameter(command1, "LastName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.LastName));
                //    if (objPatientVO.SpouseDetails.FirstName != null) objPatientVO.SpouseDetails.FirstName = objPatientVO.SpouseDetails.FirstName.Trim();
                //    dbServer.AddInParameter(command1, "FirstName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.FirstName));
                //    if (objPatientVO.SpouseDetails.MiddleName != null) objPatientVO.SpouseDetails.MiddleName = objPatientVO.SpouseDetails.MiddleName.Trim();
                //    dbServer.AddInParameter(command1, "MiddleName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.MiddleName));
                //    if (objPatientVO.SpouseDetails.FamilyName != null) objPatientVO.SpouseDetails.FamilyName = objPatientVO.SpouseDetails.FamilyName.Trim();
                //    dbServer.AddInParameter(command1, "FamilyName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.FamilyName));
                //    dbServer.AddInParameter(command1, "GenderID", DbType.Int64, objPatientVO.SpouseDetails.GenderID);
                //    dbServer.AddInParameter(command1, "DateOfBirth", DbType.DateTime, objPatientVO.SpouseDetails.DateOfBirth);
                //    dbServer.AddInParameter(command1, "BloodGroupID", DbType.Int64, objPatientVO.SpouseDetails.BloodGroupID);
                //    dbServer.AddInParameter(command1, "MaritalStatusID", DbType.Int64, objPatientVO.SpouseDetails.MaritalStatusID);
                //    dbServer.AddInParameter(command1, "CivilID", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.CivilID));
                //    if (objPatientVO.SpouseDetails.ContactNo1 != null) objPatientVO.SpouseDetails.ContactNo1 = objPatientVO.SpouseDetails.ContactNo1.Trim();
                //    dbServer.AddInParameter(command1, "ContactNo1", DbType.String, objPatientVO.SpouseDetails.ContactNo1);
                //    if (objPatientVO.SpouseDetails.ContactNo2 != null) objPatientVO.SpouseDetails.ContactNo2 = objPatientVO.SpouseDetails.ContactNo2.Trim();
                //    dbServer.AddInParameter(command1, "ContactNo2", DbType.String, objPatientVO.SpouseDetails.ContactNo2);
                //    if (objPatientVO.SpouseDetails.FaxNo != null) objPatientVO.SpouseDetails.FaxNo = objPatientVO.SpouseDetails.FaxNo.Trim();
                //    dbServer.AddInParameter(command1, "FaxNo", DbType.String, objPatientVO.SpouseDetails.FaxNo);
                //    if (objPatientVO.SpouseDetails.Email != null) objPatientVO.SpouseDetails.Email = objPatientVO.SpouseDetails.Email.Trim();
                //    dbServer.AddInParameter(command1, "Email", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Email));
                //    if (objPatientVO.SpouseDetails.AddressLine1 != null) objPatientVO.SpouseDetails.AddressLine1 = objPatientVO.SpouseDetails.AddressLine1.Trim();
                //    dbServer.AddInParameter(command1, "AddressLine1", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.AddressLine1));
                //    if (objPatientVO.SpouseDetails.AddressLine2 != null) objPatientVO.SpouseDetails.AddressLine2 = objPatientVO.SpouseDetails.AddressLine2.Trim();
                //    dbServer.AddInParameter(command1, "AddressLine2", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.AddressLine2));
                //    if (objPatientVO.SpouseDetails.AddressLine3 != null) objPatientVO.SpouseDetails.AddressLine3 = objPatientVO.SpouseDetails.AddressLine3.Trim();
                //    dbServer.AddInParameter(command1, "AddressLine3", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.AddressLine3));
                //    if (objPatientVO.SpouseDetails.Country != null) objPatientVO.SpouseDetails.Country = objPatientVO.SpouseDetails.Country.Trim();
                //    dbServer.AddInParameter(command1, "Country", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Country));
                //    if (objPatientVO.SpouseDetails.State != null) objPatientVO.SpouseDetails.State = objPatientVO.SpouseDetails.State.Trim();
                //    dbServer.AddInParameter(command1, "State", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.State));
                //    if (objPatientVO.SpouseDetails.City != null) objPatientVO.SpouseDetails.City = objPatientVO.SpouseDetails.City.Trim();
                //    dbServer.AddInParameter(command1, "City", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.City));
                //    if (objPatientVO.SpouseDetails.Taluka != null) objPatientVO.SpouseDetails.Taluka = objPatientVO.SpouseDetails.Taluka.Trim();
                //    dbServer.AddInParameter(command1, "Taluka", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Taluka));
                //    if (objPatientVO.SpouseDetails.Area != null) objPatientVO.SpouseDetails.Area = objPatientVO.SpouseDetails.Area.Trim();
                //    dbServer.AddInParameter(command1, "Area", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Area));
                //    if (objPatientVO.SpouseDetails.District != null) objPatientVO.SpouseDetails.District = objPatientVO.SpouseDetails.District.Trim();
                //    dbServer.AddInParameter(command1, "District", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.District));
                //    dbServer.AddInParameter(command1, "MobileCountryCode", DbType.Int64, objPatientVO.SpouseDetails.MobileCountryCode);
                //    dbServer.AddInParameter(command1, "ResiNoCountryCode", DbType.Int64, objPatientVO.SpouseDetails.ResiNoCountryCode);
                //    dbServer.AddInParameter(command1, "ResiSTDCode", DbType.Int64, objPatientVO.SpouseDetails.ResiSTDCode);
                //    if (objPatientVO.SpouseDetails.Pincode != null) objPatientVO.SpouseDetails.Pincode = objPatientVO.SpouseDetails.Pincode.Trim();
                //    dbServer.AddInParameter(command1, "Pincode", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Pincode));
                //    dbServer.AddInParameter(command1, "ReligionID", DbType.Int64, objPatientVO.SpouseDetails.ReligionID);
                //    dbServer.AddInParameter(command1, "OccupationId", DbType.Int64, objPatientVO.SpouseDetails.OccupationId);
                //    dbServer.AddInParameter(command1, "Photo", DbType.Binary, objPatientVO.SpouseDetails.Photo);

                //    dbServer.AddInParameter(command1, "Status", DbType.Boolean, objPatientVO.SpouseDetails.Status);

                //    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, objPatientVO.SpouseDetails.UnitId);
                //    dbServer.AddInParameter(command1, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                //    dbServer.AddInParameter(command1, "UpdatedBy", DbType.Int64, UserVo.ID);
                //    dbServer.AddInParameter(command1, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //    dbServer.AddInParameter(command1, "UpdatedDateTime", DbType.DateTime, objPatientVO.SpouseDetails.UpdatedDateTime);
                //    dbServer.AddInParameter(command1, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                //    dbServer.AddInParameter(command1, "ID", DbType.Int64, objPatientVO.SpouseDetails.ID);
                //    dbServer.AddParameter(command1, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.SuccessStatus);

                //    int intStatus1 = dbServer.ExecuteNonQuery(command1);
                //    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                //}

            }
            catch (Exception ex)
            {

                trans.Rollback();
                throw ex;
            }
            finally
            {
                trans.Dispose();
                trans = null;
            }
            return BizActionObj;
        }

        // End Transaction 

        //* Added by - Ajit Jadhav
        //* Added Date - 29/8/2016
        //* Comments -  Save Pan Number T_Registration(Total Bill > 99999)
        private clsAddPatientBizActionVO UpdatePatientPanNumber(clsAddPatientBizActionVO BizActionObj, clsUserVO UserVo)
        {

            DbTransaction trans = null;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                clsPatientVO objPatientVO = BizActionObj.PatientDetails;

                con.Open();
                trans = con.BeginTransaction();
                if (BizActionObj.PatientDetails.IsPanNoSave == true)
                {

                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdatePatientPanNumber");

                    dbServer.AddInParameter(command, "UnitId", DbType.Int64, objPatientVO.UnitID);
                    dbServer.AddInParameter(command, "PanNumber", DbType.String, objPatientVO.PanNumber);
                    dbServer.AddInParameter(command, "ID", DbType.Int64, objPatientVO.ID);

                    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objPatientVO.UpdatedDateTime);
                    dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.SuccessStatus);

                    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {

                trans.Rollback();
                throw ex;
            }
            finally
            {
                trans.Dispose();
                trans = null;
            }
            return BizActionObj;
        }
        //***//------------
        public override IValueObject GetOTPatientGeneralDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetOTPatientGeneralDetailsListBizActionVO BizActionObj = valueObject as clsGetOTPatientGeneralDetailsListBizActionVO;
            try
            {
                int SearchFor = 0;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientGeneralDetailsListForSearch_OT_1");
                //DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientGeneralDetailsListForSearchNew");
                DbDataReader reader;

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");

                dbServer.AddInParameter(command, "IsCurrentAdmitted", DbType.Boolean, BizActionObj.IsCurrentAdmitted);

                //if (BizActionObj.OPDNo != null && BizActionObj.OPDNo.Length != 0)
                //    dbServer.AddInParameter(command, "OPDNo", DbType.String, BizActionObj.OPDNo);

                if (BizActionObj.OPDNo != null && BizActionObj.OPDNo.Length != 0)
                {
                    dbServer.AddInParameter(command, "OPDNo", DbType.String, BizActionObj.OPDNo + "%");
                }

                if (BizActionObj.IPDNo != null && BizActionObj.IPDNo.Length != 0)
                    dbServer.AddInParameter(command, "AdmissionNo", DbType.String, "%" + BizActionObj.IPDNo + "%");

                //if (!String.IsNullOrEmpty(BizActionObj.FirstName))
                //    dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.FirstName);

                //if (!String.IsNullOrEmpty(BizActionObj.LastName))
                //    dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.LastName);

                //if (BizActionObj.FamilyName != null && BizActionObj.FamilyName.Length != 0)
                //    dbServer.AddInParameter(command, "FamilyName", DbType.String, (BizActionObj.FamilyName));

                if (BizActionObj.MRNo != null && BizActionObj.MRNo.Length != 0)
                    dbServer.AddInParameter(command, "MrNo", DbType.String, "%" + BizActionObj.MRNo + "%");
                else
                    dbServer.AddInParameter(command, "MrNo", DbType.String, BizActionObj.MRNo + "%");

                if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                    dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.FirstName + "%");
                if (BizActionObj.MiddleName != null && BizActionObj.MiddleName.Length != 0)
                    dbServer.AddInParameter(command, "MiddleName", DbType.String, BizActionObj.MiddleName + "%");
                if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.LastName + "%");
                if (BizActionObj.FamilyName != null && BizActionObj.FamilyName.Length != 0)

                    if (BizActionObj.FamilyName != null && BizActionObj.FamilyName.Length != 0)
                        dbServer.AddInParameter(command, "FamilyName", DbType.String, BizActionObj.FamilyName + "%");

                //if (BizActionObj.MRNo != null)
                //    dbServer.AddInParameter(command, "MrNo", DbType.String, BizActionObj.MRNo);

                if (BizActionObj.ContactNo != null && BizActionObj.ContactNo.Length != 0)
                    dbServer.AddInParameter(command, "ContactNo1", DbType.String, BizActionObj.ContactNo + "%");

                if (BizActionObj.VisitWise == true)
                {
                    // dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "Serchfor", DbType.Int16, (Int16)1);
                    SearchFor = 1;
                }
                else if (BizActionObj.AdmissionWise == true && BizActionObj.RegistrationWise == false)
                {
                    // dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "Serchfor", DbType.Int16, (Int16)2);
                    SearchFor = 2;
                }
                else if (BizActionObj.RegistrationWise == true)
                {
                    dbServer.AddInParameter(command, "Serchfor", DbType.Int16, (Int16)0);
                    // dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    SearchFor = 0;
                }
                else
                {
                    dbServer.AddInParameter(command, "Serchfor", DbType.Int16, (Int16)0);
                    SearchFor = 0;
                    // dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                }

                if (BizActionObj.VisitFromDate != null)
                {
                    dbServer.AddInParameter(command, "VisitFromDate", DbType.DateTime, BizActionObj.VisitFromDate);
                    // dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                }
                if (BizActionObj.VisitToDate != null)
                {
                    if (BizActionObj.VisitFromDate != null)
                    {
                        BizActionObj.VisitToDate = BizActionObj.VisitToDate.Value.Date.AddDays(1);
                    }
                    dbServer.AddInParameter(command, "VisitToDate", DbType.DateTime, BizActionObj.VisitToDate);
                }
                if (BizActionObj.AdmissionFromDate != null)
                    dbServer.AddInParameter(command, "AdmissionFromDate", DbType.DateTime, BizActionObj.AdmissionFromDate);
                if (BizActionObj.AdmissionToDate != null)
                {
                    if (BizActionObj.AdmissionToDate != null)
                    {
                        BizActionObj.AdmissionToDate = BizActionObj.AdmissionToDate.Value.Date.AddDays(1);
                    }
                    dbServer.AddInParameter(command, "AdmissionToDate", DbType.DateTime, BizActionObj.AdmissionToDate);
                }
                if (BizActionObj.FromDate != null)
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                if (BizActionObj.ToDate != null)
                {
                    if (BizActionObj.ToDate != null)
                    {
                        BizActionObj.ToDate = BizActionObj.ToDate.Value.Date;
                    }
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                }
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);

                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.PatientDetailsList == null)
                        BizActionObj.PatientDetailsList = new List<clsPatientGeneralVO>();
                    while (reader.Read())
                    {
                        clsPatientGeneralVO objPatientVO = new clsPatientGeneralVO();

                        objPatientVO.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));

                        objPatientVO.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));

                        objPatientVO.FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]));

                        objPatientVO.MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"]));

                        objPatientVO.LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]));

                        //objPatientVO.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));

                        objPatientVO.FamilyName = Convert.ToString(DALHelper.HandleDBNull(reader["FamilyName"]));

                        objPatientVO.DateOfBirth = (DateTime?)DALHelper.HandleDate(reader["DateofBirth"]);

                        objPatientVO.RegistrationDate = Convert.ToDateTime(DALHelper.HandleDate(reader["RegistrationDate"]));

                        objPatientVO.ContactNO1 = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNO1"]));

                        objPatientVO.Gender = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"]));

                        objPatientVO.MaritalStatus = Convert.ToString(DALHelper.HandleDBNull(reader["MaritalStatus"]));

                        //objPatientVO.Education = Convert.ToString(DALHelper.HandleDBNull(reader["Education"]));

                        objPatientVO.Religion = Convert.ToString(DALHelper.HandleDBNull(reader["Religion"]));

                        objPatientVO.Photo = (Byte[])DALHelper.HandleDBNull(reader["Photo"]);

                        //added by neena
                        string ImgPath = Convert.ToString(DALHelper.HandleDBNull(reader["ImagePath"]));

                        objPatientVO.ImageName = "https://" + ImgIP + "/" + ImgVirtualDir + "/" + ImgPath;
                        //                      


                        //objPatientVO.VisitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitID"]));
                        //objPatientVO.DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"]));
                        //objPatientVO.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                        //objPatientVO.DepartmentName = Convert.ToString(DALHelper.HandleDBNull(reader["DepartmentName"]));

                        objPatientVO.LinkServer = BizActionObj.LinkServer;

                        BizActionObj.PatientDetailsList.Add(objPatientVO);
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
            return valueObject;
        }


        public override IValueObject GetPatientGeneralDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientGeneralDetailsListBizActionVO BizActionObj = valueObject as clsGetPatientGeneralDetailsListBizActionVO;
            try
            {
                if (BizActionObj.IsFromFindPatient == true)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientGeneralDetailsListForSearch_new");
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                    dbServer.AddInParameter(command, "SearchUnitID", DbType.Int64, BizActionObj.SearchUnitID);
                    dbServer.AddInParameter(command, "IsVisitPatient", DbType.Boolean, BizActionObj.VisitWise);
                    dbServer.AddInParameter(command, "VisitFromDate", DbType.DateTime, BizActionObj.VisitFromDate);
                    if (BizActionObj.VisitFromDate != null)
                    {
                        BizActionObj.VisitToDate = BizActionObj.VisitToDate.Value.Date.AddDays(1);
                    }
                    dbServer.AddInParameter(command, "VisitToDate", DbType.DateTime, BizActionObj.VisitToDate);
                    dbServer.AddInParameter(command, "RegistFromDate", DbType.DateTime, BizActionObj.FromDate);
                    if (BizActionObj.ToDate != null)
                    {

                        BizActionObj.ToDate = BizActionObj.ToDate.Value.Date.AddDays(1);
                    }
                    dbServer.AddInParameter(command, "RegistToDate", DbType.DateTime, BizActionObj.ToDate);
                    dbServer.AddInParameter(command, "Firstname", DbType.String, BizActionObj.FirstName);
                    dbServer.AddInParameter(command, "FamilyName", DbType.String, BizActionObj.FamilyName);
                    dbServer.AddInParameter(command, "Lastname", DbType.String, BizActionObj.LastName);
                    dbServer.AddInParameter(command, "AddressLine", DbType.String, BizActionObj.AddressLine1);
                    dbServer.AddInParameter(command, "MRNo", DbType.String, BizActionObj.MRNo);
                    //dbServer.AddInParameter(command, "DOB", DbType.DateTime, BizActionObj.DOB); //Commented by AniketK on 11Feb2019
                    dbServer.AddInParameter(command, "ReferenceNo", DbType.String, BizActionObj.ReferenceNo);
                    dbServer.AddInParameter(command, "ContactNo", DbType.String, BizActionObj.ContactNo);
                    dbServer.AddInParameter(command, "DOBFromDate", DbType.String, BizActionObj.DOBFromDate); //Added by AniketK on 11Feb2019
                    dbServer.AddInParameter(command, "DOBToDate", DbType.String, BizActionObj.DOBToDate); //Added by AniketK on 11Feb2019

                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                    dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.PatientDetailsList == null)
                            BizActionObj.PatientDetailsList = new List<clsPatientGeneralVO>();
                        while (reader.Read())
                        {
                            clsPatientGeneralVO objPatientVO = new clsPatientGeneralVO();
                            objPatientVO.PatientID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            objPatientVO.PatientUnitID = (long)DALHelper.HandleDBNull(reader["UnitId"]);
                            objPatientVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitId"]);
                            objPatientVO.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                            objPatientVO.LastName = (string)DALHelper.HandleDBNull(reader["LastName"]);
                            objPatientVO.FirstName = (string)DALHelper.HandleDBNull(reader["FirstName"]);
                            objPatientVO.AddressLine1 = (String)DALHelper.HandleDBNull(reader["AddressLine1"]);
                            objPatientVO.MiddleName = (string)DALHelper.HandleDBNull(reader["MiddleName"]);
                            objPatientVO.DateOfBirth = (DateTime?)DALHelper.HandleDate(reader["DateofBirth"]);
                            objPatientVO.Age = Convert.ToInt32(DALHelper.HandleDBNull(reader["Age"]));

                            objPatientVO.RegistrationDate = (DateTime)DALHelper.HandleDate(reader["RegistrationDate"]);
                            if (BizActionObj.VisitWise == true || BizActionObj.RegistrationWise == true)
                            {
                                objPatientVO.VisitID = (long)DALHelper.HandleDBNull(reader["VAID"]);
                                objPatientVO.VisitUnitID = (long)DALHelper.HandleDBNull(reader["VisitUnitID"]);
                                objPatientVO.VisitUnitId = (long)DALHelper.HandleDBNull(reader["VisitUnitID"]);
                                objPatientVO.OPDNO = (string)DALHelper.HandleDBNull(reader["OINO"]);
                                objPatientVO.Unit = Convert.ToString(reader["UnitName"]);
                            }
                            else
                            {
                                objPatientVO.IPDAdmissionID = (long)DALHelper.HandleDBNull(reader["VAID"]);
                                objPatientVO.IPDAdmissionNo = (string)DALHelper.HandleDBNull(reader["OINO"]);
                            }
                            if (BizActionObj.VisitWise == true)
                                objPatientVO.PatientKind = PatientsKind.OPD;
                            else if (BizActionObj.AdmissionWise == true)
                                objPatientVO.PatientKind = PatientsKind.IPD;
                            else if (objPatientVO.VisitID == 0 && objPatientVO.IPDAdmissionID == 0)
                                objPatientVO.PatientKind = PatientsKind.Registration;
                            objPatientVO.TariffID = (long)DALHelper.HandleDBNull(reader["TariffID"]);
                            objPatientVO.PatientSourceID = (long)DALHelper.HandleDBNull(reader["PatientSourceID"]);
                            objPatientVO.CompanyID = (long)DALHelper.HandleDBNull(reader["CompanyID"]);
                            objPatientVO.ContactNO1 = (string)DALHelper.HandleDBNull(reader["ContactNO1"]);
                            objPatientVO.Gender = (string)DALHelper.HandleDBNull(reader["Gender"]);
                            objPatientVO.MaritalStatus = (string)DALHelper.HandleDBNull(reader["MaritalStatus"]);
                            objPatientVO.UniversalID = Security.base64Decode((string)DALHelper.HandleDBNull(reader["CivilID"]));
                            objPatientVO.PatientTypeID = (long)DALHelper.HandleDBNull(reader["PatientCategoryID"]);
                            objPatientVO.MaritalStatus = (string)DALHelper.HandleDBNull(reader["MaritalStatus"]);
                            objPatientVO.ReferenceNo = (string)DALHelper.HandleDBNull(reader["ReferenceNo"]);

                            objPatientVO.LinkServer = BizActionObj.LinkServer;

                            if (BizActionObj.ISFromQueeManagment == false && BizActionObj.isfromMaterialConsumpation == false)
                                objPatientVO.BabyWeight = Convert.ToString(DALHelper.HandleDBNull(reader["BabyWeight"]));       // For Pediatric Flow

                            BizActionObj.PatientDetailsList.Add(objPatientVO);
                        }
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");

                    reader.Close();
                }

                else if (BizActionObj.isfromCouterSaleStaff == true)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetStaffGeneralDetailsListForSearch");
                    
                    if (BizActionObj.EmpNO != null )
                        dbServer.AddInParameter(command, "EmpNO", DbType.String,BizActionObj.EmpNO );

                    if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                        dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.FirstName + "%");

                    if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                        dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.LastName + "%");

                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                    dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.PatientDetailsList == null)
                            BizActionObj.PatientDetailsList = new List<clsPatientGeneralVO>();
                        while (reader.Read())
                        {
                            clsPatientGeneralVO objPatientVO = new clsPatientGeneralVO();

                            objPatientVO.PatientID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            objPatientVO.PatientUnitID = (long)DALHelper.HandleDBNull(reader["UnitId"]);
                            objPatientVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitId"]);
                            objPatientVO.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                            objPatientVO.LastName = (string)DALHelper.HandleDBNull(reader["LastName"]);
                            objPatientVO.FirstName = (string)DALHelper.HandleDBNull(reader["FirstName"]);
                            objPatientVO.DateOfBirth = (DateTime?)DALHelper.HandleDate(reader["DateofBirth"]);

                            objPatientVO.Gender = (string)DALHelper.HandleDBNull(reader["Gender"]);
                            objPatientVO.GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]));
                            objPatientVO.Designation = (string)DALHelper.HandleDBNull(reader["Designation"]);
                            objPatientVO.DepartmentName = (string)DALHelper.HandleDBNull(reader["DepartmentName"]);

                            BizActionObj.PatientDetailsList.Add(objPatientVO);
                        }

                        reader.NextResult();
                        BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");

                        reader.Close();
                    }

                }

                else
                {
                    # region SP for OPD

                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientGeneralDetailsListForSearch");
                    DbDataReader reader;

                    dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                    if (BizActionObj.MRNo != null && BizActionObj.MRNo.Length != 0)
                        dbServer.AddInParameter(command, "MrNo", DbType.String, "%" + BizActionObj.MRNo + "%");
                    else
                        dbServer.AddInParameter(command, "MrNo", DbType.String, BizActionObj.MRNo + "%");

                    //else
                    //    dbServer.AddInParameter(command, "@DonarCode", DbType.String, BizActionObj.DonorCode + "%");
                    //-----------------

                    //if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                    //    dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(BizActionObj.FirstName) + "%");
                    //if (BizActionObj.MiddleName != null && BizActionObj.MiddleName.Length != 0)
                    //    dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(BizActionObj.MiddleName) + "%");
                    //if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    //    dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(BizActionObj.LastName) + "%");
                    if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                        dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.FirstName + "%");
                    if (BizActionObj.AddressLine1 != null && BizActionObj.AddressLine1.Length != 0)
                        dbServer.AddInParameter(command, "AddressLine1", DbType.String, Security.base64Encode(BizActionObj.AddressLine1) + "%");
                    if (BizActionObj.MiddleName != null && BizActionObj.MiddleName.Length != 0)
                        dbServer.AddInParameter(command, "MiddleName", DbType.String, BizActionObj.MiddleName + "%");
                    if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                        dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.LastName + "%");
                    if (BizActionObj.FamilyName != null && BizActionObj.FamilyName.Length != 0)

                        if (BizActionObj.FamilyName != null && BizActionObj.FamilyName.Length != 0)
                            dbServer.AddInParameter(command, "FamilyName", DbType.String, BizActionObj.FamilyName + "%");
                    if (BizActionObj.OPDNo != null && BizActionObj.OPDNo.Length != 0)
                    {
                        if (BizActionObj.VisitWise == true)
                            dbServer.AddInParameter(command, "OPDNo", DbType.String, BizActionObj.OPDNo + "%");
                        else if (BizActionObj.AdmissionWise == true)
                            dbServer.AddInParameter(command, "AdmissionNo", DbType.String, BizActionObj.OPDNo + "%");
                        else if (BizActionObj.isfromMaterialConsumpation == true)  //Added by AJ Date 9/1/2017
                            dbServer.AddInParameter(command, "OPDNo", DbType.String, BizActionObj.OPDNo + "%");
                    }

                    if (BizActionObj.ContactNo != null && BizActionObj.ContactNo.Length != 0)
                        dbServer.AddInParameter(command, "ContactNo", DbType.String, BizActionObj.ContactNo + "%");
                    //added by neena
                    if (BizActionObj.DonarCode != null && BizActionObj.DonarCode.Length != 0)
                        dbServer.AddInParameter(command, "DonarCode", DbType.String, "%" + BizActionObj.DonarCode + "%");
                    if (BizActionObj.OldRegistrationNo != null && BizActionObj.OldRegistrationNo.Length != 0)
                        dbServer.AddInParameter(command, "OldRegistrationNo", DbType.String, BizActionObj.OldRegistrationNo + "%");
                    //

                    if (BizActionObj.Country != null && BizActionObj.Country.Length != 0)
                        dbServer.AddInParameter(command, "Country", DbType.String, Security.base64Encode(BizActionObj.Country) + "%");
                    if (BizActionObj.State != null && BizActionObj.State.Length != 0)
                        dbServer.AddInParameter(command, "State", DbType.String, Security.base64Encode(BizActionObj.State) + "%");
                    if (BizActionObj.City != null && BizActionObj.City.Length != 0)
                        dbServer.AddInParameter(command, "City", DbType.String, Security.base64Encode(BizActionObj.City) + "%");
                    if (BizActionObj.Pincode != null && BizActionObj.Pincode.Length != 0)
                        dbServer.AddInParameter(command, "Pincode", DbType.String, Security.base64Encode(BizActionObj.Pincode) + "%");
                    if (BizActionObj.CivilID != null && BizActionObj.CivilID.Length != 0)
                        dbServer.AddInParameter(command, "CivilID", DbType.String, Security.base64Encode(BizActionObj.CivilID) + "%");
                    if (BizActionObj.ReferenceNo != null && BizActionObj.ReferenceNo.Length != 0)
                        dbServer.AddInParameter(command, "ReferenceNo", DbType.String, BizActionObj.ReferenceNo);
                    if (BizActionObj.GenderID != null && BizActionObj.GenderID != 0)
                        dbServer.AddInParameter(command, "GenderID", DbType.Int64, BizActionObj.GenderID);
                    if (BizActionObj.PatientCategoryID != null && BizActionObj.PatientCategoryID != 0)
                        dbServer.AddInParameter(command, "PatientCategory", DbType.Int64, BizActionObj.PatientCategoryID);

                    if (BizActionObj.DOB != null)
                        dbServer.AddInParameter(command, "DOB", DbType.DateTime, BizActionObj.DOB);
                    else
                        dbServer.AddInParameter(command, "DOB", DbType.DateTime, null);

                    if (BizActionObj.FromDate != null)
                    {
                        BizActionObj.FromDate = BizActionObj.FromDate.Value.Date;
                        dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                    }

                    if (BizActionObj.ToDate != null)
                    {
                        if (BizActionObj.FromDate != null)
                        {
                            //if (BizActionObj.FromDate.Equals(BizActionObj.ToDate))
                            BizActionObj.ToDate = BizActionObj.ToDate.Value.Date.AddDays(1);
                        }

                        dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                    }

                    if (BizActionObj.VisitWise == true)
                    {
                        dbServer.AddInParameter(command, "Serchfor", DbType.Int16, (Int16)1);

                        if (BizActionObj.VisitFromDate != null)
                        {
                            BizActionObj.VisitFromDate = BizActionObj.VisitFromDate.Value.Date;
                            dbServer.AddInParameter(command, "VisitFromDate", DbType.DateTime, BizActionObj.VisitFromDate);
                        }
                        if (BizActionObj.VisitToDate != null)
                        {
                            if (BizActionObj.VisitFromDate != null)
                            {
                                BizActionObj.VisitToDate = BizActionObj.VisitToDate.Value.Date.AddDays(1);
                            }
                            dbServer.AddInParameter(command, "VisitToDate", DbType.DateTime, BizActionObj.VisitToDate);
                        }
                    }

                    if (BizActionObj.AdmissionWise == true)
                    {
                        dbServer.AddInParameter(command, "Serchfor", DbType.Int16, (Int16)2);

                        if (BizActionObj.AdmissionFromDate != null)
                            dbServer.AddInParameter(command, "AdmissionFromDate", DbType.DateTime, BizActionObj.AdmissionFromDate);
                        if (BizActionObj.AdmissionToDate != null)
                        {
                            if (BizActionObj.AdmissionToDate != null)
                            {
                                BizActionObj.AdmissionToDate = BizActionObj.AdmissionToDate.Value.Date.AddDays(1);
                            }
                            dbServer.AddInParameter(command, "AdmissionToDate", DbType.DateTime, BizActionObj.AdmissionToDate);
                        }
                    }
                    //=====================================================
                    if (BizActionObj.DOBWise == true)
                    {
                        dbServer.AddInParameter(command, "Serchfor", DbType.Int16, (Int16)0);

                        if (BizActionObj.DOBFromDate != null)
                            dbServer.AddInParameter(command, "DOBFromDate", DbType.DateTime, BizActionObj.DOBFromDate);
                        if (BizActionObj.DOBToDate != null)
                        {
                            if (BizActionObj.DOBToDate != null)
                            {
                                BizActionObj.DOBToDate = BizActionObj.DOBToDate.Value.Date.AddDays(1);
                            }
                            dbServer.AddInParameter(command, "DOBToDate", DbType.DateTime, BizActionObj.DOBToDate);
                        }
                    }
                    //===================================================== For Not Search Donor Spouse =======
                    if(BizActionObj.ISDonorSerch == true)
                    {
                        dbServer.AddInParameter(command, "DonorSerch", DbType.Boolean, BizActionObj.ISDonorSerch);
                    }

                    //-====================================================

                    if (BizActionObj.IsLoyaltyMember == true)
                    {
                        dbServer.AddInParameter(command, "IsLoyaltyMember", DbType.Boolean, BizActionObj.IsLoyaltyMember);
                        dbServer.AddInParameter(command, "LoyaltyProgramID", DbType.Int64, BizActionObj.LoyaltyProgramID);
                    }
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                    dbServer.AddInParameter(command, "QueueUnitID", DbType.Int64, BizActionObj.PQueueUnitID);
                    dbServer.AddInParameter(command, "RegistrationTypeID", DbType.Int64, BizActionObj.RegistrationTypeID);

                    if (BizActionObj.PQueueUnitID != null && BizActionObj.PQueueUnitID > 0)
                    {
                        dbServer.AddInParameter(command, "FindPatientVisitUnitID", DbType.Int64, BizActionObj.PQueueUnitID);
                    }
                    else
                    {
                        dbServer.AddInParameter(command, "FindPatientVisitUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    }

                    if (BizActionObj.SearchInAnotherClinic == true)
                    {
                        dbServer.AddInParameter(command, "SearchInAnotherClinic", DbType.Boolean, BizActionObj.SearchInAnotherClinic);
                    }

                    //dbServer.AddInParameter(command, "IncludeSpouse", DbType.Boolean, BizActionObj.IncludeSpouse);
                    //By Anjali.....................
                    dbServer.AddInParameter(command, "IdentityID", DbType.Int64, BizActionObj.IdentityID);
                    dbServer.AddInParameter(command, "IdentityNumber", DbType.String, BizActionObj.IdentityNumber);
                    dbServer.AddInParameter(command, "SpecialRegID", DbType.Int64, BizActionObj.SpecialRegID);

                    dbServer.AddInParameter(command, "IsFromQueeManagment", DbType.Boolean, BizActionObj.ISFromQueeManagment);
                    dbServer.AddInParameter(command, "ShowOutSourceDonor", DbType.Boolean, BizActionObj.ShowOutSourceDonor);
                    dbServer.AddInParameter(command, "IsFromSurrogacyModule", DbType.Boolean, BizActionObj.IsFromSurrogacyModule);
                    dbServer.AddInParameter(command, "IsSelfAndDonor", DbType.Int32, BizActionObj.IsSelfAndDonor);
                    //................................
                    //Added by AJ Date 29/12/2016
                    dbServer.AddInParameter(command, "isfromMaterialConsumpation", DbType.Boolean, BizActionObj.isfromMaterialConsumpation);
                    //---------------------------

                    dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.StoreID);

                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                    dbServer.AddInParameter(command, "LinkServer", DbType.String, BizActionObj.LinkServer);
                    if (BizActionObj.LinkServer != null)
                        dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, BizActionObj.LinkServer.Replace(@"\", "_"));

                    dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                    reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.PatientDetailsList == null)
                            BizActionObj.PatientDetailsList = new List<clsPatientGeneralVO>();
                        while (reader.Read())
                        {
                            clsPatientGeneralVO objPatientVO = new clsPatientGeneralVO();

                            objPatientVO.PatientID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            objPatientVO.PatientUnitID = (long)DALHelper.HandleDBNull(reader["UnitId"]);
                            objPatientVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitId"]);
                            objPatientVO.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                            //objPatientVO.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                            //objPatientVO.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                            //objPatientVO.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));

                            //Added by AJ Date 27/12/2016
                            if (BizActionObj.isfromMaterialConsumpation == true)
                            {
                                objPatientVO.PatientID = (long)DALHelper.HandleDBNull(reader["PatientId"]);
                                objPatientVO.PatientUnitID = (long)DALHelper.HandleDBNull(reader["PatientUnitID"]);
                                objPatientVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitId"]);
                                objPatientVO.Opd_Ipd_External_Id = (long)DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"]);
                                objPatientVO.Opd_Ipd_External_UnitId = (long)DALHelper.HandleDBNull(reader["Opd_Ipd_External_UnitId"]);
                                objPatientVO.Opd_Ipd_External = Convert.ToInt32(DALHelper.HandleDBNull(reader["Opd_Ipd_External"]));
                                objPatientVO.OPDNO = Convert.ToString(DALHelper.HandleDBNull(reader["OPD_IPD_NO"])); //date 9/1/2017
                                objPatientVO.IsPatientIndentReceiveExists = Convert.ToInt32(DALHelper.HandleDBNull(reader["IsPatientIndentReceiveExists"]));
                            }

                            //***/---------------------

                            objPatientVO.LastName = (string)DALHelper.HandleDBNull(reader["LastName"]);
                            objPatientVO.FirstName = (string)DALHelper.HandleDBNull(reader["FirstName"]);
                            objPatientVO.MiddleName = (string)DALHelper.HandleDBNull(reader["MiddleName"]);
                            objPatientVO.AddressLine1 = (String)DALHelper.HandleDBNull(reader["AddressLine1"]);
                            objPatientVO.RegType = Convert.ToInt16(DALHelper.HandleDBNull(reader["RegType"]));
                            objPatientVO.DateOfBirth = (DateTime?)DALHelper.HandleDate(reader["DateofBirth"]);
                            objPatientVO.Age = Convert.ToInt32(DALHelper.HandleDBNull(reader["Age"]));
                            objPatientVO.RegistrationDate = (DateTime)DALHelper.HandleDate(reader["RegistrationDate"]);
                            if (BizActionObj.VisitWise == true )
                            {
                                objPatientVO.VisitID = (long)DALHelper.HandleDBNull(reader["VAID"]);
                                objPatientVO.VisitUnitID = (long)DALHelper.HandleDBNull(reader["VisitUnitID"]);
                                objPatientVO.VisitUnitId = (long)DALHelper.HandleDBNull(reader["VisitUnitID"]);
                                objPatientVO.OPDNO = (string)DALHelper.HandleDBNull(reader["OINO"]);
                                objPatientVO.Unit = Convert.ToString(reader["UnitName"]);

                                objPatientVO.FemaleName = (string)DALHelper.HandleDBNull(reader["FemaleName"]);
                                objPatientVO.MaleName = (string)DALHelper.HandleDBNull(reader["MaleName"]);                              
                                objPatientVO.FemaleAge = Convert.ToInt32(DALHelper.HandleDBNull(reader["FemaleAge"]));
                                objPatientVO.MaleAge = Convert.ToInt32(DALHelper.HandleDBNull(reader["MaleAge"]));

                            }
                           if( BizActionObj.RegistrationWise == true )
                            {
                                objPatientVO.FemaleName = (string)DALHelper.HandleDBNull(reader["FemaleName"]);
                                objPatientVO.MaleName = (string)DALHelper.HandleDBNull(reader["MaleName"]);
                                objPatientVO.FemaleAge = Convert.ToInt32(DALHelper.HandleDBNull(reader["FemaleAge"]));
                                objPatientVO.MaleAge = Convert.ToInt32(DALHelper.HandleDBNull(reader["MaleAge"]));
                            }

                            if (BizActionObj.VisitWise == true)
                                objPatientVO.PatientKind = PatientsKind.OPD;
                            else if (BizActionObj.AdmissionWise == true)
                                objPatientVO.PatientKind = PatientsKind.IPD;
                            else if (objPatientVO.VisitID == 0 && objPatientVO.IPDAdmissionID == 0)
                                objPatientVO.PatientKind = PatientsKind.Registration;

                            objPatientVO.TariffID = (long)DALHelper.HandleDBNull(reader["TariffID"]);
                            objPatientVO.PatientSourceID = (long)DALHelper.HandleDBNull(reader["PatientSourceID"]);
                            objPatientVO.CompanyID = (long)DALHelper.HandleDBNull(reader["CompanyID"]);
                            objPatientVO.ContactNO1 = (string)DALHelper.HandleDBNull(reader["ContactNO1"]);
                            objPatientVO.Gender = (string)DALHelper.HandleDBNull(reader["Gender"]);
                            objPatientVO.MaritalStatus = (string)DALHelper.HandleDBNull(reader["MaritalStatus"]);
                            objPatientVO.UniversalID = Security.base64Decode((string)DALHelper.HandleDBNull(reader["CivilID"]));
                            objPatientVO.PatientTypeID = (long)DALHelper.HandleDBNull(reader["PatientCategoryID"]);
                            objPatientVO.RelationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RelationID"])); //***//
                            // Added By CDS 
                            objPatientVO.GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]));
                            objPatientVO.NewPatientCategoryID = (long)DALHelper.HandleDBNull(reader["NewPatientCategoryID"]);
                            // Added By CDS 
                            objPatientVO.ReferenceNo = (string)DALHelper.HandleDBNull(reader["ReferenceNo"]);
                            //objPatientVO.RegistrationType = (string)DALHelper.HandleDBNull(reader["RegistrationType"]);       // ..................BHUSHAN
                            //objPatientVO.SpouseID = (long)DALHelper.HandleDBNull(reader["SpouseID"]);
                            objPatientVO.City = (string)DALHelper.HandleDBNull(reader["CityNew"]);
                            objPatientVO.DonorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DonorCode"]));
                            objPatientVO.IsSurrogateAlreadyLinked = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSurrogateAlreadyLinked"]));

                            //added by neena
                            objPatientVO.DonarCode = Convert.ToString(DALHelper.HandleDBNull(reader["DonarCode"]));
                            objPatientVO.OldRegistrationNo = Convert.ToString(DALHelper.HandleDBNull(reader["OldRegistrationNo"]));
                            objPatientVO.IsDonor = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDonor"]));
                            //
                            // added by Ashish Z. on dated 230716
                            objPatientVO.IsReferralDoc = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsReferralDoc"]));
                            objPatientVO.ReferralTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferralTypeID"]));
                            objPatientVO.ReferralDoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferralDoctorID"]));
                           
                            ///Commeted by Narendra oct 2024
                            //if (BizActionObj.VisitWise == true || BizActionObj.RegistrationWise == true)
                            //{
                            //    objPatientVO.CoConsultantDoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoConsultantDoctorID"])); //***//19
                            //}

                            objPatientVO.IsAge = (bool)DALHelper.HandleBoolDBNull(reader["IsAge"]);
                            if (objPatientVO.IsAge == true)
                            {
                                objPatientVO.DateOfBirthFromAge = (DateTime?)DALHelper.HandleDate(reader["DateOfBirth"]);
                            }
                            else
                            {
                                objPatientVO.DateOfBirth = (DateTime?)DALHelper.HandleDate(reader["DateOfBirth"]);
                            }

                            objPatientVO.EducationID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["EducationID"]));
                            objPatientVO.BloodGroupID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["BloodGroupID"]));
                            string ImgPath = Convert.ToString(DALHelper.HandleDBNull(reader["ImagePath"]));

                            if (!string.IsNullOrEmpty(ImgPath))
                                objPatientVO.ImageName = "https://" + ImgIP + "/" + ImgVirtualDir + "/" + ImgPath;
                            objPatientVO.IsDocAttached = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDocAttached"]));
                            objPatientVO.Email = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["Email"])));
                            objPatientVO.IdentityType = Convert.ToString(DALHelper.HandleDBNull(reader["IdentityType"]));
                            objPatientVO.IdentityNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IdentityNumber"]));
                            objPatientVO.RemarkForPatientType = Convert.ToString(DALHelper.HandleDBNull(reader["RemarkForPatientType"]));
                            objPatientVO.SpecialRegName = Convert.ToString(DALHelper.HandleDBNull(reader["SpecialReg"]));
                            if (objPatientVO.RegType == 0)
                                objPatientVO.RegistrationType = "OPD";
                            else if (objPatientVO.RegType == 1)
                                objPatientVO.RegistrationType = "IPD";
                            else if (objPatientVO.RegType == 2)
                                objPatientVO.RegistrationType = "Pharmacy";
                            else if (objPatientVO.RegType == 5)
                                objPatientVO.RegistrationType = "Pathology";

                            //Added by AJ Date 27/12/2016
                            if (BizActionObj.isfromMaterialConsumpation == false)
                            {
                                objPatientVO.PanNumber = Convert.ToString(DALHelper.HandleDBNull(reader["PanNumber"]));  //* Added by - Ajit Jadhav  Date - 25/8/2016
                                objPatientVO.Email = Convert.ToString(DALHelper.HandleDBNull(reader["Email"]));
                                objPatientVO.NationalityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["NationalityId"])); // Added by ajit jadhav Date 13/10/2016
                                objPatientVO.IsSpouse = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSpouse"]));
                            }

                            if (BizActionObj.ISFromQueeManagment == true)
                            {
                                objPatientVO.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["ReferralDoctorName"]));
                                objPatientVO.Camp = Convert.ToString(DALHelper.HandleDBNull(reader["Camp"]));
                                objPatientVO.SourceofReference = Convert.ToString(DALHelper.HandleDBNull(reader["SourceofReference"]));                               
                            }
                            //***//---------------------                         

                            objPatientVO.LinkServer = BizActionObj.LinkServer;

                            if (BizActionObj.ISFromQueeManagment == false && BizActionObj.isfromMaterialConsumpation == false)
                                objPatientVO.BabyWeight = Convert.ToString(DALHelper.HandleDBNull(reader["BabyWeight"]));       // For Pediatric Flow                            

                            BizActionObj.PatientDetailsList.Add(objPatientVO);
                        }
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");

                    reader.Close();

                    #endregion
                }
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

        public override IValueObject GetPatient(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientBizActionVO BizActionObj = valueObject as clsGetPatientBizActionVO;
            DbCommand command;
            try
            {
                clsPatientVO objPatientVO = BizActionObj.PatientDetails;
                if (BizActionObj.PatientDetails.IsLatestPatient == true)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetLatestPatientByUnitID");
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetPatient");

                    //objPatientVO.LinkServer=@"SHSPL19\SQLEXPRESS";

                    dbServer.AddInParameter(command, "LinkServer", DbType.String, objPatientVO.GeneralDetails.LinkServer);
                    if (objPatientVO.GeneralDetails.LinkServer != null)
                        dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objPatientVO.GeneralDetails.LinkServer.Replace(@"\", "_"));
                    // dbServer.AddInParameter(command, "ID", DbType.Int64, objPatientVO.GeneralDetails.PatientID);
                    if (BizActionObj.SurrogateID == null || BizActionObj.SurrogateID == 0)
                    {
                        dbServer.AddInParameter(command, "ID", DbType.Int64, objPatientVO.GeneralDetails.PatientID);
                    }
                    else
                    {
                        dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.SurrogateID);
                    }
                    dbServer.AddInParameter(command, "MRNo", DbType.String, objPatientVO.GeneralDetails.MRNo);

                    dbServer.AddInParameter(command, "SearchFromIPD", DbType.String, objPatientVO.GeneralDetails.SearchFromIPD);

                }
                BizActionObj.BankDetails = new clsBankDetailsInfoVO();
                DbDataReader reader;
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objPatientVO.GeneralDetails.UnitId);
                // int intStatus = dbServer.ExecuteNonQuery(command);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizActionObj.PatientDetails.GeneralDetails.PatientTypeID = (long)DALHelper.HandleDBNull(reader["PatientCategoryID"]);
                        BizActionObj.PatientDetails.GeneralDetails.IsDocAttached = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDocAttached"]));
                        BizActionObj.PatientDetails.GeneralDetails.ReferralTypeID = (long)DALHelper.HandleDBNull(reader["ReferralTypeID"]);             // BY BHUSHAN . . .  .
                        BizActionObj.PatientDetails.CompanyName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["CompanyName"]));

                        BizActionObj.PatientDetails.GeneralDetails.PatientID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        BizActionObj.PatientDetails.GeneralDetails.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                        BizActionObj.PatientDetails.GeneralDetails.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                        BizActionObj.PatientDetails.GeneralDetails.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                        BizActionObj.PatientDetails.GeneralDetails.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                        BizActionObj.PatientDetails.GeneralDetails.RegistrationDate = (DateTime?)DALHelper.HandleDate(reader["RegistrationDate"]);
                        BizActionObj.PatientDetails.GeneralDetails.IsAge = (bool)DALHelper.HandleBoolDBNull(reader["IsAge"]);
                        if (BizActionObj.PatientDetails.GeneralDetails.IsAge == true)
                        {
                            BizActionObj.PatientDetails.GeneralDetails.DateOfBirthFromAge = (DateTime?)DALHelper.HandleDate(reader["DateOfBirth"]);
                        }
                        else
                        {
                            BizActionObj.PatientDetails.GeneralDetails.DateOfBirth = (DateTime?)DALHelper.HandleDate(reader["DateOfBirth"]);
                        }
                        BizActionObj.PatientDetails.GeneralDetails.PatientSourceID = (long)DALHelper.HandleDBNull(reader["PatientSourceID"]);
                        BizActionObj.PatientDetails.GeneralDetails.PatientSponsorCategoryID = (long)DALHelper.HandleDBNull(reader["PatientSponsorCategoryID"]);
                        BizActionObj.PatientDetails.GeneralDetails.TariffID = (long)DALHelper.HandleDBNull(reader["TariffID"]);
                        BizActionObj.PatientDetails.GeneralDetails.RegType = Convert.ToInt16(DALHelper.HandleDBNull(reader["RegType"]));

                        BizActionObj.PatientDetails.FamilyName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FamilyName"]));
                        BizActionObj.PatientDetails.GenderID = (Int64)DALHelper.HandleDBNull(reader["GenderID"]);
                        BizActionObj.PatientDetails.BloodGroupID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BloodGroupID"]));
                        BizActionObj.PatientDetails.MaritalStatusID = (Int64)DALHelper.HandleDBNull(reader["MaritalStatusID"]);
                        BizActionObj.PatientDetails.CivilID = Security.base64Decode((string)DALHelper.HandleDBNull(reader["CivilID"]));
                        BizActionObj.PatientDetails.ContactNo1 = (string)DALHelper.HandleDBNull(reader["ContactNo1"]);
                        BizActionObj.PatientDetails.ContactNo2 = (string)DALHelper.HandleDBNull(reader["ContactNo2"]);
                        BizActionObj.PatientDetails.MobileNo2 = (string)DALHelper.HandleDBNull(reader["MobileNo"]);
                        BizActionObj.PatientDetails.FaxNo = (String)DALHelper.HandleDBNull(reader["FaxNo"]);
                        BizActionObj.PatientDetails.Email = Security.base64Decode((String)DALHelper.HandleDBNull(reader["Email"]));
                        BizActionObj.PatientDetails.AddressLine1 = Security.base64Decode((String)DALHelper.HandleDBNull(reader["AddressLine1"]));
                        BizActionObj.PatientDetails.AddressLine2 = Security.base64Decode((String)DALHelper.HandleDBNull(reader["AddressLine2"]));
                        BizActionObj.PatientDetails.AddressLine3 = Security.base64Decode((String)DALHelper.HandleDBNull(reader["AddressLine3"]));
                        BizActionObj.PatientDetails.Country = Security.base64Decode((String)DALHelper.HandleDBNull(reader["Country"]));
                        BizActionObj.PatientDetails.State = Convert.ToString(DALHelper.HandleDBNull(reader["State"]));
                        BizActionObj.PatientDetails.City = Convert.ToString(DALHelper.HandleDBNull(reader["City"]));

                        BizActionObj.PatientDetails.Taluka = Security.base64Decode((string)DALHelper.HandleDBNull(reader["Taluka"]));
                        BizActionObj.PatientDetails.Area = Convert.ToString(DALHelper.HandleDBNull(reader["Area"]));
                        BizActionObj.PatientDetails.District = Security.base64Decode((string)DALHelper.HandleDBNull(reader["District"]));
                        BizActionObj.PatientDetails.MobileCountryCode = Convert.ToString(DALHelper.HandleDBNull(reader["MobileCountryCode"]));
                        BizActionObj.PatientDetails.ResiNoCountryCode = (long)DALHelper.HandleDBNull(reader["ResiNoCountryCode"]);
                        BizActionObj.PatientDetails.ResiSTDCode = (long)DALHelper.HandleDBNull(reader["ResiSTDCode"]);

                        //By Anjali........................
                        BizActionObj.PatientDetails.CountryID = (long)DALHelper.HandleIntegerNull(reader["CountryID"]);
                        BizActionObj.PatientDetails.StateID = (long)DALHelper.HandleIntegerNull(reader["StateID"]);
                        BizActionObj.PatientDetails.CityID = (long)DALHelper.HandleIntegerNull(reader["CityID"]);
                        BizActionObj.PatientDetails.RegionID = (long)DALHelper.HandleIntegerNull(reader["RegionID"]);
                        BizActionObj.PatientDetails.ReferralDoctorID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ReferralDoctorID"]));
                        BizActionObj.PatientDetails.CoConsultantDoctorID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["CoConsultantDoctorID"])); //***//19
                        BizActionObj.PatientDetails.IsReferralDoc = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsReferralDoc"]));
                        BizActionObj.PatientDetails.ReferredToDoctorID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ReferredToDoctorID"]));
                        BizActionObj.PatientDetails.ReferralDetail = Convert.ToString(DALHelper.HandleDBNull(reader["ReferralDetail"]));
                        BizActionObj.PatientDetails.ReferralTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferralTypeID"]));


                        BizActionObj.PatientDetails.PrefixId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PrefixId"]));
                        BizActionObj.PatientDetails.IdentityID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["IdentityID"]));
                        BizActionObj.PatientDetails.IdentityNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IdentityNumber"]));
                        BizActionObj.PatientDetails.IsInternationalPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsInternationalPatient"]));
                        BizActionObj.PatientDetails.RemarkForPatientType = Convert.ToString(DALHelper.HandleDBNull(reader["RemarkForPatientType"]));
                        BizActionObj.PatientDetails.Education = Convert.ToString(DALHelper.HandleDBNull(reader["Education"]));


                        //* Added by - Ajit Jadhav
                        //* Added Date - 11/8/2016
                        //* Comments - get the selected patient Bd name And Pan No
                        BizActionObj.PatientDetails.PanNumber = Convert.ToString(DALHelper.HandleDBNull(reader["PanNumber"]));
                        BizActionObj.PatientDetails.BDID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BDID"]));
                        BizActionObj.PatientDetails.NationalityID = (Int64)DALHelper.HandleDBNull(reader["NationalityID"]);   //* Added Date - 6/10/2016
                        //***//
                        BizActionObj.PatientDetails.CampID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CampID"]));
                        BizActionObj.PatientDetails.NoOfYearsOfMarriage = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfYearsOfMarriage"]));
                        BizActionObj.PatientDetails.NoOfExistingChildren = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfExistingChildren"]));
                        BizActionObj.PatientDetails.FamilyTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FamilyTypeID"]));

                        BizActionObj.PatientDetails.GeneralDetails.SourceofReference = Convert.ToString(DALHelper.HandleDBNull(reader["SourceofReference"]));
                        BizActionObj.PatientDetails.GeneralDetails.Camp = Convert.ToString(DALHelper.HandleDBNull(reader["Camp"]));
                        BizActionObj.PatientDetails.GeneralDetails.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["ReferralDoctorName"]));
                        BizActionObj.PatientDetails.AgentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgentID"]));
                        BizActionObj.PatientDetails.AgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgencyID"]));

                        //***// Bank Details

                        BizActionObj.BankDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientBankDetailsID"]));
                        BizActionObj.BankDetails.BankId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BankID"]));
                        BizActionObj.BankDetails.BranchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BranchID"]));
                        BizActionObj.BankDetails.IFSCCode = Convert.ToString(DALHelper.HandleDBNull(reader["IFSCCode"]));
                        BizActionObj.BankDetails.AccountTypeId = Convert.ToBoolean(DALHelper.HandleDBNull(reader["AccountType"]));
                        BizActionObj.BankDetails.AccountNumber = Convert.ToString(DALHelper.HandleDBNull(reader["AccountNo"]));
                        BizActionObj.BankDetails.AccountHolderName = Convert.ToString(DALHelper.HandleDBNull(reader["AccountHoldersName"]));

                        //***//-----------------------------

                        //Added by Ashish Z.
                        BizActionObj.PatientDetails.GeneralDetails.SonDaughterOf = Security.base64Decode((string)DALHelper.HandleDBNull(reader["DaughterOf"]));
                        BizActionObj.PatientDetails.NationalityID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["NationalityID"]));
                        BizActionObj.PatientDetails.PreferredLangID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PrefLangID"]));
                        BizActionObj.PatientDetails.TreatRequiredID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["TreatRequiredID"]));
                        BizActionObj.PatientDetails.EducationID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["EducationID"]));
                        BizActionObj.PatientDetails.MarriageAnnDate = (DateTime?)DALHelper.HandleDate(reader["MarriageAnnivDate"]);
                        BizActionObj.PatientDetails.NoOfPeople = Convert.ToInt32(DALHelper.HandleDBNull(reader["NoOfPeople"]));
                        BizActionObj.PatientDetails.IsClinicVisited = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsClinicVisited"]));
                        BizActionObj.PatientDetails.ClinicName = Convert.ToString(DALHelper.HandleDBNull(reader["ClinicName"]));
                        BizActionObj.PatientDetails.SpecialRegID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["SpecialRegID"]));



                        //------------
                        //Added by neena
                        BizActionObj.PatientDetails.CountryN = Convert.ToString(DALHelper.HandleDBNull(reader["CountryN"]));
                        BizActionObj.PatientDetails.StateN = Convert.ToString(DALHelper.HandleDBNull(reader["State"]));
                        BizActionObj.PatientDetails.CityN = Convert.ToString(DALHelper.HandleDBNull(reader["City"]));
                        BizActionObj.PatientDetails.Region = Convert.ToString(DALHelper.HandleDBNull(reader["Area"]));
                        BizActionObj.PatientDetails.CountryCode = Convert.ToString(DALHelper.HandleDBNull(reader["CountryCode"]));
                        BizActionObj.PatientDetails.StateCode = Convert.ToString(DALHelper.HandleDBNull(reader["StateCode"]));
                        BizActionObj.PatientDetails.CityCode = Convert.ToString(DALHelper.HandleDBNull(reader["CityCode"]));
                        BizActionObj.PatientDetails.RegionCode = Convert.ToString(DALHelper.HandleDBNull(reader["RegionCode"]));

                        //BizActionObj.PatientDetails.ImageName = Convert.ToString(DALHelper.HandleDBNull(reader["ImgPath"]));
                        string ImgPath = Convert.ToString(DALHelper.HandleDBNull(reader["ImgPath"]));

                        if (!string.IsNullOrEmpty(ImgPath))
                            BizActionObj.PatientDetails.ImageName = "https://" + ImgIP + "/" + ImgVirtualDir + "/" + ImgPath;
                        // BizActionObj.PatientDetails.GeneralDetails.ImageName = "https://" + ImgIP + "/" + ImgVirtualDir + "/" + ImgPath;


                        BizActionObj.PatientDetails.OldRegistrationNo = Convert.ToString(DALHelper.HandleDBNull(reader["OldRegistrationNo"]));
                        //




                        //...................................

                        BizActionObj.PatientDetails.Pincode = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["Pincode"])));
                        BizActionObj.PatientDetails.ReligionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReligionID"]));
                        BizActionObj.PatientDetails.OccupationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["OccupationId"]));

                        //  BizActionObj.PatientDetails.Photo = (Byte[])DALHelper.HandleDBNull(reader["Photo"]);
                        //    if (BizActionObj.PatientDetails.Photo != null)
                        if ((Byte[])DALHelper.HandleDBNull(reader["Photo"]) != null)
                        {
                            BizActionObj.PatientDetails.Photo = (Byte[])DALHelper.HandleDBNull(reader["Photo"]);
                        }
                        if (BizActionObj.PatientDetails.GeneralDetails.Photo != null)
                        {
                            BizActionObj.PatientDetails.GeneralDetails.Photo = (Byte[])DALHelper.HandleDBNull(reader["Photo"]);
                        }

                        BizActionObj.PatientDetails.IsLoyaltyMember = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsLoyaltyMember"]));
                        BizActionObj.PatientDetails.LoyaltyCardID = (long?)DALHelper.HandleDBNull(reader["LoyaltyCardID"]);
                        BizActionObj.PatientDetails.IssueDate = (DateTime?)DALHelper.HandleDate(reader["IssueDate"]);
                        BizActionObj.PatientDetails.EffectiveDate = (DateTime?)DALHelper.HandleDate(reader["EffectiveDate"]);
                        BizActionObj.PatientDetails.ExpiryDate = (DateTime?)DALHelper.HandleDate(reader["ExpiryDate"]);
                        BizActionObj.PatientDetails.LoyaltyCardNo = (string)DALHelper.HandleDBNull(reader["LoyaltyCardNo"]);
                        BizActionObj.PatientDetails.RelationID = (long)DALHelper.HandleDBNull(reader["RelationID"]);
                        BizActionObj.PatientDetails.ParentPatientID = (long)DALHelper.HandleDBNull(reader["ParentPatientID"]);

                        BizActionObj.PatientDetails.GeneralDetails.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                        //BizActionObj.PatientDetails.CreatedUnitId = (Int64)DALHelper.HandleDBNull(reader["CreatedUnitId"]);
                        BizActionObj.PatientDetails.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);

                        #region For Pediatric Flow

                        if (BizActionObj.PatientDetails.IsLatestPatient == false)
                        {
                            BizActionObj.PatientDetails.BabyNo = (int?)DALHelper.HandleDBNull(reader["BabyNo"]);
                            BizActionObj.PatientDetails.BabyOfNo = (int?)DALHelper.HandleDBNull(reader["BabyOfNo"]);
                            BizActionObj.PatientDetails.BabyWeight = Convert.ToString(DALHelper.HandleDBNull(reader["BabyWeight"]));
                            BizActionObj.PatientDetails.LinkPatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LinkPatientID"]));
                            BizActionObj.PatientDetails.LinkPatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LinkPatientUnitID"]));
                            BizActionObj.PatientDetails.LinkPatientMrNo = Convert.ToString(DALHelper.HandleDBNull(reader["LinkPatientMrNo"]));
                            BizActionObj.PatientDetails.LinkParentName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["LinkParentFirstName"]))) + " " + Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["LinkParentLastName"])));
                        }
                        #endregion

                        if (objPatientVO.GeneralDetails.SearchFromIPD == true)
                        {

                            BizActionObj.PatientDetails.AdmissionDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["AdmissionDate"]));
                            BizActionObj.PatientDetails.ISDischarged = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ISDischarged"]));
                        }

                        if (BizActionObj.PatientDetails.IsLatestPatient == true)
                        {
                            BizActionObj.PatientDetails.GeneralDetails.IPDAdmissionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IPDAdmissionID"]));
                            BizActionObj.PatientDetails.GeneralDetails.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionUnitID"]));
                            BizActionObj.PatientDetails.GeneralDetails.IPDAdmissionNo = Convert.ToString(DALHelper.HandleDBNull(reader["IPDNO"]));
                            BizActionObj.PatientDetails.GeneralDetails.AdmissionDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["AdmissionDate"]));
                            BizActionObj.PatientDetails.GeneralDetails.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                            BizActionObj.PatientDetails.GeneralDetails.BedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToBedID"]));
                            BizActionObj.PatientDetails.GeneralDetails.WardID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToWardID"]));
                            BizActionObj.PatientDetails.GeneralDetails.ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassID"]));
                            BizActionObj.PatientDetails.GeneralDetails.ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]));
                            BizActionObj.PatientDetails.GeneralDetails.WardName = Convert.ToString(DALHelper.HandleDBNull(reader["WardName"]));
                            BizActionObj.PatientDetails.GeneralDetails.BedName = Convert.ToString(DALHelper.HandleDBNull(reader["BedName"]));
                        }
                        else
                        {
                            BizActionObj.PatientDetails.CreatedUnitId = (Int64)DALHelper.HandleDBNull(reader["CreatedUnitId"]);
                            BizActionObj.PatientDetails.AddedBy = (Int64?)DALHelper.HandleDBNull(reader["AddedBy"]);
                            BizActionObj.PatientDetails.AddedOn = (String)DALHelper.HandleDBNull(reader["AddedOn"]);
                            BizActionObj.PatientDetails.AddedDateTime = (DateTime?)DALHelper.HandleDate(reader["AddedDateTime"]);
                            BizActionObj.PatientDetails.AddedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]);

                            BizActionObj.PatientDetails.UpdatedUnitId = (Int64?)DALHelper.HandleDBNull(reader["UpdatedUnitID"]);
                            BizActionObj.PatientDetails.UpdatedBy = (Int64?)DALHelper.HandleDBNull(reader["UpdatedBy"]);
                            BizActionObj.PatientDetails.UpdatedOn = (String)DALHelper.HandleDBNull(reader["UpdatedOn"]);
                            BizActionObj.PatientDetails.UpdatedDateTime = (DateTime?)DALHelper.HandleDate(reader["UpdatedDateTime"]);
                            BizActionObj.PatientDetails.UpdatedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["UpdatedWindowsLoginName"]);
                            BizActionObj.PatientDetails.CompanyName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["CompanyName"])));
                        }
                    }






                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_GetPatientAttachmentFileInformation");
                    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, objPatientVO.GeneralDetails.PatientID);
                    reader = (DbDataReader)dbServer.ExecuteReader(command1);

                    if (reader.HasRows)
                    {
                        BizActionObj.PatientAttachmentDetailList = new List<clsPatientAttachmentVO>();
                        while (reader.Read())
                        {
                            clsPatientAttachmentVO PatientAttachmentDetail = new clsPatientAttachmentVO();
                            PatientAttachmentDetail.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                            PatientAttachmentDetail.CasePaperType = Convert.ToString(DALHelper.HandleDBNull(reader["SubJectName"]));
                            PatientAttachmentDetail.AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"]));
                            PatientAttachmentDetail.Attachment = (byte[])(DALHelper.HandleDBNull(reader["PDF"]));
                            PatientAttachmentDetail.Doctor = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                            BizActionObj.PatientAttachmentDetailList.Add(PatientAttachmentDetail);
                        }
                    }

                    DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_GetDonor");
                    dbServer.AddInParameter(command3, "ID", DbType.Int64, objPatientVO.GeneralDetails.PatientID);
                    dbServer.AddInParameter(command3, "UnitID", DbType.Int64, objPatientVO.GeneralDetails.UnitId);
                    reader = (DbDataReader)dbServer.ExecuteReader(command3);

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (BizActionObj.PatientDetails.GenderID == 2)
                            {
                                BizActionObj.PatientDetails.GeneralDetails.Height = Convert.ToDouble(DALHelper.HandleDBNull(reader["FemaleHeight"]));
                                BizActionObj.PatientDetails.GeneralDetails.Weight = Convert.ToDouble(DALHelper.HandleDBNull(reader["FemaleWeight"]));
                                BizActionObj.PatientDetails.GeneralDetails.BMI = Convert.ToDouble(DALHelper.HandleDBNull(reader["FemaleBMI"]));
                                BizActionObj.PatientDetails.GeneralDetails.Alerts = Convert.ToString(DALHelper.HandleDBNull(reader["FemaleAlerts"]));
                            }
                            if (BizActionObj.PatientDetails.GenderID == 1)
                            {
                                BizActionObj.PatientDetails.GeneralDetails.Height = Convert.ToDouble(DALHelper.HandleDBNull(reader["MaleHeight"]));
                                BizActionObj.PatientDetails.GeneralDetails.Weight = Convert.ToDouble(DALHelper.HandleDBNull(reader["MaleWeight"]));
                                BizActionObj.PatientDetails.GeneralDetails.BMI = Convert.ToDouble(DALHelper.HandleDBNull(reader["MaleBMI"]));
                                BizActionObj.PatientDetails.GeneralDetails.Alerts = Convert.ToString(DALHelper.HandleDBNull(reader["MaleAlerts"]));
                            }
                        }
                    }

                    //if (BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 7) // Couple
                    //{
                    //    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_GetPatientSpouseInformation");
                    //    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, objPatientVO.GeneralDetails.PatientID);
                    //    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, objPatientVO.GeneralDetails.UnitId);
                    //    // int intStatus = dbServer.ExecuteNonQuery(command);
                    //    reader = (DbDataReader)dbServer.ExecuteReader(command1);
                    //    if (reader.HasRows)
                    //    {
                    //        reader.Read();
                    //        BizActionObj.PatientDetails.SpouseDetails.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                    //        BizActionObj.PatientDetails.SpouseDetails.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                    //        BizActionObj.PatientDetails.SpouseDetails.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                    //        BizActionObj.PatientDetails.SpouseDetails.PatientUnitID = (long)DALHelper.HandleDBNull(reader["PatientUnitID"]);
                    //        BizActionObj.PatientDetails.SpouseDetails.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                    //        BizActionObj.PatientDetails.SpouseDetails.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                    //        BizActionObj.PatientDetails.SpouseDetails.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                    //        BizActionObj.PatientDetails.SpouseDetails.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                    //        BizActionObj.PatientDetails.SpouseDetails.RegistrationDate = (DateTime?)DALHelper.HandleDate(reader["RegistrationDate"]);
                    //        BizActionObj.PatientDetails.SpouseDetails.DateOfBirth = (DateTime?)DALHelper.HandleDate(reader["DateOfBirth"]);
                    //        BizActionObj.PatientDetails.SpouseDetails.FamilyName = Security.base64Decode((String)DALHelper.HandleDBNull(reader["FamilyName"]));
                    //        BizActionObj.PatientDetails.SpouseDetails.GenderID = (Int64)DALHelper.HandleDBNull(reader["GenderID"]);
                    //        BizActionObj.PatientDetails.SpouseDetails.BloodGroupID = (Int64)DALHelper.HandleDBNull(reader["BloodGroupID"]);
                    //        BizActionObj.PatientDetails.SpouseDetails.MaritalStatusID = (Int64)DALHelper.HandleDBNull(reader["MaritalStatusID"]);
                    //        BizActionObj.PatientDetails.SpouseDetails.CivilID = Security.base64Decode((string)DALHelper.HandleDBNull(reader["CivilID"]));
                    //        BizActionObj.PatientDetails.SpouseDetails.ContactNo1 = (string)DALHelper.HandleDBNull(reader["ContactNo1"]);
                    //        BizActionObj.PatientDetails.SpouseDetails.ContactNo2 = (string)DALHelper.HandleDBNull(reader["ContactNo2"]);
                    //        BizActionObj.PatientDetails.SpouseDetails.FaxNo = (String)DALHelper.HandleDBNull(reader["FaxNo"]);
                    //        BizActionObj.PatientDetails.SpouseDetails.Email = Security.base64Decode((String)DALHelper.HandleDBNull(reader["Email"]));
                    //        BizActionObj.PatientDetails.SpouseDetails.AddressLine1 = Security.base64Decode((String)DALHelper.HandleDBNull(reader["AddressLine1"]));
                    //        BizActionObj.PatientDetails.SpouseDetails.AddressLine2 = Security.base64Decode((String)DALHelper.HandleDBNull(reader["AddressLine2"]));
                    //        BizActionObj.PatientDetails.SpouseDetails.AddressLine3 = Security.base64Decode((String)DALHelper.HandleDBNull(reader["AddressLine3"]));
                    //        BizActionObj.PatientDetails.SpouseDetails.Country = Security.base64Decode((String)DALHelper.HandleDBNull(reader["Country"]));
                    //        BizActionObj.PatientDetails.SpouseDetails.State = Security.base64Decode((String)DALHelper.HandleDBNull(reader["State"]));
                    //        BizActionObj.PatientDetails.SpouseDetails.City = Security.base64Decode((String)DALHelper.HandleDBNull(reader["City"]));
                    //        BizActionObj.PatientDetails.SpouseDetails.Taluka = Security.base64Decode((string)DALHelper.HandleDBNull(reader["Taluka"]));
                    //        BizActionObj.PatientDetails.SpouseDetails.Area = Security.base64Decode((string)DALHelper.HandleDBNull(reader["Area"]));
                    //        BizActionObj.PatientDetails.SpouseDetails.District = Security.base64Decode((string)DALHelper.HandleDBNull(reader["District"]));
                    //        BizActionObj.PatientDetails.SpouseDetails.MobileCountryCode = (long)DALHelper.HandleDBNull(reader["MobileCountryCode"]);
                    //        BizActionObj.PatientDetails.SpouseDetails.ResiNoCountryCode = (long)DALHelper.HandleDBNull(reader["ResiNoCountryCode"]);
                    //        BizActionObj.PatientDetails.SpouseDetails.ResiSTDCode = (long)DALHelper.HandleDBNull(reader["ResiSTDCode"]);
                    //        BizActionObj.PatientDetails.SpouseDetails.Pincode = Security.base64Decode((String)DALHelper.HandleDBNull(reader["Pincode"]));
                    //        BizActionObj.PatientDetails.SpouseDetails.ReligionID = (Int64)DALHelper.HandleDBNull(reader["ReligionID"]);
                    //        BizActionObj.PatientDetails.SpouseDetails.OccupationId = (Int64?)DALHelper.HandleDBNull(reader["OccupationId"]);
                    //        BizActionObj.PatientDetails.SpouseDetails.Photo = (Byte[])DALHelper.HandleDBNull(reader["Photo"]);                            
                    //        BizActionObj.PatientDetails.SpouseDetails.CreatedUnitId = (Int64)DALHelper.HandleDBNull(reader["CreatedUnitId"]);
                    //        BizActionObj.PatientDetails.SpouseDetails.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                    //        BizActionObj.PatientDetails.SpouseDetails.AddedBy = (Int64?)DALHelper.HandleDBNull(reader["AddedBy"]);
                    //        BizActionObj.PatientDetails.SpouseDetails.AddedOn = (String)DALHelper.HandleDBNull(reader["AddedOn"]);
                    //        BizActionObj.PatientDetails.SpouseDetails.AddedDateTime = (DateTime?)DALHelper.HandleDate(reader["AddedDateTime"]);
                    //        BizActionObj.PatientDetails.SpouseDetails.AddedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]);
                    //        BizActionObj.PatientDetails.SpouseDetails.UpdatedUnitId = (Int64?)DALHelper.HandleDBNull(reader["UpdatedUnitID"]);
                    //        BizActionObj.PatientDetails.SpouseDetails.UpdatedBy = (Int64?)DALHelper.HandleDBNull(reader["UpdatedBy"]);
                    //        BizActionObj.PatientDetails.SpouseDetails.UpdatedOn = (String)DALHelper.HandleDBNull(reader["UpdatedOn"]);
                    //        BizActionObj.PatientDetails.SpouseDetails.UpdatedDateTime = (DateTime?)DALHelper.HandleDate(reader["UpdatedDateTime"]);
                    //        BizActionObj.PatientDetails.SpouseDetails.UpdatedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["UpdatedWindowsLoginName"]);
                    //    }
                    // }
                }
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

        public override IValueObject GetPatientForLoyaltyCardIssue(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientForLoyaltyCardIssueBizActionVO BizActionObj = (clsGetPatientForLoyaltyCardIssueBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientForIssue");
                DbDataReader reader;

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "IssueDate", DbType.Boolean, BizActionObj.IssuDate);
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                if (BizActionObj.MrNo != null && BizActionObj.MrNo.Length != 0)
                    dbServer.AddInParameter(command, "MrNo", DbType.String, BizActionObj.MrNo + "%");

                if (BizActionObj.OPDNo != null && BizActionObj.OPDNo.Length != 0)
                    dbServer.AddInParameter(command, "OPDNo", DbType.String, BizActionObj.OPDNo + "%");

                if (BizActionObj.LoyaltyCardNo != null && BizActionObj.LoyaltyCardNo.Length != 0)
                    dbServer.AddInParameter(command, "LoyaltyCardNo", DbType.String, BizActionObj.LoyaltyCardNo);

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                    dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(BizActionObj.FirstName) + "%");
                if (BizActionObj.MiddleName != null && BizActionObj.MiddleName.Length != 0)
                    dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(BizActionObj.MiddleName) + "%");
                if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(BizActionObj.LastName) + "%");
                dbServer.AddInParameter(command, "IsLoyaltymember", DbType.String, BizActionObj.IsLoyaltymember);

                if (BizActionObj.IsLoyaltymember == true)
                {
                    dbServer.AddInParameter(command, "LoyaltyProgramId", DbType.Int64, BizActionObj.LoyaltyProgramID);
                }

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.PatientDetails == null)
                        BizActionObj.PatientDetails = new List<clsPatientGeneralVO>();

                    while (reader.Read())
                    {
                        clsPatientGeneralVO objPatientVO = new clsPatientGeneralVO();

                        objPatientVO.PatientID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objPatientVO.PatientTypeID = (long)DALHelper.HandleDBNull(reader["PatientCategoryID"]);
                        objPatientVO.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                        objPatientVO.OPDNO = (string)DALHelper.HandleDBNull(reader["OPDNO"]);
                        objPatientVO.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                        objPatientVO.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                        objPatientVO.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));

                        objPatientVO.DateOfBirth = (DateTime?)DALHelper.HandleDate(reader["DateofBirth"]);
                        objPatientVO.RegistrationDate = (DateTime)DALHelper.HandleDate(reader["RegistrationDate"]);

                        objPatientVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        BizActionObj.PatientDetails.Add(objPatientVO);
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
            return BizActionObj;
        }

        public override IValueObject UpdatePatientForLoyaltyCardIssue(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;

            clsUpdatePatientForIssueBizActionVO BizActionObj = (clsUpdatePatientForIssueBizActionVO)valueObject;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                clsPatientVO objPatientVO = BizActionObj.PatientDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPatientForIssue");

                dbServer.AddInParameter(command, "LoyaltyCardNo", DbType.String, objPatientVO.LoyaltyCardNo);
                dbServer.AddInParameter(command, "PreferNameOnLoyaltyCard", DbType.String, objPatientVO.PreferNameonLoyaltyCard);
                dbServer.AddInParameter(command, "IssueDate", DbType.DateTime, objPatientVO.IssueDate);
                dbServer.AddInParameter(command, "ExpiryDate", DbType.DateTime, objPatientVO.ExpiryDate);
                dbServer.AddInParameter(command, "EffectiveDate", DbType.DateTime, objPatientVO.EffectiveDate);
                dbServer.AddInParameter(command, "LoyaltyCardID", DbType.Int64, objPatientVO.LoyaltyCardID);
                dbServer.AddInParameter(command, "TariffID", DbType.Int64, objPatientVO.TariffID);
                dbServer.AddInParameter(command, "Remark ", DbType.String, Security.base64Encode(objPatientVO.Remark).Trim());
                dbServer.AddInParameter(command, "IsLoyaltyMember ", DbType.Boolean, true);

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddInParameter(command, "ID", DbType.Int64, objPatientVO.GeneralDetails.PatientID);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.SuccessStatus);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                foreach (var ObjFamily in objPatientVO.FamilyDetails)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPatientFamilyDetailsForIssue");
                    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, ObjFamily.PatientID);
                    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, ObjFamily.PatientUnitID);
                    if (ObjFamily.FirstName != null) ObjFamily.FirstName = ObjFamily.FirstName.Trim();
                    dbServer.AddInParameter(command1, "FirstName", DbType.String, Security.base64Encode(ObjFamily.FirstName));
                    if (ObjFamily.MiddleName != null) ObjFamily.MiddleName = ObjFamily.MiddleName.Trim();
                    dbServer.AddInParameter(command1, "MiddleName", DbType.String, Security.base64Encode(ObjFamily.MiddleName));
                    if (ObjFamily.LastName != null) ObjFamily.LastName = ObjFamily.LastName.Trim();
                    dbServer.AddInParameter(command1, "LastName", DbType.String, Security.base64Encode(ObjFamily.LastName));
                    dbServer.AddInParameter(command1, "DOB", DbType.DateTime, ObjFamily.DateOfBirth);
                    dbServer.AddInParameter(command1, "RelationID", DbType.Int64, ObjFamily.RelationID);
                    dbServer.AddInParameter(command1, "TariffID", DbType.Int64, ObjFamily.TariffID);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, ObjFamily.Status);

                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjFamily.ID);
                    int iStatus = dbServer.ExecuteNonQuery(command1, trans);
                    ObjFamily.ID = (long)dbServer.GetParameterValue(command1, "ID");
                }
                if (objPatientVO.OtherDetails != null)
                {
                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddPatientOtherDetails");

                    dbServer.AddInParameter(command2, "PatientID", DbType.Int64, objPatientVO.OtherDetails.PatientID);
                    dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, objPatientVO.OtherDetails.PatientUnitID);
                    dbServer.AddInParameter(command2, "Q1", DbType.Boolean, objPatientVO.OtherDetails.Question1);
                    dbServer.AddInParameter(command2, "Q2", DbType.Boolean, objPatientVO.OtherDetails.Question2);
                    dbServer.AddInParameter(command2, "Q3", DbType.Boolean, objPatientVO.OtherDetails.Question3);
                    dbServer.AddInParameter(command2, "Q4", DbType.Boolean, objPatientVO.OtherDetails.Question4);
                    dbServer.AddInParameter(command2, "Q4Y", DbType.String, objPatientVO.OtherDetails.Question4Details);
                    dbServer.AddInParameter(command2, "Q5A", DbType.Boolean, objPatientVO.OtherDetails.Question5A);
                    dbServer.AddInParameter(command2, "Q5B", DbType.Boolean, objPatientVO.OtherDetails.Question5B);
                    dbServer.AddInParameter(command2, "Q5C", DbType.Boolean, objPatientVO.OtherDetails.Question5C);
                    dbServer.AddInParameter(command2, "Status", DbType.Boolean, objPatientVO.OtherDetails.Status);

                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.OtherDetails.ID);
                    int iStatus = dbServer.ExecuteNonQuery(command2, trans);
                    objPatientVO.OtherDetails.ID = (long)dbServer.GetParameterValue(command2, "ID");
                }
                foreach (var ObjService in objPatientVO.ServiceDetails)
                {
                    DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_AddPatientServiceDetails");

                    dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "PatientID", DbType.Int64, ObjService.PatientID);
                    dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, ObjService.PatientUnitID);
                    dbServer.AddInParameter(command3, "RelationID", DbType.Int64, ObjService.RelationID);
                    dbServer.AddInParameter(command3, "LoyaltyID", DbType.Int64, objPatientVO.LoyaltyCardID);
                    dbServer.AddInParameter(command3, "TariffID", DbType.Int64, ObjService.TariffID);
                    dbServer.AddInParameter(command3, "ServiceID", DbType.Int64, ObjService.ServiceID);
                    dbServer.AddInParameter(command3, "Rate", DbType.Double, ObjService.Rate);
                    dbServer.AddInParameter(command3, "ConcessionPercentage", DbType.Double, ObjService.ConcessionPercentage);
                    dbServer.AddInParameter(command3, "ConcessionAmount", DbType.Double, ObjService.ConcessionAmount);
                    dbServer.AddInParameter(command3, "NetAmount", DbType.Double, ObjService.NetAmount);
                    dbServer.AddInParameter(command3, "Status", DbType.Double, ObjService.SelectService);
                    dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjService.ID);
                    int iStatus = dbServer.ExecuteNonQuery(command3, trans);
                    ObjService.ID = (long)dbServer.GetParameterValue(command3, "ID");
                }
                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                //throw;
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.PatientDetails = null;
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }
            return BizActionObj;
        }

        public override IValueObject GetPatientDetailsForCRM(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientDetailsForCRMBizActionVO BizActionObj = (clsGetPatientDetailsForCRMBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_SearchPatient");
                DbDataReader reader;
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                dbServer.AddInParameter(command, "Age", DbType.Int32, BizActionObj.Age);
                dbServer.AddInParameter(command, "AgeFilter", DbType.String, BizActionObj.AgeFilter);

                if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                    dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(BizActionObj.FirstName) + "%");
                if (BizActionObj.MiddleName != null && BizActionObj.MiddleName.Length != 0)
                    dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(BizActionObj.MiddleName) + "%");
                if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(BizActionObj.LastName) + "%");
                if (BizActionObj.MRNo != null && BizActionObj.MRNo.Length != 0)
                    dbServer.AddInParameter(command, "MrNo", DbType.String, BizActionObj.MRNo + "%");
                if (BizActionObj.OPDNo != null && BizActionObj.OPDNo.Length != 0)
                    dbServer.AddInParameter(command, "OPDNo", DbType.String, BizActionObj.OPDNo + "%");
                if (BizActionObj.UnitID != null && BizActionObj.UnitID != 0)
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                if (BizActionObj.DepartmentID != null && BizActionObj.DepartmentID != 0)
                    dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizActionObj.DepartmentID);
                if (BizActionObj.DoctorID != null && BizActionObj.DoctorID != 0)
                    dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.DoctorID);
                if (BizActionObj.State != null && BizActionObj.State.Length != 0)
                    dbServer.AddInParameter(command, "State", DbType.String, Security.base64Encode(BizActionObj.State) + "%");
                if (BizActionObj.City != null && BizActionObj.City.Length != 0)
                    dbServer.AddInParameter(command, "City", DbType.String, Security.base64Encode(BizActionObj.City) + "%");
                if (BizActionObj.Area != null && BizActionObj.Area.Length != 0)
                    dbServer.AddInParameter(command, "Area", DbType.String, BizActionObj.Area);

                if (BizActionObj.GenderID != null && BizActionObj.GenderID != 0)
                    dbServer.AddInParameter(command, "GenderID", DbType.Int64, BizActionObj.GenderID);
                if (BizActionObj.MaritalStatusID != null && BizActionObj.MaritalStatusID != 0)
                    dbServer.AddInParameter(command, "MaritalStatusID", DbType.Int64, BizActionObj.MaritalStatusID);
                if (BizActionObj.ContactNo != null && BizActionObj.ContactNo.Length != 0)
                    dbServer.AddInParameter(command, "ContactNo", DbType.String, BizActionObj.ContactNo + "%");
                if (BizActionObj.LoyaltyCardID != null && BizActionObj.LoyaltyCardID != 0)
                    dbServer.AddInParameter(command, "LoyaltyCardID", DbType.Int64, BizActionObj.LoyaltyCardID);
                if (BizActionObj.ComplaintID != null && BizActionObj.ComplaintID != 0)
                    dbServer.AddInParameter(command, "ComplaintID", DbType.Int64, BizActionObj.ComplaintID);

                //if (BizActionObj.ProtocolID != null && BizActionObj.ProtocolID != 0)
                //    dbServer.AddInParameter(command, "ProtocolID", DbType.Int64, BizActionObj.ProtocolID);

                //if (BizActionObj.TreatmentID != null && BizActionObj.TreatmentID != 0)
                //    dbServer.AddInParameter(command, "TreatmentID", DbType.Int64, BizActionObj.TreatmentID);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.PatientDetails == null)
                        BizActionObj.PatientDetails = new List<clsPatientVO>();
                    while (reader.Read())
                    {
                        clsPatientVO objPatientVO = new clsPatientVO();
                        objPatientVO.GeneralDetails.PatientID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objPatientVO.GeneralDetails.PatientTypeID = (long)DALHelper.HandleDBNull(reader["PatientCategoryID"]);
                        objPatientVO.GeneralDetails.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                        objPatientVO.GeneralDetails.OPDNO = (string)DALHelper.HandleDBNull(reader["OPDNO"]);

                        objPatientVO.GeneralDetails.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                        objPatientVO.GeneralDetails.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                        objPatientVO.GeneralDetails.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                        objPatientVO.GeneralDetails.DateOfBirth = (DateTime?)DALHelper.HandleDate(reader["DateofBirth"]);
                        objPatientVO.GeneralDetails.RegistrationDate = (DateTime)DALHelper.HandleDate(reader["RegistrationDate"]);
                        objPatientVO.GeneralDetails.ContactNO1 = (string)DALHelper.HandleDBNull(reader["ContactNO1"]);
                        objPatientVO.GeneralDetails.Complaint = (string)DALHelper.HandleDBNull(reader["Complaint"]);
                        objPatientVO.Email = Security.base64Decode((string)DALHelper.HandleDBNull(reader["Email"]));

                        BizActionObj.PatientDetails.Add(objPatientVO);
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
            return BizActionObj;
        }

        public override IValueObject GetPatientDetailsForCounterSale(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientDetailsForCounterSaleBizActionVO BizActionObj = (clsGetPatientDetailsForCounterSaleBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientDetailsForCounterSale");
                DbDataReader reader;

                dbServer.AddInParameter(command, "MRNo", DbType.String, BizActionObj.MRNO);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.PatientDetails == null)
                        BizActionObj.PatientDetails = new clsPatientVO();
                    while (reader.Read())
                    {
                        clsPatientVO objPatientVO = BizActionObj.PatientDetails;
                        BizActionObj.PatientDetails.GeneralDetails.PatientID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        BizActionObj.PatientDetails.GeneralDetails.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                        BizActionObj.PatientDetails.GeneralDetails.RegistrationDate = (DateTime)DALHelper.HandleDate(reader["RegistrationDate"]);
                        BizActionObj.PatientDetails.GeneralDetails.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                        BizActionObj.PatientDetails.GeneralDetails.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                        BizActionObj.PatientDetails.GeneralDetails.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                        BizActionObj.PatientDetails.GeneralDetails.DateOfBirth = (DateTime?)DALHelper.HandleDate(reader["DateofBirth"]);
                        BizActionObj.PatientDetails.GenderID = (long)DALHelper.HandleDBNull(reader["GenderID"]);
                        BizActionObj.PatientDetails.Doctor = (string)DALHelper.HandleDBNull(reader["ReferredDoctor"]);
                        BizActionObj.PatientDetails.MobileCountryCode = Convert.ToString(DALHelper.HandleDBNull(reader["MobileCountryCode"]));
                        BizActionObj.PatientDetails.ContactNo1 = (string)DALHelper.HandleDBNull(reader["ContactNo1"]);
                        BizActionObj.PatientDetails.AddressLine3 = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine3"])));
                        BizActionObj.PatientDetails.AddressLine1 = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine1"])));
                        BizActionObj.PatientDetails.PatientSponsorCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSponsorCategoryID"]));
                        BizActionObj.PatientDetails.IsStaffPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsStaffPatient"]));    //Added by Prashant Channe on 19thNov2019 to bring taxes for StaffPatient
                    }
                }
                reader.Close();
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return BizActionObj;
        }

        public override IValueObject GetPatientTariffs(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetPatientTariffsBizActionVO BizActionObj = (clsGetPatientTariffsBizActionVO)valueObject;

            try
            {
                //Take storeprocedure name as input parameter and creates DbCommand Object.
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientTariffDetails");
                //Adding MasterTableName as Input Parameter to filter record
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "PatientSourceID", DbType.Int64, BizActionObj.PatientSourceID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.CheckDate);
                //dbServer.AddInParameter(command, "IsDate", DbType.Boolean, BizActionObj.IsDate.Value);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                //Check whether the reader contains the records
                if (reader.HasRows)
                {
                    //if masterlist instance is null then creates new instance
                    if (BizActionObj.MasterList == null)
                    {
                        BizActionObj.MasterList = new List<MasterListItem>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        //Add the object value in list
                        BizActionObj.MasterList.Add(new MasterListItem((long)reader["TariffID"], reader["Tariff"].ToString()));
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
                CurrentMethodExecutionStatus = false;
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

        public override IValueObject GetPatientFamilyDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientFamilyDetailsBizActionVO BizActionObj = (clsGetPatientFamilyDetailsBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetLoyaltyFamilyDetailsbyPatientID");
                DbDataReader reader;

                dbServer.AddInParameter(command, "PatientID", DbType.String, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.FamilyDetails == null)
                        BizActionObj.FamilyDetails = new List<clsPatientFamilyDetailsVO>();
                    while (reader.Read())
                    {
                        clsPatientFamilyDetailsVO objFamilyVO = new clsPatientFamilyDetailsVO();

                        objFamilyVO.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                        objFamilyVO.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                        objFamilyVO.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                        objFamilyVO.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                        objFamilyVO.DateOfBirth = (DateTime)DALHelper.HandleDate(reader["DOB"]);
                        objFamilyVO.TariffID = (long)DALHelper.HandleDBNull(reader["TariffID"]);
                        objFamilyVO.RelationID = (long)DALHelper.HandleDBNull(reader["RelationID"]);
                        objFamilyVO.Tariff = (string)DALHelper.HandleDBNull(reader["Tariff"]);
                        objFamilyVO.Relation = (string)DALHelper.HandleDBNull(reader["Relation"]);
                        objFamilyVO.MemberRegistered = (bool)DALHelper.HandleDBNull(reader["MemberRegistered"]);

                        BizActionObj.FamilyDetails.Add(objFamilyVO);
                    }
                }
                reader.Close();
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return BizActionObj;
        }

        public override IValueObject CheckPatientFamilyRegisterd(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCheckPatientMemberRegisteredBizActionVO BizActionObj = (clsCheckPatientMemberRegisteredBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_CheckPatientMemberRegistered");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "RelationID", DbType.Int64, BizActionObj.RelationID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        BizActionObj.SuccessStatus = true;
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return BizActionObj;
        }

        public override IValueObject GetTariffAndRelationFromApplication(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetTariffAndRelationFromApplicationConfigurationBizActionVO BizActionObj = (clsGetTariffAndRelationFromApplicationConfigurationBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetIssueDetailsFromConfig_Application");
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.FamilyDetails == null)
                        BizActionObj.FamilyDetails = new List<clsPatientFamilyDetailsVO>();
                    while (reader.Read())
                    {
                        clsPatientFamilyDetailsVO objFamilyVO = new clsPatientFamilyDetailsVO();

                        //objFamilyVO.TariffID = (long)DALHelper.HandleDBNull(reader["SelfTariffID"]);
                        objFamilyVO.RelationID = (long)DALHelper.HandleDBNull(reader["SelfRelationID"]);
                        //objFamilyVO.Tariff = (string)DALHelper.HandleDBNull(reader["Tariff"]);
                        objFamilyVO.Relation = (string)DALHelper.HandleDBNull(reader["Relation"]);
                        BizActionObj.FamilyDetails.Add(objFamilyVO);
                    }
                }
                reader.Close();
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return BizActionObj;
        }

        public override IValueObject GetPatientPenPusherDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPenPusherDetailListBizActionVO BizActionObj = valueObject as clsGetPatientPenPusherDetailListBizActionVO;
            BizActionObj.PatientPenPusherDetailsInfo = new clsPatientPenPusherInfoVO();
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientPenPusherDetailsList");

                DbDataReader reader;
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    BizActionObj.PatientPenPusherDetailsList = new List<clsPatientPenPusherInfoVO>();
                    while (reader.Read())
                    {
                        clsPatientPenPusherInfoVO PatientPenPusherDetailsInfo = new clsPatientPenPusherInfoVO();
                        PatientPenPusherDetailsInfo.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        PatientPenPusherDetailsInfo.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        PatientPenPusherDetailsInfo.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        PatientPenPusherDetailsInfo.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        //PatientPenPusherDetailsInfo.MiddleName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"])));
                        //PatientPenPusherDetailsInfo.LastName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])));
                        //PatientPenPusherDetailsInfo.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                        if (Convert.ToString(DALHelper.HandleDBNull(reader["Notes"])) != null)
                        {
                            PatientPenPusherDetailsInfo.Notes = Convert.ToString(DALHelper.HandleDBNull(reader["Notes"]));
                        }
                        BizActionObj.PatientPenPusherDetailsList.Add(PatientPenPusherDetailsInfo);
                    }
                }
                //reader.NextResult();
                //if (reader.HasRows)
                //{
                //    BizActionObj.PatientPrescriptionDetailsList = new List<clsPatientPerscriptionInfoVO>();
                //    //clsPatientPerscriptionInfoVO PatientPrescriptionDetailsInfo = new clsPatientPerscriptionInfoVO();
                //    while (reader.Read())
                //    {
                //        clsPatientPerscriptionInfoVO PatientPrescriptionDetailsInfo = new clsPatientPerscriptionInfoVO();
                //        PatientPrescriptionDetailsInfo.Days = Convert.ToInt64(DALHelper.HandleDBNull(reader["Days"]));
                //        PatientPrescriptionDetailsInfo.Dose = Convert.ToString(DALHelper.HandleDBNull(reader["Dose"]));
                //        PatientPrescriptionDetailsInfo.DrugID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DrugID"]));
                //        PatientPrescriptionDetailsInfo.Frequency = Convert.ToString(DALHelper.HandleDBNull(reader["Frequency"]));
                //        PatientPrescriptionDetailsInfo.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                //        PatientPrescriptionDetailsInfo.PrescriptionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PrescriptionID"]));
                //        PatientPrescriptionDetailsInfo.Quantity = Convert.ToInt16(DALHelper.HandleDBNull(reader["Quantity"]));
                //        PatientPrescriptionDetailsInfo.Reason = Convert.ToString(DALHelper.HandleDBNull(reader["Reason"]));
                //        PatientPrescriptionDetailsInfo.Route = Convert.ToString(DALHelper.HandleDBNull(reader["Route"]));
                //        PatientPrescriptionDetailsInfo.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));

                //        BizActionObj.PatientPrescriptionDetailsList.Add(PatientPrescriptionDetailsInfo);
                //    }
                //}
                reader.Close();
                BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return BizActionObj;
        }

        public override IValueObject clsGetPatientPenPusherDetailByID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPenPusherDetailByIDBizActionVO BizActionObj = valueObject as clsGetPatientPenPusherDetailByIDBizActionVO;
            BizActionObj.PatientPrescriptionDetails = new clsPatientPerscriptionInfoVO();

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientPenPusherDetailsByID");
                DbDataReader reader;
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.PatientID);
                //dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                //dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                //dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                //dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                //dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    BizActionObj.PatientPrescriptionDetailsList = new List<clsPatientPerscriptionInfoVO>();
                    //clsPatientPerscriptionInfoVO PatientPrescriptionDetailsInfo = new clsPatientPerscriptionInfoVO();
                    while (reader.Read())
                    {
                        clsPatientPerscriptionInfoVO PatientPrescriptionDetailsInfo = new clsPatientPerscriptionInfoVO();
                        PatientPrescriptionDetailsInfo.Days = Convert.ToInt64(DALHelper.HandleDBNull(reader["Days"]));
                        PatientPrescriptionDetailsInfo.Dose = Convert.ToString(DALHelper.HandleDBNull(reader["Dose"]));
                        PatientPrescriptionDetailsInfo.DrugID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DrugID"]));
                        PatientPrescriptionDetailsInfo.Frequency = Convert.ToString(DALHelper.HandleDBNull(reader["Frequency"]));
                        PatientPrescriptionDetailsInfo.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        PatientPrescriptionDetailsInfo.PrescriptionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PrescriptionID"]));
                        PatientPrescriptionDetailsInfo.Quantity = Convert.ToInt16(DALHelper.HandleDBNull(reader["Quantity"]));
                        PatientPrescriptionDetailsInfo.Reason = Convert.ToString(DALHelper.HandleDBNull(reader["Reason"]));
                        PatientPrescriptionDetailsInfo.Route = Convert.ToString(DALHelper.HandleDBNull(reader["Route"]));
                        PatientPrescriptionDetailsInfo.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));

                        BizActionObj.PatientPrescriptionDetailsList.Add(PatientPrescriptionDetailsInfo);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return BizActionObj;
        }

        #region Patient Consent
        public override IValueObject AddPrintedPatientConscent(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPrintedPatientConscentBizActionVO BizActionObj = valueObject as clsAddPrintedPatientConscentBizActionVO;
            if (BizActionObj.ConsentDetails.ID == 0)
                BizActionObj = AddConsent(BizActionObj, UserVo);

            return valueObject;
        }

        private clsAddPrintedPatientConscentBizActionVO AddConsent(clsAddPrintedPatientConscentBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsPatientConsentVO objConsentVO = BizActionObj.ConsentDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPrintedPatientConscent");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objConsentVO.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objConsentVO.PatientUnitID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "ConscentID", DbType.Int64, objConsentVO.ConsentID);
                dbServer.AddInParameter(command, "Template", DbType.String, objConsentVO.Template);

                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objConsentVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
                BizActionObj.ConsentDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        #endregion

        #region Patient Diet
        public override IValueObject AddPatientDietPlan(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientDietPlanBizActionVO BizActionObj = valueObject as clsAddPatientDietPlanBizActionVO;

            if (BizActionObj.DietPlan.ID == 0)
                BizActionObj = AddDiet(BizActionObj, UserVo);
            else
                BizActionObj = UpdateDiet(BizActionObj, UserVo);

            return valueObject;
        }

        private clsAddPatientDietPlanBizActionVO AddDiet(clsAddPatientDietPlanBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                clsPatientDietPlanVO ObjDiet = BizActionObj.DietPlan;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPatientDietPlan");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, ObjDiet.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, ObjDiet.PatientUnitID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, ObjDiet.VisitID);
                dbServer.AddInParameter(command, "VisitDoctorID", DbType.Int64, ObjDiet.VisitDoctorID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, ObjDiet.Date);
                dbServer.AddInParameter(command, "PlanID", DbType.Int64, ObjDiet.PlanID);
                dbServer.AddInParameter(command, "GeneralInformation", DbType.String, ObjDiet.GeneralInformation);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDiet.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.DietPlan.ID = (long)dbServer.GetParameterValue(command, "ID");

                if (ObjDiet.DietDetails != null && ObjDiet.DietDetails.Count > 0)
                {
                    foreach (var item in ObjDiet.DietDetails)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPatientDietPlanDetails");

                        dbServer.AddInParameter(command1, "DietPlanID", DbType.Int64, ObjDiet.ID);
                        dbServer.AddInParameter(command1, "FoodItemID", DbType.Int64, item.FoodItemID);
                        dbServer.AddInParameter(command1, "FoodCategoryID", DbType.Int64, item.FoodItemCategoryID);
                        dbServer.AddInParameter(command1, "Timing", DbType.String, item.Timing);
                        dbServer.AddInParameter(command1, "FoodQty", DbType.String, item.FoodQty);
                        dbServer.AddInParameter(command1, "FoodUnit", DbType.String, item.FoodUnit);
                        dbServer.AddInParameter(command1, "FoodCal", DbType.String, item.FoodCal);
                        dbServer.AddInParameter(command1, "FoodCH", DbType.String, item.FoodCH);
                        dbServer.AddInParameter(command1, "FoodFat", DbType.String, item.FoodFat);
                        dbServer.AddInParameter(command1, "FoodExpectedCal", DbType.String, item.FoodExpectedCal);
                        dbServer.AddInParameter(command1, "FoodInstruction", DbType.String, item.FoodInstruction);

                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                        item.ID = (long)dbServer.GetParameterValue(command1, "ID");
                    }
                }

                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.DietPlan = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return BizActionObj;
        }

        private clsAddPatientDietPlanBizActionVO UpdateDiet(clsAddPatientDietPlanBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                clsPatientDietPlanVO ObjDiet = BizActionObj.DietPlan;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdatePatientDietPlan");

                dbServer.AddInParameter(command, "ID", DbType.Int64, ObjDiet.ID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, ObjDiet.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, ObjDiet.PatientUnitID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, ObjDiet.VisitID);
                dbServer.AddInParameter(command, "VisitDoctorID", DbType.Int64, ObjDiet.VisitDoctorID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, ObjDiet.Date);
                dbServer.AddInParameter(command, "PlanID", DbType.Int64, ObjDiet.PlanID);
                dbServer.AddInParameter(command, "GeneralInformation", DbType.String, ObjDiet.GeneralInformation);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);

                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                if (ObjDiet.DietDetails != null && ObjDiet.DietDetails.Count > 0)
                {
                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_DeletePatientDietPlanDetails");
                    dbServer.AddInParameter(command2, "DietPlanID", DbType.Int64, ObjDiet.ID);
                    int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                }
                if (ObjDiet.DietDetails != null && ObjDiet.DietDetails.Count > 0)
                {
                    foreach (var item in ObjDiet.DietDetails)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPatientDietPlanDetails");

                        dbServer.AddInParameter(command1, "DietPlanID", DbType.Int64, ObjDiet.ID);
                        dbServer.AddInParameter(command1, "FoodItemID", DbType.Int64, item.FoodItemID);
                        dbServer.AddInParameter(command1, "FoodCategoryID", DbType.Int64, item.FoodItemCategoryID);
                        dbServer.AddInParameter(command1, "Timing", DbType.String, item.Timing);
                        dbServer.AddInParameter(command1, "FoodQty", DbType.String, item.FoodQty);
                        dbServer.AddInParameter(command1, "FoodUnit", DbType.String, item.FoodUnit);
                        dbServer.AddInParameter(command1, "FoodCal", DbType.String, item.FoodCal);
                        dbServer.AddInParameter(command1, "FoodCH", DbType.String, item.FoodCH);
                        dbServer.AddInParameter(command1, "FoodFat", DbType.String, item.FoodFat);
                        dbServer.AddInParameter(command1, "FoodExpectedCal", DbType.String, item.FoodExpectedCal);
                        dbServer.AddInParameter(command1, "FoodInstruction", DbType.String, item.FoodInstruction);
                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                        item.ID = (long)dbServer.GetParameterValue(command1, "ID");
                    }
                }
                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.DietPlan = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return BizActionObj;
        }

        public override IValueObject GetPatientDietPlan(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientDietPlanBizActionVO BizActionObj = valueObject as clsGetPatientDietPlanBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientDietPlan");
                DbDataReader reader;
                dbServer.AddInParameter(command, "PlanID", DbType.Int64, BizActionObj.PlanID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.DietList == null)
                        BizActionObj.DietList = new List<clsPatientDietPlanVO>();

                    while (reader.Read())
                    {
                        clsPatientDietPlanVO DietVO = new clsPatientDietPlanVO();
                        DietVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        DietVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        DietVO.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                        DietVO.PatientUnitID = (long)DALHelper.HandleDBNull(reader["PatientUnitID"]);
                        DietVO.VisitID = (long)DALHelper.HandleDBNull(reader["VisitID"]);
                        DietVO.VisitDoctorID = (long)DALHelper.HandleDBNull(reader["VisitDoctorID"]);
                        DietVO.VisitDoctor = (string)DALHelper.HandleDBNull(reader["DoctorName"]);
                        DietVO.Date = (DateTime)DALHelper.HandleDBNull(reader["Date"]);
                        DietVO.PlanID = (long)DALHelper.HandleDBNull(reader["PlanID"]);
                        DietVO.PlanName = (string)DALHelper.HandleDBNull(reader["PlanName"]);
                        DietVO.GeneralInformation = (string)DALHelper.HandleDBNull(reader["GeneralInformation"]);
                        DietVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        BizActionObj.DietList.Add(DietVO);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetPatientDietPlanDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientDietPlanDetailsBizActionVO BizActionObj = valueObject as clsGetPatientDietPlanDetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientDietPlanDetails");
                DbDataReader reader;

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.DietDetailsList == null)
                        BizActionObj.DietDetailsList = new List<clsPatientDietPlanDetailVO>();

                    while (reader.Read())
                    {
                        clsPatientDietPlanDetailVO DietVO = new clsPatientDietPlanDetailVO();
                        DietVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        DietVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        DietVO.DietPlanID = (long)DALHelper.HandleDBNull(reader["DietPlanID"]);
                        DietVO.FoodItemID = (long)DALHelper.HandleDBNull(reader["FoodItemID"]);
                        DietVO.FoodItem = (string)DALHelper.HandleDBNull(reader["FoodItem"]);

                        DietVO.FoodItemCategoryID = (long)DALHelper.HandleDBNull(reader["FoodCategoryID"]);
                        DietVO.FoodItemCategory = (string)DALHelper.HandleDBNull(reader["FoodCategory"]);

                        DietVO.Timing = (string)DALHelper.HandleDBNull(reader["Timing"]);
                        DietVO.FoodQty = (string)DALHelper.HandleDBNull(reader["FoodQty"]);
                        DietVO.FoodUnit = (string)DALHelper.HandleDBNull(reader["FoodUnit"]);
                        DietVO.FoodCal = (string)DALHelper.HandleDBNull(reader["FoodCal"]);
                        DietVO.FoodCH = (string)DALHelper.HandleDBNull(reader["FoodCH"]);
                        DietVO.FoodFat = (string)DALHelper.HandleDBNull(reader["FoodFat"]);
                        DietVO.FoodExpectedCal = (string)DALHelper.HandleDBNull(reader["FoodExpectedCal"]);
                        DietVO.FoodInstruction = (string)DALHelper.HandleDBNull(reader["FoodInstruction"]);
                        DietVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        BizActionObj.DietDetailsList.Add(DietVO);
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

        public override IValueObject GetDietPlanMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGeDietPlanMasterBizActionVO BizActionObj = valueObject as clsGeDietPlanMasterBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDietPlanDetails");
                DbDataReader reader;

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.PlanID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.DietDetailList == null)
                        BizActionObj.DietDetailList = new List<clsPatientDietPlanDetailVO>();

                    while (reader.Read())
                    {
                        clsPatientDietPlanDetailVO DietVO = new clsPatientDietPlanDetailVO();
                        DietVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        DietVO.DietPlanID = (long)DALHelper.HandleDBNull(reader["PlanID"]);
                        DietVO.FoodItemID = (long)DALHelper.HandleDBNull(reader["ItemID"]);
                        DietVO.FoodItem = (string)DALHelper.HandleDBNull(reader["FoodItemName"]);
                        DietVO.FoodItemCategory = (string)DALHelper.HandleDBNull(reader["FoodItemCatName"]);
                        DietVO.FoodItemCategoryID = (long)DALHelper.HandleDBNull(reader["ItemCategoryID"]);
                        DietVO.Timing = (string)DALHelper.HandleDBNull(reader["Timing"]);
                        DietVO.FoodQty = (string)DALHelper.HandleDBNull(reader["ItemQty"]);
                        DietVO.FoodUnit = (string)DALHelper.HandleDBNull(reader["ItemUnit"]);
                        DietVO.FoodCal = (string)DALHelper.HandleDBNull(reader["ItemCal"]);
                        DietVO.FoodCH = (string)DALHelper.HandleDBNull(reader["ItemCH"]);
                        DietVO.FoodFat = (string)DALHelper.HandleDBNull(reader["ItemFat"]);
                        DietVO.FoodExpectedCal = (string)DALHelper.HandleDBNull(reader["ItemExpectedCal"]);
                        DietVO.FoodInstruction = (string)DALHelper.HandleDBNull(reader["ItemInstruction"]);
                        BizActionObj.GeneralInfo = (string)DALHelper.HandleDBNull(reader["PlanInformation"]);

                        BizActionObj.DietDetailList.Add(DietVO);
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

        public override IValueObject GetPrintedPatientConscent(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPrintedPatientConscentBizActionVO BizActionObj = valueObject as clsGetPrintedPatientConscentBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_rptPatientConsentPrint");
                DbDataReader reader;

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ConsentDetails.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.ConsentDetails.UnitID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.ConsentDetails == null)
                        BizActionObj.ConsentDetails = new clsPatientConsentVO();

                    while (reader.Read())
                    {
                        BizActionObj.ConsentDetails.Template = (string)DALHelper.HandleDBNull(reader["Template"]);
                        BizActionObj.ConsentDetails.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                        BizActionObj.ConsentDetails.PatientUnitID = (long)DALHelper.HandleDBNull(reader["PatientUnitID"]);
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

        //Added by Saily P
        public override IValueObject AddNewCouple(IValueObject valueObject, clsUserVO UserVo)
        {
            try
            {
                clsAddNewCoupleBizActionVO BizActionObj = valueObject as clsAddNewCoupleBizActionVO;
                clsPatientVO objPatientVO = BizActionObj.PatientDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_DisableCoupleRegistration");
                if (BizActionObj.PatientDetails.GenderID == (long)Genders.Male)
                {
                    dbServer.AddInParameter(command, "MalePatientID", DbType.Int64, objPatientVO.GeneralDetails.PatientID);
                    dbServer.AddInParameter(command, "IsFemale", DbType.Boolean, false);
                    // dbServer.AddInParameter(command, "FemalePatientID", DbType.Int64, objPatientVO.SpouseDetails.PatientID);
                }
                else
                {
                    //  dbServer.AddInParameter(command, "MalePatientID", DbType.Int64, objPatientVO.SpouseDetails.PatientID);
                    dbServer.AddInParameter(command, "FemalePatientID", DbType.Int64, objPatientVO.GeneralDetails.PatientID);
                    dbServer.AddInParameter(command, "IsFemale", DbType.Boolean, true);
                }

                int intStatus = dbServer.ExecuteNonQuery(command);

                DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddCoupleRegistration");
                if (BizActionObj.PatientDetails.GenderID == (long)Genders.Male)
                {
                    dbServer.AddInParameter(command2, "MalePatientID", DbType.Int64, objPatientVO.GeneralDetails.PatientID);
                    dbServer.AddInParameter(command2, "FemalePatientID", DbType.Int64, objPatientVO.SpouseDetails.PatientID);
                }
                else
                {
                    dbServer.AddInParameter(command2, "MalePatientID", DbType.Int64, objPatientVO.SpouseDetails.PatientID);
                    dbServer.AddInParameter(command2, "FemalePatientID", DbType.Int64, objPatientVO.GeneralDetails.PatientID);
                }

                dbServer.AddInParameter(command2, "MalePatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command2, "FemalePatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command2, "RegistrationDate", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, objPatientVO.AddedDateTime);
                dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command2, "CoupleRgistrationNumber", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "000000000000000000000000000000000000000000000000000");
                dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddInParameter(command2, "IsFromNewCouple", DbType.Boolean, objPatientVO.GeneralDetails.IsFromNewCouple);//added by neena

                int intStatus2 = dbServer.ExecuteNonQuery(command2);
                //int intStatus = dbServer.ExecuteNonQuery(command1);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return valueObject;
        }

        public override IValueObject GetPatientList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientListBizActionVO BizActionObj = valueObject as clsGetPatientListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientList");
                DbDataReader reader;

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId); //((clsPatientVO)BizActionObj.PatientDetails.SelectPatient).Unit );
                dbServer.AddInParameter(command, "GenderID", DbType.Int64, BizActionObj.GenderID);
                // BY BHUSHAN . . . . . .
                if (BizActionObj.SearchName == true)
                {
                    dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.Description);
                }
                else
                {
                    dbServer.AddInParameter(command, "Description", DbType.String, Security.base64Encode(BizActionObj.Description));
                }
                dbServer.AddInParameter(command, "SearchBy", DbType.Boolean, BizActionObj.SearchName);
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.SortExpression);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.PatientDetails == null)
                        BizActionObj.PatientDetails = new List<clsPatientVO>();
                    if (BizActionObj.PatientDetailsList == null)
                        BizActionObj.PatientDetailsList = new List<clsPatientGeneralVO>();

                    while (reader.Read())
                    {
                        clsPatientVO BizObj = new clsPatientVO();

                        BizObj.GeneralDetails.PatientID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        BizObj.GeneralDetails.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        BizObj.GeneralDetails.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                        BizObj.GeneralDetails.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                        BizObj.GeneralDetails.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                        BizObj.GeneralDetails.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));

                        //BizObj.GeneralDetails.g = (DateTime?)DALHelper.HandleDate(reader["RegistrationDate"]);
                        BizObj.GeneralDetails.DateOfBirth = (DateTime?)DALHelper.HandleDate(reader["DateOfBirth"]);
                        BizActionObj.PatientDetails.Add(BizObj);
                        clsPatientGeneralVO BizObjGeneral = new clsPatientGeneralVO();
                        BizObjGeneral.PatientID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        BizObjGeneral.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        BizObjGeneral.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                        BizObjGeneral.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                        BizObjGeneral.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                        BizObjGeneral.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));

                        //BizObj.GeneralDetails.g = (DateTime?)DALHelper.HandleDate(reader["RegistrationDate"]);
                        BizObjGeneral.DateOfBirth = (DateTime?)DALHelper.HandleDate(reader["DateOfBirth"]);
                        BizActionObj.PatientDetailsList.Add(BizObjGeneral);
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
            return valueObject;
        }


        public override IValueObject GetPatientLinkFile(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientLinkFileBizActionVO BizActionObj = valueObject as clsGetPatientLinkFileBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientLinkFile");
                DbDataReader reader;

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);


                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.PatientDetails == null)
                        BizActionObj.PatientDetails = new List<clsPatientLinkFileBizActionVO>();

                    while (reader.Read())
                    {
                        clsPatientLinkFileBizActionVO ObjPatient = new clsPatientLinkFileBizActionVO();
                        ObjPatient.ID = (long)DALHelper.HandleIntegerNull(reader["ID"]);
                        ObjPatient.UnitId = (long)DALHelper.HandleIntegerNull(reader["UnitId"]);
                        ObjPatient.PatientID = (long)DALHelper.HandleIntegerNull(reader["PatientID"]);
                        ObjPatient.PatientUnitID = (long)DALHelper.HandleIntegerNull(reader["PatientUnitID"]);
                        ObjPatient.VisitID = (long)DALHelper.HandleIntegerNull(reader["VisitID"]);
                        ObjPatient.ReferredBy = (string)DALHelper.HandleDBNull(reader["ReferredBy"]);
                        ObjPatient.Report = (byte[])DALHelper.HandleDBNull(reader["Report"]);
                        ObjPatient.DocumentName = (string)DALHelper.HandleDBNull(reader["DocumentName"]);
                        ObjPatient.SourceURL = (string)DALHelper.HandleDBNull(reader["SourceURL"]);
                        ObjPatient.Remarks = (string)DALHelper.HandleDBNull(reader["Remarks"]);
                        ObjPatient.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);
                        ObjPatient.Time = (DateTime)DALHelper.HandleDate(reader["Time"]);
                        BizActionObj.PatientDetails.Add(ObjPatient);

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
        public override IValueObject AddPatientLinkFile(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientLinkFileBizActionVO BizActionObj = valueObject as clsAddPatientLinkFileBizActionVO;
            if (BizActionObj.FROMEMR == false)
            {
                if (BizActionObj.PatientDetails[0].ID == 0)
                    BizActionObj = AddLinkfile(BizActionObj, UserVo);
                else
                    BizActionObj = UpdateLinkfile(BizActionObj, UserVo);
            }
            else
            {
                BizActionObj = AddEMRImage(BizActionObj, UserVo);
            }

            return valueObject;
        }
        private clsAddPatientLinkFileBizActionVO AddEMRImage(clsAddPatientLinkFileBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                if (BizActionObj.PatientDetails != null && BizActionObj.PatientDetails.Count > 0)
                {
                    DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_DeleteImage");
                    dbServer.AddInParameter(command3, "TemplateID", DbType.Int64, BizActionObj.PatientDetails[0].TemplateID);
                    int intStatus2 = dbServer.ExecuteNonQuery(command3);
                }
                foreach (clsPatientLinkFileBizActionVO ObjFile in BizActionObj.PatientDetails)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddTemplateImage");
                    dbServer.AddInParameter(command, "SourceURL", DbType.String, ObjFile.SourceURL);
                    dbServer.AddInParameter(command, "Report", DbType.Binary, ObjFile.Report);
                    dbServer.AddInParameter(command, "DocumentName", DbType.String, ObjFile.DocumentName);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command, "TemplateID", DbType.Int64, ObjFile.TemplateID);
                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjFile.ID);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus = dbServer.ExecuteNonQuery(command);
                    //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    ObjFile.ID = (long)dbServer.GetParameterValue(command, "ID");
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
        private clsAddPatientLinkFileBizActionVO AddLinkfile(clsAddPatientLinkFileBizActionVO BizActionObj, clsUserVO UserVo)
        {



            try
            {

                foreach (var ObjFile in BizActionObj.PatientDetails)
                {

                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPatientLinkFile");
                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, ObjFile.PatientID);
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, ObjFile.PatientUnitID);
                    dbServer.AddInParameter(command, "VisitID", DbType.Int64, ObjFile.VisitID);
                    dbServer.AddInParameter(command, "Date", DbType.DateTime, ObjFile.Date);

                    dbServer.AddInParameter(command, "SourceURL", DbType.String, ObjFile.SourceURL);
                    dbServer.AddInParameter(command, "Report", DbType.Binary, ObjFile.Report);
                    dbServer.AddInParameter(command, "DocumentName", DbType.String, ObjFile.DocumentName);
                    dbServer.AddInParameter(command, "Notes", DbType.String, ObjFile.Notes);
                    dbServer.AddInParameter(command, "Remarks", DbType.String, ObjFile.Remarks);
                    dbServer.AddInParameter(command, "Time", DbType.DateTime, ObjFile.Time);
                    dbServer.AddInParameter(command, "ReferredBy", DbType.String, ObjFile.ReferredBy);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);

                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjFile.ID);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus = dbServer.ExecuteNonQuery(command);
                    // BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    ObjFile.ID = (long)dbServer.GetParameterValue(command, "ID");


                }

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


        private clsAddPatientLinkFileBizActionVO UpdateLinkfile(clsAddPatientLinkFileBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                if (BizActionObj.PatientDetails != null && BizActionObj.PatientDetails.Count > 0)
                {
                    foreach (var item in BizActionObj.PatientDetails)
                    {


                        DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_DeletePatientLinkFile");

                        //  dbServer.AddInParameter(command3, "ID", DbType.Int64, item.ID);
                        dbServer.AddInParameter(command3, "UnitId", DbType.Int64, item.UnitId);
                        dbServer.AddInParameter(command3, "PatientID", DbType.Int64, item.PatientID);
                        dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, item.PatientUnitID);
                        dbServer.AddInParameter(command3, "VisitID", DbType.Int64, item.VisitID);
                        int intStatus2 = dbServer.ExecuteNonQuery(command3);
                    }
                }

                foreach (var ObjFile in BizActionObj.PatientDetails)
                {
                    ObjFile.ID = 0;
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPatientLinkFile");
                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, ObjFile.PatientID);
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, ObjFile.PatientUnitID);
                    dbServer.AddInParameter(command, "VisitID", DbType.Int64, ObjFile.VisitID);
                    dbServer.AddInParameter(command, "Date", DbType.DateTime, ObjFile.Date);

                    dbServer.AddInParameter(command, "SourceURL", DbType.String, ObjFile.SourceURL);
                    dbServer.AddInParameter(command, "Report", DbType.Binary, ObjFile.Report);
                    dbServer.AddInParameter(command, "DocumentName", DbType.String, ObjFile.DocumentName);
                    dbServer.AddInParameter(command, "Notes", DbType.String, ObjFile.Notes);
                    dbServer.AddInParameter(command, "Remarks", DbType.String, ObjFile.Remarks);
                    dbServer.AddInParameter(command, "Time", DbType.DateTime, ObjFile.Time);
                    dbServer.AddInParameter(command, "ReferredBy", DbType.String, ObjFile.ReferredBy);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);

                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjFile.ID);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus = dbServer.ExecuteNonQuery(command);
                    // BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    ObjFile.ID = (long)dbServer.GetParameterValue(command, "ID");


                }

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

        // BY BHUSHAN . . . . . . . . . . . .
        public override IValueObject ADDPatientSignConsent(IValueObject valueObject, clsUserVO UserVo)
        {
            clsADDPatientSignConsentBizActionVO BizActionObj = valueObject as clsADDPatientSignConsentBizActionVO;

            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;
            string strPONO1 = String.Empty;
            try
            {
                con.Open();
                trans = con.BeginTransaction();

                clsPatientSignConsentVO objDetailsVO = BizActionObj.signConsentDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_ADD_PatientSignConsent");
                command.Connection = con;

                //dbServer.AddInParameter(command, "ID", DbType.Int32, BizActionObj.Details.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int32, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "ConsentID", DbType.Int64, Convert.ToInt64(objDetailsVO.ConsentID));
                dbServer.AddInParameter(command, "ConsentUnitID", DbType.Int64, Convert.ToInt64(objDetailsVO.ConsentUnitID));
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, Convert.ToInt64(objDetailsVO.PatientID));
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, Convert.ToInt64(objDetailsVO.PatientUnitID));
                dbServer.AddInParameter(command, "ReferredBy", DbType.String, Convert.ToString(UserVo.LoginName));
                dbServer.AddInParameter(command, "Date", DbType.DateTime, Convert.ToDateTime(objDetailsVO.Date));
                dbServer.AddInParameter(command, "Time", DbType.DateTime, Convert.ToDateTime(objDetailsVO.Time));
                dbServer.AddInParameter(command, "DocumentName", DbType.String, Convert.ToString(objDetailsVO.DocumentName));
                dbServer.AddInParameter(command, "SourceURL", DbType.String, objDetailsVO.SourceURL);
                dbServer.AddInParameter(command, "Remarks", DbType.String, Convert.ToString(objDetailsVO.Remarks));
                dbServer.AddInParameter(command, "Status", DbType.Boolean, Convert.ToBoolean(objDetailsVO.Status));
                dbServer.AddInParameter(command, "Report", DbType.Binary, objDetailsVO.Report);

                //added by neena
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, Convert.ToInt64(objDetailsVO.PlanTherapyID));
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, Convert.ToInt64(objDetailsVO.PlanTherapyUnitID));
                //
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.signConsentDetails.ID);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                //  BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                trans.Commit();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.signConsentDetails = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }


            return valueObject;
        }

        public override IValueObject GetPatientSignConsent(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientSignConsentBizActionVO BizActionObj = valueObject as clsGetPatientSignConsentBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_Get_PatientSignConsent");

                dbServer.AddInParameter(command, "ConsentID", DbType.Int64, BizActionObj.ConsentID);
                dbServer.AddInParameter(command, "ConsentUnitID", DbType.Int64, BizActionObj.ConsentUnitID);

                //added by neena
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.PlanTherapyUnitID);
                //

                dbServer.AddInParameter(command, "Status", DbType.Boolean, BizActionObj.Status);

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.SignPatientList == null)
                        BizActionObj.SignPatientList = new List<clsPatientSignConsentVO>();
                    while (reader.Read())
                    {
                        clsPatientSignConsentVO Details = new clsPatientSignConsentVO();

                        Details.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        Details.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        Details.Date = (DateTime)DALHelper.HandleDBNull(reader["Date"]);
                        Details.ConsentID = (long)DALHelper.HandleDBNull(reader["ConsentID"]);
                        Details.ConsentUnitID = (long)DALHelper.HandleDBNull(reader["ConsentUnitID"]);
                        Details.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                        Details.PatientUnitID = (long)DALHelper.HandleDBNull(reader["PatientUnitID"]);
                        Details.DocumentName = (string)DALHelper.HandleDBNull(reader["DocumentName"]);
                        Details.Remarks = (string)DALHelper.HandleDBNull(reader["Remarks"]);
                        //Details.InvoiceNo = (string)DALHelper.HandleDBNull(reader["InvoiceNO"]);
                        Details.Report = (byte[])DALHelper.HandleDBNull(reader["Report"]);
                        Details.ReferredBy = (string)DALHelper.HandleDBNull(reader["ReferredBy"]);
                        Details.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        Details.SourceURL = (string)DALHelper.HandleDBNull(reader["SourceURL"]);
                        //added by neena
                        Details.PlanTherapyID = (long)DALHelper.HandleDBNull(reader["PlanTherapyID"]);
                        Details.PlanTherapyUnitID = (long)DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]);
                        //
                        BizActionObj.SignPatientList.Add(Details);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

            return BizActionObj;
        }
        public override IValueObject DeletePatientSignConsent(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeletePatientSignConsentBizActionVO BizActionObj = valueObject as clsDeletePatientSignConsentBizActionVO;
            try
            {
                clsPatientSignConsentVO objSignResutl = BizActionObj.DeleteVO;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_Delete_SignConsent");

                dbServer.AddInParameter(command, "ID", DbType.Int64, objSignResutl.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objSignResutl.UnitID);

                int Status = dbServer.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return valueObject;
        }


        //By Anjali...........
        public override IValueObject GetCoupleGeneralDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCoupleGeneralDetailsListBizActionVO BizActionObj = valueObject as clsGetCoupleGeneralDetailsListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCoupleGeneralDetailsListForSearch");
                DbDataReader reader;

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "MrNo", DbType.String, BizActionObj.MRNo);
                dbServer.AddInParameter(command, "SearchKeyword", DbType.String, BizActionObj.SearchKeyword);

                //if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                //    dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(BizActionObj.FirstName) + "%");
                //if (BizActionObj.MiddleName != null && BizActionObj.MiddleName.Length != 0)
                //    dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(BizActionObj.MiddleName) + "%");
                //if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                //    dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(BizActionObj.LastName) + "%");
                //if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.FirstName);
                if (BizActionObj.MiddleName != null && BizActionObj.MiddleName.Length != 0)
                    dbServer.AddInParameter(command, "MiddleName", DbType.String, BizActionObj.MiddleName);
                if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.LastName);
                if (BizActionObj.FamilyName != null && BizActionObj.FamilyName.Length != 0)

                    if (BizActionObj.FamilyName != null && BizActionObj.FamilyName.Length != 0)
                        dbServer.AddInParameter(command, "FamilyName", DbType.String, Security.base64Encode(BizActionObj.FamilyName));
                if (BizActionObj.OPDNo != null && BizActionObj.OPDNo.Length != 0)
                {
                    if (BizActionObj.VisitWise == true)
                        dbServer.AddInParameter(command, "OPDNo", DbType.String, BizActionObj.OPDNo + "%");
                    else if (BizActionObj.AdmissionWise == true)
                        dbServer.AddInParameter(command, "AdmissionNo", DbType.String, BizActionObj.OPDNo + "%");
                }

                if (BizActionObj.ContactNo != null && BizActionObj.ContactNo.Length != 0)
                    dbServer.AddInParameter(command, "ContactNo", DbType.String, BizActionObj.ContactNo + "%");
                if (BizActionObj.Country != null && BizActionObj.Country.Length != 0)
                    dbServer.AddInParameter(command, "Country", DbType.String, Security.base64Encode(BizActionObj.Country) + "%");
                if (BizActionObj.State != null && BizActionObj.State.Length != 0)
                    dbServer.AddInParameter(command, "State", DbType.String, Security.base64Encode(BizActionObj.State) + "%");
                if (BizActionObj.City != null && BizActionObj.City.Length != 0)
                    dbServer.AddInParameter(command, "City", DbType.String, Security.base64Encode(BizActionObj.City) + "%");
                if (BizActionObj.Pincode != null && BizActionObj.Pincode.Length != 0)
                    dbServer.AddInParameter(command, "Pincode", DbType.String, Security.base64Encode(BizActionObj.Pincode) + "%");
                if (BizActionObj.CivilID != null && BizActionObj.CivilID.Length != 0)
                    dbServer.AddInParameter(command, "CivilID", DbType.String, Security.base64Encode(BizActionObj.CivilID) + "%");
                if (BizActionObj.GenderID != null && BizActionObj.GenderID != 0)
                    dbServer.AddInParameter(command, "GenderID", DbType.Int64, BizActionObj.GenderID);
                if (BizActionObj.PatientCategoryID != null && BizActionObj.PatientCategoryID != 0)
                    dbServer.AddInParameter(command, "PatientCategory", DbType.Int64, BizActionObj.PatientCategoryID);

                if (BizActionObj.FromDate != null)
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                if (BizActionObj.ToDate != null)
                {
                    if (BizActionObj.FromDate != null)
                    {
                        //if (BizActionObj.FromDate.Equals(BizActionObj.ToDate))
                        BizActionObj.ToDate = BizActionObj.ToDate.Value.Date.AddDays(1);
                    }

                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                }

                if (BizActionObj.VisitWise == true)
                {
                    dbServer.AddInParameter(command, "Serchfor", DbType.Int16, (Int16)1);

                    if (BizActionObj.VisitFromDate != null)
                        dbServer.AddInParameter(command, "VisitFromDate", DbType.DateTime, BizActionObj.VisitFromDate);
                    if (BizActionObj.VisitToDate != null)
                    {
                        if (BizActionObj.VisitFromDate != null)
                        {
                            BizActionObj.VisitToDate = BizActionObj.VisitToDate.Value.Date.AddDays(1);
                        }
                        dbServer.AddInParameter(command, "VisitToDate", DbType.DateTime, BizActionObj.VisitToDate);
                    }
                }

                if (BizActionObj.AdmissionWise == true)
                {
                    dbServer.AddInParameter(command, "Serchfor", DbType.Int16, (Int16)2);

                    if (BizActionObj.AdmissionFromDate != null)
                        dbServer.AddInParameter(command, "AdmissionFromDate", DbType.DateTime, BizActionObj.AdmissionFromDate);
                    if (BizActionObj.AdmissionToDate != null)
                    {
                        if (BizActionObj.AdmissionToDate != null)
                        {
                            BizActionObj.AdmissionToDate = BizActionObj.AdmissionToDate.Value.Date.AddDays(1);
                        }
                        dbServer.AddInParameter(command, "AdmissionToDate", DbType.DateTime, BizActionObj.AdmissionToDate);
                    }
                }
                //=====================================================
                if (BizActionObj.DOBWise == true)
                {
                    dbServer.AddInParameter(command, "Serchfor", DbType.Int16, (Int16)0);

                    if (BizActionObj.DOBFromDate != null)
                        dbServer.AddInParameter(command, "DOBFromDate", DbType.DateTime, BizActionObj.DOBFromDate);
                    if (BizActionObj.DOBToDate != null)
                    {
                        if (BizActionObj.DOBToDate != null)
                        {
                            BizActionObj.DOBToDate = BizActionObj.DOBToDate.Value.Date.AddDays(1);
                        }
                        dbServer.AddInParameter(command, "DOBToDate", DbType.DateTime, BizActionObj.DOBToDate);
                    }
                }
                //=====================================================

                if (BizActionObj.IsLoyaltyMember == true)
                {
                    dbServer.AddInParameter(command, "IsLoyaltyMember", DbType.Boolean, BizActionObj.IsLoyaltyMember);
                    dbServer.AddInParameter(command, "LoyaltyProgramID", DbType.Int64, BizActionObj.LoyaltyProgramID);
                }
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

                dbServer.AddInParameter(command, "RegistrationTypeID", DbType.Int64, BizActionObj.RegistrationTypeID);

                if (BizActionObj.SearchInAnotherClinic == true)
                {
                    dbServer.AddInParameter(command, "SearchInAnotherClinic", DbType.Boolean, BizActionObj.SearchInAnotherClinic);
                }

                //dbServer.AddInParameter(command, "IncludeSpouse", DbType.Boolean, BizActionObj.IncludeSpouse);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "LinkServer", DbType.String, BizActionObj.LinkServer);
                if (BizActionObj.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, BizActionObj.LinkServer.Replace(@"\", "_"));

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.PatientDetailsList == null)
                        BizActionObj.PatientDetailsList = new List<clsPatientGeneralVO>();
                    while (reader.Read())
                    {
                        clsPatientGeneralVO objPatientVO = new clsPatientGeneralVO();

                        objPatientVO.PatientID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objPatientVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitId"]);
                        objPatientVO.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                        //objPatientVO.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                        //objPatientVO.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                        //objPatientVO.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));

                        objPatientVO.LastName = (string)DALHelper.HandleDBNull(reader["LastName"]);
                        objPatientVO.FirstName = (string)DALHelper.HandleDBNull(reader["FirstName"]);
                        objPatientVO.MiddleName = (string)DALHelper.HandleDBNull(reader["MiddleName"]);


                        objPatientVO.DateOfBirth = (DateTime?)DALHelper.HandleDate(reader["DateofBirth"]);
                        objPatientVO.RegistrationDate = (DateTime)DALHelper.HandleDate(reader["RegistrationDate"]);
                        if (BizActionObj.VisitWise == true || BizActionObj.RegistrationWise == true)
                        {
                            objPatientVO.VisitID = (long)DALHelper.HandleDBNull(reader["VAID"]);
                            objPatientVO.OPDNO = (string)DALHelper.HandleDBNull(reader["OINO"]);

                        }
                        else
                        {
                            objPatientVO.IPDAdmissionID = (long)DALHelper.HandleDBNull(reader["VAID"]);
                            objPatientVO.IPDAdmissionNo = (string)DALHelper.HandleDBNull(reader["OINO"]);
                        }

                        if (BizActionObj.VisitWise == true)
                            objPatientVO.PatientKind = PatientsKind.OPD;
                        else if (BizActionObj.AdmissionWise == true)
                            objPatientVO.PatientKind = PatientsKind.IPD;
                        else if (objPatientVO.VisitID == 0 && objPatientVO.IPDAdmissionID == 0)
                            objPatientVO.PatientKind = PatientsKind.Registration;

                        objPatientVO.TariffID = (long)DALHelper.HandleDBNull(reader["TariffID"]);
                        objPatientVO.PatientSourceID = (long)DALHelper.HandleDBNull(reader["PatientSourceID"]);
                        objPatientVO.CompanyID = (long)DALHelper.HandleDBNull(reader["CompanyID"]);
                        objPatientVO.ContactNO1 = (string)DALHelper.HandleDBNull(reader["ContactNO1"]);
                        objPatientVO.Gender = (string)DALHelper.HandleDBNull(reader["Gender"]);
                        objPatientVO.MaritalStatus = (string)DALHelper.HandleDBNull(reader["MaritalStatus"]);
                        objPatientVO.UniversalID = Security.base64Decode((string)DALHelper.HandleDBNull(reader["CivilID"]));
                        objPatientVO.PatientTypeID = (long)DALHelper.HandleDBNull(reader["PatientCategoryID"]);
                        //objPatientVO.RegistrationType = (string)DALHelper.HandleDBNull(reader["RegistrationType"]);       // ..................BHUSHAN
                        //objPatientVO.SpouseID = (long)DALHelper.HandleDBNull(reader["SpouseID"]);
                        objPatientVO.LinkServer = BizActionObj.LinkServer;
                        BizActionObj.PatientDetailsList.Add(objPatientVO);
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


        //***//------------
        public override IValueObject GetPatientCoupleList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCoupleGeneralDetailsListBizActionVO BizActionObj = valueObject as clsGetCoupleGeneralDetailsListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GETDonorCoupleLinkedList");
                DbDataReader reader;

                dbServer.AddInParameter(command, "DonorID", DbType.String, BizActionObj.DonorID);
                dbServer.AddInParameter(command, "DonorUnitID", DbType.Int64, BizActionObj.DonorUnitID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.PatientDetailsList == null)
                        BizActionObj.PatientDetailsList = new List<clsPatientGeneralVO>();
                    while (reader.Read())
                    {
                        clsPatientGeneralVO objPatientVO = new clsPatientGeneralVO();

                        objPatientVO.PatientID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objPatientVO.PatientUnitID = (long)DALHelper.HandleDBNull(reader["UnitId"]);
                        objPatientVO.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                        objPatientVO.GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]));
                        objPatientVO.LastName = (string)DALHelper.HandleDBNull(reader["LastName"]);
                        objPatientVO.FirstName = (string)DALHelper.HandleDBNull(reader["FirstName"]);
                        objPatientVO.MiddleName = (string)DALHelper.HandleDBNull(reader["MiddleName"]);
                        objPatientVO.Gender = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"]));
                        objPatientVO.CompanyID = Convert.ToInt32(DALHelper.HandleDBNull(reader["CompanyID"]));
                        objPatientVO.TariffID = (long)DALHelper.HandleDBNull(reader["TariffID"]);
                        objPatientVO.PatientSourceID = (long)DALHelper.HandleDBNull(reader["PatientSourceID"]);
                        BizActionObj.PatientDetailsList.Add(objPatientVO);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }

            return BizActionObj;
        }

        //------------------

        #region IPD PatientDetails
        public override IValueObject GetIPDPatient(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetIPDPatientBizActionVO BizActionObj = valueObject as clsGetIPDPatientBizActionVO;
            try
            {
                clsGetIPDPatientVO objPatientVO = BizActionObj.PatientDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientIPDDetails");
                DbDataReader reader;
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objPatientVO.GeneralDetails.PatientID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objPatientVO.GeneralDetails.UnitId);
                dbServer.AddInParameter(command, "MrNo", DbType.String, objPatientVO.GeneralDetails.MRNo);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizActionObj.PatientDetails.GeneralDetails.IPDAdmissionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizActionObj.PatientDetails.GeneralDetails.IPDPatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        BizActionObj.PatientDetails.GeneralDetails.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        BizActionObj.PatientDetails.GeneralDetails.IPDAdmissionNo = Convert.ToString(DALHelper.HandleDBNull(reader["IPDNO"]));
                        BizActionObj.PatientDetails.GeneralDetails.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"]));
                        BizActionObj.PatientDetails.GeneralDetails.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizActionObj.PatientDetails.GeneralDetails.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        BizActionObj.PatientDetails.GeneralDetails.BedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToBedID"]));
                        BizActionObj.PatientDetails.GeneralDetails.WardID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToWardID"]));
                        BizActionObj.PatientDetails.GeneralDetails.ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToBedCategoryID"]));
                        BizActionObj.PatientDetails.GeneralDetails.BedName = Convert.ToString(DALHelper.HandleDBNull(reader["BedName"]));
                        BizActionObj.PatientDetails.GeneralDetails.WardName = Convert.ToString(DALHelper.HandleDBNull(reader["WardName"]));
                        BizActionObj.PatientDetails.GeneralDetails.ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]));
                        BizActionObj.PatientDetails.GeneralDetails.RegistrationDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                        BizActionObj.PatientDetails.GeneralDetails.IsDischarged = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDischarged"]));
                        BizActionObj.PatientDetails.GeneralDetails.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                        BizActionObj.PatientDetails.GeneralDetails.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                        BizActionObj.PatientDetails.GeneralDetails.Unit = Convert.ToString(DALHelper.HandleDBNull(reader["Unit"]));
                        BizActionObj.PatientDetails.GeneralDetails.TariffID = Convert.ToInt64(reader["PatientTariffID"]);
                        BizActionObj.PatientDetails.GeneralDetails.IsReadyForDischarged = Convert.ToBoolean(reader["IsReadyForDischarged"]);
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

        public override IValueObject GetIPDPatientList(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetIPDPatientListBizActionVO BizActionObj = valueObject as clsGetIPDPatientListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetIPDPatientList");
                DbDataReader reader;
                dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.GeneralDetails.FirstName);
                dbServer.AddInParameter(command, "MiddleName", DbType.String, BizActionObj.GeneralDetails.MiddleName);
                dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.GeneralDetails.LastName);
                dbServer.AddInParameter(command, "FamilyName", DbType.String, BizActionObj.GeneralDetails.FamilyName);
                dbServer.AddInParameter(command, "MRNO", DbType.String, BizActionObj.GeneralDetails.MRNo);
                dbServer.AddInParameter(command, "AdmissionNO", DbType.String, BizActionObj.GeneralDetails.IPDAdmissionNo);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.GeneralDetails.PatientUnitID);

                dbServer.AddInParameter(command, "IsFromDischarge", DbType.Boolean, BizActionObj.IsFromDischarge);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.IPDPatientList == null)
                        BizActionObj.IPDPatientList = new List<clsPatientGeneralVO>();
                    while (reader.Read())
                    {
                        clsPatientGeneralVO IPDPatientVO = new clsPatientGeneralVO();
                        IPDPatientVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        IPDPatientVO.IPDAdmissionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionID"]));
                        IPDPatientVO.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        IPDPatientVO.FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]));
                        IPDPatientVO.MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"]));
                        IPDPatientVO.FamilyName = Convert.ToString(DALHelper.HandleDBNull(reader["FamilyName"]));
                        IPDPatientVO.DateOfBirth = Convert.ToDateTime(DALHelper.HandleDBNull(reader["DOB"]));
                        IPDPatientVO.LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]));
                        IPDPatientVO.IPDAdmissionNo = Convert.ToString(DALHelper.HandleDBNull(reader["IPDNO"]));
                        IPDPatientVO.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        IPDPatientVO.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        IPDPatientVO.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        IPDPatientVO.BedName = Convert.ToString(DALHelper.HandleDBNull(reader["BedName"]));
                        IPDPatientVO.BedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedID"]));
                        IPDPatientVO.WardName = Convert.ToString(DALHelper.HandleDBNull(reader["WardName"]));
                        IPDPatientVO.WardID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WardID"]));
                        IPDPatientVO.ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]));
                        IPDPatientVO.ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassID"]));
                        IPDPatientVO.RegistrationDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["AdmissionDate"]));
                        IPDPatientVO.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"]));
                        IPDPatientVO.PatientSourceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceID"]));

                        IPDPatientVO.BillingToBedCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillingToBedCategoryID"]));  //For IPD Billing Class ID

                        IPDPatientVO.AdmissionTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionType"]));
                        IPDPatientVO.AdmissionUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionUnitID"])); //Added by Bhushanp 09/01/2017
                        BizActionObj.IPDPatientList.Add(IPDPatientVO);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();

            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject AddPatientIPDWithTransaction(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientBizActionVO BizActionObj = valueObject as clsAddPatientBizActionVO;        //

            if (BizActionObj.PatientDetails.GeneralDetails.PatientID == 0)
                BizActionObj = AddPatientDetailsIPDWithTransaction(BizActionObj, UserVo);
            else
                BizActionObj = InsertFromOPDPatientDetailsIPDWithTransaction(BizActionObj, UserVo);

            return valueObject;
        }

        private clsAddPatientBizActionVO AddPatientDetailsIPDWithTransaction(clsAddPatientBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = dbServer.CreateConnection();

            try
            {
                clsPatientVO objPatientVO = BizActionObj.PatientDetails;

                con.Open();
                trans = con.BeginTransaction();

                if (BizActionObj.IsSavePatientFromIPD == true && BizActionObj.IsSaveInTRegistration == true)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPatient");

                    dbServer.AddInParameter(command, "LinkServer", DbType.String, objPatientVO.GeneralDetails.LinkServer);
                    if (objPatientVO.GeneralDetails.LinkServer != null)
                        dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objPatientVO.GeneralDetails.LinkServer.Replace(@"\", "_"));

                    dbServer.AddInParameter(command, "PatientCategoryID", DbType.Int64, objPatientVO.GeneralDetails.PatientTypeID);

                    // BY BHUSHAN . . . . 
                    dbServer.AddInParameter(command, "ReferralTypeID", DbType.Int64, objPatientVO.GeneralDetails.ReferralTypeID);

                    dbServer.AddInParameter(command, "ReferralDoctorID", DbType.Int64, objPatientVO.GeneralDetails.ReferralDoctorID);
                    dbServer.AddInParameter(command, "ReferredToDoctorID", DbType.Int64, objPatientVO.GeneralDetails.ReferredToDoctorID);
                    dbServer.AddInParameter(command, "ReferralDetail", DbType.String, objPatientVO.GeneralDetails.ReferralDetail);
                    dbServer.AddInParameter(command, "CompanyName", DbType.String, Security.base64Encode(objPatientVO.CompanyName));
                    // END . . . .

                    dbServer.AddInParameter(command, "RegistrationDate", DbType.DateTime, objPatientVO.GeneralDetails.RegistrationDate);
                    if (objPatientVO.GeneralDetails.LastName != null) objPatientVO.GeneralDetails.LastName = objPatientVO.GeneralDetails.LastName.Trim();
                    dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.LastName));
                    if (objPatientVO.GeneralDetails.FirstName != null) objPatientVO.GeneralDetails.FirstName = objPatientVO.GeneralDetails.FirstName.Trim();
                    dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.FirstName));
                    if (objPatientVO.GeneralDetails.MiddleName != null) objPatientVO.GeneralDetails.MiddleName = objPatientVO.GeneralDetails.MiddleName.Trim();
                    dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.MiddleName));
                    if (objPatientVO.FamilyName != null) objPatientVO.FamilyName = objPatientVO.FamilyName.Trim();
                    dbServer.AddInParameter(command, "FamilyName", DbType.String, Security.base64Encode(objPatientVO.FamilyName));
                    dbServer.AddInParameter(command, "GenderID", DbType.Int64, objPatientVO.GenderID);
                    dbServer.AddInParameter(command, "DateOfBirth", DbType.DateTime, objPatientVO.GeneralDetails.DateOfBirth);
                    dbServer.AddInParameter(command, "BloodGroupID", DbType.Int64, objPatientVO.BloodGroupID);
                    dbServer.AddInParameter(command, "MaritalStatusID", DbType.Int64, objPatientVO.MaritalStatusID);
                    dbServer.AddInParameter(command, "CivilID", DbType.String, Security.base64Encode(objPatientVO.CivilID));
                    if (objPatientVO.ContactNo1 != null) objPatientVO.ContactNo1 = objPatientVO.ContactNo1.Trim();
                    dbServer.AddInParameter(command, "ContactNo1", DbType.String, objPatientVO.ContactNo1);
                    if (objPatientVO.ContactNo2 != null) objPatientVO.ContactNo2 = objPatientVO.ContactNo2.Trim();
                    dbServer.AddInParameter(command, "ContactNo2", DbType.String, objPatientVO.ContactNo2);
                    if (objPatientVO.FaxNo != null) objPatientVO.FaxNo = objPatientVO.FaxNo.Trim();
                    dbServer.AddInParameter(command, "FaxNo", DbType.String, objPatientVO.FaxNo);
                    if (objPatientVO.Email != null) objPatientVO.Email = objPatientVO.Email.Trim();
                    dbServer.AddInParameter(command, "Email", DbType.String, Security.base64Encode(objPatientVO.Email));
                    if (objPatientVO.AddressLine1 != null) objPatientVO.AddressLine1 = objPatientVO.AddressLine1.Trim();
                    dbServer.AddInParameter(command, "AddressLine1", DbType.String, Security.base64Encode(objPatientVO.AddressLine1));
                    if (objPatientVO.AddressLine2 != null) objPatientVO.AddressLine2 = objPatientVO.AddressLine2.Trim();
                    dbServer.AddInParameter(command, "AddressLine2", DbType.String, Security.base64Encode(objPatientVO.AddressLine2));
                    if (objPatientVO.AddressLine3 != null) objPatientVO.AddressLine3 = objPatientVO.AddressLine3.Trim();
                    dbServer.AddInParameter(command, "AddressLine3", DbType.String, Security.base64Encode(objPatientVO.AddressLine3));
                    if (objPatientVO.Country != null) objPatientVO.Country = objPatientVO.Country.Trim();

                    //added by neena
                    if (objPatientVO.OldRegistrationNo != null) objPatientVO.OldRegistrationNo = objPatientVO.OldRegistrationNo.Trim();
                    dbServer.AddInParameter(command, "OldRegistrationNo", DbType.String, objPatientVO.OldRegistrationNo);

                    //


                    dbServer.AddInParameter(command, "Country", DbType.String, null);
                    dbServer.AddInParameter(command, "State", DbType.String, null);
                    dbServer.AddInParameter(command, "City", DbType.String, null);
                    dbServer.AddInParameter(command, "Taluka", DbType.String, null);
                    dbServer.AddInParameter(command, "Area", DbType.String, null);
                    dbServer.AddInParameter(command, "District", DbType.String, null);

                    if (objPatientVO.CountryID > 0)
                        dbServer.AddInParameter(command, "CountryID", DbType.Int64, objPatientVO.CountryID);
                    if (objPatientVO.StateID > 0)
                        dbServer.AddInParameter(command, "StateID", DbType.Int64, objPatientVO.StateID);
                    if (objPatientVO.CityID > 0)
                        dbServer.AddInParameter(command, "CityID", DbType.Int64, objPatientVO.CityID);
                    if (objPatientVO.RegionID > 0)
                        dbServer.AddInParameter(command, "RegionID", DbType.Int64, objPatientVO.RegionID);

                    if (objPatientVO.PrefixId > 0)
                        dbServer.AddInParameter(command, "PrefixId", DbType.Int64, objPatientVO.PrefixId);
                    if (objPatientVO.IdentityID > 0)
                        dbServer.AddInParameter(command, "IdentityID", DbType.Int64, objPatientVO.IdentityID);
                    dbServer.AddInParameter(command, "IdentityNumber", DbType.String, objPatientVO.IdentityNumber);
                    dbServer.AddInParameter(command, "RemarkForPatientType", DbType.String, objPatientVO.RemarkForPatientType);
                    dbServer.AddInParameter(command, "IsInternationalPatient", DbType.Boolean, objPatientVO.IsInternationalPatient);
                    dbServer.AddInParameter(command, "Education", DbType.String, objPatientVO.Education);

                    //.............................................................................................................................
                    dbServer.AddInParameter(command, "MobileCountryCode", DbType.String, objPatientVO.MobileCountryCode);
                    dbServer.AddInParameter(command, "ResiNoCountryCode", DbType.Int64, objPatientVO.ResiNoCountryCode);
                    dbServer.AddInParameter(command, "ResiSTDCode", DbType.Int64, objPatientVO.ResiSTDCode);
                    if (objPatientVO.Pincode != null) objPatientVO.Pincode = objPatientVO.Pincode.Trim();
                    dbServer.AddInParameter(command, "Pincode", DbType.String, Security.base64Encode(objPatientVO.Pincode));
                    dbServer.AddInParameter(command, "ReligionID", DbType.Int64, objPatientVO.ReligionID);
                    dbServer.AddInParameter(command, "OccupationId", DbType.Int64, objPatientVO.OccupationId);

                    dbServer.AddInParameter(command, "Photo", DbType.Binary, objPatientVO.Photo);  //commented by neena

                    dbServer.AddInParameter(command, "IsLoyaltyMember", DbType.Boolean, objPatientVO.IsLoyaltyMember);
                    dbServer.AddInParameter(command, "LoyaltyCardID", DbType.Int64, objPatientVO.LoyaltyCardID);
                    dbServer.AddInParameter(command, "RelationID", DbType.Int64, objPatientVO.RelationID);
                    dbServer.AddInParameter(command, "ParentPatientID", DbType.Int64, objPatientVO.ParentPatientID);
                    dbServer.AddInParameter(command, "IssueDate", DbType.DateTime, objPatientVO.IssueDate);
                    dbServer.AddInParameter(command, "EffectiveDate", DbType.DateTime, objPatientVO.EffectiveDate);
                    dbServer.AddInParameter(command, "ExpiryDate", DbType.DateTime, objPatientVO.ExpiryDate);
                    if (objPatientVO.LoyaltyCardNo != null) objPatientVO.LoyaltyCardNo = objPatientVO.LoyaltyCardNo.Trim();
                    dbServer.AddInParameter(command, "LoyaltyCardNo", DbType.String, objPatientVO.LoyaltyCardNo);

                    dbServer.AddInParameter(command, "Status", DbType.Boolean, objPatientVO.Status);

                    dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objPatientVO.AddedDateTime);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    dbServer.AddParameter(command, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                    //dbServer.AddParameter(command, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "????????????????????");          // Commented on 15062018 For New MRNO format
                    dbServer.AddParameter(command, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????");  // Added on 15062018 For New MRNO format
                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.GeneralDetails.PatientID);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                    //added by neena
                    Random random = new Random();
                    long RandomNumber = random.Next(111111, 666666);
                    dbServer.AddInParameter(command, "RandomNumber", DbType.String, Convert.ToString(RandomNumber));
                    //dbServer.AddParameter(command, "ImagePath", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.ImageName);
                    //

                    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    //int intStatus = dbServer.ExecuteNonQuery(command);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    BizActionObj.PatientDetails.GeneralDetails.PatientID = (long)dbServer.GetParameterValue(command, "ID");
                    BizActionObj.PatientDetails.GeneralDetails.MRNo = (string)dbServer.GetParameterValue(command, "MRNo");
                    //BizActionObj.PatientDetails.ImageName = (string)dbServer.GetParameterValue(command, "ImagePath");  //added by neena
                    string Err = (string)dbServer.GetParameterValue(command, "Err");
                    BizActionObj.PatientDetails.GeneralDetails.UnitId = UserVo.UserLoginInfo.UnitId;

                    //added by neena
                    string ImgName = BizActionObj.PatientDetails.GeneralDetails.MRNo + "_" + RandomNumber + ".png";
                    DemoImage ObjDM = new DemoImage();
                    if (objPatientVO.Photo != null)
                        ObjDM.VaryQualityLevel(objPatientVO.Photo, ImgName, ImgSaveLocation);

                    //File.WriteAllBytes(@"D:\Palash\PALASH_PATIENTREGISTRATION\IMAGE\" + ImgName, objPatientVO.Photo);
                    //

                    #region Commented by Ashish Z. on 29-oct-2015
                    //DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPatient");

                    //dbServer.AddInParameter(command, "LinkServer", DbType.String, objPatientVO.GeneralDetails.LinkServer);
                    //if (objPatientVO.GeneralDetails.LinkServer != null)
                    //    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objPatientVO.GeneralDetails.LinkServer.Replace(@"\", "_"));

                    //dbServer.AddInParameter(command, "PatientCategoryID", DbType.Int64, objPatientVO.GeneralDetails.PatientTypeID);

                    //// BY BHUSHAN . . . . 
                    //dbServer.AddInParameter(command, "ReferralTypeID", DbType.Int64, objPatientVO.GeneralDetails.ReferralTypeID);
                    //dbServer.AddInParameter(command, "CompanyName", DbType.String, Security.base64Encode(objPatientVO.CompanyName));
                    //// END . . . .

                    //dbServer.AddInParameter(command, "RegistrationDate", DbType.DateTime, objPatientVO.GeneralDetails.RegistrationDate);
                    //if (objPatientVO.GeneralDetails.LastName != null) objPatientVO.GeneralDetails.LastName = objPatientVO.GeneralDetails.LastName.Trim();
                    //dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.LastName));
                    //if (objPatientVO.GeneralDetails.FirstName != null) objPatientVO.GeneralDetails.FirstName = objPatientVO.GeneralDetails.FirstName.Trim();
                    //dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.FirstName));
                    //if (objPatientVO.GeneralDetails.MiddleName != null) objPatientVO.GeneralDetails.MiddleName = objPatientVO.GeneralDetails.MiddleName.Trim();
                    //dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.MiddleName));
                    //if (objPatientVO.FamilyName != null) objPatientVO.FamilyName = objPatientVO.FamilyName.Trim();
                    //dbServer.AddInParameter(command, "FamilyName", DbType.String, Security.base64Encode(objPatientVO.FamilyName));
                    //dbServer.AddInParameter(command, "GenderID", DbType.Int64, objPatientVO.GenderID);
                    //dbServer.AddInParameter(command, "DateOfBirth", DbType.DateTime, objPatientVO.GeneralDetails.DateOfBirth);
                    //dbServer.AddInParameter(command, "BloodGroupID", DbType.Int64, objPatientVO.BloodGroupID);
                    //dbServer.AddInParameter(command, "MaritalStatusID", DbType.Int64, objPatientVO.MaritalStatusID);
                    //dbServer.AddInParameter(command, "IsIPDPatient", DbType.Boolean, objPatientVO.IsIPDPatient);  ////IPD
                    //dbServer.AddInParameter(command, "CivilID", DbType.String, Security.base64Encode(objPatientVO.CivilID));
                    //if (objPatientVO.ContactNo1 != null) objPatientVO.ContactNo1 = objPatientVO.ContactNo1.Trim();
                    //dbServer.AddInParameter(command, "ContactNo1", DbType.String, objPatientVO.ContactNo1);
                    //if (objPatientVO.ContactNo2 != null) objPatientVO.ContactNo2 = objPatientVO.ContactNo2.Trim();
                    //dbServer.AddInParameter(command, "ContactNo2", DbType.String, objPatientVO.ContactNo2);
                    ////if (objPatientVO.MobileNo2 != null) objPatientVO.MobileNo2 = objPatientVO.MobileNo2.Trim();  ////IPD
                    ////dbServer.AddInParameter(command, "MobileNo2", DbType.String, objPatientVO.MobileNo2);  ////IPD
                    //if (objPatientVO.FaxNo != null) objPatientVO.FaxNo = objPatientVO.FaxNo.Trim();
                    //dbServer.AddInParameter(command, "FaxNo", DbType.String, objPatientVO.FaxNo);
                    //if (objPatientVO.Email != null) objPatientVO.Email = objPatientVO.Email.Trim();
                    //dbServer.AddInParameter(command, "Email", DbType.String, Security.base64Encode(objPatientVO.Email));
                    //if (objPatientVO.AddressLine1 != null) objPatientVO.AddressLine1 = objPatientVO.AddressLine1.Trim();
                    //dbServer.AddInParameter(command, "AddressLine1", DbType.String, Security.base64Encode(objPatientVO.AddressLine1));
                    //if (objPatientVO.AddressLine2 != null) objPatientVO.AddressLine2 = objPatientVO.AddressLine2.Trim();
                    //dbServer.AddInParameter(command, "AddressLine2", DbType.String, Security.base64Encode(objPatientVO.AddressLine2));
                    //if (objPatientVO.AddressLine3 != null) objPatientVO.AddressLine3 = objPatientVO.AddressLine3.Trim();
                    //dbServer.AddInParameter(command, "AddressLine3", DbType.String, Security.base64Encode(objPatientVO.AddressLine3));
                    //if (objPatientVO.Country != null) objPatientVO.Country = objPatientVO.Country.Trim();

                    ////By Anjali........................................................................................................
                    //if (objPatientVO.Country != null) objPatientVO.Country = objPatientVO.Country.Trim();
                    //dbServer.AddInParameter(command, "Country", DbType.String, Security.base64Encode(objPatientVO.Country));
                    //if (objPatientVO.State != null) objPatientVO.State = objPatientVO.State.Trim();
                    //dbServer.AddInParameter(command, "State", DbType.String, Security.base64Encode(objPatientVO.State));
                    //if (objPatientVO.City != null) objPatientVO.City = objPatientVO.City.Trim();
                    //dbServer.AddInParameter(command, "City", DbType.String, Security.base64Encode(objPatientVO.City));
                    //if (objPatientVO.Taluka != null) objPatientVO.Taluka = objPatientVO.Taluka.Trim();
                    //dbServer.AddInParameter(command, "Taluka", DbType.String, Security.base64Encode(objPatientVO.Taluka));
                    //if (objPatientVO.Area != null) objPatientVO.Area = objPatientVO.Area.Trim();
                    //dbServer.AddInParameter(command, "Area", DbType.String, Security.base64Encode(objPatientVO.Area));
                    //if (objPatientVO.District != null) objPatientVO.District = objPatientVO.District.Trim();
                    //dbServer.AddInParameter(command, "District", DbType.String, Security.base64Encode(objPatientVO.District));

                    ////dbServer.AddInParameter(command, "Country", DbType.String, null);
                    ////dbServer.AddInParameter(command, "State", DbType.String, null);
                    ////dbServer.AddInParameter(command, "City", DbType.String, null);
                    ////dbServer.AddInParameter(command, "Taluka", DbType.String, null);
                    ////dbServer.AddInParameter(command, "Area", DbType.String, null);
                    ////dbServer.AddInParameter(command, "District", DbType.String, null);

                    ////if (objPatientVO.CountryID > 0)
                    ////    dbServer.AddInParameter(command, "CountryID", DbType.Int64, objPatientVO.CountryID);
                    ////if (objPatientVO.StateID > 0)
                    ////    dbServer.AddInParameter(command, "StateID", DbType.Int64, objPatientVO.StateID);
                    ////if (objPatientVO.CityID > 0)
                    ////    dbServer.AddInParameter(command, "CityID", DbType.Int64, objPatientVO.CityID);
                    ////if (objPatientVO.RegionID > 0)
                    ////    dbServer.AddInParameter(command, "RegionID", DbType.Int64, objPatientVO.RegionID);

                    ////.............................................................................................................................
                    //dbServer.AddInParameter(command, "MobileCountryCode", DbType.String, objPatientVO.MobileCountryCode);
                    //dbServer.AddInParameter(command, "ResiNoCountryCode", DbType.Int64, objPatientVO.ResiNoCountryCode);
                    //dbServer.AddInParameter(command, "ResiSTDCode", DbType.Int64, objPatientVO.ResiSTDCode);
                    //if (objPatientVO.Pincode != null) objPatientVO.Pincode = objPatientVO.Pincode.Trim();
                    //dbServer.AddInParameter(command, "Pincode", DbType.String, Security.base64Encode(objPatientVO.Pincode));
                    //dbServer.AddInParameter(command, "ReligionID", DbType.Int64, objPatientVO.ReligionID);
                    //dbServer.AddInParameter(command, "OccupationId", DbType.Int64, objPatientVO.OccupationId);
                    //dbServer.AddInParameter(command, "Photo", DbType.Binary, objPatientVO.Photo);
                    //dbServer.AddInParameter(command, "IsLoyaltyMember", DbType.Boolean, objPatientVO.IsLoyaltyMember);
                    //dbServer.AddInParameter(command, "LoyaltyCardID", DbType.Int64, objPatientVO.LoyaltyCardID);
                    //dbServer.AddInParameter(command, "RelationID", DbType.Int64, objPatientVO.RelationID);
                    //dbServer.AddInParameter(command, "ParentPatientID", DbType.Int64, objPatientVO.ParentPatientID);
                    //dbServer.AddInParameter(command, "IssueDate", DbType.DateTime, objPatientVO.IssueDate);
                    //dbServer.AddInParameter(command, "EffectiveDate", DbType.DateTime, objPatientVO.EffectiveDate);
                    //dbServer.AddInParameter(command, "ExpiryDate", DbType.DateTime, objPatientVO.ExpiryDate);
                    //if (objPatientVO.LoyaltyCardNo != null) objPatientVO.LoyaltyCardNo = objPatientVO.LoyaltyCardNo.Trim();
                    //dbServer.AddInParameter(command, "LoyaltyCardNo", DbType.String, objPatientVO.LoyaltyCardNo);

                    //dbServer.AddInParameter(command, "Status", DbType.Boolean, objPatientVO.Status);

                    //dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    //dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                    //dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    //dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    //dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objPatientVO.AddedDateTime);
                    //dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    ////dbServer.AddInParameter(command, "CompanyName", DbType.String, objPatientVO.CompanyName); // Added BY Aadhar  ////IPD

                    ////dbServer.AddInParameter(command, "MRNo", DbType.String, objPatientVO.GeneralDetails.MRNo);
                    //dbServer.AddParameter(command, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                    //dbServer.AddParameter(command, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "????????????????????");
                    //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.GeneralDetails.PatientID);
                    //dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                    //int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    ////int intStatus = dbServer.ExecuteNonQuery(command);
                    //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    //BizActionObj.PatientDetails.GeneralDetails.PatientID = (long)dbServer.GetParameterValue(command, "ID");
                    //BizActionObj.PatientDetails.GeneralDetails.MRNo = (string)dbServer.GetParameterValue(command, "MRNo");
                    //string Err = (string)dbServer.GetParameterValue(command, "Err");
                    //BizActionObj.PatientDetails.GeneralDetails.UnitId = UserVo.UserLoginInfo.UnitId;
                    #endregion

                    if (BizActionObj.PatientDetails.SpouseDetails != null && (BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 7 || BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 8 || BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 9))
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPatient");
                        dbServer.AddInParameter(command1, "PatientCategoryID", DbType.Int64, objPatientVO.GeneralDetails.PatientTypeID);
                        // BY BHSUAN . . . .
                        dbServer.AddInParameter(command, "ReferralTypeID", DbType.Int64, objPatientVO.GeneralDetails.ReferralTypeID);
                        //
                        dbServer.AddInParameter(command1, "RegistrationDate", DbType.DateTime, objPatientVO.GeneralDetails.RegistrationDate);

                        if (objPatientVO.SpouseDetails.LastName != null) objPatientVO.SpouseDetails.LastName = objPatientVO.SpouseDetails.LastName.Trim();
                        dbServer.AddInParameter(command1, "LastName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.LastName));
                        if (objPatientVO.SpouseDetails.FirstName != null) objPatientVO.SpouseDetails.FirstName = objPatientVO.SpouseDetails.FirstName.Trim();
                        dbServer.AddInParameter(command1, "FirstName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.FirstName));
                        if (objPatientVO.SpouseDetails.MiddleName != null) objPatientVO.SpouseDetails.MiddleName = objPatientVO.SpouseDetails.MiddleName.Trim();
                        dbServer.AddInParameter(command1, "MiddleName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.MiddleName));
                        if (objPatientVO.SpouseDetails.FamilyName != null) objPatientVO.SpouseDetails.FamilyName = objPatientVO.SpouseDetails.FamilyName.Trim();
                        dbServer.AddInParameter(command1, "FamilyName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.FamilyName));
                        dbServer.AddInParameter(command1, "GenderID", DbType.Int64, objPatientVO.SpouseDetails.GenderID);
                        dbServer.AddInParameter(command1, "DateOfBirth", DbType.DateTime, objPatientVO.SpouseDetails.DateOfBirth);
                        dbServer.AddInParameter(command1, "BloodGroupID", DbType.Int64, objPatientVO.SpouseDetails.BloodGroupID);
                        dbServer.AddInParameter(command1, "MaritalStatusID", DbType.Int64, objPatientVO.SpouseDetails.MaritalStatusID);
                        dbServer.AddInParameter(command1, "CivilID", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.CivilID));
                        if (objPatientVO.SpouseDetails.ContactNo1 != null) objPatientVO.SpouseDetails.ContactNo1 = objPatientVO.SpouseDetails.ContactNo1.Trim();
                        dbServer.AddInParameter(command1, "ContactNo1", DbType.String, objPatientVO.SpouseDetails.ContactNo1);
                        if (objPatientVO.SpouseDetails.ContactNo2 != null) objPatientVO.SpouseDetails.ContactNo2 = objPatientVO.SpouseDetails.ContactNo2.Trim();
                        dbServer.AddInParameter(command1, "ContactNo2", DbType.String, objPatientVO.SpouseDetails.ContactNo2);
                        if (objPatientVO.SpouseDetails.FaxNo != null) objPatientVO.SpouseDetails.FaxNo = objPatientVO.SpouseDetails.FaxNo.Trim();
                        dbServer.AddInParameter(command1, "FaxNo", DbType.String, objPatientVO.SpouseDetails.FaxNo);
                        if (objPatientVO.SpouseDetails.Email != null) objPatientVO.SpouseDetails.Email = objPatientVO.SpouseDetails.Email.Trim();
                        dbServer.AddInParameter(command1, "Email", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Email));
                        if (objPatientVO.SpouseDetails.AddressLine1 != null) objPatientVO.SpouseDetails.AddressLine1 = objPatientVO.SpouseDetails.AddressLine1.Trim();
                        dbServer.AddInParameter(command1, "AddressLine1", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.AddressLine1));
                        if (objPatientVO.SpouseDetails.AddressLine2 != null) objPatientVO.SpouseDetails.AddressLine2 = objPatientVO.SpouseDetails.AddressLine2.Trim();
                        dbServer.AddInParameter(command1, "AddressLine2", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.AddressLine2));
                        if (objPatientVO.SpouseDetails.AddressLine3 != null) objPatientVO.SpouseDetails.AddressLine3 = objPatientVO.SpouseDetails.AddressLine3.Trim();
                        dbServer.AddInParameter(command1, "AddressLine3", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.AddressLine3));
                        if (objPatientVO.SpouseDetails.Country != null) objPatientVO.SpouseDetails.Country = objPatientVO.SpouseDetails.Country.Trim();

                        //By Anjali....................................................................................

                        if (objPatientVO.SpouseDetails.Country != null) objPatientVO.SpouseDetails.Country = objPatientVO.SpouseDetails.Country.Trim();
                        dbServer.AddInParameter(command1, "Country", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Country));
                        if (objPatientVO.SpouseDetails.State != null) objPatientVO.SpouseDetails.State = objPatientVO.SpouseDetails.State.Trim();
                        dbServer.AddInParameter(command1, "State", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.State));
                        if (objPatientVO.SpouseDetails.City != null) objPatientVO.SpouseDetails.City = objPatientVO.SpouseDetails.City.Trim();
                        dbServer.AddInParameter(command1, "City", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.City));
                        if (objPatientVO.SpouseDetails.Taluka != null) objPatientVO.SpouseDetails.Taluka = objPatientVO.SpouseDetails.Taluka.Trim();
                        dbServer.AddInParameter(command1, "Taluka", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Taluka));
                        if (objPatientVO.SpouseDetails.Area != null) objPatientVO.SpouseDetails.Area = objPatientVO.SpouseDetails.Area.Trim();
                        dbServer.AddInParameter(command1, "Area", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Area));
                        if (objPatientVO.SpouseDetails.District != null) objPatientVO.SpouseDetails.District = objPatientVO.SpouseDetails.District.Trim();
                        dbServer.AddInParameter(command1, "District", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.District));

                        //added by neena
                        if (objPatientVO.SpouseDetails.SpouseOldRegistrationNo != null) objPatientVO.SpouseDetails.SpouseOldRegistrationNo = objPatientVO.SpouseDetails.SpouseOldRegistrationNo.Trim();
                        dbServer.AddInParameter(command1, "OldRegistrationNo", DbType.String, objPatientVO.SpouseDetails.SpouseOldRegistrationNo);

                        //

                        //dbServer.AddInParameter(command, "Country", DbType.String, null);
                        //dbServer.AddInParameter(command, "State", DbType.String, null);
                        //dbServer.AddInParameter(command, "City", DbType.String, null);
                        //dbServer.AddInParameter(command, "Taluka", DbType.String, null);
                        //dbServer.AddInParameter(command, "Area", DbType.String, null);
                        //dbServer.AddInParameter(command, "District", DbType.String, null);


                        //if (objPatientVO.SpouseDetails.CountryID > 0)
                        //    dbServer.AddInParameter(command, "CountryID", DbType.Int64, objPatientVO.SpouseDetails.CountryID);
                        //if (objPatientVO.SpouseDetails.StateID > 0)
                        //    dbServer.AddInParameter(command, "StateID", DbType.Int64, objPatientVO.SpouseDetails.StateID);
                        //if (objPatientVO.SpouseDetails.CityID > 0)
                        //    dbServer.AddInParameter(command, "CityID", DbType.Int64, objPatientVO.SpouseDetails.CityID);
                        //if (objPatientVO.SpouseDetails.RegionID > 0)
                        //    dbServer.AddInParameter(command, "RegionID", DbType.Int64, objPatientVO.SpouseDetails.RegionID);

                        //.............................................................................................................................

                        dbServer.AddInParameter(command1, "MobileCountryCode", DbType.String, objPatientVO.SpouseDetails.MobileCountryCode);
                        dbServer.AddInParameter(command1, "ResiNoCountryCode", DbType.Int64, objPatientVO.SpouseDetails.ResiNoCountryCode);
                        dbServer.AddInParameter(command1, "ResiSTDCode", DbType.Int64, objPatientVO.SpouseDetails.ResiSTDCode);
                        if (objPatientVO.SpouseDetails.Pincode != null) objPatientVO.Pincode = objPatientVO.SpouseDetails.Pincode.Trim();
                        dbServer.AddInParameter(command1, "Pincode", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Pincode));

                        // BY BHUSHAN. .
                        dbServer.AddInParameter(command1, "CompanyName", DbType.String, Security.base64Encode(objPatientVO.CompanyName));

                        dbServer.AddInParameter(command1, "ReligionID", DbType.Int64, objPatientVO.SpouseDetails.ReligionID);
                        dbServer.AddInParameter(command1, "OccupationId", DbType.Int64, objPatientVO.SpouseDetails.OccupationId);

                        dbServer.AddInParameter(command1, "Photo", DbType.Binary, objPatientVO.SpouseDetails.Photo);   //commented by neena

                        //dbServer.AddInParameter(command1, "IsLoyaltyMember", DbType.Boolean, objPatientVO.IsLoyaltyMember);
                        //dbServer.AddInParameter(command1, "LoyaltyCardID", DbType.Int64, objPatientVO.LoyaltyCardID);
                        //dbServer.AddInParameter(command1, "RelationID", DbType.Int64, objPatientVO.RelationID);
                        //dbServer.AddInParameter(command1, "ParentPatientID", DbType.Int64, objPatientVO.ParentPatientID);
                        //dbServer.AddInParameter(command1, "IssueDate", DbType.DateTime, objPatientVO.IssueDate);
                        //dbServer.AddInParameter(command1, "EffectiveDate", DbType.DateTime, objPatientVO.EffectiveDate);
                        //dbServer.AddInParameter(command1, "ExpiryDate", DbType.DateTime, objPatientVO.ExpiryDate);
                        //if (objPatientVO.LoyaltyCardNo != null) objPatientVO.LoyaltyCardNo = objPatientVO.LoyaltyCardNo.Trim();
                        //dbServer.AddInParameter(command1, "LoyaltyCardNo", DbType.String, objPatientVO.LoyaltyCardNo);

                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, objPatientVO.Status);

                        dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, objPatientVO.AddedDateTime);
                        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        objPatientVO.SpouseDetails.MRNo = objPatientVO.GeneralDetails.MRNo.Remove(objPatientVO.GeneralDetails.MRNo.Length - 1, 1);
                        dbServer.AddParameter(command1, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        dbServer.AddParameter(command1, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.SpouseDetails.MRNo);
                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.SpouseDetails.PatientID);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                        //added by neena
                        Random random1 = new Random();
                        long RandomNumber1 = random1.Next(111111, 666666);
                        dbServer.AddInParameter(command1, "RandomNumber", DbType.String, Convert.ToString(RandomNumber1));
                        //dbServer.AddParameter(command1, "ImagePath", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.SpouseDetails.ImageName);
                        //

                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                        //int intStatus = dbServer.ExecuteNonQuery(command1);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                        BizActionObj.PatientDetails.SpouseDetails.PatientID = (long)dbServer.GetParameterValue(command1, "ID");
                        BizActionObj.PatientDetails.SpouseDetails.MRNo = (string)dbServer.GetParameterValue(command1, "MRNo");
                        //BizActionObj.PatientDetails.SpouseDetails.ImageName = (string)dbServer.GetParameterValue(command1, "ImagePath");  //added by neena                    
                        string Err1 = (string)dbServer.GetParameterValue(command1, "Err");
                        BizActionObj.PatientDetails.SpouseDetails.UnitId = UserVo.UserLoginInfo.UnitId;


                        //added by neena
                        string ImgName1 = BizActionObj.PatientDetails.SpouseDetails.MRNo + "_" + RandomNumber1 + ".png";
                        //DemoImage ObjDM = new DemoImage();
                        if (objPatientVO.SpouseDetails.Photo != null)
                            ObjDM.VaryQualityLevel(objPatientVO.SpouseDetails.Photo, ImgName1, ImgSaveLocation);
                        //File.WriteAllBytes(@"D:\Palash\PALASH_PATIENTREGISTRATION\IMAGE\" + ImgName1, objPatientVO.SpouseDetails.Photo);
                        //

                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddCoupleRegistration");
                        if (BizActionObj.PatientDetails.GenderID == (long)Genders.Male)
                        {
                            dbServer.AddInParameter(command2, "MalePatientID", DbType.Int64, objPatientVO.GeneralDetails.PatientID);
                            dbServer.AddInParameter(command2, "FemalePatientID", DbType.Int64, objPatientVO.SpouseDetails.PatientID);
                        }
                        else
                        {
                            dbServer.AddInParameter(command2, "MalePatientID", DbType.Int64, objPatientVO.SpouseDetails.PatientID);
                            dbServer.AddInParameter(command2, "FemalePatientID", DbType.Int64, objPatientVO.GeneralDetails.PatientID);
                        }

                        dbServer.AddInParameter(command2, "MalePatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "FemalePatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                        dbServer.AddInParameter(command2, "RegistrationDate", DbType.DateTime, objPatientVO.GeneralDetails.RegistrationDate);

                        dbServer.AddInParameter(command2, "Status", DbType.Boolean, objPatientVO.Status);

                        dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);


                        dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, objPatientVO.AddedDateTime);
                        dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                        dbServer.AddParameter(command2, "CoupleRgistrationNumber", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "000000000000000000000000000000000000000000000000000");
                        dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                        dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                        //int intStatus = dbServer.ExecuteNonQuery(command1);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                        // BizActionObj.PatientDetails.SpouseDetails.PatientID = (long)dbServer.GetParameterValue(command1, "ID");
                        //BizActionObj.PatientDetails.SpouseDetails.MRNo = (string)dbServer.GetParameterValue(command1, "MRNo");
                        // string Err1 = (string)dbServer.GetParameterValue(command1, "Err");
                        //BizActionObj.PatientDetails.SpouseDetails.UnitId = UserVo.UserLoginInfo.UnitId;
                    }
                }

                if (objPatientVO.KinInformationList != null)
                {
                    foreach (clsKinInformationVO item in objPatientVO.KinInformationList)
                    {
                        DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_AddPatientKinInfoIPD");   //CIMS_AddPatientKinInfo
                        dbServer.AddInParameter(command3, "ID", DbType.Int64, 0);
                        dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.PatientID);
                        dbServer.AddInParameter(command3, "MRNo", DbType.String, item.MRCode);
                        dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.UnitId);
                        dbServer.AddInParameter(command3, "sbIsRegisteredPatient", DbType.String, item.IsRegisteredPatient);
                        //dbServer.AddInParameter(command3, "sbPatientID", DbType.String, objPatientVO.PatientID);
                        //dbServer.AddInParameter(command3, "sbPatientUnitID", DbType.String, objPatientVO.PatientUnitID);
                        dbServer.AddInParameter(command3, "sbKinPatientId", DbType.String, item.KinPatientID);
                        dbServer.AddInParameter(command3, "sbKinPatientUnitId", DbType.String, item.KinPatientUnitID);
                        dbServer.AddInParameter(command3, "sbLastName", DbType.String, Security.base64Encode(item.KinLastName));
                        dbServer.AddInParameter(command3, "sbFirstName", DbType.String, Security.base64Encode(item.KinFirstName));
                        dbServer.AddInParameter(command3, "sbMiddleName", DbType.String, Security.base64Encode(item.KinMiddleName));
                        dbServer.AddInParameter(command3, "sbFamilyName", DbType.String, Security.base64Encode(item.FamilyCode));
                        dbServer.AddInParameter(command3, "sbMobileCountryCode", DbType.String, item.MobileCountryCode);
                        dbServer.AddInParameter(command3, "sbMobileNumber", DbType.String, item.MobileCountryCode);
                        dbServer.AddInParameter(command3, "sbAddress", DbType.String, item.Address);
                        dbServer.AddInParameter(command3, "sbRelationshipID", DbType.String, item.RelationshipID);

                        dbServer.AddInParameter(command3, "Status", DbType.String, item.Status);
                        dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, objPatientVO.AddedDateTime);
                        dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                    }
                }

                if (BizActionObj.IsSaveSponsor == true)
                {
                    clsBasePatientSposorDAL objBaseDAL = clsBasePatientSposorDAL.GetInstance();
                    BizActionObj.BizActionVOSaveSponsor.PatientSponsorDetails.PatientId = BizActionObj.PatientDetails.GeneralDetails.PatientID;
                    BizActionObj.BizActionVOSaveSponsor.PatientSponsorDetails.PatientUnitId = BizActionObj.PatientDetails.GeneralDetails.UnitId;

                    BizActionObj.BizActionVOSaveSponsor = (clsAddPatientSponsorBizActionVO)objBaseDAL.AddPatientSponsorDetailsWithTransaction(BizActionObj.BizActionVOSaveSponsor, UserVo, con, trans);
                    if (BizActionObj.BizActionVOSaveSponsor.SuccessStatus == -1) throw new Exception();

                }

                if (BizActionObj.IsSaveAdmission == true)
                {
                    clsBaseIPDAdmissionDAL objBaseDAL = clsBaseIPDAdmissionDAL.GetInstance();
                    BizActionObj.BizActionVOSaveAdmission.Details.PatientId = BizActionObj.PatientDetails.GeneralDetails.PatientID;
                    BizActionObj.BizActionVOSaveAdmission.Details.PatientUnitID = BizActionObj.PatientDetails.GeneralDetails.UnitId;
                    BizActionObj.BizActionVOSaveAdmission.Details.IPDNO = BizActionObj.PatientDetails.GeneralDetails.MRNo;

                    BizActionObj.BizActionVOSaveAdmission = (clsSaveIPDAdmissionBizActionVO)objBaseDAL.AddIPDAdmissionDetailsWithTransaction(BizActionObj.BizActionVOSaveAdmission, UserVo, con, trans);
                    if (BizActionObj.BizActionVOSaveAdmission.SuccessStatus == -1) throw new Exception();
                }

                trans.Commit();
                addPatientUserDetails(BizActionObj, UserVo);


            }
            catch (Exception ex)
            {

                trans.Rollback();
                throw ex;
            }
            finally
            {
                trans.Dispose();
                trans = null;
            }
            return BizActionObj;
        }

        private clsAddPatientBizActionVO InsertFromOPDPatientDetailsIPDWithTransaction(clsAddPatientBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = dbServer.CreateConnection();

            try
            {
                clsPatientVO objPatientVO = BizActionObj.PatientDetails;

                con.Open();
                trans = con.BeginTransaction();

                //if (BizActionObj.IsSavePatientFromIPD == true)
                //{

                if (BizActionObj.IsSavePatientFromIPD == true && BizActionObj.IsSaveInTRegistration == true)
                {

                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPatient");

                    dbServer.AddInParameter(command, "LinkServer", DbType.String, objPatientVO.GeneralDetails.LinkServer);
                    if (objPatientVO.GeneralDetails.LinkServer != null)
                        dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objPatientVO.GeneralDetails.LinkServer.Replace(@"\", "_"));

                    dbServer.AddInParameter(command, "PatientCategoryID", DbType.Int64, objPatientVO.GeneralDetails.PatientTypeID);

                    // BY BHUSHAN . . . . 
                    dbServer.AddInParameter(command, "ReferralTypeID", DbType.Int64, objPatientVO.GeneralDetails.ReferralTypeID);
                    dbServer.AddInParameter(command, "CompanyName", DbType.String, Security.base64Encode(objPatientVO.CompanyName));
                    // END . . . .

                    dbServer.AddInParameter(command, "RegistrationDate", DbType.DateTime, objPatientVO.GeneralDetails.RegistrationDate);
                    if (objPatientVO.GeneralDetails.LastName != null) objPatientVO.GeneralDetails.LastName = objPatientVO.GeneralDetails.LastName.Trim();
                    dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.LastName));
                    if (objPatientVO.GeneralDetails.FirstName != null) objPatientVO.GeneralDetails.FirstName = objPatientVO.GeneralDetails.FirstName.Trim();
                    dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.FirstName));
                    if (objPatientVO.GeneralDetails.MiddleName != null) objPatientVO.GeneralDetails.MiddleName = objPatientVO.GeneralDetails.MiddleName.Trim();
                    dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.MiddleName));
                    if (objPatientVO.FamilyName != null) objPatientVO.FamilyName = objPatientVO.FamilyName.Trim();
                    dbServer.AddInParameter(command, "FamilyName", DbType.String, Security.base64Encode(objPatientVO.FamilyName));
                    dbServer.AddInParameter(command, "GenderID", DbType.Int64, objPatientVO.GenderID);
                    dbServer.AddInParameter(command, "DateOfBirth", DbType.DateTime, objPatientVO.GeneralDetails.DateOfBirth);
                    dbServer.AddInParameter(command, "BloodGroupID", DbType.Int64, objPatientVO.BloodGroupID);
                    dbServer.AddInParameter(command, "MaritalStatusID", DbType.Int64, objPatientVO.MaritalStatusID);
                    dbServer.AddInParameter(command, "IsIPDPatient", DbType.Boolean, objPatientVO.IsIPDPatient);  ////IPD
                    dbServer.AddInParameter(command, "CivilID", DbType.String, Security.base64Encode(objPatientVO.CivilID));
                    if (objPatientVO.ContactNo1 != null) objPatientVO.ContactNo1 = objPatientVO.ContactNo1.Trim();
                    dbServer.AddInParameter(command, "ContactNo1", DbType.String, objPatientVO.ContactNo1);
                    if (objPatientVO.ContactNo2 != null) objPatientVO.ContactNo2 = objPatientVO.ContactNo2.Trim();
                    dbServer.AddInParameter(command, "ContactNo2", DbType.String, objPatientVO.ContactNo2);
                    //if (objPatientVO.MobileNo2 != null) objPatientVO.MobileNo2 = objPatientVO.MobileNo2.Trim();  ////IPD
                    //dbServer.AddInParameter(command, "MobileNo2", DbType.String, objPatientVO.MobileNo2);  ////IPD
                    if (objPatientVO.FaxNo != null) objPatientVO.FaxNo = objPatientVO.FaxNo.Trim();
                    dbServer.AddInParameter(command, "FaxNo", DbType.String, objPatientVO.FaxNo);
                    if (objPatientVO.Email != null) objPatientVO.Email = objPatientVO.Email.Trim();
                    dbServer.AddInParameter(command, "Email", DbType.String, Security.base64Encode(objPatientVO.Email));
                    if (objPatientVO.AddressLine1 != null) objPatientVO.AddressLine1 = objPatientVO.AddressLine1.Trim();
                    dbServer.AddInParameter(command, "AddressLine1", DbType.String, Security.base64Encode(objPatientVO.AddressLine1));
                    if (objPatientVO.AddressLine2 != null) objPatientVO.AddressLine2 = objPatientVO.AddressLine2.Trim();
                    dbServer.AddInParameter(command, "AddressLine2", DbType.String, Security.base64Encode(objPatientVO.AddressLine2));
                    if (objPatientVO.AddressLine3 != null) objPatientVO.AddressLine3 = objPatientVO.AddressLine3.Trim();
                    dbServer.AddInParameter(command, "AddressLine3", DbType.String, Security.base64Encode(objPatientVO.AddressLine3));
                    if (objPatientVO.Country != null) objPatientVO.Country = objPatientVO.Country.Trim();

                    //added by neena
                    if (objPatientVO.OldRegistrationNo != null) objPatientVO.OldRegistrationNo = objPatientVO.OldRegistrationNo.Trim();
                    dbServer.AddInParameter(command, "OldRegistrationNo", DbType.String, objPatientVO.OldRegistrationNo);

                    //


                    //By Anjali........................................................................................................
                    if (objPatientVO.Country != null) objPatientVO.Country = objPatientVO.Country.Trim();
                    dbServer.AddInParameter(command, "Country", DbType.String, Security.base64Encode(objPatientVO.Country));
                    if (objPatientVO.State != null) objPatientVO.State = objPatientVO.State.Trim();
                    dbServer.AddInParameter(command, "State", DbType.String, Security.base64Encode(objPatientVO.State));
                    if (objPatientVO.City != null) objPatientVO.City = objPatientVO.City.Trim();
                    dbServer.AddInParameter(command, "City", DbType.String, Security.base64Encode(objPatientVO.City));
                    if (objPatientVO.Taluka != null) objPatientVO.Taluka = objPatientVO.Taluka.Trim();
                    dbServer.AddInParameter(command, "Taluka", DbType.String, Security.base64Encode(objPatientVO.Taluka));
                    if (objPatientVO.Area != null) objPatientVO.Area = objPatientVO.Area.Trim();
                    dbServer.AddInParameter(command, "Area", DbType.String, Security.base64Encode(objPatientVO.Area));
                    if (objPatientVO.District != null) objPatientVO.District = objPatientVO.District.Trim();
                    dbServer.AddInParameter(command, "District", DbType.String, Security.base64Encode(objPatientVO.District));

                    //dbServer.AddInParameter(command, "Country", DbType.String, null);
                    //dbServer.AddInParameter(command, "State", DbType.String, null);
                    //dbServer.AddInParameter(command, "City", DbType.String, null);
                    //dbServer.AddInParameter(command, "Taluka", DbType.String, null);
                    //dbServer.AddInParameter(command, "Area", DbType.String, null);
                    //dbServer.AddInParameter(command, "District", DbType.String, null);

                    //if (objPatientVO.CountryID > 0)
                    //    dbServer.AddInParameter(command, "CountryID", DbType.Int64, objPatientVO.CountryID);
                    //if (objPatientVO.StateID > 0)
                    //    dbServer.AddInParameter(command, "StateID", DbType.Int64, objPatientVO.StateID);
                    //if (objPatientVO.CityID > 0)
                    //    dbServer.AddInParameter(command, "CityID", DbType.Int64, objPatientVO.CityID);
                    //if (objPatientVO.RegionID > 0)
                    //    dbServer.AddInParameter(command, "RegionID", DbType.Int64, objPatientVO.RegionID);

                    //.............................................................................................................................
                    dbServer.AddInParameter(command, "MobileCountryCode", DbType.String, objPatientVO.MobileCountryCode);
                    dbServer.AddInParameter(command, "ResiNoCountryCode", DbType.Int64, objPatientVO.ResiNoCountryCode);
                    dbServer.AddInParameter(command, "ResiSTDCode", DbType.Int64, objPatientVO.ResiSTDCode);
                    if (objPatientVO.Pincode != null) objPatientVO.Pincode = objPatientVO.Pincode.Trim();
                    dbServer.AddInParameter(command, "Pincode", DbType.String, Security.base64Encode(objPatientVO.Pincode));
                    dbServer.AddInParameter(command, "ReligionID", DbType.Int64, objPatientVO.ReligionID);
                    dbServer.AddInParameter(command, "OccupationId", DbType.Int64, objPatientVO.OccupationId);

                    dbServer.AddInParameter(command, "Photo", DbType.Binary, objPatientVO.Photo); //commented by neena

                    dbServer.AddInParameter(command, "IsLoyaltyMember", DbType.Boolean, objPatientVO.IsLoyaltyMember);
                    dbServer.AddInParameter(command, "LoyaltyCardID", DbType.Int64, objPatientVO.LoyaltyCardID);
                    dbServer.AddInParameter(command, "RelationID", DbType.Int64, objPatientVO.RelationID);
                    dbServer.AddInParameter(command, "ParentPatientID", DbType.Int64, objPatientVO.ParentPatientID);
                    dbServer.AddInParameter(command, "IssueDate", DbType.DateTime, objPatientVO.IssueDate);
                    dbServer.AddInParameter(command, "EffectiveDate", DbType.DateTime, objPatientVO.EffectiveDate);
                    dbServer.AddInParameter(command, "ExpiryDate", DbType.DateTime, objPatientVO.ExpiryDate);
                    if (objPatientVO.LoyaltyCardNo != null) objPatientVO.LoyaltyCardNo = objPatientVO.LoyaltyCardNo.Trim();
                    dbServer.AddInParameter(command, "LoyaltyCardNo", DbType.String, objPatientVO.LoyaltyCardNo);

                    dbServer.AddInParameter(command, "Status", DbType.Boolean, objPatientVO.Status);

                    dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objPatientVO.AddedDateTime);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    //dbServer.AddInParameter(command, "CompanyName", DbType.String, objPatientVO.CompanyName); // Added BY Aadhar  ////IPD

                    //dbServer.AddInParameter(command, "MRNo", DbType.String, objPatientVO.GeneralDetails.MRNo);
                    dbServer.AddParameter(command, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                    //dbServer.AddParameter(command, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "????????????????????");          // Commented on 15062018 For New MRNO format
                    dbServer.AddParameter(command, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????");  // Added on 15062018 For New MRNO format
                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.GeneralDetails.PatientID);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                    //added by neena
                    Random random = new Random();
                    long RandomNumber = random.Next(111111, 666666);
                    dbServer.AddInParameter(command, "RandomNumber", DbType.String, Convert.ToString(RandomNumber));
                    //dbServer.AddParameter(command, "ImagePath", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.ImageName);
                    //

                    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    //int intStatus = dbServer.ExecuteNonQuery(command);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    BizActionObj.PatientDetails.GeneralDetails.PatientID = (long)dbServer.GetParameterValue(command, "ID");
                    BizActionObj.PatientDetails.GeneralDetails.MRNo = (string)dbServer.GetParameterValue(command, "MRNo");
                    //BizActionObj.PatientDetails.ImageName = (string)dbServer.GetParameterValue(command, "ImagePath");  //added by neena                   
                    string Err = (string)dbServer.GetParameterValue(command, "Err");
                    BizActionObj.PatientDetails.GeneralDetails.UnitId = UserVo.UserLoginInfo.UnitId;

                    //added by neena
                    string ImgName = BizActionObj.PatientDetails.GeneralDetails.MRNo + "_" + RandomNumber + ".png";
                    DemoImage ObjDM = new DemoImage();
                    if (objPatientVO.Photo != null)
                        ObjDM.VaryQualityLevel(objPatientVO.Photo, ImgName, ImgSaveLocation);
                    //File.WriteAllBytes(@"D:\Palash\PALASH_PATIENTREGISTRATION\IMAGE\" + ImgName, objPatientVO.Photo);
                    //

                    if (BizActionObj.PatientDetails.SpouseDetails != null && (BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 7 || BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 8 || BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 9))
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPatient");
                        dbServer.AddInParameter(command1, "PatientCategoryID", DbType.Int64, objPatientVO.GeneralDetails.PatientTypeID);
                        // BY BHSUAN . . . .
                        dbServer.AddInParameter(command, "ReferralTypeID", DbType.Int64, objPatientVO.GeneralDetails.ReferralTypeID);
                        //
                        dbServer.AddInParameter(command1, "RegistrationDate", DbType.DateTime, objPatientVO.GeneralDetails.RegistrationDate);

                        if (objPatientVO.SpouseDetails.LastName != null) objPatientVO.SpouseDetails.LastName = objPatientVO.SpouseDetails.LastName.Trim();
                        dbServer.AddInParameter(command1, "LastName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.LastName));
                        if (objPatientVO.SpouseDetails.FirstName != null) objPatientVO.SpouseDetails.FirstName = objPatientVO.SpouseDetails.FirstName.Trim();
                        dbServer.AddInParameter(command1, "FirstName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.FirstName));
                        if (objPatientVO.SpouseDetails.MiddleName != null) objPatientVO.SpouseDetails.MiddleName = objPatientVO.SpouseDetails.MiddleName.Trim();
                        dbServer.AddInParameter(command1, "MiddleName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.MiddleName));
                        if (objPatientVO.SpouseDetails.FamilyName != null) objPatientVO.SpouseDetails.FamilyName = objPatientVO.SpouseDetails.FamilyName.Trim();
                        dbServer.AddInParameter(command1, "FamilyName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.FamilyName));
                        dbServer.AddInParameter(command1, "GenderID", DbType.Int64, objPatientVO.SpouseDetails.GenderID);
                        dbServer.AddInParameter(command1, "DateOfBirth", DbType.DateTime, objPatientVO.SpouseDetails.DateOfBirth);
                        dbServer.AddInParameter(command1, "BloodGroupID", DbType.Int64, objPatientVO.SpouseDetails.BloodGroupID);
                        dbServer.AddInParameter(command1, "MaritalStatusID", DbType.Int64, objPatientVO.SpouseDetails.MaritalStatusID);
                        dbServer.AddInParameter(command1, "CivilID", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.CivilID));
                        if (objPatientVO.SpouseDetails.ContactNo1 != null) objPatientVO.SpouseDetails.ContactNo1 = objPatientVO.SpouseDetails.ContactNo1.Trim();
                        dbServer.AddInParameter(command1, "ContactNo1", DbType.String, objPatientVO.SpouseDetails.ContactNo1);
                        if (objPatientVO.SpouseDetails.ContactNo2 != null) objPatientVO.SpouseDetails.ContactNo2 = objPatientVO.SpouseDetails.ContactNo2.Trim();
                        dbServer.AddInParameter(command1, "ContactNo2", DbType.String, objPatientVO.SpouseDetails.ContactNo2);
                        if (objPatientVO.SpouseDetails.FaxNo != null) objPatientVO.SpouseDetails.FaxNo = objPatientVO.SpouseDetails.FaxNo.Trim();
                        dbServer.AddInParameter(command1, "FaxNo", DbType.String, objPatientVO.SpouseDetails.FaxNo);
                        if (objPatientVO.SpouseDetails.Email != null) objPatientVO.SpouseDetails.Email = objPatientVO.SpouseDetails.Email.Trim();
                        dbServer.AddInParameter(command1, "Email", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Email));
                        if (objPatientVO.SpouseDetails.AddressLine1 != null) objPatientVO.SpouseDetails.AddressLine1 = objPatientVO.SpouseDetails.AddressLine1.Trim();
                        dbServer.AddInParameter(command1, "AddressLine1", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.AddressLine1));
                        if (objPatientVO.SpouseDetails.AddressLine2 != null) objPatientVO.SpouseDetails.AddressLine2 = objPatientVO.SpouseDetails.AddressLine2.Trim();
                        dbServer.AddInParameter(command1, "AddressLine2", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.AddressLine2));
                        if (objPatientVO.SpouseDetails.AddressLine3 != null) objPatientVO.SpouseDetails.AddressLine3 = objPatientVO.SpouseDetails.AddressLine3.Trim();
                        dbServer.AddInParameter(command1, "AddressLine3", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.AddressLine3));
                        if (objPatientVO.SpouseDetails.Country != null) objPatientVO.SpouseDetails.Country = objPatientVO.SpouseDetails.Country.Trim();


                        //added by neena
                        if (objPatientVO.SpouseDetails.SpouseOldRegistrationNo != null) objPatientVO.SpouseDetails.SpouseOldRegistrationNo = objPatientVO.SpouseDetails.SpouseOldRegistrationNo.Trim();
                        dbServer.AddInParameter(command1, "OldRegistrationNo", DbType.String, objPatientVO.SpouseDetails.SpouseOldRegistrationNo);
                        //


                        //By Anjali....................................................................................

                        if (objPatientVO.SpouseDetails.Country != null) objPatientVO.SpouseDetails.Country = objPatientVO.SpouseDetails.Country.Trim();
                        dbServer.AddInParameter(command1, "Country", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Country));
                        if (objPatientVO.SpouseDetails.State != null) objPatientVO.SpouseDetails.State = objPatientVO.SpouseDetails.State.Trim();
                        dbServer.AddInParameter(command1, "State", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.State));
                        if (objPatientVO.SpouseDetails.City != null) objPatientVO.SpouseDetails.City = objPatientVO.SpouseDetails.City.Trim();
                        dbServer.AddInParameter(command1, "City", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.City));
                        if (objPatientVO.SpouseDetails.Taluka != null) objPatientVO.SpouseDetails.Taluka = objPatientVO.SpouseDetails.Taluka.Trim();
                        dbServer.AddInParameter(command1, "Taluka", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Taluka));
                        if (objPatientVO.SpouseDetails.Area != null) objPatientVO.SpouseDetails.Area = objPatientVO.SpouseDetails.Area.Trim();
                        dbServer.AddInParameter(command1, "Area", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Area));
                        if (objPatientVO.SpouseDetails.District != null) objPatientVO.SpouseDetails.District = objPatientVO.SpouseDetails.District.Trim();
                        dbServer.AddInParameter(command1, "District", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.District));

                        //dbServer.AddInParameter(command, "Country", DbType.String, null);
                        //dbServer.AddInParameter(command, "State", DbType.String, null);
                        //dbServer.AddInParameter(command, "City", DbType.String, null);
                        //dbServer.AddInParameter(command, "Taluka", DbType.String, null);
                        //dbServer.AddInParameter(command, "Area", DbType.String, null);
                        //dbServer.AddInParameter(command, "District", DbType.String, null);


                        //if (objPatientVO.SpouseDetails.CountryID > 0)
                        //    dbServer.AddInParameter(command, "CountryID", DbType.Int64, objPatientVO.SpouseDetails.CountryID);
                        //if (objPatientVO.SpouseDetails.StateID > 0)
                        //    dbServer.AddInParameter(command, "StateID", DbType.Int64, objPatientVO.SpouseDetails.StateID);
                        //if (objPatientVO.SpouseDetails.CityID > 0)
                        //    dbServer.AddInParameter(command, "CityID", DbType.Int64, objPatientVO.SpouseDetails.CityID);
                        //if (objPatientVO.SpouseDetails.RegionID > 0)
                        //    dbServer.AddInParameter(command, "RegionID", DbType.Int64, objPatientVO.SpouseDetails.RegionID);

                        //.............................................................................................................................

                        dbServer.AddInParameter(command1, "MobileCountryCode", DbType.String, objPatientVO.SpouseDetails.MobileCountryCode);
                        dbServer.AddInParameter(command1, "ResiNoCountryCode", DbType.Int64, objPatientVO.SpouseDetails.ResiNoCountryCode);
                        dbServer.AddInParameter(command1, "ResiSTDCode", DbType.Int64, objPatientVO.SpouseDetails.ResiSTDCode);
                        if (objPatientVO.SpouseDetails.Pincode != null) objPatientVO.Pincode = objPatientVO.SpouseDetails.Pincode.Trim();
                        dbServer.AddInParameter(command1, "Pincode", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Pincode));

                        // BY BHUSHAN. .
                        dbServer.AddInParameter(command1, "CompanyName", DbType.String, Security.base64Encode(objPatientVO.CompanyName));

                        dbServer.AddInParameter(command1, "ReligionID", DbType.Int64, objPatientVO.SpouseDetails.ReligionID);
                        dbServer.AddInParameter(command1, "OccupationId", DbType.Int64, objPatientVO.SpouseDetails.OccupationId);

                        dbServer.AddInParameter(command1, "Photo", DbType.Binary, objPatientVO.SpouseDetails.Photo); //commented by neena

                        //dbServer.AddInParameter(command1, "IsLoyaltyMember", DbType.Boolean, objPatientVO.IsLoyaltyMember);
                        //dbServer.AddInParameter(command1, "LoyaltyCardID", DbType.Int64, objPatientVO.LoyaltyCardID);
                        //dbServer.AddInParameter(command1, "RelationID", DbType.Int64, objPatientVO.RelationID);
                        //dbServer.AddInParameter(command1, "ParentPatientID", DbType.Int64, objPatientVO.ParentPatientID);
                        //dbServer.AddInParameter(command1, "IssueDate", DbType.DateTime, objPatientVO.IssueDate);
                        //dbServer.AddInParameter(command1, "EffectiveDate", DbType.DateTime, objPatientVO.EffectiveDate);
                        //dbServer.AddInParameter(command1, "ExpiryDate", DbType.DateTime, objPatientVO.ExpiryDate);
                        //if (objPatientVO.LoyaltyCardNo != null) objPatientVO.LoyaltyCardNo = objPatientVO.LoyaltyCardNo.Trim();
                        //dbServer.AddInParameter(command1, "LoyaltyCardNo", DbType.String, objPatientVO.LoyaltyCardNo);

                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, objPatientVO.Status);

                        dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, objPatientVO.AddedDateTime);
                        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        objPatientVO.SpouseDetails.MRNo = objPatientVO.GeneralDetails.MRNo.Remove(objPatientVO.GeneralDetails.MRNo.Length - 1, 1);
                        dbServer.AddParameter(command1, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        dbServer.AddParameter(command1, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.SpouseDetails.MRNo);
                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.SpouseDetails.PatientID);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                        //added by neena
                        Random random1 = new Random();
                        long RandomNumber1 = random1.Next(111111, 666666);
                        dbServer.AddInParameter(command1, "RandomNumber", DbType.String, Convert.ToString(RandomNumber1));
                        //dbServer.AddParameter(command1, "ImagePath", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.SpouseDetails.ImageName);
                        //

                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                        //int intStatus = dbServer.ExecuteNonQuery(command1);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                        BizActionObj.PatientDetails.SpouseDetails.PatientID = (long)dbServer.GetParameterValue(command1, "ID");
                        BizActionObj.PatientDetails.SpouseDetails.MRNo = (string)dbServer.GetParameterValue(command1, "MRNo");
                        // BizActionObj.PatientDetails.SpouseDetails.ImageName = (string)dbServer.GetParameterValue(command1, "ImagePath");  //added by neena                    
                        string Err1 = (string)dbServer.GetParameterValue(command1, "Err");
                        BizActionObj.PatientDetails.SpouseDetails.UnitId = UserVo.UserLoginInfo.UnitId;


                        //added by neena
                        string ImgName1 = BizActionObj.PatientDetails.SpouseDetails.MRNo + "_" + RandomNumber1 + ".png";
                        //DemoImage ObjDM = new DemoImage();
                        if (objPatientVO.SpouseDetails.Photo != null)
                            ObjDM.VaryQualityLevel(objPatientVO.SpouseDetails.Photo, ImgName1, ImgSaveLocation);
                        //File.WriteAllBytes(@"D:\Palash\PALASH_PATIENTREGISTRATION\IMAGE\" + ImgName1, objPatientVO.SpouseDetails.Photo);
                        //

                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddCoupleRegistration");
                        if (BizActionObj.PatientDetails.GenderID == (long)Genders.Male)
                        {
                            dbServer.AddInParameter(command2, "MalePatientID", DbType.Int64, objPatientVO.GeneralDetails.PatientID);
                            dbServer.AddInParameter(command2, "FemalePatientID", DbType.Int64, objPatientVO.SpouseDetails.PatientID);
                        }
                        else
                        {
                            dbServer.AddInParameter(command2, "MalePatientID", DbType.Int64, objPatientVO.SpouseDetails.PatientID);
                            dbServer.AddInParameter(command2, "FemalePatientID", DbType.Int64, objPatientVO.GeneralDetails.PatientID);
                        }

                        dbServer.AddInParameter(command2, "MalePatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "FemalePatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                        dbServer.AddInParameter(command2, "RegistrationDate", DbType.DateTime, objPatientVO.GeneralDetails.RegistrationDate);

                        dbServer.AddInParameter(command2, "Status", DbType.Boolean, objPatientVO.Status);

                        dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);


                        dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, objPatientVO.AddedDateTime);
                        dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                        dbServer.AddParameter(command2, "CoupleRgistrationNumber", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "000000000000000000000000000000000000000000000000000");
                        dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                        dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                        //int intStatus = dbServer.ExecuteNonQuery(command1);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                        // BizActionObj.PatientDetails.SpouseDetails.PatientID = (long)dbServer.GetParameterValue(command1, "ID");
                        //BizActionObj.PatientDetails.SpouseDetails.MRNo = (string)dbServer.GetParameterValue(command1, "MRNo");
                        // string Err1 = (string)dbServer.GetParameterValue(command1, "Err");
                        //BizActionObj.PatientDetails.SpouseDetails.UnitId = UserVo.UserLoginInfo.UnitId;
                    }


                }

                //By Anjali.......... ////IPD

                if (objPatientVO.KinInformationList != null)
                {
                    foreach (clsKinInformationVO item in objPatientVO.KinInformationList)
                    {
                        DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_AddPatientKinInfoIPD");   //CIMS_AddPatientKinInfo
                        dbServer.AddInParameter(command3, "ID", DbType.Int64, 0);
                        dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.PatientID);
                        dbServer.AddInParameter(command3, "MRNo", DbType.String, item.MRCode);
                        dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.UnitId);
                        dbServer.AddInParameter(command3, "sbIsRegisteredPatient", DbType.String, item.IsRegisteredPatient);
                        //dbServer.AddInParameter(command3, "sbPatientID", DbType.String, objPatientVO.PatientID);
                        //dbServer.AddInParameter(command3, "sbPatientUnitID", DbType.String, objPatientVO.PatientUnitID);
                        dbServer.AddInParameter(command3, "sbKinPatientId", DbType.String, item.KinPatientID);
                        dbServer.AddInParameter(command3, "sbKinPatientUnitId", DbType.String, item.KinPatientUnitID);
                        dbServer.AddInParameter(command3, "sbLastName", DbType.String, Security.base64Encode(item.KinLastName));
                        dbServer.AddInParameter(command3, "sbFirstName", DbType.String, Security.base64Encode(item.KinFirstName));
                        dbServer.AddInParameter(command3, "sbMiddleName", DbType.String, Security.base64Encode(item.KinMiddleName));
                        dbServer.AddInParameter(command3, "sbFamilyName", DbType.String, Security.base64Encode(item.FamilyCode));
                        dbServer.AddInParameter(command3, "sbMobileCountryCode", DbType.String, item.MobileCountryCode);
                        dbServer.AddInParameter(command3, "sbMobileNumber", DbType.String, item.MobileCountryCode);
                        dbServer.AddInParameter(command3, "sbAddress", DbType.String, item.Address);
                        dbServer.AddInParameter(command3, "sbRelationshipID", DbType.String, item.RelationshipID);

                        dbServer.AddInParameter(command3, "Status", DbType.String, item.Status);
                        dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, objPatientVO.AddedDateTime);
                        dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                    }
                }

                if (BizActionObj.IsSaveSponsor == true)
                {
                    clsBasePatientSposorDAL objBaseDAL = clsBasePatientSposorDAL.GetInstance();
                    BizActionObj.BizActionVOSaveSponsor.PatientSponsorDetails.PatientId = BizActionObj.PatientDetails.GeneralDetails.PatientID;
                    BizActionObj.BizActionVOSaveSponsor.PatientSponsorDetails.PatientUnitId = BizActionObj.PatientDetails.GeneralDetails.UnitId;

                    BizActionObj.BizActionVOSaveSponsor = (clsAddPatientSponsorBizActionVO)objBaseDAL.AddPatientSponsorDetailsWithTransaction(BizActionObj.BizActionVOSaveSponsor, UserVo, con, trans);
                    if (BizActionObj.BizActionVOSaveSponsor.SuccessStatus == -1) throw new Exception();

                }

                if (BizActionObj.IsSaveAdmission == true)
                {
                    clsBaseIPDAdmissionDAL objBaseDAL = clsBaseIPDAdmissionDAL.GetInstance();
                    BizActionObj.BizActionVOSaveAdmission.Details.PatientId = BizActionObj.PatientDetails.GeneralDetails.PatientID;
                    BizActionObj.BizActionVOSaveAdmission.Details.PatientUnitID = BizActionObj.PatientDetails.GeneralDetails.UnitId;
                    BizActionObj.BizActionVOSaveAdmission.Details.IPDNO = BizActionObj.PatientDetails.GeneralDetails.MRNo;

                    BizActionObj.BizActionVOSaveAdmission = (clsSaveIPDAdmissionBizActionVO)objBaseDAL.AddIPDAdmissionDetailsWithTransaction(BizActionObj.BizActionVOSaveAdmission, UserVo, con, trans);
                    if (BizActionObj.BizActionVOSaveAdmission.SuccessStatus == -1) throw new Exception();
                }

                trans.Commit();
                addPatientUserDetails(BizActionObj, UserVo);


            }
            catch (Exception ex)
            {

                trans.Rollback();
                throw ex;
            }
            finally
            {
                trans.Dispose();
                trans = null;
            }
            return BizActionObj;
        }

        public override IValueObject CheckPatientDuplicacy(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCheckPatientDuplicacyBizActionVO BizActionObj = valueObject as clsCheckPatientDuplicacyBizActionVO;
            try
            {
                clsPatientVO objPatientVO = BizActionObj.PatientDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_PatientDuplicacy");

                DbDataReader reader;

                dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(BizActionObj.PatientDetails.GeneralDetails.FirstName));
                dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(BizActionObj.PatientDetails.GeneralDetails.LastName));
                dbServer.AddInParameter(command, "Gender", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.GenderID);
                dbServer.AddInParameter(command, "DOB", DbType.DateTime, BizActionObj.PatientDetails.GeneralDetails.DateOfBirth);
                dbServer.AddInParameter(command, "MobileCountryCode", DbType.String, BizActionObj.PatientDetails.MobileCountryCode);
                dbServer.AddInParameter(command, "ContactNO1", DbType.String, BizActionObj.PatientDetails.GeneralDetails.ContactNO1);

                dbServer.AddInParameter(command, "SFirstName", DbType.String, Security.base64Encode(BizActionObj.PatientDetails.SpouseDetails.FirstName));
                dbServer.AddInParameter(command, "SLastName", DbType.String, Security.base64Encode(BizActionObj.PatientDetails.SpouseDetails.LastName));
                dbServer.AddInParameter(command, "SGender", DbType.Int64, BizActionObj.PatientDetails.SpouseDetails.GenderID);
                dbServer.AddInParameter(command, "SDOB", DbType.DateTime, BizActionObj.PatientDetails.SpouseDetails.DateOfBirth);
                dbServer.AddInParameter(command, "SMobileCountryCode", DbType.String, BizActionObj.PatientDetails.SpouseDetails.MobileCountryCode);
                dbServer.AddInParameter(command, "SContactNO1", DbType.String, BizActionObj.PatientDetails.SpouseDetails.ContactNO1);

                dbServer.AddInParameter(command, "PatientEditMode", DbType.Boolean, BizActionObj.PatientEditMode);


                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.UnitId);

                //* Added Date - 4/10/2016
                if (BizActionObj.PatientDetails.GeneralDetails.PanNumber != null)
                {
                    dbServer.AddInParameter(command, "PanNumber", DbType.String, BizActionObj.PatientDetails.GeneralDetails.PanNumber);
                }

                if (BizActionObj.PatientDetails.SpouseDetails.SpousePanNumber != null)
                    dbServer.AddInParameter(command, "SpousePanNumber", DbType.String, BizActionObj.PatientDetails.SpouseDetails.SpousePanNumber);
                //***//----

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                if (reader.HasRows)
                {
                    //if (BizActionObj.PatientList == null)
                    //    BizActionObj.PatientList = new List<clsPatientGeneralVO>();
                    //while (reader.Read())
                    //{
                    //    clsPatientGeneralVO Obj = new clsPatientGeneralVO();
                    //    Obj.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                    //    Obj.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                    //    Obj.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                    //    Obj.DateOfBirth = (DateTime?)DALHelper.HandleDate(reader["DateofBirth"]);
                    //    Obj.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                    //    Obj.RegistrationDate = (DateTime?)DALHelper.HandleDate(reader["RegistrationDate"]);
                    //    Obj.PatientID = (long)DALHelper.HandleDBNull(reader["ID"]);
                    //    Obj.UnitId = (long)DALHelper.HandleDBNull(reader["UnitId"]);
                    //    Obj.Gender = (string)DALHelper.HandleDBNull(reader["Gender"]);

                    //    BizActionObj.ResultStatus = true;

                    //    BizActionObj.PatientList.Add(Obj);

                    //}

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

        #endregion

        #region For Pathology Additions

        public override IValueObject GetPatientLabUploadReportData(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientLabUploadReportDataBizActionVO BizActionObj = valueObject as clsGetPatientLabUploadReportDataBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_GetPatientLabUploadReportData");
                dbServer.AddInParameter(command1, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command1, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command1, "IsPathology", DbType.Boolean, BizActionObj.IsPathology);
                reader = (DbDataReader)dbServer.ExecuteReader(command1);
                while (reader.Read())
                {
                    BizActionObj.SourceURL = Convert.ToString(DALHelper.HandleDBNull(reader["SourceURL"]));
                    BizActionObj.Report = (byte[])DALHelper.HandleDBNull(reader["Report"]);
                }
                reader.Close();
            }
            catch (Exception)
            {
                reader.Close();
                throw;
            }
            return BizActionObj;
        }

        #endregion


        //For Surrogate
        public override IValueObject AddSurrogate(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientBizActionVO BizActionObj = valueObject as clsAddPatientBizActionVO;        //

            if (BizActionObj.PatientDetails.GeneralDetails.PatientID == 0)
                BizActionObj = AddSurrogateDetails(BizActionObj, UserVo);
            else
                BizActionObj = UpdateSurrogateDetails(BizActionObj, UserVo);

            return valueObject;

        }

        private clsAddPatientBizActionVO AddSurrogateDetails(clsAddPatientBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = dbServer.CreateConnection();

            try
            {
                clsPatientVO objPatientVO = BizActionObj.PatientDetails;

                con.Open();
                trans = con.BeginTransaction();

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPatient");

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objPatientVO.GeneralDetails.LinkServer);
                if (objPatientVO.GeneralDetails.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objPatientVO.GeneralDetails.LinkServer.Replace(@"\", "_"));
                dbServer.AddInParameter(command, "PatientCategoryID", DbType.Int64, objPatientVO.GeneralDetails.PatientTypeID);
                dbServer.AddInParameter(command, "ReferralTypeID", DbType.Int64, objPatientVO.GeneralDetails.ReferralTypeID);
                dbServer.AddInParameter(command, "CompanyName", DbType.String, Security.base64Encode(objPatientVO.CompanyName));
                dbServer.AddInParameter(command, "RegistrationDate", DbType.DateTime, objPatientVO.GeneralDetails.RegistrationDate);
                if (objPatientVO.GeneralDetails.LastName != null) objPatientVO.GeneralDetails.LastName = objPatientVO.GeneralDetails.LastName.Trim();
                dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.LastName));
                if (objPatientVO.GeneralDetails.FirstName != null) objPatientVO.GeneralDetails.FirstName = objPatientVO.GeneralDetails.FirstName.Trim();
                dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.FirstName));
                if (objPatientVO.GeneralDetails.MiddleName != null) objPatientVO.GeneralDetails.MiddleName = objPatientVO.GeneralDetails.MiddleName.Trim();
                dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.MiddleName));
                if (objPatientVO.FamilyName != null) objPatientVO.FamilyName = objPatientVO.FamilyName.Trim();
                dbServer.AddInParameter(command, "FamilyName", DbType.String, Security.base64Encode(objPatientVO.FamilyName));
                dbServer.AddInParameter(command, "GenderID", DbType.Int64, objPatientVO.GenderID);
                dbServer.AddInParameter(command, "DateOfBirth", DbType.DateTime, objPatientVO.GeneralDetails.DateOfBirth);
                dbServer.AddInParameter(command, "BloodGroupID", DbType.Int64, objPatientVO.BloodGroupID);
                dbServer.AddInParameter(command, "MaritalStatusID", DbType.Int64, objPatientVO.MaritalStatusID);
                dbServer.AddInParameter(command, "CivilID", DbType.String, Security.base64Encode(objPatientVO.CivilID));
                if (objPatientVO.ContactNo1 != null) objPatientVO.ContactNo1 = objPatientVO.ContactNo1.Trim();
                dbServer.AddInParameter(command, "ContactNo1", DbType.String, objPatientVO.ContactNo1);
                if (objPatientVO.ContactNo2 != null) objPatientVO.ContactNo2 = objPatientVO.ContactNo2.Trim();
                dbServer.AddInParameter(command, "ContactNo2", DbType.String, objPatientVO.ContactNo2);
                if (objPatientVO.FaxNo != null) objPatientVO.FaxNo = objPatientVO.FaxNo.Trim();
                dbServer.AddInParameter(command, "FaxNo", DbType.String, objPatientVO.FaxNo);
                if (objPatientVO.Email != null) objPatientVO.Email = objPatientVO.Email.Trim();
                dbServer.AddInParameter(command, "Email", DbType.String, Security.base64Encode(objPatientVO.Email));
                if (objPatientVO.AddressLine1 != null) objPatientVO.AddressLine1 = objPatientVO.AddressLine1.Trim();
                dbServer.AddInParameter(command, "AddressLine1", DbType.String, Security.base64Encode(objPatientVO.AddressLine1));
                if (objPatientVO.AddressLine2 != null) objPatientVO.AddressLine2 = objPatientVO.AddressLine2.Trim();
                dbServer.AddInParameter(command, "AddressLine2", DbType.String, Security.base64Encode(objPatientVO.AddressLine2));
                if (objPatientVO.AddressLine3 != null) objPatientVO.AddressLine3 = objPatientVO.AddressLine3.Trim();
                dbServer.AddInParameter(command, "AddressLine3", DbType.String, Security.base64Encode(objPatientVO.AddressLine3));
                if (objPatientVO.Country != null) objPatientVO.Country = objPatientVO.Country.Trim();
                dbServer.AddInParameter(command, "Country", DbType.String, null);
                dbServer.AddInParameter(command, "State", DbType.String, null);
                dbServer.AddInParameter(command, "City", DbType.String, null);
                dbServer.AddInParameter(command, "Taluka", DbType.String, null);
                dbServer.AddInParameter(command, "Area", DbType.String, null);
                dbServer.AddInParameter(command, "District", DbType.String, null);
                if (objPatientVO.CountryID > 0)
                    dbServer.AddInParameter(command, "CountryID", DbType.Int64, objPatientVO.CountryID);
                if (objPatientVO.StateID > 0)
                    dbServer.AddInParameter(command, "StateID", DbType.Int64, objPatientVO.StateID);
                if (objPatientVO.CityID > 0)
                    dbServer.AddInParameter(command, "CityID", DbType.Int64, objPatientVO.CityID);
                if (objPatientVO.RegionID > 0)
                    dbServer.AddInParameter(command, "RegionID", DbType.Int64, objPatientVO.RegionID);
                dbServer.AddInParameter(command, "MobileCountryCode", DbType.String, objPatientVO.MobileCountryCode);
                dbServer.AddInParameter(command, "ResiNoCountryCode", DbType.Int64, objPatientVO.ResiNoCountryCode);
                dbServer.AddInParameter(command, "ResiSTDCode", DbType.Int64, objPatientVO.ResiSTDCode);
                if (objPatientVO.Pincode != null) objPatientVO.Pincode = objPatientVO.Pincode.Trim();
                dbServer.AddInParameter(command, "Pincode", DbType.String, Security.base64Encode(objPatientVO.Pincode));
                dbServer.AddInParameter(command, "ReligionID", DbType.Int64, objPatientVO.ReligionID);
                dbServer.AddInParameter(command, "OccupationId", DbType.Int64, objPatientVO.OccupationId);

                //added by neena
                if (objPatientVO.OldRegistrationNo != null) objPatientVO.OldRegistrationNo = objPatientVO.OldRegistrationNo.Trim();
                dbServer.AddInParameter(command, "OldRegistrationNo", DbType.String, objPatientVO.OldRegistrationNo);

                //


                dbServer.AddInParameter(command, "Photo", DbType.Binary, objPatientVO.Photo);  //commented by neena

                dbServer.AddInParameter(command, "IsLoyaltyMember", DbType.Boolean, objPatientVO.IsLoyaltyMember);
                dbServer.AddInParameter(command, "LoyaltyCardID", DbType.Int64, objPatientVO.LoyaltyCardID);
                dbServer.AddInParameter(command, "RelationID", DbType.Int64, objPatientVO.RelationID);
                dbServer.AddInParameter(command, "ParentPatientID", DbType.Int64, objPatientVO.ParentPatientID);
                dbServer.AddInParameter(command, "IssueDate", DbType.DateTime, objPatientVO.IssueDate);
                dbServer.AddInParameter(command, "EffectiveDate", DbType.DateTime, objPatientVO.EffectiveDate);
                dbServer.AddInParameter(command, "ExpiryDate", DbType.DateTime, objPatientVO.ExpiryDate);
                if (objPatientVO.LoyaltyCardNo != null) objPatientVO.LoyaltyCardNo = objPatientVO.LoyaltyCardNo.Trim();
                dbServer.AddInParameter(command, "LoyaltyCardNo", DbType.String, objPatientVO.LoyaltyCardNo);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objPatientVO.Status);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                //dbServer.AddParameter(command, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "????????????????????");          // Commented on 15062018 For New MRNO format
                dbServer.AddParameter(command, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????");  // Added on 15062018 For New MRNO format
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.GeneralDetails.PatientID);    
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                //added by neena
                //Random random = new Random();
                //long RandomNumber = random.Next(111111, 666666);
                //dbServer.AddInParameter(command, "RandomNumber", DbType.String, Convert.ToString(RandomNumber));
                dbServer.AddParameter(command, "ImagePath", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                //dbServer.AddParameter(command, "ImagePath", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.ImageName);
                //

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.PatientDetails.GeneralDetails.PatientID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.PatientDetails.GeneralDetails.MRNo = (string)dbServer.GetParameterValue(command, "MRNo");
                BizActionObj.PatientDetails.ImageName = Convert.ToString(dbServer.GetParameterValue(command, "ImagePath"));  //added by neena
                string Err = (string)dbServer.GetParameterValue(command, "Err");
                BizActionObj.PatientDetails.GeneralDetails.UnitId = UserVo.UserLoginInfo.UnitId;

                //added by neena
                //string ImgName = BizActionObj.PatientDetails.GeneralDetails.MRNo + "_" + RandomNumber + ".png";
                DemoImage ObjDM = new DemoImage();
                if (objPatientVO.Photo != null)
                    ObjDM.VaryQualityLevel(objPatientVO.Photo, BizActionObj.PatientDetails.ImageName, ImgSaveLocation);
                //File.WriteAllBytes(@"D:\Palash\PALASH_PATIENTREGISTRATION\IMAGE\" + ImgName, objPatientVO.Photo);
                //


                if (BizActionObj.PatientDetails.SpouseDetails != null && BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 10)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddSurrogateSpouseInformation");

                    dbServer.AddInParameter(command1, "SurrogateID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.PatientID);
                    dbServer.AddInParameter(command1, "SurrogateUnitID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.UnitId);
                    dbServer.AddInParameter(command1, "PatientCategoryID", DbType.Int64, objPatientVO.GeneralDetails.PatientTypeID);
                    dbServer.AddInParameter(command1, "ReferralTypeID", DbType.Int64, objPatientVO.GeneralDetails.ReferralTypeID);
                    dbServer.AddInParameter(command1, "RegistrationDate", DbType.DateTime, objPatientVO.GeneralDetails.RegistrationDate);
                    if (objPatientVO.SpouseDetails.LastName != null) objPatientVO.SpouseDetails.LastName = objPatientVO.SpouseDetails.LastName.Trim();
                    dbServer.AddInParameter(command1, "LastName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.LastName));
                    if (objPatientVO.SpouseDetails.FirstName != null) objPatientVO.SpouseDetails.FirstName = objPatientVO.SpouseDetails.FirstName.Trim();
                    dbServer.AddInParameter(command1, "FirstName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.FirstName));
                    if (objPatientVO.SpouseDetails.MiddleName != null) objPatientVO.SpouseDetails.MiddleName = objPatientVO.SpouseDetails.MiddleName.Trim();
                    dbServer.AddInParameter(command1, "MiddleName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.MiddleName));
                    if (objPatientVO.SpouseDetails.FamilyName != null) objPatientVO.SpouseDetails.FamilyName = objPatientVO.SpouseDetails.FamilyName.Trim();
                    dbServer.AddInParameter(command1, "FamilyName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.FamilyName));
                    dbServer.AddInParameter(command1, "GenderID", DbType.Int64, objPatientVO.SpouseDetails.GenderID);
                    dbServer.AddInParameter(command1, "DateOfBirth", DbType.DateTime, objPatientVO.SpouseDetails.DateOfBirth);
                    dbServer.AddInParameter(command1, "BloodGroupID", DbType.Int64, objPatientVO.SpouseDetails.BloodGroupID);
                    dbServer.AddInParameter(command1, "MaritalStatusID", DbType.Int64, objPatientVO.SpouseDetails.MaritalStatusID);
                    dbServer.AddInParameter(command1, "CivilID", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.CivilID));
                    if (objPatientVO.SpouseDetails.ContactNo1 != null) objPatientVO.SpouseDetails.ContactNo1 = objPatientVO.SpouseDetails.ContactNo1.Trim();
                    dbServer.AddInParameter(command1, "ContactNo1", DbType.String, objPatientVO.SpouseDetails.ContactNo1);
                    if (objPatientVO.SpouseDetails.ContactNo2 != null) objPatientVO.SpouseDetails.ContactNo2 = objPatientVO.SpouseDetails.ContactNo2.Trim();
                    dbServer.AddInParameter(command1, "ContactNo2", DbType.String, objPatientVO.SpouseDetails.ContactNo2);
                    if (objPatientVO.SpouseDetails.FaxNo != null) objPatientVO.SpouseDetails.FaxNo = objPatientVO.SpouseDetails.FaxNo.Trim();
                    dbServer.AddInParameter(command1, "FaxNo", DbType.String, objPatientVO.SpouseDetails.FaxNo);
                    if (objPatientVO.SpouseDetails.Email != null) objPatientVO.SpouseDetails.Email = objPatientVO.SpouseDetails.Email.Trim();
                    dbServer.AddInParameter(command1, "Email", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Email));
                    if (objPatientVO.SpouseDetails.AddressLine1 != null) objPatientVO.SpouseDetails.AddressLine1 = objPatientVO.SpouseDetails.AddressLine1.Trim();
                    dbServer.AddInParameter(command1, "AddressLine1", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.AddressLine1));
                    if (objPatientVO.SpouseDetails.AddressLine2 != null) objPatientVO.SpouseDetails.AddressLine2 = objPatientVO.SpouseDetails.AddressLine2.Trim();
                    dbServer.AddInParameter(command1, "AddressLine2", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.AddressLine2));
                    if (objPatientVO.SpouseDetails.AddressLine3 != null) objPatientVO.SpouseDetails.AddressLine3 = objPatientVO.SpouseDetails.AddressLine3.Trim();
                    dbServer.AddInParameter(command1, "AddressLine3", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.AddressLine3));
                    if (objPatientVO.SpouseDetails.Country != null) objPatientVO.SpouseDetails.Country = objPatientVO.SpouseDetails.Country.Trim();
                    dbServer.AddInParameter(command1, "Country", DbType.String, null);
                    dbServer.AddInParameter(command1, "State", DbType.String, null);
                    dbServer.AddInParameter(command1, "City", DbType.String, null);
                    dbServer.AddInParameter(command1, "Taluka", DbType.String, null);
                    dbServer.AddInParameter(command1, "Area", DbType.String, null);
                    dbServer.AddInParameter(command1, "District", DbType.String, null);
                    if (objPatientVO.SpouseDetails.CountryID > 0)
                        dbServer.AddInParameter(command1, "CountryID", DbType.Int64, objPatientVO.SpouseDetails.CountryID);
                    if (objPatientVO.SpouseDetails.StateID > 0)
                        dbServer.AddInParameter(command1, "StateID", DbType.Int64, objPatientVO.SpouseDetails.StateID);
                    if (objPatientVO.SpouseDetails.CityID > 0)
                        dbServer.AddInParameter(command1, "CityID", DbType.Int64, objPatientVO.SpouseDetails.CityID);
                    if (objPatientVO.SpouseDetails.RegionID > 0)
                        dbServer.AddInParameter(command1, "RegionID", DbType.Int64, objPatientVO.SpouseDetails.RegionID);
                    dbServer.AddInParameter(command1, "MobileCountryCode", DbType.String, objPatientVO.SpouseDetails.MobileCountryCode);
                    dbServer.AddInParameter(command1, "ResiNoCountryCode", DbType.Int64, objPatientVO.SpouseDetails.ResiNoCountryCode);
                    dbServer.AddInParameter(command1, "ResiSTDCode", DbType.Int64, objPatientVO.SpouseDetails.ResiSTDCode);
                    if (objPatientVO.SpouseDetails.Pincode != null) objPatientVO.Pincode = objPatientVO.SpouseDetails.Pincode.Trim();
                    dbServer.AddInParameter(command1, "Pincode", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Pincode));
                    dbServer.AddInParameter(command1, "CompanyName", DbType.String, Security.base64Encode(objPatientVO.CompanyName));
                    dbServer.AddInParameter(command1, "ReligionID", DbType.Int64, objPatientVO.SpouseDetails.ReligionID);
                    dbServer.AddInParameter(command1, "OccupationId", DbType.Int64, objPatientVO.SpouseDetails.OccupationId);
                    dbServer.AddInParameter(command1, "Photo", DbType.Binary, objPatientVO.SpouseDetails.Photo);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, objPatientVO.Status);
                    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    objPatientVO.SpouseDetails.MRNo = objPatientVO.GeneralDetails.MRNo.Remove(objPatientVO.GeneralDetails.MRNo.Length - 1, 1);
                    dbServer.AddParameter(command1, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                    dbServer.AddInParameter(command1, "ID", DbType.Int64, objPatientVO.SpouseDetails.PatientID);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                    string Err1 = (string)dbServer.GetParameterValue(command1, "Err");
                }
                DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddSurrogateOtherInformation");
                dbServer.AddInParameter(command2, "SurrogateID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.PatientID);
                dbServer.AddInParameter(command2, "SurrogateUnitID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.UnitId);
                dbServer.AddInParameter(command2, "ReferralTypeID", DbType.Int64, objPatientVO.GeneralDetails.ReferralTypeID);
                //dbServer.AddInParameter(command2, "SourceName", DbType.String, objPatientVO.SourceName);
                //dbServer.AddInParameter(command2, "SourceEmail", DbType.String, objPatientVO.SourceEmail);
                //dbServer.AddInParameter(command2, "SourceContactNo", DbType.String, objPatientVO.SourceContactNo);
                //dbServer.AddInParameter(command2, "SourceAddress", DbType.String, objPatientVO.SourceAddress);
                dbServer.AddInParameter(command2, "AgencyID", DbType.String, objPatientVO.AgencyID);
                dbServer.AddInParameter(command2, "SurrogateOtherDetails", DbType.String, objPatientVO.SurrogateOtherDetails);
                dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command2, "ID", DbType.Int64, 0);
                dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");

                DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_AddPatientSponsor");

                dbServer.AddInParameter(command3, "LinkServer", DbType.String, null);
                dbServer.AddInParameter(command3, "LinkServerAlias", DbType.String, null);
                dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.PatientID);
                dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.UnitId);
                dbServer.AddInParameter(command3, "PatientSourceID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.PatientSourceID);
                dbServer.AddInParameter(command3, "CompanyID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.CompanyID);
                dbServer.AddInParameter(command3, "AssociatedCompanyID", DbType.Int64, 0);
                dbServer.AddInParameter(command3, "ReferenceNo", DbType.String, null);
                dbServer.AddInParameter(command3, "CreditLimit", DbType.Double, 0);
                dbServer.AddInParameter(command3, "EffectiveDate", DbType.DateTime, objPatientVO.EffectiveDate);
                dbServer.AddInParameter(command3, "ExpiryDate", DbType.DateTime, objPatientVO.ExpiryDate);
                dbServer.AddInParameter(command3, "TariffID", DbType.Int64, objPatientVO.TariffID);
                dbServer.AddInParameter(command3, "EmployeeNo", DbType.String, null);
                dbServer.AddInParameter(command3, "DesignationID", DbType.Int64, null);
                dbServer.AddInParameter(command3, "Remark", DbType.String, null);
                dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command3, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command3, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                trans.Commit();
            }
            catch (Exception ex)
            {

                trans.Rollback();
                throw ex;
            }
            finally
            {
                trans.Dispose();
                trans = null;
            }
            return BizActionObj;
        }
        private clsAddPatientBizActionVO UpdateSurrogateDetails(clsAddPatientBizActionVO BizActionObj, clsUserVO UserVo)
        {

            DbTransaction trans = null;
            DbConnection con = dbServer.CreateConnection();

            try
            {
                clsPatientVO objPatientVO = BizActionObj.PatientDetails;

                con.Open();
                trans = con.BeginTransaction();


                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdatePatient");

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objPatientVO.GeneralDetails.LinkServer);
                if (objPatientVO.GeneralDetails.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objPatientVO.GeneralDetails.LinkServer.Replace(@"\", "_"));
                dbServer.AddInParameter(command, "PatientCategoryID", DbType.Int64, objPatientVO.GeneralDetails.PatientTypeID);
                dbServer.AddInParameter(command, "ReferralTypeID", DbType.Int64, objPatientVO.GeneralDetails.ReferralTypeID);
                dbServer.AddInParameter(command, "CompanyName", DbType.String, Security.base64Encode(objPatientVO.CompanyName));
                dbServer.AddInParameter(command, "MRNo", DbType.String, objPatientVO.GeneralDetails.MRNo);
                dbServer.AddInParameter(command, "RegistrationDate", DbType.DateTime, objPatientVO.GeneralDetails.RegistrationDate);
                if (objPatientVO.GeneralDetails.LastName != null) objPatientVO.GeneralDetails.LastName = objPatientVO.GeneralDetails.LastName.Trim();
                dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.LastName));
                if (objPatientVO.GeneralDetails.FirstName != null) objPatientVO.GeneralDetails.FirstName = objPatientVO.GeneralDetails.FirstName.Trim();
                dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.FirstName));
                if (objPatientVO.GeneralDetails.MiddleName != null) objPatientVO.GeneralDetails.MiddleName = objPatientVO.GeneralDetails.MiddleName.Trim();
                dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.MiddleName));
                if (objPatientVO.FamilyName != null) objPatientVO.FamilyName = objPatientVO.FamilyName.Trim();
                dbServer.AddInParameter(command, "FamilyName", DbType.String, Security.base64Encode(objPatientVO.FamilyName));
                dbServer.AddInParameter(command, "GenderID", DbType.Int64, objPatientVO.GenderID);
                dbServer.AddInParameter(command, "DateOfBirth", DbType.DateTime, objPatientVO.GeneralDetails.DateOfBirth);
                dbServer.AddInParameter(command, "BloodGroupID", DbType.Int64, objPatientVO.BloodGroupID);
                dbServer.AddInParameter(command, "MaritalStatusID", DbType.Int64, objPatientVO.MaritalStatusID);
                dbServer.AddInParameter(command, "CivilID", DbType.String, Security.base64Encode(objPatientVO.CivilID));
                if (objPatientVO.ContactNo1 != null) objPatientVO.ContactNo1 = objPatientVO.ContactNo1.Trim();
                dbServer.AddInParameter(command, "ContactNo1", DbType.String, objPatientVO.ContactNo1);
                if (objPatientVO.ContactNo2 != null) objPatientVO.ContactNo2 = objPatientVO.ContactNo2.Trim();
                dbServer.AddInParameter(command, "ContactNo2", DbType.String, objPatientVO.ContactNo2);
                if (objPatientVO.FaxNo != null) objPatientVO.FaxNo = objPatientVO.FaxNo.Trim();
                dbServer.AddInParameter(command, "FaxNo", DbType.String, objPatientVO.FaxNo);
                if (objPatientVO.Email != null) objPatientVO.Email = objPatientVO.Email.Trim();
                dbServer.AddInParameter(command, "Email", DbType.String, Security.base64Encode(objPatientVO.Email));
                if (objPatientVO.AddressLine1 != null) objPatientVO.AddressLine1 = objPatientVO.AddressLine1.Trim();
                dbServer.AddInParameter(command, "AddressLine1", DbType.String, Security.base64Encode(objPatientVO.AddressLine1));
                if (objPatientVO.AddressLine2 != null) objPatientVO.AddressLine2 = objPatientVO.AddressLine2.Trim();
                dbServer.AddInParameter(command, "AddressLine2", DbType.String, Security.base64Encode(objPatientVO.AddressLine2));
                if (objPatientVO.AddressLine3 != null) objPatientVO.AddressLine3 = objPatientVO.AddressLine3.Trim();
                dbServer.AddInParameter(command, "AddressLine3", DbType.String, Security.base64Encode(objPatientVO.AddressLine3));
                dbServer.AddInParameter(command, "Country", DbType.String, null);
                dbServer.AddInParameter(command, "State", DbType.String, null);
                dbServer.AddInParameter(command, "City", DbType.String, null);
                dbServer.AddInParameter(command, "Taluka", DbType.String, null);
                dbServer.AddInParameter(command, "Area", DbType.String, null);
                dbServer.AddInParameter(command, "District", DbType.String, null);
                if (objPatientVO.CountryID > 0)
                    dbServer.AddInParameter(command, "CountryID", DbType.Int64, objPatientVO.CountryID);
                if (objPatientVO.StateID > 0)
                    dbServer.AddInParameter(command, "StateID", DbType.Int64, objPatientVO.StateID);
                if (objPatientVO.CityID > 0)
                    dbServer.AddInParameter(command, "CityID", DbType.Int64, objPatientVO.CityID);
                if (objPatientVO.RegionID > 0)
                    dbServer.AddInParameter(command, "RegionID", DbType.Int64, objPatientVO.RegionID);
                dbServer.AddInParameter(command, "MobileCountryCode", DbType.String, objPatientVO.MobileCountryCode);
                dbServer.AddInParameter(command, "ResiNoCountryCode", DbType.Int64, objPatientVO.ResiNoCountryCode);
                dbServer.AddInParameter(command, "ResiSTDCode", DbType.Int64, objPatientVO.ResiSTDCode);
                if (objPatientVO.Pincode != null) objPatientVO.Pincode = objPatientVO.Pincode.Trim();
                dbServer.AddInParameter(command, "Pincode", DbType.String, Security.base64Encode(objPatientVO.Pincode));
                dbServer.AddInParameter(command, "ReligionID", DbType.Int64, objPatientVO.ReligionID);
                dbServer.AddInParameter(command, "OccupationId", DbType.Int64, objPatientVO.OccupationId);
                dbServer.AddInParameter(command, "AgentID", DbType.Int64, objPatientVO.AgentID);
                dbServer.AddInParameter(command, "AgencyID", DbType.Int64, objPatientVO.AgencyID);

                //added by neena
                if (objPatientVO.OldRegistrationNo != null) objPatientVO.OldRegistrationNo = objPatientVO.OldRegistrationNo.Trim();
                dbServer.AddInParameter(command, "OldRegistrationNo", DbType.String, objPatientVO.OldRegistrationNo);
                //

                dbServer.AddInParameter(command, "Photo", DbType.Binary, objPatientVO.Photo);  //commented by neena

                dbServer.AddInParameter(command, "IsLoyaltyMember", DbType.Boolean, objPatientVO.IsLoyaltyMember);
                dbServer.AddInParameter(command, "LoyaltyCardID", DbType.Int64, objPatientVO.LoyaltyCardID);
                dbServer.AddInParameter(command, "IssueDate", DbType.DateTime, objPatientVO.IssueDate);
                dbServer.AddInParameter(command, "EffectiveDate", DbType.DateTime, objPatientVO.EffectiveDate);
                dbServer.AddInParameter(command, "ExpiryDate", DbType.DateTime, objPatientVO.ExpiryDate);
                if (objPatientVO.LoyaltyCardNo != null) objPatientVO.LoyaltyCardNo = objPatientVO.LoyaltyCardNo.Trim();
                dbServer.AddInParameter(command, "LoyaltyCardNo", DbType.String, objPatientVO.LoyaltyCardNo);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objPatientVO.Status);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objPatientVO.GeneralDetails.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objPatientVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "ID", DbType.Int64, objPatientVO.GeneralDetails.PatientID);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.SuccessStatus);

                //added by neena
                //dbServer.AddParameter(command, "ImagePath", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.ImageName);
                dbServer.AddParameter(command, "ImagePath", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "000000000000000000000000000000000000000000000000000");
                //


                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                BizActionObj.PatientDetails.GeneralDetails.UnitId = UserVo.UserLoginInfo.UnitId;

                //added by neena
                BizActionObj.PatientDetails.ImageName = (string)dbServer.GetParameterValue(command, "ImagePath");
                DemoImage ObjDM = new DemoImage();
                if (objPatientVO.Photo != null)
                    ObjDM.VaryQualityLevel(objPatientVO.Photo, BizActionObj.PatientDetails.ImageName, ImgSaveLocation);
                //else
                //    File.WriteAllBytes(@"D:\Palash\PALASH_PATIENTREGISTRATION\IMAGE\" + BizActionObj.PatientDetails.ImageName, objPatientVO.Photo);
                //


                if (BizActionObj.PatientDetails.SpouseDetails != null && BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 10)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddSurrogateSpouseInformation");

                    dbServer.AddInParameter(command1, "SurrogateID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.PatientID);
                    dbServer.AddInParameter(command1, "SurrogateUnitID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.UnitId);
                    dbServer.AddInParameter(command1, "PatientCategoryID", DbType.Int64, objPatientVO.GeneralDetails.PatientTypeID);
                    dbServer.AddInParameter(command1, "ReferralTypeID", DbType.Int64, objPatientVO.GeneralDetails.ReferralTypeID);
                    dbServer.AddInParameter(command1, "RegistrationDate", DbType.DateTime, objPatientVO.GeneralDetails.RegistrationDate);
                    if (objPatientVO.SpouseDetails.LastName != null) objPatientVO.SpouseDetails.LastName = objPatientVO.SpouseDetails.LastName.Trim();
                    dbServer.AddInParameter(command1, "LastName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.LastName));
                    if (objPatientVO.SpouseDetails.FirstName != null) objPatientVO.SpouseDetails.FirstName = objPatientVO.SpouseDetails.FirstName.Trim();
                    dbServer.AddInParameter(command1, "FirstName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.FirstName));
                    if (objPatientVO.SpouseDetails.MiddleName != null) objPatientVO.SpouseDetails.MiddleName = objPatientVO.SpouseDetails.MiddleName.Trim();
                    dbServer.AddInParameter(command1, "MiddleName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.MiddleName));
                    if (objPatientVO.SpouseDetails.FamilyName != null) objPatientVO.SpouseDetails.FamilyName = objPatientVO.SpouseDetails.FamilyName.Trim();
                    dbServer.AddInParameter(command1, "FamilyName", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.FamilyName));
                    dbServer.AddInParameter(command1, "GenderID", DbType.Int64, objPatientVO.SpouseDetails.GenderID);
                    dbServer.AddInParameter(command1, "DateOfBirth", DbType.DateTime, objPatientVO.SpouseDetails.DateOfBirth);
                    dbServer.AddInParameter(command1, "BloodGroupID", DbType.Int64, objPatientVO.SpouseDetails.BloodGroupID);
                    dbServer.AddInParameter(command1, "MaritalStatusID", DbType.Int64, objPatientVO.SpouseDetails.MaritalStatusID);
                    dbServer.AddInParameter(command1, "CivilID", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.CivilID));
                    if (objPatientVO.SpouseDetails.ContactNo1 != null) objPatientVO.SpouseDetails.ContactNo1 = objPatientVO.SpouseDetails.ContactNo1.Trim();
                    dbServer.AddInParameter(command1, "ContactNo1", DbType.String, objPatientVO.SpouseDetails.ContactNo1);
                    if (objPatientVO.SpouseDetails.ContactNo2 != null) objPatientVO.SpouseDetails.ContactNo2 = objPatientVO.SpouseDetails.ContactNo2.Trim();
                    dbServer.AddInParameter(command1, "ContactNo2", DbType.String, objPatientVO.SpouseDetails.ContactNo2);
                    if (objPatientVO.SpouseDetails.FaxNo != null) objPatientVO.SpouseDetails.FaxNo = objPatientVO.SpouseDetails.FaxNo.Trim();
                    dbServer.AddInParameter(command1, "FaxNo", DbType.String, objPatientVO.SpouseDetails.FaxNo);
                    if (objPatientVO.SpouseDetails.Email != null) objPatientVO.SpouseDetails.Email = objPatientVO.SpouseDetails.Email.Trim();
                    dbServer.AddInParameter(command1, "Email", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Email));
                    if (objPatientVO.SpouseDetails.AddressLine1 != null) objPatientVO.SpouseDetails.AddressLine1 = objPatientVO.SpouseDetails.AddressLine1.Trim();
                    dbServer.AddInParameter(command1, "AddressLine1", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.AddressLine1));
                    if (objPatientVO.SpouseDetails.AddressLine2 != null) objPatientVO.SpouseDetails.AddressLine2 = objPatientVO.SpouseDetails.AddressLine2.Trim();
                    dbServer.AddInParameter(command1, "AddressLine2", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.AddressLine2));
                    if (objPatientVO.SpouseDetails.AddressLine3 != null) objPatientVO.SpouseDetails.AddressLine3 = objPatientVO.SpouseDetails.AddressLine3.Trim();
                    dbServer.AddInParameter(command1, "AddressLine3", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.AddressLine3));
                    if (objPatientVO.SpouseDetails.Country != null) objPatientVO.SpouseDetails.Country = objPatientVO.SpouseDetails.Country.Trim();
                    dbServer.AddInParameter(command1, "Country", DbType.String, null);
                    dbServer.AddInParameter(command1, "State", DbType.String, null);
                    dbServer.AddInParameter(command1, "City", DbType.String, null);
                    dbServer.AddInParameter(command1, "Taluka", DbType.String, null);
                    dbServer.AddInParameter(command1, "Area", DbType.String, null);
                    dbServer.AddInParameter(command1, "District", DbType.String, null);
                    if (objPatientVO.SpouseDetails.CountryID > 0)
                        dbServer.AddInParameter(command1, "CountryID", DbType.Int64, objPatientVO.SpouseDetails.CountryID);
                    if (objPatientVO.SpouseDetails.StateID > 0)
                        dbServer.AddInParameter(command1, "StateID", DbType.Int64, objPatientVO.SpouseDetails.StateID);
                    if (objPatientVO.SpouseDetails.CityID > 0)
                        dbServer.AddInParameter(command1, "CityID", DbType.Int64, objPatientVO.SpouseDetails.CityID);
                    if (objPatientVO.SpouseDetails.RegionID > 0)
                        dbServer.AddInParameter(command1, "RegionID", DbType.Int64, objPatientVO.SpouseDetails.RegionID);
                    dbServer.AddInParameter(command1, "MobileCountryCode", DbType.String, objPatientVO.SpouseDetails.MobileCountryCode);
                    dbServer.AddInParameter(command1, "ResiNoCountryCode", DbType.Int64, objPatientVO.SpouseDetails.ResiNoCountryCode);
                    dbServer.AddInParameter(command1, "ResiSTDCode", DbType.Int64, objPatientVO.SpouseDetails.ResiSTDCode);
                    if (objPatientVO.SpouseDetails.Pincode != null) objPatientVO.Pincode = objPatientVO.SpouseDetails.Pincode.Trim();
                    dbServer.AddInParameter(command1, "Pincode", DbType.String, Security.base64Encode(objPatientVO.SpouseDetails.Pincode));
                    dbServer.AddInParameter(command1, "CompanyName", DbType.String, Security.base64Encode(objPatientVO.CompanyName));
                    dbServer.AddInParameter(command1, "ReligionID", DbType.Int64, objPatientVO.SpouseDetails.ReligionID);
                    dbServer.AddInParameter(command1, "OccupationId", DbType.Int64, objPatientVO.SpouseDetails.OccupationId);
                    dbServer.AddInParameter(command1, "Photo", DbType.Binary, objPatientVO.SpouseDetails.Photo);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, objPatientVO.Status);
                    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    objPatientVO.SpouseDetails.MRNo = objPatientVO.GeneralDetails.MRNo.Remove(objPatientVO.GeneralDetails.MRNo.Length - 1, 1);
                    dbServer.AddParameter(command1, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                    dbServer.AddInParameter(command1, "ID", DbType.Int64, objPatientVO.SpouseDetails.PatientID);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                    string Err1 = (string)dbServer.GetParameterValue(command1, "Err");
                }
                DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddSurrogateOtherInformation");
                dbServer.AddInParameter(command2, "SurrogateID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.PatientID);
                dbServer.AddInParameter(command2, "SurrogateUnitID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.UnitId);
                dbServer.AddInParameter(command2, "ReferralTypeID", DbType.Int64, objPatientVO.GeneralDetails.ReferralTypeID);
                //dbServer.AddInParameter(command2, "SourceName", DbType.String, objPatientVO.SourceName);
                //dbServer.AddInParameter(command2, "SourceEmail", DbType.String, objPatientVO.SourceEmail);
                //dbServer.AddInParameter(command2, "SourceContactNo", DbType.String, objPatientVO.SourceContactNo);
                //dbServer.AddInParameter(command2, "SourceAddress", DbType.String, objPatientVO.SourceAddress);
                dbServer.AddInParameter(command2, "AgencyID", DbType.String, objPatientVO.AgencyID);
                dbServer.AddInParameter(command2, "SurrogateOtherDetails", DbType.String, objPatientVO.SurrogateOtherDetails);
                dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command2, "ID", DbType.Int64, 0);
                dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                trans.Commit();
            }
            catch (Exception ex)
            {

                trans.Rollback();
                throw ex;
            }
            finally
            {
                trans.Dispose();
                trans = null;
            }
            return BizActionObj;
        }
        public override IValueObject GetSurrogate(IValueObject valueObject, clsUserVO UserVo)
        {
            //  throw new NotImplementedException();
            clsGetPatientBizActionVO BizActionObj = valueObject as clsGetPatientBizActionVO;
            // clsAddPatientBizActionVO BizActionObj = ValueObject as clsAddPatientBizActionVO;
            try
            {
                clsPatientVO objPatientVO = BizActionObj.PatientDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetSurrogate");
                DbDataReader reader;
                dbServer.AddInParameter(command, "LinkServer", DbType.String, objPatientVO.GeneralDetails.LinkServer);
                if (objPatientVO.GeneralDetails.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objPatientVO.GeneralDetails.LinkServer.Replace(@"\", "_"));
                dbServer.AddInParameter(command, "ID", DbType.Int64, objPatientVO.GeneralDetails.PatientID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objPatientVO.GeneralDetails.UnitId);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizActionObj.PatientDetails.GeneralDetails.PatientTypeID = (long)DALHelper.HandleDBNull(reader["PatientCategoryID"]);
                        BizActionObj.PatientDetails.GeneralDetails.ReferralTypeID = (long)DALHelper.HandleDBNull(reader["ReferralTypeID"]);
                        BizActionObj.PatientDetails.CompanyName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["CompanyName"]));
                        BizActionObj.PatientDetails.GeneralDetails.PatientID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        BizActionObj.PatientDetails.GeneralDetails.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                        BizActionObj.PatientDetails.GeneralDetails.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                        BizActionObj.PatientDetails.GeneralDetails.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                        BizActionObj.PatientDetails.GeneralDetails.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                        BizActionObj.PatientDetails.GeneralDetails.RegistrationDate = (DateTime?)DALHelper.HandleDate(reader["RegistrationDate"]);
                        BizActionObj.PatientDetails.GeneralDetails.DateOfBirth = (DateTime?)DALHelper.HandleDate(reader["DateOfBirth"]);
                        BizActionObj.PatientDetails.GeneralDetails.PatientSourceID = (long)DALHelper.HandleDBNull(reader["PatientSourceID"]);
                        BizActionObj.PatientDetails.GeneralDetails.TariffID = (long)DALHelper.HandleDBNull(reader["TariffID"]);
                        BizActionObj.PatientDetails.FamilyName = Security.base64Decode((String)DALHelper.HandleDBNull(reader["FamilyName"]));
                        BizActionObj.PatientDetails.GenderID = (Int64)DALHelper.HandleDBNull(reader["GenderID"]);
                        BizActionObj.PatientDetails.BloodGroupID = (Int64)DALHelper.HandleDBNull(reader["BloodGroupID"]);
                        BizActionObj.PatientDetails.MaritalStatusID = (Int64)DALHelper.HandleDBNull(reader["MaritalStatusID"]);
                        BizActionObj.PatientDetails.CivilID = Security.base64Decode((string)DALHelper.HandleDBNull(reader["CivilID"]));
                        BizActionObj.PatientDetails.ContactNo1 = (string)DALHelper.HandleDBNull(reader["ContactNo1"]);
                        BizActionObj.PatientDetails.ContactNo2 = (string)DALHelper.HandleDBNull(reader["ContactNo2"]);
                        BizActionObj.PatientDetails.FaxNo = (String)DALHelper.HandleDBNull(reader["FaxNo"]);
                        BizActionObj.PatientDetails.Email = Security.base64Decode((String)DALHelper.HandleDBNull(reader["Email"]));
                        BizActionObj.PatientDetails.AddressLine1 = Security.base64Decode((String)DALHelper.HandleDBNull(reader["AddressLine1"]));
                        BizActionObj.PatientDetails.AddressLine2 = Security.base64Decode((String)DALHelper.HandleDBNull(reader["AddressLine2"]));
                        BizActionObj.PatientDetails.AddressLine3 = Security.base64Decode((String)DALHelper.HandleDBNull(reader["AddressLine3"]));
                        BizActionObj.PatientDetails.Country = Security.base64Decode((String)DALHelper.HandleDBNull(reader["Country"]));
                        BizActionObj.PatientDetails.State = Security.base64Decode((String)DALHelper.HandleDBNull(reader["State"]));
                        BizActionObj.PatientDetails.City = Security.base64Decode((String)DALHelper.HandleDBNull(reader["City"]));
                        BizActionObj.PatientDetails.Taluka = Security.base64Decode((string)DALHelper.HandleDBNull(reader["Taluka"]));
                        BizActionObj.PatientDetails.Area = Security.base64Decode((string)DALHelper.HandleDBNull(reader["Area"]));
                        BizActionObj.PatientDetails.District = Security.base64Decode((string)DALHelper.HandleDBNull(reader["District"]));
                        BizActionObj.PatientDetails.MobileCountryCode = Convert.ToString(DALHelper.HandleDBNull(reader["MobileCountryCode"]));
                        BizActionObj.PatientDetails.ResiNoCountryCode = (long)DALHelper.HandleDBNull(reader["ResiNoCountryCode"]);
                        BizActionObj.PatientDetails.ResiSTDCode = (long)DALHelper.HandleDBNull(reader["ResiSTDCode"]);
                        BizActionObj.PatientDetails.CountryID = (long)DALHelper.HandleIntegerNull(reader["CountryID"]);
                        BizActionObj.PatientDetails.StateID = (long)DALHelper.HandleIntegerNull(reader["StateID"]);
                        BizActionObj.PatientDetails.CityID = (long)DALHelper.HandleIntegerNull(reader["CityID"]);
                        BizActionObj.PatientDetails.RegionID = (long)DALHelper.HandleIntegerNull(reader["RegionID"]);
                        BizActionObj.PatientDetails.Pincode = Security.base64Decode((String)DALHelper.HandleDBNull(reader["Pincode"]));
                        BizActionObj.PatientDetails.ReligionID = (Int64)DALHelper.HandleDBNull(reader["ReligionID"]);
                        BizActionObj.PatientDetails.OccupationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["OccupationId"]));

                        //added by neena
                        //BizActionObj.PatientDetails.ImageName = Convert.ToString(DALHelper.HandleDBNull(reader["ImgPath"]));  
                        string ImgPath = Convert.ToString(DALHelper.HandleDBNull(reader["ImgPath"]));

                        BizActionObj.PatientDetails.ImageName = "https://" + ImgIP + "/" + ImgVirtualDir + "/" + ImgPath;

                        //

                        if ((Byte[])DALHelper.HandleDBNull(reader["Photo"]) != null)
                        {
                            BizActionObj.PatientDetails.Photo = (Byte[])DALHelper.HandleDBNull(reader["Photo"]);
                        }
                        if (BizActionObj.PatientDetails.GeneralDetails.Photo != null)
                        {
                            BizActionObj.PatientDetails.GeneralDetails.Photo = (Byte[])DALHelper.HandleDBNull(reader["Photo"]);
                        }
                        BizActionObj.PatientDetails.IsLoyaltyMember = (bool)DALHelper.HandleDBNull(reader["IsLoyaltyMember"]);
                        BizActionObj.PatientDetails.LoyaltyCardID = (long?)DALHelper.HandleDBNull(reader["LoyaltyCardID"]);
                        BizActionObj.PatientDetails.IssueDate = (DateTime?)DALHelper.HandleDate(reader["IssueDate"]);
                        BizActionObj.PatientDetails.EffectiveDate = (DateTime?)DALHelper.HandleDate(reader["EffectiveDate"]);
                        BizActionObj.PatientDetails.ExpiryDate = (DateTime?)DALHelper.HandleDate(reader["ExpiryDate"]);
                        BizActionObj.PatientDetails.LoyaltyCardNo = (string)DALHelper.HandleDBNull(reader["LoyaltyCardNo"]);
                        BizActionObj.PatientDetails.RelationID = (long)DALHelper.HandleDBNull(reader["RelationID"]);
                        BizActionObj.PatientDetails.ParentPatientID = (long)DALHelper.HandleDBNull(reader["ParentPatientID"]);
                        BizActionObj.PatientDetails.GeneralDetails.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                        BizActionObj.PatientDetails.CreatedUnitId = (Int64)DALHelper.HandleDBNull(reader["CreatedUnitId"]);
                        BizActionObj.PatientDetails.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                        BizActionObj.PatientDetails.AddedBy = (Int64?)DALHelper.HandleDBNull(reader["AddedBy"]);
                        BizActionObj.PatientDetails.AddedOn = (String)DALHelper.HandleDBNull(reader["AddedOn"]);
                        BizActionObj.PatientDetails.AddedDateTime = (DateTime?)DALHelper.HandleDate(reader["AddedDateTime"]);
                        BizActionObj.PatientDetails.AddedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]);
                        BizActionObj.PatientDetails.UpdatedUnitId = (Int64?)DALHelper.HandleDBNull(reader["UpdatedUnitID"]);
                        BizActionObj.PatientDetails.UpdatedBy = (Int64?)DALHelper.HandleDBNull(reader["UpdatedBy"]);
                        BizActionObj.PatientDetails.UpdatedOn = (String)DALHelper.HandleDBNull(reader["UpdatedOn"]);
                        BizActionObj.PatientDetails.UpdatedDateTime = (DateTime?)DALHelper.HandleDate(reader["UpdatedDateTime"]);
                        BizActionObj.PatientDetails.UpdatedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["UpdatedWindowsLoginName"]);

                        // for Spouse
                        BizActionObj.PatientDetails.SpouseDetails.RegistrationDate = (DateTime?)DALHelper.HandleDBNull(reader["SpouseRegistrationDate"]);
                        BizActionObj.PatientDetails.SpouseDetails.MRNo = (string)DALHelper.HandleDBNull(reader["SpouseMRNo"]);
                        BizActionObj.PatientDetails.SpouseDetails.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["SpouseLastName"]));
                        BizActionObj.PatientDetails.SpouseDetails.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["SpouseMiddleName"]));
                        BizActionObj.PatientDetails.SpouseDetails.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["SpouseFirstName"]));
                        BizActionObj.PatientDetails.SpouseDetails.FamilyName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["SpouseFamilyName"]));
                        BizActionObj.PatientDetails.SpouseDetails.GenderID = (long)DALHelper.HandleDBNull(reader["SpouseGenderID"]);
                        BizActionObj.PatientDetails.SpouseDetails.DateOfBirth = (DateTime?)DALHelper.HandleDate(reader["SpouseDateOfBirth"]);
                        BizActionObj.PatientDetails.SpouseDetails.BloodGroupID = (long)DALHelper.HandleDBNull(reader["SpouseBloodGroupID"]);
                        BizActionObj.PatientDetails.SpouseDetails.MaritalStatusID = (long)DALHelper.HandleDBNull(reader["SpouseMaritalStatusID"]);
                        BizActionObj.PatientDetails.SpouseDetails.ContactNo1 = (string)DALHelper.HandleDBNull(reader["SpouseContactNo1"]);
                        BizActionObj.PatientDetails.SpouseDetails.ContactNo2 = (string)DALHelper.HandleDBNull(reader["SpouseContactNo2"]);
                        BizActionObj.PatientDetails.SpouseDetails.FaxNo = (string)DALHelper.HandleDBNull(reader["SpouseFaxNo"]);
                        BizActionObj.PatientDetails.SpouseDetails.Email = Security.base64Decode((string)DALHelper.HandleDBNull(reader["SpouseEmail"]));
                        BizActionObj.PatientDetails.SpouseDetails.AddressLine1 = Security.base64Decode((string)DALHelper.HandleDBNull(reader["SpouseAddressLine1"]));
                        BizActionObj.PatientDetails.SpouseDetails.AddressLine2 = Security.base64Decode((string)DALHelper.HandleDBNull(reader["SpouseAddressLine2"]));
                        BizActionObj.PatientDetails.SpouseDetails.AddressLine3 = Security.base64Decode((string)DALHelper.HandleDBNull(reader["SpouseAddressLine3"]));
                        BizActionObj.PatientDetails.SpouseDetails.Pincode = Security.base64Decode((string)DALHelper.HandleDBNull(reader["SpousePincode"]));

                        if ((Byte[])DALHelper.HandleDBNull(reader["SpousePhoto"]) != null)
                        {
                            BizActionObj.PatientDetails.SpouseDetails.Photo = (Byte[])DALHelper.HandleDBNull(reader["SpousePhoto"]);
                        }


                        BizActionObj.PatientDetails.SpouseDetails.OccupationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpouseOccupationId"]));
                        BizActionObj.PatientDetails.SpouseDetails.CivilID = (String)DALHelper.HandleDBNull(reader["SpouseCivilID"]);
                        BizActionObj.PatientDetails.SpouseDetails.District = Security.base64Decode((String)DALHelper.HandleDBNull(reader["SpouseDistrict"]));
                        BizActionObj.PatientDetails.SpouseDetails.Taluka = Security.base64Decode((String)DALHelper.HandleDBNull(reader["SpouseTaluka"]));
                        BizActionObj.PatientDetails.SpouseDetails.Area = Security.base64Decode((String)DALHelper.HandleDBNull(reader["SpouseArea"]));
                        BizActionObj.PatientDetails.SpouseDetails.MobileCountryCode = Convert.ToString(DALHelper.HandleDBNull(reader["SpouseMobileCountryCode"]));
                        BizActionObj.PatientDetails.SpouseDetails.ResiNoCountryCode = (long)DALHelper.HandleDBNull(reader["SpouseResiNoCountryCode"]);
                        BizActionObj.PatientDetails.SpouseDetails.Country = Security.base64Decode((string)DALHelper.HandleDBNull(reader["SpouseCountry"]));
                        BizActionObj.PatientDetails.SpouseDetails.State = Security.base64Decode((string)DALHelper.HandleDBNull(reader["SpouseState"]));
                        BizActionObj.PatientDetails.SpouseDetails.City = Security.base64Decode((string)DALHelper.HandleDBNull(reader["SpouseCity"]));
                        BizActionObj.PatientDetails.SpouseDetails.SpouseCompanyName = (string)DALHelper.HandleDBNull(reader["SpouseCompanyName"]);
                        //BizActionObj.PatientDetails.SpouseDetails.re = (long)DALHelper.HandleDBNull(reader["SpouseReferralTypeID"]);
                        BizActionObj.PatientDetails.SpouseDetails.CountryID = (long)DALHelper.HandleIntegerNull(reader["SpouseCountryID"]);
                        BizActionObj.PatientDetails.SpouseDetails.StateID = (long)DALHelper.HandleIntegerNull(reader["SpouseStateID"]);
                        BizActionObj.PatientDetails.SpouseDetails.CityID = (long)DALHelper.HandleIntegerNull(reader["SpouseCityID"]);
                        BizActionObj.PatientDetails.SpouseDetails.RegionID = (long)DALHelper.HandleIntegerNull(reader["SpouseRegionID"]);
                        BizActionObj.PatientDetails.SpouseDetails.ReligionID = (long)DALHelper.HandleIntegerNull(reader["SpouseReligionID"]);


                        //For Source
                        BizActionObj.PatientDetails.AgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgencyID"]));
                        //BizActionObj.PatientDetails.SourceEmail = Convert.ToString(DALHelper.HandleDBNull(reader["SourceEmail"]));
                        //BizActionObj.PatientDetails.SourceContactNo = Convert.ToString(DALHelper.HandleDBNull(reader["SourceContactNo"]));
                        BizActionObj.PatientDetails.SurrogateOtherDetails = Convert.ToString(DALHelper.HandleDBNull(reader["SurrogateOtherDetails"]));


                    }


                }
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
        public override IValueObject GetPatientGeneralDetailsListForSurrogacy(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientGeneralDetailsListForSurrogacyBizActionVO BizActionObj = valueObject as clsGetPatientGeneralDetailsListForSurrogacyBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientGeneralDetailsListForSurrogacySearch");
                DbDataReader reader;

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "MrNo", DbType.String, BizActionObj.MRNo + "%");

                //if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                //    dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(BizActionObj.FirstName) + "%");
                //if (BizActionObj.MiddleName != null && BizActionObj.MiddleName.Length != 0)
                //    dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(BizActionObj.MiddleName) + "%");
                //if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                //    dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(BizActionObj.LastName) + "%");
                //if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.FirstName + "%");
                if (BizActionObj.MiddleName != null && BizActionObj.MiddleName.Length != 0)
                    dbServer.AddInParameter(command, "MiddleName", DbType.String, BizActionObj.MiddleName + "%");
                if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.LastName + "%");
                if (BizActionObj.FamilyName != null && BizActionObj.FamilyName.Length != 0)

                    if (BizActionObj.FamilyName != null && BizActionObj.FamilyName.Length != 0)
                        dbServer.AddInParameter(command, "FamilyName", DbType.String, Security.base64Encode(BizActionObj.FamilyName) + "%");
                if (BizActionObj.OPDNo != null && BizActionObj.OPDNo.Length != 0)
                {
                    if (BizActionObj.VisitWise == true)
                        dbServer.AddInParameter(command, "OPDNo", DbType.String, BizActionObj.OPDNo + "%");
                    else if (BizActionObj.AdmissionWise == true)
                        dbServer.AddInParameter(command, "AdmissionNo", DbType.String, BizActionObj.OPDNo + "%");
                }

                if (BizActionObj.ContactNo != null && BizActionObj.ContactNo.Length != 0)
                    dbServer.AddInParameter(command, "ContactNo", DbType.String, BizActionObj.ContactNo + "%");
                if (BizActionObj.Country != null && BizActionObj.Country.Length != 0)
                    dbServer.AddInParameter(command, "Country", DbType.String, Security.base64Encode(BizActionObj.Country) + "%");
                if (BizActionObj.State != null && BizActionObj.State.Length != 0)
                    dbServer.AddInParameter(command, "State", DbType.String, Security.base64Encode(BizActionObj.State) + "%");
                if (BizActionObj.City != null && BizActionObj.City.Length != 0)
                    dbServer.AddInParameter(command, "City", DbType.String, Security.base64Encode(BizActionObj.City) + "%");
                if (BizActionObj.Pincode != null && BizActionObj.Pincode.Length != 0)
                    dbServer.AddInParameter(command, "Pincode", DbType.String, Security.base64Encode(BizActionObj.Pincode) + "%");
                if (BizActionObj.CivilID != null && BizActionObj.CivilID.Length != 0)
                    dbServer.AddInParameter(command, "CivilID", DbType.String, Security.base64Encode(BizActionObj.CivilID) + "%");
                if (BizActionObj.GenderID != null && BizActionObj.GenderID != 0)
                    dbServer.AddInParameter(command, "GenderID", DbType.Int64, BizActionObj.GenderID);
                if (BizActionObj.PatientCategoryID != null && BizActionObj.PatientCategoryID != 0)
                    dbServer.AddInParameter(command, "PatientCategory", DbType.Int64, BizActionObj.PatientCategoryID);

                if (BizActionObj.FromDate != null)
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                if (BizActionObj.ToDate != null)
                {
                    if (BizActionObj.FromDate != null)
                    {
                        //if (BizActionObj.FromDate.Equals(BizActionObj.ToDate))
                        BizActionObj.ToDate = BizActionObj.ToDate.Value.Date.AddDays(1);
                    }

                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                }

                if (BizActionObj.VisitWise == true)
                {
                    dbServer.AddInParameter(command, "Serchfor", DbType.Int16, (Int16)1);

                    if (BizActionObj.VisitFromDate != null)
                        dbServer.AddInParameter(command, "VisitFromDate", DbType.DateTime, BizActionObj.VisitFromDate);
                    if (BizActionObj.VisitToDate != null)
                    {
                        if (BizActionObj.VisitFromDate != null)
                        {
                            BizActionObj.VisitToDate = BizActionObj.VisitToDate.Value.Date.AddDays(1);
                        }
                        dbServer.AddInParameter(command, "VisitToDate", DbType.DateTime, BizActionObj.VisitToDate);
                    }
                }

                if (BizActionObj.AdmissionWise == true)
                {
                    dbServer.AddInParameter(command, "Serchfor", DbType.Int16, (Int16)2);

                    if (BizActionObj.AdmissionFromDate != null)
                        dbServer.AddInParameter(command, "AdmissionFromDate", DbType.DateTime, BizActionObj.AdmissionFromDate);
                    if (BizActionObj.AdmissionToDate != null)
                    {
                        if (BizActionObj.AdmissionToDate != null)
                        {
                            BizActionObj.AdmissionToDate = BizActionObj.AdmissionToDate.Value.Date.AddDays(1);
                        }
                        dbServer.AddInParameter(command, "AdmissionToDate", DbType.DateTime, BizActionObj.AdmissionToDate);
                    }
                }
                //=====================================================
                if (BizActionObj.DOBWise == true)
                {
                    dbServer.AddInParameter(command, "Serchfor", DbType.Int16, (Int16)0);

                    if (BizActionObj.DOBFromDate != null)
                        dbServer.AddInParameter(command, "DOBFromDate", DbType.DateTime, BizActionObj.DOBFromDate);
                    if (BizActionObj.DOBToDate != null)
                    {
                        if (BizActionObj.DOBToDate != null)
                        {
                            BizActionObj.DOBToDate = BizActionObj.DOBToDate.Value.Date.AddDays(1);
                        }
                        dbServer.AddInParameter(command, "DOBToDate", DbType.DateTime, BizActionObj.DOBToDate);
                    }
                }
                //=====================================================

                if (BizActionObj.IsLoyaltyMember == true)
                {
                    dbServer.AddInParameter(command, "IsLoyaltyMember", DbType.Boolean, BizActionObj.IsLoyaltyMember);
                    dbServer.AddInParameter(command, "LoyaltyProgramID", DbType.Int64, BizActionObj.LoyaltyProgramID);
                }
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

                dbServer.AddInParameter(command, "RegistrationTypeID", DbType.Int64, BizActionObj.RegistrationTypeID);

                if (BizActionObj.SearchInAnotherClinic == true)
                {
                    dbServer.AddInParameter(command, "SearchInAnotherClinic", DbType.Boolean, BizActionObj.SearchInAnotherClinic);
                }

                //dbServer.AddInParameter(command, "IncludeSpouse", DbType.Boolean, BizActionObj.IncludeSpouse);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "LinkServer", DbType.String, BizActionObj.LinkServer);
                if (BizActionObj.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, BizActionObj.LinkServer.Replace(@"\", "_"));

                //By Anjali.....................
                dbServer.AddInParameter(command, "IsFromSurrogacyModule", DbType.Boolean, BizActionObj.IsFromSurrogacyModule);
                dbServer.AddInParameter(command, "AgencyID", DbType.Int64, BizActionObj.AgencyID);
                //................................

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.PatientDetailsList == null)
                        BizActionObj.PatientDetailsList = new List<clsPatientGeneralVO>();
                    while (reader.Read())
                    {
                        clsPatientGeneralVO objPatientVO = new clsPatientGeneralVO();

                        objPatientVO.PatientID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objPatientVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitId"]);
                        objPatientVO.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                        //objPatientVO.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                        //objPatientVO.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                        //objPatientVO.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));

                        objPatientVO.LastName = (string)DALHelper.HandleDBNull(reader["LastName"]);
                        objPatientVO.FirstName = (string)DALHelper.HandleDBNull(reader["FirstName"]);
                        objPatientVO.MiddleName = (string)DALHelper.HandleDBNull(reader["MiddleName"]);


                        objPatientVO.DateOfBirth = (DateTime?)DALHelper.HandleDate(reader["DateofBirth"]);
                        objPatientVO.RegistrationDate = (DateTime)DALHelper.HandleDate(reader["RegistrationDate"]);
                        if (BizActionObj.VisitWise == true || BizActionObj.RegistrationWise == true)
                        {
                            objPatientVO.VisitID = (long)DALHelper.HandleDBNull(reader["VAID"]);
                            objPatientVO.OPDNO = (string)DALHelper.HandleDBNull(reader["OINO"]);

                        }
                        else
                        {
                            objPatientVO.IPDAdmissionID = (long)DALHelper.HandleDBNull(reader["VAID"]);
                            objPatientVO.IPDAdmissionNo = (string)DALHelper.HandleDBNull(reader["OINO"]);
                        }

                        if (BizActionObj.VisitWise == true)
                            objPatientVO.PatientKind = PatientsKind.OPD;
                        else if (BizActionObj.AdmissionWise == true)
                            objPatientVO.PatientKind = PatientsKind.IPD;
                        else if (objPatientVO.VisitID == 0 && objPatientVO.IPDAdmissionID == 0)
                            objPatientVO.PatientKind = PatientsKind.Registration;

                        objPatientVO.TariffID = (long)DALHelper.HandleDBNull(reader["TariffID"]);
                        objPatientVO.PatientSourceID = (long)DALHelper.HandleDBNull(reader["PatientSourceID"]);
                        objPatientVO.CompanyID = (long)DALHelper.HandleDBNull(reader["CompanyID"]);
                        objPatientVO.ContactNO1 = (string)DALHelper.HandleDBNull(reader["ContactNO1"]);
                        objPatientVO.Gender = (string)DALHelper.HandleDBNull(reader["Gender"]);
                        objPatientVO.MaritalStatus = (string)DALHelper.HandleDBNull(reader["MaritalStatus"]);
                        objPatientVO.UniversalID = Security.base64Decode((string)DALHelper.HandleDBNull(reader["CivilID"]));
                        objPatientVO.PatientTypeID = (long)DALHelper.HandleDBNull(reader["PatientCategoryID"]);
                        //objPatientVO.RegistrationType = (string)DALHelper.HandleDBNull(reader["RegistrationType"]);       // ..................BHUSHAN
                        //objPatientVO.SpouseID = (long)DALHelper.HandleDBNull(reader["SpouseID"]);
                        objPatientVO.LinkServer = BizActionObj.LinkServer;
                        objPatientVO.AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"]));
                        objPatientVO.IsDocAttached = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDocAttached"])); //***//

                        BizActionObj.PatientDetailsList.Add(objPatientVO);
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


        //..................

        //EMR
        public override IValueObject GetPatientHeaderDetailsForConsole(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientConsoleHeaderDeailsBizActionVO BizActionObj = valueObject as clsGetPatientConsoleHeaderDeailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientDetailsConsole");
                DbDataReader reader;

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "ISOPDIPD", DbType.Int64, BizActionObj.ISOPDIPD);
                //dbServer.AddInParameter(command, "PatientUnitId", DbType.Int64, BizActionObj.PatientUnitID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.PatientDetails == null)
                        BizActionObj.PatientDetails = new clsPatientConsoleHeaderVO();

                    while (reader.Read())
                    {
                        BizActionObj.PatientDetails.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        BizActionObj.PatientDetails.LastName = Convert.ToString(DALHelper.HandleDBNull(reader["SurName"]));
                        BizActionObj.PatientDetails.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));

                        BizActionObj.PatientDetails.AgeInYearMonthDays = Convert.ToString(DALHelper.HandleDBNull(reader["AgeInYearMonthDays"]));
                        BizActionObj.PatientDetails.Age = Convert.ToInt32(DALHelper.HandleDBNull(reader["Age"]));
                        if (BizActionObj.PatientDetails.Age > 1)
                            BizActionObj.PatientDetails.AgeInYearMonthDays = BizActionObj.PatientDetails.Age.ToString() + " Years";
                        else
                            BizActionObj.PatientDetails.AgeInYearMonthDays = BizActionObj.PatientDetails.Age.ToString() + " Year";

                        BizActionObj.PatientDetails.RegisteredClinic = ((String)DALHelper.HandleDBNull(reader["RegisteredClinic"]));

                        BizActionObj.PatientDetails.MaritalStatus = Convert.ToString(reader["MaritalStatus"]);
                        BizActionObj.PatientDetails.Gender = Convert.ToString(reader["Gender"]);
                        BizActionObj.PatientDetails.MOB = Convert.ToString(reader["MOB"]);
                        BizActionObj.PatientDetails.BirthDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["DateOfBirth"]));
                        BizActionObj.PatientDetails.Photo = (Byte[])DALHelper.HandleDBNull(reader["Photo"]);

                        //added by neena
                        //BizActionObj.PatientDetails.ImageName = Convert.ToString(DALHelper.HandleDBNull(reader["ImgPath"]));  

                        string ImgPath = Convert.ToString(DALHelper.HandleDBNull(reader["ImgPath"]));

                        BizActionObj.PatientDetails.ImageName = "https://" + ImgIP + "/" + ImgVirtualDir + "/" + ImgPath;

                        BizActionObj.PatientDetails.Education = Convert.ToString(reader["Education"]);
                        BizActionObj.PatientDetails.Religion = Convert.ToString(reader["Religion"]);
                        BizActionObj.PatientDetails.Weight = Convert.ToInt64(DALHelper.HandleDBNull(reader["Weight"]));
                        BizActionObj.PatientDetails.Height = Convert.ToInt64(DALHelper.HandleDBNull(reader["Height"]));
                        BizActionObj.PatientDetails.BMI = (BizActionObj.PatientDetails.Weight / (BizActionObj.PatientDetails.Height * BizActionObj.PatientDetails.Height / 10000));
                        if (Double.IsNaN(BizActionObj.PatientDetails.BMI))
                        {
                            BizActionObj.PatientDetails.BMI = 0.00;
                        }
                        if (Double.IsInfinity(BizActionObj.PatientDetails.BMI))
                        {
                            BizActionObj.PatientDetails.BMI = 0.00;
                        }
                    }
                }

                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }

            return valueObject;
        }
        public override IValueObject SavePatientPhoto(IValueObject valueObject, clsUserVO UserVo)
        {
            clsSavePhotoBizActionVO BizAction = valueObject as clsSavePhotoBizActionVO;
            DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdatePatientPhoto");
            dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.PatientID);
            dbServer.AddInParameter(command, "Photo", DbType.Binary, BizAction.Photo);
            int intStatus = dbServer.ExecuteNonQuery(command);
            return BizAction;
        }
        public override IValueObject GetEMRAdmVisitListByPatientID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetEMRAdmVisitListByPatientIDBizActionVO BizActionObj = (clsGetEMRAdmVisitListByPatientIDBizActionVO)valueObject;
            BizActionObj.PatientDetails = new clsPatientConsoleHeaderVO();
            DbDataReader reader = null;
            try
            {
                DbCommand dbCommand = dbServer.GetStoredProcCommand("CIMS_GetPatientDetailsConsole");
                dbServer.AddInParameter(dbCommand, "PatientID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(dbCommand, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(dbCommand, "ISOPDIPD", DbType.Int64, BizActionObj.ISOPDIPD);
                reader = (DbDataReader)dbServer.ExecuteReader(dbCommand);
                if (reader.HasRows)
                {
                    if (BizActionObj.PatientDetails == null)
                        BizActionObj.PatientDetails = new clsPatientConsoleHeaderVO();

                    while (reader.Read())
                    {
                        BizActionObj.PatientDetails.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        BizActionObj.PatientDetails.LastName = Convert.ToString(DALHelper.HandleDBNull(reader["SurName"]));
                        BizActionObj.PatientDetails.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        BizActionObj.PatientDetails.AgeInYearMonthDays = Convert.ToString(DALHelper.HandleDBNull(reader["AgeInYearMonthDays"]));
                        BizActionObj.PatientDetails.Age = Convert.ToInt32(DALHelper.HandleDBNull(reader["Age"]));
                        if (BizActionObj.PatientDetails.Age > 1)
                            BizActionObj.PatientDetails.AgeInYearMonthDays = BizActionObj.PatientDetails.Age.ToString() + " Years";
                        else
                            BizActionObj.PatientDetails.AgeInYearMonthDays = BizActionObj.PatientDetails.Age.ToString() + " Year";
                        BizActionObj.PatientDetails.RegisteredClinic = ((String)DALHelper.HandleDBNull(reader["RegisteredClinic"]));
                        BizActionObj.PatientDetails.MaritalStatus = Convert.ToString(reader["MaritalStatus"]);
                        BizActionObj.PatientDetails.Gender = Convert.ToString(reader["Gender"]);
                        BizActionObj.PatientDetails.MOB = Convert.ToString(reader["MOB"]);
                        BizActionObj.PatientDetails.BirthDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["DateOfBirth"]));
                        BizActionObj.PatientDetails.Photo = (Byte[])DALHelper.HandleDBNull(reader["Photo"]);
                        //added by neena
                        //BizActionObj.PatientDetails.ImageName = Convert.ToString(DALHelper.HandleDBNull(reader["ImgPath"]));  

                        string ImgPath = Convert.ToString(DALHelper.HandleDBNull(reader["ImgPath"]));
                        BizActionObj.PatientDetails.ImageName = "https://" + ImgIP + "/" + ImgVirtualDir + "/" + ImgPath;

                        // BizActionObj.PatientDetails.Education = Convert.ToString(reader["Education"]);
                        BizActionObj.PatientDetails.Religion = Convert.ToString(reader["Religion"]);
                        BizActionObj.PatientDetails.Allergies = Convert.ToString(reader["Allergies"]);

                        BizActionObj.PatientDetails.Weight = Convert.ToInt64(DALHelper.HandleDBNull(reader["Weight"]));
                        BizActionObj.PatientDetails.Height = Convert.ToInt64(DALHelper.HandleDBNull(reader["Height"]));
                        BizActionObj.PatientDetails.BMI = (BizActionObj.PatientDetails.Weight / (BizActionObj.PatientDetails.Height * BizActionObj.PatientDetails.Height / 10000));
                        if (Double.IsNaN(BizActionObj.PatientDetails.BMI))
                        {
                            BizActionObj.PatientDetails.BMI = 0.00;
                        }
                        if (Double.IsInfinity(BizActionObj.PatientDetails.BMI))
                        {
                            BizActionObj.PatientDetails.BMI = 0.00;
                        }
                    }
                }
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetEMRAdmVisitListByPatientID");
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.EMRList == null)
                        BizActionObj.EMRList = new List<clsPatientConsoleHeaderVO>();

                    while (reader.Read())
                    {
                        clsPatientConsoleHeaderVO objPatientConsoleVO = new clsPatientConsoleHeaderVO();
                        objPatientConsoleVO.ID = Convert.ToInt64(reader["ID"]);
                        objPatientConsoleVO.OPD_IPD_ID = Convert.ToInt64(reader["VisitAdmID"]);//admissionid
                        objPatientConsoleVO.DoctorName = Convert.ToString(reader["DoctorName"]);
                        objPatientConsoleVO.DoctorCode = Convert.ToString(reader["DoctorID"]);
                        objPatientConsoleVO.DepartmentCode = Convert.ToString(reader["DepartmentID"]);
                        objPatientConsoleVO.DepartmentName = Convert.ToString(reader["DepartmentName"]);
                        objPatientConsoleVO.IPDOPDNO = Convert.ToString(reader["IPDOPDNO"]);
                        objPatientConsoleVO.IPDOPD = Convert.ToString(reader["IPDOPD"]);
                        objPatientConsoleVO.OPD_IPD = Convert.ToBoolean(reader["OPD_IPD"]);//OPD_IPD=>true then ipd else opd
                        objPatientConsoleVO.Date = (DateTime?)DALHelper.HandleDBNull(reader["Date"]);
                        //objPatientConsoleVO.DischargeDate = (DateTime?)DALHelper.HandleDBNull(reader["DischargeDate"]);
                        objPatientConsoleVO.ISDischarged = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ISDischarged"]));
                        objPatientConsoleVO.Bed = Convert.ToString(DALHelper.HandleDBNull(reader["Bed"]));

                        DbCommand commandEMR = dbServer.GetStoredProcCommand("CIMS_GetEMRENcounterListFlags");
                        dbServer.AddInParameter(commandEMR, "PatientID", DbType.Int64, BizActionObj.ID);
                        dbServer.AddInParameter(commandEMR, "OPD_IPD_ID", DbType.Int64, objPatientConsoleVO.OPD_IPD_ID);
                        dbServer.AddInParameter(commandEMR, "OPD_IPD", DbType.Boolean, objPatientConsoleVO.OPD_IPD);
                        dbServer.AddInParameter(commandEMR, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);

                        DbDataReader readerEMR = (DbDataReader)dbServer.ExecuteReader(commandEMR);
                        if (readerEMR.HasRows)
                        {
                            objPatientConsoleVO.IsAllergy = "Visible";
                            objPatientConsoleVO.IsNonAllergy = "Collapsed";
                        }

                        readerEMR.NextResult();
                        if (readerEMR.HasRows)
                        {
                            objPatientConsoleVO.IsDiagnosisRight = "Visible";
                            objPatientConsoleVO.IsDiagnosisWrong = "Collapsed";
                        }

                        readerEMR.NextResult();

                        if (readerEMR.HasRows)
                        {
                            objPatientConsoleVO.IsLaboratoryRight = "Visible";
                            objPatientConsoleVO.IsLaboratoryWrong = "Collapsed";
                        }

                        readerEMR.NextResult();
                        if (readerEMR.HasRows)
                        {
                            objPatientConsoleVO.IsRadiologyRight = "Visible";
                            objPatientConsoleVO.IsRadiologyWrong = "Collapsed";
                        }

                        //readerEMR.NextResult();
                        //if (readerEMR.HasRows)
                        //{
                        //    objPatientConsoleVO.IsDiagnostickRight = "Visible";
                        //    objPatientConsoleVO.IsDiagnostickWrong = "Collapsed";
                        //}
                        readerEMR.NextResult();
                        if (readerEMR.HasRows)
                        {
                            objPatientConsoleVO.IsPrescriptionRight = "Visible";
                            objPatientConsoleVO.IsPrescriptionwrong = "Collapsed";
                        }
                        //readerEMR.NextResult();
                        //if (readerEMR.HasRows)
                        //{
                        //    objPatientConsoleVO.IsCompdPrescRight = "Visible";
                        //    objPatientConsoleVO.IsCompdPrescWrong = "Collapsed";
                        //}

                        //readerEMR.NextResult();
                        //if (readerEMR.HasRows)
                        //{
                        //    objPatientConsoleVO.IsDocumentRight = "Visible";
                        //    objPatientConsoleVO.IsDocumentwrong = "Collapsed";
                        //}
                        readerEMR.NextResult();
                        if (readerEMR.HasRows)
                        {
                            while (readerEMR.Read())
                            {
                                objPatientConsoleVO.RoundID = Convert.ToInt64(readerEMR["RoundID"]);
                                objPatientConsoleVO.currDocSpecCode = Convert.ToString(readerEMR["SpecID"]);
                                objPatientConsoleVO.currDocSpecName = Convert.ToString(readerEMR["SpecName"]);
                                objPatientConsoleVO.CurrDoctorCode = Convert.ToString(readerEMR["DoctorID"]);
                                objPatientConsoleVO.currDoctorName = Convert.ToString(readerEMR["DoctorName"]);
                            }
                        }
                        readerEMR.NextResult();
                        if (readerEMR.HasRows)
                        {
                            objPatientConsoleVO.IsProcedure = "Visible";
                            objPatientConsoleVO.IsNonProcedure = "Collapsed";
                        }
                        BizActionObj.EMRList.Add(objPatientConsoleVO);
                        readerEMR.Close();
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

        //public override IValueObject GetEMRAdmVisitListByPatientID(IValueObject valueObject, clsUserVO objUserVO)
        //{
        //    clsGetEMRAdmVisitListByPatientIDBizActionVO BizActionObj = (clsGetEMRAdmVisitListByPatientIDBizActionVO)valueObject;
        //    BizActionObj.PatientDetails = new clsPatientConsoleHeaderVO();
        //    DbDataReader reader = null;
        //    try
        //    {

        //        DbCommand dbCommand = dbServer.GetStoredProcCommand("CIMS_GetPatientDetailsConsole");

        //        dbServer.AddInParameter(dbCommand, "PatientID", DbType.Int64, BizActionObj.ID);

        //        reader = (DbDataReader)dbServer.ExecuteReader(dbCommand);

        //        if (reader.HasRows)
        //        {
        //            if (BizActionObj.PatientDetails == null)
        //                BizActionObj.PatientDetails = new clsPatientConsoleHeaderVO();

        //            while (reader.Read())
        //            {
        //                BizActionObj.PatientDetails.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
        //                BizActionObj.PatientDetails.LastName = Convert.ToString(DALHelper.HandleDBNull(reader["SurName"]));
        //                BizActionObj.PatientDetails.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));

        //                BizActionObj.PatientDetails.AgeInYearMonthDays = Convert.ToString(DALHelper.HandleDBNull(reader["AgeInYearMonthDays"]));
        //                BizActionObj.PatientDetails.Age = Convert.ToInt32(DALHelper.HandleDBNull(reader["Age"]));
        //                if (BizActionObj.PatientDetails.Age > 1)
        //                    BizActionObj.PatientDetails.AgeInYearMonthDays = BizActionObj.PatientDetails.Age.ToString() + " Years";
        //                else
        //                    BizActionObj.PatientDetails.AgeInYearMonthDays = BizActionObj.PatientDetails.Age.ToString() + " Year";

        //                BizActionObj.PatientDetails.RegisteredClinic = ((String)DALHelper.HandleDBNull(reader["RegisteredClinic"]));

        //                BizActionObj.PatientDetails.MaritalStatus = Convert.ToString(reader["MaritalStatus"]);
        //                BizActionObj.PatientDetails.Gender = Convert.ToString(reader["Gender"]);
        //                BizActionObj.PatientDetails.MOB = Convert.ToString(reader["MOB"]);
        //                BizActionObj.PatientDetails.BirthDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["DateOfBirth"]));
        //                BizActionObj.PatientDetails.Photo = (Byte[])DALHelper.HandleDBNull(reader["Photo"]);
        //               // BizActionObj.PatientDetails.Education = Convert.ToString(reader["Education"]);
        //                BizActionObj.PatientDetails.Religion = Convert.ToString(reader["Religion"]);
        //                BizActionObj.PatientDetails.Allergies = Convert.ToString(reader["Allergies"]);
        //            }
        //        }

        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetEMRAdmVisitListByPatientID");
        //        dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
        //        dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
        //        dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
        //        dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
        //        dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

        //        reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {
        //            if (BizActionObj.EMRList == null)
        //                BizActionObj.EMRList = new List<clsPatientConsoleHeaderVO>();

        //            while (reader.Read())
        //            {
        //                clsPatientConsoleHeaderVO objPatientConsoleVO = new clsPatientConsoleHeaderVO();
        //                objPatientConsoleVO.ID = Convert.ToInt64(reader["ID"]);
        //                objPatientConsoleVO.OPD_IPD_ID = Convert.ToInt64(reader["VisitAdmID"]);
        //                objPatientConsoleVO.DoctorName = Convert.ToString(reader["DoctorName"]);
        //                objPatientConsoleVO.DoctorCode = Convert.ToString(reader["DoctorID"]);
        //                objPatientConsoleVO.DepartmentCode = Convert.ToString(reader["DepartmentID"]);
        //                objPatientConsoleVO.DepartmentName = Convert.ToString(reader["DepartmentName"]);
        //                objPatientConsoleVO.IPDOPDNO = Convert.ToString(reader["IPDOPDNO"]);
        //                objPatientConsoleVO.IPDOPD = Convert.ToString(reader["IPDOPD"]);
        //                objPatientConsoleVO.OPD_IPD = Convert.ToBoolean(reader["OPD_IPD"]);
        //                objPatientConsoleVO.Date = (DateTime?)DALHelper.HandleDBNull(reader["Date"]);
        //                objPatientConsoleVO.DischargeDate = (DateTime?)DALHelper.HandleDBNull(reader["DischargeDate"]);
        //                objPatientConsoleVO.ISDischarged = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ISDischarged"]));
        //                objPatientConsoleVO.Bed = Convert.ToString(DALHelper.HandleDBNull(reader["Bed"]));

        //                DbCommand commandEMR = dbServer.GetStoredProcCommand("CIMS_GetEMRENcounterListFlags");
        //                dbServer.AddInParameter(commandEMR, "PatientID", DbType.Int64, BizActionObj.ID);
        //                dbServer.AddInParameter(commandEMR, "OPD_IPD_ID", DbType.Int64, objPatientConsoleVO.OPD_IPD_ID);
        //                dbServer.AddInParameter(commandEMR, "OPD_IPD", DbType.Int64, objPatientConsoleVO.OPD_IPD);

        //                DbDataReader readerEMR = (DbDataReader)dbServer.ExecuteReader(commandEMR);
        //                if (readerEMR.HasRows)
        //                {
        //                    objPatientConsoleVO.IsAllergy = "Visible";
        //                    objPatientConsoleVO.IsNonAllergy = "Collapsed";
        //                }

        //                readerEMR.NextResult();
        //                if (readerEMR.HasRows)
        //                {
        //                    objPatientConsoleVO.IsDiagnosisRight = "Visible";
        //                    objPatientConsoleVO.IsDiagnosisWrong = "Collapsed";
        //                }

        //                readerEMR.NextResult();

        //                if (readerEMR.HasRows)
        //                {
        //                    objPatientConsoleVO.IsLaboratoryRight = "Visible";
        //                    objPatientConsoleVO.IsLaboratoryWrong = "Collapsed";
        //                }

        //                readerEMR.NextResult();
        //                if (readerEMR.HasRows)
        //                {
        //                    objPatientConsoleVO.IsRadiologyRight = "Visible";
        //                    objPatientConsoleVO.IsRadiologyWrong = "Collapsed";
        //                }

        //                readerEMR.NextResult();
        //                if (readerEMR.HasRows)
        //                {
        //                    objPatientConsoleVO.IsDiagnostickRight = "Visible";
        //                    objPatientConsoleVO.IsDiagnostickWrong = "Collapsed";
        //                }
        //                readerEMR.NextResult();
        //                if (readerEMR.HasRows)
        //                {
        //                    objPatientConsoleVO.IsPrescriptionRight = "Visible";
        //                    objPatientConsoleVO.IsPrescriptionwrong = "Collapsed";
        //                }
        //                readerEMR.NextResult();
        //                if (readerEMR.HasRows)
        //                {
        //                    objPatientConsoleVO.IsCompdPrescRight = "Visible";
        //                    objPatientConsoleVO.IsCompdPrescWrong = "Collapsed";
        //                }

        //                readerEMR.NextResult();
        //                if (readerEMR.HasRows)
        //                {
        //                    objPatientConsoleVO.IsDocumentRight = "Visible";
        //                    objPatientConsoleVO.IsDocumentwrong = "Collapsed";
        //                }
        //                readerEMR.NextResult();
        //                if (readerEMR.HasRows)
        //                {
        //                    while (readerEMR.Read())
        //                    {
        //                        objPatientConsoleVO.RoundID = Convert.ToInt64(readerEMR["RoundID"]);
        //                        objPatientConsoleVO.currDocSpecCode = Convert.ToString(readerEMR["SpecCode"]);
        //                        objPatientConsoleVO.currDocSpecName = Convert.ToString(readerEMR["SpecName"]);
        //                        objPatientConsoleVO.CurrDoctorCode = Convert.ToString(readerEMR["Doctorcode"]);
        //                        objPatientConsoleVO.currDoctorName = Convert.ToString(readerEMR["DoctorName"]);
        //                    }
        //                }
        //                readerEMR.NextResult();
        //                if (readerEMR.HasRows)
        //                {
        //                    objPatientConsoleVO.IsProcedure = "Visible";
        //                    objPatientConsoleVO.IsNonProcedure = "Collapsed";
        //                }
        //                BizActionObj.EMRList.Add(objPatientConsoleVO);
        //                readerEMR.Close();
        //            }
        //        }
        //        reader.NextResult();
        //        BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));

        //        reader.Close();
        //    }

        //    catch (Exception ex)
        //    {
        //        throw;

        //    }
        //    finally
        //    {

        //    }
        //    return BizActionObj;
        //}

        public override IValueObject GetEMRAdmVisitListByPatientIDForConsol(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetEMRAdmVisitListByPatientIDBizActionVO BizActionObj = (clsGetEMRAdmVisitListByPatientIDBizActionVO)valueObject;
            BizActionObj.PatientDetails = new clsPatientConsoleHeaderVO();
            DbDataReader reader = null;
            int count = 0;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPrescriptionForPatientForConsol");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.ID);
                //dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "CallFrom", DbType.Int32, BizActionObj.CallFrom);
                //dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                //dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                //dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                //dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.EMRList == null)
                        BizActionObj.EMRList = new List<clsPatientConsoleHeaderVO>();

                    while (reader.Read())
                    {
                        count++;
                        clsPatientConsoleHeaderVO objPatientConsoleVO = new clsPatientConsoleHeaderVO();
                        objPatientConsoleVO.RowNum = Convert.ToInt64(DALHelper.HandleDBNull(reader["RowNum"]));
                        objPatientConsoleVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objPatientConsoleVO.PatientId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientId"]));
                        objPatientConsoleVO.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objPatientConsoleVO.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                        objPatientConsoleVO.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                        objPatientConsoleVO.DepartmentName = Convert.ToString(DALHelper.HandleDBNull(reader["DepartmentName"]));
                        BizActionObj.EMRList.Add(objPatientConsoleVO);
                    }
                }
                reader.NextResult();
                count = 0;
                //BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));

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

        public override IValueObject GetPatientLinkFileViewDetals(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientLinkFileViewDetailsBizActionVO BizActionObj = valueObject as clsGetPatientLinkFileViewDetailsBizActionVO;
            try
            {
                if (BizActionObj.FROMEMR == false)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_GetPatientViewLinkFileDetals");
                    dbServer.AddInParameter(command1, "ID", DbType.Int64, BizActionObj.ReportID);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.PatientID);
                    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);

                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command1);
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            BizActionObj.SourceURL = Convert.ToString(DALHelper.HandleDBNull(reader["SourceURL"]));
                            BizActionObj.Report = (byte[])DALHelper.HandleDBNull(reader["Report"]);
                            //BizActionObj.DocumentName = Convert.ToString(DALHelper.HandleDBNull(reader["DocumentName"]));
                        }
                        reader.Close();
                    }

                }
                else
                {
                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_GetEMRImage");
                    dbServer.AddInParameter(command2, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                    dbServer.AddInParameter(command2, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                    dbServer.AddInParameter(command2, "OPDIPDID", DbType.Int64, BizActionObj.VisitID);
                    DbDataReader reader1 = (DbDataReader)dbServer.ExecuteReader(command2);
                    if (reader1.HasRows)
                    {
                        if (BizActionObj.PatientDetails == null)
                            BizActionObj.PatientDetails = new List<clsPatientLinkFileBizActionVO>();
                        while (reader1.Read())
                        {
                            clsPatientLinkFileBizActionVO Obj = new clsPatientLinkFileBizActionVO();
                            Obj.SourceURL = Convert.ToString(DALHelper.HandleDBNull(reader1["SourceURL"]));
                            Obj.Report = (byte[])DALHelper.HandleDBNull(reader1["Report"]);
                            Obj.DocumentName = Convert.ToString(DALHelper.HandleDBNull(reader1["DocumentName"]));
                            Obj.TemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["TemplateID"]));
                            BizActionObj.PatientDetails.Add(Obj);
                        }


                    }
                    reader1.NextResult();
                    if (reader1.HasRows)
                    {
                        while (reader1.Read())
                        {
                            BizActionObj.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader1["Remark"]));
                        }
                    }
                    reader1.Close();

                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }


        //By Anjali..............................................
        public override IValueObject GetPatientScanDoc(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsGetPatientScanDocument BizActionObj = valueObject as clsGetPatientScanDocument;
            try
            {

                DbCommand commandScanDoc = dbServer.GetStoredProcCommand("CIMS_GetPatientScanDocuments");
                if (BizActionObj.PatientScanDoc.DoctorID > 0)
                {
                    dbServer.AddInParameter(commandScanDoc, "DoctorID", DbType.Int64, BizActionObj.PatientScanDoc.DoctorID);
                    dbServer.AddInParameter(commandScanDoc, "PatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                }
                else
                {
                    dbServer.AddInParameter(commandScanDoc, "PatientID", DbType.Int64, BizActionObj.PatientScanDoc.PatientID);
                    dbServer.AddInParameter(commandScanDoc, "PatientUnitID", DbType.Int64, BizActionObj.PatientScanDoc.PatientUnitID);
                }
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(commandScanDoc);
                if (reader.HasRows)
                {
                    BizActionObj.PatientScanDocList = new List<clsPatientScanDocumentVO>();
                    while (reader.Read())
                    {
                        clsPatientScanDocumentVO PatientAttachmentDetail = new clsPatientScanDocumentVO();
                        PatientAttachmentDetail.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        PatientAttachmentDetail.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        PatientAttachmentDetail.IdentityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IdentityID"]));
                        PatientAttachmentDetail.Identity = Convert.ToString(DALHelper.HandleDBNull(reader["Identity"]));
                        PatientAttachmentDetail.IdentityNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IdentityNumber"]));
                        PatientAttachmentDetail.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        PatientAttachmentDetail.IsForSpouse = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsForSpouse"]));
                        PatientAttachmentDetail.AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"]));
                        // PatientAttachmentDetail.AttachedFileContent = (byte[])(DALHelper.HandleDBNull(reader["AttachedFileContent"]));
                        string ImgPath = Convert.ToString(DALHelper.HandleDBNull(reader["ImagePath"])); //***//
                        if (!string.IsNullOrEmpty(ImgPath))
                            PatientAttachmentDetail.ImageName = "https://" + DocImgIP + "/" + DocImgVirtualDir + "/" + ImgPath;


                        BizActionObj.PatientScanDocList.Add(PatientAttachmentDetail);

                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;

        }

        public override IValueObject AddPatientScanDoc(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsAddUpdatePatientScanDocument BizActionObj = valueObject as clsAddUpdatePatientScanDocument;
            try
            {
                foreach (clsPatientScanDocumentVO item in BizActionObj.PatientScanDocList)
                {

                    DbCommand commandscandocMale = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientScanDocument");
                    dbServer.AddInParameter(commandscandocMale, "ID", DbType.Int64, item.ID);
                    dbServer.AddInParameter(commandscandocMale, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(commandscandocMale, "IdentityID", DbType.Int64, item.IdentityID);
                    dbServer.AddInParameter(commandscandocMale, "IdentityNumber", DbType.String, item.IdentityNumber);
                    dbServer.AddInParameter(commandscandocMale, "Description", DbType.String, item.Description);
                    dbServer.AddInParameter(commandscandocMale, "IsForSpouse", DbType.Boolean, item.IsForSpouse);
                    dbServer.AddInParameter(commandscandocMale, "AttachedFileName", DbType.String, item.AttachedFileName);
                    //dbServer.AddInParameter(commandscandocMale, "AttachedFileContent", DbType.Binary, item.AttachedFileContent);
                    dbServer.AddInParameter(commandscandocMale, "AttachedFileContent", DbType.Binary, null);
                    if (item.DoctorID > 0)
                    {
                        dbServer.AddInParameter(commandscandocMale, "DoctorID", DbType.Int64, item.DoctorID);
                        dbServer.AddInParameter(commandscandocMale, "PatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    }
                    else
                    {
                        dbServer.AddInParameter(commandscandocMale, "PatientID", DbType.Int64, item.PatientID);
                        dbServer.AddInParameter(commandscandocMale, "PatientUnitID", DbType.Int64, item.PatientUnitID);
                    }
                    dbServer.AddInParameter(commandscandocMale, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(commandscandocMale, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(commandscandocMale, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(commandscandocMale, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(commandscandocMale, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(commandscandocMale, "ImagePath", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                    int intscandocMale = dbServer.ExecuteNonQuery(commandscandocMale);

                    item.ImageName = Convert.ToString(dbServer.GetParameterValue(commandscandocMale, "ImagePath"));

                    DemoImage ObjDM = new DemoImage();
                    if (item.AttachedFileContent != null)
                        ObjDM.VaryQualityLevel(item.AttachedFileContent, item.ImageName, DocSaveLocation);

                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
            //..........................................................
        }

        public override IValueObject AddBarcodeImageTODB(IValueObject valueObject, clsUserVO UserVo)
        {
            AddBarcodeImageBizActionVO BizActionObj = valueObject as AddBarcodeImageBizActionVO;

            try
            {
                DbCommand command = null;
                //command = dbServer.GetStoredProcCommand("CIMS_AddBarcodeImage");
                //command.Parameters.Clear();
                //DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_AddBarcodeImage");
                if (BizActionObj.ObjBarcodeImage.GeneralDetailsBarcodeImage != null)
                {
                    dbServer.AddInParameter(command, "GeneralDetailsBarcodeImage", DbType.Binary, BizActionObj.ObjBarcodeImage.GeneralDetailsBarcodeImage);
                }
                if (BizActionObj.ObjBarcodeImage.SpouseBarcodeImage != null)
                {
                    dbServer.AddInParameter(command, "SpouseBarcodeImage", DbType.Binary, BizActionObj.ObjBarcodeImage.SpouseBarcodeImage);
                }
                if (BizActionObj.ObjBarcodeImage.GeneralDetailsMRNo != null)
                {
                    dbServer.AddInParameter(command, "GeneralDetailsMRno", DbType.String, BizActionObj.ObjBarcodeImage.GeneralDetailsMRNo);
                }
                if (BizActionObj.ObjBarcodeImage.SpouseMRNo != null)
                {
                    dbServer.AddInParameter(command, "SpouseMRno", DbType.String, BizActionObj.ObjBarcodeImage.SpouseMRNo);
                }
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intscandocMale = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        // Added By CDS
        public override IValueObject GetPatientBillBalanceAmount(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientBillBalanceAmountBizActionVO BizActionObj = (clsGetPatientBillBalanceAmountBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientBillBalanceAmount");
                DbDataReader reader;

                dbServer.AddInParameter(command, "PatientID", DbType.String, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.PatientGeneralDetails == null)
                        BizActionObj.PatientGeneralDetails = new clsPatientGeneralVO();
                    //BizActionObj.PatientGeneralDetails = new List<clsPatientFamilyDetailsVO>();
                    while (reader.Read())
                    {
                        //clsGetPatientBillBalanceAmountBizActionVO objBillBalAmtVO = new clsGetPatientBillBalanceAmountBizActionVO();

                        BizActionObj.PatientGeneralDetails.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                        BizActionObj.PatientGeneralDetails.PatientUnitID = (long)DALHelper.HandleDBNull(reader["PatientUnitID"]);
                        //objBillBalAmtVO.BalanceAmountSelf = (float)DALHelper.HandleDBNull(reader["BalanceAmountSelf"]);
                        BizActionObj.PatientGeneralDetails.BillBalanceAmountSelf = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmountSelf"]));

                        //objBillBalAmtVO.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                        //objBillBalAmtVO.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                        //objBillBalAmtVO.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                        //objBillBalAmtVO.DateOfBirth = (DateTime)DALHelper.HandleDate(reader["DOB"]);
                        //objBillBalAmtVO.TariffID = (long)DALHelper.HandleDBNull(reader["TariffID"]);
                        //objBillBalAmtVO.RelationID = (long)DALHelper.HandleDBNull(reader["RelationID"]);
                        //objBillBalAmtVO.Tariff = (string)DALHelper.HandleDBNull(reader["Tariff"]);
                        //objBillBalAmtVO.Relation = (string)DALHelper.HandleDBNull(reader["Relation"]);
                        //objBillBalAmtVO.MemberRegistered = (bool)DALHelper.HandleDBNull(reader["MemberRegistered"]);

                        //BizActionObj.FamilyDetails.Add(objFamilyVO);
                    }
                }
                reader.Close();
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return BizActionObj;
        }
        //End


        public override IValueObject AddPatientForPathology(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientForPathologyBizActionVO BizActionObj = valueObject as clsAddPatientForPathologyBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;

            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                clsPatientVO objPatientVO = BizActionObj.PatientDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPatientForPathology");

                dbServer.AddInParameter(command, "PatientCategoryID", DbType.Int64, objPatientVO.PatientCategoryIDForPath);
                dbServer.AddInParameter(command, "RegistrationDate", DbType.DateTime, objPatientVO.GeneralDetails.RegistrationDate);
                if (objPatientVO.GeneralDetails.LastName != null) objPatientVO.GeneralDetails.LastName = objPatientVO.GeneralDetails.LastName.Trim();
                dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.LastName));
                if (objPatientVO.GeneralDetails.FirstName != null) objPatientVO.GeneralDetails.FirstName = objPatientVO.GeneralDetails.FirstName.Trim();
                dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.FirstName));
                if (objPatientVO.GeneralDetails.MiddleName != null) objPatientVO.GeneralDetails.MiddleName = objPatientVO.GeneralDetails.MiddleName.Trim();
                dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.MiddleName));

                dbServer.AddInParameter(command, "GenderID", DbType.Int64, objPatientVO.GenderID);
                dbServer.AddInParameter(command, "DateOfBirth", DbType.DateTime, objPatientVO.GeneralDetails.DateOfBirth);
                dbServer.AddInParameter(command, "MaritalStatusID", DbType.Int64, objPatientVO.MaritalStatusID);
                if (objPatientVO.ContactNo1 != null) objPatientVO.ContactNo1 = objPatientVO.ContactNo1.Trim();
                dbServer.AddInParameter(command, "ContactNo1", DbType.String, objPatientVO.ContactNo1);
                if (objPatientVO.ContactNo2 != null) objPatientVO.ContactNo2 = objPatientVO.ContactNo2.Trim();
                dbServer.AddInParameter(command, "ContactNo2", DbType.String, objPatientVO.ContactNo2);

                if (objPatientVO.Email != null) objPatientVO.Email = objPatientVO.Email.Trim();
                dbServer.AddInParameter(command, "Email", DbType.String, Security.base64Encode(objPatientVO.Email));
                //commented by rohini
                // dbServer.AddInParameter(command, "Email", DbType.String, objPatientVO.Email);

                dbServer.AddInParameter(command, "PrefixId", DbType.Int64, objPatientVO.PrefixId);

                dbServer.AddInParameter(command, "MobileCountryCode", DbType.String, objPatientVO.MobileCountryCode);
                dbServer.AddInParameter(command, "ResiNoCountryCode", DbType.Int64, objPatientVO.ResiNoCountryCode);
                dbServer.AddInParameter(command, "ResiSTDCode", DbType.Int64, objPatientVO.ResiSTDCode);

                dbServer.AddInParameter(command, "RegType", DbType.Int64, objPatientVO.GeneralDetails.RegType);
                dbServer.AddInParameter(command, "CompanyName", DbType.String, objPatientVO.CompanyName);

                dbServer.AddInParameter(command, "Status", DbType.Boolean, objPatientVO.Status);

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objPatientVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                dbServer.AddParameter(command, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "????????????????????");
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.GeneralDetails.PatientID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);


                int intstatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.PatientDetails.GeneralDetails.PatientID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.PatientDetails.GeneralDetails.UnitId = UserVo.UserLoginInfo.UnitId;


                clsBasePatientSposorDAL objBaseDAL = clsBasePatientSposorDAL.GetInstance();
                BizActionObj.BizActionVOSaveSponsor.PatientSponsorDetails.PatientId = BizActionObj.PatientDetails.GeneralDetails.PatientID;
                BizActionObj.BizActionVOSaveSponsor.PatientSponsorDetails.PatientUnitId = BizActionObj.PatientDetails.GeneralDetails.UnitId;

                BizActionObj.BizActionVOSaveSponsor = (clsAddPatientSponsorBizActionVO)objBaseDAL.AddPatientSponsorDetailsWithTransaction(BizActionObj.BizActionVOSaveSponsor, UserVo, con, trans);
                if (BizActionObj.BizActionVOSaveSponsor.SuccessStatus == -1) throw new Exception();

                if (BizActionObj.PatientDetails.IsVisitForPatho == false)
                {
                    //clsBaseVisitDAL objBaseDAL2 = clsBaseVisitDAL.GetInstance();
                    //BizActionObj.BizActionVOSaveVisit.VisitDetails.PatientId = BizActionObj.PatientDetails.GeneralDetails.PatientID;
                    //BizActionObj.BizActionVOSaveVisit.VisitDetails.PatientUnitId = BizActionObj.PatientDetails.GeneralDetails.UnitId;
                    // obj = (clsAddVisitBizActionVO)objBaseDAL.AddVisit(obj, UserVo, con, trans);
                    //if (BizActionObj.BizActionVOSaveVisit.SuccessStatus == -1) throw new Exception();

                    BizActionObj.BizActionVOSaveVisit = new clsAddVisitBizActionVO();
                    BizActionObj.BizActionVOSaveVisit.VisitDetails = new clsVisitVO();
                    clsBaseVisitDAL objBaseDAL2 = clsBaseVisitDAL.GetInstance();
                    BizActionObj.BizActionVOSaveVisit.VisitDetails.PatientId = BizActionObj.PatientDetails.GeneralDetails.PatientID;
                    BizActionObj.BizActionVOSaveVisit.VisitDetails.PatientUnitId = BizActionObj.PatientDetails.GeneralDetails.UnitId;
                    BizActionObj.BizActionVOSaveVisit.VisitDetails.VisitTypeID = BizActionObj.PatientDetails.VisitTypeID;
                    BizActionObj.BizActionVOSaveVisit = (clsAddVisitBizActionVO)objBaseDAL2.AddVisitForPathology(BizActionObj.BizActionVOSaveVisit, UserVo, con, trans);
                    if (BizActionObj.BizActionVOSaveVisit.SuccessStatus == -1) throw new Exception();
                    //BizActionObj.BizActionVOSaveVisit.VisitDetails.ID = BizActionObj.BizActionVOSaveVisit.VisitDetails.ID;
                    //BizActionObj.BizActionVOSaveVisit.VisitDetails.UnitId = UserVo.UserLoginInfo.UnitId;     


                    //clsBaseVisitDAL objBaseDAL2 = clsBaseVisitDAL.GetInstance();
                    //clsAddVisitBizActionVO obj = new clsAddVisitBizActionVO();
                    //obj.VisitDetails = new clsVisitVO();
                    //obj.VisitDetails.PatientId = BizActionObj.PatientDetails.GeneralDetails.PatientID;
                    //obj.VisitDetails.PatientUnitId = BizActionObj.PatientDetails.GeneralDetails.UnitId;
                    //obj.VisitDetails.VisitTypeID = BizActionObj.PatientDetails.VisitTypeID;
                    //BizActionObj.BizActionVOSaveVisit = (clsAddVisitBizActionVO)objBaseDAL2.AddVisit(BizActionObj.BizActionVOSaveVisit, UserVo, con, trans);
                    //obj.VisitDetails.VisitStatus = false; //As per discussion with girish sir and nilesh sir (25/4/2011) 
                    //obj.VisitDetails.Status = true;

                    //if (obj.SuccessStatus == -1) throw new Exception();
                    //BizActionObj.BizActionVOSaveVisit.VisitDetails.ID = obj.VisitDetails.ID;
                    //BizActionObj.BizActionVOSaveVisit.VisitDetails.UnitId = UserVo.UserLoginInfo.UnitId;              

                }


                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();

                throw;

            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }




            return valueObject;
        }

        public override IValueObject GetPatientDetailsForPathology(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientDetailsForPathologyBizActionVO BizActionObj = (clsGetPatientDetailsForPathologyBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientDetailsForPathology");
                DbDataReader reader;

                dbServer.AddInParameter(command, "MRNo", DbType.String, BizActionObj.MRNO);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.PatientDetails == null)
                        BizActionObj.PatientDetails = new clsPatientVO();
                    while (reader.Read())
                    {
                        clsPatientVO objPatientVO = BizActionObj.PatientDetails;
                        BizActionObj.PatientDetails.GeneralDetails.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizActionObj.PatientDetails.GeneralDetails.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        BizActionObj.PatientDetails.GeneralDetails.RegistrationDate = (DateTime)DALHelper.HandleDate(reader["RegistrationDate"]);
                        BizActionObj.PatientDetails.GeneralDetails.LastName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])));
                        BizActionObj.PatientDetails.GeneralDetails.FirstName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])));
                        BizActionObj.PatientDetails.GeneralDetails.MiddleName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"])));
                        BizActionObj.PatientDetails.GeneralDetails.DateOfBirth = (DateTime?)DALHelper.HandleDate(reader["DateofBirth"]);
                        BizActionObj.PatientDetails.GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]));
                        BizActionObj.PatientDetails.MaritalStatusID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MaritalStatusID"]));
                        BizActionObj.PatientDetails.PrefixId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PrefixId"]));
                        BizActionObj.PatientDetails.Email = Convert.ToString(DALHelper.HandleDBNull(reader["Email"]));
                        BizActionObj.PatientDetails.Doctor = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredDoctor"]));
                        BizActionObj.PatientDetails.MobileCountryCode = Convert.ToString(DALHelper.HandleDBNull(reader["MobileCountryCode"]));
                        BizActionObj.PatientDetails.ContactNo1 = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo1"]));
                        BizActionObj.PatientDetails.AddressLine3 = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine3"])));
                        BizActionObj.PatientDetails.PatientSponsorCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSponsorCategoryID"]));
                        BizActionObj.PatientDetails.GeneralDetails.VisitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitID"]));
                        BizActionObj.PatientDetails.GeneralDetails.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));

                    }

                    DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_GetPatientBillInformationForPathology");
                    dbServer.AddInParameter(command3, "VisitID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.VisitID);
                    dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.PatientID);
                    dbServer.AddInParameter(command3, "UnitID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.UnitId);


                    reader = (DbDataReader)dbServer.ExecuteReader(command3);

                    if (reader.HasRows)
                    {
                        BizActionObj.PatientBillDetailList = new List<clsChargeVO>();
                        while (reader.Read())
                        {
                            clsChargeVO PatientBillDetail = new clsChargeVO();
                            PatientBillDetail.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            PatientBillDetail.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                            PatientBillDetail.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);
                            PatientBillDetail.Opd_Ipd_External_Id = (long)DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"]);
                            PatientBillDetail.Opd_Ipd_External = (Int16)DALHelper.HandleDBNull(reader["Opd_Ipd_External"]);
                            PatientBillDetail.TariffServiceId = (long)DALHelper.HandleDBNull(reader["TariffServiceId"]);
                            PatientBillDetail.ServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceId"]));
                            PatientBillDetail.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                            PatientBillDetail.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                            PatientBillDetail.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
                            PatientBillDetail.ConcessionPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                            PatientBillDetail.Concession = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                            PatientBillDetail.StaffDiscountPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffDiscountPercent"]));
                            PatientBillDetail.StaffDiscountAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffDiscountAmount"]));
                            PatientBillDetail.StaffParentDiscountPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffParentDiscountPercent"]));
                            PatientBillDetail.StaffParentDiscountAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffParentDiscountAmount"]));
                            PatientBillDetail.ServiceTaxPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["ServiceTaxPercent"]));
                            PatientBillDetail.ServiceTaxAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ServiceTaxAmount"]));
                            PatientBillDetail.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"]));
                            PatientBillDetail.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                            PatientBillDetail.IsBilled = (bool)(DALHelper.HandleDBNull(reader["IsBilled"]));
                            PatientBillDetail.Discount = (double)DALHelper.HandleDBNull(reader["Discount"]);
                            PatientBillDetail.StaffFree = (double)DALHelper.HandleDBNull(reader["StaffFree"]);
                            PatientBillDetail.IsCancelled = (bool)DALHelper.HandleDBNull(reader["IsCancelled"]);
                            PatientBillDetail.CancellationRemark = (string)DALHelper.HandleDBNull(reader["CancellationRemark"]);
                            PatientBillDetail.CancelledBy = (long?)DALHelper.HandleDBNull(reader["CancelledBy"]);
                            PatientBillDetail.CancelledDate = (DateTime?)DALHelper.HandleDate(reader["CancelledDate"]);
                            PatientBillDetail.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                            PatientBillDetail.TariffServiceId = (long)DALHelper.HandleDBNull(reader["TariffServiceID"]);
                            PatientBillDetail.ServiceSpecilizationID = (long)DALHelper.HandleDBNull(reader["SpecializationId"]);
                            PatientBillDetail.RateEditable = (bool)DALHelper.HandleDBNull(reader["RateEditable"]);

                            //  PatientBillDetail.MaxRate = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["MaxRate"]));
                            //  PatientBillDetail.MinRate = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["MinRate"]));

                            ///////////////////////// NEW CODE BY YOGESH K///////////////////////////////////////////////////////////////
                            PatientBillDetail.MaxRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["MaxRate"]));
                            PatientBillDetail.MinRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["MinRate"]));
                            ///////////////////////////NEW CODE END///////////////////////////////////////////////


                            PatientBillDetail.ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                            PatientBillDetail.CompRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["CompRate"]));
                            PatientBillDetail.DiscountPerc = Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountPerc"]));
                            PatientBillDetail.DiscountAmt = Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountAmt"]));
                            PatientBillDetail.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmt"]));
                            PatientBillDetail.AuthorizationNo = Convert.ToString(DALHelper.HandleDBNull(reader["AuthorizationNo"]));
                            PatientBillDetail.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                            PatientBillDetail.IsFreezed = (bool)DALHelper.HandleDBNull(reader["IsFreezed"]);
                            BizActionObj.PatientBillDetailList.Add(PatientBillDetail);
                        }
                    }
                    reader.NextResult();
                    if (reader.HasRows)
                    {
                        BizActionObj.PatientBillInfoDetail = new clsBillVO();
                        while (reader.Read())
                        {
                            BizActionObj.PatientBillInfoDetail.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            BizActionObj.PatientBillInfoDetail.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                            BizActionObj.PatientBillInfoDetail.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                            BizActionObj.PatientBillInfoDetail.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);
                            BizActionObj.PatientBillInfoDetail.IsCancelled = (bool)DALHelper.HandleDBNull(reader["IsCancelled"]);
                            BizActionObj.PatientBillInfoDetail.BillNo = (string)DALHelper.HandleDBNull(reader["BillNo"]);
                            BizActionObj.PatientBillInfoDetail.MRNO = (string)DALHelper.HandleDBNull(reader["MRNO"]);
                            BizActionObj.PatientBillInfoDetail.BillType = (BillTypes)((Int16)DALHelper.HandleDBNull(reader["BillType"]));
                            BizActionObj.PatientBillInfoDetail.Opd_Ipd_External = (Int16)DALHelper.HandleDBNull(reader["Opd_Ipd_External"]);
                            BizActionObj.PatientBillInfoDetail.Opd_Ipd_External_Id = (long)DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"]);
                            BizActionObj.PatientBillInfoDetail.Opd_Ipd_External_UnitId = (long)DALHelper.HandleDBNull(reader["Opd_Ipd_External_UnitId"]);
                            BizActionObj.PatientBillInfoDetail.TotalBillAmount = (double)DALHelper.HandleDBNull(reader["TotalBillAmount"]);
                            BizActionObj.PatientBillInfoDetail.TotalConcessionAmount = (double)DALHelper.HandleDBNull(reader["TotalConcessionAmount"]);
                            BizActionObj.PatientBillInfoDetail.NetBillAmount = (double)DALHelper.HandleDBNull(reader["NetBillAmount"]);
                            BizActionObj.PatientBillInfoDetail.Opd_Ipd_External_No = (string)DALHelper.HandleDBNull(reader["OPDNO"]);
                            BizActionObj.PatientBillInfoDetail.IsFreezed = (bool)DALHelper.HandleDBNull(reader["IsFreezed"]);
                            BizActionObj.PatientBillInfoDetail.VisitTypeID = (long)DALHelper.HandleDBNull(reader["VisitTypeID"]);
                            BizActionObj.PatientBillInfoDetail.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                            BizActionObj.PatientBillInfoDetail.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                            BizActionObj.PatientBillInfoDetail.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                            BizActionObj.PatientBillInfoDetail.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                            BizActionObj.PatientBillInfoDetail.BalanceAmountSelf = (double)DALHelper.HandleDBNull(reader["BalanceAmountSelf"]);
                            BizActionObj.PatientBillInfoDetail.SelfAmount = (double)DALHelper.HandleDBNull(reader["SelfAmount"]);
                            BizActionObj.PatientBillInfoDetail.BillPaymentType = (BillPaymentTypes)((Int16)DALHelper.HandleDBNull(reader["BillPaymentType"]));
                            BizActionObj.PatientBillInfoDetail.TotalRefund = (double)DALHelper.HandleDBNull(reader["RefundAmount"]);
                        }
                    }

                }
                reader.Close();
            }



            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return BizActionObj;
        }

        public override IValueObject AddPatientForPathology(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {
            clsAddPatientForPathologyBizActionVO BizActionObj = valueObject as clsAddPatientForPathologyBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;

            try
            {


                clsPatientVO objPatientVO = BizActionObj.PatientDetails;

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

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPatientForPathology");

                dbServer.AddInParameter(command, "PatientCategoryID", DbType.Int64, objPatientVO.PatientCategoryIDForPath);
                dbServer.AddInParameter(command, "RegistrationDate", DbType.DateTime, objPatientVO.GeneralDetails.RegistrationDate);
                if (objPatientVO.GeneralDetails.LastName != null) objPatientVO.GeneralDetails.LastName = objPatientVO.GeneralDetails.LastName.Trim();
                dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.LastName));
                if (objPatientVO.GeneralDetails.FirstName != null) objPatientVO.GeneralDetails.FirstName = objPatientVO.GeneralDetails.FirstName.Trim();
                dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.FirstName));
                if (objPatientVO.GeneralDetails.MiddleName != null) objPatientVO.GeneralDetails.MiddleName = objPatientVO.GeneralDetails.MiddleName.Trim();
                dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.MiddleName));

                dbServer.AddInParameter(command, "GenderID", DbType.Int64, objPatientVO.GenderID);
                dbServer.AddInParameter(command, "DateOfBirth", DbType.DateTime, objPatientVO.GeneralDetails.DateOfBirth);
                dbServer.AddInParameter(command, "MaritalStatusID", DbType.Int64, objPatientVO.MaritalStatusID);
                if (objPatientVO.ContactNo1 != null) objPatientVO.ContactNo1 = objPatientVO.ContactNo1.Trim();
                dbServer.AddInParameter(command, "ContactNo1", DbType.String, objPatientVO.ContactNo1);
                if (objPatientVO.ContactNo2 != null) objPatientVO.ContactNo2 = objPatientVO.ContactNo2.Trim();
                dbServer.AddInParameter(command, "ContactNo2", DbType.String, objPatientVO.ContactNo2);

                if (objPatientVO.Email != null) objPatientVO.Email = objPatientVO.Email.Trim();
                dbServer.AddInParameter(command, "Email", DbType.String, Security.base64Encode(objPatientVO.Email));
                //commented by rohini
                // dbServer.AddInParameter(command, "Email", DbType.String, objPatientVO.Email);

                dbServer.AddInParameter(command, "PrefixId", DbType.Int64, objPatientVO.PrefixId);

                dbServer.AddInParameter(command, "MobileCountryCode", DbType.String, objPatientVO.MobileCountryCode);
                dbServer.AddInParameter(command, "ResiNoCountryCode", DbType.Int64, objPatientVO.ResiNoCountryCode);
                dbServer.AddInParameter(command, "ResiSTDCode", DbType.Int64, objPatientVO.ResiSTDCode);

                dbServer.AddInParameter(command, "RegType", DbType.Int64, objPatientVO.GeneralDetails.RegType);
                dbServer.AddInParameter(command, "CompanyName", DbType.String, objPatientVO.CompanyName);

                dbServer.AddInParameter(command, "Status", DbType.Boolean, objPatientVO.Status);

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objPatientVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                dbServer.AddParameter(command, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "????????????????????");
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.GeneralDetails.PatientID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);


                int intstatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.PatientDetails.GeneralDetails.PatientID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.PatientDetails.GeneralDetails.UnitId = UserVo.UserLoginInfo.UnitId;


                clsBasePatientSposorDAL objBaseDAL = clsBasePatientSposorDAL.GetInstance();
                BizActionObj.BizActionVOSaveSponsor.PatientSponsorDetails.PatientId = BizActionObj.PatientDetails.GeneralDetails.PatientID;
                BizActionObj.BizActionVOSaveSponsor.PatientSponsorDetails.PatientUnitId = BizActionObj.PatientDetails.GeneralDetails.UnitId;

                BizActionObj.BizActionVOSaveSponsor = (clsAddPatientSponsorBizActionVO)objBaseDAL.AddPatientSponsorDetailsWithTransaction(BizActionObj.BizActionVOSaveSponsor, UserVo, con, trans);
                if (BizActionObj.BizActionVOSaveSponsor.SuccessStatus == -1) throw new Exception();

                if (BizActionObj.PatientDetails.IsVisitForPatho == false)
                {
                    BizActionObj.BizActionVOSaveVisit = new clsAddVisitBizActionVO();
                    BizActionObj.BizActionVOSaveVisit.VisitDetails = new clsVisitVO();
                    clsBaseVisitDAL objBaseDAL2 = clsBaseVisitDAL.GetInstance();
                    BizActionObj.BizActionVOSaveVisit.VisitDetails.PatientId = BizActionObj.PatientDetails.GeneralDetails.PatientID;
                    BizActionObj.BizActionVOSaveVisit.VisitDetails.PatientUnitId = BizActionObj.PatientDetails.GeneralDetails.UnitId;
                    BizActionObj.BizActionVOSaveVisit.VisitDetails.VisitTypeID = BizActionObj.PatientDetails.VisitTypeID;
                    BizActionObj.BizActionVOSaveVisit = (clsAddVisitBizActionVO)objBaseDAL2.AddVisitForPathology(BizActionObj.BizActionVOSaveVisit, UserVo, con, trans);
                    if (BizActionObj.BizActionVOSaveVisit.SuccessStatus == -1) throw new Exception();
                    //BizActionObj.BizActionVOSaveVisit.VisitDetails.ID = BizActionObj.BizActionVOSaveVisit.VisitDetails.ID;
                    //BizActionObj.BizActionVOSaveVisit.VisitDetails.UnitId = UserVo.UserLoginInfo.UnitId; 


                    //clsBaseVisitDAL objBaseDAL2 = clsBaseVisitDAL.GetInstance();
                    //clsAddVisitBizActionVO obj = new clsAddVisitBizActionVO();
                    //obj.VisitDetails = new clsVisitVO();
                    //obj.VisitDetails.PatientId = BizActionObj.PatientDetails.GeneralDetails.PatientID;
                    //obj.VisitDetails.PatientUnitId = BizActionObj.PatientDetails.GeneralDetails.UnitId;
                    //obj.VisitDetails.VisitTypeID = BizActionObj.PatientDetails.VisitTypeID;
                    //BizActionObj.BizActionVOSaveVisit = (clsAddVisitBizActionVO)objBaseDAL2.AddVisit(BizActionObj.BizActionVOSaveVisit, UserVo, con, trans);
                    //obj.VisitDetails.VisitStatus = false; //As per discussion with girish sir and nilesh sir (25/4/2011) 
                    //obj.VisitDetails.Status = true;

                    //if (obj.SuccessStatus == -1) throw new Exception();
                    //BizActionObj.BizActionVOSaveVisit.VisitDetails.ID = obj.VisitDetails.ID;
                    //BizActionObj.BizActionVOSaveVisit.VisitDetails.UnitId = UserVo.UserLoginInfo.UnitId;              
                }

                if (myConnection == null) trans.Commit();


            }
            catch (Exception ex)
            {
                //throw;
                BizActionObj.SuccessStatus = -1;
                if (myConnection == null) trans.Rollback();
            }
            finally
            {
                if (myConnection == null) con.Close();
            }


            return valueObject;
        }


        //added by neena
        public override IValueObject GetPatientMRNoList(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsAddPatientPhotoToServerBizActionVO BizActionObj = (clsAddPatientPhotoToServerBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientMRNoList");

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    //if masterlist instance is null then creates new instance
                    if (BizActionObj.PatientDetailsList == null)
                    {
                        BizActionObj.PatientDetailsList = new List<clsPatientVO>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        //Add the object value in list
                        clsPatientVO objNew = new clsPatientVO();
                        objNew.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objNew.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        objNew.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                        BizActionObj.PatientDetailsList.Add(objNew);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
            }
            finally
            {
            }
            return BizActionObj;
        }

        public override IValueObject MovePatientPhotoToServer(IValueObject valueObject, clsUserVO UserVo)
        {
            clsMovePatientPhotoToServerBizActionVO BizActionObj = (clsMovePatientPhotoToServerBizActionVO)valueObject;
            try
            {
                foreach (var item in BizActionObj.PatientDetailsList)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_MovePatientPhotoToServer");
                    DbDataReader reader;

                    dbServer.AddInParameter(command, "ID", DbType.Int64, item.ID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, item.UnitID);
                    dbServer.AddInParameter(command, "MRNo", DbType.String, item.MRNo);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);


                    reader = (DbDataReader)dbServer.ExecuteReader(command);
                    //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                    if (reader.HasRows)
                    {
                        if (BizActionObj.PatientDetails == null)
                            BizActionObj.PatientDetails = new clsPatientVO();
                        while (reader.Read())
                        {
                            item.Photo = (Byte[])DALHelper.HandleDBNull(reader["Photo"]);
                            item.ImageName = (string)DALHelper.HandleDBNull(reader["ImagePath"]);

                            DemoImage ObjDM = new DemoImage();
                            ObjDM.VaryQualityLevel(item.Photo, item.ImageName, ImgSaveLocation);


                            //File.WriteAllBytes(@"D:\Palash\PALASH_PATIENTREGISTRATION\IMAGE\" + item.ImageName, item.Photo);
                        }
                    }
                    reader.Close();
                }

            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return BizActionObj;
        }


        public override IValueObject AddDonarCode(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddDonorCodeBizActionVO BizActionObj = (clsAddDonorCodeBizActionVO)valueObject;

            DbTransaction trans = null;
            DbConnection con = dbServer.CreateConnection();

            try
            {
                clsPatientVO objPatientVO = BizActionObj.PatientDetails;

                con.Open();
                trans = con.BeginTransaction();


                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddDonarCodeToPatient");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objPatientVO.GeneralDetails.PatientID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objPatientVO.GeneralDetails.UnitId);
                dbServer.AddInParameter(command, "MRNo", DbType.String, objPatientVO.GeneralDetails.MRNo);
                dbServer.AddInParameter(command, "DonarCode", DbType.String, objPatientVO.GeneralDetails.DonarCode);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.SuccessStatus);


                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                trans.Commit();


            }
            catch (Exception ex)
            {

                trans.Rollback();
                throw ex;
            }
            finally
            {
                trans.Dispose();
                trans = null;
            }
            return BizActionObj;

        }

        //

        public override IValueObject GetOTPatientPackageList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPackageListBizActionVO BizActionObj = valueObject as clsGetPatientPackageListBizActionVO;
            try
            {
                # region SP for OPD

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientWithPackageListForSearch");
                DbDataReader reader;

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");

                if (BizActionObj.MRNo != null && BizActionObj.MRNo.Length != 0)
                    dbServer.AddInParameter(command, "MrNo", DbType.String, "%" + BizActionObj.MRNo + "%");
                //else
                //    dbServer.AddInParameter(command, "MrNo", DbType.String, BizActionObj.MRNo + "%");

                //if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                //    dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(BizActionObj.FirstName) + "%");
                //if (BizActionObj.MiddleName != null && BizActionObj.MiddleName.Length != 0)
                //    dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(BizActionObj.MiddleName) + "%");
                //if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                //    dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(BizActionObj.LastName) + "%");
                if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                    dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.FirstName + "%");
                if (BizActionObj.AddressLine1 != null && BizActionObj.AddressLine1.Length != 0)
                    dbServer.AddInParameter(command, "AddressLine1", DbType.String, Security.base64Encode(BizActionObj.AddressLine1) + "%");
                if (BizActionObj.MiddleName != null && BizActionObj.MiddleName.Length != 0)
                    dbServer.AddInParameter(command, "MiddleName", DbType.String, BizActionObj.MiddleName + "%");
                if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.LastName + "%");
                if (BizActionObj.FamilyName != null && BizActionObj.FamilyName.Length != 0)

                    if (BizActionObj.FamilyName != null && BizActionObj.FamilyName.Length != 0)
                        dbServer.AddInParameter(command, "FamilyName", DbType.String, BizActionObj.FamilyName + "%");
                if (BizActionObj.OPDNo != null && BizActionObj.OPDNo.Length != 0)
                {
                    if (BizActionObj.VisitWise == true)
                        dbServer.AddInParameter(command, "OPDNo", DbType.String, BizActionObj.OPDNo + "%");
                    else if (BizActionObj.AdmissionWise == true)
                        dbServer.AddInParameter(command, "AdmissionNo", DbType.String, BizActionObj.OPDNo + "%");
                    else if (BizActionObj.isfromMaterialConsumpation == true)  //Added by AJ Date 9/1/2017
                        dbServer.AddInParameter(command, "OPDNo", DbType.String, BizActionObj.OPDNo + "%");
                }

                if (BizActionObj.ContactNo != null && BizActionObj.ContactNo.Length != 0)
                    dbServer.AddInParameter(command, "ContactNo", DbType.String, BizActionObj.ContactNo + "%");
                //added by neena
                if (BizActionObj.DonarCode != null && BizActionObj.DonarCode.Length != 0)
                    dbServer.AddInParameter(command, "DonarCode", DbType.String, BizActionObj.DonarCode + "%");
                if (BizActionObj.OldRegistrationNo != null && BizActionObj.OldRegistrationNo.Length != 0)
                    dbServer.AddInParameter(command, "OldRegistrationNo", DbType.String, BizActionObj.OldRegistrationNo + "%");
                //

                if (BizActionObj.Country != null && BizActionObj.Country.Length != 0)
                    dbServer.AddInParameter(command, "Country", DbType.String, Security.base64Encode(BizActionObj.Country) + "%");
                if (BizActionObj.State != null && BizActionObj.State.Length != 0)
                    dbServer.AddInParameter(command, "State", DbType.String, Security.base64Encode(BizActionObj.State) + "%");
                if (BizActionObj.City != null && BizActionObj.City.Length != 0)
                    dbServer.AddInParameter(command, "City", DbType.String, Security.base64Encode(BizActionObj.City) + "%");
                if (BizActionObj.Pincode != null && BizActionObj.Pincode.Length != 0)
                    dbServer.AddInParameter(command, "Pincode", DbType.String, Security.base64Encode(BizActionObj.Pincode) + "%");
                if (BizActionObj.CivilID != null && BizActionObj.CivilID.Length != 0)
                    dbServer.AddInParameter(command, "CivilID", DbType.String, Security.base64Encode(BizActionObj.CivilID) + "%");
                if (BizActionObj.ReferenceNo != null && BizActionObj.ReferenceNo.Length != 0)
                    dbServer.AddInParameter(command, "ReferenceNo", DbType.String, BizActionObj.ReferenceNo);
                if (BizActionObj.GenderID != null && BizActionObj.GenderID != 0)
                    dbServer.AddInParameter(command, "GenderID", DbType.Int64, BizActionObj.GenderID);
                if (BizActionObj.PatientCategoryID != null && BizActionObj.PatientCategoryID != 0)
                    dbServer.AddInParameter(command, "PatientCategory", DbType.Int64, BizActionObj.PatientCategoryID);

                if (BizActionObj.DOB != null)
                    dbServer.AddInParameter(command, "DOB", DbType.DateTime, BizActionObj.DOB);
                else
                    dbServer.AddInParameter(command, "DOB", DbType.DateTime, null);

                if (BizActionObj.FromDate != null)
                {
                    BizActionObj.FromDate = BizActionObj.FromDate.Value.Date;
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                }

                if (BizActionObj.ToDate != null)
                {
                    if (BizActionObj.FromDate != null)
                    {
                        //if (BizActionObj.FromDate.Equals(BizActionObj.ToDate))
                        BizActionObj.ToDate = BizActionObj.ToDate.Value.Date.AddDays(1);
                    }

                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                }

                if (BizActionObj.VisitWise == true)
                {
                    dbServer.AddInParameter(command, "Serchfor", DbType.Int16, (Int16)1);

                    if (BizActionObj.VisitFromDate != null)
                    {
                        BizActionObj.VisitFromDate = BizActionObj.VisitFromDate.Value.Date;
                        dbServer.AddInParameter(command, "VisitFromDate", DbType.DateTime, BizActionObj.VisitFromDate);
                    }
                    if (BizActionObj.VisitToDate != null)
                    {
                        if (BizActionObj.VisitFromDate != null)
                        {
                            BizActionObj.VisitToDate = BizActionObj.VisitToDate.Value.Date.AddDays(1);
                        }
                        dbServer.AddInParameter(command, "VisitToDate", DbType.DateTime, BizActionObj.VisitToDate);
                    }
                }

                if (BizActionObj.AdmissionWise == true)
                {
                    dbServer.AddInParameter(command, "Serchfor", DbType.Int16, (Int16)2);

                    if (BizActionObj.AdmissionFromDate != null)
                        dbServer.AddInParameter(command, "AdmissionFromDate", DbType.DateTime, BizActionObj.AdmissionFromDate);
                    if (BizActionObj.AdmissionToDate != null)
                    {
                        if (BizActionObj.AdmissionToDate != null)
                        {
                            BizActionObj.AdmissionToDate = BizActionObj.AdmissionToDate.Value.Date.AddDays(1);
                        }
                        dbServer.AddInParameter(command, "AdmissionToDate", DbType.DateTime, BizActionObj.AdmissionToDate);
                    }
                }
                //=====================================================
                if (BizActionObj.DOBWise == true)
                {
                    dbServer.AddInParameter(command, "Serchfor", DbType.Int16, (Int16)0);

                    if (BizActionObj.DOBFromDate != null)
                        dbServer.AddInParameter(command, "DOBFromDate", DbType.DateTime, BizActionObj.DOBFromDate);
                    if (BizActionObj.DOBToDate != null)
                    {
                        if (BizActionObj.DOBToDate != null)
                        {
                            BizActionObj.DOBToDate = BizActionObj.DOBToDate.Value.Date.AddDays(1);
                        }
                        dbServer.AddInParameter(command, "DOBToDate", DbType.DateTime, BizActionObj.DOBToDate);
                    }
                }
                //=====================================================

                if (BizActionObj.IsLoyaltyMember == true)
                {
                    dbServer.AddInParameter(command, "IsLoyaltyMember", DbType.Boolean, BizActionObj.IsLoyaltyMember);
                    dbServer.AddInParameter(command, "LoyaltyProgramID", DbType.Int64, BizActionObj.LoyaltyProgramID);
                }
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "QueueUnitID", DbType.Int64, BizActionObj.PQueueUnitID);
                dbServer.AddInParameter(command, "RegistrationTypeID", DbType.Int64, BizActionObj.RegistrationTypeID);

                if (BizActionObj.PQueueUnitID != null && BizActionObj.PQueueUnitID > 0)
                {
                    dbServer.AddInParameter(command, "FindPatientVisitUnitID", DbType.Int64, BizActionObj.PQueueUnitID);
                }
                else
                {
                    dbServer.AddInParameter(command, "FindPatientVisitUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                }

                if (BizActionObj.SearchInAnotherClinic == true)
                {
                    dbServer.AddInParameter(command, "SearchInAnotherClinic", DbType.Boolean, BizActionObj.SearchInAnotherClinic);
                }

                //dbServer.AddInParameter(command, "IncludeSpouse", DbType.Boolean, BizActionObj.IncludeSpouse);
                //By Anjali.....................
                dbServer.AddInParameter(command, "IdentityID", DbType.Int64, BizActionObj.IdentityID);
                dbServer.AddInParameter(command, "IdentityNumber", DbType.String, BizActionObj.IdentityNumber);
                dbServer.AddInParameter(command, "SpecialRegID", DbType.Int64, BizActionObj.SpecialRegID);

                dbServer.AddInParameter(command, "IsFromQueeManagment", DbType.Boolean, BizActionObj.ISFromQueeManagment);
                dbServer.AddInParameter(command, "ShowOutSourceDonor", DbType.Boolean, BizActionObj.ShowOutSourceDonor);
                dbServer.AddInParameter(command, "IsFromSurrogacyModule", DbType.Boolean, BizActionObj.IsFromSurrogacyModule);
                dbServer.AddInParameter(command, "IsSelfAndDonor", DbType.Int32, BizActionObj.IsSelfAndDonor);
                //................................
                //Added by AJ Date 29/12/2016
                dbServer.AddInParameter(command, "isfromMaterialConsumpation", DbType.Boolean, BizActionObj.isfromMaterialConsumpation);
                //---------------------------

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "LinkServer", DbType.String, BizActionObj.LinkServer);
                if (BizActionObj.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, BizActionObj.LinkServer.Replace(@"\", "_"));


                dbServer.AddInParameter(command, "PackageID", DbType.Int32, BizActionObj.PackageID);


                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.PatientDetailsList == null)
                        BizActionObj.PatientDetailsList = new List<clsPatientGeneralVO>();
                    while (reader.Read())
                    {
                        clsPatientGeneralVO objPatientVO = new clsPatientGeneralVO();

                        objPatientVO.PatientID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objPatientVO.PatientUnitID = (long)DALHelper.HandleDBNull(reader["UnitId"]);
                        objPatientVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitId"]);
                        objPatientVO.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                        objPatientVO.Package = (string)DALHelper.HandleDBNull(reader["Package"]);
                        objPatientVO.PackageID = (long)DALHelper.HandleDBNull(reader["PackageID"]);
                        objPatientVO.VisitID = (long)DALHelper.HandleDBNull(reader["VAID"]);
                        objPatientVO.BillID = (long)DALHelper.HandleDBNull(reader["BillID"]);
                        objPatientVO.BillUnitID = (long)DALHelper.HandleDBNull(reader["BillUnitID"]);
                        objPatientVO.BillNo = (string)DALHelper.HandleDBNull(reader["BillNo"]);
                        objPatientVO.BillDate = (DateTime)DALHelper.HandleDBNull(reader["BillDate"]);

                        //objPatientVO.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                        //objPatientVO.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                        //objPatientVO.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));

                        //Added by AJ Date 27/12/2016
                        if (BizActionObj.isfromMaterialConsumpation == true)
                        {
                            objPatientVO.PatientID = (long)DALHelper.HandleDBNull(reader["PatientId"]);
                            objPatientVO.PatientUnitID = (long)DALHelper.HandleDBNull(reader["PatientUnitID"]);
                            objPatientVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitId"]);
                            objPatientVO.Opd_Ipd_External_Id = (long)DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"]);
                            objPatientVO.Opd_Ipd_External_UnitId = (long)DALHelper.HandleDBNull(reader["Opd_Ipd_External_UnitId"]);
                            objPatientVO.Opd_Ipd_External = Convert.ToInt32(DALHelper.HandleDBNull(reader["Opd_Ipd_External"]));
                            objPatientVO.OPDNO = Convert.ToString(DALHelper.HandleDBNull(reader["OPD_IPD_NO"])); //date 9/1/2017
                        }

                        //***/---------------------

                        objPatientVO.LastName = (string)DALHelper.HandleDBNull(reader["LastName"]);
                        objPatientVO.FirstName = (string)DALHelper.HandleDBNull(reader["FirstName"]);
                        objPatientVO.MiddleName = (string)DALHelper.HandleDBNull(reader["MiddleName"]);
                        objPatientVO.AddressLine1 = (String)DALHelper.HandleDBNull(reader["AddressLine1"]);
                        objPatientVO.RegType = Convert.ToInt16(DALHelper.HandleDBNull(reader["RegType"]));
                        objPatientVO.DateOfBirth = (DateTime?)DALHelper.HandleDate(reader["DateofBirth"]);
                        objPatientVO.Age = Convert.ToInt32(DALHelper.HandleDBNull(reader["Age"]));
                        objPatientVO.RegistrationDate = (DateTime)DALHelper.HandleDate(reader["RegistrationDate"]);
                        if (BizActionObj.VisitWise == true || BizActionObj.RegistrationWise == true)
                        {
                            objPatientVO.VisitID = (long)DALHelper.HandleDBNull(reader["VAID"]);
                            objPatientVO.VisitUnitID = (long)DALHelper.HandleDBNull(reader["VisitUnitID"]);
                            objPatientVO.VisitUnitId = (long)DALHelper.HandleDBNull(reader["VisitUnitID"]);
                            objPatientVO.OPDNO = (string)DALHelper.HandleDBNull(reader["OINO"]);
                            objPatientVO.Unit = Convert.ToString(reader["UnitName"]);
                        }
                        else
                        {
                            //objPatientVO.IPDAdmissionID = (long)DALHelper.HandleDBNull(reader["VAID"]);
                            //objPatientVO.IPDAdmissionNo = (string)DALHelper.HandleDBNull(reader["OINO"]);
                        }

                        if (BizActionObj.VisitWise == true)
                            objPatientVO.PatientKind = PatientsKind.OPD;
                        else if (BizActionObj.AdmissionWise == true)
                            objPatientVO.PatientKind = PatientsKind.IPD;
                        else if (objPatientVO.VisitID == 0 && objPatientVO.IPDAdmissionID == 0)
                            objPatientVO.PatientKind = PatientsKind.Registration;

                        objPatientVO.TariffID = (long)DALHelper.HandleDBNull(reader["TariffID"]);
                        objPatientVO.PatientSourceID = (long)DALHelper.HandleDBNull(reader["PatientSourceID"]);
                        objPatientVO.CompanyID = (long)DALHelper.HandleDBNull(reader["CompanyID"]);
                        objPatientVO.ContactNO1 = (string)DALHelper.HandleDBNull(reader["ContactNO1"]);
                        objPatientVO.Gender = (string)DALHelper.HandleDBNull(reader["Gender"]);
                        objPatientVO.MaritalStatus = (string)DALHelper.HandleDBNull(reader["MaritalStatus"]);
                        objPatientVO.UniversalID = Security.base64Decode((string)DALHelper.HandleDBNull(reader["CivilID"]));
                        objPatientVO.PatientTypeID = (long)DALHelper.HandleDBNull(reader["PatientCategoryID"]);
                        // Added By CDS 
                        objPatientVO.GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]));
                        objPatientVO.NewPatientCategoryID = (long)DALHelper.HandleDBNull(reader["NewPatientCategoryID"]);
                        // Added By CDS 
                        objPatientVO.ReferenceNo = (string)DALHelper.HandleDBNull(reader["ReferenceNo"]);
                        //objPatientVO.RegistrationType = (string)DALHelper.HandleDBNull(reader["RegistrationType"]);       // ..................BHUSHAN
                        //objPatientVO.SpouseID = (long)DALHelper.HandleDBNull(reader["SpouseID"]);
                        objPatientVO.City = (string)DALHelper.HandleDBNull(reader["CityNew"]);
                        objPatientVO.DonorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DonorCode"]));
                        objPatientVO.IsSurrogateAlreadyLinked = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSurrogateAlreadyLinked"]));

                        //added by neena
                        objPatientVO.DonarCode = Convert.ToString(DALHelper.HandleDBNull(reader["DonarCode"]));
                        objPatientVO.OldRegistrationNo = Convert.ToString(DALHelper.HandleDBNull(reader["OldRegistrationNo"]));
                        //
                        // added by Ashish Z. on dated 230716
                        objPatientVO.IsReferralDoc = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsReferralDoc"]));
                        objPatientVO.ReferralTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferralTypeID"]));
                        objPatientVO.ReferralDoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferralDoctorID"]));
                        objPatientVO.IsAge = (bool)DALHelper.HandleBoolDBNull(reader["IsAge"]);
                        if (objPatientVO.IsAge == true)
                        {
                            objPatientVO.DateOfBirthFromAge = (DateTime?)DALHelper.HandleDate(reader["DateOfBirth"]);
                        }
                        else
                        {
                            objPatientVO.DateOfBirth = (DateTime?)DALHelper.HandleDate(reader["DateOfBirth"]);
                        }

                        objPatientVO.EducationID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["EducationID"]));
                        objPatientVO.BloodGroupID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["BloodGroupID"]));
                        string ImgPath = Convert.ToString(DALHelper.HandleDBNull(reader["ImagePath"]));

                        if (!string.IsNullOrEmpty(ImgPath))
                            objPatientVO.ImageName = "https://" + ImgIP + "/" + ImgVirtualDir + "/" + ImgPath;
                        objPatientVO.IsDocAttached = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDocAttached"]));
                        objPatientVO.Email = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["Email"])));
                        objPatientVO.IdentityType = Convert.ToString(DALHelper.HandleDBNull(reader["IdentityType"]));
                        objPatientVO.IdentityNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IdentityNumber"]));
                        objPatientVO.RemarkForPatientType = Convert.ToString(DALHelper.HandleDBNull(reader["RemarkForPatientType"]));
                        objPatientVO.SpecialRegName = Convert.ToString(DALHelper.HandleDBNull(reader["SpecialReg"]));
                        if (objPatientVO.RegType == 0)
                            objPatientVO.RegistrationType = "OPD";
                        else if (objPatientVO.RegType == 1)
                            objPatientVO.RegistrationType = "IPD";
                        else if (objPatientVO.RegType == 2)
                            objPatientVO.RegistrationType = "Pharmacy";
                        else if (objPatientVO.RegType == 5)
                            objPatientVO.RegistrationType = "Pathology";

                        //Added by AJ Date 27/12/2016
                        if (BizActionObj.isfromMaterialConsumpation == false)
                        {
                            objPatientVO.PanNumber = Convert.ToString(DALHelper.HandleDBNull(reader["PanNumber"]));  //* Added by - Ajit Jadhav  Date - 25/8/2016
                            objPatientVO.Email = Convert.ToString(DALHelper.HandleDBNull(reader["Email"]));
                            objPatientVO.NationalityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["NationalityId"])); // Added by ajit jadhav Date 13/10/2016
                        }
                        //***//---------------------                         

                        objPatientVO.LinkServer = BizActionObj.LinkServer;

                        if (BizActionObj.ISFromQueeManagment == false && BizActionObj.isfromMaterialConsumpation == false)
                            objPatientVO.BabyWeight = Convert.ToString(DALHelper.HandleDBNull(reader["BabyWeight"]));       // For Pediatric Flow

                        BizActionObj.PatientDetailsList.Add(objPatientVO);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");

                reader.Close();

                #endregion






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
    }
}
