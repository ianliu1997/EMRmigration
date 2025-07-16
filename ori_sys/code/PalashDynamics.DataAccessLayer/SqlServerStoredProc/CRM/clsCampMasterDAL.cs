using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.Web.Logging;
using System.Data;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.ValueObjects.CRM;
using System.Data.Common;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PalashDynamics.ValueObjects;

using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.CRM;
using PalashDynamics.ValueObjects.Patient;


namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsCampMasterDAL : clsBaseCampMasterDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsCampMasterDAL()
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

        public override IValueObject AddEmailSMSSentDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddEmailSMSSentListVO BizActionObj = (clsAddEmailSMSSentListVO)valueObject;
            try
            {
                clsEmailSMSSentListVO objSentVO = BizActionObj.EmailTemplate;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddEmailSMSSentDetails");

                dbServer.AddOutParameter(command, "Id", DbType.Int64, int.MaxValue);

                dbServer.AddInParameter(command, "PatientId", DbType.Int64, objSentVO.PatientId);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Email_SMS", DbType.Boolean, objSentVO.Email_SMS);
                dbServer.AddInParameter(command, "TemplateID", DbType.Boolean, objSentVO.TemplateID);
                dbServer.AddInParameter(command, "PatientEmailId", DbType.String, objSentVO.PatientEmailId);
                dbServer.AddInParameter(command, "EmailSubject", DbType.String, objSentVO.EmailSubject);
                dbServer.AddInParameter(command, "EmailText", DbType.String, objSentVO.EmailText);
                dbServer.AddInParameter(command, "EmailAttachment", DbType.String, objSentVO.EmailAttachment);
                dbServer.AddInParameter(command, "PatientMobileNo", DbType.String, objSentVO.PatientMobileNo);
                dbServer.AddInParameter(command, "EnglishText", DbType.String, objSentVO.EnglishText);
                dbServer.AddInParameter(command, "LocalText", DbType.String, objSentVO.LocalText);
                dbServer.AddInParameter(command, "SuccessStatus", DbType.Boolean, objSentVO.SuccessStatus);
                dbServer.AddInParameter(command, "FailureReason", DbType.String, objSentVO.FailureReason);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId); //objSentVO.CreatedUnitID);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID); //objSentVO.AddedBy);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName); //objSentVO.AddedOn.Trim());
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);  // UserVo.UserGeneralDetailVO.AddedDateTime); //objSentVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName); //objSentVO.AddedWindowsLoginName.Trim());
                //dbServer.AddParameter(command, "Id", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objSentVO.Id);
                int intStatus = dbServer.ExecuteNonQuery(command);
                // BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
               // BizActionObj.EmailTemplate.Id = (long)dbServer.GetParameterValue(command, "Id");
                BizActionObj.EmailTemplate.Id = (long)dbServer.GetParameterValue(command, "Id");
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
      
        public override IValueObject AddCampMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddCampMasterBizActionVO BizActionObj = (clsAddCampMasterBizActionVO)valueObject;
            try
            {
                clsCampMasterVO objCampMasterVO = BizActionObj.CampMasterDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddCampMaster");

                dbServer.AddInParameter(command, "Code", DbType.String, objCampMasterVO.Code.Trim());
                dbServer.AddInParameter(command, "Description ", DbType.String, objCampMasterVO.Description.Trim());
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime,DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                //dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objCampMasterVO.ID);
                int intStatus = dbServer.ExecuteNonQuery(command);
               // BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.CampMasterDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
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

        public override IValueObject GetCampMasterList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCampMasterListBizActionVO BizActionObj = (clsGetCampMasterListBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCampMasterList");

                if (BizActionObj.Description != null && BizActionObj.Description.Length != 0)
                    dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.Description + "%");
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizActionObj.SearchExpression);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.CampMasterList == null)
                        BizActionObj.CampMasterList = new List<clsCampMasterVO>();
                    while (reader.Read())
                    {
                        clsCampMasterVO objCampMasterVO = new clsCampMasterVO();
                        objCampMasterVO.ID = (long)reader["ID"];
                        objCampMasterVO.UnitID = (long)reader["UnitID"];
                        objCampMasterVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        objCampMasterVO.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        objCampMasterVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);

                        BizActionObj.CampMasterList.Add(objCampMasterVO);
                    }   
                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
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

        public override IValueObject GetCampMasterByID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCampMasterByIDBizActionVO BizAction =  (clsGetCampMasterByIDBizActionVO)valueObject;
            try
            {             
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCampMasterByID");
                DbDataReader reader;
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.ID);            
                
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (BizAction.CampMasterDetails == null)
                            BizAction.CampMasterDetails = new clsCampMasterVO();
                        BizAction.CampMasterDetails.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        BizAction.CampMasterDetails.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        BizAction.CampMasterDetails.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        BizAction.CampMasterDetails.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        BizAction.CampMasterDetails.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
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

        public override IValueObject UpdateCampMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateCampMasterBizActionVO BizActionObj = (clsUpdateCampMasterBizActionVO)valueObject;
            try
            {
                clsCampMasterVO objCampMasterVO = BizActionObj.CampMasterDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateCampMaster");

                dbServer.AddInParameter(command, "ID", DbType.Int64, objCampMasterVO.ID);
                dbServer.AddInParameter(command, "Code", DbType.String, objCampMasterVO.Code.Trim());
                dbServer.AddInParameter(command, "Description ", DbType.String, objCampMasterVO.Description.Trim());
               
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                //dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionobj.SuccessStatus);

                int intStatus = dbServer.ExecuteNonQuery(command);
                // BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
               // BizActionobj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
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

        public override IValueObject AddCampDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction trans=null;
            DbConnection con=null;
            clsAddCampDetailsBizActionVO BizActionObj = (clsAddCampDetailsBizActionVO)valueObject;

            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                clsCampMasterVO objCampMasterVO = BizActionObj.CampMasterDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddCampDetails");
                dbServer.AddInParameter(command, "CampID", DbType.Int64, objCampMasterVO.CampID);
                dbServer.AddInParameter(command, "FromDate ", DbType.DateTime, objCampMasterVO.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, objCampMasterVO.ToDate);
                dbServer.AddInParameter(command, "City", DbType.String, objCampMasterVO.City.Trim());
                dbServer.AddInParameter(command, "Area", DbType.String, objCampMasterVO.Area.Trim());
                dbServer.AddInParameter(command, "ValidDays", DbType.Double, objCampMasterVO.ValidDays);
                dbServer.AddInParameter(command, "PatientRegistrationValidTillDate ", DbType.DateTime, objCampMasterVO.PatientRegistrationValidTillDate);
                dbServer.AddInParameter(command, "Concession", DbType.Double, objCampMasterVO.Concession);
                dbServer.AddInParameter(command, "TariffID", DbType.Int64, objCampMasterVO.TariffID);
                dbServer.AddInParameter(command, "Reason", DbType.String, objCampMasterVO.Reason.Trim());
                dbServer.AddInParameter(command, "EmailTemplateID", DbType.Int64, objCampMasterVO.EmailTemplateID);
                dbServer.AddInParameter(command, "SmsTemplateID", DbType.Int64, objCampMasterVO.SmsTemplateID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objCampMasterVO.CampDetailID);
                int intStatus = dbServer.ExecuteNonQuery(command,trans);
                BizActionObj.CampMasterDetails.CampDetailID = (long)dbServer.GetParameterValue(command, "ID");
                //For Add records in T_CampService Table.
                foreach (var Obj in objCampMasterVO.FreeCampServiceList)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddCampService");

                    dbServer.AddInParameter(command1, "CampDetailID", DbType.Int64, objCampMasterVO.CampDetailID);
                    dbServer.AddInParameter(command1, "ServiceID", DbType.Int64, Obj.ServiceID);
                    dbServer.AddInParameter(command1, "IsFree", DbType.Boolean, Obj.IsFree);
                    dbServer.AddInParameter(command1, "IsConcession", DbType.Boolean, Obj.IsConcession);
                    dbServer.AddInParameter(command1, "Rate", DbType.Double, Obj.Rate);
                    dbServer.AddInParameter(command1, "ConcessionPercentage", DbType.Double, Obj.ConcessionPercentage);
                    dbServer.AddInParameter(command1, "ConcessionAmount", DbType.Double, Obj.ConcessionAmount);
                    dbServer.AddInParameter(command1, "NetAmount", DbType.Double, Obj.NetAmount);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, Obj.CampServiceID);

                    int iStatus = dbServer.ExecuteNonQuery(command1,trans);
                    Obj.CampServiceID = (long)dbServer.GetParameterValue(command1, "ID");
                }
                foreach (var Obj in objCampMasterVO.ConcessionServiceList)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddCampService");

                    dbServer.AddInParameter(command1, "CampDetailID", DbType.Int64, objCampMasterVO.CampDetailID);
                    dbServer.AddInParameter(command1, "ServiceID", DbType.Int64, Obj.ServiceID);

                    dbServer.AddInParameter(command1, "IsFree", DbType.Boolean, Obj.IsFree);
                    dbServer.AddInParameter(command1, "IsConcession", DbType.Boolean, Obj.IsConcession);
                    dbServer.AddInParameter(command1, "Rate", DbType.Double, Obj.Rate);
                    dbServer.AddInParameter(command1, "ConcessionPercentage", DbType.Double, Obj.ConcessionPercentage);
                    dbServer.AddInParameter(command1, "ConcessionAmount", DbType.Double, Obj.ConcessionAmount);
                    dbServer.AddInParameter(command1, "NetAmount", DbType.Double, Obj.NetAmount);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, Obj.CampServiceID);

                    int iStatus = dbServer.ExecuteNonQuery(command1,trans);
                    Obj.CampServiceID = (long)dbServer.GetParameterValue(command1, "ID");
                }
                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                //throw;
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.CampMasterDetails = null;
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }
            return BizActionObj;            
        }

        public override IValueObject GetCampDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCampDetailsListBizActionVO BizActionObj = (clsGetCampDetailsListBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCampDetailsList");
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "CampDetailID");
                dbServer.AddInParameter(command, "Camp", DbType.String, BizActionObj.Camp);
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                 DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.CampDetailsList == null)
                        BizActionObj.CampDetailsList = new List<clsCampMasterVO>();
                    while (reader.Read())
                    {
                           clsCampMasterVO objCampMasterVO = new clsCampMasterVO();
                           objCampMasterVO.CampDetailID = (long)reader["CampDetailID"];
                          objCampMasterVO.UnitID = (long)reader["UnitID"];
                           objCampMasterVO.FromDate=(DateTime)DALHelper.HandleDate(reader["FromDate"]);
                           objCampMasterVO.ToDate=(DateTime)DALHelper.HandleDate(reader["ToDate"]);
                           objCampMasterVO.City = (string)DALHelper.HandleDBNull(reader["City"]);
                           objCampMasterVO.Area = (string)DALHelper.HandleDBNull(reader["Area"]);
                           objCampMasterVO.ValidDays = (double)DALHelper.HandleDBNull(reader["ValidDays"]);
                           objCampMasterVO.Reason = (string)DALHelper.HandleDBNull(reader["Reason"]);
                           objCampMasterVO.Description = (string)DALHelper.HandleDBNull(reader["Camp"]);
                           objCampMasterVO.CampID = (long)DALHelper.HandleDBNull(reader["CampID"]);
                           objCampMasterVO.TariffID = (long)reader["TariffID"];
                           objCampMasterVO.Tariff= (string)DALHelper.HandleDBNull(reader["Tariff"]);
                           objCampMasterVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
     
                           BizActionObj.CampDetailsList.Add(objCampMasterVO);
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
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
            return BizActionObj;
            }

        public override IValueObject GetCampDetailsByID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCampDetailsByIDBizActionVO BizAction = (clsGetCampDetailsByIDBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCampDetailsByID");
                DbDataReader reader;
                dbServer.AddInParameter(command, "CampID", DbType.Int64, BizAction.ID);
         
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (BizAction.CampMasterDetails == null)
                            BizAction.CampMasterDetails = new clsCampMasterVO();

                        BizAction.CampMasterDetails.CampDetailID = (long)reader["CampDetailID"];
                        BizAction.CampMasterDetails.UnitID = (long)reader["UnitID"];
                        BizAction.CampMasterDetails.FromDate = (DateTime)DALHelper.HandleDate(reader["FromDate"]);
                        BizAction.CampMasterDetails.ToDate = (DateTime)DALHelper.HandleDate(reader["ToDate"]);
                        BizAction.CampMasterDetails.City = (string)DALHelper.HandleDBNull(reader["City"]);
                        BizAction.CampMasterDetails.Area = (string)DALHelper.HandleDBNull(reader["Area"]);
                        BizAction.CampMasterDetails.ValidDays = (double)DALHelper.HandleDBNull(reader["ValidDays"]);
                        BizAction.CampMasterDetails.Concession = reader["Concession"].HandleDBNull() == null ? 0 : Convert.ToDouble(reader["Concession"]);
                            //(double)DALHelper.HandleDBNull(reader["Concession"]);
                        BizAction.CampMasterDetails.Reason = (string)DALHelper.HandleDBNull(reader["Reason"]);
                        BizAction.CampMasterDetails.Description = (string)DALHelper.HandleDBNull(reader["Camp"]);
                        BizAction.CampMasterDetails.CampID = (long)DALHelper.HandleDBNull(reader["CampID"]);
                        BizAction.CampMasterDetails.TariffID = (long)reader["TariffID"];
                        BizAction.CampMasterDetails.Tariff = (string)DALHelper.HandleDBNull(reader["Tariff"]);
                        BizAction.CampMasterDetails.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        BizAction.CampMasterDetails.EmailTemplateID = (long)DALHelper.HandleDBNull(reader["EmailTemplateID"]);
                        BizAction.CampMasterDetails.EmailTemplateDetails.ID = (long)DALHelper.HandleDBNull(reader["EmailTemplateID"]);
                        BizAction.CampMasterDetails.EmailTemplateDetails.Description = Convert.ToString(DALHelper.HandleDBNull(reader["EmailDescription"]));
                       // BizAction.CampMasterDetails.EmailTemplateDetails.AttachmentDetails.AttachmentFileName = (string)DALHelper.HandleDBNull(reader["AttachmentFileName"]);
                        //BizAction.CampMasterDetails.EmailTemplateDetails.AttachmentDetails.Attachment = (byte[])DALHelper.HandleDBNull(reader["Attachment"]);
                        BizAction.CampMasterDetails.SMSTemplateDetails.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        BizAction.CampMasterDetails.SmsTemplateID = (long)DALHelper.HandleDBNull(reader["SmsTemplateID"]);
                        BizAction.CampMasterDetails.SMSTemplateDetails.ID = (long)DALHelper.HandleDBNull(reader["SmsTemplateID"]);

                        BizAction.CampMasterDetails.PatientRegistrationValidTillDate =(DateTime?)(DALHelper.HandleDBNull(reader["PatientRegistrationValidTillDate"]));
                        //BizAction.CampMasterDetails.SMSTemplateDetails.Description=(string)DALHelper.HandleDBNull(reader["Description
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

        public override IValueObject UpdateCampDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;
            clsUpdateCampDetailsBizActionVO BizActionObj = (clsUpdateCampDetailsBizActionVO)valueObject;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();
                
                clsCampMasterVO objCampMasterVO = BizActionObj.CampMasterDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateCampDetails");

                dbServer.AddInParameter(command, "ID", DbType.Int64, objCampMasterVO.CampDetailID);
                dbServer.AddInParameter(command, "CampID", DbType.Int64, objCampMasterVO.CampID);
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, objCampMasterVO.FromDate);
                dbServer.AddInParameter(command, "PatientRegistrationValidTillDate", DbType.DateTime, objCampMasterVO.PatientRegistrationValidTillDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, objCampMasterVO.ToDate);
                dbServer.AddInParameter(command, "City ", DbType.String, objCampMasterVO.City.Trim());
                dbServer.AddInParameter(command, "Area ", DbType.String, objCampMasterVO.Area.Trim());
                dbServer.AddInParameter(command, "ValidDays", DbType.Double, objCampMasterVO.ValidDays);
                dbServer.AddInParameter(command, "Concession", DbType.Double, objCampMasterVO.Concession);
                dbServer.AddInParameter(command, "TariffID", DbType.Int64, objCampMasterVO.TariffID);
                dbServer.AddInParameter(command, "Reason", DbType.String, objCampMasterVO.Reason.Trim());
                dbServer.AddInParameter(command, "EmailTemplateID", DbType.Int64, objCampMasterVO.EmailTemplateID);
                dbServer.AddInParameter(command, "SmsTemplateID", DbType.Int64, objCampMasterVO.SmsTemplateID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                
                int intStatus = dbServer.ExecuteNonQuery(command,trans);

                if (objCampMasterVO.FreeCampServiceList != null && objCampMasterVO.FreeCampServiceList.Count > 0 || objCampMasterVO.ConcessionServiceList != null && objCampMasterVO.ConcessionServiceList.Count > 0)
                {
                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_DeleteCampService");

                    dbServer.AddInParameter(command2, "CampID", DbType.Int64, objCampMasterVO.CampDetailID);
                    int intStatus2 = dbServer.ExecuteNonQuery(command2,trans);
                }
                //For Add records in T_CampService Table.
                foreach (var Obj in objCampMasterVO.FreeCampServiceList)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddCampService");

                    dbServer.AddInParameter(command1, "CampDetailID", DbType.Int64, objCampMasterVO.CampDetailID);
                    dbServer.AddInParameter(command1, "ServiceID", DbType.Int64, Obj.ServiceID);
                    dbServer.AddInParameter(command1, "IsFree", DbType.Boolean, Obj.IsFree);
                    dbServer.AddInParameter(command1, "IsConcession", DbType.Boolean, Obj.IsConcession);
                    dbServer.AddInParameter(command1, "Rate", DbType.Double, Obj.Rate);
                    dbServer.AddInParameter(command1, "ConcessionPercentage", DbType.Double, Obj.ConcessionPercentage);
                    dbServer.AddInParameter(command1, "ConcessionAmount", DbType.Double, Obj.ConcessionAmount);
                    dbServer.AddInParameter(command1, "NetAmount", DbType.Double, Obj.NetAmount);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, Obj.CampServiceID);

                    int iStatus = dbServer.ExecuteNonQuery(command1,trans);
                    Obj.CampServiceID = (long)dbServer.GetParameterValue(command1, "ID");
                }
                foreach (var Obj in objCampMasterVO.ConcessionServiceList)
                {
                    DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_AddCampService");
                    dbServer.AddInParameter(command3, "CampDetailID", DbType.Int64, objCampMasterVO.CampDetailID);
                    dbServer.AddInParameter(command3, "ServiceID", DbType.Int64, Obj.ServiceID);
                    dbServer.AddInParameter(command3, "IsFree", DbType.Boolean, Obj.IsFree);
                    dbServer.AddInParameter(command3, "IsConcession", DbType.Boolean, Obj.IsConcession);
                    dbServer.AddInParameter(command3, "Rate", DbType.Double, Obj.Rate);
                    dbServer.AddInParameter(command3, "ConcessionPercentage", DbType.Double, Obj.ConcessionPercentage);
                    dbServer.AddInParameter(command3, "ConcessionAmount", DbType.Double, Obj.ConcessionAmount);
                    dbServer.AddInParameter(command3, "NetAmount", DbType.Double, Obj.NetAmount);
                    dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command3, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, Obj.CampServiceID);

                    int iStatus = dbServer.ExecuteNonQuery(command3,trans);
                    Obj.CampServiceID = (long)dbServer.GetParameterValue(command3, "ID");
                }
                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                //throw;
                BizActionObj.SuccessStatus = 0;
                trans.Rollback();
                BizActionObj.CampMasterDetails = null;
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }
            return BizActionObj;
        }

        public override IValueObject GetCampFreeAndConssService(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCampFreeAndConServiceListBizActionVO BizActionObj = (clsGetCampFreeAndConServiceListBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetFreeAndConcessionCampService");
                dbServer.AddInParameter(command, "CampID", DbType.Int64, BizActionObj.CampID);

                 DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                 if (reader.HasRows)
                 {
                     if (BizActionObj.CampFreeServiceList == null)
                         BizActionObj.CampFreeServiceList = new List<CampserviceDetailsVO>();
                     while (reader.Read())
                     {
                         CampserviceDetailsVO objCampMasterVO = new CampserviceDetailsVO();
                         objCampMasterVO.CampDetailsID = (long)reader["CampDetailID"];
                         objCampMasterVO.ServiceCode = (string)reader["ServiceCode"];
                         objCampMasterVO.ServiceID = (long)reader["ServiceID"];
                         objCampMasterVO.ServiceName = (string)reader["ServiceName"];
                         objCampMasterVO.IsFree = (bool)reader["IsFree"];
                         objCampMasterVO.IsConcession = (bool)reader["IsConcession"];

                         BizActionObj.CampFreeServiceList.Add(objCampMasterVO);
                     }                
                 }
                     reader.NextResult();
                     if (reader.HasRows)
                     {
                         if (BizActionObj.CampConServiceList == null)
                             BizActionObj.CampConServiceList = new List<CampserviceDetailsVO>();
                         while (reader.Read())
                         {
                             CampserviceDetailsVO objCampMasterVO = new CampserviceDetailsVO();
                             objCampMasterVO.CampDetailsID = (long)reader["CampDetailID"];
                             objCampMasterVO.ServiceCode = (string)DALHelper.HandleDBNull(reader["ServiceCode"]);
                             objCampMasterVO.ServiceID = (long)reader["ServiceID"];
                             objCampMasterVO.ServiceName = (string)reader["ServiceName"];
                             objCampMasterVO.Rate = (double)reader["Rate"];
                             objCampMasterVO.ConcessionPercentage = (double)reader["ConcessionPercentage"];
                             objCampMasterVO.ConcessionAmount = (double)reader["ConcessionAmount"];
                             objCampMasterVO.NetAmount = (double)reader["NetAmount"];
                             objCampMasterVO.IsFree = (bool)reader["IsFree"];
                             objCampMasterVO.IsConcession = (bool)reader["IsConcession"];

                             BizActionObj.CampConServiceList.Add(objCampMasterVO);
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

        public override IValueObject AddCampService(IValueObject valueObject, clsUserVO UserVo)
        {
            //clsAddCampServiceBizActionVO BizAction = (clsAddCampServiceBizActionVO)valueObject;
            //try
            //{
            //    clsCampMasterVO objCampServiceVO = BizAction.CampService;
            //    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddCampService");
            //    dbServer.AddInParameter(command, "CampDetailID", DbType.Int64, objCampServiceVO.CampDetailID);
            //    dbServer.AddInParameter(command, "ServiceID", DbType.Int64, objCampServiceVO.ServiceID);
            //    dbServer.AddInParameter(command, "IsFree", DbType.Boolean, objCampServiceVO.IsFree);
            //    dbServer.AddInParameter(command, "IsConcession", DbType.Boolean, objCampServiceVO.IsConcession);
            //    dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
            //    dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            //    dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            //    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
            //    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
            //    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
            //    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
            //    //dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
            //    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objCampServiceVO.CampServiceID);
            //    int intStatus = dbServer.ExecuteNonQuery(command);
            //    //BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
            //    BizAction.CampService.CampServiceID = (long)dbServer.GetParameterValue(command, "ID");
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
            //finally
            //{
            //}
            //return BizAction;

            return valueObject;
        }

        public override IValueObject DeleteCampService(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteCampServiceBizActionVO BizActionobj = valueObject as clsDeleteCampServiceBizActionVO;

            try
            {
                clsCampMasterVO objCampMasterVO = BizActionobj.CampMasterDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_DeleteCampService");

                dbServer.AddInParameter(command, "CampID", DbType.Int64, objCampMasterVO.CampDetailID);
                int intStatus = dbServer.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
            return BizActionobj;
        }

        public override IValueObject GetPROPatientList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPROPatientListBizActionVO BizActionObj = valueObject as clsGetPROPatientListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetProPatientList");
               
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.VisitFromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.VisitToDate);
                dbServer.AddInParameter(command, "ReferredDoctorID", DbType.Int64, BizActionObj.ReferredDoctorId);              
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.InputPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.InputStartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.InputMaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.SortExpression);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.PatientList == null)
                        BizActionObj.PatientList = new List<ValueObjects.Patient.clsPatientGeneralVO>();
                    
                    while (reader.Read())
                    {
                        clsPatientGeneralVO obj = new clsPatientGeneralVO();
                        obj.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);                         
                        obj.Gender = (string)DALHelper.HandleDBNull(reader["Description"]);
                        obj.FirstName = (string)DALHelper.HandleDBNull(reader["FirstName"]);
                        obj.MiddleName = (string)DALHelper.HandleDBNull(reader["MiddleName"]);
                        obj.LastName = (string)DALHelper.HandleDBNull(reader["LastName"]);
                        obj.RegistrationDate = (DateTime?)DALHelper.HandleDBNull(reader["RegistrationDate"]);
                        obj.ContactNO1 = (string)DALHelper.HandleDBNull(reader["ContactNo1"]);
                        obj.DateOfBirth = (DateTime?)DALHelper.HandleDate(reader["DateOfBirth"]);
                        obj.PRORemark = (string)DALHelper.HandleDBNull(reader["Remark"]);
                        obj.PatientID = (long)DALHelper.HandleDBNull(reader["PatientId"]);
                        obj.UnitId = (long)DALHelper.HandleDBNull(reader["PatientUnitId"]);
                        BizActionObj.PatientList.Add(obj);
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

        public override IValueObject AddPROPatient(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPROPatientBizActionVO BizActionObj = valueObject as clsAddPROPatientBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
            
                con.Open();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdatePROPatient");
                 
                dbServer.AddInParameter(command,"PatientId",DbType.Int64,BizActionObj.GeneralDetails.PatientID);
                dbServer.AddInParameter(command ,"PatientUnitId", DbType.Int64,BizActionObj.GeneralDetails.UnitId);
                dbServer.AddInParameter(command,"ReferredDoctorId", DbType.Int64,BizActionObj.ReferredDoctorId);
                dbServer.AddInParameter(command,"Remark" , DbType.String,BizActionObj.GeneralDetails.PRORemark);
                dbServer.AddInParameter(command,"Date", DbType.DateTime,BizActionObj.Date);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, BizActionObj.Status);
              
                if(BizActionObj.GeneralDetails.PatientPROID>0)
                {
                    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, BizActionObj.UpdatedUnitId);
                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, BizActionObj.UpdatedBy);
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, BizActionObj.UpdatedOn);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, BizActionObj.UpdatedDateTime);
                    dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.GeneralDetails.PatientPROID);
                }
                else
                {
                    dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, BizActionObj.CreatedUnitId);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, BizActionObj.AddedBy);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, BizActionObj.AddedOn);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, BizActionObj.AddedDateTime);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, BizActionObj.WindowsUserName);
                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.GeneralDetails.PatientPROID);
                    //dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.GeneralDetails.PatientPROID);
                }
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);   
                 int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.GeneralDetails.PatientPROID = (long)dbServer.GetParameterValue(command, "ID");
            }
            catch (Exception )
            {
                throw;
            }
            finally
            {
                con.Close();
            }

            return BizActionObj;


        }
        }

    }

