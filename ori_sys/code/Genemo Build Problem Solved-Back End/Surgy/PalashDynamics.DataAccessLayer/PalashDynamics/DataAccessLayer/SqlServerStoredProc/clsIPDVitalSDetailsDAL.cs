namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration.StaffMaster;
    using PalashDynamics.ValueObjects.IPD;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsIPDVitalSDetailsDAL : clsBaseIPDVitalSDetailsDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsIPDVitalSDetailsDAL()
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
            }
        }

        public override IValueObject AddVitalSDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsIPDVitalSDetailsVO svo1 = new clsIPDVitalSDetailsVO();
            clsAddVitalSDetailsBizActionVO nvo = valueObject as clsAddVitalSDetailsBizActionVO;
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
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddVitalDetails");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "VisitAdmID", DbType.Int64, nvo.AddVitalDetails.VisitAdmID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitAdmUnitID", DbType.Int64, nvo.AddVitalDetails.VisitAdmUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd", DbType.Int16, nvo.AddVitalDetails.Opd_Ipd);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.AddVitalDetails.Date);
                this.dbServer.AddInParameter(storedProcCommand, "IsEncounter", DbType.Boolean, nvo.AddVitalDetails.IsEncounter);
                this.dbServer.AddInParameter(storedProcCommand, "TakenBy", DbType.Int64, nvo.AddVitalDetails.TakenBy);
                this.dbServer.AddInParameter(storedProcCommand, "Status ", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.UserGeneralDetailVO.AddedBy);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserGeneralDetailVO.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, objUserVO.UserGeneralDetailVO.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.AddVitalDetails.ID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                foreach (clsIPDVitalSDetailsVO svo in nvo.AddVitalDetailsList)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddVitalsDetails");
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "VitalsID", DbType.Int64, nvo.AddVitalDetails.ID);
                    this.dbServer.AddInParameter(command2, "VitalsUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "VitalSignID", DbType.Int64, svo.ID);
                    this.dbServer.AddInParameter(command2, "Value", DbType.Double, svo.DefaultValue);
                    this.dbServer.AddInParameter(command2, "Remark", DbType.String, svo.Remark);
                    this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, objUserVO.UserGeneralDetailVO.AddedBy);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, objUserVO.UserGeneralDetailVO.AddedOn);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, objUserVO.UserGeneralDetailVO.AddedDateTime);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                }
                transaction.Commit();
                nvo.SuccessStatus = 0;
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                transaction.Rollback();
                nvo = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject GetGraphDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetGraphDetailsBizActionVO nvo = valueObject as clsGetGraphDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetGraphDetails");
                this.dbServer.AddInParameter(storedProcCommand, "VisitAdmID", DbType.Int64, nvo.GetGraphDetails.VisitAdmID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitAdmUnitID", DbType.Int64, nvo.GetGraphDetails.VisitAdmUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "VitalSignID", DbType.Int64, nvo.GetGraphDetails.VitalSignID);
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.GetGraphDetails.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.GetGraphDetails.ToDate);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.GetGraphDetailsList == null)
                    {
                        nvo.GetGraphDetailsList = new List<clsIPDVitalSDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsIPDVitalSDetailsVO item = new clsIPDVitalSDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            VisitAdmID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitAdmID"])),
                            VisitAdmUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitAdmUnitID"])),
                            VitalsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VitalsID"])),
                            VitalsUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VitalsUnitID"])),
                            VitalSignID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VitalSignID"])),
                            Value = Convert.ToDouble(DALHelper.HandleDBNull(reader["Value"])),
                            Date = (DateTime?) DALHelper.HandleDBNull(reader["Date"])
                        };
                        nvo.GetGraphDetailsList.Add(item);
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

        public override IValueObject GetListofVitalDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetVitalSDetailsListBizActionVO nvo = valueObject as clsGetVitalSDetailsListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetListofVitalDetails");
                this.dbServer.AddInParameter(storedProcCommand, "VisitAdmID", DbType.Int64, nvo.GetVitalSDetails.VisitAdmID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitAdmUnitID", DbType.Int64, nvo.GetVitalSDetails.VisitAdmUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.GetVitalSDetails.Date);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.GetVitalSDetailsList == null)
                    {
                        nvo.GetVitalSDetailsList = new List<clsIPDVitalSDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsIPDVitalSDetailsVO item = new clsIPDVitalSDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Temperature = Convert.ToDouble(DALHelper.HandleDBNull(reader["Temperature"])),
                            Pulse = Convert.ToDouble(DALHelper.HandleDBNull(reader["Pulse"])),
                            BP_Sys = Convert.ToDouble(DALHelper.HandleDBNull(reader["BP_Sys"])),
                            BP_Dia = Convert.ToDouble(DALHelper.HandleDBNull(reader["BP_Dia"])),
                            Respiration = Convert.ToDouble(DALHelper.HandleDBNull(reader["Respiration"])),
                            Date = (DateTime?) DALHelper.HandleDBNull(reader["Date"]),
                            Time1 = Convert.ToString(DALHelper.HandleDBNull(reader["Time1"])),
                            TakenByName = Convert.ToString(DALHelper.HandleDBNull(reader["TakenByName"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.GetVitalSDetailsList.Add(item);
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

        public override IValueObject GetTPRDetailsListByAdmIDAdmUnitID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetVitalSDetailsListByAdmIDAdmUnitIDDateBizActionVO nvo = valueObject as clsGetVitalSDetailsListByAdmIDAdmUnitIDDateBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetTPRDetailsByAdmIdAndAdmUnitID");
                this.dbServer.AddInParameter(storedProcCommand, "AdmID", DbType.Int64, nvo.GetVitalSDetails.VisitAdmID);
                this.dbServer.AddInParameter(storedProcCommand, "AdmUnitID", DbType.Int64, nvo.GetVitalSDetails.VisitAdmUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.GetVitalSDetails.Date);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.GetVitalSDetails.InputPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.GetVitalSDetails.InputStartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.GetVitalSDetails.InputMaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.GetVitalSDetails.SortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.GetVitalSDetailsList == null)
                    {
                        nvo.GetVitalSDetailsList = new List<clsIPDVitalSDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsIPDVitalSDetailsVO item = new clsIPDVitalSDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            VisitAdmID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmID"])),
                            VisitAdmUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmUnitID"])),
                            Date = (DateTime?) DALHelper.HandleDBNull(reader["Date"]),
                            Time1 = Convert.ToString(DALHelper.HandleDBNull(reader["Time1"])),
                            Temperature = Convert.ToInt64(DALHelper.HandleDBNull(reader["Temperature"])),
                            Pulse = Convert.ToInt64(DALHelper.HandleDBNull(reader["Height"])),
                            Status = (bool) reader["Status"],
                            BP_Sys = Convert.ToInt64(DALHelper.HandleDBNull(reader["BP_Sys"])),
                            BP_Dia = Convert.ToInt64(DALHelper.HandleDBNull(reader["BP_Dia"])),
                            Respiration = Convert.ToInt64(DALHelper.HandleDBNull(reader["Respiration"])),
                            IsEncounter = (bool) DALHelper.HandleDBNull(reader["IsEncounter"])
                        };
                        nvo.GetVitalSDetailsList.Add(item);
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

        public override IValueObject GetUnitWiseEmpDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetUnitWiseEmpBizActionVO nvo = (clsGetUnitWiseEmpBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillStaffByUnitID");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                        nvo.StaffMasterList = new List<clsStaffMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsStaffMasterVO item = new clsStaffMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            FirstName = (string) DALHelper.HandleDBNull(reader["FirstName"]),
                            MiddleName = (string) DALHelper.HandleDBNull(reader["MiddleName"]),
                            LastName = (string) DALHelper.HandleDBNull(reader["LastName"]),
                            DOB = DALHelper.HandleDate(reader["DOB"]),
                            EmailId = reader["EmailId"].ToString()
                        };
                        nvo.StaffMasterList.Add(item);
                        nvo.MasterList.Add(new MasterListItem((long) reader["ID"], reader["Description"].ToString()));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetVitalsDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetVitalSDetailsListBizActionVO nvo = valueObject as clsGetVitalSDetailsListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetVitalsDetailList");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.GetVitalSDetailsList == null)
                    {
                        nvo.GetVitalSDetailsList = new List<clsIPDVitalSDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsIPDVitalSDetailsVO item = new clsIPDVitalSDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            DefaultValue = Convert.ToDouble(reader["DefaultValue"]),
                            MinValue = Convert.ToDouble(reader["MinValue"]),
                            MaxValue = Convert.ToDouble(reader["MaxValue"]),
                            Unit = Convert.ToString(DALHelper.HandleDBNull(reader["Unit"]))
                        };
                        nvo.GetVitalSDetailsList.Add(item);
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

        public override IValueObject UpdateStatusVitalDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateStatusVitalDetailsBizActionVO nvo = (clsUpdateStatusVitalDetailsBizActionVO) valueObject;
            try
            {
                clsIPDVitalSDetailsVO getVitalSDetails = nvo.GetVitalSDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("[CIMS_UpdateStatusVitalDetails]");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, getVitalSDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, getVitalSDetails.Status);
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

