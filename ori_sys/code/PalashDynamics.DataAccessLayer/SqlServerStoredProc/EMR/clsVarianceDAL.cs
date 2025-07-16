using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.EMR;
using System.Data.Common;
using System.Data;
using com.seedhealthcare.hms.CustomExceptions;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsVarianceDAL : clsBaseVarianceDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsVarianceDAL()
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


        public override IValueObject AddVariance(IValueObject valueObject, clsUserVO UserVo)
        {

            clsAddVarianceBizActionVO BizActionObj = valueObject as clsAddVarianceBizActionVO;
            try
            {
                clsVarianceVO objVarianceVO = BizActionObj.VarianceDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddVariance");

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objVarianceVO.LinkServer);
                if (objVarianceVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objVarianceVO.LinkServer.Replace(@"\", "_"));


                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objVarianceVO.DoctorID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objVarianceVO.PatientID);
                dbServer.AddInParameter(command, "TemplateID", DbType.Int64, objVarianceVO.TemplateID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objVarianceVO.PatientUnitID);

                dbServer.AddInParameter(command, "Date", DbType.DateTime, objVarianceVO.Date);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, objVarianceVO.VisitID);

                dbServer.AddInParameter(command, "Variance1", DbType.String, objVarianceVO.Variance1);
                dbServer.AddInParameter(command, "Variance2", DbType.String, objVarianceVO.Variance2);
                dbServer.AddInParameter(command, "Variance3", DbType.String, objVarianceVO.Variance3);
                dbServer.AddInParameter(command, "Variance4", DbType.String, objVarianceVO.Variance4);
                dbServer.AddInParameter(command, "Variance5", DbType.String, objVarianceVO.Variance5);
                dbServer.AddInParameter(command, "Variance6", DbType.String, objVarianceVO.Variance6);
                dbServer.AddInParameter(command, "Variance7", DbType.String, objVarianceVO.Variance7);

                dbServer.AddInParameter(command, "ListVariance1", DbType.String, objVarianceVO.ListVariance1);
                dbServer.AddInParameter(command, "ListVariance2", DbType.String, objVarianceVO.ListVariance2);
                dbServer.AddInParameter(command, "ListVariance3", DbType.String, objVarianceVO.ListVariance3);
                dbServer.AddInParameter(command, "ListVariance4", DbType.String, objVarianceVO.ListVariance4);
                dbServer.AddInParameter(command, "ListVariance5", DbType.String, objVarianceVO.ListVariance5);
                dbServer.AddInParameter(command, "ListVariance6", DbType.String, objVarianceVO.ListVariance6);
                dbServer.AddInParameter(command, "ListVariance7", DbType.String, objVarianceVO.ListVariance7);

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objVarianceVO.UnitId);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objVarianceVO.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objVarianceVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.VarianceDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
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
    }
}
