namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.IPD;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsIPDIntakeOutputChartDAL : clsBaseIPDIntakeOutputChartDAL
    {
        private Database dbServer;

        private clsIPDIntakeOutputChartDAL()
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

        public override IValueObject AddUpdateIntakeOutputChart(IValueObject valueObject, clsUserVO UserVO)
        {
            clsAddUpdateIntakeOutputChartAndDetailsBizActionVO nvo = valueObject as clsAddUpdateIntakeOutputChartAndDetailsBizActionVO;
            try
            {
                clsIPDIntakeOutputChartVO intakeOutputDetails = nvo.IntakeOutputDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateIntakeOutputChart");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, intakeOutputDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, intakeOutputDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AdmID", DbType.Int64, intakeOutputDetails.AdmID);
                this.dbServer.AddInParameter(storedProcCommand, "AdmUnitID", DbType.Int64, intakeOutputDetails.AdmUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, intakeOutputDetails.Date);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, intakeOutputDetails.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, intakeOutputDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, intakeOutputDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, intakeOutputDetails.UpdatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, intakeOutputDetails.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int32, intakeOutputDetails.UpdatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, intakeOutputDetails.UpdatedOn);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, intakeOutputDetails.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, intakeOutputDetails.UpdateWindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.IntakeOutputDetails.ID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
                if ((nvo.IntakeOutputList != null) && (nvo.IntakeOutputList.Count > 0))
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteIntakeOutputChartDetail");
                    this.dbServer.AddInParameter(command2, "IntakeOutputID", DbType.Int64, nvo.IntakeOutputDetails.ID);
                    this.dbServer.AddInParameter(command2, "IntakeOutputIDUnitID", DbType.Int64, nvo.IntakeOutputDetails.UnitID);
                    this.dbServer.ExecuteNonQuery(command2);
                    foreach (clsIPDIntakeOutputChartVO tvo2 in nvo.IntakeOutputList)
                    {
                        this.AddUpdateIntakeOutputChartDetail(tvo2, nvo.IntakeOutputDetails);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public void AddUpdateIntakeOutputChartDetail(clsIPDIntakeOutputChartVO ObjIntOutVO, clsIPDIntakeOutputChartVO IntakeOutputDetails)
        {
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateIntakeOutputChartDetail");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, ObjIntOutVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, IntakeOutputDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IntakeOutputID", DbType.Int64, IntakeOutputDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "IntakeOutputIDUnitID", DbType.Int64, IntakeOutputDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, IntakeOutputDetails.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.String, ObjIntOutVO.strTime);
                this.dbServer.AddInParameter(storedProcCommand, "Oral", DbType.Double, ObjIntOutVO.Oral);
                this.dbServer.AddInParameter(storedProcCommand, "Total_Parenteral", DbType.Double, ObjIntOutVO.Total_Parenteral);
                this.dbServer.AddInParameter(storedProcCommand, "OtherIntake", DbType.Double, ObjIntOutVO.OtherIntake);
                this.dbServer.AddInParameter(storedProcCommand, "Urine", DbType.Double, ObjIntOutVO.Urine);
                this.dbServer.AddInParameter(storedProcCommand, "Ng", DbType.Double, ObjIntOutVO.Ng);
                this.dbServer.AddInParameter(storedProcCommand, "Drain", DbType.Double, ObjIntOutVO.Drain);
                this.dbServer.AddInParameter(storedProcCommand, "OtherOutput", DbType.Double, ObjIntOutVO.Drain);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override IValueObject GetIntakeOutputChartDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIntakeOutputChartDetailsBizActionVO nvo = valueObject as clsGetIntakeOutputChartDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIntakeOutputChartDetail");
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.GetIntakeOutputDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.GetIntakeOutputDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.GetIntakeOutputDetails.InputPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.GetIntakeOutputDetails.InputStartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.GetIntakeOutputDetails.InputMaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.GetIntakeOutputList == null)
                    {
                        nvo.GetIntakeOutputList = new List<clsIPDIntakeOutputChartVO>();
                    }
                    while (reader.Read())
                    {
                        clsIPDIntakeOutputChartVO item = new clsIPDIntakeOutputChartVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            AdmID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmID"])),
                            AdmUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmUnitID"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"])),
                            Date = (DateTime?) DALHelper.HandleDBNull(reader["Date"]),
                            Status = (bool) reader["Status"],
                            IntakeTotal = (double) reader["TotalIntake"],
                            OutputTotal = (double) reader["TotalOutput"],
                            IsFreezed = new bool?(Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"])))
                        };
                        nvo.GetIntakeOutputList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetIntakeOutputChartDetailsByPatientID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIntakeOutputChartDetailsByPatientIDBizActionVO nvo = valueObject as clsGetIntakeOutputChartDetailsByPatientIDBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIntakeOutputChartDetailbyPatientID");
                this.dbServer.AddInParameter(storedProcCommand, "IntakeOutputID", DbType.Int64, nvo.GetIntakeOutputDetails.IntakeOutputID);
                this.dbServer.AddInParameter(storedProcCommand, "IntakeOutputIDUnitID", DbType.Int64, nvo.GetIntakeOutputDetails.IntakeOutputIDUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.GetIntakeOutputDetails.Date);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.GetIntakeOutputList == null)
                    {
                        nvo.GetIntakeOutputList = new List<clsIPDIntakeOutputChartVO>();
                    }
                    while (reader.Read())
                    {
                        clsIPDIntakeOutputChartVO item = new clsIPDIntakeOutputChartVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            IntakeOutputID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IntakeOutputID"])),
                            IntakeOutputIDUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IntakeOutputIDUnitID"])),
                            Date = (DateTime?) DALHelper.HandleDBNull(reader["Date"]),
                            strTime = Convert.ToString(DALHelper.HandleDBNull(reader["Time"])),
                            Oral = Convert.ToDouble(reader["Oral"]),
                            Total_Parenteral = Convert.ToDouble(reader["Total_Parenteral"]),
                            OtherIntake = Convert.ToDouble(reader["OtherIntake"]),
                            Urine = Convert.ToDouble(reader["Urine"]),
                            Ng = Convert.ToDouble(reader["Ng"]),
                            Drain = Convert.ToDouble(reader["Drain"]),
                            OtherOutput = Convert.ToDouble(reader["OtherOutput"]),
                            IsFreezed = new bool?(Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"])))
                        };
                        nvo.GetIntakeOutputList.Add(item);
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

        public override IValueObject UpdateIsFreezedStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateStatusIntakeOutputChartBizActionVO nvo = valueObject as clsUpdateStatusIntakeOutputChartBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateInatakeOutputChartIsFreezedStatus");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.IntakeOutputDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.IntakeOutputDetails.UnitID);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject UpdateStatusIntakeOutputChart(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateStatusIntakeOutputChartBizActionVO nvo = valueObject as clsUpdateStatusIntakeOutputChartBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateStatusInputOutputChart");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.IntakeOutputDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.IntakeOutputDetails.UnitID);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }
    }
}

