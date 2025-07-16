namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.CustomExceptions;
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.IPD;
    using PalashDynamics.ValueObjects.OutPatientDepartment;
    using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
    using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsVisitDAL : clsBaseVisitDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsVisitDAL()
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
                throw;
            }
        }

        private bool AddPatientData(long pPatientID, long pUnitID)
        {
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatientFromOtherClinic");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, pPatientID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, pUnitID);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, -2147483648);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                return (((int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus")) != -1);
            }
            catch (Exception exception1)
            {
                string message = exception1.Message;
                return false;
            }
        }

        public override IValueObject AddVisit(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddVisitBizActionVO bizActionObj = valueObject as clsAddVisitBizActionVO;
            bizActionObj = (bizActionObj.VisitDetails.ID != 0L) ? this.UpdateVisitDetails(bizActionObj, UserVo) : this.AddVisitDetails(bizActionObj, UserVo, null, null);
            return valueObject;
        }

        public override IValueObject AddVisit(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {
            clsAddVisitBizActionVO bizActionObj = valueObject as clsAddVisitBizActionVO;
            if (bizActionObj.VisitDetails.ID == 0L)
            {
                bizActionObj = this.AddVisitDetails(bizActionObj, UserVo, myConnection, myTransaction);
            }
            return valueObject;
        }

        private clsAddVisitBizActionVO AddVisitDetails(clsAddVisitBizActionVO BizActionObj, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                clsVisitVO visitDetails = BizActionObj.VisitDetails;
                connection = (myConnection == null) ? this.dbServer.CreateConnection() : myConnection;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = (myTransaction == null) ? connection.BeginTransaction() : myTransaction;
                bool flag = true;
                if (visitDetails.PatientUnitId != UserVo.UserLoginInfo.UnitId)
                {
                    flag = this.AddPatientData(visitDetails.PatientId, visitDetails.PatientUnitId);
                }
                if (flag)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddVisit");
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, visitDetails.LinkServer);
                    if (visitDetails.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, visitDetails.LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerDBName", DbType.String, null);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, visitDetails.PatientId);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, visitDetails.PatientUnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, visitDetails.Date);
                    if (visitDetails.OPDNO != null)
                    {
                        visitDetails.OPDNO = visitDetails.OPDNO.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "OPDNO", DbType.String, visitDetails.OPDNO);
                    this.dbServer.AddInParameter(storedProcCommand, "VisitTypeID", DbType.Int64, visitDetails.VisitTypeID);
                    if (visitDetails.Complaints != null)
                    {
                        visitDetails.Complaints = visitDetails.Complaints.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "Complaints", DbType.String, visitDetails.Complaints);
                    this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, visitDetails.DepartmentID);
                    this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, visitDetails.DoctorID);
                    this.dbServer.AddInParameter(storedProcCommand, "CabinID", DbType.Int64, visitDetails.CabinID);
                    if (visitDetails.ReferredDoctorID != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "ReferredDoctorID", DbType.Int64, visitDetails.ReferredDoctorID);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "ReferredDoctor", DbType.String, visitDetails.ReferredDoctor);
                    if (visitDetails.VisitNotes != null)
                    {
                        visitDetails.VisitNotes = visitDetails.VisitNotes.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "VisitNotes", DbType.String, visitDetails.VisitNotes);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, visitDetails.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "VisitStatus", DbType.Boolean, visitDetails.VisitStatus);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, visitDetails.AddedDateTime);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, visitDetails.ID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    BizActionObj.VisitDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                    if (BizActionObj.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                }
                if (myConnection == null)
                {
                    transaction.Commit();
                }
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                if (myConnection == null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (myConnection == null)
                {
                    connection.Close();
                }
            }
            return BizActionObj;
        }

        public override IValueObject AddVisitForPathology(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            clsAddVisitBizActionVO nvo = valueObject as clsAddVisitBizActionVO;
            try
            {
                if (pConnection == null)
                {
                    pConnection = this.dbServer.CreateConnection();
                }
                if (pConnection.State != ConnectionState.Open)
                {
                    pConnection.Open();
                }
                if (pTransaction == null)
                {
                    pTransaction = pConnection.BeginTransaction();
                }
                clsVisitVO visitDetails = nvo.VisitDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddVisitForPathology");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, visitDetails.LinkServer);
                if (visitDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, visitDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "LinkServerDBName", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, visitDetails.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, visitDetails.PatientUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, visitDetails.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, visitDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "VisitTypeID", DbType.Int64, visitDetails.VisitTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, visitDetails.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, visitDetails.ID);
                this.dbServer.AddParameter(storedProcCommand, "UnitId", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.VisitDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.VisitDetails.UnitId = (long) this.dbServer.GetParameterValue(storedProcCommand, "UnitId");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject ClosePendingVisit(IValueObject valueObject, clsUserVO UserVo)
        {
            clsClosePendingVisitBizActioVO ovo = valueObject as clsClosePendingVisitBizActioVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CloseAllPendingVisit");
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
            }
            return ovo;
        }

        public override IValueObject GetAllPendingVisitCount(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPendingVisitBizActioVO ovo = valueObject as clsGetPendingVisitBizActioVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPendingVisit");
                this.dbServer.AddParameter(storedProcCommand, "Count", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, ovo.Count);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                ovo.Count = (int) this.dbServer.GetParameterValue(storedProcCommand, "Count");
            }
            catch (Exception)
            {
                throw;
            }
            return ovo;
        }

        public override IValueObject GetCurrentVisit(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCurrentVisitBizActionVO nvo = valueObject as clsGetCurrentVisitBizActionVO;
            try
            {
                clsVisitVO details = nvo.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCurrentVisit");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, details.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, details.PatientUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, details.ID);
                if (nvo.ForHO)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, details.UnitId);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                }
                this.dbServer.AddInParameter(storedProcCommand, "GetLatestVisit", DbType.Boolean, nvo.GetLatestVisit);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.Details.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.Details.PatientId = (long) DALHelper.HandleDBNull(reader["PatientId"]);
                        nvo.Details.PatientUnitId = (long) DALHelper.HandleDBNull(reader["PatientUnitId"]);
                        nvo.Details.DepartmentID = (long) DALHelper.HandleDBNull(reader["DepartmentID"]);
                        nvo.Details.VisitTypeID = (long) DALHelper.HandleDBNull(reader["VisitTypeID"]);
                        nvo.Details.CabinID = (long) DALHelper.HandleDBNull(reader["CabinID"]);
                        nvo.Details.DoctorID = (long) DALHelper.HandleDBNull(reader["DoctorID"]);
                        nvo.Details.ReferredDoctor = (string) DALHelper.HandleDBNull(reader["ReferredDoctor"]);
                        nvo.Details.PatientCaseRecord = (string) DALHelper.HandleDBNull(reader["PatientCaseRecord"]);
                        nvo.Details.CaseReferralSheet = (string) DALHelper.HandleDBNull(reader["CaseReferralSheet"]);
                        nvo.Details.Complaints = (string) DALHelper.HandleDBNull(reader["Complaints"]);
                        nvo.Details.VisitNotes = (string) DALHelper.HandleDBNull(reader["VisitNotes"]);
                        nvo.Details.Date = (DateTime) DALHelper.HandleDBNull(reader["Date"]);
                        nvo.Details.OPDNO = (string) DALHelper.HandleDBNull(reader["OPDNO"]);
                        nvo.Details.VisitStatus = (bool) DALHelper.HandleDBNull(reader["VisitStatus"]);
                        nvo.Details.CurrentVisitStatus = (VisitCurrentStatus) DALHelper.HandleDBNull(reader["CurrentVisitStatus"]);
                        nvo.Details.UnitId = (long) DALHelper.HandleDBNull(reader["UnitId"]);
                        nvo.Details.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                        nvo.Details.CreatedUnitId = (long) DALHelper.HandleDBNull(reader["CreatedUnitId"]);
                        nvo.Details.AddedBy = (long?) DALHelper.HandleDBNull(reader["AddedBy"]);
                        nvo.Details.AddedOn = (string) DALHelper.HandleDBNull(reader["AddedOn"]);
                        nvo.Details.AddedDateTime = DALHelper.HandleDate(reader["AddedDateTime"]);
                        nvo.Details.AddedWindowsLoginName = (string) DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]);
                        nvo.Details.UpdatedUnitId = (long?) DALHelper.HandleDBNull(reader["UpdatedUnitID"]);
                        nvo.Details.UpdatedBy = (long?) DALHelper.HandleDBNull(reader["UpdatedBy"]);
                        nvo.Details.UpdatedOn = (string) DALHelper.HandleDBNull(reader["UpdatedOn"]);
                        nvo.Details.UpdatedDateTime = DALHelper.HandleDate(reader["UpdatedDateTime"]);
                        nvo.Details.UpdatedWindowsLoginName = (string) DALHelper.HandleDBNull(reader["UpdatedWindowsLoginName"]);
                        nvo.Details.Department = (string) DALHelper.HandleDBNull(reader["Department"]);
                        nvo.Details.VisitType = (string) DALHelper.HandleDBNull(reader["VisitType"]);
                        nvo.Details.Cabin = (string) DALHelper.HandleDBNull(reader["Cabin"]);
                        nvo.Details.Doctor = (string) DALHelper.HandleDBNull(reader["Doctor"]);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject GetEMRdignosisFillorNot(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetEMRVisitDignosisiValidationVo vo = valueObject as clsGetEMRVisitDignosisiValidationVo;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetEMRdignosisFillorNot");
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, vo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, vo.PatientID);
                vo.SuccessStatus = Convert.ToInt32(this.dbServer.ExecuteScalar(storedProcCommand));
            }
            catch (Exception)
            {
            }
            return valueObject;
        }

        public override IValueObject GetEMRVisit(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetEMRVisitBizActionVO nvo = valueObject as clsGetEMRVisitBizActionVO;
            try
            {
                clsVisitVO details = nvo.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetEMRVisit");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, details.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, details.ID);
                this.dbServer.AddInParameter(storedProcCommand, "GetLatestVisit", DbType.Boolean, nvo.GetLatestVisit);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.Details.ID = Convert.ToInt64(reader["ID"]);
                        nvo.Details.UnitId = Convert.ToInt64(reader["UnitID"]);
                        nvo.Details.PatientId = Convert.ToInt64(reader["PatientId"]);
                        nvo.Details.PatientUnitId = Convert.ToInt64(reader["PatientUnitId"]);
                        nvo.Details.DepartmentCode = Convert.ToString(reader["DepartmentID"]);
                        nvo.Details.DoctorID = Convert.ToInt64(reader["DoctorID"]);
                        nvo.Details.DoctorCode = Convert.ToString(reader["DoctorID"]);
                        nvo.Details.VisitTypeID = Convert.ToInt64(reader["VisitTypeID"]);
                        nvo.Details.ReferredDoctor = Convert.ToString(reader["ReferredDoctor"]);
                        nvo.Details.Complaints = Convert.ToString(reader["Complaints"]);
                        nvo.Details.VisitNotes = Convert.ToString(reader["VisitNotes"]);
                        nvo.Details.Date = Convert.ToDateTime(reader["Date"]);
                        nvo.Details.OPDNO = Convert.ToString(reader["OPDNO"]);
                        nvo.Details.VisitStatus = false;
                        nvo.Details.VisitType = Convert.ToString(reader["VisitType"]);
                        nvo.Details.Allergies = Convert.ToString(reader["Allergy"]);
                        nvo.Details.Doctor = Convert.ToString(reader["Doctor"]);
                        nvo.Details.DoctorSpecialization = Convert.ToString(reader["SpecializationName"]);
                        nvo.Details.CoupleMRNO = Convert.ToString(reader["MRNO"]);
                        nvo.Details.DonorMRNO = Convert.ToString(reader["DonorMRNO"]);
                        nvo.Details.SurrogateMRNO = Convert.ToString(reader["SurrogateMRNO"]);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject GetPatientEMRVisitList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientEMRVisitListBizActionVO nvo = valueObject as clsGetPatientEMRVisitListBizActionVO;
            try
            {
                if (nvo.PatientEMR == 1L)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientEMRSummaryList");
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.VisitList == null)
                        {
                            nvo.VisitList = new List<clsVisitVO>();
                        }
                        while (true)
                        {
                            if (!reader.Read())
                            {
                                reader.Close();
                                break;
                            }
                            clsVisitVO item = new clsVisitVO {
                                PatientId = (long) DALHelper.HandleDBNull(reader["PatientId"]),
                                VisitID = (long) DALHelper.HandleDBNull(reader["VisitID"]),
                                DoctorID = (long) DALHelper.HandleDBNull(reader["DoctorID"]),
                                ReferredDoctor = (string) DALHelper.HandleDBNull(reader["ReferredDoctor"]),
                                Complaints = (string) DALHelper.HandleDBNull(reader["Complaints"]),
                                VisitNotes = (string) DALHelper.HandleDBNull(reader["VisitNotes"]),
                                Date = (DateTime) DALHelper.HandleDBNull(reader["Date"]),
                                CurrentDate = (DateTime) DALHelper.HandleDBNull(reader["CurrentDate"]),
                                OPDNO = (string) DALHelper.HandleDBNull(reader["OPDNO"]),
                                VisitStatus = (bool) DALHelper.HandleDBNull(reader["VisitStatus"]),
                                Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                                Department = (string) DALHelper.HandleDBNull(reader["Department"]),
                                VisitType = (string) DALHelper.HandleDBNull(reader["VisitType"]),
                                Doctor = (string) DALHelper.HandleDBNull(reader["Doctor"]),
                                Clinic = (string) DALHelper.HandleDBNull(reader["Clinic"]),
                                TemplateID = (long) DALHelper.HandleDBNull(reader["TemplateID"])
                            };
                            nvo.VisitList.Add(item);
                        }
                    }
                }
                else
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientEMRVisitList");
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader2.HasRows)
                    {
                        if (nvo.VisitList == null)
                        {
                            nvo.VisitList = new List<clsVisitVO>();
                        }
                        while (true)
                        {
                            if (!reader2.Read())
                            {
                                reader2.Close();
                                break;
                            }
                            clsVisitVO item = new clsVisitVO {
                                ID = (long) DALHelper.HandleDBNull(reader2["ID"]),
                                PatientId = (long) DALHelper.HandleDBNull(reader2["PatientId"]),
                                PatientUnitId = (long) DALHelper.HandleDBNull(reader2["PatientUnitId"]),
                                DepartmentID = (long) DALHelper.HandleDBNull(reader2["DepartmentID"]),
                                VisitTypeID = (long) DALHelper.HandleDBNull(reader2["VisitTypeID"]),
                                CabinID = (long) DALHelper.HandleDBNull(reader2["CabinID"]),
                                DoctorID = (long) DALHelper.HandleDBNull(reader2["DoctorID"]),
                                ReferredDoctor = (string) DALHelper.HandleDBNull(reader2["ReferredDoctor"]),
                                PatientCaseRecord = (string) DALHelper.HandleDBNull(reader2["PatientCaseRecord"]),
                                CaseReferralSheet = (string) DALHelper.HandleDBNull(reader2["CaseReferralSheet"]),
                                Complaints = (string) DALHelper.HandleDBNull(reader2["Complaints"]),
                                VisitNotes = (string) DALHelper.HandleDBNull(reader2["VisitNotes"]),
                                Date = (DateTime) DALHelper.HandleDBNull(reader2["Date"]),
                                OPDNO = (string) DALHelper.HandleDBNull(reader2["OPDNO"]),
                                VisitStatus = (bool) DALHelper.HandleDBNull(reader2["VisitStatus"]),
                                UnitId = (long) DALHelper.HandleDBNull(reader2["UnitId"]),
                                Status = (bool) DALHelper.HandleDBNull(reader2["Status"]),
                                AddedBy = (long?) DALHelper.HandleDBNull(reader2["AddedBy"]),
                                AddedOn = (string) DALHelper.HandleDBNull(reader2["AddedOn"]),
                                AddedDateTime = DALHelper.HandleDate(reader2["AddedDateTime"]),
                                AddedWindowsLoginName = (string) DALHelper.HandleDBNull(reader2["AddedWindowsLoginName"]),
                                UpdatedBy = (long?) DALHelper.HandleDBNull(reader2["UpdatedBy"]),
                                UpdatedOn = (string) DALHelper.HandleDBNull(reader2["UpdatedOn"]),
                                UpdatedDateTime = DALHelper.HandleDate(reader2["UpdatedDateTime"]),
                                UpdatedWindowsLoginName = (string) DALHelper.HandleDBNull(reader2["UpdatedWindowsLoginName"]),
                                Department = (string) DALHelper.HandleDBNull(reader2["Department"]),
                                VisitType = (string) DALHelper.HandleDBNull(reader2["VisitType"]),
                                Cabin = (string) DALHelper.HandleDBNull(reader2["Cabin"]),
                                Doctor = (string) DALHelper.HandleDBNull(reader2["Doctor"]),
                                Clinic = (string) DALHelper.HandleDBNull(reader2["Clinic"]),
                                TemplateID = (long) DALHelper.HandleDBNull(reader2["TemplateID"])
                            };
                            nvo.VisitList.Add(item);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetPatientPastVisit(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPastVisitBizActionVO nvo = valueObject as clsGetPatientPastVisitBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientPastVisit");
                this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, nvo.MRNO);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitId", DbType.Int64, nvo.PatientUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.VisitList == null)
                    {
                        nvo.VisitList = new List<clsVisitVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        clsVisitVO item = new clsVisitVO {
                            PastVisitDate = (string) DALHelper.HandleDBNull(reader["Date"]),
                            PastVisitInTime = (string) DALHelper.HandleDBNull(reader["InTime"]),
                            DoctorID = (long) DALHelper.HandleDBNull(reader["DoctorID"]),
                            Doctor = (string) DALHelper.HandleDBNull(reader["DoctorName"]),
                            DepartmentID = (long) DALHelper.HandleDBNull(reader["DepartmentID"]),
                            Department = (string) DALHelper.HandleDBNull(reader["DepartmentName"]),
                            UnitId = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            Unit = (string) DALHelper.HandleDBNull(reader["UnitName"]),
                            VisitType = (string) DALHelper.HandleDBNull(reader["VisitType"]),
                            BalanceAmount = new double?((double) DALHelper.HandleDBNull(reader["BalanceAmount"])),
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            PatientId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientRegistrationID"])),
                            VisitTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitTypeID"])),
                            ConsultationVisitTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConsultationVisitTypeID"])),
                            IsFree = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFree"])),
                            FreeDaysDuration = Convert.ToInt64(DALHelper.HandleDBNull(reader["FreeDaysDuration"]))
                        };
                        nvo.VisitList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetSecondLastVisit(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetSecondLastVisitBizActionVO nvo = valueObject as clsGetSecondLastVisitBizActionVO;
            try
            {
                clsVisitVO details = nvo.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSecondLastVisit");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, details.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, details.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (!reader.HasRows)
                {
                    valueObject = null;
                }
                else
                {
                    while (reader.Read())
                    {
                        nvo.Details.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject GetVisit(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetVisitBizActionVO nvo = valueObject as clsGetVisitBizActionVO;
            try
            {
                if (nvo.OPD_IPD_External == 1L)
                {
                    clsGetIPDAdmissionBizActionVO objAdmission = nvo.ObjAdmission;
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAdmission");
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, objAdmission.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, objAdmission.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, objAdmission.ID);
                    if (nvo.ForHO)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objAdmission.UnitID);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "GetLatestAdmission", DbType.Boolean, nvo.GetLatestVisit);
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            nvo.ObjAdmission.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            nvo.ObjAdmission.Details.Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Date"])));
                            nvo.ObjAdmission.Details.PatientId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientId"]));
                            nvo.ObjAdmission.Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                            nvo.ObjAdmission.Details.AdmissionTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionTypeID"]));
                            nvo.ObjAdmission.Details.DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"]));
                            nvo.ObjAdmission.Details.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                            nvo.ObjAdmission.Details.BedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedID"]));
                            nvo.ObjAdmission.Details.KinRelationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["KinRelationID"]));
                            nvo.ObjAdmission.Details.KinAddress = Convert.ToString(DALHelper.HandleDBNull(reader["KinAddress"]));
                            nvo.ObjAdmission.Details.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]));
                            nvo.ObjAdmission.Details.CreatedUnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CreatedUnitId"]));
                            nvo.ObjAdmission.Details.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                            nvo.ObjAdmission.Details.AddedBy = (long?) DALHelper.HandleDBNull(reader["AddedBy"]);
                            nvo.ObjAdmission.Details.AddedOn = Convert.ToString(DALHelper.HandleDBNull(reader["AddedOn"]));
                            nvo.ObjAdmission.Details.AddedDateTime = DALHelper.HandleDate(reader["AddedDateTime"]);
                            nvo.ObjAdmission.Details.AddedWindowsLoginName = Convert.ToString(DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]));
                            nvo.ObjAdmission.Details.UpdatedUnitId = (long?) DALHelper.HandleDBNull(reader["UpdatedUnitID"]);
                            nvo.ObjAdmission.Details.UpdatedBy = (long?) DALHelper.HandleDBNull(reader["UpdatedBy"]);
                            nvo.ObjAdmission.Details.UpdatedOn = Convert.ToString(DALHelper.HandleDBNull(reader["UpdatedOn"]));
                            nvo.ObjAdmission.Details.UpdatedDateTime = DALHelper.HandleDate(reader["UpdatedDateTime"]);
                            nvo.Details.IsBillGeneratedAgainstVisit = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBillGeneratedAgainstVisit"]));
                        }
                    }
                    reader.Close();
                }
                else
                {
                    clsVisitVO details = nvo.Details;
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetVisit");
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, details.PatientId);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, details.PatientUnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, details.ID);
                    if (nvo.ForHO)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, details.UnitId);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "GetLatestVisit", DbType.Boolean, nvo.GetLatestVisit);
                    DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader2.HasRows)
                    {
                        while (reader2.Read())
                        {
                            nvo.Details.ID = (long) DALHelper.HandleDBNull(reader2["ID"]);
                            nvo.Details.PatientId = (long) DALHelper.HandleDBNull(reader2["PatientId"]);
                            nvo.Details.PatientUnitId = (long) DALHelper.HandleDBNull(reader2["PatientUnitId"]);
                            nvo.Details.DepartmentID = (long) DALHelper.HandleDBNull(reader2["DepartmentID"]);
                            nvo.Details.VisitTypeID = (long) DALHelper.HandleDBNull(reader2["VisitTypeID"]);
                            nvo.Details.CabinID = (long) DALHelper.HandleDBNull(reader2["CabinID"]);
                            nvo.Details.DoctorID = (long) DALHelper.HandleDBNull(reader2["DoctorID"]);
                            nvo.Details.ReferredDoctor = (string) DALHelper.HandleDBNull(reader2["ReferredDoctor"]);
                            nvo.Details.PatientCaseRecord = (string) DALHelper.HandleDBNull(reader2["PatientCaseRecord"]);
                            nvo.Details.CaseReferralSheet = (string) DALHelper.HandleDBNull(reader2["CaseReferralSheet"]);
                            nvo.Details.Complaints = (string) DALHelper.HandleDBNull(reader2["Complaints"]);
                            nvo.Details.VisitNotes = (string) DALHelper.HandleDBNull(reader2["VisitNotes"]);
                            nvo.Details.Date = (DateTime) DALHelper.HandleDBNull(reader2["Date"]);
                            nvo.Details.OPDNO = (string) DALHelper.HandleDBNull(reader2["OPDNO"]);
                            nvo.Details.VisitStatus = (bool) DALHelper.HandleDBNull(reader2["VisitStatus"]);
                            nvo.Details.CurrentVisitStatus = (VisitCurrentStatus) DALHelper.HandleDBNull(reader2["CurrentVisitStatus"]);
                            nvo.Details.UnitId = (long) DALHelper.HandleDBNull(reader2["UnitId"]);
                            nvo.Details.Status = (bool) DALHelper.HandleDBNull(reader2["Status"]);
                            nvo.Details.CreatedUnitId = (long) DALHelper.HandleDBNull(reader2["CreatedUnitId"]);
                            nvo.Details.AddedBy = (long?) DALHelper.HandleDBNull(reader2["AddedBy"]);
                            nvo.Details.AddedOn = (string) DALHelper.HandleDBNull(reader2["AddedOn"]);
                            nvo.Details.AddedDateTime = DALHelper.HandleDate(reader2["AddedDateTime"]);
                            nvo.Details.AddedWindowsLoginName = (string) DALHelper.HandleDBNull(reader2["AddedWindowsLoginName"]);
                            nvo.Details.UpdatedUnitId = (long?) DALHelper.HandleDBNull(reader2["UpdatedUnitID"]);
                            nvo.Details.UpdatedBy = (long?) DALHelper.HandleDBNull(reader2["UpdatedBy"]);
                            nvo.Details.UpdatedOn = (string) DALHelper.HandleDBNull(reader2["UpdatedOn"]);
                            nvo.Details.UpdatedDateTime = DALHelper.HandleDate(reader2["UpdatedDateTime"]);
                            nvo.Details.UpdatedWindowsLoginName = (string) DALHelper.HandleDBNull(reader2["UpdatedWindowsLoginName"]);
                            nvo.Details.Department = (string) DALHelper.HandleDBNull(reader2["Department"]);
                            nvo.Details.VisitType = (string) DALHelper.HandleDBNull(reader2["VisitType"]);
                            nvo.Details.Cabin = (string) DALHelper.HandleDBNull(reader2["Cabin"]);
                            nvo.Details.Doctor = (string) DALHelper.HandleDBNull(reader2["Doctor"]);
                            nvo.Details.IsBillGeneratedAgainstVisit = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsBillGeneratedAgainstVisit"]));
                        }
                    }
                    reader2.Close();
                }
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject GetVisitCount(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetVisitBizActionVO nvo = valueObject as clsGetVisitBizActionVO;
            try
            {
                clsVisitVO details = nvo.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetVisitCount");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, details.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, details.PatientUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, details.ID);
                if (nvo.ForHO)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, details.UnitId);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                }
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.Details.VisitCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitCount"]));
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject GetVisitList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetVisitListBizActionVO nvo = valueObject as clsGetVisitListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetVisit");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CheckPCR", DbType.Boolean, nvo.CheckPCR);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.VisitList == null)
                    {
                        nvo.VisitList = new List<clsVisitVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        clsVisitVO item = new clsVisitVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            PatientId = (long) DALHelper.HandleDBNull(reader["PatientId"]),
                            PatientUnitId = (long) DALHelper.HandleDBNull(reader["PatientUnitId"]),
                            DepartmentID = (long) DALHelper.HandleDBNull(reader["DepartmentID"]),
                            VisitTypeID = (long) DALHelper.HandleDBNull(reader["VisitTypeID"]),
                            CabinID = (long) DALHelper.HandleDBNull(reader["CabinID"]),
                            DoctorID = (long) DALHelper.HandleDBNull(reader["DoctorID"]),
                            ReferredDoctor = (string) DALHelper.HandleDBNull(reader["ReferredDoctor"]),
                            PatientCaseRecord = (string) DALHelper.HandleDBNull(reader["PatientCaseRecord"]),
                            CaseReferralSheet = (string) DALHelper.HandleDBNull(reader["CaseReferralSheet"]),
                            Complaints = (string) DALHelper.HandleDBNull(reader["Complaints"]),
                            VisitNotes = (string) DALHelper.HandleDBNull(reader["VisitNotes"]),
                            Date = (DateTime) DALHelper.HandleDBNull(reader["Date"]),
                            OPDNO = (string) DALHelper.HandleDBNull(reader["OPDNO"]),
                            VisitStatus = (bool) DALHelper.HandleDBNull(reader["VisitStatus"]),
                            UnitId = (long) DALHelper.HandleDBNull(reader["UnitId"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            AddedBy = (long?) DALHelper.HandleDBNull(reader["AddedBy"]),
                            AddedOn = (string) DALHelper.HandleDBNull(reader["AddedOn"]),
                            AddedDateTime = DALHelper.HandleDate(reader["AddedDateTime"]),
                            AddedWindowsLoginName = (string) DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]),
                            UpdatedBy = (long?) DALHelper.HandleDBNull(reader["UpdatedBy"]),
                            UpdatedOn = (string) DALHelper.HandleDBNull(reader["UpdatedOn"]),
                            UpdatedDateTime = DALHelper.HandleDate(reader["UpdatedDateTime"]),
                            UpdatedWindowsLoginName = (string) DALHelper.HandleDBNull(reader["UpdatedWindowsLoginName"]),
                            Department = (string) DALHelper.HandleDBNull(reader["Department"]),
                            VisitType = (string) DALHelper.HandleDBNull(reader["VisitType"]),
                            Cabin = (string) DALHelper.HandleDBNull(reader["Cabin"]),
                            Doctor = (string) DALHelper.HandleDBNull(reader["Doctor"])
                        };
                        nvo.VisitList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject UpdateCurrentVisitStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateCurrentVisitStatusBizActionVO nvo = valueObject as clsUpdateCurrentVisitStatusBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateVisitStatus");
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CurrentVisitStatus", DbType.Int32, (int) nvo.CurrentVisitStatus);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        private clsAddVisitBizActionVO UpdateVisitDetails(clsAddVisitBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsVisitVO visitDetails = BizActionObj.VisitDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateVisit");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, visitDetails.LinkServer);
                if (visitDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, visitDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, visitDetails.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, visitDetails.PatientUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, visitDetails.Date);
                if (visitDetails.OPDNO != null)
                {
                    visitDetails.OPDNO = visitDetails.OPDNO.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "OPDNO", DbType.String, visitDetails.OPDNO);
                this.dbServer.AddInParameter(storedProcCommand, "VisitTypeID", DbType.Int64, visitDetails.VisitTypeID);
                if (visitDetails.Complaints != null)
                {
                    visitDetails.Complaints = visitDetails.Complaints.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Complaints", DbType.String, visitDetails.Complaints);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, visitDetails.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, visitDetails.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "CabinID", DbType.Int64, visitDetails.CabinID);
                this.dbServer.AddInParameter(storedProcCommand, "ReferredDoctorID", DbType.Int64, visitDetails.ReferredDoctorID);
                if (visitDetails.ReferredDoctor != null)
                {
                    visitDetails.ReferredDoctor = visitDetails.ReferredDoctor.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ReferredDoctor", DbType.String, visitDetails.ReferredDoctor);
                this.dbServer.AddInParameter(storedProcCommand, "PatientCaseRecord", DbType.String, visitDetails.PatientCaseRecord);
                this.dbServer.AddInParameter(storedProcCommand, "CaseReferralSheet", DbType.String, visitDetails.CaseReferralSheet);
                if (visitDetails.VisitNotes != null)
                {
                    visitDetails.VisitNotes = visitDetails.VisitNotes.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "VisitNotes", DbType.String, visitDetails.VisitNotes);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, visitDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, visitDetails.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, visitDetails.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, visitDetails.ID);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }
    }
}

