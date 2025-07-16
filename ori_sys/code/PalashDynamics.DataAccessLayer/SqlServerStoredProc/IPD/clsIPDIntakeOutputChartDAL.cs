using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.IPD;
using System.Data.Common;
using System.Data;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsIPDIntakeOutputChartDAL : clsBaseIPDIntakeOutputChartDAL
    {

        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        #endregion

        private clsIPDIntakeOutputChartDAL()
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

        # region  DAL Added By Kiran  for ADD,Update,GetIntakeOutputChartDetails,GetIntakeOutputChartDetailsByPatientID And UpdateStatusIntakeOutputChart

        public override IValueObject AddUpdateIntakeOutputChart(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO UserVO)
        {      
            clsAddUpdateIntakeOutputChartAndDetailsBizActionVO objItem = valueObject as clsAddUpdateIntakeOutputChartAndDetailsBizActionVO;
            try
            {
                clsIPDIntakeOutputChartVO ObjIntOutVO = objItem.IntakeOutputDetails;              
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateIntakeOutputChart");

                dbServer.AddInParameter(command, "ID", DbType.Int64, ObjIntOutVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, ObjIntOutVO.UnitID);
                dbServer.AddInParameter(command, "AdmID", DbType.Int64, ObjIntOutVO.AdmID);
                dbServer.AddInParameter(command, "AdmUnitID", DbType.Int64, ObjIntOutVO.AdmUnitID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, ObjIntOutVO.Date);
                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, ObjIntOutVO.IsFreezed);

                dbServer.AddInParameter(command, "Status", DbType.Boolean, ObjIntOutVO.Status);

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, ObjIntOutVO.UnitID);            //ObjIntOutVO.CreatedUnitID
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, ObjIntOutVO.UpdatedUnitID);     //ObjIntOutVO.UpdatedUnitID
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVO.ID);                           // ObjIntOutVO.AddedBy
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVO.UserLoginInfo.MachineName);  //ObjIntOutVO.AddedOn
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, ObjIntOutVO.AddedDateTime);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int32, ObjIntOutVO.UpdatedBy);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, ObjIntOutVO.UpdatedOn);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, ObjIntOutVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);     // ObjIntOutVO.AddedWindowsLoginName
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, ObjIntOutVO.UpdateWindowsLoginName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                objItem.IntakeOutputDetails.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ResultStatus"));

                if (objItem.IntakeOutputList != null && objItem.IntakeOutputList.Count > 0)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_DeleteIntakeOutputChartDetail");
                    dbServer.AddInParameter(command1, "IntakeOutputID", DbType.Int64, objItem.IntakeOutputDetails.ID);
                    dbServer.AddInParameter(command1, "IntakeOutputIDUnitID", DbType.Int64, objItem.IntakeOutputDetails.UnitID);
                   
                    int intStatus1 = dbServer.ExecuteNonQuery(command1);


                    foreach (clsIPDIntakeOutputChartVO item in objItem.IntakeOutputList)
                    {
                        AddUpdateIntakeOutputChartDetail(item, objItem.IntakeOutputDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return valueObject;

        }

        public void AddUpdateIntakeOutputChartDetail(clsIPDIntakeOutputChartVO ObjIntOutVO, clsIPDIntakeOutputChartVO IntakeOutputDetails)
        {
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateIntakeOutputChartDetail");

                dbServer.AddInParameter(command, "ID", DbType.Int64, ObjIntOutVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64,IntakeOutputDetails.UnitID);
                dbServer.AddInParameter(command, "IntakeOutputID", DbType.Int64, IntakeOutputDetails.ID);
                dbServer.AddInParameter(command, "IntakeOutputIDUnitID", DbType.Int64, IntakeOutputDetails.UnitID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, IntakeOutputDetails.Date);
                
                dbServer.AddInParameter(command, "Time", DbType.String, ObjIntOutVO.strTime);
                dbServer.AddInParameter(command, "Oral", DbType.Double, ObjIntOutVO.Oral);
                dbServer.AddInParameter(command, "Total_Parenteral", DbType.Double, ObjIntOutVO.Total_Parenteral);
                dbServer.AddInParameter(command, "OtherIntake", DbType.Double, ObjIntOutVO.OtherIntake);
                dbServer.AddInParameter(command, "Urine", DbType.Double, ObjIntOutVO.Urine);
                dbServer.AddInParameter(command, "Ng", DbType.Double, ObjIntOutVO.Ng);
                dbServer.AddInParameter(command, "Drain", DbType.Double, ObjIntOutVO.Drain);
                dbServer.AddInParameter(command, "OtherOutput", DbType.Double, ObjIntOutVO.Drain);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                //objItem.IntakeOutputDetails.SuccessStatus = Convert.ToInt64(dbServer.GetParameterValue(command, "ResultStatus"));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override IValueObject GetIntakeOutputChartDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIntakeOutputChartDetailsBizActionVO BizActionObj = valueObject as clsGetIntakeOutputChartDetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetIntakeOutputChartDetail");

                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.GetIntakeOutputDetails.PatientUnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.GetIntakeOutputDetails.PatientID);
                //dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.GetIntakeOutputDetails.Date);

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.GetIntakeOutputDetails.InputPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.GetIntakeOutputDetails.InputStartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.GetIntakeOutputDetails.InputMaximumRows);               
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.GetIntakeOutputList == null)
                        BizActionObj.GetIntakeOutputList = new List<clsIPDIntakeOutputChartVO>();
                    while (reader.Read())
                    {
                        clsIPDIntakeOutputChartVO objAdvVO = new clsIPDIntakeOutputChartVO();

                        objAdvVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objAdvVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objAdvVO.AdmID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmID"]));
                        objAdvVO.AdmUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmUnitID"]));
                        objAdvVO.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        objAdvVO.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        objAdvVO.Date = (DateTime?)DALHelper.HandleDBNull(reader["Date"]);
                        objAdvVO.Status = (bool)reader["Status"];
                        objAdvVO.IntakeTotal = (double)reader["TotalIntake"];
                        objAdvVO.OutputTotal = (double)reader["TotalOutput"];
                        objAdvVO.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));

                        BizActionObj.GetIntakeOutputList.Add(objAdvVO);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return BizActionObj;
        }
      
        public override IValueObject GetIntakeOutputChartDetailsByPatientID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIntakeOutputChartDetailsByPatientIDBizActionVO BizActionObj = valueObject as clsGetIntakeOutputChartDetailsByPatientIDBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetIntakeOutputChartDetailbyPatientID");

                dbServer.AddInParameter(command, "IntakeOutputID", DbType.Int64, BizActionObj.GetIntakeOutputDetails.IntakeOutputID);
                dbServer.AddInParameter(command, "IntakeOutputIDUnitID", DbType.Int64, BizActionObj.GetIntakeOutputDetails.IntakeOutputIDUnitID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.GetIntakeOutputDetails.Date);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {

                    if (BizActionObj.GetIntakeOutputList == null)
                        BizActionObj.GetIntakeOutputList = new List<clsIPDIntakeOutputChartVO>();
                    while (reader.Read())
                    {
                        clsIPDIntakeOutputChartVO objAdvVO = new clsIPDIntakeOutputChartVO();
                        objAdvVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objAdvVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objAdvVO.IntakeOutputID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IntakeOutputID"]));
                        objAdvVO.IntakeOutputIDUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IntakeOutputIDUnitID"]));
                        objAdvVO.Date = (DateTime?)DALHelper.HandleDBNull(reader["Date"]);
                        objAdvVO.strTime = Convert.ToString(DALHelper.HandleDBNull(reader["Time"]));
                        objAdvVO.Oral = Convert.ToDouble(reader["Oral"]);
                        objAdvVO.Total_Parenteral = Convert.ToDouble(reader["Total_Parenteral"]);
                        objAdvVO.OtherIntake = Convert.ToDouble(reader["OtherIntake"]);
                        objAdvVO.Urine = Convert.ToDouble(reader["Urine"]);
                        objAdvVO.Ng = Convert.ToDouble(reader["Ng"]);
                        objAdvVO.Drain = Convert.ToDouble(reader["Drain"]);
                        objAdvVO.OtherOutput = Convert.ToDouble(reader["OtherOutput"]);
                        objAdvVO.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));

                        BizActionObj.GetIntakeOutputList.Add(objAdvVO);
                    }
                }

                reader.NextResult();
                reader.Close();
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return BizActionObj;
        }

        public override IValueObject UpdateStatusIntakeOutputChart(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateStatusIntakeOutputChartBizActionVO BizActionObj = valueObject as clsUpdateStatusIntakeOutputChartBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateStatusInputOutputChart");

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.IntakeOutputDetails.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.IntakeOutputDetails.UnitID);

                int intStatus = dbServer.ExecuteNonQuery(command);
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        //----------------Added by Ashutosh 21/11/2013--------------//
        public override IValueObject UpdateIsFreezedStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateStatusIntakeOutputChartBizActionVO BizActionObj = valueObject as clsUpdateStatusIntakeOutputChartBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateInatakeOutputChartIsFreezedStatus");

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.IntakeOutputDetails.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.IntakeOutputDetails.UnitID);
                int intStatus = dbServer.ExecuteNonQuery(command);
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }


        #endregion
    }

}
