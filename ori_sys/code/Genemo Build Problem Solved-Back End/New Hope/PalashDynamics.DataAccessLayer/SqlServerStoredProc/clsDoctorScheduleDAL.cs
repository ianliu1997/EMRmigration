namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration;
    using PalashDynamics.ValueObjects.Administration.DoctorScheduleMaster;
    using PalashDynamics.ValueObjects.Master;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class clsDoctorScheduleDAL : clsBaseDoctorScheduleDAL
    {
        private Database dbServer;

        private clsDoctorScheduleDAL()
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

        private clsAddDoctorScheduleMasterBizActionVO AddDcotorSchedule(clsAddDoctorScheduleMasterBizActionVO BizActionobj, clsUserVO objUserVO)
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
                clsDoctorScheduleVO doctorScheduleDetails = BizActionobj.DoctorScheduleDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddDoctorSchedule");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, doctorScheduleDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, doctorScheduleDetails.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, doctorScheduleDetails.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, doctorScheduleDetails.ID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionobj.DoctorScheduleDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                foreach (clsDoctorScheduleDetailsVO svo in doctorScheduleDetails.DoctorScheduleDetailsList)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddDoctorScheduleDetails");
                    this.dbServer.AddInParameter(command2, "DoctorScheduleID", DbType.Int64, doctorScheduleDetails.ID);
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
                BizActionobj.DoctorScheduleDetails = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return BizActionobj;
        }

        private clsAddDoctorScheduleMasterBizActionVO AddDcotorScheduleNew(clsAddDoctorScheduleMasterBizActionVO BizActionobj, clsUserVO objUserVO)
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
                clsDoctorScheduleVO doctorScheduleDetails = BizActionobj.DoctorScheduleDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddDoctorScheduleRoster");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, doctorScheduleDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, doctorScheduleDetails.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, doctorScheduleDetails.DepartmentID);
                clsDoctorScheduleDetailsVO doctorScheduleDetailsListItem = doctorScheduleDetails.DoctorScheduleDetailsListItem;
                this.dbServer.AddInParameter(storedProcCommand, "StartTime", DbType.DateTime, doctorScheduleDetailsListItem.StartTime);
                this.dbServer.AddInParameter(storedProcCommand, "EndTime", DbType.DateTime, doctorScheduleDetailsListItem.EndTime);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleID", DbType.Int64, doctorScheduleDetailsListItem.ScheduleID);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleType", DbType.String, doctorScheduleDetailsListItem.Schedule);
                this.dbServer.AddInParameter(storedProcCommand, "DayIDnew", DbType.String, doctorScheduleDetailsListItem.DayIDnew);
                this.dbServer.AddInParameter(storedProcCommand, "IsDayNo", DbType.Boolean, doctorScheduleDetailsListItem.IsDayNo);
                if (doctorScheduleDetailsListItem.MonthDayNo > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MonthDayNo", DbType.Int64, doctorScheduleDetailsListItem.MonthDayNo);
                }
                this.dbServer.AddInParameter(storedProcCommand, "MonthWeekNoID", DbType.Int64, doctorScheduleDetailsListItem.MonthWeekNoID);
                this.dbServer.AddInParameter(storedProcCommand, "MonthWeekDayID", DbType.Int64, doctorScheduleDetailsListItem.MonthWeekDayID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, doctorScheduleDetails.ID);
                this.dbServer.AddParameter(storedProcCommand, "SuccessStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionobj.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionobj.DoctorScheduleDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                BizActionobj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "SuccessStatus");
                if (BizActionobj.SuccessStatus == 10)
                {
                    throw new Exception();
                }
                transaction.Commit();
                BizActionobj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                if (BizActionobj.SuccessStatus != 10)
                {
                    BizActionobj.SuccessStatus = -1;
                }
                transaction.Rollback();
                BizActionobj.DoctorScheduleDetails = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return BizActionobj;
        }

        public override IValueObject AddDoctorScheduleMaster(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddDoctorScheduleMasterBizActionVO bizActionobj = valueObject as clsAddDoctorScheduleMasterBizActionVO;
            return ((bizActionobj.DoctorScheduleDetails.ID != 0L) ? this.UpdateDcotorSchedule(bizActionobj, objUserVO) : this.AddDcotorSchedule(bizActionobj, objUserVO));
        }

        public override IValueObject AddDoctorScheduleMasterNew(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddDoctorScheduleMasterBizActionVO bizActionobj = valueObject as clsAddDoctorScheduleMasterBizActionVO;
            return ((bizActionobj.DoctorScheduleDetails.ID != 0L) ? this.UpdateDcotorScheduleNew(bizActionobj, objUserVO) : this.AddDcotorScheduleNew(bizActionobj, objUserVO));
        }

        public override IValueObject CheckScheduleTime(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsCheckTimeForScheduleExistanceBizActionVO nvo = (clsCheckTimeForScheduleExistanceBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CheckDoctorSchedule");
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Details == null)
                    {
                        nvo.Details = new List<clsDoctorScheduleDetailsVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        clsDoctorScheduleDetailsVO item = new clsDoctorScheduleDetailsVO {
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            DepartmentID = (long) DALHelper.HandleDBNull(reader["DepartmentID"]),
                            DoctorID = (long) DALHelper.HandleDBNull(reader["DoctorID"]),
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

        public override IValueObject GetDoctorDepartmentUnitList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorDepartmentUnitListBizActionVO nvo = (clsGetDoctorDepartmentUnitListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorDepartmentUnitList");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, nvo.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ID");
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DoctorDepartmentUnitList == null)
                    {
                        nvo.DoctorDepartmentUnitList = new List<clsDoctorDepartmentUnitListVO>();
                    }
                    while (reader.Read())
                    {
                        clsDoctorDepartmentUnitListVO item = new clsDoctorDepartmentUnitListVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"])),
                            DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"])),
                            DepartmentName = Convert.ToString(DALHelper.HandleDBNull(reader["DepartmentName"])),
                            DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"])),
                            DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"])),
                            Status = new bool?(Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])))
                        };
                        nvo.DoctorDepartmentUnitList.Add(item);
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

        public override IValueObject GetDoctorSchedule(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorScheduleMasterBizActionVO nvo = (clsGetDoctorScheduleMasterBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CheckForScheduleExistance");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, nvo.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    nvo.SuccessStatus = true;
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetDoctorScheduleDetailsList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorScheduleListBizActionVO nvo = (clsGetDoctorScheduleListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorScheduleList");
                this.dbServer.AddInParameter(storedProcCommand, "DoctorScheduleID ", DbType.Int64, nvo.DoctorScheduleID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DoctorScheduleList == null)
                    {
                        nvo.DoctorScheduleList = new List<clsDoctorScheduleDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsDoctorScheduleDetailsVO item = new clsDoctorScheduleDetailsVO {
                            ID = (long) reader["ID"],
                            DoctorScheduleID = (long) reader["DoctorScheduleID"],
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
                        item.ScheduleID = (long) DALHelper.HandleDBNull(reader["ScheduleID"]);
                        item.Schedule = (string) DALHelper.HandleDBNull(reader["Schedule"]);
                        item.StartTime = new DateTime?((DateTime) DALHelper.HandleDBNull(reader["StartTime"]));
                        item.EndTime = new DateTime?((DateTime) DALHelper.HandleDBNull(reader["EndTime"]));
                        nvo.DoctorScheduleList.Add(item);
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

        public override IValueObject GetDoctorScheduleDetailsListByID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorScheduleListByIDBizActionVO nvo = (clsGetDoctorScheduleListByIDBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetScheduleByDoctorId");
                this.dbServer.AddInParameter(storedProcCommand, "DoctorScheduleID ", DbType.Int64, nvo.DoctorScheduleID);
                this.dbServer.AddInParameter(storedProcCommand, "DayID ", DbType.Int64, nvo.DayID);
                this.dbServer.AddInParameter(storedProcCommand, "ID ", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "StartTime ", DbType.DateTime, nvo.StartTime);
                this.dbServer.AddInParameter(storedProcCommand, "EndTime ", DbType.DateTime, nvo.EndTime);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DoctorScheduleListForDoctorID == null)
                    {
                        nvo.DoctorScheduleListForDoctorID = new List<clsDoctorScheduleDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsDoctorScheduleDetailsVO item = new clsDoctorScheduleDetailsVO {
                            ID = (long) reader["ID"],
                            DoctorScheduleID = (long) reader["DoctorScheduleID"],
                            DayID = (long) DALHelper.HandleDBNull(reader["DayID"]),
                            StartTime = new DateTime?((DateTime) DALHelper.HandleDBNull(reader["StartTime"])),
                            EndTime = new DateTime?((DateTime) DALHelper.HandleDBNull(reader["EndTime"]))
                        };
                        nvo.DoctorScheduleListForDoctorID.Add(item);
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

        public override IValueObject GetDoctorScheduleDetailsListNew(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorScheduleListBizActionVO nvo = (clsGetDoctorScheduleListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorScheduleListNew");
                this.dbServer.AddInParameter(storedProcCommand, "DoctorScheduleID ", DbType.Int64, nvo.DoctorScheduleID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DoctorScheduleList == null)
                    {
                        nvo.DoctorScheduleList = new List<clsDoctorScheduleDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsDoctorScheduleDetailsVO item = new clsDoctorScheduleDetailsVO {
                            ID = (long) reader["ID"],
                            DoctorScheduleID = (long) reader["DoctorScheduleID"],
                            ScheduleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ScheduleID"])),
                            Schedule = Convert.ToString(DALHelper.HandleDBNull(reader["Schedule"])),
                            StartTime = new DateTime?((DateTime) DALHelper.HandleDBNull(reader["StartTime"])),
                            EndTime = new DateTime?((DateTime) DALHelper.HandleDBNull(reader["EndTime"])),
                            DayIDnew = Convert.ToString(DALHelper.HandleDBNull(reader["WeekPaternIDS"])),
                            IsDayNo = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDayNo"])),
                            MonthDayNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["MonthDayNo"])),
                            MonthWeekNoID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MonthWeekNoID"])),
                            MonthWeekDayID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MonthWeekDayID"]))
                        };
                        nvo.DoctorScheduleList.Add(item);
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

        public override IValueObject GetDoctorScheduleList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorScheduleMasterListBizActionVO nvo = (clsGetDoctorScheduleMasterListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorScheduleBySearchCriteria");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, nvo.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ID");
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DoctorScheduleList == null)
                    {
                        nvo.DoctorScheduleList = new List<clsDoctorScheduleVO>();
                    }
                    while (reader.Read())
                    {
                        clsDoctorScheduleVO item = new clsDoctorScheduleVO {
                            ID = (long) reader["ID"],
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            UnitName = (string) DALHelper.HandleDBNull(reader["UnitName"]),
                            DepartmentID = (long) DALHelper.HandleDBNull(reader["DepartmentID"]),
                            DepartmentName = (string) DALHelper.HandleDBNull(reader["DepartmentName"]),
                            DoctorID = (long) DALHelper.HandleDBNull(reader["DoctorID"]),
                            DoctorName = (string) DALHelper.HandleDBNull(reader["DoctorName"])
                        };
                        nvo.DoctorScheduleList.Add(item);
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

        public override IValueObject GetDoctorScheduleListNew(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorScheduleMasterListBizActionVO nvo = (clsGetDoctorScheduleMasterListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorScheduleBySearchCriteriaNew");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, nvo.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ID");
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DoctorScheduleList == null)
                    {
                        nvo.DoctorScheduleList = new List<clsDoctorScheduleVO>();
                    }
                    while (reader.Read())
                    {
                        clsDoctorScheduleVO item = new clsDoctorScheduleVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"])),
                            DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"])),
                            DepartmentName = Convert.ToString(DALHelper.HandleDBNull(reader["DepartmentName"])),
                            DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"])),
                            DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"])),
                            Status = new bool?(Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))),
                            StartTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["StartTime"]))),
                            EndTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["EndTime"]))),
                            ScheduleType = Convert.ToString(DALHelper.HandleDBNull(reader["ScheduleType"]))
                        };
                        nvo.DoctorScheduleList.Add(item);
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

        public override IValueObject GetDoctorScheduleTime(IValueObject valueObject, clsUserVO objUserVO)
        {
            GetDoctorScheduleTimeVO evo = (GetDoctorScheduleTimeVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorTime");
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, evo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentId", DbType.Int64, evo.DepartmentId);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorId", DbType.Int64, evo.DoctorId);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, evo.Date);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (evo.DoctorScheduleDetailsList == null)
                    {
                        evo.DoctorScheduleDetailsList = new List<clsDoctorScheduleDetailsVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        clsDoctorScheduleDetailsVO item = new clsDoctorScheduleDetailsVO {
                            DoctorID = (long) DALHelper.HandleDBNull(reader["DoctorID"]),
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
                        evo.DoctorScheduleDetailsList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return evo;
        }

        public override IValueObject GetDoctorScheduleWise(IValueObject valueObject, clsUserVO objUserVO)
        {
            GetDoctorScheduleWiseVO evo = (GetDoctorScheduleWiseVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorSchedulewise");
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, evo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentId", DbType.Int64, evo.DepartmentId);
                this.dbServer.AddInParameter(storedProcCommand, "Day", DbType.Int64, evo.Day);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, evo.Date);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (evo.DoctorScheduleDetailsList == null)
                    {
                        evo.DoctorScheduleDetailsList = new List<clsDoctorScheduleDetailsVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        clsDoctorScheduleDetailsVO item = new clsDoctorScheduleDetailsVO {
                            DoctorID = (long) DALHelper.HandleDBNull(reader["DoctorID"]),
                            StartTime = new DateTime?((DateTime) DALHelper.HandleDBNull(reader["StartTime"])),
                            EndTime = new DateTime?((DateTime) DALHelper.HandleDBNull(reader["EndTime"])),
                            ScheduleID = (long) DALHelper.HandleDBNull(reader["ScheduleID"]),
                            DayID = (long) DALHelper.HandleDBNull(reader["DayID"]),
                            DoctorName = (string) DALHelper.HandleDBNull(reader["DoctorName"])
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
                        evo.DoctorScheduleDetailsList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return evo;
        }

        public override IValueObject GetVisitTypeDetails(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetMasterListForVisitBizActionVO nvo = (clsGetMasterListForVisitBizActionVO) valueObject;
            try
            {
                StringBuilder builder = new StringBuilder();
                if (nvo.IsActive != null)
                {
                    KeyValue parent = nvo.Parent;
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMasterList_New");
                this.dbServer.AddInParameter(storedProcCommand, "MasterTableName", DbType.String, nvo.MasterTable.ToString());
                this.dbServer.AddInParameter(storedProcCommand, "FilterExpression", DbType.String, builder.ToString());
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString(), Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFree"])), Convert.ToInt64(DALHelper.HandleDBNull(reader["FreeDaysDuration"])), Convert.ToInt64(DALHelper.HandleDBNull(reader["ConsultationVisitTypeID"]))));
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                nvo.Error = exception1.Message;
            }
            return nvo;
        }

        private clsAddDoctorScheduleMasterBizActionVO UpdateDcotorSchedule(clsAddDoctorScheduleMasterBizActionVO BizActionobj, clsUserVO objUserVO)
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
                clsDoctorScheduleVO doctorScheduleDetails = BizActionobj.DoctorScheduleDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateDoctorSchedule");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, doctorScheduleDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, doctorScheduleDetails.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, doctorScheduleDetails.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, doctorScheduleDetails.ID);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionobj.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionobj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if ((doctorScheduleDetails.DoctorScheduleDetailsList != null) && (doctorScheduleDetails.DoctorScheduleDetailsList.Count > 0))
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteDoctorScheduleDetails");
                    this.dbServer.AddInParameter(command2, "DoctorScheduleID", DbType.Int64, doctorScheduleDetails.ID);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                }
                foreach (clsDoctorScheduleDetailsVO svo in doctorScheduleDetails.DoctorScheduleDetailsList)
                {
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddDoctorScheduleDetails");
                    this.dbServer.AddInParameter(command3, "DoctorScheduleID", DbType.Int64, doctorScheduleDetails.ID);
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
                BizActionobj.DoctorScheduleDetails = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return BizActionobj;
        }

        private clsAddDoctorScheduleMasterBizActionVO UpdateDcotorScheduleNew(clsAddDoctorScheduleMasterBizActionVO BizActionobj, clsUserVO objUserVO)
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
                clsDoctorScheduleVO doctorScheduleDetails = BizActionobj.DoctorScheduleDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateDoctorScheduleRoster");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, doctorScheduleDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, doctorScheduleDetails.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, doctorScheduleDetails.DepartmentID);
                clsDoctorScheduleDetailsVO doctorScheduleDetailsListItem = doctorScheduleDetails.DoctorScheduleDetailsListItem;
                this.dbServer.AddInParameter(storedProcCommand, "StartTime", DbType.DateTime, doctorScheduleDetailsListItem.StartTime);
                this.dbServer.AddInParameter(storedProcCommand, "EndTime", DbType.DateTime, doctorScheduleDetailsListItem.EndTime);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleID", DbType.Int64, doctorScheduleDetailsListItem.ScheduleID);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleType", DbType.String, doctorScheduleDetailsListItem.Schedule);
                this.dbServer.AddInParameter(storedProcCommand, "DayIDnew", DbType.String, doctorScheduleDetailsListItem.DayIDnew);
                this.dbServer.AddInParameter(storedProcCommand, "IsDayNo", DbType.Boolean, doctorScheduleDetailsListItem.IsDayNo);
                if (doctorScheduleDetailsListItem.MonthDayNo > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MonthDayNo", DbType.Int64, doctorScheduleDetailsListItem.MonthDayNo);
                }
                this.dbServer.AddInParameter(storedProcCommand, "MonthWeekNoID", DbType.Int64, doctorScheduleDetailsListItem.MonthWeekNoID);
                this.dbServer.AddInParameter(storedProcCommand, "MonthWeekDayID", DbType.Int64, doctorScheduleDetailsListItem.MonthWeekDayID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, doctorScheduleDetails.ID);
                this.dbServer.AddParameter(storedProcCommand, "SuccessStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionobj.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionobj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "SuccessStatus");
                if (BizActionobj.SuccessStatus == 10)
                {
                    throw new Exception();
                }
                transaction.Commit();
                BizActionobj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                if (BizActionobj.SuccessStatus != 10)
                {
                    BizActionobj.SuccessStatus = -1;
                }
                transaction.Rollback();
                BizActionobj.DoctorScheduleDetails = null;
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

