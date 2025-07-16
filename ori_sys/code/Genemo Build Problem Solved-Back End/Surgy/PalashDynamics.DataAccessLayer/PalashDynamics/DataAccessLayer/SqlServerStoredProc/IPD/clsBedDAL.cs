namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IPD
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.IPD;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsBedDAL : clsBaseBedDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsBedDAL()
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

        public override IValueObject AddBedUnderMaintenance(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddBedUnderMaintenanceBizActionVO bizActionObj = valueObject as clsAddBedUnderMaintenanceBizActionVO;
            bizActionObj = this.AddBedUnderMaintenanceDetails(bizActionObj, UserVo);
            return valueObject;
        }

        private clsAddBedUnderMaintenanceBizActionVO AddBedUnderMaintenanceDetails(clsAddBedUnderMaintenanceBizActionVO BizActionObj, clsUserVO UserVo)
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
                clsIPDBedUnderMaintenanceVO bedUnderMDetails = BizActionObj.BedUnderMDetails;
                foreach (clsIPDBedUnderMaintenanceVO evo2 in bedUnderMDetails.BedUnderMList)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddBedUnderMaintanenace");
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, bedUnderMDetails.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "BedID", DbType.Int64, evo2.BedID);
                    this.dbServer.AddInParameter(storedProcCommand, "BedUnitID", DbType.Int64, evo2.BedUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, bedUnderMDetails.Remark);
                    this.dbServer.AddInParameter(storedProcCommand, "ExpectedReleasedDate", DbType.DateTime, bedUnderMDetails.ExpectedReleasedDate);
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, bedUnderMDetails.FromDate);
                    this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, bedUnderMDetails.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, bedUnderMDetails.AddedBy);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, bedUnderMDetails.AddedWindowsLoginName);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, bedUnderMDetails.ID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    BizActionObj.BedUnderMDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                    DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_UpdateIPDBedIsUnderMaintenance");
                    this.dbServer.AddInParameter(command, "BedID", DbType.Int64, evo2.BedID);
                    this.dbServer.AddInParameter(command, "BedUnitID", DbType.Int64, evo2.BedUnitID);
                    this.dbServer.AddInParameter(command, "IsUnderMaintanence", DbType.Boolean, evo2.IsUnderMaintanence);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return BizActionObj;
        }

        public override IValueObject AddUpdateReleaseBedUnderMaintenance(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            clsAddUpdateReleaseBedUnderMaintenanceBizActionVO nvo = valueObject as clsAddUpdateReleaseBedUnderMaintenanceBizActionVO;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsIPDBedUnderMaintenanceVO bedUnderMDetails = nvo.BedUnderMDetails;
                foreach (clsIPDBedUnderMaintenanceVO evo2 in nvo.BedUnderMDetails.BedUnderMList)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateReleaseBedUnderMaintanenace");
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, evo2.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "BedID", DbType.Int64, evo2.BedID);
                    this.dbServer.AddInParameter(storedProcCommand, "BedUnitID", DbType.Int64, evo2.BedUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "ReleasedDate", DbType.DateTime, bedUnderMDetails.ReleasedDate);
                    this.dbServer.AddInParameter(storedProcCommand, "ReleaseRemark", DbType.String, evo2.ReleaseRemark);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_UpdateIPDBedIsUnderMaintenance");
                    this.dbServer.AddInParameter(command, "BedID", DbType.Int64, evo2.BedID);
                    this.dbServer.AddInParameter(command, "BedUnitID", DbType.Int64, evo2.BedUnitID);
                    this.dbServer.AddInParameter(command, "IsUnderMaintanence", DbType.Boolean, "False");
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject CheckFinalBillbyPatientID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIPDBedTransferListBizActionVO nvo = valueObject as clsGetIPDBedTransferListBizActionVO;
            try
            {
                DbCommand storedProcCommand;
                if (!nvo.IsCheckFinalBillByAdmID)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CheckFinalBillbyPatientID");
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CheckFinalBillbyAdmID");
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "AdmID", DbType.Int64, nvo.AdmID);
                    this.dbServer.AddInParameter(storedProcCommand, "AdmUnitID", DbType.Int64, nvo.AdmUnitID);
                }
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.BedList == null)
                    {
                        nvo.BedList = new List<clsIPDBedTransferVO>();
                    }
                    while (reader.Read())
                    {
                        clsIPDBedTransferVO rvo = new clsIPDBedTransferVO {
                            AdmID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            IsCancel = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCancel"])),
                            IsClosed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsClosed"])),
                            ISDischarged = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ISDischarged"])),
                            InterORFinal = Convert.ToBoolean(DALHelper.HandleDBNull(reader["InterORFinal"]))
                        };
                        nvo.BedDetails = rvo;
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

        public override IValueObject GetBillAndBedByAdmIDAndAdmUnitID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetBillAndBedByAdmIDAndAdmUnitIDBizActionVO nvo = valueObject as clsGetBillAndBedByAdmIDAndAdmUnitIDBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetBillAndBedByAdmIDAndAdmUnitID");
                this.dbServer.AddInParameter(storedProcCommand, "AdmID", DbType.Int64, nvo.DischargeDetails.AdmID);
                this.dbServer.AddInParameter(storedProcCommand, "AdmUnitID", DbType.Int64, nvo.DischargeDetails.AdmUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.IsFinalBillPrepare = (bool) DALHelper.HandleDBNull(reader["IsFinalBillPrepare"]);
                        nvo.IsBedRelease = (bool) DALHelper.HandleDBNull(reader["IsBedRelease"]);
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

        public override IValueObject GetIPDBedTransferDetailsForSelectedPatient(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetIPDBedTransferListBizActionVO nvo = valueObject as clsGetIPDBedTransferListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IPDPatientBedTransferDetails_New");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.PatientUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.BedList == null)
                    {
                        nvo.BedList = new List<clsIPDBedTransferVO>();
                    }
                    while (reader.Read())
                    {
                        clsIPDBedTransferVO item = new clsIPDBedTransferVO();
                        nvo.BedDetails = new clsIPDBedTransferVO();
                        item.PatientNameForTransfer = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"])).Trim();
                        item.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        item.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        item.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        item.IPDNo = Convert.ToString(DALHelper.HandleDBNull(reader["IPDNO"]));
                        item.IPDAdmissionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IPDAdmissionID"]));
                        item.TransferDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["TransferDate"])));
                        item.FromClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FromClassID"]));
                        item.FromClass = Convert.ToString(DALHelper.HandleDBNull(reader["FromClass"]));
                        item.FromWardID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FromWardID"]));
                        item.FromWard = Convert.ToString(DALHelper.HandleDBNull(reader["FromWard"]));
                        item.FromBedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FromBedID"]));
                        item.FromBed = Convert.ToString(DALHelper.HandleDBNull(reader["FromBed"]));
                        item.ToClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToClassID"]));
                        item.ToClass = Convert.ToString(DALHelper.HandleDBNull(reader["ToClass"]));
                        item.ToWardID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToWardID"]));
                        item.ToWard = Convert.ToString(DALHelper.HandleDBNull(reader["ToWard"]));
                        item.ToBedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToBedID"]));
                        item.ToBed = Convert.ToString(DALHelper.HandleDBNull(reader["ToBed"]));
                        item.BillingToBedCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillingToBedCategoryID"]));
                        item.FromDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["FromDate"])));
                        item.FromTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["FromTime"])));
                        item.ToDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["ToDate"])));
                        item.ToTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["ToTime"])));
                        nvo.BedDetails = item;
                        nvo.BedList.Add(item);
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

        public override IValueObject GetIPDBedTransferList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIPDBedTransferListBizActionVO nvo = valueObject as clsGetIPDBedTransferListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIPDBedTransferHistoryList");
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.BedDetails.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.BedDetails.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, nvo.BedDetails.FirstName);
                this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, nvo.BedDetails.MiddleName);
                this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, nvo.BedDetails.LastName);
                this.dbServer.AddInParameter(storedProcCommand, "FamilyName", DbType.String, nvo.BedDetails.FamilyName);
                this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, nvo.BedDetails.MRNo);
                this.dbServer.AddInParameter(storedProcCommand, "IPDNO", DbType.String, nvo.BedDetails.IPDNo);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.String, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "Id");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.BedList == null)
                    {
                        nvo.BedList = new List<clsIPDBedTransferVO>();
                    }
                    while (reader.Read())
                    {
                        clsIPDBedTransferVO item = new clsIPDBedTransferVO();
                        nvo.BedDetails = new clsIPDBedTransferVO();
                        item.FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]));
                        item.MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"]));
                        item.LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]));
                        item.FamilyName = Convert.ToString(DALHelper.HandleDBNull(reader["FamilyName"]));
                        item.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        item.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        item.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        item.IPDNo = Convert.ToString(DALHelper.HandleDBNull(reader["IPDNO"]));
                        item.IPDAdmissionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IPDAdmissionID"]));
                        item.TransferDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["TransferDate"])));
                        item.FromClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FromClassID"]));
                        item.FromClass = Convert.ToString(DALHelper.HandleDBNull(reader["FromClass"]));
                        item.FromWardID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FromWardID"]));
                        item.FromWard = Convert.ToString(DALHelper.HandleDBNull(reader["FromWard"]));
                        item.FromBedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FromBedID"]));
                        item.FromBed = Convert.ToString(DALHelper.HandleDBNull(reader["FromBed"]));
                        item.ToClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToClassID"]));
                        item.ToClass = Convert.ToString(DALHelper.HandleDBNull(reader["ToClass"]));
                        item.ToWardID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToWardID"]));
                        item.ToWard = Convert.ToString(DALHelper.HandleDBNull(reader["ToWard"]));
                        item.ToBedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToBedID"]));
                        item.ToBed = Convert.ToString(DALHelper.HandleDBNull(reader["ToBed"]));
                        item.FromDate = (DateTime?) DALHelper.HandleDBNull(reader["FromDate"]);
                        item.FromTime = (DateTime?) DALHelper.HandleDBNull(reader["FromTime"]);
                        item.ToDate = (DateTime?) DALHelper.HandleDBNull(reader["ToDate"]);
                        item.ToTime = (DateTime?) DALHelper.HandleDBNull(reader["ToTime"]);
                        item.Status = Convert.ToBoolean(reader["Status"]);
                        nvo.BedDetails = item;
                        nvo.BedList.Add(item);
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

        public override IValueObject GetIPDWardByClassID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIPDWardByClassIDBizActionVO nvo = valueObject as clsGetIPDWardByClassIDBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetWardByClassID");
                this.dbServer.AddInParameter(storedProcCommand, "ClassID", DbType.Int64, nvo.BedDetails.ClassID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.BedList == null)
                    {
                        nvo.BedList = new List<clsIPDBedTransferVO>();
                    }
                    while (reader.Read())
                    {
                        clsIPDBedTransferVO item = new clsIPDBedTransferVO {
                            WardID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WardID"])),
                            Ward = (string) DALHelper.HandleDBNull(reader["Ward"])
                        };
                        nvo.BedList.Add(item);
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

        public override IValueObject GetReleaseBedUnderMaintenanceList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetReleaseBedUnderMaintenanceListBizActionVO nvo = valueObject as clsGetReleaseBedUnderMaintenanceListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetReleaseBedUnderMaintenanceList");
                this.dbServer.AddInParameter(storedProcCommand, "BedID", DbType.Int64, nvo.BedUnderMDetails.BedID);
                this.dbServer.AddInParameter(storedProcCommand, "BedUnitID", DbType.Int64, nvo.BedUnderMDetails.BedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsUnderMaintanence", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "BedID");
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.BedUnderMDetails.sortExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.BedUnderMDetails.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.BedUnderMDetails.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.BedUnderMDetails.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.BedUnderMList == null)
                    {
                        nvo.BedUnderMList = new List<clsIPDBedUnderMaintenanceVO>();
                    }
                    while (reader.Read())
                    {
                        clsIPDBedUnderMaintenanceVO item = new clsIPDBedUnderMaintenanceVO();
                        nvo.BedUnderMDetails = new clsIPDBedUnderMaintenanceVO();
                        item.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        item.BedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedID"]));
                        item.BedUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedUnitID"]));
                        item.Bed = (string) DALHelper.HandleDBNull(reader["Bed"]);
                        item.Ward = (string) DALHelper.HandleDBNull(reader["Ward"]);
                        item.WardId = Convert.ToInt64(DALHelper.HandleDBNull(reader["WardId"]));
                        item.BedClass = Convert.ToString(DALHelper.HandleDBNull(reader["Class"]));
                        item.ReleaseRemark = "";
                        item.ReleasedDate = null;
                        item.IsUnderMaintanence = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsUnderMaintanence"]));
                        nvo.BedUnderMDetails = item;
                        nvo.BedUnderMList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.BedUnderMDetails.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }
    }
}

