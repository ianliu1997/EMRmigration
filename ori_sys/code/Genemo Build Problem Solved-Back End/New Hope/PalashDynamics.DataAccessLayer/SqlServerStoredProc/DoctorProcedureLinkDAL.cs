namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Billing;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class DoctorProcedureLinkDAL : DoctorProcedureLinkBaseDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private DoctorProcedureLinkDAL()
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

        public override IValueObject AddDoctorProcedureLink(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddDoctorProcedureLinkBizActionVO nvo = valueObject as clsAddDoctorProcedureLinkBizActionVO;
            try
            {
                clsDoctorProcedureLinkVO linkDetails = nvo.LinkDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddDoctorProcedureLinkHistory");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, linkDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, linkDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, linkDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ProcedureID", DbType.Int64, linkDetails.ProcedureID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, linkDetails.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "NurseID", DbType.Int64, linkDetails.NurseID);
                this.dbServer.AddInParameter(storedProcCommand, "SpecilazationID", DbType.Int64, linkDetails.SpecilazationID);
                this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.Int64, linkDetails.BillID);
                this.dbServer.AddInParameter(storedProcCommand, "BillUnitID", DbType.Int64, linkDetails.BillUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, linkDetails.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, linkDetails.Time);
                this.dbServer.AddInParameter(storedProcCommand, "BillDate", DbType.DateTime, linkDetails.BillDate);
                this.dbServer.AddInParameter(storedProcCommand, "BillNo", DbType.String, linkDetails.BillNo);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, linkDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, linkDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject DeleteDoctorProcedureLink(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteDoctorProcedureLinkBizActionVO nvo = valueObject as clsDeleteDoctorProcedureLinkBizActionVO;
            try
            {
                clsDoctorProcedureLinkVO linkDetails = nvo.LinkDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteDoctorProcedureLinkFile");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, linkDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, linkDetails.UnitID);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject GetDoctorProcedureLink(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDoctorProcedureLinkBizActionVO nvo = valueObject as clsGetDoctorProcedureLinkBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorProcedureLinkHistoryList");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.LinkDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.LinkDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.LinkDetailsList == null)
                    {
                        nvo.LinkDetailsList = new List<clsDoctorProcedureLinkVO>();
                    }
                    while (reader.Read())
                    {
                        clsDoctorProcedureLinkVO item = new clsDoctorProcedureLinkVO {
                            ID = (long) reader["ID"],
                            UnitID = (long) reader["UnitID"],
                            Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]))),
                            Time = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Time"]))),
                            BillDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["BillDate"]))),
                            BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"])),
                            ProcedureID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ProcedureID"])),
                            Procedure = Convert.ToString(DALHelper.HandleDBNull(reader["Proceduredone"])),
                            DoctorID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["DoctorID"])),
                            Doctor = Convert.ToString(DALHelper.HandleDBNull(reader["Doctor"])),
                            NurseID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["NurseID"])),
                            Nurse = Convert.ToString(DALHelper.HandleDBNull(reader["Nurse"])),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            SpecilazationID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["SpecilazationID"]))
                        };
                        nvo.LinkDetailsList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }
    }
}

