namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration;
    using PalashDynamics.ValueObjects.Billing;
    using PalashDynamics.ValueObjects.Patient;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsTariffMasterDAL : clsBaseTariffMasterDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsTariffMasterDAL()
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

        public override IValueObject AddTariff(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddTariffMasterBizActionVO bizActionObj = valueObject as clsAddTariffMasterBizActionVO;
            bizActionObj = (bizActionObj.TariffDetails.ID != 0L) ? this.UpdateTariffDetails(bizActionObj, UserVo) : this.AddTariffDetails(bizActionObj, UserVo);
            return valueObject;
        }

        private clsAddTariffMasterBizActionVO AddTariffDetails(clsAddTariffMasterBizActionVO BizActionObj, clsUserVO UserVo)
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
                clsTariffMasterBizActionVO tariffDetails = BizActionObj.TariffDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddTariffMaster");
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, tariffDetails.Code.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, tariffDetails.Description.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "NoOfVisit", DbType.Int32, tariffDetails.NoOfVisit);
                this.dbServer.AddInParameter(storedProcCommand, "AllVisit", DbType.Boolean, tariffDetails.AllVisit);
                this.dbServer.AddInParameter(storedProcCommand, "Specify", DbType.Boolean, tariffDetails.Specify);
                this.dbServer.AddInParameter(storedProcCommand, "CheckDate", DbType.Boolean, tariffDetails.CheckDate);
                this.dbServer.AddInParameter(storedProcCommand, "EffectiveDate", DbType.DateTime, tariffDetails.EffectiveDate);
                this.dbServer.AddInParameter(storedProcCommand, "ExpiryDate", DbType.DateTime, tariffDetails.ExpiryDate);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, tariffDetails.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tariffDetails.ID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.TariffDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if ((tariffDetails.ServiceMasterList != null) && (tariffDetails.ServiceMasterList.Count > 0))
                {
                    foreach (clsServiceMasterVO rvo in tariffDetails.ServiceMasterList)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddTariffService");
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "TariffID", DbType.Int64, tariffDetails.ID);
                        this.dbServer.AddInParameter(command2, "ServiceID", DbType.Int64, rvo.ServiceID);
                        this.dbServer.AddInParameter(command2, "ServiceCode", DbType.String, rvo.ServiceCode);
                        this.dbServer.AddInParameter(command2, "SpecializationId", DbType.Int64, rvo.Specialization);
                        this.dbServer.AddInParameter(command2, "SubSpecializationId", DbType.Int64, rvo.SubSpecialization);
                        this.dbServer.AddInParameter(command2, "ShortDescription", DbType.String, rvo.ShortDescription);
                        this.dbServer.AddInParameter(command2, "LongDescription", DbType.String, rvo.LongDescription);
                        this.dbServer.AddInParameter(command2, "Description", DbType.String, rvo.ServiceName);
                        this.dbServer.AddInParameter(command2, "CodeType", DbType.Int64, rvo.CodeType);
                        this.dbServer.AddInParameter(command2, "Code", DbType.String, rvo.Code);
                        this.dbServer.AddInParameter(command2, "StaffDiscount", DbType.Boolean, rvo.StaffDiscount);
                        this.dbServer.AddInParameter(command2, "StaffDiscountAmount", DbType.Decimal, rvo.StaffDiscountAmount);
                        this.dbServer.AddInParameter(command2, "StaffDiscountPercent", DbType.Decimal, rvo.StaffDiscountPercent);
                        this.dbServer.AddInParameter(command2, "StaffDependantDiscount", DbType.Boolean, rvo.StaffDependantDiscount);
                        this.dbServer.AddInParameter(command2, "StaffDependantDiscountAmount", DbType.Decimal, rvo.StaffDependantDiscountAmount);
                        this.dbServer.AddInParameter(command2, "StaffDependantDiscountPercent", DbType.Decimal, rvo.StaffDependantDiscountPercent);
                        this.dbServer.AddInParameter(command2, "Concession", DbType.Boolean, rvo.Concession);
                        this.dbServer.AddInParameter(command2, "ConcessionAmount", DbType.Decimal, rvo.ConcessionAmount);
                        this.dbServer.AddInParameter(command2, "ConcessionPercent", DbType.Decimal, rvo.ConcessionPercent);
                        this.dbServer.AddInParameter(command2, "ServiceTax", DbType.Boolean, rvo.ServiceTax);
                        this.dbServer.AddInParameter(command2, "ServiceTaxAmount", DbType.Decimal, rvo.ServiceTaxAmount);
                        this.dbServer.AddInParameter(command2, "ServiceTaxPercent", DbType.Decimal, rvo.ServiceTaxPercent);
                        this.dbServer.AddInParameter(command2, "InHouse", DbType.Boolean, rvo.InHouse);
                        this.dbServer.AddInParameter(command2, "DoctorShare", DbType.Boolean, rvo.DoctorShare);
                        this.dbServer.AddInParameter(command2, "DoctorSharePercentage", DbType.Decimal, rvo.DoctorSharePercentage);
                        this.dbServer.AddInParameter(command2, "DoctorShareAmount", DbType.Decimal, rvo.DoctorShareAmount);
                        this.dbServer.AddInParameter(command2, "RateEditable", DbType.Boolean, rvo.RateEditable);
                        this.dbServer.AddInParameter(command2, "MaxRate", DbType.Decimal, rvo.MaxRate);
                        this.dbServer.AddInParameter(command2, "MinRate", DbType.Decimal, rvo.MinRate);
                        this.dbServer.AddInParameter(command2, "Rate", DbType.Decimal, rvo.Rate);
                        this.dbServer.AddInParameter(command2, "CheckedAllTariffs", DbType.Boolean, rvo.CheckedAllTariffs);
                        this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, rvo.SelectService);
                        this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, rvo.CreatedUnitID);
                        this.dbServer.AddInParameter(command2, "UpdatedUnitID", DbType.Int64, rvo.UpdatedUnitID);
                        this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, rvo.AddedBy);
                        this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, rvo.AddedOn);
                        this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, rvo.AddedDateTime);
                        this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, rvo.AddedWindowsLoginName);
                        this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0);
                        this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, rvo.ID);
                        this.dbServer.ExecuteNonQuery(command2, transaction);
                        rvo.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddTariffServiceClassRateDetail");
                        this.dbServer.AddInParameter(command3, "TariffServiceId", DbType.Int64, rvo.ID);
                        this.dbServer.AddInParameter(command3, "ClassId", DbType.Int64, 1);
                        this.dbServer.AddInParameter(command3, "Rate", DbType.Int64, rvo.Rate);
                        this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, rvo.SelectService);
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                    }
                }
                if ((tariffDetails.ServiceSpecializationMasterList != null) && (tariffDetails.ServiceSpecializationMasterList.Count > 0))
                {
                    foreach (clsTariffMasterBizActionVO nvo2 in tariffDetails.ServiceSpecializationMasterList)
                    {
                        DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_AddTariffServieLinkingBySpecialization");
                        this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command4, "TariffID", DbType.Int64, tariffDetails.ID);
                        this.dbServer.AddInParameter(command4, "Specialization", DbType.Int64, nvo2.GroupID);
                        this.dbServer.AddInParameter(command4, "SubSpecialization", DbType.Int64, nvo2.SubGroupID);
                        this.dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, nvo2.CreatedUnitID);
                        this.dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, nvo2.AddedBy);
                        this.dbServer.AddInParameter(command4, "AddedOn", DbType.String, nvo2.AddedOn);
                        this.dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, nvo2.AddedDateTime);
                        this.dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, nvo2.AddedWindowsLoginName);
                        this.dbServer.ExecuteNonQuery(command4, transaction);
                    }
                }
                transaction.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.TariffDetails = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return BizActionObj;
        }

        public override IValueObject GetServiceByTariffID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetServiceByTariffIDBizActionVO nvo = (clsGetServiceByTariffIDBizActionVO) valueObject;
            try
            {
                clsTariffMasterBizActionVO tariffDetails = nvo.TariffDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetServiceByTariffID");
                this.dbServer.AddInParameter(storedProcCommand, "TariffID ", DbType.Int64, nvo.TariffID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (nvo.TariffDetails == null)
                        {
                            nvo.TariffDetails = new clsTariffMasterBizActionVO();
                        }
                        nvo.TariffDetails.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.TariffDetails.UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]);
                        nvo.TariffDetails.Code = (string) DALHelper.HandleDBNull(reader["Code"]);
                        nvo.TariffDetails.Description = (string) DALHelper.HandleDBNull(reader["Description"]);
                        nvo.TariffDetails.NoOfVisit = (int) DALHelper.HandleDBNull(reader["NoOfVisit"]);
                        nvo.TariffDetails.AllVisit = (bool) DALHelper.HandleDBNull(reader["AllVisit"]);
                        nvo.TariffDetails.Specify = (bool) DALHelper.HandleDBNull(reader["Specify"]);
                        nvo.TariffDetails.CheckDate = (bool) DALHelper.HandleDBNull(reader["CheckDate"]);
                        if (DALHelper.HandleDBNull(reader["EffectiveDate"]) != null)
                        {
                            nvo.TariffDetails.EffectiveDate = new DateTime?((DateTime) DALHelper.HandleDBNull(reader["EffectiveDate"]));
                        }
                        if (DALHelper.HandleDBNull(reader["ExpiryDate"]) != null)
                        {
                            nvo.TariffDetails.ExpiryDate = new DateTime?((DateTime) DALHelper.HandleDBNull(reader["ExpiryDate"]));
                        }
                        nvo.TariffDetails.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    nvo.TariffDetails.ServiceMasterList = new List<clsServiceMasterVO>();
                    while (reader.Read())
                    {
                        clsServiceMasterVO item = new clsServiceMasterVO {
                            ID = (long) DALHelper.HandleDBNull(reader["Id"]),
                            TariffID = (long) DALHelper.HandleDBNull(reader["TariffID"]),
                            ServiceID = (long) DALHelper.HandleDBNull(reader["ServiceId"]),
                            ServiceCode = (string) DALHelper.HandleDBNull(reader["ServiceCode"]),
                            ServiceName = (string) DALHelper.HandleDBNull(reader["ServiceName"]),
                            Rate = (decimal) DALHelper.HandleDBNull(reader["Rate"]),
                            Specialization = (long) DALHelper.HandleDBNull(reader["SpecializationId"]),
                            SpecializationString = (string) DALHelper.HandleDBNull(reader["Specialization"]),
                            SubSpecialization = (long) DALHelper.HandleDBNull(reader["SubSpecializationId"]),
                            ShortDescription = (string) DALHelper.HandleDBNull(reader["ShortDescription"]),
                            LongDescription = (string) DALHelper.HandleDBNull(reader["LongDescription"]),
                            SelectService = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.TariffDetails.ServiceMasterList.Add(item);
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

        public override IValueObject GetServicesforIssue(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetServiceForIssueBizActionVO nvo = (clsGetServiceForIssueBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetServicesForIssue");
                this.dbServer.AddInParameter(storedProcCommand, "TariffID ", DbType.Int64, nvo.TariffID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ServiceList == null)
                    {
                        nvo.ServiceList = new List<clsPatientServiceDetails>();
                    }
                    while (reader.Read())
                    {
                        clsPatientServiceDetails item = new clsPatientServiceDetails {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            TariffID = (long) DALHelper.HandleDBNull(reader["TariffID"]),
                            ServiceID = (long) DALHelper.HandleDBNull(reader["ServiceId"]),
                            ServiceCode = (string) DALHelper.HandleDBNull(reader["ServiceCode"]),
                            ServiceName = (string) DALHelper.HandleDBNull(reader["ServiceName"]),
                            Rate = Convert.ToDouble((decimal) DALHelper.HandleDBNull(reader["Rate"])),
                            SelectService = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.ServiceList.Add(item);
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

        public override IValueObject GetSpecializationsByTariffId(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetSpecializationsByTariffIdBizActionVO nvo = (clsGetSpecializationsByTariffIdBizActionVO) valueObject;
            try
            {
                DbDataReader reader = null;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSpecializationsByTariffID");
                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, nvo.TariffID);
                this.dbServer.AddInParameter(storedProcCommand, "IsFromTariffCopyUtility", DbType.Boolean, nvo.IsFromTariffCopyUtility);
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                if (nvo.IsFromTariffCopyUtility)
                {
                    reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.SpecializationList == null)
                        {
                            nvo.SpecializationList = new List<clsSubSpecializationVO>();
                        }
                        while (reader.Read())
                        {
                            clsSubSpecializationVO item = new clsSubSpecializationVO {
                                SpecializationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                SpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                                SubSpecializationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["SuSpecializationID"])),
                                SubSpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecializationName"]))
                            };
                            nvo.SpecializationList.Add(item);
                        }
                    }
                    reader.NextResult();
                    nvo.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                }
                else
                {
                    reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.TariffList == null)
                        {
                            nvo.TariffList = new List<clsTariffMasterBizActionVO>();
                        }
                        while (true)
                        {
                            if (!reader.Read())
                            {
                                reader.NextResult();
                                nvo.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                                break;
                            }
                            clsTariffMasterBizActionVO item = new clsTariffMasterBizActionVO {
                                GroupID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"])),
                                StrGroup = Convert.ToString(DALHelper.HandleDBNull(reader["SpecializationName"])),
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffId"]))
                            };
                            nvo.TariffList.Add(item);
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

        public override IValueObject GetTariffList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetTariffListBizActionVO nvo = (clsGetTariffListBizActionVO) valueObject;
            try
            {
                DbDataReader reader = null;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetTariffDetails");
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.TariffList == null)
                    {
                        nvo.TariffList = new List<clsTariffMasterBizActionVO>();
                    }
                    while (reader.Read())
                    {
                        clsTariffMasterBizActionVO item = new clsTariffMasterBizActionVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.TariffList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        private clsAddTariffMasterBizActionVO UpdateTariffDetails(clsAddTariffMasterBizActionVO BizActionObj, clsUserVO UserVo)
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
                clsTariffMasterBizActionVO tariffDetails = BizActionObj.TariffDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateTariffMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, tariffDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, tariffDetails.Code.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, tariffDetails.Description.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "NoOfVisit", DbType.Int32, tariffDetails.NoOfVisit);
                this.dbServer.AddInParameter(storedProcCommand, "AllVisit", DbType.Boolean, tariffDetails.AllVisit);
                this.dbServer.AddInParameter(storedProcCommand, "Specify", DbType.Boolean, tariffDetails.Specify);
                this.dbServer.AddInParameter(storedProcCommand, "CheckDate", DbType.Boolean, tariffDetails.CheckDate);
                this.dbServer.AddInParameter(storedProcCommand, "EffectiveDate", DbType.DateTime, tariffDetails.EffectiveDate);
                this.dbServer.AddInParameter(storedProcCommand, "ExpiryDate", DbType.DateTime, tariffDetails.ExpiryDate);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                if ((tariffDetails.ServiceMasterList != null) && (tariffDetails.ServiceMasterList.Count > 0))
                {
                    foreach (clsServiceMasterVO rvo in tariffDetails.ServiceMasterList)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddTariffService");
                        this.dbServer.AddOutParameter(command2, "ID", DbType.Int64, 0);
                        this.dbServer.AddInParameter(command2, "TariffID", DbType.Int64, tariffDetails.ID);
                        this.dbServer.AddInParameter(command2, "ServiceID", DbType.Int64, rvo.ServiceID);
                        this.dbServer.AddInParameter(command2, "ServiceCode", DbType.String, rvo.ServiceCode);
                        this.dbServer.AddInParameter(command2, "SpecializationId", DbType.Int64, rvo.Specialization);
                        this.dbServer.AddInParameter(command2, "SubSpecializationId", DbType.Int64, rvo.SubSpecialization);
                        this.dbServer.AddInParameter(command2, "ShortDescription", DbType.String, rvo.ShortDescription);
                        this.dbServer.AddInParameter(command2, "LongDescription", DbType.String, rvo.LongDescription);
                        this.dbServer.AddInParameter(command2, "Description", DbType.String, rvo.ServiceName);
                        this.dbServer.AddInParameter(command2, "CodeType", DbType.Int64, rvo.CodeType);
                        this.dbServer.AddInParameter(command2, "Code", DbType.String, rvo.Code);
                        this.dbServer.AddInParameter(command2, "StaffDiscount", DbType.Boolean, rvo.StaffDiscount);
                        this.dbServer.AddInParameter(command2, "StaffDiscountAmount", DbType.Decimal, rvo.StaffDiscountAmount);
                        this.dbServer.AddInParameter(command2, "StaffDiscountPercent", DbType.Decimal, rvo.StaffDiscountPercent);
                        this.dbServer.AddInParameter(command2, "StaffDependantDiscount", DbType.Boolean, rvo.StaffDependantDiscount);
                        this.dbServer.AddInParameter(command2, "StaffDependantDiscountAmount", DbType.Decimal, rvo.StaffDependantDiscountAmount);
                        this.dbServer.AddInParameter(command2, "StaffDependantDiscountPercent", DbType.Decimal, rvo.StaffDependantDiscountPercent);
                        this.dbServer.AddInParameter(command2, "Concession", DbType.Boolean, rvo.Concession);
                        this.dbServer.AddInParameter(command2, "ConcessionAmount", DbType.Decimal, rvo.ConcessionAmount);
                        this.dbServer.AddInParameter(command2, "ConcessionPercent", DbType.Decimal, rvo.ConcessionPercent);
                        this.dbServer.AddInParameter(command2, "ServiceTax", DbType.Boolean, rvo.ServiceTax);
                        this.dbServer.AddInParameter(command2, "ServiceTaxAmount", DbType.Decimal, rvo.ServiceTaxAmount);
                        this.dbServer.AddInParameter(command2, "ServiceTaxPercent", DbType.Decimal, rvo.ServiceTaxPercent);
                        this.dbServer.AddInParameter(command2, "InHouse", DbType.Boolean, rvo.InHouse);
                        this.dbServer.AddInParameter(command2, "DoctorShare", DbType.Boolean, rvo.DoctorShare);
                        this.dbServer.AddInParameter(command2, "DoctorSharePercentage", DbType.Decimal, rvo.DoctorSharePercentage);
                        this.dbServer.AddInParameter(command2, "DoctorShareAmount", DbType.Decimal, rvo.DoctorShareAmount);
                        this.dbServer.AddInParameter(command2, "RateEditable", DbType.Boolean, rvo.RateEditable);
                        this.dbServer.AddInParameter(command2, "MaxRate", DbType.Decimal, rvo.MaxRate);
                        this.dbServer.AddInParameter(command2, "MinRate", DbType.Decimal, rvo.MinRate);
                        this.dbServer.AddInParameter(command2, "Rate", DbType.Decimal, rvo.Rate);
                        this.dbServer.AddInParameter(command2, "CheckedAllTariffs", DbType.Boolean, rvo.CheckedAllTariffs);
                        this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, rvo.SelectService);
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, rvo.CreatedUnitID);
                        this.dbServer.AddInParameter(command2, "UpdatedUnitID", DbType.Int64, rvo.UpdatedUnitID);
                        this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, rvo.AddedBy);
                        this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, rvo.AddedOn);
                        this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, rvo.AddedWindowsLoginName);
                        this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0);
                        this.dbServer.ExecuteNonQuery(command2, transaction);
                        rvo.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddTariffServiceClassRateDetail");
                        this.dbServer.AddInParameter(command3, "TariffServiceId", DbType.Int64, rvo.ID);
                        this.dbServer.AddInParameter(command3, "ClassId", DbType.Int64, 1);
                        this.dbServer.AddInParameter(command3, "Rate", DbType.Int64, rvo.Rate);
                        this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, rvo.SelectService);
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                    }
                }
                transaction.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.TariffDetails = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return BizActionObj;
        }
    }
}

