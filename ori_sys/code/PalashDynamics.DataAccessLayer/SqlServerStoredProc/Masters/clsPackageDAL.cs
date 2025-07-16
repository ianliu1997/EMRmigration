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
    public class clsPackageDAL : clsBasePackageDAL
    {

        #region Variables Declaration
            //Declare the database object
            private Database dbServer = null;
        #endregion 

        private clsPackageDAL()
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


        public override IValueObject GetPackageList(IValueObject valueObject)
        {
            clsGetPackageMasterVO BizActionObj = (clsGetPackageMasterVO)valueObject;
            try
            {                
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPackageList");                
                //dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);  
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
              
                reader.Close();

            }
            catch (Exception ex)
            {
               
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
