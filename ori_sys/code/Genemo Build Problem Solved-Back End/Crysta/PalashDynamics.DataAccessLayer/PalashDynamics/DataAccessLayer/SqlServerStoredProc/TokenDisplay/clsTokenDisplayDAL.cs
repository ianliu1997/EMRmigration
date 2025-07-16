namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.TokenDisplay
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.TokenDisplay;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.TokenDisplay;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsTokenDisplayDAL : clsBaseTokenDisplayDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsTokenDisplayDAL()
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

        public override IValueObject AddUpdateTokenDisplayDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateTokenDisplayBizActionVO nvo = valueObject as clsAddUpdateTokenDisplayBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateTokenDisplay");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientId", DbType.Int64, nvo.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "VisitId", DbType.Int64, nvo.VisitId);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitDate", DbType.DateTime, nvo.VisitDate);
                this.dbServer.AddInParameter(storedProcCommand, "IsDisplay", DbType.Boolean, nvo.IsDisplay);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.Id);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int64, ParameterDirection.Output, null, DataRowVersion.Default, nvo.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (long) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.Id = (long) this.dbServer.GetParameterValue(storedProcCommand, "Id");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPatientTokenDisplayDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetTokenDisplayPatirntDetailsBizActionVO nvo = valueObject as clsGetTokenDisplayPatirntDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetTokenDisplayPatientDetailsForStatus");
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientId", DbType.Int64, nvo.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        nvo.IsDisplay = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetTokenDisplayDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetTokenDisplayBizActionVO nvo = valueObject as clsGetTokenDisplayBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetTokenDisplayDetails");
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "VisitDate", DbType.DateTime, nvo.VisitDate);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ListTokenDisplay == null)
                    {
                        nvo.ListTokenDisplay = new List<clsTokenDisplayVO>();
                    }
                    int num = 1;
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        clsTokenDisplayVO item = new clsTokenDisplayVO {
                            ID = num,
                            MrNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"])),
                            PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["Patient Name"])),
                            DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"])),
                            Cabin = Convert.ToString(DALHelper.HandleDBNull(reader["Cabin"])),
                            TokenNo = Convert.ToString(DALHelper.HandleDBNull(reader["TokenNO"]))
                        };
                        nvo.ListTokenDisplay.Add(item);
                        num++;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject UpdateStatusTokenDisplay(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateTokenDisplayStatusBizActionVO nvo = valueObject as clsUpdateTokenDisplayStatusBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateTokenDisplayStatus");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientId", DbType.Int64, nvo.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "VisitId", DbType.Int64, nvo.VisitId);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "IsDisplay", DbType.Boolean, nvo.IsDisplay);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int64, ParameterDirection.Output, null, DataRowVersion.Default, nvo.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (long) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }
    }
}

