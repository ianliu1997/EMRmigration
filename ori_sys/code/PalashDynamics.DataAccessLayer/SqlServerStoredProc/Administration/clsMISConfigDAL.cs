using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects.Administration.MISConfiguration;
using System.Data.Common;
using System.Data;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    class clsMISConfigDAL : clsBaseMISConfigDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        public clsMISConfigDAL()
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
            catch (Exception ex)
            {
                throw;
            }
        }

        public override ValueObjects.IValueObject GetMISConfig(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO UserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetMISConfigBizActionVO objConfig = valueObject as clsGetMISConfigBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetConfigMIS");
                dbServer.AddInParameter(command, "ID", DbType.String, objConfig.ID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (objConfig.GetMISConfig == null)
                            objConfig.GetMISConfig = new clsMISConfigurationVO();

                        objConfig.GetMISConfig.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"]));
                        objConfig.GetMISConfig.ClinicId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClinicId"]));
                        objConfig.GetMISConfig.ScheduleName = Convert.ToString(DALHelper.HandleDBNull(reader["ScheduleName"]));
                        objConfig.GetMISConfig.MISReportFormatId = Convert.ToInt64(DALHelper.HandleDBNull(reader["MISReportFormatId"]));
                        objConfig.GetMISConfig.ScheduleOn = Convert.ToInt64(DALHelper.HandleDBNull(reader["ScheduledOn"]));
                        objConfig.GetMISConfig.ScheduleDetails = Convert.ToString(DALHelper.HandleDBNull(reader["ScheduleDetails"]));
                        objConfig.GetMISConfig.ScheduleTime = Convert.ToDateTime(DALHelper.HandleDate(reader["ScheduleTime"]));
                        objConfig.GetMISConfig.ScheduleStartDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ScheduleStartDate"]));
                        objConfig.GetMISConfig.ScheduleEndDate = (DateTime?)DALHelper.HandleDate(reader["ScheduleEndDate"]);
                    }
                }

                reader.NextResult();
                if (reader.HasRows)
                {
                    objConfig.GetConfigReport = new List<clsGetMISReportTypeVO>();
                    while (reader.Read())
                    {
                        clsGetMISReportTypeVO objGetReport = new clsGetMISReportTypeVO();
                        objGetReport.Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Sys_MISReportID"]));
                        objGetReport.MISTypeID = reader["MIStypeId"].HandleDBNull() == null ? 0 : Convert.ToInt64(reader["MIStypeId"]);
                        objGetReport.Status = reader["Status"].HandleDBNull() == null ? false : Convert.ToBoolean(reader["Status"]); //(bool)DALHelper.HandleDBNull(reader["Status"]);
                        objGetReport.Description = Convert.ToString(DALHelper.HandleDBNull(reader["ReportName"]));
                        objConfig.GetConfigReport.Add(objGetReport);
                    }
                }

                reader.NextResult();
                if (reader.HasRows)
                {
                    objConfig.GetConfigStaff = new List<clsGetMISStaffVO>();
                    while (reader.Read())
                    {
                        clsGetMISStaffVO objGetStaff = new clsGetMISStaffVO();
                        objGetStaff.StaffTypeId = reader["StaffTypeId"].HandleDBNull() == null ? 0 : Convert.ToInt64(reader["StaffTypeId"]);
                        objGetStaff.SelectStaffTypeId = reader["SelectStaffTypeId"].HandleDBNull() == null ? 0 : Convert.ToInt64(reader["SelectStaffTypeId"]);
                        objGetStaff.Id = reader["StaffId"].HandleDBNull() == null ? 0 : Convert.ToInt64(reader["StaffId"]);
                        objGetStaff.Status = reader["Status"].HandleDBNull() == null ? false : Convert.ToBoolean(reader["Status"]); //(bool)DALHelper.HandleDBNull(reader["Status"]);
                        objGetStaff.Name = Convert.ToString(DALHelper.HandleDBNull(reader["StaffName"]));
                        objConfig.GetConfigStaff.Add(objGetStaff);
                    }
                }


                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }

            return objConfig;
        }


        public override ValueObjects.IValueObject GetMISConfigList(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO UserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsgetMISconfigListBizActionVO objConfig = valueObject as clsgetMISconfigListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetConfigMISList");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objConfig.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objConfig.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objConfig.MaximumRows);
                dbServer.AddInParameter(command, "FilterbyClinic", DbType.String, objConfig.FilterbyClinic);
                dbServer.AddInParameter(command, "FilterbyMISType", DbType.String, objConfig.FilterbyMISType);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (objConfig.GetMISConfig == null)
                            objConfig.GetMISConfig = new List<clsMISConfigurationVO>();

                        clsMISConfigurationVO Item = new clsMISConfigurationVO();
                        Item.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        Item.ClinicId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClinicId"]));
                        Item.ScheduleName = Convert.ToString(DALHelper.HandleDBNull(reader["ScheduleName"]));
                        Item.ClinicName = Convert.ToString(DALHelper.HandleDBNull(reader["ClinicName"]));
                        Item.MISTypeName = Convert.ToString(DALHelper.HandleDBNull(reader["MISType"]));
                        Item.MISTypeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["MISTypeId"]));
                        Item.ClinicCode = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        Item.ConfigDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ConfigDate"]));
                        Item.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objConfig.GetMISConfig.Add(Item);
                    }
                }

                reader.NextResult();
                objConfig.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");


                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }

            return objConfig;
        }



        public override ValueObjects.IValueObject AddMISConfig(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO UserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsAddConfigBizActionVO objMISConfig = valueObject as clsAddConfigBizActionVO;
            try
            {
                clsMISConfigurationVO objConfig = objMISConfig.AddMISConfig;
                DbCommand command = null;


                command = dbServer.GetStoredProcCommand("CIMS_AddConfigMIS");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objMISConfig.ID);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);

                dbServer.AddInParameter(command, "ClinicId", DbType.Int64, objConfig.ClinicId);
                dbServer.AddInParameter(command, "ScheduleName", DbType.String, objConfig.ScheduleName);
                dbServer.AddInParameter(command, "MISReportFormatId", DbType.Int64, objConfig.MISReportFormatId);
                dbServer.AddInParameter(command, "ScheduledOn", DbType.Int64, objConfig.ScheduleOn);
                dbServer.AddInParameter(command, "ScheduleDetails", DbType.String, objConfig.ScheduleDetails);
                dbServer.AddInParameter(command, "ScheduleTime", DbType.DateTime, objConfig.ScheduleTime);
                dbServer.AddInParameter(command, "ScheduleStartDate", DbType.DateTime, objConfig.ScheduleStartDate);
                dbServer.AddInParameter(command, "ScheduleEndDate", DbType.DateTime, objConfig.ScheduleEndDate);


                int intStatus = dbServer.ExecuteNonQuery(command);
                objMISConfig.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));

            }
            catch (Exception ex)
            {
                throw;
            }
            return objMISConfig;
        }



        public override ValueObjects.IValueObject AddUpdateMISConfig(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO UserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsAddUpdateMISConfigurationBizActionVO objMISConfig = valueObject as clsAddUpdateMISConfigurationBizActionVO;
            try
            {
                clsMISConfigurationVO objConfig = objMISConfig.AddMISConfig;
                DbCommand command = null;


                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateConfigMIS");

                dbServer.AddInParameter(command, "ID", DbType.Int64, objMISConfig.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "ConfigDate", DbType.DateTime, objConfig.ConfigDate);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "IsStatusUpdated", DbType.Boolean, objMISConfig.IsUpdateStatus);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objConfig.Status);
                dbServer.AddInParameter(command, "ClinicId", DbType.Int64, objConfig.ClinicId);
                dbServer.AddInParameter(command, "ScheduleName", DbType.String, objConfig.ScheduleName);
                dbServer.AddInParameter(command, "MISReportFormatId", DbType.Int64, objConfig.MISReportFormatId);
                dbServer.AddInParameter(command, "ScheduledOn", DbType.Int64, objConfig.ScheduleOn);
                dbServer.AddInParameter(command, "ScheduleDetails", DbType.String, objConfig.ScheduleDetails);
                dbServer.AddInParameter(command, "ScheduleTime", DbType.DateTime, objConfig.ScheduleTime);
                dbServer.AddInParameter(command, "ScheduleStartDate", DbType.DateTime, objConfig.ScheduleStartDate);
                dbServer.AddInParameter(command, "ScheduleEndDate", DbType.DateTime, objConfig.ScheduleEndDate);
                dbServer.AddInParameter(command, "ReportList", DbType.String, objConfig.ReportDetails);
                dbServer.AddInParameter(command, "StaffList", DbType.String, objConfig.StaffDetails);

                int intStatus = dbServer.ExecuteNonQuery(command);
                objMISConfig.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));

            }
            catch (Exception ex)
            {
                throw;
            }
            return objMISConfig;
        }


        public override ValueObjects.IValueObject GetMISReportType(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO UserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetMISReportTypeBizActionVO objConfig = valueObject as clsGetMISReportTypeBizActionVO;

            if (objConfig.GetMIsReport == null)
                objConfig.GetMIsReport = new List<clsGetMISReportTypeVO>();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetMISReportType");
                dbServer.AddInParameter(command, "MISTypeID", DbType.Int64, objConfig.MISTypeID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);





                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsGetMISReportTypeVO report = new clsGetMISReportTypeVO();
                        report.Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"]));
                        report.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        report.MISTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MISTypeID"]));
                        report.rptFileName = Convert.ToString(DALHelper.HandleDBNull(reader["rptFileName"]));
                        //report.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objConfig.GetMIsReport.Add(report);
                    }
                }


            }
            catch (Exception ex)
            {
                throw;
            }

            return objConfig;
        }



        public override ValueObjects.IValueObject GetStaff(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO UserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetMISStaffBizActionVO objConfig = valueObject as clsGetMISStaffBizActionVO;

            if (objConfig.GetStaffInfo == null)
                objConfig.GetStaffInfo = new List<clsGetMISStaffVO>();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetStaff");
                dbServer.AddInParameter(command, "TypeId", DbType.Int64, objConfig.TypeId);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
               

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        clsGetMISStaffVO Staffinfo = new clsGetMISStaffVO();
                        Staffinfo.Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"]));
                        Staffinfo.Name = DALHelper.HandleDBNull(reader["Name"]).ToString();
               

                        if (objConfig.TypeId == 1)
                        {
                            Staffinfo.SelectStaffTypeId = 1;
                            Staffinfo.StaffTypeId = 2;
                        }
                        else if (objConfig.TypeId == 2)
                        {
                            Staffinfo.SelectStaffTypeId = 2;
                            Staffinfo.StaffTypeId = 2;
                        }
                        else if (objConfig.TypeId == 3)
                        {
                            Staffinfo.SelectStaffTypeId = 3;
                            Staffinfo.StaffTypeId = 3;
                        }
                        objConfig.GetStaffInfo.Add(Staffinfo);

                    }
                    reader.NextResult();
                    while (reader.Read())
                    {
                        clsGetMISStaffVO Staffinfo = new clsGetMISStaffVO();
                        Staffinfo.Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"]));
                        Staffinfo.Name = Convert.ToString(DALHelper.HandleDBNull(reader["Name"]));
                                      
                        if (objConfig.TypeId == 1)
                        {
                            Staffinfo.SelectStaffTypeId = 1;
                            Staffinfo.StaffTypeId = 3;
                        }
                        else if (objConfig.TypeId == 2)
                        {
                            Staffinfo.SelectStaffTypeId = 2;
                            Staffinfo.StaffTypeId = 2;
                        }
                        else if (objConfig.TypeId == 3)
                        {
                            Staffinfo.SelectStaffTypeId = 3;
                            Staffinfo.StaffTypeId = 3;
                        }
                        objConfig.GetStaffInfo.Add(Staffinfo);
                    }
                }


            }
            catch (Exception ex)
            {
                throw;
            }

            return objConfig;
        }

       
        public override ValueObjects.IValueObject GetAutoEmailForMIS(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO UserVO)
        {
          
            clsGetMISEmailDetailsBizActionVO BizAction = valueObject as clsGetMISEmailDetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetEmailDetailsForMIS");
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (BizAction.MISConfigDetails == null)
                            BizAction.MISConfigDetails = new List<clsMISConfigurationVO>();
                        
                        clsMISConfigurationVO ObjMISConfigDetails = new clsMISConfigurationVO();
                        ObjMISConfigDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"]));
                        ObjMISConfigDetails.ClinicId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClinicId"]));
                        ObjMISConfigDetails.ScheduleName = Convert.ToString(DALHelper.HandleDBNull(reader["ScheduleName"]));
                        ObjMISConfigDetails.MISReportFormatId = Convert.ToInt64(DALHelper.HandleDBNull(reader["MISReportFormatId"]));
                        ObjMISConfigDetails.ScheduleOn = Convert.ToInt64(DALHelper.HandleDBNull(reader["ScheduledOn"]));
                        ObjMISConfigDetails.ScheduleDetails = Convert.ToString(DALHelper.HandleDBNull(reader["ScheduleDetails"]));
                        ObjMISConfigDetails.ScheduleTime = Convert.ToDateTime(DALHelper.HandleDate(reader["ScheduleTime"]));
                        ObjMISConfigDetails.ScheduleStartDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ScheduleStartDate"]));
                        ObjMISConfigDetails.ScheduleEndDate = (DateTime?)DALHelper.HandleDate(reader["ScheduleEndDate"]);

                        BizAction.MISConfigDetails.Add(ObjMISConfigDetails);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizAction;
        }


        public override ValueObjects.IValueObject GetMISDetailsFromCriteria(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO UserVO)
        {
           GetMISDetailsFromCriteriaBizActionVO BizAction = valueObject as GetMISDetailsFromCriteriaBizActionVO;
           try
           {
               DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetMISDetailsFromCriteria");

               dbServer.AddInParameter(command, "StartDate ", DbType.DateTime, BizAction.StartDate);
               dbServer.AddInParameter(command, "ScheduleTime", DbType.DateTime, BizAction.ScheduleTime);
             
               DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

               if (reader.HasRows)
               {
                   while (reader.Read())
                   {
                       if (BizAction.MISConfigDetails == null)
                           BizAction.MISConfigDetails = new List<clsMISConfigurationVO>();


                       clsMISConfigurationVO ObjDetails = new clsMISConfigurationVO();
                       ObjDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"]));
                       BizAction.MISConfigDetails.Add(ObjDetails);

                   }
               }
           }
           catch (Exception ex)
           {
               throw;
           }
           return BizAction;
        }


        public override ValueObjects.IValueObject GetMISReportDetails(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO UserVO)
        {
            clsGetMISReportDetailsBiZActionVO BizAction= valueObject as clsGetMISReportDetailsBiZActionVO;
           try
           {
               DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetMISReportDetails");

               dbServer.AddInParameter(command, "MISID ", DbType.Int64, BizAction.MISID);
               
             
               DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

               if (reader.HasRows)
               {
                   while (reader.Read())
                   {
                       if (BizAction.MISReportDetails == null)
                           BizAction.MISReportDetails = new List<clsMISReportVO>();


                       clsMISReportVO ObjDetails = new clsMISReportVO();
                       ObjDetails.MISID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"]));
                       ObjDetails.Sys_MISReportId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Sys_MISReportId"]));
                       ObjDetails.MISReportFormat = Convert.ToInt64(DALHelper.HandleDBNull(reader["MISReportFormatId"]));
                       ObjDetails.rptFileName =Convert.ToString(DALHelper.HandleDBNull(reader["rptFileName"]));
                       ObjDetails.ReportName = Convert.ToString(DALHelper.HandleDBNull(reader["ReportName"]));
                       ObjDetails.StaffTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StaffTypeId"]));
                       ObjDetails.StaffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StaffId"]));
                       ObjDetails.EmailID = Convert.ToString(DALHelper.HandleDBNull(reader["EmailID"]));

                       BizAction.MISReportDetails.Add(ObjDetails);

                   }
               }
           }
           catch (Exception ex)
           {
               throw;
           }
           return BizAction;
        }


    }
}
