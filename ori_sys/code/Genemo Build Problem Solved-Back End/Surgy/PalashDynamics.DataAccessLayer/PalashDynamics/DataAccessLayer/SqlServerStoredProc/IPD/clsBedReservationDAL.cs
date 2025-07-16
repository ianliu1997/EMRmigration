namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IPD
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.IPD;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsBedReservationDAL : clsBaseReservationDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsBedReservationDAL()
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
            }
        }

        private clsAddIPDBedReservationBizActionVO AddBedReservationDetails(clsAddIPDBedReservationBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsIPDBedReservationVO bedDetails = BizActionObj.BedDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddBedReservation");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, bedDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, bedDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, bedDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "BedUnitID", DbType.Int64, bedDetails.BedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "BedID", DbType.Int64, bedDetails.BedID);
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, bedDetails.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, bedDetails.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, bedDetails.Remark);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, bedDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.BedDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject AddIPDBedReservation(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddIPDBedReservationBizActionVO bizActionObj = valueObject as clsAddIPDBedReservationBizActionVO;
            bizActionObj = this.AddBedReservationDetails(bizActionObj, UserVo);
            return valueObject;
        }

        public override IValueObject AddIPDPatientReminderLog(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddIPDBedReservationBizActionVO nvo = valueObject as clsAddIPDBedReservationBizActionVO;
            try
            {
                clsIPDBedReservationVO bedDetails = nvo.BedDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatientReminderLog");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, bedDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, bedDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "contactNo", DbType.String, bedDetails.ContactNo1);
                this.dbServer.AddInParameter(storedProcCommand, "BedName", DbType.String, bedDetails.BedCode);
                this.dbServer.AddInParameter(storedProcCommand, "WardName", DbType.String, bedDetails.Ward);
                this.dbServer.AddInParameter(storedProcCommand, "ClassName", DbType.String, bedDetails.ClassName);
                this.dbServer.AddInParameter(storedProcCommand, "CallingDate", DbType.Date, bedDetails.CallingDate);
                this.dbServer.AddInParameter(storedProcCommand, "CallingTime", DbType.Date, bedDetails.CallingTime);
                this.dbServer.AddInParameter(storedProcCommand, "FeedBack", DbType.String, bedDetails.Remark);
                this.dbServer.AddInParameter(storedProcCommand, "source", DbType.String, bedDetails.Source);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, bedDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.BedDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetIPDBedReservationList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIPDBedReservationListBizActionVO nvo = valueObject as clsGetIPDBedReservationListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIPDBedReservationList");
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, nvo.MRNo);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Occupied", DbType.Boolean, nvo.Occupied);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.BedList == null)
                    {
                        nvo.BedList = new List<clsIPDBedReservationVO>();
                    }
                    while (reader.Read())
                    {
                        clsIPDBedReservationVO item = new clsIPDBedReservationVO();
                        nvo.BedDetails = new clsIPDBedReservationVO();
                        item.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        item.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        item.MRNo = (string) DALHelper.HandleDBNull(reader["MRNo"]);
                        item.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        item.FirstName = Security.base64Decode(reader["FirstName"].ToString());
                        item.LastName = Security.base64Decode(reader["LastName"].ToString());
                        item.PatientName = item.FirstName + " " + item.LastName;
                        item.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        item.FromDate = (DateTime?) DALHelper.HandleDBNull(reader["FromDate"]);
                        item.ToDate = (DateTime?) DALHelper.HandleDBNull(reader["ToDate"]);
                        item.BedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedID"]));
                        item.BedUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedUnitID"]));
                        item.ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedCategoryID"]));
                        item.ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]));
                        item.WardID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WardID"]));
                        item.Ward = Convert.ToString(DALHelper.HandleDBNull(reader["WardName"]));
                        item.BedCode = Convert.ToString(DALHelper.HandleDBNull(reader["BedCode"]));
                        item.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        item.BedNo = Convert.ToString(DALHelper.HandleDBNull(reader["BedNo"]));
                        nvo.BedDetails = item;
                        nvo.BedList.Add(item);
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

        public override IValueObject GetIPDBedReservationStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIPDBedReservationStatusBizActionVO nvo = valueObject as clsGetIPDBedReservationStatusBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIPDBedReservationStatus");
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, nvo.MRNo);
                this.dbServer.AddInParameter(storedProcCommand, "firstname", DbType.String, nvo.FirstName);
                this.dbServer.AddInParameter(storedProcCommand, "lastname", DbType.String, nvo.LastName);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Occupied", DbType.Boolean, nvo.Occupied);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.BedList == null)
                    {
                        nvo.BedList = new List<clsIPDBedReservationVO>();
                    }
                    while (reader.Read())
                    {
                        clsIPDBedReservationVO item = new clsIPDBedReservationVO();
                        nvo.BedDetails = new clsIPDBedReservationVO();
                        item.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        item.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        item.MRNo = (string) DALHelper.HandleDBNull(reader["MRNo"]);
                        item.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        item.FirstName = Convert.ToString(reader["FirstName"].ToString());
                        item.LastName = Convert.ToString(reader["LastName"].ToString());
                        item.PatientName = item.FirstName + " " + item.LastName;
                        item.ContactNo1 = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo1"]));
                        item.Email = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["Email"])));
                        item.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        item.FromDate = (DateTime?) DALHelper.HandleDBNull(reader["FromDate"]);
                        item.ToDate = (DateTime?) DALHelper.HandleDBNull(reader["ToDate"]);
                        item.BedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedID"]));
                        item.BedUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedUnitID"]));
                        item.ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedCategoryID"]));
                        item.ClassName = (string) DALHelper.HandleDBNull(reader["ClassName"]);
                        item.WardID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WardID"]));
                        item.Ward = (string) DALHelper.HandleDBNull(reader["WardName"]);
                        item.BedCode = (string) DALHelper.HandleDBNull(reader["BedCode"]);
                        item.Remark = (string) DALHelper.HandleDBNull(reader["Remark"]);
                        item.BedNo = (string) DALHelper.HandleDBNull(reader["BedNo"]);
                        nvo.BedDetails = item;
                        nvo.BedList.Add(item);
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

        public override IValueObject GetIPDPatientReminderLog(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddIPDBedReservationBizActionVO nvo = valueObject as clsAddIPDBedReservationBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientReminderLog");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.BedDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.BedDetails.UnitID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    nvo.LogList = new List<clsIPDBedReservationVO>();
                    while (reader.Read())
                    {
                        clsIPDBedReservationVO item = new clsIPDBedReservationVO {
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString()
                        };
                        item.PatientName = item.FirstName + " " + item.LastName;
                        item.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        item.CallingDate = (DateTime?) DALHelper.HandleDBNull(reader["CallingDate"]);
                        item.CallingTime = new DateTime?((DateTime) DALHelper.HandleDBNull(reader["CallingTime"]));
                        item.UserName = Convert.ToString(DALHelper.HandleDBNull(reader["UserName"]));
                        item.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["FeedBack"]));
                        item.Source = (string) DALHelper.HandleDBNull(reader["source"]);
                        nvo.LogList.Add(item);
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
    }
}

