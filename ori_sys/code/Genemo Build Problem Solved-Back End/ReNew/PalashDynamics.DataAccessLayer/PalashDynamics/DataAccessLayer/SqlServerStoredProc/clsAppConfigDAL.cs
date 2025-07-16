namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.CustomExceptions;
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Reflection;

    public class clsAppConfigDAL : clsBaseAppConfigDAL
    {
        private Database dbServer;
        private LogManager logManager;
        private bool IsIVFBillingCriteria;
        private string ForIVFProcedureBilling;
        private string ForTriggerProcedureBilling;
        private string ForOPUProcedureBilling;
        private bool IsConcessionReadOnly;
        private bool IsFertilityPoint;
        private bool IsZivaFertility;
        private bool ValidationsFlag;
        private long InternationalId;
        private int EMRModVisitDateInDays;

        private clsAppConfigDAL()
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
                this.IsIVFBillingCriteria = Convert.ToBoolean(ConfigurationManager.AppSettings["IsIVFBillingCriteria"]);
                this.ForIVFProcedureBilling = ConfigurationManager.AppSettings["ForIVFProcedureBilling"].ToString();
                this.ForTriggerProcedureBilling = ConfigurationManager.AppSettings["ForTriggerProcedureBilling"].ToString();
                this.ForOPUProcedureBilling = ConfigurationManager.AppSettings["ForOPUProcedureBilling"].ToString();
                this.IsConcessionReadOnly = Convert.ToBoolean(ConfigurationManager.AppSettings["IsConcessionReadOnly"]);
                this.IsFertilityPoint = Convert.ToBoolean(ConfigurationManager.AppSettings["IsFertilityPoint"]);
                this.IsZivaFertility = Convert.ToBoolean(ConfigurationManager.AppSettings["IsZivaFertility"]);
                this.ValidationsFlag = Convert.ToBoolean(ConfigurationManager.AppSettings["ValidationsFlag"]);
                this.InternationalId = Convert.ToInt64(ConfigurationManager.AppSettings["InternationalId"]);
                this.EMRModVisitDateInDays = Convert.ToInt32(ConfigurationManager.AppSettings["EMRModVisitDateInDays"]);
                if (this.EMRModVisitDateInDays == 0)
                {
                    this.EMRModVisitDateInDays = 1;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override IValueObject AddEmailIDCCTo(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddEmailIDCCToBizActionVo vo = valueObject as clsAddEmailIDCCToBizActionVo;
            try
            {
                DbCommand storedProcCommand = null;
                clsAppEmailCCToVo appEmailCC = vo.AppEmailCC;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_Add_Config_AutoEmailCopyTo");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, appEmailCC.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, appEmailCC.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ConfigAutoEmailID", DbType.Int64, appEmailCC.ConfigAutoEmailID);
                this.dbServer.AddInParameter(storedProcCommand, "CCToEmailID", DbType.String, appEmailCC.CCToEmailID);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, appEmailCC.CreatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, appEmailCC.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, appEmailCC.AddedBy);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, appEmailCC.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, appEmailCC.AddedWindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                vo.ResultStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return vo;
        }

        public override IValueObject GetAppConfig(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetAppConfigBizActionVO nvo = valueObject as clsGetAppConfigBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAppConfig");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.AppConfig == null)
                    {
                        nvo.AppConfig = new clsAppConfigVO();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.AppEmail = new List<clsAppConfigAutoEmailSMSVO>();
                            while (true)
                            {
                                if (!reader.Read())
                                {
                                    reader.NextResult();
                                    while (true)
                                    {
                                        if (!reader.Read())
                                        {
                                            reader.NextResult();
                                            while (reader.Read())
                                            {
                                                nvo.AppConfig.ClientID = (long) DALHelper.HandleDBNull(reader["ClientID"]);
                                                nvo.AppConfig.SubsciptionEndDate = DALHelper.HandleDate(reader["SubscriptionEndtDate"]);
                                            }
                                            break;
                                        }
                                        nvo.AppConfig.Accounts.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                                        nvo.AppConfig.Accounts.UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]);
                                        nvo.AppConfig.Accounts.ChequeDDBankID = (long) DALHelper.HandleDBNull(reader["ChequeDDBankID"]);
                                        nvo.AppConfig.Accounts.CrDBBankID = (long) DALHelper.HandleDBNull(reader["CrDBBankID"]);
                                        nvo.AppConfig.Accounts.ChequeDDBankName = (string) DALHelper.HandleDBNull(reader["ChequeDDBankName"]);
                                        nvo.AppConfig.Accounts.CrDbBankName = (string) DALHelper.HandleDBNull(reader["CrDBBankName"]);
                                        nvo.AppConfig.Accounts.CashLedgerName = (string) DALHelper.HandleDBNull(reader["CashLedgerName"]);
                                        nvo.AppConfig.Accounts.AdvanceLedgerName = (string) DALHelper.HandleDBNull(reader["AdvanceLedgerName"]);
                                        nvo.AppConfig.Accounts.ConsultationLedgerName = (string) DALHelper.HandleDBNull(reader["ConsultationLedgerName"]);
                                        nvo.AppConfig.Accounts.DiagnosticLedgerName = (string) DALHelper.HandleDBNull(reader["DiagnosticLedgerName"]);
                                        nvo.AppConfig.Accounts.OtherServicesLedgerName = (string) DALHelper.HandleDBNull(reader["OtherServicesLedgerName"]);
                                        nvo.AppConfig.Accounts.PurchaseLedgerName = (string) DALHelper.HandleDBNull(reader["PurchaseLedgerName"]);
                                        nvo.AppConfig.Accounts.COGSLedgerName = (string) DALHelper.HandleDBNull(reader["COGSLedgerName"]);
                                        nvo.AppConfig.Accounts.ProfitLedgerName = (string) DALHelper.HandleDBNull(reader["ProfitLedgerName"]);
                                        nvo.AppConfig.Accounts.ScrapIncomeLedgerName = (string) DALHelper.HandleDBNull(reader["ScrapIncomeLedgerName"]);
                                        nvo.AppConfig.Accounts.CurrentAssetLedgerName = (string) DALHelper.HandleDBNull(reader["CurrentAssetLedgerName"]);
                                        nvo.AppConfig.Accounts.ExpenseLedgerName = (string) DALHelper.HandleDBNull(reader["ExpenseLedgerName"]);
                                        nvo.AppConfig.Accounts.ItemScrapCategory = (long) DALHelper.HandleDBNull(reader["ItemScrapCategory"]);
                                        nvo.AppConfig.pathonormalColorCode = Convert.ToString(DALHelper.HandleDBNull(reader["PathoNormalValueColor"]));
                                        nvo.AppConfig.pathominColorCode = Convert.ToString(DALHelper.HandleDBNull(reader["PathoMinValueColor"]));
                                        nvo.AppConfig.pathomaxColorCode = Convert.ToString(DALHelper.HandleDBNull(reader["PathoMaxValueColor"]));
                                        nvo.AppConfig.SampleNoLevel = Convert.ToInt64(DALHelper.HandleDBNull(reader["SampleNoLevel"]));
                                        nvo.AppConfig.FirstLevelResultColor = Convert.ToString(DALHelper.HandleDBNull(reader["FirstLevelResultColor"]));
                                        nvo.AppConfig.SecondLevelResultColor = Convert.ToString(DALHelper.HandleDBNull(reader["SecondLevelResultColor"]));
                                        nvo.AppConfig.ThirdLevelResultColor = Convert.ToString(DALHelper.HandleDBNull(reader["ThirdLevelResultColor"]));
                                        nvo.AppConfig.CheckResultColor = Convert.ToString(DALHelper.HandleDBNull(reader["CheckResultColor"]));
                                    }
                                    break;
                                }
                                clsAppConfigAutoEmailSMSVO item = new clsAppConfigAutoEmailSMSVO {
                                    ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                    UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                                    EventID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EventId"])),
                                    AppEmail = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmailTemplateID"])),
                                    AppSMS = Convert.ToInt64(DALHelper.HandleDBNull(reader["SMSTemplateID"])),
                                    EmailTemplatName = Convert.ToString(DALHelper.HandleDBNull(reader["EmailTemplate"])),
                                    SMSTemplatName = Convert.ToString(DALHelper.HandleDBNull(reader["SMSTemplate"])),
                                    EventType = Convert.ToString(DALHelper.HandleDBNull(reader["EventName"])),
                                    SendEmailId = Convert.ToString(DALHelper.HandleDBNull(reader["EmailID"]))
                                };
                                nvo.AppEmail.Add(item);
                            }
                            break;
                        }
                        nvo.AppConfig.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.AppConfig.IsConfigured = (bool) DALHelper.HandleDBNull(reader["IsConfigured"]);
                        nvo.AppConfig.UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]);
                        nvo.AppConfig.UnitName = (string) DALHelper.HandleDBNull(reader["Name"]);
                        nvo.AppConfig.DoctorID = (long) DALHelper.HandleDBNull(reader["DoctorID"]);
                        nvo.AppConfig.DepartmentID = (long) DALHelper.HandleDBNull(reader["DepartmentID"]);
                        nvo.AppConfig.PatientCategoryID = (long) DALHelper.HandleDBNull(reader["PatientCategoryID"]);
                        nvo.AppConfig.PharmacyPatientCategoryID = (long) DALHelper.HandleDBNull(reader["PharmacyPatientCategoryID"]);
                        nvo.AppConfig.Country = (string) DALHelper.HandleDBNull(reader["Country"]);
                        nvo.AppConfig.State = (string) DALHelper.HandleDBNull(reader["State"]);
                        nvo.AppConfig.District = (string) DALHelper.HandleDBNull(reader["District"]);
                        nvo.AppConfig.Pincode = (string) DALHelper.HandleDBNull(reader["Pincode"]);
                        nvo.AppConfig.City = (string) DALHelper.HandleDBNull(reader["City"]);
                        nvo.AppConfig.Area = (string) DALHelper.HandleDBNull(reader["Area"]);
                        nvo.AppConfig.VisitTypeID = (long) DALHelper.HandleDBNull(reader["VisitTypeID"]);
                        nvo.AppConfig.PatientSourceID = (long) DALHelper.HandleDBNull(reader["PatientSourceID"]);
                        nvo.AppConfig.RadiologyStoreID = (long) DALHelper.HandleDBNull(reader["RadiologyStoreID"]);
                        nvo.AppConfig.PathologyStoreID = (long) DALHelper.HandleDBNull(reader["PathologyStoreID"]);
                        nvo.AppConfig.OTStoreID = (long) DALHelper.HandleDBNull(reader["OTStoreID"]);
                        nvo.AppConfig.AppointmentSlot = (double) DALHelper.HandleDBNull(reader["AppointmentSlot"]);
                        nvo.AppConfig.IsHO = (bool) DALHelper.HandleDBNull(reader["IsHO"]);
                        nvo.AppConfig.Email = (string) DALHelper.HandleDBNull(reader["Email"]);
                        nvo.AppConfig.NurseRoleID = (long) DALHelper.HandleDBNull(reader["NurseRoleID"]);
                        nvo.AppConfig.AdminRoleID = (long) DALHelper.HandleDBNull(reader["AdminRoleID"]);
                        nvo.AppConfig.DoctorRoleID = (long) DALHelper.HandleDBNull(reader["DoctorRoleID"]);
                        nvo.AppConfig.ConftnMsgForAdd = (bool) DALHelper.HandleDBNull(reader["ConftnMsgForAdd"]);
                        nvo.AppConfig.SearchPatientsInterval = (short?) DALHelper.HandleDBNull(reader["SearchPatientsInterval"]);
                        nvo.AppConfig.PaymentModeID = (long) DALHelper.HandleDBNull(reader["PaymentModeID"]);
                        nvo.AppConfig.PathoSpecializationID = (long) DALHelper.HandleDBNull(reader["PathoSpecializationID"]);
                        nvo.AppConfig.TariffID = (long) DALHelper.HandleDBNull(reader["SelfTariffID"]);
                        nvo.AppConfig.SelfCompanyID = (long) DALHelper.HandleDBNull(reader["SelfCompanyID"]);
                        nvo.AppConfig.FreeFollowupDays = (short) DALHelper.HandleDBNull(reader["FreeFollowupDays"]);
                        nvo.AppConfig.FreeFollowupVisitTypeID = (long) DALHelper.HandleDBNull(reader["FreeFollowupVisitTypeID"]);
                        nvo.AppConfig.PharmacyVisitTypeID = (long) DALHelper.HandleDBNull(reader["PharmacyVisitTypeID"]);
                        nvo.AppConfig.PathologyVisitTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathologyVisitTypeID"]));
                        nvo.AppConfig.PathologyCompanyTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathologyCompanyTypeID"]));
                        nvo.AppConfig.SelfRelationID = (long) DALHelper.HandleDBNull(reader["SelfRelationID"]);
                        nvo.AppConfig.RadiologySpecializationID = (long) DALHelper.HandleDBNull(reader["RadiologySpecializationID"]);
                        nvo.AppConfig.PrintFormatID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PrintFormatID"]));
                        nvo.AppConfig.PharmacyStoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PharmacyStoreID"]));
                        nvo.AppConfig.IsCentralPurchaseStore = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCentralPurchaseStore"]));
                        nvo.AppConfig.IndentStoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentStoreID"]));
                        nvo.AppConfig.RefundPayModeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RefundMode"]));
                        nvo.AppConfig.RefundAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["RefundAmount"]));
                        nvo.AppConfig.EmbryologistID = (long) DALHelper.HandleDBNull(reader["EmbryologistID"]);
                        nvo.AppConfig.AnesthetistID = (long) DALHelper.HandleDBNull(reader["AnesthetistID"]);
                        nvo.AppConfig.RadiologistID = (long) DALHelper.HandleDBNull(reader["RadiologistID"]);
                        nvo.AppConfig.GynecologistID = (long) DALHelper.HandleDBNull(reader["GynecologistID"]);
                        nvo.AppConfig.PathologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathologistID"]));
                        nvo.AppConfig.PhysicianID = (long) DALHelper.HandleDBNull(reader["PhysicianID"]);
                        nvo.AppConfig.AndrologistID = (long) DALHelper.HandleDBNull(reader["AndriologistID"]);
                        nvo.AppConfig.BiologistID = (long) DALHelper.HandleDBNull(reader["BiologistID"]);
                        nvo.AppConfig.CountryID = (long) DALHelper.HandleDBNull(reader["CountryID"]);
                        nvo.AppConfig.StateID = (long) DALHelper.HandleDBNull(reader["StateID"]);
                        nvo.AppConfig.CityID = (long) DALHelper.HandleDBNull(reader["CityID"]);
                        nvo.AppConfig.RegionID = (long) DALHelper.HandleDBNull(reader["RegionID"]);
                        nvo.AppConfig.IsSellBySellingUnit = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSellBySellingUnit"]));
                        nvo.AppConfig.Region = (string) DALHelper.HandleDBNull(reader["Region"]);
                        nvo.AppConfig.RegionCode = (string) DALHelper.HandleDBNull(reader["RegionCode"]);
                        nvo.AppConfig.CityN = (string) DALHelper.HandleDBNull(reader["CityN"]);
                        nvo.AppConfig.CityCode = (string) DALHelper.HandleDBNull(reader["CityCode"]);
                        nvo.AppConfig.StateN = (string) DALHelper.HandleDBNull(reader["StateN"]);
                        nvo.AppConfig.StateCode = (string) DALHelper.HandleDBNull(reader["StateCode"]);
                        nvo.AppConfig.CountryN = (string) DALHelper.HandleDBNull(reader["CountryN"]);
                        nvo.AppConfig.CountryCode = (string) DALHelper.HandleDBNull(reader["CountryCode"]);
                        nvo.AppConfig.InhouseLabID = (long) DALHelper.HandleDBNull(reader["InhouseLabID"]);
                        nvo.AppConfig.OocyteDonationID = (long) DALHelper.HandleDBNull(reader["OocyteDonationID"]);
                        nvo.AppConfig.OocyteReceipentID = (long) DALHelper.HandleDBNull(reader["OocyteReceipentID"]);
                        nvo.AppConfig.EmbryoReceipentID = (long) DALHelper.HandleDBNull(reader["EmbryoReceipentID"]);
                        nvo.AppConfig.DoctorTypeForReferral = (long) DALHelper.HandleDBNull(reader["DoctorTypeForReferral"]);
                        nvo.AppConfig.IdentityForInternationalPatient = (long) DALHelper.HandleDBNull(reader["IdentityForInternationalPatient"]);
                        nvo.AppConfig.AuthorizationLevelForRefundID = (long) DALHelper.HandleDBNull(reader["AuthorizationLevelForRefundID"]);
                        nvo.AppConfig.AuthorizationLevelForConcessionID = (long) DALHelper.HandleDBNull(reader["AuthorizationLevelForConcessionID"]);
                        nvo.AppConfig.AuthorizationLevelForPatientAdvRefundID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AuthorizationLevelForPatientAdvRefundID"]));
                        nvo.AppConfig.AuthorizationLevelForMRPAdjustmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AuthorizationLevelForMRPAdjustmentID"]));
                        nvo.AppConfig.AddressTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AddressTypeID"]));
                        nvo.AppConfig.MaritalStatus = Convert.ToInt64(DALHelper.HandleDBNull(reader["MaritalStatus"]));
                        nvo.AppConfig.CompanyPatientSourceID = (long) DALHelper.HandleDBNull(reader["CompanyPatientSourceID"]);
                        nvo.AppConfig.FileName = (string) DALHelper.HandleDBNull(reader["LocalLanguageFileName"]);
                        nvo.AppConfig.FilePath = (string) DALHelper.HandleDBNull(reader["LocalLanguageFilePath"]);
                        nvo.AppConfig.ConsultationID = (long) DALHelper.HandleDBNull(reader["ConsultationId"]);
                        nvo.AppConfig.PharmacySpecializationID = (long) DALHelper.HandleDBNull(reader["PharmacySpecializationID"]);
                        nvo.AppConfig.ApplyConcessionToStaff = (bool) DALHelper.HandleDBNull(reader["ApplyConcessionToStaff"]);
                        nvo.AppConfig.AllowClinicalTransaction = (bool) DALHelper.HandleDBNull(reader["AllowClinicalTransaction"]);
                        nvo.AppConfig.AutoDeductStockFromRadiology = (bool) DALHelper.HandleDBNull(reader["AutoDeductStockFromRadiology"]);
                        nvo.AppConfig.AutoGenerateSampleNo = (bool) DALHelper.HandleDBNull(reader["AutoGenerateSampleNo"]);
                        nvo.AppConfig.AutoDeductStockFromPathology = (bool) DALHelper.HandleDBNull(reader["AutoDeductStockFromPathology"]);
                        nvo.AppConfig.DateFormatID = DALHelper.HandleIntegerNull(reader["DateFormatID"]);
                        nvo.AppConfig.AddLogoToAllReports = (bool) DALHelper.HandleDBNull(reader["AddLogoToAllReports"]);
                        nvo.AppConfig.AttachmentFileName = (string) DALHelper.HandleDBNull(reader["AttachmentFileName"]);
                        nvo.AppConfig.Attachment = (byte[]) DALHelper.HandleDBNull(reader["Attachment"]);
                        nvo.AppConfig.CreditLimitIPD = DALHelper.HandleIntegerNull(reader["IpdCreditLimit"]);
                        nvo.AppConfig.CreditLimitOPD = DALHelper.HandleIntegerNull(reader["OpdCreditLimit"]);
                        nvo.AppConfig.ItemExpiredIndays = DALHelper.HandleIntegerNull(reader["ItemExpiredIndays"]);
                        nvo.AppConfig.DefaultCountryCode = (string) DALHelper.HandleDBNull(reader["DefaultCountryCode"]);
                        nvo.AppConfig.LabCounterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabCounterID"]));
                        nvo.AppConfig.IPDCounterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IPDCounterID"]));
                        nvo.AppConfig.OPDCounterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OPDCounterID"]));
                        nvo.AppConfig.PharmacyCounterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PharmacyCounterID"]));
                        nvo.AppConfig.RadiologyCounterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RadiologyCounterID"]));
                        nvo.AppConfig.IsCounterLogin = (bool) DALHelper.HandleDBNull(reader["IsCounterLogin"]);
                        nvo.AppConfig.IsProcessingUnit = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsProcessingUnit"]));
                        nvo.AppConfig.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryID"]));
                        nvo.AppConfig.Disclaimer = Convert.ToString(DALHelper.HandleDBNull(reader["Disclaimer"]));
                        nvo.AppConfig.WebSite = Convert.ToString(DALHelper.HandleDBNull(reader["WebSite"]));
                        nvo.AppConfig.IsPhotoMoveToServer = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPhotoMoveToServer"]));
                        nvo.AppConfig.IsIVFBillingCriteria = this.IsIVFBillingCriteria;
                        nvo.AppConfig.ForIVFProcedureBilling = this.ForIVFProcedureBilling;
                        nvo.AppConfig.ForTriggerProcedureBilling = this.ForTriggerProcedureBilling;
                        nvo.AppConfig.ForOPUProcedureBilling = this.ForOPUProcedureBilling;
                        nvo.AppConfig.IsConcessionReadOnly = this.IsConcessionReadOnly;
                        nvo.AppConfig.IsFertilityPoint = this.IsFertilityPoint;
                        nvo.AppConfig.IsZivaFertility = this.IsZivaFertility;
                        nvo.AppConfig.ValidationsFlag = this.ValidationsFlag;
                        nvo.AppConfig.InternationalId = this.InternationalId;
                        if (nvo.AppConfig.DateFormatID > 0L)
                        {
                            long dateFormatID = nvo.AppConfig.DateFormatID;
                            if ((dateFormatID <= 3L) && (dateFormatID >= 1L))
                            {
                                switch (((int) (dateFormatID - 1L)))
                                {
                                    case 0:
                                        nvo.AppConfig.DateFormat = "dd/MM/yyyy";
                                        break;

                                    case 1:
                                        nvo.AppConfig.DateFormat = "dd-MMM-yy";
                                        break;

                                    case 2:
                                        nvo.AppConfig.DateFormat = "M/d/yyyy";
                                        break;

                                    default:
                                        break;
                                }
                            }
                        }
                        nvo.AppConfig.AttachmentFileName = (string) DALHelper.HandleDBNull(reader["AttachmentFileName"]);
                        nvo.AppConfig.Attachment = (byte[]) DALHelper.HandleDBNull(reader["Attachment"]);
                        nvo.AppConfig.Host = Security.base64Decode((string) DALHelper.HandleDBNull(reader["Host"]));
                        nvo.AppConfig.Port = (int) DALHelper.HandleDBNull(reader["Port"]);
                        nvo.AppConfig.UserName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["UserName"]));
                        nvo.AppConfig.Password = Security.base64Decode((string) DALHelper.HandleDBNull(reader["Password"]));
                        nvo.AppConfig.EnableSsl = (bool) DALHelper.HandleDBNull(reader["EnableSsl"]);
                        nvo.AppConfig.SMSUrl = Security.base64Decode((string) DALHelper.HandleDBNull(reader["SMS_Url"]));
                        nvo.AppConfig.SMS_UserName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["SMS_UserName"]));
                        nvo.AppConfig.SMSPassword = Security.base64Decode((string) DALHelper.HandleDBNull(reader["SMS_Password"]));
                        nvo.AppConfig.IsAllowDischargeRequest = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAllowDischargeRequest"]));
                        nvo.AppConfig.ApplicationDateTime = Convert.ToDateTime(DALHelper.HandleDate(reader["ApplicationDateTime"]));
                        nvo.AppConfig.PathologyDepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathologyDepartmentID"]));
                        nvo.AppConfig.RadiologyDepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RadiologyDepartmentID"]));
                        nvo.AppConfig.BillingExceedsLimit = Convert.ToDouble(DALHelper.HandleDBNull(reader["BillingExceedsLimit"]));
                        nvo.AppConfig.RefundToAdvancePayModeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RefundToAdvancePayModeID"]));
                        nvo.AppConfig.EMRModVisitDateInDays = this.EMRModVisitDateInDays;
                        nvo.AppConfig.PatientDailyCashLimit = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PatientDailyCashLimit"]));
                        nvo.AppConfig.CounterSaleBillAdresslimit = Convert.ToDecimal(DALHelper.HandleDBNull(reader["CounterSaleBillAdresslimit"]));
                    }
                }
                reader.Close();
            }
            catch (Exception exception)
            {
                nvo.Error = exception.Message;
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
            }
            return nvo;
        }

        public override IValueObject GetAutoEmailCCTOConfig(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAppEmailCCToBizActionVo vo = valueObject as clsAppEmailCCToBizActionVo;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetEmailCCTO");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, vo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "configAutoEmailID", DbType.Int64, vo.ConfigAutoEmailID);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows && (vo.ItemList == null))
                {
                    vo.ItemList = new List<clsAppEmailCCToVo>();
                }
                while (reader.Read())
                {
                    clsAppEmailCCToVo item = new clsAppEmailCCToVo {
                        ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                        UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                        ConfigAutoEmailID = (long) DALHelper.HandleDBNull(reader["ConfigAutoEmailID"]),
                        CCToEmailID = (string) DALHelper.HandleDBNull(reader["CCToEmailID"]),
                        Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                    };
                    vo.ItemList.Add(item);
                }
            }
            catch (Exception)
            {
            }
            return valueObject;
        }

        public override IValueObject GetAutoEmailConfig(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetAutoEmailConfigBizActionVO nvo = valueObject as clsGetAutoEmailConfigBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAutoEmailConfig");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Details == null)
                    {
                        nvo.Details = new clsAutoEmailConfigVO();
                    }
                    while (reader.Read())
                    {
                        nvo.Details.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.Details.AppointmentConfirmation = (bool) DALHelper.HandleDBNull(reader["AppointmentConfirmation"]);
                        nvo.Details.AppointmentCancellation = (bool) DALHelper.HandleDBNull(reader["AppointmentCancellation"]);
                        nvo.Details.AutogeneratedPassword = (bool) DALHelper.HandleDBNull(reader["AutogeneratedPassword"]);
                        nvo.Details.CampInformation = (bool) DALHelper.HandleDBNull(reader["CampInformation"]);
                    }
                }
                reader.Close();
            }
            catch (Exception exception)
            {
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return nvo;
        }

        public override IValueObject SetStatusAutoEmailCCTO(IValueObject valueObject, clsUserVO UserVo)
        {
            clsStatusEmailCCToBizActionVo vo = valueObject as clsStatusEmailCCToBizActionVo;
            try
            {
                DbCommand storedProcCommand = null;
                clsAppEmailCCToVo appEmailCC = vo.AppEmailCC;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_Update_StatusEmailCCTo");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, appEmailCC.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, appEmailCC.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ConfigAutoEmailID", DbType.Int64, appEmailCC.ConfigAutoEmailID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Int64, appEmailCC.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, appEmailCC.UpdatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, appEmailCC.UpdatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, appEmailCC.UpdatedOn);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, appEmailCC.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, appEmailCC.UpdateWindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                vo.ResultStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return vo;
        }

        public override IValueObject UpdateAppConfig(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateAppConfigBizActionVO nvo = valueObject as clsUpdateAppConfigBizActionVO;
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                clsAppConfigVO appConfig = nvo.AppConfig;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateApplicationConfig");
                this.dbServer.AddInParameter(storedProcCommand, "DateFormatID", DbType.Int64, appConfig.DateFormatID);
                this.dbServer.AddInParameter(storedProcCommand, "IsConfigured", DbType.Boolean, appConfig.IsConfigured);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, appConfig.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, appConfig.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, appConfig.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientCategoryID", DbType.Int64, appConfig.PatientCategoryID);
                this.dbServer.AddInParameter(storedProcCommand, "PharmacyPatientCategoryID", DbType.Int64, appConfig.PharmacyPatientCategoryID);
                this.dbServer.AddInParameter(storedProcCommand, "Country", DbType.String, appConfig.Country);
                this.dbServer.AddInParameter(storedProcCommand, "State", DbType.String, appConfig.State);
                this.dbServer.AddInParameter(storedProcCommand, "District", DbType.String, appConfig.District);
                this.dbServer.AddInParameter(storedProcCommand, "Pincode", DbType.String, appConfig.Pincode);
                this.dbServer.AddInParameter(storedProcCommand, "City", DbType.String, appConfig.City);
                this.dbServer.AddInParameter(storedProcCommand, "Area", DbType.String, appConfig.Area);
                this.dbServer.AddInParameter(storedProcCommand, "VisitTypeID", DbType.Int64, appConfig.VisitTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientSourceID", DbType.Int64, appConfig.PatientSourceID);
                this.dbServer.AddInParameter(storedProcCommand, "AppointmentSlot", DbType.Double, appConfig.AppointmentSlot);
                this.dbServer.AddInParameter(storedProcCommand, "IsHO", DbType.Boolean, appConfig.IsHO);
                this.dbServer.AddInParameter(storedProcCommand, "DatabaseName", DbType.String, appConfig.DatabaseName);
                this.dbServer.AddInParameter(storedProcCommand, "Email", DbType.String, appConfig.Email);
                this.dbServer.AddInParameter(storedProcCommand, "NurseRoleID", DbType.Int64, appConfig.NurseRoleID);
                this.dbServer.AddInParameter(storedProcCommand, "AdminRoleID", DbType.Int64, appConfig.AdminRoleID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorRoleID", DbType.Int64, appConfig.DoctorRoleID);
                this.dbServer.AddInParameter(storedProcCommand, "ConftnMsgForAdd", DbType.Boolean, appConfig.ConftnMsgForAdd);
                this.dbServer.AddInParameter(storedProcCommand, "SearchPatientsInterval", DbType.Int16, appConfig.SearchPatientsInterval);
                this.dbServer.AddInParameter(storedProcCommand, "PaymentModeID", DbType.Int64, appConfig.PaymentModeID);
                this.dbServer.AddInParameter(storedProcCommand, "PathoSpecializationID", DbType.Int64, appConfig.PathoSpecializationID);
                this.dbServer.AddInParameter(storedProcCommand, "PharmacySpecializationId", DbType.Int64, appConfig.PharmacySpecializationID);
                this.dbServer.AddInParameter(storedProcCommand, "ConsultationId", DbType.Int64, appConfig.ConsultationID);
                this.dbServer.AddInParameter(storedProcCommand, "SelfCompanyID", DbType.Int64, appConfig.SelfCompanyID);
                this.dbServer.AddInParameter(storedProcCommand, "SelfTariffID", DbType.Int64, appConfig.TariffID);
                this.dbServer.AddInParameter(storedProcCommand, "FreeFollowupDays", DbType.Int16, appConfig.FreeFollowupDays);
                this.dbServer.AddInParameter(storedProcCommand, "FreeFollowupVisitTypeID", DbType.Int16, appConfig.FreeFollowupVisitTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "PharmacyVisitTypeID", DbType.Int64, appConfig.PharmacyVisitTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "PathologyVisitTypeID", DbType.Int64, appConfig.PathologyVisitTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "PathologyCompanyTypeID", DbType.Int64, appConfig.PathologyCompanyTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "RadiologySpecializationID", DbType.Int64, appConfig.RadiologySpecializationID);
                this.dbServer.AddInParameter(storedProcCommand, "RefundMode", DbType.Int64, appConfig.RefundPayModeID);
                this.dbServer.AddInParameter(storedProcCommand, "RefundAmount", DbType.Double, appConfig.RefundAmount);
                this.dbServer.AddInParameter(storedProcCommand, "EmbryologistID", DbType.Int64, appConfig.EmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AnesthetistID", DbType.Int64, appConfig.AnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "RadiologistID", DbType.Int64, appConfig.RadiologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AndriologistID", DbType.Int64, appConfig.AndrologistID);
                this.dbServer.AddInParameter(storedProcCommand, "GynecologistID", DbType.Int64, appConfig.GynecologistID);
                this.dbServer.AddInParameter(storedProcCommand, "PhysicianID", DbType.Int64, appConfig.PhysicianID);
                this.dbServer.AddInParameter(storedProcCommand, "BiologistID", DbType.Int64, appConfig.BiologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AddressTypeID", DbType.Int64, appConfig.AddressTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "CountryID", DbType.Int64, appConfig.CountryID);
                this.dbServer.AddInParameter(storedProcCommand, "StateID", DbType.Int64, appConfig.StateID);
                this.dbServer.AddInParameter(storedProcCommand, "CityID", DbType.Int64, appConfig.CityID);
                this.dbServer.AddInParameter(storedProcCommand, "RegionID", DbType.Int64, appConfig.RegionID);
                this.dbServer.AddInParameter(storedProcCommand, "IsCentralPurchaseStore", DbType.Boolean, appConfig.IsCentralPurchaseStore);
                this.dbServer.AddInParameter(storedProcCommand, "IndentStoreID", DbType.Int64, appConfig.IndentStoreID);
                this.dbServer.AddInParameter(storedProcCommand, "InhouseLabID", DbType.Int64, appConfig.InhouseLabID);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteDonationID", DbType.Int64, appConfig.OocyteDonationID);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteReceipentID", DbType.Int64, appConfig.OocyteReceipentID);
                this.dbServer.AddInParameter(storedProcCommand, "EmbryoReceipentID", DbType.Int64, appConfig.EmbryoReceipentID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorTypeForReferral", DbType.Int64, appConfig.DoctorTypeForReferral);
                this.dbServer.AddInParameter(storedProcCommand, "IdentityForInternationalPatient", DbType.Int64, appConfig.IdentityForInternationalPatient);
                this.dbServer.AddInParameter(storedProcCommand, "AuthorizationLevelForRefundID", DbType.Int64, appConfig.AuthorizationLevelForRefundID);
                this.dbServer.AddInParameter(storedProcCommand, "AuthorizationLevelForConcessionID", DbType.Int64, appConfig.AuthorizationLevelForConcessionID);
                this.dbServer.AddInParameter(storedProcCommand, "AuthorizationLevelForMRPAdjustmentID", DbType.Int64, appConfig.AuthorizationLevelForMRPAdjustmentID);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyPatientSourceID", DbType.Int64, appConfig.CompanyPatientSourceID);
                this.dbServer.AddInParameter(storedProcCommand, "SelfRelationID", DbType.Int64, appConfig.SelfRelationID);
                this.dbServer.AddInParameter(storedProcCommand, "RadiologyStoreID", DbType.Int64, appConfig.RadiologyStoreID);
                this.dbServer.AddInParameter(storedProcCommand, "PathologyStoreID", DbType.Int64, appConfig.PathologyStoreID);
                this.dbServer.AddInParameter(storedProcCommand, "OTStoreID", DbType.Int64, appConfig.OTStoreID);
                this.dbServer.AddInParameter(storedProcCommand, "PharmacyStoreID", DbType.Int64, appConfig.PharmacyStoreID);
                this.dbServer.AddInParameter(storedProcCommand, "ApplyConcessionToStaff", DbType.Boolean, appConfig.ApplyConcessionToStaff);
                this.dbServer.AddInParameter(storedProcCommand, "AllowClinicalTransaction", DbType.Boolean, appConfig.AllowClinicalTransaction);
                this.dbServer.AddInParameter(storedProcCommand, "AutoDeductStockFromRadiology", DbType.Boolean, appConfig.AutoDeductStockFromRadiology);
                this.dbServer.AddInParameter(storedProcCommand, "AutoDeductStockFromPathology", DbType.Boolean, appConfig.AutoDeductStockFromPathology);
                this.dbServer.AddInParameter(storedProcCommand, "AutoGenerateSampleNo", DbType.Boolean, appConfig.AutoGenerateSampleNo);
                this.dbServer.AddInParameter(storedProcCommand, "AddLogoToAllReports", DbType.Boolean, appConfig.AddLogoToAllReports);
                this.dbServer.AddInParameter(storedProcCommand, "PathologistID", DbType.Int64, appConfig.PathologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AttachmentFileName", DbType.String, appConfig.AttachmentFileName);
                this.dbServer.AddInParameter(storedProcCommand, "Attachment", DbType.Binary, appConfig.Attachment);
                this.dbServer.AddInParameter(storedProcCommand, "Host", DbType.String, Security.base64Encode(appConfig.Host));
                this.dbServer.AddInParameter(storedProcCommand, "Port", DbType.Int32, appConfig.Port);
                this.dbServer.AddInParameter(storedProcCommand, "UserName", DbType.String, Security.base64Encode(appConfig.UserName));
                this.dbServer.AddInParameter(storedProcCommand, "Password", DbType.String, Security.base64Encode(appConfig.Password));
                this.dbServer.AddInParameter(storedProcCommand, "EnableSSL", DbType.Boolean, appConfig.EnableSsl);
                this.dbServer.AddInParameter(storedProcCommand, "SMS_Url", DbType.String, Security.base64Encode(appConfig.SMSUrl));
                this.dbServer.AddInParameter(storedProcCommand, "SMS_UserName", DbType.String, Security.base64Encode(appConfig.SMS_UserName));
                this.dbServer.AddInParameter(storedProcCommand, "SMS_Password", DbType.String, Security.base64Encode(appConfig.SMSPassword));
                this.dbServer.AddInParameter(storedProcCommand, "IsAllowDischargeRequest", DbType.Boolean, appConfig.IsAllowDischargeRequest);
                this.dbServer.AddInParameter(storedProcCommand, "PrintFormatID", DbType.Int64, appConfig.PrintFormatID);
                this.dbServer.AddInParameter(storedProcCommand, "PathologyDepartmentID", DbType.Int64, appConfig.PathologyDepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "RadiologyDepartmentID", DbType.Int64, appConfig.RadiologyDepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "IsIPD", DbType.Boolean, appConfig.IsIPD);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPD", DbType.Boolean, appConfig.IsOPD);
                this.dbServer.AddInParameter(storedProcCommand, "IpdCreditLimit", DbType.Int64, appConfig.CreditLimitIPD);
                this.dbServer.AddInParameter(storedProcCommand, "OpdCreditLimit", DbType.Int64, appConfig.CreditLimitOPD);
                this.dbServer.AddInParameter(storedProcCommand, "ItemExpiredIndays", DbType.Int64, appConfig.ItemExpiredIndays);
                this.dbServer.AddInParameter(storedProcCommand, "DefaultCountryCode", DbType.String, appConfig.DefaultCountryCode);
                this.dbServer.AddInParameter(storedProcCommand, "LabCounterID", DbType.Int64, appConfig.LabCounterID);
                this.dbServer.AddInParameter(storedProcCommand, "IPDCounterID", DbType.Int64, appConfig.IPDCounterID);
                this.dbServer.AddInParameter(storedProcCommand, "OPDCounterID", DbType.Int64, appConfig.OPDCounterID);
                this.dbServer.AddInParameter(storedProcCommand, "PharmacyCounterID", DbType.Int64, appConfig.PharmacyCounterID);
                this.dbServer.AddInParameter(storedProcCommand, "RadiologyCounterID", DbType.Int64, appConfig.RadiologyCounterID);
                this.dbServer.AddInParameter(storedProcCommand, "CategoryID", DbType.Int64, appConfig.CategoryID);
                this.dbServer.AddInParameter(storedProcCommand, "Disclaimer", DbType.String, appConfig.Disclaimer);
                this.dbServer.AddInParameter(storedProcCommand, "WebSite", DbType.String, appConfig.WebSite);
                this.dbServer.AddInParameter(storedProcCommand, "BillingExceedsLimit", DbType.Double, appConfig.BillingExceedsLimit);
                this.dbServer.AddInParameter(storedProcCommand, "PatientDailyCashLimit", DbType.Decimal, appConfig.PatientDailyCashLimit);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteAutoEmailConfig");
                this.dbServer.AddInParameter(command2, "UnitId", DbType.Int64, nvo.AppConfig.UnitID);
                this.dbServer.ExecuteNonQuery(command2, transaction);
                foreach (clsAppConfigAutoEmailSMSVO lsmsvo in nvo.AppEmail)
                {
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_UpdateAutoEmailConfig_New");
                    this.dbServer.AddInParameter(command3, "ID", DbType.Int64, lsmsvo.ID);
                    this.dbServer.AddInParameter(command3, "UnitId", DbType.Int64, nvo.AppConfig.UnitID);
                    this.dbServer.AddInParameter(command3, "EventId", DbType.Int64, lsmsvo.EventID);
                    this.dbServer.AddInParameter(command3, "EmailTemplateId", DbType.Int64, lsmsvo.AppEmail);
                    this.dbServer.AddInParameter(command3, "SMSTemplateId", DbType.Int64, lsmsvo.AppSMS);
                    this.dbServer.AddInParameter(command3, "EmailId", DbType.String, lsmsvo.SendEmailId);
                    this.dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, lsmsvo.CreatedUnitID);
                    this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, lsmsvo.AddedBy);
                    this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, lsmsvo.AddedOn);
                    this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, lsmsvo.AddedDateTime);
                    this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, lsmsvo.AddedWindowsLoginName);
                    this.dbServer.AddInParameter(command3, "UpdatedUnitID", DbType.Int64, lsmsvo.UpdatedUnitID);
                    this.dbServer.AddInParameter(command3, "UpdatedBy", DbType.Int64, lsmsvo.UpdatedBy);
                    this.dbServer.AddInParameter(command3, "UpdatedOn", DbType.String, lsmsvo.UpdatedOn);
                    this.dbServer.AddInParameter(command3, "UpdatedDateTime", DbType.DateTime, lsmsvo.UpdatedDateTime);
                    this.dbServer.AddInParameter(command3, "UpdateWindowsLoginName", DbType.String, lsmsvo.UpdatedWindowsLoginName);
                    this.dbServer.ExecuteNonQuery(command3, transaction);
                }
                DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_UpdateApplicationAccountsConfig");
                this.dbServer.AddInParameter(command4, "ID", DbType.Int64, appConfig.ID);
                this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, appConfig.UnitID);
                this.dbServer.AddInParameter(command4, "ChequeDDBankID", DbType.Int64, appConfig.Accounts.ChequeDDBankID);
                this.dbServer.AddInParameter(command4, "CrDBBankID", DbType.Int64, appConfig.Accounts.CrDBBankID);
                this.dbServer.AddInParameter(command4, "CashLedgerName", DbType.String, appConfig.Accounts.CashLedgerName);
                this.dbServer.AddInParameter(command4, "AdvanceLedgerName", DbType.String, appConfig.Accounts.AdvanceLedgerName);
                this.dbServer.AddInParameter(command4, "ConsultationLedgerName", DbType.String, appConfig.Accounts.ConsultationLedgerName);
                this.dbServer.AddInParameter(command4, "DiagnosticLedgerName", DbType.String, appConfig.Accounts.DiagnosticLedgerName);
                this.dbServer.AddInParameter(command4, "OtherServicesLedgerName", DbType.String, appConfig.Accounts.OtherServicesLedgerName);
                this.dbServer.AddInParameter(command4, "PurchaseLedgerName", DbType.String, appConfig.Accounts.PurchaseLedgerName);
                this.dbServer.AddInParameter(command4, "COGSLedgerName", DbType.String, appConfig.Accounts.COGSLedgerName);
                this.dbServer.AddInParameter(command4, "ProfitLedgerName", DbType.String, appConfig.Accounts.ProfitLedgerName);
                this.dbServer.AddInParameter(command4, "ScrapIncomeLedgerName", DbType.String, appConfig.Accounts.ScrapIncomeLedgerName);
                this.dbServer.AddInParameter(command4, "CurrentAssetLedgerName", DbType.String, appConfig.Accounts.CurrentAssetLedgerName);
                this.dbServer.AddInParameter(command4, "ExpenseLedgerName", DbType.String, appConfig.Accounts.ExpenseLedgerName);
                this.dbServer.AddInParameter(command4, "ItemScrapCategory ", DbType.Int64, appConfig.Accounts.ItemScrapCategory);
                this.dbServer.ExecuteNonQuery(command4, transaction);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.AppConfig = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return valueObject;
        }

        public override IValueObject UpdateAutoEmailConfig(IValueObject valueObject, clsUserVO UserVo)
        {
            try
            {
                clsAutoEmailConfigVO details = (valueObject as clsUpdateAutoEmailConfigBizActionVO).Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateAutoEmailConfig");
                this.dbServer.AddInParameter(storedProcCommand, "AppointmentConfirmation", DbType.Boolean, details.AppointmentConfirmation);
                this.dbServer.AddInParameter(storedProcCommand, "AppointmentCancellation", DbType.Boolean, details.AppointmentCancellation);
                this.dbServer.AddInParameter(storedProcCommand, "AutogeneratedPassword", DbType.Boolean, details.AutogeneratedPassword);
                this.dbServer.AddInParameter(storedProcCommand, "CampInformation", DbType.Boolean, details.CampInformation);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }
    }
}

