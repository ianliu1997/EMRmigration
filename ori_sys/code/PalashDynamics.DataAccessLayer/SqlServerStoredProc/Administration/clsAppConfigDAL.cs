using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration;
using System.Data.Common;
using System.Data;
using com.seedhealthcare.hms.CustomExceptions;
using System.Reflection;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsAppConfigDAL : clsBaseAppConfigDAL
    {

        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;

        //added by neena
        bool IsIVFBillingCriteria = false;
        string ForIVFProcedureBilling, ForTriggerProcedureBilling, ForOPUProcedureBilling;
        //
        bool IsConcessionReadOnly = false;  //***//
        bool IsFertilityPoint = false;//***//
        bool ValidationsFlag = false; //Added by NileshD on 19April2019
        long InternationalId = 0; //Added by NileshD on 19April2019
        int EMRModVisitDateInDays = 0;
        #endregion

        private clsAppConfigDAL()
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

                //added by neena
                IsIVFBillingCriteria = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsIVFBillingCriteria"]);
                ForIVFProcedureBilling = (System.Configuration.ConfigurationManager.AppSettings["ForIVFProcedureBilling"]).ToString();
                ForTriggerProcedureBilling = (System.Configuration.ConfigurationManager.AppSettings["ForTriggerProcedureBilling"]).ToString();
                ForOPUProcedureBilling = (System.Configuration.ConfigurationManager.AppSettings["ForOPUProcedureBilling"]).ToString();
                //
                IsConcessionReadOnly = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsConcessionReadOnly"]); //***//
                IsFertilityPoint = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsFertilityPoint"]); //***//
                ValidationsFlag = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["ValidationsFlag"]); //Added by NileshD on 19April2019
                InternationalId = Convert.ToInt64(System.Configuration.ConfigurationManager.AppSettings["InternationalId"]); //Added by NileshD on 19April2019
                this.EMRModVisitDateInDays = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EMRModVisitDateInDays"]);  //added by Ashish Z. on 02062017
                if (this.EMRModVisitDateInDays == 0)
                    this.EMRModVisitDateInDays = 1;
                #endregion

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public override IValueObject GetAppConfig(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetAppConfigBizActionVO BizActionObj = valueObject as clsGetAppConfigBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAppConfig");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

                DbDataReader reader;

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.AppConfig == null)
                        BizActionObj.AppConfig = new clsAppConfigVO();

                    while (reader.Read())
                    {

                        BizActionObj.AppConfig.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        BizActionObj.AppConfig.IsConfigured = (bool)DALHelper.HandleDBNull(reader["IsConfigured"]);
                        BizActionObj.AppConfig.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        BizActionObj.AppConfig.UnitName = (string)DALHelper.HandleDBNull(reader["Name"]);
                        BizActionObj.AppConfig.DoctorID = (long)DALHelper.HandleDBNull(reader["DoctorID"]);
                        BizActionObj.AppConfig.DepartmentID = (long)DALHelper.HandleDBNull(reader["DepartmentID"]);
                        BizActionObj.AppConfig.PatientCategoryID = (long)DALHelper.HandleDBNull(reader["PatientCategoryID"]);
                        BizActionObj.AppConfig.PharmacyPatientCategoryID = (long)DALHelper.HandleDBNull(reader["PharmacyPatientCategoryID"]);
                        BizActionObj.AppConfig.Country = (string)DALHelper.HandleDBNull(reader["Country"]);
                        BizActionObj.AppConfig.State = (string)DALHelper.HandleDBNull(reader["State"]);
                        BizActionObj.AppConfig.District = (string)DALHelper.HandleDBNull(reader["District"]);
                        BizActionObj.AppConfig.Pincode = (string)DALHelper.HandleDBNull(reader["Pincode"]);
                        BizActionObj.AppConfig.City = (string)DALHelper.HandleDBNull(reader["City"]);
                        BizActionObj.AppConfig.Area = (string)DALHelper.HandleDBNull(reader["Area"]);
                        BizActionObj.AppConfig.VisitTypeID = (long)DALHelper.HandleDBNull(reader["VisitTypeID"]);
                        BizActionObj.AppConfig.PatientSourceID = (long)DALHelper.HandleDBNull(reader["PatientSourceID"]);
                        BizActionObj.AppConfig.RadiologyStoreID = (long)DALHelper.HandleDBNull(reader["RadiologyStoreID"]);
                        BizActionObj.AppConfig.PathologyStoreID = (long)DALHelper.HandleDBNull(reader["PathologyStoreID"]);
                        BizActionObj.AppConfig.OTStoreID = (long)DALHelper.HandleDBNull(reader["OTStoreID"]);
                        BizActionObj.AppConfig.AppointmentSlot = (double)DALHelper.HandleDBNull(reader["AppointmentSlot"]);
                        BizActionObj.AppConfig.IsHO = (bool)DALHelper.HandleDBNull(reader["IsHO"]);
                        BizActionObj.AppConfig.Email = (string)DALHelper.HandleDBNull(reader["Email"]);
                        BizActionObj.AppConfig.NurseRoleID = (long)DALHelper.HandleDBNull(reader["NurseRoleID"]);
                        BizActionObj.AppConfig.AdminRoleID = (long)DALHelper.HandleDBNull(reader["AdminRoleID"]);
                        BizActionObj.AppConfig.DoctorRoleID = (long)DALHelper.HandleDBNull(reader["DoctorRoleID"]);
                        BizActionObj.AppConfig.ConftnMsgForAdd = (bool)DALHelper.HandleDBNull(reader["ConftnMsgForAdd"]);
                        BizActionObj.AppConfig.SearchPatientsInterval = (Int16?)DALHelper.HandleDBNull(reader["SearchPatientsInterval"]);
                        BizActionObj.AppConfig.PaymentModeID = (long)DALHelper.HandleDBNull(reader["PaymentModeID"]);
                        BizActionObj.AppConfig.PathoSpecializationID = (long)DALHelper.HandleDBNull(reader["PathoSpecializationID"]);
                        BizActionObj.AppConfig.TariffID = (long)DALHelper.HandleDBNull(reader["SelfTariffID"]);
                        BizActionObj.AppConfig.SelfCompanyID = (long)DALHelper.HandleDBNull(reader["SelfCompanyID"]);
                        BizActionObj.AppConfig.FreeFollowupDays = (Int16)DALHelper.HandleDBNull(reader["FreeFollowupDays"]);
                        BizActionObj.AppConfig.FreeFollowupVisitTypeID = (Int64)DALHelper.HandleDBNull(reader["FreeFollowupVisitTypeID"]);
                        BizActionObj.AppConfig.PharmacyVisitTypeID = (Int64)DALHelper.HandleDBNull(reader["PharmacyVisitTypeID"]);

                        //added by rohiini dated 11.4.16

                        BizActionObj.AppConfig.PathologyVisitTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathologyVisitTypeID"]));
                        BizActionObj.AppConfig.PathologyCompanyTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathologyCompanyTypeID"]));
                        //
                        BizActionObj.AppConfig.SelfRelationID = (Int64)DALHelper.HandleDBNull(reader["SelfRelationID"]);
                        BizActionObj.AppConfig.RadiologySpecializationID = (Int64)DALHelper.HandleDBNull(reader["RadiologySpecializationID"]);
                        BizActionObj.AppConfig.PrintFormatID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PrintFormatID"]));
                        BizActionObj.AppConfig.PharmacyStoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PharmacyStoreID"]));

                        BizActionObj.AppConfig.IsCentralPurchaseStore = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCentralPurchaseStore"]));
                        BizActionObj.AppConfig.IndentStoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentStoreID"]));

                        BizActionObj.AppConfig.RefundPayModeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RefundMode"]));
                        BizActionObj.AppConfig.RefundAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["RefundAmount"]));

                        BizActionObj.AppConfig.EmbryologistID = (Int64)DALHelper.HandleDBNull(reader["EmbryologistID"]);
                        BizActionObj.AppConfig.AnesthetistID = (Int64)DALHelper.HandleDBNull(reader["AnesthetistID"]);
                        BizActionObj.AppConfig.RadiologistID = (Int64)DALHelper.HandleDBNull(reader["RadiologistID"]);
                        BizActionObj.AppConfig.GynecologistID = (Int64)DALHelper.HandleDBNull(reader["GynecologistID"]);
                        /*
                         *  Pathologist for MFc, added on 25.05.2016
                         *  Added by Anumani
                         */
                        BizActionObj.AppConfig.PathologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathologistID"]));
                        //

                        BizActionObj.AppConfig.PhysicianID = (Int64)DALHelper.HandleDBNull(reader["PhysicianID"]);
                        BizActionObj.AppConfig.AndrologistID = (Int64)DALHelper.HandleDBNull(reader["AndriologistID"]);
                        BizActionObj.AppConfig.BiologistID = (Int64)DALHelper.HandleDBNull(reader["BiologistID"]);
                        BizActionObj.AppConfig.CountryID = (Int64)DALHelper.HandleDBNull(reader["CountryID"]);
                        BizActionObj.AppConfig.StateID = (Int64)DALHelper.HandleDBNull(reader["StateID"]);
                        BizActionObj.AppConfig.CityID = (Int64)DALHelper.HandleDBNull(reader["CityID"]);
                        BizActionObj.AppConfig.RegionID = (Int64)DALHelper.HandleDBNull(reader["RegionID"]);
                        BizActionObj.AppConfig.IsSellBySellingUnit = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSellBySellingUnit"]));

                        //added by neena
                        BizActionObj.AppConfig.Region = (string)DALHelper.HandleDBNull(reader["Region"]);
                        BizActionObj.AppConfig.RegionCode = (string)DALHelper.HandleDBNull(reader["RegionCode"]);
                        BizActionObj.AppConfig.CityN = (string)DALHelper.HandleDBNull(reader["CityN"]);
                        BizActionObj.AppConfig.CityCode = (string)DALHelper.HandleDBNull(reader["CityCode"]);
                        BizActionObj.AppConfig.StateN = (string)DALHelper.HandleDBNull(reader["StateN"]);
                        BizActionObj.AppConfig.StateCode = (string)DALHelper.HandleDBNull(reader["StateCode"]);
                        BizActionObj.AppConfig.CountryN = (string)DALHelper.HandleDBNull(reader["CountryN"]);
                        BizActionObj.AppConfig.CountryCode = (string)DALHelper.HandleDBNull(reader["CountryCode"]);
                        //


                        BizActionObj.AppConfig.InhouseLabID = (Int64)DALHelper.HandleDBNull(reader["InhouseLabID"]);
                        BizActionObj.AppConfig.OocyteDonationID = (Int64)DALHelper.HandleDBNull(reader["OocyteDonationID"]);
                        BizActionObj.AppConfig.OocyteReceipentID = (Int64)DALHelper.HandleDBNull(reader["OocyteReceipentID"]);
                        BizActionObj.AppConfig.EmbryoReceipentID = (Int64)DALHelper.HandleDBNull(reader["EmbryoReceipentID"]);
                        BizActionObj.AppConfig.DoctorTypeForReferral = (Int64)DALHelper.HandleDBNull(reader["DoctorTypeForReferral"]);
                        BizActionObj.AppConfig.IdentityForInternationalPatient = (Int64)DALHelper.HandleDBNull(reader["IdentityForInternationalPatient"]);
                        BizActionObj.AppConfig.AuthorizationLevelForRefundID = (Int64)DALHelper.HandleDBNull(reader["AuthorizationLevelForRefundID"]);
                        BizActionObj.AppConfig.AuthorizationLevelForConcessionID = (Int64)DALHelper.HandleDBNull(reader["AuthorizationLevelForConcessionID"]);
                        BizActionObj.AppConfig.AuthorizationLevelForPatientAdvRefundID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AuthorizationLevelForPatientAdvRefundID"]));//Added By Bhushanp 31052017

                        BizActionObj.AppConfig.AuthorizationLevelForMRPAdjustmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AuthorizationLevelForMRPAdjustmentID"]));


                        BizActionObj.AppConfig.AddressTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AddressTypeID"]));
                        BizActionObj.AppConfig.MaritalStatus = Convert.ToInt64(DALHelper.HandleDBNull(reader["MaritalStatus"]));

                        BizActionObj.AppConfig.CompanyPatientSourceID = (Int64)DALHelper.HandleDBNull(reader["CompanyPatientSourceID"]);
                        BizActionObj.AppConfig.FileName = (string)DALHelper.HandleDBNull(reader["LocalLanguageFileName"]);
                        BizActionObj.AppConfig.FilePath = (string)DALHelper.HandleDBNull(reader["LocalLanguageFilePath"]);
                        BizActionObj.AppConfig.ConsultationID = (Int64)DALHelper.HandleDBNull(reader["ConsultationId"]);
                        BizActionObj.AppConfig.PharmacySpecializationID = (Int64)DALHelper.HandleDBNull(reader["PharmacySpecializationID"]);
                        BizActionObj.AppConfig.ApplyConcessionToStaff = (bool)DALHelper.HandleDBNull(reader["ApplyConcessionToStaff"]);
                        BizActionObj.AppConfig.AllowClinicalTransaction = (bool)DALHelper.HandleDBNull(reader["AllowClinicalTransaction"]);
                        BizActionObj.AppConfig.AutoDeductStockFromRadiology = (bool)DALHelper.HandleDBNull(reader["AutoDeductStockFromRadiology"]);
                        BizActionObj.AppConfig.AutoGenerateSampleNo = (bool)DALHelper.HandleDBNull(reader["AutoGenerateSampleNo"]);
                        BizActionObj.AppConfig.AutoDeductStockFromPathology = (bool)DALHelper.HandleDBNull(reader["AutoDeductStockFromPathology"]);
                        BizActionObj.AppConfig.DateFormatID = (long)DALHelper.HandleIntegerNull(reader["DateFormatID"]);
                        BizActionObj.AppConfig.AddLogoToAllReports = (bool)DALHelper.HandleDBNull(reader["AddLogoToAllReports"]);
                        BizActionObj.AppConfig.AttachmentFileName = (string)DALHelper.HandleDBNull(reader["AttachmentFileName"]);
                        BizActionObj.AppConfig.Attachment = (byte[])DALHelper.HandleDBNull(reader["Attachment"]);
                        BizActionObj.AppConfig.CreditLimitIPD = (long)DALHelper.HandleIntegerNull(reader["IpdCreditLimit"]);
                        BizActionObj.AppConfig.CreditLimitOPD = (long)DALHelper.HandleIntegerNull(reader["OpdCreditLimit"]);
                        BizActionObj.AppConfig.ItemExpiredIndays = (long)DALHelper.HandleIntegerNull(reader["ItemExpiredIndays"]);//By Umesh
                        BizActionObj.AppConfig.DefaultCountryCode = (string)DALHelper.HandleDBNull(reader["DefaultCountryCode"]);//By Umesh

                        ////rohinee for cash caounter entery from app config //

                        BizActionObj.AppConfig.LabCounterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabCounterID"])); // (long)DALHelper.HandleDBNull(reader["LabCounterID"]);
                        BizActionObj.AppConfig.IPDCounterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IPDCounterID"])); //(long)DALHelper.HandleDBNull(reader["IPDCounterID"]);
                        BizActionObj.AppConfig.OPDCounterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OPDCounterID"])); //(long)DALHelper.HandleDBNull(reader["OPDCounterID"]); 
                        BizActionObj.AppConfig.PharmacyCounterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PharmacyCounterID"]));// (long)DALHelper.HandleDBNull(reader["PharmacyCounterID"]);
                        BizActionObj.AppConfig.RadiologyCounterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RadiologyCounterID"]));// (long)DALHelper.HandleDBNull(reader["RadiologyCounterID"]);
                        BizActionObj.AppConfig.IsCounterLogin = (bool)DALHelper.HandleDBNull(reader["IsCounterLogin"]);//(bool)DALHelper.HandleDBNull(reader["IsCounterLogin"]);
                        BizActionObj.AppConfig.IsProcessingUnit = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsProcessingUnit"]));//(bool)DALHelper.HandleDBNull(reader["IsCounterLogin"]);


                        //ROHINI
                        BizActionObj.AppConfig.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryID"])); // (long)DALHelper.HandleDBNull(reader["LabCounterID"]);
                        //
                        //by rohini dated 1/12/16
                        BizActionObj.AppConfig.Disclaimer = Convert.ToString(DALHelper.HandleDBNull(reader["Disclaimer"]));
                        //

                        //By Neena
                        BizActionObj.AppConfig.WebSite = Convert.ToString(DALHelper.HandleDBNull(reader["WebSite"]));
                        BizActionObj.AppConfig.IsPhotoMoveToServer = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPhotoMoveToServer"]));
                        //

                        //added by neena for ivf procedure billing
                        BizActionObj.AppConfig.IsIVFBillingCriteria = IsIVFBillingCriteria;
                        BizActionObj.AppConfig.ForIVFProcedureBilling = ForIVFProcedureBilling;
                        BizActionObj.AppConfig.ForTriggerProcedureBilling = ForTriggerProcedureBilling;
                        BizActionObj.AppConfig.ForOPUProcedureBilling = ForOPUProcedureBilling;
                        //

                        BizActionObj.AppConfig.IsConcessionReadOnly = IsConcessionReadOnly; //***//
                        BizActionObj.AppConfig.IsFertilityPoint = IsFertilityPoint;//***//

                        BizActionObj.AppConfig.ValidationsFlag = ValidationsFlag; //Added by NileshD on 19April2019

                        BizActionObj.AppConfig.InternationalId = InternationalId; //Added by NileshD on 19April2019

                        if (BizActionObj.AppConfig.DateFormatID > 0)
                        {
                            switch (BizActionObj.AppConfig.DateFormatID)
                            {
                                case 1: BizActionObj.AppConfig.DateFormat = "dd/MM/yyyy";
                                    break;
                                case 2: BizActionObj.AppConfig.DateFormat = "dd-MMM-yy";
                                    break;
                                case 3: BizActionObj.AppConfig.DateFormat = "M/d/yyyy";
                                    break;
                            }

                        }
                        // Added by BHUSHAN...21/01/2014   to include the Email sending details in Application configuration
                        BizActionObj.AppConfig.AttachmentFileName = (string)DALHelper.HandleDBNull(reader["AttachmentFileName"]);
                        BizActionObj.AppConfig.Attachment = (byte[])DALHelper.HandleDBNull(reader["Attachment"]);

                        BizActionObj.AppConfig.Host = Security.base64Decode((string)DALHelper.HandleDBNull(reader["Host"]));
                        BizActionObj.AppConfig.Port = (Int32)DALHelper.HandleDBNull(reader["Port"]);
                        BizActionObj.AppConfig.UserName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["UserName"]));
                        BizActionObj.AppConfig.Password = Security.base64Decode((string)DALHelper.HandleDBNull(reader["Password"]));
                        BizActionObj.AppConfig.EnableSsl = (bool)DALHelper.HandleDBNull(reader["EnableSsl"]);
                        BizActionObj.AppConfig.SMSUrl = Security.base64Decode((string)DALHelper.HandleDBNull(reader["SMS_Url"]));
                        BizActionObj.AppConfig.SMS_UserName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["SMS_UserName"]));
                        BizActionObj.AppConfig.SMSPassword = Security.base64Decode((string)DALHelper.HandleDBNull(reader["SMS_Password"]));
                        BizActionObj.AppConfig.IsAllowDischargeRequest = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAllowDischargeRequest"]));

                        //For Pathology Additions
                        BizActionObj.AppConfig.ApplicationDateTime = Convert.ToDateTime(DALHelper.HandleDate(reader["ApplicationDateTime"]));

                        //BizActionObj.AppConfig.ClinicalCostingDivisionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClinicalCostingDivisionID"])); //Costing Divisions for Clinical Billing
                        //BizActionObj.AppConfig.PharmacyCostingDivisionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PharmacyCostingDivisionID"])); //Costing Divisions for Pharmacy Billing

                        BizActionObj.AppConfig.PathologyDepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathologyDepartmentID"]));  //Set Department For Pathology
                        BizActionObj.AppConfig.RadiologyDepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RadiologyDepartmentID"]));  //Set Department For Radiology
                        //* Added by - Ajit Jadhav
                        //* Added Date - 6/9/2016
                        //* Comments - get Billing Exceeds Limit
                        BizActionObj.AppConfig.BillingExceedsLimit = Convert.ToDouble(DALHelper.HandleDBNull(reader["BillingExceedsLimit"]));
                        //***//-----------------

                        BizActionObj.AppConfig.RefundToAdvancePayModeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RefundToAdvancePayModeID"]));      // Refund To Advance 22042017

                        BizActionObj.AppConfig.EMRModVisitDateInDays = this.EMRModVisitDateInDays;  //EMR Changes Added by Ashish Z. on dated 02062017
                        BizActionObj.AppConfig.PatientDailyCashLimit = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PatientDailyCashLimit"]));
                        BizActionObj.AppConfig.CounterSaleBillAdresslimit = Convert.ToDecimal(DALHelper.HandleDBNull(reader["CounterSaleBillAdresslimit"]));
                    }
                    reader.NextResult();

                    // By BHUSHAN
                    BizActionObj.AppEmail = new List<clsAppConfigAutoEmailSMSVO>();
                    while (reader.Read())
                    {

                        clsAppConfigAutoEmailSMSVO AppEmailSMS = new clsAppConfigAutoEmailSMSVO();
                        AppEmailSMS.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        AppEmailSMS.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        AppEmailSMS.EventID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EventId"]));
                        AppEmailSMS.AppEmail = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmailTemplateID"]));
                        AppEmailSMS.AppSMS = Convert.ToInt64(DALHelper.HandleDBNull(reader["SMSTemplateID"]));
                        AppEmailSMS.EmailTemplatName = Convert.ToString(DALHelper.HandleDBNull(reader["EmailTemplate"]));
                        AppEmailSMS.SMSTemplatName = Convert.ToString(DALHelper.HandleDBNull(reader["SMSTemplate"]));
                        AppEmailSMS.EventType = Convert.ToString(DALHelper.HandleDBNull(reader["EventName"]));
                        AppEmailSMS.SendEmailId = Convert.ToString(DALHelper.HandleDBNull(reader["EmailID"]));

                        BizActionObj.AppEmail.Add(AppEmailSMS);
                    }
                    //while (reader.Read())
                    //{
                    //    BizActionObj.AppConfig.EmailConfig = new clsAutoEmailConfigVO();
                    //    BizActionObj.AppConfig.EmailConfig.ID = (long)DALHelper.HandleDBNull(reader["ID"]);

                    //    BizActionObj.AppConfig.EmailConfig.AppointmentConfirmation = (bool)DALHelper.HandleDBNull(reader["AppointmentConfirmation"]);
                    //    BizActionObj.AppConfig.EmailConfig.AppointmentCancellation = (bool)DALHelper.HandleDBNull(reader["AppointmentCancellation"]);
                    //    BizActionObj.AppConfig.EmailConfig.AutogeneratedPassword = (bool)DALHelper.HandleDBNull(reader["AutogeneratedPassword"]);
                    //    BizActionObj.AppConfig.EmailConfig.CampInformation = (bool)DALHelper.HandleDBNull(reader["CampInformation"]);

                    //    BizActionObj.AppConfig.EmailConfig.PatientRegistration = (bool)DALHelper.HandleDBNull(reader["PatReg"]);
                    //    BizActionObj.AppConfig.EmailConfig.PatientVisit = (bool)DALHelper.HandleDBNull(reader["PatVisitClosure"]);
                    //    BizActionObj.AppConfig.EmailConfig.PatientLab = (bool)DALHelper.HandleDBNull(reader["PatLabResult"]);
                    //    BizActionObj.AppConfig.EmailConfig.PatientLoyaltyReg = (bool)DALHelper.HandleDBNull(reader["PatLoyaltyReg"]);                        
                    //    BizActionObj.AppConfig.EmailConfig.PatientLoyaltyExpiry = (bool)DALHelper.HandleDBNull(reader["PatLoyaltyExpiry"]);
                    //    BizActionObj.AppConfig.EmailConfig.AppointmentReminder = (bool)DALHelper.HandleDBNull(reader["ApptReminder"]);
                    //    BizActionObj.AppConfig.EmailConfig.FollowupReminder = (bool)DALHelper.HandleDBNull(reader["Followup"]);
                    //    BizActionObj.AppConfig.EmailConfig.BirthdayWish = (bool)DALHelper.HandleDBNull(reader["Birthday"]);
                    //    // Added by Saily P

                    //    BizActionObj.AppConfig.EmailConfig.IsDrugNotify = (bool)DALHelper.HandleDBNull(reader["DrugNotify"]);
                    //    BizActionObj.AppConfig.EmailConfig.IsETNotify = (bool)DALHelper.HandleDBNull(reader["EmbryoTransfer"]);
                    //    BizActionObj.AppConfig.EmailConfig.IsOPUNotify = (bool)DALHelper.HandleDBNull(reader["OvumPickUp"]);

                    //    //
                    //    BizActionObj.AppConfig.EmailConfig.AppConfirmation = (long)DALHelper.HandleDBNull(reader["AppConfirmationId"]);
                    //    BizActionObj.AppConfig.EmailConfig.AutoUserPassword = (long)DALHelper.HandleDBNull(reader["AutoGeneratedPasswordId"]);
                    //    BizActionObj.AppConfig.EmailConfig.CampInfo = (long)DALHelper.HandleDBNull(reader["CampInfoId"]);
                    //    BizActionObj.AppConfig.EmailConfig.AppCancellation = (long)DALHelper.HandleDBNull(reader["AppCancellationId"]);

                    //    BizActionObj.AppConfig.EmailConfig.PatReg = (long)DALHelper.HandleDBNull(reader["PatRegId"]);
                    //    BizActionObj.AppConfig.EmailConfig.PatVisitClose = (long)DALHelper.HandleDBNull(reader["PatVisitClosureId"]);
                    //    BizActionObj.AppConfig.EmailConfig.PatLabResult = (long)DALHelper.HandleDBNull(reader["PatLabResultId"]);
                    //    BizActionObj.AppConfig.EmailConfig.PatLoyaltyCard = (long)DALHelper.HandleDBNull(reader["PatLoyaltyRegId"]);                        
                    //    BizActionObj.AppConfig.EmailConfig.LoyaltyCardExpiry = (long)DALHelper.HandleDBNull(reader["PatLoyaltyExpiryId"]);
                    //    BizActionObj.AppConfig.EmailConfig.ApptReminder = (long)DALHelper.HandleDBNull(reader["ApptReminderId"]);
                    //    BizActionObj.AppConfig.EmailConfig.Followup = (long)DALHelper.HandleDBNull(reader["FollowupId"]);
                    //    BizActionObj.AppConfig.EmailConfig.Birthday = (long)DALHelper.HandleDBNull(reader["BirthdayId"]);
                    //    // Added by Saily P

                    //    BizActionObj.AppConfig.EmailConfig.DrugNotify = (long)DALHelper.HandleDBNull(reader["DrugNotifyId"]);
                    //    BizActionObj.AppConfig.EmailConfig.ETNotify = (long)DALHelper.HandleDBNull(reader["ETId"]);
                    //    BizActionObj.AppConfig.EmailConfig.OPUNotify = (long)DALHelper.HandleDBNull(reader["OPUId"]);

                    //    //

                    //    BizActionObj.AppConfig.EmailConfig.AppConfirmationSMS = (long)DALHelper.HandleDBNull(reader["AppConfirmationSMSId"]);
                    //    BizActionObj.AppConfig.EmailConfig.AutoUserPasswordSMS = (long)DALHelper.HandleDBNull(reader["AutoGeneratedPasswordSMSId"]);
                    //    BizActionObj.AppConfig.EmailConfig.CampInfoSMS = (long)DALHelper.HandleDBNull(reader["CampInfoSMSId"]);
                    //    BizActionObj.AppConfig.EmailConfig.AppCancellationSMS = (long)DALHelper.HandleDBNull(reader["AppCancellationSMSId"]);

                    //    BizActionObj.AppConfig.EmailConfig.PatRegSMS = (long)DALHelper.HandleDBNull(reader["PatRegSMSId"]);
                    //    BizActionObj.AppConfig.EmailConfig.PatVisitCloseSMS = (long)DALHelper.HandleDBNull(reader["PatVisitClosureSMSId"]);
                    //    BizActionObj.AppConfig.EmailConfig.PatLabResultSMS = (long)DALHelper.HandleDBNull(reader["PatLabResultSMSId"]);
                    //    BizActionObj.AppConfig.EmailConfig.PatLoyaltyCardSMS = (long)DALHelper.HandleDBNull(reader["PatLoyaltyRegSMSId"]);
                    //    BizActionObj.AppConfig.EmailConfig.LoyaltyCardExpirySMS = (long)DALHelper.HandleDBNull(reader["PatLoyaltyExpirySMSId"]);
                    //    BizActionObj.AppConfig.EmailConfig.ApptReminderSMS = (long)DALHelper.HandleDBNull(reader["ApptReminderSMSId"]);
                    //    BizActionObj.AppConfig.EmailConfig.FollowupSMS = (long)DALHelper.HandleDBNull(reader["FollowupSMSId"]);
                    //    BizActionObj.AppConfig.EmailConfig.BirthdaySMS  = (long)DALHelper.HandleDBNull(reader["BirthdaySMSId"]);

                    //    // Added by Saily P

                    //    BizActionObj.AppConfig.EmailConfig.DrugNotifySMS = (long)DALHelper.HandleDBNull(reader["DrugNotifySMSId"]);
                    //    BizActionObj.AppConfig.EmailConfig.ETNotifySMS = (long)DALHelper.HandleDBNull(reader["ETSMSId"]);
                    //    BizActionObj.AppConfig.EmailConfig.OPUNotifySMS = (long)DALHelper.HandleDBNull(reader["OPUSMSId"]);

                    //}

                    reader.NextResult();
                    while (reader.Read())
                    {
                        BizActionObj.AppConfig.Accounts.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        BizActionObj.AppConfig.Accounts.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        BizActionObj.AppConfig.Accounts.ChequeDDBankID = (long)DALHelper.HandleDBNull(reader["ChequeDDBankID"]);
                        BizActionObj.AppConfig.Accounts.CrDBBankID = (long)DALHelper.HandleDBNull(reader["CrDBBankID"]);
                        BizActionObj.AppConfig.Accounts.ChequeDDBankName = (string)DALHelper.HandleDBNull(reader["ChequeDDBankName"]);
                        BizActionObj.AppConfig.Accounts.CrDbBankName = (string)DALHelper.HandleDBNull(reader["CrDBBankName"]);
                        BizActionObj.AppConfig.Accounts.CashLedgerName = (string)DALHelper.HandleDBNull(reader["CashLedgerName"]);
                        BizActionObj.AppConfig.Accounts.AdvanceLedgerName = (string)DALHelper.HandleDBNull(reader["AdvanceLedgerName"]);
                        BizActionObj.AppConfig.Accounts.ConsultationLedgerName = (string)DALHelper.HandleDBNull(reader["ConsultationLedgerName"]);
                        BizActionObj.AppConfig.Accounts.DiagnosticLedgerName = (string)DALHelper.HandleDBNull(reader["DiagnosticLedgerName"]);
                        BizActionObj.AppConfig.Accounts.OtherServicesLedgerName = (string)DALHelper.HandleDBNull(reader["OtherServicesLedgerName"]);
                        BizActionObj.AppConfig.Accounts.PurchaseLedgerName = (string)DALHelper.HandleDBNull(reader["PurchaseLedgerName"]);
                        BizActionObj.AppConfig.Accounts.COGSLedgerName = (string)DALHelper.HandleDBNull(reader["COGSLedgerName"]);
                        BizActionObj.AppConfig.Accounts.ProfitLedgerName = (string)DALHelper.HandleDBNull(reader["ProfitLedgerName"]);
                        BizActionObj.AppConfig.Accounts.ScrapIncomeLedgerName = (string)DALHelper.HandleDBNull(reader["ScrapIncomeLedgerName"]);
                        BizActionObj.AppConfig.Accounts.CurrentAssetLedgerName = (string)DALHelper.HandleDBNull(reader["CurrentAssetLedgerName"]);
                        BizActionObj.AppConfig.Accounts.ExpenseLedgerName = (string)DALHelper.HandleDBNull(reader["ExpenseLedgerName"]);
                        BizActionObj.AppConfig.Accounts.ItemScrapCategory = (long)DALHelper.HandleDBNull(reader["ItemScrapCategory"]);

                        //Patho Color Code added by Anumani on 16.02.2016
                        BizActionObj.AppConfig.pathonormalColorCode = Convert.ToString(DALHelper.HandleDBNull(reader["PathoNormalValueColor"]));
                        BizActionObj.AppConfig.pathominColorCode = Convert.ToString(DALHelper.HandleDBNull(reader["PathoMinValueColor"]));
                        BizActionObj.AppConfig.pathomaxColorCode = Convert.ToString(DALHelper.HandleDBNull(reader["PathoMaxValueColor"]));
                        BizActionObj.AppConfig.SampleNoLevel = Convert.ToInt64(DALHelper.HandleDBNull(reader["SampleNoLevel"]));

                        BizActionObj.AppConfig.FirstLevelResultColor = Convert.ToString(DALHelper.HandleDBNull(reader["FirstLevelResultColor"]));
                        BizActionObj.AppConfig.SecondLevelResultColor = Convert.ToString(DALHelper.HandleDBNull(reader["SecondLevelResultColor"]));
                        BizActionObj.AppConfig.ThirdLevelResultColor = Convert.ToString(DALHelper.HandleDBNull(reader["ThirdLevelResultColor"]));
                        BizActionObj.AppConfig.CheckResultColor = Convert.ToString(DALHelper.HandleDBNull(reader["CheckResultColor"]));

                        //

                    }

                    reader.NextResult();
                    while (reader.Read())
                    {
                        BizActionObj.AppConfig.ClientID = (long)DALHelper.HandleDBNull(reader["ClientID"]);
                        BizActionObj.AppConfig.SubsciptionEndDate = (DateTime?)DALHelper.HandleDate(reader["SubscriptionEndtDate"]);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                BizActionObj.Error = ex.Message;
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));

            }
            finally
            {

            }

            return BizActionObj;
        }
        //clsGetAppUserUnitBizActionVO
        //public override IValueObject GetLoginUnit(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetAppUserUnitBizActionVO BizActionObj = valueObject as clsGetAppUserUnitBizActionVO();
        //    try 
        //    {	        

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //    throw new NotImplementedException();
        //}

        public override IValueObject UpdateAppConfig(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateAppConfigBizActionVO BizActionObj = valueObject as clsUpdateAppConfigBizActionVO;

            DbConnection con = null;
            DbTransaction trans = null;

            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                clsAppConfigVO objDetailsVO = BizActionObj.AppConfig;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateApplicationConfig");

                dbServer.AddInParameter(command, "DateFormatID", DbType.Int64, objDetailsVO.DateFormatID);
                dbServer.AddInParameter(command, "IsConfigured", DbType.Boolean, objDetailsVO.IsConfigured);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDetailsVO.UnitID);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objDetailsVO.DoctorID);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, objDetailsVO.DepartmentID);
                dbServer.AddInParameter(command, "PatientCategoryID", DbType.Int64, objDetailsVO.PatientCategoryID);
                dbServer.AddInParameter(command, "PharmacyPatientCategoryID", DbType.Int64, objDetailsVO.PharmacyPatientCategoryID);
                dbServer.AddInParameter(command, "Country", DbType.String, objDetailsVO.Country);
                dbServer.AddInParameter(command, "State", DbType.String, objDetailsVO.State);
                dbServer.AddInParameter(command, "District", DbType.String, objDetailsVO.District);
                dbServer.AddInParameter(command, "Pincode", DbType.String, objDetailsVO.Pincode);
                dbServer.AddInParameter(command, "City", DbType.String, objDetailsVO.City);
                dbServer.AddInParameter(command, "Area", DbType.String, objDetailsVO.Area);
                dbServer.AddInParameter(command, "VisitTypeID", DbType.Int64, objDetailsVO.VisitTypeID);

                dbServer.AddInParameter(command, "PatientSourceID", DbType.Int64, objDetailsVO.PatientSourceID);

                dbServer.AddInParameter(command, "AppointmentSlot", DbType.Double, objDetailsVO.AppointmentSlot);
                dbServer.AddInParameter(command, "IsHO", DbType.Boolean, objDetailsVO.IsHO);
                dbServer.AddInParameter(command, "DatabaseName", DbType.String, objDetailsVO.DatabaseName);
                dbServer.AddInParameter(command, "Email", DbType.String, objDetailsVO.Email);
                dbServer.AddInParameter(command, "NurseRoleID", DbType.Int64, objDetailsVO.NurseRoleID);
                dbServer.AddInParameter(command, "AdminRoleID", DbType.Int64, objDetailsVO.AdminRoleID);
                dbServer.AddInParameter(command, "DoctorRoleID", DbType.Int64, objDetailsVO.DoctorRoleID);
                dbServer.AddInParameter(command, "ConftnMsgForAdd", DbType.Boolean, objDetailsVO.ConftnMsgForAdd);
                dbServer.AddInParameter(command, "SearchPatientsInterval", DbType.Int16, objDetailsVO.SearchPatientsInterval);
                dbServer.AddInParameter(command, "PaymentModeID", DbType.Int64, objDetailsVO.PaymentModeID);
                dbServer.AddInParameter(command, "PathoSpecializationID", DbType.Int64, objDetailsVO.PathoSpecializationID);
                dbServer.AddInParameter(command, "PharmacySpecializationId", DbType.Int64, objDetailsVO.PharmacySpecializationID);
                dbServer.AddInParameter(command, "ConsultationId", DbType.Int64, objDetailsVO.ConsultationID);
                dbServer.AddInParameter(command, "SelfCompanyID", DbType.Int64, objDetailsVO.SelfCompanyID);
                dbServer.AddInParameter(command, "SelfTariffID", DbType.Int64, objDetailsVO.TariffID);
                dbServer.AddInParameter(command, "FreeFollowupDays", DbType.Int16, objDetailsVO.FreeFollowupDays);
                dbServer.AddInParameter(command, "FreeFollowupVisitTypeID", DbType.Int16, objDetailsVO.FreeFollowupVisitTypeID);
                dbServer.AddInParameter(command, "PharmacyVisitTypeID", DbType.Int64, objDetailsVO.PharmacyVisitTypeID);
                //added by rohini
                dbServer.AddInParameter(command, "PathologyVisitTypeID", DbType.Int64, objDetailsVO.PathologyVisitTypeID);
                dbServer.AddInParameter(command, "PathologyCompanyTypeID", DbType.Int64, objDetailsVO.PathologyCompanyTypeID);
                //
                dbServer.AddInParameter(command, "RadiologySpecializationID", DbType.Int64, objDetailsVO.RadiologySpecializationID);

                dbServer.AddInParameter(command, "RefundMode", DbType.Int64, objDetailsVO.RefundPayModeID);
                dbServer.AddInParameter(command, "RefundAmount", DbType.Double, objDetailsVO.RefundAmount);

                dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, objDetailsVO.EmbryologistID);
                dbServer.AddInParameter(command, "AnesthetistID", DbType.Int64, objDetailsVO.AnesthetistID);
                dbServer.AddInParameter(command, "RadiologistID", DbType.Int64, objDetailsVO.RadiologistID);
                dbServer.AddInParameter(command, "AndriologistID", DbType.Int64, objDetailsVO.AndrologistID);
                dbServer.AddInParameter(command, "GynecologistID", DbType.Int64, objDetailsVO.GynecologistID);
                dbServer.AddInParameter(command, "PhysicianID", DbType.Int64, objDetailsVO.PhysicianID);
                dbServer.AddInParameter(command, "BiologistID", DbType.Int64, objDetailsVO.BiologistID);
                dbServer.AddInParameter(command, "AddressTypeID", DbType.Int64, objDetailsVO.AddressTypeID);
                dbServer.AddInParameter(command, "CountryID", DbType.Int64, objDetailsVO.CountryID);
                dbServer.AddInParameter(command, "StateID", DbType.Int64, objDetailsVO.StateID);
                dbServer.AddInParameter(command, "CityID", DbType.Int64, objDetailsVO.CityID);
                dbServer.AddInParameter(command, "RegionID", DbType.Int64, objDetailsVO.RegionID);

                dbServer.AddInParameter(command, "IsCentralPurchaseStore", DbType.Boolean, objDetailsVO.IsCentralPurchaseStore);
                dbServer.AddInParameter(command, "IndentStoreID", DbType.Int64, objDetailsVO.IndentStoreID);

                dbServer.AddInParameter(command, "InhouseLabID", DbType.Int64, objDetailsVO.InhouseLabID);
                dbServer.AddInParameter(command, "OocyteDonationID", DbType.Int64, objDetailsVO.OocyteDonationID);
                dbServer.AddInParameter(command, "OocyteReceipentID", DbType.Int64, objDetailsVO.OocyteReceipentID);
                dbServer.AddInParameter(command, "EmbryoReceipentID", DbType.Int64, objDetailsVO.EmbryoReceipentID);
                dbServer.AddInParameter(command, "DoctorTypeForReferral", DbType.Int64, objDetailsVO.DoctorTypeForReferral);
                dbServer.AddInParameter(command, "IdentityForInternationalPatient", DbType.Int64, objDetailsVO.IdentityForInternationalPatient);
                dbServer.AddInParameter(command, "AuthorizationLevelForRefundID", DbType.Int64, objDetailsVO.AuthorizationLevelForRefundID);
                dbServer.AddInParameter(command, "AuthorizationLevelForConcessionID", DbType.Int64, objDetailsVO.AuthorizationLevelForConcessionID);
                dbServer.AddInParameter(command, "AuthorizationLevelForMRPAdjustmentID", DbType.Int64, objDetailsVO.AuthorizationLevelForMRPAdjustmentID);

                dbServer.AddInParameter(command, "CompanyPatientSourceID", DbType.Int64, objDetailsVO.CompanyPatientSourceID);
                dbServer.AddInParameter(command, "SelfRelationID", DbType.Int64, objDetailsVO.SelfRelationID);
                dbServer.AddInParameter(command, "RadiologyStoreID", DbType.Int64, objDetailsVO.RadiologyStoreID);
                dbServer.AddInParameter(command, "PathologyStoreID", DbType.Int64, objDetailsVO.PathologyStoreID);
                dbServer.AddInParameter(command, "OTStoreID", DbType.Int64, objDetailsVO.OTStoreID);
                dbServer.AddInParameter(command, "PharmacyStoreID", DbType.Int64, objDetailsVO.PharmacyStoreID);
                dbServer.AddInParameter(command, "ApplyConcessionToStaff", DbType.Boolean, objDetailsVO.ApplyConcessionToStaff);
                dbServer.AddInParameter(command, "AllowClinicalTransaction", DbType.Boolean, objDetailsVO.AllowClinicalTransaction);
                dbServer.AddInParameter(command, "AutoDeductStockFromRadiology", DbType.Boolean, objDetailsVO.AutoDeductStockFromRadiology);
                dbServer.AddInParameter(command, "AutoDeductStockFromPathology", DbType.Boolean, objDetailsVO.AutoDeductStockFromPathology);
                dbServer.AddInParameter(command, "AutoGenerateSampleNo", DbType.Boolean, objDetailsVO.AutoGenerateSampleNo);
                dbServer.AddInParameter(command, "AddLogoToAllReports", DbType.Boolean, objDetailsVO.AddLogoToAllReports);
                //dbServer.AddInParameter(command, "AttachmentFileName", DbType.String, objDetailsVO.AttachmentFileName);
                //dbServer.AddInParameter(command, "Attachment", DbType.Binary, objDetailsVO.Attachment);
                //dbServer.AddInParameter(command, "ID", DbType.Int64, objDetailsVO.ID);

                /*
                 *  Pathologist as MFC, Added on 25.05.2016
                 *  Added by Anumani
                 */
                dbServer.AddInParameter(command, "PathologistID", DbType.Int64, objDetailsVO.PathologistID);
                //

                // By BHUSHAN On 21/01/32014.
                dbServer.AddInParameter(command, "AttachmentFileName", DbType.String, objDetailsVO.AttachmentFileName);
                dbServer.AddInParameter(command, "Attachment", DbType.Binary, objDetailsVO.Attachment);

                dbServer.AddInParameter(command, "Host", DbType.String, Security.base64Encode(objDetailsVO.Host));
                dbServer.AddInParameter(command, "Port", DbType.Int32, objDetailsVO.Port);
                dbServer.AddInParameter(command, "UserName", DbType.String, Security.base64Encode(objDetailsVO.UserName));
                dbServer.AddInParameter(command, "Password", DbType.String, Security.base64Encode(objDetailsVO.Password));
                dbServer.AddInParameter(command, "EnableSSL", DbType.Boolean, objDetailsVO.EnableSsl);

                dbServer.AddInParameter(command, "SMS_Url", DbType.String, Security.base64Encode(objDetailsVO.SMSUrl));
                dbServer.AddInParameter(command, "SMS_UserName", DbType.String, Security.base64Encode(objDetailsVO.SMS_UserName));
                dbServer.AddInParameter(command, "SMS_Password", DbType.String, Security.base64Encode(objDetailsVO.SMSPassword));
                dbServer.AddInParameter(command, "IsAllowDischargeRequest", DbType.Boolean, objDetailsVO.IsAllowDischargeRequest);

                dbServer.AddInParameter(command, "PrintFormatID", DbType.Int64, objDetailsVO.PrintFormatID);

                //dbServer.AddInParameter(command, "ClinicalCostingDivisionID", DbType.Int64, objDetailsVO.ClinicalCostingDivisionID);  //Costing Divisions for Clinical Billing
                //dbServer.AddInParameter(command, "PharmacyCostingDivisionID", DbType.Int64, objDetailsVO.PharmacyCostingDivisionID);  //Costing Divisions for Pharmacy Billing

                dbServer.AddInParameter(command, "PathologyDepartmentID", DbType.Int64, objDetailsVO.PathologyDepartmentID);  //Set Department For Pathology
                dbServer.AddInParameter(command, "RadiologyDepartmentID", DbType.Int64, objDetailsVO.RadiologyDepartmentID);  //Set Department For Radiology

                // By UmeshF To Set Credit limit for IPD and OPD

                dbServer.AddInParameter(command, "IsIPD", DbType.Boolean, objDetailsVO.IsIPD);
                dbServer.AddInParameter(command, "IsOPD", DbType.Boolean, objDetailsVO.IsOPD);
                dbServer.AddInParameter(command, "IpdCreditLimit", DbType.Int64, objDetailsVO.CreditLimitIPD);
                dbServer.AddInParameter(command, "OpdCreditLimit", DbType.Int64, objDetailsVO.CreditLimitOPD);
                dbServer.AddInParameter(command, "ItemExpiredIndays", DbType.Int64, objDetailsVO.ItemExpiredIndays);
                dbServer.AddInParameter(command, "DefaultCountryCode", DbType.String, objDetailsVO.DefaultCountryCode);

                //rohinee for set cash counter from app config
                dbServer.AddInParameter(command, "LabCounterID", DbType.Int64, objDetailsVO.LabCounterID);
                dbServer.AddInParameter(command, "IPDCounterID", DbType.Int64, objDetailsVO.IPDCounterID);
                dbServer.AddInParameter(command, "OPDCounterID", DbType.Int64, objDetailsVO.OPDCounterID);
                dbServer.AddInParameter(command, "PharmacyCounterID", DbType.Int64, objDetailsVO.PharmacyCounterID);
                dbServer.AddInParameter(command, "RadiologyCounterID", DbType.Int64, objDetailsVO.RadiologyCounterID);
                //

                //rohini
                dbServer.AddInParameter(command, "CategoryID", DbType.Int64, objDetailsVO.CategoryID);
                //
                //by rohini dated 12/11/16
                dbServer.AddInParameter(command, "Disclaimer", DbType.String, objDetailsVO.Disclaimer);
                //

                //by neena
                dbServer.AddInParameter(command, "WebSite", DbType.String, objDetailsVO.WebSite);
                //

                //* Added by - Ajit Jadhav
                //* Added Date - 6/9/2016
                //* Comments - Update Billing Exceeds Limit              
                dbServer.AddInParameter(command, "BillingExceedsLimit", DbType.Double, objDetailsVO.BillingExceedsLimit);
                //***//-----------------

                dbServer.AddInParameter(command, "PatientDailyCashLimit", DbType.Decimal, objDetailsVO.PatientDailyCashLimit);


                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                // BY BHUSHAN . . . .


                DbCommand DeleteCommand = dbServer.GetStoredProcCommand("CIMS_DeleteAutoEmailConfig");
                dbServer.AddInParameter(DeleteCommand, "UnitId", DbType.Int64, BizActionObj.AppConfig.UnitID);

                int intDelStatus = dbServer.ExecuteNonQuery(DeleteCommand, trans);

                List<clsAppConfigAutoEmailSMSVO> EmailList = BizActionObj.AppEmail;
                foreach (clsAppConfigAutoEmailSMSVO item in EmailList)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_UpdateAutoEmailConfig_New");

                    dbServer.AddInParameter(command1, "ID", DbType.Int64, item.ID);
                    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, BizActionObj.AppConfig.UnitID);
                    dbServer.AddInParameter(command1, "EventId", DbType.Int64, item.EventID);
                    dbServer.AddInParameter(command1, "EmailTemplateId", DbType.Int64, item.AppEmail);
                    dbServer.AddInParameter(command1, "SMSTemplateId", DbType.Int64, item.AppSMS);
                    dbServer.AddInParameter(command1, "EmailId", DbType.String, item.SendEmailId);
                    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, item.CreatedUnitID);
                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, item.AddedBy);
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, item.AddedOn);
                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, item.AddedDateTime);
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, item.AddedWindowsLoginName);
                    dbServer.AddInParameter(command1, "UpdatedUnitID", DbType.Int64, item.UpdatedUnitID);
                    dbServer.AddInParameter(command1, "UpdatedBy", DbType.Int64, item.UpdatedBy);
                    dbServer.AddInParameter(command1, "UpdatedOn", DbType.String, item.UpdatedOn);
                    dbServer.AddInParameter(command1, "UpdatedDateTime", DbType.DateTime, item.UpdatedDateTime);
                    dbServer.AddInParameter(command1, "UpdateWindowsLoginName", DbType.String, item.UpdatedWindowsLoginName);

                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                }

                DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_UpdateApplicationAccountsConfig");

                dbServer.AddInParameter(command2, "ID", DbType.Int64, objDetailsVO.ID);
                dbServer.AddInParameter(command2, "UnitID", DbType.Int64, objDetailsVO.UnitID);

                dbServer.AddInParameter(command2, "ChequeDDBankID", DbType.Int64, objDetailsVO.Accounts.ChequeDDBankID);
                dbServer.AddInParameter(command2, "CrDBBankID", DbType.Int64, objDetailsVO.Accounts.CrDBBankID);
                dbServer.AddInParameter(command2, "CashLedgerName", DbType.String, objDetailsVO.Accounts.CashLedgerName);
                dbServer.AddInParameter(command2, "AdvanceLedgerName", DbType.String, objDetailsVO.Accounts.AdvanceLedgerName);
                dbServer.AddInParameter(command2, "ConsultationLedgerName", DbType.String, objDetailsVO.Accounts.ConsultationLedgerName);
                dbServer.AddInParameter(command2, "DiagnosticLedgerName", DbType.String, objDetailsVO.Accounts.DiagnosticLedgerName);
                dbServer.AddInParameter(command2, "OtherServicesLedgerName", DbType.String, objDetailsVO.Accounts.OtherServicesLedgerName);
                dbServer.AddInParameter(command2, "PurchaseLedgerName", DbType.String, objDetailsVO.Accounts.PurchaseLedgerName);
                dbServer.AddInParameter(command2, "COGSLedgerName", DbType.String, objDetailsVO.Accounts.COGSLedgerName);
                dbServer.AddInParameter(command2, "ProfitLedgerName", DbType.String, objDetailsVO.Accounts.ProfitLedgerName);

                dbServer.AddInParameter(command2, "ScrapIncomeLedgerName", DbType.String, objDetailsVO.Accounts.ScrapIncomeLedgerName);
                dbServer.AddInParameter(command2, "CurrentAssetLedgerName", DbType.String, objDetailsVO.Accounts.CurrentAssetLedgerName);
                dbServer.AddInParameter(command2, "ExpenseLedgerName", DbType.String, objDetailsVO.Accounts.ExpenseLedgerName);
                dbServer.AddInParameter(command2, "ItemScrapCategory ", DbType.Int64, objDetailsVO.Accounts.ItemScrapCategory);

                int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.AppConfig = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }

            return valueObject;
        }

        public override IValueObject GetAutoEmailConfig(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetAutoEmailConfigBizActionVO BizActionObj = valueObject as clsGetAutoEmailConfigBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAutoEmailConfig");
                DbDataReader reader;

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.Details == null)
                        BizActionObj.Details = new clsAutoEmailConfigVO();
                    while (reader.Read())
                    {
                        BizActionObj.Details.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        BizActionObj.Details.AppointmentConfirmation = (bool)DALHelper.HandleDBNull(reader["AppointmentConfirmation"]);
                        BizActionObj.Details.AppointmentCancellation = (bool)DALHelper.HandleDBNull(reader["AppointmentCancellation"]);
                        BizActionObj.Details.AutogeneratedPassword = (bool)DALHelper.HandleDBNull(reader["AutogeneratedPassword"]);
                        BizActionObj.Details.CampInformation = (bool)DALHelper.HandleDBNull(reader["CampInformation"]);

                    }

                }
                reader.Close();
            }
            catch (Exception ex)
            {
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));

            }
            finally
            {

            }

            return BizActionObj;
        }

        public override IValueObject UpdateAutoEmailConfig(IValueObject valueObject, clsUserVO UserVo)
        {
            try
            {
                clsUpdateAutoEmailConfigBizActionVO BizActionObj = valueObject as clsUpdateAutoEmailConfigBizActionVO;

                clsAutoEmailConfigVO objDetailsVO = BizActionObj.Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateAutoEmailConfig");

                dbServer.AddInParameter(command, "AppointmentConfirmation", DbType.Boolean, objDetailsVO.AppointmentConfirmation);
                dbServer.AddInParameter(command, "AppointmentCancellation", DbType.Boolean, objDetailsVO.AppointmentCancellation);
                dbServer.AddInParameter(command, "AutogeneratedPassword", DbType.Boolean, objDetailsVO.AutogeneratedPassword);
                dbServer.AddInParameter(command, "CampInformation", DbType.Boolean, objDetailsVO.CampInformation);

                int intStatus = dbServer.ExecuteNonQuery(command);


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

        public override IValueObject GetAutoEmailCCTOConfig(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAppEmailCCToBizActionVo BizActionObj = valueObject as clsAppEmailCCToBizActionVo;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetEmailCCTO");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "configAutoEmailID", DbType.Int64, BizActionObj.ConfigAutoEmailID);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0);

                DbDataReader reader;
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                    //dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                    //if (BizActionObj.AppEmailCC == null)
                    //    BizActionObj.AppEmailCC = new clsAppEmailCCToVo();

                    if (BizActionObj.ItemList == null)
                        BizActionObj.ItemList = new List<clsAppEmailCCToVo>();

                while (reader.Read())
                {
                    clsAppEmailCCToVo objEmailCC = new clsAppEmailCCToVo();

                    objEmailCC.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                    objEmailCC.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                    objEmailCC.ConfigAutoEmailID = (long)DALHelper.HandleDBNull(reader["ConfigAutoEmailID"]);
                    objEmailCC.CCToEmailID = (string)DALHelper.HandleDBNull(reader["CCToEmailID"]);
                    objEmailCC.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);

                    // objEmailCC.TotalRowCount = (int)dbServer.GetParameterValue(command, "TotalRows");

                    BizActionObj.ItemList.Add(objEmailCC);
                }

            }

            catch (Exception ex)
            {

            }
            finally
            {

            }
            return valueObject;
        }

        // added by BHUSHAN  21-01-2014
        public override IValueObject SetStatusAutoEmailCCTO(IValueObject valueObject, clsUserVO UserVo)
        {
            clsStatusEmailCCToBizActionVo objItem = valueObject as clsStatusEmailCCToBizActionVo;
            try
            {
                DbCommand command = null;
                clsAppEmailCCToVo objItemVO = objItem.AppEmailCC;
                command = dbServer.GetStoredProcCommand("CIMS_Update_StatusEmailCCTo");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objItemVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitID);
                dbServer.AddInParameter(command, "ConfigAutoEmailID", DbType.Int64, objItemVO.ConfigAutoEmailID);
                dbServer.AddInParameter(command, "Status", DbType.Int64, objItemVO.Status);

                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objItemVO.UpdatedBy);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objItemVO.UpdatedOn);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objItemVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objItemVO.UpdateWindowsLoginName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0);
                dbServer.ExecuteNonQuery(command);
                objItem.ResultStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objItem;
        }

        // added by BHUSHAN  21-01-2014
        public override IValueObject AddEmailIDCCTo(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddEmailIDCCToBizActionVo objItem = valueObject as clsAddEmailIDCCToBizActionVo;

            try
            {
                DbCommand command = null;
                clsAppEmailCCToVo objItemVO = objItem.AppEmailCC;
                command = dbServer.GetStoredProcCommand("CIMS_Add_Config_AutoEmailCopyTo");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objItemVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitID);
                dbServer.AddInParameter(command, "ConfigAutoEmailID", DbType.Int64, objItemVO.ConfigAutoEmailID);
                dbServer.AddInParameter(command, "CCToEmailID", DbType.String, objItemVO.CCToEmailID);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0);

                dbServer.ExecuteNonQuery(command);

                objItem.ResultStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objItem;
        }
    }
}
