using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Masters;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.ConfigurationManager;

using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using System.Data.Common;
using System.Data;


namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Masters
{
    public class clsCounterDAL : clsBaseCounterDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        #endregion

        private clsCounterDAL()
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




        public override IValueObject GetCounterListByUnitId(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetCounterDetailsBizActionVO BizActionObj = valueObject as clsGetCounterDetailsBizActionVO;


            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_CounterListByUnitId");
                dbServer.AddInParameter(command, "ClinicID", DbType.Int64, BizActionObj.ClinicID);

                DbDataReader reader;

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.CounterDetails == null)
                        BizActionObj.CounterDetails = new List<clsCounterVO>();

                    while (reader.Read())
                    {
                        //rohinee for cash caounter entery from app config
                        clsCounterVO objCounter = new clsCounterVO();

                        objCounter.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objCounter.Description = (string)DALHelper.HandleDBNull(reader["Description"]);

                        BizActionObj.CounterDetails.Add(objCounter);
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
