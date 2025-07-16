using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using System.Data.Common;
using System.Data;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsTariffDAL :clsBaseTariffDAL
    {
          #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        #endregion 

        private clsTariffDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql object
                //Create Instance of the database object and BaseSql object
                if (dbServer == null)
                {
                    dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }
        }




        public override IValueObject GetTariffList(IValueObject valueObject, clsUserVO objUserVO)
        {
            //throw new NotImplementedException();
            clsGetTariffMasterVO BizActionObj = (clsGetTariffMasterVO)valueObject;

            try
            {
               

                //Take storeprocedure name as input parameter and creates DbCommand Object.
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetTariffList");
                //Adding MasterTableName as Input Parameter to filter record
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "CompanyID", DbType.Int64, BizActionObj.CompanyID);
                //dbServer.AddInParameter(command, "PatientSourceID", DbType.Int64, BizActionObj.PatientSourceID);
                //dbServer.AddInParameter(command, "PatientSourceType", DbType.Int16, BizActionObj.PatientSourceType);
                //dbServer.AddInParameter(command, "ParentPatientID", DbType.Int64, BizActionObj.ParentPatientID);
                //dbServer.AddInParameter(command, "IsDate", DbType.Boolean, BizActionObj.IsDate.Value);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                //Check whether the reader contains the records
                if (reader.HasRows)
                {
                    //if masterlist instance is null then creates new instance
                    if (BizActionObj.List == null)
                    {
                        BizActionObj.List = new List<MasterListItem>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        //Add the object value in list
                        BizActionObj.List.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString()));
                    }
                }
                //if (!reader.IsClosed)
                //{
                //    reader.Close();
                //}
                reader.Close();

            }
            catch (Exception ex)
            {
                //CurrentMethodExecutionStatus = false;
              //  BizActionObj.Error = ex.Message;  //"Error Occured";
                //logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }


            return BizActionObj;


        }

        public override IValueObject GetPatientCategoryList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPatientCategoryMasterVO BizActionObj = (clsGetPatientCategoryMasterVO)valueObject;
            
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientCategoryList");
             
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                
                dbServer.AddInParameter(command, "PatientCategoryId", DbType.Int64, BizActionObj.PatientSourceID);
                //dbServer.AddInParameter(command, "UnitID", DbType.Int16, BizActionObj.UnitID);
                //dbServer.AddInParameter(command, "ParentPatientID", DbType.Int64, BizActionObj.ParentPatientID);
                
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);                
                if (reader.HasRows)
                {                    
                    if (BizActionObj.List == null)
                    {
                        BizActionObj.List = new List<MasterListItem>();
                    }                 
                    while (reader.Read())
                    {                       
                        BizActionObj.List.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString()));
                    }
                }                
                reader.Close();

            }
            catch (Exception ex)
            {
                //CurrentMethodExecutionStatus = false;
                //  BizActionObj.Error = ex.Message;  //"Error Occured";
                //logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return BizActionObj;
        }

        public override IValueObject GetCompanyList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetCompanyMasterVO BizActionObj = (clsGetCompanyMasterVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCompanyList");

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "IsForPathology", DbType.Int64, BizActionObj.IsForPathology);   //added by rohini 
                dbServer.AddInParameter(command, "PathologyCompanyType", DbType.Int64, BizActionObj.PathologyCompanyType);
                dbServer.AddInParameter(command, "PatientCategoryID", DbType.Int64, BizActionObj.PatientCategoryID);
                //dbServer.AddInParameter(command, "UnitID", DbType.Int16, BizActionObj.UnitID);
             

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                    {
                        BizActionObj.List = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        BizActionObj.List.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString()));
                    }
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                //CurrentMethodExecutionStatus = false;
                //  BizActionObj.Error = ex.Message;  //"Error Occured";
                //logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return BizActionObj;
        }

    }


}
