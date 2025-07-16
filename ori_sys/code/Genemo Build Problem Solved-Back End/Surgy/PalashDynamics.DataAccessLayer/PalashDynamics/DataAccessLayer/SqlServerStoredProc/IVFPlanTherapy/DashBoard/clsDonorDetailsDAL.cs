namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IVFPlanTherapy.DashBoard
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.DashBoardVO;
    using PalashDynamics.ValueObjects.IVFPlanTherapy;
    using PalashDynamics.ValueObjects.Patient;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    internal class clsDonorDetailsDAL : clsBaseDonorDetailsDAL
    {
        private Database dbServer;
        private LogManager logManager;
        private DbTransaction trans;
        private DbConnection con;

        private clsDonorDetailsDAL()
        {
            try
            {
                if (this.dbServer == null)
                {
                    this.dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
                if (this.logManager == null)
                {
                    this.logManager = LogManager.GetInstance();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private clsAddUpdateDonorBizActionVO AddDonorDetails(clsAddUpdateDonorBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                clsPatientVO donorDetails = BizActionObj.DonorDetails;
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddDonor");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, donorDetails.GeneralDetails.LinkServer);
                if (donorDetails.GeneralDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, donorDetails.GeneralDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "PatientCategoryID", DbType.Int64, donorDetails.GeneralDetails.PatientTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "ReferralTypeID", DbType.Int64, donorDetails.GeneralDetails.ReferralTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyName", DbType.String, Security.base64Encode(donorDetails.CompanyName));
                this.dbServer.AddInParameter(storedProcCommand, "RegistrationDate", DbType.DateTime, donorDetails.GeneralDetails.RegistrationDate);
                if (donorDetails.GeneralDetails.LastName != null)
                {
                    donorDetails.GeneralDetails.LastName = donorDetails.GeneralDetails.LastName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, Security.base64Encode(donorDetails.GeneralDetails.LastName));
                if (donorDetails.GeneralDetails.FirstName != null)
                {
                    donorDetails.GeneralDetails.FirstName = donorDetails.GeneralDetails.FirstName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, Security.base64Encode(donorDetails.GeneralDetails.FirstName));
                if (donorDetails.GeneralDetails.MiddleName != null)
                {
                    donorDetails.GeneralDetails.MiddleName = donorDetails.GeneralDetails.MiddleName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, Security.base64Encode(donorDetails.GeneralDetails.MiddleName));
                if (donorDetails.FamilyName != null)
                {
                    donorDetails.FamilyName = donorDetails.FamilyName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "FamilyName", DbType.String, Security.base64Encode(donorDetails.FamilyName));
                this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, donorDetails.GenderID);
                this.dbServer.AddInParameter(storedProcCommand, "DateOfBirth", DbType.DateTime, donorDetails.GeneralDetails.DateOfBirth);
                this.dbServer.AddInParameter(storedProcCommand, "BloodGroupID", DbType.Int64, donorDetails.BloodGroupID);
                this.dbServer.AddInParameter(storedProcCommand, "MaritalStatusID", DbType.Int64, donorDetails.MaritalStatusID);
                this.dbServer.AddInParameter(storedProcCommand, "CivilID", DbType.String, Security.base64Encode(donorDetails.CivilID));
                if (donorDetails.ContactNo1 != null)
                {
                    donorDetails.ContactNo1 = donorDetails.ContactNo1.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo1", DbType.String, donorDetails.ContactNo1);
                if (donorDetails.ContactNo2 != null)
                {
                    donorDetails.ContactNo2 = donorDetails.ContactNo2.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo2", DbType.String, donorDetails.ContactNo2);
                if (donorDetails.FaxNo != null)
                {
                    donorDetails.FaxNo = donorDetails.FaxNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "FaxNo", DbType.String, donorDetails.FaxNo);
                if (donorDetails.Email != null)
                {
                    donorDetails.Email = donorDetails.Email.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Email", DbType.String, Security.base64Encode(donorDetails.Email));
                if (donorDetails.AddressLine1 != null)
                {
                    donorDetails.AddressLine1 = donorDetails.AddressLine1.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine1", DbType.String, Security.base64Encode(donorDetails.AddressLine1));
                if (donorDetails.AddressLine2 != null)
                {
                    donorDetails.AddressLine2 = donorDetails.AddressLine2.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine2", DbType.String, Security.base64Encode(donorDetails.AddressLine2));
                if (donorDetails.AddressLine3 != null)
                {
                    donorDetails.AddressLine3 = donorDetails.AddressLine3.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine3", DbType.String, Security.base64Encode(donorDetails.AddressLine3));
                if (donorDetails.Country != null)
                {
                    donorDetails.Country = donorDetails.Country.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Country", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "State", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "City", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "Taluka", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "Area", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "District", DbType.String, null);
                if (donorDetails.CountryID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CountryID", DbType.Int64, donorDetails.CountryID);
                }
                if (donorDetails.StateID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "StateID", DbType.Int64, donorDetails.StateID);
                }
                if (donorDetails.CityID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CityID", DbType.Int64, donorDetails.CityID);
                }
                if (donorDetails.RegionID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "RegionID", DbType.Int64, donorDetails.RegionID);
                }
                this.dbServer.AddInParameter(storedProcCommand, "MobileCountryCode", DbType.Int64, donorDetails.MobileCountryCode);
                this.dbServer.AddInParameter(storedProcCommand, "ResiNoCountryCode", DbType.Int64, donorDetails.ResiNoCountryCode);
                this.dbServer.AddInParameter(storedProcCommand, "ResiSTDCode", DbType.Int64, donorDetails.ResiSTDCode);
                if (donorDetails.Pincode != null)
                {
                    donorDetails.Pincode = donorDetails.Pincode.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Pincode", DbType.String, Security.base64Encode(donorDetails.Pincode));
                this.dbServer.AddInParameter(storedProcCommand, "ReligionID", DbType.Int64, donorDetails.ReligionID);
                this.dbServer.AddInParameter(storedProcCommand, "OccupationId", DbType.Int64, donorDetails.OccupationId);
                this.dbServer.AddInParameter(storedProcCommand, "Photo", DbType.Binary, donorDetails.Photo);
                this.dbServer.AddInParameter(storedProcCommand, "IsLoyaltyMember", DbType.Boolean, donorDetails.IsLoyaltyMember);
                this.dbServer.AddInParameter(storedProcCommand, "LoyaltyCardID", DbType.Int64, donorDetails.LoyaltyCardID);
                this.dbServer.AddInParameter(storedProcCommand, "RelationID", DbType.Int64, donorDetails.RelationID);
                this.dbServer.AddInParameter(storedProcCommand, "ParentPatientID", DbType.Int64, donorDetails.ParentPatientID);
                this.dbServer.AddInParameter(storedProcCommand, "IssueDate", DbType.DateTime, donorDetails.IssueDate);
                this.dbServer.AddInParameter(storedProcCommand, "EffectiveDate", DbType.DateTime, donorDetails.EffectiveDate);
                this.dbServer.AddInParameter(storedProcCommand, "ExpiryDate", DbType.DateTime, donorDetails.ExpiryDate);
                if (donorDetails.LoyaltyCardNo != null)
                {
                    donorDetails.LoyaltyCardNo = donorDetails.LoyaltyCardNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "LoyaltyCardNo", DbType.String, donorDetails.LoyaltyCardNo);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, donorDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, donorDetails.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                this.dbServer.AddParameter(storedProcCommand, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "????????????????????");
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, donorDetails.GeneralDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "IsDonor", DbType.Boolean, donorDetails.IsDonor);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.DonorDetails.GeneralDetails.PatientID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                BizActionObj.DonorDetails.GeneralDetails.MRNo = (string) this.dbServer.GetParameterValue(storedProcCommand, "MRNo");
                string parameterValue = (string) this.dbServer.GetParameterValue(storedProcCommand, "Err");
                BizActionObj.DonorDetails.GeneralDetails.UnitId = UserVo.UserLoginInfo.UnitId;
                DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddDonorOtherDetails");
                this.dbServer.AddInParameter(command2, "ID", DbType.Int64, 0);
                this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command2, "PatientID", DbType.Int64, BizActionObj.DonorDetails.GeneralDetails.PatientID);
                this.dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command2, "SkinColorID", DbType.Int64, BizActionObj.DonorDetails.SkinColorID);
                this.dbServer.AddInParameter(command2, "HairColorID", DbType.Int64, BizActionObj.DonorDetails.HairColorID);
                this.dbServer.AddInParameter(command2, "EyeColorID", DbType.Int64, BizActionObj.DonorDetails.EyeColorID);
                this.dbServer.AddInParameter(command2, "DonorSourceID", DbType.Int64, BizActionObj.DonorDetails.DonorSourceID);
                this.dbServer.AddInParameter(command2, "DonorCode", DbType.String, BizActionObj.DonorDetails.DonorCode);
                this.dbServer.AddInParameter(command2, "Height", DbType.Double, BizActionObj.DonorDetails.Height);
                this.dbServer.AddInParameter(command2, "BoneStructure", DbType.String, BizActionObj.DonorDetails.BoneStructure);
                this.dbServer.AddInParameter(command2, "ReferralTypeID", DbType.Int64, donorDetails.GeneralDetails.ReferralTypeID);
                this.dbServer.AddInParameter(command2, "AgencyID", DbType.Int64, donorDetails.GeneralDetails.AgencyID);
                this.dbServer.ExecuteNonQuery(command2, transaction);
                DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddPatientSponsor");
                this.dbServer.AddInParameter(command3, "LinkServer", DbType.String, null);
                this.dbServer.AddInParameter(command3, "LinkServerAlias", DbType.String, null);
                this.dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.DonorDetails.GeneralDetails.PatientID);
                this.dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.DonorDetails.GeneralDetails.UnitId);
                this.dbServer.AddInParameter(command3, "PatientSourceID", DbType.Int64, BizActionObj.DonorDetails.GeneralDetails.PatientSourceID);
                this.dbServer.AddInParameter(command3, "CompanyID", DbType.Int64, BizActionObj.DonorDetails.GeneralDetails.CompanyID);
                this.dbServer.AddInParameter(command3, "AssociatedCompanyID", DbType.Int64, 0);
                this.dbServer.AddInParameter(command3, "ReferenceNo", DbType.String, null);
                this.dbServer.AddInParameter(command3, "CreditLimit", DbType.Double, 0);
                this.dbServer.AddInParameter(command3, "EffectiveDate", DbType.DateTime, donorDetails.EffectiveDate);
                this.dbServer.AddInParameter(command3, "ExpiryDate", DbType.DateTime, donorDetails.ExpiryDate);
                this.dbServer.AddInParameter(command3, "TariffID", DbType.Int64, donorDetails.TariffID);
                this.dbServer.AddInParameter(command3, "EmployeeNo", DbType.String, null);
                this.dbServer.AddInParameter(command3, "DesignationID", DbType.Int64, null);
                this.dbServer.AddInParameter(command3, "Remark", DbType.String, null);
                this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(command3, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command3, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(command3, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction.Commit();
                connection.Close();
                connection = null;
                transaction = null;
            }
            return BizActionObj;
        }

        public override IValueObject AddUpdateDonorBatch(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateDonorBatchBizActionVO nvo = valueObject as clsAddUpdateDonorBatchBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                clsSemenSampleBatchVO batchDetails = nvo.BatchDetails;
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddDonorBatchDetails");
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, batchDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, batchDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, batchDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "BatchCode", DbType.String, batchDetails.BatchCode);
                this.dbServer.AddInParameter(storedProcCommand, "InvoiceNo", DbType.String, batchDetails.InvoiceNo);
                this.dbServer.AddInParameter(storedProcCommand, "ReceivedByID", DbType.Int64, batchDetails.ReceivedByID);
                this.dbServer.AddInParameter(storedProcCommand, "ReceivedDate", DbType.DateTime, batchDetails.ReceivedDate);
                this.dbServer.AddInParameter(storedProcCommand, "LabID", DbType.Int64, batchDetails.LabID);
                this.dbServer.AddInParameter(storedProcCommand, "NoOfVails", DbType.Int32, batchDetails.NoOfVails);
                this.dbServer.AddInParameter(storedProcCommand, "Volume", DbType.Single, batchDetails.Volume);
                this.dbServer.AddInParameter(storedProcCommand, "AvailableVolume", DbType.Single, batchDetails.AvailableVolume);
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, batchDetails.Remark);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.BatchDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.BatchDetails.UnitID = UserVo.UserLoginInfo.UnitId;
                DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateSpremFreezing_New");
                this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.FreezingObj.ID);
                this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command2, "PatientID", DbType.Int64, batchDetails.PatientID);
                this.dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, batchDetails.PatientUnitID);
                this.dbServer.AddInParameter(command2, "BatchID", DbType.Int64, nvo.BatchDetails.ID);
                this.dbServer.AddInParameter(command2, "BatchUnitID", DbType.Int64, nvo.BatchDetails.UnitID);
                this.dbServer.AddInParameter(command2, "TherapyID", DbType.Int64, nvo.FreezingObj.TherapyID);
                this.dbServer.AddInParameter(command2, "TherapyUnitID", DbType.Int64, nvo.FreezingObj.TherapyUnitID);
                this.dbServer.AddInParameter(command2, "CycleCode", DbType.String, nvo.FreezingObj.CycleCode);
                this.dbServer.AddInParameter(command2, "DoctorID", DbType.Int64, nvo.FreezingObj.DoctorID);
                this.dbServer.AddInParameter(command2, "EmbryologistID", DbType.Int64, nvo.FreezingObj.EmbryologistID);
                this.dbServer.AddInParameter(command2, "SpremFreezingTime", DbType.DateTime, nvo.FreezingObj.SpremFreezingDate);
                this.dbServer.AddInParameter(command2, "SpremFreezingDate", DbType.DateTime, nvo.FreezingObj.SpremFreezingTime);
                this.dbServer.AddInParameter(command2, "CollectionMethodID", DbType.Int64, nvo.FreezingObj.CollectionMethodID);
                this.dbServer.AddInParameter(command2, "ViscosityID", DbType.Int64, nvo.FreezingObj.ViscosityID);
                this.dbServer.AddInParameter(command2, "CollectionProblem", DbType.String, nvo.FreezingObj.CollectionProblem);
                this.dbServer.AddInParameter(command2, "Other", DbType.String, nvo.FreezingObj.Other);
                this.dbServer.AddInParameter(command2, "Comments", DbType.String, nvo.FreezingObj.Comments);
                this.dbServer.AddInParameter(command2, "Abstience", DbType.String, nvo.FreezingObj.Abstience);
                this.dbServer.AddInParameter(command2, "Volume", DbType.Single, nvo.FreezingObj.Volume);
                this.dbServer.AddInParameter(command2, "Motility", DbType.Decimal, nvo.FreezingObj.Motility);
                this.dbServer.AddInParameter(command2, "GradeA", DbType.Decimal, nvo.FreezingObj.GradeA);
                this.dbServer.AddInParameter(command2, "GradeB", DbType.Decimal, nvo.FreezingObj.GradeB);
                this.dbServer.AddInParameter(command2, "GradeC", DbType.Decimal, nvo.FreezingObj.GradeC);
                this.dbServer.AddInParameter(command2, "TotalSpremCount", DbType.Int64, nvo.FreezingObj.TotalSpremCount);
                this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, nvo.FreezingObj.Status);
                this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(command2, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                nvo.FreezingObj.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                nvo.FreezingObj.UnitID = (long) this.dbServer.GetParameterValue(command2, "UnitID");
                foreach (clsNew_SpremFreezingVO gvo in nvo.FreezingDetailsList)
                {
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateSpremFreezingDetails_New");
                    this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, gvo.ID);
                    this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "SpremFreezingID", DbType.Int64, nvo.FreezingObj.ID);
                    this.dbServer.AddInParameter(command3, "SpremFreezingUnitID", DbType.Int64, nvo.FreezingObj.UnitID);
                    this.dbServer.AddInParameter(command3, "ColorCodeID", DbType.Int64, gvo.GobletColorID);
                    this.dbServer.AddInParameter(command3, "PlanTherapy", DbType.Int64, gvo.PlanTherapy);
                    this.dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, gvo.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command3, "StrawID", DbType.Int64, gvo.StrawId);
                    this.dbServer.AddInParameter(command3, "GlobletShapeID", DbType.Int64, gvo.GobletShapeId);
                    this.dbServer.AddInParameter(command3, "GlobletSizeID", DbType.Int64, gvo.GobletSizeId);
                    this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, gvo.Status);
                    this.dbServer.AddInParameter(command3, "IsModify", DbType.String, gvo.IsModify);
                    this.dbServer.AddInParameter(command3, "IsThaw", DbType.String, gvo.IsThaw);
                    this.dbServer.AddInParameter(command3, "CaneID", DbType.Int64, gvo.CanID);
                    this.dbServer.AddInParameter(command3, "CanisterID", DbType.Int64, gvo.CanisterId);
                    this.dbServer.AddInParameter(command3, "TankID", DbType.Int64, gvo.TankId);
                    this.dbServer.AddInParameter(command3, "Comments", DbType.String, gvo.Comments);
                    this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command3, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction.Commit();
                connection.Close();
                connection = null;
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject AddUpdateDonorDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            cls_NewAddUpdateDonorDetailsBizActionVO nvo = valueObject as cls_NewAddUpdateDonorDetailsBizActionVO;
            try
            {
                this.con = this.dbServer.CreateConnection();
                this.con.Open();
                this.trans = this.con.BeginTransaction();
                clsFemaleSemenDetailsVO donorDetails = nvo.DonorDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVFDAshBoard_AddDonorDetails");
                if (nvo.DonorDetails.ID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.DonorDetails.ID);
                }
                else
                {
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "IsEdit", DbType.Boolean, nvo.IsEdit);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.DonorDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.DonorDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.DonorDetails.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.DonorDetails.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "MethodOfSpermPreparation", DbType.Int64, nvo.DonorDetails.MethodOfSpermPreparation);
                this.dbServer.AddInParameter(storedProcCommand, "MOSP", DbType.Int64, nvo.DonorDetails.MOSP);
                this.dbServer.AddInParameter(storedProcCommand, "Company", DbType.String, nvo.DonorDetails.Company);
                this.dbServer.AddInParameter(storedProcCommand, "SourceOfNeedle", DbType.Int64, nvo.DonorDetails.SourceOfNeedle_1);
                this.dbServer.AddInParameter(storedProcCommand, "OoctyDonorUnitID", DbType.Int64, nvo.DonorDetails.OoctyDonorUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "OoctyDonorID", DbType.Int64, nvo.DonorDetails.OoctyDonorID);
                this.dbServer.AddInParameter(storedProcCommand, "OoctyDonorMrNo", DbType.String, nvo.DonorDetails.OoctyDonorMrNo);
                this.dbServer.AddInParameter(storedProcCommand, "SemenDonorMrNo", DbType.String, nvo.DonorDetails.SemenDonorMrNo);
                this.dbServer.AddInParameter(storedProcCommand, "SourceOfOoctye", DbType.Int64, nvo.DonorDetails.SourceOfOoctye_1);
                this.dbServer.AddInParameter(storedProcCommand, "SourceOfSemen", DbType.Int64, nvo.DonorDetails.SourceOfSemen_new);
                this.dbServer.AddInParameter(storedProcCommand, "SemenDonorID", DbType.Int64, nvo.DonorDetails.SemenDonorID);
                this.dbServer.AddInParameter(storedProcCommand, "SemenDonorUnitID", DbType.Int64, nvo.DonorDetails.SemenDonorUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PreSelfVolume", DbType.Int64, nvo.DonorDetails.PreSelfVolume_1);
                this.dbServer.AddInParameter(storedProcCommand, "PreSelfConcentration", DbType.Int64, nvo.DonorDetails.PreSelfConcentration_1);
                this.dbServer.AddInParameter(storedProcCommand, "PreSelfMotality", DbType.Int64, nvo.DonorDetails.PreSelfMotality_1);
                this.dbServer.AddInParameter(storedProcCommand, "PreSelfWBC", DbType.Int64, nvo.DonorDetails.PreSelfWBC_1);
                this.dbServer.AddInParameter(storedProcCommand, "PreDonorVolume", DbType.Int64, nvo.DonorDetails.PreDonorVolume_1);
                this.dbServer.AddInParameter(storedProcCommand, "PreDonorConcentration", DbType.Int64, nvo.DonorDetails.PreDonorConcentration_1);
                this.dbServer.AddInParameter(storedProcCommand, "PreDonorMotality", DbType.Int64, nvo.DonorDetails.PreDonorMotality_1);
                this.dbServer.AddInParameter(storedProcCommand, "PreDonorWBC", DbType.Int64, nvo.DonorDetails.PreDonorWBC_1);
                this.dbServer.AddInParameter(storedProcCommand, "PostSelfVolume", DbType.Int64, nvo.DonorDetails.PostSelfVolume_1);
                this.dbServer.AddInParameter(storedProcCommand, "PostSelfConcentration", DbType.Int64, nvo.DonorDetails.PostSelfConcentration_1);
                this.dbServer.AddInParameter(storedProcCommand, "PostSelfMotality", DbType.Int64, nvo.DonorDetails.PostSelfMotality_1);
                this.dbServer.AddInParameter(storedProcCommand, "PostSelfWBC", DbType.Int64, nvo.DonorDetails.PostSelfWBC_1);
                this.dbServer.AddInParameter(storedProcCommand, "PostDonorVolume", DbType.Int64, nvo.DonorDetails.PostDonorVolume_1);
                this.dbServer.AddInParameter(storedProcCommand, "PostDonorConcentration", DbType.Int64, nvo.DonorDetails.PostDonorConcentration_1);
                this.dbServer.AddInParameter(storedProcCommand, "PostDonorMotality", DbType.Int64, nvo.DonorDetails.PostDonorMotality_1);
                this.dbServer.AddInParameter(storedProcCommand, "PostDonorWBC", DbType.Int64, nvo.DonorDetails.PostDonorWBC_1);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "SemenSampleCode", DbType.String, nvo.DonorDetails.SemenSampleCode);
                this.dbServer.AddInParameter(storedProcCommand, "SemenSampleCodeSelf", DbType.String, nvo.DonorDetails.SemenSampleCodeSelf);
                this.dbServer.AddInParameter(storedProcCommand, "IsDonorFromModuleDonor", DbType.Boolean, nvo.DonorDetails.IsDonorFromModuleDonor);
                this.dbServer.ExecuteNonQuery(storedProcCommand, this.trans);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.DonorDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_IVFDAshBoard_AddUpdateDonorDetailsInPlanTherapy");
                this.dbServer.AddInParameter(command2, "PlanTherapyID", DbType.Int64, nvo.DonorDetails.PlanTherapyID);
                this.dbServer.AddInParameter(command2, "PlanTherapyUnitID", DbType.Int64, nvo.DonorDetails.PlanTherapyUnitID);
                long ooctyDonorID = nvo.DonorDetails.OoctyDonorID;
                this.dbServer.AddInParameter(command2, "IsOocyteDonorExists", DbType.Boolean, true);
                long semenDonorID = nvo.DonorDetails.SemenDonorID;
                this.dbServer.AddInParameter(command2, "IsSemenDonorExists", DbType.Boolean, true);
                this.dbServer.AddInParameter(command2, "OoctyDonorMrNo", DbType.String, nvo.DonorDetails.OoctyDonorMrNo);
                this.dbServer.AddInParameter(command2, "SemenDonorMrNo", DbType.String, nvo.DonorDetails.SemenDonorMrNo);
                this.dbServer.ExecuteNonQuery(command2, this.trans);
            }
            catch (Exception)
            {
                this.trans.Commit();
                this.con.Close();
                throw;
            }
            finally
            {
                this.trans.Commit();
                this.con.Close();
            }
            return nvo;
        }

        public override IValueObject AddUpdateDonorRegistrationDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateDonorBizActionVO bizActionObj = valueObject as clsAddUpdateDonorBizActionVO;
            bizActionObj = (bizActionObj.DonorDetails.GeneralDetails.PatientID != 0L) ? this.UpdateDonorDetails(bizActionObj, UserVo) : this.AddDonorDetails(bizActionObj, UserVo);
            return valueObject;
        }

        public override IValueObject AddUpdateRecievOocytesDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUpdateRecieceOocytesBizActionVO nvo = valueObject as clsIVFDashboard_AddUpdateRecieceOocytesBizActionVO;
            try
            {
                this.con = this.dbServer.CreateConnection();
                this.con.Open();
                this.trans = this.con.BeginTransaction();
                clsReceiveOocyteVO oPUDetails = nvo.OPUDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVFDAshBoard_AddUpdateRecieceOocytesDetails");
                if (nvo.OPUDetails.ID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.OPUDetails.ID);
                }
                else
                {
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "IsEdit", DbType.Boolean, nvo.IsEdit);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.OPUDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.OPUDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "TherapyID", DbType.Int64, nvo.OPUDetails.TherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "TherapyUnitID", DbType.Int64, nvo.OPUDetails.TherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "DonorID", DbType.Int64, nvo.OPUDetails.DonorID);
                this.dbServer.AddInParameter(storedProcCommand, "DonorUnitID", DbType.Int64, nvo.OPUDetails.DonorUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "DonorOPUID", DbType.Int64, nvo.OPUDetails.DonorOPUID);
                this.dbServer.AddInParameter(storedProcCommand, "DonorOPUUnitID", DbType.String, nvo.OPUDetails.DonorOPUUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "DonorOPUDate", DbType.String, nvo.OPUDetails.DonorOPUDate);
                this.dbServer.AddInParameter(storedProcCommand, "DonorOocyteRetrived", DbType.Int64, nvo.OPUDetails.DonorOocyteRetrived);
                this.dbServer.AddInParameter(storedProcCommand, "DonorBalancedOocyte", DbType.Int64, nvo.OPUDetails.DonorBalancedOocyte);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteConsumed", DbType.Int64, nvo.OPUDetails.OocyteConsumed);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Int64, nvo.OPUDetails.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, this.trans);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.OPUDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                this.trans.Commit();
                this.con.Close();
                throw;
            }
            finally
            {
                this.trans.Commit();
                this.con.Close();
            }
            return nvo;
        }

        public override IValueObject CheckDuplicasyDonorCodeAndBLab(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCheckDuplicasyDonorCodeAndBLabBizActionVO nvo = valueObject as clsCheckDuplicasyDonorCodeAndBLabBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVF_CheckDuplicasyDonorCodeAndBLab");
                this.dbServer.AddInParameter(storedProcCommand, "DonorCode", DbType.Int64, nvo.DonorCode);
                this.dbServer.AddInParameter(storedProcCommand, "LabID", DbType.Int64, nvo.LabID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.IsDuplicate = (bool) DALHelper.HandleDBNull(reader["IsDuplicate"]);
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject GetDetailsOfReceivedOocyte(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDetailsOfReceivedOocyteBizActionVO nvo = valueObject as clsGetDetailsOfReceivedOocyteBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDetailsOfReceivedOocyte");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.Details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.Details.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.TherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.TherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsReceiveOocyte", DbType.Boolean, nvo.Details.IsReceiveOocyte);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.Details.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        nvo.Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        nvo.Details.TherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyID"]));
                        nvo.Details.TherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyUnitID"]));
                        nvo.Details.DonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorID"]));
                        nvo.Details.DonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorUnitID"]));
                        nvo.Details.DonorOPUID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorOPUID"]));
                        nvo.Details.DonorOPUUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorOPUUnitID"]));
                        nvo.Details.DonorOPUDate = Convert.ToDateTime(DALHelper.HandleDate(reader["DonorOPUDate"]));
                        nvo.Details.DonorOocyteRetrived = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorOocyteRetrived"]));
                        nvo.Details.DonorBalancedOocyte = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorBalancedOocyte"]));
                        nvo.Details.OocyteConsumed = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteConsumed"]));
                        nvo.Details.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
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

        public override IValueObject GetDetailsOfReceivedOocyteEmbryo(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDetailsOfReceivedOocyteEmbryoBizActionVO nvo = valueObject as clsGetDetailsOfReceivedOocyteEmbryoBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashboard_AddDonatedOocyteEmbryoForRecepient");
                this.con = this.dbServer.CreateConnection();
                this.con.Open();
                this.trans = this.con.BeginTransaction();
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.Details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.Details.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.TherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.TherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, this.trans);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                this.trans.Rollback();
                throw;
            }
            finally
            {
                this.trans.Commit();
                this.con.Close();
                this.con = null;
                this.trans = null;
            }
            return valueObject;
        }

        public override IValueObject GetDetailsOfReceivedOocyteEmbryoFromDonorCycle(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDetailsOfReceivedOocyteEmbryoBizActionVO nvo = valueObject as clsGetDetailsOfReceivedOocyteEmbryoBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashboard_AddDonatedOocyteFromDonorCycle");
                this.con = this.dbServer.CreateConnection();
                this.con.Open();
                this.trans = this.con.BeginTransaction();
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.Details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.Details.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.TherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.TherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, this.trans);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                this.trans.Rollback();
                throw;
            }
            finally
            {
                this.trans.Commit();
                this.con.Close();
                this.con = null;
                this.trans = null;
            }
            return valueObject;
        }

        public override IValueObject GetDonorBatchDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDonorBatchDetailsBizActionVO nvo = valueObject as clsGetDonorBatchDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVF_DashBoardGetDonorBatchDetails");
                this.dbServer.AddInParameter(storedProcCommand, "DonorID", DbType.Int64, nvo.BatchDetails.DonorID);
                this.dbServer.AddInParameter(storedProcCommand, "DonorUnitID", DbType.Int64, nvo.BatchDetails.DonorUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.BatchDetailsList == null)
                    {
                        nvo.BatchDetailsList = new List<clsDonorBatchVO>();
                    }
                    while (reader.Read())
                    {
                        clsDonorBatchVO item = new clsDonorBatchVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            DonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorID"])),
                            DonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorUnitID"])),
                            DonorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DonorCode"])),
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            ReceivedByID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedByID"])),
                            ReceivedDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ReceivedDate"])),
                            InvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader["InvoiceNo"])),
                            NoOfVails = Convert.ToInt32(DALHelper.HandleDBNull(reader["NoOfVails"])),
                            Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"])),
                            LabID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabID"])),
                            ReceivedBy = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedBy"])),
                            Lab = Convert.ToString(DALHelper.HandleDBNull(reader["Lab"]))
                        };
                        nvo.BatchDetailsList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject GetDonorDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            cls_NewGetDonorDetailsBizActionVO nvo = valueObject as cls_NewGetDonorDetailsBizActionVO;
            this.con = this.dbServer.CreateConnection();
            this.con.Open();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVF_DashBoardGetDonorDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.DonorDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.DonorDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.DonorDetails.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.DonorDetails.PlanTherapyUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (!reader.HasRows)
                {
                    nvo.DonorDetails = null;
                }
                else
                {
                    while (reader.Read())
                    {
                        nvo.DonorDetails.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.DonorDetails.UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]);
                        nvo.DonorDetails.PatientID = (long) DALHelper.HandleDBNull(reader["PatientID"]);
                        nvo.DonorDetails.PatientUnitID = (long) DALHelper.HandleDBNull(reader["PatientUnitID"]);
                        nvo.DonorDetails.PlanTherapyUnitID = (long) DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]);
                        nvo.DonorDetails.PlanTherapyID = (long) DALHelper.HandleDBNull(reader["PlanTherapyID"]);
                        nvo.DonorDetails.SemenDonorMrNo = (string) DALHelper.HandleDBNull(reader["SemenDonorMrNo"]);
                        nvo.DonorDetails.OoctyDonorMrNo = (string) DALHelper.HandleDBNull(reader["OoctyDonorMrNo"]);
                        nvo.DonorDetails.MethodOfSpermPreparation = (long) DALHelper.HandleDBNull(reader["MethodOfSpermPreparation"]);
                        nvo.DonorDetails.SourceOfNeedle_1 = (long) DALHelper.HandleDBNull(reader["SourceOfNeedle"]);
                        nvo.DonorDetails.OoctyDonorUnitID = (long) DALHelper.HandleDBNull(reader["OoctyDonorUnitID"]);
                        nvo.DonorDetails.OoctyDonorID = (long) DALHelper.HandleDBNull(reader["OoctyDonorID"]);
                        nvo.DonorDetails.SourceOfOoctye_1 = (long) DALHelper.HandleDBNull(reader["SourceOfOoctye"]);
                        nvo.DonorDetails.SourceOfSemen_new = (long) DALHelper.HandleDBNull(reader["SourceOfSemen"]);
                        nvo.DonorDetails.SemenDonorID = (long) DALHelper.HandleDBNull(reader["SemenDonorID"]);
                        nvo.DonorDetails.SemenDonorUnitID = (long) DALHelper.HandleDBNull(reader["SemenDonorUnitID"]);
                        nvo.DonorDetails.Company = (string) DALHelper.HandleDBNull(reader["Company"]);
                        nvo.DonorDetails.SourceNeedle = (string) DALHelper.HandleDBNull(reader["SourceNeedle"]);
                        nvo.DonorDetails.SourceOocyte = (string) DALHelper.HandleDBNull(reader["SourceOocyte"]);
                        nvo.DonorDetails.SourceSemen = (string) DALHelper.HandleDBNull(reader["SourceSemen"]);
                        nvo.DonorDetails.PreSelfVolume_1 = (long) DALHelper.HandleDBNull(reader["PreSelfVolume"]);
                        nvo.DonorDetails.PreSelfConcentration_1 = (long) DALHelper.HandleDBNull(reader["PreSelfConcentration"]);
                        nvo.DonorDetails.PreSelfMotality_1 = (long) DALHelper.HandleDBNull(reader["PreSelfMotality"]);
                        nvo.DonorDetails.PreSelfWBC_1 = (long) DALHelper.HandleDBNull(reader["PreSelfWBC"]);
                        nvo.DonorDetails.PreDonorVolume_1 = (long) DALHelper.HandleDBNull(reader["PreDonorVolume"]);
                        nvo.DonorDetails.PreDonorConcentration_1 = (long) DALHelper.HandleDBNull(reader["PreDonorConcentration"]);
                        nvo.DonorDetails.PreDonorMotality_1 = (long) DALHelper.HandleDBNull(reader["PreDonorMotality"]);
                        nvo.DonorDetails.PreDonorWBC_1 = (long) DALHelper.HandleDBNull(reader["PreDonorWBC"]);
                        nvo.DonorDetails.PostSelfVolume_1 = (long) DALHelper.HandleDBNull(reader["PostSelfVolume"]);
                        nvo.DonorDetails.PostSelfConcentration_1 = (long) DALHelper.HandleDBNull(reader["PostSelfConcentration"]);
                        nvo.DonorDetails.PostSelfMotality_1 = (long) DALHelper.HandleDBNull(reader["PostSelfMotality"]);
                        nvo.DonorDetails.PostSelfWBC_1 = (long) DALHelper.HandleDBNull(reader["PostSelfWBC"]);
                        nvo.DonorDetails.PostDonorVolume_1 = (long) DALHelper.HandleDBNull(reader["PostDonorVolume"]);
                        nvo.DonorDetails.PostDonorConcentration_1 = (long) DALHelper.HandleDBNull(reader["PostDonorConcentration"]);
                        nvo.DonorDetails.PostDonorMotality_1 = (long) DALHelper.HandleDBNull(reader["PostDonorMotality"]);
                        nvo.DonorDetails.PostDonorWBC_1 = (long) DALHelper.HandleDBNull(reader["PostDonorWBC"]);
                        nvo.DonorDetails.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                        nvo.DonorDetails.MOSP = (string) DALHelper.HandleDBNull(reader["SourceNeedle"]);
                        nvo.DonorDetails.SemenSampleCode = (string) DALHelper.HandleDBNull(reader["SemenSampleCode"]);
                        nvo.DonorDetails.SemenSampleCodeSelf = (string) DALHelper.HandleDBNull(reader["SemenSampleCodeSelf"]);
                        nvo.DonorDetails.IsDonorFromModuleDonor = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDonorFromModuleDonor"]));
                        nvo.DonorDetails.IsSampleUsedInDay0 = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleUsedInDay0"]));
                    }
                }
                reader.Close();
            }
            catch (Exception exception)
            {
                this.con.Close();
                throw exception;
            }
            finally
            {
                this.con.Close();
            }
            return nvo;
        }

        public override IValueObject GetDonorDetailsAgainstSearch(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDonorDetailsAgainstSearchBizActionVO nvo = valueObject as clsGetDonorDetailsAgainstSearchBizActionVO;
            this.con = this.dbServer.CreateConnection();
            this.con.Open();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVF_DashBoardGetDonorDetailsAgainstSearch");
                this.dbServer.AddInParameter(storedProcCommand, "DonorCode", DbType.String, nvo.DonorGeneralDetails.DonorCode);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DonorGeneralDetailsList == null)
                    {
                        nvo.DonorGeneralDetailsList = new List<clsDonorGeneralDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsDonorGeneralDetailsVO item = new clsDonorGeneralDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            DonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorID"])),
                            DonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorUnitID"])),
                            DonorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DonorCode"])),
                            Eye = Convert.ToString(DALHelper.HandleDBNull(reader["Eye"])),
                            Skin = Convert.ToString(DALHelper.HandleDBNull(reader["Skin"])),
                            Hair = Convert.ToString(DALHelper.HandleDBNull(reader["Hair"])),
                            BloodGroup = Convert.ToString(DALHelper.HandleDBNull(reader["Bloodgroup"])),
                            Height = Convert.ToSingle(DALHelper.HandleDBNull(reader["Height"])),
                            BoneStructure = Convert.ToString(DALHelper.HandleDBNull(reader["BoneStructure"])),
                            MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"])),
                            LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])),
                            FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])),
                            MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"])),
                            DateOfBirth = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["DateofBirth"]))),
                            RegistrationDate = Convert.ToDateTime(DALHelper.HandleDate(reader["RegistrationDate"])),
                            TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"])),
                            PatientSourceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceID"])),
                            CompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompanyID"])),
                            ContactNO1 = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNO1"])),
                            MaritalStatus = Convert.ToString(DALHelper.HandleDBNull(reader["MaritalStatus"])),
                            UniversalID = Security.base64Decode((string) DALHelper.HandleDBNull(reader["CivilID"])),
                            PatientTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientCategoryID"]))
                        };
                        nvo.DonorGeneralDetailsList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                this.con.Close();
                throw;
            }
            finally
            {
                this.con.Close();
            }
            return nvo;
        }

        public override IValueObject GetDonorDetailsForIUI(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDonorDetailsForIUIBizActionVO nvo = valueObject as clsGetDonorDetailsForIUIBizActionVO;
            this.con = this.dbServer.CreateConnection();
            this.con.Open();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVF_DashBoardGetDonorDetailsForIUI");
                this.dbServer.AddInParameter(storedProcCommand, "DonorID", DbType.Int64, nvo.BatchDetails.DonorID);
                this.dbServer.AddInParameter(storedProcCommand, "DonorUnitID", DbType.Int64, nvo.BatchDetails.DonorUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "BatchID", DbType.Int64, nvo.BatchDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "BatchUnitID", DbType.Int64, nvo.BatchDetails.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.BatchDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.BatchDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.BatchDetails.DonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorID"]));
                        nvo.BatchDetails.DonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorUnitID"]));
                        nvo.BatchDetails.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        nvo.BatchDetails.DonorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DonorCode"]));
                        nvo.BatchDetails.Lab = Convert.ToString(DALHelper.HandleDBNull(reader["Lab"]));
                        nvo.BatchDetails.Eye = Convert.ToString(DALHelper.HandleDBNull(reader["Eye"]));
                        nvo.BatchDetails.Skin = Convert.ToString(DALHelper.HandleDBNull(reader["Skin"]));
                        nvo.BatchDetails.Hair = Convert.ToString(DALHelper.HandleDBNull(reader["Hair"]));
                        nvo.BatchDetails.BloodGroup = Convert.ToString(DALHelper.HandleDBNull(reader["BloodGroup"]));
                        nvo.BatchDetails.Height = Convert.ToSingle(DALHelper.HandleDBNull(reader["Height"]));
                        nvo.BatchDetails.BoneStructure = Convert.ToString(DALHelper.HandleDBNull(reader["BoneStructure"]));
                    }
                }
            }
            catch (Exception)
            {
                this.con.Close();
                throw;
            }
            finally
            {
                this.con.Close();
            }
            return nvo;
        }

        public override IValueObject GetDonorDetailsToModify(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDonorDetailsBizActionVO nvo = valueObject as clsGetDonorDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDonorDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.DonorDetails.GeneralDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.DonorDetails.GeneralDetails.PatientUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.DonorDetails.GeneralDetails.PatientTypeID = (long) DALHelper.HandleDBNull(reader["PatientCategoryID"]);
                        nvo.DonorDetails.GeneralDetails.ReferralTypeID = (long) DALHelper.HandleDBNull(reader["ReferralTypeID"]);
                        nvo.DonorDetails.CompanyName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["CompanyName"]));
                        nvo.DonorDetails.GeneralDetails.PatientID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.DonorDetails.GeneralDetails.FirstName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["FirstName"]));
                        nvo.DonorDetails.GeneralDetails.MiddleName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["MiddleName"]));
                        nvo.DonorDetails.GeneralDetails.LastName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["LastName"]));
                        nvo.DonorDetails.GeneralDetails.MRNo = (string) DALHelper.HandleDBNull(reader["MRNo"]);
                        nvo.DonorDetails.GeneralDetails.RegistrationDate = DALHelper.HandleDate(reader["RegistrationDate"]);
                        nvo.DonorDetails.GeneralDetails.DateOfBirth = DALHelper.HandleDate(reader["DateOfBirth"]);
                        nvo.DonorDetails.GeneralDetails.PatientSourceID = (long) DALHelper.HandleDBNull(reader["PatientSourceID"]);
                        nvo.DonorDetails.GeneralDetails.TariffID = (long) DALHelper.HandleDBNull(reader["TariffID"]);
                        nvo.DonorDetails.FamilyName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["FamilyName"]));
                        nvo.DonorDetails.GenderID = (long) DALHelper.HandleDBNull(reader["GenderID"]);
                        nvo.DonorDetails.BloodGroupID = (long) DALHelper.HandleDBNull(reader["BloodGroupID"]);
                        nvo.DonorDetails.MaritalStatusID = (long) DALHelper.HandleDBNull(reader["MaritalStatusID"]);
                        nvo.DonorDetails.CivilID = Security.base64Decode((string) DALHelper.HandleDBNull(reader["CivilID"]));
                        nvo.DonorDetails.ContactNo1 = (string) DALHelper.HandleDBNull(reader["ContactNo1"]);
                        nvo.DonorDetails.ContactNo2 = (string) DALHelper.HandleDBNull(reader["ContactNo2"]);
                        nvo.DonorDetails.FaxNo = (string) DALHelper.HandleDBNull(reader["FaxNo"]);
                        nvo.DonorDetails.Email = Security.base64Decode((string) DALHelper.HandleDBNull(reader["Email"]));
                        nvo.DonorDetails.AddressLine1 = Security.base64Decode((string) DALHelper.HandleDBNull(reader["AddressLine1"]));
                        nvo.DonorDetails.AddressLine2 = Security.base64Decode((string) DALHelper.HandleDBNull(reader["AddressLine2"]));
                        nvo.DonorDetails.AddressLine3 = Security.base64Decode((string) DALHelper.HandleDBNull(reader["AddressLine3"]));
                        nvo.DonorDetails.Country = Security.base64Decode((string) DALHelper.HandleDBNull(reader["Country"]));
                        nvo.DonorDetails.State = Security.base64Decode((string) DALHelper.HandleDBNull(reader["State"]));
                        nvo.DonorDetails.City = Security.base64Decode((string) DALHelper.HandleDBNull(reader["City"]));
                        nvo.DonorDetails.Taluka = Security.base64Decode((string) DALHelper.HandleDBNull(reader["Taluka"]));
                        nvo.DonorDetails.Area = Security.base64Decode((string) DALHelper.HandleDBNull(reader["Area"]));
                        nvo.DonorDetails.District = Security.base64Decode((string) DALHelper.HandleDBNull(reader["District"]));
                        nvo.DonorDetails.MobileCountryCode = Convert.ToString(DALHelper.HandleDBNull(reader["MobileCountryCode"]));
                        nvo.DonorDetails.ResiNoCountryCode = (long) DALHelper.HandleDBNull(reader["ResiNoCountryCode"]);
                        nvo.DonorDetails.ResiSTDCode = (long) DALHelper.HandleDBNull(reader["ResiSTDCode"]);
                        nvo.DonorDetails.CountryID = DALHelper.HandleIntegerNull(reader["CountryID"]);
                        nvo.DonorDetails.StateID = DALHelper.HandleIntegerNull(reader["StateID"]);
                        nvo.DonorDetails.CityID = DALHelper.HandleIntegerNull(reader["CityID"]);
                        nvo.DonorDetails.RegionID = DALHelper.HandleIntegerNull(reader["RegionID"]);
                        nvo.DonorDetails.Pincode = Security.base64Decode((string) DALHelper.HandleDBNull(reader["Pincode"]));
                        nvo.DonorDetails.ReligionID = (long) DALHelper.HandleDBNull(reader["ReligionID"]);
                        nvo.DonorDetails.OccupationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["OccupationId"]));
                        if (((byte[]) DALHelper.HandleDBNull(reader["Photo"])) != null)
                        {
                            nvo.DonorDetails.Photo = (byte[]) DALHelper.HandleDBNull(reader["Photo"]);
                        }
                        if (nvo.DonorDetails.GeneralDetails.Photo != null)
                        {
                            nvo.DonorDetails.GeneralDetails.Photo = (byte[]) DALHelper.HandleDBNull(reader["Photo"]);
                        }
                        nvo.DonorDetails.IsLoyaltyMember = (bool) DALHelper.HandleDBNull(reader["IsLoyaltyMember"]);
                        nvo.DonorDetails.LoyaltyCardID = (long?) DALHelper.HandleDBNull(reader["LoyaltyCardID"]);
                        nvo.DonorDetails.IssueDate = DALHelper.HandleDate(reader["IssueDate"]);
                        nvo.DonorDetails.EffectiveDate = DALHelper.HandleDate(reader["EffectiveDate"]);
                        nvo.DonorDetails.ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]);
                        nvo.DonorDetails.LoyaltyCardNo = (string) DALHelper.HandleDBNull(reader["LoyaltyCardNo"]);
                        nvo.DonorDetails.RelationID = (long) DALHelper.HandleDBNull(reader["RelationID"]);
                        nvo.DonorDetails.ParentPatientID = (long) DALHelper.HandleDBNull(reader["ParentPatientID"]);
                        nvo.DonorDetails.GeneralDetails.UnitId = (long) DALHelper.HandleDBNull(reader["UnitId"]);
                        nvo.DonorDetails.SkinColorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SkinColorID"]));
                        nvo.DonorDetails.HairColorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["HairColorID"]));
                        nvo.DonorDetails.EyeColorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EyeColorID"]));
                        nvo.DonorDetails.DonorSourceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorSourceID"]));
                        nvo.DonorDetails.DonorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DonorCode"]));
                        nvo.DonorDetails.Height = Convert.ToSingle(DALHelper.HandleDBNull(reader["Height"]));
                        nvo.DonorDetails.BoneStructure = Convert.ToString(DALHelper.HandleDBNull(reader["BoneStructure"]));
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject GetDonorList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDonorListBizActionVO nvo = valueObject as clsGetDonorListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDonorList");
                this.dbServer.AddInParameter(storedProcCommand, "DonorCode", DbType.String, nvo.DonorDetails.DonorCode);
                this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, nvo.DonorDetails.AgencyID);
                this.dbServer.AddInParameter(storedProcCommand, "HairColorID", DbType.Int64, nvo.DonorDetails.HairColorID);
                this.dbServer.AddInParameter(storedProcCommand, "SkinColorID", DbType.Int64, nvo.DonorDetails.SkinColorID);
                this.dbServer.AddInParameter(storedProcCommand, "EyeColorID", DbType.Int64, nvo.DonorDetails.EyeColorID);
                this.dbServer.AddInParameter(storedProcCommand, "BloodGroupID", DbType.Int64, nvo.DonorDetails.BloodGroupID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DonorGeneralDetailsList == null)
                    {
                        nvo.DonorGeneralDetailsList = new List<clsPatientVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientVO item = new clsPatientVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            DonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorID"])),
                            DonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorUnitID"])),
                            DonorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DonorCode"])),
                            Eye = Convert.ToString(DALHelper.HandleDBNull(reader["Eye"])),
                            Skin = Convert.ToString(DALHelper.HandleDBNull(reader["Skin"])),
                            Hair = Convert.ToString(DALHelper.HandleDBNull(reader["Hair"])),
                            BloodGroup = Convert.ToString(DALHelper.HandleDBNull(reader["Bloodgroup"])),
                            Height = Convert.ToSingle(DALHelper.HandleDBNull(reader["Height"])),
                            BoneStructure = Convert.ToString(DALHelper.HandleDBNull(reader["BoneStructure"])),
                            MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"])),
                            LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])),
                            FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])),
                            MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"])),
                            DateOfBirth = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["DateofBirth"]))),
                            RegistrationDate = Convert.ToDateTime(DALHelper.HandleDate(reader["RegistrationDate"])),
                            TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"])),
                            PatientSourceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceID"])),
                            CompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompanyID"])),
                            ContactNO1 = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNO1"])),
                            MaritalStatus = Convert.ToString(DALHelper.HandleDBNull(reader["MaritalStatus"])),
                            UniversalID = Security.base64Decode((string) DALHelper.HandleDBNull(reader["CivilID"])),
                            PatientTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientCategoryID"])),
                            ReferralTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferralTypeID"])),
                            AgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgencyID"])),
                            ReferralName = Convert.ToString(DALHelper.HandleDBNull(reader["ReferralName"])),
                            AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"])),
                            SkinColorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SkinColorID"])),
                            HairColorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["HairColorID"])),
                            EyeColorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EyeColorID"])),
                            BloodGroupID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BloodGroupID"]))
                        };
                        nvo.DonorGeneralDetailsList.Add(item);
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
            return valueObject;
        }

        public override IValueObject GetOPUDate(IValueObject valueObject, clsUserVO UserVo)
        {
            GetIVFDashboardOPUDateBizActionVO nvo = valueObject as GetIVFDashboardOPUDateBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDetailsOfOPUDate");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsIVFDashboard_OPUVO>();
                    }
                    while (reader.Read())
                    {
                        clsIVFDashboard_OPUVO item = new clsIVFDashboard_OPUVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            OPUID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            OocyteRetrived = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteRetrived"])),
                            BalanceOocyte = Convert.ToInt64(DALHelper.HandleDBNull(reader["BalanceOocyte"]))
                        };
                        nvo.List.Add(item);
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

        public override IValueObject GetSemenBatchAndSpermiogram(IValueObject valueObject, clsUserVO UserVo)
        {
            cls_GetSemenBatchAndSpermiogramBizActionVO nvo = valueObject as cls_GetSemenBatchAndSpermiogramBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSemenBatchList");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.String, nvo.Details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.Details.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DetailsList == null)
                    {
                        nvo.DetailsList = new List<clsBatchAndSpemFreezingVO>();
                    }
                    while (reader.Read())
                    {
                        clsBatchAndSpemFreezingVO item = new clsBatchAndSpemFreezingVO {
                            MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"])),
                            DonorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DonorCode"])),
                            Abstience = Convert.ToString(DALHelper.HandleDBNull(reader["Abstience"])),
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"])),
                            BatchUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchUnitID"])),
                            Cane = Convert.ToString(DALHelper.HandleDBNull(reader["Cane"])),
                            CaneID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CaneID"])),
                            Canister = Convert.ToString(DALHelper.HandleDBNull(reader["Canister"])),
                            CanisterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CanisterID"])),
                            ColorCode = Convert.ToString(DALHelper.HandleDBNull(reader["ColorCode"])),
                            ColorCodeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ColorCodeID"])),
                            Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"])),
                            DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"])),
                            EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"])),
                            GlobletShape = Convert.ToString(DALHelper.HandleDBNull(reader["GlobletShape"])),
                            GlobletShapeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GlobletShapeID"])),
                            GlobletSize = Convert.ToString(DALHelper.HandleDBNull(reader["GlobletSize"])),
                            GlobletSizeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GlobletSizeID"])),
                            GradeA = Convert.ToDecimal(DALHelper.HandleDBNull(reader["GradeA"])),
                            GradeB = Convert.ToDecimal(DALHelper.HandleDBNull(reader["GradeB"])),
                            GradeC = Convert.ToDecimal(DALHelper.HandleDBNull(reader["GradeC"])),
                            InvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader["InvoiceNo"])),
                            IsThaw = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsThaw"])),
                            Lab = Convert.ToString(DALHelper.HandleDBNull(reader["Lab"])),
                            LabID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabID"])),
                            Motility = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Motility"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"])),
                            PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"])),
                            PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"])),
                            ReceivedByID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedByID"])),
                            ReceivedByName = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedByName"])),
                            ReceivedDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ReceivedDate"])),
                            SpermFreezingDetailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpermFreezingDetailsID"])),
                            SpermFreezingID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpermFreezingID"])),
                            SpermFreezingUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpermFreezingUnitID"])),
                            SpremFreezingDate = Convert.ToDateTime(DALHelper.HandleDate(reader["SpremFreezingDate"])),
                            SpermFreezingDetailsUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpermFreezingDetailsUnitID"])),
                            SpremFreezingTime = Convert.ToDateTime(DALHelper.HandleDate(reader["SpremFreezingTime"])),
                            SpremNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpremNo"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            Straw = Convert.ToString(DALHelper.HandleDBNull(reader["Straw"])),
                            StrawID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StrawID"])),
                            Tank = Convert.ToString(DALHelper.HandleDBNull(reader["Tank"])),
                            TankID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TankID"])),
                            Viscosity = Convert.ToString(DALHelper.HandleDBNull(reader["Viscosity"])),
                            ViscosityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ViscosityID"])),
                            Volume = Convert.ToSingle(DALHelper.HandleDBNull(reader["Volume"])),
                            CollectionMethodID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CollectionMethodID"])),
                            CollectionProblem = Convert.ToString(DALHelper.HandleDBNull(reader["CollectionProblem"])),
                            FreezingOther = Convert.ToString(DALHelper.HandleDBNull(reader["FreezingOther"])),
                            FreezingComments = Convert.ToString(DALHelper.HandleDBNull(reader["FreezingComments"])),
                            TotalSpremCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["TotalSpremCount"]))
                        };
                        nvo.DetailsList.Add(item);
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

        private clsAddUpdateDonorBizActionVO UpdateDonorDetails(clsAddUpdateDonorBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                clsPatientVO donorDetails = BizActionObj.DonorDetails;
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePatient");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, donorDetails.GeneralDetails.LinkServer);
                if (donorDetails.GeneralDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, donorDetails.GeneralDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "PatientCategoryID", DbType.Int64, donorDetails.GeneralDetails.PatientTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "ReferralTypeID", DbType.Int64, donorDetails.GeneralDetails.ReferralTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyName", DbType.String, Security.base64Encode(donorDetails.CompanyName));
                this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, donorDetails.GeneralDetails.MRNo);
                this.dbServer.AddInParameter(storedProcCommand, "RegistrationDate", DbType.DateTime, donorDetails.GeneralDetails.RegistrationDate);
                if (donorDetails.GeneralDetails.LastName != null)
                {
                    donorDetails.GeneralDetails.LastName = donorDetails.GeneralDetails.LastName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, Security.base64Encode(donorDetails.GeneralDetails.LastName));
                if (donorDetails.GeneralDetails.FirstName != null)
                {
                    donorDetails.GeneralDetails.FirstName = donorDetails.GeneralDetails.FirstName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, Security.base64Encode(donorDetails.GeneralDetails.FirstName));
                if (donorDetails.GeneralDetails.MiddleName != null)
                {
                    donorDetails.GeneralDetails.MiddleName = donorDetails.GeneralDetails.MiddleName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, Security.base64Encode(donorDetails.GeneralDetails.MiddleName));
                if (donorDetails.FamilyName != null)
                {
                    donorDetails.FamilyName = donorDetails.FamilyName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "FamilyName", DbType.String, Security.base64Encode(donorDetails.FamilyName));
                this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, donorDetails.GenderID);
                this.dbServer.AddInParameter(storedProcCommand, "DateOfBirth", DbType.DateTime, donorDetails.GeneralDetails.DateOfBirth);
                this.dbServer.AddInParameter(storedProcCommand, "BloodGroupID", DbType.Int64, donorDetails.BloodGroupID);
                this.dbServer.AddInParameter(storedProcCommand, "MaritalStatusID", DbType.Int64, donorDetails.MaritalStatusID);
                this.dbServer.AddInParameter(storedProcCommand, "CivilID", DbType.String, Security.base64Encode(donorDetails.CivilID));
                if (donorDetails.ContactNo1 != null)
                {
                    donorDetails.ContactNo1 = donorDetails.ContactNo1.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo1", DbType.String, donorDetails.ContactNo1);
                if (donorDetails.ContactNo2 != null)
                {
                    donorDetails.ContactNo2 = donorDetails.ContactNo2.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo2", DbType.String, donorDetails.ContactNo2);
                if (donorDetails.FaxNo != null)
                {
                    donorDetails.FaxNo = donorDetails.FaxNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "FaxNo", DbType.String, donorDetails.FaxNo);
                if (donorDetails.Email != null)
                {
                    donorDetails.Email = donorDetails.Email.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Email", DbType.String, Security.base64Encode(donorDetails.Email));
                if (donorDetails.AddressLine1 != null)
                {
                    donorDetails.AddressLine1 = donorDetails.AddressLine1.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine1", DbType.String, Security.base64Encode(donorDetails.AddressLine1));
                if (donorDetails.AddressLine2 != null)
                {
                    donorDetails.AddressLine2 = donorDetails.AddressLine2.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine2", DbType.String, Security.base64Encode(donorDetails.AddressLine2));
                if (donorDetails.AddressLine3 != null)
                {
                    donorDetails.AddressLine3 = donorDetails.AddressLine3.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine3", DbType.String, Security.base64Encode(donorDetails.AddressLine3));
                this.dbServer.AddInParameter(storedProcCommand, "Country", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "State", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "City", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "Taluka", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "Area", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "District", DbType.String, null);
                if (donorDetails.CountryID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CountryID", DbType.Int64, donorDetails.CountryID);
                }
                if (donorDetails.StateID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "StateID", DbType.Int64, donorDetails.StateID);
                }
                if (donorDetails.CityID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CityID", DbType.Int64, donorDetails.CityID);
                }
                if (donorDetails.RegionID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "RegionID", DbType.Int64, donorDetails.RegionID);
                }
                this.dbServer.AddInParameter(storedProcCommand, "MobileCountryCode", DbType.Int64, donorDetails.MobileCountryCode);
                this.dbServer.AddInParameter(storedProcCommand, "ResiNoCountryCode", DbType.Int64, donorDetails.ResiNoCountryCode);
                this.dbServer.AddInParameter(storedProcCommand, "ResiSTDCode", DbType.Int64, donorDetails.ResiSTDCode);
                if (donorDetails.Pincode != null)
                {
                    donorDetails.Pincode = donorDetails.Pincode.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Pincode", DbType.String, Security.base64Encode(donorDetails.Pincode));
                this.dbServer.AddInParameter(storedProcCommand, "ReligionID", DbType.Int64, donorDetails.ReligionID);
                this.dbServer.AddInParameter(storedProcCommand, "OccupationId", DbType.Int64, donorDetails.OccupationId);
                this.dbServer.AddInParameter(storedProcCommand, "Photo", DbType.Binary, donorDetails.Photo);
                this.dbServer.AddInParameter(storedProcCommand, "IsLoyaltyMember", DbType.Boolean, donorDetails.IsLoyaltyMember);
                this.dbServer.AddInParameter(storedProcCommand, "LoyaltyCardID", DbType.Int64, donorDetails.LoyaltyCardID);
                this.dbServer.AddInParameter(storedProcCommand, "IssueDate", DbType.DateTime, donorDetails.IssueDate);
                this.dbServer.AddInParameter(storedProcCommand, "EffectiveDate", DbType.DateTime, donorDetails.EffectiveDate);
                this.dbServer.AddInParameter(storedProcCommand, "ExpiryDate", DbType.DateTime, donorDetails.ExpiryDate);
                if (donorDetails.LoyaltyCardNo != null)
                {
                    donorDetails.LoyaltyCardNo = donorDetails.LoyaltyCardNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "LoyaltyCardNo", DbType.String, donorDetails.LoyaltyCardNo);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, donorDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, donorDetails.GeneralDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, donorDetails.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, donorDetails.GeneralDetails.PatientID);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddDonorOtherDetails");
                this.dbServer.AddInParameter(command2, "ID", DbType.Int64, 0);
                this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command2, "PatientID", DbType.Int64, donorDetails.GeneralDetails.PatientID);
                this.dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, donorDetails.GeneralDetails.PatientUnitID);
                this.dbServer.AddInParameter(command2, "SkinColorID", DbType.Int64, BizActionObj.DonorDetails.SkinColorID);
                this.dbServer.AddInParameter(command2, "HairColorID", DbType.Int64, BizActionObj.DonorDetails.HairColorID);
                this.dbServer.AddInParameter(command2, "EyeColorID", DbType.Int64, BizActionObj.DonorDetails.EyeColorID);
                this.dbServer.AddInParameter(command2, "DonorSourceID", DbType.Int64, BizActionObj.DonorDetails.DonorSourceID);
                this.dbServer.AddInParameter(command2, "DonorCode", DbType.String, BizActionObj.DonorDetails.DonorCode);
                this.dbServer.AddInParameter(command2, "Height", DbType.Double, BizActionObj.DonorDetails.Height);
                this.dbServer.AddInParameter(command2, "BoneStructure", DbType.String, BizActionObj.DonorDetails.BoneStructure);
                this.dbServer.AddInParameter(command2, "ReferralTypeID", DbType.Int64, donorDetails.GeneralDetails.ReferralTypeID);
                this.dbServer.AddInParameter(command2, "AgencyID", DbType.Int64, donorDetails.GeneralDetails.AgencyID);
                this.dbServer.ExecuteNonQuery(command2, transaction);
            }
            catch (Exception)
            {
                transaction.Rollback();
                transaction.Commit();
                connection.Close();
                throw;
            }
            finally
            {
                transaction.Commit();
                connection.Close();
            }
            return BizActionObj;
        }
    }
}

