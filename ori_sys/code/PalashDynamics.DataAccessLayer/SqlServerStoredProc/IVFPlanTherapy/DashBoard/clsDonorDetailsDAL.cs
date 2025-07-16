using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard;
using com.seedhealthcare.hms.Web.Logging;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects.DashBoardVO;
using System.Data.Common;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using System.Data;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Patient;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IVFPlanTherapy.DashBoard
{
    class clsDonorDetailsDAL : clsBaseDonorDetailsDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsDonorDetailsDAL()
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
        DbTransaction trans = null;
        DbConnection con = null;
        public override IValueObject AddUpdateDonorDetails(IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {
           
            cls_NewAddUpdateDonorDetailsBizActionVO BizAction = valueObject as cls_NewAddUpdateDonorDetailsBizActionVO;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                clsFemaleSemenDetailsVO ObjDay1VO = BizAction.DonorDetails;
                 DbCommand command;
                 command = dbServer.GetStoredProcCommand("CIMS_IVFDAshBoard_AddDonorDetails");
                 if (BizAction.DonorDetails.ID > 0)
                 {
                     dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.DonorDetails.ID);
                 }
                 else
                 {
                     dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                 }            
        
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "IsEdit", DbType.Boolean, BizAction.IsEdit);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.DonorDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.DonorDetails.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.DonorDetails.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.DonorDetails.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "MethodOfSpermPreparation", DbType.Int64, BizAction.DonorDetails.MethodOfSpermPreparation);
                dbServer.AddInParameter(command, "MOSP", DbType.Int64, BizAction.DonorDetails.MOSP);
                dbServer.AddInParameter(command, "Company", DbType.String, BizAction.DonorDetails.Company);
                dbServer.AddInParameter(command, "SourceOfNeedle", DbType.Int64, BizAction.DonorDetails.SourceOfNeedle_1);
                dbServer.AddInParameter(command, "OoctyDonorUnitID", DbType.Int64, BizAction.DonorDetails.OoctyDonorUnitID);
                dbServer.AddInParameter(command, "OoctyDonorID", DbType.Int64, BizAction.DonorDetails.OoctyDonorID);
                dbServer.AddInParameter(command, "OoctyDonorMrNo", DbType.String, BizAction.DonorDetails.OoctyDonorMrNo);
                dbServer.AddInParameter(command, "SemenDonorMrNo", DbType.String, BizAction.DonorDetails.SemenDonorMrNo);
                dbServer.AddInParameter(command, "SourceOfOoctye", DbType.Int64, BizAction.DonorDetails.SourceOfOoctye_1);
                dbServer.AddInParameter(command, "SourceOfSemen", DbType.Int64, BizAction.DonorDetails.SourceOfSemen_new);

                dbServer.AddInParameter(command, "SemenDonorID", DbType.Int64, BizAction.DonorDetails.SemenDonorID);
                dbServer.AddInParameter(command, "SemenDonorUnitID", DbType.Int64, BizAction.DonorDetails.SemenDonorUnitID);
                dbServer.AddInParameter(command, "PreSelfVolume", DbType.Int64, BizAction.DonorDetails.PreSelfVolume_1);
                dbServer.AddInParameter(command, "PreSelfConcentration", DbType.Int64, BizAction.DonorDetails.PreSelfConcentration_1);
                dbServer.AddInParameter(command, "PreSelfMotality", DbType.Int64, BizAction.DonorDetails.PreSelfMotality_1);
                dbServer.AddInParameter(command, "PreSelfWBC", DbType.Int64, BizAction.DonorDetails.PreSelfWBC_1);
                dbServer.AddInParameter(command, "PreDonorVolume", DbType.Int64, BizAction.DonorDetails.PreDonorVolume_1);
                dbServer.AddInParameter(command, "PreDonorConcentration", DbType.Int64, BizAction.DonorDetails.PreDonorConcentration_1);

                dbServer.AddInParameter(command, "PreDonorMotality", DbType.Int64, BizAction.DonorDetails.PreDonorMotality_1);
                dbServer.AddInParameter(command, "PreDonorWBC", DbType.Int64, BizAction.DonorDetails.PreDonorWBC_1);
                dbServer.AddInParameter(command, "PostSelfVolume", DbType.Int64, BizAction.DonorDetails.PostSelfVolume_1);
                dbServer.AddInParameter(command, "PostSelfConcentration", DbType.Int64, BizAction.DonorDetails.PostSelfConcentration_1);
                dbServer.AddInParameter(command, "PostSelfMotality", DbType.Int64, BizAction.DonorDetails.PostSelfMotality_1);
                dbServer.AddInParameter(command, "PostSelfWBC", DbType.Int64, BizAction.DonorDetails.PostSelfWBC_1);

                dbServer.AddInParameter(command, "PostDonorVolume", DbType.Int64, BizAction.DonorDetails.PostDonorVolume_1);
                dbServer.AddInParameter(command, "PostDonorConcentration", DbType.Int64, BizAction.DonorDetails.PostDonorConcentration_1);
                dbServer.AddInParameter(command, "PostDonorMotality", DbType.Int64, BizAction.DonorDetails.PostDonorMotality_1);
                dbServer.AddInParameter(command, "PostDonorWBC", DbType.Int64, BizAction.DonorDetails.PostDonorWBC_1);

                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDay1VO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddInParameter(command, "SemenSampleCode", DbType.String, BizAction.DonorDetails.SemenSampleCode);
                dbServer.AddInParameter(command, "SemenSampleCodeSelf", DbType.String, BizAction.DonorDetails.SemenSampleCodeSelf);
                dbServer.AddInParameter(command, "IsDonorFromModuleDonor", DbType.Boolean, BizAction.DonorDetails.IsDonorFromModuleDonor);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizAction.DonorDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

                //By Anjali..........................................
                DbCommand command1;
                command1 = dbServer.GetStoredProcCommand("CIMS_IVFDAshBoard_AddUpdateDonorDetailsInPlanTherapy");
                dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizAction.DonorDetails.PlanTherapyID);
                dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizAction.DonorDetails.PlanTherapyUnitID);
                if(BizAction.DonorDetails.OoctyDonorID !=null ||BizAction.DonorDetails.OoctyDonorID !=0)
                    dbServer.AddInParameter(command1, "IsOocyteDonorExists", DbType.Boolean, true);
                else
                    dbServer.AddInParameter(command1, "IsOocyteDonorExists", DbType.Boolean, false);
                if (BizAction.DonorDetails.SemenDonorID != null || BizAction.DonorDetails.SemenDonorID != 0)
                    dbServer.AddInParameter(command1, "IsSemenDonorExists", DbType.Boolean, true);
                else
                    dbServer.AddInParameter(command1, "IsSemenDonorExists", DbType.Boolean, false);
                dbServer.AddInParameter(command1, "OoctyDonorMrNo", DbType.String, BizAction.DonorDetails.OoctyDonorMrNo);
                dbServer.AddInParameter(command1, "SemenDonorMrNo", DbType.String, BizAction.DonorDetails.SemenDonorMrNo);
                int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                //........................................................



            }
            catch (Exception)
            {
                trans.Commit();
                con.Close();
               
                throw;
            }
            finally
            {
                trans.Commit();
                con.Close();
            
            }
            return BizAction;
        }

        public override IValueObject GetDonorDetails(IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {
            cls_NewGetDonorDetailsBizActionVO BizAction = (valueObject) as cls_NewGetDonorDetailsBizActionVO;
            con = dbServer.CreateConnection();
            con.Open();
            try
            {       
                DbCommand command = dbServer.GetStoredProcCommand("IVF_DashBoardGetDonorDetails");

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.DonorDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.DonorDetails.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.DonorDetails.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.DonorDetails.PlanTherapyUnitID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizAction.DonorDetails.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        BizAction.DonorDetails.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        BizAction.DonorDetails.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                        BizAction.DonorDetails.PatientUnitID = (long)DALHelper.HandleDBNull(reader["PatientUnitID"]);
                        BizAction.DonorDetails.PlanTherapyUnitID = (long)DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]);
                        BizAction.DonorDetails.PlanTherapyID = (long)DALHelper.HandleDBNull(reader["PlanTherapyID"]);
                        BizAction.DonorDetails.SemenDonorMrNo = (string)DALHelper.HandleDBNull(reader["SemenDonorMrNo"]);
                        BizAction.DonorDetails.OoctyDonorMrNo = (string)DALHelper.HandleDBNull(reader["OoctyDonorMrNo"]);
                        BizAction.DonorDetails.MethodOfSpermPreparation = (long)DALHelper.HandleDBNull(reader["MethodOfSpermPreparation"]);
                        BizAction.DonorDetails.SourceOfNeedle_1 = (long)DALHelper.HandleDBNull(reader["SourceOfNeedle"]); // string
                        BizAction.DonorDetails.OoctyDonorUnitID = (long)DALHelper.HandleDBNull(reader["OoctyDonorUnitID"]);
                        BizAction.DonorDetails.OoctyDonorID = (long)DALHelper.HandleDBNull(reader["OoctyDonorID"]);
                        BizAction.DonorDetails.SourceOfOoctye_1 = (long)DALHelper.HandleDBNull(reader["SourceOfOoctye"]);
                        BizAction.DonorDetails.SourceOfSemen_new = (long)DALHelper.HandleDBNull(reader["SourceOfSemen"]);
                        BizAction.DonorDetails.SemenDonorID = (long)DALHelper.HandleDBNull(reader["SemenDonorID"]);
                        BizAction.DonorDetails.SemenDonorUnitID = (long)DALHelper.HandleDBNull(reader["SemenDonorUnitID"]);
                        BizAction.DonorDetails.Company = (string)DALHelper.HandleDBNull(reader["Company"]);
                        BizAction.DonorDetails.SourceNeedle = (string)DALHelper.HandleDBNull(reader["SourceNeedle"]);
                        BizAction.DonorDetails.SourceOocyte = (string)DALHelper.HandleDBNull(reader["SourceOocyte"]);
                        BizAction.DonorDetails.SourceSemen = (string)DALHelper.HandleDBNull(reader["SourceSemen"]);

                        BizAction.DonorDetails.PreSelfVolume_1 = (long)DALHelper.HandleDBNull(reader["PreSelfVolume"]);
                        BizAction.DonorDetails.PreSelfConcentration_1 = (long)DALHelper.HandleDBNull(reader["PreSelfConcentration"]);
                        BizAction.DonorDetails.PreSelfMotality_1 = (long)DALHelper.HandleDBNull(reader["PreSelfMotality"]);
                        BizAction.DonorDetails.PreSelfWBC_1 = (long)DALHelper.HandleDBNull(reader["PreSelfWBC"]);
                        BizAction.DonorDetails.PreDonorVolume_1 = (long)DALHelper.HandleDBNull(reader["PreDonorVolume"]);
                        BizAction.DonorDetails.PreDonorConcentration_1 = (long)DALHelper.HandleDBNull(reader["PreDonorConcentration"]);
                        BizAction.DonorDetails.PreDonorMotality_1 = (long)DALHelper.HandleDBNull(reader["PreDonorMotality"]);
                        BizAction.DonorDetails.PreDonorWBC_1 = (long)DALHelper.HandleDBNull(reader["PreDonorWBC"]);

                        BizAction.DonorDetails.PostSelfVolume_1 = (long)DALHelper.HandleDBNull(reader["PostSelfVolume"]);
                        BizAction.DonorDetails.PostSelfConcentration_1 = (long)DALHelper.HandleDBNull(reader["PostSelfConcentration"]);
                        BizAction.DonorDetails.PostSelfMotality_1 = (long)DALHelper.HandleDBNull(reader["PostSelfMotality"]);
                        BizAction.DonorDetails.PostSelfWBC_1 = (long)DALHelper.HandleDBNull(reader["PostSelfWBC"]);
                        BizAction.DonorDetails.PostDonorVolume_1 = (long)DALHelper.HandleDBNull(reader["PostDonorVolume"]);
                        BizAction.DonorDetails.PostDonorConcentration_1 = (long)DALHelper.HandleDBNull(reader["PostDonorConcentration"]);
                        BizAction.DonorDetails.PostDonorMotality_1 = (long)DALHelper.HandleDBNull(reader["PostDonorMotality"]);
                        BizAction.DonorDetails.PostDonorWBC_1 = (long)DALHelper.HandleDBNull(reader["PostDonorWBC"]);


                        BizAction.DonorDetails.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);

                        BizAction.DonorDetails.MOSP = (string)DALHelper.HandleDBNull(reader["SourceNeedle"]);
                        BizAction.DonorDetails.SemenSampleCode = (string)DALHelper.HandleDBNull(reader["SemenSampleCode"]);
                        BizAction.DonorDetails.SemenSampleCodeSelf = (string)DALHelper.HandleDBNull(reader["SemenSampleCodeSelf"]);
                        BizAction.DonorDetails.IsDonorFromModuleDonor = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDonorFromModuleDonor"]));
                        BizAction.DonorDetails.IsSampleUsedInDay0 = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleUsedInDay0"]));


                    }
                }
                else
                {
                    BizAction.DonorDetails = null;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                con.Close(); 
                throw ex;
            }
            finally
            {
                con.Close();
            }
         
            return BizAction;
        }

        public override IValueObject AddUpdateDonorRegistrationDetails(IValueObject valueObject, ValueObjects.clsUserVO UserVo) 
        {
            clsAddUpdateDonorBizActionVO BizActionObj = valueObject as clsAddUpdateDonorBizActionVO;        //

            if (BizActionObj.DonorDetails.GeneralDetails.PatientID == 0)
                BizActionObj = AddDonorDetails(BizActionObj, UserVo);
            else
                BizActionObj = UpdateDonorDetails(BizActionObj, UserVo);

            return valueObject;
        }
        private clsAddUpdateDonorBizActionVO AddDonorDetails(clsAddUpdateDonorBizActionVO BizActionObj, clsUserVO UserVo)
        {
        
           //  DbTransaction trans = null;
           // DbConnection con = dbServer.CreateConnection();
           //// clsAddUpdateDonorBizActionVO BizActionObj= valueObject as clsAddUpdateDonorBizActionVO;
           // try
           // {
           //     clsPatientVO objPatientVO = BizActionObj.DonorDetails;

           //     con.Open();
           //     trans = con.BeginTransaction();
                
           //     DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddDonor");

           //     dbServer.AddInParameter(command, "LinkServer", DbType.String, objPatientVO.GeneralDetails.LinkServer);
           //     if (objPatientVO.GeneralDetails.LinkServer != null)
           //         dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objPatientVO.GeneralDetails.LinkServer.Replace(@"\", "_"));
           //     dbServer.AddInParameter(command, "PatientCategoryID", DbType.Int64, objPatientVO.GeneralDetails.PatientTypeID);
           //     dbServer.AddInParameter(command, "ReferralTypeID", DbType.Int64, objPatientVO.GeneralDetails.ReferralTypeID);
           //     dbServer.AddInParameter(command, "CompanyName", DbType.String, Security.base64Encode(objPatientVO.CompanyName));
           //     dbServer.AddInParameter(command, "RegistrationDate", DbType.DateTime, objPatientVO.GeneralDetails.RegistrationDate);
           //     if (objPatientVO.GeneralDetails.LastName != null) objPatientVO.GeneralDetails.LastName = objPatientVO.GeneralDetails.LastName.Trim();
           //     dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.LastName));
           //     if (objPatientVO.GeneralDetails.FirstName != null) objPatientVO.GeneralDetails.FirstName = objPatientVO.GeneralDetails.FirstName.Trim();
           //     dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.FirstName));
           //     if (objPatientVO.GeneralDetails.MiddleName != null) objPatientVO.GeneralDetails.MiddleName = objPatientVO.GeneralDetails.MiddleName.Trim();
           //     dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(objPatientVO.GeneralDetails.MiddleName));
           //     if (objPatientVO.FamilyName != null) objPatientVO.FamilyName = objPatientVO.FamilyName.Trim();
           //     dbServer.AddInParameter(command, "FamilyName", DbType.String, Security.base64Encode(objPatientVO.FamilyName));
           //     dbServer.AddInParameter(command, "GenderID", DbType.Int64, objPatientVO.GenderID);
           //     dbServer.AddInParameter(command, "DateOfBirth", DbType.DateTime, objPatientVO.GeneralDetails.DateOfBirth);
           //     dbServer.AddInParameter(command, "BloodGroupID", DbType.Int64, objPatientVO.BloodGroupID);
           //     dbServer.AddInParameter(command, "MaritalStatusID", DbType.Int64, objPatientVO.MaritalStatusID);
           //     dbServer.AddInParameter(command, "CivilID", DbType.String, Security.base64Encode(objPatientVO.CivilID));
           //     if (objPatientVO.ContactNo1 != null) objPatientVO.ContactNo1 = objPatientVO.ContactNo1.Trim();
           //     dbServer.AddInParameter(command, "ContactNo1", DbType.String, objPatientVO.ContactNo1);
           //     if (objPatientVO.ContactNo2 != null) objPatientVO.ContactNo2 = objPatientVO.ContactNo2.Trim();
           //     dbServer.AddInParameter(command, "ContactNo2", DbType.String, objPatientVO.ContactNo2);
           //     if (objPatientVO.FaxNo != null) objPatientVO.FaxNo = objPatientVO.FaxNo.Trim();
           //     dbServer.AddInParameter(command, "FaxNo", DbType.String, objPatientVO.FaxNo);
           //     if (objPatientVO.Email != null) objPatientVO.Email = objPatientVO.Email.Trim();
           //     dbServer.AddInParameter(command, "Email", DbType.String, Security.base64Encode(objPatientVO.Email));
           //     if (objPatientVO.AddressLine1 != null) objPatientVO.AddressLine1 = objPatientVO.AddressLine1.Trim();
           //     dbServer.AddInParameter(command, "AddressLine1", DbType.String, Security.base64Encode(objPatientVO.AddressLine1));
           //     if (objPatientVO.AddressLine2 != null) objPatientVO.AddressLine2 = objPatientVO.AddressLine2.Trim();
           //     dbServer.AddInParameter(command, "AddressLine2", DbType.String, Security.base64Encode(objPatientVO.AddressLine2));
           //     if (objPatientVO.AddressLine3 != null) objPatientVO.AddressLine3 = objPatientVO.AddressLine3.Trim();
           //     dbServer.AddInParameter(command, "AddressLine3", DbType.String, Security.base64Encode(objPatientVO.AddressLine3));
           //     if (objPatientVO.Country != null) objPatientVO.Country = objPatientVO.Country.Trim();
           //     dbServer.AddInParameter(command, "Country", DbType.String, null);
           //     dbServer.AddInParameter(command, "State", DbType.String, null);
           //     dbServer.AddInParameter(command, "City", DbType.String, null);
           //     dbServer.AddInParameter(command, "Taluka", DbType.String, null);
           //     dbServer.AddInParameter(command, "Area", DbType.String, null);
           //     dbServer.AddInParameter(command, "District", DbType.String, null);
           //     if (objPatientVO.CountryID > 0)
           //         dbServer.AddInParameter(command, "CountryID", DbType.Int64, objPatientVO.CountryID);
           //     if (objPatientVO.StateID > 0)
           //         dbServer.AddInParameter(command, "StateID", DbType.Int64, objPatientVO.StateID);
           //     if (objPatientVO.CityID > 0)
           //         dbServer.AddInParameter(command, "CityID", DbType.Int64, objPatientVO.CityID);
           //     if (objPatientVO.RegionID > 0)
           //         dbServer.AddInParameter(command, "RegionID", DbType.Int64, objPatientVO.RegionID);
           //     dbServer.AddInParameter(command, "MobileCountryCode", DbType.Int64, objPatientVO.MobileCountryCode);
           //     dbServer.AddInParameter(command, "ResiNoCountryCode", DbType.Int64, objPatientVO.ResiNoCountryCode);
           //     dbServer.AddInParameter(command, "ResiSTDCode", DbType.Int64, objPatientVO.ResiSTDCode);
           //     if (objPatientVO.Pincode != null) objPatientVO.Pincode = objPatientVO.Pincode.Trim();
           //     dbServer.AddInParameter(command, "Pincode", DbType.String, Security.base64Encode(objPatientVO.Pincode));
           //     dbServer.AddInParameter(command, "ReligionID", DbType.Int64, objPatientVO.ReligionID);
           //     dbServer.AddInParameter(command, "OccupationId", DbType.Int64, objPatientVO.OccupationId);
           //     dbServer.AddInParameter(command, "Photo", DbType.Binary, objPatientVO.Photo);
           //     dbServer.AddInParameter(command, "IsLoyaltyMember", DbType.Boolean, objPatientVO.IsLoyaltyMember);
           //     dbServer.AddInParameter(command, "LoyaltyCardID", DbType.Int64, objPatientVO.LoyaltyCardID);
           //     dbServer.AddInParameter(command, "RelationID", DbType.Int64, objPatientVO.RelationID);
           //     dbServer.AddInParameter(command, "ParentPatientID", DbType.Int64, objPatientVO.ParentPatientID);
           //     dbServer.AddInParameter(command, "IssueDate", DbType.DateTime, objPatientVO.IssueDate);
           //     dbServer.AddInParameter(command, "EffectiveDate", DbType.DateTime, objPatientVO.EffectiveDate);
           //     dbServer.AddInParameter(command, "ExpiryDate", DbType.DateTime, objPatientVO.ExpiryDate);
           //     if (objPatientVO.LoyaltyCardNo != null) objPatientVO.LoyaltyCardNo = objPatientVO.LoyaltyCardNo.Trim();
           //     dbServer.AddInParameter(command, "LoyaltyCardNo", DbType.String, objPatientVO.LoyaltyCardNo);
           //     dbServer.AddInParameter(command, "Status", DbType.Boolean, objPatientVO.Status);
           //     dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
           //     dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
           //     dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
           //     dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
           //     dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objPatientVO.AddedDateTime);
           //     dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
           //     dbServer.AddParameter(command, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
           //     dbServer.AddParameter(command, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default,"????????????????????");
           //     dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.GeneralDetails.PatientID);
           //     dbServer.AddInParameter(command, "IsDonor", DbType.Boolean, objPatientVO.IsDonor);
           //     dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

           //     int intStatus = dbServer.ExecuteNonQuery(command, trans);
           //     BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
           //     BizActionObj.DonorDetails.GeneralDetails.PatientID = (long)dbServer.GetParameterValue(command, "ID");
           //     BizActionObj.DonorDetails.GeneralDetails.MRNo = (string)dbServer.GetParameterValue(command, "MRNo");
           //     string Err = (string)dbServer.GetParameterValue(command, "Err");
           //     BizActionObj.DonorDetails.GeneralDetails.UnitId = UserVo.UserLoginInfo.UnitId;

               
           //     DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddDonorOtherDetails");
           //     dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
           //     dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
           //     dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.DonorDetails.GeneralDetails.PatientID);
           //     dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
           //     dbServer.AddInParameter(command1, "SkinColorID", DbType.Int64, BizActionObj.DonorDetails.SkinColorID);
           //     dbServer.AddInParameter(command1, "HairColorID", DbType.Int64, BizActionObj.DonorDetails.HairColorID);
           //     dbServer.AddInParameter(command1, "EyeColorID", DbType.Int64, BizActionObj.DonorDetails.EyeColorID);
           //     dbServer.AddInParameter(command1, "DonorSourceID", DbType.Int64, BizActionObj.DonorDetails.DonorSourceID);
           //     dbServer.AddInParameter(command1, "DonorCode", DbType.String, BizActionObj.DonorDetails.DonorCode);
           //     dbServer.AddInParameter(command1, "Height", DbType.Double, BizActionObj.DonorDetails.Height);
           //     dbServer.AddInParameter(command1, "BoneStructure", DbType.String, BizActionObj.DonorDetails.BoneStructure);
           //     int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

           //     if (BizActionObj.BatchDetails.BatchCode != null)
           //     {
           //         DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddDonorBatchDetails");
           //         dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.BatchDetails.ID);
           //         dbServer.AddInParameter(command2, "UnitID", DbType.Int64, BizActionObj.BatchDetails.UnitID);
           //         dbServer.AddInParameter(command2, "DonorID", DbType.Int64, BizActionObj.DonorDetails.GeneralDetails.PatientID);
           //         dbServer.AddInParameter(command2, "DonorUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
           //         dbServer.AddInParameter(command2, "BatchCode", DbType.String, BizActionObj.BatchDetails.BatchCode);
           //         dbServer.AddInParameter(command2, "InvoiceNo", DbType.String, BizActionObj.BatchDetails.InvoiceNo);
           //         dbServer.AddInParameter(command2, "ReceivedByID", DbType.Int64, BizActionObj.BatchDetails.ReceivedByID);
           //         dbServer.AddInParameter(command2, "ReceivedDate", DbType.DateTime, BizActionObj.BatchDetails.ReceivedDate);
           //         dbServer.AddInParameter(command2, "LabID", DbType.Int64, BizActionObj.BatchDetails.LabID);
           //         dbServer.AddInParameter(command2, "NoOfVails", DbType.Int32, BizActionObj.BatchDetails.NoOfVails);
           //         dbServer.AddInParameter(command2, "Remark", DbType.String, BizActionObj.BatchDetails.Remark);
           //         dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
           //         dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
           //         dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
           //         dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, objPatientVO.AddedDateTime);
           //         dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
           //         int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
           //         BizActionObj.BatchDetails.ID = (long)dbServer.GetParameterValue(command2, "ID");
           //         BizActionObj.BatchDetails.UnitID = UserVo.UserLoginInfo.UnitId;
           //     }

           // }
           // catch (Exception)
           // {
           //     trans.Rollback();
           //     trans.Commit();
           //     con.Close();
           //     throw;
           // }
           // finally
           // {
           //     trans.Commit();
           //     con.Close();

           // }
           // return BizActionObj;


            DbTransaction trans = null;
            DbConnection con = dbServer.CreateConnection();
            // clsAddUpdateDonorBizActionVO BizActionObj= valueObject as clsAddUpdateDonorBizActionVO;
            try
            {
                clsPatientVO objPatientVO = BizActionObj.DonorDetails;

                con.Open();
                trans = con.BeginTransaction();

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddDonor");

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
                dbServer.AddInParameter(command, "MobileCountryCode", DbType.Int64, objPatientVO.MobileCountryCode);
                dbServer.AddInParameter(command, "ResiNoCountryCode", DbType.Int64, objPatientVO.ResiNoCountryCode);
                dbServer.AddInParameter(command, "ResiSTDCode", DbType.Int64, objPatientVO.ResiSTDCode);
                if (objPatientVO.Pincode != null) objPatientVO.Pincode = objPatientVO.Pincode.Trim();
                dbServer.AddInParameter(command, "Pincode", DbType.String, Security.base64Encode(objPatientVO.Pincode));
                dbServer.AddInParameter(command, "ReligionID", DbType.Int64, objPatientVO.ReligionID);
                dbServer.AddInParameter(command, "OccupationId", DbType.Int64, objPatientVO.OccupationId);
                dbServer.AddInParameter(command, "Photo", DbType.Binary, objPatientVO.Photo);
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
                dbServer.AddParameter(command, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "????????????????????");
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.GeneralDetails.PatientID);
                dbServer.AddInParameter(command, "IsDonor", DbType.Boolean, objPatientVO.IsDonor);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.DonorDetails.GeneralDetails.PatientID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.DonorDetails.GeneralDetails.MRNo = (string)dbServer.GetParameterValue(command, "MRNo");
                string Err = (string)dbServer.GetParameterValue(command, "Err");
                BizActionObj.DonorDetails.GeneralDetails.UnitId = UserVo.UserLoginInfo.UnitId;


                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddDonorOtherDetails");
                dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.DonorDetails.GeneralDetails.PatientID);
                dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command1, "SkinColorID", DbType.Int64, BizActionObj.DonorDetails.SkinColorID);
                dbServer.AddInParameter(command1, "HairColorID", DbType.Int64, BizActionObj.DonorDetails.HairColorID);
                dbServer.AddInParameter(command1, "EyeColorID", DbType.Int64, BizActionObj.DonorDetails.EyeColorID);
                dbServer.AddInParameter(command1, "DonorSourceID", DbType.Int64, BizActionObj.DonorDetails.DonorSourceID);
                dbServer.AddInParameter(command1, "DonorCode", DbType.String, BizActionObj.DonorDetails.DonorCode);
                dbServer.AddInParameter(command1, "Height", DbType.Double, BizActionObj.DonorDetails.Height);
                dbServer.AddInParameter(command1, "BoneStructure", DbType.String, BizActionObj.DonorDetails.BoneStructure);
                dbServer.AddInParameter(command1, "ReferralTypeID", DbType.Int64, objPatientVO.GeneralDetails.ReferralTypeID);
                dbServer.AddInParameter(command1, "AgencyID", DbType.Int64, objPatientVO.GeneralDetails.AgencyID);
                int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_AddPatientSponsor");

                dbServer.AddInParameter(command3, "LinkServer", DbType.String, null);
                dbServer.AddInParameter(command3, "LinkServerAlias", DbType.String, null);
                dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.DonorDetails.GeneralDetails.PatientID);
                dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.DonorDetails.GeneralDetails.UnitId);
                dbServer.AddInParameter(command3, "PatientSourceID", DbType.Int64, BizActionObj.DonorDetails.GeneralDetails.PatientSourceID);
                dbServer.AddInParameter(command3, "CompanyID", DbType.Int64, BizActionObj.DonorDetails.GeneralDetails.CompanyID);
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


                //if (BizActionObj.BatchDetails.BatchCode != null)
                //{
                //    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddDonorBatchDetails");
                //    dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.BatchDetails.ID);
                //    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, BizActionObj.BatchDetails.UnitID);
                //    dbServer.AddInParameter(command2, "DonorID", DbType.Int64, BizActionObj.DonorDetails.GeneralDetails.PatientID);
                //    dbServer.AddInParameter(command2, "DonorUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command2, "BatchCode", DbType.String, BizActionObj.BatchDetails.BatchCode);
                //    dbServer.AddInParameter(command2, "InvoiceNo", DbType.String, BizActionObj.BatchDetails.InvoiceNo);
                //    dbServer.AddInParameter(command2, "ReceivedByID", DbType.Int64, BizActionObj.BatchDetails.ReceivedByID);
                //    dbServer.AddInParameter(command2, "ReceivedDate", DbType.DateTime, BizActionObj.BatchDetails.ReceivedDate);
                //    dbServer.AddInParameter(command2, "LabID", DbType.Int64, BizActionObj.BatchDetails.LabID);
                //    dbServer.AddInParameter(command2, "NoOfVails", DbType.Int32, BizActionObj.BatchDetails.NoOfVails);
                //    dbServer.AddInParameter(command2, "Remark", DbType.String, BizActionObj.BatchDetails.Remark);
                //    dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                //    dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //    dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, objPatientVO.AddedDateTime);
                //    dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                //    int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                //    BizActionObj.BatchDetails.ID = (long)dbServer.GetParameterValue(command2, "ID");
                //    BizActionObj.BatchDetails.UnitID = UserVo.UserLoginInfo.UnitId;
                //}

            }
            catch (Exception)
            {
                trans.Rollback();
                //trans.Commit();
                //con.Close();
                throw;
            }
            finally
            {
                trans.Commit();
                con.Close();
                con = null;
                trans = null;

            }
            return BizActionObj;

        }

        private clsAddUpdateDonorBizActionVO UpdateDonorDetails(clsAddUpdateDonorBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                clsPatientVO objPatientVO = BizActionObj.DonorDetails;
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
                dbServer.AddInParameter(command, "MobileCountryCode", DbType.Int64, objPatientVO.MobileCountryCode);
                dbServer.AddInParameter(command, "ResiNoCountryCode", DbType.Int64, objPatientVO.ResiNoCountryCode);
                dbServer.AddInParameter(command, "ResiSTDCode", DbType.Int64, objPatientVO.ResiSTDCode);
                if (objPatientVO.Pincode != null) objPatientVO.Pincode = objPatientVO.Pincode.Trim();
                dbServer.AddInParameter(command, "Pincode", DbType.String, Security.base64Encode(objPatientVO.Pincode));
                dbServer.AddInParameter(command, "ReligionID", DbType.Int64, objPatientVO.ReligionID);
                dbServer.AddInParameter(command, "OccupationId", DbType.Int64, objPatientVO.OccupationId);
                dbServer.AddInParameter(command, "Photo", DbType.Binary, objPatientVO.Photo);
                dbServer.AddInParameter(command, "IsLoyaltyMember", DbType.Boolean, objPatientVO.IsLoyaltyMember);
                dbServer.AddInParameter(command, "LoyaltyCardID", DbType.Int64, objPatientVO.LoyaltyCardID);
                dbServer.AddInParameter(command, "IssueDate", DbType.DateTime, objPatientVO.IssueDate);
                dbServer.AddInParameter(command, "EffectiveDate", DbType.DateTime, objPatientVO.EffectiveDate);
                dbServer.AddInParameter(command, "ExpiryDate", DbType.DateTime, objPatientVO.ExpiryDate);
                if (objPatientVO.LoyaltyCardNo != null) objPatientVO.LoyaltyCardNo = objPatientVO.LoyaltyCardNo.Trim();
                dbServer.AddInParameter(command, "LoyaltyCardNo", DbType.String, objPatientVO.LoyaltyCardNo);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objPatientVO.Status);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objPatientVO.GeneralDetails.PatientUnitID);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objPatientVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "ID", DbType.Int64, objPatientVO.GeneralDetails.PatientID);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.SuccessStatus);

                int intStatus = dbServer.ExecuteNonQuery(command,trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddDonorOtherDetails");
                dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command1, "PatientID", DbType.Int64, objPatientVO.GeneralDetails.PatientID);
                dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, objPatientVO.GeneralDetails.PatientUnitID);
                dbServer.AddInParameter(command1, "SkinColorID", DbType.Int64, BizActionObj.DonorDetails.SkinColorID);
                dbServer.AddInParameter(command1, "HairColorID", DbType.Int64, BizActionObj.DonorDetails.HairColorID);
                dbServer.AddInParameter(command1, "EyeColorID", DbType.Int64, BizActionObj.DonorDetails.EyeColorID);
                dbServer.AddInParameter(command1, "DonorSourceID", DbType.Int64, BizActionObj.DonorDetails.DonorSourceID);
                dbServer.AddInParameter(command1, "DonorCode", DbType.String, BizActionObj.DonorDetails.DonorCode);
                dbServer.AddInParameter(command1, "Height", DbType.Double, BizActionObj.DonorDetails.Height);
                dbServer.AddInParameter(command1, "BoneStructure", DbType.String, BizActionObj.DonorDetails.BoneStructure);
                dbServer.AddInParameter(command1, "ReferralTypeID", DbType.Int64, objPatientVO.GeneralDetails.ReferralTypeID);
                dbServer.AddInParameter(command1, "AgencyID", DbType.Int64, objPatientVO.GeneralDetails.AgencyID);
             
                int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                //if (BizActionObj.BatchDetails.BatchCode != null)
                //{
                //    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddDonorBatchDetails");
                //    dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.BatchDetails.ID);
                //    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, BizActionObj.BatchDetails.UnitID);
                //    dbServer.AddInParameter(command2, "DonorID", DbType.Int64, objPatientVO.GeneralDetails.PatientID);
                //    dbServer.AddInParameter(command2, "DonorUnitID", DbType.Int64, objPatientVO.GeneralDetails.PatientUnitID);
                //    dbServer.AddInParameter(command2, "BatchCode", DbType.String, BizActionObj.BatchDetails.BatchCode);
                //    dbServer.AddInParameter(command2, "InvoiceNo", DbType.String, BizActionObj.BatchDetails.InvoiceNo);
                //    dbServer.AddInParameter(command2, "ReceivedByID", DbType.Int64, BizActionObj.BatchDetails.ReceivedByID);
                //    dbServer.AddInParameter(command2, "ReceivedDate", DbType.DateTime, BizActionObj.BatchDetails.ReceivedDate);
                //    dbServer.AddInParameter(command2, "LabID", DbType.Int64, BizActionObj.BatchDetails.LabID);
                //    dbServer.AddInParameter(command2, "NoOfVails", DbType.Int32, BizActionObj.BatchDetails.NoOfVails);
                //    dbServer.AddInParameter(command2, "Remark", DbType.String, BizActionObj.BatchDetails.Remark);
                //    dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                //    dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //    dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, objPatientVO.AddedDateTime);
                //    dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                //    int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                //    BizActionObj.BatchDetails.ID = (long)dbServer.GetParameterValue(command2, "ID");
                //    BizActionObj.BatchDetails.UnitID = UserVo.UserLoginInfo.UnitId;
                //}

            }
            catch (Exception)
            {
                trans.Rollback();
                trans.Commit();
                con.Close();
                throw;
            }
            finally
            {
                trans.Commit();
                con.Close();

            }
            return BizActionObj;
        }

        public override IValueObject GetDonorDetailsForIUI(IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {
            clsGetDonorDetailsForIUIBizActionVO BizAction = (valueObject) as clsGetDonorDetailsForIUIBizActionVO;
            con = dbServer.CreateConnection();
            con.Open();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVF_DashBoardGetDonorDetailsForIUI");

                dbServer.AddInParameter(command, "DonorID", DbType.Int64, BizAction.BatchDetails.DonorID);
                dbServer.AddInParameter(command, "DonorUnitID", DbType.Int64, BizAction.BatchDetails.DonorUnitID);
                dbServer.AddInParameter(command, "BatchID", DbType.Int64, BizAction.BatchDetails.ID);
                dbServer.AddInParameter(command, "BatchUnitID", DbType.Int64, BizAction.BatchDetails.UnitID);
     
              
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizAction.BatchDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.BatchDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizAction.BatchDetails.DonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorID"]));
                        BizAction.BatchDetails.DonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorUnitID"]));
                        BizAction.BatchDetails.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        BizAction.BatchDetails.DonorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DonorCode"]));
                        BizAction.BatchDetails.Lab = Convert.ToString(DALHelper.HandleDBNull(reader["Lab"]));
                        BizAction.BatchDetails.Eye = Convert.ToString(DALHelper.HandleDBNull(reader["Eye"]));
                        BizAction.BatchDetails.Skin = Convert.ToString(DALHelper.HandleDBNull(reader["Skin"]));
                        BizAction.BatchDetails.Hair = Convert.ToString(DALHelper.HandleDBNull(reader["Hair"])); // string
                        BizAction.BatchDetails.BloodGroup = Convert.ToString(DALHelper.HandleDBNull(reader["BloodGroup"]));
                        BizAction.BatchDetails.Height = Convert.ToSingle(DALHelper.HandleDBNull(reader["Height"]));
                        BizAction.BatchDetails.BoneStructure = Convert.ToString(DALHelper.HandleDBNull(reader["BoneStructure"]));
                       
                    }
                }
            }
            catch (Exception)
            {

                con.Close();
                throw;
            }
            finally
            {
                con.Close();
            }
            return BizAction;
        }

        public override IValueObject GetDonorDetailsAgainstSearch(IValueObject valueObject, ValueObjects.clsUserVO UserVo) 
        {
            clsGetDonorDetailsAgainstSearchBizActionVO BizAction = (valueObject) as clsGetDonorDetailsAgainstSearchBizActionVO;
            con = dbServer.CreateConnection();
            con.Open();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVF_DashBoardGetDonorDetailsAgainstSearch");
             
                dbServer.AddInParameter(command, "DonorCode", DbType.String, BizAction.DonorGeneralDetails.DonorCode);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizAction.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizAction.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizAction.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizAction.sortExpression);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizAction.DonorGeneralDetailsList == null)
                        BizAction.DonorGeneralDetailsList = new List<clsDonorGeneralDetailsVO>();
                    while (reader.Read())
                    {
                        clsDonorGeneralDetailsVO obj = new clsDonorGeneralDetailsVO();
                        obj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        obj.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        obj.DonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorID"]));
                        obj.DonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorUnitID"]));
                        obj.DonorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DonorCode"]));
                        obj.Eye = Convert.ToString(DALHelper.HandleDBNull(reader["Eye"]));
                        obj.Skin = Convert.ToString(DALHelper.HandleDBNull(reader["Skin"]));
                        obj.Hair = Convert.ToString(DALHelper.HandleDBNull(reader["Hair"]));
                        obj.BloodGroup = Convert.ToString(DALHelper.HandleDBNull(reader["Bloodgroup"]));
                        obj.Height = Convert.ToSingle(DALHelper.HandleDBNull(reader["Height"]));
                        obj.BoneStructure = Convert.ToString(DALHelper.HandleDBNull(reader["BoneStructure"]));
                        obj.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        obj.LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]));
                        obj.FirstName =Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]));
                        obj.MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"]));
                        obj.DateOfBirth = Convert.ToDateTime(DALHelper.HandleDate(reader["DateofBirth"]));
                        obj.RegistrationDate = Convert.ToDateTime(DALHelper.HandleDate(reader["RegistrationDate"]));
                        obj.TariffID =  Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"]));
                        obj.PatientSourceID =  Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceID"]));
                        obj.CompanyID =  Convert.ToInt64(DALHelper.HandleDBNull(reader["CompanyID"]));
                        obj.ContactNO1 = (Convert.ToString(DALHelper.HandleDBNull(reader["ContactNO1"])));
                        obj.MaritalStatus = Convert.ToString(DALHelper.HandleDBNull(reader["MaritalStatus"]));
                        obj.UniversalID = Security.base64Decode((string)DALHelper.HandleDBNull(reader["CivilID"]));
                        obj.PatientTypeID =  Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientCategoryID"]));
                        BizAction.DonorGeneralDetailsList.Add(obj);
                    }
                }
            }
            catch (Exception)
            {

                con.Close();
                throw;
            }
            finally
            {
                con.Close();
            }
            return BizAction;
        }

        public override IValueObject GetDonorDetailsToModify(IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {

            clsGetDonorDetailsBizActionVO BizActionObj = valueObject as clsGetDonorDetailsBizActionVO;
            try
            {
               
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDonorDetails");
                DbDataReader reader;

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.DonorDetails.GeneralDetails.PatientID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.DonorDetails.GeneralDetails.PatientUnitID);
              
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizActionObj.DonorDetails.GeneralDetails.PatientTypeID = (long)DALHelper.HandleDBNull(reader["PatientCategoryID"]);
                        BizActionObj.DonorDetails.GeneralDetails.ReferralTypeID = (long)DALHelper.HandleDBNull(reader["ReferralTypeID"]);
                        BizActionObj.DonorDetails.CompanyName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["CompanyName"]));
                        BizActionObj.DonorDetails.GeneralDetails.PatientID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        BizActionObj.DonorDetails.GeneralDetails.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                        BizActionObj.DonorDetails.GeneralDetails.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                        BizActionObj.DonorDetails.GeneralDetails.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                        BizActionObj.DonorDetails.GeneralDetails.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                        BizActionObj.DonorDetails.GeneralDetails.RegistrationDate = (DateTime?)DALHelper.HandleDate(reader["RegistrationDate"]);
                        BizActionObj.DonorDetails.GeneralDetails.DateOfBirth = (DateTime?)DALHelper.HandleDate(reader["DateOfBirth"]);
                        BizActionObj.DonorDetails.GeneralDetails.PatientSourceID = (long)DALHelper.HandleDBNull(reader["PatientSourceID"]);
                        BizActionObj.DonorDetails.GeneralDetails.TariffID = (long)DALHelper.HandleDBNull(reader["TariffID"]);
                        BizActionObj.DonorDetails.FamilyName = Security.base64Decode((String)DALHelper.HandleDBNull(reader["FamilyName"]));
                        BizActionObj.DonorDetails.GenderID = (Int64)DALHelper.HandleDBNull(reader["GenderID"]);
                        BizActionObj.DonorDetails.BloodGroupID = (Int64)DALHelper.HandleDBNull(reader["BloodGroupID"]);
                        BizActionObj.DonorDetails.MaritalStatusID = (Int64)DALHelper.HandleDBNull(reader["MaritalStatusID"]);
                        BizActionObj.DonorDetails.CivilID = Security.base64Decode((string)DALHelper.HandleDBNull(reader["CivilID"]));
                        BizActionObj.DonorDetails.ContactNo1 = (string)DALHelper.HandleDBNull(reader["ContactNo1"]);
                        BizActionObj.DonorDetails.ContactNo2 = (string)DALHelper.HandleDBNull(reader["ContactNo2"]);
                        BizActionObj.DonorDetails.FaxNo = (String)DALHelper.HandleDBNull(reader["FaxNo"]);
                        BizActionObj.DonorDetails.Email = Security.base64Decode((String)DALHelper.HandleDBNull(reader["Email"]));
                        BizActionObj.DonorDetails.AddressLine1 = Security.base64Decode((String)DALHelper.HandleDBNull(reader["AddressLine1"]));
                        BizActionObj.DonorDetails.AddressLine2 = Security.base64Decode((String)DALHelper.HandleDBNull(reader["AddressLine2"]));
                        BizActionObj.DonorDetails.AddressLine3 = Security.base64Decode((String)DALHelper.HandleDBNull(reader["AddressLine3"]));
                        BizActionObj.DonorDetails.Country = Security.base64Decode((String)DALHelper.HandleDBNull(reader["Country"]));
                        BizActionObj.DonorDetails.State = Security.base64Decode((String)DALHelper.HandleDBNull(reader["State"]));
                        BizActionObj.DonorDetails.City = Security.base64Decode((String)DALHelper.HandleDBNull(reader["City"]));
                        BizActionObj.DonorDetails.Taluka = Security.base64Decode((string)DALHelper.HandleDBNull(reader["Taluka"]));
                        BizActionObj.DonorDetails.Area = Security.base64Decode((string)DALHelper.HandleDBNull(reader["Area"]));
                        BizActionObj.DonorDetails.District = Security.base64Decode((string)DALHelper.HandleDBNull(reader["District"]));
                        BizActionObj.DonorDetails.MobileCountryCode = Convert.ToString(DALHelper.HandleDBNull(reader["MobileCountryCode"]));
                        BizActionObj.DonorDetails.ResiNoCountryCode = (long)DALHelper.HandleDBNull(reader["ResiNoCountryCode"]);
                        BizActionObj.DonorDetails.ResiSTDCode = (long)DALHelper.HandleDBNull(reader["ResiSTDCode"]);
                        BizActionObj.DonorDetails.CountryID = (long)DALHelper.HandleIntegerNull(reader["CountryID"]);
                        BizActionObj.DonorDetails.StateID = (long)DALHelper.HandleIntegerNull(reader["StateID"]);
                        BizActionObj.DonorDetails.CityID = (long)DALHelper.HandleIntegerNull(reader["CityID"]);
                        BizActionObj.DonorDetails.RegionID = (long)DALHelper.HandleIntegerNull(reader["RegionID"]);
                        BizActionObj.DonorDetails.Pincode = Security.base64Decode((String)DALHelper.HandleDBNull(reader["Pincode"]));
                        BizActionObj.DonorDetails.ReligionID = (Int64)DALHelper.HandleDBNull(reader["ReligionID"]);
                        BizActionObj.DonorDetails.OccupationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["OccupationId"]));
                        if ((Byte[])DALHelper.HandleDBNull(reader["Photo"]) != null)
                        {
                            BizActionObj.DonorDetails.Photo = (Byte[])DALHelper.HandleDBNull(reader["Photo"]);
                        }
                        if (BizActionObj.DonorDetails.GeneralDetails.Photo != null)
                        {
                            BizActionObj.DonorDetails.GeneralDetails.Photo = (Byte[])DALHelper.HandleDBNull(reader["Photo"]);
                        }
                        BizActionObj.DonorDetails.IsLoyaltyMember = (bool)DALHelper.HandleDBNull(reader["IsLoyaltyMember"]);
                        BizActionObj.DonorDetails.LoyaltyCardID = (long?)DALHelper.HandleDBNull(reader["LoyaltyCardID"]);
                        BizActionObj.DonorDetails.IssueDate = (DateTime?)DALHelper.HandleDate(reader["IssueDate"]);
                        BizActionObj.DonorDetails.EffectiveDate = (DateTime?)DALHelper.HandleDate(reader["EffectiveDate"]);
                        BizActionObj.DonorDetails.ExpiryDate = (DateTime?)DALHelper.HandleDate(reader["ExpiryDate"]);
                        BizActionObj.DonorDetails.LoyaltyCardNo = (string)DALHelper.HandleDBNull(reader["LoyaltyCardNo"]);
                        BizActionObj.DonorDetails.RelationID = (long)DALHelper.HandleDBNull(reader["RelationID"]);
                        BizActionObj.DonorDetails.ParentPatientID = (long)DALHelper.HandleDBNull(reader["ParentPatientID"]);
                        BizActionObj.DonorDetails.GeneralDetails.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                        BizActionObj.DonorDetails.SkinColorID =  Convert.ToInt64(DALHelper.HandleDBNull(reader["SkinColorID"]));
                        BizActionObj.DonorDetails.HairColorID =  Convert.ToInt64(DALHelper.HandleDBNull(reader["HairColorID"]));
                        BizActionObj.DonorDetails.EyeColorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EyeColorID"]));
                        BizActionObj.DonorDetails.DonorSourceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorSourceID"]));
                        BizActionObj.DonorDetails.DonorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DonorCode"]));
                        BizActionObj.DonorDetails.Height = Convert.ToSingle(DALHelper.HandleDBNull(reader["Height"]));
                        BizActionObj.DonorDetails.BoneStructure = Convert.ToString(DALHelper.HandleDBNull(reader["BoneStructure"]));


                        
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


        public override IValueObject GetDonorBatchDetails(IValueObject valueObject, ValueObjects.clsUserVO UserVo) 
        {
            clsGetDonorBatchDetailsBizActionVO BizActionObj = valueObject as clsGetDonorBatchDetailsBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("IVF_DashBoardGetDonorBatchDetails");
                DbDataReader reader;

                dbServer.AddInParameter(command, "DonorID", DbType.Int64, BizActionObj.BatchDetails.DonorID);
                dbServer.AddInParameter(command, "DonorUnitID", DbType.Int64, BizActionObj.BatchDetails.DonorUnitID);
              
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.BatchDetailsList == null)
                        BizActionObj.BatchDetailsList = new List<clsDonorBatchVO>();
                    while (reader.Read())
                    {
                        clsDonorBatchVO obj = new clsDonorBatchVO();
                        obj.ID = (Int64)DALHelper.HandleDBNull(reader["ID"]);
                        obj.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        obj.DonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorID"]));
                        obj.DonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorUnitID"]));
                        obj.DonorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DonorCode"]));
                        obj.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        obj.ReceivedByID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedByID"]));
                        obj.ReceivedDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ReceivedDate"]));
                        obj.InvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader["InvoiceNo"]));
                        obj.NoOfVails = Convert.ToInt32(DALHelper.HandleDBNull(reader["NoOfVails"]));
                        obj.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        obj.LabID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabID"]));
                        obj.ReceivedBy = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedBy"]));
                        obj.Lab = Convert.ToString(DALHelper.HandleDBNull(reader["Lab"]));
                        BizActionObj.BatchDetailsList.Add(obj);
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


        public override IValueObject CheckDuplicasyDonorCodeAndBLab(IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {
            
             clsCheckDuplicasyDonorCodeAndBLabBizActionVO BizActionObj = valueObject as clsCheckDuplicasyDonorCodeAndBLabBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("IVF_CheckDuplicasyDonorCodeAndBLab");
                DbDataReader reader;

                dbServer.AddInParameter(command, "DonorCode", DbType.Int64, BizActionObj.DonorCode);
                dbServer.AddInParameter(command, "LabID", DbType.Int64, BizActionObj.LabID);
              
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizActionObj.IsDuplicate = (bool)DALHelper.HandleDBNull(reader["IsDuplicate"]);
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

        public override IValueObject GetDetailsOfReceivedOocyte(IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {
            clsGetDetailsOfReceivedOocyteBizActionVO BizActionObj = valueObject as clsGetDetailsOfReceivedOocyteBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDetailsOfReceivedOocyte");
                DbDataReader reader;

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.Details.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.Details.PatientUnitID);

                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.Details.TherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Details.TherapyUnitID);

                dbServer.AddInParameter(command, "IsReceiveOocyte", DbType.Boolean, BizActionObj.Details.IsReceiveOocyte);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizActionObj.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizActionObj.Details.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizActionObj.Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        BizActionObj.Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        BizActionObj.Details.TherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyID"]));
                        BizActionObj.Details.TherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyUnitID"]));
                        BizActionObj.Details.DonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorID"]));
                        BizActionObj.Details.DonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorUnitID"]));
                        BizActionObj.Details.DonorOPUID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorOPUID"]));
                        BizActionObj.Details.DonorOPUUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorOPUUnitID"]));
                        BizActionObj.Details.DonorOPUDate = Convert.ToDateTime(DALHelper.HandleDate(reader["DonorOPUDate"]));
                        BizActionObj.Details.DonorOocyteRetrived = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorOocyteRetrived"]));
                        BizActionObj.Details.DonorBalancedOocyte = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorBalancedOocyte"]));
                        BizActionObj.Details.OocyteConsumed = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteConsumed"]));
                        BizActionObj.Details.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                    }

                    //reader.NextResult();

                    ////if (BizActionObj.Details.TestList == null)
                    ////    BizActionObj.Details.TestList = new List<clsPathoTestParameterVO>();
                    //while (reader.Read())
                    //{
                    //    clsIVFDashboard_OPUVO OPUDetails = new clsIVFDashboard_OPUVO();

                    //    BizActionObj.OPUDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                    //    BizActionObj.OPUDetails.OPUID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                    //    BizActionObj.OPUDetails.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                    //    BizActionObj.OPUDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                    //    BizActionObj.OPUDetails.OocyteRetrived = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteRetrived"]));
                    //    BizActionObj.OPUDetails.BalanceOocyte = Convert.ToInt64(DALHelper.HandleDBNull(reader["BalanceOocyte"]));
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
        public override IValueObject AddUpdateRecievOocytesDetails(IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {

            clsIVFDashboard_AddUpdateRecieceOocytesBizActionVO BizAction = valueObject as clsIVFDashboard_AddUpdateRecieceOocytesBizActionVO;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                clsReceiveOocyteVO ObjDay1VO = BizAction.OPUDetails;
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_IVFDAshBoard_AddUpdateRecieceOocytesDetails");
                if (BizAction.OPUDetails.ID > 0)
                {
                    dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.OPUDetails.ID);
                }
                else
                {
                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                }
                //UnitID	PatientID	PatientUnitID	TherapyID	TherapyUnitID	DonorID	DonorUnitID	DonorOPUID	DonorOPUUnitID
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "IsEdit", DbType.Boolean, BizAction.IsEdit);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.OPUDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.OPUDetails.PatientUnitID);
                dbServer.AddInParameter(command, "TherapyID", DbType.Int64, BizAction.OPUDetails.TherapyID);
                dbServer.AddInParameter(command, "TherapyUnitID", DbType.Int64, BizAction.OPUDetails.TherapyUnitID);
                dbServer.AddInParameter(command, "DonorID", DbType.Int64, BizAction.OPUDetails.DonorID);
                dbServer.AddInParameter(command, "DonorUnitID", DbType.Int64, BizAction.OPUDetails.DonorUnitID);
                dbServer.AddInParameter(command, "DonorOPUID", DbType.Int64, BizAction.OPUDetails.DonorOPUID);
                dbServer.AddInParameter(command, "DonorOPUUnitID", DbType.String, BizAction.OPUDetails.DonorOPUUnitID);

                //DonorOPUID	DonorOPUUnitID	DonorOPUDate	            DonorOocyteRetrived	DonorBalancedOocyte	OocyteConsumed	IsFreezed
                dbServer.AddInParameter(command, "DonorOPUDate", DbType.String, BizAction.OPUDetails.DonorOPUDate);
                dbServer.AddInParameter(command, "DonorOocyteRetrived", DbType.Int64, BizAction.OPUDetails.DonorOocyteRetrived);
                dbServer.AddInParameter(command, "DonorBalancedOocyte", DbType.Int64, BizAction.OPUDetails.DonorBalancedOocyte);

                dbServer.AddInParameter(command, "OocyteConsumed", DbType.Int64, BizAction.OPUDetails.OocyteConsumed);
                dbServer.AddInParameter(command, "IsFreezed", DbType.Int64, BizAction.OPUDetails.IsFreezed);

                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDay1VO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizAction.OPUDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

            }
            catch (Exception)
            {
                trans.Commit();
                con.Close();

                throw;
            }
            finally
            {
                trans.Commit();
                con.Close();

            }
            return BizAction;
        }
        public override IValueObject GetOPUDate(IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {
            GetIVFDashboardOPUDateBizActionVO BizActionObj = valueObject as GetIVFDashboardOPUDateBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDetailsOfOPUDate");
                DbDataReader reader;

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsIVFDashboard_OPUVO>();

                    while (reader.Read())
                    {
                        clsIVFDashboard_OPUVO objOPUDates = new clsIVFDashboard_OPUVO();

                        objOPUDates.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objOPUDates.OPUID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objOPUDates.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objOPUDates.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objOPUDates.OocyteRetrived = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteRetrived"]));
                        objOPUDates.BalanceOocyte = Convert.ToInt64(DALHelper.HandleDBNull(reader["BalanceOocyte"]));
                        BizActionObj.List.Add(objOPUDates);
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
        public override IValueObject GetSemenBatchAndSpermiogram(IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {
            cls_GetSemenBatchAndSpermiogramBizActionVO BizActionObj = valueObject as cls_GetSemenBatchAndSpermiogramBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetSemenBatchList");
                DbDataReader reader;

                dbServer.AddInParameter(command, "PatientID", DbType.String, BizActionObj.Details.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.Details.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Details.PlanTherapyUnitID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.DetailsList == null)
                        BizActionObj.DetailsList = new List<clsBatchAndSpemFreezingVO>();

                    while (reader.Read())
                    {

                        clsBatchAndSpemFreezingVO obj = new clsBatchAndSpemFreezingVO();
                        obj.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        obj.DonorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DonorCode"]));
                        obj.Abstience = Convert.ToString(DALHelper.HandleDBNull(reader["Abstience"]));
                        obj.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        obj.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
                        obj.BatchUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchUnitID"]));
                        obj.Cane = Convert.ToString(DALHelper.HandleDBNull(reader["Cane"]));
                        obj.CaneID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CaneID"]));
                        obj.Canister = Convert.ToString(DALHelper.HandleDBNull(reader["Canister"]));
                        obj.CanisterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CanisterID"]));
                        obj.ColorCode = Convert.ToString(DALHelper.HandleDBNull(reader["ColorCode"]));
                        obj.ColorCodeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ColorCodeID"]));
                        obj.Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"]));
                        obj.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                        obj.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        obj.GlobletShape = Convert.ToString(DALHelper.HandleDBNull(reader["GlobletShape"]));
                        obj.GlobletShapeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GlobletShapeID"]));
                        obj.GlobletSize = Convert.ToString(DALHelper.HandleDBNull(reader["GlobletSize"]));
                        obj.GlobletSizeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GlobletSizeID"]));
                        obj.GradeA = Convert.ToDecimal(DALHelper.HandleDBNull(reader["GradeA"]));
                        obj.GradeB = Convert.ToDecimal(DALHelper.HandleDBNull(reader["GradeB"]));
                        obj.GradeC = Convert.ToDecimal(DALHelper.HandleDBNull(reader["GradeC"]));
                        obj.InvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader["InvoiceNo"]));
                        obj.IsThaw = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsThaw"]));
                        obj.Lab = Convert.ToString(DALHelper.HandleDBNull(reader["Lab"]));
                        obj.LabID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabID"]));
                        obj.Motility = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Motility"]));
                        //obj.NoOfVails = Convert.ToInt32(DALHelper.HandleDBNull(reader["NoOfVails"]));
                        obj.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        obj.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        obj.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        obj.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        obj.ReceivedByID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedByID"]));
                        obj.ReceivedByName = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedByName"]));
                        obj.ReceivedDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ReceivedDate"]));
                        //obj.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        obj.SpermFreezingDetailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpermFreezingDetailsID"]));
                        obj.SpermFreezingID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpermFreezingID"]));
                        obj.SpermFreezingUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpermFreezingUnitID"]));
                        obj.SpremFreezingDate = Convert.ToDateTime(DALHelper.HandleDate(reader["SpremFreezingDate"]));
                        obj.SpermFreezingDetailsUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpermFreezingDetailsUnitID"]));
                        obj.SpremFreezingTime = Convert.ToDateTime(DALHelper.HandleDate(reader["SpremFreezingTime"]));
                        obj.SpremNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpremNo"]));
                        obj.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        obj.Straw = Convert.ToString(DALHelper.HandleDBNull(reader["Straw"]));
                        obj.StrawID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StrawID"]));
                        obj.Tank = Convert.ToString(DALHelper.HandleDBNull(reader["Tank"]));
                        obj.TankID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TankID"]));
                        obj.Viscosity = Convert.ToString(DALHelper.HandleDBNull(reader["Viscosity"]));
                        obj.ViscosityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ViscosityID"]));
                        obj.Volume = Convert.ToSingle(DALHelper.HandleDBNull(reader["Volume"]));
                        obj.CollectionMethodID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CollectionMethodID"]));
                        obj.CollectionProblem = Convert.ToString(DALHelper.HandleDBNull(reader["CollectionProblem"]));
                        obj.FreezingOther = Convert.ToString(DALHelper.HandleDBNull(reader["FreezingOther"]));
                        obj.FreezingComments = Convert.ToString(DALHelper.HandleDBNull(reader["FreezingComments"]));
                        obj.TotalSpremCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["TotalSpremCount"]));

                        BizActionObj.DetailsList.Add(obj);
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

        public override IValueObject GetDonorList(IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {
            // throw new NotImplementedException();
            clsGetDonorListBizActionVO BizActionObj = valueObject as clsGetDonorListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDonorList");
                DbDataReader reader;

                dbServer.AddInParameter(command, "DonorCode", DbType.String, BizActionObj.DonorDetails.DonorCode);
                dbServer.AddInParameter(command, "AgencyID", DbType.Int64, BizActionObj.DonorDetails.AgencyID);
                dbServer.AddInParameter(command, "HairColorID", DbType.Int64, BizActionObj.DonorDetails.HairColorID);
                dbServer.AddInParameter(command, "SkinColorID", DbType.Int64, BizActionObj.DonorDetails.SkinColorID);
                dbServer.AddInParameter(command, "EyeColorID", DbType.Int64, BizActionObj.DonorDetails.EyeColorID);
                dbServer.AddInParameter(command, "BloodGroupID", DbType.Int64, BizActionObj.DonorDetails.BloodGroupID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.DonorGeneralDetailsList == null)
                        BizActionObj.DonorGeneralDetailsList = new List<clsPatientVO>();

                    while (reader.Read())
                    {

                        clsPatientVO obj = new clsPatientVO();
                        obj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        obj.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        obj.DonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorID"]));
                        obj.DonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorUnitID"]));
                        obj.DonorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DonorCode"]));
                        obj.Eye = Convert.ToString(DALHelper.HandleDBNull(reader["Eye"]));
                        obj.Skin = Convert.ToString(DALHelper.HandleDBNull(reader["Skin"]));
                        obj.Hair = Convert.ToString(DALHelper.HandleDBNull(reader["Hair"]));
                        obj.BloodGroup = Convert.ToString(DALHelper.HandleDBNull(reader["Bloodgroup"]));
                        obj.Height = Convert.ToSingle(DALHelper.HandleDBNull(reader["Height"]));
                        obj.BoneStructure = Convert.ToString(DALHelper.HandleDBNull(reader["BoneStructure"]));
                        obj.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        obj.LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]));
                        obj.FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]));
                        obj.MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"]));
                        obj.DateOfBirth = Convert.ToDateTime(DALHelper.HandleDate(reader["DateofBirth"]));
                        obj.RegistrationDate = Convert.ToDateTime(DALHelper.HandleDate(reader["RegistrationDate"]));
                        obj.TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"]));
                        obj.PatientSourceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceID"]));
                        obj.CompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompanyID"]));
                        obj.ContactNO1 = (Convert.ToString(DALHelper.HandleDBNull(reader["ContactNO1"])));
                        obj.MaritalStatus = Convert.ToString(DALHelper.HandleDBNull(reader["MaritalStatus"]));
                        obj.UniversalID = Security.base64Decode((string)DALHelper.HandleDBNull(reader["CivilID"]));
                        obj.PatientTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientCategoryID"]));
                        obj.ReferralTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferralTypeID"]));
                        obj.AgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgencyID"]));
                        obj.ReferralName = Convert.ToString(DALHelper.HandleDBNull(reader["ReferralName"]));
                        obj.AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"]));
                        obj.SkinColorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SkinColorID"]));
                        obj.HairColorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["HairColorID"]));
                        obj.EyeColorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EyeColorID"]));
                        obj.BloodGroupID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BloodGroupID"]));
                        BizActionObj.DonorGeneralDetailsList.Add(obj);
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

        public override IValueObject AddUpdateDonorBatch(IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsAddUpdateDonorBatchBizActionVO BizActionObj = valueObject as clsAddUpdateDonorBatchBizActionVO;
            DbTransaction trans = null;
            DbConnection con = dbServer.CreateConnection();
            // clsAddUpdateDonorBizActionVO BizActionObj= valueObject as clsAddUpdateDonorBizActionVO;
            try
            {
                clsSemenSampleBatchVO objPatientVO = BizActionObj.BatchDetails;

                con.Open();
                trans = con.BeginTransaction();

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddDonorBatchDetails");

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objPatientVO.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objPatientVO.PatientUnitID);
                dbServer.AddInParameter(command, "BatchCode", DbType.String, objPatientVO.BatchCode);
                dbServer.AddInParameter(command, "InvoiceNo", DbType.String, objPatientVO.InvoiceNo);
                dbServer.AddInParameter(command, "ReceivedByID", DbType.Int64, objPatientVO.ReceivedByID);
                dbServer.AddInParameter(command, "ReceivedDate", DbType.DateTime, objPatientVO.ReceivedDate);
                dbServer.AddInParameter(command, "LabID", DbType.Int64, objPatientVO.LabID);
                dbServer.AddInParameter(command, "NoOfVails", DbType.Int32, objPatientVO.NoOfVails);
                dbServer.AddInParameter(command, "Volume", DbType.Single, objPatientVO.Volume);
                dbServer.AddInParameter(command, "AvailableVolume", DbType.Single, objPatientVO.AvailableVolume);
                dbServer.AddInParameter(command, "Remark", DbType.String, objPatientVO.Remark);
                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.BatchDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.BatchDetails.UnitID = UserVo.UserLoginInfo.UnitId;


                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdateSpremFreezing_New");
                dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.FreezingObj.ID);
                dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command1, "PatientID", DbType.Int64, objPatientVO.PatientID);
                dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, objPatientVO.PatientUnitID);
                dbServer.AddInParameter(command1, "BatchID", DbType.Int64, BizActionObj.BatchDetails.ID);
                dbServer.AddInParameter(command1, "BatchUnitID", DbType.Int64, BizActionObj.BatchDetails.UnitID);
                dbServer.AddInParameter(command1, "TherapyID", DbType.Int64, BizActionObj.FreezingObj.TherapyID);
                dbServer.AddInParameter(command1, "TherapyUnitID", DbType.Int64, BizActionObj.FreezingObj.TherapyUnitID);
                dbServer.AddInParameter(command1, "CycleCode", DbType.String, BizActionObj.FreezingObj.CycleCode);
                dbServer.AddInParameter(command1, "DoctorID", DbType.Int64, BizActionObj.FreezingObj.DoctorID);
                dbServer.AddInParameter(command1, "EmbryologistID", DbType.Int64, BizActionObj.FreezingObj.EmbryologistID);
                dbServer.AddInParameter(command1, "SpremFreezingTime", DbType.DateTime, BizActionObj.FreezingObj.SpremFreezingDate);
                dbServer.AddInParameter(command1, "SpremFreezingDate", DbType.DateTime, BizActionObj.FreezingObj.SpremFreezingTime);
                dbServer.AddInParameter(command1, "CollectionMethodID", DbType.Int64, BizActionObj.FreezingObj.CollectionMethodID);
                dbServer.AddInParameter(command1, "ViscosityID", DbType.Int64, BizActionObj.FreezingObj.ViscosityID);
                dbServer.AddInParameter(command1, "CollectionProblem", DbType.String, BizActionObj.FreezingObj.CollectionProblem);
                dbServer.AddInParameter(command1, "Other", DbType.String, BizActionObj.FreezingObj.Other);
                dbServer.AddInParameter(command1, "Comments", DbType.String, BizActionObj.FreezingObj.Comments);
                dbServer.AddInParameter(command1, "Abstience", DbType.String, BizActionObj.FreezingObj.Abstience);
                dbServer.AddInParameter(command1, "Volume", DbType.Single, BizActionObj.FreezingObj.Volume);
                dbServer.AddInParameter(command1, "Motility", DbType.Decimal, BizActionObj.FreezingObj.Motility);
                dbServer.AddInParameter(command1, "GradeA", DbType.Decimal, BizActionObj.FreezingObj.GradeA);
                dbServer.AddInParameter(command1, "GradeB", DbType.Decimal, BizActionObj.FreezingObj.GradeB);
                dbServer.AddInParameter(command1, "GradeC", DbType.Decimal, BizActionObj.FreezingObj.GradeC);
                dbServer.AddInParameter(command1, "TotalSpremCount", DbType.Int64, BizActionObj.FreezingObj.TotalSpremCount);
                dbServer.AddInParameter(command1, "Status", DbType.Boolean, BizActionObj.FreezingObj.Status);
                dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                BizActionObj.FreezingObj.ID = (long)dbServer.GetParameterValue(command1, "ID");
                BizActionObj.FreezingObj.UnitID = (long)dbServer.GetParameterValue(command1, "UnitID");


                foreach (var item in BizActionObj.FreezingDetailsList)
                {

                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdateSpremFreezingDetails_New");
                    dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "SpremFreezingID", DbType.Int64, BizActionObj.FreezingObj.ID);
                    dbServer.AddInParameter(command2, "SpremFreezingUnitID", DbType.Int64, BizActionObj.FreezingObj.UnitID);
                    dbServer.AddInParameter(command2, "ColorCodeID", DbType.Int64, item.GobletColorID);
                    dbServer.AddInParameter(command2, "PlanTherapy", DbType.Int64, item.PlanTherapy);
                    dbServer.AddInParameter(command2, "PlanTherapyUnitID", DbType.Int64, item.PlanTherapyUnitID);
                    dbServer.AddInParameter(command2, "StrawID", DbType.Int64, item.StrawId);
                    dbServer.AddInParameter(command2, "GlobletShapeID", DbType.Int64, item.GobletShapeId);
                    dbServer.AddInParameter(command2, "GlobletSizeID", DbType.Int64, item.GobletSizeId);
                    dbServer.AddInParameter(command2, "Status", DbType.Boolean, item.Status);
                    dbServer.AddInParameter(command2, "IsModify", DbType.String, item.IsModify);
                    dbServer.AddInParameter(command2, "IsThaw", DbType.String, item.IsThaw);
                    dbServer.AddInParameter(command2, "CaneID", DbType.Int64, item.CanID);
                    dbServer.AddInParameter(command2, "CanisterID", DbType.Int64, item.CanisterId);
                    dbServer.AddInParameter(command2, "TankID", DbType.Int64, item.TankId);
                    dbServer.AddInParameter(command2, "Comments", DbType.String, item.Comments);
                    dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                }


            }
            catch (Exception)
            {
                trans.Rollback();
                //trans.Commit();
                //con.Close();
                throw;
            }
            finally
            {
                trans.Commit();
                con.Close();
                con = null;
                trans = null;

            }
            return BizActionObj;

        }

        //added by neena for getting donated oocyte ambryo details
        public override IValueObject GetDetailsOfReceivedOocyteEmbryo(IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {
            clsGetDetailsOfReceivedOocyteEmbryoBizActionVO BizActionObj = valueObject as clsGetDetailsOfReceivedOocyteEmbryoBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashboard_AddDonatedOocyteEmbryoForRecepient");
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                //DbDataReader reader;

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.Details.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.Details.PatientUnitID);

                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.Details.TherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Details.TherapyUnitID);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                
                //reader = (DbDataReader)dbServer.ExecuteReader(command,trans);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
            }            
            catch (Exception)
            {
                trans.Rollback();
                //trans.Commit();
                //con.Close();
                throw;
            }
            finally
            {
                trans.Commit();
                con.Close();
                con = null;
                trans = null;

            }
            return valueObject;
        }
        //

        //added by neena for getting donated oocyte Embryo from donor cycle
        public override IValueObject GetDetailsOfReceivedOocyteEmbryoFromDonorCycle(IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {
            clsGetDetailsOfReceivedOocyteEmbryoBizActionVO BizActionObj = valueObject as clsGetDetailsOfReceivedOocyteEmbryoBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashboard_AddDonatedOocyteFromDonorCycle");
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                //DbDataReader reader;

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.Details.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.Details.PatientUnitID);

                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.Details.TherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Details.TherapyUnitID);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                //reader = (DbDataReader)dbServer.ExecuteReader(command,trans);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
            }
            catch (Exception)
            {
                trans.Rollback();
                //trans.Commit();
                //con.Close();
                throw;
            }
            finally
            {
                trans.Commit();
                con.Close();
                con = null;
                trans = null;

            }
            return valueObject;
        }
        //
    }
}
