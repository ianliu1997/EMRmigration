using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.CompoundDrug;
using PalashDynamics.ValueObjects;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects.CompoundDrug;
using System.Data.Common;
using System.Reflection;
using System.Data;
using PalashDynamics.ValueObjects.EMR;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.CompoundDrug
{
    class clsCompoundDrugDAL : clsBaseCompoundDrugDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsCompoundDrugDAL()
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


        public override IValueObject AddCompoundDrug(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddCompoundDrugBizActionVO BizActionObj = valueObject as clsAddCompoundDrugBizActionVO;
            BizActionObj = AddDetails(BizActionObj, UserVo);
            return valueObject;
        }

        private clsAddCompoundDrugBizActionVO AddDetails(clsAddCompoundDrugBizActionVO BizActionObj, clsUserVO UserVO)
        {
            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;

            try
            {
                con.Open();
                trans = con.BeginTransaction();
                clsCompoundDrugMasterVO objCompoundDrug = BizActionObj.CompoundDrug;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddCompoundDrug");
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Code", DbType.String, objCompoundDrug.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, objCompoundDrug.Description);
                dbServer.AddInParameter(command, "LaborPercentage", DbType.Double, objCompoundDrug.LaborPercentage);
                dbServer.AddInParameter(command, "LaborAmount", DbType.Double, objCompoundDrug.LaborAmount);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objCompoundDrug.Status);
                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitId", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objCompoundDrug.AddedDateTime);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVO.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objCompoundDrug.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objCompoundDrug.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                objCompoundDrug.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));

                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                if (BizActionObj.SuccessStatus == 1)
                {
                    foreach (var objCompoundDrugDetail in BizActionObj.CompoundDrugDetailList)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddCompoundDrugItems");

                        dbServer.AddInParameter(command1, "CompoundDrugId", DbType.Int64, objCompoundDrug.ID);
                        dbServer.AddInParameter(command1, "CompoundDrugUnitId", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "ItemID", DbType.Double, objCompoundDrugDetail.ItemID);
                        dbServer.AddInParameter(command1, "ItemUnitID", DbType.Double, objCompoundDrugDetail.ItemUnitID);
                        dbServer.AddInParameter(command1, "Quantity", DbType.Double, objCompoundDrugDetail.Quantity);
                        dbServer.AddInParameter(command1, "UnitID", DbType.Double, UserVO.UserLoginInfo.UnitId);
                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objCompoundDrugDetail.ID);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                    }
                }
                trans.Commit();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                logManager.LogError(UserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                BizActionObj.SuccessStatus = -1;

            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                con = null;
                trans = null;
            }

            return BizActionObj;
        }

        public override IValueObject GetCompoundDrug(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCompoundDrugBizActionVO BizActionObj = valueObject as clsGetCompoundDrugBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;
            DbDataReader reader = null;
            try
            {
                con.Open();
                trans = con.BeginTransaction();
                clsCompoundDrugMasterVO objCompoundDrug = BizActionObj.CompoundDrug;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCompoundDrug");
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "StartRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "MaximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "SearchByCode", DbType.String, BizActionObj.SearchByCode);
                dbServer.AddInParameter(command, "SearchByDescription", DbType.String, BizActionObj.SearchByDescription);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0);
                reader = (DbDataReader)dbServer.ExecuteReader(command, trans);
                if (reader.HasRows)
                {
                    if (BizActionObj.CompoundDrugList == null)
                        BizActionObj.CompoundDrugList = new List<clsCompoundDrugMasterVO>();
                    while (reader.Read())
                    {
                        clsCompoundDrugMasterVO CompoundDrug = new clsCompoundDrugMasterVO();
                        CompoundDrug.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        CompoundDrug.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        CompoundDrug.LaborAmount = (float)Convert.ToDouble(DALHelper.HandleDBNull(reader["LaborAmount"]));
                        CompoundDrug.LaborPercentage = (float)Convert.ToDouble(DALHelper.HandleDBNull(reader["LaborPercentage"]));
                        CompoundDrug.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompoundDrugID"]));
                        CompoundDrug.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizActionObj.CompoundDrugList.Add(CompoundDrug);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRowCount = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (reader.IsClosed == false)
                {
                    reader.Close();
                }
            }
            return valueObject;
        }

        public override IValueObject GetCompoundDrugDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = dbServer.CreateConnection();
            clsGetCompoundDrugDetailsBizActionVO objBizActionVO = null;
            DbTransaction trans = null;
            DbDataReader reader = null;
            try
            {
                con.Open();
                trans = con.BeginTransaction();

                objBizActionVO = valueObject as clsGetCompoundDrugDetailsBizActionVO;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCompoundDrugDetails");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objBizActionVO.CompoundDrug.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizActionVO.CompoundDrug.UnitID);
                dbServer.AddInParameter(command, "ItemID", DbType.String, objBizActionVO.ItemID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, false);
                dbServer.AddInParameter(command, "StartRowIndex", DbType.Int32, 0);
                dbServer.AddInParameter(command, "MaximumRows", DbType.Int32, objBizActionVO.MinRows);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0);
                reader = (DbDataReader)dbServer.ExecuteReader(command, trans);
                if (reader.HasRows)
                {
                    if (objBizActionVO.CompoundDrugDetailList == null)
                        objBizActionVO.CompoundDrugDetailList = new List<clsCompoundDrugDetailVO>();
                    while (reader.Read())
                    {
                        clsCompoundDrugDetailVO objCompoundDrugDetail = new clsCompoundDrugDetailVO();
                        objCompoundDrugDetail.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objCompoundDrugDetail.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objCompoundDrugDetail.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objCompoundDrugDetail.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objCompoundDrugDetail.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        objCompoundDrugDetail.ItemUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemUnitID"]));
                        objCompoundDrugDetail.Quantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["Quantity"]));
                        objCompoundDrugDetail.CompoundDrugID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompoundDrugID"]));
                        objCompoundDrugDetail.CompoundDrugUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompoundDrugUnitID"]));
                        objCompoundDrugDetail.CompoundDrug = Convert.ToString(DALHelper.HandleDBNull(reader["CompoundDrug"]));
                        objCompoundDrugDetail.AvailableStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        objBizActionVO.CompoundDrugDetailList.Add(objCompoundDrugDetail);
                    }
                }
                reader.NextResult();
                objBizActionVO.TotalRowCount = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (reader.IsClosed == false)
                {
                    reader.Close();
                }
            }
            return valueObject;
        }

        public override IValueObject UpdateCompoundDrugByIDAndUnitID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateCompoundDrugBizActionVO objBizActionVO = valueObject as clsUpdateCompoundDrugBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;

            try
            {
                con.Open();
                trans = con.BeginTransaction();
                clsCompoundDrugMasterVO objCompoundDrug = objBizActionVO.CompoundDrug;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateCompoundDrug");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objCompoundDrug.ID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objCompoundDrug.UnitID);
                dbServer.AddInParameter(command, "Code", DbType.String, objCompoundDrug.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, objCompoundDrug.Description);
                dbServer.AddInParameter(command, "LaborPercentage", DbType.Double, objCompoundDrug.LaborPercentage);
                dbServer.AddInParameter(command, "LaborAmount", DbType.Double, objCompoundDrug.LaborAmount);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objCompoundDrug.Status);
                dbServer.AddInParameter(command, "UpdatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                objBizActionVO.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                if (objBizActionVO.SuccessStatus == 1 && objBizActionVO.CompoundDrugDetailList.Count > 0)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_UpdateCompoundDrugItems");

                    dbServer.AddInParameter(command1, "CompoundDrugId", DbType.Int64, objCompoundDrug.ID);
                    dbServer.AddInParameter(command1, "CompoundDrugUnitId", DbType.Int64, objCompoundDrug.UnitID);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);


                }
                if (objBizActionVO.SuccessStatus == 1)
                {
                    foreach (var objCompoundDrugDetail in objBizActionVO.CompoundDrugDetailList)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddCompoundDrugItems");

                        dbServer.AddInParameter(command1, "CompoundDrugId", DbType.Int64, objCompoundDrug.ID);
                        dbServer.AddInParameter(command1, "CompoundDrugUnitId", DbType.Int64, objCompoundDrug.UnitID);
                        dbServer.AddInParameter(command1, "ItemID", DbType.Double, objCompoundDrugDetail.ItemID);
                        dbServer.AddInParameter(command1, "ItemUnitID", DbType.Double, objCompoundDrugDetail.ItemUnitID);
                        dbServer.AddInParameter(command1, "Quantity", DbType.Double, objCompoundDrugDetail.Quantity);
                        dbServer.AddInParameter(command1, "UnitID", DbType.Double, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    }
                }

                trans.Commit();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                objBizActionVO.SuccessStatus = -1;

            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                con = null;
                trans = null;
            }

            return objBizActionVO;
        }

        public override IValueObject AddPatientCompoundDrug(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientCompoundPrescriptionBizActionVO objBizActionVO = (clsAddPatientCompoundPrescriptionBizActionVO)valueObject;
            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;

            try
            {
                con.Open();
                trans = con.BeginTransaction();
                clsPatientCompoundPrescriptionVO objPatientCompoundPrescription = objBizActionVO.PatientCompoundPrescription;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPatientCompoundPrescription");
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PrescriptionID", DbType.Int64, objPatientCompoundPrescription.PrescriptionID);
                dbServer.AddInParameter(command, "CompoundDrugID", DbType.Int64, objPatientCompoundPrescription.CompoundDrugID);
                dbServer.AddInParameter(command, "CompoundDrugUnitID", DbType.Int64, objPatientCompoundPrescription.CompoundDrugUnitID);
                dbServer.AddInParameter(command, "ItemID", DbType.Int64, objPatientCompoundPrescription.ItemID);
                dbServer.AddInParameter(command, "Reason", DbType.String, objPatientCompoundPrescription.Reason);
                dbServer.AddInParameter(command, "ComponentQuantity", DbType.Single, objPatientCompoundPrescription.ComponentQuantity);
                dbServer.AddInParameter(command, "Rate", DbType.Single, objPatientCompoundPrescription.Rate);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientCompoundPrescription.ID);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                if (intStatus < 1)
                {
                    InvalidOperationException ex = new InvalidOperationException();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                objBizActionVO.SuccessStatus = -1;

            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                con = null;
                trans = null;
            }
            return objBizActionVO;
        }

        public override IValueObject GetPatientPrescriptionCompoundDrugByVisitID(IValueObject valueObject, clsUserVO UserVo)
        {
            // objBizAction = (clsGetPatietPrescriptionCompoundDrugBizActionVO)valueObject;
            DbConnection con = dbServer.CreateConnection();
            clsGetPatietPrescriptionCompoundDrugBizActionVO objBizAction = null;
            DbTransaction trans = null;
            DbDataReader reader = null;
            try
            {
                con.Open();
                trans = con.BeginTransaction();

                objBizAction = valueObject as clsGetPatietPrescriptionCompoundDrugBizActionVO;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientPrescriptionCompoundDrugByVisitID");


                dbServer.AddInParameter(command, "VisitID", DbType.Int64, objBizAction.VisitID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                {
                    if (objBizAction.PatientPrescriptionCompoundDrugList == null)
                        objBizAction.PatientPrescriptionCompoundDrugList = new List<clsPatientPrescriptionDetailVO>();

                    while (reader.Read())
                    {
                        clsPatientPrescriptionDetailVO objPrescriptionVO = new clsPatientPrescriptionDetailVO();

                        objPrescriptionVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objPrescriptionVO.PrescriptionID = (long)DALHelper.HandleDBNull(reader["PrescriptionID"]);
                        objPrescriptionVO.DrugID = (long)DALHelper.HandleDBNull(reader["DrugID"]);
                        objPrescriptionVO.DrugName = (string)DALHelper.HandleDBNull(reader["DrugName"]);
                        objPrescriptionVO.Dose = (string)DALHelper.HandleDBNull(reader["Dose"]);
                        objPrescriptionVO.Route = (string)DALHelper.HandleDBNull(reader["Route"]);
                        objPrescriptionVO.Frequency = (string)DALHelper.HandleDBNull(reader["Frequency"]);
                        objPrescriptionVO.Days = (int)DALHelper.HandleDBNull(reader["Days"]);
                        objPrescriptionVO.Quantity = (double)DALHelper.HandleDoubleNull(reader["Quantity"]);
                        objPrescriptionVO.IsBatchRequired = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"]));
                        objPrescriptionVO.CompoundDrug = Convert.ToString(DALHelper.HandleDBNull(reader["CompoundDrug"]));
                        objPrescriptionVO.LabourChargeAmt = (float?)(DALHelper.HandleDoubleNull(reader["LaborAmount"]));
                        objPrescriptionVO.LabourChargePer = (float?)(DALHelper.HandleDoubleNull(reader["LaborPercentage"]));
                        objPrescriptionVO.CompoundDrugID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["CompoundDrugID"]));
                        objPrescriptionVO.CompoundDrugUnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["CompoundDrugUnitID"]));
                        objPrescriptionVO.InclusiveOfTax = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["InclusiveOfTax"]));
                        objBizAction.PatientPrescriptionCompoundDrugList.Add(objPrescriptionVO);
                    }
                }
                reader.NextResult();
                //objBizAction.TotalRowCount = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (reader.IsClosed == false)
                {
                    reader.Close();
                }
            }

            return valueObject;
        }

        public override IValueObject GetCompoundDrugAndDetailsByIDAndUnitID(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = dbServer.CreateConnection();
            clsGetCompoundDrugAndDetailsByIDandUnitIDBizActionVO objBizActionVO = null;
            DbTransaction trans = null;
            DbDataReader reader = null;
            try
            {
                con.Open();
                trans = con.BeginTransaction();

                objBizActionVO = valueObject as clsGetCompoundDrugAndDetailsByIDandUnitIDBizActionVO;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCompoundDrugAndDetailsByIDandUnitID");
                dbServer.AddInParameter(command, "CompoundDrugID", DbType.Int64, objBizActionVO.CompoundDrugID);
                dbServer.AddInParameter(command, "CompoundDrugUnitID", DbType.Int64, objBizActionVO.CompoundDrugUnitID);
                dbServer.AddInParameter(command, "PrescriptionID", DbType.Int64, objBizActionVO.PrescriptionID);
                reader = (DbDataReader)dbServer.ExecuteReader(command,trans);
                if (reader.HasRows)
                {
                    objBizActionVO.CompoundDrugDetailList = new List<clsCompoundDrugDetailVO>();
                    objBizActionVO.CompoundDrugMaster = new clsCompoundDrugMasterVO();
                    int iCount = 0;
                    while (reader.Read())
                    {
                        clsCompoundDrugDetailVO objCompoundDrugDetail = new clsCompoundDrugDetailVO();
                        objCompoundDrugDetail.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objCompoundDrugDetail.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objCompoundDrugDetail.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objCompoundDrugDetail.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objCompoundDrugDetail.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        objCompoundDrugDetail.ItemUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemUnitID"]));
                        objCompoundDrugDetail.Quantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["Quantity"]));
                        if (iCount == 0)
                        {
                            objBizActionVO.CompoundDrugMaster.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompoundDrugID"]));
                            objBizActionVO.CompoundDrugMaster.Code = Convert.ToString(DALHelper.HandleDBNull(reader["CompoundDrugCode"]));
                            objBizActionVO.CompoundDrugMaster.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompoundDrugUnitID"]));
                            objBizActionVO.CompoundDrugMaster.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                            objBizActionVO.CompoundDrugMaster.LaborAmount = Convert.ToSingle(DALHelper.HandleDBNull(reader["LabourAmount"]));
                            objBizActionVO.CompoundDrugMaster.LaborPercentage = Convert.ToSingle(DALHelper.HandleDBNull(reader["LabourPercentage"]));
                        }
                        iCount++;
                        objBizActionVO.CompoundDrugDetailList.Add(objCompoundDrugDetail);
                    }
                }
                reader.NextResult();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                trans = null;
                con.Close();
                if (reader.IsClosed == false)
                {
                    reader.Close();
                }
            }
            return valueObject;
        }

        public override IValueObject CheckCompoundDrug(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCheckCompoundDrugBizActionVO BizAction = valueObject as clsCheckCompoundDrugBizActionVO;
            
            DbCommand command = dbServer.GetStoredProcCommand("CIMS_CheckCompoundDrug");
            dbServer.AddInParameter(command, "Description", DbType.String, BizAction.CompoundDrug.Description);          
            
            DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    BizAction.SuccessStatus = 1;
                }
            }

            return BizAction;

        }
    }
}
