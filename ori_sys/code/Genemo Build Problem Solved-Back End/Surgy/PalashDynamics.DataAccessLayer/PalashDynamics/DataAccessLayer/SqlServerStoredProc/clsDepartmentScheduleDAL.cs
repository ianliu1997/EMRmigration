namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration.DepartmentScheduleMaster;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsDepartmentScheduleDAL : clsBaseDepartmentScheduleDAL
    {
        private Database dbServer;

        private clsDepartmentScheduleDAL()
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

        private clsAddDepartmentScheduleMasterBizActionVO AddDcotorSchedule(clsAddDepartmentScheduleMasterBizActionVO BizActionobj, clsUserVO objUserVO)
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
                clsDepartmentScheduleVO departmentScheduleDetails = BizActionobj.DepartmentScheduleDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddDepartmentSchedule");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, departmentScheduleDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, departmentScheduleDetails.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, departmentScheduleDetails.ID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionobj.DepartmentScheduleDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                foreach (clsDepartmentScheduleDetailsVO svo in departmentScheduleDetails.DepartmentScheduleDetailsList)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddDepartmentScheduleDetails");
                    this.dbServer.AddInParameter(command2, "DepartmentScheduleID", DbType.Int64, departmentScheduleDetails.ID);
                    this.dbServer.AddInParameter(command2, "DayID", DbType.Int64, svo.DayID);
                    this.dbServer.AddInParameter(command2, "ScheduleID", DbType.Int64, svo.ScheduleID);
                    this.dbServer.AddInParameter(command2, "StartTime", DbType.DateTime, svo.StartTime);
                    this.dbServer.AddInParameter(command2, "EndTime", DbType.DateTime, svo.EndTime);
                    this.dbServer.AddInParameter(command2, "ApplyToAllDay", DbType.Boolean, svo.ApplyToAllDay);
                    this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                    this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    svo.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                }
                transaction.Commit();
                BizActionobj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionobj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionobj.DepartmentScheduleDetails = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return BizActionobj;
        }

        public override IValueObject AddDepartmentScheduleMaster(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddDepartmentScheduleMasterBizActionVO bizActionobj = valueObject as clsAddDepartmentScheduleMasterBizActionVO;
            return ((bizActionobj.DepartmentScheduleDetails.ID != 0L) ? this.UpdateDcotorSchedule(bizActionobj, objUserVO) : this.AddDcotorSchedule(bizActionobj, objUserVO));
        }

        public override IValueObject CheckScheduleTime(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsCheckTimeForScheduleExistanceDepartmentBizActionVO nvo = (clsCheckTimeForScheduleExistanceDepartmentBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CheckDepartmentSchedule");
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, nvo.DepartmentID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Details == null)
                    {
                        nvo.Details = new List<clsDepartmentScheduleDetailsVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        clsDepartmentScheduleDetailsVO item = new clsDepartmentScheduleDetailsVO {
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            DepartmentID = (long) DALHelper.HandleDBNull(reader["DepartmentID"]),
                            DayID = (long) DALHelper.HandleDBNull(reader["DayID"]),
                            ScheduleID = (long) DALHelper.HandleDBNull(reader["ScheduleID"]),
                            StartTime = new DateTime?((DateTime) DALHelper.HandleDBNull(reader["StartTime"])),
                            EndTime = new DateTime?((DateTime) DALHelper.HandleDBNull(reader["EndTime"]))
                        };
                        nvo.SuccessStatus = true;
                        nvo.Details.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetDepartmentDepartmentDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDepartmentDepartmentDetailsBizActionVO nvo = (clsGetDepartmentDepartmentDetailsBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillDepartmentCombobox");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem(Convert.ToInt64(reader["Id"]), reader["Description"].ToString()));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetDepartmentScheduleDetailsList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDepartmentScheduleListBizActionVO nvo = (clsGetDepartmentScheduleListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDepartmentScheduleList");
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentScheduleID ", DbType.Int64, nvo.DepartmentScheduleID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DepartmentScheduleList == null)
                    {
                        nvo.DepartmentScheduleList = new List<clsDepartmentScheduleDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsDepartmentScheduleDetailsVO item = new clsDepartmentScheduleDetailsVO {
                            ID = Convert.ToInt64(reader["ID"]),
                            DepartmentScheduleID = Convert.ToInt64(reader["DepartmentScheduleID"]),
                            DayID = (long) DALHelper.HandleDBNull(reader["DayID"])
                        };
                        if (item.DayID == 1L)
                        {
                            item.Day = "Sunday";
                        }
                        else if (item.DayID == 2L)
                        {
                            item.Day = "Monday";
                        }
                        else if (item.DayID == 3L)
                        {
                            item.Day = "Tuesday";
                        }
                        else if (item.DayID == 4L)
                        {
                            item.Day = "Wednesday";
                        }
                        else if (item.DayID == 5L)
                        {
                            item.Day = "Thursday";
                        }
                        else if (item.DayID == 6L)
                        {
                            item.Day = "Friday";
                        }
                        else if (item.DayID == 7L)
                        {
                            item.Day = "Saturday";
                        }
                        item.ScheduleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ScheduleID"]));
                        item.Schedule = (string) DALHelper.HandleDBNull(reader["Schedule"]);
                        item.StartTime = (DateTime?) DALHelper.HandleDBNull(reader["StartTime"]);
                        item.EndTime = (DateTime?) DALHelper.HandleDBNull(reader["EndTime"]);
                        nvo.DepartmentScheduleList.Add(item);
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

        public override IValueObject GetDepartmentScheduleList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDepartmentScheduleMasterListBizActionVO nvo = (clsGetDepartmentScheduleMasterListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDepartmentScheduleBySearchCriteria");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, nvo.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ID");
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DepartmentScheduleList == null)
                    {
                        nvo.DepartmentScheduleList = new List<clsDepartmentScheduleVO>();
                    }
                    while (reader.Read())
                    {
                        clsDepartmentScheduleVO item = new clsDepartmentScheduleVO {
                            ID = Convert.ToInt64(reader["ID"]),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            UnitName = (string) DALHelper.HandleDBNull(reader["UnitName"]),
                            DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"])),
                            DepartmentName = (string) DALHelper.HandleDBNull(reader["DepartmentName"])
                        };
                        nvo.DepartmentScheduleList.Add(item);
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

        public override IValueObject GetDepartmentScheduleTime(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDepartmentScheduleTimeVO evo = (clsGetDepartmentScheduleTimeVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDepartmentTime");
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, evo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentId", DbType.Int64, evo.DepartmentId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (evo.DepartmentScheduleDetailsList == null)
                    {
                        evo.DepartmentScheduleDetailsList = new List<clsDepartmentScheduleDetailsVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        clsDepartmentScheduleDetailsVO item = new clsDepartmentScheduleDetailsVO {
                            DepartmentID = (long) DALHelper.HandleDBNull(reader["DepartmentID"]),
                            StartTime = new DateTime?((DateTime) DALHelper.HandleDBNull(reader["StartTime"])),
                            EndTime = new DateTime?((DateTime) DALHelper.HandleDBNull(reader["EndTime"])),
                            ScheduleID = (long) DALHelper.HandleDBNull(reader["ScheduleID"]),
                            DayID = (long) DALHelper.HandleDBNull(reader["DayID"])
                        };
                        if (item.DayID == 1L)
                        {
                            item.Day = "Sunday";
                        }
                        else if (item.DayID == 2L)
                        {
                            item.Day = "Monday";
                        }
                        else if (item.DayID == 3L)
                        {
                            item.Day = "Tuesday";
                        }
                        else if (item.DayID == 4L)
                        {
                            item.Day = "Wednesday";
                        }
                        else if (item.DayID == 5L)
                        {
                            item.Day = "Thursday";
                        }
                        else if (item.DayID == 6L)
                        {
                            item.Day = "Friday";
                        }
                        else if (item.DayID == 7L)
                        {
                            item.Day = "Saturday";
                        }
                        evo.DepartmentScheduleDetailsList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return evo;
        }

        private clsAddDepartmentScheduleMasterBizActionVO UpdateDcotorSchedule(clsAddDepartmentScheduleMasterBizActionVO BizActionobj, clsUserVO objUserVO)
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
                clsDepartmentScheduleVO departmentScheduleDetails = BizActionobj.DepartmentScheduleDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateDepartmentSchedule");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, departmentScheduleDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, departmentScheduleDetails.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, departmentScheduleDetails.ID);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionobj.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionobj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if ((departmentScheduleDetails.DepartmentScheduleDetailsList != null) && (departmentScheduleDetails.DepartmentScheduleDetailsList.Count > 0))
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteDepartmentScheduleDetails");
                    this.dbServer.AddInParameter(command2, "DepartmentScheduleID", DbType.Int64, departmentScheduleDetails.ID);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                }
                foreach (clsDepartmentScheduleDetailsVO svo in departmentScheduleDetails.DepartmentScheduleDetailsList)
                {
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddDepartmentScheduleDetails");
                    this.dbServer.AddInParameter(command3, "DepartmentScheduleID", DbType.Int64, departmentScheduleDetails.ID);
                    this.dbServer.AddInParameter(command3, "DayID", DbType.Int64, svo.DayID);
                    this.dbServer.AddInParameter(command3, "ScheduleID", DbType.Int64, svo.ScheduleID);
                    this.dbServer.AddInParameter(command3, "StartTime", DbType.DateTime, svo.StartTime);
                    this.dbServer.AddInParameter(command3, "EndTime", DbType.DateTime, svo.EndTime);
                    this.dbServer.AddInParameter(command3, "ApplyToAllDay", DbType.Boolean, svo.ApplyToAllDay);
                    this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                    this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                    this.dbServer.ExecuteNonQuery(command3, transaction);
                    svo.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                }
                transaction.Commit();
                BizActionobj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionobj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionobj.DepartmentScheduleDetails = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return BizActionobj;
        }
    }
}

