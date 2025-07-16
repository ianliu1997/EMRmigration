using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.TokenDisplay;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.TokenDisplay;
using System.Data;
using System.Data.Common;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.TokenDisplay
{

    public class clsTokenDisplayDAL : clsBaseTokenDisplayDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsTokenDisplayDAL()
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
            catch (Exception)
            {
                throw;
            }
        }



        public override IValueObject AddUpdateTokenDisplayDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateTokenDisplayBizActionVO objBizAction = valueObject as clsAddUpdateTokenDisplayBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateTokenDisplay");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitId);
                dbServer.AddInParameter(command, "PatientId", DbType.Int64, objBizAction.PatientId);
                dbServer.AddInParameter(command, "VisitId", DbType.Int64, objBizAction.VisitId);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objBizAction.DoctorID);
                dbServer.AddInParameter(command, "VisitDate", DbType.DateTime, objBizAction.VisitDate);
                dbServer.AddInParameter(command, "IsDisplay", DbType.Boolean, objBizAction.IsDisplay);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objBizAction.Id);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int64, ParameterDirection.Output, null, DataRowVersion.Default, objBizAction.SuccessStatus);

                int intStatus = dbServer.ExecuteNonQuery(command);
                objBizAction.SuccessStatus = (long)dbServer.GetParameterValue(command, "ResultStatus");
                objBizAction.Id = (long)dbServer.GetParameterValue(command, "Id");

            }
            catch (Exception)
            {
                throw;
            }
            return objBizAction;
        }

        public override IValueObject UpdateStatusTokenDisplay(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateTokenDisplayStatusBizActionVO objBizAction = valueObject as clsUpdateTokenDisplayStatusBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateTokenDisplayStatus");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitId);
                dbServer.AddInParameter(command, "PatientId", DbType.Int64, objBizAction.PatientId);
                dbServer.AddInParameter(command, "VisitId", DbType.Int64, objBizAction.VisitId);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objBizAction.DoctorID);
                //dbServer.AddInParameter(command, "VisitDate", DbType.DateTime, objBizAction.VisitDate);
                dbServer.AddInParameter(command, "IsDisplay", DbType.Boolean, objBizAction.IsDisplay);

                //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objBizAction.Id);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int64, ParameterDirection.Output, null, DataRowVersion.Default, objBizAction.SuccessStatus);

                int intStatus = dbServer.ExecuteNonQuery(command);
                objBizAction.SuccessStatus = (long)dbServer.GetParameterValue(command, "ResultStatus");
                //objBizAction.Id = (long)dbServer.GetParameterValue(command, "Id");

            }
            catch (Exception)
            {
                throw;
            }
            return objBizAction;
        }

        public override IValueObject GetTokenDisplayDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetTokenDisplayBizActionVO BizActionObj = valueObject as clsGetTokenDisplayBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetTokenDisplayDetails");

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizActionObj.UnitId);
                dbServer.AddInParameter(command, "VisitDate", DbType.DateTime, BizActionObj.VisitDate);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ListTokenDisplay == null)
                        BizActionObj.ListTokenDisplay = new List<clsTokenDisplayVO>();
                    int i = 1;
                    while (reader.Read())
                    {
                        clsTokenDisplayVO objTokenDisplayVO = new clsTokenDisplayVO();
                        objTokenDisplayVO.ID = i;//Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        objTokenDisplayVO.MrNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        objTokenDisplayVO.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["Patient Name"]));
                        objTokenDisplayVO.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                        objTokenDisplayVO.Cabin = Convert.ToString(DALHelper.HandleDBNull(reader["Cabin"]));
                        objTokenDisplayVO.TokenNo = Convert.ToString(DALHelper.HandleDBNull(reader["TokenNO"]));
                        BizActionObj.ListTokenDisplay.Add(objTokenDisplayVO);
                        i++;
                    }
                    //reader.NextResult();
                    //BizActionObj.Id = Convert.ToInt32(dbServer.GetParameterValue(command, "DiagnosisId"));
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }


        public override IValueObject GetPatientTokenDisplayDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetTokenDisplayPatirntDetailsBizActionVO BizActionObj = valueObject as clsGetTokenDisplayPatirntDetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetTokenDisplayPatientDetailsForStatus");

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizActionObj.UnitId);
                dbServer.AddInParameter(command, "PatientId", DbType.Int64, BizActionObj.PatientId);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitId);


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //BizActionObj.VisitDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["VisitDate"]));
                        // BizActionObj.IsDisplay  = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsDisplay"]));
                        BizActionObj.IsDisplay = true;//Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsDisplay"]));
                    }
                    reader.Close();
                }

            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;



        }
      

    }
}
