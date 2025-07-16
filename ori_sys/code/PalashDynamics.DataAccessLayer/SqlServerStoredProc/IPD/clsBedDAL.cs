using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.IPD;
using System.Data.Common;
using System.Data;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IPD
{
    public class clsBedDAL : clsBaseBedDAL
    {
        #region Variables Declaration
        private Database dbServer = null;
        private LogManager logManager = null;
        #endregion

        private clsBedDAL()
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
            }
        }

        #region Bed Transfer
        public override IValueObject GetIPDWardByClassID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIPDWardByClassIDBizActionVO BizActionObj = valueObject as clsGetIPDWardByClassIDBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetWardByClassID");
                DbDataReader reader;

                dbServer.AddInParameter(command, "ClassID", DbType.Int64, BizActionObj.BedDetails.ClassID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.BedList == null)
                        BizActionObj.BedList = new List<clsIPDBedTransferVO>();

                    while (reader.Read())
                    {
                        clsIPDBedTransferVO BedVO = new clsIPDBedTransferVO();
                        BedVO.WardID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WardID"]));
                        BedVO.Ward = (string)DALHelper.HandleDBNull(reader["Ward"]);

                        BizActionObj.BedList.Add(BedVO);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetIPDBedTransferList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIPDBedTransferListBizActionVO BizActionObj = valueObject as clsGetIPDBedTransferListBizActionVO;
            try
            {
                #region OLD code
                /*
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetIPDBedtransferList");
                DbDataReader reader;
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                dbServer.AddInParameter(command, "MRNo", DbType.String, BizActionObj.MRNo);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "IsClosed", DbType.Boolean, "false");
                dbServer.AddInParameter(command, "IsCancel", DbType.Boolean, "false");
                dbServer.AddInParameter(command, "ISDischarged", DbType.Boolean, "false");
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "TransFerID");
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.BedList == null)
                        BizActionObj.BedList = new List<clsIPDBedTransferVO>();

                    while (reader.Read())
                    {
                        clsIPDBedTransferVO BedVO = new clsIPDBedTransferVO();
                        BizActionObj.BedDetails = new clsIPDBedTransferVO();
                        BedVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransFerID"]));
                        BedVO.TransFerID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransFerID"]));
                        BedVO.TransferUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BedVO.AdmID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BedVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BedVO.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                        BedVO.UnitName = (string)DALHelper.HandleDBNull(reader["UnitName"]);
                        BedVO.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        BedVO.FirstName = Security.base64Decode(reader["FirstName"].ToString());
                        BedVO.LastName = Security.base64Decode(reader["LastName"].ToString());
                        BedVO.PatientName = BedVO.FirstName + " " + BedVO.LastName;
                        BedVO.AdmissionDate = (DateTime?)DALHelper.HandleDBNull(reader["AdmDate"]);
                        BedVO.IPDNo = (string)DALHelper.HandleDBNull(reader["IPDNO"]);
                        BedVO.DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"]));
                        BedVO.DepartmentName = (string)DALHelper.HandleDBNull(reader["DepartmentName"]);
                        BedVO.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                        BedVO.DoctorName = (string)DALHelper.HandleDBNull(reader["DoctorName"]);
                        BedVO.ISDischarged = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ISDischarged"]));
                        BedVO.AdmissionDate = (DateTime?)DALHelper.HandleDBNull(reader["Date"]);
                        BedVO.FromDate = (DateTime?)DALHelper.HandleDBNull(reader["FromDate"]);
                        BedVO.FromTime = (DateTime?)DALHelper.HandleDBNull(reader["FromTime"]);
                        BedVO.ToDate = (DateTime?)DALHelper.HandleDBNull(reader["ToDate"]);
                        BedVO.ToTime = (DateTime?)DALHelper.HandleDBNull(reader["ToTime"]);

                        BedVO.FromBedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FromBedID"]));
                        BedVO.FromBed = (string)DALHelper.HandleDBNull(reader["FromBed"]);
                        BedVO.FromBedUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FUnitID"]));

                        BedVO.ToBedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToBedID"]));
                        BedVO.ToBed = (string)DALHelper.HandleDBNull(reader["ToBed"]);
                        BedVO.ToBedUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TUnitID"]));

                        BedVO.BedCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedCategoryID"]));
                        BedVO.ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedCategoryID"]));
                        BedVO.ClassName = (string)DALHelper.HandleDBNull(reader["CalssName"]);

                        BedVO.BillingClass = (string)DALHelper.HandleDBNull(reader["BillingClass"]);
                        BedVO.BillingBedCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillingBedCategoryID"]));

                        BedVO.WardID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WardID"]));
                        BedVO.Ward = (string)DALHelper.HandleDBNull(reader["WardName"]);
                        BedVO.BedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedID"]));
                        BedVO.Remark = (string)DALHelper.HandleDBNull(reader["Remark"]);

                        BedVO.FromClass = (string)DALHelper.HandleDBNull(reader["FrmCls"]);
                        BedVO.ToClass = (string)DALHelper.HandleDBNull(reader["Tocls"]);
                        BedVO.FromWard = (string)DALHelper.HandleDBNull(reader["FrmWard"]);
                        BedVO.ToWard = (string)DALHelper.HandleDBNull(reader["Toward"]);
                        BedVO.IsSecondaryBed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSecondaryBed"]));
                        BedVO.IsOccupied = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Occupied"]));
                        BizActionObj.BedDetails = BedVO;
                        BizActionObj.BedList.Add(BedVO);
                    }*/
                #endregion
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetIPDBedTransferHistoryList");
                DbDataReader reader;
                dbServer.AddInParameter(command,"FromDate",DbType.DateTime, BizActionObj.BedDetails.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.BedDetails.ToDate);
                dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.BedDetails.FirstName);
                dbServer.AddInParameter(command, "MiddleName", DbType.String, BizActionObj.BedDetails.MiddleName);
                dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.BedDetails.LastName);
                dbServer.AddInParameter(command, "FamilyName", DbType.String, BizActionObj.BedDetails.FamilyName);
                dbServer.AddInParameter(command, "MRNo", DbType.String, BizActionObj.BedDetails.MRNo);
                dbServer.AddInParameter(command, "IPDNO", DbType.String, BizActionObj.BedDetails.IPDNo);
                dbServer.AddInParameter(command, "UnitID", DbType.String, BizActionObj.UnitID);  //Added By Bhushanp 30062017
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "Id");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.BedList == null)
                        BizActionObj.BedList = new List<clsIPDBedTransferVO>();

                    while (reader.Read())
                    {
                        clsIPDBedTransferVO BedVO = new clsIPDBedTransferVO();
                        BizActionObj.BedDetails = new clsIPDBedTransferVO();
                        BedVO.FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]));
                        BedVO.MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"]));
                        BedVO.LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]));
                        BedVO.FamilyName = Convert.ToString(DALHelper.HandleDBNull(reader["FamilyName"]));
                        BedVO.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        BedVO.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        BedVO.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        BedVO.IPDNo = Convert.ToString(DALHelper.HandleDBNull(reader["IPDNO"]));
                        BedVO.IPDAdmissionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IPDAdmissionID"]));
                        BedVO.TransferDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["TransferDate"]));
                        BedVO.FromClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FromClassID"]));
                        BedVO.FromClass = Convert.ToString(DALHelper.HandleDBNull(reader["FromClass"]));

                        BedVO.FromWardID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FromWardID"]));
                        BedVO.FromWard = Convert.ToString(DALHelper.HandleDBNull(reader["FromWard"]));

                        BedVO.FromBedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FromBedID"]));
                        BedVO.FromBed = Convert.ToString(DALHelper.HandleDBNull(reader["FromBed"]));


                        BedVO.ToClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToClassID"]));
                        BedVO.ToClass = Convert.ToString(DALHelper.HandleDBNull(reader["ToClass"]));

                        BedVO.ToWardID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToWardID"]));
                        BedVO.ToWard = Convert.ToString(DALHelper.HandleDBNull(reader["ToWard"]));

                        BedVO.ToBedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToBedID"]));
                        BedVO.ToBed = Convert.ToString(DALHelper.HandleDBNull(reader["ToBed"]));

                        BedVO.FromDate = (DateTime?)DALHelper.HandleDBNull(reader["FromDate"]);
                        BedVO.FromTime = (DateTime?)DALHelper.HandleDBNull(reader["FromTime"]);
                        BedVO.ToDate = (DateTime?)DALHelper.HandleDBNull(reader["ToDate"]);
                        BedVO.ToTime = (DateTime?)DALHelper.HandleDBNull(reader["ToTime"]);
                        BedVO.Status = Convert.ToBoolean(reader["Status"]);

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

        public override IValueObject CheckFinalBillbyPatientID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIPDBedTransferListBizActionVO BizActionObj = valueObject as clsGetIPDBedTransferListBizActionVO;
            try
            {
                DbDataReader reader;
                DbCommand command;
                if (BizActionObj.IsCheckFinalBillByAdmID)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_CheckFinalBillbyAdmID");
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                    dbServer.AddInParameter(command, "AdmID", DbType.Int64, BizActionObj.AdmID);
                    dbServer.AddInParameter(command, "AdmUnitID", DbType.Int64, BizActionObj.AdmUnitID);
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("CIMS_CheckFinalBillbyPatientID");
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                }
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.BedList == null)
                        BizActionObj.BedList = new List<clsIPDBedTransferVO>();

                    while (reader.Read())
                    {
                        clsIPDBedTransferVO BedVO = new clsIPDBedTransferVO();
                        BedVO.AdmID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BedVO.IsCancel = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCancel"]));
                        BedVO.IsClosed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsClosed"]));
                        BedVO.ISDischarged = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ISDischarged"]));
                        BedVO.InterORFinal = Convert.ToBoolean(DALHelper.HandleDBNull(reader["InterORFinal"]));
                        BizActionObj.BedDetails = BedVO;
                    }
                }
                reader.Close();

            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetIPDBedTransferDetailsForSelectedPatient(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetIPDBedTransferListBizActionVO BizActionObj = valueObject as clsGetIPDBedTransferListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IPDPatientBedTransferDetails_New");
                DbDataReader reader;
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.PatientUnitID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.BedList == null)
                        BizActionObj.BedList = new List<clsIPDBedTransferVO>();

                    while (reader.Read())
                    {
                        clsIPDBedTransferVO BedTransferVO = new clsIPDBedTransferVO();
                        BizActionObj.BedDetails = new clsIPDBedTransferVO();
                        BedTransferVO.PatientNameForTransfer = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"])).Trim();
                        BedTransferVO.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        BedTransferVO.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        BedTransferVO.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        BedTransferVO.IPDNo = Convert.ToString(DALHelper.HandleDBNull(reader["IPDNO"]));
                        BedTransferVO.IPDAdmissionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IPDAdmissionID"]));
                        BedTransferVO.TransferDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["TransferDate"]));
                        BedTransferVO.FromClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FromClassID"]));
                        BedTransferVO.FromClass = Convert.ToString(DALHelper.HandleDBNull(reader["FromClass"]));
                        BedTransferVO.FromWardID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FromWardID"]));
                        BedTransferVO.FromWard = Convert.ToString(DALHelper.HandleDBNull(reader["FromWard"]));
                        BedTransferVO.FromBedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FromBedID"]));
                        BedTransferVO.FromBed = Convert.ToString(DALHelper.HandleDBNull(reader["FromBed"]));
                        BedTransferVO.ToClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToClassID"]));
                        BedTransferVO.ToClass = Convert.ToString(DALHelper.HandleDBNull(reader["ToClass"]));
                        BedTransferVO.ToWardID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToWardID"]));
                        BedTransferVO.ToWard = Convert.ToString(DALHelper.HandleDBNull(reader["ToWard"]));
                        BedTransferVO.ToBedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToBedID"]));
                        BedTransferVO.ToBed = Convert.ToString(DALHelper.HandleDBNull(reader["ToBed"]));

                        BedTransferVO.BillingToBedCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillingToBedCategoryID"]));

                        BedTransferVO.FromDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["FromDate"]));
                        BedTransferVO.FromTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["FromTime"]));

                        BedTransferVO.ToDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ToDate"]));
                        BedTransferVO.ToTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ToTime"]));
                        
                        BizActionObj.BedDetails = BedTransferVO;
                        BizActionObj.BedList.Add(BedTransferVO);
                    }
                }
                reader.NextResult();
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }
        #endregion

        #region Bed UnderMaintaince

        public override IValueObject AddBedUnderMaintenance(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddBedUnderMaintenanceBizActionVO BizActionObj = valueObject as clsAddBedUnderMaintenanceBizActionVO;

            BizActionObj = AddBedUnderMaintenanceDetails(BizActionObj, UserVo);

            return valueObject;
        }

        private clsAddBedUnderMaintenanceBizActionVO AddBedUnderMaintenanceDetails(clsAddBedUnderMaintenanceBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                clsIPDBedUnderMaintenanceVO objBedUMVO = BizActionObj.BedUnderMDetails;

                foreach (clsIPDBedUnderMaintenanceVO item in objBedUMVO.BedUnderMList)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddBedUnderMaintanenace");

                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBedUMVO.UnitID);
                    dbServer.AddInParameter(command, "BedID", DbType.Int64, item.BedID);
                    dbServer.AddInParameter(command, "BedUnitID", DbType.Int64, item.BedUnitID);
                    dbServer.AddInParameter(command, "Remark", DbType.String, objBedUMVO.Remark);
                    dbServer.AddInParameter(command, "ExpectedReleasedDate", DbType.DateTime, objBedUMVO.ExpectedReleasedDate);
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, objBedUMVO.FromDate);
                    dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);
                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objBedUMVO.UnitID);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objBedUMVO.AddedBy);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objBedUMVO.AddedWindowsLoginName);

                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objBedUMVO.ID);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    BizActionObj.BedUnderMDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_UpdateIPDBedIsUnderMaintenance");

                    dbServer.AddInParameter(command1, "BedID", DbType.Int64, item.BedID);
                    dbServer.AddInParameter(command1, "BedUnitID", DbType.Int64, item.BedUnitID);
                    dbServer.AddInParameter(command1, "IsUnderMaintanence", DbType.Boolean, item.IsUnderMaintanence);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                }
                trans.Commit();

            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }
            return BizActionObj;
        }

        public override IValueObject GetReleaseBedUnderMaintenanceList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetReleaseBedUnderMaintenanceListBizActionVO BizActionObj = valueObject as clsGetReleaseBedUnderMaintenanceListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetReleaseBedUnderMaintenanceList");
                DbDataReader reader;

                dbServer.AddInParameter(command, "BedID", DbType.Int64, BizActionObj.BedUnderMDetails.BedID);
                dbServer.AddInParameter(command, "BedUnitID", DbType.Int64, BizActionObj.BedUnderMDetails.BedUnitID);
                dbServer.AddInParameter(command, "IsUnderMaintanence", DbType.Boolean, true);
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "BedID");
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.BedUnderMDetails.sortExpression);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.BedUnderMDetails.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.BedUnderMDetails.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.BedUnderMDetails.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.BedUnderMList == null)
                        BizActionObj.BedUnderMList = new List<clsIPDBedUnderMaintenanceVO>();

                    while (reader.Read())
                    {
                        clsIPDBedUnderMaintenanceVO BedVO = new clsIPDBedUnderMaintenanceVO();
                        BizActionObj.BedUnderMDetails = new clsIPDBedUnderMaintenanceVO();
                        BedVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));

                        BedVO.BedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedID"]));
                        BedVO.BedUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedUnitID"]));

                        BedVO.Bed = (string)DALHelper.HandleDBNull(reader["Bed"]);
                        BedVO.Ward = (string)DALHelper.HandleDBNull(reader["Ward"]);
                        BedVO.WardId = Convert.ToInt64(DALHelper.HandleDBNull(reader["WardId"]));
                        BedVO.BedClass = Convert.ToString(DALHelper.HandleDBNull(reader["Class"]));

                        BedVO.ReleaseRemark = "";
                        BedVO.ReleasedDate = null;
                        BedVO.IsUnderMaintanence = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsUnderMaintanence"]));

                        BizActionObj.BedUnderMDetails = BedVO;
                        BizActionObj.BedUnderMList.Add(BedVO);
                    }
                }
                reader.NextResult();
                BizActionObj.BedUnderMDetails.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();

            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject AddUpdateReleaseBedUnderMaintenance(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;
            clsAddUpdateReleaseBedUnderMaintenanceBizActionVO BizActionObj = valueObject as clsAddUpdateReleaseBedUnderMaintenanceBizActionVO;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                clsIPDBedUnderMaintenanceVO objDischargeVO = BizActionObj.BedUnderMDetails;

                foreach (clsIPDBedUnderMaintenanceVO item in BizActionObj.BedUnderMDetails.BedUnderMList)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateReleaseBedUnderMaintanenace");

                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, item.UnitID);
                    dbServer.AddInParameter(command, "BedID", DbType.Int64, item.BedID);
                    dbServer.AddInParameter(command, "BedUnitID", DbType.Int64, item.BedUnitID);

                    dbServer.AddInParameter(command, "ReleasedDate", DbType.DateTime, objDischargeVO.ReleasedDate);
                    dbServer.AddInParameter(command, "ReleaseRemark", DbType.String, item.ReleaseRemark);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_UpdateIPDBedIsUnderMaintenance");

                    dbServer.AddInParameter(command1, "BedID", DbType.Int64, item.BedID);
                    dbServer.AddInParameter(command1, "BedUnitID", DbType.Int64, item.BedUnitID);
                    dbServer.AddInParameter(command1, "IsUnderMaintanence", DbType.Boolean, "False");
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                }
                trans.Commit();

            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }
            return BizActionObj;
        }

        #endregion

        #region For Nursing Station 20022017

        public override IValueObject GetBillAndBedByAdmIDAndAdmUnitID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetBillAndBedByAdmIDAndAdmUnitIDBizActionVO BizActionObj = valueObject as clsGetBillAndBedByAdmIDAndAdmUnitIDBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetBillAndBedByAdmIDAndAdmUnitID");
                DbDataReader reader;
                dbServer.AddInParameter(command, "AdmID", DbType.Int64, BizActionObj.DischargeDetails.AdmID);
                dbServer.AddInParameter(command, "AdmUnitID", DbType.Int64, BizActionObj.DischargeDetails.AdmUnitID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizActionObj.IsFinalBillPrepare = (bool)DALHelper.HandleDBNull(reader["IsFinalBillPrepare"]);
                        BizActionObj.IsBedRelease = (bool)DALHelper.HandleDBNull(reader["IsBedRelease"]);
                    }
                }

                //if(reader.HasRows)
                //{
                //    //if(BizActionObj.BedList == null)
                //    //    BizActionObj.BedList = new List<clsIPDDischargeVO>();

                //    while(reader.Read())
                //    {
                //        clsIPDDischargeVO DischargeVO = new clsIPDDischargeVO();
                //        DischargeVO.AdmID = (long)DALHelper.HandleDBNull(reader["ID"]);
                //        DischargeVO.AdmUnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                //        DischargeVO.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                //        DischargeVO.PatientUnitID = (long)DALHelper.HandleDBNull(reader["PatientUnitID"]);
                //        DischargeVO.Occupied = (bool)DALHelper.HandleDBNull(reader["Occupied"]);

                //        BizActionObj.BedList.Add(DischargeVO);
                //    }
                //}
                //reader.NextResult();
                //if(reader.HasRows)
                //{
                //    if(BizActionObj.BillList == null)
                //        BizActionObj.BillList = new List<clsIPDDischargeVO>();

                //    while(reader.Read())
                //    {
                //        clsIPDDischargeVO DischargeVO = new clsIPDDischargeVO();

                //        DischargeVO.AdmID = (long)DALHelper.HandleDBNull(reader["ID"]);
                //        DischargeVO.AdmUnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                //        DischargeVO.BedID = (long)DALHelper.HandleDBNull(reader["BedID"]);
                //        DischargeVO.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                //        DischargeVO.PatientUnitID = (long)DALHelper.HandleDBNull(reader["PatientUnitID"]);
                //        DischargeVO.InterORFinal = (bool)DALHelper.HandleDBNull(reader["InterORFinal"]);

                //        BizActionObj.BillList.Add(DischargeVO);
                //    }
                //}
                //BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
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
}
