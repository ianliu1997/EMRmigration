using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.ValueObjects;
using System.Data.Common;
using PalashDynamics.ValueObjects.Patient;
using System.Data;
using PalashDynamics.ValueObjects.Administration.IPD;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IPD
{
    public class clsBedReservationDAL : clsBaseReservationDAL
    {
        #region Variables Declaration
        private Database dbServer = null;
        private LogManager logManager = null;
        #endregion
        private clsBedReservationDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql object
                if (dbServer == null)
                {
                    dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
                if (logManager == null)
                {
                    logManager = LogManager.GetInstance();
                }
                #endregion

            }
            catch (Exception ex)
            {
                //  throw;
            }
        }

        #region
        public override IValueObject AddIPDBedReservation(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddIPDBedReservationBizActionVO BizActionObj = valueObject as clsAddIPDBedReservationBizActionVO;
            BizActionObj = AddBedReservationDetails(BizActionObj, UserVo);
            return valueObject;
        }

        private clsAddIPDBedReservationBizActionVO AddBedReservationDetails(clsAddIPDBedReservationBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsIPDBedReservationVO objBedVO = BizActionObj.BedDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddBedReservation");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBedVO.UnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objBedVO.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objBedVO.PatientUnitID);
                dbServer.AddInParameter(command, "BedUnitID", DbType.Int64, objBedVO.BedUnitID);
                dbServer.AddInParameter(command, "BedID", DbType.Int64, objBedVO.BedID);
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, objBedVO.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, objBedVO.ToDate);
                dbServer.AddInParameter(command, "Remark", DbType.String, objBedVO.Remark);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objBedVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.BedDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }
            return BizActionObj;
        }



        public override IValueObject AddIPDPatientReminderLog(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddIPDBedReservationBizActionVO BizActionObj = valueObject as clsAddIPDBedReservationBizActionVO;
            try
            {
                clsIPDBedReservationVO objBedVO = BizActionObj.BedDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPatientReminderLog");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objBedVO.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objBedVO.PatientUnitID);
                dbServer.AddInParameter(command, "contactNo", DbType.String, objBedVO.ContactNo1);
                dbServer.AddInParameter(command, "BedName", DbType.String, objBedVO.BedCode);
                dbServer.AddInParameter(command, "WardName", DbType.String, objBedVO.Ward);
                dbServer.AddInParameter(command, "ClassName", DbType.String, objBedVO.ClassName);
                dbServer.AddInParameter(command, "CallingDate", DbType.Date, objBedVO.CallingDate);
                dbServer.AddInParameter(command, "CallingTime", DbType.Date, objBedVO.CallingTime);
                dbServer.AddInParameter(command, "FeedBack", DbType.String, objBedVO.Remark);
                dbServer.AddInParameter(command, "source", DbType.String, objBedVO.Source);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objBedVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.BedDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }
            return BizActionObj;
        }



        public override IValueObject GetIPDBedReservationList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIPDBedReservationListBizActionVO BizActionObj = valueObject as clsGetIPDBedReservationListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetIPDBedReservationList");
                DbDataReader reader;
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                dbServer.AddInParameter(command, "MRNo", DbType.String, BizActionObj.MRNo);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "Occupied", DbType.Boolean, BizActionObj.Occupied);
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.BedList == null)
                        BizActionObj.BedList = new List<clsIPDBedReservationVO>();

                    while (reader.Read())
                    {
                        clsIPDBedReservationVO BedVO = new clsIPDBedReservationVO();
                        BizActionObj.BedDetails = new clsIPDBedReservationVO();
                        BedVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BedVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BedVO.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                        BedVO.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        BedVO.FirstName = Security.base64Decode(reader["FirstName"].ToString());
                        BedVO.LastName = Security.base64Decode(reader["LastName"].ToString());
                        BedVO.PatientName = BedVO.FirstName + " " + BedVO.LastName;
                        BedVO.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        BedVO.FromDate = (DateTime?)DALHelper.HandleDBNull(reader["FromDate"]);
                        BedVO.ToDate = (DateTime?)DALHelper.HandleDBNull(reader["ToDate"]);

                        BedVO.BedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedID"]));
                        BedVO.BedUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedUnitID"]));
                        BedVO.ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedCategoryID"]));
                        BedVO.ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]));
                        BedVO.WardID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WardID"]));
                        BedVO.Ward = Convert.ToString(DALHelper.HandleDBNull(reader["WardName"]));
                        BedVO.BedCode = Convert.ToString(DALHelper.HandleDBNull(reader["BedCode"]));
                       
                        BedVO.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        BedVO.BedNo = Convert.ToString(DALHelper.HandleDBNull(reader["BedNo"]));
                        BizActionObj.BedDetails = BedVO;
                        BizActionObj.BedList.Add(BedVO);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();

            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }


        public override IValueObject GetIPDPatientReminderLog(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddIPDBedReservationBizActionVO BizActionObj = valueObject as clsAddIPDBedReservationBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientReminderLog");
                DbDataReader reader;
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.BedDetails.PatientID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.BedDetails.UnitID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    BizActionObj.LogList = new List<clsIPDBedReservationVO>();
                    while (reader.Read())
                    {
                        clsIPDBedReservationVO BedVO = new clsIPDBedReservationVO();

                        //BedVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));                        
                        BedVO.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        BedVO.FirstName = reader["FirstName"].ToString();
                        BedVO.LastName = reader["LastName"].ToString();
                        BedVO.PatientName = BedVO.FirstName + " " + BedVO.LastName;
                        BedVO.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));

                        BedVO.CallingDate = (DateTime?)DALHelper.HandleDBNull(reader["CallingDate"]);
                        BedVO.CallingTime = (DateTime)DALHelper.HandleDBNull(reader["CallingTime"]);

                        BedVO.UserName = Convert.ToString(DALHelper.HandleDBNull(reader["UserName"]));
                        BedVO.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["FeedBack"]));
                        BedVO.Source = (string)DALHelper.HandleDBNull(reader["source"]);
                        BizActionObj.LogList.Add(BedVO);

                    }
                }
                reader.NextResult();
                //BizActionObj.BedDetails.to = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();

            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }


        public override IValueObject GetIPDBedReservationStatus(IValueObject valueObject, clsUserVO UserVo)
        {

            clsGetIPDBedReservationStatusBizActionVO BizActionObj = valueObject as clsGetIPDBedReservationStatusBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetIPDBedReservationStatus");
                DbDataReader reader;
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                dbServer.AddInParameter(command, "MRNo", DbType.String, BizActionObj.MRNo);
                dbServer.AddInParameter(command, "firstname", DbType.String, BizActionObj.FirstName);
                dbServer.AddInParameter(command, "lastname", DbType.String, BizActionObj.LastName);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "Occupied", DbType.Boolean, BizActionObj.Occupied);
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.BedList == null)
                        BizActionObj.BedList = new List<clsIPDBedReservationVO>();

                    while (reader.Read())
                    {
                        clsIPDBedReservationVO BedVO = new clsIPDBedReservationVO();
                        BizActionObj.BedDetails = new clsIPDBedReservationVO();
                        BedVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BedVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BedVO.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                        BedVO.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        BedVO.FirstName = Convert.ToString(reader["FirstName"].ToString());
                        BedVO.LastName = Convert.ToString(reader["LastName"].ToString());
                        BedVO.PatientName = BedVO.FirstName + " " + BedVO.LastName;
                        BedVO.ContactNo1 = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo1"]));
                        BedVO.Email = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["Email"])));
                        BedVO.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        BedVO.FromDate = (DateTime?)DALHelper.HandleDBNull(reader["FromDate"]);
                        BedVO.ToDate = (DateTime?)DALHelper.HandleDBNull(reader["ToDate"]);

                        BedVO.BedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedID"]));
                        BedVO.BedUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedUnitID"]));
                        BedVO.ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedCategoryID"]));
                        BedVO.ClassName = (string)DALHelper.HandleDBNull(reader["ClassName"]);
                        BedVO.WardID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WardID"]));
                        BedVO.Ward = (string)DALHelper.HandleDBNull(reader["WardName"]);
                        BedVO.BedCode = (string)DALHelper.HandleDBNull(reader["BedCode"]);
                        BedVO.Remark = (string)DALHelper.HandleDBNull(reader["Remark"]);
                        BedVO.BedNo = (string)DALHelper.HandleDBNull(reader["BedNo"]);
                        BizActionObj.BedDetails = BedVO;
                        BizActionObj.BedList.Add(BedVO);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();

            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }


        #endregion
    }

    public class clsBedUnReservationDAL : clsBaseUnReservationDAL
    {
        #region Variables Declaration
        private Database dbServer = null;
        private LogManager logManager = null;
        #endregion

        private clsBedUnReservationDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql object
                if (dbServer == null)
                {
                    dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
                if (logManager == null)
                {
                    logManager = LogManager.GetInstance();
                }
                #endregion

            }
            catch (Exception ex)
            {
                //  throw;
            }
        }

        public override IValueObject AddIPDBedUnReservation(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddIPDBedUnReservationBizActionVO BizActionObj = valueObject as clsAddIPDBedUnReservationBizActionVO;
            BizActionObj = AddBedUnReservationDetails(BizActionObj, UserVo);
            return valueObject;
        }

        private clsAddIPDBedUnReservationBizActionVO AddBedUnReservationDetails(clsAddIPDBedUnReservationBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsIPDBedUnReservationVO objBedVO = BizActionObj.BedUnResDetails;
                foreach (clsIPDBedReservationVO item in objBedVO.BedList)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddBedUnReservation");

                    dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "BedReservationID", DbType.Int64, item.ID);
                    dbServer.AddInParameter(command, "BedReservationUnitID", DbType.Int64, item.UnitID);
                    dbServer.AddInParameter(command, "Date", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "Remark", DbType.String, item.UnResRemark); ;

                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddOutParameter(command, "ID", DbType.Int64, int.MaxValue);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus = dbServer.ExecuteNonQuery(command);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    BizActionObj.BedUnResDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
                }
            }
            catch (Exception)
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
