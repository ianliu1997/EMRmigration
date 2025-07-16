namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.CustomExceptions;
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.OutPatientDepartment;
    using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsPatientSposorDAL : clsBasePatientSposorDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsPatientSposorDAL()
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

        public override IValueObject AddFollowUpPatientNew(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddFollowUpStatusByPatientIdBizActionVO nvo = valueObject as clsAddFollowUpStatusByPatientIdBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatientFollowUpPackageDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, nvo.TariffID);
                this.dbServer.AddInParameter(storedProcCommand, "SponsorID", DbType.Int64, nvo.SponsorID);
                this.dbServer.AddInParameter(storedProcCommand, "SponsorUnitID", DbType.Int64, nvo.SponsorUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, nvo.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject AddPatientSponsor(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientSponsorBizActionVO bizActionObj = valueObject as clsAddPatientSponsorBizActionVO;
            bizActionObj = (bizActionObj.PatientSponsorDetails.ID != 0L) ? this.UpdatePatientSponsorDetails(bizActionObj) : this.AddPatientSponsorDetails(bizActionObj, UserVo);
            return valueObject;
        }

        private clsPatientSponsorCardDetailsVO AddPatientSponsorCard(clsPatientSponsorCardDetailsVO valueObject, clsUserVO UserVo)
        {
            try
            {
                clsPatientSponsorCardDetailsVO svo = valueObject;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatientSponsorCardDetails");
                this.dbServer.AddInParameter(storedProcCommand, "SponsorID", DbType.Int64, svo.SponsorID);
                this.dbServer.AddInParameter(storedProcCommand, "Title", DbType.String, svo.Title);
                this.dbServer.AddInParameter(storedProcCommand, "Image", DbType.Binary, svo.Image);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                if (svo.SponsorID == 0L)
                {
                    svo.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                }
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        private clsAddPatientSponsorBizActionVO AddPatientSponsorDetails(clsAddPatientSponsorBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsPatientSponsorVO patientSponsorDetails = BizActionObj.PatientSponsorDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatientSponsor");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, patientSponsorDetails.LinkServer);
                if (patientSponsorDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, patientSponsorDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, patientSponsorDetails.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, patientSponsorDetails.PatientUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientSourceID", DbType.Int64, patientSponsorDetails.PatientSourceID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientCategoryID", DbType.Int64, patientSponsorDetails.PatientCategoryID);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyID", DbType.Int64, patientSponsorDetails.CompanyID);
                this.dbServer.AddInParameter(storedProcCommand, "AssociatedCompanyID", DbType.Int64, patientSponsorDetails.AssociatedCompanyID);
                if (patientSponsorDetails.ReferenceNo != null)
                {
                    patientSponsorDetails.ReferenceNo = patientSponsorDetails.ReferenceNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ReferenceNo", DbType.String, patientSponsorDetails.ReferenceNo);
                this.dbServer.AddInParameter(storedProcCommand, "CreditLimit", DbType.Double, patientSponsorDetails.CreditLimit);
                this.dbServer.AddInParameter(storedProcCommand, "EffectiveDate", DbType.DateTime, patientSponsorDetails.EffectiveDate);
                this.dbServer.AddInParameter(storedProcCommand, "ExpiryDate", DbType.DateTime, patientSponsorDetails.ExpiryDate);
                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, patientSponsorDetails.TariffID);
                if (patientSponsorDetails.EmployeeNo != null)
                {
                    patientSponsorDetails.EmployeeNo = patientSponsorDetails.EmployeeNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "EmployeeNo", DbType.String, patientSponsorDetails.EmployeeNo);
                this.dbServer.AddInParameter(storedProcCommand, "DesignationID", DbType.Int64, patientSponsorDetails.DesignationID);
                if (patientSponsorDetails.Remark != null)
                {
                    patientSponsorDetails.Remark = patientSponsorDetails.Remark.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, patientSponsorDetails.Remark);
                this.dbServer.AddInParameter(storedProcCommand, "MemberRelationID", DbType.Int64, patientSponsorDetails.MemberRelationID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, patientSponsorDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, patientSponsorDetails.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "IsDupliSponser", DbType.Boolean, patientSponsorDetails.IsDupliSponser);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientSponsorDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.PatientSponsorDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject AddPatientSponsorDetailsWithTransaction(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            clsAddPatientSponsorBizActionVO nvo = (clsAddPatientSponsorBizActionVO) valueObject;
            try
            {
                if (pConnection == null)
                {
                    pConnection = this.dbServer.CreateConnection();
                }
                if (pConnection.State != ConnectionState.Open)
                {
                    pConnection.Open();
                }
                if (pTransaction == null)
                {
                    pTransaction = pConnection.BeginTransaction();
                }
                clsPatientSponsorVO patientSponsorDetails = nvo.PatientSponsorDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatientSponsor");
                storedProcCommand.Connection = pConnection;
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, patientSponsorDetails.LinkServer);
                if (patientSponsorDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, patientSponsorDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, patientSponsorDetails.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, patientSponsorDetails.PatientUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientSourceID", DbType.Int64, patientSponsorDetails.PatientSourceID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientCategoryID", DbType.Int64, patientSponsorDetails.PatientCategoryID);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyID", DbType.Int64, patientSponsorDetails.CompanyID);
                this.dbServer.AddInParameter(storedProcCommand, "AssociatedCompanyID", DbType.Int64, patientSponsorDetails.AssociatedCompanyID);
                if (patientSponsorDetails.ReferenceNo != null)
                {
                    patientSponsorDetails.ReferenceNo = patientSponsorDetails.ReferenceNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ReferenceNo", DbType.String, patientSponsorDetails.ReferenceNo);
                this.dbServer.AddInParameter(storedProcCommand, "CreditLimit", DbType.Double, patientSponsorDetails.CreditLimit);
                this.dbServer.AddInParameter(storedProcCommand, "EffectiveDate", DbType.DateTime, patientSponsorDetails.EffectiveDate);
                this.dbServer.AddInParameter(storedProcCommand, "ExpiryDate", DbType.DateTime, patientSponsorDetails.ExpiryDate);
                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, patientSponsorDetails.TariffID);
                if (patientSponsorDetails.EmployeeNo != null)
                {
                    patientSponsorDetails.EmployeeNo = patientSponsorDetails.EmployeeNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "EmployeeNo", DbType.String, patientSponsorDetails.EmployeeNo);
                this.dbServer.AddInParameter(storedProcCommand, "DesignationID", DbType.Int64, patientSponsorDetails.DesignationID);
                if (patientSponsorDetails.Remark != null)
                {
                    patientSponsorDetails.Remark = patientSponsorDetails.Remark.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, patientSponsorDetails.Remark);
                this.dbServer.AddInParameter(storedProcCommand, "MemberRelationID", DbType.Int64, patientSponsorDetails.MemberRelationID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, patientSponsorDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, patientSponsorDetails.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientSponsorDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, pTransaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.PatientSponsorDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception exception)
            {
                nvo.SuccessStatus = -1;
                throw exception;
            }
            return nvo;
        }

        public override IValueObject AddPatientSponsorForPathology(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientSponsorForPathologyBizActionVO nvo = valueObject as clsAddPatientSponsorForPathologyBizActionVO;
            try
            {
                clsPatientSponsorVO patientSponsorDetails = nvo.PatientSponsorDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatientSponsorForPathology");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, patientSponsorDetails.LinkServer);
                if (patientSponsorDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, patientSponsorDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, patientSponsorDetails.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, patientSponsorDetails.PatientUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientSourceID", DbType.Int64, patientSponsorDetails.PatientSourceID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientCategoryID", DbType.Int64, patientSponsorDetails.PatientCategoryID);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyID", DbType.Int64, patientSponsorDetails.CompanyID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, patientSponsorDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, patientSponsorDetails.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientSponsorDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.PatientSponsorDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject DeletePatientSponsor(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeletePatientSponsorBizActionVO nvo = valueObject as clsDeletePatientSponsorBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeletePatientSponsor");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.SponsorID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.SponsorUnitID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject GetFollowUpPatient(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetFollowUpStatusByPatientIdBizActionVO nvo = valueObject as clsGetFollowUpStatusByPatientIdBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientFollowUpDetailsByPatientID");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, nvo.TariffID);
                this.dbServer.AddInParameter(storedProcCommand, "SponsorID", DbType.Int64, nvo.SponsorID);
                this.dbServer.AddInParameter(storedProcCommand, "SponsorUnitID", DbType.Int64, nvo.SponsorUnitID);
                this.dbServer.AddParameter(storedProcCommand, "IsFollowUpAdded", DbType.Boolean, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.IsFollowUpAdded);
                this.dbServer.AddParameter(storedProcCommand, "IsPackageDetailsAdded", DbType.Boolean, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.IsPackageDetailsAdded);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.IsFollowUpAdded = (bool) this.dbServer.GetParameterValue(storedProcCommand, "IsFollowUpAdded");
                nvo.IsPackageDetailsAdded = (bool) this.dbServer.GetParameterValue(storedProcCommand, "IsPackageDetailsAdded");
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject GetPatientPackageInfoList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPackageInfoListBizActionVO nvo = (clsGetPatientPackageInfoListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientPackageDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID1);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID1);
                this.dbServer.AddInParameter(storedProcCommand, "IsfromCounterSale", DbType.Boolean, nvo.IsfromCounterSale);
                this.dbServer.AddInParameter(storedProcCommand, "PatientSourceID", DbType.Int64, nvo.PatientSourceID);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyID", DbType.Int64, nvo.PatientCompanyID);
                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, nvo.PatientTariffID);
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
                        nvo.MasterList.Add(new MasterListItem(Convert.ToInt64(reader["PackageID"]), reader["PackageName"].ToString(), Convert.ToInt64(reader["PatientInfoID"]), Convert.ToBoolean(reader["ApplicableToAll"]), Convert.ToDouble(reader["ApplicableToAllDiscount"]), Convert.ToDouble(reader["PharmacyFixedRate"]), Convert.ToInt64(reader["PackageBillID"]), Convert.ToInt64(reader["PackageBillUnitID"]), Convert.ToInt64(reader["ChargeID"]), Convert.ToDouble(Convert.ToDecimal(reader["PackageConsumptionAmount"])), Convert.ToDouble(reader["OPDConsumption"]), Convert.ToDouble(reader["OPDExcludeServices"]), Convert.ToDouble(reader["TotalPackageAdvance"]), Convert.ToDouble(reader["PharmacyConsumeAmount"]), Convert.ToDouble(reader["PackageConsumableLimit"]), Convert.ToDouble(reader["ConsumableServicesBilled"]), 0.0));
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject GetPatientSponsor(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientSponsorBizActionVO nvo = valueObject as clsGetPatientSponsorBizActionVO;
            try
            {
                clsPatientSponsorVO patientSponsorDetails = nvo.PatientSponsorDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientSponsorDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, patientSponsorDetails.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, patientSponsorDetails.PatientUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, patientSponsorDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.PatientSponsorDetails.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.PatientSponsorDetails.PatientId = (long) DALHelper.HandleDBNull(reader["PatientId"]);
                        nvo.PatientSponsorDetails.PatientUnitId = (long) DALHelper.HandleDBNull(reader["PatientUnitId"]);
                        nvo.PatientSponsorDetails.PatientSourceID = (long) DALHelper.HandleDBNull(reader["PatientSourceID"]);
                        nvo.PatientSponsorDetails.CompanyID = (long) DALHelper.HandleDBNull(reader["CompanyID"]);
                        nvo.PatientSponsorDetails.AssociatedCompanyID = (long) DALHelper.HandleDBNull(reader["AssociatedCompanyID"]);
                        nvo.PatientSponsorDetails.ReferenceNo = (string) DALHelper.HandleDBNull(reader["ReferenceNo"]);
                        nvo.PatientSponsorDetails.CreditLimit = (double) DALHelper.HandleDBNull(reader["CreditLimit"]);
                        nvo.PatientSponsorDetails.EffectiveDate = DALHelper.HandleDate(reader["EffectiveDate"]);
                        nvo.PatientSponsorDetails.ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]);
                        nvo.PatientSponsorDetails.TariffID = (long?) DALHelper.HandleDBNull(reader["TariffID"]);
                        nvo.PatientSponsorDetails.EmployeeNo = (string) DALHelper.HandleDBNull(reader["EmployeeNo"]);
                        nvo.PatientSponsorDetails.DesignationID = (long) DALHelper.HandleDBNull(reader["DesignationID"]);
                        nvo.PatientSponsorDetails.Remark = (string) DALHelper.HandleDBNull(reader["Remark"]);
                        nvo.PatientSponsorDetails.UnitId = (long) DALHelper.HandleDBNull(reader["UnitId"]);
                        nvo.PatientSponsorDetails.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                        nvo.PatientSponsorDetails.CreatedUnitId = (long) DALHelper.HandleDBNull(reader["CreatedUnitId"]);
                        nvo.PatientSponsorDetails.AddedBy = (long?) DALHelper.HandleDBNull(reader["AddedBy"]);
                        nvo.PatientSponsorDetails.AddedOn = (string) DALHelper.HandleDBNull(reader["AddedOn"]);
                        nvo.PatientSponsorDetails.AddedDateTime = DALHelper.HandleDate(reader["AddedDateTime"]);
                        nvo.PatientSponsorDetails.AddedWindowsLoginName = (string) DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]);
                        nvo.PatientSponsorDetails.UpdatedUnitId = (long?) DALHelper.HandleDBNull(reader["UpdatedUnitID"]);
                        nvo.PatientSponsorDetails.UpdatedBy = (long?) DALHelper.HandleDBNull(reader["UpdatedBy"]);
                        nvo.PatientSponsorDetails.UpdatedOn = (string) DALHelper.HandleDBNull(reader["UpdatedOn"]);
                        nvo.PatientSponsorDetails.UpdatedDateTime = DALHelper.HandleDate(reader["UpdatedDateTime"]);
                        nvo.PatientSponsorDetails.UpdatedWindowsLoginName = (string) DALHelper.HandleDBNull(reader["UpdatedWindowsLoginName"]);
                        nvo.PatientSponsorDetails.ComapnyName = (string) DALHelper.HandleDBNull(reader["company"]);
                        nvo.PatientSponsorDetails.AssociateComapnyName = (string) DALHelper.HandleDBNull(reader["AssociatCompany"]);
                        nvo.PatientSponsorDetails.Designation = (string) DALHelper.HandleDBNull(reader["designation"]);
                        nvo.PatientSponsorDetails.PatientSource = (string) DALHelper.HandleDBNull(reader["PatientSource"]);
                        nvo.PatientSponsorDetails.TariffName = (string) DALHelper.HandleDBNull(reader["Tariff"]);
                        nvo.PatientSponsorDetails.UnitName = (string) DALHelper.HandleDBNull(reader["UnitName"]);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject GetPatientSponsorCardList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientSponsorCardListBizActionVO nvo = valueObject as clsGetPatientSponsorCardListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientSponsorCardDetails");
                this.dbServer.AddInParameter(storedProcCommand, "SponsorID", DbType.Int64, nvo.SponsorID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DetailsList == null)
                    {
                        nvo.DetailsList = new List<clsPatientSponsorCardDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientSponsorCardDetailsVO item = new clsPatientSponsorCardDetailsVO {
                            SponsorID = (long) DALHelper.HandleDBNull(reader["SponsorID"]),
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            Title = (string) DALHelper.HandleDBNull(reader["Title"]),
                            Image = (byte[]) DALHelper.HandleDBNull(reader["Image"])
                        };
                        nvo.DetailsList.Add(item);
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

        public override IValueObject GetPatientSponsorCompanyList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientSponsorCompanyListBizActionVO nvo = (clsGetPatientSponsorCompanyListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientSponsorCompanyDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientSourceID", DbType.Int64, nvo.PatientSourceID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["CompanyID"], reader["CompanyName"].ToString()));
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject GetPatientSponsorGroupList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientSponsorGroupListBizActionVO nvo = valueObject as clsGetPatientSponsorGroupListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientSponsorGroupDetails");
                this.dbServer.AddInParameter(storedProcCommand, "SponsorID", DbType.Int64, nvo.SponsorID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DetailsList == null)
                    {
                        nvo.DetailsList = new List<clsPatientSponsorGroupDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientSponsorGroupDetailsVO item = new clsPatientSponsorGroupDetailsVO {
                            SponsorID = (long) DALHelper.HandleDBNull(reader["SponsorID"]),
                            GroupID = (long) DALHelper.HandleDBNull(reader["GroupID"]),
                            GroupName = (string) DALHelper.HandleDBNull(reader["GroupName"]),
                            DeductibleAmount = new decimal?((decimal) DALHelper.HandleDBNull(reader["DeductionAmount"])),
                            DeductionPercentage = new double?((double) DALHelper.HandleDBNull(reader["DeductionPercentage"]))
                        };
                        nvo.DetailsList.Add(item);
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

        public override IValueObject GetPatientSponsorList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientSponsorListBizActionVO nvo = valueObject as clsGetPatientSponsorListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientSponsorDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.SponsorID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientSponsorDetails == null)
                    {
                        nvo.PatientSponsorDetails = new List<clsPatientSponsorVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientSponsorVO item = new clsPatientSponsorVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            PatientId = (long) DALHelper.HandleDBNull(reader["PatientId"]),
                            PatientCategoryID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientCategoryID"]))),
                            PatientCategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientCategoryName"])),
                            PatientSourceID = (long) DALHelper.HandleDBNull(reader["PatientSourceID"]),
                            CompanyID = (long) DALHelper.HandleDBNull(reader["CompanyID"]),
                            AssociatedCompanyID = (long) DALHelper.HandleDBNull(reader["AssociatedCompanyID"]),
                            ReferenceNo = (string) DALHelper.HandleDBNull(reader["ReferenceNo"]),
                            CreditLimit = (double) DALHelper.HandleDBNull(reader["CreditLimit"]),
                            EffectiveDate = DALHelper.HandleDate(reader["EffectiveDate"]),
                            ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]),
                            TariffID = (long?) DALHelper.HandleDBNull(reader["TariffID"]),
                            EmployeeNo = (string) DALHelper.HandleDBNull(reader["EmployeeNo"]),
                            DesignationID = (long) DALHelper.HandleDBNull(reader["DesignationID"]),
                            PatientUnitId = (long) DALHelper.HandleDBNull(reader["PatientUnitId"]),
                            Remark = (string) DALHelper.HandleDBNull(reader["Remark"]),
                            UnitId = (long) DALHelper.HandleDBNull(reader["UnitId"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            CreatedUnitId = (long) DALHelper.HandleDBNull(reader["CreatedUnitId"]),
                            AddedBy = (long?) DALHelper.HandleDBNull(reader["AddedBy"]),
                            AddedOn = (string) DALHelper.HandleDBNull(reader["AddedOn"]),
                            AddedDateTime = DALHelper.HandleDate(reader["AddedDateTime"]),
                            AddedWindowsLoginName = (string) DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]),
                            UpdatedUnitId = (long?) DALHelper.HandleDBNull(reader["UpdatedUnitID"]),
                            UpdatedBy = (long?) DALHelper.HandleDBNull(reader["UpdatedBy"]),
                            UpdatedOn = (string) DALHelper.HandleDBNull(reader["UpdatedOn"]),
                            UpdatedDateTime = DALHelper.HandleDate(reader["UpdatedDateTime"]),
                            UpdatedWindowsLoginName = (string) DALHelper.HandleDBNull(reader["UpdatedWindowsLoginName"]),
                            ComapnyName = (string) DALHelper.HandleDBNull(reader["company"]),
                            AssociateComapnyName = (string) DALHelper.HandleDBNull(reader["AssociatCompany"]),
                            Designation = (string) DALHelper.HandleDBNull(reader["designation"]),
                            PatientSource = (string) DALHelper.HandleDBNull(reader["PatientSource"]),
                            TariffName = (string) DALHelper.HandleDBNull(reader["Tariff"]),
                            MemberRelationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MemberRelationID"])),
                            PatientSourceType = Convert.ToInt16(DALHelper.HandleDBNull(reader["PatientSourceType"])),
                            PatientSourceTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceTypeID"])),
                            UnitName = (string) DALHelper.HandleDBNull(reader["UnitName"])
                        };
                        nvo.PatientSponsorDetails.Add(item);
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

        public override IValueObject GetPatientSponsorServiceList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientSponsorServiceListBizActionVO nvo = valueObject as clsGetPatientSponsorServiceListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientSponsorServiceDetails");
                this.dbServer.AddInParameter(storedProcCommand, "SponsorID", DbType.Int64, nvo.SponsorID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DetailsList == null)
                    {
                        nvo.DetailsList = new List<clsPatientSponsorServiceDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientSponsorServiceDetailsVO item = new clsPatientSponsorServiceDetailsVO {
                            SponsorID = (long) DALHelper.HandleDBNull(reader["SponsorID"]),
                            ServiceID = (long) DALHelper.HandleDBNull(reader["ServiceID"]),
                            ServiceName = (string) DALHelper.HandleDBNull(reader["ServiceName"]),
                            DeductibleAmount = new decimal?((decimal) DALHelper.HandleDBNull(reader["DeductionAmount"])),
                            DeductionPercentage = new double?((double) DALHelper.HandleDBNull(reader["DeductionPercentage"]))
                        };
                        nvo.DetailsList.Add(item);
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

        public override IValueObject GetPatientSponsorTariffList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientSponsorTariffListBizActionVO nvo = (clsGetPatientSponsorTariffListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientSponsorTariffDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientCompanyID", DbType.Int64, nvo.PatientCompanyID);
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

        public override IValueObject GetSelectedPackageInfoList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPackageInfoListBizActionVO nvo = (clsGetPatientPackageInfoListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientPackageDetailsForCS");
                this.dbServer.AddInParameter(storedProcCommand, "PackageBillID", DbType.Int64, nvo.PackageBillID);
                this.dbServer.AddInParameter(storedProcCommand, "PackageBillUnitID", DbType.Int64, nvo.PackageBillUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem(Convert.ToInt64(reader["PackageID"]), reader["PackageName"].ToString(), Convert.ToInt64(reader["PatientInfoID"]), Convert.ToBoolean(reader["ApplicableToAll"]), Convert.ToDouble(reader["ApplicableToAllDiscount"]), Convert.ToDouble(reader["PharmacyFixedRate"]), Convert.ToInt64(reader["PackageBillID"]), Convert.ToInt64(reader["PackageBillUnitID"]), Convert.ToInt64(reader["ChargeID"]), Convert.ToDouble(Convert.ToDecimal(reader["PackageConsumptionAmount"])), Convert.ToDouble(reader["OPDConsumption"]), Convert.ToDouble(reader["OPDExcludeServices"]), Convert.ToDouble(reader["TotalPackageAdvance"]), Convert.ToDouble(reader["PharmacyConsumeAmount"]), Convert.ToDouble(reader["PackageConsumableLimit"]), Convert.ToDouble(reader["ConsumableServicesBilled"]), Convert.ToDouble(reader["PackageClinicalTotal"])));
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        private clsAddPatientSponsorBizActionVO UpdatePatientSponsorDetails(clsAddPatientSponsorBizActionVO BizActionObj)
        {
            try
            {
                clsPatientSponsorVO patientSponsorDetails = BizActionObj.PatientSponsorDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePatientSponsor");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, patientSponsorDetails.LinkServer);
                if (patientSponsorDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, patientSponsorDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, patientSponsorDetails.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, patientSponsorDetails.PatientUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientSourceID", DbType.Int64, patientSponsorDetails.PatientSourceID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientCategoryID", DbType.Int64, patientSponsorDetails.PatientCategoryID);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyID", DbType.Int64, patientSponsorDetails.CompanyID);
                this.dbServer.AddInParameter(storedProcCommand, "AssociatedCompanyID", DbType.Int64, patientSponsorDetails.AssociatedCompanyID);
                if (patientSponsorDetails.ReferenceNo != null)
                {
                    patientSponsorDetails.ReferenceNo = patientSponsorDetails.ReferenceNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ReferenceNo", DbType.String, patientSponsorDetails.ReferenceNo);
                this.dbServer.AddInParameter(storedProcCommand, "CreditLimit", DbType.Double, patientSponsorDetails.CreditLimit);
                this.dbServer.AddInParameter(storedProcCommand, "EffectiveDate", DbType.DateTime, patientSponsorDetails.EffectiveDate);
                this.dbServer.AddInParameter(storedProcCommand, "ExpiryDate", DbType.DateTime, patientSponsorDetails.ExpiryDate);
                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, patientSponsorDetails.TariffID);
                if (patientSponsorDetails.EmployeeNo != null)
                {
                    patientSponsorDetails.EmployeeNo = patientSponsorDetails.EmployeeNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "EmployeeNo", DbType.String, patientSponsorDetails.EmployeeNo);
                this.dbServer.AddInParameter(storedProcCommand, "DesignationID", DbType.Int64, patientSponsorDetails.DesignationID);
                if (patientSponsorDetails.Remark != null)
                {
                    patientSponsorDetails.Remark = patientSponsorDetails.Remark.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, patientSponsorDetails.Remark);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, patientSponsorDetails.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, patientSponsorDetails.UpdatedUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, patientSponsorDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, patientSponsorDetails.UpdatedBy);
                if (patientSponsorDetails.UpdatedOn != null)
                {
                    patientSponsorDetails.UpdatedOn = patientSponsorDetails.UpdatedOn.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, patientSponsorDetails.UpdatedOn);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, patientSponsorDetails.UpdatedDateTime);
                if (patientSponsorDetails.UpdatedWindowsLoginName != null)
                {
                    patientSponsorDetails.UpdatedWindowsLoginName = patientSponsorDetails.UpdatedWindowsLoginName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, patientSponsorDetails.UpdatedWindowsLoginName);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, patientSponsorDetails.ID);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }
    }
}

