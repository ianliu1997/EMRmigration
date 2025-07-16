namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration;
    using PalashDynamics.ValueObjects.Administration.PatientSourceMaster;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;

    public class clsServiceMasterDAL : clsBaseServiceMasterDAL
    {
        private Database dbServer;

        private clsServiceMasterDAL()
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

        public override IValueObject AddApplyLevelToService(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateApplyLevelsToServiceBizActionVO nvo = valueObject as clsAddUpdateApplyLevelsToServiceBizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                clsServiceLevelsVO svo1 = nvo.Obj;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateServiceLevels");
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.Obj.ServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceUnitID", DbType.Int64, nvo.Obj.ServiceUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "L1ID", DbType.Int64, nvo.Obj.L1ID);
                this.dbServer.AddInParameter(storedProcCommand, "L2ID", DbType.Int64, nvo.Obj.L2ID);
                this.dbServer.AddInParameter(storedProcCommand, "L3ID", DbType.Int64, nvo.Obj.L3ID);
                this.dbServer.AddInParameter(storedProcCommand, "L4ID", DbType.Int64, nvo.Obj.L4ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.Obj.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        private clsAddUpdateCompanyDetailsBizActionVO AddCompanyMaster(clsAddUpdateCompanyDetailsBizActionVO BizActionObj, clsUserVO objUserVO)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsCompanyVO itemMatserDetails = BizActionObj.ItemMatserDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddCompanyMaster");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, itemMatserDetails.Description);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, itemMatserDetails.Code);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyTypeId", DbType.Int64, itemMatserDetails.CompanyTypeId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientCatagoryID", DbType.Int64, itemMatserDetails.PatientCatagoryID);
                this.dbServer.AddInParameter(storedProcCommand, "ContactPerson", DbType.String, itemMatserDetails.ContactPerson);
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo", DbType.String, itemMatserDetails.ContactNo);
                this.dbServer.AddInParameter(storedProcCommand, "EmailId", DbType.String, itemMatserDetails.CompanyEmail);
                this.dbServer.AddInParameter(storedProcCommand, "Address", DbType.String, itemMatserDetails.CompanyAddress);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, itemMatserDetails.Id);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.ItemMatserDetails.Id = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                foreach (clsTariffDetailsVO svo in itemMatserDetails.TariffDetails)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddCompanyTariffDetails");
                    this.dbServer.AddInParameter(command2, "CompanyID", DbType.Int64, itemMatserDetails.Id);
                    this.dbServer.AddInParameter(command2, "TariffID", DbType.Int64, svo.TariffID);
                    this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command2, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    svo.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                }
                DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddCompanyLogoDetails");
                this.dbServer.AddInParameter(command3, "CompanyID", DbType.Int64, itemMatserDetails.Id);
                this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(command3, "CompanyLogoFile", DbType.Binary, BizActionObj.ItemMatserDetails.AttachedFileContent);
                this.dbServer.AddInParameter(command3, "LogoFileName", DbType.String, BizActionObj.ItemMatserDetails.AttachedFileName);
                this.dbServer.AddInParameter(command3, "Title", DbType.String, BizActionObj.ItemMatserDetails.Title);
                this.dbServer.AddInParameter(command3, "CompHeadImgFile", DbType.Binary, BizActionObj.ItemMatserDetails.AttachedHeadImgFileContent);
                this.dbServer.AddInParameter(command3, "HeadImgFileName", DbType.String, BizActionObj.ItemMatserDetails.AttachedHeadImgFileName);
                this.dbServer.AddInParameter(command3, "TitleHeadImg", DbType.String, BizActionObj.ItemMatserDetails.TitleHeaderImage);
                this.dbServer.AddInParameter(command3, "CompFootImgFile", DbType.Binary, BizActionObj.ItemMatserDetails.AttachedFootImgFileContent);
                this.dbServer.AddInParameter(command3, "FootImgFileName", DbType.String, BizActionObj.ItemMatserDetails.AttachedFootImgFileName);
                this.dbServer.AddInParameter(command3, "TitleFootImg", DbType.String, BizActionObj.ItemMatserDetails.TitleFooterImage);
                this.dbServer.AddInParameter(command3, "HeaderText", DbType.String, BizActionObj.ItemMatserDetails.HeaderText);
                this.dbServer.AddInParameter(command3, "FooterText", DbType.String, BizActionObj.ItemMatserDetails.FooterText);
                this.dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(command3, transaction);
                transaction.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.ItemMatserDetails = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return BizActionObj;
        }

        public override IValueObject AddServiceMaster(IValueObject valueObject, clsUserVO userVO)
        {
            DbConnection con = null;
            DbTransaction transaction = null;
            clsAddServiceMasterBizActionVO nvo = valueObject as clsAddServiceMasterBizActionVO;
            try
            {
                con = this.dbServer.CreateConnection();
                con.Open();
                transaction = con.BeginTransaction();
                DbCommand storedProcCommand = null;
                clsServiceMasterVO serviceMasterDetails = nvo.ServiceMasterDetails;
                if (nvo.IsModify)
                {
                    if (nvo.IsOLDServiceMaster)
                    {
                        storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateServiceMaster_OLD");
                    }
                    else
                    {
                        storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateServiceMaster");
                        this.dbServer.AddInParameter(storedProcCommand, "LuxuryTaxAmount", DbType.Decimal, serviceMasterDetails.LuxuryTaxAmount);
                        this.dbServer.AddInParameter(storedProcCommand, "LuxuryTaxPercent", DbType.Decimal, serviceMasterDetails.LuxuryTaxPercent);
                    }
                    storedProcCommand.Connection = con;
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ServiceMasterDetails.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, userVO.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, userVO.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);
                }
                else
                {
                    if (nvo.IsOLDServiceMaster)
                    {
                        storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddServiceMaster_OLD");
                    }
                    else
                    {
                        storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddServiceMaster");
                        this.dbServer.AddInParameter(storedProcCommand, "LuxuryTaxAmount", DbType.Decimal, serviceMasterDetails.LuxuryTaxAmount);
                        this.dbServer.AddInParameter(storedProcCommand, "LuxuryTaxPercent", DbType.Decimal, serviceMasterDetails.LuxuryTaxPercent);
                    }
                    storedProcCommand.Connection = con;
                    this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceCode", DbType.String, serviceMasterDetails.ServiceCode);
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, serviceMasterDetails.ServiceID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, userVO.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, userVO.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, serviceMasterDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CodeType", DbType.Int64, serviceMasterDetails.CodeType);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, serviceMasterDetails.Code);
                this.dbServer.AddInParameter(storedProcCommand, "SpecializationId", DbType.Int64, serviceMasterDetails.Specialization);
                this.dbServer.AddInParameter(storedProcCommand, "SubSpecializationId", DbType.Int64, serviceMasterDetails.SubSpecialization);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, serviceMasterDetails.ServiceName);
                this.dbServer.AddInParameter(storedProcCommand, "ShortDescription", DbType.String, serviceMasterDetails.ShortDescription);
                this.dbServer.AddInParameter(storedProcCommand, "LongDescription", DbType.String, serviceMasterDetails.LongDescription);
                this.dbServer.AddInParameter(storedProcCommand, "StaffDiscount", DbType.Boolean, serviceMasterDetails.StaffDiscount);
                this.dbServer.AddInParameter(storedProcCommand, "StaffDiscountAmount", DbType.Decimal, serviceMasterDetails.StaffDiscountAmount);
                this.dbServer.AddInParameter(storedProcCommand, "StaffDiscountPercent", DbType.Decimal, serviceMasterDetails.StaffDiscountPercent);
                this.dbServer.AddInParameter(storedProcCommand, "StaffDependantDiscount", DbType.Boolean, serviceMasterDetails.StaffDependantDiscount);
                this.dbServer.AddInParameter(storedProcCommand, "StaffDependantDiscountAmount", DbType.Decimal, serviceMasterDetails.StaffDependantDiscountAmount);
                this.dbServer.AddInParameter(storedProcCommand, "StaffDependantDiscountPercent", DbType.Decimal, serviceMasterDetails.StaffDependantDiscountPercent);
                this.dbServer.AddInParameter(storedProcCommand, "Concession", DbType.Boolean, serviceMasterDetails.Concession);
                this.dbServer.AddInParameter(storedProcCommand, "ConcessionAmount", DbType.Decimal, serviceMasterDetails.ConcessionAmount);
                this.dbServer.AddInParameter(storedProcCommand, "ConcessionPercent", DbType.Decimal, serviceMasterDetails.ConcessionPercent);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceTax", DbType.Boolean, serviceMasterDetails.ServiceTax);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceTaxAmount", DbType.Decimal, serviceMasterDetails.ServiceTaxAmount);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceTaxPercent", DbType.Decimal, serviceMasterDetails.ServiceTaxPercent);
                this.dbServer.AddInParameter(storedProcCommand, "SeniorCitizen", DbType.Boolean, serviceMasterDetails.SeniorCitizen);
                this.dbServer.AddInParameter(storedProcCommand, "SeniorCitizenConAmount", DbType.Decimal, serviceMasterDetails.SeniorCitizenConAmount);
                this.dbServer.AddInParameter(storedProcCommand, "SeniorCitizenConPercent", DbType.Decimal, serviceMasterDetails.SeniorCitizenConPercent);
                this.dbServer.AddInParameter(storedProcCommand, "SeniorCitizenAge", DbType.Int16, serviceMasterDetails.SeniorCitizenAge);
                this.dbServer.AddInParameter(storedProcCommand, "IsPackage", DbType.Boolean, serviceMasterDetails.IsPackage);
                this.dbServer.AddInParameter(storedProcCommand, "InHouse", DbType.Boolean, serviceMasterDetails.InHouse);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorShare", DbType.Boolean, serviceMasterDetails.DoctorShare);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorSharePercentage", DbType.Decimal, serviceMasterDetails.DoctorSharePercentage);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorShareAmount", DbType.Decimal, serviceMasterDetails.DoctorShareAmount);
                this.dbServer.AddInParameter(storedProcCommand, "RateEditable", DbType.Boolean, serviceMasterDetails.RateEditable);
                this.dbServer.AddInParameter(storedProcCommand, "MaxRate", DbType.Decimal, serviceMasterDetails.MaxRate);
                this.dbServer.AddInParameter(storedProcCommand, "MinRate", DbType.Decimal, serviceMasterDetails.MinRate);
                this.dbServer.AddInParameter(storedProcCommand, "SACCodeID", DbType.Int64, serviceMasterDetails.SACCodeID);
                if (nvo.IsOLDServiceMaster)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Rate", DbType.Decimal, serviceMasterDetails.Rate);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "BaseServiceRate", DbType.Decimal, serviceMasterDetails.Rate);
                    this.dbServer.AddInParameter(storedProcCommand, "IsFavourite", DbType.Boolean, serviceMasterDetails.IsFavourite);
                    this.dbServer.AddInParameter(storedProcCommand, "IslinkWithInventory", DbType.Boolean, serviceMasterDetails.IsLinkWithInventory);
                    this.dbServer.AddInParameter(storedProcCommand, "CodeDetails", DbType.String, serviceMasterDetails.CodeDetails);
                }
                this.dbServer.AddInParameter(storedProcCommand, "CheckedAllTariffs", DbType.Boolean, serviceMasterDetails.CheckedAllTariffs);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, serviceMasterDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, serviceMasterDetails.CreatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, serviceMasterDetails.UpdatedUnitID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                serviceMasterDetails.ID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if (!nvo.IsOLDServiceMaster)
                {
                    if (serviceMasterDetails.ID != 0L)
                    {
                        if (nvo.IsModify)
                        {
                            nvo.ServiceID = serviceMasterDetails.ID;
                            nvo.ServiceMasterDetails.ServiceCode = serviceMasterDetails.ID.ToString();
                        }
                        foreach (clsServiceMasterVO rvo2 in nvo.ServiceClassList)
                        {
                            storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateServiceClassRateDetails");
                            storedProcCommand.Connection = con;
                            this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, 0);
                            this.dbServer.AddInParameter(storedProcCommand, "ServiceId", DbType.Int64, serviceMasterDetails.ID);
                            this.dbServer.AddInParameter(storedProcCommand, "ClassId", DbType.Int64, rvo2.ClassID);
                            this.dbServer.AddInParameter(storedProcCommand, "Rate", DbType.Decimal, rvo2.Rate);
                            this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                            this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, rvo2.UnitID);
                            this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                        }
                    }
                }
                else
                {
                     storedProcCommand = null;
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddServiceClassRateDetails");
                    storedProcCommand.Connection = con;
                    this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, 0);
                    if (serviceMasterDetails.EditMode)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceId", DbType.Int64, nvo.ServiceMasterDetails.ID);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceId", DbType.Int64, serviceMasterDetails.ID);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "ClassId", DbType.Int64, 1);
                    this.dbServer.AddInParameter(storedProcCommand, "Rate", DbType.Decimal, serviceMasterDetails.Rate);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, serviceMasterDetails.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    clsAddTariffServiceBizActionVO nvo2 = new clsAddTariffServiceBizActionVO {
                        ServiceMasterDetails = serviceMasterDetails,
                        TariffList = serviceMasterDetails.TariffIDList
                    };
                    nvo2.ServiceMasterDetails.ServiceCode = serviceMasterDetails.ServiceCode;
                    nvo2.ServiceMasterDetails.ServiceID = serviceMasterDetails.EditMode ? serviceMasterDetails.ID : nvo.ServiceID;
                    nvo2.TariffServiceForm = false;
                    long serviceID = nvo2.ServiceMasterDetails.ServiceID;
                    List<long> serviceTarrifIds = this.GetServiceTarrifIds(serviceID, serviceMasterDetails.UnitID);
                    if (!nvo2.TariffServiceForm)
                    {
                        this.UpdateTariffServiceMaster(serviceTarrifIds, con, transaction);
                        this.UpdateTariffServiceClassRateDetails(serviceTarrifIds, con, transaction);
                    }
                    DbCommand command3 = null;
                    command3 = this.dbServer.GetStoredProcCommand("CIMS_AddTariffService");
                    command3.Connection = con;
                    if ((nvo2.TariffList != null) && (nvo2.TariffList.Count > 0))
                    {
                        for (int i = 0; i <= (nvo2.TariffList.Count - 1); i++)
                        {
                            command3.Parameters.Clear();
                            this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, serviceMasterDetails.UnitID);
                            this.dbServer.AddInParameter(command3, "TariffID", DbType.Int64, Convert.ToInt64(serviceMasterDetails.TariffIDList[i]));
                            this.dbServer.AddInParameter(command3, "ServiceID", DbType.Int64, serviceMasterDetails.ID);
                            this.dbServer.AddInParameter(command3, "ServiceCode", DbType.String, serviceMasterDetails.ServiceCode);
                            this.dbServer.AddInParameter(command3, "SpecializationId", DbType.Int64, serviceMasterDetails.Specialization);
                            this.dbServer.AddInParameter(command3, "SubSpecializationId", DbType.Int64, serviceMasterDetails.SubSpecialization);
                            this.dbServer.AddInParameter(command3, "ShortDescription", DbType.String, serviceMasterDetails.ShortDescription);
                            this.dbServer.AddInParameter(command3, "LongDescription", DbType.String, serviceMasterDetails.LongDescription);
                            this.dbServer.AddInParameter(command3, "Description", DbType.String, serviceMasterDetails.ServiceName);
                            this.dbServer.AddInParameter(command3, "CodeType", DbType.Int64, serviceMasterDetails.CodeType);
                            this.dbServer.AddInParameter(command3, "Code", DbType.String, serviceMasterDetails.Code);
                            this.dbServer.AddInParameter(command3, "StaffDiscount", DbType.Boolean, serviceMasterDetails.StaffDiscount);
                            this.dbServer.AddInParameter(command3, "StaffDiscountAmount", DbType.Decimal, serviceMasterDetails.StaffDiscountAmount);
                            this.dbServer.AddInParameter(command3, "StaffDiscountPercent", DbType.Decimal, serviceMasterDetails.StaffDiscountPercent);
                            this.dbServer.AddInParameter(command3, "StaffDependantDiscount", DbType.Boolean, serviceMasterDetails.StaffDependantDiscount);
                            this.dbServer.AddInParameter(command3, "StaffDependantDiscountAmount", DbType.Decimal, serviceMasterDetails.StaffDependantDiscountAmount);
                            this.dbServer.AddInParameter(command3, "StaffDependantDiscountPercent", DbType.Decimal, serviceMasterDetails.StaffDependantDiscountPercent);
                            this.dbServer.AddInParameter(command3, "Concession", DbType.Boolean, serviceMasterDetails.Concession);
                            this.dbServer.AddInParameter(command3, "ConcessionAmount", DbType.Decimal, serviceMasterDetails.ConcessionAmount);
                            this.dbServer.AddInParameter(command3, "ConcessionPercent", DbType.Decimal, serviceMasterDetails.ConcessionPercent);
                            this.dbServer.AddInParameter(command3, "ServiceTax", DbType.Boolean, serviceMasterDetails.ServiceTax);
                            this.dbServer.AddInParameter(command3, "ServiceTaxAmount", DbType.Decimal, serviceMasterDetails.ServiceTaxAmount);
                            this.dbServer.AddInParameter(command3, "ServiceTaxPercent", DbType.Decimal, serviceMasterDetails.ServiceTaxPercent);
                            this.dbServer.AddInParameter(command3, "SeniorCitizen", DbType.Boolean, serviceMasterDetails.SeniorCitizen);
                            this.dbServer.AddInParameter(command3, "SeniorCitizenConAmount", DbType.Decimal, serviceMasterDetails.SeniorCitizenConAmount);
                            this.dbServer.AddInParameter(command3, "SeniorCitizenConPercent", DbType.Decimal, serviceMasterDetails.SeniorCitizenConPercent);
                            this.dbServer.AddInParameter(command3, "SeniorCitizenAge", DbType.Int16, serviceMasterDetails.SeniorCitizenAge);
                            this.dbServer.AddInParameter(command3, "InHouse", DbType.Boolean, serviceMasterDetails.InHouse);
                            this.dbServer.AddInParameter(command3, "DoctorShare", DbType.Boolean, serviceMasterDetails.DoctorShare);
                            this.dbServer.AddInParameter(command3, "DoctorSharePercentage", DbType.Decimal, serviceMasterDetails.DoctorSharePercentage);
                            this.dbServer.AddInParameter(command3, "DoctorShareAmount", DbType.Decimal, serviceMasterDetails.DoctorShareAmount);
                            this.dbServer.AddInParameter(command3, "RateEditable", DbType.Boolean, serviceMasterDetails.RateEditable);
                            this.dbServer.AddInParameter(command3, "MaxRate", DbType.Decimal, serviceMasterDetails.MaxRate);
                            this.dbServer.AddInParameter(command3, "MinRate", DbType.Decimal, serviceMasterDetails.MinRate);
                            this.dbServer.AddInParameter(command3, "Rate", DbType.Decimal, serviceMasterDetails.Rate);
                            this.dbServer.AddInParameter(command3, "CheckedAllTariffs", DbType.Boolean, serviceMasterDetails.CheckedAllTariffs);
                            this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, serviceMasterDetails.Status);
                            this.dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, serviceMasterDetails.CreatedUnitID);
                            this.dbServer.AddInParameter(command3, "UpdatedUnitID", DbType.Int64, serviceMasterDetails.UpdatedUnitID);
                            this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, serviceMasterDetails.AddedBy);
                            this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, serviceMasterDetails.AddedOn);
                            this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, serviceMasterDetails.AddedDateTime);
                            this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, serviceMasterDetails.AddedWindowsLoginName);
                            this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0);
                            this.dbServer.AddOutParameter(command3, "Id", DbType.Int64, 0);
                            this.dbServer.ExecuteNonQuery(command3, transaction);
                            nvo2.TariffServiceID = (long) this.dbServer.GetParameterValue(command3, "Id");
                            DbCommand command4 = null;
                            command4 = this.dbServer.GetStoredProcCommand("CIMS_AddTariffServiceClassRateDetail");
                            command4.Connection = con;
                            this.dbServer.AddInParameter(command4, "TariffServiceId", DbType.Int64, nvo2.TariffServiceID);
                            this.dbServer.AddInParameter(command4, "ClassId", DbType.Int64, 1);
                            this.dbServer.AddInParameter(command4, "Rate", DbType.Int64, serviceMasterDetails.Rate);
                            this.dbServer.AddInParameter(command4, "Status", DbType.Boolean, serviceMasterDetails.Status);
                            this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, serviceMasterDetails.UnitID);
                            this.dbServer.ExecuteNonQuery(command4, transaction);
                        }
                    }
                }
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                nvo.ServiceMasterDetails = null;
                throw exception;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con = null;
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject AddServiceTariff(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddServiceTariffBizActionVO nvo = valueObject as clsAddServiceTariffBizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                nvo.ServiceMasterDetails.Query = "";
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddServiceClassRateDetails");
                storedProcCommand.Parameters.Clear();
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, 0);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceId", DbType.Int64, nvo.ServiceMasterDetails.ServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "ClassId", DbType.Int64, 1);
                this.dbServer.AddInParameter(storedProcCommand, "Rate", DbType.Decimal, nvo.ServiceMasterDetails.Rate);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.ServiceMasterDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                nvo.SuccessStatus = this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject AddTariffService(IValueObject valueObject, clsUserVO userVO)
        {
            DbConnection con = null;
            DbTransaction trans = null;
            clsAddTariffServiceBizActionVO nvo = valueObject as clsAddTariffServiceBizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                DbCommand command2 = null;
                con = this.dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                trans = con.BeginTransaction();
                clsServiceMasterVO serviceMasterDetails = nvo.ServiceMasterDetails;
                List<long> serviceTarrifIds = this.GetServiceTarrifIds(serviceMasterDetails.ID, nvo.ServiceMasterDetails.UnitID);
                if (!nvo.TariffServiceForm)
                {
                    this.UpdateTariffServiceMaster(serviceTarrifIds, con, trans);
                    this.UpdateTariffServiceClassRateDetails(serviceTarrifIds, con, trans);
                }
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddTariffService");
                storedProcCommand.Connection = con;
                if (nvo.TariffList.Count > 0)
                {
                    for (int i = 0; i <= (nvo.TariffList.Count - 1); i++)
                    {
                        storedProcCommand.Parameters.Clear();
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, serviceMasterDetails.UnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, Convert.ToInt64(nvo.TariffList[i]));
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, serviceMasterDetails.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceCode", DbType.String, serviceMasterDetails.ServiceCode);
                        this.dbServer.AddInParameter(storedProcCommand, "SpecializationId", DbType.Int64, serviceMasterDetails.Specialization);
                        this.dbServer.AddInParameter(storedProcCommand, "SubSpecializationId", DbType.Int64, serviceMasterDetails.SubSpecialization);
                        this.dbServer.AddInParameter(storedProcCommand, "ShortDescription", DbType.String, serviceMasterDetails.ShortDescription);
                        this.dbServer.AddInParameter(storedProcCommand, "LongDescription", DbType.String, serviceMasterDetails.LongDescription);
                        this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, serviceMasterDetails.ServiceName);
                        this.dbServer.AddInParameter(storedProcCommand, "CodeType", DbType.Int64, serviceMasterDetails.CodeType);
                        this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, serviceMasterDetails.Code);
                        this.dbServer.AddInParameter(storedProcCommand, "StaffDiscount", DbType.Boolean, serviceMasterDetails.StaffDiscount);
                        this.dbServer.AddInParameter(storedProcCommand, "StaffDiscountAmount", DbType.Decimal, serviceMasterDetails.StaffDiscountAmount);
                        this.dbServer.AddInParameter(storedProcCommand, "StaffDiscountPercent", DbType.Decimal, serviceMasterDetails.StaffDiscountPercent);
                        this.dbServer.AddInParameter(storedProcCommand, "StaffDependantDiscount", DbType.Boolean, serviceMasterDetails.StaffDependantDiscount);
                        this.dbServer.AddInParameter(storedProcCommand, "StaffDependantDiscountAmount", DbType.Decimal, serviceMasterDetails.StaffDependantDiscountAmount);
                        this.dbServer.AddInParameter(storedProcCommand, "StaffDependantDiscountPercent", DbType.Decimal, serviceMasterDetails.StaffDependantDiscountPercent);
                        this.dbServer.AddInParameter(storedProcCommand, "Concession", DbType.Boolean, serviceMasterDetails.Concession);
                        this.dbServer.AddInParameter(storedProcCommand, "ConcessionAmount", DbType.Decimal, serviceMasterDetails.ConcessionAmount);
                        this.dbServer.AddInParameter(storedProcCommand, "ConcessionPercent", DbType.Decimal, serviceMasterDetails.ConcessionPercent);
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceTax", DbType.Boolean, serviceMasterDetails.ServiceTax);
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceTaxAmount", DbType.Decimal, serviceMasterDetails.ServiceTaxAmount);
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceTaxPercent", DbType.Decimal, serviceMasterDetails.ServiceTaxPercent);
                        this.dbServer.AddInParameter(storedProcCommand, "InHouse", DbType.Boolean, serviceMasterDetails.InHouse);
                        this.dbServer.AddInParameter(storedProcCommand, "DoctorShare", DbType.Boolean, serviceMasterDetails.DoctorShare);
                        this.dbServer.AddInParameter(storedProcCommand, "DoctorSharePercentage", DbType.Decimal, serviceMasterDetails.DoctorSharePercentage);
                        this.dbServer.AddInParameter(storedProcCommand, "DoctorShareAmount", DbType.Decimal, serviceMasterDetails.DoctorShareAmount);
                        this.dbServer.AddInParameter(storedProcCommand, "RateEditable", DbType.Boolean, serviceMasterDetails.RateEditable);
                        this.dbServer.AddInParameter(storedProcCommand, "MaxRate", DbType.Decimal, serviceMasterDetails.MaxRate);
                        this.dbServer.AddInParameter(storedProcCommand, "MinRate", DbType.Decimal, serviceMasterDetails.MinRate);
                        this.dbServer.AddInParameter(storedProcCommand, "Rate", DbType.Decimal, serviceMasterDetails.Rate);
                        this.dbServer.AddInParameter(storedProcCommand, "CheckedAllTariffs", DbType.Boolean, serviceMasterDetails.CheckedAllTariffs);
                        this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, serviceMasterDetails.Status);
                        this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, serviceMasterDetails.CreatedUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, serviceMasterDetails.UpdatedUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, serviceMasterDetails.AddedBy);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, serviceMasterDetails.AddedOn);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, serviceMasterDetails.AddedDateTime);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, serviceMasterDetails.AddedWindowsLoginName);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0);
                        this.dbServer.AddOutParameter(storedProcCommand, "Id", DbType.Int64, 0);
                        this.dbServer.ExecuteNonQuery(storedProcCommand, trans);
                        nvo.TariffServiceID = (long) this.dbServer.GetParameterValue(storedProcCommand, "Id");
                        foreach (clsServiceTarrifClassRateDetailsVO svo in nvo.ClassList)
                        {
                            command2 = this.dbServer.GetStoredProcCommand("CIMS_AddTariffServiceClassRateDetail");
                            command2.Connection = con;
                            this.dbServer.AddInParameter(command2, "TariffServiceId", DbType.Int64, nvo.TariffServiceID);
                            this.dbServer.AddInParameter(command2, "ClassId", DbType.Int64, svo.ClassID);
                            this.dbServer.AddInParameter(command2, "Rate", DbType.Int64, svo.Rate);
                            this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, svo.Status);
                            this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, serviceMasterDetails.UnitID);
                            this.dbServer.ExecuteNonQuery(command2, trans);
                        }
                    }
                }
                trans.Commit();
                nvo.SuccessStatus = 0;
            }
            catch (Exception exception)
            {
                trans.Rollback();
                nvo.ServiceMasterDetails = null;
                throw exception;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con = null;
                trans = null;
            }
            return nvo;
        }

        public override IValueObject AddUpdateBankBranchDetails(IValueObject valueObject, clsUserVO userVO)
        {
            clsBankBranchVO hvo = new clsBankBranchVO();
            clsAddUpadateBAnkBranchDetailsBizActionVO nvo = valueObject as clsAddUpadateBAnkBranchDetailsBizActionVO;
            try
            {
                hvo = nvo.ItemMatserDetails[0];
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateBankBranchDetails");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, hvo.BranchId);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, hvo.ClinicId);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, hvo.BranchName);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, hvo.Code);
                this.dbServer.AddInParameter(storedProcCommand, "BankId", DbType.Int64, hvo.BankId);
                this.dbServer.AddInParameter(storedProcCommand, "MICRNumber", DbType.Int64, hvo.MICRNumber);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, hvo.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, hvo.CreatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, hvo.UpdatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, hvo.AddedBy);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, hvo.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, hvo.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int32, hvo.UpdatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, hvo.UpdatedOn);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, hvo.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, hvo.AddedWindowsLoginName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, hvo.UpdateWindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception exception1)
            {
                if (exception1.Message.Contains("duplicate"))
                {
                    hvo.PrimaryKeyViolationError = true;
                }
                else
                {
                    hvo.GeneralError = true;
                }
            }
            return nvo;
        }

        public override IValueObject AddUpdateCompanyAssociate(IValueObject valueObject, clsUserVO userVO)
        {
            clsCompanyAssociateVO evo = new clsCompanyAssociateVO();
            clsAddUpdateCompanyAssociateBizActionVO nvo = valueObject as clsAddUpdateCompanyAssociateBizActionVO;
            try
            {
                evo = nvo.ItemMatserDetails[0];
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateCompanyAssociateDetails");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, evo.Id);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, evo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, evo.Description);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, evo.Code);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyID", DbType.Int64, evo.CompanyId);
                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, evo.TariffId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, evo.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, evo.CreatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, evo.UpdatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, evo.AddedBy);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, evo.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, evo.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int32, evo.UpdatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, evo.UpdatedOn);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, evo.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, evo.AddedWindowsLoginName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, evo.UpdateWindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception exception1)
            {
                if (exception1.Message.Contains("duplicate"))
                {
                    evo.PrimaryKeyViolationError = true;
                }
                else
                {
                    evo.GeneralError = true;
                }
            }
            return nvo;
        }

        public override IValueObject AddUpdateCompanyDetails(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddUpdateCompanyDetailsBizActionVO bizActionObj = valueObject as clsAddUpdateCompanyDetailsBizActionVO;
            return ((bizActionObj.ItemMatserDetails.Id != 0L) ? this.UpdateCompanyMaster(bizActionObj, userVO) : this.AddCompanyMaster(bizActionObj, userVO));
        }

        public override IValueObject AddUpdateDepartmentMaster(IValueObject valueObject, clsUserVO userVO)
        {
            clsDepartmentVO tvo = new clsDepartmentVO();
            clsAddUpdateDepartmentMasterBizActionVO nvo = valueObject as clsAddUpdateDepartmentMasterBizActionVO;
            try
            {
                tvo = nvo.ItemMatserDetails[0];
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateDepartmentMaster");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, tvo.Id);
                this.dbServer.AddInParameter(storedProcCommand, "IsClinical", DbType.Boolean, tvo.IsClinical);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, tvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, tvo.Description);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, tvo.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, tvo.Status);
                this.dbServer.AddInParameter(storedProcCommand, "AddUnitID", DbType.Int64, tvo.AddUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "By", DbType.Int64, tvo.By);
                this.dbServer.AddInParameter(storedProcCommand, "On", DbType.String, tvo.On);
                this.dbServer.AddInParameter(storedProcCommand, "DateTime", DbType.DateTime, tvo.DateTime);
                this.dbServer.AddInParameter(storedProcCommand, "WindowsLoginName", DbType.String, tvo.WindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.AddOutParameter(storedProcCommand, "OutPutID", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
                if (nvo.IsUpdate && (nvo.SuccessStatus == 1))
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteDepartmentSpecializationDetails");
                    this.dbServer.AddInParameter(command2, "DeptID", DbType.Int64, tvo.Id);
                    this.dbServer.ExecuteNonQuery(command2);
                }
                if (nvo.SuccessStatus == 1)
                {
                    foreach (clsSubSpecializationVO nvo2 in tvo.SpecilizationList)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateDeptSpecList");
                        this.dbServer.AddInParameter(command3, "Id", DbType.Int64, 0);
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, tvo.UnitId);
                        this.dbServer.AddInParameter(command3, "DeptID", DbType.Int64, tvo.Id);
                        this.dbServer.AddInParameter(command3, "SpecializationID", DbType.Int64, nvo2.SpecializationId);
                        this.dbServer.AddInParameter(command3, "SubSpecializationId", DbType.Int64, nvo2.SubSpecializationId);
                        this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, nvo2.Status);
                        this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int64, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command3);
                        nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(command3, "ResultStatus"));
                    }
                }
            }
            catch (Exception exception1)
            {
                if (exception1.Message.Contains("duplicate"))
                {
                    tvo.PrimaryKeyViolationError = true;
                }
                else
                {
                    tvo.GeneralError = true;
                }
            }
            return nvo;
        }

        public override IValueObject AddUpdateDoctorServiceRateCategory(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateDoctorServiceRateCategoryBizActionVO nvo = valueObject as clsAddUpdateDoctorServiceRateCategoryBizActionVO;
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                if (nvo.DeletedServiceList == null)
                {
                    nvo.DeletedServiceList = new List<clsServiceMasterVO>();
                }
                if (nvo.IsModify && (nvo.DeletedServiceList.Count > 0))
                {
                    foreach (clsServiceMasterVO rvo in nvo.DeletedServiceList.ToList<clsServiceMasterVO>())
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteDoctorServiceRateCategoryWise");
                        this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, rvo.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, rvo.UnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "CategoryID", DbType.Int64, rvo.CategoryID);
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceId", DbType.Int64, rvo.ServiceID);
                        this.dbServer.AddInParameter(storedProcCommand, "ClassId", DbType.Int64, rvo.ClassID);
                        this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.ExecuteNonQuery(storedProcCommand);
                    }
                }
                if (nvo.ServiceMasterDetailsList == null)
                {
                    nvo.ServiceMasterDetailsList = new List<clsServiceMasterVO>();
                }
                if ((nvo.ServiceMasterDetailsList != null) && (nvo.ServiceMasterDetailsList.Count > 0))
                {
                    foreach (clsServiceMasterVO rvo2 in nvo.ServiceMasterDetailsList)
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddDoctorServiceRateCategoryWise");
                        this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, 0);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "CategoryID", DbType.Int64, nvo.CategoryID);
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, rvo2.ServiceID);
                        this.dbServer.AddInParameter(storedProcCommand, "ClassId", DbType.Int64, rvo2.ClassID);
                        this.dbServer.AddInParameter(storedProcCommand, "SpecializationID", DbType.Int64, rvo2.Specialization);
                        this.dbServer.AddInParameter(storedProcCommand, "SubSpecializationID", DbType.Int64, rvo2.SubSpecialization);
                        this.dbServer.AddInParameter(storedProcCommand, "Rate", DbType.Decimal, rvo2.Rate);
                        this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                        this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    }
                }
                transaction.Commit();
                nvo.SuccessStatus = 0L;
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1L;
                transaction.Rollback();
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject AddUpdateServiceClassRates(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            clsAddUpdateServiceMasterTariffBizActionVO nvo = valueObject as clsAddUpdateServiceMasterTariffBizActionVO;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = null;
                if (nvo.IsApplyToAllTariff)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ApllyServiceClassRateToAllTariff");
                    this.dbServer.AddInParameter(storedProcCommand, "IsupdatePreviousRate", DbType.Boolean, nvo.IsupdatePreviousRate);
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ApllyServiceClassRateToSelectedTariff");
                    this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.String, nvo.TariffIDList);
                }
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsApplyToAllTariff", DbType.Boolean, nvo.IsApplyToAllTariff);
                this.dbServer.AddInParameter(storedProcCommand, "ClassID", DbType.String, nvo.ClassIDList);
                this.dbServer.AddInParameter(storedProcCommand, "Rate", DbType.String, nvo.ClassRateList);
                this.dbServer.AddOutParameter(storedProcCommand, "SucessStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = Convert.ToInt32(DALHelper.HandleDBNull(this.dbServer.GetParameterValue(storedProcCommand, "SucessStatus")));
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw exception;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection = null;
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject AddUpdateSeviceTaxDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            clsAddUpdateServiceTaxBizActionVO nvo = valueObject as clsAddUpdateServiceTaxBizActionVO;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = null;
                if (nvo.OperationType == 1)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateServiceTaxDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "OperationType", DbType.Int32, nvo.OperationType);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.ServiceTaxDetailsVO.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceTaxDetailsVO.ServiceId);
                    this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, nvo.ServiceTaxDetailsVO.TariffId);
                    this.dbServer.AddInParameter(storedProcCommand, "ClassID", DbType.Int64, nvo.ServiceTaxDetailsVO.ClassId);
                    this.dbServer.AddInParameter(storedProcCommand, "TaxID", DbType.Int64, nvo.ServiceTaxDetailsVO.TaxID);
                    this.dbServer.AddInParameter(storedProcCommand, "TaxPercentage", DbType.Decimal, nvo.ServiceTaxDetailsVO.Percentage);
                    this.dbServer.AddInParameter(storedProcCommand, "TaxType", DbType.Int32, nvo.ServiceTaxDetailsVO.TaxType);
                    this.dbServer.AddInParameter(storedProcCommand, "IsTaxLimitApplicable", DbType.Boolean, nvo.ServiceTaxDetailsVO.IsTaxLimitApplicable);
                    this.dbServer.AddInParameter(storedProcCommand, "TaxLimit", DbType.Decimal, nvo.ServiceTaxDetailsVO.TaxLimit);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.ServiceTaxDetailsVO.status);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddOutParameter(storedProcCommand, "ReasultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.ServiceTaxDetailsVO.ID);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    nvo.SuccessStatus = Convert.ToInt32(DALHelper.HandleDBNull(this.dbServer.GetParameterValue(storedProcCommand, "ReasultStatus")));
                    nvo.ServiceTaxDetailsVO.ID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                }
                else if (nvo.OperationType != 2)
                {
                    if (nvo.OperationType == 3)
                    {
                        storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateServiceTaxDetails");
                        this.dbServer.AddInParameter(storedProcCommand, "OperationType", DbType.Int32, nvo.OperationType);
                        this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.ServiceTaxDetailsVO.status);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, objUserVO.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddOutParameter(storedProcCommand, "ReasultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.ServiceTaxDetailsVO.ID);
                        this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                        nvo.SuccessStatus = Convert.ToInt32(DALHelper.HandleDBNull(this.dbServer.GetParameterValue(storedProcCommand, "ReasultStatus")));
                        Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                    }
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateServiceTaxDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "OperationType", DbType.Int32, nvo.OperationType);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.ServiceTaxDetailsVO.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceTaxDetailsVO.ServiceId);
                    this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, nvo.ServiceTaxDetailsVO.TariffId);
                    this.dbServer.AddInParameter(storedProcCommand, "ClassID", DbType.Int64, nvo.ServiceTaxDetailsVO.ClassId);
                    this.dbServer.AddInParameter(storedProcCommand, "TaxID", DbType.Int64, nvo.ServiceTaxDetailsVO.TaxID);
                    this.dbServer.AddInParameter(storedProcCommand, "TaxPercentage", DbType.Decimal, nvo.ServiceTaxDetailsVO.Percentage);
                    this.dbServer.AddInParameter(storedProcCommand, "TaxType", DbType.Int32, nvo.ServiceTaxDetailsVO.TaxType);
                    this.dbServer.AddInParameter(storedProcCommand, "IsTaxLimitApplicable", DbType.Boolean, nvo.ServiceTaxDetailsVO.IsTaxLimitApplicable);
                    this.dbServer.AddInParameter(storedProcCommand, "TaxLimit", DbType.Decimal, nvo.ServiceTaxDetailsVO.TaxLimit);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.ServiceTaxDetailsVO.status);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, objUserVO.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddOutParameter(storedProcCommand, "ReasultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.ServiceTaxDetailsVO.ID);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    nvo.SuccessStatus = Convert.ToInt32(DALHelper.HandleDBNull(this.dbServer.GetParameterValue(storedProcCommand, "ReasultStatus")));
                    Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
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
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection = null;
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject AddUpdateSpecialization(IValueObject valueObject, clsUserVO userVO)
        {
            clsSpecializationVO nvo = new clsSpecializationVO();
            clsAddUpdateSpecializationBizActionVO nvo2 = valueObject as clsAddUpdateSpecializationBizActionVO;
            try
            {
                nvo = nvo2.ItemMatserDetails[0];
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateSpecialization");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, nvo.SpecializationId);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.ClinicId);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.SpecializationName);
                this.dbServer.AddInParameter(storedProcCommand, "IsGenerateToken", DbType.Boolean, nvo.IsGenerateToken);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, nvo.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, nvo.CreatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, nvo.UpdatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, nvo.AddedBy);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, nvo.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, nvo.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int32, nvo.UpdatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, nvo.UpdatedOn);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, nvo.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, nvo.AddedWindowsLoginName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, nvo.UpdateWindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo2.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception exception1)
            {
                if (exception1.Message.Contains("duplicate"))
                {
                    nvo.PrimaryKeyViolationError = true;
                }
                else
                {
                    nvo.GeneralError = true;
                }
            }
            return nvo2;
        }

        public override IValueObject AddUpdateSubSpecialization(IValueObject valueObject, clsUserVO userVO)
        {
            clsSubSpecializationVO nvo = new clsSubSpecializationVO();
            clsAddUpadateSubSpecializationBizActionVO nvo2 = valueObject as clsAddUpadateSubSpecializationBizActionVO;
            try
            {
                nvo = nvo2.ItemMatserDetails[0];
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateSubSpecialization");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, nvo.SubSpecializationId);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.ClinicId);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.SubSpecializationName);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, nvo.Code);
                this.dbServer.AddInParameter(storedProcCommand, "SpecializationID", DbType.Int64, nvo.SpecializationId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, nvo.CreatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, nvo.UpdatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, nvo.AddedBy);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, nvo.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, nvo.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int32, nvo.UpdatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, nvo.UpdatedOn);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, nvo.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, nvo.AddedWindowsLoginName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, nvo.UpdateWindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo2.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception exception1)
            {
                if (exception1.Message.Contains("duplicate"))
                {
                    nvo.PrimaryKeyViolationError = true;
                }
                else
                {
                    nvo.GeneralError = true;
                }
            }
            return nvo2;
        }

        public override IValueObject AddUpdateTariff(IValueObject valueObject, clsUserVO userVO)
        {
            clsTariffVO fvo = new clsTariffVO();
            clsAddUpdateTariffBizActionVO nvo = valueObject as clsAddUpdateTariffBizActionVO;
            try
            {
                fvo = nvo.ItemMatserDetails[0];
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateTariffDetails");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, fvo.Id);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, fvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, fvo.Description);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, fvo.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, fvo.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, fvo.CreatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, fvo.UpdatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, fvo.AddedBy);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, fvo.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, fvo.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int32, fvo.UpdatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, fvo.UpdatedOn);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, fvo.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, fvo.AddedWindowsLoginName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, fvo.UpdateWindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception exception1)
            {
                if (exception1.Message.Contains("duplicate"))
                {
                    fvo.PrimaryKeyViolationError = true;
                }
                else
                {
                    fvo.GeneralError = true;
                }
            }
            return nvo;
        }

        public override IValueObject ChangeTariffServiceStatus(IValueObject valueObject, clsUserVO userVO)
        {
            clsChangeTariffServiceStatusBizActionVO nvo = valueObject as clsChangeTariffServiceStatusBizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                clsServiceMasterVO serviceMasterDetails = nvo.ServiceMasterDetails;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ChangeTariffServiceStatus");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, serviceMasterDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Int32, nvo.SuccessStatus);
                nvo.SuccessStatus = (this.dbServer.ExecuteNonQuery(storedProcCommand) <= 0) ? 0 : 1;
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject CheckForTariffExistanceInServiceTariffMaster(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddServiceTariffBizActionVO nvo = valueObject as clsAddServiceTariffBizActionVO;
            try
            {
                DbCommand sqlStringCommand = null;
                clsServiceMasterVO serviceMasterDetails = nvo.ServiceMasterDetails;
                sqlStringCommand = this.dbServer.GetSqlStringCommand(serviceMasterDetails.Query);
                nvo.SuccessStatus = (((int) this.dbServer.ExecuteScalar(sqlStringCommand)) <= 0) ? 0 : 1;
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject DeletetariffServiceAndServiceTariffMaster(IValueObject valueObject, clsUserVO userVO)
        {
            clsDeleteTariffServiceAndServiceTariffBizActionVO nvo = valueObject as clsDeleteTariffServiceAndServiceTariffBizActionVO;
            try
            {
                DbCommand sqlStringCommand = null;
                clsServiceMasterVO serviceMasterDetails = nvo.ServiceMasterDetails;
                serviceMasterDetails.Query = "Delete from T_ServiceTariffMaster where ServiceId=" + serviceMasterDetails.ServiceID;
                serviceMasterDetails.Query = serviceMasterDetails.Query + " Delete from T_TariffServiceMaster where ServiceId=" + serviceMasterDetails.ServiceID;
                sqlStringCommand = this.dbServer.GetSqlStringCommand(serviceMasterDetails.Query);
                if (this.dbServer.ExecuteNonQuery(sqlStringCommand) <= 0)
                {
                    nvo.SuccessStatus = 0;
                }
                else
                {
                    nvo.SuccessStatus = 1;
                    serviceMasterDetails.Query = "";
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject DeleteTariffServiceClassRateDetail(IValueObject valueObject, clsUserVO userVO)
        {
            clsDeletetTriffServiceClassRateDetailsBizActionVO nvo = valueObject as clsDeletetTriffServiceClassRateDetailsBizActionVO;
            try
            {
                DbCommand sqlStringCommand = null;
                clsServiceMasterVO serviceMasterDetails = nvo.ServiceMasterDetails;
                serviceMasterDetails.Query = "Delete from T_TariffServiceClassRateDetail where TariffServiceId in (" + serviceMasterDetails.TariffIDs + ")";
                sqlStringCommand = this.dbServer.GetSqlStringCommand(serviceMasterDetails.Query);
                if (this.dbServer.ExecuteNonQuery(sqlStringCommand) <= 0)
                {
                    nvo.SuccessStatus = 0;
                }
                else
                {
                    nvo.SuccessStatus = 1;
                    serviceMasterDetails.Query = "";
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetAdmissionTypeTariffServiceList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetAdmissionTypeTariffServiceListBizActionVO nvo = (clsGetAdmissionTypeTariffServiceListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetTariffServiceAdmissionTypeList");
                this.dbServer.AddInParameter(storedProcCommand, "AdmissionTypeID", DbType.Int64, nvo.AdmissionTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, nvo.TariffID);
                this.dbServer.AddInParameter(storedProcCommand, "ClassID", DbType.Int64, nvo.ClassID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ServiceList == null)
                    {
                        nvo.ServiceList = new List<clsServiceMasterVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        clsServiceMasterVO item = new clsServiceMasterVO {
                            ID = (long) reader["ServiceID"],
                            TariffServiceMasterID = (long) reader["ID"],
                            ServiceName = (string) DALHelper.HandleDBNull(reader["ServiceName"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            ServiceCode = (string) DALHelper.HandleDBNull(reader["ServiceCode"]),
                            TariffID = (long) DALHelper.HandleDBNull(reader["TariffID"]),
                            Specialization = (long) DALHelper.HandleDBNull(reader["SpecializationID"]),
                            SubSpecialization = (long) DALHelper.HandleDBNull(reader["SubSpecializationId"])
                        };
                        if (nvo.PatientSourceType == 2)
                        {
                            item.Rate = Convert.ToDecimal((double) DALHelper.HandleDBNull(reader["Rate"]));
                            item.ConcessionAmount = Convert.ToDecimal((double) DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                            item.ConcessionPercent = Convert.ToDecimal((double) DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                        }
                        else if (nvo.PatientSourceType == 1)
                        {
                            item.Rate = Convert.ToDecimal((double) DALHelper.HandleDBNull(reader["Rate"]));
                            item.ConcessionAmount = Convert.ToDecimal((double) DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                            item.ConcessionPercent = Convert.ToDecimal((double) DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                        }
                        else
                        {
                            item.Rate = (decimal) DALHelper.HandleDBNull(reader["Rate"]);
                            item.SeniorCitizen = (bool) DALHelper.HandleBoolDBNull(reader["SeniorCitizen"]);
                            item.SeniorCitizenConAmount = (decimal) DALHelper.HandleDBNull(reader["SeniorCitizenConAmount"]);
                            item.SeniorCitizenConPercent = (decimal) DALHelper.HandleDBNull(reader["SeniorCitizenConPercent"]);
                            item.SeniorCitizenAge = (int) DALHelper.HandleDBNull(reader["SeniorCitizenAge"]);
                            if (item.SeniorCitizen && (nvo.Age >= item.SeniorCitizenAge))
                            {
                                item.ConcessionAmount = item.SeniorCitizenConAmount;
                                item.ConcessionPercent = item.SeniorCitizenConPercent;
                            }
                            else
                            {
                                item.ConcessionAmount = (decimal) DALHelper.HandleDBNull(reader["ConcessionAmount"]);
                                item.ConcessionPercent = (decimal) DALHelper.HandleDBNull(reader["ConcessionPercent"]);
                            }
                        }
                        item.StaffDiscount = (bool) DALHelper.HandleDBNull(reader["StaffDiscount"]);
                        item.StaffDiscountAmount = (decimal) DALHelper.HandleDBNull(reader["StaffDiscountAmount"]);
                        item.StaffDiscountPercent = (decimal) DALHelper.HandleDBNull(reader["StaffDiscountPercent"]);
                        item.StaffDependantDiscount = (bool) DALHelper.HandleDBNull(reader["StaffDependantDiscount"]);
                        item.StaffDependantDiscountAmount = (decimal) DALHelper.HandleDBNull(reader["StaffDependantDiscountAmount"]);
                        item.StaffDependantDiscountPercent = (decimal) DALHelper.HandleDBNull(reader["StaffDependantDiscountPercent"]);
                        item.Concession = (bool) DALHelper.HandleDBNull(reader["Concession"]);
                        item.ServiceTax = (bool) DALHelper.HandleDBNull(reader["ServiceTax"]);
                        item.ServiceTaxAmount = (decimal) DALHelper.HandleDBNull(reader["ServiceTaxAmount"]);
                        item.ServiceTaxPercent = (decimal) DALHelper.HandleDBNull(reader["ServiceTaxPercent"]);
                        item.InHouse = (bool) DALHelper.HandleDBNull(reader["InHouse"]);
                        item.DoctorShare = (bool) DALHelper.HandleDBNull(reader["DoctorShare"]);
                        item.DoctorSharePercentage = (decimal) DALHelper.HandleDBNull(reader["DoctorSharePercentage"]);
                        item.DoctorShareAmount = (decimal) DALHelper.HandleDBNull(reader["DoctorShareAmount"]);
                        item.RateEditable = (bool) DALHelper.HandleDBNull(reader["RateEditable"]);
                        item.MaxRate = (decimal) DALHelper.HandleDBNull(reader["MaxRate"]);
                        item.MinRate = (decimal) DALHelper.HandleDBNull(reader["MinRate"]);
                        item.TarrifCode = (string) DALHelper.HandleDBNull(reader["TarrifServiceCode"]);
                        item.TarrifName = (string) DALHelper.HandleDBNull(reader["TarrifServiceName"]);
                        item.Code = (string) DALHelper.HandleDBNull(reader["Code"]);
                        item.CodeType = (long) DALHelper.HandleDBNull(reader["CodeType"]);
                        item.ShortDescription = (string) DALHelper.HandleDBNull(reader["ShortDescription"]);
                        item.LongDescription = (string) DALHelper.HandleDBNull(reader["LongDescription"]);
                        item.SpecializationString = (string) DALHelper.HandleDBNull(reader["Specialization"]);
                        item.SubSpecializationString = (string) DALHelper.HandleDBNull(reader["SubSpecialization"]);
                        item.IsPackage = (bool) DALHelper.HandleDBNull(reader["IsPackage"]);
                        item.PackageID = DALHelper.HandleIntegerNull(reader["PackageID"]);
                        item.ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassID"]));
                        item.ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]));
                        nvo.ServiceList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetAllServiceClassRateDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceMasterListBizActionVO nvo = (clsGetServiceMasterListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ServiceClassRateDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceMaster.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ServiceMaster == null)
                    {
                        nvo.ServiceMaster = new clsServiceMasterVO();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            break;
                        }
                        nvo.ServiceMaster.Rate = (decimal) reader["Rate"];
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetAllServiceList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceMasterListBizActionVO nvo = (clsGetServiceMasterListBizActionVO) valueObject;
            try
            {
                if (nvo.IsOLDServiceMaster)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAllServiceDetails");
                    if ((nvo.ServiceName != null) && (nvo.ServiceName.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.ServiceName);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "Specialization", DbType.Int64, nvo.Specialization);
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceCode", DbType.String, nvo.ServiceCode);
                    this.dbServer.AddInParameter(storedProcCommand, "SubSpecialization", DbType.Int64, nvo.SubSpecialization);
                    this.dbServer.AddInParameter(storedProcCommand, "FromPackage", DbType.Boolean, nvo.FromPackage);
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ServiceName");
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.IsStatus);
                    this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.ServiceList == null)
                        {
                            nvo.ServiceList = new List<clsServiceMasterVO>();
                        }
                        while (true)
                        {
                            if (!reader.Read())
                            {
                                reader.NextResult();
                                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                                reader.Close();
                                break;
                            }
                            clsServiceMasterVO item = new clsServiceMasterVO {
                                ID = (long) reader["ID"],
                                ServiceName = (string) DALHelper.HandleDBNull(reader["ServiceName"]),
                                Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                                ServiceCode = (string) DALHelper.HandleDBNull(reader["ServiceCode"]),
                                Rate = (decimal) DALHelper.HandleDBNull(reader["Rate"]),
                                SpecializationString = (string) DALHelper.HandleDBNull(reader["Specialization"]),
                                SubSpecializationString = (string) DALHelper.HandleDBNull(reader["SubSpecialization"]),
                                ShortDescription = (string) DALHelper.HandleDBNull(reader["ShortDesc"]),
                                LongDescription = (string) DALHelper.HandleDBNull(reader["LongDesc"]),
                                Specialization = (long) DALHelper.HandleDBNull(reader["SpecializationId"]),
                                SubSpecialization = (long) DALHelper.HandleDBNull(reader["SubSpecializationId"]),
                                UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                                IsPackage = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackage"])),
                                PackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageID"]))
                            };
                            nvo.ServiceList.Add(item);
                        }
                    }
                }
                else
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetServiceList_New");
                    if (nvo.ServiceCode != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceCode", DbType.String, nvo.ServiceCode);
                    }
                    if ((nvo.ServiceName != null) && (nvo.ServiceName.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceName", DbType.String, nvo.ServiceName);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "Specialization", DbType.Int64, nvo.Specialization);
                    this.dbServer.AddInParameter(storedProcCommand, "SubSpecialization", DbType.Int64, nvo.SubSpecialization);
                    this.dbServer.AddInParameter(storedProcCommand, "IsFromPackage", DbType.Boolean, nvo.IsFromPackage);
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ServiceName");
                    this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                    DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader2.HasRows)
                    {
                        if (nvo.ServiceList == null)
                        {
                            nvo.ServiceList = new List<clsServiceMasterVO>();
                        }
                        while (true)
                        {
                            if (!reader2.Read())
                            {
                                reader2.NextResult();
                                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                                break;
                            }
                            clsServiceMasterVO item = new clsServiceMasterVO {
                                ID = (long) reader2["ID"],
                                UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["UnitID"])),
                                ServiceName = (string) DALHelper.HandleDBNull(reader2["ServiceName"]),
                                Status = (bool) DALHelper.HandleDBNull(reader2["Status"]),
                                ServiceCode = (string) DALHelper.HandleDBNull(reader2["ServiceCode"]),
                                Specialization = (long) DALHelper.HandleDBNull(reader2["SpecializationId"]),
                                SubSpecialization = (long) DALHelper.HandleDBNull(reader2["SubSpecializationId"]),
                                LongDescription = (string) DALHelper.HandleDBNull(reader2["LongDescription"]),
                                ShortDescription = (string) DALHelper.HandleDBNull(reader2["ShortDescription"]),
                                Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader2["BaseServiceRate"])),
                                SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader2["Specialization"])),
                                SubSpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader2["SubSpecialization"])),
                                IsFavourite = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsFavourite"])),
                                IsLinkWithInventory = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IslinkWithInventory"])),
                                IsPackage = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsPackage"])),
                                IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsFreezed"])),
                                IsApproved = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsApproved"]))
                            };
                            nvo.ServiceList.Add(item);
                        }
                    }
                    reader2.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetAllTariffApplicableList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceMasterListBizActionVO nvo = (clsGetServiceMasterListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAllTariffsApplicable");
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceMaster.ServiceID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ServiceList == null)
                    {
                        nvo.ServiceList = new List<clsServiceMasterVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            break;
                        }
                        clsServiceMasterVO item = new clsServiceMasterVO {
                            ID = (long) reader["TariffID"],
                            ServiceTariffMasterStatus = (bool) DALHelper.HandleDBNull(reader["ServiceTariffMasterStatus"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"].ToString()),
                            TariffCode = (string) DALHelper.HandleDBNull(reader["Description"].ToString())
                        };
                        nvo.ServiceList.Add(item);
                    }
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetApplyLevelToService(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetApplyLevelsToServiceBizActionVO nvo = valueObject as clsGetApplyLevelsToServiceBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetServiceLevels");
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.Obj.ServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceUnitID", DbType.Int64, nvo.Obj.ServiceUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.Obj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.Obj.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.Obj.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        nvo.Obj.ServiceUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceUnitID"]));
                        nvo.Obj.L1ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["L1ID"]));
                        nvo.Obj.L2ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["L2ID"]));
                        nvo.Obj.L3ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["L3ID"]));
                        nvo.Obj.L4ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["L4ID"]));
                    }
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            finally
            {
                connection.Close();
                connection = null;
            }
            return nvo;
        }

        public override IValueObject GetBankBranchDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetBankBranchDetailsBizActionVO nvo = valueObject as clsGetBankBranchDetailsBizActionVO;
            clsBankBranchVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetBankBranchDetails");
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsBankBranchVO {
                            BranchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"])),
                            BankId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BankID"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            MICRNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["MICRNumber"])),
                            BankName = Convert.ToString(DALHelper.HandleDBNull(reader["BankName"])),
                            BranchName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.ItemMatserDetails.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }

        public override IValueObject GetCompanyAssociateDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetCompanyAssociateDetailsBizActionVO nvo = valueObject as clsGetCompanyAssociateDetailsBizActionVO;
            clsCompanyAssociateVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCompanyAssociateDetails");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsCompanyAssociateVO {
                            Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            CompanyId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompanyID"])),
                            TariffId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            CompanyName = Convert.ToString(DALHelper.HandleDBNull(reader["Compnay"])),
                            Tariff = Convert.ToString(DALHelper.HandleDBNull(reader["Tariff"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.ItemMatserDetails.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }

        public override IValueObject GetCompanyDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetCompanyDetailsBizActionVO nvo = valueObject as clsGetCompanyDetailsBizActionVO;
            clsCompanyVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCompanyDetails");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, nvo.CompanyId);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                if ((nvo.Description != null) && (nvo.Description.Length > 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.Description);
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, null);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                if (nvo.CompanyId == 0L)
                {
                    reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            item = new clsCompanyVO {
                                Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                CompanyTypeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompanyTypeId"])),
                                Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                                Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                                CompanyName = Convert.ToString(DALHelper.HandleDBNull(reader["CompanyType"])),
                                Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                                ContactPerson = Convert.ToString(DALHelper.HandleDBNull(reader["ContactPerson"])),
                                ContactNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["ContactNo"])),
                                CompanyEmail = Convert.ToString(DALHelper.HandleDBNull(reader["EmailId"])),
                                CompanyAddress = Convert.ToString(DALHelper.HandleDBNull(reader["Address"]))
                            };
                            nvo.ItemMatserDetails.Add(item);
                        }
                    }
                    reader.NextResult();
                    nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                    reader.Close();
                }
                else if (nvo.CompanyId > 0L)
                {
                    reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            item = new clsCompanyVO {
                                Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                CompanyTypeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompanyTypeId"])),
                                Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                                Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                                CompanyName = Convert.ToString(DALHelper.HandleDBNull(reader["CompanyType"])),
                                Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                                PatientCatagoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceID"])),
                                CompanyCategory = Convert.ToString(DALHelper.HandleDBNull(reader["CompanyCategory"])),
                                ContactPerson = Convert.ToString(DALHelper.HandleDBNull(reader["ContactPerson"])),
                                ContactNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["ContactNo"])),
                                CompanyEmail = Convert.ToString(DALHelper.HandleDBNull(reader["EmailId"])),
                                CompanyAddress = Convert.ToString(DALHelper.HandleDBNull(reader["Address"])),
                                Title = Convert.ToString(DALHelper.HandleDBNull(reader["Title"])),
                                AttachedFileContent = (byte[]) DALHelper.HandleDBNull(reader["CompanyLogoFile"]),
                                TitleHeaderImage = Convert.ToString(DALHelper.HandleDBNull(reader["TitleHeadImg"])),
                                AttachedHeadImgFileContent = (byte[]) DALHelper.HandleDBNull(reader["CompHeadImgFile"]),
                                TitleFooterImage = Convert.ToString(DALHelper.HandleDBNull(reader["TitleFootImg"])),
                                AttachedFootImgFileContent = (byte[]) DALHelper.HandleDBNull(reader["CompFootImgFile"]),
                                HeaderText = Convert.ToString(DALHelper.HandleDBNull(reader["HeaderText"])),
                                FooterText = Convert.ToString(DALHelper.HandleDBNull(reader["FooterText"]))
                            };
                            nvo.ItemMatserDetails.Add(item);
                        }
                    }
                    reader.NextResult();
                    if (reader.HasRows)
                    {
                        nvo.TariffDetails = new List<clsTariffDetailsVO>();
                        while (reader.Read())
                        {
                            clsTariffDetailsVO svo = new clsTariffDetailsVO {
                                CompanyID = (long) DALHelper.HandleDBNull(reader["CompanyID"]),
                                TariffID = (long) DALHelper.HandleDBNull(reader["TariffID"]),
                                TariffDescription = (string) DALHelper.HandleDBNull(reader["Description"]),
                                Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                            };
                            nvo.TariffDetails.Add(svo);
                        }
                    }
                    reader.NextResult();
                    nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }

        public override IValueObject GetDepartmentMasterDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetDepartmentMasterDetailsBizActionVO nvo = valueObject as clsGetDepartmentMasterDetailsBizActionVO;
            clsDepartmentVO item = null;
            clsSubSpecializationVO nvo2 = null;
            nvo.DeptSpecializationDetails.SpecilizationList = new List<clsSubSpecializationVO>();
            try
            {
                DbCommand storedProcCommand;
                if (nvo.DeptSpecializationDetails.Id > 0L)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDepatmentMasterDetailsByID");
                    this.dbServer.AddInParameter(storedProcCommand, "DeptID", DbType.Int64, nvo.DeptSpecializationDetails.Id);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            nvo2 = new clsSubSpecializationVO {
                                SpecializationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationID"])),
                                SubSpecializationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"])),
                                SpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader["Specialization"])),
                                SubSpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecialization"])),
                                Status = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["Status"]))
                            };
                            nvo.DeptSpecializationDetails.SpecilizationList.Add(nvo2);
                        }
                    }
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDepatmentMasterDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                    this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            item = new clsDepartmentVO {
                                Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                                Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                                Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                                IsClinical = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsClinical"]))
                            };
                            item.ClinicalStatus = !item.IsClinical ? "No" : "Yes";
                            nvo.ItemMatserDetails.Add(item);
                        }
                    }
                    reader.NextResult();
                    nvo.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }

        public override IValueObject GetFrontPannelDataGridByID(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetFrontPannelDataGridListBizActionVO nvo = valueObject as clsGetFrontPannelDataGridListBizActionVO;
            clsServiceMasterVO item = new clsServiceMasterVO();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorServiceRateCategoryList");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, 0);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.ServiceMasterDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CategoryID", DbType.Int64, nvo.CategoryID);
                this.dbServer.AddInParameter(storedProcCommand, "SpecializationId", DbType.Int64, nvo.ServiceMasterDetails.Specialization);
                this.dbServer.AddInParameter(storedProcCommand, "SubSpecializationId", DbType.Int64, nvo.ServiceMasterDetails.SubSpecialization);
                if ((nvo.ServiceMasterDetails.ServiceName != null) && (nvo.ServiceMasterDetails.ServiceName.Length > 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceName", DbType.String, nvo.ServiceMasterDetails.ServiceName);
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, null);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                nvo.ServiceMasterDetailsList = new List<clsServiceMasterVO>();
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        item = new clsServiceMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryID"])),
                            CategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["CategoryName"])),
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceId"])),
                            ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"])),
                            ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassId"])),
                            ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"])),
                            Specialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"])),
                            SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SpecializationName"])),
                            SubSpecialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"])),
                            SubSpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecializationName"])),
                            Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.ServiceMasterDetailsList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }

        public override IValueObject GetFrontPannelDataGridList(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetFrontPannelDataGridListBizActionVO nvo = valueObject as clsGetFrontPannelDataGridListBizActionVO;
            clsServiceMasterVO item = new clsServiceMasterVO();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorServiceRateCategoryList");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, 0);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.ServiceMasterDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CategoryID", DbType.Int64, nvo.ServiceMasterDetails.CategoryID);
                this.dbServer.AddInParameter(storedProcCommand, "ClassID", DbType.Int64, nvo.ServiceMasterDetails.ClassID);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceName", DbType.String, nvo.ServiceMasterDetails.ServiceName);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, null);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                nvo.ServiceMasterDetailsList = new List<clsServiceMasterVO>();
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsServiceMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryID"])),
                            CategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["CategoryName"])),
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceId"])),
                            ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassID"])),
                            ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"])),
                            ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"])),
                            Specialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"])),
                            SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SpecializationName"])),
                            SubSpecialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"])),
                            SubSpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecializationName"])),
                            Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.ServiceMasterDetailsList.Add(item);
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
            finally
            {
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }

        public override IValueObject GetMasterListByTableName(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetMasterForServiceBizActionVO nvo = (clsGetMasterForServiceBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetServiceMasterDetailsList");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                if ((nvo.ServiceCode != null) && ((nvo.ServiceCode != "") && (nvo.ServiceCode.Length != 0)))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceCode", DbType.String, "%" + nvo.ServiceCode + "%");
                }
                if ((nvo.Description != null) && ((nvo.Description != "") && (nvo.Description.Length != 0)))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, "%" + nvo.Description + "%");
                }
                this.dbServer.AddInParameter(storedProcCommand, "SpecializationID", DbType.Int64, nvo.SpecializationID);
                this.dbServer.AddInParameter(storedProcCommand, "SubSpecializationID", DbType.Int64, nvo.SubSpecializationID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ID");
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ServiceDetails == null)
                    {
                        nvo.ServiceDetails = new List<clsServiceMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsServiceMasterVO item = new clsServiceMasterVO {
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"])),
                            Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceRate"])),
                            ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Specialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationID"])),
                            SubSpecialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.ServiceDetails.Add(item);
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

        public override IValueObject GetServiceBySpecialization(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceBySpecializationBizActionVO nvo = (clsGetServiceBySpecializationBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetServiceBySpecialization");
                this.dbServer.AddInParameter(storedProcCommand, "Specialization", DbType.Int64, nvo.ServiceMaster.Specialization);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.ServiceMaster.Description);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ServiceList == null)
                    {
                        nvo.ServiceList = new List<clsServiceMasterVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            break;
                        }
                        clsServiceMasterVO item = new clsServiceMasterVO {
                            ID = (long) reader["ID"],
                            ServiceName = (string) DALHelper.HandleDBNull(reader["Description"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            ServiceCode = (string) DALHelper.HandleDBNull(reader["ServiceCode"]),
                            Code = (string) DALHelper.HandleDBNull(reader["Code"]),
                            CodeType = (long) DALHelper.HandleDBNull(reader["CodeType"]),
                            Concession = (bool) DALHelper.HandleDBNull(reader["Concession"]),
                            DoctorShare = (bool) DALHelper.HandleDBNull(reader["DoctorShare"]),
                            DoctorShareAmount = (decimal) DALHelper.HandleDBNull(reader["DoctorShareAmount"]),
                            DoctorSharePercentage = (decimal) DALHelper.HandleDBNull(reader["DoctorSharePercentage"]),
                            InHouse = (bool) DALHelper.HandleDBNull(reader["InHouse"]),
                            LongDescription = (string) DALHelper.HandleDBNull(reader["LongDescription"]),
                            MaxRate = (decimal) DALHelper.HandleDBNull(reader["MaxRate"]),
                            MinRate = (decimal) DALHelper.HandleDBNull(reader["MinRate"]),
                            RateEditable = (bool) DALHelper.HandleDBNull(reader["RateEditable"]),
                            ServiceTax = (bool) DALHelper.HandleDBNull(reader["ServiceTax"]),
                            ShortDescription = (string) DALHelper.HandleDBNull(reader["ShortDescription"]),
                            Specialization = (long) DALHelper.HandleDBNull(reader["SpecializationId"]),
                            StaffDependantDiscount = (bool) DALHelper.HandleDBNull(reader["StaffDependantDiscount"]),
                            StaffDiscount = (bool) DALHelper.HandleDBNull(reader["StaffDiscount"]),
                            SubSpecialization = (long) DALHelper.HandleDBNull(reader["SubSpecializationId"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"])
                        };
                        nvo.ServiceList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetServiceClassRateList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceWiseClassRateBizActionVO nvo = valueObject as clsGetServiceWiseClassRateBizActionVO;
            nvo.ServiceClassList = new List<clsServiceMasterVO>();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetServiceClassRateList");
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsServiceMasterVO item = new clsServiceMasterVO {
                            ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassId"])),
                            Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]))
                        };
                        nvo.ServiceClassList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetServiceList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceMasterListBizActionVO nvo = (clsGetServiceMasterListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetServiceDetail");
                if ((nvo.ServiceName != null) && (nvo.ServiceName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceName", DbType.String, nvo.ServiceName);
                }
                this.dbServer.AddInParameter(storedProcCommand, "Specialization", DbType.Int64, nvo.Specialization);
                this.dbServer.AddInParameter(storedProcCommand, "SubSpecialization", DbType.Int64, nvo.SubSpecialization);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.IsStatus);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ServiceName");
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ServiceList == null)
                    {
                        nvo.ServiceList = new List<clsServiceMasterVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                            break;
                        }
                        clsServiceMasterVO item = new clsServiceMasterVO {
                            ID = (long) reader["ID"],
                            ServiceName = (string) DALHelper.HandleDBNull(reader["ServiceName"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            ServiceCode = (string) DALHelper.HandleDBNull(reader["ServiceCode"]),
                            Specialization = (long) DALHelper.HandleDBNull(reader["SpecializationId"]),
                            SubSpecialization = (long) DALHelper.HandleDBNull(reader["SubSpecializationId"]),
                            LongDescription = (string) DALHelper.HandleDBNull(reader["LongDescription"]),
                            ShortDescription = (string) DALHelper.HandleDBNull(reader["ShortDescription"]),
                            Rate = (decimal) DALHelper.HandleDBNull(reader["Rate"]),
                            ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassID"]))
                        };
                        nvo.ServiceList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetServiceListForDocSerRateCat(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetServiceMasterListBizActionVO nvo = (clsGetServiceMasterListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetServiceDetailForDocSerRateCat");
                if ((nvo.ServiceName != null) && (nvo.ServiceName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceName", DbType.String, nvo.ServiceName);
                }
                this.dbServer.AddInParameter(storedProcCommand, "Specialization", DbType.Int64, nvo.Specialization);
                this.dbServer.AddInParameter(storedProcCommand, "ClassID", DbType.Int64, nvo.ServiceMaster.ClassID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ServiceName");
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ServiceList == null)
                    {
                        nvo.ServiceList = new List<clsServiceMasterVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                            break;
                        }
                        clsServiceMasterVO item = new clsServiceMasterVO {
                            ID = (long) reader["ID"],
                            ServiceName = (string) DALHelper.HandleDBNull(reader["ServiceName"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            ServiceCode = (string) DALHelper.HandleDBNull(reader["ServiceCode"]),
                            Specialization = (long) DALHelper.HandleDBNull(reader["SpecializationId"]),
                            SubSpecialization = (long) DALHelper.HandleDBNull(reader["SubSpecializationId"]),
                            LongDescription = (string) DALHelper.HandleDBNull(reader["LongDescription"]),
                            ShortDescription = (string) DALHelper.HandleDBNull(reader["ShortDescription"]),
                            Rate = (decimal) DALHelper.HandleDBNull(reader["Rate"]),
                            ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassID"])),
                            ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]))
                        };
                        nvo.ServiceList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetServiceListForPathology(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceMasterListBizActionVO nvo = (clsGetServiceMasterListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetServiceListForPathology");
                if ((nvo.ServiceName != null) && (nvo.ServiceName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceName", DbType.String, nvo.ServiceName);
                }
                this.dbServer.AddInParameter(storedProcCommand, "Specialization", DbType.Int64, nvo.Specialization);
                this.dbServer.AddInParameter(storedProcCommand, "SubSpecialization", DbType.Int64, nvo.SubSpecialization);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, 1);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ServiceName");
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ServiceList == null)
                    {
                        nvo.ServiceList = new List<clsServiceMasterVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                            break;
                        }
                        clsServiceMasterVO item = new clsServiceMasterVO {
                            ID = (long) reader["ID"],
                            ServiceName = (string) DALHelper.HandleDBNull(reader["ServiceName"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            ServiceCode = (string) DALHelper.HandleDBNull(reader["ServiceCode"]),
                            Specialization = (long) DALHelper.HandleDBNull(reader["SpecializationId"]),
                            SubSpecialization = (long) DALHelper.HandleDBNull(reader["SubSpecializationId"]),
                            LongDescription = (string) DALHelper.HandleDBNull(reader["LongDescription"]),
                            ShortDescription = (string) DALHelper.HandleDBNull(reader["ShortDescription"]),
                            Rate = (decimal) DALHelper.HandleDBNull(reader["Rate"]),
                            ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassID"]))
                        };
                        nvo.ServiceList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetServiceMasterDetailsForId(IValueObject valueObject, clsUserVO objUserVO)
        {
            IValueObject obj2;
            try
            {
                clsGetServiceMasterListBizActionVO nvo = valueObject as clsGetServiceMasterListBizActionVO;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAllServiceMasterDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceMaster.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.ServiceMaster.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                while (true)
                {
                    if (!reader.Read())
                    {
                        reader.Close();
                        obj2 = nvo;
                        break;
                    }
                    nvo.ServiceMaster.ServiceCode = (string) DALHelper.HandleDBNull(reader["ServiceCode"]);
                    nvo.ServiceMaster.CodeType = (long) DALHelper.HandleDBNull(reader["CodeType"]);
                    nvo.ServiceMaster.Code = (string) DALHelper.HandleDBNull(reader["Code"]);
                    nvo.ServiceMaster.Specialization = (long) DALHelper.HandleDBNull(reader["SpecializationId"]);
                    nvo.ServiceMaster.SubSpecialization = (long) DALHelper.HandleDBNull(reader["SubSpecializationId"]);
                    nvo.ServiceMaster.Description = (string) DALHelper.HandleDBNull(reader["Description"]);
                    nvo.ServiceMaster.ShortDescription = (string) DALHelper.HandleDBNull(reader["ShortDescription"]);
                    nvo.ServiceMaster.LongDescription = (string) DALHelper.HandleDBNull(reader["LongDescription"]);
                    nvo.ServiceMaster.StaffDiscount = (bool) DALHelper.HandleDBNull(reader["StaffDiscount"]);
                    nvo.ServiceMaster.StaffDiscountAmount = (decimal) DALHelper.HandleDBNull(reader["StaffDiscountAmount"]);
                    nvo.ServiceMaster.StaffDiscountPercent = (decimal) DALHelper.HandleDBNull(reader["StaffDiscountPercent"]);
                    nvo.ServiceMaster.StaffDependantDiscount = (bool) DALHelper.HandleDBNull(reader["StaffDependantDiscount"]);
                    nvo.ServiceMaster.StaffDependantDiscountAmount = (decimal) DALHelper.HandleDBNull(reader["StaffDependantDiscountAmount"]);
                    nvo.ServiceMaster.StaffDependantDiscountPercent = (decimal) DALHelper.HandleDBNull(reader["StaffDependantDiscountPercent"]);
                    nvo.ServiceMaster.Concession = (bool) DALHelper.HandleDBNull(reader["Concession"]);
                    nvo.ServiceMaster.ConcessionAmount = (decimal) DALHelper.HandleDBNull(reader["ConcessionAmount"]);
                    nvo.ServiceMaster.ConcessionPercent = (decimal) DALHelper.HandleDBNull(reader["ConcessionPercent"]);
                    nvo.ServiceMaster.ServiceTax = (bool) DALHelper.HandleDBNull(reader["ServiceTax"]);
                    nvo.ServiceMaster.ServiceTaxAmount = (decimal) DALHelper.HandleDBNull(reader["ServiceTaxAmount"]);
                    nvo.ServiceMaster.ServiceTaxPercent = (decimal) DALHelper.HandleDBNull(reader["ServiceTaxPercent"]);
                    nvo.ServiceMaster.InHouse = (bool) DALHelper.HandleDBNull(reader["InHouse"]);
                    nvo.ServiceMaster.DoctorShare = (bool) DALHelper.HandleDBNull(reader["DoctorShare"]);
                    nvo.ServiceMaster.DoctorSharePercentage = (decimal) DALHelper.HandleDBNull(reader["DoctorSharePercentage"]);
                    nvo.ServiceMaster.DoctorShareAmount = (decimal) DALHelper.HandleDBNull(reader["DoctorShareAmount"]);
                    nvo.ServiceMaster.RateEditable = (bool) DALHelper.HandleDBNull(reader["RateEditable"]);
                    nvo.ServiceMaster.MaxRate = (decimal) DALHelper.HandleDBNull(reader["MaxRate"]);
                    nvo.ServiceMaster.MinRate = (decimal) DALHelper.HandleDBNull(reader["MinRate"]);
                    nvo.ServiceMaster.CheckedAllTariffs = (bool) DALHelper.HandleDBNull(reader["CheckedAllTariffs"]);
                    nvo.ServiceMaster.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                    nvo.ServiceMaster.ServiceName = nvo.ServiceMaster.Description;
                    nvo.ServiceMaster.IsPackage = (bool) DALHelper.HandleDBNull(reader["IsPackage"]);
                    nvo.ServiceMaster.IsFavourite = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFavourite"]));
                    nvo.ServiceMaster.IsLinkWithInventory = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IslinkWithInventory"]));
                    nvo.ServiceMaster.SeniorCitizen = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["SeniorCitizen"]));
                    nvo.ServiceMaster.SeniorCitizenConAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SeniorCitizenConAmount"]));
                    nvo.ServiceMaster.SeniorCitizenConPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SeniorCitizenConPercent"]));
                    nvo.ServiceMaster.SeniorCitizenAge = Convert.ToInt16(DALHelper.HandleDBNull(reader["SeniorCitizenAge"]));
                    nvo.ServiceMaster.LuxuryTaxAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["LuxuryTaxAmount"]));
                    nvo.ServiceMaster.LuxuryTaxPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["LuxuryTaxPercent"]));
                    nvo.ServiceMaster.SACCodeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SACCodeID"]));
                    if (!nvo.IsOLDServiceMaster)
                    {
                        nvo.ServiceMaster.CodeDetails = Convert.ToString(DALHelper.HandleDBNull(reader["CodeDetails"]));
                        nvo.ServiceMaster.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["BaseServiceRate"]));
                        nvo.ServiceList = new List<clsServiceMasterVO>();
                        reader.NextResult();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                clsServiceMasterVO item = new clsServiceMasterVO {
                                    ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassId"])),
                                    Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"])),
                                    Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                                    UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                                    ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]))
                                };
                                nvo.ServiceList.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return obj2;
        }

        public override IValueObject GetServiceTariff(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceTariffBizActionVO nvo = valueObject as clsGetServiceTariffBizActionVO;
            nvo.ServiceList = new List<clsServiceMasterVO>();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ServiceTariffDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceMaster.ServiceID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            break;
                        }
                        clsServiceMasterVO item = new clsServiceMasterVO {
                            TariffID = (long) reader["TariffID"]
                        };
                        nvo.ServiceList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetServiceTariffClassList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetTariffServiceClassBizActionVO nvo = (clsGetTariffServiceClassBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetTariffServiceClassList");
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "TariffID ", DbType.Int64, nvo.TariffID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ClassList == null)
                    {
                        nvo.ClassList = new List<clsServiceTarrifClassRateDetailsVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            break;
                        }
                        clsServiceTarrifClassRateDetailsVO item = new clsServiceTarrifClassRateDetailsVO {
                            ClassID = (long) reader["ID"],
                            ClassName = (string) DALHelper.HandleDBNull(reader["ClassName"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            Rate = (double) DALHelper.HandleDBNull(reader["Rate"])
                        };
                        nvo.ClassList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        private List<long> GetServiceTarrifIds(long serviceID, long unitId)
        {
            List<long> list2;
            List<long> list = new List<long>();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ServiceTariffDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, serviceID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        long item = (long) reader["Id"];
                        list.Add(item);
                    }
                }
                list2 = list;
            }
            catch (Exception)
            {
                throw;
            }
            return list2;
        }

        public override IValueObject GetServiceTaxDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceTaxDetailsBizActionVO nvo = (clsGetServiceTaxDetailsBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetServiceTaxDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ServiceId", DbType.Int64, nvo.ServiceTaxDetailsVO.ServiceId);
                this.dbServer.AddInParameter(storedProcCommand, "ClassID", DbType.Int64, nvo.ServiceTaxDetailsVO.ClassId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ServiceTaxDetailsVOList == null)
                    {
                        nvo.ServiceTaxDetailsVOList = new List<clsServiceTaxVO>();
                    }
                    while (reader.Read())
                    {
                        clsServiceTaxVO item = new clsServiceTaxVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            ServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"])),
                            TariffId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"])),
                            ClassId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassID"])),
                            TaxID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TaxID"])),
                            Percentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TaxPercentage"])),
                            TaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["TaxType"])),
                            IsTaxLimitApplicable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsTaxLimitApplicable"])),
                            TaxLimit = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TaxLimit"])),
                            ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"])),
                            TaxName = Convert.ToString(DALHelper.HandleDBNull(reader["TaxName"])),
                            ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"])),
                            UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"])),
                            status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.ServiceTaxDetailsVOList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetSpecializationDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetSpecializationDetailsBizActionVO nvo = valueObject as clsGetSpecializationDetailsBizActionVO;
            clsSpecializationVO item = null;
            if (nvo.IsFromAgency)
            {
                try
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillSpecializationComboForAgencyServiceLink");
                    this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, nvo.AgencyID);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.MasterList == null)
                        {
                            nvo.MasterList = new List<MasterListItem>();
                        }
                        while (reader.Read())
                        {
                            nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString()));
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                try
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSpecializationDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SerachExpression);
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                    this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            item = new clsSpecializationVO {
                                SpecializationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"])),
                                Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                                SpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                                Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                                IsGenerateToken = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsGenerateToken"])),
                                SubSpeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpeID"]))
                            };
                            nvo.ItemMatserDetails.Add(item);
                        }
                    }
                    reader.NextResult();
                    nvo.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if ((reader != null) && !reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }
            return nvo;
        }

        public override IValueObject GetSubSpecializationDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetSubSpecializationDetailsBizActionVO nvo = valueObject as clsGetSubSpecializationDetailsBizActionVO;
            clsSubSpecializationVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSubSpecializationDetails");
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsSubSpecializationVO {
                            SubSpecializationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"])),
                            SpecializationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["fkSpecializationID"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            SpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader["Specilaization"])),
                            SubSpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.ItemMatserDetails.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }

        public override IValueObject GetTariffDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetTariffDetailsBizActionVO nvo = valueObject as clsGetTariffDetailsBizActionVO;
            clsTariffVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetTariffDetails");
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsTariffVO {
                            Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.ItemMatserDetails.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }

        public override IValueObject GetTariffService(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetTariffServiceBizActionVO nvo = valueObject as clsGetTariffServiceBizActionVO;
            nvo.ServiceList = new List<clsServiceMasterVO>();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIDsFromTariffServiceMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceMaster.ServiceID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            break;
                        }
                        clsServiceMasterVO item = new clsServiceMasterVO {
                            TariffServiceMasterID = (long) reader["ID"]
                        };
                        nvo.ServiceList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetTariffServiceClassRate(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetTariffServiceClassRateBizActionVO nvo = (clsGetTariffServiceClassRateBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_TariffServiceClassRateDetails");
                this.dbServer.AddInParameter(storedProcCommand, "TariffServiceID", DbType.Int64, nvo.ServiceMaster.TariffServiceMasterID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ServiceMaster == null)
                    {
                        nvo.ServiceMaster = new clsServiceMasterVO();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            break;
                        }
                        nvo.ServiceMaster.Rate = (decimal) reader["Rate"];
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetTariffServiceClassRateNew(IValueObject valueObject, clsUserVO objUserVo)
        {
            clsGetTariffServiceClassRateNewBizActionVO nvo = (clsGetTariffServiceClassRateNewBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetServiceTariffClassRateDetails_New");
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "TariffName", DbType.String, nvo.TariffName);
                this.dbServer.AddInParameter(storedProcCommand, "OperationType", DbType.Int32, nvo.OperationType);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.SortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ServiceList == null)
                    {
                        nvo.ServiceList = new List<clsServiceTarrifClassRateDetailsNewVO>();
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
                        clsServiceTarrifClassRateDetailsNewVO item = new clsServiceTarrifClassRateDetailsNewVO {
                            TSMID = Convert.ToInt64(reader["TSM"]),
                            ServiceID = Convert.ToInt64(reader["ServiceId"]),
                            TariffID = Convert.ToInt64(reader["TariffId"]),
                            TariffName = Convert.ToString(reader["TariffDescription"]),
                            ClassID = Convert.ToInt64(reader["ClassId"]),
                            Rate = Convert.ToDecimal(reader["Rate"])
                        };
                        if (nvo.OperationType == 1)
                        {
                            item.ClassName = Convert.ToString(reader["ClassName"]);
                        }
                        nvo.ServiceList.Add(item);
                    }
                }
                if (nvo.ServiceList != null)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_GetExistClassANDRate");
                    this.dbServer.AddInParameter(command2, "ServiceID", DbType.Int64, nvo.ServiceID);
                    DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(command2);
                    if (reader2.HasRows)
                    {
                        if (nvo.ExistingClassRates == null)
                        {
                            nvo.ExistingClassRates = new List<clsServiceTarrifClassRateDetailsNewVO>();
                        }
                        while (true)
                        {
                            if (!reader2.Read())
                            {
                                reader2.NextResult();
                                reader2.Close();
                                break;
                            }
                            clsServiceTarrifClassRateDetailsNewVO item = new clsServiceTarrifClassRateDetailsNewVO {
                                ServiceID = Convert.ToInt64(reader2["ServiceId"]),
                                TariffID = Convert.ToInt64(reader2["TariffId"]),
                                ClassID = Convert.ToInt64(reader2["ClassId"]),
                                ClassName = Convert.ToString(reader2["ClassName"]),
                                Rate = Convert.ToDecimal(reader2["Rate"])
                            };
                            nvo.ExistingClassRates.Add(item);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetTariffServiceList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetTariffServiceListBizActionVO nvo = (clsGetTariffServiceListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand;
                if (nvo.PrescribedService)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetTariffServiceListForPackage");
                    if (nvo.UsePackageSubsql)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "UsePackageSubsql", DbType.Boolean, nvo.UsePackageSubsql);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "ISOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows1);
                    this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                    if (!nvo.IsPackage)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int64, nvo.ForFilterPackageID);
                    }
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetTariffServiceListNewForPackage3");
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                    if (nvo.IsPackage)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "IsPackage", DbType.Boolean, nvo.IsPackage);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int64, nvo.ForFilterPackageID);
                    }
                    if (nvo.UsePackageSubsql)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "UsePackageSubsql", DbType.Boolean, nvo.UsePackageSubsql);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "SponsorID", DbType.Int64, nvo.SponsorID);
                    this.dbServer.AddInParameter(storedProcCommand, "SponsorUnitID", DbType.Int64, nvo.SponsorUnitID);
                }
                if ((nvo.ServiceName != null) && (nvo.ServiceName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceName", DbType.String, nvo.ServiceName);
                }
                this.dbServer.AddInParameter(storedProcCommand, "Specialization", DbType.Int64, nvo.Specialization);
                this.dbServer.AddInParameter(storedProcCommand, "SubSpecialization", DbType.Int64, nvo.SubSpecialization);
                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, nvo.TariffID);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientSourceType", DbType.Int16, nvo.PatientSourceType);
                this.dbServer.AddInParameter(storedProcCommand, "PatientSourceTypeID", DbType.Int64, nvo.PatientSourceTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "GetSuggestedServices", DbType.Boolean, nvo.GetSuggestedServices);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ChargeID", DbType.Int64, nvo.ChargeID);
                if ((nvo.SearchExpression != null) && (nvo.SearchExpression.Length > 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.SearchExpression);
                }
                if (nvo.ClassID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ClassID", DbType.Int64, nvo.ClassID);
                }
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ServiceList == null)
                    {
                        nvo.ServiceList = new List<clsServiceMasterVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            if (nvo.PrescribedService)
                            {
                                reader.NextResult();
                                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                            }
                            reader.NextResult();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PatientId"]));
                                }
                            }
                            if (!nvo.PrescribedService)
                            {
                                reader.NextResult();
                                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                            }
                            reader.Close();
                            break;
                        }
                        clsServiceMasterVO item = new clsServiceMasterVO {
                            ID = Convert.ToInt64(reader["ServiceID"]),
                            TariffServiceMasterID = Convert.ToInt64(reader["ID"]),
                            ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"])),
                            TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"])),
                            Specialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationID"])),
                            SubSpecialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"]))
                        };
                        if (nvo.PatientSourceType == 2)
                        {
                            item.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                            item.ConcessionAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                            item.ConcessionPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                        }
                        else if (nvo.PatientSourceType == 1)
                        {
                            item.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                            item.ConcessionAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                            item.ConcessionPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                        }
                        else
                        {
                            item.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                            item.SeniorCitizen = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["SeniorCitizen"]));
                            item.SeniorCitizenConAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SeniorCitizenConAmount"]));
                            item.SeniorCitizenConPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SeniorCitizenConPercent"]));
                            item.SeniorCitizenAge = Convert.ToInt32(DALHelper.HandleDBNull(reader["SeniorCitizenAge"]));
                            if (item.SeniorCitizen && (nvo.Age >= item.SeniorCitizenAge))
                            {
                                item.ConcessionAmount = item.SeniorCitizenConAmount;
                                item.ConcessionPercent = item.SeniorCitizenConPercent;
                            }
                            else
                            {
                                item.ConcessionAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                                item.ConcessionPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                            }
                            item.PatientCategoryL3 = Convert.ToString(DALHelper.HandleDBNull(reader["TariffName"]));
                            item.ProcessID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcessID"]));
                            item.ProcessName = Convert.ToString(DALHelper.HandleDBNull(reader["ProcessName"]));
                            item.AdjustableHeadType = Convert.ToInt32(DALHelper.HandleDBNull(reader["AdjustableHeadType"]));
                        }
                        item.StaffDiscount = Convert.ToBoolean(DALHelper.HandleDBNull(reader["StaffDiscount"]));
                        item.StaffDiscountAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDiscountAmount"]));
                        item.StaffDiscountPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDiscountPercent"]));
                        item.StaffDependantDiscount = Convert.ToBoolean(DALHelper.HandleDBNull(reader["StaffDependantDiscount"]));
                        item.StaffDependantDiscountAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDependantDiscountAmount"]));
                        item.StaffDependantDiscountPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDependantDiscountPercent"]));
                        item.Concession = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Concession"]));
                        item.ServiceTax = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ServiceTax"]));
                        item.ServiceTaxAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceTaxAmount"]));
                        item.ServiceTaxPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceTaxPercent"]));
                        item.InHouse = Convert.ToBoolean(DALHelper.HandleDBNull(reader["InHouse"]));
                        item.DoctorShare = Convert.ToBoolean(DALHelper.HandleDBNull(reader["DoctorShare"]));
                        item.DoctorSharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorSharePercentage"]));
                        item.DoctorShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorShareAmount"]));
                        item.RateEditable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["RateEditable"]));
                        item.MaxRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MaxRate"]));
                        item.MinRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MinRate"]));
                        item.TarrifCode = Convert.ToString(DALHelper.HandleDBNull(reader["TarrifServiceCode"]));
                        item.TarrifName = Convert.ToString(DALHelper.HandleDBNull(reader["TarrifServiceName"]));
                        item.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        item.CodeType = (long) DALHelper.HandleDBNull(reader["CodeType"]);
                        item.ShortDescription = Convert.ToString(DALHelper.HandleDBNull(reader["ShortDescription"]));
                        item.LongDescription = Convert.ToString(DALHelper.HandleDBNull(reader["LongDescription"]));
                        item.SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["Specialization"]));
                        item.SubSpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecialization"]));
                        item.IsPackage = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackage"]));
                        item.PackageID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PackageID"]));
                        item.IsMarkUp = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsMarkUp"]));
                        if (nvo.PrescribedService && (nvo.ForFilterPackageID == 0L))
                        {
                            item.Billed = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["Billed"]));
                            item.VisitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["VisitID"]));
                            item.PrescriptionDetailsID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PreDetailsID"]));
                            item.InvestigationDetailsID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["InvDetailsID"]));
                            item.InvestigationBilled = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["InvBilled"]));
                        }
                        if (nvo.ForFilterPackageID > 0L)
                        {
                            item.ServiceComponentRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceComponentRate"]));
                            item.IsAdjustableHead = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAdjustableHead"]));
                            item.IsConsiderAdjustable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsConsiderAdjustable"]));
                            item.SumOfExludedServices = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SumOfExludedServices"]));
                        }
                        if (nvo.UsePackageSubsql)
                        {
                            item.ApplicableTo = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ApplicableTo"]));
                            item.ApplicableToString = Convert.ToString(DALHelper.HandleDBNull(reader["ApplicableToString"]));
                            item.PackageServiceConditionID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PackageServiceConditionID"]));
                        }
                        if (nvo.PrescribedService)
                        {
                            if (!nvo.IsOPDIPD)
                            {
                                item.RoundDate = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                            }
                            else
                            {
                                bool? getSuggestedServices = nvo.GetSuggestedServices;
                                if (getSuggestedServices.GetValueOrDefault() && (getSuggestedServices != null))
                                {
                                    item.RoundDate = Convert.ToDateTime(DALHelper.HandleDate(reader["RoundDate"]));
                                    item.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["drName"]));
                                    item.SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["RoundSpecialization"]));
                                }
                            }
                            item.IsBilledEMR = Convert.ToInt64(DALHelper.HandleDBNull(reader["Isbilled"])) != 0L;
                        }
                        nvo.ServiceList.Add(item);
                    }
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetTariffServiceListForPathology(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetTariffServiceListBizActionForPathologyVO yvo = (clsGetTariffServiceListBizActionForPathologyVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetTariffServiceListNewForPackage3");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, yvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, yvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, yvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                if (yvo.IsPackage)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IsPackage", DbType.Boolean, yvo.IsPackage);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int64, yvo.ForFilterPackageID);
                }
                if (yvo.UsePackageSubsql)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "UsePackageSubsql", DbType.Boolean, yvo.UsePackageSubsql);
                }
                this.dbServer.AddInParameter(storedProcCommand, "SponsorID", DbType.Int64, yvo.SponsorID);
                this.dbServer.AddInParameter(storedProcCommand, "SponsorUnitID", DbType.Int64, yvo.SponsorUnitID);
                if ((yvo.ServiceName != null) && (yvo.ServiceName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceName", DbType.String, yvo.ServiceName);
                }
                this.dbServer.AddInParameter(storedProcCommand, "Specialization", DbType.Int64, yvo.Specialization);
                this.dbServer.AddInParameter(storedProcCommand, "SubSpecialization", DbType.Int64, yvo.SubSpecialization);
                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, yvo.TariffID);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, yvo.ServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientSourceType", DbType.Int16, yvo.PatientSourceType);
                this.dbServer.AddInParameter(storedProcCommand, "PatientSourceTypeID", DbType.Int64, yvo.PatientSourceTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, yvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, yvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "GetSuggestedServices", DbType.Boolean, yvo.GetSuggestedServices);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, yvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, yvo.UnitID);
                if ((yvo.SearchExpression != null) && (yvo.SearchExpression.Length > 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, yvo.SearchExpression);
                }
                if (yvo.ClassID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ClassID", DbType.Int64, yvo.ClassID);
                }
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (yvo.ServiceList == null)
                    {
                        yvo.ServiceList = new List<clsServiceMasterVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PatientId"]));
                                }
                            }
                            if (!yvo.PrescribedService)
                            {
                                reader.NextResult();
                                yvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                            }
                            reader.Close();
                            break;
                        }
                        clsServiceMasterVO item = new clsServiceMasterVO {
                            ID = Convert.ToInt64(reader["ServiceID"]),
                            TariffServiceMasterID = Convert.ToInt64(reader["ID"]),
                            ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"])),
                            TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"])),
                            Specialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationID"])),
                            SubSpecialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"]))
                        };
                        if (yvo.PatientSourceType == 2)
                        {
                            item.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                            item.ConcessionAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                            item.ConcessionPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                        }
                        else if (yvo.PatientSourceType == 1)
                        {
                            item.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                            item.ConcessionAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                            item.ConcessionPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                        }
                        else
                        {
                            item.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                            item.SeniorCitizen = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["SeniorCitizen"]));
                            item.SeniorCitizenConAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SeniorCitizenConAmount"]));
                            item.SeniorCitizenConPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SeniorCitizenConPercent"]));
                            item.SeniorCitizenAge = Convert.ToInt32(DALHelper.HandleDBNull(reader["SeniorCitizenAge"]));
                            if (item.SeniorCitizen && (yvo.Age >= item.SeniorCitizenAge))
                            {
                                item.ConcessionAmount = item.SeniorCitizenConAmount;
                                item.ConcessionPercent = item.SeniorCitizenConPercent;
                            }
                            else
                            {
                                item.ConcessionAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                                item.ConcessionPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                            }
                            if (!yvo.PrescribedService)
                            {
                                item.PatientCategoryL3 = Convert.ToString(DALHelper.HandleDBNull(reader["TariffName"]));
                            }
                        }
                        item.StaffDiscount = Convert.ToBoolean(DALHelper.HandleDBNull(reader["StaffDiscount"]));
                        item.StaffDiscountAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDiscountAmount"]));
                        item.StaffDiscountPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDiscountPercent"]));
                        item.StaffDependantDiscount = Convert.ToBoolean(DALHelper.HandleDBNull(reader["StaffDependantDiscount"]));
                        item.StaffDependantDiscountAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDependantDiscountAmount"]));
                        item.StaffDependantDiscountPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDependantDiscountPercent"]));
                        item.Concession = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Concession"]));
                        item.ServiceTax = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ServiceTax"]));
                        item.ServiceTaxAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceTaxAmount"]));
                        item.ServiceTaxPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceTaxPercent"]));
                        item.InHouse = Convert.ToBoolean(DALHelper.HandleDBNull(reader["InHouse"]));
                        item.DoctorShare = Convert.ToBoolean(DALHelper.HandleDBNull(reader["DoctorShare"]));
                        item.DoctorSharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorSharePercentage"]));
                        item.DoctorShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorShareAmount"]));
                        item.RateEditable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["RateEditable"]));
                        item.MaxRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MaxRate"]));
                        item.MinRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MinRate"]));
                        item.TarrifCode = Convert.ToString(DALHelper.HandleDBNull(reader["TarrifServiceCode"]));
                        item.TarrifName = Convert.ToString(DALHelper.HandleDBNull(reader["TarrifServiceName"]));
                        item.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        item.CodeType = (long) DALHelper.HandleDBNull(reader["CodeType"]);
                        item.ShortDescription = Convert.ToString(DALHelper.HandleDBNull(reader["ShortDescription"]));
                        item.LongDescription = Convert.ToString(DALHelper.HandleDBNull(reader["LongDescription"]));
                        item.SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["Specialization"]));
                        item.SubSpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecialization"]));
                        item.IsPackage = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackage"]));
                        item.PackageID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PackageID"]));
                        item.IsMarkUp = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsMarkUp"]));
                        if (yvo.UsePackageSubsql)
                        {
                            item.ApplicableTo = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ApplicableTo"]));
                            item.ApplicableToString = Convert.ToString(DALHelper.HandleDBNull(reader["ApplicableToString"]));
                            item.PackageServiceConditionID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PackageServiceConditionID"]));
                        }
                        yvo.ServiceList.Add(item);
                    }
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return yvo;
        }

        public override IValueObject GetTariffServiceMasterID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceMasterListBizActionVO nvo = (clsGetServiceMasterListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetTariffServiceMasterID");
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceMaster.ServiceID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ServiceList == null)
                    {
                        nvo.ServiceList = new List<clsServiceMasterVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            break;
                        }
                        clsServiceMasterVO item = new clsServiceMasterVO {
                            TariffServiceMasterID = (long) reader["ID"]
                        };
                        nvo.ServiceList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetTariffServiceMasterList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetTariffServiceMasterListBizActionVO nvo = (clsGetTariffServiceMasterListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetTariffServiceMasterList");
                if ((nvo.ServiceName != null) && (nvo.ServiceName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceName", DbType.String, nvo.ServiceName);
                }
                this.dbServer.AddInParameter(storedProcCommand, "Specialization", DbType.Int64, nvo.Specialization);
                this.dbServer.AddInParameter(storedProcCommand, "SubSpecialization", DbType.Int64, nvo.SubSpecialization);
                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, nvo.TariffID);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceID);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, nvo.TotalRows);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ServiceList == null)
                    {
                        nvo.ServiceList = new List<clsServiceMasterVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                            break;
                        }
                        clsServiceMasterVO item = new clsServiceMasterVO {
                            ID = (long) reader["ServiceID"],
                            TariffServiceMasterID = (long) reader["ID"],
                            ServiceName = (string) DALHelper.HandleDBNull(reader["ServiceName"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            ServiceCode = (string) DALHelper.HandleDBNull(reader["ServiceCode"]),
                            TariffID = (long) DALHelper.HandleDBNull(reader["TariffID"]),
                            Specialization = (long) DALHelper.HandleDBNull(reader["SpecializationID"]),
                            SubSpecialization = (long) DALHelper.HandleDBNull(reader["SubSpecializationId"]),
                            BaseServiceRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["BaseServiceRate"])),
                            Rate = (decimal) DALHelper.HandleDBNull(reader["Rate"]),
                            ConcessionAmount = (decimal) DALHelper.HandleDBNull(reader["ConcessionAmount"]),
                            ConcessionPercent = (decimal) DALHelper.HandleDBNull(reader["ConcessionPercent"]),
                            StaffDiscount = (bool) DALHelper.HandleDBNull(reader["StaffDiscount"]),
                            StaffDiscountAmount = (decimal) DALHelper.HandleDBNull(reader["StaffDiscountAmount"]),
                            StaffDiscountPercent = (decimal) DALHelper.HandleDBNull(reader["StaffDiscountPercent"]),
                            StaffDependantDiscount = (bool) DALHelper.HandleDBNull(reader["StaffDependantDiscount"]),
                            StaffDependantDiscountAmount = (decimal) DALHelper.HandleDBNull(reader["StaffDependantDiscountAmount"]),
                            StaffDependantDiscountPercent = (decimal) DALHelper.HandleDBNull(reader["StaffDependantDiscountPercent"]),
                            Concession = (bool) DALHelper.HandleDBNull(reader["Concession"]),
                            ServiceTax = (bool) DALHelper.HandleDBNull(reader["ServiceTax"]),
                            ServiceTaxAmount = (decimal) DALHelper.HandleDBNull(reader["ServiceTaxAmount"]),
                            ServiceTaxPercent = (decimal) DALHelper.HandleDBNull(reader["ServiceTaxPercent"]),
                            InHouse = (bool) DALHelper.HandleDBNull(reader["InHouse"]),
                            DoctorShare = (bool) DALHelper.HandleDBNull(reader["DoctorShare"]),
                            DoctorSharePercentage = (decimal) DALHelper.HandleDBNull(reader["DoctorSharePercentage"]),
                            DoctorShareAmount = (decimal) DALHelper.HandleDBNull(reader["DoctorShareAmount"]),
                            RateEditable = (bool) DALHelper.HandleDBNull(reader["RateEditable"]),
                            MaxRate = (decimal) DALHelper.HandleDBNull(reader["MaxRate"]),
                            MinRate = (decimal) DALHelper.HandleDBNull(reader["MinRate"]),
                            TarrifCode = (string) DALHelper.HandleDBNull(reader["TarrifServiceCode"]),
                            TarrifName = (string) DALHelper.HandleDBNull(reader["TariffName"]),
                            Code = (string) DALHelper.HandleDBNull(reader["Code"]),
                            CodeType = (long) DALHelper.HandleDBNull(reader["CodeType"]),
                            ShortDescription = (string) DALHelper.HandleDBNull(reader["ShortDescription"]),
                            LongDescription = (string) DALHelper.HandleDBNull(reader["LongDescription"]),
                            SpecializationString = (string) DALHelper.HandleDBNull(reader["Specialization"]),
                            SubSpecializationString = (string) DALHelper.HandleDBNull(reader["SubSpecialization"]),
                            ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]))
                        };
                        nvo.ServiceList.Add(item);
                    }
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetUnSelectedRecordForCategoryComboBox(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetUnSelectedRecordForCategoryComboBoxBizActionVO nvo = valueObject as clsGetUnSelectedRecordForCategoryComboBoxBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetUnSelectedRecordForComboBox");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsFromDocSerLinling", DbType.Boolean, nvo.IsFromDocSerLinling);
                this.dbServer.AddOutParameter(storedProcCommand, "Status", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "Status"));
                if (reader.HasRows)
                {
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }

        public override IValueObject GetUnSelectedRecordForClinicComboBox(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetUnSelectedRecordForCategoryComboBoxBizActionVO nvo = valueObject as clsGetUnSelectedRecordForCategoryComboBoxBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetUnSelectedRecordForComboBox");
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsFromDocSerLinling", DbType.Boolean, nvo.IsFromDocSerLinling);
                this.dbServer.AddOutParameter(storedProcCommand, "Status", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "Status"));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }

        public override IValueObject ModifyServiceClassRates(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUpdateServiceMasterTariffBizActionVO nvo = valueObject as clsAddUpdateServiceMasterTariffBizActionVO;
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = null;
                if ((nvo.SelectedTariffClassList != null) && (nvo.SelectedTariffClassList.Count > 0))
                {
                    foreach (clsServiceTarrifClassRateDetailsNewVO wvo in nvo.SelectedTariffClassList)
                    {
                        if (!nvo.IsRemoveTariffClassRatesLink)
                        {
                            storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ModifyTariffClassRates");
                        }
                        else
                        {
                            storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_RemoveTariffClassRatesLink_New");
                            this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceID);
                        this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.String, wvo.TariffID);
                        this.dbServer.AddInParameter(storedProcCommand, "ClassID", DbType.String, wvo.ClassID);
                        this.dbServer.AddInParameter(storedProcCommand, "Rate", DbType.String, wvo.Rate);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                        this.dbServer.AddOutParameter(storedProcCommand, "SucessStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "SucessStatus");
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
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Dispose();
                transaction.Dispose();
            }
            return nvo;
        }

        private clsAddUpdateCompanyDetailsBizActionVO UpdateCompanyMaster(clsAddUpdateCompanyDetailsBizActionVO BizActionObj, clsUserVO objUserVO)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsCompanyVO itemMatserDetails = BizActionObj.ItemMatserDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateCompanyMaster");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, itemMatserDetails.Id);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, itemMatserDetails.Description);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, itemMatserDetails.Code);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyTypeId", DbType.Int64, itemMatserDetails.CompanyTypeId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientCatagoryID", DbType.Int64, itemMatserDetails.PatientCatagoryID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ContactPerson", DbType.String, itemMatserDetails.ContactPerson);
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo", DbType.String, itemMatserDetails.ContactNo);
                this.dbServer.AddInParameter(storedProcCommand, "EmailId", DbType.String, itemMatserDetails.CompanyEmail);
                this.dbServer.AddInParameter(storedProcCommand, "Address", DbType.String, itemMatserDetails.CompanyAddress);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                if ((itemMatserDetails.TariffDetails != null) && (itemMatserDetails.TariffDetails.Count > 0))
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteCompanyTariffDetails");
                    this.dbServer.AddInParameter(command2, "ID", DbType.Int64, itemMatserDetails.Id);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                }
                foreach (clsTariffDetailsVO svo in itemMatserDetails.TariffDetails)
                {
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddCompanyTariffDetails");
                    this.dbServer.AddInParameter(command3, "CompanyID", DbType.Int64, itemMatserDetails.Id);
                    this.dbServer.AddInParameter(command3, "TariffID", DbType.Int64, svo.TariffID);
                    this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command3, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                    this.dbServer.ExecuteNonQuery(command3, transaction);
                    svo.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                }
                DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_UpdateCompanyLogoDetails");
                this.dbServer.AddInParameter(command4, "CompanyID", DbType.Int64, itemMatserDetails.Id);
                this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command4, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(command4, "CompanyLogoFile", DbType.Binary, BizActionObj.ItemMatserDetails.AttachedFileContent);
                this.dbServer.AddInParameter(command4, "LogoFileName", DbType.String, BizActionObj.ItemMatserDetails.AttachedFileName);
                this.dbServer.AddInParameter(command4, "Title", DbType.String, BizActionObj.ItemMatserDetails.Title);
                this.dbServer.AddInParameter(command4, "CompHeadImgFile", DbType.Binary, BizActionObj.ItemMatserDetails.AttachedHeadImgFileContent);
                this.dbServer.AddInParameter(command4, "HeadImgFileName", DbType.String, BizActionObj.ItemMatserDetails.AttachedHeadImgFileName);
                this.dbServer.AddInParameter(command4, "TitleHeadImg", DbType.String, BizActionObj.ItemMatserDetails.TitleHeaderImage);
                this.dbServer.AddInParameter(command4, "CompFootImgFile", DbType.Binary, BizActionObj.ItemMatserDetails.AttachedFootImgFileContent);
                this.dbServer.AddInParameter(command4, "FootImgFileName", DbType.String, BizActionObj.ItemMatserDetails.AttachedFootImgFileName);
                this.dbServer.AddInParameter(command4, "TitleFootImg", DbType.String, BizActionObj.ItemMatserDetails.TitleFooterImage);
                this.dbServer.AddInParameter(command4, "HeaderText", DbType.String, BizActionObj.ItemMatserDetails.HeaderText);
                this.dbServer.AddInParameter(command4, "FooterText", DbType.String, BizActionObj.ItemMatserDetails.FooterText);
                this.dbServer.AddInParameter(command4, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command4, "UpdatedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(command4, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(command4, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(command4, "UpdateWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(command4, transaction);
                transaction.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.ItemMatserDetails = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return BizActionObj;
        }

        public override IValueObject UpdateServiceMasterStatus(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddServiceMasterBizActionVO nvo = valueObject as clsAddServiceMasterBizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                clsServiceMasterVO serviceMasterDetails = nvo.ServiceMasterDetails;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateServiceMasterStatus");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, serviceMasterDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, serviceMasterDetails.Status);
                nvo.SuccessStatus = (this.dbServer.ExecuteNonQuery(storedProcCommand) <= 0) ? 0 : 1;
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject UpdateServiceTariff(IValueObject valueObject, clsUserVO userVO)
        {
            return valueObject;
        }

        public override IValueObject UpdateTariffService(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddTariffServiceBizActionVO nvo = valueObject as clsAddTariffServiceBizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                DbCommand command2 = null;
                clsServiceMasterVO serviceMasterDetails = nvo.ServiceMasterDetails;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddTariffService");
                if (nvo.TariffList.Count > 0)
                {
                    for (int i = 0; i <= (nvo.TariffList.Count - 1); i++)
                    {
                        storedProcCommand.Parameters.Clear();
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, serviceMasterDetails.UnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, Convert.ToInt64(nvo.TariffList[i]));
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, serviceMasterDetails.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceCode", DbType.String, serviceMasterDetails.ServiceCode);
                        this.dbServer.AddInParameter(storedProcCommand, "SpecializationId", DbType.Int64, serviceMasterDetails.Specialization);
                        this.dbServer.AddInParameter(storedProcCommand, "SubSpecializationId", DbType.Int64, serviceMasterDetails.SubSpecialization);
                        this.dbServer.AddInParameter(storedProcCommand, "ShortDescription", DbType.String, serviceMasterDetails.ShortDescription);
                        this.dbServer.AddInParameter(storedProcCommand, "LongDescription", DbType.String, serviceMasterDetails.LongDescription);
                        this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, serviceMasterDetails.ServiceName);
                        this.dbServer.AddInParameter(storedProcCommand, "CodeType", DbType.Int64, serviceMasterDetails.CodeType);
                        this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, serviceMasterDetails.Code);
                        this.dbServer.AddInParameter(storedProcCommand, "StaffDiscount", DbType.Boolean, serviceMasterDetails.StaffDiscount);
                        this.dbServer.AddInParameter(storedProcCommand, "StaffDiscountAmount", DbType.Decimal, serviceMasterDetails.StaffDiscountAmount);
                        this.dbServer.AddInParameter(storedProcCommand, "StaffDiscountPercent", DbType.Decimal, serviceMasterDetails.StaffDiscountPercent);
                        this.dbServer.AddInParameter(storedProcCommand, "StaffDependantDiscount", DbType.Boolean, serviceMasterDetails.StaffDependantDiscount);
                        this.dbServer.AddInParameter(storedProcCommand, "StaffDependantDiscountAmount", DbType.Decimal, serviceMasterDetails.StaffDependantDiscountAmount);
                        this.dbServer.AddInParameter(storedProcCommand, "StaffDependantDiscountPercent", DbType.Decimal, serviceMasterDetails.StaffDependantDiscountPercent);
                        this.dbServer.AddInParameter(storedProcCommand, "Concession", DbType.Boolean, serviceMasterDetails.Concession);
                        this.dbServer.AddInParameter(storedProcCommand, "ConcessionAmount", DbType.Decimal, serviceMasterDetails.ConcessionAmount);
                        this.dbServer.AddInParameter(storedProcCommand, "ConcessionPercent", DbType.Decimal, serviceMasterDetails.ConcessionPercent);
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceTax", DbType.Boolean, serviceMasterDetails.ServiceTax);
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceTaxAmount", DbType.Decimal, serviceMasterDetails.ServiceTaxAmount);
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceTaxPercent", DbType.Decimal, serviceMasterDetails.ServiceTaxPercent);
                        this.dbServer.AddInParameter(storedProcCommand, "InHouse", DbType.Boolean, serviceMasterDetails.InHouse);
                        this.dbServer.AddInParameter(storedProcCommand, "DoctorShare", DbType.Boolean, serviceMasterDetails.DoctorShare);
                        this.dbServer.AddInParameter(storedProcCommand, "DoctorSharePercentage", DbType.Decimal, serviceMasterDetails.DoctorSharePercentage);
                        this.dbServer.AddInParameter(storedProcCommand, "DoctorShareAmount", DbType.Decimal, serviceMasterDetails.DoctorShareAmount);
                        this.dbServer.AddInParameter(storedProcCommand, "RateEditable", DbType.Boolean, serviceMasterDetails.RateEditable);
                        this.dbServer.AddInParameter(storedProcCommand, "MaxRate", DbType.Decimal, serviceMasterDetails.MaxRate);
                        this.dbServer.AddInParameter(storedProcCommand, "MinRate", DbType.Decimal, serviceMasterDetails.MinRate);
                        this.dbServer.AddInParameter(storedProcCommand, "Rate", DbType.Decimal, serviceMasterDetails.Rate);
                        this.dbServer.AddInParameter(storedProcCommand, "CheckedAllTariffs", DbType.Boolean, serviceMasterDetails.CheckedAllTariffs);
                        this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, serviceMasterDetails.Status);
                        this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, serviceMasterDetails.CreatedUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, serviceMasterDetails.UpdatedUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, serviceMasterDetails.AddedBy);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, serviceMasterDetails.AddedOn);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, serviceMasterDetails.AddedDateTime);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, serviceMasterDetails.AddedWindowsLoginName);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0);
                        this.dbServer.AddOutParameter(storedProcCommand, "Id", DbType.Int64, 0);
                        this.dbServer.ExecuteNonQuery(storedProcCommand);
                        nvo.TariffServiceID = (long) this.dbServer.GetParameterValue(storedProcCommand, "Id");
                        command2 = this.dbServer.GetStoredProcCommand("CIMS_AddTariffServiceClassRateDetail");
                        this.dbServer.AddInParameter(command2, "TariffServiceId", DbType.Int64, nvo.TariffServiceID);
                        this.dbServer.AddInParameter(command2, "ClassId", DbType.Int64, 1);
                        this.dbServer.AddInParameter(command2, "Rate", DbType.Int64, serviceMasterDetails.Rate);
                        this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, serviceMasterDetails.Status);
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, serviceMasterDetails.UnitID);
                        this.dbServer.ExecuteNonQuery(command2);
                    }
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public void UpdateTariffServiceClassRateDetails(List<long> ids, DbConnection con, DbTransaction trans)
        {
            DbCommand storedProcCommand = null;
            for (int i = 0; i < ids.Count; i++)
            {
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ChangeTariffServiceClassRateDetailsStatus");
                storedProcCommand.Connection = con;
                storedProcCommand.Parameters.Clear();
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, ids[i]);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, 0);
                this.dbServer.ExecuteNonQuery(storedProcCommand, trans);
            }
        }

        public void UpdateTariffServiceMaster(List<long> ids, DbConnection con, DbTransaction trans)
        {
            DbCommand storedProcCommand = null;
            for (int i = 0; i < ids.Count; i++)
            {
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ChangeTariffServiceStatus");
                storedProcCommand.Connection = con;
                storedProcCommand.Parameters.Clear();
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, ids[i]);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, 0);
                this.dbServer.ExecuteNonQuery(storedProcCommand, trans);
            }
        }
    }
}

