namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.ValueObjects;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsPackageServiceDAL : clsBasePackageServiceDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsPackageServiceDAL()
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

        private clsAddPackageServiceBizActionVO AddPackage(clsAddPackageServiceBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                clsPackageServiceVO details = BizActionObj.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPackageService");
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, details.ServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "Validity", DbType.String, details.Validity);
                this.dbServer.AddInParameter(storedProcCommand, "PackageAmount", DbType.Double, details.PackageAmount);
                this.dbServer.AddInParameter(storedProcCommand, "NoOfFollowUp", DbType.String, details.NoOfFollowUp);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, details.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if ((details.PackageDetails != null) && (details.PackageDetails.Count != 0))
                {
                    foreach (clsPackageServiceDetailsVO svo in details.PackageDetails)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddPackageServiceDetails");
                        this.dbServer.AddInParameter(command2, "PackageID", DbType.Int64, details.ID);
                        this.dbServer.AddInParameter(command2, "DepartmentID", DbType.Int64, svo.DepartmentID);
                        this.dbServer.AddInParameter(command2, "ServiceID", DbType.Int64, svo.ServiceID);
                        this.dbServer.AddInParameter(command2, "Rate", DbType.Double, svo.Rate);
                        this.dbServer.AddInParameter(command2, "ConcessionPercentage", DbType.Double, svo.ConcessionPercentage);
                        this.dbServer.AddInParameter(command2, "ConcessionAmount", DbType.Double, svo.ConcessionAmount);
                        this.dbServer.AddInParameter(command2, "NetAmount", DbType.Double, svo.NetAmount);
                        this.dbServer.AddInParameter(command2, "FreeAtFollowUp", DbType.Boolean, svo.FreeAtFollowUp);
                        this.dbServer.AddInParameter(command2, "IsActive", DbType.Boolean, svo.IsActive);
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                        this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                        this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command2, transaction);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                        svo.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                    }
                }
                transaction.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.Details = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return BizActionObj;
        }

        public override IValueObject AddPackageServices(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPackageServiceBizActionVO bizActionObj = valueObject as clsAddPackageServiceBizActionVO;
            bizActionObj = (bizActionObj.Details.ID != 0L) ? this.UpdatePackage(bizActionObj, UserVo) : this.AddPackage(bizActionObj, UserVo);
            return valueObject;
        }

        public override IValueObject GetAllPackageServices(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPackageServiceBizActionVO nvo = (clsGetPackageServiceBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAllPackageServices");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString()));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPackageServiceDetailList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPackageServiceDetailsListBizActionVO nvo = valueObject as clsGetPackageServiceDetailsListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPackageServiceDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int64, nvo.PackageID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PackageDetailList == null)
                    {
                        nvo.PackageDetailList = new List<clsPackageServiceDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsPackageServiceDetailsVO item = new clsPackageServiceDetailsVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            PackageID = (long) DALHelper.HandleDBNull(reader["PackageID"]),
                            DepartmentID = (long) DALHelper.HandleDBNull(reader["DepartmentID"]),
                            Department = (string) DALHelper.HandleDBNull(reader["Department"]),
                            ServiceID = (long) DALHelper.HandleDBNull(reader["ServiceID"]),
                            ServiceName = (string) DALHelper.HandleDBNull(reader["ServiceName"]),
                            ServiceCode = (string) DALHelper.HandleDBNull(reader["ServiceCode"]),
                            Rate = (double) DALHelper.HandleDBNull(reader["Rate"]),
                            ConcessionPercentage = (double) DALHelper.HandleDBNull(reader["ConcessionPercentage"]),
                            ConcessionAmount = (double) DALHelper.HandleDBNull(reader["ConcessionAmount"]),
                            NetAmount = (double) DALHelper.HandleDBNull(reader["NetAmount"]),
                            FreeAtFollowUp = (bool) DALHelper.HandleDBNull(reader["FreeAtFollowUp"]),
                            IsActive = (bool) DALHelper.HandleDBNull(reader["IsActive"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.PackageDetailList.Add(item);
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

        public override IValueObject GetPackageServiceDetailListbyServiceID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPackageServiceFromServiceIDBizActionVO nvo = valueObject as clsGetPackageServiceFromServiceIDBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPackageServiceFromServiceID");
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID ", DbType.Int64, nvo.ServiceID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PackageDetailList == null)
                    {
                        nvo.PackageDetailList = new List<clsPackageServiceDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsPackageServiceDetailsVO item = new clsPackageServiceDetailsVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            PackageID = (long) DALHelper.HandleDBNull(reader["PackageID"]),
                            DepartmentID = (long) DALHelper.HandleDBNull(reader["DepartmentID"]),
                            Department = (string) DALHelper.HandleDBNull(reader["Department"]),
                            ServiceID = (long) DALHelper.HandleDBNull(reader["ServiceID"]),
                            ServiceName = (string) DALHelper.HandleDBNull(reader["ServiceName"]),
                            ServiceCode = (string) DALHelper.HandleDBNull(reader["ServiceCode"]),
                            Rate = (double) DALHelper.HandleDBNull(reader["Rate"]),
                            ConcessionPercentage = (double) DALHelper.HandleDBNull(reader["ConcessionPercentage"]),
                            ConcessionAmount = (double) DALHelper.HandleDBNull(reader["ConcessionAmount"]),
                            NetAmount = (double) DALHelper.HandleDBNull(reader["NetAmount"]),
                            FreeAtFollowUp = (bool) DALHelper.HandleDBNull(reader["FreeAtFollowUp"]),
                            IsActive = (bool) DALHelper.HandleDBNull(reader["IsActive"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.PackageDetailList.Add(item);
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

        public override IValueObject GetPackageServiceForBill(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPackageServiceForBillBizActionVO nvo = valueObject as clsGetPackageServiceForBillBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPackageServiceForBill");
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID ", DbType.Int64, nvo.ServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "TariffID ", DbType.Int64, nvo.TariffID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PackageDetailList == null)
                    {
                        nvo.PackageDetailList = new List<clsPackageServiceDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsPackageServiceDetailsVO item = new clsPackageServiceDetailsVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            PackageID = (long) DALHelper.HandleDBNull(reader["PackageID"]),
                            DepartmentID = (long) DALHelper.HandleDBNull(reader["DepartmentID"]),
                            Department = (string) DALHelper.HandleDBNull(reader["Department"]),
                            ServiceID = (long) DALHelper.HandleDBNull(reader["ServiceID"]),
                            ServiceName = (string) DALHelper.HandleDBNull(reader["ServiceName"]),
                            ServiceCode = (string) DALHelper.HandleDBNull(reader["ServiceCode"]),
                            FreeAtFollowUp = (bool) DALHelper.HandleDBNull(reader["FreeAtFollowUp"]),
                            IsActive = (bool) DALHelper.HandleDBNull(reader["IsActive"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            ServiceSpecilizationID = (long) DALHelper.HandleDBNull(reader["SpecializationId"]),
                            ServiceSubSpecilizationID = (long) DALHelper.HandleDBNull(reader["SubSpecializationId"]),
                            TariffServiceID = (long) DALHelper.HandleDBNull(reader["TariffServiceID"]),
                            TariffID = (long) DALHelper.HandleDBNull(reader["TariffID"])
                        };
                        nvo.PackageDetailList.Add(item);
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

        public override IValueObject GetPackageServiceList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPackageServiceListBizActionVO nvo = valueObject as clsGetPackageServiceListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPackegeService");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PackageList == null)
                    {
                        nvo.PackageList = new List<clsPackageServiceVO>();
                    }
                    while (reader.Read())
                    {
                        clsPackageServiceVO item = new clsPackageServiceVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            ServiceID = (long) DALHelper.HandleDBNull(reader["ServiceID"]),
                            Service = (string) DALHelper.HandleDBNull(reader["ServiceName"]),
                            Validity = (string) DALHelper.HandleDBNull(reader["Validity"]),
                            PackageAmount = (double) DALHelper.HandleDBNull(reader["PackageAmount"]),
                            NoOfFollowUp = (string) DALHelper.HandleDBNull(reader["NoOfFollowUp"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.PackageList.Add(item);
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

        private clsAddPackageServiceBizActionVO UpdatePackage(clsAddPackageServiceBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                clsPackageServiceVO details = BizActionObj.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePackageService");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, details.ID);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, details.ServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "Validity", DbType.String, details.Validity);
                this.dbServer.AddInParameter(storedProcCommand, "PackageAmount", DbType.Double, details.PackageAmount);
                this.dbServer.AddInParameter(storedProcCommand, "NoOfFollowUp", DbType.String, details.NoOfFollowUp);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if ((details.PackageDetails != null) && (details.PackageDetails.Count != 0))
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeletePackageService");
                    this.dbServer.AddInParameter(command2, "PackageID", DbType.Int64, details.ID);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                }
                if ((details.PackageDetails != null) && (details.PackageDetails.Count != 0))
                {
                    foreach (clsPackageServiceDetailsVO svo in details.PackageDetails)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddPackageServiceDetails");
                        this.dbServer.AddInParameter(command3, "PackageID", DbType.Int64, details.ID);
                        this.dbServer.AddInParameter(command3, "DepartmentID", DbType.Int64, svo.DepartmentID);
                        this.dbServer.AddInParameter(command3, "ServiceID", DbType.Int64, svo.ServiceID);
                        this.dbServer.AddInParameter(command3, "Rate", DbType.Double, svo.Rate);
                        this.dbServer.AddInParameter(command3, "ConcessionPercentage", DbType.Double, svo.ConcessionPercentage);
                        this.dbServer.AddInParameter(command3, "ConcessionAmount", DbType.Double, svo.ConcessionAmount);
                        this.dbServer.AddInParameter(command3, "NetAmount", DbType.Double, svo.NetAmount);
                        this.dbServer.AddInParameter(command3, "FreeAtFollowUp", DbType.Boolean, svo.FreeAtFollowUp);
                        this.dbServer.AddInParameter(command3, "IsActive", DbType.Boolean, svo.IsActive);
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                        this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                        this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
                        svo.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                    }
                }
                transaction.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.Details = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return BizActionObj;
        }
    }
}

