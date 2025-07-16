namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration;
    using PalashDynamics.ValueObjects.Administration.PackageNew;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsPackageServiceNewDAL : clsBasePackageServiceNewDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsPackageServiceNewDAL()
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

        public override IValueObject AddPackageConsentLink(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            clsAddPackageConsentLinkBizActionVO nvo = valueObject as clsAddPackageConsentLinkBizActionVO;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateStatusPackageConsentDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                foreach (clsServiceMasterVO rvo in nvo.ServiceItemMasterDetails)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePackageConsentDetails");
                    this.dbServer.AddInParameter(command2, "ServiceID", DbType.Int64, rvo.ServiceID);
                    this.dbServer.AddInParameter(command2, "TemplateID", DbType.Int64, rvo.TemplateID);
                    this.dbServer.AddInParameter(command2, "Description", DbType.String, rvo.Description);
                    this.dbServer.AddInParameter(command2, "DepartmentID", DbType.Int64, rvo.DepartmentID);
                    this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                }
                transaction.Commit();
            }
            catch (Exception)
            {
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

        private clsAddPackageServiceNewBizActionVO AddPackageNew(clsAddPackageServiceNewBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                clsPackageServiceVO details = BizActionObj.Details;
                if (BizActionObj.Details.ID == 0L)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPackageServiceForPackage");
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, details.ServiceID);
                    this.dbServer.AddInParameter(storedProcCommand, "Validity", DbType.String, details.Validity);
                    this.dbServer.AddInParameter(storedProcCommand, "ValidityUnit", DbType.String, details.ValidityUnit);
                    this.dbServer.AddInParameter(storedProcCommand, "PackageAmount", DbType.Double, details.PackageAmount);
                    this.dbServer.AddInParameter(storedProcCommand, "ApplicableToAll", DbType.Boolean, details.ApplicableToAll);
                    this.dbServer.AddInParameter(storedProcCommand, "ApplicableToAllDiscount", DbType.Double, details.ApplicableToAllDiscount);
                    this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, details.IsFreezed);
                    this.dbServer.AddInParameter(storedProcCommand, "IsFixed", DbType.Boolean, details.IsFixedRate);
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceFixedRate", DbType.Double, details.ServiceFixedRate);
                    this.dbServer.AddInParameter(storedProcCommand, "PharmacyFixedRate", DbType.Double, details.PharmacyFixedRate);
                    this.dbServer.AddInParameter(storedProcCommand, "ServicePercentage", DbType.Double, details.ServicePercentage);
                    this.dbServer.AddInParameter(storedProcCommand, "PharmacyPercentage", DbType.Double, details.PharmacyPercentage);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, details.ID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    BizActionObj.Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                    details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                }
                if (BizActionObj.Details.ID > 0L)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePackageServiceForPackage");
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, details.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, details.ServiceID);
                    this.dbServer.AddInParameter(storedProcCommand, "Validity", DbType.String, details.Validity);
                    this.dbServer.AddInParameter(storedProcCommand, "PackageAmount", DbType.Double, details.PackageAmount);
                    this.dbServer.AddInParameter(storedProcCommand, "NoOfFollowUp", DbType.String, details.NoOfFollowUp);
                    this.dbServer.AddInParameter(storedProcCommand, "ApplicableToAll", DbType.Boolean, details.ApplicableToAll);
                    this.dbServer.AddInParameter(storedProcCommand, "ApplicableToAllDiscount", DbType.Double, details.ApplicableToAllDiscount);
                    this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, details.IsFreezed);
                    this.dbServer.AddInParameter(storedProcCommand, "IsFixed", DbType.Boolean, details.IsFixedRate);
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceFixedRate", DbType.Double, details.ServiceFixedRate);
                    this.dbServer.AddInParameter(storedProcCommand, "PharmacyFixedRate", DbType.Double, details.PharmacyFixedRate);
                    this.dbServer.AddInParameter(storedProcCommand, "ServicePercentage", DbType.Double, details.ServicePercentage);
                    this.dbServer.AddInParameter(storedProcCommand, "PharmacyPercentage", DbType.Double, details.PharmacyPercentage);
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
                }
                long serviceID = 0L;
                int num2 = 0;
                if ((details.PackageDetails != null) && (details.PackageDetails.Count != 0))
                {
                    foreach (clsPackageServiceDetailsVO svo in details.PackageDetails)
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPackageServiceDetailsForPackage");
                        this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int64, details.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "PackageUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "SpecilizationID", DbType.Int64, svo.DepartmentID);
                        this.dbServer.AddInParameter(storedProcCommand, "IsSpecilizationGroup", DbType.Boolean, svo.IsSpecilizationGroup);
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, svo.ServiceID);
                        this.dbServer.AddInParameter(storedProcCommand, "Amount", DbType.Double, svo.Amount);
                        this.dbServer.AddInParameter(storedProcCommand, "Discount", DbType.Double, svo.Discount);
                        this.dbServer.AddInParameter(storedProcCommand, "IsDiscountOnQuantity", DbType.Boolean, svo.IsDiscountOnQuantity);
                        this.dbServer.AddInParameter(storedProcCommand, "AgeLimit", DbType.Int64, svo.AgeLimit);
                        this.dbServer.AddInParameter(storedProcCommand, "IsFollowupNotRequired", DbType.Boolean, svo.IsFollowupNotRequired);
                        this.dbServer.AddInParameter(storedProcCommand, "ApplicableTo", DbType.Int64, svo.SelectedGender.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "Quantity", DbType.Double, svo.Quantity);
                        this.dbServer.AddInParameter(storedProcCommand, "Infinite", DbType.Boolean, svo.Infinite);
                        this.dbServer.AddInParameter(storedProcCommand, "NetAmount", DbType.Double, svo.NetAmount);
                        this.dbServer.AddInParameter(storedProcCommand, "IsActive", DbType.Boolean, svo.IsActive);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                        this.dbServer.AddInParameter(storedProcCommand, "Month", DbType.String, svo.Month);
                        this.dbServer.AddInParameter(storedProcCommand, "MonthStatus", DbType.Boolean, svo.MonthStatus);
                        this.dbServer.AddInParameter(storedProcCommand, "AdjustableHead", DbType.Boolean, svo.AdjustableHead);
                        this.dbServer.AddInParameter(storedProcCommand, "IsFixed", DbType.Boolean, svo.IsFixed);
                        this.dbServer.AddInParameter(storedProcCommand, "Rate", DbType.Double, svo.Rate);
                        this.dbServer.AddInParameter(storedProcCommand, "Percentage", DbType.Double, svo.RatePercentage);
                        this.dbServer.AddInParameter(storedProcCommand, "IsDoctorSharePercentage", DbType.Boolean, svo.IsDoctorSharePercentage);
                        this.dbServer.AddInParameter(storedProcCommand, "ConsiderAdjustable", DbType.Boolean, svo.ConsiderAdjustable);
                        this.dbServer.AddInParameter(storedProcCommand, "ProcessID", DbType.Int64, svo.SelectedProcess.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "AdjustableHeadType", DbType.Int32, svo.AdjustableHeadType);
                        this.dbServer.AddInParameter(storedProcCommand, "IsConsumables", DbType.Boolean, svo.IsConsumables);
                        this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                        svo.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                        if (serviceID != svo.ServiceID)
                        {
                            serviceID = svo.ServiceID;
                            num2 = 0;
                        }
                        if ((serviceID == svo.ServiceID) && (num2 == 0))
                        {
                            if ((details.PackageServiceRelationDetails != null) && (details.PackageServiceRelationDetails.Count != 0))
                            {
                                foreach (clsPackageServiceRelationsVO svo2 in details.PackageServiceRelationDetails)
                                {
                                    DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddPackageServiceRelationForPackage");
                                    this.dbServer.AddInParameter(command, "RelationID", DbType.Int64, svo2.RelationID);
                                    this.dbServer.AddInParameter(command, "PackageID", DbType.Int64, BizActionObj.Details.ID);
                                    this.dbServer.AddInParameter(command, "PackageUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command, "ServiceID", DbType.Int64, svo2.ServiceID);
                                    this.dbServer.AddInParameter(command, "SpecilizationID", DbType.Int64, svo.DepartmentID);
                                    this.dbServer.AddInParameter(command, "IsSpecilizationGroup", DbType.Boolean, svo.IsSpecilizationGroup);
                                    this.dbServer.AddInParameter(command, "IsSetAllRelations", DbType.Boolean, svo2.IsSetAllRelations);
                                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command, "Status", DbType.Boolean, svo.Status);
                                    this.dbServer.AddInParameter(command, "ProcessID", DbType.Int64, svo2.ProcessID);
                                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo2.ID);
                                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, 0);
                                    this.dbServer.ExecuteNonQuery(command, transaction);
                                    svo2.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                                }
                            }
                            if ((details.ServiceConditionDetails != null) && (details.ServiceConditionDetails.Count != 0))
                            {
                                foreach (clsPackageServiceConditionsVO svo3 in details.ServiceConditionDetails)
                                {
                                    DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddPackageServiceConditionForPackage");
                                    this.dbServer.AddInParameter(command, "PackageID", DbType.Int64, BizActionObj.Details.ID);
                                    this.dbServer.AddInParameter(command, "PackageUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command, "PackageServiceId", DbType.Int64, svo.ServiceID);
                                    this.dbServer.AddInParameter(command, "ServiceID", DbType.Int64, svo3.SelectedService.ID);
                                    this.dbServer.AddInParameter(command, "Rate", DbType.Double, svo3.Rate);
                                    this.dbServer.AddInParameter(command, "Quantity", DbType.Double, svo3.Quantity);
                                    this.dbServer.AddInParameter(command, "Discount", DbType.Double, svo3.Discount);
                                    this.dbServer.AddInParameter(command, "ConditionType", DbType.String, svo3.SelectedCondition.Description);
                                    this.dbServer.AddInParameter(command, "ConditionTypeID", DbType.String, svo3.SelectedCondition.ID);
                                    this.dbServer.AddInParameter(command, "SpecilizationID", DbType.Int64, svo.DepartmentID);
                                    this.dbServer.AddInParameter(command, "IsSpecilizationGroup", DbType.Boolean, svo.IsSpecilizationGroup);
                                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command, "Status", DbType.Boolean, svo3.Status);
                                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo3.ID);
                                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, 0);
                                    this.dbServer.ExecuteNonQuery(command, transaction);
                                    svo3.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                                }
                            }
                            num2++;
                        }
                    }
                }
                 serviceID = 0L;
                int num4 = 0;
                if (BizActionObj.IsSavePatientData && ((details.PackageDetails != null) && (details.PackageDetails.Count != 0)))
                {
                    foreach (clsPackageServiceDetailsVO svo4 in details.PackageDetails)
                    {
                        if (serviceID != svo4.ServiceID)
                        {
                            serviceID = svo4.ServiceID;
                            num4 = 0;
                        }
                        if ((serviceID == svo4.ServiceID) && (num4 == 0))
                        {
                            if (BizActionObj.IsSavePatientData)
                            {
                                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatientPackageDetailsForPackageAllAddAfterFreeze2");
                                this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int64, details.ID);
                                this.dbServer.AddInParameter(storedProcCommand, "PackageUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, svo4.ServiceID);
                                this.dbServer.AddInParameter(storedProcCommand, "SpecilizationID", DbType.Int64, svo4.DepartmentID);
                                this.dbServer.AddInParameter(storedProcCommand, "IsSpecilizationGroup", DbType.Boolean, svo4.IsSpecilizationGroup);
                                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                            }
                            if (BizActionObj.IsSavePatientData && !svo4.IsSpecilizationGroup)
                            {
                                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatientFollowUpHealthSpringForPackageAllAddAfterFreeze");
                                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int64, details.ID);
                                this.dbServer.AddInParameter(storedProcCommand, "PackageUnitID", DbType.Int64, details.UnitID);
                                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, svo4.ServiceID);
                                this.dbServer.AddInParameter(storedProcCommand, "SpecilizationID", DbType.Int64, svo4.DepartmentID);
                                this.dbServer.AddInParameter(storedProcCommand, "IsSpecilizationGroup", DbType.Int64, svo4.IsSpecilizationGroup);
                                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                            }
                            num4++;
                        }
                    }
                }
                transaction.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.Details = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return BizActionObj;
        }

        public override IValueObject AddPackagePharmacyItemsNew(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            clsAddPackagePharmacyItemsNewBizActionVO nvo = valueObject as clsAddPackagePharmacyItemsNewBizActionVO;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                List<clsPackageItemMasterVO> itemDetails = nvo.ItemDetails;
                if ((itemDetails != null) && (itemDetails.Count != 0))
                {
                    foreach (clsPackageItemMasterVO rvo in itemDetails)
                    {
                        DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddPackageItemDetailsForPackage");
                        this.dbServer.AddInParameter(command, "PackageID", DbType.Int64, nvo.PackageID);
                        this.dbServer.AddInParameter(command, "PackageUnitID", DbType.Int64, nvo.PackageUnitID);
                        this.dbServer.AddInParameter(command, "ItemID", DbType.Int64, rvo.ItemID);
                        this.dbServer.AddInParameter(command, "Discount", DbType.Double, rvo.Discount);
                        this.dbServer.AddInParameter(command, "Quantity", DbType.Double, rvo.Quantity);
                        this.dbServer.AddInParameter(command, "ItemCategory", DbType.Int64, rvo.ItemCategory);
                        this.dbServer.AddInParameter(command, "IsCategory", DbType.Int64, rvo.IsCategory);
                        this.dbServer.AddInParameter(command, "ItemGroup", DbType.Int64, rvo.ItemGroup);
                        this.dbServer.AddInParameter(command, "IsGroup", DbType.Int64, rvo.IsGroup);
                        this.dbServer.AddInParameter(command, "Budget", DbType.Double, rvo.Budget);
                        this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "Status", DbType.Boolean, rvo.Status);
                        this.dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, rvo.ID);
                        this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, 0);
                        this.dbServer.ExecuteNonQuery(command, transaction);
                        rvo.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                    }
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePackagePharmacyItemsForPackage");
                    this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int64, nvo.PackageID);
                    this.dbServer.AddInParameter(storedProcCommand, "PackageUnitID", DbType.Int64, nvo.PackageUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "TotalBudget", DbType.Double, nvo.TotalBudget);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                }
                transaction.Commit();
                nvo.SuccessStatus = 0;
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                transaction.Rollback();
                nvo.ItemDetails = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject AddPackageServiceMaster(IValueObject valueObject, clsUserVO userVO)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            clsAddServiceMasterNewBizActionVO nvo = valueObject as clsAddServiceMasterNewBizActionVO;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = null;
                clsServiceMasterVO serviceMasterDetails = nvo.ServiceMasterDetails;
                if (!serviceMasterDetails.IsNewMaster)
                {
                    if (serviceMasterDetails.EditMode)
                    {
                        storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateServiceMasterForPackageNew");
                        storedProcCommand.Connection = connection;
                        this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, serviceMasterDetails.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, userVO.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, userVO.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);
                    }
                    else
                    {
                        storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddServiceMasterForPackageNew");
                        storedProcCommand.Connection = connection;
                        this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0);
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceCode", DbType.String, serviceMasterDetails.ServiceCode);
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, serviceMasterDetails.ServiceID);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, userVO.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, userVO.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
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
                    this.dbServer.AddInParameter(storedProcCommand, "IsFamily", DbType.Boolean, serviceMasterDetails.IsFamily);
                    this.dbServer.AddInParameter(storedProcCommand, "FamilyMemberCount", DbType.Int32, serviceMasterDetails.FamilyMemberCount);
                    this.dbServer.AddInParameter(storedProcCommand, "IsPackage", DbType.Boolean, serviceMasterDetails.IsPackage);
                    this.dbServer.AddInParameter(storedProcCommand, "IsFavorite", DbType.Boolean, serviceMasterDetails.IsFavorite);
                    this.dbServer.AddInParameter(storedProcCommand, "ExpiryDate", DbType.DateTime, serviceMasterDetails.ExpiryDate);
                    this.dbServer.AddInParameter(storedProcCommand, "EffectiveDate", DbType.DateTime, serviceMasterDetails.EffectiveDate);
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
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0);
                    this.dbServer.AddInParameter(storedProcCommand, "IsMarkUp", DbType.Boolean, serviceMasterDetails.IsMarkUp);
                    this.dbServer.AddInParameter(storedProcCommand, "PercentageOnMrp", DbType.Decimal, serviceMasterDetails.PercentageOnMrp);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    serviceMasterDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(this.dbServer.GetParameterValue(storedProcCommand, "ID")));
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    if ((nvo.FamilyMemberMasterDetails != null) && (nvo.FamilyMemberMasterDetails.Count > 0))
                    {
                        if (serviceMasterDetails.EditMode)
                        {
                            storedProcCommand = null;
                             storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeletePackageMember");
                            
                            storedProcCommand.Connection = connection;
                            this.dbServer.AddInParameter(storedProcCommand, "PackageServiceID", DbType.Int64, serviceMasterDetails.ID);
                            this.dbServer.AddInParameter(storedProcCommand, "PackageServiceUnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                            this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                        }
                        foreach (clsPackageRelationsVO svo in nvo.FamilyMemberMasterDetails)
                        {
                            storedProcCommand = null;
                            
                             storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPackageMemberRelationMasterDetails");

                            storedProcCommand.Connection = connection;
                            if (svo.IsSetAll)
                            {
                                this.dbServer.AddInParameter(storedProcCommand, "PackageServiceId", DbType.Int64, serviceMasterDetails.ID);
                                this.dbServer.AddInParameter(storedProcCommand, "PackageServiceUnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(storedProcCommand, "RelationID", DbType.Int64, svo.RelationID);
                                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, svo.Status);
                                this.dbServer.AddInParameter(storedProcCommand, "IsSetAll", DbType.Boolean, svo.IsSetAll);
                                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0);
                                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                                svo.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                                break;
                            }
                            this.dbServer.AddInParameter(storedProcCommand, "PackageServiceId", DbType.Int64, serviceMasterDetails.ID);
                            this.dbServer.AddInParameter(storedProcCommand, "PackageServiceUnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(storedProcCommand, "RelationID", DbType.Int64, svo.RelationID);
                            this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, svo.Status);
                            this.dbServer.AddInParameter(storedProcCommand, "IsSetAll", DbType.Boolean, false);
                            this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                            this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0);
                            this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                            svo.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
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
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection = null;
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject AddPackageServicesNew(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPackageServiceNewBizActionVO bizActionObj = valueObject as clsAddPackageServiceNewBizActionVO;
            if (bizActionObj.Details.PackageDetails[0].ID == 0L)
            {
                bizActionObj = this.AddPackageNew(bizActionObj, UserVo);
            }
            else if (bizActionObj.Details.PackageDetails[0].ID > 0L)
            {
                bizActionObj = this.UpdatePackage(bizActionObj, UserVo);
            }
            return valueObject;
        }

        public override IValueObject AddPackageSourceTariffCompanyLinking(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            clsAddPackageSourceTariffCompanyRelationsBizActionVO nvo = valueObject as clsAddPackageSourceTariffCompanyRelationsBizActionVO;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsPackageSourceRelationVO packageSourceRelation = nvo.PackageSourceRelation;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePackageRelation");
                this.dbServer.AddInParameter(storedProcCommand, "PatientCatagoryID", DbType.Int64, packageSourceRelation.PatientCategoryL1ID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientCategoryL2ID", DbType.Int64, packageSourceRelation.PatientCategoryL2ID);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyID", DbType.Int64, packageSourceRelation.CompanyID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientCategoryL3ID", DbType.Int64, packageSourceRelation.PatientCategoryL3ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, packageSourceRelation.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, packageSourceRelation.UnitID);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, packageSourceRelation.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                packageSourceRelation.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.ResultSuccessStatus = (long) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                transaction.Commit();
            }
            catch (Exception)
            {
                nvo.ResultSuccessStatus = -1L;
                transaction.Rollback();
                nvo.PackageSourceRelation = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject AddUpdatePackageRateClinicWise(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            clsAddPackageRateClinicWiseBizActionVO nvo = valueObject as clsAddPackageRateClinicWiseBizActionVO;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                foreach (clsPackageRateClinicWiseVO evo in nvo.PackageRateClinicWiseList)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePackageRateClinicWise");
                    this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, evo.PatientCategoryL3);
                    this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int64, nvo.PackageID);
                    this.dbServer.AddInParameter(storedProcCommand, "PackageServiceID", DbType.Int64, nvo.PackageServiceID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, evo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, evo.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "Rate", DbType.Decimal, evo.Rate);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.ID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    evo.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                    nvo.ResultSuccessStatus = (long) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                nvo.ResultSuccessStatus = -1L;
                transaction.Rollback();
                nvo.PackageRateClinicWise = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject DeletePackageItemsDetilsNew(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            clsDeletePackageItemDetilsListNewBizActionVO nvo = valueObject as clsDeletePackageItemDetilsListNewBizActionVO;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeletePackageItemDetailsForPackage");
                this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int64, nvo.PackageID);
                this.dbServer.AddInParameter(storedProcCommand, "PackageUnitID", DbType.Int64, nvo.PackageUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, nvo.ItemID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemGroupID", DbType.Int64, nvo.ItemGroupID);
                this.dbServer.AddInParameter(storedProcCommand, "IsGroup", DbType.Boolean, nvo.IsGroup);
                this.dbServer.AddInParameter(storedProcCommand, "ItemCategoryID", DbType.Int64, nvo.ItemCategoryID);
                this.dbServer.AddInParameter(storedProcCommand, "IsCategory", DbType.Boolean, nvo.IsCategory);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.Status);
                this.dbServer.AddInParameter(storedProcCommand, "IsDeletePatientData", DbType.Boolean, nvo.IsDeletePatientData);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                transaction.Commit();
                nvo.SuccessStatus = 0;
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                transaction.Rollback();
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject DeletePackageServicesDetilsNew(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            clsDeletePackageServiceDetilsListNewBizActionVO nvo = valueObject as clsDeletePackageServiceDetilsListNewBizActionVO;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeletePackageServiceDetailsForPackage");
                this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int64, nvo.PackageID);
                this.dbServer.AddInParameter(storedProcCommand, "PackageUnitID", DbType.Int64, nvo.PackageUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "SpecilizationID", DbType.Int64, nvo.SpecilizationID);
                this.dbServer.AddInParameter(storedProcCommand, "IsSpecilizationGroup", DbType.Boolean, nvo.IsSpecilizationGroup);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.Status);
                this.dbServer.AddInParameter(storedProcCommand, "IsDeletePatientData", DbType.Boolean, nvo.IsDeletePatientData);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                transaction.Commit();
                nvo.SuccessStatus = 0;
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                transaction.Rollback();
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject GetPackageConditionalServiceListNew(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPackageConditionalServicesNewBizActionVO nvo = valueObject as clsGetPackageConditionalServicesNewBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPackageConditionalServiceListForPackage4");
                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, nvo.TariffID);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int64, nvo.PackageID);
                this.dbServer.AddInParameter(storedProcCommand, "SponsorID", DbType.Int64, nvo.SponsorID);
                this.dbServer.AddInParameter(storedProcCommand, "SponsorUnitID", DbType.Int64, nvo.SponsorUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientDateOfBirth", DbType.DateTime, nvo.PatientDateOfBirth);
                this.dbServer.AddInParameter(storedProcCommand, "MemberRelationID", DbType.Int64, nvo.MemberRelationID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ServiceConditionList == null)
                    {
                        nvo.ServiceConditionList = new List<clsServiceMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsServiceMasterVO item = new clsServiceMasterVO {
                            ID = Convert.ToInt64(reader["ServiceID"]),
                            TariffServiceMasterID = Convert.ToInt64(reader["ID"]),
                            ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"])),
                            TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"])),
                            Specialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationID"])),
                            SubSpecialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"])),
                            Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"])),
                            SeniorCitizen = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["SeniorCitizen"])),
                            SeniorCitizenConAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SeniorCitizenConAmount"])),
                            SeniorCitizenConPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SeniorCitizenConPercent"])),
                            SeniorCitizenAge = Convert.ToInt32(DALHelper.HandleDBNull(reader["SeniorCitizenAge"])),
                            ConcessionAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionAmount"])),
                            ConcessionPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionPercent"])),
                            StaffDiscount = Convert.ToBoolean(DALHelper.HandleDBNull(reader["StaffDiscount"])),
                            StaffDiscountAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDiscountAmount"])),
                            StaffDiscountPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDiscountPercent"])),
                            StaffDependantDiscount = Convert.ToBoolean(DALHelper.HandleDBNull(reader["StaffDependantDiscount"])),
                            StaffDependantDiscountAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDependantDiscountAmount"])),
                            StaffDependantDiscountPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDependantDiscountPercent"])),
                            Concession = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Concession"])),
                            ServiceTax = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ServiceTax"])),
                            ServiceTaxAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceTaxAmount"])),
                            ServiceTaxPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceTaxPercent"])),
                            InHouse = Convert.ToBoolean(DALHelper.HandleDBNull(reader["InHouse"])),
                            DoctorShare = Convert.ToBoolean(DALHelper.HandleDBNull(reader["DoctorShare"])),
                            DoctorSharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorSharePercentage"])),
                            DoctorShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorShareAmount"])),
                            RateEditable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["RateEditable"])),
                            MaxRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MaxRate"])),
                            MinRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MinRate"])),
                            TarrifCode = Convert.ToString(DALHelper.HandleDBNull(reader["TarrifServiceCode"])),
                            TarrifName = Convert.ToString(DALHelper.HandleDBNull(reader["TarrifServiceName"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            CodeType = (long) DALHelper.HandleDBNull(reader["CodeType"]),
                            ShortDescription = Convert.ToString(DALHelper.HandleDBNull(reader["ShortDescription"])),
                            LongDescription = Convert.ToString(DALHelper.HandleDBNull(reader["LongDescription"])),
                            SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["Specialization"])),
                            SubSpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecialization"])),
                            IsPackage = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackage"])),
                            PackageID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PackageID"])),
                            IsMarkUp = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsMarkUp"])),
                            ConditionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConditionID"])),
                            ConditionUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConditionUnitID"])),
                            MainServicePackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainServicePackageID"])),
                            MainServicePackageUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainServicePackageUnitID"])),
                            PackageServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageServiceID"])),
                            ConditionServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConditionServiceID"])),
                            ConditionServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ConditionServiceName"])),
                            MainServiceSpecilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainServiceSpecilizationID"])),
                            MainSerivceIsSpecilizationGroup = Convert.ToBoolean(DALHelper.HandleDBNull(reader["MainSerivceIsSpecilizationGroup"])),
                            ConditionalRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConditionalRate"])),
                            ConditionalQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConditionalQuantity"])),
                            ConditionalDiscount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConditionalDiscount"])),
                            ConditionTypeID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ConditionTypeID"])),
                            ConditionType = Convert.ToString(DALHelper.HandleDBNull(reader["ConditionType"])),
                            ConditionalUsedQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConditionalUsedQuantity"])),
                            MainServiceUsedQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["MainServiceUsedQuantity"])),
                            ServiceMemberRelationID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ServiceMemberRelationID"])),
                            IsAgeApplicable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAgeApplicable"])),
                            ApplicableTo = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ApplicableTo"])),
                            ApplicableToString = Convert.ToString(DALHelper.HandleDBNull(reader["ApplicableToString"]))
                        };
                        nvo.ServiceConditionList.Add(item);
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

        public override IValueObject GetPackageConsentLink(IValueObject valueObject, clsUserVO objUserVO)
        {
            IValueObject obj2;
            try
            {
                clsGetPackageConsentLinkBizActionVO nvo = valueObject as clsGetPackageConsentLinkBizActionVO;
                nvo.ServiceItemMasterDetails = new List<clsServiceMasterVO>();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPackageConsentDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceID);
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
                        clsServiceMasterVO item = new clsServiceMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"])),
                            TemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"]))
                        };
                        nvo.ServiceItemMasterDetails.Add(item);
                    }
                }
                obj2 = nvo;
            }
            catch (Exception)
            {
                throw;
            }
            return obj2;
        }

        public override IValueObject GetPackagePharmacyItemListNew(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPackagePharmacyItemListNewBizActionVO nvo = valueObject as clsGetPackagePharmacyItemListNewBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPackagePharmacyItemDetailsForPackage");
                this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int64, nvo.PackageID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemDetails == null)
                    {
                        nvo.ItemDetails = new List<clsPackageItemMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsPackageItemMasterVO item = new clsPackageItemMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            PackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageID"])),
                            PackageUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageUnitID"])),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                            Discount = (float) ((double) DALHelper.HandleDBNull(reader["Discount"])),
                            Budget = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["Budget"])),
                            Quantity = (float) ((double) DALHelper.HandleDBNull(reader["Quantity"])),
                            ItemName = (string) DALHelper.HandleDBNull(reader["ItemName"]),
                            ItemCategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCategoryName"])),
                            ItemGroupName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemGroupName"])),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            ItemCategory = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryId"])),
                            IsCategory = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCategory"])),
                            ItemGroup = Convert.ToInt64(DALHelper.HandleDBNull(reader["GroupId"])),
                            IsGroup = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsGroup"]))
                        };
                        if (item.IsCategory)
                        {
                            item.ItemID = item.ItemCategory;
                            item.ItemName = item.ItemCategoryName;
                        }
                        if (item.IsGroup)
                        {
                            item.ItemID = item.ItemGroup;
                            item.ItemName = item.ItemGroupName;
                        }
                        nvo.ItemDetails.Add(item);
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

        public override IValueObject GetPackageRateClinicWise(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPackageRateClinicWiseBizActionVO nvo = valueObject as clsGetPackageRateClinicWiseBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPackageRateClinicWise");
                this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int64, nvo.PackageID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PackageRateClinicWiseList == null)
                    {
                        nvo.PackageRateClinicWiseList = new List<clsPackageRateClinicWiseVO>();
                    }
                    while (reader.Read())
                    {
                        clsPackageRateClinicWiseVO item = new clsPackageRateClinicWiseVO {
                            ID = Convert.ToInt64(reader["ID"]),
                            UnitID = Convert.ToInt64(reader["UnitID"]),
                            UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"])),
                            TariffName = Convert.ToString(DALHelper.HandleDBNull(reader["TariffName"])),
                            Rate = (decimal) DALHelper.HandleDBNull(reader["Rate"]),
                            PatientCategoryL3 = Convert.ToString(DALHelper.HandleDBNull(reader["TariffId"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.PackageRateClinicWiseList.Add(item);
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

        public override IValueObject GetPackageRelationsList(IValueObject valueObject, clsUserVO objUserVO)
        {
            IValueObject obj2;
            try
            {
                clsGetPackageRelationsBizActionVO nvo = valueObject as clsGetPackageRelationsBizActionVO;
                nvo.PackageRelationsList = nvo.PackageRelationsList;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPackageMemberRelation");
                this.dbServer.AddInParameter(storedProcCommand, "PackageServiceID", DbType.Int64, nvo.PackageServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "PackageServiceUnitID", DbType.Int64, nvo.PackageServiceUnitID);
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
                        MasterListItem item = new MasterListItem {
                            ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]))
                        };
                        nvo.PackageRelationsList.Add(item);
                    }
                }
                obj2 = nvo;
            }
            catch (Exception)
            {
                throw;
            }
            return obj2;
        }

        public override IValueObject GetPackageRelationsListForPackageOnly(IValueObject valueObject, clsUserVO objUserVO)
        {
            IValueObject obj2;
            try
            {
                clsGetPackageRelationListBizActionVO nvo = valueObject as clsGetPackageRelationListBizActionVO;
                nvo.PackageRelationsList = nvo.PackageRelationsList;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPackageMemberRelationList");
                this.dbServer.AddInParameter(storedProcCommand, "PackageTariffID", DbType.Int64, nvo.PackageTariffID);
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
                        MasterListItem item = new MasterListItem {
                            ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["RelationID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Relation"]))
                        };
                        nvo.PackageRelationsList.Add(item);
                    }
                }
                obj2 = nvo;
            }
            catch (Exception)
            {
                throw;
            }
            return obj2;
        }

        public override IValueObject GetPackageServiceDetailListNew(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPackageServiceDetailsListNewBizActionVO nvo = valueObject as clsGetPackageServiceDetailsListNewBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPackageServiceDetailsForPackage");
                this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int64, nvo.PackageID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PackageMasterList == null)
                    {
                        nvo.PackageMasterList = new clsPackageServiceVO();
                    }
                    while (reader.Read())
                    {
                        clsPackageServiceVO evo = new clsPackageServiceVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            ServiceID = (long) DALHelper.HandleDBNull(reader["ServiceID"]),
                            Service = (string) DALHelper.HandleDBNull(reader["ServiceName"]),
                            Validity = (string) DALHelper.HandleDBNull(reader["Validity"]),
                            ValidityUnit = DALHelper.HandleIntegerNull(reader["ValidityUnit"]),
                            PackageAmount = (double) DALHelper.HandleDBNull(reader["PackageAmount"]),
                            NoOfFollowUp = (string) DALHelper.HandleDBNull(reader["NoOfFollowUp"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            ApplicableToAll = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ApplicableToAll"])),
                            ApplicableToAllDiscount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ApplicableToAllDiscount"])),
                            TotalBudget = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalBudget"])),
                            IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"])),
                            IsApproved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsApproved"])),
                            IsFixedRate = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFixed"])),
                            ServiceFixedRate = (double) DALHelper.HandleDBNull(reader["ServiceFixedRate"]),
                            PharmacyFixedRate = (double) DALHelper.HandleDBNull(reader["PharmacyFixedRate"]),
                            ServicePercentage = (double) DALHelper.HandleDBNull(reader["ServicePercentage"]),
                            PharmacyPercentage = (double) DALHelper.HandleDBNull(reader["PharmacyPercentage"])
                        };
                        nvo.PackageMasterList = evo;
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    if (nvo.PackageDetailList == null)
                    {
                        nvo.PackageDetailList = new List<clsPackageServiceDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsPackageServiceDetailsVO item = new clsPackageServiceDetailsVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            PackageID = (long) DALHelper.HandleDBNull(reader["PackageID"]),
                            DepartmentID = (long) DALHelper.HandleDBNull(reader["DepartmentID"]),
                            Department = (string) DALHelper.HandleDBNull(reader["Department"]),
                            IsSpecilizationGroup = (bool) DALHelper.HandleBoolDBNull(reader["IsSpecilizationGroup"]),
                            ServiceID = (long) DALHelper.HandleDBNull(reader["ServiceID"]),
                            ServiceName = (string) DALHelper.HandleDBNull(reader["ServiceName"])
                        };
                        if (item.IsSpecilizationGroup)
                        {
                            item.ServiceID = item.DepartmentID;
                            item.ServiceName = item.Department;
                        }
                        item.ServiceCode = (string) DALHelper.HandleDBNull(reader["ServiceCode"]);
                        item.Rate = (double) DALHelper.HandleDBNull(reader["Rate"]);
                        item.Amount = Convert.ToDouble(DALHelper.HandleDBNull(reader["Amount"]));
                        item.Discount = Convert.ToDouble(DALHelper.HandleDBNull(reader["Discount"]));
                        item.IsDiscountOnQuantity = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsDiscountOnQuantity"]));
                        item.ApplicableTo = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApplicableTo"]));
                        item.AgeLimit = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgeLimit"]));
                        item.IsFollowupNotRequired = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsFollowupNotRequired"]));
                        item.Quantity = (double) DALHelper.HandleDBNull(reader["Quantity"]);
                        item.NetAmount = (double) DALHelper.HandleDBNull(reader["NetAmount"]);
                        item.IsActive = (bool) DALHelper.HandleDBNull(reader["IsActive"]);
                        item.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                        item.Infinite = (bool) DALHelper.HandleBoolDBNull(reader["Infinite"]);
                        item.Month = (string) DALHelper.HandleBoolDBNull(reader["Month"]);
                        item.MonthStatus = (bool) DALHelper.HandleBoolDBNull(reader["MonthStatus"]);
                        item.Validity = Convert.ToString(DALHelper.HandleBoolDBNull(reader["Validity"]));
                        item.ValidityUnit = Convert.ToInt64(DALHelper.HandleBoolDBNull(reader["ValidityUnit"]));
                        item.DisplayQuantity = Convert.ToString(DALHelper.HandleDBNull(reader["DisplayQuantity"]));
                        item.AdjustableHead = (bool) DALHelper.HandleDBNull(reader["AdjustableHead"]);
                        item.IsFixed = (bool) DALHelper.HandleDBNull(reader["IsFixed"]);
                        item.RatePercentage = (double) DALHelper.HandleDBNull(reader["Percentage"]);
                        item.IsDoctorSharePercentage = (bool) DALHelper.HandleDBNull(reader["IsDoctorSharePercentage"]);
                        item.ConsiderAdjustable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ConsiderAdjustable"]));
                        item.ProcessID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcessID"]));
                        item.AdjustableHeadType = Convert.ToInt32(DALHelper.HandleDBNull(reader["AdjustableHeadType"]));
                        item.IsConsumables = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsConsumables"]));
                        nvo.PackageDetailList.Add(item);
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

        public override IValueObject GetPackageServiceList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceMasterListNewBizActionVO nvo = (clsGetServiceMasterListNewBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAllServiceDetailsForPackage");
                if ((nvo.ServiceName != null) && (nvo.ServiceName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.ServiceName);
                }
                this.dbServer.AddInParameter(storedProcCommand, "Specialization", DbType.Int64, nvo.Specialization);
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
                            PackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageID"])),
                            TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"])),
                            PackageUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageUnitID"])),
                            IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"])),
                            IsApproved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsApproved"]))
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

        public override IValueObject GetPackageServiceMasterDetailsForId(IValueObject valueObject, clsUserVO objUserVO)
        {
            IValueObject obj2;
            try
            {
                clsGetServiceMasterListNewBizActionVO nvo = valueObject as clsGetServiceMasterListNewBizActionVO;
                nvo.ServiceMaster = nvo.ServiceMaster;
                if (!nvo.IsNewServiceMaster)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAllServiceMasterDetailsForPackage");
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceMaster.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            nvo.ServiceMaster.ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                            nvo.ServiceMaster.ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"]));
                            nvo.ServiceMaster.CodeType = Convert.ToInt64(DALHelper.HandleDBNull(reader["CodeType"]));
                            nvo.ServiceMaster.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                            nvo.ServiceMaster.Specialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"]));
                            nvo.ServiceMaster.SubSpecialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"]));
                            nvo.ServiceMaster.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                            nvo.ServiceMaster.ShortDescription = Convert.ToString(DALHelper.HandleDBNull(reader["ShortDescription"]));
                            nvo.ServiceMaster.LongDescription = Convert.ToString(DALHelper.HandleDBNull(reader["LongDescription"]));
                            nvo.ServiceMaster.StaffDiscount = Convert.ToBoolean(DALHelper.HandleDBNull(reader["StaffDiscount"]));
                            nvo.ServiceMaster.StaffDiscountAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDiscountAmount"]));
                            nvo.ServiceMaster.StaffDiscountPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDiscountPercent"]));
                            nvo.ServiceMaster.StaffDependantDiscount = Convert.ToBoolean(DALHelper.HandleDBNull(reader["StaffDependantDiscount"]));
                            nvo.ServiceMaster.StaffDependantDiscountAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDependantDiscountAmount"]));
                            nvo.ServiceMaster.StaffDependantDiscountPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDependantDiscountPercent"]));
                            nvo.ServiceMaster.Concession = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Concession"]));
                            nvo.ServiceMaster.ConcessionAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                            nvo.ServiceMaster.ConcessionPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                            nvo.ServiceMaster.ServiceTax = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ServiceTax"]));
                            nvo.ServiceMaster.ServiceTaxAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceTaxAmount"]));
                            nvo.ServiceMaster.ServiceTaxPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceTaxPercent"]));
                            nvo.ServiceMaster.InHouse = Convert.ToBoolean(DALHelper.HandleDBNull(reader["InHouse"]));
                            nvo.ServiceMaster.DoctorShare = Convert.ToBoolean(DALHelper.HandleDBNull(reader["DoctorShare"]));
                            nvo.ServiceMaster.DoctorSharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorSharePercentage"]));
                            nvo.ServiceMaster.DoctorShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorShareAmount"]));
                            nvo.ServiceMaster.RateEditable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["RateEditable"]));
                            nvo.ServiceMaster.MaxRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MaxRate"]));
                            nvo.ServiceMaster.MinRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MinRate"]));
                            nvo.ServiceMaster.CheckedAllTariffs = Convert.ToBoolean(DALHelper.HandleDBNull(reader["CheckedAllTariffs"]));
                            nvo.ServiceMaster.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                            nvo.ServiceMaster.ServiceName = nvo.ServiceMaster.Description;
                            nvo.ServiceMaster.IsFamily = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsFamily"]));
                            nvo.ServiceMaster.FamilyMemberCount = Convert.ToInt32(DALHelper.HandleDBNull(reader["FamilyMemberCount"]));
                            nvo.ServiceMaster.IsPackage = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackage"]));
                            nvo.ServiceMaster.EffectiveDate = DALHelper.HandleDate(reader["PackageEffectiveDate"]);
                            nvo.ServiceMaster.ExpiryDate = DALHelper.HandleDate(reader["PackageExpiryDate"]);
                            nvo.ServiceMaster.SeniorCitizen = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["SeniorCitizen"]));
                            nvo.ServiceMaster.SeniorCitizenConAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SeniorCitizenConAmount"]));
                            nvo.ServiceMaster.SeniorCitizenConPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SeniorCitizenConPercent"]));
                            nvo.ServiceMaster.SeniorCitizenAge = Convert.ToInt32(DALHelper.HandleDBNull(reader["SeniorCitizenAge"]));
                            nvo.ServiceMaster.IsMarkUp = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsMarkUp"]));
                            nvo.ServiceMaster.PercentageOnMrp = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PercentageOnMrp"]));
                            nvo.ServiceMaster.IsFavorite = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFavorite"]));
                            nvo.ServiceMaster.IsLinkWithInventory = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IslinkWithInventory"]));
                            nvo.ServiceMaster.CodeDetails = Convert.ToString(DALHelper.HandleDBNull(reader["CodeDetails"]));
                            nvo.ServiceMaster.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                        }
                    }
                    reader.NextResult();
                    if (nvo.FamilyMemberMasterDetails == null)
                    {
                        nvo.FamilyMemberMasterDetails = new List<clsPackageRelationsVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        clsPackageRelationsVO item = new clsPackageRelationsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            PackageServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageServiceID"])),
                            PackageServiceUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageServiceUnitID"])),
                            RelationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RelationID"])),
                            Relation = Convert.ToString(DALHelper.HandleDBNull(reader["Relation"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            IsSetAll = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSetAll"]))
                        };
                        nvo.FamilyMemberMasterDetails.Add(item);
                    }
                }
                obj2 = nvo;
            }
            catch (Exception)
            {
                throw;
            }
            return obj2;
        }

        public override IValueObject GetPackageServiceRelationsListNew(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPackageServicesAndRelationsNewBizActionVO nvo = valueObject as clsGetPackageServicesAndRelationsNewBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPackageServiceRelationsListForPackage");
                this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int64, nvo.PackageID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ServiceConditionList == null)
                    {
                        nvo.ServiceConditionList = new List<clsPackageServiceConditionsVO>();
                    }
                    while (reader.Read())
                    {
                        clsPackageServiceConditionsVO item = new clsPackageServiceConditionsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            PackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageID"])),
                            PackageUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageUnitID"])),
                            PackageServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageServiceID"])),
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"])),
                            ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"])),
                            SpecilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecilizationID"])),
                            IsSpecilizationGroup = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSpecilizationGroup"])),
                            Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"])),
                            Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"])),
                            Discount = Convert.ToDouble(DALHelper.HandleDBNull(reader["Discount"])),
                            ConditionTypeID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ConditionTypeID"])),
                            ConditionType = Convert.ToString(DALHelper.HandleDBNull(reader["ConditionType"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.ServiceConditionList.Add(item);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    if (nvo.PackageServiceRelationList == null)
                    {
                        nvo.PackageServiceRelationList = new List<clsPackageServiceRelationsVO>();
                    }
                    while (reader.Read())
                    {
                        clsPackageServiceRelationsVO item = new clsPackageServiceRelationsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            PackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageID"])),
                            PackageUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageUnitID"])),
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"])),
                            SpecilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecilizationID"])),
                            IsSpecilizationGroup = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSpecilizationGroup"])),
                            IsSetAllRelations = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSetAllRelations"])),
                            RelationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RelationID"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.PackageServiceRelationList.Add(item);
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

        public override IValueObject GetPackageSourceTariffCompanyLinking(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPackageSourceTariffCompanyListBizActionVO nvo = valueObject as clsGetPackageSourceTariffCompanyListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPackageRelationDetails");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, nvo.tariffID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PackageLinkingDetails == null)
                    {
                        nvo.PackageLinkingDetails = new List<clsPackageSourceRelationVO>();
                    }
                    while (reader.Read())
                    {
                        clsPackageSourceRelationVO item = new clsPackageSourceRelationVO {
                            ID = Convert.ToInt64(reader["ID"]),
                            PatientCategoryL1ID = Convert.ToInt64(reader["PatientCategoryL1"]),
                            PatientCategoryL1 = Convert.ToString(DALHelper.HandleDBNull(reader["PatientCategory"])),
                            PatientCategoryL2ID = Convert.ToInt64(reader["PatientCategoryL2"]),
                            PatientCategoryL2 = Convert.ToString(DALHelper.HandleDBNull(reader["PatientSource"])),
                            PatientCategoryL3ID = Convert.ToInt64(reader["PatientCategoryL3"]),
                            PatientCategoryL3 = Convert.ToString(DALHelper.HandleDBNull(reader["Tariff"])),
                            CompanyID = Convert.ToInt64(reader["CompanyId"]),
                            Company = Convert.ToString(DALHelper.HandleDBNull(reader["Company"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.PackageLinkingDetails.Add(item);
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

        private clsAddPackageServiceNewBizActionVO UpdatePackage(clsAddPackageServiceNewBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                clsPackageServiceVO details = BizActionObj.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePackageServiceForPackage");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, details.ID);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, details.ServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "Validity", DbType.String, details.Validity);
                this.dbServer.AddInParameter(storedProcCommand, "PackageAmount", DbType.Double, details.PackageAmount);
                this.dbServer.AddInParameter(storedProcCommand, "NoOfFollowUp", DbType.String, details.NoOfFollowUp);
                this.dbServer.AddInParameter(storedProcCommand, "ApplicableToAll", DbType.Boolean, details.ApplicableToAll);
                this.dbServer.AddInParameter(storedProcCommand, "ApplicableToAllDiscount", DbType.Double, details.ApplicableToAllDiscount);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, details.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "IsFixed", DbType.Boolean, details.IsFixedRate);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceFixedRate", DbType.Double, details.ServiceFixedRate);
                this.dbServer.AddInParameter(storedProcCommand, "PharmacyFixedRate", DbType.Double, details.PharmacyFixedRate);
                this.dbServer.AddInParameter(storedProcCommand, "ServicePercentage", DbType.Double, details.ServicePercentage);
                this.dbServer.AddInParameter(storedProcCommand, "PharmacyPercentage", DbType.Double, details.PharmacyPercentage);
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
                long serviceID = 0L;
                int num2 = 0;
                if ((details.PackageDetails != null) && (details.PackageDetails.Count != 0))
                {
                    foreach (clsPackageServiceDetailsVO svo in details.PackageDetails)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_UpdatePackageServiceDetailsForPackage");
                        this.dbServer.AddInParameter(command2, "PackageID", DbType.Int64, details.ID);
                        this.dbServer.AddInParameter(command2, "PackageUnitID", DbType.Int64, details.UnitID);
                        this.dbServer.AddInParameter(command2, "SpecilizationID", DbType.Int64, svo.DepartmentID);
                        this.dbServer.AddInParameter(command2, "IsSpecilizationGroup", DbType.Boolean, svo.IsSpecilizationGroup);
                        this.dbServer.AddInParameter(command2, "ServiceID", DbType.Int64, svo.ServiceID);
                        this.dbServer.AddInParameter(command2, "Amount", DbType.Double, svo.Amount);
                        this.dbServer.AddInParameter(command2, "Discount", DbType.Double, svo.Discount);
                        this.dbServer.AddInParameter(command2, "IsDiscountOnQuantity", DbType.Boolean, svo.IsDiscountOnQuantity);
                        this.dbServer.AddInParameter(command2, "AgeLimit", DbType.Int64, svo.AgeLimit);
                        this.dbServer.AddInParameter(command2, "IsFollowupNotRequired", DbType.Boolean, svo.IsFollowupNotRequired);
                        this.dbServer.AddInParameter(command2, "ApplicableTo", DbType.Int64, svo.SelectedGender.ID);
                        this.dbServer.AddInParameter(command2, "Quantity", DbType.Double, svo.Quantity);
                        this.dbServer.AddInParameter(command2, "Infinite", DbType.Boolean, svo.Infinite);
                        this.dbServer.AddInParameter(command2, "NetAmount", DbType.Double, svo.NetAmount);
                        this.dbServer.AddInParameter(command2, "IsActive", DbType.Boolean, svo.IsActive);
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                        this.dbServer.AddInParameter(command2, "Month", DbType.String, svo.Month);
                        this.dbServer.AddInParameter(command2, "MonthStatus", DbType.Boolean, svo.MonthStatus);
                        this.dbServer.AddInParameter(command2, "AdjustableHead", DbType.Boolean, svo.AdjustableHead);
                        this.dbServer.AddInParameter(command2, "IsFixed", DbType.Boolean, svo.IsFixed);
                        this.dbServer.AddInParameter(command2, "Rate", DbType.Double, svo.Rate);
                        this.dbServer.AddInParameter(command2, "Percentage", DbType.Double, svo.RatePercentage);
                        this.dbServer.AddInParameter(command2, "IsDoctorSharePercentage", DbType.Boolean, svo.IsDoctorSharePercentage);
                        this.dbServer.AddInParameter(command2, "ConsiderAdjustable", DbType.Boolean, svo.ConsiderAdjustable);
                        this.dbServer.AddInParameter(command2, "ProcessID", DbType.Int64, svo.SelectedProcess.ID);
                        this.dbServer.AddInParameter(command2, "AdjustableHeadType", DbType.Int32, svo.AdjustableHeadType);
                        this.dbServer.AddInParameter(command2, "IsConsumables", DbType.Boolean, svo.IsConsumables);
                        this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                        this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command2, transaction);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                        svo.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                        if (serviceID != svo.ServiceID)
                        {
                            serviceID = svo.ServiceID;
                            num2 = 0;
                        }
                        if ((serviceID == svo.ServiceID) && (num2 == 0))
                        {
                            if ((details.PackageServiceRelationDetailsDelete != null) && (details.PackageServiceRelationDetailsDelete.Count != 0))
                            {
                                foreach (clsPackageServiceRelationsVO svo2 in details.PackageServiceRelationDetailsDelete)
                                {
                                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_DeletePackageServiceRelationForPackage");
                                    this.dbServer.AddInParameter(command3, "RelationID", DbType.Int64, svo2.RelationID);
                                    this.dbServer.AddInParameter(command3, "PackageID", DbType.Int64, BizActionObj.Details.ID);
                                    this.dbServer.AddInParameter(command3, "PackageUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command3, "ServiceID", DbType.Int64, svo2.ServiceID);
                                    this.dbServer.AddInParameter(command3, "SpecilizationID", DbType.Int64, svo.DepartmentID);
                                    this.dbServer.AddInParameter(command3, "IsSpecilizationGroup", DbType.Boolean, svo.IsSpecilizationGroup);
                                    this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, svo.Status);
                                    this.dbServer.AddInParameter(command3, "ID", DbType.Int64, svo2.ID);
                                    this.dbServer.ExecuteNonQuery(command3, transaction);
                                }
                            }
                            if ((details.PackageServiceRelationDetails != null) && (details.PackageServiceRelationDetails.Count != 0))
                            {
                                foreach (clsPackageServiceRelationsVO svo3 in details.PackageServiceRelationDetails)
                                {
                                    DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_UpdatePackageServiceRelationForPackage");
                                    this.dbServer.AddInParameter(command4, "RelationID", DbType.Int64, svo3.RelationID);
                                    this.dbServer.AddInParameter(command4, "PackageID", DbType.Int64, BizActionObj.Details.ID);
                                    this.dbServer.AddInParameter(command4, "PackageUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command4, "ServiceID", DbType.Int64, svo3.ServiceID);
                                    this.dbServer.AddInParameter(command4, "SpecilizationID", DbType.Int64, svo.DepartmentID);
                                    this.dbServer.AddInParameter(command4, "IsSpecilizationGroup", DbType.Boolean, svo.IsSpecilizationGroup);
                                    this.dbServer.AddInParameter(command4, "IsSetAllRelations", DbType.Boolean, svo3.IsSetAllRelations);
                                    this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command4, "Status", DbType.Boolean, svo.Status);
                                    this.dbServer.AddInParameter(command4, "ProcessID", DbType.Int64, svo3.ProcessID);
                                    this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo3.ID);
                                    this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int64, 0);
                                    this.dbServer.ExecuteNonQuery(command4, transaction);
                                    svo3.ID = (long) this.dbServer.GetParameterValue(command4, "ID");
                                }
                            }
                            if ((details.ServiceConditionDetailsDelete != null) && (details.ServiceConditionDetailsDelete.Count != 0))
                            {
                                foreach (clsPackageServiceConditionsVO svo4 in details.ServiceConditionDetailsDelete)
                                {
                                    DbCommand command5 = this.dbServer.GetStoredProcCommand("CIMS_DeletePackageServiceConditionForPackage");
                                    this.dbServer.AddInParameter(command5, "PackageID", DbType.Int64, BizActionObj.Details.ID);
                                    this.dbServer.AddInParameter(command5, "PackageUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command5, "PackageServiceId", DbType.Int64, svo.ServiceID);
                                    this.dbServer.AddInParameter(command5, "ServiceID", DbType.Int64, svo4.SelectedService.ID);
                                    this.dbServer.AddInParameter(command5, "SpecilizationID", DbType.Int64, svo.DepartmentID);
                                    this.dbServer.AddInParameter(command5, "IsSpecilizationGroup", DbType.Boolean, svo.IsSpecilizationGroup);
                                    this.dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command5, "Status", DbType.Boolean, svo4.Status);
                                    this.dbServer.AddInParameter(command5, "ID", DbType.Int64, svo4.ID);
                                    this.dbServer.ExecuteNonQuery(command5, transaction);
                                }
                            }
                            if ((details.ServiceConditionDetails != null) && (details.ServiceConditionDetails.Count != 0))
                            {
                                foreach (clsPackageServiceConditionsVO svo5 in details.ServiceConditionDetails)
                                {
                                    DbCommand command6 = this.dbServer.GetStoredProcCommand("CIMS_UpdatePackageServiceConditionForPackage");
                                    this.dbServer.AddInParameter(command6, "PackageID", DbType.Int64, BizActionObj.Details.ID);
                                    this.dbServer.AddInParameter(command6, "PackageUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command6, "PackageServiceId", DbType.Int64, svo.ServiceID);
                                    this.dbServer.AddInParameter(command6, "ServiceID", DbType.Int64, svo5.SelectedService.ID);
                                    this.dbServer.AddInParameter(command6, "Rate", DbType.Double, svo5.Rate);
                                    this.dbServer.AddInParameter(command6, "Quantity", DbType.Double, svo5.Quantity);
                                    this.dbServer.AddInParameter(command6, "Discount", DbType.Double, svo5.Discount);
                                    this.dbServer.AddInParameter(command6, "ConditionType", DbType.String, svo5.SelectedCondition.Description);
                                    this.dbServer.AddInParameter(command6, "ConditionTypeID", DbType.String, svo5.SelectedCondition.ID);
                                    this.dbServer.AddInParameter(command6, "SpecilizationID", DbType.Int64, svo.DepartmentID);
                                    this.dbServer.AddInParameter(command6, "IsSpecilizationGroup", DbType.Boolean, svo.IsSpecilizationGroup);
                                    this.dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command6, "Status", DbType.Boolean, svo5.Status);
                                    this.dbServer.AddParameter(command6, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo5.ID);
                                    this.dbServer.AddOutParameter(command6, "ResultStatus", DbType.Int64, 0);
                                    this.dbServer.ExecuteNonQuery(command6, transaction);
                                    svo5.ID = (long) this.dbServer.GetParameterValue(command6, "ID");
                                }
                            }
                            num2++;
                        }
                    }
                }
                 serviceID = 0L;
                int num4 = 0;
                if (BizActionObj.IsSavePatientData && ((details.PackageDetails != null) && (details.PackageDetails.Count != 0)))
                {
                    foreach (clsPackageServiceDetailsVO svo6 in details.PackageDetails)
                    {
                        if (serviceID != svo6.ServiceID)
                        {
                            serviceID = svo6.ServiceID;
                            num4 = 0;
                        }
                        if ((serviceID == svo6.ServiceID) && (num4 == 0))
                        {
                            if (BizActionObj.IsSavePatientData)
                            {
                                DbCommand command7 = this.dbServer.GetStoredProcCommand("CIMS_UpdatePatientPackageDetailsForPackageAllAddAfterFreeze2");
                                this.dbServer.AddInParameter(command7, "PackageID", DbType.Int64, details.ID);
                                this.dbServer.AddInParameter(command7, "PackageUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command7, "ServiceID", DbType.Int64, svo6.ServiceID);
                                this.dbServer.AddInParameter(command7, "SpecilizationID", DbType.Int64, svo6.DepartmentID);
                                this.dbServer.AddInParameter(command7, "IsSpecilizationGroup", DbType.Boolean, svo6.IsSpecilizationGroup);
                                this.dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.ExecuteNonQuery(command7, transaction);
                            }
                            num4++;
                        }
                    }
                }
                transaction.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.Details = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return BizActionObj;
        }

        public override IValueObject UpdatePackageApplicableToAllPharmacyItemsNew(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            clsAddPackagePharmacyItemsNewBizActionVO nvo = valueObject as clsAddPackagePharmacyItemsNewBizActionVO;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePackageApplicableToAllPharmacyItemsForPackage");
                this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int64, nvo.PackageID);
                this.dbServer.AddInParameter(storedProcCommand, "PackageUnitID", DbType.Int64, nvo.PackageUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ApplicableToAll", DbType.Boolean, nvo.ApplicableToAll);
                this.dbServer.AddInParameter(storedProcCommand, "ApplicableToAllDiscount", DbType.Double, nvo.ApplicableToAllDiscount);
                this.dbServer.AddInParameter(storedProcCommand, "TotalBudget", DbType.Double, nvo.TotalBudget);
                int num = this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                if (nvo.ItemDetails != null)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeletePackageItemDetailsAllForPackage");
                    this.dbServer.AddInParameter(command2, "PackageID", DbType.Int64, nvo.PackageID);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                }
                transaction.Commit();
                nvo.SuccessStatus = (num <= 0) ? 0 : 1;
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                transaction.Rollback();
                nvo.ItemDetails = null;
            }
            return nvo;
        }

        public override IValueObject UpdatePackageFreezeStatusNew(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            clsAddPackageServiceNewBizActionVO nvo = valueObject as clsAddPackageServiceNewBizActionVO;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = null;
                clsPackageServiceVO details = nvo.Details;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePackageMasterFreezeStatusForPackage");
                this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int64, details.ID);
                this.dbServer.AddInParameter(storedProcCommand, "PackageUnitID", DbType.Int64, details.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, details.IsFreezed);
                int num = this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                transaction.Commit();
                nvo.SuccessStatus = (num <= 0) ? 0 : 1;
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                transaction.Rollback();
                nvo.Details = null;
            }
            return nvo;
        }

        public override IValueObject UpdatePackageServiceMasterStatus(IValueObject valueObject, clsUserVO userVO)
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

        public override IValueObject UpdatePackgeApproveStatusNew(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            clsAddPackageServiceNewBizActionVO nvo = valueObject as clsAddPackageServiceNewBizActionVO;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = null;
                clsPackageServiceVO details = nvo.Details;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePackgeApproveStatusForPackage");
                this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int64, details.ID);
                this.dbServer.AddInParameter(storedProcCommand, "PackageUnitID", DbType.Int64, details.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsApproved", DbType.Boolean, details.IsApproved);
                int num = this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                transaction.Commit();
                nvo.SuccessStatus = (num <= 0) ? 0 : 1;
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                transaction.Rollback();
                nvo.Details = null;
            }
            return nvo;
        }
    }
}

