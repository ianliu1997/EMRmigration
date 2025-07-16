using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration;
using System.Data.Common;
using System.Data;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Administration
{
    class clsMasterEntryDAL : clsBaseMasterEntryDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsMasterEntryDAL()
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
                #endregion

            }
            catch (Exception)
            {

                throw;
            }
        }

        public override IValueObject AddUpdateCityDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpadateCityBizActionVO objBizAction = valueObject as clsAddUpadateCityBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State != ConnectionState.Open) con.Open();
                trans = con.BeginTransaction();

                int intStatus = 0;
                clsCityVO objCityVO = objBizAction.ObjCity;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateCityMaster");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objCityVO.Id);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Code", DbType.String, objCityVO.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, objCityVO.Description);
                dbServer.AddInParameter(command, "StateId", DbType.Int64, objCityVO.StateId);
                dbServer.AddInParameter(command, "CountryId", DbType.Int64, objCityVO.CountryId);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objCityVO.Status);


                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int32, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int32, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, objBizAction.SuccessStatus);

                intStatus = dbServer.ExecuteNonQuery(command, trans);
                objBizAction.SuccessStatus = Convert.ToInt32(dbServer.GetParameterValue(command, "ResultStatus"));
                //objBizAction.SuccessStatus = Convert.ToInt32(dbServer.GetParameterValue(command, "ID"));
                //if (intStatus == 0)
                //    throw new Exception();
                ///////////////////////////////////////  
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
                con.Dispose();
                trans.Dispose();
            }
            return objBizAction;

        }
        public override IValueObject GetBdMasterList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsgetBdMasterBizActionVO BizActionObj = valueObject as clsgetBdMasterBizActionVO;


            try
            {
               

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_FillBdMasterCombobox");
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizActionObj.UnitID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.MasterListItem == null)
                    {
                        BizActionObj.MasterListItem = new List<MasterListItem>();
                    }
                   
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        BizActionObj.MasterListItem.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString()));
                    }
                }
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
        public override IValueObject GetCityDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCityDetailsBizActionVO BizActionObj = valueObject as clsGetCityDetailsBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCityMasterList");
                dbServer.AddInParameter(command, "Id", DbType.Int64, BizActionObj.CityId);
                if (BizActionObj.Code != null && BizActionObj.Code.Length > 0)
                    dbServer.AddInParameter(command, "Code", DbType.String, BizActionObj.Code);
                if (BizActionObj.Description != null && BizActionObj.Description.Length > 0)
                    dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.Description);
                if (BizActionObj.CountryId > 0)
                    dbServer.AddInParameter(command, "CountryId", DbType.Int64, BizActionObj.CountryId);
                if (BizActionObj.StateId > 0)
                    dbServer.AddInParameter(command, "StateId", DbType.Int64, BizActionObj.StateId);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, null);

                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ListCityDetails == null)
                        BizActionObj.ListCityDetails = new List<clsCityVO>();
                    while (reader.Read())
                    {
                        clsCityVO objCityVO = new clsCityVO();
                        objCityVO.Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        objCityVO.UnitId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"]));
                        objCityVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objCityVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objCityVO.StateId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["StateId"]));
                        objCityVO.CountryId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["CountryId"]));
                        objCityVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        objCityVO.CountryName = Convert.ToString(DALHelper.HandleDBNull(reader["CountryName"]));
                        objCityVO.StateName = Convert.ToString(DALHelper.HandleDBNull(reader["StateName"]));
                        BizActionObj.ListCityDetails.Add(objCityVO);
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    //var aa = dbServer.GetParameterValue(command, "TotalRows");
                    //BizActionObj.TotalRows = Convert.ToInt64(aa);
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetStateDetailsByCountryIDList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetStateDetailsByCountyIDBizActionVO BizActionObj = valueObject as clsGetStateDetailsByCountyIDBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetStateMasterListByCountryID");
                if (BizActionObj.CountryId > 0)
                    dbServer.AddInParameter(command, "CountryID", DbType.Int64, BizActionObj.CountryId);
                else
                    dbServer.AddInParameter(command, "CountryID", DbType.Int64, null);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ListStateDetails == null)
                        BizActionObj.ListStateDetails = new List<clsStateVO>();
                    while (reader.Read())
                    {
                        clsStateVO objStateVO = new clsStateVO();
                        objStateVO.Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        //objStateVO.UnitId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"]));
                        //objStateVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objStateVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        BizActionObj.ListStateDetails.Add(objStateVO);
                    }

                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetCityDetailsByStateIDList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCityDetailsByStateIDBizActionVO BizActionObj = valueObject as clsGetCityDetailsByStateIDBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCityMasterListByStateID");
                if (BizActionObj.StateId > 0)
                    dbServer.AddInParameter(command, "StateId", DbType.Int64, BizActionObj.StateId);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ListCityDetails == null)
                        BizActionObj.ListCityDetails = new List<clsCityVO>();
                    while (reader.Read())
                    {
                        clsCityVO objCityVO = new clsCityVO();
                        objCityVO.Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        //objCityVO.UnitId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"]));
                        //objCityVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objCityVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        BizActionObj.ListCityDetails.Add(objCityVO);
                    }

                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetRegionDetailsByCityIDList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRegionDetailsByCityIDBizActionVO BizActionObj = valueObject as clsGetRegionDetailsByCityIDBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetRegionMasterListByCityID");
                if (BizActionObj.CityId > 0)
                    dbServer.AddInParameter(command, "CityId", DbType.Int64, BizActionObj.CityId);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ListRegionDetails == null)
                        BizActionObj.ListRegionDetails = new List<clsRegionVO>();
                    while (reader.Read())
                    {
                        clsRegionVO objRegionVO = new clsRegionVO();
                        objRegionVO.Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        //objRegionVO.UnitId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"]));
                        //objRegionVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objRegionVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objRegionVO.PinCode = Convert.ToString(DALHelper.HandleDBNull(reader["PinCode"]));
                        BizActionObj.ListRegionDetails.Add(objRegionVO);
                    }
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject AddUpdateStateDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpadateStateBizActionVO objBizAction = valueObject as clsAddUpadateStateBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State != ConnectionState.Open) con.Open();
                trans = con.BeginTransaction();

                int intStatus = 0;
                clsStateVO objStateVO = objBizAction.ObjState;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateStateMaster");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objStateVO.Id);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Code", DbType.String, objStateVO.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, objStateVO.Description);
                dbServer.AddInParameter(command, "CountryID", DbType.Int64, objStateVO.CountryId);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objStateVO.Status);

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int32, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int32, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, objBizAction.SuccessStatus);

                intStatus = dbServer.ExecuteNonQuery(command, trans);
                objBizAction.SuccessStatus = Convert.ToInt32(dbServer.GetParameterValue(command, "ResultStatus"));
                //objBizAction.SuccessStatus = Convert.ToInt32(dbServer.GetParameterValue(command, "ID"));
                //if (intStatus == 0)
                //    throw new Exception();
                ///////////////////////////////////////  
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
                con.Dispose();
                trans.Dispose();
            }
            return objBizAction;
        }

        public override IValueObject GetStateDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetStateDetailsBizActionVO BizActionObj = valueObject as clsGetStateDetailsBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetStateMaster");
                dbServer.AddInParameter(command, "Id", DbType.Int64, BizActionObj.StateId);
                if (BizActionObj.Code != null && BizActionObj.Code.Length > 0)
                    dbServer.AddInParameter(command, "Code", DbType.String, BizActionObj.Code);
                if (BizActionObj.Description != null && BizActionObj.Description.Length > 0)
                    dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.Description);
                if (BizActionObj.CountryId > 0)
                    dbServer.AddInParameter(command, "CountryID", DbType.Int64, BizActionObj.CountryId);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, null);

                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ListStateDetails == null)
                        BizActionObj.ListStateDetails = new List<clsStateVO>();
                    while (reader.Read())
                    {
                        clsStateVO objStateVO = new clsStateVO();
                        objStateVO.Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        objStateVO.UnitId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"]));
                        objStateVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objStateVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objStateVO.CountryId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["CountryID"]));
                        objStateVO.CountryName = Convert.ToString(DALHelper.HandleDBNull(reader["CountryName"]));
                        objStateVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        BizActionObj.ListStateDetails.Add(objStateVO);
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject AddUpdateCountryDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpadateCountryBizActionVO objBizAction = valueObject as clsAddUpadateCountryBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State != ConnectionState.Open) con.Open();
                trans = con.BeginTransaction();

                int intStatus = 0;
                clsCountryVO objCountryVO = objBizAction.ObjCountry;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateCountryMaster");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objCountryVO.Id);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Code", DbType.String, objCountryVO.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, objCountryVO.Description);

                dbServer.AddInParameter(command, "Nationality", DbType.String, objCountryVO.Nationality);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objCountryVO.Status);

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int32, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int32, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, objBizAction.SuccessStatus);

                intStatus = dbServer.ExecuteNonQuery(command, trans);
                objBizAction.SuccessStatus = Convert.ToInt32(dbServer.GetParameterValue(command, "ResultStatus"));
                //objBizAction.SuccessStatus = Convert.ToInt32(dbServer.GetParameterValue(command, "ID"));
                //if (intStatus == 0)
                //    throw new Exception();
                ///////////////////////////////////////  
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
                con.Dispose();
                trans.Dispose();
            }
            return objBizAction;

        }

        public override IValueObject GetCountryDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCountryDetailsBizActionVO BizActionObj = valueObject as clsGetCountryDetailsBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCountryMaster");
                dbServer.AddInParameter(command, "Id", DbType.Int64, BizActionObj.CountryId);
                if (BizActionObj.Code != null && BizActionObj.Code.Length > 0)
                    dbServer.AddInParameter(command, "Code", DbType.String, BizActionObj.Code);
                if (BizActionObj.Description != null && BizActionObj.Description.Length > 0)
                    dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.Description);
                if (BizActionObj.Nationality != null && BizActionObj.Nationality.Length > 0)
                    dbServer.AddInParameter(command, "Nationality", DbType.String, BizActionObj.Nationality);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, null);

                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ListCountryDetails == null)
                        BizActionObj.ListCountryDetails = new List<clsCountryVO>();
                    while (reader.Read())
                    {
                        clsCountryVO objStateVO = new clsCountryVO();
                        objStateVO.Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        objStateVO.UnitId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"]));
                        objStateVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objStateVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objStateVO.Nationality = Convert.ToString(DALHelper.HandleDBNull(reader["Nationality"]));
                        objStateVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        BizActionObj.ListCountryDetails.Add(objStateVO);
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject AddUpdateRegionDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpadateRegionBizActionVO objBizAction = valueObject as clsAddUpadateRegionBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State != ConnectionState.Open) con.Open();
                trans = con.BeginTransaction();

                int intStatus = 0;
                clsRegionVO objRegionVO = objBizAction.ObjRegion;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateRegionMaster");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objRegionVO.Id);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Code", DbType.String, objRegionVO.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, objRegionVO.Description);
                dbServer.AddInParameter(command, "CityId", DbType.Int64, objRegionVO.CityId);
                dbServer.AddInParameter(command, "StateId", DbType.Int64, objRegionVO.StateId);
                dbServer.AddInParameter(command, "CountryId", DbType.Int64, objRegionVO.CountryId);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objRegionVO.Status);
                dbServer.AddInParameter(command, "PinCode", DbType.String, objRegionVO.PinCode);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int32, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int32, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, objBizAction.SuccessStatus);

                intStatus = dbServer.ExecuteNonQuery(command, trans);
                objBizAction.SuccessStatus = Convert.ToInt32(dbServer.GetParameterValue(command, "ResultStatus"));
                //objBizAction.SuccessStatus = Convert.ToInt32(dbServer.GetParameterValue(command, "ID"));
                //if (intStatus == 0)
                //    throw new Exception();
                ///////////////////////////////////////  
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
                con.Dispose();
                trans.Dispose();
            }
            return objBizAction;

        }

        public override IValueObject GetRegionDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRegionDetailsBizActionVO BizActionObj = valueObject as clsGetRegionDetailsBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetRegionMasterList");
                dbServer.AddInParameter(command, "Id", DbType.Int64, BizActionObj.CountryId);
                if (BizActionObj.Code != null && BizActionObj.Code.Length > 0)
                    dbServer.AddInParameter(command, "Code", DbType.String, BizActionObj.Code);
                if (BizActionObj.Description != null && BizActionObj.Description.Length > 0)
                    dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.Description);
                if (BizActionObj.CountryId > 0)
                    dbServer.AddInParameter(command, "CountryId", DbType.Int64, BizActionObj.CountryId);
                if (BizActionObj.StateId > 0)
                    dbServer.AddInParameter(command, "StateId", DbType.Int64, BizActionObj.StateId);
                if (BizActionObj.CityId > 0)
                    dbServer.AddInParameter(command, "CityId", DbType.Int64, BizActionObj.CityId);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, null);

                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ListRegionDetails == null)
                        BizActionObj.ListRegionDetails = new List<clsRegionVO>();
                    while (reader.Read())
                    {
                        clsRegionVO objRegionVO = new clsRegionVO();
                        objRegionVO.Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        objRegionVO.UnitId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"]));
                        objRegionVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objRegionVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objRegionVO.CityId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["CityId"]));
                        objRegionVO.StateId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["StateId"]));
                        objRegionVO.CountryId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["CountryId"]));
                        objRegionVO.PinCode = Convert.ToString(DALHelper.HandleDBNull(reader["PinCode"]));
                        objRegionVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        objRegionVO.CountryName = Convert.ToString(DALHelper.HandleDBNull(reader["CountryName"]));
                        objRegionVO.StateName = Convert.ToString(DALHelper.HandleDBNull(reader["StateName"]));
                        objRegionVO.CityName = Convert.ToString(DALHelper.HandleDBNull(reader["CityName"]));
                        BizActionObj.ListRegionDetails.Add(objRegionVO);
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetAllItemListByMoluculeID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetItemListByMoluculeIdBizActionVO BizAction = (clsGetItemListByMoluculeIdBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetItemListByMoluculeId");

                if (BizAction.MoluculeId > 0)

                    dbServer.AddInParameter(command, "MoluculeId", DbType.Int64, BizAction.MoluculeId);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    if (BizAction.MasterList == null)
                    {
                        BizAction.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        //BizAction.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString()));
                        BizAction.MasterList.Add(new MasterListItem((long)reader["Id"], reader["ItemName"].ToString()));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizAction;
        }

        //public override IValueObject GetDrugDetailsList(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetDrugMasterDetailsBizActionVO BizActionObj = valueObject as clsGetDrugMasterDetailsBizActionVO;
        //    try
        //    {
        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDrugMaster");
        //        dbServer.AddInParameter(command, "Id", DbType.Int64, BizActionObj.Id);
        //        if (BizActionObj.Code != null && BizActionObj.Code.Length > 0)
        //            dbServer.AddInParameter(command, "Code", DbType.String, BizActionObj.Code);
        //        if (BizActionObj.Description != null && BizActionObj.Description.Length > 0)
        //            dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.Description);
        //        //if (BizActionObj.CategoryID > 0)
        //        dbServer.AddInParameter(command, "CategoryID", DbType.Int64, BizActionObj.CategoryID);

        //        dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
        //        dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
        //        dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
        //        dbServer.AddInParameter(command, "sortExpression", DbType.String, null);

        //        dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);
        //        DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {
        //            if (BizActionObj.ListDrugDetails == null)
        //                BizActionObj.ListDrugDetails = new List<clsDrugsVO>();
        //            while (reader.Read())
        //            {
        //                clsDrugsVO objDrugsVO = new clsDrugsVO();
        //                objDrugsVO.Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
        //                objDrugsVO.UnitId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"]));
        //                objDrugsVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
        //                objDrugsVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                objDrugsVO.MoleculeID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["MoleculeID"]));
        //                //objDrugsVO.MoleculeName = Convert.ToString(DALHelper.HandleDBNull(reader["CategoryName"]));
        //                objDrugsVO.CategoryID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["CategoryID"]));
        //                objDrugsVO.CategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["CategoryName"]));
        //                objDrugsVO.Status = (bool)DALHelper.HandleBoolDBNull(reader["Status"]);
        //                BizActionObj.ListDrugDetails.Add(objDrugsVO);
        //            }
        //            reader.NextResult();
        //            BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
        //            reader.Close();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return BizActionObj;
        //}

        //public override IValueObject UpdateObservationCodeStatus(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsUpdateObservationCodeStatusBizActionVO objBizAction = valueObject as clsUpdateObservationCodeStatusBizActionVO;
        //    DbTransaction trans = null;
        //    DbConnection con = null;
        //    try
        //    {
        //        con = dbServer.CreateConnection();
        //        if (con.State != ConnectionState.Open) con.Open();
        //        trans = con.BeginTransaction();
        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateObservationCodeStatus");

        //        dbServer.AddInParameter(command, "ID", DbType.Int64, objBizAction.ObservationCodeID);
        //        dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "Status", DbType.Boolean, objBizAction.Status);
        //        dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int32, UserVo.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
        //        dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //        dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
        //        dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //        dbServer.AddParameter(command, "ResultStatus", DbType.Int64, ParameterDirection.Output, null, DataRowVersion.Default, objBizAction.SuccessStatus);

        //        int intStatus = dbServer.ExecuteNonQuery(command, trans);
        //        objBizAction.SuccessStatus = (long)dbServer.GetParameterValue(command, "ResultStatus");

        //        trans.Commit();
        //        if (con.State == ConnectionState.Open) con.Close();
        //        command.Dispose();
        //    }
        //    catch (Exception)
        //    {
        //        trans.Rollback();
        //        throw;
        //    }
        //    finally
        //    {
        //        con.Dispose();
        //        trans.Dispose();
        //    }
        //    return objBizAction;
        //}

        //public override IValueObject GetObservationCodeList(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetObservationCodeMasterBizActionVO BizActionObj = valueObject as clsGetObservationCodeMasterBizActionVO;
        //    try
        //    {
        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetObservationCodeMasterList");

        //        if (BizActionObj.ObservationCodeID > 0)
        //            dbServer.AddInParameter(command, "Id", DbType.Int64, BizActionObj.ObservationCodeID);
        //        if (BizActionObj.Code != null && BizActionObj.Code.Length > 0)
        //            dbServer.AddInParameter(command, "Code", DbType.String, BizActionObj.Code);
        //        if (BizActionObj.Description != null && BizActionObj.Description.Length > 0)
        //            dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.Description);
        //        if (BizActionObj.CodeTypeId > 0)
        //            dbServer.AddInParameter(command, "CodeTypeId", DbType.Int64, BizActionObj.CodeTypeId);

        //        dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
        //        dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
        //        dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);


        //        dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
        //        dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);
        //        DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {
        //            if (BizActionObj.ObservationCodeMasterDetails == null)
        //                BizActionObj.ObservationCodeMasterDetails = new List<clsObservationCodeMasterVO>();
        //            while (reader.Read())
        //            {
        //                clsObservationCodeMasterVO objCodeTypeMasterVO = new clsObservationCodeMasterVO();
        //                objCodeTypeMasterVO.ID = (long)DALHelper.HandleIntegerNull(reader["ID"]);
        //                objCodeTypeMasterVO.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
        //                objCodeTypeMasterVO.Status = (bool)DALHelper.HandleBoolDBNull(reader["Status"]);
        //                objCodeTypeMasterVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
        //                objCodeTypeMasterVO.CodeTypeID = (long)DALHelper.HandleIntegerNull(reader["CodeTypeID"]);
        //                objCodeTypeMasterVO.CodeType = (string)DALHelper.HandleDBNull(reader["CodeType"]);

        //                BizActionObj.ObservationCodeMasterDetails.Add(objCodeTypeMasterVO);
        //            }

        //            reader.NextResult();
        //            BizActionObj.TotalRows = Convert.ToInt64(dbServer.GetParameterValue(command, "TotalRows"));
        //            reader.Close();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return BizActionObj;
        //}

        //public override IValueObject AddUpdateObservationCode(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsAddUpdateObservationCodeBizActionVO objBizAction = valueObject as clsAddUpdateObservationCodeBizActionVO;
        //    DbTransaction trans = null;
        //    DbConnection con = null;
        //    try
        //    {
        //        con = dbServer.CreateConnection();
        //        if (con.State != ConnectionState.Open) con.Open();
        //        trans = con.BeginTransaction();
        //        clsObservationCodeMasterVO objCodeTypeMasterVO = objBizAction.ObservationCodeMasterDetails;
        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateObservationCodeMaster");

        //        dbServer.AddInParameter(command, "ID", DbType.Int64, objCodeTypeMasterVO.ID);
        //        dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "Code", DbType.String, objCodeTypeMasterVO.Code);
        //        dbServer.AddInParameter(command, "Description", DbType.String, objCodeTypeMasterVO.Description);
        //        dbServer.AddInParameter(command, "CodeType", DbType.Int64, objCodeTypeMasterVO.CodeTypeID);
        //        dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
        //        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int32, null);
        //        dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int32, UserVo.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
        //        dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
        //        dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
        //        dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //        dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
        //        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, null);
        //        dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //        dbServer.AddParameter(command, "ResultStatus", DbType.Int64, ParameterDirection.Output, null, DataRowVersion.Default, objCodeTypeMasterVO.ResultStatus);

        //        int intStatus = dbServer.ExecuteNonQuery(command, trans);
        //        objBizAction.ResultStatus = (long)dbServer.GetParameterValue(command, "ResultStatus");
        //        objBizAction.SuccessStatus = 0;
        //        trans.Commit();
        //        if (con.State == ConnectionState.Open) con.Close();
        //        command.Dispose();
        //    }
        //    catch (Exception)
        //    {
        //        trans.Rollback();
        //        throw;
        //    }
        //    finally
        //    {
        //        con.Dispose();
        //        trans.Dispose();
        //    }
        //    return objBizAction;
        //}

        //public override IValueObject GetCashCounterUnitMasterList(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetCashCounterUnitMasterBizActionVO BizActionObj = valueObject as clsGetCashCounterUnitMasterBizActionVO;

        //    try
        //    {
        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCashCounterUnitMaster");

        //        dbServer.AddInParameter(command, "CashCounterId", DbType.Int64, BizActionObj.CashCounterId);
        //        dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
        //        dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);
        //        DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {
        //            if (BizActionObj.ListCashCounterUnitMaster == null)
        //                BizActionObj.ListCashCounterUnitMaster = new List<clsCashCounterUnitMasterVO>();
        //            while (reader.Read())
        //            {
        //                clsCashCounterUnitMasterVO objCashCounterMasterVO = new clsCashCounterUnitMasterVO();
        //                objCashCounterMasterVO.ID = (long)DALHelper.HandleIntegerNull(reader["ID"]);

        //                objCashCounterMasterVO.UnitID = (long)DALHelper.HandleIntegerNull(reader["UnitId"]);
        //                objCashCounterMasterVO.CashCounterId = (long)DALHelper.HandleIntegerNull(reader["CashCounterId"]);
        //                objCashCounterMasterVO.Details_UnitId = (long)DALHelper.HandleIntegerNull(reader["Details_UnitId"]);
        //                objCashCounterMasterVO.Status = (bool)DALHelper.HandleBoolDBNull(reader["Status"]);

        //                BizActionObj.ListCashCounterUnitMaster.Add(objCashCounterMasterVO);
        //            }
        //            reader.NextResult();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return BizActionObj;
        //}









        // Added By CDS

        public override IValueObject AddUpdateCashCounterDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpadateCashCounterBizActionVO objBizAction = valueObject as clsAddUpadateCashCounterBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State != ConnectionState.Open) con.Open();
                trans = con.BeginTransaction();

                int intStatus = 0;
                clsCashCounterVO ObjCashCounter = objBizAction.ObjCashCounter;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateCashCounterMaster");
                dbServer.AddInParameter(command, "ID", DbType.Int64, ObjCashCounter.Id);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Code", DbType.String, ObjCashCounter.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, ObjCashCounter.Description);
                dbServer.AddInParameter(command, "ClinicID", DbType.Int64, ObjCashCounter.ClinicId);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, ObjCashCounter.Status);

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int32, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int32, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddInParameter(command, "Synchronized", DbType.Boolean, false);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, objBizAction.SuccessStatus);

                intStatus = dbServer.ExecuteNonQuery(command, trans);
                objBizAction.SuccessStatus = Convert.ToInt32(dbServer.GetParameterValue(command, "ResultStatus"));
                //objBizAction.SuccessStatus = Convert.ToInt32(dbServer.GetParameterValue(command, "ID"));
                //if (intStatus == 0)
                //    throw new Exception();
                ///////////////////////////////////////  
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
                con.Dispose();
                trans.Dispose();
            }
            return objBizAction;
        }

        public override IValueObject GetCashCounterDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCashCounterDetailsBizActionVO BizActionObj = valueObject as clsGetCashCounterDetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCashCounterMaster");
                dbServer.AddInParameter(command, "Id", DbType.Int64, BizActionObj.StateId);
                if (BizActionObj.Code != null && BizActionObj.Code.Length > 0)
                    dbServer.AddInParameter(command, "Code", DbType.String, BizActionObj.Code);
                if (BizActionObj.Description != null && BizActionObj.Description.Length > 0)
                    dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.Description);
                if (BizActionObj.ClinicId > 0)
                    dbServer.AddInParameter(command, "ClinicId", DbType.Int64, BizActionObj.ClinicId);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, null);

                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ListCashCounterDetails == null)
                        BizActionObj.ListCashCounterDetails = new List<clsCashCounterVO>();
                    while (reader.Read())
                    {
                        clsCashCounterVO objStateVO = new clsCashCounterVO();
                        objStateVO.Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        objStateVO.UnitId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"]));
                        objStateVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objStateVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objStateVO.ClinicId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ClinicID"]));
                        objStateVO.ClinicName = Convert.ToString(DALHelper.HandleDBNull(reader["ClinicName"]));
                        objStateVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        BizActionObj.ListCashCounterDetails.Add(objStateVO);
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetCashCounterDetailsListByClinicID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCashCounterDetailsByClinicIDBizActionVO BizActionObj = valueObject as clsGetCashCounterDetailsByClinicIDBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCashCounterListByClinicID");
                if (BizActionObj.ClinicID > 0)
                    dbServer.AddInParameter(command, "ClinicID", DbType.Int64, BizActionObj.ClinicID);
                else
                    dbServer.AddInParameter(command, "ClinicID", DbType.Int64, null);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ListCashCODetails == null)
                        BizActionObj.ListCashCODetails = new List<clsCashCounterVO>();
                    while (reader.Read())
                    {
                        clsCashCounterVO objStateVO = new clsCashCounterVO();
                        objStateVO.Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        objStateVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        BizActionObj.ListCashCODetails.Add(objStateVO);
                    }

                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        //Added By Akshays

        public override IValueObject AddUpdatePriffixMaster(IValueObject valueObject, clsUserVO UserVo)
        {

            clsAddUpdatePreffixMasterBizActionVO objBizAction = valueObject as clsAddUpdatePreffixMasterBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State != ConnectionState.Open) con.Open();
                trans = con.BeginTransaction();
                int intStatus = 0;
                clsPreffixMasterVO objPreffixMaster = objBizAction.PreffixMasterDetails[0];
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdatePreffixMasterMaster");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objPreffixMaster.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Code", DbType.String, objPreffixMaster.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, objPreffixMaster.Description);
                dbServer.AddInParameter(command, "ApplicableGender", DbType.Int64, objPreffixMaster.ApplicableGender);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objPreffixMaster.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int32, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int32, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "Synchronized", DbType.Boolean, false);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, objBizAction.SuccessStatus);
                intStatus = dbServer.ExecuteNonQuery(command, trans);
                objBizAction.SuccessStatus = Convert.ToInt32(dbServer.GetParameterValue(command, "ResultStatus"));
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
                con.Dispose();
                trans.Dispose();
            }
            return objBizAction;
        }

        public override IValueObject GetPriffixMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPreffixMasterBizActionVO BizActionObj = valueObject as clsGetPreffixMasterBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPreffixMaster");

                dbServer.AddInParameter(command, "Code", DbType.String, BizActionObj.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.Description);
                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                //dbServer.AddInParameter(command, "ApplicableGender", DbType.Int64, BizActionObj.ApplicableGender);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.PreffixMasterDetails == null)
                        BizActionObj.PreffixMasterDetails = new List<clsPreffixMasterVO>();
                    while (reader.Read())
                    {
                        clsPreffixMasterVO objStateVO = new clsPreffixMasterVO();
                        objStateVO.ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        objStateVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objStateVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objStateVO.GetApplicableGender = Convert.ToString(DALHelper.HandleDBNull(reader["ApplicableGender"]));
                        objStateVO.ApplicableGender = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApplicableGenderID"]));
                        objStateVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        
                        //dbServer.AddInParameter(command, "sortExpression", DbType.String, null);

                        //dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);
                        BizActionObj.PreffixMasterDetails.Add(objStateVO);
                        
                    }

                    reader.Close();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        #region Defined to get optimised data in DAL (in terms of Data Size) 09012017

        public override IValueObject GetRegionDetailsByCityIDListForReg(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRegionDetailsByCityIDForRegBizActionVO BizActionObj = valueObject as clsGetRegionDetailsByCityIDForRegBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetRegionMasterListByCityID");
                if (BizActionObj.CityId > 0)
                    dbServer.AddInParameter(command, "CityId", DbType.Int64, BizActionObj.CityId);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ListRegionDetails == null)
                        BizActionObj.ListRegionDetails = new List<clsRegionForRegVO>();
                    while (reader.Read())
                    {
                        clsRegionForRegVO objRegionVO = new clsRegionForRegVO();
                        objRegionVO.Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        //objRegionVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objRegionVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objRegionVO.PinCode = Convert.ToString(DALHelper.HandleDBNull(reader["PinCode"]));
                        BizActionObj.ListRegionDetails.Add(objRegionVO);
                    }
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        #endregion

    }
}
