namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration.PatientSourceMaster;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class clsRegistrationChargesMasterDAL : clsBaseRegistrationChargesMasterMasterDAL
    {
        private Database dbServer;

        public clsRegistrationChargesMasterDAL()
        {
            try
            {
                if (this.dbServer == null)
                {
                    this.dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private clsAddRegistrationChargesBizActionVO AddPatientSourceMaster(clsAddRegistrationChargesBizActionVO BizActionObj, clsUserVO objUserVO)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsRegistrationChargesVO patientDetails = BizActionObj.PatientDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddRegistrationCharges");
                this.dbServer.AddInParameter(storedProcCommand, "PatientTypeId", DbType.Int64, patientDetails.PatientTypeId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientServiceId", DbType.Int64, patientDetails.PatientServiceId);
                this.dbServer.AddInParameter(storedProcCommand, "Rate", DbType.Double, patientDetails.Rate);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, patientDetails.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "Synchronized", DbType.Boolean, false);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.PatientDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                BizActionObj.SuccessStatus = (long) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.ResultSuccessStatus = BizActionObj.SuccessStatus;
                transaction.Commit();
                BizActionObj.ResultSuccessStatus = 0L;
            }
            catch (Exception)
            {
                BizActionObj.ResultSuccessStatus = -1L;
                transaction.Rollback();
                BizActionObj.PatientDetails = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return BizActionObj;
        }

        public override IValueObject AddRegistartionChargesMaster(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddRegistrationChargesBizActionVO bizActionObj = valueObject as clsAddRegistrationChargesBizActionVO;
            return ((bizActionObj.PatientDetails.ID != 0L) ? this.UpdatePatientSourceMaster(bizActionObj, objUserVO) : this.AddPatientSourceMaster(bizActionObj, objUserVO));
        }

        public override IValueObject GetPatientSourceList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetRegistrationChargesListBizActionVO nvo = (clsGetRegistrationChargesListBizActionVO) valueObject;
            try
            {
                if (!nvo.ValidPatientMasterSourceList)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetRegistrationChargesList");
                    this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                    this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                    this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.PatientSourceDetails == null)
                        {
                            nvo.PatientSourceDetails = new List<clsRegistrationChargesVO>();
                        }
                        while (true)
                        {
                            if (!reader.Read())
                            {
                                reader.NextResult();
                                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                                reader.Close();
                                break;
                            }
                            clsRegistrationChargesVO item = new clsRegistrationChargesVO {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                                PatientCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientCategoryID"])),
                                PatientTypeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientTypeId"])),
                                PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"])),
                                PatientServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientServiceId"])),
                                PatientServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"])),
                                Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]))
                            };
                            nvo.PatientSourceDetails.Add(item);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetRegistrationChargesByID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetRegistrationChargesDetailsByIDBizActionVO nvo = (clsGetRegistrationChargesDetailsByIDBizActionVO) valueObject;
            try
            {
                clsRegistrationChargesVO details = nvo.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetRegistrationChargesDetailsByID");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (nvo.Details == null)
                        {
                            nvo.Details = new clsRegistrationChargesVO();
                        }
                        nvo.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.Details.PatientCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientCategoryID"]));
                        nvo.Details.PatientServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientServiceId"]));
                        nvo.Details.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                        nvo.Details.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        nvo.Details.FromDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["FromDate"])));
                        nvo.Details.ToDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["ToDate"])));
                    }
                }
                reader.NextResult();
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetRegistrationChargesByPatientTypeID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetRegistrationChargesDetailsByPatientTypeIDBizActionVO nvo = (clsGetRegistrationChargesDetailsByPatientTypeIDBizActionVO) valueObject;
            try
            {
                StringBuilder builder1 = new StringBuilder();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetRegistartionTypeChargesByPatientType");
                this.dbServer.AddInParameter(storedProcCommand, "PatientTypeID", DbType.Int64, nvo.PatientTypeID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsRegistrationChargesVO>();
                    }
                    while (reader.Read())
                    {
                        clsRegistrationChargesVO item = new clsRegistrationChargesVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            PatientServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientServiceId"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.List.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        private clsAddRegistrationChargesBizActionVO UpdatePatientSourceMaster(clsAddRegistrationChargesBizActionVO BizActionObj, clsUserVO objUserVO)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsRegistrationChargesVO patientDetails = BizActionObj.PatientDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateRagistrationCharges");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, patientDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientTypeId", DbType.Int64, patientDetails.PatientTypeId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientServiceId", DbType.Int64, patientDetails.PatientServiceId);
                this.dbServer.AddInParameter(storedProcCommand, "Rate", DbType.Double, patientDetails.Rate);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, patientDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, patientDetails.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (long) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                transaction.Commit();
                BizActionObj.SuccessStatus = 0L;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1L;
                transaction.Rollback();
                BizActionObj.PatientDetails = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return BizActionObj;
        }
    }
}

