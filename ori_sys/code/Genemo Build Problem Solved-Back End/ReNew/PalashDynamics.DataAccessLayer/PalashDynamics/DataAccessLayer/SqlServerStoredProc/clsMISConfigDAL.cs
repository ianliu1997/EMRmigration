namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration.MISConfiguration;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    internal class clsMISConfigDAL : clsBaseMISConfigDAL
    {
        private Database dbServer;
        private LogManager logManager;

        public clsMISConfigDAL()
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

        public override IValueObject AddMISConfig(IValueObject valueObject, clsUserVO UserVO)
        {
            clsAddConfigBizActionVO nvo = valueObject as clsAddConfigBizActionVO;
            try
            {
                clsMISConfigurationVO addMISConfig = nvo.AddMISConfig;
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddConfigMIS");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "ClinicId", DbType.Int64, addMISConfig.ClinicId);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleName", DbType.String, addMISConfig.ScheduleName);
                this.dbServer.AddInParameter(storedProcCommand, "MISReportFormatId", DbType.Int64, addMISConfig.MISReportFormatId);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduledOn", DbType.Int64, addMISConfig.ScheduleOn);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleDetails", DbType.String, addMISConfig.ScheduleDetails);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleTime", DbType.DateTime, addMISConfig.ScheduleTime);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleStartDate", DbType.DateTime, addMISConfig.ScheduleStartDate);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleEndDate", DbType.DateTime, addMISConfig.ScheduleEndDate);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddUpdateMISConfig(IValueObject valueObject, clsUserVO UserVO)
        {
            clsAddUpdateMISConfigurationBizActionVO nvo = valueObject as clsAddUpdateMISConfigurationBizActionVO;
            try
            {
                clsMISConfigurationVO addMISConfig = nvo.AddMISConfig;
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateConfigMIS");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "ConfigDate", DbType.DateTime, addMISConfig.ConfigDate);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "IsStatusUpdated", DbType.Boolean, nvo.IsUpdateStatus);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, addMISConfig.Status);
                this.dbServer.AddInParameter(storedProcCommand, "ClinicId", DbType.Int64, addMISConfig.ClinicId);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleName", DbType.String, addMISConfig.ScheduleName);
                this.dbServer.AddInParameter(storedProcCommand, "MISReportFormatId", DbType.Int64, addMISConfig.MISReportFormatId);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduledOn", DbType.Int64, addMISConfig.ScheduleOn);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleDetails", DbType.String, addMISConfig.ScheduleDetails);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleTime", DbType.DateTime, addMISConfig.ScheduleTime);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleStartDate", DbType.DateTime, addMISConfig.ScheduleStartDate);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleEndDate", DbType.DateTime, addMISConfig.ScheduleEndDate);
                this.dbServer.AddInParameter(storedProcCommand, "ReportList", DbType.String, addMISConfig.ReportDetails);
                this.dbServer.AddInParameter(storedProcCommand, "StaffList", DbType.String, addMISConfig.StaffDetails);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetAutoEmailForMIS(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetMISEmailDetailsBizActionVO nvo = valueObject as clsGetMISEmailDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetEmailDetailsForMIS");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (nvo.MISConfigDetails == null)
                        {
                            nvo.MISConfigDetails = new List<clsMISConfigurationVO>();
                        }
                        clsMISConfigurationVO item = new clsMISConfigurationVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"])),
                            ClinicId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClinicId"])),
                            ScheduleName = Convert.ToString(DALHelper.HandleDBNull(reader["ScheduleName"])),
                            MISReportFormatId = Convert.ToInt64(DALHelper.HandleDBNull(reader["MISReportFormatId"])),
                            ScheduleOn = Convert.ToInt64(DALHelper.HandleDBNull(reader["ScheduledOn"])),
                            ScheduleDetails = Convert.ToString(DALHelper.HandleDBNull(reader["ScheduleDetails"])),
                            ScheduleTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["ScheduleTime"]))),
                            ScheduleStartDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["ScheduleStartDate"]))),
                            ScheduleEndDate = DALHelper.HandleDate(reader["ScheduleEndDate"])
                        };
                        nvo.MISConfigDetails.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetMISConfig(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetMISConfigBizActionVO nvo = valueObject as clsGetMISConfigBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetConfigMIS");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.String, nvo.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (nvo.GetMISConfig == null)
                        {
                            nvo.GetMISConfig = new clsMISConfigurationVO();
                        }
                        nvo.GetMISConfig.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"]));
                        nvo.GetMISConfig.ClinicId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClinicId"]));
                        nvo.GetMISConfig.ScheduleName = Convert.ToString(DALHelper.HandleDBNull(reader["ScheduleName"]));
                        nvo.GetMISConfig.MISReportFormatId = Convert.ToInt64(DALHelper.HandleDBNull(reader["MISReportFormatId"]));
                        nvo.GetMISConfig.ScheduleOn = Convert.ToInt64(DALHelper.HandleDBNull(reader["ScheduledOn"]));
                        nvo.GetMISConfig.ScheduleDetails = Convert.ToString(DALHelper.HandleDBNull(reader["ScheduleDetails"]));
                        nvo.GetMISConfig.ScheduleTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["ScheduleTime"])));
                        nvo.GetMISConfig.ScheduleStartDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["ScheduleStartDate"])));
                        nvo.GetMISConfig.ScheduleEndDate = DALHelper.HandleDate(reader["ScheduleEndDate"]);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    nvo.GetConfigReport = new List<clsGetMISReportTypeVO>();
                    while (reader.Read())
                    {
                        clsGetMISReportTypeVO item = new clsGetMISReportTypeVO {
                            Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Sys_MISReportID"])),
                            MISTypeID = (reader["MIStypeId"].HandleDBNull() == null) ? 0L : Convert.ToInt64(reader["MIStypeId"]),
                            Status = (reader["Status"].HandleDBNull() != null) && Convert.ToBoolean(reader["Status"]),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["ReportName"]))
                        };
                        nvo.GetConfigReport.Add(item);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    nvo.GetConfigStaff = new List<clsGetMISStaffVO>();
                    while (reader.Read())
                    {
                        clsGetMISStaffVO item = new clsGetMISStaffVO {
                            StaffTypeId = (reader["StaffTypeId"].HandleDBNull() == null) ? 0L : Convert.ToInt64(reader["StaffTypeId"]),
                            SelectStaffTypeId = (reader["SelectStaffTypeId"].HandleDBNull() == null) ? 0L : Convert.ToInt64(reader["SelectStaffTypeId"]),
                            Id = (reader["StaffId"].HandleDBNull() == null) ? 0L : Convert.ToInt64(reader["StaffId"]),
                            Status = (reader["Status"].HandleDBNull() != null) && Convert.ToBoolean(reader["Status"]),
                            Name = Convert.ToString(DALHelper.HandleDBNull(reader["StaffName"]))
                        };
                        nvo.GetConfigStaff.Add(item);
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

        public override IValueObject GetMISConfigList(IValueObject valueObject, clsUserVO UserVO)
        {
            clsgetMISconfigListBizActionVO nvo = valueObject as clsgetMISconfigListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetConfigMISList");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "FilterbyClinic", DbType.String, nvo.FilterbyClinic);
                this.dbServer.AddInParameter(storedProcCommand, "FilterbyMISType", DbType.String, nvo.FilterbyMISType);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (nvo.GetMISConfig == null)
                        {
                            nvo.GetMISConfig = new List<clsMISConfigurationVO>();
                        }
                        clsMISConfigurationVO item = new clsMISConfigurationVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            ClinicId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClinicId"])),
                            ScheduleName = Convert.ToString(DALHelper.HandleDBNull(reader["ScheduleName"])),
                            ClinicName = Convert.ToString(DALHelper.HandleDBNull(reader["ClinicName"])),
                            MISTypeName = Convert.ToString(DALHelper.HandleDBNull(reader["MISType"])),
                            MISTypeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["MISTypeId"])),
                            ClinicCode = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            ConfigDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["ConfigDate"]))),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.GetMISConfig.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetMISDetailsFromCriteria(IValueObject valueObject, clsUserVO UserVO)
        {
            GetMISDetailsFromCriteriaBizActionVO nvo = valueObject as GetMISDetailsFromCriteriaBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMISDetailsFromCriteria");
                this.dbServer.AddInParameter(storedProcCommand, "StartDate ", DbType.DateTime, nvo.StartDate);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleTime", DbType.DateTime, nvo.ScheduleTime);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (nvo.MISConfigDetails == null)
                        {
                            nvo.MISConfigDetails = new List<clsMISConfigurationVO>();
                        }
                        clsMISConfigurationVO item = new clsMISConfigurationVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"]))
                        };
                        nvo.MISConfigDetails.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetMISReportDetails(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetMISReportDetailsBiZActionVO nvo = valueObject as clsGetMISReportDetailsBiZActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMISReportDetails");
                this.dbServer.AddInParameter(storedProcCommand, "MISID ", DbType.Int64, nvo.MISID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (nvo.MISReportDetails == null)
                        {
                            nvo.MISReportDetails = new List<clsMISReportVO>();
                        }
                        clsMISReportVO item = new clsMISReportVO {
                            MISID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"])),
                            Sys_MISReportId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Sys_MISReportId"])),
                            MISReportFormat = Convert.ToInt64(DALHelper.HandleDBNull(reader["MISReportFormatId"])),
                            rptFileName = Convert.ToString(DALHelper.HandleDBNull(reader["rptFileName"])),
                            ReportName = Convert.ToString(DALHelper.HandleDBNull(reader["ReportName"])),
                            StaffTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StaffTypeId"])),
                            StaffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StaffId"])),
                            EmailID = Convert.ToString(DALHelper.HandleDBNull(reader["EmailID"]))
                        };
                        nvo.MISReportDetails.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetMISReportType(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetMISReportTypeBizActionVO nvo = valueObject as clsGetMISReportTypeBizActionVO;
            if (nvo.GetMIsReport == null)
            {
                nvo.GetMIsReport = new List<clsGetMISReportTypeVO>();
            }
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMISReportType");
                this.dbServer.AddInParameter(storedProcCommand, "MISTypeID", DbType.Int64, nvo.MISTypeID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsGetMISReportTypeVO item = new clsGetMISReportTypeVO {
                            Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            MISTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MISTypeID"])),
                            rptFileName = Convert.ToString(DALHelper.HandleDBNull(reader["rptFileName"]))
                        };
                        nvo.GetMIsReport.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetStaff(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetMISStaffBizActionVO nvo = valueObject as clsGetMISStaffBizActionVO;
            if (nvo.GetStaffInfo == null)
            {
                nvo.GetStaffInfo = new List<clsGetMISStaffVO>();
            }
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStaff");
                this.dbServer.AddInParameter(storedProcCommand, "TypeId", DbType.Int64, nvo.TypeId);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            while (reader.Read())
                            {
                                clsGetMISStaffVO fvo2 = new clsGetMISStaffVO {
                                    Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"])),
                                    Name = Convert.ToString(DALHelper.HandleDBNull(reader["Name"]))
                                };
                                if (nvo.TypeId == 1L)
                                {
                                    fvo2.SelectStaffTypeId = 1L;
                                    fvo2.StaffTypeId = 3L;
                                }
                                else if (nvo.TypeId == 2L)
                                {
                                    fvo2.SelectStaffTypeId = 2L;
                                    fvo2.StaffTypeId = 2L;
                                }
                                else if (nvo.TypeId == 3L)
                                {
                                    fvo2.SelectStaffTypeId = 3L;
                                    fvo2.StaffTypeId = 3L;
                                }
                                nvo.GetStaffInfo.Add(fvo2);
                            }
                            break;
                        }
                        clsGetMISStaffVO item = new clsGetMISStaffVO {
                            Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"])),
                            Name = DALHelper.HandleDBNull(reader["Name"]).ToString()
                        };
                        if (nvo.TypeId == 1L)
                        {
                            item.SelectStaffTypeId = 1L;
                            item.StaffTypeId = 2L;
                        }
                        else if (nvo.TypeId == 2L)
                        {
                            item.SelectStaffTypeId = 2L;
                            item.StaffTypeId = 2L;
                        }
                        else if (nvo.TypeId == 3L)
                        {
                            item.SelectStaffTypeId = 3L;
                            item.StaffTypeId = 3L;
                        }
                        nvo.GetStaffInfo.Add(item);
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

