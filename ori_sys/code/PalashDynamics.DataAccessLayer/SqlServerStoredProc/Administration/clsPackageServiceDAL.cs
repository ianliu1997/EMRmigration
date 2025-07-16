using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.Web.Logging;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PalashDynamics.ValueObjects;
using System.Data.Common;
using System.Data;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{

    public class clsPackageServiceDAL : clsBasePackageServiceDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsPackageServiceDAL()
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

        #region Package Services
        public override IValueObject GetAllPackageServices(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {
            clsGetPackageServiceBizActionVO BizAction = (clsGetPackageServiceBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAllPackageServices");


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    if (BizAction.MasterList == null)
                    {
                        BizAction.MasterList = new List<MasterListItem>();
                    }

                    while (reader.Read())
                    {
                        BizAction.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString()));
                    }
                }
            }


            catch (Exception ex)
            {

                throw;
            }
            finally
            {

            }
            return BizAction;
        }

        public override IValueObject AddPackageServices(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPackageServiceBizActionVO BizActionObj = valueObject as clsAddPackageServiceBizActionVO;

            if (BizActionObj.Details.ID == 0)
                BizActionObj = AddPackage(BizActionObj, UserVo);
            else
                BizActionObj = UpdatePackage(BizActionObj, UserVo);


            return valueObject;
        }

        private clsAddPackageServiceBizActionVO AddPackage(clsAddPackageServiceBizActionVO BizActionObj, clsUserVO UserVo)
        {

            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                clsPackageServiceVO objPackageVO = BizActionObj.Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPackageService");

                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, objPackageVO.ServiceID);
                dbServer.AddInParameter(command, "Validity", DbType.String, objPackageVO.Validity);
                dbServer.AddInParameter(command, "PackageAmount", DbType.Double, objPackageVO.PackageAmount);
                dbServer.AddInParameter(command, "NoOfFollowUp", DbType.String, objPackageVO.NoOfFollowUp);


                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPackageVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Details.ID = (long)dbServer.GetParameterValue(command, "ID");

                if (objPackageVO.PackageDetails != null && objPackageVO.PackageDetails.Count != 0)
                {
                    foreach (var item in objPackageVO.PackageDetails)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPackageServiceDetails");

                        dbServer.AddInParameter(command1, "PackageID", DbType.Int64, objPackageVO.ID);
                        dbServer.AddInParameter(command1, "DepartmentID", DbType.Int64, item.DepartmentID);
                        dbServer.AddInParameter(command1, "ServiceID", DbType.Int64, item.ServiceID);
                        dbServer.AddInParameter(command1, "Rate", DbType.Double, item.Rate);
                        dbServer.AddInParameter(command1, "ConcessionPercentage", DbType.Double, item.ConcessionPercentage);
                        dbServer.AddInParameter(command1, "ConcessionAmount", DbType.Double, item.ConcessionAmount);
                        dbServer.AddInParameter(command1, "NetAmount", DbType.Double, item.NetAmount);
                        dbServer.AddInParameter(command1, "FreeAtFollowUp", DbType.Boolean, item.FreeAtFollowUp);
                        dbServer.AddInParameter(command1, "IsActive", DbType.Boolean, item.IsActive);
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);

                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                        item.ID = (long)dbServer.GetParameterValue(command1, "ID");

                    }
                }
                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.Details = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return BizActionObj;

        }
        private clsAddPackageServiceBizActionVO UpdatePackage(clsAddPackageServiceBizActionVO BizActionObj, clsUserVO UserVo)
        {

            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                clsPackageServiceVO objPackageVO = BizActionObj.Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdatePackageService");

                dbServer.AddInParameter(command, "ID", DbType.Int64, objPackageVO.ID);
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, objPackageVO.ServiceID);
                dbServer.AddInParameter(command, "Validity", DbType.String, objPackageVO.Validity);
                dbServer.AddInParameter(command, "PackageAmount", DbType.Double, objPackageVO.PackageAmount);
                dbServer.AddInParameter(command, "NoOfFollowUp", DbType.String, objPackageVO.NoOfFollowUp);


                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);

                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                if (objPackageVO.PackageDetails != null && objPackageVO.PackageDetails.Count != 0)
                {
                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_DeletePackageService");
                    dbServer.AddInParameter(command2, "PackageID", DbType.Int64, objPackageVO.ID);
                    int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                }

                if (objPackageVO.PackageDetails != null && objPackageVO.PackageDetails.Count != 0)
                {
                    foreach (var item in objPackageVO.PackageDetails)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPackageServiceDetails");

                        dbServer.AddInParameter(command1, "PackageID", DbType.Int64, objPackageVO.ID);
                        dbServer.AddInParameter(command1, "DepartmentID", DbType.Int64, item.DepartmentID);
                        dbServer.AddInParameter(command1, "ServiceID", DbType.Int64, item.ServiceID);
                        dbServer.AddInParameter(command1, "Rate", DbType.Double, item.Rate);
                        dbServer.AddInParameter(command1, "ConcessionPercentage", DbType.Double, item.ConcessionPercentage);
                        dbServer.AddInParameter(command1, "ConcessionAmount", DbType.Double, item.ConcessionAmount);
                        dbServer.AddInParameter(command1, "NetAmount", DbType.Double, item.NetAmount);
                        dbServer.AddInParameter(command1, "FreeAtFollowUp", DbType.Boolean, item.FreeAtFollowUp);
                        dbServer.AddInParameter(command1, "IsActive", DbType.Boolean, item.IsActive);
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);

                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                        item.ID = (long)dbServer.GetParameterValue(command1, "ID");

                    }
                }
                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.Details = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return BizActionObj;

        }

        public override IValueObject GetPackageServiceList(IValueObject valueObject, clsUserVO UserVo)
        {

            clsGetPackageServiceListBizActionVO BizActionObj = valueObject as clsGetPackageServiceListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPackegeService");
                DbDataReader reader;


                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitId);
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceID);

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.PackageList == null)
                        BizActionObj.PackageList = new List<clsPackageServiceVO>();

                    while (reader.Read())
                    {
                        clsPackageServiceVO PackageVO = new clsPackageServiceVO();
                        PackageVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        PackageVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        PackageVO.ServiceID = (long)DALHelper.HandleDBNull(reader["ServiceID"]);
                        PackageVO.Service = (string)DALHelper.HandleDBNull(reader["ServiceName"]);
                        PackageVO.Validity = (string)DALHelper.HandleDBNull(reader["Validity"]);
                        PackageVO.PackageAmount = (double)DALHelper.HandleDBNull(reader["PackageAmount"]);
                        PackageVO.NoOfFollowUp = (string)DALHelper.HandleDBNull(reader["NoOfFollowUp"]);
                        PackageVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        BizActionObj.PackageList.Add(PackageVO);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetPackageServiceDetailList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPackageServiceDetailsListBizActionVO BizActionObj = valueObject as clsGetPackageServiceDetailsListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPackageServiceDetails");
                DbDataReader reader;


                dbServer.AddInParameter(command, "PackageID", DbType.Int64, BizActionObj.PackageID);


                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.PackageDetailList == null)
                        BizActionObj.PackageDetailList = new List<clsPackageServiceDetailsVO>();

                    while (reader.Read())
                    {
                        clsPackageServiceDetailsVO PackageVO = new clsPackageServiceDetailsVO();
                        PackageVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        PackageVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        PackageVO.PackageID = (long)DALHelper.HandleDBNull(reader["PackageID"]);
                        PackageVO.DepartmentID = (long)DALHelper.HandleDBNull(reader["DepartmentID"]);
                        PackageVO.Department = (string)DALHelper.HandleDBNull(reader["Department"]);
                        PackageVO.ServiceID = (long)DALHelper.HandleDBNull(reader["ServiceID"]);
                        PackageVO.ServiceName = (string)DALHelper.HandleDBNull(reader["ServiceName"]);
                        PackageVO.ServiceCode = (string)DALHelper.HandleDBNull(reader["ServiceCode"]);
                        PackageVO.Rate = (double)DALHelper.HandleDBNull(reader["Rate"]);
                        PackageVO.ConcessionPercentage = (double)DALHelper.HandleDBNull(reader["ConcessionPercentage"]);
                        PackageVO.ConcessionAmount = (double)DALHelper.HandleDBNull(reader["ConcessionAmount"]);
                        PackageVO.NetAmount = (double)DALHelper.HandleDBNull(reader["NetAmount"]);
                        PackageVO.FreeAtFollowUp = (bool)DALHelper.HandleDBNull(reader["FreeAtFollowUp"]);
                        PackageVO.IsActive = (bool)DALHelper.HandleDBNull(reader["IsActive"]);
                        PackageVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        BizActionObj.PackageDetailList.Add(PackageVO);
                    }
                }


                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;

        }

        public override IValueObject GetPackageServiceDetailListbyServiceID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPackageServiceFromServiceIDBizActionVO BizActionObj = valueObject as clsGetPackageServiceFromServiceIDBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPackageServiceFromServiceID");
                DbDataReader reader;


                dbServer.AddInParameter(command, "ServiceID ", DbType.Int64, BizActionObj.ServiceID);


                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.PackageDetailList == null)
                        BizActionObj.PackageDetailList = new List<clsPackageServiceDetailsVO>();

                    while (reader.Read())
                    {
                        clsPackageServiceDetailsVO PackageVO = new clsPackageServiceDetailsVO();
                        PackageVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        PackageVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        PackageVO.PackageID = (long)DALHelper.HandleDBNull(reader["PackageID"]);
                        PackageVO.DepartmentID = (long)DALHelper.HandleDBNull(reader["DepartmentID"]);
                        PackageVO.Department = (string)DALHelper.HandleDBNull(reader["Department"]);
                        PackageVO.ServiceID = (long)DALHelper.HandleDBNull(reader["ServiceID"]);
                        PackageVO.ServiceName = (string)DALHelper.HandleDBNull(reader["ServiceName"]);
                        PackageVO.ServiceCode = (string)DALHelper.HandleDBNull(reader["ServiceCode"]);
                        PackageVO.Rate = (double)DALHelper.HandleDBNull(reader["Rate"]);
                        PackageVO.ConcessionPercentage = (double)DALHelper.HandleDBNull(reader["ConcessionPercentage"]);
                        PackageVO.ConcessionAmount = (double)DALHelper.HandleDBNull(reader["ConcessionAmount"]);
                        PackageVO.NetAmount = (double)DALHelper.HandleDBNull(reader["NetAmount"]);
                        PackageVO.FreeAtFollowUp = (bool)DALHelper.HandleDBNull(reader["FreeAtFollowUp"]);
                        PackageVO.IsActive = (bool)DALHelper.HandleDBNull(reader["IsActive"]);
                        PackageVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        BizActionObj.PackageDetailList.Add(PackageVO);
                    }
                }


                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        #endregion

        #region For IPD Bill

        public override IValueObject GetPackageServiceForBill(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPackageServiceForBillBizActionVO BizActionObj = valueObject as clsGetPackageServiceForBillBizActionVO;
            try
            {
                DbDataReader reader;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPackageServiceForBill");
                dbServer.AddInParameter(command, "ServiceID ", DbType.Int64, BizActionObj.ServiceID);
                dbServer.AddInParameter(command, "TariffID ", DbType.Int64, BizActionObj.TariffID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.PackageDetailList == null)
                        BizActionObj.PackageDetailList = new List<clsPackageServiceDetailsVO>();
                    while (reader.Read())
                    {
                        clsPackageServiceDetailsVO PackageVO = new clsPackageServiceDetailsVO();
                        PackageVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        PackageVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        PackageVO.PackageID = (long)DALHelper.HandleDBNull(reader["PackageID"]);
                        PackageVO.DepartmentID = (long)DALHelper.HandleDBNull(reader["DepartmentID"]);
                        PackageVO.Department = (string)DALHelper.HandleDBNull(reader["Department"]);
                        PackageVO.ServiceID = (long)DALHelper.HandleDBNull(reader["ServiceID"]);
                        PackageVO.ServiceName = (string)DALHelper.HandleDBNull(reader["ServiceName"]);
                        PackageVO.ServiceCode = (string)DALHelper.HandleDBNull(reader["ServiceCode"]);
                        //     PackageVO.Rate = (double)DALHelper.HandleDBNull(reader["Rate"]);
                        //     PackageVO.ConcessionPercentage = (double)DALHelper.HandleDBNull(reader["ConcessionPercentage"]);
                        //     PackageVO.ConcessionAmount = (double)DALHelper.HandleDBNull(reader["ConcessionAmount"]);
                        //     PackageVO.NetAmount = (double)DALHelper.HandleDBNull(reader["NetAmount"]);
                        PackageVO.FreeAtFollowUp = (bool)DALHelper.HandleDBNull(reader["FreeAtFollowUp"]);
                        PackageVO.IsActive = (bool)DALHelper.HandleDBNull(reader["IsActive"]);
                        PackageVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        PackageVO.ServiceSpecilizationID = (long)DALHelper.HandleDBNull(reader["SpecializationId"]);
                        PackageVO.ServiceSubSpecilizationID = (long)DALHelper.HandleDBNull(reader["SubSpecializationId"]);
                        PackageVO.TariffServiceID = (long)DALHelper.HandleDBNull(reader["TariffServiceID"]);
                        PackageVO.TariffID = (long)DALHelper.HandleDBNull(reader["TariffID"]);
                        BizActionObj.PackageDetailList.Add(PackageVO);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        #endregion

    }
}
