namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.CRM;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.CRM;
    using PalashDynamics.ValueObjects.Patient;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsCampMasterDAL : clsBaseCampMasterDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsCampMasterDAL()
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

        public override IValueObject AddCampDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            clsAddCampDetailsBizActionVO nvo = (clsAddCampDetailsBizActionVO) valueObject;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsCampMasterVO campMasterDetails = nvo.CampMasterDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddCampDetails");
                this.dbServer.AddInParameter(storedProcCommand, "CampID", DbType.Int64, campMasterDetails.CampID);
                this.dbServer.AddInParameter(storedProcCommand, "FromDate ", DbType.DateTime, campMasterDetails.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, campMasterDetails.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "City", DbType.String, campMasterDetails.City.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Area", DbType.String, campMasterDetails.Area.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "ValidDays", DbType.Double, campMasterDetails.ValidDays);
                this.dbServer.AddInParameter(storedProcCommand, "PatientRegistrationValidTillDate ", DbType.DateTime, campMasterDetails.PatientRegistrationValidTillDate);
                this.dbServer.AddInParameter(storedProcCommand, "Concession", DbType.Double, campMasterDetails.Concession);
                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, campMasterDetails.TariffID);
                this.dbServer.AddInParameter(storedProcCommand, "Reason", DbType.String, campMasterDetails.Reason.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "EmailTemplateID", DbType.Int64, campMasterDetails.EmailTemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "SmsTemplateID", DbType.Int64, campMasterDetails.SmsTemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, campMasterDetails.CampDetailID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.CampMasterDetails.CampDetailID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                foreach (CampserviceDetailsVO svo in campMasterDetails.FreeCampServiceList)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddCampService");
                    this.dbServer.AddInParameter(command2, "CampDetailID", DbType.Int64, campMasterDetails.CampDetailID);
                    this.dbServer.AddInParameter(command2, "ServiceID", DbType.Int64, svo.ServiceID);
                    this.dbServer.AddInParameter(command2, "IsFree", DbType.Boolean, svo.IsFree);
                    this.dbServer.AddInParameter(command2, "IsConcession", DbType.Boolean, svo.IsConcession);
                    this.dbServer.AddInParameter(command2, "Rate", DbType.Double, svo.Rate);
                    this.dbServer.AddInParameter(command2, "ConcessionPercentage", DbType.Double, svo.ConcessionPercentage);
                    this.dbServer.AddInParameter(command2, "ConcessionAmount", DbType.Double, svo.ConcessionAmount);
                    this.dbServer.AddInParameter(command2, "NetAmount", DbType.Double, svo.NetAmount);
                    this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.CampServiceID);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    svo.CampServiceID = (long) this.dbServer.GetParameterValue(command2, "ID");
                }
                foreach (CampserviceDetailsVO svo2 in campMasterDetails.ConcessionServiceList)
                {
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddCampService");
                    this.dbServer.AddInParameter(command3, "CampDetailID", DbType.Int64, campMasterDetails.CampDetailID);
                    this.dbServer.AddInParameter(command3, "ServiceID", DbType.Int64, svo2.ServiceID);
                    this.dbServer.AddInParameter(command3, "IsFree", DbType.Boolean, svo2.IsFree);
                    this.dbServer.AddInParameter(command3, "IsConcession", DbType.Boolean, svo2.IsConcession);
                    this.dbServer.AddInParameter(command3, "Rate", DbType.Double, svo2.Rate);
                    this.dbServer.AddInParameter(command3, "ConcessionPercentage", DbType.Double, svo2.ConcessionPercentage);
                    this.dbServer.AddInParameter(command3, "ConcessionAmount", DbType.Double, svo2.ConcessionAmount);
                    this.dbServer.AddInParameter(command3, "NetAmount", DbType.Double, svo2.NetAmount);
                    this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command3, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo2.CampServiceID);
                    this.dbServer.ExecuteNonQuery(command3, transaction);
                    svo2.CampServiceID = (long) this.dbServer.GetParameterValue(command3, "ID");
                }
                transaction.Commit();
                nvo.SuccessStatus = 0;
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                transaction.Rollback();
                nvo.CampMasterDetails = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject AddCampMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddCampMasterBizActionVO nvo = (clsAddCampMasterBizActionVO) valueObject;
            try
            {
                clsCampMasterVO campMasterDetails = nvo.CampMasterDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddCampMaster");
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, campMasterDetails.Code.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Description ", DbType.String, campMasterDetails.Description.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, campMasterDetails.ID);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.CampMasterDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddCampService(IValueObject valueObject, clsUserVO UserVo)
        {
            return valueObject;
        }

        public override IValueObject AddEmailSMSSentDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddEmailSMSSentListVO tvo = (clsAddEmailSMSSentListVO) valueObject;
            try
            {
                clsEmailSMSSentListVO emailTemplate = tvo.EmailTemplate;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddEmailSMSSentDetails");
                this.dbServer.AddOutParameter(storedProcCommand, "Id", DbType.Int64, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "PatientId", DbType.Int64, emailTemplate.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Email_SMS", DbType.Boolean, emailTemplate.Email_SMS);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Boolean, emailTemplate.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientEmailId", DbType.String, emailTemplate.PatientEmailId);
                this.dbServer.AddInParameter(storedProcCommand, "EmailSubject", DbType.String, emailTemplate.EmailSubject);
                this.dbServer.AddInParameter(storedProcCommand, "EmailText", DbType.String, emailTemplate.EmailText);
                this.dbServer.AddInParameter(storedProcCommand, "EmailAttachment", DbType.String, emailTemplate.EmailAttachment);
                this.dbServer.AddInParameter(storedProcCommand, "PatientMobileNo", DbType.String, emailTemplate.PatientMobileNo);
                this.dbServer.AddInParameter(storedProcCommand, "EnglishText", DbType.String, emailTemplate.EnglishText);
                this.dbServer.AddInParameter(storedProcCommand, "LocalText", DbType.String, emailTemplate.LocalText);
                this.dbServer.AddInParameter(storedProcCommand, "SuccessStatus", DbType.Boolean, emailTemplate.SuccessStatus);
                this.dbServer.AddInParameter(storedProcCommand, "FailureReason", DbType.String, emailTemplate.FailureReason);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                tvo.EmailTemplate.Id = (long) this.dbServer.GetParameterValue(storedProcCommand, "Id");
            }
            catch (Exception)
            {
                throw;
            }
            return tvo;
        }

        public override IValueObject AddPROPatient(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPROPatientBizActionVO nvo = valueObject as clsAddPROPatientBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePROPatient");
                this.dbServer.AddInParameter(storedProcCommand, "PatientId", DbType.Int64, nvo.GeneralDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitId", DbType.Int64, nvo.GeneralDetails.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ReferredDoctorId", DbType.Int64, nvo.ReferredDoctorId);
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, nvo.GeneralDetails.PRORemark);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.Status);
                if (nvo.GeneralDetails.PatientPROID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, nvo.UpdatedUnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, nvo.UpdatedBy);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, nvo.UpdatedOn);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, nvo.UpdatedDateTime);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.GeneralDetails.PatientPROID);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, nvo.CreatedUnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, nvo.AddedBy);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, nvo.AddedOn);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, nvo.AddedDateTime);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, nvo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.GeneralDetails.PatientPROID);
                }
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.GeneralDetails.PatientPROID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject DeleteCampService(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteCampServiceBizActionVO nvo = valueObject as clsDeleteCampServiceBizActionVO;
            try
            {
                clsCampMasterVO campMasterDetails = nvo.CampMasterDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteCampService");
                this.dbServer.AddInParameter(storedProcCommand, "CampID", DbType.Int64, campMasterDetails.CampDetailID);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject GetCampDetailsByID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCampDetailsByIDBizActionVO nvo = (clsGetCampDetailsByIDBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCampDetailsByID");
                this.dbServer.AddInParameter(storedProcCommand, "CampID", DbType.Int64, nvo.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (nvo.CampMasterDetails == null)
                        {
                            nvo.CampMasterDetails = new clsCampMasterVO();
                        }
                        nvo.CampMasterDetails.CampDetailID = (long) reader["CampDetailID"];
                        nvo.CampMasterDetails.UnitID = (long) reader["UnitID"];
                        nvo.CampMasterDetails.FromDate = new DateTime?(DALHelper.HandleDate(reader["FromDate"]).Value);
                        nvo.CampMasterDetails.ToDate = new DateTime?(DALHelper.HandleDate(reader["ToDate"]).Value);
                        nvo.CampMasterDetails.City = (string) DALHelper.HandleDBNull(reader["City"]);
                        nvo.CampMasterDetails.Area = (string) DALHelper.HandleDBNull(reader["Area"]);
                        nvo.CampMasterDetails.ValidDays = new double?((double) DALHelper.HandleDBNull(reader["ValidDays"]));
                        nvo.CampMasterDetails.Concession = new double?((reader["Concession"].HandleDBNull() == null) ? 0.0 : Convert.ToDouble(reader["Concession"]));
                        nvo.CampMasterDetails.Reason = (string) DALHelper.HandleDBNull(reader["Reason"]);
                        nvo.CampMasterDetails.Description = (string) DALHelper.HandleDBNull(reader["Camp"]);
                        nvo.CampMasterDetails.CampID = (long) DALHelper.HandleDBNull(reader["CampID"]);
                        nvo.CampMasterDetails.TariffID = (long) reader["TariffID"];
                        nvo.CampMasterDetails.Tariff = (string) DALHelper.HandleDBNull(reader["Tariff"]);
                        nvo.CampMasterDetails.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                        nvo.CampMasterDetails.EmailTemplateID = (long) DALHelper.HandleDBNull(reader["EmailTemplateID"]);
                        nvo.CampMasterDetails.EmailTemplateDetails.ID = (long) DALHelper.HandleDBNull(reader["EmailTemplateID"]);
                        nvo.CampMasterDetails.EmailTemplateDetails.Description = Convert.ToString(DALHelper.HandleDBNull(reader["EmailDescription"]));
                        nvo.CampMasterDetails.SMSTemplateDetails.Description = (string) DALHelper.HandleDBNull(reader["Description"]);
                        nvo.CampMasterDetails.SmsTemplateID = (long) DALHelper.HandleDBNull(reader["SmsTemplateID"]);
                        nvo.CampMasterDetails.SMSTemplateDetails.ID = (long) DALHelper.HandleDBNull(reader["SmsTemplateID"]);
                        nvo.CampMasterDetails.PatientRegistrationValidTillDate = (DateTime?) DALHelper.HandleDBNull(reader["PatientRegistrationValidTillDate"]);
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

        public override IValueObject GetCampDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCampDetailsListBizActionVO nvo = (clsGetCampDetailsListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCampDetailsList");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "CampDetailID");
                this.dbServer.AddInParameter(storedProcCommand, "Camp", DbType.String, nvo.Camp);
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.CampDetailsList == null)
                    {
                        nvo.CampDetailsList = new List<clsCampMasterVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                            reader.Close();
                            break;
                        }
                        clsCampMasterVO item = new clsCampMasterVO {
                            CampDetailID = (long) reader["CampDetailID"],
                            UnitID = (long) reader["UnitID"]
                        };
                        DateTime? nullable = DALHelper.HandleDate(reader["FromDate"]);
                        item.FromDate = new DateTime?(nullable.Value);
                        DateTime? nullable2 = DALHelper.HandleDate(reader["ToDate"]);
                        item.ToDate = new DateTime?(nullable2.Value);
                        item.City = (string) DALHelper.HandleDBNull(reader["City"]);
                        item.Area = (string) DALHelper.HandleDBNull(reader["Area"]);
                        item.ValidDays = new double?((double) DALHelper.HandleDBNull(reader["ValidDays"]));
                        item.Reason = (string) DALHelper.HandleDBNull(reader["Reason"]);
                        item.Description = (string) DALHelper.HandleDBNull(reader["Camp"]);
                        item.CampID = (long) DALHelper.HandleDBNull(reader["CampID"]);
                        item.TariffID = (long) reader["TariffID"];
                        item.Tariff = (string) DALHelper.HandleDBNull(reader["Tariff"]);
                        item.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                        nvo.CampDetailsList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetCampFreeAndConssService(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCampFreeAndConServiceListBizActionVO nvo = (clsGetCampFreeAndConServiceListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetFreeAndConcessionCampService");
                this.dbServer.AddInParameter(storedProcCommand, "CampID", DbType.Int64, nvo.CampID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.CampFreeServiceList == null)
                    {
                        nvo.CampFreeServiceList = new List<CampserviceDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        CampserviceDetailsVO item = new CampserviceDetailsVO {
                            CampDetailsID = (long) reader["CampDetailID"],
                            ServiceCode = (string) reader["ServiceCode"],
                            ServiceID = (long) reader["ServiceID"],
                            ServiceName = (string) reader["ServiceName"],
                            IsFree = (bool) reader["IsFree"],
                            IsConcession = (bool) reader["IsConcession"]
                        };
                        nvo.CampFreeServiceList.Add(item);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    if (nvo.CampConServiceList == null)
                    {
                        nvo.CampConServiceList = new List<CampserviceDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        CampserviceDetailsVO item = new CampserviceDetailsVO {
                            CampDetailsID = (long) reader["CampDetailID"],
                            ServiceCode = (string) DALHelper.HandleDBNull(reader["ServiceCode"]),
                            ServiceID = (long) reader["ServiceID"],
                            ServiceName = (string) reader["ServiceName"],
                            Rate = (double) reader["Rate"],
                            ConcessionPercentage = (double) reader["ConcessionPercentage"],
                            ConcessionAmount = (double) reader["ConcessionAmount"],
                            NetAmount = (double) reader["NetAmount"],
                            IsFree = (bool) reader["IsFree"],
                            IsConcession = (bool) reader["IsConcession"]
                        };
                        nvo.CampConServiceList.Add(item);
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

        public override IValueObject GetCampMasterByID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCampMasterByIDBizActionVO nvo = (clsGetCampMasterByIDBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCampMasterByID");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (nvo.CampMasterDetails == null)
                        {
                            nvo.CampMasterDetails = new clsCampMasterVO();
                        }
                        nvo.CampMasterDetails.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.CampMasterDetails.UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]);
                        nvo.CampMasterDetails.Code = (string) DALHelper.HandleDBNull(reader["Code"]);
                        nvo.CampMasterDetails.Description = (string) DALHelper.HandleDBNull(reader["Description"]);
                        nvo.CampMasterDetails.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
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

        public override IValueObject GetCampMasterList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCampMasterListBizActionVO nvo = (clsGetCampMasterListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCampMasterList");
                if ((nvo.Description != null) && (nvo.Description.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.Description + "%");
                }
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.CampMasterList == null)
                    {
                        nvo.CampMasterList = new List<clsCampMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsCampMasterVO item = new clsCampMasterVO {
                            ID = (long) reader["ID"],
                            UnitID = (long) reader["UnitID"],
                            Code = (string) DALHelper.HandleDBNull(reader["Code"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.CampMasterList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPROPatientList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPROPatientListBizActionVO nvo = valueObject as clsGetPROPatientListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetProPatientList");
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.VisitFromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.VisitToDate);
                this.dbServer.AddInParameter(storedProcCommand, "ReferredDoctorID", DbType.Int64, nvo.ReferredDoctorId);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.InputPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.InputStartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.InputMaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.SortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientList == null)
                    {
                        nvo.PatientList = new List<clsPatientGeneralVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientGeneralVO item = new clsPatientGeneralVO {
                            MRNo = (string) DALHelper.HandleDBNull(reader["MRNo"]),
                            Gender = (string) DALHelper.HandleDBNull(reader["Description"]),
                            FirstName = (string) DALHelper.HandleDBNull(reader["FirstName"]),
                            MiddleName = (string) DALHelper.HandleDBNull(reader["MiddleName"]),
                            LastName = (string) DALHelper.HandleDBNull(reader["LastName"]),
                            RegistrationDate = (DateTime?) DALHelper.HandleDBNull(reader["RegistrationDate"]),
                            ContactNO1 = (string) DALHelper.HandleDBNull(reader["ContactNo1"]),
                            DateOfBirth = DALHelper.HandleDate(reader["DateOfBirth"]),
                            PRORemark = (string) DALHelper.HandleDBNull(reader["Remark"]),
                            PatientID = (long) DALHelper.HandleDBNull(reader["PatientId"]),
                            UnitId = (long) DALHelper.HandleDBNull(reader["PatientUnitId"])
                        };
                        nvo.PatientList.Add(item);
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

        public override IValueObject UpdateCampDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            clsUpdateCampDetailsBizActionVO nvo = (clsUpdateCampDetailsBizActionVO) valueObject;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsCampMasterVO campMasterDetails = nvo.CampMasterDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateCampDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, campMasterDetails.CampDetailID);
                this.dbServer.AddInParameter(storedProcCommand, "CampID", DbType.Int64, campMasterDetails.CampID);
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, campMasterDetails.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "PatientRegistrationValidTillDate", DbType.DateTime, campMasterDetails.PatientRegistrationValidTillDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, campMasterDetails.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "City ", DbType.String, campMasterDetails.City.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Area ", DbType.String, campMasterDetails.Area.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "ValidDays", DbType.Double, campMasterDetails.ValidDays);
                this.dbServer.AddInParameter(storedProcCommand, "Concession", DbType.Double, campMasterDetails.Concession);
                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, campMasterDetails.TariffID);
                this.dbServer.AddInParameter(storedProcCommand, "Reason", DbType.String, campMasterDetails.Reason.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "EmailTemplateID", DbType.Int64, campMasterDetails.EmailTemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "SmsTemplateID", DbType.Int64, campMasterDetails.SmsTemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                if (((campMasterDetails.FreeCampServiceList != null) && (campMasterDetails.FreeCampServiceList.Count > 0)) || ((campMasterDetails.ConcessionServiceList != null) && (campMasterDetails.ConcessionServiceList.Count > 0)))
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteCampService");
                    this.dbServer.AddInParameter(command2, "CampID", DbType.Int64, campMasterDetails.CampDetailID);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                }
                foreach (CampserviceDetailsVO svo in campMasterDetails.FreeCampServiceList)
                {
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddCampService");
                    this.dbServer.AddInParameter(command3, "CampDetailID", DbType.Int64, campMasterDetails.CampDetailID);
                    this.dbServer.AddInParameter(command3, "ServiceID", DbType.Int64, svo.ServiceID);
                    this.dbServer.AddInParameter(command3, "IsFree", DbType.Boolean, svo.IsFree);
                    this.dbServer.AddInParameter(command3, "IsConcession", DbType.Boolean, svo.IsConcession);
                    this.dbServer.AddInParameter(command3, "Rate", DbType.Double, svo.Rate);
                    this.dbServer.AddInParameter(command3, "ConcessionPercentage", DbType.Double, svo.ConcessionPercentage);
                    this.dbServer.AddInParameter(command3, "ConcessionAmount", DbType.Double, svo.ConcessionAmount);
                    this.dbServer.AddInParameter(command3, "NetAmount", DbType.Double, svo.NetAmount);
                    this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command3, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.CampServiceID);
                    this.dbServer.ExecuteNonQuery(command3, transaction);
                    svo.CampServiceID = (long) this.dbServer.GetParameterValue(command3, "ID");
                }
                foreach (CampserviceDetailsVO svo2 in campMasterDetails.ConcessionServiceList)
                {
                    DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_AddCampService");
                    this.dbServer.AddInParameter(command4, "CampDetailID", DbType.Int64, campMasterDetails.CampDetailID);
                    this.dbServer.AddInParameter(command4, "ServiceID", DbType.Int64, svo2.ServiceID);
                    this.dbServer.AddInParameter(command4, "IsFree", DbType.Boolean, svo2.IsFree);
                    this.dbServer.AddInParameter(command4, "IsConcession", DbType.Boolean, svo2.IsConcession);
                    this.dbServer.AddInParameter(command4, "Rate", DbType.Double, svo2.Rate);
                    this.dbServer.AddInParameter(command4, "ConcessionPercentage", DbType.Double, svo2.ConcessionPercentage);
                    this.dbServer.AddInParameter(command4, "ConcessionAmount", DbType.Double, svo2.ConcessionAmount);
                    this.dbServer.AddInParameter(command4, "NetAmount", DbType.Double, svo2.NetAmount);
                    this.dbServer.AddInParameter(command4, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command4, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command4, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo2.CampServiceID);
                    this.dbServer.ExecuteNonQuery(command4, transaction);
                    svo2.CampServiceID = (long) this.dbServer.GetParameterValue(command4, "ID");
                }
                transaction.Commit();
                nvo.SuccessStatus = 0;
            }
            catch (Exception)
            {
                nvo.SuccessStatus = 0;
                transaction.Rollback();
                nvo.CampMasterDetails = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject UpdateCampMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateCampMasterBizActionVO nvo = (clsUpdateCampMasterBizActionVO) valueObject;
            try
            {
                clsCampMasterVO campMasterDetails = nvo.CampMasterDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateCampMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, campMasterDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, campMasterDetails.Code.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Description ", DbType.String, campMasterDetails.Description.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }
    }
}

