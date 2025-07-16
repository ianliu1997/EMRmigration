namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration.PatientSourceMaster;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsPatientSourceMasterDAL : clsBasePatientSourceMasterMasterDAL
    {
        private Database dbServer;

        public clsPatientSourceMasterDAL()
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

        private clsAddPatientSourceBizActionVO AddItemGroupMaster(clsAddPatientSourceBizActionVO BizActionObj, clsUserVO objUserVO)
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
                clsPatientSourceVO patientDetails = BizActionObj.PatientDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddItemGroupMaster");
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, patientDetails.Code.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, patientDetails.Description.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "GeneralLedgerID", DbType.Int64, patientDetails.PatientCatagoryID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, patientDetails.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.PatientDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                transaction.Commit();
            }
            catch (Exception)
            {
                BizActionObj.ResultSuccessStatus = -1L;
                transaction.Rollback();
                BizActionObj.PatientDetails = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return BizActionObj;
        }

        private clsAddPatientSourceBizActionVO AddPatientSourceMaster(clsAddPatientSourceBizActionVO BizActionObj, clsUserVO objUserVO)
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
                clsPatientSourceVO patientDetails = BizActionObj.PatientDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatientSource");
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, patientDetails.Code.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, patientDetails.Description.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "PatientCatagoryID", DbType.Int64, patientDetails.PatientCatagoryID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, patientDetails.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.PatientDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                transaction.Commit();
            }
            catch (Exception)
            {
                BizActionObj.ResultSuccessStatus = -1L;
                transaction.Rollback();
                BizActionObj.PatientDetails = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return BizActionObj;
        }

        public override IValueObject AddPatientSourceMaster(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddPatientSourceBizActionVO bizActionObj = valueObject as clsAddPatientSourceBizActionVO;
            return (!bizActionObj.IsFromItemGroupMaster ? ((bizActionObj.PatientDetails.ID != 0L) ? this.UpdatePatientSourceMaster(bizActionObj, objUserVO) : this.AddPatientSourceMaster(bizActionObj, objUserVO)) : ((bizActionObj.PatientDetails.ID != 0L) ? this.UpdateItemGroupMaster(bizActionObj, objUserVO) : this.AddItemGroupMaster(bizActionObj, objUserVO)));
        }

        public override IValueObject GetItemGroupMasterList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPatientSourceListBizActionVO nvo = (clsGetPatientSourceListBizActionVO) valueObject;
            try
            {
                if (!nvo.ValidPatientMasterSourceList)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemGroupMaster");
                    this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                    this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                    this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.PatientSourceDetails == null)
                        {
                            nvo.PatientSourceDetails = new List<clsPatientSourceVO>();
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
                            clsPatientSourceVO item = new clsPatientSourceVO {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                                Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                                Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                                PatientCategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["GeneralLedgerName"])),
                                PatientCatagoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GeneralLedgerID"]))
                            };
                            nvo.PatientSourceDetails.Add(item);
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

        public override IValueObject GetPatientSourceByID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPatientSourceDetailsByIDBizActionVO nvo = (clsGetPatientSourceDetailsByIDBizActionVO) valueObject;
            try
            {
                clsPatientSourceVO details = nvo.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientSourceDetailsByID");
                this.dbServer.AddInParameter(storedProcCommand, "PatientSourceID", DbType.Int64, nvo.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (nvo.Details == null)
                        {
                            nvo.Details = new clsPatientSourceVO();
                        }
                        nvo.Details.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.Details.Code = (string) DALHelper.HandleDBNull(reader["Code"]);
                        nvo.Details.Description = (string) DALHelper.HandleDBNull(reader["Description"]);
                        nvo.Details.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                        nvo.Details.PatientCatagoryID = (long) DALHelper.HandleDBNull(reader["PatientCategoryId"]);
                        nvo.Details.FromDate = DALHelper.HandleDate(reader["FromDate"]);
                        nvo.Details.ToDate = DALHelper.HandleDate(reader["ToDate"]);
                        nvo.Details.PatientSourceType = (short) DALHelper.HandleDBNull(reader["PatientSourceType"]);
                        nvo.Details.PatientSourceTypeID = (long) DALHelper.HandleDBNull(reader["PatientSourceTypeID"]);
                    }
                }
                reader.NextResult();
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPatientSourceList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPatientSourceListBizActionVO nvo = (clsGetPatientSourceListBizActionVO) valueObject;
            try
            {
                if (nvo.ValidPatientMasterSourceList)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetValidPatientSourceList");
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.String, nvo.ID);
                    DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader2.HasRows)
                    {
                        if (nvo.MasterList == null)
                        {
                            nvo.MasterList = new List<MasterListItem>();
                        }
                        while (true)
                        {
                            if (!reader2.Read())
                            {
                                reader2.Close();
                                break;
                            }
                            MasterListItem item = new MasterListItem {
                                ID = (long) reader2["ID"],
                                Code = reader2["Code"].ToString(),
                                Description = reader2["Description"].ToString(),
                                Status = (bool) reader2["Status"]
                            };
                            nvo.MasterList.Add(item);
                        }
                    }
                }
                else
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientSourceList");
                    this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                    this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientSourceType", DbType.String, nvo.FilterPatientSourceType);
                    this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.PatientSourceDetails == null)
                        {
                            nvo.PatientSourceDetails = new List<clsPatientSourceVO>();
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
                            clsPatientSourceVO item = new clsPatientSourceVO {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                                PatientCategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientCategoryName"])),
                                Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                                Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                                PatientCatagoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientCategoryID"])),
                                PatientSourceType = Convert.ToInt16(DALHelper.HandleDBNull(reader["PatientSourceType"])),
                                PatientSourceTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceTypeID"]))
                            };
                            nvo.PatientSourceDetails.Add(item);
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

        public override IValueObject GetPatientSourceListByPatientCategoryId(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPatientSourceListByPatientCategoryIdBizActionVO nvo = (clsGetPatientSourceListByPatientCategoryIdBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientSourceListByPatientCategoryId");
                this.dbServer.AddInParameter(storedProcCommand, "PatientCategoryId", DbType.Int64, nvo.SelectedPatientCategoryIdID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientSourceDetails == null)
                    {
                        nvo.PatientSourceDetails = new List<clsPatientSourceVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        clsPatientSourceVO item = new clsPatientSourceVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            PatientSourceType = Convert.ToInt16(DALHelper.HandleDBNull(reader["PatientSourceType"])),
                            PatientSourceTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceTypeID"]))
                        };
                        nvo.PatientSourceDetails.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetTariffList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetTariffDetailsListBizActionVO nvo = (clsGetTariffDetailsListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetTariffList");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientSourceDetails == null)
                    {
                        nvo.PatientSourceDetails = new List<clsTariffDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsTariffDetailsVO item = new clsTariffDetailsVO {
                            TariffID = (long) reader["ID"],
                            TariffCode = reader["Code"].ToString(),
                            TariffDescription = reader["Description"].ToString(),
                            Status = (bool) reader["Status"],
                            IsFamily = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFamily"]))
                        };
                        nvo.PatientSourceDetails.Add(item);
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

        public override IValueObject GetTariffListForCompMaster(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetTariffDetailsListBizActionVO nvo = (clsGetTariffDetailsListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetTariffListForCompanyMaster");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, null);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientSourceDetails == null)
                    {
                        nvo.PatientSourceDetails = new List<clsTariffDetailsVO>();
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
                        clsTariffDetailsVO item = new clsTariffDetailsVO {
                            TariffID = (long) reader["ID"],
                            TariffCode = reader["Code"].ToString(),
                            TariffDescription = reader["Description"].ToString(),
                            Status = (bool) reader["Status"],
                            IsFamily = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFamily"]))
                        };
                        nvo.PatientSourceDetails.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        private clsAddPatientSourceBizActionVO UpdateItemGroupMaster(clsAddPatientSourceBizActionVO BizActionObj, clsUserVO objUserVO)
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
                clsPatientSourceVO patientDetails = BizActionObj.PatientDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateItemGroupMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, patientDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, patientDetails.Code.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, patientDetails.Description.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, patientDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "@GeneralLedgerID", DbType.Int64, patientDetails.PatientCatagoryID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, patientDetails.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                transaction.Commit();
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.PatientDetails = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return BizActionObj;
        }

        private clsAddPatientSourceBizActionVO UpdatePatientSourceMaster(clsAddPatientSourceBizActionVO BizActionObj, clsUserVO objUserVO)
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
                clsPatientSourceVO patientDetails = BizActionObj.PatientDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePatientSource");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, patientDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, patientDetails.Code.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, patientDetails.Description.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, patientDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "PatientCatagoryID", DbType.Int64, patientDetails.PatientCatagoryID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, patientDetails.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                transaction.Commit();
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.PatientDetails = null;
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

