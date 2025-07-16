namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.CustomExceptions;
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsAppointmentDAL : clsBaseAppointmentDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsAppointmentDAL()
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

        public override IValueObject AddAppointment(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddAppointmentBizActionVO bizActionobj = valueObject as clsAddAppointmentBizActionVO;
            return ((bizActionobj.AppointmentDetails.AppointmentID != 0L) ? this.UpdateAppointment(bizActionobj, UserVo) : this.AddAppointment(bizActionobj, UserVo));
        }

        private clsAddAppointmentBizActionVO AddAppointment(clsAddAppointmentBizActionVO BizActionobj, clsUserVO UserVo)
        {
            try
            {
                clsAppointmentVO appointmentDetails = BizActionobj.AppointmentDetails;
                bool flag = true;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_SearchAppointment");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, appointmentDetails.LinkServer);
                if (appointmentDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, appointmentDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, appointmentDetails.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, appointmentDetails.PatientUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AppointmentDate", DbType.DateTime, appointmentDetails.AppointmentDate);
                this.dbServer.AddInParameter(storedProcCommand, "FromTime", DbType.DateTime, appointmentDetails.FromTime);
                this.dbServer.AddInParameter(storedProcCommand, "ToTime", DbType.DateTime, appointmentDetails.ToTime);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Boolean, ParameterDirection.InputOutput, null, DataRowVersion.Default, appointmentDetails.ResultStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionobj.AppointmentDetails.ResultStatus = (bool) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if (appointmentDetails.PatientUnitId != UserVo.UserLoginInfo.UnitId)
                {
                    flag = this.AddPatientData(appointmentDetails.PatientId, appointmentDetails.PatientUnitId);
                }
                if (BizActionobj.AppointmentDetails.ResultStatus && flag)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddAppointment");
                    this.dbServer.AddInParameter(command2, "LinkServer", DbType.String, appointmentDetails.LinkServer);
                    if (appointmentDetails.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(command2, "LinkServerAlias", DbType.String, appointmentDetails.LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddInParameter(command2, "PatientID", DbType.Int64, appointmentDetails.PatientId);
                    this.dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, appointmentDetails.PatientUnitId);
                    this.dbServer.AddInParameter(command2, "VisitID", DbType.Int64, appointmentDetails.VisitId);
                    this.dbServer.AddInParameter(command2, "FirstName", DbType.String, Security.base64Encode(appointmentDetails.FirstName.Trim()));
                    this.dbServer.AddInParameter(command2, "MiddleName", DbType.String, Security.base64Encode(appointmentDetails.MiddleName.Trim()));
                    this.dbServer.AddInParameter(command2, "LastName", DbType.String, Security.base64Encode(appointmentDetails.LastName.Trim()));
                    this.dbServer.AddInParameter(command2, "FamilyName", DbType.String, Security.base64Encode(appointmentDetails.FamilyName.Trim()));
                    this.dbServer.AddInParameter(command2, "GenderID", DbType.Int64, appointmentDetails.GenderId);
                    this.dbServer.AddInParameter(command2, "DOB", DbType.DateTime, appointmentDetails.DOB);
                    this.dbServer.AddInParameter(command2, "BloodGroupID", DbType.Int64, appointmentDetails.BloodId);
                    this.dbServer.AddInParameter(command2, "MaritalStatusID", DbType.Int64, appointmentDetails.MaritalStatusId);
                    this.dbServer.AddInParameter(command2, "Contact1", DbType.String, appointmentDetails.ContactNo1.Trim());
                    this.dbServer.AddInParameter(command2, "Contact2", DbType.String, appointmentDetails.ContactNo2.Trim());
                    this.dbServer.AddInParameter(command2, "MobileCountryCode", DbType.String, appointmentDetails.MobileCountryCode);
                    this.dbServer.AddInParameter(command2, "ResiNoCountryCode", DbType.Int64, appointmentDetails.ResiNoCountryCode);
                    this.dbServer.AddInParameter(command2, "ResiSTDCode", DbType.Int64, appointmentDetails.ResiSTDCode);
                    this.dbServer.AddInParameter(command2, "FaxNo", DbType.String, appointmentDetails.FaxNo.Trim());
                    this.dbServer.AddInParameter(command2, "EmailId", DbType.String, Security.base64Encode(appointmentDetails.Email.Trim()));
                    this.dbServer.AddInParameter(command2, "DepartmentID", DbType.Int64, appointmentDetails.DepartmentId);
                    this.dbServer.AddInParameter(command2, "DoctorID", DbType.Int64, appointmentDetails.DoctorId);
                    this.dbServer.AddInParameter(command2, "AppointmentReasonID", DbType.Int64, appointmentDetails.AppointmentReasonId);
                    this.dbServer.AddInParameter(command2, "AppointmentDate", DbType.DateTime, appointmentDetails.AppointmentDate);
                    this.dbServer.AddInParameter(command2, "FromTime", DbType.DateTime, appointmentDetails.FromTime);
                    this.dbServer.AddInParameter(command2, "ToTime", DbType.DateTime, appointmentDetails.ToTime);
                    this.dbServer.AddInParameter(command2, "SpecialRegistrationID", DbType.Int64, appointmentDetails.SpecialRegistrationID);
                    this.dbServer.AddInParameter(command2, "Remark", DbType.String, Security.base64Encode(appointmentDetails.Remark.Trim()));
                    this.dbServer.AddInParameter(command2, "AppTypeId", DbType.Int64, appointmentDetails.AppointmentSourceId);
                    this.dbServer.AddInParameter(command2, "IsAcknowledge", DbType.Boolean, appointmentDetails.IsAcknowledged);
                    this.dbServer.AddInParameter(command2, "ReminderCount", DbType.Int64, appointmentDetails.ReminderCount);
                    this.dbServer.AddInParameter(command2, "AppointmentStatus", DbType.Int32, appointmentDetails.AppointmentStatus);
                    this.dbServer.AddInParameter(command2, "ParentappointID", DbType.Int64, appointmentDetails.ParentAppointmentID);
                    this.dbServer.AddInParameter(command2, "ParentappointUnitID", DbType.Int64, appointmentDetails.ParentAppointmentUnitID);
                    this.dbServer.AddInParameter(command2, "IsAge", DbType.Boolean, appointmentDetails.IsAge);
                    this.dbServer.AddInParameter(command2, "NationalityID", DbType.Int64, appointmentDetails.NationalityId);
                    this.dbServer.AddInParameter(command2, "Reschedule", DbType.String, Security.base64Encode(appointmentDetails.Reschedule.Trim()));
                    this.dbServer.AddInParameter(command2, "VisitMark", DbType.Boolean, appointmentDetails.VisitMark);
                    this.dbServer.AddInParameter(command2, "UserName", DbType.String, Security.base64Encode(appointmentDetails.UserName.Trim()));
                    this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, appointmentDetails.UnitId);
                    this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int64, 0x7fffffff);
                    this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, appointmentDetails.AppointmentID);
                    this.dbServer.ExecuteNonQuery(command2);
                    BizActionobj.AppointmentDetails.AppointmentID = (long) this.dbServer.GetParameterValue(command2, "ID");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionobj;
        }

        public override IValueObject AddCancelAppReason(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddCancelAppReasonBizActionVO nvo = valueObject as clsAddCancelAppReasonBizActionVO;
            try
            {
                clsAppointmentVO appointmentDetails = nvo.AppointmentDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddCancelAppointmentReason");
                this.dbServer.AddInParameter(storedProcCommand, "AppointmentId", DbType.Int64, appointmentDetails.AppointmentID);
                this.dbServer.AddInParameter(storedProcCommand, "AppCancelReason", DbType.String, Security.base64Encode(appointmentDetails.AppCancelReason.Trim()));
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, appointmentDetails.UnitId);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddMarkVisit(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddMarkVisitInAppointmenBizActionVO nvo = valueObject as clsAddMarkVisitInAppointmenBizActionVO;
            try
            {
                clsAppointmentVO appointmentDetails = nvo.AppointmentDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_MarkVisitInAppointment");
                this.dbServer.AddInParameter(storedProcCommand, "AppointmentId", DbType.Int64, appointmentDetails.AppointmentID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitMark", DbType.Boolean, appointmentDetails.VisitMark);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, appointmentDetails.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, appointmentDetails.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
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

        public override IValueObject CancelAppointment(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCancelAppointmentBizActionVO nvo = valueObject as clsCancelAppointmentBizActionVO;
            try
            {
                clsAppointmentVO appointmentDetails = nvo.AppointmentDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteAppointment");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, appointmentDetails.LinkServer);
                if (appointmentDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, appointmentDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "AppointmentId", DbType.Int64, appointmentDetails.AppointmentID);
                this.dbServer.AddInParameter(storedProcCommand, "AppCancelReason", DbType.String, Security.base64Encode(appointmentDetails.AppCancelReason.Trim()));
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, appointmentDetails.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "IsEnabled", DbType.Boolean, appointmentDetails.IsEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "AppointmentStatus", DbType.Int32, appointmentDetails.AppointmentStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return nvo;
        }

        public override IValueObject CheckAppointmentPatientDuplicacy(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCheckPatientDuplicacyAppointmentBizActionVO nvo = valueObject as clsCheckPatientDuplicacyAppointmentBizActionVO;
            try
            {
                clsAppointmentVO appointmentDetails = nvo.AppointmentDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AppointmentPatientDuplicacy");
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, Security.base64Encode(nvo.AppointmentDetails.FirstName));
                this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, Security.base64Encode(nvo.AppointmentDetails.LastName));
                this.dbServer.AddInParameter(storedProcCommand, "Gender", DbType.Int64, nvo.AppointmentDetails.GenderId);
                this.dbServer.AddInParameter(storedProcCommand, "DOB", DbType.DateTime, nvo.AppointmentDetails.DOB);
                this.dbServer.AddInParameter(storedProcCommand, "MobileCountryCode", DbType.String, nvo.AppointmentDetails.MobileCountryCode);
                this.dbServer.AddInParameter(storedProcCommand, "ContactNO1", DbType.String, nvo.AppointmentDetails.ContactNo1);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.AppointmentDetails.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.AppointmentDetails.PatientUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AppointmentDate", DbType.DateTime, nvo.AppointmentDetails.AppointmentDate);
                this.dbServer.AddInParameter(storedProcCommand, "FromTime", DbType.DateTime, nvo.AppointmentDetails.FromTime);
                this.dbServer.AddInParameter(storedProcCommand, "ToTime", DbType.DateTime, nvo.AppointmentDetails.ToTime);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                bool hasRows = reader.HasRows;
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject CheckAppointmentTime(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCheckAppointmentTimeBizActionVO nvo = (clsCheckAppointmentTimeBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CheckAppointmentTime");
                this.dbServer.AddInParameter(storedProcCommand, "DoctorId", DbType.Int64, nvo.DoctorId);
                this.dbServer.AddInParameter(storedProcCommand, "AppointmentDate", DbType.DateTime, nvo.AppointmentDate);
                this.dbServer.AddInParameter(storedProcCommand, "FromTime", DbType.DateTime, nvo.FromTime);
                this.dbServer.AddInParameter(storedProcCommand, "ToTime", DbType.DateTime, nvo.ToTime);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    nvo.SuccessStatus = true;
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject CheckMarkVisit(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCheckMarkVisitBizActionVO nvo = (clsCheckMarkVisitBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CheckVisitMark");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    nvo.SuccessStatus = true;
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject CheckMRNO(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCheckMRnoBizActionVO nvo = (clsCheckMRnoBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CheckForMRNO");
                this.dbServer.AddInParameter(storedProcCommand, "MRNO", DbType.String, nvo.MRNO);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    nvo.SuccessStatus = true;
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetAppointmentByDoctorAndAppointmentDate(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetAppointmentByDoctorAndAppointmentDateBizActionVO nvo = (clsGetAppointmentByDoctorAndAppointmentDateBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAppointmentListByDoctorIDAndDate");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, nvo.LinkServer);
                if (nvo.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, nvo.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorId);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentId", DbType.Int64, nvo.DepartmentId);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AppointmentDate", DbType.DateTime, nvo.AppointmentDate);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.AppointmentDetailsList == null)
                    {
                        nvo.AppointmentDetailsList = new List<clsAppointmentVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            break;
                        }
                        clsAppointmentVO item = new clsAppointmentVO {
                            DoctorId = (long) DALHelper.HandleDBNull(reader["DoctorID"]),
                            AppointmentDate = DALHelper.HandleDate(reader["AppointmentDate"]),
                            FromTime = new DateTime?((DateTime) reader["FromTime"]),
                            ToTime = new DateTime?((DateTime) reader["ToTime"])
                        };
                        nvo.AppointmentDetailsList.Add(item);
                    }
                }
                reader.Close();
            }
            catch
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetAppointmentBYId(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetAppointmentByIdBizActionVO nvo = valueObject as clsGetAppointmentByIdBizActionVO;
            try
            {
                clsAppointmentVO appointmentDetails = nvo.AppointmentDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAppointment");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, appointmentDetails.LinkServer);
                if (appointmentDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, appointmentDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "AppointmentId", DbType.Int64, appointmentDetails.AppointmentID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, appointmentDetails.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.AppointmentDetails.AppointmentID = (long) reader["ID"];
                        nvo.AppointmentDetails.FirstName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["FirstName"]));
                        nvo.AppointmentDetails.LastName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["LastName"]));
                        nvo.AppointmentDetails.MiddleName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["Middlename"]));
                        nvo.AppointmentDetails.FamilyName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["FamilyName"]));
                        nvo.AppointmentDetails.GenderId = (long) reader["GenderID"];
                        nvo.AppointmentDetails.NationalityId = (long) reader["NationalityId"];
                        nvo.AppointmentDetails.BloodId = (long) reader["BloodGroupID"];
                        nvo.AppointmentDetails.MaritalStatusId = (long) reader["MaritalStatusID"];
                        nvo.AppointmentDetails.ContactNo1 = (string) DALHelper.HandleDBNull(reader["Contact1"]);
                        nvo.AppointmentDetails.ContactNo2 = (string) DALHelper.HandleDBNull(reader["Contact2"]);
                        nvo.AppointmentDetails.MobileCountryCode = Convert.ToString(DALHelper.HandleDBNull(reader["MobileCountryCode"]));
                        nvo.AppointmentDetails.ResiNoCountryCode = (long) DALHelper.HandleDBNull(reader["ResiNoCountryCode"]);
                        nvo.AppointmentDetails.ResiSTDCode = (long) DALHelper.HandleDBNull(reader["ResiSTDCode"]);
                        nvo.AppointmentDetails.FaxNo = (string) DALHelper.HandleDBNull(reader["FaxNo"]);
                        nvo.AppointmentDetails.Email = Security.base64Decode((string) DALHelper.HandleDBNull(reader["EmailId"]));
                        nvo.AppointmentDetails.UnitId = (long) reader["UnitId"];
                        nvo.AppointmentDetails.DepartmentId = (long) reader["DepartmentId"];
                        nvo.AppointmentDetails.DoctorId = (long) reader["DoctorId"];
                        nvo.AppointmentDetails.AppointmentReasonId = (long) reader["AppointmentReasonId"];
                        nvo.AppointmentDetails.AppointmentDate = new DateTime?((DateTime) reader["AppointmentDate"]);
                        nvo.AppointmentDetails.FromTime = new DateTime?((DateTime) reader["FromTime"]);
                        nvo.AppointmentDetails.ToTime = new DateTime?((DateTime) reader["ToTime"]);
                        nvo.AppointmentDetails.Remark = Security.base64Decode((string) DALHelper.HandleDBNull(reader["Remark"]));
                        nvo.AppointmentDetails.SpecialRegistrationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecialRegistrationID"]));
                        nvo.AppointmentDetails.IsAge = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAge"]));
                        if (nvo.AppointmentDetails.IsAge)
                        {
                            nvo.AppointmentDetails.DateOfBirthFromAge = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["DOB"])));
                            continue;
                        }
                        nvo.AppointmentDetails.DOB = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["DOB"])));
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetAppointmentBYMrNo(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetAppointmentByMrNoBizActionVO nvo = valueObject as clsGetAppointmentByMrNoBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAppointmentByMrNo");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, nvo.LinkServer);
                if (nvo.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, nvo.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, nvo.MRNo);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsAppointmentVO appointmentDetails = nvo.AppointmentDetails;
                        nvo.AppointmentDetails.PatientId = (long) reader["ID"];
                        nvo.AppointmentDetails.PatientUnitId = (long) reader["UnitID"];
                        nvo.AppointmentDetails.FirstName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["FirstName"]));
                        nvo.AppointmentDetails.MiddleName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["MiddleName"]));
                        nvo.AppointmentDetails.LastName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["LastName"]));
                        nvo.AppointmentDetails.FamilyName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["FamilyName"]));
                        nvo.AppointmentDetails.GenderId = (long) reader["GenderID"];
                        nvo.AppointmentDetails.SpecialRegistrationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecialRegistrationID"]));
                        nvo.AppointmentDetails.IsAge = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAge"]));
                        if (nvo.AppointmentDetails.IsAge)
                        {
                            nvo.AppointmentDetails.DateOfBirthFromAge = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["DOB"])));
                        }
                        else
                        {
                            nvo.AppointmentDetails.DOB = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["DOB"])));
                        }
                        nvo.AppointmentDetails.BloodId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BloodGroupID"]));
                        nvo.AppointmentDetails.MaritalStatusId = Convert.ToInt64(reader["MaritalStatusID"]);
                        nvo.AppointmentDetails.CivilId = reader["CivilID"].ToString();
                        nvo.AppointmentDetails.ContactNo1 = (string) DALHelper.HandleDBNull(reader["ContactNo1"]);
                        nvo.AppointmentDetails.ContactNo2 = (string) DALHelper.HandleDBNull(reader["ContactNo2"]);
                        nvo.AppointmentDetails.ResiNoCountryCode = (long) DALHelper.HandleDBNull(reader["ResiNoCountryCode"]);
                        nvo.AppointmentDetails.ResiSTDCode = (long) DALHelper.HandleDBNull(reader["ResiSTDCode"]);
                        nvo.AppointmentDetails.MobileCountryCode = Convert.ToString(DALHelper.HandleDBNull(reader["MobileCountryCode"]));
                        nvo.AppointmentDetails.FaxNo = (string) DALHelper.HandleDBNull(reader["FaxNo"]);
                        nvo.AppointmentDetails.Email = Security.base64Decode((string) DALHelper.HandleDBNull(reader["Email"]));
                        nvo.AppointmentDetails.NationalityId = Convert.ToInt64(reader["NationalityID"]);
                        nvo.AppointmentDetails.Gender = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"]));
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetAppointmentDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetAppointmentBizActionVO nvo = (clsGetAppointmentBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAppointmentMrNoList");
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, nvo.DepartmentId);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorId);
                this.dbServer.AddInParameter(storedProcCommand, "UnRegistered", DbType.Boolean, nvo.UnRegistered);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.InputPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.InputStartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.InputMaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.SortExpression);
                this.dbServer.AddInParameter(storedProcCommand, "AppointStatus", DbType.Int32, nvo.AppintmentStatusID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitMark", DbType.Int32, nvo.VisitMark);
                this.dbServer.AddInParameter(storedProcCommand, "AppType", DbType.Int64, nvo.AppType);
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo", DbType.String, nvo.ContactNo);
                this.dbServer.AddInParameter(storedProcCommand, "SpecialRegistrationId", DbType.Int64, nvo.SpecialRegistrationId);
                this.dbServer.AddInParameter(storedProcCommand, "@Status", DbType.Boolean, true);
                int appintmentStatusID = nvo.AppintmentStatusID;
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, nvo.FirstName);
                this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, nvo.LastName);
                this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, nvo.MrNo);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.AppointmentDetailsList == null)
                    {
                        nvo.AppointmentDetailsList = new List<clsAppointmentVO>();
                    }
                    while (reader.Read())
                    {
                        clsAppointmentVO item = new clsAppointmentVO {
                            AppointmentID = (long) reader["ID"],
                            PatientId = (long) DALHelper.HandleDBNull(reader["PatientID"]),
                            PatientUnitId = (long) DALHelper.HandleDBNull(reader["PatientUnitID"]),
                            VisitId = (long) DALHelper.HandleDBNull(reader["VisitID"]),
                            FirstName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["FirstName"])),
                            MiddleName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["MiddleName"])),
                            LastName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["LastName"])),
                            FamilyName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["FamilyName"])),
                            GenderId = (long) DALHelper.HandleDBNull(reader["GenderID"]),
                            BloodId = (long) reader["BloodGroupID"],
                            MaritalStatusId = (long) reader["MaritalStatusID"],
                            ContactNo1 = (string) DALHelper.HandleDBNull(reader["Contact1"]),
                            ContactNo2 = (string) DALHelper.HandleDBNull(reader["Contact2"]),
                            MobileCountryCode = (string) DALHelper.HandleDBNull(reader["MobileCountryCode"]),
                            ResiNoCountryCode = (long) DALHelper.HandleDBNull(reader["ResiNoCountryCode"]),
                            ResiSTDCode = (long) DALHelper.HandleDBNull(reader["ResiSTDCode"]),
                            FaxNo = (string) DALHelper.HandleDBNull(reader["FaxNo"]),
                            Email = Security.base64Decode((string) DALHelper.HandleDBNull(reader["EmailId"])),
                            UnitId = (long) DALHelper.HandleDBNull(reader["UnitId"]),
                            DepartmentId = (long) DALHelper.HandleDBNull(reader["DepartmentId"]),
                            AppointmentReasonId = (long) DALHelper.HandleDBNull(reader["AppointmentReasonID"]),
                            AppointmentReason = (string) DALHelper.HandleDBNull(reader["AppointmentReason"]),
                            AppointmentDate = DALHelper.HandleDate(reader["AppointmentDate"]),
                            FromTime = new DateTime?((DateTime) reader["FromTime"]),
                            ToTime = new DateTime?((DateTime) reader["ToTime"]),
                            Remark = Security.base64Decode((string) DALHelper.HandleDBNull(reader["Remark"])),
                            RegRemark = Security.base64Decode((string) DALHelper.HandleDBNull(reader["RegRemark"])),
                            DoctorName = reader["DoctorName"].ToString(),
                            Description = reader["Description"].ToString(),
                            DoctorId = (long) DALHelper.HandleDBNull(reader["DoctorId"]),
                            MrNo = reader["MRNo"].ToString(),
                            VisitMark = (bool) DALHelper.HandleDBNull(reader["VisitMark"]),
                            SpecialRegistrationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecialRegistrationID"])),
                            SpecialRegistration = (string) DALHelper.HandleDBNull(reader["SpecialRegistration"]),
                            AddedDateTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["AddedDateTime"]))),
                            UpdatedDateTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["UpdatedDateTime"]))),
                            createdByName = Convert.ToString(DALHelper.HandleDBNull(reader["createdByName"])),
                            ModifiedByName = Convert.ToString(DALHelper.HandleDBNull(reader["ModifiedByName"])),
                            MarkVisitStatus = Convert.ToString(DALHelper.HandleDBNull(reader["MarkVisitStatus"])),
                            AppointmentStatusNew = Convert.ToString(DALHelper.HandleDBNull(reader["AppointmentStatusNew"])),
                            AppointmentStatusID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ASID"])),
                            IsAge = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAge"])),
                            IsDocAttached = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDocAttached"]))
                        };
                        if (item.IsAge)
                        {
                            item.DateOfBirthFromAge = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["DOB"])));
                        }
                        else
                        {
                            item.DOB = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["DOB"])));
                        }
                        item.Reschedule = Security.base64Decode((string) DALHelper.HandleDBNull(reader["ReSchedulingReason"]));
                        item.Cancelschedule = Security.base64Decode((string) DALHelper.HandleDBNull(reader["AppCancelReason"]));
                        item.NationalityId = (long) DALHelper.HandleDBNull(reader["NationalityID"]);
                        item.CompanyID = (long) DALHelper.HandleDBNull(reader["CompanyID"]);
                        item.UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["Name"]));
                        nvo.AppointmentDetailsList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.OutputTotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetDoctorTime(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDoctorTimeBizActionVO nvo = (clsGetDoctorTimeBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorTime");
                this.dbServer.AddInParameter(storedProcCommand, "DoctorId", DbType.Int64, nvo.DoctorId);
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.InputSearchExpression);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.AppointmentDetailsList == null)
                    {
                        nvo.AppointmentDetailsList = new List<clsAppointmentVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            break;
                        }
                        clsAppointmentVO item = new clsAppointmentVO {
                            Schedule1_StartTime = (string) DALHelper.HandleDBNull(reader["Schedule1_StartTime"]),
                            Schedule1_EndTime = (string) DALHelper.HandleDBNull(reader["Schedule1_EndTime"]),
                            Schedule2_StartTime = (string) DALHelper.HandleDBNull(reader["Schedule2_StartTime"]),
                            Schedule2_EndTime = (string) DALHelper.HandleDBNull(reader["Schedule2_EndTime"])
                        };
                        nvo.AppointmentDetailsList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetFutureAppointment(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetFutureAppointmentBizActionVO nvo = valueObject as clsGetFutureAppointmentBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetFutureAppointment");
                this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, nvo.MRNO);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitId", DbType.Int64, nvo.PatientUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.AppointmentList == null)
                    {
                        nvo.AppointmentList = new List<clsAppointmentVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        clsAppointmentVO item = new clsAppointmentVO {
                            FutureAppointmentDate = DALHelper.HandleDate(reader["AppointmentDate"]),
                            FutureAppointmentFromTime = (string) DALHelper.HandleDBNull(reader["AppointmentTime"]),
                            DoctorId = DALHelper.HandleIntegerNull(reader["DoctorID"]),
                            DoctorName = (string) DALHelper.HandleDBNull(reader["DoctorName"]),
                            DepartmentId = DALHelper.HandleIntegerNull(reader["DepartmentID"]),
                            Description = (string) DALHelper.HandleDBNull(reader["DepartmentName"]),
                            UnitId = DALHelper.HandleIntegerNull(reader["UnitID"]),
                            UnitName = (string) DALHelper.HandleDBNull(reader["UnitName"]),
                            AppointmentReason = (string) DALHelper.HandleDBNull(reader["AppointmentReason"])
                        };
                        nvo.AppointmentList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetPastAppointment(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPastAppointmentBizActionVO nvo = valueObject as clsGetPastAppointmentBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPastAppointment");
                this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, nvo.MRNO);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitId", DbType.Int64, nvo.PatientUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.AppointmentList == null)
                    {
                        nvo.AppointmentList = new List<clsAppointmentVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        clsAppointmentVO item = new clsAppointmentVO {
                            PastAppointmentDate = DALHelper.HandleDate(reader["AppointmentDate"]),
                            PastAppointmentFromTime = (string) DALHelper.HandleDBNull(reader["AppointmentTime"]),
                            DoctorId = DALHelper.HandleIntegerNull(reader["DoctorID"]),
                            DoctorName = (string) DALHelper.HandleDBNull(reader["DoctorName"]),
                            DepartmentId = DALHelper.HandleIntegerNull(reader["DepartmentID"]),
                            Description = (string) DALHelper.HandleDBNull(reader["DepartmentName"]),
                            UnitId = DALHelper.HandleIntegerNull(reader["UnitID"]),
                            UnitName = (string) DALHelper.HandleDBNull(reader["UnitName"]),
                            AppointmentReason = (string) DALHelper.HandleDBNull(reader["AppointmentReason"])
                        };
                        nvo.AppointmentList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        private clsAddAppointmentBizActionVO UpdateAppointment(clsAddAppointmentBizActionVO BizActionobj, clsUserVO UserVo)
        {
            try
            {
                clsAppointmentVO appointmentDetails = BizActionobj.AppointmentDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateAppointment");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, appointmentDetails.LinkServer);
                if (appointmentDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, appointmentDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, appointmentDetails.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, appointmentDetails.PatientUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, appointmentDetails.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, appointmentDetails.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, appointmentDetails.AppointmentID);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionobj;
        }
    }
}

