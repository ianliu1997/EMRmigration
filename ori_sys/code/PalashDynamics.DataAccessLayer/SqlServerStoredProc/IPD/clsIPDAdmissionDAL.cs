using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
using PalashDynamics.ValueObjects;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects.IPD;
using System.Data.Common;
using System.Data;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IPD
{
    public class clsIPDAdmissionDAL : clsBaseIPDAdmissionDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsIPDAdmissionDAL()
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

        public override IValueObject Save(IValueObject valueObject, clsUserVO UserVo)
        {
            clsSaveIPDAdmissionBizActionVO BizActionObj = valueObject as clsSaveIPDAdmissionBizActionVO;
            if (BizActionObj.Details.ID == 0)
                BizActionObj = AddDetails(BizActionObj, UserVo);
            else
                BizActionObj = UpdateDetails(BizActionObj, UserVo);

            return valueObject;
        }

        private clsSaveIPDAdmissionBizActionVO AddDetails(clsSaveIPDAdmissionBizActionVO BizActionObj, clsUserVO UserVo)
        {

            DbTransaction trans = null;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                con.Open();
                trans = con.BeginTransaction();
                clsIPDAdmissionVO objDetailsVO = BizActionObj.Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateIPDAdmissionNew");
                dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientId", DbType.Int64, objDetailsVO.PatientId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "IPDNO", DbType.String, objDetailsVO.IPDNO);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, objDetailsVO.DepartmentID);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objDetailsVO.DoctorID);
                dbServer.AddInParameter(command, "MLC", DbType.Boolean, objDetailsVO.MLC);
                dbServer.AddOutParameter(command, "IPDAdmissionNO", DbType.String, int.MaxValue);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Details.ID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.Details.IPDAdmissionNO = Convert.ToString(dbServer.GetParameterValue(command, "IPDAdmissionNO"));
                foreach (var item in BizActionObj.List)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdateIPDAdmissionDetailsNew");
                    dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "AdmissionID", DbType.Int64, BizActionObj.Details.ID);
                    dbServer.AddInParameter(command1, "PatientSourceID", DbType.Int64, objDetailsVO.PatientSourceID);
                    dbServer.AddInParameter(command1, "ReferingEntityID", DbType.Int64, objDetailsVO.RefEntityTypeID);
                    dbServer.AddInParameter(command1, "ReferingEntityTypeID", DbType.Int64, objDetailsVO.RefEntityID);
                    dbServer.AddInParameter(command1, "AdmissionType", DbType.Int64, objDetailsVO.AdmissionTypeID);
                    dbServer.AddInParameter(command1, "ProvDiagnosis", DbType.String, objDetailsVO.ProvisionalDiagnosis);
                    dbServer.AddInParameter(command1, "Remark", DbType.String, objDetailsVO.Remarks);
                    dbServer.AddInParameter(command1, "BedID", DbType.Int64, item.ID);
                    dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, objDetailsVO.Status);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdateBedTranferNew");
                    dbServer.AddInParameter(command2, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "IPDAdmissionNO", DbType.String, BizActionObj.Details.IPDAdmissionNO);
                    dbServer.AddInParameter(command2, "PatientId", DbType.Int64, objDetailsVO.PatientId);
                    dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, objDetailsVO.PatientUnitID);
                    dbServer.AddInParameter(command2, "IPDAdmissionID", DbType.Int64, BizActionObj.Details.ID);
                    dbServer.AddInParameter(command2, "TransferDate", DbType.DateTime, objDetailsVO.AddedDateTime);
                    dbServer.AddInParameter(command2, "ToBedCategoryID", DbType.Int64, item.BedCategoryID);
                    dbServer.AddInParameter(command2, "ToWardID", DbType.Int64, item.WardID);
                    dbServer.AddInParameter(command2, "ToBedID", DbType.Int64, item.ID);
                    dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                    dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);

                    dbServer.AddInParameter(command2, "BillingToBedCategoryID", DbType.Int64, item.BillingToBedCategoryID);  // For IPD Billing Class

                    int intStatus3 = dbServer.ExecuteNonQuery(command2, trans);

                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                string err = ex.Message;
            }
            finally
            {
                trans.Dispose();
                trans = null;
            }
            return BizActionObj;

            #region Old SP
            //try
            //{
            //    clsIPDAdmissionVO objDetailsVO = BizActionObj.Details;

            //    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateIPDAdmission");

            //    dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);
            //    dbServer.AddInParameter(command, "Time", DbType.DateTime, objDetailsVO.Time);
            //    dbServer.AddInParameter(command, "PatientId", DbType.Int64, objDetailsVO.PatientId);
            //    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objDetailsVO.PatientUnitID);
            //    dbServer.AddInParameter(command, "AdmissionTypeID", DbType.Int64, objDetailsVO.AdmissionTypeID);
            //    dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, objDetailsVO.DepartmentID);
            //    dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objDetailsVO.DoctorID);
            //    if (objDetailsVO.RefferedDoctor != null) objDetailsVO.RefferedDoctor = objDetailsVO.RefferedDoctor.Trim();
            //    dbServer.AddInParameter(command, "RefferedDoctor", DbType.String, objDetailsVO.RefferedDoctor);
            //    dbServer.AddInParameter(command, "MLC", DbType.Boolean, objDetailsVO.MLC);
            //    dbServer.AddInParameter(command, "BedCategoryID", DbType.Int64, objDetailsVO.BedCategoryID);
            //    dbServer.AddInParameter(command, "WardID", DbType.Int64, objDetailsVO.WardID);
            //    dbServer.AddInParameter(command, "BedID", DbType.Int64, objDetailsVO.BedID);
            //    if (objDetailsVO.KinName != null) objDetailsVO.KinName = objDetailsVO.KinName.Trim();
            //    dbServer.AddInParameter(command, "KinName", DbType.String, objDetailsVO.KinName);
            //    dbServer.AddInParameter(command, "KinRelationID", DbType.Int64, objDetailsVO.KinRelationID);
            //    if (objDetailsVO.KinAddress != null) objDetailsVO.KinAddress = objDetailsVO.KinAddress.Trim();
            //    dbServer.AddInParameter(command, "KinAddress", DbType.String, objDetailsVO.KinAddress);
            //    if (objDetailsVO.kinPhone != null) objDetailsVO.kinPhone = objDetailsVO.kinPhone.Trim();
            //    dbServer.AddInParameter(command, "kinPhone", DbType.String, objDetailsVO.kinPhone);
            //    if (objDetailsVO.KinMobile != null) objDetailsVO.KinMobile = objDetailsVO.KinMobile.Trim();
            //    dbServer.AddInParameter(command, "KinMobile", DbType.String, objDetailsVO.KinMobile);
            //    dbServer.AddInParameter(command, "Doctor1_ID", DbType.Int64, objDetailsVO.Doctor1_ID);
            //    dbServer.AddInParameter(command, "Doctor2_ID", DbType.Int64, objDetailsVO.Doctor2_ID);
            //    if (objDetailsVO.Remarks != null) objDetailsVO.Remarks = objDetailsVO.Remarks.Trim();
            //    dbServer.AddInParameter(command, "Remarks", DbType.String, objDetailsVO.Remarks);

            //    dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);

            //    dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            //    dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

            //    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
            //    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
            //    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
            //    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


            //    dbServer.AddParameter(command, "AdmissionNO", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "????????????????????");
            //    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
            //    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

            //    int intStatus = dbServer.ExecuteNonQuery(command);

            //    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
            //    BizActionObj.Details.ID = (long)dbServer.GetParameterValue(command, "ID");
            //    BizActionObj.Details.AdmissionNO = (string)dbServer.GetParameterValue(command, "AdmissionNO");

            //}
            //catch (Exception ex)
            //{

            //    string err = ex.Message;
            //}
            //finally
            //{

            //}
            //return BizActionObj;

            #endregion

        }

        #region OLD Code Commented By CDS
        //private clsSaveIPDAdmissionBizActionVO UpdateDetails(clsSaveIPDAdmissionBizActionVO BizActionObj, clsUserVO UserVo)
        //{
        //    try
        //    {
        //        clsIPDAdmissionVO objDetailsVO = BizActionObj.Details;
        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateIPDAdmission");

        //        dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);
        //        dbServer.AddInParameter(command, "Time", DbType.DateTime, objDetailsVO.Time);
        //        dbServer.AddInParameter(command, "PatientId", DbType.Int64, objDetailsVO.PatientId);
        //        dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objDetailsVO.PatientUnitID);
        //        dbServer.AddInParameter(command, "AdmissionTypeID", DbType.Int64, objDetailsVO.AdmissionTypeID);
        //        dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, objDetailsVO.DepartmentID);
        //        dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objDetailsVO.DoctorID);
        //        if (objDetailsVO.RefferedDoctor != null) objDetailsVO.RefferedDoctor = objDetailsVO.RefferedDoctor.Trim();
        //        dbServer.AddInParameter(command, "RefferedDoctor", DbType.String, objDetailsVO.RefferedDoctor);
        //        dbServer.AddInParameter(command, "MLC", DbType.Boolean, objDetailsVO.MLC);
        //        dbServer.AddInParameter(command, "BedCategoryID", DbType.Int64, objDetailsVO.BedCategoryID);
        //        dbServer.AddInParameter(command, "WardID", DbType.Int64, objDetailsVO.WardID);
        //        dbServer.AddInParameter(command, "BedID", DbType.Int64, objDetailsVO.BedID);
        //        if (objDetailsVO.KinName != null) objDetailsVO.KinName = objDetailsVO.KinName.Trim();
        //        dbServer.AddInParameter(command, "KinName", DbType.String, objDetailsVO.KinName);
        //        dbServer.AddInParameter(command, "KinRelationID", DbType.Int64, objDetailsVO.KinRelationID);
        //        if (objDetailsVO.KinAddress != null) objDetailsVO.KinAddress = objDetailsVO.KinAddress.Trim();
        //        dbServer.AddInParameter(command, "KinAddress", DbType.String, objDetailsVO.KinAddress);
        //        if (objDetailsVO.kinPhone != null) objDetailsVO.kinPhone = objDetailsVO.kinPhone.Trim();
        //        dbServer.AddInParameter(command, "kinPhone", DbType.String, objDetailsVO.kinPhone);
        //        if (objDetailsVO.KinMobile != null) objDetailsVO.KinMobile = objDetailsVO.KinMobile.Trim();
        //        dbServer.AddInParameter(command, "KinMobile", DbType.String, objDetailsVO.KinMobile);
        //        dbServer.AddInParameter(command, "Doctor1_ID", DbType.Int64, objDetailsVO.Doctor1_ID);
        //        dbServer.AddInParameter(command, "Doctor2_ID", DbType.Int64, objDetailsVO.Doctor2_ID);
        //        if (objDetailsVO.Remarks != null) objDetailsVO.Remarks = objDetailsVO.Remarks.Trim();
        //        dbServer.AddInParameter(command, "Remarks", DbType.String, objDetailsVO.Remarks);

        //        dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);

        //        dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

        //        dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
        //        dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //        dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
        //        dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


        //        dbServer.AddParameter(command, "AdmissionNO", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "????????????????????");
        //        dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
        //        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

        //        int intStatus = dbServer.ExecuteNonQuery(command);
        //        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

        //    }
        //    catch (Exception ex)
        //    {

        //        string msg = ex.Message;
        //    }
        //    finally
        //    {

        //    }
        //    return BizActionObj;
        //}
        #endregion

        private clsSaveIPDAdmissionBizActionVO UpdateDetails(clsSaveIPDAdmissionBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsIPDAdmissionVO objDetailsVO = BizActionObj.Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateIPDAdmissionNew");
                dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientId", DbType.Int64, objDetailsVO.PatientId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "IPDNO", DbType.String, objDetailsVO.IPDNO);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, objDetailsVO.DepartmentID);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objDetailsVO.DoctorID);
                dbServer.AddInParameter(command, "MLC", DbType.Boolean, BizActionObj.IsMedicoLegalCase);
                dbServer.AddOutParameter(command, "IPDAdmissionNO", DbType.String, int.MaxValue);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Details.ID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.Details.IPDAdmissionNO = Convert.ToString(dbServer.GetParameterValue(command, "IPDAdmissionNO"));
                if (BizActionObj.IsMedicoLegalCase == true)
                {
                    AddAdmMLDCDetails(BizActionObj);
                }
            }
            catch (Exception ex)
            {

                string msg = ex.Message;
            }
            finally
            {

            }
            return BizActionObj;
        }

        private void AddAdmMLDCDetails(clsSaveIPDAdmissionBizActionVO BizActionObj)
        {
            DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddAdmMLCDetails");
            dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.Details.UnitId);
            dbServer.AddInParameter(command, "AdmID", DbType.Int64, BizActionObj.Details.ID);
            dbServer.AddInParameter(command, "AdmUnitID", DbType.Int64, BizActionObj.Details.UnitId);
            dbServer.AddInParameter(command, "PoliceStation", DbType.String, BizActionObj.AdmMLDCDetails.PoliceStation);
            dbServer.AddInParameter(command, "Authority", DbType.String, BizActionObj.AdmMLDCDetails.Authority);
            dbServer.AddInParameter(command, "Number", DbType.String, BizActionObj.AdmMLDCDetails.Number);
            dbServer.AddInParameter(command, "Address", DbType.String, BizActionObj.AdmMLDCDetails.Address);
            dbServer.AddInParameter(command, "Remark", DbType.String, BizActionObj.AdmMLDCDetails.Remark);
            dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.AdmMLDCDetails.Description);
            dbServer.AddInParameter(command, "Title", DbType.String, BizActionObj.AdmMLDCDetails.Title);
            dbServer.AddInParameter(command, "AttachedFileName", DbType.String, BizActionObj.AdmMLDCDetails.AttachedFileName);
            dbServer.AddInParameter(command, "AttachedFileContent", DbType.Binary, BizActionObj.AdmMLDCDetails.AttachedFileContent);
            dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
            dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
            int intStatus = dbServer.ExecuteNonQuery(command);
        }

        public override IValueObject Get(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIPDAdmissionBizActionVO BizActionObj = valueObject as clsGetIPDAdmissionBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetIPDAdmission");
                DbDataReader reader;

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    BizActionObj.Details = new clsIPDAdmissionVO();

                    while (reader.Read())
                    {
                        BizActionObj.Details.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);
                        BizActionObj.Details.Time = (DateTime)DALHelper.HandleDate(reader["Time"]);

                        BizActionObj.Details.PatientId = (Int64)DALHelper.HandleDBNull(reader["PatientId"]);
                        BizActionObj.Details.PatientUnitID = (Int64)DALHelper.HandleDBNull(reader["PatientUnitID"]);
                        BizActionObj.Details.AdmissionTypeID = (Int64)DALHelper.HandleDBNull(reader["AdmissionTypeID"]);
                        BizActionObj.Details.DepartmentID = (Int64)DALHelper.HandleDBNull(reader["DepartmentID"]);
                        BizActionObj.Details.DoctorID = (Int64)DALHelper.HandleDBNull(reader["DoctorID"]);

                        BizActionObj.Details.RefferedDoctor = (string)DALHelper.HandleDBNull(reader["RefferedDoctor"]);
                        BizActionObj.Details.MLC = (bool)DALHelper.HandleDBNull(reader["MLC"]);
                        BizActionObj.Details.BedCategoryID = (Int64)DALHelper.HandleDBNull(reader["BedCategoryID"]);
                        BizActionObj.Details.WardID = (Int64)DALHelper.HandleDBNull(reader["WardID"]);
                        BizActionObj.Details.BedID = (Int64)DALHelper.HandleDBNull(reader["BedID"]);
                        BizActionObj.Details.KinRelationID = (Int64)DALHelper.HandleDBNull(reader["KinRelationID"]);
                        BizActionObj.Details.KinAddress = (string)DALHelper.HandleDBNull(reader["KinAddress"]);
                        BizActionObj.Details.kinPhone = (string)DALHelper.HandleDBNull(reader["kinPhone"]);
                        BizActionObj.Details.KinMobile = (string)DALHelper.HandleDBNull(reader["KinMobile"]);
                        BizActionObj.Details.Doctor1_ID = (Int64)DALHelper.HandleDBNull(reader["Doctor1_ID"]);
                        BizActionObj.Details.Doctor2_ID = (Int64)DALHelper.HandleDBNull(reader["Doctor2_ID"]);
                        BizActionObj.Details.Remarks = (string)DALHelper.HandleDBNull(reader["KinMobile"]);

                        BizActionObj.Details.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                        BizActionObj.Details.CreatedUnitId = (Int64)DALHelper.HandleDBNull(reader["CreatedUnitId"]);
                        BizActionObj.Details.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                        BizActionObj.Details.AddedBy = (Int64?)DALHelper.HandleDBNull(reader["AddedBy"]);
                        BizActionObj.Details.AddedOn = (String)DALHelper.HandleDBNull(reader["AddedOn"]);
                        BizActionObj.Details.AddedDateTime = (DateTime?)DALHelper.HandleDate(reader["AddedDateTime"]);
                        BizActionObj.Details.AddedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]);

                        BizActionObj.Details.UpdatedUnitId = (Int64?)DALHelper.HandleDBNull(reader["UpdatedUnitID"]);
                        BizActionObj.Details.UpdatedBy = (Int64?)DALHelper.HandleDBNull(reader["UpdatedBy"]);
                        BizActionObj.Details.UpdatedOn = (String)DALHelper.HandleDBNull(reader["UpdatedOn"]);
                        BizActionObj.Details.UpdatedDateTime = (DateTime?)DALHelper.HandleDate(reader["UpdatedDateTime"]);
                        BizActionObj.Details.UpdatedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["UpdatedWindowsLoginName"]);

                    }

                }


                reader.Close();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
            finally
            {

            }

            return valueObject;

        }

        public override IValueObject GetMedicoLegalCase(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIPDAdmissionListBizActionVO BizActionObj = valueObject as clsGetIPDAdmissionListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetMedicoLegalCaseDetailsByAdmID");
                dbServer.AddInParameter(command, "AdmID", DbType.Int64, BizActionObj.AdmDetails.ID);
                dbServer.AddInParameter(command, "AdmUnitID", DbType.Int64, BizActionObj.AdmDetails.UnitId);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.AdmMLDCDetails == null)
                        BizActionObj.AdmMLDCDetails = new clsIPDAdmMLCDetailsVO();
                    while (reader.Read())
                    {
                        clsIPDAdmMLCDetailsVO objAdmMLCDVO = new clsIPDAdmMLCDetailsVO();
                        objAdmMLCDVO.PoliceStation = Convert.ToString(DALHelper.HandleDBNull(reader["PoliceStation"]));
                        objAdmMLCDVO.IsMLC = Convert.ToBoolean(DALHelper.HandleDBNull(reader["MLC"]));
                        objAdmMLCDVO.Number = Convert.ToString(DALHelper.HandleDBNull(reader["Number"]));
                        objAdmMLCDVO.Authority = Convert.ToString(DALHelper.HandleDBNull(reader["Authority"]));
                        objAdmMLCDVO.Address = Convert.ToString(DALHelper.HandleDBNull(reader["Address"]));
                        objAdmMLCDVO.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        objAdmMLCDVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objAdmMLCDVO.Title = Convert.ToString(DALHelper.HandleDBNull(reader["Title"]));
                        objAdmMLCDVO.AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["MLCFileName"]));
                        objAdmMLCDVO.AttachedFileContent = (Byte[])DALHelper.HandleDBNull(reader["MLCFile"]);
                        BizActionObj.AdmMLDCDetails = objAdmMLCDVO;
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

        public override IValueObject GetAdmissionList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIPDAdmissionListBizActionVO BizActionObj = valueObject as clsGetIPDAdmissionListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientAdmissionList");
                //DbCommand command = dbServer.GetStoredProcCommand("temp_CIMS_GetPatientAdmissionList");
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.AdmDetails.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.AdmDetails.ToDate);
                dbServer.AddInParameter(command, "PatientId", DbType.Int64, BizActionObj.AdmDetails.PatientId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.AdmDetails.PatientUnitID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.AdmDetails.UnitId);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.AdmDetails.DoctorID);
                dbServer.AddInParameter(command, "BedCategoryID", DbType.Int64, BizActionObj.AdmDetails.BedCategoryID);
                dbServer.AddInParameter(command, "WardID", DbType.Int64, BizActionObj.AdmDetails.WardID);
                dbServer.AddInParameter(command, "strWard", DbType.String, BizActionObj.AdmDetails.strWard);
                dbServer.AddInParameter(command, "MrNo", DbType.String, BizActionObj.AdmDetails.MRNo);
                dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.AdmDetails.FirstName);
                dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.AdmDetails.LastName);
                dbServer.AddInParameter(command, "OldRegistrationNo", DbType.String, BizActionObj.AdmDetails.OldRegistrationNo);
                
            
                if (!BizActionObj.AdmDetails.IsAllPatient.Equals(true))
                {
                    if (BizActionObj.AdmDetails.CurrentAdmittedList.Equals(true))
                    {
                        dbServer.AddInParameter(command, "IsDischarged", DbType.String, "false");
                        dbServer.AddInParameter(command, "IsCancel", DbType.String, "false");
                        dbServer.AddInParameter(command, "IsClosed", DbType.String, "false");
                    }
                    if (BizActionObj.AdmDetails.IsCancelled.Equals(true))
                        dbServer.AddInParameter(command, "IsCancel", DbType.String, "true");
                    if (BizActionObj.AdmDetails.IsMedicoLegalCase.Equals(true))
                        dbServer.AddInParameter(command, "IsMedicoLegalCase", DbType.String, "true");
                    if (BizActionObj.AdmDetails.IsNonPresence.Equals(true))
                        dbServer.AddInParameter(command, "IsNonPresence", DbType.String, "true");
                }
                dbServer.AddInParameter(command, "UnRegistered", DbType.Boolean, BizActionObj.AdmDetails.UnRegistered);
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.AdmList == null)
                        BizActionObj.AdmList = new List<clsIPDAdmissionVO>();
                    while (reader.Read())
                    {
                        clsIPDAdmissionVO objAdmVO = new clsIPDAdmissionVO();
                        objAdmVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objAdmVO.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objAdmVO.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                        objAdmVO.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                        objAdmVO.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                        objAdmVO.Number = (string)DALHelper.HandleDBNull(reader["ContactNo1"]);
                        objAdmVO.IPDNO = (string)DALHelper.HandleDBNull(reader["IPDNO"]);
                        objAdmVO.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["Preffix"])) + " " + objAdmVO.FirstName + " " + objAdmVO.LastName; //Added on Dated 30012017
                        objAdmVO.GenderName = (string)DALHelper.HandleDBNull(reader["Gender"]);
                        objAdmVO.DateOfBirth = (DateTime?)DALHelper.HandleDate(reader["DateOfBirth"]);
                        objAdmVO.AdmissionDate = (DateTime?)DALHelper.HandleDate(reader["AdmissionDate"]);
                        objAdmVO.DFName = (string)DALHelper.HandleDBNull(reader["DFName"]);
                        objAdmVO.DLName = (string)DALHelper.HandleDBNull(reader["DLName"]);
                        objAdmVO.DoctorName = objAdmVO.DFName + " " + objAdmVO.DLName;
                        objAdmVO.PatientId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        objAdmVO.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        objAdmVO.RefEntityTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferingEntityTypeID"]));
                        objAdmVO.RefEntityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferingEntityID"]));
                        objAdmVO.RefDoctor = (string)DALHelper.HandleDBNull(reader["RefTypeDesc"]);
                        objAdmVO.CompanyName = (string)DALHelper.HandleDBNull(reader["CompanyName"]);
                        objAdmVO.Bed = (string)DALHelper.HandleDBNull(reader["Bed"]);
                        objAdmVO.Ward = (string)DALHelper.HandleDBNull(reader["Ward"]);
                        objAdmVO.AdmissionTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionType"]));
                        objAdmVO.IsCancelled = (Boolean)DALHelper.HandleDBNull(reader["IsCancel"]);
                        objAdmVO.IsClosed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsClosed"]));
                        objAdmVO.IsDischarge = (Boolean)DALHelper.HandleDBNull(reader["ISDischarged"]);
                        objAdmVO.PatientSourceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceID"]));
                       // objAdmVO.classID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillingToBedCategoryID"]));  //Convert.ToInt64(DALHelper.HandleDBNull(reader["BedCategoryID"]));
                        objAdmVO.classID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedCategoryId"])); // Added By Bhushan 
                        objAdmVO.BillCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillCount"]));
                        objAdmVO.UnfreezedBillCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnfreezedBillCount"]));
                        //objAdmVO.GrossDiscountReasonID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GrossDiscountReasonID"]));
                        //objAdmVO.ConcessionAuthorizedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConcessionAuthorizedBy"]));
                        //objAdmVO.DischargeDate = (DateTime?)DALHelper.HandleDate(reader["DateOfBirth"]);
                        //objAdmVO.AdvanceBalance = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Advance_Balance"]));
                        objAdmVO.CompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompanyID"])); // Added By Bhushan OldRegistrationNo
                        objAdmVO.OldRegistrationNo = Convert.ToString(DALHelper.HandleDBNull(reader["OldRegistrationNo"]));
                        //Added by AJ Date 12/12/2016
                        objAdmVO.DischargeDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["DischargeDate"]));
                        if (objAdmVO.DischargeDate.ToString() == "1/1/0001 12:00:00 AM")
                        {
                            objAdmVO.DischargeDate = null;
                        }
                        else if (objAdmVO.DischargeDate.ToString() == "1/1/1900 12:00:00 AM")
                        {
                            objAdmVO.DischargeDate = null;
                        }
                        objAdmVO.Balance = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Balance"]));

                        objAdmVO.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));  //Added by AJ Date 10/2/2017   
                        objAdmVO.PatientCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientCategoryID"]));  //Added by AJ Date 22/2/2017   
                        //***//------------------
                        // Added By Bhushanp 02/02/2017
                        //objAdmVO.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillID"]));
                        //objAdmVO.BillUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillUnitID"]));
                        objAdmVO.BabyWeight = Convert.ToString(DALHelper.HandleDBNull(reader["BabyWeight"]));
                       // objAdmVO.DischargeDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["DischargeDate"]));
                        objAdmVO.GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]));  //Added by Bhushanp Date 31/03/2017   

                        objAdmVO.TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"]));    // Added on 18Feb2019 for Package Flow in IPD

                        BizActionObj.AdmList.Add(objAdmVO);
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

        public override IValueObject GetPatientDischargeApprovalList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIPDAdmissionListBizActionVO BizActionObj = valueObject as clsGetIPDAdmissionListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientDischargeApprovalList");
                //DbCommand command = dbServer.GetStoredProcCommand("temp_CIMS_GetPatientAdmissionList");
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.AdmDetails.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.AdmDetails.ToDate);
                dbServer.AddInParameter(command, "PatientId", DbType.Int64, BizActionObj.AdmDetails.PatientId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.AdmDetails.PatientUnitID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.AdmDetails.UnitId);
                dbServer.AddInParameter(command, "BedCategoryID", DbType.Int64, BizActionObj.AdmDetails.BedCategoryID);
                dbServer.AddInParameter(command, "WardID", DbType.Int64, BizActionObj.AdmDetails.WardID);                
                dbServer.AddInParameter(command, "MrNo", DbType.String, BizActionObj.AdmDetails.MRNo);                
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.AdmList == null)
                        BizActionObj.AdmList = new List<clsIPDAdmissionVO>();
                    while (reader.Read())
                    {
                        clsIPDAdmissionVO objAdmVO = new clsIPDAdmissionVO();
                        objAdmVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objAdmVO.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objAdmVO.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                        objAdmVO.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["PatientName"]));
                        objAdmVO.Number = (string)DALHelper.HandleDBNull(reader["ContactNo1"]);
                        objAdmVO.IPDNO = (string)DALHelper.HandleDBNull(reader["IPDNO"]);
                        objAdmVO.PatientName = (string)DALHelper.HandleDBNull(reader["PatientName"]);
                        objAdmVO.GenderName = (string)DALHelper.HandleDBNull(reader["Gender"]);
                        objAdmVO.DateOfBirth = (DateTime?)DALHelper.HandleDate(reader["DateOfBirth"]);
                        objAdmVO.AdmissionDate = (DateTime?)DALHelper.HandleDate(reader["AdmissionDate"]);
                        objAdmVO.DFName = (string)DALHelper.HandleDBNull(reader["DFName"]);
                        objAdmVO.DLName = (string)DALHelper.HandleDBNull(reader["DLName"]);
                        objAdmVO.DoctorName = "Dr"+" "+ objAdmVO.DFName + " " + objAdmVO.DLName;
                        objAdmVO.PatientId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        objAdmVO.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        objAdmVO.RefEntityTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferingEntityTypeID"]));
                        objAdmVO.RefEntityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferingEntityID"]));
                        objAdmVO.RefDoctor = (string)DALHelper.HandleDBNull(reader["RefTypeDesc"]);
                        objAdmVO.CompanyName = (string)DALHelper.HandleDBNull(reader["CompanyName"]);
                        objAdmVO.Bed = (string)DALHelper.HandleDBNull(reader["Bed"]);
                        objAdmVO.Ward = (string)DALHelper.HandleDBNull(reader["Ward"]);
                        objAdmVO.AdmissionTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionType"]));
                        objAdmVO.IsCancelled = (Boolean)DALHelper.HandleDBNull(reader["IsCancel"]);
                        objAdmVO.IsClosed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsClosed"]));
                        objAdmVO.IsDischarge = (Boolean)DALHelper.HandleDBNull(reader["ISDischarged"]);
                        objAdmVO.PatientSourceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceID"]));
                        objAdmVO.classID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedCategoryId"])); // Added By Bhushan 
                        objAdmVO.BedClass = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]));
                        objAdmVO.CompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompanyID"])); 
                        BizActionObj.AdmList.Add(objAdmVO);
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


        public override IValueObject GetConsentByTempleteID(IValueObject valueObject, clsUserVO UserVo)
        {
            throw new NotImplementedException();
        }

        public override IValueObject GetActiveAdmissionOfRegisteredPatient(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetActiveAdmissionBizActionVO BizActionObj = valueObject as clsGetActiveAdmissionBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetActiveAdmissionOfRegisteredPatient");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "MRNo", DbType.String, BizActionObj.MRNo);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizActionObj.Details = new clsIPDAdmissionVO();
                        BizActionObj.Details.AdmID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizActionObj.Details.AdmissionUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                    }
                }
                reader.NextResult();
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

        public override IValueObject UpdateAdmissionType(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateAdmissionTypeBizActionVO BizActionObj = valueObject as clsUpdateAdmissionTypeBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateAdmtype");
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.AdmID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.AdmUnitID);
                dbServer.AddInParameter(command, "AdmissionType", DbType.Int64, BizActionObj.AdmTypeID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, BizActionObj.UpdateAdmType.UpdatedUnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, BizActionObj.UpdateAdmType.UpdatedBy);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, BizActionObj.UpdateAdmType.UpdatedOn);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, BizActionObj.UpdateAdmType.UpdatedWindowsLoginName);
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
            finally
            {

            }
            return BizActionObj;
        }

        public override IValueObject GetRefEntityTransfer(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRefEntityDetailsBizActionVO BizAction = valueObject as clsGetRefEntityDetailsBizActionVO;
            BizAction.List = new List<clsRefEntityDetailsVO>();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("GetRefEntityDetails");
                dbServer.AddInParameter(command, "AdmissionID", DbType.Int64, BizAction.Details.AdmID);
                dbServer.AddInParameter(command, "AdmissionUnitID", DbType.Int64, BizAction.Details.AdmissionUnitID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    BizAction.List = new List<clsRefEntityDetailsVO>();
                    while (reader.Read())
                    {
                        clsRefEntityDetailsVO Details = new clsRefEntityDetailsVO();
                        Details.RefEntityIDDesc = Convert.ToString(DALHelper.HandleDBNull(reader["RefTypeDesc"]));
                        Details.RefEntityTypeIDDesc = Convert.ToString(DALHelper.HandleDBNull(reader["RefEntityTypeDesc"]));
                        Details.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        BizAction.List.Add(Details);
                    }
                }
                reader.NextResult();
                reader.Close();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
            return BizAction;
        }
        public override IValueObject AddRefEntityTransfer(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddRefEntityDetailsBizActionVO BizActionObj = valueObject as clsAddRefEntityDetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("AddRefEntityDetails");
                dbServer.AddInParameter(command, "ID", DbType.Int64, 0);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.Details.UnitId);
                dbServer.AddInParameter(command, "AdmissionID", DbType.Int64, BizActionObj.Details.AdmID);
                dbServer.AddInParameter(command, "AdmissionUnitID", DbType.Int64, BizActionObj.Details.AdmissionUnitID);
                dbServer.AddInParameter(command, "RefEntityID", DbType.Int64, BizActionObj.Details.RefEntityID);
                dbServer.AddInParameter(command, "RefEntityTypeID", DbType.Int64, BizActionObj.Details.RefEntityTypeID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
            return BizActionObj;
        }

        public override IValueObject AddAdviseDischargeList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddIPDAdviseDischargeListBizActionVO BizActionObj = valueObject as clsAddIPDAdviseDischargeListBizActionVO;
            BizActionObj = AddAdviseDischargeList(BizActionObj, UserVo);
            return valueObject;
        }

        private clsAddIPDAdviseDischargeListBizActionVO AddAdviseDischargeList(clsAddIPDAdviseDischargeListBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();
                clsIPDAdmissionVO objVO = BizActionObj.AddAdviseDetails;
                if (BizActionObj.AddAdviseDList != null)
                {
                    foreach (var item in BizActionObj.AddAdviseDList)
                    {
                        DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddAdvisedDischargeList");
                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "AdmID", DbType.Int64, objVO.AdmID);
                        dbServer.AddInParameter(command, "AdmUnitID", DbType.Int64, objVO.AdmissionUnitID);
                        dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, item.ID);
                        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objVO.CreatedUnitId);
                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objVO.AddedBy);
                        dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objVO.AddedWindowsLoginName);
                        dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objVO.ID);
                        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                        int intStatus = dbServer.ExecuteNonQuery(command, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                        BizActionObj.AddAdviseDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
                    }
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

        public override IValueObject GetAdvisedDischargeByAdmIdAndAdmUnitID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetAdvisedDischargeByAdmIdAndAdmUnitIDBizActionVO BizActionObj = valueObject as clsGetAdvisedDischargeByAdmIdAndAdmUnitIDBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAdvisedDischargeByAdmIdAndAdmUnitID");
                dbServer.AddInParameter(command, "AdmID", DbType.Int64, BizActionObj.GetAdviseDetails.AdmID);
                dbServer.AddInParameter(command, "AdmUnitID", DbType.Int64, BizActionObj.GetAdviseDetails.AdmissionUnitID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.GetAdviseDList == null)
                        BizActionObj.GetAdviseDList = new List<clsIPDAdmissionVO>();
                    while (reader.Read())
                    {
                        clsIPDAdmissionVO objAdmVO = new clsIPDAdmissionVO();
                        objAdmVO.AdmID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmID"]));
                        objAdmVO.AdmissionUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmUnitID"]));
                        objAdmVO.DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"]));
                        objAdmVO.Status = (bool)(DALHelper.HandleDBNull(reader["ConfirmationStatus"]));
                        BizActionObj.GetAdviseDList.Add(objAdmVO);
                    }
                }
                reader.NextResult();
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
        public override IValueObject AddDischargeApprovalByDepartment(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddDiischargeApprovedByDepartmentBizActionVO BizAction = valueObject as clsAddDiischargeApprovedByDepartmentBizActionVO;
            BizAction = AddApprovedAdviseDischarge(BizAction, UserVo);
            return valueObject;
        }

        private clsAddDiischargeApprovedByDepartmentBizActionVO AddApprovedAdviseDischarge(clsAddDiischargeApprovedByDepartmentBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                //trans = con.BeginTransaction();
                clsDiischargeApprovedByDepartmentVO objVO = BizActionObj.AddAdviseDetails;
                if (BizActionObj.AddAdviseList != null)
                {
                    foreach (var item in BizActionObj.AddAdviseList)
                    {
                        DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddDischargeApprovalByDepartment");
                        dbServer.AddInParameter(command, "ID", DbType.Int64, item.ID);
                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, item.UnitId);
                        dbServer.AddInParameter(command, "AdmissionID", DbType.Int64, item.AdmissionId);
                        dbServer.AddInParameter(command, "AdmissionUnitID", DbType.Int64, item.AdmissionUnitID);
                        dbServer.AddInParameter(command, "PatientID", DbType.Int64, item.PatientId);
                        dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, item.PatientUnitID);
                        dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, item.DepartmentID);
                        dbServer.AddInParameter(command, "DepartmentName", DbType.String, item.DepartmentName);
                        dbServer.AddInParameter(command, "AdviseAuthorityName", DbType.String, item.SelectedStaff != null ? ((MasterListItem)item.SelectedStaff).Description : string.Empty);
                        dbServer.AddInParameter(command, "LoginUserID", DbType.Int64, item.LoginUserID);
                        dbServer.AddInParameter(command, "Remark", DbType.String, item.Remark);
                        dbServer.AddInParameter(command, "ApprovalStatus", DbType.Boolean, item.ApprovalStatus);
                        dbServer.AddInParameter(command, "ApprovalRemark", DbType.String, item.ApprovalRemark);
                        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                        int intStatus = dbServer.ExecuteNonQuery(command);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    }
                }
             //   trans.Commit();
            }
            catch (Exception)
            {
                //trans.Rollback();
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

        public override IValueObject UpdateDischargeApproval(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddDiischargeApprovedByDepartmentBizActionVO BizActionObj = valueObject as clsAddDiischargeApprovedByDepartmentBizActionVO;
           
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                //trans = con.BeginTransaction();
                clsDiischargeApprovedByDepartmentVO objVO = BizActionObj.AddAdviseDetails;
                if (BizActionObj.AddAdviseList != null)
                {
                    foreach (var item in BizActionObj.AddAdviseList)
                    {
                        DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateDischargeApproval");
                        dbServer.AddInParameter(command, "ID", DbType.Int64, item.ID);
                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, item.UnitId);
                        dbServer.AddInParameter(command, "AdmissionID", DbType.Int64, item.AdmissionId);
                        dbServer.AddInParameter(command, "AdmissionUnitID", DbType.Int64, item.AdmissionUnitID);
                        dbServer.AddInParameter(command, "PatientID", DbType.Int64, item.PatientId);
                        dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, item.PatientUnitID);
                        dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, item.DepartmentID);
                        dbServer.AddInParameter(command, "ApprovalStatus", DbType.Boolean, item.ApprovalStatus);
                        dbServer.AddInParameter(command, "ApprovalRemark", DbType.String, item.ApprovalRemark);
                        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                        int intStatus = dbServer.ExecuteNonQuery(command);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    }
                }
                //   trans.Commit();
            }
            catch (Exception)
            {
                //trans.Rollback();
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


        public override IValueObject GetAdviseDischargeListForApproval(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetListOfAdviseDischargeForApprovalBizActionVO BizAction = valueObject as clsGetListOfAdviseDischargeForApprovalBizActionVO;
            BizAction.AddAdviseList = new List<clsDiischargeApprovedByDepartmentVO>();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("GetListOfDischargeForApproval");
                //dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.AddAdviseDetails.UnitId);
                dbServer.AddInParameter(command, "AdmissionID", DbType.Int64, BizAction.AddAdviseDetails.AdmissionId);
                dbServer.AddInParameter(command, "AdmissionUnitID", DbType.Int64, BizAction.AddAdviseDetails.AdmissionUnitID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    BizAction.AddAdviseList = new List<clsDiischargeApprovedByDepartmentVO>();
                    while (reader.Read())
                    {
                        clsDiischargeApprovedByDepartmentVO Details = new clsDiischargeApprovedByDepartmentVO();
                        Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        Details.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        Details.DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"]));
                        Details.DepartmentName = Convert.ToString(DALHelper.HandleDBNull(reader["DepartmentName"]));
                        Details.AdmissionId = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionID"]));
                        Details.AdmissionUnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["AdmissionUnitID"]));
                        Details.PatientId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        Details.AdviseAuthorityName = Convert.ToString(DALHelper.HandleDBNull(reader["AdviseAuthorityName"]));
                        Details.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        Details.ApprovalRemark = Convert.ToString(DALHelper.HandleDBNull(reader["ApprovalRemark"]));
                        Details.ApprovalStatus = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ApprovalStatus"]));
                        Details.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        if (UserVo.UserGeneralDetailVO.DepartmentID == Details.DepartmentID )
                        {
                            Details.IsEnable = true;
                        }
                        if (Details.ApprovalStatus)
                        {
                            Details.IsEnable = false;
                        }
                        BizAction.AddAdviseList.Add(Details);
                    }
                }               
                reader.Close();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
            return BizAction;
        }

        public override IValueObject AddIPDAdmissionDetailsWithTransaction(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            //DbTransaction trans = null;
            //DbConnection con = dbServer.CreateConnection();

            clsSaveIPDAdmissionBizActionVO BizActionObj = (clsSaveIPDAdmissionBizActionVO)valueObject;

            try
            {
                //con.Open();
                //trans = con.BeginTransaction();

                if (pConnection == null)
                    pConnection = dbServer.CreateConnection();

                if (pConnection.State != ConnectionState.Open) pConnection.Open();
                if (pTransaction == null)
                    pTransaction = pConnection.BeginTransaction();

                clsIPDAdmissionVO objDetailsVO = BizActionObj.Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateIPDAdmissionNew");
                command.Connection = pConnection;

                dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientId", DbType.Int64, objDetailsVO.PatientId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "IPDNO", DbType.String, objDetailsVO.IPDNO);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, objDetailsVO.DepartmentID);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objDetailsVO.DoctorID);
                dbServer.AddInParameter(command, "MLC", DbType.Boolean, objDetailsVO.MLC);
                dbServer.AddOutParameter(command, "IPDAdmissionNO", DbType.String, int.MaxValue);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command, pTransaction);

                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Details.ID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.Details.IPDAdmissionNO = Convert.ToString(dbServer.GetParameterValue(command, "IPDAdmissionNO"));

                foreach (var item in BizActionObj.List)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdateIPDAdmissionDetailsNew");
                    command1.Connection = pConnection;

                    dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "AdmissionID", DbType.Int64, BizActionObj.Details.ID);
                    dbServer.AddInParameter(command1, "PatientSourceID", DbType.Int64, objDetailsVO.PatientSourceID);
                    dbServer.AddInParameter(command1, "ReferingEntityID", DbType.Int64, objDetailsVO.RefEntityTypeID);
                    dbServer.AddInParameter(command1, "ReferingEntityTypeID", DbType.Int64, objDetailsVO.RefEntityID);
                    dbServer.AddInParameter(command1, "AdmissionType", DbType.Int64, objDetailsVO.AdmissionTypeID);
                    dbServer.AddInParameter(command1, "ProvDiagnosis", DbType.String, objDetailsVO.ProvisionalDiagnosis);
                    dbServer.AddInParameter(command1, "Remark", DbType.String, objDetailsVO.Remarks);
                    dbServer.AddInParameter(command1, "BedID", DbType.Int64, item.ID);
                    dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, objDetailsVO.Status);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1, pTransaction);

                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdateBedTranferNew");
                    command2.Connection = pConnection;

                    dbServer.AddInParameter(command2, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "IPDAdmissionNO", DbType.String, BizActionObj.Details.IPDAdmissionNO);
                    dbServer.AddInParameter(command2, "PatientId", DbType.Int64, objDetailsVO.PatientId);
                    dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, objDetailsVO.PatientUnitID);
                    dbServer.AddInParameter(command2, "IPDAdmissionID", DbType.Int64, BizActionObj.Details.ID);
                    dbServer.AddInParameter(command2, "TransferDate", DbType.DateTime, objDetailsVO.AddedDateTime);
                    dbServer.AddInParameter(command2, "ToBedCategoryID", DbType.Int64, item.BedCategoryID);
                    dbServer.AddInParameter(command2, "ToWardID", DbType.Int64, item.WardID);
                    dbServer.AddInParameter(command2, "ToBedID", DbType.Int64, item.ID);
                    dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                    dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);

                    dbServer.AddInParameter(command2, "BillingToBedCategoryID", DbType.Int64, item.BillingToBedCategoryID);  // For IPD Billing Class

                    int intStatus3 = dbServer.ExecuteNonQuery(command2, pTransaction);

                }
                //trans.Commit();
            }
            catch (Exception ex)
            {
                //trans.Rollback();
                //string err = ex.Message;
                BizActionObj.SuccessStatus = -1;
                throw ex;
            }
            finally
            {
                //trans.Dispose();
                //trans = null;
            }
            return BizActionObj;
        }
        public override IValueObject SaveRoundTrip(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIPDRoundDetailsBizactionVO bizAction = valueObject as clsIPDRoundDetailsBizactionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddIPDRound");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, bizAction.PatientUnitID);
                //dbServer.AddInParameter(command, "SepecID", DbType.String, bizAction.SpecCode);
                //dbServer.AddInParameter(command, "SepecName", DbType.String, bizAction.SpecName);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, bizAction.DoctorId);
                //dbServer.AddInParameter(command, "DoctorName", DbType.String, bizAction.DoctorName);
                dbServer.AddInParameter(command, "AdmissionId", DbType.Int64, bizAction.AdmisstionId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.Output, null, DataRowVersion.Default, bizAction.ID);
                int intStatus = dbServer.ExecuteNonQuery(command);
                bizAction.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));
            }
            catch (Exception ex)
            {
                throw;
            }
            return bizAction;
        }
        public override IValueObject CheckIPDRoundExitsOrNot(IValueObject valueObject, clsUserVO UserVo)
        {
            ClsIPDCheckRoundExistsBizactionVO BizAction = valueObject as ClsIPDCheckRoundExistsBizactionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_CheckIPDRoundExists");
                dbServer.AddInParameter(command, "PatientId", DbType.Int64, BizAction.PatientID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizAction.VisitId);
                dbServer.AddInParameter(command, "IsOpdIpd", DbType.Boolean, BizAction.IsOpdIpd);
                dbServer.AddInParameter(command, "DoctorId", DbType.Int64, BizAction.DoctorID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                BizAction.status = Convert.ToInt64(dbServer.ExecuteScalar(command));
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizAction;
        }

        /// <summary> Added by Ashish Z.
        ///For Canceling the current patient Admission.
        /// </summary>
        public override IValueObject CancelIPDAdmission(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCancelIPDAdmissionBizactionVO BizActionObj = valueObject as clsCancelIPDAdmissionBizactionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetChargesByAdmIDAndAdmUnitID");
                dbServer.AddInParameter(command, "AdmID", DbType.Int64, BizActionObj.AdmissionID);
                dbServer.AddInParameter(command, "AdmUnitID", DbType.Int64, BizActionObj.AdmissionUnitID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.AdmissionDetailsList == null) BizActionObj.AdmissionDetailsList = new List<clsIPDAdmissionVO>();
                    while (reader.Read())
                    {
                        clsIPDAdmissionVO objAdmVO = new clsIPDAdmissionVO();
                        objAdmVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"]));
                        objAdmVO.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_UnitId"]));
                        objAdmVO.IsBilled = (bool)DALHelper.HandleDBNull(reader["IsBilled"]);
                        objAdmVO.IsCancelled = (bool)DALHelper.HandleDBNull(reader["IsCancelled"]);
                        BizActionObj.AdmissionDetailsList.Add(objAdmVO);
                    }
                }

                reader.NextResult();

                while (reader.Read())
                {
                    clsIPDAdmissionVO objAdmVO = new clsIPDAdmissionVO();
                    objAdmVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"]));
                    objAdmVO.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External_UnitId"]));
                    objAdmVO.IsBilled = (bool)DALHelper.HandleDBNull(reader["IsBilled"]);
                    BizActionObj.AdmissionDetailsList.Add(objAdmVO);
                }

                if (BizActionObj.AdmissionDetailsList != null)
                {
                    if (BizActionObj.AdmissionDetailsList.Where(S => S.IsBilled.Equals(true)).Count() > 0)
                    {
                        BizActionObj.SuccessStatus = 1; //Bill is already prepared you can not cancel the admission
                    }
                    else if (BizActionObj.AdmissionDetailsList.Where(S => S.IsBilled.Equals(false) && S.IsCancelled.Equals(false)).Count() > 0)
                    {
                        BizActionObj.SuccessStatus = 2; //cannot cancel admission, please first cancel charges.
                    }
                    else
                    {
                        BizActionObj.SuccessStatus = 3;
                        //try
                        //{
                        //    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_UpdateAdmissionDetailsByAdmIDAndAdmUnitIDForCancel");

                        //    dbServer.AddInParameter(command1, "AdmID", DbType.Int64, BizActionObj.AdmissionID);
                        //    dbServer.AddInParameter(command1, "AdmUnitID", DbType.Int64, BizActionObj.AdmissionUnitID);
                        //    dbServer.AddInParameter(command1, "IsCancel", DbType.Boolean, 1);
                        //    int intStatus = dbServer.ExecuteNonQuery(command1);
                        //    BizActionObj.SuccessStatus = 3; //Admission cancelled successfuly
                        //}
                        //catch (Exception ex)
                        //{
                        //    string err = ex.Message;
                        //    throw;
                        //}
                    }
                }
                reader.NextResult();
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject UpdateCancelIPDAdmission(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCancelIPDAdmissionBizactionVO BizActionObj = valueObject as clsCancelIPDAdmissionBizactionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateAdmissionDetailsByAdmIDAndAdmUnitIDForCancel");
                dbServer.AddInParameter(command, "AdmID", DbType.Int64, BizActionObj.AdmissionID);
                dbServer.AddInParameter(command, "AdmUnitID", DbType.Int64, BizActionObj.AdmissionUnitID);
                dbServer.AddInParameter(command, "IsCancel", DbType.Boolean, 1);
                int intStatus = dbServer.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetDoctorDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIPDRoundDoctorBizactionVO BizAction = valueObject as clsIPDRoundDoctorBizactionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorDetails");
                dbServer.AddInParameter(command, "DoctorId", DbType.Int64, BizAction.DoctorId);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizAction.DoctorName =Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]));
                        BizAction.SpecCode = Convert.ToString(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.SpecName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                    }
                }
            }
            catch(Exception ex)
            {
                throw;
            }
            return valueObject;
        }
    }
}
