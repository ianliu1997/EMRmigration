namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Masters;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Master.Location;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsLocationDAL : clsBaseLocationDAL
    {
        private Database dbServer;

        private clsLocationDAL()
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

        public override IValueObject AddAddressLocation6BizActionInfo(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddAddressLocation6BizActionVO nvo = (clsAddAddressLocation6BizActionVO) valueObject;
            try
            {
                clsAddressLocation6VO locationvo = nvo.objAddressLocation6Detail;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddAddressLocationMaster");
                this.dbServer.AddInParameter(storedProcCommand, "AddressLocation6Name", DbType.String, locationvo.AddressLocation6Name.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "ZoneID", DbType.Int64, locationvo.ZoneID);
                this.dbServer.AddInParameter(storedProcCommand, "PinCode", DbType.String, locationvo.PinCode.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, locationvo.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, locationvo.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.objAddressLocation6Detail.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddAreaInfo(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddAreaBizActionVO nvo = (clsAddAreaBizActionVO) valueObject;
            try
            {
                clsAreaVO objAreaDetail = nvo.objAreaDetail;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddAreaMaster");
                this.dbServer.AddInParameter(storedProcCommand, "AreaName", DbType.String, objAreaDetail.AreaName.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "CityID", DbType.Int64, objAreaDetail.CityID);
                this.dbServer.AddInParameter(storedProcCommand, "PinCode", DbType.String, objAreaDetail.PinCode.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, objAreaDetail.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objAreaDetail.AreaID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.objAreaDetail.AreaID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddCityInfo(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddCityBizActionVO nvo = (clsAddCityBizActionVO) valueObject;
            try
            {
                clsCityVO objCityDetail = nvo.objCityDetail;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddCityMaster");
                this.dbServer.AddInParameter(storedProcCommand, "CityCode", DbType.String, objCityDetail.CityCode.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "CityName", DbType.String, objCityDetail.CityName.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, objCityDetail.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objCityDetail.CityID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.objCityDetail.CityID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if ((nvo.SuccessStatus == 1) && (nvo.CityAreaList.Count > 0))
                {
                    foreach (clsAreaVO avo in nvo.CityAreaList)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddCityAreaMaster");
                        this.dbServer.AddInParameter(command2, "CityID", DbType.Int64, nvo.objCityDetail.CityID);
                        this.dbServer.AddInParameter(command2, "AreaID", DbType.Int64, avo.AreaID);
                        this.dbServer.ExecuteNonQuery(command2);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddCountryInfo(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddCountryBizActionVO nvo = (clsAddCountryBizActionVO) valueObject;
            try
            {
                clsCountryVO objCountryDetail = nvo.objCountryDetail;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddCountryMaster");
                this.dbServer.AddInParameter(storedProcCommand, "CountryCode", DbType.String, objCountryDetail.CountryCode.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "CountryName", DbType.String, objCountryDetail.CountryName.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, objCountryDetail.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objCountryDetail.CountryID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.objCountryDetail.CountryID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if ((nvo.SuccessStatus == 1) && (nvo.CountryStateList.Count > 0))
                {
                    foreach (clsStateVO evo in nvo.CountryStateList)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddCountryStateMaster");
                        this.dbServer.AddInParameter(command2, "CountryID", DbType.Int64, nvo.objCountryDetail.CountryID);
                        this.dbServer.AddInParameter(command2, "StateID", DbType.Int64, evo.StateID);
                        this.dbServer.ExecuteNonQuery(command2);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddDistInfo(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddDistBizActionVO nvo = (clsAddDistBizActionVO) valueObject;
            try
            {
                clsDistVO objDistDetail = nvo.objDistDetail;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddDistMaster");
                this.dbServer.AddInParameter(storedProcCommand, "DistName", DbType.String, objDistDetail.DistName.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, objDistDetail.Code.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, objDistDetail.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDistDetail.DistID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.objDistDetail.DistID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if ((nvo.SuccessStatus == 1) && (nvo.DistCityList.Count > 0))
                {
                    foreach (clsCityVO yvo in nvo.DistCityList)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddDistCityMaster");
                        this.dbServer.AddInParameter(command2, "DistID", DbType.Int64, nvo.objDistDetail.DistID);
                        this.dbServer.AddInParameter(command2, "CityID", DbType.Int64, yvo.CityID);
                        this.dbServer.ExecuteNonQuery(command2);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddStateInfo(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddStateBizActionVO nvo = (clsAddStateBizActionVO) valueObject;
            try
            {
                clsStateVO objStateDetail = nvo.objStateDetail;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddStateMaster");
                this.dbServer.AddInParameter(storedProcCommand, "MasterZoneCode", DbType.String, objStateDetail.MasterZoneCode.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "StateName", DbType.String, objStateDetail.StateName.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, objStateDetail.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objStateDetail.StateID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.objStateDetail.StateID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if ((nvo.SuccessStatus == 1) && (nvo.StateDistList.Count > 0))
                {
                    foreach (clsDistVO tvo in nvo.StateDistList)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddStateDistMaster");
                        this.dbServer.AddInParameter(command2, "StateID", DbType.Int64, nvo.objStateDetail.StateID);
                        this.dbServer.AddInParameter(command2, "DistID", DbType.Int64, tvo.DistID);
                        this.dbServer.ExecuteNonQuery(command2);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetAddressLocation6List(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetAddressLocation6BizActionVO nvo = (clsGetAddressLocation6BizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAddressLocation6List");
                if ((nvo.AddressLocation6Name != null) && (nvo.AddressLocation6Name.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "AddressLocation6Name", DbType.String, nvo.AddressLocation6Name + "%");
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "AddressLocation6Name", DbType.String, null);
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ID");
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.objAddressLocation6List == null)
                    {
                        nvo.objAddressLocation6List = new List<clsAddressLocation6VO>();
                    }
                    while (reader.Read())
                    {
                        clsAddressLocation6VO item = new clsAddressLocation6VO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            AddressLocation6Name = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            PinCode = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            ZoneID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AreaID"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.objAddressLocation6List.Add(item);
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

        public override IValueObject GetAddressLocation6ListByZoneId(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetAddressLocation6ListByZoneIdBizActionVO nvo = (clsGetAddressLocation6ListByZoneIdBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAddressLocation6ListByZoneID");
                this.dbServer.AddInParameter(storedProcCommand, "ZoneID", DbType.Int64, nvo.ipZoneID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.objAddressLocation6List == null)
                    {
                        nvo.objAddressLocation6List = new List<clsAddressLocation6VO>();
                    }
                    while (reader.Read())
                    {
                        clsAddressLocation6VO item = new clsAddressLocation6VO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AddressLocation6ID"])),
                            AddressLocation6Name = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            PinCode = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            ZoneID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AreaID"]))
                        };
                        nvo.objAddressLocation6List.Add(item);
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

        public override IValueObject GetAreaList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetAreaListBizActionVO nvo = (clsGetAreaListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAreaList");
                if ((nvo.AreaName != null) && (nvo.AreaName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "AreaName", DbType.String, nvo.AreaName + "%");
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "AreaName", DbType.String, null);
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ID");
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.AreaList == null)
                    {
                        nvo.AreaList = new List<clsAreaVO>();
                    }
                    while (reader.Read())
                    {
                        clsAreaVO item = new clsAreaVO {
                            AreaID = (long) reader["ID"],
                            AreaName = reader["AreaName"].ToString(),
                            PinCode = reader["PinCode"].ToString(),
                            CityID = (long) reader["CityID"],
                            Status = Convert.ToBoolean(reader["Status"])
                        };
                        nvo.AreaList.Add(item);
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

        public override IValueObject GetCityList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCityListBizActionVO nvo = (clsGetCityListBizActionVO) valueObject;
            try
            {
                if (nvo.CityID != 0L)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCityAreaDetailsByID");
                    this.dbServer.AddInParameter(storedProcCommand, "CityID", DbType.Int64, nvo.CityID);
                    DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader2.HasRows)
                    {
                        if (nvo.AreaList == null)
                        {
                            nvo.AreaList = new List<clsAreaVO>();
                        }
                        while (reader2.Read())
                        {
                            clsAreaVO item = new clsAreaVO {
                                AreaID = (long) reader2["AreaID"],
                                AreaName = reader2["Area"].ToString(),
                                PinCode = reader2["PinCode"].ToString()
                            };
                            nvo.AreaList.Add(item);
                        }
                    }
                    reader2.NextResult();
                    reader2.Close();
                }
                else
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCityList");
                    if ((nvo.CityName != null) && (nvo.CityName.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "CityName", DbType.String, nvo.CityName + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "CityName", DbType.String, null);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ID");
                    this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.CityList == null)
                        {
                            nvo.CityList = new List<clsCityVO>();
                        }
                        while (reader.Read())
                        {
                            clsCityVO item = new clsCityVO {
                                CityID = (long) reader["ID"],
                                CityName = reader["CityName"].ToString(),
                                CityCode = reader["CityCode"].ToString(),
                                Status = Convert.ToBoolean(reader["Status"])
                            };
                            nvo.CityList.Add(item);
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
            return nvo;
        }

        public override IValueObject GetCountryList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCountryListBizActionVO nvo = (clsGetCountryListBizActionVO) valueObject;
            try
            {
                if (nvo.CountryID != 0L)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCountryDetailsByID");
                    this.dbServer.AddInParameter(storedProcCommand, "CountryID", DbType.Int64, nvo.CountryID);
                    DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader2.HasRows)
                    {
                        if (nvo.StateList == null)
                        {
                            nvo.StateList = new List<clsStateVO>();
                        }
                        while (reader2.Read())
                        {
                            clsStateVO item = new clsStateVO {
                                StateID = (long) reader2["StateID"],
                                StateName = reader2["State"].ToString()
                            };
                            nvo.StateList.Add(item);
                        }
                    }
                    reader2.NextResult();
                    reader2.Close();
                }
                else
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCountryList");
                    if ((nvo.CountryName != null) && (nvo.CountryName.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "CountryName", DbType.String, nvo.CountryName + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "CountryName", DbType.String, null);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ID");
                    this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.CountryList == null)
                        {
                            nvo.CountryList = new List<clsCountryVO>();
                        }
                        while (reader.Read())
                        {
                            clsCountryVO item = new clsCountryVO {
                                CountryID = (long) reader["ID"],
                                CountryCode = Convert.ToString(DALHelper.HandleDBNull(reader["CountryCode"])),
                                CountryName = reader["CountryName"].ToString(),
                                Status = Convert.ToBoolean(reader["Status"])
                            };
                            nvo.CountryList.Add(item);
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
            return nvo;
        }

        public override IValueObject GetDistList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDistListBizActionVO nvo = (clsGetDistListBizActionVO) valueObject;
            try
            {
                if (nvo.DistID != 0L)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDistCityDetailsByID");
                    this.dbServer.AddInParameter(storedProcCommand, "DistID", DbType.Int64, nvo.DistID);
                    DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader2.HasRows)
                    {
                        if (nvo.CityList == null)
                        {
                            nvo.CityList = new List<clsCityVO>();
                        }
                        while (reader2.Read())
                        {
                            clsCityVO item = new clsCityVO {
                                CityID = (long) reader2["CityID"],
                                CityName = reader2["CityName"].ToString()
                            };
                            nvo.CityList.Add(item);
                        }
                    }
                    reader2.NextResult();
                    reader2.Close();
                }
                else
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDistList");
                    if ((nvo.DistName != null) && (nvo.DistName.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "DistName", DbType.String, nvo.DistName + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "DistName", DbType.String, null);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ID");
                    this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.DistList == null)
                        {
                            nvo.DistList = new List<clsDistVO>();
                        }
                        while (reader.Read())
                        {
                            clsDistVO item = new clsDistVO {
                                DistID = (long) reader["ID"],
                                Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                                DistName = reader["DistName"].ToString(),
                                Status = Convert.ToBoolean(reader["Status"])
                            };
                            nvo.DistList.Add(item);
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
            return nvo;
        }

        public override IValueObject GetStateList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetStateListBizActionVO nvo = (clsGetStateListBizActionVO) valueObject;
            try
            {
                if (nvo.StateID != 0L)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDistDetailsByID");
                    this.dbServer.AddInParameter(storedProcCommand, "StateID", DbType.Int64, nvo.StateID);
                    DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader2.HasRows)
                    {
                        if (nvo.DistrictList == null)
                        {
                            nvo.DistrictList = new List<clsDistVO>();
                        }
                        while (reader2.Read())
                        {
                            clsDistVO item = new clsDistVO {
                                DistID = (long) reader2["DistID"],
                                DistName = reader2["DistName"].ToString()
                            };
                            nvo.DistrictList.Add(item);
                        }
                    }
                    reader2.NextResult();
                    reader2.Close();
                }
                else
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStateList");
                    if ((nvo.StateName != null) && (nvo.StateName.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "StateName", DbType.String, nvo.StateName + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "StateName", DbType.String, null);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ID");
                    this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.StateList == null)
                        {
                            nvo.StateList = new List<clsStateVO>();
                        }
                        while (reader.Read())
                        {
                            clsStateVO item = new clsStateVO {
                                StateID = (long) reader["ID"],
                                MasterZoneCode = reader["MasterZoneCode"].ToString(),
                                StateName = reader["StateName"].ToString(),
                                Status = Convert.ToBoolean(reader["Status"])
                            };
                            nvo.StateList.Add(item);
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
            return nvo;
        }

        public override IValueObject UpdateAddressLocation6Info(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateAddressLocation6BizActionVO nvo = (clsUpdateAddressLocation6BizActionVO) valueObject;
            try
            {
                clsAddressLocation6VO locationvo = nvo.objAddressLocation6Detail;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateAddressLocation6Master");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.String, locationvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddressLocation6Name", DbType.String, locationvo.AddressLocation6Name.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "PinCode", DbType.String, locationvo.PinCode.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "ZoneID", DbType.Int64, locationvo.ZoneID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, locationvo.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject UpdateAreaInfo(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateAreaMasterDetailsBizActionVO nvo = (clsUpdateAreaMasterDetailsBizActionVO) valueObject;
            try
            {
                clsAreaVO objAreaDetail = nvo.objAreaDetail;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateAreaMaster");
                this.dbServer.AddInParameter(storedProcCommand, "AreaID", DbType.String, objAreaDetail.AreaID);
                this.dbServer.AddInParameter(storedProcCommand, "AreaName", DbType.String, objAreaDetail.AreaName.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "PinCode", DbType.String, objAreaDetail.PinCode.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "CityID", DbType.Int64, objAreaDetail.CityID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, objAreaDetail.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject UpdateCityInfo(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateCityMasterDetailsBizActionVO nvo = (clsUpdateCityMasterDetailsBizActionVO) valueObject;
            try
            {
                clsCityVO objCityDetail = nvo.objCityDetail;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateCityMaster");
                this.dbServer.AddInParameter(storedProcCommand, "CityID", DbType.String, objCityDetail.CityID);
                this.dbServer.AddInParameter(storedProcCommand, "CityCode", DbType.String, objCityDetail.CityCode.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "CityName", DbType.String, objCityDetail.CityName.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, objCityDetail.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if (nvo.SuccessStatus == 1)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteCityAreaMaster");
                    this.dbServer.AddInParameter(command2, "CityID", DbType.Int64, nvo.objCityDetail.CityID);
                    this.dbServer.ExecuteNonQuery(command2);
                    foreach (clsAreaVO avo in nvo.CityAreaList)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddCityAreaMaster");
                        this.dbServer.AddInParameter(command3, "CityID", DbType.Int64, nvo.objCityDetail.CityID);
                        this.dbServer.AddInParameter(command3, "AreaID", DbType.Int64, avo.AreaID);
                        this.dbServer.ExecuteNonQuery(command3);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject UpdateCountryInfo(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateCountryMasterDetails details = (clsUpdateCountryMasterDetails) valueObject;
            try
            {
                clsCountryVO objCountryDetail = details.objCountryDetail;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateCountryMaster");
                this.dbServer.AddInParameter(storedProcCommand, "CountryID", DbType.String, objCountryDetail.CountryID);
                this.dbServer.AddInParameter(storedProcCommand, "CountryCode", DbType.String, objCountryDetail.CountryCode.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "CountryName", DbType.String, objCountryDetail.CountryName.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, objCountryDetail.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                details.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if (details.SuccessStatus == 1)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteCountryStateMaster");
                    this.dbServer.AddInParameter(command2, "CountryID", DbType.Int64, details.objCountryDetail.CountryID);
                    this.dbServer.ExecuteNonQuery(command2);
                    foreach (clsStateVO evo in details.CountryStateList)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddCountryStateMaster");
                        this.dbServer.AddInParameter(command3, "CountryID", DbType.Int64, details.objCountryDetail.CountryID);
                        this.dbServer.AddInParameter(command3, "StateID", DbType.Int64, evo.StateID);
                        this.dbServer.ExecuteNonQuery(command3);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return details;
        }

        public override IValueObject UpdateDistInfo(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateDistMasterDetailsBizActionVO nvo = (clsUpdateDistMasterDetailsBizActionVO) valueObject;
            try
            {
                clsDistVO objDistDetail = nvo.objDistDetail;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateDistMaster");
                this.dbServer.AddInParameter(storedProcCommand, "DistID", DbType.String, objDistDetail.DistID);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, objDistDetail.Code.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "DistName", DbType.String, objDistDetail.DistName.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, objDistDetail.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if (nvo.SuccessStatus == 1)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteDistCityMaster");
                    this.dbServer.AddInParameter(command2, "DistID", DbType.Int64, nvo.objDistDetail.DistID);
                    this.dbServer.ExecuteNonQuery(command2);
                    foreach (clsCityVO yvo in nvo.DistCityList)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddDistCityMaster");
                        this.dbServer.AddInParameter(command3, "DistID", DbType.Int64, nvo.objDistDetail.DistID);
                        this.dbServer.AddInParameter(command3, "CityID", DbType.Int64, yvo.CityID);
                        this.dbServer.ExecuteNonQuery(command3);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject UpdateStateInfo(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateStateMasterDetailsBizActionVO nvo = (clsUpdateStateMasterDetailsBizActionVO) valueObject;
            try
            {
                clsStateVO objStateDetail = nvo.objStateDetail;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateStateMaster");
                this.dbServer.AddInParameter(storedProcCommand, "StateID", DbType.String, objStateDetail.StateID);
                this.dbServer.AddInParameter(storedProcCommand, "MasterZoneCode", DbType.String, objStateDetail.MasterZoneCode.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "StateName", DbType.String, objStateDetail.StateName.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, objStateDetail.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if (nvo.SuccessStatus == 1)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteStateDistMaster");
                    this.dbServer.AddInParameter(command2, "StateID", DbType.Int64, nvo.objStateDetail.StateID);
                    this.dbServer.ExecuteNonQuery(command2);
                    foreach (clsDistVO tvo in nvo.StateDistList)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddStateDistMaster");
                        this.dbServer.AddInParameter(command3, "StateID", DbType.Int64, nvo.objStateDetail.StateID);
                        this.dbServer.AddInParameter(command3, "DistID", DbType.Int64, tvo.DistID);
                        this.dbServer.ExecuteNonQuery(command3);
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

