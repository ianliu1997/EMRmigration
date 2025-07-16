using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Inventory;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using System.Data;
using com.seedhealthcare.hms.Web.ConfigurationManager;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Inventory
{
   public class clsCAGRNDAL : clsBaseCAGRNDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        public clsCAGRNDAL()
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
       public override IValueObject GetCAGRNSearchList(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();

            clsCAGRNBizActionVO BizActionObj = valueObject as clsCAGRNBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetGRNListForCA");
                DbDataReader reader;

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "SupplierID", DbType.Int64, BizActionObj.SupplierID);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.StoreID);
                
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "GRNNO", DbType.String, BizActionObj.GRNNO);

                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, BizActionObj.Freezed);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);


                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsCAGRNVO>();
                    while (reader.Read())
                    {
                        clsCAGRNVO objVO = new clsCAGRNVO();

                        objVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVO.StoreID = (long)DALHelper.HandleDBNull(reader["StoreID"]);
                        objVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        objVO.SupplierID = (long)DALHelper.HandleDBNull(reader["SupplierID"]);
                        objVO.GRNNO = (string)DALHelper.HandleDBNull(reader["GRNNO"]);
                        objVO.GRNType = (InventoryGRNType)((Int16)DALHelper.HandleDBNull(reader["GRNType"]));
                        objVO.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);

                        objVO.Freezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Freezed"]));
                        
                        BizActionObj.List.Add(objVO);

                    }

                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");

                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return BizActionObj;

        }

       public override IValueObject GetCAGRNItemDetailList(IValueObject valueObject, clsUserVO UserVo)
       {
           //throw new NotImplementedException();

           clsGetCAGRNItemDetailListBizActionVO BizActionObj = valueObject as clsGetCAGRNItemDetailListBizActionVO;
           try
           {
               DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCAGRNItemDetail");
               DbDataReader reader;

               dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
               dbServer.AddInParameter(command, "GRNID", DbType.Int64, BizActionObj.GRNID);
               reader = (DbDataReader)dbServer.ExecuteReader(command);

               if (reader.HasRows)
               {
                   if (BizActionObj.List == null)
                       BizActionObj.List = new List<clsCAGRNDetailsVO>();
                   while (reader.Read())
                   {
                       clsCAGRNDetailsVO objVO = new clsCAGRNDetailsVO();

                       objVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                       objVO.ItemID = (long)DALHelper.HandleDBNull(reader["ItemID"]);
                       objVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                       objVO.GRNUnitID = (long)DALHelper.HandleDBNull(reader["GRNUnitID"]);
                       objVO.ItemName = (string)DALHelper.HandleDBNull(reader["ItemName"]);
                       objVO.ItemCode = (string)DALHelper.HandleDBNull(reader["ItemCode"]);
                       BizActionObj.List.Add(objVO);

                   }

               }
               reader.NextResult();
               

               reader.Close();
           }
           catch (Exception ex)
           {
               throw;
           }
           finally
           {

           }
           return BizActionObj;

       }


       public override IValueObject AddCAGRNItem(IValueObject valueObject, clsUserVO UserVo)
       {
           //throw new NotImplementedException();

           clsAddCAGRNItemBizActionVO BizActionObj = valueObject as clsAddCAGRNItemBizActionVO;
           DbTransaction trans = null;
           DbConnection con = dbServer.CreateConnection();
           try
           {
               
               con.Open();
               trans = con.BeginTransaction();
               DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddCAGRNDetail");
               command.Connection = con;
               dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitId);
               dbServer.AddInParameter(command, "GRNUnitID", DbType.Int64, BizActionObj.GRNUnitId);
               dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.StoreId);
               dbServer.AddInParameter(command, "SupplierId", DbType.Int64, BizActionObj.SupplierID);
               dbServer.AddInParameter(command, "IsFinalised", DbType.Boolean, BizActionObj.Finalised);
               
               dbServer.AddInParameter(command, "ServiceAgent", DbType.String, BizActionObj.ServiceAgent);
               dbServer.AddInParameter(command, "ContractExpiryDate", DbType.DateTime, BizActionObj.ContracExpirytDate);
               dbServer.AddInParameter(command, "SerialNo", DbType.String, BizActionObj.SerialNumber);
               dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.StartDate);
               dbServer.AddInParameter(command, "EndDate", DbType.DateTime, BizActionObj.EndDate);
               dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
               dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
               dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
               dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
               dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
              
               dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
               dbServer.AddOutParameter(command, "Id", DbType.Int32, int.MaxValue);
               int intStatus = dbServer.ExecuteNonQuery(command);
               BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
               BizActionObj.ID = (int)dbServer.GetParameterValue(command, "Id");

               if (BizActionObj.SuccessStatus == 0)
               {
                   foreach (var itemGRN in BizActionObj.Details.ItemsCAGRN)
                   {
                       DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddCAGRNDetailItem");
                       command1.Connection = con;
                       dbServer.AddInParameter(command1, "ContractManagementId", DbType.Int64, BizActionObj.ID);
                       dbServer.AddInParameter(command1, "GRNUnitID", DbType.Int64, itemGRN.GRNUnitID);
                       dbServer.AddInParameter(command1, "ItemID", DbType.Int64, itemGRN.ItemID);
                       dbServer.AddInParameter(command1, "UnitID", DbType.Int64, BizActionObj.UnitId);
                       dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                       intStatus = dbServer.ExecuteNonQuery(command1);
                       BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");

                   }
               }
               
           }
           catch (Exception ex)
           {
               throw;
           }
           finally
           {
               if (con.State == ConnectionState.Open)
                   con.Close();
               con = null;
               trans = null;
           }
           return BizActionObj;

       }


       public override IValueObject GetCAList(IValueObject valueObject, clsUserVO UserVo)
       {
           //throw new NotImplementedException();


           DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCAListForSearch");
               clsGetCAListBizActionVO BizActionObj = valueObject as clsGetCAListBizActionVO;
            try
            {
                DbDataReader reader;

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "SupplierID", DbType.Int64, BizActionObj.SupplierID);
                if (BizActionObj.FromDate != null)
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);

                if (BizActionObj.ToDate != null)
                {


                    BizActionObj.ToDate = BizActionObj.ToDate.Value.Date.AddDays(1);


                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);

                }
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);


                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);


                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsCAGRNVO>();
                    while (reader.Read())
                    {
                        clsCAGRNVO objVO = new clsCAGRNVO();

                        objVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVO.SupplierName = (string)DALHelper.HandleDBNull(reader["SupplierName"]);
                        objVO.ServiceAgent = (String)DALHelper.HandleDBNull(reader["ServiceAgent"]);
                        objVO.ContractExpiryDate = (DateTime)DALHelper.HandleDate(reader["ContractExpiryDate"]);
                        objVO.FromDate = (DateTime)DALHelper.HandleDate(reader["FromDate"]);
                        objVO.EndDate = (DateTime)DALHelper.HandleDate(reader["EndDate"]);
                        objVO.SerialNo = (String)DALHelper.HandleDBNull(reader["SerialNo"]);
                        objVO.SupplierID = (long)DALHelper.HandleDBNull(reader["SupplierId"]);
                        objVO.StoreID = (long)DALHelper.HandleDBNull(reader["StoreId"]);
                        objVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitId"]);
                        objVO.Finalized = (Boolean)DALHelper.HandleBoolDBNull(reader["IsFinalised"]);
                        BizActionObj.List.Add(objVO);

                    }

                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");

                reader.Close();
           }
           catch (Exception ex)
           {
               throw;
           }
           finally
           {

           }
           return BizActionObj;

       }

       public override IValueObject GetCAItemDetailList(IValueObject valueObject, clsUserVO UserVo)
       {
           //throw new NotImplementedException();

           clsGetCAItemDetailListBizActionVO BizActionObj = valueObject as clsGetCAItemDetailListBizActionVO;
           try
           {
               DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCAItemDetail");
               DbDataReader reader;

               dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
               dbServer.AddInParameter(command, "ContractManagementId", DbType.Int64, BizActionObj.ContractManagementId);
               reader = (DbDataReader)dbServer.ExecuteReader(command);

               if (reader.HasRows)
               {
                   if (BizActionObj.List == null)
                       BizActionObj.List = new List<clsCAGRNDetailsVO>();
                   while (reader.Read())
                   {
                       clsCAGRNDetailsVO objVO = new clsCAGRNDetailsVO();

                       objVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                       objVO.ItemID = (long)DALHelper.HandleDBNull(reader["ItemID"]);
                       objVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                       objVO.GRNUnitID = (long)DALHelper.HandleDBNull(reader["GRNUnitID"]);
                       objVO.ItemName = (string)DALHelper.HandleDBNull(reader["ItemName"]);
                       objVO.ItemCode = (string)DALHelper.HandleDBNull(reader["ItemCode"]);
                       BizActionObj.List.Add(objVO);

                   }

               }
               reader.NextResult();


               reader.Close();
           }
           catch (Exception ex)
           {
               throw;
           }
           finally
           {

           }
           return BizActionObj;

       }

       public override IValueObject GetCAItemDetailListById(IValueObject valueObject, clsUserVO UserVo)
       {
           //throw new NotImplementedException();

           clsGetCADetailsListByIDBizActionVO BizActionObj = valueObject as clsGetCADetailsListByIDBizActionVO;
           try
           {
               DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCAItemDetail");
               DbDataReader reader;

               dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
               dbServer.AddInParameter(command, "ContractManagementId", DbType.Int64, BizActionObj.ContractManagementId);
               reader = (DbDataReader)dbServer.ExecuteReader(command);

               if (reader.HasRows)
               {
                   if (BizActionObj.List == null)
                       BizActionObj.List = new List<clsCAGRNDetailsVO>();
                   while (reader.Read())
                   {
                       clsCAGRNDetailsVO objVO = new clsCAGRNDetailsVO();

                       objVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                       objVO.ItemID = (long)DALHelper.HandleDBNull(reader["ItemID"]);
                       objVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                       objVO.GRNUnitID = (long)DALHelper.HandleDBNull(reader["GRNUnitID"]);
                       objVO.ItemName = (string)DALHelper.HandleDBNull(reader["ItemName"]);
                       objVO.ItemCode = (string)DALHelper.HandleDBNull(reader["ItemCode"]);
                       BizActionObj.List.Add(objVO);

                   }

               }
               reader.NextResult();


               reader.Close();
           }
           catch (Exception ex)
           {
               throw;
           }
           finally
           {

           }
           return BizActionObj;

       }


       public override IValueObject DeleteCAItemById(IValueObject valueObject, clsUserVO UserVo)
       {
           //throw new NotImplementedException();

           clsDeleteCAByIDBizActionVO BizActionObj = valueObject as clsDeleteCAByIDBizActionVO;
           DbTransaction trans = null;
           DbConnection con = dbServer.CreateConnection();
           try
           {

               con.Open();
               trans = con.BeginTransaction();
               DbCommand command = dbServer.GetStoredProcCommand("CIMS_DeleteCAItemDetail");
               command.Connection = con;
               dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
               dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitId);
               dbServer.AddInParameter(command, "ContractManagementId", DbType.Int64, BizActionObj.ContractManagementId);

               dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
               dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
               dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
               dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

               int intStatus = dbServer.ExecuteNonQuery(command);
            //   BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
              
           }
           catch (Exception ex)
           {
               throw;
           }
           finally
           {
               if (con.State == ConnectionState.Open)
                   con.Close();
               con = null;
               trans = null;
           }
           return BizActionObj;

       }

       public override IValueObject UpdateCAGRNItem(IValueObject valueObject, clsUserVO UserVo)
       {
           //throw new NotImplementedException();

           clsUpdateCAGRNItemBizActionVO BizActionObj = valueObject as clsUpdateCAGRNItemBizActionVO;
           DbTransaction trans = null;
           DbConnection con = dbServer.CreateConnection();
           try
           {

               con.Open();
               trans = con.BeginTransaction();
               DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateCAGRNDetail");
               command.Connection = con;
               dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
               dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitId);
               dbServer.AddInParameter(command, "GRNUnitID", DbType.Int64, BizActionObj.GRNUnitId);
               dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.StoreId);
               dbServer.AddInParameter(command, "SupplierId", DbType.Int64, BizActionObj.SupplierID);
               dbServer.AddInParameter(command, "IsFinalised", DbType.Boolean, BizActionObj.Finalised);

               dbServer.AddInParameter(command, "ServiceAgent", DbType.String, BizActionObj.ServiceAgent);
               dbServer.AddInParameter(command, "ContractExpiryDate", DbType.DateTime, BizActionObj.ContracExpirytDate);
               dbServer.AddInParameter(command, "SerialNo", DbType.String, BizActionObj.SerialNumber);
               dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.StartDate);
               dbServer.AddInParameter(command, "EndDate", DbType.DateTime, BizActionObj.EndDate);
               dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
               dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
               dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
               dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
               dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

               dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
               
               int intStatus = dbServer.ExecuteNonQuery(command);
               BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");


               if (BizActionObj.SuccessStatus == 0)
               {
                   foreach (var itemGRN in BizActionObj.Details.ItemsCAGRN)
                   {
                       DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddCAGRNDetailItem");
                       command1.Connection = con;
                       dbServer.AddInParameter(command1, "ContractManagementId", DbType.Int64, BizActionObj.ID);
                       dbServer.AddInParameter(command1, "GRNUnitID", DbType.Int64, itemGRN.GRNUnitID);
                       dbServer.AddInParameter(command1, "ItemID", DbType.Int64, itemGRN.ItemID);
                       dbServer.AddInParameter(command1, "UnitID", DbType.Int64, BizActionObj.UnitId);
                       dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                       intStatus = dbServer.ExecuteNonQuery(command1);
                       BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");

                   }
               }

           }
           catch (Exception ex)
           {
               throw;
           }
           finally
           {
               if (con.State == ConnectionState.Open)
                   con.Close();
               con = null;
               trans = null;
           }
           return BizActionObj;

       }
    }
}
