using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.DashBoardVO;
using System.Data.Common;
using System.Data;
using com.seedhealthcare.hms.Web.Logging;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IVFPlanTherapy.DashBoard
{
    class clsIVFROSEstimationDAL : clsBaseIVFROSEstimationDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsIVFROSEstimationDAL()
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


        public override IValueObject AddUpdateROSEstimation(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateIVFROSEstimationBizActionVO BizActionObj = valueObject as clsAddUpdateIVFROSEstimationBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateROSEstimation");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.clsIVFROSEstimation.UnitId);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.clsIVFROSEstimation.Date);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.clsIVFROSEstimation.VisitID);
                dbServer.AddInParameter(command, "VisitUnitID", DbType.Int64, BizActionObj.clsIVFROSEstimation.VisitUnitID);
                dbServer.AddInParameter(command, "ROSLevel", DbType.Double, BizActionObj.clsIVFROSEstimation.ROSLevel);
                dbServer.AddInParameter(command, "TotalAntioxidantCapacity", DbType.Double, BizActionObj.clsIVFROSEstimation.TotalAntioxidantCapacity);
                dbServer.AddInParameter(command, "ROSTACScore", DbType.Double, BizActionObj.clsIVFROSEstimation.ROSTACScore);
                dbServer.AddInParameter(command, "AndrologistID", DbType.Int64, BizActionObj.clsIVFROSEstimation.AndrologistID);
                dbServer.AddInParameter(command, "Remark", DbType.String, BizActionObj.clsIVFROSEstimation.Remarks.Trim());
                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, BizActionObj.clsIVFROSEstimation.IsFreezed);            
                
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.clsIVFROSEstimation.ID);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.clsIVFROSEstimation.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));
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

        public override IValueObject GetROSEstimationList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIVFROSEstimationBizActionVO BizActionObj = valueObject as clsGetIVFROSEstimationBizActionVO;
            BizActionObj.ObjROSEstimationList =null;
            try
            {
                DbCommand command;
                DbDataReader reader;
                command = dbServer.GetStoredProcCommand("CIMS_GetROSEstimationList");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.clsIVFROSEstimation.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.clsIVFROSEstimation.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.clsIVFROSEstimation.PatientUnitID);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.ObjROSEstimationList == null)
                        BizActionObj.ObjROSEstimationList = new List<clsIVFROSEstimationVO>();
                    while (reader.Read())
                    {
                        clsIVFROSEstimationVO objVO = new clsIVFROSEstimationVO();

                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objVO.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        objVO.VisitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitID"]));
                        objVO.VisitUnitID= Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitUnitID"]));
                        objVO.ROSLevel = Convert.ToDouble(DALHelper.HandleDBNull(reader["ROSLevel"]));
                        objVO.TotalAntioxidantCapacity = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAntioxidantCapacity"]));
                        objVO.ROSTACScore = Convert.ToDouble(DALHelper.HandleDBNull(reader["ROSTACScore"]));
                        objVO.AndrologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AndrologistID"]));
                        objVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        objVO.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        objVO.OPDNo = Convert.ToString(DALHelper.HandleDBNull(reader["OPDNO"]));
                        BizActionObj.ObjROSEstimationList.Add(objVO);
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
