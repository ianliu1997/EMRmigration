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

    internal class clsIPDDAL : clsBaseIPDDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsIPDDAL()
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

        private clsAddIPDBedTransferBizActionVO AddBedDetails(clsAddIPDBedTransferBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                DbCommand storedProcCommand;
                clsIPDBedTransferVO bedDetails = BizActionObj.BedDetails;
                if (!BizActionObj.IsTransfer)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddIPDBedTransfer");
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, bedDetails.PatientUnitID);
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddIPDBedTransferFromBedTransferForm");
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, bedDetails.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "OldBedCategoryID", DbType.Int64, bedDetails.BedCategoryID);
                    this.dbServer.AddInParameter(storedProcCommand, "OldWardId", DbType.Int64, bedDetails.WardID);
                    this.dbServer.AddInParameter(storedProcCommand, "OldBedID", DbType.Int64, bedDetails.BedID);
                    this.dbServer.AddInParameter(storedProcCommand, "FromTime", DbType.DateTime, bedDetails.FromTime);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                }
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, bedDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "IPDAdmissionID", DbType.Int64, bedDetails.IPDAdmissionID);
                this.dbServer.AddInParameter(storedProcCommand, "IPDAdmissionNo", DbType.String, bedDetails.IPDAdmissionNo);
                this.dbServer.AddInParameter(storedProcCommand, "TransferDate", DbType.String, DateTime.Now);
                if (!BizActionObj.IsTransfer)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "BedCategoryID", DbType.Int64, bedDetails.BedCategoryID);
                    this.dbServer.AddInParameter(storedProcCommand, "WardID", DbType.Int64, bedDetails.WardID);
                    this.dbServer.AddInParameter(storedProcCommand, "BedID", DbType.Int64, bedDetails.BedID);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "BedCategoryID", DbType.Int64, bedDetails.ToClassID);
                    this.dbServer.AddInParameter(storedProcCommand, "WardID", DbType.Int64, bedDetails.ToWardID);
                    this.dbServer.AddInParameter(storedProcCommand, "BedID", DbType.Int64, bedDetails.ToBedID);
                    this.dbServer.AddInParameter(storedProcCommand, "BillingToBedCategoryID", DbType.Int64, bedDetails.BillingToBedCategoryID);
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, bedDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.BedDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        private clsAddIPDDischargeBizActionVO AddDischargeDetails(clsAddIPDDischargeBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsIPDDischargeVO dischargeDetails = BizActionObj.DischargeDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddIPDPatientDischarge");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, dischargeDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, dischargeDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IPDAdmissionID", DbType.Int64, dischargeDetails.IPDAdmissionID);
                this.dbServer.AddInParameter(storedProcCommand, "IPDAdmissionNo", DbType.String, dischargeDetails.IPDAdmissionNo);
                this.dbServer.AddInParameter(storedProcCommand, "DischargeDate", DbType.DateTime, dischargeDetails.DischargeDate);
                this.dbServer.AddInParameter(storedProcCommand, "DischargeTime", DbType.DateTime, dischargeDetails.DischargeTime);
                this.dbServer.AddInParameter(storedProcCommand, "DischargeDoctor", DbType.Int64, dischargeDetails.DischargeDoctor);
                this.dbServer.AddInParameter(storedProcCommand, "DischargeType", DbType.Int64, dischargeDetails.DischargeType);
                this.dbServer.AddInParameter(storedProcCommand, "DischargeDestination", DbType.Int64, dischargeDetails.DischargeDestination);
                this.dbServer.AddInParameter(storedProcCommand, "IsDeathDischarge", DbType.Boolean, dischargeDetails.IsDeathdischarge);
                this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, dischargeDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.DischargeDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject AddIPDBedTransfer(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddIPDBedTransferBizActionVO bizActionObj = valueObject as clsAddIPDBedTransferBizActionVO;
            bizActionObj = (bizActionObj.BedDetails.ID != 0L) ? this.UpdateBedDetails(bizActionObj, UserVo) : this.AddBedDetails(bizActionObj, UserVo);
            return valueObject;
        }

        public override IValueObject AddIPDDischarge(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddIPDDischargeBizActionVO bizActionObj = valueObject as clsAddIPDDischargeBizActionVO;
            bizActionObj = (bizActionObj.DischargeDetails.ID != 0L) ? this.UpdateDischargeDetails(bizActionObj, UserVo) : this.AddDischargeDetails(bizActionObj, UserVo);
            return valueObject;
        }

        public override IValueObject AddMultipleBed(IValueObject valueObject, clsUserVO UserVO)
        {
            clsAddIPDBedTransferBizActionVO nvo = valueObject as clsAddIPDBedTransferBizActionVO;
            try
            {
                clsIPDBedTransferVO bedDetails = nvo.BedDetails;
                List<clsIPDBedTransferVO> bedList = nvo.BedList;
                foreach (clsIPDBedTransferVO rvo in nvo.BedList)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddMultipleIPDBed");
                    this.dbServer.AddInParameter(storedProcCommand, "PatientId", DbType.Int64, rvo.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, rvo.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "IPDAdmissionID", DbType.Int64, rvo.IPDAdmissionID);
                    this.dbServer.AddInParameter(storedProcCommand, "TransferDate", DbType.DateTime, rvo.TransferDate);
                    this.dbServer.AddInParameter(storedProcCommand, "ToBedCategoryID", DbType.Int64, rvo.BedCategoryID);
                    this.dbServer.AddInParameter(storedProcCommand, "ToWardID", DbType.Int64, rvo.WardID);
                    this.dbServer.AddInParameter(storedProcCommand, "ToBedID", DbType.Int64, rvo.BedID);
                    this.dbServer.AddInParameter(storedProcCommand, "IPDAdmissionNO", DbType.String, rvo.IPDAdmissionNo);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVO.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(storedProcCommand, "BillingToBedCategoryID", DbType.Int64, rvo.BillingToBedCategoryID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetDischargeStatusDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDischargeStatusBizActionVO nvo = valueObject as clsGetDischargeStatusBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDischargeStatusDetails");
                this.dbServer.AddInParameter(storedProcCommand, "AdmissionID", DbType.Int64, nvo.DischargeDetails.AdmID);
                this.dbServer.AddInParameter(storedProcCommand, "AdmissionUnitID ", DbType.Int64, nvo.DischargeDetails.AdmUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DischargeList == null)
                    {
                        nvo.DischargeList = new List<clsIPDDischargeVO>();
                    }
                    while (reader.Read())
                    {
                        clsIPDDischargeVO item = new clsIPDDischargeVO {
                            rownum = Convert.ToInt64(DALHelper.HandleDBNull(reader["rownum"])),
                            AdmID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmID"])),
                            AdmUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmUnitID"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"])),
                            DepartmentName = Convert.ToString(DALHelper.HandleDBNull(reader["Department"])),
                            AdviseAuthorityName = Convert.ToString(DALHelper.HandleDBNull(reader["AdviseAuthorityName"])),
                            Remark = Convert.ToString(DALHelper.HandleDBNull(reader["remark"]))
                        };
                        nvo.DischargeList.Add(item);
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

        public override IValueObject GetIPDBedTransfer(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIPDBedTransferBizActionVO nvo = valueObject as clsGetIPDBedTransferBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIPDBedtransferDetails");
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, nvo.FirstName);
                this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, nvo.LastName);
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
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
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            UnitName = (string) DALHelper.HandleDBNull(reader["Clinic"]),
                            PatientID = (long) DALHelper.HandleDBNull(reader["PatientID"]),
                            PatientUnitID = (long) DALHelper.HandleDBNull(reader["PatientUnitID"]),
                            IPDAdmissionID = (long) DALHelper.HandleDBNull(reader["IPDAdmissionID"]),
                            IPDAdmissionNo = (string) DALHelper.HandleDBNull(reader["IPDAdmissionNo"]),
                            TransferDate = new DateTime?((DateTime) DALHelper.HandleDBNull(reader["TransferDate"])),
                            BedCategoryID = (long) DALHelper.HandleDBNull(reader["BedCategoryID"]),
                            BedCategory = (string) DALHelper.HandleDBNull(reader["BedCategory"]),
                            WardID = (long) DALHelper.HandleDBNull(reader["WardID"]),
                            Ward = (string) DALHelper.HandleDBNull(reader["Ward"]),
                            BedID = (long) DALHelper.HandleDBNull(reader["BedID"]),
                            BedNo = (string) DALHelper.HandleDBNull(reader["BedNo"]),
                            MRNo = (string) DALHelper.HandleDBNull(reader["MRNo"]),
                            FirstName = (string) DALHelper.HandleDBNull(reader["FirstName"]),
                            MiddleName = (string) DALHelper.HandleDBNull(reader["MiddleName"]),
                            LastName = (string) DALHelper.HandleDBNull(reader["LastName"]),
                            AdmissionDate = new DateTime?((DateTime) DALHelper.HandleDBNull(reader["AdmissionDate"])),
                            DateOfBirth = (DateTime) DALHelper.HandleDBNull(reader["DateOfBirth"]),
                            GenderID = (long) DALHelper.HandleDBNull(reader["GenderID"])
                        };
                        nvo.BedList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetIPDDischargeDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIPDDischargeBizActionVO nvo = valueObject as clsGetIPDDischargeBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIPDDischargeDetails");
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, nvo.FirstName);
                this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, nvo.LastName);
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DischargeList == null)
                    {
                        nvo.DischargeList = new List<clsIPDDischargeVO>();
                    }
                    while (reader.Read())
                    {
                        clsIPDDischargeVO item = new clsIPDDischargeVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            PatientID = (long) DALHelper.HandleDBNull(reader["PatientID"]),
                            PatientUnitID = (long) DALHelper.HandleDBNull(reader["PatientUnitID"]),
                            IPDAdmissionID = (long) DALHelper.HandleDBNull(reader["IPDAdmissionID"]),
                            IPDAdmissionNo = (string) DALHelper.HandleDBNull(reader["IPDAdmissionNo"]),
                            DischargeDate = (DateTime) DALHelper.HandleDBNull(reader["DischargeDate"]),
                            DischargeTime = (DateTime) DALHelper.HandleDBNull(reader["DischargeTime"]),
                            DischargeDoctor = (long) DALHelper.HandleDBNull(reader["DischargeDoctor"]),
                            DischargeType = (long) DALHelper.HandleDBNull(reader["DischargeTypeID"]),
                            DischargeDestination = (long) DALHelper.HandleDBNull(reader["DischargeDestinationID"]),
                            DoctorName = (string) DALHelper.HandleDBNull(reader["DoctorName"]),
                            Type = (string) DALHelper.HandleDBNull(reader["DischargeType"]),
                            Destination = (string) DALHelper.HandleDBNull(reader["DischargeDestination"]),
                            MRNo = (string) DALHelper.HandleDBNull(reader["MRNo"]),
                            FirstName = (string) DALHelper.HandleDBNull(reader["FirstName"]),
                            MiddleName = (string) DALHelper.HandleDBNull(reader["MiddleName"]),
                            LastName = (string) DALHelper.HandleDBNull(reader["LastName"]),
                            DateOfBirth = (DateTime) DALHelper.HandleDBNull(reader["DateOfBirth"]),
                            GenderID = (long) DALHelper.HandleDBNull(reader["GenderID"])
                        };
                        nvo.DischargeList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetIPDPatientDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIPDPatientDetailsBizActionVO nvo = valueObject as clsGetIPDPatientDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientIPDDetails");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, nvo.MRNo);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientDetails == null)
                    {
                        nvo.PatientDetails = new clsIPDAdmissionVO();
                    }
                    while (reader.Read())
                    {
                        nvo.PatientDetails.PatientId = (long) DALHelper.HandleDBNull(reader["PatientId"]);
                        nvo.PatientDetails.PatientUnitID = (long) DALHelper.HandleDBNull(reader["PatientUnitID"]);
                        nvo.PatientDetails.UnitId = (long) DALHelper.HandleDBNull(reader["UnitId"]);
                        nvo.PatientDetails.ID = (long) DALHelper.HandleDBNull(reader["IPDAdmissionID"]);
                        nvo.PatientDetails.AdmissionNO = (string) DALHelper.HandleDBNull(reader["AdmissionNo"]);
                        nvo.PatientDetails.Date = new DateTime?((DateTime) DALHelper.HandleDBNull(reader["AdmissionDate"]));
                        nvo.PatientDetails.DepartmentID = (long) DALHelper.HandleDBNull(reader["DepartmentID"]);
                        nvo.PatientDetails.DoctorID = (long) DALHelper.HandleDBNull(reader["DoctorID"]);
                        nvo.PatientDetails.WardID = (long) DALHelper.HandleDBNull(reader["WardID"]);
                        nvo.PatientDetails.BedCategoryID = (long) DALHelper.HandleDBNull(reader["BedCategoryID"]);
                        nvo.PatientDetails.BedID = (long) DALHelper.HandleDBNull(reader["BedID"]);
                        nvo.PatientDetails.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
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

        private clsAddIPDBedTransferBizActionVO UpdateBedDetails(clsAddIPDBedTransferBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsIPDBedTransferVO bedDetails = BizActionObj.BedDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateIPDBedTransfer");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, bedDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, bedDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, bedDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IPDAdmissionID", DbType.Int64, bedDetails.IPDAdmissionID);
                this.dbServer.AddInParameter(storedProcCommand, "IPDAdmissionNo", DbType.String, bedDetails.IPDAdmissionNo);
                this.dbServer.AddInParameter(storedProcCommand, "TransferDate", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "BedCategoryID", DbType.Int64, bedDetails.BedCategoryID);
                this.dbServer.AddInParameter(storedProcCommand, "WardID", DbType.Int64, bedDetails.WardID);
                this.dbServer.AddInParameter(storedProcCommand, "BedID", DbType.Int64, bedDetails.BedID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        private clsAddIPDDischargeBizActionVO UpdateDischargeDetails(clsAddIPDDischargeBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsIPDDischargeVO dischargeDetails = BizActionObj.DischargeDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateIPDDischarge");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, dischargeDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, dischargeDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, dischargeDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IPDAdmissionID", DbType.Int64, dischargeDetails.IPDAdmissionID);
                this.dbServer.AddInParameter(storedProcCommand, "IPDAdmissionNo", DbType.String, dischargeDetails.IPDAdmissionNo);
                this.dbServer.AddInParameter(storedProcCommand, "DischargeDate", DbType.DateTime, dischargeDetails.DischargeDate);
                this.dbServer.AddInParameter(storedProcCommand, "DischargeTime", DbType.DateTime, dischargeDetails.DischargeTime);
                this.dbServer.AddInParameter(storedProcCommand, "DischargeDoctor", DbType.Int64, dischargeDetails.DischargeDoctor);
                this.dbServer.AddInParameter(storedProcCommand, "DischargeType", DbType.Int64, dischargeDetails.DischargeType);
                this.dbServer.AddInParameter(storedProcCommand, "DischargeDestination", DbType.Int64, dischargeDetails.DischargeDestination);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }
    }
}

