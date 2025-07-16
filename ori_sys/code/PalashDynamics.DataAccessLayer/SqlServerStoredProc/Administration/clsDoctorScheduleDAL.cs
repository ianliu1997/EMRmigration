using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using PalashDynamics.ValueObjects.Administration.DoctorScheduleMaster;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsDoctorScheduleDAL : clsBaseDoctorScheduleDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        #endregion

        private clsDoctorScheduleDAL()
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

        public override IValueObject AddDoctorScheduleMaster(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddDoctorScheduleMasterBizActionVO BizActionobj = valueObject as clsAddDoctorScheduleMasterBizActionVO;
            if (BizActionobj.DoctorScheduleDetails.ID == 0)
            {
                BizActionobj = AddDcotorSchedule(BizActionobj, objUserVO);
            }

            else
            {
                BizActionobj = UpdateDcotorSchedule(BizActionobj, objUserVO);

            }
            return BizActionobj;

        }


        public override IValueObject GetVisitTypeDetails(IValueObject valueObject, clsUserVO UserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            //    clsGetMasterListBizActionVO BizActionObj = (clsGetMasterListBizActionVO)valueObject;
            clsGetMasterListForVisitBizActionVO BizActionObj = (clsGetMasterListForVisitBizActionVO)valueObject;


            try
            {
                StringBuilder FilterExpression = new StringBuilder();

                if (BizActionObj.IsActive.HasValue)
                    // FilterExpression.Append("Status = '" + BizActionObj.IsActive.Value + "'");

                    if (BizActionObj.Parent != null)
                    {
                        //if (FilterExpression.Length > 0)
                        //    FilterExpression.Append(" And ");
                        //FilterExpression.Append(BizActionObj.Parent.Value.ToString() + "='" + BizActionObj.Parent.Key.ToString() + "'");
                    }


                //Take storeprocedure name as input parameter and creates DbCommand Object.
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetMasterList_New");
                //Adding MasterTableName as Input Parameter to filter record
                dbServer.AddInParameter(command, "MasterTableName", DbType.String, BizActionObj.MasterTable.ToString());
                dbServer.AddInParameter(command, "FilterExpression", DbType.String, FilterExpression.ToString());
                //dbServer.AddInParameter(command, "IsDate", DbType.Boolean, BizActionObj.IsDate.Value);



                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                //Check whether the reader contains the records
                if (reader.HasRows)
                {
                    //if masterlist instance is null then creates new instance
                    if (BizActionObj.MasterList == null)
                    {
                        BizActionObj.MasterList = new List<MasterListItem>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        //Add the object value in list
                        //BizActionObj.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString(), (DateTime?)DALHelper.HandleDate(reader["Date"])));//HandleDBNull(reader["Date"])));

                        ////Added By CDS 22/2/16


                        BizActionObj.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString(), Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFree"])), Convert.ToInt64(DALHelper.HandleDBNull(reader["FreeDaysDuration"])), Convert.ToInt64(DALHelper.HandleDBNull(reader["ConsultationVisitTypeID"]))));//HandleDBNull(reader["Date"])));

                    }
                }
                //if (!reader.IsClosed)
                //{
                //    reader.Close();
                //}
                reader.Close();

            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                BizActionObj.Error = ex.Message;  //"Error Occured";
                //logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }


            return BizActionObj;
        }

        private clsAddDoctorScheduleMasterBizActionVO AddDcotorSchedule(clsAddDoctorScheduleMasterBizActionVO BizActionobj, clsUserVO objUserVO)
        {
            DbConnection con = null;
            DbTransaction trans = null;

            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                clsDoctorScheduleVO objDoctorScheduleVO = BizActionobj.DoctorScheduleDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddDoctorSchedule");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDoctorScheduleVO.UnitID);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objDoctorScheduleVO.DoctorID);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, objDoctorScheduleVO.DepartmentID);

                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDoctorScheduleVO.ID);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionobj.DoctorScheduleDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

                foreach (var ObjDetails in objDoctorScheduleVO.DoctorScheduleDetailsList)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddDoctorScheduleDetails");

                    dbServer.AddInParameter(command1, "DoctorScheduleID", DbType.Int64, objDoctorScheduleVO.ID);
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
                BizActionobj.DoctorScheduleDetails = null;

            }
            finally
            {
                con.Close();
                con = null;
                trans = null;

            }
            return BizActionobj;
        }

        private clsAddDoctorScheduleMasterBizActionVO UpdateDcotorSchedule(clsAddDoctorScheduleMasterBizActionVO BizActionobj, clsUserVO objUserVO)
        {
            DbConnection con = null;
            DbTransaction trans = null;

            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();


                clsDoctorScheduleVO objDoctorScheduleVO = BizActionobj.DoctorScheduleDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateDoctorSchedule");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDoctorScheduleVO.UnitID);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, objDoctorScheduleVO.DepartmentID);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objDoctorScheduleVO.DoctorID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);

                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);

                dbServer.AddInParameter(command, "ID", DbType.Int64, objDoctorScheduleVO.ID);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionobj.SuccessStatus);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionobj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");


                if (objDoctorScheduleVO.DoctorScheduleDetailsList != null && objDoctorScheduleVO.DoctorScheduleDetailsList.Count > 0)
                {
                    DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_DeleteDoctorScheduleDetails");

                    dbServer.AddInParameter(command3, "DoctorScheduleID", DbType.Int64, objDoctorScheduleVO.ID);
                    int intStatus2 = dbServer.ExecuteNonQuery(command3, trans);
                }
                foreach (var ObjDetails in objDoctorScheduleVO.DoctorScheduleDetailsList)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddDoctorScheduleDetails");

                    dbServer.AddInParameter(command1, "DoctorScheduleID", DbType.Int64, objDoctorScheduleVO.ID);
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
                BizActionobj.DoctorScheduleDetails = null;

            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }
            return BizActionobj;
        }

        public override IValueObject GetDoctorScheduleList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorScheduleMasterListBizActionVO BizActionObj = (clsGetDoctorScheduleMasterListBizActionVO)valueObject;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorScheduleBySearchCriteria");
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");


                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizActionObj.DepartmentID);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.DoctorID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DoctorScheduleList == null)
                        BizActionObj.DoctorScheduleList = new List<clsDoctorScheduleVO>();

                    while (reader.Read())
                    {
                        clsDoctorScheduleVO objDoctorScheduleVO = new clsDoctorScheduleVO();
                        objDoctorScheduleVO.ID = (long)reader["ID"];
                        objDoctorScheduleVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        objDoctorScheduleVO.UnitName = (string)DALHelper.HandleDBNull(reader["UnitName"]);
                        objDoctorScheduleVO.DepartmentID = (long)DALHelper.HandleDBNull(reader["DepartmentID"]);
                        objDoctorScheduleVO.DepartmentName = (string)DALHelper.HandleDBNull(reader["DepartmentName"]);
                        objDoctorScheduleVO.DoctorID = (long)DALHelper.HandleDBNull(reader["DoctorID"]);
                        objDoctorScheduleVO.DoctorName = (string)DALHelper.HandleDBNull(reader["DoctorName"]);
                        BizActionObj.DoctorScheduleList.Add(objDoctorScheduleVO);
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



        public override IValueObject GetDoctorSchedule(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorScheduleMasterBizActionVO BizActionObj = (clsGetDoctorScheduleMasterBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_CheckForScheduleExistance");
                DbDataReader reader;

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizActionObj.DepartmentID);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.DoctorID);

                //dbServer.AddInParameter(command, "DayID", DbType.String, BizActionObj.DayID);
                //dbServer.AddInParameter(command, "Schedule1_StartTime", DbType.String, BizActionObj.Schedule1_StartTime);
                //dbServer.AddInParameter(command, "Schedule1_EndTime", DbType.String, BizActionObj.Schedule1_EndTime);
                //dbServer.AddInParameter(command, "Schedule2_StartTime", DbType.String, BizActionObj.Schedule2_StartTime);
                //dbServer.AddInParameter(command, "Schedule2_EndTime", DbType.String, BizActionObj.Schedule2_EndTime);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    BizActionObj.SuccessStatus = true;

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





        ///New DoctorSchedule
        ///Date:11 - Aug -2011

        public override IValueObject GetDoctorScheduleDetailsList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorScheduleListBizActionVO BizActionObj = (clsGetDoctorScheduleListBizActionVO)valueObject;

            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorScheduleList");

                dbServer.AddInParameter(command, "DoctorScheduleID ", DbType.Int64, BizActionObj.DoctorScheduleID);


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DoctorScheduleList == null)
                        BizActionObj.DoctorScheduleList = new List<clsDoctorScheduleDetailsVO>();

                    while (reader.Read())
                    {
                        clsDoctorScheduleDetailsVO objDoctorScheduleVO = new clsDoctorScheduleDetailsVO();
                        objDoctorScheduleVO.ID = (long)reader["ID"];
                        objDoctorScheduleVO.DoctorScheduleID = (long)reader["DoctorScheduleID"];
                        objDoctorScheduleVO.DayID = (long)DALHelper.HandleDBNull(reader["DayID"]);
                        if (objDoctorScheduleVO.DayID == 1)
                            objDoctorScheduleVO.Day = "Sunday";
                        else if (objDoctorScheduleVO.DayID == 2)
                            objDoctorScheduleVO.Day = "Monday";
                        else if (objDoctorScheduleVO.DayID == 3)
                            objDoctorScheduleVO.Day = "Tuesday";
                        else if (objDoctorScheduleVO.DayID == 4)
                            objDoctorScheduleVO.Day = "Wednesday";
                        else if (objDoctorScheduleVO.DayID == 5)
                            objDoctorScheduleVO.Day = "Thursday";
                        else if (objDoctorScheduleVO.DayID == 6)
                            objDoctorScheduleVO.Day = "Friday";
                        else if (objDoctorScheduleVO.DayID == 7)
                            objDoctorScheduleVO.Day = "Saturday";

                        objDoctorScheduleVO.ScheduleID = (long)DALHelper.HandleDBNull(reader["ScheduleID"]);
                        objDoctorScheduleVO.Schedule = (string)DALHelper.HandleDBNull(reader["Schedule"]);
                        objDoctorScheduleVO.StartTime = (DateTime)DALHelper.HandleDBNull(reader["StartTime"]);
                        objDoctorScheduleVO.EndTime = (DateTime)DALHelper.HandleDBNull(reader["EndTime"]);
                        BizActionObj.DoctorScheduleList.Add(objDoctorScheduleVO);
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

        //ROHINEE
        public override IValueObject GetDoctorScheduleDetailsListByID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorScheduleListByIDBizActionVO BizActionObj = (clsGetDoctorScheduleListByIDBizActionVO)valueObject;

            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetScheduleByDoctorId");

                dbServer.AddInParameter(command, "DoctorScheduleID ", DbType.Int64, BizActionObj.DoctorScheduleID);
                dbServer.AddInParameter(command, "DayID ", DbType.Int64, BizActionObj.DayID);
                dbServer.AddInParameter(command, "ID ", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "StartTime ", DbType.DateTime, BizActionObj.StartTime);
                dbServer.AddInParameter(command, "EndTime ", DbType.DateTime, BizActionObj.EndTime);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DoctorScheduleListForDoctorID == null)
                        BizActionObj.DoctorScheduleListForDoctorID = new List<clsDoctorScheduleDetailsVO>();

                    while (reader.Read())
                    {
                        clsDoctorScheduleDetailsVO objDoctorScheduleVO = new clsDoctorScheduleDetailsVO();
                        objDoctorScheduleVO.ID = (long)reader["ID"];
                        objDoctorScheduleVO.DoctorScheduleID = (long)reader["DoctorScheduleID"];
                        objDoctorScheduleVO.DayID = (long)DALHelper.HandleDBNull(reader["DayID"]);
                        //objDoctorScheduleVO.ScheduleID = (long)DALHelper.HandleDBNull(reader["ScheduleID"]);
                        //objDoctorScheduleVO.Schedule = (string)DALHelper.HandleDBNull(reader["Schedule"]);
                        objDoctorScheduleVO.StartTime = (DateTime)DALHelper.HandleDBNull(reader["StartTime"]);
                        objDoctorScheduleVO.EndTime = (DateTime)DALHelper.HandleDBNull(reader["EndTime"]);
                        BizActionObj.DoctorScheduleListForDoctorID.Add(objDoctorScheduleVO);
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
            clsCheckTimeForScheduleExistanceBizActionVO BizAction = (clsCheckTimeForScheduleExistanceBizActionVO)valueObject;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_CheckDoctorSchedule");

                //dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                //dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizAction.DepartmentID);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizAction.DoctorID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizAction.Details == null)
                        BizAction.Details = new List<clsDoctorScheduleDetailsVO>();

                    while (reader.Read())
                    {
                        clsDoctorScheduleDetailsVO DoctorScheduleVO = new clsDoctorScheduleDetailsVO();
                        DoctorScheduleVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        DoctorScheduleVO.DepartmentID = (long)DALHelper.HandleDBNull(reader["DepartmentID"]);
                        DoctorScheduleVO.DoctorID = (long)DALHelper.HandleDBNull(reader["DoctorID"]);
                        DoctorScheduleVO.DayID = (long)DALHelper.HandleDBNull(reader["DayID"]);
                        DoctorScheduleVO.ScheduleID = (long)DALHelper.HandleDBNull(reader["ScheduleID"]);
                        DoctorScheduleVO.StartTime = (DateTime)DALHelper.HandleDBNull(reader["StartTime"]);
                        DoctorScheduleVO.EndTime = (DateTime)DALHelper.HandleDBNull(reader["EndTime"]);

                        BizAction.SuccessStatus = true;

                        BizAction.Details.Add(DoctorScheduleVO);



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


        //public override IValueObject GetDoctorScheduleTime(IValueObject valueObject, clsUserVO objUserVO)
        //{
        //    GetDoctorScheduleTimeVO BizAction = (GetDoctorScheduleTimeVO)valueObject;

        //    try
        //    {
        //        if (BizAction.AppointmentType == Convert.ToInt64(AppointmentType.Doctor))
        //        {
        //            DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorTime");

        //            dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizAction.UnitId);
        //            dbServer.AddInParameter(command, "DepartmentId", DbType.Int64, BizAction.DepartmentId);
        //            dbServer.AddInParameter(command, "DoctorId", DbType.Int64, BizAction.DoctorId);
        //            dbServer.AddInParameter(command, "Date", DbType.DateTime, BizAction.Date);
        //            DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

        //            if (reader.HasRows)
        //            {
        //                if (BizAction.DoctorScheduleDetailsList == null)
        //                    BizAction.DoctorScheduleDetailsList = new List<clsDoctorScheduleDetailsVO>();

        //                while (reader.Read())
        //                {
        //                    clsDoctorScheduleDetailsVO DoctorScheduleVO = new clsDoctorScheduleDetailsVO();
        //                    DoctorScheduleVO.DoctorID = (long)DALHelper.HandleDBNull(reader["DoctorID"]);
        //                    DoctorScheduleVO.StartTime = (DateTime)DALHelper.HandleDBNull(reader["StartTime"]);
        //                    DoctorScheduleVO.EndTime = (DateTime)DALHelper.HandleDBNull(reader["EndTime"]);
        //                    DoctorScheduleVO.ScheduleID = (long)DALHelper.HandleDBNull(reader["ScheduleID"]);
        //                    DoctorScheduleVO.DayID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DayID"]));
        //                    if (DoctorScheduleVO.DayID == 1)
        //                        DoctorScheduleVO.Day = "Sunday";
        //                    else if (DoctorScheduleVO.DayID == 2)
        //                        DoctorScheduleVO.Day = "Monday";
        //                    else if (DoctorScheduleVO.DayID == 3)
        //                        DoctorScheduleVO.Day = "Tuesday";
        //                    else if (DoctorScheduleVO.DayID == 4)
        //                        DoctorScheduleVO.Day = "Wednesday";
        //                    else if (DoctorScheduleVO.DayID == 5)
        //                        DoctorScheduleVO.Day = "Thursday";
        //                    else if (DoctorScheduleVO.DayID == 6)
        //                        DoctorScheduleVO.Day = "Friday";
        //                    else if (DoctorScheduleVO.DayID == 7)
        //                        DoctorScheduleVO.Day = "Saturday";
        //                    BizAction.DoctorScheduleDetailsList.Add(DoctorScheduleVO);
        //                }

        //                reader.Close();
        //            }
        //        }
        //        else if (BizAction.AppointmentType == Convert.ToInt64(AppointmentType.Department))
        //        {
        //            DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDepartmentSchedule");

        //            dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizAction.UnitId);
        //            dbServer.AddInParameter(command, "DepartmentId", DbType.Int64, BizAction.DepartmentId);

        //            DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

        //            if (reader.HasRows)
        //            {
        //                if (BizAction.DoctorScheduleDetailsList == null)
        //                    BizAction.DoctorScheduleDetailsList = new List<clsDoctorScheduleDetailsVO>();

        //                while (reader.Read())
        //                {
        //                    clsDoctorScheduleDetailsVO DoctorScheduleVO = new clsDoctorScheduleDetailsVO();
        //                    DoctorScheduleVO.DepartmentID = (long)DALHelper.HandleDBNull(reader["DepartmentID"]);
        //                    DoctorScheduleVO.StartTime = (DateTime)DALHelper.HandleDBNull(reader["StartTime"]);
        //                    DoctorScheduleVO.EndTime = (DateTime)DALHelper.HandleDBNull(reader["EndTime"]);
        //                    DoctorScheduleVO.ScheduleID = (long)DALHelper.HandleDBNull(reader["ScheduleID"]);
        //                    DoctorScheduleVO.DayID = (long)DALHelper.HandleDBNull(reader["DayID"]);
        //                    if (DoctorScheduleVO.DayID == 1)
        //                        DoctorScheduleVO.Day = "Sunday";
        //                    else if (DoctorScheduleVO.DayID == 2)
        //                        DoctorScheduleVO.Day = "Monday";
        //                    else if (DoctorScheduleVO.DayID == 3)
        //                        DoctorScheduleVO.Day = "Tuesday";
        //                    else if (DoctorScheduleVO.DayID == 4)
        //                        DoctorScheduleVO.Day = "Wednesday";
        //                    else if (DoctorScheduleVO.DayID == 5)
        //                        DoctorScheduleVO.Day = "Thursday";
        //                    else if (DoctorScheduleVO.DayID == 6)
        //                        DoctorScheduleVO.Day = "Friday";
        //                    else if (DoctorScheduleVO.DayID == 7)
        //                        DoctorScheduleVO.Day = "Saturday";
        //                    BizAction.DoctorScheduleDetailsList.Add(DoctorScheduleVO);
        //                }
        //                reader.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //    }
        //    return BizAction;


        //}


        public override IValueObject GetDoctorScheduleTime(IValueObject valueObject, clsUserVO objUserVO)
        {
            GetDoctorScheduleTimeVO BizAction = (GetDoctorScheduleTimeVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorTime");

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizAction.UnitId);
                dbServer.AddInParameter(command, "DepartmentId", DbType.Int64, BizAction.DepartmentId);
                dbServer.AddInParameter(command, "DoctorId", DbType.Int64, BizAction.DoctorId);

                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizAction.Date);      //For New Doctor Schedule Changes modified on 29052018 

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizAction.DoctorScheduleDetailsList == null)
                        BizAction.DoctorScheduleDetailsList = new List<clsDoctorScheduleDetailsVO>();

                    while (reader.Read())
                    {
                        clsDoctorScheduleDetailsVO DoctorScheduleVO = new clsDoctorScheduleDetailsVO();
                        DoctorScheduleVO.DoctorID = (long)DALHelper.HandleDBNull(reader["DoctorID"]);
                        DoctorScheduleVO.StartTime = (DateTime)DALHelper.HandleDBNull(reader["StartTime"]);
                        DoctorScheduleVO.EndTime = (DateTime)DALHelper.HandleDBNull(reader["EndTime"]);
                        DoctorScheduleVO.ScheduleID = (long)DALHelper.HandleDBNull(reader["ScheduleID"]);
                        DoctorScheduleVO.DayID = (long)DALHelper.HandleDBNull(reader["DayID"]);
                        if (DoctorScheduleVO.DayID == 1)
                            DoctorScheduleVO.Day = "Sunday";
                        else if (DoctorScheduleVO.DayID == 2)
                            DoctorScheduleVO.Day = "Monday";
                        else if (DoctorScheduleVO.DayID == 3)
                            DoctorScheduleVO.Day = "Tuesday";
                        else if (DoctorScheduleVO.DayID == 4)
                            DoctorScheduleVO.Day = "Wednesday";
                        else if (DoctorScheduleVO.DayID == 5)
                            DoctorScheduleVO.Day = "Thursday";
                        else if (DoctorScheduleVO.DayID == 6)
                            DoctorScheduleVO.Day = "Friday";
                        else if (DoctorScheduleVO.DayID == 7)
                            DoctorScheduleVO.Day = "Saturday";
                        BizAction.DoctorScheduleDetailsList.Add(DoctorScheduleVO);
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

        public override IValueObject GetDoctorScheduleWise(IValueObject valueObject, clsUserVO objUserVO)
        {
            GetDoctorScheduleWiseVO BizAction = (GetDoctorScheduleWiseVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorSchedulewise");

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizAction.UnitId);
                dbServer.AddInParameter(command, "DepartmentId", DbType.Int64, BizAction.DepartmentId);
                dbServer.AddInParameter(command, "Day", DbType.Int64, BizAction.Day);

                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizAction.Date);      //For New Doctor Schedule Changes modified on 29052018 

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizAction.DoctorScheduleDetailsList == null)
                        BizAction.DoctorScheduleDetailsList = new List<clsDoctorScheduleDetailsVO>();

                    while (reader.Read())
                    {
                        clsDoctorScheduleDetailsVO DoctorScheduleVO = new clsDoctorScheduleDetailsVO();
                        DoctorScheduleVO.DoctorID = (long)DALHelper.HandleDBNull(reader["DoctorID"]);
                        DoctorScheduleVO.StartTime = (DateTime)DALHelper.HandleDBNull(reader["StartTime"]);
                        DoctorScheduleVO.EndTime = (DateTime)DALHelper.HandleDBNull(reader["EndTime"]);
                        DoctorScheduleVO.ScheduleID = (long)DALHelper.HandleDBNull(reader["ScheduleID"]);
                        DoctorScheduleVO.DayID = (long)DALHelper.HandleDBNull(reader["DayID"]);
                        DoctorScheduleVO.DoctorName = (string)DALHelper.HandleDBNull(reader["DoctorName"]);
                        if (DoctorScheduleVO.DayID == 1)
                            DoctorScheduleVO.Day = "Sunday";
                        else if (DoctorScheduleVO.DayID == 2)
                            DoctorScheduleVO.Day = "Monday";
                        else if (DoctorScheduleVO.DayID == 3)
                            DoctorScheduleVO.Day = "Tuesday";
                        else if (DoctorScheduleVO.DayID == 4)
                            DoctorScheduleVO.Day = "Wednesday";
                        else if (DoctorScheduleVO.DayID == 5)
                            DoctorScheduleVO.Day = "Thursday";
                        else if (DoctorScheduleVO.DayID == 6)
                            DoctorScheduleVO.Day = "Friday";
                        else if (DoctorScheduleVO.DayID == 7)
                            DoctorScheduleVO.Day = "Saturday";
                        BizAction.DoctorScheduleDetailsList.Add(DoctorScheduleVO);
                    }

                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }
            return BizAction;
        }

        public override IValueObject GetDoctorDepartmentUnitList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorDepartmentUnitListBizActionVO BizActionObj = (clsGetDoctorDepartmentUnitListBizActionVO)valueObject;     // added on 13032018 for Doctor Schedule Change
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorDepartmentUnitList");

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizActionObj.DepartmentID);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.DoctorID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DoctorDepartmentUnitList == null)
                        BizActionObj.DoctorDepartmentUnitList = new List<clsDoctorDepartmentUnitListVO>();

                    while (reader.Read())
                    {
                        clsDoctorDepartmentUnitListVO objDoctorDepartmentUnitVO = new clsDoctorDepartmentUnitListVO();
                        objDoctorDepartmentUnitVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objDoctorDepartmentUnitVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objDoctorDepartmentUnitVO.UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"]));
                        objDoctorDepartmentUnitVO.DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"]));
                        objDoctorDepartmentUnitVO.DepartmentName = Convert.ToString(DALHelper.HandleDBNull(reader["DepartmentName"]));
                        objDoctorDepartmentUnitVO.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                        objDoctorDepartmentUnitVO.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                        objDoctorDepartmentUnitVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        BizActionObj.DoctorDepartmentUnitList.Add(objDoctorDepartmentUnitVO);
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

        public override IValueObject GetDoctorScheduleListNew(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorScheduleMasterListBizActionVO BizActionObj = (clsGetDoctorScheduleMasterListBizActionVO)valueObject;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorScheduleBySearchCriteriaNew");
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");


                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizActionObj.DepartmentID);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.DoctorID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DoctorScheduleList == null)
                        BizActionObj.DoctorScheduleList = new List<clsDoctorScheduleVO>();

                    while (reader.Read())
                    {
                        clsDoctorScheduleVO objDoctorScheduleVO = new clsDoctorScheduleVO();
                        objDoctorScheduleVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objDoctorScheduleVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objDoctorScheduleVO.UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"]));
                        objDoctorScheduleVO.DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"]));
                        objDoctorScheduleVO.DepartmentName = Convert.ToString(DALHelper.HandleDBNull(reader["DepartmentName"]));
                        objDoctorScheduleVO.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                        objDoctorScheduleVO.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                        objDoctorScheduleVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

                        objDoctorScheduleVO.StartTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["StartTime"]));
                        objDoctorScheduleVO.EndTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["EndTime"]));
                        objDoctorScheduleVO.ScheduleType = Convert.ToString(DALHelper.HandleDBNull(reader["ScheduleType"]));

                        BizActionObj.DoctorScheduleList.Add(objDoctorScheduleVO);
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

        public override IValueObject AddDoctorScheduleMasterNew(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddDoctorScheduleMasterBizActionVO BizActionobj = valueObject as clsAddDoctorScheduleMasterBizActionVO;
            if (BizActionobj.DoctorScheduleDetails.ID == 0)
            {
                BizActionobj = AddDcotorScheduleNew(BizActionobj, objUserVO);
            }

            else
            {
                BizActionobj = UpdateDcotorScheduleNew(BizActionobj, objUserVO);

            }
            return BizActionobj;

            ///////////////////////////////////////////////////


        }

        // added on 21032018 for New Doctor Schedule
        private clsAddDoctorScheduleMasterBizActionVO AddDcotorScheduleNew(clsAddDoctorScheduleMasterBizActionVO BizActionobj, clsUserVO objUserVO)
        {
            //clsAddDoctorScheduleMasterBizActionVO BizActionobj = (clsAddDoctorScheduleMasterBizActionVO)valueObject;

            DbConnection con = null;
            DbTransaction trans = null;

            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                clsDoctorScheduleVO objDoctorScheduleVO = BizActionobj.DoctorScheduleDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddDoctorScheduleRoster");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDoctorScheduleVO.UnitID);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objDoctorScheduleVO.DoctorID);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, objDoctorScheduleVO.DepartmentID);

                clsDoctorScheduleDetailsVO ObjDetails = objDoctorScheduleVO.DoctorScheduleDetailsListItem;

                dbServer.AddInParameter(command, "StartTime", DbType.DateTime, ObjDetails.StartTime);
                dbServer.AddInParameter(command, "EndTime", DbType.DateTime, ObjDetails.EndTime);
                dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, ObjDetails.ScheduleID);
                dbServer.AddInParameter(command, "ScheduleType", DbType.String, ObjDetails.Schedule);

                dbServer.AddInParameter(command, "DayIDnew", DbType.String, ObjDetails.DayIDnew);    //added on 14032018 for New Doctor Schedule

                dbServer.AddInParameter(command, "IsDayNo", DbType.Boolean, ObjDetails.IsDayNo);    //added on 23032018 for New Doctor Schedule

                if (ObjDetails.MonthDayNo > 0)
                    dbServer.AddInParameter(command, "MonthDayNo", DbType.Int64, ObjDetails.MonthDayNo);    //added on 14032018 for New Doctor Schedule

                dbServer.AddInParameter(command, "MonthWeekNoID", DbType.Int64, ObjDetails.MonthWeekNoID);    //added on 23032018 for New Doctor Schedule
                dbServer.AddInParameter(command, "MonthWeekDayID", DbType.Int64, ObjDetails.MonthWeekDayID);    //added on 23032018 for New Doctor Schedule

                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDoctorScheduleVO.ID);
                dbServer.AddParameter(command, "SuccessStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionobj.SuccessStatus);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionobj.DoctorScheduleDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionobj.SuccessStatus = (int)dbServer.GetParameterValue(command, "SuccessStatus");

                if (BizActionobj.SuccessStatus == 10) throw new Exception();    //added on 29052018 for New Doctor Schedule

                #region Commented

                //clsDoctorScheduleVO objDoctorScheduleVO = BizActionobj.DoctorScheduleDetails;
                //DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddDoctorSchedule");

                //dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDoctorScheduleVO.UnitID);
                //dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objDoctorScheduleVO.DoctorID);
                //dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, objDoctorScheduleVO.DepartmentID);

                //dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                //dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);

                //dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                //dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                //dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                //dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);

                //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDoctorScheduleVO.ID);
                //int intStatus = dbServer.ExecuteNonQuery(command, trans);
                //BizActionobj.DoctorScheduleDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

                //foreach (var ObjDetails in objDoctorScheduleVO.DoctorScheduleDetailsList)
                //{
                //    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddDoctorScheduleDetails");

                //    dbServer.AddInParameter(command1, "DoctorScheduleID", DbType.Int64, objDoctorScheduleVO.ID);
                //    dbServer.AddInParameter(command1, "DayID", DbType.Int64, ObjDetails.DayID);
                //    dbServer.AddInParameter(command1, "ScheduleID", DbType.Int64, ObjDetails.ScheduleID);
                //    dbServer.AddInParameter(command1, "StartTime", DbType.DateTime, ObjDetails.StartTime);
                //    dbServer.AddInParameter(command1, "EndTime", DbType.DateTime, ObjDetails.EndTime);
                //    dbServer.AddInParameter(command1, "ApplyToAllDay", DbType.Boolean, ObjDetails.ApplyToAllDay);
                //    dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);

                //    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDetails.ID);

                //    int iStatus = dbServer.ExecuteNonQuery(command1, trans);
                //    ObjDetails.ID = (long)dbServer.GetParameterValue(command1, "ID");


                //}

                #endregion

                trans.Commit();
                BizActionobj.SuccessStatus = 0;


            }
            catch (Exception ex)
            {
                //   throw;
                if (BizActionobj.SuccessStatus == 10) { }   //added on 29052018 for New Doctor Schedule
                else
                    BizActionobj.SuccessStatus = -1;
                trans.Rollback();
                BizActionobj.DoctorScheduleDetails = null;

            }
            finally
            {
                con.Close();
                con = null;
                trans = null;

            }
            return BizActionobj;
        }

        // added on 21032018 for New Doctor Schedule
        private clsAddDoctorScheduleMasterBizActionVO UpdateDcotorScheduleNew(clsAddDoctorScheduleMasterBizActionVO BizActionobj, clsUserVO objUserVO)
        {
            DbConnection con = null;
            DbTransaction trans = null;

            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();


                clsDoctorScheduleVO objDoctorScheduleVO = BizActionobj.DoctorScheduleDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateDoctorScheduleRoster");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDoctorScheduleVO.UnitID);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objDoctorScheduleVO.DoctorID);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, objDoctorScheduleVO.DepartmentID);

                clsDoctorScheduleDetailsVO ObjDetails = objDoctorScheduleVO.DoctorScheduleDetailsListItem;

                dbServer.AddInParameter(command, "StartTime", DbType.DateTime, ObjDetails.StartTime);
                dbServer.AddInParameter(command, "EndTime", DbType.DateTime, ObjDetails.EndTime);
                dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, ObjDetails.ScheduleID);
                dbServer.AddInParameter(command, "ScheduleType", DbType.String, ObjDetails.Schedule);

                dbServer.AddInParameter(command, "DayIDnew", DbType.String, ObjDetails.DayIDnew);    //added on 14032018 for New Doctor Schedule

                dbServer.AddInParameter(command, "IsDayNo", DbType.Boolean, ObjDetails.IsDayNo);    //added on 23032018 for New Doctor Schedule

                if (ObjDetails.MonthDayNo > 0)
                    dbServer.AddInParameter(command, "MonthDayNo", DbType.Int64, ObjDetails.MonthDayNo);    //added on 14032018 for New Doctor Schedule

                dbServer.AddInParameter(command, "MonthWeekNoID", DbType.Int64, ObjDetails.MonthWeekNoID);    //added on 23032018 for New Doctor Schedule
                dbServer.AddInParameter(command, "MonthWeekDayID", DbType.Int64, ObjDetails.MonthWeekDayID);    //added on 23032018 for New Doctor Schedule

                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);

                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);

                dbServer.AddInParameter(command, "ID", DbType.Int64, objDoctorScheduleVO.ID);
                dbServer.AddParameter(command, "SuccessStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionobj.SuccessStatus);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionobj.SuccessStatus = (int)dbServer.GetParameterValue(command, "SuccessStatus");

                if (BizActionobj.SuccessStatus == 10) throw new Exception();    //added on 29052018 for New Doctor Schedule

                # region Commented

                //if (objDoctorScheduleVO.DoctorScheduleDetailsList != null && objDoctorScheduleVO.DoctorScheduleDetailsList.Count > 0)
                //{
                //    DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_DeleteDoctorScheduleDetails");

                //    dbServer.AddInParameter(command3, "DoctorScheduleID", DbType.Int64, objDoctorScheduleVO.ID);
                //    int intStatus2 = dbServer.ExecuteNonQuery(command3, trans);
                //}
                //foreach (var ObjDetails in objDoctorScheduleVO.DoctorScheduleDetailsList)
                //{
                //    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddDoctorScheduleDetails");

                //    dbServer.AddInParameter(command1, "DoctorScheduleID", DbType.Int64, objDoctorScheduleVO.ID);
                //    dbServer.AddInParameter(command1, "DayID", DbType.Int64, ObjDetails.DayID);
                //    dbServer.AddInParameter(command1, "ScheduleID", DbType.Int64, ObjDetails.ScheduleID);
                //    dbServer.AddInParameter(command1, "StartTime", DbType.DateTime, ObjDetails.StartTime);
                //    dbServer.AddInParameter(command1, "EndTime", DbType.DateTime, ObjDetails.EndTime);
                //    dbServer.AddInParameter(command1, "ApplyToAllDay", DbType.Boolean, ObjDetails.ApplyToAllDay);
                //    dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);

                //    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDetails.ID);

                //    int iStatus = dbServer.ExecuteNonQuery(command1, trans);
                //    ObjDetails.ID = (long)dbServer.GetParameterValue(command1, "ID");


                //}

                #endregion

                trans.Commit();
                BizActionobj.SuccessStatus = 0;

            }
            catch (Exception ex)
            {
                //throw;
                if (BizActionobj.SuccessStatus == 10) { }   //added on 29052018 for New Doctor Schedule
                else
                    BizActionobj.SuccessStatus = -1;
                trans.Rollback();
                BizActionobj.DoctorScheduleDetails = null;

            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }
            return BizActionobj;
        }


        // added on 21032018 for New Doctor Schedule
        public override IValueObject GetDoctorScheduleDetailsListNew(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorScheduleListBizActionVO BizActionObj = (clsGetDoctorScheduleListBizActionVO)valueObject;

            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorScheduleListNew");

                dbServer.AddInParameter(command, "DoctorScheduleID ", DbType.Int64, BizActionObj.DoctorScheduleID);


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DoctorScheduleList == null)
                        BizActionObj.DoctorScheduleList = new List<clsDoctorScheduleDetailsVO>();

                    while (reader.Read())
                    {
                        clsDoctorScheduleDetailsVO objDoctorScheduleVO = new clsDoctorScheduleDetailsVO();

                        objDoctorScheduleVO.ID = (long)reader["ID"];
                        objDoctorScheduleVO.DoctorScheduleID = (long)reader["DoctorScheduleID"];

                        //objDoctorScheduleVO.DayID = (long)DALHelper.HandleDBNull(reader["DayID"]);

                        //if (objDoctorScheduleVO.DayID == 1)
                        //    objDoctorScheduleVO.Day = "Sunday";
                        //else if (objDoctorScheduleVO.DayID == 2)
                        //    objDoctorScheduleVO.Day = "Monday";
                        //else if (objDoctorScheduleVO.DayID == 3)
                        //    objDoctorScheduleVO.Day = "Tuesday";
                        //else if (objDoctorScheduleVO.DayID == 4)
                        //    objDoctorScheduleVO.Day = "Wednesday";
                        //else if (objDoctorScheduleVO.DayID == 5)
                        //    objDoctorScheduleVO.Day = "Thursday";
                        //else if (objDoctorScheduleVO.DayID == 6)
                        //    objDoctorScheduleVO.Day = "Friday";
                        //else if (objDoctorScheduleVO.DayID == 7)
                        //    objDoctorScheduleVO.Day = "Saturday";

                        objDoctorScheduleVO.ScheduleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ScheduleID"]));
                        objDoctorScheduleVO.Schedule = Convert.ToString(DALHelper.HandleDBNull(reader["Schedule"]));
                        objDoctorScheduleVO.StartTime = (DateTime)DALHelper.HandleDBNull(reader["StartTime"]);
                        objDoctorScheduleVO.EndTime = (DateTime)DALHelper.HandleDBNull(reader["EndTime"]);
                        objDoctorScheduleVO.DayIDnew = Convert.ToString(DALHelper.HandleDBNull(reader["WeekPaternIDS"]));

                        objDoctorScheduleVO.IsDayNo = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDayNo"]));                 // added on 23032018 for new Doctor Schedule
                        objDoctorScheduleVO.MonthDayNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["MonthDayNo"]));             // added on 23032018 for new Doctor Schedule
                        objDoctorScheduleVO.MonthWeekNoID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MonthWeekNoID"]));       // added on 23032018 for new Doctor Schedule
                        objDoctorScheduleVO.MonthWeekDayID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MonthWeekDayID"]));     // added on 23032018 for new Doctor Schedule

                        BizActionObj.DoctorScheduleList.Add(objDoctorScheduleVO);
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

    }
}


