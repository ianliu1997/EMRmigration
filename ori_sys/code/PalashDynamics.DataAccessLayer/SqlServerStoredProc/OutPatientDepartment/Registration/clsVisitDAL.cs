using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using System.Data.Common;
using System.Data;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using PalashDynamics.ValueObjects.Administration.UnitMaster;
using PalashDynamics.ValueObjects.IPD;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsVisitDAL : clsBaseVisitDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsVisitDAL()
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


        public override IValueObject AddVisit(IValueObject valueObject, clsUserVO UserVo)
        {

            clsAddVisitBizActionVO BizActionObj = valueObject as clsAddVisitBizActionVO;

            if (BizActionObj.VisitDetails.ID == 0)
                BizActionObj = AddVisitDetails(BizActionObj, UserVo, null, null);
            else
                BizActionObj = UpdateVisitDetails(BizActionObj, UserVo);

            return valueObject;

        }
        //By Anjali.................
        public override IValueObject AddVisit(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {

            clsAddVisitBizActionVO BizActionObj = valueObject as clsAddVisitBizActionVO;

            if (BizActionObj.VisitDetails.ID == 0)
                BizActionObj = AddVisitDetails(BizActionObj, UserVo, myConnection, myTransaction);

            return valueObject;

        }
        //..........................
        private clsAddVisitBizActionVO AddVisitDetails(clsAddVisitBizActionVO BizActionObj, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {
            DbConnection con = null;
            DbTransaction trans = null;

            try
            {
                clsVisitVO objDetailsVO = BizActionObj.VisitDetails;

                if (myConnection != null)
                    con = myConnection;
                else
                    con = dbServer.CreateConnection();


                if (con.State == ConnectionState.Closed)
                {
                    con.Open();

                }

                if (myTransaction != null)
                    trans = myTransaction;
                else
                    trans = con.BeginTransaction();




                bool ValidFlag = true;

                if (objDetailsVO.PatientUnitId != UserVo.UserLoginInfo.UnitId)
                {
                    ValidFlag = AddPatientData(objDetailsVO.PatientId, objDetailsVO.PatientUnitId);
                }

                if (ValidFlag)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddVisit");

                    dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                    if (objDetailsVO.LinkServer != null)
                        dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));
                    dbServer.AddInParameter(command, "LinkServerDBName", DbType.String, null);

                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, objDetailsVO.PatientId);
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objDetailsVO.PatientUnitId);
                    dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);
                    if (objDetailsVO.OPDNO != null) objDetailsVO.OPDNO = objDetailsVO.OPDNO.Trim();
                    dbServer.AddInParameter(command, "OPDNO", DbType.String, objDetailsVO.OPDNO);
                    dbServer.AddInParameter(command, "VisitTypeID", DbType.Int64, objDetailsVO.VisitTypeID);
                    if (objDetailsVO.Complaints != null) objDetailsVO.Complaints = objDetailsVO.Complaints.Trim();
                    dbServer.AddInParameter(command, "Complaints", DbType.String, objDetailsVO.Complaints);
                    dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, objDetailsVO.DepartmentID);
                    dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objDetailsVO.DoctorID);
                    dbServer.AddInParameter(command, "CabinID", DbType.Int64, objDetailsVO.CabinID);
                    if (objDetailsVO.ReferredDoctorID != null)

                        dbServer.AddInParameter(command, "ReferredDoctorID", DbType.Int64, objDetailsVO.ReferredDoctorID);
                    //if (objDetailsVO.ReferredDoctorID != null && objDetailsVO.ReferredDoctorID > 0)
                    //{
                    //    objDetailsVO.ReferredDoctor = objDetailsVO.ReferredDoctor.Trim();
                    dbServer.AddInParameter(command, "ReferredDoctor", DbType.String, objDetailsVO.ReferredDoctor);
                    //}
                    //else
                    //{
                    //    dbServer.AddInParameter(command, "ReferredDoctor", DbType.String, null);
                    //}

                    //dbServer.AddInParameter(command, "PatientCaseRecord", DbType.String, objDetailsVO.PatientCaseRecord);
                    //dbServer.AddInParameter(command, "CaseReferralSheet", DbType.String, objDetailsVO.CaseReferralSheet);
                    if (objDetailsVO.VisitNotes != null) objDetailsVO.VisitNotes = objDetailsVO.VisitNotes.Trim();
                    dbServer.AddInParameter(command, "VisitNotes", DbType.String, objDetailsVO.VisitNotes);

                    dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);
                    dbServer.AddInParameter(command, "VisitStatus", DbType.Boolean, objDetailsVO.VisitStatus);

                    dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    BizActionObj.VisitDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

                    if (BizActionObj.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }

                }


                if (myConnection == null) trans.Commit();
            }
            catch (Exception ex)
            {
                //throw;
                BizActionObj.SuccessStatus = -1;
                if (myConnection == null) trans.Rollback();
            }
            finally
            {
                if (myConnection == null) con.Close();
            }

            return BizActionObj;
        }

        private bool AddPatientData(long pPatientID, long pUnitID)
        {
            bool ResultStatus = true;

            try
            {
                int SuccessStatus = 0;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPatientFromOtherClinic");


                dbServer.AddInParameter(command, "PatientID", DbType.Int64, pPatientID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, pUnitID);

                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, Int32.MinValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                if (SuccessStatus == -1)
                    ResultStatus = false;
                else
                    ResultStatus = true;


            }
            catch (Exception ex)
            {
                string err = ex.Message;
                ResultStatus = false;
            }

            return ResultStatus;
        }


        private clsAddVisitBizActionVO UpdateVisitDetails(clsAddVisitBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsVisitVO objDetailsVO = BizActionObj.VisitDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateVisit");

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                if (objDetailsVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objDetailsVO.PatientId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objDetailsVO.PatientUnitId);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);
                if (objDetailsVO.OPDNO != null) objDetailsVO.OPDNO = objDetailsVO.OPDNO.Trim();
                dbServer.AddInParameter(command, "OPDNO", DbType.String, objDetailsVO.OPDNO);
                dbServer.AddInParameter(command, "VisitTypeID", DbType.Int64, objDetailsVO.VisitTypeID);
                if (objDetailsVO.Complaints != null) objDetailsVO.Complaints = objDetailsVO.Complaints.Trim();
                dbServer.AddInParameter(command, "Complaints", DbType.String, objDetailsVO.Complaints);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, objDetailsVO.DepartmentID);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objDetailsVO.DoctorID);
                dbServer.AddInParameter(command, "CabinID", DbType.Int64, objDetailsVO.CabinID);
                dbServer.AddInParameter(command, "ReferredDoctorID", DbType.Int64, objDetailsVO.ReferredDoctorID);
                if (objDetailsVO.ReferredDoctor != null) objDetailsVO.ReferredDoctor = objDetailsVO.ReferredDoctor.Trim();
                dbServer.AddInParameter(command, "ReferredDoctor", DbType.String, objDetailsVO.ReferredDoctor);
                dbServer.AddInParameter(command, "PatientCaseRecord", DbType.String, objDetailsVO.PatientCaseRecord);
                dbServer.AddInParameter(command, "CaseReferralSheet", DbType.String, objDetailsVO.CaseReferralSheet);
                if (objDetailsVO.VisitNotes != null) objDetailsVO.VisitNotes = objDetailsVO.VisitNotes.Trim();
                dbServer.AddInParameter(command, "VisitNotes", DbType.String, objDetailsVO.VisitNotes);

                //dbServer.AddInParameter(command, "UnitId", DbType.Int64, objDetailsVO.UnitId);
                //dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objDetailsVO.UpdatedUnitId);

                //dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);
                //dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objDetailsVO.UpdatedBy);
                //if (objDetailsVO.UpdatedOn != null) objDetailsVO.UpdatedOn = objDetailsVO.UpdatedOn.Trim();
                //dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objDetailsVO.UpdatedOn);
                //dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objDetailsVO.UpdatedDateTime);
                //if (objDetailsVO.UpdatedWindowsLoginName != null) objDetailsVO.UpdatedWindowsLoginName = objDetailsVO.UpdatedWindowsLoginName.Trim();
                //dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objDetailsVO.UpdatedWindowsLoginName);

                dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objDetailsVO.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objDetailsVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);



                dbServer.AddInParameter(command, "ID", DbType.Int64, objDetailsVO.ID);

                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.SuccessStatus);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

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

        public override IValueObject GetVisit(IValueObject valueObject, clsUserVO UserVo)
        {

            clsGetVisitBizActionVO BizActionObj = valueObject as clsGetVisitBizActionVO;
            try
            {
                if (BizActionObj.OPD_IPD_External == 1)
                {
                    clsGetIPDAdmissionBizActionVO objDetailsVO = BizActionObj.ObjAdmission;
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAdmission");
                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, objDetailsVO.PatientID);
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objDetailsVO.PatientUnitID);
                    dbServer.AddInParameter(command, "ID", DbType.Int64, objDetailsVO.ID);
                    if (BizActionObj.ForHO == true)
                    {
                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDetailsVO.UnitID);
                    }
                    else
                    {
                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    }
                    dbServer.AddInParameter(command, "GetLatestAdmission", DbType.Boolean, BizActionObj.GetLatestVisit);
                    DbDataReader reader;
                    reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            BizActionObj.ObjAdmission.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            BizActionObj.ObjAdmission.Details.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                            BizActionObj.ObjAdmission.Details.PatientId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientId"]));
                            BizActionObj.ObjAdmission.Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                            BizActionObj.ObjAdmission.Details.AdmissionTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionTypeID"]));
                            BizActionObj.ObjAdmission.Details.DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"]));
                            BizActionObj.ObjAdmission.Details.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                            BizActionObj.ObjAdmission.Details.BedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedID"]));
                            BizActionObj.ObjAdmission.Details.KinRelationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["KinRelationID"]));
                            BizActionObj.ObjAdmission.Details.KinAddress = Convert.ToString(DALHelper.HandleDBNull(reader["KinAddress"]));
                            BizActionObj.ObjAdmission.Details.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]));
                            BizActionObj.ObjAdmission.Details.CreatedUnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CreatedUnitId"]));
                            BizActionObj.ObjAdmission.Details.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                            BizActionObj.ObjAdmission.Details.AddedBy = (Int64?)DALHelper.HandleDBNull(reader["AddedBy"]);
                            BizActionObj.ObjAdmission.Details.AddedOn = Convert.ToString(DALHelper.HandleDBNull(reader["AddedOn"]));
                            BizActionObj.ObjAdmission.Details.AddedDateTime = (DateTime?)DALHelper.HandleDate(reader["AddedDateTime"]);
                            BizActionObj.ObjAdmission.Details.AddedWindowsLoginName = Convert.ToString(DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]));
                            BizActionObj.ObjAdmission.Details.UpdatedUnitId = (Int64?)DALHelper.HandleDBNull(reader["UpdatedUnitID"]);
                            BizActionObj.ObjAdmission.Details.UpdatedBy = (Int64?)DALHelper.HandleDBNull(reader["UpdatedBy"]);
                            BizActionObj.ObjAdmission.Details.UpdatedOn = Convert.ToString(DALHelper.HandleDBNull(reader["UpdatedOn"]));
                            BizActionObj.ObjAdmission.Details.UpdatedDateTime = (DateTime?)DALHelper.HandleDate(reader["UpdatedDateTime"]);
                            BizActionObj.Details.IsBillGeneratedAgainstVisit = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBillGeneratedAgainstVisit"]));

                        }
                    }
                    reader.Close();
                }
                else
                {
                    clsVisitVO objDetailsVO = BizActionObj.Details;
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetVisit");
                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, objDetailsVO.PatientId);
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objDetailsVO.PatientUnitId);
                    dbServer.AddInParameter(command, "ID", DbType.Int64, objDetailsVO.ID);
                    if (BizActionObj.ForHO == true)
                    {
                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDetailsVO.UnitId);
                    }
                    else
                    {
                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    }
                    dbServer.AddInParameter(command, "GetLatestVisit", DbType.Boolean, BizActionObj.GetLatestVisit);
                    DbDataReader reader;
                    reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            BizActionObj.Details.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            BizActionObj.Details.PatientId = (long)DALHelper.HandleDBNull(reader["PatientId"]);
                            BizActionObj.Details.PatientUnitId = (long)DALHelper.HandleDBNull(reader["PatientUnitId"]);
                            BizActionObj.Details.DepartmentID = (long)DALHelper.HandleDBNull(reader["DepartmentID"]);
                            BizActionObj.Details.VisitTypeID = (long)DALHelper.HandleDBNull(reader["VisitTypeID"]);
                            BizActionObj.Details.CabinID = (long)DALHelper.HandleDBNull(reader["CabinID"]);
                            BizActionObj.Details.DoctorID = (long)DALHelper.HandleDBNull(reader["DoctorID"]);
                            BizActionObj.Details.ReferredDoctor = (string)DALHelper.HandleDBNull(reader["ReferredDoctor"]);
                            BizActionObj.Details.PatientCaseRecord = (string)DALHelper.HandleDBNull(reader["PatientCaseRecord"]);
                            BizActionObj.Details.CaseReferralSheet = (string)DALHelper.HandleDBNull(reader["CaseReferralSheet"]);
                            BizActionObj.Details.Complaints = (string)DALHelper.HandleDBNull(reader["Complaints"]);
                            BizActionObj.Details.VisitNotes = (string)DALHelper.HandleDBNull(reader["VisitNotes"]);
                            BizActionObj.Details.Date = (DateTime)DALHelper.HandleDBNull(reader["Date"]);
                            BizActionObj.Details.OPDNO = (string)DALHelper.HandleDBNull(reader["OPDNO"]);
                            BizActionObj.Details.VisitStatus = (bool)DALHelper.HandleDBNull(reader["VisitStatus"]);
                            BizActionObj.Details.CurrentVisitStatus = (VisitCurrentStatus)DALHelper.HandleDBNull(reader["CurrentVisitStatus"]);
                            BizActionObj.Details.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                            BizActionObj.Details.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                            BizActionObj.Details.CreatedUnitId = (Int64)DALHelper.HandleDBNull(reader["CreatedUnitId"]);
                            BizActionObj.Details.AddedBy = (Int64?)DALHelper.HandleDBNull(reader["AddedBy"]);
                            BizActionObj.Details.AddedOn = (String)DALHelper.HandleDBNull(reader["AddedOn"]);
                            BizActionObj.Details.AddedDateTime = (DateTime?)DALHelper.HandleDate(reader["AddedDateTime"]);
                            BizActionObj.Details.AddedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]);
                            BizActionObj.Details.UpdatedUnitId = (Int64?)DALHelper.HandleDBNull(reader["UpdatedUnitID"]);
                            BizActionObj.Details.UpdatedBy = (Int64?)DALHelper.HandleDBNull(reader["UpdatedBy"]);
                            BizActionObj.Details.UpdatedOn = (String)DALHelper.HandleDBNull(reader["UpdatedOn"]);
                            BizActionObj.Details.UpdatedDateTime = (DateTime?)DALHelper.HandleDate(reader["UpdatedDateTime"]);
                            BizActionObj.Details.UpdatedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["UpdatedWindowsLoginName"]);
                            BizActionObj.Details.Department = (string)DALHelper.HandleDBNull(reader["Department"]);
                            BizActionObj.Details.VisitType = (string)DALHelper.HandleDBNull(reader["VisitType"]);
                            BizActionObj.Details.Cabin = (string)DALHelper.HandleDBNull(reader["Cabin"]);
                            BizActionObj.Details.Doctor = (string)DALHelper.HandleDBNull(reader["Doctor"]);
                            BizActionObj.Details.IsBillGeneratedAgainstVisit = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBillGeneratedAgainstVisit"]));

                        }
                    }
                    reader.Close();
                }


            }
            catch (Exception ex)
            {

                // logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return valueObject;
        }


        public override IValueObject GetVisitCount(IValueObject valueObject, clsUserVO UserVo)
        {

            clsGetVisitBizActionVO BizActionObj = valueObject as clsGetVisitBizActionVO;
            try
            {
                clsVisitVO objDetailsVO = BizActionObj.Details;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetVisitCount");

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objDetailsVO.PatientId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objDetailsVO.PatientUnitId);
                dbServer.AddInParameter(command, "ID", DbType.Int64, objDetailsVO.ID);
                if (BizActionObj.ForHO == true)
                {
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDetailsVO.UnitId);
                }
                else
                {
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                }

                DbDataReader reader;
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizActionObj.Details.VisitCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitCount"]));

                        //BizActionObj.Details.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        //BizActionObj.Details.PatientId = (long)DALHelper.HandleDBNull(reader["PatientId"]);
                        //BizActionObj.Details.PatientUnitId = (long)DALHelper.HandleDBNull(reader["PatientUnitId"]);
                        //BizActionObj.Details.DepartmentID = (long)DALHelper.HandleDBNull(reader["DepartmentID"]);
                        //BizActionObj.Details.VisitTypeID = (long)DALHelper.HandleDBNull(reader["VisitTypeID"]);
                        //BizActionObj.Details.VisitStatus = (bool)DALHelper.HandleDBNull(reader["VisitStatus"]);                                                                  
                        //BizActionObj.Details.VisitType = (string)DALHelper.HandleDBNull(reader["VisitType"]);                      
                    }
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {

            }
            return valueObject;
        }


        public override IValueObject GetVisitList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetVisitListBizActionVO BizActionObj = valueObject as clsGetVisitListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetVisit");

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CheckPCR", DbType.Boolean, BizActionObj.CheckPCR);

                DbDataReader reader;

                // clsPatientGeneralVO objPatientVO = BizActionObj.PatientDetails;


                //int intStatus = dbServer.ExecuteNonQuery(command);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.VisitList == null)
                        BizActionObj.VisitList = new List<clsVisitVO>();
                    while (reader.Read())
                    {
                        clsVisitVO objVisitVO = new clsVisitVO();

                        objVisitVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVisitVO.PatientId = (long)DALHelper.HandleDBNull(reader["PatientId"]);
                        objVisitVO.PatientUnitId = (long)DALHelper.HandleDBNull(reader["PatientUnitId"]);
                        objVisitVO.DepartmentID = (long)DALHelper.HandleDBNull(reader["DepartmentID"]);
                        objVisitVO.VisitTypeID = (long)DALHelper.HandleDBNull(reader["VisitTypeID"]);
                        objVisitVO.CabinID = (long)DALHelper.HandleDBNull(reader["CabinID"]);
                        objVisitVO.DoctorID = (long)DALHelper.HandleDBNull(reader["DoctorID"]);
                        objVisitVO.ReferredDoctor = (string)DALHelper.HandleDBNull(reader["ReferredDoctor"]);
                        objVisitVO.PatientCaseRecord = (string)DALHelper.HandleDBNull(reader["PatientCaseRecord"]);
                        objVisitVO.CaseReferralSheet = (string)DALHelper.HandleDBNull(reader["CaseReferralSheet"]);
                        objVisitVO.Complaints = (string)DALHelper.HandleDBNull(reader["Complaints"]);
                        objVisitVO.VisitNotes = (string)DALHelper.HandleDBNull(reader["VisitNotes"]);
                        objVisitVO.Date = (DateTime)DALHelper.HandleDBNull(reader["Date"]);
                        objVisitVO.OPDNO = (string)DALHelper.HandleDBNull(reader["OPDNO"]);
                        objVisitVO.VisitStatus = (bool)DALHelper.HandleDBNull(reader["VisitStatus"]);
                        // objVisitVO.TokenNo = (int)DALHelper.HandleDBNull(reader["TokenNo"]);


                        objVisitVO.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                        objVisitVO.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                        objVisitVO.AddedBy = (Int64?)DALHelper.HandleDBNull(reader["AddedBy"]);
                        objVisitVO.AddedOn = (String)DALHelper.HandleDBNull(reader["AddedOn"]);
                        objVisitVO.AddedDateTime = (DateTime?)DALHelper.HandleDate(reader["AddedDateTime"]);
                        objVisitVO.AddedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]);
                        objVisitVO.UpdatedBy = (Int64?)DALHelper.HandleDBNull(reader["UpdatedBy"]);
                        objVisitVO.UpdatedOn = (String)DALHelper.HandleDBNull(reader["UpdatedOn"]);
                        objVisitVO.UpdatedDateTime = (DateTime?)DALHelper.HandleDate(reader["UpdatedDateTime"]);
                        objVisitVO.UpdatedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["UpdatedWindowsLoginName"]);

                        objVisitVO.Department = (string)DALHelper.HandleDBNull(reader["Department"]);
                        objVisitVO.VisitType = (string)DALHelper.HandleDBNull(reader["VisitType"]);
                        objVisitVO.Cabin = (string)DALHelper.HandleDBNull(reader["Cabin"]);
                        objVisitVO.Doctor = (string)DALHelper.HandleDBNull(reader["Doctor"]);

                        BizActionObj.VisitList.Add(objVisitVO);

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
            return valueObject;
        }

        public override IValueObject GetSecondLastVisit(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetSecondLastVisitBizActionVO BizActionObj = valueObject as clsGetSecondLastVisitBizActionVO;
            try
            {
                clsVisitVO objDetailsVO = BizActionObj.Details;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetSecondLastVisit");

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objDetailsVO.PatientId);
                dbServer.AddInParameter(command, "ID", DbType.Int64, objDetailsVO.ID);


                DbDataReader reader;
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizActionObj.Details.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                    }
                }
                else
                {
                    valueObject = null;
                }
                //int intStatus = dbServer.ExecuteNonQuery(command);
                reader.Close();
            }
            catch (Exception ex)
            {

                // logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return valueObject;
        }

        public override IValueObject GetPatientPastVisit(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPastVisitBizActionVO BizAction = valueObject as clsGetPatientPastVisitBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientPastVisit");


                dbServer.AddInParameter(command, "MRNo", DbType.String, BizAction.MRNO);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                dbServer.AddInParameter(command, "PatientUnitId", DbType.Int64, BizAction.PatientUnitID);
                //dbServer.AddInParameter(command, "SpouseID", DbType.Int64, BizAction.SpouseID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    if (BizAction.VisitList == null)
                        BizAction.VisitList = new List<clsVisitVO>();
                    while (reader.Read())
                    {
                        clsVisitVO objVisitVO = new clsVisitVO();

                        //objVisitVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVisitVO.PastVisitDate = (string)DALHelper.HandleDBNull(reader["Date"]);
                        objVisitVO.PastVisitInTime = (string)DALHelper.HandleDBNull(reader["InTime"]);
                        objVisitVO.DoctorID = (long)DALHelper.HandleDBNull(reader["DoctorID"]);
                        objVisitVO.Doctor = (string)DALHelper.HandleDBNull(reader["DoctorName"]);
                        objVisitVO.DepartmentID = (long)DALHelper.HandleDBNull(reader["DepartmentID"]);
                        objVisitVO.Department = (string)DALHelper.HandleDBNull(reader["DepartmentName"]);
                        objVisitVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        objVisitVO.Unit = (string)DALHelper.HandleDBNull(reader["UnitName"]);
                        objVisitVO.VisitType = (string)DALHelper.HandleDBNull(reader["VisitType"]);
                        objVisitVO.BalanceAmount = (double)DALHelper.HandleDBNull(reader["BalanceAmount"]);
                        objVisitVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVisitVO.PatientId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientRegistrationID"]));

                        objVisitVO.VisitTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitTypeID"]));//Added By YK
                        objVisitVO.ConsultationVisitTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConsultationVisitTypeID"]));//Added By YK
                        objVisitVO.IsFree = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFree"]));//Added By YK
                        objVisitVO.FreeDaysDuration = Convert.ToInt64(DALHelper.HandleDBNull(reader["FreeDaysDuration"]));//Added By YK


                        BizAction.VisitList.Add(objVisitVO);
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
            return valueObject;
        }

        public override IValueObject UpdateCurrentVisitStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateCurrentVisitStatusBizActionVO BizActionObj = valueObject as clsUpdateCurrentVisitStatusBizActionVO;

            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateVisitStatus");


                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CurrentVisitStatus", DbType.Int32, (Int32)BizActionObj.CurrentVisitStatus);


                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                int intStatus = dbServer.ExecuteNonQuery(command);
                //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

            }
            catch (Exception ex)
            {
                //   throw;
            }
            finally
            {

            }

            return BizActionObj;
        }

        public override IValueObject GetPatientEMRVisitList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientEMRVisitListBizActionVO BizActionObj = valueObject as clsGetPatientEMRVisitListBizActionVO;
            try
            {
                if (BizActionObj.PatientEMR == 1)
                {
                    #region IVF EMR
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientEMRSummaryList");

                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                    dbServer.AddInParameter(command, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                    DbDataReader reader;
                    reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        if (BizActionObj.VisitList == null)
                            BizActionObj.VisitList = new List<clsVisitVO>();
                        while (reader.Read())
                        {
                            clsVisitVO objVisitVO = new clsVisitVO();

                            //objVisitVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            objVisitVO.PatientId = (long)DALHelper.HandleDBNull(reader["PatientId"]);
                            objVisitVO.VisitID = (long)DALHelper.HandleDBNull(reader["VisitID"]);
                            objVisitVO.DoctorID = (long)DALHelper.HandleDBNull(reader["DoctorID"]);
                            //  objVisitVO.ReferredDoctorID = (long)DALHelper.HandleDBNull(reader["ReferredDoctorID"]);
                            objVisitVO.ReferredDoctor = (string)DALHelper.HandleDBNull(reader["ReferredDoctor"]);
                            objVisitVO.Complaints = (string)DALHelper.HandleDBNull(reader["Complaints"]);
                            objVisitVO.VisitNotes = (string)DALHelper.HandleDBNull(reader["VisitNotes"]);
                            objVisitVO.Date = (DateTime)DALHelper.HandleDBNull(reader["Date"]);
                            objVisitVO.CurrentDate = (DateTime)DALHelper.HandleDBNull(reader["CurrentDate"]);
                            objVisitVO.OPDNO = (string)DALHelper.HandleDBNull(reader["OPDNO"]);
                            objVisitVO.VisitStatus = (bool)DALHelper.HandleDBNull(reader["VisitStatus"]);
                            //  objVisitVO.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                            objVisitVO.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                            objVisitVO.Department = (string)DALHelper.HandleDBNull(reader["Department"]);
                            objVisitVO.VisitType = (string)DALHelper.HandleDBNull(reader["VisitType"]);
                            objVisitVO.Doctor = (string)DALHelper.HandleDBNull(reader["Doctor"]);
                            objVisitVO.Clinic = (string)DALHelper.HandleDBNull(reader["Clinic"]);
                            objVisitVO.TemplateID = (long)DALHelper.HandleDBNull(reader["TemplateID"]);

                            BizActionObj.VisitList.Add(objVisitVO);
                        }

                        reader.Close();
                    }

                    #endregion
                }
                else
                {
                    #region EMR
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientEMRVisitList");

                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

                    DbDataReader reader;

                    // clsPatientGeneralVO objPatientVO = BizActionObj.PatientDetails;


                    //int intStatus = dbServer.ExecuteNonQuery(command);
                    reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.VisitList == null)
                            BizActionObj.VisitList = new List<clsVisitVO>();
                        while (reader.Read())
                        {
                            clsVisitVO objVisitVO = new clsVisitVO();

                            objVisitVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            objVisitVO.PatientId = (long)DALHelper.HandleDBNull(reader["PatientId"]);
                            objVisitVO.PatientUnitId = (long)DALHelper.HandleDBNull(reader["PatientUnitId"]);
                            objVisitVO.DepartmentID = (long)DALHelper.HandleDBNull(reader["DepartmentID"]);
                            objVisitVO.VisitTypeID = (long)DALHelper.HandleDBNull(reader["VisitTypeID"]);
                            objVisitVO.CabinID = (long)DALHelper.HandleDBNull(reader["CabinID"]);
                            objVisitVO.DoctorID = (long)DALHelper.HandleDBNull(reader["DoctorID"]);
                            objVisitVO.ReferredDoctor = (string)DALHelper.HandleDBNull(reader["ReferredDoctor"]);
                            objVisitVO.PatientCaseRecord = (string)DALHelper.HandleDBNull(reader["PatientCaseRecord"]);
                            objVisitVO.CaseReferralSheet = (string)DALHelper.HandleDBNull(reader["CaseReferralSheet"]);
                            objVisitVO.Complaints = (string)DALHelper.HandleDBNull(reader["Complaints"]);
                            objVisitVO.VisitNotes = (string)DALHelper.HandleDBNull(reader["VisitNotes"]);
                            objVisitVO.Date = (DateTime)DALHelper.HandleDBNull(reader["Date"]);
                            objVisitVO.OPDNO = (string)DALHelper.HandleDBNull(reader["OPDNO"]);
                            objVisitVO.VisitStatus = (bool)DALHelper.HandleDBNull(reader["VisitStatus"]);
                            // objVisitVO.TokenNo = (int)DALHelper.HandleDBNull(reader["TokenNo"]);


                            objVisitVO.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                            objVisitVO.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                            objVisitVO.AddedBy = (Int64?)DALHelper.HandleDBNull(reader["AddedBy"]);
                            objVisitVO.AddedOn = (String)DALHelper.HandleDBNull(reader["AddedOn"]);
                            objVisitVO.AddedDateTime = (DateTime?)DALHelper.HandleDate(reader["AddedDateTime"]);
                            objVisitVO.AddedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]);
                            objVisitVO.UpdatedBy = (Int64?)DALHelper.HandleDBNull(reader["UpdatedBy"]);
                            objVisitVO.UpdatedOn = (String)DALHelper.HandleDBNull(reader["UpdatedOn"]);
                            objVisitVO.UpdatedDateTime = (DateTime?)DALHelper.HandleDate(reader["UpdatedDateTime"]);
                            objVisitVO.UpdatedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["UpdatedWindowsLoginName"]);

                            objVisitVO.Department = (string)DALHelper.HandleDBNull(reader["Department"]);
                            objVisitVO.VisitType = (string)DALHelper.HandleDBNull(reader["VisitType"]);
                            objVisitVO.Cabin = (string)DALHelper.HandleDBNull(reader["Cabin"]);
                            objVisitVO.Doctor = (string)DALHelper.HandleDBNull(reader["Doctor"]);
                            objVisitVO.Clinic = (string)DALHelper.HandleDBNull(reader["Clinic"]);
                            objVisitVO.TemplateID = (long)DALHelper.HandleDBNull(reader["TemplateID"]);

                            BizActionObj.VisitList.Add(objVisitVO);

                        }

                        reader.Close();
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return valueObject;
        }

        public override IValueObject GetAllPendingVisitCount(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPendingVisitBizActioVO BizActionObj = valueObject as clsGetPendingVisitBizActioVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPendingVisit");
                dbServer.AddParameter(command, "Count", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.Count);
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.Count = (int)dbServer.GetParameterValue(command, "Count");

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

        public override IValueObject ClosePendingVisit(IValueObject valueObject, clsUserVO UserVo)
        {
            clsClosePendingVisitBizActioVO BizActionObj = valueObject as clsClosePendingVisitBizActioVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_CloseAllPendingVisit");

                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                int intStatus = dbServer.ExecuteNonQuery(command);

            }
            catch (Exception ex)
            {
                //   throw;
            }
            finally
            {

            }

            return BizActionObj;
        }

        public override IValueObject GetCurrentVisit(IValueObject valueObject, clsUserVO UserVo)
        {

            clsGetCurrentVisitBizActionVO BizActionObj = valueObject as clsGetCurrentVisitBizActionVO;
            try
            {
                clsVisitVO objDetailsVO = BizActionObj.Details;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCurrentVisit");

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objDetailsVO.PatientId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objDetailsVO.PatientUnitId);
                dbServer.AddInParameter(command, "ID", DbType.Int64, objDetailsVO.ID);
                if (BizActionObj.ForHO == true)
                {
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDetailsVO.UnitId);
                }
                else
                {
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                }
                dbServer.AddInParameter(command, "GetLatestVisit", DbType.Boolean, BizActionObj.GetLatestVisit);

                DbDataReader reader;
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizActionObj.Details.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        BizActionObj.Details.PatientId = (long)DALHelper.HandleDBNull(reader["PatientId"]);
                        BizActionObj.Details.PatientUnitId = (long)DALHelper.HandleDBNull(reader["PatientUnitId"]);
                        BizActionObj.Details.DepartmentID = (long)DALHelper.HandleDBNull(reader["DepartmentID"]);
                        BizActionObj.Details.VisitTypeID = (long)DALHelper.HandleDBNull(reader["VisitTypeID"]);
                        BizActionObj.Details.CabinID = (long)DALHelper.HandleDBNull(reader["CabinID"]);
                        BizActionObj.Details.DoctorID = (long)DALHelper.HandleDBNull(reader["DoctorID"]);
                        BizActionObj.Details.ReferredDoctor = (string)DALHelper.HandleDBNull(reader["ReferredDoctor"]);
                        BizActionObj.Details.PatientCaseRecord = (string)DALHelper.HandleDBNull(reader["PatientCaseRecord"]);
                        BizActionObj.Details.CaseReferralSheet = (string)DALHelper.HandleDBNull(reader["CaseReferralSheet"]);
                        BizActionObj.Details.Complaints = (string)DALHelper.HandleDBNull(reader["Complaints"]);
                        BizActionObj.Details.VisitNotes = (string)DALHelper.HandleDBNull(reader["VisitNotes"]);
                        BizActionObj.Details.Date = (DateTime)DALHelper.HandleDBNull(reader["Date"]);
                        BizActionObj.Details.OPDNO = (string)DALHelper.HandleDBNull(reader["OPDNO"]);
                        BizActionObj.Details.VisitStatus = (bool)DALHelper.HandleDBNull(reader["VisitStatus"]);
                        BizActionObj.Details.CurrentVisitStatus = (VisitCurrentStatus)DALHelper.HandleDBNull(reader["CurrentVisitStatus"]);

                        BizActionObj.Details.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                        BizActionObj.Details.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);

                        BizActionObj.Details.CreatedUnitId = (Int64)DALHelper.HandleDBNull(reader["CreatedUnitId"]);
                        BizActionObj.Details.AddedBy = (Int64?)DALHelper.HandleDBNull(reader["AddedBy"]);
                        BizActionObj.Details.AddedOn = (String)DALHelper.HandleDBNull(reader["AddedOn"]);
                        BizActionObj.Details.AddedDateTime = (DateTime?)DALHelper.HandleDate(reader["AddedDateTime"]);
                        BizActionObj.Details.AddedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]);

                        BizActionObj.Details.UpdatedUnitId = (Int64?)DALHelper.HandleDBNull(reader["UpdatedUnitID"]);
                        BizActionObj.Details.UpdatedBy = (Int64?)DALHelper.HandleDBNull(reader["UpdatedBy"]);
                        BizActionObj.Details.UpdatedOn = (String)DALHelper.HandleDBNull(reader["UpdatedOn"]);
                        BizActionObj.Details.UpdatedDateTime = (DateTime?)DALHelper.HandleDate(reader["UpdatedDateTime"]);
                        BizActionObj.Details.UpdatedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["UpdatedWindowsLoginName"]);

                        BizActionObj.Details.Department = (string)DALHelper.HandleDBNull(reader["Department"]);
                        BizActionObj.Details.VisitType = (string)DALHelper.HandleDBNull(reader["VisitType"]);
                        BizActionObj.Details.Cabin = (string)DALHelper.HandleDBNull(reader["Cabin"]);
                        BizActionObj.Details.Doctor = (string)DALHelper.HandleDBNull(reader["Doctor"]);

                    }
                }
                reader.Close();

            }
            catch (Exception ex)
            {

                // logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return valueObject;
        }

        public override IValueObject GetEMRVisit(IValueObject valueObject, clsUserVO UserVo)
        {

            clsGetEMRVisitBizActionVO BizActionObj = valueObject as clsGetEMRVisitBizActionVO;
            try
            {
                clsVisitVO objDetailsVO = BizActionObj.Details;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetEMRVisit");

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objDetailsVO.PatientId);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objDetailsVO.PatientUnitId);
                dbServer.AddInParameter(command, "ID", DbType.Int64, objDetailsVO.ID);

                dbServer.AddInParameter(command, "GetLatestVisit", DbType.Boolean, BizActionObj.GetLatestVisit);

                DbDataReader reader;
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizActionObj.Details.ID = Convert.ToInt64(reader["ID"]);
                        BizActionObj.Details.UnitId = Convert.ToInt64(reader["UnitID"]);
                        BizActionObj.Details.PatientId = Convert.ToInt64(reader["PatientId"]);
                        BizActionObj.Details.PatientUnitId = Convert.ToInt64(reader["PatientUnitId"]);
                        BizActionObj.Details.DepartmentCode = Convert.ToString(reader["DepartmentID"]);
                        BizActionObj.Details.DoctorID = Convert.ToInt64(reader["DoctorID"]);
                        BizActionObj.Details.DoctorCode = Convert.ToString(reader["DoctorID"]);
                        BizActionObj.Details.VisitTypeID = Convert.ToInt64(reader["VisitTypeID"]);
                        BizActionObj.Details.ReferredDoctor = Convert.ToString(reader["ReferredDoctor"]);
                        BizActionObj.Details.Complaints = Convert.ToString(reader["Complaints"]);
                        BizActionObj.Details.VisitNotes = Convert.ToString(reader["VisitNotes"]);
                        BizActionObj.Details.Date = Convert.ToDateTime(reader["Date"]);
                        BizActionObj.Details.OPDNO = Convert.ToString(reader["OPDNO"]);
                        // BizActionObj.Details.VisitStatus = (bool)DALHelper.HandleDBNull(reader["VisitStatus"]);
                        BizActionObj.Details.VisitStatus = false;
                        BizActionObj.Details.VisitType = Convert.ToString(reader["VisitType"]);
                        BizActionObj.Details.Allergies = Convert.ToString(reader["Allergy"]);
                        BizActionObj.Details.Doctor = Convert.ToString(reader["Doctor"]);
                        BizActionObj.Details.DoctorSpecialization = Convert.ToString(reader["SpecializationName"]);
                        //BizActionObj.Details.IsReferral = Convert.ToBoolean(reader["Referral"]);
                        BizActionObj.Details.CoupleMRNO = Convert.ToString(reader["MRNO"]);
                        BizActionObj.Details.DonorMRNO = Convert.ToString(reader["DonorMRNO"]);
                        BizActionObj.Details.SurrogateMRNO = Convert.ToString(reader["SurrogateMRNO"]);
                    }
                }
                reader.Close();

            }
            catch (Exception ex)
            {

                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
            }
            return valueObject;
        }

        public override IValueObject GetEMRdignosisFillorNot(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetEMRVisitDignosisiValidationVo BizActionObj = valueObject as clsGetEMRVisitDignosisiValidationVo;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetEMRdignosisFillorNot");
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                BizActionObj.SuccessStatus = Convert.ToInt32(dbServer.ExecuteScalar(command));
            }
            catch (Exception)
            {
            }
            return valueObject;
        }

        public override IValueObject AddVisitForPathology(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            clsAddVisitBizActionVO BizActionObj = valueObject as clsAddVisitBizActionVO;
            try
            {
                if (pConnection == null)
                    pConnection = dbServer.CreateConnection();

                if (pConnection.State != ConnectionState.Open) pConnection.Open();
                if (pTransaction == null)
                    pTransaction = pConnection.BeginTransaction();

                clsVisitVO objDetailsVO = BizActionObj.VisitDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddVisitForPathology");
                dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                if (objDetailsVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));
                dbServer.AddInParameter(command, "LinkServerDBName", DbType.String, null);

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objDetailsVO.PatientId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objDetailsVO.PatientUnitId);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);
                dbServer.AddInParameter(command, "VisitTypeID", DbType.Int64, objDetailsVO.VisitTypeID);
                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
                dbServer.AddParameter(command, "UnitId", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, UserVo.UserLoginInfo.UnitId);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.VisitDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.VisitDetails.UnitId = (long)dbServer.GetParameterValue(command, "UnitId");
            }

            catch (Exception ex)
            {
                throw;

            }
            return BizActionObj;
        }




    }
}
