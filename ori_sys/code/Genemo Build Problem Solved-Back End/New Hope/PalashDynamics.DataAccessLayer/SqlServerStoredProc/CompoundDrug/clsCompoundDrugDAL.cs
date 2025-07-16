namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.CompoundDrug
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.CompoundDrug;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.CompoundDrug;
    using PalashDynamics.ValueObjects.EMR;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Reflection;

    internal class clsCompoundDrugDAL : clsBaseCompoundDrugDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsCompoundDrugDAL()
        {
            try
            {
                if (this.dbServer == null)
                {
                    this.dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
                if (this.logManager == null)
                {
                    this.logManager = LogManager.GetInstance();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override IValueObject AddCompoundDrug(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddCompoundDrugBizActionVO bizActionObj = valueObject as clsAddCompoundDrugBizActionVO;
            bizActionObj = this.AddDetails(bizActionObj, UserVo);
            return valueObject;
        }

        private clsAddCompoundDrugBizActionVO AddDetails(clsAddCompoundDrugBizActionVO BizActionObj, clsUserVO UserVO)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                clsCompoundDrugMasterVO compoundDrug = BizActionObj.CompoundDrug;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddCompoundDrug");
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, compoundDrug.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, compoundDrug.Description);
                this.dbServer.AddInParameter(storedProcCommand, "LaborPercentage", DbType.Double, compoundDrug.LaborPercentage);
                this.dbServer.AddInParameter(storedProcCommand, "LaborAmount", DbType.Double, compoundDrug.LaborAmount);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, compoundDrug.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitId", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, compoundDrug.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, compoundDrug.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, compoundDrug.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                compoundDrug.ID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if (BizActionObj.SuccessStatus == 1)
                {
                    foreach (clsCompoundDrugDetailVO lvo in BizActionObj.CompoundDrugDetailList)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddCompoundDrugItems");
                        this.dbServer.AddInParameter(command2, "CompoundDrugId", DbType.Int64, compoundDrug.ID);
                        this.dbServer.AddInParameter(command2, "CompoundDrugUnitId", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "ItemID", DbType.Double, lvo.ItemID);
                        this.dbServer.AddInParameter(command2, "ItemUnitID", DbType.Double, lvo.ItemUnitID);
                        this.dbServer.AddInParameter(command2, "Quantity", DbType.Double, lvo.Quantity);
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Double, UserVO.UserLoginInfo.UnitId);
                        this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, lvo.ID);
                        this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command2, transaction);
                    }
                }
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(UserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                BizActionObj.SuccessStatus = -1;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection = null;
                transaction = null;
            }
            return BizActionObj;
        }

        public override IValueObject AddPatientCompoundDrug(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientCompoundPrescriptionBizActionVO nvo = (clsAddPatientCompoundPrescriptionBizActionVO) valueObject;
            DbConnection connection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            try
            {
                connection.Open();
                clsPatientCompoundPrescriptionVO patientCompoundPrescription = nvo.PatientCompoundPrescription;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatientCompoundPrescription");
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PrescriptionID", DbType.Int64, patientCompoundPrescription.PrescriptionID);
                this.dbServer.AddInParameter(storedProcCommand, "CompoundDrugID", DbType.Int64, patientCompoundPrescription.CompoundDrugID);
                this.dbServer.AddInParameter(storedProcCommand, "CompoundDrugUnitID", DbType.Int64, patientCompoundPrescription.CompoundDrugUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, patientCompoundPrescription.ItemID);
                this.dbServer.AddInParameter(storedProcCommand, "Reason", DbType.String, patientCompoundPrescription.Reason);
                this.dbServer.AddInParameter(storedProcCommand, "ComponentQuantity", DbType.Single, patientCompoundPrescription.ComponentQuantity);
                this.dbServer.AddInParameter(storedProcCommand, "Rate", DbType.Single, patientCompoundPrescription.Rate);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientCompoundPrescription.ID);
                if (this.dbServer.ExecuteNonQuery(storedProcCommand, connection.BeginTransaction()) < 1)
                {
                    throw new InvalidOperationException();
                }
            }
            catch (Exception exception2)
            {
                transaction.Rollback();
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception2.ToString());
                nvo.SuccessStatus = -1;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection = null;
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject CheckCompoundDrug(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCheckCompoundDrugBizActionVO nvo = valueObject as clsCheckCompoundDrugBizActionVO;
            DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CheckCompoundDrug");
            this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.CompoundDrug.Description);
            DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    nvo.SuccessStatus = 1;
                }
            }
            return nvo;
        }

        public override IValueObject GetCompoundDrug(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCompoundDrugBizActionVO nvo = valueObject as clsGetCompoundDrugBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            DbDataReader reader = null;
            try
            {
                connection.Open();
                clsCompoundDrugMasterVO compoundDrug = nvo.CompoundDrug;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCompoundDrug");
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "StartRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "MaximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "SearchByCode", DbType.String, nvo.SearchByCode);
                this.dbServer.AddInParameter(storedProcCommand, "SearchByDescription", DbType.String, nvo.SearchByDescription);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand, connection.BeginTransaction());
                if (reader.HasRows)
                {
                    if (nvo.CompoundDrugList == null)
                    {
                        nvo.CompoundDrugList = new List<clsCompoundDrugMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsCompoundDrugMasterVO item = new clsCompoundDrugMasterVO {
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            LaborAmount = new float?((float) Convert.ToDouble(DALHelper.HandleDBNull(reader["LaborAmount"]))),
                            LaborPercentage = new float?((float) Convert.ToDouble(DALHelper.HandleDBNull(reader["LaborPercentage"]))),
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompoundDrugID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]))
                        };
                        nvo.CompoundDrugList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRowCount = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return valueObject;
        }

        public override IValueObject GetCompoundDrugAndDetailsByIDAndUnitID(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            clsGetCompoundDrugAndDetailsByIDandUnitIDBizActionVO nvo = null;
            DbTransaction transaction = null;
            DbDataReader reader = null;
            try
            {
                connection.Open();
                nvo = valueObject as clsGetCompoundDrugAndDetailsByIDandUnitIDBizActionVO;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCompoundDrugAndDetailsByIDandUnitID");
                this.dbServer.AddInParameter(storedProcCommand, "CompoundDrugID", DbType.Int64, nvo.CompoundDrugID);
                this.dbServer.AddInParameter(storedProcCommand, "CompoundDrugUnitID", DbType.Int64, nvo.CompoundDrugUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PrescriptionID", DbType.Int64, nvo.PrescriptionID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand, connection.BeginTransaction());
                if (reader.HasRows)
                {
                    nvo.CompoundDrugDetailList = new List<clsCompoundDrugDetailVO>();
                    nvo.CompoundDrugMaster = new clsCompoundDrugMasterVO();
                    int num = 0;
                    while (reader.Read())
                    {
                        clsCompoundDrugDetailVO item = new clsCompoundDrugDetailVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                            ItemUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemUnitID"])),
                            Quantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["Quantity"]))
                        };
                        if (num == 0)
                        {
                            nvo.CompoundDrugMaster.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompoundDrugID"]));
                            nvo.CompoundDrugMaster.Code = Convert.ToString(DALHelper.HandleDBNull(reader["CompoundDrugCode"]));
                            nvo.CompoundDrugMaster.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompoundDrugUnitID"]));
                            nvo.CompoundDrugMaster.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                            nvo.CompoundDrugMaster.LaborAmount = new float?(Convert.ToSingle(DALHelper.HandleDBNull(reader["LabourAmount"])));
                            nvo.CompoundDrugMaster.LaborPercentage = new float?(Convert.ToSingle(DALHelper.HandleDBNull(reader["LabourPercentage"])));
                        }
                        num++;
                        nvo.CompoundDrugDetailList.Add(item);
                    }
                }
                reader.NextResult();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            finally
            {
                transaction = null;
                connection.Close();
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return valueObject;
        }

        public override IValueObject GetCompoundDrugDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            clsGetCompoundDrugDetailsBizActionVO nvo = null;
            DbDataReader reader = null;
            try
            {
                connection.Open();
                nvo = valueObject as clsGetCompoundDrugDetailsBizActionVO;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCompoundDrugDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.CompoundDrug.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.CompoundDrug.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.String, nvo.ItemID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, false);
                this.dbServer.AddInParameter(storedProcCommand, "StartRowIndex", DbType.Int32, 0);
                this.dbServer.AddInParameter(storedProcCommand, "MaximumRows", DbType.Int32, nvo.MinRows);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand, connection.BeginTransaction());
                if (reader.HasRows)
                {
                    if (nvo.CompoundDrugDetailList == null)
                    {
                        nvo.CompoundDrugDetailList = new List<clsCompoundDrugDetailVO>();
                    }
                    while (reader.Read())
                    {
                        clsCompoundDrugDetailVO item = new clsCompoundDrugDetailVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                            ItemUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemUnitID"])),
                            Quantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["Quantity"])),
                            CompoundDrugID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompoundDrugID"])),
                            CompoundDrugUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompoundDrugUnitID"])),
                            CompoundDrug = Convert.ToString(DALHelper.HandleDBNull(reader["CompoundDrug"])),
                            AvailableStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"]))
                        };
                        nvo.CompoundDrugDetailList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRowCount = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return valueObject;
        }

        public override IValueObject GetPatientPrescriptionCompoundDrugByVisitID(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            clsGetPatietPrescriptionCompoundDrugBizActionVO nvo = null;
            DbDataReader reader = null;
            try
            {
                connection.Open();
                connection.BeginTransaction();
                nvo = valueObject as clsGetPatietPrescriptionCompoundDrugBizActionVO;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientPrescriptionCompoundDrugByVisitID");
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (nvo.PatientPrescriptionCompoundDrugList == null)
                {
                    nvo.PatientPrescriptionCompoundDrugList = new List<clsPatientPrescriptionDetailVO>();
                }
                while (true)
                {
                    if (!reader.Read())
                    {
                        reader.NextResult();
                        break;
                    }
                    clsPatientPrescriptionDetailVO item = new clsPatientPrescriptionDetailVO {
                        ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                        PrescriptionID = (long) DALHelper.HandleDBNull(reader["PrescriptionID"]),
                        DrugID = (long) DALHelper.HandleDBNull(reader["DrugID"]),
                        DrugName = (string) DALHelper.HandleDBNull(reader["DrugName"]),
                        Dose = (string) DALHelper.HandleDBNull(reader["Dose"]),
                        Route = (string) DALHelper.HandleDBNull(reader["Route"]),
                        Frequency = (string) DALHelper.HandleDBNull(reader["Frequency"]),
                        Days = new int?((int) DALHelper.HandleDBNull(reader["Days"])),
                        Quantity = DALHelper.HandleDoubleNull(reader["Quantity"]),
                        IsBatchRequired = new bool?(Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"]))),
                        CompoundDrug = Convert.ToString(DALHelper.HandleDBNull(reader["CompoundDrug"])),
                        LabourChargeAmt = new float?((float) DALHelper.HandleDoubleNull(reader["LaborAmount"])),
                        LabourChargePer = new float?((float) DALHelper.HandleDoubleNull(reader["LaborPercentage"])),
                        CompoundDrugID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["CompoundDrugID"])),
                        CompoundDrugUnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["CompoundDrugUnitID"])),
                        InclusiveOfTax = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["InclusiveOfTax"]))
                    };
                    nvo.PatientPrescriptionCompoundDrugList.Add(item);
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return valueObject;
        }

        public override IValueObject UpdateCompoundDrugByIDAndUnitID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateCompoundDrugBizActionVO nvo = valueObject as clsUpdateCompoundDrugBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                clsCompoundDrugMasterVO compoundDrug = nvo.CompoundDrug;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateCompoundDrug");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, compoundDrug.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, compoundDrug.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, compoundDrug.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, compoundDrug.Description);
                this.dbServer.AddInParameter(storedProcCommand, "LaborPercentage", DbType.Double, compoundDrug.LaborPercentage);
                this.dbServer.AddInParameter(storedProcCommand, "LaborAmount", DbType.Double, compoundDrug.LaborAmount);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, compoundDrug.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if ((nvo.SuccessStatus == 1) && (nvo.CompoundDrugDetailList.Count > 0))
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_UpdateCompoundDrugItems");
                    this.dbServer.AddInParameter(command2, "CompoundDrugId", DbType.Int64, compoundDrug.ID);
                    this.dbServer.AddInParameter(command2, "CompoundDrugUnitId", DbType.Int64, compoundDrug.UnitID);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                }
                if (nvo.SuccessStatus == 1)
                {
                    foreach (clsCompoundDrugDetailVO lvo in nvo.CompoundDrugDetailList)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddCompoundDrugItems");
                        this.dbServer.AddInParameter(command3, "CompoundDrugId", DbType.Int64, compoundDrug.ID);
                        this.dbServer.AddInParameter(command3, "CompoundDrugUnitId", DbType.Int64, compoundDrug.UnitID);
                        this.dbServer.AddInParameter(command3, "ItemID", DbType.Double, lvo.ItemID);
                        this.dbServer.AddInParameter(command3, "ItemUnitID", DbType.Double, lvo.ItemUnitID);
                        this.dbServer.AddInParameter(command3, "Quantity", DbType.Double, lvo.Quantity);
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Double, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                        this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                    }
                }
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                nvo.SuccessStatus = -1;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection = null;
                transaction = null;
            }
            return nvo;
        }
    }
}

