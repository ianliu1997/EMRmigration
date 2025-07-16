namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
    using PalashDynamics.ValueObjects;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsPatientConsentDAL : clsBasePatientConsentDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsPatientConsentDAL()
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

        private clsAddPatientConsentBizActionVO AddConsent(clsAddPatientConsentBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsPatientConsentVO consentDetails = BizActionObj.ConsentDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatientConsentMaster");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, consentDetails.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, consentDetails.Description);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, consentDetails.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "Template", DbType.String, consentDetails.Template);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, consentDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
                BizActionObj.ConsentDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject AddPatientConsent(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddPatientConsentBizActionVO bizActionObj = valueObject as clsAddPatientConsentBizActionVO;
            bizActionObj = (bizActionObj.ConsentDetails.ID != 0L) ? this.UpdateConsent(bizActionObj, objUserVO) : this.AddConsent(bizActionObj, objUserVO);
            return valueObject;
        }

        public override IValueObject AddUpdateIVFPackegeConsentList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateIVFPackegeConsentBizActionVO nvo = valueObject as clsAddUpdateIVFPackegeConsentBizActionVO;
            try
            {
                foreach (clsPatientConsentVO tvo in nvo.ConsentMatserDetails)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateIVFTherapyConsentDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyId", DbType.Int64, nvo.PlanTherapyId);
                    this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitId", DbType.Int64, nvo.PlanTherapyUnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, tvo.ServiceID);
                    this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, tvo.ConsentID);
                    this.dbServer.AddInParameter(storedProcCommand, "IsConsentCheck", DbType.Boolean, tvo.IsConsentCheck);
                    this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, tvo.Description);
                    this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, tvo.DepartmentID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdateConsentCheckInPlanTherapy", DbType.Boolean, nvo.UpdateConsentCheckInPlanTherapy);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.ID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                    this.dbServer.AddParameter(storedProcCommand, "ConsentCheck", DbType.Boolean, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.ConsentCheck);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
                    nvo.ConsentCheck = Convert.ToBoolean(this.dbServer.GetParameterValue(storedProcCommand, "ConsentCheck"));
                    tvo.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetIVFPackegeConsentList(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsGetIVFPackegeConsentBizActionVO nvo = valueObject as clsGetIVFPackegeConsentBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIVFConsentListDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientunitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.PlanTherapyId);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.PlanTherapyUnitId);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ConsentMatserDetails == null)
                    {
                        nvo.ConsentMatserDetails = new List<clsPatientConsentVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientConsentVO item = new clsPatientConsentVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"])),
                            ConsentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"])),
                            Template = Convert.ToString(DALHelper.HandleDBNull(reader["Template"]))
                        };
                        nvo.ConsentMatserDetails.Add(item);
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

        public override IValueObject GetIVFSavedPackegeConsentList(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsGetIVFSavedPackegeConsentBizActionVO nvo = valueObject as clsGetIVFSavedPackegeConsentBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIVFSavedTherapyConsentListDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientunitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.PlanTherapyId);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.PlanTherapyUnitId);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ConsentMatserDetails == null)
                    {
                        nvo.ConsentMatserDetails = new List<clsPatientConsentVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientConsentVO item = new clsPatientConsentVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"])),
                            ConsentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"])),
                            IsConsentCheck = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsConsentCheck"])),
                            Template = Convert.ToString(DALHelper.HandleDBNull(reader["Template"]))
                        };
                        nvo.ConsentMatserDetails.Add(item);
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

        public override IValueObject GetPatientConsent(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetPatientConsentMasterBizActionVO nvo = valueObject as clsGetPatientConsentMasterBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientConsentMaster");
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, nvo.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "Template", DbType.Int64, nvo.Template);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ConsentMatserDetails == null)
                    {
                        nvo.ConsentMatserDetails = new List<clsPatientConsentVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientConsentVO item = new clsPatientConsentVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"])),
                            Template = Convert.ToString(DALHelper.HandleDBNull(reader["Template"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            Department = Convert.ToString(DALHelper.HandleDBNull(reader["Department"]))
                        };
                        nvo.ConsentMatserDetails.Add(item);
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

        public override IValueObject GetPatientConsentList(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsGetPatientConsentBizActionVO nvo = valueObject as clsGetPatientConsentBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientConsentList");
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "PatientId", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ConsentMatserDetails == null)
                    {
                        nvo.ConsentMatserDetails = new List<clsPatientConsentVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientConsentVO item = new clsPatientConsentVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]))),
                            ConsentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConsentId"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["ConsentName"])),
                            AddedBy = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ConsentAddedBy"]))),
                            IsDoc = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDoctor"])),
                            IsEmployee = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEmployee"])),
                            AddedByUserName = Convert.ToString(DALHelper.HandleDBNull(reader["UserName"])),
                            Template = Convert.ToString(DALHelper.HandleDBNull(reader["Template"]))
                        };
                        nvo.ConsentMatserDetails.Add(item);
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

        private clsAddPatientConsentBizActionVO UpdateConsent(clsAddPatientConsentBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsPatientConsentVO consentDetails = BizActionObj.ConsentDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePatientConsentMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, consentDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, consentDetails.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, consentDetails.Description);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, consentDetails.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "Template", DbType.String, consentDetails.Template);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }
    }
}

