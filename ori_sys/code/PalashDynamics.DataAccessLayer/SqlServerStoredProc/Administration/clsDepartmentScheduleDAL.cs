using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration.DepartmentScheduleMaster;
using System.Data.Common;
using System.Data;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsDepartmentScheduleDAL : clsBaseDepartmentScheduleDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        #endregion

        private clsDepartmentScheduleDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql object
                //Create Instance of the database object and BaseSql object
                if (dbServer == null)
                {
                    dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override IValueObject AddDepartmentScheduleMaster(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddDepartmentScheduleMasterBizActionVO BizActionobj = valueObject as clsAddDepartmentScheduleMasterBizActionVO;
            if (BizActionobj.DepartmentScheduleDetails.ID == 0)
            {
                BizActionobj = AddDcotorSchedule(BizActionobj, objUserVO);
            }
            else
            {
                BizActionobj = UpdateDcotorSchedule(BizActionobj, objUserVO);
            }
            return BizActionobj;

        }

        private clsAddDepartmentScheduleMasterBizActionVO AddDcotorSchedule(clsAddDepartmentScheduleMasterBizActionVO BizActionobj, clsUserVO objUserVO)
        {
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                clsDepartmentScheduleVO objDepartmentScheduleVO = BizActionobj.DepartmentScheduleDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddDepartmentSchedule");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDepartmentScheduleVO.UnitID);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, objDepartmentScheduleVO.DepartmentID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDepartmentScheduleVO.ID);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionobj.DepartmentScheduleDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

                foreach (var ObjDetails in objDepartmentScheduleVO.DepartmentScheduleDetailsList)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddDepartmentScheduleDetails");

                    dbServer.AddInParameter(command1, "DepartmentScheduleID", DbType.Int64, objDepartmentScheduleVO.ID);
                    dbServer.AddInParameter(command1, "DayID", DbType.Int64, ObjDetails.DayID);
                    dbServer.AddInParameter(command1, "ScheduleID", DbType.Int64, ObjDetails.ScheduleID);
                    dbServer.AddInParameter(command1, "StartTime", DbType.DateTime, ObjDetails.StartTime);
                    dbServer.AddInParameter(command1, "EndTime", DbType.DateTime, ObjDetails.EndTime);
                    dbServer.AddInParameter(command1, "ApplyToAllDay", DbType.Boolean, ObjDetails.ApplyToAllDay);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);

                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDetails.ID);

                    int iStatus = dbServer.ExecuteNonQuery(command1, trans);
                    ObjDetails.ID = (long)dbServer.GetParameterValue(command1, "ID");
                }
                trans.Commit();
                BizActionobj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                //   throw;
                BizActionobj.SuccessStatus = -1;
                trans.Rollback();
                BizActionobj.DepartmentScheduleDetails = null;
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }
            return BizActionobj;
        }

        private clsAddDepartmentScheduleMasterBizActionVO UpdateDcotorSchedule(clsAddDepartmentScheduleMasterBizActionVO BizActionobj, clsUserVO objUserVO)
        {
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();
                clsDepartmentScheduleVO objDepartmentScheduleVO = BizActionobj.DepartmentScheduleDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateDepartmentSchedule");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDepartmentScheduleVO.UnitID);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, objDepartmentScheduleVO.DepartmentID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);

                dbServer.AddInParameter(command, "ID", DbType.Int64, objDepartmentScheduleVO.ID);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionobj.SuccessStatus);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionobj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");


                if (objDepartmentScheduleVO.DepartmentScheduleDetailsList != null && objDepartmentScheduleVO.DepartmentScheduleDetailsList.Count > 0)
                {
                    DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_DeleteDepartmentScheduleDetails");

                    dbServer.AddInParameter(command3, "DepartmentScheduleID", DbType.Int64, objDepartmentScheduleVO.ID);
                    int intStatus2 = dbServer.ExecuteNonQuery(command3, trans);
                }
                foreach (var ObjDetails in objDepartmentScheduleVO.DepartmentScheduleDetailsList)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddDepartmentScheduleDetails");

                    dbServer.AddInParameter(command1, "DepartmentScheduleID", DbType.Int64, objDepartmentScheduleVO.ID);
                    dbServer.AddInParameter(command1, "DayID", DbType.Int64, ObjDetails.DayID);
                    dbServer.AddInParameter(command1, "ScheduleID", DbType.Int64, ObjDetails.ScheduleID);
                    dbServer.AddInParameter(command1, "StartTime", DbType.DateTime, ObjDetails.StartTime);
                    dbServer.AddInParameter(command1, "EndTime", DbType.DateTime, ObjDetails.EndTime);
                    dbServer.AddInParameter(command1, "ApplyToAllDay", DbType.Boolean, ObjDetails.ApplyToAllDay);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);

                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDetails.ID);

                    int iStatus = dbServer.ExecuteNonQuery(command1, trans);
                    ObjDetails.ID = (long)dbServer.GetParameterValue(command1, "ID");
                }
                trans.Commit();
                BizActionobj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                //throw;
                BizActionobj.SuccessStatus = -1;
                trans.Rollback();
                BizActionobj.DepartmentScheduleDetails = null;
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }
            return BizActionobj;
        }

        public override IValueObject GetDepartmentScheduleList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDepartmentScheduleMasterListBizActionVO BizActionObj = (clsGetDepartmentScheduleMasterListBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDepartmentScheduleBySearchCriteria");
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizActionObj.DepartmentID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DepartmentScheduleList == null)
                        BizActionObj.DepartmentScheduleList = new List<clsDepartmentScheduleVO>();

                    while (reader.Read())
                    {
                        clsDepartmentScheduleVO objDepartmentScheduleVO = new clsDepartmentScheduleVO();
                        objDepartmentScheduleVO.ID = Convert.ToInt64(reader["ID"]);
                        objDepartmentScheduleVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objDepartmentScheduleVO.UnitName = (string)DALHelper.HandleDBNull(reader["UnitName"]);
                        objDepartmentScheduleVO.DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"]));
                        objDepartmentScheduleVO.DepartmentName = (string)DALHelper.HandleDBNull(reader["DepartmentName"]);
                        BizActionObj.DepartmentScheduleList.Add(objDepartmentScheduleVO);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return BizActionObj;
        }


        ///New DepartmentSchedule
        ///Date:11 - Aug -2011

        public override IValueObject GetDepartmentScheduleDetailsList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDepartmentScheduleListBizActionVO BizActionObj = (clsGetDepartmentScheduleListBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDepartmentScheduleList");
                dbServer.AddInParameter(command, "DepartmentScheduleID ", DbType.Int64, BizActionObj.DepartmentScheduleID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DepartmentScheduleList == null)
                        BizActionObj.DepartmentScheduleList = new List<clsDepartmentScheduleDetailsVO>();

                    while (reader.Read())
                    {
                        clsDepartmentScheduleDetailsVO objDepartmentScheduleVO = new clsDepartmentScheduleDetailsVO();
                        objDepartmentScheduleVO.ID = Convert.ToInt64(reader["ID"]);
                        objDepartmentScheduleVO.DepartmentScheduleID = Convert.ToInt64(reader["DepartmentScheduleID"]);
                        objDepartmentScheduleVO.DayID = (long)DALHelper.HandleDBNull(reader["DayID"]);
                        if (objDepartmentScheduleVO.DayID == 1)
                            objDepartmentScheduleVO.Day = "Sunday";
                        else if (objDepartmentScheduleVO.DayID == 2)
                            objDepartmentScheduleVO.Day = "Monday";
                        else if (objDepartmentScheduleVO.DayID == 3)
                            objDepartmentScheduleVO.Day = "Tuesday";
                        else if (objDepartmentScheduleVO.DayID == 4)
                            objDepartmentScheduleVO.Day = "Wednesday";
                        else if (objDepartmentScheduleVO.DayID == 5)
                            objDepartmentScheduleVO.Day = "Thursday";
                        else if (objDepartmentScheduleVO.DayID == 6)
                            objDepartmentScheduleVO.Day = "Friday";
                        else if (objDepartmentScheduleVO.DayID == 7)
                            objDepartmentScheduleVO.Day = "Saturday";

                        objDepartmentScheduleVO.ScheduleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ScheduleID"]));
                        objDepartmentScheduleVO.Schedule = (string)DALHelper.HandleDBNull(reader["Schedule"]);
                        objDepartmentScheduleVO.StartTime = (DateTime?)DALHelper.HandleDBNull(reader["StartTime"]);
                        objDepartmentScheduleVO.EndTime = (DateTime?)DALHelper.HandleDBNull(reader["EndTime"]);
                        BizActionObj.DepartmentScheduleList.Add(objDepartmentScheduleVO);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return BizActionObj;
        }

        public override IValueObject CheckScheduleTime(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsCheckTimeForScheduleExistanceDepartmentBizActionVO BizAction = (clsCheckTimeForScheduleExistanceDepartmentBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_CheckDepartmentSchedule");
                //dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                //dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizAction.DepartmentID);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizAction.DepartmentID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizAction.Details == null)
                        BizAction.Details = new List<clsDepartmentScheduleDetailsVO>();

                    while (reader.Read())
                    {
                        clsDepartmentScheduleDetailsVO DepartmentScheduleVO = new clsDepartmentScheduleDetailsVO();
                        DepartmentScheduleVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        DepartmentScheduleVO.DepartmentID = (long)DALHelper.HandleDBNull(reader["DepartmentID"]);
                        DepartmentScheduleVO.DayID = (long)DALHelper.HandleDBNull(reader["DayID"]);
                        DepartmentScheduleVO.ScheduleID = (long)DALHelper.HandleDBNull(reader["ScheduleID"]);
                        DepartmentScheduleVO.StartTime = (DateTime)DALHelper.HandleDBNull(reader["StartTime"]);
                        DepartmentScheduleVO.EndTime = (DateTime)DALHelper.HandleDBNull(reader["EndTime"]);

                        BizAction.SuccessStatus = true;
                        BizAction.Details.Add(DepartmentScheduleVO);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return BizAction;
        }

        public override IValueObject GetDepartmentScheduleTime(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDepartmentScheduleTimeVO BizAction = (clsGetDepartmentScheduleTimeVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDepartmentTime");
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizAction.UnitId);
                dbServer.AddInParameter(command, "DepartmentId", DbType.Int64, BizAction.DepartmentId);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizAction.DepartmentScheduleDetailsList == null)
                        BizAction.DepartmentScheduleDetailsList = new List<clsDepartmentScheduleDetailsVO>();

                    while (reader.Read())
                    {
                        clsDepartmentScheduleDetailsVO DepartmentScheduleVO = new clsDepartmentScheduleDetailsVO();
                        DepartmentScheduleVO.DepartmentID = (long)DALHelper.HandleDBNull(reader["DepartmentID"]);
                        DepartmentScheduleVO.StartTime = (DateTime)DALHelper.HandleDBNull(reader["StartTime"]);
                        DepartmentScheduleVO.EndTime = (DateTime)DALHelper.HandleDBNull(reader["EndTime"]);
                        DepartmentScheduleVO.ScheduleID = (long)DALHelper.HandleDBNull(reader["ScheduleID"]);
                        DepartmentScheduleVO.DayID = (long)DALHelper.HandleDBNull(reader["DayID"]);
                        if (DepartmentScheduleVO.DayID == 1)
                            DepartmentScheduleVO.Day = "Sunday";
                        else if (DepartmentScheduleVO.DayID == 2)
                            DepartmentScheduleVO.Day = "Monday";
                        else if (DepartmentScheduleVO.DayID == 3)
                            DepartmentScheduleVO.Day = "Tuesday";
                        else if (DepartmentScheduleVO.DayID == 4)
                            DepartmentScheduleVO.Day = "Wednesday";
                        else if (DepartmentScheduleVO.DayID == 5)
                            DepartmentScheduleVO.Day = "Thursday";
                        else if (DepartmentScheduleVO.DayID == 6)
                            DepartmentScheduleVO.Day = "Friday";
                        else if (DepartmentScheduleVO.DayID == 7)
                            DepartmentScheduleVO.Day = "Saturday";
                        BizAction.DepartmentScheduleDetailsList.Add(DepartmentScheduleVO);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return BizAction;
        }

        public override IValueObject GetDepartmentDepartmentDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDepartmentDepartmentDetailsBizActionVO BizAction = (clsGetDepartmentDepartmentDetailsBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_FillDepartmentCombobox");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitId);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizAction.MasterList == null)
                    {
                        BizAction.MasterList = new List<MasterListItem>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        BizAction.MasterList.Add(new MasterListItem(Convert.ToInt64(reader["Id"]), reader["Description"].ToString()));
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
            return BizAction;
        }
    }
}
