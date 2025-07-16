using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.Web.Logging;
using System.Data;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
using System.Data.Common;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects.Patient;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsAppointmentDAL : clsBaseAppointmentDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion


        private clsAppointmentDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql Object
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
        public override IValueObject CheckAppointmentPatientDuplicacy(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCheckPatientDuplicacyAppointmentBizActionVO BizActionObj = valueObject as clsCheckPatientDuplicacyAppointmentBizActionVO;
            try
            {
                clsAppointmentVO objPatientVO = BizActionObj.AppointmentDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AppointmentPatientDuplicacy");

                DbDataReader reader;

                dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(BizActionObj.AppointmentDetails.FirstName));
                dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(BizActionObj.AppointmentDetails.LastName));
                dbServer.AddInParameter(command, "Gender", DbType.Int64, BizActionObj.AppointmentDetails.GenderId);
                dbServer.AddInParameter(command, "DOB", DbType.DateTime, BizActionObj.AppointmentDetails.DOB);
                dbServer.AddInParameter(command, "MobileCountryCode", DbType.String, BizActionObj.AppointmentDetails.MobileCountryCode);
                dbServer.AddInParameter(command, "ContactNO1", DbType.String, BizActionObj.AppointmentDetails.ContactNo1);

                //dbServer.AddInParameter(command, "SFirstName", DbType.String, Security.base64Encode(BizActionObj.PatientDetails.SpouseDetails.FirstName));
                //dbServer.AddInParameter(command, "SLastName", DbType.String, Security.base64Encode(BizActionObj.PatientDetails.SpouseDetails.LastName));
                //dbServer.AddInParameter(command, "SGender", DbType.Int64, BizActionObj.PatientDetails.SpouseDetails.GenderID);
                //dbServer.AddInParameter(command, "SDOB", DbType.DateTime, BizActionObj.PatientDetails.SpouseDetails.DateOfBirth);
                //dbServer.AddInParameter(command, "SMobileCountryCode", DbType.String, BizActionObj.PatientDetails.SpouseDetails.MobileCountryCode);
                //dbServer.AddInParameter(command, "SContactNO1", DbType.String, BizActionObj.PatientDetails.SpouseDetails.ContactNO1);

                //dbServer.AddInParameter(command, "PatientEditMode", DbType.Boolean, BizActionObj.PatientEditMode);


                //dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientDetails.GeneralDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.AppointmentDetails.UnitId);



                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                if (reader.HasRows)
                {
                    #region commented
                    //if (BizActionObj.PatientList == null)
                    //    BizActionObj.PatientList = new List<clsPatientGeneralVO>();
                    //while (reader.Read())
                    //{
                    //    clsPatientGeneralVO Obj = new clsPatientGeneralVO();
                    //    Obj.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                    //    Obj.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                    //    Obj.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                    //    Obj.DateOfBirth = (DateTime?)DALHelper.HandleDate(reader["DateofBirth"]);
                    //    Obj.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                    //    Obj.RegistrationDate = (DateTime?)DALHelper.HandleDate(reader["RegistrationDate"]);
                    //    Obj.PatientID = (long)DALHelper.HandleDBNull(reader["ID"]);
                    //    Obj.UnitId = (long)DALHelper.HandleDBNull(reader["UnitId"]);
                    //    Obj.Gender = (string)DALHelper.HandleDBNull(reader["Gender"]);

                    //    BizActionObj.ResultStatus = true;

                    //    BizActionObj.PatientList.Add(Obj);

                    //}

                    #endregion
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

            return valueObject;


        }
        public override IValueObject AddAppointment(IValueObject valueObject, clsUserVO UserVo)
        {

            clsAddAppointmentBizActionVO BizActionobj = valueObject as clsAddAppointmentBizActionVO;
            if (BizActionobj.AppointmentDetails.AppointmentID == 0)
            {
                BizActionobj = AddAppointment(BizActionobj, UserVo);
            }
            else
            {
                BizActionobj = UpdateAppointment(BizActionobj, UserVo);
            }
            return BizActionobj;


        }

        private clsAddAppointmentBizActionVO AddAppointment(clsAddAppointmentBizActionVO BizActionobj, clsUserVO UserVo)
        {

            try
            {
                clsAppointmentVO objAppointmentVO = BizActionobj.AppointmentDetails;
                bool ValidFlag = true;


                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_SearchAppointment");
                dbServer.AddInParameter(command1, "LinkServer", DbType.String, objAppointmentVO.LinkServer);
                if (objAppointmentVO.LinkServer != null)
                    dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, objAppointmentVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command1, "PatientID", DbType.Int64, objAppointmentVO.PatientId);
                dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, objAppointmentVO.PatientUnitId);
                dbServer.AddInParameter(command1, "AppointmentDate", DbType.DateTime, objAppointmentVO.AppointmentDate);
                dbServer.AddInParameter(command1, "FromTime", DbType.DateTime, objAppointmentVO.FromTime);
                dbServer.AddInParameter(command1, "ToTime", DbType.DateTime, objAppointmentVO.ToTime);
                dbServer.AddParameter(command1, "ResultStatus", DbType.Boolean, ParameterDirection.InputOutput, null, DataRowVersion.Default, objAppointmentVO.ResultStatus);

                int intStatus1 = dbServer.ExecuteNonQuery(command1);

                BizActionobj.AppointmentDetails.ResultStatus = (bool)dbServer.GetParameterValue(command1, "ResultStatus");






                if (objAppointmentVO.PatientUnitId != UserVo.UserLoginInfo.UnitId)
                {
                    ValidFlag = AddPatientData(objAppointmentVO.PatientId, objAppointmentVO.PatientUnitId);
                }
                if (BizActionobj.AppointmentDetails.ResultStatus)
                {
                    if (ValidFlag)
                    {
                        DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddAppointment");

                        dbServer.AddInParameter(command, "LinkServer", DbType.String, objAppointmentVO.LinkServer);
                        if (objAppointmentVO.LinkServer != null)
                            dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objAppointmentVO.LinkServer.Replace(@"\", "_"));


                        dbServer.AddInParameter(command, "PatientID", DbType.Int64, objAppointmentVO.PatientId);
                        dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objAppointmentVO.PatientUnitId);
                        dbServer.AddInParameter(command, "VisitID", DbType.Int64, objAppointmentVO.VisitId);
                        dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(objAppointmentVO.FirstName.Trim()));
                        dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(objAppointmentVO.MiddleName.Trim()));
                        dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(objAppointmentVO.LastName.Trim()));
                        dbServer.AddInParameter(command, "FamilyName", DbType.String, Security.base64Encode(objAppointmentVO.FamilyName.Trim()));
                        dbServer.AddInParameter(command, "GenderID", DbType.Int64, objAppointmentVO.GenderId);
                        dbServer.AddInParameter(command, "DOB", DbType.DateTime, objAppointmentVO.DOB);
                        dbServer.AddInParameter(command, "BloodGroupID", DbType.Int64, objAppointmentVO.BloodId);
                        dbServer.AddInParameter(command, "MaritalStatusID", DbType.Int64, objAppointmentVO.MaritalStatusId);
                        dbServer.AddInParameter(command, "Contact1", DbType.String, objAppointmentVO.ContactNo1.Trim());
                        dbServer.AddInParameter(command, "Contact2", DbType.String, objAppointmentVO.ContactNo2.Trim());
                        dbServer.AddInParameter(command, "MobileCountryCode", DbType.Int64, objAppointmentVO.MobileCountryCode);
                        dbServer.AddInParameter(command, "ResiNoCountryCode", DbType.Int64, objAppointmentVO.ResiNoCountryCode);
                        dbServer.AddInParameter(command, "ResiSTDCode", DbType.Int64, objAppointmentVO.ResiSTDCode);
                        dbServer.AddInParameter(command, "FaxNo", DbType.String, objAppointmentVO.FaxNo.Trim());
                        dbServer.AddInParameter(command, "EmailId", DbType.String, Security.base64Encode(objAppointmentVO.Email.Trim()));
                        dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, objAppointmentVO.DepartmentId);
                        dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objAppointmentVO.DoctorId);
                        dbServer.AddInParameter(command, "AppointmentReasonID", DbType.Int64, objAppointmentVO.AppointmentReasonId);
                        dbServer.AddInParameter(command, "AppointmentDate", DbType.DateTime, objAppointmentVO.AppointmentDate);
                        dbServer.AddInParameter(command, "FromTime", DbType.DateTime, objAppointmentVO.FromTime);
                        dbServer.AddInParameter(command, "ToTime", DbType.DateTime, objAppointmentVO.ToTime);
                        dbServer.AddInParameter(command, "SpecialRegistrationID", DbType.Int64, objAppointmentVO.SpecialRegistrationID);

                        dbServer.AddInParameter(command, "Remark", DbType.String, Security.base64Encode(objAppointmentVO.Remark.Trim()));
                        dbServer.AddInParameter(command, "AppTypeId", DbType.Int64, objAppointmentVO.AppointmentSourceId);
                        dbServer.AddInParameter(command, "IsAcknowledge", DbType.Boolean, objAppointmentVO.IsAcknowledged);
                        dbServer.AddInParameter(command, "ReminderCount", DbType.Int64, objAppointmentVO.ReminderCount);
                        dbServer.AddInParameter(command, "AppointmentStatus", DbType.Int32, objAppointmentVO.AppointmentStatus);
                        dbServer.AddInParameter(command, "ParentappointID", DbType.Int64, objAppointmentVO.ParentAppointmentID);
                        dbServer.AddInParameter(command, "ParentappointUnitID", DbType.Int64, objAppointmentVO.ParentAppointmentUnitID);
                        dbServer.AddInParameter(command, "IsAge", DbType.Boolean, objAppointmentVO.IsAge);

                        //***// added by ajit To save NationalityID /9/8/16
                        dbServer.AddInParameter(command, "NationalityID", DbType.Int64, objAppointmentVO.NationalityId);

                        //added date 24/9/2016
                        dbServer.AddInParameter(command, "Reschedule", DbType.String, Security.base64Encode(objAppointmentVO.Reschedule.Trim()));

                        //--------


                        dbServer.AddInParameter(command, "VisitMark", DbType.Boolean, objAppointmentVO.VisitMark);

                        dbServer.AddInParameter(command, "UserName", DbType.String, Security.base64Encode(objAppointmentVO.UserName.Trim()));
                        dbServer.AddInParameter(command, "Status", DbType.Boolean, true);

                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, objAppointmentVO.UnitId);
                        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                        dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objAppointmentVO.AppointmentID);
                        int intStatus = dbServer.ExecuteNonQuery(command);

                        //BizActionobj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                        BizActionobj.AppointmentDetails.AppointmentID = (long)dbServer.GetParameterValue(command, "ID");
                    }
                }
                else
                {

                }
            }


            catch (Exception ex)
            {
                throw;

            }
            finally
            {

            }
            return BizActionobj;
        }

        private clsAddAppointmentBizActionVO UpdateAppointment(clsAddAppointmentBizActionVO BizActionobj, clsUserVO UserVo)
        {
            try
            {
                clsAppointmentVO objAppointmentVO = BizActionobj.AppointmentDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateAppointment");

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objAppointmentVO.LinkServer);
                if (objAppointmentVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objAppointmentVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objAppointmentVO.PatientId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objAppointmentVO.PatientUnitId);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objAppointmentVO.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objAppointmentVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddInParameter(command, "ID", DbType.Int64, objAppointmentVO.AppointmentID);

                int intStatus = dbServer.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw;

            }
            finally
            {

            }
            return BizActionobj;
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

        public override IValueObject GetAppointmentDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetAppointmentBizActionVO BizActionObj = (clsGetAppointmentBizActionVO)valueObject;

            try
            {
                //DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAppointmentListBySearchCriteria");
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAppointmentMrNoList");

                //dbServer.AddInParameter(command, "LinkServer", DbType.String, BizActionObj.LinkServer);
                //if (BizActionObj.LinkServer != null)
                //    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, BizActionObj.LinkServer.Replace(@"\", "_"));



                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitId);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizActionObj.DepartmentId);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.DoctorId);
                dbServer.AddInParameter(command, "UnRegistered", DbType.Boolean, BizActionObj.UnRegistered);
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.InputPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.InputStartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.InputMaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.SortExpression);
                dbServer.AddInParameter(command, "AppointStatus", DbType.Int32, BizActionObj.AppintmentStatusID);
                dbServer.AddInParameter(command, "VisitMark", DbType.Int32, BizActionObj.VisitMark);
                dbServer.AddInParameter(command, "AppType", DbType.Int64, BizActionObj.AppType);
                dbServer.AddInParameter(command, "ContactNo", DbType.String, BizActionObj.ContactNo);
                dbServer.AddInParameter(command, "SpecialRegistrationId", DbType.Int64, BizActionObj.SpecialRegistrationId);
                dbServer.AddInParameter(command, "@Status", DbType.Boolean, true);

                long AppTypeID = 0;
                AppTypeID = BizActionObj.AppintmentStatusID;

                dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.FirstName);
                //  dbServer.AddInParameter(command, "MiddleName", DbType.String, BizActionObj.MiddleName);
                dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.LastName);
                dbServer.AddInParameter(command, "MRNo", DbType.String, BizActionObj.MrNo);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);




                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.AppointmentDetailsList == null)
                        BizActionObj.AppointmentDetailsList = new List<clsAppointmentVO>();
                    while (reader.Read())
                    {
                        clsAppointmentVO AppointmentVO = new clsAppointmentVO();

                        AppointmentVO.AppointmentID = (long)reader["ID"];
                        AppointmentVO.PatientId = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                        AppointmentVO.PatientUnitId = (long)DALHelper.HandleDBNull(reader["PatientUnitID"]);

                        AppointmentVO.VisitId = (long)DALHelper.HandleDBNull(reader["VisitID"]);


                        AppointmentVO.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                        AppointmentVO.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                        AppointmentVO.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                        AppointmentVO.FamilyName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FamilyName"]));
                        AppointmentVO.GenderId = (long)DALHelper.HandleDBNull(reader["GenderID"]);
                        // AppointmentVO.DOB = (DateTime?)DALHelper.HandleDate(reader["DOB"]);
                        AppointmentVO.BloodId = (long)reader["BloodGroupID"];
                        AppointmentVO.MaritalStatusId = (long)reader["MaritalStatusID"];
                        AppointmentVO.ContactNo1 = (string)DALHelper.HandleDBNull(reader["Contact1"]);
                        AppointmentVO.ContactNo2 = (string)DALHelper.HandleDBNull(reader["Contact2"]);

                        AppointmentVO.MobileCountryCode = (string)DALHelper.HandleDBNull(reader["MobileCountryCode"]);
                        AppointmentVO.ResiNoCountryCode = (long)DALHelper.HandleDBNull(reader["ResiNoCountryCode"]);
                        AppointmentVO.ResiSTDCode = (long)DALHelper.HandleDBNull(reader["ResiSTDCode"]);

                        AppointmentVO.FaxNo = (string)DALHelper.HandleDBNull(reader["FaxNo"]);
                        AppointmentVO.Email = Security.base64Decode((string)DALHelper.HandleDBNull(reader["EmailId"]));
                        AppointmentVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitId"]);
                        AppointmentVO.DepartmentId = (long)DALHelper.HandleDBNull(reader["DepartmentId"]);

                        AppointmentVO.AppointmentReasonId = (long)DALHelper.HandleDBNull(reader["AppointmentReasonID"]);
                        AppointmentVO.AppointmentReason = (string)DALHelper.HandleDBNull(reader["AppointmentReason"]);
                        AppointmentVO.AppointmentDate = (DateTime?)DALHelper.HandleDate(reader["AppointmentDate"]);
                        AppointmentVO.FromTime = (DateTime)reader["FromTime"];
                        AppointmentVO.ToTime = (DateTime)reader["ToTime"];
                        AppointmentVO.Remark = Security.base64Decode((string)DALHelper.HandleDBNull(reader["Remark"]));
                        AppointmentVO.RegRemark = Security.base64Decode((string)DALHelper.HandleDBNull(reader["RegRemark"]));
                        AppointmentVO.DoctorName = reader["DoctorName"].ToString();
                        AppointmentVO.Description = reader["Description"].ToString();
                        AppointmentVO.DoctorId = (long)DALHelper.HandleDBNull(reader["DoctorId"]);
                        AppointmentVO.MrNo = reader["MRNo"].ToString();
                        AppointmentVO.VisitMark = (bool)DALHelper.HandleDBNull(reader["VisitMark"]);
                        AppointmentVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitId"]);
                        AppointmentVO.SpecialRegistrationID = (Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecialRegistrationID"])));
                        AppointmentVO.SpecialRegistration = (string)DALHelper.HandleDBNull(reader["SpecialRegistration"]);
                        AppointmentVO.AddedDateTime = (Convert.ToDateTime(DALHelper.HandleDate(reader["AddedDateTime"])));
                        AppointmentVO.UpdatedDateTime = (Convert.ToDateTime(DALHelper.HandleDate(reader["UpdatedDateTime"])));
                        AppointmentVO.createdByName = (Convert.ToString(DALHelper.HandleDBNull(reader["createdByName"])));
                        AppointmentVO.ModifiedByName = (Convert.ToString(DALHelper.HandleDBNull(reader["ModifiedByName"])));
                        AppointmentVO.MarkVisitStatus = (Convert.ToString(DALHelper.HandleDBNull(reader["MarkVisitStatus"])));
                        AppointmentVO.AppointmentStatusNew = (Convert.ToString(DALHelper.HandleDBNull(reader["AppointmentStatusNew"])));
                        AppointmentVO.IsAge = (Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAge"])));
                        AppointmentVO.IsDocAttached = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDocAttached"]));
                        if (AppointmentVO.IsAge == true) 
                        {
                            AppointmentVO.DateOfBirthFromAge = Convert.ToDateTime(DALHelper.HandleDate(reader["DOB"]));
                        }
                        else
                        {
                            AppointmentVO.DOB = Convert.ToDateTime(DALHelper.HandleDate(reader["DOB"]));
                        }
                        //* Added by - Ajit Jadhav  //* Added Date - 24/9/2016   //* Comments - get ReScheduling Reason
                        AppointmentVO.Reschedule = Security.base64Decode((string)DALHelper.HandleDBNull(reader["ReSchedulingReason"]));
                        AppointmentVO.Cancelschedule = Security.base64Decode((string)DALHelper.HandleDBNull(reader["AppCancelReason"]));   //* Added by - Ajit Jadhav  //* Added Date - 4/10/2016 

                        // Added by ajit jadhav Date 12/10/2016 
                        AppointmentVO.NationalityId = (long)DALHelper.HandleDBNull(reader["NationalityID"]); 
                        //***//---------------------

                        BizActionObj.AppointmentDetailsList.Add(AppointmentVO);
                    }


                }
                reader.NextResult();
                BizActionObj.OutputTotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
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

        public override IValueObject CancelAppointment(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsCancelAppointmentBizActionVO BizActionobj = valueObject as clsCancelAppointmentBizActionVO;

            try
            {
                clsAppointmentVO objAppointmentVO = BizActionobj.AppointmentDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_DeleteAppointment");

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objAppointmentVO.LinkServer);
                if (objAppointmentVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objAppointmentVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "AppointmentId", DbType.Int64, objAppointmentVO.AppointmentID);
                dbServer.AddInParameter(command, "AppCancelReason", DbType.String, Security.base64Encode(objAppointmentVO.AppCancelReason.Trim()));

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objAppointmentVO.UnitId);
                dbServer.AddInParameter(command, "IsEnabled", DbType.Boolean, objAppointmentVO.IsEnabled);

                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "AppointmentStatus", DbType.Int32, objAppointmentVO.AppointmentStatus);

                int intStatus = dbServer.ExecuteNonQuery(command);

            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {

            }
            return BizActionobj;

        }

        public override IValueObject GetAppointmentBYId(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetAppointmentByIdBizActionVO BizAction = valueObject as clsGetAppointmentByIdBizActionVO;
            try
            {
                clsAppointmentVO ObjAppointmentVO = BizAction.AppointmentDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAppointment");
                DbDataReader reader;

                dbServer.AddInParameter(command, "LinkServer", DbType.String, ObjAppointmentVO.LinkServer);
                if (ObjAppointmentVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, ObjAppointmentVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "AppointmentId", DbType.Int64, ObjAppointmentVO.AppointmentID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, ObjAppointmentVO.UnitId);


                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizAction.AppointmentDetails.AppointmentID = (long)reader["ID"];
                        BizAction.AppointmentDetails.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                        BizAction.AppointmentDetails.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                        BizAction.AppointmentDetails.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["Middlename"]));
                        BizAction.AppointmentDetails.FamilyName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FamilyName"]));
                        BizAction.AppointmentDetails.GenderId = (long)reader["GenderID"];
                        //  BizAction.AppointmentDetails.DOB = (DateTime?)DALHelper.HandleDate(reader["DOB"]); //Convert.ToDateTime(reader["DOB"]);

                       // Added by ajit jadhav Date 6/10/2016 
                        BizAction.AppointmentDetails.NationalityId = (long)reader["NationalityId"];
                        //***//---------------------

                        BizAction.AppointmentDetails.BloodId = (long)reader["BloodGroupID"];
                        BizAction.AppointmentDetails.MaritalStatusId = (long)reader["MaritalStatusID"];

                        BizAction.AppointmentDetails.ContactNo1 = (string)DALHelper.HandleDBNull(reader["Contact1"]);
                        BizAction.AppointmentDetails.ContactNo2 = (string)DALHelper.HandleDBNull(reader["Contact2"]);



                        BizAction.AppointmentDetails.MobileCountryCode = Convert.ToString(DALHelper.HandleDBNull(reader["MobileCountryCode"]));
                        BizAction.AppointmentDetails.ResiNoCountryCode = (long)DALHelper.HandleDBNull(reader["ResiNoCountryCode"]);
                        BizAction.AppointmentDetails.ResiSTDCode = (long)DALHelper.HandleDBNull(reader["ResiSTDCode"]);


                        BizAction.AppointmentDetails.FaxNo = (string)DALHelper.HandleDBNull(reader["FaxNo"]);
                        BizAction.AppointmentDetails.Email = Security.base64Decode((string)DALHelper.HandleDBNull(reader["EmailId"]));
                        BizAction.AppointmentDetails.UnitId = (long)reader["UnitId"];
                        BizAction.AppointmentDetails.DepartmentId = (long)reader["DepartmentId"];
                        BizAction.AppointmentDetails.DoctorId = (long)reader["DoctorId"];
                        BizAction.AppointmentDetails.AppointmentReasonId = (long)reader["AppointmentReasonId"];
                        BizAction.AppointmentDetails.AppointmentDate = (DateTime)reader["AppointmentDate"];
                        BizAction.AppointmentDetails.FromTime = (DateTime)reader["FromTime"];
                        BizAction.AppointmentDetails.ToTime = (DateTime)reader["ToTime"];
                        BizAction.AppointmentDetails.Remark = Security.base64Decode((string)DALHelper.HandleDBNull(reader["Remark"]));
                        //By Anjali........................................
                        BizAction.AppointmentDetails.SpecialRegistrationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecialRegistrationID"]));
                        BizAction.AppointmentDetails.IsAge = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAge"]));
                        if (BizAction.AppointmentDetails.IsAge == true)
                        {
                            BizAction.AppointmentDetails.DateOfBirthFromAge = Convert.ToDateTime(DALHelper.HandleDate(reader["DOB"]));
                        }
                        else
                        {
                            BizAction.AppointmentDetails.DOB = Convert.ToDateTime(DALHelper.HandleDate(reader["DOB"]));
                        }
                        //..........................................



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

            return BizAction;
        }

        public override IValueObject GetAppointmentBYMrNo(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetAppointmentByMrNoBizActionVO BizAction = valueObject as clsGetAppointmentByMrNoBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAppointmentByMrNo");
                DbDataReader reader;

                dbServer.AddInParameter(command, "LinkServer", DbType.String, BizAction.LinkServer);
                if (BizAction.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, BizAction.LinkServer.Replace(@"\", "_"));


                dbServer.AddInParameter(command, "MRNo", DbType.String, BizAction.MRNo);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsAppointmentVO ObjAppointmentVO = BizAction.AppointmentDetails;
                        BizAction.AppointmentDetails.PatientId = (long)reader["ID"];
                        BizAction.AppointmentDetails.PatientUnitId = (long)reader["UnitID"];
                        BizAction.AppointmentDetails.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                        BizAction.AppointmentDetails.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                        BizAction.AppointmentDetails.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                        BizAction.AppointmentDetails.FamilyName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FamilyName"]));
                        BizAction.AppointmentDetails.GenderId = (long)reader["GenderID"];
                        //BizAction.AppointmentDetails.DOB = (DateTime)reader["DateOfBirth"];
                        BizAction.AppointmentDetails.SpecialRegistrationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecialRegistrationID"]));
                        BizAction.AppointmentDetails.IsAge = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAge"]));
                        if (BizAction.AppointmentDetails.IsAge == true)
                        {
                            BizAction.AppointmentDetails.DateOfBirthFromAge = Convert.ToDateTime(DALHelper.HandleDate(reader["DOB"]));
                        }
                        else
                        {
                            BizAction.AppointmentDetails.DOB = Convert.ToDateTime(DALHelper.HandleDate(reader["DOB"]));
                        }
                        BizAction.AppointmentDetails.BloodId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BloodGroupID"]));
                        BizAction.AppointmentDetails.MaritalStatusId =  Convert.ToInt64(reader["MaritalStatusID"]);

                        BizAction.AppointmentDetails.CivilId = reader["CivilID"].ToString();
                        BizAction.AppointmentDetails.ContactNo1 = (string)DALHelper.HandleDBNull(reader["ContactNo1"]);
                        BizAction.AppointmentDetails.ContactNo2 = (string)DALHelper.HandleDBNull(reader["ContactNo2"]);

                        BizAction.AppointmentDetails.ResiNoCountryCode = (long)DALHelper.HandleDBNull(reader["ResiNoCountryCode"]);
                        BizAction.AppointmentDetails.ResiSTDCode = (long)DALHelper.HandleDBNull(reader["ResiSTDCode"]);
                        BizAction.AppointmentDetails.MobileCountryCode = Convert.ToString(DALHelper.HandleDBNull(reader["MobileCountryCode"]));
                        BizAction.AppointmentDetails.FaxNo = (string)DALHelper.HandleDBNull(reader["FaxNo"]);
                        BizAction.AppointmentDetails.Email = Security.base64Decode((string)DALHelper.HandleDBNull(reader["Email"]));
                        BizAction.AppointmentDetails.NationalityId = Convert.ToInt64(reader["NationalityID"]);
                        BizAction.AppointmentDetails.Gender = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"]));

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

            return BizAction;



        }

        public override IValueObject GetDoctorTime(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetDoctorTimeBizActionVO BizAction = (clsGetDoctorTimeBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorTime");
                dbServer.AddInParameter(command, "DoctorId", DbType.Int64, BizAction.DoctorId);
                //dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizAction.InputSearchExpression);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizAction.AppointmentDetailsList == null)
                        BizAction.AppointmentDetailsList = new List<clsAppointmentVO>();

                    while (reader.Read())
                    {
                        clsAppointmentVO AppointmentVO = new clsAppointmentVO();
                        AppointmentVO.Schedule1_StartTime = (string)DALHelper.HandleDBNull(reader["Schedule1_StartTime"]);
                        AppointmentVO.Schedule1_EndTime = (string)DALHelper.HandleDBNull(reader["Schedule1_EndTime"]);
                        AppointmentVO.Schedule2_StartTime = (string)DALHelper.HandleDBNull(reader["Schedule2_StartTime"]);
                        AppointmentVO.Schedule2_EndTime = (string)DALHelper.HandleDBNull(reader["Schedule2_EndTime"]);


                        BizAction.AppointmentDetailsList.Add(AppointmentVO);
                    }
                    reader.NextResult();
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
            return BizAction;
        }

        //public override IValueObject GetPatientPastVisit(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetPatientPastVisitBizActionVO BizAction = valueObject as clsGetPatientPastVisitBizActionVO;

        //    try
        //    {
        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientPastVisit");

        //        dbServer.AddInParameter(command, "LinkServer", DbType.String, BizAction.LinkServer);
        //        if (BizAction.LinkServer != null)
        //            dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, BizAction.LinkServer);

        //        dbServer.AddInParameter(command, "MRNo", DbType.String, BizAction.MRNO);
        //        DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

        //        if (reader.HasRows)
        //        {
        //            if (BizAction.AppointmentDetails == null)
        //                BizAction.AppointmentDetails = new List<clsAppointmentVO>();

        //            while (reader.Read())
        //            {
        //                clsAppointmentVO AppointmentVO = new clsAppointmentVO();
        //                AppointmentVO.AppointmentDate = (DateTime)reader["AppointmentDate"];
        //                AppointmentVO.FromTime = (DateTime)reader["FromTime"];
        //                AppointmentVO.DoctorName = (string)DALHelper.HandleDBNull(reader["DoctorName"]);

        //                BizAction.AppointmentDetails.Add(AppointmentVO);

        //            }
        //            //reader.NextResult();
        //            reader.Close();

        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        throw;
        //        //CurrentMethodExecutionStatus = false;
        //        //throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
        //    }
        //    finally
        //    {

        //    }
        //    return BizAction;
        //}

        public override IValueObject AddCancelAppReason(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsAddCancelAppReasonBizActionVO BizActionobj = valueObject as clsAddCancelAppReasonBizActionVO;

            try
            {
                clsAppointmentVO objAppointmentVO = BizActionobj.AppointmentDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddCancelAppointmentReason");
                dbServer.AddInParameter(command, "AppointmentId", DbType.Int64, objAppointmentVO.AppointmentID);
                dbServer.AddInParameter(command, "AppCancelReason", DbType.String, Security.base64Encode(objAppointmentVO.AppCancelReason.Trim()));
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objAppointmentVO.UnitId);
                int intStatus = dbServer.ExecuteNonQuery(command);


            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw;
                //throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {

            }
            return BizActionobj;

        }

        public override IValueObject GetAppointmentByDoctorAndAppointmentDate(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetAppointmentByDoctorAndAppointmentDateBizActionVO BizActionObj = (clsGetAppointmentByDoctorAndAppointmentDateBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAppointmentListByDoctorIDAndDate");

                dbServer.AddInParameter(command, "LinkServer", DbType.String, BizActionObj.LinkServer);
                if (BizActionObj.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, BizActionObj.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.DoctorId);
                dbServer.AddInParameter(command, "DepartmentId", DbType.Int64, BizActionObj.DepartmentId);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizActionObj.UnitId);
                dbServer.AddInParameter(command, "AppointmentDate", DbType.DateTime, BizActionObj.AppointmentDate);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.AppointmentDetailsList == null)
                        BizActionObj.AppointmentDetailsList = new List<clsAppointmentVO>();
                    while (reader.Read())
                    {
                        clsAppointmentVO AppointmentVO = new clsAppointmentVO();
                        AppointmentVO.DoctorId = (long)DALHelper.HandleDBNull(reader["DoctorID"]);
                        AppointmentVO.AppointmentDate = (DateTime?)DALHelper.HandleDate(reader["AppointmentDate"]);
                        AppointmentVO.FromTime = (DateTime)reader["FromTime"];
                        AppointmentVO.ToTime = (DateTime)reader["ToTime"];


                        BizActionObj.AppointmentDetailsList.Add(AppointmentVO);
                    }
                    reader.NextResult();
                }
                reader.Close();

            }

            catch
            {
                throw;
            }

            finally
            {
            }

            return BizActionObj;
        }

        public override IValueObject CheckMRNO(IValueObject valueObject, clsUserVO UserVo)
        {

            clsCheckMRnoBizActionVO BizActionObj = (clsCheckMRnoBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_CheckForMRNO");
                DbDataReader reader;
                dbServer.AddInParameter(command, "MRNO", DbType.String, BizActionObj.MRNO);
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

        public override IValueObject CheckAppointmentTime(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCheckAppointmentTimeBizActionVO BizActionObj = (clsCheckAppointmentTimeBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_CheckAppointmentTime");
                DbDataReader reader;
                dbServer.AddInParameter(command, "DoctorId", DbType.Int64, BizActionObj.DoctorId);
                dbServer.AddInParameter(command, "AppointmentDate", DbType.DateTime, BizActionObj.AppointmentDate);
                dbServer.AddInParameter(command, "FromTime", DbType.DateTime, BizActionObj.FromTime);
                dbServer.AddInParameter(command, "ToTime", DbType.DateTime, BizActionObj.ToTime);

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

        public override IValueObject AddMarkVisit(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddMarkVisitInAppointmenBizActionVO BizActionobj = valueObject as clsAddMarkVisitInAppointmenBizActionVO;

            try
            {
                clsAppointmentVO objAppointmentVO = BizActionobj.AppointmentDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_MarkVisitInAppointment");



                dbServer.AddInParameter(command, "AppointmentId", DbType.Int64, objAppointmentVO.AppointmentID);
                dbServer.AddInParameter(command, "VisitMark", DbType.Boolean, objAppointmentVO.VisitMark);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objAppointmentVO.UnitId);

                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objAppointmentVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                int intStatus = dbServer.ExecuteNonQuery(command);

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return BizActionobj;
        }

        public override IValueObject CheckMarkVisit(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCheckMarkVisitBizActionVO BizActionObj = (clsCheckMarkVisitBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_CheckVisitMark");
                DbDataReader reader;

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
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

        public override IValueObject GetPastAppointment(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPastAppointmentBizActionVO BizActionobj = valueObject as clsGetPastAppointmentBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPastAppointment");


                dbServer.AddInParameter(command, "MRNo", DbType.String, BizActionobj.MRNO);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionobj.UnitID);
                dbServer.AddInParameter(command, "PatientUnitId", DbType.Int64, BizActionobj.PatientUnitID);
                //dbServer.AddInParameter(command, "SpouseID", DbType.Int64, BizAction.SpouseID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    if (BizActionobj.AppointmentList == null)
                        BizActionobj.AppointmentList = new List<clsAppointmentVO>();
                    while (reader.Read())
                    {
                        clsAppointmentVO objAppointmentVO = new clsAppointmentVO();

                        //objVisitVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objAppointmentVO.PastAppointmentDate = (DateTime?)DALHelper.HandleDate(reader["AppointmentDate"]);
                        objAppointmentVO.PastAppointmentFromTime = (string)DALHelper.HandleDBNull(reader["AppointmentTime"]);
                        objAppointmentVO.DoctorId = (long)DALHelper.HandleIntegerNull(reader["DoctorID"]);
                        objAppointmentVO.DoctorName = (string)DALHelper.HandleDBNull(reader["DoctorName"]);
                        objAppointmentVO.DepartmentId = (long)DALHelper.HandleIntegerNull(reader["DepartmentID"]);
                        objAppointmentVO.Description = (string)DALHelper.HandleDBNull(reader["DepartmentName"]);

                        objAppointmentVO.UnitId = (long)DALHelper.HandleIntegerNull(reader["UnitID"]);

                        objAppointmentVO.UnitName = (string)DALHelper.HandleDBNull(reader["UnitName"]);
                        objAppointmentVO.AppointmentReason = (string)DALHelper.HandleDBNull(reader["AppointmentReason"]);
                        //objAppointmentVO.BalanceAmount = (double)DALHelper.HandleDBNull(reader["BalanceAmount"]);

                        BizActionobj.AppointmentList.Add(objAppointmentVO);
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

        public override IValueObject GetFutureAppointment(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetFutureAppointmentBizActionVO BizActionobj = valueObject as clsGetFutureAppointmentBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetFutureAppointment");


                dbServer.AddInParameter(command, "MRNo", DbType.String, BizActionobj.MRNO);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionobj.UnitID);
                dbServer.AddInParameter(command, "PatientUnitId", DbType.Int64, BizActionobj.PatientUnitID);
                //dbServer.AddInParameter(command, "SpouseID", DbType.Int64, BizAction.SpouseID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    if (BizActionobj.AppointmentList == null)
                        BizActionobj.AppointmentList = new List<clsAppointmentVO>();
                    while (reader.Read())
                    {
                        clsAppointmentVO objAppointmentVO = new clsAppointmentVO();

                        //objVisitVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objAppointmentVO.FutureAppointmentDate = (DateTime?)DALHelper.HandleDate(reader["AppointmentDate"]);
                        objAppointmentVO.FutureAppointmentFromTime = (string)DALHelper.HandleDBNull(reader["AppointmentTime"]);
                        objAppointmentVO.DoctorId = (long)DALHelper.HandleIntegerNull(reader["DoctorID"]);
                        objAppointmentVO.DoctorName = (string)DALHelper.HandleDBNull(reader["DoctorName"]);
                        objAppointmentVO.DepartmentId = (long)DALHelper.HandleIntegerNull(reader["DepartmentID"]);
                        objAppointmentVO.Description = (string)DALHelper.HandleDBNull(reader["DepartmentName"]);

                        objAppointmentVO.UnitId = (long)DALHelper.HandleIntegerNull(reader["UnitID"]);

                        objAppointmentVO.UnitName = (string)DALHelper.HandleDBNull(reader["UnitName"]);
                        objAppointmentVO.AppointmentReason = (string)DALHelper.HandleDBNull(reader["AppointmentReason"]);
                        // objAppointmentVO.BalanceAmount = (double)DALHelper.HandleDBNull(reader["BalanceAmount"]);

                        BizActionobj.AppointmentList.Add(objAppointmentVO);
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

    }

}







