using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.DashBoardVO;
using System.Data.Common;
using System.Data;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IVFPlanTherapy.DashBoard
{
    class clsIVFSpermDefectTestDAL : clsBaseIVFSpermDefectTestDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsIVFSpermDefectTestDAL()
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


        public override IValueObject AddUpdateSpermDefectTest(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateIVFSpermDefectTestBizActionVO BizActionObj = valueObject as clsAddUpdateIVFSpermDefectTestBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateSpermDefectTest");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.ClsIVFSpermDefectTest.UnitId);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.ClsIVFSpermDefectTest.Date);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.ClsIVFSpermDefectTest.VisitID);
                dbServer.AddInParameter(command, "VisitUnitID", DbType.Int64, BizActionObj.ClsIVFSpermDefectTest.VisitUnitID);
                dbServer.AddInParameter(command, "AnnexinBinding", DbType.Double, BizActionObj.ClsIVFSpermDefectTest.AnnexinBinding);
                dbServer.AddInParameter(command, "CaspaseActivity", DbType.Double, BizActionObj.ClsIVFSpermDefectTest.CaspaseActivity);
                dbServer.AddInParameter(command, "AcrosinActivity", DbType.Double, BizActionObj.ClsIVFSpermDefectTest.AcrosinActivity);
                dbServer.AddInParameter(command, "GlucosidaseActivity", DbType.Double, BizActionObj.ClsIVFSpermDefectTest.GlucosidaseActivity);
                dbServer.AddInParameter(command, "AndrologistID", DbType.Int64, BizActionObj.ClsIVFSpermDefectTest.AndrologistID);
                dbServer.AddInParameter(command, "Remark", DbType.String, BizActionObj.ClsIVFSpermDefectTest.Remarks.Trim());
                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, BizActionObj.ClsIVFSpermDefectTest.IsFreezed);            
                
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput,null,DataRowVersion.Default, BizActionObj.ClsIVFSpermDefectTest.ID);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.ClsIVFSpermDefectTest.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;               
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return BizActionObj;
        }

        public override IValueObject GetSpermDefectTestList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIVFSpermDefectTestBizActionVO BizActionObj = valueObject as clsGetIVFSpermDefectTestBizActionVO;
            BizActionObj.ObjSpermDefectTestList =null;
            try
            {
                DbCommand command;
                DbDataReader reader;
                command = dbServer.GetStoredProcCommand("CIMS_GetSpermDefectTestList");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.ClsIVFSpermDefectTest.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.ClsIVFSpermDefectTest.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.ClsIVFSpermDefectTest.PatientUnitID);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.ObjSpermDefectTestList == null)
                        BizActionObj.ObjSpermDefectTestList = new List<clsIVFSpermDefectTestVO>();
                    while (reader.Read())
                    {
                        clsIVFSpermDefectTestVO objVO = new clsIVFSpermDefectTestVO();

                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objVO.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        objVO.VisitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitID"]));
                        objVO.VisitUnitID= Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitUnitID"]));
                        objVO.AnnexinBinding = Convert.ToDouble(DALHelper.HandleDBNull(reader["AnnexinBinding"]));
                        objVO.CaspaseActivity = Convert.ToDouble(DALHelper.HandleDBNull(reader["CaspaseActivity"]));
                        objVO.AcrosinActivity = Convert.ToDouble(DALHelper.HandleDBNull(reader["AcrosinActivity"]));
                        objVO.GlucosidaseActivity = Convert.ToDouble(DALHelper.HandleDBNull(reader["GlucosidaseActivity"]));
                        objVO.AndrologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AndrologistID"]));
                        objVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        objVO.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        objVO.OPDNo = Convert.ToString(DALHelper.HandleDBNull(reader["OPDNO"]));
                        BizActionObj.ObjSpermDefectTestList.Add(objVO);
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
            return valueObject;
        }
    }
}
