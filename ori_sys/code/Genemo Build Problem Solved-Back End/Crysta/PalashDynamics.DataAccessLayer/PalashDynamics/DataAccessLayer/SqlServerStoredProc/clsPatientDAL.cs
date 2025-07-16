namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Billing;
    using PalashDynamics.ValueObjects.IPD;
    using PalashDynamics.ValueObjects.OutPatientDepartment;
    using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
    using PalashDynamics.ValueObjects.Patient;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;

    public class clsPatientDAL : clsBasePatientDAL
    {
        private Database dbServer;
        private LogManager logManager;
        private string ImgIP = string.Empty;
        private string ImgVirtualDir = string.Empty;
        private string ImgSaveLocation = string.Empty;
        private string DocImgIP = string.Empty;
        private string DocImgVirtualDir = string.Empty;
        private string DocSaveLocation = string.Empty;

        private clsPatientDAL()
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
                this.ImgIP = ConfigurationManager.AppSettings["RegImgIP"];
                this.ImgVirtualDir = ConfigurationManager.AppSettings["RegImgVirtualDir"];
                this.ImgSaveLocation = ConfigurationManager.AppSettings["ImgSavingLocation"];
                this.DocImgIP = ConfigurationManager.AppSettings["DocImgIP"];
                this.DocImgVirtualDir = ConfigurationManager.AppSettings["DocImgVirtualDir"];
                this.DocSaveLocation = ConfigurationManager.AppSettings["DocSavingLocation"];
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
        }

        public override IValueObject AddBarcodeImageTODB(IValueObject valueObject, clsUserVO UserVo)
        {
            AddBarcodeImageBizActionVO nvo = valueObject as AddBarcodeImageBizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddBarcodeImage");
                if (nvo.ObjBarcodeImage.GeneralDetailsBarcodeImage != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "GeneralDetailsBarcodeImage", DbType.Binary, nvo.ObjBarcodeImage.GeneralDetailsBarcodeImage);
                }
                if (nvo.ObjBarcodeImage.SpouseBarcodeImage != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "SpouseBarcodeImage", DbType.Binary, nvo.ObjBarcodeImage.SpouseBarcodeImage);
                }
                if (nvo.ObjBarcodeImage.GeneralDetailsMRNo != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "GeneralDetailsMRno", DbType.String, nvo.ObjBarcodeImage.GeneralDetailsMRNo);
                }
                if (nvo.ObjBarcodeImage.SpouseMRNo != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "SpouseMRno", DbType.String, nvo.ObjBarcodeImage.SpouseMRNo);
                }
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        private clsAddPrintedPatientConscentBizActionVO AddConsent(clsAddPrintedPatientConscentBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsPatientConsentVO consentDetails = BizActionObj.ConsentDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPrintedPatientConscent");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, consentDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, consentDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "ConscentID", DbType.Int64, consentDetails.ConsentID);
                this.dbServer.AddInParameter(storedProcCommand, "Template", DbType.String, consentDetails.Template);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, consentDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
                BizActionObj.ConsentDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        private clsAddPatientDietPlanBizActionVO AddDiet(clsAddPatientDietPlanBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                clsPatientDietPlanVO dietPlan = BizActionObj.DietPlan;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatientDietPlan");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, dietPlan.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, dietPlan.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, dietPlan.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitDoctorID", DbType.Int64, dietPlan.VisitDoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, dietPlan.Date);
                this.dbServer.AddInParameter(storedProcCommand, "PlanID", DbType.Int64, dietPlan.PlanID);
                this.dbServer.AddInParameter(storedProcCommand, "GeneralInformation", DbType.String, dietPlan.GeneralInformation);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, dietPlan.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.DietPlan.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if ((dietPlan.DietDetails != null) && (dietPlan.DietDetails.Count > 0))
                {
                    foreach (clsPatientDietPlanDetailVO lvo in dietPlan.DietDetails)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddPatientDietPlanDetails");
                        this.dbServer.AddInParameter(command2, "DietPlanID", DbType.Int64, dietPlan.ID);
                        this.dbServer.AddInParameter(command2, "FoodItemID", DbType.Int64, lvo.FoodItemID);
                        this.dbServer.AddInParameter(command2, "FoodCategoryID", DbType.Int64, lvo.FoodItemCategoryID);
                        this.dbServer.AddInParameter(command2, "Timing", DbType.String, lvo.Timing);
                        this.dbServer.AddInParameter(command2, "FoodQty", DbType.String, lvo.FoodQty);
                        this.dbServer.AddInParameter(command2, "FoodUnit", DbType.String, lvo.FoodUnit);
                        this.dbServer.AddInParameter(command2, "FoodCal", DbType.String, lvo.FoodCal);
                        this.dbServer.AddInParameter(command2, "FoodCH", DbType.String, lvo.FoodCH);
                        this.dbServer.AddInParameter(command2, "FoodFat", DbType.String, lvo.FoodFat);
                        this.dbServer.AddInParameter(command2, "FoodExpectedCal", DbType.String, lvo.FoodExpectedCal);
                        this.dbServer.AddInParameter(command2, "FoodInstruction", DbType.String, lvo.FoodInstruction);
                        this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, lvo.ID);
                        this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command2, transaction);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                        lvo.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                    }
                }
                transaction.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.DietPlan = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return BizActionObj;
        }

        public override IValueObject AddDonarCode(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddDonorCodeBizActionVO nvo = (clsAddDonorCodeBizActionVO) valueObject;
            DbTransaction transaction = null;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                clsPatientVO patientDetails = nvo.PatientDetails;
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddDonarCodeToPatient");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, patientDetails.GeneralDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, patientDetails.GeneralDetails.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, patientDetails.GeneralDetails.MRNo);
                this.dbServer.AddInParameter(storedProcCommand, "DonarCode", DbType.String, patientDetails.GeneralDetails.DonarCode);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw exception;
            }
            finally
            {
                transaction.Dispose();
                transaction = null;
            }
            return nvo;
        }

        private clsAddPatientLinkFileBizActionVO AddEMRImage(clsAddPatientLinkFileBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                if ((BizActionObj.PatientDetails != null) && (BizActionObj.PatientDetails.Count > 0))
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteImage");
                    this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, BizActionObj.PatientDetails[0].TemplateID);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                }
                foreach (clsPatientLinkFileBizActionVO nvo in BizActionObj.PatientDetails)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddTemplateImage");
                    this.dbServer.AddInParameter(storedProcCommand, "SourceURL", DbType.String, nvo.SourceURL);
                    this.dbServer.AddInParameter(storedProcCommand, "Report", DbType.Binary, nvo.Report);
                    this.dbServer.AddInParameter(storedProcCommand, "DocumentName", DbType.String, nvo.DocumentName);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.ID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    nvo.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        private clsAddPatientLinkFileBizActionVO AddLinkfile(clsAddPatientLinkFileBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                foreach (clsPatientLinkFileBizActionVO nvo in BizActionObj.PatientDetails)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatientLinkFile");
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                    this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.Date);
                    this.dbServer.AddInParameter(storedProcCommand, "SourceURL", DbType.String, nvo.SourceURL);
                    this.dbServer.AddInParameter(storedProcCommand, "Report", DbType.Binary, nvo.Report);
                    this.dbServer.AddInParameter(storedProcCommand, "DocumentName", DbType.String, nvo.DocumentName);
                    this.dbServer.AddInParameter(storedProcCommand, "Notes", DbType.String, nvo.Notes);
                    this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, nvo.Remarks);
                    this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, nvo.Time);
                    this.dbServer.AddInParameter(storedProcCommand, "ReferredBy", DbType.String, nvo.ReferredBy);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.ID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    nvo.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject AddNewCouple(IValueObject valueObject, clsUserVO UserVo)
        {
            try
            {
                clsAddNewCoupleBizActionVO nvo = valueObject as clsAddNewCoupleBizActionVO;
                clsPatientVO patientDetails = nvo.PatientDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DisableCoupleRegistration");
                if (nvo.PatientDetails.GenderID == 1L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MalePatientID", DbType.Int64, patientDetails.GeneralDetails.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "IsFemale", DbType.Boolean, false);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FemalePatientID", DbType.Int64, patientDetails.GeneralDetails.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "IsFemale", DbType.Boolean, true);
                }
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddCoupleRegistration");
                if (nvo.PatientDetails.GenderID == 1L)
                {
                    this.dbServer.AddInParameter(command2, "MalePatientID", DbType.Int64, patientDetails.GeneralDetails.PatientID);
                    this.dbServer.AddInParameter(command2, "FemalePatientID", DbType.Int64, patientDetails.SpouseDetails.PatientID);
                }
                else
                {
                    this.dbServer.AddInParameter(command2, "MalePatientID", DbType.Int64, patientDetails.SpouseDetails.PatientID);
                    this.dbServer.AddInParameter(command2, "FemalePatientID", DbType.Int64, patientDetails.GeneralDetails.PatientID);
                }
                this.dbServer.AddInParameter(command2, "MalePatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command2, "FemalePatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command2, "RegistrationDate", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, patientDetails.AddedDateTime);
                this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(command2, "CoupleRgistrationNumber", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "000000000000000000000000000000000000000000000000000");
                this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(command2, "IsFromNewCouple", DbType.Boolean, patientDetails.GeneralDetails.IsFromNewCouple);
                this.dbServer.ExecuteNonQuery(command2);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject AddPatient(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientBizActionVO bizActionObj = valueObject as clsAddPatientBizActionVO;
            bizActionObj = bizActionObj.PatientDetails.IsPanNoSave ? this.UpdatePatientPanNumber(bizActionObj, UserVo) : ((bizActionObj.PatientDetails.GeneralDetails.PatientID != 0L) ? this.UpdatePatientDetails(bizActionObj, UserVo) : this.AddPatientDetails(bizActionObj, UserVo));
            return valueObject;
        }

        private clsAddPatientBizActionVO AddPatientDetails(clsAddPatientBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                clsPatientVO patientDetails = BizActionObj.PatientDetails;
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatient");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, patientDetails.GeneralDetails.LinkServer);
                if (patientDetails.GeneralDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, patientDetails.GeneralDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "PatientCategoryID", DbType.Int64, patientDetails.GeneralDetails.PatientTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "ReferralTypeID", DbType.Int64, patientDetails.GeneralDetails.ReferralTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "ReferralDoctorID", DbType.Int64, patientDetails.GeneralDetails.ReferralDoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "ReferredToDoctorID", DbType.Int64, patientDetails.GeneralDetails.ReferredToDoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "ReferralDetail", DbType.String, patientDetails.GeneralDetails.ReferralDetail);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyName", DbType.String, Security.base64Encode(patientDetails.CompanyName));
                this.dbServer.AddInParameter(storedProcCommand, "RegistrationDate", DbType.DateTime, patientDetails.GeneralDetails.RegistrationDate);
                if (patientDetails.GeneralDetails.LastName != null)
                {
                    patientDetails.GeneralDetails.LastName = patientDetails.GeneralDetails.LastName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.LastName));
                if (patientDetails.GeneralDetails.FirstName != null)
                {
                    patientDetails.GeneralDetails.FirstName = patientDetails.GeneralDetails.FirstName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.FirstName));
                if (patientDetails.GeneralDetails.MiddleName != null)
                {
                    patientDetails.GeneralDetails.MiddleName = patientDetails.GeneralDetails.MiddleName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.MiddleName));
                if (patientDetails.FamilyName != null)
                {
                    patientDetails.FamilyName = patientDetails.FamilyName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "FamilyName", DbType.String, Security.base64Encode(patientDetails.FamilyName));
                this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, patientDetails.GenderID);
                this.dbServer.AddInParameter(storedProcCommand, "DateOfBirth", DbType.DateTime, patientDetails.GeneralDetails.DateOfBirth);
                this.dbServer.AddInParameter(storedProcCommand, "BloodGroupID", DbType.Int64, patientDetails.BloodGroupID);
                this.dbServer.AddInParameter(storedProcCommand, "MaritalStatusID", DbType.Int64, patientDetails.MaritalStatusID);
                this.dbServer.AddInParameter(storedProcCommand, "CivilID", DbType.String, Security.base64Encode(patientDetails.CivilID));
                if (patientDetails.ContactNo1 != null)
                {
                    patientDetails.ContactNo1 = patientDetails.ContactNo1.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo1", DbType.String, patientDetails.ContactNo1);
                if (patientDetails.ContactNo2 != null)
                {
                    patientDetails.ContactNo2 = patientDetails.ContactNo2.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo2", DbType.String, patientDetails.ContactNo2);
                if (patientDetails.FaxNo != null)
                {
                    patientDetails.FaxNo = patientDetails.FaxNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "FaxNo", DbType.String, patientDetails.FaxNo);
                if (patientDetails.Email != null)
                {
                    patientDetails.Email = patientDetails.Email.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Email", DbType.String, Security.base64Encode(patientDetails.Email));
                if (patientDetails.AddressLine1 != null)
                {
                    patientDetails.AddressLine1 = patientDetails.AddressLine1.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine1", DbType.String, Security.base64Encode(patientDetails.AddressLine1));
                if (patientDetails.AddressLine2 != null)
                {
                    patientDetails.AddressLine2 = patientDetails.AddressLine2.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine2", DbType.String, Security.base64Encode(patientDetails.AddressLine2));
                if (patientDetails.AddressLine3 != null)
                {
                    patientDetails.AddressLine3 = patientDetails.AddressLine3.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine3", DbType.String, Security.base64Encode(patientDetails.AddressLine3));
                if (patientDetails.Country != null)
                {
                    patientDetails.Country = patientDetails.Country.Trim();
                }
                if (patientDetails.OldRegistrationNo != null)
                {
                    patientDetails.OldRegistrationNo = patientDetails.OldRegistrationNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "OldRegistrationNo", DbType.String, patientDetails.OldRegistrationNo);
                this.dbServer.AddInParameter(storedProcCommand, "Country", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "State", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "City", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "Taluka", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "Area", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "District", DbType.String, null);
                if (patientDetails.CountryID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CountryID", DbType.Int64, patientDetails.CountryID);
                }
                if (patientDetails.StateID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "StateID", DbType.Int64, patientDetails.StateID);
                }
                if (patientDetails.CityID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CityID", DbType.Int64, patientDetails.CityID);
                }
                if (patientDetails.RegionID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "RegionID", DbType.Int64, patientDetails.RegionID);
                }
                this.dbServer.AddInParameter(storedProcCommand, "RegType", DbType.Int16, patientDetails.GeneralDetails.RegType);
                this.dbServer.AddInParameter(storedProcCommand, "MobileCountryCode", DbType.String, patientDetails.MobileCountryCode);
                this.dbServer.AddInParameter(storedProcCommand, "ResiNoCountryCode", DbType.Int64, patientDetails.ResiNoCountryCode);
                this.dbServer.AddInParameter(storedProcCommand, "ResiSTDCode", DbType.Int64, patientDetails.ResiSTDCode);
                if (patientDetails.Pincode != null)
                {
                    patientDetails.Pincode = patientDetails.Pincode.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Pincode", DbType.String, Security.base64Encode(patientDetails.Pincode));
                this.dbServer.AddInParameter(storedProcCommand, "ReligionID", DbType.Int64, patientDetails.ReligionID);
                this.dbServer.AddInParameter(storedProcCommand, "OccupationId", DbType.Int64, patientDetails.OccupationId);
                this.dbServer.AddInParameter(storedProcCommand, "Photo", DbType.Binary, patientDetails.Photo);
                this.dbServer.AddInParameter(storedProcCommand, "IsLoyaltyMember", DbType.Boolean, patientDetails.IsLoyaltyMember);
                this.dbServer.AddInParameter(storedProcCommand, "LoyaltyCardID", DbType.Int64, patientDetails.LoyaltyCardID);
                this.dbServer.AddInParameter(storedProcCommand, "RelationID", DbType.Int64, patientDetails.RelationID);
                this.dbServer.AddInParameter(storedProcCommand, "ParentPatientID", DbType.Int64, patientDetails.ParentPatientID);
                this.dbServer.AddInParameter(storedProcCommand, "IssueDate", DbType.DateTime, patientDetails.IssueDate);
                this.dbServer.AddInParameter(storedProcCommand, "EffectiveDate", DbType.DateTime, patientDetails.EffectiveDate);
                this.dbServer.AddInParameter(storedProcCommand, "ExpiryDate", DbType.DateTime, patientDetails.ExpiryDate);
                if (patientDetails.LoyaltyCardNo != null)
                {
                    patientDetails.LoyaltyCardNo = patientDetails.LoyaltyCardNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "LoyaltyCardNo", DbType.String, patientDetails.LoyaltyCardNo);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, patientDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, patientDetails.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                this.dbServer.AddParameter(storedProcCommand, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????");
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientDetails.GeneralDetails.PatientID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                long num = new Random().Next(0x1b207, 0xa2c2a);
                this.dbServer.AddInParameter(storedProcCommand, "RandomNumber", DbType.String, Convert.ToString(num));
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.PatientDetails.GeneralDetails.PatientID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                BizActionObj.PatientDetails.GeneralDetails.MRNo = (string) this.dbServer.GetParameterValue(storedProcCommand, "MRNo");
                string parameterValue = (string) this.dbServer.GetParameterValue(storedProcCommand, "Err");
                BizActionObj.PatientDetails.GeneralDetails.UnitId = UserVo.UserLoginInfo.UnitId;
                object[] objArray = new object[] { BizActionObj.PatientDetails.GeneralDetails.MRNo, "_", num, ".png" };
                string imgName = string.Concat(objArray);
                DemoImage image = new DemoImage();
                if (patientDetails.Photo != null)
                {
                    image.VaryQualityLevel(patientDetails.Photo, imgName, this.ImgSaveLocation);
                }
                if ((BizActionObj.PatientDetails.SpouseDetails != null) && ((BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 7L) || ((BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 8L) || (BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 9))))
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddPatient");
                    this.dbServer.AddInParameter(command2, "PatientCategoryID", DbType.Int64, patientDetails.GeneralDetails.PatientTypeID);
                    this.dbServer.AddInParameter(command2, "ReferralTypeID", DbType.Int64, patientDetails.GeneralDetails.ReferralTypeID);
                    this.dbServer.AddInParameter(command2, "ReferralDoctorID", DbType.Int64, patientDetails.GeneralDetails.ReferralDoctorID);
                    this.dbServer.AddInParameter(command2, "ReferredToDoctorID", DbType.Int64, patientDetails.GeneralDetails.ReferredToDoctorID);
                    this.dbServer.AddInParameter(command2, "ReferralDetail", DbType.String, patientDetails.GeneralDetails.ReferralDetail);
                    this.dbServer.AddInParameter(command2, "RegistrationDate", DbType.DateTime, patientDetails.GeneralDetails.RegistrationDate);
                    if (patientDetails.SpouseDetails.LastName != null)
                    {
                        patientDetails.SpouseDetails.LastName = patientDetails.SpouseDetails.LastName.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "LastName", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.LastName));
                    if (patientDetails.SpouseDetails.FirstName != null)
                    {
                        patientDetails.SpouseDetails.FirstName = patientDetails.SpouseDetails.FirstName.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "FirstName", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.FirstName));
                    if (patientDetails.SpouseDetails.MiddleName != null)
                    {
                        patientDetails.SpouseDetails.MiddleName = patientDetails.SpouseDetails.MiddleName.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "MiddleName", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.MiddleName));
                    if (patientDetails.SpouseDetails.FamilyName != null)
                    {
                        patientDetails.SpouseDetails.FamilyName = patientDetails.SpouseDetails.FamilyName.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "FamilyName", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.FamilyName));
                    this.dbServer.AddInParameter(command2, "GenderID", DbType.Int64, patientDetails.SpouseDetails.GenderID);
                    this.dbServer.AddInParameter(command2, "DateOfBirth", DbType.DateTime, patientDetails.SpouseDetails.DateOfBirth);
                    this.dbServer.AddInParameter(command2, "BloodGroupID", DbType.Int64, patientDetails.SpouseDetails.BloodGroupID);
                    this.dbServer.AddInParameter(command2, "MaritalStatusID", DbType.Int64, patientDetails.SpouseDetails.MaritalStatusID);
                    this.dbServer.AddInParameter(command2, "CivilID", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.CivilID));
                    if (patientDetails.SpouseDetails.ContactNo1 != null)
                    {
                        patientDetails.SpouseDetails.ContactNo1 = patientDetails.SpouseDetails.ContactNo1.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "ContactNo1", DbType.String, patientDetails.SpouseDetails.ContactNo1);
                    if (patientDetails.SpouseDetails.ContactNo2 != null)
                    {
                        patientDetails.SpouseDetails.ContactNo2 = patientDetails.SpouseDetails.ContactNo2.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "ContactNo2", DbType.String, patientDetails.SpouseDetails.ContactNo2);
                    if (patientDetails.SpouseDetails.FaxNo != null)
                    {
                        patientDetails.SpouseDetails.FaxNo = patientDetails.SpouseDetails.FaxNo.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "FaxNo", DbType.String, patientDetails.SpouseDetails.FaxNo);
                    if (patientDetails.SpouseDetails.Email != null)
                    {
                        patientDetails.SpouseDetails.Email = patientDetails.SpouseDetails.Email.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "Email", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.Email));
                    if (patientDetails.SpouseDetails.AddressLine1 != null)
                    {
                        patientDetails.SpouseDetails.AddressLine1 = patientDetails.SpouseDetails.AddressLine1.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "AddressLine1", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.AddressLine1));
                    if (patientDetails.SpouseDetails.AddressLine2 != null)
                    {
                        patientDetails.SpouseDetails.AddressLine2 = patientDetails.SpouseDetails.AddressLine2.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "AddressLine2", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.AddressLine2));
                    if (patientDetails.SpouseDetails.AddressLine3 != null)
                    {
                        patientDetails.SpouseDetails.AddressLine3 = patientDetails.SpouseDetails.AddressLine3.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "AddressLine3", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.AddressLine3));
                    if (patientDetails.SpouseDetails.Country != null)
                    {
                        patientDetails.SpouseDetails.Country = patientDetails.SpouseDetails.Country.Trim();
                    }
                    if (patientDetails.SpouseDetails.SpouseOldRegistrationNo != null)
                    {
                        patientDetails.SpouseDetails.SpouseOldRegistrationNo = patientDetails.SpouseDetails.SpouseOldRegistrationNo.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "OldRegistrationNo", DbType.String, patientDetails.SpouseDetails.SpouseOldRegistrationNo);
                    this.dbServer.AddInParameter(command2, "Country", DbType.String, null);
                    this.dbServer.AddInParameter(command2, "State", DbType.String, null);
                    this.dbServer.AddInParameter(command2, "City", DbType.String, null);
                    this.dbServer.AddInParameter(command2, "Taluka", DbType.String, null);
                    this.dbServer.AddInParameter(command2, "Area", DbType.String, null);
                    this.dbServer.AddInParameter(command2, "District", DbType.String, null);
                    if (patientDetails.SpouseDetails.CountryID > 0L)
                    {
                        this.dbServer.AddInParameter(command2, "CountryID", DbType.Int64, patientDetails.SpouseDetails.CountryID);
                    }
                    if (patientDetails.SpouseDetails.StateID > 0L)
                    {
                        this.dbServer.AddInParameter(command2, "StateID", DbType.Int64, patientDetails.SpouseDetails.StateID);
                    }
                    if (patientDetails.SpouseDetails.CityID > 0L)
                    {
                        this.dbServer.AddInParameter(command2, "CityID", DbType.Int64, patientDetails.SpouseDetails.CityID);
                    }
                    if (patientDetails.SpouseDetails.RegionID > 0L)
                    {
                        this.dbServer.AddInParameter(command2, "RegionID", DbType.Int64, patientDetails.SpouseDetails.RegionID);
                    }
                    this.dbServer.AddInParameter(command2, "RegType", DbType.Int16, patientDetails.GeneralDetails.RegType);
                    this.dbServer.AddInParameter(command2, "MobileCountryCode", DbType.String, patientDetails.SpouseDetails.MobileCountryCode);
                    this.dbServer.AddInParameter(command2, "ResiNoCountryCode", DbType.Int64, patientDetails.SpouseDetails.ResiNoCountryCode);
                    this.dbServer.AddInParameter(command2, "ResiSTDCode", DbType.Int64, patientDetails.SpouseDetails.ResiSTDCode);
                    if (patientDetails.SpouseDetails.Pincode != null)
                    {
                        patientDetails.Pincode = patientDetails.SpouseDetails.Pincode.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "Pincode", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.Pincode));
                    this.dbServer.AddInParameter(command2, "CompanyName", DbType.String, Security.base64Encode(patientDetails.CompanyName));
                    this.dbServer.AddInParameter(command2, "ReligionID", DbType.Int64, patientDetails.SpouseDetails.ReligionID);
                    this.dbServer.AddInParameter(command2, "OccupationId", DbType.Int64, patientDetails.SpouseDetails.OccupationId);
                    this.dbServer.AddInParameter(command2, "Photo", DbType.Binary, patientDetails.SpouseDetails.Photo);
                    this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, patientDetails.Status);
                    this.dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, patientDetails.AddedDateTime);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    patientDetails.SpouseDetails.MRNo = patientDetails.GeneralDetails.MRNo.Remove(patientDetails.GeneralDetails.MRNo.Length - 1, 1);
                    this.dbServer.AddParameter(command2, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                    this.dbServer.AddParameter(command2, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientDetails.SpouseDetails.MRNo);
                    this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientDetails.SpouseDetails.PatientID);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    long num2 = new Random().Next(0x1b207, 0xa2c2a);
                    this.dbServer.AddInParameter(command2, "RandomNumber", DbType.String, Convert.ToString(num2));
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                    BizActionObj.PatientDetails.SpouseDetails.PatientID = (long) this.dbServer.GetParameterValue(command2, "ID");
                    BizActionObj.PatientDetails.SpouseDetails.MRNo = (string) this.dbServer.GetParameterValue(command2, "MRNo");
                    string text2 = (string) this.dbServer.GetParameterValue(command2, "Err");
                    BizActionObj.PatientDetails.SpouseDetails.UnitId = UserVo.UserLoginInfo.UnitId;
                    object[] objArray2 = new object[] { BizActionObj.PatientDetails.SpouseDetails.MRNo, "_", num2, ".png" };
                    string str2 = string.Concat(objArray2);
                    if (patientDetails.SpouseDetails.Photo != null)
                    {
                        image.VaryQualityLevel(patientDetails.SpouseDetails.Photo, str2, this.ImgSaveLocation);
                    }
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddCoupleRegistration");
                    if (BizActionObj.PatientDetails.GenderID == 1L)
                    {
                        this.dbServer.AddInParameter(command3, "MalePatientID", DbType.Int64, patientDetails.GeneralDetails.PatientID);
                        this.dbServer.AddInParameter(command3, "FemalePatientID", DbType.Int64, patientDetails.SpouseDetails.PatientID);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(command3, "MalePatientID", DbType.Int64, patientDetails.SpouseDetails.PatientID);
                        this.dbServer.AddInParameter(command3, "FemalePatientID", DbType.Int64, patientDetails.GeneralDetails.PatientID);
                    }
                    this.dbServer.AddInParameter(command3, "MalePatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "FemalePatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "RegistrationDate", DbType.DateTime, patientDetails.GeneralDetails.RegistrationDate);
                    this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, patientDetails.Status);
                    this.dbServer.AddInParameter(command3, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, patientDetails.AddedDateTime);
                    this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command3, "CoupleRgistrationNumber", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "000000000000000000000000000000000000000000000000000");
                    this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command3, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
                }
                transaction.Commit();
                this.addPatientUserDetails(BizActionObj, UserVo);
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw exception;
            }
            finally
            {
                transaction.Dispose();
                transaction = null;
            }
            return BizActionObj;
        }

        private clsAddPatientBizActionVO AddPatientDetailsIPDWithTransaction(clsAddPatientBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection pConnection = this.dbServer.CreateConnection();
            try
            {
                clsPatientVO patientDetails = BizActionObj.PatientDetails;
                pConnection.Open();
                transaction = pConnection.BeginTransaction();
                if (BizActionObj.IsSavePatientFromIPD && BizActionObj.IsSaveInTRegistration)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatient");
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, patientDetails.GeneralDetails.LinkServer);
                    if (patientDetails.GeneralDetails.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, patientDetails.GeneralDetails.LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "PatientCategoryID", DbType.Int64, patientDetails.GeneralDetails.PatientTypeID);
                    this.dbServer.AddInParameter(storedProcCommand, "ReferralTypeID", DbType.Int64, patientDetails.GeneralDetails.ReferralTypeID);
                    this.dbServer.AddInParameter(storedProcCommand, "ReferralDoctorID", DbType.Int64, patientDetails.GeneralDetails.ReferralDoctorID);
                    this.dbServer.AddInParameter(storedProcCommand, "ReferredToDoctorID", DbType.Int64, patientDetails.GeneralDetails.ReferredToDoctorID);
                    this.dbServer.AddInParameter(storedProcCommand, "ReferralDetail", DbType.String, patientDetails.GeneralDetails.ReferralDetail);
                    this.dbServer.AddInParameter(storedProcCommand, "CompanyName", DbType.String, Security.base64Encode(patientDetails.CompanyName));
                    this.dbServer.AddInParameter(storedProcCommand, "RegistrationDate", DbType.DateTime, patientDetails.GeneralDetails.RegistrationDate);
                    if (patientDetails.GeneralDetails.LastName != null)
                    {
                        patientDetails.GeneralDetails.LastName = patientDetails.GeneralDetails.LastName.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.LastName));
                    if (patientDetails.GeneralDetails.FirstName != null)
                    {
                        patientDetails.GeneralDetails.FirstName = patientDetails.GeneralDetails.FirstName.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.FirstName));
                    if (patientDetails.GeneralDetails.MiddleName != null)
                    {
                        patientDetails.GeneralDetails.MiddleName = patientDetails.GeneralDetails.MiddleName.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.MiddleName));
                    if (patientDetails.FamilyName != null)
                    {
                        patientDetails.FamilyName = patientDetails.FamilyName.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "FamilyName", DbType.String, Security.base64Encode(patientDetails.FamilyName));
                    this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, patientDetails.GenderID);
                    this.dbServer.AddInParameter(storedProcCommand, "DateOfBirth", DbType.DateTime, patientDetails.GeneralDetails.DateOfBirth);
                    this.dbServer.AddInParameter(storedProcCommand, "BloodGroupID", DbType.Int64, patientDetails.BloodGroupID);
                    this.dbServer.AddInParameter(storedProcCommand, "MaritalStatusID", DbType.Int64, patientDetails.MaritalStatusID);
                    this.dbServer.AddInParameter(storedProcCommand, "CivilID", DbType.String, Security.base64Encode(patientDetails.CivilID));
                    if (patientDetails.ContactNo1 != null)
                    {
                        patientDetails.ContactNo1 = patientDetails.ContactNo1.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "ContactNo1", DbType.String, patientDetails.ContactNo1);
                    if (patientDetails.ContactNo2 != null)
                    {
                        patientDetails.ContactNo2 = patientDetails.ContactNo2.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "ContactNo2", DbType.String, patientDetails.ContactNo2);
                    if (patientDetails.FaxNo != null)
                    {
                        patientDetails.FaxNo = patientDetails.FaxNo.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "FaxNo", DbType.String, patientDetails.FaxNo);
                    if (patientDetails.Email != null)
                    {
                        patientDetails.Email = patientDetails.Email.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "Email", DbType.String, Security.base64Encode(patientDetails.Email));
                    if (patientDetails.AddressLine1 != null)
                    {
                        patientDetails.AddressLine1 = patientDetails.AddressLine1.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "AddressLine1", DbType.String, Security.base64Encode(patientDetails.AddressLine1));
                    if (patientDetails.AddressLine2 != null)
                    {
                        patientDetails.AddressLine2 = patientDetails.AddressLine2.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "AddressLine2", DbType.String, Security.base64Encode(patientDetails.AddressLine2));
                    if (patientDetails.AddressLine3 != null)
                    {
                        patientDetails.AddressLine3 = patientDetails.AddressLine3.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "AddressLine3", DbType.String, Security.base64Encode(patientDetails.AddressLine3));
                    if (patientDetails.Country != null)
                    {
                        patientDetails.Country = patientDetails.Country.Trim();
                    }
                    if (patientDetails.OldRegistrationNo != null)
                    {
                        patientDetails.OldRegistrationNo = patientDetails.OldRegistrationNo.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "OldRegistrationNo", DbType.String, patientDetails.OldRegistrationNo);
                    this.dbServer.AddInParameter(storedProcCommand, "Country", DbType.String, null);
                    this.dbServer.AddInParameter(storedProcCommand, "State", DbType.String, null);
                    this.dbServer.AddInParameter(storedProcCommand, "City", DbType.String, null);
                    this.dbServer.AddInParameter(storedProcCommand, "Taluka", DbType.String, null);
                    this.dbServer.AddInParameter(storedProcCommand, "Area", DbType.String, null);
                    this.dbServer.AddInParameter(storedProcCommand, "District", DbType.String, null);
                    if (patientDetails.CountryID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "CountryID", DbType.Int64, patientDetails.CountryID);
                    }
                    if (patientDetails.StateID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "StateID", DbType.Int64, patientDetails.StateID);
                    }
                    if (patientDetails.CityID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "CityID", DbType.Int64, patientDetails.CityID);
                    }
                    if (patientDetails.RegionID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "RegionID", DbType.Int64, patientDetails.RegionID);
                    }
                    if (patientDetails.PrefixId > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "PrefixId", DbType.Int64, patientDetails.PrefixId);
                    }
                    if (patientDetails.IdentityID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "IdentityID", DbType.Int64, patientDetails.IdentityID);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "IdentityNumber", DbType.String, patientDetails.IdentityNumber);
                    this.dbServer.AddInParameter(storedProcCommand, "RemarkForPatientType", DbType.String, patientDetails.RemarkForPatientType);
                    this.dbServer.AddInParameter(storedProcCommand, "IsInternationalPatient", DbType.Boolean, patientDetails.IsInternationalPatient);
                    this.dbServer.AddInParameter(storedProcCommand, "Education", DbType.String, patientDetails.Education);
                    this.dbServer.AddInParameter(storedProcCommand, "MobileCountryCode", DbType.String, patientDetails.MobileCountryCode);
                    this.dbServer.AddInParameter(storedProcCommand, "ResiNoCountryCode", DbType.Int64, patientDetails.ResiNoCountryCode);
                    this.dbServer.AddInParameter(storedProcCommand, "ResiSTDCode", DbType.Int64, patientDetails.ResiSTDCode);
                    if (patientDetails.Pincode != null)
                    {
                        patientDetails.Pincode = patientDetails.Pincode.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "Pincode", DbType.String, Security.base64Encode(patientDetails.Pincode));
                    this.dbServer.AddInParameter(storedProcCommand, "ReligionID", DbType.Int64, patientDetails.ReligionID);
                    this.dbServer.AddInParameter(storedProcCommand, "OccupationId", DbType.Int64, patientDetails.OccupationId);
                    this.dbServer.AddInParameter(storedProcCommand, "Photo", DbType.Binary, patientDetails.Photo);
                    this.dbServer.AddInParameter(storedProcCommand, "IsLoyaltyMember", DbType.Boolean, patientDetails.IsLoyaltyMember);
                    this.dbServer.AddInParameter(storedProcCommand, "LoyaltyCardID", DbType.Int64, patientDetails.LoyaltyCardID);
                    this.dbServer.AddInParameter(storedProcCommand, "RelationID", DbType.Int64, patientDetails.RelationID);
                    this.dbServer.AddInParameter(storedProcCommand, "ParentPatientID", DbType.Int64, patientDetails.ParentPatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "IssueDate", DbType.DateTime, patientDetails.IssueDate);
                    this.dbServer.AddInParameter(storedProcCommand, "EffectiveDate", DbType.DateTime, patientDetails.EffectiveDate);
                    this.dbServer.AddInParameter(storedProcCommand, "ExpiryDate", DbType.DateTime, patientDetails.ExpiryDate);
                    if (patientDetails.LoyaltyCardNo != null)
                    {
                        patientDetails.LoyaltyCardNo = patientDetails.LoyaltyCardNo.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "LoyaltyCardNo", DbType.String, patientDetails.LoyaltyCardNo);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, patientDetails.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, patientDetails.AddedDateTime);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                    this.dbServer.AddParameter(storedProcCommand, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????");
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientDetails.GeneralDetails.PatientID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    long num = new Random().Next(0x1b207, 0xa2c2a);
                    this.dbServer.AddInParameter(storedProcCommand, "RandomNumber", DbType.String, Convert.ToString(num));
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    BizActionObj.PatientDetails.GeneralDetails.PatientID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                    BizActionObj.PatientDetails.GeneralDetails.MRNo = (string) this.dbServer.GetParameterValue(storedProcCommand, "MRNo");
                    string parameterValue = (string) this.dbServer.GetParameterValue(storedProcCommand, "Err");
                    BizActionObj.PatientDetails.GeneralDetails.UnitId = UserVo.UserLoginInfo.UnitId;
                    object[] objArray = new object[] { BizActionObj.PatientDetails.GeneralDetails.MRNo, "_", num, ".png" };
                    string imgName = string.Concat(objArray);
                    DemoImage image = new DemoImage();
                    if (patientDetails.Photo != null)
                    {
                        image.VaryQualityLevel(patientDetails.Photo, imgName, this.ImgSaveLocation);
                    }
                    if ((BizActionObj.PatientDetails.SpouseDetails != null) && ((BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 7L) || ((BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 8L) || (BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 9))))
                    {
                        DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddPatient");
                        this.dbServer.AddInParameter(command, "PatientCategoryID", DbType.Int64, patientDetails.GeneralDetails.PatientTypeID);
                        this.dbServer.AddInParameter(storedProcCommand, "ReferralTypeID", DbType.Int64, patientDetails.GeneralDetails.ReferralTypeID);
                        this.dbServer.AddInParameter(command, "RegistrationDate", DbType.DateTime, patientDetails.GeneralDetails.RegistrationDate);
                        if (patientDetails.SpouseDetails.LastName != null)
                        {
                            patientDetails.SpouseDetails.LastName = patientDetails.SpouseDetails.LastName.Trim();
                        }
                        this.dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.LastName));
                        if (patientDetails.SpouseDetails.FirstName != null)
                        {
                            patientDetails.SpouseDetails.FirstName = patientDetails.SpouseDetails.FirstName.Trim();
                        }
                        this.dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.FirstName));
                        if (patientDetails.SpouseDetails.MiddleName != null)
                        {
                            patientDetails.SpouseDetails.MiddleName = patientDetails.SpouseDetails.MiddleName.Trim();
                        }
                        this.dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.MiddleName));
                        if (patientDetails.SpouseDetails.FamilyName != null)
                        {
                            patientDetails.SpouseDetails.FamilyName = patientDetails.SpouseDetails.FamilyName.Trim();
                        }
                        this.dbServer.AddInParameter(command, "FamilyName", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.FamilyName));
                        this.dbServer.AddInParameter(command, "GenderID", DbType.Int64, patientDetails.SpouseDetails.GenderID);
                        this.dbServer.AddInParameter(command, "DateOfBirth", DbType.DateTime, patientDetails.SpouseDetails.DateOfBirth);
                        this.dbServer.AddInParameter(command, "BloodGroupID", DbType.Int64, patientDetails.SpouseDetails.BloodGroupID);
                        this.dbServer.AddInParameter(command, "MaritalStatusID", DbType.Int64, patientDetails.SpouseDetails.MaritalStatusID);
                        this.dbServer.AddInParameter(command, "CivilID", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.CivilID));
                        if (patientDetails.SpouseDetails.ContactNo1 != null)
                        {
                            patientDetails.SpouseDetails.ContactNo1 = patientDetails.SpouseDetails.ContactNo1.Trim();
                        }
                        this.dbServer.AddInParameter(command, "ContactNo1", DbType.String, patientDetails.SpouseDetails.ContactNo1);
                        if (patientDetails.SpouseDetails.ContactNo2 != null)
                        {
                            patientDetails.SpouseDetails.ContactNo2 = patientDetails.SpouseDetails.ContactNo2.Trim();
                        }
                        this.dbServer.AddInParameter(command, "ContactNo2", DbType.String, patientDetails.SpouseDetails.ContactNo2);
                        if (patientDetails.SpouseDetails.FaxNo != null)
                        {
                            patientDetails.SpouseDetails.FaxNo = patientDetails.SpouseDetails.FaxNo.Trim();
                        }
                        this.dbServer.AddInParameter(command, "FaxNo", DbType.String, patientDetails.SpouseDetails.FaxNo);
                        if (patientDetails.SpouseDetails.Email != null)
                        {
                            patientDetails.SpouseDetails.Email = patientDetails.SpouseDetails.Email.Trim();
                        }
                        this.dbServer.AddInParameter(command, "Email", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.Email));
                        if (patientDetails.SpouseDetails.AddressLine1 != null)
                        {
                            patientDetails.SpouseDetails.AddressLine1 = patientDetails.SpouseDetails.AddressLine1.Trim();
                        }
                        this.dbServer.AddInParameter(command, "AddressLine1", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.AddressLine1));
                        if (patientDetails.SpouseDetails.AddressLine2 != null)
                        {
                            patientDetails.SpouseDetails.AddressLine2 = patientDetails.SpouseDetails.AddressLine2.Trim();
                        }
                        this.dbServer.AddInParameter(command, "AddressLine2", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.AddressLine2));
                        if (patientDetails.SpouseDetails.AddressLine3 != null)
                        {
                            patientDetails.SpouseDetails.AddressLine3 = patientDetails.SpouseDetails.AddressLine3.Trim();
                        }
                        this.dbServer.AddInParameter(command, "AddressLine3", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.AddressLine3));
                        if (patientDetails.SpouseDetails.Country != null)
                        {
                            patientDetails.SpouseDetails.Country = patientDetails.SpouseDetails.Country.Trim();
                        }
                        if (patientDetails.SpouseDetails.Country != null)
                        {
                            patientDetails.SpouseDetails.Country = patientDetails.SpouseDetails.Country.Trim();
                        }
                        this.dbServer.AddInParameter(command, "Country", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.Country));
                        if (patientDetails.SpouseDetails.State != null)
                        {
                            patientDetails.SpouseDetails.State = patientDetails.SpouseDetails.State.Trim();
                        }
                        this.dbServer.AddInParameter(command, "State", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.State));
                        if (patientDetails.SpouseDetails.City != null)
                        {
                            patientDetails.SpouseDetails.City = patientDetails.SpouseDetails.City.Trim();
                        }
                        this.dbServer.AddInParameter(command, "City", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.City));
                        if (patientDetails.SpouseDetails.Taluka != null)
                        {
                            patientDetails.SpouseDetails.Taluka = patientDetails.SpouseDetails.Taluka.Trim();
                        }
                        this.dbServer.AddInParameter(command, "Taluka", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.Taluka));
                        if (patientDetails.SpouseDetails.Area != null)
                        {
                            patientDetails.SpouseDetails.Area = patientDetails.SpouseDetails.Area.Trim();
                        }
                        this.dbServer.AddInParameter(command, "Area", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.Area));
                        if (patientDetails.SpouseDetails.District != null)
                        {
                            patientDetails.SpouseDetails.District = patientDetails.SpouseDetails.District.Trim();
                        }
                        this.dbServer.AddInParameter(command, "District", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.District));
                        if (patientDetails.SpouseDetails.SpouseOldRegistrationNo != null)
                        {
                            patientDetails.SpouseDetails.SpouseOldRegistrationNo = patientDetails.SpouseDetails.SpouseOldRegistrationNo.Trim();
                        }
                        this.dbServer.AddInParameter(command, "OldRegistrationNo", DbType.String, patientDetails.SpouseDetails.SpouseOldRegistrationNo);
                        this.dbServer.AddInParameter(command, "MobileCountryCode", DbType.String, patientDetails.SpouseDetails.MobileCountryCode);
                        this.dbServer.AddInParameter(command, "ResiNoCountryCode", DbType.Int64, patientDetails.SpouseDetails.ResiNoCountryCode);
                        this.dbServer.AddInParameter(command, "ResiSTDCode", DbType.Int64, patientDetails.SpouseDetails.ResiSTDCode);
                        if (patientDetails.SpouseDetails.Pincode != null)
                        {
                            patientDetails.Pincode = patientDetails.SpouseDetails.Pincode.Trim();
                        }
                        this.dbServer.AddInParameter(command, "Pincode", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.Pincode));
                        this.dbServer.AddInParameter(command, "CompanyName", DbType.String, Security.base64Encode(patientDetails.CompanyName));
                        this.dbServer.AddInParameter(command, "ReligionID", DbType.Int64, patientDetails.SpouseDetails.ReligionID);
                        this.dbServer.AddInParameter(command, "OccupationId", DbType.Int64, patientDetails.SpouseDetails.OccupationId);
                        this.dbServer.AddInParameter(command, "Photo", DbType.Binary, patientDetails.SpouseDetails.Photo);
                        this.dbServer.AddInParameter(command, "Status", DbType.Boolean, patientDetails.Status);
                        this.dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, patientDetails.AddedDateTime);
                        this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        patientDetails.SpouseDetails.MRNo = patientDetails.GeneralDetails.MRNo.Remove(patientDetails.GeneralDetails.MRNo.Length - 1, 1);
                        this.dbServer.AddParameter(command, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        this.dbServer.AddParameter(command, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientDetails.SpouseDetails.MRNo);
                        this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientDetails.SpouseDetails.PatientID);
                        this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                        long num2 = new Random().Next(0x1b207, 0xa2c2a);
                        this.dbServer.AddInParameter(command, "RandomNumber", DbType.String, Convert.ToString(num2));
                        this.dbServer.ExecuteNonQuery(command, transaction);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                        BizActionObj.PatientDetails.SpouseDetails.PatientID = (long) this.dbServer.GetParameterValue(command, "ID");
                        BizActionObj.PatientDetails.SpouseDetails.MRNo = (string) this.dbServer.GetParameterValue(command, "MRNo");
                        string text2 = (string) this.dbServer.GetParameterValue(command, "Err");
                        BizActionObj.PatientDetails.SpouseDetails.UnitId = UserVo.UserLoginInfo.UnitId;
                        object[] objArray2 = new object[] { BizActionObj.PatientDetails.SpouseDetails.MRNo, "_", num2, ".png" };
                        string str2 = string.Concat(objArray2);
                        if (patientDetails.SpouseDetails.Photo != null)
                        {
                            image.VaryQualityLevel(patientDetails.SpouseDetails.Photo, str2, this.ImgSaveLocation);
                        }
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddCoupleRegistration");
                        if (BizActionObj.PatientDetails.GenderID == 1L)
                        {
                            this.dbServer.AddInParameter(command3, "MalePatientID", DbType.Int64, patientDetails.GeneralDetails.PatientID);
                            this.dbServer.AddInParameter(command3, "FemalePatientID", DbType.Int64, patientDetails.SpouseDetails.PatientID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "MalePatientID", DbType.Int64, patientDetails.SpouseDetails.PatientID);
                            this.dbServer.AddInParameter(command3, "FemalePatientID", DbType.Int64, patientDetails.GeneralDetails.PatientID);
                        }
                        this.dbServer.AddInParameter(command3, "MalePatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "FemalePatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "RegistrationDate", DbType.DateTime, patientDetails.GeneralDetails.RegistrationDate);
                        this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, patientDetails.Status);
                        this.dbServer.AddInParameter(command3, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, patientDetails.AddedDateTime);
                        this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddParameter(command3, "CoupleRgistrationNumber", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "000000000000000000000000000000000000000000000000000");
                        this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                        this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
                    }
                }
                if (patientDetails.KinInformationList != null)
                {
                    foreach (clsKinInformationVO nvo in patientDetails.KinInformationList)
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatientKinInfoIPD");
                        this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, 0);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.PatientID);
                        this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, nvo.MRCode);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "sbIsRegisteredPatient", DbType.String, nvo.IsRegisteredPatient);
                        this.dbServer.AddInParameter(storedProcCommand, "sbKinPatientId", DbType.String, nvo.KinPatientID);
                        this.dbServer.AddInParameter(storedProcCommand, "sbKinPatientUnitId", DbType.String, nvo.KinPatientUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "sbLastName", DbType.String, Security.base64Encode(nvo.KinLastName));
                        this.dbServer.AddInParameter(storedProcCommand, "sbFirstName", DbType.String, Security.base64Encode(nvo.KinFirstName));
                        this.dbServer.AddInParameter(storedProcCommand, "sbMiddleName", DbType.String, Security.base64Encode(nvo.KinMiddleName));
                        this.dbServer.AddInParameter(storedProcCommand, "sbFamilyName", DbType.String, Security.base64Encode(nvo.FamilyCode));
                        this.dbServer.AddInParameter(storedProcCommand, "sbMobileCountryCode", DbType.String, nvo.MobileCountryCode);
                        this.dbServer.AddInParameter(storedProcCommand, "sbMobileNumber", DbType.String, nvo.MobileCountryCode);
                        this.dbServer.AddInParameter(storedProcCommand, "sbAddress", DbType.String, nvo.Address);
                        this.dbServer.AddInParameter(storedProcCommand, "sbRelationshipID", DbType.String, nvo.RelationshipID);
                        this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.String, nvo.Status);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, patientDetails.AddedDateTime);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    }
                }
                if (BizActionObj.IsSaveSponsor)
                {
                    clsBasePatientSposorDAL instance = clsBasePatientSposorDAL.GetInstance();
                    BizActionObj.BizActionVOSaveSponsor.PatientSponsorDetails.PatientId = BizActionObj.PatientDetails.GeneralDetails.PatientID;
                    BizActionObj.BizActionVOSaveSponsor.PatientSponsorDetails.PatientUnitId = BizActionObj.PatientDetails.GeneralDetails.UnitId;
                    BizActionObj.BizActionVOSaveSponsor = (clsAddPatientSponsorBizActionVO) instance.AddPatientSponsorDetailsWithTransaction(BizActionObj.BizActionVOSaveSponsor, UserVo, pConnection, transaction);
                    if (BizActionObj.BizActionVOSaveSponsor.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                }
                if (BizActionObj.IsSaveAdmission)
                {
                    clsBaseIPDAdmissionDAL instance = clsBaseIPDAdmissionDAL.GetInstance();
                    BizActionObj.BizActionVOSaveAdmission.Details.PatientId = BizActionObj.PatientDetails.GeneralDetails.PatientID;
                    BizActionObj.BizActionVOSaveAdmission.Details.PatientUnitID = BizActionObj.PatientDetails.GeneralDetails.UnitId;
                    BizActionObj.BizActionVOSaveAdmission.Details.IPDNO = BizActionObj.PatientDetails.GeneralDetails.MRNo;
                    BizActionObj.BizActionVOSaveAdmission = (clsSaveIPDAdmissionBizActionVO) instance.AddIPDAdmissionDetailsWithTransaction(BizActionObj.BizActionVOSaveAdmission, UserVo, pConnection, transaction);
                    if (BizActionObj.BizActionVOSaveAdmission.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                }
                transaction.Commit();
                this.addPatientUserDetails(BizActionObj, UserVo);
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw exception;
            }
            finally
            {
                transaction.Dispose();
                transaction = null;
            }
            return BizActionObj;
        }

        private clsAddPatientBizActionVO AddPatientDetailsOPDWithTransaction(clsAddPatientBizActionVO BizActionObj, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {
            DbConnection pConnection = null;
            DbTransaction transaction = null;
            try
            {
                clsPatientVO patientDetails = BizActionObj.PatientDetails;
                pConnection = (myConnection == null) ? this.dbServer.CreateConnection() : myConnection;
                if (pConnection.State == ConnectionState.Closed)
                {
                    pConnection.Open();
                }
                transaction = (myTransaction == null) ? pConnection.BeginTransaction() : myTransaction;
                if (BizActionObj.IsSavePatientFromOPD)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatient");
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, patientDetails.GeneralDetails.LinkServer);
                    if (patientDetails.GeneralDetails.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, patientDetails.GeneralDetails.LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "PatientCategoryID", DbType.Int64, patientDetails.GeneralDetails.PatientTypeID);
                    this.dbServer.AddInParameter(storedProcCommand, "ReferralTypeID", DbType.Int64, patientDetails.GeneralDetails.ReferralTypeID);
                    this.dbServer.AddInParameter(storedProcCommand, "ReferralDoctorID", DbType.Int64, patientDetails.GeneralDetails.ReferralDoctorID);
                    this.dbServer.AddInParameter(storedProcCommand, "ReferredToDoctorID", DbType.Int64, patientDetails.GeneralDetails.ReferredToDoctorID);
                    this.dbServer.AddInParameter(storedProcCommand, "IsReferralDoc", DbType.Boolean, patientDetails.GeneralDetails.IsReferralDoc);
                    this.dbServer.AddInParameter(storedProcCommand, "ReferralDetail", DbType.String, patientDetails.GeneralDetails.ReferralDetail);
                    this.dbServer.AddInParameter(storedProcCommand, "CompanyName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.CompanyName));
                    this.dbServer.AddInParameter(storedProcCommand, "BDID", DbType.Int64, patientDetails.BDID);
                    if (patientDetails.PanNumber != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "PanNumber", DbType.String, patientDetails.PanNumber);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "CampID", DbType.Int64, patientDetails.CampID);
                    this.dbServer.AddInParameter(storedProcCommand, "NoOfYearsOfMarriage", DbType.Int64, patientDetails.NoOfYearsOfMarriage);
                    this.dbServer.AddInParameter(storedProcCommand, "NoOfExistingChildren", DbType.Int64, patientDetails.NoOfExistingChildren);
                    this.dbServer.AddInParameter(storedProcCommand, "FamilyTypeID", DbType.Int64, patientDetails.FamilyTypeID);
                    this.dbServer.AddInParameter(storedProcCommand, "AgentID", DbType.Int64, patientDetails.AgentID);
                    this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, patientDetails.AgencyID);
                    this.dbServer.AddInParameter(storedProcCommand, "RegistrationDate", DbType.DateTime, patientDetails.GeneralDetails.RegistrationDate);
                    if (patientDetails.GeneralDetails.LastName != null)
                    {
                        patientDetails.GeneralDetails.LastName = patientDetails.GeneralDetails.LastName.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.LastName));
                    if (patientDetails.GeneralDetails.FirstName != null)
                    {
                        patientDetails.GeneralDetails.FirstName = patientDetails.GeneralDetails.FirstName.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.FirstName));
                    if (patientDetails.GeneralDetails.MiddleName != null)
                    {
                        patientDetails.GeneralDetails.MiddleName = patientDetails.GeneralDetails.MiddleName.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.MiddleName));
                    if (patientDetails.FamilyName != null)
                    {
                        patientDetails.FamilyName = patientDetails.FamilyName.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "FamilyName", DbType.String, Security.base64Encode(patientDetails.FamilyName));
                    this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, patientDetails.GenderID);
                    this.dbServer.AddInParameter(storedProcCommand, "DateOfBirth", DbType.DateTime, patientDetails.GeneralDetails.DateOfBirth);
                    this.dbServer.AddInParameter(storedProcCommand, "IsAge", DbType.Boolean, patientDetails.GeneralDetails.IsAge);
                    this.dbServer.AddInParameter(storedProcCommand, "BloodGroupID", DbType.Int64, patientDetails.BloodGroupID);
                    this.dbServer.AddInParameter(storedProcCommand, "MaritalStatusID", DbType.Int64, patientDetails.MaritalStatusID);
                    this.dbServer.AddInParameter(storedProcCommand, "CivilID", DbType.String, Security.base64Encode(patientDetails.CivilID));
                    if (patientDetails.ContactNo1 != null)
                    {
                        patientDetails.ContactNo1 = patientDetails.ContactNo1.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "ContactNo1", DbType.String, patientDetails.ContactNo1);
                    if (patientDetails.ContactNo2 != null)
                    {
                        patientDetails.ContactNo2 = patientDetails.ContactNo2.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "ContactNo2", DbType.String, patientDetails.ContactNo2);
                    if (patientDetails.FaxNo != null)
                    {
                        patientDetails.FaxNo = patientDetails.FaxNo.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "FaxNo", DbType.String, patientDetails.FaxNo);
                    if (patientDetails.Email != null)
                    {
                        patientDetails.Email = patientDetails.Email.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "Email", DbType.String, Security.base64Encode(patientDetails.Email));
                    if (patientDetails.AddressLine1 != null)
                    {
                        patientDetails.AddressLine1 = patientDetails.AddressLine1.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "AddressLine1", DbType.String, Security.base64Encode(patientDetails.AddressLine1));
                    if (patientDetails.AddressLine2 != null)
                    {
                        patientDetails.AddressLine2 = patientDetails.AddressLine2.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "AddressLine2", DbType.String, Security.base64Encode(patientDetails.AddressLine2));
                    if (patientDetails.AddressLine3 != null)
                    {
                        patientDetails.AddressLine3 = patientDetails.AddressLine3.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "AddressLine3", DbType.String, Security.base64Encode(patientDetails.AddressLine3));
                    if (patientDetails.Country != null)
                    {
                        patientDetails.Country = patientDetails.Country.Trim();
                    }
                    if (patientDetails.OldRegistrationNo != null)
                    {
                        patientDetails.OldRegistrationNo = patientDetails.OldRegistrationNo.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "OldRegistrationNo", DbType.String, patientDetails.OldRegistrationNo);
                    this.dbServer.AddInParameter(storedProcCommand, "Country", DbType.String, null);
                    this.dbServer.AddInParameter(storedProcCommand, "State", DbType.String, null);
                    this.dbServer.AddInParameter(storedProcCommand, "City", DbType.String, null);
                    this.dbServer.AddInParameter(storedProcCommand, "Taluka", DbType.String, null);
                    this.dbServer.AddInParameter(storedProcCommand, "Area", DbType.String, null);
                    this.dbServer.AddInParameter(storedProcCommand, "District", DbType.String, null);
                    if (patientDetails.CountryID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "CountryID", DbType.Int64, patientDetails.CountryID);
                    }
                    if (patientDetails.StateID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "StateID", DbType.Int64, patientDetails.StateID);
                    }
                    if (patientDetails.CityID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "CityID", DbType.Int64, patientDetails.CityID);
                    }
                    if (patientDetails.RegionID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "RegionID", DbType.Int64, patientDetails.RegionID);
                    }
                    if (patientDetails.PrefixId > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "PrefixId", DbType.Int64, patientDetails.PrefixId);
                    }
                    if (patientDetails.IdentityID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "IdentityID", DbType.Int64, patientDetails.IdentityID);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "IdentityNumber", DbType.String, patientDetails.IdentityNumber);
                    this.dbServer.AddInParameter(storedProcCommand, "RemarkForPatientType", DbType.String, patientDetails.RemarkForPatientType);
                    this.dbServer.AddInParameter(storedProcCommand, "IsInternationalPatient", DbType.Boolean, patientDetails.IsInternationalPatient);
                    this.dbServer.AddInParameter(storedProcCommand, "Education", DbType.String, patientDetails.Education);
                    this.dbServer.AddInParameter(storedProcCommand, "RegType", DbType.Int16, patientDetails.GeneralDetails.RegType);
                    this.dbServer.AddInParameter(storedProcCommand, "MobileCountryCode", DbType.String, patientDetails.MobileCountryCode);
                    this.dbServer.AddInParameter(storedProcCommand, "ResiNoCountryCode", DbType.Int64, patientDetails.ResiNoCountryCode);
                    this.dbServer.AddInParameter(storedProcCommand, "ResiSTDCode", DbType.Int64, patientDetails.ResiSTDCode);
                    if (patientDetails.Pincode != null)
                    {
                        patientDetails.Pincode = patientDetails.Pincode.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "Pincode", DbType.String, Security.base64Encode(patientDetails.Pincode));
                    this.dbServer.AddInParameter(storedProcCommand, "ReligionID", DbType.Int64, patientDetails.ReligionID);
                    this.dbServer.AddInParameter(storedProcCommand, "OccupationId", DbType.Int64, patientDetails.OccupationId);
                    this.dbServer.AddInParameter(storedProcCommand, "Photo", DbType.Binary, patientDetails.Photo);
                    this.dbServer.AddInParameter(storedProcCommand, "IsLoyaltyMember", DbType.Boolean, patientDetails.IsLoyaltyMember);
                    this.dbServer.AddInParameter(storedProcCommand, "LoyaltyCardID", DbType.Int64, patientDetails.LoyaltyCardID);
                    this.dbServer.AddInParameter(storedProcCommand, "RelationID", DbType.Int64, patientDetails.RelationID);
                    this.dbServer.AddInParameter(storedProcCommand, "ParentPatientID", DbType.Int64, patientDetails.ParentPatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "IssueDate", DbType.DateTime, patientDetails.IssueDate);
                    this.dbServer.AddInParameter(storedProcCommand, "EffectiveDate", DbType.DateTime, patientDetails.EffectiveDate);
                    this.dbServer.AddInParameter(storedProcCommand, "ExpiryDate", DbType.DateTime, patientDetails.ExpiryDate);
                    if (patientDetails.LoyaltyCardNo != null)
                    {
                        patientDetails.LoyaltyCardNo = patientDetails.LoyaltyCardNo.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "LoyaltyCardNo", DbType.String, patientDetails.LoyaltyCardNo);
                    if (patientDetails.GeneralDetails.SonDaughterOf != null)
                    {
                        patientDetails.GeneralDetails.SonDaughterOf = patientDetails.GeneralDetails.SonDaughterOf.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "DaughterOf", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.SonDaughterOf));
                    this.dbServer.AddInParameter(storedProcCommand, "NationalityID", DbType.Int64, patientDetails.NationalityID);
                    this.dbServer.AddInParameter(storedProcCommand, "PrefLangID", DbType.Int64, patientDetails.PreferredLangID);
                    this.dbServer.AddInParameter(storedProcCommand, "TreatRequiredID", DbType.Int64, patientDetails.TreatRequiredID);
                    this.dbServer.AddInParameter(storedProcCommand, "EducationID", DbType.Int64, patientDetails.EducationID);
                    this.dbServer.AddInParameter(storedProcCommand, "MarriageAnnivDate", DbType.DateTime, patientDetails.MarriageAnnDate);
                    this.dbServer.AddInParameter(storedProcCommand, "NoOfPeople", DbType.Int32, patientDetails.GeneralDetails.NoOfPeople);
                    this.dbServer.AddInParameter(storedProcCommand, "IsClinicVisited", DbType.Boolean, BizActionObj.PatientDetails.IsClinicVisited);
                    this.dbServer.AddInParameter(storedProcCommand, "ClinicName", DbType.String, patientDetails.GeneralDetails.ClinicName);
                    this.dbServer.AddInParameter(storedProcCommand, "SpecialRegID", DbType.Int64, patientDetails.SpecialRegID);
                    if ((BizActionObj.PatientDetails != null) && (BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 13))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "BabyNo", DbType.Int32, patientDetails.BabyNo);
                        this.dbServer.AddInParameter(storedProcCommand, "BabyOfNo", DbType.Int32, patientDetails.BabyOfNo);
                        this.dbServer.AddInParameter(storedProcCommand, "BabyWeight", DbType.String, patientDetails.BabyWeight);
                        this.dbServer.AddInParameter(storedProcCommand, "LinkPatientID", DbType.Int64, patientDetails.LinkPatientID);
                        this.dbServer.AddInParameter(storedProcCommand, "LinkPatientUnitID", DbType.Int64, patientDetails.LinkPatientUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "LinkPatientMrNo", DbType.String, patientDetails.LinkPatientMrNo);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "IsStaffPatient", DbType.Boolean, patientDetails.IsStaffPatient);
                    this.dbServer.AddInParameter(storedProcCommand, "StaffID", DbType.Int64, patientDetails.StaffID);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, patientDetails.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, patientDetails.AddedDateTime);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                    this.dbServer.AddParameter(storedProcCommand, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????");
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientDetails.GeneralDetails.PatientID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddParameter(storedProcCommand, "ImagePath", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    BizActionObj.PatientDetails.GeneralDetails.PatientID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                    BizActionObj.PatientDetails.GeneralDetails.MRNo = (string) this.dbServer.GetParameterValue(storedProcCommand, "MRNo");
                    BizActionObj.PatientDetails.ImageName = Convert.ToString(this.dbServer.GetParameterValue(storedProcCommand, "ImagePath"));
                    string parameterValue = (string) this.dbServer.GetParameterValue(storedProcCommand, "Err");
                    BizActionObj.PatientDetails.GeneralDetails.UnitId = UserVo.UserLoginInfo.UnitId;
                    DemoImage image = new DemoImage();
                    if (patientDetails.Photo != null)
                    {
                        image.VaryQualityLevel(patientDetails.Photo, BizActionObj.PatientDetails.ImageName, this.ImgSaveLocation);
                    }
                    if ((BizActionObj.PatientDetails.SpouseDetails != null) && ((BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 7L) || ((BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 8L) || (BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 9))))
                    {
                        DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddPatient");
                        this.dbServer.AddInParameter(command, "BDID", DbType.Int64, patientDetails.BDID);
                        if (patientDetails.PanNumber != null)
                        {
                            this.dbServer.AddInParameter(command, "PanNumber", DbType.String, patientDetails.SpouseDetails.SpousePanNumber);
                        }
                        long campID = patientDetails.CampID;
                        this.dbServer.AddInParameter(command, "CampID", DbType.Int64, patientDetails.CampID);
                        this.dbServer.AddInParameter(command, "PatientCategoryID", DbType.Int64, patientDetails.GeneralDetails.PatientTypeID);
                        this.dbServer.AddInParameter(command, "ReferralTypeID", DbType.Int64, patientDetails.GeneralDetails.ReferralTypeID);
                        this.dbServer.AddInParameter(command, "ReferralDoctorID", DbType.Int64, patientDetails.GeneralDetails.ReferralDoctorID);
                        this.dbServer.AddInParameter(command, "ReferredToDoctorID", DbType.Int64, patientDetails.GeneralDetails.ReferredToDoctorID);
                        this.dbServer.AddInParameter(command, "IsReferralDoc", DbType.Boolean, patientDetails.GeneralDetails.IsReferralDoc);
                        this.dbServer.AddInParameter(command, "ReferralDetail", DbType.String, patientDetails.GeneralDetails.ReferralDetail);
                        this.dbServer.AddInParameter(command, "CompanyName", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.CompanyName));
                        this.dbServer.AddInParameter(command, "RegistrationDate", DbType.DateTime, patientDetails.GeneralDetails.RegistrationDate);
                        if (patientDetails.SpouseDetails.LastName != null)
                        {
                            patientDetails.SpouseDetails.LastName = patientDetails.SpouseDetails.LastName.Trim();
                        }
                        this.dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.LastName));
                        if (patientDetails.SpouseDetails.FirstName != null)
                        {
                            patientDetails.SpouseDetails.FirstName = patientDetails.SpouseDetails.FirstName.Trim();
                        }
                        this.dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.FirstName));
                        if (patientDetails.SpouseDetails.MiddleName != null)
                        {
                            patientDetails.SpouseDetails.MiddleName = patientDetails.SpouseDetails.MiddleName.Trim();
                        }
                        this.dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.MiddleName));
                        if (patientDetails.SpouseDetails.FamilyName != null)
                        {
                            patientDetails.SpouseDetails.FamilyName = patientDetails.SpouseDetails.FamilyName.Trim();
                        }
                        this.dbServer.AddInParameter(command, "FamilyName", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.FamilyName));
                        this.dbServer.AddInParameter(command, "GenderID", DbType.Int64, patientDetails.SpouseDetails.GenderID);
                        this.dbServer.AddInParameter(command, "DateOfBirth", DbType.DateTime, patientDetails.SpouseDetails.DateOfBirth);
                        this.dbServer.AddInParameter(command, "IsAge", DbType.Boolean, patientDetails.SpouseDetails.IsAge);
                        this.dbServer.AddInParameter(command, "BloodGroupID", DbType.Int64, patientDetails.SpouseDetails.BloodGroupID);
                        this.dbServer.AddInParameter(command, "MaritalStatusID", DbType.Int64, patientDetails.SpouseDetails.MaritalStatusID);
                        this.dbServer.AddInParameter(command, "CivilID", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.CivilID));
                        if (patientDetails.SpouseDetails.ContactNo1 != null)
                        {
                            patientDetails.SpouseDetails.ContactNo1 = patientDetails.SpouseDetails.ContactNo1.Trim();
                        }
                        this.dbServer.AddInParameter(command, "ContactNo1", DbType.String, patientDetails.SpouseDetails.ContactNo1);
                        if (patientDetails.SpouseDetails.ContactNo2 != null)
                        {
                            patientDetails.SpouseDetails.ContactNo2 = patientDetails.SpouseDetails.ContactNo2.Trim();
                        }
                        this.dbServer.AddInParameter(command, "ContactNo2", DbType.String, patientDetails.SpouseDetails.ContactNo2);
                        if (patientDetails.SpouseDetails.FaxNo != null)
                        {
                            patientDetails.SpouseDetails.FaxNo = patientDetails.SpouseDetails.FaxNo.Trim();
                        }
                        this.dbServer.AddInParameter(command, "FaxNo", DbType.String, patientDetails.SpouseDetails.FaxNo);
                        if (patientDetails.SpouseDetails.Email != null)
                        {
                            patientDetails.SpouseDetails.Email = patientDetails.SpouseDetails.Email.Trim();
                        }
                        this.dbServer.AddInParameter(command, "Email", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.Email));
                        if (patientDetails.SpouseDetails.AddressLine1 != null)
                        {
                            patientDetails.SpouseDetails.AddressLine1 = patientDetails.SpouseDetails.AddressLine1.Trim();
                        }
                        this.dbServer.AddInParameter(command, "AddressLine1", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.AddressLine1));
                        if (patientDetails.SpouseDetails.AddressLine2 != null)
                        {
                            patientDetails.SpouseDetails.AddressLine2 = patientDetails.SpouseDetails.AddressLine2.Trim();
                        }
                        this.dbServer.AddInParameter(command, "AddressLine2", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.AddressLine2));
                        if (patientDetails.SpouseDetails.AddressLine3 != null)
                        {
                            patientDetails.SpouseDetails.AddressLine3 = patientDetails.SpouseDetails.AddressLine3.Trim();
                        }
                        this.dbServer.AddInParameter(command, "AddressLine3", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.AddressLine3));
                        if (patientDetails.SpouseDetails.Country != null)
                        {
                            patientDetails.SpouseDetails.Country = patientDetails.SpouseDetails.Country.Trim();
                        }
                        if (patientDetails.SpouseDetails.SpouseOldRegistrationNo != null)
                        {
                            patientDetails.SpouseDetails.SpouseOldRegistrationNo = patientDetails.SpouseDetails.SpouseOldRegistrationNo.Trim();
                        }
                        this.dbServer.AddInParameter(command, "OldRegistrationNo", DbType.String, patientDetails.SpouseDetails.SpouseOldRegistrationNo);
                        this.dbServer.AddInParameter(command, "Country", DbType.String, null);
                        this.dbServer.AddInParameter(command, "State", DbType.String, null);
                        this.dbServer.AddInParameter(command, "City", DbType.String, null);
                        this.dbServer.AddInParameter(command, "Taluka", DbType.String, null);
                        this.dbServer.AddInParameter(command, "Area", DbType.String, null);
                        this.dbServer.AddInParameter(command, "District", DbType.String, null);
                        if (patientDetails.SpouseDetails.CountryID > 0L)
                        {
                            this.dbServer.AddInParameter(command, "CountryID", DbType.Int64, patientDetails.SpouseDetails.CountryID);
                        }
                        if (patientDetails.SpouseDetails.StateID > 0L)
                        {
                            this.dbServer.AddInParameter(command, "StateID", DbType.Int64, patientDetails.SpouseDetails.StateID);
                        }
                        if (patientDetails.SpouseDetails.CityID > 0L)
                        {
                            this.dbServer.AddInParameter(command, "CityID", DbType.Int64, patientDetails.SpouseDetails.CityID);
                        }
                        if (patientDetails.SpouseDetails.RegionID > 0L)
                        {
                            this.dbServer.AddInParameter(command, "RegionID", DbType.Int64, patientDetails.SpouseDetails.RegionID);
                        }
                        if (patientDetails.PrefixId > 0L)
                        {
                            this.dbServer.AddInParameter(command, "PrefixId", DbType.Int64, patientDetails.SpouseDetails.PrefixId);
                        }
                        if (patientDetails.IdentityID > 0L)
                        {
                            this.dbServer.AddInParameter(command, "IdentityID", DbType.Int64, patientDetails.SpouseDetails.IdentityID);
                        }
                        this.dbServer.AddInParameter(command, "IdentityNumber", DbType.String, patientDetails.SpouseDetails.IdentityNumber);
                        this.dbServer.AddInParameter(command, "RemarkForPatientType", DbType.String, patientDetails.SpouseDetails.RemarkForPatientType);
                        this.dbServer.AddInParameter(command, "IsInternationalPatient", DbType.Boolean, patientDetails.SpouseDetails.IsInternationalPatient);
                        this.dbServer.AddInParameter(command, "Education", DbType.String, patientDetails.SpouseDetails.Education);
                        this.dbServer.AddInParameter(command, "RegType", DbType.Int16, patientDetails.GeneralDetails.RegType);
                        this.dbServer.AddInParameter(command, "AgentID", DbType.Int64, patientDetails.AgentID);
                        this.dbServer.AddInParameter(command, "AgencyID", DbType.Int64, patientDetails.AgencyID);
                        this.dbServer.AddInParameter(command, "MobileCountryCode", DbType.String, patientDetails.SpouseDetails.MobileCountryCode);
                        this.dbServer.AddInParameter(command, "ResiNoCountryCode", DbType.Int64, patientDetails.SpouseDetails.ResiNoCountryCode);
                        this.dbServer.AddInParameter(command, "ResiSTDCode", DbType.Int64, patientDetails.SpouseDetails.ResiSTDCode);
                        if (patientDetails.SpouseDetails.Pincode != null)
                        {
                            patientDetails.Pincode = patientDetails.SpouseDetails.Pincode.Trim();
                        }
                        this.dbServer.AddInParameter(command, "Pincode", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.Pincode));
                        this.dbServer.AddInParameter(command, "ReligionID", DbType.Int64, patientDetails.SpouseDetails.ReligionID);
                        this.dbServer.AddInParameter(command, "OccupationId", DbType.Int64, patientDetails.SpouseDetails.OccupationId);
                        this.dbServer.AddInParameter(command, "Photo", DbType.Binary, patientDetails.SpouseDetails.Photo);
                        if (patientDetails.SpouseDetails.SonDaughterOf != null)
                        {
                            patientDetails.SpouseDetails.SonDaughterOf = patientDetails.SpouseDetails.SonDaughterOf.Trim();
                        }
                        this.dbServer.AddInParameter(command, "DaughterOf", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.SonDaughterOf));
                        this.dbServer.AddInParameter(command, "NationalityID", DbType.Int64, patientDetails.SpouseDetails.NationalityID);
                        this.dbServer.AddInParameter(command, "PrefLangID", DbType.Int64, patientDetails.SpouseDetails.PreferredLangID);
                        this.dbServer.AddInParameter(command, "TreatRequiredID", DbType.Int64, patientDetails.TreatRequiredID);
                        this.dbServer.AddInParameter(command, "EducationID", DbType.Int64, patientDetails.SpouseDetails.EducationID);
                        this.dbServer.AddInParameter(command, "MarriageAnnivDate", DbType.DateTime, patientDetails.MarriageAnnDate);
                        this.dbServer.AddInParameter(command, "NoOfPeople", DbType.Int32, patientDetails.GeneralDetails.NoOfPeople);
                        this.dbServer.AddInParameter(command, "IsClinicVisited", DbType.Boolean, BizActionObj.PatientDetails.IsClinicVisited);
                        this.dbServer.AddInParameter(command, "ClinicName", DbType.String, patientDetails.GeneralDetails.ClinicName);
                        this.dbServer.AddInParameter(command, "SpecialRegID", DbType.Int64, patientDetails.SpecialRegID);
                        this.dbServer.AddInParameter(command, "NoOfYearsOfMarriage", DbType.Int64, patientDetails.NoOfYearsOfMarriage);
                        this.dbServer.AddInParameter(command, "NoOfExistingChildren", DbType.Int64, patientDetails.NoOfExistingChildren);
                        this.dbServer.AddInParameter(command, "FamilyTypeID", DbType.Int64, patientDetails.FamilyTypeID);
                        this.dbServer.AddInParameter(command, "DonorMRNO", DbType.String, BizActionObj.PatientDetails.GeneralDetails.MRNo);
                        this.dbServer.AddInParameter(command, "Status", DbType.Boolean, patientDetails.Status);
                        this.dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, patientDetails.AddedDateTime);
                        this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        patientDetails.SpouseDetails.MRNo = patientDetails.GeneralDetails.MRNo;
                        this.dbServer.AddParameter(command, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        this.dbServer.AddParameter(command, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientDetails.SpouseDetails.MRNo);
                        this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientDetails.SpouseDetails.PatientID);
                        this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.AddParameter(command, "ImagePath", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        this.dbServer.ExecuteNonQuery(command, transaction);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                        BizActionObj.PatientDetails.SpouseDetails.PatientID = (long) this.dbServer.GetParameterValue(command, "ID");
                        BizActionObj.PatientDetails.SpouseDetails.MRNo = Convert.ToString(this.dbServer.GetParameterValue(command, "MRNo"));
                        BizActionObj.PatientDetails.SpouseDetails.ImageName = Convert.ToString(this.dbServer.GetParameterValue(command, "ImagePath"));
                        string text2 = (string) this.dbServer.GetParameterValue(command, "Err");
                        BizActionObj.PatientDetails.SpouseDetails.UnitId = UserVo.UserLoginInfo.UnitId;
                        if (patientDetails.SpouseDetails.Photo != null)
                        {
                            image.VaryQualityLevel(patientDetails.SpouseDetails.Photo, BizActionObj.PatientDetails.SpouseDetails.ImageName, this.ImgSaveLocation);
                        }
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddCoupleRegistration");
                        if (BizActionObj.PatientDetails.GenderID == 1L)
                        {
                            this.dbServer.AddInParameter(command3, "MalePatientID", DbType.Int64, patientDetails.GeneralDetails.PatientID);
                            this.dbServer.AddInParameter(command3, "FemalePatientID", DbType.Int64, patientDetails.SpouseDetails.PatientID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "MalePatientID", DbType.Int64, patientDetails.SpouseDetails.PatientID);
                            this.dbServer.AddInParameter(command3, "FemalePatientID", DbType.Int64, patientDetails.GeneralDetails.PatientID);
                        }
                        this.dbServer.AddInParameter(command3, "MalePatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "FemalePatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "RegistrationDate", DbType.DateTime, patientDetails.GeneralDetails.RegistrationDate);
                        this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, patientDetails.Status);
                        this.dbServer.AddInParameter(command3, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, patientDetails.AddedDateTime);
                        this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddParameter(command3, "CoupleRgistrationNumber", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "000000000000000000000000000000000000000000000000000");
                        this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                        this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
                        if (BizActionObj.BankDetails != null)
                        {
                            DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientBankDetails");
                            this.dbServer.AddInParameter(command4, "PatientID", DbType.Int64, patientDetails.SpouseDetails.PatientID);
                            this.dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.UnitId);
                            this.dbServer.AddInParameter(command4, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command4, "BankID", DbType.Int64, BizActionObj.BankDetails.BankId);
                            this.dbServer.AddInParameter(command4, "BranchID", DbType.Int64, BizActionObj.BankDetails.BranchId);
                            this.dbServer.AddInParameter(command4, "IFSCCode", DbType.String, BizActionObj.BankDetails.IFSCCode);
                            this.dbServer.AddInParameter(command4, "AccountType", DbType.Boolean, BizActionObj.BankDetails.AccountTypeId);
                            this.dbServer.AddInParameter(command4, "AccountNo", DbType.String, BizActionObj.BankDetails.AccountNumber);
                            this.dbServer.AddInParameter(command4, "AccountHolderName", DbType.String, BizActionObj.BankDetails.AccountHolderName);
                            this.dbServer.AddInParameter(command4, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, patientDetails.AddedDateTime);
                            this.dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientDetails.ID);
                            this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command4, transaction);
                            BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command4, "ResultStatus");
                        }
                    }
                    if (BizActionObj.BankDetails != null)
                    {
                        DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientBankDetails");
                        this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, patientDetails.GeneralDetails.PatientID);
                        this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.UnitId);
                        this.dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "BankID", DbType.Int64, BizActionObj.BankDetails.BankId);
                        this.dbServer.AddInParameter(command, "BranchID", DbType.Int64, BizActionObj.BankDetails.BranchId);
                        this.dbServer.AddInParameter(command, "IFSCCode", DbType.String, BizActionObj.BankDetails.IFSCCode);
                        this.dbServer.AddInParameter(command, "AccountType", DbType.Boolean, BizActionObj.BankDetails.AccountTypeId);
                        this.dbServer.AddInParameter(command, "AccountNo", DbType.String, BizActionObj.BankDetails.AccountNumber);
                        this.dbServer.AddInParameter(command, "AccountHolderName", DbType.String, BizActionObj.BankDetails.AccountHolderName);
                        this.dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, patientDetails.AddedDateTime);
                        this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientDetails.ID);
                        this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command, transaction);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                    }
                }
                if (BizActionObj.IsSaveSponsor)
                {
                    clsBasePatientSposorDAL instance = clsBasePatientSposorDAL.GetInstance();
                    BizActionObj.BizActionVOSaveSponsor.PatientSponsorDetails.PatientId = BizActionObj.PatientDetails.GeneralDetails.PatientID;
                    BizActionObj.BizActionVOSaveSponsor.PatientSponsorDetails.PatientUnitId = BizActionObj.PatientDetails.GeneralDetails.UnitId;
                    BizActionObj.BizActionVOSaveSponsor = (clsAddPatientSponsorBizActionVO) instance.AddPatientSponsorDetailsWithTransaction(BizActionObj.BizActionVOSaveSponsor, UserVo, pConnection, transaction);
                    if (BizActionObj.BizActionVOSaveSponsor.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    if ((BizActionObj.PatientDetails.SpouseDetails != null) && ((BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 7L) || ((BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 8L) || (BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 9))))
                    {
                        clsBasePatientSposorDAL rdal2 = clsBasePatientSposorDAL.GetInstance();
                        BizActionObj.BizActionVOSaveSponsorForMale.PatientSponsorDetails.PatientId = BizActionObj.PatientDetails.SpouseDetails.PatientID;
                        BizActionObj.BizActionVOSaveSponsorForMale.PatientSponsorDetails.PatientUnitId = BizActionObj.PatientDetails.SpouseDetails.UnitId;
                        BizActionObj.BizActionVOSaveSponsorForMale = (clsAddPatientSponsorBizActionVO) rdal2.AddPatientSponsorDetailsWithTransaction(BizActionObj.BizActionVOSaveSponsorForMale, UserVo, pConnection, transaction);
                        if (BizActionObj.BizActionVOSaveSponsorForMale.SuccessStatus == -1)
                        {
                            throw new Exception();
                        }
                    }
                }
                if (myConnection == null)
                {
                    transaction.Commit();
                }
                this.addPatientUserDetails(BizActionObj, UserVo);
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                if (myConnection == null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (myConnection == null)
                {
                    pConnection.Close();
                }
            }
            return BizActionObj;
        }

        public override IValueObject AddPatientDietPlan(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientDietPlanBizActionVO bizActionObj = valueObject as clsAddPatientDietPlanBizActionVO;
            bizActionObj = (bizActionObj.DietPlan.ID != 0L) ? this.UpdateDiet(bizActionObj, UserVo) : this.AddDiet(bizActionObj, UserVo);
            return valueObject;
        }

        public override IValueObject AddPatientForPathology(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientForPathologyBizActionVO nvo = valueObject as clsAddPatientForPathologyBizActionVO;
            DbTransaction transaction = null;
            DbConnection pConnection = null;
            try
            {
                pConnection = this.dbServer.CreateConnection();
                pConnection.Open();
                transaction = pConnection.BeginTransaction();
                clsPatientVO patientDetails = nvo.PatientDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatientForPathology");
                this.dbServer.AddInParameter(storedProcCommand, "PatientCategoryID", DbType.Int64, patientDetails.PatientCategoryIDForPath);
                this.dbServer.AddInParameter(storedProcCommand, "RegistrationDate", DbType.DateTime, patientDetails.GeneralDetails.RegistrationDate);
                if (patientDetails.GeneralDetails.LastName != null)
                {
                    patientDetails.GeneralDetails.LastName = patientDetails.GeneralDetails.LastName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.LastName));
                if (patientDetails.GeneralDetails.FirstName != null)
                {
                    patientDetails.GeneralDetails.FirstName = patientDetails.GeneralDetails.FirstName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.FirstName));
                if (patientDetails.GeneralDetails.MiddleName != null)
                {
                    patientDetails.GeneralDetails.MiddleName = patientDetails.GeneralDetails.MiddleName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.MiddleName));
                this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, patientDetails.GenderID);
                this.dbServer.AddInParameter(storedProcCommand, "DateOfBirth", DbType.DateTime, patientDetails.GeneralDetails.DateOfBirth);
                this.dbServer.AddInParameter(storedProcCommand, "MaritalStatusID", DbType.Int64, patientDetails.MaritalStatusID);
                if (patientDetails.ContactNo1 != null)
                {
                    patientDetails.ContactNo1 = patientDetails.ContactNo1.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo1", DbType.String, patientDetails.ContactNo1);
                if (patientDetails.ContactNo2 != null)
                {
                    patientDetails.ContactNo2 = patientDetails.ContactNo2.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo2", DbType.String, patientDetails.ContactNo2);
                if (patientDetails.Email != null)
                {
                    patientDetails.Email = patientDetails.Email.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Email", DbType.String, Security.base64Encode(patientDetails.Email));
                this.dbServer.AddInParameter(storedProcCommand, "PrefixId", DbType.Int64, patientDetails.PrefixId);
                this.dbServer.AddInParameter(storedProcCommand, "MobileCountryCode", DbType.String, patientDetails.MobileCountryCode);
                this.dbServer.AddInParameter(storedProcCommand, "ResiNoCountryCode", DbType.Int64, patientDetails.ResiNoCountryCode);
                this.dbServer.AddInParameter(storedProcCommand, "ResiSTDCode", DbType.Int64, patientDetails.ResiSTDCode);
                this.dbServer.AddInParameter(storedProcCommand, "RegType", DbType.Int64, patientDetails.GeneralDetails.RegType);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyName", DbType.String, patientDetails.CompanyName);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, patientDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, patientDetails.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "????????????????????");
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientDetails.GeneralDetails.PatientID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.PatientDetails.GeneralDetails.PatientID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.PatientDetails.GeneralDetails.UnitId = UserVo.UserLoginInfo.UnitId;
                nvo.BizActionVOSaveSponsor.PatientSponsorDetails.PatientId = nvo.PatientDetails.GeneralDetails.PatientID;
                nvo.BizActionVOSaveSponsor.PatientSponsorDetails.PatientUnitId = nvo.PatientDetails.GeneralDetails.UnitId;
                nvo.BizActionVOSaveSponsor = (clsAddPatientSponsorBizActionVO) clsBasePatientSposorDAL.GetInstance().AddPatientSponsorDetailsWithTransaction(nvo.BizActionVOSaveSponsor, UserVo, pConnection, transaction);
                if (nvo.BizActionVOSaveSponsor.SuccessStatus == -1)
                {
                    throw new Exception();
                }
                if (!nvo.PatientDetails.IsVisitForPatho)
                {
                    nvo.BizActionVOSaveVisit = new clsAddVisitBizActionVO();
                    nvo.BizActionVOSaveVisit.VisitDetails = new clsVisitVO();
                    nvo.BizActionVOSaveVisit.VisitDetails.PatientId = nvo.PatientDetails.GeneralDetails.PatientID;
                    nvo.BizActionVOSaveVisit.VisitDetails.PatientUnitId = nvo.PatientDetails.GeneralDetails.UnitId;
                    nvo.BizActionVOSaveVisit.VisitDetails.VisitTypeID = nvo.PatientDetails.VisitTypeID;
                    nvo.BizActionVOSaveVisit = (clsAddVisitBizActionVO) clsBaseVisitDAL.GetInstance().AddVisitForPathology(nvo.BizActionVOSaveVisit, UserVo, pConnection, transaction);
                    if (nvo.BizActionVOSaveVisit.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                pConnection.Close();
                transaction = null;
                pConnection = null;
            }
            return valueObject;
        }

        public override IValueObject AddPatientForPathology(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {
            clsAddPatientForPathologyBizActionVO nvo = valueObject as clsAddPatientForPathologyBizActionVO;
            DbTransaction transaction = null;
            DbConnection pConnection = null;
            try
            {
                clsPatientVO patientDetails = nvo.PatientDetails;
                pConnection = (myConnection == null) ? this.dbServer.CreateConnection() : myConnection;
                if (pConnection.State == ConnectionState.Closed)
                {
                    pConnection.Open();
                }
                transaction = (myTransaction == null) ? pConnection.BeginTransaction() : myTransaction;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatientForPathology");
                this.dbServer.AddInParameter(storedProcCommand, "PatientCategoryID", DbType.Int64, patientDetails.PatientCategoryIDForPath);
                this.dbServer.AddInParameter(storedProcCommand, "RegistrationDate", DbType.DateTime, patientDetails.GeneralDetails.RegistrationDate);
                if (patientDetails.GeneralDetails.LastName != null)
                {
                    patientDetails.GeneralDetails.LastName = patientDetails.GeneralDetails.LastName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.LastName));
                if (patientDetails.GeneralDetails.FirstName != null)
                {
                    patientDetails.GeneralDetails.FirstName = patientDetails.GeneralDetails.FirstName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.FirstName));
                if (patientDetails.GeneralDetails.MiddleName != null)
                {
                    patientDetails.GeneralDetails.MiddleName = patientDetails.GeneralDetails.MiddleName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.MiddleName));
                this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, patientDetails.GenderID);
                this.dbServer.AddInParameter(storedProcCommand, "DateOfBirth", DbType.DateTime, patientDetails.GeneralDetails.DateOfBirth);
                this.dbServer.AddInParameter(storedProcCommand, "MaritalStatusID", DbType.Int64, patientDetails.MaritalStatusID);
                if (patientDetails.ContactNo1 != null)
                {
                    patientDetails.ContactNo1 = patientDetails.ContactNo1.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo1", DbType.String, patientDetails.ContactNo1);
                if (patientDetails.ContactNo2 != null)
                {
                    patientDetails.ContactNo2 = patientDetails.ContactNo2.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo2", DbType.String, patientDetails.ContactNo2);
                if (patientDetails.Email != null)
                {
                    patientDetails.Email = patientDetails.Email.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Email", DbType.String, Security.base64Encode(patientDetails.Email));
                this.dbServer.AddInParameter(storedProcCommand, "PrefixId", DbType.Int64, patientDetails.PrefixId);
                this.dbServer.AddInParameter(storedProcCommand, "MobileCountryCode", DbType.String, patientDetails.MobileCountryCode);
                this.dbServer.AddInParameter(storedProcCommand, "ResiNoCountryCode", DbType.Int64, patientDetails.ResiNoCountryCode);
                this.dbServer.AddInParameter(storedProcCommand, "ResiSTDCode", DbType.Int64, patientDetails.ResiSTDCode);
                this.dbServer.AddInParameter(storedProcCommand, "RegType", DbType.Int64, patientDetails.GeneralDetails.RegType);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyName", DbType.String, patientDetails.CompanyName);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, patientDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, patientDetails.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "????????????????????");
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientDetails.GeneralDetails.PatientID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.PatientDetails.GeneralDetails.PatientID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.PatientDetails.GeneralDetails.UnitId = UserVo.UserLoginInfo.UnitId;
                nvo.BizActionVOSaveSponsor.PatientSponsorDetails.PatientId = nvo.PatientDetails.GeneralDetails.PatientID;
                nvo.BizActionVOSaveSponsor.PatientSponsorDetails.PatientUnitId = nvo.PatientDetails.GeneralDetails.UnitId;
                nvo.BizActionVOSaveSponsor = (clsAddPatientSponsorBizActionVO) clsBasePatientSposorDAL.GetInstance().AddPatientSponsorDetailsWithTransaction(nvo.BizActionVOSaveSponsor, UserVo, pConnection, transaction);
                if (nvo.BizActionVOSaveSponsor.SuccessStatus == -1)
                {
                    throw new Exception();
                }
                if (!nvo.PatientDetails.IsVisitForPatho)
                {
                    nvo.BizActionVOSaveVisit = new clsAddVisitBizActionVO();
                    nvo.BizActionVOSaveVisit.VisitDetails = new clsVisitVO();
                    nvo.BizActionVOSaveVisit.VisitDetails.PatientId = nvo.PatientDetails.GeneralDetails.PatientID;
                    nvo.BizActionVOSaveVisit.VisitDetails.PatientUnitId = nvo.PatientDetails.GeneralDetails.UnitId;
                    nvo.BizActionVOSaveVisit.VisitDetails.VisitTypeID = nvo.PatientDetails.VisitTypeID;
                    nvo.BizActionVOSaveVisit = (clsAddVisitBizActionVO) clsBaseVisitDAL.GetInstance().AddVisitForPathology(nvo.BizActionVOSaveVisit, UserVo, pConnection, transaction);
                    if (nvo.BizActionVOSaveVisit.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                }
                if (myConnection == null)
                {
                    transaction.Commit();
                }
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                if (myConnection == null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (myConnection == null)
                {
                    pConnection.Close();
                }
            }
            return valueObject;
        }

        public override IValueObject AddPatientIPDWithTransaction(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientBizActionVO bizActionObj = valueObject as clsAddPatientBizActionVO;
            bizActionObj = (bizActionObj.PatientDetails.GeneralDetails.PatientID != 0L) ? this.InsertFromOPDPatientDetailsIPDWithTransaction(bizActionObj, UserVo) : this.AddPatientDetailsIPDWithTransaction(bizActionObj, UserVo);
            return valueObject;
        }

        public override IValueObject AddPatientLinkFile(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientLinkFileBizActionVO bizActionObj = valueObject as clsAddPatientLinkFileBizActionVO;
            bizActionObj = bizActionObj.FROMEMR ? this.AddEMRImage(bizActionObj, UserVo) : ((bizActionObj.PatientDetails[0].ID != 0L) ? this.UpdateLinkfile(bizActionObj, UserVo) : this.AddLinkfile(bizActionObj, UserVo));
            return valueObject;
        }

        public override IValueObject AddPatientOPDWithTransaction(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientBizActionVO bizActionObj = valueObject as clsAddPatientBizActionVO;
            bizActionObj = (bizActionObj.PatientDetails.GeneralDetails.PatientID != 0L) ? this.UpdatePatientDetailsOPDWithTransaction(bizActionObj, UserVo) : this.AddPatientDetailsOPDWithTransaction(bizActionObj, UserVo, null, null);
            return valueObject;
        }

        public override IValueObject AddPatientOPDWithTransaction(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {
            clsAddPatientBizActionVO bizActionObj = valueObject as clsAddPatientBizActionVO;
            if (bizActionObj.PatientDetails.GeneralDetails.PatientID == 0L)
            {
                bizActionObj = this.AddPatientDetailsOPDWithTransaction(bizActionObj, UserVo, myConnection, myTransaction);
            }
            return valueObject;
        }

        public override IValueObject AddPatientScanDoc(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientScanDocument document = valueObject as clsAddUpdatePatientScanDocument;
            try
            {
                foreach (clsPatientScanDocumentVO tvo in document.PatientScanDocList)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientScanDocument");
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, tvo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "IdentityID", DbType.Int64, tvo.IdentityID);
                    this.dbServer.AddInParameter(storedProcCommand, "IdentityNumber", DbType.String, tvo.IdentityNumber);
                    this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, tvo.Description);
                    this.dbServer.AddInParameter(storedProcCommand, "IsForSpouse", DbType.Boolean, tvo.IsForSpouse);
                    this.dbServer.AddInParameter(storedProcCommand, "AttachedFileName", DbType.String, tvo.AttachedFileName);
                    this.dbServer.AddInParameter(storedProcCommand, "AttachedFileContent", DbType.Binary, null);
                    if (tvo.DoctorID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, tvo.DoctorID);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, tvo.PatientID);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, tvo.PatientUnitID);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "ImagePath", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    tvo.ImageName = Convert.ToString(this.dbServer.GetParameterValue(storedProcCommand, "ImagePath"));
                    DemoImage image = new DemoImage();
                    if (tvo.AttachedFileContent != null)
                    {
                        image.VaryQualityLevelScanDocument(tvo.AttachedFileContent, tvo.ImageName, this.DocSaveLocation);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject ADDPatientSignConsent(IValueObject valueObject, clsUserVO UserVo)
        {
            clsADDPatientSignConsentBizActionVO nvo = valueObject as clsADDPatientSignConsentBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                clsPatientSignConsentVO signConsentDetails = nvo.signConsentDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ADD_PatientSignConsent");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int32, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ConsentID", DbType.Int64, Convert.ToInt64(signConsentDetails.ConsentID));
                this.dbServer.AddInParameter(storedProcCommand, "ConsentUnitID", DbType.Int64, Convert.ToInt64(signConsentDetails.ConsentUnitID));
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, Convert.ToInt64(signConsentDetails.PatientID));
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, Convert.ToInt64(signConsentDetails.PatientUnitID));
                this.dbServer.AddInParameter(storedProcCommand, "ReferredBy", DbType.String, Convert.ToString(UserVo.LoginName));
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, Convert.ToDateTime(signConsentDetails.Date));
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, Convert.ToDateTime(signConsentDetails.Time));
                this.dbServer.AddInParameter(storedProcCommand, "DocumentName", DbType.String, Convert.ToString(signConsentDetails.DocumentName));
                this.dbServer.AddInParameter(storedProcCommand, "SourceURL", DbType.String, signConsentDetails.SourceURL);
                this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, Convert.ToString(signConsentDetails.Remarks));
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, Convert.ToBoolean(signConsentDetails.Status));
                this.dbServer.AddInParameter(storedProcCommand, "Report", DbType.Binary, signConsentDetails.Report);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, Convert.ToInt64(signConsentDetails.PlanTherapyID));
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, Convert.ToInt64(signConsentDetails.PlanTherapyUnitID));
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, signConsentDetails.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.signConsentDetails.ID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.signConsentDetails = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return valueObject;
        }

        private void addPatientUserDetails(clsAddPatientBizActionVO BizActionObj, clsUserVO UserVo)
        {
        }

        public override IValueObject AddPrintedPatientConscent(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPrintedPatientConscentBizActionVO bizActionObj = valueObject as clsAddPrintedPatientConscentBizActionVO;
            if (bizActionObj.ConsentDetails.ID == 0L)
            {
                bizActionObj = this.AddConsent(bizActionObj, UserVo);
            }
            return valueObject;
        }

        public override IValueObject AddSurrogate(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientBizActionVO bizActionObj = valueObject as clsAddPatientBizActionVO;
            bizActionObj = (bizActionObj.PatientDetails.GeneralDetails.PatientID != 0L) ? this.UpdateSurrogateDetails(bizActionObj, UserVo) : this.AddSurrogateDetails(bizActionObj, UserVo);
            return valueObject;
        }

        private clsAddPatientBizActionVO AddSurrogateDetails(clsAddPatientBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                clsPatientVO patientDetails = BizActionObj.PatientDetails;
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatient");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, patientDetails.GeneralDetails.LinkServer);
                if (patientDetails.GeneralDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, patientDetails.GeneralDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "PatientCategoryID", DbType.Int64, patientDetails.GeneralDetails.PatientTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "ReferralTypeID", DbType.Int64, patientDetails.GeneralDetails.ReferralTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyName", DbType.String, Security.base64Encode(patientDetails.CompanyName));
                this.dbServer.AddInParameter(storedProcCommand, "RegistrationDate", DbType.DateTime, patientDetails.GeneralDetails.RegistrationDate);
                if (patientDetails.GeneralDetails.LastName != null)
                {
                    patientDetails.GeneralDetails.LastName = patientDetails.GeneralDetails.LastName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.LastName));
                if (patientDetails.GeneralDetails.FirstName != null)
                {
                    patientDetails.GeneralDetails.FirstName = patientDetails.GeneralDetails.FirstName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.FirstName));
                if (patientDetails.GeneralDetails.MiddleName != null)
                {
                    patientDetails.GeneralDetails.MiddleName = patientDetails.GeneralDetails.MiddleName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.MiddleName));
                if (patientDetails.FamilyName != null)
                {
                    patientDetails.FamilyName = patientDetails.FamilyName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "FamilyName", DbType.String, Security.base64Encode(patientDetails.FamilyName));
                this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, patientDetails.GenderID);
                this.dbServer.AddInParameter(storedProcCommand, "DateOfBirth", DbType.DateTime, patientDetails.GeneralDetails.DateOfBirth);
                this.dbServer.AddInParameter(storedProcCommand, "BloodGroupID", DbType.Int64, patientDetails.BloodGroupID);
                this.dbServer.AddInParameter(storedProcCommand, "MaritalStatusID", DbType.Int64, patientDetails.MaritalStatusID);
                this.dbServer.AddInParameter(storedProcCommand, "CivilID", DbType.String, Security.base64Encode(patientDetails.CivilID));
                if (patientDetails.ContactNo1 != null)
                {
                    patientDetails.ContactNo1 = patientDetails.ContactNo1.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo1", DbType.String, patientDetails.ContactNo1);
                if (patientDetails.ContactNo2 != null)
                {
                    patientDetails.ContactNo2 = patientDetails.ContactNo2.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo2", DbType.String, patientDetails.ContactNo2);
                if (patientDetails.FaxNo != null)
                {
                    patientDetails.FaxNo = patientDetails.FaxNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "FaxNo", DbType.String, patientDetails.FaxNo);
                if (patientDetails.Email != null)
                {
                    patientDetails.Email = patientDetails.Email.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Email", DbType.String, Security.base64Encode(patientDetails.Email));
                if (patientDetails.AddressLine1 != null)
                {
                    patientDetails.AddressLine1 = patientDetails.AddressLine1.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine1", DbType.String, Security.base64Encode(patientDetails.AddressLine1));
                if (patientDetails.AddressLine2 != null)
                {
                    patientDetails.AddressLine2 = patientDetails.AddressLine2.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine2", DbType.String, Security.base64Encode(patientDetails.AddressLine2));
                if (patientDetails.AddressLine3 != null)
                {
                    patientDetails.AddressLine3 = patientDetails.AddressLine3.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine3", DbType.String, Security.base64Encode(patientDetails.AddressLine3));
                if (patientDetails.Country != null)
                {
                    patientDetails.Country = patientDetails.Country.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Country", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "State", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "City", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "Taluka", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "Area", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "District", DbType.String, null);
                if (patientDetails.CountryID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CountryID", DbType.Int64, patientDetails.CountryID);
                }
                if (patientDetails.StateID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "StateID", DbType.Int64, patientDetails.StateID);
                }
                if (patientDetails.CityID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CityID", DbType.Int64, patientDetails.CityID);
                }
                if (patientDetails.RegionID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "RegionID", DbType.Int64, patientDetails.RegionID);
                }
                this.dbServer.AddInParameter(storedProcCommand, "MobileCountryCode", DbType.String, patientDetails.MobileCountryCode);
                this.dbServer.AddInParameter(storedProcCommand, "ResiNoCountryCode", DbType.Int64, patientDetails.ResiNoCountryCode);
                this.dbServer.AddInParameter(storedProcCommand, "ResiSTDCode", DbType.Int64, patientDetails.ResiSTDCode);
                if (patientDetails.Pincode != null)
                {
                    patientDetails.Pincode = patientDetails.Pincode.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Pincode", DbType.String, Security.base64Encode(patientDetails.Pincode));
                this.dbServer.AddInParameter(storedProcCommand, "ReligionID", DbType.Int64, patientDetails.ReligionID);
                this.dbServer.AddInParameter(storedProcCommand, "OccupationId", DbType.Int64, patientDetails.OccupationId);
                if (patientDetails.OldRegistrationNo != null)
                {
                    patientDetails.OldRegistrationNo = patientDetails.OldRegistrationNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "OldRegistrationNo", DbType.String, patientDetails.OldRegistrationNo);
                this.dbServer.AddInParameter(storedProcCommand, "Photo", DbType.Binary, patientDetails.Photo);
                this.dbServer.AddInParameter(storedProcCommand, "IsLoyaltyMember", DbType.Boolean, patientDetails.IsLoyaltyMember);
                this.dbServer.AddInParameter(storedProcCommand, "LoyaltyCardID", DbType.Int64, patientDetails.LoyaltyCardID);
                this.dbServer.AddInParameter(storedProcCommand, "RelationID", DbType.Int64, patientDetails.RelationID);
                this.dbServer.AddInParameter(storedProcCommand, "ParentPatientID", DbType.Int64, patientDetails.ParentPatientID);
                this.dbServer.AddInParameter(storedProcCommand, "IssueDate", DbType.DateTime, patientDetails.IssueDate);
                this.dbServer.AddInParameter(storedProcCommand, "EffectiveDate", DbType.DateTime, patientDetails.EffectiveDate);
                this.dbServer.AddInParameter(storedProcCommand, "ExpiryDate", DbType.DateTime, patientDetails.ExpiryDate);
                if (patientDetails.LoyaltyCardNo != null)
                {
                    patientDetails.LoyaltyCardNo = patientDetails.LoyaltyCardNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "LoyaltyCardNo", DbType.String, patientDetails.LoyaltyCardNo);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, patientDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                this.dbServer.AddParameter(storedProcCommand, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????");
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientDetails.GeneralDetails.PatientID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddParameter(storedProcCommand, "ImagePath", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.PatientDetails.GeneralDetails.PatientID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                BizActionObj.PatientDetails.GeneralDetails.MRNo = (string) this.dbServer.GetParameterValue(storedProcCommand, "MRNo");
                BizActionObj.PatientDetails.ImageName = Convert.ToString(this.dbServer.GetParameterValue(storedProcCommand, "ImagePath"));
                string parameterValue = (string) this.dbServer.GetParameterValue(storedProcCommand, "Err");
                BizActionObj.PatientDetails.GeneralDetails.UnitId = UserVo.UserLoginInfo.UnitId;
                DemoImage image = new DemoImage();
                if (patientDetails.Photo != null)
                {
                    image.VaryQualityLevel(patientDetails.Photo, BizActionObj.PatientDetails.ImageName, this.ImgSaveLocation);
                }
                if ((BizActionObj.PatientDetails.SpouseDetails != null) && (BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 10))
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddSurrogateSpouseInformation");
                    this.dbServer.AddInParameter(command2, "SurrogateID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.PatientID);
                    this.dbServer.AddInParameter(command2, "SurrogateUnitID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.UnitId);
                    this.dbServer.AddInParameter(command2, "PatientCategoryID", DbType.Int64, patientDetails.GeneralDetails.PatientTypeID);
                    this.dbServer.AddInParameter(command2, "ReferralTypeID", DbType.Int64, patientDetails.GeneralDetails.ReferralTypeID);
                    this.dbServer.AddInParameter(command2, "RegistrationDate", DbType.DateTime, patientDetails.GeneralDetails.RegistrationDate);
                    if (patientDetails.SpouseDetails.LastName != null)
                    {
                        patientDetails.SpouseDetails.LastName = patientDetails.SpouseDetails.LastName.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "LastName", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.LastName));
                    if (patientDetails.SpouseDetails.FirstName != null)
                    {
                        patientDetails.SpouseDetails.FirstName = patientDetails.SpouseDetails.FirstName.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "FirstName", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.FirstName));
                    if (patientDetails.SpouseDetails.MiddleName != null)
                    {
                        patientDetails.SpouseDetails.MiddleName = patientDetails.SpouseDetails.MiddleName.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "MiddleName", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.MiddleName));
                    if (patientDetails.SpouseDetails.FamilyName != null)
                    {
                        patientDetails.SpouseDetails.FamilyName = patientDetails.SpouseDetails.FamilyName.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "FamilyName", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.FamilyName));
                    this.dbServer.AddInParameter(command2, "GenderID", DbType.Int64, patientDetails.SpouseDetails.GenderID);
                    this.dbServer.AddInParameter(command2, "DateOfBirth", DbType.DateTime, patientDetails.SpouseDetails.DateOfBirth);
                    this.dbServer.AddInParameter(command2, "BloodGroupID", DbType.Int64, patientDetails.SpouseDetails.BloodGroupID);
                    this.dbServer.AddInParameter(command2, "MaritalStatusID", DbType.Int64, patientDetails.SpouseDetails.MaritalStatusID);
                    this.dbServer.AddInParameter(command2, "CivilID", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.CivilID));
                    if (patientDetails.SpouseDetails.ContactNo1 != null)
                    {
                        patientDetails.SpouseDetails.ContactNo1 = patientDetails.SpouseDetails.ContactNo1.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "ContactNo1", DbType.String, patientDetails.SpouseDetails.ContactNo1);
                    if (patientDetails.SpouseDetails.ContactNo2 != null)
                    {
                        patientDetails.SpouseDetails.ContactNo2 = patientDetails.SpouseDetails.ContactNo2.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "ContactNo2", DbType.String, patientDetails.SpouseDetails.ContactNo2);
                    if (patientDetails.SpouseDetails.FaxNo != null)
                    {
                        patientDetails.SpouseDetails.FaxNo = patientDetails.SpouseDetails.FaxNo.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "FaxNo", DbType.String, patientDetails.SpouseDetails.FaxNo);
                    if (patientDetails.SpouseDetails.Email != null)
                    {
                        patientDetails.SpouseDetails.Email = patientDetails.SpouseDetails.Email.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "Email", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.Email));
                    if (patientDetails.SpouseDetails.AddressLine1 != null)
                    {
                        patientDetails.SpouseDetails.AddressLine1 = patientDetails.SpouseDetails.AddressLine1.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "AddressLine1", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.AddressLine1));
                    if (patientDetails.SpouseDetails.AddressLine2 != null)
                    {
                        patientDetails.SpouseDetails.AddressLine2 = patientDetails.SpouseDetails.AddressLine2.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "AddressLine2", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.AddressLine2));
                    if (patientDetails.SpouseDetails.AddressLine3 != null)
                    {
                        patientDetails.SpouseDetails.AddressLine3 = patientDetails.SpouseDetails.AddressLine3.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "AddressLine3", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.AddressLine3));
                    if (patientDetails.SpouseDetails.Country != null)
                    {
                        patientDetails.SpouseDetails.Country = patientDetails.SpouseDetails.Country.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "Country", DbType.String, null);
                    this.dbServer.AddInParameter(command2, "State", DbType.String, null);
                    this.dbServer.AddInParameter(command2, "City", DbType.String, null);
                    this.dbServer.AddInParameter(command2, "Taluka", DbType.String, null);
                    this.dbServer.AddInParameter(command2, "Area", DbType.String, null);
                    this.dbServer.AddInParameter(command2, "District", DbType.String, null);
                    if (patientDetails.SpouseDetails.CountryID > 0L)
                    {
                        this.dbServer.AddInParameter(command2, "CountryID", DbType.Int64, patientDetails.SpouseDetails.CountryID);
                    }
                    if (patientDetails.SpouseDetails.StateID > 0L)
                    {
                        this.dbServer.AddInParameter(command2, "StateID", DbType.Int64, patientDetails.SpouseDetails.StateID);
                    }
                    if (patientDetails.SpouseDetails.CityID > 0L)
                    {
                        this.dbServer.AddInParameter(command2, "CityID", DbType.Int64, patientDetails.SpouseDetails.CityID);
                    }
                    if (patientDetails.SpouseDetails.RegionID > 0L)
                    {
                        this.dbServer.AddInParameter(command2, "RegionID", DbType.Int64, patientDetails.SpouseDetails.RegionID);
                    }
                    this.dbServer.AddInParameter(command2, "MobileCountryCode", DbType.String, patientDetails.SpouseDetails.MobileCountryCode);
                    this.dbServer.AddInParameter(command2, "ResiNoCountryCode", DbType.Int64, patientDetails.SpouseDetails.ResiNoCountryCode);
                    this.dbServer.AddInParameter(command2, "ResiSTDCode", DbType.Int64, patientDetails.SpouseDetails.ResiSTDCode);
                    if (patientDetails.SpouseDetails.Pincode != null)
                    {
                        patientDetails.Pincode = patientDetails.SpouseDetails.Pincode.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "Pincode", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.Pincode));
                    this.dbServer.AddInParameter(command2, "CompanyName", DbType.String, Security.base64Encode(patientDetails.CompanyName));
                    this.dbServer.AddInParameter(command2, "ReligionID", DbType.Int64, patientDetails.SpouseDetails.ReligionID);
                    this.dbServer.AddInParameter(command2, "OccupationId", DbType.Int64, patientDetails.SpouseDetails.OccupationId);
                    this.dbServer.AddInParameter(command2, "Photo", DbType.Binary, patientDetails.SpouseDetails.Photo);
                    this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, patientDetails.Status);
                    this.dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    patientDetails.SpouseDetails.MRNo = patientDetails.GeneralDetails.MRNo.Remove(patientDetails.GeneralDetails.MRNo.Length - 1, 1);
                    this.dbServer.AddParameter(command2, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                    this.dbServer.AddInParameter(command2, "ID", DbType.Int64, patientDetails.SpouseDetails.PatientID);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                    string text2 = (string) this.dbServer.GetParameterValue(command2, "Err");
                }
                DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddSurrogateOtherInformation");
                this.dbServer.AddInParameter(command3, "SurrogateID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.PatientID);
                this.dbServer.AddInParameter(command3, "SurrogateUnitID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.UnitId);
                this.dbServer.AddInParameter(command3, "ReferralTypeID", DbType.Int64, patientDetails.GeneralDetails.ReferralTypeID);
                this.dbServer.AddInParameter(command3, "AgencyID", DbType.String, patientDetails.AgencyID);
                this.dbServer.AddInParameter(command3, "SurrogateOtherDetails", DbType.String, patientDetails.SurrogateOtherDetails);
                this.dbServer.AddInParameter(command3, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command3, "ID", DbType.Int64, 0);
                this.dbServer.AddInParameter(command3, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(command3, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
                DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_AddPatientSponsor");
                this.dbServer.AddInParameter(command4, "LinkServer", DbType.String, null);
                this.dbServer.AddInParameter(command4, "LinkServerAlias", DbType.String, null);
                this.dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.PatientID);
                this.dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.UnitId);
                this.dbServer.AddInParameter(command4, "PatientSourceID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.PatientSourceID);
                this.dbServer.AddInParameter(command4, "CompanyID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.CompanyID);
                this.dbServer.AddInParameter(command4, "AssociatedCompanyID", DbType.Int64, 0);
                this.dbServer.AddInParameter(command4, "ReferenceNo", DbType.String, null);
                this.dbServer.AddInParameter(command4, "CreditLimit", DbType.Double, 0);
                this.dbServer.AddInParameter(command4, "EffectiveDate", DbType.DateTime, patientDetails.EffectiveDate);
                this.dbServer.AddInParameter(command4, "ExpiryDate", DbType.DateTime, patientDetails.ExpiryDate);
                this.dbServer.AddInParameter(command4, "TariffID", DbType.Int64, patientDetails.TariffID);
                this.dbServer.AddInParameter(command4, "EmployeeNo", DbType.String, null);
                this.dbServer.AddInParameter(command4, "DesignationID", DbType.Int64, null);
                this.dbServer.AddInParameter(command4, "Remark", DbType.String, null);
                this.dbServer.AddInParameter(command4, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(command4, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command4, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(command4, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command4, "ResultStatus");
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw exception;
            }
            finally
            {
                transaction.Dispose();
                transaction = null;
            }
            return BizActionObj;
        }

        public override IValueObject CheckPatientDuplicacy(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCheckPatientDuplicacyBizActionVO nvo = valueObject as clsCheckPatientDuplicacyBizActionVO;
            try
            {
                clsPatientVO patientDetails = nvo.PatientDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_PatientDuplicacy");
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, Security.base64Encode(nvo.PatientDetails.GeneralDetails.FirstName));
                this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, Security.base64Encode(nvo.PatientDetails.GeneralDetails.LastName));
                this.dbServer.AddInParameter(storedProcCommand, "Gender", DbType.Int64, nvo.PatientDetails.GeneralDetails.GenderID);
                this.dbServer.AddInParameter(storedProcCommand, "DOB", DbType.DateTime, nvo.PatientDetails.GeneralDetails.DateOfBirth);
                this.dbServer.AddInParameter(storedProcCommand, "MobileCountryCode", DbType.String, nvo.PatientDetails.MobileCountryCode);
                this.dbServer.AddInParameter(storedProcCommand, "ContactNO1", DbType.String, nvo.PatientDetails.GeneralDetails.ContactNO1);
                this.dbServer.AddInParameter(storedProcCommand, "SFirstName", DbType.String, Security.base64Encode(nvo.PatientDetails.SpouseDetails.FirstName));
                this.dbServer.AddInParameter(storedProcCommand, "SLastName", DbType.String, Security.base64Encode(nvo.PatientDetails.SpouseDetails.LastName));
                this.dbServer.AddInParameter(storedProcCommand, "SGender", DbType.Int64, nvo.PatientDetails.SpouseDetails.GenderID);
                this.dbServer.AddInParameter(storedProcCommand, "SDOB", DbType.DateTime, nvo.PatientDetails.SpouseDetails.DateOfBirth);
                this.dbServer.AddInParameter(storedProcCommand, "SMobileCountryCode", DbType.String, nvo.PatientDetails.SpouseDetails.MobileCountryCode);
                this.dbServer.AddInParameter(storedProcCommand, "SContactNO1", DbType.String, nvo.PatientDetails.SpouseDetails.ContactNO1);
                this.dbServer.AddInParameter(storedProcCommand, "PatientEditMode", DbType.Boolean, nvo.PatientEditMode);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientDetails.GeneralDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientDetails.GeneralDetails.UnitId);
                if (nvo.PatientDetails.GeneralDetails.PanNumber != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "PanNumber", DbType.String, nvo.PatientDetails.GeneralDetails.PanNumber);
                }
                if (nvo.PatientDetails.SpouseDetails.SpousePanNumber != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "SpousePanNumber", DbType.String, nvo.PatientDetails.SpouseDetails.SpousePanNumber);
                }
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                bool hasRows = reader.HasRows;
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject CheckPatientFamilyRegisterd(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCheckPatientMemberRegisteredBizActionVO nvo = (clsCheckPatientMemberRegisteredBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CheckPatientMemberRegistered");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "RelationID", DbType.Int64, nvo.RelationID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.SuccessStatus = true;
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject clsGetPatientPenPusherDetailByID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPenPusherDetailByIDBizActionVO nvo = valueObject as clsGetPatientPenPusherDetailByIDBizActionVO;
            nvo.PatientPrescriptionDetails = new clsPatientPerscriptionInfoVO();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientPenPusherDetailsByID");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.PatientID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    nvo.PatientPrescriptionDetailsList = new List<clsPatientPerscriptionInfoVO>();
                    while (reader.Read())
                    {
                        clsPatientPerscriptionInfoVO item = new clsPatientPerscriptionInfoVO {
                            Days = Convert.ToInt64(DALHelper.HandleDBNull(reader["Days"])),
                            Dose = Convert.ToString(DALHelper.HandleDBNull(reader["Dose"])),
                            DrugID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DrugID"])),
                            Frequency = Convert.ToString(DALHelper.HandleDBNull(reader["Frequency"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            PrescriptionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PrescriptionID"])),
                            Quantity = Convert.ToInt16(DALHelper.HandleDBNull(reader["Quantity"])),
                            Reason = Convert.ToString(DALHelper.HandleDBNull(reader["Reason"])),
                            Route = Convert.ToString(DALHelper.HandleDBNull(reader["Route"])),
                            Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]))
                        };
                        nvo.PatientPrescriptionDetailsList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject DeletePatientSignConsent(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeletePatientSignConsentBizActionVO nvo = valueObject as clsDeletePatientSignConsentBizActionVO;
            try
            {
                clsPatientSignConsentVO deleteVO = nvo.DeleteVO;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_Delete_SignConsent");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, deleteVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, deleteVO.UnitID);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject GetCoupleGeneralDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCoupleGeneralDetailsListBizActionVO nvo = valueObject as clsGetCoupleGeneralDetailsListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCoupleGeneralDetailsListForSearch");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "MrNo", DbType.String, nvo.MRNo);
                this.dbServer.AddInParameter(storedProcCommand, "SearchKeyword", DbType.String, nvo.SearchKeyword);
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, nvo.FirstName);
                if ((nvo.MiddleName != null) && (nvo.MiddleName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, nvo.MiddleName);
                }
                if ((nvo.LastName != null) && (nvo.LastName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, nvo.LastName);
                }
                if ((nvo.FamilyName != null) && ((nvo.FamilyName.Length != 0) && ((nvo.FamilyName != null) && (nvo.FamilyName.Length != 0))))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FamilyName", DbType.String, Security.base64Encode(nvo.FamilyName));
                }
                if ((nvo.OPDNo != null) && (nvo.OPDNo.Length != 0))
                {
                    if (nvo.VisitWise)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "OPDNo", DbType.String, nvo.OPDNo + "%");
                    }
                    else if (nvo.AdmissionWise)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "AdmissionNo", DbType.String, nvo.OPDNo + "%");
                    }
                }
                if ((nvo.ContactNo != null) && (nvo.ContactNo.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ContactNo", DbType.String, nvo.ContactNo + "%");
                }
                if ((nvo.Country != null) && (nvo.Country.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Country", DbType.String, Security.base64Encode(nvo.Country) + "%");
                }
                if ((nvo.State != null) && (nvo.State.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "State", DbType.String, Security.base64Encode(nvo.State) + "%");
                }
                if ((nvo.City != null) && (nvo.City.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "City", DbType.String, Security.base64Encode(nvo.City) + "%");
                }
                if ((nvo.Pincode != null) && (nvo.Pincode.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Pincode", DbType.String, Security.base64Encode(nvo.Pincode) + "%");
                }
                if ((nvo.CivilID != null) && (nvo.CivilID.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CivilID", DbType.String, Security.base64Encode(nvo.CivilID) + "%");
                }
                long genderID = nvo.GenderID;
                if (nvo.GenderID != 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, nvo.GenderID);
                }
                long patientCategoryID = nvo.PatientCategoryID;
                if (nvo.PatientCategoryID != 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "PatientCategory", DbType.Int64, nvo.PatientCategoryID);
                }
                if (nvo.FromDate != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                }
                if (nvo.ToDate != null)
                {
                    if (nvo.FromDate != null)
                    {
                        nvo.ToDate = new DateTime?(nvo.ToDate.Value.Date.AddDays(1.0));
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                }
                if (nvo.VisitWise)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Serchfor", DbType.Int16, (short) 1);
                    if (nvo.VisitFromDate != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "VisitFromDate", DbType.DateTime, nvo.VisitFromDate);
                    }
                    if (nvo.VisitToDate != null)
                    {
                        if (nvo.VisitFromDate != null)
                        {
                            nvo.VisitToDate = new DateTime?(nvo.VisitToDate.Value.Date.AddDays(1.0));
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "VisitToDate", DbType.DateTime, nvo.VisitToDate);
                    }
                }
                if (nvo.AdmissionWise)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Serchfor", DbType.Int16, (short) 2);
                    if (nvo.AdmissionFromDate != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "AdmissionFromDate", DbType.DateTime, nvo.AdmissionFromDate);
                    }
                    if (nvo.AdmissionToDate != null)
                    {
                        if (nvo.AdmissionToDate != null)
                        {
                            nvo.AdmissionToDate = new DateTime?(nvo.AdmissionToDate.Value.Date.AddDays(1.0));
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "AdmissionToDate", DbType.DateTime, nvo.AdmissionToDate);
                    }
                }
                if (nvo.DOBWise)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Serchfor", DbType.Int16, (short) 0);
                    if (nvo.DOBFromDate != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "DOBFromDate", DbType.DateTime, nvo.DOBFromDate);
                    }
                    if (nvo.DOBToDate != null)
                    {
                        if (nvo.DOBToDate != null)
                        {
                            nvo.DOBToDate = new DateTime?(nvo.DOBToDate.Value.Date.AddDays(1.0));
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "DOBToDate", DbType.DateTime, nvo.DOBToDate);
                    }
                }
                if (nvo.IsLoyaltyMember)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IsLoyaltyMember", DbType.Boolean, nvo.IsLoyaltyMember);
                    this.dbServer.AddInParameter(storedProcCommand, "LoyaltyProgramID", DbType.Int64, nvo.LoyaltyProgramID);
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "RegistrationTypeID", DbType.Int64, nvo.RegistrationTypeID);
                if (nvo.SearchInAnotherClinic)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "SearchInAnotherClinic", DbType.Boolean, nvo.SearchInAnotherClinic);
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, nvo.LinkServer);
                if (nvo.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, nvo.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientDetailsList == null)
                    {
                        nvo.PatientDetailsList = new List<clsPatientGeneralVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientGeneralVO item = new clsPatientGeneralVO {
                            PatientID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitId = (long) DALHelper.HandleDBNull(reader["UnitId"]),
                            MRNo = (string) DALHelper.HandleDBNull(reader["MRNo"]),
                            LastName = (string) DALHelper.HandleDBNull(reader["LastName"]),
                            FirstName = (string) DALHelper.HandleDBNull(reader["FirstName"]),
                            MiddleName = (string) DALHelper.HandleDBNull(reader["MiddleName"]),
                            DateOfBirth = DALHelper.HandleDate(reader["DateofBirth"])
                        };
                        DateTime? nullable17 = DALHelper.HandleDate(reader["RegistrationDate"]);
                        item.RegistrationDate = new DateTime?(nullable17.Value);
                        if (!nvo.VisitWise && !nvo.RegistrationWise)
                        {
                            item.IPDAdmissionID = (long) DALHelper.HandleDBNull(reader["VAID"]);
                            item.IPDAdmissionNo = (string) DALHelper.HandleDBNull(reader["OINO"]);
                        }
                        else
                        {
                            item.VisitID = (long) DALHelper.HandleDBNull(reader["VAID"]);
                            item.OPDNO = (string) DALHelper.HandleDBNull(reader["OINO"]);
                        }
                        if (nvo.VisitWise)
                        {
                            item.PatientKind = PatientsKind.OPD;
                        }
                        else if (nvo.AdmissionWise)
                        {
                            item.PatientKind = PatientsKind.IPD;
                        }
                        else if ((item.VisitID == 0L) && (item.IPDAdmissionID == 0L))
                        {
                            item.PatientKind = PatientsKind.Registration;
                        }
                        item.TariffID = (long) DALHelper.HandleDBNull(reader["TariffID"]);
                        item.PatientSourceID = (long) DALHelper.HandleDBNull(reader["PatientSourceID"]);
                        item.CompanyID = (long) DALHelper.HandleDBNull(reader["CompanyID"]);
                        item.ContactNO1 = (string) DALHelper.HandleDBNull(reader["ContactNO1"]);
                        item.Gender = (string) DALHelper.HandleDBNull(reader["Gender"]);
                        item.MaritalStatus = (string) DALHelper.HandleDBNull(reader["MaritalStatus"]);
                        item.UniversalID = Security.base64Decode((string) DALHelper.HandleDBNull(reader["CivilID"]));
                        item.PatientTypeID = (long) DALHelper.HandleDBNull(reader["PatientCategoryID"]);
                        item.LinkServer = nvo.LinkServer;
                        nvo.PatientDetailsList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject GetDietPlanMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGeDietPlanMasterBizActionVO nvo = valueObject as clsGeDietPlanMasterBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDietPlanDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.PlanID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DietDetailList == null)
                    {
                        nvo.DietDetailList = new List<clsPatientDietPlanDetailVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientDietPlanDetailVO item = new clsPatientDietPlanDetailVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            DietPlanID = (long) DALHelper.HandleDBNull(reader["PlanID"]),
                            FoodItemID = (long) DALHelper.HandleDBNull(reader["ItemID"]),
                            FoodItem = (string) DALHelper.HandleDBNull(reader["FoodItemName"]),
                            FoodItemCategory = (string) DALHelper.HandleDBNull(reader["FoodItemCatName"]),
                            FoodItemCategoryID = (long) DALHelper.HandleDBNull(reader["ItemCategoryID"]),
                            Timing = (string) DALHelper.HandleDBNull(reader["Timing"]),
                            FoodQty = (string) DALHelper.HandleDBNull(reader["ItemQty"]),
                            FoodUnit = (string) DALHelper.HandleDBNull(reader["ItemUnit"]),
                            FoodCal = (string) DALHelper.HandleDBNull(reader["ItemCal"]),
                            FoodCH = (string) DALHelper.HandleDBNull(reader["ItemCH"]),
                            FoodFat = (string) DALHelper.HandleDBNull(reader["ItemFat"]),
                            FoodExpectedCal = (string) DALHelper.HandleDBNull(reader["ItemExpectedCal"]),
                            FoodInstruction = (string) DALHelper.HandleDBNull(reader["ItemInstruction"])
                        };
                        nvo.GeneralInfo = (string) DALHelper.HandleDBNull(reader["PlanInformation"]);
                        nvo.DietDetailList.Add(item);
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

        public override IValueObject GetEMRAdmVisitListByPatientID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetEMRAdmVisitListByPatientIDBizActionVO nvo = (clsGetEMRAdmVisitListByPatientIDBizActionVO) valueObject;
            nvo.PatientDetails = new clsPatientConsoleHeaderVO();
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientDetailsConsole");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ISOPDIPD", DbType.Int64, nvo.ISOPDIPD);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientDetails == null)
                    {
                        nvo.PatientDetails = new clsPatientConsoleHeaderVO();
                    }
                    while (reader.Read())
                    {
                        nvo.PatientDetails.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        nvo.PatientDetails.LastName = Convert.ToString(DALHelper.HandleDBNull(reader["SurName"]));
                        nvo.PatientDetails.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        nvo.PatientDetails.AgeInYearMonthDays = Convert.ToString(DALHelper.HandleDBNull(reader["AgeInYearMonthDays"]));
                        nvo.PatientDetails.Age = Convert.ToInt32(DALHelper.HandleDBNull(reader["Age"]));
                        nvo.PatientDetails.AgeInYearMonthDays = (nvo.PatientDetails.Age <= 1) ? (nvo.PatientDetails.Age.ToString() + " Year") : (nvo.PatientDetails.Age.ToString() + " Years");
                        nvo.PatientDetails.RegisteredClinic = (string) DALHelper.HandleDBNull(reader["RegisteredClinic"]);
                        nvo.PatientDetails.MaritalStatus = Convert.ToString(reader["MaritalStatus"]);
                        nvo.PatientDetails.Gender = Convert.ToString(reader["Gender"]);
                        nvo.PatientDetails.MOB = Convert.ToString(reader["MOB"]);
                        nvo.PatientDetails.BirthDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["DateOfBirth"])));
                        nvo.PatientDetails.Photo = (byte[]) DALHelper.HandleDBNull(reader["Photo"]);
                        string[] strArray = new string[] { "https://", this.ImgIP, "/", this.ImgVirtualDir, "/", Convert.ToString(DALHelper.HandleDBNull(reader["ImgPath"])) };
                        nvo.PatientDetails.ImageName = string.Concat(strArray);
                        nvo.PatientDetails.Religion = Convert.ToString(reader["Religion"]);
                        nvo.PatientDetails.Allergies = Convert.ToString(reader["Allergies"]);
                        nvo.PatientDetails.Weight = Convert.ToInt64(DALHelper.HandleDBNull(reader["Weight"]));
                        nvo.PatientDetails.Height = Convert.ToInt64(DALHelper.HandleDBNull(reader["Height"]));
                        nvo.PatientDetails.BMI = nvo.PatientDetails.Weight / ((nvo.PatientDetails.Height * nvo.PatientDetails.Height) / 10000.0);
                        if (double.IsNaN(nvo.PatientDetails.BMI))
                        {
                            nvo.PatientDetails.BMI = 0.0;
                        }
                        if (double.IsInfinity(nvo.PatientDetails.BMI))
                        {
                            nvo.PatientDetails.BMI = 0.0;
                        }
                    }
                }
                DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_GetEMRAdmVisitListByPatientID");
                this.dbServer.AddInParameter(command2, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(command2, "UnitId", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(command2, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(command2, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(command2, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddParameter(command2, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                reader = (DbDataReader) this.dbServer.ExecuteReader(command2);
                if (reader.HasRows)
                {
                    if (nvo.EMRList == null)
                    {
                        nvo.EMRList = new List<clsPatientConsoleHeaderVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientConsoleHeaderVO item = new clsPatientConsoleHeaderVO {
                            ID = Convert.ToInt64(reader["ID"]),
                            OPD_IPD_ID = Convert.ToInt64(reader["VisitAdmID"]),
                            DoctorName = Convert.ToString(reader["DoctorName"]),
                            DoctorCode = Convert.ToString(reader["DoctorID"]),
                            DepartmentCode = Convert.ToString(reader["DepartmentID"]),
                            DepartmentName = Convert.ToString(reader["DepartmentName"]),
                            IPDOPDNO = Convert.ToString(reader["IPDOPDNO"]),
                            IPDOPD = Convert.ToString(reader["IPDOPD"]),
                            OPD_IPD = Convert.ToBoolean(reader["OPD_IPD"]),
                            Date = (DateTime?) DALHelper.HandleDBNull(reader["Date"]),
                            ISDischarged = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ISDischarged"])),
                            Bed = Convert.ToString(DALHelper.HandleDBNull(reader["Bed"]))
                        };
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_GetEMRENcounterListFlags");
                        this.dbServer.AddInParameter(command3, "PatientID", DbType.Int64, nvo.ID);
                        this.dbServer.AddInParameter(command3, "OPD_IPD_ID", DbType.Int64, item.OPD_IPD_ID);
                        this.dbServer.AddInParameter(command3, "OPD_IPD", DbType.Boolean, item.OPD_IPD);
                        this.dbServer.AddInParameter(command3, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                        DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(command3);
                        if (reader2.HasRows)
                        {
                            item.IsAllergy = "Visible";
                            item.IsNonAllergy = "Collapsed";
                        }
                        reader2.NextResult();
                        if (reader2.HasRows)
                        {
                            item.IsDiagnosisRight = "Visible";
                            item.IsDiagnosisWrong = "Collapsed";
                        }
                        reader2.NextResult();
                        if (reader2.HasRows)
                        {
                            item.IsLaboratoryRight = "Visible";
                            item.IsLaboratoryWrong = "Collapsed";
                        }
                        reader2.NextResult();
                        if (reader2.HasRows)
                        {
                            item.IsRadiologyRight = "Visible";
                            item.IsRadiologyWrong = "Collapsed";
                        }
                        reader2.NextResult();
                        if (reader2.HasRows)
                        {
                            item.IsPrescriptionRight = "Visible";
                            item.IsPrescriptionwrong = "Collapsed";
                        }
                        reader2.NextResult();
                        if (reader2.HasRows)
                        {
                            while (reader2.Read())
                            {
                                item.RoundID = Convert.ToInt64(reader2["RoundID"]);
                                item.currDocSpecCode = Convert.ToString(reader2["SpecID"]);
                                item.currDocSpecName = Convert.ToString(reader2["SpecName"]);
                                item.CurrDoctorCode = Convert.ToString(reader2["DoctorID"]);
                                item.currDoctorName = Convert.ToString(reader2["DoctorName"]);
                            }
                        }
                        reader2.NextResult();
                        if (reader2.HasRows)
                        {
                            item.IsProcedure = "Visible";
                            item.IsNonProcedure = "Collapsed";
                        }
                        nvo.EMRList.Add(item);
                        reader2.Close();
                    }
                }
                reader.NextResult();
                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(command2, "TotalRows"));
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetEMRAdmVisitListByPatientIDForConsol(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetEMRAdmVisitListByPatientIDBizActionVO nvo = (clsGetEMRAdmVisitListByPatientIDBizActionVO) valueObject;
            nvo.PatientDetails = new clsPatientConsoleHeaderVO();
            DbDataReader reader = null;
            int num = 0;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPrescriptionForPatientForConsol");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "CallFrom", DbType.Int32, nvo.CallFrom);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.EMRList == null)
                    {
                        nvo.EMRList = new List<clsPatientConsoleHeaderVO>();
                    }
                    while (reader.Read())
                    {
                        num++;
                        clsPatientConsoleHeaderVO item = new clsPatientConsoleHeaderVO {
                            RowNum = Convert.ToInt64(DALHelper.HandleDBNull(reader["RowNum"])),
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            PatientId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientId"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]))),
                            DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"])),
                            DepartmentName = Convert.ToString(DALHelper.HandleDBNull(reader["DepartmentName"]))
                        };
                        nvo.EMRList.Add(item);
                    }
                }
                reader.NextResult();
                num = 0;
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetIPDPatient(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetIPDPatientBizActionVO nvo = valueObject as clsGetIPDPatientBizActionVO;
            try
            {
                clsGetIPDPatientVO patientDetails = nvo.PatientDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientIPDDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, patientDetails.GeneralDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, patientDetails.GeneralDetails.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "MrNo", DbType.String, patientDetails.GeneralDetails.MRNo);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.PatientDetails.GeneralDetails.IPDAdmissionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.PatientDetails.GeneralDetails.IPDPatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        nvo.PatientDetails.GeneralDetails.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        nvo.PatientDetails.GeneralDetails.IPDAdmissionNo = Convert.ToString(DALHelper.HandleDBNull(reader["IPDNO"]));
                        nvo.PatientDetails.GeneralDetails.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"]));
                        nvo.PatientDetails.GeneralDetails.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.PatientDetails.GeneralDetails.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        nvo.PatientDetails.GeneralDetails.BedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToBedID"]));
                        nvo.PatientDetails.GeneralDetails.WardID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToWardID"]));
                        nvo.PatientDetails.GeneralDetails.ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToBedCategoryID"]));
                        nvo.PatientDetails.GeneralDetails.BedName = Convert.ToString(DALHelper.HandleDBNull(reader["BedName"]));
                        nvo.PatientDetails.GeneralDetails.WardName = Convert.ToString(DALHelper.HandleDBNull(reader["WardName"]));
                        nvo.PatientDetails.GeneralDetails.ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]));
                        nvo.PatientDetails.GeneralDetails.RegistrationDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"])));
                        nvo.PatientDetails.GeneralDetails.IsDischarged = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDischarged"]));
                        nvo.PatientDetails.GeneralDetails.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                        nvo.PatientDetails.GeneralDetails.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                        nvo.PatientDetails.GeneralDetails.Unit = Convert.ToString(DALHelper.HandleDBNull(reader["Unit"]));
                        nvo.PatientDetails.GeneralDetails.TariffID = Convert.ToInt64(reader["PatientTariffID"]);
                        nvo.PatientDetails.GeneralDetails.IsReadyForDischarged = Convert.ToBoolean(reader["IsReadyForDischarged"]);
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

        public override IValueObject GetIPDPatientList(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetIPDPatientListBizActionVO nvo = valueObject as clsGetIPDPatientListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIPDPatientList");
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, nvo.GeneralDetails.FirstName);
                this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, nvo.GeneralDetails.MiddleName);
                this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, nvo.GeneralDetails.LastName);
                this.dbServer.AddInParameter(storedProcCommand, "FamilyName", DbType.String, nvo.GeneralDetails.FamilyName);
                this.dbServer.AddInParameter(storedProcCommand, "MRNO", DbType.String, nvo.GeneralDetails.MRNo);
                this.dbServer.AddInParameter(storedProcCommand, "AdmissionNO", DbType.String, nvo.GeneralDetails.IPDAdmissionNo);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.GeneralDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsFromDischarge", DbType.Boolean, nvo.IsFromDischarge);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.IPDPatientList == null)
                    {
                        nvo.IPDPatientList = new List<clsPatientGeneralVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientGeneralVO item = new clsPatientGeneralVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            IPDAdmissionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionID"])),
                            PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"])),
                            FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])),
                            MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"])),
                            FamilyName = Convert.ToString(DALHelper.HandleDBNull(reader["FamilyName"])),
                            DateOfBirth = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["DOB"]))),
                            LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])),
                            IPDAdmissionNo = Convert.ToString(DALHelper.HandleDBNull(reader["IPDNO"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"])),
                            BedName = Convert.ToString(DALHelper.HandleDBNull(reader["BedName"])),
                            BedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedID"])),
                            WardName = Convert.ToString(DALHelper.HandleDBNull(reader["WardName"])),
                            WardID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WardID"])),
                            ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"])),
                            ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassID"])),
                            RegistrationDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["AdmissionDate"]))),
                            MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"])),
                            PatientSourceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceID"])),
                            BillingToBedCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillingToBedCategoryID"])),
                            AdmissionTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionType"])),
                            AdmissionUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionUnitID"]))
                        };
                        nvo.IPDPatientList.Add(item);
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

        public override IValueObject GetOTPatientGeneralDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetOTPatientGeneralDetailsListBizActionVO nvo = valueObject as clsGetOTPatientGeneralDetailsListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientGeneralDetailsListForSearch_OT_1");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "IsCurrentAdmitted", DbType.Boolean, nvo.IsCurrentAdmitted);
                if ((nvo.OPDNo != null) && (nvo.OPDNo.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "OPDNo", DbType.String, nvo.OPDNo + "%");
                }
                if ((nvo.IPDNo != null) && (nvo.IPDNo.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "AdmissionNo", DbType.String, "%" + nvo.IPDNo + "%");
                }
                if ((nvo.MRNo != null) && (nvo.MRNo.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MrNo", DbType.String, "%" + nvo.MRNo + "%");
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MrNo", DbType.String, nvo.MRNo + "%");
                }
                if ((nvo.FirstName != null) && (nvo.FirstName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, nvo.FirstName + "%");
                }
                if ((nvo.MiddleName != null) && (nvo.MiddleName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, nvo.MiddleName + "%");
                }
                if ((nvo.LastName != null) && (nvo.LastName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, nvo.LastName + "%");
                }
                if ((nvo.FamilyName != null) && ((nvo.FamilyName.Length != 0) && ((nvo.FamilyName != null) && (nvo.FamilyName.Length != 0))))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FamilyName", DbType.String, nvo.FamilyName + "%");
                }
                if ((nvo.ContactNo != null) && (nvo.ContactNo.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ContactNo1", DbType.String, nvo.ContactNo + "%");
                }
                if (nvo.VisitWise)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Serchfor", DbType.Int16, (short) 1);
                }
                else if (nvo.AdmissionWise && !nvo.RegistrationWise)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Serchfor", DbType.Int16, (short) 2);
                }
                else if (nvo.RegistrationWise)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Serchfor", DbType.Int16, (short) 0);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Serchfor", DbType.Int16, (short) 0);
                }
                if (nvo.VisitFromDate != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "VisitFromDate", DbType.DateTime, nvo.VisitFromDate);
                }
                if (nvo.VisitToDate != null)
                {
                    if (nvo.VisitFromDate != null)
                    {
                        nvo.VisitToDate = new DateTime?(nvo.VisitToDate.Value.Date.AddDays(1.0));
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "VisitToDate", DbType.DateTime, nvo.VisitToDate);
                }
                if (nvo.AdmissionFromDate != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "AdmissionFromDate", DbType.DateTime, nvo.AdmissionFromDate);
                }
                if (nvo.AdmissionToDate != null)
                {
                    if (nvo.AdmissionToDate != null)
                    {
                        nvo.AdmissionToDate = new DateTime?(nvo.AdmissionToDate.Value.Date.AddDays(1.0));
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "AdmissionToDate", DbType.DateTime, nvo.AdmissionToDate);
                }
                if (nvo.FromDate != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                }
                if (nvo.ToDate != null)
                {
                    if (nvo.ToDate != null)
                    {
                        nvo.ToDate = new DateTime?(nvo.ToDate.Value.Date);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientDetailsList == null)
                    {
                        nvo.PatientDetailsList = new List<clsPatientGeneralVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientGeneralVO item = new clsPatientGeneralVO {
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"])),
                            FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])),
                            MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"])),
                            LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])),
                            FamilyName = Convert.ToString(DALHelper.HandleDBNull(reader["FamilyName"])),
                            DateOfBirth = DALHelper.HandleDate(reader["DateofBirth"]),
                            RegistrationDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["RegistrationDate"]))),
                            ContactNO1 = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNO1"])),
                            Gender = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"])),
                            MaritalStatus = Convert.ToString(DALHelper.HandleDBNull(reader["MaritalStatus"])),
                            Religion = Convert.ToString(DALHelper.HandleDBNull(reader["Religion"])),
                            Photo = (byte[]) DALHelper.HandleDBNull(reader["Photo"])
                        };
                        string str = Convert.ToString(DALHelper.HandleDBNull(reader["ImagePath"]));
                        string[] strArray = new string[] { "https://", this.ImgIP, "/", this.ImgVirtualDir, "/", str };
                        item.ImageName = string.Concat(strArray);
                        item.LinkServer = nvo.LinkServer;
                        nvo.PatientDetailsList.Add(item);
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
            return valueObject;
        }

        public override IValueObject GetOTPatientPackageList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPackageListBizActionVO nvo = valueObject as clsGetPatientPackageListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientWithPackageListForSearch");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                if ((nvo.MRNo != null) && (nvo.MRNo.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MrNo", DbType.String, "%" + nvo.MRNo + "%");
                }
                if ((nvo.FirstName != null) && (nvo.FirstName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, nvo.FirstName + "%");
                }
                if ((nvo.AddressLine1 != null) && (nvo.AddressLine1.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "AddressLine1", DbType.String, Security.base64Encode(nvo.AddressLine1) + "%");
                }
                if ((nvo.MiddleName != null) && (nvo.MiddleName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, nvo.MiddleName + "%");
                }
                if ((nvo.LastName != null) && (nvo.LastName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, nvo.LastName + "%");
                }
                if ((nvo.FamilyName != null) && ((nvo.FamilyName.Length != 0) && ((nvo.FamilyName != null) && (nvo.FamilyName.Length != 0))))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FamilyName", DbType.String, nvo.FamilyName + "%");
                }
                if ((nvo.OPDNo != null) && (nvo.OPDNo.Length != 0))
                {
                    if (nvo.VisitWise)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "OPDNo", DbType.String, nvo.OPDNo + "%");
                    }
                    else if (nvo.AdmissionWise)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "AdmissionNo", DbType.String, nvo.OPDNo + "%");
                    }
                    else if (nvo.isfromMaterialConsumpation)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "OPDNo", DbType.String, nvo.OPDNo + "%");
                    }
                }
                if ((nvo.ContactNo != null) && (nvo.ContactNo.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ContactNo", DbType.String, nvo.ContactNo + "%");
                }
                if ((nvo.DonarCode != null) && (nvo.DonarCode.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "DonarCode", DbType.String, nvo.DonarCode + "%");
                }
                if ((nvo.OldRegistrationNo != null) && (nvo.OldRegistrationNo.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "OldRegistrationNo", DbType.String, nvo.OldRegistrationNo + "%");
                }
                if ((nvo.Country != null) && (nvo.Country.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Country", DbType.String, Security.base64Encode(nvo.Country) + "%");
                }
                if ((nvo.State != null) && (nvo.State.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "State", DbType.String, Security.base64Encode(nvo.State) + "%");
                }
                if ((nvo.City != null) && (nvo.City.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "City", DbType.String, Security.base64Encode(nvo.City) + "%");
                }
                if ((nvo.Pincode != null) && (nvo.Pincode.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Pincode", DbType.String, Security.base64Encode(nvo.Pincode) + "%");
                }
                if ((nvo.CivilID != null) && (nvo.CivilID.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CivilID", DbType.String, Security.base64Encode(nvo.CivilID) + "%");
                }
                if ((nvo.ReferenceNo != null) && (nvo.ReferenceNo.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ReferenceNo", DbType.String, nvo.ReferenceNo);
                }
                long genderID = nvo.GenderID;
                if (nvo.GenderID != 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, nvo.GenderID);
                }
                long patientCategoryID = nvo.PatientCategoryID;
                if (nvo.PatientCategoryID != 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "PatientCategory", DbType.Int64, nvo.PatientCategoryID);
                }
                if (nvo.DOB != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "DOB", DbType.DateTime, nvo.DOB);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "DOB", DbType.DateTime, null);
                }
                if (nvo.FromDate != null)
                {
                    nvo.FromDate = new DateTime?(nvo.FromDate.Value.Date);
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                }
                if (nvo.ToDate != null)
                {
                    if (nvo.FromDate != null)
                    {
                        nvo.ToDate = new DateTime?(nvo.ToDate.Value.Date.AddDays(1.0));
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                }
                if (nvo.VisitWise)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Serchfor", DbType.Int16, (short) 1);
                    if (nvo.VisitFromDate != null)
                    {
                        nvo.VisitFromDate = new DateTime?(nvo.VisitFromDate.Value.Date);
                        this.dbServer.AddInParameter(storedProcCommand, "VisitFromDate", DbType.DateTime, nvo.VisitFromDate);
                    }
                    if (nvo.VisitToDate != null)
                    {
                        if (nvo.VisitFromDate != null)
                        {
                            nvo.VisitToDate = new DateTime?(nvo.VisitToDate.Value.Date.AddDays(1.0));
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "VisitToDate", DbType.DateTime, nvo.VisitToDate);
                    }
                }
                if (nvo.AdmissionWise)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Serchfor", DbType.Int16, (short) 2);
                    if (nvo.AdmissionFromDate != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "AdmissionFromDate", DbType.DateTime, nvo.AdmissionFromDate);
                    }
                    if (nvo.AdmissionToDate != null)
                    {
                        if (nvo.AdmissionToDate != null)
                        {
                            nvo.AdmissionToDate = new DateTime?(nvo.AdmissionToDate.Value.Date.AddDays(1.0));
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "AdmissionToDate", DbType.DateTime, nvo.AdmissionToDate);
                    }
                }
                if (nvo.DOBWise)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Serchfor", DbType.Int16, (short) 0);
                    if (nvo.DOBFromDate != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "DOBFromDate", DbType.DateTime, nvo.DOBFromDate);
                    }
                    if (nvo.DOBToDate != null)
                    {
                        if (nvo.DOBToDate != null)
                        {
                            nvo.DOBToDate = new DateTime?(nvo.DOBToDate.Value.Date.AddDays(1.0));
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "DOBToDate", DbType.DateTime, nvo.DOBToDate);
                    }
                }
                if (nvo.IsLoyaltyMember)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IsLoyaltyMember", DbType.Boolean, nvo.IsLoyaltyMember);
                    this.dbServer.AddInParameter(storedProcCommand, "LoyaltyProgramID", DbType.Int64, nvo.LoyaltyProgramID);
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "QueueUnitID", DbType.Int64, nvo.PQueueUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "RegistrationTypeID", DbType.Int64, nvo.RegistrationTypeID);
                long pQueueUnitID = nvo.PQueueUnitID;
                if (nvo.PQueueUnitID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FindPatientVisitUnitID", DbType.Int64, nvo.PQueueUnitID);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FindPatientVisitUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                }
                if (nvo.SearchInAnotherClinic)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "SearchInAnotherClinic", DbType.Boolean, nvo.SearchInAnotherClinic);
                }
                this.dbServer.AddInParameter(storedProcCommand, "IdentityID", DbType.Int64, nvo.IdentityID);
                this.dbServer.AddInParameter(storedProcCommand, "IdentityNumber", DbType.String, nvo.IdentityNumber);
                this.dbServer.AddInParameter(storedProcCommand, "SpecialRegID", DbType.Int64, nvo.SpecialRegID);
                this.dbServer.AddInParameter(storedProcCommand, "IsFromQueeManagment", DbType.Boolean, nvo.ISFromQueeManagment);
                this.dbServer.AddInParameter(storedProcCommand, "ShowOutSourceDonor", DbType.Boolean, nvo.ShowOutSourceDonor);
                this.dbServer.AddInParameter(storedProcCommand, "IsFromSurrogacyModule", DbType.Boolean, nvo.IsFromSurrogacyModule);
                this.dbServer.AddInParameter(storedProcCommand, "IsSelfAndDonor", DbType.Int32, nvo.IsSelfAndDonor);
                this.dbServer.AddInParameter(storedProcCommand, "isfromMaterialConsumpation", DbType.Boolean, nvo.isfromMaterialConsumpation);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, nvo.LinkServer);
                if (nvo.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, nvo.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int32, nvo.PackageID);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientDetailsList == null)
                    {
                        nvo.PatientDetailsList = new List<clsPatientGeneralVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientGeneralVO item = new clsPatientGeneralVO {
                            PatientID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            PatientUnitID = (long) DALHelper.HandleDBNull(reader["UnitId"]),
                            UnitId = (long) DALHelper.HandleDBNull(reader["UnitId"]),
                            MRNo = (string) DALHelper.HandleDBNull(reader["MRNo"]),
                            Package = (string) DALHelper.HandleDBNull(reader["Package"]),
                            PackageID = (long) DALHelper.HandleDBNull(reader["PackageID"]),
                            VisitID = (long) DALHelper.HandleDBNull(reader["VAID"]),
                            BillID = (long) DALHelper.HandleDBNull(reader["BillID"]),
                            BillUnitID = (long) DALHelper.HandleDBNull(reader["BillUnitID"]),
                            BillNo = (string) DALHelper.HandleDBNull(reader["BillNo"]),
                            BillDate = new DateTime?((DateTime) DALHelper.HandleDBNull(reader["BillDate"]))
                        };
                        if (nvo.isfromMaterialConsumpation)
                        {
                            item.PatientID = (long) DALHelper.HandleDBNull(reader["PatientId"]);
                            item.PatientUnitID = (long) DALHelper.HandleDBNull(reader["PatientUnitID"]);
                            item.UnitId = (long) DALHelper.HandleDBNull(reader["UnitId"]);
                            item.Opd_Ipd_External_Id = (long) DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"]);
                            item.Opd_Ipd_External_UnitId = (long) DALHelper.HandleDBNull(reader["Opd_Ipd_External_UnitId"]);
                            item.Opd_Ipd_External = Convert.ToInt32(DALHelper.HandleDBNull(reader["Opd_Ipd_External"]));
                            item.OPDNO = Convert.ToString(DALHelper.HandleDBNull(reader["OPD_IPD_NO"]));
                        }
                        item.LastName = (string) DALHelper.HandleDBNull(reader["LastName"]);
                        item.FirstName = (string) DALHelper.HandleDBNull(reader["FirstName"]);
                        item.MiddleName = (string) DALHelper.HandleDBNull(reader["MiddleName"]);
                        item.AddressLine1 = (string) DALHelper.HandleDBNull(reader["AddressLine1"]);
                        item.RegType = Convert.ToInt16(DALHelper.HandleDBNull(reader["RegType"]));
                        item.DateOfBirth = DALHelper.HandleDate(reader["DateofBirth"]);
                        item.Age = Convert.ToInt32(DALHelper.HandleDBNull(reader["Age"]));
                        item.RegistrationDate = new DateTime?(DALHelper.HandleDate(reader["RegistrationDate"]).Value);
                        if (nvo.VisitWise || nvo.RegistrationWise)
                        {
                            item.VisitID = (long) DALHelper.HandleDBNull(reader["VAID"]);
                            item.VisitUnitID = (long) DALHelper.HandleDBNull(reader["VisitUnitID"]);
                            item.VisitUnitId = (long) DALHelper.HandleDBNull(reader["VisitUnitID"]);
                            item.OPDNO = (string) DALHelper.HandleDBNull(reader["OINO"]);
                            item.Unit = Convert.ToString(reader["UnitName"]);
                        }
                        if (nvo.VisitWise)
                        {
                            item.PatientKind = PatientsKind.OPD;
                        }
                        else if (nvo.AdmissionWise)
                        {
                            item.PatientKind = PatientsKind.IPD;
                        }
                        else if ((item.VisitID == 0L) && (item.IPDAdmissionID == 0L))
                        {
                            item.PatientKind = PatientsKind.Registration;
                        }
                        item.TariffID = (long) DALHelper.HandleDBNull(reader["TariffID"]);
                        item.PatientSourceID = (long) DALHelper.HandleDBNull(reader["PatientSourceID"]);
                        item.CompanyID = (long) DALHelper.HandleDBNull(reader["CompanyID"]);
                        item.ContactNO1 = (string) DALHelper.HandleDBNull(reader["ContactNO1"]);
                        item.Gender = (string) DALHelper.HandleDBNull(reader["Gender"]);
                        item.MaritalStatus = (string) DALHelper.HandleDBNull(reader["MaritalStatus"]);
                        item.UniversalID = Security.base64Decode((string) DALHelper.HandleDBNull(reader["CivilID"]));
                        item.PatientTypeID = (long) DALHelper.HandleDBNull(reader["PatientCategoryID"]);
                        item.GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]));
                        item.NewPatientCategoryID = (long) DALHelper.HandleDBNull(reader["NewPatientCategoryID"]);
                        item.ReferenceNo = (string) DALHelper.HandleDBNull(reader["ReferenceNo"]);
                        item.City = (string) DALHelper.HandleDBNull(reader["CityNew"]);
                        item.DonorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DonorCode"]));
                        item.IsSurrogateAlreadyLinked = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSurrogateAlreadyLinked"]));
                        item.DonarCode = Convert.ToString(DALHelper.HandleDBNull(reader["DonarCode"]));
                        item.OldRegistrationNo = Convert.ToString(DALHelper.HandleDBNull(reader["OldRegistrationNo"]));
                        item.IsReferralDoc = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsReferralDoc"]));
                        item.ReferralTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferralTypeID"]));
                        item.ReferralDoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferralDoctorID"]));
                        item.IsAge = (bool) DALHelper.HandleBoolDBNull(reader["IsAge"]);
                        if (item.IsAge)
                        {
                            item.DateOfBirthFromAge = DALHelper.HandleDate(reader["DateOfBirth"]);
                        }
                        else
                        {
                            item.DateOfBirth = DALHelper.HandleDate(reader["DateOfBirth"]);
                        }
                        item.EducationID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["EducationID"]));
                        item.BloodGroupID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["BloodGroupID"]));
                        string str = Convert.ToString(DALHelper.HandleDBNull(reader["ImagePath"]));
                        if (!string.IsNullOrEmpty(str))
                        {
                            string[] strArray = new string[] { "https://", this.ImgIP, "/", this.ImgVirtualDir, "/", str };
                            item.ImageName = string.Concat(strArray);
                        }
                        item.IsDocAttached = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDocAttached"]));
                        item.Email = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["Email"])));
                        item.IdentityType = Convert.ToString(DALHelper.HandleDBNull(reader["IdentityType"]));
                        item.IdentityNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IdentityNumber"]));
                        item.RemarkForPatientType = Convert.ToString(DALHelper.HandleDBNull(reader["RemarkForPatientType"]));
                        item.SpecialRegName = Convert.ToString(DALHelper.HandleDBNull(reader["SpecialReg"]));
                        if (item.RegType == 0)
                        {
                            item.RegistrationType = "OPD";
                        }
                        else if (item.RegType == 1)
                        {
                            item.RegistrationType = "IPD";
                        }
                        else if (item.RegType == 2)
                        {
                            item.RegistrationType = "Pharmacy";
                        }
                        else if (item.RegType == 5)
                        {
                            item.RegistrationType = "Pathology";
                        }
                        if (!nvo.isfromMaterialConsumpation)
                        {
                            item.PanNumber = Convert.ToString(DALHelper.HandleDBNull(reader["PanNumber"]));
                            item.Email = Convert.ToString(DALHelper.HandleDBNull(reader["Email"]));
                            item.NationalityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["NationalityId"]));
                        }
                        item.LinkServer = nvo.LinkServer;
                        if (!nvo.ISFromQueeManagment && !nvo.isfromMaterialConsumpation)
                        {
                            item.BabyWeight = Convert.ToString(DALHelper.HandleDBNull(reader["BabyWeight"]));
                        }
                        nvo.PatientDetailsList.Add(item);
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

        public override IValueObject GetPatient(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientBizActionVO nvo = valueObject as clsGetPatientBizActionVO;
            try
            {
                DbCommand storedProcCommand;
                clsPatientVO patientDetails = nvo.PatientDetails;
                if (nvo.PatientDetails.IsLatestPatient)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetLatestPatientByUnitID");
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatient");
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, patientDetails.GeneralDetails.LinkServer);
                    if (patientDetails.GeneralDetails.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, patientDetails.GeneralDetails.LinkServer.Replace(@"\", "_"));
                    }
                    long surrogateID = nvo.SurrogateID;
                    if (nvo.SurrogateID == 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, patientDetails.GeneralDetails.PatientID);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.SurrogateID);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, patientDetails.GeneralDetails.MRNo);
                    this.dbServer.AddInParameter(storedProcCommand, "SearchFromIPD", DbType.String, patientDetails.GeneralDetails.SearchFromIPD);
                }
                nvo.BankDetails = new clsBankDetailsInfoVO();
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, patientDetails.GeneralDetails.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            storedProcCommand = null;
                           storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientAttachmentFileInformation");


                            this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, patientDetails.GeneralDetails.PatientID);
                            reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                            if (reader.HasRows)
                            {
                                nvo.PatientAttachmentDetailList = new List<clsPatientAttachmentVO>();
                                while (reader.Read())
                                {
                                    clsPatientAttachmentVO item = new clsPatientAttachmentVO {
                                        Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"])),
                                        CasePaperType = Convert.ToString(DALHelper.HandleDBNull(reader["SubJectName"])),
                                        AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"])),
                                        Attachment = (byte[]) DALHelper.HandleDBNull(reader["PDF"]),
                                        Doctor = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]))
                                    };
                                    nvo.PatientAttachmentDetailList.Add(item);
                                }
                            }
                            DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_GetDonor");
                            this.dbServer.AddInParameter(command3, "ID", DbType.Int64, patientDetails.GeneralDetails.PatientID);
                            this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, patientDetails.GeneralDetails.UnitId);
                            reader = (DbDataReader) this.dbServer.ExecuteReader(command3);
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    if (nvo.PatientDetails.GenderID == 2L)
                                    {
                                        nvo.PatientDetails.GeneralDetails.Height = Convert.ToDouble(DALHelper.HandleDBNull(reader["FemaleHeight"]));
                                        nvo.PatientDetails.GeneralDetails.Weight = Convert.ToDouble(DALHelper.HandleDBNull(reader["FemaleWeight"]));
                                        nvo.PatientDetails.GeneralDetails.BMI = Convert.ToDouble(DALHelper.HandleDBNull(reader["FemaleBMI"]));
                                        nvo.PatientDetails.GeneralDetails.Alerts = Convert.ToString(DALHelper.HandleDBNull(reader["FemaleAlerts"]));
                                    }
                                    if (nvo.PatientDetails.GenderID == 1L)
                                    {
                                        nvo.PatientDetails.GeneralDetails.Height = Convert.ToDouble(DALHelper.HandleDBNull(reader["MaleHeight"]));
                                        nvo.PatientDetails.GeneralDetails.Weight = Convert.ToDouble(DALHelper.HandleDBNull(reader["MaleWeight"]));
                                        nvo.PatientDetails.GeneralDetails.BMI = Convert.ToDouble(DALHelper.HandleDBNull(reader["MaleBMI"]));
                                        nvo.PatientDetails.GeneralDetails.Alerts = Convert.ToString(DALHelper.HandleDBNull(reader["MaleAlerts"]));
                                    }
                                }
                            }
                            break;
                        }
                        nvo.PatientDetails.GeneralDetails.PatientTypeID = (long) DALHelper.HandleDBNull(reader["PatientCategoryID"]);
                        nvo.PatientDetails.GeneralDetails.IsDocAttached = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDocAttached"]));
                        nvo.PatientDetails.GeneralDetails.ReferralTypeID = (long) DALHelper.HandleDBNull(reader["ReferralTypeID"]);
                        nvo.PatientDetails.CompanyName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["CompanyName"]));
                        nvo.PatientDetails.GeneralDetails.PatientID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.PatientDetails.GeneralDetails.FirstName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["FirstName"]));
                        nvo.PatientDetails.GeneralDetails.MiddleName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["MiddleName"]));
                        nvo.PatientDetails.GeneralDetails.LastName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["LastName"]));
                        nvo.PatientDetails.GeneralDetails.MRNo = (string) DALHelper.HandleDBNull(reader["MRNo"]);
                        nvo.PatientDetails.GeneralDetails.RegistrationDate = DALHelper.HandleDate(reader["RegistrationDate"]);
                        nvo.PatientDetails.GeneralDetails.IsAge = (bool) DALHelper.HandleBoolDBNull(reader["IsAge"]);
                        if (nvo.PatientDetails.GeneralDetails.IsAge)
                        {
                            nvo.PatientDetails.GeneralDetails.DateOfBirthFromAge = DALHelper.HandleDate(reader["DateOfBirth"]);
                        }
                        else
                        {
                            nvo.PatientDetails.GeneralDetails.DateOfBirth = DALHelper.HandleDate(reader["DateOfBirth"]);
                        }
                        nvo.PatientDetails.GeneralDetails.PatientSourceID = (long) DALHelper.HandleDBNull(reader["PatientSourceID"]);
                        nvo.PatientDetails.GeneralDetails.PatientSponsorCategoryID = (long) DALHelper.HandleDBNull(reader["PatientSponsorCategoryID"]);
                        nvo.PatientDetails.GeneralDetails.TariffID = (long) DALHelper.HandleDBNull(reader["TariffID"]);
                        nvo.PatientDetails.GeneralDetails.RegType = Convert.ToInt16(DALHelper.HandleDBNull(reader["RegType"]));
                        nvo.PatientDetails.FamilyName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["FamilyName"]));
                        nvo.PatientDetails.GenderID = (long) DALHelper.HandleDBNull(reader["GenderID"]);
                        nvo.PatientDetails.BloodGroupID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BloodGroupID"]));
                        nvo.PatientDetails.MaritalStatusID = (long) DALHelper.HandleDBNull(reader["MaritalStatusID"]);
                        nvo.PatientDetails.CivilID = Security.base64Decode((string) DALHelper.HandleDBNull(reader["CivilID"]));
                        nvo.PatientDetails.ContactNo1 = (string) DALHelper.HandleDBNull(reader["ContactNo1"]);
                        nvo.PatientDetails.ContactNo2 = (string) DALHelper.HandleDBNull(reader["ContactNo2"]);
                        nvo.PatientDetails.MobileNo2 = (string) DALHelper.HandleDBNull(reader["MobileNo"]);
                        nvo.PatientDetails.FaxNo = (string) DALHelper.HandleDBNull(reader["FaxNo"]);
                        nvo.PatientDetails.Email = Security.base64Decode((string) DALHelper.HandleDBNull(reader["Email"]));
                        nvo.PatientDetails.AddressLine1 = Security.base64Decode((string) DALHelper.HandleDBNull(reader["AddressLine1"]));
                        nvo.PatientDetails.AddressLine2 = Security.base64Decode((string) DALHelper.HandleDBNull(reader["AddressLine2"]));
                        nvo.PatientDetails.AddressLine3 = Security.base64Decode((string) DALHelper.HandleDBNull(reader["AddressLine3"]));
                        nvo.PatientDetails.Country = Security.base64Decode((string) DALHelper.HandleDBNull(reader["Country"]));
                        nvo.PatientDetails.State = Convert.ToString(DALHelper.HandleDBNull(reader["State"]));
                        nvo.PatientDetails.City = Convert.ToString(DALHelper.HandleDBNull(reader["City"]));
                        nvo.PatientDetails.Taluka = Security.base64Decode((string) DALHelper.HandleDBNull(reader["Taluka"]));
                        nvo.PatientDetails.Area = Convert.ToString(DALHelper.HandleDBNull(reader["Area"]));
                        nvo.PatientDetails.District = Security.base64Decode((string) DALHelper.HandleDBNull(reader["District"]));
                        nvo.PatientDetails.MobileCountryCode = Convert.ToString(DALHelper.HandleDBNull(reader["MobileCountryCode"]));
                        nvo.PatientDetails.ResiNoCountryCode = (long) DALHelper.HandleDBNull(reader["ResiNoCountryCode"]);
                        nvo.PatientDetails.ResiSTDCode = (long) DALHelper.HandleDBNull(reader["ResiSTDCode"]);
                        nvo.PatientDetails.CountryID = DALHelper.HandleIntegerNull(reader["CountryID"]);
                        nvo.PatientDetails.StateID = DALHelper.HandleIntegerNull(reader["StateID"]);
                        nvo.PatientDetails.CityID = DALHelper.HandleIntegerNull(reader["CityID"]);
                        nvo.PatientDetails.RegionID = DALHelper.HandleIntegerNull(reader["RegionID"]);
                        nvo.PatientDetails.ReferralDoctorID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ReferralDoctorID"]));
                        nvo.PatientDetails.IsReferralDoc = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsReferralDoc"]));
                        nvo.PatientDetails.ReferredToDoctorID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ReferredToDoctorID"]));
                        nvo.PatientDetails.ReferralDetail = Convert.ToString(DALHelper.HandleDBNull(reader["ReferralDetail"]));
                        nvo.PatientDetails.ReferralTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferralTypeID"]));
                        nvo.PatientDetails.PrefixId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PrefixId"]));
                        nvo.PatientDetails.IdentityID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["IdentityID"]));
                        nvo.PatientDetails.IdentityNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IdentityNumber"]));
                        nvo.PatientDetails.IsInternationalPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsInternationalPatient"]));
                        nvo.PatientDetails.RemarkForPatientType = Convert.ToString(DALHelper.HandleDBNull(reader["RemarkForPatientType"]));
                        nvo.PatientDetails.Education = Convert.ToString(DALHelper.HandleDBNull(reader["Education"]));
                        nvo.PatientDetails.PanNumber = Convert.ToString(DALHelper.HandleDBNull(reader["PanNumber"]));
                        nvo.PatientDetails.BDID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BDID"]));
                        nvo.PatientDetails.NationalityID = (long) DALHelper.HandleDBNull(reader["NationalityID"]);
                        nvo.PatientDetails.CampID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CampID"]));
                        nvo.PatientDetails.NoOfYearsOfMarriage = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfYearsOfMarriage"]));
                        nvo.PatientDetails.NoOfExistingChildren = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfExistingChildren"]));
                        nvo.PatientDetails.FamilyTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FamilyTypeID"]));
                        nvo.PatientDetails.GeneralDetails.SourceofReference = Convert.ToString(DALHelper.HandleDBNull(reader["SourceofReference"]));
                        nvo.PatientDetails.GeneralDetails.Camp = Convert.ToString(DALHelper.HandleDBNull(reader["Camp"]));
                        nvo.PatientDetails.GeneralDetails.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["ReferralDoctorName"]));
                        nvo.PatientDetails.AgentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgentID"]));
                        nvo.PatientDetails.AgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgencyID"]));
                        nvo.BankDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientBankDetailsID"]));
                        nvo.BankDetails.BankId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BankID"]));
                        nvo.BankDetails.BranchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BranchID"]));
                        nvo.BankDetails.IFSCCode = Convert.ToString(DALHelper.HandleDBNull(reader["IFSCCode"]));
                        nvo.BankDetails.AccountTypeId = Convert.ToBoolean(DALHelper.HandleDBNull(reader["AccountType"]));
                        nvo.BankDetails.AccountNumber = Convert.ToString(DALHelper.HandleDBNull(reader["AccountNo"]));
                        nvo.BankDetails.AccountHolderName = Convert.ToString(DALHelper.HandleDBNull(reader["AccountHoldersName"]));
                        nvo.PatientDetails.GeneralDetails.SonDaughterOf = Security.base64Decode((string) DALHelper.HandleDBNull(reader["DaughterOf"]));
                        nvo.PatientDetails.NationalityID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["NationalityID"]));
                        nvo.PatientDetails.PreferredLangID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PrefLangID"]));
                        nvo.PatientDetails.TreatRequiredID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["TreatRequiredID"]));
                        nvo.PatientDetails.EducationID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["EducationID"]));
                        nvo.PatientDetails.MarriageAnnDate = DALHelper.HandleDate(reader["MarriageAnnivDate"]);
                        nvo.PatientDetails.NoOfPeople = Convert.ToInt32(DALHelper.HandleDBNull(reader["NoOfPeople"]));
                        nvo.PatientDetails.IsClinicVisited = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsClinicVisited"]));
                        nvo.PatientDetails.ClinicName = Convert.ToString(DALHelper.HandleDBNull(reader["ClinicName"]));
                        nvo.PatientDetails.SpecialRegID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["SpecialRegID"]));
                        nvo.PatientDetails.CountryN = Convert.ToString(DALHelper.HandleDBNull(reader["CountryN"]));
                        nvo.PatientDetails.StateN = Convert.ToString(DALHelper.HandleDBNull(reader["State"]));
                        nvo.PatientDetails.CityN = Convert.ToString(DALHelper.HandleDBNull(reader["City"]));
                        nvo.PatientDetails.Region = Convert.ToString(DALHelper.HandleDBNull(reader["Area"]));
                        nvo.PatientDetails.CountryCode = Convert.ToString(DALHelper.HandleDBNull(reader["CountryCode"]));
                        nvo.PatientDetails.StateCode = Convert.ToString(DALHelper.HandleDBNull(reader["StateCode"]));
                        nvo.PatientDetails.CityCode = Convert.ToString(DALHelper.HandleDBNull(reader["CityCode"]));
                        nvo.PatientDetails.RegionCode = Convert.ToString(DALHelper.HandleDBNull(reader["RegionCode"]));
                        string str = Convert.ToString(DALHelper.HandleDBNull(reader["ImgPath"]));
                        if (!string.IsNullOrEmpty(str))
                        {
                            string[] strArray = new string[] { "https://", this.ImgIP, "/", this.ImgVirtualDir, "/", str };
                            nvo.PatientDetails.ImageName = string.Concat(strArray);
                        }
                        nvo.PatientDetails.OldRegistrationNo = Convert.ToString(DALHelper.HandleDBNull(reader["OldRegistrationNo"]));
                        nvo.PatientDetails.Pincode = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["Pincode"])));
                        nvo.PatientDetails.ReligionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReligionID"]));
                        nvo.PatientDetails.OccupationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["OccupationId"]));
                        if (((byte[]) DALHelper.HandleDBNull(reader["Photo"])) != null)
                        {
                            nvo.PatientDetails.Photo = (byte[]) DALHelper.HandleDBNull(reader["Photo"]);
                        }
                        if (nvo.PatientDetails.GeneralDetails.Photo != null)
                        {
                            nvo.PatientDetails.GeneralDetails.Photo = (byte[]) DALHelper.HandleDBNull(reader["Photo"]);
                        }
                        nvo.PatientDetails.IsLoyaltyMember = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsLoyaltyMember"]));
                        nvo.PatientDetails.LoyaltyCardID = (long?) DALHelper.HandleDBNull(reader["LoyaltyCardID"]);
                        nvo.PatientDetails.IssueDate = DALHelper.HandleDate(reader["IssueDate"]);
                        nvo.PatientDetails.EffectiveDate = DALHelper.HandleDate(reader["EffectiveDate"]);
                        nvo.PatientDetails.ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]);
                        nvo.PatientDetails.LoyaltyCardNo = (string) DALHelper.HandleDBNull(reader["LoyaltyCardNo"]);
                        nvo.PatientDetails.RelationID = (long) DALHelper.HandleDBNull(reader["RelationID"]);
                        nvo.PatientDetails.ParentPatientID = (long) DALHelper.HandleDBNull(reader["ParentPatientID"]);
                        nvo.PatientDetails.GeneralDetails.UnitId = (long) DALHelper.HandleDBNull(reader["UnitId"]);
                        nvo.PatientDetails.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                        if (!nvo.PatientDetails.IsLatestPatient)
                        {
                            nvo.PatientDetails.BabyNo = (int?) DALHelper.HandleDBNull(reader["BabyNo"]);
                            nvo.PatientDetails.BabyOfNo = (int?) DALHelper.HandleDBNull(reader["BabyOfNo"]);
                            nvo.PatientDetails.BabyWeight = Convert.ToString(DALHelper.HandleDBNull(reader["BabyWeight"]));
                            nvo.PatientDetails.LinkPatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LinkPatientID"]));
                            nvo.PatientDetails.LinkPatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LinkPatientUnitID"]));
                            nvo.PatientDetails.LinkPatientMrNo = Convert.ToString(DALHelper.HandleDBNull(reader["LinkPatientMrNo"]));
                            nvo.PatientDetails.LinkParentName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["LinkParentFirstName"]))) + " " + Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["LinkParentLastName"])));
                        }
                        if (patientDetails.GeneralDetails.SearchFromIPD)
                        {
                            nvo.PatientDetails.AdmissionDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["AdmissionDate"])));
                            nvo.PatientDetails.ISDischarged = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ISDischarged"]));
                        }
                        if (nvo.PatientDetails.IsLatestPatient)
                        {
                            nvo.PatientDetails.GeneralDetails.IPDAdmissionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IPDAdmissionID"]));
                            nvo.PatientDetails.GeneralDetails.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionUnitID"]));
                            nvo.PatientDetails.GeneralDetails.IPDAdmissionNo = Convert.ToString(DALHelper.HandleDBNull(reader["IPDNO"]));
                            nvo.PatientDetails.GeneralDetails.AdmissionDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["AdmissionDate"])));
                            nvo.PatientDetails.GeneralDetails.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                            nvo.PatientDetails.GeneralDetails.BedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToBedID"]));
                            nvo.PatientDetails.GeneralDetails.WardID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToWardID"]));
                            nvo.PatientDetails.GeneralDetails.ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassID"]));
                            nvo.PatientDetails.GeneralDetails.ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]));
                            nvo.PatientDetails.GeneralDetails.WardName = Convert.ToString(DALHelper.HandleDBNull(reader["WardName"]));
                            nvo.PatientDetails.GeneralDetails.BedName = Convert.ToString(DALHelper.HandleDBNull(reader["BedName"]));
                        }
                        else
                        {
                            nvo.PatientDetails.CreatedUnitId = (long) DALHelper.HandleDBNull(reader["CreatedUnitId"]);
                            nvo.PatientDetails.AddedBy = (long?) DALHelper.HandleDBNull(reader["AddedBy"]);
                            nvo.PatientDetails.AddedOn = (string) DALHelper.HandleDBNull(reader["AddedOn"]);
                            nvo.PatientDetails.AddedDateTime = DALHelper.HandleDate(reader["AddedDateTime"]);
                            nvo.PatientDetails.AddedWindowsLoginName = (string) DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]);
                            nvo.PatientDetails.UpdatedUnitId = (long?) DALHelper.HandleDBNull(reader["UpdatedUnitID"]);
                            nvo.PatientDetails.UpdatedBy = (long?) DALHelper.HandleDBNull(reader["UpdatedBy"]);
                            nvo.PatientDetails.UpdatedOn = (string) DALHelper.HandleDBNull(reader["UpdatedOn"]);
                            nvo.PatientDetails.UpdatedDateTime = DALHelper.HandleDate(reader["UpdatedDateTime"]);
                            nvo.PatientDetails.UpdatedWindowsLoginName = (string) DALHelper.HandleDBNull(reader["UpdatedWindowsLoginName"]);
                            nvo.PatientDetails.CompanyName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["CompanyName"])));
                        }
                        nvo.PatientDetails.GeneralDetails.PatientUnitID = (long) DALHelper.HandleDBNull(reader["UnitId"]);
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

        public override IValueObject GetPatientBillBalanceAmount(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientBillBalanceAmountBizActionVO nvo = (clsGetPatientBillBalanceAmountBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientBillBalanceAmount");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.String, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientGeneralDetails == null)
                    {
                        nvo.PatientGeneralDetails = new clsPatientGeneralVO();
                    }
                    while (reader.Read())
                    {
                        nvo.PatientGeneralDetails.PatientID = (long) DALHelper.HandleDBNull(reader["PatientID"]);
                        nvo.PatientGeneralDetails.PatientUnitID = (long) DALHelper.HandleDBNull(reader["PatientUnitID"]);
                        nvo.PatientGeneralDetails.BillBalanceAmountSelf = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmountSelf"]));
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetPatientCoupleList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCoupleGeneralDetailsListBizActionVO nvo = valueObject as clsGetCoupleGeneralDetailsListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GETDonorCoupleLinkedList");
                this.dbServer.AddInParameter(storedProcCommand, "DonorID", DbType.String, nvo.DonorID);
                this.dbServer.AddInParameter(storedProcCommand, "DonorUnitID", DbType.Int64, nvo.DonorUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientDetailsList == null)
                    {
                        nvo.PatientDetailsList = new List<clsPatientGeneralVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientGeneralVO item = new clsPatientGeneralVO {
                            PatientID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            PatientUnitID = (long) DALHelper.HandleDBNull(reader["UnitId"]),
                            MRNo = (string) DALHelper.HandleDBNull(reader["MRNo"]),
                            GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"])),
                            LastName = (string) DALHelper.HandleDBNull(reader["LastName"]),
                            FirstName = (string) DALHelper.HandleDBNull(reader["FirstName"]),
                            MiddleName = (string) DALHelper.HandleDBNull(reader["MiddleName"]),
                            Gender = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"])),
                            CompanyID = Convert.ToInt32(DALHelper.HandleDBNull(reader["CompanyID"])),
                            TariffID = (long) DALHelper.HandleDBNull(reader["TariffID"]),
                            PatientSourceID = (long) DALHelper.HandleDBNull(reader["PatientSourceID"])
                        };
                        nvo.PatientDetailsList.Add(item);
                    }
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetPatientDetailsForCounterSale(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientDetailsForCounterSaleBizActionVO nvo = (clsGetPatientDetailsForCounterSaleBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientDetailsForCounterSale");
                this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, nvo.MRNO);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientDetails == null)
                    {
                        nvo.PatientDetails = new clsPatientVO();
                    }
                    while (reader.Read())
                    {
                        clsPatientVO patientDetails = nvo.PatientDetails;
                        nvo.PatientDetails.GeneralDetails.PatientID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.PatientDetails.GeneralDetails.MRNo = (string) DALHelper.HandleDBNull(reader["MRNo"]);
                        DateTime? nullable = DALHelper.HandleDate(reader["RegistrationDate"]);
                        nvo.PatientDetails.GeneralDetails.RegistrationDate = new DateTime?(nullable.Value);
                        nvo.PatientDetails.GeneralDetails.LastName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["LastName"]));
                        nvo.PatientDetails.GeneralDetails.FirstName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["FirstName"]));
                        nvo.PatientDetails.GeneralDetails.MiddleName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["MiddleName"]));
                        nvo.PatientDetails.GeneralDetails.DateOfBirth = DALHelper.HandleDate(reader["DateofBirth"]);
                        nvo.PatientDetails.GenderID = (long) DALHelper.HandleDBNull(reader["GenderID"]);
                        nvo.PatientDetails.Doctor = (string) DALHelper.HandleDBNull(reader["ReferredDoctor"]);
                        nvo.PatientDetails.MobileCountryCode = Convert.ToString(DALHelper.HandleDBNull(reader["MobileCountryCode"]));
                        nvo.PatientDetails.ContactNo1 = (string) DALHelper.HandleDBNull(reader["ContactNo1"]);
                        nvo.PatientDetails.AddressLine3 = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine3"])));
                        nvo.PatientDetails.AddressLine1 = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine1"])));
                        nvo.PatientDetails.PatientSponsorCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSponsorCategoryID"]));
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetPatientDetailsForCRM(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientDetailsForCRMBizActionVO nvo = (clsGetPatientDetailsForCRMBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_SearchPatient");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "Age", DbType.Int32, nvo.Age);
                this.dbServer.AddInParameter(storedProcCommand, "AgeFilter", DbType.String, nvo.AgeFilter);
                if ((nvo.FirstName != null) && (nvo.FirstName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, Security.base64Encode(nvo.FirstName) + "%");
                }
                if ((nvo.MiddleName != null) && (nvo.MiddleName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, Security.base64Encode(nvo.MiddleName) + "%");
                }
                if ((nvo.LastName != null) && (nvo.LastName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, Security.base64Encode(nvo.LastName) + "%");
                }
                if ((nvo.MRNo != null) && (nvo.MRNo.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MrNo", DbType.String, nvo.MRNo + "%");
                }
                if ((nvo.OPDNo != null) && (nvo.OPDNo.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "OPDNo", DbType.String, nvo.OPDNo + "%");
                }
                long unitID = nvo.UnitID;
                if (nvo.UnitID != 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                }
                long departmentID = nvo.DepartmentID;
                if (nvo.DepartmentID != 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, nvo.DepartmentID);
                }
                long doctorID = nvo.DoctorID;
                if (nvo.DoctorID != 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorID);
                }
                if ((nvo.State != null) && (nvo.State.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "State", DbType.String, Security.base64Encode(nvo.State) + "%");
                }
                if ((nvo.City != null) && (nvo.City.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "City", DbType.String, Security.base64Encode(nvo.City) + "%");
                }
                if ((nvo.Area != null) && (nvo.Area.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Area", DbType.String, nvo.Area);
                }
                long genderID = nvo.GenderID;
                if (nvo.GenderID != 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, nvo.GenderID);
                }
                long maritalStatusID = nvo.MaritalStatusID;
                if (nvo.MaritalStatusID != 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MaritalStatusID", DbType.Int64, nvo.MaritalStatusID);
                }
                if ((nvo.ContactNo != null) && (nvo.ContactNo.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ContactNo", DbType.String, nvo.ContactNo + "%");
                }
                long loyaltyCardID = nvo.LoyaltyCardID;
                if (nvo.LoyaltyCardID != 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LoyaltyCardID", DbType.Int64, nvo.LoyaltyCardID);
                }
                long complaintID = nvo.ComplaintID;
                if (nvo.ComplaintID != 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ComplaintID", DbType.Int64, nvo.ComplaintID);
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientDetails == null)
                    {
                        nvo.PatientDetails = new List<clsPatientVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientVO item = new clsPatientVO {
                            GeneralDetails = { 
                                PatientID = (long) DALHelper.HandleDBNull(reader["ID"]),
                                PatientTypeID = (long) DALHelper.HandleDBNull(reader["PatientCategoryID"]),
                                MRNo = (string) DALHelper.HandleDBNull(reader["MRNo"]),
                                OPDNO = (string) DALHelper.HandleDBNull(reader["OPDNO"]),
                                LastName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["LastName"])),
                                FirstName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["FirstName"])),
                                MiddleName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["MiddleName"])),
                                DateOfBirth = DALHelper.HandleDate(reader["DateofBirth"])
                            }
                        };
                        DateTime? nullable = DALHelper.HandleDate(reader["RegistrationDate"]);
                        item.GeneralDetails.RegistrationDate = new DateTime?(nullable.Value);
                        item.GeneralDetails.ContactNO1 = (string) DALHelper.HandleDBNull(reader["ContactNO1"]);
                        item.GeneralDetails.Complaint = (string) DALHelper.HandleDBNull(reader["Complaint"]);
                        item.Email = Security.base64Decode((string) DALHelper.HandleDBNull(reader["Email"]));
                        nvo.PatientDetails.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetPatientDetailsForPathology(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientDetailsForPathologyBizActionVO nvo = (clsGetPatientDetailsForPathologyBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientDetailsForPathology");
                this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, nvo.MRNO);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientDetails == null)
                    {
                        nvo.PatientDetails = new clsPatientVO();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_GetPatientBillInformationForPathology");
                            this.dbServer.AddInParameter(command2, "VisitID", DbType.Int64, nvo.PatientDetails.GeneralDetails.VisitID);
                            this.dbServer.AddInParameter(command2, "PatientID", DbType.Int64, nvo.PatientDetails.GeneralDetails.PatientID);
                            this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, nvo.PatientDetails.GeneralDetails.UnitId);
                            reader = (DbDataReader) this.dbServer.ExecuteReader(command2);
                            if (reader.HasRows)
                            {
                                nvo.PatientBillDetailList = new List<clsChargeVO>();
                                while (reader.Read())
                                {
                                    clsChargeVO item = new clsChargeVO {
                                        ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                                        UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"])
                                    };
                                    DateTime? nullable2 = DALHelper.HandleDate(reader["Date"]);
                                    item.Date = new DateTime?(nullable2.Value);
                                    item.Opd_Ipd_External_Id = (long) DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"]);
                                    item.Opd_Ipd_External = (short) DALHelper.HandleDBNull(reader["Opd_Ipd_External"]);
                                    item.TariffServiceId = (long) DALHelper.HandleDBNull(reader["TariffServiceId"]);
                                    item.ServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceId"]));
                                    item.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                                    item.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                                    item.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
                                    item.ConcessionPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                                    item.Concession = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                                    item.StaffDiscountPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffDiscountPercent"]));
                                    item.StaffDiscountAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffDiscountAmount"]));
                                    item.StaffParentDiscountPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffParentDiscountPercent"]));
                                    item.StaffParentDiscountAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffParentDiscountAmount"]));
                                    item.ServiceTaxPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["ServiceTaxPercent"]));
                                    item.ServiceTaxAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ServiceTaxAmount"]));
                                    item.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"]));
                                    item.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                                    item.IsBilled = (bool) DALHelper.HandleDBNull(reader["IsBilled"]);
                                    item.Discount = (double) DALHelper.HandleDBNull(reader["Discount"]);
                                    item.StaffFree = (double) DALHelper.HandleDBNull(reader["StaffFree"]);
                                    item.IsCancelled = (bool) DALHelper.HandleDBNull(reader["IsCancelled"]);
                                    item.CancellationRemark = (string) DALHelper.HandleDBNull(reader["CancellationRemark"]);
                                    item.CancelledBy = (long?) DALHelper.HandleDBNull(reader["CancelledBy"]);
                                    item.CancelledDate = DALHelper.HandleDate(reader["CancelledDate"]);
                                    item.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                                    item.TariffServiceId = (long) DALHelper.HandleDBNull(reader["TariffServiceID"]);
                                    item.ServiceSpecilizationID = (long) DALHelper.HandleDBNull(reader["SpecializationId"]);
                                    item.RateEditable = (bool) DALHelper.HandleDBNull(reader["RateEditable"]);
                                    item.MaxRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["MaxRate"]));
                                    item.MinRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["MinRate"]));
                                    item.ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                                    item.CompRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["CompRate"]));
                                    item.DiscountPerc = Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountPerc"]));
                                    item.DiscountAmt = Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountAmt"]));
                                    item.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmt"]));
                                    item.AuthorizationNo = Convert.ToString(DALHelper.HandleDBNull(reader["AuthorizationNo"]));
                                    item.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                                    item.IsFreezed = (bool) DALHelper.HandleDBNull(reader["IsFreezed"]);
                                    nvo.PatientBillDetailList.Add(item);
                                }
                            }
                            reader.NextResult();
                            if (reader.HasRows)
                            {
                                nvo.PatientBillInfoDetail = new clsBillVO();
                                while (reader.Read())
                                {
                                    nvo.PatientBillInfoDetail.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                                    nvo.PatientBillInfoDetail.UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]);
                                    nvo.PatientBillInfoDetail.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                                    DateTime? nullable3 = DALHelper.HandleDate(reader["Date"]);
                                    nvo.PatientBillInfoDetail.Date = nullable3.Value;
                                    nvo.PatientBillInfoDetail.IsCancelled = (bool) DALHelper.HandleDBNull(reader["IsCancelled"]);
                                    nvo.PatientBillInfoDetail.BillNo = (string) DALHelper.HandleDBNull(reader["BillNo"]);
                                    nvo.PatientBillInfoDetail.MRNO = (string) DALHelper.HandleDBNull(reader["MRNO"]);
                                    nvo.PatientBillInfoDetail.BillType = (BillTypes) ((short) DALHelper.HandleDBNull(reader["BillType"]));
                                    nvo.PatientBillInfoDetail.Opd_Ipd_External = (short) DALHelper.HandleDBNull(reader["Opd_Ipd_External"]);
                                    nvo.PatientBillInfoDetail.Opd_Ipd_External_Id = (long) DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"]);
                                    nvo.PatientBillInfoDetail.Opd_Ipd_External_UnitId = (long) DALHelper.HandleDBNull(reader["Opd_Ipd_External_UnitId"]);
                                    nvo.PatientBillInfoDetail.TotalBillAmount = (double) DALHelper.HandleDBNull(reader["TotalBillAmount"]);
                                    nvo.PatientBillInfoDetail.TotalConcessionAmount = (double) DALHelper.HandleDBNull(reader["TotalConcessionAmount"]);
                                    nvo.PatientBillInfoDetail.NetBillAmount = (double) DALHelper.HandleDBNull(reader["NetBillAmount"]);
                                    nvo.PatientBillInfoDetail.Opd_Ipd_External_No = (string) DALHelper.HandleDBNull(reader["OPDNO"]);
                                    nvo.PatientBillInfoDetail.IsFreezed = (bool) DALHelper.HandleDBNull(reader["IsFreezed"]);
                                    nvo.PatientBillInfoDetail.VisitTypeID = (long) DALHelper.HandleDBNull(reader["VisitTypeID"]);
                                    nvo.PatientBillInfoDetail.PatientID = (long) DALHelper.HandleDBNull(reader["PatientID"]);
                                    nvo.PatientBillInfoDetail.FirstName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["FirstName"]));
                                    nvo.PatientBillInfoDetail.MiddleName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["MiddleName"]));
                                    nvo.PatientBillInfoDetail.LastName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["LastName"]));
                                    nvo.PatientBillInfoDetail.BalanceAmountSelf = (double) DALHelper.HandleDBNull(reader["BalanceAmountSelf"]);
                                    nvo.PatientBillInfoDetail.SelfAmount = (double) DALHelper.HandleDBNull(reader["SelfAmount"]);
                                    nvo.PatientBillInfoDetail.BillPaymentType = (BillPaymentTypes) ((short) DALHelper.HandleDBNull(reader["BillPaymentType"]));
                                    nvo.PatientBillInfoDetail.TotalRefund = (double) DALHelper.HandleDBNull(reader["RefundAmount"]);
                                }
                            }
                            break;
                        }
                        clsPatientVO patientDetails = nvo.PatientDetails;
                        nvo.PatientDetails.GeneralDetails.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.PatientDetails.GeneralDetails.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        DateTime? nullable = DALHelper.HandleDate(reader["RegistrationDate"]);
                        nvo.PatientDetails.GeneralDetails.RegistrationDate = new DateTime?(nullable.Value);
                        nvo.PatientDetails.GeneralDetails.LastName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])));
                        nvo.PatientDetails.GeneralDetails.FirstName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])));
                        nvo.PatientDetails.GeneralDetails.MiddleName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"])));
                        nvo.PatientDetails.GeneralDetails.DateOfBirth = DALHelper.HandleDate(reader["DateofBirth"]);
                        nvo.PatientDetails.GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]));
                        nvo.PatientDetails.MaritalStatusID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MaritalStatusID"]));
                        nvo.PatientDetails.PrefixId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PrefixId"]));
                        nvo.PatientDetails.Email = Convert.ToString(DALHelper.HandleDBNull(reader["Email"]));
                        nvo.PatientDetails.Doctor = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredDoctor"]));
                        nvo.PatientDetails.MobileCountryCode = Convert.ToString(DALHelper.HandleDBNull(reader["MobileCountryCode"]));
                        nvo.PatientDetails.ContactNo1 = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo1"]));
                        nvo.PatientDetails.AddressLine3 = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine3"])));
                        nvo.PatientDetails.PatientSponsorCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSponsorCategoryID"]));
                        nvo.PatientDetails.GeneralDetails.VisitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitID"]));
                        nvo.PatientDetails.GeneralDetails.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetPatientDietPlan(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientDietPlanBizActionVO nvo = valueObject as clsGetPatientDietPlanBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientDietPlan");
                this.dbServer.AddInParameter(storedProcCommand, "PlanID", DbType.Int64, nvo.PlanID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DietList == null)
                    {
                        nvo.DietList = new List<clsPatientDietPlanVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientDietPlanVO item = new clsPatientDietPlanVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            PatientID = (long) DALHelper.HandleDBNull(reader["PatientID"]),
                            PatientUnitID = (long) DALHelper.HandleDBNull(reader["PatientUnitID"]),
                            VisitID = (long) DALHelper.HandleDBNull(reader["VisitID"]),
                            VisitDoctorID = (long) DALHelper.HandleDBNull(reader["VisitDoctorID"]),
                            VisitDoctor = (string) DALHelper.HandleDBNull(reader["DoctorName"]),
                            Date = new DateTime?((DateTime) DALHelper.HandleDBNull(reader["Date"])),
                            PlanID = (long) DALHelper.HandleDBNull(reader["PlanID"]),
                            PlanName = (string) DALHelper.HandleDBNull(reader["PlanName"]),
                            GeneralInformation = (string) DALHelper.HandleDBNull(reader["GeneralInformation"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.DietList.Add(item);
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

        public override IValueObject GetPatientDietPlanDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientDietPlanDetailsBizActionVO nvo = valueObject as clsGetPatientDietPlanDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientDietPlanDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DietDetailsList == null)
                    {
                        nvo.DietDetailsList = new List<clsPatientDietPlanDetailVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientDietPlanDetailVO item = new clsPatientDietPlanDetailVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            DietPlanID = (long) DALHelper.HandleDBNull(reader["DietPlanID"]),
                            FoodItemID = (long) DALHelper.HandleDBNull(reader["FoodItemID"]),
                            FoodItem = (string) DALHelper.HandleDBNull(reader["FoodItem"]),
                            FoodItemCategoryID = (long) DALHelper.HandleDBNull(reader["FoodCategoryID"]),
                            FoodItemCategory = (string) DALHelper.HandleDBNull(reader["FoodCategory"]),
                            Timing = (string) DALHelper.HandleDBNull(reader["Timing"]),
                            FoodQty = (string) DALHelper.HandleDBNull(reader["FoodQty"]),
                            FoodUnit = (string) DALHelper.HandleDBNull(reader["FoodUnit"]),
                            FoodCal = (string) DALHelper.HandleDBNull(reader["FoodCal"]),
                            FoodCH = (string) DALHelper.HandleDBNull(reader["FoodCH"]),
                            FoodFat = (string) DALHelper.HandleDBNull(reader["FoodFat"]),
                            FoodExpectedCal = (string) DALHelper.HandleDBNull(reader["FoodExpectedCal"]),
                            FoodInstruction = (string) DALHelper.HandleDBNull(reader["FoodInstruction"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.DietDetailsList.Add(item);
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

        public override IValueObject GetPatientFamilyDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientFamilyDetailsBizActionVO nvo = (clsGetPatientFamilyDetailsBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetLoyaltyFamilyDetailsbyPatientID");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.String, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.FamilyDetails == null)
                    {
                        nvo.FamilyDetails = new List<clsPatientFamilyDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientFamilyDetailsVO item = new clsPatientFamilyDetailsVO {
                            PatientID = (long) DALHelper.HandleDBNull(reader["PatientID"]),
                            FirstName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["FirstName"])),
                            MiddleName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["MiddleName"])),
                            LastName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["LastName"]))
                        };
                        DateTime? nullable = DALHelper.HandleDate(reader["DOB"]);
                        item.DateOfBirth = new DateTime?(nullable.Value);
                        item.TariffID = (long) DALHelper.HandleDBNull(reader["TariffID"]);
                        item.RelationID = (long) DALHelper.HandleDBNull(reader["RelationID"]);
                        item.Tariff = (string) DALHelper.HandleDBNull(reader["Tariff"]);
                        item.Relation = (string) DALHelper.HandleDBNull(reader["Relation"]);
                        item.MemberRegistered = (bool) DALHelper.HandleDBNull(reader["MemberRegistered"]);
                        nvo.FamilyDetails.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetPatientForLoyaltyCardIssue(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientForLoyaltyCardIssueBizActionVO nvo = (clsGetPatientForLoyaltyCardIssueBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientForIssue");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "IssueDate", DbType.Boolean, nvo.IssuDate);
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                if ((nvo.MrNo != null) && (nvo.MrNo.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MrNo", DbType.String, nvo.MrNo + "%");
                }
                if ((nvo.OPDNo != null) && (nvo.OPDNo.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "OPDNo", DbType.String, nvo.OPDNo + "%");
                }
                if ((nvo.LoyaltyCardNo != null) && (nvo.LoyaltyCardNo.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LoyaltyCardNo", DbType.String, nvo.LoyaltyCardNo);
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                if ((nvo.FirstName != null) && (nvo.FirstName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, Security.base64Encode(nvo.FirstName) + "%");
                }
                if ((nvo.MiddleName != null) && (nvo.MiddleName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, Security.base64Encode(nvo.MiddleName) + "%");
                }
                if ((nvo.LastName != null) && (nvo.LastName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, Security.base64Encode(nvo.LastName) + "%");
                }
                this.dbServer.AddInParameter(storedProcCommand, "IsLoyaltymember", DbType.String, nvo.IsLoyaltymember);
                if (nvo.IsLoyaltymember)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LoyaltyProgramId", DbType.Int64, nvo.LoyaltyProgramID);
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientDetails == null)
                    {
                        nvo.PatientDetails = new List<clsPatientGeneralVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientGeneralVO item = new clsPatientGeneralVO {
                            PatientID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            PatientTypeID = (long) DALHelper.HandleDBNull(reader["PatientCategoryID"]),
                            MRNo = (string) DALHelper.HandleDBNull(reader["MRNo"]),
                            OPDNO = (string) DALHelper.HandleDBNull(reader["OPDNO"]),
                            LastName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["LastName"])),
                            FirstName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["FirstName"])),
                            MiddleName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["MiddleName"])),
                            DateOfBirth = DALHelper.HandleDate(reader["DateofBirth"])
                        };
                        DateTime? nullable = DALHelper.HandleDate(reader["RegistrationDate"]);
                        item.RegistrationDate = new DateTime?(nullable.Value);
                        item.UnitId = (long) DALHelper.HandleDBNull(reader["UnitID"]);
                        nvo.PatientDetails.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetPatientGeneralDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientGeneralDetailsListBizActionVO nvo = valueObject as clsGetPatientGeneralDetailsListBizActionVO;
            try
            {
                if (nvo.IsFromFindPatient)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientGeneralDetailsListForSearch_new");
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "SearchUnitID", DbType.Int64, nvo.SearchUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "IsVisitPatient", DbType.Boolean, nvo.VisitWise);
                    this.dbServer.AddInParameter(storedProcCommand, "VisitFromDate", DbType.DateTime, nvo.VisitFromDate);
                    if (nvo.VisitFromDate != null)
                    {
                        nvo.VisitToDate = new DateTime?(nvo.VisitToDate.Value.Date.AddDays(1.0));
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "VisitToDate", DbType.DateTime, nvo.VisitToDate);
                    this.dbServer.AddInParameter(storedProcCommand, "RegistFromDate", DbType.DateTime, nvo.FromDate);
                    if (nvo.ToDate != null)
                    {
                        nvo.ToDate = new DateTime?(nvo.ToDate.Value.Date.AddDays(1.0));
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "RegistToDate", DbType.DateTime, nvo.ToDate);
                    this.dbServer.AddInParameter(storedProcCommand, "Firstname", DbType.String, nvo.FirstName);
                    this.dbServer.AddInParameter(storedProcCommand, "FamilyName", DbType.String, nvo.FamilyName);
                    this.dbServer.AddInParameter(storedProcCommand, "Lastname", DbType.String, nvo.LastName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddressLine", DbType.String, nvo.AddressLine1);
                    this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, nvo.MRNo);
                    this.dbServer.AddInParameter(storedProcCommand, "ReferenceNo", DbType.String, nvo.ReferenceNo);
                    this.dbServer.AddInParameter(storedProcCommand, "ContactNo", DbType.String, nvo.ContactNo);
                    this.dbServer.AddInParameter(storedProcCommand, "DOBFromDate", DbType.String, nvo.DOBFromDate);
                    this.dbServer.AddInParameter(storedProcCommand, "DOBToDate", DbType.String, nvo.DOBToDate);
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.PatientDetailsList == null)
                        {
                            nvo.PatientDetailsList = new List<clsPatientGeneralVO>();
                        }
                        while (reader.Read())
                        {
                            clsPatientGeneralVO item = new clsPatientGeneralVO {
                                PatientID = (long) DALHelper.HandleDBNull(reader["ID"]),
                                PatientUnitID = (long) DALHelper.HandleDBNull(reader["UnitId"]),
                                UnitId = (long) DALHelper.HandleDBNull(reader["UnitId"]),
                                MRNo = (string) DALHelper.HandleDBNull(reader["MRNo"]),
                                LastName = (string) DALHelper.HandleDBNull(reader["LastName"]),
                                FirstName = (string) DALHelper.HandleDBNull(reader["FirstName"]),
                                AddressLine1 = (string) DALHelper.HandleDBNull(reader["AddressLine1"]),
                                MiddleName = (string) DALHelper.HandleDBNull(reader["MiddleName"]),
                                DateOfBirth = DALHelper.HandleDate(reader["DateofBirth"]),
                                Age = Convert.ToInt32(DALHelper.HandleDBNull(reader["Age"]))
                            };
                            DateTime? nullable5 = DALHelper.HandleDate(reader["RegistrationDate"]);
                            item.RegistrationDate = new DateTime?(nullable5.Value);
                            if (!nvo.VisitWise && !nvo.RegistrationWise)
                            {
                                item.IPDAdmissionID = (long) DALHelper.HandleDBNull(reader["VAID"]);
                                item.IPDAdmissionNo = (string) DALHelper.HandleDBNull(reader["OINO"]);
                            }
                            else
                            {
                                item.VisitID = (long) DALHelper.HandleDBNull(reader["VAID"]);
                                item.VisitUnitID = (long) DALHelper.HandleDBNull(reader["VisitUnitID"]);
                                item.VisitUnitId = (long) DALHelper.HandleDBNull(reader["VisitUnitID"]);
                                item.OPDNO = (string) DALHelper.HandleDBNull(reader["OINO"]);
                                item.Unit = Convert.ToString(reader["UnitName"]);
                            }
                            if (nvo.VisitWise)
                            {
                                item.PatientKind = PatientsKind.OPD;
                            }
                            else if (nvo.AdmissionWise)
                            {
                                item.PatientKind = PatientsKind.IPD;
                            }
                            else if ((item.VisitID == 0L) && (item.IPDAdmissionID == 0L))
                            {
                                item.PatientKind = PatientsKind.Registration;
                            }
                            item.TariffID = (long) DALHelper.HandleDBNull(reader["TariffID"]);
                            item.PatientSourceID = (long) DALHelper.HandleDBNull(reader["PatientSourceID"]);
                            item.CompanyID = (long) DALHelper.HandleDBNull(reader["CompanyID"]);
                            item.ContactNO1 = (string) DALHelper.HandleDBNull(reader["ContactNO1"]);
                            item.Gender = (string) DALHelper.HandleDBNull(reader["Gender"]);
                            item.MaritalStatus = (string) DALHelper.HandleDBNull(reader["MaritalStatus"]);
                            item.UniversalID = Security.base64Decode((string) DALHelper.HandleDBNull(reader["CivilID"]));
                            item.PatientTypeID = (long) DALHelper.HandleDBNull(reader["PatientCategoryID"]);
                            item.MaritalStatus = (string) DALHelper.HandleDBNull(reader["MaritalStatus"]);
                            item.ReferenceNo = (string) DALHelper.HandleDBNull(reader["ReferenceNo"]);
                            item.LinkServer = nvo.LinkServer;
                            if (!nvo.ISFromQueeManagment && !nvo.isfromMaterialConsumpation)
                            {
                                item.BabyWeight = Convert.ToString(DALHelper.HandleDBNull(reader["BabyWeight"]));
                            }
                            nvo.PatientDetailsList.Add(item);
                        }
                    }
                    reader.NextResult();
                    nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                    reader.Close();
                }
                else if (nvo.isfromCouterSaleStaff)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStaffGeneralDetailsListForSearch");
                    long empNO = nvo.EmpNO;
                    this.dbServer.AddInParameter(storedProcCommand, "EmpNO", DbType.String, nvo.EmpNO);
                    if ((nvo.FirstName != null) && (nvo.FirstName.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, nvo.FirstName + "%");
                    }
                    if ((nvo.LastName != null) && (nvo.LastName.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, nvo.LastName + "%");
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                    DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader2.HasRows)
                    {
                        if (nvo.PatientDetailsList == null)
                        {
                            nvo.PatientDetailsList = new List<clsPatientGeneralVO>();
                        }
                        while (true)
                        {
                            if (!reader2.Read())
                            {
                                reader2.NextResult();
                                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                                reader2.Close();
                                break;
                            }
                            clsPatientGeneralVO item = new clsPatientGeneralVO {
                                PatientID = (long) DALHelper.HandleDBNull(reader2["ID"]),
                                PatientUnitID = (long) DALHelper.HandleDBNull(reader2["UnitId"]),
                                UnitId = (long) DALHelper.HandleDBNull(reader2["UnitId"]),
                                MRNo = (string) DALHelper.HandleDBNull(reader2["MRNo"]),
                                LastName = (string) DALHelper.HandleDBNull(reader2["LastName"]),
                                FirstName = (string) DALHelper.HandleDBNull(reader2["FirstName"]),
                                DateOfBirth = DALHelper.HandleDate(reader2["DateofBirth"]),
                                Gender = (string) DALHelper.HandleDBNull(reader2["Gender"]),
                                GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["GenderID"])),
                                Designation = (string) DALHelper.HandleDBNull(reader2["Designation"]),
                                DepartmentName = (string) DALHelper.HandleDBNull(reader2["DepartmentName"])
                            };
                            nvo.PatientDetailsList.Add(item);
                        }
                    }
                }
                else
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientGeneralDetailsListForSearch");
                    this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                    if ((nvo.MRNo != null) && (nvo.MRNo.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "MrNo", DbType.String, "%" + nvo.MRNo + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "MrNo", DbType.String, nvo.MRNo + "%");
                    }
                    if ((nvo.FirstName != null) && (nvo.FirstName.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, nvo.FirstName + "%");
                    }
                    if ((nvo.AddressLine1 != null) && (nvo.AddressLine1.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "AddressLine1", DbType.String, Security.base64Encode(nvo.AddressLine1) + "%");
                    }
                    if ((nvo.MiddleName != null) && (nvo.MiddleName.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, nvo.MiddleName + "%");
                    }
                    if ((nvo.LastName != null) && (nvo.LastName.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, nvo.LastName + "%");
                    }
                    if ((nvo.FamilyName != null) && ((nvo.FamilyName.Length != 0) && ((nvo.FamilyName != null) && (nvo.FamilyName.Length != 0))))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "FamilyName", DbType.String, nvo.FamilyName + "%");
                    }
                    if ((nvo.OPDNo != null) && (nvo.OPDNo.Length != 0))
                    {
                        if (nvo.VisitWise)
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "OPDNo", DbType.String, nvo.OPDNo + "%");
                        }
                        else if (nvo.AdmissionWise)
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "AdmissionNo", DbType.String, nvo.OPDNo + "%");
                        }
                        else if (nvo.isfromMaterialConsumpation)
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "OPDNo", DbType.String, nvo.OPDNo + "%");
                        }
                    }
                    if ((nvo.ContactNo != null) && (nvo.ContactNo.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "ContactNo", DbType.String, nvo.ContactNo + "%");
                    }
                    if ((nvo.DonarCode != null) && (nvo.DonarCode.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "DonarCode", DbType.String, "%" + nvo.DonarCode + "%");
                    }
                    if ((nvo.OldRegistrationNo != null) && (nvo.OldRegistrationNo.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "OldRegistrationNo", DbType.String, nvo.OldRegistrationNo + "%");
                    }
                    if ((nvo.Country != null) && (nvo.Country.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "Country", DbType.String, Security.base64Encode(nvo.Country) + "%");
                    }
                    if ((nvo.State != null) && (nvo.State.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "State", DbType.String, Security.base64Encode(nvo.State) + "%");
                    }
                    if ((nvo.City != null) && (nvo.City.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "City", DbType.String, Security.base64Encode(nvo.City) + "%");
                    }
                    if ((nvo.Pincode != null) && (nvo.Pincode.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "Pincode", DbType.String, Security.base64Encode(nvo.Pincode) + "%");
                    }
                    if ((nvo.CivilID != null) && (nvo.CivilID.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "CivilID", DbType.String, Security.base64Encode(nvo.CivilID) + "%");
                    }
                    if ((nvo.ReferenceNo != null) && (nvo.ReferenceNo.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "ReferenceNo", DbType.String, nvo.ReferenceNo);
                    }
                    long genderID = nvo.GenderID;
                    if (nvo.GenderID != 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, nvo.GenderID);
                    }
                    long patientCategoryID = nvo.PatientCategoryID;
                    if (nvo.PatientCategoryID != 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "PatientCategory", DbType.Int64, nvo.PatientCategoryID);
                    }
                    if (nvo.DOB != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "DOB", DbType.DateTime, nvo.DOB);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "DOB", DbType.DateTime, null);
                    }
                    if (nvo.FromDate != null)
                    {
                        nvo.FromDate = new DateTime?(nvo.FromDate.Value.Date);
                        this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                    }
                    if (nvo.ToDate != null)
                    {
                        if (nvo.FromDate != null)
                        {
                            nvo.ToDate = new DateTime?(nvo.ToDate.Value.Date.AddDays(1.0));
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                    }
                    if (nvo.VisitWise)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "Serchfor", DbType.Int16, (short) 1);
                        if (nvo.VisitFromDate != null)
                        {
                            nvo.VisitFromDate = new DateTime?(nvo.VisitFromDate.Value.Date);
                            this.dbServer.AddInParameter(storedProcCommand, "VisitFromDate", DbType.DateTime, nvo.VisitFromDate);
                        }
                        if (nvo.VisitToDate != null)
                        {
                            if (nvo.VisitFromDate != null)
                            {
                                nvo.VisitToDate = new DateTime?(nvo.VisitToDate.Value.Date.AddDays(1.0));
                            }
                            this.dbServer.AddInParameter(storedProcCommand, "VisitToDate", DbType.DateTime, nvo.VisitToDate);
                        }
                    }
                    if (nvo.AdmissionWise)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "Serchfor", DbType.Int16, (short) 2);
                        if (nvo.AdmissionFromDate != null)
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "AdmissionFromDate", DbType.DateTime, nvo.AdmissionFromDate);
                        }
                        if (nvo.AdmissionToDate != null)
                        {
                            if (nvo.AdmissionToDate != null)
                            {
                                nvo.AdmissionToDate = new DateTime?(nvo.AdmissionToDate.Value.Date.AddDays(1.0));
                            }
                            this.dbServer.AddInParameter(storedProcCommand, "AdmissionToDate", DbType.DateTime, nvo.AdmissionToDate);
                        }
                    }
                    if (nvo.DOBWise)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "Serchfor", DbType.Int16, (short) 0);
                        if (nvo.DOBFromDate != null)
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "DOBFromDate", DbType.DateTime, nvo.DOBFromDate);
                        }
                        if (nvo.DOBToDate != null)
                        {
                            if (nvo.DOBToDate != null)
                            {
                                nvo.DOBToDate = new DateTime?(nvo.DOBToDate.Value.Date.AddDays(1.0));
                            }
                            this.dbServer.AddInParameter(storedProcCommand, "DOBToDate", DbType.DateTime, nvo.DOBToDate);
                        }
                    }
                    if (nvo.ISDonorSerch)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "DonorSerch", DbType.Boolean, nvo.ISDonorSerch);
                    }
                    if (nvo.IsLoyaltyMember)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "IsLoyaltyMember", DbType.Boolean, nvo.IsLoyaltyMember);
                        this.dbServer.AddInParameter(storedProcCommand, "LoyaltyProgramID", DbType.Int64, nvo.LoyaltyProgramID);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "QueueUnitID", DbType.Int64, nvo.PQueueUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "RegistrationTypeID", DbType.Int64, nvo.RegistrationTypeID);
                    long pQueueUnitID = nvo.PQueueUnitID;
                    if (nvo.PQueueUnitID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "FindPatientVisitUnitID", DbType.Int64, nvo.PQueueUnitID);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "FindPatientVisitUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    }
                    if (nvo.SearchInAnotherClinic)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "SearchInAnotherClinic", DbType.Boolean, nvo.SearchInAnotherClinic);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "IdentityID", DbType.Int64, nvo.IdentityID);
                    this.dbServer.AddInParameter(storedProcCommand, "IdentityNumber", DbType.String, nvo.IdentityNumber);
                    this.dbServer.AddInParameter(storedProcCommand, "SpecialRegID", DbType.Int64, nvo.SpecialRegID);
                    this.dbServer.AddInParameter(storedProcCommand, "IsFromQueeManagment", DbType.Boolean, nvo.ISFromQueeManagment);
                    this.dbServer.AddInParameter(storedProcCommand, "ShowOutSourceDonor", DbType.Boolean, nvo.ShowOutSourceDonor);
                    this.dbServer.AddInParameter(storedProcCommand, "IsFromSurrogacyModule", DbType.Boolean, nvo.IsFromSurrogacyModule);
                    this.dbServer.AddInParameter(storedProcCommand, "IsSelfAndDonor", DbType.Int32, nvo.IsSelfAndDonor);
                    this.dbServer.AddInParameter(storedProcCommand, "isfromMaterialConsumpation", DbType.Boolean, nvo.isfromMaterialConsumpation);
                    this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.StoreID);
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, nvo.LinkServer);
                    if (nvo.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, nvo.LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                    DbDataReader reader3 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader3.HasRows)
                    {
                        if (nvo.PatientDetailsList == null)
                        {
                            nvo.PatientDetailsList = new List<clsPatientGeneralVO>();
                        }
                        while (reader3.Read())
                        {
                            clsPatientGeneralVO item = new clsPatientGeneralVO {
                                PatientID = (long) DALHelper.HandleDBNull(reader3["ID"]),
                                PatientUnitID = (long) DALHelper.HandleDBNull(reader3["UnitId"]),
                                UnitId = (long) DALHelper.HandleDBNull(reader3["UnitId"]),
                                MRNo = (string) DALHelper.HandleDBNull(reader3["MRNo"])
                            };
                            if (nvo.VisitWise)
                            {
                                item.PatientID = (long) DALHelper.HandleDBNull(reader3["PatientID"]);
                                item.PatientUnitID = (long) DALHelper.HandleDBNull(reader3["PatientUnitID"]);
                                item.UnitId = (long) DALHelper.HandleDBNull(reader3["UnitId"]);
                            }
                            if (nvo.isfromMaterialConsumpation)
                            {
                                item.PatientID = (long) DALHelper.HandleDBNull(reader3["PatientId"]);
                                item.PatientUnitID = (long) DALHelper.HandleDBNull(reader3["PatientUnitID"]);
                                item.UnitId = (long) DALHelper.HandleDBNull(reader3["UnitId"]);
                                item.Opd_Ipd_External_Id = (long) DALHelper.HandleDBNull(reader3["Opd_Ipd_External_Id"]);
                                item.Opd_Ipd_External_UnitId = (long) DALHelper.HandleDBNull(reader3["Opd_Ipd_External_UnitId"]);
                                item.Opd_Ipd_External = Convert.ToInt32(DALHelper.HandleDBNull(reader3["Opd_Ipd_External"]));
                                item.OPDNO = Convert.ToString(DALHelper.HandleDBNull(reader3["OPD_IPD_NO"]));
                                item.IsPatientIndentReceiveExists = Convert.ToInt32(DALHelper.HandleDBNull(reader3["IsPatientIndentReceiveExists"]));
                            }
                            item.LastName = (string) DALHelper.HandleDBNull(reader3["LastName"]);
                            item.FirstName = (string) DALHelper.HandleDBNull(reader3["FirstName"]);
                            item.MiddleName = (string) DALHelper.HandleDBNull(reader3["MiddleName"]);
                            item.AddressLine1 = (string) DALHelper.HandleDBNull(reader3["AddressLine1"]);
                            item.RegType = Convert.ToInt16(DALHelper.HandleDBNull(reader3["RegType"]));
                            item.DateOfBirth = DALHelper.HandleDate(reader3["DateofBirth"]);
                            item.Age = Convert.ToInt32(DALHelper.HandleDBNull(reader3["Age"]));
                            item.RegistrationDate = new DateTime?(DALHelper.HandleDate(reader3["RegistrationDate"]).Value);
                            if (nvo.VisitWise)
                            {
                                item.VisitID = (long) DALHelper.HandleDBNull(reader3["VAID"]);
                                item.VisitUnitID = (long) DALHelper.HandleDBNull(reader3["VisitUnitID"]);
                                item.VisitUnitId = (long) DALHelper.HandleDBNull(reader3["VisitUnitID"]);
                                item.OPDNO = (string) DALHelper.HandleDBNull(reader3["OINO"]);
                                item.Unit = Convert.ToString(reader3["UnitName"]);
                            }
                            if (nvo.VisitWise || nvo.RegistrationWise)
                            {
                                item.FemaleName = (string) DALHelper.HandleDBNull(reader3["FemaleName"]);
                                item.MaleName = (string) DALHelper.HandleDBNull(reader3["MaleName"]);
                                item.FemaleAge = Convert.ToInt32(DALHelper.HandleDBNull(reader3["FemaleAge"]));
                                item.MaleAge = Convert.ToInt32(DALHelper.HandleDBNull(reader3["MaleAge"]));
                            }
                            if (nvo.VisitWise)
                            {
                                item.PatientKind = PatientsKind.OPD;
                            }
                            else if (nvo.AdmissionWise)
                            {
                                item.PatientKind = PatientsKind.IPD;
                            }
                            else if ((item.VisitID == 0L) && (item.IPDAdmissionID == 0L))
                            {
                                item.PatientKind = PatientsKind.Registration;
                            }
                            item.TariffID = (long) DALHelper.HandleDBNull(reader3["TariffID"]);
                            item.PatientSourceID = (long) DALHelper.HandleDBNull(reader3["PatientSourceID"]);
                            item.CompanyID = (long) DALHelper.HandleDBNull(reader3["CompanyID"]);
                            item.ContactNO1 = (string) DALHelper.HandleDBNull(reader3["ContactNO1"]);
                            item.Gender = (string) DALHelper.HandleDBNull(reader3["Gender"]);
                            item.MaritalStatus = (string) DALHelper.HandleDBNull(reader3["MaritalStatus"]);
                            item.UniversalID = Security.base64Decode((string) DALHelper.HandleDBNull(reader3["CivilID"]));
                            item.PatientTypeID = (long) DALHelper.HandleDBNull(reader3["PatientCategoryID"]);
                            item.RelationID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["RelationID"]));
                            item.GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["GenderID"]));
                            item.NewPatientCategoryID = (long) DALHelper.HandleDBNull(reader3["NewPatientCategoryID"]);
                            item.ReferenceNo = (string) DALHelper.HandleDBNull(reader3["ReferenceNo"]);
                            item.City = (string) DALHelper.HandleDBNull(reader3["CityNew"]);
                            item.DonorCode = Convert.ToString(DALHelper.HandleDBNull(reader3["DonorCode"]));
                            item.IsSurrogateAlreadyLinked = Convert.ToBoolean(DALHelper.HandleDBNull(reader3["IsSurrogateAlreadyLinked"]));
                            item.DonarCode = Convert.ToString(DALHelper.HandleDBNull(reader3["DonarCode"]));
                            item.OldRegistrationNo = Convert.ToString(DALHelper.HandleDBNull(reader3["OldRegistrationNo"]));
                            item.IsDonor = Convert.ToBoolean(DALHelper.HandleDBNull(reader3["IsDonor"]));
                            item.IsReferralDoc = Convert.ToBoolean(DALHelper.HandleDBNull(reader3["IsReferralDoc"]));
                            item.ReferralTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["ReferralTypeID"]));
                            item.ReferralDoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["ReferralDoctorID"]));
                            item.IsAge = (bool) DALHelper.HandleBoolDBNull(reader3["IsAge"]);
                            if (item.IsAge)
                            {
                                item.DateOfBirthFromAge = DALHelper.HandleDate(reader3["DateOfBirth"]);
                            }
                            else
                            {
                                item.DateOfBirth = DALHelper.HandleDate(reader3["DateOfBirth"]);
                            }
                            item.EducationID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader3["EducationID"]));
                            item.BloodGroupID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader3["BloodGroupID"]));
                            string str = Convert.ToString(DALHelper.HandleDBNull(reader3["ImagePath"]));
                            if (!string.IsNullOrEmpty(str))
                            {
                                string[] strArray = new string[] { "https://", this.ImgIP, "/", this.ImgVirtualDir, "/", str };
                                item.ImageName = string.Concat(strArray);
                            }
                            item.IsDocAttached = Convert.ToBoolean(DALHelper.HandleDBNull(reader3["IsDocAttached"]));
                            item.Email = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader3["Email"])));
                            item.IdentityType = Convert.ToString(DALHelper.HandleDBNull(reader3["IdentityType"]));
                            item.IdentityNumber = Convert.ToString(DALHelper.HandleDBNull(reader3["IdentityNumber"]));
                            item.RemarkForPatientType = Convert.ToString(DALHelper.HandleDBNull(reader3["RemarkForPatientType"]));
                            item.SpecialRegName = Convert.ToString(DALHelper.HandleDBNull(reader3["SpecialReg"]));
                            if (item.RegType == 0)
                            {
                                item.RegistrationType = "OPD";
                            }
                            else if (item.RegType == 1)
                            {
                                item.RegistrationType = "IPD";
                            }
                            else if (item.RegType == 2)
                            {
                                item.RegistrationType = "Pharmacy";
                            }
                            else if (item.RegType == 5)
                            {
                                item.RegistrationType = "Pathology";
                            }
                            if (!nvo.isfromMaterialConsumpation)
                            {
                                item.PanNumber = Convert.ToString(DALHelper.HandleDBNull(reader3["PanNumber"]));
                                item.Email = Convert.ToString(DALHelper.HandleDBNull(reader3["Email"]));
                                item.NationalityID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["NationalityId"]));
                                item.IsSpouse = Convert.ToBoolean(DALHelper.HandleDBNull(reader3["IsSpouse"]));
                                item.IsDischarged = Convert.ToBoolean(DALHelper.HandleDBNull(reader3["IsDischarged"]));
                            }
                            if (nvo.ISFromQueeManagment)
                            {
                                item.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader3["ReferralDoctorName"]));
                                item.Camp = Convert.ToString(DALHelper.HandleDBNull(reader3["Camp"]));
                                item.SourceofReference = Convert.ToString(DALHelper.HandleDBNull(reader3["SourceofReference"]));
                            }
                            item.LinkServer = nvo.LinkServer;
                            if (!nvo.ISFromQueeManagment && !nvo.isfromMaterialConsumpation)
                            {
                                item.BabyWeight = Convert.ToString(DALHelper.HandleDBNull(reader3["BabyWeight"]));
                            }
                            nvo.PatientDetailsList.Add(item);
                        }
                    }
                    reader3.NextResult();
                    nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                    reader3.Close();
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject GetPatientGeneralDetailsListForSurrogacy(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientGeneralDetailsListForSurrogacyBizActionVO nvo = valueObject as clsGetPatientGeneralDetailsListForSurrogacyBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientGeneralDetailsListForSurrogacySearch");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "MrNo", DbType.String, nvo.MRNo + "%");
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, nvo.FirstName + "%");
                if ((nvo.MiddleName != null) && (nvo.MiddleName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, nvo.MiddleName + "%");
                }
                if ((nvo.LastName != null) && (nvo.LastName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, nvo.LastName + "%");
                }
                if ((nvo.FamilyName != null) && ((nvo.FamilyName.Length != 0) && ((nvo.FamilyName != null) && (nvo.FamilyName.Length != 0))))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FamilyName", DbType.String, Security.base64Encode(nvo.FamilyName) + "%");
                }
                if ((nvo.OPDNo != null) && (nvo.OPDNo.Length != 0))
                {
                    if (nvo.VisitWise)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "OPDNo", DbType.String, nvo.OPDNo + "%");
                    }
                    else if (nvo.AdmissionWise)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "AdmissionNo", DbType.String, nvo.OPDNo + "%");
                    }
                }
                if ((nvo.ContactNo != null) && (nvo.ContactNo.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ContactNo", DbType.String, nvo.ContactNo + "%");
                }
                if ((nvo.Country != null) && (nvo.Country.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Country", DbType.String, Security.base64Encode(nvo.Country) + "%");
                }
                if ((nvo.State != null) && (nvo.State.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "State", DbType.String, Security.base64Encode(nvo.State) + "%");
                }
                if ((nvo.City != null) && (nvo.City.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "City", DbType.String, Security.base64Encode(nvo.City) + "%");
                }
                if ((nvo.Pincode != null) && (nvo.Pincode.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Pincode", DbType.String, Security.base64Encode(nvo.Pincode) + "%");
                }
                if ((nvo.CivilID != null) && (nvo.CivilID.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CivilID", DbType.String, Security.base64Encode(nvo.CivilID) + "%");
                }
                long genderID = nvo.GenderID;
                if (nvo.GenderID != 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, nvo.GenderID);
                }
                long patientCategoryID = nvo.PatientCategoryID;
                if (nvo.PatientCategoryID != 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "PatientCategory", DbType.Int64, nvo.PatientCategoryID);
                }
                if (nvo.FromDate != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                }
                if (nvo.ToDate != null)
                {
                    if (nvo.FromDate != null)
                    {
                        nvo.ToDate = new DateTime?(nvo.ToDate.Value.Date.AddDays(1.0));
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                }
                if (nvo.VisitWise)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Serchfor", DbType.Int16, (short) 1);
                    if (nvo.VisitFromDate != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "VisitFromDate", DbType.DateTime, nvo.VisitFromDate);
                    }
                    if (nvo.VisitToDate != null)
                    {
                        if (nvo.VisitFromDate != null)
                        {
                            nvo.VisitToDate = new DateTime?(nvo.VisitToDate.Value.Date.AddDays(1.0));
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "VisitToDate", DbType.DateTime, nvo.VisitToDate);
                    }
                }
                if (nvo.AdmissionWise)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Serchfor", DbType.Int16, (short) 2);
                    if (nvo.AdmissionFromDate != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "AdmissionFromDate", DbType.DateTime, nvo.AdmissionFromDate);
                    }
                    if (nvo.AdmissionToDate != null)
                    {
                        if (nvo.AdmissionToDate != null)
                        {
                            nvo.AdmissionToDate = new DateTime?(nvo.AdmissionToDate.Value.Date.AddDays(1.0));
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "AdmissionToDate", DbType.DateTime, nvo.AdmissionToDate);
                    }
                }
                if (nvo.DOBWise)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Serchfor", DbType.Int16, (short) 0);
                    if (nvo.DOBFromDate != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "DOBFromDate", DbType.DateTime, nvo.DOBFromDate);
                    }
                    if (nvo.DOBToDate != null)
                    {
                        if (nvo.DOBToDate != null)
                        {
                            nvo.DOBToDate = new DateTime?(nvo.DOBToDate.Value.Date.AddDays(1.0));
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "DOBToDate", DbType.DateTime, nvo.DOBToDate);
                    }
                }
                if (nvo.IsLoyaltyMember)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IsLoyaltyMember", DbType.Boolean, nvo.IsLoyaltyMember);
                    this.dbServer.AddInParameter(storedProcCommand, "LoyaltyProgramID", DbType.Int64, nvo.LoyaltyProgramID);
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "RegistrationTypeID", DbType.Int64, nvo.RegistrationTypeID);
                if (nvo.SearchInAnotherClinic)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "SearchInAnotherClinic", DbType.Boolean, nvo.SearchInAnotherClinic);
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, nvo.LinkServer);
                if (nvo.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, nvo.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "IsFromSurrogacyModule", DbType.Boolean, nvo.IsFromSurrogacyModule);
                this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, nvo.AgencyID);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientDetailsList == null)
                    {
                        nvo.PatientDetailsList = new List<clsPatientGeneralVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientGeneralVO item = new clsPatientGeneralVO {
                            PatientID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitId = (long) DALHelper.HandleDBNull(reader["UnitId"]),
                            MRNo = (string) DALHelper.HandleDBNull(reader["MRNo"]),
                            LastName = (string) DALHelper.HandleDBNull(reader["LastName"]),
                            FirstName = (string) DALHelper.HandleDBNull(reader["FirstName"]),
                            MiddleName = (string) DALHelper.HandleDBNull(reader["MiddleName"]),
                            DateOfBirth = DALHelper.HandleDate(reader["DateofBirth"])
                        };
                        DateTime? nullable17 = DALHelper.HandleDate(reader["RegistrationDate"]);
                        item.RegistrationDate = new DateTime?(nullable17.Value);
                        if (!nvo.VisitWise && !nvo.RegistrationWise)
                        {
                            item.IPDAdmissionID = (long) DALHelper.HandleDBNull(reader["VAID"]);
                            item.IPDAdmissionNo = (string) DALHelper.HandleDBNull(reader["OINO"]);
                        }
                        else
                        {
                            item.VisitID = (long) DALHelper.HandleDBNull(reader["VAID"]);
                            item.OPDNO = (string) DALHelper.HandleDBNull(reader["OINO"]);
                        }
                        if (nvo.VisitWise)
                        {
                            item.PatientKind = PatientsKind.OPD;
                        }
                        else if (nvo.AdmissionWise)
                        {
                            item.PatientKind = PatientsKind.IPD;
                        }
                        else if ((item.VisitID == 0L) && (item.IPDAdmissionID == 0L))
                        {
                            item.PatientKind = PatientsKind.Registration;
                        }
                        item.TariffID = (long) DALHelper.HandleDBNull(reader["TariffID"]);
                        item.PatientSourceID = (long) DALHelper.HandleDBNull(reader["PatientSourceID"]);
                        item.CompanyID = (long) DALHelper.HandleDBNull(reader["CompanyID"]);
                        item.ContactNO1 = (string) DALHelper.HandleDBNull(reader["ContactNO1"]);
                        item.Gender = (string) DALHelper.HandleDBNull(reader["Gender"]);
                        item.MaritalStatus = (string) DALHelper.HandleDBNull(reader["MaritalStatus"]);
                        item.UniversalID = Security.base64Decode((string) DALHelper.HandleDBNull(reader["CivilID"]));
                        item.PatientTypeID = (long) DALHelper.HandleDBNull(reader["PatientCategoryID"]);
                        item.LinkServer = nvo.LinkServer;
                        item.AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"]));
                        item.IsDocAttached = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDocAttached"]));
                        nvo.PatientDetailsList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject GetPatientHeaderDetailsForConsole(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientConsoleHeaderDeailsBizActionVO nvo = valueObject as clsGetPatientConsoleHeaderDeailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientDetailsConsole");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ISOPDIPD", DbType.Int64, nvo.ISOPDIPD);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientDetails == null)
                    {
                        nvo.PatientDetails = new clsPatientConsoleHeaderVO();
                    }
                    while (reader.Read())
                    {
                        nvo.PatientDetails.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        nvo.PatientDetails.LastName = Convert.ToString(DALHelper.HandleDBNull(reader["SurName"]));
                        nvo.PatientDetails.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        nvo.PatientDetails.AgeInYearMonthDays = Convert.ToString(DALHelper.HandleDBNull(reader["AgeInYearMonthDays"]));
                        nvo.PatientDetails.Age = Convert.ToInt32(DALHelper.HandleDBNull(reader["Age"]));
                        nvo.PatientDetails.AgeInYearMonthDays = (nvo.PatientDetails.Age <= 1) ? (nvo.PatientDetails.Age.ToString() + " Year") : (nvo.PatientDetails.Age.ToString() + " Years");
                        nvo.PatientDetails.RegisteredClinic = (string) DALHelper.HandleDBNull(reader["RegisteredClinic"]);
                        nvo.PatientDetails.MaritalStatus = Convert.ToString(reader["MaritalStatus"]);
                        nvo.PatientDetails.Gender = Convert.ToString(reader["Gender"]);
                        nvo.PatientDetails.MOB = Convert.ToString(reader["MOB"]);
                        nvo.PatientDetails.BirthDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["DateOfBirth"])));
                        nvo.PatientDetails.Photo = (byte[]) DALHelper.HandleDBNull(reader["Photo"]);
                        string[] strArray = new string[] { "https://", this.ImgIP, "/", this.ImgVirtualDir, "/", Convert.ToString(DALHelper.HandleDBNull(reader["ImgPath"])) };
                        nvo.PatientDetails.ImageName = string.Concat(strArray);
                        nvo.PatientDetails.Education = Convert.ToString(reader["Education"]);
                        nvo.PatientDetails.Religion = Convert.ToString(reader["Religion"]);
                        nvo.PatientDetails.Weight = Convert.ToInt64(DALHelper.HandleDBNull(reader["Weight"]));
                        nvo.PatientDetails.Height = Convert.ToInt64(DALHelper.HandleDBNull(reader["Height"]));
                        nvo.PatientDetails.BMI = nvo.PatientDetails.Weight / ((nvo.PatientDetails.Height * nvo.PatientDetails.Height) / 10000.0);
                        if (double.IsNaN(nvo.PatientDetails.BMI))
                        {
                            nvo.PatientDetails.BMI = 0.0;
                        }
                        if (double.IsInfinity(nvo.PatientDetails.BMI))
                        {
                            nvo.PatientDetails.BMI = 0.0;
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

        public override IValueObject GetPatientLabUploadReportData(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientLabUploadReportDataBizActionVO nvo = valueObject as clsGetPatientLabUploadReportDataBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientLabUploadReportData");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsPathology", DbType.Boolean, nvo.IsPathology);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                while (true)
                {
                    if (!reader.Read())
                    {
                        reader.Close();
                        break;
                    }
                    nvo.SourceURL = Convert.ToString(DALHelper.HandleDBNull(reader["SourceURL"]));
                    nvo.Report = (byte[]) DALHelper.HandleDBNull(reader["Report"]);
                }
            }
            catch (Exception)
            {
                reader.Close();
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPatientLinkFile(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientLinkFileBizActionVO nvo = valueObject as clsGetPatientLinkFileBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientLinkFile");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientDetails == null)
                    {
                        nvo.PatientDetails = new List<clsPatientLinkFileBizActionVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientLinkFileBizActionVO item = new clsPatientLinkFileBizActionVO {
                            ID = DALHelper.HandleIntegerNull(reader["ID"]),
                            UnitId = DALHelper.HandleIntegerNull(reader["UnitId"]),
                            PatientID = DALHelper.HandleIntegerNull(reader["PatientID"]),
                            PatientUnitID = DALHelper.HandleIntegerNull(reader["PatientUnitID"]),
                            VisitID = DALHelper.HandleIntegerNull(reader["VisitID"]),
                            ReferredBy = (string) DALHelper.HandleDBNull(reader["ReferredBy"]),
                            Report = (byte[]) DALHelper.HandleDBNull(reader["Report"]),
                            DocumentName = (string) DALHelper.HandleDBNull(reader["DocumentName"]),
                            SourceURL = (string) DALHelper.HandleDBNull(reader["SourceURL"]),
                            Remarks = (string) DALHelper.HandleDBNull(reader["Remarks"])
                        };
                        DateTime? nullable = DALHelper.HandleDate(reader["Date"]);
                        item.Date = new DateTime?(nullable.Value);
                        DateTime? nullable2 = DALHelper.HandleDate(reader["Time"]);
                        item.Time = new DateTime?(nullable2.Value);
                        nvo.PatientDetails.Add(item);
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

        public override IValueObject GetPatientLinkFileViewDetals(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientLinkFileViewDetailsBizActionVO nvo = valueObject as clsGetPatientLinkFileViewDetailsBizActionVO;
            try
            {
                if (!nvo.FROMEMR)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientViewLinkFileDetals");
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ReportID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            nvo.SourceURL = Convert.ToString(DALHelper.HandleDBNull(reader["SourceURL"]));
                            nvo.Report = (byte[]) DALHelper.HandleDBNull(reader["Report"]);
                        }
                        reader.Close();
                    }
                }
                else
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetEMRImage");
                    this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                    this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                    this.dbServer.AddInParameter(storedProcCommand, "OPDIPDID", DbType.Int64, nvo.VisitID);
                    DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader2.HasRows)
                    {
                        if (nvo.PatientDetails == null)
                        {
                            nvo.PatientDetails = new List<clsPatientLinkFileBizActionVO>();
                        }
                        while (reader2.Read())
                        {
                            clsPatientLinkFileBizActionVO item = new clsPatientLinkFileBizActionVO {
                                SourceURL = Convert.ToString(DALHelper.HandleDBNull(reader2["SourceURL"])),
                                Report = (byte[]) DALHelper.HandleDBNull(reader2["Report"]),
                                DocumentName = Convert.ToString(DALHelper.HandleDBNull(reader2["DocumentName"])),
                                TemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["TemplateID"]))
                            };
                            nvo.PatientDetails.Add(item);
                        }
                    }
                    reader2.NextResult();
                    if (reader2.HasRows)
                    {
                        while (reader2.Read())
                        {
                            nvo.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader2["Remark"]));
                        }
                    }
                    reader2.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetPatientList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientListBizActionVO nvo = valueObject as clsGetPatientListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientList");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, nvo.GenderID);
                if (nvo.SearchName)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.Description);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, Security.base64Encode(nvo.Description));
                }
                this.dbServer.AddInParameter(storedProcCommand, "SearchBy", DbType.Boolean, nvo.SearchName);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.SortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientDetails == null)
                    {
                        nvo.PatientDetails = new List<clsPatientVO>();
                    }
                    if (nvo.PatientDetailsList == null)
                    {
                        nvo.PatientDetailsList = new List<clsPatientGeneralVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientVO item = new clsPatientVO {
                            GeneralDetails = { 
                                PatientID = (long) DALHelper.HandleDBNull(reader["ID"]),
                                UnitId = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                                MRNo = (string) DALHelper.HandleDBNull(reader["MRNo"]),
                                LastName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["LastName"])),
                                FirstName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["FirstName"])),
                                MiddleName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["MiddleName"])),
                                DateOfBirth = DALHelper.HandleDate(reader["DateOfBirth"])
                            }
                        };
                        nvo.PatientDetails.Add(item);
                        clsPatientGeneralVO lvo = new clsPatientGeneralVO {
                            PatientID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitId = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            MRNo = (string) DALHelper.HandleDBNull(reader["MRNo"]),
                            LastName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["LastName"])),
                            FirstName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["FirstName"])),
                            MiddleName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["MiddleName"])),
                            DateOfBirth = DALHelper.HandleDate(reader["DateOfBirth"])
                        };
                        nvo.PatientDetailsList.Add(lvo);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject GetPatientMRNoList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientPhotoToServerBizActionVO nvo = (clsAddPatientPhotoToServerBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientMRNoList");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientDetailsList == null)
                    {
                        nvo.PatientDetailsList = new List<clsPatientVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientVO item = new clsPatientVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            MRNo = (string) DALHelper.HandleDBNull(reader["MRNo"])
                        };
                        nvo.PatientDetailsList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject GetPatientPenPusherDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPenPusherDetailListBizActionVO nvo = valueObject as clsGetPatientPenPusherDetailListBizActionVO;
            nvo.PatientPenPusherDetailsInfo = new clsPatientPenPusherInfoVO();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientPenPusherDetailsList");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ID");
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    nvo.PatientPenPusherDetailsList = new List<clsPatientPenPusherInfoVO>();
                    while (reader.Read())
                    {
                        clsPatientPenPusherInfoVO item = new clsPatientPenPusherInfoVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"])),
                            PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]))
                        };
                        if (Convert.ToString(DALHelper.HandleDBNull(reader["Notes"])) != null)
                        {
                            item.Notes = Convert.ToString(DALHelper.HandleDBNull(reader["Notes"]));
                        }
                        nvo.PatientPenPusherDetailsList.Add(item);
                    }
                }
                reader.Close();
                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetPatientScanDoc(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientScanDocument document = valueObject as clsGetPatientScanDocument;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientScanDocuments");
                if (document.PatientScanDoc.DoctorID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, document.PatientScanDoc.DoctorID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, document.PatientScanDoc.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, document.PatientScanDoc.PatientUnitID);
                }
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    document.PatientScanDocList = new List<clsPatientScanDocumentVO>();
                    while (reader.Read())
                    {
                        clsPatientScanDocumentVO item = new clsPatientScanDocumentVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            IdentityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IdentityID"])),
                            Identity = Convert.ToString(DALHelper.HandleDBNull(reader["Identity"])),
                            IdentityNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IdentityNumber"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            IsForSpouse = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsForSpouse"])),
                            AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"]))
                        };
                        string str = Convert.ToString(DALHelper.HandleDBNull(reader["ImagePath"]));
                        if (!string.IsNullOrEmpty(str))
                        {
                            string[] strArray = new string[] { "https://", this.DocImgIP, "/", this.DocImgVirtualDir, "/", str };
                            item.ImageName = string.Concat(strArray);
                        }
                        document.PatientScanDocList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetPatientSignConsent(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientSignConsentBizActionVO nvo = valueObject as clsGetPatientSignConsentBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_Get_PatientSignConsent");
                this.dbServer.AddInParameter(storedProcCommand, "ConsentID", DbType.Int64, nvo.ConsentID);
                this.dbServer.AddInParameter(storedProcCommand, "ConsentUnitID", DbType.Int64, nvo.ConsentUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.Status);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.SignPatientList == null)
                    {
                        nvo.SignPatientList = new List<clsPatientSignConsentVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientSignConsentVO item = new clsPatientSignConsentVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            Date = (DateTime) DALHelper.HandleDBNull(reader["Date"]),
                            ConsentID = (long) DALHelper.HandleDBNull(reader["ConsentID"]),
                            ConsentUnitID = (long) DALHelper.HandleDBNull(reader["ConsentUnitID"]),
                            PatientID = (long) DALHelper.HandleDBNull(reader["PatientID"]),
                            PatientUnitID = (long) DALHelper.HandleDBNull(reader["PatientUnitID"]),
                            DocumentName = (string) DALHelper.HandleDBNull(reader["DocumentName"]),
                            Remarks = (string) DALHelper.HandleDBNull(reader["Remarks"]),
                            Report = (byte[]) DALHelper.HandleDBNull(reader["Report"]),
                            ReferredBy = (string) DALHelper.HandleDBNull(reader["ReferredBy"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            SourceURL = (string) DALHelper.HandleDBNull(reader["SourceURL"]),
                            PlanTherapyID = (long) DALHelper.HandleDBNull(reader["PlanTherapyID"]),
                            PlanTherapyUnitID = (long) DALHelper.HandleDBNull(reader["PlanTherapyUnitID"])
                        };
                        nvo.SignPatientList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetPatientTariffs(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientTariffsBizActionVO nvo = (clsGetPatientTariffsBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientTariffDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientSourceID", DbType.Int64, nvo.PatientSourceID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.CheckDate);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["TariffID"], reader["Tariff"].ToString()));
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject GetPrintedPatientConscent(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPrintedPatientConscentBizActionVO nvo = valueObject as clsGetPrintedPatientConscentBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_rptPatientConsentPrint");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ConsentDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.ConsentDetails.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ConsentDetails == null)
                    {
                        nvo.ConsentDetails = new clsPatientConsentVO();
                    }
                    while (reader.Read())
                    {
                        nvo.ConsentDetails.Template = (string) DALHelper.HandleDBNull(reader["Template"]);
                        nvo.ConsentDetails.PatientID = (long) DALHelper.HandleDBNull(reader["PatientID"]);
                        nvo.ConsentDetails.PatientUnitID = (long) DALHelper.HandleDBNull(reader["PatientUnitID"]);
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

        public override IValueObject GetSurrogate(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientBizActionVO nvo = valueObject as clsGetPatientBizActionVO;
            try
            {
                clsPatientVO patientDetails = nvo.PatientDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSurrogate");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, patientDetails.GeneralDetails.LinkServer);
                if (patientDetails.GeneralDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, patientDetails.GeneralDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, patientDetails.GeneralDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, patientDetails.GeneralDetails.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.PatientDetails.GeneralDetails.PatientTypeID = (long) DALHelper.HandleDBNull(reader["PatientCategoryID"]);
                        nvo.PatientDetails.GeneralDetails.ReferralTypeID = (long) DALHelper.HandleDBNull(reader["ReferralTypeID"]);
                        nvo.PatientDetails.CompanyName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["CompanyName"]));
                        nvo.PatientDetails.GeneralDetails.PatientID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.PatientDetails.GeneralDetails.FirstName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["FirstName"]));
                        nvo.PatientDetails.GeneralDetails.MiddleName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["MiddleName"]));
                        nvo.PatientDetails.GeneralDetails.LastName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["LastName"]));
                        nvo.PatientDetails.GeneralDetails.MRNo = (string) DALHelper.HandleDBNull(reader["MRNo"]);
                        nvo.PatientDetails.GeneralDetails.RegistrationDate = DALHelper.HandleDate(reader["RegistrationDate"]);
                        nvo.PatientDetails.GeneralDetails.DateOfBirth = DALHelper.HandleDate(reader["DateOfBirth"]);
                        nvo.PatientDetails.GeneralDetails.PatientSourceID = (long) DALHelper.HandleDBNull(reader["PatientSourceID"]);
                        nvo.PatientDetails.GeneralDetails.TariffID = (long) DALHelper.HandleDBNull(reader["TariffID"]);
                        nvo.PatientDetails.FamilyName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["FamilyName"]));
                        nvo.PatientDetails.GenderID = (long) DALHelper.HandleDBNull(reader["GenderID"]);
                        nvo.PatientDetails.BloodGroupID = (long) DALHelper.HandleDBNull(reader["BloodGroupID"]);
                        nvo.PatientDetails.MaritalStatusID = (long) DALHelper.HandleDBNull(reader["MaritalStatusID"]);
                        nvo.PatientDetails.CivilID = Security.base64Decode((string) DALHelper.HandleDBNull(reader["CivilID"]));
                        nvo.PatientDetails.ContactNo1 = (string) DALHelper.HandleDBNull(reader["ContactNo1"]);
                        nvo.PatientDetails.ContactNo2 = (string) DALHelper.HandleDBNull(reader["ContactNo2"]);
                        nvo.PatientDetails.FaxNo = (string) DALHelper.HandleDBNull(reader["FaxNo"]);
                        nvo.PatientDetails.Email = Security.base64Decode((string) DALHelper.HandleDBNull(reader["Email"]));
                        nvo.PatientDetails.AddressLine1 = Security.base64Decode((string) DALHelper.HandleDBNull(reader["AddressLine1"]));
                        nvo.PatientDetails.AddressLine2 = Security.base64Decode((string) DALHelper.HandleDBNull(reader["AddressLine2"]));
                        nvo.PatientDetails.AddressLine3 = Security.base64Decode((string) DALHelper.HandleDBNull(reader["AddressLine3"]));
                        nvo.PatientDetails.Country = Security.base64Decode((string) DALHelper.HandleDBNull(reader["Country"]));
                        nvo.PatientDetails.State = Security.base64Decode((string) DALHelper.HandleDBNull(reader["State"]));
                        nvo.PatientDetails.City = Security.base64Decode((string) DALHelper.HandleDBNull(reader["City"]));
                        nvo.PatientDetails.Taluka = Security.base64Decode((string) DALHelper.HandleDBNull(reader["Taluka"]));
                        nvo.PatientDetails.Area = Security.base64Decode((string) DALHelper.HandleDBNull(reader["Area"]));
                        nvo.PatientDetails.District = Security.base64Decode((string) DALHelper.HandleDBNull(reader["District"]));
                        nvo.PatientDetails.MobileCountryCode = Convert.ToString(DALHelper.HandleDBNull(reader["MobileCountryCode"]));
                        nvo.PatientDetails.ResiNoCountryCode = (long) DALHelper.HandleDBNull(reader["ResiNoCountryCode"]);
                        nvo.PatientDetails.ResiSTDCode = (long) DALHelper.HandleDBNull(reader["ResiSTDCode"]);
                        nvo.PatientDetails.CountryID = DALHelper.HandleIntegerNull(reader["CountryID"]);
                        nvo.PatientDetails.StateID = DALHelper.HandleIntegerNull(reader["StateID"]);
                        nvo.PatientDetails.CityID = DALHelper.HandleIntegerNull(reader["CityID"]);
                        nvo.PatientDetails.RegionID = DALHelper.HandleIntegerNull(reader["RegionID"]);
                        nvo.PatientDetails.Pincode = Security.base64Decode((string) DALHelper.HandleDBNull(reader["Pincode"]));
                        nvo.PatientDetails.ReligionID = (long) DALHelper.HandleDBNull(reader["ReligionID"]);
                        nvo.PatientDetails.OccupationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["OccupationId"]));
                        string str = Convert.ToString(DALHelper.HandleDBNull(reader["ImgPath"]));
                        string[] strArray = new string[] { "https://", this.ImgIP, "/", this.ImgVirtualDir, "/", str };
                        nvo.PatientDetails.ImageName = string.Concat(strArray);
                        if (((byte[]) DALHelper.HandleDBNull(reader["Photo"])) != null)
                        {
                            nvo.PatientDetails.Photo = (byte[]) DALHelper.HandleDBNull(reader["Photo"]);
                        }
                        if (nvo.PatientDetails.GeneralDetails.Photo != null)
                        {
                            nvo.PatientDetails.GeneralDetails.Photo = (byte[]) DALHelper.HandleDBNull(reader["Photo"]);
                        }
                        nvo.PatientDetails.IsLoyaltyMember = (bool) DALHelper.HandleDBNull(reader["IsLoyaltyMember"]);
                        nvo.PatientDetails.LoyaltyCardID = (long?) DALHelper.HandleDBNull(reader["LoyaltyCardID"]);
                        nvo.PatientDetails.IssueDate = DALHelper.HandleDate(reader["IssueDate"]);
                        nvo.PatientDetails.EffectiveDate = DALHelper.HandleDate(reader["EffectiveDate"]);
                        nvo.PatientDetails.ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]);
                        nvo.PatientDetails.LoyaltyCardNo = (string) DALHelper.HandleDBNull(reader["LoyaltyCardNo"]);
                        nvo.PatientDetails.RelationID = (long) DALHelper.HandleDBNull(reader["RelationID"]);
                        nvo.PatientDetails.ParentPatientID = (long) DALHelper.HandleDBNull(reader["ParentPatientID"]);
                        nvo.PatientDetails.GeneralDetails.UnitId = (long) DALHelper.HandleDBNull(reader["UnitId"]);
                        nvo.PatientDetails.CreatedUnitId = (long) DALHelper.HandleDBNull(reader["CreatedUnitId"]);
                        nvo.PatientDetails.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                        nvo.PatientDetails.AddedBy = (long?) DALHelper.HandleDBNull(reader["AddedBy"]);
                        nvo.PatientDetails.AddedOn = (string) DALHelper.HandleDBNull(reader["AddedOn"]);
                        nvo.PatientDetails.AddedDateTime = DALHelper.HandleDate(reader["AddedDateTime"]);
                        nvo.PatientDetails.AddedWindowsLoginName = (string) DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]);
                        nvo.PatientDetails.UpdatedUnitId = (long?) DALHelper.HandleDBNull(reader["UpdatedUnitID"]);
                        nvo.PatientDetails.UpdatedBy = (long?) DALHelper.HandleDBNull(reader["UpdatedBy"]);
                        nvo.PatientDetails.UpdatedOn = (string) DALHelper.HandleDBNull(reader["UpdatedOn"]);
                        nvo.PatientDetails.UpdatedDateTime = DALHelper.HandleDate(reader["UpdatedDateTime"]);
                        nvo.PatientDetails.UpdatedWindowsLoginName = (string) DALHelper.HandleDBNull(reader["UpdatedWindowsLoginName"]);
                        nvo.PatientDetails.SpouseDetails.RegistrationDate = (DateTime?) DALHelper.HandleDBNull(reader["SpouseRegistrationDate"]);
                        nvo.PatientDetails.SpouseDetails.MRNo = (string) DALHelper.HandleDBNull(reader["SpouseMRNo"]);
                        nvo.PatientDetails.SpouseDetails.LastName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["SpouseLastName"]));
                        nvo.PatientDetails.SpouseDetails.MiddleName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["SpouseMiddleName"]));
                        nvo.PatientDetails.SpouseDetails.FirstName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["SpouseFirstName"]));
                        nvo.PatientDetails.SpouseDetails.FamilyName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["SpouseFamilyName"]));
                        nvo.PatientDetails.SpouseDetails.GenderID = (long) DALHelper.HandleDBNull(reader["SpouseGenderID"]);
                        nvo.PatientDetails.SpouseDetails.DateOfBirth = DALHelper.HandleDate(reader["SpouseDateOfBirth"]);
                        nvo.PatientDetails.SpouseDetails.BloodGroupID = (long) DALHelper.HandleDBNull(reader["SpouseBloodGroupID"]);
                        nvo.PatientDetails.SpouseDetails.MaritalStatusID = (long) DALHelper.HandleDBNull(reader["SpouseMaritalStatusID"]);
                        nvo.PatientDetails.SpouseDetails.ContactNo1 = (string) DALHelper.HandleDBNull(reader["SpouseContactNo1"]);
                        nvo.PatientDetails.SpouseDetails.ContactNo2 = (string) DALHelper.HandleDBNull(reader["SpouseContactNo2"]);
                        nvo.PatientDetails.SpouseDetails.FaxNo = (string) DALHelper.HandleDBNull(reader["SpouseFaxNo"]);
                        nvo.PatientDetails.SpouseDetails.Email = Security.base64Decode((string) DALHelper.HandleDBNull(reader["SpouseEmail"]));
                        nvo.PatientDetails.SpouseDetails.AddressLine1 = Security.base64Decode((string) DALHelper.HandleDBNull(reader["SpouseAddressLine1"]));
                        nvo.PatientDetails.SpouseDetails.AddressLine2 = Security.base64Decode((string) DALHelper.HandleDBNull(reader["SpouseAddressLine2"]));
                        nvo.PatientDetails.SpouseDetails.AddressLine3 = Security.base64Decode((string) DALHelper.HandleDBNull(reader["SpouseAddressLine3"]));
                        nvo.PatientDetails.SpouseDetails.Pincode = Security.base64Decode((string) DALHelper.HandleDBNull(reader["SpousePincode"]));
                        if (((byte[]) DALHelper.HandleDBNull(reader["SpousePhoto"])) != null)
                        {
                            nvo.PatientDetails.SpouseDetails.Photo = (byte[]) DALHelper.HandleDBNull(reader["SpousePhoto"]);
                        }
                        nvo.PatientDetails.SpouseDetails.OccupationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpouseOccupationId"]));
                        nvo.PatientDetails.SpouseDetails.CivilID = (string) DALHelper.HandleDBNull(reader["SpouseCivilID"]);
                        nvo.PatientDetails.SpouseDetails.District = Security.base64Decode((string) DALHelper.HandleDBNull(reader["SpouseDistrict"]));
                        nvo.PatientDetails.SpouseDetails.Taluka = Security.base64Decode((string) DALHelper.HandleDBNull(reader["SpouseTaluka"]));
                        nvo.PatientDetails.SpouseDetails.Area = Security.base64Decode((string) DALHelper.HandleDBNull(reader["SpouseArea"]));
                        nvo.PatientDetails.SpouseDetails.MobileCountryCode = Convert.ToString(DALHelper.HandleDBNull(reader["SpouseMobileCountryCode"]));
                        nvo.PatientDetails.SpouseDetails.ResiNoCountryCode = (long) DALHelper.HandleDBNull(reader["SpouseResiNoCountryCode"]);
                        nvo.PatientDetails.SpouseDetails.Country = Security.base64Decode((string) DALHelper.HandleDBNull(reader["SpouseCountry"]));
                        nvo.PatientDetails.SpouseDetails.State = Security.base64Decode((string) DALHelper.HandleDBNull(reader["SpouseState"]));
                        nvo.PatientDetails.SpouseDetails.City = Security.base64Decode((string) DALHelper.HandleDBNull(reader["SpouseCity"]));
                        nvo.PatientDetails.SpouseDetails.SpouseCompanyName = (string) DALHelper.HandleDBNull(reader["SpouseCompanyName"]);
                        nvo.PatientDetails.SpouseDetails.CountryID = DALHelper.HandleIntegerNull(reader["SpouseCountryID"]);
                        nvo.PatientDetails.SpouseDetails.StateID = DALHelper.HandleIntegerNull(reader["SpouseStateID"]);
                        nvo.PatientDetails.SpouseDetails.CityID = DALHelper.HandleIntegerNull(reader["SpouseCityID"]);
                        nvo.PatientDetails.SpouseDetails.RegionID = DALHelper.HandleIntegerNull(reader["SpouseRegionID"]);
                        nvo.PatientDetails.SpouseDetails.ReligionID = DALHelper.HandleIntegerNull(reader["SpouseReligionID"]);
                        nvo.PatientDetails.AgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgencyID"]));
                        nvo.PatientDetails.SurrogateOtherDetails = Convert.ToString(DALHelper.HandleDBNull(reader["SurrogateOtherDetails"]));
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

        public override IValueObject GetTariffAndRelationFromApplication(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetTariffAndRelationFromApplicationConfigurationBizActionVO nvo = (clsGetTariffAndRelationFromApplicationConfigurationBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIssueDetailsFromConfig_Application");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.FamilyDetails == null)
                    {
                        nvo.FamilyDetails = new List<clsPatientFamilyDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientFamilyDetailsVO item = new clsPatientFamilyDetailsVO {
                            RelationID = (long) DALHelper.HandleDBNull(reader["SelfRelationID"]),
                            Relation = (string) DALHelper.HandleDBNull(reader["Relation"])
                        };
                        nvo.FamilyDetails.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        private clsAddPatientBizActionVO InsertFromOPDPatientDetailsIPDWithTransaction(clsAddPatientBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection pConnection = this.dbServer.CreateConnection();
            try
            {
                clsPatientVO patientDetails = BizActionObj.PatientDetails;
                pConnection.Open();
                transaction = pConnection.BeginTransaction();
                if (BizActionObj.IsSavePatientFromIPD && BizActionObj.IsSaveInTRegistration)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatient");
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, patientDetails.GeneralDetails.LinkServer);
                    if (patientDetails.GeneralDetails.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, patientDetails.GeneralDetails.LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "PatientCategoryID", DbType.Int64, patientDetails.GeneralDetails.PatientTypeID);
                    this.dbServer.AddInParameter(storedProcCommand, "ReferralTypeID", DbType.Int64, patientDetails.GeneralDetails.ReferralTypeID);
                    this.dbServer.AddInParameter(storedProcCommand, "CompanyName", DbType.String, Security.base64Encode(patientDetails.CompanyName));
                    this.dbServer.AddInParameter(storedProcCommand, "RegistrationDate", DbType.DateTime, patientDetails.GeneralDetails.RegistrationDate);
                    if (patientDetails.GeneralDetails.LastName != null)
                    {
                        patientDetails.GeneralDetails.LastName = patientDetails.GeneralDetails.LastName.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.LastName));
                    if (patientDetails.GeneralDetails.FirstName != null)
                    {
                        patientDetails.GeneralDetails.FirstName = patientDetails.GeneralDetails.FirstName.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.FirstName));
                    if (patientDetails.GeneralDetails.MiddleName != null)
                    {
                        patientDetails.GeneralDetails.MiddleName = patientDetails.GeneralDetails.MiddleName.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.MiddleName));
                    if (patientDetails.FamilyName != null)
                    {
                        patientDetails.FamilyName = patientDetails.FamilyName.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "FamilyName", DbType.String, Security.base64Encode(patientDetails.FamilyName));
                    this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, patientDetails.GenderID);
                    this.dbServer.AddInParameter(storedProcCommand, "DateOfBirth", DbType.DateTime, patientDetails.GeneralDetails.DateOfBirth);
                    this.dbServer.AddInParameter(storedProcCommand, "BloodGroupID", DbType.Int64, patientDetails.BloodGroupID);
                    this.dbServer.AddInParameter(storedProcCommand, "MaritalStatusID", DbType.Int64, patientDetails.MaritalStatusID);
                    this.dbServer.AddInParameter(storedProcCommand, "IsIPDPatient", DbType.Boolean, patientDetails.IsIPDPatient);
                    this.dbServer.AddInParameter(storedProcCommand, "CivilID", DbType.String, Security.base64Encode(patientDetails.CivilID));
                    if (patientDetails.ContactNo1 != null)
                    {
                        patientDetails.ContactNo1 = patientDetails.ContactNo1.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "ContactNo1", DbType.String, patientDetails.ContactNo1);
                    if (patientDetails.ContactNo2 != null)
                    {
                        patientDetails.ContactNo2 = patientDetails.ContactNo2.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "ContactNo2", DbType.String, patientDetails.ContactNo2);
                    if (patientDetails.FaxNo != null)
                    {
                        patientDetails.FaxNo = patientDetails.FaxNo.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "FaxNo", DbType.String, patientDetails.FaxNo);
                    if (patientDetails.Email != null)
                    {
                        patientDetails.Email = patientDetails.Email.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "Email", DbType.String, Security.base64Encode(patientDetails.Email));
                    if (patientDetails.AddressLine1 != null)
                    {
                        patientDetails.AddressLine1 = patientDetails.AddressLine1.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "AddressLine1", DbType.String, Security.base64Encode(patientDetails.AddressLine1));
                    if (patientDetails.AddressLine2 != null)
                    {
                        patientDetails.AddressLine2 = patientDetails.AddressLine2.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "AddressLine2", DbType.String, Security.base64Encode(patientDetails.AddressLine2));
                    if (patientDetails.AddressLine3 != null)
                    {
                        patientDetails.AddressLine3 = patientDetails.AddressLine3.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "AddressLine3", DbType.String, Security.base64Encode(patientDetails.AddressLine3));
                    if (patientDetails.Country != null)
                    {
                        patientDetails.Country = patientDetails.Country.Trim();
                    }
                    if (patientDetails.OldRegistrationNo != null)
                    {
                        patientDetails.OldRegistrationNo = patientDetails.OldRegistrationNo.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "OldRegistrationNo", DbType.String, patientDetails.OldRegistrationNo);
                    if (patientDetails.Country != null)
                    {
                        patientDetails.Country = patientDetails.Country.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "Country", DbType.String, Security.base64Encode(patientDetails.Country));
                    if (patientDetails.State != null)
                    {
                        patientDetails.State = patientDetails.State.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "State", DbType.String, Security.base64Encode(patientDetails.State));
                    if (patientDetails.City != null)
                    {
                        patientDetails.City = patientDetails.City.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "City", DbType.String, Security.base64Encode(patientDetails.City));
                    if (patientDetails.Taluka != null)
                    {
                        patientDetails.Taluka = patientDetails.Taluka.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "Taluka", DbType.String, Security.base64Encode(patientDetails.Taluka));
                    if (patientDetails.Area != null)
                    {
                        patientDetails.Area = patientDetails.Area.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "Area", DbType.String, Security.base64Encode(patientDetails.Area));
                    if (patientDetails.District != null)
                    {
                        patientDetails.District = patientDetails.District.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "District", DbType.String, Security.base64Encode(patientDetails.District));
                    this.dbServer.AddInParameter(storedProcCommand, "MobileCountryCode", DbType.String, patientDetails.MobileCountryCode);
                    this.dbServer.AddInParameter(storedProcCommand, "ResiNoCountryCode", DbType.Int64, patientDetails.ResiNoCountryCode);
                    this.dbServer.AddInParameter(storedProcCommand, "ResiSTDCode", DbType.Int64, patientDetails.ResiSTDCode);
                    if (patientDetails.Pincode != null)
                    {
                        patientDetails.Pincode = patientDetails.Pincode.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "Pincode", DbType.String, Security.base64Encode(patientDetails.Pincode));
                    this.dbServer.AddInParameter(storedProcCommand, "ReligionID", DbType.Int64, patientDetails.ReligionID);
                    this.dbServer.AddInParameter(storedProcCommand, "OccupationId", DbType.Int64, patientDetails.OccupationId);
                    this.dbServer.AddInParameter(storedProcCommand, "Photo", DbType.Binary, patientDetails.Photo);
                    this.dbServer.AddInParameter(storedProcCommand, "IsLoyaltyMember", DbType.Boolean, patientDetails.IsLoyaltyMember);
                    this.dbServer.AddInParameter(storedProcCommand, "LoyaltyCardID", DbType.Int64, patientDetails.LoyaltyCardID);
                    this.dbServer.AddInParameter(storedProcCommand, "RelationID", DbType.Int64, patientDetails.RelationID);
                    this.dbServer.AddInParameter(storedProcCommand, "ParentPatientID", DbType.Int64, patientDetails.ParentPatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "IssueDate", DbType.DateTime, patientDetails.IssueDate);
                    this.dbServer.AddInParameter(storedProcCommand, "EffectiveDate", DbType.DateTime, patientDetails.EffectiveDate);
                    this.dbServer.AddInParameter(storedProcCommand, "ExpiryDate", DbType.DateTime, patientDetails.ExpiryDate);
                    if (patientDetails.LoyaltyCardNo != null)
                    {
                        patientDetails.LoyaltyCardNo = patientDetails.LoyaltyCardNo.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "LoyaltyCardNo", DbType.String, patientDetails.LoyaltyCardNo);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, patientDetails.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, patientDetails.AddedDateTime);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                    this.dbServer.AddParameter(storedProcCommand, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????");
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientDetails.GeneralDetails.PatientID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    long num = new Random().Next(0x1b207, 0xa2c2a);
                    this.dbServer.AddInParameter(storedProcCommand, "RandomNumber", DbType.String, Convert.ToString(num));
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    BizActionObj.PatientDetails.GeneralDetails.PatientID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                    BizActionObj.PatientDetails.GeneralDetails.MRNo = (string) this.dbServer.GetParameterValue(storedProcCommand, "MRNo");
                    string parameterValue = (string) this.dbServer.GetParameterValue(storedProcCommand, "Err");
                    BizActionObj.PatientDetails.GeneralDetails.UnitId = UserVo.UserLoginInfo.UnitId;
                    object[] objArray = new object[] { BizActionObj.PatientDetails.GeneralDetails.MRNo, "_", num, ".png" };
                    string imgName = string.Concat(objArray);
                    DemoImage image = new DemoImage();
                    if (patientDetails.Photo != null)
                    {
                        image.VaryQualityLevel(patientDetails.Photo, imgName, this.ImgSaveLocation);
                    }
                    if ((BizActionObj.PatientDetails.SpouseDetails != null) && ((BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 7L) || ((BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 8L) || (BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 9))))
                    {
                        DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddPatient");
                        this.dbServer.AddInParameter(command, "PatientCategoryID", DbType.Int64, patientDetails.GeneralDetails.PatientTypeID);
                        this.dbServer.AddInParameter(storedProcCommand, "ReferralTypeID", DbType.Int64, patientDetails.GeneralDetails.ReferralTypeID);
                        this.dbServer.AddInParameter(command, "RegistrationDate", DbType.DateTime, patientDetails.GeneralDetails.RegistrationDate);
                        if (patientDetails.SpouseDetails.LastName != null)
                        {
                            patientDetails.SpouseDetails.LastName = patientDetails.SpouseDetails.LastName.Trim();
                        }
                        this.dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.LastName));
                        if (patientDetails.SpouseDetails.FirstName != null)
                        {
                            patientDetails.SpouseDetails.FirstName = patientDetails.SpouseDetails.FirstName.Trim();
                        }
                        this.dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.FirstName));
                        if (patientDetails.SpouseDetails.MiddleName != null)
                        {
                            patientDetails.SpouseDetails.MiddleName = patientDetails.SpouseDetails.MiddleName.Trim();
                        }
                        this.dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.MiddleName));
                        if (patientDetails.SpouseDetails.FamilyName != null)
                        {
                            patientDetails.SpouseDetails.FamilyName = patientDetails.SpouseDetails.FamilyName.Trim();
                        }
                        this.dbServer.AddInParameter(command, "FamilyName", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.FamilyName));
                        this.dbServer.AddInParameter(command, "GenderID", DbType.Int64, patientDetails.SpouseDetails.GenderID);
                        this.dbServer.AddInParameter(command, "DateOfBirth", DbType.DateTime, patientDetails.SpouseDetails.DateOfBirth);
                        this.dbServer.AddInParameter(command, "BloodGroupID", DbType.Int64, patientDetails.SpouseDetails.BloodGroupID);
                        this.dbServer.AddInParameter(command, "MaritalStatusID", DbType.Int64, patientDetails.SpouseDetails.MaritalStatusID);
                        this.dbServer.AddInParameter(command, "CivilID", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.CivilID));
                        if (patientDetails.SpouseDetails.ContactNo1 != null)
                        {
                            patientDetails.SpouseDetails.ContactNo1 = patientDetails.SpouseDetails.ContactNo1.Trim();
                        }
                        this.dbServer.AddInParameter(command, "ContactNo1", DbType.String, patientDetails.SpouseDetails.ContactNo1);
                        if (patientDetails.SpouseDetails.ContactNo2 != null)
                        {
                            patientDetails.SpouseDetails.ContactNo2 = patientDetails.SpouseDetails.ContactNo2.Trim();
                        }
                        this.dbServer.AddInParameter(command, "ContactNo2", DbType.String, patientDetails.SpouseDetails.ContactNo2);
                        if (patientDetails.SpouseDetails.FaxNo != null)
                        {
                            patientDetails.SpouseDetails.FaxNo = patientDetails.SpouseDetails.FaxNo.Trim();
                        }
                        this.dbServer.AddInParameter(command, "FaxNo", DbType.String, patientDetails.SpouseDetails.FaxNo);
                        if (patientDetails.SpouseDetails.Email != null)
                        {
                            patientDetails.SpouseDetails.Email = patientDetails.SpouseDetails.Email.Trim();
                        }
                        this.dbServer.AddInParameter(command, "Email", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.Email));
                        if (patientDetails.SpouseDetails.AddressLine1 != null)
                        {
                            patientDetails.SpouseDetails.AddressLine1 = patientDetails.SpouseDetails.AddressLine1.Trim();
                        }
                        this.dbServer.AddInParameter(command, "AddressLine1", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.AddressLine1));
                        if (patientDetails.SpouseDetails.AddressLine2 != null)
                        {
                            patientDetails.SpouseDetails.AddressLine2 = patientDetails.SpouseDetails.AddressLine2.Trim();
                        }
                        this.dbServer.AddInParameter(command, "AddressLine2", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.AddressLine2));
                        if (patientDetails.SpouseDetails.AddressLine3 != null)
                        {
                            patientDetails.SpouseDetails.AddressLine3 = patientDetails.SpouseDetails.AddressLine3.Trim();
                        }
                        this.dbServer.AddInParameter(command, "AddressLine3", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.AddressLine3));
                        if (patientDetails.SpouseDetails.Country != null)
                        {
                            patientDetails.SpouseDetails.Country = patientDetails.SpouseDetails.Country.Trim();
                        }
                        if (patientDetails.SpouseDetails.SpouseOldRegistrationNo != null)
                        {
                            patientDetails.SpouseDetails.SpouseOldRegistrationNo = patientDetails.SpouseDetails.SpouseOldRegistrationNo.Trim();
                        }
                        this.dbServer.AddInParameter(command, "OldRegistrationNo", DbType.String, patientDetails.SpouseDetails.SpouseOldRegistrationNo);
                        if (patientDetails.SpouseDetails.Country != null)
                        {
                            patientDetails.SpouseDetails.Country = patientDetails.SpouseDetails.Country.Trim();
                        }
                        this.dbServer.AddInParameter(command, "Country", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.Country));
                        if (patientDetails.SpouseDetails.State != null)
                        {
                            patientDetails.SpouseDetails.State = patientDetails.SpouseDetails.State.Trim();
                        }
                        this.dbServer.AddInParameter(command, "State", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.State));
                        if (patientDetails.SpouseDetails.City != null)
                        {
                            patientDetails.SpouseDetails.City = patientDetails.SpouseDetails.City.Trim();
                        }
                        this.dbServer.AddInParameter(command, "City", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.City));
                        if (patientDetails.SpouseDetails.Taluka != null)
                        {
                            patientDetails.SpouseDetails.Taluka = patientDetails.SpouseDetails.Taluka.Trim();
                        }
                        this.dbServer.AddInParameter(command, "Taluka", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.Taluka));
                        if (patientDetails.SpouseDetails.Area != null)
                        {
                            patientDetails.SpouseDetails.Area = patientDetails.SpouseDetails.Area.Trim();
                        }
                        this.dbServer.AddInParameter(command, "Area", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.Area));
                        if (patientDetails.SpouseDetails.District != null)
                        {
                            patientDetails.SpouseDetails.District = patientDetails.SpouseDetails.District.Trim();
                        }
                        this.dbServer.AddInParameter(command, "District", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.District));
                        this.dbServer.AddInParameter(command, "MobileCountryCode", DbType.String, patientDetails.SpouseDetails.MobileCountryCode);
                        this.dbServer.AddInParameter(command, "ResiNoCountryCode", DbType.Int64, patientDetails.SpouseDetails.ResiNoCountryCode);
                        this.dbServer.AddInParameter(command, "ResiSTDCode", DbType.Int64, patientDetails.SpouseDetails.ResiSTDCode);
                        if (patientDetails.SpouseDetails.Pincode != null)
                        {
                            patientDetails.Pincode = patientDetails.SpouseDetails.Pincode.Trim();
                        }
                        this.dbServer.AddInParameter(command, "Pincode", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.Pincode));
                        this.dbServer.AddInParameter(command, "CompanyName", DbType.String, Security.base64Encode(patientDetails.CompanyName));
                        this.dbServer.AddInParameter(command, "ReligionID", DbType.Int64, patientDetails.SpouseDetails.ReligionID);
                        this.dbServer.AddInParameter(command, "OccupationId", DbType.Int64, patientDetails.SpouseDetails.OccupationId);
                        this.dbServer.AddInParameter(command, "Photo", DbType.Binary, patientDetails.SpouseDetails.Photo);
                        this.dbServer.AddInParameter(command, "Status", DbType.Boolean, patientDetails.Status);
                        this.dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, patientDetails.AddedDateTime);
                        this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        patientDetails.SpouseDetails.MRNo = patientDetails.GeneralDetails.MRNo.Remove(patientDetails.GeneralDetails.MRNo.Length - 1, 1);
                        this.dbServer.AddParameter(command, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        this.dbServer.AddParameter(command, "MRNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientDetails.SpouseDetails.MRNo);
                        this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientDetails.SpouseDetails.PatientID);
                        this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                        long num2 = new Random().Next(0x1b207, 0xa2c2a);
                        this.dbServer.AddInParameter(command, "RandomNumber", DbType.String, Convert.ToString(num2));
                        this.dbServer.ExecuteNonQuery(command, transaction);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                        BizActionObj.PatientDetails.SpouseDetails.PatientID = (long) this.dbServer.GetParameterValue(command, "ID");
                        BizActionObj.PatientDetails.SpouseDetails.MRNo = (string) this.dbServer.GetParameterValue(command, "MRNo");
                        string text2 = (string) this.dbServer.GetParameterValue(command, "Err");
                        BizActionObj.PatientDetails.SpouseDetails.UnitId = UserVo.UserLoginInfo.UnitId;
                        object[] objArray2 = new object[] { BizActionObj.PatientDetails.SpouseDetails.MRNo, "_", num2, ".png" };
                        string str2 = string.Concat(objArray2);
                        if (patientDetails.SpouseDetails.Photo != null)
                        {
                            image.VaryQualityLevel(patientDetails.SpouseDetails.Photo, str2, this.ImgSaveLocation);
                        }
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddCoupleRegistration");
                        if (BizActionObj.PatientDetails.GenderID == 1L)
                        {
                            this.dbServer.AddInParameter(command3, "MalePatientID", DbType.Int64, patientDetails.GeneralDetails.PatientID);
                            this.dbServer.AddInParameter(command3, "FemalePatientID", DbType.Int64, patientDetails.SpouseDetails.PatientID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "MalePatientID", DbType.Int64, patientDetails.SpouseDetails.PatientID);
                            this.dbServer.AddInParameter(command3, "FemalePatientID", DbType.Int64, patientDetails.GeneralDetails.PatientID);
                        }
                        this.dbServer.AddInParameter(command3, "MalePatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "FemalePatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "RegistrationDate", DbType.DateTime, patientDetails.GeneralDetails.RegistrationDate);
                        this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, patientDetails.Status);
                        this.dbServer.AddInParameter(command3, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, patientDetails.AddedDateTime);
                        this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddParameter(command3, "CoupleRgistrationNumber", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "000000000000000000000000000000000000000000000000000");
                        this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                        this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
                    }
                }
                if (patientDetails.KinInformationList != null)
                {
                    foreach (clsKinInformationVO nvo in patientDetails.KinInformationList)
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatientKinInfoIPD");
                        this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, 0);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.PatientID);
                        this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, nvo.MRCode);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "sbIsRegisteredPatient", DbType.String, nvo.IsRegisteredPatient);
                        this.dbServer.AddInParameter(storedProcCommand, "sbKinPatientId", DbType.String, nvo.KinPatientID);
                        this.dbServer.AddInParameter(storedProcCommand, "sbKinPatientUnitId", DbType.String, nvo.KinPatientUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "sbLastName", DbType.String, Security.base64Encode(nvo.KinLastName));
                        this.dbServer.AddInParameter(storedProcCommand, "sbFirstName", DbType.String, Security.base64Encode(nvo.KinFirstName));
                        this.dbServer.AddInParameter(storedProcCommand, "sbMiddleName", DbType.String, Security.base64Encode(nvo.KinMiddleName));
                        this.dbServer.AddInParameter(storedProcCommand, "sbFamilyName", DbType.String, Security.base64Encode(nvo.FamilyCode));
                        this.dbServer.AddInParameter(storedProcCommand, "sbMobileCountryCode", DbType.String, nvo.MobileCountryCode);
                        this.dbServer.AddInParameter(storedProcCommand, "sbMobileNumber", DbType.String, nvo.MobileCountryCode);
                        this.dbServer.AddInParameter(storedProcCommand, "sbAddress", DbType.String, nvo.Address);
                        this.dbServer.AddInParameter(storedProcCommand, "sbRelationshipID", DbType.String, nvo.RelationshipID);
                        this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.String, nvo.Status);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, patientDetails.AddedDateTime);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    }
                }
                if (BizActionObj.IsSaveSponsor)
                {
                    clsBasePatientSposorDAL instance = clsBasePatientSposorDAL.GetInstance();
                    BizActionObj.BizActionVOSaveSponsor.PatientSponsorDetails.PatientId = BizActionObj.PatientDetails.GeneralDetails.PatientID;
                    BizActionObj.BizActionVOSaveSponsor.PatientSponsorDetails.PatientUnitId = BizActionObj.PatientDetails.GeneralDetails.UnitId;
                    BizActionObj.BizActionVOSaveSponsor = (clsAddPatientSponsorBizActionVO) instance.AddPatientSponsorDetailsWithTransaction(BizActionObj.BizActionVOSaveSponsor, UserVo, pConnection, transaction);
                    if (BizActionObj.BizActionVOSaveSponsor.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                }
                if (BizActionObj.IsSaveAdmission)
                {
                    clsBaseIPDAdmissionDAL instance = clsBaseIPDAdmissionDAL.GetInstance();
                    BizActionObj.BizActionVOSaveAdmission.Details.PatientId = BizActionObj.PatientDetails.GeneralDetails.PatientID;
                    BizActionObj.BizActionVOSaveAdmission.Details.PatientUnitID = BizActionObj.PatientDetails.GeneralDetails.UnitId;
                    BizActionObj.BizActionVOSaveAdmission.Details.IPDNO = BizActionObj.PatientDetails.GeneralDetails.MRNo;
                    BizActionObj.BizActionVOSaveAdmission = (clsSaveIPDAdmissionBizActionVO) instance.AddIPDAdmissionDetailsWithTransaction(BizActionObj.BizActionVOSaveAdmission, UserVo, pConnection, transaction);
                    if (BizActionObj.BizActionVOSaveAdmission.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                }
                transaction.Commit();
                this.addPatientUserDetails(BizActionObj, UserVo);
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw exception;
            }
            finally
            {
                transaction.Dispose();
                transaction = null;
            }
            return BizActionObj;
        }

        public override IValueObject MovePatientPhotoToServer(IValueObject valueObject, clsUserVO UserVo)
        {
            clsMovePatientPhotoToServerBizActionVO nvo = (clsMovePatientPhotoToServerBizActionVO) valueObject;
            try
            {
                foreach (clsPatientVO tvo in nvo.PatientDetailsList)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_MovePatientPhotoToServer");
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, tvo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, tvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, tvo.MRNo);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.PatientDetails == null)
                        {
                            nvo.PatientDetails = new clsPatientVO();
                        }
                        while (reader.Read())
                        {
                            tvo.Photo = (byte[]) DALHelper.HandleDBNull(reader["Photo"]);
                            tvo.ImageName = (string) DALHelper.HandleDBNull(reader["ImagePath"]);
                            new DemoImage().VaryQualityLevel(tvo.Photo, tvo.ImageName, this.ImgSaveLocation);
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject SavePatientPhoto(IValueObject valueObject, clsUserVO UserVo)
        {
            clsSavePhotoBizActionVO nvo = valueObject as clsSavePhotoBizActionVO;
            DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePatientPhoto");
            this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
            this.dbServer.AddInParameter(storedProcCommand, "Photo", DbType.Binary, nvo.Photo);
            this.dbServer.ExecuteNonQuery(storedProcCommand);
            return nvo;
        }

        private clsAddPatientDietPlanBizActionVO UpdateDiet(clsAddPatientDietPlanBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                clsPatientDietPlanVO dietPlan = BizActionObj.DietPlan;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePatientDietPlan");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, dietPlan.ID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, dietPlan.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, dietPlan.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, dietPlan.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitDoctorID", DbType.Int64, dietPlan.VisitDoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, dietPlan.Date);
                this.dbServer.AddInParameter(storedProcCommand, "PlanID", DbType.Int64, dietPlan.PlanID);
                this.dbServer.AddInParameter(storedProcCommand, "GeneralInformation", DbType.String, dietPlan.GeneralInformation);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if ((dietPlan.DietDetails != null) && (dietPlan.DietDetails.Count > 0))
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeletePatientDietPlanDetails");
                    this.dbServer.AddInParameter(command2, "DietPlanID", DbType.Int64, dietPlan.ID);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                }
                if ((dietPlan.DietDetails != null) && (dietPlan.DietDetails.Count > 0))
                {
                    foreach (clsPatientDietPlanDetailVO lvo in dietPlan.DietDetails)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddPatientDietPlanDetails");
                        this.dbServer.AddInParameter(command3, "DietPlanID", DbType.Int64, dietPlan.ID);
                        this.dbServer.AddInParameter(command3, "FoodItemID", DbType.Int64, lvo.FoodItemID);
                        this.dbServer.AddInParameter(command3, "FoodCategoryID", DbType.Int64, lvo.FoodItemCategoryID);
                        this.dbServer.AddInParameter(command3, "Timing", DbType.String, lvo.Timing);
                        this.dbServer.AddInParameter(command3, "FoodQty", DbType.String, lvo.FoodQty);
                        this.dbServer.AddInParameter(command3, "FoodUnit", DbType.String, lvo.FoodUnit);
                        this.dbServer.AddInParameter(command3, "FoodCal", DbType.String, lvo.FoodCal);
                        this.dbServer.AddInParameter(command3, "FoodCH", DbType.String, lvo.FoodCH);
                        this.dbServer.AddInParameter(command3, "FoodFat", DbType.String, lvo.FoodFat);
                        this.dbServer.AddInParameter(command3, "FoodExpectedCal", DbType.String, lvo.FoodExpectedCal);
                        this.dbServer.AddInParameter(command3, "FoodInstruction", DbType.String, lvo.FoodInstruction);
                        this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, lvo.ID);
                        this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
                        lvo.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                    }
                }
                transaction.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.DietPlan = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return BizActionObj;
        }

        private clsAddPatientLinkFileBizActionVO UpdateLinkfile(clsAddPatientLinkFileBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                if ((BizActionObj.PatientDetails != null) && (BizActionObj.PatientDetails.Count > 0))
                {
                    foreach (clsPatientLinkFileBizActionVO nvo in BizActionObj.PatientDetails)
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeletePatientLinkFile");
                        this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                        this.dbServer.ExecuteNonQuery(storedProcCommand);
                    }
                }
                foreach (clsPatientLinkFileBizActionVO nvo2 in BizActionObj.PatientDetails)
                {
                    nvo2.ID = 0L;
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatientLinkFile");
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo2.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo2.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo2.VisitID);
                    this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo2.Date);
                    this.dbServer.AddInParameter(storedProcCommand, "SourceURL", DbType.String, nvo2.SourceURL);
                    this.dbServer.AddInParameter(storedProcCommand, "Report", DbType.Binary, nvo2.Report);
                    this.dbServer.AddInParameter(storedProcCommand, "DocumentName", DbType.String, nvo2.DocumentName);
                    this.dbServer.AddInParameter(storedProcCommand, "Notes", DbType.String, nvo2.Notes);
                    this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, nvo2.Remarks);
                    this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, nvo2.Time);
                    this.dbServer.AddInParameter(storedProcCommand, "ReferredBy", DbType.String, nvo2.ReferredBy);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo2.ID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    nvo2.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        private clsAddPatientBizActionVO UpdatePatientDetails(clsAddPatientBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                clsPatientVO patientDetails = BizActionObj.PatientDetails;
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePatient");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, patientDetails.GeneralDetails.LinkServer);
                if (patientDetails.GeneralDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, patientDetails.GeneralDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "PatientCategoryID", DbType.Int64, patientDetails.GeneralDetails.PatientTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "ReferralTypeID", DbType.Int64, patientDetails.GeneralDetails.ReferralTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "ReferralDoctorID", DbType.Int64, patientDetails.GeneralDetails.ReferralDoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "ReferredToDoctorID", DbType.Int64, patientDetails.GeneralDetails.ReferredToDoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "ReferralDetail", DbType.String, patientDetails.GeneralDetails.ReferralDetail);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyName", DbType.String, Security.base64Encode(patientDetails.CompanyName));
                this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, patientDetails.GeneralDetails.MRNo);
                this.dbServer.AddInParameter(storedProcCommand, "RegistrationDate", DbType.DateTime, patientDetails.GeneralDetails.RegistrationDate);
                if (patientDetails.GeneralDetails.LastName != null)
                {
                    patientDetails.GeneralDetails.LastName = patientDetails.GeneralDetails.LastName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.LastName));
                if (patientDetails.GeneralDetails.FirstName != null)
                {
                    patientDetails.GeneralDetails.FirstName = patientDetails.GeneralDetails.FirstName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.FirstName));
                if (patientDetails.GeneralDetails.MiddleName != null)
                {
                    patientDetails.GeneralDetails.MiddleName = patientDetails.GeneralDetails.MiddleName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.MiddleName));
                if (patientDetails.FamilyName != null)
                {
                    patientDetails.FamilyName = patientDetails.FamilyName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "FamilyName", DbType.String, Security.base64Encode(patientDetails.FamilyName));
                this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, patientDetails.GenderID);
                this.dbServer.AddInParameter(storedProcCommand, "DateOfBirth", DbType.DateTime, patientDetails.GeneralDetails.DateOfBirth);
                this.dbServer.AddInParameter(storedProcCommand, "BloodGroupID", DbType.Int64, patientDetails.BloodGroupID);
                this.dbServer.AddInParameter(storedProcCommand, "MaritalStatusID", DbType.Int64, patientDetails.MaritalStatusID);
                this.dbServer.AddInParameter(storedProcCommand, "CivilID", DbType.String, Security.base64Encode(patientDetails.CivilID));
                if (patientDetails.ContactNo1 != null)
                {
                    patientDetails.ContactNo1 = patientDetails.ContactNo1.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo1", DbType.String, patientDetails.ContactNo1);
                if (patientDetails.ContactNo2 != null)
                {
                    patientDetails.ContactNo2 = patientDetails.ContactNo2.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo2", DbType.String, patientDetails.ContactNo2);
                if (patientDetails.FaxNo != null)
                {
                    patientDetails.FaxNo = patientDetails.FaxNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "FaxNo", DbType.String, patientDetails.FaxNo);
                if (patientDetails.Email != null)
                {
                    patientDetails.Email = patientDetails.Email.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Email", DbType.String, Security.base64Encode(patientDetails.Email));
                if (patientDetails.AddressLine1 != null)
                {
                    patientDetails.AddressLine1 = patientDetails.AddressLine1.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine1", DbType.String, Security.base64Encode(patientDetails.AddressLine1));
                if (patientDetails.AddressLine2 != null)
                {
                    patientDetails.AddressLine2 = patientDetails.AddressLine2.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine2", DbType.String, Security.base64Encode(patientDetails.AddressLine2));
                if (patientDetails.AddressLine3 != null)
                {
                    patientDetails.AddressLine3 = patientDetails.AddressLine3.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine3", DbType.String, Security.base64Encode(patientDetails.AddressLine3));
                if (patientDetails.OldRegistrationNo != null)
                {
                    patientDetails.OldRegistrationNo = patientDetails.OldRegistrationNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "OldRegistrationNo", DbType.String, patientDetails.OldRegistrationNo);
                this.dbServer.AddInParameter(storedProcCommand, "Country", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "State", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "City", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "Taluka", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "Area", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "District", DbType.String, null);
                if (patientDetails.CountryID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CountryID", DbType.Int64, patientDetails.CountryID);
                }
                if (patientDetails.StateID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "StateID", DbType.Int64, patientDetails.StateID);
                }
                if (patientDetails.CityID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CityID", DbType.Int64, patientDetails.CityID);
                }
                if (patientDetails.RegionID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "RegionID", DbType.Int64, patientDetails.RegionID);
                }
                this.dbServer.AddInParameter(storedProcCommand, "MobileCountryCode", DbType.String, patientDetails.MobileCountryCode);
                this.dbServer.AddInParameter(storedProcCommand, "ResiNoCountryCode", DbType.Int64, patientDetails.ResiNoCountryCode);
                this.dbServer.AddInParameter(storedProcCommand, "ResiSTDCode", DbType.Int64, patientDetails.ResiSTDCode);
                if (patientDetails.Pincode != null)
                {
                    patientDetails.Pincode = patientDetails.Pincode.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Pincode", DbType.String, Security.base64Encode(patientDetails.Pincode));
                this.dbServer.AddInParameter(storedProcCommand, "ReligionID", DbType.Int64, patientDetails.ReligionID);
                this.dbServer.AddInParameter(storedProcCommand, "OccupationId", DbType.Int64, patientDetails.OccupationId);
                this.dbServer.AddInParameter(storedProcCommand, "AgentID", DbType.Int64, patientDetails.AgentID);
                this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, patientDetails.AgencyID);
                this.dbServer.AddInParameter(storedProcCommand, "Photo", DbType.Binary, patientDetails.Photo);
                this.dbServer.AddInParameter(storedProcCommand, "IsLoyaltyMember", DbType.Boolean, patientDetails.IsLoyaltyMember);
                this.dbServer.AddInParameter(storedProcCommand, "LoyaltyCardID", DbType.Int64, patientDetails.LoyaltyCardID);
                this.dbServer.AddInParameter(storedProcCommand, "IssueDate", DbType.DateTime, patientDetails.IssueDate);
                this.dbServer.AddInParameter(storedProcCommand, "EffectiveDate", DbType.DateTime, patientDetails.EffectiveDate);
                this.dbServer.AddInParameter(storedProcCommand, "ExpiryDate", DbType.DateTime, patientDetails.ExpiryDate);
                if (patientDetails.LoyaltyCardNo != null)
                {
                    patientDetails.LoyaltyCardNo = patientDetails.LoyaltyCardNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "LoyaltyCardNo", DbType.String, patientDetails.LoyaltyCardNo);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, patientDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, patientDetails.GeneralDetails.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, patientDetails.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, patientDetails.GeneralDetails.PatientID);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                this.dbServer.AddParameter(storedProcCommand, "ImagePath", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "000000000000000000000000000000000000000000000000000");
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.PatientDetails.ImageName = (string) this.dbServer.GetParameterValue(storedProcCommand, "ImagePath");
                DemoImage image = new DemoImage();
                if (patientDetails.Photo != null)
                {
                    image.VaryQualityLevel(patientDetails.Photo, BizActionObj.PatientDetails.ImageName, this.ImgSaveLocation);
                }
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw exception;
            }
            finally
            {
                transaction.Dispose();
                transaction = null;
            }
            return BizActionObj;
        }

        private clsAddPatientBizActionVO UpdatePatientDetailsOPDWithTransaction(clsAddPatientBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                clsPatientVO patientDetails = BizActionObj.PatientDetails;
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePatient");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, patientDetails.GeneralDetails.LinkServer);
                if (patientDetails.GeneralDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, patientDetails.GeneralDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "PatientCategoryID", DbType.Int64, patientDetails.GeneralDetails.PatientTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "ReferralTypeID", DbType.Int64, patientDetails.GeneralDetails.ReferralTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "ReferralDoctorID", DbType.Int64, patientDetails.GeneralDetails.ReferralDoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "ReferredToDoctorID", DbType.Int64, patientDetails.GeneralDetails.ReferredToDoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "IsReferralDoc", DbType.Boolean, patientDetails.GeneralDetails.IsReferralDoc);
                this.dbServer.AddInParameter(storedProcCommand, "ReferralDetail", DbType.String, patientDetails.GeneralDetails.ReferralDetail);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyName", DbType.String, Security.base64Encode(patientDetails.CompanyName));
                this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, patientDetails.GeneralDetails.MRNo);
                this.dbServer.AddInParameter(storedProcCommand, "RegistrationDate", DbType.DateTime, patientDetails.GeneralDetails.RegistrationDate);
                if (patientDetails.GeneralDetails.LastName != null)
                {
                    patientDetails.GeneralDetails.LastName = patientDetails.GeneralDetails.LastName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.LastName));
                if (patientDetails.GeneralDetails.FirstName != null)
                {
                    patientDetails.GeneralDetails.FirstName = patientDetails.GeneralDetails.FirstName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.FirstName));
                if (patientDetails.GeneralDetails.MiddleName != null)
                {
                    patientDetails.GeneralDetails.MiddleName = patientDetails.GeneralDetails.MiddleName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.MiddleName));
                if (patientDetails.FamilyName != null)
                {
                    patientDetails.FamilyName = patientDetails.FamilyName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "FamilyName", DbType.String, Security.base64Encode(patientDetails.FamilyName));
                this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, patientDetails.GenderID);
                this.dbServer.AddInParameter(storedProcCommand, "DateOfBirth", DbType.DateTime, patientDetails.GeneralDetails.DateOfBirth);
                this.dbServer.AddInParameter(storedProcCommand, "IsAge", DbType.Boolean, patientDetails.GeneralDetails.IsAge);
                this.dbServer.AddInParameter(storedProcCommand, "BloodGroupID", DbType.Int64, patientDetails.BloodGroupID);
                this.dbServer.AddInParameter(storedProcCommand, "MaritalStatusID", DbType.Int64, patientDetails.MaritalStatusID);
                this.dbServer.AddInParameter(storedProcCommand, "CivilID", DbType.String, Security.base64Encode(patientDetails.CivilID));
                if (patientDetails.ContactNo1 != null)
                {
                    patientDetails.ContactNo1 = patientDetails.ContactNo1.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo1", DbType.String, patientDetails.ContactNo1);
                if (patientDetails.ContactNo2 != null)
                {
                    patientDetails.ContactNo2 = patientDetails.ContactNo2.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo2", DbType.String, patientDetails.ContactNo2);
                if (patientDetails.FaxNo != null)
                {
                    patientDetails.FaxNo = patientDetails.FaxNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "FaxNo", DbType.String, patientDetails.FaxNo);
                if (patientDetails.Email != null)
                {
                    patientDetails.Email = patientDetails.Email.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Email", DbType.String, Security.base64Encode(patientDetails.Email));
                if (patientDetails.AddressLine1 != null)
                {
                    patientDetails.AddressLine1 = patientDetails.AddressLine1.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine1", DbType.String, Security.base64Encode(patientDetails.AddressLine1));
                if (patientDetails.AddressLine2 != null)
                {
                    patientDetails.AddressLine2 = patientDetails.AddressLine2.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine2", DbType.String, Security.base64Encode(patientDetails.AddressLine2));
                if (patientDetails.AddressLine3 != null)
                {
                    patientDetails.AddressLine3 = patientDetails.AddressLine3.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine3", DbType.String, Security.base64Encode(patientDetails.AddressLine3));
                if (patientDetails.BDID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "BDID", DbType.Int64, patientDetails.BDID);
                }
                if (patientDetails.OldRegistrationNo != null)
                {
                    patientDetails.OldRegistrationNo = patientDetails.OldRegistrationNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "OldRegistrationNo", DbType.String, patientDetails.OldRegistrationNo);
                this.dbServer.AddInParameter(storedProcCommand, "Country", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "State", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "City", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "Taluka", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "Area", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "District", DbType.String, null);
                if (patientDetails.CountryID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CountryID", DbType.Int64, patientDetails.CountryID);
                }
                if (patientDetails.StateID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "StateID", DbType.Int64, patientDetails.StateID);
                }
                if (patientDetails.CityID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CityID", DbType.Int64, patientDetails.CityID);
                }
                if (patientDetails.RegionID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "RegionID", DbType.Int64, patientDetails.RegionID);
                }
                if (patientDetails.PrefixId > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "PrefixId", DbType.Int64, patientDetails.PrefixId);
                }
                if (patientDetails.IdentityID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IdentityID", DbType.Int64, patientDetails.IdentityID);
                }
                this.dbServer.AddInParameter(storedProcCommand, "IdentityNumber", DbType.String, patientDetails.IdentityNumber);
                this.dbServer.AddInParameter(storedProcCommand, "RemarkForPatientType", DbType.String, patientDetails.RemarkForPatientType);
                this.dbServer.AddInParameter(storedProcCommand, "IsInternationalPatient", DbType.Boolean, patientDetails.IsInternationalPatient);
                this.dbServer.AddInParameter(storedProcCommand, "Education", DbType.String, patientDetails.Education);
                this.dbServer.AddInParameter(storedProcCommand, "MobileCountryCode", DbType.String, patientDetails.MobileCountryCode);
                this.dbServer.AddInParameter(storedProcCommand, "ResiNoCountryCode", DbType.Int64, patientDetails.ResiNoCountryCode);
                this.dbServer.AddInParameter(storedProcCommand, "ResiSTDCode", DbType.Int64, patientDetails.ResiSTDCode);
                if (patientDetails.Pincode != null)
                {
                    patientDetails.Pincode = patientDetails.Pincode.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Pincode", DbType.String, Security.base64Encode(patientDetails.Pincode));
                this.dbServer.AddInParameter(storedProcCommand, "ReligionID", DbType.Int64, patientDetails.ReligionID);
                this.dbServer.AddInParameter(storedProcCommand, "OccupationId", DbType.Int64, patientDetails.OccupationId);
                this.dbServer.AddInParameter(storedProcCommand, "AgentID", DbType.Int64, patientDetails.AgentID);
                this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, patientDetails.AgencyID);
                this.dbServer.AddInParameter(storedProcCommand, "Photo", DbType.Binary, patientDetails.Photo);
                this.dbServer.AddInParameter(storedProcCommand, "IsLoyaltyMember", DbType.Boolean, patientDetails.IsLoyaltyMember);
                this.dbServer.AddInParameter(storedProcCommand, "LoyaltyCardID", DbType.Int64, patientDetails.LoyaltyCardID);
                this.dbServer.AddInParameter(storedProcCommand, "IssueDate", DbType.DateTime, patientDetails.IssueDate);
                this.dbServer.AddInParameter(storedProcCommand, "EffectiveDate", DbType.DateTime, patientDetails.EffectiveDate);
                this.dbServer.AddInParameter(storedProcCommand, "ExpiryDate", DbType.DateTime, patientDetails.ExpiryDate);
                if (patientDetails.LoyaltyCardNo != null)
                {
                    patientDetails.LoyaltyCardNo = patientDetails.LoyaltyCardNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "LoyaltyCardNo", DbType.String, patientDetails.LoyaltyCardNo);
                if (patientDetails.GeneralDetails.SonDaughterOf != null)
                {
                    patientDetails.GeneralDetails.SonDaughterOf = patientDetails.GeneralDetails.SonDaughterOf.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "DaughterOf", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.SonDaughterOf));
                this.dbServer.AddInParameter(storedProcCommand, "NationalityID", DbType.Int64, patientDetails.NationalityID);
                this.dbServer.AddInParameter(storedProcCommand, "PrefLangID", DbType.Int64, patientDetails.PreferredLangID);
                this.dbServer.AddInParameter(storedProcCommand, "TreatRequiredID", DbType.Int64, patientDetails.TreatRequiredID);
                this.dbServer.AddInParameter(storedProcCommand, "EducationID", DbType.Int64, patientDetails.EducationID);
                this.dbServer.AddInParameter(storedProcCommand, "MarriageAnnivDate", DbType.DateTime, patientDetails.MarriageAnnDate);
                this.dbServer.AddInParameter(storedProcCommand, "NoOfPeople", DbType.Int32, patientDetails.GeneralDetails.NoOfPeople);
                this.dbServer.AddInParameter(storedProcCommand, "IsClinicVisited", DbType.Boolean, patientDetails.IsClinicVisited);
                this.dbServer.AddInParameter(storedProcCommand, "ClinicName", DbType.String, patientDetails.GeneralDetails.ClinicName);
                this.dbServer.AddInParameter(storedProcCommand, "SpecialRegID", DbType.Int64, patientDetails.SpecialRegID);
                if ((BizActionObj.PatientDetails != null) && (BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 13))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "BabyNo", DbType.Int32, patientDetails.BabyNo);
                    this.dbServer.AddInParameter(storedProcCommand, "BabyOfNo", DbType.Int32, patientDetails.BabyOfNo);
                    this.dbServer.AddInParameter(storedProcCommand, "BabyWeight", DbType.String, patientDetails.BabyWeight);
                    this.dbServer.AddInParameter(storedProcCommand, "LinkPatientID", DbType.Int64, patientDetails.LinkPatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "LinkPatientUnitID", DbType.Int64, patientDetails.LinkPatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "LinkPatientMrNo", DbType.String, patientDetails.LinkPatientMrNo);
                }
                if (patientDetails.PanNumber != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "PanNumber", DbType.String, patientDetails.PanNumber);
                }
                this.dbServer.AddInParameter(storedProcCommand, "FamilyTypeID", DbType.Int64, patientDetails.FamilyTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "NoOfYearsOfMarriage", DbType.Int64, patientDetails.NoOfYearsOfMarriage);
                this.dbServer.AddInParameter(storedProcCommand, "NoOfExistingChildren", DbType.Int64, patientDetails.NoOfExistingChildren);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, patientDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, patientDetails.GeneralDetails.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, patientDetails.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, patientDetails.GeneralDetails.PatientID);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                this.dbServer.AddParameter(storedProcCommand, "ImagePath", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "000000000000000000000000000000000000000000000000000");
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.PatientDetails.ImageName = Convert.ToString(this.dbServer.GetParameterValue(storedProcCommand, "ImagePath"));
                DemoImage image = new DemoImage();
                if (patientDetails.Photo != null)
                {
                    image.VaryQualityLevel(patientDetails.Photo, BizActionObj.PatientDetails.ImageName, this.ImgSaveLocation);
                }
                if (BizActionObj.BankDetails != null)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientBankDetails");
                    this.dbServer.AddInParameter(command2, "PatientID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.PatientID);
                    this.dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "BankID", DbType.Int64, BizActionObj.BankDetails.BankId);
                    this.dbServer.AddInParameter(command2, "BranchID", DbType.Int64, BizActionObj.BankDetails.BranchId);
                    this.dbServer.AddInParameter(command2, "IFSCCode", DbType.String, BizActionObj.BankDetails.IFSCCode);
                    this.dbServer.AddInParameter(command2, "AccountType", DbType.Boolean, BizActionObj.BankDetails.AccountTypeId);
                    this.dbServer.AddInParameter(command2, "AccountNo", DbType.String, BizActionObj.BankDetails.AccountNumber);
                    this.dbServer.AddInParameter(command2, "AccountHolderName", DbType.String, BizActionObj.BankDetails.AccountHolderName);
                    this.dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, patientDetails.AddedDateTime);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientDetails.ID);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                }
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw exception;
            }
            finally
            {
                transaction.Dispose();
                transaction = null;
            }
            return BizActionObj;
        }

        public override IValueObject UpdatePatientForLoyaltyCardIssue(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            clsUpdatePatientForIssueBizActionVO nvo = (clsUpdatePatientForIssueBizActionVO) valueObject;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsPatientVO patientDetails = nvo.PatientDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatientForIssue");
                this.dbServer.AddInParameter(storedProcCommand, "LoyaltyCardNo", DbType.String, patientDetails.LoyaltyCardNo);
                this.dbServer.AddInParameter(storedProcCommand, "PreferNameOnLoyaltyCard", DbType.String, patientDetails.PreferNameonLoyaltyCard);
                this.dbServer.AddInParameter(storedProcCommand, "IssueDate", DbType.DateTime, patientDetails.IssueDate);
                this.dbServer.AddInParameter(storedProcCommand, "ExpiryDate", DbType.DateTime, patientDetails.ExpiryDate);
                this.dbServer.AddInParameter(storedProcCommand, "EffectiveDate", DbType.DateTime, patientDetails.EffectiveDate);
                this.dbServer.AddInParameter(storedProcCommand, "LoyaltyCardID", DbType.Int64, patientDetails.LoyaltyCardID);
                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, patientDetails.TariffID);
                this.dbServer.AddInParameter(storedProcCommand, "Remark ", DbType.String, Security.base64Encode(patientDetails.Remark).Trim());
                this.dbServer.AddInParameter(storedProcCommand, "IsLoyaltyMember ", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, patientDetails.GeneralDetails.PatientID);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                foreach (clsPatientFamilyDetailsVO svo in patientDetails.FamilyDetails)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddPatientFamilyDetailsForIssue");
                    this.dbServer.AddInParameter(command2, "PatientID", DbType.Int64, svo.PatientID);
                    this.dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, svo.PatientUnitID);
                    if (svo.FirstName != null)
                    {
                        svo.FirstName = svo.FirstName.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "FirstName", DbType.String, Security.base64Encode(svo.FirstName));
                    if (svo.MiddleName != null)
                    {
                        svo.MiddleName = svo.MiddleName.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "MiddleName", DbType.String, Security.base64Encode(svo.MiddleName));
                    if (svo.LastName != null)
                    {
                        svo.LastName = svo.LastName.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "LastName", DbType.String, Security.base64Encode(svo.LastName));
                    this.dbServer.AddInParameter(command2, "DOB", DbType.DateTime, svo.DateOfBirth);
                    this.dbServer.AddInParameter(command2, "RelationID", DbType.Int64, svo.RelationID);
                    this.dbServer.AddInParameter(command2, "TariffID", DbType.Int64, svo.TariffID);
                    this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, svo.Status);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    svo.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                }
                if (patientDetails.OtherDetails != null)
                {
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddPatientOtherDetails");
                    this.dbServer.AddInParameter(command3, "PatientID", DbType.Int64, patientDetails.OtherDetails.PatientID);
                    this.dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, patientDetails.OtherDetails.PatientUnitID);
                    this.dbServer.AddInParameter(command3, "Q1", DbType.Boolean, patientDetails.OtherDetails.Question1);
                    this.dbServer.AddInParameter(command3, "Q2", DbType.Boolean, patientDetails.OtherDetails.Question2);
                    this.dbServer.AddInParameter(command3, "Q3", DbType.Boolean, patientDetails.OtherDetails.Question3);
                    this.dbServer.AddInParameter(command3, "Q4", DbType.Boolean, patientDetails.OtherDetails.Question4);
                    this.dbServer.AddInParameter(command3, "Q4Y", DbType.String, patientDetails.OtherDetails.Question4Details);
                    this.dbServer.AddInParameter(command3, "Q5A", DbType.Boolean, patientDetails.OtherDetails.Question5A);
                    this.dbServer.AddInParameter(command3, "Q5B", DbType.Boolean, patientDetails.OtherDetails.Question5B);
                    this.dbServer.AddInParameter(command3, "Q5C", DbType.Boolean, patientDetails.OtherDetails.Question5C);
                    this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, patientDetails.OtherDetails.Status);
                    this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientDetails.OtherDetails.ID);
                    this.dbServer.ExecuteNonQuery(command3, transaction);
                    patientDetails.OtherDetails.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                }
                foreach (clsPatientServiceDetails details in patientDetails.ServiceDetails)
                {
                    DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_AddPatientServiceDetails");
                    this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command4, "PatientID", DbType.Int64, details.PatientID);
                    this.dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, details.PatientUnitID);
                    this.dbServer.AddInParameter(command4, "RelationID", DbType.Int64, details.RelationID);
                    this.dbServer.AddInParameter(command4, "LoyaltyID", DbType.Int64, patientDetails.LoyaltyCardID);
                    this.dbServer.AddInParameter(command4, "TariffID", DbType.Int64, details.TariffID);
                    this.dbServer.AddInParameter(command4, "ServiceID", DbType.Int64, details.ServiceID);
                    this.dbServer.AddInParameter(command4, "Rate", DbType.Double, details.Rate);
                    this.dbServer.AddInParameter(command4, "ConcessionPercentage", DbType.Double, details.ConcessionPercentage);
                    this.dbServer.AddInParameter(command4, "ConcessionAmount", DbType.Double, details.ConcessionAmount);
                    this.dbServer.AddInParameter(command4, "NetAmount", DbType.Double, details.NetAmount);
                    this.dbServer.AddInParameter(command4, "Status", DbType.Double, details.SelectService);
                    this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, details.ID);
                    this.dbServer.ExecuteNonQuery(command4, transaction);
                    details.ID = (long) this.dbServer.GetParameterValue(command4, "ID");
                }
                transaction.Commit();
                nvo.SuccessStatus = 0;
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                transaction.Rollback();
                nvo.PatientDetails = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return nvo;
        }

        private clsAddPatientBizActionVO UpdatePatientPanNumber(clsAddPatientBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                clsPatientVO patientDetails = BizActionObj.PatientDetails;
                connection.Open();
                transaction = connection.BeginTransaction();
                if (BizActionObj.PatientDetails.IsPanNoSave)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePatientPanNumber");
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, patientDetails.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PanNumber", DbType.String, patientDetails.PanNumber);
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, patientDetails.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, patientDetails.UpdatedDateTime);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    transaction.Commit();
                }
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw exception;
            }
            finally
            {
                transaction.Dispose();
                transaction = null;
            }
            return BizActionObj;
        }

        private clsAddPatientBizActionVO UpdateSurrogateDetails(clsAddPatientBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                clsPatientVO patientDetails = BizActionObj.PatientDetails;
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePatient");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, patientDetails.GeneralDetails.LinkServer);
                if (patientDetails.GeneralDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, patientDetails.GeneralDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "PatientCategoryID", DbType.Int64, patientDetails.GeneralDetails.PatientTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "ReferralTypeID", DbType.Int64, patientDetails.GeneralDetails.ReferralTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyName", DbType.String, Security.base64Encode(patientDetails.CompanyName));
                this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, patientDetails.GeneralDetails.MRNo);
                this.dbServer.AddInParameter(storedProcCommand, "RegistrationDate", DbType.DateTime, patientDetails.GeneralDetails.RegistrationDate);
                if (patientDetails.GeneralDetails.LastName != null)
                {
                    patientDetails.GeneralDetails.LastName = patientDetails.GeneralDetails.LastName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.LastName));
                if (patientDetails.GeneralDetails.FirstName != null)
                {
                    patientDetails.GeneralDetails.FirstName = patientDetails.GeneralDetails.FirstName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.FirstName));
                if (patientDetails.GeneralDetails.MiddleName != null)
                {
                    patientDetails.GeneralDetails.MiddleName = patientDetails.GeneralDetails.MiddleName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, Security.base64Encode(patientDetails.GeneralDetails.MiddleName));
                if (patientDetails.FamilyName != null)
                {
                    patientDetails.FamilyName = patientDetails.FamilyName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "FamilyName", DbType.String, Security.base64Encode(patientDetails.FamilyName));
                this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, patientDetails.GenderID);
                this.dbServer.AddInParameter(storedProcCommand, "DateOfBirth", DbType.DateTime, patientDetails.GeneralDetails.DateOfBirth);
                this.dbServer.AddInParameter(storedProcCommand, "BloodGroupID", DbType.Int64, patientDetails.BloodGroupID);
                this.dbServer.AddInParameter(storedProcCommand, "MaritalStatusID", DbType.Int64, patientDetails.MaritalStatusID);
                this.dbServer.AddInParameter(storedProcCommand, "CivilID", DbType.String, Security.base64Encode(patientDetails.CivilID));
                if (patientDetails.ContactNo1 != null)
                {
                    patientDetails.ContactNo1 = patientDetails.ContactNo1.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo1", DbType.String, patientDetails.ContactNo1);
                if (patientDetails.ContactNo2 != null)
                {
                    patientDetails.ContactNo2 = patientDetails.ContactNo2.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo2", DbType.String, patientDetails.ContactNo2);
                if (patientDetails.FaxNo != null)
                {
                    patientDetails.FaxNo = patientDetails.FaxNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "FaxNo", DbType.String, patientDetails.FaxNo);
                if (patientDetails.Email != null)
                {
                    patientDetails.Email = patientDetails.Email.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Email", DbType.String, Security.base64Encode(patientDetails.Email));
                if (patientDetails.AddressLine1 != null)
                {
                    patientDetails.AddressLine1 = patientDetails.AddressLine1.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine1", DbType.String, Security.base64Encode(patientDetails.AddressLine1));
                if (patientDetails.AddressLine2 != null)
                {
                    patientDetails.AddressLine2 = patientDetails.AddressLine2.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine2", DbType.String, Security.base64Encode(patientDetails.AddressLine2));
                if (patientDetails.AddressLine3 != null)
                {
                    patientDetails.AddressLine3 = patientDetails.AddressLine3.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine3", DbType.String, Security.base64Encode(patientDetails.AddressLine3));
                this.dbServer.AddInParameter(storedProcCommand, "Country", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "State", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "City", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "Taluka", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "Area", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "District", DbType.String, null);
                if (patientDetails.CountryID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CountryID", DbType.Int64, patientDetails.CountryID);
                }
                if (patientDetails.StateID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "StateID", DbType.Int64, patientDetails.StateID);
                }
                if (patientDetails.CityID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CityID", DbType.Int64, patientDetails.CityID);
                }
                if (patientDetails.RegionID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "RegionID", DbType.Int64, patientDetails.RegionID);
                }
                this.dbServer.AddInParameter(storedProcCommand, "MobileCountryCode", DbType.String, patientDetails.MobileCountryCode);
                this.dbServer.AddInParameter(storedProcCommand, "ResiNoCountryCode", DbType.Int64, patientDetails.ResiNoCountryCode);
                this.dbServer.AddInParameter(storedProcCommand, "ResiSTDCode", DbType.Int64, patientDetails.ResiSTDCode);
                if (patientDetails.Pincode != null)
                {
                    patientDetails.Pincode = patientDetails.Pincode.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Pincode", DbType.String, Security.base64Encode(patientDetails.Pincode));
                this.dbServer.AddInParameter(storedProcCommand, "ReligionID", DbType.Int64, patientDetails.ReligionID);
                this.dbServer.AddInParameter(storedProcCommand, "OccupationId", DbType.Int64, patientDetails.OccupationId);
                this.dbServer.AddInParameter(storedProcCommand, "AgentID", DbType.Int64, patientDetails.AgentID);
                this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, patientDetails.AgencyID);
                if (patientDetails.OldRegistrationNo != null)
                {
                    patientDetails.OldRegistrationNo = patientDetails.OldRegistrationNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "OldRegistrationNo", DbType.String, patientDetails.OldRegistrationNo);
                this.dbServer.AddInParameter(storedProcCommand, "Photo", DbType.Binary, patientDetails.Photo);
                this.dbServer.AddInParameter(storedProcCommand, "IsLoyaltyMember", DbType.Boolean, patientDetails.IsLoyaltyMember);
                this.dbServer.AddInParameter(storedProcCommand, "LoyaltyCardID", DbType.Int64, patientDetails.LoyaltyCardID);
                this.dbServer.AddInParameter(storedProcCommand, "IssueDate", DbType.DateTime, patientDetails.IssueDate);
                this.dbServer.AddInParameter(storedProcCommand, "EffectiveDate", DbType.DateTime, patientDetails.EffectiveDate);
                this.dbServer.AddInParameter(storedProcCommand, "ExpiryDate", DbType.DateTime, patientDetails.ExpiryDate);
                if (patientDetails.LoyaltyCardNo != null)
                {
                    patientDetails.LoyaltyCardNo = patientDetails.LoyaltyCardNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "LoyaltyCardNo", DbType.String, patientDetails.LoyaltyCardNo);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, patientDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, patientDetails.GeneralDetails.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, patientDetails.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, patientDetails.GeneralDetails.PatientID);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                this.dbServer.AddParameter(storedProcCommand, "ImagePath", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "000000000000000000000000000000000000000000000000000");
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.PatientDetails.GeneralDetails.UnitId = UserVo.UserLoginInfo.UnitId;
                BizActionObj.PatientDetails.ImageName = (string) this.dbServer.GetParameterValue(storedProcCommand, "ImagePath");
                DemoImage image = new DemoImage();
                if (patientDetails.Photo != null)
                {
                    image.VaryQualityLevel(patientDetails.Photo, BizActionObj.PatientDetails.ImageName, this.ImgSaveLocation);
                }
                if ((BizActionObj.PatientDetails.SpouseDetails != null) && (BizActionObj.PatientDetails.GeneralDetails.PatientTypeID == 10))
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddSurrogateSpouseInformation");
                    this.dbServer.AddInParameter(command2, "SurrogateID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.PatientID);
                    this.dbServer.AddInParameter(command2, "SurrogateUnitID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.UnitId);
                    this.dbServer.AddInParameter(command2, "PatientCategoryID", DbType.Int64, patientDetails.GeneralDetails.PatientTypeID);
                    this.dbServer.AddInParameter(command2, "ReferralTypeID", DbType.Int64, patientDetails.GeneralDetails.ReferralTypeID);
                    this.dbServer.AddInParameter(command2, "RegistrationDate", DbType.DateTime, patientDetails.GeneralDetails.RegistrationDate);
                    if (patientDetails.SpouseDetails.LastName != null)
                    {
                        patientDetails.SpouseDetails.LastName = patientDetails.SpouseDetails.LastName.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "LastName", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.LastName));
                    if (patientDetails.SpouseDetails.FirstName != null)
                    {
                        patientDetails.SpouseDetails.FirstName = patientDetails.SpouseDetails.FirstName.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "FirstName", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.FirstName));
                    if (patientDetails.SpouseDetails.MiddleName != null)
                    {
                        patientDetails.SpouseDetails.MiddleName = patientDetails.SpouseDetails.MiddleName.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "MiddleName", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.MiddleName));
                    if (patientDetails.SpouseDetails.FamilyName != null)
                    {
                        patientDetails.SpouseDetails.FamilyName = patientDetails.SpouseDetails.FamilyName.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "FamilyName", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.FamilyName));
                    this.dbServer.AddInParameter(command2, "GenderID", DbType.Int64, patientDetails.SpouseDetails.GenderID);
                    this.dbServer.AddInParameter(command2, "DateOfBirth", DbType.DateTime, patientDetails.SpouseDetails.DateOfBirth);
                    this.dbServer.AddInParameter(command2, "BloodGroupID", DbType.Int64, patientDetails.SpouseDetails.BloodGroupID);
                    this.dbServer.AddInParameter(command2, "MaritalStatusID", DbType.Int64, patientDetails.SpouseDetails.MaritalStatusID);
                    this.dbServer.AddInParameter(command2, "CivilID", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.CivilID));
                    if (patientDetails.SpouseDetails.ContactNo1 != null)
                    {
                        patientDetails.SpouseDetails.ContactNo1 = patientDetails.SpouseDetails.ContactNo1.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "ContactNo1", DbType.String, patientDetails.SpouseDetails.ContactNo1);
                    if (patientDetails.SpouseDetails.ContactNo2 != null)
                    {
                        patientDetails.SpouseDetails.ContactNo2 = patientDetails.SpouseDetails.ContactNo2.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "ContactNo2", DbType.String, patientDetails.SpouseDetails.ContactNo2);
                    if (patientDetails.SpouseDetails.FaxNo != null)
                    {
                        patientDetails.SpouseDetails.FaxNo = patientDetails.SpouseDetails.FaxNo.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "FaxNo", DbType.String, patientDetails.SpouseDetails.FaxNo);
                    if (patientDetails.SpouseDetails.Email != null)
                    {
                        patientDetails.SpouseDetails.Email = patientDetails.SpouseDetails.Email.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "Email", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.Email));
                    if (patientDetails.SpouseDetails.AddressLine1 != null)
                    {
                        patientDetails.SpouseDetails.AddressLine1 = patientDetails.SpouseDetails.AddressLine1.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "AddressLine1", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.AddressLine1));
                    if (patientDetails.SpouseDetails.AddressLine2 != null)
                    {
                        patientDetails.SpouseDetails.AddressLine2 = patientDetails.SpouseDetails.AddressLine2.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "AddressLine2", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.AddressLine2));
                    if (patientDetails.SpouseDetails.AddressLine3 != null)
                    {
                        patientDetails.SpouseDetails.AddressLine3 = patientDetails.SpouseDetails.AddressLine3.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "AddressLine3", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.AddressLine3));
                    if (patientDetails.SpouseDetails.Country != null)
                    {
                        patientDetails.SpouseDetails.Country = patientDetails.SpouseDetails.Country.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "Country", DbType.String, null);
                    this.dbServer.AddInParameter(command2, "State", DbType.String, null);
                    this.dbServer.AddInParameter(command2, "City", DbType.String, null);
                    this.dbServer.AddInParameter(command2, "Taluka", DbType.String, null);
                    this.dbServer.AddInParameter(command2, "Area", DbType.String, null);
                    this.dbServer.AddInParameter(command2, "District", DbType.String, null);
                    if (patientDetails.SpouseDetails.CountryID > 0L)
                    {
                        this.dbServer.AddInParameter(command2, "CountryID", DbType.Int64, patientDetails.SpouseDetails.CountryID);
                    }
                    if (patientDetails.SpouseDetails.StateID > 0L)
                    {
                        this.dbServer.AddInParameter(command2, "StateID", DbType.Int64, patientDetails.SpouseDetails.StateID);
                    }
                    if (patientDetails.SpouseDetails.CityID > 0L)
                    {
                        this.dbServer.AddInParameter(command2, "CityID", DbType.Int64, patientDetails.SpouseDetails.CityID);
                    }
                    if (patientDetails.SpouseDetails.RegionID > 0L)
                    {
                        this.dbServer.AddInParameter(command2, "RegionID", DbType.Int64, patientDetails.SpouseDetails.RegionID);
                    }
                    this.dbServer.AddInParameter(command2, "MobileCountryCode", DbType.String, patientDetails.SpouseDetails.MobileCountryCode);
                    this.dbServer.AddInParameter(command2, "ResiNoCountryCode", DbType.Int64, patientDetails.SpouseDetails.ResiNoCountryCode);
                    this.dbServer.AddInParameter(command2, "ResiSTDCode", DbType.Int64, patientDetails.SpouseDetails.ResiSTDCode);
                    if (patientDetails.SpouseDetails.Pincode != null)
                    {
                        patientDetails.Pincode = patientDetails.SpouseDetails.Pincode.Trim();
                    }
                    this.dbServer.AddInParameter(command2, "Pincode", DbType.String, Security.base64Encode(patientDetails.SpouseDetails.Pincode));
                    this.dbServer.AddInParameter(command2, "CompanyName", DbType.String, Security.base64Encode(patientDetails.CompanyName));
                    this.dbServer.AddInParameter(command2, "ReligionID", DbType.Int64, patientDetails.SpouseDetails.ReligionID);
                    this.dbServer.AddInParameter(command2, "OccupationId", DbType.Int64, patientDetails.SpouseDetails.OccupationId);
                    this.dbServer.AddInParameter(command2, "Photo", DbType.Binary, patientDetails.SpouseDetails.Photo);
                    this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, patientDetails.Status);
                    this.dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    patientDetails.SpouseDetails.MRNo = patientDetails.GeneralDetails.MRNo.Remove(patientDetails.GeneralDetails.MRNo.Length - 1, 1);
                    this.dbServer.AddParameter(command2, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                    this.dbServer.AddInParameter(command2, "ID", DbType.Int64, patientDetails.SpouseDetails.PatientID);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                    string parameterValue = (string) this.dbServer.GetParameterValue(command2, "Err");
                }
                DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddSurrogateOtherInformation");
                this.dbServer.AddInParameter(command3, "SurrogateID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.PatientID);
                this.dbServer.AddInParameter(command3, "SurrogateUnitID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.UnitId);
                this.dbServer.AddInParameter(command3, "ReferralTypeID", DbType.Int64, patientDetails.GeneralDetails.ReferralTypeID);
                this.dbServer.AddInParameter(command3, "AgencyID", DbType.String, patientDetails.AgencyID);
                this.dbServer.AddInParameter(command3, "SurrogateOtherDetails", DbType.String, patientDetails.SurrogateOtherDetails);
                this.dbServer.AddInParameter(command3, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command3, "ID", DbType.Int64, 0);
                this.dbServer.AddInParameter(command3, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(command3, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw exception;
            }
            finally
            {
                transaction.Dispose();
                transaction = null;
            }
            return BizActionObj;
        }
    }
}

