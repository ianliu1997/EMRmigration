namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Administration
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration;
    using PalashDynamics.ValueObjects.Inventory;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    internal class clsMasterEntryDAL : clsBaseMasterEntryDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsMasterEntryDAL()
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

        public override IValueObject AddUpdateCashCounterDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpadateCashCounterBizActionVO nvo = valueObject as clsAddUpadateCashCounterBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsCashCounterVO objCashCounter = nvo.ObjCashCounter;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateCashCounterMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, objCashCounter.Id);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, objCashCounter.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, objCashCounter.Description);
                this.dbServer.AddInParameter(storedProcCommand, "ClinicID", DbType.Int64, objCashCounter.ClinicId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, objCashCounter.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int32, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int32, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "Synchronized", DbType.Boolean, false);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, nvo.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
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

        public override IValueObject AddUpdateCityDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpadateCityBizActionVO nvo = valueObject as clsAddUpadateCityBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsCityVO objCity = nvo.ObjCity;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateCityMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, objCity.Id);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, objCity.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, objCity.Description);
                this.dbServer.AddInParameter(storedProcCommand, "StateId", DbType.Int64, objCity.StateId);
                this.dbServer.AddInParameter(storedProcCommand, "CountryId", DbType.Int64, objCity.CountryId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, objCity.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int32, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int32, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, nvo.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
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

        public override IValueObject AddUpdateCountryDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpadateCountryBizActionVO nvo = valueObject as clsAddUpadateCountryBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsCountryVO objCountry = nvo.ObjCountry;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateCountryMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, objCountry.Id);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, objCountry.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, objCountry.Description);
                this.dbServer.AddInParameter(storedProcCommand, "Nationality", DbType.String, objCountry.Nationality);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, objCountry.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int32, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int32, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, nvo.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
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

        public override IValueObject AddUpdatePriffixMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePreffixMasterBizActionVO nvo = valueObject as clsAddUpdatePreffixMasterBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsPreffixMasterVO rvo = nvo.PreffixMasterDetails[0];
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePreffixMasterMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, rvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, rvo.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, rvo.Description);
                this.dbServer.AddInParameter(storedProcCommand, "ApplicableGender", DbType.Int64, rvo.ApplicableGender);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, rvo.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int32, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int32, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "Synchronized", DbType.Boolean, false);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, nvo.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
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

        public override IValueObject AddUpdateRegionDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpadateRegionBizActionVO nvo = valueObject as clsAddUpadateRegionBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsRegionVO objRegion = nvo.ObjRegion;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateRegionMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, objRegion.Id);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, objRegion.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, objRegion.Description);
                this.dbServer.AddInParameter(storedProcCommand, "CityId", DbType.Int64, objRegion.CityId);
                this.dbServer.AddInParameter(storedProcCommand, "StateId", DbType.Int64, objRegion.StateId);
                this.dbServer.AddInParameter(storedProcCommand, "CountryId", DbType.Int64, objRegion.CountryId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, objRegion.Status);
                this.dbServer.AddInParameter(storedProcCommand, "PinCode", DbType.String, objRegion.PinCode);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int32, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int32, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, nvo.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
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

        public override IValueObject AddUpdateStateDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpadateStateBizActionVO nvo = valueObject as clsAddUpadateStateBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsStateVO objState = nvo.ObjState;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateStateMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, objState.Id);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, objState.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, objState.Description);
                this.dbServer.AddInParameter(storedProcCommand, "CountryID", DbType.Int64, objState.CountryId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, objState.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int32, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int32, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, nvo.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
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

        public override IValueObject GetAllItemListByMoluculeID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetItemListByMoluculeIdBizActionVO nvo = (clsGetItemListByMoluculeIdBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemListByMoluculeId");
                if (nvo.MoluculeId > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MoluculeId", DbType.Int64, nvo.MoluculeId);
                }
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["ItemName"].ToString()));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetBdMasterList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsgetBdMasterBizActionVO nvo = valueObject as clsgetBdMasterBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillBdMasterCombobox");
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterListItem == null)
                    {
                        nvo.MasterListItem = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterListItem.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString()));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetCashCounterDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCashCounterDetailsBizActionVO nvo = valueObject as clsGetCashCounterDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCashCounterMaster");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, nvo.StateId);
                if ((nvo.Code != null) && (nvo.Code.Length > 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, nvo.Code);
                }
                if ((nvo.Description != null) && (nvo.Description.Length > 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.Description);
                }
                if (nvo.ClinicId > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ClinicId", DbType.Int64, nvo.ClinicId);
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, null);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ListCashCounterDetails == null)
                    {
                        nvo.ListCashCounterDetails = new List<clsCashCounterVO>();
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
                        clsCashCounterVO item = new clsCashCounterVO {
                            Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            ClinicId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ClinicID"])),
                            ClinicName = Convert.ToString(DALHelper.HandleDBNull(reader["ClinicName"])),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.ListCashCounterDetails.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetCashCounterDetailsListByClinicID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCashCounterDetailsByClinicIDBizActionVO nvo = valueObject as clsGetCashCounterDetailsByClinicIDBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCashCounterListByClinicID");
                if (nvo.ClinicID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ClinicID", DbType.Int64, nvo.ClinicID);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ClinicID", DbType.Int64, null);
                }
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ListCashCODetails == null)
                    {
                        nvo.ListCashCODetails = new List<clsCashCounterVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        clsCashCounterVO item = new clsCashCounterVO {
                            Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]))
                        };
                        nvo.ListCashCODetails.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetCityDetailsByStateIDList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCityDetailsByStateIDBizActionVO nvo = valueObject as clsGetCityDetailsByStateIDBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCityMasterListByStateID");
                if (nvo.StateId > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "StateId", DbType.Int64, nvo.StateId);
                }
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ListCityDetails == null)
                    {
                        nvo.ListCityDetails = new List<clsCityVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        clsCityVO item = new clsCityVO {
                            Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]))
                        };
                        nvo.ListCityDetails.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetCityDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCityDetailsBizActionVO nvo = valueObject as clsGetCityDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCityMasterList");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, nvo.CityId);
                if ((nvo.Code != null) && (nvo.Code.Length > 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, nvo.Code);
                }
                if ((nvo.Description != null) && (nvo.Description.Length > 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.Description);
                }
                if (nvo.CountryId > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CountryId", DbType.Int64, nvo.CountryId);
                }
                if (nvo.StateId > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "StateId", DbType.Int64, nvo.StateId);
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, null);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ListCityDetails == null)
                    {
                        nvo.ListCityDetails = new List<clsCityVO>();
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
                        clsCityVO item = new clsCityVO {
                            Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            StateId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["StateId"])),
                            CountryId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["CountryId"])),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            CountryName = Convert.ToString(DALHelper.HandleDBNull(reader["CountryName"])),
                            StateName = Convert.ToString(DALHelper.HandleDBNull(reader["StateName"]))
                        };
                        nvo.ListCityDetails.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetCountryDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCountryDetailsBizActionVO nvo = valueObject as clsGetCountryDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCountryMaster");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, nvo.CountryId);
                if ((nvo.Code != null) && (nvo.Code.Length > 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, nvo.Code);
                }
                if ((nvo.Description != null) && (nvo.Description.Length > 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.Description);
                }
                if ((nvo.Nationality != null) && (nvo.Nationality.Length > 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Nationality", DbType.String, nvo.Nationality);
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, null);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ListCountryDetails == null)
                    {
                        nvo.ListCountryDetails = new List<clsCountryVO>();
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
                        clsCountryVO item = new clsCountryVO {
                            Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Nationality = Convert.ToString(DALHelper.HandleDBNull(reader["Nationality"])),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.ListCountryDetails.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPriffixMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPreffixMasterBizActionVO nvo = valueObject as clsGetPreffixMasterBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPreffixMaster");
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, nvo.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.Description);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PreffixMasterDetails == null)
                    {
                        nvo.PreffixMasterDetails = new List<clsPreffixMasterVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                            break;
                        }
                        clsPreffixMasterVO item = new clsPreffixMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            GetApplicableGender = Convert.ToString(DALHelper.HandleDBNull(reader["ApplicableGender"])),
                            ApplicableGender = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApplicableGenderID"])),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.PreffixMasterDetails.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetRegionDetailsByCityIDList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRegionDetailsByCityIDBizActionVO nvo = valueObject as clsGetRegionDetailsByCityIDBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetRegionMasterListByCityID");
                if (nvo.CityId > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CityId", DbType.Int64, nvo.CityId);
                }
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ListRegionDetails == null)
                    {
                        nvo.ListRegionDetails = new List<clsRegionVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        clsRegionVO item = new clsRegionVO {
                            Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            PinCode = Convert.ToString(DALHelper.HandleDBNull(reader["PinCode"]))
                        };
                        nvo.ListRegionDetails.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetRegionDetailsByCityIDListForReg(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRegionDetailsByCityIDForRegBizActionVO nvo = valueObject as clsGetRegionDetailsByCityIDForRegBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetRegionMasterListByCityID");
                if (nvo.CityId > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CityId", DbType.Int64, nvo.CityId);
                }
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ListRegionDetails == null)
                    {
                        nvo.ListRegionDetails = new List<clsRegionForRegVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        clsRegionForRegVO item = new clsRegionForRegVO {
                            Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            PinCode = Convert.ToString(DALHelper.HandleDBNull(reader["PinCode"]))
                        };
                        nvo.ListRegionDetails.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetRegionDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRegionDetailsBizActionVO nvo = valueObject as clsGetRegionDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetRegionMasterList");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, nvo.CountryId);
                if ((nvo.Code != null) && (nvo.Code.Length > 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, nvo.Code);
                }
                if ((nvo.Description != null) && (nvo.Description.Length > 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.Description);
                }
                if (nvo.CountryId > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CountryId", DbType.Int64, nvo.CountryId);
                }
                if (nvo.StateId > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "StateId", DbType.Int64, nvo.StateId);
                }
                if (nvo.CityId > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CityId", DbType.Int64, nvo.CityId);
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, null);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ListRegionDetails == null)
                    {
                        nvo.ListRegionDetails = new List<clsRegionVO>();
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
                        clsRegionVO item = new clsRegionVO {
                            Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            CityId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["CityId"])),
                            StateId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["StateId"])),
                            CountryId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["CountryId"])),
                            PinCode = Convert.ToString(DALHelper.HandleDBNull(reader["PinCode"])),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            CountryName = Convert.ToString(DALHelper.HandleDBNull(reader["CountryName"])),
                            StateName = Convert.ToString(DALHelper.HandleDBNull(reader["StateName"])),
                            CityName = Convert.ToString(DALHelper.HandleDBNull(reader["CityName"]))
                        };
                        nvo.ListRegionDetails.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetStateDetailsByCountryIDList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetStateDetailsByCountyIDBizActionVO nvo = valueObject as clsGetStateDetailsByCountyIDBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStateMasterListByCountryID");
                if (nvo.CountryId > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CountryID", DbType.Int64, nvo.CountryId);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CountryID", DbType.Int64, null);
                }
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ListStateDetails == null)
                    {
                        nvo.ListStateDetails = new List<clsStateVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        clsStateVO item = new clsStateVO {
                            Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]))
                        };
                        nvo.ListStateDetails.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetStateDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetStateDetailsBizActionVO nvo = valueObject as clsGetStateDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStateMaster");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, nvo.StateId);
                if ((nvo.Code != null) && (nvo.Code.Length > 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, nvo.Code);
                }
                if ((nvo.Description != null) && (nvo.Description.Length > 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.Description);
                }
                if (nvo.CountryId > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CountryID", DbType.Int64, nvo.CountryId);
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, null);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ListStateDetails == null)
                    {
                        nvo.ListStateDetails = new List<clsStateVO>();
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
                        clsStateVO item = new clsStateVO {
                            Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            CountryId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["CountryID"])),
                            CountryName = Convert.ToString(DALHelper.HandleDBNull(reader["CountryName"])),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.ListStateDetails.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }
    }
}

