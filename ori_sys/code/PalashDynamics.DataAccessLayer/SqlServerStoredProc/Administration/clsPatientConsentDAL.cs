using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using System.Data.Common;
using System.Data;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
   public class clsPatientConsentDAL:clsBasePatientConsentDAL
    {
         
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsPatientConsentDAL()
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


        public override IValueObject AddPatientConsent(IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsAddPatientConsentBizActionVO BizActionObj = valueObject as clsAddPatientConsentBizActionVO;

            if (BizActionObj.ConsentDetails.ID == 0)
                BizActionObj = AddConsent(BizActionObj, objUserVO);
            else
                BizActionObj=UpdateConsent(BizActionObj,objUserVO);

            return valueObject;
        }


        private clsAddPatientConsentBizActionVO AddConsent(clsAddPatientConsentBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsPatientConsentVO objConsentVO = BizActionObj.ConsentDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPatientConsentMaster");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Code", DbType.String, objConsentVO.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, objConsentVO.Description);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, objConsentVO.DepartmentID);
                dbServer.AddInParameter(command, "Template", DbType.String, objConsentVO.Template);

                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objConsentVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
                BizActionObj.ConsentDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;

        }


        private clsAddPatientConsentBizActionVO UpdateConsent(clsAddPatientConsentBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsPatientConsentVO objConsentVO = BizActionObj.ConsentDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdatePatientConsentMaster");

                dbServer.AddInParameter(command, "ID", DbType.Int64, objConsentVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Code", DbType.String, objConsentVO.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, objConsentVO.Description);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, objConsentVO.DepartmentID);
                dbServer.AddInParameter(command, "Template", DbType.String, objConsentVO.Template);

                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

               
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
               
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;

        }

        public override IValueObject GetPatientConsent(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetPatientConsentMasterBizActionVO BizAction = valueObject as clsGetPatientConsentMasterBizActionVO;
           
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetPatientConsentMaster");
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizAction.SearchExpression);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizAction.DepartmentID);
                dbServer.AddInParameter(command, "Template", DbType.Int64, BizAction.Template);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizAction.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizAction.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizAction.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                
                if (reader.HasRows)
                {
                    if (BizAction.ConsentMatserDetails == null)
                        BizAction.ConsentMatserDetails = new List<clsPatientConsentVO>();
                    while (reader.Read())
                    {
                        clsPatientConsentVO ConsentVO = new clsPatientConsentVO();

                        ConsentVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        ConsentVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        ConsentVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        ConsentVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        ConsentVO.DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"]));
                        ConsentVO.Template = Convert.ToString(DALHelper.HandleDBNull(reader["Template"]));
                        ConsentVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        ConsentVO.Department = Convert.ToString(DALHelper.HandleDBNull(reader["Department"]));
                        
                        BizAction.ConsentMatserDetails.Add(ConsentVO);
                    }
                }

                reader.NextResult();
                BizAction.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();

            }
            catch (Exception ex)
            {
                throw;
            }
            return BizAction; 
        }

        public override IValueObject GetPatientConsentList(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsGetPatientConsentBizActionVO BizAction = valueObject as clsGetPatientConsentBizActionVO;

            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetPatientConsentList");
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizAction.SearchExpression);
                //dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizAction.DepartmentID);
                //dbServer.AddInParameter(command, "Template", DbType.Int64, BizAction.Template);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizAction.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizAction.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizAction.MaximumRows);
                dbServer.AddInParameter(command, "PatientId", DbType.Int64, BizAction.PatientID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizAction.ConsentMatserDetails == null)
                        BizAction.ConsentMatserDetails = new List<clsPatientConsentVO>();
                    while (reader.Read())
                    {
                        clsPatientConsentVO ConsentVO = new clsPatientConsentVO();

                        ConsentVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        ConsentVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        ConsentVO.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                        //ConsentVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        ConsentVO.ConsentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConsentId"]));
                        ConsentVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["ConsentName"]));
                        ConsentVO.AddedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConsentAddedBy"]));
                        ConsentVO.IsDoc = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDoctor"]));
                        ConsentVO.IsEmployee = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEmployee"]));
                        ConsentVO.AddedByUserName = Convert.ToString(DALHelper.HandleDBNull(reader["UserName"]));                        
                        //ConsentVO.DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"]));
                        ConsentVO.Template = Convert.ToString(DALHelper.HandleDBNull(reader["Template"]));
                        //ConsentVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        //ConsentVO.Department = Convert.ToString(DALHelper.HandleDBNull(reader["Department"]));

                        BizAction.ConsentMatserDetails.Add(ConsentVO);
                    }
                }
                reader.NextResult();
                BizAction.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();
            }
            catch (Exception ex)
            {                
                throw;
            }

            return BizAction;
        }

       //added by neena
        public override IValueObject GetIVFPackegeConsentList(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsGetIVFPackegeConsentBizActionVO BizAction = valueObject as clsGetIVFPackegeConsentBizActionVO;

            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetIVFConsentListDetails");
                //dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizAction.SearchExpression);
                //dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizAction.DepartmentID);
                //dbServer.AddInParameter(command, "Template", DbType.Int64, BizAction.Template);
                //dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizAction.PagingEnabled);
                //dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizAction.StartRowIndex);
                //dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizAction.MaximumRows);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.PatientID);
                dbServer.AddInParameter(command, "PatientunitID", DbType.Int64, BizAction.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.PlanTherapyId);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.PlanTherapyUnitId);            
                //dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizAction.ConsentMatserDetails == null)
                        BizAction.ConsentMatserDetails = new List<clsPatientConsentVO>();
                    while (reader.Read())
                    {
                        clsPatientConsentVO ConsentVO = new clsPatientConsentVO();
                        ConsentVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        ConsentVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        ConsentVO.ConsentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateID"]));
                        ConsentVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        ConsentVO.DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"]));
                        ConsentVO.Template = Convert.ToString(DALHelper.HandleDBNull(reader["Template"]));
                       // ConsentVO.IsEnabledConsent = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEnabledConsent"]));

                        //ConsentVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                       // ConsentVO.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                        //ConsentVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                     
                        //ConsentVO.AddedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConsentAddedBy"]));
                        //ConsentVO.IsDoc = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDoctor"]));
                        //ConsentVO.IsEmployee = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEmployee"]));
                        //ConsentVO.AddedByUserName = Convert.ToString(DALHelper.HandleDBNull(reader["UserName"]));
                        //ConsentVO.DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"]));
                     
                        //ConsentVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        //ConsentVO.Department = Convert.ToString(DALHelper.HandleDBNull(reader["Department"]));

                        BizAction.ConsentMatserDetails.Add(ConsentVO);
                    }
                }
                //reader.NextResult();
                //BizAction.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }

            return BizAction;
        }

        public override IValueObject AddUpdateIVFPackegeConsentList(IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {
            clsAddUpdateIVFPackegeConsentBizActionVO BizActionObj = valueObject as clsAddUpdateIVFPackegeConsentBizActionVO;
            try
            {
                foreach (var item in BizActionObj.ConsentMatserDetails)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateIVFTherapyConsentDetails");
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                    dbServer.AddInParameter(command, "PlanTherapyId", DbType.Int64, BizActionObj.PlanTherapyId);
                    dbServer.AddInParameter(command, "PlanTherapyUnitId", DbType.Int64, BizActionObj.PlanTherapyUnitId);

                    dbServer.AddInParameter(command, "ServiceID", DbType.Int64, item.ServiceID);
                    dbServer.AddInParameter(command, "TemplateID", DbType.Int64, item.ConsentID);
                    dbServer.AddInParameter(command, "IsConsentCheck", DbType.Boolean, item.IsConsentCheck);
                    dbServer.AddInParameter(command, "Description", DbType.String, item.Description);
                    dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, item.DepartmentID);
                    dbServer.AddInParameter(command, "UpdateConsentCheckInPlanTherapy", DbType.Boolean, BizActionObj.UpdateConsentCheckInPlanTherapy);
                    //dbServer.AddInParameter(command, "Template", DbType.String, item.Template);

                   // dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                    dbServer.AddParameter(command, "ConsentCheck", DbType.Boolean, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.ConsentCheck);                    

                    int intStatus = dbServer.ExecuteNonQuery(command);
                    BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
                    BizActionObj.ConsentCheck = Convert.ToBoolean(dbServer.GetParameterValue(command, "ConsentCheck"));
                    item.ID = (long)dbServer.GetParameterValue(command, "ID");
                }
                                
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
           

            return valueObject;
        }

        public override IValueObject GetIVFSavedPackegeConsentList(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsGetIVFSavedPackegeConsentBizActionVO BizAction = valueObject as clsGetIVFSavedPackegeConsentBizActionVO;

            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetIVFSavedTherapyConsentListDetails");
               // dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizAction.SearchExpression);
                //dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizAction.DepartmentID);
                //dbServer.AddInParameter(command, "Template", DbType.Int64, BizAction.Template);
                //dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizAction.PagingEnabled);
                //dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizAction.StartRowIndex);
                //dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizAction.MaximumRows);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.PatientID);
                dbServer.AddInParameter(command, "PatientunitID", DbType.Int64, BizAction.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.PlanTherapyId);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.PlanTherapyUnitId);
               // dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizAction.ConsentMatserDetails == null)
                        BizAction.ConsentMatserDetails = new List<clsPatientConsentVO>();
                    while (reader.Read())
                    {
                        clsPatientConsentVO ConsentVO = new clsPatientConsentVO();
                        ConsentVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        ConsentVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        ConsentVO.ConsentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateID"]));
                        ConsentVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        ConsentVO.DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"]));
                        ConsentVO.IsConsentCheck = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsConsentCheck"]));
                        ConsentVO.Template = Convert.ToString(DALHelper.HandleDBNull(reader["Template"]));

                        //ConsentVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        // ConsentVO.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                        //ConsentVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));

                        //ConsentVO.AddedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConsentAddedBy"]));
                        //ConsentVO.IsDoc = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDoctor"]));
                        //ConsentVO.IsEmployee = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEmployee"]));
                        //ConsentVO.AddedByUserName = Convert.ToString(DALHelper.HandleDBNull(reader["UserName"]));
                        //ConsentVO.DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"]));

                        //ConsentVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        //ConsentVO.Department = Convert.ToString(DALHelper.HandleDBNull(reader["Department"]));

                        BizAction.ConsentMatserDetails.Add(ConsentVO);
                    }
                }
                //reader.NextResult();
                //BizAction.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }

            return BizAction;
        }

       //
    }
}
