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

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IPD
{
    class clsIPDDAL : clsBaseIPDDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsIPDDAL()
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

                //  throw;
            }
        }

        public override IValueObject GetIPDPatientDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIPDPatientDetailsBizActionVO BizActionObj = valueObject as clsGetIPDPatientDetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientIPDDetails");
                DbDataReader reader;


                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "MRNo", DbType.String, BizActionObj.MRNo);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.PatientDetails == null)
                        BizActionObj.PatientDetails = new clsIPDAdmissionVO();

                    while (reader.Read())
                    {
                        BizActionObj.PatientDetails.PatientId = (long)DALHelper.HandleDBNull(reader["PatientId"]);
                        BizActionObj.PatientDetails.PatientUnitID = (long)DALHelper.HandleDBNull(reader["PatientUnitID"]);
                        BizActionObj.PatientDetails.UnitId = (long)DALHelper.HandleDBNull(reader["UnitId"]);
                        BizActionObj.PatientDetails.ID = (long)DALHelper.HandleDBNull(reader["IPDAdmissionID"]);
                        BizActionObj.PatientDetails.AdmissionNO = (string)DALHelper.HandleDBNull(reader["AdmissionNo"]);
                        BizActionObj.PatientDetails.Date = (DateTime)DALHelper.HandleDBNull(reader["AdmissionDate"]);
                        BizActionObj.PatientDetails.DepartmentID = (long)DALHelper.HandleDBNull(reader["DepartmentID"]);
                        BizActionObj.PatientDetails.DoctorID = (long)DALHelper.HandleDBNull(reader["DoctorID"]);
                        BizActionObj.PatientDetails.WardID = (long)DALHelper.HandleDBNull(reader["WardID"]);
                        BizActionObj.PatientDetails.BedCategoryID = (long)DALHelper.HandleDBNull(reader["BedCategoryID"]);
                        BizActionObj.PatientDetails.BedID = (long)DALHelper.HandleDBNull(reader["BedID"]);
                        BizActionObj.PatientDetails.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);


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

        #region BedTransfer
        public override IValueObject AddIPDBedTransfer(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddIPDBedTransferBizActionVO BizActionObj = valueObject as clsAddIPDBedTransferBizActionVO;

            if (BizActionObj.BedDetails.ID == 0)
                BizActionObj = AddBedDetails(BizActionObj, UserVo);
            else
                BizActionObj = UpdateBedDetails(BizActionObj, UserVo);
            return valueObject;
        }

        private clsAddIPDBedTransferBizActionVO AddBedDetails(clsAddIPDBedTransferBizActionVO BizActionObj, clsUserVO UserVo)
        {

            #region OLD Bagul Code For Add

            //try
            //{
            //    clsIPDBedTransferVO objBedVO = BizActionObj.BedDetails;
            //    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddIPDBedTransfer");

            //    dbServer.AddInParameter(command, "PatientID", DbType.Int64, objBedVO.PatientID);
            //    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objBedVO.PatientUnitID);
            //    dbServer.AddInParameter(command, "IPDAdmissionID", DbType.Int64, objBedVO.IPDAdmissionID);
            //    dbServer.AddInParameter(command, "IPDAdmissionNo", DbType.String, objBedVO.IPDAdmissionNo);
            //    dbServer.AddInParameter(command, "TransferDate", DbType.String, DateTime.Now);
            //    dbServer.AddInParameter(command, "BedCategoryID", DbType.Int64, objBedVO.BedCategoryID);
            //    dbServer.AddInParameter(command, "WardID", DbType.Int64, objBedVO.WardID);
            //    dbServer.AddInParameter(command, "BedID", DbType.Int64, objBedVO.BedID);

            //    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            //    dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);

            //    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            //    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
            //    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
            //    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
            //    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

            //    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objBedVO.ID);
            //    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

            //    int intStatus = dbServer.ExecuteNonQuery(command);
            //    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
            //    BizActionObj.BedDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

            //}
            //catch (Exception)
            //{
            //    throw;
            //}
            //return BizActionObj;

            #endregion



            #region From Admission Form
            try
            {
                DbCommand command;
                clsIPDBedTransferVO objBedVO = BizActionObj.BedDetails;
                if (BizActionObj.IsTransfer == true)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_AddIPDBedTransferFromBedTransferForm");
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objBedVO.UnitID);
                    dbServer.AddInParameter(command, "OldBedCategoryID", DbType.Int64, objBedVO.BedCategoryID);
                    dbServer.AddInParameter(command, "OldWardId", DbType.Int64, objBedVO.WardID);
                    dbServer.AddInParameter(command, "OldBedID", DbType.Int64, objBedVO.BedID);
                    dbServer.AddInParameter(command, "FromTime", DbType.DateTime, objBedVO.FromTime);
                    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("CIMS_AddIPDBedTransfer");
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objBedVO.PatientUnitID);
                }
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objBedVO.PatientID);
                dbServer.AddInParameter(command, "IPDAdmissionID", DbType.Int64, objBedVO.IPDAdmissionID);
                dbServer.AddInParameter(command, "IPDAdmissionNo", DbType.String, objBedVO.IPDAdmissionNo);
                dbServer.AddInParameter(command, "TransferDate", DbType.String, DateTime.Now);
                if (BizActionObj.IsTransfer == true)
                {
                    dbServer.AddInParameter(command, "BedCategoryID", DbType.Int64, objBedVO.ToClassID);
                    dbServer.AddInParameter(command, "WardID", DbType.Int64, objBedVO.ToWardID);
                    dbServer.AddInParameter(command, "BedID", DbType.Int64, objBedVO.ToBedID);

                    dbServer.AddInParameter(command, "BillingToBedCategoryID", DbType.Int64, objBedVO.BillingToBedCategoryID);  // For IPD Billing Class
                }
                else
                {
                    dbServer.AddInParameter(command, "BedCategoryID", DbType.Int64, objBedVO.BedCategoryID);
                    dbServer.AddInParameter(command, "WardID", DbType.Int64, objBedVO.WardID);
                    dbServer.AddInParameter(command, "BedID", DbType.Int64, objBedVO.BedID);
                }

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);

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
            return BizActionObj;

            #endregion

        }

        private clsAddIPDBedTransferBizActionVO UpdateBedDetails(clsAddIPDBedTransferBizActionVO BizActionObj, clsUserVO UserVo)
        {

            try
            {
                clsIPDBedTransferVO objBedVO = BizActionObj.BedDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateIPDBedTransfer");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objBedVO.ID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objBedVO.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objBedVO.PatientUnitID);
                dbServer.AddInParameter(command, "IPDAdmissionID", DbType.Int64, objBedVO.IPDAdmissionID);
                dbServer.AddInParameter(command, "IPDAdmissionNo", DbType.String, objBedVO.IPDAdmissionNo);
                dbServer.AddInParameter(command, "TransferDate", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "BedCategoryID", DbType.Int64, objBedVO.BedCategoryID);
                dbServer.AddInParameter(command, "WardID", DbType.Int64, objBedVO.WardID);
                dbServer.AddInParameter(command, "BedID", DbType.Int64, objBedVO.BedID);

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);

                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");


            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetIPDBedTransfer(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIPDBedTransferBizActionVO BizActionObj = valueObject as clsGetIPDBedTransferBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetIPDBedtransferDetails");
                DbDataReader reader;

                dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.FirstName);
                dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.LastName);
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.BedList == null)
                        BizActionObj.BedList = new List<clsIPDBedTransferVO>();

                    while (reader.Read())
                    {
                        clsIPDBedTransferVO BedVO = new clsIPDBedTransferVO();
                        BedVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        BedVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        BedVO.UnitName = (string)DALHelper.HandleDBNull(reader["Clinic"]);
                        BedVO.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                        BedVO.PatientUnitID = (long)DALHelper.HandleDBNull(reader["PatientUnitID"]);
                        BedVO.IPDAdmissionID = (long)DALHelper.HandleDBNull(reader["IPDAdmissionID"]);
                        BedVO.IPDAdmissionNo = (string)DALHelper.HandleDBNull(reader["IPDAdmissionNo"]);
                        BedVO.TransferDate = (DateTime)DALHelper.HandleDBNull(reader["TransferDate"]);
                        BedVO.BedCategoryID = (long)DALHelper.HandleDBNull(reader["BedCategoryID"]);
                        BedVO.BedCategory = (string)DALHelper.HandleDBNull(reader["BedCategory"]);
                        BedVO.WardID = (long)DALHelper.HandleDBNull(reader["WardID"]);
                        BedVO.Ward = (string)DALHelper.HandleDBNull(reader["Ward"]);
                        BedVO.BedID = (long)DALHelper.HandleDBNull(reader["BedID"]);
                        BedVO.BedNo = (string)DALHelper.HandleDBNull(reader["BedNo"]);
                        BedVO.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                        BedVO.FirstName = (string)DALHelper.HandleDBNull(reader["FirstName"]);
                        BedVO.MiddleName = (string)DALHelper.HandleDBNull(reader["MiddleName"]);
                        BedVO.LastName = (string)DALHelper.HandleDBNull(reader["LastName"]);
                        BedVO.AdmissionDate = (DateTime)DALHelper.HandleDBNull(reader["AdmissionDate"]);
                        BedVO.DateOfBirth = (DateTime)DALHelper.HandleDBNull(reader["DateOfBirth"]);
                        BedVO.GenderID = (long)DALHelper.HandleDBNull(reader["GenderID"]);
                        BizActionObj.BedList.Add(BedVO);


                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();

            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }
        #endregion


        public override IValueObject AddIPDDischarge(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddIPDDischargeBizActionVO BizActionObj = valueObject as clsAddIPDDischargeBizActionVO;

            if (BizActionObj.DischargeDetails.ID == 0)
                BizActionObj = AddDischargeDetails(BizActionObj, UserVo);
            else
                BizActionObj = UpdateDischargeDetails(BizActionObj, UserVo);

            return valueObject;
        }
        private clsAddIPDDischargeBizActionVO AddDischargeDetails(clsAddIPDDischargeBizActionVO BizActionObj, clsUserVO UserVo)
        {

            try
            {
                clsIPDDischargeVO objDischargeVO = BizActionObj.DischargeDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddIPDPatientDischarge");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objDischargeVO.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objDischargeVO.PatientUnitID);
                dbServer.AddInParameter(command, "IPDAdmissionID", DbType.Int64, objDischargeVO.IPDAdmissionID);
                dbServer.AddInParameter(command, "IPDAdmissionNo", DbType.String, objDischargeVO.IPDAdmissionNo);
                dbServer.AddInParameter(command, "DischargeDate", DbType.DateTime, objDischargeVO.DischargeDate);
                dbServer.AddInParameter(command, "DischargeTime", DbType.DateTime, objDischargeVO.DischargeTime);
                dbServer.AddInParameter(command, "DischargeDoctor", DbType.Int64, objDischargeVO.DischargeDoctor);
                dbServer.AddInParameter(command, "DischargeType", DbType.Int64, objDischargeVO.DischargeType);
                dbServer.AddInParameter(command, "DischargeDestination", DbType.Int64, objDischargeVO.DischargeDestination);
                dbServer.AddInParameter(command, "IsDeathDischarge", DbType.Boolean, objDischargeVO.IsDeathdischarge);

                dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDischargeVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);

                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.DischargeDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;

            #region OLD Descharge Code
            //try
            //{
            //    clsIPDDischargeVO objDischargeVO = BizActionObj.DischargeDetails;
            //    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddIPDDischarge");

            //    dbServer.AddInParameter(command, "PatientID", DbType.Int64, objDischargeVO.PatientID);
            //    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objDischargeVO.PatientUnitID);
            //    dbServer.AddInParameter(command, "IPDAdmissionID", DbType.Int64, objDischargeVO.IPDAdmissionID);
            //    dbServer.AddInParameter(command, "IPDAdmissionNo", DbType.String, objDischargeVO.IPDAdmissionNo);
            //    dbServer.AddInParameter(command, "DischargeDate", DbType.DateTime, objDischargeVO.DischargeDate);
            //    dbServer.AddInParameter(command, "DischargeTime", DbType.DateTime, objDischargeVO.DischargeTime);
            //    dbServer.AddInParameter(command, "DischargeDoctor", DbType.Int64, objDischargeVO.DischargeDoctor);
            //    dbServer.AddInParameter(command, "DischargeType", DbType.Int64, objDischargeVO.DischargeType);
            //    dbServer.AddInParameter(command, "DischargeDestination", DbType.Int64, objDischargeVO.DischargeDestination);

            //    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            //    dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);

            //    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            //    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
            //    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
            //    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
            //    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

            //    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDischargeVO.ID);
            //    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

            //    int intStatus = dbServer.ExecuteNonQuery(command);
            //    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
            //    BizActionObj.DischargeDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

            //}
            //catch (Exception)
            //{
            //    throw;
            //}
            //return BizActionObj;
            #endregion
        }

        private clsAddIPDDischargeBizActionVO UpdateDischargeDetails(clsAddIPDDischargeBizActionVO BizActionObj, clsUserVO UserVo)
        {

            try
            {
                clsIPDDischargeVO objDischargeVO = BizActionObj.DischargeDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateIPDDischarge");

                dbServer.AddInParameter(command, "ID", DbType.Int64, objDischargeVO.ID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objDischargeVO.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objDischargeVO.PatientUnitID);
                dbServer.AddInParameter(command, "IPDAdmissionID", DbType.Int64, objDischargeVO.IPDAdmissionID);
                dbServer.AddInParameter(command, "IPDAdmissionNo", DbType.String, objDischargeVO.IPDAdmissionNo);
                dbServer.AddInParameter(command, "DischargeDate", DbType.DateTime, objDischargeVO.DischargeDate);
                dbServer.AddInParameter(command, "DischargeTime", DbType.DateTime, objDischargeVO.DischargeTime);
                dbServer.AddInParameter(command, "DischargeDoctor", DbType.Int64, objDischargeVO.DischargeDoctor);
                dbServer.AddInParameter(command, "DischargeType", DbType.Int64, objDischargeVO.DischargeType);
                dbServer.AddInParameter(command, "DischargeDestination", DbType.Int64, objDischargeVO.DischargeDestination);

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);

                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");


            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetIPDDischargeDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIPDDischargeBizActionVO BizActionObj = valueObject as clsGetIPDDischargeBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetIPDDischargeDetails");
                DbDataReader reader;

                dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.FirstName);
                dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.LastName);
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.DischargeList == null)
                        BizActionObj.DischargeList = new List<clsIPDDischargeVO>();

                    while (reader.Read())
                    {
                        clsIPDDischargeVO DischargeVO = new clsIPDDischargeVO();
                        DischargeVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        DischargeVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        DischargeVO.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                        DischargeVO.PatientUnitID = (long)DALHelper.HandleDBNull(reader["PatientUnitID"]);
                        DischargeVO.IPDAdmissionID = (long)DALHelper.HandleDBNull(reader["IPDAdmissionID"]);
                        DischargeVO.IPDAdmissionNo = (string)DALHelper.HandleDBNull(reader["IPDAdmissionNo"]);

                        DischargeVO.DischargeDate = (DateTime)DALHelper.HandleDBNull(reader["DischargeDate"]);
                        DischargeVO.DischargeTime = (DateTime)DALHelper.HandleDBNull(reader["DischargeTime"]);
                        DischargeVO.DischargeDoctor = (long)DALHelper.HandleDBNull(reader["DischargeDoctor"]);
                        DischargeVO.DischargeType = (long)DALHelper.HandleDBNull(reader["DischargeTypeID"]);
                        DischargeVO.DischargeDestination = (long)DALHelper.HandleDBNull(reader["DischargeDestinationID"]);

                        DischargeVO.DoctorName = (string)DALHelper.HandleDBNull(reader["DoctorName"]);
                        DischargeVO.Type = (string)DALHelper.HandleDBNull(reader["DischargeType"]);
                        DischargeVO.Destination = (string)DALHelper.HandleDBNull(reader["DischargeDestination"]);

                        DischargeVO.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                        DischargeVO.FirstName = (string)DALHelper.HandleDBNull(reader["FirstName"]);
                        DischargeVO.MiddleName = (string)DALHelper.HandleDBNull(reader["MiddleName"]);
                        DischargeVO.LastName = (string)DALHelper.HandleDBNull(reader["LastName"]);

                        DischargeVO.DateOfBirth = (DateTime)DALHelper.HandleDBNull(reader["DateOfBirth"]);
                        DischargeVO.GenderID = (long)DALHelper.HandleDBNull(reader["GenderID"]);
                        BizActionObj.DischargeList.Add(DischargeVO);


                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();

            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject AddMultipleBed(IValueObject valueObject, clsUserVO UserVO)
        {
            clsAddIPDBedTransferBizActionVO BizActionObj = valueObject as clsAddIPDBedTransferBizActionVO;
            try
            {
                clsIPDBedTransferVO objBedVO = BizActionObj.BedDetails;
                List<clsIPDBedTransferVO> objBedList = BizActionObj.BedList;
                foreach (clsIPDBedTransferVO item in BizActionObj.BedList)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddMultipleIPDBed");
                    dbServer.AddInParameter(command, "PatientId", DbType.Int64, item.PatientID);
                    dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, item.PatientUnitID);
                    dbServer.AddInParameter(command, "IPDAdmissionID", DbType.Int64, item.IPDAdmissionID);
                    dbServer.AddInParameter(command, "TransferDate", DbType.DateTime, item.TransferDate);
                    dbServer.AddInParameter(command, "ToBedCategoryID", DbType.Int64, item.BedCategoryID);
                    dbServer.AddInParameter(command, "ToWardID", DbType.Int64, item.WardID);
                    dbServer.AddInParameter(command, "ToBedID", DbType.Int64, item.BedID);
                    dbServer.AddInParameter(command, "IPDAdmissionNO", DbType.String, item.IPDAdmissionNo);
                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVO.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);

                    dbServer.AddInParameter(command, "BillingToBedCategoryID", DbType.Int64, item.BillingToBedCategoryID);

                    dbServer.AddOutParameter(command, "ID", DbType.Int64, int.MaxValue);
                    int intStatus = dbServer.ExecuteNonQuery(command);
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

        public override IValueObject GetDischargeStatusDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDischargeStatusBizActionVO BizActionObj = valueObject as clsGetDischargeStatusBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDischargeStatusDetails");
                dbServer.AddInParameter(command, "AdmissionID", DbType.Int64, BizActionObj.DischargeDetails.AdmID);
                dbServer.AddInParameter(command, "AdmissionUnitID ", DbType.Int64, BizActionObj.DischargeDetails.AdmUnitID);
                DbDataReader reader;
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DischargeList == null)
                        BizActionObj.DischargeList = new List<clsIPDDischargeVO>();
                    while (reader.Read())
                    {
                        clsIPDDischargeVO DischargeVO = new clsIPDDischargeVO();
                        DischargeVO.rownum = Convert.ToInt64(DALHelper.HandleDBNull(reader["rownum"]));
                        DischargeVO.AdmID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmID"]));
                        DischargeVO.AdmUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmUnitID"]));
                        DischargeVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        DischargeVO.DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"]));
                        DischargeVO.DepartmentName = Convert.ToString(DALHelper.HandleDBNull(reader["Department"]));
                        DischargeVO.AdviseAuthorityName = Convert.ToString(DALHelper.HandleDBNull(reader["AdviseAuthorityName"]));
                        DischargeVO.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["remark"]));
                        BizActionObj.DischargeList.Add(DischargeVO);
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
    }
}
